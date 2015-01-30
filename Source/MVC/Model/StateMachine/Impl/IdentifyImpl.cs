using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using Cognitec.FRsdk;
using Eyes = Cognitec.FRsdk.Eyes;
using Identification = Cognitec.FRsdk.Identification;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class IdentifyImpl : BaseImpl<Arg.TrackingDecisionArg, Arg.IdentifyArg>
    {
		protected float threshold = 1;

        private class IdentificationException : System.Exception
        {
            public IdentificationException(string message)
                : base(message)
            {
            }
        }
		
        private class IdentificationFeedback : Identification.Feedback
        {
            public Match[] match = null;
            public float sample_quality;

            public void eyesNotFound()
            {
                throw new IdentificationException("eyesNotFound");
            }

            public void sampleQualityTooLow()
            {
                throw new IdentificationException("sampleQualityTooLow");
            }

            public void failure()
            {
                throw new IdentificationException("failure");
            }

            public void sampleQuality(float f)
            {
                this.sample_quality = f;
            }

            public void matches(Match[] matches)
            {
                this.match = matches;
            }


            public void end()
            {
                
            }

            public void eyesFound(Eyes.Location eyeLoc)
            {
                
            }

            public void processingImage(Cognitec.FRsdk.Image img)
            {
                
            }

            public void start()
            {
                
            }
        }

		protected Guid Identify (List<Sample> samples)
		{
            

			/* Run Identification */
			IdentificationFeedback fb = new IdentificationFeedback ();

			Model.IdentificationProcessor.process (
                samples.ToArray (),
                Model.FARScore,
                fb,
                3
			);

			/* Find the highest matching person */
			Match winner;
			Guid guid;

			if (fb.match.Length == 0) {
				return Storage.Person.fail;	
			}
			
			winner = fb.match [0];

			for (int i = 1; i < fb.match.Length; i++) {
				Match m = fb.match [i];

				Console.WriteLine ("Match on FIR[{0}] has Score {1}", m.name, m.score.value);
				if (m.score.value > winner.score.value) {
					winner = m;
				}
			}
			
			if (winner.score.value > this.threshold) {
				return Storage.Person.fail;
			}
			
			// Storage.Person match = Model.MainDatabase.People.Find(p => p.Id == Guid.Parse(winner.name));
			return Guid.Parse (winner.name);
		}
		
        public override void Run (Arg.IdentifyArg arg)
		{
			/* Prepare Samples for identification */
			List<Sample> identificationSamples = new List<Sample> ();

            int zaehler = 0;
			foreach (Bitmap fratze in arg.Faces) {
                fratze.Save("fratze-" + zaehler++ + ".bmp");
                
				//MemoryStream ms = new MemoryStream ();

				//fratze.Save (ms, System.Drawing.Imaging.ImageFormat.Bmp);

				//identificationSamples.Add (new Sample (Bmp.load (ms)));
                
                Util.CompatibleImage ci = Util.CompatibleImage.FromBitmap(fratze);
                identificationSamples.Add(new Sample(ci));
			}

            try
            {
                Result = new Arg.TrackingDecisionArg()
                {
                    PersonId = this.Identify(identificationSamples)
                };
            }
            catch (IdentificationException)
            {
                Result = new Arg.TrackingDecisionArg()
                {
                    PersonId = Storage.Person.fail
                };
            }			
        }
    }
}
