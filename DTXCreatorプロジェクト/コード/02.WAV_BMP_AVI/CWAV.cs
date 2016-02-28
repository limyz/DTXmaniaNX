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
		public int nWAV番号1to1295 = 1;
		public int n位置_100to100;
		public int n音量0to100 = 100;
		public string strファイル名 = "";
		public string strラベル名 = "";

		public bool b内容が同じ・WAV番号を除くwith( CWAV wc )
		{
			return ( ( ( this.strラベル名.Equals( wc.strラベル名 ) && this.strファイル名.Equals( wc.strファイル名 ) ) && ( ( this.n音量0to100 == wc.n音量0to100 ) && ( this.n位置_100to100 == wc.n位置_100to100 ) ) ) && ( ( ( this.bBGMとして使用 == wc.bBGMとして使用 ) && ( this.col文字色 == wc.col文字色 ) ) && ( this.col背景色 == wc.col背景色 ) ) );
		}
		public bool b内容が同じwith( CWAV wc )
		{
			return ( ( ( this.strラベル名.Equals( wc.strラベル名 ) && ( this.nWAV番号1to1295 == wc.nWAV番号1to1295 ) ) && ( this.strファイル名.Equals( wc.strファイル名 ) && ( this.n音量0to100 == wc.n音量0to100 ) ) ) && ( ( ( this.n位置_100to100 == wc.n位置_100to100 ) && ( this.bBGMとして使用 == wc.bBGMとして使用 ) ) && ( ( this.col文字色 == wc.col文字色 ) && ( this.col背景色 == wc.col背景色 ) ) ) );
		}
		public void tコピーfrom( CWAV wc )
		{
			this.strラベル名 = wc.strラベル名;
			if( ( wc.nWAV番号1to1295 < 1 ) || ( wc.nWAV番号1to1295 > 36 * 36 - 1 ) )
			{
				throw new Exception( "WAV番号が範囲を超えています。-> [" + this.nWAV番号1to1295 + "]" );
			}
			this.nWAV番号1to1295 = wc.nWAV番号1to1295;
			this.strファイル名 = wc.strファイル名;
			if( ( wc.n音量0to100 < 0 ) || ( wc.n音量0to100 > 100 ) )
			{
				throw new Exception( "音量が範囲を超えています。-> [" + this.n音量0to100 + "]" );
			}
			this.n音量0to100 = wc.n音量0to100;
			if( ( wc.n位置_100to100 < -100 ) || ( wc.n位置_100to100 > 100 ) )
			{
				throw new Exception( "位置が範囲を超えています。-> [" + this.n位置_100to100 + "]" );
			}
			this.n位置_100to100 = wc.n位置_100to100;
			this.bBGMとして使用 = wc.bBGMとして使用;
			this.col文字色 = wc.col文字色;
			this.col背景色 = wc.col背景色;
		}
		public void tコピーfrom( ListViewItem lvi )
		{
			this.strラベル名 = lvi.SubItems[ 0 ].Text;
			this.nWAV番号1to1295 = C変換.n36進数2桁の文字列を数値に変換して返す( lvi.SubItems[ 1 ].Text );
			this.strファイル名 = lvi.SubItems[ 2 ].Text;
			if( !int.TryParse( lvi.SubItems[ 3 ].Text, out this.n音量0to100 ) )
			{
				this.n音量0to100 = 100;
			}
			if( ( this.n音量0to100 < 0 ) || ( this.n音量0to100 > 100 ) )
			{
				throw new Exception( "音量の値が範囲を超えています。-> [" + this.n音量0to100 + "]" );
			}
			if( !int.TryParse( lvi.SubItems[ 4 ].Text, out this.n位置_100to100 ) )
			{
				this.n位置_100to100 = 0;
			}
			if( ( this.n位置_100to100 < -100 ) || ( this.n位置_100to100 > 100 ) )
			{
				throw new Exception( "位置の値が範囲を超えています。-> [" + this.n音量0to100 + "]" );
			}
			this.bBGMとして使用 = lvi.SubItems[ 5 ].Text.Equals( "o" );
			this.col文字色 = lvi.ForeColor;
			this.col背景色 = lvi.BackColor;
		}
		public void tコピーto( ListViewItem lvi )
		{
			lvi.SubItems[ 0 ].Text = this.strラベル名;
			lvi.SubItems[ 1 ].Text = C変換.str数値を36進数2桁に変換して返す( this.nWAV番号1to1295 );
			lvi.SubItems[ 2 ].Text = this.strファイル名;
			lvi.SubItems[ 3 ].Text = this.n音量0to100.ToString();
			lvi.SubItems[ 4 ].Text = this.n位置_100to100.ToString();
			lvi.SubItems[ 5 ].Text = this.bBGMとして使用 ? "o" : "";
			lvi.ForeColor = this.col文字色;
			lvi.BackColor = this.col背景色;
		}
		public ListViewItem t現在の内容から新しいListViewItemを作成して返す()
		{
			ListViewItem item = new ListViewItem( new string[] { this.strラベル名, C変換.str数値を36進数2桁に変換して返す( this.nWAV番号1to1295 ), this.strファイル名, this.n音量0to100.ToString(), this.n位置_100to100.ToString(), this.bBGMとして使用 ? "o" : "" } );
			item.ForeColor = this.col文字色;
			item.BackColor = this.col背景色;
			return item;
		}
	}
}
