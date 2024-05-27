using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

using Grille.Graphics.Isometric.Diagnostics;
using System.Drawing.Drawing2D;

namespace Grille.Graphics.Isometric.WinForms;

public class GdiRenderer
{
    System.Drawing.Graphics? g;

    /// <summary>
    /// Information displayed in top left corner.
    /// </summary>
    public StringBuilder Info { get; }

    public Font Font { get; set; }

    public Camera Camera { get; }

    public GdiRenderer(Camera camera)
    {
        Info = new StringBuilder();

        Font = new Font("consolas", 11);

        Camera = camera;
    }

    public void UseGraphics(System.Drawing.Graphics graphics)
    {
        g = graphics;
        g.InterpolationMode = InterpolationMode.NearestNeighbor;
        g.SmoothingMode = SmoothingMode.None;
    }

    public void DrawImage(Bitmap bitmap, float offsetY)
    {
        AssertGraphics();

        if (bitmap == null)
            return;

        var dstRect = GetScreenSpaceBoundings(bitmap, offsetY);
        var srcRect = new RectangleF(0, 0, bitmap.Width, bitmap.Height);

        g.DrawImage(bitmap, dstRect, srcRect, GraphicsUnit.Pixel);
    }

    public void DrawLine3D(Vector3 pos1, Vector3 pos2)
    {
        DrawLine3D(pos1, pos2, Pens.White);
    }

    public void DrawLine3D(Vector3 pos1, Vector3 pos2, Pen pen)
    {
        AssertGraphics();

        var tpos1 = Camera.WorldToScreenSpace(pos1);
        var tpos2 = Camera.WorldToScreenSpace(pos2);

        g.DrawLine(pen, (PointF)tpos1, (PointF)tpos2);
    }

    public void DrawCoords(Vector3 pos, float size = 100)
    {
        var posX = pos + new Vector3(size, 0, 0);
        var posY = pos + new Vector3(0, size, 0);
        var posZ = pos + new Vector3(0, 0, size);

        DrawLine3D(pos, posX, Pens.Red);
        DrawLine3D(pos, posY, Pens.Blue);
        DrawLine3D(pos, posZ, Pens.Lime);

        DrawText("X", Brushes.Red, posX);
        DrawText("Y", Brushes.Blue, posY);
        DrawText("Z", Brushes.Lime, posZ);
    }

    public void FillCircle(float radius, Brush brush, Vector2 position)
    {
        FillCircle(radius, brush, new Vector3(position.X, position.Y, 0));
    }

    public void FillCircle(float radius, Brush brush, Vector3 position)
    {
        AssertGraphics();

        var pos = Camera.WorldToScreenSpace(position);
        var rect = new RectangleF(pos.X - radius, pos.Y - radius, radius * 2, radius * 2);
        g.FillEllipse(brush, rect);
    }

    public void DrawText(string text, Brush brush, Vector3 position)
    {
        AssertGraphics();

        var pos = Camera.WorldToScreenSpace(position);
        g.DrawString(text, Font, brush, (PointF)pos);
    }

    public void DrawBoundings(float size, float height)
    {
        var begin = new Vector3(-size, -size, 0);
        var end = new Vector3(size, size, height);

        DrawBoundings(begin, end);
    }

    public void DrawBoundings(Vector3 begin, Vector3 end)
    {
        void DrawRectangle(float z)
        {
            void DrawLine(float x1, float y1, float x2, float y2)
            {
                DrawLine3D(new Vector3(x1, y1, z), new Vector3(x2, y2, z));
            }
            DrawLine(begin.X, begin.Y, end.X, begin.Y);
            DrawLine(end.X, begin.Y, end.X, end.Y);
            DrawLine(end.X, end.Y, begin.X, end.Y);
            DrawLine(begin.X, end.Y, begin.X, begin.Y);
        }
        DrawRectangle(begin.Z);
        DrawRectangle(end.Z);
        void DrawLine(float x, float y)
        {
            DrawLine3D(new Vector3(x, y, begin.Z), new Vector3(x, y, end.Z));
        }
        DrawLine(begin.X, begin.Y);
        DrawLine(end.X, begin.Y);
        DrawLine(end.X, end.Y);
        DrawLine(begin.X, end.Y);
    }

    public RectangleF GetScreenSpaceBoundings(Bitmap bitmap, float offsetY)
    {
        float scale = Camera.Scale;
        var nullPos = Camera.WorldToScreenSpace(Vector2.Zero);
        var drawPos = new PointF(nullPos.X - (bitmap.Width / 2) * scale, nullPos.Y - offsetY * scale - ((bitmap.Height - offsetY) / 2) * scale);
        var dstRect = new RectangleF(drawPos.X, drawPos.Y, bitmap.Width * scale, bitmap.Height * scale);
        if (dstRect.Y is float.NaN)
            throw new Exception();
        return dstRect;
    }

    public void DrawInfo()
    {
        var text = Info.ToString();
        var textsize = g.MeasureString(text, Font);
        var textrect = new RectangleF(PointF.Empty, textsize);
        g.FillRectangle(new SolidBrush(Color.FromArgb(128, 0, 0, 0)), textrect);
        g.DrawString(text, Font, new SolidBrush(Color.White), textrect);

        Info.Clear();
    }


    [MemberNotNull(nameof(g))]
    void AssertGraphics()
    {
        if (g == null)
        {
            throw new InvalidOperationException();
        }
    }
}
