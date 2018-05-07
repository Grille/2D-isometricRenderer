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
        public string Name;
        public byte[] Data;//[r,g,b,a,l]
        public Texture(string name,byte[] input)
        {
            this.Name = name;
            Data = input;
        }
    }
}