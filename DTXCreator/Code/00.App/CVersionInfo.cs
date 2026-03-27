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
			using (var font1 = new System.Drawing.Font( "Arial", 9f, FontStyle.Bold ))
			using (var brush1 = new SolidBrush( Color.FromArgb( 0xff, 179, 194, 251 ) ))
			{
				Rectangle layoutRectangle = new Rectangle( 8, 54, 400, 40 );
				e.Graphics.DrawString( "Copyright (c) 2000-2024 FROM/K.YAMASAKI All rights reserved.\nModified by fisyher", font1, brush1, layoutRectangle );
			}

			using (var font2 = new System.Drawing.Font( "MS US Gothic", 12f, FontStyle.Regular ))
			using (var brush2 = new SolidBrush( Color.FromArgb( 0xff, 220, 220, 220 ) ))
			{
				Rectangle layoutRectangle = new Rectangle( 330, 220, 150, 20 );
				e.Graphics.DrawString( Resources.DTXC_VERSION, font2, brush2, layoutRectangle );
			}
		}
		//-----------------
		#endregion
	}
}
