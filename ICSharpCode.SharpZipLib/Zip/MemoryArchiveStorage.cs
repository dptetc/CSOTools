using System;
using System.IO;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	public class MemoryArchiveStorage : BaseArchiveStorage
	{
		public MemoryArchiveStorage() : base(FileUpdateMode.Direct)
		{
		}

		public MemoryArchiveStorage(FileUpdateMode updateMode) : base(updateMode)
		{
		}

		public MemoryStream FinalStream
		{
			get
			{
				return this.finalStream_;
			}
		}

		public override Stream GetTemporaryOutput()
		{
			this.temporaryStream_ = new MemoryStream();
			return this.temporaryStream_;
		}

		public override Stream ConvertTemporaryToFinal()
		{
			if (this.temporaryStream_ == null)
			{
				throw new ZipException("No temporary stream has been created");
			}
			this.finalStream_ = new MemoryStream(this.temporaryStream_.ToArray());
			return this.finalStream_;
		}

		public override Stream MakeTemporaryCopy(Stream stream)
		{
			this.temporaryStream_ = new MemoryStream();
			stream.Position = 0L;
			StreamUtils.Copy(stream, this.temporaryStream_, new byte[4096]);
			return this.temporaryStream_;
		}

		public override Stream OpenForDirectUpdate(Stream stream)
		{
			Stream stream2;
			if (stream == null || !stream.CanWrite)
			{
				stream2 = new MemoryStream();
				if (stream != null)
				{
					stream.Position = 0L;
					StreamUtils.Copy(stream, stream2, new byte[4096]);
					stream.Close();
				}
			}
			else
			{
				stream2 = stream;
			}
			return stream2;
		}

		public override void Dispose()
		{
			if (this.temporaryStream_ != null)
			{
				this.temporaryStream_.Close();
			}
		}

		private MemoryStream temporaryStream_;

		private MemoryStream finalStream_;
	}
}
