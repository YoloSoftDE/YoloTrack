using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    class WaitTakePictureState : BaseState<Arg.IdentifyArg, Arg.WaitTakePictureArg>
    {
        public WaitTakePictureState(Arg.WaitTakePictureArg arg)
            : base(new Impl.WaitTakePictureImpl(), arg)
        { }

        public override IState Transist()
        {
            Arg.IdentifyArg result = RunImpl();

            if (result.SkeletonId >= 0)
            {
                return new IdentifyState(result);
            }

            return new WaitForBodyState(new Arg.WaitForBodyArg());
        }

        public override States State
        {
            get { return States.WAIT_TAKE_PICTURE; }
        }
    }
}
