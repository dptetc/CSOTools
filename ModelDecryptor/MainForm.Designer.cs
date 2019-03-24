namespace Nexon.CSO.ModelDecryptor
{
	public partial class MainForm : global::System.Windows.Forms.Form
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
			this.addButton = new global::System.Windows.Forms.Button();
			this.removeButton = new global::System.Windows.Forms.Button();
			this.modifyButton = new global::System.Windows.Forms.Button();
			this.exitButton = new global::System.Windows.Forms.Button();
			this.decryptButton = new global::System.Windows.Forms.Button();
			this.listBox = new global::System.Windows.Forms.ListBox();
			this.inputTextBox = new global::System.Windows.Forms.TextBox();
			this.backgroundWorker = new global::System.ComponentModel.BackgroundWorker();
			this.backupCheckBox = new global::System.Windows.Forms.CheckBox();
			global::System.Windows.Forms.Label inputLabel = new global::System.Windows.Forms.Label();
			global::System.Windows.Forms.TableLayoutPanel tableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			tableLayoutPanel.SuspendLayout();
			base.SuspendLayout();
			inputLabel.AutoSize = true;
			inputLabel.Location = new global::System.Drawing.Point(270, 9);
			inputLabel.Name = "inputLabel";
			inputLabel.Size = new global::System.Drawing.Size(87, 13);
			inputLabel.TabIndex = 2;
			inputLabel.Text = "&Input File/Folder:";
			tableLayoutPanel.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			tableLayoutPanel.ColumnCount = 3;
			tableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 33.33333f));
			tableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 33.33333f));
			tableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 33.33333f));
			tableLayoutPanel.Controls.Add(this.addButton, 0, 0);
			tableLayoutPanel.Controls.Add(this.removeButton, 2, 0);
			tableLayoutPanel.Controls.Add(this.modifyButton, 1, 0);
			tableLayoutPanel.Location = new global::System.Drawing.Point(270, 90);
			tableLayoutPanel.Name = "tableLayoutPanel";
			tableLayoutPanel.RowCount = 1;
			tableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 100f));
			tableLayoutPanel.Size = new global::System.Drawing.Size(291, 29);
			tableLayoutPanel.TabIndex = 7;
			this.addButton.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.addButton.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this.addButton.Location = new global::System.Drawing.Point(3, 3);
			this.addButton.Name = "addButton";
			this.addButton.Size = new global::System.Drawing.Size(91, 23);
			this.addButton.TabIndex = 6;
			this.addButton.Text = "&Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new global::System.EventHandler(this.addButton_Click);
			this.removeButton.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.removeButton.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this.removeButton.Location = new global::System.Drawing.Point(197, 3);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new global::System.Drawing.Size(91, 23);
			this.removeButton.TabIndex = 6;
			this.removeButton.Text = "&Remove";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new global::System.EventHandler(this.removeButton_Click);
			this.modifyButton.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.modifyButton.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this.modifyButton.Location = new global::System.Drawing.Point(100, 3);
			this.modifyButton.Name = "modifyButton";
			this.modifyButton.Size = new global::System.Drawing.Size(91, 23);
			this.modifyButton.TabIndex = 6;
			this.modifyButton.Text = "&Modify";
			this.modifyButton.UseVisualStyleBackColor = true;
			this.modifyButton.Click += new global::System.EventHandler(this.modifyButton_Click);
			this.exitButton.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom;
			this.exitButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.exitButton.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this.exitButton.Location = new global::System.Drawing.Point(289, 241);
			this.exitButton.Name = "exitButton";
			this.exitButton.Size = new global::System.Drawing.Size(75, 23);
			this.exitButton.TabIndex = 0;
			this.exitButton.Text = "E&xit";
			this.exitButton.UseVisualStyleBackColor = true;
			this.exitButton.Click += new global::System.EventHandler(this.exitButton_Click);
			this.decryptButton.Anchor = global::System.Windows.Forms.AnchorStyles.Bottom;
			this.decryptButton.FlatStyle = global::System.Windows.Forms.FlatStyle.System;
			this.decryptButton.Location = new global::System.Drawing.Point(208, 241);
			this.decryptButton.Name = "decryptButton";
			this.decryptButton.Size = new global::System.Drawing.Size(75, 23);
			this.decryptButton.TabIndex = 0;
			this.decryptButton.Text = "&Decrypt";
			this.decryptButton.UseVisualStyleBackColor = true;
			this.decryptButton.Click += new global::System.EventHandler(this.decryptButton_Click);
			this.listBox.AllowDrop = true;
			this.listBox.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Left);
			this.listBox.FormattingEnabled = true;
			this.listBox.IntegralHeight = false;
			this.listBox.Location = new global::System.Drawing.Point(12, 12);
			this.listBox.Name = "listBox";
			this.listBox.Size = new global::System.Drawing.Size(252, 223);
			this.listBox.TabIndex = 1;
			this.listBox.SelectedIndexChanged += new global::System.EventHandler(this.ListBox_SelectedIndexChanged);
			this.listBox.DragDrop += new global::System.Windows.Forms.DragEventHandler(this.ListBox_DragDrop);
			this.listBox.DragEnter += new global::System.Windows.Forms.DragEventHandler(this.File_DragEnter);
			this.inputTextBox.AllowDrop = true;
			this.inputTextBox.Anchor = (global::System.Windows.Forms.AnchorStyles.Top | global::System.Windows.Forms.AnchorStyles.Left | global::System.Windows.Forms.AnchorStyles.Right);
			this.inputTextBox.Location = new global::System.Drawing.Point(270, 25);
			this.inputTextBox.Name = "inputTextBox";
			this.inputTextBox.Size = new global::System.Drawing.Size(291, 20);
			this.inputTextBox.TabIndex = 3;
			this.inputTextBox.DragDrop += new global::System.Windows.Forms.DragEventHandler(this.TextBox_DragDrop);
			this.inputTextBox.Leave += new global::System.EventHandler(this.TextBox_Leave);
			this.inputTextBox.Enter += new global::System.EventHandler(this.TextBox_Enter);
			this.inputTextBox.DragEnter += new global::System.Windows.Forms.DragEventHandler(this.File_DragEnter);
			this.backgroundWorker.WorkerSupportsCancellation = true;
			this.backgroundWorker.DoWork += new global::System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
			this.backgroundWorker.RunWorkerCompleted += new global::System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
			this.backupCheckBox.AutoSize = true;
			this.backupCheckBox.Checked = true;
			this.backupCheckBox.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.backupCheckBox.Location = new global::System.Drawing.Point(270, 125);
			this.backupCheckBox.Name = "backupCheckBox";
			this.backupCheckBox.Size = new global::System.Drawing.Size(138, 17);
			this.backupCheckBox.TabIndex = 8;
			this.backupCheckBox.Text = "&Backup Encrypted Files";
			this.backupCheckBox.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.exitButton;
			base.ClientSize = new global::System.Drawing.Size(573, 276);
			base.Controls.Add(this.backupCheckBox);
			base.Controls.Add(tableLayoutPanel);
			base.Controls.Add(this.inputTextBox);
			base.Controls.Add(inputLabel);
			base.Controls.Add(this.listBox);
			base.Controls.Add(this.decryptButton);
			base.Controls.Add(this.exitButton);
			this.MinimumSize = new global::System.Drawing.Size(495, 196);
			base.Name = "MainForm";
			this.Text = "Model Decryptor by Da_FileServer";
			base.Load += new global::System.EventHandler(this.ModelDecryptorForm_Load);
			tableLayoutPanel.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private global::System.ComponentModel.IContainer components;

		private global::System.Windows.Forms.Button exitButton;

		private global::System.Windows.Forms.Button decryptButton;

		private global::System.Windows.Forms.ListBox listBox;

		private global::System.Windows.Forms.TextBox inputTextBox;

		private global::System.Windows.Forms.Button addButton;

		private global::System.Windows.Forms.Button modifyButton;

		private global::System.Windows.Forms.Button removeButton;

		private global::System.ComponentModel.BackgroundWorker backgroundWorker;

		private global::System.Windows.Forms.CheckBox backupCheckBox;
	}
}
