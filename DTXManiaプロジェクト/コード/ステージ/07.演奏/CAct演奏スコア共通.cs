using System;
using System.Collections.Generic;
using System.Text;
using FDK;

namespace DTXMania
{
	internal class CAct演奏スコア共通 : CActivity
	{
		// プロパティ

		public STDGBVALUE<long> nスコアの増分;
        public STDGBVALUE<int>[] x位置 = new STDGBVALUE<int>[10];
		public STDGBVALUE<double> n現在の本当のスコア;
		public STDGBVALUE<long> n現在表示中のスコア;
        public STDGBVALUE<int> n本体X;
        public int n本体Y;
        public long n進行用タイマ;
		protected CTexture txScore;

		
		// コンストラクタ

		public CAct演奏スコア共通()
		{
			base.b活性化してない = true;
		}


		// メソッド

		public double Get( E楽器パート part )
		{
			return this.n現在の本当のスコア[ (int) part ];
		}
		public void Set( E楽器パート part, double nScore )
		{
			int nPart = (int) part;
			if( this.n現在の本当のスコア[ nPart ] != nScore )
			{
				this.n現在の本当のスコア[ nPart ] = nScore;
				this.nスコアの増分[ nPart ] = (long) ( ( (double) ( this.n現在の本当のスコア[ nPart ] - this.n現在表示中のスコア[ nPart ] ) ) / 20.0 );
				if( this.nスコアの増分[ nPart ] < 1L )
				{
					this.nスコアの増分[ nPart ] = 1L;
				}
			}
		}
		/// <summary>
		/// 点数を加える(各種AUTO補正つき)
		/// </summary>
		/// <param name="part"></param>
		/// <param name="bAutoPlay"></param>
		/// <param name="delta"></param>
		public void Add( E楽器パート part, STAUTOPLAY bAutoPlay, long delta )
		{
			double rev = 1.0 ;
			switch ( part )
			{
				#region [ Unknown ]
				case E楽器パート.UNKNOWN:
					throw new ArgumentException();
				#endregion
				#region [ Gutiar ]
				case E楽器パート.GUITAR:
					if ( !CDTXMania.ConfigIni.bギターが全部オートプレイである )
					{
						#region [ Auto Wailing ]
						if ( bAutoPlay.GtW )
						{
							rev /= 2;
						}
						#endregion
						#region [ Auto Pick ]
						if ( bAutoPlay.GtPick )
						{
							rev /= 3;
						}
						#endregion
						#region [ Auto Neck ]
						if ( bAutoPlay.GtR || bAutoPlay.GtG || bAutoPlay.GtB )
						{
							rev /= 4;
						}
						#endregion
					}
					break;
				#endregion
				#region [ Bass ]
				case E楽器パート.BASS:
					if ( !CDTXMania.ConfigIni.bベースが全部オートプレイである )
					{
						#region [ Auto Wailing ]
						if ( bAutoPlay.BsW )
						{
							rev /= 2;
						}
						#endregion
						#region [ Auto Pick ]
						if ( bAutoPlay.BsPick )
						{
							rev /= 3;
						}
						#endregion
						#region [ Auto Neck ]
						if ( bAutoPlay.BsR || bAutoPlay.BsG || bAutoPlay.BsB )
						{
							rev /= 4;
						}
						#endregion
					}
					break;
				#endregion
			}
			this.Set( part, this.Get( part ) + delta * rev );
		}


		// CActivity 実装

		public override void On活性化()
		{
			this.n進行用タイマ = -1;
			for( int i = 0; i < 3; i++ )
			{
				this.n現在表示中のスコア[ i ] = 0L;
				this.n現在の本当のスコア[ i ] = 0L;
				this.nスコアの増分[ i ] = 0L;
			}
			base.On活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
                this.txScore = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_score numbersGD.png"));
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				CDTXMania.tテクスチャの解放( ref this.txScore );
				base.OnManagedリソースの解放();
			}
		}
	}
}
