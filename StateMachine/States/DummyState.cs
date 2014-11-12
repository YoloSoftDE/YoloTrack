using System;

namespace FaceTrack.StateMachine
{
    class DummyState : State, IAcceptState
    {
        public DummyState() : base(new TestAction())
        {
        }

        public override State Transpose()
        {
            return this;
        }

        public string Message()
        {
            return "Test";
        }
    }
}
