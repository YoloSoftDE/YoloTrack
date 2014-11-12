using System;

namespace FaceTrack.StateMachine
{
    abstract class Action
    {
        public abstract int Perform(ActionArgs a = null);
    }
}
