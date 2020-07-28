using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CAct演奏Guitarスコア : CAct演奏スコア共通
	{
		// コンストラクタ

		public CAct演奏Guitarスコア()
		{
			base.b活性化してない = true;
		}

        public override void On活性化()
        {

            #region [ 本体位置 ]

            {
                this.n本体X[1] = 373;
                this.n本体X[2] = 665;

                this.n本体Y = 12;
            }

            if (CDTXMania.ConfigIni.bGraph有効.Guitar || CDTXMania.ConfigIni.bGraph有効.Bass )
            {
                if (!CDTXMania.ConfigIni.bギターが全部オートプレイである && CDTXMania.ConfigIni.bベースが全部オートプレイである)
                {
                    this.n本体X[2] = 0;
                }
                else if (CDTXMania.ConfigIni.bギターが全部オートプレイである && !CDTXMania.ConfigIni.bベースが全部オートプレイである)
                {
                    this.n本体X[1] = 0;
                }
            }

            #endregion

            base.On活性化();
        }

		// CActivity 実装（共通クラスからの差分のみ）

		public override unsafe int On進行描画()
		{
			if( !base.b活性化してない )
			{
				if( base.b初めての進行描画 )
				{
					base.n進行用タイマ = CDTXMania.Timer.n現在時刻;
					base.b初めての進行描画 = false;
				}
				long num = CDTXMania.Timer.n現在時刻;
				if( num < base.n進行用タイマ )
				{
					base.n進行用タイマ = num;
				}
				while( ( num - base.n進行用タイマ ) >= 10 )
				{
					for( int j = 1; j < 3; j++ )
					{
						this.n現在表示中のスコア[ j ] += this.nスコアの増分[ j ];

						if( this.n現在表示中のスコア[ j ] > (long) this.n現在の本当のスコア[ j ] )
							this.n現在表示中のスコア[ j ] = (long) this.n現在の本当のスコア[ j ];
					}
					base.n進行用タイマ += 10;
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
                                base.txScore.t2D描画(CDTXMania.app.Device, n本体X[j] + (i * 34), 28 + this.n本体Y, rectangle);
                            }
                        }
                        if (base.txScore != null)
                        {
                            base.txScore.t2D描画(CDTXMania.app.Device, this.n本体X[j], this.n本体Y, new Rectangle(0, 50, 86, 28));
                        }
                    }
                }
			}
			return 0;
		}

		// その他

		#region [ private ]
		//-----------------
		//-----------------
		#endregion
	}
}
