using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.Graphics.Isometric.Numerics;

public struct S32Vec2
{
    public int X;
    public int Y;

    public S32Vec2()
    {
        X = 0;
        Y = 0;
    }

    public S32Vec2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public S32Vec2 Clamp(int min, int max)
    {
        return new S32Vec2(Math.Clamp(X, min, max), Math.Clamp(Y, min, max));
    }

    public static S32Vec2 operator +(S32Vec2 a, S32Vec2 b) => new S32Vec2(a.X + b.X, a.Y + b.Y);

    public static S32Vec2 operator -(S32Vec2 a, S32Vec2 b) => new S32Vec2(a.X - b.X, a.Y - b.Y);
}
