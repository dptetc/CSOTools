namespace Nexon.CSO.Extractor
{
	public partial class TaskDialog : global::System.Windows.Forms.Form
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
			this.progressBar = new global::System.Windows.Forms.ProgressBar();
			this.cancelButton = new global::System.Windows.Forms.Button();
			this.backgroundWorker = new global::System.ComponentModel.BackgroundWorker();
			this.destinationPathLabel = new global::Nexon.CSO.Extractor.PathLabel();
			this.sourcePathLabel = new global::Nexon.CSO.Extractor.PathLabel();
			global::System.Windows.Forms.Label label = new global::System.Windows.Forms.Label();
			global::System.Windows.Forms.Label label2 = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			label.AutoSize = true;
			label.Location = new global::System.Drawing.Point(12, 9);
			label.Name = "label1";
			label.Size = new global::System.Drawing.Size(44, 13);
			label.TabIndex = 0;
			label.Text = "&Source:";
			label2.AutoSize = true;
			label2.Location = new global::System.Drawing.Point(12, 29);
			label2.Name = "label2";
			label2.Size = new global::System.Drawing.Size(63, 13);
			label2.TabIndex = 2;
			label2.Text = "&Destination:";
			this.progressBar.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.progressBar.Location = new global::System.Drawing.Point(12, 52);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new global::System.Drawing.Size(380, 23);
			this.progressBar.Style = global::System.Windows.Forms.ProgressBarStyle.Marquee;
			this.progressBar.TabIndex = 4;
			this.cancelButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Right);
			this.cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this.cancelButton.Location = new global::System.Drawing.Point(317, 81);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new global::System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new global::System.EventHandler(this.cancelButton_Click);
			this.backgroundWorker.WorkerReportsProgress = true;
			this.backgroundWorker.WorkerSupportsCancellation = true;
			this.backgroundWorker.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
			this.backgroundWorker.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
			this.backgroundWorker.ProgressChanged += new global::System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
			this.destinationPathLabel.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.destinationPathLabel.Location = new global::System.Drawing.Point(81, 29);
			this.destinationPathLabel.Name = "destinationPathLabel";
			this.destinationPathLabel.Size = new global::System.Drawing.Size(311, 13);
			this.destinationPathLabel.TabIndex = 3;
			this.destinationPathLabel.Text = "n/a";
			this.sourcePathLabel.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.sourcePathLabel.Location = new global::System.Drawing.Point(81, 9);
			this.sourcePathLabel.Name = "sourcePathLabel";
			this.sourcePathLabel.Size = new global::System.Drawing.Size(311, 13);
			this.sourcePathLabel.TabIndex = 1;
			this.sourcePathLabel.Text = "n/a";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.cancelButton;
			base.ClientSize = new global::System.Drawing.Size(404, 116);
			base.Controls.Add(this.cancelButton);
			base.Controls.Add(this.progressBar);
			base.Controls.Add(this.destinationPathLabel);
			base.Controls.Add(label2);
			base.Controls.Add(this.sourcePathLabel);
			base.Controls.Add(label);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "NexonArchiveTaskDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Task Dialog...";
			base.Shown += new global::System.EventHandler(this.NexusArchiveTaskDialog_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private global::System.ComponentModel.IContainer components;

		private global::Nexon.CSO.Extractor.PathLabel sourcePathLabel;

		private global::Nexon.CSO.Extractor.PathLabel destinationPathLabel;

		private global::System.Windows.Forms.ProgressBar progressBar;

		private global::System.Windows.Forms.Button cancelButton;

		private global::System.ComponentModel.BackgroundWorker backgroundWorker;
	}
}
