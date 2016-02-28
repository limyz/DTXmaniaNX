using System;
using System.Collections.Generic;
using System.Text;

namespace DTXCreator.UndoRedo
{
	internal class CUndoRedoディレクトリ : CUndoRedoセル仮想
	{
		public List<CUndoRedoセル仮想> listノード;
		public int n次にノードが追加される位置0to;
		public CUndoRedoディレクトリ urd親ノード;
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
				return this.n次にノードが追加される位置0to;
			}
		}
		public int n現在の総ノード数
		{
			get
			{
				if( this.listノード == null )
				{
					return 0;
				}
				return this.listノード.Count;
			}
		}

		public CUndoRedoディレクトリ( CUndoRedoディレクトリ urd親ノード )
		{
			this.urd親ノード = urd親ノード;
			this.listノード = new List<CUndoRedoセル仮想>();
			this.n次にノードが追加される位置0to = 0;
		}
		public override void tRedoを実行する()
		{
			foreach( CUndoRedoセル仮想 oセル仮想 in this.listノード )
			{
				oセル仮想.tRedoを実行する();
			}
		}
		public override void tUndoを実行する()
		{
			for( int i = this.listノード.Count - 1; i >= 0; i-- )
			{
				this.listノード[ i ].tUndoを実行する();
			}
		}
	}
}
