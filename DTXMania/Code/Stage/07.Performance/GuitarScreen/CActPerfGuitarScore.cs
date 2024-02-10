using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CActPerfGuitarScore : CActPerfCommonScore
	{
		// コンストラクタ

		public CActPerfGuitarScore()
		{
			base.bNotActivated = true;
		}

        public override void OnActivate()
        {

            #region [ 本体位置 ]

            {
                this.n本体X[1] = 373;
                this.n本体X[2] = 665;

                this.n本体Y = 12;
            }

            if (CDTXMania.ConfigIni.bGraph有効.Guitar || CDTXMania.ConfigIni.bGraph有効.Bass )
            {
                if (!CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay && CDTXMania.ConfigIni.bAllBassAreAutoPlay)
                {
                    this.n本体X[2] = 0;
                }
                else if (CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay && !CDTXMania.ConfigIni.bAllBassAreAutoPlay)
                {
                    this.n本体X[1] = 0;
                }
            }

            #endregion

            base.OnActivate();
        }

		// CActivity 実装（共通クラスからの差分のみ）

		public override unsafe int OnUpdateAndDraw()
		{
			if( !base.bNotActivated && !CDTXMania.DTXVmode.Enabled && !CDTXMania.DTX2WAVmode.Enabled)
			{
				if( base.bJustStartedUpdate )
				{
					base.n進行用タイマ = CDTXMania.Timer.nCurrentTime;
					base.bJustStartedUpdate = false;
				}
                if (CDTXMania.stagePerfGuitarScreen.bIsTrainingMode)
                {
                    this.n現在表示中のスコア[0] = 0;
                }
                else
                {
                    long num = CDTXMania.Timer.nCurrentTime;
                    if (num < base.n進行用タイマ)
                    {
                        base.n進行用タイマ = num;
                    }
                    while ((num - base.n進行用タイマ) >= 10)
                    {
                        for (int j = 1; j < 3; j++)
                        {
                            this.n現在表示中のスコア[j] += this.nスコアの増分[j];

                            if (this.n現在表示中のスコア[j] > (long)this.nCurrentTrueScore[j])
                                this.n現在表示中のスコア[j] = (long)this.nCurrentTrueScore[j];
                        }
                        base.n進行用タイマ += 10;
                    }
                }
				for( int j = 1; j < 3; j++ )
                {
                    if ( CDTXMania.DTX.bチップがある[j] && n本体X[j] != 0 )
                    {
                        string str = string.Format("{0,7:######0}", this.n現在表示中のスコア[j]);
                        for (int i = 0; i < 7; i++)
                        {
                            Rectangle rectangle;
                            char ch = str[i];
                            if (ch.Equals(' '))
                            {
                                rectangle = new Rectangle(0, 0, 0, 0);
                            }
                            else
                            {
                                int num4 = int.Parse(str.Substring(i, 1));
                                rectangle = new Rectangle(num4 * 36, 0, 36, 50);
                            }
                            if (base.txScore != null)
                            {
                                base.txScore.tDraw2D(CDTXMania.app.Device, n本体X[j] + (i * 34), 28 + this.n本体Y, rectangle);
                            }
                        }
                        if (base.txScore != null)
                        {
                            base.txScore.tDraw2D(CDTXMania.app.Device, this.n本体X[j], this.n本体Y, new Rectangle(0, 50, 86, 28));
                        }
                    }
                }
			}
			return 0;
		}
	}
}
