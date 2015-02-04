using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace YoloTrack.MVC.View.Components
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DatabaseView : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler BeginMerge;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MergeEventArgs> EndMerge;

        public bool Merging { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                return Items.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class DatabaseViewItemCollection : List<DatabaseViewItem>
        {
            public class ListChangedEventArgs : EventArgs
            {
                public DatabaseViewItem Item;
            }

            public event EventHandler<ListChangedEventArgs> Added;
            public event EventHandler<ListChangedEventArgs> Removed;

            public void Add(DatabaseViewItem item)
            {
                base.Add(item);
                if (Added != null)
                {
                    Added(this, new ListChangedEventArgs()
                    {
                        Item = item
                    });
                }
            }

            public void Remove(DatabaseViewItem item)
            {
                base.Remove(item);
                if (Removed != null)
                {
                    Removed(this, new ListChangedEventArgs()
                    {
                        Item = item
                    });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DatabaseViewItemCollection Items { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseView()
        {
            InitializeComponent();

            Items = new DatabaseViewItemCollection();

            Items.Added += new EventHandler<DatabaseViewItemCollection.ListChangedEventArgs>(OnItemAdded);
            Items.Removed += new EventHandler<DatabaseViewItemCollection.ListChangedEventArgs>(OnItemRemoved);
        }

        void OnItemAdded(object sender, DatabaseViewItemCollection.ListChangedEventArgs e)
        {
            _set_count();
            control_container.Controls.Add(e.Item);
            e.Item.Width = Width - 24;

            e.Item.Click += new EventHandler(Item_Click);
        }

        void Item_Click(object sender, EventArgs e)
        {
            if (Merging)
            {
                DatabaseViewItem item = (DatabaseViewItem)sender;
                item.Selected = !item.Selected;
            }
        }

        void OnItemRemoved(object sender, DatabaseViewItemCollection.ListChangedEventArgs e)
        {
            _set_count();
            control_container.Controls.Remove(e.Item);
        }

        private void DatabaseView_Resize(object sender, EventArgs e)
        {
            foreach (DatabaseViewItem item in Items)
            {
                item.Width = Width - 24;
            }
        }

        private void _set_count()
        {
            label_count.Text = Count.ToString();
            if (Count == 1)
            {
                label1.Text = "record.";
            }
            else
            {
                label1.Text = "records.";
            }
        }

        private void button_merge_Click(object sender, EventArgs e)
        {
            Merging = !Merging;
            if (Merging)
            {
                button_merge.Text = "Select items...";
            }
            else
            {
                button_merge.Text = "Merge";
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MergeEventArgs : EventArgs
    {
        DatabaseViewItem[] Items;
    }
}
