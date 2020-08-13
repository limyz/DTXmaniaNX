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
			base.bNotActivated = true;
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

		public override void OnActivate()
		{
			for( int i = 0; i < 10; i++ )
			{
				this.ct進行[ i ] = new CCounter();
			}
			base.OnActivate();
		}
		public override void OnDeactivate()
		{
			for( int i = 0; i < 10; i++ )
			{
				this.ct進行[ i ] = null;
			}
			base.OnDeactivate();
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
				this.txFlush[ 0 ] = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlay lane flush red.png" ) );
				this.txFlush[ 1 ] = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlay lane flush green.png" ) );
				this.txFlush[ 2 ] = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlay lane flush blue.png" ) );
                this.txFlush[ 3 ] = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlay lane flush yellow.png" ) );
				this.txFlush[ 4 ] = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlay lane flush purple.png" ) );

				this.txFlush[ 5 ] = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlay lane flush red reverse.png" ) );
				this.txFlush[ 6 ] = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlay lane flush green reverse.png" ) );
				this.txFlush[ 7 ] = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlay lane flush blue reverse.png" ) );
                this.txFlush[ 8 ] = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlay lane flush yellow reverse.png" ) );
				this.txFlush[ 9 ] = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlay lane flush purple reverse.png" ) );
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
				for( int i = 0; i < 10; i++ )
				{
					CDTXMania.tReleaseTexture( ref this.txFlush[ i ] );
				}
				base.OnManagedReleaseResources();
			}
		}
	}
}
