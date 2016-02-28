using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using DTXCreator.Properties;

namespace DTXCreator.汎用
{
	public partial class Cメッセージポップアップ : Form
	{
		public Cメッセージポップアップ( string strメッセージ )
		{
			this.InitializeComponent();
			this.strメッセージ = strメッセージ;
			this.ftフォント = new Font( "MS PGothic", 10f );
		}

		private string strメッセージ;
		private Font ftフォント;

		private void Cメッセージポップアップ_FormClosing( object sender, FormClosingEventArgs e )
		{
			this.ftフォント.Dispose();
		}
		private void Cメッセージポップアップ_Load( object sender, EventArgs e )
		{
			base.Location = new Point( base.Owner.Location.X + ( ( base.Owner.Width - base.Width ) / 2 ), base.Owner.Location.Y + ( ( base.Owner.Height - base.Height ) / 2 ) );
		}
		private void panelメッセージ_Paint( object sender, PaintEventArgs e )
		{
			SolidBrush brush = new SolidBrush( Color.Black );
			RectangleF layoutRectangle = new RectangleF( 0f, 0f, (float) this.panelメッセージ.ClientSize.Width, (float) this.panelメッセージ.ClientSize.Height );
			StringFormat format = new StringFormat();
			format.LineAlignment = StringAlignment.Center;
			format.Alignment = StringAlignment.Near;
			e.Graphics.DrawString( this.strメッセージ, this.ftフォント, brush, layoutRectangle, format );
			brush.Dispose();
		}
	}
}
