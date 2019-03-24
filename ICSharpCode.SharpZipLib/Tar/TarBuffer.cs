using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Tar
{
	public class TarBuffer
	{
		public int RecordSize
		{
			get
			{
				return this.recordSize;
			}
		}

		[Obsolete("Use RecordSize property instead")]
		public int GetRecordSize()
		{
			return this.recordSize;
		}

		public int BlockFactor
		{
			get
			{
				return this.blockFactor;
			}
		}

		[Obsolete("Use BlockFactor property instead")]
		public int GetBlockFactor()
		{
			return this.blockFactor;
		}

		protected TarBuffer()
		{
		}

		public static TarBuffer CreateInputTarBuffer(Stream inputStream)
		{
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			return TarBuffer.CreateInputTarBuffer(inputStream, 20);
		}

		public static TarBuffer CreateInputTarBuffer(Stream inputStream, int blockFactor)
		{
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (blockFactor <= 0)
			{
				throw new ArgumentOutOfRangeException("blockFactor", "Factor cannot be negative");
			}
			TarBuffer tarBuffer = new TarBuffer();
			tarBuffer.inputStream = inputStream;
			tarBuffer.outputStream = null;
			tarBuffer.Initialize(blockFactor);
			return tarBuffer;
		}

		public static TarBuffer CreateOutputTarBuffer(Stream outputStream)
		{
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			return TarBuffer.CreateOutputTarBuffer(outputStream, 20);
		}

		public static TarBuffer CreateOutputTarBuffer(Stream outputStream, int blockFactor)
		{
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			if (blockFactor <= 0)
			{
				throw new ArgumentOutOfRangeException("blockFactor", "Factor cannot be negative");
			}
			TarBuffer tarBuffer = new TarBuffer();
			tarBuffer.inputStream = null;
			tarBuffer.outputStream = outputStream;
			tarBuffer.Initialize(blockFactor);
			return tarBuffer;
		}

		private void Initialize(int blockFactor)
		{
			this.blockFactor = blockFactor;
			this.recordSize = blockFactor * 512;
			this.recordBuffer = new byte[this.RecordSize];
			if (this.inputStream != null)
			{
				this.currentRecordIndex = -1;
				this.currentBlockIndex = this.BlockFactor;
				return;
			}
			this.currentRecordIndex = 0;
			this.currentBlockIndex = 0;
		}

		[Obsolete("Use IsEndOfArchiveBlock instead")]
		public bool IsEOFBlock(byte[] block)
		{
			if (block == null)
			{
				throw new ArgumentNullException("block");
			}
			if (block.Length != 512)
			{
				throw new ArgumentException("block length is invalid");
			}
			for (int i = 0; i < 512; i++)
			{
				if (block[i] != 0)
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsEndOfArchiveBlock(byte[] block)
		{
			if (block == null)
			{
				throw new ArgumentNullException("block");
			}
			if (block.Length != 512)
			{
				throw new ArgumentException("block length is invalid");
			}
			for (int i = 0; i < 512; i++)
			{
				if (block[i] != 0)
				{
					return false;
				}
			}
			return true;
		}

		public void SkipBlock()
		{
			if (this.inputStream == null)
			{
				throw new TarException("no input stream defined");
			}
			if (this.currentBlockIndex >= this.BlockFactor && !this.ReadRecord())
			{
				throw new TarException("Failed to read a record");
			}
			this.currentBlockIndex++;
		}

		public byte[] ReadBlock()
		{
			if (this.inputStream == null)
			{
				throw new TarException("TarBuffer.ReadBlock - no input stream defined");
			}
			if (this.currentBlockIndex >= this.BlockFactor && !this.ReadRecord())
			{
				throw new TarException("Failed to read a record");
			}
			byte[] array = new byte[512];
			Array.Copy(this.recordBuffer, this.currentBlockIndex * 512, array, 0, 512);
			this.currentBlockIndex++;
			return array;
		}

		private bool ReadRecord()
		{
			if (this.inputStream == null)
			{
				throw new TarException("no input stream stream defined");
			}
			this.currentBlockIndex = 0;
			int num = 0;
			long num2;
			for (int i = this.RecordSize; i > 0; i -= (int)num2)
			{
				num2 = (long)this.inputStream.Read(this.recordBuffer, num, i);
				if (num2 <= 0L)
				{
					break;
				}
				num += (int)num2;
			}
			this.currentRecordIndex++;
			return true;
		}

		public int CurrentBlock
		{
			get
			{
				return this.currentBlockIndex;
			}
		}

		[Obsolete("Use CurrentBlock property instead")]
		public int GetCurrentBlockNum()
		{
			return this.currentBlockIndex;
		}

		public int CurrentRecord
		{
			get
			{
				return this.currentRecordIndex;
			}
		}

		[Obsolete("Use CurrentRecord property instead")]
		public int GetCurrentRecordNum()
		{
			return this.currentRecordIndex;
		}

		public void WriteBlock(byte[] block)
		{
			if (block == null)
			{
				throw new ArgumentNullException("block");
			}
			if (this.outputStream == null)
			{
				throw new TarException("TarBuffer.WriteBlock - no output stream defined");
			}
			if (block.Length != 512)
			{
				string message = string.Format("TarBuffer.WriteBlock - block to write has length '{0}' which is not the block size of '{1}'", block.Length, 512);
				throw new TarException(message);
			}
			if (this.currentBlockIndex >= this.BlockFactor)
			{
				this.WriteRecord();
			}
			Array.Copy(block, 0, this.recordBuffer, this.currentBlockIndex * 512, 512);
			this.currentBlockIndex++;
		}

		public void WriteBlock(byte[] buffer, int offset)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (this.outputStream == null)
			{
				throw new TarException("TarBuffer.WriteBlock - no output stream stream defined");
			}
			if (offset < 0 || offset >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + 512 > buffer.Length)
			{
				string message = string.Format("TarBuffer.WriteBlock - record has length '{0}' with offset '{1}' which is less than the record size of '{2}'", buffer.Length, offset, this.recordSize);
				throw new TarException(message);
			}
			if (this.currentBlockIndex >= this.BlockFactor)
			{
				this.WriteRecord();
			}
			Array.Copy(buffer, offset, this.recordBuffer, this.currentBlockIndex * 512, 512);
			this.currentBlockIndex++;
		}

		private void WriteRecord()
		{
			if (this.outputStream == null)
			{
				throw new TarException("TarBuffer.WriteRecord no output stream defined");
			}
			this.outputStream.Write(this.recordBuffer, 0, this.RecordSize);
			this.outputStream.Flush();
			this.currentBlockIndex = 0;
			this.currentRecordIndex++;
		}

		private void Flush()
		{
			if (this.outputStream == null)
			{
				throw new TarException("TarBuffer.Flush no output stream defined");
			}
			if (this.currentBlockIndex > 0)
			{
				int num = this.currentBlockIndex * 512;
				Array.Clear(this.recordBuffer, num, this.RecordSize - num);
				this.WriteRecord();
			}
			this.outputStream.Flush();
		}

		public void Close()
		{
			if (this.outputStream != null)
			{
				this.Flush();
				this.outputStream.Close();
				this.outputStream = null;
				return;
			}
			if (this.inputStream != null)
			{
				this.inputStream.Close();
				this.inputStream = null;
			}
		}

		public const int BlockSize = 512;

		public const int DefaultBlockFactor = 20;

		public const int DefaultRecordSize = 10240;

		private Stream inputStream;

		private Stream outputStream;

		private byte[] recordBuffer;

		private int currentBlockIndex;

		private int currentRecordIndex;

		private int recordSize = 10240;

		private int blockFactor = 20;
	}
}
