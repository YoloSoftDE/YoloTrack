using System;

namespace YoloTrack.MVC.Model.StateMachine.State
{
    /// <summary>
    /// State transistion logic for 'WaitTakePicture'
    /// </summary>
    class WaitTakePictureState : BaseState<Impl.WaitTakePictureImpl, Arg.WaitTakePictureArg>
    {
        /// <summary>
        /// Generalized construcor. No way to omit the arguments paremeter here.
        /// </summary>
        /// <param name="arg"></param>
        public WaitTakePictureState(Arg.WaitTakePictureArg arg)
            : base(arg)
        {
        }

        /// <summary>
        /// Transistion logic
        /// </summary>
        /// <returns></returns>
        protected override StateTransistion Transist()
        {
            BaseArg result = RunImpl();
            
            if (result.GetType() == typeof(Arg.IdentifyArg))
                return new IdentifyState((Arg.IdentifyArg)result);

            else if (result.GetType() == typeof(Arg.WaitForBodyArg))
                return new WaitForBodyState((Arg.WaitForBodyArg)result);

            return null;
        }
    } // End class
} // End namespace
