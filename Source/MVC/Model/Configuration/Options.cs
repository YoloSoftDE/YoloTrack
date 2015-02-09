using System;

namespace YoloTrack.MVC.Model.Configuration
{
    namespace Option
    {
        /// <summary>
        /// Database subtree
        /// </summary>
        public struct Database
        {
            public string FileName;
            public uint RecordLimit;
        } // End struct

        /// <summary>
        /// IndentificationData subtree
        /// </summary>
        public struct IdentificationData
        {
            public string ConfigurationFileName;
            public float LearnThreshold;
            public float IdentifyThreshold;
            public uint SampleCount;
            public uint IdentificationTimeout;
        }

        /// <summary>
        /// Subtree containing logging options
        /// </summary>
        public struct Logging
        {
            /// <summary>
            /// Default value of the debug log level.
            /// This modifies which messages will be shown and which will be ommited
            /// </summary>
            public View.Debug.DebugLevel LogLevel;
        }
    } // End namespace

    /// <summary>
    /// Configuration root
    /// </summary>
    public struct Options
    {

        public Options(Option.Database db, Option.IdentificationData idd, Option.Logging log)
        {
            /* This must be first, because C# sucks */
            Database = db;
            Logging = log;
            IdentificationData = idd;
        }

        /// <summary>
        /// Database subtree
        /// </summary>
        public Option.Database Database;

        /// <summary>
        /// FaceVACS subtree
        /// </summary>
        public Option.IdentificationData IdentificationData;

        /// <summary>
        /// Logging
        /// </summary>
        public Option.Logging Logging;
    } // End struct
} // End namespace
