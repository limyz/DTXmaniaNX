using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using FDK;

namespace DTXCreator.WAV_BMP_AVI
{
	internal class CAVI
	{
		public Color col背景色 = SystemColors.Window;
		public Color col文字色 = SystemColors.WindowText;
		public int nAVI番号1to1295 = 1;
		public string strファイル名 = "";
		public string strラベル名 = "";

		public bool b内容が同じ・AVI番号を除くwith( CAVI ac )
		{
			return ( ( this.strラベル名.Equals( ac.strラベル名 ) && this.strファイル名.Equals( ac.strファイル名 ) ) && ( ( this.col文字色 == ac.col文字色 ) && ( this.col背景色 == ac.col背景色 ) ) );
		}
		public bool b内容が同じwith( CAVI ac )
		{
			return ( ( this.strラベル名.Equals( ac.strラベル名 ) && ( this.nAVI番号1to1295 == ac.nAVI番号1to1295 ) ) && ( ( this.strファイル名.Equals( ac.strファイル名 ) && ( this.col文字色 == ac.col文字色 ) ) && ( this.col背景色 == ac.col背景色 ) ) );
		}
		public void tコピーfrom( CAVI ac )
		{
			this.strラベル名 = ac.strラベル名;
			if( ( ac.nAVI番号1to1295 < 1 ) || ( ac.nAVI番号1to1295 > 36 * 36 - 1 ) )
			{
				throw new Exception( "AVI番号が範囲を超えています。-> [" + this.nAVI番号1to1295 + "]" );
			}
			this.nAVI番号1to1295 = ac.nAVI番号1to1295;
			this.strファイル名 = ac.strファイル名;
			this.col背景色 = ac.col背景色;
			this.col文字色 = ac.col文字色;
		}
		public void tコピーfrom( ListViewItem lvi )
		{
			this.strラベル名 = lvi.SubItems[ 0 ].Text;
			this.nAVI番号1to1295 = C変換.n36進数2桁の文字列を数値に変換して返す( lvi.SubItems[ 1 ].Text );
			this.strファイル名 = lvi.SubItems[ 2 ].Text;
			this.col背景色 = lvi.BackColor;
			this.col文字色 = lvi.ForeColor;
		}
		public void tコピーto( ListViewItem lvi )
		{
			lvi.SubItems[ 0 ].Text = this.strラベル名;
			lvi.SubItems[ 1 ].Text = C変換.str数値を36進数2桁に変換して返す( this.nAVI番号1to1295 );
			lvi.SubItems[ 2 ].Text = this.strファイル名;
			lvi.ForeColor = this.col文字色;
			lvi.BackColor = this.col背景色;
		}
		public ListViewItem t現在の内容から新しいListViewItemを作成して返す()
		{
			ListViewItem item = new ListViewItem( new string[] { this.strラベル名, C変換.str数値を36進数2桁に変換して返す( this.nAVI番号1to1295 ), this.strファイル名 } );
			item.ForeColor = this.col文字色;
			item.BackColor = this.col背景色;
			return item;
		}
	}
}
