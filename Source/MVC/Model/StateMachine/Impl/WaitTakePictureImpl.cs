using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class WaitTakePictureImpl : BaseImpl<Arg.IdentifyArg, Arg.WaitTakePictureArg>
    {
        public override void Run(Arg.WaitTakePictureArg arg)
        {
            //Thread.Sleep(1000);
        }
    }
}
