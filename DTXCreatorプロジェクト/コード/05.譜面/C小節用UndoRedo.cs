using System;
using System.Collections.Generic;
using System.Text;

namespace DTXCreator.譜面
{
	public class C小節用UndoRedo
	{
		public float f倍率;
		public int n小節番号0to;

		public C小節用UndoRedo( int n小節番号0to, float f倍率 )
		{
			this.n小節番号0to = n小節番号0to;
			this.f倍率 = f倍率;
		}
	}
}
