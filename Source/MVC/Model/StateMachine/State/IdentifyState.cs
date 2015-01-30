using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    class IdentifyState : BaseState<Arg.TrackingDecisionArg, Arg.IdentifyArg>
    {
        public IdentifyState(Arg.IdentifyArg arg)
            : base(new Impl.IdentifyImpl(), arg)
        { }

        public override IState Transist()
        {
            Arg.TrackingDecisionArg result = RunImpl();

            // TODO: much.
            if (result.PersonId != Storage.Person.fail)
            {
                return new TrackingDecisionState(result);
            }

            return new WaitForBodyState(new Arg.WaitForBodyArg());
        }

        public override States State
        {
            get { return States.IDENTIFY; }
        }
    }
}
