using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grille.Graphics.Isometric.WinForms;

public static class CameraExtension
{
    public static void MouseScroll(this Camera camera, MouseEventArgs e, float scrollFactor)
    {
        camera.MouseScroll(e.Delta > 0, scrollFactor);
    }

    public static void MouseMove(this Camera camera, MouseEventArgs e, bool move)
    {
        var location = new Vector2(e.Location.X, e.Location.Y);
        camera.MouseMove(location, move);
    }
}