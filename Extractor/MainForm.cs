using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Nexon.CSO.Extractor.Properties;

namespace Nexon.CSO.Extractor
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			this.InitializeComponent();
		}

		public string InitialLoadFile { get; set; }

		private void NARExtractorForm_Load(object sender, EventArgs e)
		{
			Settings settings = Settings.Default;
			if (!settings.FormSize.IsEmpty)
			{
				base.Size = settings.FormSize;
			}
			if (settings.FormWindowState != FormWindowState.Minimized)
			{
				base.WindowState = settings.FormWindowState;
			}
			if (settings.SplitterDistance > 0)
			{
				this.splitContainer1.SplitterDistance = settings.SplitterDistance;
			}
			if (settings.ColumnNameWidth >= 0)
			{
				this.listView.Columns[0].Width = settings.ColumnNameWidth;
			}
			if (settings.ColumnNamePosition >= 0)
			{
				this.listView.Columns[0].DisplayIndex = settings.ColumnNamePosition;
			}
			if (settings.ColumnLastModifiedWidth >= 0)
			{
				this.listView.Columns[1].Width = settings.ColumnLastModifiedWidth;
			}
			if (settings.ColumnLastModifiedPosition >= 0)
			{
				this.listView.Columns[1].DisplayIndex = settings.ColumnLastModifiedPosition;
			}
			if (settings.ColumnSizeWidth >= 0)
			{
				this.listView.Columns[2].Width = settings.ColumnSizeWidth;
			}
			if (settings.ColumnSizePosition >= 0)
			{
				this.listView.Columns[2].DisplayIndex = settings.ColumnSizePosition;
			}
			if (settings.SortColumn >= 0)
			{
				this.listView.SortColumn = settings.SortColumn;
				this.listView.SortColumnOrder = SortOrder.None;
			}
			if (settings.SortColumnOrder != SortOrder.None)
			{
				this.listView.SortColumnOrder = settings.SortColumnOrder;
			}
			this.autoDecryptModelsToolStripMenuItem.Checked = settings.AutoDecryptModels;
			this.pathToolStripStatusLabel.Text = string.Empty;
			this.fileToolStripStatusLabel.Text = string.Empty;
		}

		private void NARExtractorForm_Shown(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.InitialLoadFile))
			{
				this.Open(this.InitialLoadFile);
			}
		}

		private void NARExtractorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.CloseArchive();
		}

		private void NARExtractorForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Settings settings = Settings.Default;
			settings.FormSize = ((base.WindowState == FormWindowState.Minimized || base.WindowState == FormWindowState.Maximized) ? base.RestoreBounds.Size : base.Size);
			settings.FormWindowState = base.WindowState;
			settings.SplitterDistance = this.splitContainer1.SplitterDistance;
			settings.ColumnNameWidth = this.listView.Columns[0].Width;
			settings.ColumnNamePosition = this.listView.Columns[0].DisplayIndex;
			settings.ColumnLastModifiedWidth = this.listView.Columns[1].Width;
			settings.ColumnLastModifiedPosition = this.listView.Columns[1].DisplayIndex;
			settings.ColumnSizeWidth = this.listView.Columns[2].Width;
			settings.ColumnSizePosition = this.listView.Columns[2].DisplayIndex;
			settings.SortColumn = this.listView.SortColumn;
			settings.SortColumnOrder = this.listView.SortColumnOrder;
			settings.AutoDecryptModels = this.autoDecryptModelsToolStripMenuItem.Checked;
			settings.Save();
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.OpenDialog();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.listView.Focus();
			foreach (object obj in this.listView.Items)
			{
				ListViewItem item = (ListViewItem)obj;
				item.Selected = true;
			}
		}

		private void extractAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.archive != null && this.treeView.TopNode != null)
			{
				List<NexonArchiveFileEntry> files = new List<NexonArchiveFileEntry>();
				FolderTreeView.GetFilesRecursive(this.treeView.TopNode, files);
				this.ExtractFiles(this, new FilesEventArgs(FolderTreeView.GetFullPath(this.treeView.TopNode), files));
			}
		}

		private void verifyAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.archive != null && this.treeView.TopNode != null)
			{
				List<NexonArchiveFileEntry> files = new List<NexonArchiveFileEntry>();
				FolderTreeView.GetFilesRecursive(this.treeView.TopNode, files);
				this.VerifyFiles(this, new FilesEventArgs(FolderTreeView.GetFullPath(this.treeView.TopNode), files));
			}
		}

		private void autoDecryptModelsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Settings.Default.AutoDecryptModels = this.autoDecryptModelsToolStripMenuItem.Checked;
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (AboutBox aboutBox = new AboutBox())
			{
				aboutBox.ShowDialog();
			}
		}

		private void NARExtractorForm_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Move;
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		private void NARExtractorForm_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
				if (files != null && files.Length > 0)
				{
					this.Open(files[0]);
				}
			}
		}

		private void treeView_ShowFolder(object sender, FilesEventArgs e)
		{
			this.pathToolStripStatusLabel.Text = e.Path;
			this.listView.FullPath = e.Path;
			this.listView.BeginUpdate();
			this.listView.Items.Clear();
			this.listView_SelectedIndexChanged(this, EventArgs.Empty);
			if (e.Files != null)
			{
				foreach (NexonArchiveFileEntry entry in e.Files)
				{
					this.listView.AddFile(entry);
				}
			}
			this.listView.EndUpdate();
			this.listView_SelectedIndexChanged(this, EventArgs.Empty);
		}

		private void listView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.treeView.SelectedNode == null)
			{
				this.fileToolStripStatusLabel.Text = string.Empty;
				return;
			}
			if (this.listView.SelectedIndices.Count > 0)
			{
				if (this.listView.SelectedIndices.Count == 1)
				{
					this.fileToolStripStatusLabel.Text = string.Format(NumberFormatInfo.CurrentInfo, "{0} item selected", new object[]
					{
						this.listView.SelectedIndices.Count
					});
					return;
				}
				this.fileToolStripStatusLabel.Text = string.Format(NumberFormatInfo.CurrentInfo, "{0} items selected", new object[]
				{
					this.listView.SelectedIndices.Count
				});
				return;
			}
			else
			{
				if (this.listView.Items.Count == 1)
				{
					this.fileToolStripStatusLabel.Text = string.Format(NumberFormatInfo.CurrentInfo, "{0} item", new object[]
					{
						this.listView.Items.Count
					});
					return;
				}
				this.fileToolStripStatusLabel.Text = string.Format(NumberFormatInfo.CurrentInfo, "{0} items", new object[]
				{
					this.listView.Items.Count
				});
				return;
			}
		}

		private void SetTitle(string fileName)
		{
			this.SetTitle(fileName, false);
		}

		private void SetTitle(string fileName, bool modified)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				this.Text = string.Format(CultureInfo.CurrentUICulture, "NAR Extractor", new object[0]);
				return;
			}
			if (modified)
			{
				this.Text = string.Format(CultureInfo.CurrentUICulture, "NAR Extractor [{0}]*", new object[]
				{
					fileName
				});
				return;
			}
			this.Text = string.Format(CultureInfo.CurrentUICulture, "NAR Extractor [{0}]", new object[]
			{
				fileName
			});
		}

		private void CloseArchive()
		{
			if (this.archive != null)
			{
				this.treeView.Nodes.Clear();
				this.listView.Items.Clear();
				this.archive.Close();
				this.SetTitle(null);
			}
		}

		private void OpenDialog()
		{
			if (this.narOpenFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.Open(this.narOpenFileDialog.FileName);
			}
		}

		public void Open(string fileName)
		{
			this.CloseArchive();
			try
			{
				this.archive = new NexonArchive();
				this.archive.Load(fileName, false);
			}
			catch (Exception)
			{
				MessageBox.Show(this, "Could not open file: " + fileName, "Error");
				return;
			}
			this.SetTitle(fileName);
			this.treeView.LoadArchive(this.archive);
		}

		private void ExtractFiles(object sender, FilesEventArgs e)
		{
			if (this.extractFolderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				MainForm.ExtractHelper extractHelper = new MainForm.ExtractHelper();
				extractHelper.DecryptModels = Settings.Default.AutoDecryptModels;
				extractHelper.ExtractPath = this.extractFolderBrowserDialog.SelectedPath;
				using (TaskDialog taskDialog = new TaskDialog(new Predicate<NexonArchiveFileEntry>(extractHelper.Extract), e.Files))
				{
					taskDialog.Text = "Extracting files...";
					taskDialog.DestinationPath = extractHelper.ExtractPath;
					DialogResult result = taskDialog.ShowDialog(this);
					if (result == DialogResult.Abort)
					{
						MessageBox.Show("An error occured while extracting the files.", "Error");
					}
					else if (result == DialogResult.OK)
					{
						MessageBox.Show("All the selected files have been extracted successfully.", "Extraction Complete");
					}
				}
			}
		}

		private static bool VerifyFilesHelper(NexonArchiveFileEntry file)
		{
			return file.Verify();
		}

		private void VerifyFiles(object sender, FilesEventArgs e)
		{
			using (TaskDialog taskDialog = new TaskDialog(new Predicate<NexonArchiveFileEntry>(MainForm.VerifyFilesHelper), e.Files))
			{
				taskDialog.Text = "Verifying files...";
				DialogResult result = taskDialog.ShowDialog(this);
				if (result == DialogResult.Abort)
				{
					MessageBox.Show("An error occured while verifying the files.", "Error");
				}
				else if (result == DialogResult.OK)
				{
					MessageBox.Show("All the selected files have been verified successfully.", "Verification Complete");
				}
			}
		}

		private NexonArchive archive;

		private sealed class ExtractHelper
		{
			public bool Extract(NexonArchiveFileEntry file)
			{
				string path = file.Path;
				int slashIndex = 0;
				while (path[slashIndex] == Path.DirectorySeparatorChar || path[slashIndex] == Path.AltDirectorySeparatorChar)
				{
					slashIndex++;
				}
				path = path.Substring(slashIndex);
				path = Path.Combine(this.ExtractPath, path);
				DirectoryInfo pathDirectory = new DirectoryInfo(Path.GetDirectoryName(path));
				if (!pathDirectory.Exists)
				{
					pathDirectory.Create();
				}
				using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
				{
					file.Extract(fileStream);
					if (this.DecryptModels && string.Compare(Path.GetExtension(path), ".mdl", StringComparison.OrdinalIgnoreCase) == 0)
					{
						ModelHelper.DecryptModel(fileStream);
					}
				}
				return true;
			}

			public bool DecryptModels;

			public string ExtractPath;
		}
	}
}
