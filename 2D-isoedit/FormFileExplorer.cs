using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using GGL;
namespace program
{
    public partial class FormFileExplorer : Form
    {
        private string fullPath;
        private byte mode;
        public FormFileExplorer(string path,byte mode)
        {
            InitializeComponent();
            this.mode = mode;
            move(path);
        }
        private void move(string path)
        {
            int[] a = new int[]{3,4};
            textBoxDst.Text = fullPath = System.IO.Path.GetFullPath(path);
            listBoxExplorer.Items.Clear();
            foreach (string dateien in Directory.GetFiles(path))
            {
                string item = (System.IO.Path.GetFileName(dateien));
                //if (item.Split(new char[1]{'.'},1)[0]=="png")
                    listBoxExplorer.Items.Add(item);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (mode==0)
            Program.MainForm.LoadMap(fullPath + (string)listBoxExplorer.SelectedItem);
            else
            Program.MainForm.LoadTexture(fullPath + (string)listBoxExplorer.SelectedItem);
            this.Close();
        }
    }
}
