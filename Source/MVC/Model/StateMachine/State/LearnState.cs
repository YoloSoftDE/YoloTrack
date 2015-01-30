using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    class LearnState : BaseState<Arg.LearnArg>
    {
        public LearnState(Arg.LearnArg arg)
            : base(new Impl.LearnImpl(), arg)
        { }

        protected override StateTransistion Transist()
        {
            BaseArg result = RunImpl();
            
            if (result.GetType() == typeof(Arg.WaitForBodyArg))
                return new WaitForBodyState((Arg.WaitForBodyArg)result);

            return null;
        }
    }
}
