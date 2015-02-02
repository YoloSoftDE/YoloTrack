namespace YoloTrack.MVC.Model.StateMachine.Arg
{
    /// <summary>
    /// Arguments provided to the TrackingDescision-state
    /// </summary>
    class TrackingDecisionArg : BaseArg
    {
        /// <summary>
        /// Id of the record in the database
        /// </summary>
        public int DatabaseRecordId;

        /// <summary>
        /// Clones the whole object to ensure that we start with a fresh copy 
        /// into the next state and release everything of the previous
        /// one.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new TrackingDecisionArg()
            {
                DatabaseRecordId = DatabaseRecordId
            };
        }
    } // End class
} // End namespace
