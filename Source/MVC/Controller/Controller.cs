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
    struct ModelState
    {
        public bool sensor_ok;
        public bool identification_api_ok;
        public bool database_ok;
        public bool runtime_database_ok;
        public bool state_machine_ok;
        public bool configuration_ok;
    }

    class Controller
    {
        ModelState m_model_state;

        DependencyManager m_dependeny_manager;

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
            m_model_state = new ModelState();
            m_dependeny_manager = new DependencyManager();

            #region Instanciate views

            // Init Debug view
            m_debug_view = new DebugView();
            // Init Application view
            m_app_view = new ApplicationView();

            #endregion

            #region Init models

            // Load the configuration from default config file
            m_model_state.configuration_ok = _initialize_configuration();
            
            // Grab the sensor & init
            m_model_state.sensor_ok = _initialize_sensor();
            
            // Instanciate the identification data model
            m_model_state.identification_api_ok = _initialize_identification_api();

            // Initialize the Database
            m_model_state.database_ok = _initialize_database();
            
            // Initialize the runtime database
            m_model_state.runtime_database_ok = _initialize_runtime_database();

            // Start the state machine
            m_model_state.state_machine_ok = _initialize_state_machine();

            #endregion

            m_dependeny_manager.AddDependencyStaticBind(m_state_machine, m_sensor);

            m_dependeny_manager.TryHandle(m_state_machine);


            /*
            m_dependeny_manager.AddDependent(m_state_machine, dependent =>
            {
                dependent.Bind(m_sensor);
                dependent.Finalize();
            });
            */


            #region Dependency terror

            // Handle reserse-dependencies
            if (m_model_state.configuration_ok)
            {
                _late_init_configuration_dependents();
                //_late_init_configuration_dependencies();
            }

            // Handle reserse-dependencies
            if (m_model_state.sensor_ok)
            {
                _late_init_sensor_dependents();
                //_late_init_sensor_dependencies();
            }

            // Handle reserse-dependencies
            if (m_model_state.identification_api_ok)
            {
                _late_init_identification_api_dependents();
                //_late_init_identification_api_dependencies();
            }

            // Handle reserse-dependencies
            if (m_model_state.database_ok)
            {
                _late_init_database_dependents();
                //_late_init_database_dependencies();
            }

            // Handle reserse-dependencies
            if (m_model_state.runtime_database_ok)
            {
                _late_init_runtime_database_dependents();
                //_late_init_runtime_database_dependencies();
            }

            // Handle reserse-dependencies
            if (m_model_state.state_machine_ok)
            {
                _late_init_state_machine_dependents();
                //_late_init_state_machine_dependencies();
            }

            #endregion

            m_configuration.Options.Logging.LogLevel = View.Debug.DebugLevel.Info;

            m_app_view.FormClosing += new FormClosingEventHandler(m_app_view_FormClosing);


            m_state_machine.Start(); // !!!!!!!!!!!
            Application.Run(m_app_view);
        }

        void m_app_view_FormClosing(object sender, FormClosingEventArgs e)
        {
            /* Main Form Requested Closing*/

            /* Shutdown State Machine */
            m_state_machine.Terminate();

            /* Shutdown Kinect Sensor */
            // #ichkannohneapinichtarbeiten!
            
            /* Shutdown Cognitec API */
            /* Nothing to do here */

            /* Shutdown Database */

            /* > Store Database as file */
            m_database.SaveTo("test.ydb");

        }

        #region Late init dependents

        /// <summary>
        /// Satisfies the dependents of the State Machine
        /// </summary>
        /// <returns></returns>
        private void _late_init_state_machine_dependents()
        {
            m_debug_view.Observe(m_state_machine);
            m_app_view.Observe(m_state_machine);
        }

        /// <summary>
        /// Satisfies the dependents of the Runtime Database
        /// </summary>
        /// <returns></returns>
        private void _late_init_runtime_database_dependents()
        {
            if (m_model_state.state_machine_ok)
            {
                m_state_machine.Bind(m_runtime_database);
            }

            m_debug_view.Observe(m_runtime_database);
            m_app_view.Bind(m_runtime_database);
            m_app_view.Observe(m_runtime_database);
        }

        /// <summary>
        /// Satisfies the dependents of the Database
        /// </summary>
        /// <returns></returns>
        private void _late_init_database_dependents()
        {
            m_database.RecordAdded += new EventHandler<Model.Database.RecordAddedEventArgs>(OnDatabaseRecordAdded);
            m_database.RecordRemoved += new EventHandler<Model.Database.RecordRemovedEventArgs>(OnDatabaseRecordRemoved);

            m_debug_view.Observe(m_database);
            m_app_view.Bind(m_database);
            m_app_view.Observe(m_database);
        }

        /// <summary>
        /// Satisfies the dependents of the Identification API
        /// </summary>
        /// <returns></returns>
        private void _late_init_identification_api_dependents()
        {
            if (m_model_state.state_machine_ok)
            {
                m_state_machine.Bind(m_identification_data);
            }

            m_debug_view.Observe(m_identification_data);
            m_app_view.Observe(m_identification_data);
        }

        /// <summary>
        /// Satisfies the dependents of the Configuration
        /// </summary>
        /// <returns></returns>
        private void _late_init_configuration_dependents()
        {
            m_debug_view.Bind(m_configuration);
        }

        /// <summary>
        /// Satisfies the dependents of the Sensor
        /// </summary>
        /// <returns></returns>
        private void _late_init_sensor_dependents()
        {
            if (m_model_state.state_machine_ok)
            {
                m_state_machine.Bind(m_sensor);
            }

            if (m_model_state.runtime_database_ok)
            {
                m_runtime_database.Bind(m_sensor);
            }

            m_app_view.Bind(m_sensor);
            m_debug_view.Observe(m_sensor);
            m_app_view.Observe(m_sensor);
        }

        #endregion

        #region Late init dependencies

        /// <summary>
        /// Late init / assign of the dependencies for Configuration
        /// </summary>
        /// <returns></returns>
        private void _late_init_configuration_dependencies()
        {
            // Nothing for now
        }

        /// <summary>
        /// Late init / assign of the dependencies for Sensor
        /// </summary>
        /// <returns></returns>
        private void _late_init_sensor_dependencies()
        {
            // Nothing for now
        }

        /// <summary>
        /// Late init / assign of the dependencies for Database
        /// </summary>
        /// <returns></returns>
        private void _late_init_database_dependencies()
        {
            // Nothing for now
        }

        /// <summary>
        /// Late init / assign of the dependencies for Runtime Database
        /// </summary>
        /// <returns></returns>
        private void _late_init_runtime_database_dependencies()
        {
            // Nothing for now
        }

        /// <summary>
        /// Late init / assign of the dependencies for State Machine
        /// </summary>
        /// <returns></returns>
        private void _late_init_state_machine_dependencies()
        {
            if (m_model_state.configuration_ok)
            {
                m_state_machine.Bind(m_configuration);
            }

            if (m_model_state.database_ok)
            {
                m_state_machine.Bind(m_database);
            }

            if (m_model_state.sensor_ok)
            {
                m_state_machine.Bind(m_sensor);
            }
        }

        /// <summary>
        /// Late init / assign of the dependencies for Identification API
        /// </summary>
        /// <returns></returns>
        private void _late_init_identification_api_dependencies()
        {
            // Nothing for now
        }

        #endregion

        #region Initializations

        /// <summary>
        /// Initializes the Configuration
        /// </summary>
        /// <returns></returns>
        private bool _initialize_configuration()
        {
            try
            {
                m_configuration = Configuration.LoadDefault();
            } 
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Initializes the State Machine
        /// </summary>
        /// <returns></returns>
        private bool _initialize_state_machine()
        {
            try
            {
                m_state_machine = new StateMachine();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Initializes the Runtime Database
        /// </summary>
        /// <returns></returns>
        private bool _initialize_runtime_database()
        {
            try
            {
                m_runtime_database = new RuntimeDatabase();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Initializes the Database
        /// </summary>
        /// <returns></returns>
        private bool _initialize_database()
        {
            try
            {
                m_database = Database.LoadFromOrEmpty("test.ydb", m_identification_data); //m_configuration.Options.Database.FileName);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Initializes the Identification API
        /// </summary>
        /// <returns></returns>
        private bool _initialize_identification_api()
        {
            try
            {
                m_identification_data = new IdentificationData("frsdk.cfg"); // m_configuration.Options.IdentificationData.ConfigurationFileName
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Initializes the sensor
        /// </summary>
        /// <returns></returns>
        private bool _initialize_sensor()
        {
            try
            {
                m_sensor = Sensor.GrabAny();
                m_sensor.Initialize();
            }
            catch (Model.Sensor.SensorException ex)
            {
                m_app_view.ShowSensorFailure(ex.Message);
                return false;
            }

            return true;
        }

        #endregion

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
