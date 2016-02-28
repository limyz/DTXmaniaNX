using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using SlimDX;
using SlimDX.Direct3D9;
using SlimDX.Windows;


namespace FDK
{
	public class CApplicationForm : IDisposable
	{
		// プロパティ

		public RenderForm Window
		{
			get { return this._Window; }
			protected set { this._Window = value; }
		}
		public Direct3D Direct3D
		{
			get { return this._Direct3D; }
			protected set { this._Direct3D = value; }
		}
		public Device D3D9Device
		{
			get { return this._D3D9Device; }
			protected set { this._D3D9Device = value; }
		}
		public CD3DSettings currentD3DSettings
		{
			get;
			protected set;
		}
		public volatile object obj排他用 = new object();


		// コンストラクタ

		public CApplicationForm()
		{
			this.currentD3DSettings = null;
		}


		// メソッド

		/// <summary>
		/// <para>メインループを実行する。</para>
		/// <para>ウィンドウが閉じられるか例外が発生すると、このメソッドを抜ける。</para>
		/// </summary>
		public void Run()
		{
			// 初期化。

			#region [ Direct3D9 を生成する。]
			//-----------------
			try
			{
				this.Direct3D = new Direct3D();
			}
#if !DEBUG
			catch
			{
				MessageBox.Show( "Direct3D9 の生成に失敗しました。\n終了します。", "StrokeStyle<T>エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}
#endif
			finally
			{ 
			}
			//-----------------
			#endregion
			#region [ On初期化() を実行し、ウィンドウ（this.Window）を生成する。]
			//-----------------
			try
			{
				this.On初期化();	// この中でウィンドウを作成すること。

				if( this.Window == null )
					return;			// 作ってない。アプリ終了。
			}
#if !DEBUG
			catch
			{
				MessageBox.Show( "初期化に失敗しました。\n終了します。", "StrokeStyle<T>エラー", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}
#endif
			finally
			{
			}
			//-----------------
			#endregion


			// 各スレッドの実行開始。

			#region [ 進行スレッドの生成・実行開始。]
			//-----------------
			var h進行スレッド = new Thread( this.t進行スレッド処理 );
			h進行スレッド.Priority = ThreadPriority.Normal;
			h進行スレッド.Start();
			while( !h進行スレッド.IsAlive ) ;		// 起動するまでスピンロック；MSDN にこうしろと書いてある。
			//-----------------
			#endregion
			#region [ フロー制御スレッドの生成・実行開始。]
			//-----------------
			var hフロー制御スレッド = new Thread( this.tフロー制御スレッド処理 );
			hフロー制御スレッド.Priority = ThreadPriority.Normal;
			hフロー制御スレッド.Start();
			while( !hフロー制御スレッド.IsAlive ) ;		// 起動するまでスピンロック；MSDN にこうしろと書いてある。
			//-----------------
			#endregion
			#region [ 描画スレッド（＝メインスレッド）の実行開始。]
			//-----------------
			this.t描画スレッド処理();		// アプリが終了すると戻ってくる。
			//-----------------
			#endregion


			// 全スレッドの停止。

			#region [ 進行スレッドを停止。]
			//-----------------
			if( h進行スレッド.IsAlive )
			{
				this.bアプリケーションを終了する = true;	// 終了通知。（このフラグは進行スレッドで立てるのが基本だが、念のため。）
				
				if( !h進行スレッド.Join( 10 * 1000 ) )		// 最大10秒待つ。
				{
					Trace.TraceError( "進行スレッドの停止待ちがタイムアウトしました。進行スレッドを強制終了します。" );
					h進行スレッド.Abort();
				}
			}
			//-----------------
			#endregion
			#region [ フロー制御スレッドを停止。]
			//-----------------
			if( hフロー制御スレッド.IsAlive )
			{
				this.bアプリケーションを終了する = true;	// 終了通知。（このフラグは進行スレッドで立てるのが基本だが、念のため。）

				if( !hフロー制御スレッド.Join( 10 * 1000 ) )		// 最大10秒待つ。
				{
					Trace.TraceError( "フロー制御スレッドの停止待ちがタイムアウトしました。フロー制御スレッドを強制終了します。" );
					hフロー制御スレッド.Abort();
				}
			}
			//-----------------
			#endregion
			

			// アプリ正常終了。

			this.On終了();

			#region [ Direct3D9Device を解放。]
			//-----------------
			this.OnManageリソースを解放する();
			this.OnUnmanageリソースを解放する();
			C共通.tDisposeする( this.D3D9Device );
			//-----------------
			#endregion
			#region [ Direct3D9 を解放。]
			//-----------------
			C共通.tDisposeする( this.Direct3D );
			//-----------------
			#endregion
		}

		/// <summary>
		/// <para>Direct3Dデバイスの生成、変更、リセットを行う。</para>
		/// <para>新しい設定と現在の設定とを比較し、生成、変更、リセットのいずれかを実行する。</para>
		/// <para>ウィンドウのクライアントサイズはバックバッファに等しく設定される。</para>
		/// <para>処理に成功すれば true を返す。処理に失敗すれば、準正常系は false を返し、異常系は例外を発出する。</para>
		/// </summary>
		public bool tDirect3Dデバイスを生成・変更・リセットする( CD3DSettings newD3DSettings, Size sz論理画面, uint wsウィンドウモード時のウィンドウスタイル, uint ws全画面モード時のウィンドウスタイル, bool bマウスカーソルの表示を制御する )
		{
			if( this.Window == null )
				throw new InvalidOperationException( "ウィンドウが未生成のままDirect3D9デバイスを生成しようとしました。" );

			bool b初めての生成 = ( this.currentD3DSettings == null );
			var oldD3DSettings = ( b初めての生成 ) ? null : this.currentD3DSettings.Clone();
			bool bウィンドウモードにする = ( newD3DSettings.PresentParameters.Windowed );
			bool b全画面モードにする = !bウィンドウモードにする;
			bool b全画面からウィンドウへの切替えである = !b初めての生成 && !oldD3DSettings.PresentParameters.Windowed && bウィンドウモードにする;
			bool bウィンドウから全画面への切替えである = ( b初めての生成 || oldD3DSettings.PresentParameters.Windowed ) && b全画面モードにする;

			#region [ ウィンドウスタイルの設定。]
			//-----------------
			if( bウィンドウモードにする )
			{
				if( b初めての生成 || b全画面からウィンドウへの切替えである )
				{
					// ウィンドウモード用のウィンドウスタイルを設定する。
					CWin32.SetWindowLong( this.Window.Handle, CWin32.GWL_STYLE, CApplicationForm.wsウィンドウスタイル );
				}
			}
			else
			{
				if( bウィンドウから全画面への切替えである )
				{
					// 現在のウィンドウパラメータを保存する。
					this.mウインドウモード時の状態のバックアップ = new Cウィンドウ状態();
					this.mウインドウモード時の状態のバックアップ.bTopMostWindow = this.Window.TopMost;
					this.mウインドウモード時の状態のバックアップ.WindowPlacement = new CWin32.WINDOWPLACEMENT() { length = CWin32.WINDOWPLACEMENT.Length };
					CWin32.GetWindowPlacement( this.Window.Handle, ref this.mウインドウモード時の状態のバックアップ.WindowPlacement );
				}

				if( b初めての生成 || bウィンドウから全画面への切替えである )
				{
					// 全画面用のウィンドウスタイルを設定する。
					CWin32.SetWindowLong( this.Window.Handle, CWin32.GWL_STYLE, CApplicationForm.ws全画面スタイル );
				}
			}
			//-----------------
			#endregion
			#region [ ウィンドウのクライアントサイズをバックバッファに合わせる。]
			//-----------------
			if( this.Window.ClientSize.Width != newD3DSettings.PresentParameters.BackBufferWidth ||
				this.Window.ClientSize.Height != newD3DSettings.PresentParameters.BackBufferHeight )
			{
				this.Window.ClientSize = new Size( newD3DSettings.PresentParameters.BackBufferWidth, newD3DSettings.PresentParameters.BackBufferHeight );
			}
			//-----------------
			#endregion
			#region [ Win32メッセージをあるだけ処理。]
			//-----------------
            /*
			CWin32.WindowMessage msg;
            
			while( CWin32.PeekMessage( out msg, IntPtr.Zero, 0, 0, CWin32.PM_REMOVE ) )
			{
				CWin32.TranslateMessage( ref msg );
				CWin32.DispatchMessage( ref msg );
			}
            */
			//-----------------
			#endregion

			#region [ Direct3D9デバイスをリセットまたは新規生成し、リソースを復元する。]
			//-----------------
			bool bリセットだけでOK = ( !b初めての生成 && oldD3DSettings.bデバイスの再生成が不要でリセットだけで済む( newD3DSettings ) );

			if( bリセットだけでOK )
			{
				#region [ デバイスのリセット ]
				//-----------------
				this.OnUnmanageリソースを解放する();

				if( this.D3D9Device.Reset( newD3DSettings.PresentParameters ) != ResultCode.DeviceLost )
				{
					Trace.TraceInformation( "Direct3D9 デバイスをリセットしました。" );
					this.OnUnmanageリソースを生成する();
					bリセットだけでOK = true;
				}
				else
				{
					Trace.TraceWarning( "Direct3D9 デバイスのリセットに失敗しました。続けて、デバイスの新規生成を行います。" );
					bリセットだけでOK = false;		// 後段で新規生成する
				}
				//-----------------
				#endregion
			}
			
			if( !bリセットだけでOK ) // ← "else" にしないこと。前段でリセットに失敗した場合、bリセットでOK = false に変えてここに来るため。
			{
				#region [ デバイスの新規生成 ]
				//-----------------
				this.OnManageリソースを解放する();
				this.OnUnmanageリソースを解放する();
				
				C共通.tDisposeする( this.D3D9Device );
				this.D3D9Device = new Device(		// 失敗したら異常系とみなし、そのまま例外をthrow。
					this.Direct3D,
					newD3DSettings.nAdaptor,
					newD3DSettings.DeviceType,
					newD3DSettings.PresentParameters.DeviceWindowHandle,
					newD3DSettings.CreateFlags,
					newD3DSettings.PresentParameters );

				Trace.TraceInformation( "Direct3D9 デバイスを生成しました。" );
				this.D3D9Device.SetDialogBoxMode( true );

				this.OnManageリソースを生成する();
				this.OnUnmanageリソースを生成する();
				//-----------------
				#endregion
			}
			//-----------------
			#endregion

			this.currentD3DSettings = newD3DSettings.Clone();	// 成功したので設定を正式に保存する。

			this.OnD3Dデバイスステータスの初期化();


			// D3Dデバイスに連動する設定。

			#region [ ウィンドウパラメータを復元する（全画面→ウィンドウ切替えのときのみ）]
			//-----------------
			if( b全画面からウィンドウへの切替えである )
			{
				if( this.mウインドウモード時の状態のバックアップ != null )	// 全画面で起動していま初めてウィンドウモードにする場合は、復元するものがないので無視。
				{
					CWin32.SetWindowPlacement( this.Window.Handle, ref this.mウインドウモード時の状態のバックアップ.WindowPlacement );
					this.Window.TopMost = this.mウインドウモード時の状態のバックアップ.bTopMostWindow;
				}
			}
			//-----------------
			#endregion
			#region [ ウィンドウを不可視にしてたら可視に戻す。]
			//-----------------
			if( !this.Window.Visible )
				CWin32.ShowWindow( this.Window.Handle, CWin32.EShowWindow.Show );
			//-----------------
			#endregion
			
			#region [ 長期入力無反応時のアクションの設定。]
			//-----------------
			// システムは、入力反応が無くてもスリープしたりモニタをいじったりしてはいけない。
			CWin32.SetThreadExecutionState( CWin32.ES_SYSTEM_REQUIRED | CWin32.ES_DISPLAY_REQUIRED | CWin32.ES_CONTINUOUS );
			//-----------------
			#endregion
			
			#region [ マウスカーソルの表示・非表示 ]
			//-----------------
			if( bマウスカーソルの表示を制御する )
			{
				if( bウィンドウから全画面への切替えである || ( b初めての生成 && b全画面モードにする ) )
					this.tマウスカーソルを消す();

				if( b全画面からウィンドウへの切替えである || ( b初めての生成 && bウィンドウモードにする ) )
					this.tマウスカーソルを表示する();
			}
			//-----------------
			#endregion

			#region [ CTexture の持つ画面情報の更新 ]
			//-----------------
			CTexture.sz論理画面 = sz論理画面;
			CTexture.sz物理画面 = this.Window.ClientSize;

			Vector2 vc論理画面を1とする場合の物理画面の倍率 = new Vector2() {
				X = (float) CTexture.sz物理画面.Width / CTexture.sz論理画面.Width,
				Y = (float) CTexture.sz物理画面.Height / CTexture.sz論理画面.Height,
			};

			if( vc論理画面を1とする場合の物理画面の倍率.X < vc論理画面を1とする場合の物理画面の倍率.Y )
			{
				// (A) 物理と論理の横幅を一致させる。

				CTexture.f画面比率 = vc論理画面を1とする場合の物理画面の倍率.X;		// X
				CTexture.rc物理画面描画領域 = new Rectangle();
				CTexture.rc物理画面描画領域.Width = CTexture.sz物理画面.Width;
				CTexture.rc物理画面描画領域.Height = (int) ( CTexture.sz論理画面.Height * CTexture.f画面比率 );
				CTexture.rc物理画面描画領域.X = 0;
				CTexture.rc物理画面描画領域.Y = ( CTexture.sz物理画面.Height - CTexture.rc物理画面描画領域.Height ) / 2;
			}
			else
			{
				// (B) 物理と論理の縦幅を一致させる。

				CTexture.f画面比率 = vc論理画面を1とする場合の物理画面の倍率.Y;		// Y
				CTexture.rc物理画面描画領域 = new Rectangle();
				CTexture.rc物理画面描画領域.Width = (int) ( CTexture.sz論理画面.Width * CTexture.f画面比率 );
				CTexture.rc物理画面描画領域.Height = CTexture.sz物理画面.Height;
				CTexture.rc物理画面描画領域.X = ( CTexture.sz物理画面.Width - CTexture.rc物理画面描画領域.Width ) / 2;
				CTexture.rc物理画面描画領域.Y = 0;
			}

			Trace.TraceInformation( "Direct3D9 デバイス：論理画面:{0}x{1}, 物理画面:{2}x{3}, 画面比率:{4}, 物理画面描画領域:({5},{6})-({7}x{8})",
				CTexture.sz論理画面.Width, CTexture.sz論理画面.Height,
				CTexture.sz物理画面.Width, CTexture.sz物理画面.Height,
				CTexture.f画面比率,
				CTexture.rc物理画面描画領域.Left, CTexture.rc物理画面描画領域.Top, CTexture.rc物理画面描画領域.Right, CTexture.rc物理画面描画領域.Bottom );
			//-----------------
			#endregion

			return true;
		}
		public bool tDirect3Dデバイスを生成・変更・リセットする( CD3DSettings newD3DSettings, Size sz論理画面, uint wsウィンドウモード時のウィンドウスタイル, uint ws全画面モード時のウィンドウスタイル )
		{
			return this.tDirect3Dデバイスを生成・変更・リセットする( newD3DSettings, sz論理画面, wsウィンドウモード時のウィンドウスタイル, ws全画面モード時のウィンドウスタイル, true );
		}
		public bool tDirect3Dデバイスを生成・変更・リセットする( CD3DSettings newD3DSettings, Size sz論理画面 )
		{
			return this.tDirect3Dデバイスを生成・変更・リセットする( newD3DSettings, sz論理画面, uint.MaxValue, uint.MaxValue, true );
		}
		public bool tDirect3Dデバイスを生成・変更・リセットする( CD3DSettings newD3DSettings )
		{
			return this.tDirect3Dデバイスを生成・変更・リセットする( newD3DSettings, Size.Empty, uint.MaxValue, uint.MaxValue, true );
		}

		public void tDirect3Dデバイスをクリアする()
		{
			this.D3D9Device.Clear( ClearFlags.Target, this.colorデバイスクリア色, 0.0f, 0 );
		}

		public void tマウスカーソルを表示する()
		{
			if( !this.bマウスカーソル表示中 )
			{
				Cursor.Show();
				this.bマウスカーソル表示中 = true;
			}
		}
		public void tマウスカーソルを消す()
		{
			if( this.bマウスカーソル表示中 )
			{
				Cursor.Hide();
				this.bマウスカーソル表示中 = false;
			}
		}


		protected void t進行スレッド処理()
		{
			while( true )
			{
				lock( this.obj排他用 )
				{
					#region [ スレッド終了チェック。]
					//-----------------
					if( this.bアプリケーションを終了する ||
						this.bWindowClose済み )
						break;	// スレッド終了。
					//-----------------
					#endregion

					this.On進行();
				}


				// 一定期間スリープ。

				Thread.Sleep( 1 );		// 固定。
			}
		}
		protected void t描画スレッド処理()
		{
			const bool b時間計測 = true;	// デバッグ用時間計測を行うならtrueにする。


			#region [ 時間計測（デバッグ用）]
			//-----------------
			CTimer timer = null;
			if( b時間計測 )
				timer = new CTimer( CTimer.E種別.MultiMedia );
			long n開始 = CTimer.n未使用;
			long nPresent開始 = CTimer.n未使用;
			long n終了 = CTimer.n未使用;
			//-----------------
			#endregion

			while( true )
			{
				// メッセージ処理。

				#region [ Win32メッセージをあるだけ処理。]
				//-----------------
                /*
				CWin32.WindowMessage msg;
				while( CWin32.PeekMessage( out msg, IntPtr.Zero, 0, 0, CWin32.PM_REMOVE ) )
				{
					if( msg.msg == CWin32.WM_QUIT )
						return;		// GUIスレッド終了。

					CWin32.TranslateMessage( ref msg );
					CWin32.DispatchMessage( ref msg );
				}
                */
				//-----------------
				#endregion


				// 描画。

				lock( this.obj排他用 )
				{
					#region [ 終了チェック。]
					//-----------------
					if( this.bアプリケーションを終了する )
						return;		// スレッド終了。
					//-----------------
					#endregion

					#region [ デバイスが消失してたら復元を試みる。]
					//-----------------
					if( this.bデバイス消失中 )
					{
						#region [ ウィンドウが最小化されている → 待機 ]
						//-----------------
						if( this.Window.WindowState == FormWindowState.Minimized )
						{
							Trace.TraceWarning( "ウィンドウが最小化されています。描画を5秒待機します。" );
							Thread.Sleep( 5000 );
							continue;
						}
						//-----------------
						#endregion

						if( this.D3D9Device.TestCooperativeLevel() == ResultCode.DeviceNotReset )	// リセット可能
						{
							// 新しい Direct3D9 デバイスを作成。

							var newSettings = this.currentD3DSettings.Clone();

							#region [ ウィンドウモードの場合、BackBuffer フォーマットと Desktop フォーマットを一致させる。]
							//-----------------
							if( this.currentD3DSettings.PresentParameters.Windowed )
							{
								DisplayMode dm = this.Direct3D.GetAdapterDisplayMode( this.currentD3DSettings.nAdaptor );
								if( this.currentD3DSettings.PresentParameters.BackBufferFormat != dm.Format )
								{
									// BackBuffer フォーマットが Desktopフォーマットと異なるなら後者に合わせる。
									newSettings.PresentParameters.BackBufferFormat = dm.Format;
								}
							}
							//-----------------
							#endregion

							try
							{
								if( this.tDirect3Dデバイスを生成・変更・リセットする( newSettings, CTexture.sz論理画面 ) )
								{
									// 作成成功。

									Trace.TraceInformation( "Direct3Dデバイスを復元しました。" );
									this.bデバイス消失中 = false;
								}
							}
							catch
							{
								Trace.TraceInformation( "復元に失敗しました。2秒後に再度試みます。" );
								Thread.Sleep( 2000 );
								continue;
							}
						}
						else
						{
							Trace.TraceInformation( "まだ復元できない状態です。2秒後に再度試みます。" );
							Thread.Sleep( 2000 );
							continue;
						}
					}
					//-----------------
					#endregion

					#region [ 時間計測（デバッグ用）]
					//-----------------
					if( b時間計測 )
						n開始 = timer.nシステム時刻ms;
					//-----------------
					#endregion

					try
					{
						this.On描画();				// BeginScene～EndScene はこの中で。
					}
					catch( Direct3D9Exception e )	// デバイス関連以外の例外はそのまま発出する。
					{
						#region [ DeviceLost 以外の例外が発生したらアプリを終了する。]
						//-----------------
						if( e.ResultCode == ResultCode.DeviceLost )
						{
							Trace.TraceInformation( "Direct3Dデバイスを消失しました。" );
							this.bデバイス消失中 = true;
						}
						else
						{
							Trace.TraceInformation( "Direct3Dデバイスにおいて、DeviceLost 以外の例外が発生しました。" );
							throw;
						}
						//-----------------
						#endregion
					}
				}

				// 表示。（lock の範囲外）

				if( NOT( this.bPresent停止 ) &&
					this.D3D9Device != null &&
					NOT( this.bアプリケーションを終了する ) &&
					NOT( this.bWindowClose済み ) )
				{
					#region [ 時間計測（デバッグ用）]
					//-----------------
					if( b時間計測 )
						nPresent開始 = timer.nシステム時刻ms;
					//-----------------
					#endregion

					try
					{
						this.D3D9Device.Present();
					}
					catch( Direct3D9Exception e )	// デバイス関連以外の例外はそのまま発出する。
					{
						#region [ DeviceLost 以外の例外が発生したらアプリを終了する。]
						//-----------------
						if( e.ResultCode == ResultCode.DeviceLost )
						{
							Trace.TraceInformation( "Direct3Dデバイスを消失しました。" );

							lock( this.obj排他用 )
							{
								this.bデバイス消失中 = true;
							}
						}
						else
						{
							#region [ アプリ終了時の例外なら無視する。それ以外は発出。]
							//-----------------
							bool bアプリ終了中 = false;
							lock( this.obj排他用 )
								bアプリ終了中 = this.bアプリケーションを終了する;

							if( !bアプリ終了中 )
							{
								Trace.TraceInformation( "Direct3Dデバイスにおいて、DeviceLost 以外の例外が発生しました。" );
								throw;
							}
							else
							{
								// アプリ終了時の例外なので無視。
							}
							//-----------------
							#endregion
						}
						//-----------------
						#endregion
					}

					#region [ 時間計測（デバッグ用）]
					//-----------------
					if( b時間計測 )
					{
						n終了 = timer.nシステム時刻ms;

						if( ( n終了 - n開始 ) > 18 )
						{
							Debug.WriteLine( string.Format( "{0}(描画)+{1}(Present) = {2}",
								nPresent開始 - n開始,
								n終了 - nPresent開始,
								n終了 - n開始
								) );
						}
					}
					//-----------------
					#endregion
				}
			}
		}
		protected void tフロー制御スレッド処理()
		{
			this.Onフロー制御();	// ループごと子クラスに丸投げ。
		}

	
		// 子クラスで override するメソッド群

		/// <summary>
		/// <para>メインウィンドウの生成と各種初期化を行う。</para>
		/// <para>Direct3D の生成の後に呼び出される。</para>
		/// <para>エラー等でアプリを終了したい場合は例外を発生させ、正常に（無言で）終了したい場合は this.Window を null にして return すること。</para>
		/// </summary>
		protected virtual void On初期化() { }

		/// <summary>
		/// <para>Direct3Dデバイスに対する設定を行う。</para>
		/// <para>デバイスのリセット時や再作成時に呼び出される。</para>
		/// </summary>
		protected virtual void OnD3Dデバイスステータスの初期化()
		{
			// this.Device.SetTransform()
			// this.Device.SetRenderState()
			// this.Device.SetTextureStageState() 
			// etc,.
		}



		/// <summary>
		/// <para>進行処理を行う。</para>
		/// <para>ロックを得た進行スレッドにより実行される。</para>
		/// </summary>
		protected virtual void On進行() { }

		/// <summary>
		/// <para>描画処理を行う。</para>
		/// <para>ロックを得た描画スレッドにより実行される。</para>
		/// <para>BeginScene() と EndScene() の間に呼び出される。</para>
		/// <para>そのため、Direct3Dデバイスの変更を伴うような操作は行わないこと。</para>
		/// </summary>
		protected virtual void On描画() { }

		/// <summary>
		/// <para>アプリケーション全体のフローについて、現在の状態を管理し、状態に応じて各スレッドに指示を出す。</para>
		/// </summary>
		protected virtual void Onフロー制御() { }



		/// <summary>
		/// <para>終了処理を行う。</para>
		/// <para>メインループを抜けた後、一番最後に呼び出される。</para>
		/// <para>Direct3D, Device は Dispose 不要。</para>
		/// </summary>
		protected virtual void On終了() { }

		/// <summary>
		/// <para>Managed リソースを生成する。</para>
		/// <para>デバイスを生成した直後に呼び出される。（リセットの直後には呼び出されない。）</para>
		/// </summary>
		protected virtual void OnManageリソースを生成する() { }

		/// <summary>
		/// <para>Managed リソースを解放する。</para>
		/// <para>デバイスを破棄する直前に呼び出される。（リセットの直前には呼び出されない。）</para>
		/// </summary>
		protected virtual void OnManageリソースを解放する() { }

		/// <summary>
		/// <para>Unmanaged リソースがあれば、それを生成する。</para>
		/// <para>デバイスロストした直後に呼び出される。</para>
		/// <para>連続して呼び出されても問題ないようにコーディングすること。</para>
		/// </summary>
		protected virtual void OnUnmanageリソースを生成する() { }

		/// <summary>
		/// <para>Unmanaged リソースがあれば、それを解放する。</para>
		/// <para>デバイスをリセットまたは破棄する直前に呼び出される。</para>
		/// </summary>
		protected virtual void OnUnmanageリソースを解放する() { }

		#region [ IDisposable 実装 ]
		//-----------------
		public void Dispose()
		{
			C共通.tDisposeする( this.D3D9Device ); this.D3D9Device = null;
			C共通.tDisposeする( this.Direct3D ); this.Direct3D = null;
		}
		//-----------------
		#endregion

		#region [ protected ]
		//-----------------
		protected volatile bool bアプリケーションを終了する = false;
		protected volatile bool bWindowClose済み = false;
		/// <summary>
		/// <para>On描画()の後にPresent()を行うか否かを指定する。</para>
		/// <para>例えば、Direct3Dデバイスの切替え中にはPresentは停止しなければならない。</para>
		/// </summary>
		protected volatile bool bPresent停止 = false;

		protected class Cウィンドウ状態
		{
			public CWin32.WINDOWPLACEMENT WindowPlacement;
			public bool bTopMostWindow;
		}
		protected Cウィンドウ状態 mウインドウモード時の状態のバックアップ = null;
		protected bool bマウスカーソル表示中 = true;
		protected readonly Color4 colorデバイスクリア色 = new Color4( 1f, 0f, 0f, 0f );
		protected const uint wsウィンドウスタイル =
			(uint)CWin32.WS_OVERLAPPED |		// オーバラップウィンドウ。
            (uint)CWin32.WS_BORDER |			// 境界を持つ。
            (uint)CWin32.WS_CAPTION |			// タイトルバーを持つ。（WS_DLGFRAME スタイルと一緒に使うことは不可。）
            (uint)CWin32.WS_THICKFRAME |		// ウィンドウのサイズ変更に使うことができる太い枠を持つ。
            (uint)CWin32.WS_CLIPCHILDREN |	// 親ウィンドウの内部で描画するときに、子ウィンドウが占める領域を除外する。親ウィンドウを作成するときに使う。
            (uint)CWin32.WS_SYSMENU |			// タイトルバーにコントロールメニューボックスと「閉じる」ボタンを持つ。
            (uint)CWin32.WS_EX_APPWINDOW;		// ウィンドウが表示されているときには、必ずトップレベルウィンドウがタスクバー上に置かれる。
		protected const uint ws全画面スタイル =
            (uint)CWin32.WS_POPUP |			// ポップアップウィンドウ。（タイトルバー、枠なし。クライアント領域のみ。）
            (uint)CWin32.WS_VISIBLE;			// 初期状態で可視。
		//-----------------
		#endregion

		#region [ private ]
		//-----------------
		private volatile RenderForm _Window = null;
		private volatile Direct3D _Direct3D = null;
		private volatile Device _D3D9Device = null;
		private volatile bool bデバイス消失中 = false;

		private bool NOT( bool a ) { return !a; }
		//-----------------
		#endregion
	}
}
