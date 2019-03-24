using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Nexon.CSO.Extractor.Properties
{
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
	[CompilerGenerated]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		[DefaultSettingValue("False")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public bool AutoDecryptModels
		{
			get
			{
				return (bool)this["AutoDecryptModels"];
			}
			set
			{
				this["AutoDecryptModels"] = value;
			}
		}

		[DefaultSettingValue("Normal")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public FormWindowState FormWindowState
		{
			get
			{
				return (FormWindowState)this["FormWindowState"];
			}
			set
			{
				this["FormWindowState"] = value;
			}
		}

		[DefaultSettingValue("0, 0")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public Size FormSize
		{
			get
			{
				return (Size)this["FormSize"];
			}
			set
			{
				this["FormSize"] = value;
			}
		}

		[DefaultSettingValue("0")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public int SplitterDistance
		{
			get
			{
				return (int)this["SplitterDistance"];
			}
			set
			{
				this["SplitterDistance"] = value;
			}
		}

		[DefaultSettingValue("-1")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public int ColumnNameWidth
		{
			get
			{
				return (int)this["ColumnNameWidth"];
			}
			set
			{
				this["ColumnNameWidth"] = value;
			}
		}

		[DefaultSettingValue("-1")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public int ColumnLastModifiedWidth
		{
			get
			{
				return (int)this["ColumnLastModifiedWidth"];
			}
			set
			{
				this["ColumnLastModifiedWidth"] = value;
			}
		}

		[DefaultSettingValue("-1")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public int ColumnSizeWidth
		{
			get
			{
				return (int)this["ColumnSizeWidth"];
			}
			set
			{
				this["ColumnSizeWidth"] = value;
			}
		}

		[DefaultSettingValue("-1")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public int ColumnNamePosition
		{
			get
			{
				return (int)this["ColumnNamePosition"];
			}
			set
			{
				this["ColumnNamePosition"] = value;
			}
		}

		[DefaultSettingValue("-1")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public int ColumnLastModifiedPosition
		{
			get
			{
				return (int)this["ColumnLastModifiedPosition"];
			}
			set
			{
				this["ColumnLastModifiedPosition"] = value;
			}
		}

		[DefaultSettingValue("-1")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public int ColumnSizePosition
		{
			get
			{
				return (int)this["ColumnSizePosition"];
			}
			set
			{
				this["ColumnSizePosition"] = value;
			}
		}

		[DefaultSettingValue("0")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public int SortColumn
		{
			get
			{
				return (int)this["SortColumn"];
			}
			set
			{
				this["SortColumn"] = value;
			}
		}

		[DefaultSettingValue("Ascending")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public SortOrder SortColumnOrder
		{
			get
			{
				return (SortOrder)this["SortColumnOrder"];
			}
			set
			{
				this["SortColumnOrder"] = value;
			}
		}

		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
