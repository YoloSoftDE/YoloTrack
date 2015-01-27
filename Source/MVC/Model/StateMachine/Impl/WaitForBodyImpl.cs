using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class WaitForBodyImpl : BaseImpl<Arg.WaitTakePictureArg, Arg.WaitForBodyArg>
    {
        public override void Run(Arg.WaitForBodyArg arg)
        {
            //Thread.Sleep(1000);

            //Storage.RuntimeInfo new_info = model.RuntimeDatabase.At(1);
            // new_info 
            //model.RuntimeDatabase.Update(1, new_info);
        }
    }
}
