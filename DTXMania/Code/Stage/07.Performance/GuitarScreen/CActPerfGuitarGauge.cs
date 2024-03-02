using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SharpDX;
using FDK;

using Rectangle = System.Drawing.Rectangle;

namespace DTXMania
{
    internal class CActPerfGuitarGauge : CActPerfCommonGauge
    {

        // コンストラクタ

        public CActPerfGuitarGauge()
        {
            base.bNotActivated = true;
        }


        // CActivity 実装

        public override void OnActivate()
        {
            base.n本体X.Guitar = 80;
            base.n本体X.Bass = 912 + 290 + 38;
            base.OnActivate();
        }
        public override void OnDeactivate()
        {
            this.ct本体移動 = null;
            this.ct本体振動 = null;
            base.OnDeactivate();
        }
        public override void OnManagedCreateResources()
        {
            if (!base.bNotActivated && !CDTXMania.DTXVmode.Enabled && !CDTXMania.DTX2WAVmode.Enabled)
            {
                this.txフレーム.Guitar = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Gauge_Guitar.png"));
                this.txフレーム.Bass = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Gauge_Bass.png"));
                this.txフルゲージ = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_gauge_bar.jpg"));
                this.txゲージ = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_gauge_bar.png"));

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
                CDTXMania.tReleaseTexture(ref this.txゲージ);
                CDTXMania.tReleaseTexture(ref this.txフルゲージ);
                CDTXMania.tReleaseTexture(ref this.txフレーム.Guitar);
                CDTXMania.tReleaseTexture(ref this.txフレーム.Bass);

                //CDTXMania.tReleaseTexture(ref this.txマスクF);
                //CDTXMania.tReleaseTexture(ref this.txマスクD);
                CDTXMania.tReleaseTexture(ref this.txハイスピ);

                base.OnManagedReleaseResources();
            }
        }
        public override int OnUpdateAndDraw()
        {
            //int num9;
            if (base.bJustStartedUpdate)
            {
                this.ct本体移動 = new CCounter(0, 0x1a, 20, CDTXMania.Timer);
                this.ct本体振動 = new CCounter(0, 360, 4, CDTXMania.Timer);
                base.bJustStartedUpdate = false;
            }
            this.ct本体移動.tUpdateLoop();
            this.ct本体振動.tUpdateLoop();
            
            #region [ ギターのゲージ ]
            if (base.txフレーム.Guitar != null && CDTXMania.DTX.bチップがある.Guitar)
            {
                base.txフレーム.Guitar.tDraw2D(CDTXMania.app.Device, base.n本体X.Guitar, 0, new Rectangle(0, 0, base.txフレーム.Guitar.szImageSize.Width, 68));
                base.txハイスピ.vcScaleRatio = new Vector3(0.76190476190476190476190476190476f, 0.66666666666666666666666666666667f, 1.0f);
                int speedTexturePosY = CDTXMania.ConfigIni.nScrollSpeed.Guitar * 48 > 20 * 48 ? 20 * 48 : CDTXMania.ConfigIni.nScrollSpeed.Guitar * 48;
                base.txハイスピ.tDraw2D(CDTXMania.app.Device, - 36 + base.n本体X.Guitar + base.txフレーム.Guitar.szImageSize.Width, 30, new Rectangle(0, speedTexturePosY, 42, 48));
                if (base.db現在のゲージ値.Guitar == 1.0 && base.txフルゲージ != null)
                {
                    base.txフルゲージ.tDraw2D(CDTXMania.app.Device, 6 + base.n本体X.Guitar, 31, new Rectangle(0, 0, base.txフレーム.Guitar.szImageSize.Width - 48, 30));
                }
                else if (base.db現在のゲージ値.Guitar >= 0.0)
                {
                    base.txゲージ.vcScaleRatio.X = (float)base.db現在のゲージ値.Guitar;
                    base.txゲージ.tDraw2D(CDTXMania.app.Device, 6 + base.n本体X.Guitar, 31, new Rectangle(0, 0, base.txフレーム.Guitar.szImageSize.Width - 48, 30));
                }
                base.txフレーム.Guitar.tDraw2D(CDTXMania.app.Device, base.n本体X.Guitar, 0, new Rectangle(0, 68, base.txフレーム.Guitar.szImageSize.Width, 68));
                /*
                if (base.IsDanger(EInstrumentPart.GUITAR) && base.db現在のゲージ値.Guitar >= 0.0 && this.txマスクD != null)
                {
                    this.txマスクD.tDraw2D(CDTXMania.app.Device, base.n本体X.Guitar, 0);
                }
                if (base.db現在のゲージ値.Guitar == 1.0 && this.txマスクF != null)
                {
                    this.txマスクF.tDraw2D(CDTXMania.app.Device, base.n本体X.Guitar, 0);
                }
                 */
            }
            #endregion

            #region [ ベースのゲージ ]
            if (base.txフレーム.Bass != null && CDTXMania.DTX.bチップがある.Bass)
            {
                base.txフレーム.Bass.tDraw2D(CDTXMania.app.Device, base.n本体X.Bass - base.txフレーム.Bass.szImageSize.Width, 0, new Rectangle(0, 0, base.txフレーム.Bass.szImageSize.Width, 68));
                base.txハイスピ.vcScaleRatio = new Vector3(0.76190476190476190476190476190476f, 0.66666666666666666666666666666667f, 1.0f);
                int speedTexturePosY = CDTXMania.ConfigIni.nScrollSpeed.Bass * 48 > 20 * 48 ? 20 * 48 : CDTXMania.ConfigIni.nScrollSpeed.Bass * 48;
                base.txハイスピ.tDraw2D(CDTXMania.app.Device, 4 + base.n本体X.Bass - base.txフレーム.Bass.szImageSize.Width, 30, new Rectangle(0, speedTexturePosY, 42, 48));
                if (base.db現在のゲージ値.Bass == 1.0 && base.txフルゲージ != null)
                {
                    base.txフルゲージ.tDraw2D(CDTXMania.app.Device, 42 + base.n本体X.Bass - base.txフレーム.Bass.szImageSize.Width, 31, new Rectangle(0, 0, base.txフレーム.Bass.szImageSize.Width - 48, 30));
                }
                else if (base.db現在のゲージ値.Bass >= 0.0)
                {
                    base.txゲージ.vcScaleRatio.X = (float)base.db現在のゲージ値.Bass;
                    base.txゲージ.tDraw2D(CDTXMania.app.Device, 42 + base.n本体X.Bass - base.txフレーム.Bass.szImageSize.Width, 31, new Rectangle(0, 0, base.txフレーム.Bass.szImageSize.Width - 48, 30));
                }
                base.txフレーム.Bass.tDraw2D(CDTXMania.app.Device, base.n本体X.Bass - base.txフレーム.Bass.szImageSize.Width, 0, new Rectangle(0, 68, base.txフレーム.Bass.szImageSize.Width, 68));
                /*
                if (base.IsDanger(EInstrumentPart.BASS) && base.db現在のゲージ値.Bass >= 0.0 && this.txマスクD != null)
                {
                    this.txマスクD.tDraw2D(CDTXMania.app.Device, base.n本体X.Bass, 0);
                }
                if (base.db現在のゲージ値.Bass == 1.0 && this.txマスクF != null)
                {
                    this.txマスクF.tDraw2D(CDTXMania.app.Device, base.n本体X.Bass, 0);
                }
                 */
            }
            #endregion

            return 0;
        }

        // Other

        #region [ private ]
        //-----------------
        //-----------------
        #endregion
    }
}
