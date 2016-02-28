using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Xml.Serialization;
using DTXCreator.チップパレット関連;
using DTXCreator.譜面;
using DTXCreator.WAV_BMP_AVI;
using DTXCreator.UndoRedo;
using DTXCreator.オプション関連;
using DTXCreator.汎用;
using DTXCreator.Properties;
using FDK;

namespace DTXCreator
{
	public partial class Cメインフォーム : Form
	{
		// コンストラクタ

		#region [ コンストラクタ ]
		//-----------------
		public Cメインフォーム()
		{
			this.InitializeComponent();
		}
		//-----------------
		#endregion


		// プロパティ

		#region [ プロパティ ]
		//-----------------

		public AppSetting appアプリ設定;
		public Cチップパレット dlgチップパレット;
		private Cオプション管理 mgrオプション管理者 = null;
		private C選択モード管理 mgr選択モード管理者 = null;
		private C編集モード管理 mgr編集モード管理者 = null;
		internal C譜面管理 mgr譜面管理者 = null;
		internal CWAVリスト管理 mgrWAVリスト管理者 = null;
		internal CBMPリスト管理 mgrBMPリスト管理者 = null;
		internal CAVIリスト管理 mgrAVIリスト管理者 = null;
		internal CUndoRedo管理 mgrUndoRedo管理者 = null;
		internal Cクリップボード cbクリップボード = null;

		internal MakeTempDTX makeTempDTX = null;

		public bool b選択モードである
		{
			get
			{
				if( this.toolStripButton選択モード.CheckState != CheckState.Checked )
				{
					return false;
				}
				return true;
			}
		}
		public bool b編集モードである
		{
			get
			{
				if( this.toolStripButton編集モード.CheckState != CheckState.Checked )
				{
					return false;
				}
				return true;
			}
		}
		public decimal dc現在のBPM
		{
			get
			{
				return this.numericUpDownBPM.Value;
			}
		}
		internal int n現在選択中のWAV・BMP・AVIリストの行番号0to1294;

		/// <summary>
		/// DTXC.exe のあるフォルダの絶対パス。
		/// </summary>
		public string strDTXCのあるフォルダ名;

		/// <summary>
		/// 各種ファイル（WAVなど）の相対パスの基点となるフォルダの絶対パス。
		/// </summary>
		public string str作業フォルダ名;

		/// <summary>
		/// 現在作成中のDTXファイル名。パスは含まない。（例："test.dtx"）
		/// </summary>
		public string strDTXファイル名;

		/// <summary>
		/// <para>未保存の場合にtrueとなり、ウィンドウタイトルに[*]が加えられる。</para>
		/// </summary>
		internal bool b未保存
		{
			get
			{
				return this._b未保存;
			}
			set
			{
				// 現状と値が違うときだけ更新する。

				if( this._b未保存 != value )
				{
					this._b未保存 = value;		// #24133 2011.1.14 yyagi: 「代入後にif文分岐」するよう、代入を頭に移動。

					// タイトル文字列を取得。

					string strタイトル = Resources.strデフォルトウィンドウタイトル;

					if( this.strDTXファイル名.Length > 0 )
						strタイトル = this.strDTXファイル名;

					
					// タイトル文字列を修正。

					if( this._b未保存 )
					{
						// 変更ありかつ未保存なら「*」を付ける

						this.Text = "DTXC* [" + strタイトル + "]";
						this.toolStripMenuItem上書き保存.Enabled = true;
						this.toolStripButton上書き保存.Enabled = true;
					}
					else
					{
						// 保存後変更がないなら「*」なない

						this.Text = "DTXC [" + strタイトル + "]";
						this.toolStripMenuItem上書き保存.Enabled = false;
						this.toolStripButton上書き保存.Enabled = false;
					}
				}
			}
		}

		//-----------------
		#endregion


		// シナリオ

		#region [ アプリの起動・初期化、終了 ]
		//-----------------
		private void tアプリ起動時に一度だけ行う初期化処理()
		{
			// 初期化

			#region [ アプリ設定オブジェクトを生成する。]
			//-----------------
			this.appアプリ設定 = new AppSetting();
			//-----------------
			#endregion

			#region [ DTXCreator.exe の存在するフォルダを取得する。 ]
			//-----------------
			this.strDTXCのあるフォルダ名 = Directory.GetCurrentDirectory() + @"\";
			//-----------------
			#endregion
			#region [ 作業フォルダを取得する。]
			//-----------------
			this.str作業フォルダ名 = this.strDTXCのあるフォルダ名;
			//-----------------
			#endregion

			#region [ デザイナで設定できないイベントを実装する。]
			//-----------------
			this.splitContainerタブと譜面を分割.MouseWheel += new MouseEventHandler( this.splitContainerタブと譜面を分割_MouseWheel );
			//-----------------
			#endregion

			#region [ 全体を通して必要な管理者オブジェクトを生成する。]
			//-----------------
			this.mgrオプション管理者 = new Cオプション管理( this );
			//-----------------
			#endregion

			#region [ クリップボードオブジェクトを生成する。 ]
			//-----------------
			this.cbクリップボード = new Cクリップボード( this );
			//-----------------
			#endregion

			#region [ Viewer再生用一時DTX生成オブジェクトを生成する。 ]
			makeTempDTX = new MakeTempDTX();
			#endregion

			#region [ 譜面を初期化する。]
			//-----------------
			this.t譜面を初期化する();
			//-----------------
			#endregion

			#region [ アプリ設定ファイル (DTXCreatorSetting.config) を読み込む。]
			//-----------------
			this.tアプリ設定の読み込み();	// 譜面の生成後に行うこと。（GUIイベント発生時にmgr各種が使われるため。）
			//-----------------
			#endregion

			#region [ チップパレットウィンドウの初期位置を変更する。（読み込んだアプリ設定に合わせる。）]
			//-----------------
			this.dlgチップパレット.Left = this.Left + ( ( this.Width - this.dlgチップパレット.Width ) / 2 );
			this.dlgチップパレット.Top = this.Top + ( ( this.Height - this.dlgチップパレット.Height ) / 2 );
			//-----------------
			#endregion

			#region [ [ファイル]メニューに、最近使ったファイルを追加する。]
			//-----------------
			this.t最近使ったファイルをFileメニューへ追加する();
			//-----------------
			#endregion


			// ファイル指定があればそれを開く。

			#region [ コマンドライン引数にファイルの指定があるならそれを開く。 ]
			//-----------------
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			if( ( commandLineArgs.Length > 1 ) && File.Exists( commandLineArgs[ 1 ] ) )
				this.t演奏ファイルを開いて読み込む( commandLineArgs[ 1 ] );
			//-----------------
			#endregion
		}
		private void tアプリ終了時に行う終了処理()
		{
			#region [ アプリ設定ファイル (DTXCreatorSetting.config) を保存する。]
			//-----------------
			this.tアプリ設定の保存();
			//-----------------
			#endregion

			#region [ 各管理者で必要な終了処理を行う。]
			//-----------------
			this.mgrWAVリスト管理者.tDirectSoundの解放();
			//-----------------
			#endregion

			#region [ Viewer再生用一時DTX生成オブジェクトの終了処理を行う。 ]
			makeTempDTX.Dispose();
			makeTempDTX = null;
			#endregion
		}
		private void tアプリ設定の読み込み()
		{
			string str設定ファイル名 =
				this.strDTXCのあるフォルダ名 + @"DTXCreatorSetting.config";


			// 読み込む。

			#region [ アプリ設定ファイルを読み込む。 → 失敗したら内容リセットして継続する。]
			//-----------------
			if( File.Exists( str設定ファイル名 ) )
			{
				try
				{
					// アプリ設定ファイル（XML形式）を this.appアプリ設定 に読み込む。

					var serializer = new XmlSerializer( typeof( AppSetting ) );
					var stream = new FileStream( str設定ファイル名, FileMode.Open );
					this.appアプリ設定 = (AppSetting) serializer.Deserialize( stream );
					stream.Close();
				}
				catch( Exception )
				{
					// 失敗時：内容をリセットして継続する。

					this.appアプリ設定 = new AppSetting();
				}


				// 反復要素とか足りなかったりしてもリセットする。

				if( this.appアプリ設定.SoundListColumnWidth.Length != 5
					|| this.appアプリ設定.GraphicListColumnWidth.Length != 4
					|| this.appアプリ設定.MovieListColumnWidth.Length != 3
					|| !this.appアプリ設定.bSameVersion() )
				{
					this.appアプリ設定 = new AppSetting();
				}


				// 内容の妥当性を確認する。

				this.appアプリ設定.Confirm();
			}
			//-----------------
			#endregion


			// 読み込んだアプリ設定を DTXCreator に反映する。

			#region [ ウィンドウの位置とサイズ ]
			//-----------------
			this.SetDesktopBounds( this.appアプリ設定.X, this.appアプリ設定.Y, this.appアプリ設定.Width, this.appアプリ設定.Height );
			//-----------------
			#endregion
			#region [ 最大化 ]
			//-----------------
			if( this.appアプリ設定.Maximized )
				this.WindowState = FormWindowState.Maximized;
			//-----------------
			#endregion
			#region [ タブ（左側）と譜面（右側）の表示幅の割合 ]
			//-----------------
			this.splitContainerタブと譜面を分割.SplitterDistance =
				this.appアプリ設定.SplitterDistance;
			//-----------------
			#endregion
			#region [ WAV・BMP・AVIリストの各列の幅 ]
			//-----------------
			for( int i = 0; i < 5; i++ )
				this.listViewWAVリスト.Columns[ i ].Width = this.appアプリ設定.SoundListColumnWidth[ i ];

			for( int i = 0; i < 4; i++ )
				this.listViewBMPリスト.Columns[ i ].Width = this.appアプリ設定.GraphicListColumnWidth[ i ];

			for( int i = 0; i < 3; i++ )
				this.listViewAVIリスト.Columns[ i ].Width = this.appアプリ設定.MovieListColumnWidth[ i ];
			//-----------------
			#endregion
			#region [ 譜面拡大率 ]
			//-----------------
			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.toolStripComboBox譜面拡大率.SelectedIndex =
				this.appアプリ設定.HViewScaleIndex;
			//-----------------
			#endregion
			#region [ ガイド間隔 ]
			//-----------------
			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.toolStripComboBoxガイド間隔.SelectedIndex =
				this.appアプリ設定.GuideIndex;
			//-----------------
			#endregion
			#region [ 演奏速度 ]
			//-----------------
			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.toolStripComboBox演奏速度.SelectedIndex = 5;
			//-----------------
			#endregion
			#region [ 作業フォルダ ]
			//-----------------
			this.str作業フォルダ名 =
				this.appアプリ設定.LastWorkFolder;
			
			if( Directory.Exists( this.str作業フォルダ名 ) )
			{
				Directory.SetCurrentDirectory( this.str作業フォルダ名 );
			}
			else
			{
				// 作業フォルダが既になくなってる場合はカレントフォルダを適用。

				this.str作業フォルダ名 = Directory.GetCurrentDirectory();
			}
			//-----------------
			#endregion
			#region [ レーン表示/非表示の反映 #26005 2011.8.29 yyagi; added ]
			for ( int i = 0; i < this.appアプリ設定.LanesInfo.Count; i++ )
			{
				for ( int j = 0; j < this.mgr譜面管理者.listレーン.Count; j++ )
				{
					if ( this.mgr譜面管理者.listレーン[ j ].strレーン名 == this.appアプリ設定.LanesInfo[ i ].Name )
					{
						this.mgr譜面管理者.listレーン[ j ].bIsVisible = this.appアプリ設定.LanesInfo[ i ].Checked;
						break;
					}
				}
			}
			this.mgr譜面管理者.tRefreshDisplayLanes();
			#endregion
		}
		private void tアプリ設定の保存()
		{
			string str設定ファイル名 = 
				this.strDTXCのあるフォルダ名 + @"DTXCreatorSetting.config";


			// DTXCreator から保存すべきアプリ設定を取得する。

			#region [ #23729 2010.11.22 yyagi: to get DTXC's x, y, width & height correctly, set windowstate "normal" if it is "minimized." ]
			if (this.WindowState == FormWindowState.Minimized)
			{
				this.WindowState = FormWindowState.Normal;
			}
			#endregion
			#region [ ウィンドウの位置とサイズ ]
			//-----------------
			this.appアプリ設定.X = this.Location.X;
			this.appアプリ設定.Y = this.Location.Y;
			this.appアプリ設定.Width = this.Width;
			this.appアプリ設定.Height = this.Height;
			//-----------------
			#endregion
			#region [ 最大化 ]
			//-----------------
			this.appアプリ設定.Maximized =
				( this.WindowState == FormWindowState.Maximized ) ? true : false;
			//-----------------
			#endregion
			#region [ タブ（左側）と譜面（右側）の表示幅の割合 ]
			//-----------------
			this.appアプリ設定.SplitterDistance =
				this.splitContainerタブと譜面を分割.SplitterDistance;
			//-----------------
			#endregion
			#region [ WAV・BMP・AVIリストの各列の幅 ]
			//-----------------
			for( int i = 0; i < 5; i++ )
				this.appアプリ設定.SoundListColumnWidth[ i ] = this.listViewWAVリスト.Columns[ i ].Width;

			for( int i = 0; i < 4; i++ )
				this.appアプリ設定.GraphicListColumnWidth[ i ] = this.listViewBMPリスト.Columns[ i ].Width;

			for( int i = 0; i < 3; i++ )
				this.appアプリ設定.MovieListColumnWidth[ i ] = this.listViewAVIリスト.Columns[ i ].Width;
			//-----------------
			#endregion
			#region [ 譜面拡大率 ]
			//-----------------
			this.appアプリ設定.HViewScaleIndex =
				this.toolStripComboBox譜面拡大率.SelectedIndex;
			//-----------------
			#endregion
			#region [ ガイド間隔 ]
			//-----------------
			this.appアプリ設定.GuideIndex =
				this.toolStripComboBoxガイド間隔.SelectedIndex;
			//-----------------
			#endregion
			#region [ 作業フォルダ ]
			//-----------------
			this.appアプリ設定.LastWorkFolder =
				this.str作業フォルダ名;
			//-----------------
			#endregion
			#region [ レーン表示/非表示 #26005 2011.8.29 yyagi; added ]
			this.appアプリ設定.LanesInfo.Clear();
			foreach ( DTXCreator.譜面.Cレーン c in this.mgr譜面管理者.listレーン )
			{
				this.appアプリ設定.AddLanesInfo( c.strレーン名, c.bIsVisible );
			}
			#endregion


			// 保存する。

			#region [ アプリ設定をXML形式ファイルで出力する。 ]
			//-----------------
			var serializer = new XmlSerializer( typeof( AppSetting ) );
			var stream = new FileStream( str設定ファイル名, FileMode.Create );
			serializer.Serialize( (Stream) stream, this.appアプリ設定 );
			stream.Close();
			//-----------------
			#endregion
		}
		//-----------------
		#endregion
		#region [ 新規作成 ]
		//-----------------
		private void tシナリオ・新規作成()
		{
			// 作成前の保存確認。

			#region [ 未保存なら保存する。→ キャンセルされた場合はここで中断。]
			//-----------------
			if( this.t未保存なら保存する() == DialogResult.Cancel )
				return;	// 中断
			//-----------------
			#endregion


			// 新規作成。

			#region [「初期化中です」ポップアップを表示する。]
			//-----------------
			this.dlgチップパレット.t一時的に隠蔽する();

			Cメッセージポップアップ msg
				= new Cメッセージポップアップ( Resources.str初期化中ですMSG + Environment.NewLine + Resources.strしばらくお待ち下さいMSG );
			msg.Owner = this;
			msg.Show();
			msg.Refresh();
			//-----------------
			#endregion

			this.t譜面を初期化する();

			#region [「初期化中です」ポップアップを閉じる。]
			//-----------------
			msg.Close();
			this.dlgチップパレット.t一時的な隠蔽を解除する();

			this.Refresh();			// リスト内容等を消すために再描画
			//-----------------
			#endregion
		}
		private void t譜面を初期化する()
		{
			this.strDTXファイル名 = "";

			// 画面項目の初期化。

			#region [ 基本情報タブの初期化 ]
			//-----------------
			CUndoRedo管理.bUndoRedoした直後 = true;	this.textBox曲名.Clear();
			CUndoRedo管理.bUndoRedoした直後 = true;	this.textBox製作者.Clear();
			CUndoRedo管理.bUndoRedoした直後 = true;	this.textBoxコメント.Clear();
			CUndoRedo管理.bUndoRedoした直後 = true;	this.numericUpDownBPM.Value = 120.0M;
			CUndoRedo管理.bUndoRedoした直後 = true;	this.textBoxDLEVEL.Text = "50";
			CUndoRedo管理.bUndoRedoした直後 = true;	this.textBoxGLEVEL.Text = "0";
			CUndoRedo管理.bUndoRedoした直後 = true;	this.textBoxBLEVEL.Text = "0";
			CUndoRedo管理.bUndoRedoした直後 = true;	this.hScrollBarDLEVEL.Value = 50;
			CUndoRedo管理.bUndoRedoした直後 = true;	this.hScrollBarGLEVEL.Value = 0;
			CUndoRedo管理.bUndoRedoした直後 = true;	this.hScrollBarBLEVEL.Value = 0;
			CUndoRedo管理.bUndoRedoした直後 = true;	this.textBoxパネル.Clear();
			CUndoRedo管理.bUndoRedoした直後 = true;	this.textBoxPREVIEW.Clear();
			CUndoRedo管理.bUndoRedoした直後 = true;	this.textBoxPREIMAGE.Clear();
			CUndoRedo管理.bUndoRedoした直後 = true;	this.textBoxSTAGEFILE.Clear();
			CUndoRedo管理.bUndoRedoした直後 = true;	this.textBoxBACKGROUND.Clear();
			CUndoRedo管理.bUndoRedoした直後 = true;	this.textBoxRESULTIMAGE.Clear();
			//-----------------
			#endregion
			
			#region [ WAVタブ・BMPタブ・AVIタブの初期化 ]
			//-----------------
			this.listViewWAVリスト.Items.Clear();
			this.mgrWAVリスト管理者 = new CWAVリスト管理( this, this.listViewWAVリスト );

			this.listViewBMPリスト.Items.Clear();
			this.mgrBMPリスト管理者 = new CBMPリスト管理( this, this.listViewBMPリスト );

			this.listViewAVIリスト.Items.Clear();
			this.mgrAVIリスト管理者 = new CAVIリスト管理( this, this.listViewAVIリスト );

			this.tWAV・BMP・AVIリストのカーソルを全部同じ行に合わせる( 0 );
			//-----------------
			#endregion
			
			#region [ 自由入力タブの初期化 ]
			//-----------------
			CUndoRedo管理.bUndoRedoした直後 = true;	this.textBox自由入力欄.Clear();
			//-----------------
			#endregion

			#region [ チップパレットの初期化 ]
			//-----------------
			if( this.dlgチップパレット != null )
				this.dlgチップパレット.Close();

			this.dlgチップパレット = new Cチップパレット( this );
			this.dlgチップパレット.Left = this.Left + ( this.Width - this.dlgチップパレット.Width ) / 2;
			this.dlgチップパレット.Top = this.Top + ( this.Height - this.dlgチップパレット.Height ) / 2;
			this.dlgチップパレット.Owner = this;

			if( this.toolStripButtonチップパレット.CheckState == CheckState.Checked )
				this.dlgチップパレット.t表示する();
			//-----------------
			#endregion

			#region [ 譜面の生成・初期化 ]
			//-----------------
			if ( this.mgr譜面管理者 == null )		// 初回起動時は、レーン表示有無の構成に初期値を使用(して、後でDTXCreatorConfig.settingsのものに置き換える)
			{
				this.mgr譜面管理者 = new C譜面管理( this );
				this.mgr譜面管理者.t初期化();
			}
			else									// 起動後のdtxファイル読み込み等の場合は、直前のレーン表示有無の構成を踏襲する
			{
				#region [ レーン表示/非表示状態の待避 #26005 2011.8.30 yyagi; added ]
				List<Cレーン> lc = new List<Cレーン>(this.mgr譜面管理者.listレーン);
				#endregion

				this.mgr譜面管理者 = new C譜面管理( this );
				this.mgr譜面管理者.t初期化();

				#region [ レーン表示/非表示の反映 #26005 2011.8.30 yyagi; added ]
				for ( int i = 0; i < this.mgr譜面管理者.listレーン.Count; i++ )
				{
					this.mgr譜面管理者.listレーン[ i ].bIsVisible = lc[ i ].bIsVisible;
				}
				this.mgr譜面管理者.tRefreshDisplayLanes();
				#endregion
			}
			//-----------------
			#endregion

			#region [ DTXViewer 関連 GUI の初期化 ]
			//-----------------
			this.tDTXV演奏関連のボタンとメニューのEnabledの設定();
			//-----------------
			#endregion

			#region [ ガイド間隔の初期値を設定する。]
			//-----------------
			this.tガイド間隔を変更する( 16 );
			//-----------------
			#endregion


			// 内部処理の初期化。

			#region [ Undo/Redoリストのリセット ]
			//-----------------
			this.mgrUndoRedo管理者 = new CUndoRedo管理();

			CUndoRedo管理.bUndoRedoした直後 = false;
			this.tUndoRedo用GUIの有効・無効を設定する();
			//-----------------
			#endregion

			#region [ 「２大モード」の管理者を生成、初期は編集モードにする。]
			//-----------------
			this.mgr選択モード管理者 = new C選択モード管理( this );
			this.mgr編集モード管理者 = new C編集モード管理( this );
			
			this.t編集モードにする();
			//-----------------
			#endregion


			// 上記のプロパティ変更操作により未保存フラグがtrueになってしまってるので、元に戻す。

			#region [ 未保存フラグをクリアする。]
			//-----------------
			this.b未保存 = false;
			//-----------------
			#endregion
		}
		//-----------------
		#endregion
		#region [ 開く ]
		//-----------------
		private void tシナリオ・開く()
		{
			// 作成前の保存確認。

			#region [ 未保存なら保存する。→ キャンセルされた場合はここで中断。]
			//-----------------
			if( this.t未保存なら保存する() == DialogResult.Cancel )
				return;	// 中断
			//-----------------
			#endregion


			// 開くファイルを選択させる。

			#region [ 「ファイルを開く」ダイアログでファイルを選択する。 ]
			//-----------------
			this.dlgチップパレット.t一時的に隠蔽する();

			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Title = Resources.strDTXファイル選択ダイアログのタイトル;
			dialog.Filter = Resources.strDTXファイル選択ダイアログのフィルタ;
			dialog.FilterIndex = 1;
			dialog.InitialDirectory = this.str作業フォルダ名;
			DialogResult result = dialog.ShowDialog();

			this.dlgチップパレット.t一時的な隠蔽を解除する();
			this.Refresh();     // メインフォームを再描画してダイアログを完全に消す

			if( result != DialogResult.OK )
				return;
			//-----------------
			#endregion


			// 選択されたファイルを読み込む。

			#region [ ファイルを読み込む。]
			//-----------------
			this.t演奏ファイルを開いて読み込む( dialog.FileName );
			//-----------------
			#endregion
		}
		private void tシナリオ・DragDropされたファイルを開く( string[] DropFiles )
		{
			// 開くファイルを決定する。

			#region [ Dropされたファイルが複数個ある → 先頭のファイルだけを有効とする。 ]
			//-----------------
			string strファイル名 = DropFiles[ 0 ];
			//-----------------
			#endregion


			// 開く前の保存確認。

			#region [ 未保存なら保存する。→ キャンセルされた場合はここで中断。]
			//-----------------
			if( this.t未保存なら保存する() == DialogResult.Cancel )
				return;	// 中断
			//-----------------
			#endregion


			// Drop されたファイルを読み込む。

			#region [ ファイルを読み込む。]
			//-----------------
			this.t演奏ファイルを開いて読み込む( strファイル名 );
			//-----------------
			#endregion
		}
		private void t演奏ファイルを開いて読み込む( string strファイル名 )
		{
			// 前処理。

			#region [ ファイルの存在を確認する。なかったらその旨を表示して中断する。]
			//-----------------
			if( !File.Exists( strファイル名 ) )
			{
				MessageBox.Show(
					Resources.strファイルが存在しませんMSG,
					Resources.str確認ダイアログのタイトル,
					MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1 );

				return;	// 中断
			}
			//-----------------
			#endregion

			#region [ 拡張子からデータ種別を判定する。]
			//-----------------
			
			CDTX入出力.E種別 e種別 = CDTX入出力.E種別.DTX;

			string str拡張子 = Path.GetExtension( strファイル名 );

			if( str拡張子.Equals( ".dtx", StringComparison.OrdinalIgnoreCase ) )
			{
				e種別 = CDTX入出力.E種別.DTX;
			}
			else if( str拡張子.Equals( ".gda", StringComparison.OrdinalIgnoreCase ) )
			{
				e種別 = CDTX入出力.E種別.GDA;
			}
			else if( str拡張子.Equals( ".g2d", StringComparison.OrdinalIgnoreCase ) )
			{
				e種別 = CDTX入出力.E種別.G2D;
			}
			else if( str拡張子.Equals( ".bms", StringComparison.OrdinalIgnoreCase ) )
			{
				e種別 = CDTX入出力.E種別.BMS;
			}
			else if( str拡張子.Equals( ".bme", StringComparison.OrdinalIgnoreCase ) )
			{
				e種別 = CDTX入出力.E種別.BME;
			}
			else
			{
				MessageBox.Show(
					Resources.strDTXファイルではありませんMSG,
					Resources.str確認ダイアログのタイトル,
					MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1 );

				return;	// 中断
			}
			//-----------------
			#endregion

			this.dlgチップパレット.t一時的に隠蔽する();

			#region [「読み込み中です」ポップアップを表示する。]
			//-----------------
			Cメッセージポップアップ msg
				= new Cメッセージポップアップ( Resources.str読み込み中ですMSG + Environment.NewLine + Resources.strしばらくお待ち下さいMSG );
			msg.Owner = this;
			msg.Show();
			msg.Refresh();
			//-----------------
			#endregion


			// 読み込む。


			this.t譜面を初期化する();

			#region [ DTXファイルを読み込む。]
			//-----------------

			// 全部を１つの string として読み込む。

			StreamReader reader = new StreamReader( strファイル名, Encoding.GetEncoding( 932/*Shift-JIS*/ ) );
			string str全入力文字列 = reader.ReadToEnd();
			reader.Close();


			// その string から DTX データを読み込む。

			new CDTX入出力( this ).tDTX入力( e種別, ref str全入力文字列 );


			// ファイル名、作業フォルダ名を更新する。

			this.strDTXファイル名 = Path.ChangeExtension( Path.GetFileName( strファイル名 ), ".dtx" );		// 拡張子は強制的に .dtx に変更。
			this.str作業フォルダ名 = Path.GetDirectoryName( strファイル名 ) + @"\";		// 読み込み後、カレントフォルダは、作業ファイルのあるフォルダに移動する。

			//-----------------
			#endregion

			#region [ 読み込んだファイルを [ファイル]メニューの最近使ったファイル一覧に追加する。]
			//-----------------
			this.appアプリ設定.AddRecentUsedFile( this.str作業フォルダ名 + this.strDTXファイル名 );
			this.t最近使ったファイルをFileメニューへ追加する();
			//-----------------
			#endregion

			#region [ DTX以外を読み込んだ場合は、（DTXに変換されているので）最初から未保存フラグを立てる。]
			//-----------------
			if( e種別 != CDTX入出力.E種別.DTX )
				this.b未保存 = true;
			//-----------------
			#endregion


			#region [「読み込み中です」ポップアップを閉じる。 ]
			//-----------------
			msg.Close();
			this.Refresh();	// リスト内容等を消すために再描画する。
			//-----------------
			#endregion

			#region [ 未保存フラグをクリアする。]
			//-----------------
			this.b未保存 = true;	// ウィンドウタイトルを書き換えるため、一度未保存フラグをtrueにして b未保存の setter を作動させる。
			this.b未保存 = false;
			//-----------------
			#endregion
		}
		//-----------------
		#endregion
		#region [ 上書き保存／名前をつけて保存 ]
		//-----------------
		private void tシナリオ・上書き保存()
		{
			// 前処理。

			this.dlgチップパレット.t一時的に隠蔽する();

			#region [「保存中です」ポップアップを表示する。 ]
			//-----------------
			var msg = new Cメッセージポップアップ( Resources.str保存中ですMSG + Environment.NewLine + Resources.strしばらくお待ち下さいMSG );
			msg.Owner = this;
			msg.Show();
			msg.Refresh();
			//-----------------
			#endregion

			#region [ ファイル名がない → 初めての保存と見なし、ファイル保存ダイアログで保存ファイル名を指定させる。 ]
			//-----------------
			if( string.IsNullOrEmpty( this.strDTXファイル名 ) )
			{
				// ダイアログでファイル名を取得する。

				string str絶対パスファイル名 = this.tファイル保存ダイアログを開いてファイル名を取得する();

				if( string.IsNullOrEmpty( str絶対パスファイル名 ) )
					return;	// ファイル保存ダイアログがキャンセルされたのならここで打ち切り。

				//this.str作業フォルダ名 = Directory.GetCurrentDirectory() + @"\";	// ダイアログでディレクトリを変更した場合、カレントディレクトリも変更されている。
				this.str作業フォルダ名 = Path.GetDirectoryName(str絶対パスファイル名) + @"\";
				this.strDTXファイル名 = Path.GetFileName( str絶対パスファイル名 );


				// WAV・BMP・AVIリストにあるすべてのファイル名を、作業フォルダに対する相対パスに変換する。

				this.mgrWAVリスト管理者.tファイル名の相対パス化( this.str作業フォルダ名 );
				this.mgrBMPリスト管理者.tファイル名の相対パス化( this.str作業フォルダ名 );
				this.mgrAVIリスト管理者.tファイル名の相対パス化( this.str作業フォルダ名 );
			}
			//-----------------
			#endregion


			// DTXファイルへ出力。


			#region [ 選択モードだったなら選択を解除する。]
			//-----------------
			if( this.b選択モードである )
				this.mgr選択モード管理者.t全チップの選択を解除する();
			//-----------------
			#endregion

			#region [ DTXファイルを出力する。]
			//-----------------
			var sw = new StreamWriter( this.str作業フォルダ名 + this.strDTXファイル名, false, Encoding.GetEncoding( 932/*Shift-JIS*/ ) );
			new CDTX入出力( this ).tDTX出力( sw );
			sw.Close();
			//-----------------
			#endregion

			#region [ 出力したファイルのパスを、[ファイル]メニューの最近使ったファイル一覧に追加する。 ]
			//-----------------
			this.appアプリ設定.AddRecentUsedFile( this.str作業フォルダ名 + this.strDTXファイル名 );
			this.t最近使ったファイルをFileメニューへ追加する();
			//-----------------
			#endregion


			// 後処理。

			#region [「保存中です」ポップアップを閉じる。]
			//-----------------
			msg.Close();
			this.Refresh();		// リスト内容等を消すために再描画する。
			//-----------------
			#endregion

			this.dlgチップパレット.t一時的な隠蔽を解除する();
			this.b未保存 = false;
		}
		private void tシナリオ・名前をつけて保存()
		{
			// 前処理。

			#region [ ユーザに保存ファイル名を入力させる。]
			//-----------------
			// ファイル保存ダイアログを表示し、出力するファイル名を指定させる。キャンセルされたらここで中断。

			string str絶対パスファイル名 = this.tファイル保存ダイアログを開いてファイル名を取得する();
			if( string.IsNullOrEmpty( str絶対パスファイル名 ) )
				return;	// 中断


			// フォルダ名とファイル名を更新。

			//this.str作業フォルダ名 = Directory.GetCurrentDirectory() + @"\";	// ダイアログでディレクトリを変更した場合は、カレントディレクトリも変更されている。
			this.str作業フォルダ名 = Path.GetDirectoryName(str絶対パスファイル名) + @"\";
			this.strDTXファイル名 = Path.GetFileName( str絶対パスファイル名 );
			//-----------------
			#endregion

			#region [ WAV・BMP・AVIリストにあるすべてのファイル名を、作業フォルダに対する相対パスに変換する。 ]
			//-----------------
			this.mgrWAVリスト管理者.tファイル名の相対パス化( this.str作業フォルダ名 );
			this.mgrBMPリスト管理者.tファイル名の相対パス化( this.str作業フォルダ名 );
			this.mgrAVIリスト管理者.tファイル名の相対パス化( this.str作業フォルダ名 );
			//-----------------
			#endregion


			// 保存する。
			
			this.tシナリオ・上書き保存();


			// 後処理。

			this.b未保存 = true;	// ウィンドウタイトルに表示されているファイル名を変更するため一度 true にする。
			this.b未保存 = false;
		}
		private string tファイル保存ダイアログを開いてファイル名を取得する()
		{
			// ダイアログでファイル名を取得。

			this.dlgチップパレット.t一時的に隠蔽する();

			var dialog = new SaveFileDialog() {
				Title = "名前をつけて保存",
				Filter = "DTXファイル(*.dtx)|*.dtx",
				FilterIndex = 1,
				InitialDirectory = this.str作業フォルダ名
			};
			DialogResult result = dialog.ShowDialog();

			this.dlgチップパレット.t一時的な隠蔽を解除する();


			// 画面を再描画。

			this.Refresh();


			// キャンセルされたら "" を返す。

			if( result != DialogResult.OK )
				return "";


			// ファイルの拡張子を .dtx に変更。

			string fileName = dialog.FileName;
			if( Path.GetExtension( fileName ).Length == 0 )
				fileName = Path.ChangeExtension( fileName, ".dtx" );

			return fileName;
		}
		//-----------------
		#endregion
		#region [ 終了 ]
		//-----------------
		private void tシナリオ・終了()
		{
			// ウィンドウを閉じる。

			this.Close();
		}
		//-----------------
		#endregion
		#region [ 検索／置換 ]
		//-----------------
		private void tシナリオ・検索()
		{
			this.mgr選択モード管理者.t検索する();	// モードによらず、検索はすべて選択モード管理者が行う。
		}
		private void tシナリオ・置換()
		{
			this.mgr選択モード管理者.t置換する();	// モードによらず、置換はすべて選択モード管理者が行う。
		}
		//-----------------
		#endregion
		#region [ 小節長変更／小節の挿入／小節の削除 ]
		//-----------------
		private void tシナリオ・小節長を変更する( C小節 cs )
		{
			// 前処理。

			#region [ 小節長をユーザに入力させる。]
			//-----------------

			// 小節長ダイアログを表示し、小節長を取得する。

			this.dlgチップパレット.t一時的に隠蔽する();

			var dlg = new C小節長変更ダイアログ( cs.n小節番号0to3599 );
			dlg.f倍率 = cs.f小節長倍率;
			dlg.b後続変更 = false;

			this.dlgチップパレット.t一時的な隠蔽を解除する();


			// キャンセルされたらここで中断。
			if( dlg.ShowDialog() != DialogResult.OK )
				return;

			//-----------------
			#endregion


			// 小節長を変更。

			#region [ 小節長を変更する。]
			//-----------------
			int n変更開始小節番号 = cs.n小節番号0to3599;
			int n変更終了小節番号 = ( dlg.b後続変更 ) ? this.mgr譜面管理者.n現在の最大の小節番号を返す() : cs.n小節番号0to3599;

			for( int i = n変更開始小節番号; i <= n変更終了小節番号; i++ )
				this.t小節長を変更する・小節単位( i, dlg.f倍率 );

			//-----------------
			#endregion


			// 後処理。

			this.b未保存 = true;
		}
		private void t小節長を変更する・小節単位( int n小節番号, float f倍率 )
		{
			// 対象の小節を取得。

			#region [ 小節番号から小節オブジェクトを取得する。→ 指定された小節が存在しない場合はここで中断。]
			//-----------------
			
			C小節 c小節 = this.mgr譜面管理者.p小節を返す( n小節番号 );
	
			if( c小節 == null )
				return;	// 中断
			
			//-----------------
			#endregion


			// 作業記録開始。

			this.mgrUndoRedo管理者.tトランザクション記録を開始する();

			#region [ UndoRedo リストにこの操作（小節長変更）を記録する。 ]
			//-----------------
			var ur変更前 = new C小節用UndoRedo( c小節.n小節番号0to3599, c小節.f小節長倍率 );
			var ur変更後 = new C小節用UndoRedo( c小節.n小節番号0to3599, f倍率 );

			this.mgrUndoRedo管理者.tノードを追加する( 
				new CUndoRedoセル<C小節用UndoRedo>(
					null,
					new DGUndoを実行する<C小節用UndoRedo>( this.mgr譜面管理者.t小節長変更のUndo ),
					new DGRedoを実行する<C小節用UndoRedo>( this.mgr譜面管理者.t小節長変更のRedo ),
					ur変更前, ur変更後 ) );
			//-----------------
			#endregion


			// 小節長倍率を変更。

			#region [ 小節長倍率を変更する。]
			//-----------------
			c小節.f小節長倍率 = f倍率;
			//-----------------
			#endregion

			#region [ 小節からはみ出したチップを削除する。チップの削除操作は Undo/Redo に記録する。]
			//-----------------
			for( int i = 0; i < c小節.listチップ.Count; i++ )
			{
				Cチップ cチップ = c小節.listチップ[ i ];

				if( cチップ.n位置grid >= c小節.n小節長倍率を考慮した現在の小節の高さgrid )
				{

					#region [ UndoRedo リストにこの操作（チップ削除）を記録する。]
					//-----------------
					var cc = new Cチップ();
					cc.tコピーfrom( cチップ );
					
					var ur = new Cチップ配置用UndoRedo( c小節.n小節番号0to3599, cc );

					this.mgrUndoRedo管理者.tノードを追加する(
						new CUndoRedoセル<Cチップ配置用UndoRedo>( 
							null, 
							new DGUndoを実行する<Cチップ配置用UndoRedo>( this.mgr譜面管理者.tチップ削除のUndo ), 
							new DGRedoを実行する<Cチップ配置用UndoRedo>( this.mgr譜面管理者.tチップ削除のRedo ),
							ur, ur ) );
					//-----------------
					#endregion

		
					// チップを小節のチップリストから削除する。

					c小節.listチップ.RemoveAt( i );


					// リストが更新されたので、最初のチップから見直す。
					
					i = -1;
				}
			}
			//-----------------
			#endregion


			// 作業記録終了。

			this.mgrUndoRedo管理者.tトランザクション記録を終了する();


			// 画面を再描画。

			this.tUndoRedo用GUIの有効・無効を設定する();
		}
		private void tシナリオ・小節を挿入する( int n挿入位置の小節番号 )
		{
			// 作業を記録。

			#region [ UndoRedo リストにこの操作（小節挿入）を記録する。]
			//-----------------
			this.mgrUndoRedo管理者.tノードを追加する(
				new CUndoRedoセル<int>(
					null, 
					new DGUndoを実行する<int>( this.mgr譜面管理者.t小節挿入のUndo ),
					new DGRedoを実行する<int>( this.mgr譜面管理者.t小節挿入のRedo ),
					n挿入位置の小節番号, n挿入位置の小節番号 ) );
			//-----------------
			#endregion


			// 小節を挿入。
			
			#region [ 挿入位置以降の小節を１つずつ後ろにずらす（小節番号を +1 していく）。 ]
			//-----------------
			for( int i = this.mgr譜面管理者.n現在の最大の小節番号を返す(); i >= n挿入位置の小節番号; i-- )
			{
				// ずらす小節オブジェクトを取得する。

				C小節 cずらす小節 = this.mgr譜面管理者.p小節を返す( i );
				if( cずらす小節 == null )
					continue;


				// 小節番号を＋１する。
				
				this.mgr譜面管理者.dic小節.Remove( i );		// 小節番号は Dictionary のキー値であるため、番号が変われば再登録が必要。
				cずらす小節.n小節番号0to3599 = i + 1;
				this.mgr譜面管理者.dic小節.Add( cずらす小節.n小節番号0to3599, cずらす小節 );
			}
			//-----------------
			#endregion

			#region [ 新しい小節を作成し、譜面の持つ小節リストに追加する。 ]
			//-----------------

			// 小節を該当位置に追加する。

			this.mgr譜面管理者.dic小節.Add( n挿入位置の小節番号, new C小節( n挿入位置の小節番号 ) );

	
			// 譜面を再描画する。

			this.pictureBox譜面パネル.Refresh();

			//-----------------
			#endregion


			// 後処理。

			this.tUndoRedo用GUIの有効・無効を設定する();
			this.b未保存 = true;
		}
		private void tシナリオ・小節を削除する( int n削除位置の小節番号 )
		{
			// 作業記録開始。

			this.mgrUndoRedo管理者.tトランザクション記録を開始する();


			// 小節を削除。

			#region [ 最大小節番号を取得する。]
			//-----------------
			int n最大小節番号 = this.mgr譜面管理者.n現在の最大の小節番号を返す();	// 小節を削除すると数が変わるので、削除前に取得する。
			//-----------------
			#endregion
			#region [ 削除する小節オブジェクトを取得する。]
			//-----------------
			C小節 c削除する小節 = this.mgr譜面管理者.p小節を返す( n削除位置の小節番号 );
			//-----------------
			#endregion

			#region [ その小節が持っているチップを全て削除する。チップの削除作業は、Undo/Redoリストに記録する。]
			//-----------------
			
			while( c削除する小節.listチップ.Count > 0 )
			{
				#region [ UndoRedo リストにこの操作（チップ削除）を記録する。]
				//-----------------
				var cc = new Cチップ();
				cc.tコピーfrom( c削除する小節.listチップ[ 0 ] );
				var redo = new Cチップ配置用UndoRedo( c削除する小節.n小節番号0to3599, cc );

				this.mgrUndoRedo管理者.tノードを追加する(
					new CUndoRedoセル<Cチップ配置用UndoRedo>(
						null,
						new DGUndoを実行する<Cチップ配置用UndoRedo>( this.mgr譜面管理者.tチップ削除のUndo ),
						new DGRedoを実行する<Cチップ配置用UndoRedo>( this.mgr譜面管理者.tチップ削除のRedo ),
						redo, redo ) );
				//-----------------
				#endregion

				// 小節からチップを削除する。

				c削除する小節.listチップ.RemoveAt( 0 );
			}
			
			//-----------------
			#endregion

			#region [ UndoRedo リストにこの操作（小節削除）を記録する。]
			//-----------------
			this.mgrUndoRedo管理者.tノードを追加する(
				new CUndoRedoセル<int>( 
					null, 
					new DGUndoを実行する<int>( this.mgr譜面管理者.t小節削除のUndo ),
					new DGRedoを実行する<int>( this.mgr譜面管理者.t小節削除のRedo ),
					n削除位置の小節番号, n削除位置の小節番号 ) );
			//-----------------
			#endregion
			#region [ 該当小節を譜面の小節リストから削除する。]
			//-----------------
			this.mgr譜面管理者.dic小節.Remove( n削除位置の小節番号 );
			//-----------------
			#endregion
			#region [ 削除した小節より後方にある小節を１つずつ前にずらす。（小節番号を -1 していく）]
			//-----------------

			for( int i = n削除位置の小節番号 + 1; i <= n最大小節番号; i++ )
			{
				// 小節オブジェクトを取得する。

				C小節 cずらす小節 = this.mgr譜面管理者.p小節を返す( i );
				if( cずらす小節 == null )
					continue;

				// 小節番号を－１する。

				this.mgr譜面管理者.dic小節.Remove( i );		// 小節番号は Dictionary のキー値であるため、番号が変われば再登録が必要。
				cずらす小節.n小節番号0to3599--;
				this.mgr譜面管理者.dic小節.Add( cずらす小節.n小節番号0to3599, cずらす小節 );
			}

			// 譜面内の小節が全部無くなったらさすがにまずいので、最低１個の小節は残す。

			if( this.mgr譜面管理者.dic小節.Count == 0 )
				this.mgr譜面管理者.dic小節.Add( 0, new C小節( 0 ) );

			//-----------------
			#endregion


			// 作業記録終了。

			this.mgrUndoRedo管理者.tトランザクション記録を終了する();

	
			// 後処理。

			this.tUndoRedo用GUIの有効・無効を設定する();
			this.pictureBox譜面パネル.Refresh();
			this.b未保存 = true;
		}
		//-----------------
		#endregion
		#region [ 選択チップの切り取り／コピー／貼り付け／削除 ]
		//-----------------
		private void tシナリオ・切り取り()
		{
			// 事前チェック。

			#region [ 譜面にフォーカスが来てないなら何もしない。 ]
			//-----------------
			if( !this.pictureBox譜面パネル.Focused )
				return;
			//-----------------
			#endregion


			// 切り取り。

			#region [ 切り取り ＝ コピー ＋ 削除 ]
			//-----------------
			this.tシナリオ・コピー();
			this.tシナリオ・削除();
			//-----------------
			#endregion
		}
		private void tシナリオ・コピー()
		{
			// 事前チェック。

			#region [ 譜面にフォーカスが来てないなら何もしない。 ]
			//-----------------
			if( !this.pictureBox譜面パネル.Focused )
				return;
			//-----------------
			#endregion


			// コピー。

			this.cbクリップボード.t現在選択されているチップをボードにコピーする();


			// 画面を再描画。

			#region [ 画面を再描画する。]
			//-----------------
			this.t選択チップの有無に応じて編集用GUIの有効・無効を設定する();
			this.pictureBox譜面パネル.Refresh();
			//-----------------
			#endregion
		}
		private void tシナリオ・貼り付け( int n譜面先頭からの位置grid )
		{
			// 事前チェック。

			#region [ 譜面にフォーカスが来てないなら何もしない。 ]
			//-----------------
			if( !this.pictureBox譜面パネル.Focused )
				return;
			//-----------------
			#endregion


			// 貼り付け。

			#region [ 貼り付け先の小節と貼り付け開始位置を取得する。]
			//-----------------
			C小節 c小節 = this.mgr譜面管理者.p譜面先頭からの位置gridを含む小節を返す( n譜面先頭からの位置grid );
			if( c小節 == null )
				return;	// 中断

			int n小節先頭からの位置grid =
				n譜面先頭からの位置grid - this.mgr譜面管理者.n譜面先頭からみた小節先頭の位置gridを返す( c小節.n小節番号0to3599 );
			//-----------------
			#endregion
			
			#region [ クリップボードからチップを貼り付ける。]
			//-----------------
			this.cbクリップボード.tチップを指定位置から貼り付ける( c小節, n小節先頭からの位置grid );
			//-----------------
			#endregion


			// 画面の再描画。

			#region [ 画面を再描画する。]
			//-----------------
			this.t選択チップの有無に応じて編集用GUIの有効・無効を設定する();
			this.pictureBox譜面パネル.Refresh();
			//-----------------
			#endregion
		}
		private void tシナリオ・削除()
		{
			// 事前チェック。

			#region [ 譜面にフォーカスが来てないなら何もしない。 ]
			//-----------------
			if( !this.pictureBox譜面パネル.Focused )
				return;
			//-----------------
			#endregion


			// 操作記録開始。

			this.mgrUndoRedo管理者.tトランザクション記録を開始する();


			// チップを削除。

			#region [ 譜面が持つすべての小節について、選択されているチップがあれば削除する。]
			//-----------------
			foreach( KeyValuePair<int, C小節> pair in this.mgr譜面管理者.dic小節 )
			{
				C小節 c小節 = pair.Value;

				bool b削除されたチップがある = false;
				bool b削除完了 = false;

				while( !b削除完了 )
				{
					#region [ 小節の持つチップのうち、選択されているチップがあれば削除してループする。なくなったら抜ける。]
					//-----------------
					
					b削除完了 = true;

					// 小節が持つすべてのチップについて……
					foreach( Cチップ cチップ in c小節.listチップ )
					{
						if( cチップ.b確定選択中 )
						{
							#region [ UndoRedo リストにこの操作（チップ削除）を記録する。]
							//-----------------
							var cc = new Cチップ();
							cc.tコピーfrom( cチップ );
							var redo = new Cチップ配置用UndoRedo( c小節.n小節番号0to3599, cc );

							this.mgrUndoRedo管理者.tノードを追加する(
								new CUndoRedoセル<Cチップ配置用UndoRedo>(
									null,
									new DGUndoを実行する<Cチップ配置用UndoRedo>( this.mgr譜面管理者.tチップ削除のUndo ),
									new DGRedoを実行する<Cチップ配置用UndoRedo>( this.mgr譜面管理者.tチップ削除のRedo ),
									redo, redo ) );
							//-----------------
							#endregion


							// チップオブジェクトを削除する。

							c小節.listチップ.Remove( cチップ );
							
							
							// フラグを設定してループする。

							b削除完了 = false;		// まだ終わらんよ
							b削除されたチップがある = true;
							break;
						}
					}
					//-----------------
					#endregion
				}

				#region [ 1つでもチップを削除したなら、未保存フラグを立てる。 ]
				//-----------------
				if( b削除されたチップがある )
					this.b未保存 = true;
				//-----------------
				#endregion
			}
			//-----------------
			#endregion


			// 操作記録終了。

			this.mgrUndoRedo管理者.tトランザクション記録を終了する();


			// 画面を再描画する。

			this.tUndoRedo用GUIの有効・無効を設定する();
			this.t選択チップの有無に応じて編集用GUIの有効・無効を設定する();
			this.pictureBox譜面パネル.Refresh();
		}
		//-----------------
		#endregion
		#region [ DTXViewer での再生・停止 ]
		//-----------------
		private void tシナリオ・Viewerで最初から再生する()
		{
			this.tViewer用の一時ファイルを出力する( false );

			#region [ 再生開始オプション引数に一時ファイルを指定して DTXViewer プロセスを起動する。]
			//-----------------
			try
			{
				string strDTXViewerのパス = this.strDTXCのあるフォルダ名 + this.appアプリ設定.ViewerInfo.Path;

				#region [ DTXViewer が起動していなければ起動する。]
				//-----------------
				Process.Start( strDTXViewerのパス ).WaitForInputIdle( 20 * 1000 );	// 起動完了まで最大20秒待つ
				//-----------------
				#endregion
				
				#region [ 実行中の DTXViewer に再生オプションを渡す。 ]
				//-----------------
				Process.Start( strDTXViewerのパス, 
					this.appアプリ設定.ViewerInfo.PlayStartOption + " " + this.strViewer演奏用一時ファイル名 );
				//-----------------
				#endregion
			}
			catch( Exception )
			{
				#region [ 失敗ダイアログを表示する。]
				//-----------------
				MessageBox.Show(
					Resources.strプロセスの起動に失敗しましたMSG,
					Resources.strエラーダイアログのタイトル,
					MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1 );
				//-----------------
				#endregion
			}
			//-----------------
			#endregion
		}
		private void tシナリオ・Viewerで現在位置から再生する()
		{
			this.tViewer用の一時ファイルを出力する( false );

			try
			{
				string strDTXViewerのパス = this.strDTXCのあるフォルダ名 + this.appアプリ設定.ViewerInfo.Path;

				#region [ DTXViewer が起動していなければ起動する。]
				//-----------------
				
				Process.Start( strDTXViewerのパス ).WaitForInputIdle( 20 * 1000 );	// 起動完了まで最大20秒待つ
				
				//-----------------
				#endregion

				#region [ 実行中の DTXViewer に再生オプションを渡す。 ]
				//-----------------
				
				C小節 c小節 =
					this.mgr譜面管理者.p譜面先頭からの位置gridを含む小節を返す( this.mgr譜面管理者.n現在の譜面表示下辺の譜面先頭からの位置grid );
				
				Process.Start( strDTXViewerのパス,
					this.appアプリ設定.ViewerInfo.PlayStartFromOption + c小節.n小節番号0to3599 + " " + this.strViewer演奏用一時ファイル名 );
				
				//-----------------
				#endregion
			}
			catch( Exception )
			{
				#region [ 失敗ダイアログを表示する。]
				//-----------------
				MessageBox.Show(
					Resources.strプロセスの起動に失敗しましたMSG,
					Resources.strエラーダイアログのタイトル, 
					MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1 );
				//-----------------
				#endregion
			}
		}
		private void tシナリオ・Viewerで現在位置からBGMのみ再生する()
		{
			#region [ DTXViewer 用の一時ファイルを出力する。]
			//-----------------
			this.tViewer用の一時ファイルを出力する( true );
			//-----------------
			#endregion

			try
			{
				string strDTXViewerのパス = this.strDTXCのあるフォルダ名 + this.appアプリ設定.ViewerInfo.Path;

				#region [ DTXViewer が起動していなければ起動する。]
				//-----------------
				Process.Start( strDTXViewerのパス ).WaitForInputIdle( 20 * 1000 );	// 起動完了まで最大20秒待つ
				//-----------------
				#endregion

				#region [ 実行中の DTXViewer に再生オプションを渡す。 ]
				//-----------------
				C小節 c小節 = this.mgr譜面管理者.p譜面先頭からの位置gridを含む小節を返す( this.mgr譜面管理者.n現在の譜面表示下辺の譜面先頭からの位置grid );
				Process.Start( strDTXViewerのパス,
					this.appアプリ設定.ViewerInfo.PlayStartFromOption + c小節.n小節番号0to3599 + " " + this.strViewer演奏用一時ファイル名 );
				//-----------------
				#endregion
			}
			catch( Exception )
			{
				#region [ 失敗ダイアログを表示する。]
				//-----------------
				MessageBox.Show(
					Resources.strプロセスの起動に失敗しましたMSG,
					Resources.strエラーダイアログのタイトル,
					MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1 );
				//-----------------
				#endregion
			}
		}
		private void tシナリオ・Viewerを再生停止する()
		{
			try
			{
				string strViewerのパス = this.strDTXCのあるフォルダ名 + this.appアプリ設定.ViewerInfo.Path;

				#region [ 実行中の DTXViewer に再生停止オプションを渡す。 ]
				//-----------------

				// 停止のときは１回のプロセス起動で完結(BMSV仕様)

				Process.Start( strViewerのパス, this.appアプリ設定.ViewerInfo.PlayStopOption );

				//-----------------
				#endregion

			}
			catch( Exception )
			{
				#region [ 失敗ダイアログを表示する。]
				//-----------------
				MessageBox.Show(
					Resources.strプロセスの起動に失敗しましたMSG,
					Resources.strエラーダイアログのタイトル,
					MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1 );
				//-----------------
				#endregion
			}
		}

		private string strViewer演奏用一時ファイル名 = "";
		private void tViewer用の一時ファイルを出力する( bool bBGMのみ出力 )
		{
			// 一時ファイル名を自動生成。

			//this.strViewer演奏用一時ファイル名 = Path.GetTempFileName();			//
			this.strViewer演奏用一時ファイル名 = makeTempDTX.GetTempFileName();		// #24746 2011.4.1 yyagi add; a countermeasure for temp-flooding
			
			// 一時ファイルにDTXを出力。

			this.mgr譜面管理者.strPATH_WAV = this.str作業フォルダ名;

			try
			{
				#region [ もし小数点にコンマを使うcultureなら、一時的に(小数点を使う)"en-GB"に切り替える。(DTXVはピリオドしか使えないため) ]
				string currentCultureEnglishName = CultureInfo.CurrentCulture.Name;
				bool bSwitchCulture = false;
				if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
				{
					Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB", false);	// #24241, #24790 2011.4.8 yyagi: switch culture where the country uses period as the decimal point
					bSwitchCulture = true;													// I mistook here using CurrentUICulture. Use CurrentCulture to work correctly.
				}
				#endregion
				#region [ 一時ファイルにDTXを出力する。 ]
				//-----------------
				StreamWriter sw = new StreamWriter( this.strViewer演奏用一時ファイル名, false, Encoding.GetEncoding( 0x3a4 ) );
				new CDTX入出力( this ).tDTX出力( sw, bBGMのみ出力 );
				sw.Close();
				//-----------------
				#endregion
				#region [ cultureを元に戻す。 ]
				if (bSwitchCulture)
				{
					Thread.CurrentThread.CurrentCulture = new CultureInfo(currentCultureEnglishName, false);
				}
				#endregion
			}
			finally
			{
				this.mgr譜面管理者.strPATH_WAV = "";
			}
		}
		//-----------------
		#endregion
		#region [ Undo / Redo ]
		//-----------------
		private void tシナリオ・Undoする()
		{
			// Undo を実行する。

			#region [ Undo する対象を Undo/Redo リストから取得する。]
			//-----------------
	
			CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す();

			if( oセル仮想 == null )
				return;		// なければ中断
			
			//-----------------
			#endregion
			
			oセル仮想.tUndoを実行する();


			// GUIを再描画する。

			#region [ GUI を再描画する。]
			//-----------------
			this.tUndoRedo用GUIの有効・無効を設定する();
			this.t選択チップの有無に応じて編集用GUIの有効・無効を設定する();
			this.pictureBox譜面パネル.Refresh();
			//-----------------
			#endregion
		}
		private void tシナリオ・Redoする()
		{
			// Redo を実行する。

			#region [ Redo する対象を Undo/Redo リストから取得する。]
			//-----------------

			CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tRedoするノードを取得して返す();

			if( oセル仮想 == null )
				return;	// なければ中断

			//-----------------
			#endregion

			oセル仮想.tRedoを実行する();


			// GUI を再描画する。

			#region [ GUI を再描画する。]
			//-----------------
			this.tUndoRedo用GUIの有効・無効を設定する();
			this.t選択チップの有無に応じて編集用GUIの有効・無効を設定する();
			this.pictureBox譜面パネル.Refresh();
			//-----------------
			#endregion
		}
		//-----------------
		#endregion


		// メソッド

		public enum Eタブ種別 : int
		{
			基本情報 = 0,
			WAV = 1,
			BMP = 2,
			AVI = 3,
			自由入力 = 4
		}

		public void t選択モードにする()
		{
			this.toolStripButton選択モード.CheckState = CheckState.Checked;
			this.toolStripButton編集モード.CheckState = CheckState.Unchecked;
			this.toolStripMenuItem選択モード.CheckState = CheckState.Checked;
			this.toolStripMenuItem編集モード.CheckState = CheckState.Unchecked;
		}
		public void t編集モードにする()
		{
			this.mgr選択モード管理者.t全チップの選択を解除する();
			this.pictureBox譜面パネル.Refresh();
			this.toolStripButton選択モード.CheckState = CheckState.Unchecked;
			this.toolStripButton編集モード.CheckState = CheckState.Checked;
			this.toolStripMenuItem選択モード.CheckState = CheckState.Unchecked;
			this.toolStripMenuItem編集モード.CheckState = CheckState.Checked;
		}
		public void t選択チップの有無に応じて編集用GUIの有効・無効を設定する()
		{
			bool b譜面上に選択チップがある = this.b選択チップがある;
			bool bクリップボードに選択チップがある = ( this.cbクリップボード != null ) && ( this.cbクリップボード.nセル数 > 0 );


			// 編集メニュー

			this.toolStripMenuItemコピー.Enabled = b譜面上に選択チップがある;
			this.toolStripMenuItem切り取り.Enabled = b譜面上に選択チップがある;
			this.toolStripMenuItem貼り付け.Enabled = bクリップボードに選択チップがある;
			this.toolStripMenuItem削除.Enabled = b譜面上に選択チップがある;


			// ツールバー

			this.toolStripButtonコピー.Enabled = b譜面上に選択チップがある;
			this.toolStripButton切り取り.Enabled = b譜面上に選択チップがある;
			this.toolStripButton貼り付け.Enabled = bクリップボードに選択チップがある;
			this.toolStripButton削除.Enabled = b譜面上に選択チップがある;

	
			// 右メニュー

			this.toolStripMenuItem選択チップのコピー.Enabled = b譜面上に選択チップがある;
			this.toolStripMenuItem選択チップの切り取り.Enabled = b譜面上に選択チップがある;
			this.toolStripMenuItem選択チップの貼り付け.Enabled = bクリップボードに選択チップがある;
			this.toolStripMenuItem選択チップの削除.Enabled = b譜面上に選択チップがある;
		}
		public void t選択モードのコンテクストメニューを表示する( int x, int y )
		{
            // メニューの左上隅座標を控えておく。

			this.pt選択モードのコンテクストメニューを開いたときのマウスの位置 = new Point( x, y );
			
			#region [ クリックされた箇所のレーン番号を取得する。]
			//-----------------
			int lane = this.mgr譜面管理者.nX座標dotが位置するレーン番号を返す( pt選択モードのコンテクストメニューを開いたときのマウスの位置.X );
			string strLane = (lane < 0)? "" : this.mgr譜面管理者.listレーン[ lane ].strレーン名;
			//-----------------
			#endregion

			#region [ クリックされた箇所の小節番号を取得する。]
			//-----------------
			int n譜面先頭からの位置grid = this.mgr譜面管理者.nY座標dotが位置するgridを返す・ガイド幅単位( pt選択モードのコンテクストメニューを開いたときのマウスの位置.Y );
			C小節 csクリックされた小節 = this.mgr譜面管理者.p譜面先頭からの位置gridを含む小節を返す( n譜面先頭からの位置grid );
			int nPartNo = csクリックされた小節.n小節番号0to3599;
			string strPartNo = C変換.str小節番号を文字列3桁に変換して返す( nPartNo );
			//-----------------
			#endregion

			#region [ コンテクストメニューの[選択]項目に、レーン名と小節番号の情報をを付与する。 ]
			int indexMenuLaneSelect = this.contextMenuStrip譜面右メニュー.Items.IndexOfKey( "toolStripMenuItemレーン内のすべてのチップの選択" );
			int indexMenuPartSelect = this.contextMenuStrip譜面右メニュー.Items.IndexOfKey( "toolStripMenuItem小節内のすべてのチップの選択" );

			string strItemMenuLaneSelect = this.contextMenuStrip譜面右メニュー.Items[ indexMenuLaneSelect ].Text;
			strItemMenuLaneSelect = System.Text.RegularExpressions.Regex.Replace(
				strItemMenuLaneSelect , @"\[(.*)\]", "[" + strLane + "]" );
			this.contextMenuStrip譜面右メニュー.Items[ indexMenuLaneSelect ].Text = strItemMenuLaneSelect;

			string strItemMenuPartSelect = this.contextMenuStrip譜面右メニュー.Items[ indexMenuPartSelect ].Text;
			strItemMenuPartSelect = System.Text.RegularExpressions.Regex.Replace(
				strItemMenuPartSelect, @"\[(.*)\]", "[" + strPartNo + "]" );
			this.contextMenuStrip譜面右メニュー.Items[ indexMenuPartSelect ].Text = strItemMenuPartSelect;
			#endregion

			// メニューを表示。

			this.contextMenuStrip譜面右メニュー.Show( this.pictureBox譜面パネル, x, y );
		}
		public void t最近使ったファイルをFileメニューへ追加する()
		{
			#region [ [ファイル] メニューから、最近使ったファイルの一覧をクリアする。]
			//-----------------
			for( int i = 0; i < this.toolStripMenuItemファイル.DropDownItems.Count; i++ )
			{
				ToolStripItem item = this.toolStripMenuItemファイル.DropDownItems[ i ];

				// ↓削除したくないサブメニューの一覧。これ以外のサブメニュー項目はすべて削除する。
				if( item != this.toolStripMenuItem新規 &&
					item != this.toolStripMenuItem開く &&
					item != this.toolStripMenuItem上書き保存 &&
					item != this.toolStripMenuItem名前を付けて保存 &&
					item != this.toolStripSeparator1 &&
					item != this.toolStripMenuItem終了 )
				{
					this.toolStripMenuItemファイル.DropDownItems.Remove( item );
					i = -1;	// 要素数が変わったので列挙しなおし
				}
			}
			//-----------------
			#endregion

			#region [ 表示しないオプション設定であるか、履歴が０件ならここで終了する。]
			//-----------------
			if( !this.appアプリ設定.ShowRecentFiles || this.appアプリ設定.RecentUsedFile.Count == 0 )
				return;
			//-----------------
			#endregion

			#region [ アプリ設定が持つ履歴にそって、[ファイル] メニューにサブメニュー項目リストを追加する（ただし最大表示数まで）。 ]
			//-----------------

			// [ファイル] のサブメニューリストに項目が１つでもある場合は、履歴サブメニュー項目の追加の前に「終了」の下にセパレータを入れる。手動で。

			bool bセパレータの追加がまだ = true;


			// すべての「最近使ったファイル」について...

			for( int i = 0; i < this.appアプリ設定.RecentUsedFile.Count; i++ )
			{
				#region [ 最大表示数を越えたらここで終了。 ]
				//-----------------
				if( i >= this.appアプリ設定.RecentFilesNum )
					return;
				//-----------------
				#endregion

				#region [ ファイル名を、サブメニュー項目として [ファイル] メニューに追加する。 ]
				//-----------------
				string path = this.appアプリ設定.RecentUsedFile[ i ];

				if( path.Length == 0 )
					continue;

				#region [ セパレータの追加がまだなら追加する。]
				//-----------------
				if( bセパレータの追加がまだ )
				{
					var separator = new ToolStripSeparator();
					separator.Size = this.toolStripSeparator1.Size;
					this.toolStripMenuItemファイル.DropDownItems.Add( separator );
					bセパレータの追加がまだ = false;
				}
				//-----------------
				#endregion

				#region [ ToolStripMenuItem を手動で作って [ファイル] のサブメニューリストに追加する。]
				//-----------------
				var item2 = new ToolStripMenuItem() {
					Name = "最近使ったファイル" + i,
					Size = this.toolStripMenuItem終了.Size,
					Text = "&" + i + " " + path,
				};
				item2.Click += new EventHandler( this.toolStripMenuItem最近使ったファイル_Click );
				this.toolStripMenuItemファイル.DropDownItems.Add( item2 );
				//-----------------
				#endregion

				#region [ 追加したファイルが既に存在していないなら項目を無効化（グレー表示）する。]
				//-----------------
				if( !File.Exists( path ) )
					item2.Enabled = false;
				//-----------------
				#endregion

				//-----------------
				#endregion
			}
			//-----------------
			#endregion
		}
		public void tWAV・BMP・AVIリストのカーソルを全部同じ行に合わせる( int nIndex番号0to1294 )
		{
			if( nIndex番号0to1294 >= 0 && nIndex番号0to1294 <= 1294 )
			{
				this.mgrWAVリスト管理者.tItemを選択する( nIndex番号0to1294 );
				this.mgrBMPリスト管理者.tItemを選択する( nIndex番号0to1294 );
				this.mgrAVIリスト管理者.tItemを選択する( nIndex番号0to1294 );

				this.n現在選択中のWAV・BMP・AVIリストの行番号0to1294 = nIndex番号0to1294;
			}
		}
		public string strファイルの存在するディレクトリを絶対パスで返す( string strファイル )
		{
			string strファイルの絶対パス = strファイル;

			try
			{
				// ファイルが絶対パスかどうかを判定する。（new Uri() は相対パスを指定されると例外が発生するので、それを利用する。）

				new Uri( strファイル );
			}
			catch
			{
				// 例外が発生したので相対パスとみなし、絶対パスに直す。

				strファイルの絶対パス = this.str作業フォルダ名 + strファイル;
			}

			return strファイルの絶対パス;
		}
		public Point pt現在のマウス位置を譜面の可視領域相対の座標dotで返す()
		{
			Point p = new Point( Cursor.Position.X, Cursor.Position.Y );
			return this.splitContainerタブと譜面を分割.Panel2.PointToClient( p );
		}
		public Size sz譜面の可視領域の大きさdotを返す()
		{
			return new Size( this.splitContainerタブと譜面を分割.Panel2.Width, this.pictureBox譜面パネル.Height );
		}
		public void tUndoRedo用GUIの有効・無効を設定する()
		{
			this.toolStripMenuItemアンドゥ.Enabled = this.mgrUndoRedo管理者.nUndo可能な回数 > 0;
			this.toolStripMenuItemリドゥ.Enabled = this.mgrUndoRedo管理者.nRedo可能な回数 > 0;
			this.toolStripButtonアンドゥ.Enabled = this.mgrUndoRedo管理者.nUndo可能な回数 > 0;
			this.toolStripButtonリドゥ.Enabled = this.mgrUndoRedo管理者.nRedo可能な回数 > 0;
		}
		public void tタブを選択する( Eタブ種別 eタブ種別 )
		{
			this.tabControl情報パネル.SelectedIndex = (int) eタブ種別;
		}


		// その他

		#region [ private ]
		//-----------------
		private bool _b未保存 = true;
		private Point pt選択モードのコンテクストメニューを開いたときのマウスの位置;
		private int n現在のガイド間隔4to64or0 = 16;		// 初期は16分間隔
		private bool b選択チップがある
		{
			get
			{
				foreach( KeyValuePair<int, C小節> pair in this.mgr譜面管理者.dic小節 )
				{
					foreach( Cチップ cチップ in pair.Value.listチップ )
					{
						if( cチップ.b確定選択中 )
							return true;
					}
				}
				return false;
			}
		}

		private void tDTXV演奏関連のボタンとメニューのEnabledの設定()
		{
			if( File.Exists( this.strDTXCのあるフォルダ名 + this.appアプリ設定.ViewerInfo.Path ) )
			{
				// DTXViewer が存在するなら Enable

				this.toolStripButton先頭から再生.Enabled = true;
				this.toolStripButton現在位置から再生.Enabled = true;
				this.toolStripButton現在位置からBGMのみ再生.Enabled = true;
				this.toolStripButton再生停止.Enabled = true;
				this.toolStripMenuItem先頭から再生.Enabled = true;
				this.toolStripMenuItem現在位置から再生.Enabled = true;
				this.toolStripMenuItem現在位置からBGMのみ再生.Enabled = true;
				this.toolStripMenuItem再生停止.Enabled = true;
			}
			else
			{
				// DTXViewer が存在しないなら Disable

				this.toolStripButton先頭から再生.Enabled = false;
				this.toolStripButton現在位置から再生.Enabled = false;
				this.toolStripButton現在位置からBGMのみ再生.Enabled = false;
				this.toolStripButton再生停止.Enabled = false;
				this.toolStripMenuItem先頭から再生.Enabled = false;
				this.toolStripMenuItem現在位置から再生.Enabled = false;
				this.toolStripMenuItem現在位置からBGMのみ再生.Enabled = false;
				this.toolStripMenuItem再生停止.Enabled = false;
			}
		}
		private string tファイル選択ダイアログでファイルを選択し相対パスにして返す( string strタイトル, string strフィルタ, string str初期フォルダ )
		{
			string str相対ファイル名 = "";

			this.dlgチップパレット.t一時的に隠蔽する();

			var dialog = new OpenFileDialog() {
				Title = strタイトル,
				Filter = strフィルタ,
				FilterIndex = 1,
				InitialDirectory = str初期フォルダ,
			};
			if( dialog.ShowDialog() == DialogResult.OK )
			{
				str相対ファイル名 = Cファイル選択・パス変換.str基点からの相対パスに変換して返す( dialog.FileName, this.str作業フォルダ名 );
				str相対ファイル名.Replace( '/', '\\' );
			}
			else
				str相対ファイル名 = "";

			this.dlgチップパレット.t一時的な隠蔽を解除する();

			return str相対ファイル名;
		}
		private DialogResult t未保存なら保存する()
		{
			var result = DialogResult.OK;

			if( this.b未保存 )
			{
				// ダイアログで保存可否を確認。

				this.dlgチップパレット.t一時的に隠蔽する();
				result = MessageBox.Show( Resources.str編集中のデータを保存しますかMSG, Resources.str確認ダイアログのタイトル, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1 );
				this.dlgチップパレット.t一時的な隠蔽を解除する();
				

				// YES なら上書き保存。

				if( result == DialogResult.Yes )
					this.tシナリオ・上書き保存();


				// 画面を再描画。

				this.Refresh();
			}
			return result;
		}
		private void t次のプロパティ変更処理がUndoRedoリストに載らないようにする()
		{
			CUndoRedo管理.bUndoRedoした直後 = true;
		}
		private void t次のプロパティ変更処理がUndoRedoリストに載るようにする()
		{
			CUndoRedo管理.bUndoRedoした直後 = false;
		}

		/// <summary>
		/// <para>n分 … 4分間隔なら 4、8分間隔なら 8 など、フリー間隔なら 0 を指定する。</para>
		/// </summary>
		private void tガイド間隔を変更する( int n分 )
		{
			// 新しいガイド間隔を設定。

			#region [ 新しいガイド間隔を設定。 ]
			//-----------------
			
			this.n現在のガイド間隔4to64or0 = n分;

			this.mgr譜面管理者.n現在のガイド幅grid =
				( n分 == 0 ) ? 1 : ( C小節.n基準の高さgrid / n分 );
			
			//-----------------
			#endregion


			// ガイド間隔メニュー GUI を更新。

			#region [ 一度すべてのガイド間隔メニューのチェックをはずし、制定された分数のメニューのみチェックする。 ]
			//-----------------
			this.toolStripMenuItemガイド間隔4分.CheckState = CheckState.Unchecked;
			this.toolStripMenuItemガイド間隔8分.CheckState = CheckState.Unchecked;
			this.toolStripMenuItemガイド間隔12分.CheckState = CheckState.Unchecked;
			this.toolStripMenuItemガイド間隔16分.CheckState = CheckState.Unchecked;
			this.toolStripMenuItemガイド間隔24分.CheckState = CheckState.Unchecked;
			this.toolStripMenuItemガイド間隔32分.CheckState = CheckState.Unchecked;
			this.toolStripMenuItemガイド間隔48分.CheckState = CheckState.Unchecked;
			this.toolStripMenuItemガイド間隔64分.CheckState = CheckState.Unchecked;
			this.toolStripMenuItemガイド間隔フリー.CheckState = CheckState.Unchecked;
			
			switch( n分 )
			{
				case 8:
					this.toolStripMenuItemガイド間隔8分.CheckState = CheckState.Checked;
					this.toolStripComboBoxガイド間隔.SelectedIndex = 1;
					break;

				case 12:
					this.toolStripMenuItemガイド間隔12分.CheckState = CheckState.Checked;
					this.toolStripComboBoxガイド間隔.SelectedIndex = 2;
					break;

				case 0:
					this.toolStripMenuItemガイド間隔フリー.CheckState = CheckState.Checked;
					this.toolStripComboBoxガイド間隔.SelectedIndex = 8;
					break;

				case 4:
					this.toolStripMenuItemガイド間隔4分.CheckState = CheckState.Checked;
					this.toolStripComboBoxガイド間隔.SelectedIndex = 0;
					break;

				case 0x10:
					this.toolStripMenuItemガイド間隔16分.CheckState = CheckState.Checked;
					this.toolStripComboBoxガイド間隔.SelectedIndex = 3;
					break;

				case 0x18:
					this.toolStripMenuItemガイド間隔24分.CheckState = CheckState.Checked;
					this.toolStripComboBoxガイド間隔.SelectedIndex = 4;
					break;

				case 0x20:
					this.toolStripMenuItemガイド間隔32分.CheckState = CheckState.Checked;
					this.toolStripComboBoxガイド間隔.SelectedIndex = 5;
					break;

				case 0x30:
					this.toolStripMenuItemガイド間隔48分.CheckState = CheckState.Checked;
					this.toolStripComboBoxガイド間隔.SelectedIndex = 6;
					break;

				case 0x40:
					this.toolStripMenuItemガイド間隔64分.CheckState = CheckState.Checked;
					this.toolStripComboBoxガイド間隔.SelectedIndex = 7;
					break;
			}
			//-----------------
			#endregion


			// 画面を再描画。

			#region [ 画面を再描画する。]
			//-----------------
			this.pictureBox譜面パネル.Invalidate();
			//-----------------
			#endregion
		}


		// GUI イベント

		#region [ GUIイベント：メインフォーム ]
		//-----------------
		private void Cメインフォーム_DragDrop( object sender, DragEventArgs e )
		{
			string[] data = (string[]) e.Data.GetData( DataFormats.FileDrop );
			if( data.Length >= 1 )
			{
				this.tシナリオ・DragDropされたファイルを開く( data );
			}
		}
		private void Cメインフォーム_DragEnter( object sender, DragEventArgs e )
		{
			if( e.Data.GetDataPresent( DataFormats.FileDrop ) )
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}
		private void Cメインフォーム_FormClosing( object sender, FormClosingEventArgs e )
		{
			if( this.t未保存なら保存する() == DialogResult.Cancel )
			{
				e.Cancel = true;
			}
			else
			{
				this.tアプリ終了時に行う終了処理();
			}
		}
		private void Cメインフォーム_Load( object sender, EventArgs e )
		{
			this.tアプリ起動時に一度だけ行う初期化処理();
			// this.t譜面を初期化する();					// 2011.8.29 yyagi; removed this.t譜面を初期化する() because it has already done in this.tアプリ起動時に一度だけ行う初期化処理().
		}
		//-----------------
		#endregion
		#region [ GUIイベント：pictureBox譜面パネル、Panel2、スクロールバー関連 ]
		//-----------------
		private void pictureBox譜面パネル_MouseClick( object sender, MouseEventArgs e )
		{
			// フォーカスを得る。

			this.pictureBox譜面パネル.Focus();


			// 選択・編集のいずれかの管理者へ処理を引き継ぐ。

			if( this.b選択モードである )
			{
				this.mgr選択モード管理者.MouseClick( e );
			}
			else
			{
				this.mgr編集モード管理者.MouseClick( e );
			}
		}
		private void pictureBox譜面パネル_MouseDown( object sender, MouseEventArgs e )
		{
			if( this.b選択モードである )
				this.mgr選択モード管理者.MouseDown( e );
		}
		private void pictureBox譜面パネル_MouseEnter( object sender, EventArgs e )
		{
			#region [ オートフォーカスが有効の場合、譜面にマウスが入ったら譜面がフォーカスを得る。 ]
			//-----------------
			if( this.appアプリ設定.AutoFocus )
				this.pictureBox譜面パネル.Focus();
			//-----------------
			#endregion
		}
		private void pictureBox譜面パネル_MouseLeave( object sender, EventArgs e )
		{
			if( this.b編集モードである )
				this.mgr編集モード管理者.MouseLeave( e );
		}
		private void pictureBox譜面パネル_MouseMove( object sender, MouseEventArgs e )
		{
			// 選択・編集のいずれかの管理者へ処理を引き継ぐ。

			if( this.b選択モードである )
			{
				this.mgr選択モード管理者.MouseMove( e );
			}
			else
			{
				this.mgr編集モード管理者.MouseMove( e );
			}
		}
		private void pictureBox譜面パネル_Paint( object sender, PaintEventArgs e )
		{
			if( this.mgr譜面管理者 == null )
				return;		// まだ初期化が終わってないうちに Paint が呼び出される場合がある。

			#region [ 小節数が変わってたら、スクロールバーの値域を調整する。]
			//-----------------
			int n全譜面の高さgrid = this.mgr譜面管理者.n全小節の高さgridの合計を返す();

			if( this.vScrollBar譜面用垂直スクロールバー.Maximum != n全譜面の高さgrid - 1 )	// 小節数が変わっている
			{
				// 譜面の高さ(grid)がどれだけ変わったか？

				int n増加分grid = ( n全譜面の高さgrid - 1 ) - this.vScrollBar譜面用垂直スクロールバー.Maximum;


				// スクロールバーを調整。

				#region [ スクロールバーの状態を新しい譜面の高さに合わせる。]
				//-----------------
				this.vScrollBar譜面用垂直スクロールバー.Maximum = n全譜面の高さgrid - 1;

				if( ( this.vScrollBar譜面用垂直スクロールバー.Value + n増加分grid ) < 0 )
				{
					this.vScrollBar譜面用垂直スクロールバー.Value = 0;
				}
				else
				{
					this.vScrollBar譜面用垂直スクロールバー.Value += n増加分grid;
				}
				//-----------------
				#endregion


				// 譜面表示下辺の位置を更新。

				#region [ 譜面表示下辺の位置を更新する。 ]
				//-----------------
				this.mgr譜面管理者.n現在の譜面表示下辺の譜面先頭からの位置grid =
					( ( this.vScrollBar譜面用垂直スクロールバー.Maximum - this.vScrollBar譜面用垂直スクロールバー.LargeChange ) + 1 ) - this.vScrollBar譜面用垂直スクロールバー.Value;
				//-----------------
				#endregion
			}
			//-----------------
			#endregion

			#region [ 譜面を描画する。]
			//-----------------
			int nPicBoxの幅 = this.pictureBox譜面パネル.ClientSize.Width;
			int nPanel2の幅 = this.splitContainerタブと譜面を分割.Panel2.Width;

			var rc可視領域 = new Rectangle() {
				X = -this.pictureBox譜面パネル.Location.X,
				Y = 0,
				Width = ( nPanel2の幅 > nPicBoxの幅 ) ? nPicBoxの幅 : nPanel2の幅,
				Height = this.pictureBox譜面パネル.ClientSize.Height,
			};

			this.mgr譜面管理者.t譜面を描画する( e.Graphics, this.pictureBox譜面パネル.ClientSize, rc可視領域 );
			//-----------------
			#endregion

			#region [ 現在のモード管理者の Paint() を呼び出す。]
			//-----------------
			if( this.b選択モードである )
			{
				if( this.mgr選択モード管理者 != null )
					this.mgr選択モード管理者.Paint( e );
			}
			else
			{
				if( this.mgr編集モード管理者 != null )
					this.mgr編集モード管理者.Paint( e );
			}
			//-----------------
			#endregion
		}
		private void pictureBox譜面パネル_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
		{
			if( e.KeyCode == Keys.Prior )
			{
				#region [ PageUp → 移動量に対応する grid だけ垂直つまみを移動させる。あとはこの移動で生じる ChangedValue イベントで処理。]
				//-----------------
				int n移動すべき数grid = -C小節.n基準の高さgrid;
				int n新しい位置 = this.vScrollBar譜面用垂直スクロールバー.Value + n移動すべき数grid;
				int n最小値 = this.vScrollBar譜面用垂直スクロールバー.Minimum;
				int n最大値 = ( this.vScrollBar譜面用垂直スクロールバー.Maximum + 1 ) - this.vScrollBar譜面用垂直スクロールバー.LargeChange;

				if( n新しい位置 < n最小値 )
				{
					n新しい位置 = n最小値;
				}
				else if( n新しい位置 > n最大値 )
				{
					n新しい位置 = n最大値;
				}

				this.vScrollBar譜面用垂直スクロールバー.Value = n新しい位置;
				//-----------------
				#endregion
			}
			else if( e.KeyCode == Keys.Next )
			{
				#region [ PageDown → 移動量に対応する grid だけ垂直つまみを移動させる。あとはこの移動で生じる ChangedValue イベントで処理。]
				//-----------------
				int n移動すべき数grid = C小節.n基準の高さgrid;
				int n新しい位置 = this.vScrollBar譜面用垂直スクロールバー.Value + n移動すべき数grid;
				int n最小値 = this.vScrollBar譜面用垂直スクロールバー.Minimum;
				int n最大値 = ( this.vScrollBar譜面用垂直スクロールバー.Maximum + 1 ) - this.vScrollBar譜面用垂直スクロールバー.LargeChange;

				if( n新しい位置 < n最小値 )
				{
					n新しい位置 = n最小値;
				}
				else if( n新しい位置 > n最大値 )
				{
					n新しい位置 = n最大値;
				}

				this.vScrollBar譜面用垂直スクロールバー.Value = n新しい位置;
				//-----------------
				#endregion
			}
		}

		private void splitContainerタブと譜面を分割_MouseWheel( object sender, MouseEventArgs e )
		{
			if ( ( Control.ModifierKeys & Keys.Shift ) == Keys.Shift )
			{
				#region [ Shiftを押しながらホイール操作すると、横スクロール。]
				if ( e.Delta == 0 )
					return;		// 移動量なし

				// e.Delta は、スクロールバーを下へ動かしたいときに負、上へ動かしたいときに正となる。

				int n移動すべき行数 = ( -e.Delta * SystemInformation.MouseWheelScrollLines ) / 120;

				// １行＝１レーン とする。(が、実際には適当に設定しただけ。1レーンには設定していない)

				int n移動すべき数grid = n移動すべき行数 * 16;


				// スクロールバーのつまみを移動。

				int n新しい位置 = this.hScrollBar譜面用水平スクロールバー.Value + n移動すべき数grid;
				int n最小値 = this.hScrollBar譜面用水平スクロールバー.Minimum;
				int n最大値 = ( this.hScrollBar譜面用水平スクロールバー.Maximum + 1 ) - this.hScrollBar譜面用水平スクロールバー.LargeChange;

				if ( n新しい位置 < n最小値 )
				{
					n新しい位置 = n最小値;
				}
				else if ( n新しい位置 > n最大値 )
				{
					n新しい位置 = n最大値;
				}

				this.hScrollBar譜面用水平スクロールバー.Value = n新しい位置;
				//-----------------
				#endregion
			}
			else
			{
				#region [ 移動量に対応する grid だけ垂直つまみを移動させる。あとはこの移動で生じる ChangedValue イベントで処理する。]
				//-----------------
				if ( e.Delta == 0 )
					return;		// 移動量なし


				// e.Delta は、スクロールバーを下へ動かしたいときに負、上へ動かしたいときに正となる。

				int n移動すべき行数 = ( -e.Delta * SystemInformation.MouseWheelScrollLines ) / 120;


				// １行＝１拍（64/4=16グリッド）とする。

				int n移動すべき数grid = n移動すべき行数 * 16;


				// スクロールバーのつまみを移動。

				int n新しい位置 = this.vScrollBar譜面用垂直スクロールバー.Value + n移動すべき数grid;
				int n最小値 = this.vScrollBar譜面用垂直スクロールバー.Minimum;
				int n最大値 = ( this.vScrollBar譜面用垂直スクロールバー.Maximum + 1 ) - this.vScrollBar譜面用垂直スクロールバー.LargeChange;

				if ( n新しい位置 < n最小値 )
				{
					n新しい位置 = n最小値;
				}
				else if ( n新しい位置 > n最大値 )
				{
					n新しい位置 = n最大値;
				}

				this.vScrollBar譜面用垂直スクロールバー.Value = n新しい位置;
				//-----------------
				#endregion
			}
		}
		private void splitContainerタブと譜面を分割_Panel2_SizeChanged( object sender, EventArgs e )
		{
			if( this.mgr譜面管理者 != null )	// 初期化前に呼び出されることがある。
			{
				this.mgr譜面管理者.t水平スクロールバーと譜面パネル左右位置の調整();
				this.mgr譜面管理者.t垂直スクロールバーと譜面可視領域の上下位置の調整();
			}
		}

		private void hScrollBar譜面用水平スクロールバー_ValueChanged( object sender, EventArgs e )
		{
			if( this.mgr譜面管理者 != null )
				this.mgr譜面管理者.t水平スクロールバーと譜面パネル左右位置の調整();
		}
		private void vScrollBar譜面用垂直スクロールバー_ValueChanged( object sender, EventArgs e )
		{
			if( mgr譜面管理者 != null )
				this.mgr譜面管理者.t垂直スクロールバーと譜面可視領域の上下位置の調整();
		}
		//-----------------
		#endregion
		#region [ GUIイベント：譜面右メニュー関連 ]
		//-----------------
		private void toolStripMenuItem選択チップの切り取り_Click( object sender, EventArgs e )
		{
			this.tシナリオ・切り取り();
		}
		private void toolStripMenuItem選択チップのコピー_Click( object sender, EventArgs e )
		{
			this.tシナリオ・コピー();
		}
		private void toolStripMenuItem選択チップの貼り付け_Click( object sender, EventArgs e )
		{
			// クリックされた座標を取得。

			Point ptMenu = new Point( this.contextMenuStrip譜面右メニュー.Left, this.contextMenuStrip譜面右メニュー.Top );
			Point ptMenuClient = this.contextMenuStrip譜面右メニュー.SourceControl.PointToClient( ptMenu );


			// Y座標から位置gridを得て、そこへ貼り付ける。

			this.tシナリオ・貼り付け( this.mgr譜面管理者.nY座標dotが位置するgridを返す・ガイド幅単位( ptMenuClient.Y ) );
		}
		private void toolStripMenuItem選択チップの削除_Click( object sender, EventArgs e )
		{
			this.tシナリオ・削除();
		}

		private void toolStripMenuItemすべてのチップの選択_Click( object sender, EventArgs e )
		{
			// 編集モードなら強制的に選択モードにする。

			if( this.b編集モードである )
				this.t選択モードにする();


			// 全チップを選択。

			this.mgr選択モード管理者.t全チップを選択する();
		}
		private void toolStripMenuItemレーン内のすべてのチップの選択_Click( object sender, EventArgs e )
		{
			// 編集モードなら強制的に選択モードにする。

			if ( this.b編集モードである )
				this.t選択モードにする();

			// メニューが開かれたときのマウスの座標を取得。
			// ※メニューは必ずマウス位置を左上にして表示されるとは限らないため、
			// 　メニューの表示位置からは取得しないこと。

			Point ptマウスの位置 = this.pt選択モードのコンテクストメニューを開いたときのマウスの位置;


			// マウス位置に小節を挿入。

			#region [ クリックされた箇所のレーン番号を取得する。]
			//-----------------
			int lane = this.mgr譜面管理者.nX座標dotが位置するレーン番号を返す( ptマウスの位置.X );
			if ( lane < 0 )
				return;		// クリックされた箇所にレーンがない

			//-----------------
			#endregion

			this.mgr選択モード管理者.tレーン上の全チップを選択する( lane );

		}

		private void toolStripMenuItem小節内のすべてのチップの選択_Click( object sender, EventArgs e )
		{
			// 編集モードなら強制的に選択モードにする。

			if ( this.b編集モードである )
				this.t選択モードにする();

			// メニューが開かれたときのマウスの座標を取得。
			// ※メニューは必ずマウス位置を左上にして表示されるとは限らないため、
			// 　メニューの表示位置からは取得しないこと。

			Point ptマウスの位置 = this.pt選択モードのコンテクストメニューを開いたときのマウスの位置;

			#region [ クリックされた箇所の小節を取得する。]
			//-----------------
			if ( this.mgr譜面管理者.nX座標dotが位置するレーン番号を返す( ptマウスの位置.X ) < 0 )
				return;		// クリックされた箇所にレーンがない

			int n譜面先頭からの位置grid = this.mgr譜面管理者.nY座標dotが位置するgridを返す・ガイド幅単位( ptマウスの位置.Y );
			C小節 csクリックされた小節 = this.mgr譜面管理者.p譜面先頭からの位置gridを含む小節を返す( n譜面先頭からの位置grid );
			//-----------------
			#endregion

			this.mgr選択モード管理者.t小節上の全チップを選択する( csクリックされた小節.n小節番号0to3599 );
		}

		private void toolStripMenuItem小節長変更_Click( object sender, EventArgs e )
		{
			// メニューが開かれたときのマウスの座標を取得。
			// ※メニューは必ずマウス位置を左上にして表示されるとは限らないため、
			// 　メニューの表示位置からは取得しないこと。

			Point ptマウス位置 = this.pt選択モードのコンテクストメニューを開いたときのマウスの位置;


			// 小節の小節長を変更。

			#region [ クリックされた小節を取得する。]
			//-----------------
			if( this.mgr譜面管理者.nX座標dotが位置するレーン番号を返す( ptマウス位置.X ) < 0 )
				return;		// クリックされた箇所にレーンがないなら無視。

			int n譜面先頭からの位置grid = this.mgr譜面管理者.nY座標dotが位置するgridを返す・ガイド幅単位( ptマウス位置.Y );
			C小節 csクリックされた小節 =  this.mgr譜面管理者.p譜面先頭からの位置gridを含む小節を返す( n譜面先頭からの位置grid );
			//-----------------
			#endregion

			#region [ 取得した小節の小節長を変更する。]
			//-----------------
			if( csクリックされた小節 != null )
				this.tシナリオ・小節長を変更する( csクリックされた小節 );
			//-----------------
			#endregion
		}
		private void toolStripMenuItem小節の挿入_Click( object sender, EventArgs e )
		{
			// メニューが開かれたときのマウスの座標を取得。
			// ※メニューは必ずマウス位置を左上にして表示されるとは限らないため、
			// 　メニューの表示位置からは取得しないこと。

			Point ptマウスの位置 = this.pt選択モードのコンテクストメニューを開いたときのマウスの位置;


			// マウス位置に小節を挿入。

			#region [ クリックされた箇所の小節を取得する。]
			//-----------------
			if( this.mgr譜面管理者.nX座標dotが位置するレーン番号を返す( ptマウスの位置.X ) < 0 )
				return;		// クリックされた箇所にレーンがない

			int n譜面先頭からの位置grid = this.mgr譜面管理者.nY座標dotが位置するgridを返す・ガイド幅単位( ptマウスの位置.Y );
			C小節 csクリックされた小節 = this.mgr譜面管理者.p譜面先頭からの位置gridを含む小節を返す( n譜面先頭からの位置grid );
			//-----------------
			#endregion

			#region [ 該当小節の下へ新しい小節を挿入する。]
			//-----------------
			if( csクリックされた小節 != null )
				this.tシナリオ・小節を挿入する( csクリックされた小節.n小節番号0to3599 );
			//-----------------
			#endregion
		}
		private void toolStripMenuItem小節の削除_Click( object sender, EventArgs e )
		{
			// メニューが開かれたときのマウスの座標を取得。
			// ※メニューは必ずマウス位置を左上にして表示されるとは限らないため、
			// 　メニューの表示位置からは取得しないこと。

			Point ptマウス位置 = this.pt選択モードのコンテクストメニューを開いたときのマウスの位置;


			// マウス位置にある小節を削除。

			#region [ クリックされた箇所の小節を取得する。 ]
			//-----------------
			if( this.mgr譜面管理者.nX座標dotが位置するレーン番号を返す( ptマウス位置.X ) < 0 )
				return;		// クリックされた箇所にレーンがないなら無視。

			int n譜面先頭からの位置grid = this.mgr譜面管理者.nY座標dotが位置するgridを返す・ガイド幅単位( ptマウス位置.Y );
			C小節 cs削除する小節 = this.mgr譜面管理者.p譜面先頭からの位置gridを含む小節を返す( n譜面先頭からの位置grid );
			//-----------------
			#endregion

			#region [ 該当小節を削除する。]
			//-----------------
			if( cs削除する小節 != null )
				this.tシナリオ・小節を削除する( cs削除する小節.n小節番号0to3599 );
			//-----------------
			#endregion
		}
		//-----------------
		#endregion
		
		#region [ GUIイベント：基本情報関連 ]
		//-----------------
		private string textBox曲名_以前の値 = "";
		private void textBox曲名_TextChanged( object sender, EventArgs e )
		{
			// Undo/Redo リストを修正する。

			#region [ Undo/Redo リストを修正。]
			//-----------------
			if( !CUndoRedo管理.bUndoRedoした直後 )
			{
				CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

				if( ( oセル仮想 != null ) && oセル仮想.b所有権がある( this.textBox曲名 ) )
				{
					// 既存のセルの値を更新。

					( (CUndoRedoセル<string>) oセル仮想 ).変更後の値 = this.textBox曲名.Text;
				}
				else
				{
					// 新しいセルを追加。

					this.mgrUndoRedo管理者.tノードを追加する( 
						new CUndoRedoセル<string>(
							this.textBox曲名,
							new DGUndoを実行する<string>( this.textBox曲名_Undo ),
							new DGRedoを実行する<string>( this.textBox曲名_Redo ),
							this.textBox曲名_以前の値, this.textBox曲名.Text ) );


					// Undo ボタンを有効にする。

					this.tUndoRedo用GUIの有効・無効を設定する();
				}
			}
			//-----------------
			#endregion


			// Undo 用に値を保管しておく。

			this.textBox曲名_以前の値 = this.textBox曲名.Text;


			// 完了。

			CUndoRedo管理.bUndoRedoした直後 = false;
			this.b未保存 = true;
		}
		private void textBox曲名_Leave( object sender, EventArgs e )
		{
			CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

			if( oセル仮想 != null )
				oセル仮想.t所有権の放棄( this.textBox曲名 );
		}
		private void textBox曲名_Undo( string str変更前, string str変更後 )
		{
			// 変更前の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBox曲名.Text = str変更前;

			this.textBox曲名.Focus();
		}
		private void textBox曲名_Redo( string str変更前, string str変更後 )
		{
			// 変更後の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBox曲名.Text = str変更後;

			this.textBox曲名.Focus();
		}

		private string textBox製作者_以前の値 = "";
		private void textBox製作者_TextChanged( object sender, EventArgs e )
		{
			// Undo/Redo リストを修正する。

			#region [ Undo/Redo リストを修正。]
			//-----------------
			if( !CUndoRedo管理.bUndoRedoした直後 )
			{
				CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

				if( ( oセル仮想 != null ) && oセル仮想.b所有権がある( this.textBox製作者 ) )
				{
					// 既存のセルの値を更新。

					( (CUndoRedoセル<string>) oセル仮想 ).変更後の値 = this.textBox製作者.Text;
				}
				else
				{
					// 新しいセルを追加。

					this.mgrUndoRedo管理者.tノードを追加する(
						new CUndoRedoセル<string>(
							this.textBox製作者, 
							new DGUndoを実行する<string>( this.textBox製作者_Undo ),
							new DGRedoを実行する<string>( this.textBox製作者_Redo ),
							this.textBox製作者_以前の値, this.textBox製作者.Text ) );


					// Undo ボタンを有効にする。

					this.tUndoRedo用GUIの有効・無効を設定する();
				}
			}
			//-----------------
			#endregion


			// Undo 用に値を保管しておく。

			this.textBox製作者_以前の値 = this.textBox製作者.Text;


			// 完了。

			CUndoRedo管理.bUndoRedoした直後 = false;
			this.b未保存 = true;
		}
		private void textBox製作者_Leave( object sender, EventArgs e )
		{
			CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();
		
			if( oセル仮想 != null )
				oセル仮想.t所有権の放棄( this.textBox製作者 );
		}
		private void textBox製作者_Undo( string str変更前, string str変更後 )
		{
			// 変更前の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBox製作者.Text = str変更前;

			this.textBox製作者.Focus();
		}
		private void textBox製作者_Redo( string str変更前, string str変更後 )
		{
			// 変更後の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBox製作者.Text = str変更後;

			this.textBox製作者.Focus();
		}

		private string textBoxコメント_以前の値 = "";
		private void textBoxコメント_TextChanged( object sender, EventArgs e )
		{
			// Undo/Redo リストを修正する。

			#region [ Undo/Redo リストを修正。]
			//-----------------
			if( !CUndoRedo管理.bUndoRedoした直後 )
			{
				CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

				if( ( oセル仮想 != null ) && oセル仮想.b所有権がある( this.textBoxコメント ) )
				{
					// 既存のセルの値を更新。

					( (CUndoRedoセル<string>) oセル仮想 ).変更後の値 = this.textBoxコメント.Text;
				}
				else
				{
					// 新しいセルを追加。

					this.mgrUndoRedo管理者.tノードを追加する(
						new CUndoRedoセル<string>( 
							this.textBoxコメント, 
							new DGUndoを実行する<string>( this.textBoxコメント_Undo ), 
							new DGRedoを実行する<string>( this.textBoxコメント_Redo ),
							this.textBoxコメント_以前の値, this.textBoxコメント.Text ) );

					// Undo ボタンを有効にする。

					this.tUndoRedo用GUIの有効・無効を設定する();
				}
			}
			//-----------------
			#endregion


			// Undo 用に値を保管しておく。

			this.textBoxコメント_以前の値 = this.textBoxコメント.Text;


			// 完了。

			CUndoRedo管理.bUndoRedoした直後 = false;
			this.b未保存 = true;
		}
		private void textBoxコメント_Leave( object sender, EventArgs e )
		{
			CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();
		
			if( oセル仮想 != null )
				oセル仮想.t所有権の放棄( this.textBoxコメント );
		}
		private void textBoxコメント_Undo( string str変更前, string str変更後 )
		{
			// 変更前の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxコメント.Text = str変更前;

			this.textBoxコメント.Focus();
		}
		private void textBoxコメント_Redo( string str変更前, string str変更後 )
		{
			// 変更後の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxコメント.Text = str変更後;

			this.textBoxコメント.Focus();
		}

		private decimal numericUpDownBPM_以前の値 = 120.0M;
		private void numericUpDownBPM_ValueChanged( object sender, EventArgs e )
		{
			// Undo/Redo リストを修正する。

			#region [ Undo/Redo リストの修正。]
			//-----------------
			if( !CUndoRedo管理.bUndoRedoした直後 )
			{
				CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

				if( ( oセル仮想 != null ) && oセル仮想.b所有権がある( this.numericUpDownBPM ) )
				{
					// 既存のセルの値を更新。

					( (CUndoRedoセル<decimal>) oセル仮想 ).変更後の値 = this.numericUpDownBPM.Value;
				}
				else
				{
					// 新しいセルを追加。

					this.mgrUndoRedo管理者.tノードを追加する(
						new CUndoRedoセル<decimal>(
							this.numericUpDownBPM,
							new DGUndoを実行する<decimal>( this.numericUpDownBPM_Undo ), 
							new DGRedoを実行する<decimal>( this.numericUpDownBPM_Redo ),
							this.numericUpDownBPM_以前の値, this.numericUpDownBPM.Value ) );


					// Undoボタンを有効にする。

					this.tUndoRedo用GUIの有効・無効を設定する();
				}
			}
			//-----------------
			#endregion

			
			// Undo 用に値を保管しておく。

			this.numericUpDownBPM_以前の値 = this.numericUpDownBPM.Value;


			// 完了。

			CUndoRedo管理.bUndoRedoした直後 = false;
			this.b未保存 = true;
		}
		private void numericUpDownBPM_Leave( object sender, EventArgs e )
		{
			CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();
		
			if( oセル仮想 != null )
				oセル仮想.t所有権の放棄( this.numericUpDownBPM );
		}
		private void numericUpDownBPM_Undo( decimal dec変更前, decimal dec変更後 )
		{
			// 変更前の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.numericUpDownBPM.Value = dec変更前;

			this.numericUpDownBPM.Focus();
		}
		private void numericUpDownBPM_Redo( decimal dec変更前, decimal dec変更後 )
		{
			// 変更後の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.numericUpDownBPM.Value = dec変更後;

			this.numericUpDownBPM.Focus();
		}

		private int nDLEVEL_以前の値 = 50;
		private void textBoxDLEVEL_TextChanged( object sender, EventArgs e )
		{
			// 何もしない。→ 数字以外が入力されていることもあるため、Leaveまで待つ。
		}
		private void textBoxDLEVEL_Leave( object sender, EventArgs e )
		{
			if( this.textBoxDLEVEL.Text.Length > 0 )
			{
				// 数値チェック。

				int n値;
				if( !int.TryParse( this.textBoxDLEVEL.Text, out n値 ) )
				{
					n値 = 0;
				}
				else if( n値 < 0 )
				{
					n値 = 0;
				}
				else if( n値 > 100 )
				{
					n値 = 100;
				}


				// 値を水平スクロールバーにも反映。

				if( this.hScrollBarDLEVEL.Value != n値 )
				{
					this.t次のプロパティ変更処理がUndoRedoリストに載るようにする();
					this.hScrollBarDLEVEL.Value = n値;	// ここで hScrollBarDLEVEL_ValueChanged が発動 → UndoRedo処理はそちらで。
				}
			}
		}
		private void hScrollBarDLEVEL_ValueChanged( object sender, EventArgs e )
		{
			// 値をテキストボックスにも反映。

			this.textBoxDLEVEL.Text = this.hScrollBarDLEVEL.Value.ToString();


			// Undo/Redo リストを修正。

			#region [ Undo/Redo リストを修正する。]
			//-----------------
			if( !CUndoRedo管理.bUndoRedoした直後 )
			{
				CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

				if( oセル仮想 != null && oセル仮想.b所有権がある( this.hScrollBarDLEVEL ) )
				{
					// 既存のセルの値を更新。

					( (CUndoRedoセル<int>) oセル仮想 ).変更後の値 = this.hScrollBarDLEVEL.Value;
				}
				else
				{
					// 新しいセルを追加。

					this.mgrUndoRedo管理者.tノードを追加する( 
						new CUndoRedoセル<int>( 
							this.hScrollBarDLEVEL,
							new DGUndoを実行する<int>( this.nDLEVEL_Undo ),
							new DGRedoを実行する<int>( this.nDLEVEL_Redo ), 
							this.nDLEVEL_以前の値, this.hScrollBarDLEVEL.Value ) );


					// Undo ボタンを有効にする。

					this.tUndoRedo用GUIの有効・無効を設定する();
				}
			}
			//-----------------
			#endregion

			
			// Undo 用に値を保管しておく。

			this.nDLEVEL_以前の値 = this.hScrollBarDLEVEL.Value;


			// 完了。

			CUndoRedo管理.bUndoRedoした直後 = false;
			this.b未保存 = true;
		}
		private void nDLEVEL_Undo( int n変更前, int n変更後 )
		{
			// 変更前の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxDLEVEL.Text = n変更前.ToString();

			this.textBoxDLEVEL.Focus();


			// 値を水平スクロールバーにも反映。

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.hScrollBarDLEVEL.Value = n変更前;
		}
		private void nDLEVEL_Redo( int n変更前, int n変更後 )
		{
			// 変更後の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxDLEVEL.Text = n変更後.ToString();

			this.textBoxDLEVEL.Focus();


			// 値を水平スクロールバーにも反映。

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.hScrollBarDLEVEL.Value = n変更後;
		}

		private int nGLEVEL_以前の値;
		private void textBoxGLEVEL_TextChanged( object sender, EventArgs e )
		{
			// 何もしない。→ 数字以外が入力されていることもあるため、Leaveまで待つ。
		}
		private void textBoxGLEVEL_Leave( object sender, EventArgs e )
		{
			if( this.textBoxGLEVEL.Text.Length > 0 )
			{
				// 数値チェック。

				int n値;
				if( !int.TryParse( this.textBoxGLEVEL.Text, out n値 ) )
				{
					n値 = 0;
				}
				else if( n値 < 0 )
				{
					n値 = 0;
				}
				else if( n値 > 100 )
				{
					n値 = 100;
				}


				// 値を水平スクロールバーにも反映。

				if( this.hScrollBarGLEVEL.Value != n値 )
				{
					this.t次のプロパティ変更処理がUndoRedoリストに載るようにする();
					this.hScrollBarGLEVEL.Value = n値;		// ここで hScrollBarGLEVEL_ValueChanged が発動 → UndoRedo処理はそちらで。
				}
			}
		}
		private void hScrollBarGLEVEL_ValueChanged( object sender, EventArgs e )
		{
			// 値をテキストボックスにも反映。

			this.textBoxGLEVEL.Text = this.hScrollBarGLEVEL.Value.ToString();

			// Undo/Redo リストを修正。

			#region [ Undo/Redo リストを修正する。]
			//-----------------
			if( !CUndoRedo管理.bUndoRedoした直後 )
			{
				CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

				if( ( oセル仮想 != null ) && oセル仮想.b所有権がある( this.hScrollBarGLEVEL ) )
				{
					// 既存のセルの値を更新。

					( (CUndoRedoセル<int>) oセル仮想 ).変更後の値 = this.hScrollBarGLEVEL.Value;
				}
				else
				{
					// 新しいセルを追加。

					this.mgrUndoRedo管理者.tノードを追加する(
						new CUndoRedoセル<int>(
							this.hScrollBarGLEVEL,
							new DGUndoを実行する<int>( this.nGLEVEL_Undo ),
							new DGRedoを実行する<int>( this.nGLEVEL_Redo ),
							this.nGLEVEL_以前の値, this.hScrollBarGLEVEL.Value ) );


					// Undo ボタンを有効にする。
					
					this.tUndoRedo用GUIの有効・無効を設定する();
				}
			}
			//-----------------
			#endregion


			// Undo 用に値を保管しておく。

			this.nGLEVEL_以前の値 = this.hScrollBarGLEVEL.Value;
			
			
			// 完了。

			CUndoRedo管理.bUndoRedoした直後 = false;
			this.b未保存 = true;
		}
		private void nGLEVEL_Undo( int n変更前, int n変更後 )
		{
			// 変更前の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxGLEVEL.Text = n変更前.ToString();

			this.textBoxGLEVEL.Focus();


			// 値を水平スクロールバーにも反映する。

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.hScrollBarGLEVEL.Value = n変更前;
		}
		private void nGLEVEL_Redo( int n変更前, int n変更後 )
		{
			// 変更後の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxGLEVEL.Text = n変更後.ToString();

			this.textBoxGLEVEL.Focus();

			
			// 値を水平スクロールバーにも反映する。

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.hScrollBarGLEVEL.Value = n変更後;
		}

		private int nBLEVEL_以前の値;
		private void textBoxBLEVEL_TextChanged( object sender, EventArgs e )
		{
			// 何もしない。→ 数字以外が入力されていることもあるため、Leaveまで待つ。
		}
		private void textBoxBLEVEL_Leave( object sender, EventArgs e )
		{
			if( this.textBoxBLEVEL.Text.Length > 0 )
			{
				// 数値チェック。

				int n値;
				if( !int.TryParse( this.textBoxBLEVEL.Text, out n値 ) )
				{
					n値 = 0;
				}
				else if( n値 < 0 )
				{
					n値 = 0;
				}
				else if( n値 > 100 )
				{
					n値 = 100;
				}


				// 値を水平スクロールバーにも反映。

				if( this.hScrollBarBLEVEL.Value != n値 )
				{
					this.t次のプロパティ変更処理がUndoRedoリストに載るようにする();
					this.hScrollBarBLEVEL.Value = n値;		// ここで hScrollBarBLEVEL_ValueChanged が発動 → UndoRedo処理はそちらで。
				}
			}
		}
		private void hScrollBarBLEVEL_ValueChanged( object sender, EventArgs e )
		{
			// 値をテキストボックスにも反映。

			this.textBoxBLEVEL.Text = this.hScrollBarBLEVEL.Value.ToString();


			// Undo/Redo リストを修正。

			#region [ Undo/Redo リストを修正する。]
			//-----------------
			if( !CUndoRedo管理.bUndoRedoした直後 )
			{
				CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

				if( ( oセル仮想 != null ) && oセル仮想.b所有権がある( this.hScrollBarBLEVEL ) )
				{
					// 既存のセルの値を更新。
					
					( (CUndoRedoセル<int>) oセル仮想 ).変更後の値 = this.hScrollBarBLEVEL.Value;
				}
				else
				{
					// 新しいセルを追加。

					this.mgrUndoRedo管理者.tノードを追加する( 
						new CUndoRedoセル<int>(
							this.hScrollBarBLEVEL,
							new DGUndoを実行する<int>( this.nBLEVEL_Undo ),
							new DGRedoを実行する<int>( this.nBLEVEL_Redo ),
							this.nBLEVEL_以前の値, this.hScrollBarBLEVEL.Value ) );

		
					// Undo ボタンを有効にする。

					this.tUndoRedo用GUIの有効・無効を設定する();
				}
			}
			//-----------------
			#endregion

			
			// Undo 用に値を保管しておく。

			this.nBLEVEL_以前の値 = this.hScrollBarBLEVEL.Value;


			// 完了。

			CUndoRedo管理.bUndoRedoした直後 = false;
			this.b未保存 = true;
		}
		private void nBLEVEL_Undo( int n変更前, int n変更後 )
		{
			// 変更前の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxBLEVEL.Text = n変更前.ToString();

			this.textBoxBLEVEL.Focus();


			// 値を水平スクロールバーにも反映。

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.hScrollBarBLEVEL.Value = n変更前;
		}
		private void nBLEVEL_Redo( int n変更前, int n変更後 )
		{
			// 変更後の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxBLEVEL.Text = n変更後.ToString();

			this.textBoxBLEVEL.Focus();


			// 値を水平スクロールバーにも反映。

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.hScrollBarBLEVEL.Value = n変更後;
		}

		private string textBoxパネル_以前の値 = "";
		private void textBoxパネル_TextChanged( object sender, EventArgs e )
		{
			// Undo/Redo リストを修正。

			#region [ Undo/Redo リストを修正する。]
			//-----------------
			if( !CUndoRedo管理.bUndoRedoした直後 )
			{
				CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

				if( ( oセル仮想 != null ) && oセル仮想.b所有権がある( this.textBoxパネル ) )
				{
					// 既存のセルの値を更新。

					( (CUndoRedoセル<string>) oセル仮想 ).変更後の値 = this.textBoxパネル.Text;
				}
				else
				{
					// 新しいセルを追加。

					this.mgrUndoRedo管理者.tノードを追加する(
						new CUndoRedoセル<string>(
							this.textBoxパネル, 
							new DGUndoを実行する<string>( this.textBoxパネル_Undo ),
							new DGRedoを実行する<string>( this.textBoxパネル_Redo ), 
							this.textBoxパネル_以前の値, this.textBoxパネル.Text ) );


					// Undoボタンを有効にする。

					this.tUndoRedo用GUIの有効・無効を設定する();
				}
			}
			//-----------------
			#endregion


			// Undo 用に値を保管しておく。

			this.textBoxパネル_以前の値 = this.textBoxパネル.Text;


			// 完了。

			CUndoRedo管理.bUndoRedoした直後 = false;
			this.b未保存 = true;
		}
		private void textBoxパネル_Leave( object sender, EventArgs e )
		{
			CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

			if( oセル仮想 != null )
				oセル仮想.t所有権の放棄( this.textBoxパネル );
		}
		private void textBoxパネル_Undo( string str変更前, string str変更後 )
		{
			// 変更前の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxパネル.Text = str変更前;

			this.textBoxパネル.Focus();
		}
		private void textBoxパネル_Redo( string str変更前, string str変更後 )
		{
			// 変更後の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxパネル.Text = str変更後;

			this.textBoxパネル.Focus();
		}

		private string textBoxPREVIEW_以前の値 = "";
		private void textBoxPREVIEW_TextChanged( object sender, EventArgs e )
		{
			// Undo/Redo リストを修正。

			#region [ Undo/Redo リストを修正する。]
			//-----------------
			if( !CUndoRedo管理.bUndoRedoした直後 )
			{
				CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

				if( ( oセル仮想 != null ) && oセル仮想.b所有権がある( this.textBoxPREVIEW ) )
				{
					// 既存のセルの値を更新。

					( (CUndoRedoセル<string>) oセル仮想 ).変更後の値 = this.textBoxPREVIEW.Text;
				}
				else
				{
					// 新しいセルを追加。

					this.mgrUndoRedo管理者.tノードを追加する( 
						new CUndoRedoセル<string>(
							this.textBoxPREVIEW,
							new DGUndoを実行する<string>( this.textBoxPREVIEW_Undo ),
							new DGRedoを実行する<string>( this.textBoxPREVIEW_Redo ), 
							this.textBoxPREVIEW_以前の値, this.textBoxPREVIEW.Text ) );

					
					// Undo ボタンを有効にする。

					this.tUndoRedo用GUIの有効・無効を設定する();
				}
			}
			//-----------------
			#endregion


			// Undo 用に値を保管しておく。

			this.textBoxPREVIEW_以前の値 = this.textBoxPREVIEW.Text;


			// 完了。

			CUndoRedo管理.bUndoRedoした直後 = false;
			this.b未保存 = true;
		}
		private void textBoxPREVIEW_Leave( object sender, EventArgs e )
		{
			CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

			if( oセル仮想 != null )
				oセル仮想.t所有権の放棄( this.textBoxPREVIEW );
		}
		private void textBoxPREVIEW_Undo( string str変更前, string str変更後 )
		{
			// 変更前の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxPREVIEW.Text = str変更前;

			this.textBoxPREVIEW.Focus();
		}
		private void textBoxPREVIEW_Redo( string str変更前, string str変更後 )
		{
			// 変更前の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxPREVIEW.Text = str変更後;

			this.textBoxPREVIEW.Focus();
		}

		private string textBoxPREIMAGE_以前の値 = "";
		private void textBoxPREIMAGE_TextChanged( object sender, EventArgs e )
		{
			// Undo/Redo リストを修正。

			#region [ Undo/Redo リストを修正する。]
			//-----------------
			if( !CUndoRedo管理.bUndoRedoした直後 )
			{
				CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

				if( ( oセル仮想 != null ) && oセル仮想.b所有権がある( this.textBoxPREIMAGE ) )
				{
					// 既存のセルの値を更新。

					( (CUndoRedoセル<string>) oセル仮想 ).変更後の値 = this.textBoxPREIMAGE.Text;
				}
				else
				{
					// 新規のセルを作成。

					this.mgrUndoRedo管理者.tノードを追加する( 
						new CUndoRedoセル<string>( 
							this.textBoxPREIMAGE, 
							new DGUndoを実行する<string>( this.textBoxPREIMAGE_Undo ), 
							new DGRedoを実行する<string>( this.textBoxPREIMAGE_Redo ), 
							this.textBoxPREIMAGE_以前の値, this.textBoxPREIMAGE.Text ) );


					// Undo ボタンを有効にする。

					this.tUndoRedo用GUIの有効・無効を設定する();
				}
			}
			//-----------------
			#endregion


			// Undo用に値を保管しておく。]

			this.textBoxPREIMAGE_以前の値 = this.textBoxPREIMAGE.Text;
			
			
			// 完了。

			CUndoRedo管理.bUndoRedoした直後 = false;
			this.b未保存 = true;
		}
		private void textBoxPREIMAGE_Leave( object sender, EventArgs e )
		{
			CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

			if( oセル仮想 != null )
				oセル仮想.t所有権の放棄( this.textBoxPREIMAGE );
		}
		private void textBoxPREIMAGE_Undo( string str変更前, string str変更後 )
		{
			// 変更前の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxPREIMAGE.Text = str変更前;

			this.textBoxPREIMAGE.Focus();
		}
		private void textBoxPREIMAGE_Redo( string str変更前, string str変更後 )
		{
			// 変更後の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxPREIMAGE.Text = str変更後;

			this.textBoxPREIMAGE.Focus();
		}

		private string textBoxSTAGEFILE_以前の値 = "";
		private void textBoxSTAGEFILE_TextChanged( object sender, EventArgs e )
		{
			// Undo/Redo リストを修正。

			#region [ Undo/Redo リストを修正する。]
			//-----------------
			if( !CUndoRedo管理.bUndoRedoした直後 )
			{
				CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

				if( ( oセル仮想 != null ) && oセル仮想.b所有権がある( this.textBoxSTAGEFILE ) )
				{
					// 既存のセルの値を更新。

					( (CUndoRedoセル<string>) oセル仮想 ).変更後の値 = this.textBoxSTAGEFILE.Text;
				}
				else
				{
					// 新しいセルを追加。

					this.mgrUndoRedo管理者.tノードを追加する( 
						new CUndoRedoセル<string>(
							this.textBoxSTAGEFILE,
							new DGUndoを実行する<string>( this.textBoxSTAGEFILE_Undo ),
							new DGRedoを実行する<string>( this.textBoxSTAGEFILE_Redo ), 
							this.textBoxSTAGEFILE_以前の値, this.textBoxSTAGEFILE.Text ) );


					// Undo ボタンを有効にする。

					this.tUndoRedo用GUIの有効・無効を設定する();
				}
			}
			//-----------------
			#endregion


			// Undo 用に値を保管しておく。

			this.textBoxSTAGEFILE_以前の値 = this.textBoxSTAGEFILE.Text;


			// 完了。

			CUndoRedo管理.bUndoRedoした直後 = false;
			this.b未保存 = true;
		}
		private void textBoxSTAGEFILE_Leave( object sender, EventArgs e )
		{
			CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

			if( oセル仮想 != null )
				oセル仮想.t所有権の放棄( this.textBoxSTAGEFILE );
		}
		private void textBoxSTAGEFILE_Undo( string str変更前, string str変更後 )
		{
			// 変更前の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxSTAGEFILE.Text = str変更前;

			this.textBoxSTAGEFILE.Focus();
		}
		private void textBoxSTAGEFILE_Redo( string str変更前, string str変更後 )
		{
			// 変更後の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxSTAGEFILE.Text = str変更後;

			this.textBoxSTAGEFILE.Focus();
		}

		private string textBoxBACKGROUND_以前の値 = "";
		private void textBoxBACKGROUND_TextChanged( object sender, EventArgs e )
		{
			// Undo/Redo リストを修正。

			#region [ Undo/Redo リストを修正する。]
			//-----------------
			if( !CUndoRedo管理.bUndoRedoした直後 )
			{
				CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

				if( ( oセル仮想 != null ) && oセル仮想.b所有権がある( this.textBoxBACKGROUND ) )
				{
					// 既存のセルの値を更新。

					( (CUndoRedoセル<string>) oセル仮想 ).変更後の値 = this.textBoxBACKGROUND.Text;
				}
				else
				{
					// 新しいセルを追加。

					this.mgrUndoRedo管理者.tノードを追加する( 
						new CUndoRedoセル<string>(
							this.textBoxBACKGROUND,
							new DGUndoを実行する<string>( this.textBoxBACKGROUND_Undo ),
							new DGRedoを実行する<string>( this.textBoxBACKGROUND_Redo ), 
							this.textBoxBACKGROUND_以前の値, this.textBoxBACKGROUND.Text ) );


					// Undo ボタンを有効にする。

					this.tUndoRedo用GUIの有効・無効を設定する();
				}
			}
			//-----------------
			#endregion


			// Undo 用の値を保管しておく。

			this.textBoxBACKGROUND_以前の値 = this.textBoxBACKGROUND.Text;


			// 完了。

			CUndoRedo管理.bUndoRedoした直後 = false;
			this.b未保存 = true;
		}
		private void textBoxBACKGROUND_Leave( object sender, EventArgs e )
		{
			CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

			if( oセル仮想 != null )
				oセル仮想.t所有権の放棄( this.textBoxBACKGROUND );
		}
		private void textBoxBACKGROUND_Undo( string str変更前, string str変更後 )
		{
			// 変更前の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxBACKGROUND.Text = str変更前;

			this.textBoxBACKGROUND.Focus();
		}
		private void textBoxBACKGROUND_Redo( string str変更前, string str変更後 )
		{
			// 変更後の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxBACKGROUND.Text = str変更後;

			this.textBoxBACKGROUND.Focus();
		}

		private string textBoxRESULTIMAGE_以前の値 = "";
		private void textBoxRESULTIMAGE_TextChanged( object sender, EventArgs e )
		{
			// Undo/Redo リストを修正。

			#region [ Undo/Redo リストを修正する。]
			//-----------------
			if( !CUndoRedo管理.bUndoRedoした直後 )
			{
				CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

				if( ( oセル仮想 != null ) && oセル仮想.b所有権がある( this.textBoxRESULTIMAGE ) )
				{
					// 既存のセルの値を更新。

					( (CUndoRedoセル<string>) oセル仮想 ).変更後の値 = this.textBoxRESULTIMAGE.Text;
				}
				else
				{
					// 新しいセルを追加。

					this.mgrUndoRedo管理者.tノードを追加する( 
						new CUndoRedoセル<string>(
							this.textBoxRESULTIMAGE,
							new DGUndoを実行する<string>( this.textBoxRESULTIMAGE_Undo ),
							new DGRedoを実行する<string>( this.textBoxRESULTIMAGE_Redo ), 
							this.textBoxRESULTIMAGE_以前の値, this.textBoxRESULTIMAGE.Text ) );


					// Undo ボタンを有効にする。

					this.tUndoRedo用GUIの有効・無効を設定する();
				}
			}
			//-----------------
			#endregion


			// Undo 用に値を保管しておく。

			this.textBoxRESULTIMAGE_以前の値 = this.textBoxRESULTIMAGE.Text;


			// 完了。

			CUndoRedo管理.bUndoRedoした直後 = false;
			this.b未保存 = true;
		}
		private void textBoxRESULTIMAGE_Leave( object sender, EventArgs e )
		{
			CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();
		
			if( oセル仮想 != null )
				oセル仮想.t所有権の放棄( this.textBoxRESULTIMAGE );
		}
		private void textBoxRESULTIMAGE_Undo( string str変更前, string str変更後 )
		{
			// 変更前の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxRESULTIMAGE.Text = str変更前;

			this.textBoxRESULTIMAGE.Focus();
		}
		private void textBoxRESULTIMAGE_Redo( string str変更前, string str変更後 )
		{
			// 変更後の値に戻す。

			this.tタブを選択する( Eタブ種別.基本情報 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBoxRESULTIMAGE.Text = str変更後;

			this.textBoxRESULTIMAGE.Focus();
		}

		private void buttonPREVIEW参照_Click( object sender, EventArgs e )
		{
			string str初期フォルダ名 = this.str作業フォルダ名;

			if( this.textBoxPREVIEW.Text.Length > 0 )
				str初期フォルダ名 = this.strファイルの存在するディレクトリを絶対パスで返す( this.textBoxPREVIEW.Text );

			string strファイル名 = this.tファイル選択ダイアログでファイルを選択し相対パスにして返す(
				Resources.strプレビュー音ファイル選択ダイアログのタイトル, 
				Resources.strサウンドファイル選択ダイアログのフィルタ, 
				str初期フォルダ名 );

			if( strファイル名.Length > 0 )
			{
				this.textBoxPREVIEW.Text = strファイル名;
				this.b未保存 = true;
			}
		}
		private void buttonPREIMAGE参照_Click( object sender, EventArgs e )
		{
			string str初期フォルダ名 = this.str作業フォルダ名;

			if( this.textBoxPREIMAGE.Text.Length > 0 )
				str初期フォルダ名 = this.strファイルの存在するディレクトリを絶対パスで返す( this.textBoxPREIMAGE.Text );

			string strファイル名 = this.tファイル選択ダイアログでファイルを選択し相対パスにして返す(
				Resources.strプレビュー画像ファイル選択ダイアログのタイトル, 
				Resources.str画像ファイル選択ダイアログのフィルタ,
				str初期フォルダ名 );
			
			if( strファイル名.Length > 0 )
			{
				this.textBoxPREIMAGE.Text = strファイル名;
				this.b未保存 = true;
			}
		}
		private void buttonSTAGEFILE参照_Click( object sender, EventArgs e )
		{
			string str初期フォルダ名 = this.str作業フォルダ名;

			if( this.textBoxSTAGEFILE.Text.Length > 0 )
				str初期フォルダ名 = this.strファイルの存在するディレクトリを絶対パスで返す( this.textBoxSTAGEFILE.Text );

			string strファイル名 = this.tファイル選択ダイアログでファイルを選択し相対パスにして返す(
				Resources.strステージ画像ファイル選択ダイアログのタイトル,
				Resources.str画像ファイル選択ダイアログのフィルタ, 
				str初期フォルダ名 );

			if( strファイル名.Length > 0 )
			{
				this.textBoxSTAGEFILE.Text = strファイル名;
				this.b未保存 = true;
			}
		}
		private void buttonBACKGROUND参照_Click( object sender, EventArgs e )
		{
			string str初期フォルダ名 = this.str作業フォルダ名;

			if( this.textBoxBACKGROUND.Text.Length > 0 )
				str初期フォルダ名 = this.strファイルの存在するディレクトリを絶対パスで返す( this.textBoxBACKGROUND.Text );

			string strファイル名 = this.tファイル選択ダイアログでファイルを選択し相対パスにして返す(
				Resources.str背景画像ファイル選択ダイアログのタイトル, 
				Resources.str画像ファイル選択ダイアログのフィルタ, 
				str初期フォルダ名 );

			if( strファイル名.Length > 0 )
			{
				this.textBoxBACKGROUND.Text = strファイル名;
				this.b未保存 = true;
			}
		}
		private void buttonRESULTIMAGE参照_Click( object sender, EventArgs e )
		{
			string str初期フォルダ名 = this.str作業フォルダ名;
			
			if( this.textBoxRESULTIMAGE.Text.Length > 0 )
				str初期フォルダ名 = this.strファイルの存在するディレクトリを絶対パスで返す( this.textBoxRESULTIMAGE.Text );

			string strファイル名 = this.tファイル選択ダイアログでファイルを選択し相対パスにして返す(
				Resources.str結果画像ファイル選択ダイアログのタイトル,
				Resources.str画像ファイル選択ダイアログのフィルタ, 
				str初期フォルダ名 );

			if( strファイル名.Length > 0 )
			{
				this.textBoxRESULTIMAGE.Text = strファイル名;
				this.b未保存 = true;
			}
		}
		//-----------------
		#endregion
		#region [ GUIイベント：WAVリスト関連 ]
		//-----------------
		private void listViewWAVリスト_Click( object sender, EventArgs e )
		{
			#region [ プレビュー音を再生する。]
			//-----------------
			if( this.listViewWAVリスト.SelectedIndices.Count > 0 && this.toolStripButtonWAVリストプレビュースイッチ.Checked )
			{
				ListViewItem item = this.listViewWAVリスト.Items[ this.listViewWAVリスト.SelectedIndices[ 0 ] ];
				this.mgrWAVリスト管理者.tプレビュー音を再生する( C変換.n36進数2桁の文字列を数値に変換して返す( item.SubItems[ 1 ].Text ) );
			}
			//-----------------
			#endregion
		}
		private void listViewWAVリスト_DoubleClick( object sender, EventArgs e )
		{
			#region [ サウンドプロパティを開いて編集する。]
			//-----------------
			if( this.mgrWAVリスト管理者.n現在選択中のItem番号0to1294 < 0 )
				return;		// 選択されていない

			this.mgrWAVリスト管理者.tサウンドプロパティを開いて編集する( this.mgrWAVリスト管理者.n現在選択中のItem番号0to1294 + 1, this.str作業フォルダ名 );
			//-----------------
			#endregion
		}
		private void listViewWAVリスト_ItemDrag( object sender, ItemDragEventArgs e )
		{
			#region [ CWAVデータをDragDrop用データに格納し、DoDragDrop()を呼び出す。]
			//-----------------
			var item = (ListViewItem) e.Item;

			var data = new Cチップパレット向けDragDropデータ() {
				n種類 = 0,
				strラベル名 = item.SubItems[ 0 ].Text,
				n番号1to1295 = C変換.n36進数2桁の文字列を数値に変換して返す( item.SubItems[ 1 ].Text ),
				strファイル名 = item.SubItems[ 2 ].Text,
				col文字色 = item.ForeColor,
				col背景色 = item.BackColor,
			};

			this.DoDragDrop( data, DragDropEffects.Copy );

			//-----------------
			#endregion
		}
		private void listViewWAVリスト_KeyPress( object sender, KeyPressEventArgs e )
		{
			#region [ ENTER が押下されたら、サウンドプロパティを開いて編集する。]
			//-----------------
			if( e.KeyChar == (char) Keys.Return )
			{
				if( this.mgrWAVリスト管理者.n現在選択中のItem番号0to1294 < 0 )
					return;		// 選択されていない

				this.mgrWAVリスト管理者.tサウンドプロパティを開いて編集する( this.mgrWAVリスト管理者.n現在選択中のItem番号0to1294 + 1, this.str作業フォルダ名 );
			}
			//-----------------
			#endregion
		}
		private void listViewWAVリスト_MouseEnter( object sender, EventArgs e )
		{
			#region [ WAVリストにフォーカスを移動する。]
			//-----------------
			if( this.appアプリ設定.AutoFocus )
				this.mgrWAVリスト管理者.tWAVリストにフォーカスを当てる();
			//-----------------
			#endregion
		}
		private void listViewWAVリスト_RetrieveVirtualItem( object sender, RetrieveVirtualItemEventArgs e )
		{
			e.Item = this.mgrWAVリスト管理者.tCWAVとListViewItemを生成して返す( e.ItemIndex + 1 );
		}
		private void listViewWAVリスト_SelectedIndexChanged( object sender, EventArgs e )
		{
			#region [ WAV, BMP, AVI のカーソルを、選択された行に全部合わせる。]
			//-----------------
			if( this.listViewWAVリスト.SelectedIndices.Count > 0 )
				this.tWAV・BMP・AVIリストのカーソルを全部同じ行に合わせる( this.listViewWAVリスト.SelectedIndices[ 0 ] );
			//-----------------
			#endregion
		}

		private void toolStripButtonWAVリストプレビュースイッチ_CheckStateChanged( object sender, EventArgs e )
		{
			#region [ 再生ボタンと停止ボタンの有効・無効を設定する。]
			//-----------------
			bool b再生有効 = ( this.toolStripButtonWAVリストプレビュースイッチ.CheckState == CheckState.Checked ) ? true : false;
			this.toolStripButtonWAVリストプレビュー再生開始.Enabled = b再生有効;
			this.toolStripButtonWAVリストプレビュー再生停止.Enabled = b再生有効;
			//-----------------
			#endregion

			#region [ 無効かつ再生中ならプレビュー音を停止する。]
			//-----------------
			if( !b再生有効 )
				this.mgrWAVリスト管理者.tプレビュー音を停止する();
			//-----------------
			#endregion
		}
		private void toolStripButtonWAVリストプレビュー再生開始_Click( object sender, EventArgs e )
		{
			#region [ 現在選択中のWAVのプレビュー音を再生する。]
			//-----------------
			if( this.listViewWAVリスト.SelectedIndices.Count <= 0 )
				return; // 選択されてない

			bool b再生有効 = ( this.toolStripButtonWAVリストプレビュースイッチ.CheckState == CheckState.Checked ) ? true : false;

            if (b再生有効)
			{
				int nWAV番号1to1295 = this.mgrWAVリスト管理者.n現在選択中のItem番号0to1294 + 1;
				this.mgrWAVリスト管理者.tプレビュー音を再生する( nWAV番号1to1295 );
			}
			//-----------------
			#endregion
		}
		private void toolStripButtonWAVリストプレビュー再生停止_Click( object sender, EventArgs e )
		{
			this.mgrWAVリスト管理者.tプレビュー音を停止する();
		}
		private void toolStripButtonWAVリスト上移動_Click( object sender, EventArgs e )
		{
			#region [ 上の行とWAVを交換する。]
			//-----------------
			if( this.listViewWAVリスト.SelectedIndices.Count <= 0 )
				return; // 選択されていない

			int n選択されたItem番号0to1294 = this.listViewWAVリスト.SelectedIndices[ 0 ];

			if( n選択されたItem番号0to1294 == 0 )
				return;	// 最上行なので無視

			this.mgrWAVリスト管理者.tItemを交換する( n選択されたItem番号0to1294, n選択されたItem番号0to1294 - 1 );
			//-----------------
			#endregion
		}
		private void toolStripButtonWAVリスト下移動_Click( object sender, EventArgs e )
		{
			if( this.listViewWAVリスト.SelectedIndices.Count <= 0 )
				return; // 選択されていない

			int n選択されたItem番号0to1294 = this.listViewWAVリスト.SelectedIndices[ 0 ];

			if( n選択されたItem番号0to1294 >= 1294 )
				return; // 最下行なので無視

			this.mgrWAVリスト管理者.tItemを交換する( n選択されたItem番号0to1294, n選択されたItem番号0to1294 + 1 );
		}
		//-----------------
		#endregion
		#region [ GUIイベント：BMPリスト関連 ]
		//-----------------
		private void listViewBMPリスト_Click( object sender, EventArgs e )
		{
			// 何もしない
		}
		private void listViewBMPリスト_DoubleClick( object sender, EventArgs e )
		{
			#region [ 画像プロパティを開いて編集する。]
			//-----------------
			if( this.mgrBMPリスト管理者.n現在選択中のItem番号0to1294 < 0 )
				return;		// 選択されていない

			this.mgrBMPリスト管理者.t画像プロパティを開いて編集する( this.mgrBMPリスト管理者.n現在選択中のItem番号0to1294 + 1, this.str作業フォルダ名 );
			//-----------------
			#endregion
		}
		private void listViewBMPリスト_ItemDrag( object sender, ItemDragEventArgs e )
		{
			#region [ CBMPデータをDragDrop用データに格納し、DoDragDrop()を呼び出す。]
			//-----------------
			var item = (ListViewItem) e.Item;

			var data = new Cチップパレット向けDragDropデータ() {
				n種類 = 1,
				strラベル名 = item.SubItems[ 1 ].Text,
				n番号1to1295 = C変換.n36進数2桁の文字列を数値に変換して返す( item.SubItems[ 2 ].Text ),
				strファイル名 = item.SubItems[ 3 ].Text,
				col文字色 = item.ForeColor,
				col背景色 = item.BackColor,
			};

			this.DoDragDrop( data, DragDropEffects.Copy );
			//-----------------
			#endregion
		}
		private void listViewBMPリスト_KeyPress( object sender, KeyPressEventArgs e )
		{
			#region [ ENTER が押下されたら、画像プロパティを開いて編集する。]
			//-----------------
			if( e.KeyChar == (char) Keys.Return )
			{
				if( this.mgrBMPリスト管理者.n現在選択中のItem番号0to1294 < 0 )
					return;		// 選択されていない

				this.mgrBMPリスト管理者.t画像プロパティを開いて編集する( this.mgrBMPリスト管理者.n現在選択中のItem番号0to1294 + 1, this.str作業フォルダ名 );
			}
			//-----------------
			#endregion
		}
		private void listViewBMPリスト_MouseEnter( object sender, EventArgs e )
		{
			#region [ BMPリストにフォーカスを移動する。]
			//-----------------
			if( this.appアプリ設定.AutoFocus )
				this.mgrBMPリスト管理者.tBMPリストにフォーカスを当てる();
			//-----------------
			#endregion
		}
		private void listViewBMPリスト_RetrieveVirtualItem( object sender, RetrieveVirtualItemEventArgs e )
		{
			e.Item = this.mgrBMPリスト管理者.tCBMPとListViewItemを生成して返す( e.ItemIndex + 1 );
		}
		private void listViewBMPリスト_SelectedIndexChanged( object sender, EventArgs e )
		{
			#region [ WAV, BMP, AVI のカーソルを、選択された行に全部合わせる。]
			//-----------------
			if( this.listViewBMPリスト.SelectedIndices.Count > 0 )
				this.tWAV・BMP・AVIリストのカーソルを全部同じ行に合わせる( this.listViewBMPリスト.SelectedIndices[ 0 ] );
			//-----------------
			#endregion
		}

		private void toolStripButtonBMPリスト上移動_Click( object sender, EventArgs e )
		{
			#region [ 上の行とBMPを交換する。]
			//-----------------
			if( this.listViewBMPリスト.SelectedIndices.Count <= 0 )
				return; // 選択されていない

			int n選択されたItem番号0to1294 = this.listViewBMPリスト.SelectedIndices[ 0 ];

			if( n選択されたItem番号0to1294 != 0 )
				this.mgrBMPリスト管理者.tItemを交換する( n選択されたItem番号0to1294, n選択されたItem番号0to1294 - 1 );
			//-----------------
			#endregion
		}
		private void toolStripButtonBMPリスト下移動_Click( object sender, EventArgs e )
		{
			#region [ 下の行とBMPを交換する。]
			//-----------------
			if( this.listViewBMPリスト.SelectedIndices.Count <= 0 )
				return; // 選択されていない

			int n選択されたItem番号0to1294 = this.listViewBMPリスト.SelectedIndices[ 0 ];

			if( n選択されたItem番号0to1294 >= 1294 )
				return; // 最下行なので無視

			this.mgrBMPリスト管理者.tItemを交換する( n選択されたItem番号0to1294, n選択されたItem番号0to1294 + 1 );
			//-----------------
			#endregion
		}
		//-----------------
		#endregion
		#region [ GUIイベント：AVIリスト関連 ]
		//-----------------
		private void listViewAVIリスト_Click( object sender, EventArgs e )
		{
			// 何もしない
		}
		private void listViewAVIリスト_DoubleClick( object sender, EventArgs e )
		{
			#region [ 動画プロパティを開いて編集する。]
			//-----------------
			if( this.mgrAVIリスト管理者.n現在選択中のItem番号0to1294 < 0 )
				return;	// 選択されていない

			this.mgrAVIリスト管理者.t動画プロパティを開いて編集する( this.mgrAVIリスト管理者.n現在選択中のItem番号0to1294 + 1, this.str作業フォルダ名 );
			//-----------------
			#endregion
		}
		private void listViewAVIリスト_ItemDrag( object sender, ItemDragEventArgs e )
		{
			#region [ CAVIデータをDragDrop用データに格納してDoDragDrop()を呼び出す。]
			//-----------------
			var item = (ListViewItem) e.Item;

			var data = new Cチップパレット向けDragDropデータ() {
				n種類 = 2,
				strラベル名 = item.SubItems[ 0 ].Text,
				n番号1to1295 = C変換.n36進数2桁の文字列を数値に変換して返す( item.SubItems[ 1 ].Text ),
				strファイル名 = item.SubItems[ 2 ].Text,
				col文字色 = item.ForeColor,
				col背景色 = item.BackColor,
			};

			this.DoDragDrop( data, DragDropEffects.Copy );
			//-----------------
			#endregion
		}
		private void listViewAVIリスト_KeyPress( object sender, KeyPressEventArgs e )
		{
			#region [ ENTER が押下されたら、動画プロパティを開いて編集する。]
			//-----------------
			if( e.KeyChar == (char) Keys.Return ) 
			{
				if( this.mgrAVIリスト管理者.n現在選択中のItem番号0to1294 < 0 )
					return;		// 選択されてない

				this.mgrAVIリスト管理者.t動画プロパティを開いて編集する( this.mgrAVIリスト管理者.n現在選択中のItem番号0to1294 + 1, this.str作業フォルダ名 );
			}
			//-----------------
			#endregion
		}
		private void listViewAVIリスト_MouseEnter( object sender, EventArgs e )
		{
			#region [ AVIリストにフォーカスを移動する。]
			//-----------------
			if( this.appアプリ設定.AutoFocus )
				this.mgrAVIリスト管理者.tAVIリストにフォーカスを当てる();
			//-----------------
			#endregion
		}
		private void listViewAVIリスト_RetrieveVirtualItem( object sender, RetrieveVirtualItemEventArgs e )
		{
			e.Item = this.mgrAVIリスト管理者.tCAVIとListViewItemを生成して返す( e.ItemIndex + 1 );
		}
		private void listViewAVIリスト_SelectedIndexChanged( object sender, EventArgs e )
		{
			#region [ WAV, BMP, AVI のカーソルを、選択された行に全部合わせる。]
			//-----------------
			if( this.listViewAVIリスト.SelectedIndices.Count > 0 )
				this.tWAV・BMP・AVIリストのカーソルを全部同じ行に合わせる( this.listViewAVIリスト.SelectedIndices[ 0 ] );
			//-----------------
			#endregion
		}

		private void toolStripButtonAVIリスト上移動_Click( object sender, EventArgs e )
		{
			#region [ 上の行とAVIを交換する。]
			//-----------------
			if( this.listViewAVIリスト.SelectedIndices.Count <= 0 )
				return; // 選択されていない

			int n選択されたItem番号0to1294 = this.listViewAVIリスト.SelectedIndices[ 0 ];

			if( n選択されたItem番号0to1294 == 0 )
				return; // 最上行なので無視

			this.mgrAVIリスト管理者.tItemを交換する( n選択されたItem番号0to1294, n選択されたItem番号0to1294 - 1 );
			//-----------------
			#endregion
		}
		private void toolStripButtonAVIリスト下移動_Click( object sender, EventArgs e )
		{
			#region [ 下の行とAVIを交換する。]
			//-----------------
			if( this.listViewAVIリスト.SelectedIndices.Count <= 0 )
				return; // 選択されていない

			int n選択されたItem番号0to1294 = this.listViewAVIリスト.SelectedIndices[ 0 ];

			if( n選択されたItem番号0to1294 >= 1294 )
				return; // 最下行なので無視

			this.mgrAVIリスト管理者.tItemを交換する( n選択されたItem番号0to1294, n選択されたItem番号0to1294 + 1 );
			//-----------------
			#endregion
		}
		//-----------------
		#endregion
		#region [ GUIイベント：自由入力関連 ]
		//-----------------
		private string textBox自由入力欄_以前の値 = "";
		private void textBox自由入力欄_TextChanged( object sender, EventArgs e )
		{
			// Undo/Redo リストを修正。

			#region [ Undo/Redo リストを修正する。]
			//-----------------
			if( !CUndoRedo管理.bUndoRedoした直後 )
			{
				CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

				if( ( oセル仮想 != null ) && oセル仮想.b所有権がある( this.textBox自由入力欄 ) )
				{
					// 既存のセルの値を更新。

					( (CUndoRedoセル<string>) oセル仮想 ).変更後の値 = this.textBox自由入力欄.Text;
				}
				else
				{
					// 新しいセルを追加。

					this.mgrUndoRedo管理者.tノードを追加する(
						new CUndoRedoセル<string>(
							this.textBox自由入力欄, 
							new DGUndoを実行する<string>( this.textBox自由入力欄_Undo ),
							new DGRedoを実行する<string>( this.textBox自由入力欄_Redo ),
							this.textBox自由入力欄_以前の値, this.textBox自由入力欄.Text ) );


					// Undoボタンを有効にする。

					this.tUndoRedo用GUIの有効・無効を設定する();
				}
			}
			//-----------------
			#endregion


			// Undo 用に値を保管しておく。

			this.textBox自由入力欄_以前の値 = this.textBox自由入力欄.Text;


			// 完了。

			CUndoRedo管理.bUndoRedoした直後 = false;
			this.b未保存 = true;
		}
		private void textBox自由入力欄_Leave( object sender, EventArgs e )
		{
			CUndoRedoセル仮想 oセル仮想 = this.mgrUndoRedo管理者.tUndoするノードを取得して返す・見るだけ();

			if( oセル仮想 != null )
				oセル仮想.t所有権の放棄( this.textBox自由入力欄 );
		}
		private void textBox自由入力欄_Undo( string str変更前, string str変更後 )
		{
			// 変更前の値に戻す。

			this.tタブを選択する( Eタブ種別.自由入力 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBox自由入力欄.Text = str変更前;

			this.textBox自由入力欄.Focus();
		}
		private void textBox自由入力欄_Redo( string str変更前, string str変更後 )
		{
			// 変更後の値に戻す。

			this.tタブを選択する( Eタブ種別.自由入力 );

			this.t次のプロパティ変更処理がUndoRedoリストに載らないようにする();
			this.textBox自由入力欄.Text = str変更後;

			this.textBox自由入力欄.Focus();
		}
		//-----------------
		#endregion
	
		#region [ GUIイベント：メニューバー [ファイル] ]
		//-----------------
		private void toolStripMenuItem新規_Click( object sender, EventArgs e )
		{
			this.tシナリオ・新規作成();
		}
		private void toolStripMenuItem開く_Click( object sender, EventArgs e )
		{
			this.tシナリオ・開く();
		}
		private void toolStripMenuItem上書き保存_Click( object sender, EventArgs e )
		{
			this.tシナリオ・上書き保存();
		}
		private void toolStripMenuItem名前を付けて保存_Click( object sender, EventArgs e )
		{
			this.tシナリオ・名前をつけて保存();
		}
		private void toolStripMenuItem終了_Click( object sender, EventArgs e )
		{
			this.tシナリオ・終了();
		}
		private void toolStripMenuItem最近使ったファイル_Click( object sender, EventArgs e )
		{
			// ※このイベントハンドラに対応する「toolStripMenuItem最近使ったファイル」というアイテムはデザイナにはないので注意。
			//   this.t最近使ったファイルをFileメニューへ追加する() の中で、手動で作って追加したアイテムに対するハンドラである。

			if( this.t未保存なら保存する() == DialogResult.Cancel )
				return;

			this.t演奏ファイルを開いて読み込む( ( (ToolStripMenuItem) sender ).Text.Substring( 3 ) );
		}
		//-----------------
		#endregion
		#region [ GUIイベント：メニューバー [編集] ]
		//-----------------
		private void toolStripMenuItemアンドゥ_Click( object sender, EventArgs e )
		{
			this.tシナリオ・Undoする();
		}
		private void toolStripMenuItemリドゥ_Click( object sender, EventArgs e )
		{
			this.tシナリオ・Redoする();
		}
		private void toolStripMenuItem切り取り_Click( object sender, EventArgs e )
		{
			this.tシナリオ・切り取り();
		}
		private void toolStripMenuItemコピー_Click( object sender, EventArgs e )
		{
			this.tシナリオ・コピー();
		}
		private void toolStripMenuItem貼り付け_Click( object sender, EventArgs e )
		{
			// マウスが譜面上にあるならそこから貼り付ける。

			Point ptマウス位置 = this.pt現在のマウス位置を譜面の可視領域相対の座標dotで返す();
			Size sz譜面の可視サイズ = this.sz譜面の可視領域の大きさdotを返す();

	
			if( ( ( ptマウス位置.X < 0 ) || ( ptマウス位置.Y < 0 ) ) || ( ( ptマウス位置.X > sz譜面の可視サイズ.Width ) || ( ptマウス位置.Y > sz譜面の可視サイズ.Height ) ) )
			{
				// マウスが譜面上になかった → 表示領域下辺から貼り付ける

				this.tシナリオ・貼り付け( this.mgr譜面管理者.n現在の譜面表示下辺の譜面先頭からの位置grid );
			}
			else
			{
				// マウスが譜面上にあった

				this.tシナリオ・貼り付け( this.mgr譜面管理者.nY座標dotが位置するgridを返す・ガイド幅単位( ptマウス位置.Y ) );
			}
		}
		private void toolStripMenuItem削除_Click( object sender, EventArgs e )
		{
			this.tシナリオ・削除();
		}
		private void toolStripMenuItemすべて選択_Click( object sender, EventArgs e )
		{
			// 編集モードなら強制的に選択モードにする。

			if( this.b編集モードである )
				this.t選択モードにする();


			// 全チップを選択する。

			this.mgr選択モード管理者.t全チップを選択する();
		}
		private void toolStripMenuItem選択モード_Click( object sender, EventArgs e )
		{
			this.t選択モードにする();
		}
		private void toolStripMenuItem編集モード_Click( object sender, EventArgs e )
		{
			this.t編集モードにする();
		}
		private void toolStripMenuItemモード切替_Click( object sender, EventArgs e )
		{
			if( this.b選択モードである )
			{
				this.t編集モードにする();
			}
			else
			{
				this.t選択モードにする();
			}
		}
		private void toolStripMenuItem検索_Click( object sender, EventArgs e )
		{
			this.tシナリオ・検索();
		}
		private void toolStripMenuItem置換_Click( object sender, EventArgs e )
		{
			this.tシナリオ・置換();
		}
		//-----------------
		#endregion
		#region [ GUIイベント：メニューバー [表示] ]
		//-----------------
		private void toolStripMenuItemチップパレット_Click( object sender, EventArgs e )
		{
			if( this.toolStripMenuItemチップパレット.CheckState == CheckState.Checked )
			{
				this.dlgチップパレット.t表示する();
			}
			else
			{
				this.dlgチップパレット.t隠す();
			}
		}
		private void toolStripMenuItemガイド間隔4分_Click( object sender, EventArgs e )
		{
			this.tガイド間隔を変更する( 4 );
		}
		private void toolStripMenuItemガイド間隔8分_Click( object sender, EventArgs e )
		{
			this.tガイド間隔を変更する( 8 );
		}
		private void toolStripMenuItemガイド間隔12分_Click( object sender, EventArgs e )
		{
			this.tガイド間隔を変更する( 12 );
		}
		private void toolStripMenuItemガイド間隔16分_Click( object sender, EventArgs e )
		{
			this.tガイド間隔を変更する( 0x10 );
		}
		private void toolStripMenuItemガイド間隔24分_Click( object sender, EventArgs e )
		{
			this.tガイド間隔を変更する( 0x18 );
		}
		private void toolStripMenuItemガイド間隔32分_Click( object sender, EventArgs e )
		{
			this.tガイド間隔を変更する( 0x20 );
		}
		private void toolStripMenuItemガイド間隔48分_Click( object sender, EventArgs e )
		{
			this.tガイド間隔を変更する( 0x30 );
		}
		private void toolStripMenuItemガイド間隔64分_Click( object sender, EventArgs e )
		{
			this.tガイド間隔を変更する( 0x40 );
		}
		private void toolStripMenuItemガイド間隔フリー_Click( object sender, EventArgs e )
		{
			this.tガイド間隔を変更する( 0 );
		}
		private void toolStripMenuItemガイド間隔拡大_Click( object sender, EventArgs e )
		{
			switch( this.n現在のガイド間隔4to64or0 )
			{
				case 4: break;
				case 8: this.tガイド間隔を変更する( 4 ); break;
				case 12: this.tガイド間隔を変更する( 8 ); break;
				case 16: this.tガイド間隔を変更する( 12 ); break;
				case 24: this.tガイド間隔を変更する( 16 ); break;
				case 32: this.tガイド間隔を変更する( 24 ); break;
				case 48: this.tガイド間隔を変更する( 32 ); break;
				case 64: this.tガイド間隔を変更する( 48 ); break;
				case 0: this.tガイド間隔を変更する( 64 ); break;
			}
		}
		private void toolStripMenuItemガイド間隔縮小_Click( object sender, EventArgs e )
		{
			switch( this.n現在のガイド間隔4to64or0 )
			{
				case 4: this.tガイド間隔を変更する( 8 ); break;
				case 8: this.tガイド間隔を変更する( 12 ); break;
				case 12: this.tガイド間隔を変更する( 16 ); break;
				case 16: this.tガイド間隔を変更する( 24 ); break;
				case 24: this.tガイド間隔を変更する( 32 ); break;
				case 32: this.tガイド間隔を変更する( 48 ); break;
				case 48: this.tガイド間隔を変更する( 64 ); break;
				case 64: this.tガイド間隔を変更する( 0 ); break;
				case 0: break;
			}
		}
		//-----------------
		#endregion
		#region [ GUIイベント：メニューバー [再生] ]
		//-----------------
		private void toolStripMenuItem先頭から再生_Click( object sender, EventArgs e )
		{
			this.tシナリオ・Viewerで最初から再生する();
		}
		private void toolStripMenuItem現在位置から再生_Click( object sender, EventArgs e )
		{
			this.tシナリオ・Viewerで現在位置から再生する();
		}
		private void toolStripMenuItem現在位置からBGMのみ再生_Click( object sender, EventArgs e )
		{
			this.tシナリオ・Viewerで現在位置からBGMのみ再生する();
		}
		private void toolStripMenuItem再生停止_Click( object sender, EventArgs e )
		{
			this.tシナリオ・Viewerを再生停止する();
		}
		//-----------------
		#endregion
		#region [ GUIイベント：メニューバー [ツール] ]
		//-----------------
		private void toolStripMenuItemオプション_Click( object sender, EventArgs e )
		{
			this.mgrオプション管理者.tオプションダイアログを開いて編集し結果をアプリ設定に格納する();
			this.mgr譜面管理者.tRefreshDisplayLanes();	// レーンの表示/非表示切り替えに備えて追加
		}
		//-----------------
		#endregion
		#region [ GUIイベント：メニューバー [ヘルプ] ]
		//-----------------
		private void toolStripMenuItemDTXCreaterマニュアル_Click( object sender, EventArgs e )
		{
			try
			{
				// マニュアルを別プロセスとして開く。

				Process.Start( this.strDTXCのあるフォルダ名 + @"\Manual.chm" );
			}
			catch
			{
				this.toolStripMenuItemDTXCreaterマニュアル.Enabled = false;
			}
		}
		private void toolStripMenuItemバージョン_Click( object sender, EventArgs e )
		{
			this.dlgチップパレット.t一時的に隠蔽する();

			Cバージョン情報 cバージョン情報 = new Cバージョン情報();
			cバージョン情報.ShowDialog();
			cバージョン情報.Dispose();
			
			this.dlgチップパレット.t一時的な隠蔽を解除する();
		}
		//-----------------
		#endregion

		#region [ GUIイベント：ツールバー ]
		//-----------------
		private void toolStripButton新規作成_Click( object sender, EventArgs e )
		{
			this.tシナリオ・新規作成();
		}
		private void toolStripButton開く_Click( object sender, EventArgs e )
		{
			this.tシナリオ・開く();
		}
		private void toolStripButton上書き保存_Click( object sender, EventArgs e )
		{
			this.tシナリオ・上書き保存();
		}
		private void toolStripButton切り取り_Click( object sender, EventArgs e )
		{
			this.tシナリオ・切り取り();
		}
		private void toolStripButtonコピー_Click( object sender, EventArgs e )
		{
			this.tシナリオ・コピー();
		}
		private void toolStripButton貼り付け_Click( object sender, EventArgs e )
		{
			// マウスが譜面上にあるならそこから貼り付ける。

			Point ptマウスの位置 = this.pt現在のマウス位置を譜面の可視領域相対の座標dotで返す();
			Size sz譜面の可視サイズ = this.sz譜面の可視領域の大きさdotを返す();


			if( ( ( ptマウスの位置.X < 0 ) || ( ptマウスの位置.Y < 0 ) ) || ( ( ptマウスの位置.X > sz譜面の可視サイズ.Width ) || ( ptマウスの位置.Y > sz譜面の可視サイズ.Height ) ) )
			{
				// マウスが譜面上になかった → 表示領域下辺から貼り付ける

				this.tシナリオ・貼り付け( this.mgr譜面管理者.n現在の譜面表示下辺の譜面先頭からの位置grid );
			}
			else
			{
				// マウスが譜面上にあった

				this.tシナリオ・貼り付け( this.mgr譜面管理者.nY座標dotが位置するgridを返す・ガイド幅単位( ptマウスの位置.Y ) );
			}
		}
		private void toolStripButton削除_Click( object sender, EventArgs e )
		{
			this.tシナリオ・削除();
		}
		private void toolStripButtonアンドゥ_Click( object sender, EventArgs e )
		{
			this.tシナリオ・Undoする();
		}
		private void toolStripButtonリドゥ_Click( object sender, EventArgs e )
		{
			this.tシナリオ・Redoする();
		}
		private void toolStripButtonチップパレット_Click( object sender, EventArgs e )
		{
			if( this.toolStripButtonチップパレット.CheckState == CheckState.Checked )
			{
				this.dlgチップパレット.t表示する();
			}
			else
			{
				this.dlgチップパレット.t隠す();
			}
		}
		private void toolStripComboBox譜面拡大率_SelectedIndexChanged( object sender, EventArgs e )
		{
			C小節.n基準の高さdot = 192 * ( this.toolStripComboBox譜面拡大率.SelectedIndex + 1 );
			
			this.pictureBox譜面パネル.Refresh();
		}
		private void toolStripComboBoxガイド間隔_SelectedIndexChanged( object sender, EventArgs e )
		{
			switch( this.toolStripComboBoxガイド間隔.SelectedIndex )
			{
				case 0:
					this.tガイド間隔を変更する( 4 );
					return;

				case 1:
					this.tガイド間隔を変更する( 8 );
					return;

				case 2:
					this.tガイド間隔を変更する( 12 );
					return;

				case 3:
					this.tガイド間隔を変更する( 16 );
					return;

				case 4:
					this.tガイド間隔を変更する( 24 );
					return;

				case 5:
					this.tガイド間隔を変更する( 32 );
					return;

				case 6:
					this.tガイド間隔を変更する( 48 );
					return;

				case 7:
					this.tガイド間隔を変更する( 64 );
					return;

				case 8:
					this.tガイド間隔を変更する( 0 );
					return;
			}
		}
		private void toolStripButton選択モード_Click( object sender, EventArgs e )
		{
			this.t選択モードにする();
		}
		private void toolStripButton編集モード_Click( object sender, EventArgs e )
		{
			this.t編集モードにする();
		}
		private void toolStripButton先頭から再生_Click( object sender, EventArgs e )
		{
			this.tシナリオ・Viewerで最初から再生する();
		}
		private void toolStripButton現在位置から再生_Click( object sender, EventArgs e )
		{
			this.tシナリオ・Viewerで現在位置から再生する();
		}
		private void toolStripButton現在位置からBGMのみ再生_Click( object sender, EventArgs e )
		{
			this.tシナリオ・Viewerで現在位置からBGMのみ再生する();
		}
		private void toolStripButton再生停止_Click( object sender, EventArgs e )
		{
			this.tシナリオ・Viewerを再生停止する();
		}
		//-----------------
		#endregion

		//-----------------
		#endregion
	}
}
