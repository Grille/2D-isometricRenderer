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

namespace Grille.Graphics.Isometric.WinForms;

public static class BitmapInputData
{
    public static RenderDataBuffer FromBitmap(string pathHeight)
    {
        var name = Path.GetFileNameWithoutExtension(pathHeight);
        var ext = Path.GetExtension(pathHeight);
        var path = Path.GetDirectoryName(pathHeight);
        if (path == null)
            throw new ArgumentException();

        using var bitmapHeight = new Bitmap(pathHeight);

        var data = new RenderDataBuffer(bitmapHeight.Width, bitmapHeight.Height);

        data.LoadHeightBitmap(bitmapHeight);

        string pathTexture = Path.Combine(path, name + "_tex" + ext);
        if (File.Exists(pathTexture))
        {
            using var bitmapTexture = new Bitmap(pathTexture);
            data.LoadTextureBitmap(bitmapTexture);
        }

        return data;
    }

    public static RenderDataBuffer FromBitmap(string pathHeight, string pathTexture)
    {
        using var bitmapHeight = new Bitmap(pathHeight);

        var data = new RenderDataBuffer(bitmapHeight.Width, bitmapHeight.Height);

        data.LoadHeightBitmap(bitmapHeight);

        using var bitmapTexture = new Bitmap(pathTexture);
        data.LoadTextureBitmap(bitmapTexture);

        return data;
    }

    public static RenderDataBuffer FromBitmap(Bitmap bitmapHeight, Bitmap bitmapTexture)
    {
        var data = new RenderDataBuffer(bitmapHeight.Width, bitmapHeight.Height);

        data.LoadHeightBitmap(bitmapHeight);
        data.LoadTextureBitmap(bitmapTexture);

        return data;
    }

    public static void LoadTextureBitmap(this RenderDataBuffer data, string path)
    {
        using var bitmap = new Bitmap(path);
        data.LoadTextureBitmap(bitmap);
    }

    public static unsafe void LoadTextureBitmap(this RenderDataBuffer idata, Bitmap bitmap)
    {
        if (bitmap.Width != idata.Width || bitmap.Height != idata.Height)
        {
            var newbitmap = new Bitmap(idata.Width, idata.Height);
            using (var g = System.Drawing.Graphics.FromImage(newbitmap))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.DrawImage(bitmap, new Rectangle(0, 0, idata.Width, idata.Height));
            }
            bitmap = newbitmap;
        }

        var data = LockBits(bitmap);
        byte* ptr = (byte*)data.Scan0;
        int size = data.Width*data.Height;

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

    public static void LoadHeightBitmap(this RenderDataBuffer data, string path)
    {
        using var bitmap = new Bitmap(path);
        data.LoadHeightBitmap(bitmap);
    }

    public static unsafe void LoadHeightBitmap(this RenderDataBuffer idata, Bitmap bitmap)
    {
        var data = LockBits(bitmap);
        byte* ptr = (byte*)data.Scan0;
        int size = data.Width * data.Height;
        int stride = data.Stride / data.Width;

        for (int i = 0; i < size; i++)
        {
            idata[i].Height = ptr[i * stride];
        }

        idata.CalculateNormals();
    }

    static BitmapData LockBits(Bitmap bitmap)
    {
        var bitmapRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        return bitmap.LockBits(bitmapRect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
    }
}
