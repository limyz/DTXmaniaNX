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

		public void tフェードアウト開始()
		{
			this.mode = EFIFOモード.フェードアウト;
			this.counter = new CCounter( 0, 150, 5, CDTXMania.Timer );
		}
		public void tフェードイン開始()
		{
			this.mode = EFIFOモード.フェードイン;
			this.counter = new CCounter( 0, 150, 5, CDTXMania.Timer );
		}

		
		// CActivity 実装

		public override void On非活性化()
		{
			if( !base.b活性化してない )
			{
				CDTXMania.tテクスチャの解放( ref this.tx黒タイル64x64 );
                CDTXMania.tテクスチャの解放( ref this.tx黒幕 );
                CDTXMania.tテクスチャの解放( ref this.txジャケット );
				base.On非活性化();
			}
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				this.tx黒タイル64x64 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Tile black 64x64.png" ), false );
                this.tx黒幕 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\6_FadeOut.jpg"), false);
				base.OnManagedリソースの作成();
			}
		}
		public override int On進行描画()
		{
			if( base.b活性化してない || ( this.counter == null ) )
			{
				return 0;
			}
			this.counter.t進行();
			// Size clientSize = CDTXMania.app.Window.ClientSize;	// #23510 2010.10.31 yyagi: delete as of no one use this any longer.
			if (this.tx黒幕 != null)
			{
                this.tx黒幕.n透明度 = (this.mode == EFIFOモード.フェードイン) ? (((100 - this.counter.n現在の値) * 0xff) / 100) : ((this.counter.n現在の値 * 0xff) / 100);
                this.tx黒幕.t2D描画(CDTXMania.app.Device, 0, 0);
                string path = CDTXMania.DTX.strフォルダ名 + CDTXMania.DTX.PREIMAGE;
                if( this.txジャケット == null ) // 2019.04.26 kairera0467
                {
                    if (!File.Exists(path))
                    {
                        //Trace.TraceWarning("ファイルが存在しません。({0})", new object[] { path });
                        this.txジャケット = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\\5_preimage default.png"));
                    }
                    else
                    {
                        this.txジャケット = CDTXMania.tテクスチャの生成(path);
                    }
                }

                if( this.txジャケット != null )
                {
                    this.txジャケット.vc拡大縮小倍率.X = 0.96f;
                    this.txジャケット.vc拡大縮小倍率.Y = 0.96f;
                    this.txジャケット.fZ軸中心回転 = 0.28f;
                    this.txジャケット.n透明度 = (this.mode == EFIFOモード.フェードイン) ? (((100 - this.counter.n現在の値) * 0xff) / 100) : ((this.counter.n現在の値 * 0xff) / 100);
                    this.txジャケット.t2D描画(CDTXMania.app.Device, 620, 40);
                }
			}
            else if (this.tx黒幕 == null)
            {
                this.tx黒タイル64x64.n透明度 = (this.mode == EFIFOモード.フェードイン) ? (((100 - this.counter.n現在の値) * 0xff) / 100) : ((this.counter.n現在の値 * 0xff) / 100);
                for (int i = 0; i <= (SampleFramework.GameWindowSize.Width / 64); i++)		// #23510 2010.10.31 yyagi: change "clientSize.Width" to "640" to fix FIFO drawing size
                {
                    for (int j = 0; j <= (SampleFramework.GameWindowSize.Height / 64); j++)	// #23510 2010.10.31 yyagi: change "clientSize.Height" to "480" to fix FIFO drawing size
                    {
                        this.tx黒タイル64x64.t2D描画(CDTXMania.app.Device, i * 64, j * 64);
                    }
                }
            }
			if( this.counter.n現在の値 != 150 )
			{
				return 0;
			}
			return 1;
		}


		// その他

		#region [ private ]
		//-----------------
		public CCounter counter;
		private EFIFOモード mode;
		private CTexture tx黒タイル64x64;
        private CTexture tx黒幕;
        private CTexture txジャケット;
		//-----------------
		#endregion
	}
}
