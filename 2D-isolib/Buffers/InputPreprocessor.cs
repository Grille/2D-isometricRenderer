using Grille.Graphics.Isometric.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Grille.Graphics.Isometric.Buffers;

public static class InputPreprocessor
{
    public static unsafe void CalculateNormals<T>(this NativeBuffer<T> data) where T : unmanaged
    {
        InputData* Access(int x, int y) => (InputData*)data.GetClampedPointer(x, y);

        int GetHeight(int x, int y) => Access(x, y)->Height;

        for (int iy = 0; iy < data.Height; iy++)
        {
            for (int ix = 0; ix < data.Width; ix++)
            {
                var left = GetHeight(ix - 1, iy);
                var right = GetHeight(ix + 1, iy);
                var top = GetHeight(ix, iy - 1);
                var bottom = GetHeight(ix, iy + 1);

                float dzdx = (right - left) / 2.0f;
                float dzdy = (bottom - top) / 2.0f;

                Vector3 vec = new Vector3(-dzdx, -dzdy, 1f);
                var nvec = Vector3.Normalize(vec);
                sbyte x = (sbyte)(nvec.X * 127f);
                sbyte y = (sbyte)(nvec.Y * 127f);
                var normals = new S8Vec2(x, y);

                Access(ix, iy)->Normals = normals;
            }
        }
    }
}
