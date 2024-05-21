using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Grille.Graphics.Isometric.Numerics;

[StructLayout(LayoutKind.Explicit)]
public struct S8Vec2
{
    public static S8Vec2 FromBytes(byte x, byte y)
    {
        return new S8Vec2((sbyte)(x - 127), (sbyte)(y - 127));
    }

    public static S8Vec2 FromVector2(Vector2 vector)
    {
        return new S8Vec2((sbyte)(vector.X * 127f), (sbyte)(vector.Y * 127f));
    }

    public S8Vec2(sbyte x, sbyte y)
    {
        X = x; Y = y;
        Unsafe.SkipInit(out XY);
    }

    const float mul = 1f / 127f;
    public Vector2 ToVector2() => new Vector2(X * mul, Y * mul);

    [FieldOffset(0)]
    public short XY;

    [FieldOffset(0)]
    public sbyte X;

    [FieldOffset(1)]
    public sbyte Y;
}