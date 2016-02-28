using System;
using System.Collections.Generic;
using System.Text;

namespace DTXCreator.譜面
{
	public class Cチップ位置用UndoRedo
	{
		public int nレーン番号0to;
		public int n位置grid;
		public int n小節番号0to;
		public int n値・整数1to1295;

		public Cチップ位置用UndoRedo( int n小節番号0to, int nレーン番号0to, int n位置grid, int n値・整数0to1295 )
		{
			this.n小節番号0to = n小節番号0to;
			this.nレーン番号0to = nレーン番号0to;
			this.n位置grid = n位置grid;
			this.n値・整数1to1295 = n値・整数0to1295;
		}
	}
}
