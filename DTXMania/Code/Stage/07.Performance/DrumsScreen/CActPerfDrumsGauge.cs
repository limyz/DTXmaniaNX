using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using SharpDX;
using FDK;

using Rectangle = System.Drawing.Rectangle;

namespace DTXMania
{
    internal class CActPerfDrumsGauge : CActPerfCommonGauge
    {

        // コンストラクタ

        public CActPerfDrumsGauge()
        {
            base.bNotActivated = true;
        }


        // CActivity 実装

        public override void OnActivate()
        {
            base.n本体X.Drums = 294;
            base.OnActivate();
        }
        public override void OnDeactivate()
        {
            base.OnDeactivate();
        }
        public override void OnManagedCreateResources()
        {
            if (!base.bNotActivated && !CDTXMania.DTXVmode.Enabled && !CDTXMania.DTX2WAVmode.Enabled)
            {
                this.txフレーム.Drums = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Gauge.png"));
                this.txゲージ = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_gauge_bar.png"));
                this.txフルゲージ = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_gauge_bar.jpg"));

                //this.txマスクF = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Dummy.png"));
                //this.txマスクD = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Dummy.png"));
                this.txハイスピ = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Panel_icons.jpg"));

                base.OnManagedCreateResources();
            }
        }
        public override void OnManagedReleaseResources()
        {
            if (!base.bNotActivated && !CDTXMania.DTXVmode.Enabled && !CDTXMania.DTX2WAVmode.Enabled)
            {
                CDTXMania.tReleaseTexture(ref this.txフレーム.Drums);
                CDTXMania.tReleaseTexture(ref this.txゲージ);
                CDTXMania.tReleaseTexture(ref this.txフルゲージ);

                //CDTXMania.tReleaseTexture(ref this.txマスクF);
                //CDTXMania.tReleaseTexture(ref this.txマスクD);
                CDTXMania.tReleaseTexture(ref this.txハイスピ);

                base.OnManagedReleaseResources();
            }
        }
        public override int OnUpdateAndDraw()
        {

            if (!base.bNotActivated)
            {

                if (base.txフレーム.Drums != null)
                {
                    base.txフレーム.Drums.tDraw2D(CDTXMania.app.Device, base.n本体X.Drums, (CDTXMania.ConfigIni.bReverse.Drums ? 28 : 626), new Rectangle(0, 0, base.txフレーム.Drums.szImageSize.Width, 47));
                    base.txハイスピ.vcScaleRatio = new Vector3(0.76190476190476190476190476190476f, 0.66666666666666666666666666666667f, 1.0f);
                    int speedTexturePosY = CDTXMania.ConfigIni.nScrollSpeed.Drums * 48 > 20 * 48 ? 20 * 48 : CDTXMania.ConfigIni.nScrollSpeed.Drums * 48;
                    base.txハイスピ.tDraw2D(CDTXMania.app.Device, -37 + base.n本体X.Drums + base.txフレーム.Drums.szImageSize.Width, (CDTXMania.ConfigIni.bReverse.Drums ? 35 : 634), new Rectangle(0, speedTexturePosY, 42, 48));
                    if (base.db現在のゲージ値.Drums == 1.0 && base.txフルゲージ != null)
                    {
                        base.txフルゲージ.tDraw2D(CDTXMania.app.Device, 20 + base.n本体X.Drums, (CDTXMania.ConfigIni.bReverse.Drums ? 37 : 635), new Rectangle(0, 0, base.txフレーム.Drums.szImageSize.Width - 63, 31));
                    }
                    else
                    {
                        base.txゲージ.vcScaleRatio.X = (float)base.db現在のゲージ値.Drums;
                        base.txゲージ.tDraw2D(CDTXMania.app.Device, 20 + base.n本体X.Drums, (CDTXMania.ConfigIni.bReverse.Drums ? 37 : 635), new Rectangle(0, 0, base.txフレーム.Drums.szImageSize.Width - 63, 31));
                    }
                    base.txフレーム.Drums.tDraw2D(CDTXMania.app.Device, base.n本体X.Drums, (CDTXMania.ConfigIni.bReverse.Drums ? 28 : 626), new Rectangle(0, 47, base.txフレーム.Drums.szImageSize.Width, 47));
                }
                /*
                if (base.IsDanger(EInstrumentPart.DRUMS) && base.db現在のゲージ値.Drums >= 0.0)
                {
                    this.txマスクD.tDraw2D(CDTXMania.app.Device, base.n本体X.Drums, (CDTXMania.ConfigIni.bReverse.Drums ? 28 : 626));
                }
                if (base.db現在のゲージ値.Drums == 1.0)
                {
                    this.txマスクF.tDraw2D(CDTXMania.app.Device, base.n本体X.Drums, (CDTXMania.ConfigIni.bReverse.Drums ? 28 : 626));
                }
                 */
            }
            return 0;
        }

        // Other

        #region [ private ]
        //-----------------
        //-----------------
        #endregion
    }
}
