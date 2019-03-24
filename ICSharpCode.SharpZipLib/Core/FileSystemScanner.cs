using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Core
{
	public class FileSystemScanner
	{
		public FileSystemScanner(string filter)
		{
			this.fileFilter_ = new PathFilter(filter);
		}

		public FileSystemScanner(string fileFilter, string directoryFilter)
		{
			this.fileFilter_ = new PathFilter(fileFilter);
			this.directoryFilter_ = new PathFilter(directoryFilter);
		}

		public FileSystemScanner(IScanFilter fileFilter)
		{
			this.fileFilter_ = fileFilter;
		}

		public FileSystemScanner(IScanFilter fileFilter, IScanFilter directoryFilter)
		{
			this.fileFilter_ = fileFilter;
			this.directoryFilter_ = directoryFilter;
		}

		private bool OnDirectoryFailure(string directory, Exception e)
		{
			DirectoryFailureHandler directoryFailure = this.DirectoryFailure;
			bool flag = directoryFailure != null;
			if (flag)
			{
				ScanFailureEventArgs scanFailureEventArgs = new ScanFailureEventArgs(directory, e);
				directoryFailure(this, scanFailureEventArgs);
				this.alive_ = scanFailureEventArgs.ContinueRunning;
			}
			return flag;
		}

		private bool OnFileFailure(string file, Exception e)
		{
			FileFailureHandler fileFailure = this.FileFailure;
			bool flag = fileFailure != null;
			if (flag)
			{
				ScanFailureEventArgs scanFailureEventArgs = new ScanFailureEventArgs(file, e);
				this.FileFailure(this, scanFailureEventArgs);
				this.alive_ = scanFailureEventArgs.ContinueRunning;
			}
			return flag;
		}

		private void OnProcessFile(string file)
		{
			ProcessFileHandler processFile = this.ProcessFile;
			if (processFile != null)
			{
				ScanEventArgs scanEventArgs = new ScanEventArgs(file);
				processFile(this, scanEventArgs);
				this.alive_ = scanEventArgs.ContinueRunning;
			}
		}

		private void OnCompleteFile(string file)
		{
			CompletedFileHandler completedFile = this.CompletedFile;
			if (completedFile != null)
			{
				ScanEventArgs scanEventArgs = new ScanEventArgs(file);
				completedFile(this, scanEventArgs);
				this.alive_ = scanEventArgs.ContinueRunning;
			}
		}

		private void OnProcessDirectory(string directory, bool hasMatchingFiles)
		{
			ProcessDirectoryHandler processDirectory = this.ProcessDirectory;
			if (processDirectory != null)
			{
				DirectoryEventArgs directoryEventArgs = new DirectoryEventArgs(directory, hasMatchingFiles);
				processDirectory(this, directoryEventArgs);
				this.alive_ = directoryEventArgs.ContinueRunning;
			}
		}

		public void Scan(string directory, bool recurse)
		{
			this.alive_ = true;
			this.ScanDir(directory, recurse);
		}

		private void ScanDir(string directory, bool recurse)
		{
			try
			{
				string[] files = Directory.GetFiles(directory);
				bool flag = false;
				for (int i = 0; i < files.Length; i++)
				{
					if (!this.fileFilter_.IsMatch(files[i]))
					{
						files[i] = null;
					}
					else
					{
						flag = true;
					}
				}
				this.OnProcessDirectory(directory, flag);
				if (this.alive_ && flag)
				{
					foreach (string text in files)
					{
						try
						{
							if (text != null)
							{
								this.OnProcessFile(text);
								if (!this.alive_)
								{
									break;
								}
							}
						}
						catch (Exception e)
						{
							if (!this.OnFileFailure(text, e))
							{
								throw;
							}
						}
					}
				}
			}
			catch (Exception e2)
			{
				if (!this.OnDirectoryFailure(directory, e2))
				{
					throw;
				}
			}
			if (this.alive_ && recurse)
			{
				try
				{
					string[] directories = Directory.GetDirectories(directory);
					foreach (string text2 in directories)
					{
						if (this.directoryFilter_ == null || this.directoryFilter_.IsMatch(text2))
						{
							this.ScanDir(text2, true);
							if (!this.alive_)
							{
								break;
							}
						}
					}
				}
				catch (Exception e3)
				{
					if (!this.OnDirectoryFailure(directory, e3))
					{
						throw;
					}
				}
			}
		}

		public ProcessDirectoryHandler ProcessDirectory;

		public ProcessFileHandler ProcessFile;

		public CompletedFileHandler CompletedFile;

		public DirectoryFailureHandler DirectoryFailure;

		public FileFailureHandler FileFailure;

		private IScanFilter fileFilter_;

		private IScanFilter directoryFilter_;

		private bool alive_;
	}
}
