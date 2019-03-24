using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	public class KeysRequiredEventArgs : EventArgs
	{
		public KeysRequiredEventArgs(string name)
		{
			this.fileName = name;
		}

		public KeysRequiredEventArgs(string name, byte[] keyValue)
		{
			this.fileName = name;
			this.key = keyValue;
		}

		public string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		public byte[] Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
			}
		}

		private string fileName;

		private byte[] key;
	}
}
