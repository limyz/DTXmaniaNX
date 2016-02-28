using System;
using System.Collections.Generic;
using System.Text;
using DTXCreator.UndoRedo;

namespace DTXCreator.譜面
{
	internal class Cクリップボード
	{
		public int nセル数
		{
			get
			{
				return this.cbボード.Count;
			}
		}

		public Cクリップボード( Cメインフォーム form )
		{
			this._Form = form;
		}
		public void tクリアする()
		{
			this.cbボード.Clear();
		}
		public void tチップを指定位置から貼り付ける( C小節 cs配置開始小節, int n貼り付け先頭grid )
		{
			if( this.cbボード.Count != 0 )
			{
				List<Cクリップセル> list = new List<Cクリップセル>();
				foreach( Cクリップセル cクリップセル in this.cbボード )
				{
					Cクリップセル item = new Cクリップセル();
					item.pチップ = new Cチップ();
					item.pチップ.tコピーfrom( cクリップセル.pチップ );
					item.nレーン番号 = cクリップセル.nレーン番号;
					item.n位置grid = cクリップセル.n位置grid;
					item.b貼り付け済 = false;
					list.Add( item );
				}
				int num = list[ 0 ].n位置grid;
				foreach( Cクリップセル cクリップセル3 in list )
				{
					if( cクリップセル3.n位置grid < num )
					{
						num = cクリップセル3.n位置grid;
					}
				}
				for( int i = 0; i < list.Count; i++ )
				{
					Cクリップセル local1 = list[ i ];
					local1.n位置grid -= num;
					Cクリップセル local2 = list[ i ];
					local2.n位置grid += n貼り付け先頭grid;
				}
				this._Form.mgrUndoRedo管理者.tトランザクション記録を開始する();
				C小節 c小節 = cs配置開始小節;
				for( int j = 0; j < list.Count; j++ )
				{
					int num4 = list[ j ].n位置grid;
					if( ( num4 >= 0 ) && ( num4 < c小節.n小節長倍率を考慮した現在の小節の高さgrid ) )
					{
						Cチップ cチップ = new Cチップ();
						cチップ.tコピーfrom( list[ j ].pチップ );
						cチップ.n位置grid = num4;
						cチップ.bドラッグで選択中 = false;
						cチップ.b確定選択中 = true;
						c小節.listチップ.Add( cチップ );
						Cチップ cc = new Cチップ();
						cc.tコピーfrom( cチップ );
						Cチップ配置用UndoRedo redo = new Cチップ配置用UndoRedo( c小節.n小節番号0to3599, cc );
						this._Form.mgrUndoRedo管理者.tノードを追加する( new CUndoRedoセル<Cチップ配置用UndoRedo>( null, new DGUndoを実行する<Cチップ配置用UndoRedo>( this._Form.mgr譜面管理者.tチップ配置のUndo ), new DGRedoを実行する<Cチップ配置用UndoRedo>( this._Form.mgr譜面管理者.tチップ配置のRedo ), redo, redo ) );
						c小節 = cs配置開始小節;
					}
					else
					{
						Cクリップセル local3 = list[ j ];
						local3.n位置grid -= c小節.n小節長倍率を考慮した現在の小節の高さgrid;
						int num5 = c小節.n小節番号0to3599 + 1;
						c小節 = this._Form.mgr譜面管理者.p小節を返す( num5 );
						if( c小節 == null )
						{
							c小節 = new C小節( num5 );
							this._Form.mgr譜面管理者.dic小節.Add( num5, c小節 );
						}
						j--;
					}
				}
				this._Form.mgrUndoRedo管理者.tトランザクション記録を終了する();
				this._Form.tUndoRedo用GUIの有効・無効を設定する();
				list.Clear();
			}
		}
		public void tチップを追加する( Cチップ cc, int nレーン番号, int n位置grid )
		{
			Cクリップセル item = new Cクリップセル();
			item.pチップ = new Cチップ();
			item.pチップ.tコピーfrom( cc );
			item.nレーン番号 = nレーン番号;
			item.n位置grid = n位置grid;
			this.cbボード.Add( item );
		}
		public void t現在選択されているチップをボードにコピーする()
		{
			this.tクリアする();
			foreach( KeyValuePair<int, C小節> pair in this._Form.mgr譜面管理者.dic小節 )
			{
				C小節 c小節 = pair.Value;
				int num = this._Form.mgr譜面管理者.n譜面先頭からみた小節先頭の位置gridを返す( c小節.n小節番号0to3599 );
				foreach( Cチップ cチップ in c小節.listチップ )
				{
					if( cチップ.b確定選択中 )
					{
						this.tチップを追加する( cチップ, cチップ.nレーン番号0to, num + cチップ.n位置grid );
					}
				}
			}
		}

		#region [ private ]
		//-----------------
		private Cメインフォーム _Form;
		private List<Cクリップセル> cbボード = new List<Cクリップセル>();
		//-----------------
		#endregion
	}
}
