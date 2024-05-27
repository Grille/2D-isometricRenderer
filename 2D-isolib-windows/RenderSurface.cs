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
using Grille.Graphics.Isometric.Shading;
using static System.Windows.Forms.DataFormats;

namespace Grille.Graphics.Isometric.WinForms;

public class RenderSurface : Control
{
    public enum LDownAction
    {
        None,
        DragView,
        DragLight,
        RotateRender,
        Tilt,
    }

    public GdiRenderer GdiRenderer { get; }

    public Profiler Profiler { get; }

    public Camera Camera { get; }

    public BitmapSwapchain Swapchain { get; }

    public IsometricRenderer Renderer { get; }

    public bool DebugInfoEnabled { get; set; } = false;

    public bool DefaultMouseEventsEnabled { get; set; } = true;

    public bool DrawBoundings {  get; set; } = true;

    public bool FancyBackgroundEnabled { get; set; } = false;

    public LDownAction OnLeftMouseDown { get; set; } = LDownAction.DragView;

    public RenderTimer Timer { get; }

    public float LightAngle { get; set; }

    public ShaderProgram DemoShader { get; }

    public bool RenderAllways { get; set; }

    public MonitorHandle<Bitmap> Image => Swapchain.Image;

    Task renderTaks;

    bool _needRender = true;
    bool _needRefresh = true;
    object _lock = new object();

    public RenderSurface()
    {
        DoubleBuffered = true;

        DemoShader = new ShaderProgram(Shaders.DynamicShading);

        Profiler = new Profiler();
        Camera = new Camera();
        GdiRenderer = new GdiRenderer(Camera);

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
        float angle = (LightAngle - Camera.Angle) * MathF.PI / 180f;
        var lightDirection = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        Renderer.Uniforms.LightDirection = lightDirection;
        Renderer.ApplyCamera(Camera);
        Renderer.Render();
        _needRefresh = true;
        Tick();
    }

    private void Tick()
    {
        lock (_lock)
        {
            _needRender |= RenderAllways;

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
        var g = e.Graphics;
        g.InterpolationMode = InterpolationMode.NearestNeighbor;
        g.SmoothingMode = SmoothingMode.None;

        Camera.ScreenSize = (Vector2)(SizeF)ClientSize;
        GdiRenderer.UseGraphics(e.Graphics);

        Profiler.Begin();

        if (FancyBackgroundEnabled)
        {
            var windowRect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            var windowBrush = new LinearGradientBrush(windowRect, Color.FromArgb(50, 50, 100), Color.FromArgb(15, 15, 30), LinearGradientMode.Vertical);
            g.FillRectangle(windowBrush, windowRect);
        }

        if (DrawBoundings)
        {
            GdiRenderer.DrawBoundings(Swapchain.ImageWidth / 2f * (1 / 1.41f), Renderer.MaxHeight);
            GdiRenderer.DrawCoords(Vector3.Zero);
        }

        if (OnLeftMouseDown == LDownAction.DragLight)
        {
            var lightPos = Camera.Rotate(new Vector2(Swapchain.ImageWidth / 2f + 100f, 0), LightAngle - Camera.Angle);
            GdiRenderer.DrawLine3D(Vector3.Zero, new Vector3(lightPos.X, lightPos.Y, 0), new Pen(Color.Yellow, 4f));
            GdiRenderer.FillCircle(10, Brushes.LightYellow, lightPos);
        }

        using (var handle = Swapchain.Image)
        {
            var bitmap = handle.Value;
            GdiRenderer.DrawImage(bitmap, Renderer.MaxHeight);
        }

        Profiler.End();

        if (DebugInfoEnabled)
        {
            var sb = GdiRenderer.Info;
            sb.AppendLine($"Render:");
            sb.AppendLine($"  {Renderer.FPS:F2}fps");
            sb.AppendLine($"  {Renderer.FrameTime:F2}ms");
            sb.AppendLine();
            sb.AppendLine($"Display:");
            sb.AppendLine($"  {Profiler.FPS:F2}fps");
            sb.AppendLine($"  {Profiler.FrameTime:F2}ms");
        }

        base.OnPaint(e);

        GdiRenderer.DrawInfo();
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
                if (OnLeftMouseDown == LDownAction.DragView)
                {
                    move = true;
                    refresh = true;
                }
                else if (OnLeftMouseDown == LDownAction.RotateRender)
                {
                    Camera.Angle += Camera.MouseMoveAngleAroundOrigin((Vector2)(PointF)e.Location);
                    _needRender = true;

                }
                else if (OnLeftMouseDown == LDownAction.DragLight)
                {
                    LightAngle = Camera.AngleFromScreenPosition((Vector2)(PointF)e.Location);
                    _needRender = true;
                }
                else if (OnLeftMouseDown == LDownAction.Tilt)
                {
                    var dist = ((e.Location.Y - Camera.LastLocation.Y) / 600) / Camera.Scale;
                    Camera.Tilt = Math.Clamp(Camera.Tilt + dist, 0, 1);
                    _needRender = true;
                }
            }

            Camera.MouseMove(e, move);
            _needRefresh |= refresh;
        }

        base.OnMouseMove(e);
    }
}
