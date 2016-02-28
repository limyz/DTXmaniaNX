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
			base.b活性化してない = true;
		}

		public void Start( E楽器パート part )
		{
			this.Start( part, null );
		}
		public abstract void Start( E楽器パート part, CDTX.CChip r歓声Chip );



		// CActivity 実装

		public override void On活性化()
		{
			base.On活性化();
		}
		public override void On非活性化()
		{
			base.On非活性化();
		}

		public override void OnManagedリソースの作成()
		{
			if ( !base.b活性化してない )
			{
				this.txWailingBonus = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlay wailing bonus.png" ) );
                this.txWailingFlush = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_WailingFlush.png" ) );
                this.txWailingFire = CDTXMania.tテクスチャの生成Af( CSkin.Path( @"Graphics\7_WailingFire.png" ) );
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if ( !base.b活性化してない )
			{
				CDTXMania.tテクスチャの解放( ref this.txWailingBonus );
                CDTXMania.tテクスチャの解放( ref this.txWailingFlush );
                CDTXMania.tテクスチャの解放( ref this.txWailingFire );
				base.OnManagedリソースの解放();
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
