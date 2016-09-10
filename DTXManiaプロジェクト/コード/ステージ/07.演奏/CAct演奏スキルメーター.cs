using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using SlimDX;
using FDK;

namespace DTXMania
{
    internal class CAct演奏スキルメーター : CActivity
    {
        // グラフ仕様
        // ・ギターとベースで同時にグラフを出すことはない。
        //
        // ・目標のメーター画像
        //   →ゴーストがあった
        // 　　・ゴーストに基づいたグラフ(リアルタイム比較)
        // 　→なかった
        // 　　・ScoreIniの自己ベストのグラフ
        //

		private STDGBVALUE<int> nGraphBG_XPos = new STDGBVALUE<int>(); //ドラムにも座標指定があるためDGBVALUEとして扱う。
		private int nGraphBG_YPos = 200;
		private int DispHeight = 400;
		private int DispWidth = 60;
		private CCounter counterYposInImg = null;
		private readonly int slices = 10;
        private int nGraphUsePart = 0;
        private int[] nGraphGauge_XPos = new int[ 2 ];
        private int nPart = 0;

		// プロパティ

        public double dbグラフ値現在_渡
        {
            get
            {
                return this.dbグラフ値現在;
            }
            set
            {
                this.dbグラフ値現在 = value;
            }
        }
        public double dbグラフ値目標_渡
        {
            get
            {
                return this.dbグラフ値目標;
            }
            set
            {
                this.dbグラフ値目標 = value;
            }
        }
        public int[] n現在のAutoを含まない判定数_渡
        {
            get
            {
                return this.n現在のAutoを含まない判定数;
            }
            set
            {
                this.n現在のAutoを含まない判定数 = value;
            }
        }
		
		// コンストラクタ

		public CAct演奏スキルメーター()
		{
			base.b活性化してない = true;
		}


		// CActivity 実装

		public override void On活性化()
        {
            this.dbグラフ値目標 = 0f;
            this.dbグラフ値現在 = 0f;

            this.n現在のAutoを含まない判定数 = new int[ 6 ];

			base.On活性化();
		}
		public override void On非活性化()
		{
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
                //this.pfNameFont = new CPrivateFastFont( new FontFamily( "Arial" ), 16, FontStyle.Bold );
                this.txグラフ = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Graph_Main.png" ) );
                this.txグラフ_ゲージ = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Graph_Gauge.png" ) );

                //if( this.pfNameFont != null )
                //{
                //    if( CDTXMania.ConfigIni.eTargetGhost.Drums == ETargetGhostData.PERFECT )
                //    {
                //        this.txPlayerName = this.t指定された文字テクスチャを生成する( "DJ AUTO" );
                //    }
                //    else if( CDTXMania.ConfigIni.eTargetGhost.Drums == ETargetGhostData.LAST_PLAY )
                //    {
                //        this.txPlayerName = this.t指定された文字テクスチャを生成する( "LAST PLAY" );
                //    }
                //}
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				CDTXMania.tテクスチャの解放( ref this.txグラフ );
                CDTXMania.tテクスチャの解放( ref this.txグラフ_ゲージ );
                CDTXMania.tテクスチャの解放( ref this.txグラフ値自己ベストライン );
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
				if( base.b初めての進行描画 )
				{
                    //座標などの定義は初回だけにする。
                    //2016.03.29 kairera0467 非セッション譜面で、譜面が無いパートでグラフを有効にしている場合、譜面があるパートに一時的にグラフを切り替える。
                    //                       時間がなくて雑なコードになったため、後日最適化を行う。
                    if( CDTXMania.ConfigIni.bDrums有効 )
                    {
                        this.nPart = 0;
                        this.nGraphUsePart = 0;
                    }
                    else if( CDTXMania.ConfigIni.bGuitar有効 )
                    {
                        this.nGraphUsePart = ( CDTXMania.ConfigIni.bGraph有効.Guitar == true ) ? 1 : 2;
                        if( CDTXMania.DTX.bチップがある.Guitar )
                            this.nPart = CDTXMania.ConfigIni.bGraph有効.Guitar ? 0 : 1;
                        else if( !CDTXMania.DTX.bチップがある.Guitar && CDTXMania.ConfigIni.bGraph有効.Guitar )
                        {
                            this.nPart = 1;
                            this.nGraphUsePart = 2;
                        }

                        if( !CDTXMania.DTX.bチップがある.Bass && CDTXMania.ConfigIni.bGraph有効.Bass )
                            this.nPart = 0;
                    }

                    this.nGraphBG_XPos.Drums = 870;
                    this.nGraphBG_XPos.Guitar = 356;
                    this.nGraphBG_XPos.Bass = 647;
                    this.nGraphBG_YPos = this.nGraphUsePart == 0 ? 50 : 110;
                    //2016.06.24 kairera0467 StatusPanelとSkillMaterの場合はX座標を調整する。
                    if( CDTXMania.ConfigIni.nInfoType == 1 )
                    {
                        this.nGraphBG_XPos.Guitar = 629 + 9;
                        this.nGraphBG_XPos.Bass = 403;
                    }

                    if( CDTXMania.ConfigIni.eTargetGhost[ this.nGraphUsePart ] != ETargetGhostData.NONE )
                    {
                        if( CDTXMania.listTargetGhostScoreData[ this.nGraphUsePart ] != null )
                        {
                            //this.dbグラフ値目標 = CDTXMania.listTargetGhostScoreData[ this.nGraphUsePart ].db演奏型スキル値;
                            this.dbグラフ値目標_表示 = CDTXMania.listTargetGhostScoreData[ this.nGraphUsePart ].db演奏型スキル値;
                        }
                    }

                    this.nGraphGauge_XPos = new int[] { 3, 205 };

					base.b初めての進行描画 = false;
                }

				int stYposInImg = 0;



                if( this.txグラフ != null )
                {
                    //背景
					this.txグラフ.vc拡大縮小倍率 = new Vector3( 1f, 1f, 1f );
                    this.txグラフ.t2D描画( CDTXMania.app.Device, nGraphBG_XPos[ this.nGraphUsePart ], nGraphBG_YPos, new Rectangle( 2, 2, 251, 584 ) );
                    
                    //自己ベスト数値表示
                    this.t達成率文字表示( nGraphBG_XPos[ this.nGraphUsePart ] + 136, nGraphBG_YPos + 501, string.Format( "{0,6:##0.00}" + "%", this.dbグラフ値自己ベスト ) );
                }

                //ゲージ現在
				if( this.txグラフ_ゲージ != null )
                {
                    //ゲージ本体
                    int nGaugeSize = (int)( 434.0f * (float)this.dbグラフ値現在 / 100.0f );
                    int nPosY = this.nGraphUsePart == 0 ? 527 - nGaugeSize : 587 - nGaugeSize;
                    this.txグラフ_ゲージ.n透明度 = 255;
                    this.txグラフ_ゲージ.t2D描画( CDTXMania.app.Device, nGraphBG_XPos[ this.nGraphUsePart ] + 45, nPosY, new Rectangle( 2, 2, 30, nGaugeSize ) );
                    
                    //ゲージ比較
                    int nTargetGaugeSize = (int)( 434.0f * ( (float)this.dbグラフ値目標 / 100.0f ) );
                    int nTargetGaugePosY = this.nGraphUsePart == 0 ? 527 - nTargetGaugeSize : 587 - nTargetGaugeSize;
                    int nTargetGaugeRectX = this.dbグラフ値現在 > this.dbグラフ値目標 ? 38 : 74;
                    this.txグラフ_ゲージ.n透明度 = 255;
                    this.txグラフ_ゲージ.t2D描画( CDTXMania.app.Device, nGraphBG_XPos[ this.nGraphUsePart ] + 75, nTargetGaugePosY, new Rectangle( nTargetGaugeRectX, 2, 30, nTargetGaugeSize ) );
                    if( this.txグラフ != null )
                    {
                        //ターゲット達成率数値

                        //ターゲット名
                        //現在
                        this.txグラフ.t2D描画( CDTXMania.app.Device, nGraphBG_XPos[ this.nGraphUsePart ] + 45, nGraphBG_YPos + 357, new Rectangle( 260, 2, 30, 120 ) );
                        //比較対象
                        this.txグラフ.t2D描画( CDTXMania.app.Device, nGraphBG_XPos[ this.nGraphUsePart ] + 75, nGraphBG_YPos + 357, new Rectangle( 260 + ( 30 * ( (int)CDTXMania.ConfigIni.eTargetGhost[ this.nGraphUsePart ] ) ), 2, 30, 120 ) );

                        //以下使用予定
                        //最終プレイ
                        this.txグラフ.t2D描画( CDTXMania.app.Device, nGraphBG_XPos[ this.nGraphUsePart ] + 106, nGraphBG_YPos + 357, new Rectangle( 260 + 60, 2, 30, 120 ) );
                        //自己ベスト
                        this.txグラフ.t2D描画( CDTXMania.app.Device, nGraphBG_XPos[ this.nGraphUsePart ] + 136, nGraphBG_YPos + 357, new Rectangle( 260 + 90, 2, 30, 120 ) );
                        //最高スコア
                        this.txグラフ.t2D描画( CDTXMania.app.Device, nGraphBG_XPos[ this.nGraphUsePart ] + 164, nGraphBG_YPos + 357, new Rectangle( 260 + 120, 2, 30, 120 ) );
                    }
                    this.t比較文字表示( nGraphBG_XPos[ this.nGraphUsePart ] + 44, nPosY - 10, string.Format( "{0,5:##0.00}", Math.Abs( this.dbグラフ値現在 ) ) );
                    this.t比較文字表示( nGraphBG_XPos[ this.nGraphUsePart ] + 74, nTargetGaugePosY - 10, string.Format( "{0,5:##0.00}", Math.Abs( this.dbグラフ値目標 ) ) );
                }


			}
			return 0;
		}


		// その他

		#region [ private ]
		//----------------
        private double dbグラフ値目標;
        private double dbグラフ値目標_表示;
        private double dbグラフ値現在;
        private double dbグラフ値現在_表示;
        public double dbグラフ値自己ベスト;
        private int[] n現在のAutoを含まない判定数;

        private CTexture txPlayerName;
		private CTexture txグラフ;
        private CTexture txグラフ_ゲージ;
        private CTexture txグラフ値自己ベストライン;

        private CPrivateFastFont pfNameFont;

        [StructLayout(LayoutKind.Sequential)]
        private struct ST文字位置
        {
            public char ch;
            public Point pt;
            public ST文字位置( char ch, Point pt )
            {
                this.ch = ch;
                this.pt = pt;
            }
        }

        private ST文字位置[] st比較数字位置 = new ST文字位置[]{
            new ST文字位置( '0', new Point( 0, 0 ) ),
            new ST文字位置( '1', new Point( 8, 0 ) ),
            new ST文字位置( '2', new Point( 16, 0 ) ),
            new ST文字位置( '3', new Point( 24, 0 ) ),
            new ST文字位置( '4', new Point( 32, 0 ) ),
            new ST文字位置( '5', new Point( 40, 0 ) ),
            new ST文字位置( '6', new Point( 48, 0 ) ),
            new ST文字位置( '7', new Point( 56, 0 ) ),
            new ST文字位置( '8', new Point( 64, 0 ) ),
            new ST文字位置( '9', new Point( 72, 0 ) ),
            new ST文字位置( '.', new Point( 80, 0 ) )
        };
        private ST文字位置[] st達成率数字位置 = new ST文字位置[]{
            new ST文字位置( '0', new Point( 0, 0 ) ),
            new ST文字位置( '1', new Point( 16, 0 ) ),
            new ST文字位置( '2', new Point( 32, 0 ) ),
            new ST文字位置( '3', new Point( 48, 0 ) ),
            new ST文字位置( '4', new Point( 64, 0 ) ),
            new ST文字位置( '5', new Point( 80, 0 ) ),
            new ST文字位置( '6', new Point( 96, 0 ) ),
            new ST文字位置( '7', new Point( 112, 0 ) ),
            new ST文字位置( '8', new Point( 128, 0 ) ),
            new ST文字位置( '9', new Point( 144, 0 ) ),
            new ST文字位置( '.', new Point( 160, 0 ) ),
            new ST文字位置( '%', new Point( 168, 0 ) ),
        };


		private void t比較文字表示( int x, int y, string str )
		{
			foreach( char ch in str )
			{
				for( int i = 0; i < this.st比較数字位置.Length; i++ )
				{
					if( this.st比較数字位置[ i ].ch == ch )
					{
                        int RectX = 8;
                        if( ch == '.' ) RectX = 2;
						Rectangle rectangle = new Rectangle( 260 + this.st比較数字位置[ i ].pt.X, 162, RectX, 10 );
						if( this.txグラフ != null )
						{
                            this.txグラフ.n透明度 = 255;
							this.txグラフ.t2D描画( CDTXMania.app.Device, x, y, rectangle );
						}
						break;
					}
				}
                if( ch == '.' ) x += 2;
                else x += 7;
			}
		}
		private void t達成率文字表示( int x, int y, string str )
		{
			foreach( char ch in str )
			{
				for( int i = 0; i < this.st達成率数字位置.Length; i++ )
				{
					if( this.st達成率数字位置[ i ].ch == ch )
					{
                        int RectX = 16;
                        if( ch == '.' ) RectX = 8;
						Rectangle rectangle = new Rectangle( 260 + this.st達成率数字位置[ i ].pt.X, 128, RectX, 28 );
						if( this.txグラフ != null )
						{
                            this.txグラフ.n透明度 = 255;
							this.txグラフ.t2D描画( CDTXMania.app.Device, x, y, rectangle );
						}
						break;
					}
				}
                if( ch == '.' ) x += 8;
				else x += 16;
			}
		}
        private CTexture t指定された文字テクスチャを生成する( string str文字 )
        {
            Bitmap bmp;
            bmp = this.pfNameFont.DrawPrivateFont( str文字, Color.White, Color.Transparent );

            CTexture tx文字テクスチャ = CDTXMania.tテクスチャの生成( bmp, false );

            if( tx文字テクスチャ != null )
                tx文字テクスチャ.vc拡大縮小倍率 = new Vector3( 1.0f, 1.0f, 1f );

            bmp.Dispose();

            return tx文字テクスチャ;
        }
        private void t折れ線を描画する( int nBoardPosA, int nBoardPosB )
        {
            //やる気がまるでない線
            //2016.03.28 kairera0467 ギター画面では1Pと2Pで向きが変わるが、そこは残念ながら未対応。
            //参考 http://dobon.net/vb/dotnet/graphics/drawline.html
            if( this.txグラフ値自己ベストライン == null )
            {
                Bitmap canvas = new Bitmap( 280, 720 );

                Graphics g = Graphics.FromImage( canvas );
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                int nMybestGaugeSize = (int)( 560.0f * (float)this.dbグラフ値自己ベスト / 100.0f );
                int nMybestGaugePosY = 600 - nMybestGaugeSize;

                int nTargetGaugeSize = (int)( 560.0f * (float)this.dbグラフ値目標_表示 / 100.0f );
                int nTargetGaugePosY = 600 - nTargetGaugeSize;

                Point[] posMybest = {
                    new Point( 3, nMybestGaugePosY ),
                    new Point( 75, nMybestGaugePosY ),
                    new Point( 94, nBoardPosA + 31 ),
                    new Point( 102, nBoardPosA + 31 )
                };

                Point[] posTarget = {
                    new Point( 3, nTargetGaugePosY ),
                    new Point( 75, nTargetGaugePosY ),
                    new Point( 94, nBoardPosB + 59 ),
                    new Point( 102, nBoardPosB + 59 )
                };

                if( this.nGraphUsePart == 2 )
                {
                    posMybest = new Point[]{
                        new Point( 271, nMybestGaugePosY ),
                        new Point( 206, nMybestGaugePosY ),
                        new Point( 187, nBoardPosA + 31 ),
                        new Point( 178, nBoardPosA + 31 )
                    };

                    posTarget = new Point[]{
                        new Point( 271, nTargetGaugePosY ),
                        new Point( 206, nTargetGaugePosY ),
                        new Point( 187, nBoardPosB + 59 ),
                        new Point( 178, nBoardPosB + 59 )
                    };
                }

                Pen penMybest = new Pen( Color.Pink, 2 );
                g.DrawLines( penMybest, posMybest );

                if( CDTXMania.listTargetGhsotLag[ this.nGraphUsePart ] != null && CDTXMania.listTargetGhostScoreData[ this.nGraphUsePart ] != null )
                {
                    Pen penTarget = new Pen( Color.Orange, 2 );
                    g.DrawLines( penTarget, posTarget );
                }

                g.Dispose();

                this.txグラフ値自己ベストライン = new CTexture( CDTXMania.app.Device, canvas, CDTXMania.TextureFormat, false );
            }
            if( this.txグラフ値自己ベストライン != null )
                this.txグラフ値自己ベストライン.t2D描画( CDTXMania.app.Device, nGraphBG_XPos[ this.nGraphUsePart ], nGraphBG_YPos );
        }
		//-----------------
		#endregion
	}
}
