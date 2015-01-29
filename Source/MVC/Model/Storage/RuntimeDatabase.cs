﻿using System.Collections.Generic;
using System.Collections;

namespace YoloTrack.MVC.Model.Storage
{
    public delegate void RuntimeInfoChangeHandler(Storage.RuntimeInfo info);

    public class RuntimeDatabase
    {
        public event RuntimeInfoChangeHandler OnRuntimeInfoChange;

        private List<Storage.RuntimeInfo> m_data;

        public void Update(int i, Storage.RuntimeInfo data)
        {
            // ...
            OnRuntimeInfoChange(data);
        }

        public Storage.RuntimeInfo At(int i)
        {
            return m_data[i];
        }

        public List<Storage.RuntimeInfo> List()
        {
            return m_data;
        }

        public List<Storage.RuntimeInfo> Info
        {
            get { return m_data; }
        }

        public void Add(RuntimeInfo item)
        {
            item.State = TrackingState.UNIDENTIFIED;
            m_data.Add(item);
        }
    }
}
