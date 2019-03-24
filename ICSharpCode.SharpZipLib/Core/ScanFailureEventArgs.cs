using System;

namespace ICSharpCode.SharpZipLib.Core
{
	public class ScanFailureEventArgs : EventArgs
	{
		public ScanFailureEventArgs(string name, Exception e)
		{
			this.name_ = name;
			this.exception_ = e;
			this.continueRunning_ = true;
		}

		public string Name
		{
			get
			{
				return this.name_;
			}
		}

		public Exception Exception
		{
			get
			{
				return this.exception_;
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

		private Exception exception_;

		private bool continueRunning_;
	}
}
