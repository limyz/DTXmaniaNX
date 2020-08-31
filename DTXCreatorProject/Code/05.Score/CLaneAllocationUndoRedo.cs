using System;
using System.Collections.Generic;
using System.Text;

namespace DTXCreator.Score
{
	public class CLaneAllocationUndoRedo  // Cレーン割付用UndoRedo
	{
		public bool b裏;
		public CLane lc;
		public int n番号0or1to1295;

		public CLaneAllocationUndoRedo( CLane lc, int n番号0or1to1295, bool b裏 )
		{
			this.lc = lc;
			this.n番号0or1to1295 = n番号0or1to1295;
			this.b裏 = b裏;
		}
	}
}
