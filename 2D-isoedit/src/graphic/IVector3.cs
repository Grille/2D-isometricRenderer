using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program;

public struct IVector3
{
    public int X;
    public int Y;
    public int Z;

    public IVector3()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }

    public IVector3(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}
