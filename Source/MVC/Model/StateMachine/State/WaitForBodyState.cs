using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    class WaitForBodyState : BaseState<Arg.WaitTakePictureArg, Arg.WaitForBodyArg>
    {
        public WaitForBodyState(Arg.WaitForBodyArg arg)
            : base(new Impl.WaitForBodyImpl(), arg)
        { }

        public override IState Transist()
        {
            Arg.WaitTakePictureArg result = RunImpl();

            return new WaitTakePictureState(result);
        }

        public override States State
        {
            get { return States.WAIT_FOR_BODY; }
        }
    }
}
