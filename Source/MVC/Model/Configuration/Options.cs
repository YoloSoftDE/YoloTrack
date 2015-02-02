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
        /// <summary>
        /// Database subtree
        /// </summary>
        public Option.Database Database { get; private set; }

        /// <summary>
        /// FceVACS subtree
        /// </summary>
        public Option.IdentificationData IdentificationData { get; private set; }

        /// <summary>
        /// Logging
        /// </summary>
        public Option.Logging Logging { get; private set; }
    } // End struct
} // End namespace
