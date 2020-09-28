using System;
using System.Collections.Generic;
using System.Text;

namespace FDK
{
	public class CFPS
	{
		// プロパティ

		public int n現在のFPS
		{
			get;
			private set;
		}
		public bool bFPSの値が変化した
		{
			get;
			private set;
		}


		// コンストラクタ

		public CFPS()
		{
			this.n現在のFPS = 0;
			this.timer = new CTimer( CTimer.EType.MultiMedia );
			this.基点時刻ms = this.timer.nCurrentTime;
			this.内部FPS = 0;
			this.bFPSの値が変化した = false;
		}


		// メソッド

		public void tカウンタ更新()
		{
			this.timer.tUpdate();
			this.bFPSの値が変化した = false;

			const long INTERVAL = 1000;
			while( ( this.timer.nCurrentTime - this.基点時刻ms ) >= INTERVAL )
			{
				this.n現在のFPS = this.内部FPS;
				this.内部FPS = 0;
				this.bFPSの値が変化した = true;
				this.基点時刻ms += INTERVAL;
			}
			this.内部FPS++;
		}


		// その他

		#region [ private ]
		//-----------------
		private CTimer	timer;
		private long	基点時刻ms;
		private int		内部FPS;
		//-----------------
		#endregion
	}
}
