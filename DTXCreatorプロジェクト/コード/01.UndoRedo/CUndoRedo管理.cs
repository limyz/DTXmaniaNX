using System;
using System.Collections.Generic;
using System.Text;

namespace DTXCreator.UndoRedo
{
	public delegate void DGRedoを実行する<T>( T 変更前の値, T 変更後の値 );
	public delegate void DGUndoを実行する<T>( T 変更前の値, T 変更後の値 );

	internal class CUndoRedo管理
	{
		public static bool bUndoRedoした直後;

		public CUndoRedo管理()
		{
			this.urd現在のリストノード = this.root;
		}
		public CUndoRedoセル仮想 tRedoするノードを取得して返す()
		{
			this.urd現在のリストノード = this.root;
			if( this.root.nRedo可能な回数 <= 0 )
			{
				return null;
			}
			this.root.n次にノードが追加される位置0to++;
			return this.root.listノード[ this.root.n次にノードが追加される位置0to - 1 ];
		}
		public CUndoRedoセル仮想 tUndoするノードを取得して返す()
		{
			this.urd現在のリストノード = this.root;
			if( this.root.nUndo可能な回数 <= 0 )
			{
				return null;
			}
			this.root.n次にノードが追加される位置0to--;
			return this.root.listノード[ this.root.n次にノードが追加される位置0to ];
		}
		public CUndoRedoセル仮想 tUndoするノードを取得して返す・見るだけ()
		{
			this.urd現在のリストノード = this.root;
			if( this.root.nUndo可能な回数 <= 0 )
			{
				return null;
			}
			return this.root.listノード[ this.root.n次にノードが追加される位置0to - 1 ];
		}
		public void tノードを追加する( CUndoRedoセル仮想 ur単独ノード )
		{
			int index = this.urd現在のリストノード.n次にノードが追加される位置0to;
			int count = this.urd現在のリストノード.n現在の総ノード数 - this.urd現在のリストノード.n次にノードが追加される位置0to;
			if( count > 0 )
			{
				this.urd現在のリストノード.listノード.RemoveRange( index, count );
			}
			this.urd現在のリストノード.listノード.Add( ur単独ノード );
			this.urd現在のリストノード.n次にノードが追加される位置0to++;
		}
		public void tトランザクション記録を開始する()
		{
			// リストノードを追加して開く。

			int index = this.urd現在のリストノード.n次にノードが追加される位置0to;
			int count = this.urd現在のリストノード.n現在の総ノード数 - this.urd現在のリストノード.n次にノードが追加される位置0to;
			if( count > 0 )
			{
				this.urd現在のリストノード.listノード.RemoveRange( index, count );
			}
			CUndoRedoディレクトリ item = new CUndoRedoディレクトリ( this.urd現在のリストノード );
			this.urd現在のリストノード.listノード.Add( item );
			this.urd現在のリストノード.n次にノードが追加される位置0to++;
			this.urd現在のリストノード = item;
		}
		public void tトランザクション記録を終了する()
		{
			// リストノードを閉じる。

			if( this.urd現在のリストノード.urd親ノード != null )
			{
				CUndoRedoディレクトリ item = this.urd現在のリストノード;
				this.urd現在のリストノード = this.urd現在のリストノード.urd親ノード;
				if( item.listノード.Count == 0 )
				{
					this.urd現在のリストノード.listノード.Remove( item );
					this.urd現在のリストノード.n次にノードが追加される位置0to--;
				}
			}
		}
		public void t空にする()
		{
			this.root = new CUndoRedoディレクトリ( null );
			this.urd現在のリストノード = this.root;
		}

		public int nRedo可能な回数
		{
			get
			{
				return ( this.n現在の総ノード数 - this.nUndo可能な回数 );
			}
		}
		public int nUndo可能な回数
		{
			get
			{
				return this.root.nUndo可能な回数;
			}
		}
		public int n現在の総ノード数
		{
			get
			{
				return this.root.n現在の総ノード数;
			}
		}

		#region [ private ]
		//-----------------
		private CUndoRedoディレクトリ root = new CUndoRedoディレクトリ( null );
		private CUndoRedoディレクトリ urd現在のリストノード;
		//-----------------
		#endregion
	}
}
