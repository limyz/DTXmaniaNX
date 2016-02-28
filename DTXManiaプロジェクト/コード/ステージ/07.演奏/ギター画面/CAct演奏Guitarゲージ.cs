using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SlimDX;
using FDK;

namespace DTXMania
{
    internal class CAct演奏Guitarゲージ : CAct演奏ゲージ共通
    {

        // コンストラクタ

        public CAct演奏Guitarゲージ()
        {
            base.b活性化してない = true;
        }


        // CActivity 実装

        public override void On活性化()
        {
            base.n本体X.Guitar = 80;
            base.n本体X.Bass = 912 + 290;
            base.On活性化();
        }
        public override void On非活性化()
        {
            this.ct本体移動 = null;
            this.ct本体振動 = null;
            base.On非活性化();
        }
        public override void OnManagedリソースの作成()
        {
            if (!base.b活性化してない)
            {
                this.txフレーム.Guitar = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Gauge_Guitar.png"));
                this.txフレーム.Bass = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Gauge_Bass.png"));
                this.txフルゲージ = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_gauge_bar.jpg"));
                this.txゲージ = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_gauge_bar.png"));

                //this.txマスクF = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Dummy.png"));
                //this.txマスクD = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Dummy.png"));
                this.txハイスピ = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Panel_icons.jpg"));

                base.OnManagedリソースの作成();
            }
        }
        public override void OnManagedリソースの解放()
        {
            if (!base.b活性化してない)
            {
                CDTXMania.tテクスチャの解放(ref this.txゲージ);
                CDTXMania.tテクスチャの解放(ref this.txフルゲージ);
                CDTXMania.tテクスチャの解放(ref this.txフレーム.Guitar);
                CDTXMania.tテクスチャの解放(ref this.txフレーム.Bass);

                //CDTXMania.tテクスチャの解放(ref this.txマスクF);
                //CDTXMania.tテクスチャの解放(ref this.txマスクD);
                CDTXMania.tテクスチャの解放(ref this.txハイスピ);

                base.OnManagedリソースの解放();
            }
        }
        public override int On進行描画()
        {
            //int num9;
            if (base.b初めての進行描画)
            {
                this.ct本体移動 = new CCounter(0, 0x1a, 20, CDTXMania.Timer);
                this.ct本体振動 = new CCounter(0, 360, 4, CDTXMania.Timer);
                base.b初めての進行描画 = false;
            }
            this.ct本体移動.t進行Loop();
            this.ct本体振動.t進行Loop();

            base.txハイスピ.vc拡大縮小倍率 = new Vector3(0.76190476190476190476190476190476f, 0.66666666666666666666666666666667f, 1.0f);

            #region [ ギターのゲージ ]
            if (base.txフレーム.Guitar != null && CDTXMania.DTX.bチップがある.Guitar)
            {
                base.txフレーム.Guitar.t2D描画(CDTXMania.app.Device, base.n本体X.Guitar, 0, new Rectangle(0, 0, base.txフレーム.Guitar.sz画像サイズ.Width, 68));
                base.txハイスピ.t2D描画(CDTXMania.app.Device, - 36 + base.n本体X.Guitar + base.txフレーム.Guitar.sz画像サイズ.Width, 30, new Rectangle(0, CDTXMania.ConfigIni.n譜面スクロール速度.Guitar * 48, 42, 48));
                if (base.db現在のゲージ値.Guitar == 1.0 && base.txフルゲージ != null)
                {
                    base.txフルゲージ.t2D描画(CDTXMania.app.Device, 6 + base.n本体X.Guitar, 31, new Rectangle(0, 0, base.txフレーム.Guitar.sz画像サイズ.Width - 48, 30));
                }
                else if (base.db現在のゲージ値.Guitar >= 0.0)
                {
                    base.txゲージ.vc拡大縮小倍率.X = (float)base.db現在のゲージ値.Guitar;
                    base.txゲージ.t2D描画(CDTXMania.app.Device, 6 + base.n本体X.Guitar, 31, new Rectangle(0, 0, base.txフレーム.Guitar.sz画像サイズ.Width - 48, 30));
                }
                base.txフレーム.Guitar.t2D描画(CDTXMania.app.Device, base.n本体X.Guitar, 0, new Rectangle(0, 68, base.txフレーム.Guitar.sz画像サイズ.Width, 68));
                /*
                if (base.IsDanger(E楽器パート.GUITAR) && base.db現在のゲージ値.Guitar >= 0.0 && this.txマスクD != null)
                {
                    this.txマスクD.t2D描画(CDTXMania.app.Device, base.n本体X.Guitar, 0);
                }
                if (base.db現在のゲージ値.Guitar == 1.0 && this.txマスクF != null)
                {
                    this.txマスクF.t2D描画(CDTXMania.app.Device, base.n本体X.Guitar, 0);
                }
                 */
            }
            #endregion

            #region [ ベースのゲージ ]
            if (base.txフレーム.Bass != null && CDTXMania.DTX.bチップがある.Bass)
            {
                base.txフレーム.Bass.t2D描画(CDTXMania.app.Device, base.n本体X.Bass - base.txフレーム.Bass.sz画像サイズ.Width, 0, new Rectangle(0, 0, base.txフレーム.Bass.sz画像サイズ.Width, 68));
                base.txハイスピ.t2D描画(CDTXMania.app.Device, 4 + base.n本体X.Bass - base.txフレーム.Bass.sz画像サイズ.Width, 30, new Rectangle(0, CDTXMania.ConfigIni.n譜面スクロール速度.Bass * 48, 42, 48));
                if (base.db現在のゲージ値.Bass == 1.0 && base.txフルゲージ != null)
                {
                    base.txフルゲージ.t2D描画(CDTXMania.app.Device, 42 + base.n本体X.Bass - base.txフレーム.Bass.sz画像サイズ.Width, 31, new Rectangle(0, 0, base.txフレーム.Bass.sz画像サイズ.Width - 48, 30));
                }
                else if (base.db現在のゲージ値.Bass >= 0.0)
                {
                    base.txゲージ.vc拡大縮小倍率.X = (float)base.db現在のゲージ値.Bass;
                    base.txゲージ.t2D描画(CDTXMania.app.Device, 42 + base.n本体X.Bass - base.txフレーム.Bass.sz画像サイズ.Width, 31, new Rectangle(0, 0, base.txフレーム.Bass.sz画像サイズ.Width - 48, 30));
                }
                base.txフレーム.Bass.t2D描画(CDTXMania.app.Device, base.n本体X.Bass - base.txフレーム.Bass.sz画像サイズ.Width, 0, new Rectangle(0, 68, base.txフレーム.Bass.sz画像サイズ.Width, 68));
                /*
                if (base.IsDanger(E楽器パート.BASS) && base.db現在のゲージ値.Bass >= 0.0 && this.txマスクD != null)
                {
                    this.txマスクD.t2D描画(CDTXMania.app.Device, base.n本体X.Bass, 0);
                }
                if (base.db現在のゲージ値.Bass == 1.0 && this.txマスクF != null)
                {
                    this.txマスクF.t2D描画(CDTXMania.app.Device, base.n本体X.Bass, 0);
                }
                 */
            }
            #endregion

            return 0;
        }

        // その他

        #region [ private ]
        //-----------------
        //-----------------
        #endregion
    }
}
