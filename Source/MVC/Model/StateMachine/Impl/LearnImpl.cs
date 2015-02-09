using System;
using YoloTrack.MVC.Model.IdentificationData;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    /// <summary>
    /// State implementation for 'Learn'
    /// </summary>
    class LearnImpl : BaseImpl<Arg.LearnArg>
    {		
        /// <summary>
        /// Implementation of the execution step.
        /// </summary>
        /// <param name="arg"></param>
        public override void Run (Arg.LearnArg arg)
		{
            try
            {
                EnrollmentFeedback feedback = m_identification_api.Enroll(arg.Samples);

                Database.Record record = m_database.CreateRecord(
                    new Database.IdentificationRecord(feedback.IdentificationRecord));
                record.Image = arg.Faces[0];
                m_runtime_database[arg.TrackingId].State = RuntimeDatabase.RecordState.Identified;
                m_runtime_database[arg.TrackingId].Attach(record);

                m_identification_api.Population.append(
                    record.IdentificationRecord.Value, 
                    record.Id.ToString());
			
            }
            catch (EnrollmentException)
            {
                Console.WriteLine("[Learn] Learn failed");
            }
            finally
            {
                Result = new Arg.WaitForBodyArg();
            }
        }
    } // End class
} // End namespace
