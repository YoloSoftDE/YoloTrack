using System.Collections.Generic;
using System.Collections;

namespace YoloTrack.MVC.Model.Storage
{
    public delegate void RuntimeInfoChangeHandler(Storage.RuntimeInfo info);

    public class RuntimeDatabase : Dictionary<int, Storage.RuntimeInfo>
    {
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

        public void Insert(int SkeletonId)
        {
            base[SkeletonId] = new RuntimeInfo()
            {
                SkeletonId = SkeletonId,
                State = TrackingState.UNIDENTIFIED
            };
        }
    }
}
