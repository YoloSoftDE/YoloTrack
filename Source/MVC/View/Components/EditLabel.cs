using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace YoloTrack.Source.MVC.View.Components
{
    /// <summary>
    /// Cool editable label with neat events
    /// </summary>
    public partial class EditLabel : UserControl
    {
        /// <summary>
        /// Mode of the Label
        /// </summary>
        public enum EditLabelMode
        {
            LabelMode,
            EditMode
        }

        /// <summary>
        /// Custom eventargs
        /// </summary>
        public class ModeChangedEventArgs : EventArgs
        {
            public ModeChangedEventArgs(EditLabelMode mode)
            {
                this.Mode = mode;
            }

            public EditLabelMode Mode { get; protected set; }
        }

        /// <summary>
        /// Current text-value of the label
        /// </summary>
        private string m_string = "";

        /// <summary>
        /// Current mode of the label
        /// </summary>
        private EditLabelMode m_mode = EditLabelMode.LabelMode;


        /// <summary>
        /// Fired when the mode of the label has changed
        /// </summary>
        public event EventHandler<ModeChangedEventArgs> ModeChanged;

        /// <summary>
        /// Fired, when the Text of the label has changed.
        /// Explicit marked as browsable!
        /// </summary>
        [Browsable(true)]
        public new event EventHandler TextChanged;

        /// <summary>
        /// Get or set the mode of the label
        /// </summary>
        public EditLabelMode Mode
        {
            get
            {
                return m_mode;
            }
            set
            {
                /* Store the mode */
                m_mode = value;

                /* Update visual design */
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

                /* Fire event */
                OnModeChanged(new ModeChangedEventArgs(value));
            }
        }

        /// <summary>
        /// Get or set the current ext of the label
        /// </summary>
        public override string Text
        {
            get
            {
                return m_string;
            }
            set
            {
                m_string = value;
                if (m_string == "")
                {
                    m_label.Text = DefaultText;
                    m_label.Font = new Font(m_label.Font, FontStyle.Italic);
                    m_label.ForeColor = SystemColors.GrayText;
                }
                else
                {
                    m_label.Text = m_string;
                    m_label.Font = new Font(m_label.Font, FontStyle.Regular);
                    m_label.ForeColor = SystemColors.ControlText;
                }
                m_textbox.Text = m_string;

                //this.OnTextChanged(new EventArgs());
            }
        }

        [DefaultValue("<Empty>")]
        public string DefaultText { get; set; }

        public EditLabel()
        {
            InitializeComponent();

            /* Register LostFocus-Eventhandler */
            m_textbox.LostFocus += new EventHandler(m_textbox_LostFocus);
        }

        #region Event Callers

        /// <summary>
        /// Event-Caller for ModeChanged-event
        /// </summary>
        /// <param name="e">Instance of ModeChangedEventArgs</param>
        protected virtual OnModeChanged(ModeChangedEventArgs e)
        {
            if (this.ModeChanged != null)
            {
                this.ModeChanged(this, e);
            }
        }

        /// <summary>
        /// Event-Caller for TextChanged-event
        /// </summary>
        /// <param name="e">Empty EventArgs</param>
        protected override void OnTextChanged(EventArgs e)
        {
            if (TextChanged != null)
            {
                this.TextChanged(this, e);
            }
        }

        #endregion


        #region Event Handlers
        protected virtual void m_textbox_LostFocus(object sender, EventArgs e)
        {
            /* Reset to LabelMode if focus of textbox is lost */

            if (Mode == EditLabelMode.EditMode)
            {
                this._leave_edit_mode();
            }
        }

        protected virtual void m_label_Click(object sender, EventArgs e)
        {
            if (Mode == EditLabelMode.LabelMode)
            {
                /* Enter EditMode if label is clicked */
                this._enter_edit_mode();
            }
        }

        protected virtual void m_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            /* Suppress all keypresses if not in EditMode */
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

        /// <summary>
        /// Enter the EditMode
        /// </summary>
        private void _enter_edit_mode()
        {
            Mode = EditLabelMode.EditMode;
        }

        /// <summary>
        /// Leave the EditMode
        /// </summary>
        private void _leave_edit_mode()
        {
            Mode = EditLabelMode.LabelMode;
            this.Text = m_textbox.Text;
            this.OnTextChanged(new EventArgs());
        }


    }
}
