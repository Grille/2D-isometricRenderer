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

namespace Grille.Graphics.Isometric.Buffers;

[StructLayout(LayoutKind.Explicit, Size = Layout.Size)]
public struct RenderData
{
    public static class Layout
    {
        public const int Height = InputData.Layout.Height;
        public const int Normals = InputData.Layout.Normals;
        public const int Color = InputData.Layout.Color;
        public const int Position = InputData.Layout.Size;
        public const int Data = InputData.Layout.Size + 4;
        public const int Size = 16;
    }

    [FieldOffset(Layout.Height)]
    public ushort Height;

    [FieldOffset(Layout.Normals)]
    public S8Vec2 Normals;

    [FieldOffset(Layout.Color)]
    public ARGBColor Color;

    [FieldOffset(Layout.Position)]
    public U16Vec2 Position;

    [FieldOffset(Layout.Data)]
    public uint Data;
}
