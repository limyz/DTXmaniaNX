using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using SlimDX;
using FDK;

namespace DTXMania
{
	internal class CAct演奏判定文字列共通 : CActivity
	{
		// プロパティ
        public int iP_A;
        public int iP_B;
		protected STSTATUS[] st状態 = new STSTATUS[ 15 ];
		[StructLayout( LayoutKind.Sequential )]
		protected struct STSTATUS
		{
			public CCounter ct進行;
			public E判定 judge;
            public float fZ軸回転度_棒;
            public float fX方向拡大率_棒;
			public float fY方向拡大率_棒;
            public int n相対X座標_棒;
			public int n相対Y座標_棒;


            public float fZ軸回転度;
			public float fX方向拡大率;
			public float fY方向拡大率;
			public int n相対X座標;
			public int n相対Y座標;

            public float fX方向拡大率B;
            public float fY方向拡大率B;
            public int n相対X座標B;
            public int n相対Y座標B;
            public int n透明度B;

			public int n透明度;
			public int nLag;								// #25370 2011.2.1 yyagi
            public int nRect;
		}

		protected readonly ST判定文字列[] st判定文字列;
		[StructLayout( LayoutKind.Sequential )]
		protected struct ST判定文字列
		{
			public int n画像番号;
			public Rectangle rc;
		}

		protected readonly STlag数値[] stLag数値;			// #25370 2011.2.1 yyagi
		[StructLayout( LayoutKind.Sequential )]
		protected struct STlag数値
		{
			public Rectangle rc;
		}

		protected CTexture[] tx判定文字列 = new CTexture[ 3 ];
		protected CTexture txlag数値 = new CTexture();		// #25370 2011.2.1 yyagi

		public int nShowLagType							// #25370 2011.6.3 yyagi
		{
			get;
			set;
		}

        [StructLayout(LayoutKind.Sequential)]
        protected struct STレーンサイズ
        {
            public int x;
            public int w;
        }

        protected STレーンサイズ[] stレーンサイズ;
		// コンストラクタ

		public CAct演奏判定文字列共通()
		{
            int iP_A = 390;
            int iP_B = 0x248;
			this.st判定文字列 = new ST判定文字列[ 7 ];
			Rectangle[] r = new Rectangle[] {
				new Rectangle( 0, 0,    0x80, 0x2a ),		// Perfect
				new Rectangle( 0, 0x2b, 0x80, 0x2a ),		// Great
				new Rectangle( 0, 0x56, 0x80, 0x2a ),		// Good
				new Rectangle( 0, 0,    0x80, 0x2a ),		// Poor
				new Rectangle( 0, 0x2b, 0x80, 0x2a ),		// Miss
				new Rectangle( 0, 0x56, 0x80, 0x2a ),		// Bad
				new Rectangle( 0, 0,    0x80, 0x2a )		// Auto
			};

			for ( int i = 0; i < 7; i++ )
			{
				this.st判定文字列[ i ] = new ST判定文字列();
				this.st判定文字列[ i ].n画像番号 = i / 3;
				this.st判定文字列[ i ].rc = r[i];
			}

			this.stLag数値 = new STlag数値[ 12 * 2 ];		// #25370 2011.2.1 yyagi
			base.b活性化してない = true;
		}


		// メソッド

		public virtual void Start( int nLane, E判定 judge, int lag )
		{
			if( ( nLane < 0 ) || ( nLane > 14 ) )
			{
				throw new IndexOutOfRangeException( "有効範囲は 0～14 です。" );
			}
			if( ( ( nLane >= 10 ) || ( ( (Eタイプ) CDTXMania.ConfigIni.判定文字表示位置.Drums ) != Eタイプ.C ) ) && ( ( ( nLane != 13 ) || ( ( (Eタイプ) CDTXMania.ConfigIni.判定文字表示位置.Guitar ) != Eタイプ.D ) ) && ( ( nLane != 14 ) || ( ( (Eタイプ) CDTXMania.ConfigIni.判定文字表示位置.Bass ) != Eタイプ.D ) ) ) )
			{
                if (CDTXMania.ConfigIni.nJudgeAnimeType != 0)
                {
                    this.st状態[nLane].ct進行 = new CCounter(0, CDTXMania.ConfigIni.nJudgeFrames - 1, CDTXMania.ConfigIni.nJudgeInterval, CDTXMania.Timer);
                }
                else
                {
                    this.st状態[nLane].ct進行 = new CCounter(0, 300, 1, CDTXMania.Timer);
                }
				this.st状態[ nLane ].judge = judge;
				this.st状態[ nLane ].fX方向拡大率 = 1f;
				this.st状態[ nLane ].fY方向拡大率 = 1f;
                this.st状態[ nLane ].fZ軸回転度 = 0f;
				this.st状態[ nLane ].n相対X座標 = 0;
				this.st状態[ nLane ].n相対Y座標 = 0;
                this.st状態[ nLane ].n透明度 = 0xff;

				this.st状態[ nLane ].fX方向拡大率B = 1f;
				this.st状態[ nLane ].fY方向拡大率B = 1f;
				this.st状態[ nLane ].n相対X座標B = 0;
				this.st状態[ nLane ].n相対Y座標B = 0;
                this.st状態[ nLane ].n透明度B = 0xff;
                
                this.st状態[ nLane ].fZ軸回転度_棒 = 0f;
                this.st状態[ nLane ].fX方向拡大率_棒 = 0;
				this.st状態[ nLane ].fY方向拡大率_棒 = 0;
				this.st状態[ nLane ].n相対X座標_棒 = 0;
				this.st状態[ nLane ].n相対Y座標_棒 = 0;                

				this.st状態[ nLane ].nLag = lag;
			}
		}


		// CActivity 実装

		public override void On活性化()
		{
			for( int i = 0; i < 15; i++ )
			{
				this.st状態[ i ].ct進行 = new CCounter();
			}

			for ( int i = 0; i < 12; i++ )
			{
                if( CDTXMania.ConfigIni.nShowLagTypeColor == 0 )
                {
				    this.stLag数値[ i      ].rc = new Rectangle( ( i % 4 ) * 15     , ( i / 4 ) * 19     , 15, 19 );	// plus numbers
				    this.stLag数値[ i + 12 ].rc = new Rectangle( ( i % 4 ) * 15 + 64, ( i / 4 ) * 19 + 64, 15, 19 );	// minus numbers
                }
                else
                {
				    this.stLag数値[ i      ].rc = new Rectangle( ( i % 4 ) * 15 + 64, ( i / 4 ) * 19 + 64, 15, 19 );	// minus numbers
				    this.stLag数値[ i + 12 ].rc = new Rectangle( ( i % 4 ) * 15     , ( i / 4 ) * 19     , 15, 19 );	// plus numbers
                }
			}

            this.stレーンサイズ = new STレーンサイズ[15];
                STレーンサイズ stレーンサイズ = new STレーンサイズ();
                //                                LC          HH          SD            BD          HT           LT           FT            CY          LP             RD
                int[,] sizeXW = new int[,] { { 290, 80 }, { 367, 46 }, { 470, 54 }, { 582, 60 }, { 528, 46 }, { 645, 46 }, { 694, 46 }, { 748, 64 }, { 419, 46 }, { 815, 80 }, { 815, 80 }, { 815, 80 }, { 815, 80 }, { 815, 80 }, { 815, 80 }, };
                int[,] sizeXW_B = new int[,] { { 290, 80 }, { 367, 46 }, { 419, 54 }, { 534, 60 }, { 590, 46 }, { 645, 46 }, { 694, 46 }, { 748, 64 }, { 478, 46 }, { 815, 64 }, { 815, 80 }, { 507, 80 }, { 815, 80 }, { 815, 80 }, { 815, 80 }, };
                int[,] sizeXW_C = new int[,] { { 290, 80 }, { 367, 46 }, { 470, 54 }, { 534, 60 }, { 590, 46 }, { 645, 46 }, { 694, 46 }, { 748, 64 }, { 419, 46 }, { 815, 64 }, { 815, 80 }, { 507, 80 }, { 815, 80 }, { 815, 80 }, { 815, 80 }, };
                int[,] sizeXW_D = new int[,] { { 290, 80 }, { 367, 46 }, { 419, 54 }, { 582, 60 }, { 476, 46 }, { 645, 46 }, { 694, 46 }, { 748, 64 }, { 528, 46 }, { 815, 64 }, { 815, 80 }, { 507, 80 }, { 815, 80 }, { 815, 80 }, { 815, 80 }, };

                for (int i = 0; i < 15; i++)
                {
                    this.stレーンサイズ[i] = new STレーンサイズ();
                    if( CDTXMania.ConfigIni.bDrums有効 )
                    {
                        this.stレーンサイズ[i] = default(CAct演奏Drums判定文字列.STレーンサイズ);
                        switch ( CDTXMania.ConfigIni.eLaneType.Drums )
                        {
                            case Eタイプ.A:
                                this.stレーンサイズ[i].x = sizeXW[i, 0];
                                this.stレーンサイズ[i].w = sizeXW[i, 1];
                                goto IL_19F;
                            case Eタイプ.B:
                                this.stレーンサイズ[i].x = sizeXW_B[i, 0];
                                this.stレーンサイズ[i].w = sizeXW_B[i, 1];
                                goto IL_19F;
                            case Eタイプ.C:
                                this.stレーンサイズ[i].x = sizeXW_C[i, 0];
                                this.stレーンサイズ[i].w = sizeXW_C[i, 1];
                                goto IL_19F;
                            case Eタイプ.D:
                                this.stレーンサイズ[i].x = sizeXW_D[i, 0];
                                this.stレーンサイズ[i].w = sizeXW_D[i, 1];
                                goto IL_19F;
                        }
                    IL_19F:
                        if (i == 7 && CDTXMania.ConfigIni.eRDPosition == ERDPosition.RDRC)
                        {
                            this.stレーンサイズ[i].x = sizeXW[9, 0] - 24;
                        }
                        if (i == 9 && CDTXMania.ConfigIni.eRDPosition == ERDPosition.RDRC)
                        {
                            this.stレーンサイズ[i].x = sizeXW[7, 0] - 18;
                        }
                    }
                }
			base.On活性化();
			this.nShowLagType = CDTXMania.ConfigIni.nShowLagType;
		}
		public override void On非活性化()
		{
			for( int i = 0; i < 15; i++ )
			{
				this.st状態[ i ].ct進行 = null;
			}
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
                if(CDTXMania.ConfigIni.nJudgeAnimeType == 1)
                {
                    //this.tx判定文字列[0] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_judge strings.png"));
                    //this.tx判定文字列[1] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_judge strings.png"));
                    //this.tx判定文字列[2] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_judge strings.png"));
                    //2013.8.2 kairera0467 CStage演奏画面共通側で読み込むテスト。
                }
                else if( CDTXMania.ConfigIni.nJudgeAnimeType == 2 )
                {
                    
                }
                else
                {
                    this.tx判定文字列[0] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\ScreenPlay judge strings 1.png"));
                    this.tx判定文字列[1] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\ScreenPlay judge strings 2.png"));
                    this.tx判定文字列[2] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\ScreenPlay judge strings 3.png"));
                }

                this.txlag数値 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_lag numbers.png"));
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				CDTXMania.tテクスチャの解放( ref this.tx判定文字列[ 0 ] );
				CDTXMania.tテクスチャの解放( ref this.tx判定文字列[ 1 ] );
				CDTXMania.tテクスチャの解放( ref this.tx判定文字列[ 2 ] );
				CDTXMania.tテクスチャの解放( ref this.txlag数値 );
				base.OnManagedリソースの解放();
			}
		}
	}
}