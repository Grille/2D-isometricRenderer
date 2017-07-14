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
    struct Editor
    {
        public Bitmap Map;
        public float MapPosX;
        public float MapPosY;
        public float MapSize;
        public string renderInfo;
        public void init()
        {
            Map = new Bitmap(64, 64);
            MapPosX = 300 / 2 - 32;
            MapPosY = 300 / 2 - 32;

            MapSize = 1;
        }
    }
    /// <summary>-</summary>
    class LookBitmap
    {
        private Bitmap bmp;
        private Rectangle rect;
        private System.Drawing.Imaging.BitmapData bmpData;
        private IntPtr ptr;
        private int bytes;
        private byte[] rgbValues;


        public LookBitmap(Bitmap input, bool byValue)
        {
            if (byValue) bmp = new Bitmap(input);
            else bmp = input;
            rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
            ptr = bmpData.Scan0;
            bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
        }
        public Bitmap getBitmap()
        {
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);
            return bmp;
        }
        public byte[] getRGB()
        {
            return rgbValues;
        }
    }
    class Texture
    {
        private bool enabled;
        private bool downwards;
        private bool endColor;
        private byte size;
        private Color[] colorsZ;
        public Texture(Color[] colors, bool endColor)
        {
            this.endColor = endColor;
            colorsZ = colors;
            size = (byte)(colorsZ.Length);
            if (endColor) size--;
        }

        public void setColor(byte[] arrayRGB, int offset, byte height, byte maxHeight, float shadow)
        {
            if (endColor && height + 1 == maxHeight)
            {
                arrayRGB[offset + 0] = (byte)(colorsZ[size].B * shadow);
                arrayRGB[offset + 1] = (byte)(colorsZ[size].G * shadow);
                arrayRGB[offset + 2] = (byte)(colorsZ[size].R * shadow);
                arrayRGB[offset + 3] = (byte)(colorsZ[size].A);
            }
            else
            {
                while (height >= size) height -= size;
                arrayRGB[offset + 0] = (byte)(colorsZ[height].B * shadow);
                arrayRGB[offset + 1] = (byte)(colorsZ[height].G * shadow);
                arrayRGB[offset + 2] = (byte)(colorsZ[height].R * shadow);
                arrayRGB[offset + 3] = (byte)(colorsZ[height].A);
            }
        }

    }

    public partial class FormEditor : Form
    {    

        Texture[] textures;
        Bitmap inputMap;
        Editor input;
        Editor result;
        Point lastMousePos;

        //Rendering Values
        byte heightExcess = 0;
        byte[] shadowMap;

        //Rendering orientation
        int angle = 45;
        float tilt = 2;

        //Editor Values
        bool curTextureEdit;
        bool renderAllInTimer;
        byte editValue = 1;

        public FormEditor()
        {
            input.init();
            result.init();

            textures = new Texture[] {
            new Texture(new Color[] {Color.FromArgb(80, 100, 50),Color.FromArgb(80, 105, 50),Color.FromArgb(80, 100, 50)},false),
            new Texture(new Color[] {Color.FromArgb(110, 100, 80)},false),
            new Texture(new Color[] {Color.FromArgb(80, 100, 50),Color.FromArgb(80, 110, 50)},true),
            new Texture(new Color[] {Color.FromArgb(150, 150, 150),Color.FromArgb(140, 140, 160),Color.FromArgb(140, 140, 150)},false),
            new Texture(new Color[] {Color.FromArgb(30, 70, 20),Color.FromArgb(40, 90, 30)},true),
            new Texture(new Color[] {Color.FromArgb(220, 220, 255),Color.FromArgb(150, 150, 255)},true),
            new Texture(new Color[] {Color.FromArgb(200, 200, 200),Color.FromArgb(200, 100, 50)},true),
            };

            InitializeComponent();
        }

        //Prepare the heightMap (rotate, compress y and add shadow)
        private Bitmap switchMode(Bitmap heightMap)
        {
            Stopwatch now = new Stopwatch();
            now.Start();
            int width = heightMap.Width;
            int height = heightMap.Height;
            Bitmap heightBM;

            if (checkBoxGame.Checked)//game render
            {
                heightBM = new Bitmap(heightMap);
                if (angle >= 270) heightBM.RotateFlip(RotateFlipType.Rotate270FlipNone);
                else if (angle >= 180) heightBM.RotateFlip(RotateFlipType.Rotate180FlipNone);
                else if (angle >= 90)  heightBM.RotateFlip(RotateFlipType.Rotate90FlipNone); 
                Console.WriteLine(angle);
            }
            else//dynamic render
            {
                width = (int)(width*1.5f); height = (int)(height * 1.5f);
                heightBM = new Bitmap((int)(width), (int)(height));
                Graphics g = Graphics.FromImage(heightBM);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                g.TranslateTransform(width / 2, height / 2);
                g.RotateTransform(angle, System.Drawing.Drawing2D.MatrixOrder.Prepend);
                g.DrawImage(heightMap, new Rectangle(-heightMap.Width / 2, -heightMap.Height / 2, heightMap.Width, heightMap.Height), new RectangleF(0, 0, heightMap.Width, heightMap.Width), GraphicsUnit.Pixel);
                g.ResetTransform();
            }

            //Look bitmap and set ref to RGB byte array
            LookBitmap heightLB = new LookBitmap(heightBM, false);
            LookBitmap resultLB = new LookBitmap(new Bitmap((int)(width), (int)(height / 2)), false);
            byte[] heightRGB = heightLB.getRGB();
            byte[] resultRGB = resultLB.getRGB();

            int offsetWidth = width * 4;

            if (checkBoxGame.Checked) //game render
            {
                for (int ix = 0; ix < width; ix++)
                {
                    for (int iy = (int)((height) / 4); iy >= 0; iy--)//downwards
                    {
                        int counterDest = (ix + iy * width) * 4 + offsetWidth;
                        int counterSrc = (int)((ix + iy * 2 * width) * 4) + 0;

                        resultRGB[counterDest + 1] = heightRGB[counterSrc + 1];
                        resultRGB[counterDest + 3] = heightRGB[counterSrc + 3];
                        resultRGB[counterDest + 0] = heightRGB[counterSrc + 0];
                    }
                    for (int iy = (int)((height) / 4); iy < (int)((height) / 2)-1; iy++)//downwards
                    {
                        int counterDest = (ix + iy * width) * 4 + offsetWidth;
                        int counterSrc = (int)((ix + iy * 2 * width) * 4) + offsetWidth;

                        resultRGB[counterDest + 1] = heightRGB[counterSrc + 1];
                        resultRGB[counterDest + 3] = heightRGB[counterSrc + 3];
                        resultRGB[counterDest + 0] = heightRGB[counterSrc + 0];
                    }


                }
            }
            else //dynamic render
            {
                for (int ix = 0; ix < width; ix++)
                {
                    for (int iy = (int)((height - 1) / tilt); iy >= 0; iy--)
                    {
                        int counterDest = (ix + iy * width) * 4;
                        int counterSrc = (int)((ix + iy * tilt * width) * 4);
                        if (heightRGB[counterSrc + 1] < heightRGB[counterSrc + 1 + offsetWidth])
                        {
                            resultRGB[counterDest + 1] = heightRGB[counterSrc + 1 + offsetWidth];
                            resultRGB[counterDest + 3] = heightRGB[counterSrc + 3 + offsetWidth];
                            resultRGB[counterDest + 0] = heightRGB[counterSrc + 0 + offsetWidth];
                        }
                        else
                        {
                            resultRGB[counterDest + 1] = heightRGB[counterSrc + 1];
                            resultRGB[counterDest + 3] = heightRGB[counterSrc + 3];
                            resultRGB[counterDest + 0] = heightRGB[counterSrc + 0];
                        }
                    }
                }
            }

            if (checkBoxShadow.Checked)//render shadows?
            {
                for (int ix = 0; ix < width; ix++)
                {
                    for (int iy = (int)((height - 1) / tilt); iy >= 0; iy--)
                    {
                        int counter = (ix + iy * width) * 4;
                        int i = 0;
                        int max = (resultRGB[counter + 1]);
                        while (iy + i < height && resultRGB[counter + 2] < max && resultRGB[counter + 1] <= max + 1)
                        {
                            if (i > 0) resultRGB[counter + 2] = (byte)(max * 1f);
                            i++;
                            max--;
                            counter += 4;
                        }

                    }
                }
            }
            return resultLB.getBitmap();
        }
        //Rendering the image from heightmap (elevate and apply textures & shadows)
        private void renderResult(Bitmap inputMap)
        {
            Stopwatch now = new Stopwatch();
            now.Start();
            if (inputMap == null) return;
            LookBitmap inputLB = new LookBitmap(inputMap, false);
            LookBitmap resultLB = new LookBitmap(new Bitmap(inputMap.Width, inputMap.Height + heightExcess), false);
            byte[] inputRGB = inputLB.getRGB();
            byte[] resultRGB = resultLB.getRGB();


            int renderPixel = 0;
            int width = inputMap.Width;
            int height = inputMap.Height;
            for (int ix = 0; ix < width; ix++)
            {
                for (int iy = height - 1; iy >= 0; iy--) //Downwards
                {
                    int counter = (ix + iy * width) * 4;
                    for (byte i = inputRGB[counter + 1]; i > 0; i--) //Downwards
                    {
                        if ((iy + heightExcess) - i >= 0)//save
                        {
                            int counter2 = counter - (width * i * 4) + width * heightExcess * 4;//pos + curent height
                            if (resultRGB[counter2 + 3] == 0)
                            {
                                float shadow = 1f;
                                if (i < inputRGB[counter + 2]) shadow = 0.75f;
                                textures[inputRGB[counter]].setColor(resultRGB, counter2, (byte)(i - 1), inputRGB[counter + 1], shadow);

                                //resultRGB[counter2 + 3] = 255;
                                //if (i == inputRGB[counter + 1]) resultRGB[counter2] = 100;
                                //resultRGB[counter2 + 1] = (byte)(((byte)(i * 100)) / 4 + 30);

                                renderPixel++;
                            }
                            else
                            {
                                //renderPixel++;
                                break;
                            }
                            //renderPixel++;
                        }
                    }
                }
            }

            result.renderInfo = ("renderPixels => " + renderPixel) + '\n' + ("renderTime => " + now.ElapsedMilliseconds);
            result.Map = resultLB.getBitmap();
        }

        //render the high editor map
        private void renderHeight(Bitmap inputMap)
        {
            Stopwatch now = new Stopwatch();
            now.Start();
            if (inputMap == null) return;
            LookBitmap heightLB = new LookBitmap(inputMap, true);
            LookBitmap resultLB = new LookBitmap(new Bitmap(inputMap.Width, inputMap.Height), false);
            byte[] heightRGB = heightLB.getRGB();
            byte[] resultRGB = resultLB.getRGB();
            int renderPixel = 0;
            int width = inputMap.Width * 4;
            for (int ix = 1; ix < inputMap.Width - 1; ix++)
            {
                for (int iy = inputMap.Height - 2; iy >= 1; iy--)
                {
                    int counter = iy* width + ((ix) * 4);
                    byte thmp = (byte)((byte)(heightRGB[counter + 1] * 20)/2);
                    resultRGB[counter + 0] = thmp;
                    resultRGB[counter + 1] = (byte)(((byte)(heightRGB[counter + 1]/20))*40);
                    byte dd = 0;
                    if (heightRGB[counter + 1] > heightRGB[counter + 1 + 4]) dd++;
                    if (heightRGB[counter + 1] > heightRGB[counter + 1 - 4]) dd++;
                    if (heightRGB[counter + 1] > heightRGB[counter + 1 + width]) dd++;
                    if (heightRGB[counter + 1] > heightRGB[counter + 1 - width]) dd++;
                    if (dd > 0)
                    {
                        //resultRGB[counter + 0] = 100;

                        //shadowMap[counter / 4] = dd;
                    }
                    //else shadowMap[counter / 4] = 0;
                    resultRGB[counter + 3] = 255;
                    //renderPixel++;

                    //renderPixel++;
                }
            }

            this.input.renderInfo = ("renderPixels => " + renderPixel)+'\n'+ ("renderTime => " + now.ElapsedMilliseconds);
            this.input.Map = resultLB.getBitmap();
        }
        //render the high texture map
        private void renderTexture(Bitmap inputMap)
        {
            Stopwatch now = new Stopwatch();
            now.Start();
            if (inputMap == null) return;
            LookBitmap heightLB = new LookBitmap(inputMap, true);
            LookBitmap resultLB = new LookBitmap(new Bitmap(inputMap.Width, inputMap.Height), false);
            byte[] heightRGB = heightLB.getRGB();
            byte[] resultRGB = resultLB.getRGB();
            int renderPixel = 0;
            int width = inputMap.Width * 4;

            for (int ix = 1; ix < inputMap.Width - 1; ix++)
            {
                for (int iy = inputMap.Height - 2; iy >= 1; iy--)
                {
                    int counter = iy * width + ((ix) * 4);

                    float h, s, v;
                    h = (byte)(heightRGB[counter + 0] * 30);
                    //s =  (255-(heightRGB[counter + 1]))/255f;

                    if (heightRGB[counter + 1] != heightRGB[counter + 1 + 4]) s = 0.7f;
                    else if (heightRGB[counter + 1] != heightRGB[counter + 1 - 4]) s = 0.7f;
                    else if (heightRGB[counter + 1] != heightRGB[counter + 1 + width]) s = 0.7f;
                    else if (heightRGB[counter + 1] != heightRGB[counter + 1 - width]) s = 0.7f;
                    else s = 1;

                    if (heightRGB[counter + 0] != heightRGB[counter + 0 + 4]) v = 0.7f;
                    else if (heightRGB[counter + 0] != heightRGB[counter + 0 - 4]) v = 0.7f;
                    else if (heightRGB[counter + 0] != heightRGB[counter + 0 + width]) v = 0.7f;
                    else if (heightRGB[counter + 0] != heightRGB[counter + 0 - width]) v = 0.7f;
                    else v = 1;

                    int pos = (int)(h / 256 * 6);
                    int x = (int)(h / 256 * (256 * 6));
                    int r = 0, g = 0, b = 0;
                    while (x > 255) x -= 255;
                    switch (pos)
                    {
                        case 0: r += 255; g += x; b += 0; break;
                        case 1: r += 255 - x; g += 255; b += 0; break;
                        case 2: r += 0; g += 255; b += x; break;
                        case 3: r += 0; g += 255 - x; b += 255; break;
                        case 4: r += x; g += 0; b += 255; break;
                        case 5: r += 255; g += 0; b += 255 - x; break;
                    }
                    float pro = (((s) / 1));
                    r = (int)(r * (pro) + ((255) * (1 - pro)));//r
                    g = (int)(g * (pro) + ((255) * (1 - pro)));//g
                    b = (int)(b * (pro) + ((255) * (1 - pro)));//b

                    pro = v / 1;
                    r = (int)(r *pro);//r
                    g = (int)(g * pro);//g
                    b = (int)(b * pro);//b

                    //else shadowMap[counter / 4] = 0;

                    //if (heightRGB[counter + 0] == 1) resultRGB[counter + 2] = 50;
                    resultRGB[counter + 0] = (byte)b;
                    resultRGB[counter + 1] = (byte)g;
                    resultRGB[counter + 2] = (byte)r;
                    resultRGB[counter + 3] = 255;
                    //renderPixel++;

                    //renderPixel++;
                }
            }

            this.input.renderInfo = ("renderPixels => " + renderPixel) + '\n' + ("renderTime => " + now.ElapsedMilliseconds);
            this.input.Map = resultLB.getBitmap();
        }


        private void render(bool all)
        {

            Bitmap inputMap = switchMode(this.inputMap);
            if (all)
            {
                if (curTextureEdit) renderTexture(this.inputMap);
                else renderHeight(this.inputMap);
                pBEditorMap.Refresh();
            }
            renderResult(inputMap);
            pBRender.Refresh();
        }

        private void addAngle(int value)
        {
            angle += value;
            if (angle <= 0) angle += 360;
            else if (angle >= 360) angle -= 360;
        }
    }
}
