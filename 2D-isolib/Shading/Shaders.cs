
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grille.Graphics.Isometric.Numerics;

namespace Grille.Graphics.Isometric.Shading;

public unsafe static class Shaders
{
    public static void RawColor(ShaderArgs args)
    {
        *args.Color = args.Cell->Color;
    }

    public static void AlphaBlend(ShaderArgs args)
    {
        args.Color->Blend(args.Cell->Color);
    }

    public static void HeightShading(ShaderArgs args)
    {
        var location = args.Location;
        var cell = args.Cell;
        /*
        if (location.Z < cell->Data + 1)
        {
            *args.Color = cell->Color.ApplyShading(0.75f);
        }
        *args.Color = cell->Color;
        **/
    }

    public static void NormalShading(ShaderArgs args)
    {
        var cell =  args.Cell;

        var shading = cell->Normals.X / 255f + 1f;

        *args.Color = cell->Color.ApplyShadingClamped(shading);

    }

    public static void DebugPosition(ShaderArgs args)
    {
        *args.Color = new ARGBColor((byte)args.Cell->Position.X, (byte)args.Cell->Position.Y, 0);
    }

    public static void DebugNormals(ShaderArgs args)
    {
        var normal = args.Cell->Normals;
        *args.Color = (ARGBColor)normal;
    }

    public static void DebugHeight(ShaderArgs args)
    {
        var height = args.Cell->Height;
        *args.Color = new ARGBColor((byte)(height*10), (byte)(height * 5), 255);
    }

    public static ushort RawHeight(ushort args)
    {
        return args;
    }

}
