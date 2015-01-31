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
                CoordinateMapper mapper = new CoordinateMapper(sensor);
                ColorImagePoint pt = mapper.MapSkeletonPointToColorPoint(head_joint.Position, ColorImageFormat.RgbResolution1280x960Fps12);
                
                // TODO: adopt to be calculated by the actual bone
                return new Rectangle(new Point(pt.X - 100, pt.Y - 100), new Size(200, 200));
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
