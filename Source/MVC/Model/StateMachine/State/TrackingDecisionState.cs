using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    class TrackingDecisionState : BaseState<Arg.TrackingArg, Arg.TrackingDecisionArg>
    {
        public TrackingDecisionState(Arg.TrackingDecisionArg arg)
            : base(new Impl.TrackingDecisionImpl(), arg)
        { }

        public override IState Transist()
        {
            Arg.TrackingArg result = RunImpl();

            if (result.SkeletonId >= 0)
            {
                return new TrackingState(result);
            }

            return new WaitForBodyState(new Arg.WaitForBodyArg());
        }

        public override States State
        {
            get { return States.TRACKING_DECISION; }
        }
    }
}
