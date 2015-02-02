using System;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    /// <summary>
    /// State transistion logic for 'Tracking'
    /// </summary>
    class TrackingState : BaseState<Impl.TrackingImpl, Arg.TrackingArg>
    {
        /// <summary>
        /// Generalized construcor. No way to omit the arguments paremeter here.
        /// </summary>
        /// <param name="arg"></param>
        public TrackingState(Arg.TrackingArg arg)
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
            
            if (result.GetType() == typeof(Arg.SwitchTargetArg))
                return new SwitchTargetState((Arg.SwitchTargetArg)result);

            else if (result.GetType() == typeof(Arg.WaitForBodyArg))
                return new WaitForBodyState((Arg.WaitForBodyArg)result);

            return null;
        }
    } // End class
} // End namespace
