namespace YoloTrack.MVC.Model.StateMachine.Arg
{
    /// <summary>
    /// Arguments provided to the WaitTakePicture-state
    /// </summary>
    class WaitTakePictureArg : BaseArg
    {
        /// <summary>
        /// Id-reference to the runtime database record which holds the
        /// person for the identification process.
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
            return new WaitTakePictureArg()
            {
                TrackingId = TrackingId
            };
        }
    } // End class
} // End namespace
