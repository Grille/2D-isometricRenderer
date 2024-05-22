using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

using System.Numerics;
using Grille.Graphics.Isometric.Numerics;

using InputDataBuffer = Grille.Graphics.Isometric.Buffers.NativeBuffer<Grille.Graphics.Isometric.Buffers.InputData>;
using Grille.Graphics.Isometric.Buffers;
using System.Xml.Linq;
using System.Security.Policy;

namespace Grille.Graphics.Isometric.WinForms;

public static class BitmapInputData
{
    public static InputDataBuffer FromHeightBitmap(string path)
    {
        using var bitmap = LoadBitmap(path);
        return FromHeightBitmap(bitmap);
    }

    public static InputDataBuffer FromHeightBitmap(Bitmap bitmap)
    {
        var data = new InputDataBuffer(bitmap.Width, bitmap.Height);
        data.LoadHeightBitmap(bitmap);

        var gray = new ARGBColor(127, 127, 127);
        for (int i = 0; i < data.Length; i++)
        {
            data[i].Color = gray;
        }

        return data;
    }

    public static void LoadTextureBitmap(this InputDataBuffer data, string path)
    {
        using var bitmap = LoadBitmap(path);
        data.LoadTextureBitmap(bitmap);
    }

    public static void LoadHeightBitmap(this InputDataBuffer data, string path)
    {
        using var bitmap = LoadBitmap(path);
        data.LoadHeightBitmap(bitmap);
    }

    public static void LoadNormalBitmap(this InputDataBuffer data, string path)
    {
        using var bitmap = LoadBitmap(path);
        data.LoadNormalBitmap(bitmap);
    }

    public static unsafe void LoadTextureBitmap(this InputDataBuffer idata, Bitmap bitmap)
    {
        using var data = LockBits<byte>(idata, bitmap);
        var ptr = data.Scan0;

        int size = data.Length;

        switch (data.PixelFormat)
        {
            case PixelFormat.Format4bppIndexed:
            {
                var palette = bitmap.Palette.Entries;
                int iDst = 0;
                for (int iSrc = 0; iSrc < size / 2; iSrc++)
                {
                    byte idx8 = ptr[iSrc];

                    idata[iDst].Color = palette[idx8 & 15];
                    ++iDst;

                    idata[iDst].Color = palette[idx8 >> 4];
                    ++iDst;
                }
            }
            break;
            case PixelFormat.Format8bppIndexed:
            {
                var palette = bitmap.Palette.Entries;
                for (int i = 0; i < size; i++)
                {
                    idata[i].Color = palette[ptr[i]];
                }
            }
            break;
            case PixelFormat.Format24bppRgb:
            {
                for (int i = 0; i < size; i++)
                {
                    idata[i].Color = Color.FromArgb(
                        ptr[i * 3 + 2],
                        ptr[i * 3 + 1],
                        ptr[i * 3 + 0]
                    );
                }
            }
            break;
            case PixelFormat.Format32bppArgb:
            {
                var cptr = (ARGBColor*)ptr;
                for (int i = 0; i < size; i++)
                {
                    idata[i].Color = cptr[i];
                }
            }
            break;
            default:
            {
                throw new InvalidDataException($"{data.PixelFormat}");
            }
        }
    }

    public static unsafe void LoadHeightBitmap(this InputDataBuffer idata, Bitmap bitmap)
    {
        using var data = LockBits<byte>(idata, bitmap);
        var ptr = data.Scan0;

        int size = data.Length;
        int stride = data.Stride;

        for (int i = 0; i < size; i++)
        {
            idata[i].Height = ptr[i * stride];
        }
    }

    public static unsafe void LoadNormalBitmap(this InputDataBuffer idata, Bitmap bitmap)
    {
        using var data = LockBits<byte>(idata, bitmap);
        var ptr = data.Scan0;

        int length = data.Length;
        int stride = data.Stride;
        int offsetX;
        int offsetY;

        switch (data.PixelFormat)
        {
            case PixelFormat.Format24bppRgb:
                offsetX = 2; offsetY = 1;
            break;
            case PixelFormat.Format32bppArgb:
                offsetX = 2; offsetY = 1;
            break;
            default:
                throw new InvalidDataException($"{data.PixelFormat}");
        }

        for (int i = 0; i < length; i++)
        {
            idata[i].Normals = S8Vec2.FromBytes(ptr[i * stride + offsetX], ptr[i * stride + offsetY]);
        }
    }

    public unsafe static Bitmap NormalDataToBitmap(this InputDataBuffer idata)
    {
        var bitmap = new Bitmap(idata.Width, idata.Height, PixelFormat.Format32bppArgb);

        using var data = LockBits<ARGBColor>(idata, bitmap);
        var ptr = data.Scan0;

        for (int i = 0; i < idata.Length; i++)
        {
            ptr[i] = (ARGBColor)idata[i].Normals;
        }

        return bitmap;
    }

    unsafe record struct BitmapEntry<T> (Bitmap Bitmap, BitmapData BitmapData, bool Owned) : IDisposable where T : unmanaged 
    {
        public int Width => BitmapData.Width;
        public int Height => BitmapData.Height;
        public int Length => BitmapData.Width * BitmapData.Height;
        public int Stride => BitmapData.Stride / BitmapData.Width;
        public PixelFormat PixelFormat => BitmapData.PixelFormat;

        public T* Scan0 => (T*)BitmapData.Scan0;

        public void Dispose()
        {
            if (Owned)
            {
                Bitmap.Dispose();
                return;
            }
            Bitmap.UnlockBits(BitmapData);
        }
    }

    static BitmapEntry<T> LockBits<T>(InputDataBuffer idata, Bitmap bitmap) where T : unmanaged
    {
        bool owned = false;
        if (bitmap.Width != idata.Width || bitmap.Height != idata.Height)
        {
            var newbitmap = new Bitmap(idata.Width, idata.Height, PixelFormat.Format24bppRgb);
            using (var g = System.Drawing.Graphics.FromImage(newbitmap))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.DrawImage(bitmap, new Rectangle(0, 0, idata.Width, idata.Height));
            }
            bitmap = newbitmap;
            owned = true;
        }

        var bitmapRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        var data = bitmap.LockBits(bitmapRect, ImageLockMode.ReadOnly, bitmap.PixelFormat);

        return new BitmapEntry<T>(bitmap, data, owned);
    }

    static Bitmap LoadBitmap(string path)
    {
        var bitmap = (Bitmap)Image.FromFile(path);
        return bitmap;
    }
}
