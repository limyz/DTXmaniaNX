using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using SlimDX;
using FDK;

namespace DTXMania
{
	internal class CActSelectShowCurrentPosition : CActivity
	{
		// メソッド

		public CActSelectShowCurrentPosition()
		{
			base.bNotActivated = true;
		}

		// CActivity 実装

		public override void OnActivate()
		{
			if ( this.b活性化してる )
				return;

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
                this.txScrollBar = CDTXMania.tテクスチャの生成( CSkin.Path(@"Graphics\5_scrollbar.png"), false );
                base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if ( !base.bNotActivated )
			{
				CDTXMania.t安全にDisposeする( ref this.txScrollBar );
				base.OnManagedReleaseResources();
			}
		}
		public override int On進行描画()
		{
            int x = 1280 - 24 + 50;
            int y = 120;

			if ( this.txScrollBar != null )
			{
			#region [ スクロールバーの描画 #27648 ]
                this.txScrollBar.tDraw2D(CDTXMania.app.Device, x - (CDTXMania.stage選曲.ct登場時アニメ用共通.n現在の値 / 2f), y, new Rectangle(0, 0, 12, 492));	// 本当のy座標は88なんだが、なぜか約30のバイアスが掛かる___
			#endregion
			#region [ スクロール地点の描画 (計算はCActSelect曲リストで行う。スクロール位置と選曲項目の同期のため。)#27648 ]
				int py = CDTXMania.stage選曲.nスクロールバー相対y座標;
                if ( py <= 492 - 12 && py >= 0 )
				{
                    this.txScrollBar.tDraw2D(CDTXMania.app.Device, x - (CDTXMania.stage選曲.ct登場時アニメ用共通.n現在の値 / 2f), y + py, new Rectangle(0, 492, 12, 12));
				}
			#endregion
			}

			return 0;
		}


		// その他

		#region [ private ]
		//-----------------
		private CTexture txScrollBar;
		//-----------------
		#endregion
	}
}
