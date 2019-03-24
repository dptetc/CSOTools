using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Tar
{
	public class TarEntry : ICloneable
	{
		private TarEntry()
		{
			this.header = new TarHeader();
		}

		public TarEntry(byte[] headerBuffer)
		{
			this.header = new TarHeader();
			this.header.ParseBuffer(headerBuffer);
		}

		public TarEntry(TarHeader header)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			this.header = (TarHeader)header.Clone();
		}

		public object Clone()
		{
			return new TarEntry
			{
				file = this.file,
				header = (TarHeader)this.header.Clone(),
				Name = this.Name
			};
		}

		public static TarEntry CreateTarEntry(string name)
		{
			TarEntry tarEntry = new TarEntry();
			TarEntry.NameTarHeader(tarEntry.header, name);
			return tarEntry;
		}

		public static TarEntry CreateEntryFromFile(string fileName)
		{
			TarEntry tarEntry = new TarEntry();
			tarEntry.GetFileTarHeader(tarEntry.header, fileName);
			return tarEntry;
		}

		public override bool Equals(object obj)
		{
			TarEntry tarEntry = obj as TarEntry;
			return tarEntry != null && this.Name.Equals(tarEntry.Name);
		}

		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}

		public bool IsDescendent(TarEntry toTest)
		{
			if (toTest == null)
			{
				throw new ArgumentNullException("toTest");
			}
			return toTest.Name.StartsWith(this.Name);
		}

		public TarHeader TarHeader
		{
			get
			{
				return this.header;
			}
		}

		public string Name
		{
			get
			{
				return this.header.Name;
			}
			set
			{
				this.header.Name = value;
			}
		}

		public int UserId
		{
			get
			{
				return this.header.UserId;
			}
			set
			{
				this.header.UserId = value;
			}
		}

		public int GroupId
		{
			get
			{
				return this.header.GroupId;
			}
			set
			{
				this.header.GroupId = value;
			}
		}

		public string UserName
		{
			get
			{
				return this.header.UserName;
			}
			set
			{
				this.header.UserName = value;
			}
		}

		public string GroupName
		{
			get
			{
				return this.header.GroupName;
			}
			set
			{
				this.header.GroupName = value;
			}
		}

		public void SetIds(int userId, int groupId)
		{
			this.UserId = userId;
			this.GroupId = groupId;
		}

		public void SetNames(string userName, string groupName)
		{
			this.UserName = userName;
			this.GroupName = groupName;
		}

		public DateTime ModTime
		{
			get
			{
				return this.header.ModTime;
			}
			set
			{
				this.header.ModTime = value;
			}
		}

		public string File
		{
			get
			{
				return this.file;
			}
		}

		public long Size
		{
			get
			{
				return this.header.Size;
			}
			set
			{
				this.header.Size = value;
			}
		}

		public bool IsDirectory
		{
			get
			{
				if (this.file != null)
				{
					return Directory.Exists(this.file);
				}
				return this.header != null && (this.header.TypeFlag == 53 || this.Name.EndsWith("/"));
			}
		}

		public void GetFileTarHeader(TarHeader header, string file)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			this.file = file;
			string text = file;
			if (text.IndexOf(Environment.CurrentDirectory) == 0)
			{
				text = text.Substring(Environment.CurrentDirectory.Length);
			}
			text = text.Replace(Path.DirectorySeparatorChar, '/');
			while (text.StartsWith("/"))
			{
				text = text.Substring(1);
			}
			header.LinkName = string.Empty;
			header.Name = text;
			if (Directory.Exists(file))
			{
				header.Mode = 1003;
				header.TypeFlag = 53;
				if (header.Name.Length == 0 || header.Name[header.Name.Length - 1] != '/')
				{
					header.Name += "/";
				}
				header.Size = 0L;
			}
			else
			{
				header.Mode = 33216;
				header.TypeFlag = 48;
				header.Size = new FileInfo(file.Replace('/', Path.DirectorySeparatorChar)).Length;
			}
			header.ModTime = System.IO.File.GetLastWriteTime(file.Replace('/', Path.DirectorySeparatorChar)).ToUniversalTime();
			header.DevMajor = 0;
			header.DevMinor = 0;
		}

		public TarEntry[] GetDirectoryEntries()
		{
			if (this.file == null || !Directory.Exists(this.file))
			{
				return new TarEntry[0];
			}
			string[] fileSystemEntries = Directory.GetFileSystemEntries(this.file);
			TarEntry[] array = new TarEntry[fileSystemEntries.Length];
			for (int i = 0; i < fileSystemEntries.Length; i++)
			{
				array[i] = TarEntry.CreateEntryFromFile(fileSystemEntries[i]);
			}
			return array;
		}

		public void WriteEntryHeader(byte[] outBuffer)
		{
			this.header.WriteHeader(outBuffer);
		}

		public static void AdjustEntryName(byte[] buffer, string newName)
		{
			int offset = 0;
			TarHeader.GetNameBytes(newName, buffer, offset, 100);
		}

		public static void NameTarHeader(TarHeader header, string name)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			bool flag = name.EndsWith("/");
			header.Name = name;
			header.Mode = (flag ? 1003 : 33216);
			header.UserId = 0;
			header.GroupId = 0;
			header.Size = 0L;
			header.ModTime = DateTime.UtcNow;
            header.TypeFlag = flag ? TarHeader.LF_DIR : TarHeader.LF_NORMAL;
            header.LinkName = string.Empty;
			header.UserName = string.Empty;
			header.GroupName = string.Empty;
			header.DevMajor = 0;
			header.DevMinor = 0;
		}

		private string file;

		private TarHeader header;
	}
}
