using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CActPerfDrumsDanger : CActPerfCommonDanger
	{
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
                this.tx黒 = CDTXMania.tGenerateTexture(CSkin.Path( @"Graphics\7_Danger.png" ) );
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
                CDTXMania.tReleaseTexture(ref this.tx黒);
				base.OnManagedReleaseResources();
			}
		}
		public override int OnUpdateAndDraw()
		{
			throw new InvalidOperationException( "tUpdateAndDraw(bool)のほうを使用してください。" );
		}
		/// <summary>
		/// ドラム画面のDANGER描画
		/// </summary>
		/// <param name="bIsDangerDrums">DrumsのゲージがDangerかどうか(Guitar/Bassと共用のゲージ)</param>
		/// <param name="bIsDangerGuitar">Guitarのゲージ(未使用)</param>
		/// <param name="bIsDangerBass">Bassのゲージ(未使用)</param>
		/// <returns></returns>
		public override int t進行描画( bool bIsDangerDrums, bool bIsDangerGuitar, bool bIsDangerBass )
		{
			if( !base.bNotActivated )
			{
				if( !bIsDangerDrums )
				{
					this.bDanger中[(int)EInstrumentPart.DRUMS] = false;
					return 0;
				}
                if (!this.bDanger中[(int)EInstrumentPart.DRUMS])
                {
                    this.ct移動用 = new CCounter(0, 0x7f, 7, CDTXMania.Timer);
                    this.ct透明度用 = new CCounter(0, 250, 4, CDTXMania.Timer);
                }
                    this.bDanger中[(int)EInstrumentPart.DRUMS] = bIsDangerDrums;
                    this.ct移動用.tUpdateLoop();
                    this.ct透明度用.tUpdateLoop();
                    if (!this.bDanger中[(int)EInstrumentPart.DRUMS])
                    {
                        return 0;
                    }
                    int num = this.ct透明度用.nCurrentValue;
                    this.tx黒.nTransparency = num;　　//
                    num = this.ct移動用.nCurrentValue;
                    int num2 = num;
                    for (int i = 0; i < 2; i++)
                    {
                        this.tx黒.tDraw2D(CDTXMania.app.Device, 0, 0);
                    }
                
			}
			return 0;
		}


		// Other

		#region [ private ]
		//-----------------
		//private bool bDanger中;
		//private CCounter ct移動用;
		//private CCounter ct透明度用;
//		private const int n右位置 = 0x12a;
//		private const int n左位置 = 0x26;
		private readonly Rectangle[] rc領域 = new Rectangle[] { new Rectangle( 0, 0, 0x20, 0x40 ), new Rectangle( 0x20, 0, 0x20, 0x40 ) };
        private CTexture tx黒;
		//-----------------
		#endregion
	}
}
