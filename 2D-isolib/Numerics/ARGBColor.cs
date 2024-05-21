using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
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

    public void Blend(ARGBColor color)
    {
        this = BlendColors(this, color);
    }

    public static ARGBColor BlendColors(ARGBColor color1, ARGBColor color2)
    {
        // Convert alpha from 0-255 to 0-1
        float alpha1 = color1.A / 255f;
        float alpha2 = color2.A / 255f;

        // Calculate the resulting alpha
        float outAlpha = alpha1 + alpha2 * (1 - alpha1);

        // Calculate the resulting color components
        float outR = (color1.R * alpha1 + color2.R * alpha2 * (1 - alpha1)) / outAlpha;
        float outG = (color1.G * alpha1 + color2.G * alpha2 * (1 - alpha1)) / outAlpha;
        float outB = (color1.B * alpha1 + color2.B * alpha2 * (1 - alpha1)) / outAlpha;

        // Convert back to 0-255 range
        int outA = (int)(outAlpha * 255);
        int outRed = (int)(outR);
        int outGreen = (int)(outG);
        int outBlue = (int)(outB);

        return new ARGBColor((byte)outA, (byte)outRed, (byte)outGreen, (byte)outBlue);
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

    public ARGBColor ApplyShading(float shadow)
    {
        return new ARGBColor(A, (byte)(R * shadow), (byte)(G * shadow), (byte)(B * shadow));
    }

    public ARGBColor ApplyShadingClamped(float shadow)
    {
        return new ARGBColor(A, ClampColor(R * shadow), ClampColor(G * shadow), ClampColor(B * shadow));
    }

    public ARGBColor ApplyShadingClamped(Vector3 shadow)
    {
        return new ARGBColor(A, ClampColor(R * shadow.X), ClampColor(G * shadow.Y), ClampColor(B * shadow.Z));
    }

    static byte ClampColor(float color) =>(byte)Math.Clamp(color, 0f, 255f);

    public ARGBColor(byte r, byte g, byte b) :
        this(255, r, g, b)
    { }

    public static explicit operator ARGBColor(S8Vec2 normals) => new ARGBColor((byte)(normals.X + 127), (byte)(normals.Y + 127), 255);

    public static implicit operator ARGBColor(int argb) => Unsafe.As<int, ARGBColor>(ref argb);

    public static implicit operator int(ARGBColor color) => Unsafe.As<ARGBColor, int>(ref color);

    public static implicit operator ARGBColor(Color color) => color.ToArgb();

    public static implicit operator Color(ARGBColor color) => Color.FromArgb(color.ARGB);
}
