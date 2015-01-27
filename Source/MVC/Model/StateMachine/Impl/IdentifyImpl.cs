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

        private class IdentificationFeedback : Identification.Feedback
        {
            public Match[] match = null;
            public float sample_quality;

            public void eyesNotFound()
            {
                throw new System.Exception("Eyes not found");
            }

            public void sampleQualityTooLow()
            {
                throw new System.Exception("Quality to low");
            }

            public void failure()
            {
                throw new System.Exception("Error: Out of magic dust");
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

        public override void Run(Arg.IdentifyArg arg)
        {
            /* Prepare Samples for identification */
            List<Sample> identificationSamples = new List<Sample>();

            foreach (Bitmap fratze in arg.Faces)
            {
                MemoryStream ms = new MemoryStream();

                fratze.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                identificationSamples.Add(new Sample(Bmp.load(ms)));
            }

            /* Needs own because shit */
            IdentificationFeedback fb = new IdentificationFeedback();

            Model.IdentificationProcessor.process(
                identificationSamples.ToArray(),
                Model.FARScore,
                fb,
                3
            );

            Match winner;

            foreach (Match m in fb.match)
            {
                /* ka welche werte hier rauskommen */

                Console.WriteLine("Match on FIR[{0}] has Score {1}", m.name, m.score);


                /*
                if (mwinner == null) {
                    mwinner = m;
                } else if (m.score > mwinner.score) {
                    mwinner = m;
                }
                */
            }

            /* HowTo weitergeben ? */
            // result = mwinner.name;


            //Thread.Sleep(1000);
        }
    }
}
