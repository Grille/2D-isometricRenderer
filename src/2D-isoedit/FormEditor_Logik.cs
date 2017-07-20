using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace _2Deditor
{
    //partial class hide { } //Hide Designer in VS 
    public partial class FormEditor
    {
        Texture[] textures;
        Bitmap inputMap;
        RenderInfo input;
        RenderInfo result;
        Point lastMousePos;

        Stopwatch fps;

        //Rendering Values
        byte heightExcess = 128;
        byte[] shadowHeightMap;
        byte[] shadowSmoothMap;

        //Rendering orientation
        int angle = 45;
        float tilt = 0.5f;//fixed

        //Editor Values
        bool curTextureEdit;
        bool renderAllInTimer;
        byte editValue = 1;
        public void init()
        

        {
            
            input.init();
            result.init();

            textures = new Texture[] {
            new Texture(new Color[] {Color.FromArgb(80, 100, 50),Color.FromArgb(80, 105, 50),Color.FromArgb(80, 100, 50)},false),
            new Texture(new Color[] {Color.FromArgb(110, 100, 80)},false),
            new Texture(new Color[] {Color.FromArgb(80, 100, 50),Color.FromArgb(80, 110, 50)},true),
            new Texture(new Color[] {Color.FromArgb(150, 150, 150),Color.FromArgb(140, 140, 160),Color.FromArgb(140, 140, 150)},false),
            new Texture(new Color[] {Color.FromArgb(30, 70, 20),Color.FromArgb(40, 90, 30)},true),
            new Texture(new Color[] {Color.FromArgb(220, 220, 255),Color.FromArgb(150, 150, 255)},true),
            new Texture(new Color[] {Color.FromArgb(200, 200, 200),Color.FromArgb(200, 100, 50)},true),
            };

            //InitializeComponent();
        }
        //Prepare the heightMap (rotate, compress y and add shadow)
        private LockBitmap prepareMap(Bitmap heightMap)
        {
            Stopwatch now = new Stopwatch();
            now.Start();
            int width = heightMap.Width;
            int height = heightMap.Height;
            Bitmap heightBM = new Bitmap(heightMap); 
            LockBitmap heightLB;
            byte[] heightRGB;


            ////Rotate graphic
            if (checkBoxGame.Checked)//game render
            {
                if (angle >= 270) heightBM.RotateFlip(RotateFlipType.Rotate270FlipNone);
                else if (angle >= 180) heightBM.RotateFlip(RotateFlipType.Rotate180FlipNone);
                else if (angle >= 90) heightBM.RotateFlip(RotateFlipType.Rotate90FlipNone);
                heightLB = new LockBitmap(heightBM, false);
                heightRGB = heightLB.getRGB();
            }
            else//dynamic render
            {
                int oldWidth = width, oldHeight = height;
                width = (int)(width * 1.5f); height = (int)(height * 1.5f);
                heightLB = new LockBitmap(new Bitmap(width, height), false);
                heightRGB = heightLB.getRGB();
                LockBitmap baseLB = new LockBitmap(heightBM, false);
                byte[] baseRGB = baseLB.getRGB();
                int midx = baseLB.Width / 2;
                int midy = baseLB.Height / 2;

                int tx = 0, ty = 0;
                float sin = (float)Math.Sin(angle * 3.14159265 / 180), cos = (float)Math.Cos(angle * 3.14159265 / 180);
                for (int ix = 0; ix < oldWidth; ix++)//x 0 to 1
                {
                    for (int iy = (int)((oldHeight - 1)); iy >= 0; iy--)//y 1 to 0
                    {
                        tx = (int)((ix - midx) * cos - (iy - midy) * sin) + (int)(midx*1.5f);
                        ty = (int)((iy - midy) * cos + (ix - midx) * sin) + (int)(midy*1.5f);
                        //get pixel in 1D byte arrey
                        int counterDest = (tx + ty * width) * 4;
                        int counterSrc = (ix + iy * oldWidth) * 4;
                        //Prefer height
                        if (ty > 0 && tx > 0 && ty < height && tx < width)
                        {
                            heightRGB[counterDest + 1] = baseRGB[counterSrc + 1];
                            //heightRGB[counterDest + 2] = baseRGB[counterSrc + 2];
                            heightRGB[counterDest + 3] = baseRGB[counterSrc + 3];
                            heightRGB[counterDest + 0] = baseRGB[counterSrc + 0];
                        }
                    }
                }
            }



            //Look bitmap and set ref to RGB byte array
            LockBitmap resultLB = new LockBitmap(new Bitmap((int)(width), (int)(height * tilt)), false);
            byte[] resultRGB = resultLB.getRGB();

            //stored var to Y move
            int offsetWidth = width * 4;

            //Scale graphic
            if (checkBoxGame.Checked) //game render
            {
                for (int ix = 0; ix < width; ix++)//x 0 to 1
                {
                    for (int iy = (int)((height) / 4); iy >= 0; iy--) //y 0.5 to 0
                    {
                        //get pixel in 1D byte arrey
                        int counterDest = (ix + iy * width) * 4 + offsetWidth;
                        int counterSrc = (int)((ix + iy * 2 * width) * 4) + 0;

                        resultRGB[counterDest + 1] = heightRGB[counterSrc + 1];
                        resultRGB[counterDest + 3] = heightRGB[counterSrc + 3];
                        resultRGB[counterDest + 0] = heightRGB[counterSrc + 0];
                    }
                    for (int iy = (int)((height) / 4); iy < (int)((height) / 2) - 1; iy++) //y 0.5 to 1
                    {
                        //get pixel in 1D byte arrey
                        int counterDest = (ix + iy * width) * 4 + offsetWidth;
                        int counterSrc = (int)((ix + iy * 2 * width) * 4) + offsetWidth;

                        resultRGB[counterDest + 1] = heightRGB[counterSrc + 1];
                        resultRGB[counterDest + 3] = heightRGB[counterSrc + 3];
                        resultRGB[counterDest + 0] = heightRGB[counterSrc + 0];
                    }


                }
            }
            else //dynamic render
            {
                for (int ix = 0; ix < width; ix++)//x 0 to 1
                {
                    for (int iy = (int)((height - 1) * tilt); iy >= 0; iy--)//y 1 to 0
                    {
                        //get pixel in 1D byte arrey
                        int counterDest = (ix + iy * width) * 4;
                        int counterSrc = (int)((ix + iy / tilt * width) * 4);
                        //Prefer height
                        if (heightRGB[counterSrc + 1] < heightRGB[counterSrc + 1 + offsetWidth])
                        {
                            resultRGB[counterDest + 1] = heightRGB[counterSrc + 1 + offsetWidth];
                            resultRGB[counterDest + 3] = heightRGB[counterSrc + 3 + offsetWidth];
                            resultRGB[counterDest + 0] = heightRGB[counterSrc + 0 + offsetWidth];
                        }
                        else
                        {
                            resultRGB[counterDest + 1] = heightRGB[counterSrc + 1];
                            resultRGB[counterDest + 3] = heightRGB[counterSrc + 3];
                            resultRGB[counterDest + 0] = heightRGB[counterSrc + 0];
                        }
                    }
                }
            }

            //render shadows?
            //shadowHeightMap = new byte[width * height];
            if (checkBoxShadow.Checked)
            {
                for (int ix = 0; ix < width; ix++)//x 0 to 1
                {
                    for (int iy = (int)((height - 1) * tilt); iy >= 0; iy--)//y 1 to 0
                    {
                        //get pixel in 1D byte arrey
                        int counterBase = (ix + iy * width);
                        int counter = counterBase * 4;
                        int i = 0, max = (resultRGB[counter + 1]);
                        while (iy + i < height && resultRGB[counter + 2] < max && resultRGB[counter + 1] <= max + 1)
                        {
                            if (i > 0) resultRGB[counter + 2] = (byte)(max * 1f);
                            i++; max--; counter += 4;
                        }

                    }
                }
            }
            //Console.WriteLine(now.ElapsedMilliseconds);
            return resultLB;
        }
        //Rendering the image from heightmap (elevate and apply textures & shadows)
        private void renderResult(LockBitmap inputMap)
        {
            Stopwatch now = new Stopwatch();
            now.Start();
            if (inputMap == null) return;
            LockBitmap inputLB = inputMap;
            LockBitmap resultLB = new LockBitmap(new Bitmap(inputMap.Width, inputMap.Height + heightExcess), false);
            byte[] inputRGB = inputLB.getRGB();
            byte[] resultRGB = resultLB.getRGB();


            int renderPixel = 0;
            float cores = 1;
            Thread[] thread = new Thread[(int)cores];
            for (int i = 0; i < cores; i++)
            {
                thread[i] = new Thread(() =>
                    elevate(inputRGB, resultRGB, inputMap, i / cores - 1/ cores, i/ cores));
                thread[i].Start();
                //thread[i].Join();
            }
            for (int i = 0; i < cores; i++)
            {
                thread[i].Join();
                Console.WriteLine("thread"+i+" join");
            }
            Console.WriteLine("return\n\n");
            result.renderInfo = ("renderPixels => " + renderPixel) + '\n' + ("renderTime => " + now.ElapsedMilliseconds);
            result.Map = resultLB.getBitmap();
        }

        void elevate(byte[] inputRGB, byte[] resultRGB, LockBitmap inputMap, float start, float end)
        {
            int width = inputMap.Width, height = inputMap.Height;
            for (int ix = (int)(width* start); ix < (int)(width* end); ix++)
            {
                for (int iy = height - 1; iy >= 0; iy--) //Downwards
                {
                    int counter = (ix + iy * width) * 4;
                    for (byte iz = inputRGB[counter + 1]; iz > 0; iz--) //Downwards
                    {
                        if ((iy + heightExcess) - iz >= 0)//save
                        {
                            int counter2 = counter - (width * iz * 4) + width * heightExcess * 4;//pos + curent height
                            if (resultRGB[counter2 + 3] == 0)
                            {

                                float shadow = 1f;
                                if (iz < inputRGB[counter + 2]) shadow = 0.75f;
                                textures[inputRGB[counter]].setColor(resultRGB, counter2, (byte)(iz - 1), inputRGB[counter + 1], shadow);
                                //resultRGB[counter2 + 3] = 255;
                                //if (i == inputRGB[counter + 1]) resultRGB[counter2] = 100;
                                //resultRGB[counter2 + 1] = (byte)(((byte)(i * 100)) / 4 + 30);

                                //renderPixel++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        //render the high editor map
        private void renderHeight(LockBitmap inputMap)
        {
            Stopwatch now = new Stopwatch();
            now.Start();
            if (inputMap == null) return;
            LockBitmap heightLB = inputMap;
            LockBitmap resultLB = new LockBitmap(new Bitmap(inputMap.Width, inputMap.Height), false);
            byte[] heightRGB = heightLB.getRGB();
            byte[] resultRGB = resultLB.getRGB();
            int renderPixel = 0;
            int offsetWidth = inputMap.Width * 4;
            int width = inputMap.Width, height = inputMap.Height;
            for (int ix = 0; ix < inputMap.Width; ix++)
            {
                for (int iy = inputMap.Height - 1; iy >= 0; iy--)
                {
                    int counter = iy * offsetWidth + ((ix) * 4);
                    if (heightRGB[counter + 1] > 0)
                    {
                        byte thmp = (byte)((byte)(heightRGB[counter + 1] * 20) / 2);
                        resultRGB[counter + 0] = thmp;
                        resultRGB[counter + 1] = (byte)(((byte)(heightRGB[counter + 1] / 20)) * 40);
                        //if (ix < width - 1 && heightRGB[counter + 1] > heightRGB[counter + 1 + 4]) 
                        //if (ix > 0 && heightRGB[counter + 1] > heightRGB[counter + 1 - 4]) 
                        //if (iy < height - 1 && heightRGB[counter + 1] > heightRGB[counter + 1 + offsetWidth]) 
                        //if (iy > 0 && heightRGB[counter + 1] > heightRGB[counter + 1 - offsetWidth]) 
                        resultRGB[counter + 3] = 255;
                        renderPixel++;
                    }
                }
            }

            this.input.renderInfo = ("renderPixels => " + renderPixel) + '\n' + ("renderTime => " + now.ElapsedMilliseconds);
            this.input.Map = resultLB.getBitmap();
        }
        //render the high texture map
        private void renderTexture(LockBitmap inputMap)
        {
            Stopwatch now = new Stopwatch();
            now.Start();
            if (inputMap == null) return;
            LockBitmap heightLB = inputMap;
            LockBitmap resultLB = new LockBitmap(new Bitmap(inputMap.Width, inputMap.Height), false);
            byte[] heightRGB = heightLB.getRGB();
            byte[] resultRGB = resultLB.getRGB();
            int renderPixel = 0;
            int offsetWidth = inputMap.Width * 4;
            int width = inputMap.Width, height = inputMap.Height;

            for (int ix = 0; ix < width; ix++)
            {
                for (int iy = height - 1; iy >= 0; iy--)
                {
                    int counter = iy * offsetWidth + ((ix) * 4);

                    if (heightRGB[counter + 1] > 0)
                    {
                        float h, s, v;
                        h = (byte)(heightRGB[counter + 0] * 30);

                        if (ix < width - 1 && heightRGB[counter + 1] != heightRGB[counter + 1 + 4]) s = 0.7f;
                        else if (ix > 0 && heightRGB[counter + 1] != heightRGB[counter + 1 - 4]) s = 0.7f;
                        else if (iy < height - 1 && heightRGB[counter + 1] != heightRGB[counter + 1 + offsetWidth]) s = 0.7f;
                        else if (iy > 0 && heightRGB[counter + 1] != heightRGB[counter + 1 - offsetWidth]) s = 0.7f;
                        else s = 1;

                        if (ix < width - 1 && heightRGB[counter + 0] != heightRGB[counter + 0 + 4]) v = 0.7f;
                        else if (ix > 0 && heightRGB[counter + 0] != heightRGB[counter + 0 - 4]) v = 0.7f;
                        else if (iy < height - 1 && heightRGB[counter + 0] != heightRGB[counter + 0 + offsetWidth]) v = 0.7f;
                        else if (iy > 0 && heightRGB[counter + 0] != heightRGB[counter + 0 - offsetWidth]) v = 0.7f;
                        else v = 1;

                        int pos = (int)(h / 256 * 6);
                        int x = (int)(h / 256 * (256 * 6));
                        int r = 0, g = 0, b = 0;
                        while (x > 255) x -= 255;
                        switch (pos)
                        {
                            case 0: r += 255; g += x; b += 0; break;
                            case 1: r += 255 - x; g += 255; b += 0; break;
                            case 2: r += 0; g += 255; b += x; break;
                            case 3: r += 0; g += 255 - x; b += 255; break;
                            case 4: r += x; g += 0; b += 255; break;
                            case 5: r += 255; g += 0; b += 255 - x; break;
                        }
                        float pro = (((s) / 1));
                        r = (int)(r * (pro) + ((255) * (1 - pro)));//r
                        g = (int)(g * (pro) + ((255) * (1 - pro)));//g
                        b = (int)(b * (pro) + ((255) * (1 - pro)));//b

                        pro = v / 1;
                        r = (int)(r * pro);//r
                        g = (int)(g * pro);//g
                        b = (int)(b * pro);//b


                        resultRGB[counter + 0] = (byte)b;
                        resultRGB[counter + 1] = (byte)g;
                        resultRGB[counter + 2] = (byte)r;
                        resultRGB[counter + 3] = 255;
                        renderPixel++;
                    }
                }
            }

            this.input.renderInfo = ("renderPixels => " + renderPixel) + '\n' + ("renderTime => " + now.ElapsedMilliseconds);
            this.input.Map = resultLB.getBitmap();
        }


        private void render(bool renderEditor)
        {

            LockBitmap inputLB = prepareMap(this.inputMap);
            renderResult(inputLB);
            pBResult.Refresh();
            if (true||renderEditor)
            {
                //render editor graphic with native inputMap
                LockBitmap baseLB = new LockBitmap(this.inputMap,true);
                if (curTextureEdit) renderTexture(inputLB);
                else renderHeight(inputLB);
                pBEditorMap.Refresh();
            }
        }

        private void addAngle(int value)
        {
            angle += value;
            if (angle <= 0) angle += 360;
            else if (angle >= 360) angle -= 360;
        }
    }
}