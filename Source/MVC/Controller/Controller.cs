using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YoloTrack.MVC.Controller
{
    class Controller
    {
        // Model
        private Model.TrackingModel model;
        // Views
        private View.DebugView debug_view;
        private View.MainView main_view;

        public Controller()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                model = Model.TrackingModel.Instance();
            }
            catch (Cognitec.FRsdk.Exception e)
            {
                MessageBox.Show(e.Message);
            }

            debug_view = new View.DebugView();
            main_view = new View.MainView();

            main_view.initToolStripMenuItem.Click += new EventHandler(initToolStripMenuItem_Click);

            InitStartModel();
            Application.Run(main_view);
        }

        void initToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                model = Model.TrackingModel.Instance();
            }
            catch (Cognitec.FRsdk.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            InitStartModel();
        }

        private void InitStartModel()
        {
            if (model == null)
            {
                main_view.Status = Model.Status.SENSOR_UNAVAILABLE;
                return;
            }

            model.Start();
            if (model.Running())
            {
                main_view.Status = Model.Status.RUNNING;
                debug_view.Observe(model);
                main_view.Observe(model);
            }
            else
                main_view.Status = Model.Status.SENSOR_UNAVAILABLE;
        }
    }
}
