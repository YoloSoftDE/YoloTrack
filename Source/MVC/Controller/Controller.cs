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
        Model.TrackingModel model;
        Model.ConfigModel config;

        // Views
        View.DebugView debug_view;
        View.MainView main_view;

        public Controller()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            config = Model.ConfigModel.Instance();

            CreateModel();

            debug_view = new View.DebugView();
            main_view = new View.MainView();

            main_view.initToolStripMenuItem.Click += new EventHandler(initToolStripMenuItem_Click);

            InitStartModel();
            Application.Run(main_view);

            config.Save();
        }

        private void CreateModel()
        {
            try
            {
                model = Model.TrackingModel.Instance();
                model.MainDatabase.LoadFromFile(config.conf.DatabaseFilename);
                model.MainDatabase.PersonChanged += new EventHandler(MainDatabase_PersonChanged);
            }
            catch (Cognitec.FRsdk.Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void MainDatabase_PersonChanged(object sender, EventArgs e)
        {
            model.MainDatabase.SaveToFile(config.conf.DatabaseFilename);
        }

        void initToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateModel();
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
