using System;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    /// <summary>
    /// State transistion logic for 'Learn'
    /// </summary>
    class LearnState : BaseState<Impl.LearnImpl, Arg.LearnArg>
    {
        /// <summary>
        /// Generalized construcor. No way to omit the arguments paremeter here.
        /// </summary>
        /// <param name="arg"></param>
        public LearnState(Arg.LearnArg arg)
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

            return null;
        }
    } // End class
} // End namespace
