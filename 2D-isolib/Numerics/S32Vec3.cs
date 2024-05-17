using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Grille.Graphics.Isometric.Numerics;

public struct S32Vec3
{
    public int X;
    public int Y;
    public int Z;

    public S32Vec3()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }

    public S32Vec3(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}
