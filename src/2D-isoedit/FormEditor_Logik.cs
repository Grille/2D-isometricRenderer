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
using System.Threading.Tasks;

using GrillesGameLibrary;
namespace program
{
    //partial class hide { } //Hide Designer in VS 
    public partial class FormEditor
    {
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
        
        //Prepare the heightMap control multi threading (rotate, compress y and add shadow)      
        private LockBitmap prepareMap(LockBitmap inputLB)
        {
            Stopwatch now = new Stopwatch();
            now.Start();

            Task[] thread = new Task[(int)cores];

            int width = inputLB.Width;
            int height = inputLB.Height;
            LockBitmap heightLB;
            byte[] heightRGB;


            ////Rotate graphic
            if (checkBoxGame.Checked)//game render
            {
                //if (angle >= 270) heightBM.RotateFlip(RotateFlipType.Rotate270FlipNone);
                //else if (angle >= 180) heightBM.RotateFlip(RotateFlipType.Rotate180FlipNone);
                //else if (angle >= 90) heightBM.RotateFlip(RotateFlipType.Rotate90FlipNone);
                heightLB = inputLB;
                heightRGB = heightLB.getRGB();
            }
            else//dynamic render
            {
                width = (int)(width * 1.5f); height = (int)(height * 1.5f);
                heightLB = new LockBitmap(width, height);
                heightRGB = heightLB.getRGB();
                LockBitmap baseLB = inputLB;
                byte[] baseRGB = baseLB.getRGB();

                for (int i = 0; i < cores; i++)
                {
                    int thmp = (int)(i + 1);
                    thread[i] = new Task(() =>
                        rotate(baseRGB, heightRGB, width, height, thmp / cores - 1 / cores, thmp / cores));
                }
                for (int i = 0; i < cores; i++) thread[i].Start();
                for (int i = 0; i < cores; i++) thread[i].Wait();


            }

            //Look bitmap and set ref to RGB byte array

            if (checkBoxGame.Checked) tilt = 0.5f;
            LockBitmap resultLB = new LockBitmap((int)(width), (int)(height * tilt));
            byte[] resultRGB = resultLB.getRGB();

            //Scale graphic
            if (checkBoxGame.Checked)//game render
            {
                for (int i = 0; i < cores; i++)
                {
                    int thmp = (int)(i + 1);
                    thread[i] = new Task(() =>
                        compress(heightRGB, resultRGB, width, height, thmp / cores - 1 / cores, thmp / cores));
                }
                for (int i = 0; i < cores; i++) thread[i].Start();
                for (int i = 0; i < cores; i++) thread[i].Wait();
            }
            else //dynamic render
            {
                //compress(heightRGB, resultRGB, width, height, 0, 1, tilt);
                for (int i = 0; i < cores; i++)
                {
                    int thmp = (int)(i + 1);
                    thread[i] = new Task(() =>
                        compress(heightRGB, resultRGB, width, height, thmp / cores - 1 / cores, thmp / cores, tilt));
                }
                for (int i = 0; i < cores; i++) thread[i].Start();
                for (int i = 0; i < cores; i++) thread[i].Wait();
            }

            //render shadows?
            if (checkBoxShadow.Checked) shadows(resultRGB, width, height);

            //Console.WriteLine(now.ElapsedMilliseconds);
            return resultLB;
        }
        //rotate byte pixel array
        private void rotate(byte[] inputRGB, byte[] resultRGB, int width, int height, float start, float end) 
        {
            int oldWidth = (int)(width / 1.5f), oldHeight = (int)(height / 1.5f);
            int midx = oldWidth / 2;
            int midy = oldHeight / 2;

            int tx = 0, ty = 0;
            float sin = (float)Math.Sin(angle * 3.14159265 / 180), cos = (float)Math.Cos(angle * 3.14159265 / 180);
            for (int ix = (int)(oldWidth * start); ix < (int)(oldWidth * end); ix++)//x 0 to 1
            {
                for (int iy = (int)((oldHeight - 1)); iy >= 0; iy--)//y 1 to 0
                {
                    tx = (int)((ix - midx) * cos - (iy - midy) * sin) + (int)(midx * 1.5f);
                    ty = (int)((iy - midy) * cos + (ix - midx) * sin) + (int)(midy * 1.5f);
                    //get pixel in 1D byte arrey
                    int counterDest = (tx + ty * width) * 4;
                    int counterSrc = (ix + iy * oldWidth) * 4;
                    //Prefer height
                    if (ty > 0 && tx > 0 && ty < height && tx < width)
                    {
                        resultRGB[counterDest + 1] = inputRGB[counterSrc + 1];
                        //heightRGB[counterDest + 2] = baseRGB[counterSrc + 2];
                        resultRGB[counterDest + 3] = inputRGB[counterSrc + 3];
                        resultRGB[counterDest + 0] = inputRGB[counterSrc + 0];
                    }
                }
            }
        }
        //compress y of byte pixel array (iso scale)
        private void compress(byte[] inputRGB, byte[] resultRGB, int width, int height, float start, float end)
        {
            int offsetWidth = width * 4;
            for (int ix = (int)(width * start); ix < (int)(width * end); ix++)//x 0 to 1
            {
                for (int iy = (int)((height) / 4); iy >= 0; iy--) //y 0.5 to 0
                {
                    //get pixel in 1D byte arrey
                    int counterDest = (ix + iy * width) * 4 + offsetWidth;
                    int counterSrc = (int)((ix + iy * 2 * width) * 4) + 0;

                    resultRGB[counterDest + 1] = inputRGB[counterSrc + 1];
                    resultRGB[counterDest + 3] = inputRGB[counterSrc + 3];
                    resultRGB[counterDest + 0] = inputRGB[counterSrc + 0];
                }
                for (int iy = (int)((height) / 4); iy < (int)((height) / 2) - 1; iy++) //y 0.5 to 1
                {
                    //get pixel in 1D byte arrey
                    int counterDest = (ix + iy * width) * 4 + offsetWidth;
                    int counterSrc = (int)((ix + iy * 2 * width) * 4) + offsetWidth;

                    resultRGB[counterDest + 1] = inputRGB[counterSrc + 1];
                    resultRGB[counterDest + 3] = inputRGB[counterSrc + 3];
                    resultRGB[counterDest + 0] = inputRGB[counterSrc + 0];
                }


            }
        }
        //compress y of byte pixel array (dynamic scale)
        private void compress(byte[] inputRGB,byte[] resultRGB, int width, int height,float start, float end,float tilt) 
        {
            int offsetWidth = width * 4;
            for (int ix = (int)(width * start); ix < (int)(width * end); ix++)//x 0 to 1
            {
                for (int iy = 0; iy < (int)(height * tilt); iy++)//y 0 to 1
                {
                    //get pixel in 1D byte arrey
                    int counterDest = (ix + iy * width) * 4;
                    int counterSrc = (int)((ix + (int)(iy / tilt) * width) * 4);
                    //Prefer height
                    if (inputRGB[counterSrc + 1] < inputRGB[counterSrc + 1 + offsetWidth])
                    {
                        resultRGB[counterDest + 1] = inputRGB[counterSrc + 1 + offsetWidth];
                        resultRGB[counterDest + 3] = inputRGB[counterSrc + 3 + offsetWidth];
                        resultRGB[counterDest + 0] = inputRGB[counterSrc + 0 + offsetWidth];
                    }
                    else
                    {
                        resultRGB[counterDest + 1] = inputRGB[counterSrc + 1];
                        resultRGB[counterDest + 3] = inputRGB[counterSrc + 3];
                        resultRGB[counterDest + 0] = inputRGB[counterSrc + 0];
                    }
                }
            }
        }
        //add shadows
        private void shadows(byte[] resultRGB,int width,int height) 
        {
            for (int ix = 0; ix < width; ix++)//x 0 to 1
            {
                    for (int iy = 0; iy < (int)(height * tilt); iy++)//y 0 to 1
                    {
                    //get pixel in 1D byte arrey
                    int counterBase = (ix + iy * width);
                    int counter = counterBase * 4;

                    int i = 0;
                    //shadowHeight = curent terrainHeight
                    float shadowHeight = (resultRGB[counter + 1]);
                    while (iy + i < height && resultRGB[counter + 2] < shadowHeight)
                    {
                        if (i > 0) resultRGB[counter + 2] = (byte)(shadowHeight * 1f);

                        if (resultRGB[counter + 1] > shadowHeight + 1) break;
                        i++; shadowHeight -= 1f; counter += 4;
                    }

                }
            }
        }
        
        //Rendering the image control multi threading
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

            int width = inputMap.Width, height = inputMap.Height;

            Task[] thread = new Task[(int)cores];
            for (int i = 0; i < cores; i++)
            {
                int thmp = (int)(i + 1);
                thread[i] = new Task(() =>
                    elevate(inputRGB, resultRGB, width, height, thmp / cores - 1 / cores, thmp / cores));
            }
            for (int i = 0; i < cores; i++) thread[i].Start();
            for (int i = 0; i < cores; i++) thread[i].Wait();

            result.renderInfo = ("renderPixels => " + renderPixel) + '\n' + ("renderTime => " + now.ElapsedMilliseconds);
            result.Map = resultLB.returnBitmap();
        }
        //Rendering the part of image from heightmap (elevate and apply textures & shadows)
        private void elevate(byte[] inputRGB, byte[] resultRGB, int width,int height, float start, float end)
        {
            float tiltFactor;
            if (tilt >= 0.5) tiltFactor = (2 - tilt * 2);
            else tiltFactor = (1.5f-tilt);
            for (int ix = (int)(width* start); ix < (int)(width* end); ix++)
            {
                for (int iy = height - 1; iy >= 0; iy--) //Downwards
                {
                    int counter = (ix + iy * width) * 4;
                    for (int iz = (int)(inputRGB[counter + 1] * tiltFactor); iz > 0; iz--) //Downwards
                    {
                        if ((iy + heightExcess) - iz >= 0)//save
                        {
                            int counter2 = counter - (width * iz * 4) + width * heightExcess * 4;//pos + curent height
                            if (resultRGB[counter2 + 3] == 0)
                            {

                                float shadow = 1f;
                                if (iz < inputRGB[counter + 2] * tiltFactor) shadow = 0.75f;
                                textures[inputRGB[counter]].setColor(resultRGB, (int)(counter2), (byte)(iz - 1), (byte)(inputRGB[counter + 1]), shadow);
                                //resultRGB[counter2 + 3] = 255;
                                //if (iz == inputRGB[counter + 1]) resultRGB[counter2] = 100;
                                //resultRGB[counter2 + 1] = (byte)(((byte)(iz * 100)) / 4 + 30);

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
            LockBitmap resultLB = new LockBitmap(inputMap.Width, inputMap.Height);
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
            this.input.Map = resultLB.returnBitmap();
        }
        //render the high texture map
        private void renderTexture(LockBitmap inputMap)
        {
            Stopwatch now = new Stopwatch();
            now.Start();
            if (inputMap == null) return;
            LockBitmap heightLB = inputMap;
            LockBitmap resultLB = new LockBitmap(inputMap.Width, inputMap.Height);
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
            this.input.Map = resultLB.returnBitmap();
        }

        //Render
        private void render(bool renderEditor)
        {
            renderResult(prepareMap(this.inputLB));
            
            if (renderEditor)
            {
                //render editor graphic with native inputMap
                if (curTextureEdit) renderTexture(this.inputLB);
                else renderHeight(this.inputLB);
                
            }
            pBEditorMap.Refresh();
            pBResult.Refresh();
        }

        //
        private void draw(LockBitmap inputMap, Point startMouse,Point endMouse)
        {
            Console.WriteLine("draw");

            int posX1 = (int)((startMouse.X - input.MapPosX) / input.MapSize + 0.5f);
            int posY1 = (int)((startMouse.Y - input.MapPosY) / input.MapSize + 0.5f);
            int posX2 = (int)((endMouse.X - input.MapPosX) / input.MapSize + 0.5f);
            int posY2 = (int)((endMouse.Y - input.MapPosY) / input.MapSize + 0.5f);

            drawLine(inputMap, posX1, posY1, posX2, posY2);
        }
        private void drawLine(LockBitmap inputMap, int posX1, int posY1, int posX2, int posY2)
        {
            byte[] resultRGB = inputMap.getRGB();
            int offsetWidth = inputMap.Width * 4;

            Console.WriteLine("X=" + posX2 + " Y=" + posY2);

            if (posX2 < posX1)
            {
                int tmp = posX1;
                posX1 = posX2;
                posX2 = tmp;
                    tmp = posY1;
                    posY1 = posY2;
                    posY2 = tmp;

            }
            posX2++; posY2++;
            //if (posY2 < posY1)
            //{
            //    int tmp = posY1;
            //    posY1 = posY2;
            //    posY2 = tmp;
            //}

            int distX = posX2 - posX1;
            int distY = posY2 - posY1;

            float factor = (distY / (float)distX);
            Console.WriteLine(factor);
            for (int ix = posX1; ix < posX2; ix++)
            {
                int addY = (int)((ix - posX1) * factor);
                int y = posY1 + addY;
                int iy = 0;
                while (iy < factor) 
                {
                    int counter = (int)(ix + (y+iy) * inputMap.Width) * 4;
                    resultRGB[counter + 0] = 1;
                    iy++;
                } 


            }


        }

        private void addAngle(int value)
        {
            angle += value;
            if (angle <= 0) angle += 360;
            else if (angle >= 360) angle -= 360;
        }
        private void addTilt(int value)
        {
            tilt += value/100f;
            if (tilt < 0.2) tilt = 0.2f;
            else if (tilt > 0.8) tilt = 0.8f;
            tilt = (int)(tilt * 100)/100f;
        }
    }
}