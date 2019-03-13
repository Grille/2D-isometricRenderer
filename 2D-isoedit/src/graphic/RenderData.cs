using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace program
{
    public class RenderData
    {
        public int Width, Height;
        public byte[] HeightMap;
        public byte[] TextureTypeMap;
        public byte[] TextureMap;
        public byte[] ShadowMap;
        public RenderData(int width,int height)
        {
            Width = width;
            Height = height;
            HeightMap = new byte[width * height];
            TextureMap = new byte[width * height];
            ShadowMap = new byte[width * height];
        }
    }
}
