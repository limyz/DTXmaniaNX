using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using SlimDX;
using FDK;

namespace DTXMania
{
    internal class CAct演奏Drumsゲージ : CAct演奏ゲージ共通
    {

        // コンストラクタ

        public CAct演奏Drumsゲージ()
        {
            base.b活性化してない = true;
        }


        // CActivity 実装

        public override void On活性化()
        {
            base.n本体X.Drums = 294;
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
                this.txフレーム.Drums = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Gauge.png"));
                this.txゲージ = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_gauge_bar.png"));
                this.txフルゲージ = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_gauge_bar.jpg"));

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
                CDTXMania.tテクスチャの解放(ref this.txフレーム.Drums);
                CDTXMania.tテクスチャの解放(ref this.txゲージ);
                CDTXMania.tテクスチャの解放(ref this.txフルゲージ);

                //CDTXMania.tテクスチャの解放(ref this.txマスクF);
                //CDTXMania.tテクスチャの解放(ref this.txマスクD);
                CDTXMania.tテクスチャの解放(ref this.txハイスピ);

                base.OnManagedリソースの解放();
            }
        }
        public override int On進行描画()
        {

            if (!base.b活性化してない)
            {

                if (base.txフレーム.Drums != null)
                {
                    base.txフレーム.Drums.t2D描画(CDTXMania.app.Device, base.n本体X.Drums, (CDTXMania.ConfigIni.bReverse.Drums ? 28 : 626), new Rectangle(0, 0, base.txフレーム.Drums.sz画像サイズ.Width, 47));
                    base.txハイスピ.vc拡大縮小倍率 = new Vector3(0.76190476190476190476190476190476f, 0.66666666666666666666666666666667f, 1.0f);
                    base.txハイスピ.t2D描画(CDTXMania.app.Device, -37 + base.n本体X.Drums + base.txフレーム.Drums.sz画像サイズ.Width, (CDTXMania.ConfigIni.bReverse.Drums ? 35 : 634), new Rectangle(0, CDTXMania.ConfigIni.n譜面スクロール速度.Drums * 48, 42, 48));
                    if (base.db現在のゲージ値.Drums == 1.0 && base.txフルゲージ != null)
                    {
                        base.txフルゲージ.t2D描画(CDTXMania.app.Device, 20 + base.n本体X.Drums, (CDTXMania.ConfigIni.bReverse.Drums ? 37 : 635), new Rectangle(0, 0, base.txフレーム.Drums.sz画像サイズ.Width - 63, 31));
                    }
                    else
                    {
                        base.txゲージ.vc拡大縮小倍率.X = (float)base.db現在のゲージ値.Drums;
                        base.txゲージ.t2D描画(CDTXMania.app.Device, 20 + base.n本体X.Drums, (CDTXMania.ConfigIni.bReverse.Drums ? 37 : 635), new Rectangle(0, 0, base.txフレーム.Drums.sz画像サイズ.Width - 63, 31));
                    }
                    base.txフレーム.Drums.t2D描画(CDTXMania.app.Device, base.n本体X.Drums, (CDTXMania.ConfigIni.bReverse.Drums ? 28 : 626), new Rectangle(0, 47, base.txフレーム.Drums.sz画像サイズ.Width, 47));
                }
                /*
                if (base.IsDanger(E楽器パート.DRUMS) && base.db現在のゲージ値.Drums >= 0.0)
                {
                    this.txマスクD.t2D描画(CDTXMania.app.Device, base.n本体X.Drums, (CDTXMania.ConfigIni.bReverse.Drums ? 28 : 626));
                }
                if (base.db現在のゲージ値.Drums == 1.0)
                {
                    this.txマスクF.t2D描画(CDTXMania.app.Device, base.n本体X.Drums, (CDTXMania.ConfigIni.bReverse.Drums ? 28 : 626));
                }
                 */
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
