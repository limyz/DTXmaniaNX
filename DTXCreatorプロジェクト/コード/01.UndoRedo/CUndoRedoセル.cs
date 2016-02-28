using System;
using System.Collections.Generic;
using System.Text;

namespace DTXCreator.UndoRedo
{
	internal class CUndoRedoセル<T> : CUndoRedoセル仮想
	{
		public T 変更後の値;
		public T 変更前の値;

		// Methods
		public CUndoRedoセル( object 所有者objectID, DGUndoを実行する<T> undoメソッド, DGRedoを実行する<T> redoメソッド, T 変更前の値, T 変更後の値 )
		{
			base.所有者ID = 所有者objectID;
			this.undoデリゲート = undoメソッド;
			this.redoデリゲート = redoメソッド;
			this.変更前の値 = 変更前の値;
			this.変更後の値 = 変更後の値;
		}
		public override void tRedoを実行する()
		{
			if( this.redoデリゲート == null )
			{
				throw new Exception( "Redoデリゲートが未設定です。" );
			}
			base.所有者ID = null;
			this.redoデリゲート( this.変更前の値, this.変更後の値 );
		}
		public override void tUndoを実行する()
		{
			if( this.undoデリゲート == null )
			{
				throw new Exception( "Undoデリゲートが未設定です。" );
			}
			base.所有者ID = null;
			this.undoデリゲート( this.変更前の値, this.変更後の値 );
		}

		#region [ private ]
		//-----------------
		private DGRedoを実行する<T> redoデリゲート;
		private DGUndoを実行する<T> undoデリゲート;
		//-----------------
		#endregion

	}
}
