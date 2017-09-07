using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
/// <summary>-</summary>
namespace program
{

    struct RenderInfo
    {
        public Bitmap Map;
        public float MapPosX;
        public float MapPosY;
        public float MapSize;
        public string renderInfo;
        public void init()
        {
            Map = new Bitmap(64, 64);
            MapPosX = 300 / 2 - 32;
            MapPosY = 300 / 2 - 32;

            MapSize = 1;
        }
    }
    class Texture
    {
        private string name;
        private byte[] data;//[r,g,b,a,l]
        public Texture(string name,byte[] input)
        {
            this.name = name;
            data = input;
        }
        public byte[] getData() { return data; }
        public string getName() { return name; }
    }
    class ByteArray
    {
        public int Width;
        public int Height;
        public int Offset;
        private byte[] values;
        public ByteArray(int Width, int Height, int Offset)
        {
            this.Width = Width;
            this.Height = Height;
            this.Offset = Offset;
            values = new byte[Width * Height * Offset];
        }
        public byte[] getValue()
        {
            return values;
        }
    }
}