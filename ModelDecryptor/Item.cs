using System;
using System.IO;

namespace Nexon.CSO.ModelDecryptor
{
	internal sealed class Item
	{
		public Item(string inputPath)
		{
			if (string.IsNullOrEmpty(inputPath))
			{
				inputPath = Environment.CurrentDirectory;
			}
			if (!Directory.Exists(inputPath) && !File.Exists(inputPath))
			{
				throw new ArgumentException("Input path must be an existing directory or file.", "inputPath");
			}
			this.InputPath = inputPath;
		}

		public string InputPath
		{
			get
			{
				return this.inputPath;
			}
			set
			{
				if (!Directory.Exists(value) && !File.Exists(value))
				{
					throw new ArgumentException("Input path must be an existing directory or file.", "value");
				}
				this.inputPath = value;
			}
		}

		public void Decrypt(bool backup)
		{
			DirectoryInfo directory = new DirectoryInfo(this.InputPath);
			if (directory.Exists)
			{
				this.Decrypt(backup, directory);
				return;
			}
			FileInfo file = new FileInfo(this.InputPath);
			if (file.Exists)
			{
				this.Decrypt(backup, file);
			}
		}

		private void Decrypt(bool backup, DirectoryInfo directory)
		{
			foreach (FileInfo file in directory.GetFiles("*.mdl", SearchOption.AllDirectories))
			{
				this.Decrypt(backup, file);
			}
		}

		private void Decrypt(bool backup, FileInfo file)
		{
			FileInfo tempFile = null;
			do
			{
				tempFile = new FileInfo(Path.Combine(Path.GetDirectoryName(file.DirectoryName), Path.ChangeExtension(Path.GetTempFileName(), ".mdl")));
			}
			while (tempFile.Exists);
			file.CopyTo(tempFile.FullName);
			tempFile.Attributes = (FileAttributes.Hidden | FileAttributes.Temporary);
			try
			{
				if (ModelHelper.DecryptModel(tempFile.FullName) == ModelResult.Success)
				{
					if (backup)
					{
						FileInfo backupFile = new FileInfo(this.GetBackupFileName(file.FullName));
						file.CopyTo(backupFile.FullName);
					}
					tempFile.CopyTo(file.FullName, true);
				}
			}
			finally
			{
				tempFile.Refresh();
				if (tempFile.Exists)
				{
					tempFile.Delete();
				}
			}
		}

		private string GetBackupFileName(string fileName)
		{
			int backupFileIndex = 0;
			FileInfo backupFile;
			do
			{
				string bakExtension = ".bak" + ((backupFileIndex == 0) ? string.Empty : backupFileIndex.ToString());
				backupFile = new FileInfo(Path.ChangeExtension(fileName, Path.GetExtension(fileName) + bakExtension));
				backupFileIndex++;
			}
			while (backupFile.Exists);
			return backupFile.FullName;
		}

		public override string ToString()
		{
			return this.InputPath;
		}

		private string inputPath;
	}
}
