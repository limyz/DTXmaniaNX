using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using FDK;

namespace DTXMania
{
	internal class CActFIFOBlackStart : CActivity
	{
		// メソッド

		public void tStartFadeOut()
		{
			this.mode = EFIFOMode.FadeOut;  // tフェードアウト開始
			this.counter = new CCounter( 0, 150, 5, CDTXMania.Timer );
		}
		public void tStartFadeIn()  // tフェードイン開始
		{
			this.mode = EFIFOMode.FadeIn;
			this.counter = new CCounter( 0, 150, 5, CDTXMania.Timer );
		}

		
		// CActivity 実装

		public override void OnDeactivate()
		{
			if( !base.bNotActivated )
			{
				CDTXMania.tReleaseTexture( ref this.tx黒タイル64x64 );
                CDTXMania.tReleaseTexture( ref this.tx黒幕 );
                CDTXMania.tReleaseTexture( ref this.txジャケット );
				base.OnDeactivate();
			}
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
				this.tx黒タイル64x64 = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\Tile black 64x64.png" ), false );
                this.tx黒幕 = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\6_FadeOut.jpg"), false);
				base.OnManagedCreateResources();
			}
		}
		public override int OnUpdateAndDraw()
		{
			if( base.bNotActivated || ( this.counter == null ) )
			{
				return 0;
			}
			this.counter.tUpdate();
			// Size clientSize = CDTXMania.app.Window.ClientSize;	// #23510 2010.10.31 yyagi: delete as of no one use this any longer.
			if (this.tx黒幕 != null)
			{
                this.tx黒幕.nTransparency = (this.mode == EFIFOMode.FadeIn) ? (((100 - this.counter.nCurrentValue) * 0xff) / 100) : ((this.counter.nCurrentValue * 0xff) / 100);
                this.tx黒幕.tDraw2D(CDTXMania.app.Device, 0, 0);
                string path = CDTXMania.DTX.strFolderName + CDTXMania.DTX.PREIMAGE;
                if( this.txジャケット == null ) // 2019.04.26 kairera0467
                {
                    if (!File.Exists(path))
                    {
                        //Trace.TraceWarning("ファイルが存在しません。({0})", new object[] { path });
                        this.txジャケット = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\\5_preimage default.png"));
                    }
                    else
                    {
                        this.txジャケット = CDTXMania.tGenerateTexture(path);
                    }
                }

                if( this.txジャケット != null )
                {
                    this.txジャケット.vcScaleRatio.X = 0.96f;
                    this.txジャケット.vcScaleRatio.Y = 0.96f;
                    this.txジャケット.fZAxisRotation = 0.28f;
                    this.txジャケット.nTransparency = (this.mode == EFIFOMode.FadeIn) ? (((100 - this.counter.nCurrentValue) * 0xff) / 100) : ((this.counter.nCurrentValue * 0xff) / 100);
                    this.txジャケット.tDraw2D(CDTXMania.app.Device, 620, 40);
                }
			}
            else if (this.tx黒幕 == null)
            {
                this.tx黒タイル64x64.nTransparency = (this.mode == EFIFOMode.FadeIn) ? (((100 - this.counter.nCurrentValue) * 0xff) / 100) : ((this.counter.nCurrentValue * 0xff) / 100);
                for (int i = 0; i <= (SampleFramework.GameWindowSize.Width / 64); i++)		// #23510 2010.10.31 yyagi: change "clientSize.Width" to "640" to fix FIFO drawing size
                {
                    for (int j = 0; j <= (SampleFramework.GameWindowSize.Height / 64); j++)	// #23510 2010.10.31 yyagi: change "clientSize.Height" to "480" to fix FIFO drawing size
                    {
                        this.tx黒タイル64x64.tDraw2D(CDTXMania.app.Device, i * 64, j * 64);
                    }
                }
            }
			if( this.counter.nCurrentValue != 150 )
			{
				return 0;
			}
			return 1;
		}


		// Other

		#region [ private ]
		//-----------------
		public CCounter counter;
		private EFIFOMode mode;
		private CTexture tx黒タイル64x64;
        private CTexture tx黒幕;
        private CTexture txジャケット;
		//-----------------
		#endregion
	}
}
