using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	public sealed class ZipExtraData : IDisposable
	{
		public ZipExtraData()
		{
			this.Clear();
		}

		public ZipExtraData(byte[] data)
		{
			if (data == null)
			{
				this.data_ = new byte[0];
				return;
			}
			this.data_ = data;
		}

		public byte[] GetEntryData()
		{
			if (this.Length > 65535)
			{
				throw new ZipException("Data exceeds maximum length");
			}
			return (byte[])this.data_.Clone();
		}

		public void Clear()
		{
			if (this.data_ == null || this.data_.Length != 0)
			{
				this.data_ = new byte[0];
			}
		}

		public int Length
		{
			get
			{
				return this.data_.Length;
			}
		}

		public Stream GetStreamForTag(int tag)
		{
			Stream result = null;
			if (this.Find(tag))
			{
				result = new MemoryStream(this.data_, this.index_, this.readValueLength_, false);
			}
			return result;
		}

		private ITaggedData GetData(short tag)
		{
			ITaggedData result = null;
			if (this.Find((int)tag))
			{
				result = ZipExtraData.Create(tag, this.data_, this.readValueStart_, this.readValueLength_);
			}
			return result;
		}

		private static ITaggedData Create(short tag, byte[] data, int offset, int count)
		{
			ITaggedData taggedData;
			if (tag != 10)
			{
				if (tag != 21589)
				{
					taggedData = new RawTaggedData(tag);
				}
				else
				{
					taggedData = new ExtendedUnixData();
				}
			}
			else
			{
				taggedData = new NTTaggedData();
			}
			taggedData.SetData(data, offset, count);
			return taggedData;
		}

		public int ValueLength
		{
			get
			{
				return this.readValueLength_;
			}
		}

		public int CurrentReadIndex
		{
			get
			{
				return this.index_;
			}
		}

		public int UnreadCount
		{
			get
			{
				if (this.readValueStart_ > this.data_.Length || this.readValueStart_ < 4)
				{
					throw new ZipException("Find must be called before calling a Read method");
				}
				return this.readValueStart_ + this.readValueLength_ - this.index_;
			}
		}

		public bool Find(int headerID)
		{
			this.readValueStart_ = this.data_.Length;
			this.readValueLength_ = 0;
			this.index_ = 0;
			int num = this.readValueStart_;
			int num2 = headerID - 1;
			while (num2 != headerID && this.index_ < this.data_.Length - 3)
			{
				num2 = this.ReadShortInternal();
				num = this.ReadShortInternal();
				if (num2 != headerID)
				{
					this.index_ += num;
				}
			}
			bool flag = num2 == headerID && this.index_ + num <= this.data_.Length;
			if (flag)
			{
				this.readValueStart_ = this.index_;
				this.readValueLength_ = num;
			}
			return flag;
		}

		public void AddEntry(ITaggedData taggedData)
		{
			if (taggedData == null)
			{
				throw new ArgumentNullException("taggedData");
			}
			this.AddEntry((int)taggedData.TagID, taggedData.GetData());
		}

		public void AddEntry(int headerID, byte[] fieldData)
		{
			if (headerID > 65535 || headerID < 0)
			{
				throw new ArgumentOutOfRangeException("headerID");
			}
			int num = (fieldData == null) ? 0 : fieldData.Length;
			if (num > 65535)
			{
				throw new ArgumentOutOfRangeException("fieldData", "exceeds maximum length");
			}
			int num2 = this.data_.Length + num + 4;
			if (this.Find(headerID))
			{
				num2 -= this.ValueLength + 4;
			}
			if (num2 > 65535)
			{
				throw new ZipException("Data exceeds maximum length");
			}
			this.Delete(headerID);
			byte[] array = new byte[num2];
			this.data_.CopyTo(array, 0);
			int index = this.data_.Length;
			this.data_ = array;
			this.SetShort(ref index, headerID);
			this.SetShort(ref index, num);
			if (fieldData != null)
			{
				fieldData.CopyTo(array, index);
			}
		}

		public void StartNewEntry()
		{
			this.newEntry_ = new MemoryStream();
		}

		public void AddNewEntry(int headerID)
		{
			byte[] fieldData = this.newEntry_.ToArray();
			this.newEntry_ = null;
			this.AddEntry(headerID, fieldData);
		}

		public void AddData(byte data)
		{
			this.newEntry_.WriteByte(data);
		}

		public void AddData(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this.newEntry_.Write(data, 0, data.Length);
		}

		public void AddLeShort(int toAdd)
		{
			this.newEntry_.WriteByte((byte)toAdd);
			this.newEntry_.WriteByte((byte)(toAdd >> 8));
		}

		public void AddLeInt(int toAdd)
		{
			this.AddLeShort((int)((short)toAdd));
			this.AddLeShort((int)((short)(toAdd >> 16)));
		}

		public void AddLeLong(long toAdd)
		{
			this.AddLeInt((int)(toAdd & (long)((ulong)-1)));
			this.AddLeInt((int)(toAdd >> 32));
		}

		public bool Delete(int headerID)
		{
			bool result = false;
			if (this.Find(headerID))
			{
				result = true;
				int num = this.readValueStart_ - 4;
				byte[] destinationArray = new byte[this.data_.Length - (this.ValueLength + 4)];
				Array.Copy(this.data_, 0, destinationArray, 0, num);
				int num2 = num + this.ValueLength + 4;
				Array.Copy(this.data_, num2, destinationArray, num, this.data_.Length - num2);
				this.data_ = destinationArray;
			}
			return result;
		}

		public long ReadLong()
		{
			this.ReadCheck(8);
			return ((long)this.ReadInt() & (long)((ulong)-1)) | (long)this.ReadInt() << 32;
		}

		public int ReadInt()
		{
			this.ReadCheck(4);
			int result = (int)this.data_[this.index_] + ((int)this.data_[this.index_ + 1] << 8) + ((int)this.data_[this.index_ + 2] << 16) + ((int)this.data_[this.index_ + 3] << 24);
			this.index_ += 4;
			return result;
		}

		public int ReadShort()
		{
			this.ReadCheck(2);
			int result = (int)this.data_[this.index_] + ((int)this.data_[this.index_ + 1] << 8);
			this.index_ += 2;
			return result;
		}

		public int ReadByte()
		{
			int result = -1;
			if (this.index_ < this.data_.Length && this.readValueStart_ + this.readValueLength_ > this.index_)
			{
				result = (int)this.data_[this.index_];
				this.index_++;
			}
			return result;
		}

		public void Skip(int amount)
		{
			this.ReadCheck(amount);
			this.index_ += amount;
		}

		private void ReadCheck(int length)
		{
			if (this.readValueStart_ > this.data_.Length || this.readValueStart_ < 4)
			{
				throw new ZipException("Find must be called before calling a Read method");
			}
			if (this.index_ > this.readValueStart_ + this.readValueLength_ - length)
			{
				throw new ZipException("End of extra data");
			}
		}

		private int ReadShortInternal()
		{
			if (this.index_ > this.data_.Length - 2)
			{
				throw new ZipException("End of extra data");
			}
			int result = (int)this.data_[this.index_] + ((int)this.data_[this.index_ + 1] << 8);
			this.index_ += 2;
			return result;
		}

		private void SetShort(ref int index, int source)
		{
			this.data_[index] = (byte)source;
			this.data_[index + 1] = (byte)(source >> 8);
			index += 2;
		}

		public void Dispose()
		{
			if (this.newEntry_ != null)
			{
				this.newEntry_.Close();
			}
		}

		private int index_;

		private int readValueStart_;

		private int readValueLength_;

		private MemoryStream newEntry_;

		private byte[] data_;
	}
}
