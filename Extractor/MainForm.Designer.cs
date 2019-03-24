namespace Nexon.CSO.Extractor
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
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::Nexon.CSO.Extractor.MainForm));
			this.menuStrip = new global::System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new global::System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new global::System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.undoToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.redoToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new global::System.Windows.Forms.ToolStripSeparator();
			this.cutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new global::System.Windows.Forms.ToolStripSeparator();
			this.selectAllToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new global::System.Windows.Forms.ToolStripSeparator();
			this.extractAllToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.verifyAllToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.autoDecryptModelsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip = new global::System.Windows.Forms.StatusStrip();
			this.pathToolStripStatusLabel = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.fileToolStripStatusLabel = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.splitContainer1 = new global::System.Windows.Forms.SplitContainer();
			this.treeView = new global::Nexon.CSO.Extractor.FolderTreeView();
			this.listView = new global::Nexon.CSO.Extractor.FilesListView();
			this.narOpenFileDialog = new global::System.Windows.Forms.OpenFileDialog();
			this.extractFolderBrowserDialog = new global::System.Windows.Forms.FolderBrowserDialog();
			this.menuStrip.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			base.SuspendLayout();
			this.menuStrip.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.fileToolStripMenuItem,
				this.editToolStripMenuItem,
				this.toolsToolStripMenuItem,
				this.helpToolStripMenuItem
			});
			this.menuStrip.Location = new global::System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new global::System.Drawing.Size(624, 24);
			this.menuStrip.TabIndex = 0;
			this.menuStrip.Text = "menuStrip1";
			this.fileToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.newToolStripMenuItem,
				this.openToolStripMenuItem,
				this.toolStripSeparator,
				this.saveToolStripMenuItem,
				this.saveAsToolStripMenuItem,
				this.toolStripSeparator1,
				this.exitToolStripMenuItem
			});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new global::System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			this.newToolStripMenuItem.Enabled = false;
			this.newToolStripMenuItem.Image = (global::System.Drawing.Image)resources.GetObject("newToolStripMenuItem.Image");
			this.newToolStripMenuItem.ImageTransparentColor = global::System.Drawing.Color.Magenta;
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.ShortcutKeys = (global::System.Windows.Forms.Keys)131150;
			this.newToolStripMenuItem.Size = new global::System.Drawing.Size(146, 22);
			this.newToolStripMenuItem.Text = "&New";
			this.openToolStripMenuItem.Image = (global::System.Drawing.Image)resources.GetObject("openToolStripMenuItem.Image");
			this.openToolStripMenuItem.ImageTransparentColor = global::System.Drawing.Color.Magenta;
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = (global::System.Windows.Forms.Keys)131151;
			this.openToolStripMenuItem.Size = new global::System.Drawing.Size(146, 22);
			this.openToolStripMenuItem.Text = "&Open";
			this.openToolStripMenuItem.Click += new global::System.EventHandler(this.openToolStripMenuItem_Click);
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new global::System.Drawing.Size(143, 6);
			this.saveToolStripMenuItem.Enabled = false;
			this.saveToolStripMenuItem.Image = (global::System.Drawing.Image)resources.GetObject("saveToolStripMenuItem.Image");
			this.saveToolStripMenuItem.ImageTransparentColor = global::System.Drawing.Color.Magenta;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = (global::System.Windows.Forms.Keys)131155;
			this.saveToolStripMenuItem.Size = new global::System.Drawing.Size(146, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			this.saveAsToolStripMenuItem.Enabled = false;
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new global::System.Drawing.Size(146, 22);
			this.saveAsToolStripMenuItem.Text = "Save &As";
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new global::System.Drawing.Size(143, 6);
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new global::System.Drawing.Size(146, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new global::System.EventHandler(this.exitToolStripMenuItem_Click);
			this.editToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.undoToolStripMenuItem,
				this.redoToolStripMenuItem,
				this.toolStripSeparator3,
				this.cutToolStripMenuItem,
				this.copyToolStripMenuItem,
				this.pasteToolStripMenuItem,
				this.toolStripSeparator4,
				this.selectAllToolStripMenuItem,
				this.toolStripMenuItem1,
				this.extractAllToolStripMenuItem,
				this.verifyAllToolStripMenuItem
			});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new global::System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "&Edit";
			this.undoToolStripMenuItem.Enabled = false;
			this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
			this.undoToolStripMenuItem.ShortcutKeys = (global::System.Windows.Forms.Keys)131162;
			this.undoToolStripMenuItem.Size = new global::System.Drawing.Size(164, 22);
			this.undoToolStripMenuItem.Text = "&Undo";
			this.redoToolStripMenuItem.Enabled = false;
			this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
			this.redoToolStripMenuItem.ShortcutKeys = (global::System.Windows.Forms.Keys)131161;
			this.redoToolStripMenuItem.Size = new global::System.Drawing.Size(164, 22);
			this.redoToolStripMenuItem.Text = "&Redo";
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new global::System.Drawing.Size(161, 6);
			this.cutToolStripMenuItem.Enabled = false;
			this.cutToolStripMenuItem.Image = (global::System.Drawing.Image)resources.GetObject("cutToolStripMenuItem.Image");
			this.cutToolStripMenuItem.ImageTransparentColor = global::System.Drawing.Color.Magenta;
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.ShortcutKeys = (global::System.Windows.Forms.Keys)131160;
			this.cutToolStripMenuItem.Size = new global::System.Drawing.Size(164, 22);
			this.cutToolStripMenuItem.Text = "Cu&t";
			this.copyToolStripMenuItem.Enabled = false;
			this.copyToolStripMenuItem.Image = (global::System.Drawing.Image)resources.GetObject("copyToolStripMenuItem.Image");
			this.copyToolStripMenuItem.ImageTransparentColor = global::System.Drawing.Color.Magenta;
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.ShortcutKeys = (global::System.Windows.Forms.Keys)131139;
			this.copyToolStripMenuItem.Size = new global::System.Drawing.Size(164, 22);
			this.copyToolStripMenuItem.Text = "&Copy";
			this.pasteToolStripMenuItem.Enabled = false;
			this.pasteToolStripMenuItem.Image = (global::System.Drawing.Image)resources.GetObject("pasteToolStripMenuItem.Image");
			this.pasteToolStripMenuItem.ImageTransparentColor = global::System.Drawing.Color.Magenta;
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.ShortcutKeys = (global::System.Windows.Forms.Keys)131158;
			this.pasteToolStripMenuItem.Size = new global::System.Drawing.Size(164, 22);
			this.pasteToolStripMenuItem.Text = "&Paste";
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new global::System.Drawing.Size(161, 6);
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			this.selectAllToolStripMenuItem.ShortcutKeys = (global::System.Windows.Forms.Keys)131137;
			this.selectAllToolStripMenuItem.Size = new global::System.Drawing.Size(164, 22);
			this.selectAllToolStripMenuItem.Text = "Select &All";
			this.selectAllToolStripMenuItem.Click += new global::System.EventHandler(this.selectAllToolStripMenuItem_Click);
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new global::System.Drawing.Size(161, 6);
			this.extractAllToolStripMenuItem.Name = "extractAllToolStripMenuItem";
			this.extractAllToolStripMenuItem.Size = new global::System.Drawing.Size(164, 22);
			this.extractAllToolStripMenuItem.Text = "E&xtract All...";
			this.extractAllToolStripMenuItem.Click += new global::System.EventHandler(this.extractAllToolStripMenuItem_Click);
			this.verifyAllToolStripMenuItem.Name = "verifyAllToolStripMenuItem";
			this.verifyAllToolStripMenuItem.Size = new global::System.Drawing.Size(164, 22);
			this.verifyAllToolStripMenuItem.Text = "&Verify All";
			this.verifyAllToolStripMenuItem.Click += new global::System.EventHandler(this.verifyAllToolStripMenuItem_Click);
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.autoDecryptModelsToolStripMenuItem
			});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new global::System.Drawing.Size(48, 20);
			this.toolsToolStripMenuItem.Text = "&Tools";
			this.autoDecryptModelsToolStripMenuItem.CheckOnClick = true;
			this.autoDecryptModelsToolStripMenuItem.Name = "autoDecryptModelsToolStripMenuItem";
			this.autoDecryptModelsToolStripMenuItem.Size = new global::System.Drawing.Size(234, 22);
			this.autoDecryptModelsToolStripMenuItem.Text = "Automatically Decrypt Models";
			this.autoDecryptModelsToolStripMenuItem.Click += new global::System.EventHandler(this.autoDecryptModelsToolStripMenuItem_Click);
			this.helpToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.aboutToolStripMenuItem
			});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new global::System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "&Help";
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new global::System.Drawing.Size(116, 22);
			this.aboutToolStripMenuItem.Text = "&About...";
			this.aboutToolStripMenuItem.Click += new global::System.EventHandler(this.aboutToolStripMenuItem_Click);
			this.statusStrip.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.pathToolStripStatusLabel,
				this.fileToolStripStatusLabel
			});
			this.statusStrip.Location = new global::System.Drawing.Point(0, 422);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.RenderMode = global::System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
			this.statusStrip.Size = new global::System.Drawing.Size(624, 22);
			this.statusStrip.TabIndex = 2;
			this.pathToolStripStatusLabel.Name = "pathToolStripStatusLabel";
			this.pathToolStripStatusLabel.Size = new global::System.Drawing.Size(478, 17);
			this.pathToolStripStatusLabel.Spring = true;
			this.pathToolStripStatusLabel.Text = "pathToolStripStatusLabel";
			this.pathToolStripStatusLabel.TextAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.fileToolStripStatusLabel.Name = "fileToolStripStatusLabel";
			this.fileToolStripStatusLabel.Size = new global::System.Drawing.Size(131, 17);
			this.fileToolStripStatusLabel.Text = "fileToolStripStatusLabel";
			this.fileToolStripStatusLabel.TextAlign = global::System.Drawing.ContentAlignment.MiddleRight;
			this.splitContainer1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new global::System.Drawing.Point(0, 24);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.treeView);
			this.splitContainer1.Panel2.Controls.Add(this.listView);
			this.splitContainer1.Size = new global::System.Drawing.Size(624, 398);
			this.splitContainer1.SplitterDistance = 208;
			this.splitContainer1.TabIndex = 1;
			this.splitContainer1.TabStop = false;
			this.treeView.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.treeView.HideSelection = false;
			this.treeView.Location = new global::System.Drawing.Point(0, 0);
			this.treeView.Name = "treeView";
			this.treeView.Size = new global::System.Drawing.Size(208, 398);
			this.treeView.Sorted = true;
			this.treeView.TabIndex = 0;
			this.treeView.ExtractFolder += new global::System.EventHandler<global::Nexon.CSO.Extractor.FilesEventArgs>(this.ExtractFiles);
			this.treeView.ShowFolder += new global::System.EventHandler<global::Nexon.CSO.Extractor.FilesEventArgs>(this.treeView_ShowFolder);
			this.treeView.VerifyFolder += new global::System.EventHandler<global::Nexon.CSO.Extractor.FilesEventArgs>(this.VerifyFiles);
			this.listView.AllowColumnReorder = true;
			this.listView.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.listView.FullPath = null;
			this.listView.FullRowSelect = true;
			this.listView.HideSelection = false;
			this.listView.Location = new global::System.Drawing.Point(0, 0);
			this.listView.Name = "listView";
			this.listView.Size = new global::System.Drawing.Size(412, 398);
			this.listView.SortColumn = -1;
			this.listView.SortColumnOrder = global::System.Windows.Forms.SortOrder.None;
			this.listView.TabIndex = 0;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = global::System.Windows.Forms.View.Details;
			this.listView.VerifyFiles += new global::System.EventHandler<global::Nexon.CSO.Extractor.FilesEventArgs>(this.VerifyFiles);
			this.listView.SelectedIndexChanged += new global::System.EventHandler(this.listView_SelectedIndexChanged);
			this.listView.ExtractFiles += new global::System.EventHandler<global::Nexon.CSO.Extractor.FilesEventArgs>(this.ExtractFiles);
			this.narOpenFileDialog.DefaultExt = "nar";
			this.narOpenFileDialog.Filter = "NAR files|*.nar|All files|*.*";
			this.extractFolderBrowserDialog.Description = "Select folder to extract files to. Note that the archive's folder structure will be replicated in whichever folder you select.";
			this.AllowDrop = true;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(624, 444);
			base.Controls.Add(this.splitContainer1);
			base.Controls.Add(this.statusStrip);
			base.Controls.Add(this.menuStrip);
			base.MainMenuStrip = this.menuStrip;
			base.Name = "NARExtractorForm";
			this.Text = "NAR Extractor";
			base.Load += new global::System.EventHandler(this.NARExtractorForm_Load);
			base.Shown += new global::System.EventHandler(this.NARExtractorForm_Shown);
			base.DragDrop += new global::System.Windows.Forms.DragEventHandler(this.NARExtractorForm_DragDrop);
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.NARExtractorForm_FormClosed);
			base.DragEnter += new global::System.Windows.Forms.DragEventHandler(this.NARExtractorForm_DragEnter);
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.NARExtractorForm_FormClosing);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private global::System.ComponentModel.IContainer components;

		private global::System.Windows.Forms.MenuStrip menuStrip;

		private global::System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;

		private global::System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;

		private global::System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;

		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator;

		private global::System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;

		private global::System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;

		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

		private global::System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;

		private global::System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;

		private global::System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;

		private global::System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;

		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator3;

		private global::System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;

		private global::System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;

		private global::System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;

		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator4;

		private global::System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;

		private global::System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;

		private global::System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;

		private global::System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;

		private global::System.Windows.Forms.StatusStrip statusStrip;

		private global::System.Windows.Forms.SplitContainer splitContainer1;

		private global::Nexon.CSO.Extractor.FolderTreeView treeView;

		private global::Nexon.CSO.Extractor.FilesListView listView;

		private global::System.Windows.Forms.OpenFileDialog narOpenFileDialog;

		private global::System.Windows.Forms.ToolStripStatusLabel pathToolStripStatusLabel;

		private global::System.Windows.Forms.ToolStripStatusLabel fileToolStripStatusLabel;

		private global::System.Windows.Forms.FolderBrowserDialog extractFolderBrowserDialog;

		private global::System.Windows.Forms.ToolStripMenuItem autoDecryptModelsToolStripMenuItem;

		private global::System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;

		private global::System.Windows.Forms.ToolStripMenuItem extractAllToolStripMenuItem;

		private global::System.Windows.Forms.ToolStripMenuItem verifyAllToolStripMenuItem;
	}
}
