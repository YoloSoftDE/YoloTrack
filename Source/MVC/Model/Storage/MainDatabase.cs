using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace YoloTrack.MVC.Model.Storage
{
    public class MainDatabase
    {
        public event EventHandler PersonAdded;
        public event EventHandler PersonRemoved;
        public event EventHandler PersonChanged;

        public void OnPersonAdded()
        {
            if (PersonAdded == null)
                return;

            PersonAdded(this, new EventArgs());
        }

        public void OnPersonRemoved()
        {
            if (PersonRemoved == null)
                return;

            PersonRemoved(this, new EventArgs());
        }

        public void OnPersonChanged()
        {
            if (PersonChanged == null)
                return;

            PersonChanged(this, new EventArgs());
        }

        List<Person> m_people;

        public List<Person> People
        {
            get { return m_people; }
        }

        public Person Target
        {
            get {
                return m_people.Find(p => p.IsTarget == true);
            }
            set {
                // LoL
                Person benis = m_people.Find(p => p.IsTarget == true);
                benis.IsTarget = false;
                benis = m_people.Find(p => p.Equals(value));
                benis.IsTarget = true;
                // ^^------------------ dat Code
                /*/
                 
                                ROFL ROFL LOL ROFL ROFL
                                   ________/\_______
                               L   \             \  \
                              LOL===\ [][][][][]  \__\
                               L     \                \
                                      \________________]
                                       __||_________||___/
              
               /*/
            }
        }

        public void Add(Person p)
        {
            if (m_people.Count >= ConfigModel.Instance().conf.MaxDatabaseEntries)
            {
                
                return;
            }

            m_people.Add(p);
            p.RuntimeInfo.UpdateState(TrackingState.IDENTIFIED);
            OnPersonAdded();
        }

        public void Remove(Predicate<Person> pre)
        {
            m_people.Remove(m_people.Find(pre));
            OnPersonRemoved();
        }

        public void SaveToFile(string Filename)
        {
            /*
            XmlSerializer serializer = new XmlSerializer(typeof(List<Person>));
            System.IO.FileStream fs = new System.IO.FileStream(Filename, System.IO.FileMode.OpenOrCreate);
            serializer.Serialize(fs, m_people);
            fs.Close();
            */
        }

        public void LoadFromFile(string Filename)
        {
            if (!System.IO.File.Exists(Filename))
            {
                m_people = new List<Person>();
                SaveToFile(Filename);
                return;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(List<Person>));
            System.IO.FileStream fs = new System.IO.FileStream(Filename, System.IO.FileMode.Open);
            m_people = (List<Person>)serializer.Deserialize(fs);
            fs.Close();
        }

        public void Update(Person p)
        {
            OnPersonChanged();
        }
    }
}
