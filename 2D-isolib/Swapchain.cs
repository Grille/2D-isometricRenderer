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

    public int Position { get; private set; }

    public int Size { get; }

    public abstract ARGBColor* ImageData { get; }

    public Swapchain(int size)
    {
        Size = size;
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
    readonly T[] _items;

    ARGBColor* imageData;
    private bool disposedValue;

    public override ARGBColor* ImageData => imageData;

    public MonitorHandle<T> Image => new(Get(Position - 1));

    public Swapchain(int size) : base(size)
    {
        _items = new T[size];
    }

    public override sealed void OnResizeImages()
    {
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
    }

    protected abstract T OnCreateItem();

    protected abstract void OnDisposeItem(T item);



    T Get(int pos)
    {
        if (pos == -1)
            pos = _items.Length - 1;

        var bitmap = _items[pos % _items.Length];
        return bitmap;
    }

    public override sealed unsafe void LockActive()
    {
        var bitmap = Get(Position);

        Monitor.Enter(bitmap);

        var ptr = OnLockActive(bitmap);
        imageData = ptr;

        /*
        for (int i = 0; i < size; i++)
        {
            iptr[i] = 0;
        }
        */
    }

    public override sealed void UnlockActive()
    {
        var bitmap = Get(Position);

        OnUnlockActive(bitmap);

        Monitor.Exit(bitmap);
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
