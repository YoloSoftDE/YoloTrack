using System.Drawing;
using Microsoft.Kinect;

namespace YoloTrack.MVC.Model.Storage
{
    public enum TrackingState
    {
        UNIDENTIFIED, IDENTIFIED, UNKNOWN, TRACKED, LOST
    }

    public struct RuntimeInfo
    {
        public Skeleton Skeleton;
        public Person Person;
        public TrackingState State;

        public RuntimeInfo(Skeleton skeleton)
        {
            Skeleton = skeleton;
            Person = null;
            State = TrackingState.UNIDENTIFIED;
        }

        public RuntimeInfo(RuntimeInfo original)
        {
            Skeleton = original.Skeleton;
            Person = original.Person;
            State = original.State;
        }

        public Rectangle HeadRect
        {
            get
            {
                Joint head_joint = Skeleton.Joints[JointType.Head];
                if (head_joint.TrackingState == JointTrackingState.NotTracked)
                    return new Rectangle(0, 0, 0, 0);

                KinectSensor sensor = Model.TrackingModel.Instance().Kinect;
                ColorImagePoint pt_center;
                try
                {
                    pt_center = sensor.MapSkeletonPointToColor(head_joint.Position, Model.TrackingModel.Instance().ColorStreamFormat);
                    int radius;

                    // Button (ShoulderCenter) available?
                    if (Skeleton.Joints[JointType.ShoulderCenter].TrackingState == JointTrackingState.Tracked)
                    {
                        // Calculation by 2-Tracked-Points (more accurate)
                        ColorImagePoint pt_bottom = sensor.MapSkeletonPointToColor(Skeleton.Joints[JointType.ShoulderCenter].Position, Model.TrackingModel.Instance().ColorStreamFormat);
                        radius = System.Math.Abs(pt_bottom.Y - pt_center.Y);
                    }
                    else
                    {
                        // Calculation by distance, if buttom point unavailable (fallback)
                        radius = (int)System.Math.Round(180.0 / head_joint.Position.Z);
                    }

                    return new Rectangle(new Point(pt_center.X - radius, pt_center.Y - radius), new Size(2 * radius, 2 * radius));
                }
                catch (System.InvalidCastException)
                {
                    System.Console.WriteLine("[RuntimeInfo] Invalid cast!");
                }

                return new Rectangle(0, 0, 0, 0);
            }
        }

        public void UpdateState(TrackingState next)
        {
            State = next;
        }

        public void UpdateSkeleton(Microsoft.Kinect.Skeleton skeleton)
        {
            Skeleton = skeleton;
        }

        public void Watch()
        {
            KinectSensor sensor = Model.TrackingModel.Instance().Kinect;
            sensor.SkeletonStream.ChooseSkeletons(Skeleton.TrackingId);
            
            return;
        } // End Watch()
    }
}
