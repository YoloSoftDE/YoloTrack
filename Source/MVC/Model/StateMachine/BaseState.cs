using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine
{
    abstract class BaseState<U> : StateTransistion
    {
        private BaseImpl<U> m_impl;
        private U m_arg;
        protected static BaseState<U> m_instance = null;

        public BaseState(BaseImpl<U> impl, U arg)
        {
            m_impl = impl;
            m_arg = arg;
        }

        protected BaseArg RunImpl()
        {
            m_impl.Run(m_arg);
            return m_impl.Result;
        }

        public static BaseState<U> Instance()
        {
            return m_instance;
        }
    }
}
