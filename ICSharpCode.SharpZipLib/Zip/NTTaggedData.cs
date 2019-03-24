using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	public class NTTaggedData : ITaggedData
	{
		public short TagID
		{
			get
			{
				return 10;
			}
		}

		public void SetData(byte[] data, int index, int count)
		{
			using (MemoryStream memoryStream = new MemoryStream(data, index, count, false))
			{
				using (ZipHelperStream zipHelperStream = new ZipHelperStream(memoryStream))
				{
					zipHelperStream.ReadLEInt();
					while (zipHelperStream.Position < zipHelperStream.Length)
					{
						int num = zipHelperStream.ReadLEShort();
						int num2 = zipHelperStream.ReadLEShort();
						if (num == 1)
						{
							if (num2 >= 24)
							{
								long fileTime = zipHelperStream.ReadLELong();
								this.lastModificationTime_ = DateTime.FromFileTime(fileTime);
								long fileTime2 = zipHelperStream.ReadLELong();
								this.lastAccessTime_ = DateTime.FromFileTime(fileTime2);
								long fileTime3 = zipHelperStream.ReadLELong();
								this.createTime_ = DateTime.FromFileTime(fileTime3);
								break;
							}
							break;
						}
						else
						{
							zipHelperStream.Seek((long)num2, SeekOrigin.Current);
						}
					}
				}
			}
		}

		public byte[] GetData()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (ZipHelperStream zipHelperStream = new ZipHelperStream(memoryStream))
				{
					zipHelperStream.IsStreamOwner = false;
					zipHelperStream.WriteLEInt(0);
					zipHelperStream.WriteLEShort(1);
					zipHelperStream.WriteLEShort(24);
					zipHelperStream.WriteLELong(this.lastModificationTime_.ToFileTime());
					zipHelperStream.WriteLELong(this.lastAccessTime_.ToFileTime());
					zipHelperStream.WriteLELong(this.createTime_.ToFileTime());
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		public static bool IsValidValue(DateTime value)
		{
			bool result = true;
			try
			{
				value.ToFileTimeUtc();
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public DateTime LastModificationTime
		{
			get
			{
				return this.lastModificationTime_;
			}
			set
			{
				if (!NTTaggedData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.lastModificationTime_ = value;
			}
		}

		public DateTime CreateTime
		{
			get
			{
				return this.createTime_;
			}
			set
			{
				if (!NTTaggedData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.createTime_ = value;
			}
		}

		public DateTime LastAccessTime
		{
			get
			{
				return this.lastAccessTime_;
			}
			set
			{
				if (!NTTaggedData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.lastAccessTime_ = value;
			}
		}

		private DateTime lastAccessTime_ = DateTime.FromFileTime(0L);

		private DateTime lastModificationTime_ = DateTime.FromFileTime(0L);

		private DateTime createTime_ = DateTime.FromFileTime(0L);
	}
}
