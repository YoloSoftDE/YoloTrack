using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    class SwitchTargetState : BaseState<Arg.TrackingArg, Arg.SwitchTargetArg>
    {
        public SwitchTargetState(Arg.SwitchTargetArg arg)
            : base(new Impl.SwitchTargetImpl(), arg)
        { }

        public override IState Transist()
        {
            Arg.TrackingArg result = RunImpl();
            if (result.SkeletonId != 0)
                return new TrackingState(result);
            else
                return new WaitForBodyState(new Arg.WaitForBodyArg());
        }

        public override States State
        {
            get { return States.SWITCH_TARGET; }
        }
    }
}
