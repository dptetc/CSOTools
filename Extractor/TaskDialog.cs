using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Nexon.CSO.Extractor
{
	public partial class TaskDialog : Form
	{
		public TaskDialog(Predicate<NexonArchiveFileEntry> task, ICollection<NexonArchiveFileEntry> entries)
		{
			if (task == null)
			{
				throw new ArgumentNullException("task");
			}
			if (entries == null)
			{
				throw new ArgumentNullException("entries");
			}
			this.task = task;
			this.entries = entries;
			this.InitializeComponent();
		}

		public string DestinationPath
		{
			get
			{
				return this.destinationPath;
			}
			set
			{
				this.destinationPath = value;
			}
		}

		private void NexusArchiveTaskDialog_Load(object sender, EventArgs e)
		{
			this.backgroundWorker.RunWorkerAsync();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			if (this.backgroundWorker.IsBusy)
			{
				this.backgroundWorker.CancelAsync();
				this.progressBar.Style = ProgressBarStyle.Marquee;
				return;
			}
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		private void InitializeProgressWork(object state)
		{
			this.progressBar.Style = ProgressBarStyle.Continuous;
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			lock (this.entries)
			{
				TaskDialog.ProgressUpdate progress = new TaskDialog.ProgressUpdate();
				foreach (NexonArchiveFileEntry entry in this.entries)
				{
					progress.TotalFileSize += entry.Size;
				}
				if (this.backgroundWorker.CancellationPending)
				{
					e.Cancel = true;
				}
				else
				{
					Delegate method = new Action<object>(this.InitializeProgressWork);
					object[] args = new object[1];
					base.Invoke(method, args);
					if (this.backgroundWorker.CancellationPending)
					{
						e.Cancel = true;
					}
					else
					{
						foreach (NexonArchiveFileEntry entry2 in this.entries)
						{
							this.backgroundWorker.ReportProgress(0, entry2.Path);
							if (!this.task(entry2))
							{
								throw new ApplicationException("Task sent an abort code.");
							}
							if (this.backgroundWorker.CancellationPending)
							{
								e.Cancel = true;
								break;
							}
							progress.CurrentFileSize += entry2.Size;
							this.backgroundWorker.ReportProgress(0, progress);
						}
					}
				}
			}
		}

		private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			TaskDialog.ProgressUpdate progress = e.UserState as TaskDialog.ProgressUpdate;
			if (progress != null)
			{
				int width = this.progressBar.ClientSize.Width;
				this.progressBar.Maximum = width;
				this.progressBar.Value = Convert.ToInt32(progress.CurrentFileSize * (long)width / progress.TotalFileSize);
				return;
			}
			string path = e.UserState as string;
			if (!string.IsNullOrEmpty(path))
			{
				path = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
				int slashIndex = 0;
				while (path[slashIndex] == Path.DirectorySeparatorChar)
				{
					slashIndex++;
				}
				path = path.Substring(slashIndex);
				this.sourcePathLabel.Text = path;
				if (this.destinationPath != null)
				{
					this.destinationPathLabel.Text = Path.Combine(this.destinationPath, path);
				}
			}
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				base.DialogResult = DialogResult.Abort;
			}
			else if (e.Cancelled)
			{
				base.DialogResult = DialogResult.Cancel;
			}
			else
			{
				this.progressBar.Value = this.progressBar.Maximum;
				base.DialogResult = DialogResult.OK;
			}
			base.Close();
		}

		private Predicate<NexonArchiveFileEntry> task;

		private ICollection<NexonArchiveFileEntry> entries;

		private string destinationPath;

		private sealed class ProgressUpdate
		{
			public long CurrentFileSize;

			public long TotalFileSize;
		}
	}
}
