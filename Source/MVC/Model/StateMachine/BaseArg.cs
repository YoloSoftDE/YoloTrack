using System;

namespace YoloTrack.MVC.Model.StateMachine
{
    /// <summary>
    /// Basic class for the state implementations depending arguments.
    /// </summary>
    class BaseArg : ICloneable
    {
        public virtual object Clone()
        {
            throw new NotImplementedException();
        }
    } // End class
} // End namespace
