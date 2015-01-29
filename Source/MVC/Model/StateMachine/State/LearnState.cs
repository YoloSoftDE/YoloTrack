using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    class LearnState : BaseState<Arg.WaitForBodyArg, Arg.LearnArg>
    {
        public LearnState(Arg.LearnArg arg)
            : base(new Impl.LearnImpl(), arg)
        { }

        public override IState Transist()
        {
            Arg.WaitForBodyArg result = RunImpl();

           
        }

        public override States State
        {
            get { return States.LEARN; }
        }
    }
}
