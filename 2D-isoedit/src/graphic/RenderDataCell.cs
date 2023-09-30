using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using Program.src.graphic;

namespace Program;

[StructLayout(LayoutKind.Explicit)]
public struct RenderDataCell
{
    public static class Layout
    {
        public const int Height = 0;
        public const int Shadow = 2;
        public const int TextureIndex = 4;
        public const int Color = 8;
        public const int DebugColor = 14;
        public const int HeightDifference= 12;
    }

    [FieldOffset(Layout.Height)]
    public ushort Height;

    [FieldOffset(Layout.Shadow)]
    public ushort ShadowHeight;

    [FieldOffset(Layout.TextureIndex)]
    public byte TextureIndex;

    [FieldOffset(Layout.Color)]
    public ARGBColor Color;

    [FieldOffset(Layout.DebugColor)]
    public ARGBColor DebugColor;

    [FieldOffset(Layout.HeightDifference)]
    public ushort HeightDifference;
}
