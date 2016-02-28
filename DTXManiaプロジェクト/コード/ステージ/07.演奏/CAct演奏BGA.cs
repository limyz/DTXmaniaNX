using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using SlimDX.Direct3D9;
using FDK;

namespace DTXMania
{
	internal class CAct演奏BGA : CActivity
	{
		// コンストラクタ

		public CAct演奏BGA()
		{
			base.b活性化してない = true;
		}
		
		
		// メソッド

		public void ChangeScope( int nチャンネル, CDTX.CBMP bmp, CDTX.CBMPTEX bmptex )
		{
			for( int i = 0; i < 8; i++ )
			{
				if( nチャンネル == this.nChannel[ i ] )
				{
					this.stLayer[ i ].rBMP = bmp;
					this.stLayer[ i ].rBMPTEX = bmptex;
					return;
				}
			}
		}
		public void Start( int nチャンネル, CDTX.CBMP bmp, CDTX.CBMPTEX bmptex, int n開始サイズW, int n開始サイズH, int n終了サイズW, int n終了サイズH, int n画像側開始位置X, int n画像側開始位置Y, int n画像側終了位置X, int n画像側終了位置Y, int n表示側開始位置X, int n表示側開始位置Y, int n表示側終了位置X, int n表示側終了位置Y, int n総移動時間ms )
		{
			this.Start( nチャンネル, bmp, bmptex, n開始サイズW, n開始サイズH, n終了サイズW, n終了サイズH, n画像側開始位置X, n画像側開始位置Y, n画像側終了位置X, n画像側終了位置Y, n表示側開始位置X, n表示側開始位置Y, n表示側終了位置X, n表示側終了位置Y, n総移動時間ms, -1 );
		}
		public void Start( int nチャンネル, CDTX.CBMP bmp, CDTX.CBMPTEX bmptex, int n開始サイズW, int n開始サイズH, int n終了サイズW, int n終了サイズH, int n画像側開始位置X, int n画像側開始位置Y, int n画像側終了位置X, int n画像側終了位置Y, int n表示側開始位置X, int n表示側開始位置Y, int n表示側終了位置X, int n表示側終了位置Y, int n総移動時間ms, int n移動開始時刻ms )
		{
			for( int i = 0; i < 8; i++ )
			{
				if( nチャンネル == this.nChannel[ i ] )
				{
					this.stLayer[ i ].rBMP = bmp;
					this.stLayer[ i ].rBMPTEX = bmptex;
					this.stLayer[ i ].sz開始サイズ.Width = n開始サイズW;
					this.stLayer[ i ].sz開始サイズ.Height = n開始サイズH;
					this.stLayer[ i ].sz終了サイズ.Width = n終了サイズW;
					this.stLayer[ i ].sz終了サイズ.Height = n終了サイズH;
					this.stLayer[ i ].pt画像側開始位置.X = n画像側開始位置X;
					this.stLayer[ i ].pt画像側開始位置.Y = n画像側開始位置Y;
					this.stLayer[ i ].pt画像側終了位置.X = n画像側終了位置X;
					this.stLayer[ i ].pt画像側終了位置.Y = n画像側終了位置Y;
					this.stLayer[ i ].pt表示側開始位置.X = n表示側開始位置X;
					this.stLayer[ i ].pt表示側開始位置.Y = n表示側開始位置Y;
					this.stLayer[ i ].pt表示側終了位置.X = n表示側終了位置X;
					this.stLayer[ i ].pt表示側終了位置.Y = n表示側終了位置Y;
					this.stLayer[ i ].n総移動時間ms = n総移動時間ms;
					this.stLayer[ i ].n移動開始時刻ms = ( n移動開始時刻ms != -1 ) ? n移動開始時刻ms : CDTXMania.Timer.n現在時刻;
				}
			}
		}
		public void SkipStart( int n移動開始時刻ms )
		{
			for( int i = 0; i < CDTXMania.DTX.listChip.Count; i++ )
			{
				CDTX.CChip chip = CDTXMania.DTX.listChip[ i ];
				if( chip.n発声時刻ms > n移動開始時刻ms )
				{
					return;
				}
				switch( chip.eBGA種別 )
				{
					case EBGA種別.BMP:
						if( ( chip.rBMP != null ) && ( chip.rBMP.tx画像 != null ) )
						{
							this.Start( chip.nチャンネル番号, chip.rBMP, null, chip.rBMP.n幅, chip.rBMP.n高さ, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, chip.n発声時刻ms );
						}
						break;

					case EBGA種別.BMPTEX:
						if( ( chip.rBMPTEX != null ) && ( chip.rBMPTEX.tx画像 != null ) )
						{
							this.Start( chip.nチャンネル番号, null, chip.rBMPTEX, chip.rBMPTEX.tx画像.sz画像サイズ.Width, chip.rBMPTEX.tx画像.sz画像サイズ.Height, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, chip.n発声時刻ms );
						}
						break;

					case EBGA種別.BGA:
						if( chip.rBGA != null )
						{
							this.Start( chip.nチャンネル番号, chip.rBMP, chip.rBMPTEX, chip.rBGA.pt画像側右下座標.X - chip.rBGA.pt画像側左上座標.X, chip.rBGA.pt画像側右下座標.Y - chip.rBGA.pt画像側左上座標.Y, 0, 0, chip.rBGA.pt画像側左上座標.X, chip.rBGA.pt画像側左上座標.Y, 0, 0, chip.rBGA.pt表示座標.X, chip.rBGA.pt表示座標.Y, 0, 0, 0, chip.n発声時刻ms );
						}
						break;

					case EBGA種別.BGAPAN:
						if( chip.rBGAPan != null )
						{
							this.Start( chip.nチャンネル番号, chip.rBMP, chip.rBMPTEX, chip.rBGAPan.sz開始サイズ.Width, chip.rBGAPan.sz開始サイズ.Height, chip.rBGAPan.sz終了サイズ.Width, chip.rBGAPan.sz終了サイズ.Height, chip.rBGAPan.pt画像側開始位置.X, chip.rBGAPan.pt画像側開始位置.Y, chip.rBGAPan.pt画像側終了位置.X, chip.rBGAPan.pt画像側終了位置.Y, chip.rBGAPan.pt表示側開始位置.X, chip.rBGAPan.pt表示側開始位置.Y, chip.rBGAPan.pt表示側終了位置.X, chip.rBGAPan.pt表示側終了位置.Y, chip.n総移動時間, chip.n発声時刻ms );
						}
						break;
				}
			}
		}
		public void Stop()
		{
			for( int i = 0; i < 8; i++ )
			{
				this.stLayer[ i ].n移動開始時刻ms = -1;
			}
		}


		// CActivity 実装

		public override void On活性化()
		{
			for( int i = 0; i < 8; i++ )
			{
				STLAYER stlayer2 = new STLAYER();
				STLAYER stlayer = stlayer2;
				stlayer.rBMP = null;
				stlayer.rBMPTEX = null;
				stlayer.n移動開始時刻ms = -1;
				this.stLayer[ i ] = stlayer;
			}
			base.On活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
                this.txBGAバックパネル = new CTexture(CDTXMania.app.Device, 278, 355, CDTXMania.app.GraphicsDeviceManager.CurrentSettings.BackBufferFormat, Pool.Managed);
				using( Surface surface = CDTXMania.app.Device.GetBackBuffer( 0, 0 ) )
				{
					this.sfBackBuffer = Surface.CreateOffscreenPlain( CDTXMania.app.Device, surface.Description.Width, surface.Description.Height, surface.Description.Format, Pool.SystemMemory );
				}
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
                CDTXMania.tテクスチャの解放( ref this.txBGAバックパネル );
				if( this.sfBackBuffer != null )
				{
					this.sfBackBuffer.Dispose();
					this.sfBackBuffer = null;
				}
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			throw new InvalidOperationException( "t進行描画(x,y)のほうを使用してください。" );
		}
		public int t進行描画( int x, int y )
		{
			if( !base.b活性化してない )
			{
                this.txBGAバックパネル.t2D描画(CDTXMania.app.Device, 980, 0);
				for( int i = 0; i < 8; i++ )
				{
					if( ( ( this.stLayer[ i ].n移動開始時刻ms != -1 ) && ( ( this.stLayer[ i ].rBMP != null ) || ( this.stLayer[ i ].rBMPTEX != null ) ) ) && ( ( ( this.stLayer[ i ].rBMP == null ) || ( this.stLayer[ i ].rBMP.bUse && ( this.stLayer[ i ].rBMP.tx画像 != null ) ) ) && ( ( this.stLayer[ i ].rBMPTEX == null ) || ( this.stLayer[ i ].rBMPTEX.bUse && ( this.stLayer[ i ].rBMPTEX.tx画像 != null ) ) ) ) )
					{
						Size size = this.stLayer[ i ].sz開始サイズ;
						Size size2 = this.stLayer[ i ].sz終了サイズ;
						Point point = this.stLayer[ i ].pt画像側開始位置;
						Point point2 = this.stLayer[ i ].pt画像側終了位置;
						Point point3 = this.stLayer[ i ].pt表示側開始位置;
						Point point4 = this.stLayer[ i ].pt表示側終了位置;
						long num2 = this.stLayer[ i ].n総移動時間ms;
						long num3 = this.stLayer[ i ].n移動開始時刻ms;
						if( CDTXMania.Timer.n現在時刻 < num3 )
						{
							num3 = CDTXMania.Timer.n現在時刻;
						}
						Size size3 = new Size( 0x116, 0x163 );
						Size size4 = new Size( ( this.stLayer[ i ].rBMP != null ) ? this.stLayer[ i ].rBMP.n幅 : this.stLayer[ i ].rBMPTEX.tx画像.sz画像サイズ.Width, ( this.stLayer[ i ].rBMP != null ) ? this.stLayer[ i ].rBMP.n高さ : this.stLayer[ i ].rBMPTEX.tx画像.sz画像サイズ.Height );
						int num4 = (int) ( ( CDTXMania.Timer.n現在時刻 - num3 ) * ( ( (double) CDTXMania.ConfigIni.n演奏速度 ) / 20.0 ) );
						if( ( num2 != 0 ) && ( num2 < num4 ) )
						{
							this.stLayer[ i ].pt画像側開始位置 = point = point2;
							this.stLayer[ i ].pt表示側開始位置 = point3 = point4;
							this.stLayer[ i ].sz開始サイズ = size = size2;
							this.stLayer[ i ].n総移動時間ms = num2 = 0;
						}
						Rectangle rectangle = new Rectangle();
						Rectangle rectangle2 = new Rectangle();
						if( num2 == 0 )
						{
							rectangle.X = point.X;
							rectangle.Y = point.Y;
							rectangle.Width = size.Width;
							rectangle.Height = size.Height;
							rectangle2.X = point3.X;
							rectangle2.Y = point3.Y;
							rectangle2.Width = size.Width;
							rectangle2.Height = size.Height;
						}
						else
						{
							double num5 = ( (double) num4 ) / ( (double) num2 );
							Size size5 = new Size( size.Width + ( (int) ( ( size2.Width - size.Width ) * num5 ) ), size.Height + ( (int) ( ( size2.Height - size.Height ) * num5 ) ) );
							rectangle.X = point.X + ( (int) ( ( point2.X - point.X ) * num5 ) );
							rectangle.Y = point.Y + ( (int) ( ( point2.Y - point.Y ) * num5 ) );
							rectangle.Width = size5.Width;
							rectangle.Height = size5.Height;
							rectangle2.X = point3.X + ( (int) ( ( point4.X - point3.X ) * num5 ) );
							rectangle2.Y = point3.Y + ( (int) ( ( point4.Y - point3.Y ) * num5 ) );
							rectangle2.Width = size5.Width;
							rectangle2.Height = size5.Height;
						}
						if( ( ( ( rectangle.Right > 0 ) && ( rectangle.Bottom > 0 ) ) && ( ( rectangle.Left < size4.Width ) && ( rectangle.Top < size4.Height ) ) ) && ( ( ( rectangle2.Right > 0 ) && ( rectangle2.Bottom > 0 ) ) && ( ( rectangle2.Left < size3.Width ) && ( rectangle2.Top < size3.Height ) ) ) )
						{
							if( rectangle.X < 0 )
							{
								rectangle2.Width -= -rectangle.X;
								rectangle2.X += -rectangle.X;
								rectangle.Width -= -rectangle.X;
								rectangle.X = 0;
							}
							if( rectangle.Y < 0 )
							{
								rectangle2.Height -= -rectangle.Y;
								rectangle2.Y += -rectangle.Y;
								rectangle.Height -= -rectangle.Y;
								rectangle.Y = 0;
							}
							if( rectangle.Right > size4.Width )
							{
								rectangle2.Width -= rectangle.Right - size4.Width;
								rectangle.Width -= rectangle.Right - size4.Width;
							}
							if( rectangle.Bottom > size4.Height )
							{
								rectangle2.Height -= rectangle.Bottom - size4.Height;
								rectangle.Height -= rectangle.Bottom - size4.Height;
							}
							if( rectangle2.X < 0 )
							{
								rectangle.Width -= -rectangle2.X;
								rectangle.X += -rectangle2.X;
								rectangle2.Width -= rectangle2.X;
								rectangle2.X = 0;
							}
							if( rectangle2.Y < 0 )
							{
								rectangle.Height -= -rectangle2.Y;
								rectangle.Y += -rectangle2.Y;
								rectangle2.Height -= -rectangle2.Y;
								rectangle2.Y = 0;
							}
							if( rectangle2.Right > size3.Width )
							{
								rectangle.Width -= rectangle2.Right - size3.Width;
								rectangle2.Width -= rectangle2.Right - size3.Width;
							}
							if( rectangle2.Bottom > size3.Height )
							{
								rectangle.Height -= rectangle2.Bottom - size3.Height;
								rectangle2.Height -= rectangle2.Bottom - size3.Height;
							}
							if( ( ( ( ( rectangle.Left < rectangle.Right ) && ( rectangle.Top < rectangle.Bottom ) ) && ( ( rectangle2.Left < rectangle2.Right ) && ( rectangle2.Top < rectangle2.Bottom ) ) ) && ( ( ( rectangle.Right >= 0 ) && ( rectangle.Bottom >= 0 ) ) && ( ( rectangle.Left <= size4.Width ) && ( rectangle.Top <= size4.Height ) ) ) ) && ( ( ( rectangle2.Right >= 0 ) && ( rectangle2.Bottom >= 0 ) ) && ( ( rectangle2.Left <= size3.Width ) && ( rectangle2.Top <= size3.Height ) ) ) )
							{
								if( ( this.stLayer[ i ].rBMP != null ) && ( this.stLayer[ i ].rBMP.tx画像 != null ) )
								{
									this.stLayer[ i ].rBMP.tx画像.t2D描画( CDTXMania.app.Device, x + rectangle2.X, y + rectangle2.Y, rectangle );
								}
								else if( ( this.stLayer[ i ].rBMPTEX != null ) && ( this.stLayer[ i ].rBMPTEX.tx画像 != null ) )
								{
									this.stLayer[ i ].rBMPTEX.tx画像.t2D描画( CDTXMania.app.Device, x + rectangle2.X, y + rectangle2.Y, rectangle );
								}
							}
						}
					}
				}
			}
			return 0;
		}
		

		// その他

		#region [ private ]
		//-----------------
		[StructLayout( LayoutKind.Sequential )]
		private struct STLAYER
		{
			public CDTX.CBMP rBMP;
			public CDTX.CBMPTEX rBMPTEX;
			public Size sz開始サイズ;
			public Size sz終了サイズ;
			public Point pt画像側開始位置;
			public Point pt画像側終了位置;
			public Point pt表示側開始位置;
			public Point pt表示側終了位置;
			public long n総移動時間ms;
			public long n移動開始時刻ms;
		}

        private CTexture txBGAバックパネル;
		private readonly int[] nChannel = new int[] { 4, 7, 0x55, 0x56, 0x57, 0x58, 0x59, 0x60 };
		private Surface sfBackBuffer;
		private STLAYER[] stLayer = new STLAYER[ 8 ];
		//-----------------
		#endregion
	}
}
