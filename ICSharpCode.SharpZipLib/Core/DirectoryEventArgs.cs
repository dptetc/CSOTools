using System;

namespace ICSharpCode.SharpZipLib.Core
{
	public class DirectoryEventArgs : ScanEventArgs
	{
		public DirectoryEventArgs(string name, bool hasMatchingFiles) : base(name)
		{
			this.hasMatchingFiles_ = hasMatchingFiles;
		}

		public bool HasMatchingFiles
		{
			get
			{
				return this.hasMatchingFiles_;
			}
		}

		private bool hasMatchingFiles_;
	}
}
