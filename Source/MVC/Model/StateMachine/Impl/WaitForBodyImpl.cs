using System.Collections.Generic;
using System.Threading;

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
                RuntimeDatabase.Record record = m_runtime_database.SelectLeastIdentifyAttempts();
                if (record != null)
                {
                    // Enable tracking for joint-orientations
                    m_sensor.SkeletonStream.Track(record.KinectResource.Skeleton.TrackingId);
                    Result = new Arg.WaitTakePictureArg()
                    {
                        TrackingId = record.KinectResource.Skeleton.TrackingId
                    };
                    return;
                }

                // Iterate again and search for identified targets
                if (m_database.HasTarget)
                {
                    foreach (KeyValuePair<int, RuntimeDatabase.Record> entry in m_runtime_database)
                    {
                        if (entry.Value.State == RuntimeDatabase.RecordState.Identified && entry.Value.DatabaseRecord.IsTarget)
                        {
                            Result = new Arg.TrackingArg()
                            {
                                TrackingId = entry.Value.KinectResource.Skeleton.TrackingId
                            };
                            return;
                        }
                    }

                }

                Thread.Sleep(50);
            }
        }
    } // End class
} // End Namespace
