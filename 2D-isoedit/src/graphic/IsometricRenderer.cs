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
using Program.src.graphic;
using System.Numerics;

namespace Program;

public unsafe class IsometricRenderer
{
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
    Profiler profiler;

    ParallelExecutor parallel;

    public Func<IVector3, RenderDataCell, ARGBColor> PixelShader { get; set; }

    public MonitorHandle<Bitmap> Result => swapchain.Result;
    public float FrameTime => profiler.FrameTime;
    public float FPS => profiler.FPS;

    SemaphoreSlim semaphore;

    //settings
    public int ShadowQuality { get; set; } = 1;

    float angle = 45;

    int heightExcess = 255;

    public IsometricRenderer() {
        parallel = new ParallelExecutor(Environment.ProcessorCount);
        profiler = new Profiler();
        PixelShader = new((location, cell) =>
        {
            if (location.Z < cell.ShadowHeight + 1)
            {
                return cell.Color.ApplyShadow(0.75f);
            }
            return cell.Color;
        });

        /*
        var groundcurve = new ColorCurve();
        groundcurve.Add(0.0f, new ARGBColor(90,117,55));
        groundcurve.Add(0.3f, new ARGBColor(93, 65, 45));
        groundcurve.Add(0.5f, new ARGBColor(70, 65, 45));
        groundcurve.Add(0.6f, new ARGBColor(227, 243, 255));
        groundcurve.Add(1.0f, new ARGBColor(255, 255, 255));

        var wallcurve = new ColorCurve();
        wallcurve.Add(0.0f, new ARGBColor(100,100,100));
        wallcurve.Add(1.0f, new ARGBColor(40,40,40));

        var groundbake = groundcurve.Bake(255);
        var wallbake = wallcurve.Bake(255);

        PixelShader = new((location, cell) =>
        {
            var ground = groundbake[cell.Height];
            var wall = wallbake[cell.Height];
            var color = ARGBColor.Mix(ground, wall, Math.Clamp(cell.HeightDifference / 6f, 0f, 1f));// = new ARGBColor(0, location.Z % 16 < 1 ? (byte)255 : (byte)0, (byte)(location.Z));

            if (location.Z < cell.ShadowHeight + 1)
            {
                return color.ApplyShadow(0.75f);
            }
            return color;
        });
        */
        
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


    // Rotate byte pixel array
    private void Rotate()
    {
        parallel.Run(Rotate);
    }

    // Rotate part of the byte pixel array
    private void Rotate(float start, float end)
    {
        int srcWidth = input.Width, srcHeight = input.Height; 
        int dstWidth = work.Width, dstHeight = work.Height;

        double rad = -angle * Math.PI / 180;
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

    // Add shadows
    private void CalcShadows(int resolution)
    {

        if (resolution == 0) 
            return;

        var buffer = work.Buffer;
        int width = work.Width;
        int height = work.Height;

        for (int iy = 0; iy < height; iy++)//y 0 to 1
        {
            for (int ix = 0; ix < width; ix += resolution)//x 0 to 1
            {
                //get position
                int offset = ix + iy * width;
                int i = 0;

                float shadowHeight = buffer[offset].Height;
                while (buffer[offset].ShadowHeight < shadowHeight)
                {
                    if (i > 0) 
                        buffer[offset].ShadowHeight = (byte)(shadowHeight * 1f);

                    if (buffer[offset].Height > shadowHeight + 1) 
                        break;

                    i++; shadowHeight -= 1f; offset += 1;
                }
            }
        }

    }

    // Rendering the image
    private void Elevate()
    {
        parallel.Run(Elevate);
    }

    // Rendering the part of image from heightmap (elevate and apply textures & shadows)
    private unsafe void Elevate(float start, float end)
    {
        var data = swapchain.Data;
        var pixels = (ARGBColor*)data.Scan0;

        int widthSrc = work.Width, 
            heightSrc = work.Height, 
            widthDst = data.Width, 
            heightDst = data.Height;

        int beginX = (int)(widthSrc * start),
            endX = (int)(heightSrc * end);

        var src = work.Buffer;

        for (int ix = beginX; ix < endX; ix++) // Left -> Right
        {
            for (int iy = (heightSrc - 1); iy >= 0; iy -= 1) // Bottom -> Top
                                                             //for (int iy = 0; iy < heightSrc; iy += 1) //Upwards
            {
                // get positions
                int offSrc = (ix + iy * widthSrc);
                int offDst = (ix + iy / 2 * widthDst);

                int iz = src[offSrc].Height;
                while (iz > 0) // Downwards
                {
                    // save
                    if (iy + heightExcess - iz >= 0)
                    {
                        // get position on z axe
                        int offDstZ = offDst - (widthDst * iz) + widthDst * heightExcess;//pos + curent height

                        // pixel not yet drawn
                        if (pixels[offDstZ].A == 0)
                        {
                            var location = new IVector3(ix, iy, iz);

                            pixels[offDstZ] = PixelShader(location, src[offSrc]);
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

    // Render
    public void Render()
    {
        if (InputData == null)
            throw new InvalidOperationException("No input data given.");

        profiler.Begin();

        work.Clear();

        Rotate();
        CalcShadows(ShadowQuality);

        swapchain.LockActive();
        Elevate();
        swapchain.UnlockActive();

        swapchain.Next();

        profiler.End();
    }
}
