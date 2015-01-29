using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class TrackingDecisionImpl : BaseImpl<Arg.TrackingArg, Arg.TrackingDecisionArg>
    {
        public override void Run(Arg.TrackingDecisionArg arg)
        {
            Storage.Person matched = Model.MainDatabase.People.Find(p => p.Id == arg.PersonId);
            int skeletonId = 0;

            if (matched.IsTarget)
            {
                skeletonId = matched.RTInfo.SkeletonId;
            }

            Result.SkeletonId = skeletonId;
        }
    }
}
