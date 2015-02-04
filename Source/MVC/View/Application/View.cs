using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YoloTrack.MVC.View.Components;

namespace YoloTrack.MVC.View.Application
{
    public partial class View : Form
    {
        public View()
        {
            InitializeComponent();

            databaseView1.Items.Add(new DatabaseViewItem()
            {
                Id = 1,
                FirstName = "Hans",
                LastName = "Yolo",
                LearnedAt = DateTime.Now,
                TimesRecognized = 0,
                TimesTracked = 0
            });

            databaseView1.Items.Add(new DatabaseViewItem()
            {
                Id = 2,
                FirstName = "Captain",
                LastName = "Obvious",
                LearnedAt = DateTime.Now,
                TimesRecognized = 5,
                TimesTracked = 0
            });

            databaseView1.Items.Add(new DatabaseViewItem()
            {
                Id = 3,
                FirstName = "Vincent",
                LastName = "Stech",
                LearnedAt = DateTime.Now,
                TimesRecognized = 0,
                TimesTracked = 0
            });

            databaseView1.Items.Add(new DatabaseViewItem()
            {
                Id = 4,
                FirstName = "Fabian",
                LastName = "Möbus",
                LearnedAt = DateTime.Now,
                TimesRecognized = 1,
                TimesTracked = 0
            });

            databaseView1.Items.Add(new DatabaseViewItem()
            {
                Id = 5,
                FirstName = "Florian",
                LastName = "Zorbach",
                LearnedAt = DateTime.Now,
                TimesRecognized = 1,
                TimesTracked = 1
            });

            databaseView1.Items.Add(new DatabaseViewItem()
            {
                Id = 6,
                FirstName = "Sebastian",
                LastName = "Büttner",
                LearnedAt = DateTime.Now,
                TimesRecognized = 5,
                TimesTracked = 3
            });

            databaseView1.Items.Add(new DatabaseViewItem()
            {
                Id = 7,
                LearnedAt = DateTime.Now,
                TimesRecognized = 17,
                TimesTracked = 17
            });

            visualTimer1.Start(20);

        }

        private void programToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openExistingDatabasefileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
