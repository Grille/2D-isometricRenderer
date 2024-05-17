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

/// <summary>-</summary>
namespace Program;


//[System.ComponentModel.DesignerCategory("code")]
public partial class FormEditor : Form
{
    const string ImageFilter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.bmp;*.jpg;*.gif;*.png|All files (*.*)|*.*";

    //Graphic
    RenderDataBuffer inputData;
    IsometricRenderer renderer;
    Camera camera;
    Profiler profilerTimer;

    //setings
    private SettingsFile settings;

    public FormEditor()
    {
        Icon = Properties.Resources.Cube;

        Console.WriteLine("Start");
        InitializeComponent();

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

        inputData = BitmapInputData.FromBitmap(settings.DefaultMap);
        renderer = pBResult.Renderer;

        renderer.SetInput(inputData, false);


        Console.WriteLine("Init Renderer");
        renderTimer.Start();
        pBResult.InvalidateRender();

        pBResult.Timer.Interval = 1;
        pBResult.Timer.Start();
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
            TextBoxRotate.Text = renderer.Angle.ToString();
        }

        if (autoRotateToolStripMenuItem.Checked)
        {
            renderer.Angle += (profilerTimer.Delta / 64);
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
        if (toolStripMenuItem1.Checked)
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

    private void openHighMapToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.InitialDirectory = Path.GetFullPath(settings.DirectorySave);
        dialog.Filter = "IsoHightMap(*.IHM)|*.ihm|All files (*.*)|*.*";
        dialog.FileOk += new CancelEventHandler((object csender, CancelEventArgs ce) =>
        {
            /*
            ByteStream bs = new ByteStream(dialog.FileName);
            bs.ReadByte();
            Program.MainForm.renderer.Data = new RenderData(bs.ReadInt(), bs.ReadInt());
            //Program.MainForm.renderer.Data.HeightMap = bs.ReadByteArray();
            //Program.MainForm.renderer.Data.TextureMap = bs.ReadByteArray();
            Program.MainForm.Repainting = true;
            settings.DirectorySave = Path.GetDirectoryName(dialog.FileName);
            */
        });
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {

        }
    }

    private void saveRenderToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var dialog = new SaveFileDialog();
        dialog.InitialDirectory = Path.GetFullPath(settings.DirectorySave);
        dialog.AddExtension = true;
        dialog.DefaultExt = "ihm";
        dialog.Filter = "IsoHightMap(*.IHM)|*.ihm|All files (*.*)|*.*";
        dialog.FileOk += new CancelEventHandler((object csender, CancelEventArgs ce) =>
        {
            /*
            ByteStream bs = new ByteStream();
            bs.WriteByte(0);
            bs.WriteInt(Program.MainForm.renderer.Data.Width);
            bs.WriteInt(Program.MainForm.renderer.Data.Height);
            //bs.WriteByteArray(Program.MainForm.renderer.Data.HeightMap,CompressMode.Auto);
            //bs.WriteByteArray(Program.MainForm.renderer.Data.TextureMap, CompressMode.Auto);
            bs.Save(dialog.FileName);
            settings.DirectorySave = Path.GetDirectoryName(dialog.FileName);
            */
        });
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {

        }
        //ByteStream bs = new ByteStream();
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

    private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
    {
        FormTextureEditor form = new FormTextureEditor();
        form.Show(this);
    }

    private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        FormRenderSettings form = new FormRenderSettings();
        form.Show(this);
    }

    private void toolboxToolStripMenuItem_Click(object sender, EventArgs e)
    {
        FormToolbox form = new FormToolbox();
        form.Show(this);
    }

    private void toolStripButtonDrag_Click(object sender, EventArgs e)
    {
        pBResult.OnLeftMouseDown = RenderSurface.LDownAction.Drag;
        toolStripButtonRotate.Checked = false;
    }

    private void toolStripButtonRotate_Click(object sender, EventArgs e)
    {
        pBResult.OnLeftMouseDown = RenderSurface.LDownAction.Rotate;
        toolStripButtonDrag.Checked = false;
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

    private void ImportMenuItemClick(object sender, EventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.InitialDirectory = Path.GetFullPath(settings.DirectoryImport);
        dialog.Filter = ImageFilter;

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            inputData = BitmapInputData.FromBitmap(dialog.FileName);
            renderer.SetInput(inputData, false);
            settings.DirectoryImport = Path.GetDirectoryName(dialog.FileName);
            pBResult.InvalidateRender();
        }
    }

    private void ImportTextureMenuItemClick(object sender, EventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.InitialDirectory = Path.GetFullPath(settings.DirectoryImport);
        dialog.Filter = ImageFilter;

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            inputData.LoadTextureBitmap(dialog.FileName);
            settings.DirectoryImport = Path.GetDirectoryName(dialog.FileName);
            pBResult.InvalidateRender();
        }
    }

    private void ExportMenuItemClick(object sender, EventArgs e)
    {
        var dialog = new SaveFileDialog();
        dialog.InitialDirectory = Path.GetFullPath(settings.DirectoryExport);
        dialog.AddExtension = true;
        dialog.Filter = ImageFilter;
        dialog.DefaultExt = ".png";

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            using var handle = pBResult.Image;
            var bitmap = handle.Value;

            var extension = Path.GetExtension(dialog.FileName).ToLower();
            var format = extension switch
            {
                ".bmp" => ImageFormat.Bmp,
                ".jpg" => ImageFormat.Jpeg,
                ".jpeg" => ImageFormat.Jpeg,
                ".gif" => ImageFormat.Gif,
                ".png" => ImageFormat.Png,
                _ => ImageFormat.Png,
            };

            bitmap.Save(dialog.FileName, format);
            settings.DirectoryExport = Path.GetDirectoryName(dialog.FileName);
        }
    }
    #endregion

    private void DebugMenuItemClick(object sender, EventArgs e)
    {
        pBResult.DebugInfoEnabled = debugToolStripMenuItem.Checked;
        pBResult.Invalidate();
    }

    private void ButtonReset_Click(object sender, EventArgs e)
    {
        camera.Position = PointF.Empty;
        renderer.Angle = 45;
        pBResult.InvalidateRender();
    }

    private void TextBoxRotate_TextChanged(object sender, EventArgs e)
    {
        if (float.TryParse(TextBoxRotate.Text, out float angle))
        {
            if (renderer.Angle != angle)
            {
                TextBoxRotate.ForeColor = Color.Blue;
                renderer.Angle = angle;
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
        renderer.Angle += 45;
        pBResult.InvalidateRender();
    }

    private void ButtonLeft_Click(object sender, EventArgs e)
    {
        renderer.Angle -= 45;
        pBResult.InvalidateRender();
    }
}
