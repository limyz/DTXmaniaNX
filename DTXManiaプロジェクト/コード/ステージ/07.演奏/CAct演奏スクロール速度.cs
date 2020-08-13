using System;
using System.Collections.Generic;
using System.Text;
using FDK;

namespace DTXMania
{
	internal class CAct演奏スクロール速度 : CActivity
	{
		// プロパティ

		public STDGBVALUE<double> db現在の譜面スクロール速度;


		// コンストラクタ

		public CAct演奏スクロール速度()
		{
			base.bNotActivated = true;
		}


		// CActivity 実装

		public override void OnActivate()
		{
			for( int i = 0; i < 3; i++ )
			{
				this.db譜面スクロール速度[ i ] = this.db現在の譜面スクロール速度[ i ] = (double) CDTXMania.ConfigIni.n譜面スクロール速度[ i ];
				this.n速度変更制御タイマ[ i ] = -1;
			}
			base.OnActivate();
		}
		public override unsafe int OnUpdateAndDraw()
		{
			if( !base.bNotActivated )
			{
				if( base.bJustStartedUpdate )
				{
					this.n速度変更制御タイマ.Drums = this.n速度変更制御タイマ.Guitar = this.n速度変更制御タイマ.Bass = CSoundManager.rc演奏用タイマ.nシステム時刻;
					base.bJustStartedUpdate = false;
				}
				long num = CSoundManager.rc演奏用タイマ.n現在時刻;
				for( int i = 0; i < 3; i++ )
				{
					double num3 = (double) CDTXMania.ConfigIni.n譜面スクロール速度[ i ];
					if( num < this.n速度変更制御タイマ[ i ] )
					{
						this.n速度変更制御タイマ[ i ] = num;
					}
					while( ( num - this.n速度変更制御タイマ[ i ] ) >= 2 )
					{
						if( this.db譜面スクロール速度[ i ] < num3 )
						{
							this.db現在の譜面スクロール速度[ i ] += 0.012;

							if( this.db現在の譜面スクロール速度[ i ] > num3 )
							{
								this.db現在の譜面スクロール速度[ i ] = num3;
								this.db譜面スクロール速度[ i ] = num3;
							}
						}
						else if( this.db譜面スクロール速度[ i ] > num3 )
						{
							this.db現在の譜面スクロール速度[ i ] -= 0.012;

							if( this.db現在の譜面スクロール速度[ i ] < num3 )
							{
								this.db現在の譜面スクロール速度[ i ] = num3;
								this.db譜面スクロール速度[ i ] = num3;
							}
						}
						//this.db現在の譜面スクロール速度[ i ] = this.db譜面スクロール速度[ i ];
						this.n速度変更制御タイマ[ i ] += 2;
					}
				}
			}
			return 0;
		}


		// その他

		#region [ private ]
		//-----------------
		private STDGBVALUE<double> db譜面スクロール速度;
		private STDGBVALUE<long> n速度変更制御タイマ;
		//-----------------
		#endregion
	}
}
