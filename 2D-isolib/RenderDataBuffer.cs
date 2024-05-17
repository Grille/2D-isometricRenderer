using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Grille.Graphics.Isometric.Numerics;

namespace Grille.Graphics.Isometric;

public unsafe class RenderDataBuffer : IDisposable
{
    private bool disposedValue;

    public readonly RenderData* Pointer;

    public int Width { get; }
    public int Height { get; }
    public int Length { get; }

    public static RenderDataBuffer Empty { get; } = new RenderDataBuffer(0, 0);

    public RenderDataBuffer(int width, int height)
    {
        Width = width;
        Height = height;
        Length = width * height;

        nuint size = (nuint)(Length * sizeof(RenderData));
        Pointer = (RenderData*)NativeMemory.AllocZeroed(size);
    }

    public ref RenderData this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref Pointer[index];
    }

    public ref RenderData this[S32Vec2 index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref Pointer[index.X + index.Y * Width];
    }

    public ref RenderData this[int x, int y]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref Pointer[x + y * Width];
    }

    public ref RenderData GetClamped(int x, int y)
    {
        x = Math.Clamp(x, 0, Width - 1);
        y = Math.Clamp(y, 0, Height - 1);
        int idx = y * Width + x;
        return ref Pointer[idx];
    }

    public RenderDataBuffer Copy()
    {
        var buffer = new RenderDataBuffer(Width, Height);
        uint size = (uint)(Length * sizeof(RenderData));
        Unsafe.CopyBlock(Pointer, buffer.Pointer, size);
        return buffer;
    }

    public void Clear()
    {
        new Span<RenderData>(Pointer, Length).Clear();
    }

    void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            NativeMemory.Free(Pointer);

            disposedValue = true;
        }
    }

    ~RenderDataBuffer()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
