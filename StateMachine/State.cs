using System;

namespace FaceTrack.StateMachine
{
    abstract class State
    {
        private Action m_action;

        public State(Action action)
        {
            m_action = action;
        }

        abstract public State Transpose();

        protected int Perform(ActionArgs a = null)
        {
            return m_action.Perform(a);
        }
    }
}
