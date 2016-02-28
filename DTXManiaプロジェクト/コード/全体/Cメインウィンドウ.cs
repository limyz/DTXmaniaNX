using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using SlimDX.Windows;
using FDK;
using DirectShowLib;

namespace DTXMania
{
	class Cメインウィンドウ : RenderForm
	{
		public Cメインウィンドウ( CApp app )
		{
			this.app = app;
		}
		public Cメインウィンドウ( CApp app, string title, int width, int height )
		{
			this.app = app;
			this.Text = title;
			this.ClientSize = new Size( width, height );
		}

		/// <summary>
		/// <para>ここには、対応するイベントハンドラのないイベントをハンドルするコードを書く。</para>
		/// </summary>
		protected override void WndProc( ref Message m )
		{
            /*
			#region [ WM_COPYDATA: 二重起動された StrokeStyleT.exe からコマンドライン行が送られてきたら、それを取得してアクション実行予約を行う。]
			//-----------------
			if( m.Msg == CWin32.WM_COPYDATA )
			{
				// コマンドライン行を取得。
				var copyData = (CWin32.COPYDATA) m.GetLParam( (new CWin32.COPYDATA()).GetType() );
				
				// コマンドライン登録（アクション実行予約）
			    this.arrayコマンドライン = null;
				if( copyData.cbData > 0 )
					this.arrayコマンドライン = copyData.lpData.Split( new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
			}
			//-----------------
			#endregion
            */
			#region [ WM_DSGRAPHNOTIFY: DirectShow グラフからのイベントを受信し、処理を行う。]
			//-----------------
			if( m.Msg == CDirectShow.WM_DSGRAPHNOTIFY )
			{
				int nインスタンスID = m.LParam.ToInt32();
				CDirectShow dsイベント発信者 = null;
				bool b発信者が背景動画である = false;

				#region [ イベント発信者(CDirectShow) を特定する。]
				//-----------------
				// このインスタンスID は、今のところ以下のいずれかである。
				// ・背景動画
				// ・仮想ドラムキット
				// なので、これらを順に見ていって、イベント発信者を特定する。

				if( CDTXMania.stage選曲.r現在演奏中のスコアの背景動画 != null &&
                    CDTXMania.stage選曲.r現在演奏中のスコアの背景動画.nインスタンスID == nインスタンスID)
				{
                    dsイベント発信者 = CDTXMania.stage選曲.r現在演奏中のスコアの背景動画;
					b発信者が背景動画である = true;
				}
				else
				{
					dsイベント発信者 = CDirectShow.tインスタンスを返す( nインスタンスID );
					b発信者が背景動画である = false;
				}
				//-----------------
				#endregion

				if( dsイベント発信者 == null )
					return;		// 特定に失敗したら無視。

				#region [ イベント発信者からすべてのイベントを受け取り、処理する。]
				//-----------------
				IMediaEventEx mediaEventEx = dsイベント発信者.MediaEventEx;

				if( mediaEventEx == null )
					return;		// 既に DirectShow の終了処理が始まっている場合は何もしない。

				// この辺りの処理の参考URL：
				// http://msdn.microsoft.com/ja-jp/library/cc370589.aspx

				EventCode eventCode;
				IntPtr param1, param2;
				while( mediaEventEx.GetEvent( out eventCode, out param1, out param2, 0 ) == CWin32.S_OK )	// イベントがなくなるまで
				{
					// イベント処理。

					if( eventCode == EventCode.Complete )
					{
						#region [ 再生完了 ]
						//-----------------
						if( dsイベント発信者.bループ再生 )
						{
							if( dsイベント発信者.bループ再生 )				// ループ指定ありなら
								dsイベント発信者.t再生位置を変更( 0.0 );	// すぐ再生開始。
						}
						else
						{
							if( b発信者が背景動画である )
							{
								dsイベント発信者.b再生中 = false;
								dsイベント発信者.t再生停止();				// 背景動画は終了しても巻き戻さない。
							}
							else
							{
								dsイベント発信者.b再生中 = false;
								dsイベント発信者.t再生停止();
								dsイベント発信者.t再生位置を変更( 0.0 );	// 再生完了しても再生は続くので、先頭に戻して
								dsイベント発信者.t再生準備開始();			// 次の再生に備えて準備しておく。
							}
						}
						//-----------------
						#endregion
					}

					// イベントを解放。

					mediaEventEx.FreeEventParams( eventCode, param1, param2 );
				}
				//-----------------
				#endregion
			}
			//-----------------
			#endregion
			#region [ WM_CLOSE: 終了指示フラグを立てる。立てるだけ。]
			//-----------------
			if( m.Msg == CWin32.WM_CLOSE )
			{
				this.app.bWM_CLOSEを受け取った = true;
				return;
			}
			//-----------------
			#endregion

			base.WndProc( ref m );
		}

		#region [ protected ]
		//-----------------
		protected CApp app  = null;
        public volatile string[] arrayコマンドライン = null;
		//-----------------
		#endregion
	}
}
