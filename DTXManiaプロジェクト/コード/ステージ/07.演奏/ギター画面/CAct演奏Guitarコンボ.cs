using System;
using System.Collections.Generic;
using System.Text;

namespace DTXMania
{
	internal class CAct演奏Guitarコンボ : CAct演奏Combo共通
	{
		// CAct演奏Combo共通 実装

		protected override void tコンボ表示・ギター( int nCombo値, int nジャンプインデックス )
		{
            int x = 560;
            int y = 220;

            base.tコンボ表示・ギター(nCombo値, nジャンプインデックス, x, y);
        }
		protected override void tコンボ表示・ドラム( int nCombo値, int nジャンプインデックス )
		{
		}
		protected override void tコンボ表示・ベース( int nCombo値, int nジャンプインデックス )
		{
            int x = 845;
            int y = 220;

            base.tコンボ表示・ベース(nCombo値, nジャンプインデックス, x, y);
        }
	}
}
