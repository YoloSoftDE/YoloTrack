using System;
using FaceTrack.StateMachine;

namespace FaceTrack
{
    class Program
    {
        static void Main(string[] args)
        {
            State state = new StartState();
            
            while (!(state is IAcceptState))
                state = state.Transpose();

            IAcceptState accept_state = (IAcceptState)state;
            Console.WriteLine("*** Error: " + accept_state.Message() + " ***");
        }
    }
}
