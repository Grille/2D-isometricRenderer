using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program;

public struct IVector2
{
    public int X;
    public int Y;

    public IVector2()
    {
        X = 0;
        Y = 0;
    }

    public IVector2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public IVector2 Clamp(int min, int max)
    {
        return new IVector2(Math.Clamp(X, min, max), Math.Clamp(Y, min, max));
    }

    public static IVector2 operator +(IVector2 a, IVector2 b) => new IVector2(a.X + b.X, a.Y + b.Y);
    
    public static IVector2 operator -(IVector2 a, IVector2 b) => new IVector2(a.X - b.X, a.Y - b.Y);
}
