using System;
using System.IO;
using System.Text;

namespace Nexon.CSO
{
	internal sealed class NexonArchiveFileDecoderStream : Stream
	{
		public NexonArchiveFileDecoderStream(Stream stream, string path)
		{
			this.baseStream = stream;
			this.GenerateKey(path);
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
				return true;
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
				return this.baseStream.Length;
			}
		}

		public override long Position
		{
			get
			{
				return this.baseStream.Position;
			}
			set
			{
				this.baseStream.Position = value;
			}
		}

		private static uint PythonHash(byte[] data)
		{
			uint hash = 0u;
			for (int i = 0; i < data.Length; i++)
			{
				hash = (hash * 1000003u ^ (uint)data[i]);
			}
			return hash ^ (uint)data.Length;
		}

		private void GenerateKey(string path)
		{
			uint seed = NexonArchiveFileDecoderStream.PythonHash(Encoding.ASCII.GetBytes(path));
			for (int i = 0; i < 16; i++)
			{
				seed = seed * 1103515245u + 12345u;
				this.key[i] = (byte)(seed & 255u);
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

		public override int Read(byte[] buffer, int offset, int count)
		{
			long tempOffset = this.Position;
			int length = this.baseStream.Read(buffer, offset, count);
			for (int i = 0; i < length; i++)
			{
				int num = offset + i;
				buffer[num] ^= this.key[(int)(checked((IntPtr)(unchecked(tempOffset + (long)i) & 15L)))];
			}
			return length;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.baseStream.Seek(offset, origin);
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

		private byte[] key = new byte[16];
	}
}
