using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using DTXCreator.Properties;

namespace DTXCreator
{
	public partial class Cバージョン情報 : Form
	{
		public Cバージョン情報()
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
			Font font = new System.Drawing.Font( "Arial", 8f, FontStyle.Bold );
			Brush brush = new SolidBrush( Color.FromArgb( 0xff, 179, 194, 251 ) );
			Rectangle layoutRectangle = new Rectangle( 8, 54, 400, 20 );
			e.Graphics.DrawString( "Copyright (c) 2000-2011 FROM/K.YAMASAKI All rights reserved.", font, brush, layoutRectangle );
			brush.Dispose();
			font.Dispose();

			font = new System.Drawing.Font( "MS US Gothic", 12f, FontStyle.Regular );
			brush = new SolidBrush( Color.FromArgb( 0xff, 220, 220, 220 ) );
			layoutRectangle = new Rectangle( 330, 220, 150, 20 );
			e.Graphics.DrawString( "Release " + Resources.DTXC_VERSION, font, brush, layoutRectangle );
            brush.Dispose();
			font.Dispose();
		}
		//-----------------
		#endregion
	}
}
