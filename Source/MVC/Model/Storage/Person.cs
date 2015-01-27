using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cognitec.FRsdk;

namespace YoloTrack.MVC.Model.Storage
{
    public class Person
    {
        public string Name { get; set; }
        public FIR IdentificationRecord { get; set; }
    }
}
