using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace _2Deditor
{
    partial class hide { } //Hide Designer in VS 
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
            Rectangle dest = new Rectangle((int)input.MapPosX, (int)input.MapPosY, (int)(curEdit.Map.Width * input.MapSize), (int)(curEdit.Map.Height * input.MapSize));
            g.DrawImage(curEdit.Map, dest, new RectangleF(0, 0, curEdit.Map.Width, curEdit.Map.Height), GraphicsUnit.Pixel);
            g.DrawRectangle(Pens.White, dest);
            g.DrawString(curEdit.renderInfo, new Font("consolas", 11), new SolidBrush(Color.White), new Point(0, 0));

        }
        private void pBRender_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (result.MapSize < 1) g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            else g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            Rectangle dest = new Rectangle((int)result.MapPosX, (int)result.MapPosY, (int)(result.Map.Width * result.MapSize), (int)((result.Map.Height) * result.MapSize));
            g.DrawImage(result.Map, dest, new RectangleF(0, 0, result.Map.Width, result.Map.Height), GraphicsUnit.Pixel);
            g.DrawRectangle(Pens.White, dest);
            g.DrawString(result.renderInfo, new Font("consolas", 11), new SolidBrush(Color.White), new Point(0, 0));
        }

        //RenderLoop & AutoRotate
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Console.WriteLine(angle);
            if (checkBoxPreAR.Checked)
            {
                addAngle(1);
            }
            render(renderAllInTimer);
            renderAllInTimer = false;
            //gf += gfadd;
            //if (gf <= 1f) gfadd = 0.01f;
            //if (gf >= 10f) gfadd = -0.01f;
        }

        private void pBTextureMap_MouseMove(object sender, MouseEventArgs e)
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
                    Graphics g = Graphics.FromImage(this.inputMap);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(0, editValue, listBoxTexture.SelectedIndex)), new RectangleF((lastMousePos.X - input.MapPosX) / input.MapSize, (lastMousePos.Y - input.MapPosY) / input.MapSize, 10, 10));
                    renderAllInTimer = true;
                }
            }
            //}
            lastMousePos = e.Location;
        }
        private void pBTextureMap_MouseWheel(object sender, MouseEventArgs e)
        {
            input.MapSize += (float)(input.MapSize * e.Delta) / 1000f;
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
            addAngle(-90);
            render(false);
        }
        private void bRotR_Click(object sender, EventArgs e)
        {
            addAngle(90);
            render(false);
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
             inputMap = new Bitmap("../input/test_512x512.png");
            //inputMap = new Bitmap("../input/test_flat_64x64.png");
            render(true);
            timer1.Enabled = true;
        }
        private void bSwitch_Click(object sender, EventArgs e)
        {
            curTextureEdit = !curTextureEdit;
            renderAllInTimer = true;
        }
        private void bSave_Click(object sender, EventArgs e)
        {
            render(false);
            Bitmap save = new Bitmap(result.Map);
            result.Map.Save("../output/test.png", System.Drawing.Imaging.ImageFormat.Png);
        }
        private void bLoad_Click(object sender, EventArgs e)
        {
            Form fileExplorer = new FormFileExplorer();
            //FormFileExplorer.
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