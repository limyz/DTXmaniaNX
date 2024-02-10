using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using FDK;

namespace DTXCreator.WAV_BMP_AVI
{
	internal class CBMP
	{
		public bool bテクスチャ;
		public Color col背景色 = SystemColors.Window;
		public Color col文字色 = SystemColors.WindowText;
		public int nBMP番号1to3843 = 1;
		public string strファイル名 = "";
		public string strラベル名 = "";

		public bool b内容が同じ_BMP番号を除くwith( CBMP bc )
		{
			return ( ( this.strラベル名.Equals( bc.strラベル名 ) && this.strファイル名.Equals( bc.strファイル名 ) ) && ( ( ( this.bテクスチャ == bc.bテクスチャ ) && ( this.col文字色 == bc.col文字色 ) ) && ( this.col背景色 == bc.col背景色 ) ) );
		}
		public bool b内容が同じwith( CBMP bc )
		{
			return ( ( ( this.strラベル名.Equals( bc.strラベル名 ) && ( this.nBMP番号1to3843 == bc.nBMP番号1to3843 ) ) && ( this.strファイル名.Equals( bc.strファイル名 ) && ( this.bテクスチャ == bc.bテクスチャ ) ) ) && ( ( this.col文字色 == bc.col文字色 ) && ( this.col背景色 == bc.col背景色 ) ) );
		}
		public void tコピーfrom( CBMP bc )
		{
			this.bテクスチャ = bc.bテクスチャ;
			this.strラベル名 = bc.strラベル名;
			if( ( bc.nBMP番号1to3843 < 1 ) || ( bc.nBMP番号1to3843 > 62 * 62 - 1 ) )
			{
				throw new Exception( "BMP番号が範囲を超えています。-> [" + this.nBMP番号1to3843 + "]" );
			}
			this.nBMP番号1to3843 = bc.nBMP番号1to3843;
			this.strファイル名 = bc.strファイル名;
			this.col背景色 = bc.col背景色;
			this.col文字色 = bc.col文字色;
		}
		public void tコピーfrom( ListViewItem lvi )
		{
			this.bテクスチャ = lvi.SubItems[ 0 ].Text.Equals( "o" );
			this.strラベル名 = lvi.SubItems[ 1 ].Text;
			this.nBMP番号1to3843 = CConversion.nConvert2DigitBase62StringToNumber( lvi.SubItems[ 2 ].Text );
			this.strファイル名 = lvi.SubItems[ 3 ].Text;
			this.col背景色 = lvi.BackColor;
			this.col文字色 = lvi.ForeColor;
		}
		public void tコピーto( ListViewItem lvi )
		{
			lvi.SubItems[ 0 ].Text = this.bテクスチャ ? "o" : "";
			lvi.SubItems[ 1 ].Text = this.strラベル名;
			lvi.SubItems[ 2 ].Text = CConversion.strConvertNumberTo2DigitBase62String( this.nBMP番号1to3843 );
			lvi.SubItems[ 3 ].Text = this.strファイル名;
			lvi.ForeColor = this.col文字色;
			lvi.BackColor = this.col背景色;
		}
		public ListViewItem t現在の内容から新しいListViewItemを作成して返す()
		{
			ListViewItem item = new ListViewItem( new string[] { this.bテクスチャ ? "o" : "", this.strラベル名, CConversion.strConvertNumberTo2DigitBase62String( this.nBMP番号1to3843 ), this.strファイル名 } );
			item.ForeColor = this.col文字色;
			item.BackColor = this.col背景色;
			return item;
		}
	}
}
