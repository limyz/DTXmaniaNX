using System;
using System.Collections.Generic;
using System.Text;

namespace DTXMania
{
	internal class CActPerfGuitarCombo : CActPerfCommonCombo
	{
		// CActPerfCommonCombo 実装

		protected override void tコンボ表示_ギター( int nCombo値, int nジャンプインデックス )
		{
            int x = 560;
            int y = 220;

            base.tコンボ表示_ギター(nCombo値, nジャンプインデックス, x, y);
        }
		protected override void tコンボ表示_ドラム( int nCombo値, int nジャンプインデックス )
		{
		}
		protected override void tコンボ表示_ベース( int nCombo値, int nジャンプインデックス )
		{
            int x = 845;
            int y = 220;

            base.tコンボ表示_ベース(nCombo値, nジャンプインデックス, x, y);
        }
	}
}
