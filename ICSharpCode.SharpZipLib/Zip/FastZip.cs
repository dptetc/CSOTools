using System;
using System.Collections;
using System.IO;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	public class FastZip
	{
		public FastZip()
		{
		}

		public FastZip(FastZipEvents events)
		{
			this.events_ = events;
		}

		public bool CreateEmptyDirectories
		{
			get
			{
				return this.createEmptyDirectories_;
			}
			set
			{
				this.createEmptyDirectories_ = value;
			}
		}

		public string Password
		{
			get
			{
				return this.password_;
			}
			set
			{
				this.password_ = value;
			}
		}

		public INameTransform NameTransform
		{
			get
			{
				return this.entryFactory_.NameTransform;
			}
			set
			{
				this.entryFactory_.NameTransform = value;
			}
		}

		public IEntryFactory EntryFactory
		{
			get
			{
				return this.entryFactory_;
			}
			set
			{
				if (value == null)
				{
					this.entryFactory_ = new ZipEntryFactory();
					return;
				}
				this.entryFactory_ = value;
			}
		}

		public UseZip64 UseZip64
		{
			get
			{
				return this.useZip64_;
			}
			set
			{
				this.useZip64_ = value;
			}
		}

		public bool RestoreDateTimeOnExtract
		{
			get
			{
				return this.restoreDateTimeOnExtract_;
			}
			set
			{
				this.restoreDateTimeOnExtract_ = value;
			}
		}

		public bool RestoreAttributesOnExtract
		{
			get
			{
				return this.restoreAttributesOnExtract_;
			}
			set
			{
				this.restoreAttributesOnExtract_ = value;
			}
		}

		public void CreateZip(string zipFileName, string sourceDirectory, bool recurse, string fileFilter, string directoryFilter)
		{
			this.CreateZip(File.Create(zipFileName), sourceDirectory, recurse, fileFilter, directoryFilter);
		}

		public void CreateZip(string zipFileName, string sourceDirectory, bool recurse, string fileFilter)
		{
			this.CreateZip(File.Create(zipFileName), sourceDirectory, recurse, fileFilter, null);
		}

		public void CreateZip(Stream outputStream, string sourceDirectory, bool recurse, string fileFilter, string directoryFilter)
		{
			this.NameTransform = new ZipNameTransform(sourceDirectory);
			this.sourceDirectory_ = sourceDirectory;
			using (this.outputStream_ = new ZipOutputStream(outputStream))
			{
				if (this.password_ != null)
				{
					this.outputStream_.Password = this.password_;
				}
				this.outputStream_.UseZip64 = this.UseZip64;
				FileSystemScanner fileSystemScanner = new FileSystemScanner(fileFilter, directoryFilter);
				FileSystemScanner fileSystemScanner2 = fileSystemScanner;
				fileSystemScanner2.ProcessFile = (ProcessFileHandler)Delegate.Combine(fileSystemScanner2.ProcessFile, new ProcessFileHandler(this.ProcessFile));
				if (this.CreateEmptyDirectories)
				{
					FileSystemScanner fileSystemScanner3 = fileSystemScanner;
					fileSystemScanner3.ProcessDirectory = (ProcessDirectoryHandler)Delegate.Combine(fileSystemScanner3.ProcessDirectory, new ProcessDirectoryHandler(this.ProcessDirectory));
				}
				if (this.events_ != null)
				{
					if (this.events_.FileFailure != null)
					{
						FileSystemScanner fileSystemScanner4 = fileSystemScanner;
						fileSystemScanner4.FileFailure = (FileFailureHandler)Delegate.Combine(fileSystemScanner4.FileFailure, this.events_.FileFailure);
					}
					if (this.events_.DirectoryFailure != null)
					{
						FileSystemScanner fileSystemScanner5 = fileSystemScanner;
						fileSystemScanner5.DirectoryFailure = (DirectoryFailureHandler)Delegate.Combine(fileSystemScanner5.DirectoryFailure, this.events_.DirectoryFailure);
					}
				}
				fileSystemScanner.Scan(sourceDirectory, recurse);
			}
		}

		public void ExtractZip(string zipFileName, string targetDirectory, string fileFilter)
		{
			this.ExtractZip(zipFileName, targetDirectory, FastZip.Overwrite.Always, null, fileFilter, null, this.restoreDateTimeOnExtract_);
		}

		public void ExtractZip(string zipFileName, string targetDirectory, FastZip.Overwrite overwrite, FastZip.ConfirmOverwriteDelegate confirmDelegate, string fileFilter, string directoryFilter, bool restoreDateTime)
		{
			if (overwrite == FastZip.Overwrite.Prompt && confirmDelegate == null)
			{
				throw new ArgumentNullException("confirmDelegate");
			}
			this.continueRunning_ = true;
			this.overwrite_ = overwrite;
			this.confirmDelegate_ = confirmDelegate;
			this.extractNameTransform_ = new WindowsNameTransform(targetDirectory);
			this.fileFilter_ = new NameFilter(fileFilter);
			this.directoryFilter_ = new NameFilter(directoryFilter);
			this.restoreDateTimeOnExtract_ = restoreDateTime;
			using (this.zipFile_ = new ZipFile(zipFileName))
			{
				if (this.password_ != null)
				{
					this.zipFile_.Password = this.password_;
				}
				IEnumerator enumerator = this.zipFile_.GetEnumerator();
				while (this.continueRunning_ && enumerator.MoveNext())
				{
					ZipEntry zipEntry = (ZipEntry)enumerator.Current;
					if (zipEntry.IsFile)
					{
						if (this.directoryFilter_.IsMatch(Path.GetDirectoryName(zipEntry.Name)) && this.fileFilter_.IsMatch(zipEntry.Name))
						{
							this.ExtractEntry(zipEntry);
						}
					}
					else if (zipEntry.IsDirectory && this.directoryFilter_.IsMatch(zipEntry.Name) && this.CreateEmptyDirectories)
					{
						this.ExtractEntry(zipEntry);
					}
				}
			}
		}

		private void ProcessDirectory(object sender, DirectoryEventArgs e)
		{
			if (!e.HasMatchingFiles && this.CreateEmptyDirectories)
			{
				if (this.events_ != null)
				{
					this.events_.OnProcessDirectory(e.Name, e.HasMatchingFiles);
				}
				if (e.ContinueRunning && e.Name != this.sourceDirectory_)
				{
					ZipEntry entry = this.entryFactory_.MakeDirectoryEntry(e.Name);
					this.outputStream_.PutNextEntry(entry);
				}
			}
		}

		private void ProcessFile(object sender, ScanEventArgs e)
		{
			if (this.events_ != null && this.events_.ProcessFile != null)
			{
				this.events_.ProcessFile(sender, e);
			}
			if (e.ContinueRunning)
			{
				using (FileStream fileStream = File.OpenRead(e.Name))
				{
					ZipEntry entry = this.entryFactory_.MakeFileEntry(e.Name);
					this.outputStream_.PutNextEntry(entry);
					this.AddFileContents(e.Name, fileStream);
				}
			}
		}

		private void AddFileContents(string name, Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (this.buffer_ == null)
			{
				this.buffer_ = new byte[4096];
			}
			if (this.events_ != null && this.events_.Progress != null)
			{
				StreamUtils.Copy(stream, this.outputStream_, this.buffer_, this.events_.Progress, this.events_.ProgressInterval, this, name);
			}
			else
			{
				StreamUtils.Copy(stream, this.outputStream_, this.buffer_);
			}
			if (this.events_ != null)
			{
				this.continueRunning_ = this.events_.OnCompletedFile(name);
			}
		}

		private void ExtractFileEntry(ZipEntry entry, string targetName)
		{
			bool flag = true;
			if (this.overwrite_ != FastZip.Overwrite.Always && File.Exists(targetName))
			{
				flag = (this.overwrite_ == FastZip.Overwrite.Prompt && this.confirmDelegate_ != null && this.confirmDelegate_(targetName));
			}
			if (flag)
			{
				if (this.events_ != null)
				{
					this.continueRunning_ = this.events_.OnProcessFile(entry.Name);
				}
				if (this.continueRunning_)
				{
					try
					{
						using (FileStream fileStream = File.Create(targetName))
						{
							if (this.buffer_ == null)
							{
								this.buffer_ = new byte[4096];
							}
							if (this.events_ != null && this.events_.Progress != null)
							{
								StreamUtils.Copy(this.zipFile_.GetInputStream(entry), fileStream, this.buffer_, this.events_.Progress, this.events_.ProgressInterval, this, entry.Name, entry.Size);
							}
							else
							{
								StreamUtils.Copy(this.zipFile_.GetInputStream(entry), fileStream, this.buffer_);
							}
							if (this.events_ != null)
							{
								this.continueRunning_ = this.events_.OnCompletedFile(entry.Name);
							}
						}
						if (this.restoreDateTimeOnExtract_)
						{
							File.SetLastWriteTime(targetName, entry.DateTime);
						}
						if (this.RestoreAttributesOnExtract && entry.IsDOSEntry && entry.ExternalFileAttributes != -1)
						{
							FileAttributes fileAttributes = (FileAttributes)entry.ExternalFileAttributes;
							fileAttributes &= (FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.Archive | FileAttributes.Normal);
							File.SetAttributes(targetName, fileAttributes);
						}
					}
					catch (Exception e)
					{
						if (this.events_ == null)
						{
							this.continueRunning_ = false;
							throw;
						}
						this.continueRunning_ = this.events_.OnFileFailure(targetName, e);
					}
				}
			}
		}

		private void ExtractEntry(ZipEntry entry)
		{
			bool flag = entry.IsCompressionMethodSupported();
			string text = entry.Name;
			if (flag)
			{
				if (entry.IsFile)
				{
					text = this.extractNameTransform_.TransformFile(text);
				}
				else if (entry.IsDirectory)
				{
					text = this.extractNameTransform_.TransformDirectory(text);
				}
				flag = (text != null && text.Length != 0);
			}
			string path = null;
			if (flag)
			{
				if (entry.IsDirectory)
				{
					path = text;
				}
				else
				{
					path = Path.GetDirectoryName(Path.GetFullPath(text));
				}
			}
			if (flag && !Directory.Exists(path))
			{
				if (entry.IsDirectory)
				{
					if (!this.CreateEmptyDirectories)
					{
						goto IL_D9;
					}
				}
				try
				{
					Directory.CreateDirectory(path);
				}
				catch (Exception e)
				{
					flag = false;
					if (this.events_ == null)
					{
						this.continueRunning_ = false;
						throw;
					}
					if (entry.IsDirectory)
					{
						this.continueRunning_ = this.events_.OnDirectoryFailure(text, e);
					}
					else
					{
						this.continueRunning_ = this.events_.OnFileFailure(text, e);
					}
				}
			}
			IL_D9:
			if (flag && entry.IsFile)
			{
				this.ExtractFileEntry(entry, text);
			}
		}

		private static int MakeExternalAttributes(FileInfo info)
		{
			return (int)info.Attributes;
		}

		private static bool NameIsValid(string name)
		{
			return name != null && name.Length > 0 && name.IndexOfAny(Path.GetInvalidPathChars()) < 0;
		}

		private bool continueRunning_;

		private byte[] buffer_;

		private ZipOutputStream outputStream_;

		private ZipFile zipFile_;

		private string sourceDirectory_;

		private NameFilter fileFilter_;

		private NameFilter directoryFilter_;

		private FastZip.Overwrite overwrite_;

		private FastZip.ConfirmOverwriteDelegate confirmDelegate_;

		private bool restoreDateTimeOnExtract_;

		private bool restoreAttributesOnExtract_;

		private bool createEmptyDirectories_;

		private FastZipEvents events_;

		private IEntryFactory entryFactory_ = new ZipEntryFactory();

		private INameTransform extractNameTransform_;

		private UseZip64 useZip64_ = UseZip64.Dynamic;

		private string password_;

		public enum Overwrite
		{
			Prompt,
			Never,
			Always
		}

		public delegate bool ConfirmOverwriteDelegate(string fileName);
	}
}
