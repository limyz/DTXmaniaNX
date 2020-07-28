using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SlimDX;
using FDK;

namespace DTXMania
{
    internal class CAct演奏Drumsスコア : CAct演奏スコア共通
    {
        // CActivity 実装（共通クラスからの差分のみ）

        public override void On活性化()
        {
            this.n本体X[0] = 40;
            this.n本体Y = 13;

            base.On活性化();
        }

        public override unsafe int On進行描画()
        {
            if (!base.b活性化してない)
            {
                if (base.b初めての進行描画)
                {
                    base.n進行用タイマ = CDTXMania.Timer.n現在時刻;
                    base.b初めての進行描画 = false;
                }
                long num = CDTXMania.Timer.n現在時刻;
                if (num < base.n進行用タイマ)
                {
                    base.n進行用タイマ = num;
                }
                while ((num - base.n進行用タイマ) >= 10)
                {
                        this.n現在表示中のスコア[0] += this.nスコアの増分[0];

                        if (this.n現在表示中のスコア[0] > (long)this.n現在の本当のスコア[0])
                            this.n現在表示中のスコア[0] = (long)this.n現在の本当のスコア[0];
                    base.n進行用タイマ += 10;
                }
                string str = string.Format("{0,7:######0}", this.n現在表示中のスコア[0]);
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
                    if( base.txScore != null )
                    {
                        base.txScore.t2D描画(CDTXMania.app.Device, this.n本体X[0] + (i * 34), 28 + this.n本体Y, rectangle);
                    }
                }
                if( base.txScore != null )
                {
                    base.txScore.t2D描画(CDTXMania.app.Device, this.n本体X[0], this.n本体Y, new Rectangle(0, 50, 86, 28));
                }
            }
            return 0;
        }
        #region [ private ]
        //-----------------
        //-----------------
        #endregion
    }
}
