using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using FDK;

namespace DTXMania
{
	internal class CActPerfGuitarDanger : CActPerfCommonDanger
	{

		public override void OnManagedCreateResources()
		{
			if ( !base.bNotActivated )
			{
				this.txDANGER = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlayGuitar danger.png" ) );
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if ( !base.bNotActivated )
			{
				CDTXMania.tReleaseTexture( ref this.txDANGER );
				base.OnManagedReleaseResources();
			}
		}
		public override int OnUpdateAndDraw()
		{
			throw new InvalidOperationException( "tUpdateAndDraw(bool)のほうを使用してください。" );
		}
		/// <summary>
		/// DANGER表示(Guitar/Bass)
		/// </summary>
		/// <param name="bIsDangerDrums">DrumsがDangerか否か(未使用)</param>
		/// <param name="bIsDangerGuitar">GuitarがDangerか否か</param>
		/// <param name="bIsDangerBass">BassがDangerか否か</param>
		/// <returns></returns>
		public override int t進行描画( bool bIsDangerDrums, bool bIsDangerGuitar, bool bIsDangerBass )
		{
			bool[] bIsDanger = { bIsDangerDrums, bIsDangerGuitar, bIsDangerBass };

			if ( !base.bNotActivated )
			{
				if ( this.ct透明度用 == null )
				{
					//this.ct移動用 = new CCounter( 0, 0x7f, 7, CDTXMania.Timer );
					this.ct透明度用 = new CCounter( 0, n波長, 8, CDTXMania.Timer );
				}
				if ( this.ct透明度用 != null )
				{
					//this.ct移動用.tUpdateLoop();
					this.ct透明度用.tUpdateLoop();
				}
				for ( int nPart = (int) EInstrumentPart.GUITAR; nPart <= (int) EInstrumentPart.BASS; nPart++ )
				{
				//	this.bDanger中[nPart] = bIsDanger[nPart];
					if ( bIsDanger[ nPart ] )
					{
						if ( this.txDANGER != null )
						{
							int d = this.ct透明度用.nCurrentValue;
							this.txDANGER.nTransparency = n透明度MIN + ( ( d < n波長 / 2 ) ? ( n透明度MAX - n透明度MIN ) * d / ( n波長 / 2 ) : ( n透明度MAX - n透明度MIN ) * ( n波長 - d ) / ( n波長 / 2 ) );		// 60-200
							this.txDANGER.tDraw2D( CDTXMania.app.Device, nGaugeX[ nPart ], 0 );
						}
					}
				}
			}
			return 0;
		}


		// Other

		#region [ private ]
		//-----------------
		private const int n波長 = 40;
		private const int n透明度MAX = 180;
		private const int n透明度MIN = 20;
		private readonly int[] nGaugeX = { 0, 168, 328 };
//		private readonly Rectangle[] rc領域 = new Rectangle[] { new Rectangle( 0, 0, 0x20, 0x40 ), new Rectangle( 0x20, 0, 0x20, 0x40 ) };
		private CTexture txDANGER;
		//-----------------
		#endregion
	}
}
