using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using SlimDX;
using FDK;

namespace DTXMania
{
    internal class CActSelectステータスパネル : CActivity
    {
        // メソッド

        public CActSelectステータスパネル()
        {
            base.b活性化してない = true;
        }
        public void t選択曲が変更された()
        {
            C曲リストノード c曲リストノード = CDTXMania.stage選曲.r現在選択中の曲;
            Cスコア cスコア = CDTXMania.stage選曲.r現在選択中のスコア;
            if ((c曲リストノード != null) && (cスコア != null))
            {
                this.n現在選択中の曲の難易度 = CDTXMania.stage選曲.n現在選択中の曲の難易度;
                for (int i = 0; i < 3; i++)
                {
                    if (CDTXMania.ConfigIni.nSkillMode == 0)
                    {
                        this.n現在選択中の曲の最高ランク[i] = cスコア.譜面情報.最大ランク[i];
                    }
                    else if (CDTXMania.ConfigIni.nSkillMode == 1)
                    {
                        this.n現在選択中の曲の最高ランク[i] = DTXMania.CScoreIni.tランク値を計算して返す(0, cスコア.譜面情報.最大スキル[i]);
                    }

                    this.b現在選択中の曲がフルコンボ[i] = cスコア.譜面情報.フルコンボ[i];
                    this.db現在選択中の曲の最高スキル値[i] = cスコア.譜面情報.最大スキル[i];
                    this.db現在選択中の曲の曲別スキル[i] = cスコア.譜面情報.最大曲別スキル[i];
                    this.b現在選択中の曲の譜面[i] = cスコア.譜面情報.b譜面がある[i];
                    this.n現在選択中の曲のレベル[i] = cスコア.譜面情報.レベル[i];
                    this.n現在選択中の曲のレベル小数点[ i ] = cスコア.譜面情報.レベルDec[ i ];
                    for (int j = 0; j < 5; j++)
                    {
                        if (c曲リストノード.arスコア[j] != null)
                        {
                            this.n現在選択中の曲のレベル難易度毎DGB[j][i] = c曲リストノード.arスコア[j].譜面情報.レベル[i];
                            this.n現在選択中の曲のレベル小数点難易度毎DGB[j][i] = c曲リストノード.arスコア[j].譜面情報.レベルDec[i];
                            //this.n現在選択中の曲の最高ランク難易度毎[j][i] = c曲リストノード.arスコア[j].譜面情報.最大ランク[i];
                            if (CDTXMania.ConfigIni.nSkillMode == 0)
                            {
                                this.n現在選択中の曲の最高ランク難易度毎[j][i] = c曲リストノード.arスコア[j].譜面情報.最大ランク[i];
                            }
                            else if (CDTXMania.ConfigIni.nSkillMode == 1)
                            {
                                this.n現在選択中の曲の最高ランク難易度毎[j][i] = (DTXMania.CScoreIni.tランク値を計算して返す(0, c曲リストノード.arスコア[j].譜面情報.最大スキル[i]) == (int)DTXMania.CScoreIni.ERANK.S && DTXMania.CScoreIni.tランク値を計算して返す(0, c曲リストノード.arスコア[j].譜面情報.最大スキル[i]) >= 95 ? DTXMania.CScoreIni.tランク値を計算して返す(0, cスコア.譜面情報.最大スキル[i]) : c曲リストノード.arスコア[j].譜面情報.最大ランク[i]);
                            }

                            this.db現在選択中の曲の最高スキル値難易度毎[j][i] = c曲リストノード.arスコア[j].譜面情報.最大スキル[i];
                            this.b現在選択中の曲がフルコンボ難易度毎[j][i] = c曲リストノード.arスコア[j].譜面情報.フルコンボ[i];
                            this.b現在選択中の曲に譜面がある[j][i] = c曲リストノード.arスコア[j].譜面情報.b譜面がある[i];
                        }
                    }
                }
                for (int i = 0; i < 5; i++)
                {
                    if (c曲リストノード.arスコア[i] != null)
                    {
                        int nLevel = c曲リストノード.arスコア[i].譜面情報.レベル.Drums;
                        if( nLevel < 0 )
                        {
                            nLevel = 0;
                        }
                        if( nLevel > 999 )
                        {
                            nLevel = 999;
                        }
                        this.n選択中の曲のレベル難易度毎[i] = nLevel;

                        this.db現在選択中の曲の曲別スキル値難易度毎[i] = c曲リストノード.arスコア[i].譜面情報.最大曲別スキル.Drums;
                    }
                    else
                    {
                        this.n選択中の曲のレベル難易度毎[i] = 0;
                    }
                    this.str難易度ラベル[i] = c曲リストノード.ar難易度ラベル[i];

                }
                if (this.r直前の曲 != c曲リストノード)
                {
                    this.n難易度開始文字位置 = 0;
                }
                this.r直前の曲 = c曲リストノード;
            }
        }


        // CActivity 実装

        public override void On活性化()
        {

            this.n現在選択中の曲の難易度 = 0;
            for( int i = 0; i < 3; i++ )
            {
                this.n現在選択中の曲のレベル[ i ] = 0;
                this.n現在選択中の曲のレベル小数点[ i ] = 0;
                this.db現在選択中の曲の曲別スキル[ i ] = 0.0;
                this.n現在選択中の曲の最高ランク[ i ] = (int)CScoreIni.ERANK.UNKNOWN;
                this.b現在選択中の曲がフルコンボ[ i ] = false;
                this.db現在選択中の曲の最高スキル値[ i ] = 0.0;
                for( int j = 0; j < 5; j++ )
                {
                    this.n現在選択中の曲のレベル難易度毎DGB[ j ][ i ] = 0;
                    this.n現在選択中の曲のレベル小数点難易度毎DGB[ j ][ i ] = 0;
                    this.db現在選択中の曲の最高スキル値難易度毎[ j ][ i ] = 0.0;
                    this.n現在選択中の曲の最高ランク難易度毎[ j ][ i ] = (int)CScoreIni.ERANK.UNKNOWN;
                    this.b現在選択中の曲がフルコンボ難易度毎[ j ][ i ] = false;
                }
            }
            for( int j = 0; j < 5; j++ )
            {
                this.str難易度ラベル[ j ] = "";
                this.n選択中の曲のレベル難易度毎[ j ] = 0;

                this.db現在選択中の曲の曲別スキル値難易度毎[ j ] = 0.0;
            }
            this.n難易度開始文字位置 = 0;
            this.r直前の曲 = null;
            base.On活性化();
        }
        public override void On非活性化()
        {
            this.ct登場アニメ用 = null;
            this.ct難易度スクロール用 = null;
            this.ct難易度矢印用 = null;
            base.On非活性化();
        }
        public override void OnManagedリソースの作成()
        {
            if (!base.b活性化してない)
            {
                this.txパネル本体 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\5_status panel.png"));
                this.tx難易度パネル = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\5_difficulty panel.png"));
                this.tx難易度枠 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\5_difficulty frame.png"));
                this.txランク = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\5_skill icon.png"));
                this.tx達成率MAX = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\5_skill max.png"));
                this.tx難易度数字 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\5_level number.png"));
                this.tx達成率数字 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\5_skill number.png"));
                this.txBPM数字 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\5_bpm font.png"));
                this.txBPM画像 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\5_bpm icon.png"));
                base.OnManagedリソースの作成();
            }
        }
        public override void OnManagedリソースの解放()
        {
            if (!base.b活性化してない)
            {
                CDTXMania.tテクスチャの解放(ref this.txパネル本体);
                CDTXMania.tテクスチャの解放(ref this.tx難易度パネル);
                CDTXMania.tテクスチャの解放(ref this.tx難易度枠);
                CDTXMania.tテクスチャの解放(ref this.txランク);
                CDTXMania.tテクスチャの解放(ref this.tx達成率MAX);
                CDTXMania.tテクスチャの解放(ref this.tx難易度数字);
                CDTXMania.tテクスチャの解放(ref this.tx達成率数字);
                CDTXMania.tテクスチャの解放(ref this.txBPM数字);
                CDTXMania.tテクスチャの解放(ref this.txBPM画像);
                base.OnManagedリソースの解放();
            }
        }
        public override int On進行描画()
        {

            if (!base.b活性化してない)
            {
                #region [ 初めての進行描画 ]
                //-----------------
                if (base.b初めての進行描画)
                {
                    this.ct登場アニメ用 = new CCounter(0, 100, 5, CDTXMania.Timer);
                    this.ct難易度スクロール用 = new CCounter(0, 20, 1, CDTXMania.Timer);
                    this.ct難易度矢印用 = new CCounter(0, 5, 80, CDTXMania.Timer);
                    base.b初めての進行描画 = false;
                }
                //-----------------
                #endregion

                // 進行

                this.ct登場アニメ用.t進行();

                this.ct難易度スクロール用.t進行();
                if (this.ct難易度スクロール用.b終了値に達した)
                {
                    int num = this.n現在の難易度ラベルが完全表示されているかを調べてスクロール方向を返す();
                    if (num < 0)
                    {
                        this.n難易度開始文字位置--;
                    }
                    else if (num > 0)
                    {
                        this.n難易度開始文字位置++;
                    }
                    this.ct難易度スクロール用.n現在の値 = 0;
                }

                this.ct難易度矢印用.t進行Loop();

                // 描画

                Cスコア cスコア = CDTXMania.stage選曲.r現在選択中のスコア;

                #region [ 選択曲の BPM の描画 ]
                if (CDTXMania.stage選曲.r現在選択中の曲 != null && this.txBPM画像 != null)
                {

                    int nBPM位置X = 490;
                    int nBPM位置Y = 385;

                    if (this.txパネル本体 != null)
                    {
                        nBPM位置X = 90;
                        nBPM位置Y = 275;
                    }

                    string strBPM;
                    switch (CDTXMania.stage選曲.r現在選択中の曲.eノード種別)
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                            {
                                strBPM = cスコア.譜面情報.Bpm.ToString();
                                break;
                            }
                        default:
                            {
                                strBPM = "---";
                                break;
                            }
                    }

                    this.txBPM画像.t2D描画(CDTXMania.app.Device, nBPM位置X, nBPM位置Y);
                    this.tBPM表示(nBPM位置X + 17, nBPM位置Y + 25, string.Format("{0,3:###}", strBPM));
                    //CDTXMania.act文字コンソール.tPrint(50, 570, C文字コンソール.Eフォント種別.白, string.Format("BPM:{0:####0}", this.n現在選択中の曲のBPM));
                }
                #endregion

                //-----------------

                int[] nPart = { 0, CDTXMania.ConfigIni.bIsSwappedGuitarBass ? 2 : 1, CDTXMania.ConfigIni.bIsSwappedGuitarBass ? 1 : 2 };

                int nBaseX = 30;
                int nBaseY = 350;

                int n難易度文字X = 70;
                int n難易度文字Y = 75;

                if (this.txパネル本体 != null)
                {
                    n難易度文字X = nBaseX + 10;
                    n難易度文字Y = nBaseY + 2;
                }

                #region [ ステータスパネルの描画 ]
                //-----------------
                if (this.txパネル本体 != null)
                {
                    this.txパネル本体.t2D描画(CDTXMania.app.Device, nBaseX, nBaseY);

                    for (int j = 0; j < 3; j++)
                    {

                        int nPanelW = 187;
                        int nPanelH = 60;

                        if (this.tx難易度パネル != null)
                        {
                            nPanelW = this.tx難易度パネル.sz画像サイズ.Width / 3;
                            nPanelH = this.tx難易度パネル.sz画像サイズ.Height * 2 / 11;
                        }

                        int nPanelX = nBaseX + this.txパネル本体.sz画像サイズ.Width + (nPanelW * (nPart[j] - 3));
                        int nPanelY = nBaseY + this.txパネル本体.sz画像サイズ.Height - (nPanelH * 11 / 2) - 5;

                        int nRankW;

                        int flag = 0;
                        int n変数;
                        double db変数;

                        if (this.tx難易度パネル != null)
                            this.tx難易度パネル.t2D描画(CDTXMania.app.Device, nPanelX, nPanelY, new Rectangle(nPanelW * j, 0, nPanelW, this.tx難易度パネル.sz画像サイズ.Height));

                        int[] n難易度整数 = new int[5];
                        int[] n難易度小数 = new int[5];
                        for (int i = 0; i < 5; i++)
                        {
                            if (this.str難易度ラベル[i] != null || CDTXMania.stage選曲.r現在選択中の曲.eノード種別 == C曲リストノード.Eノード種別.RANDOM)
                            {

                                int nBoxX = nPanelX;
                                int nBoxY = ( 391 + ( ( 4 - i ) * 60 ) ) - 2;

                                if (this.n現在選択中の曲の難易度 == i && this.tx難易度枠 != null)
                                {
                                    if ((CDTXMania.ConfigIni.bDrums有効 && j == 0) || (CDTXMania.ConfigIni.bGuitar有効 && j != 0))
                                        this.tx難易度枠.t2D描画(CDTXMania.app.Device, nBoxX, nBoxY);
                                }

                                #region [ 選択曲の Lv の描画 ]
                                if ((cスコア != null) && (this.tx難易度数字 != null))
                                {

                                    if (n選択中の曲のレベル難易度毎[i] > 100)
                                    {
                                        n難易度整数[i] = (int)this.n現在選択中の曲のレベル難易度毎DGB[i][j] / 100;
                                        n難易度小数[i] = (n選択中の曲のレベル難易度毎[i] - (n難易度整数[i] * 100));
                                    }
                                    else
                                    {
                                        n難易度整数[i] = (int)this.n現在選択中の曲のレベル難易度毎DGB[i][j] / 10;
                                        n難易度小数[i] = (this.n現在選択中の曲のレベル難易度毎DGB[i][j] - (n難易度整数[i] * 10)) * 10;
                                        n難易度小数[i] += this.n現在選択中の曲のレベル小数点難易度毎DGB[i][j];
                                    }

                                    if (this.str難易度ラベル[i] != null && this.b現在選択中の曲に譜面がある[i][j])
                                    {
                                        this.t難易度表示(nBoxX + nPanelW - 77, nBoxY + nPanelH - 35, string.Format("{0,4:0.00}", ((double)n難易度整数[i]) + (((double)n難易度小数[i]) / 100)));
                                    }
                                    else if ((this.str難易度ラベル[i] != null && !this.b現在選択中の曲に譜面がある[i][j]) || CDTXMania.stage選曲.r現在選択中の曲.eノード種別 == C曲リストノード.Eノード種別.RANDOM)
                                    {
                                        this.t難易度表示(nBoxX + nPanelW - 77, nBoxY + nPanelH - 35, ("-.--"));
                                    }
                                }
                                #endregion
                                db変数 = this.db現在選択中の曲の最高スキル値難易度毎[i][j];

                                if (db変数 < 0)
                                    db変数 = 0;

                                if (db変数 > 100)
                                    db変数 = 100;

                                if (db変数 != 0.00)
                                {
                                    if (this.txランク != null)
                                    {
                                        nRankW = this.txランク.sz画像サイズ.Width / 9;

                                        #region [ 選択曲の FullCombo Excellent の 描画 ]
                                        if (this.db現在選択中の曲の最高スキル値難易度毎[i][j] == 100)
                                            this.txランク.t2D描画(CDTXMania.app.Device, nBoxX + 42, nBoxY + 5, new Rectangle(nRankW * 8, 0, nRankW, this.txランク.sz画像サイズ.Height));
                                        else if (this.b現在選択中の曲がフルコンボ難易度毎[i][j])
                                            this.txランク.t2D描画(CDTXMania.app.Device, nBoxX + 42, nBoxY + 5, new Rectangle(nRankW * 7, 0, nRankW, this.txランク.sz画像サイズ.Height));
                                        #endregion
                                        #region [ 選択曲の 最高ランクの描画 ]
                                        n変数 = this.n現在選択中の曲の最高ランク難易度毎[i][j];

                                        if (n変数 != 99)
                                        {
                                            if (n変数 < 0)
                                                n変数 = 0;

                                            if (n変数 > 6)
                                                n変数 = 6;

                                            this.txランク.t2D描画(CDTXMania.app.Device, nBoxX + 7, nBoxY + 5, new Rectangle(nRankW * n変数, 0, nRankW, this.txランク.sz画像サイズ.Height));
                                        }
                                        #endregion
                                    }
                                    #region [ 選択曲の 最高スキル値ゲージ＋数値の描画 ]
                                    if (this.tx達成率MAX != null && db変数 == 100)
                                        this.tx達成率MAX.t2D描画(CDTXMania.app.Device, nBoxX + nPanelW - 155, nBoxY + nPanelH - 27);
                                    else
                                        this.t達成率表示(nBoxX + nPanelW - 157, nBoxY + nPanelH - 27, string.Format("{0,6:##0.00}%", db変数));
                                    #endregion
                                }

                            }
                            else if (CDTXMania.stage選曲.r現在選択中の曲.eノード種別 == C曲リストノード.Eノード種別.SCORE)
                            {
                                flag = flag + 1;
                            }
                        }
                        if (flag == 5)
                        {
                            int nBoxX = nPanelX;
                            int nBoxY = nPanelY + (nPanelH / 2);

                            if (this.tx難易度枠 != null)
                            {
                                if ((CDTXMania.ConfigIni.bDrums有効 && j == 0) || (CDTXMania.ConfigIni.bGuitar有効 && j != 0))
                                    this.tx難易度枠.t2D描画(CDTXMania.app.Device, nBoxX, nBoxY);
                            }

                            #region [ 選択曲の Lv の描画 ]
                            if ((cスコア != null) && (this.tx難易度数字 != null))
                            {
                                n難易度整数[0] = (int)this.n現在選択中の曲のレベル[ j ] / 10;
                                n難易度小数[0] = (this.n現在選択中の曲のレベル[ j ] - ( n難易度整数[ 0 ] * 10 ) ) * 10;
                                n難易度小数[0] += this.n現在選択中の曲のレベル小数点[ j ];

                                if (this.b現在選択中の曲の譜面[j] && CDTXMania.stage選曲.r現在選択中の曲.eノード種別 == C曲リストノード.Eノード種別.SCORE)
                                {
                                    this.t難易度表示(nBoxX + nPanelW - 77, nBoxY + nPanelH - 35, string.Format("{0,4:0.00}", ((double)n難易度整数[ 0 ]) + (((double)n難易度小数[ 0 ]) / 100)));
                                }
                                else if (!this.b現在選択中の曲の譜面[j] && CDTXMania.stage選曲.r現在選択中の曲.eノード種別 == C曲リストノード.Eノード種別.SCORE)
                                {
                                    this.t難易度表示(nBoxX + nPanelW - 77, nBoxY + nPanelH - 35, ("-.--"));
                                }
                            }
                            #endregion
                            db変数 = this.db現在選択中の曲の最高スキル値[j];

                            if (db変数 < 0)
                                db変数 = 0;

                            if (db変数 > 100)
                                db変数 = 100;

                            if (db変数 != 0.00)
                            {
                                if (this.txランク != null)
                                {
                                    nRankW = this.txランク.sz画像サイズ.Width / 9;

                                    #region [ 選択曲の FullCombo Excellent の 描画 ]
                                    if (this.db現在選択中の曲の最高スキル値[j] == 100)
                                        this.txランク.t2D描画(CDTXMania.app.Device, nBoxX + 42, nBoxY + 5, new Rectangle(nRankW * 8, 0, nRankW, this.txランク.sz画像サイズ.Height));
                                    else if (this.b現在選択中の曲がフルコンボ[j])
                                        this.txランク.t2D描画(CDTXMania.app.Device, nBoxX + 42, nBoxY + 5, new Rectangle(nRankW * 7, 0, nRankW, this.txランク.sz画像サイズ.Height));
                                    #endregion
                                    #region [ 選択曲の 最高ランクの描画 ]
                                    n変数 = this.n現在選択中の曲の最高ランク[j];

                                    if (n変数 != 99)
                                    {
                                        if (n変数 < 0)
                                            n変数 = 0;

                                        if (n変数 > 6)
                                            n変数 = 6;

                                        this.txランク.t2D描画(CDTXMania.app.Device, nBoxX + 7, nBoxY + 5, new Rectangle(nRankW * n変数, 0, nRankW, this.txランク.sz画像サイズ.Height));
                                    }
                                    #endregion
                                }
                                #region [ 選択曲の 最高スキル値ゲージ＋数値の描画 ]
                                if (this.tx達成率MAX != null && this.db現在選択中の曲の最高スキル値[j] == 100.00)
                                    this.tx達成率MAX.t2D描画(CDTXMania.app.Device, nBoxX + nPanelW - 155, nBoxY + nPanelH - 27);
                                else
                                    this.t達成率表示(nBoxX + nPanelW - 157, nBoxY + nPanelH - 27, string.Format("{0,6:##0.00}%", db変数));
                                #endregion
                            }

                        }
                    }
                }
                #endregion
                #region [ 難易度文字列の描画 ]
                //-----------------
                for (int i = 0; i < 5; i++)
                {
                    CDTXMania.act文字コンソール.tPrint(n難易度文字X + (i * 110), n難易度文字Y, (this.n現在選択中の曲の難易度 == i) ? C文字コンソール.Eフォント種別.赤 : C文字コンソール.Eフォント種別.白, this.str難易度ラベル[i]);
                }
                #endregion
            }
            return 0;
        }


        // その他

        #region [ private ]
        //-----------------
        [StructLayout(LayoutKind.Sequential)]
        private struct ST数字
        {
            public char ch;
            public Rectangle rc;
            public ST数字(char ch, Rectangle rc)
            {
                this.ch = ch;
                this.rc = rc;
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct ST達成率数字
        {
            public char ch;
            public Rectangle rc;
            public ST達成率数字(char ch, Rectangle rc)
            {
                this.ch = ch;
                this.rc = rc;
            }
        }
        private struct ST難易度数字
        {
            public char ch;
            public Rectangle rc;
            public ST難易度数字(char ch, Rectangle rc)
            {
                this.ch = ch;
                this.rc = rc;
            }
        }

        private STDGBVALUE<bool> b現在選択中の曲がフルコンボ;
        private STDGBVALUE<bool> b現在選択中の曲の譜面;
        private STDGBVALUE<bool>[] b現在選択中の曲がフルコンボ難易度毎 = new STDGBVALUE<bool>[5];
        private STDGBVALUE<bool>[] b現在選択中の曲に譜面がある = new STDGBVALUE<bool>[5];
        private STDGBVALUE<int>[] n現在選択中の曲のレベル難易度毎DGB = new STDGBVALUE<int>[5];
        private STDGBVALUE<int>[] n現在選択中の曲のレベル小数点難易度毎DGB = new STDGBVALUE<int>[5];
        private CCounter ct登場アニメ用;
        private CCounter ct難易度スクロール用;
        private CCounter ct難易度矢印用;
        private STDGBVALUE<double> db現在選択中の曲の最高スキル値;
        private STDGBVALUE<double>[] db現在選択中の曲の最高スキル値難易度毎 = new STDGBVALUE<double>[5];
        private double[] db現在選択中の曲の曲別スキル値難易度毎 = new double[5];
        private STDGBVALUE<double> db現在選択中の曲の曲別スキル;
        private STDGBVALUE<int> n現在選択中の曲のレベル;
        private STDGBVALUE<int> n現在選択中の曲のレベル小数点;
        private int[] n選択中の曲のレベル難易度毎 = new int[5];
        private STDGBVALUE<int> n現在選択中の曲の最高ランク;
        private STDGBVALUE<int>[] n現在選択中の曲の最高ランク難易度毎 = new STDGBVALUE<int>[5];
        private int n現在選択中の曲の難易度;
        private int n難易度開始文字位置;
        private const int n難易度表示可能文字数 = 0x24;
        /*
        private readonly Rectangle[] rcランク = new Rectangle[]
        {
            new Rectangle(0 * 34, 0, 34, 50),
            new Rectangle(1 * 34, 0, 34, 50),
            new Rectangle(2 * 34, 0, 34, 50),
            new Rectangle(3 * 34, 0, 34, 50),
            new Rectangle(4 * 34, 0, 34, 50),
            new Rectangle(5 * 34, 0, 34, 50),
            new Rectangle(6 * 34, 0, 34, 50),
            new Rectangle(7 * 34, 0, 34, 50),
            new Rectangle(8 * 34, 0, 34, 50),
            new Rectangle(9 * 34, 0, 34, 50)
        };
         */
        private C曲リストノード r直前の曲;
        public string[] str難易度ラベル = new string[] { "", "", "", "", "" };
        private readonly ST数字[] st数字 = new ST数字[]
        {
            new ST数字('0', new Rectangle(0 * 12, 0, 12, 20)),
            new ST数字('1', new Rectangle(1 * 12, 0, 12, 20)),
            new ST数字('2', new Rectangle(2 * 12, 0, 12, 20)),
            new ST数字('3', new Rectangle(3 * 12, 0, 12, 20)),
            new ST数字('4', new Rectangle(4 * 12, 0, 12, 20)),
            new ST数字('5', new Rectangle(5 * 12, 0, 12, 20)),
            new ST数字('6', new Rectangle(6 * 12, 0, 12, 20)),
            new ST数字('7', new Rectangle(7 * 12, 0, 12, 20)),
            new ST数字('8', new Rectangle(8 * 12, 0, 12, 20)),
            new ST数字('9', new Rectangle(9 * 12, 0, 12, 20)),
            new ST数字('-', new Rectangle(10 * 12, 0, 12, 20)),
            new ST数字('p', new Rectangle(11 * 12, 0, 12, 20)),
        };
        private readonly ST難易度数字[] st難易度数字 = new ST難易度数字[]
        {
            new ST難易度数字('0', new Rectangle(0 * 20, 0, 20, 28)),
            new ST難易度数字('1', new Rectangle(1 * 20, 0, 20, 28)),
            new ST難易度数字('2', new Rectangle(2 * 20, 0, 20, 28)),
            new ST難易度数字('3', new Rectangle(3 * 20, 0, 20, 28)),
            new ST難易度数字('4', new Rectangle(4 * 20, 0, 20, 28)),
            new ST難易度数字('5', new Rectangle(5 * 20, 0, 20, 28)),
            new ST難易度数字('6', new Rectangle(6 * 20, 0, 20, 28)),
            new ST難易度数字('7', new Rectangle(7 * 20, 0, 20, 28)),
            new ST難易度数字('8', new Rectangle(8 * 20, 0, 20, 28)),
            new ST難易度数字('9', new Rectangle(9 * 20, 0, 20, 28)),
            new ST難易度数字('.', new Rectangle(10 * 20, 0, 10, 28)),
            new ST難易度数字('-', new Rectangle(11 * 20 - 10, 0, 20, 28)),
            new ST難易度数字('?', new Rectangle(12 * 20 - 10, 0, 20, 28))
        };
        private readonly ST達成率数字[] st達成率数字 = new ST達成率数字[]
        {
            new ST達成率数字('0', new Rectangle(0 * 12, 0, 12, 20)),
            new ST達成率数字('1', new Rectangle(1 * 12, 0, 12, 20)),
            new ST達成率数字('2', new Rectangle(2 * 12, 0, 12, 20)),
            new ST達成率数字('3', new Rectangle(3 * 12, 0, 12, 20)),
            new ST達成率数字('4', new Rectangle(4 * 12, 0, 12, 20)),
            new ST達成率数字('5', new Rectangle(5 * 12, 0, 12, 20)),
            new ST達成率数字('6', new Rectangle(6 * 12, 0, 12, 20)),
            new ST達成率数字('7', new Rectangle(7 * 12, 0, 12, 20)),
            new ST達成率数字('8', new Rectangle(8 * 12, 0, 12, 20)),
            new ST達成率数字('9', new Rectangle(9 * 12, 0, 12, 20)),
            new ST達成率数字('.', new Rectangle(10 * 12, 0, 6, 20)),
            new ST達成率数字('%', new Rectangle(11 * 12 - 6, 0, 12, 20))
        };
        private readonly Rectangle rcunused = new Rectangle(0, 0x21, 80, 15);
        public CTexture txパネル本体;
        private CTexture txランク;
        private CTexture tx達成率MAX;
        private CTexture tx難易度パネル;
        private CTexture tx難易度枠;
        private CTexture tx難易度数字;
        private CTexture tx達成率数字;
        private CTexture txBPM数字;
        private CTexture txBPM画像;
        private int n現在の難易度ラベルが完全表示されているかを調べてスクロール方向を返す()
        {
            int num = 0;
            int length = 0;
            for (int i = 0; i < 5; i++)
            {
                if ((this.str難易度ラベル[i] != null) && (this.str難易度ラベル[i].Length > 0))
                {
                    length = this.str難易度ラベル[i].Length;
                }
                if (this.n現在選択中の曲の難易度 == i)
                {
                    break;
                }
                if ((this.str難易度ラベル[i] != null) && (this.str難易度ラベル.Length > 0))
                {
                    num += length + 2;
                }
            }
            if (num >= (this.n難易度開始文字位置 + 0x24))
            {
                return 1;
            }
            if ((num + length) <= this.n難易度開始文字位置)
            {
                return -1;
            }
            if (((num + length) - 1) >= (this.n難易度開始文字位置 + 0x24))
            {
                return 1;
            }
            if (num < this.n難易度開始文字位置)
            {
                return -1;
            }
            return 0;
        }
        private void t達成率表示(int x, int y, string str)
        {
            for (int j = 0; j < str.Length; j++)
            {
                char c = str[j];
                for (int i = 0; i < this.st達成率数字.Length; i++)
                {
                    if (this.st達成率数字[i].ch == c)
                    {
                        Rectangle rectangle = new Rectangle(this.st達成率数字[i].rc.X, this.st達成率数字[i].rc.Y, 12, 20);
                        if (c == '.')
                        {
                            rectangle.Width -= 6;
                        }
                        if (this.tx達成率数字 != null)
                        {
                            this.tx達成率数字.t2D描画(CDTXMania.app.Device, x, y, rectangle);
                        }
                        break;
                    }
                }
                if (c == '.')
                {
                    x += 6;
                }
                else
                {
                    x += 12;
                }
            }
        }
        private void t難易度表示(int x, int y, string str)
        {
            for (int j = 0; j < str.Length; j++)
            {
                char c = str[j];
                for (int i = 0; i < this.st難易度数字.Length; i++)
                {
                    if (this.st難易度数字[i].ch == c)
                    {
                        Rectangle rectangle = new Rectangle(this.st難易度数字[i].rc.X, this.st難易度数字[i].rc.Y, 20, 28);
                        if (c == '.')
                        {
                            rectangle.Width -= 10;
                        }
                        if (this.tx難易度数字 != null)
                        {
                            this.tx難易度数字.t2D描画(CDTXMania.app.Device, x, y, rectangle);
                        }
                        break;
                    }
                }
                if (c == '.')
                {
                    x += 10;
                }
                else
                {
                    x += 20;
                }
            }
        }
        private void tBPM表示(int x, int y, string str)
        {
            for (int j = 0; j < str.Length; j++)
            {
                for (int i = 0; i < this.st数字.Length; i++)
                {
                    if (this.st数字[i].ch == str[j])
                    {
                        Rectangle rectangle = new Rectangle(this.st数字[i].rc.X, this.st数字[i].rc.Y, 12, 20);
                        if (this.txBPM数字 != null)
                        {
                            this.txBPM数字.t2D描画(CDTXMania.app.Device, x, y, rectangle);
                        }
                        break;
                    }
                }
                x += 12;
            }
        }
        //-----------------
        #endregion
    }
}
