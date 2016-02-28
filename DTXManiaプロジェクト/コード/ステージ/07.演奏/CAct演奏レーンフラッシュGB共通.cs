using System;
using System.Collections.Generic;
using System.Text;
using FDK;

namespace DTXMania
{
	internal class CAct演奏レーンフラッシュGB共通 : CActivity
	{
		// プロパティ

		protected CCounter[] ct進行 = new CCounter[ 10 ];
		protected CTexture[] txFlush = new CTexture[ 10 ];


		// コンストラクタ

		public CAct演奏レーンフラッシュGB共通()
		{
			base.b活性化してない = true;
		}


		// メソッド

		public void Start( int nLane )
		{
			if( ( nLane < 0 ) || ( nLane > 10 ) )
			{
				throw new IndexOutOfRangeException( "有効範囲は 0～10 です。" );
			}
			this.ct進行[ nLane ] = new CCounter( 0, 70, 1, CDTXMania.Timer );
		}


		// CActivity 実装

		public override void On活性化()
		{
			for( int i = 0; i < 10; i++ )
			{
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
				this.txFlush[ 0 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlay lane flush red.png" ) );
				this.txFlush[ 1 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlay lane flush green.png" ) );
				this.txFlush[ 2 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlay lane flush blue.png" ) );
                this.txFlush[ 3 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlay lane flush yellow.png" ) );
				this.txFlush[ 4 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlay lane flush purple.png" ) );

				this.txFlush[ 5 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlay lane flush red reverse.png" ) );
				this.txFlush[ 6 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlay lane flush green reverse.png" ) );
				this.txFlush[ 7 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlay lane flush blue reverse.png" ) );
                this.txFlush[ 8 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlay lane flush yellow reverse.png" ) );
				this.txFlush[ 9 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlay lane flush purple reverse.png" ) );
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				for( int i = 0; i < 10; i++ )
				{
					CDTXMania.tテクスチャの解放( ref this.txFlush[ i ] );
				}
				base.OnManagedリソースの解放();
			}
		}
	}
}
