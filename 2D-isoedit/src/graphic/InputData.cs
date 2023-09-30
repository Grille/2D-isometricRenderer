using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Program.src.graphic;

namespace Program;

public class InputData
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Size { get; private set; }

    public RenderDataCell[] Buffer { get; private set; }

    TexturePack textures;

    public TexturePack Textures
    {
        get => textures;
        set
        {
            textures = value;
            UpdateTextureIndices();
        }
    }

    public InputData(int width, int height)
    {
        Init(width, height);
    }

    public InputData(string pathHeight)
    {
        string name = Path.GetFileNameWithoutExtension(pathHeight);
        string ext = Path.GetExtension(pathHeight);
        string path = Path.GetDirectoryName(pathHeight);

        using var bitmapHeight = new Bitmap(pathHeight);

        Init(bitmapHeight.Width, bitmapHeight.Height);

        LoadHeightBitmap(bitmapHeight);

        string pathTexture = Path.Combine(path, name + "_tex" + ext);
        if (File.Exists(pathTexture))
        {
            using var bitmapTexture = new Bitmap(pathTexture);
            LoadTextureBitmap(bitmapTexture);
        }
    }

    public InputData(string pathHeight, string pathTexture)
    {
        using var bitmapHeight = new Bitmap(pathHeight);

        Init(bitmapHeight.Width, bitmapHeight.Height);

        LoadHeightBitmap(bitmapHeight);

        using var bitmapTexture = new Bitmap(pathTexture);
        LoadTextureBitmap(bitmapTexture);
    }

    public InputData(Bitmap bitmapHeight, Bitmap bitmapTexture)
    {
        Init(bitmapHeight.Width, bitmapHeight.Height);

        LoadHeightBitmap(bitmapHeight);
        LoadTextureBitmap(bitmapTexture);
    }

    public ref RenderDataCell this[int index]
    {
        get => ref Buffer[index];
    }

    public ref RenderDataCell this[IVector2 index]
    {
        get => ref Buffer[index.X + index.Y * Width];
    }

    void Init(int width, int height)
    {
        Width = width;
        Height = height;
        Size = Width * Height;

        Buffer = new RenderDataCell[Size];
    }

    void UpdateTextureIndices()
    {
        int size = Width * Height;
        for (int i = 0; i < size; i++)
        {
            Buffer[i].TextureIndex = (byte)textures.GetId(Buffer[i].Color);
        }
    }

    public void LoadTextureBitmap(string path)
    {
        using var bitmap = new Bitmap(path);
        LoadTextureBitmap(bitmap);
    }

    public unsafe void LoadTextureBitmap(Bitmap bitmap)
    {
        if (bitmap.Width != Width || bitmap.Height != Height)
        {
            var newbitmap = new Bitmap(Width, Height);
            using (var g = Graphics.FromImage(newbitmap))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.DrawImage(bitmap, new Rectangle(0, 0, Width, Height));
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

                    Buffer[iDst].TextureIndex = 0;
                    Buffer[iDst].Color = palette[idx8 & 15];
                    ++iDst;

                    Buffer[iDst].TextureIndex = 0;
                    Buffer[iDst].Color = palette[idx8 >> 4];
                    ++iDst;
                }
            }
            break;
            case PixelFormat.Format8bppIndexed:
            {
                var palette = bitmap.Palette.Entries;
                for (int i = 0; i < size; i++)
                {
                    Buffer[i].TextureIndex = 0;
                    Buffer[i].Color = palette[ptr[i]];
                }
            }
            break;
            case PixelFormat.Format24bppRgb:
            {
                for (int i = 0; i < size; i++)
                {
                    Buffer[i].TextureIndex = 0;
                    Buffer[i].Color = Color.FromArgb(
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
                    Buffer[i].TextureIndex = 0;
                    Buffer[i].Color = cptr[i];
                }
            }
            break;
            default:
            {
                throw new InvalidDataException($"{data.PixelFormat}");
            }
        }

        if (textures != null)
        {
            UpdateTextureIndices();
        }
    }

    public void LoadHeightBitmap(string path)
    {
        using var bitmap = new Bitmap(path);
        LoadHeightBitmap(bitmap);
    }

    public unsafe void LoadHeightBitmap(Bitmap bitmap)
    {
        var data = LockBits(bitmap);
        byte* ptr = (byte*)data.Scan0;
        int size = data.Width * data.Height;
        int stride = data.Stride / data.Width;

        for (int i = 0; i < size; i++)
        {
            Buffer[i].Height = ptr[i * stride];
        }

        CalculateHeightDifference();
    }

    public unsafe void CalculateHeightDifference()
    {
        var offsets = new IVector2[] {
            new IVector2(1, 0),
            new IVector2(-1, 0),
            new IVector2(0, 1),
            new IVector2(0, -1),
            new IVector2(1, 1),
            new IVector2(-1, -1),
            new IVector2(1, -1),
            new IVector2(-1, 1)
        };

        int SampleHeightDifference(IVector2 location1, IVector2 location2)
        {
            return Math.Abs(this[location1].Height - this[location2].Height);
        }

        for (int ix = 0; ix < Width; ix++)
        {
            for (int iy = 0; iy < Height; iy++)
            {
                var location = new IVector2(ix, iy);

                int max = 0;

                foreach (var offset in offsets)
                {
                    var location2 = (location + offset).Clamp(0, Height - 1);
                    max = Math.Max(max, SampleHeightDifference(location, location2));
                }

                this[location].HeightDifference = (ushort)max;
            }
        }
    }

    BitmapData LockBits(Bitmap bitmap)
    {
        if (bitmap.Width != Width || bitmap.Height != Height)
            throw new ArgumentException();

        var bitmapRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        return bitmap.LockBits(bitmapRect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
    }
}
