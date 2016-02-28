using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using DTXCreator.UndoRedo;
using DTXCreator.WAV_BMP_AVI;
using DTXCreator.Properties;

namespace DTXCreator.譜面
{
	public class C編集モード管理
	{
		public C編集モード管理( Cメインフォーム formメインフォーム )
		{
			this._Form = formメインフォーム;
			this.mgr譜面管理者ref = formメインフォーム.mgr譜面管理者;
		}
		internal void MouseClick( MouseEventArgs e )
		{
			if( e.Y < ( C譜面管理.nレーン割付チップ番号表示高さdot + 10 ) )
			{
				Cレーン lc = this.mgr譜面管理者ref.listレーン[ this.n現在のチップカーソルがあるレーン番号0to ];
                if (lc.eレーン種別 == Cレーン.E種別.GtR || lc.eレーン種別 == Cレーン.E種別.GtG || lc.eレーン種別 == Cレーン.E種別.GtB || lc.eレーン種別 == Cレーン.E種別.GtY || lc.eレーン種別 == Cレーン.E種別.GtP || lc.eレーン種別 == Cレーン.E種別.BsR || lc.eレーン種別 == Cレーン.E種別.BsG || lc.eレーン種別 == Cレーン.E種別.BsB || lc.eレーン種別 == Cレーン.E種別.BsY || lc.eレーン種別 == Cレーン.E種別.BsP)
				{
					return;
				}
				if( ( Control.ModifierKeys & Keys.Control ) != Keys.Control )
				{
					int num = ( e.Button == MouseButtons.Left ) ? ( this._Form.n現在選択中のWAV・BMP・AVIリストの行番号0to1294 + 1 ) : 0;
					if( num != lc.nレーン割付チップ・表0or1to1295 )
					{
						Cレーン割付用UndoRedo redo = new Cレーン割付用UndoRedo( lc, lc.nレーン割付チップ・表0or1to1295, false );
						Cレーン割付用UndoRedo redo2 = new Cレーン割付用UndoRedo( lc, num, false );
						this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoセル<Cレーン割付用UndoRedo>( null, new DGUndoを実行する<Cレーン割付用UndoRedo>( this.tレーン割付のUndo ), new DGRedoを実行する<Cレーン割付用UndoRedo>( this.tレーン割付のRedo ), redo, redo2 ) );
						this._Form.tUndoRedo用GUIの有効・無効を設定する();
						lc.nレーン割付チップ・表0or1to1295 = num;
					}
				}
				else
				{
					int num2 = ( e.Button == MouseButtons.Left ) ? ( this._Form.n現在選択中のWAV・BMP・AVIリストの行番号0to1294 + 1 ) : 0;
					if( num2 != lc.nレーン割付チップ・裏0or1to1295 )
					{
						Cレーン割付用UndoRedo redo3 = new Cレーン割付用UndoRedo( lc, lc.nレーン割付チップ・裏0or1to1295, true );
						Cレーン割付用UndoRedo redo4 = new Cレーン割付用UndoRedo( lc, num2, true );
						this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoセル<Cレーン割付用UndoRedo>( null, new DGUndoを実行する<Cレーン割付用UndoRedo>( this.tレーン割付のUndo ), new DGRedoを実行する<Cレーン割付用UndoRedo>( this.tレーン割付のRedo ), redo3, redo4 ) );
						this._Form.tUndoRedo用GUIの有効・無効を設定する();
						lc.nレーン割付チップ・裏0or1to1295 = num2;
					}
				}
			}
			else
			{
				this.tチップの配置または削除( e );
			}
			this._Form.pictureBox譜面パネル.Refresh();
		}
		internal void MouseLeave( EventArgs e )
		{
			this.rc現在のチップカーソル領域.X = 0;
			this.rc現在のチップカーソル領域.Y = 0;
			this.rc現在のチップカーソル領域.Width = 0;
			this.rc現在のチップカーソル領域.Height = 0;
			this._Form.pictureBox譜面パネル.Refresh();
		}
		internal void MouseMove( MouseEventArgs e )
		{
			Rectangle rectangle = new Rectangle( this.rc現在のチップカーソル領域.Location, this.rc現在のチップカーソル領域.Size );
			this.n現在のチップカーソルがあるレーン番号0to = this.mgr譜面管理者ref.nX座標dotが位置するレーン番号を返す( e.X );
			this.n現在のチップカーソルの譜面先頭からの位置grid = this.mgr譜面管理者ref.nY座標dotが位置するgridを返す・ガイド幅単位( e.Y );
			bool bOutOfLanes = false;
			if( e.Y < ( C譜面管理.nレーン割付チップ番号表示高さdot + 10 ) )
			{
				this.rc現在のチップカーソル領域 = new Rectangle( 0, 0, 0, 0 );
			}
			else
			{
				int nLaneNo = this.n現在のチップカーソルがあるレーン番号0to;
				if ( nLaneNo < 0 )				// #24264 2011.1.27 yyagi; to avoid ArgumentOutOfExceptions in x and width.
				{
					bOutOfLanes = true;
					nLaneNo = 0;
				}
				int x = this.mgr譜面管理者ref.nレーンの左端X座標dotを返す( nLaneNo );
				int y = this.mgr譜面管理者ref.n譜面先頭からの位置gridから描画領域内のY座標dotを返す( this.n現在のチップカーソルの譜面先頭からの位置grid, this._Form.pictureBox譜面パネル.ClientSize ) - Cチップ.nチップの高さdot;
				int width = this.mgr譜面管理者ref.listレーン[ nLaneNo ].n幅dot;
				int height = Cチップ.nチップの高さdot;
				this.rc現在のチップカーソル領域 = new Rectangle( x, y, width, height );
			}
			if ( !rectangle.Equals( this.rc現在のチップカーソル領域 ) && !bOutOfLanes )	// #24264 2011.1.27 yyagi add condition !bOutOfLanes to avoid ArgumentOutOfException in Refresh().
			{
				this._Form.pictureBox譜面パネル.Refresh(); 
			}
		}
		internal void Paint( PaintEventArgs e )
		{
			this.tチップカーソルを描画する( e.Graphics );
		}

		#region [ private ]
		//-----------------
		private Cメインフォーム _Form;
		private C譜面管理 mgr譜面管理者ref;
		private int n現在のチップカーソルがあるレーン番号0to;
		private int n現在のチップカーソルの譜面先頭からの位置grid;
		private Rectangle rc現在のチップカーソル領域 = new Rectangle( 0, 0, 0, 0 );

		private bool b指定位置にRGBチップがひとつもない( int n譜面先頭からの位置grid, int nRレーン番号0to, int nGレーン番号0to, int nBレーン番号0to )
		{
			C小節 c小節 = this.mgr譜面管理者ref.p譜面先頭からの位置gridを含む小節を返す( n譜面先頭からの位置grid );
			if( c小節 == null )
			{
				return false;
			}
			int num = this.mgr譜面管理者ref.n譜面先頭からみた小節先頭の位置gridを返す( c小節.n小節番号0to3599 );
			foreach( Cチップ cチップ in c小節.listチップ )
			{
				if( ( ( num + cチップ.n位置grid ) == n譜面先頭からの位置grid ) && ( ( ( cチップ.nレーン番号0to == nRレーン番号0to ) || ( cチップ.nレーン番号0to == nGレーン番号0to ) ) || ( cチップ.nレーン番号0to == nBレーン番号0to ) ) )
				{
					return false;
				}
			}
			return true;
		}
		private void tチップカーソルを描画する( Graphics g )
		{
			if( ( this.rc現在のチップカーソル領域.Width > 0 ) && ( this.rc現在のチップカーソル領域.Height > 0 ) )
			{
				Cレーン cレーン = this.mgr譜面管理者ref.listレーン[ this.n現在のチップカーソルがあるレーン番号0to ];
				bool flag = ( Control.ModifierKeys & Keys.Control ) == Keys.Control;
				int num = -1;
				switch( cレーン.eレーン種別 )
				{
					case Cレーン.E種別.GtR:
					case Cレーン.E種別.GtG:
					case Cレーン.E種別.GtB:
                    case Cレーン.E種別.GtY:
                    case Cレーン.E種別.GtP:
                    case Cレーン.E種別.BsR:
					case Cレーン.E種別.BsG:
					case Cレーン.E種別.BsB:
                    case Cレーン.E種別.BsY:
                    case Cレーン.E種別.BsP:
                    case Cレーン.E種別.BPM:
						num = -1;
						break;

					default:
						num = this._Form.n現在選択中のWAV・BMP・AVIリストの行番号0to1294 + 1;
						if( ( Control.ModifierKeys & Keys.Shift ) != Keys.Shift )
						{
							int num2 = flag ? cレーン.nレーン割付チップ・裏0or1to1295 : cレーン.nレーン割付チップ・表0or1to1295;
							if( num2 != 0 )
							{
								num = num2;
							}
						}
						break;
				}
				if( !flag )
				{
					Cチップ.t表チップを描画する( g, this.rc現在のチップカーソル領域, num, cレーン.col背景色 );
				}
				else
				{
					Cチップ.t裏チップを描画する( g, this.rc現在のチップカーソル領域, num, cレーン.col背景色 );
				}
				Cチップ.tチップの周囲の太枠を描画する( g, this.rc現在のチップカーソル領域 );
			}
		}
		private void tチップの配置または削除( MouseEventArgs e )
		{
			if( ( this.rc現在のチップカーソル領域.Width > 0 ) && ( this.rc現在のチップカーソル領域.Height > 0 ) )
			{
				if( e.Button == MouseButtons.Left )
				{
					bool flag = ( Control.ModifierKeys & Keys.Control ) == Keys.Control;
					bool flag2 = ( Control.ModifierKeys & Keys.Shift ) == Keys.Shift;
					Cレーン cレーン = this.mgr譜面管理者ref.listレーン[ this.n現在のチップカーソルがあるレーン番号0to ];
					if( cレーン.eレーン種別 != Cレーン.E種別.BPM )
					{
						if( ( cレーン.eレーン種別 == Cレーン.E種別.GtV ) || ( cレーン.eレーン種別 == Cレーン.E種別.BsV ) )
						{
							int num5 = flag ? cレーン.nレーン割付チップ・裏0or1to1295 : cレーン.nレーン割付チップ・表0or1to1295;
							if( ( num5 == 0 ) || flag2 )
							{
								num5 = this._Form.n現在選択中のWAV・BMP・AVIリストの行番号0to1294 + 1;
							}
							this._Form.mgrUndoRedo管理者.tトランザクション記録を開始する();
							this.mgr譜面管理者ref.tチップを配置または置換する( this.n現在のチップカーソルがあるレーン番号0to, this.n現在のチップカーソルの譜面先頭からの位置grid, num5, 0f, flag );
							if( this.b指定位置にRGBチップがひとつもない( this.n現在のチップカーソルの譜面先頭からの位置grid, this.n現在のチップカーソルがあるレーン番号0to + 1, this.n現在のチップカーソルがあるレーン番号0to + 2, this.n現在のチップカーソルがあるレーン番号0to + 3 ) )
							{
								this.mgr譜面管理者ref.tチップを配置または置換する( this.n現在のチップカーソルがあるレーン番号0to + 1, this.n現在のチップカーソルの譜面先頭からの位置grid, 2, 0f, false );
							}
							this._Form.mgrUndoRedo管理者.tトランザクション記録を終了する();
						}
                        else if (cレーン.eレーン種別 == Cレーン.E種別.GtR || cレーン.eレーン種別 == Cレーン.E種別.GtG || cレーン.eレーン種別 == Cレーン.E種別.GtB || cレーン.eレーン種別 == Cレーン.E種別.GtY || cレーン.eレーン種別 == Cレーン.E種別.GtP)
						{
							if( flag )
							{
								for( int i = 0; i < this.mgr譜面管理者ref.listレーン.Count; i++ )
								{
									Cレーン cレーン2 = this.mgr譜面管理者ref.listレーン[ i ];
									if( cレーン2.eレーン種別 == Cレーン.E種別.GtR )
									{
										this.mgr譜面管理者ref.tチップを配置または置換する( i, this.n現在のチップカーソルの譜面先頭からの位置grid, 2, 0f, false );
										break;
									}
								}
							}
							else
							{
								this.mgr譜面管理者ref.tチップを配置または置換する( this.n現在のチップカーソルがあるレーン番号0to, this.n現在のチップカーソルの譜面先頭からの位置grid, 1, 0f, false );
							}
						}
                        else if ( cレーン.eレーン種別 == Cレーン.E種別.BsR || cレーン.eレーン種別 == Cレーン.E種別.BsG || cレーン.eレーン種別 == Cレーン.E種別.BsB || cレーン.eレーン種別 == Cレーン.E種別.BsY || cレーン.eレーン種別 == Cレーン.E種別.BsP )
						{
							if( flag )
							{
								for( int j = 0; j < this.mgr譜面管理者ref.listレーン.Count; j++ )
								{
									Cレーン cレーン3 = this.mgr譜面管理者ref.listレーン[ j ];
									if( cレーン3.eレーン種別 == Cレーン.E種別.BsR )
									{
										this.mgr譜面管理者ref.tチップを配置または置換する( j, this.n現在のチップカーソルの譜面先頭からの位置grid, 2, 0f, false );
										break;
									}
								}
							}
							else
							{
								this.mgr譜面管理者ref.tチップを配置または置換する( this.n現在のチップカーソルがあるレーン番号0to, this.n現在のチップカーソルの譜面先頭からの位置grid, 1, 0f, false );
							}
						}
						else
						{
							int num8 = flag ? cレーン.nレーン割付チップ・裏0or1to1295 : cレーン.nレーン割付チップ・表0or1to1295;
							if( ( num8 == 0 ) || flag2 )
							{
								num8 = this._Form.n現在選択中のWAV・BMP・AVIリストの行番号0to1294 + 1;
							}
							this.mgr譜面管理者ref.tチップを配置または置換する( this.n現在のチップカーソルがあるレーン番号0to, this.n現在のチップカーソルの譜面先頭からの位置grid, num8, 0f, flag );
						}
					}
					else
					{
						this._Form.dlgチップパレット.t一時的に隠蔽する();
						C数値入力ダイアログ c数値入力ダイアログ = new C数値入力ダイアログ( this.mgr譜面管理者ref.dc譜面先頭からの位置gridにおけるBPMを返す( this.n現在のチップカーソルの譜面先頭からの位置grid ), 0.0001M, 1000M, Resources.strBPM選択ダイアログの説明文 );
						Point point = this._Form.pictureBox譜面パネル.PointToScreen( new Point( e.X, e.Y ) );
						c数値入力ダイアログ.Left = point.X - ( c数値入力ダイアログ.Width / 2 );
						c数値入力ダイアログ.Top = point.Y + 4;
						DialogResult result = c数値入力ダイアログ.ShowDialog();
						this._Form.dlgチップパレット.t一時的な隠蔽を解除する();
						if( result != DialogResult.OK )
						{
							return;
						}
						float num = (float) c数値入力ダイアログ.dc数値;
						int key = -1;
						foreach( KeyValuePair<int, float> pair in this.mgr譜面管理者ref.dicBPx )
						{
							if( pair.Value == num )
							{
								key = pair.Key;
								break;
							}
						}
						if( key == -1 )
						{
							for( int k = 1; k < 36 * 36; k++ )
							{
								if( !this.mgr譜面管理者ref.dicBPx.ContainsKey( k ) )
								{
									this.mgr譜面管理者ref.dicBPx.Add( k, num );
									key = k;
									break;
								}
							}
						}
						this.mgr譜面管理者ref.tチップを配置または置換する( this.n現在のチップカーソルがあるレーン番号0to, this.n現在のチップカーソルの譜面先頭からの位置grid, key, num, false );
					}
					if( this._Form.appアプリ設定.PlaySoundOnWAVChipAllocated && ( ( ( cレーン.eレーン種別 == Cレーン.E種別.WAV ) || ( cレーン.eレーン種別 == Cレーン.E種別.GtV ) ) || ( cレーン.eレーン種別 == Cレーン.E種別.BsV ) ) )
					{
						int num9 = flag ? cレーン.nレーン割付チップ・裏0or1to1295 : cレーン.nレーン割付チップ・表0or1to1295;
						if( ( num9 == 0 ) || flag2 )
						{
							num9 = this._Form.n現在選択中のWAV・BMP・AVIリストの行番号0to1294 + 1;
						}
						CWAV wc = this._Form.mgrWAVリスト管理者.tWAVをキャッシュから検索して返す( num9 );
						if( ( wc != null ) && ( !this._Form.appアプリ設定.NoPreviewBGM || !wc.bBGMとして使用 ) )
						{
							this._Form.mgrWAVリスト管理者.tプレビュー音を再生する( wc );
						}
					}
				}
				if( e.Button == MouseButtons.Right )
				{
					this.mgr譜面管理者ref.tチップを削除する( this.n現在のチップカーソルがあるレーン番号0to, this.n現在のチップカーソルの譜面先頭からの位置grid );
				}
			}
		}
		private void tレーン割付のRedo( Cレーン割付用UndoRedo lur変更前, Cレーン割付用UndoRedo lur変更後 )
		{
			if( !lur変更前.b裏 )
			{
				lur変更前.lc.nレーン割付チップ・表0or1to1295 = lur変更後.n番号0or1to1295;
			}
			else
			{
				lur変更前.lc.nレーン割付チップ・裏0or1to1295 = lur変更後.n番号0or1to1295;
			}
			this._Form.pictureBox譜面パネル.Refresh();
		}
		private void tレーン割付のUndo( Cレーン割付用UndoRedo lur変更前, Cレーン割付用UndoRedo lur変更後 )
		{
			if( !lur変更前.b裏 )
			{
				lur変更前.lc.nレーン割付チップ・表0or1to1295 = lur変更前.n番号0or1to1295;
			}
			else
			{
				lur変更前.lc.nレーン割付チップ・裏0or1to1295 = lur変更前.n番号0or1to1295;
			}
			this._Form.pictureBox譜面パネル.Refresh();
		}
		//-----------------
		#endregion
	}
}
