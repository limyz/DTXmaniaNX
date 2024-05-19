using System;
using System.Windows.Forms;
using System.Drawing;
using DTXCreator.Properties;

namespace DTXCreator
{
	public partial class CVersionInfo : Form  // Cバージョン情報
	{
		public CVersionInfo()
		{
			this.InitializeComponent();
		}

		#region [ private ]
		//-----------------
		private void Cバージョン情報_Click( object sender, EventArgs e )
		{
			base.Close();
		}
		private void Cバージョン情報_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Escape )
			{
				base.Close();
			}
		}
		private void Cバージョン情報_Paint( object sender, PaintEventArgs e )
		{
			Font font = new System.Drawing.Font( "Arial", 9f, FontStyle.Bold );
			Brush brush = new SolidBrush( Color.FromArgb( 0xff, 179, 194, 251 ) );
			Rectangle layoutRectangle = new Rectangle( 8, 54, 400, 40 );
			e.Graphics.DrawString( "Copyright (c) 2000-2024 FROM/K.YAMASAKI All rights reserved.\nModified by fisyher", font, brush, layoutRectangle );
			brush.Dispose();
			font.Dispose();

			font = new System.Drawing.Font( "MS US Gothic", 12f, FontStyle.Regular );
			brush = new SolidBrush( Color.FromArgb( 0xff, 220, 220, 220 ) );
			layoutRectangle = new Rectangle( 330, 220, 150, 20 );
			e.Graphics.DrawString( Resources.DTXC_VERSION, font, brush, layoutRectangle );
            brush.Dispose();
			font.Dispose();
		}
		//-----------------
		#endregion
	}
}
