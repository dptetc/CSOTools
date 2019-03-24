using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	public class ExtendedUnixData : ITaggedData
	{
		public short TagID
		{
			get
			{
				return 21589;
			}
		}

		public void SetData(byte[] data, int index, int count)
		{
			using (MemoryStream memoryStream = new MemoryStream(data, index, count, false))
			{
				using (ZipHelperStream zipHelperStream = new ZipHelperStream(memoryStream))
				{
					this.flags_ = (ExtendedUnixData.Flags)zipHelperStream.ReadByte();
					if ((byte)(this.flags_ & ExtendedUnixData.Flags.ModificationTime) != 0 && count >= 5)
					{
						int seconds = zipHelperStream.ReadLEInt();
						this.modificationTime_ = (new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime() + new TimeSpan(0, 0, 0, seconds, 0)).ToLocalTime();
					}
					if ((byte)(this.flags_ & ExtendedUnixData.Flags.AccessTime) != 0)
					{
						int seconds2 = zipHelperStream.ReadLEInt();
						this.lastAccessTime_ = (new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime() + new TimeSpan(0, 0, 0, seconds2, 0)).ToLocalTime();
					}
					if ((byte)(this.flags_ & ExtendedUnixData.Flags.CreateTime) != 0)
					{
						int seconds3 = zipHelperStream.ReadLEInt();
						this.createTime_ = (new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime() + new TimeSpan(0, 0, 0, seconds3, 0)).ToLocalTime();
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
					zipHelperStream.WriteByte((byte)this.flags_);
					if ((byte)(this.flags_ & ExtendedUnixData.Flags.ModificationTime) != 0)
					{
						int value = (int)(this.modificationTime_.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime()).TotalSeconds;
						zipHelperStream.WriteLEInt(value);
					}
					if ((byte)(this.flags_ & ExtendedUnixData.Flags.AccessTime) != 0)
					{
						int value2 = (int)(this.lastAccessTime_.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime()).TotalSeconds;
						zipHelperStream.WriteLEInt(value2);
					}
					if ((byte)(this.flags_ & ExtendedUnixData.Flags.CreateTime) != 0)
					{
						int value3 = (int)(this.createTime_.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime()).TotalSeconds;
						zipHelperStream.WriteLEInt(value3);
					}
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		public static bool IsValidValue(DateTime value)
		{
			return value >= new DateTime(1901, 12, 13, 20, 45, 52) || value <= new DateTime(2038, 1, 19, 3, 14, 7);
		}

		public DateTime ModificationTime
		{
			get
			{
				return this.modificationTime_;
			}
			set
			{
				if (!ExtendedUnixData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.flags_ |= ExtendedUnixData.Flags.ModificationTime;
				this.modificationTime_ = value;
			}
		}

		public DateTime AccessTime
		{
			get
			{
				return this.lastAccessTime_;
			}
			set
			{
				if (!ExtendedUnixData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.flags_ |= ExtendedUnixData.Flags.AccessTime;
				this.lastAccessTime_ = value;
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
				if (!ExtendedUnixData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.flags_ |= ExtendedUnixData.Flags.CreateTime;
				this.createTime_ = value;
			}
		}

		private ExtendedUnixData.Flags Include
		{
			get
			{
				return this.flags_;
			}
			set
			{
				this.flags_ = value;
			}
		}

		private ExtendedUnixData.Flags flags_;

		private DateTime modificationTime_ = new DateTime(1970, 1, 1);

		private DateTime lastAccessTime_ = new DateTime(1970, 1, 1);

		private DateTime createTime_ = new DateTime(1970, 1, 1);

		[Flags]
		public enum Flags : byte
		{
			ModificationTime = 1,
			AccessTime = 2,
			CreateTime = 4
		}
	}
}
