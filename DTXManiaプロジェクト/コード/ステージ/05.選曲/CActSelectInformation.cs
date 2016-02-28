using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CActSelectInformation : CActivity
	{
		// コンストラクタ

		public CActSelectInformation()
		{
			base.b活性化してない = true;
		}


		// CActivity 実装

		public override void On活性化()
		{
			this.n画像Index上 = -1;
			this.n画像Index下 = 0;
			base.On活性化();
		}
		public override void On非活性化()
		{
			this.ctスクロール用 = null;
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				string[] infofiles = {		// #25381 2011.6.4 yyagi
				   @"Graphics\5_information.png" ,
				   @"Graphics\5_informatione.png"
				};
				int c = ( CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ja" ) ? 0 : 1; 
				this.txInfo = CDTXMania.tテクスチャの生成( CSkin.Path( infofiles[ c ] ), false );
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				CDTXMania.tテクスチャの解放( ref this.txInfo );
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
				if( base.b初めての進行描画 )
				{
					this.ctスクロール用 = new CCounter( 0, 6000, 1, CDTXMania.Timer );
					base.b初めての進行描画 = false;
				}
				this.ctスクロール用.t進行();
				if( this.ctスクロール用.b終了値に達した )
				{
					this.n画像Index上 = this.n画像Index下;
					this.n画像Index下 = ( this.n画像Index下 + 1 ) % stInfo.GetLength( 0 );		//8;
					this.ctスクロール用.n現在の値 = 0;
				}
				int n現在の値 = this.ctスクロール用.n現在の値;
				if( n現在の値 <= 250 )
				{
					double n現在の割合 = ( (double) n現在の値 ) / 250.0;
					if( this.n画像Index上 >= 0 )
					{
						STINFO stinfo = this.stInfo[ this.n画像Index上 ];
						Rectangle rectangle = new Rectangle( stinfo.pt左上座標.X, stinfo.pt左上座標.Y + ( (int) ( 42.0 * n現在の割合 ) ), 240, Convert.ToInt32(42.0 * (1.0 - n現在の割合)) );
						if( this.txInfo != null )
						{
							this.txInfo.t2D描画( CDTXMania.app.Device, 4, 0, rectangle );
						}
					}
					if( this.n画像Index下 >= 0 )
					{
						STINFO stinfo = this.stInfo[ this.n画像Index下 ];
						Rectangle rectangle = new Rectangle( stinfo.pt左上座標.X, stinfo.pt左上座標.Y, 240, (int) ( 42.0 * n現在の割合 ) );
						if( this.txInfo != null )
						{
							this.txInfo.t2D描画( CDTXMania.app.Device, 4, 0 + ( (int) ( 42.0 * ( 1.0 - n現在の割合 ) ) ), rectangle );
						}
					}
				}
				else
				{
					STINFO stinfo = this.stInfo[ this.n画像Index下 ];
					Rectangle rectangle = new Rectangle( stinfo.pt左上座標.X, stinfo.pt左上座標.Y, 240, 42 );
					if( this.txInfo != null )
					{
						this.txInfo.t2D描画( CDTXMania.app.Device, 4, 0, rectangle );
					}
				}
			}
			return 0;
		}


		// その他

		#region [ private ]
		//-----------------
		[StructLayout( LayoutKind.Sequential )]
		private struct STINFO
		{
			public Point pt左上座標;
			public STINFO( int x, int y )
			{
				this.pt左上座標 = new Point( x, y );
			}
		}

		private CCounter ctスクロール用;
		private int n画像Index下;
		private int n画像Index上;
        private readonly STINFO[] stInfo = new STINFO[] {
            new STINFO(0, 0 * 42),
            new STINFO(0, 1 * 42),
            new STINFO(0, 2 * 42),
            new STINFO(0, 3 * 42),
            new STINFO(0, 4 * 42),
            new STINFO(0, 5 * 42),
            new STINFO(0, 6 * 42),
            new STINFO(0, 7 * 42)
        };
        private CTexture txInfo;
		//-----------------
		#endregion
	}
}
