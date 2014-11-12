using System;

namespace FaceTrack.StateMachine
{
    class StartAction : Action
    {
        public override int Perform(ActionArgs a)
        {
            StartActionArgs args = (StartActionArgs)a;
            Console.WriteLine("Start Action");

            return 0;
        }
    }
}
