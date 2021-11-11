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

namespace Program
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
            Data = new RenderData();
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

            int widthSrc = input.Width, 
                heightSrc = input.Height, 
                widthDst = resultLB.Width, 
                heightDst = resultLB.Height;

            int beginX = (int)(widthSrc * start),
                endX = (int)(heightSrc * end);
                
            for (int ix = beginX; ix < endX; ix++) //L->R
            {
                for (int iy = (heightSrc - 1); iy >= 0; iy -= 1) //Downwards
                //for (int iy = 0; iy < heightSrc; iy += 1) //Upwards
                {
                    //get positions
                    int offSrc = (ix + iy * widthSrc);
                    int offDst = (ix + iy / 2 * widthDst) * 4;

                    int iz = input.HeightMap[offSrc];
                    while (iz > 0) //Downwards
                    {
                        //save
                        if (iy + heightExcess - iz >= 0)
                        {
                            //get position on z axe
                            int offDstZ = offDst - (widthDst * iz * 4) + widthDst * heightExcess * 4;//pos + curent height

                            //pixel not yet drawn
                            if (resultRGB[offDstZ + 3] == 0)
                            {
                                //draw pixel
                                float shadow = 1f;
                                if (iz < input.ShadowMap[offSrc] + 1)
                                    shadow = 0.75f;

                                var color = TexturePack[input.TextureMap[offSrc]].GetColorAt(iz);
                                float ff = 0.01f;// color.A / 255f;
                                float ff2 = 1 - ff;
                                //resultRGB[offDstZ + 0] = (byte)(255 * shadow);//b
                                //resultRGB[offDstZ + 1] = (byte)(255 * (iz % 5) * shadow);//g
                                //resultRGB[offDstZ + 2] = (byte)(0 * shadow);//r
                                //resultRGB[offDstZ + 3] = (byte)(255);//a
                                /*
                                resultRGB[offDstZ + 0] = (byte)(resultRGB[offDstZ + 0] * ff2 + color.B * shadow * ff);//b
                                resultRGB[offDstZ + 1] = (byte)(resultRGB[offDstZ + 1] * ff2 + color.G * shadow * ff);//g
                                resultRGB[offDstZ + 2] = (byte)(resultRGB[offDstZ + 2] * ff2 + color.R * shadow * ff);//r
                                resultRGB[offDstZ + 3] = 255;// (byte)(resultRGB[offDstZ + 3] * ff2 + color.A * shadow * ff); ;// (byte)(color.A);//a
                                */
                                
                                resultRGB[offDstZ + 0] += (byte)(color.B * shadow);//b
                                resultRGB[offDstZ + 1] += (byte)(color.G * shadow);//g
                                resultRGB[offDstZ + 2] += (byte)(color.R * shadow);//r
                                resultRGB[offDstZ + 3] += 255;// (byte)(color.A);//a
                                
                            }
                            else
                            {
                                iz = 0;
                                break;
                            }
                        }
                        iz--;
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
    }
}
