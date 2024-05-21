using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Grille.Graphics.Isometric.Buffers;
using Grille.Graphics.Isometric.Numerics;

namespace Grille.Graphics.Isometric.Shading;

public unsafe struct ShaderArgs
{
    internal struct PointerGroup
    {
        public S32Vec3* Location;
        public RenderData* Cell;
        public ARGBColor* Color;
    }

    internal PointerGroup* _Ptr;

    public S32Vec3* Location => _Ptr->Location;
    public RenderData* Cell => _Ptr->Cell;
    public ARGBColor* Color => _Ptr->Color;
}

