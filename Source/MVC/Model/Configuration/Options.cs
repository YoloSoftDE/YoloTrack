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
            this.Logging = log;

            this.Database = db;
            this.IdentificationData = idd;
        }

        /// <summary>
        /// Database subtree
        /// </summary>
        public Option.Database Database { get; protected set; }

        /// <summary>
        /// FaceVACS subtree
        /// </summary>
        public Option.IdentificationData IdentificationData { get; protected set; }

        /// <summary>
        /// Logging
        /// </summary>
        public Option.Logging Logging;
    } // End struct
} // End namespace
