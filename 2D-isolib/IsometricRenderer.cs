﻿using System;
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
    NativeBuffer<InputData> _input;
    NativeBuffer<int> _work;

    ParallelExecutor parallel;

    readonly Swapchain swapchain;
    readonly Profiler profiler;

    public NativeBuffer<InputData> Input => _input;

    public ShaderProgram Shader { get; set; }

    public float FrameTime => profiler.FrameTime;
    public float FPS => profiler.FPS;

    ShaderUniformObject _uniforms;

    public ref ShaderUniformObject Uniforms => ref _uniforms;

    //settings
    public int ShadowQuality { get; set; } = 1;

    public bool AutoResizeSwapchain { get; set; } = false;

    int _maxHeight = 255;

    /// <summary>
    /// Max supported height, Set calls <see cref="InputChanged"/> after.
    /// </summary>
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
        set => Uniforms.Angle = value;
        get => Uniforms.Angle;
    }

    public IsometricRenderer(Swapchain swapchain, int workerCount)
    {
        parallel = new ParallelExecutor(workerCount);
        profiler = new Profiler();
        this.swapchain = swapchain;

        Uniforms.ZScale = 1;
        Uniforms.ZOffset = 1;
        Uniforms.YScale = 0.5f;

        Shader = ShaderProgram.Default;
        _input = new(0, 0);
        _work = new(0, 0);
    }

    public void SetInput(NativeBuffer<InputData> buffer, bool copy = true)
    {
        if (copy == true)
        {
            SetInput(buffer.Copy(), false);
            return;
        }

        _input = buffer;

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

    /// <summary>
    /// Informs the renderer that the input may have changed, 
    /// resizes <see cref="Swapchain"/> and internal buffers to accommodate the current settings if the current size is no longer adequate.
    /// </summary>
    public void InputChanged()
    {
        bool rebuildBuffer = false;
        bool resizeSwapchain = false;

        S32Vec2 bufferSize = (S32Vec2)new Vector2(_input.Width * sqrt2, _input.Height * sqrt2);
        S32Vec2 swapchainSize = new S32Vec2(bufferSize.X, (int)(bufferSize.Y * MathF.Abs(Uniforms.YScale)) + _maxHeight);
        if (swapchainSize.X == 0) swapchainSize.X = 1;
        if (swapchainSize.Y == 0) swapchainSize.Y = 1;

        if (_work.Width != bufferSize.X || _work.Height != bufferSize.Y)
            rebuildBuffer = false;

        if (swapchain.ImageWidth != swapchainSize.X || swapchain.ImageHeight != swapchainSize.Y)
            resizeSwapchain = true;

        if (rebuildBuffer)
        {
            lock (_work)
            {
                _work.Dispose();
                _work = new NativeBuffer<int>(bufferSize.X, bufferSize.Y);
            }
        }

        if (resizeSwapchain)
        {
            swapchain.ResizeImages(swapchainSize.X, swapchainSize.Y);
        }
    }

    public void ApplyCamera(Camera camera)
    {
        Uniforms.Angle = camera.Angle;
        Uniforms.YScale = camera.Tilt;
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
        lock (_work)
        {
            parallel.Run(Elevate);
        }
    }

    // Rendering the part of image from heightmap (elevate and apply textures & shadows)
    private unsafe void Elevate(float start, float end)
    {
        int srcWidth = _input.Width, srcHeight = _input.Height;

        float rad = -Angle * MathF.PI / 180f;
        float sinma = MathF.Sin(rad);
        float cosma = MathF.Cos(rad);

        float srcHalfWidth = srcWidth / 2f;
        float srcHalfHeight = srcHeight / 2f;

        float srcFactorWidth = srcHalfWidth * sqrt2;
        float srcFactorHeight = srcHalfHeight * sqrt2;

        var src = _input.Pointer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        int workWidth = (int)(srcWidth * sqrt2),
            workHeight = (int)(srcHeight * sqrt2),
            dstWidth = swapchain.ImageWidth,
            dstHeight = swapchain.ImageHeight;

        int beginX = (int)(workWidth * start),
            endX = (int)(workWidth * end);

        var argsptr = new ShaderArgs.PointerGroup();
        var args = new ShaderArgs();
        args._Ptr = &argsptr;

        int maxHeightOffset = dstWidth * _maxHeight;

        Vector3 location;
        RenderData cell;

        argsptr.Location = &location;
        argsptr.Cell = &cell;

        var shader = Shader;
        var uniforms = _uniforms;

        argsptr.Uniforms = &uniforms;

        bool usingZScale = uniforms.ZScale != 1;

        //for (int iy = 0; iy < heightSrc; iy += 1) //Upwards
        for (int iy = (workHeight - 1) + MaxHeight; iy >= 0; iy -= 1) // Bottom -> Top                                              
        {
            int cy = (int)(iy * Uniforms.YScale) * dstWidth;
            for (int ix = beginX; ix < endX; ix++) // Left -> Right
            {
                if (!Sample(&cell, ix, iy))
                    continue;

                int cx;
                int cz;

                if (shader.UseLocationShader)
                {
                    location = new Vector3(ix, iy, cell.Height);
                    argsptr.Location = &location;
                    shader.LocationShader(args);
                    cy = (int)location.Y * dstWidth;
                    cx = (int)location.X;
                    cz = (int)location.Z;
                }
                else
                {
                    cx = ix;
                    cz = (int)(cell.Height * uniforms.ZScale) + uniforms.ZOffset;
                }

                int offDst = (cx + cy);

                if (cz > _maxHeight)
                    cz = _maxHeight;

                while (cz > 0) // Downwards
                {
                    // get position on z axe
                    int offDstZ = offDst - (dstWidth * cz) + maxHeightOffset;//pos + curent height

                    // pixel not yet drawn
                    if (pixels[offDstZ].A < 255)
                    {
                        location.Z = cz;
                        if (shader.UsePixelShader)
                        {
                            argsptr.Color = pixels + offDstZ;
                            shader.PixelShader(args);
                        }
                        else
                        {
                            pixels[offDstZ] = cell.Color;
                        }
                    }
                    else
                    {
                        break;
                    }

                    cz--;
                }
            }
        }

    }

    // Render
    public void Render()
    {
        if (_input == null)
            throw new InvalidOperationException("No input data given.");

        if (_work == null)
            throw new InvalidOperationException();

        if (Uniforms.YScale < 0)
            throw new InvalidOperationException("Value must be >0.");

        profiler.Begin();

        swapchain.LockActive();
        Elevate();
        swapchain.UnlockActive();

        swapchain.Next();

        profiler.End();
    }
}
