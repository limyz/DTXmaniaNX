using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using FDK;

namespace DTXMania
{
	// グローバル定数

	public enum Eシステムサウンド
	{
		BGMオプション画面 = 0,
		BGMコンフィグ画面,
		BGM起動画面,
		BGM選曲画面,
        BGM結果画面,
		SOUNDステージ失敗音,
		SOUNDカーソル移動音,
		SOUNDゲーム開始音,
		SOUNDゲーム終了音,
		SOUNDステージクリア音,
		SOUNDタイトル音,
		SOUNDフルコンボ音,
		SOUND歓声音,
		SOUND曲読込開始音,
		SOUND決定音,
		SOUND取消音,
		SOUND変更音,
        SOUND曲決定,
        SOUNDエクセレント音,
        SOUND新記録音,
        SOUNDSELECTMUSIC,
        SOUNDNOVICE,
        SOUNDREGULAR,
        SOUNDEXPERT,
        SOUNDMASTER,
        SOUNDBASIC,
        SOUNDADVANCED,
        SOUNDEXTREME,
		Count				// システムサウンド総数の計算用
	}

	internal class CSkin : IDisposable
	{
		// クラス

		public class Cシステムサウンド : IDisposable
		{
			// static フィールド

			public static CSkin.Cシステムサウンド r最後に再生した排他システムサウンド;

			// フィールド、プロパティ

			public bool bCompact対象;
			public bool bループ;
			public bool b読み込み未試行;
			public bool b読み込み成功;
			public bool b排他;
			public string strファイル名 = "";
			public bool b再生中
			{
				get
				{
					if( this.rSound[ 1 - this.n次に鳴るサウンド番号 ] == null )
						return false;

					return this.rSound[ 1 - this.n次に鳴るサウンド番号 ].b再生中;
				}
			}
			public int n位置・現在のサウンド
			{
				get
				{
					CSound sound = this.rSound[ 1 - this.n次に鳴るサウンド番号 ];
					if( sound == null )
						return 0;

					return sound.n位置;
				}
				set
				{
					CSound sound = this.rSound[ 1 - this.n次に鳴るサウンド番号 ];
					if( sound != null )
						sound.n位置 = value;
				}
			}
			public int n位置・次に鳴るサウンド
			{
				get
				{
					CSound sound = this.rSound[ this.n次に鳴るサウンド番号 ];
					if( sound == null )
						return 0;

					return sound.n位置;
				}
				set
				{
					CSound sound = this.rSound[ this.n次に鳴るサウンド番号 ];
					if( sound != null )
						sound.n位置 = value;
				}
			}
			public int n音量・現在のサウンド
			{
				get
				{
					CSound sound = this.rSound[ 1 - this.n次に鳴るサウンド番号 ];
					if( sound == null )
						return 0;

					return sound.n音量;
				}
				set
				{
					CSound sound = this.rSound[ 1 - this.n次に鳴るサウンド番号 ];
					if( sound != null )
						sound.n音量 = value;
				}
			}
			public int n音量・次に鳴るサウンド
			{
				get
				{
					CSound sound = this.rSound[ this.n次に鳴るサウンド番号 ];
					if( sound == null )
					{
						return 0;
					}
					return sound.n音量;
				}
				set
				{
					CSound sound = this.rSound[ this.n次に鳴るサウンド番号 ];
					if( sound != null )
					{
						sound.n音量 = value;
					}
				}
			}
			public int n長さ・現在のサウンド
			{
				get
				{
					CSound sound = this.rSound[ 1 - this.n次に鳴るサウンド番号 ];
					if( sound == null )
					{
						return 0;
					}
					return sound.n総演奏時間ms;
				}
			}
			public int n長さ・次に鳴るサウンド
			{
				get
				{
					CSound sound = this.rSound[ this.n次に鳴るサウンド番号 ];
					if( sound == null )
					{
						return 0;
					}
					return sound.n総演奏時間ms;
				}
			}


			/// <summary>
			/// コンストラクタ
			/// </summary>
			/// <param name="strファイル名"></param>
			/// <param name="bループ"></param>
			/// <param name="b排他"></param>
			/// <param name="bCompact対象"></param>
			public Cシステムサウンド(string strファイル名, bool bループ, bool b排他, bool bCompact対象)
			{
				this.strファイル名 = strファイル名;
				this.bループ = bループ;
				this.b排他 = b排他;
				this.bCompact対象 = bCompact対象;
				this.b読み込み未試行 = true;
			}
			public Cシステムサウンド()
			{
				this.b読み込み未試行 = true;
			}
			

			// メソッド

			public void t読み込み()
			{
				this.b読み込み未試行 = false;
				this.b読み込み成功 = false;
				if( string.IsNullOrEmpty( this.strファイル名 ) )
					throw new InvalidOperationException( "ファイル名が無効です。" );

				if( !File.Exists( CSkin.Path( this.strファイル名 ) ) )
				{
					throw new FileNotFoundException( this.strファイル名 );
				}
////				for( int i = 0; i < 2; i++ )		// #27790 2012.3.10 yyagi 2回読み出しを、1回読みだし＋1回メモリコピーに変更
////				{
//                    try
//                    {
//                        this.rSound[ 0 ] = CDTXMania.Sound管理.tサウンドを生成する( CSkin.Path( this.strファイル名 ) );
//                    }
//                    catch
//                    {
//                        this.rSound[ 0 ] = null;
//                        throw;
//                    }
//                    if ( this.rSound[ 0 ] == null )	// #28243 2012.5.3 yyagi "this.rSound[ 0 ].bストリーム再生する"時もCloneするようにし、rSound[1]がnullにならないよう修正→rSound[1]の再生正常化
//                    {
//                        this.rSound[ 1 ] = null;
//                    }
//                    else
//                    {
//                        this.rSound[ 1 ] = ( CSound ) this.rSound[ 0 ].Clone();	// #27790 2012.3.10 yyagi add: to accelerate loading chip sounds
//                        CDTXMania.Sound管理.tサウンドを登録する( this.rSound[ 1 ] );	// #28243 2012.5.3 yyagi add (登録漏れによりストリーム再生処理が発生していなかった)
//                    }

////				}

				for ( int i = 0; i < 2; i++ )		// 一旦Cloneを止めてASIO対応に専念
				{
					try
					{
						this.rSound[ i ] = CDTXMania.Sound管理.tサウンドを生成する( CSkin.Path( this.strファイル名 ) );
					}
					catch
					{
						this.rSound[ i ] = null;
						throw;
					}
				}
				this.b読み込み成功 = true;
			}
			public void t再生する()
			{
				if ( this.b読み込み未試行 )
				{
					try
					{
						t読み込み();
					}
					catch
					{
						this.b読み込み未試行 = false;
					}
				}
				if( this.b排他 )
				{
					if( r最後に再生した排他システムサウンド != null )
						r最後に再生した排他システムサウンド.t停止する();

					r最後に再生した排他システムサウンド = this;
				}
				CSound sound = this.rSound[ this.n次に鳴るサウンド番号 ];
				if( sound != null )
					sound.t再生を開始する( this.bループ );

				this.n次に鳴るサウンド番号 = 1 - this.n次に鳴るサウンド番号;
			}
			public void t停止する()
			{
				if( this.rSound[ 0 ] != null )
					this.rSound[ 0 ].t再生を停止する();

				if( this.rSound[ 1 ] != null )
					this.rSound[ 1 ].t再生を停止する();

				if( r最後に再生した排他システムサウンド == this )
					r最後に再生した排他システムサウンド = null;
			}

			public void tRemoveMixer()
			{
				if ( CDTXMania.Sound管理.GetCurrentSoundDeviceType() != "DirectShow" )
				{
					for ( int i = 0; i < 2; i++ )
					{
						if ( this.rSound[ i ] != null )
						{
							CDTXMania.Sound管理.RemoveMixer( this.rSound[ i ] );
						}
					}
				}
			}

			#region [ IDisposable 実装 ]
			//-----------------
			public void Dispose()
			{
				if( !this.bDisposed済み )
				{
					for( int i = 0; i < 2; i++ )
					{
						if( this.rSound[ i ] != null )
						{
							CDTXMania.Sound管理.tサウンドを破棄する( this.rSound[ i ] );
							this.rSound[ i ] = null;
						}
					}
					this.b読み込み成功 = false;
					this.bDisposed済み = true;
				}
			}
			//-----------------
			#endregion

			#region [ private ]
			//-----------------
			private bool bDisposed済み;
			private int n次に鳴るサウンド番号;
			private CSound[] rSound = new CSound[ 2 ];
			//-----------------
			#endregion
		}

	
		// プロパティ

		public Cシステムサウンド bgmオプション画面 = null;
		public Cシステムサウンド bgmコンフィグ画面 = null;
		public Cシステムサウンド bgm起動画面 = null;
		public Cシステムサウンド bgm選曲画面 = null;
        public Cシステムサウンド bgm結果画面 = null;
		public Cシステムサウンド soundSTAGEFAILED音 = null;
		public Cシステムサウンド soundカーソル移動音 = null;
		public Cシステムサウンド soundゲーム開始音 = null;
		public Cシステムサウンド soundゲーム終了音 = null;
		public Cシステムサウンド soundステージクリア音 = null;
		public Cシステムサウンド soundタイトル音 = null;
		public Cシステムサウンド soundフルコンボ音 = null;
		public Cシステムサウンド sound歓声音 = null;
		public Cシステムサウンド sound曲読込開始音 = null;
		public Cシステムサウンド sound決定音 = null;
		public Cシステムサウンド sound取消音 = null;
		public Cシステムサウンド sound変更音 = null;
        public Cシステムサウンド sound曲決定 = null;
        public Cシステムサウンド soundエクセレント音 = null;
        public Cシステムサウンド sound新記録音 = null;
        public Cシステムサウンド soundSelectMusic = null;
        public Cシステムサウンド soundNovice = null;
        public Cシステムサウンド soundRegular = null;
        public Cシステムサウンド soundExpert = null;
        public Cシステムサウンド soundMaster = null;
        public Cシステムサウンド soundBasic = null;
        public Cシステムサウンド soundAdvanced = null;
        public Cシステムサウンド soundExtreme = null;
		public readonly int nシステムサウンド数 = (int)Eシステムサウンド.Count;
		public Cシステムサウンド this[ Eシステムサウンド sound ]
		{
			get
			{
				switch( sound )
				{
					case Eシステムサウンド.SOUNDカーソル移動音:
						return this.soundカーソル移動音;

					case Eシステムサウンド.SOUND決定音:
						return this.sound決定音;

					case Eシステムサウンド.SOUND変更音:
						return this.sound変更音;

					case Eシステムサウンド.SOUND取消音:
						return this.sound取消音;

					case Eシステムサウンド.SOUND歓声音:
						return this.sound歓声音;

					case Eシステムサウンド.SOUNDステージ失敗音:
						return this.soundSTAGEFAILED音;

					case Eシステムサウンド.SOUNDゲーム開始音:
						return this.soundゲーム開始音;

					case Eシステムサウンド.SOUNDゲーム終了音:
						return this.soundゲーム終了音;

					case Eシステムサウンド.SOUNDステージクリア音:
						return this.soundステージクリア音;

					case Eシステムサウンド.SOUNDフルコンボ音:
						return this.soundフルコンボ音;

                    case Eシステムサウンド.SOUNDエクセレント音:
                        return this.soundエクセレント音;

                    case Eシステムサウンド.SOUND新記録音:
                        return this.sound新記録音;

					case Eシステムサウンド.SOUND曲読込開始音:
						return this.sound曲読込開始音;

					case Eシステムサウンド.SOUNDタイトル音:
						return this.soundタイトル音;

                    case Eシステムサウンド.SOUND曲決定:
                        return this.sound曲決定;

                    case Eシステムサウンド.SOUNDNOVICE:
                        return this.soundNovice;

                    case Eシステムサウンド.SOUNDREGULAR:
                        return this.soundRegular;

                    case Eシステムサウンド.SOUNDEXPERT:
                        return this.soundExpert;

                    case Eシステムサウンド.SOUNDMASTER:
                        return this.soundMaster;

                    case Eシステムサウンド.SOUNDBASIC:
                        return this.soundBasic;

                    case Eシステムサウンド.SOUNDADVANCED:
                        return this.soundAdvanced;

                    case Eシステムサウンド.SOUNDEXTREME:
                        return this.soundExtreme;

                    case Eシステムサウンド.SOUNDSELECTMUSIC:
                        return this.soundSelectMusic;

					case Eシステムサウンド.BGM起動画面:
						return this.bgm起動画面;

					case Eシステムサウンド.BGMオプション画面:
						return this.bgmオプション画面;

					case Eシステムサウンド.BGMコンフィグ画面:
						return this.bgmコンフィグ画面;

					case Eシステムサウンド.BGM選曲画面:
						return this.bgm選曲画面;

                    case Eシステムサウンド.BGM結果画面:
						return this.bgm結果画面;
				}
				throw new IndexOutOfRangeException();
			}
		}
		public Cシステムサウンド this[ int index ]
		{
			get
			{
				switch( index )
				{
					case 0:
						return this.soundカーソル移動音;

					case 1:
						return this.sound決定音;

					case 2:
						return this.sound変更音;

					case 3:
						return this.sound取消音;

					case 4:
						return this.sound歓声音;

					case 5:
						return this.soundSTAGEFAILED音;

					case 6:
						return this.soundゲーム開始音;

					case 7:
						return this.soundゲーム終了音;

					case 8:
						return this.soundフルコンボ音;

                    case 9:
                        return this.soundエクセレント音;

                    case 10:
                        return this.sound新記録音;

					case 11:
						return this.sound曲読込開始音;

					case 12:
						return this.soundタイトル音;

                    case 13:
                        return this.sound曲決定;

					case 14:
						return this.bgm起動画面;

					case 15:
						return this.bgmオプション画面;

					case 16:
						return this.bgmコンフィグ画面;

                    case 17:
                        return this.bgm選曲画面;

                    case 18:
                        return this.bgm結果画面;

                    case 19:
                        return this.soundステージクリア音;

                    case 20:
                        return this.soundNovice;

                    case 21:
                        return this.soundRegular;

                    case 22:
                        return this.soundExpert;

                    case 23:
                        return this.soundMaster;

                    case 24:
                        return this.soundSelectMusic;
                    
                    case 25:
                        return this.soundBasic;

                    case 26:
                        return this.soundAdvanced;

                    case 27:
                        return this.soundExtreme;
				}
				throw new IndexOutOfRangeException();
			}
		}


		// スキンの切り替えについて・・・
		//
		// ・スキンの種類は大きく分けて2種類。Systemスキンとboxdefスキン。
		// 　前者はSystem/フォルダにユーザーが自らインストールしておくスキン。
		// 　後者はbox.defで指定する、曲データ制作者が提示するスキン。
		//
		// ・Config画面で、2種のスキンを区別無く常時使用するよう設定することができる。
		// ・box.defの#SKINPATH 設定により、boxdefスキンを一時的に使用するよう設定する。
		// 　(box.defの効果の及ばない他のmuxic boxでは、当該boxdefスキンの有効性が無くなる)
		//
		// これを実現するために・・・
		// ・Systemスキンの設定情報と、boxdefスキンの設定情報は、分離して持つ。
		// 　(strSystem～～ と、strBoxDef～～～)
		// ・Config画面からは前者のみ書き換えできるようにし、
		// 　選曲画面からは後者のみ書き換えできるようにする。(SetCurrent...())
		// ・読み出しは両者から行えるようにすると共に
		// 　選曲画面用に二種の情報を区別しない読み出し方法も提供する(GetCurrent...)

		private object lockBoxDefSkin;
		public static bool bUseBoxDefSkin = true;						// box.defからのスキン変更を許容するか否か

		public string strSystemSkinRoot = null;
		public string[] strSystemSkinSubfolders = null;		// List<string>だとignoreCaseな検索が面倒なので、配列に逃げる :-)
		private string[] _strBoxDefSkinSubfolders = null;
		public string[] strBoxDefSkinSubfolders
		{
			get
			{
				lock ( lockBoxDefSkin )
				{
					return _strBoxDefSkinSubfolders;
				}
			}
			set
			{
				lock ( lockBoxDefSkin )
				{
					_strBoxDefSkinSubfolders = value;
				}
			}
		}			// 別スレッドからも書き込みアクセスされるため、スレッドセーフなアクセス法を提供

		private static string strSystemSkinSubfolderFullName;			// Config画面で設定されたスキン
		private static string strBoxDefSkinSubfolderFullName = "";		// box.defで指定されているスキン

		/// <summary>
		/// スキンパス名をフルパスで取得する
		/// </summary>
		/// <param name="bFromUserConfig">ユーザー設定用ならtrue, box.defからの設定ならfalse</param>
		/// <returns></returns>
		public string GetCurrentSkinSubfolderFullName( bool bFromUserConfig )
		{
			if ( !bUseBoxDefSkin || bFromUserConfig == true || strBoxDefSkinSubfolderFullName == "" )
			{
				return strSystemSkinSubfolderFullName;
			}
			else
			{
				return strBoxDefSkinSubfolderFullName;
			}
		}
		/// <summary>
		/// スキンパス名をフルパスで設定する
		/// </summary>
		/// <param name="value">スキンパス名</param>
		/// <param name="bFromUserConfig">ユーザー設定用ならtrue, box.defからの設定ならfalse</param>
		public void SetCurrentSkinSubfolderFullName( string value, bool bFromUserConfig )
		{
			if ( bFromUserConfig )
			{
				strSystemSkinSubfolderFullName = value;
			}
			else
			{
				strBoxDefSkinSubfolderFullName = value;
			}
		}


		// コンストラクタ
		public CSkin( string _strSkinSubfolderFullName, bool _bUseBoxDefSkin )
		{
			lockBoxDefSkin = new object();
			strSystemSkinSubfolderFullName = _strSkinSubfolderFullName;
			bUseBoxDefSkin = _bUseBoxDefSkin;
			InitializeSkinPathRoot();
			ReloadSkinPaths();
			PrepareReloadSkin();
		}
		public CSkin()
		{
			lockBoxDefSkin = new object();
			InitializeSkinPathRoot();
			bUseBoxDefSkin = true;
			ReloadSkinPaths();
			PrepareReloadSkin();
		}
		private string InitializeSkinPathRoot()
		{
			strSystemSkinRoot = System.IO.Path.Combine( CDTXMania.strEXEのあるフォルダ, "System" + System.IO.Path.DirectorySeparatorChar );
			return strSystemSkinRoot;
		}

		/// <summary>
		/// Skin(Sounds)を再読込する準備をする(再生停止,Dispose,ファイル名再設定)。
		/// あらかじめstrSkinSubfolderを適切に設定しておくこと。
		/// その後、ReloadSkinPaths()を実行し、strSkinSubfolderの正当性を確認した上で、本メソッドを呼び出すこと。
		/// 本メソッド呼び出し後に、ReloadSkin()を実行することで、システムサウンドを読み込み直す。
		/// ReloadSkin()の内容は本メソッド内に含めないこと。起動時はReloadSkin()相当の処理をCEnumSongsで行っているため。
		/// </summary>
		public void PrepareReloadSkin()
		{
			Trace.TraceInformation( "SkinPath設定: {0}",
				( strBoxDefSkinSubfolderFullName == "" ) ?
				strSystemSkinSubfolderFullName :
				strBoxDefSkinSubfolderFullName
			);

			for ( int i = 0; i < nシステムサウンド数; i++ )
			{
				if ( this[ i ] != null && this[i].b読み込み成功 )
				{
					this[ i ].t停止する();
					this[ i ].Dispose();
				}
			}
			this.soundカーソル移動音	= new Cシステムサウンド( @"Sounds\Move.ogg",			false, false, false );
			this.sound決定音			= new Cシステムサウンド( @"Sounds\Decide.ogg",			false, false, false );
			this.sound変更音			= new Cシステムサウンド( @"Sounds\Change.ogg",			false, false, false );
			this.sound取消音			= new Cシステムサウンド( @"Sounds\Cancel.ogg",			false, false, true  );
			this.sound歓声音			= new Cシステムサウンド( @"Sounds\Audience.ogg",		false, false,  true  );
			this.soundSTAGEFAILED音		= new Cシステムサウンド( @"Sounds\Stage failed.ogg",	false, true,  true  );
			this.soundゲーム開始音		= new Cシステムサウンド( @"Sounds\Game start.ogg",		false, false, false );
			this.soundゲーム終了音		= new Cシステムサウンド( @"Sounds\Game end.ogg",		false, true,  false );
			this.soundステージクリア音	= new Cシステムサウンド( @"Sounds\Stage clear.ogg",		false, true,  false );
			this.soundフルコンボ音		= new Cシステムサウンド( @"Sounds\Full combo.ogg",		false, false, true  );
            this.sound新記録音          = new Cシステムサウンド( @"Sounds\New Record.ogg",      false, false, true  );
            this.soundエクセレント音    = new Cシステムサウンド( @"Sounds\Excellent.ogg",       false, false, true  );
			this.sound曲読込開始音		= new Cシステムサウンド( @"Sounds\Now loading.ogg",		false, true,  true  );
			this.soundタイトル音		= new Cシステムサウンド( @"Sounds\Title.ogg",			false, true,  false );
            this.sound曲決定            = new Cシステムサウンド( @"Sounds\MusicDecide.ogg",     false, false, false );
            this.soundNovice            = new Cシステムサウンド( @"Sounds\Novice.ogg",          false, false, false );
            this.soundRegular           = new Cシステムサウンド( @"Sounds\Regular.ogg",         false, false, false );
			this.soundExpert		    = new Cシステムサウンド( @"Sounds\Expert.ogg",		    false, false, false );
            this.soundBasic             = new Cシステムサウンド( @"Sounds\Basic.ogg",           false, false, false );
            this.soundAdvanced          = new Cシステムサウンド( @"Sounds\Advanced.ogg",        false, false, false );
			this.soundExtreme	        = new Cシステムサウンド( @"Sounds\Extreme.ogg",		    false, false, false );
			this.soundMaster		    = new Cシステムサウンド( @"Sounds\Master.ogg",			false, false, false );
            this.soundSelectMusic       = new Cシステムサウンド( @"Sounds\SelectMusic.ogg",     false, false, false );
			this.bgm起動画面			= new Cシステムサウンド( @"Sounds\Setup BGM.ogg",		true,  true,  false );
			this.bgmオプション画面		= new Cシステムサウンド( @"Sounds\Option BGM.ogg",		true,  true,  false );
			this.bgmコンフィグ画面		= new Cシステムサウンド( @"Sounds\Config BGM.ogg",		true,  true,  false );
			this.bgm選曲画面			= new Cシステムサウンド( @"Sounds\Select BGM.ogg",		true,  true,  false );
            this.bgm結果画面            = new Cシステムサウンド( @"Sounds\Result BGM.ogg",      true,  true,  false);
		}

		public void ReloadSkin()
		{
			for ( int i = 0; i < nシステムサウンド数; i++ )
			{
				if ( !this[ i ].b排他 )	// BGM系以外のみ読み込む。(BGM系は必要になったときに読み込む)
				{
					Cシステムサウンド cシステムサウンド = this[ i ];
					if ( !CDTXMania.bコンパクトモード || cシステムサウンド.bCompact対象 )
					{
						try
						{
							cシステムサウンド.t読み込み();
							Trace.TraceInformation( "システムサウンドを読み込みました。({0})", cシステムサウンド.strファイル名 );
						}
						catch ( FileNotFoundException )
						{
							Trace.TraceWarning( "システムサウンドが存在しません。({0})", cシステムサウンド.strファイル名 );
						}
						catch ( Exception e )
						{
							Trace.TraceError( e.Message );
							Trace.TraceWarning( "システムサウンドの読み込みに失敗しました。({0})", cシステムサウンド.strファイル名 );
						}
					}
				}
			}
		}


		/// <summary>
		/// Skinの一覧を再取得する。
		/// System/*****/Graphics (やSounds/) というフォルダ構成を想定している。
		/// もし再取得の結果、現在使用中のSkinのパス(strSystemSkinSubfloderFullName)が消えていた場合は、
		/// 以下の優先順位で存在確認の上strSystemSkinSubfolderFullNameを再設定する。
		/// 1. System/Default/
		/// 2. System/*****/ で最初にenumerateされたもの
 		/// 3. System/ (従来互換)
		/// </summary>
		public void ReloadSkinPaths()
		{
			#region [ まず System/*** をenumerateする ]
			string[] tempSkinSubfolders = System.IO.Directory.GetDirectories( strSystemSkinRoot, "*" );
			strSystemSkinSubfolders = new string[ tempSkinSubfolders.Length ];
			int size = 0;
			for ( int i = 0; i < tempSkinSubfolders.Length; i++ )
			{
				#region [ 検出したフォルダがスキンフォルダかどうか確認する]
				if ( !bIsValid( tempSkinSubfolders[ i ] ) )
					continue;
				#endregion
				#region [ スキンフォルダと確認できたものを、strSkinSubfoldersに入れる ]
				// フォルダ名末尾に必ず\をつけておくこと。さもないとConfig読み出し側(必ず\をつける)とマッチできない
				if ( tempSkinSubfolders[ i ][ tempSkinSubfolders[ i ].Length - 1 ] != System.IO.Path.DirectorySeparatorChar )
				{
					tempSkinSubfolders[ i ] += System.IO.Path.DirectorySeparatorChar;
				}
				strSystemSkinSubfolders[ size ] = tempSkinSubfolders[ i ];
				Trace.TraceInformation( "SkinPath検出: {0}", strSystemSkinSubfolders[ size ] );
				size++;
				#endregion
			}
			Trace.TraceInformation( "SkinPath入力: {0}", strSystemSkinSubfolderFullName );
			Array.Resize( ref strSystemSkinSubfolders, size );
			Array.Sort( strSystemSkinSubfolders );	// BinarySearch実行前にSortが必要
			#endregion

			#region [ 現在のSkinパスがbox.defスキンをCONFIG指定していた場合のために、最初にこれが有効かチェックする。有効ならこれを使う。 ]
			if ( bIsValid( strSystemSkinSubfolderFullName ) &&
				Array.BinarySearch( strSystemSkinSubfolders, strSystemSkinSubfolderFullName,
				StringComparer.InvariantCultureIgnoreCase ) < 0 )
			{
				strBoxDefSkinSubfolders = new string[ 1 ]{ strSystemSkinSubfolderFullName };
				return;
			}
			#endregion

			#region [ 次に、現在のSkinパスが存在するか調べる。あれば終了。]
			if ( Array.BinarySearch( strSystemSkinSubfolders, strSystemSkinSubfolderFullName,
				StringComparer.InvariantCultureIgnoreCase ) >= 0 )
				return;
			#endregion
			#region [ カレントのSkinパスが消滅しているので、以下で再設定する。]
			/// 以下の優先順位で現在使用中のSkinパスを再設定する。
			/// 1. System/Default/
			/// 2. System/*****/ で最初にenumerateされたもの
			/// 3. System/ (従来互換)
			#region [ System/Default/ があるなら、そこにカレントSkinパスを設定する]
			string tempSkinPath_default = System.IO.Path.Combine( strSystemSkinRoot, "Default" + System.IO.Path.DirectorySeparatorChar );
			if ( Array.BinarySearch( strSystemSkinSubfolders, tempSkinPath_default, 
				StringComparer.InvariantCultureIgnoreCase ) >= 0 )
			{
				strSystemSkinSubfolderFullName = tempSkinPath_default;
				return;
			}
			#endregion
			#region [ System/SkinFiles.*****/ で最初にenumerateされたものを、カレントSkinパスに再設定する ]
			if ( strSystemSkinSubfolders.Length > 0 )
			{
				strSystemSkinSubfolderFullName = strSystemSkinSubfolders[ 0 ];
				return;
			}
			#endregion
			#region [ System/ に、カレントSkinパスを再設定する。]
			strSystemSkinSubfolderFullName = strSystemSkinRoot;
			strSystemSkinSubfolders = new string[ 1 ]{ strSystemSkinSubfolderFullName };
			#endregion
			#endregion
		}

		// メソッド

		public static string Path( string strファイルの相対パス )
		{
			if ( strBoxDefSkinSubfolderFullName == "" || !bUseBoxDefSkin )
			{
				return System.IO.Path.Combine( strSystemSkinSubfolderFullName, strファイルの相対パス );
			}
			else
			{
				return System.IO.Path.Combine( strBoxDefSkinSubfolderFullName, strファイルの相対パス );
			}
		}

		/// <summary>
		/// フルパス名を与えると、スキン名として、ディレクトリ名末尾の要素を返す
		/// 例: C:\foo\bar\ なら、barを返す
		/// </summary>
		/// <param name="skinpath">スキンが格納されたパス名(フルパス)</param>
		/// <returns>スキン名</returns>
		public static string GetSkinName( string skinPathFullName )
		{
			if ( skinPathFullName != null )
			{
				if ( skinPathFullName == "" )		// 「box.defで未定義」用
					skinPathFullName = strSystemSkinSubfolderFullName;
				string[] tmp = skinPathFullName.Split( System.IO.Path.DirectorySeparatorChar );
				return tmp[ tmp.Length - 2 ];		// ディレクトリ名の最後から2番目の要素がスキン名(最後の要素はnull。元stringの末尾が\なので。)
			}
			return null;
		}
		public static string[] GetSkinName( string[] skinPathFullNames )
		{
			string[] ret = new string[ skinPathFullNames.Length ];
			for ( int i = 0; i < skinPathFullNames.Length; i++ )
			{
				ret[ i ] = GetSkinName( skinPathFullNames[ i ] );
			}
			return ret;
		}


		public string GetSkinSubfolderFullNameFromSkinName( string skinName )
		{
			foreach ( string s in strSystemSkinSubfolders )
			{
				if ( GetSkinName( s ) == skinName )
					return s;
			}
			foreach ( string b in strBoxDefSkinSubfolders )
			{
				if ( GetSkinName( b ) == skinName )
					return b;
			}
			return null;
		}

		/// <summary>
		/// スキンパス名が妥当かどうか
		/// (タイトル画像にアクセスできるかどうかで判定する)
		/// </summary>
		/// <param name="skinPathFullName">妥当性を確認するスキンパス(フルパス)</param>
		/// <returns>妥当ならtrue</returns>
		public bool bIsValid( string skinPathFullName )
		{
			string filePathTitle;
			filePathTitle = System.IO.Path.Combine( skinPathFullName, @"Graphics\1_background.jpg" );
			return ( File.Exists( filePathTitle ) );
		}


        public void tRemoveMixerAll()
        {
            for (int i = 0; i < nシステムサウンド数; i++)
            {
                if (this[i] != null && this[i].b読み込み成功)
                {
                    this[i].t停止する();
                    this[i].tRemoveMixer();
                }
            }

        }

        public void tReadSkinConfig()
        {
            if( File.Exists( CSkin.Path( @"SkinConfig.ini" ) ) )
            {
                string str;
				//this.tキーアサインを全部クリアする();
				using ( StreamReader reader = new StreamReader( CSkin.Path( @"SkinConfig.ini" ), Encoding.GetEncoding( "unicode" ) ) )
                {
				    str = reader.ReadToEnd();
                }
                this.t文字列から読み込み( str );
            }
        }

        /// <summary>
        /// 2016.07.30 kairera0467 #36413
        /// </summary>
        public void tSaveSkinConfig()
        {
            if( File.Exists( CSkin.Path( @"SkinConfig.ini" ) ) )
            {
                StreamWriter sw = new StreamWriter( CSkin.Path( @"SkinConfig.ini" ), false, Encoding.GetEncoding( "unicode" ) );
                sw.WriteLine( "; スキンごとでの設定ファイル。現在テスト段階です。" );
                sw.WriteLine( "; ここで設定した数値が優先的に使用されます。" );
                sw.WriteLine( ";" );
                sw.WriteLine( "; 読み込み画面、演奏画面、ネームプレート、リザルト画面の曲名で使用するフォント名" );
                sw.WriteLine( "DisplayFontName={0}", CDTXMania.ConfigIni.str曲名表示フォント );
                sw.WriteLine();
                sw.WriteLine( "; 選曲リストのフォント名" );
                sw.WriteLine( "; Font name for select song item." );
                sw.WriteLine( "SelectListFontName={0}", CDTXMania.ConfigIni.str選曲リストフォント );
                sw.WriteLine();
                sw.WriteLine( "; 選曲リストのフォントのサイズ[dot]" );
                sw.WriteLine( "; Font size[dot] for select song item." );
                sw.WriteLine( "SelectListFontSize={0}", CDTXMania.ConfigIni.n選曲リストフォントのサイズdot );
                sw.WriteLine();
                sw.WriteLine( "; ネームプレートタイプ" );
                sw.WriteLine( "; 0:タイプA XG2風の表示がされます。" );
                sw.WriteLine( "; 1:タイプB XG風の表示がされます。このタイプでは7_NamePlate_XG.png、7_Difficlty_XG.pngが読み込まれます。" );
                sw.WriteLine( "NamePlateType={0}", (int)CDTXMania.ConfigIni.eNamePlate );
                sw.WriteLine();
                sw.WriteLine( "; 動くドラムセット(0:ON, 1:OFF, 2:NONE)" );
                sw.WriteLine( "DrumSetMoves={0}", (int)CDTXMania.ConfigIni.eドラムセットを動かす );
                sw.WriteLine();
                sw.WriteLine( "; BPMバーの表示(0:表示する, 1:左のみ表示, 2:動くバーを表示しない, 3:表示しない)" );
                sw.WriteLine( "BPMBar={0}", (int)CDTXMania.ConfigIni.eBPMbar );
                sw.WriteLine();
                sw.WriteLine( "; LivePointの表示(0:OFF, 1:ON)" );
                sw.WriteLine( "LivePoint={0}", CDTXMania.ConfigIni.bLivePoint ? 1 : 0 );
                sw.WriteLine();
                sw.WriteLine( "; スピーカーの表示(0:OFF, 1:ON)" );
                sw.WriteLine( "Speaker={0}", CDTXMania.ConfigIni.bSpeaker ? 1 : 0 );
                sw.WriteLine();
                sw.WriteLine( ";判定画像のアニメーション方式" );
                sw.WriteLine( ";(0:旧DTXMania方式 1:コマ方式 2:擬似XG方式)" );
                sw.WriteLine( "JudgeAnimeType={0}", CDTXMania.ConfigIni.nJudgeAnimeType );
                sw.WriteLine();
                sw.WriteLine( ";判定画像のコマ数" );
                sw.WriteLine( "JudgeFrames={0}", CDTXMania.ConfigIni.nJudgeFrames );
                sw.WriteLine();
                sw.WriteLine( ";判定画像の1コマのフレーム数" );
                sw.WriteLine( "JudgeInterval={0}", CDTXMania.ConfigIni.nJudgeInterval );
                sw.WriteLine();
                sw.WriteLine( ";判定画像の1コマの幅" );
                sw.WriteLine( "JudgeWidgh={0}", CDTXMania.ConfigIni.nJudgeWidgh );
                sw.WriteLine();
                sw.WriteLine( ";判定画像の1コマの高さ" );
                sw.WriteLine( "JudgeHeight={0}", CDTXMania.ConfigIni.nJudgeHeight );
                sw.WriteLine();
                sw.WriteLine( ";アタックエフェクトのコマ数" );
                sw.WriteLine( "ExplosionFrames={0}", CDTXMania.ConfigIni.nExplosionFrames );
                sw.WriteLine();
                sw.WriteLine( ";アタックエフェクトの1コマのフレーム数" );
                sw.WriteLine( "ExplosionInterval={0}", CDTXMania.ConfigIni.nExplosionInterval );
                sw.WriteLine();
                sw.WriteLine( ";アタックエフェクトの1コマの幅" );
                sw.WriteLine( "ExplosionWidgh={0}", CDTXMania.ConfigIni.nExplosionWidgh );
                sw.WriteLine();
                sw.WriteLine( ";アタックエフェクトの1コマの高さ" );
                sw.WriteLine( "ExplosionHeight={0}", CDTXMania.ConfigIni.nExplosionHeight );
                sw.WriteLine();
                sw.WriteLine( "ワイリングエフェクトのコマ数;" );
                sw.WriteLine( "WailingFireFrames={0}", CDTXMania.ConfigIni.nWailingFireFrames );
                sw.WriteLine();
                sw.WriteLine( ";ワイリングエフェクトの1コマのフレーム数" );
                sw.WriteLine( "WailingFireInterval={0}", CDTXMania.ConfigIni.nWailingFireInterval );
                sw.WriteLine();
                sw.WriteLine( ";ワイリングエフェクトの1コマの幅" );
                sw.WriteLine( "WailingFireWidgh={0}", CDTXMania.ConfigIni.nWailingFireWidgh );
                sw.WriteLine();
                sw.WriteLine( ";ワイリングエフェクトの1コマの高さ" );
                sw.WriteLine( "WailingFireHeight={0}", CDTXMania.ConfigIni.nWailingFireHeight );
                sw.WriteLine();
                sw.WriteLine( ";ワイリングエフェクトのX座標" );
                sw.WriteLine( "WailingFirePosXGuitar={0}", CDTXMania.ConfigIni.nWailingFireX.Guitar );
                sw.WriteLine( "WailingFirePosXBass={0}", CDTXMania.ConfigIni.nWailingFireX.Bass );
                sw.WriteLine();
                sw.WriteLine( ";ワイリングエフェクトのY座標(Guitar、Bass共通)" );
                sw.WriteLine( "WailingFirePosY={0}", CDTXMania.ConfigIni.nWailingFireY );
                sw.WriteLine();

                sw.Close();
            }
        }

        private void t文字列から読み込み(string strAllSettings)	// 2011.4.13 yyagi; refactored to make initial KeyConfig easier.
        {
            string[] delimiter = { "\n" };
            string[] strSingleLine = strAllSettings.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in strSingleLine)
            {
                string str = s.Replace('\t', ' ').TrimStart(new char[] { '\t', ' ' });
                if ((str.Length != 0) && (str[0] != ';'))
                {
                    try
                    {
                        string str3;
                        string str4;
                        string[] strArray = str.Split(new char[] { '=' });
                        if (strArray.Length == 2)
                        {
                            str3 = strArray[0].Trim();
                            str4 = strArray[1].Trim();
                            //-----------------------------
                            if (str3.Equals("SelectListFontName"))
                            {
                                CDTXMania.ConfigIni.str選曲リストフォント = str4;
                            }
                            else if (str3.Equals("DisplayFontName"))
                            {
                                CDTXMania.ConfigIni.str曲名表示フォント = str4;
                            }
                            else if (str3.Equals("SelectListFontSize"))
                            {
                                CDTXMania.ConfigIni.n選曲リストフォントのサイズdot = C変換.n値を文字列から取得して範囲内に丸めて返す(str4, 1, 0x3e7, CDTXMania.ConfigIni.n選曲リストフォントのサイズdot);
                            }
                            else if (str3.Equals("SelectListFontBold"))
                            {
                                CDTXMania.ConfigIni.b選曲リストフォントを太字にする = C変換.bONorOFF(str4[0]);
                            }
                            else if (str3.Equals("NamePlateType"))
                            {
                                CDTXMania.ConfigIni.eNamePlate = (Eタイプ)C変換.n値を文字列から取得して範囲内に丸めて返す(str4, 0, 3, (int)CDTXMania.ConfigIni.eNamePlate);
                            }
                            else if (str3.Equals("DrumSetMoves"))
                            {
                                CDTXMania.ConfigIni.eドラムセットを動かす = (Eタイプ)C変換.n値を文字列から取得して範囲内に丸めて返す(str4, 0, 2, (int)CDTXMania.ConfigIni.eドラムセットを動かす);
                            }
                            else if (str3.Equals("BPMBar"))
                            {
                                CDTXMania.ConfigIni.eBPMbar = (Eタイプ)C変換.n値を文字列から取得して範囲内に丸めて返す(str4, 0, 3, (int)CDTXMania.ConfigIni.eBPMbar);
                            }
                            else if (str3.Equals("LivePoint"))
                            {
                                CDTXMania.ConfigIni.bLivePoint = C変換.bONorOFF(str4[0]);
                            }
                            else if (str3.Equals("Speaker"))
                            {
                                CDTXMania.ConfigIni.bSpeaker = C変換.bONorOFF(str4[0]);
                            }
                            else if (str3.Equals("JudgeAnimeType"))
                            {
                                CDTXMania.ConfigIni.nJudgeAnimeType = C変換.n値を文字列から取得して範囲内に丸めて返す(str4, 0, 2, CDTXMania.ConfigIni.nJudgeAnimeType);
                            }
                            else if (str3.Equals("JudgeFrames"))
                            {
                                CDTXMania.ConfigIni.nJudgeFrames = C変換.n値を文字列から取得して返す(str4, CDTXMania.ConfigIni.nJudgeFrames);
                            }
                            else if (str3.Equals("JudgeInterval"))
                            {
                                CDTXMania.ConfigIni.nJudgeInterval = C変換.n値を文字列から取得して返す(str4, CDTXMania.ConfigIni.nJudgeInterval);
                            }
                            else if (str3.Equals("JudgeWidgh"))
                            {
                                CDTXMania.ConfigIni.nJudgeWidgh = C変換.n値を文字列から取得して返す(str4, CDTXMania.ConfigIni.nJudgeWidgh);
                            }
                            else if (str3.Equals("JudgeHeight"))
                            {
                                CDTXMania.ConfigIni.nJudgeHeight = C変換.n値を文字列から取得して返す(str4, CDTXMania.ConfigIni.nJudgeHeight);
                            }
                            else if (str3.Equals("ExplosionFrames"))
                            {
                                CDTXMania.ConfigIni.nExplosionFrames = C変換.n値を文字列から取得して範囲内に丸めて返す(str4, 0, int.MaxValue, (int)CDTXMania.ConfigIni.nExplosionFrames);
                            }
                            else if (str3.Equals("ExplosionInterval"))
                            {
                                CDTXMania.ConfigIni.nExplosionInterval = C変換.n値を文字列から取得して範囲内に丸めて返す(str4, 0, int.MaxValue, (int)CDTXMania.ConfigIni.nExplosionInterval);
                            }
                            else if (str3.Equals("ExplosionWidgh"))
                            {
                                CDTXMania.ConfigIni.nExplosionWidgh = C変換.n値を文字列から取得して範囲内に丸めて返す(str4, 0, int.MaxValue, (int)CDTXMania.ConfigIni.nExplosionWidgh);
                            }
                            else if (str3.Equals("ExplosionHeight"))
                            {
                                CDTXMania.ConfigIni.nExplosionHeight = C変換.n値を文字列から取得して範囲内に丸めて返す(str4, 0, int.MaxValue, (int)CDTXMania.ConfigIni.nExplosionHeight);
                            }
                            else if (str3.Equals("WailingFireFrames"))
                            {
                                CDTXMania.ConfigIni.nWailingFireFrames = C変換.n値を文字列から取得して範囲内に丸めて返す(str4, 0, int.MaxValue, (int)CDTXMania.ConfigIni.nWailingFireFrames);
                            }
                            else if (str3.Equals("WailingFireInterval"))
                            {
                                CDTXMania.ConfigIni.nWailingFireInterval = C変換.n値を文字列から取得して範囲内に丸めて返す(str4, 0, int.MaxValue, (int)CDTXMania.ConfigIni.nWailingFireInterval);
                            }
                            else if (str3.Equals("WailingFireWidgh"))
                            {
                                CDTXMania.ConfigIni.nWailingFireWidgh = C変換.n値を文字列から取得して範囲内に丸めて返す(str4, 0, int.MaxValue, (int)CDTXMania.ConfigIni.nWailingFireWidgh);
                            }
                            else if (str3.Equals("WailingFireHeight"))
                            {
                                CDTXMania.ConfigIni.nWailingFireHeight = C変換.n値を文字列から取得して範囲内に丸めて返す(str4, 0, int.MaxValue, (int)CDTXMania.ConfigIni.nWailingFireHeight);
                            }
                            else if (str3.Equals("WailingFirePositionXGuitar"))
                            {
                                CDTXMania.ConfigIni.nWailingFireX.Guitar = C変換.n値を文字列から取得して範囲内に丸めて返す(str4, 0, int.MaxValue, (int)CDTXMania.ConfigIni.nWailingFireX.Guitar);
                            }
                            else if (str3.Equals("WailingFirePositionXBass"))
                            {
                                CDTXMania.ConfigIni.nWailingFireX.Bass = C変換.n値を文字列から取得して範囲内に丸めて返す(str4, 0, int.MaxValue, (int)CDTXMania.ConfigIni.nWailingFireX.Bass);
                            }
                            else if (str3.Equals("WailingFirePosY"))
                            {
                                CDTXMania.ConfigIni.nWailingFireX.Bass = C変換.n値を文字列から取得して範囲内に丸めて返す(str4, 0, int.MaxValue, (int)CDTXMania.ConfigIni.nWailingFireY);
                            }
                            //-----------------------------
                        }
                        continue;
                    }
                    catch (Exception exception)
                    {
                        Trace.TraceError(exception.Message);
                        continue;
                    }
                }
            }
        }

		#region [ IDisposable 実装 ]
		//-----------------
		public void Dispose()
		{
			if( !this.bDisposed済み )
			{
				for( int i = 0; i < this.nシステムサウンド数; i++ )
					this[ i ].Dispose();

				this.bDisposed済み = true;
			}
		}
		//-----------------
		#endregion


		// その他

		#region [ private ]
		//-----------------
		private bool bDisposed済み;
		//-----------------
		#endregion

	}
}
