using System;
using System.Threading;
using YoloTrack.MVC.Controller;
using YoloTrack.MVC.Model.Configuration;

namespace YoloTrack.MVC.Model.StateMachine
{
    /// <summary>
    /// 
    /// </summary>
    public class StateChangeEventArgs : EventArgs
    {
        public StateTransistion PreviousState;
        public StateTransistion NextState;
    }

    /// <summary>
    /// 
    /// </summary>
    public class StateMachineException : Exception
    {
        public StateMachineException(string Message)
            : base(Message)
        {
        }
    }

    /// <summary>
    /// State machine model
    /// </summary>
    public class Model : IConfigurable, 
        IBindable<Configuration.Model>, 
        IBindable<Sensor.Model>, 
        IBindable<IdentificationData.Model>, 
        IBindable<RuntimeDatabase.Model>, 
        IBindable<Database.Model>
    {
        /// <summary>
        /// Fired on each state change.
        /// </summary>
        public event EventHandler<StateChangeEventArgs> StateChange;

        /// <summary>
        /// Worker thread containing the Statemachine logic.
        /// </summary>
        private Thread m_worker;

        /// <summary>
        /// Stop flag to gracefully stop the state machine.
        /// </summary>
        private bool m_terminate;

        /// <summary>
        /// Application configuration
        /// </summary>
        private Configuration.Model m_app_conf;

        /// <summary>
        /// Sensor to be used
        /// </summary>
        private Sensor.Model m_sensor;

        /// <summary>
        /// 
        /// </summary>
        /// 
        protected Database.Model m_database;
        /// <summary>
        /// 
        /// </summary>
        protected RuntimeDatabase.Model m_runtime_database;

        /// <summary>
        /// Identification API instance to use.
        /// </summary>
        private IdentificationData.Model m_identification_api;

        /// <summary>
        /// Starts the state machine.
        /// </summary>
        public void Start()
        {
            if (m_worker != null)
                throw new StateMachineException("State machine already running");

            if (m_app_conf == null)
                throw new StateMachineException("Appconfig not bound");

            if (m_identification_api == null)
                throw new StateMachineException("No identification API bound");

            if (m_sensor == null)
                throw new StateMachineException("No sensor bound");

            m_terminate = false;
            m_worker = new Thread(new ThreadStart(_work));
            m_worker.Name = "StateMachine Worker";
            m_worker.Start();
        }

        /// <summary>
        /// Gracefully terminates the state machine. This may take up to one state to take effekt.
        /// </summary>
        public void Terminate()
        {
            m_terminate = true;
            m_worker.Join();
        }

        /// <summary>
        /// Aborts the state machine instantly.
        /// </summary>
        public void Abort()
        {
            m_worker.Abort();
        }

        /// <summary>
        /// Worker class for the actual state machine process.
        /// </summary>
        private void _work()
        {
            // Set the start state/point
            StateTransistion next_state = new State.WaitForBodyState();

            while (!m_terminate)
            {
                // Transist state
                StateTransistion current_state = next_state;
                current_state.Bind(m_identification_api);
                current_state.Bind(m_sensor);
                current_state.Bind(m_runtime_database);
                current_state.Bind(m_database);
                current_state.Bind(m_app_conf);
                next_state = current_state.Next();

                // Fire event 'OnStateChange'
                if (StateChange != null)
                {
                    StateChange(this, new StateChangeEventArgs()
                    {
                        PreviousState = current_state, 
                        NextState = next_state
                    });
                }
            }
        }

        /// <summary>
        /// Binder to an instance of the sensor model
        /// </summary>
        /// <param name="Sensor"></param>
        public void Bind(Sensor.Model Sensor)
        {
            m_sensor = Sensor;
        }

        /// <summary>
        /// Binder to the configuration models instance
        /// </summary>
        /// <param name="Configuration"></param>
        public void Bind(Configuration.Model Configuration)
        {
            m_app_conf = Configuration;
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
        /// <param name="IdentificationAPI"></param>
        public void Bind(RuntimeDatabase.Model RuntimeDatabase)
        {
            m_runtime_database = RuntimeDatabase;
        }

        /// <summary>
        /// Binder to the database
        /// </summary>
        /// <param name="IdentificationAPI"></param>
        public void Bind(Database.Model Database)
        {
            m_database = Database;
        }
    } // End class
} // End namespace
