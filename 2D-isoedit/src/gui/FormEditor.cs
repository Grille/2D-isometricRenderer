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
/// <summary>-</summary>
namespace Program;


//[System.ComponentModel.DesignerCategory("code")]
public partial class FormEditor : Form
{
    const string ImageFilter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.bmp;*.jpg;*.gif;*.png|All files (*.*)|*.*";

    //Graphic
    IsometricRenderer renderer;
    Camera camera;
    Profiler profiler;
    Profiler profilerTimer;

    Task task;

    //Rendering Values
    public bool NeedRender = false;
    public bool NeedRefresh = false;

    //setings
    private SettingsFile settings;

    //Editor Values
    bool curTextureEdit;

    public FormEditor()
    {
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

        task = Task.CompletedTask;

        profiler = new Profiler();
        profilerTimer = new Profiler();

        camera = new Camera();

        var textures = TexturePack.FromFile(settings.DefaultTexture);
        var input = new InputData(settings.DefaultMap)
        {
            Textures = textures
        };

        renderer = new IsometricRenderer()
        {
            InputData = input,
        };

        Console.WriteLine("Init Renderer");
        renderTimer.Start();
        NeedRender = true;
    }

    //Draw rendered image
    private void pBRender_Paint(object sender, PaintEventArgs e)
    {
        using var handle = renderer.Result;
        var bitmap = handle.Value;

        if (bitmap == null)
            return;

        profiler.Begin();

        camera.ScreenSize = pBResult.Size;

        var g = e.Graphics;
        g.InterpolationMode = InterpolationMode.NearestNeighbor;
        g.SmoothingMode = SmoothingMode.None;

        var windowRect = new Rectangle(0, 0, pBResult.Width, pBResult.Height);
        var windowBrush = new LinearGradientBrush(windowRect, Color.FromArgb(50, 50, 100), Color.FromArgb(15, 15, 30), LinearGradientMode.Vertical);
        //sg.FillRectangle(windowBrush, windowRect);

        float scale = camera.Scale;
        var nullPos = camera.WorldToScreenSpace(PointF.Empty);
        var drawPos = new PointF(nullPos.X - (bitmap.Width / 2) * scale, nullPos.Y - 255 * scale - ((bitmap.Height - 255) / 2) * scale);

        var dstRect = new RectangleF(drawPos.X, drawPos.Y, bitmap.Width * scale, bitmap.Height * scale);
        var srcRect = new RectangleF(0, 0, bitmap.Width, bitmap.Height);

        g.DrawImage(bitmap, dstRect, srcRect, GraphicsUnit.Pixel);

        profiler.End();

        if (debugToolStripMenuItem.Checked)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Render:");
            sb.AppendLine($"  {renderer.FPS:F2}fps");
            sb.AppendLine($"  {renderer.FrameTime:F2}ms");
            sb.AppendLine();
            sb.AppendLine($"Display:");
            sb.AppendLine($"  {profiler.FPS:F2}fps");
            sb.AppendLine($"  {profiler.FrameTime:F2}ms");

            var font = new Font("consolas", 11);
            var text = sb.ToString();
            var textsize = g.MeasureString(text, font);
            var textrect = new RectangleF(PointF.Empty, textsize);
            g.FillRectangle(new SolidBrush(Color.FromArgb(128, 0, 0, 0)), textrect);
            g.DrawString(text, font, new SolidBrush(Color.White), textrect);
        }


        //g.DrawLine(new Pen(Color.FromArgb(100, Color.Red), 2), new PointF(drawPos.X, drawPos.Y-100), new PointF(drawPos.X, drawPos.Y + 100));
        //g.DrawLine(new Pen(Color.FromArgb(100, Color.Red), 2), new PointF(drawPos.X- (result.Width/2) * camScale, drawPos.Y), new PointF(drawPos.X+ (result.Width / 2) * camScale, drawPos.Y));
        //g.DrawLine(new Pen(Color.FromArgb(100, Color.Lime), 2), new PointF(drawPos.X - 100, drawPos.Y - 50), new PointF(drawPos.X + 100, drawPos.Y + 50));

        toolStripStatusLabelRenderTime.Text = "RenderTime " + renderer.FrameTime + "ms";
    }

    //Render Image
    private void Render()
    {
        renderer.Render();
        NeedRefresh = true;
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
            NeedRender = true;
            NeedRefresh = true;
        }

        if (NeedRender && task.IsCompleted)
        {
            task = Task.Run(Render);
            NeedRender = false;
        }

        if (NeedRefresh)
        {
            pBResult.Refresh();
            NeedRefresh = false;
        }
    }

    #region GUI events
    private void pBRender_MouseMove(object sender, MouseEventArgs e)
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
            if (toolStripButtonDrag.Checked)
            {
                move = true;
                refresh = true;
            }
            else if (toolStripButtonRotate.Checked)
            {

                var curPos = camera.ScreenToWorldSpace(e.Location);
                var lastPos = camera.ScreenToWorldSpace(camera.LastLocation);

                float curAngle = (float)(Math.Atan2(curPos.Y * 2, curPos.X) * (180 / Math.PI));
                float lastAngle = (float)(Math.Atan2(lastPos.Y * 2, lastPos.X) * (180 / Math.PI));

                renderer.Angle += curAngle - lastAngle;
                NeedRender = true;

            }
        }

        camera.MouseMoveEvent(e, move);
        NeedRefresh |= refresh;
    }
    private void pBRender_MouseWheel(object sender, MouseEventArgs e)
    {
        camera.MouseScrollEvent(e, 1.5f);

        NeedRefresh = true;
    }

    //Buttons
    private void bRotL_Click(object sender, EventArgs e)
    {
        renderer.Angle -= 45;
        NeedRender = true;
    }
    private void bRotR_Click(object sender, EventArgs e)
    {
        renderer.Angle += 45;
        NeedRender = true;
    }
    private void bRot_Click(object sender, EventArgs e)
    {
        renderer.Angle = 0;
        NeedRender = true;
    }

    private void bClose_Click(object sender, EventArgs e)
    {
        this.Close();
    }
    private void bNew_Click(object sender, EventArgs e)
    {
        /*
        //inputLB = new LockBitmap(new Bitmap("../input/test_256x256.png"), false);
        inputLB = new LockBitmap(new Bitmap(128,128), false);
        result.Map = renderer.Render();
        renderTimer.Enabled = true;
        */
    }
    private void bSwitch_Click(object sender, EventArgs e)
    {
        curTextureEdit = !curTextureEdit;
    }
    private void bSave_Click(object sender, EventArgs e)
    {
        Render();
        //Bitmap save = new Bitmap(result);
        //result.Save("../output/render.png", System.Drawing.Imaging.ImageFormat.Png);
        //save = inputLB.returnBitmap();
        //save.Save("../output/work.png", System.Drawing.Imaging.ImageFormat.Png);
        //save.Save("../input/autoSave.png", System.Drawing.Imaging.ImageFormat.Png);
        //inputLB = new LockBitmap(save, false);
    }
    private void bLoad_Click(object sender, EventArgs e)
    {

    }
    private void bLoadTexture_Click(object sender, EventArgs e)
    {
        /*
        var fileExplorer = new FormFileExplorer("../textures/");
        fileExplorer.FileSelectet += new FileSystemEventHandler(
            (object fssender, FileSystemEventArgs fse) =>
            {
                Program.MainForm.renderer.LoadTexture(fse.FullPath);
                Program.MainForm.Repainting = true;
            }
            );
        fileExplorer.Show();
        */
    }

    private void FormEditor_Resize(object sender, EventArgs e)
    {
        pBResult.Refresh();
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

    private void pBResult_MouseDown(object sender, MouseEventArgs e)
    {
        camera.MouseMoveEvent(e, false);
    }

    private void toolStripMenuItem1_CheckedChanged(object sender, EventArgs e)
    {
        if (toolStripMenuItem1.Checked)
        {
            renderer.ShadowQuality = 1;
            NeedRender = true;
        }
        else
        {
            renderer.ShadowQuality = 0;
            NeedRender = true;
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
                Program.MainForm.NeedRender = true;
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
        toolStripButtonRotate.Checked = false;
    }

    private void toolStripButtonRotate_Click(object sender, EventArgs e)
    {
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
            var input = new InputData(dialog.FileName)
            {
                Textures = renderer.InputData.Textures,
            };

            renderer.InputData = input;
            settings.DirectoryImport = Path.GetDirectoryName(dialog.FileName);
            NeedRender = true;
        }
    }

    private void ImportTextureMenuItemClick(object sender, EventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.InitialDirectory = Path.GetFullPath(settings.DirectoryImport);
        dialog.Filter = ImageFilter;

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            renderer.InputData.LoadTextureBitmap(dialog.FileName);
            settings.DirectoryImport = Path.GetDirectoryName(dialog.FileName);
            NeedRender = true;
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
            using var handle = renderer.Result;
            var bitmap = handle.Value;

            if (Program.MainForm.NeedRender)
                renderer.Render();

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
        NeedRefresh = true;
    }

    private void ButtonReset_Click(object sender, EventArgs e)
    {
        camera.Position = PointF.Empty;
        renderer.Angle = 45;
        NeedRender = true;
        NeedRefresh = true;
    }

    private void TextBoxRotate_TextChanged(object sender, EventArgs e)
    {
        if (float.TryParse(TextBoxRotate.Text, out float angle))
        {
            if (renderer.Angle != angle)
            {
                TextBoxRotate.ForeColor = Color.Blue;
                renderer.Angle = angle;
                NeedRender = true;
                NeedRefresh = true;
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
        NeedRender = true;
        NeedRefresh = true;
    }

    private void ButtonLeft_Click(object sender, EventArgs e)
    {
        renderer.Angle -= 45;
        NeedRender = true;
        NeedRefresh = true;
    }
}
