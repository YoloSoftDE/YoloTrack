using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.Arg
{
    class TrackingDecisionArg : BaseArg
    {
        public Guid PersonId;

        public override object Clone()
        {
            return new TrackingDecisionArg()
            {
                PersonId = new Guid(PersonId.ToString())
            };
        }
    }
}
