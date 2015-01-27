using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine
{
    abstract class BaseState<T, U> : IState
    {
        private BaseImpl<T, U> m_impl;
        private U m_arg;
        protected static BaseState<T, U> m_instance = null;

        public BaseState(BaseImpl<T, U> impl, U arg)
        {
            m_impl = impl;
            m_arg = arg;
        }

        protected T RunImpl()
        {
            //m_impl.Run(m_arg);
            return m_impl.Result;
        }

        public static BaseState<T, U> Instance()
        {
            return m_instance;
        }

        public abstract State.States State { get; }

        public abstract IState Transist();
    }
}
