using System.Collections.Generic;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    /// <summary>
    /// Implementation of the state logic for WaitForBody
    /// </summary>
    class WaitForBodyImpl : BaseImpl<Arg.WaitForBodyArg>
    {
        /// <summary>
        /// State logic
        /// </summary>
        /// <param name="arg"></param>
        public override void Run(Arg.WaitForBodyArg arg)
        {
            while (true)
            {
                m_sensor.SkeletonStream.Track();
                m_runtime_database.Refresh();

                // Search for those RuntimeInfos that need clarification (maybe identification?)
                foreach (KeyValuePair<int, RuntimeDatabase.Record> entry in m_runtime_database)
                {
                    if (entry.Value.State == RuntimeDatabase.RecordState.Unidentified ||
                        entry.Value.State == RuntimeDatabase.RecordState.Unknown)
                    {
                        // Enable tracking for joint-orientations
                        m_sensor.SkeletonStream.Track(entry.Key);
                        Result = new Arg.WaitTakePictureArg()
                        {
                            TrackingId = entry.Key
                        };
                        return;
                    }
                }
            }
        }
    } // End class
} // End Namespace
