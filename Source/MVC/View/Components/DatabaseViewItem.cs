using System;
using System.Drawing;
using System.Windows.Forms;

namespace YoloTrack.MVC.View.Components
{
    /// <summary>
    /// A visual simple item in the database
    /// </summary>
    public partial class DatabaseViewItem : UserControl
    {
        private bool m_selected = false;
        private string m_first_name = "";
        private int m_id;
        private string m_last_name = ""; 
        private DateTime m_learned_at = new DateTime();
        private int m_times_recognized;
        private int m_times_tracked;

        /// <summary>
        /// Get or set the selected-status
        /// </summary>
        public bool Selected 
        { 
            get
            {
                return m_selected;
            }
            set
            {
                m_selected = value;
                _draw();
            }
        }

        /// <summary>
        /// Get or set the id-number
        /// </summary>
        public int Id
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
                _set_id();
            }
        }

        /// <summary>
        /// Property for the first name to get or set.
        /// </summary>
        public string FirstName
        {
            get
            {
                return m_first_name;
            }
            set
            {
                m_first_name = value;
                _set_name();
            }
        }

        /// <summary>
        /// Property for the last name to get or set.
        /// </summary>
        public string LastName
        {
            get
            {
                return m_last_name;
            }
            set
            {
                m_last_name = value;
                _set_name();
            }
        }

        /// <summary>
        /// Property for the datetime the record was learned.
        /// </summary>
        public DateTime LearnedAt
        {
            get
            {
                return m_learned_at;
            }
            set
            {
                m_learned_at = value;
                _set_learned_at();
            }
        }

        /// <summary>
        /// Get or set the number of times this profile has been recognized by the software
        /// </summary>
        public int TimesRecognized
        {
            get
            {
                return m_times_recognized;
            }
            set
            {
                m_times_recognized = value;
                _set_counters();
            }
        }

        /// <summary>
        /// Get or set the number of times this profile as been tracked by the software
        /// </summary>
        public int TimesTracked
        {
            get
            {
                return m_times_tracked;
            }
            set
            {
                m_times_tracked = value;
                _set_counters();
            }
        }

        /// <summary>
        /// Get ot set the user profile image
        /// </summary>
        public Image Image
        {
            get
            {
                return m_picturebox_user_image.Image;
            }
            set
            {
                m_picturebox_user_image.Image = value;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DatabaseViewItem()
        {
            InitializeComponent();

            _register_event(Controls);
            _set_name();
            _set_id();
            _set_counters();
            _set_learned_at();
        }

        /// <summary>
        /// Register Events for all Controls on the thing
        /// </summary>
        /// <param name="Controls"></param>
        private void _register_event(ControlCollection Controls)
        {
            foreach (Control control in Controls)
            {
                control.MouseEnter += new EventHandler(DatabaseViewItem_MouseEnter);
                control.MouseLeave += new EventHandler(DatabaseViewItem_MouseLeave);
                control.Click += new EventHandler(DatabaseViewItem_Click);
                _register_event(control.Controls);
            }
        }

        /// <summary>
        /// Add visual feedback for mousehover-interaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatabaseViewItem_MouseEnter(object sender, EventArgs e)
        {
            _draw(true);
        }

        /// <summary>
        /// Add visual feedback for mousehover-interaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatabaseViewItem_MouseLeave(object sender, EventArgs e)
        {
            _draw();
        }

        /// <summary>
        /// Draw visual feedback for mouse-interaction
        /// </summary>
        /// <param name="Hover"></param>
        private void _draw(bool Hover = false)
        {
            if (Hover || this.Selected)
            {
                this.BackColor = SystemColors.MenuHighlight;
                this.label_name.ForeColor = SystemColors.HighlightText;
                this.label_learned_at.ForeColor = SystemColors.HighlightText;
                this.label_text_counters.ForeColor = SystemColors.HighlightText;
                this.label_id.BackColor = SystemColors.HotTrack;
            }
            else
            {
                this.BackColor = SystemColors.Control;
                this.label_name.ForeColor = SystemColors.ControlText;
                this.label_learned_at.ForeColor = SystemColors.GrayText;
                this.label_text_counters.ForeColor = SystemColors.GrayText;
                this.label_id.BackColor = SystemColors.Highlight;
                if (this.FirstName.Length == 0 && this.LastName.Length == 0)
                {
                    this.label_name.ForeColor = SystemColors.GrayText;
                }
            }
        }

        /// <summary>
        /// Add feedback for mouseclick-interaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatabaseViewItem_Click(object sender, EventArgs e)
        {
            Focus();
            OnClick(e);
        }

        /// <summary>
        /// Update the id on the label
        /// </summary>
        private void _set_id()
        {
            label_id.Text = Id.ToString();
        }

        /// <summary>
        /// Update the name based on availability of first and lastname
        /// </summary>
        private void _set_name()
        {
            SuspendLayout();
            label_name.ForeColor = SystemColors.ControlText;

            label_name.Text = "";
            if (FirstName.Length > 0)
            {
                label_name.Text = FirstName;
            }
            if (LastName.Length > 0)
            {
                if (FirstName.Length > 0)
                {
                    label_name.Text += " ";
                }
                label_name.Text += LastName;
            }
            else if (FirstName.Length == 0)
            {
                label_name.Text = "Unnamed";
                label_name.ForeColor = SystemColors.GrayText;
            }

            ResumeLayout();
        }

        /// <summary>
        /// Update the learned-date
        /// </summary>
        private void _set_learned_at()
        {
            label_learned_at.Text = "Listed since " + LearnedAt.ToShortDateString() + ", " + LearnedAt.ToShortTimeString();
        }

        /// <summary>
        /// Update the times-tracked counter with correct grammar
        /// </summary>
        private void _set_counters()
        {
            if (TimesRecognized == 0)
            {
                label_text_counters.Text = "Never seen";
            }
            else if (TimesRecognized == TimesTracked)
            {
                if (TimesTracked == 1)
                {
                    label_text_counters.Text = "1 time tracked";
                }
                else
                {
                    label_text_counters.Text = TimesTracked.ToString() + " times tracked";
                }
            }
            else
            {
                string text = "";
                if (TimesRecognized == 1)
                {
                    text += "1 time seen";
                }
                else
                {
                    text += TimesRecognized.ToString() + " times seen";
                }

                if (TimesTracked == 0)
                {
                    text += ", but never tracked";
                }
                else if (TimesTracked == 1)
                {
                    text += " and 1 time tracked";
                }
                else
                {
                    text += " and " + TimesTracked.ToString() + " times tracked";
                }
                label_text_counters.Text = text;
            }
        }
    }
}
