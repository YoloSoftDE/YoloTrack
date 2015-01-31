using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Microsoft.Kinect;
using YoloTrack.MVC.Model.Storage;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class WaitForBodyImpl : BaseImpl<Arg.WaitForBodyArg>
    {
        public override void Run(Arg.WaitForBodyArg arg)
        {
            while (true)
            {
                Model.Kinect.SkeletonStream.AppChoosesSkeletons = true;
                Model.Kinect.SkeletonStream.ChooseSkeletons();
                Model.RuntimeDatabase.Refresh();

                // Search for those RuntimeInfos that need clarification (maybe identification?)
                foreach (KeyValuePair<int, RuntimeInfo> entry in Model.RuntimeDatabase)
                {
                    if (entry.Value.State == TrackingState.UNIDENTIFIED ||
                        entry.Value.State == TrackingState.UNKNOWN)
                    {
                        // Enable tracking for joint-orientations
                        entry.Value.Watch();
                        Result = new Arg.WaitTakePictureArg()
                        {
                            SkeletonId = entry.Key
                        };
                        return;
                    }
                }
            }
        }
    }
}
