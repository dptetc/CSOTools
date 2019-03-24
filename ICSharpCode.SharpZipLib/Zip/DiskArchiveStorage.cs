using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	public class DiskArchiveStorage : BaseArchiveStorage
	{
		public DiskArchiveStorage(ZipFile file, FileUpdateMode updateMode) : base(updateMode)
		{
			if (file.Name == null)
			{
				throw new ZipException("Cant handle non file archives");
			}
			this.fileName_ = file.Name;
		}

		public DiskArchiveStorage(ZipFile file) : this(file, FileUpdateMode.Safe)
		{
		}

		public override Stream GetTemporaryOutput()
		{
			if (this.temporaryName_ != null)
			{
				this.temporaryName_ = DiskArchiveStorage.GetTempFileName(this.temporaryName_, true);
				this.temporaryStream_ = File.OpenWrite(this.temporaryName_);
			}
			else
			{
				this.temporaryName_ = Path.GetTempFileName();
				this.temporaryStream_ = File.OpenWrite(this.temporaryName_);
			}
			return this.temporaryStream_;
		}

		public override Stream ConvertTemporaryToFinal()
		{
			if (this.temporaryStream_ == null)
			{
				throw new ZipException("No temporary stream has been created");
			}
			Stream result = null;
			string tempFileName = DiskArchiveStorage.GetTempFileName(this.fileName_, false);
			bool flag = false;
			try
			{
				this.temporaryStream_.Close();
				File.Move(this.fileName_, tempFileName);
				File.Move(this.temporaryName_, this.fileName_);
				flag = true;
				File.Delete(tempFileName);
				result = File.OpenRead(this.fileName_);
			}
			catch (Exception)
			{
				result = null;
				if (!flag)
				{
					File.Move(tempFileName, this.fileName_);
					File.Delete(this.temporaryName_);
				}
				throw;
			}
			return result;
		}

		public override Stream MakeTemporaryCopy(Stream stream)
		{
			stream.Close();
			this.temporaryName_ = DiskArchiveStorage.GetTempFileName(this.fileName_, true);
			File.Copy(this.fileName_, this.temporaryName_, true);
			this.temporaryStream_ = new FileStream(this.temporaryName_, FileMode.Open, FileAccess.ReadWrite);
			return this.temporaryStream_;
		}

		public override Stream OpenForDirectUpdate(Stream stream)
		{
			Stream result;
			if (stream == null || !stream.CanWrite)
			{
				if (stream != null)
				{
					stream.Close();
				}
				result = new FileStream(this.fileName_, FileMode.Open, FileAccess.ReadWrite);
			}
			else
			{
				result = stream;
			}
			return result;
		}

		public override void Dispose()
		{
			if (this.temporaryStream_ != null)
			{
				this.temporaryStream_.Close();
			}
		}

		private static string GetTempFileName(string original, bool makeTempFile)
		{
			string text = null;
			if (original == null)
			{
				text = Path.GetTempFileName();
			}
			else
			{
				int num = 0;
				int second = DateTime.Now.Second;
				while (text == null)
				{
					num++;
					string text2 = string.Format("{0}.{1}{2}.tmp", original, second, num);
					if (!File.Exists(text2))
					{
						if (makeTempFile)
						{
							try
							{
								using (File.Create(text2))
								{
								}
								text = text2;
								continue;
							}
							catch
							{
								second = DateTime.Now.Second;
								continue;
							}
						}
						text = text2;
					}
				}
			}
			return text;
		}

		private Stream temporaryStream_;

		private string fileName_;

		private string temporaryName_;
	}
}
