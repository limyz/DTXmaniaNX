using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using FDK;


namespace DTXMania
{
	public class CDTXVmode
	{
		public enum ECommand
		{
			Stop,
			Play,
			Preview
		}

		/// <summary>
		/// DTXVモードかどうか
		/// </summary>
		public bool Enabled
		{
			get;
			set;
		}

		/// <summary>
		/// プレビューサウンドの再生が発生した
		/// </summary>
		public bool Preview
		{
			get;
			set;
		}

		/// <summary>
		/// 外部から再指示が発生したか
		/// </summary>
		public bool Refreshed
		{
			get;
			set;
		}

		/// <summary>
		/// 演奏開始小節番号
		/// </summary>
		public int nStartBar
		{
			get;
			set;
		}

		/// <summary>
		/// DTXファイルの再読み込みが必要かどうか
		/// </summary>
		public bool NeedReload
		{
			get;
			private set;
			//			private set;	// 本来はprivate setにすべきだが、デバッグが簡単になるので、しばらくはprivateなしのままにする。
		}

		/// <summary>
		/// DTXCからのコマンド
		/// </summary>
		public ECommand Command
		{
			get;
			set;
		}

		public ESoundDeviceType soundDeviceType
		{
			get;
			set;
		}
		public int nASIOdevice
		{
			get;
			set;
		}
		/// <summary>
		/// 前回からサウンドデバイスが変更されたか
		/// </summary>
		public bool ChangedSoundDevice
		{
			get;
			set;
		}

		public string filename
		{
			get
			{
				return last_path;
			}
		}

		public string previewFilename
		{
			get;
			set;
		}
		public int previewVolume
		{
			get;
			set;
		}
		public int previewPan
		{
			get;
			set;
		}
		public bool GRmode
		{
			get;
			set;
		}
		public bool lastGRmode
		{
			get;
			private set;
		}
		public bool TimeStretch
		{
			get;
			set;
		}
		public bool lastTimeStretch
		{
			get;
			private set;
		}
		public bool VSyncWait
		{
			get;
			set;
		}
		public bool lastVSyncWait
		{
			get;
			private set;
		}

        public int heightResolution
        {
            get;
            set;
        }

        public int widthResolution
        {
            get;
            set;
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CDTXVmode()
		{
			this.last_path = "";
			this.last_timestamp = DateTime.MinValue;
			this.Enabled = false;
			this.nStartBar = 0;
			this.Refreshed = false;
			this.NeedReload = false;
			this.previewFilename = "";
			this.GRmode = false;
			this.lastGRmode = false;
			this.TimeStretch = false;
			this.lastTimeStretch = false;
			this.VSyncWait = true;
			this.lastVSyncWait = true;
			this.heightResolution = 360;
			this.widthResolution = 640;
		}

		/// <summary>
		/// DTXファイルのリロードが必要かどうか判定する
		/// </summary>
		/// <param name="filename">DTXファイル名</param>
		/// <returns>再読込が必要ならtrue</returns>
		/// <remarks>プロパティNeedReloadにも結果が入る</remarks>
		/// <remarks>これを呼び出すたびに、Refreshedをtrueにする</remarks>
		/// <exception cref="FileNotFoundException"></exception>
		public bool bIsNeedReloadDTX(string filename)
		{
			if (!File.Exists(filename))     // 指定したファイルが存在しないなら例外終了
			{
				Trace.TraceError("ファイルが見つかりません。({0})", filename);
				this.last_path = filename;
				throw new FileNotFoundException();
				//return false;
			}

			this.Refreshed = true;

			// 前回とファイル名が異なるか、タイムスタンプが更新されているか、
			// GRmode等の設定を変更したなら、DTX要更新
			DateTime current_timestamp = File.GetLastWriteTime(filename);
			if (last_path != filename || current_timestamp > last_timestamp ||
				this.lastGRmode != this.GRmode || this.lastTimeStretch != this.TimeStretch || this.lastVSyncWait != this.VSyncWait)
			{
				this.last_path = filename;
				this.last_timestamp = current_timestamp;
				this.lastGRmode = this.GRmode;
				this.lastTimeStretch = this.TimeStretch;
				this.lastVSyncWait = this.VSyncWait;

				this.NeedReload = true;
				return true;
			}
			this.NeedReload = false;
			return false;
		}


		/// <summary>
		/// Viewer関連の設定のみを更新して、Config.iniに書き出す
		/// </summary>
		/*
		public void tUpdateConfigIni()
		{
			/// Viewer関連の設定のみを更新するために、
			/// 1. 現在のconfig.ini相当の情報を、別変数にコピーしておく
			/// 2. config.iniを読み込みなおす
			/// 3. 別変数のコピーから、Viewer関連の設定を、configに入れ込む
			/// 4. Config.iniを保存する

			CConfigXml ConfigIni_backup = (CConfigXml)CDTXMania.Instance.ConfigIni.Clone();     // #36612 2016.9.12 yyagi
			CDTXMania.Instance.LoadConfig();

			// CConfigIni cc = new CConfigIni();
			//string path = CDTXMania.Instance.strEXEのあるフォルダ + "Config.ini";
			//if (File.Exists(path))
			//{
			//	FileInfo fi = new FileInfo(path);
			//	if (fi.Length > 0)	// Config.iniが0byteだったなら、読み込まない
			//	{
			//		try
			//		{
			//			CDTXMania..tファイルから読み込み(path);
			//		}
			//		catch
			//		{
			//			//ConfigIni = new CConfigIni();	// 存在してなければ新規生成
			//		}
			//	}
			//	fi = null;
			//}

			for (EPart inst = EPart.Drums; inst <= EPart.Bass; ++inst)
			{
				CDTXMania.Instance.ConfigIni.nViewerScrollSpeed[inst].Value = ConfigIni_backup.nScrollSpeed[inst];
			}
			CDTXMania.Instance.ConfigIni.bViewerShowDebugStatus.Value = ConfigIni_backup.bDebugInfo;
			CDTXMania.Instance.ConfigIni.bViewerVSyncWait.Value = ConfigIni_backup.bVSyncWait;
			CDTXMania.Instance.ConfigIni.bViewerTimeStretch.Value = ConfigIni_backup.bTimeStretch;
			CDTXMania.Instance.ConfigIni.bViewerDrumsActive.Value = ConfigIni_backup.bDrums有効;
			CDTXMania.Instance.ConfigIni.bViewerGuitarActive.Value = ConfigIni_backup.bGuitar有効;

			CDTXMania.Instance.ConfigIni.rcViewerWindow.W = ConfigIni_backup.rcWindow.W;
			CDTXMania.Instance.ConfigIni.rcViewerWindow.H = ConfigIni_backup.rcWindow.H;
			CDTXMania.Instance.ConfigIni.rcViewerWindow.X = ConfigIni_backup.rcWindow.X;
			CDTXMania.Instance.ConfigIni.rcViewerWindow.Y = ConfigIni_backup.rcWindow.Y;

			CDTXMania.Instance.SaveConfig();

			ConfigIni_backup = null;
		}*/

		private string last_path;
		private DateTime last_timestamp;

	}
}