using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YoloTrack.MVC.Controller
{
    class Controller
    {
        public Controller()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            View.DebugView debug_view = new View.DebugView();
            View.MainView main_view = new View.MainView();
            
            /*
            Model.TrackingModel model = Model.TrackingModel.Instance();

            debug_view.Observe(model);
            main_view.Observe(model);

            model.Start();

            if (model.Running())
                main_view.Status = Model.Status.RUNNING;
            else
                main_view.Status = Model.Status.SENSOR_UNAVAILABLE;
            */
            Application.Run(main_view);
        }

    }
}
