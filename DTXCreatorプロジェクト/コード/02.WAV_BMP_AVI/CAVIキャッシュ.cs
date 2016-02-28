using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DTXCreator.WAV_BMP_AVI
{
	internal class CAVIキャッシュ
	{
		public Dictionary<int, CAVI> dicAVIディクショナリ = new Dictionary<int, CAVI>();
		public int n現在のキャッシュアイテム数
		{
			get
			{
				if( this.dicAVIディクショナリ == null )
				{
					return 0;
				}
				return this.dicAVIディクショナリ.Count;
			}
		}

		public CAVI tAVIをキャッシュから検索して返す( int nAVI番号1to1295 )
		{
			CAVI cavi;
			if( ( nAVI番号1to1295 < 1 ) || ( nAVI番号1to1295 > 36 * 36 - 1 ) )
			{
				throw new Exception( "AVI番号が範囲を超えています。-> [" + nAVI番号1to1295 + "]" );
			}
			if( this.dicAVIディクショナリ.TryGetValue( nAVI番号1to1295, out cavi ) )
			{
				return cavi;
			}
			return null;
		}
		public CAVI tAVIをキャッシュから検索して返す・なければ新規生成する( int nAVI番号1to1295 )
		{
			if( ( nAVI番号1to1295 < 1 ) || ( nAVI番号1to1295 > 36 * 36 - 1 ) )
			{
				throw new Exception( "AVI番号が範囲を超えています。-> [" + nAVI番号1to1295 + "]" );
			}
			CAVI cavi = null;
			if( !this.dicAVIディクショナリ.TryGetValue( nAVI番号1to1295, out cavi ) )
			{
				cavi = new CAVI();
				cavi.nAVI番号1to1295 = nAVI番号1to1295;
				this.tキャッシュに追加する( cavi );
			}
			return cavi;
		}
		public void tAVIをキャッシュから削除する( int nAVI番号1to1295 )
		{
			if( ( nAVI番号1to1295 < 1 ) || ( nAVI番号1to1295 > 36 * 36 - 1 ) )
			{
				throw new Exception( "AVI番号が範囲を超えています。-> [" + nAVI番号1to1295 + "]" );
			}
			CAVI cavi = null;
			if( this.dicAVIディクショナリ.TryGetValue( nAVI番号1to1295, out cavi ) )
			{
				this.dicAVIディクショナリ.Remove( nAVI番号1to1295 );
			}
		}
		public void tキャッシュに追加する( CAVI ac追加するAVI )
		{
			CAVI cavi;
			if( this.dicAVIディクショナリ.TryGetValue( ac追加するAVI.nAVI番号1to1295, out cavi ) )
			{
				this.dicAVIディクショナリ.Remove( ac追加するAVI.nAVI番号1to1295 );
			}
			this.dicAVIディクショナリ.Add( ac追加するAVI.nAVI番号1to1295, ac追加するAVI );
		}
		public void tキャッシュに追加する( ListViewItem lvi )
		{
			CAVI cavi = new CAVI();
			cavi.tコピーfrom( lvi );
			this.tキャッシュに追加する( cavi );
		}
		public void t空にする()
		{
			this.dicAVIディクショナリ.Clear();
		}
	}
}
