using System;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    /// <summary>
    /// Implementation of the state logic for 'TrackingDecision'
    /// </summary>
    class TrackingDecisionImpl : BaseImpl<Arg.TrackingDecisionArg>
    {
        /// <summary>
        /// Actual state logic.
        /// </summary>
        /// <param name="arg"></param>
        public override void Run(Arg.TrackingDecisionArg arg)
        {
            // Get person from database...
            Database.Record record = m_database[arg.DatabaseRecordId];
            
            /// ...and check if it is the target
            if (record.IsTarget)
            {
                record.IncrementTimesTracked();

                Result = new Arg.TrackingArg()
                {
                    TrackingId = record.RuntimeRecord.KinectResource.Skeleton.TrackingId
                };
            }
            else
            {
                Result = new Arg.WaitForBodyArg();
            }
        }
    } // End class
} // End namespace
