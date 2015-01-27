using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace YoloTrack
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MVC.Controller.Controller controller = new MVC.Controller.Controller();
        }
    }
}
