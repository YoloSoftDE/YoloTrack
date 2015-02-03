#region Usings

using System;
using System.Windows.Forms;

using Sensor = YoloTrack.MVC.Model.Sensor.Model;
using StateMachine = YoloTrack.MVC.Model.StateMachine.Model;
using Configuration = YoloTrack.MVC.Model.Configuration.Model;
using IdentificationData = YoloTrack.MVC.Model.IdentificationData.Model;
using RuntimeDatabase = YoloTrack.MVC.Model.RuntimeDatabase.Model;
using Database = YoloTrack.MVC.Model.Database.Model;

using DebugView = YoloTrack.MVC.View.Debug.View;
using ApplicationView = YoloTrack.MVC.View.Application.View;

#endregion

namespace YoloTrack.MVC.Controller
{
    class Controller
    {
        #region Models

        /// <summary>
        /// Sensor model instance.
        /// </summary>
        Sensor m_sensor;

        /// <summary>
        /// StateMachine Model instance.
        /// </summary>
        StateMachine m_state_machine;

        /// <summary>
        /// Application configuration model instance.
        /// </summary>
        Configuration m_configuration;

        /// <summary>
        /// Identification processing API.
        /// </summary>
        IdentificationData m_identification_data;

        /// <summary>
        /// Database containing runtime information.
        /// </summary>
        RuntimeDatabase m_runtime_database;

        /// <summary>
        /// ...
        /// </summary>
        Database m_database;

        #endregion

        #region Views

        /// <summary>
        /// Main view of the application.
        /// </summary>
        ApplicationView m_app_view;

        /// <summary>
        /// Debug output window.
        /// </summary>
        DebugView m_debug_view;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public Controller()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            #region Init models

            // Load the configuration from default config file
            m_configuration = Configuration.LoadDefault();

            // Grab the sensor & init
            m_sensor = Sensor.GrabAny();
            m_sensor.Initialize();

            // Instanciate the identification data model
            m_identification_data = new IdentificationData("frsdk.cfg"); // m_configuration.Options.IdentificationData.ConfigurationFileName

            // Initialize the Database
            m_database = Database.LoadFromOrEmpty("test.ydb", m_identification_data); //m_configuration.Options.Database.FileName);
            m_database.RecordAdded += new EventHandler<Model.Database.RecordAddedEventArgs>(OnDatabaseRecordAdded);
            m_database.RecordRemoved += new EventHandler<Model.Database.RecordRemovedEventArgs>(OnDatabaseRecordRemoved);
            
            // Initialize the runtime database
            m_runtime_database = new RuntimeDatabase();
            m_runtime_database.Bind(m_sensor);

            // Start the state machine
            m_state_machine = new StateMachine();
            m_state_machine.Bind(m_configuration);
            m_state_machine.Bind(m_sensor);
            m_state_machine.Bind(m_identification_data);
            m_state_machine.Bind(m_runtime_database);
            m_state_machine.Bind(m_database);

            #endregion

            #region Init views

            // Init Debug view
            m_debug_view = new DebugView();
            m_debug_view.Bind(m_configuration);
            m_debug_view.Observe(m_state_machine);
            m_debug_view.Observe(m_sensor);
            m_debug_view.Observe(m_runtime_database);
            m_debug_view.Observe(m_identification_data);
            m_debug_view.Observe(m_database);

            // Init Application view
            m_app_view = new ApplicationView();
            /*
            m_app_view.Bind(m_sensor);
            m_app_view.Bind(m_runtime_database);
            m_app_view.Bind(m_database);
            m_app_view.Observe(m_state_machine);
            m_app_view.Observe(m_sensor);
            m_app_view.Observe(m_runtime_database);
            m_app_view.Observe(m_identification_data);
            m_app_view.Observe(m_database);
            */

            #endregion

            m_configuration.Options.Logging.LogLevel = View.Debug.DebugLevel.Info;

            m_state_machine.Start();
            Application.Run(m_app_view);
        }

        #region Model Event handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDatabaseRecordAdded(object sender, Model.Database.RecordAddedEventArgs e)
        {
            m_database.SaveTo("test.ydb");

            e.Record.RecordChanged += new EventHandler<Model.Database.RecordChangedEventArgs>(OnDatabaseRecordChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDatabaseRecordChanged(object sender, Model.Database.RecordChangedEventArgs e)
        {
            m_database.SaveTo("test.ydb");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDatabaseRecordRemoved(object sender, Model.Database.RecordRemovedEventArgs e)
        {
            m_database.SaveTo("test.ydb");
        }

        #endregion
    }
}
