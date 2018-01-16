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

using GrillesGameLibrary;
/// <summary>-</summary>
namespace program
{

    //[System.ComponentModel.DesignerCategory("code")]
    public partial class FormEditor : Form
    {
        //Bitmap
        Texture[] textures;
        LockBitmap inputLB;       
        RenderInfo input;
        RenderInfo result;

        //mouse
        Point lastMousePos;
        Point startMousePos;

        //setings
        float cores = (int)(Environment.ProcessorCount*1.5f);

        //Tasks
        Task renderTask;

        //Rendering Values
        bool isRenering = false;
        byte heightExcess = 255;

        //Rendering orientation
        int angle = 45;
        float tilt = 0.5f;//fixed

        //Editor Values
        bool curTextureEdit;
        bool renderEditor;
        byte editValue = 1;

        public FormEditor()
        {
            InitializeComponent();
            init();
        }
    }
}
