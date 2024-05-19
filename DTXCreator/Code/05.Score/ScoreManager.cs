using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DTXCreator.UndoRedo;
using FDK;

namespace DTXCreator.Score
{
	internal class ScoreManager  // C譜面管理
	{
		public Bitmap bmp背景画像;
		public Dictionary<int, float> dicBPx = new Dictionary<int, float>();
		public Dictionary<int, CMeasure> dic小節 = new Dictionary<int, CMeasure>();
		public List<CLane> listレーン = new List<CLane>();
		public static readonly int nレーン割付チップ番号表示高さdot = 10;
		public static readonly int nレーン番号表示高さdot = 0x20;
		public int n現在のガイド幅grid = ( CMeasure.n基準の高さgrid / 0x10 );
		public int n現在の譜面表示下辺の譜面先頭からの位置grid;
		public string strPATH_WAV = "";

		public ScoreManager( CMainForm cm )
		{
			this._Form = cm;
		}
		public bool bOPENチップである( CChip cc )
		{
			CLane cレーン = this.listレーン[ cc.nレーン番号0to ];
			return ( ( cc.n値_整数1to1295 == 2 ) && ( ( cレーン.eレーン種別 == CLane.E種別.GtR ) || ( cレーン.eレーン種別 == CLane.E種別.BsR ) ) );
		}
		public bool b確定選択中のチップがある()
		{
			foreach( KeyValuePair<int, CMeasure> pair in this.dic小節 )
			{
				foreach( CChip cチップ in pair.Value.listチップ )
				{
					if( cチップ.b確定選択中 || cチップ.bドラッグで選択中 )
					{
						return true;
					}
				}
			}
			return false;
		}
		public decimal dc譜面先頭からの位置gridにおけるBPMを返す( int n譜面先頭からの位置grid )
		{
			decimal num = this._Form.dc現在のBPM;
			CMeasure c小節 = this.p譜面先頭からの位置gridを含む小節を返す( n譜面先頭からの位置grid );
			if( c小節 != null )
			{
				for( int i = 0; i <= c小節.n小節番号0to3599; i++ )
				{
					CMeasure c小節2 = this.p小節を返す( i );
					if( c小節2 != null )
					{
						int num3 = this.n譜面先頭からみた小節先頭の位置gridを返す( i );
						foreach( CChip cチップ in c小節2.listチップ )
						{
							if( ( cチップ.nチャンネル番号00toFF == 8 ) && ( ( num3 + cチップ.n位置grid ) <= n譜面先頭からの位置grid ) )
							{
								num = (decimal) cチップ.f値_浮動小数;
							}
						}
					}
				}
			}
			return num;
		}
		public int nX座標dotが位置するレーン番号を返す( int nXdot )
		{
			if( nXdot >= 0 )
			{
				int num = 0;
				int num2 = 0;
				foreach( CLane cレーン in this.listレーン )
				{
					num += cレーン.n幅dot;
					if( nXdot < num )
					{
						return num2;
					}
					num2++;
				}
			}
			return -1;
		}
		public int nY座標dotが位置するgridを返す_ガイド幅単位( int nY )
		{
			int num = this.nY座標dotが位置するgridを返す_最高解像度( nY );
			CMeasure c小節 = this.p譜面先頭からの位置gridを含む小節を返す( num );
			if( c小節 == null )
			{
				c小節 = this.p小節を返す( this.n現在の最大の小節番号を返す() );
			}
			int num2 = this.n譜面先頭からみた小節先頭の位置gridを返す( c小節.n小節番号0to3599 );
			int num3 = ( ( num - num2 ) / this.n現在のガイド幅grid ) * this.n現在のガイド幅grid;
			return ( num2 + num3 );
		}
		public int nY座標dotが位置するgridを返す_最高解像度( int nY )
		{
			int num = this._Form.pictureBox譜面パネル.ClientSize.Height - nY;
			return ( ( num / CMeasure.n１グリッドの高さdot ) + this.n現在の譜面表示下辺の譜面先頭からの位置grid );
		}
		public int nレーンの左端X座標dotを返す( int nレーン番号0to )
		{
			if( ( nレーン番号0to >= 0 ) && ( nレーン番号0to < this.listレーン.Count ) )
			{
				return this.listレーン[ nレーン番号0to ].n位置Xdot;
			}
			return -1;
		}
		public int nレーン名に対応するレーン番号を返す( string strレーン名 )
		{
			int nLaneNo = 0;
			foreach( CLane cレーン in this.listレーン )
			{
				if( cレーン.strレーン名.Equals( strレーン名 ) )
				{
					return nLaneNo;
				}
				nLaneNo++;
			}
			return -1;
		}
		public int n現在の最大の小節番号を返す()
		{
			int nBar = -1;
			foreach( KeyValuePair<int, CMeasure> pair in this.dic小節 )
			{
				CMeasure c小節 = pair.Value;
				if( c小節.n小節番号0to3599 > nBar )
				{
					nBar = c小節.n小節番号0to3599;
				}
			}
			return nBar;
		}
		public int n全小節の高さdotの合計を返す()
		{
			int nHeights = 0;
			foreach( KeyValuePair<int, CMeasure> pair in this.dic小節 )
			{
				nHeights += pair.Value.n小節長倍率を考慮した現在の小節の高さdot;
			}
			return nHeights;
		}
		public int n全小節の高さgridの合計を返す()
		{
			int num = 0;
			foreach( KeyValuePair<int, CMeasure> pair in this.dic小節 )
			{
				num += pair.Value.n小節長倍率を考慮した現在の小節の高さgrid;
			}
			return num;
		}
		public int n譜面先頭からの位置gridから描画領域内のY座標dotを返す( int n譜面先頭からの位置grid, Size sz描画領域dot )
		{
			int num = n譜面先頭からの位置grid - this.n現在の譜面表示下辺の譜面先頭からの位置grid;
			return ( sz描画領域dot.Height - ( num * CMeasure.n１グリッドの高さdot ) );
		}
		public int n譜面先頭からみた小節先頭の位置gridを返す( int n小節番号0to3599 )
		{
			if( ( n小節番号0to3599 < 0 ) || ( n小節番号0to3599 > this.n現在の最大の小節番号を返す() ) )
			{
				return -1;
			}
			int num = 0;
			for( int i = 0; i < n小節番号0to3599; i++ )
			{
				CMeasure c小節 = this.p小節を返す( i );
				if( c小節 != null )
				{
					num += c小節.n小節長倍率を考慮した現在の小節の高さgrid;
				}
			}
			return num;
		}
		public CMeasure pチップの存在する小節を返す( CChip cc )
		{
			foreach( KeyValuePair<int, CMeasure> pair in this.dic小節 )
			{
				CMeasure c小節 = pair.Value;
				if( c小節.listチップ.Contains( cc ) )
				{
					return c小節;
				}
			}
			return null;
		}
		public CChip p指定された座標dotにあるチップを返す( int x, int y )
		{
			int num = this.nX座標dotが位置するレーン番号を返す( x );
			if( num >= 0 )
			{
				int num2 = this.nY座標dotが位置するgridを返す_最高解像度( y );
				CMeasure c小節 = this.p譜面先頭からの位置gridを含む小節を返す( num2 );
				if( c小節 == null )
				{
					return null;
				}
				int num3 = this.n譜面先頭からみた小節先頭の位置gridを返す( c小節.n小節番号0to3599 );
				foreach( CChip cチップ in c小節.listチップ )
				{
					int num4 = num3 + cチップ.n位置grid;
					int num5 = CMeasure.n位置変換dot2grid( CChip.nチップの高さdot );
					if( ( ( cチップ.nレーン番号0to == num ) && ( num4 <= num2 ) ) && ( num2 <= ( num4 + num5 ) ) )
					{
						return cチップ;
					}
				}
			}
			return null;
		}
		public CChip p指定位置にあるチップを返す( int n小節番号0to, int nレーン番号0to, int n小節内の位置grid )
		{
			CMeasure c小節 = this.p小節を返す( n小節番号0to );
			if( c小節 != null )
			{
				for( int i = 0; i < c小節.listチップ.Count; i++ )
				{
					CChip cチップ = c小節.listチップ[ i ];
					if( ( cチップ.n位置grid == n小節内の位置grid ) && ( cチップ.nレーン番号0to == nレーン番号0to ) )
					{
						return cチップ;
					}
				}
			}
			return null;
		}
		public CMeasure p小節を返す( int n小節番号0to3599 )
		{
			CMeasure c小節 = null;
			if( this.dic小節.TryGetValue( n小節番号0to3599, out c小節 ) )
			{
				return c小節;
			}
			return null;
		}
		public CMeasure p譜面先頭からの位置gridを含む小節を返す( int n譜面先頭からの位置grid )
		{
			int num = 0;
			int num2 = this.n現在の最大の小節番号を返す();
			for( int i = 0; i <= num2; i++ )
			{
				CMeasure c小節 = this.p小節を返す( i );
				if( c小節 != null )
				{
					num += c小節.n小節長倍率を考慮した現在の小節の高さgrid;
					if( n譜面先頭からの位置grid < num )
					{
						return c小節;
					}
				}
			}
			return null;
		}
		public void tチップを削除する( int nレーン番号0to, int n譜面先頭からの位置grid )
		{
			CMeasure c小節 = this.p譜面先頭からの位置gridを含む小節を返す( n譜面先頭からの位置grid );
			if( c小節 != null )
			{
				bool flag3;
				int num = n譜面先頭からの位置grid - this.n譜面先頭からみた小節先頭の位置gridを返す( c小節.n小節番号0to3599 );
				this._Form.mgrUndoRedo管理者.tトランザクション記録を開始する();
				do
				{
					CLane cレーン = this.listレーン[ nレーン番号0to ];
                    bool flag = ((cレーン.eレーン種別 == CLane.E種別.GtR) || (cレーン.eレーン種別 == CLane.E種別.GtG)) || (cレーン.eレーン種別 == CLane.E種別.GtB) || (cレーン.eレーン種別 == CLane.E種別.GtY) || (cレーン.eレーン種別 == CLane.E種別.GtP);
                    bool flag2 = ((cレーン.eレーン種別 == CLane.E種別.BsR) || (cレーン.eレーン種別 == CLane.E種別.BsG)) || (cレーン.eレーン種別 == CLane.E種別.BsB) || (cレーン.eレーン種別 == CLane.E種別.BsY) || (cレーン.eレーン種別 == CLane.E種別.BsP);
					flag3 = true;
					foreach( CChip cチップ in c小節.listチップ )
					{
						if( cチップ.n位置grid == num )
						{
							if( cチップ.nレーン番号0to == nレーン番号0to )
							{
								CChip cc = new CChip();
								cc.tコピーfrom( cチップ );
								CChipLocationUndoRedo redo = new CChipLocationUndoRedo( c小節.n小節番号0to3599, cc );
								this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoCell<CChipLocationUndoRedo>( null, new DGUndoを実行する<CChipLocationUndoRedo>( this.tチップ削除のUndo ), new DGRedoを実行する<CChipLocationUndoRedo>( this.tチップ削除のRedo ), redo, redo ) );
								c小節.listチップ.Remove( cチップ );
								flag3 = false;
								this._Form.b未保存 = true;
								break;
							}
							if( ( flag && ( this.listレーン[ cチップ.nレーン番号0to ].eレーン種別 == CLane.E種別.GtR ) ) && ( cチップ.n値_整数1to1295 == 2 ) )
							{
								CChip cチップ3 = new CChip();
								cチップ3.tコピーfrom( cチップ );
								CChipLocationUndoRedo redo2 = new CChipLocationUndoRedo( c小節.n小節番号0to3599, cチップ3 );
								this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoCell<CChipLocationUndoRedo>( null, new DGUndoを実行する<CChipLocationUndoRedo>( this.tチップ削除のUndo ), new DGRedoを実行する<CChipLocationUndoRedo>( this.tチップ削除のRedo ), redo2, redo2 ) );
								c小節.listチップ.Remove( cチップ );
								flag3 = false;
								this._Form.b未保存 = true;
								break;
							}
							if( ( flag2 && ( this.listレーン[ cチップ.nレーン番号0to ].eレーン種別 == CLane.E種別.BsR ) ) && ( cチップ.n値_整数1to1295 == 2 ) )
							{
								CChip cチップ4 = new CChip();
								cチップ4.tコピーfrom( cチップ );
								CChipLocationUndoRedo redo3 = new CChipLocationUndoRedo( c小節.n小節番号0to3599, cチップ4 );
								this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoCell<CChipLocationUndoRedo>( null, new DGUndoを実行する<CChipLocationUndoRedo>( this.tチップ削除のUndo ), new DGRedoを実行する<CChipLocationUndoRedo>( this.tチップ削除のRedo ), redo3, redo3 ) );
								c小節.listチップ.Remove( cチップ );
								flag3 = false;
								this._Form.b未保存 = true;
								break;
							}
						}
					}
				}
				while( !flag3 );
				this._Form.tUndoRedo用GUIの有効_無効を設定する();
				this._Form.mgrUndoRedo管理者.tトランザクション記録を終了する();
			}
		}
		public void tチップを配置または置換する( int nレーン番号0to, int n譜面先頭からの位置grid, int nチップ値_整数, float fチップ値_浮動小数, bool b裏として配置 )
		{
			CMeasure c小節 = this.p譜面先頭からの位置gridを含む小節を返す( n譜面先頭からの位置grid );
			if( c小節 != null )
			{
				this._Form.mgrUndoRedo管理者.tトランザクション記録を開始する();
				this.tチップを削除する( nレーン番号0to, n譜面先頭からの位置grid );
				if( ( ( this.listレーン[ nレーン番号0to ].eレーン種別 == CLane.E種別.GtR ) || ( this.listレーン[ nレーン番号0to ].eレーン種別 == CLane.E種別.BsR ) ) && ( nチップ値_整数 == 2 ) )
				{
					this.tチップを削除する( nレーン番号0to + 1, n譜面先頭からの位置grid );
					this.tチップを削除する( nレーン番号0to + 2, n譜面先頭からの位置grid );
				}
				CLane cレーン = this.listレーン[ nレーン番号0to ];
				CChip item = new CChip();
				item.nチャンネル番号00toFF = b裏として配置 ? cレーン.nチャンネル番号_裏00toFF : cレーン.nチャンネル番号_表00toFF;
				item.nレーン番号0to = nレーン番号0to;
				item.n位置grid = n譜面先頭からの位置grid - this.n譜面先頭からみた小節先頭の位置gridを返す( c小節.n小節番号0to3599 );
				item.n値_整数1to1295 = nチップ値_整数;
				item.f値_浮動小数 = fチップ値_浮動小数;
				item.b裏 = b裏として配置;
				c小節.listチップ.Add( item );
				CChip cc = new CChip();
				cc.tコピーfrom( item );
				CChipLocationUndoRedo redo = new CChipLocationUndoRedo( c小節.n小節番号0to3599, cc );
				this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoCell<CChipLocationUndoRedo>( null, new DGUndoを実行する<CChipLocationUndoRedo>( this.tチップ配置のUndo ), new DGRedoを実行する<CChipLocationUndoRedo>( this.tチップ配置のRedo ), redo, redo ) );
				int num = this.n現在の最大の小節番号を返す();
				if( c小節.n小節番号0to3599 == num )
				{
					for( int i = num + 1; i <= ( num + 4 ); i++ )
					{
						this.dic小節.Add( i, new CMeasure( i ) );
						this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoCell<int>( null, new DGUndoを実行する<int>( this.t小節挿入のUndo ), new DGRedoを実行する<int>( this.t小節挿入のRedo ), i, i ) );
					}
				}
				this._Form.mgrUndoRedo管理者.tトランザクション記録を終了する();
				this._Form.tUndoRedo用GUIの有効_無効を設定する();
				this._Form.b未保存 = true;
			}
		}
		public void tチップ削除のRedo( CChipLocationUndoRedo ur変更前, CChipLocationUndoRedo ur変更後 )
		{
			CMeasure c小節 = this.p小節を返す( ur変更前.n小節番号0to );
			if( c小節 != null )
			{
				for( int i = 0; i < c小節.listチップ.Count; i++ )
				{
					CChip cチップ = c小節.listチップ[ i ];
					if( ( cチップ.n位置grid == ur変更前.cc.n位置grid ) && ( cチップ.nレーン番号0to == ur変更前.cc.nレーン番号0to ) )
					{
						c小節.listチップ.RemoveAt( i );
						this._Form.b未保存 = true;
						return;
					}
				}
			}
		}
		public void tチップ削除のUndo( CChipLocationUndoRedo ur変更前, CChipLocationUndoRedo ur変更後 )
		{
			CMeasure c小節 = this.p小節を返す( ur変更前.n小節番号0to );
			if( c小節 != null )
			{
				CChip item = new CChip();
				item.tコピーfrom( ur変更前.cc );
				c小節.listチップ.Add( item );
				this._Form.b未保存 = true;
			}
		}
		public void tチップ選択のRedo( CChipPositionUndoRedo ur変更前, CChipPositionUndoRedo ur変更後 )
		{
			CChip cチップ = this.p指定位置にあるチップを返す( ur変更前.n小節番号0to, ur変更前.nレーン番号0to, ur変更前.n位置grid );
			if( cチップ != null )
			{
				cチップ.b確定選択中 = true;
			}
		}
		public void tチップ選択のUndo( CChipPositionUndoRedo ur変更前, CChipPositionUndoRedo ur変更後 )
		{
			CChip cチップ = this.p指定位置にあるチップを返す( ur変更前.n小節番号0to, ur変更前.nレーン番号0to, ur変更前.n位置grid );
			if( cチップ != null )
			{
				cチップ.b確定選択中 = false;
			}
		}
		public void tチップ選択解除のRedo( CChipPositionUndoRedo ur変更前, CChipPositionUndoRedo ur変更後 )
		{
			CChip cチップ = this.p指定位置にあるチップを返す( ur変更前.n小節番号0to, ur変更前.nレーン番号0to, ur変更前.n位置grid );
			if( cチップ != null )
			{
				cチップ.b確定選択中 = false;
			}
		}
		public void tチップ選択解除のUndo( CChipPositionUndoRedo ur変更前, CChipPositionUndoRedo ur変更後 )
		{
			CChip cチップ = this.p指定位置にあるチップを返す( ur変更前.n小節番号0to, ur変更前.nレーン番号0to, ur変更前.n位置grid );
			if( cチップ != null )
			{
				cチップ.b確定選択中 = true;
			}
		}
		public void tチップ配置のRedo( CChipLocationUndoRedo ur変更前, CChipLocationUndoRedo ur変更後 )
		{
			CMeasure c小節 = this.p小節を返す( ur変更前.n小節番号0to );
			if( c小節 != null )
			{
				CChip item = new CChip();
				item.tコピーfrom( ur変更前.cc );
				c小節.listチップ.Add( item );
				this._Form.b未保存 = true;
			}
		}
		public void tチップ配置のUndo( CChipLocationUndoRedo ur変更前, CChipLocationUndoRedo ur変更後 )
		{
			CMeasure c小節 = this.p小節を返す( ur変更前.n小節番号0to );
			if( c小節 != null )
			{
				for( int i = 0; i < c小節.listチップ.Count; i++ )
				{
					CChip cチップ = c小節.listチップ[ i ];
					if( ( cチップ.n位置grid == ur変更前.cc.n位置grid ) && ( cチップ.nレーン番号0to == ur変更前.cc.nレーン番号0to ) )
					{
						c小節.listチップ.RemoveAt( i );
						this._Form.b未保存 = true;
						return;
					}
				}
			}
		}
		public void tチップ番号置換のRedo( CChipPositionUndoRedo ur変更前, CChipPositionUndoRedo ur変更後 )
		{
			CChip cチップ = this.p指定位置にあるチップを返す( ur変更前.n小節番号0to, ur変更前.nレーン番号0to, ur変更前.n位置grid );
			if( cチップ != null )
			{
				cチップ.n値_整数1to1295 = ur変更後.n値_整数1to1295;
				this._Form.b未保存 = true;
			}
		}
		public void tチップ番号置換のUndo( CChipPositionUndoRedo ur変更前, CChipPositionUndoRedo ur変更後 )
		{
			CChip cチップ = this.p指定位置にあるチップを返す( ur変更後.n小節番号0to, ur変更後.nレーン番号0to, ur変更後.n位置grid );
			if( cチップ != null )
			{
				cチップ.n値_整数1to1295 = ur変更前.n値_整数1to1295;
				this._Form.b未保存 = true;
			}
		}
		public void tチップ表裏反転のRedo( CChipPositionUndoRedo ur変更前, CChipPositionUndoRedo ur変更後 )
		{
			CChip cチップ = this.p指定位置にあるチップを返す( ur変更前.n小節番号0to, ur変更前.nレーン番号0to, ur変更前.n位置grid );
			if( cチップ != null )
			{
				if( cチップ.b裏 )
				{
					cチップ.nチャンネル番号00toFF = this.listレーン[ cチップ.nレーン番号0to ].nチャンネル番号_表00toFF;
					cチップ.b裏 = false;
				}
				else
				{
					cチップ.nチャンネル番号00toFF = this.listレーン[ cチップ.nレーン番号0to ].nチャンネル番号_裏00toFF;
					cチップ.b裏 = true;
				}
				this._Form.b未保存 = true;
			}
		}
		public void tチップ表裏反転のUndo( CChipPositionUndoRedo ur変更前, CChipPositionUndoRedo ur変更後 )
		{
			CChip cチップ = this.p指定位置にあるチップを返す( ur変更前.n小節番号0to, ur変更前.nレーン番号0to, ur変更前.n位置grid );
			if( cチップ != null )
			{
				if( cチップ.b裏 )
				{
					cチップ.nチャンネル番号00toFF = this.listレーン[ cチップ.nレーン番号0to ].nチャンネル番号_表00toFF;
					cチップ.b裏 = false;
				}
				else
				{
					cチップ.nチャンネル番号00toFF = this.listレーン[ cチップ.nレーン番号0to ].nチャンネル番号_裏00toFF;
					cチップ.b裏 = true;
				}
				this._Form.b未保存 = true;
			}
		}
		public void t初期化()
		{
			this.t初期化_listレーンの生成();
			this.t初期化_小節を１０個まで作成する();
			this.t初期化_背景画像を生成する();
			this.t初期化_スクロールバーを初期設定する();
		}
		public void t小節削除のRedo( int nダミー, int n削除する小節番号0to )
		{
			this.dic小節.Remove( n削除する小節番号0to );
			int num = this.n現在の最大の小節番号を返す();
			for( int i = n削除する小節番号0to + 1; i <= num; i++ )
			{
				CMeasure c小節 = this.p小節を返す( i );
				if( c小節 != null )
				{
					this.dic小節.Remove( i );
					c小節.n小節番号0to3599 = i - 1;
					this.dic小節.Add( c小節.n小節番号0to3599, c小節 );
				}
			}
			this._Form.b未保存 = true;
		}
		public void t小節削除のUndo( int n削除された小節番号0to, int nダミー )
		{
			for( int i = this.n現在の最大の小節番号を返す(); i >= n削除された小節番号0to; i-- )
			{
				CMeasure c小節 = this.p小節を返す( i );
				if( c小節 != null )
				{
					this.dic小節.Remove( i );
					c小節.n小節番号0to3599 = i + 1;
					this.dic小節.Add( c小節.n小節番号0to3599, c小節 );
				}
			}
			this.dic小節.Add( n削除された小節番号0to, new CMeasure( n削除された小節番号0to ) );
			this._Form.b未保存 = true;
		}
		public void t小節挿入のRedo( int nダミー, int n挿入する小節番号0to )
		{
			for( int i = this.n現在の最大の小節番号を返す(); i >= n挿入する小節番号0to; i-- )
			{
				CMeasure c小節 = this.p小節を返す( i );
				if( c小節 != null )
				{
					this.dic小節.Remove( i );
					c小節.n小節番号0to3599 = i + 1;
					this.dic小節.Add( c小節.n小節番号0to3599, c小節 );
				}
			}
			this.dic小節.Add( n挿入する小節番号0to, new CMeasure( n挿入する小節番号0to ) );
			this._Form.b未保存 = true;
		}
		public void t小節挿入のUndo( int n挿入された小節番号0to, int nダミー )
		{
			this.dic小節.Remove( n挿入された小節番号0to );
			int num = this.n現在の最大の小節番号を返す();
			for( int i = n挿入された小節番号0to + 1; i <= num; i++ )
			{
				CMeasure c小節 = this.p小節を返す( i );
				if( c小節 != null )
				{
					this.dic小節.Remove( i );
					c小節.n小節番号0to3599 = i - 1;
					this.dic小節.Add( c小節.n小節番号0to3599, c小節 );
				}
			}
			this._Form.b未保存 = true;
		}
		public void t小節長変更のRedo( CMeasureUndoRedo ur変更前, CMeasureUndoRedo ur変更後 )
		{
			CMeasure c小節 = this.p小節を返す( ur変更後.n小節番号0to );
			if( c小節 != null )
			{
				c小節.f小節長倍率 = ur変更後.f倍率;
				this._Form.b未保存 = true;
			}
		}
		public void t小節長変更のUndo( CMeasureUndoRedo ur変更前, CMeasureUndoRedo ur変更後 )
		{
			CMeasure c小節 = this.p小節を返す( ur変更前.n小節番号0to );
			if( c小節 != null )
			{
				c小節.f小節長倍率 = ur変更前.f倍率;
				this._Form.b未保存 = true;
			}
		}
		public void t垂直スクロールバーと譜面可視領域の上下位置の調整()
		{
			VScrollBar bar = this._Form.vScrollBar譜面用垂直スクロールバー;
			PictureBox box = this._Form.pictureBox譜面パネル;
			bar.Minimum = 0;
			bar.Maximum = this.n全小節の高さgridの合計を返す() - 1;
			bar.SmallChange = CMeasure.n基準の高さgrid / 0x10;
			bar.LargeChange = CMeasure.n基準の高さgrid;
			bar.Enabled = bar.Maximum > bar.LargeChange;
			if( bar.Enabled )
			{
				this.n現在の譜面表示下辺の譜面先頭からの位置grid = ( ( bar.Maximum + 1 ) - bar.LargeChange ) - bar.Value;
				box.Refresh();
			}
		}
		public void t水平スクロールバーと譜面パネル左右位置の調整()
		{
			HScrollBar bar = this._Form.hScrollBar譜面用水平スクロールバー;
			SplitContainer container = this._Form.splitContainerタブと譜面を分割;
			PictureBox box = this._Form.pictureBox譜面パネル;
			bar.Minimum = 0;
			bar.Maximum = box.Width - 1;
			bar.SmallChange = 5;
			bar.LargeChange = container.Panel2.Width;
			bar.Enabled = bar.Maximum > bar.LargeChange;
			if( bar.Enabled )
			{
				int num = bar.Value;
				if( ( num + bar.LargeChange ) > bar.Maximum )
				{
					num = ( bar.Maximum - bar.LargeChange ) + 1;
				}
				bar.Value = num;
				box.Location = new Point( -num, 0 );
				box.Refresh();
			}
		}
		public void t譜面を描画する( Graphics g, Size sz描画対象サイズdot, Rectangle rc可視領域dot )
		{
			this.strfmt小節番号文字フォーマット.LineAlignment = StringAlignment.Center;
			this.strfmt小節番号文字フォーマット.Alignment = StringAlignment.Center;
			this.strfmtレーン名文字フォーマット.LineAlignment = StringAlignment.Near;
			this.strfmtレーン名文字フォーマット.Alignment = StringAlignment.Center;
			this.strfmtレーン割付チップ番号文字フォーマット.LineAlignment = StringAlignment.Near;
			this.strfmtレーン割付チップ番号文字フォーマット.Alignment = StringAlignment.Near;
			int h = 0;
			int num2 = 0;
			int num3 = this.n現在の譜面表示下辺の譜面先頭からの位置grid * CMeasure.n１グリッドの高さdot;
			int num4 = num3 + rc可視領域dot.Height;
			int maxBar = this.n現在の最大の小節番号を返す();
			int bar = 0;
			while( bar <= maxBar )
			{
				CMeasure cs = this.p小節を返す( bar );
				if( cs != null )
				{
					num2 = h + cs.n小節長倍率を考慮した現在の小節の高さdot;
					if( h >= num4 )
					{
						break;
					}
					if( num2 > num3 )
					{
						Rectangle rectangle = new Rectangle( 0, sz描画対象サイズdot.Height - ( num2 - num3 ), sz描画対象サイズdot.Width, num2 - h );
						Rectangle rectangle2 = new Rectangle( rc可視領域dot.X, rectangle.Y, rc可視領域dot.Width, rectangle.Height );
						this.t譜面を描画する_１小節を描画する( g, cs, rectangle, rectangle2 );
					}
				}
				bar++;
				h = num2;
			}
			Rectangle rectangle3 = new Rectangle( 0, 0, sz描画対象サイズdot.Width, nレーン割付チップ番号表示高さdot );
			this.t譜面を描画する_レーン割付チップを描画する( g, rectangle3 );
			rectangle3 = new Rectangle( 0, 10, sz描画対象サイズdot.Width, nレーン番号表示高さdot );
			this.t譜面を描画する_レーン名を描画する( g, rectangle3 );
		}

		/// <summary>
		/// 指定した種類のレーンを非表示にする
		/// </summary>
		/// <param name="eLaneType">非表示にするレーンの種類</param>
		public void tCollapseLanes( CLane.ELaneType eLaneType )
		{
			// なお、格納/展開状態を、#DTXV_COLLAPSED_LANES とかでDTXファイルに残した方がいいかも。
			int count = this.listレーン.Count;
			for ( int i = 0; i < this.listレーン.Count; i++ )
			{
				CLane c = listレーン[ i ];
				if ( c.eLaneType == eLaneType && c.bIsVisible == true )	// 対象レーンが見つかったら
				{
					this.listレーン[ i ].bIsVisible = false;
					this.listレーン[ i ].n幅dot = 0;
				}
			}
			this.tRefreshDisplayLanes();
		}
		/// <summary>
		/// 指定した種類のレーンを表示する
		/// </summary>
		/// <param name="eLaneType">表示にするレーンの種類</param>
		public void tExpandLanes( CLane.ELaneType eLaneType )
		{
			// tCollapseLanes()の反対の処理をする。

			for ( int i = 0; i < this.listレーン.Count; i++ )				// 以下本番
			{
				CLane c = listレーン[ i ];
				if ( c.eLaneType == eLaneType && c.bIsVisible == false )	// 対象レーンが見つかったら
				{
					this.listレーン[ i ].bIsVisible = true;
					this.listレーン[ i ].n幅dot = CLane.LANEWIDTH;
				}
			}
			this.tRefreshDisplayLanes();
		}

		/// <summary>
		/// レーンの表示/非表示を反映する
		/// </summary>
		/// <param name="eLaneType">表示にするレーンの種類</param>
		public void tRefreshDisplayLanes()
		{
			this.tRecalc_n位置XdotX();											// レーン位置が変わったので、レーン毎のX座標を再計算
			this.t初期化_背景画像を生成する();									// レーン数が変わったので、レーン画像を納める背景も再生成
			this.t水平スクロールバーと譜面パネル左右位置の調整();				// レーン数が変わったので、スクロールバーの長さも再調整
		}

		private void tRecalc_n位置XdotX()										// n位置Xdotの再計算
		{
			int x = 0;
			int count = this.listレーン.Count;
			for ( int i = 0; i < count; i++ )
			{
				this.listレーン[ i ].n位置Xdot = x;
				x += this.listレーン[ i ].n幅dot;
			}
		}


		#region [ private ]
		//-----------------
		private CMainForm _Form;
		private Brush brレーン割付番号文字ブラシ = new SolidBrush( Color.White );
		private Brush brレーン割付番号文字ブラシ影 = new SolidBrush( Color.Black );
		private Brush brレーン名文字ブラシ = new SolidBrush( Color.FromArgb( 0xff, 220, 220, 220 ) );
		private Brush brレーン名文字ブラシ影 = new SolidBrush( Color.Black );
		private Brush br小節番号文字ブラシ = new SolidBrush( Color.FromArgb( 80, 0xff, 0xff, 0xff ) );
		private Font ftレーン割付チップ番号文字フォント = new Font( "MS UI Gothic", 7f, FontStyle.Regular );
		private Font ftレーン番号文字フォント = new Font( "MS US Gothic", 8f, FontStyle.Regular );
		private Font ft小節番号文字フォント = new Font( "MS UI Gothic", 50f, FontStyle.Regular );
		private Pen penガイド線ペン = new Pen( Color.FromArgb( 50, 50, 50 ) );
		private Pen penレーン区分線ペン細 = new Pen( Color.Gray );
		private Pen penレーン区分線ペン太 = new Pen( Color.White, 2f );
		private Pen pen小節線ペン = new Pen( Color.White, 2f );
		private Pen pen拍線ペン = new Pen( Color.Gray );
		private StringFormat strfmtレーン割付チップ番号文字フォーマット = new StringFormat();
		private StringFormat strfmtレーン名文字フォーマット = new StringFormat();
		private StringFormat strfmt小節番号文字フォーマット = new StringFormat();

		private void t初期化_listレーンの生成()
		{
			this.listレーン.Clear();
			int width = CLane.LANEWIDTH;
			int alpha = 0x19;

			this.listレーン.Add( new CLane( CLane.E種別.BPM, "BPM", 0x08, 0x03, true, Color.FromArgb( alpha, 160, 160, 160 ), 0, width, CLane.ELaneType.BPM, true ) );

			this.listレーン.Add( new CLane( CLane.E種別.WAV, "LC",  0x1a, 0x1a, true,  Color.FromArgb( alpha, 0xdf, 0x5f, 0x7f), 0, width, CLane.ELaneType.Drums, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "HH",  0x11, 0x18, false, Color.FromArgb( alpha, 0, 0xff, 0xff ), 0, width, CLane.ELaneType.Drums, true ) );
            this.listレーン.Add( new CLane( CLane.E種別.WAV, "LP",  0x1b, 0x1b, false, Color.FromArgb( alpha, 0xff, 0x9f, 0xcf), 0, width, CLane.ELaneType.Drums, true) );
            this.listレーン.Add( new CLane( CLane.E種別.WAV, "LB",  0x1c, 0x1c, false, Color.FromArgb( alpha, 0xff, 0x9f, 0xcf), 0, width, CLane.ELaneType.Drums, true));
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "SD",  0x12, 0x12, false, Color.FromArgb( alpha, 0xff, 0xff, 0 ), 0, width, CLane.ELaneType.Drums, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "HT",  0x14, 0x14, false, Color.FromArgb( alpha, 0, 0xff, 0 ), 0, width, CLane.ELaneType.Drums, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "BD",  0x13, 0x13, false, Color.FromArgb( alpha, 0xbf, 0xbf, 0xff), 0, width, CLane.ELaneType.Drums, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "LT",  0x15, 0x15, false, Color.FromArgb( alpha, 0xff, 0, 0 ), 0, width, CLane.ELaneType.Drums, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "FT",  0x17, 0x17, false, Color.FromArgb( alpha, 0xff, 0x7f, 0), 0, width, CLane.ELaneType.Drums, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "CY",  0x16, 0x16, false, Color.FromArgb( alpha, 0x9f, 0x9f, 0xff), 0, width, CLane.ELaneType.Drums, true ) );
            this.listレーン.Add( new CLane( CLane.E種別.WAV, "RD",  0x19, 0x19, false, Color.FromArgb( alpha, 0, 0xff, 0xff), 0, width, CLane.ELaneType.Drums, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.FI,  "FI",  0x53, 0x53, true,  Color.FromArgb( alpha, 0xff, 0xff, 0 ), 0, width, CLane.ELaneType.Drums, true ) );
            this.listレーン.Add( new CLane( CLane.E種別.FI,  "BN1",  0x4F, 0x4F, false, Color.FromArgb( alpha, 0xff, 0xff, 0 ), 0, width, CLane.ELaneType.Drums, true ) );
            this.listレーン.Add( new CLane( CLane.E種別.FI,  "BN2",  0x4E, 0x4E, false, Color.FromArgb( alpha, 0xff, 0xff, 0 ), 0, width, CLane.ELaneType.Drums, true ) );
            this.listレーン.Add( new CLane( CLane.E種別.FI,  "BN3",  0x4D, 0x4D, false, Color.FromArgb( alpha, 0xff, 0xff, 0 ), 0, width, CLane.ELaneType.Drums, true ) );
            this.listレーン.Add( new CLane( CLane.E種別.FI,  "BN4",  0x4C, 0x4C, false, Color.FromArgb( alpha, 0xff, 0xff, 0 ), 0, width, CLane.ELaneType.Drums, true ) );

            this.listレーン.Add( new CLane( CLane.E種別.WAV, "MLC", 0x87, 0x87, true,  Color.FromArgb( alpha, 0xdf, 0x5f, 0x7f), 0, width, CLane.ELaneType.Drums, true ) );
            this.listレーン.Add( new CLane( CLane.E種別.WAV, "MHH", 0x84, 0x84, false, Color.FromArgb( alpha, 0, 0xff, 0xff), 0, width, CLane.ELaneType.Drums, true));
            this.listレーン.Add( new CLane( CLane.E種別.WAV, "MCY", 0x85, 0x85, false, Color.FromArgb( alpha, 0x9f, 0x9f, 0xff), 0, width, CLane.ELaneType.Drums, true));
            this.listレーン.Add( new CLane( CLane.E種別.WAV, "MRD", 0x86, 0x86, false, Color.FromArgb( alpha, 0, 0xff, 0xff), 0, width, CLane.ELaneType.Drums, true));

			this.listレーン.Add( new CLane( CLane.E種別.WAV, "BGM", 0x01, 0x01, true,  Color.FromArgb( alpha, 160, 160, 160 ), 0, width, CLane.ELaneType.BGM, true ) );

			this.listレーン.Add( new CLane( CLane.E種別.WAV, "SE1", 0x61, 0x61, true,  Color.FromArgb( alpha, 160, 160, 160 ), 0, width, CLane.ELaneType.SE1_5, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "SE2", 0x62, 0x62, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, width, CLane.ELaneType.SE1_5, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "SE3", 0x63, 0x63, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, width, CLane.ELaneType.SE1_5, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "SE4", 0x64, 0x64, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, width, CLane.ELaneType.SE1_5, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "SE5", 0x65, 0x65, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, width, CLane.ELaneType.SE1_5, true ) );

			// SE6～32は、初期状態では非表示とする。(n幅dotを0にし、bIsVisibleをfalseにする)
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "SE6", 0x66, 0x66, true, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "SE7", 0x67, 0x67, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "SE8", 0x68, 0x68, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "SE9", 0x69, 0x69, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S10", 0x70, 0x70, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S11", 0x71, 0x71, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S12", 0x72, 0x72, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S13", 0x73, 0x73, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S14", 0x74, 0x74, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S15", 0x75, 0x75, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S16", 0x76, 0x76, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S17", 0x77, 0x77, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S18", 0x78, 0x78, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S19", 0x79, 0x79, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S20", 0x80, 0x80, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S21", 0x81, 0x81, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S22", 0x82, 0x82, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S23", 0x83, 0x83, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S24", 0x84, 0x84, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S25", 0x85, 0x85, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S26", 0x86, 0x86, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S27", 0x87, 0x87, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S28", 0x88, 0x88, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S29", 0x89, 0x89, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S30", 0x90, 0x90, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S31", 0x91, 0x91, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.WAV, "S32", 0x92, 0x92, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.SE6_32, false ) );

			this.listレーン.Add( new CLane( CLane.E種別.GtV, "GtV", 0,    0,    true,  Color.FromArgb( alpha, 160, 160, 160 ), 0, width, CLane.ELaneType.Guitar, true ) );
            this.listレーン.Add( new CLane( CLane.E種別.GtR, "GtR", 0,    0,    false, Color.FromArgb( alpha, 0xff, 0, 0 ), 0, width, CLane.ELaneType.Guitar, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.GtG, "GtG", 0,    0,    false, Color.FromArgb( alpha, 0, 0xff, 0 ), 0, width, CLane.ELaneType.Guitar, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.GtB, "GtB", 0,    0,    false, Color.FromArgb( alpha, 0, 0x80, 0xff ), 0, width, CLane.ELaneType.Guitar, true ) );
            this.listレーン.Add( new CLane( CLane.E種別.GtY, "GtY", 0,    0,    false, Color.FromArgb( alpha, 0xff, 0xff, 0 ), 0, width, CLane.ELaneType.Guitar, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.GtP, "GtP", 0,    0,    false, Color.FromArgb( alpha, 0xff, 0, 0xff ), 0, width, CLane.ELaneType.Guitar, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.GtW, "GtW", 0x28, 0x28, true,  Color.FromArgb( alpha, 160, 160, 160 ), 0, width, CLane.ELaneType.Guitar, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.GtL, "GtL", 0x2C, 0x2C, false,  Color.FromArgb( alpha, 240, 192, 160), 0, width, CLane.ELaneType.Guitar, true));

			this.listレーン.Add( new CLane( CLane.E種別.BsV, "BsV", 0,    0,    true,  Color.FromArgb( alpha, 160, 160, 160 ), 0, width, CLane.ELaneType.Bass, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.BsR, "BsR", 0,    0,    false, Color.FromArgb( alpha, 0xff, 0, 0 ), 0, width, CLane.ELaneType.Bass, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.BsG, "BsG", 0,    0,    false, Color.FromArgb( alpha, 0, 0xff, 0 ), 0, width, CLane.ELaneType.Bass, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.BsB, "BsB", 0,    0,    false, Color.FromArgb( alpha, 0, 0x80, 0xff ), 0, width, CLane.ELaneType.Bass, true ) );
            this.listレーン.Add( new CLane( CLane.E種別.BsY, "BsY", 0,    0,    false, Color.FromArgb( alpha, 0xff, 0xff, 0), 0, width, CLane.ELaneType.Bass, true));
            this.listレーン.Add( new CLane( CLane.E種別.BsP, "BsP", 0,    0,    false, Color.FromArgb( alpha, 0xff, 0, 0xff), 0, width, CLane.ELaneType.Bass, true));
            this.listレーン.Add( new CLane( CLane.E種別.BsW, "BsW", 0xa8, 0xa8, true,  Color.FromArgb( alpha, 160, 160, 160), 0, width, CLane.ELaneType.Bass, true));
			this.listレーン.Add( new CLane( CLane.E種別.BsL, "BsL", 0x2D, 0x2D, false,  Color.FromArgb( alpha, 240, 192, 160), 0, width, CLane.ELaneType.Bass, true));

			this.listレーン.Add( new CLane( CLane.E種別.AVI, "AVI", 0x54, 0x54, true,  Color.FromArgb( alpha, 160, 160, 160 ), 0, width, CLane.ELaneType.AVI1_2, true ) );
            //this.listレーン.Add( new Cレーン( Cレーン.E種別.AVI, "AVI2", 0x55, 0x55, false, Color.FromArgb(alpha, 160, 160, 160), 0, width, Cレーン.ELaneType.AVI1_2, true));

			this.listレーン.Add( new CLane( CLane.E種別.BMP, "BG1", 0x04, 0xc4, true,  Color.FromArgb( alpha, 160, 160, 160 ), 0, width, CLane.ELaneType.BGA1_5, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.BMP, "BG2", 0x07, 0xc7, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, width, CLane.ELaneType.BGA1_5, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.BMP, "BG3", 0x100, 0xd5, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, width, CLane.ELaneType.BGA1_5, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.BMP, "BG4", 0x56, 0xd6, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, width, CLane.ELaneType.BGA1_5, true ) );
			this.listレーン.Add( new CLane( CLane.E種別.BMP, "BG5", 0x57, 0xd7, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, width, CLane.ELaneType.BGA1_5, true ) );

			// BG6～8も、初期状態では非表示とする。(n幅dotを0にし、bIsVisibleをfalseにする)
			this.listレーン.Add( new CLane( CLane.E種別.BMP, "BG6", 0x58, 0xd8, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.BGA6_8, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.BMP, "BG7", 0x59, 0xd9, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.BGA6_8, false ) );
			this.listレーン.Add( new CLane( CLane.E種別.BMP, "BG8", 0x60, 0xe0, false, Color.FromArgb( alpha, 160, 160, 160 ), 0, 0, CLane.ELaneType.BGA6_8, false ) );

			this.tRecalc_n位置XdotX();	// 0で固定初期化していたn位置XdotXを、ここで計算する。
										// (初期化の際に1つ1つまじめに計算しても良いが、単純にコード記述量が減るのでここでまとめて計算している)

		}
		private void t初期化_スクロールバーを初期設定する()
		{
			this._Form.hScrollBar譜面用水平スクロールバー.Value = 0;
			this._Form.vScrollBar譜面用垂直スクロールバー.Value = 0;
			this.t水平スクロールバーと譜面パネル左右位置の調整();
			this.t垂直スクロールバーと譜面可視領域の上下位置の調整();
			this._Form.vScrollBar譜面用垂直スクロールバー.Value = ( this._Form.vScrollBar譜面用垂直スクロールバー.Maximum + 1 ) - this._Form.vScrollBar譜面用垂直スクロールバー.LargeChange;
		}
		private void t初期化_小節を１０個まで作成する()
		{
			for( int i = 0; i < 10; i++ )
			{
				this.dic小節.Add( i, new CMeasure( i ) );
			}
		}
		private void t初期化_背景画像を生成する()
		{
			int width = 0;
			foreach( CLane cレーン in this.listレーン )
			{
				width += cレーン.n幅dot;
			}
			this.bmp背景画像 = new Bitmap( width, 1 );
			Graphics graphics = Graphics.FromImage( this.bmp背景画像 );
			int x = 0;
			foreach( CLane cレーン in this.listレーン )
			{
				graphics.FillRectangle( new SolidBrush( cレーン.col背景色 ), x, 0, cレーン.n幅dot, this.bmp背景画像.Height );
				x += cレーン.n幅dot;
			}
			graphics.Dispose();
			this._Form.pictureBox譜面パネル.Width = this.bmp背景画像.Width;
			this._Form.pictureBox譜面パネル.BackgroundImage = this.bmp背景画像;
			this._Form.pictureBox譜面パネル.BackgroundImageLayout = ImageLayout.Tile;
		}
		private void t譜面を描画する_１小節を描画する( Graphics g, CMeasure cs, Rectangle rc小節のPicBox内描画領域, Rectangle rc小節のPicBox内可視領域 )
		{
			this.t譜面を描画する_１小節を描画する_小節番号を描画する( g, cs, rc小節のPicBox内可視領域 );
			this.t譜面を描画する_１小節を描画する_ガイド線を描画する( g, cs, rc小節のPicBox内描画領域 );
			this.t譜面を描画する_１小節を描画する_拍線を描画する( g, cs, rc小節のPicBox内描画領域 );
			this.t譜面を描画する_１小節を描画する_レーン区分線を描画する( g, cs, rc小節のPicBox内描画領域 );
			this.t譜面を描画する_１小節を描画する_小節線を描画する( g, cs, rc小節のPicBox内描画領域 );
			this.t譜面を描画する_１小節を描画する_チップを描画する( g, cs, rc小節のPicBox内描画領域 );
		}
		private void t譜面を描画する_１小節を描画する_ガイド線を描画する( Graphics g, CMeasure cs, Rectangle rc小節のPicBox内描画領域 )
		{
			if( this.n現在のガイド幅grid > 1 )
			{
				int num = cs.n小節長倍率を考慮した現在の小節の高さgrid;
				for( int i = 0; i < num; i += this.n現在のガイド幅grid )
				{
					int num3 = rc小節のPicBox内描画領域.Bottom - ( i * CMeasure.n１グリッドの高さdot );
					g.DrawLine( this.penガイド線ペン, rc小節のPicBox内描画領域.X, num3, rc小節のPicBox内描画領域.Right, num3 );
				}
			}
		}
		private void t譜面を描画する_１小節を描画する_チップを描画する( Graphics g, CMeasure cs, Rectangle rc小節のPicBox内描画領域 )
		{
			cs.listチップ.Sort();
			Rectangle rectangle = new Rectangle( 0, 0, 0, 0 );
			foreach( CChip cチップ in cs.listチップ )
			{
				if( cチップ.n枠外レーン数 != 0 )
				{
					continue;
				}
				int num = cチップ.n位置grid;
				CLane cレーン = this.listレーン[ cチップ.nレーン番号0to ];
				if ( !cレーン.bIsVisible )
				{
					continue;
				}
				rectangle.X = cレーン.n位置Xdot;
				rectangle.Y = ( rc小節のPicBox内描画領域.Bottom - ( num * CMeasure.n１グリッドの高さdot ) ) - CChip.nチップの高さdot;
				rectangle.Width = cレーン.n幅dot;
				rectangle.Height = CChip.nチップの高さdot;
				if( !cチップ.b裏 )
				{
					switch( cレーン.eレーン種別 )
					{
						case CLane.E種別.GtR:
						case CLane.E種別.GtG:
						case CLane.E種別.GtB:
                        case CLane.E種別.GtY:
                        case CLane.E種別.GtP:
                        case CLane.E種別.BsR:
						case CLane.E種別.BsG:
						case CLane.E種別.BsB:
						case CLane.E種別.BsY:
						case CLane.E種別.BsP:
							if( ( ( cレーン.eレーン種別 != CLane.E種別.GtR ) || ( cチップ.n値_整数1to1295 != 2 ) ) && ( ( cレーン.eレーン種別 != CLane.E種別.BsR ) || ( cチップ.n値_整数1to1295 != 2 ) ) )
							{
								CChip.t表チップを描画する( g, rectangle, -1, cレーン.col背景色 );
								break;
							}
							rectangle.Width = cレーン.n幅dot * 5;
							CChip.tOPENチップを描画する( g, rectangle );
							break;

						case CLane.E種別.BPM:
							CChip.t表チップを描画する( g, rectangle, cチップ.f値_浮動小数, cレーン.col背景色 );
							break;

						default:
							CChip.t表チップを描画する( g, rectangle, cチップ.n値_整数1to1295, cレーン.col背景色 );
							break;
					}
				}
				else if( cレーン.eレーン種別 == CLane.E種別.BPM )
				{
					CChip.t裏チップを描画する( g, rectangle, cチップ.f値_浮動小数, cレーン.col背景色 );
				}
				else
				{
					CChip.t裏チップを描画する( g, rectangle, cチップ.n値_整数1to1295, cレーン.col背景色 );
				}
				if ( cチップ.bドラッグで選択中 || cチップ.b確定選択中 )
				{
					CChip.tチップの周囲の太枠を描画する( g, rectangle );
				}
			}
		}
		private void t譜面を描画する_１小節を描画する_レーン区分線を描画する( Graphics g, CMeasure cs, Rectangle rc小節のPicBox内描画領域 )
		{
			int num = 0;
			foreach( CLane cレーン in this.listレーン )
			{
				Pen pen = cレーン.b左側の線が太線 ? this.penレーン区分線ペン太 : this.penレーン区分線ペン細;
				g.DrawLine( pen, num, rc小節のPicBox内描画領域.Top, num, rc小節のPicBox内描画領域.Bottom );
				num += cレーン.n幅dot;
			}
		}
		private void t譜面を描画する_１小節を描画する_小節線を描画する( Graphics g, CMeasure cs, Rectangle rc小節のPicBox内描画領域 )
		{
			Rectangle rectangle = rc小節のPicBox内描画領域;
			g.DrawLine( this.pen小節線ペン, rectangle.X, rectangle.Bottom, rectangle.Right, rectangle.Bottom );
			g.DrawLine( this.pen小節線ペン, rectangle.X, rectangle.Top, rectangle.Right, rectangle.Top );
		}
		private void t譜面を描画する_１小節を描画する_小節番号を描画する( Graphics g, CMeasure cs, Rectangle rc小節のPicBox内可視領域 )
		{
			g.DrawString( CConversion.strConvertNumberTo3DigitMeasureNumber( cs.n小節番号0to3599 ), this.ft小節番号文字フォント, this.br小節番号文字ブラシ, rc小節のPicBox内可視領域, this.strfmt小節番号文字フォーマット );
		}
		private void t譜面を描画する_１小節を描画する_拍線を描画する( Graphics g, CMeasure cs, Rectangle rc小節のPicBox内描画領域 )
		{
			int num = cs.n小節長倍率を考慮した現在の小節の高さgrid;
			for( int i = 0; i < num; i += CMeasure.n基準の高さgrid / 4 )
			{
				int num3 = rc小節のPicBox内描画領域.Bottom - ( i * CMeasure.n１グリッドの高さdot );
				g.DrawLine( this.pen拍線ペン, rc小節のPicBox内描画領域.X, num3, rc小節のPicBox内描画領域.Right, num3 );
			}
		}
		private void t譜面を描画する_レーン割付チップを描画する( Graphics g, Rectangle rc小節のPicBox内描画領域 )
		{
			LinearGradientBrush brush = new LinearGradientBrush( rc小節のPicBox内描画領域, Color.FromArgb( 0xff, 0, 0, 100 ), Color.FromArgb( 0xff, 100, 100, 0xff ), LinearGradientMode.Vertical );
			g.FillRectangle( brush, rc小節のPicBox内描画領域 );
			brush.Dispose();
			Rectangle layoutRectangle = new Rectangle( 0, 0, 0, 0 );
			foreach( CLane cレーン in this.listレーン )
			{
				if ( !cレーン.bIsVisible )
				{
					continue;
				}
				layoutRectangle.X = ( rc小節のPicBox内描画領域.X + cレーン.n位置Xdot ) + 1;
				layoutRectangle.Y = rc小節のPicBox内描画領域.Y + 1;
				layoutRectangle.Width = cレーン.n幅dot;
				layoutRectangle.Height = rc小節のPicBox内描画領域.Height;
				if ( cレーン.nレーン割付チップ_表0or1to1295 > 0 )
				{
					string s = CConversion.strConvertNumberTo2DigitBase36String( cレーン.nレーン割付チップ_表0or1to1295 );
					g.DrawString( s, this.ftレーン割付チップ番号文字フォント, this.brレーン割付番号文字ブラシ影, layoutRectangle, this.strfmtレーン割付チップ番号文字フォーマット );
					layoutRectangle.X--;
					layoutRectangle.Y--;
					g.DrawString( s, this.ftレーン割付チップ番号文字フォント, this.brレーン割付番号文字ブラシ, layoutRectangle, this.strfmtレーン割付チップ番号文字フォーマット );
					layoutRectangle.X++;
					layoutRectangle.Y++;
				}
				layoutRectangle.X += cレーン.n幅dot / 2;
				if ( cレーン.nレーン割付チップ_裏0or1to1295 > 0 )
				{
					string str2 = CConversion.strConvertNumberTo2DigitBase36String( cレーン.nレーン割付チップ_裏0or1to1295 );
					g.DrawString( str2, this.ftレーン割付チップ番号文字フォント, this.brレーン割付番号文字ブラシ影, layoutRectangle, this.strfmtレーン割付チップ番号文字フォーマット );
					layoutRectangle.X--;
					layoutRectangle.Y--;
					g.DrawString( str2, this.ftレーン割付チップ番号文字フォント, this.brレーン割付番号文字ブラシ, layoutRectangle, this.strfmtレーン割付チップ番号文字フォーマット );
				}
			}
		}
		private void t譜面を描画する_レーン名を描画する( Graphics g, Rectangle rcレーン名のPicBox内描画領域 )
		{
			LinearGradientBrush brush = new LinearGradientBrush( rcレーン名のPicBox内描画領域, Color.FromArgb( 0xff, 100, 100, 0xff ), Color.FromArgb( 0, 0, 0, 0xff ), LinearGradientMode.Vertical );
			g.FillRectangle( brush, rcレーン名のPicBox内描画領域 );
			brush.Dispose();
			Rectangle layoutRectangle = new Rectangle( 0, 0, 0, 0 );
			foreach( CLane cレーン in this.listレーン )
			{
				if ( !cレーン.bIsVisible )
				{
					continue;
				}
				layoutRectangle.X = ( rcレーン名のPicBox内描画領域.X + cレーン.n位置Xdot ) + 2;
				layoutRectangle.Y = rcレーン名のPicBox内描画領域.Y + 2;
				layoutRectangle.Width = cレーン.n幅dot;
				layoutRectangle.Height = 0x18;
				g.DrawString( cレーン.strレーン名, this.ftレーン番号文字フォント, this.brレーン名文字ブラシ影, layoutRectangle, this.strfmtレーン名文字フォーマット );
				layoutRectangle.X -= 2;
				layoutRectangle.Y -= 2;
				g.DrawString( cレーン.strレーン名, this.ftレーン番号文字フォント, this.brレーン名文字ブラシ, layoutRectangle, this.strfmtレーン名文字フォーマット );
			}
		}
		//-----------------
		#endregion
	}
}
