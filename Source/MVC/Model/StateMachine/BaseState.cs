using System;

namespace YoloTrack.MVC.Model.StateMachine
{
    /// <summary>
    /// Base class for the single states of the stateful state machine
    /// </summary>
    /// <typeparam name="TArg"></typeparam>
    abstract class BaseState<TImpl, TArg> : StateTransistion 
        where TArg : new()
        where TImpl : BaseImpl<TArg>, new()
    {
        /// <summary>
        /// Implementation holder
        /// </summary>
        private BaseImpl<TArg> m_impl;

        /// <summary>
        /// Stateful arguments to state
        /// </summary>
        private TArg m_arg;

        /// <summary>
        /// Constructor that only takes the implementation of a state. This is, when the default contrustor for the
        /// argument will work.
        /// </summary>
        /// <param name="impl"></param>
        public BaseState()
            : this(new TArg())
        {            
        }

        /// <summary>
        /// Constructor that takes the instance of the implementation and the arguments passed to at executation
        /// </summary>
        /// <param name="impl"></param>
        /// <param name="arg"></param>
        public BaseState(TArg arg)
        {
            m_impl = new TImpl();
            m_arg = arg;
        }

        /// <summary>
        /// Executes the actual implementation and passes the result
        /// </summary>
        /// <returns></returns>
        protected BaseArg RunImpl()
        {
            // Bind before run
            m_impl.Bind(m_sensor);
            m_impl.Bind(m_identification_api);
            m_impl.Bind(m_runtime_database);
            m_impl.Bind(m_database);
            m_impl.Bind(m_configuration);

            // Run
            m_impl.Run(m_arg);
            return m_impl.Result;
        }
    } // End class
} // End namespace
