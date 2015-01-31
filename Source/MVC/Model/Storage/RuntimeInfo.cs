using System.Drawing;

namespace YoloTrack.MVC.Model.Storage
{
    public enum TrackingState
    {
        UNIDENTIFIED, IDENTIFIED, UNKNOWN, TRACKED, LOST
    }

    public struct RuntimeInfo
    {
        public int SkeletonId;
        public Rectangle HeadRect;
        public Person Person;
        public TrackingState State;

        public RuntimeInfo(RuntimeInfo original)
        {
            SkeletonId = original.SkeletonId;
            HeadRect = new Rectangle(original.HeadRect.X, original.HeadRect.Y, original.HeadRect.Width, original.HeadRect.Height);
            Person = original.Person; // Shoudl always be a reference, not a copy!
            State = original.State; // TODO: check if true copy or reference
        }

        public void UpdateState(TrackingState next)
        {
            State = next;
        }
    }
}
