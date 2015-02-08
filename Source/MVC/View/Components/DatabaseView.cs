using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace YoloTrack.MVC.View.Components
{
    /// <summary>
    /// Database-View
    /// </summary>
    public partial class DatabaseView : UserControl
    {
        /// <summary>
        /// Event for the Merge-Button
        /// </summary>
        public event EventHandler<MergeEventArgs> MergeRequest;

        /// <summary>
        /// Get the number of items in the control
        /// </summary>
        public int Count { get { return this.Items.Count; } }

        /// <summary>
        /// Get or set the items in the control
        /// </summary>
        public DatabaseViewItemCollection Items { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DatabaseView()
        {
            InitializeComponent();

            this.Items = new DatabaseViewItemCollection();

            this.Items.Added += new EventHandler<DatabaseViewItemCollection.ListChangedEventArgs>(this.OnItemAdded);
            this.Items.Removed += new EventHandler<DatabaseViewItemCollection.ListChangedEventArgs>(this.OnItemRemoved);
        }

        /// <summary>
        /// OnItemAdded event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemAdded(object sender, DatabaseViewItemCollection.ListChangedEventArgs e)
        {
            _set_count();
            control_container.Controls.Add(e.Item);
            e.Item.Width = Width - 24;

            e.Item.Click += new EventHandler(Item_Click);
            e.Item.Margin = new Padding(1);
        }

        /// <summary>
        /// Select the item on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Item_Click(object sender, EventArgs e)
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
        private void OnItemRemoved(object sender, DatabaseViewItemCollection.ListChangedEventArgs e)
        {
            _set_count();
            control_container.Controls.Remove(e.Item);
        }

        /// <summary>
        /// Calculate Correct Item size on resize (subtract scrollbarwidth from itemwidth)
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
        /// Update the visible count
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
        /// ItemCollection Class
        /// </summary>
        public class DatabaseViewItemCollection : List<DatabaseViewItem>
        {
            public class ListChangedEventArgs : EventArgs
            {
                public DatabaseViewItem Item;
            }

            /// <summary>
            /// Fired, when a new item is added
            /// </summary>
            public event EventHandler<ListChangedEventArgs> Added;

            /// <summary>
            /// Fired, when an item is removed
            /// </summary>
            public event EventHandler<ListChangedEventArgs> Removed;

            /// <summary>
            /// Add a new item to the collection
            /// </summary>
            /// <param name="item"></param>
            public new void Add(DatabaseViewItem item)
            {
                base.Add(item);
                if (this.Added != null)
                {
                    this.Added(this, new ListChangedEventArgs()
                    {
                        Item = item
                    });
                }
            }

            /// <summary>
            /// Remove an item from the collection
            /// </summary>
            /// <param name="item"></param>
            public new void Remove(DatabaseViewItem item)
            {
                base.Remove(item);
                if (this.Removed != null)
                {
                    this.Removed(this, new ListChangedEventArgs()
                    {
                        Item = item
                    });
                }
            }
        }

        /// <summary>
        /// Hook repaint to refresh the header-control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            databaseViewHeader1.HasBottomLine = control_container.VerticalScroll.Value > 0;
            databaseViewHeader1.Refresh();
        }

        private void control_container_Scroll(object sender, ScrollEventArgs e)
        {
        }

        /// <summary>
        /// Click-Handler to merge the selected items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void control_container_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// Deselect all items on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void control_container_Click(object sender, EventArgs e)
        {
            foreach (DatabaseViewItem item in Items)
            {
                item.Selected = false;
            }
        }
    }

    /// <summary>
    /// Event-Args for merging a set of items
    /// </summary>
    public class MergeEventArgs : EventArgs
    {
        public DatabaseViewItem[] Items;
    }
}
