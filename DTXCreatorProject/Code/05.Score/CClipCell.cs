using System;
using System.Collections.Generic;
using System.Text;

namespace DTXCreator.Score
{
	internal class CClipCell  // Cクリップセル
	{
		public bool b貼り付け済;
		public int nグループID;
		public int nレーン番号 = -1;
		public int n位置grid;
		public CChip pチップ;
	}
}
