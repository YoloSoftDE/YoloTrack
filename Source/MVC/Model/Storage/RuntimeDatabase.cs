using System.Collections.Generic;
using System.Collections;
using System.Threading;
using Microsoft.Kinect;
using System;

namespace YoloTrack.MVC.Model.Storage
{
    public delegate void RuntimeInfoChangeHandler(Storage.RuntimeInfo info);

    public class RuntimeDatabase : Dictionary<int, Storage.RuntimeInfo>
    {
        ManualResetEvent m_refresh_lock = new ManualResetEvent(false);

        public event RuntimeInfoChangeHandler OnRuntimeInfoChange;

        public new RuntimeInfo this[int key]
        {
            get {
                return base[key];
            }
            set {
                base[key] = value;
                OnRuntimeInfoChange(value);
            }
        }

        public void Insert(Skeleton skeleton)
        {
            base[skeleton.TrackingId] = new RuntimeInfo(skeleton)
            {
                State = TrackingState.UNIDENTIFIED
            };
        }

        public void Refresh()
        {
            SkeletonFrame frame = Model.TrackingModel.Instance().Kinect.SkeletonStream.OpenNextFrame(1000);
            if (frame == null)
                return;

            Skeleton[] skeletons = new Skeleton[frame.SkeletonArrayLength];
            frame.CopySkeletonDataTo(skeletons);

            // Add newly appered Persons (?) to RuntimeDB
            // Update existing skeletons
            Skeleton skeleton;
            for (int i = 0; i < skeletons.Length; i++)
            {
                skeleton = skeletons[i];

                if (skeleton == null || skeleton.TrackingId == 0)
                    continue;

                if (!ContainsKey(skeleton.TrackingId))
                {
                    Insert(skeleton);
                }
                else
                {
                    this[skeleton.TrackingId].UpdateSkeleton(skeleton);
                }
            }

            // Remove obsolete RuntimeInfos from RuntimeDB
            List<int> ids = new List<int>();
            foreach (KeyValuePair<int, RuntimeInfo> entry in this)
                ids.Add(entry.Key);

            foreach (int id in ids)
            {
                bool present = false;
                foreach (Skeleton skel in skeletons)
                {
                    if (skel.TrackingId == id)
                        present = true;
                }
                if (!present)
                    Remove(id);
            }

            return;
        }
    }
}
