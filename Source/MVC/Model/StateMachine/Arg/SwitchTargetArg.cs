namespace YoloTrack.MVC.Model.StateMachine.Arg
{
    /// <summary>
    /// Arguments provided to the SwitchTarget-state
    /// </summary>
    class SwitchTargetArg : BaseArg
    {
        /// <summary>
        /// Clones the whole object to ensure that we start with a fresh copy 
        /// into the next state and release everything of the previous
        /// one.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new SwitchTargetArg();
        }
    } // End class
} // End namespace