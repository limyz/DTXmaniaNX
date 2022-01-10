using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using FDK;

namespace DTXMania
{
    internal class CActPerfGuitarBonus : CActivity
    {
        // コンストラクタ

        public CActPerfGuitarBonus()
        {
            base.bNotActivated = true;
        }

        public override void OnActivate()
        {
            ctBonusScoreAnimationCounter[(int)EInstrumentPart.GUITAR] = new CCounter();
            ctBonusScoreAnimationCounter[(int)EInstrumentPart.BASS] = new CCounter();

            base.OnActivate();
        }

        public override void OnDeactivate()
        {

            base.OnDeactivate();
        }

        public override void OnManagedCreateResources()
        {
            if (!base.bNotActivated)
            {
                this.txBonus100 = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Bonus_100.png"));
                base.OnManagedCreateResources();
            }
        }

        public override void OnManagedReleaseResources()
        {
            if (!base.bNotActivated)
            {
                CDTXMania.tReleaseTexture(ref this.txBonus100);
                base.OnManagedReleaseResources();
            }
        }

        public override unsafe int OnUpdateAndDraw()
        {
            if (!base.bNotActivated)
            {
                if (base.bJustStartedUpdate)
                {
                    //base.n進行用タイマ = CDTXMania.Timer.nCurrentTime;                    
                    base.bJustStartedUpdate = false;
                }

                ctBonusScoreAnimationCounter[(int)EInstrumentPart.GUITAR].tUpdate();
                ctBonusScoreAnimationCounter[(int)EInstrumentPart.BASS].tUpdate();

                if (CDTXMania.ConfigIni.bShowScore)
                {
                    drawBonusScoreAnimation(EInstrumentPart.GUITAR, new Point(303, 45));
                    drawBonusScoreAnimation(EInstrumentPart.BASS, new Point(885, 45));                    
                }


            }
            return 0;
        }

        public void startBonus(EInstrumentPart eInstrument)
        {
            ctBonusScoreAnimationCounter[(int)eInstrument].tStart(0, 500, 1, CDTXMania.Timer);
        }

        private void drawBonusScoreAnimation(EInstrumentPart eInstrumentPart, Point pt)
        {
            if (!ctBonusScoreAnimationCounter[(int)eInstrumentPart].bReachedEndValue 
                && this.txBonus100 != null)
            {
                int nCounterValue = ctBonusScoreAnimationCounter[(int)eInstrumentPart].nCurrentValue;                
                this.txBonus100.tDraw2D(CDTXMania.app.Device, pt.X, pt.Y - nCounterValue / 25);
            }
        }

        // Other

        #region [ private ]
        //-----------------
        private CTexture txBonus100;
        private STDGBVALUE<CCounter> ctBonusScoreAnimationCounter;
        //-----------------
        #endregion
    }
}
