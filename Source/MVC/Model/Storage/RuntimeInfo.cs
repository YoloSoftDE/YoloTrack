using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.Storage
{
    public struct RuntimeInfo
    {
        public int SkeletonId { get; set; }
        public int RecognizedCount { get; set; }
        public int TrackedCount { get; set; }
        public Person Person { get; set; }
    }
}
