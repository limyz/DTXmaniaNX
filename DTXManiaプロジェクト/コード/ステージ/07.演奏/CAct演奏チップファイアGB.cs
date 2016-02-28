using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SlimDX;
using FDK;

namespace DTXMania
{
	internal abstract class CAct演奏チップファイアGB : CActivity
	{
		// コンストラクタ

		public CAct演奏チップファイアGB()
		{
			base.b活性化してない = true;
		}


		// メソッド

		public virtual void Start( int nLane, int n中央X, int n中央Y )
		{
			if( ( nLane >= 0 ) || ( nLane <= 5 ) )
			{
				this.pt中央位置[ nLane ].X = n中央X;
				this.pt中央位置[ nLane ].Y = n中央Y;
				this.ct進行[ nLane ].t開始( 28, 56, 8, CDTXMania.Timer );		// #24736 2011.2.17 yyagi: (0, 0x38, 4,..) -> (24, 0x38, 8) に変更 ギターチップの光り始めを早くするため
			}
		}

		public abstract void Start( int nLane );

		// CActivity 実装

		public override void On活性化()
		{
			for( int i = 0; i < 10; i++ )
			{
				this.pt中央位置[ i ] = new Point( 0, 0 );
				this.ct進行[ i ] = new CCounter();
			}
			base.On活性化();
		}
		public override void On非活性化()
		{
			for( int i = 0; i < 10; i++ )
			{
				this.ct進行[ i ] = null;
			}
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				this.tx火花[ 0 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlay chip fire red.png" ) );
				if( this.tx火花[ 0 ] != null )
				{
					this.tx火花[ 0 ].b加算合成 = true;
				}
				this.tx火花[ 1 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlay chip fire green.png" ) );
				if( this.tx火花[ 1 ] != null )
				{
					this.tx火花[ 1 ].b加算合成 = true;
				}
				this.tx火花[ 2 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlay chip fire blue.png" ) );
				if( this.tx火花[ 2 ] != null )
				{
					this.tx火花[ 2 ].b加算合成 = true;
				}
                this.tx火花[ 3 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlay chip fire yellow.png" ) );
				if( this.tx火花[ 3 ] != null )
				{
					this.tx火花[ 3 ].b加算合成 = true;
				}
                this.tx火花[ 4 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlay chip fire purple.png" ) );
				if( this.tx火花[ 4 ] != null )
				{
					this.tx火花[ 4 ].b加算合成 = true;
				}
                this.txレーンの線 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_guitar line.png"));
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				CDTXMania.tテクスチャの解放( ref this.tx火花[ 0 ] );
				CDTXMania.tテクスチャの解放( ref this.tx火花[ 1 ] );
				CDTXMania.tテクスチャの解放( ref this.tx火花[ 2 ] );
                CDTXMania.tテクスチャの解放( ref this.tx火花[ 3 ] );
                CDTXMania.tテクスチャの解放( ref this.tx火花[ 4 ] );
                CDTXMania.tテクスチャの解放( ref this.txレーンの線 );
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
				for( int i = 0; i < 10; i++ )
				{
					this.ct進行[ i ].t進行();
					if( this.ct進行[ i ].b終了値に達した )
					{
						this.ct進行[ i ].t停止();
					}
				}
				for( int j = 0; j < 10; j++ )
				{
					if( ( this.ct進行[ j ].n現在の経過時間ms != -1 ) && ( this.tx火花[ j % 5 ] != null ) )
					{
						float scale = (float) ( 3.0 * Math.Cos( ( Math.PI * ( 90.0 - ( 90.0 * ( ( (double) this.ct進行[ j ].n現在の値 ) / 56.0 ) ) ) ) / 180.0 ) );
						int x = this.pt中央位置[ j ].X - ( (int) ( ( this.tx火花[ j % 3 ].sz画像サイズ.Width * scale ) / 2f ) );
						int y = this.pt中央位置[ j ].Y - ( (int) ( ( this.tx火花[ j % 3 ].sz画像サイズ.Height * scale ) / 2f ) );
						this.tx火花[ j % 5 ].n透明度 = ( this.ct進行[ j ].n現在の値 < 0x1c ) ? 0xff : ( 0xff - ( (int) ( 255.0 * Math.Cos( ( Math.PI * ( 90.0 - ( 90.0 * ( ( (double) ( this.ct進行[ j ].n現在の値 - 0x1c ) ) / 28.0 ) ) ) ) / 180.0 ) ) ) );
						this.tx火花[ j % 5 ].vc拡大縮小倍率 = new Vector3( scale, scale, 1f );
						this.tx火花[ j % 5 ].t2D描画( CDTXMania.app.Device, x, y );
					}
				}
			}
			return 0;
		}


		// その他

		#region [ private ]
		//-----------------
		private CCounter[] ct進行 = new CCounter[ 10 ];
		private Point[] pt中央位置 = new Point[ 10 ];
		private CTexture[] tx火花 = new CTexture[ 5 ];
        private CTexture txレーンの線;
		//-----------------
		#endregion
	}
}
