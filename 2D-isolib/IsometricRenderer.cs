using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using System.Drawing.Imaging;

using System.Numerics;
using Grille.Graphics.Isometric.Shading;
using Grille.Graphics.Isometric.Numerics;
using System.Runtime.CompilerServices;
using Grille.Graphics.Isometric.Diagnostics;

namespace Grille.Graphics.Isometric;

public unsafe class IsometricRenderer
{
    //Tasks
    RenderDataBuffer input;
    RenderDataBuffer work;

    ParallelExecutor parallel;

    readonly Swapchain swapchain;
    readonly Profiler profiler;

    public Func<ShaderArgs, ARGBColor> PixelShader { get; set; }

    public float FrameTime => profiler.FrameTime;
    public float FPS => profiler.FPS;

    //settings
    public int ShadowQuality { get; set; } = 1;

    float angle = 45;

    bool angleChanged = false;

    int heightExcess = 255;

    int _workerCount;

    public int WorkerCount
    {
        get => _workerCount; set
        {
            if (_workerCount == value)
                return;
            _workerCount = value;
            parallel.Dispose();
            parallel = new ParallelExecutor(value);
        }
    }

    public IsometricRenderer(Swapchain swapchain, int workerCount)
    {
        parallel = new ParallelExecutor(workerCount);
        profiler = new Profiler();
        this.swapchain = swapchain;


        PixelShader = DefaultShaders.DefaultShader;

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

        PixelShader = new((args) =>
        {
            var ground = groundbake[args.Cell->Height];
            var wall = wallbake[args.Cell->Height];
            var color = ARGBColor.Mix(ground, wall, Math.Clamp(args.Cell->HeightDifference / 6f, 0f, 1f));// = new ARGBColor(0, location.Z % 16 < 1 ? (byte)255 : (byte)0, (byte)(location.Z));

            if (args.Location.Z < args.Cell->ShadowHeight + 1)
            {
                return color.ApplyShadow(0.75f);
            }
            return color;
        });
        */


    }

    public void SetInput(RenderDataBuffer buffer, bool copy = true)
    {
        if (copy == true)
        {
            SetInput(buffer.Copy(), false);
            return;
        }

        input = buffer;


        if (work != null)
        {
            work.Dispose();
        }
        work = new RenderDataBuffer((int)(input.Width * 1.5), (int)(input.Height * 1.5));
        swapchain.ResizeImages(work.Width, work.Height / 2 + heightExcess);
    }

    public void SetInput(RenderData[,] array)
    {
        var width = array.GetLength(0);
        var height = array.GetLength(1);
        var buffer = new RenderDataBuffer(width, height);
        for (int ix = 0; ix < width; ix++)
        {
            for (int iy = 0; iy < height; iy++)
            {
                buffer[ix,iy] = array[ix, iy];
            }
        }
        SetInput(buffer, false);
    }

    public float Angle
    {
        set
        {
            if (angle == value) return;
            angle = value;
            if (angle <= 0) angle += 360;
            else if (angle >= 360) angle -= 360;
            angleChanged = true;
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

        float rad = -angle * MathF.PI / 180f;
        float sinma = MathF.Sin(rad);
        float cosma = MathF.Cos(rad);

        float srcHalfWidth = srcWidth / 2f;
        float srcHalfHeight = srcHeight / 2f;

        int ixStart = (int)(dstWidth * start);
        int ixEnd = (int)(dstWidth * end);
        int ixCount = ixEnd - ixStart;
        int ixSkip = dstWidth - ixCount;

        var dst = work.Pointer;
        var src = input.Pointer;

        var dstPtr = dst + ixStart;

        for (int y = 0; y < dstHeight; y++)
        {
            for (int x = ixStart; x < ixEnd; x++)
            {
                dstPtr += 1;

                float xt = x - srcHalfWidth * 1.5f;
                float yt = y - srcHalfHeight * 1.5f;

                int xs = (int)((cosma * xt - sinma * yt) + srcHalfWidth);
                int ys = (int)((sinma * xt + cosma * yt) + srcHalfHeight);

                int offsetSrc = (xs + ys * srcWidth);
                if (xs >= 0 && xs < srcWidth && ys >= 0 && ys < srcHeight)
                {
                    var srcPtr = src + offsetSrc;
                    Unsafe.CopyBlock(dstPtr, srcPtr, 10);
                    dstPtr->Position = new U16Vec2((ushort)xs, (ushort)ys);
                }
            }

            dstPtr += ixSkip;
        }

    }

    // Add shadows
    private void CalcShadows(int resolution)
    {

        if (resolution == 0) 
            return;

        var buffer = work.Pointer;
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
        var pixels = swapchain.ImageData;

        int widthSrc = work.Width, 
            heightSrc = work.Height, 
            widthDst = swapchain.ImageWidth, 
            heightDst = swapchain.ImageHeight;

        int beginX = (int)(widthSrc * start),
            endX = (int)(heightSrc * end);

        var src = work.Pointer;

        var args = new ShaderArgs();


        //for (int iy = 0; iy < heightSrc; iy += 1) //Upwards
        for (int iy = (heightSrc - 1); iy >= 0; iy -= 1) // Bottom -> Top                                              
        {
            args.Location.Y = iy;
            for (int ix = beginX; ix < endX; ix++) // Left -> Right
            {
                args.Location.X = ix;
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
                            args.Location.Z = iz;
                            args.Cell = src + offSrc;

                            pixels[offDstZ] = PixelShader(args);
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
        if (input == null)
            throw new InvalidOperationException("No input data given.");

        if (work == null)
            throw new InvalidOperationException();

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
