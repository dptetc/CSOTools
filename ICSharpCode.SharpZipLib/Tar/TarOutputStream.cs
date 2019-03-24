using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Tar
{
	public class TarOutputStream : Stream
	{
		public TarOutputStream(Stream outputStream) : this(outputStream, 20)
		{
		}

		public TarOutputStream(Stream outputStream, int blockFactor)
		{
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			this.outputStream = outputStream;
			this.buffer = TarBuffer.CreateOutputTarBuffer(outputStream, blockFactor);
			this.assemblyBuffer = new byte[512];
			this.blockBuffer = new byte[512];
		}

		public override bool CanRead
		{
			get
			{
				return this.outputStream.CanRead;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return this.outputStream.CanSeek;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return this.outputStream.CanWrite;
			}
		}

		public override long Length
		{
			get
			{
				return this.outputStream.Length;
			}
		}

		public override long Position
		{
			get
			{
				return this.outputStream.Position;
			}
			set
			{
				this.outputStream.Position = value;
			}
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.outputStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			this.outputStream.SetLength(value);
		}

		public override int ReadByte()
		{
			return this.outputStream.ReadByte();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.outputStream.Read(buffer, offset, count);
		}

		public override void Flush()
		{
			this.outputStream.Flush();
		}

		public void Finish()
		{
			if (this.IsEntryOpen)
			{
				this.CloseEntry();
			}
			this.WriteEofBlock();
		}

		public override void Close()
		{
			if (!this.isClosed)
			{
				this.isClosed = true;
				this.Finish();
				this.buffer.Close();
			}
		}

		public int RecordSize
		{
			get
			{
				return this.buffer.RecordSize;
			}
		}

		[Obsolete("Use RecordSize property instead")]
		public int GetRecordSize()
		{
			return this.buffer.RecordSize;
		}

		private bool IsEntryOpen
		{
			get
			{
				return this.currBytes < this.currSize;
			}
		}

		public void PutNextEntry(TarEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			if (entry.TarHeader.Name.Length >= 100)
			{
				TarHeader tarHeader = new TarHeader();
				tarHeader.TypeFlag = 76;
				tarHeader.Name += "././@LongLink";
				tarHeader.UserId = 0;
				tarHeader.GroupId = 0;
				tarHeader.GroupName = "";
				tarHeader.UserName = "";
				tarHeader.LinkName = "";
				tarHeader.Size = (long)entry.TarHeader.Name.Length;
				tarHeader.WriteHeader(this.blockBuffer);
				this.buffer.WriteBlock(this.blockBuffer);
				int i = 0;
				while (i < entry.TarHeader.Name.Length)
				{
					Array.Clear(this.blockBuffer, 0, this.blockBuffer.Length);
					TarHeader.GetAsciiBytes(entry.TarHeader.Name, i, this.blockBuffer, 0, 512);
					i += 512;
					this.buffer.WriteBlock(this.blockBuffer);
				}
			}
			entry.WriteEntryHeader(this.blockBuffer);
			this.buffer.WriteBlock(this.blockBuffer);
			this.currBytes = 0L;
			this.currSize = (entry.IsDirectory ? 0L : entry.Size);
		}

		public void CloseEntry()
		{
			if (this.assemblyBufferLength > 0)
			{
				Array.Clear(this.assemblyBuffer, this.assemblyBufferLength, this.assemblyBuffer.Length - this.assemblyBufferLength);
				this.buffer.WriteBlock(this.assemblyBuffer);
				this.currBytes += (long)this.assemblyBufferLength;
				this.assemblyBufferLength = 0;
			}
			if (this.currBytes < this.currSize)
			{
				string message = string.Format("Entry closed at '{0}' before the '{1}' bytes specified in the header were written", this.currBytes, this.currSize);
				throw new TarException(message);
			}
		}

		public override void WriteByte(byte value)
		{
			this.Write(new byte[]
			{
				value
			}, 0, 1);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Cannot be negative");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("offset and count combination is invalid");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Cannot be negative");
			}
			if (this.currBytes + (long)count > this.currSize)
			{
				string message = string.Format("request to write '{0}' bytes exceeds size in header of '{1}' bytes", count, this.currSize);
				throw new ArgumentOutOfRangeException("count", message);
			}
			if (this.assemblyBufferLength > 0)
			{
				if (this.assemblyBufferLength + count >= this.blockBuffer.Length)
				{
					int num = this.blockBuffer.Length - this.assemblyBufferLength;
					Array.Copy(this.assemblyBuffer, 0, this.blockBuffer, 0, this.assemblyBufferLength);
					Array.Copy(buffer, offset, this.blockBuffer, this.assemblyBufferLength, num);
					this.buffer.WriteBlock(this.blockBuffer);
					this.currBytes += (long)this.blockBuffer.Length;
					offset += num;
					count -= num;
					this.assemblyBufferLength = 0;
				}
				else
				{
					Array.Copy(buffer, offset, this.assemblyBuffer, this.assemblyBufferLength, count);
					offset += count;
					this.assemblyBufferLength += count;
					count -= count;
				}
			}
			while (count > 0)
			{
				if (count < this.blockBuffer.Length)
				{
					Array.Copy(buffer, offset, this.assemblyBuffer, this.assemblyBufferLength, count);
					this.assemblyBufferLength += count;
					return;
				}
				this.buffer.WriteBlock(buffer, offset);
				int num2 = this.blockBuffer.Length;
				this.currBytes += (long)num2;
				count -= num2;
				offset += num2;
			}
		}

		private void WriteEofBlock()
		{
			Array.Clear(this.blockBuffer, 0, this.blockBuffer.Length);
			this.buffer.WriteBlock(this.blockBuffer);
		}

		private long currBytes;

		private int assemblyBufferLength;

		private bool isClosed;

		protected long currSize;

		protected byte[] blockBuffer;

		protected byte[] assemblyBuffer;

		protected TarBuffer buffer;

		protected Stream outputStream;
	}
}
