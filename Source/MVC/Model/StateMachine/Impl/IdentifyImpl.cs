using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using Cognitec.FRsdk;
using Eyes = Cognitec.FRsdk.Eyes;
using Identification = Cognitec.FRsdk.Identification;

namespace YoloTrack.MVC.Model.StateMachine.Impl
{
    class IdentifyImpl : BaseImpl<Arg.IdentifyArg>
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

        protected struct IdentifyResult
        {
            public Guid PersonId;
            public float Score;
        }

		protected IdentifyResult Identify (List<Sample> samples)
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
            IdentifyResult result = new IdentifyResult();

			if (fb.match.Length == 0) {
                result.Score = 0;
                return result;
			}
			
			winner = fb.match [0];

			for (int i = 1; i < fb.match.Length; i++) {
				Match m = fb.match [i];

				Console.WriteLine ("Match on FIR[{0}] has Score {1}", m.name, m.score.value);
				if (m.score.value > winner.score.value) {
					winner = m;
				}
			}

            result.PersonId = Guid.Parse(winner.name);
			// Storage.Person match = Model.MainDatabase.People.Find(p => p.Id == Guid.Parse(winner.name));
			return result;
		}
		
        public override void Run (Arg.IdentifyArg arg)
		{
			/* Prepare Samples for identification */
			List<Sample> identificationSamples = new List<Sample> ();

            int zaehler = 0;
			foreach (Bitmap fratze in arg.Faces.ToArray()) {
                fratze.Save("fratze-" + zaehler++ + ".bmp");
                
				//MemoryStream ms = new MemoryStream ();

				//fratze.Save (ms, System.Drawing.Imaging.ImageFormat.Bmp);

				//identificationSamples.Add (new Sample (Bmp.load (ms)));
                
                Util.CompatibleImage ci = Util.CompatibleImage.FromBitmap(fratze);
                identificationSamples.Add(new Sample(ci));
			}

            /* 0-20% -> Unknown -> Learn
             * 21%-80% -> Unidentified -> WaitForBody
             * 81%-100% -> Identified -> Track
             */
            try
            {
                IdentifyResult result = this.Identify(identificationSamples);

                if (result.Score <= 0.2)
                {
                    Result = new Arg.LearnArg()
                    {
                        SkeletonId = arg.SkeletonId,
                        Samples = identificationSamples,
                        Faces = arg.Faces
                    };
                }
                else if (result.Score <= 0.8)
                {
                    Result = new Arg.WaitForBodyArg();
                }
                else
                {
                    Result = new Arg.TrackingDecisionArg()
                    {
                        PersonId = result.PersonId
                    };
                }
            }
            catch (IdentificationException)
            {
                Result = new Arg.WaitForBodyArg();
            }
        }
    }
}
