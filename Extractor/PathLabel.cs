using System;
using System.Drawing;
using System.Windows.Forms;

namespace Nexon.CSO.Extractor
{
	public class PathLabel : Label
	{
		public PathLabel()
		{
			this.stringFormat.Trimming = StringTrimming.EllipsisPath;
			this.foreBrush = new SolidBrush(this.ForeColor);
		}

		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
				this.foreBrush.Dispose();
				this.foreBrush = new SolidBrush(value);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.stringFormat.Dispose();
				this.foreBrush.Dispose();
			}
			base.Dispose(disposing);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawString(this.Text, this.Font, this.foreBrush, base.ClientRectangle, this.stringFormat);
		}

		private StringFormat stringFormat = new StringFormat(StringFormatFlags.NoWrap);

		private SolidBrush foreBrush;
	}
}
