using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Grille.Graphics.Isometric.Shading;

public unsafe struct ShaderUniformObject
{
    public float ZScale;
    public int ZOffset;
    public float YScale;
    public float Angle;
    public Vector2 LightDirection;
}
