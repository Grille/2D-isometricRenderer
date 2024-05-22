using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        public Vector3* Location;
        public RenderData* Cell;
        public ARGBColor* Color;
        public ShaderUniformObject* Uniforms;
        public void* Data;
    }

    internal PointerGroup* _Ptr;

    public Vector3* Location => _Ptr->Location;
    public RenderData* Cell => _Ptr->Cell;
    public ARGBColor* Color => _Ptr->Color;
    public ShaderUniformObject* Uniforms => _Ptr->Uniforms;
    public void* Data => _Ptr->Data;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyColorToOutput()
    {
        *Color = Cell->Color;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyLightToOutput()
    {
        ApplyLightToOutput(Uniforms->LightDirection);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyLightToOutput(Vector2 light)
    {
        var normal = Cell->Normals.ToVector2();

        float dotProduct = Vector2.Dot(normal, light);
        float shading = dotProduct * 0.5f + 1f;

        *Color = Color->ApplyShadingClamped(shading);
    }
}

