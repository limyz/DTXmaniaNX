using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using SlimDX;
using FDK;

namespace DTXMania
{
	internal class CActResultSongBar : CActivity
	{
		// コンストラクタ

		public CActResultSongBar()
		{
			base.b活性化してない = true;
		}


		// メソッド

		public void tアニメを完了させる()
		{
			this.ct登場用.n現在の値 = this.ct登場用.n終了値;
		}


		// CActivity 実装

		public override void On活性化()
		{
			this.n本体X = 0;
			this.n本体Y = 0x18b;
			this.ft曲名用フォント = new Font( "MS PGothic", 44f, FontStyle.Regular, GraphicsUnit.Pixel );
			base.On活性化();
		}
		public override void On非活性化()
		{
			if( this.ft曲名用フォント != null )
			{
				this.ft曲名用フォント.Dispose();
				this.ft曲名用フォント = null;
			}
			if( this.ct登場用 != null )
			{
				this.ct登場用 = null;
			}
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				//this.txバー = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenResult song bar.png" ) );
				try
				{
					Bitmap image = new Bitmap( 0x3a8, 0x36 );
					Graphics graphics = Graphics.FromImage( image );
					graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
					graphics.DrawString( CDTXMania.DTX.TITLE, this.ft曲名用フォント, Brushes.White, ( float ) 8f, ( float ) 0f );
					this.tx曲名 = new CTexture( CDTXMania.app.Device, image, CDTXMania.TextureFormat );
					this.tx曲名.vc拡大縮小倍率 = new Vector3( 0.5f, 0.5f, 1f );
					graphics.Dispose();
					image.Dispose();
				}
				catch( CTextureCreateFailedException )
				{
					Trace.TraceError( "曲名テクスチャの生成に失敗しました。" );
					this.tx曲名 = null;
				}
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				CDTXMania.tテクスチャの解放( ref this.txバー );
				CDTXMania.tテクスチャの解放( ref this.tx曲名 );
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if( base.b活性化してない )
			{
				return 0;
			}
			if( base.b初めての進行描画 )
			{
				this.ct登場用 = new CCounter( 0, 270, 4, CDTXMania.Timer );
				base.b初めての進行描画 = false;
			}
			this.ct登場用.t進行();
			int num = 0x1d4;
			int num2 = num - 0x40;
			if( this.ct登場用.b進行中 )
			{
				if( this.ct登場用.n現在の値 <= 100 )
				{
					double num3 = 1.0 - ( ( (double) this.ct登場用.n現在の値 ) / 100.0 );
					this.n本体X = -( (int) ( num * Math.Sin( Math.PI / 2 * num3 ) ) );
					this.n本体Y = 0x18b;
				}
				else if( this.ct登場用.n現在の値 <= 200 )
				{
					double num4 = ( (double) ( this.ct登場用.n現在の値 - 100 ) ) / 100.0;
					this.n本体X = -( (int) ( ( ( (double) num ) / 6.0 ) * Math.Sin( Math.PI * num4 ) ) );
					this.n本体Y = 0x18b;
				}
				else if( this.ct登場用.n現在の値 <= 270 )
				{
					double num5 = ( (double) ( this.ct登場用.n現在の値 - 200 ) ) / 70.0;
					this.n本体X = -( (int) ( ( ( (double) num ) / 18.0 ) * Math.Sin( Math.PI * num5 ) ) );
					this.n本体Y = 0x18b;
				}
			}
			else
			{
				this.n本体X = 0;
				this.n本体Y = 0x18b;
			}
			int num6 = this.n本体X;
			int y = this.n本体Y;
			int num8 = 0;
			while( num8 < num2 )
			{
				Rectangle rectangle = new Rectangle( 0, 0, 0x40, 0x40 );
				if( ( num8 + rectangle.Width ) >= num2 )
				{
					rectangle.Width -= ( num8 + rectangle.Width ) - num2;
				}
				num8 += rectangle.Width;
			}
			if( this.tx曲名 != null )
			{
				this.tx曲名.t2D描画( CDTXMania.app.Device, this.n本体X, this.n本体Y + 20 );
			}
			if( !this.ct登場用.b終了値に達した )
			{
				return 0;
			}
			return 1;
		}


		// その他

		#region [ private ]
		//-----------------
		private CCounter ct登場用;
		private Font ft曲名用フォント;
		private int n本体X;
		private int n本体Y;
		private CTexture txバー;
		private CTexture tx曲名;
		//-----------------
		#endregion
	}
}
