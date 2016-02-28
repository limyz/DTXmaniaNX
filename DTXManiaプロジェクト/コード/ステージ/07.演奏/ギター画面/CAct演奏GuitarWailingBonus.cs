using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CAct演奏GuitarWailingBonus : CAct演奏WailingBonus共通
	{
		// メソッド
        private int posXGuitar = CDTXMania.ConfigIni.nWailingFireX.Guitar;
        private int posXBass = CDTXMania.ConfigIni.nWailingFireX.Bass;
        private int posY = CDTXMania.ConfigIni.nWailingFireY;
        private int rectW = CDTXMania.ConfigIni.nWailingFireWidgh;
        private int rectH = CDTXMania.ConfigIni.nWailingFireHeight;
        private int frames = CDTXMania.ConfigIni.nWailingFireFrames;
        private int interval = CDTXMania.ConfigIni.nWailingFireInterval;


		public CAct演奏GuitarWailingBonus()
		{
			base.b活性化してない = true;
		}
		//public override void Start( E楽器パート part )
		//{
		//    this.Start( part, null );
		//}
		public override void Start( E楽器パート part, CDTX.CChip r歓声Chip )
		{
			if( part != E楽器パート.DRUMS )
			{
				for( int i = 0; i < 4; i++ )
				{
					if( ( this.ct進行用[ (int) part, i ] == null ) || this.ct進行用[ (int) part, i ].b停止中 )
					{
						this.ct進行用[ (int) part, i ] = new CCounter( 0, 300, 2, CDTXMania.Timer );
                        this.ctWailing炎[ (int) part, i ] = new CCounter( 0, this.frames, this.interval, CDTXMania.Timer );
						if( CDTXMania.ConfigIni.b歓声を発声する )
						{
							if( r歓声Chip != null )
							{
								CDTXMania.DTX.tチップの再生( r歓声Chip, CSound管理.rc演奏用タイマ.nシステム時刻, (int) Eレーン.BGM, CDTXMania.DTX.nモニタを考慮した音量( E楽器パート.UNKNOWN ) );
								return;
							}
							CDTXMania.Skin.sound歓声音.n位置・次に鳴るサウンド = ( part == E楽器パート.GUITAR ) ? -50 : 50;
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
					this.ct進行用[ i, j ] = null;
                    this.ctWailing炎[ i, j  ] = null;
				}
			}
			base.On活性化();
		}
		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
				for( int i = 0; i < 2; i++ )
				{
					E楽器パート e楽器パート = ( i == 0 ) ? E楽器パート.GUITAR : E楽器パート.BASS;
					for( int k = 0; k < 4; k++ )
					{
						if( ( this.ct進行用[ (int) e楽器パート, k ] != null ) && !this.ct進行用[ (int) e楽器パート, k ].b停止中 )
						{
							if( this.ct進行用[ (int) e楽器パート, k ].b終了値に達した )
							{
								this.ct進行用[ (int) e楽器パート, k ].t停止();
							}
							else
							{
								this.ct進行用[ (int) e楽器パート, k ].t進行();
							}
						}

                        if( ( this.ctWailing炎[ (int) e楽器パート, k ] != null ) && ( !this.ctWailing炎[ (int) e楽器パート, k ].b停止中 ) )
                        {
                            if( this.ctWailing炎[ (int) e楽器パート, k ].b終了値に達した )
                            {
                                this.ctWailing炎[ (int) e楽器パート, k ].t停止();
                            }
                            else
                            {
                                this.ctWailing炎[ (int) e楽器パート, k ].t進行();
                            }
                        }

					}
				}
				for( int j = 0; j < 2; j++ )
				{
					E楽器パート e楽器パート2 = ( j == 0 ) ? E楽器パート.GUITAR : E楽器パート.BASS;
					for( int m = 0; m < 4; m++ )
					{
						if( ( this.ct進行用[ (int) e楽器パート2, m ] != null ) && !this.ct進行用[ (int) e楽器パート2, m ].b停止中 )
						{
                            //XGではWailingレーンの幅が42px
							int x = ( ( e楽器パート2 == E楽器パート.GUITAR ) ? 160 : 1030 ) + 133;
							int num6 = 0;
							int num7 = 0;
							int num8 = this.ct進行用[ (int) e楽器パート2, m ].n現在の値;
							if( num8 < 100 )
							{
								num6 = (int) ( 120.0 + ( 290.0 * Math.Cos( Math.PI / 2 * ( ( (double) num8 ) / 100.0 ) ) ) );
							}
							else if( num8 < 150 )
							{
								num6 = (int) ( 120.0 + ( ( 150 - num8 ) * Math.Sin( ( Math.PI * ( ( num8 - 100 ) % 25 ) ) / 25.0 ) ) );
							}
							else if( num8 < 200 )
							{
								num6 = 64;
							}
							else
							{
								num6 = (int) ( 64.0 - ( ( (double) ( 290 * ( num8 - 200 ) ) ) / 100.0 ) );
							}
							if( CDTXMania.ConfigIni.bReverse[ (int) e楽器パート2 ] )
							{
								num6 = ( 670 - num6 ) - 244;
							}
							Rectangle rectangle = new Rectangle( 0, 0, 26, 122 );
							if( ( 720 - num6 ) < rectangle.Bottom )
							{
								rectangle.Height = ( 720 - num6 ) - rectangle.Top;
							}
							if( num6 < 0 )
							{
								rectangle.Y = -num6;
								rectangle.Height -= -num6;
								num7 = -num6;
							}
							if( ( rectangle.Top < rectangle.Bottom ) && ( this.txWailingBonus != null ) )
							{
								this.txWailingBonus.t2D描画( CDTXMania.app.Device, x, num6 + num7, rectangle );
							}
							num7 = 0;
							rectangle = new Rectangle( 26, 0, 26, 122 );
							if( ( 720 - ( num6 + 122 ) ) < rectangle.Bottom )
							{
								rectangle.Height = ( 720 - ( num6 + 122 ) ) - rectangle.Top;
							}
							if( ( num6 + 0x7a ) < 0 )
							{
								rectangle.Y = -( num6 + 122 );
								rectangle.Height -= rectangle.Y;
								num7 = -( num6 + 122 );
							}
							if( ( rectangle.Top < rectangle.Bottom ) && ( this.txWailingBonus != null ) && CDTXMania.ConfigIni.nWailingFireFrames == 0 )
							{
								this.txWailingBonus.t2D描画( CDTXMania.app.Device, x, ( num6 + num7 ) + 122, rectangle );
							}

                            if( this.txWailingFlush != null && CDTXMania.ConfigIni.nWailingFireFrames == 0 )
                            {
                                for( int i = 0; i <= 12; i++ )
                                {
                                    this.txWailingFlush.t2D描画( CDTXMania.app.Device, ( e楽器パート2 == E楽器パート.GUITAR ) ? 283 : 1153, 64 * i, new Rectangle( 0, 0, 42, 64 ) );
                                }

                                int count = this.ct進行用[ (int)e楽器パート2, m ].n現在の値;
                                this.txWailingFlush.n透明度 = ( count <= 55 ? 255 : 255 - count );
                            }
						}

                        if( ( this.ctWailing炎[ (int) e楽器パート2, m ] != null ) && !this.ctWailing炎[ (int) e楽器パート2, m ].b停止中 )
                        {
                            if( this.txWailingFire != null )
                            {
                                if( e楽器パート2 == E楽器パート.GUITAR )
                                {
                                    this.txWailingFire.t2D描画( CDTXMania.app.Device, this.posXGuitar, this.posY, new Rectangle( this.rectW * this.ctWailing炎[ (int) e楽器パート2, m ].n現在の値, 0, this.rectW, this.rectH ) );
                                }
                                if( e楽器パート2 == E楽器パート.BASS )
                                {
                                    this.txWailingFire.t2D描画( CDTXMania.app.Device, this.posXBass, this.posY, new Rectangle( this.rectW * this.ctWailing炎[ (int) e楽器パート2, m ].n現在の値, 0, this.rectW, this.rectH ) );
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
