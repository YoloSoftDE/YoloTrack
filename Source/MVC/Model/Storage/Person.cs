using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cognitec.FRsdk;
using System.Drawing;

namespace YoloTrack.MVC.Model.Storage
{
    public struct Person
    {
        public string Name { get; set; }
        public FIR IdentificationRecord { get; set; }
        public DateTime Learned { get; set; }
        public bool IsTarget { get; set; }
        public Bitmap Picture { get; set; }

        public override bool Equals(object obj)
        {
            return ((Person)obj).Name.Equals(Name);
        }
    }
}
