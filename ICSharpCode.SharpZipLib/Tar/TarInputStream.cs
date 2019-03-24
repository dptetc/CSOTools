using System;
using System.IO;
using System.Text;

namespace ICSharpCode.SharpZipLib.Tar
{
	public class TarInputStream : Stream
	{
		public TarInputStream(Stream inputStream) : this(inputStream, 20)
		{
		}

		public TarInputStream(Stream inputStream, int blockFactor)
		{
			this.inputStream = inputStream;
			this.buffer = TarBuffer.CreateInputTarBuffer(inputStream, blockFactor);
		}

		public override bool CanRead
		{
			get
			{
				return this.inputStream.CanRead;
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
				return this.inputStream.Length;
			}
		}

		public override long Position
		{
			get
			{
				return this.inputStream.Position;
			}
			set
			{
				throw new NotSupportedException("TarInputStream Seek not supported");
			}
		}

		public override void Flush()
		{
			this.inputStream.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("TarInputStream Seek not supported");
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException("TarInputStream SetLength not supported");
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("TarInputStream Write not supported");
		}

		public override void WriteByte(byte value)
		{
			throw new NotSupportedException("TarInputStream WriteByte not supported");
		}

		public override int ReadByte()
		{
			byte[] array = new byte[1];
			int num = this.Read(array, 0, 1);
			if (num <= 0)
			{
				return -1;
			}
			return (int)array[0];
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int num = 0;
			if (this.entryOffset >= this.entrySize)
			{
				return 0;
			}
			long num2 = (long)count;
			if (num2 + this.entryOffset > this.entrySize)
			{
				num2 = this.entrySize - this.entryOffset;
			}
			if (this.readBuffer != null)
			{
				int num3 = (num2 > (long)this.readBuffer.Length) ? this.readBuffer.Length : ((int)num2);
				Array.Copy(this.readBuffer, 0, buffer, offset, num3);
				if (num3 >= this.readBuffer.Length)
				{
					this.readBuffer = null;
				}
				else
				{
					int num4 = this.readBuffer.Length - num3;
					byte[] destinationArray = new byte[num4];
					Array.Copy(this.readBuffer, num3, destinationArray, 0, num4);
					this.readBuffer = destinationArray;
				}
				num += num3;
				num2 -= (long)num3;
				offset += num3;
			}
			while (num2 > 0L)
			{
				byte[] array = this.buffer.ReadBlock();
				if (array == null)
				{
					throw new TarException("unexpected EOF with " + num2 + " bytes unread");
				}
				int num5 = (int)num2;
				int num6 = array.Length;
				if (num6 > num5)
				{
					Array.Copy(array, 0, buffer, offset, num5);
					this.readBuffer = new byte[num6 - num5];
					Array.Copy(array, num5, this.readBuffer, 0, num6 - num5);
				}
				else
				{
					num5 = num6;
					Array.Copy(array, 0, buffer, offset, num6);
				}
				num += num5;
				num2 -= (long)num5;
				offset += num5;
			}
			this.entryOffset += (long)num;
			return num;
		}

		public override void Close()
		{
			this.buffer.Close();
		}

		public void SetEntryFactory(TarInputStream.IEntryFactory factory)
		{
			this.entryFactory = factory;
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

		public long Available
		{
			get
			{
				return this.entrySize - this.entryOffset;
			}
		}

		public void Skip(long skipCount)
		{
			byte[] array = new byte[8192];
			int num2;
			for (long num = skipCount; num > 0L; num -= (long)num2)
			{
				int count = (num > (long)array.Length) ? array.Length : ((int)num);
				num2 = this.Read(array, 0, count);
				if (num2 == -1)
				{
					return;
				}
			}
		}

		public bool IsMarkSupported
		{
			get
			{
				return false;
			}
		}

		public void Mark(int markLimit)
		{
		}

		public void Reset()
		{
		}

		public TarEntry GetNextEntry()
		{
			if (this.hasHitEOF)
			{
				return null;
			}
			if (this.currentEntry != null)
			{
				this.SkipToNextEntry();
			}
			byte[] array = this.buffer.ReadBlock();
			if (array == null)
			{
				this.hasHitEOF = true;
			}
			else if (TarBuffer.IsEndOfArchiveBlock(array))
			{
				this.hasHitEOF = true;
			}
			if (this.hasHitEOF)
			{
				this.currentEntry = null;
			}
			else
			{
				try
				{
					TarHeader tarHeader = new TarHeader();
					tarHeader.ParseBuffer(array);
					if (!tarHeader.IsChecksumValid)
					{
						throw new TarException("Header checksum is invalid");
					}
					this.entryOffset = 0L;
					this.entrySize = tarHeader.Size;
					StringBuilder stringBuilder = null;
					if (tarHeader.TypeFlag == 76)
					{
						byte[] array2 = new byte[512];
						long num = this.entrySize;
						stringBuilder = new StringBuilder();
						while (num > 0L)
						{
							int num2 = this.Read(array2, 0, (num > (long)array2.Length) ? array2.Length : ((int)num));
							if (num2 == -1)
							{
								throw new InvalidHeaderException("Failed to read long name entry");
							}
							stringBuilder.Append(TarHeader.ParseName(array2, 0, num2).ToString());
							num -= (long)num2;
						}
						this.SkipToNextEntry();
						array = this.buffer.ReadBlock();
					}
					else if (tarHeader.TypeFlag == 103)
					{
						this.SkipToNextEntry();
						array = this.buffer.ReadBlock();
					}
					else if (tarHeader.TypeFlag == 120)
					{
						this.SkipToNextEntry();
						array = this.buffer.ReadBlock();
					}
					else if (tarHeader.TypeFlag == 86)
					{
						this.SkipToNextEntry();
						array = this.buffer.ReadBlock();
					}
					else if (tarHeader.TypeFlag != 48 && tarHeader.TypeFlag != 0 && tarHeader.TypeFlag != 53)
					{
						this.SkipToNextEntry();
						array = this.buffer.ReadBlock();
					}
					if (this.entryFactory == null)
					{
						this.currentEntry = new TarEntry(array);
						if (stringBuilder != null)
						{
							this.currentEntry.Name = stringBuilder.ToString();
						}
					}
					else
					{
						this.currentEntry = this.entryFactory.CreateEntry(array);
					}
					this.entryOffset = 0L;
					this.entrySize = this.currentEntry.Size;
				}
				catch (InvalidHeaderException ex)
				{
					this.entrySize = 0L;
					this.entryOffset = 0L;
					this.currentEntry = null;
					string message = string.Format("Bad header in record {0} block {1} {2}", this.buffer.CurrentRecord, this.buffer.CurrentBlock, ex.Message);
					throw new InvalidHeaderException(message);
				}
			}
			return this.currentEntry;
		}

		public void CopyEntryContents(Stream outputStream)
		{
			byte[] array = new byte[32768];
			for (;;)
			{
				int num = this.Read(array, 0, array.Length);
				if (num <= 0)
				{
					break;
				}
				outputStream.Write(array, 0, num);
			}
		}

		private void SkipToNextEntry()
		{
			long num = this.entrySize - this.entryOffset;
			if (num > 0L)
			{
				this.Skip(num);
			}
			this.readBuffer = null;
		}

		protected bool hasHitEOF;

		protected long entrySize;

		protected long entryOffset;

		protected byte[] readBuffer;

		protected TarBuffer buffer;

		private TarEntry currentEntry;

		protected TarInputStream.IEntryFactory entryFactory;

		private Stream inputStream;

		public interface IEntryFactory
		{
			TarEntry CreateEntry(string name);

			TarEntry CreateEntryFromFile(string fileName);

			TarEntry CreateEntry(byte[] headerBuffer);
		}

		public class EntryFactoryAdapter : TarInputStream.IEntryFactory
		{
			public TarEntry CreateEntry(string name)
			{
				return TarEntry.CreateTarEntry(name);
			}

			public TarEntry CreateEntryFromFile(string fileName)
			{
				return TarEntry.CreateEntryFromFile(fileName);
			}

			public TarEntry CreateEntry(byte[] headerBuffer)
			{
				return new TarEntry(headerBuffer);
			}
		}
	}
}
