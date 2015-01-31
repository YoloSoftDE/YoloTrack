using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine
{
    class BaseArg : ICloneable
    {
        public virtual object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
