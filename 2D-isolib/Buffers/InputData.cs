using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Grille.Graphics.Isometric.Numerics;

namespace Grille.Graphics.Isometric.Buffers;

[StructLayout(LayoutKind.Explicit, Size = Layout.Size)]
public struct InputData
{
    public static class Layout
    {
        public const int Height = 0;
        public const int Normals = 2;
        public const int Color = 4;
        public const int Size = 8;
    }

    [FieldOffset(Layout.Height)]
    public ushort Height;

    [FieldOffset(Layout.Normals)]
    public S8Vec2 Normals;

    [FieldOffset(Layout.Color)]
    public ARGBColor Color;

}
