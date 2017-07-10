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
        Size bitmapSize;
        byte addHeight = 64;
        byte[] shadowMap;
        int angle = 45;
        float gf = 0.5f;
        float gfadd = 0.01f;
        //int[] heightMap; 


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
            heightMap = new Bitmap("../input/test.png");
            textureMap = new Bitmap("../input/test.png");
            render(); render();
            timer1.Enabled = true;
        }

        private void render()
        {
            Bitmap heightMap = switchMode(this.heightMap);
            bitmapSize = heightMap.Size;
            shadowMap = new byte[bitmapSize.Width * bitmapSize.Height];
            renderHeight(heightMap);
            renderResult(heightMap);
            pBHeightMap.Refresh();
            pBTextureMap.Refresh();
            pBRender.Refresh();
        }

        private Bitmap switchMode(Bitmap heightMap)
        {
            
            bitmapSize.Width = (int)(64*1.5f);
            bitmapSize.Height = (int)(64 * 1.5f);
            Bitmap result = new Bitmap((int)(bitmapSize.Width), (int)(bitmapSize.Height));
            Graphics g = Graphics.FromImage(result);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.TranslateTransform(bitmapSize.Width / 2, bitmapSize.Height / 2);
            g.RotateTransform(angle);
            //g.ScaleTransform(1, 0.5f);
            //g.DrawImage(heightMap,new Point(0,0));
            g.DrawImage(heightMap, new RectangleF(-heightMap.Width/2, -heightMap.Height / 2, heightMap.Width, heightMap.Width), new RectangleF(0, 0, heightMap.Width, heightMap.Width), GraphicsUnit.Pixel);
            //g.FillRectangle(new SolidBrush(Color.FromArgb(0,10,0)),new Rectangle(0, 0, 10, 10));
            g.ResetTransform();

            LookBitmap heightLB = new LookBitmap(result);
            LookBitmap resultLB = new LookBitmap(new Bitmap((int)(bitmapSize.Width), (int)(bitmapSize.Height)));
            byte[] heightRGB = heightLB.getRGB();
            byte[] resultRGB = resultLB.getRGB();

            
            int height = bitmapSize.Width * 4;
            for (int ix = 0; ix < bitmapSize.Width; ix++)
            {
                for (int iy = (bitmapSize.Height - 1)/2; iy >= 0; iy--)
                {
                    int counterSrc = (ix + iy*2 * bitmapSize.Width) * 4;
                    int counterDest = (ix + iy * bitmapSize.Width) * 4;
                    if (heightRGB[counterSrc + 1] < heightRGB[counterSrc + 1+height])
                    {
                        resultRGB[counterDest + 1] = heightRGB[counterSrc + 1 + height];
                        resultRGB[counterDest + 3] = heightRGB[counterSrc + 3 + height];
                    }
                    else
                    {
                        resultRGB[counterDest + 1] = heightRGB[counterSrc + 1];
                        resultRGB[counterDest + 3] = heightRGB[counterSrc + 3];
                    }
                }
            }

            return resultLB.getBitmap();




        }
        private void renderHeight(Bitmap heightMap)
        {
            Stopwatch now = new Stopwatch();
            now.Start();
            if (textureMap == null || heightMap == null) return;
            LookBitmap heightLB = new LookBitmap(heightMap);
            LookBitmap resultLB = new LookBitmap(new Bitmap(bitmapSize.Width, bitmapSize.Height));
            byte[] heightRGB = heightLB.getRGB();
            byte[] resultRGB = resultLB.getRGB();

            /*
            [][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][]
            [][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][]
            [][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][]
            [][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][]
            [][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][]
            [][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][]
            */

            int renderPixel = 0;
            int height = bitmapSize.Width * 4;
            for (int ix = 1; ix < bitmapSize.Width - 1; ix++)
            {
                for (int iy = bitmapSize.Height - 2; iy >= 1; iy--)
                {
                    int counter = (ix + iy * bitmapSize.Width) * 4;

                    resultRGB[counter + 1] = (byte)(heightRGB[counter + 1] * 5);
                    byte dd = 0;
                    if (heightRGB[counter + 1] > heightRGB[counter + 1 + 4]) dd++;
                    if (heightRGB[counter + 1] > heightRGB[counter + 1 - 4]) dd++;
                    if (heightRGB[counter + 1] > heightRGB[counter + 1 + height]) dd++;
                    if (heightRGB[counter + 1] > heightRGB[counter + 1 - height]) dd++;
                    if (dd > 0) 
                    {
                        resultRGB[counter] = 255;
                        shadowMap[counter / 4] = dd; 
                    }
                    else shadowMap[counter / 4] = 0;

                    if (heightRGB[counter + 1]==1)resultRGB[counter + 2]=200;
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
        private void renderResult(Bitmap heightMap)
        {
            Stopwatch now = new Stopwatch();
            now.Start();
            if (textureMap == null || heightMap == null) return;
            LookBitmap heightLB = new LookBitmap(heightMap);
            LookBitmap textureLB = new LookBitmap(textureMap);
            LookBitmap resultLB = new LookBitmap(new Bitmap(bitmapSize.Width, bitmapSize.Height + addHeight));
            byte[] heightRGB = heightLB.getRGB();
            byte[] textureRGB = textureLB.getRGB();
            byte[] resultRGB = resultLB.getRGB();


            int renderPixel = 0;
            int height = bitmapSize.Height * 4;
            for (int ix = 0; ix < bitmapSize.Width; ix++)
            {
                for (int iy = bitmapSize.Height - 1; iy >= 0; iy--)
                {
                    int counter = (ix + iy * bitmapSize.Width) * 4;
                    for (int i = 0; i < heightRGB[counter + 1]; i++)
                    {
                        if ((iy + addHeight) - i >= 0)//save
                        {
                            int counter2 = counter - (bitmapSize.Width * i * 4) + height * addHeight;//pos + curent height

                            if (resultRGB[counter2 + 3] == 0)
                            {
                                //rgbValues[counter2 + 1] = 255;
                                //if (ix % 3 == 0 || iy % 3 == 0) resultRGB[counter2 + 1] = 100;
                                resultRGB[counter2 + 3] = 255;
                                renderPixel++;
                                if (i + 1 == heightRGB[counter + 1]) resultRGB[counter2] = 100;
                                if (shadowMap[counter / 4] > 0) resultRGB[counter2+1] = 255;
                                //else if (heightDivMap[counter / 4] > 1) resultRGB[counter2 + 1] = 100;
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
            g.DrawImage(height.Map, new RectangleF(height.MapPosX, height.MapPosY, (int)(height.Map.Width * height.MapSize), (int)(height.Map.Height * height.MapSize)), new RectangleF(0, 0, height.Map.Width, height.Map.Height), GraphicsUnit.Pixel);
        }
        private void pBTextureMap_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int width = texture.Map.Width;
            if (texture.MapSize < 1) g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            else g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(texture.Map, new RectangleF(texture.MapPosX, texture.MapPosY, (int)(width * texture.MapSize), (int)(width * texture.MapSize)), new RectangleF(0, 0, width, width), GraphicsUnit.Pixel);

        }
        private void pBRender_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int width = result.Map.Width;
            if(result.MapSize < 1) g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            else g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(result.Map, new RectangleF(result.MapPosX, result.MapPosY, (int)(width * result.MapSize), (int)((width + addHeight) * result.MapSize)), new RectangleF(0, 0, width, width + addHeight), GraphicsUnit.Pixel);
        }

        private void bRender_Click(object sender, EventArgs e)
        {

        }
        private void bRotL_Click(object sender, EventArgs e)
        {
            angle -= 45;
            render();
        }
        private void bRotR_Click(object sender, EventArgs e)
        {
            angle += 45;
            render();
        }

        private void bSwitch_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Console.WriteLine(angle);
            //render(); angle+=1;
            //gf += gfadd;
            //if (gf <= 1f) gfadd = 0.01f;
            //if (gf >= 10f) gfadd = -0.01f;
        }
    }
}
