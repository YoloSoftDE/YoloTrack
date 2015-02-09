namespace YoloTrack.MVC.Model.StateMachine
{
    /// <summary>
    /// State transistoin logic class
    /// </summary>
    public abstract class StateTransistion
    {
        /// <summary>
        /// Sensor to be used
        /// </summary>
        protected Sensor.Model m_sensor;

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
        protected IdentificationData.Model m_identification_api;

        /// <summary>
        /// 
        /// </summary>
        protected Configuration.Model m_configuration;

        /// <summary>
        /// State transistion that must be implemented by each state (state transistion logic)
        /// </summary>
        /// <returns></returns>
        protected abstract StateTransistion Transist();

        /// <summary>
        /// Simple wrapper around the transition to prevent fail-states.
        /// </summary>
        /// <returns></returns>
        public StateTransistion Next()
        {
            // Transist
            StateTransistion next_state = Transist();

            // Fail-check
            if (next_state == null)
                throw new StateMachineException("StateMachine entered fail state");

            return next_state;
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

        /// <summary>
        /// Binder to the configurtaion
        /// </summary>
        /// <param name="IdentificationAPI"></param>
        public void Bind(Configuration.Model Configuration)
        {
            m_configuration = Configuration;
        }
    } // End class
} // End namespace
