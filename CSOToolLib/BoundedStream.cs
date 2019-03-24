using System;
using System.IO;

namespace Nexon.CSO
{
	internal sealed class BoundedStream : Stream
	{
		public BoundedStream(Stream stream, long offset, long length)
		{
			this.baseStream = stream;
			this.baseOffset = offset;
			this.baseLength = length;
			this.position = this.baseStream.Position - this.baseOffset;
			if (this.position < 0L)
			{
				this.baseStream.Seek(-this.position, SeekOrigin.Current);
				this.position = 0L;
			}
		}

		public override bool CanRead
		{
			get
			{
				return this.baseStream.CanRead;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return this.baseStream.CanSeek;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return this.baseStream.CanWrite;
			}
		}

		public override long Length
		{
			get
			{
				return this.baseLength;
			}
		}

		public override long Position
		{
			get
			{
				return this.position;
			}
			set
			{
				if (!this.CanSeek)
				{
					throw new NotSupportedException("Cannot seek in stream.");
				}
				if (value < 0L || value > this.baseLength)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.baseStream.Position = this.baseOffset + value;
				this.position = this.baseStream.Position - this.baseOffset;
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				this.baseStream.Dispose();
			}
		}

		public override void Flush()
		{
			this.baseStream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
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
			if (this.position + (long)count > this.baseLength)
			{
				count = Convert.ToInt32(Math.Min(this.baseLength - this.position, 2147483647L));
			}
			int bytesRead = this.baseStream.Read(buffer, offset, count);
			this.position += (long)bytesRead;
			return bytesRead;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			if (!this.CanSeek)
			{
				throw new NotSupportedException("Cannot seek in stream.");
			}
			long temp;
			switch (origin)
			{
			case SeekOrigin.Begin:
				if (offset < 0L || offset > this.baseLength)
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				temp = this.baseStream.Seek(this.baseOffset + offset, SeekOrigin.Begin);
				break;
			case SeekOrigin.Current:
				temp = this.position + offset;
				if (temp < 0L || temp > this.baseLength)
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				temp = this.baseStream.Seek(offset, SeekOrigin.Current);
				break;
			case SeekOrigin.End:
				temp = this.baseLength + offset;
				if (temp < 0L || temp > this.baseLength)
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				temp = this.baseStream.Seek(offset, SeekOrigin.End);
				break;
			default:
				throw new ArgumentException("Not a valid seek origin.", "origin");
			}
			this.position = temp - this.baseOffset;
			return this.position;
		}

		public override void SetLength(long value)
		{
			if (value < this.baseOffset + this.baseLength)
			{
				throw new ArgumentException("Value is less than the stream's boundaries.", "value");
			}
			this.baseStream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
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
			if (this.position + (long)count > this.baseLength)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this.baseStream.Write(buffer, offset, count);
			this.position += (long)count;
		}

		private Stream baseStream;

		private long baseOffset;

		private long baseLength;

		private long position;
	}
}
