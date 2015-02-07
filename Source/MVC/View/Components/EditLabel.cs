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
        public enum EditLabelMode
        {
            LabelMode,
            EditMode
        }

        public class ModeChangedEventArgs : EventArgs
        {
            public ModeChangedEventArgs(EditLabelMode mode)
            {
                this.Mode = mode;
            }

            public EditLabelMode Mode { get; protected set; }
        }

        public event EventHandler<ModeChangedEventArgs> ModeChanged;

        [Browsable(true)]
        public new event EventHandler TextChanged;

        private string m_string = "";
        private EditLabelMode m_mode = EditLabelMode.LabelMode;

        public EditLabelMode Mode
        {
            get
            {
                return m_mode;
            }
            set
            {
                m_mode = value;

                switch (value)
                {
                    case EditLabelMode.EditMode:
                        m_textbox.Visible = true;
                        m_textbox.Focus();
                        m_label.Visible = false;
                        m_textbox.Size = m_label.Size;
                        break;

                    case EditLabelMode.LabelMode:
                        m_textbox.Visible = false;
                        m_label.Visible = true;
                        break;

                }
                OnModeChanged(new ModeChangedEventArgs(value));
            }
        }

        public override string Text
        {
            get
            {
                return m_string;
            }
            set
            {
                if (value == "")
                {
                    m_string = DefaultText;
                }
                else
                {
                    m_string = value;
                }

                m_label.Text = m_string;
                m_textbox.Text = m_string;

                this.OnTextChanged(new EventArgs());
            }
        }

        [DefaultValue("<Empty>")]
        public string DefaultText { get; set; }

        public EditLabel()
        {
            InitializeComponent();

            m_textbox.LostFocus += new EventHandler(m_textbox_LostFocus);
        }

        #region Event Callers

        protected void OnModeChanged(ModeChangedEventArgs e)
        {
            if (this.ModeChanged != null)
            {
                this.ModeChanged(this, e);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            this.TextChanged(this, e);
        }

        #endregion


        #region Event Handlers
        private void m_textbox_LostFocus(object sender, EventArgs e)
        {
            if (Mode == EditLabelMode.EditMode)
            {
                this._leave_edit_mode();
            }
        }

        private void m_label_Click(object sender, EventArgs e)
        {
            if (Mode == EditLabelMode.LabelMode)
            {
                this._enter_edit_mode();
            }
        }

        private void m_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (Mode != EditLabelMode.EditMode)
            {
                e.SuppressKeyPress = true;
                return;
            }

            /* Check for enter key to end input mode */
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                this._leave_edit_mode();
            }
        }
        #endregion

        private void _enter_edit_mode()
        {
            Mode = EditLabelMode.EditMode;
        }

        private void _leave_edit_mode()
        {
            Mode = EditLabelMode.LabelMode;
            this.Text = m_textbox.Text;

        }


    }
}
