using System;

namespace FaceTrack.StateMachine
{
    class TestAction : Action
    {
        public override int Perform(ActionArgs a)
        {
            Console.WriteLine("Oops! This action should never be called.");

            return 0;
        }
    }
}
