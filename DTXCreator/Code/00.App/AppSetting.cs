using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DTXCreator
{
	// 使い方
	//
	// 保存方法：
	// 　var xmlsl = new System.Xml.Serialization.XmlSerializer(typeof(AppSetting));
	// 　FileStream fs = new FileStream(設定ファイル名, FileMode.Create);
	// 　xmlsl.Serialize(fs, this.アプリ設定);
	// 　fs.Close();
	// 
	// 読み込み方法：
	//   try
	//   {
	//       var xmlsl = new System.Xml.Serialization.XmlSerializer(typeof(AppSetting));
	//       FileStream fs = new FileStream(設定ファイル名, FileMode.Open);
	//       this.アプリ設定 = (AppSetting)xmlsl.Deserialize(fs);
	//       fs.Close();
	//   }
	//   catch (Exception)
	//   {
	//       Debug.WriteLine("アプリ設定ファイルの読み込みに失敗しました。");
	//       return;
	//   }

	public class AppSetting
	{
		#region [ コンストラクタ ]
		//-----------------
		public AppSetting()
		{
			this._SoundListColumnWidth[ 0 ] = 80;
			this._SoundListColumnWidth[ 1 ] = 28;
			this._SoundListColumnWidth[ 2 ] = 80;
			this._SoundListColumnWidth[ 3 ] = 40;
			this._SoundListColumnWidth[ 4 ] = 60;
			this._GraphicListColumnWidth[ 0 ] = 34;
			this._GraphicListColumnWidth[ 1 ] = 127;
			this._GraphicListColumnWidth[ 2 ] = 28;
			this._GraphicListColumnWidth[ 3 ] = 120;
			this._MovieListColumnWidth[ 0 ] = 127;
			this._MovieListColumnWidth[ 1 ] = 28;
			this._MovieListColumnWidth[ 2 ] = 120;
			this._LastWorkFolder = Directory.GetCurrentDirectory();
			this._ViewerInfo = new Viewer();
			this._InitialOperationMode = false;
		}
		//-----------------
		#endregion

		// プロパティ(1) オプション項目関連

		#region [ List<string> RecentUsedFile - 最近使ったファイル名のリスト ]
		//-----------------
		public List<string> RecentUsedFile
		{
			get { return _RecentUsedFile; }
			set { _RecentUsedFile = value; }
		}
		private List<string> _RecentUsedFile = new List<string>();
		//-----------------
		#endregion

		public void AddRecentUsedFile( string fileName )
		{
			for( int i = 0; i < this._RecentUsedFile.Count; i++ )
			{
				if( this._RecentUsedFile[ i ].Equals( fileName ) )
				{
					this._RecentUsedFile.RemoveAt( i );
					break;
				}
			}
			this._RecentUsedFile.Insert( 0, fileName );
			if( this._RecentUsedFile.Count > 10 )
			{
				int num2 = this._RecentUsedFile.Count - 10;
				for( int j = 0; j < num2; j++ )
				{
					this._RecentUsedFile.RemoveAt( 10 + j );
				}
			}
		}

		#region [ List<Lanes> LanesInfo - レーンの表示/非表示 ]
		//-----------------
		public List<Lanes> LanesInfo
		{
			get { return _LanesInfo; }
			set { _LanesInfo = value; }
		}
		private List<Lanes> _LanesInfo = new List<Lanes>();
		//-----------------
		#endregion

		public void AddLanesInfo( string Name, bool Checked )
		{
			this._LanesInfo.Add( new Lanes( Name, Checked ) );
		}
		
		public bool bSameVersion()
		{
			return ( this._ConfigVersion == _ConfigSchemaVersion );
		}
		public void Confirm()
		{
			if( this._RecentFilesNum <= 0 )
			{
				this._RecentFilesNum = 5;
				this._ShowRecentFiles = false;
			}
			else if( this._RecentFilesNum > 10 )
			{
				this._RecentFilesNum = 10;
			}
		}

		public bool AutoFocus
		{
			get
			{
				return this._AutoFocus;
			}
			set
			{
				this._AutoFocus = value;
			}
		}
		public int ConfigVersion
		{
			get
			{
				return this._ConfigVersion;
			}
			set
			{
				this._ConfigVersion = value;
			}
		}
		public int[] GraphicListColumnWidth
		{
			get
			{
				return this._GraphicListColumnWidth;
			}
			set
			{
				this._GraphicListColumnWidth = value;
			}
		}
		public int GuideIndex
		{
			get
			{
				return this._GuideIndex;
			}
			set
			{
				this._GuideIndex = value;
				if( this._GuideIndex < 0 )
				{
					this._GuideIndex = 0;
				}
				else if( this._GuideIndex > 8 )
				{
					this._GuideIndex = 8;
				}
			}
		}
		public int Height
		{
			get
			{
				return this._Height;
			}
			set
			{
				this._Height = value;
				if( this._Height < 0 )
				{
					this._Height = 10;
				}
			}
		}
		public int HViewScaleIndex
		{
			get
			{
				return this._HViewScaleIndex;
			}
			set
			{
				this._HViewScaleIndex = value;
				if( this._HViewScaleIndex < 0 )
				{
					this._HViewScaleIndex = 0;
				}
				else if( this._HViewScaleIndex > 9 )
				{
					this._HViewScaleIndex = 9;
				}
			}
		}
		public string LastWorkFolder
		{
			get
			{
				return this._LastWorkFolder;
			}
			set
			{
				this._LastWorkFolder = value;
			}
		}
		public bool Maximized
		{
			get
			{
				return this._Maximized;
			}
			set
			{
				this._Maximized = value;
			}
		}
		public int[] MovieListColumnWidth
		{
			get
			{
				return this._MovieListColumnWidth;
			}
			set
			{
				this._MovieListColumnWidth = value;
			}
		}
		public bool NoPreviewBGM
		{
			get
			{
				return this._NoPreviewBGM;
			}
			set
			{
				this._NoPreviewBGM = value;
			}
		}
		public bool PlaySoundOnWAVChipAllocated
		{
			get
			{
				return this._PlaySoundOnWAVChipAllocated;
			}
			set
			{
				this._PlaySoundOnWAVChipAllocated = value;
			}
		}
		public int RecentFilesNum
		{
			get
			{
				return this._RecentFilesNum;
			}
			set
			{
				this._RecentFilesNum = value;
			}
		}
		public bool ShowRecentFiles
		{
			get
			{
				return this._ShowRecentFiles;
			}
			set
			{
				this._ShowRecentFiles = value;
			}
		}
		public int[] SoundListColumnWidth
		{
			get
			{
				return this._SoundListColumnWidth;
			}
			set
			{
				this._SoundListColumnWidth = value;
			}
		}
		public int SplitterDistance
		{
			get
			{
				return this._SplitterDistance;
			}
			set
			{
				this._SplitterDistance = value;
			}
		}
		public Viewer ViewerInfo
		{
			get
			{
				return this._ViewerInfo;
			}
			set
			{
				this._ViewerInfo = value;
			}
		}
		public int Width
		{
			get
			{
				return this._Width;
			}
			set
			{
				this._Width = value;
				if( this._Width < 0 )
				{
					this._Width = 10;
				}
			}
		}
		public int X
		{
			get
			{
				return this._X;
			}
			set
			{
				this._X = value;
			}
		}
		public int Y
		{
			get
			{
				return this._Y;
			}
			set
			{
				this._Y = value;
			}
		}

		/// <summary>
		/// 操作モードの初期値
		/// false: 編集モード
		/// true:  選択モード
		/// </summary>
		public bool InitialOperationMode
		{
			get
			{
				return this._InitialOperationMode;
			}
			set
			{
				this._InitialOperationMode = value;
			}
		}

		//public enum ViewerSoundType
		//{
		//    DirectSound,
		//    WASAPI,
		//    ASIO
		//}

		public class Viewer
		{
			public const string FileNameDTXM = "DTXManiaNX.exe";
			public string PathDTXV = Directory.GetCurrentDirectory() + @"\dll\DTXV.exe";
			public bool bViewerIsDTXV = false;

			public string PlayStartFromOption = "-N";
			public string PlayStartOption = "-N-1";
			public string PlayStopOption = "-S";
			// public ViewerSoundType SoundType = ( FDK.COS.bIsVistaOrLater ) ? ViewerSoundType.WASAPI : ViewerSoundType.DirectSound;
			public FDK.ESoundDeviceType SoundType = (FDK.COS.bIsVistaOrLater()) ? FDK.ESoundDeviceType.ExclusiveWASAPI : FDK.ESoundDeviceType.DirectSound;
			public int ASIODeviceNo = 0;
			public bool GRmode;
			public bool TimeStretch;
			public bool VSyncWait = true;
			public int ViewerHeightResolution = 360;

			// 引数無しのコンストラクタがないとSerializeできないのでダミー定義する
			public Viewer()
			{
				PlayStartFromOption = "-N";
				PlayStartOption = "-N-1";
				PlayStopOption = "-S";
				//SoundType =  (FDK.COS.bIsVistaOrLater)? ViewerSoundType.WASAPI : ViewerSoundType.DirectSound;
				SoundType = (FDK.COS.bIsVistaOrLater()) ?
								((FDK.COS.bIsWin10OrLater()) ? FDK.ESoundDeviceType.SharedWASAPI : FDK.ESoundDeviceType.ExclusiveWASAPI)
							: FDK.ESoundDeviceType.DirectSound;
				ASIODeviceNo = 0;
				GRmode = false;
				TimeStretch = false;
				VSyncWait = true;
			}

			public string PlaySoundOption
			{
				get
				{
					string opt = "";
					if (bViewerIsDTXV)
					{
						opt = "";
					}
					else
					{
						string soundtypeopt = "";
						switch (SoundType)
						{
							case FDK.ESoundDeviceType.DirectSound:
								soundtypeopt = "D";
								break;
							case FDK.ESoundDeviceType.ExclusiveWASAPI:
								soundtypeopt = "WE";
								break;
							case FDK.ESoundDeviceType.SharedWASAPI:
								soundtypeopt = "WS";
								break;
							case FDK.ESoundDeviceType.ASIO:
								soundtypeopt = "A";
								soundtypeopt += ASIODeviceNo.ToString();
								break;
						}
						
						opt = "-D" + soundtypeopt;
						opt += GRmode ? "Y" : "N";  // この辺は手抜き
						opt += TimeStretch ? "Y" : "N"; //
						opt += VSyncWait ? "Y" : "N";   //
					}
					return opt;
				}
			}
		
			public string PlayViewerOption 
			{
                get
                {
                    string opt = "";
                    if (bViewerIsDTXV)
                    {
                        opt = "";
                    }
                    else
                    {
                        opt = "-R" + ViewerHeightResolution.ToString();
                    }
                    return opt;
                }
            }
		}

		/// <summary>
		/// レーン名と表示/非表示の状態の保持/復元
		/// </summary>
		public class Lanes
		{
			public string Name;
			public bool Checked;

			// 引数無しのコンストラクタがないとSerializeできないのでダミー定義する
			public Lanes()
			{
				Name = "";
				Checked = false;
			}
			public Lanes( string Name_, bool Checked_ )
			{
				Name = Name_;
				Checked = Checked_;
			}
		}

		#region [ private ]
		//-----------------
		private bool _AutoFocus = true;
		private static int _ConfigSchemaVersion = 0x69;
		private int _ConfigVersion = _ConfigSchemaVersion;
		private int[] _GraphicListColumnWidth = new int[ 4 ];
		private int _GuideIndex = 3;
		private int _Height = 0x1db;
		private int _HViewScaleIndex;
		private string _LastWorkFolder = "";
		private bool _Maximized;
		private int[] _MovieListColumnWidth = new int[ 3 ];
		private bool _NoPreviewBGM = true;
		private bool _PlaySoundOnWAVChipAllocated = true;
		private int _RecentFilesNum = 5;
		private bool _ShowRecentFiles = true;
		private int[] _SoundListColumnWidth = new int[ 5 ];
		private int _SplitterDistance = 0x128;
		private Viewer _ViewerInfo;
		private int _Width = 600;
		private int _X;
		private int _Y;
		private bool _InitialOperationMode;
		//-----------------
		#endregion
	}
}
