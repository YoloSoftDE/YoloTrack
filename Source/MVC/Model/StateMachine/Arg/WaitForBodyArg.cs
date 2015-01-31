using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.Arg
{
    class WaitForBodyArg : BaseArg
    {
        public override object Clone()
        {
            return new WaitForBodyArg();
        }
    }
}
