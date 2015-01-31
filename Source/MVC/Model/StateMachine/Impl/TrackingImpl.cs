using Microsoft.Kinect;
using System;
using System.Threading;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class TrackingImpl : BaseImpl<Arg.TrackingArg>
    {        
        public override void Run(Arg.TrackingArg arg)
        {
            Skeleton skeleton = Model.RuntimeDatabase[arg.SkeletonId].Skeleton;

            while (true)
            {
                if (skeleton.TrackingState == SkeletonTrackingState.NotTracked) 
                {
                    Result = new Arg.WaitForBodyArg();
                    break;
                }

                if (Model.RuntimeDatabase[arg.SkeletonId].Person.IsTarget == false)
                {
                    Result = new Arg.SwitchTargetArg();
                    break;
                } 
            };

            return;
        }
    }
}
