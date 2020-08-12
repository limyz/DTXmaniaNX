using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal abstract class CAct演奏WailingBonus共通 : CActivity
	{
		// メソッド

		public CAct演奏WailingBonus共通()
		{
			base.bNotActivated = true;
		}

		public void Start( E楽器パート part )
		{
			this.Start( part, null );
		}
		public abstract void Start( E楽器パート part, CDTX.CChip r歓声Chip );



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
				this.txWailingBonus = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlay wailing bonus.png" ) );
                this.txWailingFlush = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_WailingFlush.png" ) );
                this.txWailingFire = CDTXMania.tテクスチャの生成Af( CSkin.Path( @"Graphics\7_WailingFire.png" ) );
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if ( !base.bNotActivated )
			{
				CDTXMania.tテクスチャの解放( ref this.txWailingBonus );
                CDTXMania.tテクスチャの解放( ref this.txWailingFlush );
                CDTXMania.tテクスチャの解放( ref this.txWailingFire );
				base.OnManagedReleaseResources();
			}
		}


		// その他

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
