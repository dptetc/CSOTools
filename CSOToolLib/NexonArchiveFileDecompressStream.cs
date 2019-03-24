using System;
using System.IO;

namespace Nexon.CSO
{
	internal sealed class NexonArchiveFileDecompressStream : Stream
	{
		public NexonArchiveFileDecompressStream(Stream stream, long length)
		{
			this.baseStream = stream;
			this.outputLength = length;
		}

		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		public override long Length
		{
			get
			{
				return this.outputLength;
			}
		}

		public override long Position
		{
			get
			{
				return this.outputPosition;
			}
			set
			{
				throw new NotSupportedException("Cannot seek in stream.");
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
		}

		private byte ReadByteChecked()
		{
			int temp = this.baseStream.ReadByte();
			if (temp < 0)
			{
				throw new EndOfStreamException();
			}
			return Convert.ToByte(temp);
		}

		private void ReadHeader()
		{
			byte tempByte = this.ReadByteChecked();
			int operation = tempByte >> 5;
			int length = (int)(tempByte & 31);
			if (operation == 0)
			{
				this.lastReadDistance = 0;
				this.lastReadLength = length + 1;
				return;
			}
			if (operation == 7)
			{
				operation += (int)this.ReadByteChecked();
			}
			operation += 2;
			length = (length << 8 | (int)this.ReadByteChecked()) + 1;
			this.lastReadDistance = length;
			this.lastReadLength = operation;
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
			if (count == 0)
			{
				return 0;
			}
			if (this.baseStream.Position >= this.baseStream.Length)
			{
				return 0;
			}
			if (this.Position >= this.Length)
			{
				return 0;
			}
			count = Convert.ToInt32(Math.Min(this.Length - this.Position, (long)count));
			int totalCount = count;
			int totalCountFixed = totalCount;
			while (totalCount > 0)
			{
				if (this.lastReadLength == 0)
				{
					this.ReadHeader();
					if (this.lastReadDistance > 0 && this.lastReadDistance > this.dictionary.Length)
					{
						throw new InvalidDataException("Distance is larger than the dictionary's current length.");
					}
				}
				if (count > this.lastReadLength)
				{
					count = this.lastReadLength;
				}
				if (this.lastReadDistance == 0)
				{
					int lengthRead = this.baseStream.Read(buffer, offset, count);
					if (lengthRead == 0)
					{
						throw new EndOfStreamException("Expected " + this.lastReadLength + " more bytes in compressed stream.");
					}
					this.dictionary.Append(buffer, offset, lengthRead);
					this.lastReadLength -= lengthRead;
					this.outputPosition += (long)lengthRead;
					totalCount -= lengthRead;
					offset += lengthRead;
				}
				else
				{
					while (count > 0)
					{
						this.dictionary.Copy(this.lastReadDistance, buffer, offset, 1);
						this.dictionary.Append(buffer, offset, 1);
						this.lastReadLength--;
						this.outputPosition += 1L;
						totalCount--;
						offset++;
						count--;
					}
				}
				count = totalCount;
			}
			return totalCountFixed;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("Cannot seek in stream.");
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException("Cannot write to stream.");
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("Cannot write to stream.");
		}

		private Stream baseStream;

		private long outputLength;

		private long outputPosition;

		private CircularBuffer dictionary = new CircularBuffer(8192);

		private int lastReadDistance;

		private int lastReadLength;
	}
}
