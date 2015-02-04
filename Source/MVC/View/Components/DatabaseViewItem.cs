using System;
using System.Drawing;
using System.Windows.Forms;

namespace YoloTrack.MVC.View.Components
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DatabaseViewItem : UserControl
    {
        private bool m_selected = false;
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
        /// 
        /// </summary>
        private int m_id;
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
        private string m_first_name = "";
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
        private string m_last_name = "";
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
        private DateTime m_learned_at = new DateTime();
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
        /// 
        /// </summary>
        private int m_times_recognized;
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
        /// 
        /// </summary>
        private int m_times_tracked;
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
        /// Default construcor
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
        /// 
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatabaseViewItem_Paint(object sender, PaintEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatabaseViewItem_MouseEnter(object sender, EventArgs e)
        {
            _draw(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatabaseViewItem_MouseLeave(object sender, EventArgs e)
        {
            _draw();
        }

        private void _draw(bool Hover = false)
        {
            if (Hover || Selected)
            {
                BackColor = SystemColors.MenuHighlight;
                label_name.ForeColor = SystemColors.HighlightText;
                label_learned_at.ForeColor = SystemColors.HighlightText;
                label_text_counters.ForeColor = SystemColors.HighlightText;
                label_id.BackColor = SystemColors.HotTrack;
            }
            else
            {
                BackColor = SystemColors.Control;
                label_name.ForeColor = SystemColors.ControlText;
                label_learned_at.ForeColor = SystemColors.GrayText;
                label_text_counters.ForeColor = SystemColors.GrayText;
                label_id.BackColor = SystemColors.Highlight;
                if (FirstName.Length == 0 && LastName.Length == 0)
                {
                    label_name.ForeColor = SystemColors.GrayText;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatabaseViewItem_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        private void _set_id()
        {
            label_id.Text = Id.ToString();
        }

        private void _set_name()
        {
            SuspendLayout();
            label_name.ForeColor = SystemColors.ControlText;

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

        private void _set_learned_at()
        {
            label_learned_at.Text = "Listed since " + LearnedAt.ToShortDateString() + ", " + LearnedAt.ToShortTimeString();
        }

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
