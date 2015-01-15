using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect1_skeletonTracking
{
    class Program
    {
        public static void Main()
        {
            SkeletonTracking skeletonTracker = new SkeletonTracking();
            skeletonTracker.StartKinect();
            
            // Wait for User Input
            Console.Read();
        }
    }
}
