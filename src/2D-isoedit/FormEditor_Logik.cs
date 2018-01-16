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
    public partial class FormEditor : Form
    {
        public void init()
        {
            input.init();
            result.init();


            textures = new Texture[] {
            new Texture("grass", new byte[] {1,0, 255,70,100,40,255 }),
            new Texture("dirt", new byte[] {1,0, 255,110,100,80,255 }),
            new Texture("sand", new byte[] {2,1, 255,80,100,50,255, 1,80,110,50,255 }),
            new Texture("stone", new byte[] {2,0, 2,150,150,150,255, 6,130,130,130,255}),
            new Texture("dark grass", new byte[] {2,1, 1,30,70,20,255, 1,40,90,30,255}),
            new Texture("water", new byte[] {2,0, 1,220,220,255,255, 1,150,150,255,255}),
            new Texture("grass", new byte[] {2,1, 1,200,200,200,255, 1,200,100,50,255}),
            new Texture("wall 1", new byte[] {1,0, 255,200,200,200,255}),
            new Texture("wall 2", new byte[] {1,0, 255,150,150,150,255}),
            new Texture("wall 3", new byte[] {2,0, 4,100,100,100,255, 4,75,75,75,255}),
            new Texture("wall 4", new byte[] {2,0, 6,120,120,120,255, 2,150,0,0,255}),
            new Texture("window 1", new byte[] {3,0, 3,200,200,200,255, 2,50,50,100,255, 255,200,200,200,255}),
            };

            load("../maps/autosave.png");
            //255 

            //InitializeComponent();
        }
        
        //Prepare the heightMap call rotate and shadow    
        private LockBitmap prepareMap(LockBitmap inputLB)
        {
            LockBitmap resultLB = new LockBitmap((int)(inputLB.Width*1.5), (int)(inputLB.Height*1.5));
            if (cores == 1)
                rotate(inputLB, resultLB, 0, 1);
            else
            {
                Task[] thread = new Task[(int)cores];

                for (int i = 0; i < cores; i++)
                {
                    int thmp = (int)(i + 1);
                    thread[i] = new Task(() =>
                        rotate(inputLB, resultLB, thmp / cores - 1 / cores, thmp / cores));
                }
                for (int i = 0; i < cores; i++) thread[i].Start();
                for (int i = 0; i < cores; i++) thread[i].Wait();
            }

            if (radioButtonShadowHigh.Checked) shadows(resultLB,1);
            else if (radioButtonShadowLow.Checked) shadows(resultLB, 5);

            return resultLB;
        }
        //rotate byte pixel array
        private void rotate(LockBitmap inputLB, LockBitmap resultLB, float start, float end) 
        {
            byte[] inputRGB = inputLB.getData();
            byte[] resultRGB = resultLB.getData();
            int inputW = inputLB.Width, inputH = inputLB.Height,resultW = resultLB.Width, resultH = resultLB.Height;

            double sinma = Math.Sin(-angle * 3.14159265 / 180);
            double cosma = Math.Cos(-angle * 3.14159265 / 180);

            for (int x = (int)(resultW * start); x < (int)(resultW * end); x++)
            {
                for (int y = 0; y < resultH; y++)
                {
                    int hwidth = (int)(inputW /2);
                    int hheight = (int)(inputH /2);

                    int xt = (int)(x - hwidth*1.5);
                    int yt = (int)(y - hheight*1.5);

                    int xs = (int)((cosma * (xt) - sinma * (yt)) + hwidth);
                    int ys = (int)((sinma * (xt) + cosma * (yt)) + hheight);

                    int offsetDst = (x + y * resultW) * 4;
                    int offsetSrc = (xs + ys * inputW) * 4;
                    if (xs >= 0 && xs < inputW && ys >= 0 && ys < inputH)
                    {
                        resultRGB[offsetDst + 0] = inputRGB[offsetSrc + 0];//texture
                        resultRGB[offsetDst + 1] = inputRGB[offsetSrc + 1];//height
                    }
                }
            }

            }
        //add shadows
        private void shadows(LockBitmap resultLB,int resulution) 
        {
            byte[] resultRGB = resultLB.getData();
            int width = resultLB.Width, height = resultLB.Height;
            for (int iy = 0; iy < (int)(height); iy++)//y 0 to 1
            {
                for (int ix = 0; ix < width; ix += resulution)//x 0 to 1
                {
                    //get position
                    int offset = (ix + iy * width) * 4;
                        int i = 0;

                        float shadowHeight = (resultRGB[offset + 1]);
                        while (resultRGB[offset + 2] < shadowHeight)
                        {
                            if (i > 0) resultRGB[offset + 2] = (byte)(shadowHeight * 1f);

                            if (resultRGB[offset + 1] > shadowHeight + 1) break;
                            i++; shadowHeight -= 1f; offset += 4;
                        }
                    }
                
            }
        }

        //Rendering the image call elevate
        private void renderResult(LockBitmap inputMap)
        {
            if (inputMap == null) return;
            LockBitmap inputLB = inputMap;
            LockBitmap resultLB = new LockBitmap(new Bitmap((int)(inputMap.Width), (int)(inputMap.Height*0.5) + heightExcess), false);
            byte[] inputRGB = inputLB.getData();
            byte[] resultRGB = resultLB.getData();

            int renderPixel = 0;

            if (cores == 1)
                elevate(inputLB, resultLB, 0, 1);
            else
            {
                Task[] thread = new Task[(int)cores];
                for (int i = 0; i < cores; i++)
                {
                    int thmp = (int)(i + 1);
                    thread[i] = new Task(() =>
                        elevate(inputLB, resultLB, thmp / cores - 1 / cores, thmp / cores));
                }
                for (int i = 0; i < cores; i++) thread[i].Start();
                for (int i = 0; i < cores; i++) thread[i].Wait();
            }
            result.Map = resultLB.returnBitmap();
        }
        //Rendering the part of image from heightmap (elevate and apply textures & shadows)
        private void elevate(LockBitmap inputLB, LockBitmap resultLB, float start, float end)
        {
            byte[] inputRGB = inputLB.getData();
            byte[] resultRGB = resultLB.getData();
            int widthSrc = inputLB.Width, heightSrc = inputLB.Height, widthDst = resultLB.Width, heightDst = resultLB.Height;

            for (int ix = (int)(widthSrc * start); ix < (int)(widthSrc * end); ix++)//Upwards
            {
                for (int iy = (heightSrc - 1); iy >= 0; iy -= 1)//Downwards
                {
                        //get positions
                        int offSrc = (ix + iy * widthSrc) * 4;
                        int offDst = (ix + iy / 2 * widthDst) * 4;


                            //height > 0
                            if (inputRGB[offSrc + 1] > 0)
                            {
                                //get colorList & find color pos
                                byte[] refColor = textures[inputRGB[offSrc]].getData();
                                int colorSize = refColor[0] - refColor[1];
                                int colorStart = 0;
                                int colorListPos = -1;
                                int colorPos = 0;
                                while (colorStart < inputRGB[offSrc + 1])
                                {
                                    colorListPos++;
                                    if (colorListPos >= colorSize) colorListPos = 0;
                                    colorStart += refColor[2 + colorListPos * 5];
                                }
                                colorPos = refColor[2 + colorListPos * 5] - (colorStart - inputRGB[offSrc + 1]);

                                int iz = inputRGB[offSrc + 1];
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
                                                if (iz < inputRGB[offSrc + 2]+1) shadow = 0.75f;
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
        private void render(bool renderEditor)
        {
            Stopwatch now = new Stopwatch();
            now.Start();
            isRenering = true;
            renderResult(prepareMap(this.inputLB));
            isRenering = false;
            result.renderInfo = ("RenderTime: " + now.ElapsedMilliseconds + "\nFPS: " + 1000 / (now.ElapsedMilliseconds + 0.1f) + "\nTasks: " + (int)cores);
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