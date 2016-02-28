using System;
using System.Collections.Generic;
using System.Text;

namespace DTXCreator.譜面
{
	public class Cレーン割付用UndoRedo
	{
		public bool b裏;
		public Cレーン lc;
		public int n番号0or1to1295;

		public Cレーン割付用UndoRedo( Cレーン lc, int n番号0or1to1295, bool b裏 )
		{
			this.lc = lc;
			this.n番号0or1to1295 = n番号0or1to1295;
			this.b裏 = b裏;
		}
	}
}
