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
        DependencyManager m_dependency_manager;

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
            m_dependency_manager = new DependencyManager();

            // Dependencies for the State Machine
            m_dependency_manager.AddDependent(
                () => m_state_machine,
                d =>
                {
                    d.Initialize(() => _initialize_state_machine());

                    d.Bind(() => m_sensor);
                    d.Bind(() => m_configuration);
                    d.Bind(() => m_identification_data);
                    d.Bind(() => m_runtime_database);
                    d.Bind(() => m_database);

                    d.Finalize(() =>
                    {
                        m_state_machine.Start();
                    });
                }
            );

            // Dependencies for the Sensor
            m_dependency_manager.AddDependent(
                () => m_sensor,
                d =>
                {
                    d.Initialize(() => _initialize_sensor());
                }
            );

            // Dependencies for the Indentification API
            m_dependency_manager.AddDependent(
                () => m_identification_data,
                d =>
                {
                    d.Initialize(() => _initialize_identification_api());
                }
            );

            // Dependencies for the Runtime Database
            m_dependency_manager.AddDependent(
                () => m_runtime_database,
                d =>
                {
                    d.Initialize(() => _initialize_runtime_database());

                    d.Bind(() => m_sensor);
                }
            );

            // Dependencies for the Database
            m_dependency_manager.AddDependent(
                () => m_database,
                d =>
                {
                    d.Initialize(() => _initialize_database());

                    d.Finalize(() =>
                    {
                        m_database.RecordAdded += new EventHandler<Model.Database.RecordAddedEventArgs>(OnDatabaseRecordAdded);
                        m_database.RecordRemoved += new EventHandler<Model.Database.RecordRemovedEventArgs>(OnDatabaseRecordRemoved);
                    });
                }
            );

            // Dependencies for the Configuration
            m_dependency_manager.AddDependent(
                () => m_configuration,
                d =>
                {
                    d.Initialize(() => _initialize_configuration());
                }
            );

            // Dependencies for the Debug View
            m_dependency_manager.AddDependent(
                () => m_debug_view,
                d =>
                {
                    d.Initialize(() => _initialize_debug_view());

                    d.Bind(() => m_configuration);
                    d.Observe(() => m_state_machine);
                    d.Observe(() => m_sensor);
                    d.Observe(() => m_runtime_database);
                    d.Observe(() => m_identification_data);
                    d.Observe(() => m_database);
                }
            );

            // Dependencies for the Application View
            m_dependency_manager.AddDependent(
                () => m_app_view,
                d =>
                {
                    d.Initialize(() => _initialize_application_view());

                    d.Bind(() => m_sensor);
                    d.Bind(() => m_runtime_database);
                    d.Bind(() => m_database);
                    d.Observe(() => m_state_machine);
                    d.Observe(() => m_sensor);
                    d.Observe(() => m_runtime_database);
                    d.Observe(() => m_identification_data);
                    d.Observe(() => m_database);
                }
            );


            m_dependency_manager.FixAll();

            m_app_view.RepeatInitTimeout += m_app_view_RepeatInitTimeout;

            if (m_sensor == null)
            {
                m_app_view.ShowSensorFailure(m_error_message);
            }

            m_configuration.Options.Logging.LogLevel = View.Debug.DebugLevel.Info;


            Application.Run(m_app_view);
        }

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
                m_error_message = ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool _initialize_debug_view()
        {
            // Init Debug view
            m_debug_view = new DebugView();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool _initialize_application_view()
        {
            // Init Application view
            m_app_view = new ApplicationView();

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

        #region View Event handlers

        void m_app_view_RepeatInitTimeout(object sender, EventArgs e)
        {
            m_app_view.HideFailureMessage();
            m_dependency_manager.FixAll();
        }

        string m_error_message = "";

        #endregion
    }
}
