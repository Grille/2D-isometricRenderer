using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Grille.Graphics.Isometric.Numerics;

namespace Grille.Graphics.Isometric.Shading;

public unsafe class ShaderProgram
{
    public Action<ShaderArgs> LocationShader { get; }

    public Action<ShaderArgs> PixelShader { get; }

    public bool UsePixelShader { get; }

    public bool UseLocationShader { get; }

    public ShaderProgram() : this(null, null) { }

    public ShaderProgram(Action<ShaderArgs>? pixel) : this(pixel, null) { }

    public ShaderProgram(Action<ShaderArgs>? pixel, Action<ShaderArgs>? height)
    {
        if (UseLocationShader = height != null)
        {
            LocationShader = height!;
        }
        else
        {
            LocationShader = Shaders.RawHeight;
        }

        if (UsePixelShader = pixel != null)
        {
            PixelShader = pixel!;
        }
        else
        {
            PixelShader = Shaders.RawColor;
        }
    }

    public static ShaderProgram Default { get; } = new ShaderProgram();
}
