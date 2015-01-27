using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.View
{
    interface IObserver
    {
        void Observe(Model.TrackingModel model);
    }
}
