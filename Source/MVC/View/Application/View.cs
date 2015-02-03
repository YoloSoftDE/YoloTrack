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
                Id = 1337,
                FirstName = "Hans",
                LastName = "Yolo",
                LearnedAt = DateTime.Now
            });

            databaseView1.Items.Add(new DatabaseViewItem()
            {
                Id = 42,
                FirstName = "Captain",
                LastName = "Obvious",
                LearnedAt = DateTime.Now
            });

            databaseView1.Items.Add(new DatabaseViewItem()
            {
                Id = 42,
                FirstName = "Captain",
                LastName = "Obvious",
                LearnedAt = DateTime.Now
            });

            databaseView1.Items.Add(new DatabaseViewItem()
            {
                Id = 42,
                FirstName = "Captain",
                LastName = "Obvious",
                LearnedAt = DateTime.Now
            });

            databaseView1.Items.Add(new DatabaseViewItem()
            {
                Id = 42,
                FirstName = "Captain",
                LastName = "Obvious",
                LearnedAt = DateTime.Now
            });

            databaseView1.Items.Add(new DatabaseViewItem()
            {
                Id = 1337,
                FirstName = "Hans",
                LastName = "Yolo",
                LearnedAt = DateTime.Now
            });

            databaseView1.Items.Add(new DatabaseViewItem()
            {
                Id = 42,
                FirstName = "Captain",
                LastName = "Obvious",
                LearnedAt = DateTime.Now
            });

            databaseView1.Items.Add(new DatabaseViewItem()
            {
                Id = 42,
                FirstName = "Captain",
                LastName = "Obvious",
                LearnedAt = DateTime.Now
            });

            databaseView1.Items.Add(new DatabaseViewItem()
            {
                Id = 42,
                FirstName = "Captain",
                LastName = "Obvious",
                LearnedAt = DateTime.Now
            });

            databaseView1.Items.Add(new DatabaseViewItem()
            {
                Id = 42,
                FirstName = "Captain",
                LastName = "Obvious",
                LearnedAt = DateTime.Now
            });
        }
    }
}
