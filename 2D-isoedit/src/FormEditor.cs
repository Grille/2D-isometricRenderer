using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using GGL;
using GGL.IO;
using System.Drawing.Drawing2D;
/// <summary>-</summary>
namespace program
{

    //[System.ComponentModel.DesignerCategory("code")]
    public partial class FormEditor : Form
    {
        //Graphic
        public IsometricRenderer renderer;
        private Bitmap result;
        //Rendering Values
        public bool Repainting = false;
        private bool resultRedy = false;

        //mouse
        private Point lastMousePos;
        private Point startMousePos;

        private string directory,directorySave, directoryImport, directoryExport;
        //setings
        private float cores = (int)(Environment.ProcessorCount);

        //Tasks
        private Task renderTask;

        //Editor Values
        bool curTextureEdit;

        float camPosX, camPosY, camScale = 1;

        public FormEditor()
        {
            Console.WriteLine("Start");
            InitializeComponent();
            directory = directorySave = directoryImport = directoryExport = "";

            if (File.Exists("config.ini"))
            {
                string[] configLines = File.ReadAllLines("config.ini");
                var config = new SortedList<string, string>();
                for (int i = 0; i < configLines.Length; i++)
                {
                    string[] pair = configLines[i].Split(' ');
                    for (int j = 0; j < pair.Length - 2; j++)
                        pair[1] += " "+pair[j + 2];
                    if (pair.Length >= 2)
                    {
                        config.Add(pair[0].Trim().ToLower(), pair[1].Trim());
                    }
                }
                try
                {
                    if (config.TryGetValue("-fullscreen", out string fullscreen))
                        Fullscreen(Convert.ToBoolean(fullscreen));
                    if (config.TryGetValue("-width", out string width))
                        Width = Convert.ToInt32(width);
                    if (config.TryGetValue("-height", out string height))
                        Height = Convert.ToInt32(height);
                    if (config.TryGetValue("-directory", out string path))
                        directory = directorySave = directoryImport = directoryExport = path.Trim('"');
                    if (config.TryGetValue("-directorysave", out string paths))
                        directorySave = paths.Trim('"');
                    if (config.TryGetValue("-directoryexport", out string pathe))
                        directoryExport = pathe.Trim('"');
                    if (config.TryGetValue("-directoryimport", out string pathi))
                        directoryImport = pathi.Trim('"');
                }
                catch (Exception e)
                {
                    Console.WriteLine("config error "+e.Message);
                }
            }
            else
            {
                Console.WriteLine("config not found");
            }
                
            renderer = new IsometricRenderer();
            renderer.LoadDataFromBitmapFile(directoryImport + "/Mountain_512.png");
            renderer.TexturePack.Load(directory + "/default.tex");
            Console.WriteLine("Init Renderer");
            result = null;
            renderTimer.Start();
            Repainting = true;
        }

        //Draw rendered image
        private void pBRender_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            if (result == null) return;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            Rectangle windowRect = new Rectangle(0, 0, pBResult.Width, pBResult.Height);
            g.FillRectangle(new LinearGradientBrush(windowRect, Color.FromArgb(50, 50, 100), Color.FromArgb(15, 15, 30), LinearGradientMode.Vertical), windowRect);
            float drawPosX = ((-camPosX) * camScale) + Width / 2;
            float drawPosY = ((-camPosY) * camScale) + Height / 2;
            g.DrawImage(result,
                new RectangleF(drawPosX - (result.Width / 2) * camScale, drawPosY - 255 * camScale - ((result.Height - 255) / 2) * camScale, result.Width * camScale, result.Height * camScale),
                new RectangleF(0, 0, result.Width, result.Height), GraphicsUnit.Pixel);
            //g.DrawString("" + renderer.RenderTime + "ms", new Font("consolas", 11), new SolidBrush(Color.White), new Point(0, 0));


            float X=0, Y=0, Z=0;

            //renderer.AddAngle(curAngle - lastAngle);

            //g.DrawLine(new Pen(Color.FromArgb(100, Color.Red), 2), new PointF(drawPosX, drawPosY-100), new PointF(drawPosX, drawPosY + 100));
            //g.DrawLine(new Pen(Color.FromArgb(100, Color.Red), 2), new PointF(drawPosX- (result.Width/2) * camScale, drawPosY), new PointF(drawPosX+ (result.Width / 2) * camScale, drawPosY));
            //g.DrawLine(new Pen(Color.FromArgb(100, Color.Lime), 2), new PointF(drawPosX - 100, drawPosY - 50), new PointF(drawPosX + 100, drawPosY + 50));
            toolStripStatusLabelRenderTime.Text = "RenderTime " + renderer.RenderTime + "ms";
        }

        //Render Image
        private void Render()
        {
            result = renderer.Render();
            resultRedy = true;
        }
        //RenderLoop & AutoRotate
        private void renderTimer_Tick(object sender, EventArgs e)
        {
            if (autoRotateToolStripMenuItem.Checked)
            {
                renderer.Angle+=1;
                Repainting = true;
            }
            if (!renderer.IsRendering && Repainting)
            {
                renderTask = new Task(() => Render());
                renderTask.Start();
                Repainting = false;
            }
            if (resultRedy)
            {
                pBResult.Refresh();
                resultRedy = false;
            }
        }

        #region GUI events
        private void pBRender_MouseMove(object sender, MouseEventArgs e)
        {
            pBResult.Focus();
            //if (e.X > heightMapPosX && e.Y > heightMapPosY)
            //{
            if (e.Button == MouseButtons.Middle)
            {
                camPosX += (lastMousePos.X - e.X) / camScale;
                camPosY += (lastMousePos.Y - e.Y) / camScale;
                pBResult.Refresh();
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (toolStripButtonDrag.Checked)
                {
                    camPosX += (lastMousePos.X - e.X)/camScale;
                    camPosY += (lastMousePos.Y - e.Y)/camScale;
                    pBResult.Refresh();
                    //Render();
                }
                else if (toolStripButtonRotate.Checked)
                {
                    float curPosX = e.X - Width / 2 + camPosX * camScale, curPosY = (e.Y - Height / 2 + camPosY * camScale);
                    float lastPosX = lastMousePos.X - Width / 2 + camPosX * camScale, lastPosY = (lastMousePos.Y - Height / 2 + camPosY * camScale);

                    float curAngle = (float)(Math.Atan2(curPosY * 2, curPosX) * (180 / Math.PI));
                    float lastAngle = (float)(Math.Atan2(lastPosY * 2, lastPosX) * (180 / Math.PI));

                    renderer.Angle += curAngle - lastAngle;
                    Repainting = true;
                }
                //pBResult.Refresh();
            }
            //}
            lastMousePos = e.Location;
        }
        private void pBRender_MouseWheel(object sender, MouseEventArgs e)
        {
            
            float posX = -camPosX + (e.X - Width / 2) / camScale;
            float posY = -camPosY + (e.Y - Height / 2) / camScale;

            
            camScale += (e.Delta / 500f) * camScale;

            if (camScale < 0.1) camScale = 0.1f;
            else if (camScale > 4f) camScale = 4f;

            
            camPosX += (camPosX - (-posX + (Width / 2 * (e.X / (float)Width * 2 - 1)) / camScale));
            camPosY += (camPosY - (-posY + (Height / 2 * (e.Y / (float)Height * 2 - 1)) / camScale));
            
            pBResult.Refresh();
        }

        //Buttons
        private void bRotL_Click(object sender, EventArgs e)
        {
            renderer.Angle -= 45;
            Repainting = true;
        }
        private void bRotR_Click(object sender, EventArgs e)
        {
            renderer.Angle += 45;
            Repainting = true;
        }
        private void bRot_Click(object sender, EventArgs e)
        {
            renderer.Angle = 0;
            Repainting = true;
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
            Bitmap save = new Bitmap(result);
            result.Save("../output/render.png", System.Drawing.Imaging.ImageFormat.Png);
            //save = inputLB.returnBitmap();
            save.Save("../output/work.png", System.Drawing.Imaging.ImageFormat.Png);
            save.Save("../input/autoSave.png", System.Drawing.Imaging.ImageFormat.Png);
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
            lastMousePos = e.Location;
        }

        private void toolStripMenuItem1_CheckedChanged(object sender, EventArgs e)
        {
            if (toolStripMenuItem1.Checked)
            {
                renderer.ShadowQuality = 1;
                Repainting = true;
            }
            else
            {
                renderer.ShadowQuality = 0;
                Repainting = true;
            }
        }

        private void openHighMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = Path.GetFullPath(directorySave);
            dialog.Filter = "IsoHightMap(*.IHM)|*.IHM|All files (*.*)|*.*";
            dialog.FileOk += new CancelEventHandler((object csender, CancelEventArgs ce) =>
            {
                ByteStream bs = new ByteStream(dialog.FileName);
                bs.ReadByte();
                Program.MainForm.renderer.Data = new RenderData(bs.ReadInt(), bs.ReadInt());
                Program.MainForm.renderer.Data.HeightMap = bs.ReadByteArray();
                Program.MainForm.renderer.Data.TextureMap = bs.ReadByteArray();
                Program.MainForm.Repainting = true;
                directorySave = Path.GetDirectoryName(dialog.FileName);
            }
            );
            dialog.ShowDialog(this);
        }

        private void saveRenderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.InitialDirectory = Path.GetFullPath(directorySave);
            dialog.AddExtension = true;
            dialog.DefaultExt = "ihm";
            dialog.Filter = "IsoHightMap(*.IHM)|*.IHM|All files (*.*)|*.*";
            dialog.FileOk += new CancelEventHandler((object csender, CancelEventArgs ce) =>
            {
                ByteStream bs = new ByteStream();
                bs.WriteByte(0);
                bs.WriteInt(Program.MainForm.renderer.Data.Width);
                bs.WriteInt(Program.MainForm.renderer.Data.Height);
                bs.WriteByteArray(Program.MainForm.renderer.Data.HeightMap,CompressMode.Auto);
                bs.WriteByteArray(Program.MainForm.renderer.Data.TextureMap, CompressMode.Auto);
                bs.Save(dialog.FileName);
                directorySave = Path.GetDirectoryName(dialog.FileName);
            }
            );
            dialog.ShowDialog(this);
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
                    Program.MainForm.Repainting = true;
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

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = Path.GetFullPath(directoryImport);
            dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            dialog.FileOk += new CancelEventHandler((object csender, CancelEventArgs ce) =>
            {
                //FormImport form = new FormImport();
                //if (form.ShowDialog(this,out ImportOptions options) == DialogResult.OK)
                //{
                    renderer.LoadDataFromBitmapFile(dialog.FileName);
                    directoryImport = Path.GetDirectoryName(dialog.FileName);
                    Repainting = true;
                //}
                //else
                //{
                //    ce.Cancel = true;
                //}
            }
            );
            dialog.ShowDialog(this);
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.InitialDirectory = Path.GetFullPath(directoryExport);
            dialog.AddExtension = true;
            dialog.DefaultExt = "png";
            dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            dialog.FileOk += new CancelEventHandler((object csender, CancelEventArgs ce) =>
            {
                if (Program.MainForm.Repainting)
                    renderer.Render();
                renderer.Result.Save(dialog.FileName);
                directoryExport = Path.GetDirectoryName(dialog.FileName);
                //Program.MainForm.Repainting = true;
                //ce.Cancel = false;
            }
            );
            dialog.ShowDialog(this);
        }
        #endregion

    }
}
