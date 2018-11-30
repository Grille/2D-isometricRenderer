using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
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

        //setings
        private float cores = (int)(Environment.ProcessorCount);

        //Tasks
        private Task renderTask;

        //Editor Values
        bool curTextureEdit;

        float camPosX, camPosY, camScale = 1;

        public FormEditor()
        {
            Console.WriteLine("build");
            InitializeComponent();
            renderer = new IsometricRenderer();
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
            float drawPosX = ((-camPosX) * camScale) + Width / 2 - (result.Width/2)*camScale;
            float drawPosY = ((-camPosY) * camScale) + Height / 2-255*camScale - ((result.Height-255) / 2) * camScale;
            g.DrawImage(result,
                new RectangleF(drawPosX, drawPosY, result.Width * camScale, result.Height * camScale),
                new RectangleF(0, 0, result.Width, result.Height), GraphicsUnit.Pixel);
            g.DrawString("" + renderer.RenderTime + "ms", new Font("consolas", 11), new SolidBrush(Color.White), new Point(0, 0));
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
            if (checkBoxPreAR.Checked)
            {
                renderer.AddAngle(1);
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
                if (radioButtonPreM.Checked)
                {
                    camPosX += (lastMousePos.X - e.X)/camScale;
                    camPosY += (lastMousePos.Y - e.Y)/camScale;
                    pBResult.Refresh();
                    //Render();
                }
                else if (radioButtonPreR.Checked)
                {
                    float curPosX = e.X - Width / 2 + camPosX * camScale, curPosY = (e.Y - Height / 2 + camPosY * camScale);
                    float lastPosX = lastMousePos.X - Width / 2 + camPosX * camScale, lastPosY = (lastMousePos.Y - Height / 2 + camPosY * camScale);

                    float curAngle = (float)(Math.Atan2(curPosY*2, curPosX) * (180 / Math.PI));
                    float lastAngle = (float)(Math.Atan2(lastPosY*2, lastPosX) * (180 / Math.PI));

                    renderer.AddAngle(curAngle- lastAngle);
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
            renderer.AddAngle(-45);
            Repainting = true;
        }
        private void bRotR_Click(object sender, EventArgs e)
        {
            renderer.AddAngle(45);
            Repainting = true;
        }
        private void bRot_Click(object sender, EventArgs e)
        {
            //angle = 0;
            renderer.SetAngle(0);
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
            var fileExplorer = new FormFileExplorer("../maps/");
            fileExplorer.FileSelectet += new FileSystemEventHandler(
                (object fssender, FileSystemEventArgs fse) =>
                {
                    Program.MainForm.renderer.LoadMap(fse.FullPath);
                    Program.MainForm.Repainting = true;
                }
                );
            fileExplorer.Show();
        }
        private void bLoadTexture_Click(object sender, EventArgs e)
        {
            var fileExplorer = new FormFileExplorer("../textures/");
            fileExplorer.FileSelectet += new FileSystemEventHandler(
                (object fssender, FileSystemEventArgs fse) =>
                {
                    Program.MainForm.renderer.LoadTexture(fse.FullPath);
                    Program.MainForm.Repainting = true;
                }
                );
            fileExplorer.Show();
        }
        private void radioButtonShadowHigh_CheckedChanged(object sender, EventArgs e)
        {
            renderer.ShadowQuality = 1;
            Repainting = true;
        }

        private void FormEditor_Resize(object sender, EventArgs e)
        {
            pBResult.Refresh();
        }

        private void radioButtonShadowLow_CheckedChanged(object sender, EventArgs e)
        {
            renderer.ShadowQuality = 3;
            Repainting = true;
        }
        private void radioButtonShadowOf_CheckedChanged(object sender, EventArgs e)
        {
            renderer.ShadowQuality = 0;
            Repainting = true;
        }
        #endregion

    }
}
