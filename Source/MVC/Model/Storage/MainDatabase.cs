using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.Model.Storage
{
    public class MainDatabase
    {
        private List<Person> m_people;

        public List<Person> People
        {
            get { return m_people; }
        }
    }
}
