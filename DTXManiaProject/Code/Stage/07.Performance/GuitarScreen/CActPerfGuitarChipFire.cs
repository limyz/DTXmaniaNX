using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DTXMania
{
	internal class CActPerfGuitarChipFire : CActPerfChipFireGB
	{
		// コンストラクタ

		public CActPerfGuitarChipFire()
		{
			base.bNotActivated = true;
		}
		
		
		// メソッド

		public override void Start( int nLane )
		{
			if( ( nLane < 0 ) && ( nLane > 9 ) )
			{
				throw new IndexOutOfRangeException();
			}
			EInstrumentPart e楽器パート = ( nLane < 5 ) ? EInstrumentPart.GUITAR : EInstrumentPart.BASS;
			int index = nLane;

            //LEFT時のY座標
			if( CDTXMania.ConfigIni.bLeft[ (int) e楽器パート ] )
			{
				index = ( ( index / 5 ) * 5 ) + ( 4 - ( index % 5 ) );
			}
			int x = this.pt中央[ index ].X;
            int y = (CDTXMania.ConfigIni.bReverse[(int)e楽器パート] ? 611 - CDTXMania.ConfigIni.nJudgeLine[(int)e楽器パート] : 155 + CDTXMania.ConfigIni.nJudgeLine[(int)e楽器パート]);

            if ( CDTXMania.ConfigIni.eAttackEffect[ (int) e楽器パート ] != EType.B )
			    base.Start( nLane, x, y );
		}


		// その他

		#region [ private ]
		//-----------------
        private readonly Point[] pt中央 = new Point[] { new Point(107, 155), new Point(146, 155), new Point(185, 155), new Point(224, 155), new Point(264, 155), new Point(978, 155), new Point(1017, 155), new Point(1056, 155), new Point(1095, 155), new Point(1134, 155) };
		//-----------------
		#endregion
	}
}
