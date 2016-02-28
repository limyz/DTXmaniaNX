using System;
using System.Collections.Generic;
using System.Text;

namespace DTXCreator.譜面
{
	public class Cチップ配置用UndoRedo
	{
		public Cチップ cc;
		public int n小節番号0to;

		public Cチップ配置用UndoRedo( int n小節番号0to, Cチップ cc )
		{
			this.n小節番号0to = n小節番号0to;
			this.cc = cc;
		}
	}
}
