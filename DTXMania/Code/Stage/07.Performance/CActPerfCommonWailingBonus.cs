using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal abstract class CActPerfCommonWailingBonus : CActivity
	{
		// メソッド

		public CActPerfCommonWailingBonus()
		{
			base.bNotActivated = true;
		}

		public void Start( EInstrumentPart part )
		{
			this.Start( part, null );
		}
		public abstract void Start( EInstrumentPart part, CChip r歓声Chip );



		// CActivity 実装

		public override void OnActivate()
		{
			base.OnActivate();
		}
		public override void OnDeactivate()
		{
			base.OnDeactivate();
		}

		public override void OnManagedCreateResources()
		{
			if ( !base.bNotActivated )
			{
				this.txWailingBonus = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlay wailing bonus.png" ) );
                this.txWailingFlush = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\7_WailingFlush.png" ) );
                this.txWailingFire = CDTXMania.tテクスチャの生成Af( CSkin.Path( @"Graphics\7_WailingFire.png" ) );
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if ( !base.bNotActivated )
			{
				CDTXMania.tReleaseTexture( ref this.txWailingBonus );
                CDTXMania.tReleaseTexture( ref this.txWailingFlush );
                CDTXMania.tReleaseTexture( ref this.txWailingFire );
				base.OnManagedReleaseResources();
			}
		}


		// Other

		#region [ private ]
		//-----------------
		protected CCounter[,] ct進行用 = new CCounter[ 3, 4 ];
        protected CCounter[,] ctWailing炎 = new CCounter[ 3, 4 ];
		protected CTexture txWailingBonus;
        protected CTexture txWailingFlush;
        protected CTextureAf txWailingFire;
		//-----------------
		#endregion
	}
}
