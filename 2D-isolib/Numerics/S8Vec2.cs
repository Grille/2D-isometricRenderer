using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Grille.Graphics.Isometric.Numerics;

[StructLayout(LayoutKind.Explicit)]
public struct S8Vec2
{
    public S8Vec2(sbyte x, sbyte y)
    {
        X = x; Y = y;
        Unsafe.SkipInit(out XY);
    }

    [FieldOffset(0)]
    public short XY;

    [FieldOffset(0)]
    public sbyte X;

    [FieldOffset(1)]
    public sbyte Y;
}