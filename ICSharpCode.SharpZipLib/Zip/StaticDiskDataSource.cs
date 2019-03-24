using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	public class StaticDiskDataSource : IStaticDataSource
	{
		public StaticDiskDataSource(string fileName)
		{
			this.fileName_ = fileName;
		}

		public Stream GetSource()
		{
			return File.OpenRead(this.fileName_);
		}

		private string fileName_;
	}
}
