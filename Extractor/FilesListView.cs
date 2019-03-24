using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Nexon.CSO.Extractor
{
	public class FilesListView : ListView
	{
		public event EventHandler<FilesEventArgs> ExtractFiles;

		public event EventHandler<FilesEventArgs> VerifyFiles;

		public FilesListView()
		{
			this.InitializeComponent();
			base.ListViewItemSorter = new FilesListView.ColumnSort(this)
			{
				Column = -1,
				Order = SortOrder.None
			};
		}

		public string FullPath { get; set; }

		public int SortColumn
		{
			get
			{
				FilesListView.ColumnSort columnSort = base.ListViewItemSorter as FilesListView.ColumnSort;
				if (columnSort != null && columnSort.Column >= 0 && columnSort.Column < base.Columns.Count)
				{
					return columnSort.Column;
				}
				return -1;
			}
			set
			{
				FilesListView.ColumnSort columnSort = base.ListViewItemSorter as FilesListView.ColumnSort;
				if (columnSort != null)
				{
					columnSort.Column = value;
					base.Sort();
				}
			}
		}

		public SortOrder SortColumnOrder
		{
			get
			{
				FilesListView.ColumnSort columnSort = base.ListViewItemSorter as FilesListView.ColumnSort;
				if (columnSort != null && columnSort.Column >= 0 && columnSort.Column < base.Columns.Count)
				{
					return columnSort.Order;
				}
				return SortOrder.None;
			}
			set
			{
				FilesListView.ColumnSort columnSort = base.ListViewItemSorter as FilesListView.ColumnSort;
				if (columnSort != null)
				{
					columnSort.Order = value;
					base.Sort();
				}
			}
		}

		protected void OnExtractFiles(FilesEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.ExtractFiles != null)
			{
				this.ExtractFiles(this, e);
			}
		}

		protected void OnVerifyFiles(FilesEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.VerifyFiles != null)
			{
				this.VerifyFiles(this, e);
			}
		}

		private void FilesListView_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			FilesListView.ColumnSort columnSorter = base.ListViewItemSorter as FilesListView.ColumnSort;
			if (columnSorter != null)
			{
				if (columnSorter.Column != e.Column)
				{
					columnSorter.Column = e.Column;
					columnSorter.Order = SortOrder.Ascending;
				}
				else
				{
					switch (columnSorter.Order)
					{
					case SortOrder.None:
					case SortOrder.Descending:
						columnSorter.Order = SortOrder.Ascending;
						break;
					case SortOrder.Ascending:
						columnSorter.Order = SortOrder.Descending;
						break;
					}
				}
				this.SetSortIcon(columnSorter.Column, columnSorter.Order);
				base.Sort();
				return;
			}
			this.SetSortIcon(-1, SortOrder.None);
		}

		private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			if (base.SelectedIndices.Count <= 0)
			{
				e.Cancel = true;
			}
		}

		private void extractToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (base.SelectedIndices.Count > 0)
			{
				List<NexonArchiveFileEntry> files = new List<NexonArchiveFileEntry>();
				foreach (object obj in base.SelectedItems)
				{
					ListViewItem item = (ListViewItem)obj;
					NexonArchiveFileEntry file = item.Tag as NexonArchiveFileEntry;
					if (file != null)
					{
						files.Add(file);
					}
				}
				this.OnExtractFiles(new FilesEventArgs(this.FullPath, files));
			}
		}

		private void verifyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (base.SelectedIndices.Count > 0)
			{
				List<NexonArchiveFileEntry> files = new List<NexonArchiveFileEntry>();
				foreach (object obj in base.SelectedItems)
				{
					ListViewItem item = (ListViewItem)obj;
					NexonArchiveFileEntry file = item.Tag as NexonArchiveFileEntry;
					if (file != null)
					{
						files.Add(file);
					}
				}
				this.OnVerifyFiles(new FilesEventArgs(this.FullPath, files));
			}
		}

		protected override void CreateHandle()
		{
			base.CreateHandle();
			FilesListView.ColumnSort columnSorter = base.ListViewItemSorter as FilesListView.ColumnSort;
			if (columnSorter != null)
			{
				this.SetSortIcon(columnSorter.Column, columnSorter.Order);
			}
		}

		private static string GetHumanSize(long size)
		{
			if (size < 0L)
			{
				return string.Empty;
			}
			double temp = (double)size;
			if (temp <= 1024.0)
			{
				return temp.ToString("n0", NumberFormatInfo.CurrentInfo) + ((temp == 1.0) ? " byte" : " bytes");
			}
			temp /= 1024.0;
			if (temp <= 1024.0)
			{
				return temp.ToString("n0", NumberFormatInfo.CurrentInfo) + " KB";
			}
			temp /= 1024.0;
			if (temp > 1024.0)
			{
				return (temp / 1024.0).ToString("n2", NumberFormatInfo.CurrentInfo) + " GB";
			}
			return temp.ToString("n2", NumberFormatInfo.CurrentInfo) + " MB";
		}

		public ListViewItem AddFile(NexonArchiveFileEntry file)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			ListViewItem item = new ListViewItem(Path.GetFileName(file.Path));
			item.Tag = file;
			item.SubItems.Add(file.LastModifiedTime.ToString(DateTimeFormatInfo.CurrentInfo));
			item.SubItems.Add(FilesListView.GetHumanSize(file.Size));
			base.Items.Add(item);
			return item;
		}

		private void SetSortIcon(int columnIndex, SortOrder order)
		{
			IntPtr columnHeader = NativeMethods.SendMessage(base.Handle, 4127, IntPtr.Zero, IntPtr.Zero);
			for (int columnNumber = 0; columnNumber <= base.Columns.Count - 1; columnNumber++)
			{
				IntPtr columnPtr = new IntPtr(columnNumber);
				NativeMethods.LVCOLUMN lvColumn = default(NativeMethods.LVCOLUMN);
				lvColumn.mask = 4;
				NativeMethods.SendMessage(columnHeader, 4619, columnPtr, ref lvColumn);
				if (order != SortOrder.None && columnNumber == columnIndex)
				{
					switch (order)
					{
					case SortOrder.Ascending:
						lvColumn.fmt &= -513;
						lvColumn.fmt |= 1024;
						break;
					case SortOrder.Descending:
						lvColumn.fmt &= -1025;
						lvColumn.fmt |= 512;
						break;
					}
				}
				else
				{
					lvColumn.fmt &= -1537;
				}
				NativeMethods.SendMessage(columnHeader, 4620, columnPtr, ref lvColumn);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			this.colName = new ColumnHeader();
			this.colLastModified = new ColumnHeader();
			this.colSize = new ColumnHeader();
			this.contextMenuStrip = new ContextMenuStrip(this.components);
			this.extractToolStripMenuItem = new ToolStripMenuItem();
			this.verifyToolStripMenuItem = new ToolStripMenuItem();
			this.contextMenuStrip.SuspendLayout();
			base.SuspendLayout();
			this.colName.Text = "Name";
			this.colName.Width = 150;
			this.colLastModified.Text = "Last Modified";
			this.colLastModified.Width = 150;
			this.colSize.Text = "Size";
			this.colSize.Width = 80;
			this.contextMenuStrip.Items.AddRange(new ToolStripItem[]
			{
				this.extractToolStripMenuItem,
				this.verifyToolStripMenuItem
			});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new Size(119, 48);
			this.contextMenuStrip.Opening += this.contextMenuStrip_Opening;
			this.extractToolStripMenuItem.Name = "extractToolStripMenuItem";
			this.extractToolStripMenuItem.Size = new Size(118, 22);
			this.extractToolStripMenuItem.Text = "E&xtract...";
			this.extractToolStripMenuItem.Click += this.extractToolStripMenuItem_Click;
			this.verifyToolStripMenuItem.Name = "verifyToolStripMenuItem";
			this.verifyToolStripMenuItem.Size = new Size(118, 22);
			this.verifyToolStripMenuItem.Text = "&Verify";
			this.verifyToolStripMenuItem.Click += this.verifyToolStripMenuItem_Click;
			base.Columns.AddRange(new ColumnHeader[]
			{
				this.colName,
				this.colLastModified,
				this.colSize
			});
			this.ContextMenuStrip = this.contextMenuStrip;
			base.FullRowSelect = true;
			base.HideSelection = false;
			base.View = View.Details;
			base.ColumnClick += this.FilesListView_ColumnClick;
			this.contextMenuStrip.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private IContainer components;

		private ColumnHeader colName;

		private ColumnHeader colLastModified;

		private ColumnHeader colSize;

		private ContextMenuStrip contextMenuStrip;

		private ToolStripMenuItem extractToolStripMenuItem;

		private ToolStripMenuItem verifyToolStripMenuItem;

		private sealed class ColumnSort : IComparer
		{
			public ColumnSort(FilesListView listView)
			{
				this.listView = listView;
			}

			public int Compare(object x, object y)
			{
				ListViewItem xItem = x as ListViewItem;
				ListViewItem yItem = y as ListViewItem;
				if (xItem == null)
				{
					if (yItem == null)
					{
						return 0;
					}
					return -1;
				}
				else
				{
					if (yItem == null)
					{
						return 1;
					}
					if (this.Column < 0 || this.Order == SortOrder.None)
					{
						return 0;
					}
					if (this.Order != SortOrder.Ascending && this.Order != SortOrder.Descending)
					{
						return 0;
					}
					NexonArchiveFileEntry xFile = xItem.Tag as NexonArchiveFileEntry;
					NexonArchiveFileEntry yFile = yItem.Tag as NexonArchiveFileEntry;
					if (this.Column >= xItem.SubItems.Count && this.Column >= yItem.SubItems.Count)
					{
						return 0;
					}
					int value;
					switch (this.Column)
					{
					case 1:
						if (xFile != null && yFile != null)
						{
							value = DateTime.Compare(xFile.LastModifiedTime, yFile.LastModifiedTime);
							goto IL_10F;
						}
						break;
					case 2:
						if (xFile != null && yFile != null)
						{
							value = xFile.Size.CompareTo(yFile.Size);
							goto IL_10F;
						}
						break;
					}
					value = string.Compare(xItem.SubItems[this.Column].Text, yItem.SubItems[this.Column].Text, StringComparison.CurrentCultureIgnoreCase);
					IL_10F:
					if (this.Order == SortOrder.Descending)
					{
						return -value;
					}
					return value;
				}
			}

			private FilesListView listView;

			public int Column = -1;

			public SortOrder Order;
		}
	}
}
