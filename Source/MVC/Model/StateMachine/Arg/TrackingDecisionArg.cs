using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.Arg
{
    struct TrackingDecisionArg : BaseArg
    {
        public int PersonId { get; set; }
    }
}
