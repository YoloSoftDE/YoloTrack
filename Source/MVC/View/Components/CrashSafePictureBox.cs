using System;
using System.Windows.Forms;

namespace YoloTrack.MVC.View.Components
{
    /// <summary>
    /// Crash safe implementation of the popular but very shitty original component by mircosoft. Have fun.
    /// </summary>
    class CrashSafePictureBox : PictureBox
    {
        protected override void OnPaint(PaintEventArgs pe)
        {
            try
            {
                base.OnPaint(pe);
            }
            catch (Exception)
            {
                // ¯\ (ツ) /¯
            }
        }
    }
}
