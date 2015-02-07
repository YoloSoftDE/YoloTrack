using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YoloTrack.Source.MVC.View.Components
{
    public partial class EditLabel : UserControl
    {
        [DefaultValue(EditLabelMode.LabelMode)]
        public EditLabelMode Mode { get; set; }

        private string m_string = "";
        public new string Text
        {
            get
            {
                return m_string;
            }
            set
            {
                master.Text = value;
            }
        }

        private Control master
        {
            get
            {
                switch (Mode)
                {
                    case EditLabelMode.EditMode:
                        return textBox1;

                    case EditLabelMode.LabelMode:
                    default:
                        return label1;
                }
            }
        }

        public EditLabel()
        {
            InitializeComponent();

            textBox1.GotFocus += new EventHandler(textBox1_GotFocus);
            textBox1.LostFocus += new EventHandler(textBox1_LostFocus);
        }

        void textBox1_GotFocus(object sender, EventArgs e)
        {
        }

        void textBox1_LostFocus(object sender, EventArgs e)
        {
            if (Mode == EditLabelMode.EditMode)
            {
                label1.Text = master.Text;
                Mode = EditLabelMode.LabelMode;
                textBox1.Visible = false;
                label1.Visible = true;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (Mode == EditLabelMode.LabelMode)
            {
                textBox1.Text = master.Text;
                Mode = EditLabelMode.EditMode;
                textBox1.Visible = true;
                //textBox1.Focus();
                label1.Visible = false;
                textBox1.Size = label1.Size;
            }
        }

        public enum EditLabelMode
        {
            LabelMode,
            EditMode
        }
    }
}
