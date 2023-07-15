using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Program;

internal unsafe class Swapchain : IDisposable
{
    Bitmap active;
    Bitmap result;
    BitmapData data;
    byte* ptr;

    private bool disposed;

    public BitmapData Data => data;

    public byte* Ptr => ptr;

    public Bitmap Result => result;

    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Size { get; private set; }

    public Swapchain(int width, int height)
    {
        Width = width;
        Height = height;
        Size = Width * Height;

        var fromat = PixelFormat.Format32bppArgb;
        active = new Bitmap(width, height, fromat);
        result = new Bitmap(width, height, fromat);

        LockActive();
    }

    void LockActive()
    {
        var rect = new Rectangle(0, 0, active.Width, active.Height);
        data = active.LockBits(rect, ImageLockMode.ReadWrite, active.PixelFormat);
        ptr = (byte*)data.Scan0;

        int size = active.Width * active.Height;
        var iptr = (int*)ptr;
        /*
        for (int i = 0; i < size; i++)
        {
            iptr[i] = 0;
        }
        */
    }

    void UnlockActive()
    {
        active.UnlockBits(data);
    }

    public void Swap()
    {
        UnlockActive();

        var temp = active;
        var fromat = PixelFormat.Format32bppArgb;
        active = new Bitmap(Width, Height, fromat);
        result = temp;

        LockActive();

    }

    public void Dispose()
    {
        if (disposed)
            return;

        disposed = true;
        active.Dispose();
        result.Dispose();
    }
}
