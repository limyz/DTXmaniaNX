using System;
using System.Collections.Generic;
using System.Text;

namespace DTXCreator.UndoRedo
{
	internal abstract class CUndoRedoセル仮想
	{
		public E種別 eノード種別;
		public enum E種別
		{
			単独,
			リスト
		}
		protected object 所有者ID;

		protected CUndoRedoセル仮想()
		{
		}
		public bool b所有権がある( object 所有者候補 )
		{
			if( this.所有者ID != 所有者候補 )
			{
				return false;
			}
			return true;
		}
		public void t所有権の放棄( object 現所有者 )
		{
			if( this.所有者ID == 現所有者 )
			{
				this.所有者ID = null;
			}
		}
		public abstract void tRedoを実行する();
		public abstract void tUndoを実行する();
	}
}
