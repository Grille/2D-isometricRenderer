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

using GrillesGameLibrary;
namespace program
{
    //partial class hide { } //Hide Designer in VS 
    public partial class FormEditor
    {
        //Draw Rendered Images
        private void pBTextureMap_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (input.MapSize < 1) g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            else g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            RenderInfo curEdit;
            if (curTextureEdit) curEdit = input;
            else curEdit = input;
            Rectangle dest = new Rectangle(
                (int)input.MapPosX,
                (int)input.MapPosY,
                (int)(curEdit.Map.Width * input.MapSize),
                (int)(curEdit.Map.Height * input.MapSize)
                );
            g.DrawImage(curEdit.Map, dest, new RectangleF(0, 0, curEdit.Map.Width, curEdit.Map.Height), GraphicsUnit.Pixel);
            g.DrawRectangle(Pens.White, dest);
            g.DrawString(curEdit.renderInfo, new Font("consolas", 11), new SolidBrush(Color.White), new Point(0, 0));

        }
        private void pBRender_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (result.MapSize < 1) g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            else g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            Rectangle dest = new Rectangle(
                (int)(result.MapPosX - ((result.Map.Width) / 2 * result.MapSize)),
                (int)(result.MapPosY- ((result.Map.Height+heightExcess) /2*result.MapSize)),
                (int)(result.Map.Width * result.MapSize),
                (int)((result.Map.Height) * result.MapSize)
                );
            g.DrawImage(result.Map, dest, new RectangleF(0, 0, result.Map.Width, result.Map.Height), GraphicsUnit.Pixel);
            g.DrawRectangle(Pens.White, dest);
            g.DrawString(result.renderInfo, new Font("consolas", 11), new SolidBrush(Color.White), new Point(0, 0));
        }

        //RenderLoop & AutoRotate
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (checkBoxPreAR.Checked)
            {
                addAngle(1);
            }
            if (!isRenering)
            {
                Task thread = new Task(() => render(renderEditor));
                thread.Start();
            }
            pBEditorMap.Refresh();
            pBResult.Refresh();
        }

        private void pBEditorMap_MouseDown(object sender, MouseEventArgs e)
        {
            startMousePos = e.Location;
        }
        private void pBEditorMap_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (radioButtonEditM.Checked)
                {
                }
                else
                {
                    draw(inputLB, startMousePos, e.Location);
                    renderEditor = true;
                }
            }
        }
        private void pBEditorMap_MouseMove(object sender, MouseEventArgs e)
        {
            pBEditorMap.Focus();
            //if (e.X > heightMapPosX && e.Y > heightMapPosY)
            //{
            if (e.Button == MouseButtons.Middle)
            {
                input.MapPosX -= (lastMousePos.X - e.X);
                input.MapPosY -= (lastMousePos.Y - e.Y);
                pBEditorMap.Refresh();
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (radioButtonEditM.Checked)
                {
                    input.MapPosX -= (lastMousePos.X - e.X);
                    input.MapPosY -= (lastMousePos.Y - e.Y);
                    pBEditorMap.Refresh();
                }
                else
                {
                }
            }
            //}
            lastMousePos = e.Location;
        }
        private void pBEditorMap_MouseWheel(object sender, MouseEventArgs e)
        {
            input.MapSize += (float)(input.MapSize * e.Delta) / 1000f;
            input.MapPosX -= 1;
            pBEditorMap.Refresh();
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
        }
        private void bRotR_Click(object sender, EventArgs e)
        {
            addAngle(45);
        }
        private void bRot_Click(object sender, EventArgs e)
        {
            angle = 0;
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
            Form fileExplorer = new FormFileExplorer();
            fileExplorer.Show();
            //inputLB = new LockBitmap(new Bitmap("../input/test_256x256.png"), false);
            //inputLB = new LockBitmap(new Bitmap("../input/test_512x512.png"), false);
            //render(true);
            //timer1.Enabled = true;
        }
        public void load(string path)
        {
            using (var bmpTemp = new Bitmap(path))
            {
                inputLB = new LockBitmap(new Bitmap(bmpTemp), false);
            }
            render(true);
            timer1.Enabled = true;
        }

        //Input
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                editValue = (byte)Convert.ToInt16(textBoxValue.Text);
                textBoxValue.Text = "" + editValue;
            }
            catch { textBoxValue.Text = "" + 0; }
        }
    }
}