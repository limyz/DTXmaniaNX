using System;
using System.Collections.Generic;
using System.Text;
using FDK;

namespace DTXMania
{
	internal class CActPerfCommonScore : CActivity
	{
		// プロパティ

		public STDGBVALUE<long> nスコアの増分;
        public STDGBVALUE<int>[] x位置 = new STDGBVALUE<int>[10];
		public STDGBVALUE<double> nCurrentTrueScore;
		public STDGBVALUE<long> n現在表示中のスコア;
        public STDGBVALUE<int> n本体X;
        public int n本体Y;
        public long n進行用タイマ;
		protected CTexture txScore;

		
		// コンストラクタ

		public CActPerfCommonScore()
		{
			base.bNotActivated = true;
		}


		// メソッド

		public double Get( EInstrumentPart part )
		{
			return this.nCurrentTrueScore[ (int) part ];
		}
		public void Set( EInstrumentPart part, double nScore )
		{
			int nPart = (int) part;
			if( this.nCurrentTrueScore[ nPart ] != nScore )
			{
				this.nCurrentTrueScore[ nPart ] = nScore;
				this.nスコアの増分[ nPart ] = (long) ( ( (double) ( this.nCurrentTrueScore[ nPart ] - this.n現在表示中のスコア[ nPart ] ) ) / 20.0 );
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
		public void Add( EInstrumentPart part, STAUTOPLAY bAutoPlay, long delta )
		{
			double rev = 1.0 ;
			switch ( part )
			{
				#region [ Unknown ]
				case EInstrumentPart.UNKNOWN:
					throw new ArgumentException();
				#endregion
				#region [ Gutiar ]
				case EInstrumentPart.GUITAR:
					if ( !CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay )
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
				case EInstrumentPart.BASS:
					if ( !CDTXMania.ConfigIni.bAllBassAreAutoPlay )
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

		public override void OnActivate()
		{
			this.n進行用タイマ = -1;
			for( int i = 0; i < 3; i++ )
			{
				this.n現在表示中のスコア[ i ] = 0L;
				this.nCurrentTrueScore[ i ] = 0L;
				this.nスコアの増分[ i ] = 0L;
			}
			base.OnActivate();
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated && !CDTXMania.DTXVmode.Enabled && !CDTXMania.DTX2WAVmode.Enabled)
			{
                this.txScore = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_score numbersGD.png"));
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated && !CDTXMania.DTXVmode.Enabled && !CDTXMania.DTX2WAVmode.Enabled)
			{
				CDTXMania.tReleaseTexture( ref this.txScore );
				base.OnManagedReleaseResources();
			}
		}
	}
}
