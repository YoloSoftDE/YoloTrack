using System;
using System.Drawing;
using System.Windows.Forms;

namespace YoloTrack.MVC.View.Components
{
    public partial class DetailEditView : UserControl
    {
        public event EventHandler<FirstNameChangedEventArgs> FirstNameChanged;

        public event EventHandler<LastNameChangedEventArgs> LastNameChanged;

        public event EventHandler<ImageChangedEventArgs> ImageChanged;

        public event EventHandler<TrackingRequestEventArgs> TrackingRequest;

        public event EventHandler<HaltTrackingRequestEventArgs> HaltTrackingRequest;

        public event EventHandler<DeleteRequestEventArgs> DeleteRequest;

        public int Id
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
                _show_id();
            }
        }

        public string FirstName
        {
            get
            {
                return m_first_name;
            }
            set
            {
                m_first_name = value;
                _show_first_name();
            }
        }

        public string LastName
        {
            get
            {
                return m_last_name;
            }
            set
            {
                m_last_name = value;
                _show_last_name();
            }
        }

        public int TimesRecognized
        {
            get
            {
                return m_times_recognized;
            }
            set
            {
                m_times_recognized = value;
                _show_counters();
            }
        }

        public int TimesTracked
        {
            get
            {
                return m_times_tracked;
            }
            set
            {
                m_times_tracked = value;
                _show_counters();
            }
        }

        public int TrackingId
        {
            get
            {
                return m_tracking_id;
            }
            set
            {
                m_tracking_id = value;
                _show_rt_info();
            }
        }

        public int IdentifyAttempts
        {
            get
            {
                return m_identify_attemps;
            }
            set
            {
                m_identify_attemps = value;
                _show_rt_info();
            }
        }

        public Image Image
        {
            get
            {
                return m_image;
            }
            set
            {
                m_image = value;
                _show_image();
            }
        }

        public DateTime LearnedAt
        {
            get
            {
                return m_learned_at;
            }
            set
            {
                m_learned_at = value;
                _show_learned_at();
            }
        }

        public bool IsTracked
        {
            get
            {
                return m_is_tracked;
            }
            set
            {
                m_is_tracked = value;
                _show_is_tracked();
            }
        }

        private int m_id;
        private string m_first_name;
        private string m_last_name;
        private int m_times_recognized;
        private int m_times_tracked;
        private int m_tracking_id;
        private int m_identify_attemps;
        private DateTime m_learned_at;
        private Image m_image;
        private bool m_is_tracked;

        public DetailEditView()
        {
            InitializeComponent();
            DoubleBuffered = true;

            label_first_name.Text = "Sebastian";
            label_last_name.Text = "Büttner";
        }

        private void DetailEditView_Load(object sender, EventArgs e)
        {

        }

        private void DetailEditView_Paint(object sender, PaintEventArgs e)
        {
            Pen blackpen = new Pen(SystemColors.ActiveBorder, 1);
            Graphics g = e.Graphics;
            g.DrawLine(blackpen, 0, 0, Width, 0);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            DialogResult user_input = openFileDialog1.ShowDialog();
            if (user_input == DialogResult.OK)
            {
                if (ImageChanged != null)
                {
                    ImageChanged(this, new ImageChangedEventArgs()
                    {
                        Image = Bitmap.FromFile(openFileDialog1.FileName)
                    });
                }
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (Image == null)
            {
                pb_image.BackColor = SystemColors.ControlDarkDark;
            }
            toolTip1.Show("Click to select another image for this record", pb_image);
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            if (Image == null)
            {
                pb_image.BackColor = SystemColors.ControlDark;
            }
        }

        private void _show_rt_info()
        {
            if (TrackingId > 0)
            {
                label_rt_info.Text = "Current assigned TrackingId is " +
                    TrackingId.ToString() + ", " +
                    IdentifyAttempts.ToString() + " attempts required for identification.";
            } else
            {
                label_rt_info.Text = "Currently not in view range.";
            }
        }

        private void _show_first_name()
        {
            label_first_name.Text = FirstName;
        }

        private void _show_last_name()
        {
            label_last_name.Text = LastName;
        }

        private void _show_counters()
        {
            if (TimesRecognized == 0)
            {
                label_counters.Text = "Never seen";
            }
            else if (TimesRecognized == TimesTracked)
            {
                if (TimesTracked == 1)
                {
                    label_counters.Text = "1 time tracked";
                }
                else
                {
                    label_counters.Text = TimesTracked.ToString() + " times tracked";
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
                label_counters.Text = text;
            }
        }

        private void _show_id()
        {
            label_id.Text = Id.ToString();
        }

        private void _show_image()
        {
            pb_image.Image = Image;
        }

        private void _show_learned_at()
        {
            label_learned_at.Text = "Listed since " + LearnedAt.ToShortDateString() + ", " + LearnedAt.ToShortTimeString();
        }

        private void _show_is_tracked()
        {
            button_track.Checked = IsTracked;
            if (IsTracked)
            {
                button_track.Text = "Unset Target";
            }
            else
            {
                button_track.Text = "Set as Target";
            }
        }

        private void button_track_CheckedChanged(object sender, EventArgs e)
        {
            if (button_track.Checked)
            {
                if (TrackingRequest != null)
                {
                    TrackingRequest(this, new TrackingRequestEventArgs()
                    {
                        DatabaseId = Id
                    });
                }
            } else
            {
                if (HaltTrackingRequest != null)
                {
                    HaltTrackingRequest(this, new HaltTrackingRequestEventArgs()
                    {
                        DatabaseId = Id
                    });
                }
            }
        }

        private void label_first_name_TextChanged(object sender, EventArgs e)
        {
            if (FirstNameChanged != null)
            {
                FirstNameChanged(this, new FirstNameChangedEventArgs()
                {
                    FirstName = label_first_name.Text
                });
            }
        }

        private void label_last_name_TextChanged(object sender, EventArgs e)
        {
            if (LastNameChanged != null)
            {
                LastNameChanged(this, new LastNameChangedEventArgs()
                {
                    LastName = label_last_name.Text
                });
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (DeleteRequest != null)
            {
                DeleteRequest(this, new DeleteRequestEventArgs()
                {
                    DatabaseId = Id
                });
            }
        }
    }

    public class FirstNameChangedEventArgs : EventArgs
    {
        public string FirstName;
    }

    public class LastNameChangedEventArgs : EventArgs
    {
        public string LastName;
    }

    public class ImageChangedEventArgs : EventArgs
    {
        public Image Image;
    }

    public class TrackingRequestEventArgs : EventArgs
    {
        public int DatabaseId;
    }

    public class DeleteRequestEventArgs : EventArgs
    {
        public int DatabaseId;
    }

    public class HaltTrackingRequestEventArgs : EventArgs
    {
        public int DatabaseId;
    }
}
