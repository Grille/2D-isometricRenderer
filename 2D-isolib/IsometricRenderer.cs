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
using Grille.Graphics.Isometric.Buffers;

namespace Grille.Graphics.Isometric;

public unsafe class IsometricRenderer
{
    //Tasks
    NativeBuffer<InputData> input;
    NativeBuffer<int> work;

    ParallelExecutor parallel;

    readonly Swapchain swapchain;
    readonly Profiler profiler;

    public ShaderProgram Shader { get; set; }

    public float FrameTime => profiler.FrameTime;
    public float FPS => profiler.FPS;

    //settings
    public int ShadowQuality { get; set; } = 1;

    float _tilt = 0.5f;

    float _angle = 45;

    int _maxHeight = 255;

    public int MaxHeight
    {
        get => _maxHeight;
        set
        {
            _maxHeight = value;
            InputChanged();
        }
    }

    int _workerCount;

    static readonly float sqrt2 = MathF.Sqrt(2);

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

    public float Angle
    {
        set
        {
            if (_angle == value) return;
            _angle = value;
            if (_angle <= 0) _angle += 360;
            else if (_angle >= 360) _angle -= 360;
        }
        get
        {
            return _angle;
        }
    }

    public float Tilt
    {
        get => _tilt;
        set
        {
            _tilt = value;
            InputChanged();
        }
    }


    public IsometricRenderer(Swapchain swapchain, int workerCount)
    {
        parallel = new ParallelExecutor(workerCount);
        profiler = new Profiler();
        this.swapchain = swapchain;

        Shader = ShaderProgram.Default;
        input = new(0, 0);
        work = new(0, 0);
    }

    public void SetInput(NativeBuffer<InputData> buffer, bool copy = true)
    {
        if (copy == true)
        {
            SetInput(buffer.Copy(), false);
            return;
        }

        input = buffer;

        InputChanged();
    }

    public void SetInput(InputData[,] array)
    {
        var width = array.GetLength(0);
        var height = array.GetLength(1);
        var buffer = new NativeBuffer<InputData>(width, height);
        for (int ix = 0; ix < width; ix++)
        {
            for (int iy = 0; iy < height; iy++)
            {
                buffer[ix,iy] = array[ix, iy];
            }
        }
        SetInput(buffer, false);
    }

    public void InputChanged()
    {
        bool rebuildBuffer = false;
        bool resizeSwapchain = false;

        S32Vec2 bufferSize = (S32Vec2)new Vector2(input.Width * sqrt2, input.Height * sqrt2);
        S32Vec2 swapchainSize = new S32Vec2(bufferSize.X, (int)(bufferSize.Y * MathF.Abs(_tilt)) + _maxHeight);
        if (swapchainSize.X == 0) swapchainSize.X = 1;
        if (swapchainSize.Y == 0) swapchainSize.Y = 1;

        if (work.Width != bufferSize.X || work.Height != bufferSize.Y)
            rebuildBuffer = true;

        if (swapchain.ImageWidth != swapchainSize.X || swapchain.ImageHeight != swapchainSize.Y)
            resizeSwapchain = true;

        if (rebuildBuffer)
        {
            lock (work)
            {
                work.Dispose();
                work = new NativeBuffer<int>(bufferSize.X, bufferSize.Y);
            }
        }

        if (resizeSwapchain)
        {
            swapchain.ResizeImages(swapchainSize.X, swapchainSize.Y);
        }
    }

    public void ApplyCamera(Camera camera)
    {
        _angle = camera.Angle;
        _tilt = camera.Tilt;
        InputChanged();
    }
    /*
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

                float shadowHeight = Shader.HeightShader(buffer[offset].Height);
                while (buffer[offset].Data < shadowHeight)
                {
                    if (i > 0) 
                        buffer[offset].Data = (byte)(shadowHeight * 1f);

                    if (Shader.HeightShader(buffer[offset].Height) > shadowHeight + 1) 
                        break;

                    i++; shadowHeight -= 1f; offset += 1;
                }
            }
        }

    }
    */

    // Rendering the image
    private void Elevate()
    {
        lock (work)
        {
            parallel.Run(Elevate);
        }
    }

    // Rendering the part of image from heightmap (elevate and apply textures & shadows)
    private unsafe void Elevate(float start, float end)
    {
        int srcWidth = input.Width, srcHeight = input.Height;

        float rad = -_angle * MathF.PI / 180f;
        float sinma = MathF.Sin(rad);
        float cosma = MathF.Cos(rad);

        float srcHalfWidth = srcWidth / 2f;
        float srcHalfHeight = srcHeight / 2f;

        float srcFactorWidth = srcHalfWidth * sqrt2;
        float srcFactorHeight = srcHalfHeight * sqrt2;

        var src = input.Pointer;

        bool Sample(RenderData* cell, int x, int y)
        {
            float xt = x - srcFactorWidth;
            float yt = y - srcFactorHeight;

            int xs = (int)((cosma * xt - sinma * yt) + srcHalfWidth);
            int ys = (int)((sinma * xt + cosma * yt) + srcHalfHeight);

            int offsetSrc = (xs + ys * srcWidth);
            if (xs >= 0 && xs < srcWidth && ys >= 0 && ys < srcHeight)
            {
                var srcPtr = src + offsetSrc;

                Unsafe.CopyBlock(cell, srcPtr, InputData.Layout.Size);
                cell->Position = new U16Vec2((ushort)xs, (ushort)ys);

                return true;
            }
            return false;
        }

        var pixels = swapchain.ImageData;

        int workWidth = work.Width, 
            workHeight = work.Height, 
            dstWidth = swapchain.ImageWidth, 
            dstHeight = swapchain.ImageHeight;

        int beginX = (int)(workWidth * start),
            endX = (int)(workHeight * end);

        var argsptr = new ShaderArgs.PointerGroup();
        var args = new ShaderArgs();
        args._Ptr = &argsptr;

        int maxHeightOffset = dstWidth * _maxHeight;

        S32Vec3 location;
        RenderData cell;

        argsptr.Location = &location;
        argsptr.Cell = &cell;

        //for (int iy = 0; iy < heightSrc; iy += 1) //Upwards
        for (int iy = (workHeight - 1); iy >= 0; iy -= 1) // Bottom -> Top                                              
        {
            location.Y = iy;
            for (int ix = beginX; ix < endX; ix++) // Left -> Right
            {
                location.X = ix;
                // get positions
                int offSrc = (ix + iy * workWidth);
                int offDst = (ix + (int)(iy * Tilt) * dstWidth);

                if (!Sample(&cell, ix, iy))
                    continue;

                int iz = Shader.HeightShader(cell.Height);
                if (iz > _maxHeight)
                    iz = _maxHeight;
                while (iz > 0) // Downwards
                {
                    // get position on z axe
                    int offDstZ = offDst - (dstWidth * iz) + maxHeightOffset;//pos + curent height

                    // pixel not yet drawn
                    if (pixels[offDstZ].A < 255)
                    {
                        location.Z = iz;
                        argsptr.Color = pixels + offDstZ;
                        pixels[offDstZ].A = 255;
                        Shader.PixelShader(args);
                    }
                    else
                    {
                        break;
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

        if (Tilt < 0)
            throw new InvalidOperationException("Value must be >0.");


        profiler.Begin();

        //work.Clear();

        //Rotate();

        if (Shader.EnabledRecalcNormalsAfterRotation)
            work.CalculateNormals();

        //if (Shader.EnableHeightShadows)
        //    CalcShadows(ShadowQuality);

        swapchain.LockActive();
        Elevate();
        swapchain.UnlockActive();

        swapchain.Next();

        profiler.End();
    }
}
