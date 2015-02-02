using System;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    /// <summary>
    /// State transistion logic for 'WaitForBody'
    /// </summary>
    class WaitForBodyState : BaseState<Impl.WaitForBodyImpl, Arg.WaitForBodyArg>
    {
        /// <summary>
        /// Default constructor. This works. There are reasons, read on below.
        /// </summary>
        /// <param name="arg"></param>
        public WaitForBodyState()
            : base()
        {
        }

        /// <summary>
        /// More generalized construcor taking an explicit instance of the argument.
        /// You might run better with the default constructor above, as the WaitForBodyArg
        /// structure is default-construtable (not containing anything).
        /// </summary>
        /// <param name="arg"></param>
        public WaitForBodyState(Arg.WaitForBodyArg arg)
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

            if (result.GetType() == typeof(Arg.WaitTakePictureArg))
                return new WaitTakePictureState((Arg.WaitTakePictureArg)result);

            return null;
        }
    } // End class
} // End namespace
