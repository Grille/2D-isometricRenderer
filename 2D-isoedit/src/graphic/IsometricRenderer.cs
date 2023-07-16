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

using System.Drawing.Imaging;

namespace Program;

public unsafe class IsometricRenderer
{
    //setings
    float cores = (int)Environment.ProcessorCount;

    //Tasks
    public InputData InputData
    {
        get => input;
        set
        {
            if (input == value) 
                return;

            input = value;
            work = new WorkData((int)(input.Width * 1.5), (int)(input.Height * 1.5));
            swapchain = new Swapchain(work.Width, work.Height / 2 + heightExcess);
        }
    }

    InputData input;
    WorkData work;
    Swapchain swapchain;

    public Bitmap Result => swapchain.Result;



    public int RenderTime;
    //settings
    public int ShadowQuality = 1;

    float angle = 45;

    byte heightExcess = 255;

    public bool IsRendering { get; private set; } = false;

    public IsometricRenderer() { }

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

    void DistributeTasks(Action<float, float> func)
    {
        if (cores == 1)
        {
            func(0, 1);
            return;
        }

        Task[] tasks = new Task[(int)cores];

        for (int i = 0; i < cores; i++)
        {
            int tempi = i;
            tasks[i] = Task.Run(() =>
                func(tempi / cores, (tempi + 1) / cores)
            );
        }

        Task.WaitAll(tasks);
    }

    //Prepare the heightMap call rotate and shadow    
    private void Rotate()
    {
        DistributeTasks(Rotate);
    }

    //rotate byte pixel array
    private void Rotate(float start, float end)
    {
        int srcWidth = input.Width, srcHeight = input.Height; 
        int dstWidth = work.Width, dstHeight = work.Height;

        double rad = -(double)(int)angle * Math.PI / 180;
        double sinma = Math.Sin(rad);
        double cosma = Math.Cos(rad);

        var dst = work.Buffer;
        var src = input.Buffer;

        int srcHalfWidth = srcWidth / 2;
        int srcHalfHeight = srcHeight / 2;

        for (int x = (int)(dstWidth * start); x < (int)(dstWidth * end); x++)
        {
            for (int y = 0; y < dstHeight; y++)
            {
                int xt = (int)(x - srcHalfWidth * 1.5);
                int yt = (int)(y - srcHalfHeight * 1.5);

                int xs = (int)((cosma * xt - sinma * yt) + srcHalfWidth);
                int ys = (int)((sinma * xt + cosma * yt) + srcHalfHeight);

                int offsetDst = (x + y * dstWidth);
                int offsetSrc = (xs + ys * srcWidth);
                if (xs >= 0 && xs < srcWidth && ys >= 0 && ys < srcHeight)
                {
                    dst[offsetDst] = src[offsetSrc];
                }
            }
        }
    }

    //add shadows
    private void CalcShadows(int resulution)
    {

        if (resulution == 0) 
            return;

        var buffer = work.Buffer;
        int width = work.Width;
        int height = work.Height;

        for (int iy = 0; iy < height; iy++)//y 0 to 1
        {
            for (int ix = 0; ix < width; ix += resulution)//x 0 to 1
            {
                //get position
                int offset = ix + iy * width;
                int i = 0;

                float shadowHeight = buffer[offset].Height;
                while (buffer[offset].Shadow < shadowHeight)
                {
                    if (i > 0) 
                        buffer[offset].Shadow = (byte)(shadowHeight * 1f);

                    if (buffer[offset].Height > shadowHeight + 1) 
                        break;

                    i++; shadowHeight -= 1f; offset += 1;
                }
            }
        }

    }

    //Rendering the image call elevate
    private void Elevate()
    {
        DistributeTasks(Elevate);
    }

    //Rendering the part of image from heightmap (elevate and apply textures & shadows)
    private unsafe void Elevate(float start, float end)
    {
        var data = swapchain.Data;
        byte* pixels = (byte*)data.Scan0;

        int widthSrc = work.Width, 
            heightSrc = work.Height, 
            widthDst = data.Width, 
            heightDst = data.Height;

        int beginX = (int)(widthSrc * start),
            endX = (int)(heightSrc * end);

        var src = work.Buffer;


        for (int ix = beginX; ix < endX; ix++) //L->R
        {
            for (int iy = (heightSrc - 1); iy >= 0; iy -= 1) //Downwards
                                                             //for (int iy = 0; iy < heightSrc; iy += 1) //Upwards
            {
                //get positions
                int offSrc = (ix + iy * widthSrc);
                int offDst = (ix + iy / 2 * widthDst) * 4;

                int iz = src[offSrc].Height;
                while (iz > 0) //Downwards
                {
                    //save
                    if (iy + heightExcess - iz >= 0)
                    {
                        //get position on z axe
                        int offDstZ = offDst - (widthDst * iz * 4) + widthDst * heightExcess * 4;//pos + curent height

                        //pixel not yet drawn
                        if (pixels[offDstZ + 3] == 0)
                        {
                            //draw pixel
                            float shadow = 1f;
                            if (iz < src[offSrc].Shadow + 1)
                                shadow = 0.75f;

                            var color = input.Textures[src[offSrc].TextureIndex].GetColorAt(iz);
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

                            pixels[offDstZ + 0] = (byte)(color.B * shadow);//b
                            pixels[offDstZ + 1] = (byte)(color.G * shadow);//g
                            pixels[offDstZ + 2] = (byte)(color.R * shadow);//r
                            pixels[offDstZ + 3] = 255;// (byte)(color.A);//a

                        }
                        else
                        {
                            break;
                        }
                    }
                    iz--;
                }
            }


        }
    }

    //Render
    public void Render()
    {
        if (InputData == null)
            throw new InvalidOperationException("No input data given.");

        var sw = Stopwatch.StartNew();

        IsRendering = true;

        work.Clear();

        Rotate();
        CalcShadows(ShadowQuality);
        Elevate();

        swapchain.Swap();

        IsRendering = false;

        RenderTime = (int)sw.ElapsedMilliseconds;
    }
}
