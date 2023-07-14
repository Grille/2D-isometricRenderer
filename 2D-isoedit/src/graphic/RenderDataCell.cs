using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Program
{
    public struct RenderDataCell
    {
        public byte Height;
        public byte Shadow;
        public byte TextureIndex;
        public byte R;
        public byte G;
        public byte B;
        public byte A;

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
}
