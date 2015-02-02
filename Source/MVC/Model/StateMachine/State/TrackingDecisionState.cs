using System;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    /// <summary>
    /// State transistion logic for 'TrackingDecision'
    /// </summary>
    class TrackingDecisionState : BaseState<Impl.TrackingDecisionImpl, Arg.TrackingDecisionArg>
    {
        /// <summary>
        /// Generalized construcor. No way to omit the arguments paremeter here.
        /// </summary>
        /// <param name="arg"></param>
        public TrackingDecisionState(Arg.TrackingDecisionArg arg)
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
            
            if (result.GetType() == typeof(Arg.WaitForBodyArg))
                return new WaitForBodyState((Arg.WaitForBodyArg)result);

            else if (result.GetType() == typeof(Arg.TrackingArg))
                return new TrackingState((Arg.TrackingArg)result);

            return null;
        }
    } // End clss
} // End namespace
