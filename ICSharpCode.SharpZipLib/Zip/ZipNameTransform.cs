using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	public class ZipNameTransform : INameTransform
	{
		public ZipNameTransform()
		{
		}

		public ZipNameTransform(string trimPrefix)
		{
			this.TrimPrefix = trimPrefix;
		}

		static ZipNameTransform()
		{
			char[] invalidPathChars = Path.GetInvalidPathChars();
			int num = invalidPathChars.Length + 2;
			ZipNameTransform.InvalidEntryCharsRelaxed = new char[num];
			Array.Copy(invalidPathChars, 0, ZipNameTransform.InvalidEntryCharsRelaxed, 0, invalidPathChars.Length);
			ZipNameTransform.InvalidEntryCharsRelaxed[num - 1] = '*';
			ZipNameTransform.InvalidEntryCharsRelaxed[num - 2] = '?';
			num = invalidPathChars.Length + 4;
			ZipNameTransform.InvalidEntryChars = new char[num];
			Array.Copy(invalidPathChars, 0, ZipNameTransform.InvalidEntryChars, 0, invalidPathChars.Length);
			ZipNameTransform.InvalidEntryChars[num - 1] = ':';
			ZipNameTransform.InvalidEntryChars[num - 2] = '\\';
			ZipNameTransform.InvalidEntryChars[num - 3] = '*';
			ZipNameTransform.InvalidEntryChars[num - 4] = '?';
		}

		public string TransformDirectory(string name)
		{
			name = this.TransformFile(name);
			if (name.Length > 0)
			{
				if (!name.EndsWith("/"))
				{
					name += "/";
				}
				return name;
			}
			throw new ZipException("Cannot have an empty directory name");
		}

		public string TransformFile(string name)
		{
			if (name != null)
			{
				string text = name.ToLower();
				if (this.trimPrefix_ != null && text.IndexOf(this.trimPrefix_) == 0)
				{
					name = name.Substring(this.trimPrefix_.Length);
				}
				name = name.Replace("\\", "/");
				name = WindowsPathUtils.DropPathRoot(name);
				while (name.Length > 0)
				{
					if (name[0] != '/')
					{
						break;
					}
					name = name.Remove(0, 1);
				}
				while (name.Length > 0 && name[name.Length - 1] == '/')
				{
					name = name.Remove(name.Length - 1, 1);
				}
				for (int i = name.IndexOf("//"); i >= 0; i = name.IndexOf("//"))
				{
					name = name.Remove(i, 1);
				}
				name = ZipNameTransform.MakeValidName(name, '_');
			}
			else
			{
				name = string.Empty;
			}
			return name;
		}

		public string TrimPrefix
		{
			get
			{
				return this.trimPrefix_;
			}
			set
			{
				this.trimPrefix_ = value;
				if (this.trimPrefix_ != null)
				{
					this.trimPrefix_ = this.trimPrefix_.ToLower();
				}
			}
		}

		private static string MakeValidName(string name, char replacement)
		{
			int i = name.IndexOfAny(ZipNameTransform.InvalidEntryChars);
			if (i >= 0)
			{
				StringBuilder stringBuilder = new StringBuilder(name);
				while (i >= 0)
				{
					stringBuilder[i] = replacement;
					if (i >= name.Length)
					{
						i = -1;
					}
					else
					{
						i = name.IndexOfAny(ZipNameTransform.InvalidEntryChars, i + 1);
					}
				}
				name = stringBuilder.ToString();
			}
			if (name.Length > 65535)
			{
				throw new PathTooLongException();
			}
			return name;
		}

		public static bool IsValidName(string name, bool relaxed)
		{
			bool flag = name != null;
			if (flag)
			{
				if (relaxed)
				{
					flag = (name.IndexOfAny(ZipNameTransform.InvalidEntryCharsRelaxed) < 0);
				}
				else
				{
					flag = (name.IndexOfAny(ZipNameTransform.InvalidEntryChars) < 0 && name.IndexOf('/') != 0);
				}
			}
			return flag;
		}

		public static bool IsValidName(string name)
		{
			return name != null && name.IndexOfAny(ZipNameTransform.InvalidEntryChars) < 0 && name.IndexOf('/') != 0;
		}

		private string trimPrefix_;

		private static readonly char[] InvalidEntryChars;

		private static readonly char[] InvalidEntryCharsRelaxed;
	}
}
