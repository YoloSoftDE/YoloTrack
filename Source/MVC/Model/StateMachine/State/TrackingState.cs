using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    class TrackingState : BaseState<Arg.TrackingArg>
    {
        public TrackingState(Arg.TrackingArg arg)
            : base(new Impl.TrackingImpl(), arg)
        { }


        protected override StateTransistion Transist()
        {
            BaseArg result = RunImpl();
            
            if (result.GetType() == typeof(Arg.SwitchTargetArg))
                return new SwitchTargetState((Arg.SwitchTargetArg)result);

            else if (result.GetType() == typeof(Arg.WaitForBodyArg))
                return new WaitForBodyState((Arg.WaitForBodyArg)result);

            return null;
        }
    }
}
