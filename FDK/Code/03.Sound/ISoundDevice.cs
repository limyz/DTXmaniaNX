using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Un4seen.Bass;
using Un4seen.BassAsio;
using Un4seen.BassWasapi;
using Un4seen.Bass.AddOn.Mix;

namespace FDK
{
	public interface ISoundDevice : IDisposable
	{
		ESoundDeviceType e出力デバイス { get; }
		int nMasterVolume { get; set; }
		long n実出力遅延ms { get; }
		long n実バッファサイズms { get; }
		long n経過時間ms { get; }
		long n経過時間を更新したシステム時刻ms { get; }
		CTimer tmシステムタイマ { get; }
		string strDefaultSoundDeviceBusType { get; }

		CSound tサウンドを作成する( string strファイル名 );
		CSound tサウンドを作成する(string strファイル名, CSound.EInstType einstType);
		CSound tサウンドを作成する( byte[] byArrWAVファイルイメージ );
		CSound tサウンドを作成する(byte[] byArrWAVファイルイメージ, CSound.EInstType eInstType);
		//void tサウンドを作成する( string strファイル名, ref CSound sound );
		void tサウンドを作成する(string strファイル名, ref CSound sound, CSound.EInstType eInstType);
		//void tサウンドを作成する( byte[] byArrWAVファイルイメージ, ref CSound sound );
		void tサウンドを作成する(byte[] byArrWAVファイルイメージ, ref CSound sound, CSound.EInstType eInstType);
		bool tStartRecording();
		bool tStopRecording();
	}
}
