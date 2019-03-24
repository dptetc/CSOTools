using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Nexon.CSO.Extractor
{
	public class FolderTreeView : TreeView
	{
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
			this.contextMenuStrip = new ContextMenuStrip(this.components);
			this.extractToolStripMenuItem = new ToolStripMenuItem();
			this.verifyToolStripMenuItem = new ToolStripMenuItem();
			this.contextMenuStrip.SuspendLayout();
			base.SuspendLayout();
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
			this.ContextMenuStrip = this.contextMenuStrip;
			base.HideSelection = false;
			base.LineColor = Color.Black;
			base.AfterSelect += this.FolderTreeView_AfterSelect;
			base.MouseDown += this.FolderTreeView_MouseDown;
			this.contextMenuStrip.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		public event EventHandler<FilesEventArgs> ShowFolder;

		public event EventHandler<FilesEventArgs> ExtractFolder;

		public event EventHandler<FilesEventArgs> VerifyFolder;

		public FolderTreeView()
		{
			this.InitializeComponent();
			base.TreeViewNodeSorter = new FolderTreeView.TreeSort(this);
		}

		protected void OnShowFolder(FilesEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.ShowFolder != null)
			{
				this.ShowFolder(this, e);
			}
		}

		protected void OnExtractFolder(FilesEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.ExtractFolder != null)
			{
				this.ExtractFolder(this, e);
			}
		}

		protected void OnVerifyFolder(FilesEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.VerifyFolder != null)
			{
				this.VerifyFolder(this, e);
			}
		}

		private void FolderTreeView_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
			{
				base.SelectedNode = base.GetNodeAt(e.Location);
				if (base.SelectedNode == null)
				{
					this.OnShowFolder(new FilesEventArgs());
				}
			}
		}

		private void FolderTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			this.OnShowFolder(new FilesEventArgs(FolderTreeView.GetFullPath(e.Node), e.Node.Tag as List<NexonArchiveFileEntry>));
		}

		private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			if (base.SelectedNode == null)
			{
				e.Cancel = true;
			}
		}

		private void extractToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (base.SelectedNode != null)
			{
				List<NexonArchiveFileEntry> files = new List<NexonArchiveFileEntry>();
				FolderTreeView.GetFilesRecursive(base.SelectedNode, files);
				this.OnExtractFolder(new FilesEventArgs(FolderTreeView.GetFullPath(base.SelectedNode), files));
			}
		}

		private void verifyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (base.SelectedNode != null)
			{
				List<NexonArchiveFileEntry> files = new List<NexonArchiveFileEntry>();
				FolderTreeView.GetFilesRecursive(base.SelectedNode, files);
				this.OnVerifyFolder(new FilesEventArgs(FolderTreeView.GetFullPath(base.SelectedNode), files));
			}
		}

		private static TreeNode FindOrCreateNodePath(TreeNode rootNode, string path)
		{
			if (path.Length == 0)
			{
				return rootNode;
			}
			int startIndex = 0;
			while (path[startIndex] == '/' || path[startIndex] == '\\')
			{
				startIndex++;
			}
			int separatorIndex = path.IndexOfAny(new char[]
			{
				'/',
				'\\'
			}, startIndex);
			string name;
			if (separatorIndex >= 0)
			{
				name = path.Substring(startIndex, separatorIndex - startIndex);
			}
			else
			{
				name = path.Substring(startIndex);
			}
			TreeNode finalNode = null;
			foreach (object obj in rootNode.Nodes)
			{
				TreeNode node = (TreeNode)obj;
				if (string.Compare(node.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
				{
					finalNode = node;
					break;
				}
			}
			if (finalNode == null)
			{
				finalNode = new TreeNode(name);
				finalNode.Name = name;
				rootNode.Nodes.Add(finalNode);
			}
			if (separatorIndex >= 0)
			{
				return FolderTreeView.FindOrCreateNodePath(finalNode, path.Substring(separatorIndex + 1));
			}
			return finalNode;
		}

		public void LoadArchive(NexonArchive archive)
		{
			base.Nodes.Clear();
			if (archive == null)
			{
				return;
			}
			TreeNode rootNode = new TreeNode("(root)");
			foreach (NexonArchiveFileEntry entry in archive.FileEntries)
			{
				TreeNode node = FolderTreeView.FindOrCreateNodePath(rootNode, Path.GetDirectoryName(entry.Path));
				IList<NexonArchiveFileEntry> nodeList = node.Tag as IList<NexonArchiveFileEntry>;
				if (nodeList == null)
				{
					nodeList = new List<NexonArchiveFileEntry>();
					node.Tag = nodeList;
				}
				nodeList.Add(entry);
			}
			rootNode.Expand();
			base.Nodes.Add(rootNode);
			base.SelectedNode = rootNode;
		}

		public static IList<NexonArchiveFileEntry> GetFilesRecursive(TreeNode node, IList<NexonArchiveFileEntry> files)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			if (files == null)
			{
				throw new ArgumentNullException("files");
			}
			IList<NexonArchiveFileEntry> directoryFiles = node.Tag as IList<NexonArchiveFileEntry>;
			if (directoryFiles != null)
			{
				foreach (NexonArchiveFileEntry file in directoryFiles)
				{
					files.Add(file);
				}
			}
			foreach (object obj in node.Nodes)
			{
				TreeNode childNode = (TreeNode)obj;
				FolderTreeView.GetFilesRecursive(childNode, files);
			}
			return files;
		}

		public static string GetFullPath(TreeNode node)
		{
			if (node == null || node.Parent == null)
			{
				return string.Empty;
			}
			if (node.Parent != null && node.Parent.Parent != null)
			{
				return FolderTreeView.GetFullPath(node.Parent) + "/" + node.Text;
			}
			return node.Text;
		}

		private IContainer components;

		private ContextMenuStrip contextMenuStrip;

		private ToolStripMenuItem extractToolStripMenuItem;

		private ToolStripMenuItem verifyToolStripMenuItem;

		private sealed class TreeSort : IComparer
		{
			public TreeSort(FolderTreeView treeView)
			{
				this.treeView = treeView;
			}

			public int Compare(object x, object y)
			{
				TreeNode xNode = x as TreeNode;
				TreeNode yNode = y as TreeNode;
				if (xNode == null)
				{
					if (yNode == null)
					{
						return 0;
					}
					return -1;
				}
				else
				{
					if (yNode == null)
					{
						return 1;
					}
					return string.Compare(xNode.Text, yNode.Text, StringComparison.CurrentCultureIgnoreCase);
				}
			}

			private FolderTreeView treeView;
		}
	}
}
