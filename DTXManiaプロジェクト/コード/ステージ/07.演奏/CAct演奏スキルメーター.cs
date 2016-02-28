using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using SlimDX;
using FDK;

namespace DTXMania
{
    internal class CAct演奏スキルメーター : CActivity
    {


        // #24074 2011.01.23 ikanick グラフの描画
        // 実装内容
        // ・左を現在、右を目標
        // ・基準線(60,70,80,90,100%)を超えると線が黄色くなる（元は白）
        // ・目標を超えると現在が光る
        // ・オート時には描画しない
        // 要望・実装予定
        // ・グラフを波打たせるなどの視覚の向上→実装済
        // 修正等
        // ・画像がないと落ちる→修正済

        // プロパティ

        public double dbグラフ値現在_渡
        {
            get
            {
                return this.dbグラフ値現在;
            }
            set
            {
                this.dbグラフ値現在 = value;
            }
        }
        public double dbグラフ値目標_渡
        {
            get
            {
                return this.dbグラフ値目標;
            }
            set
            {
                this.dbグラフ値目標 = value;
            }
        }

        // コンストラクタ

        public CAct演奏スキルメーター()
        {
            ST文字位置[] st文字位置Array = new ST文字位置[11];
            ST文字位置 st文字位置 = new ST文字位置();
            st文字位置.ch = '0';
            st文字位置.pt = new Point(210, 0);
            st文字位置Array[0] = st文字位置;
            ST文字位置 st文字位置2 = new ST文字位置();
            st文字位置2.ch = '1';
            st文字位置2.pt = new Point(223, 0);
            st文字位置Array[1] = st文字位置2;
            ST文字位置 st文字位置3 = new ST文字位置();
            st文字位置3.ch = '2';
            st文字位置3.pt = new Point(235, 0);
            st文字位置Array[2] = st文字位置3;
            ST文字位置 st文字位置4 = new ST文字位置();
            st文字位置4.ch = '3';
            st文字位置4.pt = new Point(247, 0);
            st文字位置Array[3] = st文字位置4;
            ST文字位置 st文字位置5 = new ST文字位置();
            st文字位置5.ch = '4';
            st文字位置5.pt = new Point(259, 0);
            st文字位置Array[4] = st文字位置5;
            ST文字位置 st文字位置6 = new ST文字位置();
            st文字位置6.ch = '5';
            st文字位置6.pt = new Point(271, 0);
            st文字位置Array[5] = st文字位置6;
            ST文字位置 st文字位置7 = new ST文字位置();
            st文字位置7.ch = '6';
            st文字位置7.pt = new Point(283, 0);
            st文字位置Array[6] = st文字位置7;
            ST文字位置 st文字位置8 = new ST文字位置();
            st文字位置8.ch = '7';
            st文字位置8.pt = new Point(295, 0);
            st文字位置Array[7] = st文字位置8;
            ST文字位置 st文字位置9 = new ST文字位置();
            st文字位置9.ch = '8';
            st文字位置9.pt = new Point(307, 0);
            st文字位置Array[8] = st文字位置9;
            ST文字位置 st文字位置10 = new ST文字位置();
            st文字位置10.ch = '9';
            st文字位置10.pt = new Point(319, 0);
            st文字位置Array[9] = st文字位置10;
            ST文字位置 st文字位置11 = new ST文字位置();
            st文字位置11.ch = '.';
            st文字位置11.pt = new Point(331, 0);
            st文字位置Array[10] = st文字位置11;
            this.st小文字位置 = st文字位置Array;
            base.b活性化してない = true;
        }


        // CActivity 実装

        public override void On活性化()
        {
            this.n本体X[0] = 900;
            this.n本体X[1] = 574;
            this.n本体X[2] = 290;

            this.dbグラフ値目標 = 80f;
            this.dbグラフ値現在 = 0f;
            this.dbグラフ値比較 = 0f;
            this.db現在の判定数合計 = 0f;
            base.On活性化();
        }
        public override void On非活性化()
        {
            base.On非活性化();
        }
        public override void OnManagedリソースの作成()
        {
            if (!base.b活性化してない)
            {
                this.txグラフ = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Graph_main.png"));
                this.tx比較 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Graph_main.png"));
                this.txグラフバックパネル = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Graph_main.png"));
                this.tx数字 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Graph_main.png"));
                this.txComboBom = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Combobomb.png"));
                if (this.txComboBom != null)
                    this.txComboBom.b加算合成 = true;
                base.OnManagedリソースの作成();
            }
        }
        public override void OnManagedリソースの解放()
        {
            if (!base.b活性化してない)
            {
                CDTXMania.tテクスチャの解放(ref this.txグラフ);
                CDTXMania.tテクスチャの解放(ref this.tx比較);
                CDTXMania.tテクスチャの解放(ref this.txグラフバックパネル);
                CDTXMania.tテクスチャの解放(ref this.tx数字);
                CDTXMania.tテクスチャの解放(ref this.txComboBom);
                base.OnManagedリソースの解放();
            }
        }
        public override int On進行描画()
        {
            int j = 0;

            if (CDTXMania.ConfigIni.bGuitar有効)
            {
                if (!CDTXMania.DTX.bチップがある.Bass)
                    j = 1;
                else if (!CDTXMania.DTX.bチップがある.Guitar)
                    j = 2;
                else if (!CDTXMania.ConfigIni.bギターが全部オートプレイである && CDTXMania.ConfigIni.bベースが全部オートプレイである)
                    j = 1;
                else if (CDTXMania.ConfigIni.bギターが全部オートプレイである && !CDTXMania.ConfigIni.bベースが全部オートプレイである)
                    j = 2;
            }

            if (!base.b活性化してない)
            {
                if (base.b初めての進行描画)
                {
                    this.ct爆発エフェクト = new CCounter(0, 13, 20, CDTXMania.Timer);
                    base.b初めての進行描画 = false;
                }
                double db1ノーツごとの達成率 = (double)this.dbグラフ値目標 / CDTXMania.DTX.n可視チップ数[j];

                if (j == 0)
                    this.n現在演奏されたノーツ数 =
                        CDTXMania.stage演奏ドラム画面.nヒット数・Auto含む[j].Perfect +
                        CDTXMania.stage演奏ドラム画面.nヒット数・Auto含む[j].Great +
                        CDTXMania.stage演奏ドラム画面.nヒット数・Auto含む[j].Good +
                        CDTXMania.stage演奏ドラム画面.nヒット数・Auto含む[j].Poor +
                        CDTXMania.stage演奏ドラム画面.nヒット数・Auto含む[j].Miss;
                else
                    this.n現在演奏されたノーツ数 =
                        CDTXMania.stage演奏ギター画面.nヒット数・Auto含む[j].Perfect +
                        CDTXMania.stage演奏ギター画面.nヒット数・Auto含む[j].Great +
                        CDTXMania.stage演奏ギター画面.nヒット数・Auto含む[j].Good +
                        CDTXMania.stage演奏ギター画面.nヒット数・Auto含む[j].Poor +
                        CDTXMania.stage演奏ギター画面.nヒット数・Auto含む[j].Miss;

                CScoreIni.C演奏記録 drums = new CScoreIni.C演奏記録();

                double rate = (double)n現在演奏されたノーツ数 / (double)CDTXMania.DTX.n可視チップ数[j];

                if (CDTXMania.ConfigIni.nSkillMode == 0)
                {
                    //int n逆算Perfect = drums.nPerfect数・Auto含まない / this.n現在演奏されたノーツ数;
                    //int n逆算Great = drums.nGreat数・Auto含まない / this.n現在演奏されたノーツ数;
                    //this.dbグラフ値比較 = CScoreIni.t旧ゴーストスキルを計算して返す(CDTXMania.DTX.n可視チップ数[j], drums.nPerfect数, drums.nGreat数, drums.nGood数, drums.nPoor数, drums.nMiss数, E楽器パート[j]) * rate;
                    //this.dbグラフ値比較 = ((this.dbグラフ値目標_渡) / (double)CDTXMania.DTX.n可視チップ数[j]) * rate;
                    this.dbグラフ値比較 = this.dbグラフ値目標_渡;
                }
                else if (CDTXMania.ConfigIni.nSkillMode == 1)
                {
                    //this.dbグラフ値比較 = CScoreIni.tゴーストスキルを計算して返す(CDTXMania.DTX.n可視チップ数[j], this.n現在演奏されたノーツ数, drums.n最大コンボ数, E楽器パート[j]);
                    this.dbグラフ値比較 = (double)(db1ノーツごとの達成率 * n現在演奏されたノーツ数);
                }


                //this.dbグラフ値比較 = (double)(db1ノーツごとの達成率 * n現在演奏されたノーツ数);
                // 背景暗幕
                Rectangle rectangle = new Rectangle(900, 0, 380, 720);
                if (this.txグラフ != null)
                {
                    this.txグラフバックパネル.t2D描画(CDTXMania.app.Device, this.n本体X[j], 0, rectangle);
                    this.txグラフバックパネル.t2D描画(CDTXMania.app.Device, 141 + this.n本体X[j], 650 - (int)(this.dbグラフ値現在 * 5.56), new Rectangle(499, 0, 201, (int)(this.dbグラフ値現在 * 5.56)));
                }

                this.t小文字表示(270 + this.n本体X[j], 658, string.Format("{0,6:##0.00}%", this.dbグラフ値現在));
                if (CDTXMania.ConfigIni.nInfoType == 0)
                {
                    this.tx比較.t2D描画(CDTXMania.app.Device, 170 + this.n本体X[j], 200, new Rectangle(336, 0, 162, 60));
                    this.t小文字表示(250 + this.n本体X[j], 224, string.Format("{0,6:##0.00}%", this.dbグラフ値目標));
                    if (this.dbグラフ値現在 > this.dbグラフ値目標)
                    {
                        this.tx比較.n透明度 = 128;
                    }
                }
                else if (CDTXMania.ConfigIni.nInfoType == 1)
                {
                    this.tx比較.t2D描画(CDTXMania.app.Device, 170 + this.n本体X[j], 200, new Rectangle(336, 205, 162, 60));
                    this.tx比較.t2D描画(CDTXMania.app.Device, 170 + this.n本体X[j], 280, new Rectangle(336, 265, 162, 60));
                    this.tx比較.t2D描画(CDTXMania.app.Device, 170 + this.n本体X[j], 360, new Rectangle(336, 325, 162, 60));
                    this.tx比較.t2D描画(CDTXMania.app.Device, 170 + this.n本体X[j], 440, new Rectangle(336, 385, 162, 60));
                    this.tx比較.t2D描画(CDTXMania.app.Device, 170 + this.n本体X[j], 520, new Rectangle(336, 445, 162, 60));

                    if (j == 0)
                    {
                        this.t小文字表示(250 + this.n本体X[j], 224, string.Format("{0,6:###0}", CDTXMania.stage演奏ドラム画面.nヒット数・Auto含まない[j].Perfect));
                        this.t小文字表示(250 + this.n本体X[j], 304, string.Format("{0,6:###0}", CDTXMania.stage演奏ドラム画面.nヒット数・Auto含まない[j].Great));
                        this.t小文字表示(250 + this.n本体X[j], 384, string.Format("{0,6:###0}", CDTXMania.stage演奏ドラム画面.nヒット数・Auto含まない[j].Good));
                        this.t小文字表示(250 + this.n本体X[j], 464, string.Format("{0,6:###0}", CDTXMania.stage演奏ドラム画面.nヒット数・Auto含まない[j].Poor));
                        this.t小文字表示(250 + this.n本体X[j], 544, string.Format("{0,6:###0}", CDTXMania.stage演奏ドラム画面.nヒット数・Auto含まない[j].Miss));
                    }
                    else
                    {
                        this.t小文字表示(250 + this.n本体X[j], 224, string.Format("{0,6:###0}", CDTXMania.stage演奏ギター画面.nヒット数・Auto含まない[j].Perfect));
                        this.t小文字表示(250 + this.n本体X[j], 304, string.Format("{0,6:###0}", CDTXMania.stage演奏ギター画面.nヒット数・Auto含まない[j].Great));
                        this.t小文字表示(250 + this.n本体X[j], 384, string.Format("{0,6:###0}", CDTXMania.stage演奏ギター画面.nヒット数・Auto含まない[j].Good));
                        this.t小文字表示(250 + this.n本体X[j], 464, string.Format("{0,6:###0}", CDTXMania.stage演奏ギター画面.nヒット数・Auto含まない[j].Poor));
                        this.t小文字表示(250 + this.n本体X[j], 544, string.Format("{0,6:###0}", CDTXMania.stage演奏ギター画面.nヒット数・Auto含まない[j].Miss));
                    }
                }

                // 基準線
                rectangle = new Rectangle(78, 0, 60, 3);
                if (this.txグラフ != null)
                {
                    //this.txグラフ.n透明度 = 32;
                    //this.txグラフ.vc拡大縮小倍率 = new Vector3(58f, 1f, 1f);
                    for (int i = 0; i < 20; i++)
                    {
                        //this.txグラフ.t2D描画(CDTXMania.app.Device, 75 + this.n本体X[j], 94 + (int)(29.26 * i), rectangle);
                    }
                    //this.txグラフ.vc拡大縮小倍率 = new Vector3(1f, 230f, 1f);
                    for (int i = 0; i < 2; i++)
                    {
                        //this.txグラフ.t2D描画(CDTXMania.app.Device, 75 + this.n本体X[j] + (int)(29.26 * i), 94, rectangle);
                        //this.txグラフ.t2D描画(CDTXMania.app.Device, 75 + this.n本体X[j] + (int)(29.26 * i), 94, rectangle);
                    }
                }
                if (this.txグラフ != null)
                {
                    //this.txグラフ.vc拡大縮小倍率 = new Vector3(38f, 1f, 1f);
                }
                for (int i = 0; i < 5; i++)
                {
                    // 基準線を越えたら線が黄色くなる
                    if (this.dbグラフ値現在 >= (100 - i * 10))
                    {
                        rectangle = new Rectangle(78, 1, 60, 2);//黄色
                        if (this.txグラフ != null)
                        {
                            //this.txグラフ.n透明度 = 224;
                        }
                    }
                    else
                    {
                        rectangle = new Rectangle(78, 4, 60, 2);
                        if (this.txグラフ != null)
                        {
                            this.txグラフ.n透明度 = 160;
                        }
                    }

                    if (this.txグラフ != null)
                    {
                        this.txグラフ.t2D描画(CDTXMania.app.Device, 75 + this.n本体X[j], 94 + (int)(58.52 * i), rectangle);

                    }
                }
                // グラフ
                // --現在値
                if (this.dbグラフ値現在_表示 < this.dbグラフ値現在)
                {
                    this.dbグラフ値現在_表示 += (this.dbグラフ値現在 - this.dbグラフ値現在_表示) / 5 + 0.01;
                }
                if (this.dbグラフ値現在_表示 >= this.dbグラフ値現在)
                {
                    this.dbグラフ値現在_表示 = this.dbグラフ値現在;
                }
                rectangle = new Rectangle(0, 0, 72, (int)(556f * this.dbグラフ値現在_表示 / 100));
                if (this.txグラフ != null)
                {
                    this.txグラフ.vc拡大縮小倍率 = new Vector3(1f, 1f, 1f);
                    //this.txグラフ.n透明度 = 192;
                    this.txグラフ.t2D描画(CDTXMania.app.Device, 69 + this.n本体X[j], 650 - (int)(556f * this.dbグラフ値現在_表示 / 100), rectangle);
                }
                for (int k = 0; k < 32; k++)
                {
                    rectangle = new Rectangle(20, 0, 1, 1);
                    if (this.txグラフ != null)
                    {
                        //this.stキラキラ[ k ].ct進行.t進行Loop();
                        int num1 = (int)this.stキラキラ[k].x;
                        //int num2 = this.stキラキラ[ k ].ct進行.n現在の値;
                        //this.txグラフ.vc拡大縮小倍率 = new Vector3(this.stキラキラ[ k ].fScale, this.stキラキラ[ k ].fScale, this.stキラキラ[ k ].fScale);
                        //this.txグラフ.n透明度 = 138 - 2 * this.stキラキラ[ k ].Trans;
                        //if ( num2 < (2.3f * this.dbグラフ値現在_表示) )
                        {
                            //this.txグラフ.t2D描画(CDTXMania.app.Device, 860+num1, 318-num2, rectangle);
                        }
                    }
                }
                // --現在値_追加エフェクト

                if (this.dbグラフ値直前 != this.dbグラフ値現在)
                {
                    this.stフラッシュ[nグラフフラッシュct].y = 0;
                    this.stフラッシュ[nグラフフラッシュct].Trans = 650;
                    nグラフフラッシュct++;
                    if (nグラフフラッシュct >= 16)
                    {
                        nグラフフラッシュct = 0;
                    }
                }

                this.dbグラフ値直前 = this.dbグラフ値現在;
                for (int m = 0; m < 16; m++)
                {
                    rectangle = new Rectangle(6, 0, 60, 2);
                    if ((this.stフラッシュ[m].y >= 0) && (this.stフラッシュ[m].y + 3 < (int)(650f * this.dbグラフ値現在_表示 / 100)) && (this.txグラフ != null))
                    {
                        //this.txグラフ.n透明度 = this.stフラッシュ[ m ].Trans;
                        //this.txグラフ.t2D描画(CDTXMania.app.Device, 75 + this.n本体X[j], this.stフラッシュ[ m ].y + (620 - (int)(556f * this.dbグラフ値現在_表示 / 100)), rectangle);
                        //this.txグラフ.n透明度 = this.stフラッシュ[ m ].Trans;
                        //this.txグラフ.t2D描画(CDTXMania.app.Device, 75 + this.n本体X[j], this.stフラッシュ[ m ].y + 2 + (620 - (int)(556f * this.dbグラフ値現在_表示 / 100)), rectangle);
                    }
                    this.stフラッシュ[m].y += 5;
                    this.stフラッシュ[m].Trans -= 5;
                }
                // --現在値_目標越
                rectangle = new Rectangle(0, 0, 10, (int)(556f * this.dbグラフ値現在_表示 / 100));
                if ((dbグラフ値現在 >= dbグラフ値目標) && (this.txグラフ != null))
                {
                    //this.txグラフ.vc拡大縮小倍率 = new Vector3(1.4f, 1f, 1f);
                    //this.txグラフ.n透明度 = 128;
                    //this.txグラフ.b加算合成 = true;
                    this.txグラフ.t2D描画(CDTXMania.app.Device, 69 + this.n本体X[j], 650 - (int)(556f * this.dbグラフ値現在_表示 / 100), rectangle);

                }
                // --目標値

                if (this.dbグラフ値目標_表示 < this.dbグラフ値目標)
                {
                    this.dbグラフ値目標_表示 += (this.dbグラフ値目標 - this.dbグラフ値目標_表示) / 5 + 0.01;
                }
                if (this.dbグラフ値目標_表示 >= this.dbグラフ値目標)
                {
                    this.dbグラフ値目標_表示 = this.dbグラフ値目標;
                }

                db現在の判定数合計 = 0;
                //db現在の判定数合計 = CDTXMania.stage演奏画面共通.nヒット数・Auto含む[j].Perfect + CDTXMania.stage演奏画面共通.nヒット数・Auto含む[j].Great + CDTXMania.stage演奏画面共通.nヒット数・Auto含む[j].Good + CDTXMania.stage演奏画面共通.nヒット数・Auto含む[j].Miss + CDTXMania.stage演奏画面共通.nヒット数・Auto含む[j].Poor;
                //this.dbグラフ値目標_Ghost = ((1.0 * CDTXMania.stage選曲.r確定されたスコア.譜面情報.最大スキル[0] / CDTXMania.DTX.n可視チップ数[j]) * db現在の判定数合計);
                //System.IO.StreamWriter sw = new System.IO.StreamWriter(@"debug.txt", true, System.Text.Encoding.GetEncoding("shift_jis"));
                //sw.WriteLine("TotalJudgeは{0}で、Ghostは{1}です。", db現在の判定数合計, this.dbグラフ値目標_Ghost);
                //sw.Close();
                this.dbグラフ値目標_表示 = this.dbグラフ値目標;
                rectangle = new Rectangle(138, 0, 72, (int)(556f * this.dbグラフ値目標_表示 / 100));
                if (this.txグラフ != null)
                {
                    //this.txグラフ.vc拡大縮小倍率 = new Vector3(1f, 1f, 1f);
                    //this.txグラフ.n透明度 = 192;
                    //this.txグラフ.t2D描画(CDTXMania.app.Device, 69 + this.n本体X[j], 650 - (int)(556f * this.dbグラフ値目標_表示 / 100), rectangle);
                    //this.txグラフ.vc拡大縮小倍率 = new Vector3(1.4f, 1f, 1f);
                    this.txグラフ.n透明度 = 48;
                    //this.txグラフ.b加算合成 = true;
                    //this.txグラフ.t2D描画(CDTXMania.app.Device, 69 + this.n本体X[j], 650 - (int)(556f * this.dbグラフ値目標_表示 / 100), rectangle);
                    this.txグラフ.t2D描画(CDTXMania.app.Device, 69 + this.n本体X[j], 650 - (int)(556f * this.dbグラフ値比較 / 100), new Rectangle(138, 0, 72, (int)(556f * this.dbグラフ値比較 / 100)));
                }
                /*
				for( int k = 32; k < 64; k++ )
				{
                    rectangle = new Rectangle(6, 0, 1, 1);
                    if (this.txグラフ != null)
                    {
				    	this.stキラキラ[ k ].ct進行.t進行Loop();
                        int num1 = (int)this.stキラキラ[ k ].x;
                        int num2 = this.stキラキラ[ k ].ct進行.n現在の値;
                        this.txグラフ.vc拡大縮小倍率 = new Vector3(this.stキラキラ[ k ].fScale, this.stキラキラ[ k ].fScale, this.stキラキラ[ k ].fScale);
                        //this.txグラフ.n透明度 = 138 - 2 * this.stキラキラ[ k ].Trans;
                        if ( num2 < (2.3f * this.dbグラフ値目標_表示) )
                        {
                            this.txグラフ.t2D描画(CDTXMania.app.Device, 75 +this.n本体X[j] + num1, 318 - num2, rectangle);
                        }
                    }
				}
                 */

            }
            return 0;
        }


        // その他

        #region [ private ]
        //----------------
        [StructLayout(LayoutKind.Sequential)]
        private struct STキラキラ
        {
            public int x;
            public int y;
            public float fScale;
            public int Trans;
            public CCounter ct進行;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct ST文字位置
        {
            public char ch;
            public Point pt;
        }
        private readonly ST文字位置[] st小文字位置;
        private STキラキラ[] stキラキラ = new STキラキラ[64];
        private STキラキラ[] stフラッシュ = new STキラキラ[16];

        private CCounter ct爆発エフェクト;
        public double db現在の判定数合計;
        private double dbグラフ値目標;
        public double dbグラフ値比較;
        private double dbグラフ値目標_表示;
        private double dbグラフ値現在;
        private double dbグラフ値現在_表示;
        private double dbグラフ値直前;
        private int nグラフフラッシュct;
        private int n現在演奏されたノーツ数;
        private STDGBVALUE<int> n本体X;
        private CTexture tx数字;
        private CTexture tx比較;
        private CTexture txグラフ;
        private CTexture txグラフバックパネル;
        protected CTexture txComboBom;
        //-----------------


        private void t小文字表示(int x, int y, string str)
        {
            foreach (char ch in str)
            {
                for (int i = 0; i < this.st小文字位置.Length; i++)
                {
                    if (this.st小文字位置[i].ch == ch)
                    {
                        Rectangle rectangle = new Rectangle(this.st小文字位置[i].pt.X, this.st小文字位置[i].pt.Y, 12, 16);
                        if (ch == '.')
                        {
                            rectangle.Width -= 8;
                        }
                        if (this.tx数字 != null)
                        {
                            this.tx数字.t2D描画(CDTXMania.app.Device, x, y, rectangle);
                        }
                        break;
                    }
                }
                x += 12;
            }
        }
        #endregion
    }
}
