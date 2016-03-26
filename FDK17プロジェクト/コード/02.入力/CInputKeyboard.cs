using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using SlimDX;
using SlimDX.DirectInput;

namespace FDK
{
	public class CInputKeyboard : IInputDevice, IDisposable
	{
		// コンストラクタ

		public CInputKeyboard( IntPtr hWnd, DirectInput directInput )
		{
			this.e入力デバイス種別 = E入力デバイス種別.Keyboard;
			this.GUID = "";
			this.ID = 0;
			try
			{
				this.devKeyboard = new Keyboard( directInput );
				this.devKeyboard.SetCooperativeLevel( hWnd, CooperativeLevel.NoWinKey | CooperativeLevel.Foreground | CooperativeLevel.Nonexclusive );
				this.devKeyboard.Properties.BufferSize = 0x20;
				Trace.TraceInformation( this.devKeyboard.Information.ProductName + " を生成しました。" );
			}
			catch( DirectInputException )
			{
				if( this.devKeyboard != null )
				{
					this.devKeyboard.Dispose();
					this.devKeyboard = null;
				}
				Trace.TraceWarning( "Keyboard デバイスの生成に失敗しました。" );
				throw;
			}
			try
			{
				this.devKeyboard.Acquire();
			}
			catch( DirectInputException )
			{
			}

			for( int i = 0; i < this.bKeyState.Length; i++ )
				this.bKeyState[ i ] = false;

			//this.timer = new CTimer( CTimer.E種別.MultiMedia );
			this.list入力イベント = new List<STInputEvent>( 32 );
			// this.ct = new CTimer( CTimer.E種別.PerformanceCounter );
		}


		// メソッド

		#region [ IInputDevice 実装 ]
		//-----------------
		public E入力デバイス種別 e入力デバイス種別 { get; private set; }
		public string GUID { get; private set; }
		public int ID { get; private set; }
		public List<STInputEvent> list入力イベント { get; private set; }

		public void tポーリング( bool bWindowがアクティブ中, bool bバッファ入力を使用する )
		{
			for ( int i = 0; i < 256; i++ )
			{
				this.bKeyPushDown[ i ] = false;
				this.bKeyPullUp[ i ] = false;
			}

			if ( ( ( bWindowがアクティブ中 && ( this.devKeyboard != null ) ) && !this.devKeyboard.Acquire().IsFailure ) && !this.devKeyboard.Poll().IsFailure )
			{
				//this.list入力イベント = new List<STInputEvent>( 32 );
				this.list入力イベント.Clear();			// #xxxxx 2012.6.11 yyagi; To optimize, I removed new();
				int posEnter = -1;
				//string d = DateTime.Now.ToString( "yyyy/MM/dd HH:mm:ss.ffff" );

				if ( bバッファ入力を使用する )
				{
					#region [ a.バッファ入力 ]
					//-----------------------------
					var bufferedData = this.devKeyboard.GetBufferedData();
					if ( Result.Last.IsSuccess && bufferedData != null )
					{
						foreach ( KeyboardState data in bufferedData )
						{
							foreach ( Key key in data.PressedKeys )
							{
								STInputEvent item = new STInputEvent()
								{
									nKey = (int) key,
									b押された = true,
									b離された = false,
									nTimeStamp = CSound管理.rc演奏用タイマ.nサウンドタイマーのシステム時刻msへの変換( data.TimeStamp ),
									nVelocity = CInput管理.n通常音量
								};
								this.list入力イベント.Add( item );

								this.bKeyState[ (int) key ] = true;
								this.bKeyPushDown[ (int) key ] = true;

								//if ( item.nKey == (int) SlimDX.DirectInput.Key.Space )
								//{
								//    Trace.TraceInformation( "FDK(buffered): SPACE key registered. " + ct.nシステム時刻 );
								//}
							}
							foreach ( Key key in data.ReleasedKeys )
							{
								STInputEvent item = new STInputEvent()
								{
									nKey = (int) key,
									b押された = false,
									b離された = true,
									nTimeStamp = CSound管理.rc演奏用タイマ.nサウンドタイマーのシステム時刻msへの変換( data.TimeStamp ),
									nVelocity = CInput管理.n通常音量
								};
								this.list入力イベント.Add( item );

								this.bKeyState[ (int) key ] = false;
								this.bKeyPullUp[ (int) key ] = true;
							}
						}
					}
					//-----------------------------
					#endregion
				}
				else
				{
					#region [ b.状態入力 ]
					//-----------------------------
					KeyboardState currentState = this.devKeyboard.GetCurrentState();
					if ( Result.Last.IsSuccess && currentState != null )
					{
						foreach ( Key key in currentState.PressedKeys )
						{
							if ( this.bKeyState[ (int) key ] == false )
							{
								var ev = new STInputEvent()
								{
									nKey = (int) key,
									b押された = true,
									b離された = false,
									nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
									nVelocity = CInput管理.n通常音量,
								};
								this.list入力イベント.Add( ev );

								this.bKeyState[ (int) key ] = true;
								this.bKeyPushDown[ (int) key ] = true;

								//if ( (int) key == (int) SlimDX.DirectInput.Key.Space )
								//{
								//    Trace.TraceInformation( "FDK(direct): SPACE key registered. " + ct.nシステム時刻 );
								//}
							}
						}
						foreach ( Key key in currentState.ReleasedKeys )
						{
							if ( this.bKeyState[ (int) key ] == true )
							{
								var ev = new STInputEvent()
								{
									nKey = (int) key,
									b押された = false,
									b離された = true,
									nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
									nVelocity = CInput管理.n通常音量,
								};
								this.list入力イベント.Add( ev );

								this.bKeyState[ (int) key ] = false;
								this.bKeyPullUp[ (int) key ] = true;
							}
						}
					}
					//-----------------------------
					#endregion
				}
				#region [#23708 2011.4.8 yyagi Altが押されているときは、Enter押下情報を削除する -> 副作用が見つかり削除]
				//if ( this.bKeyState[ (int) SlimDX.DirectInput.Key.RightAlt ] ||
				//     this.bKeyState[ (int) SlimDX.DirectInput.Key.LeftAlt ] )
				//{
				//    int cr = (int) SlimDX.DirectInput.Key.Return;
				//    this.bKeyPushDown[ cr ] = false;
				//    this.bKeyPullUp[ cr ] = false;
				//    this.bKeyState[ cr ] = false;
				//}
				#endregion
			}
		}
		public bool bキーが押された( int nKey )
		{
			return this.bKeyPushDown[ nKey ];
		}
		public bool bキーが押されている( int nKey )
		{
			return this.bKeyState[ nKey ];
		}
		public bool bキーが離された( int nKey )
		{
			return this.bKeyPullUp[ nKey ];
		}
		public bool bキーが離されている( int nKey )
		{
			return !this.bKeyState[ nKey ];
		}
		//-----------------
		#endregion

		#region [ IDisposable 実装 ]
		//-----------------
		public void Dispose()
		{
			if( !this.bDispose完了済み )
			{
				if( this.devKeyboard != null )
				{
					this.devKeyboard.Dispose();
					this.devKeyboard = null;
				}
				//if( this.timer != null )
				//{
				//    this.timer.Dispose();
				//    this.timer = null;
				//}
				if ( this.list入力イベント != null )
				{
					this.list入力イベント = null;
				}
				this.bDispose完了済み = true;
			}
		}
		//-----------------
		#endregion


		// その他

		#region [ private ]
		//-----------------
		private bool bDispose完了済み;
		private bool[] bKeyPullUp = new bool[ 0x100 ];
		private bool[] bKeyPushDown = new bool[ 0x100 ];
		private bool[] bKeyState = new bool[ 0x100 ];
		private Keyboard devKeyboard;
		//private CTimer timer;
		//private CTimer ct;
		//-----------------
		#endregion
	}
}
