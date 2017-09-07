using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using GrillesGameLibrary;
namespace program
{
    public partial class FormFileExplorer : Form
    {
        private string fullPath;
        public FormFileExplorer()
        {
            InitializeComponent();
            move("../input/");
        }
        private void move(string path)
        {

            textBoxDst.Text = fullPath = System.IO.Path.GetFullPath(path);
            listBoxExplorer.Items.Clear();
            foreach (string dateien in Directory.GetFiles(path)) listBoxExplorer.Items.Add(System.IO.Path.GetFileName(dateien));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.mainForm.load(fullPath + (string)listBoxExplorer.SelectedItem);
            this.Close();
        }
    }
}
