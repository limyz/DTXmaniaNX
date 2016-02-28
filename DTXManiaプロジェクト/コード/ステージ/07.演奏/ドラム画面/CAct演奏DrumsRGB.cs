using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DTXMania
{
	internal class CAct演奏DrumsRGB : CAct演奏RGB共通
	{
		// CActivity 実装（共通クラスからの差分のみ）

		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
				if( !CDTXMania.ConfigIni.bGuitar有効 )
				{
					return 0;
				}
				if( CDTXMania.DTX.bチップがある.Guitar )
				{
					for( int j = 0; j < 3; j++ )
					{
						int index = CDTXMania.ConfigIni.bLeft.Guitar ? ( 2 - j ) : j;
						Rectangle rectangle = new Rectangle( index * 0x18, 0, 0x18, 0x20 );
						if( base.b押下状態[ index ] )
						{
							rectangle.Y += 0x20;
						}
						if( base.txRGB != null )
						{
							base.txRGB.t2D描画( CDTXMania.app.Device, 0x1fd + ( j * 0x1a ), 0x39, rectangle );
						}
					}
				}
				if( CDTXMania.DTX.bチップがある.Bass )
				{
					for( int k = 0; k < 3; k++ )
					{
						int num4 = CDTXMania.ConfigIni.bLeft.Bass ? ( 2 - k ) : k;
						Rectangle rectangle2 = new Rectangle( num4 * 0x18, 0, 0x18, 0x20 );
						if( base.b押下状態[ num4 + 3 ] )
						{
							rectangle2.Y += 0x20;
						}
						if( base.txRGB != null )
						{
							base.txRGB.t2D描画( CDTXMania.app.Device, 400 + ( k * 0x1a ), 0x39, rectangle2 );
						}
					}
				}
				for( int i = 0; i < 6; i++ )
				{
					base.b押下状態[ i ] = false;
				}
			}
			return 0;
		}
	}
}
