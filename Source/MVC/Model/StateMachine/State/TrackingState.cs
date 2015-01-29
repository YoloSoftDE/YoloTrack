using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    class TrackingState : BaseState<Arg.WaitForBodyArg, Arg.TrackingArg>
    {
        public TrackingState(Arg.TrackingArg arg)
            : base(new Impl.TrackingImpl(), arg)
        { }


        public override IState Transist()
        {
            Arg.WaitForBodyArg result = RunImpl();
            if (result.FocusLost)
                return new WaitForBodyState(result);
            else
                return new SwitchTargetState(new Arg.SwitchTargetArg());
        }

       public override States State
        {
            get { return States.TRACK; }
        }
    }
}
