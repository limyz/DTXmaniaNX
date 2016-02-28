using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CAct演奏DrumsWailingBonus : CAct演奏WailingBonus共通
	{
		// コンストラクタ

		public CAct演奏DrumsWailingBonus()
		{
			base.b活性化してない = true;
		}
		
		
		// メソッド

		//public override void Start( E楽器パート part )
		//{
		//    this.Start( part, null );
		//}
		public override void Start( E楽器パート part, CDTX.CChip r歓声Chip )
		{
			if( ( part == E楽器パート.GUITAR ) || ( part == E楽器パート.BASS ) )
			{
				int num = (int) part;
				for( int i = 0; i < 4; i++ )
				{
					if( this.ct進行用[ num, i ].b停止中 )
					{
						this.ct進行用[ num, i ] = new CCounter( 0, 300, 2, CDTXMania.Timer );
						if( CDTXMania.ConfigIni.b歓声を発声する )
						{
							if( r歓声Chip != null )
							{
								CDTXMania.DTX.tチップの再生( r歓声Chip, CDTXMania.Timer.nシステム時刻, (int) Eレーン.BGM, CDTXMania.DTX.nモニタを考慮した音量( E楽器パート.UNKNOWN ) );
								return;
							}
							CDTXMania.Skin.sound歓声音.t再生する();
							return;
						}
						break;
					}
				}
			}
		}


		// CActivity 実装

		public override void On活性化()
		{
			for( int i = 0; i < 3; i++ )
			{
				for( int j = 0; j < 4; j++ )
				{
					this.ct進行用[ i, j ] = new CCounter();
				}
			}
			base.On活性化();
		}
		public override void On非活性化()
		{
			for( int i = 0; i < 3; i++ )
			{
				for( int j = 0; j < 4; j++ )
				{
					this.ct進行用[ i, j ] = null;
				}
			}
			base.On非活性化();
		}

		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
				for( int i = 0; i < 2; i++ )
				{
					E楽器パート e楽器パート = ( i == 0 ) ? E楽器パート.GUITAR : E楽器パート.BASS;
					for( int j = 0; j < 4; j++ )
					{
						if( !this.ct進行用[ (int) e楽器パート, j ].b停止中 )
						{
							if( this.ct進行用[ (int) e楽器パート, j ].b終了値に達した )
							{
								this.ct進行用[ (int) e楽器パート, j ].t停止();
							}
							else
							{
								this.ct進行用[ (int) e楽器パート, j ].t進行();
								int x = ( ( e楽器パート == E楽器パート.GUITAR ) ? 0x1fb : 0x18e ) + 0x4e;
								int num4 = 0;
								int num5 = 0;
								int num6 = this.ct進行用[ (int) e楽器パート, j ].n現在の値;
								if( num6 < 100 )
								{
									num4 = (int) ( 64.0 + ( 290.0 * Math.Cos( Math.PI / 2 * ( ( (float) num6 ) / 100f ) ) ) );
								}
								else if( num6 < 150 )
								{
									num4 = (int) ( 64.0 + ( ( 150 - num6 ) * Math.Sin( ( Math.PI * ( ( num6 - 100 ) % 0x19 ) ) / 25.0 ) ) );
								}
								else if( num6 < 200 )
								{
									num4 = 0x40;
								}
								else
								{
									num4 = (int) ( 64f - ( ( (float) ( 290 * ( num6 - 200 ) ) ) / 100f ) );
								}
								if( CDTXMania.ConfigIni.bReverse[ (int) e楽器パート ] )
								{
									num4 = ( 0x163 - num4 ) - 0xf4;
								}
								Rectangle rectangle = new Rectangle( 0, 0, 0x1a, 0x7a );
								if( ( 0x163 - num4 ) < rectangle.Bottom )
								{
									rectangle.Height = ( 0x163 - num4 ) - rectangle.Top;
								}
								if( num4 < 0 )
								{
									rectangle.Y = -num4;
									num5 = -num4;
								}
								if( ( rectangle.Top < rectangle.Bottom ) && ( this.txWailingBonus != null ) )
								{
									this.txWailingBonus.t2D描画( CDTXMania.app.Device, x, ( ( ( e楽器パート == E楽器パート.GUITAR ) ? 0x39 : 0x39 ) + num4 ) + num5, rectangle );
								}
								num5 = 0;
								rectangle = new Rectangle( 0x1a, 0, 0x1a, 0x7a );
								if( ( 0x163 - ( num4 + 0x7a ) ) < rectangle.Bottom )
								{
									rectangle.Height = ( 0x163 - ( num4 + 0x7a ) ) - rectangle.Top;
								}
								if( ( num4 + 0x7a ) < 0 )
								{
									rectangle.Y = -( num4 + 0x7a );
									num5 = -( num4 + 0x7a );
								}
								if( ( rectangle.Top < rectangle.Bottom ) && ( this.txWailingBonus != null ) )
								{
									this.txWailingBonus.t2D描画( CDTXMania.app.Device, x, ( ( ( ( e楽器パート == E楽器パート.GUITAR ) ? 0x39 : 0x39 ) + num4 ) + num5 ) + 0x7a, rectangle );
								}
							}
						}
					}
				}
			}
			return 0;
		}
	}
}
