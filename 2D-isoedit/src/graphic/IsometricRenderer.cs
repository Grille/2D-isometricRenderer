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
        LockBitmap inputLB;
        Bitmap result;

        Texture[] textures;

        public int RenderTime;
        //settings
        public int ShadowQuality = 1;

        float angle = 45;

        byte heightExcess = 255;

        private bool isRenering = false;

        public bool IsRendering
        {
            get
            {
                return isRenering;
            }
        }

        public IsometricRenderer()
        {
            LoadMap("../maps/autosave.png");
            LoadTexture("../textures/default.tex");
        }


        //Prepare the heightMap call rotate and shadow    
        private LockBitmap prepareMap(LockBitmap inputLB)
        {
            LockBitmap resultLB = new LockBitmap((int)(inputLB.Width * 1.5), (int)(inputLB.Height * 1.5));
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

            shadows(resultLB, ShadowQuality);

            return resultLB;
        }
        //rotate byte pixel array
        private void rotate(LockBitmap inputLB, LockBitmap resultLB, float start, float end)
        {
            byte[] inputRGB = inputLB.getData();
            byte[] resultRGB = resultLB.getData();
            int inputW = inputLB.Width, inputH = inputLB.Height, resultW = resultLB.Width, resultH = resultLB.Height;

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
        private void shadows(LockBitmap resultLB, int resulution)
        {
            if (resulution == 0) return;
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
            LockBitmap resultLB = new LockBitmap(new Bitmap((int)(inputMap.Width), (int)(inputMap.Height * 0.5) + heightExcess), false);
            byte[] inputRGB = inputLB.getData();
            byte[] resultRGB = resultLB.getData();

            if (cores == 1)
                elevate(inputLB, resultLB, 0, 1);
            else
            {
                Task[] thread = new Task[(int)cores];
                for (int i = 0; i < cores; i++)
                {
                    int thmp = (int)(i + 1);
                    thread[i] = new Task(() => elevate(inputLB, resultLB, thmp / cores - 1 / cores, thmp / cores));
                }
                for (int i = 0; i < cores; i++) thread[i].Start();
                for (int i = 0; i < cores; i++) thread[i].Wait();
            }
            result = resultLB.returnBitmap();
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
                        byte[] refColor = textures[inputRGB[offSrc]].Data;
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
                                        if (iz < inputRGB[offSrc + 2] + 1) shadow = 0.75f;
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
            Stopwatch now = new Stopwatch();
            now.Start();
            isRenering = true;
            renderResult(prepareMap(this.inputLB));
            isRenering = false;
            RenderTime = (int)now.ElapsedMilliseconds;
            return result;
            //drawImage = true;
            //result.renderInfo = ("RenderTime: " + now.ElapsedMilliseconds + "\nFPS: " + 1000 / (now.ElapsedMilliseconds + 0.1f) + "\nTasks: " + (int)cores);
        }

        public void AddAngle(float value)
        {
            SetAngle(angle + value);
        }
        public void SetAngle(float value)
        {
            angle = value;
            if (angle <= 0) angle += 360;
            else if (angle >= 360) angle -= 360;
        }
        /*
        private void addTilt(int value)
        {
            tilt += value / 100f;
            if (tilt < 0.2) tilt = 0.2f;
            else if (tilt > 0.8) tilt = 0.8f;
            tilt = (int)(tilt * 100) / 100f;
        }
        */

        public void LoadMap(string path)
        {
            Console.WriteLine("LoadMap: " + path);
            using (var bmpTemp = new Bitmap(path))
            {
                inputLB = new LockBitmap(new Bitmap(bmpTemp), false);
            }
        }
        public void LoadTexture(string path)
        {
            Console.WriteLine("LoadTexture: " + path);
            //"../textures/default.tex"
            ByteStream bs = new ByteStream(path);
            bs.ResetIndex();
            int length = bs.ReadInt();
            textures = new Texture[255];
            for (int i = 0; i < 255; i++)
            {
                if (i < length)
                    textures[i] = new Texture(bs.ReadString(), bs.ReadByteArray());
                else
                    textures[i] = new Texture("-", new byte[] { 6, 0, 4, 255, 0, 0, 255, 4, 255, 255, 0, 255, 4, 0, 255, 0, 255, 4, 0, 255, 255, 255, 4, 0, 0, 255, 255, 4, 255, 0, 255, 255 });
                //textures[i].Data = new byte[] { 1, 0, 8,70,100, 40, 255, 8, 70, 100, 40, 255 };//[bs.ReadByteArray();
            }
        }
        public void SaveTexture(string path)
        {
            ByteStream bs = new ByteStream();
            bs.ResetIndex();
            bs.WriteInt(textures.Length);
            for (int i = 0; i < textures.Length; i++)
            {
                bs.WriteString(textures[i].Name);
                bs.WriteByteArray(textures[i].Data);
            }
            bs.Save(path);
        }
    }
}
