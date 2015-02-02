using System;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    /// <summary>
    /// State transistion logic for 'SwitchTarget'
    /// </summary>
    class SwitchTargetState : BaseState<Impl.SwitchTargetImpl, Arg.SwitchTargetArg>
    {
        /// <summary>
        /// Default constructor. This works. There are reasons, read on below.
        /// </summary>
        /// <param name="arg"></param>
        public SwitchTargetState()
            : base()
        {
        }

        /// <summary>
        /// More generalized construcor taking an explicit instance of the argument.
        /// You might run better with the default constructor above, as the SwitchTargetArg
        /// structure is default-construtable (not containing anything).
        /// </summary>
        /// <param name="arg"></param>
        public SwitchTargetState(Arg.SwitchTargetArg arg)
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
    } // End class
} // End namespace
