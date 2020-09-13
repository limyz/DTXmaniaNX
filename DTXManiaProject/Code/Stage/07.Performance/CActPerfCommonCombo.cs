using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using SlimDX;
using FDK;

namespace DTXMania
{
    internal class CActPerfCommonCombo : CActivity
    {
        // プロパティ

        public STCOMBO nCurrentCombo;
        public struct STCOMBO
        {
            public CActPerfCommonCombo act;

            public int this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return this.Drums;

                        case 1:
                            return this.Guitar;

                        case 2:
                            return this.Bass;
                    }
                    throw new IndexOutOfRangeException();
                }
                set
                {
                    switch (index)
                    {
                        case 0:
                            this.Drums = value;
                            return;

                        case 1:
                            this.Guitar = value;
                            return;

                        case 2:
                            this.Bass = value;
                            return;
                    }
                    throw new IndexOutOfRangeException();
                }
            }
            public int Drums
            {
                get
                {
                    return this.drums;
                }
                set
                {
                    this.drums = value;
                    if (this.drums > this.HighestValue.Drums)
                    {
                        this.HighestValue.Drums = this.drums;
                    }
                    this.act.status.Drums.nCOMBO値 = this.drums;
                    this.act.status.Drums.n最高COMBO値 = this.HighestValue.Drums;
                }
            }
            public int Guitar
            {
                get
                {
                    return this.guitar;
                }
                set
                {
                    this.guitar = value;
                    if (this.guitar > this.HighestValue.Guitar)
                    {
                        this.HighestValue.Guitar = this.guitar;
                    }
                    this.act.status.Guitar.nCOMBO値 = this.guitar;
                    this.act.status.Guitar.n最高COMBO値 = this.HighestValue.Guitar;
                }
            }
            public int Bass
            {
                get
                {
                    return this.bass;
                }
                set
                {
                    this.bass = value;
                    if (this.bass > this.HighestValue.Bass)
                    {
                        this.HighestValue.Bass = this.bass;
                    }
                    this.act.status.Bass.nCOMBO値 = this.bass;
                    this.act.status.Bass.n最高COMBO値 = this.HighestValue.Bass;
                }
            }
            public STDGBVALUE<int> HighestValue;

            private int drums;
            private int guitar;
            private int bass;
        }

        protected enum EEvent { 非表示, 数値更新, 同一数値, ミス通知 }
        protected enum EMode { 非表示中, 進行表示中, 残像表示中 }
        protected const int nギターコンボのCOMBO文字の高さ = 32;
        protected const int nギターコンボのCOMBO文字の幅 = 90;
        protected const int nギターコンボの高さ = 115;
        protected const int nギターコンボの幅 = 90;
        protected const int nギターコンボの文字間隔 = -6;
        protected const int nドラムコンボのCOMBO文字の高さ = 60;
        protected const int nドラムコンボのCOMBO文字の幅 = 250;
        protected const int nドラムコンボの高さ = 160;
        protected const int nドラムコンボの幅 = 120;
        protected const int nドラムコンボの文字間隔 = -6;
        protected int[] nジャンプ差分値 = new int[180];
        protected CSTATUS status;
        protected CTexture txCOMBOギター;
        protected CTexture txCOMBOドラム;
        protected CTexture txCOMBOドラム1000;
        protected CTexture txComboBom;
        public float nUnitTime;
        public CCounter ctコンボ;
        public CCounter ctコンボアニメ;
        public CCounter ctComboAnimation_2P;
        public int nY1の位座標差分値 = 0;
        public int nY1の位座標差分値_2P = 0;

        [StructLayout(LayoutKind.Sequential)]
        public struct ST爆発
        {
            public bool b使用中;
            public CCounter ct進行;
        }
        public ST爆発[] st爆発 = new ST爆発[2];
        public bool[] b爆発した = new bool[256];    //たぶん256個あったら十分かな。
        public int n火薬カウント;   //なんとなく火薬(笑)

        public STDGBVALUE<bool>[] bn00コンボに到達した = new STDGBVALUE<bool>[256];
        public STDGBVALUE<int> nコンボカウント = new STDGBVALUE<int>();


        // 内部クラス

        protected class CSTATUS
        {
            public CSTAT Bass = new CSTAT();
            public CSTAT Drums = new CSTAT();
            public CSTAT Guitar = new CSTAT();
            public CSTAT this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return this.Drums;

                        case 1:
                            return this.Guitar;

                        case 2:
                            return this.Bass;
                    }
                    throw new IndexOutOfRangeException();
                }
                set
                {
                    switch (index)
                    {
                        case 0:
                            this.Drums = value;
                            return;

                        case 1:
                            this.Guitar = value;
                            return;

                        case 2:
                            this.Bass = value;
                            return;
                    }
                    throw new IndexOutOfRangeException();
                }
            }

            public class CSTAT
            {
                public CActPerfCommonCombo.EMode e現在のモード;
                public int nCOMBO値;
                public long nコンボが切れた時刻;
                public int nジャンプインデックス値;
                public int n現在表示中のCOMBO値;
                public int n最高COMBO値;
                public int n残像表示中のCOMBO値;
                public long n前回の時刻_ジャンプ用;
            }
        }


        // コンストラクタ

        public CActPerfCommonCombo()
        {
            this.bNotActivated = true;

            // 180度分のジャンプY座標差分を取得。(0度: 0 → 90度:-15 → 180度: 0)
            for (int i = 0; i < 180; i++)
                this.nジャンプ差分値[i] = (int)(-15.0 * Math.Sin((Math.PI * i) / 180.0));

        }

        public void tコンボリセット処理()
        {
            for (int i = 0; i < 256; i++)
            {
                this.b爆発した[i] = false;
                this.bn00コンボに到達した[i].Drums = false;
            }
        }


        // メソッド

        protected virtual void tDrawCombo_Drums(int nCombo値, int nジャンプインデックス)
        {
        }
        protected virtual void tDrawCombo_Drums(int nCombo値, int nジャンプインデックス, int nX中央位置px, int nY上辺位置px)
        {

            #region [ 事前チェック。]
            //-----------------
            if (CDTXMania.ConfigIni.ドラムコンボ文字の表示位置 == EDrumComboTextDisplayPosition.OFF)
                return;		// 表示OFF。

            if (nCombo値 == 0)
                return;		// コンボゼロは表示しない。
            //-----------------
            #endregion

            int[] n位の数 = new int[10];	// 表示は10桁もあれば足りるだろう

            #region [ nCombo値を桁数ごとに n位の数[] に格納する。（例：nCombo値=125 のとき n位の数 = { 5,2,1,0,0,0,0,0,0,0 } ） ]
            //-----------------
            int n = nCombo値;
            int n桁数 = 0;
            while ((n > 0) && (n桁数 < 10))
            {
                n位の数[n桁数] = n % 10;		// 1の位を格納
                n = (n - (n % 10)) / 10;	// 右へシフト（例: 12345 → 1234 ）
                n桁数++;
            }
            //-----------------
            #endregion

            int n全桁の合計幅 = nドラムコンボの幅 * n桁数;

            #region [ n位の数[] を、"COMBO" → 1の位 → 10の位 … の順に、右から左へ向かって順番に表示する。]
            //-----------------
            const int n1桁ごとのジャンプの遅れ = 10;	// 1桁につき 50 インデックス遅れる

            int n数字とCOMBOを合わせた画像の全長px = ((nドラムコンボの幅) * n桁数);
            int x = nX中央位置px;
            int y = (nY上辺位置px + nドラムコンボの高さ) - nドラムコンボのCOMBO文字の高さ;
            int y2 = (nY上辺位置px) - nドラムコンボのCOMBO文字の高さ;
            int nJump = nジャンプインデックス - (n桁数);
            int y動作差分 = 0;
            //this.ctコンボ.tUpdateLoop();

            if ((nJump >= 0) && (nJump < 180))
            {
                y += this.nジャンプ差分値[nJump];
            }

            if ((int)(CDTXMania.stagePerfDrumsScreen.ctComboTimer.db現在の値 / 4) != 0)
            {
                y動作差分 = 2;
            }
            else if ((int)(CDTXMania.stagePerfDrumsScreen.ctComboTimer.db現在の値 / 16) != 1)
            {
                y動作差分 = 8;
            }

            // "COMBO" を表示。


            if (this.txCOMBOドラム != null)
                {
                    this.nコンボカウント.Drums = this.nCurrentCombo.Drums / 100;

                    #region [ "COMBO" の拡大率を設定。]
                    //-----------------
                    float f拡大率 = 1.0f;

                    if ((this.nCurrentCombo.Drums > (this.nCurrentCombo.Drums / 100) + 100) && this.bn00コンボに到達した[nコンボカウント.Drums].Drums == false && (nジャンプインデックス >= 0 && nジャンプインデックス < 180))
                    {
                        f拡大率 = 1.22f - (((float)this.nジャンプ差分値[nジャンプインデックス]) / 180.0f);		// f拡大率 = 1.0 → 1.3333... → 1.0
                    }
                    this.txCOMBOドラム.vcScaleRatio = new Vector3(f拡大率, f拡大率, 1.0f);
                    //-----------------
                    #endregion
                    #region [ "COMBO" 文字を表示。]
                    //-----------------
                    int nコンボx = nX中央位置px - 68 - ((int)((nドラムコンボのCOMBO文字の幅 * f拡大率) / 1.3f));
                    int nコンボy = 162 + nY上辺位置px;

                    if ((this.nCurrentCombo.Drums > (this.nCurrentCombo.Drums / 100 * 100) && (this.nCurrentCombo.Drums >= 100 ? this.bn00コンボに到達した[nコンボカウント.Drums].Drums == false : false)))
                    {
                        //nコンボx += n表示中央X - ((int)(( nドラムコンボのCOMBO文字の幅 * f拡大率 ) / 1.3f));
                        nコンボy += 10;
                    }
                    //-----------------
                    #endregion

                    this.txCOMBOドラム.tDraw2D(CDTXMania.app.Device, nコンボx, nコンボy + y動作差分, new Rectangle(0, 320, 250, 60));
                }

            // COMBO値を1の位から順に表示。
            // 基準位置を固定にし、1ケタ描画したらxを左につめる。

            for (int i = 0; i < n桁数; i++)
            {
                if( n桁数 < 4 )
                {
                    if ((this.nCurrentCombo.Drums > (this.nCurrentCombo.Drums / 100 * 100) && (this.nCurrentCombo.Drums >= 100 ? this.bn00コンボに到達した[nコンボカウント.Drums].Drums == false : false) && (nジャンプインデックス >= 0 && nジャンプインデックス < 180)))
                    {
                        x -= nドラムコンボの幅 + nドラムコンボの文字間隔 + 20;
                    }
                    else
                    {
                        x -= nドラムコンボの幅 + nドラムコンボの文字間隔;
                        this.txCOMBOドラム.vcScaleRatio = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                }
                else if( n桁数 >= 4 )
                {
                    if ((this.nCurrentCombo.Drums > (this.nCurrentCombo.Drums / 100 * 100) && (this.nCurrentCombo.Drums >= 100 ? this.bn00コンボに到達した[nコンボカウント.Drums].Drums == false : false) && (nジャンプインデックス >= 0 && nジャンプインデックス < 180)))
                    {
                        x -= 96 + nドラムコンボの文字間隔 + 20;
                    }
                    else
                    {
                        x -= 96 + nドラムコンボの文字間隔;
                        this.txCOMBOドラム.vcScaleRatio = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                }


                y = nY上辺位置px;

                nJump = nジャンプインデックス - (((n桁数 - i) - 1));
                if ((nJump >= 0) && (nJump < 180))
                {
                    y += this.nジャンプ差分値[nJump];
                }

                if( n桁数 < 4 )
                {
                    if( this.txCOMBOドラム != null )
                    {
                        this.txCOMBOドラム.tDraw2D(CDTXMania.app.Device, x, y + y動作差分,
                            new Rectangle((n位の数[i] % 5) * nドラムコンボの幅, (n位の数[i] / 5) * nドラムコンボの高さ, nドラムコンボの幅, nドラムコンボの高さ));
                    }
                }
                else if( n桁数 >= 4 )
                {
                    y = nY上辺位置px + 20;
                    if ((nJump >= 0) && (nJump < 180))
                    {
                        y += this.nジャンプ差分値[nJump];
                    }
                    if( this.txCOMBOドラム1000 != null )
                    {
                        this.txCOMBOドラム1000.tDraw2D(CDTXMania.app.Device, x, y + y動作差分,
                            new Rectangle((n位の数[i] % 5) * 96, (n位の数[i] / 5) * 128, 96, 128));
                    }
                }
            }


            //-----------------
            #endregion
        }
        protected virtual void tDrawCombo_Guitar(int nCombo値, int nジャンプインデックス)
        {
        }
        protected virtual void tDrawCombo_Bass(int nCombo値, int nジャンプインデックス)
        {
        }
        protected void tDrawCombo_Guitar(int nCombo値, int nジャンプインデックス, int nコンボx, int nコンボy)
        {
            #region [ 事前チェック。]
            //-----------------
            if ( CDTXMania.ConfigIni.n表示可能な最小コンボ数.Guitar == 0 )
                return;		// 表示OFF。

            if (nCombo値 == 0)
                return;		// コンボゼロは表示しない。
            //-----------------
            #endregion

            int[] n位の数 = new int[10];	// 表示は10桁もあれば足りるだろう

            #region [ nCombo値を桁数ごとに n位の数[] に格納する。（例：nCombo値=125 のとき n位の数 = { 5,2,1,0,0,0,0,0,0,0 } ） ]
            //-----------------
            int n = nCombo値;
            int n桁数 = 0;
            while ((n > 0) && (n桁数 < 10))
            {
                n位の数[n桁数] = n % 10;		// 1の位を格納
                n = (n - (n % 10)) / 10;	// 右へシフト（例: 12345 → 1234 ）
                n桁数++;
            }
            //-----------------
            #endregion

            int y = nコンボy;
            int n全桁の合計幅 = nギターコンボの幅 * n桁数;
            int nJump = nジャンプインデックス - (n桁数);
            int y動作差分 = 0;

            this.ctコンボアニメ.tUpdate();
            this.ctコンボアニメ.tUpdateDb();
            //CDTXMania.act文字コンソール.tPrint(1200, 0, CCharacterConsole.Eフォント種別.白, this.ctコンボアニメ.nCurrentValue.ToString());
            //CDTXMania.act文字コンソール.tPrint(1200, 16, CCharacterConsole.Eフォント種別.白, this.ctコンボアニメ.db現在の値.ToString());
            //CDTXMania.act文字コンソール.tPrint(1200, 32, CCharacterConsole.Eフォント種別.白, this.ctコンボアニメ.b進行中.ToString());
            //CDTXMania.act文字コンソール.tPrint(1200, 48, CCharacterConsole.Eフォント種別.白, this.ctコンボアニメ.n終了値.ToString());
            if( this.nY1の位座標差分値 > 0 )
            {
                //this.nY1の位座標差分値 -= ( CDTXMania.ConfigIni.bVerticalSyncWait ? 16 : 4);
                this.nY1の位座標差分値 = this.nY1の位座標差分値 - ( CDTXMania.ConfigIni.bVerticalSyncWait ? (int)this.ctコンボアニメ.db現在の値 : this.ctコンボアニメ.nCurrentValue);
            }
            else
            {
                this.nY1の位座標差分値 = 0;
            }

            if ((nJump >= 0) && (nJump < 180))
            {
                y += this.nジャンプ差分値[nJump];
            }

            #region [ "COMBO" の拡大率を設定。]
            //-----------------
            float f拡大率 = 1.0f;
            if (nジャンプインデックス >= 0 && nジャンプインデックス < 180)
                f拡大率 = 1.0f - (((float)this.nジャンプ差分値[nジャンプインデックス]) / 45.0f);		// f拡大率 = 1.0 → 1.3333... → 1.0

            if (this.txCOMBOギター != null)
                this.txCOMBOギター.vcScaleRatio = new Vector3(1.0f, 1.0f, 0.5f);

            //-----------------
            #endregion
            #region [ "COMBO" 文字を表示。]
            //-----------------
            int x = nコンボx - 100; // -((int)((nギターコンボのCOMBO文字の幅 * f拡大率) / 2.0f));

                this.txCOMBOギター.tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(0, 230, 200, 64));
            //-----------------
            #endregion

            x = nコンボx + (n全桁の合計幅 / 2);
            for (int i = 0; i < n桁数; i++)
            {
                #region [ 数字の拡大率を設定。]
                //-----------------
                if (this.txCOMBOドラム != null)
                    this.txCOMBOドラム.vcScaleRatio = new Vector3(1.0f, 1.0f, 1.0f);
                //-----------------
                #endregion
                #region [ 数字を1桁表示。]
                //-----------------
                x -= nギターコンボの幅 + nギターコンボの文字間隔;
                y = nコンボy - nギターコンボの高さ;

                nJump = nジャンプインデックス - (((n桁数 - i) - 1));
                if ((nJump >= 0) && (nJump < 180))
                {
                    y += this.nジャンプ差分値[nJump];
                }
                if (this.txCOMBOギター != null)
                {
                    this.txCOMBOギター.tDraw2D(
                        CDTXMania.app.Device,
                        x - ((int)((nギターコンボの幅) / 2.0f)),
                        y,
                        new Rectangle((n位の数[i] % 5) * nギターコンボの幅, (n位の数[i] / 5) * nギターコンボの高さ, nギターコンボの幅, nギターコンボの高さ));
                }
                //-----------------
                #endregion
            }
        }
        protected void tDrawCombo_Bass(int nCombo値, int nジャンプインデックス, int nコンボx, int nコンボy)
        {
            #region [ 事前チェック。]
            //-----------------
            if ( CDTXMania.ConfigIni.n表示可能な最小コンボ数.Bass == 0 )
                return;		// 表示OFF。

            if (nCombo値 == 0)
                return;		// コンボゼロは表示しない。
            //-----------------
            #endregion

            int[] n位の数 = new int[10];	// 表示は10桁もあれば足りるだろう

            #region [ nCombo値を桁数ごとに n位の数[] に格納する。（例：nCombo値=125 のとき n位の数 = { 5,2,1,0,0,0,0,0,0,0 } ） ]
            //-----------------
            int n = nCombo値;
            int n桁数 = 0;
            while ((n > 0) && (n桁数 < 10))
            {
                n位の数[n桁数] = n % 10;		// 1の位を格納
                n = (n - (n % 10)) / 10;	// 右へシフト（例: 12345 → 1234 ）
                n桁数++;
            }
            //-----------------
            #endregion

            int y = nコンボy;
            int n全桁の合計幅 = nギターコンボの幅 * n桁数;
            int nJump = nジャンプインデックス - (n桁数);
            int y動作差分 = 0;

            this.ctComboAnimation_2P.tUpdate();
            this.ctComboAnimation_2P.tUpdateDb();
            //CDTXMania.act文字コンソール.tPrint(1200, 0, CCharacterConsole.Eフォント種別.白, this.ctコンボアニメ.nCurrentValue.ToString());
            //CDTXMania.act文字コンソール.tPrint(1200, 16, CCharacterConsole.Eフォント種別.白, this.ctコンボアニメ.db現在の値.ToString());
            //CDTXMania.act文字コンソール.tPrint(1200, 32, CCharacterConsole.Eフォント種別.白, this.ctコンボアニメ.b進行中.ToString());
            //CDTXMania.act文字コンソール.tPrint(1200, 48, CCharacterConsole.Eフォント種別.白, this.ctコンボアニメ.n終了値.ToString());
            if( this.nY1の位座標差分値_2P > 0 )
            {
                //this.nY1の位座標差分値 -= ( CDTXMania.ConfigIni.bVerticalSyncWait ? 16 : 4);
                this.nY1の位座標差分値_2P = this.nY1の位座標差分値_2P - ( CDTXMania.ConfigIni.bVerticalSyncWait ? (int)this.ctComboAnimation_2P.db現在の値 : this.ctComboAnimation_2P.nCurrentValue);
            }
            else
            {
                this.nY1の位座標差分値_2P = 0;
            }

            if ((nJump >= 0) && (nJump < 180))
            {
                y += this.nジャンプ差分値[nJump];
            }

            #region [ "COMBO" の拡大率を設定。]
            //-----------------
            float f拡大率 = 1.0f;
            if (nジャンプインデックス >= 0 && nジャンプインデックス < 180)
                f拡大率 = 1.0f - (((float)this.nジャンプ差分値[nジャンプインデックス]) / 45.0f);		// f拡大率 = 1.0 → 1.3333... → 1.0

            if (this.txCOMBOギター != null)
                this.txCOMBOギター.vcScaleRatio = new Vector3(1.0f, 1.0f, 1.0f);
            //-----------------
            #endregion
            #region [ "COMBO" 文字を表示。]
            //-----------------
            int x = nコンボx - 95; // -((int)((nギターコンボのCOMBO文字の幅 * f拡大率) / 2.0f));

                this.txCOMBOギター.tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(0, 230, 200, 64));
            //-----------------
            #endregion

            x = nコンボx + (n全桁の合計幅 / 2);
            for (int i = 0; i < n桁数; i++)
            {
                #region [ 数字の拡大率を設定。]
                //-----------------
                f拡大率 = 1.0f;
                if (nジャンプインデックス >= 0 && nジャンプインデックス < 180)
                    f拡大率 = 1.0f - (((float)this.nジャンプ差分値[nジャンプインデックス]) / 45f);		// f拡大率 = 1.0 → 1.3333... → 1.0

                if (this.txCOMBOギター != null)
                    this.txCOMBOギター.vcScaleRatio = new Vector3(1.0f, 1.0f, 1.0f);
                //-----------------
                #endregion
                #region [ 数字を1桁表示。]
                //-----------------
                x -= nギターコンボの幅 + nギターコンボの文字間隔;
                y = nコンボy - nギターコンボの高さ;

                nJump = nジャンプインデックス - (((n桁数 - i) - 1));
                if ((nJump >= 0) && (nJump < 180))
                {
                    y += this.nジャンプ差分値[nJump];
                }
                if (this.txCOMBOギター != null)
                {
                    this.txCOMBOギター.tDraw2D(
                        CDTXMania.app.Device,
                        x - ((int)((nギターコンボの幅) / 2.0f)),
                        y,
                        new Rectangle((n位の数[i] % 5) * nギターコンボの幅, (n位の数[i] / 5) * nギターコンボの高さ, nギターコンボの幅, nギターコンボの高さ));
                }
                //-----------------
                #endregion
            }
        }


        // CActivity 実装

        public override void OnActivate()
        {
            this.nCurrentCombo = new STCOMBO() { act = this };
            this.status = new CSTATUS();
            for (int i = 0; i < 3; i++)
            {
                this.status[i].e現在のモード = EMode.非表示中;
                this.status[i].nCOMBO値 = 0;
                this.status[i].n最高COMBO値 = 0;
                this.status[i].n現在表示中のCOMBO値 = 0;
                this.status[i].n残像表示中のCOMBO値 = 0;
                this.status[i].nジャンプインデックス値 = 99999;
                this.status[i].n前回の時刻_ジャンプ用 = -1;
                this.status[i].nコンボが切れた時刻 = -1;
            }
            this.nUnitTime = (float)((60 / CDTXMania.DTX.BPM) / 4) * 10;
            this.ctコンボ = new CCounter(0, 1, (int)this.nUnitTime, CDTXMania.Timer);

            this.ctコンボアニメ = new CCounter( 0, 130, 4, CDTXMania.Timer );
            this.ctComboAnimation_2P = new CCounter( 0, 130, 4, CDTXMania.Timer );
            if(CDTXMania.ConfigIni.bVerticalSyncWait)
            {
                this.ctコンボアニメ = new CCounter( 0.0, 130.0, 0.003, CSoundManager.rcPerformanceTimer );
                this.ctComboAnimation_2P = new CCounter( 0.0, 130.0, 0.003, CSoundManager.rcPerformanceTimer );
            }

            base.OnActivate();
        }
        public override void OnDeactivate()
        {
            if (this.status != null)
                this.status = null;

            base.OnDeactivate();
        }
        public override void OnManagedCreateResources()
        {
            if( this.bNotActivated )
                return;

            this.txCOMBOドラム = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlayDrums combo.png" ) );
            this.txCOMBOドラム1000 = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlayDrums combo_2.png" ) );
            this.txComboBom = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\7_Combobomb.png" ) );
            if( this.txComboBom != null )
                this.txComboBom.bAdditiveBlending = true;
            this.txCOMBOギター = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlayGuitar combo.png" ) );
            base.OnManagedCreateResources();
        }
        public override void OnManagedReleaseResources()
        {
            if( this.bNotActivated )
                return;

            CDTXMania.tReleaseTexture( ref this.txCOMBOドラム );
            CDTXMania.tReleaseTexture( ref this.txCOMBOドラム1000 );
            CDTXMania.tReleaseTexture( ref this.txCOMBOギター );
            CDTXMania.tReleaseTexture( ref this.txComboBom );

            base.OnManagedReleaseResources();
        }
        public override int OnUpdateAndDraw()
        {
            if (this.bNotActivated)
                return 0;

            for (int i = 2; i >= 0; i--)
            {
                EEvent e今回の状態遷移イベント;

                #region [ 前回と今回の COMBO 値から、e今回の状態遷移イベントを決定する。]
                //-----------------
                if (this.status[i].n現在表示中のCOMBO値 == this.status[i].nCOMBO値)
                {
                    e今回の状態遷移イベント = EEvent.同一数値;
                }
                else if (this.status[i].n現在表示中のCOMBO値 > this.status[i].nCOMBO値)
                {
                    e今回の状態遷移イベント = EEvent.ミス通知;
                }
                else if ((this.status[i].n現在表示中のCOMBO値 < CDTXMania.ConfigIni.n表示可能な最小コンボ数[i]) && (this.status[i].nCOMBO値 < CDTXMania.ConfigIni.n表示可能な最小コンボ数[i]))
                {
                    e今回の状態遷移イベント = EEvent.非表示;
                }
                else
                {
                    e今回の状態遷移イベント = EEvent.数値更新;
                }
                //-----------------
                #endregion

                #region [ nジャンプインデックス値 の進行。]
                //-----------------
                if (this.status[i].nジャンプインデックス値 < 360)
                {
                    if ((this.status[i].n前回の時刻_ジャンプ用 == -1) || (CDTXMania.Timer.nCurrentTime < this.status[i].n前回の時刻_ジャンプ用))
                        this.status[i].n前回の時刻_ジャンプ用 = CDTXMania.Timer.nCurrentTime;

                    const long INTERVAL = 2;
                    while ((CDTXMania.Timer.nCurrentTime - this.status[i].n前回の時刻_ジャンプ用) >= INTERVAL)
                    {
                        if (this.status[i].nジャンプインデックス値 < 2000)
                            this.status[i].nジャンプインデックス値 += 3;

                        this.status[i].n前回の時刻_ジャンプ用 += INTERVAL;
                    }
                }
            //-----------------
                #endregion


            Retry:	// モードが変化した場合はここからリトライする。

                switch (this.status[i].e現在のモード)
                {
                    case EMode.非表示中:
                        #region [ *** ]
                        //-----------------

                        if (e今回の状態遷移イベント == EEvent.数値更新)
                        {
                            // モード変更
                            this.status[i].e現在のモード = EMode.進行表示中;
                            this.status[i].nジャンプインデックス値 = 0;
                            this.status[i].n前回の時刻_ジャンプ用 = CDTXMania.Timer.nCurrentTime;
                            goto Retry;
                        }

                        this.status[i].n現在表示中のCOMBO値 = this.status[i].nCOMBO値;
                        break;
                    //-----------------
                        #endregion

                    case EMode.進行表示中:
                        #region [ *** ]
                        //-----------------

                        if ((e今回の状態遷移イベント == EEvent.非表示) || (e今回の状態遷移イベント == EEvent.ミス通知))
                        {
                            // モード変更
                            this.status[i].e現在のモード = EMode.残像表示中;
                            this.status[i].n残像表示中のCOMBO値 = this.status[i].n現在表示中のCOMBO値;
                            this.status[i].nコンボが切れた時刻 = CDTXMania.Timer.nCurrentTime;
                            goto Retry;
                        }

                        if (e今回の状態遷移イベント == EEvent.数値更新)
                        {
                            this.status[i].nジャンプインデックス値 = 0;
                            this.status[i].n前回の時刻_ジャンプ用 = CDTXMania.Timer.nCurrentTime;
                        }

                        this.status[i].n現在表示中のCOMBO値 = this.status[i].nCOMBO値;
                        switch (i)
                        {
                            case 0:
                                this.tDrawCombo_Drums(this.status[i].nCOMBO値, this.status[i].nジャンプインデックス値);
                                break;

                            case 1:
                                this.tDrawCombo_Guitar(this.status[i].nCOMBO値, this.status[i].nジャンプインデックス値);
                                break;

                            case 2:
                                this.tDrawCombo_Bass(this.status[i].nCOMBO値, this.status[i].nジャンプインデックス値);
                                break;
                        }
                        break;
                    //-----------------
                        #endregion

                    case EMode.残像表示中:
                        #region [ *** ]
                        //-----------------
                        if (e今回の状態遷移イベント == EEvent.数値更新)
                        {
                            // モード変更１
                            this.status[i].e現在のモード = EMode.進行表示中;
                            goto Retry;
                        }
                        if ((CDTXMania.Timer.nCurrentTime - this.status[i].nコンボが切れた時刻) > 1000)
                        {
                            // モード変更２
                            this.status[i].e現在のモード = EMode.非表示中;
                            goto Retry;
                        }
                        this.status[i].n現在表示中のCOMBO値 = this.status[i].nCOMBO値;
                        break;
                    //-----------------
                        #endregion
                }
            }

            return 0;
        }

        public void tComboAnime( EInstrumentPart ePart )
        {
            if( ePart == EInstrumentPart.DRUMS || ePart == EInstrumentPart.GUITAR )
            {
                this.ctコンボアニメ.nCurrentValue = 0;
                this.ctコンボアニメ.db現在の値 = 0;
                this.nY1の位座標差分値 = 130;
            }
            else
            {
                this.ctComboAnimation_2P.nCurrentValue = 0;
                this.ctComboAnimation_2P.db現在の値 = 0;
                this.nY1の位座標差分値_2P = 130;
            }
        }
    }
}
