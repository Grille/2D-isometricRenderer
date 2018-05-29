using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace program
{
    public static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>    
        //Application.SetCompatibleTextRenderingDefault(false);
        public static FormEditor MainForm = new FormEditor();
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            
            Application.Run(MainForm);
        }
    }
}
