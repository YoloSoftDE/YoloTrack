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
            //Thread.Sleep(1000);
        }
    }
}
