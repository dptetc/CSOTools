using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Checksums;

namespace Nexon.CSO
{
	public sealed class NexonArchiveFileEntry
	{
		internal NexonArchiveFileEntry(NexonArchive archive)
		{
			this.archive = archive;
		}

		public NexonArchive Archive
		{
			get
			{
				return this.archive;
			}
		}

		public string Path
		{
			get
			{
				return this.path;
			}
		}

		public long Size
		{
			get
			{
				return this.extractedSize;
			}
		}

		public DateTime LastModifiedTime
		{
			get
			{
				return this.lastModifiedTime;
			}
		}

		private static DateTime FromEpoch(int epoch)
		{
			return new DateTime((long)epoch * 10000000L + 621355968000000000L);
		}

		internal int Load(byte[] header, int offset)
		{
			int result;
			try
			{
				int pathSize = (int)BitConverter.ToUInt16(header, offset);
				this.path = Encoding.ASCII.GetString(header, offset + 2, pathSize);
				this.storedType = (NexonArchiveFileEntryType)BitConverter.ToInt32(header, offset + 2 + pathSize);
				this.offset = (long)((ulong)BitConverter.ToUInt32(header, offset + 2 + pathSize + 4));
				this.storedSize = (long)BitConverter.ToInt32(header, offset + 2 + pathSize + 8);
				this.extractedSize = (long)BitConverter.ToInt32(header, offset + 2 + pathSize + 12);
				this.lastModifiedTime = NexonArchiveFileEntry.FromEpoch(BitConverter.ToInt32(header, offset + 2 + pathSize + 16));
				this.checksum = BitConverter.ToUInt32(header, offset + 2 + pathSize + 20);
				result = 2 + pathSize + 24;
			}
			catch (ArgumentOutOfRangeException ex)
			{
				throw new InvalidDataException("NAR file entry is invalid.", ex);
			}
			return result;
		}

		public long Extract(Stream outputStream)
		{
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			if (!outputStream.CanWrite)
			{
				throw new ArgumentException("Cannot write to stream.", "outputStream");
			}
			if (this.extractedSize == 0L)
			{
				return 0L;
			}
			long result;
			lock (this.archive.Stream)
			{
				Stream readStream = new BoundedStream(this.archive.Stream, this.offset, this.storedSize);
				readStream.Position = 0L;
				switch (this.storedType)
				{
				case NexonArchiveFileEntryType.Raw:
					break;
				case NexonArchiveFileEntryType.Encoded:
					readStream = new NexonArchiveFileDecoderStream(readStream, this.path);
					break;
				case NexonArchiveFileEntryType.EncodedAndCompressed:
					readStream = new NexonArchiveFileDecompressStream(new NexonArchiveFileDecoderStream(readStream, this.path), this.extractedSize);
					break;
				default:
					throw new NotSupportedException("Unsupported file storage type: " + this.storedType + ".");
				}
				lock (outputStream)
				{
					byte[] buffer = new byte[8192];
					long totalLength = 0L;
					int length;
					while ((length = readStream.Read(buffer, 0, 8192)) > 0)
					{
						outputStream.Write(buffer, 0, length);
						totalLength += (long)length;
					}
					result = totalLength;
				}
			}
			return result;
		}

		public bool Verify()
		{
			Crc32 crc = new Crc32();
			lock (this.archive.Stream)
			{
				Stream readStream = new BoundedStream(this.archive.Stream, this.offset, this.storedSize);
				readStream.Position = 0L;
				byte[] buffer = new byte[8192];
				int length;
				while ((length = readStream.Read(buffer, 0, 8192)) > 0)
				{
					crc.Update(buffer, 0, length);
				}
			}
			return (ulong)this.checksum == (ulong)crc.Value;
		}

		private NexonArchive archive;

		private string path;

		private NexonArchiveFileEntryType storedType;

		private long offset;

		private long storedSize;

		private long extractedSize;

		private DateTime lastModifiedTime;

		private uint checksum;
	}
}
