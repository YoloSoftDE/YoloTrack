namespace YoloTrack.MVC.Model.StateMachine.Arg
{
    /// <summary>
    /// Arguments provided to the Tracking-state
    /// </summary>
    class TrackingArg : BaseArg
    {
        /// <summary>
        /// Id-reference to the runtime database entry of the person to be tracked
        /// </summary>
        public int TrackingId;

        /// <summary>
        /// Clones the whole object to ensure that we start with a fresh copy 
        /// into the next state and release everything of the previous
        /// one.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new TrackingArg()
            {
                TrackingId = TrackingId
            };
        }
    } // End class
} // End namespace
