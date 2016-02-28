using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CAct演奏DrumsレーンフラッシュD : CActivity
	{
		// コンストラクタ

		public CAct演奏DrumsレーンフラッシュD()
		{
            STレーンサイズ[] stレーンサイズArray = new STレーンサイズ[11];
            STレーンサイズ stレーンサイズ = new STレーンサイズ();
            stレーンサイズ.x = 0x12a;
            stレーンサイズ.w = 0x40;
            stレーンサイズArray[0] = stレーンサイズ;
            STレーンサイズ stレーンサイズ2 = new STレーンサイズ();
            stレーンサイズ2.x = 370;
            stレーンサイズ2.w = 0x2e;
            stレーンサイズArray[1] = stレーンサイズ2;
            STレーンサイズ stレーンサイズ3 = new STレーンサイズ();
            stレーンサイズ3.x = 470;
            stレーンサイズ3.w = 0x36;
            stレーンサイズArray[2] = stレーンサイズ3;
            STレーンサイズ stレーンサイズ4 = new STレーンサイズ();
            stレーンサイズ4.x = 582;
            stレーンサイズ4.w = 60;
            stレーンサイズArray[3] = stレーンサイズ4;
            STレーンサイズ stレーンサイズ5 = new STレーンサイズ();
            stレーンサイズ5.x = 0x210;
            stレーンサイズ5.w = 0x2e;
            stレーンサイズArray[4] = stレーンサイズ5;
            STレーンサイズ stレーンサイズ6 = new STレーンサイズ();
            stレーンサイズ6.x = 0x285;
            stレーンサイズ6.w = 0x2e;
            stレーンサイズArray[5] = stレーンサイズ6;
            STレーンサイズ stレーンサイズ7 = new STレーンサイズ();
            stレーンサイズ7.x = 0x2b6;
            stレーンサイズ7.w = 0x2e;
            stレーンサイズArray[6] = stレーンサイズ7;
            STレーンサイズ stレーンサイズ8 = new STレーンサイズ();
            stレーンサイズ8.x = 0x2ec;
            stレーンサイズ8.w = 0x40;
            stレーンサイズArray[7] = stレーンサイズ8;
            STレーンサイズ stレーンサイズ9 = new STレーンサイズ();
            stレーンサイズ9.x = 0x1a3;
            stレーンサイズ9.w = 0x30;
            stレーンサイズArray[8] = stレーンサイズ9;
            STレーンサイズ stレーンサイズ10 = new STレーンサイズ();
            stレーンサイズ10.x = 0x32f;
            stレーンサイズ10.w = 0x26;
            stレーンサイズArray[9] = stレーンサイズ10;
            STレーンサイズ stレーンサイズ11 = new STレーンサイズ();
            stレーンサイズ11.x = 0x1a3;
            stレーンサイズ11.w = 0x30;
            stレーンサイズArray[10] = stレーンサイズ11;
            this.stレーンサイズ = stレーンサイズArray;
            this.strファイル名 = new string[] { 
        @"Graphics\ScreenPlayDrums lane flush leftcymbal.png",
        @"Graphics\ScreenPlayDrums lane flush hihat.png",
        @"Graphics\ScreenPlayDrums lane flush snare.png",
        @"Graphics\ScreenPlayDrums lane flush bass.png",
        @"Graphics\ScreenPlayDrums lane flush hitom.png",
        @"Graphics\ScreenPlayDrums lane flush lowtom.png",
        @"Graphics\ScreenPlayDrums lane flush floortom.png",
        @"Graphics\ScreenPlayDrums lane flush cymbal.png",
        @"Graphics\ScreenPlayDrums lane flush leftpedal.png",
        @"Graphics\ScreenPlayDrums lane flush hihat.png",
        @"Graphics\ScreenPlayDrums lane flush leftpedal.png",

        @"Graphics\ScreenPlayDrums lane flush leftcymbal reverse.png",
        @"Graphics\ScreenPlayDrums lane flush hihat reverse.png",
        @"Graphics\ScreenPlayDrums lane flush snare reverse.png",
        @"Graphics\ScreenPlayDrums lane flush bass reverse.png",
        @"Graphics\ScreenPlayDrums lane flush hitom reverse.png", 
        @"Graphics\ScreenPlayDrums lane flush lowtom reverse.png",
        @"Graphics\ScreenPlayDrums lane flush floortom reverse.png",
        @"Graphics\ScreenPlayDrums lane flush cymbal reverse.png",
        @"Graphics\ScreenPlayDrums lane flush leftpedal reverse.png",
        @"Graphics\ScreenPlayDrums lane flush hihat reverse.png",
        @"Graphics\ScreenPlayDrums lane flush leftpedal reverse.png"
     };


           

			base.b活性化してない = true;
		}
		
		
		// メソッド

		public void Start( Eレーン lane, float f強弱度合い )
		{
			int num = (int) ( ( 1f - f強弱度合い ) * 55f );
			this.ct進行[ (int) lane ] = new CCounter( num, 90, 3, CDTXMania.Timer );
		}


		// CActivity 実装

		public override void On活性化()
		{
			for( int i = 0; i < 11; i++ )
			{
				this.ct進行[ i ] = new CCounter();
			}
			base.On活性化();
		}
		public override void On非活性化()
		{
			for( int i = 0; i < 11; i++ )
			{
				this.ct進行[ i ] = null;
			}
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
                if (CDTXMania.ConfigIni.nLaneDisp.Drums == 0 || CDTXMania.ConfigIni.nLaneDisp.Drums == 2)
                {
                    this.txLine = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Paret.png"));
                    this.txBass = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Paret.png"));
                    this.txHitom = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Paret.png"));
                }
                else
                {
                    this.txLine = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Paret_Dark.png"));
                    this.txBass = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Paret_Dark.png"));
                    this.txHitom = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Paret_Dark.png"));
                }

				for( int i = 0; i < 22; i++ )
				{
					this.txFlush[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( this.strファイル名[ i ] ) );
				}
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				for( int i = 0; i < 22; i++ )
				{
					CDTXMania.tテクスチャの解放( ref this.txFlush[ i ] );
				}
                CDTXMania.tテクスチャの解放(ref this.txLC);
                CDTXMania.tテクスチャの解放(ref this.txLine);
                CDTXMania.tテクスチャの解放(ref this.txBass);
                CDTXMania.tテクスチャの解放(ref this.txHitom);
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
				for( int i = 0; i < 11; i++ )
				{
					if( !this.ct進行[ i ].b停止中 )
					{
						this.ct進行[ i ].t進行();
						if( this.ct進行[ i ].b終了値に達した )
						{
							this.ct進行[ i ].t停止();
						}
					}
				}
                for ( int i = 0; i < 10; i++ )
                {
                        int index = this.n描画順[i];
                        int x2 = (CDTXMania.stage演奏ドラム画面.actPad.st基本位置[index].x + 32);
                        int x3 = (CDTXMania.stage演奏ドラム画面.actPad.st基本位置[index].x + (CDTXMania.ConfigIni.bReverse.Drums ? 32 : 32));
                        int xHH = (CDTXMania.stage演奏ドラム画面.actPad.st基本位置[index].x + 32);
                        int xLC = (CDTXMania.stage演奏ドラム画面.actPad.st基本位置[index].x + (CDTXMania.ConfigIni.bReverse.Drums ? 32 : 32));
                        int xCY = (CDTXMania.stage演奏ドラム画面.actPad.st基本位置[index].x + (CDTXMania.ConfigIni.bReverse.Drums ? 79 : 79));
                        int nAlpha = 255 - ((int)(((float)(CDTXMania.ConfigIni.nMovieAlpha * 255)) / 10f));
                        //if (CDTXMania.ConfigIni.eDark == Eダークモード.OFF) //2013.02.17 kairera0467 ダークOFF以外でも透明度を有効にした。
                    if (this.txLine != null)
                    {
                        this.txLine.n透明度 = nAlpha;
                        this.txBass.n透明度 = nAlpha;
                        this.txHitom.n透明度 = nAlpha;
                        #region[ 動くレーン ]
                        if (CDTXMania.ConfigIni.nLaneDisp.Drums == 0 || CDTXMania.ConfigIni.nLaneDisp.Drums == 2)
                        {
                            if (index == 0) //LC
                            {
                                this.txLine.t2D描画(CDTXMania.app.Device, 295, 0, new Rectangle(0, 0, 72, 720)); //左の棒
                            }
                            if (index == 1) //HH
                            {
                                this.txLine.t2D描画(CDTXMania.app.Device, 367, 0, new Rectangle(72, 0, 49, 720)); //左の棒
                            }
                            if (index == 2) //SD
                            {
                                if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.A || CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.C)
                                {
                                    this.txLine.t2D描画(CDTXMania.app.Device, 467, 0, new Rectangle(172, 0, 57, 720));
                                }
                                else if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.B || CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.D)
                                {
                                    this.txLine.t2D描画(CDTXMania.app.Device, 416, 0, new Rectangle(172, 0, 57, 720));
                                }
                            }
                            if (index == 3) //BD
                            {
                                if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.A || CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.D)
                                {
                                    this.txLine.t2D描画(CDTXMania.app.Device, 573, 0, new Rectangle(278, 0, 69, 720));
                                }
                                else if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.B || CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.C)
                                {
                                    this.txLine.t2D描画(CDTXMania.app.Device, 524, 0, new Rectangle(278, 0, 69, 720));
                                }
                            }
                            if (index == 4) //HT
                            {
                                if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.A)
                                {
                                    this.txLine.t2D描画(CDTXMania.app.Device, 524, 0, new Rectangle(229, 0, 49, 720));
                                }
                                else if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.B || CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.C)
                                {
                                    this.txLine.t2D描画(CDTXMania.app.Device, 593, 0, new Rectangle(229, 0, 49, 720));
                                }
                                else if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.D)
                                {
                                    this.txLine.t2D描画(CDTXMania.app.Device, 473, 0, new Rectangle(229, 0, 49, 720));
                                }
                            }

                            if (index == 5) //LT
                            {
                                this.txLine.t2D描画(CDTXMania.app.Device, 642, 0, new Rectangle(347, 0, 49, 720));
                            }
                            if (index == 6) //FT
                            {
                                this.txLine.t2D描画(CDTXMania.app.Device, 691, 0, new Rectangle(396, 0, 54, 720));
                                //if (CDTXMania.ConfigIni.eRDPosition == ERDPosition.RCRD)
                                {
                                    //this.txLine.t2D描画(CDTXMania.app.Device, 742, 0, new Rectangle(447, 0, 5, 720));
                                }
                                //else
                                {
                                    //this.txLine.t2D描画(CDTXMania.app.Device, 742, 0, new Rectangle(447, 0, 4, 720));
                                }

                            }

                            if (index == 7) //CY
                            {
                                if (CDTXMania.ConfigIni.eRDPosition == ERDPosition.RCRD)
                                {
                                    this.txLine.t2D描画(CDTXMania.app.Device, 745, 0, new Rectangle(450, 0, 70, 720));
                                }
                                else if (CDTXMania.ConfigIni.eRDPosition == ERDPosition.RDRC)
                                {
                                    this.txLine.t2D描画(CDTXMania.app.Device, xCY - 31, 0, new Rectangle(450, 0, 70, 720));
                                }
                            }
                            if (index == 8) //RD
                            {
                                if (CDTXMania.ConfigIni.eRDPosition == ERDPosition.RCRD)
                                {
                                    this.txLine.t2D描画(CDTXMania.app.Device, xCY - 55, 0, new Rectangle(520, 0, 38, 720));
                                }
                                else if (CDTXMania.ConfigIni.eRDPosition == ERDPosition.RDRC)
                                {
                                    this.txLine.t2D描画(CDTXMania.app.Device, xCY - 124, 0, new Rectangle(520, 0, 38, 720));
                                }
                            }
                            if (index == 9) //LP
                            {
                                if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.A || CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.C)
                                {
                                    this.txLine.t2D描画(CDTXMania.app.Device, x2 - 12, 0, new Rectangle(121, 0, 51, 720));
                                }
                                else if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.B)
                                {
                                    this.txLine.t2D描画(CDTXMania.app.Device, x2 + 45, 0, new Rectangle(121, 0, 51, 720));
                                    //this.txLine.t2D描画(CDTXMania.app.Device, 524, 0, new Rectangle(278, 0, 6, 720));
                                }
                                else if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.D)
                                {
                                    this.txLine.t2D描画(CDTXMania.app.Device, 522, 0, new Rectangle(121, 0, 51, 720));
                                }
                            }
                        }
                        else
                        {

                            if (index == 0) //LC
                            {
                                this.txLine.t2D描画(CDTXMania.app.Device, 295, 0, new Rectangle(0, 0, 558, 720));
                            }
                        }
                    }
                    
                    #endregion
                }
                for (int j = 0; j < 11; j++)
                {
                    if (CDTXMania.ConfigIni.bLaneFlush.Drums != false)
                    {
                        if (!this.ct進行[j].b停止中)
                        {
                            int x = this.stレーンサイズ[j].x;
                            int w = this.stレーンサイズ[j].w;
                            #region[レーン切り替え関連]
                            if (j == 2)
                            {
                                //SD
                                if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.B || CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.D)
                                {
                                    x = this.stレーンサイズ[9].x - 396;
                                }
                            }
                            if (j == 3)
                            {
                                //BD
                                if ((CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.B) || (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.C))
                                {
                                    x = this.stレーンサイズ[4].x + 6;
                                }
                                else if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.D)
                                {
                                    x = this.stレーンサイズ[4].x + 54;
                                }
                            }

                            if (j == 4)
                            {
                                //HT
                                if ((CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.B) || (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.C))
                                {
                                    x = this.stレーンサイズ[3].x + 13;
                                }
                                else if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.D)
                                {
                                    x = this.stレーンサイズ[3].x - 106;
                                }
                            }

                            if (j == 7)
                            {
                                if ((CDTXMania.ConfigIni.eRDPosition == ERDPosition.RDRC))
                                {
                                    x = this.stレーンサイズ[9].x - 29;
                                }
                            }

                            if (j == 9)
                            {
                                if ((CDTXMania.ConfigIni.eRDPosition == ERDPosition.RDRC))
                                {
                                    x = this.stレーンサイズ[7].x;
                                }
                            }

                            if ((j == 8) || (j == 10))
                            {
                                if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.B)
                                {
                                    x = this.stレーンサイズ[2].x + 5;
                                }
                                else if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.D)
                                {
                                    x = this.stレーンサイズ[2].x + 56;
                                }

                            }
                            #endregion
                            for (int k = 0; k < 3; k++)
                            {
                                if (CDTXMania.ConfigIni.bReverse.Drums)
                                {
                                    int y = 32 + ((this.ct進行[j].n現在の値 * 740) / 100);
                                    for (int m = 0; m < w; m += 42)
                                    {
                                        if (this.txFlush[j + 11] != null)
                                        {
                                            this.txFlush[j + 11].t2D描画(CDTXMania.app.Device, x + m, y, new Rectangle((k * 42), 0, ((w - m) < 42) ? (w - m) : 42, 128));
                                        }
                                    }
                                }
                                else
                                {
                                    int num8 = (200 + (500)) - ((this.ct進行[j].n現在の値 * 740) / 100);
                                    if (num8 < 720)
                                    {
                                        for (int n = 0; n < w; n += 42)
                                        {
                                            if (this.txFlush[j] != null)
                                            {
                                                this.txFlush[j].n透明度 = (num8);
                                                this.txFlush[j].t2D描画(CDTXMania.app.Device, x + n, num8, new Rectangle(k * 42, 0, ((w - n) < 42) ? (w - n) : 42, 128));
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
				}
			}
			return 0;
		}

		
		// その他

		#region [ private ]
		//-----------------
		[StructLayout( LayoutKind.Sequential )]
		private struct STレーンサイズ
		{
			public int x;
			public int w;
		}
        private readonly STレーンサイズ[] stレーンサイズ;
		private CCounter[] ct進行 = new CCounter[ 11 ];
		private readonly string[] strファイル名;
        private readonly int[] n描画順 = new int[] { 9, 2, 4, 6, 5, 3, 1, 8, 7, 0};
		private CTexture[] txFlush = new CTexture[ 0x16 ];
        private CTexture txLC;
        private CTexture txLine;
        private CTexture txBass;
        private CTexture txHitom;
		//-----------------
		#endregion
	}
}
