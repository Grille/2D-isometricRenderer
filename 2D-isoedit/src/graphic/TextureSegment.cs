using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Program;

public struct TextureSegment
{
    public int A;
    public int R;
    public int G;
    public int B;
    public int Repeat;

    public Color Color
    {
        get => Color.FromArgb(A, R, G, B);
        set
        {
            R = value.R; G = value.G;
            B = value.B; A = value.A;
        }
    }
}
