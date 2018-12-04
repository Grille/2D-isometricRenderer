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
        public event FileSystemEventHandler FileSelectet;
        private string fullPath;
        private string[] types;
        public FormFileExplorer(string path) : this(path,""){}
        public FormFileExplorer(string path,string types)
        {
            InitializeComponent();
            this.types = types.Split(',');
            move(path);
            listBox.Items.Add("<Favorites>");
            //listBox.Items.Add("<My Computer>");
            listBox.Items.Add("<Desktop>");
            listBox.Items.Add("<Documents>");
        }
        private void move(string path)
        {
            int[] a = new int[]{3,4};
            if (path==null||path==""||!Directory.Exists(Path.GetFullPath(path))) return;
            textBoxDst.Text = fullPath = Path.GetFullPath(path);

            listBoxExplorer.Items.Clear();
            listBoxExplorer.Items.Add("...");
            try
            {
                foreach (string dateien in Directory.GetDirectories(path))
                {

                    string item = '<' + (Path.GetFileName(dateien)) + '>';
                    //if (item.Split(new char[1]{'.'},1)[0]=="png")
                    listBoxExplorer.Items.Add(item);

                }
                foreach (string dateien in Directory.GetFiles(path))
                {
                    string item = (Path.GetFileName(dateien));
                    string extension = Path.GetExtension(item);
                    bool visible = false;
                    if (types[0] != "")
                    {
                        for (int i = 0; i < types.Length; i++)
                            if (extension == '.' + types[i]) visible = true;
                    }
                    else visible = true;
                    if (visible) listBoxExplorer.Items.Add(item);      
                }
            }
            catch { }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();        
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            itemSelectet();
        }

        private void textBoxDst_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(textBoxDst.Text))
            move(textBoxDst.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            move(Path.GetDirectoryName(fullPath.TrimEnd('\\')));
        }

        private void listBoxExplorer_DoubleClick(object sender, EventArgs e)
        {
            itemSelectet();
        }
        private void itemSelectet()
        {
            string item = (string)listBoxExplorer.SelectedItem;
            if (item == "...")
            {
                move(Path.GetDirectoryName(fullPath.TrimEnd('\\')));
            }
            else if (item[0] == '<')
            {
                move(fullPath.TrimEnd('\\') + '\\'+ item.Trim(new char[] { '<', '>' }));
            }
            else
            {
                string path = Path.GetDirectoryName(fullPath.TrimEnd('\\'));
                FileSelectet?.Invoke(this, new FileSystemEventArgs(WatcherChangeTypes.All, fullPath, item));
                this.Close();
            }
        }
        private void shortcutSelectet()
        {
            switch ((string)listBox.SelectedItem)
            {
                case "<Favorites>": move(Environment.GetFolderPath(Environment.SpecialFolder.Favorites)); break;
                case "<My Computer>": move(Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)); break;
                case "<Desktop>":move(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));break;
                case "<Documents>": move(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)); break;
            }
        }

        private void listBoxExplorer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) itemSelectet();
            else if (e.KeyCode == Keys.Back) move(Path.GetDirectoryName(fullPath.TrimEnd('\\')));
        }

        private void listBox_DoubleClick(object sender, EventArgs e)
        {
            shortcutSelectet();
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) shortcutSelectet();
        }
    }
}
