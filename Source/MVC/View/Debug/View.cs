using System;
using YoloTrack.MVC.Model.Configuration;

using Sensor = YoloTrack.MVC.Model.Sensor.Model;
using StateMachine = YoloTrack.MVC.Model.StateMachine.Model;
using Configuration = YoloTrack.MVC.Model.Configuration.Model;
using IdentificationData = YoloTrack.MVC.Model.IdentificationData.Model;
using RuntimeDatabase = YoloTrack.MVC.Model.RuntimeDatabase.Model;
using Database = YoloTrack.MVC.Model.Database.Model;

namespace YoloTrack.MVC.View.Debug
{
    public enum DebugLevel
    {
        Notice,
        Info,
        Warn,
        Error,
        Crit,
        Emerge
    } // ENd enum

    /// <summary>
    /// View for very simple console debugging output
    /// </summary>
    public class View : IConfigurable
    {
        /// <summary>
        /// Application configuration model instance.
        /// </summary>
        Configuration m_app_config;

        /// <summary>
        /// Method for unified logging.
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Message"></param>
        /// <param name="Level"></param>
        private void _log(string Section, string Message, DebugLevel Level = DebugLevel.Notice)
        {
            if (Level < m_app_config.Options.Logging.LogLevel)
                return;

            Console.WriteLine("[{0}] ({1}): {2}", Level, Section, Message);
        }

        /// <summary>
        /// Binders the application config
        /// </summary>
        /// <param name="Sensor"></param>
        public void Bind(Configuration Configuration)
        {
            m_app_config = Configuration;
        }

        /// <summary>
        /// Observer-binder
        /// </summary>
        /// <param name="Sensor"></param>
        public void Observe(Sensor Sensor)
        {
            // Do not subscribe yet
        }

        /// <summary>
        /// Observer-binder
        /// </summary>
        /// <param name="Sensor"></param>
        public void Observe(StateMachine StateMachine)
        {
            StateMachine.StateChange += new EventHandler<Model.StateMachine.StateChangeEventArgs>(_log);
        }

        /// <summary>
        /// Observer-binder
        /// </summary>
        /// <param name="RuntimeDatabase"></param>
        public void Observe(RuntimeDatabase RuntimeDatabase)
        {
            RuntimeDatabase.RecordAdded += new EventHandler<Model.RuntimeDatabase.RecordAddedEventArgs>(_log);
            RuntimeDatabase.RecordRemoved += new EventHandler<Model.RuntimeDatabase.RecordRemovedEventArgs>(_log);
        }

        /// <summary>
        /// Observer-binder
        /// </summary>
        /// <param name="Database"></param>
        public void Observe(Database Database)
        {
            Database.RecordAdded += new EventHandler<Model.Database.RecordAddedEventArgs>(_log);
            Database.RecordRemoved += new EventHandler<Model.Database.RecordRemovedEventArgs>(_log);
        }

        /// <summary>
        /// Observer-binder
        /// </summary>
        /// <param name="IdentificationData"></param>
        public void Observe(IdentificationData IdentificationAPI)
        {
            // Do not subscribe yet
        }

        #region Log messages

        /// <summary>
        /// Specialized log message event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _log(object sender, Model.StateMachine.StateChangeEventArgs e)
        {
            _log("State machine",
                String.Format("{0} -> {1}",
                e.PreviousState.ToString(),
                e.NextState.ToString()),
                DebugLevel.Notice);
        }

        /// <summary>
        /// Specialized log message event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _log(object sender, Model.RuntimeDatabase.RecordAddedEventArgs e)
        {
            _log("Runtime database", 
                String.Format("Added record with TrackingId {0}, State is {1}",
                e.Record.KinectResource.Skeleton.TrackingId,
                e.Record.State),
                DebugLevel.Info);

            // Add RecordChange event listener to record
            e.Record.RecordChanged += new EventHandler<Model.RuntimeDatabase.RecordChangedEventArgs>(_log);
        }

        /// <summary>
        /// Specialized log message event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _log(object sender, Model.RuntimeDatabase.RecordChangedEventArgs e)
        {
            Model.RuntimeDatabase.Record record = (Model.RuntimeDatabase.Record)sender;
            _log("Runtime database",
                String.Format("Record with TrackingId {0} changed, State is {1}",
                record.KinectResource.Skeleton.TrackingId,
                record.State),
                DebugLevel.Info);
        }

        /// <summary>
        /// Specialized log message event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _log(object sender, Model.RuntimeDatabase.RecordRemovedEventArgs e)
        {
            _log("Runtime database", 
                String.Format("Removed record with TrackingId {0}",
                e.Record.KinectResource.Skeleton.TrackingId,
                e.Record.State),
                DebugLevel.Info);

            // Remove RecordChange event listener to record
            // TODO: Fix this. Handler cannot be unsubscribed due to the new keyword
            //e.Record.RecordChanged -= new EventHandler<Model.RuntimeDatabase.RecordChangedEventArgs>(_log);
        }

        /// <summary>
        /// Specialized log message event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _log(object sender, Model.Database.RecordAddedEventArgs e)
        {
            _log("Database", 
                String.Format("Added record with Id {0}",
                e.Record.Id),
                DebugLevel.Info);

            // Add RecordChange event listener to record
            e.Record.RecordChanged += new EventHandler<Model.Database.RecordChangedEventArgs>(_log);
        }

        /// <summary>
        /// Specialized log message event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _log(object sender, Model.Database.RecordChangedEventArgs e)
        {
            Model.Database.Record record = (Model.Database.Record)sender;
            _log("Database", 
                String.Format("Record with Id {0} changed, Name = {1}, {2}",
                record.Id,
                record.LastName,
                record.FirstName),
                DebugLevel.Info);
        }

        /// <summary>
        /// Specialized log message event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _log(object sender, Model.Database.RecordRemovedEventArgs e)
        {
            _log("Database", 
                String.Format("Removed record with Id {0}, Name = {1}, {2}",
                e.Record.Id,
                e.Record.LastName,
                e.Record.FirstName),
                DebugLevel.Info);

            // Remove RecordChange event listener to record
            // TODO: Fix this. Handler cannot be unsubscribed due to the new keyword
            //e.Record.RecordChanged -= new EventHandler<Model.RuntimeDatabase.RecordChangedEventArgs>(_log);
        }

        #endregion
    } // End class
} // End namespace
