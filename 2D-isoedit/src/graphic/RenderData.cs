using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Program
{
    public class RenderData
    {
        public int Width, Height;

        public RenderDataCell[] Buffer;

        public RenderData() : this(0, 0) { }
        public RenderData(int width,int height)
        {
            init(width, height);
        }

        private void init(int width, int height)
        {
            if (Width == width && Height == height)
                return;
            Width = width;
            Height = height;

            Buffer = new RenderDataCell[width * height];
        }


        public void UseTexturePack(TexturePack textures)
        {
            for (int i = 0; i < Width * Height; i++)
            {
                Buffer[i].TextureIndex = (byte)textures.GetId(Buffer[i].Color);
            }
        }

        public void LoadFromBitmapFiles(string pathHeight)
        {
            string name = Path.GetFileNameWithoutExtension(pathHeight);
            string ext = Path.GetExtension(pathHeight);
            string path = Path.GetDirectoryName(pathHeight);

            string pathTexture = Path.Combine(path, name + "_tex" + ext);

            LoadFromBitmapFiles(pathHeight, pathTexture);
        }
        public void LoadFromBitmapFiles(string pathHeight, string pathTexture)
        {
            LoadHeightFromBitmapFile(pathHeight);
            LoadTextureMapFromBitmapFile(pathTexture);
        }

        public unsafe void LoadTextureMapFromBitmapFile(string path)
        {
            
            try
            {
                using var bitmap = new Bitmap(path);

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

                Console.WriteLine("LoadData: " + path);
            }
            catch (Exception e)
            {
                Console.WriteLine("LoadData: " + path + " Failed " + e.Message);
            }
            
        }

        public unsafe void LoadHeightFromBitmapFile(string path)
        {
            try
            {
                using var bitmap = new Bitmap(path);

                var data = LockBits(bitmap);
                byte* ptr = (byte*)data.Ptr;
                int size = data.Size;
                int stride = data.Stride;

                for (int i = 0; i < size; i++)
                {
                    Buffer[i].Height = ptr[i * stride];
                }

                Console.WriteLine("LoadData: " + path);
            }
            catch (Exception e)
            {
                Console.WriteLine("LoadData: " + path + " Failed " + e.Message);
            }

        }

        record struct BitmapData(nint Ptr, int Stride, int Size);
        BitmapData LockBits(Bitmap bitmap)
        {
            var bitmapRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(bitmapRect, ImageLockMode.ReadOnly, bitmap.PixelFormat);

            nint ptr = bitmapData.Scan0;

            init(bitmap.Width, bitmap.Height);

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
}
