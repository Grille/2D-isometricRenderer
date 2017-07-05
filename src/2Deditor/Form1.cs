using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace _2Deditor
{
    public partial class Form1 : Form
    {    
        struct Editor
        {
            public Bitmap Map;
            public float MapPosX;
            public float MapPosY;
            public float MapSize;
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

            public LookBitmap(Bitmap input)
            {
                bmp = new Bitmap(input);
                rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,bmp.PixelFormat);
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
        class oneDTexture{
            bool enabled;
            bool end;
            Color endColor;
            Color[] Colors;
        }

        Bitmap heightMap;
        Bitmap textureMap;
        Editor height;
        Editor texture;
        Editor result;
        Point lastMouse;
        int size;


        public Form1()
        {
            height.init();
            texture.init();
            result.init();

            InitializeComponent();
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void pBHeightMap_MouseDown(object sender    , MouseEventArgs e){}
        private void pBTextureMap_MouseDown(object sender, MouseEventArgs e) { Console.WriteLine(e.Clicks); }
        private void pBRender_MouseDown(object sender       , MouseEventArgs e){}
        private void pBHeightMap_MouseMove(object sender    , MouseEventArgs e)
        {
            pBHeightMap.Focus();
            //if (e.X > heightMapPosX && e.Y > heightMapPosY)
            //{
                if (e.Button == MouseButtons.Left)
                {
                    height.MapPosX -= (lastMouse.X - e.X);
                    height.MapPosY -= (lastMouse.Y - e.Y);
                    pBHeightMap.Refresh();
                }
            //}
            lastMouse = e.Location;
        }
        private void pBHeightMap_MouseWheel(object sender    , MouseEventArgs e)
        {
            Console.WriteLine(e.Delta);
            height.MapSize += (float)(height.MapSize*e.Delta) / 1000f;
            Console.WriteLine(height.MapSize);
            pBHeightMap.Refresh();
        }
        private void pBTextureMap_MouseMove(object sender   , MouseEventArgs e)
        {
            pBTextureMap.Focus();
            //if (e.X > heightMapPosX && e.Y > heightMapPosY)
            //{
            if (e.Button == MouseButtons.Left)
            {
                texture.MapPosX -= (lastMouse.X - e.X);
                texture.MapPosY -= (lastMouse.Y - e.Y);
                pBTextureMap.Refresh();
            }
            //}
            lastMouse = e.Location;
        }
        private void pBTextureMap_MouseWheel(object sender, MouseEventArgs e)
        {
            Console.WriteLine(e.Delta);
            texture.MapSize += (float)(height.MapSize * e.Delta) / 1000f;
            Console.WriteLine(texture.MapSize);
            pBTextureMap.Refresh();
        }
        private void pBRender_MouseMove(object sender       , MouseEventArgs e)
        {
            pBRender.Focus();
            //if (e.X > heightMapPosX && e.Y > heightMapPosY)
            //{
            if (e.Button == MouseButtons.Left)
            {
                result.MapPosX -= (lastMouse.X - e.X);
                result.MapPosY -= (lastMouse.Y - e.Y);
                pBRender.Refresh();
            }
            //}
            lastMouse = e.Location;
        }
        private void pBRender_MouseWheel(object sender, MouseEventArgs e)
        {
            Console.WriteLine(e.Delta);
            result.MapSize += (float)(height.MapSize * e.Delta) / 1000f;
            Console.WriteLine(result.MapSize);
            pBRender.Refresh();
        }
        private void bNew_Click(object sender, EventArgs e)
        {
            heightMap =  new Bitmap("../input/test.png");
            textureMap = new Bitmap("../input/test.png");
            renderHeight();
            renderResult();
            pBHeightMap.Refresh();
            pBTextureMap.Refresh();
            pBRender.Refresh();
        }


        private void renderHeight()
        {
            Stopwatch now = new Stopwatch();
            now.Start();
            if (textureMap == null || heightMap == null) return;
            LookBitmap heightLB = new LookBitmap(heightMap);
            LookBitmap resultLB = new LookBitmap(new Bitmap(64, 64));
            byte[] heightRGB = heightLB.getRGB();
            byte[] resultRGB = resultLB.getRGB();


            int renderPixel = 0;
            int height = 64 * 4;
            for (int ix = 1; ix < 63; ix++)
            {
                for (int iy = 62; iy >= 1; iy--)
                {
                    int counter = (ix + iy * 64) * 4;



                    resultRGB[counter + 1] = (byte)(heightRGB[counter + 1] * 5);
                    if (
                        (heightRGB[counter + 1] > heightRGB[counter + 1 + 4]) ||
                        (heightRGB[counter + 1] > heightRGB[counter + 1 - 4]) ||
                        (heightRGB[counter + 1] > heightRGB[counter + 1 + height]) ||
                        (heightRGB[counter + 1] > heightRGB[counter + 1 - height])

                        ) resultRGB[counter] = 255;

                    resultRGB[counter + 3] = 255;
                    renderPixel++;

                    //renderPixel++;
                }
            }

            Console.WriteLine("--renderHeight()--");
            Console.WriteLine("renderPixels => " + renderPixel);
            Console.WriteLine("renderTime => " + now.Elapsed);

            this.height.Map = resultLB.getBitmap();
        }
        private void renderResult()
        {
            Stopwatch now = new Stopwatch();
            now.Start();
            if (textureMap == null || heightMap == null) return;
            LookBitmap heightLB = new LookBitmap(heightMap);
            LookBitmap textureLB = new LookBitmap(textureMap);
            LookBitmap resultLB = new LookBitmap(new Bitmap(64, 64 + 32));
            byte[] heightRGB = heightLB.getRGB();
            byte[] textureRGB = textureLB.getRGB();
            byte[] resultRGB = resultLB.getRGB();


            int renderPixel = 0;
            int height = 64 * 4;
            for (int ix = 0; ix < 64; ix++)
            {
                for (int iy = 63; iy >= 0; iy--)
                {
                    int counter = (ix + iy * 64) * 4;
                    for (int i = 0; i < heightRGB[counter + 1]; i++)
                    {
                        if ((iy + 32) - i >= 0)//save
                        {
                            int counter2 = counter - (64 * i * 4) + height * 32;//pos + curent height

                            if (resultRGB[counter2 + 3] == 0)
                            {
                                //rgbValues[counter2 + 1] = 255;
                                if (ix % 3 == 0 || iy % 3 == 0) resultRGB[counter2 + 1] = 100;
                                resultRGB[counter2 + 3] = 255;
                                renderPixel++;
                                if (i + 1 == heightRGB[counter + 1]) resultRGB[counter2] = 100;
                            }
                            else
                            {
                                //break;


                            }
                            //renderPixel++;
                        }
                    }
                }
            }

            Console.WriteLine("--renderResult()--");
            Console.WriteLine("renderPixels => " + renderPixel);
            Console.WriteLine("renderTime => " + now.Elapsed);

            result.Map = resultLB.getBitmap();
        }

        private void pBHeightMap_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (height.MapSize < 1) g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            else g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(height.Map, new RectangleF(height.MapPosX, height.MapPosY, (int)(64 * height.MapSize), (int)(64 * height.MapSize)), new RectangleF(0, 0, 64, 64), GraphicsUnit.Pixel);
        }
        private void pBTextureMap_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (texture.MapSize < 1) g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            else g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(texture.Map, new RectangleF(texture.MapPosX, texture.MapPosY, (int)(64 * texture.MapSize), (int)(64 * texture.MapSize)), new RectangleF(0, 0, 64, 64), GraphicsUnit.Pixel);

        }
        private void pBRender_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            
            if(result.MapSize < 1) g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            else g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(result.Map, new RectangleF(result.MapPosX, result.MapPosY, (int)(64 * result.MapSize), (int)((64+32) * result.MapSize)), new RectangleF(0, 0, 64, 64+32), GraphicsUnit.Pixel);
        }

        private void bRender_Click(object sender, EventArgs e)
        {
            renderResult();
        }
        private void bRotL_Click(object sender, EventArgs e)
        {
            heightMap.RotateFlip(RotateFlipType.Rotate270FlipNone);
            renderResult();
            pBRender.Refresh();
            pBHeightMap.Refresh();
        }
        private void bRotR_Click(object sender, EventArgs e)
        {
            heightMap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            renderResult();
            pBRender.Refresh();
            pBHeightMap.Refresh();
        }
    }
}
