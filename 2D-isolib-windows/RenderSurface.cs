using Grille.Graphics.Isometric.Diagnostics;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

    public LDownAction OnLeftMouseDown { get; set; } = LDownAction.Drag;

    public Timer Timer { get; }

    public MonitorHandle<Bitmap> Image => Swapchain.Image;

    Task renderTaks;

    Task timerTask;

    bool _needRender = true;
    bool _needRefresh = true;

    public RenderSurface()
    {
        DoubleBuffered = true;

        Profiler = new Profiler();
        Camera = new Camera();

        Swapchain = new BitmapSwapchain(16, 16);
        Renderer = new IsometricRenderer(Swapchain, Environment.ProcessorCount);

        Timer = new Timer()
        {
            Interval = 10
        };

        renderTaks = Task.CompletedTask;

        Timer.Tick += Tick;
    }

    public void InvalidateRender()
    {
        _needRender = true;
    }

    void Render()
    {
        Renderer.Render();
        _needRefresh = true;
        Invoke(() => Tick(null, null!));
    }

    private void Tick(object? sender, EventArgs e)
    {
        if (_needRender && renderTaks.IsCompleted)
        {
            renderTaks = Task.Run(Render);
            _needRender = false;
        }

        if (_needRefresh)
        {
            Refresh();
            _needRefresh = false;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        using var handle = Swapchain.Image;
        var bitmap = handle.Value;

        if (bitmap == null)
            return;

        Profiler.Begin();

        Camera.ScreenSize = ClientSize;

        var g = e.Graphics;
        g.InterpolationMode = InterpolationMode.NearestNeighbor;
        g.SmoothingMode = SmoothingMode.None;

        var windowRect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
        var windowBrush = new LinearGradientBrush(windowRect, Color.FromArgb(50, 50, 100), Color.FromArgb(15, 15, 30), LinearGradientMode.Vertical);
        //sg.FillRectangle(windowBrush, windowRect);

        float scale = Camera.Scale;
        var nullPos = Camera.WorldToScreenSpace(PointF.Empty);
        var drawPos = new PointF(nullPos.X - (bitmap.Width / 2) * scale, nullPos.Y - 255 * scale - ((bitmap.Height - 255) / 2) * scale);

        var dstRect = new RectangleF(drawPos.X, drawPos.Y, bitmap.Width * scale, bitmap.Height * scale);
        var srcRect = new RectangleF(0, 0, bitmap.Width, bitmap.Height);

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

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        if (DefaultMouseEventsEnabled)
        {
            Camera.MouseScrollEvent(e, 1.5f);
            _needRefresh = true;
        }

        base.OnMouseWheel(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (DefaultMouseEventsEnabled)
        {
            Camera.MouseMoveEvent(e, false);
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
                    var curPos = Camera.ScreenToWorldSpace(e.Location);
                    var lastPos = Camera.ScreenToWorldSpace(Camera.LastLocation);

                    float curAngle = (float)(Math.Atan2(curPos.Y * 2, curPos.X) * (180 / Math.PI));
                    float lastAngle = (float)(Math.Atan2(lastPos.Y * 2, lastPos.X) * (180 / Math.PI));

                    Renderer.Angle += curAngle - lastAngle;
                    _needRender = true;

                }
            }

            Camera.MouseMoveEvent(e, move);
            _needRefresh |= refresh;
        }

        base.OnMouseMove(e);
    }
}
