using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal abstract class CActPerfCommonDanger : CActivity
	{
		// コンストラクタ

		public CActPerfCommonDanger()
		{
			base.bNotActivated = true;
		}


		// CActivity 実装

		public override void OnActivate()
		{
			for ( int i = 0; i < 3; i++ )
			{
				this.bDanger中[i] = false;
			}
//			this.ct移動用 = new CCounter();
//			this.ct透明度用 = new CCounter();
			this.ct移動用 = null;
			this.ct透明度用 = null;

			base.OnActivate();
		}
		public override void OnDeactivate()
		{
			this.ct移動用 = null;
			this.ct透明度用 = null;
			base.OnDeactivate();
		}

		/// <summary>
		/// DANGER描画
		/// </summary>
		/// <param name="bIsDangerDrums">DrumsがDangerならtrue</param>
		/// <param name="bIsDamgerGuitar">GuitarがDangerならtrue</param>
		/// <param name="bIsDangerBass">BassがDangerならtrue</param>
		/// <returns></returns>
		public abstract int t進行描画( bool bIsDangerDrums, bool bIsDamgerGuitar, bool bIsDangerBass );



		// その他

		#region [ private ]
		//-----------------
		protected bool[] bDanger中 = { false, false, false};
		protected CCounter ct移動用;
		protected CCounter ct透明度用;
		//-----------------
		#endregion
	
	}
}
