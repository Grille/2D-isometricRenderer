using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;

using System.Numerics;
using System.Runtime.CompilerServices;

using Grille.Graphics.Isometric.Numerics;

namespace Grille.Graphics.Isometric;

[StructLayout(LayoutKind.Explicit, Size = 16)]
public struct RenderData
{
    public static class Layout
    {
        public const int Height = 0;
        public const int Shadow = 2;
        public const int Color = 4;
        public const int Normals = 8;
        public const int Position = 12;
    }

    [FieldOffset(Layout.Height)]
    public ushort Height;

    [FieldOffset(Layout.Shadow)]
    public ushort ShadowHeight;

    [FieldOffset(Layout.Color)]
    public ARGBColor Color;

    [FieldOffset(Layout.Normals)]
    public S8Vec2 Normals;

    [FieldOffset(Layout.Position)]
    public U16Vec2 Position;
}
