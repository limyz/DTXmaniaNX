using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CAct演奏GuitarレーンフラッシュGB : CAct演奏レーンフラッシュGB共通
	{
		// コンストラクタ

		public CAct演奏GuitarレーンフラッシュGB()
		{
			base.b活性化してない = true;
		}
        // 2013.02.22 kairera0467
        // ギターのレーンフラッシュの幅は37。

		// CActivity 実装（共通クラスからの差分のみ）

        public override void OnManagedリソースの作成()
        {
            this.txレーン = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Paret_Guitar.png"));
            this.txレーンダーク = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Paret_Guitar_Dark.png"));
            this.txレーンフラッシュ = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_guitar line.png"));

            base.OnManagedリソースの作成();
        }
        public override void OnManagedリソースの解放()
        {
            CDTXMania.tテクスチャの解放( ref this.txレーン );
            CDTXMania.tテクスチャの解放( ref this.txレーンダーク );
            CDTXMania.tテクスチャの解放( ref this.txレーンフラッシュ );
            base.OnManagedリソースの解放();
        }

		public override int On進行描画()
		{
			if( !base.b活性化してない )
            {
                #region[ レーンの描画 ]
                //---------------
                //レ－ンのみ先に描画しておく。
                if (CDTXMania.DTX.bチップがある.Guitar)
                {
                    if ( CDTXMania.ConfigIni.nLaneDisp.Guitar == 0 || CDTXMania.ConfigIni.nLaneDisp.Guitar == 2 )
                        this.txレーン.t2D描画(CDTXMania.app.Device, 67, 42);
                    else
                        this.txレーンダーク.t2D描画(CDTXMania.app.Device, 67, 42);
                }
                if (CDTXMania.DTX.bチップがある.Bass)
                {
                    if ( CDTXMania.ConfigIni.nLaneDisp.Bass == 0 || CDTXMania.ConfigIni.nLaneDisp.Bass == 2 )
                        this.txレーン.t2D描画(CDTXMania.app.Device, 937, 42);
                    else
                        this.txレーンダーク.t2D描画(CDTXMania.app.Device, 937, 42);
                }
                //---------------
                #endregion

                for ( int i = 0; i < 10; i++ )
				{
					if( !base.ct進行[ i ].b停止中 )
					{
						E楽器パート e楽器パート = ( i < 5 ) ? E楽器パート.GUITAR : E楽器パート.BASS;
						CTexture texture = CDTXMania.ConfigIni.bReverse[ (int) e楽器パート ] ? base.txFlush[ ( i % 5 ) + 5 ] : base.txFlush[ i % 5 ];
						int num2 = CDTXMania.ConfigIni.bLeft[ (int) e楽器パート ] ? 1 : 0;
						//int x = ( ( ( i < 5 ) ? 88 : 480 ) + this.nRGBのX座標[ num2, i ] ) + ( ( 37 * base.ct進行[ i ].n現在の値 ) / 100 );
                        int x = (((i < 5) ? 88 : 958) + this.nRGBのX座標[num2, i] + ( ( 19 * base.ct進行[ i ].n現在の値 ) / 70 ));
                        int x2 = ((i < 5) ? 88 : 954);
		                int y = CDTXMania.ConfigIni.bReverse[ (int) e楽器パート ] ? 414 : 100;
                        int y2 = CDTXMania.ConfigIni.bReverse[(int)e楽器パート] ? 414 : 104;
						if( texture != null && CDTXMania.ConfigIni.bLaneFlush[ (int) e楽器パート ] )
						{
                            texture.t2D描画( CDTXMania.app.Device, x, y, new Rectangle( 37, 0, ( 37 * ( 70 - base.ct進行[ i ].n現在の値)) / 70, 256 ) );
                            //if( j == 4 )
                                //this.txレーンフラッシュ.t2D描画( CDTXMania.app.Device, x2 + ( ( i < 5 ? i : i - 5 ) * 39 ), y2, new Rectangle( i * 39, 0, 41, 566 ) );
				        }
                        base.ct進行[ i ].t進行();
						if( base.ct進行[ i ].b終了値に達した )
						{
							base.ct進行[ i ].t停止();
						}
					}
				}
                //ここの分岐文はbase.ct進行[ n ]のものを使わないと、停止中にレーンフラッシュが消えてしまう。
                
                if ( !base.ct進行[ 0 ].b停止中 && CDTXMania.ConfigIni.bLaneFlush.Guitar )
                {
                    this.txレーンフラッシュ.t2D描画(CDTXMania.app.Device, ( CDTXMania.ConfigIni.bLeft.Guitar ? 242 : 86 ), 104, new Rectangle(0, 0, 41, 566));
                }
                if ( !base.ct進行[ 1 ].b停止中 && CDTXMania.ConfigIni.bLaneFlush.Guitar )
                {
                    this.txレーンフラッシュ.t2D描画(CDTXMania.app.Device, ( CDTXMania.ConfigIni.bLeft.Guitar ? 203 : 125 ), 104, new Rectangle(39, 0, 41, 566));
                }
                if ( !base.ct進行[ 2 ].b停止中 && CDTXMania.ConfigIni.bLaneFlush.Guitar )
                {
                    this.txレーンフラッシュ.t2D描画(CDTXMania.app.Device, 164, 104, new Rectangle(78, 0, 41, 566));
                }
                if ( !base.ct進行[ 3 ].b停止中 && CDTXMania.ConfigIni.bLaneFlush.Guitar )
                {
                    this.txレーンフラッシュ.t2D描画(CDTXMania.app.Device, ( CDTXMania.ConfigIni.bLeft.Guitar ? 125 : 203 ), 104, new Rectangle(117, 0, 41, 566));
                }
                if ( !base.ct進行[ 4 ].b停止中 && CDTXMania.ConfigIni.bLaneFlush.Guitar )
                {
                    this.txレーンフラッシュ.t2D描画(CDTXMania.app.Device, ( CDTXMania.ConfigIni.bLeft.Guitar ? 86 : 242 ), 104, new Rectangle(156, 0, 41, 566));
                }

                if( !base.ct進行[ 5 ].b停止中 && CDTXMania.ConfigIni.bLaneFlush.Bass )
                {
                    this.txレーンフラッシュ.t2D描画(CDTXMania.app.Device, ( CDTXMania.ConfigIni.bLeft.Bass ? 1112 : 957 ), 104, new Rectangle(0, 0, 41, 566));
                }
                if( !base.ct進行[ 6 ].b停止中 && CDTXMania.ConfigIni.bLaneFlush.Bass )
                {
                    this.txレーンフラッシュ.t2D描画(CDTXMania.app.Device, ( CDTXMania.ConfigIni.bLeft.Bass ? 1073 : 995 ), 104, new Rectangle(39, 0, 41, 566));
                }
                if( !base.ct進行[ 7 ].b停止中 && CDTXMania.ConfigIni.bLaneFlush.Bass )
                {
                    this.txレーンフラッシュ.t2D描画(CDTXMania.app.Device, 1034, 104, new Rectangle(78, 0, 41, 566));
                }
                if( !base.ct進行[ 8 ].b停止中 && CDTXMania.ConfigIni.bLaneFlush.Bass )
                {
                    this.txレーンフラッシュ.t2D描画(CDTXMania.app.Device, ( CDTXMania.ConfigIni.bLeft.Bass ? 995 : 1073 ), 104, new Rectangle(117, 0, 41, 566));
                }
                if( !base.ct進行[ 9 ].b停止中 && CDTXMania.ConfigIni.bLaneFlush.Bass )
                {
                    this.txレーンフラッシュ.t2D描画(CDTXMania.app.Device, ( CDTXMania.ConfigIni.bLeft.Bass ? 957 : 1112 ), 104, new Rectangle(156, 0, 41, 566));
                }

			}
			return 0;
		}
		

		// その他

		#region [ private ]
		//-----------------
		private readonly int[,] nRGBのX座標 = new int[ , ] { { 0, 39, 78, 117, 156, 0, 39, 78, 117, 156 }, { 156, 117, 78, 39, 0, 156, 117, 78, 39, 0 } };

        private CTexture txレーン;
        private CTexture txレーンダーク;
        private CTexture txレーンフラッシュ;
		//-----------------
		#endregion
	}
}
