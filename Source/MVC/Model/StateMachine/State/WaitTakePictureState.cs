using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    class WaitTakePictureState : BaseState<Arg.WaitTakePictureArg>
    {
        public WaitTakePictureState(Arg.WaitTakePictureArg arg)
            : base(new Impl.WaitTakePictureImpl(), arg)
        { }

        protected override StateTransistion Transist()
        {
            BaseArg result = RunImpl();
            
            if (result.GetType() == typeof(Arg.IdentifyArg))
                return new IdentifyState((Arg.IdentifyArg)result);

            else if (result.GetType() == typeof(Arg.WaitForBodyArg))
                return new WaitForBodyState((Arg.WaitForBodyArg)result);

            return null;
        }
    }
}
