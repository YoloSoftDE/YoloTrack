using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace YoloTrack.MVC.Model
{
    public struct Config
    {
        public string DatabaseFilename;
        public int MaxDatabaseEntries;
    }

    class ConfigModel
    {
        const string Filename = "YoloTrack.conf";
        public Config conf;
        static private ConfigModel m_inst = null;

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            System.IO.FileStream fs = new System.IO.FileStream(Filename, System.IO.FileMode.OpenOrCreate);
            serializer.Serialize(fs, conf);
            fs.Close();
        }

        public void Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            if (!System.IO.File.Exists(Filename))
            {
                CreateInitConfig();
                Save();
            }
            System.IO.FileStream fs = new System.IO.FileStream(Filename, System.IO.FileMode.Open);
            conf = (Config)serializer.Deserialize(fs);
            fs.Close();
        }

        private void CreateInitConfig()
        {
            conf = new Config()
            {
                DatabaseFilename = "YoloTrack.db.bin"
            };
        }

        static public ConfigModel Instance()
        {
            if (m_inst == null)
            {
                m_inst = new ConfigModel();
                m_inst.Load();
            }

            return m_inst;

            
        }
    }
}
