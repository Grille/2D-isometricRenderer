using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GGL;
using System.IO;

namespace Program
{
    public class RenderData
    {
        public int Width, Height;
        public byte[] HeightMap;
        public byte[] TextureTypeMap;
        public byte[] TextureMap;
        public Color[] ColorMap;
        public byte[] ShadowMap;

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
            HeightMap = new byte[width * height];
            TextureMap = new byte[width * height];
            ShadowMap = new byte[width * height];

            ColorMap = new Color[width * height];
        }


        public void UseTexturePack(TexturePack textures)
        {
            for (int i = 0; i < Width * Height; i++)
            {
                TextureMap[i] = (byte)textures.GetId(ColorMap[i]);
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

        public void LoadTextureMapFromBitmapFile(string path)
        {
            try
            {
                using (var bmpTemp = new Bitmap(path))
                {
                    var lockBitmap = new LockBitmap(new Bitmap(bmpTemp), false);
                    int width = lockBitmap.Width, 
                        height = lockBitmap.Height;
                    init(width, height);
                    for (int i = 0; i < width * height; i++)
                    {
                        TextureMap[i] = 0;// lockBitmap.Data[i * 4 + 0];
                        ColorMap[i] = Color.FromArgb(
                            lockBitmap.Data[i * 4 + 3],
                            lockBitmap.Data[i * 4 + 2],
                            lockBitmap.Data[i * 4 + 1],
                            lockBitmap.Data[i * 4 + 0]
                        );
                    }
                }
                Console.WriteLine("LoadData: " + path);
            }
            catch (Exception e)
            {
                Console.WriteLine("LoadData: " + path + " Failed " + e.Message);
            }

        }

        public void LoadHeightFromBitmapFile(string path)
        {
            try
            {
                using (var bmpTemp = new Bitmap(path))
                {
                    var lockBitmap = new LockBitmap(new Bitmap(bmpTemp), false);
                    int width = lockBitmap.Width, 
                        height = lockBitmap.Height;
                    init(width, height);
                    for (int i = 0; i < width * height; i++)
                    {
                        HeightMap[i] = lockBitmap.Data[i * 4];

                    }
                }
                Console.WriteLine("LoadData: " + path);
            }
            catch (Exception e)
            {
                Console.WriteLine("LoadData: " + path + " Failed " + e.Message);
            }

        }
    }
}
