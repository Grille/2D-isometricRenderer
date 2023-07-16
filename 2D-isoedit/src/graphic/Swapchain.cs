using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace Program;

internal unsafe class Swapchain : IDisposable
{
    Bitmap[] bitmaps;
    int position = 0;

    bool disposed;
    BitmapData data;

    public BitmapData Data => data;

    public MonitorHandle<Bitmap> Result => new(Get(position - 1));

    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Size { get; private set; }

    public Swapchain(int width, int height)
    {
        Width = width;
        Height = height;
        Size = Width * Height;

        bitmaps = new Bitmap[3];
        for (int i = 0; i < bitmaps.Length; i++)
        {
            bitmaps[i] = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        }
    }

    Bitmap Get(int pos)
    {
        if (pos == -1)
            pos = bitmaps.Length - 1;

        var bitmap = bitmaps[pos % bitmaps.Length];
        return bitmap;
    }

    public void Next()
    {
        position += 1;
    }

    public unsafe void LockActive()
    {
        var bitmap = Get(position);

        Monitor.Enter(bitmap);

        var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        data = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);

        int size = bitmap.Width * bitmap.Height;
        var iptr = (int*)data.Scan0;

        for (int i = 0; i < size; i++)
        {
            iptr[i] = 0;
        }
    }

    public void UnlockActive()
    {
        var bitmap = Get(position);

        Monitor.Exit(bitmap);

        bitmap.UnlockBits(data);
    }

    public void Dispose()
    {
        if (disposed) 
            return;

        for (int i = 0; i < bitmaps.Length; i++)
        {
            var bitmap = (Bitmap)bitmaps[i];
            bitmap.Dispose();
        }

        disposed = true;
    }
}
