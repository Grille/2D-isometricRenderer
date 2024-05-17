using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grille.Graphics.Isometric.Numerics;

namespace Grille.Graphics.Isometric.Shading;

public unsafe struct ShaderArgs
{
    public S32Vec3 Location;
    public RenderData* Cell;
}

