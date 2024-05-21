using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Grille.Graphics.Isometric.Numerics;

namespace Grille.Graphics.Isometric.Buffers;

public unsafe class NativeBuffer<T> : IDisposable where T : unmanaged
{
    private bool disposedValue;

    public readonly T* Pointer;

    public int Width { get; }
    public int Height { get; }
    public int Length { get; }

    public static NativeBuffer<T> Empty { get; } = new NativeBuffer<T>(0, 0);

    public NativeBuffer(int width, int height)
    {
        Width = width;
        Height = height;
        Length = width * height;

        nuint size = (nuint)(Length * sizeof(T));
        Pointer = (T*)NativeMemory.AllocZeroed(size);
    }

    public ref T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref Pointer[index];
    }

    public ref T this[S32Vec2 index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref Pointer[index.X + index.Y * Width];
    }

    public ref T this[int x, int y]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref Pointer[x + y * Width];
    }

    public int GetClampedIndex(int x, int y)
    {
        x = Math.Clamp(x, 0, Width - 1);
        y = Math.Clamp(y, 0, Height - 1);
        return y * Width + x;
    }

    public T* GetClampedPointer(int x, int y)
    {
        int idx = GetClampedIndex(x, y);
        return Pointer + idx;
    }

    public ref T GetClamped(int x, int y)
    {
        int idx = GetClampedIndex(x, y);
        return ref Pointer[idx];
    }

    public NativeBuffer<T> Copy()
    {
        var buffer = new NativeBuffer<T>(Width, Height);
        uint size = (uint)(Length * sizeof(T));
        Unsafe.CopyBlock(Pointer, buffer.Pointer, size);
        return buffer;
    }

    public void Clear()
    {
        new Span<T>(Pointer, Length).Clear();
    }

    void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            NativeMemory.Free(Pointer);

            disposedValue = true;
        }
    }

    public Span<T> AsSpan() => new Span<T>(Pointer, Length);

    ~NativeBuffer()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
