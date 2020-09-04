using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CActPerfDrumsComboDGB : CActPerfCommonCombo
	{
		// CActPerfCommonCombo 実装
        public override void OnActivate()
        {
            for( int i = 0; i < 256; i++ )
            {
                this.b爆発した[ i ] = false;
                base.bn00コンボに到達した[i].Drums = false;
            }
            base.nコンボカウント.Drums = 0;
            this.n火薬カウント = 0;
            base.OnActivate();
        }
        public override void OnDeactivate()
        {
            for( int i = 0; i < 256; i++ )
            {
                this.b爆発した[ i ] = false;
                base.bn00コンボに到達した[i].Drums = false;
            }
            base.nコンボカウント.Drums = 0;
            this.n火薬カウント = 0;
            base.OnDeactivate();
        }

        public void Start( int nCombo値 )
        {
            this.n火薬カウント = nCombo値 / 100;
 
            for (int j = 0; j < 1; j++)
            {
                if (this.st爆発[j].b使用中)
                {
                    this.st爆発[j].ct進行.tStop();
                    this.st爆発[j].b使用中 = false;
                    this.b爆発した[ this.n火薬カウント ] = true;

                }
            }
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    if (!this.st爆発[j].b使用中)
                    {
                        this.st爆発[j].b使用中 = true;
                        this.st爆発[j].ct進行 = new CCounter(0, 13, 20, CDTXMania.Timer);
                        break;
                    }
                }
            }
        }

		protected override void tDrawCombo_Guitar( int nCombo値, int nジャンプインデックス )
		{
		}
		protected override void tDrawCombo_Drums( int nCombo値, int nジャンプインデックス )
		{
            bool guitar = CDTXMania.DTX.bチップがある.Guitar;
            bool bass = CDTXMania.DTX.bチップがある.Bass;

            if( CDTXMania.ConfigIni.bドラムコンボ文字の表示 )
            {
                if (CDTXMania.ConfigIni.bGraph有効.Drums && CDTXMania.ConfigIni.bSmallGraph)
                    base.tDrawCombo_Drums(nCombo値, nジャンプインデックス, 1275, 60);
                else
                    base.tDrawCombo_Drums(nCombo値, nジャンプインデックス, 1245, 60);
            }

            this.n火薬カウント = (nCombo値 / 100);

            //if (nCombo値 % 100 == 0)
            if(( nCombo値 > (nCombo値 / 100) + 100) && this.b爆発した[ n火薬カウント ] == false )
            {
                this.Start( nCombo値 );
            }

            int x = 845;
            int y = -130;

            if (nCombo値 >= 100)
            {
                for (int i = 0; i < 1; i++)
                {
                    if (this.st爆発[i].b使用中)
                    {
                        int num1 = this.st爆発[i].ct進行.nCurrentValue;
                        this.st爆発[i].ct進行.tUpdate();
                        if (this.st爆発[i].ct進行.bReachedEndValue)
                        {
                            this.st爆発[i].ct進行.tStop();
                            this.st爆発[i].b使用中 = false;
                            this.bn00コンボに到達した[this.nコンボカウント.Drums].Drums = true;
                        }
                        if ( this.txComboBom != null && CDTXMania.ConfigIni.bドラムコンボ文字の表示 != false )
                        {
                            this.txComboBom.tDraw2D(CDTXMania.app.Device, x, y, new Rectangle(0, (340 * num1), 360, 340));
                            this.txComboBom.vcScaleRatio = new SlimDX.Vector3(1.5f, 1.5f, 1f);
                        }
                    }
                }
            }

		}
		protected override void tDrawCombo_Base( int nCombo値, int nジャンプインデックス )
		{
		}
	}
}
