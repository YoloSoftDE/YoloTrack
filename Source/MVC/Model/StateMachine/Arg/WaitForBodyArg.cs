namespace YoloTrack.MVC.Model.StateMachine.Arg
{
    /// <summary>
    /// Arguemnts provided to the WaitForBody-state
    /// </summary>
    class WaitForBodyArg : BaseArg
    {
        /// <summary>
        /// Clones the whole object to ensure that we start with a fresh copy 
        /// into the next state and release everything of the previous
        /// one.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new WaitForBodyArg();
        }
    } // End class
} // End namespace
