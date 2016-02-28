using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DTXCreator.譜面;
using DTXCreator.UndoRedo;

namespace DTXCreator.WAV_BMP_AVI
{
	internal class CBMPリスト管理
	{
		public int n現在選択中のItem番号0to1294 = -1;

		public CBMPリスト管理( Cメインフォーム sメインフォーム, ListView pListViewBMPリスト )
		{
			this._Form = sメインフォーム;
			this.listViewBMPリスト = pListViewBMPリスト;
		}
		public void tBMPリストにフォーカスを当てる()
		{
			this.listViewBMPリスト.Focus();
		}
		public CBMP tBMPをキャッシュから検索して返す( int nBMP番号1to1295 )
		{
			return this.BMPキャッシュ.tBMPをキャッシュから検索して返す( nBMP番号1to1295 );
		}
		public CBMP tBMPをキャッシュから検索して返す・なければ新規生成する( int nBMP番号1to1295 )
		{
			return this.BMPキャッシュ.tBMPをキャッシュから検索して返す・なければ新規生成する( nBMP番号1to1295 );
		}
		public ListViewItem tBMP番号に対応するListViewItemを返す( int nBMP番号1to1295 )
		{
			if( ( nBMP番号1to1295 < 1 ) || ( nBMP番号1to1295 > 36 * 36 - 1 ) )
			{
				throw new Exception( "BMP番号が範囲外です。--->[" + nBMP番号1to1295 + "]" );
			}
			return this.listViewBMPリスト.Items[ nBMP番号1to1295 - 1 ];
		}
		public void tBMP編集のRedo( CBMP bc変更前, CBMP bc変更後 )
		{
			int num = bc変更後.nBMP番号1to1295;
			CBMP cbmp = this.BMPキャッシュ.tBMPをキャッシュから検索して返す( num );
			cbmp.tコピーfrom( bc変更後 );
			cbmp.tコピーto( this.listViewBMPリスト.Items[ num - 1 ] );
			this._Form.tWAV・BMP・AVIリストのカーソルを全部同じ行に合わせる( cbmp.nBMP番号1to1295 - 1 );
			this._Form.tタブを選択する( Cメインフォーム.Eタブ種別.BMP );
			this.listViewBMPリスト.Refresh();
		}
		public void tBMP編集のUndo( CBMP bc変更前, CBMP bc変更後 )
		{
			int num = bc変更前.nBMP番号1to1295;
			CBMP cbmp = this.BMPキャッシュ.tBMPをキャッシュから検索して返す( num );
			cbmp.tコピーfrom( bc変更前 );
			cbmp.tコピーto( this.listViewBMPリスト.Items[ num - 1 ] );
			this._Form.tWAV・BMP・AVIリストのカーソルを全部同じ行に合わせる( cbmp.nBMP番号1to1295 - 1 );
			this._Form.tタブを選択する( Cメインフォーム.Eタブ種別.BMP );
			this.listViewBMPリスト.Refresh();
		}
		public ListViewItem tCBMPとListViewItemを生成して返す( int n行番号1to1295 )
		{
			return this.tBMPをキャッシュから検索して返す・なければ新規生成する( n行番号1to1295 ).t現在の内容から新しいListViewItemを作成して返す();
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
			this.tItemを交換する・BMPキャッシュ( nItem番号1, nItem番号2 );
			this.tItemを交換する・チップパレット( nItem番号1, nItem番号2 );
			this.tItemを交換する・譜面上のチップ( nItem番号1, nItem番号2 );
			this.tItemを交換する・レーン割付チップ( nItem番号1, nItem番号2 );
			this.tItemを交換する・カーソル移動( nItem番号1, nItem番号2 );
			this._Form.listViewBMPリスト.Refresh();
			this._Form.pictureBox譜面パネル.Refresh();
			this._Form.b未保存 = true;
		}
		public void tItemを選択する( int nItem番号0to1294 )
		{
			this.n現在選択中のItem番号0to1294 = nItem番号0to1294;
			this.listViewBMPリスト.Items[ nItem番号0to1294 ].Selected = true;
			this.listViewBMPリスト.Items[ nItem番号0to1294 ].Focused = true;
		}
		public void tファイル名の相対パス化( string str基本フォルダ名 )
		{
			for( int i = 1; i <= 36 * 36 - 1; i++ )
			{
				CBMP cbmp = this.BMPキャッシュ.tBMPをキャッシュから検索して返す( i );
				if( ( cbmp != null ) && ( cbmp.strファイル名.Length > 0 ) )
				{
					try
					{
						Uri uri = new Uri( str基本フォルダ名 );
						cbmp.strファイル名 = Uri.UnescapeDataString( uri.MakeRelativeUri( new Uri( cbmp.strファイル名 ) ).ToString() ).Replace( '/', '\\' );
					}
					catch( UriFormatException )
					{
					}
				}
			}
		}
		public void t画像プロパティを開いて編集する( int nBMP番号1to1295, string str相対パスの基本フォルダ )
		{
			this._Form.dlgチップパレット.t一時的に隠蔽する();
			CBMP cbmp = this.tBMPをキャッシュから検索して返す・なければ新規生成する( nBMP番号1to1295 );
			ListViewItem item = cbmp.t現在の内容から新しいListViewItemを作成して返す();
			string directoryName = "";
			if( item.SubItems[ 3 ].Text.Length > 0 )
			{
				directoryName = Path.GetDirectoryName( this._Form.strファイルの存在するディレクトリを絶対パスで返す( item.SubItems[ 3 ].Text ) );
			}
			C画像プロパティダイアログ c画像プロパティダイアログ = new C画像プロパティダイアログ( str相対パスの基本フォルダ, directoryName );
			c画像プロパティダイアログ.bmp = cbmp;
			c画像プロパティダイアログ.textBoxBMP番号.Text = item.SubItems[ 2 ].Text;
			c画像プロパティダイアログ.textBoxラベル.Text = item.SubItems[ 1 ].Text;
			c画像プロパティダイアログ.textBoxファイル.Text = item.SubItems[ 3 ].Text;
			c画像プロパティダイアログ.checkBoxBMPTEX.CheckState = c画像プロパティダイアログ.bmp.bテクスチャ ? CheckState.Checked : CheckState.Unchecked;
			c画像プロパティダイアログ.textBoxBMP番号.ForeColor = item.ForeColor;
			c画像プロパティダイアログ.textBoxBMP番号.BackColor = item.BackColor;
			if( c画像プロパティダイアログ.ShowDialog() == DialogResult.OK )
			{
				CBMP bmp = c画像プロパティダイアログ.bmp;
				CBMP cbmp3 = new CBMP();
				cbmp3.nBMP番号1to1295 = c画像プロパティダイアログ.bmp.nBMP番号1to1295;
				cbmp3.strラベル名 = c画像プロパティダイアログ.textBoxラベル.Text;
				cbmp3.strファイル名 = c画像プロパティダイアログ.textBoxファイル.Text;
				cbmp3.bテクスチャ = c画像プロパティダイアログ.checkBoxBMPTEX.Checked;
				cbmp3.col文字色 = c画像プロパティダイアログ.textBoxBMP番号.ForeColor;
				cbmp3.col背景色 = c画像プロパティダイアログ.textBoxBMP番号.BackColor;
				if( !cbmp3.b内容が同じwith( bmp ) )
				{
					bmp = new CBMP();
					bmp.tコピーfrom( c画像プロパティダイアログ.bmp );
					this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoセル<CBMP>( null, new DGUndoを実行する<CBMP>( this.tBMP編集のUndo ), new DGRedoを実行する<CBMP>( this.tBMP編集のRedo ), bmp, cbmp3 ) );
					this._Form.tUndoRedo用GUIの有効・無効を設定する();
					c画像プロパティダイアログ.bmp.tコピーfrom( cbmp3 );
					if( this.tBMP番号に対応するListViewItemを返す( nBMP番号1to1295 ) != null )
					{
						ListViewItem item2 = c画像プロパティダイアログ.bmp.t現在の内容から新しいListViewItemを作成して返す();
						item = this.tBMP番号に対応するListViewItemを返す( nBMP番号1to1295 );
						item.SubItems[ 0 ].Text = item2.SubItems[ 0 ].Text;
						item.SubItems[ 1 ].Text = item2.SubItems[ 1 ].Text;
						item.SubItems[ 2 ].Text = item2.SubItems[ 2 ].Text;
						item.SubItems[ 3 ].Text = item2.SubItems[ 3 ].Text;
						item.ForeColor = item2.ForeColor;
						item.BackColor = item2.BackColor;
					}
					this.listViewBMPリスト.Refresh();
					this._Form.b未保存 = true;
				}
			}
			this._Form.dlgチップパレット.t一時的な隠蔽を解除する();
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
		public void t新規生成のRedo( CBMP bc生成前はNull, CBMP bc生成されたBMPの複製 )
		{
			int num = bc生成されたBMPの複製.nBMP番号1to1295;
			CBMP cbmp = this.BMPキャッシュ.tBMPをキャッシュから検索して返す・なければ新規生成する( num );
			cbmp.tコピーfrom( bc生成されたBMPの複製 );
			cbmp.tコピーto( this.listViewBMPリスト.Items[ num - 1 ] );
			this._Form.tタブを選択する( Cメインフォーム.Eタブ種別.BMP );
			this.listViewBMPリスト.Refresh();
		}
		public void t新規生成のUndo( CBMP bc生成前はNull, CBMP bc生成されたBMPの複製 )
		{
			int num = bc生成されたBMPの複製.nBMP番号1to1295;
			new CBMP().tコピーto( this.listViewBMPリスト.Items[ num - 1 ] );
			this.BMPキャッシュ.tBMPをキャッシュから削除する( num );
			this._Form.tタブを選択する( Cメインフォーム.Eタブ種別.BMP );
			this.listViewBMPリスト.Refresh();
		}

		#region [ private ]
		//-----------------
		private Cメインフォーム _Form;
		private CBMPキャッシュ BMPキャッシュ = new CBMPキャッシュ();
		private ListView listViewBMPリスト;

		private void tItemを交換する・BMPキャッシュ( int nItem番号1, int nItem番号2 )
		{
			int num = nItem番号1 + 1;
			int num2 = nItem番号2 + 1;
			CBMP bc = this.BMPキャッシュ.tBMPをキャッシュから検索して返す( num );
			CBMP cbmp2 = this.BMPキャッシュ.tBMPをキャッシュから検索して返す( num2 );
			CBMP cbmp3 = new CBMP();
			cbmp3.tコピーfrom( bc );
			bc.tコピーfrom( cbmp2 );
			bc.nBMP番号1to1295 = num;
			cbmp2.tコピーfrom( cbmp3 );
			cbmp2.nBMP番号1to1295 = num2;
		}
		private void tItemを交換する・ListViewItem( int nItem番号1, int nItem番号2 )
		{
			int num = nItem番号1 + 1;
			int num2 = nItem番号2 + 1;
			CBMP cbmp = new CBMP();
			cbmp.tコピーfrom( this.listViewBMPリスト.Items[ nItem番号1 ] );
			cbmp.nBMP番号1to1295 = num2;
			CBMP cbmp2 = new CBMP();
			cbmp2.tコピーfrom( this.listViewBMPリスト.Items[ nItem番号2 ] );
			cbmp2.nBMP番号1to1295 = num;
			cbmp2.tコピーto( this.listViewBMPリスト.Items[ nItem番号1 ] );
			cbmp.tコピーto( this.listViewBMPリスト.Items[ nItem番号2 ] );
		}
		private void tItemを交換する・カーソル移動( int nItem番号1, int nItem番号2 )
		{
			this.tItemを選択する( nItem番号2 );
		}
		private void tItemを交換する・チップパレット( int nItem番号1, int nItem番号2 )
		{
			this._Form.dlgチップパレット.tパレットセルの番号を置換する( 1, nItem番号1 + 1, nItem番号2 + 1 );
		}
		private void tItemを交換する・レーン割付チップ( int nItem番号1, int nItem番号2 )
		{
			for( int i = 0; i < this._Form.mgr譜面管理者.listレーン.Count; i++ )
			{
				Cレーン cレーン = this._Form.mgr譜面管理者.listレーン[ i ];
				if( cレーン.eレーン種別 == Cレーン.E種別.BMP )
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
					if( this._Form.mgr譜面管理者.listレーン[ cチップ.nレーン番号0to ].eレーン種別 == Cレーン.E種別.BMP )
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
