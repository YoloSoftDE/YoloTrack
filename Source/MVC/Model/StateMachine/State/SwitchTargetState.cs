using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    class SwitchTargetState : BaseState<Arg.SwitchTargetArg>
    {
        public SwitchTargetState(Arg.SwitchTargetArg arg)
            : base(new Impl.SwitchTargetImpl(), arg)
        { }

        protected override StateTransistion Transist()
        {
            BaseArg result = RunImpl();
            
            if (result.GetType() == typeof(Arg.WaitForBodyArg))
                return new WaitForBodyState((Arg.WaitForBodyArg)result);
            
            else if (result.GetType() == typeof(Arg.TrackingArg))
                return new TrackingState((Arg.TrackingArg)result);

            return null;
        }
    }
}
