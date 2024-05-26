using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using System.Drawing.Drawing2D;
using System.Reflection;
using System.Reflection.Metadata;

using Grille.Graphics.Isometric;
using Grille.Graphics.Isometric.Diagnostics;
using Grille.Graphics.Isometric.WinForms;
using Grille.Graphics.Isometric.Shading;
using System.Numerics;
using Grille.Graphics.Isometric.Buffers;
using Grille.Graphics.Isometric.Numerics;
using System.Globalization;

/// <summary>-</summary>
namespace Program;


//[System.ComponentModel.DesignerCategory("code")]
public partial class FormEditor : Form
{

    //Graphic
    NativeBuffer<InputData> inputData;
    IsometricRenderer renderer;
    Camera camera;
    Profiler profilerTimer;

    //setings
    private SettingsFile settings;

    Dictionary<string, ShaderProgram> _shaders;

    float _scale = 1;

    public FormEditor()
    {
        Console.WriteLine("Start");
        InitializeComponent();

        _shaders = new Dictionary<string, ShaderProgram>
        {
            { "Dynamic Shading", new ShaderProgram(Shaders.DynamicShading) },
            { "Fixed Shading", new ShaderProgram(Shaders.FixedShading) },
            { "Debug_Normals", new ShaderProgram(Shaders.DebugNormals) },
            { "Debug_Position", new ShaderProgram(Shaders.DebugPosition) },
            { "Debug_Height", new ShaderProgram(Shaders.DebugHeight) },
            { "Alpha Blend", new ShaderProgram(Shaders.AlphaBlend) },
            { "Raw Color", new ShaderProgram() },
        };

        pBResult.FancyBackgroundEnabled = false;
        pBResult.DrawBoundings = false;
        pBResult.DebugInfoEnabled = true;

        Icon = Properties.Resources.Cube;

        settings = new SettingsFile();
        try
        {
            settings.Load("config.ini");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        Width = settings.WindowWidth;
        Height = settings.WindowHeight;
        Fullscreen(settings.Fullscreen);

        profilerTimer = new Profiler();

        camera = pBResult.Camera;


        renderer = pBResult.Renderer;
        renderer.Shader = _shaders["Dynamic Shading"];

        Console.WriteLine("Init Renderer");
        renderTimer.Start();

        if (File.Exists(settings.DefaultMap))
        {
            LoadBitmap(settings.DefaultMap);
        }
        else
        {
            inputData = new(16, 16);
        }

        pBResult.InvalidateRender();
    }

    private unsafe void ScaleHeight(ShaderArgs args)
    {
        args.Location->Z *= _scale;
    }

    //Draw rendered image
    private void pBRender_Paint(object sender, PaintEventArgs e)
    {
        toolStripStatusLabelRenderTime.Text = $"RenderTime {renderer.FrameTime:F2}ms";
    }

    //RenderLoop & AutoRotate
    private void renderTimer_Tick(object sender, EventArgs e)
    {
        profilerTimer.Log();

        if (!TextBoxRotate.Focused)
        {
            TextBoxRotate.Text = camera.Angle.ToString();
        }

        if (!TextBoxTilt.Focused)
        {
            TextBoxTilt.Text = camera.Tilt.ToString();
        }

        if (autoRotateToolStripMenuItem.Checked)
        {
            camera.Angle += (profilerTimer.Delta / 64);
            pBResult.InvalidateRender();
        }
    }

    #region GUI events
    private void FormEditor_Resize(object sender, EventArgs e)
    {
        pBResult.Invalidate();
    }

    private void fgdToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void fullscrenToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Fullscreen(FormBorderStyle != FormBorderStyle.None);
    }

    private void quitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void toolStripMenuItem1_CheckedChanged(object sender, EventArgs e)
    {
        if (toolStripMenuItemShaders.Checked)
        {
            renderer.ShadowQuality = 1;
            pBResult.InvalidateRender();
        }
        else
        {
            renderer.ShadowQuality = 0;
            pBResult.InvalidateRender();
        }
    }

    private void FormEditor_KeyUp(object sender, KeyEventArgs e)
    {

    }

    private void FormEditor_KeyDown(object sender, KeyEventArgs e)
    {
        switch (e.KeyData)
        {
            case Keys.F5:
                pBResult.InvalidateRender();
                break;
        }
    }

    private void toolStripButtonDrag_Click(object sender, EventArgs e)
    {
        SetToolStripButton(sender, RenderSurface.LDownAction.DragView);
    }

    private void toolStripButtonRotate_Click(object sender, EventArgs e)
    {
        SetToolStripButton(sender, RenderSurface.LDownAction.RotateRender);
    }

    private void toolStripButtonLight_Click(object sender, EventArgs e)
    {
        SetToolStripButton(sender, RenderSurface.LDownAction.DragLight);
    }

    private void toolStripButtonTilt_Click(object sender, EventArgs e)
    {
        SetToolStripButton(sender, RenderSurface.LDownAction.Tilt);
    }

    void SetToolStripButton(object sender, RenderSurface.LDownAction action)
    {
        void SetIfEqual(ToolStripButton target)
        {
            target.Checked = target == sender;
        }

        pBResult.OnLeftMouseDown = action;

        SetIfEqual(toolStripButtonRotate);
        SetIfEqual(toolStripButtonDrag);
        SetIfEqual(toolStripButtonLight);
        SetIfEqual(toolStripButtonTilt);

        pBResult.Invalidate();
    }

    public void Fullscreen(bool fullscreen)
    {
        if (fullscreen)
        {
            FormBorderStyle = FormBorderStyle.None;
            if (WindowState == FormWindowState.Maximized)
                WindowState = FormWindowState.Normal;
            WindowState = FormWindowState.Maximized;
            fullscrenToolStripMenuItem.Checked = true;
        }
        else
        {
            FormBorderStyle = FormBorderStyle.Sizable;
            WindowState = FormWindowState.Normal;
            fullscrenToolStripMenuItem.Checked = false;
        }
    }

    private void ImportMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            if (ImageFileDialog.TryOpenImage(this, out string path))
            {
                LoadBitmap(path);

                pBResult.InvalidateRender();
            }
        }
        catch (Exception ex)
        {
            ExceptionDialog.Show(this, ex);
        }
    }

    private void ImportTextureMenuItem_Click(object sender, EventArgs e)
    {
        if (ImageFileDialog.TryOpenImage(this, out string path))
        {
            LoadTextureBitmap(path);
            pBResult.InvalidateRender();
        }
    }

    private void ImportNormalsMenuItem_Click(object sender, EventArgs e)
    {
        if (ImageFileDialog.TryOpenImage(this, out string path))
        {
            LoadNormalBitmap(path);
            pBResult.InvalidateRender();
        }
    }

    void LoadBitmap(string path)
    {
        try
        {
            var old = inputData;
            inputData = BitmapInputData.FromHeightBitmap(path);
            renderer.SetInput(inputData, false);
            old?.Dispose();

            var extension = Path.GetExtension(path);
            var basepath = Path.ChangeExtension(path, null);

            var texturePath = $"{basepath}_texture{extension}";
            var normalPath = $"{basepath}_normals{extension}";

            if (File.Exists(texturePath))
                LoadTextureBitmap(texturePath);

            if (File.Exists(normalPath))
                LoadNormalBitmap(normalPath);
            else
                inputData.CalculateNormals();
        }
        catch (Exception ex)
        {
            ExceptionDialog.Show(this, ex);
        }
    }

    void LoadTextureBitmap(string path)
    {
        try
        {
            inputData.LoadTextureBitmap(path);
            pBResult.InvalidateRender();
        }
        catch (Exception ex)
        {
            ExceptionDialog.Show(this, ex);
        }
    }

    void LoadNormalBitmap(string path)
    {
        try
        {
            inputData.LoadNormalBitmap(path);
            pBResult.InvalidateRender();
        }
        catch (Exception ex)
        {
            ExceptionDialog.Show(this, ex);
        }
    }

    private void ExportMenuItem_Click(object sender, EventArgs e)
    {
        using var handle = pBResult.Image;
        var image = handle.Value;
        ImageFileDialog.SaveImage(this, image);
    }

    private void ExportNormalsMenuItem_Click(object sender, EventArgs e)
    {
        using var normalmap = inputData.NormalDataToBitmap();
        ImageFileDialog.SaveImage(this, normalmap);
    }
    #endregion

    private void DebugMenuItemClick(object sender, EventArgs e)
    {
        pBResult.DebugInfoEnabled = debugToolStripMenuItem.Checked;
        pBResult.Invalidate();
    }

    private void ButtonReset_Click(object sender, EventArgs e)
    {
        camera.Position = Vector2.Zero;
        camera.Angle = 45;
        camera.Tilt = 0.5f;
        pBResult.InvalidateRender();
    }

    private void TextBoxRotate_TextChanged(object sender, EventArgs e)
    {
        if (float.TryParse(TextBoxRotate.Text, out float angle))
        {
            if (camera.Angle != angle)
            {
                TextBoxRotate.ForeColor = Color.Blue;
                camera.Angle = angle;
                pBResult.InvalidateRender();
            }
            else
            {
                TextBoxRotate.ForeColor = Color.Black;
            }
        }
        else
        {
            TextBoxRotate.ForeColor = Color.Red;
        }
    }

    private void ButtonRight_Click(object sender, EventArgs e)
    {
        camera.Angle += 45;
        pBResult.InvalidateRender();
    }

    private void ButtonLeft_Click(object sender, EventArgs e)
    {
        camera.Angle -= 45;
        pBResult.InvalidateRender();
    }

    private void FormEditor_Load(object sender, EventArgs e)
    {
        pBResult.Timer.Start();

        var node = toolStripMenuItemShaders;
        var items = node.DropDownItems;

        void handler(object obj, EventArgs args)
        {
            var item = (ToolStripMenuItem)obj;
            foreach (var sobj in items)
            {
                var sitem = (ToolStripMenuItem)sobj;
                sitem.Checked = false;
            }
            item.Checked = true;

            var text = item.Text;

            var shader = _shaders[text];
            renderer.Shader = shader;

            pBResult.InvalidateRender();
        }


        bool first = true;
        foreach (var shader in _shaders)
        {
            var item = new ToolStripMenuItem(shader.Key, null, handler);
            item.Checked = first;
            item.Image = node.Image;
            items.Add(item);
            first = false;
        }
    }

    private void TextBoxTilt_TextChanged(object sender, EventArgs e)
    {
        if (TryGetNumberFromToolStripTextBox(sender, out float value))
        {
            camera.Tilt = value;
            pBResult.InvalidateRender();
        }
    }

    private void TextBoxHeight_TextChanged(object sender, EventArgs e)
    {
        if (TryGetNumberFromToolStripTextBox(sender, out float value))
        {
            renderer.MaxHeight = (int)value;
            pBResult.InvalidateRender();
        }
    }

    private void TextBoxScaling_TextChanged(object sender, EventArgs e)
    {
        if (TryGetNumberFromToolStripTextBox(sender, out float value))
        {
            _scale = value;
            renderer.Uniforms.ZScale = value;
            pBResult.InvalidateRender();
        }
    }

    bool TryGetNumberFromToolStripTextBox(object sender, out float value)
    {
        var control = (ToolStripTextBox)sender;
        var text = control.Text;

        if (float.TryParse(text, out value))
        {
            if (value >= 0)
            {
                control.ForeColor = Color.Black;
                return true;
            }
        }
        control.ForeColor = Color.Red;
        return false;
    }

    private void rAToolStripMenuItem_Click(object sender, EventArgs e)
    {
        pBResult.RenderAllways = rAToolStripMenuItem.Checked;
    }

    private void boundingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        pBResult.DrawBoundings = boundingsToolStripMenuItem.Checked;
        pBResult.Invalidate();
    }
}
