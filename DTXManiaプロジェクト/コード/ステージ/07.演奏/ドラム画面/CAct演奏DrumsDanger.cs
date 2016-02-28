using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CAct演奏DrumsDanger : CAct演奏Danger共通
	{
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
                this.tx黒 = CDTXMania.tテクスチャの生成(CSkin.Path( @"Graphics\7_Danger.png" ) );
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
                CDTXMania.tテクスチャの解放(ref this.tx黒);
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			throw new InvalidOperationException( "t進行描画(bool)のほうを使用してください。" );
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
			if( !base.b活性化してない )
			{
				if( !bIsDangerDrums )
				{
					this.bDanger中[(int)E楽器パート.DRUMS] = false;
					return 0;
				}
                if (!this.bDanger中[(int)E楽器パート.DRUMS])
                {
                    this.ct移動用 = new CCounter(0, 0x7f, 7, CDTXMania.Timer);
                    this.ct透明度用 = new CCounter(0, 250, 4, CDTXMania.Timer);
                }
                    this.bDanger中[(int)E楽器パート.DRUMS] = bIsDangerDrums;
                    this.ct移動用.t進行Loop();
                    this.ct透明度用.t進行Loop();
                    if (!this.bDanger中[(int)E楽器パート.DRUMS])
                    {
                        return 0;
                    }
                    int num = this.ct透明度用.n現在の値;
                    this.tx黒.n透明度 = num;　　//
                    num = this.ct移動用.n現在の値;
                    int num2 = num;
                    for (int i = 0; i < 2; i++)
                    {
                        this.tx黒.t2D描画(CDTXMania.app.Device, 0, 0);
                    }
                
			}
			return 0;
		}


		// その他

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
