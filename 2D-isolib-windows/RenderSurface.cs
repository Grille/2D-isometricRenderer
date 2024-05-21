using Grille.Graphics.Isometric.Diagnostics;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Numerics;

namespace Grille.Graphics.Isometric.WinForms;

public class RenderSurface : Control
{
    public enum LDownAction
    {
        None,
        Drag,
        Rotate,
    }

    public Profiler Profiler { get; }

    public Camera Camera { get; }

    public BitmapSwapchain Swapchain { get; }

    public IsometricRenderer Renderer { get; }

    public bool DebugInfoEnabled { get; set; } = false;

    public bool DefaultMouseEventsEnabled { get; set; } = true;

    public bool DrawBoundings {  get; set; } = true;

    public LDownAction OnLeftMouseDown { get; set; } = LDownAction.Drag;

    public RenderTimer Timer { get; }

    public MonitorHandle<Bitmap> Image => Swapchain.Image;

    Task renderTaks;

    bool _needRender = true;
    bool _needRefresh = true;
    object _lock = new object();

    public RenderSurface()
    {
        DoubleBuffered = true;

        Profiler = new Profiler();
        Camera = new Camera();

        Swapchain = new BitmapSwapchain(16, 16);
        Renderer = new IsometricRenderer(Swapchain, Environment.ProcessorCount);

        Timer = new RenderTimer(Tick);
        Timer.TargetFPS = 1200;
        

        renderTaks = Task.CompletedTask;
    }

    public void InvalidateRender()
    {
        _needRender = true;
    }

    void Render()
    {
        Renderer.ApplyCamera(Camera);
        Renderer.Render();
        _needRefresh = true;
        Tick();
    }

    private void Tick()
    {
        lock (_lock)
        {
            if (_needRender && renderTaks.IsCompleted)
            {
                renderTaks = Task.Run(Render);
                _needRender = false;
            }

            if (_needRefresh)
            {
                Invoke(Refresh);
                _needRefresh = false;
            }
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        using var handle = Swapchain.Image;
        var bitmap = handle.Value;

        if (bitmap == null)
            return;

        Profiler.Begin();

        Camera.ScreenSize = (Vector2)(SizeF)ClientSize;

        var g = e.Graphics;
        g.InterpolationMode = InterpolationMode.NearestNeighbor;
        g.SmoothingMode = SmoothingMode.None;

        var windowRect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
        var windowBrush = new LinearGradientBrush(windowRect, Color.FromArgb(50, 50, 100), Color.FromArgb(15, 15, 30), LinearGradientMode.Vertical);
        //sg.FillRectangle(windowBrush, windowRect);

        var dstRect = GetScreenSpaceBoundings();
        var srcRect = new RectangleF(0, 0, bitmap.Width, bitmap.Height);

        if (DrawBoundings)
        {
            g.DrawRectangle(Pens.White, dstRect.X, dstRect.Y, dstRect.Width, dstRect.Height);
            _DrawBoundings(g, bitmap.Width / 2f * (1 / 1.41f), Renderer.MaxHeight);
            DrawCoords(g, Vector3.Zero);
        }

        g.DrawImage(bitmap, dstRect, srcRect, GraphicsUnit.Pixel);

        Profiler.End();

        if (DebugInfoEnabled)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Render:");
            sb.AppendLine($"  {Renderer.FPS:F2}fps");
            sb.AppendLine($"  {Renderer.FrameTime:F2}ms");
            sb.AppendLine();
            sb.AppendLine($"Display:");
            sb.AppendLine($"  {Profiler.FPS:F2}fps");
            sb.AppendLine($"  {Profiler.FrameTime:F2}ms");

            var font = new Font("consolas", 11);
            var text = sb.ToString();
            var textsize = g.MeasureString(text, font);
            var textrect = new RectangleF(PointF.Empty, textsize);
            g.FillRectangle(new SolidBrush(Color.FromArgb(128, 0, 0, 0)), textrect);
            g.DrawString(text, font, new SolidBrush(Color.White), textrect);
        }

        base.OnPaint(e);
    }

    public RectangleF GetScreenSpaceBoundings()
    {
        float scale = Camera.Scale;
        var nullPos = Camera.WorldToScreenSpace(Vector2.Zero);
        var drawPos = new PointF(nullPos.X - (Swapchain.ImageWidth / 2) * scale, nullPos.Y - Renderer.MaxHeight * scale - ((Swapchain.ImageHeight - Renderer.MaxHeight) / 2) * scale);
        var dstRect = new RectangleF(drawPos.X, drawPos.Y, Swapchain.ImageWidth * scale, Swapchain.ImageHeight * scale);
        if (dstRect.Y is float.NaN)
            throw new Exception();
        return dstRect;
    }

    void DrawLine3D(System.Drawing.Graphics g, Vector3 pos1, Vector3 pos2)
    {
        DrawLine3D(g, pos1, pos2, Pens.White);
    }

    void DrawLine3D(System.Drawing.Graphics g, Vector3 pos1, Vector3 pos2, Pen pen)
    {
        var tpos1 = Camera.WorldToScreenSpace(pos1);
        var tpos2 = Camera.WorldToScreenSpace(pos2);

        g.DrawLine(pen, (PointF)tpos1, (PointF)tpos2);
    }

    void DrawCoords(System.Drawing.Graphics g, Vector3 pos)
    {
        DrawLine3D(g, pos, pos + new Vector3(1000, 0, 0), Pens.Red);
        DrawLine3D(g, pos, pos + new Vector3(0, 1000, 0), Pens.Blue);
        DrawLine3D(g, pos, pos + new Vector3(0, 0, 1000), Pens.Lime);
    }

    void _DrawBoundings(System.Drawing.Graphics g, float size, float height)
    {
        void DrawQuad(float h)
        {
            DrawLine3D(g, new Vector3(-size, size, h), new Vector3(size, size, h));
            DrawLine3D(g, new Vector3(-size, -size, h), new Vector3(size, -size, h));
            DrawLine3D(g, new Vector3(-size, size, h), new Vector3(-size, -size, h));
            DrawLine3D(g, new Vector3(size, size, h), new Vector3(size, -size, h));
        }

        DrawLine3D(g, new Vector3(size, size, 0), new Vector3(size, size, height));
        DrawLine3D(g, new Vector3(-size, -size, 0), new Vector3(-size, -size, height));
        DrawLine3D(g, new Vector3(-size, size, 0), new Vector3(-size, size, height));
        DrawLine3D(g, new Vector3(size, -size, 0), new Vector3(size, -size, height));

        DrawQuad(0);
        DrawQuad(height);

    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        if (DefaultMouseEventsEnabled)
        {
            Camera.MouseScroll(e, 1.5f);
            _needRefresh = true;
        }

        base.OnMouseWheel(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (DefaultMouseEventsEnabled)
        {
            Camera.MouseMove(e, false);
        }

        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (DefaultMouseEventsEnabled)
        {
            bool move = false;
            bool refresh = false;

            if (e.Button == MouseButtons.Middle)
            {
                move = true;
                refresh = true;
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (OnLeftMouseDown == LDownAction.Drag)
                {
                    move = true;
                    refresh = true;
                }
                else if (OnLeftMouseDown == LDownAction.Rotate)
                {
                    var curPos = Camera.ScreenToWorldSpace((Vector2)(PointF)e.Location, false);
                    var lastPos = Camera.ScreenToWorldSpace(Camera.LastLocation, false);

                    float curAngle = (float)(Math.Atan2(curPos.Y, curPos.X) * (180 / Math.PI));
                    float lastAngle = (float)(Math.Atan2(lastPos.Y, lastPos.X) * (180 / Math.PI));

                    Camera.Angle += curAngle - lastAngle;
                    _needRender = true;

                }
            }

            Camera.MouseMove(e, move);
            _needRefresh |= refresh;
        }

        base.OnMouseMove(e);
    }
}
