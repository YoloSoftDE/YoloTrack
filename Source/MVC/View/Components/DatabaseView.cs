using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
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
            control_container.Controls.Add(e.Item);
            e.Item.Width = Width - 24;
        }

        void OnItemRemoved(object sender, DatabaseViewItemCollection.ListChangedEventArgs e)
        {
            control_container.Controls.Remove(e.Item);
        }

        private void DatabaseView_Resize(object sender, EventArgs e)
        {
            foreach (DatabaseViewItem item in Items)
            {
                item.Width = Width - 24;
            }
        }
    }
}
