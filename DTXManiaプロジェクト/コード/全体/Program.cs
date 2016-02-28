using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using FDK;

namespace DTXMania
{
	internal class Program
	{
		#region [ 二重機動チェック、DLL存在チェック ]
		//-----------------------------
		private static Mutex mutex二重起動防止用;

		private static bool tDLLの存在チェック( string strDll名, string str存在しないときに表示するエラー文字列jp, string str存在しないときに表示するエラー文字列en )
		{
			string str存在しないときに表示するエラー文字列 = (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ja") ?
				str存在しないときに表示するエラー文字列jp : str存在しないときに表示するエラー文字列en;
			IntPtr hModule = LoadLibrary( strDll名 );
			if( hModule == IntPtr.Zero )
			{
				MessageBox.Show( str存在しないときに表示するエラー文字列, "DTXMania runtime error", MessageBoxButtons.OK, MessageBoxIcon.Hand );
				return false;
			}
			FreeLibrary( hModule );
			return true;
		}

		#region [DllImport]
		[DllImport( "kernel32", CharSet = CharSet.Unicode, SetLastError = true )]
		internal static extern void FreeLibrary( IntPtr hModule );

		[DllImport( "kernel32", CharSet = CharSet.Unicode, SetLastError = true )]
		internal static extern IntPtr LoadLibrary( string lpFileName );
		#endregion
		//-----------------------------
		#endregion

		[STAThread]
		private static void Main()
		{
			mutex二重起動防止用 = new Mutex( false, "DTXManiaMutex" );

			if( mutex二重起動防止用.WaitOne( 0, false ) )
			{
				string newLine = Environment.NewLine;
				bool flag = false;

				#region [DLLの存在チェック]
				if (!tDLLの存在チェック("SlimDX" + CDTXMania.SLIMDXDLL,
					"SlimDX" + CDTXMania.SLIMDXDLL + ".dll またはその依存するdllが存在しません。" + newLine + "DTXManiaをダウンロードしなおしてください。",
					"SlimDX" + CDTXMania.SLIMDXDLL + ".dll, or its depended DLL, is not found." + newLine + "Please download DTXMania again."
					)) flag = true;
				if (!tDLLの存在チェック("FDK.dll",
					"FDK.dll またはその依存するdllが存在しません。" + newLine + "DTXManiaをダウンロードしなおしてください。",
					"FDK.dll, or its depended DLL, is not found." + newLine + "Please download DTXMania again."
					) ) flag = true;
				if( !tDLLの存在チェック( "xadec.dll",
					"xadec.dll が存在しません。" + newLine + "DTXManiaをダウンロードしなおしてください。",
					"xadec.dll is not found." + newLine + "Please download DTXMania again."
					) ) flag = true;
				if( !tDLLの存在チェック( "SoundDecoder.dll",
					"SoundDecoder.dll またはその依存するdllが存在しません。" + newLine + "DTXManiaをダウンロードしなおしてください。",
					"SoundDecoder.dll, or its depended DLL, is not found." + newLine + "Please download DTXMania again."
					) ) flag = true;
				if (!tDLLの存在チェック(CDTXMania.D3DXDLL,
					CDTXMania.D3DXDLL + " が存在しません。" + newLine + "DirectX Redist フォルダの DXSETUP.exe を実行し、" + newLine + "必要な DirectX ランタイムをインストールしてください。",
					CDTXMania.D3DXDLL + " is not found." + newLine + "Please execute DXSETUP.exe in \"DirectX Redist\" folder, to install DirectX runtimes required for DTXMania."
					)) flag = true;
				if ( !tDLLの存在チェック( "bass.dll",
					"bass.dll が存在しません。" + newLine + "DTXManiaをダウンロードしなおしてください。",
					"baas.dll is not found." + newLine + "Please download DTXMania again."
					) ) flag = true;
				if ( !tDLLの存在チェック( "Bass.Net.dll",
					"Bass.Net.dll が存在しません。" + newLine + "DTXManiaをダウンロードしなおしてください。",
					"Bass.Net.dll is not found." + newLine + "Please download DTXMania again."
					) ) flag = true;
				if ( !tDLLの存在チェック( "bassmix.dll",
					"bassmix.dll を読み込めません。bassmix.dll か bass.dll が存在しません。" + newLine + "DTXManiaをダウンロードしなおしてください。",
					"bassmix.dll is not loaded. bassmix.dll or bass.dll must not exist." + newLine + "Please download DTXMania again."
					) ) flag = true;
				if ( !tDLLの存在チェック( "bassasio.dll",
					"bassasio.dll を読み込めません。bassasio.dll か bass.dll が存在しません。" + newLine + "DTXManiaをダウンロードしなおしてください。",
					"bassasio.dll is not loaded. bassasio.dll or bass.dll must not exist." + newLine + "Please download DTXMania again."
					) ) flag = true;
				if ( !tDLLの存在チェック( "basswasapi.dll",
					"basswasapi.dll を読み込めません。basswasapi.dll か bass.dll が存在しません。" + newLine + "DTXManiaをダウンロードしなおしてください。",
					"basswasapi.dll is not loaded. basswasapi.dll or bass.dll must not exist." + newLine + "Please download DTXMania again."
					) ) flag = true;
				if ( !tDLLの存在チェック( "bass_fx.dll",
					"bass_fx.dll を読み込めません。bass_fx.dll か bass.dll が存在しません。" + newLine + "DTXManiaをダウンロードしなおしてください。",
					"bass_fx.dll is not loaded. bass_fx.dll or bass.dll must not exist." + newLine + "Please download DTXMania again."
					) ) flag = true;
				if ( !tDLLの存在チェック( "DirectShowLib-2005.dll",
					"DirectShowLib-2005.dll が存在しません。" + newLine + "DTXManiaをダウンロードしなおしてください。",
					"DirectShowLib-2005.dll is not found." + newLine + "Please download DTXMania again."
					) ) flag = true;
				#endregion
				if (!flag)
				{
#if DEBUG && TEST_ENGLISH
					Thread.CurrentThread.CurrentCulture = new CultureInfo( "en-US" );
#endif

					DWM.EnableComposition(false);	// Disable AeroGrass temporally

					// BEGIN #23670 2010.11.13 from: キャッチされない例外は放出せずに、ログに詳細を出力する。
					// BEGIM #24606 2011.03.08 from: DEBUG 時は例外発生箇所を直接デバッグできるようにするため、例外をキャッチしないようにする。
#if !DEBUG
					try
#endif
					{
						using( var mania = new CDTXMania() )
							mania.Run();

						Trace.WriteLine( "" );
						Trace.WriteLine( "遊んでくれてありがとう！" );
					}
#if !DEBUG
					catch( Exception e )
					{
						Trace.WriteLine( "" );
						Trace.Write( e.ToString() );
						Trace.WriteLine( "" );
						Trace.WriteLine( "エラーだゴメン！（涙" );
						MessageBox.Show( e.ToString(), "DTXMania Error", MessageBoxButtons.OK, MessageBoxIcon.Error );	// #23670 2011.2.28 yyagi to show error dialog
					}
#endif
					// END #24606 2011.03.08 from
					// END #23670 2010.11.13 from

					if( Trace.Listeners.Count > 1 )
						Trace.Listeners.RemoveAt( 1 );
				}

				// BEGIN #24615 2011.03.09 from: Mutex.WaitOne() が true を返した場合は、Mutex のリリースが必要である。
				
				mutex二重起動防止用.ReleaseMutex();
				mutex二重起動防止用 = null;
				
				// END #24615 2011.03.09 from
			}
		}
	}
}
