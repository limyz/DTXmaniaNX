using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using FDK;

namespace DTXCreator.WAV_BMP_AVI
{
	internal class CWAV
	{
		public bool bBGMとして使用;
		public Color col背景色 = SystemColors.Window;
		public Color col文字色 = SystemColors.WindowText;
		public int nWAV番号1to3843 = 1;
		public int n位置_0to127;
		public int n音量0to127 = 127;
		public string strファイル名 = "";
		public string strラベル名 = "";

		public bool b内容が同じ_WAV番号を除くwith( CWAV wc )
		{
			return ( ( ( this.strラベル名.Equals( wc.strラベル名 ) && this.strファイル名.Equals( wc.strファイル名 ) ) && ( ( this.n音量0to127 == wc.n音量0to127 ) && ( this.n位置_0to127 == wc.n位置_0to127 ) ) ) && ( ( ( this.bBGMとして使用 == wc.bBGMとして使用 ) && ( this.col文字色 == wc.col文字色 ) ) && ( this.col背景色 == wc.col背景色 ) ) );
		}
		public bool b内容が同じwith( CWAV wc )
		{
			return ( ( ( this.strラベル名.Equals( wc.strラベル名 ) && ( this.nWAV番号1to3843 == wc.nWAV番号1to3843 ) ) && ( this.strファイル名.Equals( wc.strファイル名 ) && ( this.n音量0to127 == wc.n音量0to127 ) ) ) && ( ( ( this.n位置_0to127 == wc.n位置_0to127 ) && ( this.bBGMとして使用 == wc.bBGMとして使用 ) ) && ( ( this.col文字色 == wc.col文字色 ) && ( this.col背景色 == wc.col背景色 ) ) ) );
		}
		public void tコピーfrom( CWAV wc )
		{
			this.strラベル名 = wc.strラベル名;
			if( ( wc.nWAV番号1to3843 < 1 ) || ( wc.nWAV番号1to3843 > 62 * 62 - 1 ) )
			{
				throw new Exception( "WAV番号が範囲を超えています。-> [" + this.nWAV番号1to3843 + "]" );
			}
			this.nWAV番号1to3843 = wc.nWAV番号1to3843;
			this.strファイル名 = wc.strファイル名;
			if( ( wc.n音量0to127 < 0 ) || ( wc.n音量0to127 > 127 ) )
			{
				throw new Exception( "音量が範囲を超えています。-> [" + this.n音量0to127 + "]" );
			}
			this.n音量0to127 = wc.n音量0to127;
			if( ( wc.n位置_0to127 < -127 ) || ( wc.n位置_0to127 > 127 ) )
			{
				throw new Exception( "位置が範囲を超えています。-> [" + this.n位置_0to127 + "]" );
			}
			this.n位置_0to127 = wc.n位置_0to127;
			this.bBGMとして使用 = wc.bBGMとして使用;
			this.col文字色 = wc.col文字色;
			this.col背景色 = wc.col背景色;
		}
		public void tコピーfrom( ListViewItem lvi )
		{
			this.strラベル名 = lvi.SubItems[ 0 ].Text;
			this.nWAV番号1to3843 = CConversion.nConvert2DigitBase62StringToNumber( lvi.SubItems[ 1 ].Text );
			this.strファイル名 = lvi.SubItems[ 2 ].Text;
			if( !int.TryParse( lvi.SubItems[ 3 ].Text, out this.n音量0to127 ) )
			{
				this.n音量0to127 = 127;
			}
			if( ( this.n音量0to127 < 0 ) || ( this.n音量0to127 > 127 ) )
			{
				throw new Exception( "音量の値が範囲を超えています。-> [" + this.n音量0to127 + "]" );
			}
			if( !int.TryParse( lvi.SubItems[ 4 ].Text, out this.n位置_0to127 ) )
			{
				this.n位置_0to127 = 0;
			}
			if( ( this.n位置_0to127 < -127 ) || ( this.n位置_0to127 > 127 ) )
			{
				throw new Exception( "位置の値が範囲を超えています。-> [" + this.n音量0to127 + "]" );
			}
			this.bBGMとして使用 = lvi.SubItems[ 5 ].Text.Equals( "o" );
			this.col文字色 = lvi.ForeColor;
			this.col背景色 = lvi.BackColor;
		}
		public void tコピーto( ListViewItem lvi )
		{
			lvi.SubItems[ 0 ].Text = this.strラベル名;
			lvi.SubItems[ 1 ].Text = CConversion.strConvertNumberTo2DigitBase62String( this.nWAV番号1to3843 );
			lvi.SubItems[ 2 ].Text = this.strファイル名;
			lvi.SubItems[ 3 ].Text = this.n音量0to127.ToString();
			lvi.SubItems[ 4 ].Text = this.n位置_0to127.ToString();
			lvi.SubItems[ 5 ].Text = this.bBGMとして使用 ? "o" : "";
			lvi.ForeColor = this.col文字色;
			lvi.BackColor = this.col背景色;
		}
		public ListViewItem t現在の内容から新しいListViewItemを作成して返す()
		{
			ListViewItem item = new ListViewItem( new string[] { this.strラベル名, CConversion.strConvertNumberTo2DigitBase62String( this.nWAV番号1to3843 ), this.strファイル名, this.n音量0to127.ToString(), this.n位置_0to127.ToString(), this.bBGMとして使用 ? "o" : "" } );
			item.ForeColor = this.col文字色;
			item.BackColor = this.col背景色;
			return item;
		}
	}
}
