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
            while (true)
            {
                // refresh skeleton-Data
                Skeleton[] wfb_skeletonData = (Skeleton[])Model.skeletonData.Clone();

                if (wfb_skeletonData == null)
                    continue;

                // Add newly appered Persons (?) to RuntimeDB
                foreach (Skeleton skeleton in wfb_skeletonData)
                {
                    if (skeleton != null && skeleton.TrackingId != 0)
                    {
                        // compare skeleton's ID with RuntimeDatabase
                        if (!Model.RuntimeDatabase.ContainsKey(skeleton.TrackingId))
                        {
                            // add in RuntimeDatabase
                            Model.RuntimeDatabase.Insert(skeleton.TrackingId);
                        }
                    }
                }

                // Remove obsolete RuntimeInfos from RuntimeDB
                foreach (KeyValuePair<int, RuntimeInfo> entry in Model.RuntimeDatabase)
                {
                    bool present = false;
                    foreach (Skeleton skel in wfb_skeletonData)
                    {
                        if (skel.TrackingId == entry.Key)
                            present = true;
                    }
                    if (!present)
                        Model.RuntimeDatabase.Remove(entry.Key);
                }

                // Search for those RuntimeInfos that need clarification (maybe identification?)
                foreach (KeyValuePair<int, RuntimeInfo> entry in Model.RuntimeDatabase)
                {
                    if (entry.Value.State == TrackingState.UNIDENTIFIED ||
                        entry.Value.State == TrackingState.UNKNOWN)
                    {
                        // enable tracking for joint-orientations
                        for (int i = 0; i < Model.skeletonData.Length; i++)
                        {
                            if (Model.skeletonData[i].TrackingId == entry.Key)
                                Model.skeletonData[i].TrackingState = SkeletonTrackingState.Tracked;
                        }

                        // new body found
                        Result.SkeletonId = entry.Key;
                        return;
                    }
                }
            }
        }
    }
}
