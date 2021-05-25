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
        public int typ;
        public string Name;
        public byte[] Data;//[r,g,b,a,l]
        public Texture(string name, Color color)
        {
            typ = 0;
            this.Name = name;
            //Data = input;
        }
        public Texture(string name,byte[] input)
        {
            this.Name = name;
            Data = input;
        }
    }
}