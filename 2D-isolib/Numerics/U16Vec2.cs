using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.Graphics.Isometric.Numerics;

public struct U16Vec2
{
    public ushort X;
    public ushort Y;

    public U16Vec2()
    {
        X = 0;
        Y = 0;
    }

    public U16Vec2(ushort x, ushort y)
    {
        X = x;
        Y = y;
    }

    public U16Vec2 Clamp(ushort min, ushort max)
    {
        return new U16Vec2(Math.Clamp(X, min, max), Math.Clamp(Y, min, max));
    }
}
