using Grille.Graphics.Isometric.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Grille.Graphics.Isometric;

public static class InputPreprocessor
{
    public static unsafe void CalculateNormals(this RenderDataBuffer data)
    {
        int GetHeight(int x, int y)
        {
            return data.GetClamped(x, y).Height;
        }

        for (int iy = 0; iy < data.Height; iy++)
        {
            for (int ix = 0; ix < data.Width; ix++)
            {
                float dzdx = (GetHeight(ix + 1, iy) - GetHeight(ix - 1, iy)) / 2.0f;
                float dzdy = (GetHeight(ix, iy + 1) - GetHeight(ix, iy - 1)) / 2.0f;

                Vector3 vec = new Vector3(-dzdx, -dzdy, 1.0f);
                var nvec = Vector3.Normalize(vec);
                sbyte x = (sbyte)(nvec.X * 127f);
                sbyte y = (sbyte)(nvec.Y * 127f);
                var normals = new S8Vec2(x, y);

                data[ix, iy].Normals = normals;
            }
        }
    }
}
