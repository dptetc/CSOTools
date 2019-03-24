using System;
using System.Windows.Forms;

namespace Nexon.CSO.ModelDecryptor
{
	internal static class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			MainForm modelDecryptorForm = new MainForm();
			foreach (string arg in args)
			{
				modelDecryptorForm.AddInputItem(arg);
			}
			Application.Run(modelDecryptorForm);
		}
	}
}
