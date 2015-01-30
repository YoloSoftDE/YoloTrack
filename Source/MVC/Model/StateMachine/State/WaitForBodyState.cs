using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    class WaitForBodyState : BaseState<Arg.WaitForBodyArg>
    {
        public WaitForBodyState(Arg.WaitForBodyArg arg)
            : base(new Impl.WaitForBodyImpl(), arg)
        { }

        protected override StateTransistion Transist()
        {
            BaseArg result = RunImpl();

            if (result.GetType() == typeof(Arg.WaitTakePictureArg))
                return new WaitTakePictureState((Arg.WaitTakePictureArg)result);

            return null;
        }
    }
}
