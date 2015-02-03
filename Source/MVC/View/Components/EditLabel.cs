using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YoloTrack.MVC.View.Components
{
    public partial class EditLabel : UserControl
    {

        public EditLabel()
        {
            InitializeComponent();

        }

        void label_GotFocus(object sender, EventArgs e)
        {
            /*
            text_box.Text = label.Text;
            label.Visible = false;
            text_box.Size = label.Size;
            text_box.Visible = false;            
             * */
        }
    }
}
