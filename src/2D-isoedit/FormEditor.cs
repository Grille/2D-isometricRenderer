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
/// <summary>-</summary>
namespace _2Deditor
{
    

    public partial class FormEditor : Form
    {

        Texture[] textures;
        Bitmap inputMap;
        RenderInfo input;
        RenderInfo result;
        Point lastMousePos;

        Stopwatch fps;

        float cores = Environment.ProcessorCount;

        //Tasks
        Task renderTask;

        //Rendering Values
        byte heightExcess = 255;
        byte[] shadowHeightMap;
        byte[] shadowSmoothMap;

        //Rendering orientation
        int angle = 45;
        float tilt = 0.5f;//fixed

        //Editor Values
        bool curTextureEdit;
        bool renderAllInTimer;
        byte editValue = 1;

        public FormEditor()
        {
            InitializeComponent();
            init();
        }
    }
}
