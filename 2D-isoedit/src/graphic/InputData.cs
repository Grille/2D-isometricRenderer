using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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

        var data = LockBits(bitmap);
        byte* ptr = (byte*)data.Ptr;
        int size = data.Size;
        int stride = data.Stride;

        switch (stride)
        {
            case 3:
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
            case 4:
            {
                for (int i = 0; i < size; i++)
                {
                    Buffer[i].TextureIndex = 0;
                    Buffer[i].Color = Color.FromArgb(
                        ptr[i * 4 + 3],
                        ptr[i * 4 + 2],
                        ptr[i * 4 + 1],
                        ptr[i * 4 + 0]
                    );
                }
            }
            break;
            default:
            {
                throw new Exception();
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
        byte* ptr = (byte*)data.Ptr;
        int size = data.Size;
        int stride = data.Stride;

        for (int i = 0; i < size; i++)
        {
            Buffer[i].Height = ptr[i * stride];
        }
    }

    record struct BitmapData(nint Ptr, int Stride, int Size);
    BitmapData LockBits(Bitmap bitmap)
    {
        if (bitmap.Width != Width || bitmap.Height != Height)
            throw new ArgumentException();

        var bitmapRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        var bitmapData = bitmap.LockBits(bitmapRect, ImageLockMode.ReadOnly, bitmap.PixelFormat);

        nint ptr = bitmapData.Scan0;

        int size = bitmapRect.Width * bitmapRect.Height;

        int stride = bitmap.PixelFormat switch
        {
            PixelFormat.Format8bppIndexed => 1,
            PixelFormat.Format24bppRgb => 3,
            PixelFormat.Format32bppArgb => 4,
            _ => throw new Exception(),
        };

        return new BitmapData(ptr, stride, size);
    }
}
