using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using Grille.Graphics.Isometric.Numerics;


namespace Grille.Graphics.Isometric;

public abstract unsafe class Swapchain : IDisposable
{
    public int ImageWidth { get; private set; }

    public int ImageHeight { get; private set; }

    public int ImageLength => ImageWidth * ImageHeight;

    public int Position { get; private set; }

    public int Count { get; }

    public abstract ARGBColor* ImageData { get; }

    public Swapchain(int count)
    {
        Count = count;
    }

    public void Next()
    {
        Position += 1;
    }

    public abstract void LockActive();

    public abstract void UnlockActive();

    public void ResizeImages(int width, int height)
    {
        if (ImageWidth == width && ImageHeight == height)
            return;

        ImageWidth = width;
        ImageHeight = height;

        OnResizeImages();
    }

    public abstract void OnResizeImages();

    public abstract void Dispose();
}

public abstract unsafe class Swapchain<T> : Swapchain where T : class
{
    MonitorHandle<T>? _handle;
    readonly object[] _locks;
    readonly T[] _items;

    ARGBColor* imageData;
    private bool disposedValue;

    public override ARGBColor* ImageData => imageData;

    public MonitorHandle<T> Image
    {
        get
        {
            if (_handle?.disposed == false)
                throw new InvalidOperationException("Last handle not disposed.");

            var index = GetIndex(Position - 1);
            return new MonitorHandle<T>(_items[index], _locks[index]);
        }
    }

    public Swapchain(int size) : base(size)
    {
        _items = new T[size];
        _locks = new object[size];
        for (int i = 0; i < size; i++)
        {
            _locks[i] = new object();
        }
    }

    public override sealed void OnResizeImages()
    {
        foreach (var item in _locks)
            Monitor.Enter(item);

        DisposeItems();
        for (int i = 0; i < _items.Length; i++)
        {
            var item = OnCreateItem();
            if (item == null)
            {
                throw new InvalidOperationException("OnCreateItem() can't return null.");
            }
            _items[i] = item;
        }

        foreach (var item in _locks)
            Monitor.Exit(item);
    }

    protected abstract T OnCreateItem();

    protected abstract void OnDisposeItem(T item);

    int GetIndex(int position)
    {
        if (position == -1)
            position = _items.Length - 1;

        return position % _items.Length;
    }

    public override sealed unsafe void LockActive()
    {
        var index = GetIndex(Position);

        Monitor.Enter(_locks[index]);
        var ptr = OnLockActive(_items[index]);
        imageData = ptr;
    }

    public override sealed void UnlockActive()
    {
        var index = GetIndex(Position);
        OnUnlockActive(_items[index]);
        Monitor.Exit(_locks[index]);
    }

    protected abstract ARGBColor* OnLockActive(T item);

    protected abstract void OnUnlockActive(T item);

    void DisposeItems()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            ref var item = ref _items[i];
            if (item != null)
            {
                OnDisposeItem(item);
                item = null;
            }
        }
    }

    public override void Dispose()
    {
        if (!disposedValue)
        {
            DisposeItems();
            disposedValue = true;
        }
    }
}
