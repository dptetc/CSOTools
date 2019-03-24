using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using ICSharpCode.SharpZipLib.BZip2;

namespace Nexon.CSO
{
	public sealed class NexonArchive : IDisposable
	{
		internal Stream Stream
		{
			get
			{
				return this.stream;
			}
		}

		public ReadOnlyCollection<NexonArchiveFileEntry> FileEntries
		{
			get
			{
				return this.fileEntries.AsReadOnly();
			}
		}

		public void Load(string fileName, bool writable)
		{
			if (this.stream != null)
			{
				throw new InvalidOperationException("The archive must be disposed before it can be loaded again.");
			}
			this.Load(new FileStream(fileName, FileMode.Open, writable ? FileAccess.ReadWrite : FileAccess.Read, FileShare.Read), writable);
		}

		public void Load(Stream stream, bool writable)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanRead)
			{
				throw new ArgumentException("Cannot read from stream.", "stream");
			}
			if (!stream.CanSeek)
			{
				throw new ArgumentException("Cannot seek in stream.", "stream");
			}
			if (writable && !stream.CanWrite)
			{
				throw new ArgumentException("Cannot write to stream.", "stream");
			}
			if (this.stream != null)
			{
				throw new InvalidOperationException("The archive must be disposed before it can be loaded again.");
			}
			int headerSize;
			byte[] header;
			lock (stream)
			{
				stream.Position = 0L;
				this.stream = stream;
				BinaryReader reader = new BinaryReader(this.stream);
				if (reader.ReadInt32() != 5390670)
				{
					throw new InvalidDataException("NAR file signature is invalid.");
				}
				if (reader.ReadInt32() != 16777216)
				{
					throw new InvalidDataException("NAR file version is invalid.");
				}
				if (this.stream.Length < 16L)
				{
					throw new InvalidDataException("NAR file is not long enough to be valid.");
				}
				this.stream.Seek(-4L, SeekOrigin.End);
				if (reader.ReadInt32() != 5390670)
				{
					throw new InvalidDataException("NAR end file signature is invalid.");
				}
				this.stream.Seek(-8L, SeekOrigin.Current);
				headerSize = (reader.ReadInt32() ^ 1081496863);
				if (this.stream.Length < (long)(headerSize + 16))
				{
					throw new InvalidDataException("NAR file is not long enough to be valid.");
				}
				this.stream.Seek((long)(-4 - headerSize), SeekOrigin.Current);
				header = reader.ReadBytes(headerSize);
			}
			for (int i = 0; i < header.Length; i++)
			{
				byte[] array = header;
				int num = i;
				array[num] ^= NexonArchive.HeaderXor[i & 15];
			}
			using (MemoryStream decompressedHeaderStream = new MemoryStream(headerSize))
			{
				BZip2.Decompress(new MemoryStream(header, false), decompressedHeaderStream);
				byte[] decompressedHeader = decompressedHeaderStream.ToArray();
				this.LoadHeader(decompressedHeader);
			}
		}

		private void LoadHeader(byte[] header)
		{
			if (header.Length < 4)
			{
				throw new InvalidDataException("NAR header is invalid.");
			}
			int version = BitConverter.ToInt32(header, 0);
			if (version != 1)
			{
				throw new InvalidDataException("NAR header version is invalid.");
			}
			if (header.Length < 16)
			{
				throw new InvalidDataException("NAR header is invalid.");
			}
			BitConverter.ToInt32(header, 4);
			BitConverter.ToInt32(header, 8);
			BitConverter.ToInt32(header, 12);
			int directoryCount = BitConverter.ToInt32(header, 16);
			if (directoryCount < 0)
			{
				throw new InvalidDataException("Directory entry count is too large.");
			}
			int entryOffset = 20;
			for (int i = 0; i < directoryCount; i++)
			{
				NexonArchiveFileEntry fileEntry = new NexonArchiveFileEntry(this);
				entryOffset += fileEntry.Load(header, entryOffset);
				this.fileEntries.Add(fileEntry);
			}
		}

		public void Close()
		{
			this.fileEntries.Clear();
			this.stream.Close();
			this.stream = null;
		}

		public void Dispose()
		{
			this.Close();
		}

		private static readonly byte[] HeaderXor = new byte[]
		{
			25,
			91,
			123,
			44,
			101,
			94,
			121,
			37,
			110,
			75,
			7,
			33,
			98,
			127,
			0,
			41
		};

		private Stream stream;

		private List<NexonArchiveFileEntry> fileEntries = new List<NexonArchiveFileEntry>();
	}
}
