using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using DTXCreator.UndoRedo;
using DTXCreator.Properties;

namespace DTXCreator.Score
{
	public class CSelectionModeManager  // C選択モード管理
	{
		public CSelectionModeManager( CMainForm formメインフォーム )
		{
			this._Form = formメインフォーム;
			this.mgr譜面管理者ref = formメインフォーム.mgr譜面管理者;
		}
		public void t検索する()
		{
			this._Form.dlgチップパレット.t一時的に隠蔽する();
			this.t検索する_メイン();
			this._Form.dlgチップパレット.t一時的な隠蔽を解除する();
		}
		public void t個別選択解除( CChip cc )
		{
			CChipPositionUndoRedo redo = new CChipPositionUndoRedo( this.mgr譜面管理者ref.pチップの存在する小節を返す( cc ).n小節番号0to3599, cc.nレーン番号0to, cc.n位置grid, cc.n値_整数1to1295 );
			this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoCell<CChipPositionUndoRedo>( null, new DGUndoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ選択解除のUndo ), new DGRedoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ選択解除のRedo ), redo, redo ) );
			this._Form.tUndoRedo用GUIの有効_無効を設定する();
			cc.b確定選択中 = false;
		}
		public void t全チップの選択を解除する()
		{
			this._Form.mgrUndoRedo管理者.tトランザクション記録を開始する();
			foreach( KeyValuePair<int, CMeasure> pair in this.mgr譜面管理者ref.dic小節 )
			{
				CMeasure c小節 = pair.Value;
				for( int j = 0; j < c小節.listチップ.Count; j++ )
				{
					CChip cチップ = c小節.listチップ[ j ];
					if( cチップ.n枠外レーン数 != 0 )
					{
						CChip cc = new CChip();
						cc.tコピーfrom( cチップ );
						CChipLocationUndoRedo redo = new CChipLocationUndoRedo( c小節.n小節番号0to3599, cc );
						this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoCell<CChipLocationUndoRedo>( null, new DGUndoを実行する<CChipLocationUndoRedo>( this.mgr譜面管理者ref.tチップ削除のUndo ), new DGRedoを実行する<CChipLocationUndoRedo>( this.mgr譜面管理者ref.tチップ削除のRedo ), redo, redo ) );
						c小節.listチップ.Remove( cチップ );
						j = -1;
					}
				}
			}
			for( int i = 0; i < this.mgr譜面管理者ref.dic小節.Count; i++ )
			{
				CMeasure c小節2 = this.mgr譜面管理者ref.dic小節[ i ];
				for( int k = 0; k < c小節2.listチップ.Count; k++ )
				{
					CChip cチップ3 = c小節2.listチップ[ k ];
					if( cチップ3.b確定選択中 || cチップ3.bドラッグで選択中 )
					{
						this.mgr譜面管理者ref.bOPENチップである( cチップ3 );
						for( int m = 0; m < c小節2.listチップ.Count; m++ )
						{
							CChip cチップ4 = c小節2.listチップ[ m ];
							if( ( ( k != m ) && ( cチップ3.nレーン番号0to == cチップ4.nレーン番号0to ) ) && ( ( cチップ3.n位置grid == cチップ4.n位置grid ) && !cチップ4.b確定選択中 ) )
							{
								CChip cチップ5 = new CChip();
								cチップ5.tコピーfrom( cチップ4 );
								CChipLocationUndoRedo redo2 = new CChipLocationUndoRedo( c小節2.n小節番号0to3599, cチップ5 );
								this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoCell<CChipLocationUndoRedo>( null, new DGUndoを実行する<CChipLocationUndoRedo>( this.mgr譜面管理者ref.tチップ削除のUndo ), new DGRedoを実行する<CChipLocationUndoRedo>( this.mgr譜面管理者ref.tチップ削除のRedo ), redo2, redo2 ) );
								c小節2.listチップ.RemoveAt( m );
								k = -1;
								break;
							}
						}
					}
				}
			}
			foreach( KeyValuePair<int, CMeasure> pair2 in this.mgr譜面管理者ref.dic小節 )
			{
				CMeasure c小節3 = pair2.Value;
				for( int n = 0; n < c小節3.listチップ.Count; n++ )
				{
					CChip cチップ6 = c小節3.listチップ[ n ];
					if( cチップ6.b確定選択中 )
					{
						CChipPositionUndoRedo redo3 = new CChipPositionUndoRedo( c小節3.n小節番号0to3599, cチップ6.nレーン番号0to, cチップ6.n位置grid, cチップ6.n値_整数1to1295 );
						this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoCell<CChipPositionUndoRedo>( null, new DGUndoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ選択解除のUndo ), new DGRedoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ選択解除のRedo ), redo3, redo3 ) );
						cチップ6.b移動済 = false;
						cチップ6.bドラッグで選択中 = false;
						cチップ6.b確定選択中 = false;
					}
				}
			}
			this._Form.mgrUndoRedo管理者.tトランザクション記録を終了する();
			this._Form.tUndoRedo用GUIの有効_無効を設定する();
		}
		public void t全チップを選択する()
		{
			this._Form.mgrUndoRedo管理者.tトランザクション記録を開始する();
			foreach( KeyValuePair<int, CMeasure> pair in this.mgr譜面管理者ref.dic小節 )
			{
				CMeasure c小節 = pair.Value;
				for( int i = 0; i < c小節.listチップ.Count; i++ )
				{
					CChip cチップ = c小節.listチップ[ i ];
					if( !cチップ.b確定選択中 )
					{
						CChipPositionUndoRedo redo = new CChipPositionUndoRedo( c小節.n小節番号0to3599, cチップ.nレーン番号0to, cチップ.n位置grid, cチップ.n値_整数1to1295 );
						this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoCell<CChipPositionUndoRedo>( null, new DGUndoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ選択のUndo ), new DGRedoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ選択のRedo ), redo, redo ) );
						cチップ.bドラッグで選択中 = false;
						cチップ.b確定選択中 = true;
					}
				}
			}
			this._Form.mgrUndoRedo管理者.tトランザクション記録を終了する();
			this._Form.tUndoRedo用GUIの有効_無効を設定する();
			this._Form.t選択チップの有無に応じて編集用GUIの有効_無効を設定する();
			this._Form.pictureBox譜面パネル.Refresh();
		}
        public void tレーン上の全チップを選択する( int lane )			// #32134 2013.9.29 suggested by beatme
		{
			// Debug.WriteLine( "laneno=" + lane + " " + this.mgr譜面管理者ref.listレーン[ lane ].strレーン名 );

			this._Form.mgrUndoRedo管理者.tトランザクション記録を開始する();
			foreach ( KeyValuePair<int, CMeasure> pair in this.mgr譜面管理者ref.dic小節 )
			{
				CMeasure c小節 = pair.Value;
				for ( int i = 0; i < c小節.listチップ.Count; i++ )
				{
					CChip cチップ = c小節.listチップ[ i ];
					if ( cチップ.nレーン番号0to == lane && !cチップ.b確定選択中 )
					{
						CChipPositionUndoRedo redo = new CChipPositionUndoRedo( c小節.n小節番号0to3599, cチップ.nレーン番号0to, cチップ.n位置grid, cチップ.n値_整数1to1295 );
						this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoCell<CChipPositionUndoRedo>( null, new DGUndoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ選択のUndo ), new DGRedoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ選択のRedo ), redo, redo ) );
						cチップ.bドラッグで選択中 = false;
						cチップ.b確定選択中 = true;
					}
				}
			}
			this._Form.mgrUndoRedo管理者.tトランザクション記録を終了する();
			this._Form.tUndoRedo用GUIの有効_無効を設定する();
			this._Form.t選択チップの有無に応じて編集用GUIの有効_無効を設定する();
			this._Form.pictureBox譜面パネル.Refresh();
		}
		public void t小節上の全チップを選択する( int n小節番号 )			// #32134 2013.9.29 suggested by beatme
		{
			this._Form.mgrUndoRedo管理者.tトランザクション記録を開始する();
			CMeasure c小節 = this.mgr譜面管理者ref.dic小節[ n小節番号 ];
			for ( int i = 0; i < c小節.listチップ.Count; i++ )
			{
				CChip cチップ = c小節.listチップ[ i ];
				if ( !cチップ.b確定選択中 )
				{
					CChipPositionUndoRedo redo = new CChipPositionUndoRedo( c小節.n小節番号0to3599, cチップ.nレーン番号0to, cチップ.n位置grid, cチップ.n値_整数1to1295 );
					this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoCell<CChipPositionUndoRedo>( null, new DGUndoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ選択のUndo ), new DGRedoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ選択のRedo ), redo, redo ) );
					cチップ.bドラッグで選択中 = false;
					cチップ.b確定選択中 = true;
				}
			}
			this._Form.mgrUndoRedo管理者.tトランザクション記録を終了する();
			this._Form.tUndoRedo用GUIの有効_無効を設定する();
			this._Form.t選択チップの有無に応じて編集用GUIの有効_無効を設定する();
			this._Form.pictureBox譜面パネル.Refresh();
		}
		public void t置換する()
		{
			this._Form.dlgチップパレット.t一時的に隠蔽する();
			this.t置換する_メイン();
			this._Form.dlgチップパレット.t一時的な隠蔽を解除する();
		}
		internal void MouseClick( MouseEventArgs e )
		{
			if( e.Button == MouseButtons.Right )
			{
				this._Form.t選択モードのコンテクストメニューを表示する( e.X, e.Y );
			}
		}
		internal void MouseDown( MouseEventArgs e )
		{
			if( e.Button == MouseButtons.Left )
			{
				CChip cc = this.mgr譜面管理者ref.p指定された座標dotにあるチップを返す( e.X, e.Y );
				if( ( cc == null ) || !cc.b確定選択中 )
				{
					this.t範囲選択開始処理( e );
				}
				else if( ( Control.ModifierKeys & Keys.Control ) != Keys.Control )
				{
					this.t移動開始処理( e );
				}
				else
				{
					this.t個別選択解除( cc );
				}
			}
		}
		internal void MouseMove( MouseEventArgs e )
		{
			if( e.Button == MouseButtons.Left )
			{
				if( this.b範囲選択のためにドラッグ中 )
				{
					this.t範囲選択継続処理( e );
				}
				if( this.b移動のためにドラッグ中 )
				{
					this.t移動継続処理( e );
				}
			}
			else
			{
				if( this.b範囲選択のためにドラッグ中 )
				{
					this.t範囲選択終了処理( e );
				}
				if( this.b移動のためにドラッグ中 )
				{
					this.t移動終了処理( e );
				}
			}
			this._Form.pictureBox譜面パネル.Refresh();
		}
		internal void Paint( PaintEventArgs e )
		{
			if( this.b範囲選択のためにドラッグ中 )
			{
				this.t現在の選択範囲を描画する( e.Graphics );
			}
		}

		#region [ private ]
		//-----------------
		private CMainForm _Form;
		private SolidBrush br選択領域ブラシ = new SolidBrush( Color.FromArgb( 80, 0x37, 0x37, 0xff ) );
		private bool b移動のためにドラッグ中;
		private bool b範囲選択のためにドラッグ中;
		private ScoreManager mgr譜面管理者ref;
		private Point pt現在のドラッグ開始位置dot = new Point();
		private Point pt現在のドラッグ終了位置dot = new Point();
		private Point pt前回の位置LaneGrid = new Point();

		private void tチップを横に移動する( CChip cc, int n移動量lane )
		{
			int num2;
			int count = this.mgr譜面管理者ref.listレーン.Count;
			if( cc.n枠外レーン数 < 0 )
			{
				num2 = cc.n枠外レーン数 + n移動量lane;
			}
			else if( cc.n枠外レーン数 > 0 )
			{
				num2 = ( ( count - 1 ) + cc.n枠外レーン数 ) + n移動量lane;
			}
			else
			{
				num2 = cc.nレーン番号0to + n移動量lane;
			}
			if( num2 < 0 )
			{
				cc.n枠外レーン数 = num2;
			}
			else if( num2 >= count )
			{
				cc.n枠外レーン数 = num2 - ( count - 1 );
			}
			else
			{
				cc.nレーン番号0to = num2;
				cc.n枠外レーン数 = 0;
			}
			cc.b移動済 = true;
			this._Form.b未保存 = true;
		}
		private void tチップを縦に移動する( CChip cc, int n移動量grid, CMeasure csチップのある小節 )
		{
			cc.b移動済 = true;
			int num = cc.n位置grid + n移動量grid;
			if( num < 0 )
			{
				int num2 = csチップのある小節.n小節番号0to3599;
				CMeasure c小節 = null;
				while( num < 0 )
				{
					num2--;
					c小節 = this.mgr譜面管理者ref.p小節を返す( num2 );
					if( c小節 == null )
					{
						return;
					}
					num += c小節.n小節長倍率を考慮した現在の小節の高さgrid;
				}
				CChip item = new CChip();
				item.tコピーfrom( cc );
				item.n位置grid = num;
				csチップのある小節.listチップ.Remove( cc );
				c小節.listチップ.Add( item );
			}
			else if( num >= csチップのある小節.n小節長倍率を考慮した現在の小節の高さgrid )
			{
				int num3 = csチップのある小節.n小節番号0to3599;
				CMeasure c小節2 = csチップのある小節;
				while( num >= c小節2.n小節長倍率を考慮した現在の小節の高さgrid )
				{
					num -= c小節2.n小節長倍率を考慮した現在の小節の高さgrid;
					num3++;
					c小節2 = this.mgr譜面管理者ref.p小節を返す( num3 );
					if( c小節2 == null )
					{
						c小節2 = new CMeasure( num3 );
						this.mgr譜面管理者ref.dic小節.Add( num3, c小節2 );
					}
				}
				CChip cチップ2 = new CChip();
				cチップ2.tコピーfrom( cc );
				cチップ2.n位置grid = num;
				csチップのある小節.listチップ.Remove( cc );
				c小節2.listチップ.Add( cチップ2 );
			}
			else
			{
				cc.n位置grid = num;
			}
			this._Form.b未保存 = true;
		}
		private void tチップ移動のRedo( CChipPositionUndoRedo ur変更前, CChipPositionUndoRedo ur変更後 )
		{
			CMeasure c小節 = this.mgr譜面管理者ref.p小節を返す( ur変更前.n小節番号0to );
			if( c小節 != null )
			{
				for( int i = 0; i < c小節.listチップ.Count; i++ )
				{
					CChip item = c小節.listチップ[ i ];
					if( ( item.b確定選択中 && ( item.n位置grid == ur変更前.n位置grid ) ) && ( item.nレーン番号0to == ur変更前.nレーン番号0to ) )
					{
						CMeasure c小節2 = this.mgr譜面管理者ref.p小節を返す( ur変更後.n小節番号0to );
						if( c小節2 != null )
						{
							c小節.listチップ.RemoveAt( i );
							item.nレーン番号0to = ur変更後.nレーン番号0to;
							item.n位置grid = ur変更後.n位置grid;
							c小節2.listチップ.Add( item );
							break;
						}
					}
				}
				this._Form.pictureBox譜面パネル.Refresh();
			}
		}
		private void tチップ移動のUndo( CChipPositionUndoRedo ur変更前, CChipPositionUndoRedo ur変更後 )
		{
			CMeasure c小節 = this.mgr譜面管理者ref.p小節を返す( ur変更後.n小節番号0to );
			if( c小節 != null )
			{
				for( int i = 0; i < c小節.listチップ.Count; i++ )
				{
					CChip item = c小節.listチップ[ i ];
					if( ( item.b確定選択中 && ( item.n位置grid == ur変更後.n位置grid ) ) && ( item.nレーン番号0to == ur変更後.nレーン番号0to ) )
					{
						CMeasure c小節2 = this.mgr譜面管理者ref.p小節を返す( ur変更前.n小節番号0to );
						if( c小節2 != null )
						{
							c小節.listチップ.RemoveAt( i );
							item.nレーン番号0to = ur変更前.nレーン番号0to;
							item.n位置grid = ur変更前.n位置grid;
							c小節2.listチップ.Add( item );
							break;
						}
					}
				}
				this._Form.pictureBox譜面パネル.Refresh();
			}
		}
		private void tドラッグ範囲中のチップを選択する()
		{
			Rectangle rectangle = new Rectangle();
			rectangle.X = Math.Min( this.pt現在のドラッグ開始位置dot.X, this.pt現在のドラッグ終了位置dot.X );
			rectangle.Y = Math.Min( this.pt現在のドラッグ開始位置dot.Y, this.pt現在のドラッグ終了位置dot.Y );
			rectangle.Width = Math.Abs( (int) ( this.pt現在のドラッグ開始位置dot.X - this.pt現在のドラッグ終了位置dot.X ) );
			rectangle.Height = Math.Abs( (int) ( this.pt現在のドラッグ開始位置dot.Y - this.pt現在のドラッグ終了位置dot.Y ) );
			Rectangle rectangle2 = new Rectangle();
			rectangle2.X = this.mgr譜面管理者ref.nX座標dotが位置するレーン番号を返す( rectangle.X );
			rectangle2.Y = this.mgr譜面管理者ref.nY座標dotが位置するgridを返す_最高解像度( rectangle.Y );
			rectangle2.Width = this.mgr譜面管理者ref.nX座標dotが位置するレーン番号を返す( rectangle.Right ) - rectangle2.X;
			rectangle2.Height = this.mgr譜面管理者ref.nY座標dotが位置するgridを返す_最高解像度( rectangle.Bottom ) - rectangle2.Y;
			int num = 0;
			for( int i = 0; i < this.mgr譜面管理者ref.dic小節.Count; i++ )
			{
				CMeasure c小節 = this.mgr譜面管理者ref.dic小節[ i ];
				int num3 = c小節.n小節長倍率を考慮した現在の小節の高さgrid;
				for( int j = 0; j < c小節.listチップ.Count; j++ )
				{
					int num5;
					CChip cc = c小節.listチップ[ j ];
					if( this.mgr譜面管理者ref.bOPENチップである( cc ) )
					{
						if( ( ( cc.nレーン番号0to + 2 ) >= rectangle2.X ) && ( rectangle2.Right >= cc.nレーン番号0to ) )
						{
							goto Label_01B0;
						}
						cc.bドラッグで選択中 = false;
						continue;
					}
					if( ( cc.nレーン番号0to < rectangle2.X ) || ( rectangle2.Right < cc.nレーン番号0to ) )
					{
						cc.bドラッグで選択中 = false;
						continue;
					}
				Label_01B0:
					num5 = num + cc.n位置grid;
					int num6 = num5 + CMeasure.n位置変換dot2grid( CChip.nチップの高さdot );
					if( ( num6 < rectangle2.Bottom ) || ( rectangle2.Top < num5 ) )
					{
						cc.bドラッグで選択中 = false;
					}
					else
					{
						cc.bドラッグで選択中 = true;
					}
				}
				num += num3;
			}
		}
		private void t移動開始処理( MouseEventArgs e )
		{
			this.b移動のためにドラッグ中 = true;
			this.pt現在のドラッグ開始位置dot.X = this.pt現在のドラッグ終了位置dot.X = e.X;
			this.pt現在のドラッグ開始位置dot.Y = this.pt現在のドラッグ終了位置dot.Y = e.Y;
			this.pt前回の位置LaneGrid.X = this.mgr譜面管理者ref.nX座標dotが位置するレーン番号を返す( e.X );
			this.pt前回の位置LaneGrid.Y = this.mgr譜面管理者ref.nY座標dotが位置するgridを返す_ガイド幅単位( e.Y );
			foreach( KeyValuePair<int, CMeasure> pair in this._Form.mgr譜面管理者.dic小節 )
			{
				CMeasure c小節 = pair.Value;
				for( int i = 0; i < c小節.listチップ.Count; i++ )
				{
					CChip cチップ = c小節.listチップ[ i ];
					if( cチップ.b確定選択中 )
					{
						cチップ.n移動開始時の小節番号0to = c小節.n小節番号0to3599;
						cチップ.n移動開始時のレーン番号0to = cチップ.nレーン番号0to;
						cチップ.n移動開始時の小節内の位置grid = cチップ.n位置grid;
					}
				}
			}
		}
		private void t移動継続処理( MouseEventArgs e )
		{
			this.pt現在のドラッグ終了位置dot.X = e.X;
			this.pt現在のドラッグ終了位置dot.Y = e.Y;
			this.t画面上下にマウスカーソルがあるなら譜面を縦スクロールする( e );
			this.t確定選択中のチップを移動する();
		}
		private void t移動終了処理( MouseEventArgs e )
		{
			this.b移動のためにドラッグ中 = false;
			foreach( KeyValuePair<int, CMeasure> pair in this.mgr譜面管理者ref.dic小節 )
			{
				for( int i = 0; i < pair.Value.listチップ.Count; i++ )
				{
					CChip cチップ = pair.Value.listチップ[ i ];
					if( cチップ.b確定選択中 && ( cチップ.n枠外レーン数 == 0 ) )
					{
						CLane cレーン = this.mgr譜面管理者ref.listレーン[ cチップ.nレーン番号0to ];
						cチップ.nチャンネル番号00toFF = cチップ.b裏 ? cレーン.nチャンネル番号_裏00toFF : cレーン.nチャンネル番号_表00toFF;
						if( ( cレーン.bパターンレーンである() && ( cチップ.n値_整数1to1295 != 1 ) ) && ( cチップ.n値_整数1to1295 != 2 ) )
						{
							cチップ.n値_整数1to1295 = 1;
						}
					}
				}
			}
			this._Form.mgrUndoRedo管理者.tトランザクション記録を開始する();
			foreach( KeyValuePair<int, CMeasure> pair2 in this.mgr譜面管理者ref.dic小節 )
			{
				CMeasure c小節 = pair2.Value;
				for( int j = 0; j < pair2.Value.listチップ.Count; j++ )
				{
					CChip cチップ2 = c小節.listチップ[ j ];
					if( cチップ2.b確定選択中 && ( ( ( cチップ2.n移動開始時の小節番号0to != c小節.n小節番号0to3599 ) || ( cチップ2.n移動開始時のレーン番号0to != cチップ2.nレーン番号0to ) ) || ( cチップ2.n移動開始時の小節内の位置grid != cチップ2.n位置grid ) ) )
					{
						CChipPositionUndoRedo redo = new CChipPositionUndoRedo( cチップ2.n移動開始時の小節番号0to, cチップ2.n移動開始時のレーン番号0to, cチップ2.n移動開始時の小節内の位置grid, cチップ2.n値_整数1to1295 );
						CChipPositionUndoRedo redo2 = new CChipPositionUndoRedo( c小節.n小節番号0to3599, cチップ2.nレーン番号0to, cチップ2.n位置grid, cチップ2.n値_整数1to1295 );
						this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoCell<CChipPositionUndoRedo>( null, new DGUndoを実行する<CChipPositionUndoRedo>( this.tチップ移動のUndo ), new DGRedoを実行する<CChipPositionUndoRedo>( this.tチップ移動のRedo ), redo, redo2 ) );
					}
				}
			}
			this._Form.tUndoRedo用GUIの有効_無効を設定する();
			this._Form.mgrUndoRedo管理者.tトランザクション記録を終了する();
		}
		private void t画面上下にマウスカーソルがあるなら譜面を縦スクロールする( MouseEventArgs e )
		{
			if( e.Y < 70 )
			{
				int nGrid = -( 70 - e.Y ) / 2;
				int num2 = this._Form.vScrollBar譜面用垂直スクロールバー.Value;
				int num3 = this._Form.vScrollBar譜面用垂直スクロールバー.Value + nGrid;
				int minimum = this._Form.vScrollBar譜面用垂直スクロールバー.Minimum;
				int num5 = ( this._Form.vScrollBar譜面用垂直スクロールバー.Maximum + 1 ) - this._Form.vScrollBar譜面用垂直スクロールバー.LargeChange;
				if( num3 < minimum )
				{
					num3 = minimum;
				}
				else if( num3 > num5 )
				{
					num3 = num5;
				}
				this._Form.vScrollBar譜面用垂直スクロールバー.Value = num3;
				nGrid = num3 - num2;
				this.pt現在のドラッグ開始位置dot.Y -= CMeasure.n位置変換grid2dot( nGrid );
			}
			else if( e.Y > ( this._Form.pictureBox譜面パネル.Height - 50 ) )
			{
				int num6 = ( ( e.Y - this._Form.pictureBox譜面パネル.Height ) + 50 ) / 2;
				int num7 = this._Form.vScrollBar譜面用垂直スクロールバー.Value;
				int num8 = this._Form.vScrollBar譜面用垂直スクロールバー.Value + num6;
				int num9 = this._Form.vScrollBar譜面用垂直スクロールバー.Minimum;
				int num10 = ( this._Form.vScrollBar譜面用垂直スクロールバー.Maximum + 1 ) - this._Form.vScrollBar譜面用垂直スクロールバー.LargeChange;
				if( num8 < num9 )
				{
					num8 = num9;
				}
				else if( num8 > num10 )
				{
					num8 = num10;
				}
				this._Form.vScrollBar譜面用垂直スクロールバー.Value = num8;
				num6 = num8 - num7;
				this.pt現在のドラッグ開始位置dot.Y -= CMeasure.n位置変換grid2dot( num6 );
			}
		}
		private void t確定選択中のチップを移動する()
		{
			Point point = new Point();
			point.X = this.mgr譜面管理者ref.nX座標dotが位置するレーン番号を返す( this.pt現在のドラッグ終了位置dot.X );
			point.Y = this.mgr譜面管理者ref.nY座標dotが位置するgridを返す_ガイド幅単位( this.pt現在のドラッグ終了位置dot.Y );
			Point point2 = new Point();
			point2.X = point.X - this.pt前回の位置LaneGrid.X;
			point2.Y = point.Y - this.pt前回の位置LaneGrid.Y;
			if( ( point2.X != 0 ) || ( point2.Y != 0 ) )
			{
				foreach( KeyValuePair<int, CMeasure> pair in this.mgr譜面管理者ref.dic小節 )
				{
					pair.Value.t小節内の全チップの移動済フラグをリセットする();
				}
				for( int i = 0; i < this.mgr譜面管理者ref.dic小節.Count; i++ )
				{
					CMeasure c小節 = this.mgr譜面管理者ref.dic小節[ i ];
					for( int j = 0; j < c小節.listチップ.Count; j++ )
					{
						CChip cc = c小節.listチップ[ j ];
						if( cc.b確定選択中 && !cc.b移動済 )
						{
							if( point2.X != 0 )
							{
								this.tチップを横に移動する( cc, point2.X );
							}
							if( point2.Y != 0 )
							{
								this.tチップを縦に移動する( cc, point2.Y, c小節 );
								i = -1;
								break;
							}
						}
					}
				}
				this.pt前回の位置LaneGrid.X = point.X;
				this.pt前回の位置LaneGrid.Y = point.Y;
			}
		}
		private void t検索する_メイン()
		{
			CSearchDialog c検索ダイアログ = new CSearchDialog();
			if( !c検索ダイアログ.bレーンリストの内訳が生成済みである )
			{
				int count = this._Form.mgr譜面管理者.listレーン.Count;
				string[] strArray = new string[ count ];
				for( int i = 0; i < count; i++ )
				{
					strArray[ i ] = this._Form.mgr譜面管理者.listレーン[ i ].strレーン名;
				}
				c検索ダイアログ.tレーンリストの内訳を生成する( strArray );
			}
			if( c検索ダイアログ.ShowDialog() == DialogResult.OK )
			{
				int num3 = c検索ダイアログ.bチップ範囲指定CheckBoxがチェックされている ? c検索ダイアログ.nチップ範囲開始番号 : 0;
				int num4 = c検索ダイアログ.bチップ範囲指定CheckBoxがチェックされている ? c検索ダイアログ.nチップ範囲終了番号 : 36 * 36 - 1;
				if( ( c検索ダイアログ.bチップ範囲指定CheckBoxがチェックされている && ( num3 < 0 ) ) && ( num4 < 0 ) )
				{
					num3 = 0;
					num4 = 36 * 36 - 1;
				}
				int num5 = c検索ダイアログ.b小節範囲指定CheckBoxがチェックされている ? c検索ダイアログ.n小節範囲開始番号 : 0;
				int num6 = c検索ダイアログ.b小節範囲指定CheckBoxがチェックされている ? c検索ダイアログ.n小節範囲終了番号 : this._Form.mgr譜面管理者.n現在の最大の小節番号を返す();
				if( ( c検索ダイアログ.b小節範囲指定CheckBoxがチェックされている && ( num5 < 0 ) ) && ( num6 < 0 ) )
				{
					num5 = 0;
					num6 = this._Form.mgr譜面管理者.n現在の最大の小節番号を返す();
				}
				if( ( ( num5 >= 0 ) && ( num6 >= 0 ) ) && ( ( num3 >= 0 ) && ( num4 >= 0 ) ) )
				{
					this._Form.mgrUndoRedo管理者.tトランザクション記録を開始する();
					int num7 = 0;
					for( int j = num5; j <= num6; j++ )
					{
						CMeasure c小節 = this._Form.mgr譜面管理者.p小節を返す( j );
						if( c小節 != null )
						{
							for( int k = 0; k < c小節.listチップ.Count; k++ )
							{
								CChip cチップ = c小節.listチップ[ k ];
								if( ( ( !c検索ダイアログ.bレーン指定CheckBoxがチェックされている || c検索ダイアログ.bレーンが検索対象である( cチップ.nレーン番号0to ) ) && ( ( cチップ.n値_整数1to1295 >= num3 ) && ( cチップ.n値_整数1to1295 <= num4 ) ) ) && ( ( c検索ダイアログ.b表チップCheckBoxがチェックされている && !cチップ.b裏 ) || ( c検索ダイアログ.b裏チップCheckBoxがチェックされている && cチップ.b裏 ) ) )
								{
									CChipPositionUndoRedo redo = new CChipPositionUndoRedo( c小節.n小節番号0to3599, cチップ.nレーン番号0to, cチップ.n位置grid, cチップ.n値_整数1to1295 );
									this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoCell<CChipPositionUndoRedo>( null, new DGUndoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ選択のUndo ), new DGRedoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ選択のRedo ), redo, redo ) );
									cチップ.b確定選択中 = true;
									num7++;
								}
							}
						}
					}
					this._Form.mgrUndoRedo管理者.tトランザクション記録を終了する();
					this._Form.tUndoRedo用GUIの有効_無効を設定する();
					this._Form.pictureBox譜面パネル.Refresh();
					if( num7 > 0 )
					{
						this._Form.t選択チップの有無に応じて編集用GUIの有効_無効を設定する();
						MessageBox.Show( num7 + Resources.str個のチップが選択されましたMSG, Resources.str検索結果ダイアログのタイトル, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1 );
					}
					else
					{
						MessageBox.Show( Resources.str該当するチップはありませんでしたMSG, Resources.str検索結果ダイアログのタイトル, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1 );
					}
				}
			}
		}
		private void t現在の選択範囲を描画する( Graphics g )
		{
			Rectangle rect = new Rectangle();
			rect.X = Math.Min( this.pt現在のドラッグ開始位置dot.X, this.pt現在のドラッグ終了位置dot.X );
			rect.Y = Math.Min( this.pt現在のドラッグ開始位置dot.Y, this.pt現在のドラッグ終了位置dot.Y );
			rect.Width = Math.Abs( (int) ( this.pt現在のドラッグ開始位置dot.X - this.pt現在のドラッグ終了位置dot.X ) );
			rect.Height = Math.Abs( (int) ( this.pt現在のドラッグ開始位置dot.Y - this.pt現在のドラッグ終了位置dot.Y ) );
			if( rect.Width < 0 )
			{
				rect.X = this.pt現在のドラッグ開始位置dot.X;
				rect.Width = this.pt現在のドラッグ開始位置dot.X - rect.X;
			}
			if( rect.Height < 0 )
			{
				rect.Y = this.pt現在のドラッグ開始位置dot.Y;
				rect.Height = this.pt現在のドラッグ開始位置dot.Y - rect.Y;
			}
			if( ( rect.Width != 0 ) && ( rect.Height != 0 ) )
			{
				g.FillRectangle( this.br選択領域ブラシ, rect );
				g.DrawRectangle( Pens.LightBlue, rect );
			}
		}
		private void t選択チップを単純置換する( int n元番号, int n先番号 )
		{
			if( ( n元番号 < 0 ) || ( n先番号 < 0 ) )
			{
				MessageBox.Show( Resources.strチップ番号に誤りがありますMSG, Resources.str置換結果ダイアログのタイトル, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1 );
			}
			else
			{
				int num = 0;
				this._Form.mgrUndoRedo管理者.tトランザクション記録を開始する();
				foreach( KeyValuePair<int, CMeasure> pair in this._Form.mgr譜面管理者.dic小節 )
				{
					CMeasure c小節 = pair.Value;
					for( int i = 0; i < c小節.listチップ.Count; i++ )
					{
						CChip cチップ = c小節.listチップ[ i ];
						if( cチップ.b確定選択中 && ( cチップ.n値_整数1to1295 == n元番号 ) )
						{
							CChipPositionUndoRedo redo = new CChipPositionUndoRedo( c小節.n小節番号0to3599, cチップ.nレーン番号0to, cチップ.n位置grid, cチップ.n値_整数1to1295 );
							CChipPositionUndoRedo redo2 = new CChipPositionUndoRedo( c小節.n小節番号0to3599, cチップ.nレーン番号0to, cチップ.n位置grid, n先番号 );
							this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoCell<CChipPositionUndoRedo>( null, new DGUndoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ番号置換のUndo ), new DGRedoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ番号置換のRedo ), redo, redo2 ) );
							cチップ.n値_整数1to1295 = n先番号;
							num++;
						}
					}
				}
				this._Form.mgrUndoRedo管理者.tトランザクション記録を終了する();
				this._Form.tUndoRedo用GUIの有効_無効を設定する();
				if( num > 0 )
				{
					this._Form.b未保存 = true;
					this._Form.pictureBox譜面パネル.Refresh();
					MessageBox.Show( num + Resources.str個のチップを置換しましたMSG, Resources.str置換結果ダイアログのタイトル, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1 );
				}
				else
				{
					MessageBox.Show( Resources.str該当するチップはありませんでしたMSG, Resources.str置換結果ダイアログのタイトル, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1 );
				}
			}
		}
		private void t選択チップを表裏反転置換する()
		{
			int num = 0;
			this._Form.mgrUndoRedo管理者.tトランザクション記録を開始する();
			foreach( KeyValuePair<int, CMeasure> pair in this._Form.mgr譜面管理者.dic小節 )
			{
				CMeasure c小節 = pair.Value;
				for( int i = 0; i < c小節.listチップ.Count; i++ )
				{
					CChip cチップ = c小節.listチップ[ i ];
					if( cチップ.b確定選択中 )
					{
						CChipPositionUndoRedo redo = new CChipPositionUndoRedo( c小節.n小節番号0to3599, cチップ.nレーン番号0to, cチップ.n位置grid, cチップ.n値_整数1to1295 );
						this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoCell<CChipPositionUndoRedo>( null, new DGUndoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ表裏反転のUndo ), new DGRedoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ表裏反転のRedo ), redo, redo ) );
						if( cチップ.b裏 )
						{
							cチップ.nチャンネル番号00toFF = this._Form.mgr譜面管理者.listレーン[ cチップ.nレーン番号0to ].nチャンネル番号_表00toFF;
							cチップ.b裏 = false;
						}
						else
						{
							cチップ.nチャンネル番号00toFF = this._Form.mgr譜面管理者.listレーン[ cチップ.nレーン番号0to ].nチャンネル番号_裏00toFF;
							cチップ.b裏 = true;
						}
						num++;
					}
				}
			}
			this._Form.mgrUndoRedo管理者.tトランザクション記録を終了する();
			this._Form.tUndoRedo用GUIの有効_無効を設定する();
			if( num > 0 )
			{
				this._Form.b未保存 = true;
				this._Form.pictureBox譜面パネル.Refresh();
				MessageBox.Show( num + Resources.str個のチップを置換しましたMSG, Resources.str置換結果ダイアログのタイトル, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1 );
			}
		}
		private void t置換する_メイン()
		{
			CReplaceDialog c置換ダイアログ = new CReplaceDialog();
			bool flag = false;
			if( !this._Form.mgr譜面管理者.b確定選択中のチップがある() )
			{
				this.t全チップを選択する();
				flag = true;
			}
			int num = -1;
			bool flag2 = true;
			foreach( KeyValuePair<int, CMeasure> pair in this._Form.mgr譜面管理者.dic小節 )
			{
				foreach( CChip cチップ in pair.Value.listチップ )
				{
					if( cチップ.b確定選択中 )
					{
						if( num < 0 )
						{
							num = cチップ.n値_整数1to1295;
						}
						else if( num != cチップ.n値_整数1to1295 )
						{
							flag2 = false;
							break;
						}
					}
				}
				if( !flag2 )
				{
					break;
				}
			}
			if( flag2 )
			{
				c置換ダイアログ.n元番号 = num;
			}
			if( c置換ダイアログ.ShowDialog() == DialogResult.OK )
			{
				if( c置換ダイアログ.b表裏反転RadioButtonがチェックされている )
				{
					this.t選択チップを表裏反転置換する();
				}
				else if( c置換ダイアログ.b単純置換RadioButtonがチェックされている )
				{
					this.t選択チップを単純置換する( c置換ダイアログ.n元番号, c置換ダイアログ.n先番号 );
				}
			}
			else if( flag )
			{
				this.t全チップの選択を解除する();
			}
		}
		private void t範囲選択開始処理( MouseEventArgs e )
		{
			this.b範囲選択のためにドラッグ中 = true;
			this.pt現在のドラッグ開始位置dot.X = this.pt現在のドラッグ終了位置dot.X = e.X;
			this.pt現在のドラッグ開始位置dot.Y = this.pt現在のドラッグ終了位置dot.Y = e.Y;
			if( ( Control.ModifierKeys & Keys.Control ) != Keys.Control )
			{
				this.t全チップの選択を解除する();
			}
			this.tドラッグ範囲中のチップを選択する();
		}
		private void t範囲選択継続処理( MouseEventArgs e )
		{
			this.pt現在のドラッグ終了位置dot.X = e.X;
			this.pt現在のドラッグ終了位置dot.Y = e.Y;
			this.t画面上下にマウスカーソルがあるなら譜面を縦スクロールする( e );
			this.tドラッグ範囲中のチップを選択する();
		}
		private void t範囲選択終了処理( MouseEventArgs e )
		{
			this.b範囲選択のためにドラッグ中 = false;
			this._Form.mgrUndoRedo管理者.tトランザクション記録を開始する();
			foreach( KeyValuePair<int, CMeasure> pair in this.mgr譜面管理者ref.dic小節 )
			{
				CMeasure c小節 = pair.Value;
				for( int i = 0; i < c小節.listチップ.Count; i++ )
				{
					CChip cチップ = c小節.listチップ[ i ];
					if( cチップ.bドラッグで選択中 && !cチップ.b確定選択中 )
					{
						CChipPositionUndoRedo redo = new CChipPositionUndoRedo( c小節.n小節番号0to3599, cチップ.nレーン番号0to, cチップ.n位置grid, cチップ.n値_整数1to1295 );
						this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoCell<CChipPositionUndoRedo>( null, new DGUndoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ選択のUndo ), new DGRedoを実行する<CChipPositionUndoRedo>( this.mgr譜面管理者ref.tチップ選択のRedo ), redo, redo ) );
					}
				}
			}
			this._Form.mgrUndoRedo管理者.tトランザクション記録を終了する();
			this._Form.tUndoRedo用GUIの有効_無効を設定する();
			foreach( KeyValuePair<int, CMeasure> pair2 in this.mgr譜面管理者ref.dic小節 )
			{
				CMeasure c小節2 = pair2.Value;
				for( int j = 0; j < c小節2.listチップ.Count; j++ )
				{
					CChip cチップ2 = c小節2.listチップ[ j ];
					if( cチップ2.bドラッグで選択中 )
					{
						cチップ2.bドラッグで選択中 = false;
						cチップ2.b確定選択中 = true;
					}
				}
			}
			this._Form.t選択チップの有無に応じて編集用GUIの有効_無効を設定する();
		}
		//-----------------
		#endregion
	}
}
