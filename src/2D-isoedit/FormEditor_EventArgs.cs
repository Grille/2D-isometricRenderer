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
using System.Drawing.Drawing2D;

using GGL;
namespace program
{
    public partial class FormEditor : Form
    {
        //Draw Rendered Images
        private void pBRender_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (result.MapSize < 1) g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            else g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            Rectangle dest = new Rectangle(
                (int)(result.MapPosX - ((result.Map.Width) / 2 * result.MapSize)),
                (int)(result.MapPosY- ((result.Map.Height+heightExcess) /2*result.MapSize)),
                (int)(result.Map.Width * result.MapSize),
                (int)((result.Map.Height) * result.MapSize)
                );
            Rectangle windowRect = new Rectangle(0, 0, pBResult.Width, pBResult.Height);
            g.FillRectangle(new LinearGradientBrush(windowRect, Color.FromArgb(50, 50, 100), Color.FromArgb(15,15,30),LinearGradientMode.Vertical), windowRect);
            g.DrawImage(result.Map, dest, new RectangleF(0, 0, result.Map.Width, result.Map.Height), GraphicsUnit.Pixel);
            g.DrawRectangle(Pens.White, dest);
            g.DrawString(result.renderInfo, new Font("consolas", 11), new SolidBrush(Color.White), new Point(0, 0));
        }

        //RenderLoop & AutoRotate
        private void renderTimer_Tick(object sender, EventArgs e)
        {
            if (checkBoxPreAR.Checked)
            {
                addAngle(1);
            }
            if (!isRenering && viewChange)
            {
                Console.WriteLine("render");
                Task renderThread = new Task(() => render(renderEditor));
                renderThread.Start();
                viewChange = false;
            }
            if (drawImage)
            {
                pBResult.Refresh();
                drawImage = false;
            }
        }

        private void pBRender_MouseMove(object sender, MouseEventArgs e)
        {
            pBResult.Focus();
            //if (e.X > heightMapPosX && e.Y > heightMapPosY)
            //{
            if (e.Button == MouseButtons.Middle)
            {
                result.MapPosX -= (lastMousePos.X - e.X);
                result.MapPosY -= (lastMousePos.Y - e.Y);
                pBResult.Refresh();
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (radioButtonPreM.Checked)
                {
                    result.MapPosX -= (lastMousePos.X - e.X);
                    result.MapPosY -= (lastMousePos.Y - e.Y);
                }
                else if (radioButtonPreR.Checked)
                {
                    addAngle(lastMousePos.X - e.X);
                    //addTilt(-(lastMousePos.Y - e.Y));
                    lastMousePos = e.Location;

                    render(true);
                }
                pBResult.Refresh();
            }
            //}
            lastMousePos = e.Location;
        }
        private void pBRender_MouseWheel(object sender, MouseEventArgs e)
        {
            result.MapSize += (float)(result.MapSize * e.Delta) / 1000f;
            pBResult.Refresh();
        }

        //Buttons
        private void bRotL_Click(object sender, EventArgs e)
        {
            addAngle(-45);
            viewChange = true;
        }
        private void bRotR_Click(object sender, EventArgs e)
        {
            addAngle(45);
            viewChange = true;
        }
        private void bRot_Click(object sender, EventArgs e)
        {
            angle = 0;
            viewChange = true;
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void bNew_Click(object sender, EventArgs e)
        {

            //inputLB = new LockBitmap(new Bitmap("../input/test_256x256.png"), false);
            inputLB = new LockBitmap(new Bitmap(128,128), false);
            render(true);
            timer1.Enabled = true;
        }
        private void bSwitch_Click(object sender, EventArgs e)
        {
            curTextureEdit = !curTextureEdit;
            renderEditor = true;
        }
        private void bSave_Click(object sender, EventArgs e)
        {
            render(false);
            Bitmap save = new Bitmap(result.Map);
            result.Map.Save("../output/render.png", System.Drawing.Imaging.ImageFormat.Png);
            save = inputLB.returnBitmap();
            save.Save("../output/work.png", System.Drawing.Imaging.ImageFormat.Png);
            save.Save("../input/autoSave.png", System.Drawing.Imaging.ImageFormat.Png);
            inputLB = new LockBitmap(save, false);
        }
        private void bLoad_Click(object sender, EventArgs e)
        {
            Form fileExplorer = new FormFileExplorer("../maps/");
            fileExplorer.Show();
            //inputLB = new LockBitmap(new Bitmap("../input/test_256x256.png"), false);
            //inputLB = new LockBitmap(new Bitmap("../input/test_512x512.png"), false);
            //render(true);
            //timer1.Enabled = true;
        }
        private void bLoadTexture_Click(object sender, EventArgs e)
        {
            Form fileExplorer = new FormFileExplorer("../textures/");
            fileExplorer.Show();
            viewChange = true;
        }
        public void load(string path)
        {
            using (var bmpTemp = new Bitmap(path))
            {
                inputLB = new LockBitmap(new Bitmap(bmpTemp), false);
            }
            timer1.Enabled = true;
            viewChange = true;
        }
        private void radioButtonShadowHigh_CheckedChanged(object sender, EventArgs e)
        {
            viewChange = true;
        }
        private void radioButtonShadowLow_CheckedChanged(object sender, EventArgs e)
        {
            viewChange = true;
        }
        private void radioButtonShadowOf_CheckedChanged(object sender, EventArgs e)
        {
            viewChange = true;
        }
    }
}