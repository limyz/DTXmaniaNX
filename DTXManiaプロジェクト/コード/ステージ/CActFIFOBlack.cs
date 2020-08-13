using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using FDK;

namespace DTXMania
{
	internal class CActFIFOBlack : CActivity
	{
		// メソッド

		public void tフェードアウト開始()
		{
			this.mode = EFIFOモード.フェードアウト;
			this.counter = new CCounter( 0, 100, 5, CDTXMania.Timer );
		}
		public void tフェードイン開始()
		{
			this.mode = EFIFOモード.フェードイン;
			this.counter = new CCounter( 0, 100, 5, CDTXMania.Timer );
		}
        public void tフェードイン完了()		// #25406 2011.6.9 yyagi
		{
			this.counter.n現在の値 = this.counter.n終了値;
		}

		
		// CActivity 実装

		public override void OnDeactivate()
		{
			if( !base.bNotActivated )
			{
				CDTXMania.tReleaseTexture( ref this.tx黒タイル64x64 );
				base.OnDeactivate();
			}
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
				this.tx黒タイル64x64 = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\Tile black 64x64.png" ), false );
				base.OnManagedCreateResources();
			}
		}
		public override int On進行描画()
		{
			if( base.bNotActivated || ( this.counter == null ) )
			{
				return 0;
			}
			this.counter.t進行();
			// Size clientSize = CDTXMania.app.Window.ClientSize;	// #23510 2010.10.31 yyagi: delete as of no one use this any longer.
			if (this.tx黒タイル64x64 != null)
			{
				this.tx黒タイル64x64.nTransparency = ( this.mode == EFIFOモード.フェードイン ) ? ( ( ( 100 - this.counter.n現在の値 ) * 0xff ) / 100 ) : ( ( this.counter.n現在の値 * 0xff ) / 100 );
				for (int i = 0; i <= (SampleFramework.GameWindowSize.Width / 64); i++)		// #23510 2010.10.31 yyagi: change "clientSize.Width" to "640" to fix FIFO drawing size
				{
					for (int j = 0; j <= (SampleFramework.GameWindowSize.Height / 64); j++)	// #23510 2010.10.31 yyagi: change "clientSize.Height" to "480" to fix FIFO drawing size
					{
						this.tx黒タイル64x64.tDraw2D( CDTXMania.app.Device, i * 64, j * 64 );
					}
				}
			}
			if( this.counter.n現在の値 != 100 )
			{
				return 0;
			}
			return 1;
		}


		// その他

		#region [ private ]
		//-----------------
		private CCounter counter;
		private EFIFOモード mode;
		private CTexture tx黒タイル64x64;
		//-----------------
		#endregion
	}
}
