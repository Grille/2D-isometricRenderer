
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grille.Graphics.Isometric.Numerics;

namespace Grille.Graphics.Isometric.Shading;

public unsafe static class DefaultShaders
{
    public static ARGBColor DefaultShader(ShaderArgs args)
    {
        var location = args.Location;
        var cell = args.Cell;
        if (location.Z < cell->ShadowHeight + 1)
        {
            return cell->Color.ApplyShadow(0.75f);
        }
        return cell->Color;
    }

    public static ARGBColor DebugPosition(ShaderArgs args)
    {
        return new ARGBColor((byte)args.Cell->Position.X, (byte)args.Cell->Position.Y, 0);
    }

    public static ARGBColor DebugNormals(ShaderArgs args)
    {
        var normal = args.Cell->Normals;
        return (ARGBColor)normal;
    }
    
}
