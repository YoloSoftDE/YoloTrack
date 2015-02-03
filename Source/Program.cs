using System;
using System.Windows.Forms;
using ApplicationView = YoloTrack.MVC.View.Application.View;

namespace YoloTrack
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ApplicationView());
        }
    }
}
