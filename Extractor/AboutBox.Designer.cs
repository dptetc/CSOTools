namespace Nexon.CSO.Extractor
{
	internal partial class AboutBox : global::System.Windows.Forms.Form
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
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::Nexon.CSO.Extractor.AboutBox));
			this.tableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			this.logoPictureBox = new global::System.Windows.Forms.PictureBox();
			this.labelProductName = new global::System.Windows.Forms.Label();
			this.labelVersion = new global::System.Windows.Forms.Label();
			this.labelCopyright = new global::System.Windows.Forms.Label();
			this.labelCompanyName = new global::System.Windows.Forms.Label();
			this.textBoxDescription = new global::System.Windows.Forms.TextBox();
			this.okButton = new global::System.Windows.Forms.Button();
			this.tableLayoutPanel.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.logoPictureBox).BeginInit();
			base.SuspendLayout();
			this.tableLayoutPanel.ColumnCount = 2;
			this.tableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 33f));
			this.tableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 67f));
			this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.labelProductName, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.labelVersion, 1, 1);
			this.tableLayoutPanel.Controls.Add(this.labelCopyright, 1, 2);
			this.tableLayoutPanel.Controls.Add(this.labelCompanyName, 1, 3);
			this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 1, 4);
			this.tableLayoutPanel.Controls.Add(this.okButton, 1, 5);
			this.tableLayoutPanel.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new global::System.Drawing.Point(9, 9);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 6;
			this.tableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 10f));
			this.tableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 10f));
			this.tableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 10f));
			this.tableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 10f));
			this.tableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.tableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 10f));
			this.tableLayoutPanel.Size = new global::System.Drawing.Size(417, 265);
			this.tableLayoutPanel.TabIndex = 0;
			this.logoPictureBox.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.logoPictureBox.Image = (global::System.Drawing.Image)resources.GetObject("logoPictureBox.Image");
			this.logoPictureBox.Location = new global::System.Drawing.Point(3, 3);
			this.logoPictureBox.Name = "logoPictureBox";
			this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 6);
			this.logoPictureBox.Size = new global::System.Drawing.Size(131, 259);
			this.logoPictureBox.SizeMode = global::System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.logoPictureBox.TabIndex = 12;
			this.logoPictureBox.TabStop = false;
			this.labelProductName.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.labelProductName.Location = new global::System.Drawing.Point(143, 0);
			this.labelProductName.Margin = new global::System.Windows.Forms.Padding(6, 0, 3, 0);
			this.labelProductName.MaximumSize = new global::System.Drawing.Size(0, 17);
			this.labelProductName.Name = "labelProductName";
			this.labelProductName.Size = new global::System.Drawing.Size(271, 17);
			this.labelProductName.TabIndex = 19;
			this.labelProductName.Text = "Product Name";
			this.labelProductName.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.labelVersion.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.labelVersion.Location = new global::System.Drawing.Point(143, 26);
			this.labelVersion.Margin = new global::System.Windows.Forms.Padding(6, 0, 3, 0);
			this.labelVersion.MaximumSize = new global::System.Drawing.Size(0, 17);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new global::System.Drawing.Size(271, 17);
			this.labelVersion.TabIndex = 0;
			this.labelVersion.Text = "Version";
			this.labelVersion.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.labelCopyright.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.labelCopyright.Location = new global::System.Drawing.Point(143, 52);
			this.labelCopyright.Margin = new global::System.Windows.Forms.Padding(6, 0, 3, 0);
			this.labelCopyright.MaximumSize = new global::System.Drawing.Size(0, 17);
			this.labelCopyright.Name = "labelCopyright";
			this.labelCopyright.Size = new global::System.Drawing.Size(271, 17);
			this.labelCopyright.TabIndex = 21;
			this.labelCopyright.Text = "Copyright";
			this.labelCopyright.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.labelCompanyName.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.labelCompanyName.Location = new global::System.Drawing.Point(143, 78);
			this.labelCompanyName.Margin = new global::System.Windows.Forms.Padding(6, 0, 3, 0);
			this.labelCompanyName.MaximumSize = new global::System.Drawing.Size(0, 17);
			this.labelCompanyName.Name = "labelCompanyName";
			this.labelCompanyName.Size = new global::System.Drawing.Size(271, 17);
			this.labelCompanyName.TabIndex = 22;
			this.labelCompanyName.Text = "Company Name";
			this.labelCompanyName.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.textBoxDescription.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.textBoxDescription.Location = new global::System.Drawing.Point(143, 107);
			this.textBoxDescription.Margin = new global::System.Windows.Forms.Padding(6, 3, 3, 3);
			this.textBoxDescription.Multiline = true;
			this.textBoxDescription.Name = "textBoxDescription";
			this.textBoxDescription.ReadOnly = true;
			this.textBoxDescription.ScrollBars = global::System.Windows.Forms.ScrollBars.Both;
			this.textBoxDescription.Size = new global::System.Drawing.Size(271, 126);
			this.textBoxDescription.TabIndex = 23;
			this.textBoxDescription.TabStop = false;
			this.textBoxDescription.Text = "Description";
			this.okButton.Anchor = (global::System.Windows.Forms.AnchorStyles.Bottom | global::System.Windows.Forms.AnchorStyles.Right);
			this.okButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.okButton.Location = new global::System.Drawing.Point(339, 239);
			this.okButton.Name = "okButton";
			this.okButton.Size = new global::System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 24;
			this.okButton.Text = "&OK";
			base.AcceptButton = this.okButton;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(435, 283);
			base.Controls.Add(this.tableLayoutPanel);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AboutBox";
			base.Padding = new global::System.Windows.Forms.Padding(9);
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "AboutBox";
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.logoPictureBox).EndInit();
			base.ResumeLayout(false);
		}

		private global::System.ComponentModel.IContainer components;

		private global::System.Windows.Forms.TableLayoutPanel tableLayoutPanel;

		private global::System.Windows.Forms.PictureBox logoPictureBox;

		private global::System.Windows.Forms.Label labelProductName;

		private global::System.Windows.Forms.Label labelVersion;

		private global::System.Windows.Forms.Label labelCopyright;

		private global::System.Windows.Forms.Label labelCompanyName;

		private global::System.Windows.Forms.TextBox textBoxDescription;

		private global::System.Windows.Forms.Button okButton;
	}
}
