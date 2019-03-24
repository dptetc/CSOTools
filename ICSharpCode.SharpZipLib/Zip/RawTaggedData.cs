using System;

namespace ICSharpCode.SharpZipLib.Zip
{
	public class RawTaggedData : ITaggedData
	{
		public RawTaggedData(short tag)
		{
			this.tag_ = tag;
		}

		public short TagID
		{
			get
			{
				return this.tag_;
			}
			set
			{
				this.tag_ = value;
			}
		}

		public void SetData(byte[] data, int offset, int count)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this.data_ = new byte[count];
			Array.Copy(data, offset, this.data_, 0, count);
		}

		public byte[] GetData()
		{
			return this.data_;
		}

		public byte[] Data
		{
			get
			{
				return this.data_;
			}
			set
			{
				this.data_ = value;
			}
		}

		protected short tag_;

		private byte[] data_;
	}
}
