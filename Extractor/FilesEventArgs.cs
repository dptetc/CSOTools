using System;
using System.Collections.Generic;

namespace Nexon.CSO.Extractor
{
	public class FilesEventArgs : EventArgs
	{
		public FilesEventArgs() : this(string.Empty, null)
		{
		}

		public FilesEventArgs(string path, IList<NexonArchiveFileEntry> files)
		{
			this.path = path;
			this.files = files;
		}

		public string Path
		{
			get
			{
				return this.path;
			}
		}

		public IList<NexonArchiveFileEntry> Files
		{
			get
			{
				return this.files;
			}
		}

		private string path;

		private IList<NexonArchiveFileEntry> files;
	}
}
