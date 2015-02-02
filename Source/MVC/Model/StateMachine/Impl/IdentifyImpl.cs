using System;
using System.Drawing;
using System.Collections.Generic;
using Cognitec.FRsdk;
using Eyes = Cognitec.FRsdk.Eyes;
using YoloTrack.MVC.Model.IdentificationData;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    /// <summary>
    /// State implementation for 'Identify'
    /// </summary>
    class IdentifyImpl : BaseImpl<Arg.IdentifyArg>
    {
        /// <summary>
        /// Result of the identification procedure
        /// </summary>
        protected struct IdentifyResult
        {
            public int DatabaseRecordId;
            public float Score;
        }

        /// <summary>
        /// Actual identification procedure
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
		protected IdentifyResult Identify (List<Sample> samples)
		{
			/* Run Identification */
            IdentificationFeedback fb = m_identification_api.Identify(samples, 3);

			/* Find the highest matching person */
			Match winner;
            IdentifyResult result = new IdentifyResult();

			if (fb.Match.Length == 0) {
                result.Score = 0;
                return result;
			}

            winner = fb.Match[0];
            Console.WriteLine("Match on FIR[{0}] has Score {1}", winner.name, winner.score.value);

            for (int i = 1; i < fb.Match.Length; i++)
            {
                Match m = fb.Match[i];

				Console.WriteLine ("Match on FIR[{0}] has Score {1}", m.name, m.score.value);
				if (m.score.value > winner.score.value) {
					winner = m;
				}
			}

            result.DatabaseRecordId = Int16.Parse(winner.name);
            result.Score = winner.score.value;
			// Storage.Person match = Model.MainDatabase.People.Find(p => p.Id == Guid.Parse(winner.name));
			return result;
		}
		
        /// <summary>
        /// Execution step.
        /// </summary>
        /// <param name="arg"></param>
        public override void Run (Arg.IdentifyArg arg)
		{
			/* Prepare Samples for identification */
			List<Sample> identificationSamples = new List<Sample> ();

			foreach (Bitmap fratze in arg.Faces) {
                Util.CompatibleImage ci = Util.CompatibleImage.FromBitmap(fratze);
                identificationSamples.Add(new Sample(ci));
			}

            try
            {
                IdentifyResult result = this.Identify(identificationSamples);

                // 0% - 20% -> Unknown -> Learn
                if (result.Score <= 0.2)
                {
                    RuntimeDatabase.Record record = m_runtime_database[arg.TrackingId];
                    record.State = RuntimeDatabase.RecordState.Unknown;
                    m_runtime_database[arg.TrackingId] = record;

                    Result = new Arg.LearnArg()
                    {
                        TrackingId = arg.TrackingId,
                        Samples = identificationSamples,
                        Faces = arg.Faces
                    };
                }
                // >20% - 50% -> Unidentified -> WaitForBody
                else if (result.Score <= 0.5)
                {
                    RuntimeDatabase.Record record = m_runtime_database[arg.TrackingId];
                    record.State = RuntimeDatabase.RecordState.Unidentified;
                    m_runtime_database[arg.TrackingId] = record;

                    Result = new Arg.WaitForBodyArg();
                }
                // >50% - 100% -> Identified -> Track
                else
                {
                    RuntimeDatabase.Record record = m_runtime_database[arg.TrackingId];
                    record.State = RuntimeDatabase.RecordState.Identified;
                    m_runtime_database[arg.TrackingId] = record;

                    Database.Record p = m_database[result.DatabaseRecordId];
                    p.RuntimeRecord = record;
                    p.IncrementTimesRecognized();

                    Result = new Arg.TrackingDecisionArg()
                    {
                        DatabaseRecordId = result.DatabaseRecordId
                    };
                }
            } // End try
            catch (IdentificationException)
            {
                Result = new Arg.WaitForBodyArg();
            }

            return;
        } // End Run()
        
    } // End Class IdentifyImpl
} // End Namespace
