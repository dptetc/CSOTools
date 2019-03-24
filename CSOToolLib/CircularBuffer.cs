using System;

namespace Nexon.CSO
{
	internal sealed class CircularBuffer
	{
		public CircularBuffer(int length)
		{
			this.data = new byte[length];
		}

		public int Length
		{
			get
			{
				return this.length;
			}
		}

		public void Append(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count == 0)
			{
				return;
			}
			if (count >= this.data.Length)
			{
				Buffer.BlockCopy(buffer, offset + (count - this.data.Length), this.data, 0, this.data.Length);
				this.source = 0;
				this.length = this.data.Length;
				return;
			}
			if (this.source == this.data.Length)
			{
				this.source = 0;
			}
			int initialCopyLength = Math.Min(this.data.Length - this.source, count);
			Buffer.BlockCopy(buffer, offset, this.data, this.source, initialCopyLength);
			if (count > initialCopyLength)
			{
				Buffer.BlockCopy(buffer, offset + initialCopyLength, this.data, 0, count - initialCopyLength);
			}
			this.source = (this.source + count) % this.data.Length;
			this.length = Math.Min(this.length + count, this.data.Length);
		}

		public void Copy(int distance, byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || offset + count > buffer.Length || count > this.length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (distance <= 0 || distance > this.length)
			{
				throw new ArgumentOutOfRangeException("distance");
			}
			if (count == 0)
			{
				return;
			}
			int copySource = this.source - distance;
			if (copySource < 0)
			{
				copySource = this.data.Length + copySource;
			}
			int copyLength = this.data.Length - copySource;
			int actualCopyLength = Math.Min(count, copyLength);
			Buffer.BlockCopy(this.data, copySource, buffer, offset, actualCopyLength);
			if (count > copyLength)
			{
				Buffer.BlockCopy(this.data, 0, buffer, offset + actualCopyLength, count - actualCopyLength);
			}
		}

		private byte[] data;

		private int source;

		private int length;
	}
}
