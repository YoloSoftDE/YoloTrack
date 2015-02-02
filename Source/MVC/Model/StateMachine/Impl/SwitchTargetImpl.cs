using System;
using Microsoft.Kinect;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    /// <summary>
    /// State implementatin for 'SwitchTarget'
    /// </summary>
    class SwitchTargetImpl : BaseImpl<Arg.SwitchTargetArg>
    {
        public override void Run(Arg.SwitchTargetArg arg)
        {
            // TODO
            throw new NotImplementedException();
            /*

            KinectSensor kinect = Model.Kinect;
            Arg.TrackingArg res = new Arg.TrackingArg();

            foreach (Skeleton skeleton in skeletons)
            {
                if (skeleton.TrackingId == Model.MainDatabase.Target.RuntimeInfo.Skeleton.TrackingId)
                {
                    skeleton.TrackingState = SkeletonTrackingState.Tracked;
                    res.SkeletonId = skeleton.TrackingId;
                    break;
                }
            }
            Result = res;
            return;
             */

            // TODO: adopt to the "new" concept, that all data is obtained from the runtimedatabase
            throw new NotImplementedException();
        }
    } // End class
} // End namespace
