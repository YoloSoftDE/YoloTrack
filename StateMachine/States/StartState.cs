using System;

namespace FaceTrack.StateMachine
{
    class StartState : State
    {
        public StartState() : base(new StartAction())
        {
        }

        public override State Transpose()
        {
            StartActionArgs args = new StartActionArgs();
            args.Test = "Information to pass";

            switch (Perform(args)) {
                case 0:
                    return this;
                default:
                    return new DummyState();
            }
        }
    }
}
