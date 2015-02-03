using System;
using System.Collections.Generic;

namespace YoloTrack.MVC.Model.Configuration
{
    /// <summary>
    /// Simple configuration provider.
    /// </summary>
    public class Model
    {
        /// <summary>
        /// Default filename to use
        /// </summary>
        public static string DefaultFileName = "YoloTrack.conf";

        /// <summary>
        /// Option holder.
        /// </summary>
        public Options Options;

        /// <summary>
        /// Currently loaded configuration file.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Private constructor to force factory usage
        /// </summary>
        private Model()
        {
        }

        /// <summary>
        /// Factory method to create a configuration instance from file.
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static Model LoadFrom(string FileName)
        {
            Model model = new Model();
            // TODO
            return model;
        }

        /// <summary>
        /// Save as LoadFrom, but loades from the default filename instead.
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
