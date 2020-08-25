using System;
using System.Collections.Generic;
using System.Text;

namespace FDK
{
	/// <summary>
	/// <para>一定の間隔で処理を行うテンプレートパターンの定義。</para>
	/// <para>たとえば、tUpdate() で 5ms ごとに行う処理を前回のt進行()の呼び出しから 15ms 後に呼び出した場合は、処理が 3回 実行される。</para>
	/// </summary>
	public class CFixedIntervalProcessing : IDisposable
	{
		public delegate void dg処理();
		public void tUpdate( long n間隔ms, dg処理 dg処理 )
		{
			// タイマ更新

			if( this.timer == null )
				return;
			this.timer.tUpdate();


			// 初めての進行処理

			if( this.n前回の時刻 == CTimer.nUnused )
				this.n前回の時刻 = this.timer.n現在時刻ms;


			// タイマが一回りしてしまった時のため……

			if( this.timer.n現在時刻ms < this.n前回の時刻 )
				this.n前回の時刻 = this.timer.n現在時刻ms;

	
			// 時間内の処理を実行。

			while( ( this.timer.n現在時刻ms - this.n前回の時刻 ) >= n間隔ms )
			{
				dg処理();

				this.n前回の時刻 += n間隔ms;
			}
		}

		#region [ IDisposable 実装 ]
		//-----------------
		public void Dispose()
		{
			CCommon.tDispose( ref this.timer );
		}
		//-----------------
		#endregion

		#region [ protected ]
		//-----------------
		protected CTimer timer = new CTimer( CTimer.EType.MultiMedia );
		protected long n前回の時刻 = CTimer.nUnused;
		//-----------------
		#endregion
	}
}
