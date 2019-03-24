using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Core
{
	public class PathFilter : IScanFilter
	{
		public PathFilter(string filter)
		{
			this.nameFilter_ = new NameFilter(filter);
		}

		public virtual bool IsMatch(string name)
		{
			bool result = false;
			if (name != null)
			{
				string name2 = (name.Length > 0) ? Path.GetFullPath(name) : "";
				result = this.nameFilter_.IsMatch(name2);
			}
			return result;
		}

		private NameFilter nameFilter_;
	}
}
