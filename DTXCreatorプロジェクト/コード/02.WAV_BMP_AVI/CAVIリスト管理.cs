using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DTXCreator.譜面;
using DTXCreator.UndoRedo;

namespace DTXCreator.WAV_BMP_AVI
{
	internal class CAVIリスト管理
	{
		public int n現在選択中のItem番号0to1294 = -1;

		public CAVIリスト管理( Cメインフォーム pメインフォーム, ListView pListViewAVIリスト )
		{
			this._Form = pメインフォーム;
			this.listViewAVIリスト = pListViewAVIリスト;
		}
		public void tAVIリストにフォーカスを当てる()
		{
			this.listViewAVIリスト.Focus();
		}
		public CAVI tAVIをキャッシュから検索して返す( int nAVI番号1to1295 )
		{
			return this.AVIキャッシュ.tAVIをキャッシュから検索して返す( nAVI番号1to1295 );
		}
		public CAVI tAVIをキャッシュから検索して返す・なければ新規生成する( int nAVI番号1to1295 )
		{
			return this.AVIキャッシュ.tAVIをキャッシュから検索して返す・なければ新規生成する( nAVI番号1to1295 );
		}
		public ListViewItem tAVI番号に対応するListViewItemを返す( int nAVI番号1to1295 )
		{
			if( ( nAVI番号1to1295 < 1 ) || ( nAVI番号1to1295 > 36 * 36 - 1 ) )
			{
				throw new Exception( "AVI番号が範囲外です。--->[" + nAVI番号1to1295 + "]" );
			}
			return this.listViewAVIリスト.Items[ nAVI番号1to1295 - 1 ];
		}
		public void tAVI編集のRedo( CAVI ac変更前, CAVI ac変更後 )
		{
			int num = ac変更後.nAVI番号1to1295;
			CAVI cavi = this.AVIキャッシュ.tAVIをキャッシュから検索して返す( num );
			cavi.tコピーfrom( ac変更後 );
			cavi.tコピーto( this.listViewAVIリスト.Items[ num - 1 ] );
			this._Form.tWAV・BMP・AVIリストのカーソルを全部同じ行に合わせる( cavi.nAVI番号1to1295 - 1 );
			this._Form.tタブを選択する( Cメインフォーム.Eタブ種別.AVI );
			this.listViewAVIリスト.Refresh();
		}
		public void tAVI編集のUndo( CAVI ac変更前, CAVI ac変更後 )
		{
			int num = ac変更前.nAVI番号1to1295;
			CAVI cavi = this.AVIキャッシュ.tAVIをキャッシュから検索して返す( num );
			cavi.tコピーfrom( ac変更前 );
			cavi.tコピーto( this.listViewAVIリスト.Items[ num - 1 ] );
			this._Form.tWAV・BMP・AVIリストのカーソルを全部同じ行に合わせる( cavi.nAVI番号1to1295 - 1 );
			this._Form.tタブを選択する( Cメインフォーム.Eタブ種別.AVI );
			this.listViewAVIリスト.Refresh();
		}
		public ListViewItem tCAVIとListViewItemを生成して返す( int n行番号1to1295 )
		{
			return this.tAVIをキャッシュから検索して返す・なければ新規生成する( n行番号1to1295 ).t現在の内容から新しいListViewItemを作成して返す();
		}
		public void tItemを交換する( int nItem番号1, int nItem番号2 )
		{
			if( !CUndoRedo管理.bUndoRedoした直後 )
			{
				this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoセル<int>( null, new DGUndoを実行する<int>( this.t行交換のUndo ), new DGRedoを実行する<int>( this.t行交換のRedo ), nItem番号1, nItem番号2 ) );
				this._Form.tUndoRedo用GUIの有効・無効を設定する();
			}
			CUndoRedo管理.bUndoRedoした直後 = false;
			this.tItemを交換する・ListViewItem( nItem番号1, nItem番号2 );
			this.tItemを交換する・AVIキャッシュ( nItem番号1, nItem番号2 );
			this.tItemを交換する・チップパレット( nItem番号1, nItem番号2 );
			this.tItemを交換する・譜面上のチップ( nItem番号1, nItem番号2 );
			this.tItemを交換する・レーン割付チップ( nItem番号1, nItem番号2 );
			this.tItemを交換する・カーソル移動( nItem番号1, nItem番号2 );
			this._Form.listViewAVIリスト.Refresh();
			this._Form.pictureBox譜面パネル.Refresh();
			this._Form.b未保存 = true;
		}
		public void tItemを選択する( int nItem番号0to1294 )
		{
			this.n現在選択中のItem番号0to1294 = nItem番号0to1294;
			this.listViewAVIリスト.Items[ nItem番号0to1294 ].Selected = true;
			this.listViewAVIリスト.Items[ nItem番号0to1294 ].Focused = true;
		}
		public void tファイル名の相対パス化( string str基本フォルダ名 )
		{
			for( int i = 1; i <= 36 * 36 - 1; i++ )
			{
				CAVI cavi = this.AVIキャッシュ.tAVIをキャッシュから検索して返す( i );
				if( ( cavi != null ) && ( cavi.strファイル名.Length > 0 ) )
				{
					try
					{
						Uri uri = new Uri( str基本フォルダ名 );
						cavi.strファイル名 = Uri.UnescapeDataString( uri.MakeRelativeUri( new Uri( cavi.strファイル名 ) ).ToString() ).Replace( '/', '\\' );
					}
					catch( UriFormatException )
					{
					}
				}
			}
		}
		public void t行交換のRedo( int n変更前のItem番号0to1294, int n変更後のItem番号0to1294 )
		{
			CUndoRedo管理.bUndoRedoした直後 = true;
			this.tItemを交換する( n変更前のItem番号0to1294, n変更後のItem番号0to1294 );
		}
		public void t行交換のUndo( int n変更前のItem番号0to1294, int n変更後のItem番号0to1294 )
		{
			CUndoRedo管理.bUndoRedoした直後 = true;
			this.tItemを交換する( n変更前のItem番号0to1294, n変更後のItem番号0to1294 );
		}
		public void t新規生成のRedo( CAVI ac生成前はNull, CAVI ac生成されたAVIの複製 )
		{
			int num = ac生成されたAVIの複製.nAVI番号1to1295;
			CAVI cavi = this.AVIキャッシュ.tAVIをキャッシュから検索して返す・なければ新規生成する( num );
			cavi.tコピーfrom( ac生成されたAVIの複製 );
			cavi.tコピーto( this.listViewAVIリスト.Items[ num - 1 ] );
			this._Form.tタブを選択する( Cメインフォーム.Eタブ種別.AVI );
			this.listViewAVIリスト.Refresh();
		}
		public void t新規生成のUndo( CAVI ac生成前はNull, CAVI ac生成されたAVIの複製 )
		{
			int num = ac生成されたAVIの複製.nAVI番号1to1295;
			new CAVI().tコピーto( this.listViewAVIリスト.Items[ num - 1 ] );
			this.AVIキャッシュ.tAVIをキャッシュから削除する( num );
			this._Form.tタブを選択する( Cメインフォーム.Eタブ種別.AVI );
			this.listViewAVIリスト.Refresh();
		}
		public void t動画プロパティを開いて編集する( int nAVI番号1to1295, string str相対パスの基本フォルダ )
		{
			this._Form.dlgチップパレット.t一時的に隠蔽する();
			CAVI cavi = this.tAVIをキャッシュから検索して返す・なければ新規生成する( nAVI番号1to1295 );
			ListViewItem item = cavi.t現在の内容から新しいListViewItemを作成して返す();
			string directoryName = "";
			if( item.SubItems[ 2 ].Text.Length > 0 )
			{
				directoryName = Path.GetDirectoryName( this._Form.strファイルの存在するディレクトリを絶対パスで返す( item.SubItems[ 2 ].Text ) );
			}
			C動画プロパティダイアログ c動画プロパティダイアログ = new C動画プロパティダイアログ( str相対パスの基本フォルダ, directoryName );
			c動画プロパティダイアログ.avi = cavi;
			c動画プロパティダイアログ.textBoxラベル.Text = item.SubItems[ 0 ].Text;
			c動画プロパティダイアログ.textBoxAVI番号.Text = item.SubItems[ 1 ].Text;
			c動画プロパティダイアログ.textBoxファイル.Text = item.SubItems[ 2 ].Text;
			c動画プロパティダイアログ.textBoxAVI番号.ForeColor = item.ForeColor;
			c動画プロパティダイアログ.textBoxAVI番号.BackColor = item.BackColor;
			if( c動画プロパティダイアログ.ShowDialog() == DialogResult.OK )
			{
				CAVI avi = c動画プロパティダイアログ.avi;
				CAVI cavi3 = new CAVI();
				cavi3.nAVI番号1to1295 = c動画プロパティダイアログ.avi.nAVI番号1to1295;
				cavi3.strラベル名 = c動画プロパティダイアログ.textBoxラベル.Text;
				cavi3.strファイル名 = c動画プロパティダイアログ.textBoxファイル.Text;
				cavi3.col文字色 = c動画プロパティダイアログ.textBoxAVI番号.ForeColor;
				cavi3.col背景色 = c動画プロパティダイアログ.textBoxAVI番号.BackColor;
				if( !cavi3.b内容が同じwith( avi ) )
				{
					avi = new CAVI();
					avi.tコピーfrom( c動画プロパティダイアログ.avi );
					this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoセル<CAVI>( null, new DGUndoを実行する<CAVI>( this.tAVI編集のUndo ), new DGRedoを実行する<CAVI>( this.tAVI編集のRedo ), avi, cavi3 ) );
					this._Form.tUndoRedo用GUIの有効・無効を設定する();
					c動画プロパティダイアログ.avi.tコピーfrom( cavi3 );
					if( this.tAVI番号に対応するListViewItemを返す( nAVI番号1to1295 ) != null )
					{
						ListViewItem item2 = c動画プロパティダイアログ.avi.t現在の内容から新しいListViewItemを作成して返す();
						item = this.tAVI番号に対応するListViewItemを返す( nAVI番号1to1295 );
						item.SubItems[ 0 ].Text = item2.SubItems[ 0 ].Text;
						item.SubItems[ 1 ].Text = item2.SubItems[ 1 ].Text;
						item.SubItems[ 2 ].Text = item2.SubItems[ 2 ].Text;
						item.ForeColor = item2.ForeColor;
						item.BackColor = item2.BackColor;
					}
					this.listViewAVIリスト.Refresh();
					this._Form.b未保存 = true;
				}
			}
			this._Form.dlgチップパレット.t一時的な隠蔽を解除する();
		}

		#region [ private ]
		//-----------------
		private Cメインフォーム _Form;
		private CAVIキャッシュ AVIキャッシュ = new CAVIキャッシュ();
		private ListView listViewAVIリスト;

		private void tItemを交換する・AVIキャッシュ( int nItem番号1, int nItem番号2 )
		{
			int num = nItem番号1 + 1;
			int num2 = nItem番号2 + 1;
			CAVI ac = this.AVIキャッシュ.tAVIをキャッシュから検索して返す( num );
			CAVI cavi2 = this.AVIキャッシュ.tAVIをキャッシュから検索して返す( num2 );
			CAVI cavi3 = new CAVI();
			cavi3.tコピーfrom( ac );
			ac.tコピーfrom( cavi2 );
			ac.nAVI番号1to1295 = num;
			cavi2.tコピーfrom( cavi3 );
			cavi2.nAVI番号1to1295 = num2;
		}
		private void tItemを交換する・ListViewItem( int nItem番号1, int nItem番号2 )
		{
			int num = nItem番号1 + 1;
			int num2 = nItem番号2 + 1;
			CAVI cavi = new CAVI();
			cavi.tコピーfrom( this.listViewAVIリスト.Items[ nItem番号1 ] );
			cavi.nAVI番号1to1295 = num2;
			CAVI cavi2 = new CAVI();
			cavi2.tコピーfrom( this.listViewAVIリスト.Items[ nItem番号2 ] );
			cavi2.nAVI番号1to1295 = num;
			cavi2.tコピーto( this.listViewAVIリスト.Items[ nItem番号1 ] );
			cavi.tコピーto( this.listViewAVIリスト.Items[ nItem番号2 ] );
		}
		private void tItemを交換する・カーソル移動( int nItem番号1, int nItem番号2 )
		{
			this.tItemを選択する( nItem番号2 );
		}
		private void tItemを交換する・チップパレット( int nItem番号1, int nItem番号2 )
		{
			this._Form.dlgチップパレット.tパレットセルの番号を置換する( 2, nItem番号1 + 1, nItem番号2 + 1 );
		}
		private void tItemを交換する・レーン割付チップ( int nItem番号1, int nItem番号2 )
		{
			for( int i = 0; i < this._Form.mgr譜面管理者.listレーン.Count; i++ )
			{
				Cレーン cレーン = this._Form.mgr譜面管理者.listレーン[ i ];
				if( cレーン.eレーン種別 == Cレーン.E種別.AVI )
				{
					if( cレーン.nレーン割付チップ・表0or1to1295 == ( nItem番号1 + 1 ) )
					{
						cレーン.nレーン割付チップ・表0or1to1295 = nItem番号2 + 1;
					}
					else if( cレーン.nレーン割付チップ・表0or1to1295 == ( nItem番号2 + 1 ) )
					{
						cレーン.nレーン割付チップ・表0or1to1295 = nItem番号1 + 1;
					}
					if( cレーン.nレーン割付チップ・裏0or1to1295 == ( nItem番号1 + 1 ) )
					{
						cレーン.nレーン割付チップ・裏0or1to1295 = nItem番号2 + 1;
					}
					else if( cレーン.nレーン割付チップ・裏0or1to1295 == ( nItem番号2 + 1 ) )
					{
						cレーン.nレーン割付チップ・裏0or1to1295 = nItem番号1 + 1;
					}
				}
			}
		}
		private void tItemを交換する・譜面上のチップ( int nItem番号1, int nItem番号2 )
		{
			foreach( KeyValuePair<int, C小節> pair in this._Form.mgr譜面管理者.dic小節 )
			{
				C小節 c小節 = pair.Value;
				for( int i = 0; i < c小節.listチップ.Count; i++ )
				{
					Cチップ cチップ = c小節.listチップ[ i ];
					if( this._Form.mgr譜面管理者.listレーン[ cチップ.nレーン番号0to ].eレーン種別 == Cレーン.E種別.AVI )
					{
						if( cチップ.n値・整数1to1295 == ( nItem番号1 + 1 ) )
						{
							cチップ.n値・整数1to1295 = nItem番号2 + 1;
						}
						else if( cチップ.n値・整数1to1295 == ( nItem番号2 + 1 ) )
						{
							cチップ.n値・整数1to1295 = nItem番号1 + 1;
						}
					}
				}
			}
		}
		//-----------------
		#endregion
	}
}
