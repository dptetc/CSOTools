using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Nexon.CSO.ModelDecryptor
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			this.InitializeComponent();
		}

		public void AddInputItem(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			Item item = new Item(path);
			this.listBox.Items.Add(item);
		}

		private void ModelDecryptorForm_Load(object sender, EventArgs e)
		{
			this.ListBox_SelectedIndexChanged(this, EventArgs.Empty);
		}

		private void File_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Move;
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		private void TextBox_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
				if (files != null && files.Length > 0)
				{
					((TextBox)sender).Text = files[0];
				}
			}
		}

		private void ListBox_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
				if (files != null && files.Length > 0)
				{
					foreach (string file in files)
					{
						this.AddInputItem(file);
					}
				}
			}
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			try
			{
				this.AddInputItem(this.inputTextBox.Text);
			}
			catch (ArgumentException)
			{
				MessageBox.Show(this, "Input path must be a real file or directory.", "Error");
			}
		}

		private void modifyButton_Click(object sender, EventArgs e)
		{
			if (this.listBox.SelectedIndex < 0)
			{
				return;
			}
			Item item;
			try
			{
				item = new Item(this.inputTextBox.Text);
			}
			catch (ArgumentException)
			{
				MessageBox.Show("Input path must be a real file or directory.");
				return;
			}
			this.listBox.Items[this.listBox.SelectedIndex] = item;
		}

		private void removeButton_Click(object sender, EventArgs e)
		{
			if (this.listBox.SelectedIndex < 0)
			{
				return;
			}
			this.listBox.Items.RemoveAt(this.listBox.SelectedIndex);
		}

		private void exitButton_Click(object sender, EventArgs e)
		{
			if (this.backgroundWorker.IsBusy)
			{
				this.backgroundWorker.CancelAsync();
				return;
			}
			base.Close();
		}

		private void decryptButton_Click(object sender, EventArgs e)
		{
			this.listBox.SelectedItems.Clear();
			this.decryptButton.Enabled = false;
			this.inputTextBox.Enabled = false;
			this.addButton.Enabled = false;
			this.backupFiles = this.backupCheckBox.Checked;
			List<Item> items = new List<Item>(this.listBox.Items.Count);
			foreach (object obj in this.listBox.Items)
			{
				Item item = (Item)obj;
				items.Add(item);
			}
			this.backgroundWorker.RunWorkerAsync(items);
		}

		private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool editEnabled = this.listBox.SelectedIndex >= 0;
			this.modifyButton.Enabled = editEnabled;
			this.removeButton.Enabled = editEnabled;
			if (editEnabled)
			{
				Item item = this.listBox.SelectedItem as Item;
				this.inputTextBox.Text = item.InputPath;
			}
		}

		private void TextBox_Enter(object sender, EventArgs e)
		{
			base.AcceptButton = this.addButton;
		}

		private void TextBox_Leave(object sender, EventArgs e)
		{
			base.AcceptButton = null;
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			List<Item> items = e.Argument as List<Item>;
			if (items == null)
			{
				return;
			}
			foreach (Item item in items)
			{
				if (this.backgroundWorker.CancellationPending)
				{
					e.Cancel = true;
					break;
				}
				item.Decrypt(this.backupFiles);
			}
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (this.exitOnFinish)
			{
				base.Close();
			}
			this.decryptButton.Enabled = true;
			this.inputTextBox.Enabled = true;
			this.addButton.Enabled = true;
		}

		private bool exitOnFinish;

		private bool backupFiles;
	}
}
