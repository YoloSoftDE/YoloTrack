using System;

namespace YoloTrack.MVC.Model.Configuration
{
    /// <summary>
    /// Defines the interface for configurable items. These must accept an instance of the configuration model.
    /// </summary>
    interface IConfigurable
    {
        /// <summary>
        /// Binder for the configuration model.
        /// </summary>
        /// <param name="Configuration"></param>
        void Bind(Configuration.Model Configuration);
    } // End interface
} // End namespace
