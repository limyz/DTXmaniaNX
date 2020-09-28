using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FDK
{
	public unsafe class BASSThreadedMixerLibraryWrapper
	{

		//* Parameters
		public static readonly int BASSTM_PARAMETER_NO_THREAD_FOR_1_SOURCE	= 1; //* Default = True
		public static readonly int BASSTM_PARAMETER_THREADS_PRIORITY		= 2; //* Affects only sources added after setting this parameter
		public static readonly int BASSTM_THREADS_PRIORITY_Idle				= 0;
		public static readonly int BASSTM_THREADS_PRIORITY_Lowest			= 1;
		public static readonly int BASSTM_THREADS_PRIORITY_Lower			= 2;
		public static readonly int BASSTM_THREADS_PRIORITY_Normal			= 3;
		public static readonly int BASSTM_THREADS_PRIORITY_Higher			= 4; //* Default
		public static readonly int BASSTM_THREADS_PRIORITY_Highest			= 5;
		public static readonly int BASSTM_THREADS_PRIORITY_TimeCritical		= 6;

		//* .dll file name
		public static readonly string FILENAME_DLL_BASS_THREADED_MIXER_LIBRARY = @"dll\BASSThreadedMixer.dll";
		//public static readonly int BASSARLIBCALL __stdcall
		//public static readonly int GETBASSTMLIBFUNCTION(f) *((void**)&f) = GetProcAddress(BASSThreadedMixerLibraryDLLHandle, #f)


		#region [DllImport]
		[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern void FreeLibrary(IntPtr hModule);

		[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern IntPtr LoadLibrary(string lpFileName);
		#endregion

		#region [ BASSThreadedMixer.dll インポート ]
		//-----------------
		[return: MarshalAs(UnmanagedType.U4)]
		[DllImport("BASSThreadedMixer.dll", EntryPoint = "BASS_ThreadedMixer_Create", CallingConvention = CallingConvention.StdCall)]
		public static extern Int32 BASS_ThreadedMixer_Create(Int32 Freq, Int32 Chans, Int32 Flags, out IntPtr ThreadedMixerHandle);

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("BASSThreadedMixer.dll", EntryPoint = "BASS_ThreadedMixer_AddSource", CallingConvention = CallingConvention.StdCall)]
		public static extern bool BASS_ThreadedMixer_AddSource(IntPtr ThreadedMixerHandle, Int32 SourceChannel, IntPtr Matrix);
		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("BASSThreadedMixer.dll", EntryPoint = "BASS_ThreadedMixer_RemoveSource", CallingConvention = CallingConvention.StdCall)]
		public static extern bool BASS_ThreadedMixer_RemoveSource(IntPtr ThreadedMixerHandle, Int32 SourceChannel);
		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("BASSThreadedMixer.dll", EntryPoint = "BASS_ThreadedMixer_SetMatrix", CallingConvention = CallingConvention.StdCall)]
		public static extern bool BASS_ThreadedMixer_SetMatrix(IntPtr ThreadedMixerHandle, Int32 SourceChannel, IntPtr Matrix, Single FadeTime);
		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("BASSThreadedMixer.dll", EntryPoint = "BASS_ThreadedMixer_SetParameter", CallingConvention = CallingConvention.StdCall)]
		public static extern bool BASS_ThreadedMixer_SetParameter(IntPtr ThreadedMixerHandle, Int32 Parameter, Int32 Value);

		//typedef DWORD(BASSARLIBCALL* t_BASS_ThreadedMixer_Create)(DWORD Freq, DWORD Chans, DWORD Flags, HBASSTM* ThreadedMixerHandle);
		//typedef BOOL(BASSARLIBCALL* t_BASS_ThreadedMixer_AddSource)(HBASSTM ThreadedMixerHandle, DWORD SourceChannel, void* Matrix);
		//typedef BOOL(BASSARLIBCALL* t_BASS_ThreadedMixer_RemoveSource)(HBASSTM ThreadedMixerHandle, DWORD SourceChannel);
		//typedef BOOL(BASSARLIBCALL* t_BASS_ThreadedMixer_SetMatrix)(HBASSTM ThreadedMixerHandle, DWORD SourceChannel, void* Matrix, float FadeTime);
		//typedef BOOL(BASSARLIBCALL* t_BASS_ThreadedMixer_SetParameter)(HBASSTM ThreadedMixerHandle, DWORD Parameter, int Value);

		//-----------------
		#endregion


		private static IntPtr BASSThreadedMixerLibraryDLLHandle;

		public static bool InitBASSThreadedMixerLibrary()
		{
			//#if _WIN32
			BASSThreadedMixerLibraryDLLHandle = LoadLibrary(FILENAME_DLL_BASS_THREADED_MIXER_LIBRARY);
			//#else //* OSX
			//BASSThreadedMixerLibraryDLLHandle = dlopen(FILENAME_DLL_BASS_THREADED_MIXER_LIBRARY, RTLD_NOW);
			//#endif

			if (null != BASSThreadedMixerLibraryDLLHandle)
			{
				throw new Exception("BASSThreadedMixer.dllの組み込みに失敗しました。");

				//GETBASSTMLIBFUNCTION(BASS_ThreadedMixer_Create);
				//GETBASSTMLIBFUNCTION(BASS_ThreadedMixer_AddSource);
				//GETBASSTMLIBFUNCTION(BASS_ThreadedMixer_RemoveSource);
				//GETBASSTMLIBFUNCTION(BASS_ThreadedMixer_SetMatrix);
				//GETBASSTMLIBFUNCTION(BASS_ThreadedMixer_SetParameter);

				//if ((NULL == BASS_ThreadedMixer_Create)
				//	|| (NULL == BASS_ThreadedMixer_AddSource)
				//	|| (NULL == BASS_ThreadedMixer_RemoveSource)
				//	|| (NULL == BASS_ThreadedMixer_SetMatrix)
				//	|| (NULL == BASS_ThreadedMixer_SetParameter)
				//	)
				//{
				//	BASSThreadedMixerLibraryDLLLoaded = FALSE;
				//}
				//else
				//{
				//	BASSThreadedMixerLibraryDLLLoaded = TRUE;
				//}
			}

			return true;
		}

		public static bool FreeBASSThreadedMixerLibrary()
		{
			if (null != BASSThreadedMixerLibraryDLLHandle)
			{
//# ifdef _WIN32
				FreeLibrary(BASSThreadedMixerLibraryDLLHandle);
//#else //* OSX
//				BASSThreadedMixerLibraryDLLLoaded = dlclose(BASSThreadedMixerLibraryDLLHandle);
//#endif
			}
			return true;
		}
	}
}
