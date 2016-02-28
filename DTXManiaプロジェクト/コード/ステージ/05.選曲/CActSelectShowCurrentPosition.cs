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
			base.b活性化してない = true;
		}

		// CActivity 実装

		public override void On活性化()
		{
			if ( this.b活性化してる )
				return;

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
                this.txScrollBar = CDTXMania.tテクスチャの生成( CSkin.Path(@"Graphics\5_scrollbar.png"), false );
                base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if ( !base.b活性化してない )
			{
				CDTXMania.t安全にDisposeする( ref this.txScrollBar );
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
            int x = 1280 - 24 + 50;
            int y = 120;

			if ( this.txScrollBar != null )
			{
			#region [ スクロールバーの描画 #27648 ]
                this.txScrollBar.t2D描画(CDTXMania.app.Device, x - (CDTXMania.stage選曲.ct登場時アニメ用共通.n現在の値 / 2f), y, new Rectangle(0, 0, 12, 492));	// 本当のy座標は88なんだが、なぜか約30のバイアスが掛かる・・・
			#endregion
			#region [ スクロール地点の描画 (計算はCActSelect曲リストで行う。スクロール位置と選曲項目の同期のため。)#27648 ]
				int py = CDTXMania.stage選曲.nスクロールバー相対y座標;
                if ( py <= 492 - 12 && py >= 0 )
				{
                    this.txScrollBar.t2D描画(CDTXMania.app.Device, x - (CDTXMania.stage選曲.ct登場時アニメ用共通.n現在の値 / 2f), y + py, new Rectangle(0, 492, 12, 12));
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
