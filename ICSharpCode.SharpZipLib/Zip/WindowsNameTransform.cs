using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	public class WindowsNameTransform : INameTransform
	{
		public WindowsNameTransform(string baseDirectory)
		{
			if (baseDirectory == null)
			{
				throw new ArgumentNullException("baseDirectory", "Directory name is invalid");
			}
			this.BaseDirectory = baseDirectory;
		}

		public WindowsNameTransform()
		{
		}

		public string BaseDirectory
		{
			get
			{
				return this.baseDirectory_;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.baseDirectory_ = Path.GetFullPath(value);
			}
		}

		public bool TrimIncomingPaths
		{
			get
			{
				return this.trimIncomingPaths_;
			}
			set
			{
				this.trimIncomingPaths_ = value;
			}
		}

		public string TransformDirectory(string name)
		{
			name = this.TransformFile(name);
			if (name.Length > 0)
			{
				while (name.EndsWith("\\"))
				{
					name = name.Remove(name.Length - 1, 1);
				}
				return name;
			}
			throw new ZipException("Cannot have an empty directory name");
		}

		public string TransformFile(string name)
		{
			if (name != null)
			{
				name = WindowsNameTransform.MakeValidName(name, this.replacementChar_);
				if (this.trimIncomingPaths_)
				{
					name = Path.GetFileName(name);
				}
				if (this.baseDirectory_ != null)
				{
					name = Path.Combine(this.baseDirectory_, name);
				}
			}
			else
			{
				name = string.Empty;
			}
			return name;
		}

		public static bool IsValidName(string name)
		{
			return name != null && name.Length <= 260 && string.Compare(name, WindowsNameTransform.MakeValidName(name, '_')) == 0;
		}

		static WindowsNameTransform()
		{
			char[] invalidPathChars = Path.GetInvalidPathChars();
			int num = invalidPathChars.Length + 3;
			WindowsNameTransform.InvalidEntryChars = new char[num];
			Array.Copy(invalidPathChars, 0, WindowsNameTransform.InvalidEntryChars, 0, invalidPathChars.Length);
			WindowsNameTransform.InvalidEntryChars[num - 1] = '*';
			WindowsNameTransform.InvalidEntryChars[num - 2] = '?';
			WindowsNameTransform.InvalidEntryChars[num - 2] = ':';
		}

		public static string MakeValidName(string name, char replacement)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			name = WindowsPathUtils.DropPathRoot(name.Replace("/", "\\"));
			while (name.Length > 0)
			{
				if (name[0] != '\\')
				{
					break;
				}
				name = name.Remove(0, 1);
			}
			while (name.Length > 0 && name[name.Length - 1] == '\\')
			{
				name = name.Remove(name.Length - 1, 1);
			}
			int i;
			for (i = name.IndexOf("\\\\"); i >= 0; i = name.IndexOf("\\\\"))
			{
				name = name.Remove(i, 1);
			}
			i = name.IndexOfAny(WindowsNameTransform.InvalidEntryChars);
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
						i = name.IndexOfAny(WindowsNameTransform.InvalidEntryChars, i + 1);
					}
				}
				name = stringBuilder.ToString();
			}
			if (name.Length > 260)
			{
				throw new PathTooLongException();
			}
			return name;
		}

		public char Replacement
		{
			get
			{
				return this.replacementChar_;
			}
			set
			{
				for (int i = 0; i < WindowsNameTransform.InvalidEntryChars.Length; i++)
				{
					if (WindowsNameTransform.InvalidEntryChars[i] == value)
					{
						throw new ArgumentException("invalid path character");
					}
				}
				if (value == '\\' || value == '/')
				{
					throw new ArgumentException("invalid replacement character");
				}
				this.replacementChar_ = value;
			}
		}

		private const int MaxPath = 260;

		private string baseDirectory_;

		private bool trimIncomingPaths_;

		private char replacementChar_ = '_';

		private static readonly char[] InvalidEntryChars;
	}
}
