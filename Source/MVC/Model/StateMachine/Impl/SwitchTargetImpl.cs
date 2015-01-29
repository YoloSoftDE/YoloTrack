using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Microsoft.Kinect;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class SwitchTargetImpl : BaseImpl<Arg.TrackingArg, Arg.SwitchTargetArg>
    {
        public override void Run(Arg.SwitchTargetArg arg)
        {
            // TODO
            KinectSensor kinect = Model.Kinect;
            Skeleton[] skeletons = Model.skeletonData;
            Arg.TrackingArg res = new Arg.TrackingArg();

            foreach (Skeleton skeleton in skeletons)
            {
                if (skeleton.TrackingId == Model.MainDatabase.Target.RTInfo.SkeletonId)
                {
                    skeleton.TrackingState = SkeletonTrackingState.Tracked;
                    res.SkeletonId = skeleton.TrackingId;
                    break;
                }
            }
            Result = res;
            return;
        }
    }
}
