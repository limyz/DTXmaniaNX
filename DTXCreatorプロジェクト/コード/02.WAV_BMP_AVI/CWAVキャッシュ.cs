using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DTXCreator.WAV_BMP_AVI
{
	internal class CWAVキャッシュ
	{
		public Dictionary<int, CWAV> dicWAVディクショナリ = new Dictionary<int, CWAV>();
		public int n現在のキャッシュアイテム数
		{
			get
			{
				if( this.dicWAVディクショナリ == null )
				{
					return 0;
				}
				return this.dicWAVディクショナリ.Count;
			}
		}

		public CWAV tWAVをキャッシュから検索して返す( int nWAV番号1to1295 )
		{
			CWAV cwav;
			if( ( nWAV番号1to1295 < 1 ) || ( nWAV番号1to1295 > 36 * 36 - 1 ) )
			{
				throw new Exception( "WAV番号が範囲を超えています。-> [" + nWAV番号1to1295 + "]" );
			}
			if( this.dicWAVディクショナリ.TryGetValue( nWAV番号1to1295, out cwav ) )
			{
				return cwav;
			}
			return null;
		}
		public CWAV tWAVをキャッシュから検索して返す・なければ新規生成する( int nWAV番号1to1295 )
		{
			if( ( nWAV番号1to1295 < 1 ) || ( nWAV番号1to1295 > 36 * 36 - 1 ) )
			{
				throw new Exception( "WAV番号が範囲を超えています。-> [" + nWAV番号1to1295 + "]" );
			}
			CWAV cwav = null;
			if( !this.dicWAVディクショナリ.TryGetValue( nWAV番号1to1295, out cwav ) )
			{
				cwav = new CWAV();
				cwav.strラベル名 = "";
				cwav.nWAV番号1to1295 = nWAV番号1to1295;
				cwav.strファイル名 = "";
				cwav.n音量0to100 = 100;
				cwav.n位置_100to100 = 0;
				cwav.bBGMとして使用 = false;
				this.tキャッシュに追加する( cwav );
			}
			return cwav;
		}
		public void tWAVをキャッシュから削除する( int nWAV番号1to1295 )
		{
			if( ( nWAV番号1to1295 < 1 ) || ( nWAV番号1to1295 > 36 * 36 - 1 ) )
			{
				throw new Exception( "WAV番号が範囲を超えています。-> [" + nWAV番号1to1295 + "]" );
			}
			CWAV cwav = null;
			if( this.dicWAVディクショナリ.TryGetValue( nWAV番号1to1295, out cwav ) )
			{
				this.dicWAVディクショナリ.Remove( nWAV番号1to1295 );
			}
		}
		public void tキャッシュに追加する( CWAV 追加するセル )
		{
			CWAV cwav;
			if( this.dicWAVディクショナリ.TryGetValue( 追加するセル.nWAV番号1to1295, out cwav ) )
			{
				this.dicWAVディクショナリ.Remove( 追加するセル.nWAV番号1to1295 );
			}
			this.dicWAVディクショナリ.Add( 追加するセル.nWAV番号1to1295, 追加するセル );
		}
		public void tキャッシュに追加する( ListViewItem 追加するLVI )
		{
			CWAV cwav = new CWAV();
			cwav.tコピーfrom( 追加するLVI );
			this.tキャッシュに追加する( cwav );
		}
		public void t空にする()
		{
			this.dicWAVディクショナリ.Clear();
		}
	}
}
