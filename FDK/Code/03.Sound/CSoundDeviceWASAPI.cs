using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Un4seen.Bass;
using Un4seen.BassWasapi;
using Un4seen.Bass.AddOn.Mix;
using Un4seen.Bass.Misc;

namespace FDK
{
	public class CSoundDeviceWASAPI : ISoundDevice
	{
		// プロパティ

		public ESoundDeviceType e出力デバイス
		{
			get;
			protected set;
		}
		public long n実出力遅延ms
		{
			get;
			protected set;
		}
		public long n実バッファサイズms
		{
			get;
			protected set;
		}

		public string strRecordFileType = null;

		// CSoundTimer 用に公開しているプロパティ

		public long n経過時間ms
		{
			get;
			protected set;
		}
		public long n経過時間を更新したシステム時刻ms
		{
			get;
			protected set;
		}
		public CTimer tmシステムタイマ
		{
			get;
			protected set;
		}

		public enum Eデバイスモード { 排他, 共有 }

		public int nMasterVolume
		{
			get
			{
				float f音量 = 0.0f;
				//if ( BassMix.BASS_Mixer_ChannelGetEnvelopePos( this.hMixer, BASSMIXEnvelope.BASS_MIXER_ENV_VOL, ref f音量 ) == -1 )
				//    return 100;
				//bool b = Bass.BASS_ChannelGetAttribute( this.hMixer, BASSAttribute.BASS_ATTRIB_VOL, ref f音量 );
				bool b = Bass.BASS_ChannelGetAttribute(this.hMixer, BASSAttribute.BASS_ATTRIB_VOL, ref f音量);
				if (!b)
				{
					BASSError be = Bass.BASS_ErrorGetCode();
					Trace.TraceInformation("WASAPI Master Volume Get Error: " + be.ToString());
				}
				else
				{
					Trace.TraceInformation("WASAPI Master Volume Get Success: " + (f音量 * 100));

				}
				return (int)(f音量 * 100);
			}
			set
			{
				// bool b = Bass.BASS_SetVolume( value / 100.0f );
				// →Exclusiveモード時は無効

				//				bool b = BassWasapi.BASS_WASAPI_SetVolume( BASSWASAPIVolume.BASS_WASAPI_VOL_SESSION, (float) ( value / 100 ) );
				//				bool b = BassWasapi.BASS_WASAPI_SetVolume( BASSWASAPIVolume.BASS_WASAPI_CURVE_WINDOWS, (float) ( value / 100 ) );
				bool b = Bass.BASS_ChannelSetAttribute(this.hMixer, BASSAttribute.BASS_ATTRIB_VOL, (float)(value / 100.0));
				// If you would like to have a volume control in exclusive mode too, and you're using the BASSmix add-on,
				// you can adjust the source's BASS_ATTRIB_VOL setting via BASS_ChannelSetAttribute.
				// しかし、hMixerに対するBASS_ChannelSetAttribute()でBASS_ATTRIB_VOLを変更: なぜか出力音量に反映されず

				// Bass_SetVolume(): BASS_ERROR_NOTAVIL ("no sound" deviceには適用不可)

				// Mixer_ChannelSetEnvelope():

				//var nodes = new BASS_MIXER_NODE[ 1 ] { new BASS_MIXER_NODE( 0, (float) value ) };
				//bool b = BassMix.BASS_Mixer_ChannelSetEnvelope( this.hMixer, BASSMIXEnvelope.BASS_MIXER_ENV_VOL, nodes );
				//bool b = Bass.BASS_ChannelSetAttribute( this.hMixer, BASSAttribute.BASS_ATTRIB_VOL, value / 100.0f );
				if (!b)
				{
					BASSError be = Bass.BASS_ErrorGetCode();
					Trace.TraceInformation("WASAPI Master Volume Set Error: " + be.ToString());
				}
				else
				{
					// int n = this.nMasterVolume;	
					// Trace.TraceInformation( "WASAPI Master Volume Set Success: " + value );

				}
			}
		}

		public string strDefaultSoundDeviceBusType
		{
			get;
			protected set;
		}


		// メソッド

		/// <summary>
		/// WASAPIの初期化
		/// </summary>
		/// <param name="mode"></param>
		/// <param name="n希望バッファサイズms">WASAPIのサウンドバッファサイズ</param>
		/// <param name="n更新間隔ms">サウンドバッファの更新間隔</param>
		public CSoundDeviceWASAPI(Eデバイスモード mode, long n希望バッファサイズms, long n更新間隔ms, string strRecordFileType, string strEncoderPath)
		{
			// 初期化。

			Trace.TraceInformation("BASS (WASAPI{0}) の初期化を開始します。", mode.ToString());

			this.e出力デバイス = ESoundDeviceType.Unknown;
			this.n実出力遅延ms = 0;
			this.n経過時間ms = 0;
			this.n経過時間を更新したシステム時刻ms = CTimer.nUnused;
			this.tmシステムタイマ = new CTimer(CTimer.EType.MultiMedia);
			this.b最初の実出力遅延算出 = true;

			#region [ BASS registration ]
			// BASS.NET ユーザ登録（BASSスプラッシュが非表示になる）。

			BassNet.Registration("dtxmaniaxgk@gmail.com", "2X9182021152222");
			#endregion

			#region [ BASS Version Check ]
			// BASS のバージョンチェック。
			int nBASSVersion = Utils.HighWord(Bass.BASS_GetVersion());
			if (nBASSVersion != Bass.BASSVERSION)
				throw new DllNotFoundException(string.Format("bass.dll のバージョンが異なります({0})。このプログラムはバージョン{1}で動作します。", nBASSVersion, Bass.BASSVERSION));

			int nBASSMixVersion = Utils.HighWord(BassMix.BASS_Mixer_GetVersion());
			if (nBASSMixVersion != BassMix.BASSMIXVERSION)
				throw new DllNotFoundException(string.Format("bassmix.dll のバージョンが異なります({0})。このプログラムはバージョン{1}で動作します。", nBASSMixVersion, BassMix.BASSMIXVERSION));

			int nBASSWASAPIVersion = Utils.HighWord(BassWasapi.BASS_WASAPI_GetVersion());
			if (nBASSWASAPIVersion != BassWasapi.BASSWASAPIVERSION)
				throw new DllNotFoundException(string.Format("basswasapi.dll のバージョンが異なります({0})。このプログラムはバージョン{1}で動作します。", nBASSWASAPIVersion, BassWasapi.BASSWASAPIVERSION));
			#endregion

			// BASS の設定。

			this.bIsBASSFree = true;
			Debug.Assert(Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 0),       // 0:BASSストリームの自動更新を行わない。（BASSWASAPIから行うため）
				string.Format("BASS_SetConfig() に失敗しました。[{0}", Bass.BASS_ErrorGetCode()));

			#region [ デバッグ用: BASSデバイスのenumerateと、ログ出力 ]
			//Trace.TraceInformation( "BASSデバイス一覧:" );
			//int defDevice = -1;
			//BASS_DEVICEINFO bdi;
			//for ( int n = 0; ( bdi = Bass.BASS_GetDeviceInfo( n ) ) != null; n++ )
			//{
			//	Trace.TraceInformation( "BASS Device #{0}: {1}: IsDefault={2}, flags={3}, type={4}",
			//		n,
			//		bdi.name,
			//		bdi.IsDefault, bdi.flags.ToString(), bdi.type,ToString()
			//	);

			//	//if ( bdi.IsDefault )
			//	//{
			//	//	defDevice = n;
			//	//	break;
			//	//}
			//}
			#endregion

			// BASS の初期化。

			int n周波数 = 48000;   // 仮決め。lデバイス（≠ドライバ）がネイティブに対応している周波数であれば何でもいい？ようだ。BASSWASAPIでデバイスの周波数は変えられる。いずれにしろBASSMXで自動的にリサンプリングされる。
								// BASS_Initは、WASAPI初期化の直前に行うよう変更。WASAPIのmix周波数を使って初期化することで、余計なリサンプリング処理を省き高速化するため。
								//if( !Bass.BASS_Init( nデバイス, n周波数, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero ) )
								//	throw new Exception( string.Format( "BASS (WASAPI) の初期化に失敗しました。(BASS_Init)[{0}]", Bass.BASS_ErrorGetCode().ToString() ) );


			#region [ デバッグ用: サウンドデバイスのenumerateと、ログ出力 ]
			//(デバッグ用)
			Trace.TraceInformation("サウンドデバイス一覧:");
			int a;
			string strDefaultSoundDeviceName = null;
			BASS_DEVICEINFO[] bassDevInfos = Bass.BASS_GetDeviceInfos();
			for (a = 0; a < bassDevInfos.GetLength(0); a++)
			{
				{
					Trace.TraceInformation("Sound Device #{0}: {1}: IsDefault={2}, isEnabled={3}, flags={4}, id={5}",
						a,
						bassDevInfos[a].name,
						bassDevInfos[a].IsDefault,
						bassDevInfos[a].IsEnabled,
						bassDevInfos[a].flags,
						bassDevInfos[a].id
					);
					if (bassDevInfos[a].IsDefault)
					{
						// これはOS標準のdefault device。後でWASAPIのdefault deviceと比較する。
						strDefaultSoundDeviceName = bassDevInfos[a].name;

						// 以下はOS標準 default deviceのbus type (PNPIDの頭の文字列)。上位側で使用する。
						string[] s = bassDevInfos[a].id.ToString().ToUpper().Split(new char[] { '#' });
						if (s != null && s[0] != null)
						{
							strDefaultSoundDeviceBusType = s[0];
						}
					}
				}
			}
			#endregion

			// BASS WASAPI の初期化。

			n周波数 = 0;           // デフォルトデバイスの周波数 (0="mix format" sample rate)
			int nチャンネル数 = 0;    // デフォルトデバイスのチャンネル数 (0="mix format" channels)
			this.tWasapiProc = new WASAPIPROC(this.tWASAPI処理);      // アンマネージに渡す delegate は、フィールドとして保持しておかないとGCでアドレスが変わってしまう。

			// WASAPIの更新間隔(period)は、バッファサイズにも影響を与える。
			// 更新間隔を最小にするには、BassWasapi.BASS_WASAPI_GetDeviceInfo( ndevNo ).minperiod の値を使えばよい。
			// これをやらないと、更新間隔ms=6ms となり、バッファサイズを 6ms x 4 = 24msより小さくできない。
			#region [ 既定の出力デバイスと設定されているWASAPIデバイスを検索し、更新間隔msを設定できる最小値にする ]
			int nDevNo = -1;
			BASS_WASAPI_DEVICEINFO deviceInfo;
			for (int n = 0; (deviceInfo = BassWasapi.BASS_WASAPI_GetDeviceInfo(n)) != null; n++)
			{
				// BASS_DEVICEINFOとBASS_WASAPI_DEVICEINFOで、IsDefaultとなっているデバイスが異なる場合がある。
				// (WASAPIでIsDefaultとなっているデバイスが正しくない場合がある)
				// そのため、BASS_DEVICEでIsDefaultとなっているものを探し、それと同じ名前のWASAPIデバイスを使用する。
				//if ( deviceInfo.IsDefault )
				if (deviceInfo.name == strDefaultSoundDeviceName)
				{
					nDevNo = n;
					#region [ 既定の出力デバイスの情報を表示 ]
					Trace.TraceInformation("WASAPI Device #{0}: {1}: IsDefault={2}, defPeriod={3}s, minperiod={4}s, mixchans={5}, mixfreq={6}",
						n,
						deviceInfo.name,
						deviceInfo.IsDefault, deviceInfo.defperiod, deviceInfo.minperiod, deviceInfo.mixchans, deviceInfo.mixfreq);
					#endregion
					break;
				}
			}
			if (nDevNo != -1)
			{
				Trace.TraceInformation("Start Bass_Init(device=0(fixed value: no sound), deviceInfo.mixfreq=" + deviceInfo.mixfreq + ", BASS_DEVICE_DEFAULT, Zero)");
				if (!Bass.BASS_Init(0, deviceInfo.mixfreq, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))  // device = 0:"no device": BASS からはデバイスへアクセスさせない。アクセスは BASSWASAPI アドオンから行う。
					throw new Exception(string.Format("BASS (WASAPI{0}) の初期化に失敗しました。(BASS_Init)[{1}]", mode.ToString(), Bass.BASS_ErrorGetCode().ToString()));

				// Trace.TraceInformation( "Selected Default WASAPI Device: {0}", deviceInfo.name );
				// Trace.TraceInformation( "MinPeriod={0}, DefaultPeriod={1}", deviceInfo.minperiod, deviceInfo.defperiod );

				// n更新間隔ms = ( mode == Eデバイスモード.排他 )?	Convert.ToInt64(Math.Ceiling(deviceInfo.minperiod * 1000.0f)) : Convert.ToInt64(Math.Ceiling(deviceInfo.defperiod * 1000.0f));
				// 更新間隔として、WASAPI排他時はminperiodより大きい最小のms値を、WASAPI共有時はdefperiodより大きい最小のms値を用いる
				// Win10では、更新間隔がminperiod以下だと、確実にBASS_ERROR_UNKNOWNとなる。

				//if ( n希望バッファサイズms <= 0 || n希望バッファサイズms < n更新間隔ms + 1 )
				//{
				//	n希望バッファサイズms = n更新間隔ms + 1; // 2013.4.25 #31237 yyagi; バッファサイズ設定の完全自動化。更新間隔＝バッファサイズにするとBASS_ERROR_UNKNOWNになるので+1する。
				//}
			}
			else
			{
				Trace.TraceError("Error: Default WASAPI Device is not found.");
			}
			#endregion

			#region [ デバッグ用: WASAPIデバイスのenumerateと、ログ出力 ]
			//(デバッグ用)
			Trace.TraceInformation("WASAPIデバイス一覧:");
			//int a, count = 0;
			BASS_WASAPI_DEVICEINFO wasapiDevInfo;
			for (a = 0; (wasapiDevInfo = BassWasapi.BASS_WASAPI_GetDeviceInfo(a)) != null; a++)
			{
				if ((wasapiDevInfo.flags & BASSWASAPIDeviceInfo.BASS_DEVICE_INPUT) == 0 // device is an output device (not input)
						&& (wasapiDevInfo.flags & BASSWASAPIDeviceInfo.BASS_DEVICE_ENABLED) != 0) // and it is enabled
				{
					Trace.TraceInformation("WASAPI Device #{0}: {1}: IsDefault={2}, defPeriod={3}s, minperiod={4}s, mixchans={5}, mixfreq={6}",
						a,
						wasapiDevInfo.name,
						wasapiDevInfo.IsDefault, wasapiDevInfo.defperiod, wasapiDevInfo.minperiod, wasapiDevInfo.mixchans, wasapiDevInfo.mixfreq);
				}
			}
		#endregion

		Retry:
			var flags = (mode == Eデバイスモード.排他) ?
				BASSWASAPIInit.BASS_WASAPI_AUTOFORMAT | BASSWASAPIInit.BASS_WASAPI_EXCLUSIVE :
				BASSWASAPIInit.BASS_WASAPI_AUTOFORMAT | BASSWASAPIInit.BASS_WASAPI_SHARED;  // 注: BASS_WASAPI_SHARED==0 なので、SHAREDの指定は意味なし
			if (COS.bIsWin7OrLater() && CSoundManager.bSoundUpdateByEventWASAPI)
			{
				flags |= BASSWASAPIInit.BASS_WASAPI_EVENT;  // Win7以降の場合は、WASAPIをevent drivenで動作させてCPU負荷減、レイテインシ改善
			}
			n周波数 = deviceInfo.mixfreq;
			nチャンネル数 = deviceInfo.mixchans;

			// 更新間隔として、WASAPI排他時はminperiodより大きい最小のms値を、WASAPI共有時はdefperiodより大きい最小のms値を用いる
			// (Win10のlow latency modeではない前提でまずは設定値を決める)
			float fPeriod = (mode == Eデバイスモード.排他) ? deviceInfo.minperiod : deviceInfo.defperiod;

			Trace.TraceInformation("arg: n希望バッファサイズms=" + n希望バッファサイズms);
			Trace.TraceInformation("arg: n更新間隔ms=" + n更新間隔ms);
			Trace.TraceInformation("fPeriod = " + fPeriod + " (排他時: minperiod, 共有時: defperiod。Win10 low latency audio考慮前)");

			float f更新間隔sec = (n更新間隔ms > 0) ? (n更新間隔ms / 1000.0f) : fPeriod;
			if (f更新間隔sec < fPeriod)
			{
				f更新間隔sec = fPeriod; // Win10では、更新間隔がminperiod以下だと、確実にBASS_ERROR_UNKNOWNとなる。
			}
			Trace.TraceInformation("f更新間隔sec=" + f更新間隔sec);
			// バッファサイズは、更新間隔より大きくする必要あり。(イコールだと、WASAPI排他での初期化時にBASS_ERROR_UNKNOWNとなる)
			// そのため、最低でも、更新間隔より1ms大きく設定する。
			float f希望バッファサイズsec = (n希望バッファサイズms > 0) ? (n希望バッファサイズms / 1000.0f) : fPeriod + 0.001f;
			if (f希望バッファサイズsec < fPeriod)
			{
				f希望バッファサイズsec = fPeriod + 0.001f;
			}
			// WASAPI排他時は、バッファサイズは更新間隔の4倍必要(event driven時は2倍)
			if (mode == Eデバイスモード.排他)
			{
				if ((flags & BASSWASAPIInit.BASS_WASAPI_EVENT) != BASSWASAPIInit.BASS_WASAPI_EVENT &&
					f希望バッファサイズsec < f更新間隔sec * 4)
				{
					f希望バッファサイズsec = f更新間隔sec * 4;
				}
				else if ((flags & BASSWASAPIInit.BASS_WASAPI_EVENT) == BASSWASAPIInit.BASS_WASAPI_EVENT &&
					f希望バッファサイズsec < f更新間隔sec * 2)
				{
					f希望バッファサイズsec = f更新間隔sec * 2;
				}
			}
			//else
			//if (COS.bIsWin10OrLater() && (mode == Eデバイスモード.共有))		// Win10 low latency shared mode support
			//{
			//	// バッファ自動設定をユーザーが望む場合は、periodを最小値にする。さもなくば、バッファサイズとしてユーザーが指定した値を、periodとして用いる。
			//	if (n希望バッファサイズms == 0)
			//	{
			//		f更新間隔sec = deviceInfo.minperiod;
			//	}
			//	else
			//	{
			//		f更新間隔sec = n希望バッファサイズms / 1000.0f;
			//		if (f更新間隔sec < deviceInfo.minperiod)
			//		{
			//			f更新間隔sec = deviceInfo.minperiod;
			//		}
			//	}
			//	f希望バッファサイズsec = 0.0f;
			//}

			Trace.TraceInformation("f希望バッファサイズsec=" + f希望バッファサイズsec + ", f更新間隔sec=" + f更新間隔sec + ": Win10 low latency audio 考慮後");

			Trace.TraceInformation("Start Bass_Wasapi_Init(device=" + nDevNo + ", freq=" + n周波数 + ", nchans=" + nチャンネル数 + ", flags=" + flags + "," +
				" buffer=" + f希望バッファサイズsec + ", period=" + f更新間隔sec + ")");
			if (BassWasapi.BASS_WASAPI_Init(nDevNo, n周波数, nチャンネル数, flags, f希望バッファサイズsec, f更新間隔sec, this.tWasapiProc, IntPtr.Zero))
			{
				if (mode == Eデバイスモード.排他)
				{
					#region [ 排他モードで作成成功。]
					//-----------------
					this.e出力デバイス = ESoundDeviceType.ExclusiveWASAPI;

					nDevNo = BassWasapi.BASS_WASAPI_GetDevice();
					deviceInfo = BassWasapi.BASS_WASAPI_GetDeviceInfo(nDevNo);
					var wasapiInfo = BassWasapi.BASS_WASAPI_GetInfo();
					int n1サンプルのバイト数 = 2 * wasapiInfo.chans; // default;
					switch (wasapiInfo.format)      // BASS WASAPI で扱うサンプルはすべて 32bit float で固定されているが、デバイスはそうとは限らない。
					{
						case BASSWASAPIFormat.BASS_WASAPI_FORMAT_8BIT: n1サンプルのバイト数 = 1 * wasapiInfo.chans; break;
						case BASSWASAPIFormat.BASS_WASAPI_FORMAT_16BIT: n1サンプルのバイト数 = 2 * wasapiInfo.chans; break;
						case BASSWASAPIFormat.BASS_WASAPI_FORMAT_24BIT: n1サンプルのバイト数 = 3 * wasapiInfo.chans; break;
						case BASSWASAPIFormat.BASS_WASAPI_FORMAT_32BIT: n1サンプルのバイト数 = 4 * wasapiInfo.chans; break;
						case BASSWASAPIFormat.BASS_WASAPI_FORMAT_FLOAT: n1サンプルのバイト数 = 4 * wasapiInfo.chans; break;
					}
					int n1秒のバイト数 = n1サンプルのバイト数 * wasapiInfo.freq;
					this.n実バッファサイズms = (long)(wasapiInfo.buflen * 1000.0f / n1秒のバイト数);
					this.n実出力遅延ms = 0;  // 初期値はゼロ
					Trace.TraceInformation("使用デバイス: #" + nDevNo + " : " + deviceInfo.name + ", flags=" + deviceInfo.flags);
					Trace.TraceInformation("BASS を初期化しました。(WASAPI排他モード, {0}Hz, {1}ch, フォーマット:{2}, バッファ{3}bytes [{4}ms(希望{5}ms)], 更新間隔{6}ms)",
						wasapiInfo.freq,
						wasapiInfo.chans,
						wasapiInfo.format.ToString(),
						wasapiInfo.buflen,
						n実バッファサイズms.ToString(),
						(f希望バッファサイズsec * 1000).ToString(),  //n希望バッファサイズms.ToString(),
						(f更新間隔sec * 1000).ToString()            //n更新間隔ms.ToString()
					);
					Trace.TraceInformation("デバイスの最小更新時間={0}ms, 既定の更新時間={1}ms", deviceInfo.minperiod * 1000, deviceInfo.defperiod * 1000);
					this.bIsBASSFree = false;
					//-----------------
					#endregion
				}
				else
				{
					#region [ 共有モードで作成成功。]
					//-----------------
					this.e出力デバイス = ESoundDeviceType.SharedWASAPI;

					var wasapiInfo = BassWasapi.BASS_WASAPI_GetInfo();
					int n1サンプルのバイト数 = 2 * wasapiInfo.chans; // default;
					int n1秒のバイト数 = n1サンプルのバイト数 * wasapiInfo.freq;
					this.n実バッファサイズms = (long)(wasapiInfo.buflen * 1000.0f / n1秒のバイト数);
					this.n実出力遅延ms = 0;  // 初期値はゼロ
					var devInfo = BassWasapi.BASS_WASAPI_GetDeviceInfo(BassWasapi.BASS_WASAPI_GetDevice()); // 共有モードの場合、更新間隔はデバイスのデフォルト値に固定される。
																											//Trace.TraceInformation( "BASS を初期化しました。(WASAPI共有モード, 希望バッファサイズ={0}ms, 更新間隔{1}ms)", n希望バッファサイズms, devInfo.defperiod * 1000.0f );
					Trace.TraceInformation("使用デバイス: #" + nDevNo + " : " + deviceInfo.name + ", flags=" + deviceInfo.flags);
					Trace.TraceInformation("BASS を初期化しました。(WASAPI共有モード, {0}Hz, {1}ch, フォーマット:{2}, バッファ{3}bytes [{4}ms(希望{5}ms)], 更新間隔{6}ms)",
						wasapiInfo.freq,
						wasapiInfo.chans,
						wasapiInfo.format.ToString(),
						wasapiInfo.buflen,
						n実バッファサイズms.ToString(),
						(f希望バッファサイズsec * 1000).ToString(),  //n希望バッファサイズms.ToString(),
						(f更新間隔sec * 1000).ToString()            //n更新間隔ms.ToString()
					);
					Trace.TraceInformation("デバイスの最小更新時間={0}ms, 既定の更新時間={1}ms", deviceInfo.minperiod * 1000, deviceInfo.defperiod * 1000);
					this.bIsBASSFree = false;
					//-----------------
					#endregion
				}
			}
			#region [ #31737 WASAPI排他モードのみ利用可能とし、WASAPI共有モードは使用できないようにするために、WASAPI共有モードでの初期化フローを削除する。 ]
			else if (mode == Eデバイスモード.排他)
			{
				BASSError errcode = Bass.BASS_ErrorGetCode();
				Trace.TraceInformation("Failed to initialize setting BASS_WASAPI_Init (WASAPI{0}): [{1}]", mode.ToString(), errcode);
				#region [ 排他モードに失敗したのなら共有モードでリトライ。]
				//-----------------
				//	mode = Eデバイスモード.共有;
				//	goto Retry;
				//-----------------
				Bass.BASS_Free();
				this.bIsBASSFree = true;
				throw new Exception(string.Format("BASS (WASAPI{0}) の初期化に失敗しました。(BASS_WASAPI_Init)[{1}]", mode.ToString(), errcode));
				#endregion
			}
			#endregion
			else
			{
				#region [ それでも失敗したら例外発生。]
				//-----------------
				BASSError errcode = Bass.BASS_ErrorGetCode();
				Bass.BASS_Free();
				this.bIsBASSFree = true;
				throw new Exception(string.Format("BASS (WASAPI{0}) の初期化に失敗しました。(BASS_WASAPI_Init)[{1}]", mode.ToString(), errcode));
				//-----------------
				#endregion
			}
#if TEST_MultiThreadedMixer
			//LoadLibraryに失敗する・・・
			//BASSThreadedMixerLibraryWrapper.InitBASSThreadedMixerLibrary();
#endif


			// WASAPI出力と同じフォーマットを持つ BASS ミキサーを作成。
			// 1つのまとめとなるmixer (hMixer) と、そこにつなぐ複数の楽器別mixer (hMixer _forChips)を作成。

			//Debug.Assert( Bass.BASS_SetConfig( BASSConfig.BASS_CONFIG_MIXER_BUFFER, 5 ),		// バッファ量を最大量の5にする
			//	string.Format( "BASS_SetConfig(CONFIG_MIXER_BUFFER) に失敗しました。[{0}", Bass.BASS_ErrorGetCode() ) );

			var info = BassWasapi.BASS_WASAPI_GetInfo();
#if TEST_MultiThreadedMixer
			this.hMixer = BASSThreadedMixerLibraryWrapper.BASS_ThreadedMixer_Create(
				info.freq,
				info.chans,
				(int)(BASSFlag.BASS_MIXER_NONSTOP | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_MIXER_POSEX),
				out hMixerThreaded
			);
#else
			this.hMixer = BassMix.BASS_Mixer_StreamCreate(
				info.freq,
				info.chans,
				BASSFlag.BASS_MIXER_NONSTOP | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_MIXER_POSEX);    // デコードのみ＝発声しない。WASAPIに出力されるだけ。
#endif
			if (this.hMixer == 0)
			{
				BASSError errcode = Bass.BASS_ErrorGetCode();
				BassWasapi.BASS_WASAPI_Free();
				Bass.BASS_Free();
				this.bIsBASSFree = true;
				throw new Exception(string.Format("BASSミキサ(mixing)の作成に失敗しました。[{0}]", errcode));
			}


			for (int i = 0; i <= (int)CSound.EInstType.Unknown; i++)
			{
#if TEST_MultiThreadedMixer
				this.hMixer_Chips[i] = BASSThreadedMixerLibraryWrapper.BASS_ThreadedMixer_Create(
					info.freq,
					info.chans,
					(int)(BASSFlag.BASS_MIXER_NONSTOP | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_MIXER_POSEX),
					out this.hMixerThreaded_Chips[i]
				);    // デコードのみ＝発声しない。WASAPIに出力されるだけ。
#else
				this.hMixer_Chips[i] = BassMix.BASS_Mixer_StreamCreate(
					info.freq,
					info.chans,
					BASSFlag.BASS_MIXER_NONSTOP | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_MIXER_POSEX);    // デコードのみ＝発声しない。WASAPIに出力されるだけ。
#endif
				if (this.hMixer_Chips[i] == 0)
				{
					BASSError errcode = Bass.BASS_ErrorGetCode();
					BassWasapi.BASS_WASAPI_Free();
					Bass.BASS_Free();
					this.bIsBASSFree = true;
					throw new Exception(string.Format("BASSミキサ(楽器[{1}]ごとのmixing)の作成に失敗しました。[{0}]", errcode, i));
				}

				// Mixerのボリューム設定
				Bass.BASS_ChannelSetAttribute(this.hMixer_Chips[i], BASSAttribute.BASS_ATTRIB_VOL, CSoundManager.nMixerVolume[i] / 100.0f);
				//Trace.TraceInformation("Vol{0}: {1}", i, CSound管理.nMixerVolume[i]);

#if TEST_MultiThreadedMixer
				bool b1 = BASSThreadedMixerLibraryWrapper.BASS_ThreadedMixer_AddSource(this.hMixerThreaded, this.hMixer_Chips[i], IntPtr.Zero);
#else
				bool b1 = BassMix.BASS_Mixer_StreamAddChannel(this.hMixer, this.hMixer_Chips[i], BASSFlag.BASS_DEFAULT);
#endif
				if (!b1)
				{
					BASSError errcode = Bass.BASS_ErrorGetCode();
					BassWasapi.BASS_WASAPI_Free();
					Bass.BASS_Free();
					this.bIsBASSFree = true;
					throw new Exception(string.Format("個別BASSミキサ({1}}から(mixing)への接続に失敗しました。[{0}]", errcode, i));
				};

			}

			// BASS ミキサーの1秒あたりのバイト数を算出。

			var mixerInfo = Bass.BASS_ChannelGetInfo(this.hMixer);
			long nミキサーの1サンプルあたりのバイト数 = mixerInfo.chans * 4; // 4 = sizeof(FLOAT)
			this.nミキサーの1秒あたりのバイト数 = nミキサーの1サンプルあたりのバイト数 * mixerInfo.freq;



			// 単純に、hMixerの音量をMasterVolumeとして制御しても、
			// ChannelGetData()の内容には反映されない。
			// そのため、もう一段mixerを噛ませて、一段先のmixerからChannelGetData()することで、
			// hMixerの音量制御を反映させる。
			this.hMixer_DeviceOut = BassMix.BASS_Mixer_StreamCreate(
				info.freq,
				info.chans,
				BASSFlag.BASS_MIXER_NONSTOP | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_MIXER_POSEX);    // デコードのみ＝発声しない。WASAPIに出力されるだけ。
			if (this.hMixer_DeviceOut == 0)
			{
				BASSError errcode = Bass.BASS_ErrorGetCode();
				BassWasapi.BASS_WASAPI_Free();
				Bass.BASS_Free();
				this.bIsBASSFree = true;
				throw new Exception(string.Format("BASSミキサ(最終段)の作成に失敗しました。[{0}]", errcode));
			}

			{
				bool b1 = BassMix.BASS_Mixer_StreamAddChannel(this.hMixer_DeviceOut, this.hMixer, BASSFlag.BASS_DEFAULT);
				if (!b1)
				{
					BASSError errcode = Bass.BASS_ErrorGetCode();
					BassWasapi.BASS_WASAPI_Free();
					Bass.BASS_Free();
					this.bIsBASSFree = true;
					throw new Exception(string.Format("BASSミキサ(最終段とmixing)の接続に失敗しました。[{0}]", errcode));
				};
			}


			// 録音設定(DTX2WAV)
			if (!string.IsNullOrEmpty(strRecordFileType))
			{
				switch (strRecordFileType.ToUpper())
				{
					case "WAV":
						{
							var e = new EncoderWAV(this.hMixer_DeviceOut);
							//e.WAV_EncoderType = BASSChannelType.BASS_CTYPE_STREAM_WAV_PCM;
							encoder = e;
						}
						break;
					case "OGG":
						{
							var e = new EncoderOGG(this.hMixer_DeviceOut);
							e.EncoderDirectory = strEncoderPath;
							e.OGG_UseQualityMode = true;
							e.OGG_Quality = (float)CSoundManager.nBitrate;
							//e.OGG_Bitrate = 128;
							//e.OGG_MinBitrate = 0;
							//e.OGG_MaxBitrate = 0;

							encoder = e;
						}
						break;
					case "MP3":
						{
							var e = new EncoderLAME(this.hMixer_DeviceOut);
							e.EncoderDirectory = strEncoderPath;
							e.LAME_UseVBR = false;
							e.LAME_Bitrate = CSoundManager.nBitrate;
							encoder = e;
						}
						break;
					default:
						encoder = new EncoderWAV(this.hMixer_DeviceOut);
						break;
				}
				encoder.InputFile = null;    //STDIN
				encoder.OutputFile = CSoundManager.strRecordOutFilename;
				encoder.UseAsyncQueue = true;
				encoder.Start(null, IntPtr.Zero, true);     // PAUSE状態で録音開始
			}
			//Bass.BASS_ChannelSetAttribute(this.hMixer_DeviceOut, BASSAttribute.BASS_ATTRIB_VOL, 0.10f);
			//Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_GVOL_SAMPLE, 1000);
			//Bass.BASS_SetVolume(0.1f);

			// 出力を開始。
			BassWasapi.BASS_WASAPI_Start();
		}

		#region [録音開始]
		public bool tStartRecording()
		{
			return encoder.Pause(false);
		}
		#endregion
		#region [録音終了]
		public bool tStopRecording()
		{
			return encoder.Stop(true);
		}
		#endregion

		#region [ tサウンドを作成する() ]
		public CSound tサウンドを作成する(string strファイル名)
		{
			return tサウンドを作成する(strファイル名, CSound.EInstType.Unknown);
		}
		public CSound tサウンドを作成する(string strファイル名, CSound.EInstType eInstType)
		{
			var sound = new CSound();
#if TEST_MultiThreadedMixer
			int hmixer = (int)hMixerThreaded_Chips[ (int)eInstType ];
#else
			int hmixer = hMixer_Chips[(int)eInstType];
#endif
			sound.tWASAPIサウンドを作成する(strファイル名, hmixer, this.e出力デバイス, eInstType);
			return sound;
		}
		public CSound tサウンドを作成する(byte[] byArrWAVファイルイメージ)
		{
			return tサウンドを作成する(byArrWAVファイルイメージ, CSound.EInstType.Unknown);
		}
		public CSound tサウンドを作成する(byte[] byArrWAVファイルイメージ, CSound.EInstType eInstType)
		{
			var sound = new CSound();
#if TEST_MultiThreadedMixer
			int hmixer = (int)hMixerThreaded_Chips[(int)eInstType];
#else
			int hmixer = hMixer_Chips[(int)eInstType];
#endif
			sound.tWASAPIサウンドを作成する(byArrWAVファイルイメージ, hmixer, this.e出力デバイス, eInstType);
			return sound;
		}
		public void tサウンドを作成する(string strファイル名, ref CSound sound, CSound.EInstType eInstType)
		{
#if TEST_MultiThreadedMixer
			int hmixer = (int)hMixerThreaded_Chips[(int)eInstType];
#else
			int hmixer = hMixer_Chips[(int)eInstType];
#endif
			sound.tWASAPIサウンドを作成する(strファイル名, hmixer, this.e出力デバイス, eInstType);
		}
		public void tサウンドを作成する(byte[] byArrWAVファイルイメージ, ref CSound sound, CSound.EInstType eInstType)
		{
#if TEST_MultiThreadedMixer
			int hmixer = (int)hMixerThreaded_Chips[(int)eInstType];
#else
			int hmixer = hMixer_Chips[(int)eInstType];
#endif
			sound.tWASAPIサウンドを作成する(byArrWAVファイルイメージ, hmixer, this.e出力デバイス, eInstType);
		}
		#endregion

		#region [ Dispose-Finallizeパターン実装 ]
		//-----------------
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected void Dispose(bool bManagedDispose)
		{
			if (encoder != null)
			{
				encoder.Stop();  // finish
				encoder.Dispose();
				encoder = null;
			}
			this.e出力デバイス = ESoundDeviceType.Unknown;        // まず出力停止する(Dispose中にクラス内にアクセスされることを防ぐ)

			if (this.hMixer_DeviceOut != 0)
			{
				BassMix.BASS_Mixer_ChannelPause(this.hMixer_DeviceOut);
				Bass.BASS_StreamFree(this.hMixer_DeviceOut);
				this.hMixer_DeviceOut = 0;
			}
			if (this.hMixer_Record != 0)
			{
				BassMix.BASS_Mixer_ChannelPause(this.hMixer_Record);
				Bass.BASS_StreamFree(this.hMixer_Record);
				this.hMixer_Record = 0;
			}

			if (hMixer != 0)
			{
				BassMix.BASS_Mixer_ChannelPause(this.hMixer_Record);
				Bass.BASS_StreamFree(this.hMixer);
			}
			if (this.hMixer_Chips != null)
			{
				for (int i = 0; i <= (int)CSound.EInstType.Unknown; i++)
				{
					if (this.hMixer_Chips[i] != 0)
					{
						// Mixerにinputされるchannelsがfreeされると、Mixerへのinputも自動でremoveされる。
						// 従い、ここでは、mixer本体をfreeするだけでよい
						BassMix.BASS_Mixer_ChannelPause(this.hMixer_Chips[i]);
						Bass.BASS_StreamFree(this.hMixer_Chips[i]);
						this.hMixer_Chips[i] = 0;
					}
				}
			}
#if TEST_MultiThreadedMixer
			//BASSThreadedMixerLibraryWrapper.FreeBASSThreadedMixerLibrary();		
#endif

			if (!this.bIsBASSFree)
			{
				BassWasapi.BASS_WASAPI_Free();  // システムタイマより先に呼び出すこと。（tWasapi処理() の中でシステムタイマを参照してるため）
				Bass.BASS_Free();
			}
			if (bManagedDispose)
			{
				CCommon.tDispose(this.tmシステムタイマ);
				this.tmシステムタイマ = null;
			}
		}
		~CSoundDeviceWASAPI()
		{
			this.Dispose(false);
		}
		//-----------------
		#endregion

		protected int hMixer = 0;
		protected int hMixer_DeviceOut = 0;
		protected int hMixer_Record = 0;
		protected int[] hMixer_Chips = new int[(int)CSound.EInstType.Unknown + 1];  //DTX2WAV対応 BGM, SE, Drums...を別々のmixerに入れて、個別に音量変更できるようにする

#if TEST_MultiThreadedMixer
		protected IntPtr hMixerThreaded = IntPtr.Zero;
		protected IntPtr[] hMixerThreaded_Chips = new IntPtr[(int)CSound.EInstType.Unknown + 1];
#endif
		protected BaseEncoder encoder;
		protected int stream;
		protected WASAPIPROC tWasapiProc = null;

		protected int tWASAPI処理(IntPtr buffer, int length, IntPtr user)
		{
			// BASSミキサからの出力データをそのまま WASAPI buffer へ丸投げ。

			int num = Bass.BASS_ChannelGetData(this.hMixer_DeviceOut, buffer, length);        // num = 実際に転送した長さ
																							  //int num = BassMix.BASS_Mixer_ChannelGetData(this.hMixer_DeviceOut, buffer, length);      // これだと動作がめちゃくちゃ重くなる
			if (num == -1) num = 0;


			// 経過時間を更新。
			// データの転送差分ではなく累積転送バイト数から算出する。

			int n未再生バイト数 = BassWasapi.BASS_WASAPI_GetData(null, (int)BASSData.BASS_DATA_AVAILABLE); // 誤差削減のため、必要となるギリギリ直前に取得する。
			this.n経過時間ms = (this.n累積転送バイト数 - n未再生バイト数) * 1000 / this.nミキサーの1秒あたりのバイト数;
			this.n経過時間を更新したシステム時刻ms = this.tmシステムタイマ.nSystemTimeMs;

			// 実出力遅延を更新。
			// 未再生バイト数の平均値。

			long n今回の遅延ms = n未再生バイト数 * 1000 / this.nミキサーの1秒あたりのバイト数;
			this.n実出力遅延ms = (this.b最初の実出力遅延算出) ? n今回の遅延ms : (this.n実出力遅延ms + n今回の遅延ms) / 2;
			this.b最初の実出力遅延算出 = false;


			// 経過時間を更新後に、今回分の累積転送バイト数を反映。

			this.n累積転送バイト数 += num;
			return num;
		}

		private long nミキサーの1秒あたりのバイト数 = 0;
		private long n累積転送バイト数 = 0;
		private bool b最初の実出力遅延算出 = true;
		private bool bIsBASSFree = true;
	}
}
