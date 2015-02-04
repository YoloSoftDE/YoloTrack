using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

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
        public event EventHandler<MergeEventArgs> MergeRequest;

        /// <summary>
        /// 
        /// </summary>
        public int Count { get{ return Items.Count; } }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnItemAdded(object sender, DatabaseViewItemCollection.ListChangedEventArgs e)
        {
            _set_count();
            control_container.Controls.Add(e.Item);
            e.Item.Width = Width - 24;

            e.Item.Click += new EventHandler(Item_Click);
            e.Item.Margin = new Padding(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Item_Click(object sender, EventArgs e)
        {
            DatabaseViewItem item = (DatabaseViewItem)sender;
            if (ModifierKeys == Keys.Shift)
            {
                item.Selected = !item.Selected;
            }
            else
            {
                foreach (DatabaseViewItem i in Items)
                {
                    i.Selected = false;
                }
                item.Selected = true;
            }

            int selected_count = Items.FindAll(i => i.Selected == true).Count;
            databaseViewHeader1.SelectedItems = selected_count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnItemRemoved(object sender, DatabaseViewItemCollection.ListChangedEventArgs e)
        {
            _set_count();
            control_container.Controls.Remove(e.Item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatabaseView_Resize(object sender, EventArgs e)
        {
            foreach (DatabaseViewItem item in Items)
            {
                item.Width = Width - 24;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _set_count()
        {
            databaseViewHeader1.ItemCount = Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_merge_Click(object sender, EventArgs e)
        {
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

            public new void Add(DatabaseViewItem item)
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

            /// <summary>
            /// 
            /// </summary>
            /// <param name="item"></param>
            public new void Remove(DatabaseViewItem item)
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            databaseViewHeader1.HasBottomLine = control_container.VerticalScroll.Value > 0;
            databaseViewHeader1.Refresh();
        }

        private void control_container_Scroll(object sender, ScrollEventArgs e)
        {
        }

        private void databaseViewHeader1_MergeClick(object sender, EventArgs e)
        {
            if (Items.FindAll(item => item.Selected == true).Count >= 2)
            {
                MergeEventArgs args = new MergeEventArgs()
                {
                    Items = Items.FindAll(item => item.Selected == true).ToArray()
                };
                foreach (DatabaseViewItem item in Items)
                {
                    if (item.Selected)
                    {
                        item.Selected = false;
                    }
                }

                if (MergeRequest != null)
                {
                    MergeRequest(this, args);
                }
            }
            databaseViewHeader1.SelectedItems = 0;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MergeEventArgs : EventArgs
    {
        public DatabaseViewItem[] Items;
    }
}
