using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
/// <summary>-</summary>
namespace Program
{
    public class Texture
    {
        public string Name;
        private Color[] data;//[r,g,b,a,l]
        private List<TextureSegment> segments;

        public Texture()
        {
            segments = new List<TextureSegment>();
            data = new Color[256];
        }

        public void AddSegment(TextureSegment seg)
        {
            segments.Add(seg);
        }

        public void FillData()
        {
            int iz = 0;
            if (segments.Count == 0)
            {
                for (int i = 0; i < 256; i++)
                    data[i] = Color.White;
            }
            else
            {
                while (iz < 255)
                {
                    foreach (var pair in segments)
                    {
                        var color = Color.FromArgb(pair.A, pair.R, pair.G, pair.B);
                        for (int i = 0; i < pair.Repeat; i++)
                        {
                            if (iz >= 255)
                                break;

                            data[iz] = color;
                            iz += 1;
                        }
                    }
                }
            }

        }


        public Color GetColorAt(int x, int y, int z)
        {
            return data[z];
        }

        public Color GetColorAt(int z)
        {
            return data[z];
        }
    }
}