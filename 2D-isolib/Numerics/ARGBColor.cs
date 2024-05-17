using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Grille.Graphics.Isometric.Numerics;

[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct ARGBColor
{
    [FieldOffset(0)]
    public int ARGB;
    [FieldOffset(3)]
    public byte A;
    [FieldOffset(2)]
    public byte R;
    [FieldOffset(1)]
    public byte G;
    [FieldOffset(0)]
    public byte B;


    public ARGBColor(byte a, byte r, byte g, byte b)
    {
        A = a;
        R = r;
        G = g;
        B = b;
        Unsafe.SkipInit(out ARGB);
    }

    public static ARGBColor Mix(ARGBColor a, ARGBColor b, float factor)
    {
        // Interpolate between the two colors
        return new ARGBColor(
            (byte)(a.A + (b.A - a.A) * factor),
            (byte)(a.R + (b.R - a.R) * factor),
            (byte)(a.G + (b.G - a.G) * factor),
            (byte)(a.B + (b.B - a.B) * factor)
        );
    }

    public ARGBColor ApplyShadow(float shadow)
    {
        return new ARGBColor(A, (byte)(R * shadow), (byte)(G * shadow), (byte)(B * shadow));
    }

    public ARGBColor(byte r, byte g, byte b) :
        this(255, r, g, b)
    { }

    public static explicit operator ARGBColor(S8Vec2 normals) => new ARGBColor((byte)(normals.X + 127), (byte)(normals.Y + 127), 255);

    public static implicit operator ARGBColor(int argb) => Unsafe.As<int, ARGBColor>(ref argb);

    public static implicit operator int(ARGBColor color) => Unsafe.As<ARGBColor, int>(ref color);

    public static implicit operator ARGBColor(Color color) => color.ToArgb();

    public static implicit operator Color(ARGBColor color) => Color.FromArgb(color.ARGB);
}
