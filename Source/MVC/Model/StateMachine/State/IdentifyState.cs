using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    class IdentifyState : BaseState<Arg.IdentifyArg>
    {
        public IdentifyState(Arg.IdentifyArg arg)
            : base(new Impl.IdentifyImpl(), arg)
        { }

        protected override StateTransistion Transist()
        {
            BaseArg result = RunImpl();

            if (result.GetType() == typeof(Arg.TrackingDecisionArg))
                return new TrackingDecisionState((Arg.TrackingDecisionArg)result);

            else if (result.GetType() == typeof(Arg.LearnArg))
                return new LearnState((Arg.LearnArg)result);

            else if (result.GetType() == typeof(Arg.WaitForBodyArg))
                return new WaitForBodyState((Arg.WaitForBodyArg)result);

            return null;
        }
    }
}
