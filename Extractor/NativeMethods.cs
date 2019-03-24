using System;
using System.Runtime.InteropServices;

namespace Nexon.CSO.Extractor
{
	internal static class NativeMethods
	{
		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref NativeMethods.LVCOLUMN lPLVCOLUMN);

		public const int HDI_FORMAT = 4;

		public const int HDF_SORTUP = 1024;

		public const int HDF_SORTDOWN = 512;

		public const int LVM_GETHEADER = 4127;

		public const int HDM_GETITEM = 4619;

		public const int HDM_SETITEM = 4620;

		public struct LVCOLUMN
		{
			public int mask;

			public int cx;

			[MarshalAs(UnmanagedType.LPTStr)]
			public string pszText;

			public IntPtr hbm;

			public int cchTextMax;

			public int fmt;

			public int iSubItem;

			public int iImage;

			public int iOrder;
		}
	}
}
