using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using Grille.Graphics.Isometric.Numerics;


namespace Grille.Graphics.Isometric.WinForms;

public unsafe class BitmapSwapchain : Swapchain<Bitmap>
{
    BitmapData? _bitmapData;

    public BitmapSwapchain(int width, int height) : base(3)
    {
        ResizeImages(width, height);
    }

    protected override Bitmap OnCreateItem()
    {
        return new Bitmap(ImageWidth, ImageHeight, PixelFormat.Format32bppArgb);
    }

    protected override void OnDisposeItem(Bitmap item)
    {
        item.Dispose();
    }

    protected override unsafe ARGBColor* OnLockActive(Bitmap bitmap)
    {
        var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        _bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);

        int size = bitmap.Width * bitmap.Height;
        var ptr = (ARGBColor*)_bitmapData.Scan0;

        new Span<ARGBColor>(ptr, size).Clear();

        return ptr;
    }

    protected override void OnUnlockActive(Bitmap bitmap)
    {
        if (_bitmapData == null) throw new InvalidOperationException("_bitmapData is null.");
        bitmap.UnlockBits(_bitmapData);
        _bitmapData = null;
    }
}
