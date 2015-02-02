using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Microsoft.Kinect;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class WaitForBodyImpl : BaseImpl<Arg.WaitForBodyArg>
    {
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
    }
}
