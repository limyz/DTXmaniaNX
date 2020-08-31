using System;
using System.Collections.Generic;
using System.Text;

namespace DTXCreator.Score
{
	public class CMeasureUndoRedo  // C小節用UndoRedo
	{
		public float f倍率;
		public int n小節番号0to;

		public CMeasureUndoRedo( int n小節番号0to, float f倍率 )
		{
			this.n小節番号0to = n小節番号0to;
			this.f倍率 = f倍率;
		}
	}
}
