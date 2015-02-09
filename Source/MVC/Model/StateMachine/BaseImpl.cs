using System;

namespace YoloTrack.MVC.Model.StateMachine
{
    /// <summary>
    /// Base class for each state logic implementation
    /// </summary>
    /// <typeparam name="U"></typeparam>
    abstract class BaseImpl<U>
    {
        /// <summary>
        /// Variable holding the result of the execution.
        /// </summary>
        private BaseArg result;

        /// <summary>
        /// Sensor to be used
        /// </summary>
        protected Sensor.Model m_sensor;

        /// <summary>
        /// Identification API instance to use.
        /// </summary>
        protected IdentificationData.Model m_identification_api;

        /// <summary>
        /// Identification API instance to use.
        /// </summary>
        protected RuntimeDatabase.Model m_runtime_database;

        /// <summary>
        /// Identification API instance to use.
        /// </summary>
        protected Database.Model m_database;

        /// <summary>
        /// Identification API instance to use.
        /// </summary>
        protected Configuration.Model m_configuration;

        /// <summary>
        /// Property for the result of the execution step.
        /// </summary>
        public BaseArg Result
        {
            get 
            { 
                return (BaseArg)(result.Clone()); 
            }
            set 
            { 
                result = value; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected BaseImpl()
        {
        }

        /// <summary>
        /// Method containing the actual state logic.
        /// </summary>
        /// <param name="arg"></param>
        public abstract void Run(U arg);

        /// <summary>
        /// Binder to an instance of the sensor model
        /// </summary>
        /// <param name="Sensor"></param>
        public void Bind(Sensor.Model Sensor)
        {
            m_sensor = Sensor;
        }

        /// <summary>
        /// Binder to the identification api instance
        /// </summary>
        /// <param name="IdentificationAPI"></param>
        public void Bind(IdentificationData.Model IdentificationAPI)
        {
            m_identification_api = IdentificationAPI;
        }

        /// <summary>
        /// Binder to the runtime database
        /// </summary>
        /// <param name=""></param>
        public void Bind(RuntimeDatabase.Model RuntimeDatabase)
        {
            m_runtime_database = RuntimeDatabase;
        }

        /// <summary>
        /// Binder to the database
        /// </summary>
        /// <param name=""></param>
        public void Bind(Database.Model Database)
        {
            m_database = Database;
        }

        /// <summary>
        /// Binder to the configuration
        /// </summary>
        /// <param name=""></param>
        public void Bind(Configuration.Model Configuration)
        {
            m_configuration = Configuration;
        }
    } // End class
} // End namespace