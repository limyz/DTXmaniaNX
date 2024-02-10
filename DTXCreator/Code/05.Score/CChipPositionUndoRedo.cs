using System;
using System.Collections.Generic;
using System.Text;

namespace DTXCreator.Score
{
	public class CChipPositionUndoRedo  // Cチップ位置用UndoRedo
	{
		public int nレーン番号0to;
		public int n位置grid;
		public int n小節番号0to;
		public int n値_整数1to3843;

		public CChipPositionUndoRedo( int n小節番号0to, int nレーン番号0to, int n位置grid, int n値_整数0to3843 )
		{
			this.n小節番号0to = n小節番号0to;
			this.nレーン番号0to = nレーン番号0to;
			this.n位置grid = n位置grid;
			this.n値_整数1to3843 = n値_整数0to3843;
		}
	}
}
