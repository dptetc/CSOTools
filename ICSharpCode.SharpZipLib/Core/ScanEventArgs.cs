using System;

namespace ICSharpCode.SharpZipLib.Core
{
	public class ScanEventArgs : EventArgs
	{
		public ScanEventArgs(string name)
		{
			this.name_ = name;
		}

		public string Name
		{
			get
			{
				return this.name_;
			}
		}

		public bool ContinueRunning
		{
			get
			{
				return this.continueRunning_;
			}
			set
			{
				this.continueRunning_ = value;
			}
		}

		private string name_;

		private bool continueRunning_ = true;
	}
}
