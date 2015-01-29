using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Microsoft.Kinect;
using YoloTrack.MVC.Model.Storage;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class WaitForBodyImpl : BaseImpl<Arg.WaitTakePictureArg, Arg.WaitForBodyArg>
    {
        public override void Run(Arg.WaitForBodyArg arg)
        {
            Skeleton[] wfb_skeletonData = Model.skeletonData;
            bool body_in_list = false;
            RuntimeInfo run_info = new RuntimeInfo();
            Arg.WaitTakePictureArg res = new Arg.WaitTakePictureArg();

            //Thread.Sleep(1000);
            while (true)
            {
                foreach (Skeleton skeleton in wfb_skeletonData)
                {
                    if (skeleton != null)
                    {
                        // compare skeleton's ID with RuntimeDatabase
                        foreach (Storage.RuntimeInfo info in Model.RuntimeDatabase.Info)
                        {
                            if (skeleton.TrackingId == info.SkeletonId)
                            {
                                body_in_list = true;
                            }
                        }

                        if (body_in_list == false)
                        {
                            // enable tracking for joint-orientations
                            skeleton.TrackingState = SkeletonTrackingState.Tracked;

                            // new body found
                            res.SkeletonId = skeleton.TrackingId;
                            Result = res;

                            // add in RuntimeDatabase
                            run_info.SkeletonId = skeleton.TrackingId;
                            Model.RuntimeDatabase.Add(run_info);
                            return;
                        }
                    }
                }
            }
        }
    }
}
