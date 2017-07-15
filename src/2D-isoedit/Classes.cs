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
namespace _2Deditor
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
    class LookBitmap
    {
        private Bitmap bmp;
        private Rectangle rect;
        private System.Drawing.Imaging.BitmapData bmpData;
        private IntPtr ptr;
        private int bytes;
        private byte[] rgbValues;


        public LookBitmap(Bitmap input, bool byValue)
        {
            if (byValue) bmp = new Bitmap(input);
            else bmp = input;
            rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
            ptr = bmpData.Scan0;
            bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
        }
        public Bitmap getBitmap()
        {
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);
            return bmp;
        }
        public byte[] getRGB()
        {
            return rgbValues;
        }
    }
    class Texture
    {
        private bool endColor;
        private byte size;
        private Color[] colorsZ;
        public Texture(Color[] colors, bool endColor)
        {
            this.endColor = endColor;
            colorsZ = colors;
            size = (byte)(colorsZ.Length);
            if (endColor) size--;
        }

        public void setColor(byte[] arrayRGB, int offset, byte height, byte maxHeight, float shadow)
        {
            if (endColor && height + 1 == maxHeight)
            {
                arrayRGB[offset + 0] = (byte)(colorsZ[size].B * shadow);
                arrayRGB[offset + 1] = (byte)(colorsZ[size].G * shadow);
                arrayRGB[offset + 2] = (byte)(colorsZ[size].R * shadow);
                arrayRGB[offset + 3] = (byte)(colorsZ[size].A);
            }
            else
            {
                while (height >= size) height -= size;
                arrayRGB[offset + 0] = (byte)(colorsZ[height].B * shadow);
                arrayRGB[offset + 1] = (byte)(colorsZ[height].G * shadow);
                arrayRGB[offset + 2] = (byte)(colorsZ[height].R * shadow);
                arrayRGB[offset + 3] = (byte)(colorsZ[height].A);
            }
        }

    }
}