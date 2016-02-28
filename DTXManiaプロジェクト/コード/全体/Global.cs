using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.Direct3D9;
using FDK;

namespace DTXMania
{
	class Global
	{
		// 静的プロパティ

		public const int nチップの最大音量 = 4;
		public const int nチップの最小音量 = 1;
		public const int nドラムキットのチップ別音声多重度 = 2;

		public static readonly int nメジャー番号 = FileVersionInfo.GetVersionInfo( Assembly.GetExecutingAssembly().Location ).FileMajorPart;
		public static readonly int nマイナー番号 = FileVersionInfo.GetVersionInfo( Assembly.GetExecutingAssembly().Location ).FileMinorPart;
		public static readonly int nリビジョン番号 = FileVersionInfo.GetVersionInfo( Assembly.GetExecutingAssembly().Location ).FilePrivatePart;
		public static readonly int nビルド番号 = FileVersionInfo.GetVersionInfo( Assembly.GetExecutingAssembly().Location ).FileBuildPart;

		public static readonly Random Random = new Random( DateTime.Now.Millisecond );

		public static bool bウィンドウがアクティブである
		{
			get;
			set;	// this.Window のイベントハンドラ（匿名デリゲート）内で設定される。
		}
		public static bool bウィンドウがアクティブでない
		{
			get { return !bウィンドウがアクティブである; }
			set { bウィンドウがアクティブである = !value; }
		}

		public static CApp App
		{
			get;
			protected set;
		}
		public static Folder Folder
		{
			get;
			protected set;
		}
#if n
		public static PlayerMode PlayerMode
		{
			get;
			protected set;
		}
		public static Users Users
		{
			get;
			protected set;
		}
		public static Input Input
		{
			get;
			protected set;
		}
		public static Song Song
		{
			get;
			protected set;
		}
		public static Stage Stage
		{
			get;
			protected set;
		}
		public static VirtualDrums VirtualDrums
		{
			get;
			protected set;
		}
		public static Theme Theme
		{
			get;
			set;
		}
		public static EnvironmentParameters EnvironmentProperties
		{
			get;
			protected set;
		}
#endif
		public static ISoundDevice SoundDevice
		{
			get;
			set;
		}
		public static CSoundTimer rc演奏用タイマ = null;
		
#if n
		public static CAct英数字描画 Act英数字描画
		{
			get;
			protected set;
		}
		public static CMActフリップボード Actフリップボード
		{
			get;
			protected set;
		}
#endif


		// 静的メソッド

		public static void t初期化( CApp app )
		{
			Global.App = app;
			Global.Folder = new Folder();				// ユーザ個別フォルダのみユーザ依存
#if n
			Global.PlayerMode = new PlayerMode();
			Global.Users = Users.t読み込む( Global.Folder.strUsersXMLの絶対パス );
			Global.Input = new Input();
			Global.Song = new Song();
			Global.Stage = new Stage();
			Global.VirtualDrums = new VirtualDrums();	// ユーザ依存
			Global.Theme = new Theme();					// ユーザ依存
			Global.EnvironmentProperties = StrokeStyleT.EnvironmentParameters.t読み込む( Global.Folder.strEnvironmentPropertiesXMLの絶対パス );	// 環境依存
            Global.rc演奏用タイマ = null;				// Global.Bass 依存（つまりユーザ依存）
            Global.Actフリップボード = new CMActフリップボード( Global.Theme.TextureFormat );
			Global.Act英数字描画 = new CAct英数字描画();
#endif
			Global.SoundDevice = null;							// ユーザ依存
			Global.bウィンドウがアクティブである = false;

		}
		public static void t終了()
		{
#if n
			Global.EnvironmentProperties.t保存する( Global.Folder.strEnvironmentPropertiesXMLの絶対パス );
			Global.Actフリップボード.On非活性化(); Global.Actフリップボード = null;
			Global.Act英数字描画.On非活性化(); Global.Act英数字描画 = null;

			C共通.tDisposeする( Global.Theme ); Global.Theme = null;
			C共通.tDisposeする( Global.VirtualDrums ); Global.VirtualDrums = null;
			C共通.tDisposeする( Global.Stage ); Global.Stage = null;
			C共通.tDisposeする( Global.Song ); Global.Song = null;
			C共通.tDisposeする( Global.Input ); Global.Input = null;
			C共通.tDisposeする( Global.Users ); Global.Users = null;
			C共通.tDisposeする( Global.PlayerMode ); Global.PlayerMode = null;
#endif
			C共通.tDisposeする( Global.SoundDevice ); Global.SoundDevice = null;
			C共通.tDisposeする( ref Global.rc演奏用タイマ );	// Global.Bass を解放した後に解放すること。（Global.Bass で参照されているため）

			C共通.tDisposeする( Global.Folder ); Global.Folder = null;
			//C共通.tDisposeする( Global.App );	--> 呼び出し側でDisposeする。
		}

		public static CTexture tテクスチャを生成する( string fileName )
		{
			return Global.tテクスチャを生成する( fileName, false );
		}
		public static CTexture tテクスチャを生成する( string fileName, bool b黒を透過する )
		{
			try
			{
				return new CTexture( Global.App.D3D9Device, fileName, TextureFormat, b黒を透過する );
			}
			catch( FileNotFoundException )
			{
				Trace.TraceError( "ファイルが存在しません。[{0}]", Global.Folder.tファイルパスをマクロ付きパスに逆展開する( fileName ) );
				return null;
			}
			catch( Exception e )
			{
				C共通.t例外の詳細をログに出力する( e );
				Trace.TraceError( "テクスチャの生成に失敗しました。[{0}]", Global.Folder.tファイルパスをマクロ付きパスに逆展開する( fileName ) );
				return null;
			}
		}
		public static CTexture tテクスチャを生成する( int width, int height )
		{
			try
			{
				return new CTexture( Global.App.D3D9Device, width, height, Global.Theme.TextureFormat );
			}
			catch( Exception e )
			{
				C共通.t例外の詳細をログに出力する( e );
				Trace.TraceError( "空のテクスチャ({0}x{1})の生成に失敗しました。", width, height );
				return null;
			}
		}
		public static CTexture tテクスチャを生成する( Bitmap bitmap )
		{
			try
			{
				return new CTexture( Global.App.D3D9Device, bitmap, Global.Theme.TextureFormat );
			}
			catch( Exception e )
			{
				C共通.t例外の詳細をログに出力する( e );
				Trace.TraceError( "Bitmap からのテクスチャの生成に失敗しました。" );
				return null;
			}
		}

		public static CDirectShow t失敗してもスキップ可能なDirectShowを生成する( string filePath, IntPtr hWnd, bool bオーディオレンダラなし )
		{
			CDirectShow ds = null;

			try
			{
				ds = new CDirectShow( filePath, hWnd, bオーディオレンダラなし );
			}
			catch( FileNotFoundException )
			{
				Trace.TraceWarning( "ファイルが存在しません。[{0}]", Global.Folder.tファイルパスをマクロ付きパスに逆展開する( filePath ) );
				ds = null;	// Dispose はコンストラクタ内で実施済み
			}
			catch( Exception e )
			{
				C共通.t例外の詳細をログに出力する( e );
				Trace.TraceError( "DirectShow の生成に失敗しました。[{0}]", Global.Folder.tファイルパスをマクロ付きパスに逆展開する( filePath ) );
				ds = null;	// Dispose はコンストラクタ内で実施済み
			}

			return ds;
		}

		public static void tダイアログ表示準備()
		{
			Global.tダイアログ表示準備( true );
		}
		public static void tダイアログ表示準備( bool bサウンドあり )
		{
			#region [ 必要ならサウンドを再生。]
			//-----------------
			if( bサウンドあり )
				Global.Theme.tシステムサウンドを再生する( Theme.Eシステムサウンド.ダイアログ開く );
			//-----------------
			#endregion

			#region [ 現在全画面モードなら一時的にマウスカーソルを表示。]
			//-----------------
			if( Global.App.currentD3DSettings != null &&						// 最初の表示はD3D9Device作成前。
				!Global.App.currentD3DSettings.PresentParameters.Windowed )		// ユーザが未定かも知れないので Global.Users.Config.Windowed は使えない。
				Global.App.tマウスカーソルを表示する();
			//-----------------
			#endregion
		}
		public static void tダイアログ表示後始末()
		{
			Global.tダイアログ表示後始末( true );
		}
		public static void tダイアログ表示後始末( bool bサウンドあり )
		{
			#region [ 必要ならサウンドを再生。]
			//-----------------
			if( bサウンドあり )
				Global.Theme.tシステムサウンドを再生する( Theme.Eシステムサウンド.ダイアログ閉じる );
			//-----------------
			#endregion

			#region [ 全画面モードだったならマウスカーソルを非表示。]
			//-----------------
			if( Global.App.currentD3DSettings != null &&						// 最初の表示はD3D9Device作成前。
				!Global.App.currentD3DSettings.PresentParameters.Windowed )		// ユーザが未定かも知れないので Global.Users.Config.Windowed は使えない。
				Global.App.tマウスカーソルを消す();
			//-----------------
			#endregion
		}

		public static void t現在のユーザConfigに従ってサウンドデバイスとすべての既存サウンドを再構築する()
		{
			#region [ すでにサウンドデバイスと演奏タイマが構築されていれば解放する。]
			//-----------------
			if( Global.SoundDevice != null )
			{
				// すでに生成済みのサウンドがあれば初期状態に戻す。

				CSound.tすべてのサウンドを初期状態に戻す();

	
				// サウンドデバイスと演奏タイマを解放する。

				C共通.tDisposeする( Global.SoundDevice ); Global.SoundDevice = null;
				C共通.tDisposeする( ref Global.rc演奏用タイマ );	// Global.SoundDevice を解放した後に解放すること。（Global.SoundDevice で参照されているため）
			}
			//-----------------
			#endregion

			#region [ 新しいサウンドデバイスを構築する。]
			//-----------------
			switch( Global.Users.Config.SoundDeviceType )
			{
				case ESoundDeviceType.ExclusiveWASAPI:
					Global.SoundDevice = new CSoundDeviceWASAPI( CSoundDeviceWASAPI.Eデバイスモード.排他, Global.Users.Config.SoundDelayExclusiveWASAPI, Global.Users.Config.SoundUpdatePeriodExclusiveWASAPI );
					break;

				case ESoundDeviceType.SharedWASAPI:
					Global.SoundDevice = new CSoundDeviceWASAPI( CSoundDeviceWASAPI.Eデバイスモード.共有, Global.Users.Config.SoundDelaySharedWASAPI, Global.Users.Config.SoundUpdatePeriodSharedWASAPI );
					break;

				case ESoundDeviceType.ASIO:
					Global.SoundDevice = new CSoundDeviceASIO( Global.Users.Config.SoundDelayASIO );
					break;

				case ESoundDeviceType.DirectSound:
					Global.SoundDevice = new CSoundDeviceDirectSound( Global.App.Window.Handle, Global.Users.Config.SoundDelayDirectSound );
					break;

				default:
					throw new Exception( string.Format( "未対応の SoundDeviceType です。[{0}]", Global.Users.Config.SoundDeviceType.ToString() ) );
			}
			//-----------------
			#endregion
			#region [ 新しい演奏タイマを構築する。]
			//-----------------
			Global.rc演奏用タイマ = new CSoundTimer( Global.SoundDevice );
			//-----------------
			#endregion

			CSound.tすべてのサウンドを再構築する( Global.SoundDevice );		// すでに生成済みのサウンドがあれば作り直す。
		}
	}
}
