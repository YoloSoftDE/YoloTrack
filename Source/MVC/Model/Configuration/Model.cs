using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace YoloTrack.MVC.Model.Configuration
{

    /// <summary>
    /// Simple configuration provider.
    /// </summary>
    public class Model
    {

        #region Imports
        [DllImport("KERNEL32.DLL", EntryPoint = "GetPrivateProfileStringW",
            SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            string lpReturnString,
            int nSize,
            string lpFilename);

        [DllImport("KERNEL32.DLL", EntryPoint = "WritePrivateProfileStringW",
            SetLastError = true,
            CharSet = CharSet.Unicode, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int WritePrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpString,
            string lpFilename);
        #endregion

        protected static string GetString(string file, string section, string key, string default_value)
        {
            string tmp = new string(' ', 100);

            GetPrivateProfileString(section, key, default_value, tmp, tmp.Length, file);
            tmp = tmp.TrimEnd('\0', ' ');

            return tmp;
        }
        protected static string GetString(string file, string section, string key)
        {
            return GetString(file, section, key, "");
        }

        protected static void WriteString(string file, string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, file);
        }

        /// <summary>
        /// Default filename to use
        /// </summary>
        public static string DefaultFileName = Environment.CurrentDirectory + @"\YoloTrack.conf";

        /// <summary>
        /// Option holder.
        /// </summary>
        public Options Options { get; protected set; }

        /// <summary>
        /// Currently loaded configuration file.
        /// </summary>
        public string FileName { get; protected set; }

        /// <summary>
        /// Private constructor to force factory usage
        /// </summary>
        private Model()
        {
        }

        /// <summary>
        /// Factory method to create a configuration instance from file.
        /// </summary>
        /// <param name="FileName">MUST BE ABSOLUTE PATH!</param>
        /// <returns></returns>
        public static Model LoadFrom(string FileName)
        {
            Model model = new Model();

            Option.Database db = new Option.Database();

            db.FileName = GetString(FileName, "Database", "FileName", "Database.ydb");
            db.RecordLimit = uint.Parse(GetString(FileName, "Database", "RecordLimit", "100"));


            Option.IdentificationData idd = new Option.IdentificationData();

            idd.ConfigurationFileName = GetString(FileName, "Identification", "ConfigFileName", "frsdk.cfg");
            idd.IdentifyThreshold = float.Parse(GetString(FileName, "Identification", "IdentifyThreshold", "0.4"), CultureInfo.InvariantCulture);
            idd.LearnThreshold = float.Parse(GetString(FileName, "Identification", "LearnThreshold", "0.2"), CultureInfo.InvariantCulture);
            idd.SampleCount = uint.Parse(GetString(FileName, "Identification", "SampleCount", "5"));
            idd.IdentificationTimeout = uint.Parse(GetString(FileName, "Identification", "IdentificationTimeout", "3000"));

            Option.Logging log = new Option.Logging();

            string loglevel = GetString(FileName, "Logging", "Level", "WARN");

            if (loglevel.ToUpper() == "CRIT")
                log.LogLevel = View.Debug.DebugLevel.Crit;
            else if (loglevel.ToUpper() == "EMERGE")
                log.LogLevel = View.Debug.DebugLevel.Emerge;
            else if (loglevel.ToUpper() == "ERROR")
                log.LogLevel = View.Debug.DebugLevel.Error;
            else if (loglevel.ToUpper() == "INFO")
                log.LogLevel = View.Debug.DebugLevel.Info;
            else if (loglevel.ToUpper() == "WARN")
                log.LogLevel = View.Debug.DebugLevel.Warn;
            else if (loglevel.ToUpper() == "NOTICE")
                log.LogLevel = View.Debug.DebugLevel.Notice;
            else 
                log.LogLevel = View.Debug.DebugLevel.Warn;


            Options opt = new Options(db, idd, log);

            model.Options = opt;

            model.SaveTo(DefaultFileName);

            /* TODO: Is this working */
            return model;
        }

        /// <summary>
        /// Same as LoadFrom, but loades from the default filename instead.
        /// </summary>
        /// <returns></returns>
        public static Model LoadDefault()
        {
            return LoadFrom(DefaultFileName);
        }

        /// <summary>
        /// Saves the current configuration to the given file name.
        /// </summary>
        /// <param name="FileName"></param>
        public void SaveTo(string FileName)
        {
            WriteString(FileName, "Database", "FileName", this.Options.Database.FileName);
            WriteString(FileName, "Database", "RecordLimit", this.Options.Database.RecordLimit.ToString());


            WriteString(FileName, "Identification", "ConfigFileName", this.Options.IdentificationData.ConfigurationFileName);
            WriteString(FileName, "Identification", "IdentifyThreshold", this.Options.IdentificationData.IdentifyThreshold.ToString(CultureInfo.InvariantCulture));
            WriteString(FileName, "Identification", "LearnThreshold", this.Options.IdentificationData.LearnThreshold.ToString(CultureInfo.InvariantCulture));
            WriteString(FileName, "Identification", "SampleCount", this.Options.IdentificationData.SampleCount.ToString());
            WriteString(FileName, "Identification", "IdentificationTimeout", this.Options.IdentificationData.IdentificationTimeout.ToString());

            WriteString(FileName, "Logging", "Level", this.Options.Logging.LogLevel.ToString());
        }

        /// <summary>
        /// Saves the configration under the same filename as it was loaded from.
        /// </summary>
        public void Save()
        {
            SaveTo(FileName);
        }
    } // End class
} // End namespace
