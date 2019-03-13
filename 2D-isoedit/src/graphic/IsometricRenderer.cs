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

using GGL;
using GGL.IO;

namespace program
{
    public class IsometricRenderer
    {
        //setings
        float cores = (int)(Environment.ProcessorCount);

        //Tasks
        public RenderData Data;
        private RenderData translatedData;
        public TexturePack TexturePack;
        public Bitmap Result { get; private set; }



        public int RenderTime;
        //settings
        public int ShadowQuality = 1;

        float angle = 45;

        byte heightExcess = 255;

        public bool IsRendering { get; private set; } = false;

        public IsometricRenderer()
        {
            //LoadDataFromFile("../examples/maps/autosave.png");
            TexturePack = new TexturePack();
            //TexturePack.Load("../examples/textures/default.tex");
        }

        public float Angle
        {
            set
            {
                angle = value;
                if (angle <= 0) angle += 360;
                else if (angle >= 360) angle -= 360;
            }
            get
            {
                return angle;
            }
        }
        //Prepare the heightMap call rotate and shadow    
        private RenderData translateMap(RenderData input)
        {
            translatedData = new RenderData((int)(input.Width * 1.5), (int)(input.Height * 1.5));
            //LockBitmap resultLB = new LockBitmap((int)(inputLB.Width * 1.5), (int)(inputLB.Height * 1.5));
            if (cores == 1)
                rotate(input, translatedData, 0, 1);
            else
            {
                Task[] thread = new Task[(int)cores];

                for (int i = 0; i < cores; i++)
                {
                    int thmp = (int)(i + 1);
                    thread[i] = new Task(() =>
                        rotate(input, translatedData, thmp / cores - 1 / cores, thmp / cores));
                }
                for (int i = 0; i < cores; i++) thread[i].Start();
                for (int i = 0; i < cores; i++) thread[i].Wait();
            }

            shadows(translatedData, ShadowQuality);

            return translatedData;
        }
        //rotate byte pixel array
        private void rotate(RenderData input, RenderData result, float start, float end)
        {
            int inputW = input.Width, inputH = input.Height, resultW = result.Width, resultH = result.Height;

            double sinma = Math.Sin(-angle * 3.14159265 / 180);
            double cosma = Math.Cos(-angle * 3.14159265 / 180);

            for (int x = (int)(resultW * start); x < (int)(resultW * end); x++)
            {
                for (int y = 0; y < resultH; y++)
                {
                    int hwidth = (int)(inputW / 2);
                    int hheight = (int)(inputH / 2);

                    int xt = (int)(x - hwidth * 1.5);
                    int yt = (int)(y - hheight * 1.5);

                    int xs = (int)((cosma * (xt) - sinma * (yt)) + hwidth);
                    int ys = (int)((sinma * (xt) + cosma * (yt)) + hheight);

                    int offsetDst = (x + y * resultW);
                    int offsetSrc = (xs + ys * inputW);
                    if (xs >= 0 && xs < inputW && ys >= 0 && ys < inputH)
                    {
                        result.HeightMap[offsetDst] = input.HeightMap[offsetSrc];//texture
                        result.TextureMap[offsetDst] = input.TextureMap[offsetSrc];//height
                    }
                }
            }

        }
        //add shadows
        private void shadows(RenderData result, int resulution)
        {

            if (resulution == 0) return;
            int width = result.Width, height = result.Height;
            for (int iy = 0; iy < (int)(height); iy++)//y 0 to 1
            {
                for (int ix = 0; ix < width; ix += resulution)//x 0 to 1
                {
                    //get position
                    int offset = (ix + iy * width);
                    int i = 0;

                    float shadowHeight = (result.HeightMap[offset]);
                    while (result.ShadowMap[offset] < shadowHeight)
                    {
                        if (i > 0) result.ShadowMap[offset] = (byte)(shadowHeight * 1f);

                        if (result.HeightMap[offset] > shadowHeight + 1) break;
                        i++; shadowHeight -= 1f; offset += 1;
                    }
                }

            }

        }

        //Rendering the image call elevate
        private void renderResult(RenderData input)
        {
            if (input == null) return;
            LockBitmap resultLB = new LockBitmap(new Bitmap((int)(input.Width), (int)(input.Height * 0.5) + heightExcess), false);

            if (cores == 1)
                elevate(input, resultLB, 0, 1);
            else
            {
                Task[] thread = new Task[(int)cores];
                for (int i = 0; i < cores; i++)
                {
                    int thmp = (int)(i + 1);
                    thread[i] = new Task(() => elevate(input, resultLB, thmp / cores - 1 / cores, thmp / cores));
                }
                for (int i = 0; i < cores; i++) thread[i].Start();
                for (int i = 0; i < cores; i++) thread[i].Wait();
            }
            Result = resultLB.returnBitmap();
        }
        //Rendering the part of image from heightmap (elevate and apply textures & shadows)
        private void elevate(RenderData input, LockBitmap resultLB, float start, float end)
        {
            byte[] resultRGB = resultLB.Data;
            int widthSrc = input.Width, heightSrc = input.Height, widthDst = resultLB.Width, heightDst = resultLB.Height;

            for (int ix = (int)(widthSrc * start); ix < (int)(widthSrc * end); ix++)//Upwards
            {
                for (int iy = (heightSrc - 1); iy >= 0; iy -= 1)//Downwards
                {
                    //get positions
                    int offSrc = (ix + iy * widthSrc);
                    int offDst = (ix + iy / 2 * widthDst) * 4;


                    //height > 0
                    if (input.HeightMap[offSrc] > 0)
                    {
                        //get colorList & find color pos
                        byte[] refColor = TexturePack.Textures[input.TextureMap[offSrc]].Data;
                        int colorSize = refColor[0] - refColor[1];
                        int colorStart = 0;
                        int colorListPos = -1;
                        int colorPos = 0;
                        while (colorStart < input.HeightMap[offSrc])
                        {
                            colorListPos++;
                            if (colorListPos >= colorSize) colorListPos = 0;
                            colorStart += refColor[2 + colorListPos * 5];
                        }
                        colorPos = refColor[2 + colorListPos * 5] - (colorStart - input.HeightMap[offSrc]);

                        int iz = input.HeightMap[offSrc];
                        while (iz > 0) //Downwards
                        {
                            //Repeat until color is changed | ground reached
                            while (iz > 0 && colorPos > 0)
                            {
                                //save
                                if ((iy + heightExcess) - iz >= 0)
                                {
                                    //get position on z axe
                                    int offDstZ = offDst - (widthDst * iz * 4) + widthDst * heightExcess * 4;//pos + curent height
                                                                                                             //pixel not yet drawn
                                    if (resultRGB[offDstZ + 3] == 0)
                                    {
                                        //draw pixel
                                        float shadow = 1f;
                                        if (iz < input.ShadowMap[offSrc] + 1) shadow = 0.75f;
                                        //resultRGB[offDstZ + 0] = (byte)(255 * shadow);//b
                                        //resultRGB[offDstZ + 1] = (byte)(255 * (iz % 5) * shadow);//g
                                        //resultRGB[offDstZ + 2] = (byte)(0 * shadow);//r
                                        //resultRGB[offDstZ + 3] = (byte)(255);//a
                                        resultRGB[offDstZ + 0] = (byte)(refColor[5 + colorListPos * 5] * shadow);//b
                                        resultRGB[offDstZ + 1] = (byte)(refColor[4 + colorListPos * 5] * shadow);//g
                                        resultRGB[offDstZ + 2] = (byte)(refColor[3 + colorListPos * 5] * shadow);//r
                                        resultRGB[offDstZ + 3] = (byte)(refColor[6 + colorListPos * 5]);//a
                                    }
                                    else
                                    {
                                        iz = 0;
                                        break;
                                    }
                                }
                                //get next z & color pos
                                iz--;
                                colorPos--;
                            }
                            //get next color
                            colorListPos--;
                            if (colorListPos < 0) colorListPos = colorSize - 1;
                            colorPos = refColor[2 + colorListPos * 5];
                        }
                    }
                }

            }
        }

        //Render
        public Bitmap Render()
        {
            if (this.Data == null)
                return new Bitmap(1, 1);
            Stopwatch now = new Stopwatch();
            now.Start();
            IsRendering = true;
            renderResult(translateMap(this.Data));
            IsRendering = false;
            RenderTime = (int)now.ElapsedMilliseconds;
            return Result;
            //drawImage = true;
            //result.renderInfo = ("RenderTime: " + now.ElapsedMilliseconds + "\nFPS: " + 1000 / (now.ElapsedMilliseconds + 0.1f) + "\nTasks: " + (int)cores);
        }

        public void LoadDataFromBitmapFile(string path)
        {
            try
            {
                using (var bmpTemp = new Bitmap(path))
                {
                    LockBitmap lockBitmap = new LockBitmap(new Bitmap(bmpTemp), false);
                    int width = lockBitmap.Width, height = lockBitmap.Height;
                    Data = new RenderData(width, height);
                    for (int i = 0; i < width * height; i++)
                    {
                        Data.TextureMap[i] = lockBitmap.Data[i * 4 + 0];
                        Data.HeightMap[i] = lockBitmap.Data[i * 4 + 1];
                    }
                }
                Console.WriteLine("LoadData: " + path);
            }
            catch (Exception e)
            {
                Console.WriteLine("LoadData: " + path + " Failed "+e.Message);
            }

        }
    }
}
