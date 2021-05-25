using System;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;

namespace Program
{
    public static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>    
        //Application.SetCompatibleTextRenderingDefault(false);
        public static FormEditor MainForm;
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm = new FormEditor();
            Application.Run(MainForm);
        }
    }
}
