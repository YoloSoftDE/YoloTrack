using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    public enum States
    {
        WAIT_FOR_BODY,
        WAIT_TAKE_PICTURE,
        IDENTIFY,
        TRACKING_DECISION,
        TRACK,
        LEARN
    }
}
