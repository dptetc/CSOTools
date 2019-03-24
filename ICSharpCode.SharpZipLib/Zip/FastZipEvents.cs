using System;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	public class FastZipEvents
	{
		public bool OnDirectoryFailure(string directory, Exception e)
		{
			bool result = false;
			DirectoryFailureHandler directoryFailure = this.DirectoryFailure;
			if (directoryFailure != null)
			{
				ScanFailureEventArgs scanFailureEventArgs = new ScanFailureEventArgs(directory, e);
				directoryFailure(this, scanFailureEventArgs);
				result = scanFailureEventArgs.ContinueRunning;
			}
			return result;
		}

		public bool OnFileFailure(string file, Exception e)
		{
			FileFailureHandler fileFailure = this.FileFailure;
			bool flag = fileFailure != null;
			if (flag)
			{
				ScanFailureEventArgs scanFailureEventArgs = new ScanFailureEventArgs(file, e);
				fileFailure(this, scanFailureEventArgs);
				flag = scanFailureEventArgs.ContinueRunning;
			}
			return flag;
		}

		public bool OnProcessFile(string file)
		{
			bool result = true;
			ProcessFileHandler processFile = this.ProcessFile;
			if (processFile != null)
			{
				ScanEventArgs scanEventArgs = new ScanEventArgs(file);
				this.ProcessFile(this, scanEventArgs);
				result = scanEventArgs.ContinueRunning;
			}
			return result;
		}

		public bool OnCompletedFile(string file)
		{
			bool result = true;
			CompletedFileHandler completedFile = this.CompletedFile;
			if (completedFile != null)
			{
				ScanEventArgs scanEventArgs = new ScanEventArgs(file);
				completedFile(this, scanEventArgs);
				result = scanEventArgs.ContinueRunning;
			}
			return result;
		}

		public bool OnProcessDirectory(string directory, bool hasMatchingFiles)
		{
			bool result = true;
			ProcessDirectoryHandler processDirectory = this.ProcessDirectory;
			if (processDirectory != null)
			{
				DirectoryEventArgs directoryEventArgs = new DirectoryEventArgs(directory, hasMatchingFiles);
				processDirectory(this, directoryEventArgs);
				result = directoryEventArgs.ContinueRunning;
			}
			return result;
		}

		public TimeSpan ProgressInterval
		{
			get
			{
				return this.progressInterval_;
			}
			set
			{
				this.progressInterval_ = value;
			}
		}

		public ProcessDirectoryHandler ProcessDirectory;

		public ProcessFileHandler ProcessFile;

		public ProgressHandler Progress;

		public CompletedFileHandler CompletedFile;

		public DirectoryFailureHandler DirectoryFailure;

		public FileFailureHandler FileFailure;

		private TimeSpan progressInterval_ = TimeSpan.FromSeconds(3.0);
	}
}
