using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grille.Graphics.Isometric.Numerics;

namespace Grille.Graphics.Isometric.Shading;

public class ShaderProgram
{
    public bool EnableHeightShadows { get; init; }

    public bool EnabledRecalcNormalsAfterRotation { get; init; }

    public Func<ushort, ushort> HeightShader { get; }

    public Action<ShaderArgs> PixelShader { get; }

    public ShaderProgram(Func<ushort, ushort> height, Action<ShaderArgs> pixel)
    {
        HeightShader = height;
        PixelShader = pixel;
    }

    public static ShaderProgram Default { get; } = new ShaderProgram(Shaders.RawHeight, Shaders.RawColor);
}
