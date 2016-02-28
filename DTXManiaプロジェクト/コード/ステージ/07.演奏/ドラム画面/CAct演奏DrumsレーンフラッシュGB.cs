using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CAct演奏DrumsレーンフラッシュGB : CAct演奏レーンフラッシュGB共通
	{
		// CActivity 実装（共通クラスからの差分のみ）

		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
				for( int i = 0; i < 6; i++ )
				{
					if( !base.ct進行[ i ].b停止中 )
					{
						E楽器パート e楽器パート = ( i < 3 ) ? E楽器パート.GUITAR : E楽器パート.BASS;
						CTexture texture = CDTXMania.ConfigIni.bReverse[ (int) e楽器パート ] ? base.txFlush[ ( i % 3 ) + 3 ] : base.txFlush[ i % 3 ];
						int num2 = CDTXMania.ConfigIni.bLeft[ (int) e楽器パート ] ? 1 : 0;
						for( int j = 0; j < 3; j++ )
						{
							int x = ( ( ( i < 3 ) ? 0x1fb : 0x18e ) + this.nRGBのX座標[ num2, i ] ) + ( ( 0x10 * base.ct進行[ i ].n現在の値 ) / 100 );
							int y = ( ( i < 3 ) ? 0x39 : 0x39 ) + ( j * 0x76 );
							if( texture != null )
							{
								texture.t2D描画( CDTXMania.app.Device, x, y, new Rectangle( j * 0x20, 0, ( 0x18 * ( 100 - base.ct進行[ i ].n現在の値 ) ) / 100, 0x76 ) );
							}
						}
						base.ct進行[ i ].t進行();
						if( base.ct進行[ i ].b終了値に達した )
						{
							base.ct進行[ i ].t停止();
						}
					}
				}
			}
			return 0;
		}


		// その他

		#region [ private ]
		//-----------------
		private readonly int[,] nRGBのX座標 = new int[ , ] { { 2, 0x1c, 0x36, 2, 0x1c, 0x36 }, { 0x36, 0x1c, 2, 0x36, 0x1c, 2 } };
		//-----------------
		#endregion
	}
}
