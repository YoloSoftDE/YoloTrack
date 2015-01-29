using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cognitec.FRsdk;
using System.Drawing;

namespace YoloTrack.MVC.Model.Storage
{
    public struct IdentificationRecord
    {
        public FIR Value;
        // public Bitmap[] Sources;
    }

    public class Person
    {
        public Guid Id;
        public string Name;
        public DateTime Learned;
        public bool IsTarget;
        public IdentificationRecord IR;
        public Bitmap Picture;
        public bool IsPresent = false;

        public override bool Equals(object obj)
        {
            return ((Person)obj).Id.Equals(Id);
        }

        public static Guid fail = Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

        public RuntimeInfo RTInfo
        {
            get {
                return RTInfo;
            }
            set
            {
                IsPresent = true;
                RTInfo = value;
            }
        }
    }
}
