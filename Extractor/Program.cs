using System;
using System.Windows.Forms;

namespace Nexon.CSO.Extractor
{
	internal static class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			MainForm extractorForm = new MainForm();
			if (args.Length > 0)
			{
				extractorForm.InitialLoadFile = args[0];
			}
			Application.Run(extractorForm);
		}
	}
}
