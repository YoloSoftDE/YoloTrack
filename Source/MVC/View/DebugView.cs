using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YoloTrack.MVC.View
{
    enum DebugLevel {
        NOTICE,
        INFO,
        WARN,
        ERROR,
        CRIT,
        EMERGE
    }

    class DebugView : IObserver
    {
        public DebugView()
        {
        }

        public void Observe(Model.TrackingModel model)
        {
            // Register event handlers
            model.OnRuntimeInfoChange += new Model.Storage.RuntimeInfoChangeHandler(model_OnRuntimeInfoChange);
            model.OnStateChange += new Model.StateChangeHandler(model_OnStateChange);
        }

        void model_OnStateChange(Model.StateMachine.StateTransistion current_state, Model.StateMachine.StateTransistion next_state)
        {
            Log(String.Format("StateChanged {0} -> {1}", current_state.GetType().Name, next_state.GetType().Name), DebugLevel.INFO);
        }

        void model_OnRuntimeInfoChange(Model.Storage.RuntimeInfo info)
        {
            //Log("RuntimeInfoChanged", DebugLevel.NOTICE);
        }

        private void Log(string Message, DebugLevel level = DebugLevel.NOTICE)
        {
            Console.WriteLine("[{0}] {1}", level, Message);
        }
    }
}
