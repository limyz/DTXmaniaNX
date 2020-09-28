using System;
using System.Collections.Generic;
using System.Text;

namespace DTXMania
{
	internal class CActPerfGuitarCombo : CActPerfCommonCombo
	{
		// CActPerfCommonCombo 実装

		protected override void tDrawCombo_Guitar( int nCombo値, int nジャンプインデックス )
		{
            int x = 560;
            int y = 220;

            base.tDrawCombo_Guitar(nCombo値, nジャンプインデックス, x, y);
        }
		protected override void tDrawCombo_Drums( int nCombo値, int nジャンプインデックス )
		{
		}
		protected override void tDrawCombo_Bass( int nCombo値, int nジャンプインデックス )
		{
            int x = 845;
            int y = 220;

            base.tDrawCombo_Bass(nCombo値, nジャンプインデックス, x, y);
        }
	}
}
