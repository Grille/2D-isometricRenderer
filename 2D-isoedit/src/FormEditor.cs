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
            int drawPosX = -result.Width / 2 + Width / 2, drawPosY = -255 - (result.Height - 255) / 2 + Height / 2;
            g.DrawImage(result,
                new RectangleF(drawPosX, drawPosY, result.Width, result.Height),
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
                renderer.addAngle(1);
                Repainting = true;
            }
            if (!renderer.IsRenering && Repainting)
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
                //result.MapPosX -= (lastMousePos.X - e.X);
                //result.MapPosY -= (lastMousePos.Y - e.Y);
                pBResult.Refresh();
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (radioButtonPreM.Checked)
                {
                    //result.MapPosX -= (lastMousePos.X - e.X);
                    //result.MapPosY -= (lastMousePos.Y - e.Y);
                }
                else if (radioButtonPreR.Checked)
                {
                    renderer.addAngle(lastMousePos.X - e.X);
                    //addTilt(-(lastMousePos.Y - e.Y));
                    lastMousePos = e.Location;

                    Render();
                }
                pBResult.Refresh();
            }
            //}
            lastMousePos = e.Location;
        }
        private void pBRender_MouseWheel(object sender, MouseEventArgs e)
        {
            //result.MapSize += (float)(result.MapSize * e.Delta) / 1000f;
            pBResult.Refresh();
        }

        //Buttons
        private void bRotL_Click(object sender, EventArgs e)
        {
            renderer.addAngle(-45);
            Repainting = true;
        }
        private void bRotR_Click(object sender, EventArgs e)
        {
            renderer.addAngle(45);
            Repainting = true;
        }
        private void bRot_Click(object sender, EventArgs e)
        {
            //angle = 0;
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
            Form fileExplorer = new FormFileExplorer("../maps/", 0);
            fileExplorer.Show();
        }
        private void bLoadTexture_Click(object sender, EventArgs e)
        {
            Form fileExplorer = new FormFileExplorer("../textures/", 1);
            fileExplorer.Show();
        }
        private void radioButtonShadowHigh_CheckedChanged(object sender, EventArgs e)
        {
            Repainting = true;
        }
        private void radioButtonShadowLow_CheckedChanged(object sender, EventArgs e)
        {
            Repainting = true;
        }
        private void radioButtonShadowOf_CheckedChanged(object sender, EventArgs e)
        {
            Repainting = true;
        }
        #endregion

    }
}
