using System;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    /// <summary>
    /// State transistion logic for 'Identify'
    /// </summary>
    class IdentifyState : BaseState<Impl.IdentifyImpl, Arg.IdentifyArg>
    {
        /// <summary>
        /// Generalized construcor. No way to omit the arguments paremeter here.
        /// </summary>
        /// <param name="arg"></param>
        public IdentifyState(Arg.IdentifyArg arg)
            : base(arg)
        {
        }

        /// <summary>
        /// Transistion logic.
        /// </summary>
        /// <returns></returns>
        protected override StateTransistion Transist()
        {
            BaseArg result = RunImpl();

            if (result.GetType() == typeof(Arg.TrackingDecisionArg))
                return new TrackingDecisionState((Arg.TrackingDecisionArg)result);

            else if (result.GetType() == typeof(Arg.LearnArg))
                return new LearnState((Arg.LearnArg)result);

            else if (result.GetType() == typeof(Arg.WaitForBodyArg))
                return new WaitForBodyState((Arg.WaitForBodyArg)result);

            return null;
        }
    } // End class
} // End namespace
