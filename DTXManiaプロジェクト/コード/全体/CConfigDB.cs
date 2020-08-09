using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Data.SQLite;
using System.Diagnostics;
using System.Web;
using FDK;

namespace DTXMania
{
	internal class CConfigDB
	{
		// クラス

		public class CKeyAssign
		{
			public class CKeyAssignPad
			{
				public CConfigDB.CKeyAssign.STKEYASSIGN[] HH
				{
					get
					{
						return this.padHH_R;
					}
					set
					{
						this.padHH_R = value;
					}
				}
				public CConfigDB.CKeyAssign.STKEYASSIGN[] R
				{
					get
					{
						return this.padHH_R;
					}
					set
					{
						this.padHH_R = value;
					}
				}
				public CConfigDB.CKeyAssign.STKEYASSIGN[] SD
				{
					get
					{
						return this.padSD_G;
					}
					set
					{
						this.padSD_G = value;
					}
				}
				public CConfigDB.CKeyAssign.STKEYASSIGN[] G
				{
					get
					{
						return this.padSD_G;
					}
					set
					{
						this.padSD_G = value;
					}
				}
				public CConfigDB.CKeyAssign.STKEYASSIGN[] BD
				{
					get
					{
						return this.padBD_B;
					}
					set
					{
						this.padBD_B = value;
					}
				}
				public CConfigDB.CKeyAssign.STKEYASSIGN[] B
				{
					get
					{
						return this.padBD_B;
					}
					set
					{
						this.padBD_B = value;
					}
				}
				public CConfigDB.CKeyAssign.STKEYASSIGN[] HT
				{
					get
					{
						return this.padHT_Pick;
					}
					set
					{
						this.padHT_Pick = value;
					}
				}
				public CConfigDB.CKeyAssign.STKEYASSIGN[] Pick
				{
					get
					{
						return this.padHT_Pick;
					}
					set
					{
						this.padHT_Pick = value;
					}
				}
				public CConfigDB.CKeyAssign.STKEYASSIGN[] LT
				{
					get
					{
						return this.padLT_Wail;
					}
					set
					{
						this.padLT_Wail = value;
					}
				}
				public CConfigDB.CKeyAssign.STKEYASSIGN[] Wail
				{
					get
					{
						return this.padLT_Wail;
					}
					set
					{
						this.padLT_Wail = value;
					}
				}
				public CConfigDB.CKeyAssign.STKEYASSIGN[] FT
				{
					get
					{
                        return this.padFT_Help;
					}
					set
					{
                        this.padFT_Help = value;
					}
				}
                public CConfigDB.CKeyAssign.STKEYASSIGN[] Help
				{
					get
					{
                        return this.padFT_Help;
					}
					set
					{
                        this.padFT_Help = value;
					}
				}
				public CConfigDB.CKeyAssign.STKEYASSIGN[] CY
				{
					get
					{
						return this.padCY_Decide;
					}
					set
					{
						this.padCY_Decide = value;
					}
				}
				public CConfigDB.CKeyAssign.STKEYASSIGN[] Decide
				{
					get
					{
						return this.padCY_Decide;
					}
					set
					{
						this.padCY_Decide = value;
					}
				}
				public CConfigDB.CKeyAssign.STKEYASSIGN[] HHO
				{
					get
					{
						return this.padHHO_Y;
					}
					set
					{
						this.padHHO_Y = value;
					}
				}
                public CConfigDB.CKeyAssign.STKEYASSIGN[] Y
				{
					get
					{
						return this.padHHO_Y;
					}
					set
					{
						this.padHHO_Y = value;
					}
				}
				public CConfigDB.CKeyAssign.STKEYASSIGN[] RD
				{
					get
					{
						return this.padRD;
					}
					set
					{
						this.padRD = value;
					}
				}
                public CConfigDB.CKeyAssign.STKEYASSIGN[] P
				{
					get
					{
						return this.padLC_P;
					}
					set
					{
						this.padLC_P = value;
					}
				}
				public CConfigDB.CKeyAssign.STKEYASSIGN[] LC
				{
					get
					{
						return this.padLC_P;
					}
					set
					{
						this.padLC_P = value;
					}
				}
				public CConfigDB.CKeyAssign.STKEYASSIGN[] LP
				{
					get
					{
						return this.padLP;
					}
					set
					{
						this.padLP = value;
					}
				}

                public CConfigDB.CKeyAssign.STKEYASSIGN[] LBD
                {
                    get
                    {
                        return this.padLBD;
                    }
                    set
                    {
                        this.padLBD = value;
                    }
                }

				public CConfigDB.CKeyAssign.STKEYASSIGN[] Capture
				{
					get
					{
						return this.padCapture;
					}
					set
					{
						this.padCapture = value;
					}
				}
				public CConfigDB.CKeyAssign.STKEYASSIGN[] this[ int index ]
				{
					get
					{
						switch ( index )
						{
							case (int) EKeyConfigPad.HH:
								return this.padHH_R;

							case (int) EKeyConfigPad.SD:
								return this.padSD_G;

							case (int) EKeyConfigPad.BD:
								return this.padBD_B;

							case (int) EKeyConfigPad.HT:
								return this.padHT_Pick;

							case (int) EKeyConfigPad.LT:
								return this.padLT_Wail;

							case (int) EKeyConfigPad.FT:
                                return this.padFT_Help;

							case (int) EKeyConfigPad.CY:
								return this.padCY_Decide;

							case (int) EKeyConfigPad.HHO:
								return this.padHHO_Y;

							case (int) EKeyConfigPad.RD:
								return this.padRD;

							case (int) EKeyConfigPad.LC:
								return this.padLC_P;

							case (int) EKeyConfigPad.LP:	// #27029 2012.1.4 from
								return this.padLP;			//(HPからLPに。)

                            case (int) EKeyConfigPad.LBD:
                                return this.padLBD;

							case (int) EKeyConfigPad.Capture:
								return this.padCapture;
						}
						throw new IndexOutOfRangeException();
					}
					set
					{
						switch ( index )
						{
							case (int) EKeyConfigPad.HH:
								this.padHH_R = value;
								return;

							case (int) EKeyConfigPad.SD:
								this.padSD_G = value;
								return;

							case (int) EKeyConfigPad.BD:
								this.padBD_B = value;
								return;

							case (int) EKeyConfigPad.Pick:
								this.padHT_Pick = value;
								return;

							case (int) EKeyConfigPad.LT:
								this.padLT_Wail = value;
								return;

							case (int) EKeyConfigPad.FT:
                                this.padFT_Help = value;
								return;

							case (int) EKeyConfigPad.CY:
								this.padCY_Decide = value;
								return;

							case (int) EKeyConfigPad.HHO:
								this.padHHO_Y = value;
								return;
                            
                            case (int) EKeyConfigPad.RD:
								this.padRD = value;
								return;
                            
                            case (int) EKeyConfigPad.LC:
								this.padLC_P = value;
								return;
                            
                            case (int) EKeyConfigPad.LP:
                                this.padLP = value;
                                return;
                            
                            case (int) EKeyConfigPad.LBD:
								this.padLBD = value;
								return;

							case (int) EKeyConfigPad.Capture:
								this.padCapture = value;
								return;
						}
						throw new IndexOutOfRangeException();
					}
				}

				#region [ private ]
				//-----------------
				private CConfigDB.CKeyAssign.STKEYASSIGN[] padBD_B;
				private CConfigDB.CKeyAssign.STKEYASSIGN[] padCY_Decide;
                private CConfigDB.CKeyAssign.STKEYASSIGN[] padFT_Help;
				private CConfigDB.CKeyAssign.STKEYASSIGN[] padHH_R;
				private CConfigDB.CKeyAssign.STKEYASSIGN[] padHHO_Y;
				private CConfigDB.CKeyAssign.STKEYASSIGN[] padHT_Pick;
				private CConfigDB.CKeyAssign.STKEYASSIGN[] padLC_P;
				private CConfigDB.CKeyAssign.STKEYASSIGN[] padLT_Wail;
				private CConfigDB.CKeyAssign.STKEYASSIGN[] padRD;
				private CConfigDB.CKeyAssign.STKEYASSIGN[] padSD_G;
				private CConfigDB.CKeyAssign.STKEYASSIGN[] padLP;
                private CConfigDB.CKeyAssign.STKEYASSIGN[] padLBD;
				private CConfigDB.CKeyAssign.STKEYASSIGN[] padCapture;
				//-----------------
				#endregion
			}

			[StructLayout( LayoutKind.Sequential )]
			public struct STKEYASSIGN
			{
				public E入力デバイス 入力デバイス;
				public int ID;
				public int コード;
				public STKEYASSIGN( E入力デバイス DeviceType, int nID, int nCode )
				{
					this.入力デバイス = DeviceType;
					this.ID = nID;
					this.コード = nCode;
				}
			}

			public CKeyAssignPad Bass = new CKeyAssignPad();
			public CKeyAssignPad Drums = new CKeyAssignPad();
			public CKeyAssignPad Guitar = new CKeyAssignPad();
			public CKeyAssignPad System = new CKeyAssignPad();
			public CKeyAssignPad this[ int index ]
			{
				get
				{
					switch( index )
					{
						case (int) EKeyConfigPart.DRUMS:
							return this.Drums;

						case (int) EKeyConfigPart.GUITAR:
							return this.Guitar;

						case (int) EKeyConfigPart.BASS:
							return this.Bass;

						case (int) EKeyConfigPart.SYSTEM:
							return this.System;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case (int) EKeyConfigPart.DRUMS:
							this.Drums = value;
							return;

						case (int) EKeyConfigPart.GUITAR:
							this.Guitar = value;
							return;

						case (int) EKeyConfigPart.BASS:
							this.Bass = value;
							return;

						case (int) EKeyConfigPart.SYSTEM:
							this.System = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}
        public enum ESoundDeviceTypeForConfig
        {
            ACM = 0,
            ASIO,
            WASAPI,
            Unknown = 99
        }

		// プロパティ

#if false      // #23625 2011.1.11 Config.iniからダメージ/回復値の定数変更を行う場合はここを有効にする 087リリースに合わせ機能無効化
		//----------------------------------------
		public float[,] fGaugeFactor = new float[5,2];
		public float[] fDamageLevelFactor = new float[3];
		//----------------------------------------
#endif
		//
		public int nSelectedProfileID = 1;//1 is for SYSTEM
		//
		public int nBGAlpha;
        public int nMovieAlpha;
		public bool bAVI有効;
		public bool bBGA有効;
		public bool bBGM音を発声する;
		public STDGBVALUE<bool> bHidden;
		public STDGBVALUE<bool> bLeft;
		public STDGBVALUE<bool> bLight;
		public bool bLogDTX詳細ログ出力;
		public bool bLog曲検索ログ出力;
		public bool bLog作成解放ログ出力;
		public STDGBVALUE<bool> bReverse;
		public bool bScoreIniを出力する;
		public bool bSTAGEFAILED有効;
		public STDGBVALUE<bool> bSudden;
		public bool bTight;
		public STDGBVALUE<bool> bGraph有効;     // #24074 2011.01.23 add ikanick
		public bool bWave再生位置自動調整機能有効;
		public bool bシンバルフリー;
		public bool bストイックモード;
		public bool bドラム打音を発声する;
		public bool bフィルイン有効;
		public bool bランダムセレクトで子BOXを検索対象とする;
		public bool bログ出力;
		public STDGBVALUE<bool> b演奏音を強調する;
		public bool b演奏情報を表示する;
        public bool bAutoAddGage; //2012.9.18
		public bool b歓声を発声する;
		public bool b垂直帰線待ちを行う;
		public bool b選曲リストフォントを斜体にする;
		public bool b選曲リストフォントを太字にする;
        public bool bDirectShowMode;
		public bool b全画面モード;
        public int n初期ウィンドウ開始位置X; // #30675 2013.02.04 ikanick add
	    public int n初期ウィンドウ開始位置Y;
		public int nウインドウwidth;				// #23510 2010.10.31 yyagi add
		public int nウインドウheight;				// #23510 2010.10.31 yyagi add
        public bool ボーナス演出を表示する;
        public bool bHAZARD;
        public int nSoundDeviceType; // #24820 2012.12.23 yyagi 出力サウンドデバイス(0=ACM(にしたいが設計がきつそうならDirectShow), 1=ASIO, 2=WASAPI)
        public int nWASAPIBufferSizeMs; // #24820 2013.1.15 yyagi WASAPIのバッファサイズ
        //public int nASIOBufferSizeMs; // #24820 2012.12.28 yyagi ASIOのバッファサイズ
        public int nASIODevice; // #24820 2013.1.17 yyagi ASIOデバイス
        public bool bDynamicBassMixerManagement; // #24820
        public STDGBVALUE<Eタイプ> eAttackEffect;
        public STDGBVALUE<Eタイプ> eNumOfLanes;
        public STDGBVALUE<Eタイプ> eDkdkType;
        public STDGBVALUE<Eタイプ> eLaneType;
        public STDGBVALUE<Eタイプ> eLBDGraphics;
        public STDGBVALUE<Eタイプ> eHHOGraphics;
        public ERDPosition eRDPosition;
        public int nInfoType;
        public int nSkillMode;
		public Dictionary<int, string> dicJoystick;
		public ECYGroup eCYGroup;
		public Eダークモード eDark;
		public EFTGroup eFTGroup;
		public EHHGroup eHHGroup;
		public EBDGroup eBDGroup;					// #27029 2012.1.4 from add
		public E打ち分け時の再生の優先順位 eHitSoundPriorityCY;
		public E打ち分け時の再生の優先順位 eHitSoundPriorityFT;
		public E打ち分け時の再生の優先順位 eHitSoundPriorityHH;
        public E打ち分け時の再生の優先順位 eHitSoundPriorityLP;
        public STDGBVALUE<Eランダムモード> eRandom;
        public STDGBVALUE<Eランダムモード> eRandomPedal;
        public STDGBVALUE<bool> bAssignToLBD;
		public Eダメージレベル eダメージレベル;
        public CKeyAssign KeyAssign;

        public STDGBVALUE<int> nLaneDisp;
        public STDGBVALUE<bool> bJudgeLineDisp;
        public STDGBVALUE<bool> bLaneFlush;

        public int nPedalLagTime;   //#xxxxx 2013.07.11 kairera0467

		public int n非フォーカス時スリープms;       // #23568 2010.11.04 ikanick add
		public int nフレーム毎スリープms;			// #xxxxx 2011.11.27 yyagi add
		public int n演奏速度;
		public int n曲が選択されてからプレビュー音が鳴るまでのウェイトms;
		public int n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms;
		public int n自動再生音量;
		public int n手動再生音量;
		public int n選曲リストフォントのサイズdot;
        public int[] nNameColor;
		public STDGBVALUE<int> n表示可能な最小コンボ数;
		public STDGBVALUE<int> n譜面スクロール速度;
		public string strDTXManiaのバージョン;
		public string str曲データ検索パス;
		public string str選曲リストフォント;
        public string[] strCardName; //2015.12.3 kaiera0467 DrumとGuitarとBassで名前を別々にするため、string[3]に変更。
        public string[] strGroupName;
		public Eドラムコンボ文字の表示位置 ドラムコンボ文字の表示位置;
        public bool bドラムコンボ文字の表示;
        public STDGBVALUE<Eタイプ> 判定文字表示位置;
        public int nMovieMode;
        public STDGBVALUE<int> nJudgeLine;
        public STDGBVALUE<int> nShutterInSide;
        public STDGBVALUE<int> nShutterOutSide;
        public bool bCLASSIC譜面判別を有効にする;
        public bool bMutingLP;
        public bool bSkillModeを自動切換えする;

        public bool b曲名表示をdefのものにする;

        #region [ XGオプション ]
        public Eタイプ eNamePlate;
        public Eタイプ eドラムセットを動かす;
        public Eタイプ eBPMbar;
        public bool bLivePoint;
        public bool bSpeaker;
        #endregion

        #region [ GDオプション ]
        public bool b難易度表示をXG表示にする;
        public bool bShowMusicInfo;
        public bool bShowScore;
        public string str曲名表示フォント;
        #endregion

        #region[ 画像関連 ]
        public int nJudgeAnimeType;
        public int nJudgeFrames;
        public int nJudgeInterval;
        public int nJudgeWidgh;
        public int nJudgeHeight;
        public int nExplosionFrames;
        public int nExplosionInterval;
        public int nExplosionWidgh;
        public int nExplosionHeight;
        public int nWailingFireFrames;
        public int nWailingFireInterval;
        public int nWailingFireWidgh;
        public int nWailingFireHeight;
        public STDGBVALUE<int> nWailingFireX;
        public int nWailingFireY;
        #endregion

        public STDGBVALUE<int> nInputAdjustTimeMs;	// #23580 2011.1.3 yyagi タイミングアジャスト機能
        public int nCommonBGMAdjustMs;              // #36372 2016.06.19 kairera0467 全曲共通のBGMオフセット
        public STDGBVALUE<int> nJudgeLinePosOffset; // #31602 2013.6.23 yyagi 判定ライン表示位置のオフセット
        public int nShowLagType;					// #25370 2011.6.5 yyagi ズレ時間表示機能
        public int nShowLagTypeColor;
        public STDGBVALUE<int> nHidSud;
        public bool bIsAutoResultCapture;			// #25399 2011.6.9 yyagi リザルト画像自動保存機能のON/OFF制御
		public int nPoliphonicSounds;				// #28228 2012.5.1 yyagi レーン毎の最大同時発音数
		public bool bバッファ入力を行う;
		public bool bIsEnabledSystemMenu;			// #28200 2012.5.1 yyagi System Menuの使用可否切替
		public string strSystemSkinSubfolderFullName;	// #28195 2012.5.2 yyagi Skin切替用 System/以下のサブフォルダ名
		public bool bUseBoxDefSkin;						// #28195 2012.5.6 yyagi Skin切替用 box.defによるスキン変更機能を使用するか否か

        //つまみ食い
        public STDGBVALUE<EAutoGhostData> eAutoGhost;               // #35411 2015.8.18 chnmr0 プレー時使用ゴーストデータ種別
        public STDGBVALUE<ETargetGhostData> eTargetGhost;               // #35411 2015.8.18 chnmr0 ゴーストデータ再生方法

        public bool bConfigIniがないかDTXManiaのバージョンが異なる
		{
			get
			{
				return ( !this.bConfigIniが存在している || !CDTXMania.VERSION.Equals( this.strDTXManiaのバージョン ) );
			}
		}
		public bool bDrums有効
		{
			get
			{
				return this._bDrums有効;
			}
			set
			{
				this._bDrums有効 = value;
				if( !this._bGuitar有効 && !this._bDrums有効 )
				{
					this._bGuitar有効 = true;
				}
			}
		}
		public bool bEnterがキー割り当てのどこにも使用されていない
		{
			get
			{
				for( int i = 0; i <= (int)EKeyConfigPart.SYSTEM; i++ )
				{
					for( int j = 0; j <= (int)EKeyConfigPad.Capture; j++ )
					{
						for( int k = 0; k < 0x10; k++ )
						{
							if( ( this.KeyAssign[ i ][ j ][ k ].入力デバイス == E入力デバイス.キーボード ) && ( this.KeyAssign[ i ][ j ][ k ].コード == (int) SlimDX.DirectInput.Key.Return ) )
							{
								return false;
							}
						}
					}
				}
				return true;
			}
		}
		public bool bGuitar有効
		{
			get
			{
				return this._bGuitar有効;
			}
			set
			{
				this._bGuitar有効 = value;
				if( !this._bGuitar有効 && !this._bDrums有効 )
				{
					this._bDrums有効 = true;
				}
			}
		}
		public bool bウィンドウモード
		{
			get
			{
				return !this.b全画面モード;
			}
			set
			{
				this.b全画面モード = !value;
			}
		}
		public bool bギタレボモード
		{
			get
			{
				return ( !this.bDrums有効 && this.bGuitar有効 );
			}
		}
		public bool bドラムが全部オートプレイである
		{
			get
			{
                for (int i = (int) Eレーン.LC; i < (int) Eレーン.LBD; i++)
				{
					if( !this.bAutoPlay[ i ] )
					{
						return false;
					}
				}
				return true;
			}
		}
		public bool bギターが全部オートプレイである
		{
			get
			{
				for ( int i = (int) Eレーン.GtR; i <= (int) Eレーン.GtW; i++ )
				{
					if ( !this.bAutoPlay[ i ] )
					{
						return false;
					}
				}
				return true;
			}
		}
		public bool bベースが全部オートプレイである
		{
			get
			{
				for ( int i = (int) Eレーン.BsR; i <= (int) Eレーン.BsW; i++ )
				{
					if ( !this.bAutoPlay[ i ] )
					{
						return false;
					}
				}
				return true;
			}
		}
		public bool b演奏情報を表示しない
		{
			get
			{
				return !this.b演奏情報を表示する;
			}
			set
			{
				this.b演奏情報を表示する = !value;
			}
		}
		public int n背景の透過度
		{
			get
			{
				return this.nBGAlpha;
			}
			set
			{
				if( value < 0 )
				{
					this.nBGAlpha = 0;
				}
				else if( value > 0xff )
				{
					this.nBGAlpha = 0xff;
				}
				else
				{
					this.nBGAlpha = value;
				}
			}
		}
		public int nRisky;						// #23559 2011.6.20 yyagi Riskyでの残ミス数。0で閉店
		public bool bIsAllowedDoubleClickFullscreen;	// #26752 2011.11.27 yyagi ダブルクリックしてもフルスクリーンに移行しない
		public bool bIsSwappedGuitarBass			// #24063 2011.1.16 yyagi ギターとベースの切り替え中か否か
		{
			get;
			set;
		}
		public bool bIsSwappedGuitarBass_AutoFlagsAreSwapped	// #24415 2011.2.21 yyagi FLIP中にalt-f4終了で、AUTOフラグがswapした状態でconfig.iniが出力されてしまうことを避けるためのフラグ
		{
		    get;
		    set;
		}
        public bool bTimeStretch;					// #23664 2013.2.24 yyagi ピッチ変更無しで再生速度を変更するかどうか
		public STAUTOPLAY bAutoPlay;
		public STRANGE nヒット範囲ms;
		[StructLayout( LayoutKind.Sequential )]
		public struct STRANGE
		{
			public int Perfect;
			public int Great;
			public int Good;
			public int Poor;
			public int this[ int index ]
			{
				get
				{
					switch( index )
					{
						case 0:
							return this.Perfect;

						case 1:
							return this.Great;

						case 2:
							return this.Good;

						case 3:
							return this.Poor;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case 0:
							this.Perfect = value;
							return;

						case 1:
							this.Great = value;
							return;

						case 2:
							this.Good = value;
							return;

						case 3:
							this.Poor = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}

		public STLANEVALUE nVelocityMin;
		[StructLayout( LayoutKind.Sequential )]
		public struct STLANEVALUE
		{
			public int LC;
			public int HH;
			public int SD;
			public int BD;
			public int HT;
			public int LT;
			public int FT;
			public int CY;
			public int RD;
            public int LP;
            public int LBD;
			public int Guitar;
			public int Bass;
			public int this[ int index ]
			{
				get
				{
					switch( index )
					{
						case 0:
							return this.LC;

						case 1:
							return this.HH;

						case 2:
							return this.SD;

						case 3:
							return this.BD;

						case 4:
							return this.HT;

						case 5:
							return this.LT;

						case 6:
							return this.FT;

						case 7:
							return this.CY;

						case 8:
							return this.RD;

                        case 9:
                            return this.LP;

                        case 10:
                            return this.LBD;

						case 11:
							return this.Guitar;

						case 12:
							return this.Bass;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case 0:
							this.LC = value;
							return;

						case 1:
							this.HH = value;
							return;

						case 2:
							this.SD = value;
							return;

						case 3:
							this.BD = value;
							return;

						case 4:
							this.HT = value;
							return;

						case 5:
							this.LT = value;
							return;

						case 6:
							this.FT = value;
							return;

						case 7:
							this.CY = value;
							return;

						case 8:
							this.RD = value;
							return;

                        case 9:
                            this.LP = value;
                            return;

                        case 10:
                            this.LBD = value;
                            return;

						case 11:
							this.Guitar = value;
							return;

						case 12:
							this.Bass = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}

        /*
		// #27029 2012.1.5 from:
		// BDGroup が FP|BD→FP&BD に変化した際に自動変化するパラメータの値のバックアップ。FP&BD→FP|BD の時に元に戻す。
		// これらのバックアップ値は、BDGroup が FP&BD 状態の時にのみ Config.ini に出力され、BDGroup が FP|BD に戻ったら Config.ini から消える。
		public class CBackupOf1BD
		{
			public EHHGroup eHHGroup = EHHGroup.全部打ち分ける;
			public E打ち分け時の再生の優先順位 eHitSoundPriorityHH = E打ち分け時の再生の優先順位.ChipがPadより優先;
		}
		public CBackupOf1BD BackupOf1BD = null;
        */
        public void SwapGuitarBassInfos_AutoFlags()
        {
            //bool ts = CDTXMania.ConfigDB.bAutoPlay.Bass;			// #24415 2011.2.21 yyagi: FLIP時のリザルトにAUTOの記録が混ざらないよう、AUTOのフラグもswapする
            //CDTXMania.ConfigDB.bAutoPlay.Bass = CDTXMania.ConfigDB.bAutoPlay.Guitar;
            //CDTXMania.ConfigDB.bAutoPlay.Guitar = ts;

            int looptime = (int)Eレーン.GtW - (int)Eレーン.GtR + 1;		// #29390 2013.1.25 yyagi ギターのAutoLane/AutoPick対応に伴い、FLIPもこれに対応
            for (int i = 0; i < looptime; i++)							// こんなに離れたところを独立して修正しなければならない設計ではいけませんね___
            {
                bool b = CDTXMania.ConfigDB.bAutoPlay[i + (int)Eレーン.BsR];
                CDTXMania.ConfigDB.bAutoPlay[i + (int)Eレーン.BsR] = CDTXMania.ConfigDB.bAutoPlay[i + (int)Eレーン.GtR];
                CDTXMania.ConfigDB.bAutoPlay[i + (int)Eレーン.GtR] = b;
            }

            CDTXMania.ConfigDB.bIsSwappedGuitarBass_AutoFlagsAreSwapped = !CDTXMania.ConfigDB.bIsSwappedGuitarBass_AutoFlagsAreSwapped;
        }
		
		// コンストラクタ

		public CConfigDB()
		{
#if false		// #23625 2011.1.11 Config.iniからダメージ/回復値の定数変更を行う場合はここを有効にする 087リリースに合わせ機能無効化
			//----------------------------------------
			this.fGaugeFactor[0,0] =  0.004f;
			this.fGaugeFactor[0,1] =  0.006f;
			this.fGaugeFactor[1,0] =  0.002f;
			this.fGaugeFactor[1,1] =  0.003f;
			this.fGaugeFactor[2,0] =  0.000f;
			this.fGaugeFactor[2,1] =  0.000f;
			this.fGaugeFactor[3,0] = -0.020f;
			this.fGaugeFactor[3,1] = -0.030f;
			this.fGaugeFactor[4,0] = -0.050f;
			this.fGaugeFactor[4,1] = -0.050f;

			this.fDamageLevelFactor[0] = 0.5f;
			this.fDamageLevelFactor[1] = 1.0f;
			this.fDamageLevelFactor[2] = 1.5f;
			//----------------------------------------
#endif
			this.strDTXManiaのバージョン = "Unknown";
			this.str曲データ検索パス = @".\";
			this.b全画面モード = false;
			this.b垂直帰線待ちを行う = true;
            this.n初期ウィンドウ開始位置X = 0; // #30675 2013.02.04 ikanick add
            this.n初期ウィンドウ開始位置Y = 0;
            this.bDirectShowMode = false;
			this.nウインドウwidth = SampleFramework.GameWindowSize.Width;			// #23510 2010.10.31 yyagi add
			this.nウインドウheight = SampleFramework.GameWindowSize.Height;			// 
            this.nMovieMode = 1;
            this.nMovieAlpha = 0;
            this.nJudgeLine.Drums = 0;
            this.nJudgeLine.Guitar = 0;
            this.nJudgeLine.Bass = 0;
            this.nShutterInSide = new STDGBVALUE<int>();
            this.nShutterInSide.Drums = 0;
            this.nShutterOutSide = new STDGBVALUE<int>();
            this.nShutterOutSide.Drums = 0;
			this.nフレーム毎スリープms = -1;			// #xxxxx 2011.11.27 yyagi add
			this.n非フォーカス時スリープms = 1;			// #23568 2010.11.04 ikanick add
			this._bGuitar有効 = false;
			this._bDrums有効 = true;
			this.nBGAlpha = 100;
			this.eダメージレベル = Eダメージレベル.普通;
			this.bSTAGEFAILED有効 = true;
			this.bAVI有効 = true;
			this.bBGA有効 = true;
			this.bフィルイン有効 = true;
            this.ボーナス演出を表示する = true;
            this.eRDPosition = ERDPosition.RCRD;
            this.nInfoType = 0;
            this.nSkillMode = 0;
            this.eAttackEffect.Drums = Eタイプ.A;
            this.eAttackEffect.Guitar = Eタイプ.A;
            this.eAttackEffect.Bass = Eタイプ.A;
            this.eLaneType = new STDGBVALUE<Eタイプ>();
            this.eLaneType.Drums = Eタイプ.A;
            this.eHHOGraphics = new STDGBVALUE<Eタイプ>();
            this.eHHOGraphics.Drums = Eタイプ.A;
            this.eLBDGraphics = new STDGBVALUE<Eタイプ>();
            this.eLBDGraphics.Drums = Eタイプ.A;
            this.eDkdkType = new STDGBVALUE<Eタイプ>();
            this.eDkdkType.Drums = Eタイプ.A;
            this.eNumOfLanes = new STDGBVALUE<Eタイプ>();
            this.eNumOfLanes.Drums = Eタイプ.A;
            this.eNumOfLanes.Guitar = Eタイプ.A;
            this.eNumOfLanes.Bass = Eタイプ.A;
            this.bAssignToLBD = default(STDGBVALUE<bool>);
            this.bAssignToLBD.Drums = false;
            this.eRandom = default(STDGBVALUE<Eランダムモード>);
            this.eRandom.Drums = Eランダムモード.OFF;
            this.eRandom.Guitar = Eランダムモード.OFF;
            this.eRandom.Bass = Eランダムモード.OFF;
            this.eRandomPedal = default(STDGBVALUE<Eランダムモード>);
            this.eRandomPedal.Drums = Eランダムモード.OFF;
            this.eRandomPedal.Guitar = Eランダムモード.OFF;
            this.eRandomPedal.Bass = Eランダムモード.OFF;
            this.nLaneDisp = new STDGBVALUE<int>();
            this.nLaneDisp.Drums = 0;
            this.nLaneDisp.Guitar = 0;
            this.nLaneDisp.Bass = 0;
            this.bJudgeLineDisp = new STDGBVALUE<bool>();
            this.bJudgeLineDisp.Drums = true;
            this.bJudgeLineDisp.Guitar = true;
            this.bJudgeLineDisp.Bass = true;
            this.bLaneFlush = new STDGBVALUE<bool>();
            this.bLaneFlush.Drums = true;
            this.bLaneFlush.Guitar = true;
            this.bLaneFlush.Bass = true;

            this.strCardName = new string[ 3 ];
            this.strGroupName = new string[ 3 ];

			//
			this.strCardName[0] = "";
			this.strCardName[1] = "";
			this.strCardName[2] = "";

			this.strGroupName[0] = "";
			this.strGroupName[1] = "";
			this.strGroupName[2] = "";

			this.nNameColor = new int[ 3 ];

            #region[ 画像関連 ]
            this.nJudgeAnimeType = 1;
            this.nJudgeFrames = 24;
            this.nJudgeInterval = 14;
            this.nJudgeWidgh = 250;
            this.nJudgeHeight = 170;

            this.nExplosionFrames = 1;
            this.nExplosionInterval = 50;
            this.nExplosionWidgh = 0;
            this.nExplosionHeight = 0;

            this.nWailingFireFrames = 0;
            this.nWailingFireInterval = 0;
            this.nWailingFireWidgh = 0;
            this.nWailingFireHeight = 0;
            this.nWailingFireY = 0;
            #endregion

            this.nPedalLagTime = 0;

            this.bAutoAddGage = false;
			this.n曲が選択されてからプレビュー音が鳴るまでのウェイトms = 1000;
			this.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms = 100;
			this.bWave再生位置自動調整機能有効 = true;
			this.bBGM音を発声する = true;
			this.bドラム打音を発声する = true;
			this.b歓声を発声する = true;
			this.bScoreIniを出力する = true;
			this.bランダムセレクトで子BOXを検索対象とする = true;
			this.n表示可能な最小コンボ数 = new STDGBVALUE<int>();
			this.n表示可能な最小コンボ数.Drums = 11;
			this.n表示可能な最小コンボ数.Guitar = 2;
			this.n表示可能な最小コンボ数.Bass = 2;
			this.str選曲リストフォント = "MS PGothic";
			this.n選曲リストフォントのサイズdot = 20;
			this.b選曲リストフォントを太字にする = true;
			this.n自動再生音量 = 80;
			this.n手動再生音量 = 100;
			this.bログ出力 = true;
            this.b曲名表示をdefのものにする = false;
			this.b演奏音を強調する = new STDGBVALUE<bool>();
			this.bSudden = new STDGBVALUE<bool>();
			this.bHidden = new STDGBVALUE<bool>();
			this.bReverse = new STDGBVALUE<bool>();
			this.eRandom = new STDGBVALUE<Eランダムモード>();
			this.bLight = new STDGBVALUE<bool>();
			this.bLeft = new STDGBVALUE<bool>();
            this.判定文字表示位置 = new STDGBVALUE<Eタイプ>();
			this.n譜面スクロール速度 = new STDGBVALUE<int>();
			this.nInputAdjustTimeMs = new STDGBVALUE<int>();	// #23580 2011.1.3 yyagi
            this.nCommonBGMAdjustMs = 0; // #36372 2016.06.19 kairera0467
            this.nJudgeLinePosOffset = new STDGBVALUE<int>(); // #31602 2013.6.23 yyagi
			for ( int i = 0; i < 3; i++ )
			{
				this.b演奏音を強調する[ i ] = true;
				this.bSudden[ i ] = false;
				this.bHidden[ i ] = false;
				this.bReverse[ i ] = false;
				this.eRandom[ i ] = Eランダムモード.OFF;
				this.bLight[ i ] = false;
				this.bLeft[ i ] = false;
				this.判定文字表示位置[ i ] = Eタイプ.A;
				this.n譜面スクロール速度[ i ] = 1;
				this.nInputAdjustTimeMs[ i ] = 0;
                this.nJudgeLinePosOffset[i] = 0;
			}
			this.n演奏速度 = 20;
            this.ドラムコンボ文字の表示位置 = Eドラムコンボ文字の表示位置.RIGHT;
            this.bドラムコンボ文字の表示 = true;
            this.bCLASSIC譜面判別を有効にする = false;
            this.bSkillModeを自動切換えする = false;
            this.bMutingLP = true;
            #region [ AutoPlay ]
            this.bAutoPlay = new STAUTOPLAY();
			this.bAutoPlay.HH = false;
			this.bAutoPlay.SD = false;
			this.bAutoPlay.BD = false;
			this.bAutoPlay.HT = false;
			this.bAutoPlay.LT = false;
			this.bAutoPlay.FT = false;
			this.bAutoPlay.CY = false;
            this.bAutoPlay.RD = false;
			this.bAutoPlay.LC = false;
            this.bAutoPlay.LP = false;
            this.bAutoPlay.LBD = false;
			//this.bAutoPlay.Guitar = true;
			//this.bAutoPlay.Bass = true;
			this.bAutoPlay.GtR = true;
			this.bAutoPlay.GtG = true;
			this.bAutoPlay.GtB = true;
            this.bAutoPlay.GtY = true;
            this.bAutoPlay.GtP = true;
			this.bAutoPlay.GtPick = true;
			this.bAutoPlay.GtW = true;
			this.bAutoPlay.BsR = true;
			this.bAutoPlay.BsG = true;
			this.bAutoPlay.BsB = true;
            this.bAutoPlay.BsY = true;
            this.bAutoPlay.BsP = true;
			this.bAutoPlay.BsPick = true;
			this.bAutoPlay.BsW = true;
            #endregion
            this.nヒット範囲ms = new STRANGE();
			this.nヒット範囲ms.Perfect = 34;
			this.nヒット範囲ms.Great = 67;
			this.nヒット範囲ms.Good = 84;
			this.nヒット範囲ms.Poor = 117;
			this.ConfigIniファイル名 = "";
			this.dicJoystick = new Dictionary<int, string>( 10 );
			//Clear all key assignment
			this.tキーアサインを全部クリアする();
			#region [ velocityMin ]
			this.nVelocityMin.LC = 0;					// #23857 2011.1.31 yyagi VelocityMin
			this.nVelocityMin.HH = 20;
			this.nVelocityMin.SD = 0;
			this.nVelocityMin.BD = 0;
			this.nVelocityMin.HT = 0;
			this.nVelocityMin.LT = 0;
			this.nVelocityMin.FT = 0;
			this.nVelocityMin.CY = 0;
			this.nVelocityMin.RD = 0;
            this.nVelocityMin.LP = 0;
            this.nVelocityMin.LBD = 0;
            #endregion

            this.bHAZARD = false;
			this.nRisky = 0;							// #23539 2011.7.26 yyagi RISKYモード
			this.nShowLagType = (int) EShowLagType.OFF;	// #25370 2011.6.3 yyagi ズレ時間表示
            this.nShowLagTypeColor = 0;
			this.bIsAutoResultCapture = false;			// #25399 2011.6.9 yyagi リザルト画像自動保存機能ON/OFF

            #region [ XGオプション ]
            this.bLivePoint = true;
            this.bSpeaker = true;
            this.eNamePlate = Eタイプ.A;
            #endregion

            #region [ GDオプション ]
            this.b難易度表示をXG表示にする = false;
            this.bShowScore = true;
            this.bShowMusicInfo = true;
            this.str曲名表示フォント = "MS PGothic";
            #endregion

			this.bバッファ入力を行う = true;
			this.bIsSwappedGuitarBass = false;			// #24063 2011.1.16 yyagi ギターとベースの切り替え
			this.bIsAllowedDoubleClickFullscreen = true;	// #26752 2011.11.26 ダブルクリックでのフルスクリーンモード移行を許可
			this.eBDGroup = EBDGroup.打ち分ける;		// #27029 2012.1.4 from HHPedalとBassPedalのグルーピング
            this.nPoliphonicSounds = 4;                 // #28228 2012.5.1 yyagi レーン毎の最大同時発音数
                                                        // #24820 2013.1.15 yyagi 初期値を4から2に変更。BASS.net使用時の負荷軽減のため。
                                                        // #24820 2013.1.17 yyagi 初期値を4に戻した。動的なミキサー制御がうまく動作しているため。
			this.bIsEnabledSystemMenu = true;			// #28200 2012.5.1 yyagi System Menuの利用可否切替(使用可)
			this.strSystemSkinSubfolderFullName = "";	// #28195 2012.5.2 yyagi 使用中のSkinサブフォルダ名
			this.bUseBoxDefSkin = true;					// #28195 2012.5.6 yyagi box.defによるスキン切替機能を使用するか否か
            this.bTight = false;                        // #29500 2012.9.11 kairera0467
            this.nSoundDeviceType = (int)ESoundDeviceTypeForConfig.ACM; // #24820 2012.12.23 yyagi 初期値はACM
            this.nWASAPIBufferSizeMs = 0;               // #24820 2013.1.15 yyagi 初期値は0(自動設定)
            this.nASIODevice = 0;                       // #24820 2013.1.17 yyagi
//          this.nASIOBufferSizeMs = 0;                 // #24820 2012.12.25 yyagi 初期値は0(自動設定)
            this.bDynamicBassMixerManagement = true;    //
            this.bTimeStretch = false;					// #23664 2013.2.24 yyagi 初期値はfalse (再生速度変更を、ピッチ変更にて行う)

		}
		public CConfigDB( string dbファイル名)
			: this()
		{
			this.tファイルから読み込み(dbファイル名);
		}


		// メソッド

		public void t指定した入力が既にアサイン済みである場合はそれを全削除する( E入力デバイス DeviceType, int nID, int nCode )
		{
			for( int i = 0; i <= (int)EKeyConfigPart.SYSTEM; i++ )
			{
				for( int j = 0; j <= (int)EKeyConfigPad.Capture; j++ )
				{
					for( int k = 0; k < 0x10; k++ )
					{
						if( ( ( this.KeyAssign[ i ][ j ][ k ].入力デバイス == DeviceType ) && ( this.KeyAssign[ i ][ j ][ k ].ID == nID ) ) && ( this.KeyAssign[ i ][ j ][ k ].コード == nCode ) )
						{
							for( int m = k; m < 15; m++ )
							{
								this.KeyAssign[ i ][ j ][ m ] = this.KeyAssign[ i ][ j ][ m + 1 ];
							}
							this.KeyAssign[ i ][ j ][ 15 ].入力デバイス = E入力デバイス.不明;
							this.KeyAssign[ i ][ j ][ 15 ].ID = 0;
							this.KeyAssign[ i ][ j ][ 15 ].コード = 0;
							k--;
						}
					}
				}
			}
		}
		public void t書き出し( string dbファイル名)
		{
			//Change to this to write into a sqlite database instead
			//First, punish those who delete db file during gameplay
			//This forces user to restart which will re-create the db file with default values
			if (!File.Exists(dbファイル名))
			{
				throw new Exception("DB File is missing. Please do not delete the file during gameplay!!");
			}

			this.tSaveIntoDB(dbファイル名);
		}
		public void tファイルから読み込み( string dbファイル名 )
		{
			//Change this to read from a sqlite database instead
			if(File.Exists(dbファイル名))
            {
				t文字列から読み込み(dbファイル名);
			}
			else
            {
				//Re-create the DB with default values
				tRecreateConfigDB(dbファイル名);
			}
		}

		private void tRecreateConfigDB(string dbFilePath)
        {
            try
            {
				SQLiteConnection.CreateFile(dbFilePath);
				string connectionString = @"Data Source=" + dbFilePath + @";Version=3; FailIfMissing=True; Foreign Keys=True;";
				using (SQLiteConnection conn = new SQLiteConnection(connectionString))
				{
					conn.Open();

					using (SQLiteTransaction createDBTransaction = conn.BeginTransaction())
					{
						using (SQLiteCommand createDBCommand = new SQLiteCommand(conn))
						{
							createDBCommand.CommandText = @"CREATE TABLE ""dtx_profile"" (
	""profile_id""	INTEGER PRIMARY KEY,
	""drums_player_name""	TEXT,
	""drums_title""	TEXT,
	""drums_color""	INTEGER NOT NULL,
	""guitar_player_name""	TEXT,
	""guitar_title""	TEXT,
	""guitar_color""	INTEGER NOT NULL,
	""bass_player_name""	TEXT,
	""bass_title""	TEXT,
	""bass_color""	INTEGER NOT NULL
);

CREATE TABLE dtx_system_settings
(
item_id INTEGER PRIMARY KEY,
dtx_path TEXT,
curr_selected_profile_id INTEGER NOT NULL,
FOREIGN KEY(curr_selected_profile_id) REFERENCES dtx_profile(profile_id)
);

CREATE TABLE dtx_joystick
(
item_id INTEGER PRIMARY KEY,
joystick_id INTEGER NOT NULL,
joystick_guid TEXT NOT NULL,
UNIQUE(""joystick_id"")
);

CREATE TABLE dtx_profile_system_settings
(
item_id INTEGER PRIMARY KEY,
profile_id INTEGER NOT NULL,
field_name TEXT,
field_value TEXT,
UNIQUE(""profile_id"",""field_name""),
FOREIGN KEY(profile_id) REFERENCES dtx_profile(profile_id)
);

CREATE TABLE dtx_playoption
(
item_id INTEGER PRIMARY KEY,
profile_id INTEGER NOT NULL,
field_name TEXT,
field_value TEXT,
UNIQUE(""profile_id"",""field_name""),
FOREIGN KEY(profile_id) REFERENCES dtx_profile(profile_id)
);

CREATE TABLE dtx_autoplay
(
item_id INTEGER PRIMARY KEY,
profile_id INTEGER NOT NULL,
LC INTEGER NOT NULL,
HH INTEGER NOT NULL,
SD INTEGER NOT NULL,
BD INTEGER NOT NULL,
HT INTEGER NOT NULL,
LT INTEGER NOT NULL,
FT INTEGER NOT NULL,
CY INTEGER NOT NULL,
RD INTEGER NOT NULL,
LP INTEGER NOT NULL,
LBD INTEGER NOT NULL,
GuitarR INTEGER NOT NULL,
GuitarG INTEGER NOT NULL,
GuitarB INTEGER NOT NULL,
GuitarY INTEGER NOT NULL,
GuitarP INTEGER NOT NULL,
GuitarPick INTEGER NOT NULL,
GuitarWailing INTEGER NOT NULL,
BassR INTEGER NOT NULL,
BassG INTEGER NOT NULL,
BassB INTEGER NOT NULL,
BassY INTEGER NOT NULL,
BassP INTEGER NOT NULL,
BassPick INTEGER NOT NULL,
BassWailing INTEGER NOT NULL,
UNIQUE(""profile_id""),
FOREIGN KEY(profile_id) REFERENCES dtx_profile(profile_id)
);

CREATE TABLE dtx_judgement_timing
(
item_id INTEGER PRIMARY KEY,
profile_id INTEGER NOT NULL,
group_type TEXT,
perfect INTEGER NOT NULL,
great INTEGER NOT NULL,
good INTEGER NOT NULL,
poor INTEGER NOT NULL,
UNIQUE(""profile_id"",""group_type""),
FOREIGN KEY(profile_id) REFERENCES dtx_profile(profile_id)
);

CREATE TABLE dtx_key_assignment
(
item_id INTEGER PRIMARY KEY,
profile_id INTEGER NOT NULL,
LC TEXT,
HH TEXT,
HOpen TEXT,
SD TEXT,
BD TEXT,
HT TEXT,
LT TEXT,
FT TEXT,
CY TEXT,
RD TEXT,
LP TEXT,
LBD TEXT,
GuitarR TEXT,
GuitarG TEXT,
GuitarB TEXT,
GuitarY TEXT,
GuitarP TEXT,
GuitarPick TEXT,
GuitarWailing TEXT,
GuitarDecide TEXT,
BassR TEXT,
BassG TEXT,
BassB TEXT,
BassY TEXT,
BassP TEXT,
BassPick TEXT,
BassWailing TEXT,
BassDecide TEXT,
SystemCapture TEXT,
SystemHelp TEXT,
SystemPause TEXT,
UNIQUE(""profile_id""),
FOREIGN KEY(profile_id) REFERENCES dtx_profile(profile_id)
);";

							createDBCommand.ExecuteNonQuery();

							//Insert some default system values
							SQLiteCommand insertDefaultValuesCommand = new SQLiteCommand(conn);
							insertDefaultValuesCommand.CommandText = @"INSERT INTO [dtx_profile] (drums_player_name, drums_title, drums_color, guitar_player_name, guitar_title, guitar_color, bass_player_name, bass_title, bass_color) VALUES ('', '', 0, '', '', 0, '', '', 0);
INSERT INTO [dtx_system_settings] (dtx_path, curr_selected_profile_id) VALUES ('.\', 1);
INSERT INTO [dtx_autoplay] (profile_id, 
LC,
HH,
SD,
BD,
HT,
LT,
FT,
CY,
RD,
LP,
LBD,
GuitarR,
GuitarG,
GuitarB,
GuitarY,
GuitarP,
GuitarPick,
GuitarWailing,
BassR,
BassG,
BassB,
BassY,
BassP,
BassPick,
BassWailing) 
VALUES (1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

INSERT INTO [dtx_key_assignment] (profile_id, 
LC,
HH,
HOpen,
SD,
BD,
HT,
LT,
FT,
CY,
RD,
LP,
LBD,
GuitarR,
GuitarG,
GuitarB,
GuitarY,
GuitarP,
GuitarPick,
GuitarWailing,
GuitarDecide,
BassR,
BassG,
BassB,
BassY,
BassP,
BassPick,
BassWailing,
BassDecide,
SystemCapture,
SystemHelp,
SystemPause) 
VALUES (1, 'K035,K010', 'K033,M042,M093', 'K028,M046,M092', 'K012,K013,M025,M026,M027,M028,M029,M031,M032,M034,M037,M038,M040,M0113',
'K0126,K048,M033,M035,M036,M0112', 'K031,K015,M048,M050', 'K011,K016,M047', 'K023,K017,M041,M043,M045', 'K022,K019,M049,M052,M055,M057,M091',
'K047,K020,M051,M053,M059,M089', 'K087', 'K077',
'K054', 'K055,J012', 'K056', 'K057', 'K058', 'K0115,K046,J06', 'K0116', 'K060',
'K090', 'K091,J013', 'K092', 'K093', 'K094', 'K0103,K0100,J08', 'K089', 'K096',
'K065', 'K064', 'K0110'
);
";							
							insertDefaultValuesCommand.ExecuteNonQuery();

                            #region ProfileSystemSettings
                            using (SQLiteCommand insertCommand = new SQLiteCommand(conn))
                            {
								insertCommand.CommandText = @"INSERT INTO [dtx_profile_system_settings](profile_id, field_name, field_value) VALUES(@defaultprofileid, @fieldname, @field_value)";
								insertCommand.Parameters.AddWithValue("@defaultprofileid", 1);

								insertCommand.Parameters.AddWithValue("@fieldname", "MovieMode");
								insertCommand.Parameters.AddWithValue("@field_value", this.nMovieMode);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "MovieAlpha");
								insertCommand.Parameters.AddWithValue("@field_value", this.nMovieAlpha);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "InfoType");
								insertCommand.Parameters.AddWithValue("@field_value", this.nInfoType);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "SkinPath");
								insertCommand.Parameters.AddWithValue("@field_value", ".\\");
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "SkinChangeByBoxDef");
								insertCommand.Parameters.AddWithValue("@field_value", this.bUseBoxDefSkin ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "FullScreen");
								insertCommand.Parameters.AddWithValue("@field_value", this.b全画面モード ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "WindowWidth");
								insertCommand.Parameters.AddWithValue("@field_value", this.nウインドウwidth);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "WindowHeight");
								insertCommand.Parameters.AddWithValue("@field_value", this.nウインドウheight);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "WindowX");
								insertCommand.Parameters.AddWithValue("@field_value", this.n初期ウィンドウ開始位置X);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "WindowY");
								insertCommand.Parameters.AddWithValue("@field_value", this.n初期ウィンドウ開始位置Y);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DoubleClickFullScreen");
								insertCommand.Parameters.AddWithValue("@field_value", this.bIsAllowedDoubleClickFullscreen ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "EnableSystemMenu");
								insertCommand.Parameters.AddWithValue("@field_value", this.bIsEnabledSystemMenu ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BackSleep");
								insertCommand.Parameters.AddWithValue("@field_value", this.n非フォーカス時スリープms);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "VSyncWait");
								insertCommand.Parameters.AddWithValue("@field_value", this.b垂直帰線待ちを行う ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "SleepTimePerFrame");
								insertCommand.Parameters.AddWithValue("@field_value", this.nフレーム毎スリープms);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "SoundDeviceType");
								insertCommand.Parameters.AddWithValue("@field_value", this.nSoundDeviceType);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "WASAPIBufferSizeMs");
								insertCommand.Parameters.AddWithValue("@field_value", this.nWASAPIBufferSizeMs);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "ASIODevice");
								insertCommand.Parameters.AddWithValue("@field_value", this.nASIODevice);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "Guitar");
								insertCommand.Parameters.AddWithValue("@field_value", this.bGuitar有効 ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "Drums");
								insertCommand.Parameters.AddWithValue("@field_value", this.bDrums有効 ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DirectShowMode");
								insertCommand.Parameters.AddWithValue("@field_value", this.bDirectShowMode ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BGAlpha");
								insertCommand.Parameters.AddWithValue("@field_value", this.nBGAlpha);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DamageLevel");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eダメージレベル);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "StageFailed");
								insertCommand.Parameters.AddWithValue("@field_value", this.bSTAGEFAILED有効 ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "HHGroup");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eHHGroup);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "FTGroup");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eFTGroup);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "CYGroup");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eCYGroup);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BDGroup");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eBDGroup);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "HitSoundPriorityHH");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eHitSoundPriorityHH);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "HitSoundPriorityFT");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eHitSoundPriorityFT);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "HitSoundPriorityCY");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eHitSoundPriorityCY);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "HitSoundPriorityLP");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eHitSoundPriorityLP);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "CymbalFree");
								insertCommand.Parameters.AddWithValue("@field_value", this.bシンバルフリー ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "AVI");
								insertCommand.Parameters.AddWithValue("@field_value", this.bAVI有効 ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BGA");
								insertCommand.Parameters.AddWithValue("@field_value", this.bBGA有効 ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "FillInEffect");
								insertCommand.Parameters.AddWithValue("@field_value", this.bフィルイン有効 ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "AudienceSound");
								insertCommand.Parameters.AddWithValue("@field_value", this.b歓声を発声する ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "PreviewSoundWait");
								insertCommand.Parameters.AddWithValue("@field_value", this.n曲が選択されてからプレビュー音が鳴るまでのウェイトms);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "PreviewImageWait");
								insertCommand.Parameters.AddWithValue("@field_value", this.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "AdjustWaves");
								insertCommand.Parameters.AddWithValue("@field_value", this.bWave再生位置自動調整機能有効);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BGMSound");
								insertCommand.Parameters.AddWithValue("@field_value", this.bBGM音を発声する ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "HitSound");
								insertCommand.Parameters.AddWithValue("@field_value", this.bドラム打音を発声する ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "SaveScoreIni");
								insertCommand.Parameters.AddWithValue("@field_value", this.bScoreIniを出力する ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "RandomFromSubBox");
								insertCommand.Parameters.AddWithValue("@field_value", this.bランダムセレクトで子BOXを検索対象とする ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "SoundMonitorDrums");
								insertCommand.Parameters.AddWithValue("@field_value", this.b演奏音を強調する.Drums ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "SoundMonitorGuitar");
								insertCommand.Parameters.AddWithValue("@field_value", this.b演奏音を強調する.Guitar ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "SoundMonitorBass");
								insertCommand.Parameters.AddWithValue("@field_value", this.b演奏音を強調する.Bass ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "MinComboDrums");
								insertCommand.Parameters.AddWithValue("@field_value", this.n表示可能な最小コンボ数.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "MinComboGuitar");
								insertCommand.Parameters.AddWithValue("@field_value", this.n表示可能な最小コンボ数.Guitar);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "MinComboBass");
								insertCommand.Parameters.AddWithValue("@field_value", this.n表示可能な最小コンボ数.Bass);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "MusicNameDispDef");
								insertCommand.Parameters.AddWithValue("@field_value", this.b曲名表示をdefのものにする ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "ShowDebugStatus");
								insertCommand.Parameters.AddWithValue("@field_value", this.b演奏情報を表示する ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "Difficulty");
								insertCommand.Parameters.AddWithValue("@field_value", this.b難易度表示をXG表示にする ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "ShowScore");
								insertCommand.Parameters.AddWithValue("@field_value", this.bShowScore ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "ShowMusicInfo");
								insertCommand.Parameters.AddWithValue("@field_value", this.bShowMusicInfo ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DisplayFontName");
								insertCommand.Parameters.AddWithValue("@field_value", this.str曲名表示フォント);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "SelectListFontName");
								insertCommand.Parameters.AddWithValue("@field_value", this.str選曲リストフォント);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "SelectListFontSize");
								insertCommand.Parameters.AddWithValue("@field_value", this.n選曲リストフォントのサイズdot);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "SelectListFontItalic");
								insertCommand.Parameters.AddWithValue("@field_value", this.b選曲リストフォントを斜体にする ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "SelectListFontBold");
								insertCommand.Parameters.AddWithValue("@field_value", this.b選曲リストフォントを太字にする ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "ChipVolume");
								insertCommand.Parameters.AddWithValue("@field_value", this.n手動再生音量);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "AutoChipVolume");
								insertCommand.Parameters.AddWithValue("@field_value", this.n自動再生音量);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "StoicMode");
								insertCommand.Parameters.AddWithValue("@field_value", this.bストイックモード ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BufferedInput");
								insertCommand.Parameters.AddWithValue("@field_value", this.bバッファ入力を行う ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "HHOGraphics");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eHHOGraphics.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "LBDGraphics");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eLBDGraphics.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "RDPosition");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eRDPosition);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "PolyphonicSounds");
								insertCommand.Parameters.AddWithValue("@field_value", this.nPoliphonicSounds);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "ShowLagTime");
								insertCommand.Parameters.AddWithValue("@field_value", this.nShowLagType);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "ShowLagTimeColor");
								insertCommand.Parameters.AddWithValue("@field_value", this.nShowLagTypeColor);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "AutoResultCapture");
								insertCommand.Parameters.AddWithValue("@field_value", this.bIsAutoResultCapture ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "TimeStretch");
								insertCommand.Parameters.AddWithValue("@field_value", this.bTimeStretch ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "InputAdjustTimeDrums");
								insertCommand.Parameters.AddWithValue("@field_value", this.nInputAdjustTimeMs.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "InputAdjustTimeGuitar");
								insertCommand.Parameters.AddWithValue("@field_value", this.nInputAdjustTimeMs.Guitar);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "InputAdjustTimeBass");
								insertCommand.Parameters.AddWithValue("@field_value", this.nInputAdjustTimeMs.Bass);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BGMAdjustTime");
								insertCommand.Parameters.AddWithValue("@field_value", this.nCommonBGMAdjustMs);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "JudgeLinePosOffsetDrums");
								insertCommand.Parameters.AddWithValue("@field_value", this.nJudgeLinePosOffset.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "JudgeLinePosOffsetGuitar");
								insertCommand.Parameters.AddWithValue("@field_value", this.nJudgeLinePosOffset.Guitar);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "JudgeLinePosOffsetBass");
								insertCommand.Parameters.AddWithValue("@field_value", this.nJudgeLinePosOffset.Bass);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "LCVelocityMin");
								insertCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.LC);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "HHVelocityMin");
								insertCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.HH);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "SDVelocityMin");
								insertCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.SD);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BDVelocityMin");
								insertCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.BD);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "HTVelocityMin");
								insertCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.HT);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "LTVelocityMin");
								insertCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.LT);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "FTVelocityMin");
								insertCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.FT);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "CYVelocityMin");
								insertCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.CY);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "RDVelocityMin");
								insertCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.RD);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "LPVelocityMin");
								insertCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.LP);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "LBDVelocityMin");
								insertCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.LBD);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "AutoAddGage");
								insertCommand.Parameters.AddWithValue("@field_value", this.bAutoAddGage ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "OutputLog");
								insertCommand.Parameters.AddWithValue("@field_value", this.bログ出力 ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "TraceSongSearch");
								insertCommand.Parameters.AddWithValue("@field_value", this.bLog曲検索ログ出力 ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "TraceCreatedDisposed");
								insertCommand.Parameters.AddWithValue("@field_value", this.bLog作成解放ログ出力 ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "TraceDTXDetails");
								insertCommand.Parameters.AddWithValue("@field_value", this.bLogDTX詳細ログ出力 ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();
							}
							#endregion

							#region PlayOptions
							using (SQLiteCommand insertCommand = new SQLiteCommand(conn))
							{
								insertCommand.CommandText = @"INSERT INTO [dtx_playoption](profile_id, field_name, field_value) VALUES(@defaultprofileid, @fieldname, @field_value)";
								insertCommand.Parameters.AddWithValue("@defaultprofileid", 1);

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumsReverse");
								insertCommand.Parameters.AddWithValue("@field_value", this.bReverse.Drums ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "GuitarReverse");
								insertCommand.Parameters.AddWithValue("@field_value", this.bReverse.Guitar ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BassReverse");
								insertCommand.Parameters.AddWithValue("@field_value", this.bReverse.Bass ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "GuitarRandom");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eRandom.Guitar);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BassRandom");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eRandom.Bass);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "GuitarLight");
								insertCommand.Parameters.AddWithValue("@field_value", this.bLight.Guitar ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BassLight");
								insertCommand.Parameters.AddWithValue("@field_value", this.bLight.Bass ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "GuitarLeft");
								insertCommand.Parameters.AddWithValue("@field_value", this.bLeft.Guitar ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BassLeft");
								insertCommand.Parameters.AddWithValue("@field_value", this.bLeft.Bass ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "Risky");
								insertCommand.Parameters.AddWithValue("@field_value", this.nRisky);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "HAZARD");
								insertCommand.Parameters.AddWithValue("@field_value", this.bHAZARD ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumsTight");
								insertCommand.Parameters.AddWithValue("@field_value", this.bTight ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumsHiddenSudden");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.nHidSud.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "GuitarHiddenSudden");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.nHidSud.Guitar);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BassHiddenSudden");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.nHidSud.Bass);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumsPosition");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.判定文字表示位置.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "GuitarPosition");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.判定文字表示位置.Guitar);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BassPosition");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.判定文字表示位置.Bass);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumsScrollSpeed");
								insertCommand.Parameters.AddWithValue("@field_value", this.n譜面スクロール速度.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "GuitarScrollSpeed");
								insertCommand.Parameters.AddWithValue("@field_value", this.n譜面スクロール速度.Guitar);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BassScrollSpeed");
								insertCommand.Parameters.AddWithValue("@field_value", this.n譜面スクロール速度.Bass);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "PlaySpeed");
								insertCommand.Parameters.AddWithValue("@field_value", this.n演奏速度);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumGraph");
								insertCommand.Parameters.AddWithValue("@field_value", this.bGraph有効.Drums ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "GuitarGraph");
								insertCommand.Parameters.AddWithValue("@field_value", this.bGraph有効.Guitar ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BassGraph");
								insertCommand.Parameters.AddWithValue("@field_value", this.bGraph有効.Bass ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumComboDisp");
								insertCommand.Parameters.AddWithValue("@field_value", this.bドラムコンボ文字の表示 ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumAutoGhost");
								insertCommand.Parameters.AddWithValue("@field_value", (int)eAutoGhost.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "GuitarAutoGhost");
								insertCommand.Parameters.AddWithValue("@field_value", (int)eAutoGhost.Guitar);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BassAutoGhost");
								insertCommand.Parameters.AddWithValue("@field_value", (int)eAutoGhost.Bass);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumTargetGhost");
								insertCommand.Parameters.AddWithValue("@field_value", (int)eTargetGhost.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "GuitarTargetGhost");
								insertCommand.Parameters.AddWithValue("@field_value", (int)eTargetGhost.Guitar);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BassTargetGhost");
								insertCommand.Parameters.AddWithValue("@field_value", (int)eTargetGhost.Bass);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "NumOfLanes");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eNumOfLanes.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DkdkType");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eDkdkType.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "AssignToLBD");
								insertCommand.Parameters.AddWithValue("@field_value", this.bAssignToLBD.Drums ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumsRandomPad");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eRandom.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumsRandomPedal");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eRandomPedal.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "MutingLP");
								insertCommand.Parameters.AddWithValue("@field_value", this.bMutingLP ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumsJudgeLine");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.nJudgeLine.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "GuitarJudgeLine");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.nJudgeLine.Guitar);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BassJudgeLine");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.nJudgeLine.Bass);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumsShutterIn");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.nShutterInSide.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "GuitarShutterIn");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.nShutterInSide.Guitar);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BassShutterIn");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.nShutterInSide.Bass);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumsShutterOut");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.nShutterOutSide.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "GuitarShutterOut");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.nShutterOutSide.Guitar);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BassShutterOut");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.nShutterOutSide.Bass);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumsStageEffect");
								insertCommand.Parameters.AddWithValue("@field_value", this.ボーナス演出を表示する ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumsLaneType");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eLaneType.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "CLASSIC");
								insertCommand.Parameters.AddWithValue("@field_value", this.bCLASSIC譜面判別を有効にする ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "SkillMode");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.nSkillMode);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "SwitchSkillMode");
								insertCommand.Parameters.AddWithValue("@field_value", this.bSkillModeを自動切換えする ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumsAttackEffect");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eAttackEffect.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "GuitarAttackEffect");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eAttackEffect.Guitar);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BassAttackEffect");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.eAttackEffect.Bass);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumsLaneDisp");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.nLaneDisp.Drums);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "GuitarLaneDisp");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.nLaneDisp.Guitar);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BassLaneDisp");
								insertCommand.Parameters.AddWithValue("@field_value", (int)this.nLaneDisp.Bass);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumsJudgeLineDisp");
								insertCommand.Parameters.AddWithValue("@field_value", this.bJudgeLineDisp.Drums ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "GuitarJudgeLineDisp");
								insertCommand.Parameters.AddWithValue("@field_value", this.bJudgeLineDisp.Guitar ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BassJudgeLineDisp");
								insertCommand.Parameters.AddWithValue("@field_value", this.bJudgeLineDisp.Bass ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "DrumsLaneFlush");
								insertCommand.Parameters.AddWithValue("@field_value", this.bLaneFlush.Drums ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "GuitarLaneFlush");
								insertCommand.Parameters.AddWithValue("@field_value", this.bLaneFlush.Guitar ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "BassLaneFlush");
								insertCommand.Parameters.AddWithValue("@field_value", this.bLaneFlush.Bass ? 1 : 0);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "PedalLagTime");
								insertCommand.Parameters.AddWithValue("@field_value", this.nPedalLagTime);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "JudgeAnimeType");
								insertCommand.Parameters.AddWithValue("@field_value", this.nJudgeAnimeType);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "JudgeFrames");
								insertCommand.Parameters.AddWithValue("@field_value", this.nJudgeFrames);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "JudgeInterval");
								insertCommand.Parameters.AddWithValue("@field_value", this.nJudgeInterval);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "JudgeWidth");
								insertCommand.Parameters.AddWithValue("@field_value", this.nJudgeWidgh);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "JudgeHeight");
								insertCommand.Parameters.AddWithValue("@field_value", this.nJudgeHeight);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "ExplosionFrames");
								insertCommand.Parameters.AddWithValue("@field_value", this.nExplosionFrames);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "ExplosionInterval");
								insertCommand.Parameters.AddWithValue("@field_value", this.nExplosionInterval);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "ExplosionWidth");
								insertCommand.Parameters.AddWithValue("@field_value", this.nExplosionWidgh);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "ExplosionHeight");
								insertCommand.Parameters.AddWithValue("@field_value", this.nExplosionHeight);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "WailingFireFrames");
								insertCommand.Parameters.AddWithValue("@field_value", this.nWailingFireFrames);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "WailingFireInterval");
								insertCommand.Parameters.AddWithValue("@field_value", this.nWailingFireInterval);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "WailingFireWidth");
								insertCommand.Parameters.AddWithValue("@field_value", this.nWailingFireWidgh);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "WailingFireHeight");
								insertCommand.Parameters.AddWithValue("@field_value", this.nWailingFireHeight);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "WailingFirePosXGuitar");
								insertCommand.Parameters.AddWithValue("@field_value", this.nWailingFireX.Guitar);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "WailingFirePosXBass");
								insertCommand.Parameters.AddWithValue("@field_value", this.nWailingFireX.Bass);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();

								insertCommand.Parameters.AddWithValue("@fieldname", "WailingFirePosY");
								insertCommand.Parameters.AddWithValue("@field_value", this.nWailingFireY);
								//insertCommand.Prepare();
								insertCommand.ExecuteNonQuery();
							}
							#endregion

							#region JudgementTiming
							SQLiteCommand insertJudgementTimingValuesCommand = new SQLiteCommand(conn);
							insertJudgementTimingValuesCommand.CommandText = @"INSERT INTO [dtx_judgement_timing](profile_id, group_type, perfect, great, good, poor) VALUES(1, 'all', @perfect, @great, @good, @poor)";
							insertJudgementTimingValuesCommand.Parameters.AddWithValue("@perfect", this.nヒット範囲ms.Perfect);
							insertJudgementTimingValuesCommand.Parameters.AddWithValue("@great", this.nヒット範囲ms.Great);
							insertJudgementTimingValuesCommand.Parameters.AddWithValue("@good", this.nヒット範囲ms.Good);
							insertJudgementTimingValuesCommand.Parameters.AddWithValue("@poor", this.nヒット範囲ms.Poor);
							//insertProfileSystemValuesCommand.Prepare();
							insertJudgementTimingValuesCommand.ExecuteNonQuery();

							#endregion

						}
                        createDBTransaction.Commit();
					}

					conn.Close();
				}
			}
            catch (System.Data.SQLite.SQLiteException e)
            {
				Trace.TraceError(e.Message);
            }

			//Read from DB to re-initialize KeyAssignments
			this.tAssignKeysFromDBValues(dbFilePath);

		}

		private void t文字列から読み込み( string dbFilePath )	// 2011.4.13 yyagi; refactored to make initial KeyConfig easier.
		{
			//Read from SQLite DB instead
			try
			{
				string connectionString = @"Data Source=" + dbFilePath + @";Version=3; FailIfMissing=True; Foreign Keys=True;";
				using (SQLiteConnection conn = new SQLiteConnection(connectionString))
				{
					conn.Open();

					//Get the current selected profile
					using (SQLiteCommand readDBCommand = new SQLiteCommand(conn))
					{
						readDBCommand.CommandText = @"SELECT * FROM [dtx_system_settings] LIMIT 1";
						using (SQLiteDataReader rdr = readDBCommand.ExecuteReader())
						{
							if (rdr.Read())
							{
								this.str曲データ検索パス = rdr.GetString(1);
								this.nSelectedProfileID = rdr.GetInt32(2);
							}

						}
					}

					//Get the profile name, title and colors
					using (SQLiteCommand readDBCommand = new SQLiteCommand(conn))
					{
						readDBCommand.CommandText = @"SELECT * FROM [dtx_profile] where profile_id = @profileid";
						readDBCommand.Parameters.AddWithValue("@profileid", this.nSelectedProfileID);
						using (SQLiteDataReader rdr = readDBCommand.ExecuteReader())
						{
							if (rdr.Read())
							{
								this.strCardName[0] = rdr.GetString(1);
								this.strCardName[1] = rdr.GetString(4);
								this.strCardName[2] = rdr.GetString(7);

								this.strGroupName[0] = rdr.GetString(2);
								this.strGroupName[1] = rdr.GetString(5);
								this.strGroupName[2] = rdr.GetString(8);

								this.nNameColor[0] = rdr.GetInt32(3);
								this.nNameColor[1] = rdr.GetInt32(6);
								this.nNameColor[2] = rdr.GetInt32(9);
							}

						}
					}

					//Get the profile system settings
					#region ProfileSystemRead
					using (SQLiteCommand readDBCommand = new SQLiteCommand(conn))
					{
						readDBCommand.CommandText = @"SELECT * FROM [dtx_profile_system_settings] where profile_id = @profileid";
						readDBCommand.Parameters.AddWithValue("@profileid", this.nSelectedProfileID);
						using (SQLiteDataReader rdr = readDBCommand.ExecuteReader())
						{
							while (rdr.Read())
							{
								var field_name = rdr.GetString(2);

								//Incoming ugly huge list of if-else statements
								if (field_name == "MovieMode")
								{
									this.nMovieMode = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "MovieAlpha")
								{
									this.nMovieAlpha = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "InfoType")
								{
									this.nInfoType = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "SkinPath")
								{
									string absSkinPath = rdr.GetString(3);
									string str4 = absSkinPath;
									if (!System.IO.Path.IsPathRooted(str4))
									{
										absSkinPath = System.IO.Path.Combine(CDTXMania.strEXEのあるフォルダ, "System");
										absSkinPath = System.IO.Path.Combine(absSkinPath, str4);
										Uri u = new Uri(absSkinPath);
										absSkinPath = u.AbsolutePath.ToString();    // str4内に相対パスがある場合に備える
										absSkinPath = System.Web.HttpUtility.UrlDecode(absSkinPath);                        // デコードする
										absSkinPath = absSkinPath.Replace('/', System.IO.Path.DirectorySeparatorChar);  // 区切り文字が\ではなく/なので置換する
									}
									if (absSkinPath[absSkinPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)   // フォルダ名末尾に\を必ずつけて、CSkin側と表記を統一する
									{
										absSkinPath += System.IO.Path.DirectorySeparatorChar;
									}
									this.strSystemSkinSubfolderFullName = absSkinPath;
								}
								else if (field_name == "SkinChangeByBoxDef")
								{
									this.bUseBoxDefSkin = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "FullScreen")
								{
									this.b全画面モード = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "WindowWidth")
								{
									this.nウインドウwidth = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "WindowHeight")
								{
									this.nウインドウheight = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "WindowX")
								{
									this.n初期ウィンドウ開始位置X = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "WindowY")
								{
									this.n初期ウィンドウ開始位置Y = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "DoubleClickFullScreen")
								{
									this.bIsAllowedDoubleClickFullscreen = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "EnableSystemMenu")
								{
									this.bIsEnabledSystemMenu = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "BackSleep")
								{
									this.n非フォーカス時スリープms = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "VSyncWait")
								{
									this.b垂直帰線待ちを行う = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "SleepTimePerFrame")
								{
									this.nフレーム毎スリープms = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "SoundDeviceType")
								{
									this.nSoundDeviceType = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "WASAPIBufferSizeMs")
								{
									this.nWASAPIBufferSizeMs = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "ASIODevice")
								{
									string[] asiodev = CEnumerateAllAsioDevices.GetAllASIODevices();
									this.nASIODevice = C変換.n値を文字列から取得して範囲内に丸めて返す(rdr.GetString(3), 0, asiodev.Length - 1, this.nASIODevice);
								}
								else if (field_name == "Guitar")
								{
									this.bGuitar有効 = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "Drums")
								{
									this.bDrums有効 = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "DirectShowMode")
								{
									this.bDirectShowMode = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "BGAlpha")
								{
									this.nBGAlpha = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "DamageLevel")
								{
									this.eダメージレベル = (Eダメージレベル)C変換.n値を文字列から取得して範囲内に丸めて返す(rdr.GetString(3), 0, 2, (int)this.eダメージレベル);
								}
								else if (field_name == "StageFailed")
								{
									this.bSTAGEFAILED有効 = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "HHGroup")
								{
									this.eHHGroup = (EHHGroup)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "FTGroup")
								{
									this.eFTGroup = (EFTGroup)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "CYGroup")
								{
									this.eCYGroup = (ECYGroup)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "BDGroup")
								{
									this.eBDGroup = (EBDGroup)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "HitSoundPriorityHH")
								{
									this.eHitSoundPriorityHH = (E打ち分け時の再生の優先順位)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "HitSoundPriorityFT")
								{
									this.eHitSoundPriorityFT = (E打ち分け時の再生の優先順位)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "HitSoundPriorityCY")
								{
									this.eHitSoundPriorityCY = (E打ち分け時の再生の優先順位)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "HitSoundPriorityLP")
								{
									this.eHitSoundPriorityLP = (E打ち分け時の再生の優先順位)int.Parse(rdr.GetString(3));
								}
								else if (field_name.Equals("AVI"))
								{
									this.bAVI有効 = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name.Equals("BGA"))
								{
									this.bBGA有効 = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name.Equals("FillInEffect"))
								{
									this.bフィルイン有効 = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name.Equals("PreviewSoundWait"))
								{
									this.n曲が選択されてからプレビュー音が鳴るまでのウェイトms = int.Parse(rdr.GetString(3));
								}
								else if (field_name.Equals("PreviewImageWait"))
								{
									this.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "AdjustWaves")
								{
									this.bWave再生位置自動調整機能有効 = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name.Equals("BGMSound"))
								{
									this.bBGM音を発声する = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name.Equals("HitSound"))
								{
									this.bドラム打音を発声する = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name.Equals("AudienceSound"))
								{
									this.b歓声を発声する = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "SaveScoreIni")
								{
									this.bScoreIniを出力する = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "RandomFromSubBox")
								{
									this.bランダムセレクトで子BOXを検索対象とする = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name.Equals("SoundMonitorDrums"))
								{
									this.b演奏音を強調する.Drums = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name.Equals("SoundMonitorGuitar"))
								{
									this.b演奏音を強調する.Guitar = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name.Equals("SoundMonitorBass"))
								{
									this.b演奏音を強調する.Bass = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "MinComboDrums")
								{
									this.n表示可能な最小コンボ数.Drums = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "MinComboGuitar")
								{
									this.n表示可能な最小コンボ数.Guitar = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "MinComboBass")
								{
									this.n表示可能な最小コンボ数.Bass = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "MusicNameDispDef")
								{
									this.b曲名表示をdefのものにする = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "ShowDebugStatus")
								{
									this.b演奏情報を表示する = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "Difficulty")
								{
									this.b難易度表示をXG表示にする = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "ShowScore")
								{
									this.bShowScore = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "ShowMusicInfo")
								{
									this.bShowMusicInfo = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "DisplayFontName")
								{
									this.str曲名表示フォント = rdr.GetString(3);
								}
								else if (field_name == "SelectListFontName")
								{
									this.str選曲リストフォント = rdr.GetString(3);
								}
								else if (field_name == "SelectListFontSize")
								{
									this.n選曲リストフォントのサイズdot = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "SelectListFontItalic")
								{
									this.b選曲リストフォントを斜体にする = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "SelectListFontBold")
								{
									this.b選曲リストフォントを太字にする = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "ChipVolume")
								{
									this.n手動再生音量 = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "AutoChipVolume")
								{
									this.n自動再生音量 = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "StoicMode")
								{
									this.bストイックモード = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name.Equals("CymbalFree"))
								{
									this.bシンバルフリー = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "BufferedInput")
								{
									this.bバッファ入力を行う = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "HHOGraphics")
								{
									this.eHHOGraphics.Drums = (Eタイプ)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "LBDGraphics")
								{
									this.eLBDGraphics.Drums = (Eタイプ)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "RDPosition")
								{
									this.eRDPosition = (ERDPosition)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "PolyphonicSounds")
								{
									this.nPoliphonicSounds = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "ShowLagTime")
								{
									this.nShowLagType = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "ShowLagTimeColor")
								{
									this.nShowLagTypeColor = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "AutoResultCapture")
								{
									this.bIsAutoResultCapture = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "TimeStretch")
								{
									this.bTimeStretch = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name.Equals("InputAdjustTimeDrums"))       // #23580 2011.1.3 yyagi
								{
									this.nInputAdjustTimeMs.Drums = C変換.n値を文字列から取得して範囲内に丸めて返す(rdr.GetString(3), -99, 99, this.nInputAdjustTimeMs.Drums);
								}
								else if (field_name.Equals("InputAdjustTimeGuitar"))  // #23580 2011.1.3 yyagi
								{
									this.nInputAdjustTimeMs.Guitar = C変換.n値を文字列から取得して範囲内に丸めて返す(rdr.GetString(3), -99, 99, this.nInputAdjustTimeMs.Guitar);
								}
								else if (field_name.Equals("InputAdjustTimeBass"))        // #23580 2011.1.3 yyagi
								{
									this.nInputAdjustTimeMs.Bass = C変換.n値を文字列から取得して範囲内に丸めて返す(rdr.GetString(3), -99, 99, this.nInputAdjustTimeMs.Bass);
								}
								else if (field_name.Equals("BGMAdjustTime"))              // #36372 2016.06.19 kairera0467
								{
									this.nCommonBGMAdjustMs = C変換.n値を文字列から取得して範囲内に丸めて返す(rdr.GetString(3), -99, 99, this.nCommonBGMAdjustMs);
								}
								else if (field_name.Equals("JudgeLinePosOffsetDrums")) // #31602 2013.6.23 yyagi
								{
									this.nJudgeLinePosOffset.Drums = C変換.n値を文字列から取得して範囲内に丸めて返す(rdr.GetString(3), -99, 99, this.nJudgeLinePosOffset.Drums);
								}
								else if (field_name.Equals("JudgeLinePosOffsetGuitar")) // #31602 2013.6.23 yyagi
								{
									this.nJudgeLinePosOffset.Guitar = C変換.n値を文字列から取得して範囲内に丸めて返す(rdr.GetString(3), -99, 99, this.nJudgeLinePosOffset.Guitar);
								}
								else if (field_name.Equals("JudgeLinePosOffsetBass")) // #31602 2013.6.23 yyagi
								{
									this.nJudgeLinePosOffset.Bass = C変換.n値を文字列から取得して範囲内に丸めて返す(rdr.GetString(3), -99, 99, this.nJudgeLinePosOffset.Bass);
								}
								else if (field_name == "LCVelocityMin")
								{
									this.nVelocityMin.LC = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "HHVelocityMin")
								{
									this.nVelocityMin.HH = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "SDVelocityMin")
								{
									this.nVelocityMin.SD = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "BDVelocityMin")
								{
									this.nVelocityMin.BD = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "HTVelocityMin")
								{
									this.nVelocityMin.HT = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "LTVelocityMin")
								{
									this.nVelocityMin.LT = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "FTVelocityMin")
								{
									this.nVelocityMin.FT = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "CYVelocityMin")
								{
									this.nVelocityMin.CY = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "RDVelocityMin")
								{
									this.nVelocityMin.RD = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "LPVelocityMin")
								{
									this.nVelocityMin.LP = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "LBDVelocityMin")
								{
									this.nVelocityMin.LBD = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "AutoAddGage")
								{
									this.bAutoAddGage = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "OutputLog")
								{
									this.bログ出力 = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "TraceSongSearch")
								{
									this.bLog曲検索ログ出力 = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "TraceCreatedDisposed")
								{
									this.bLog作成解放ログ出力 = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "TraceDTXDetails")
								{
									this.bLogDTX詳細ログ出力 = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}

							}


						}
					}
					#endregion

					//Get the profile play options
					#region PlayOptionRead
					using (SQLiteCommand readDBCommand = new SQLiteCommand(conn))
					{
						readDBCommand.CommandText = @"SELECT * FROM [dtx_playoption] where profile_id = @profileid";
						readDBCommand.Parameters.AddWithValue("@profileid", this.nSelectedProfileID);
						using (SQLiteDataReader rdr = readDBCommand.ExecuteReader())
						{
							while (rdr.Read())
							{
								var field_name = rdr.GetString(2);

								//Incoming ugly huge list of if-else statements
								if (field_name == "DrumsReverse")
								{
									this.bReverse.Drums = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "GuitarReverse")
								{
									this.bReverse.Guitar = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "BassReverse")
								{
									this.bReverse.Bass = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "GuitarRandom")
								{
									this.eRandom.Guitar = (Eランダムモード)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "BassRandom")
								{
									this.eRandom.Bass = (Eランダムモード)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "GuitarLight")
								{
									this.bLight.Guitar = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "BassLight")
								{
									this.bLight.Bass = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "GuitarLeft")
								{
									this.bLeft.Guitar = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "BassLeft")
								{
									this.bLeft.Bass = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "Risky")
								{
									this.nRisky = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "HAZARD")
								{
									this.bHAZARD = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "DrumsTight")
								{
									this.bTight = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "DrumsHiddenSudden")
								{
									this.nHidSud.Drums = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "GuitarHiddenSudden")
								{
									this.nHidSud.Guitar = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "BassHiddenSudden")
								{
									this.nHidSud.Bass = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "DrumsPosition")
								{
									this.判定文字表示位置.Drums = (Eタイプ)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "GuitarPosition")
								{
									this.判定文字表示位置.Guitar = (Eタイプ)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "BassPosition")
								{
									this.判定文字表示位置.Bass = (Eタイプ)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "DrumsScrollSpeed")
								{
									this.n譜面スクロール速度.Drums = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "GuitarScrollSpeed")
								{
									this.n譜面スクロール速度.Guitar = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "BassScrollSpeed")
								{
									this.n譜面スクロール速度.Bass = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "PlaySpeed")
								{
									this.n演奏速度 = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "DrumGraph")
								{
									this.bGraph有効.Drums = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "GuitarGraph")
								{
									this.bGraph有効.Guitar = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "BassGraph")
								{
									this.bGraph有効.Bass = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "DrumComboDisp")
								{
									this.bドラムコンボ文字の表示 = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "DrumAutoGhost")
								{
									this.eAutoGhost.Drums = (EAutoGhostData)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "GuitarAutoGhost")
								{
									this.eAutoGhost.Guitar = (EAutoGhostData)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "BassAutoGhost")
								{
									this.eAutoGhost.Bass = (EAutoGhostData)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "DrumTargetGhost")
								{
									this.eTargetGhost.Drums = (ETargetGhostData)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "GuitarTargetGhost")
								{
									this.eTargetGhost.Guitar = (ETargetGhostData)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "BassTargetGhost")
								{
									this.eTargetGhost.Bass = (ETargetGhostData)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "NumOfLanes")
								{
									this.eNumOfLanes.Drums = (Eタイプ)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "DkdkType")
								{
									this.eDkdkType.Drums = (Eタイプ)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "AssignToLBD")
								{
									this.bAssignToLBD.Drums = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "DrumsRandomPad")
								{
									this.eRandom.Drums = (Eランダムモード)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "DrumsRandomPedal")
								{
									this.eRandomPedal.Drums = (Eランダムモード)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "MutingLP")
								{
									this.bMutingLP = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "DrumsJudgeLine")
								{
									this.nJudgeLine.Drums = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "GuitarJudgeLine")
								{
									this.nJudgeLine.Guitar = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "BassJudgeLine")
								{
									this.nJudgeLine.Bass = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "DrumsShutterIn")
								{
									this.nShutterInSide.Drums = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "GuitarShutterIn")
								{
									this.nShutterInSide.Guitar = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "BassShutterIn")
								{
									this.nShutterInSide.Bass = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "DrumsShutterOut")
								{
									this.nShutterOutSide.Drums = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "GuitarShutterOut")
								{
									this.nShutterOutSide.Guitar = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "BassShutterOut")
								{
									this.nShutterOutSide.Bass = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "DrumsStageEffect")
								{
									this.ボーナス演出を表示する = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "DrumsLaneType")
								{
									this.eLaneType.Drums = (Eタイプ)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "CLASSIC")
								{
									this.bCLASSIC譜面判別を有効にする = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "SkillMode")
								{
									this.nSkillMode = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "SwitchSkillMode")
								{
									this.bSkillModeを自動切換えする = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "DrumsAttackEffect")
								{
									this.eAttackEffect.Drums = (Eタイプ)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "GuitarAttackEffect")
								{
									this.eAttackEffect.Guitar = (Eタイプ)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "BassAttackEffect")
								{
									this.eAttackEffect.Bass = (Eタイプ)int.Parse(rdr.GetString(3));
								}
								else if (field_name == "DrumsLaneDisp")
								{
									this.nLaneDisp.Drums = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "GuitarLaneDisp")
								{
									this.nLaneDisp.Guitar = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "BassLaneDisp")
								{
									this.nLaneDisp.Bass = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "DrumsJudgeLineDisp")
								{
									this.bJudgeLineDisp.Drums = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "GuitarJudgeLineDisp")
								{
									this.bJudgeLineDisp.Guitar = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "BassJudgeLineDisp")
								{
									this.bJudgeLineDisp.Bass = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "DrumsLaneFlush")
								{
									this.bLaneFlush.Drums = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "GuitarLaneFlush")
								{
									this.bLaneFlush.Guitar = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "BassLaneFlush")
								{
									this.bLaneFlush.Bass = int.Parse(rdr.GetString(3)) == 1 ? true : false;
								}
								else if (field_name == "PedalLagTime")
								{
									this.nPedalLagTime = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "JudgeAnimeType")
								{
									this.nJudgeAnimeType = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "JudgeFrames")
								{
									this.nJudgeFrames = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "JudgeInterval")
								{
									this.nJudgeInterval = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "JudgeWidth")
								{
									this.nJudgeWidgh = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "JudgeHeight")
								{
									this.nJudgeHeight = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "ExplosionFrames")
								{
									this.nExplosionFrames = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "ExplosionInterval")
								{
									this.nExplosionInterval = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "ExplosionWidth")
								{
									this.nExplosionWidgh = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "ExplosionHeight")
								{
									this.nExplosionHeight = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "WailingFireFrames")
								{
									this.nWailingFireFrames = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "WailingFireInterval")
								{
									this.nWailingFireInterval = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "WailingFireWidth")
								{
									this.nWailingFireWidgh = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "WailingFireHeight")
								{
									this.nWailingFireHeight = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "WailingFirePosXGuitar")
								{
									this.nWailingFireX.Guitar = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "WailingFirePosXBass")
								{
									this.nWailingFireX.Bass = int.Parse(rdr.GetString(3));
								}
								else if (field_name == "WailingFirePosY")
								{
									this.nWailingFireY = int.Parse(rdr.GetString(3));
								}
							}
						}

					}
					#endregion

					//Get Judgement Timing
					using (SQLiteCommand readDBCommand = new SQLiteCommand(conn))
					{
						readDBCommand.CommandText = @"SELECT * FROM [dtx_judgement_timing] where profile_id = @profileid AND group_type = @grouptype";
						readDBCommand.Parameters.AddWithValue("@profileid", this.nSelectedProfileID);
						readDBCommand.Parameters.AddWithValue("@grouptype", "all");
						using (SQLiteDataReader rdr = readDBCommand.ExecuteReader())
						{
							if (rdr.Read())
							{
								this.nヒット範囲ms.Perfect = rdr.GetInt32(3);
								this.nヒット範囲ms.Great = rdr.GetInt32(4);
								this.nヒット範囲ms.Good = rdr.GetInt32(5);
								this.nヒット範囲ms.Poor = rdr.GetInt32(6);
							}

						}
					}

					//AutoKey settings
					using (SQLiteCommand readDBCommand = new SQLiteCommand(conn))
					{
						readDBCommand.CommandText = @"SELECT * FROM [dtx_autoplay] where profile_id = @profileid";
						readDBCommand.Parameters.AddWithValue("@profileid", this.nSelectedProfileID);
						using (SQLiteDataReader rdr = readDBCommand.ExecuteReader())
						{
							if (rdr.Read())
							{
								this.bAutoPlay.LC = rdr.GetInt32(2) == 1 ? true : false;
								this.bAutoPlay.HH = rdr.GetInt32(3) == 1 ? true : false;
								this.bAutoPlay.SD = rdr.GetInt32(4) == 1 ? true : false;
								this.bAutoPlay.BD = rdr.GetInt32(5) == 1 ? true : false;
								this.bAutoPlay.HT = rdr.GetInt32(6) == 1 ? true : false;
								this.bAutoPlay.LT = rdr.GetInt32(7) == 1 ? true : false;
								this.bAutoPlay.FT = rdr.GetInt32(8) == 1 ? true : false;
								this.bAutoPlay.CY = rdr.GetInt32(9) == 1 ? true : false;
								this.bAutoPlay.RD = rdr.GetInt32(10) == 1 ? true : false;
								this.bAutoPlay.LP = rdr.GetInt32(11) == 1 ? true : false;
								this.bAutoPlay.LBD = rdr.GetInt32(12) == 1 ? true : false;

								this.bAutoPlay.GtR = rdr.GetInt32(13) == 1 ? true : false;
								this.bAutoPlay.GtG = rdr.GetInt32(14) == 1 ? true : false;
								this.bAutoPlay.GtB = rdr.GetInt32(15) == 1 ? true : false;
								this.bAutoPlay.GtY = rdr.GetInt32(16) == 1 ? true : false;
								this.bAutoPlay.GtP = rdr.GetInt32(17) == 1 ? true : false;
								this.bAutoPlay.GtPick = rdr.GetInt32(18) == 1 ? true : false;
								this.bAutoPlay.GtW = rdr.GetInt32(19) == 1 ? true : false;

								this.bAutoPlay.BsR = rdr.GetInt32(20) == 1 ? true : false;
								this.bAutoPlay.BsG = rdr.GetInt32(21) == 1 ? true : false;
								this.bAutoPlay.BsB = rdr.GetInt32(22) == 1 ? true : false;
								this.bAutoPlay.BsY = rdr.GetInt32(23) == 1 ? true : false;
								this.bAutoPlay.BsP = rdr.GetInt32(24) == 1 ? true : false;
								this.bAutoPlay.BsPick = rdr.GetInt32(25) == 1 ? true : false;
								this.bAutoPlay.BsW = rdr.GetInt32(26) == 1 ? true : false;
							}

						}
					}

					//Retrieve Joystick GUID settings
					using (SQLiteCommand readDBCommand = new SQLiteCommand(conn))
                    {
						readDBCommand.CommandText = @"SELECT * FROM [dtx_joystick]";
						using (SQLiteDataReader rdr = readDBCommand.ExecuteReader()) 
						{
							while (rdr.Read())
                            {
								int joystick_id = rdr.GetInt32(1);
								string joystick_guid = rdr.GetString(2);
								if (this.dicJoystick.ContainsKey(joystick_id))
								{
									this.dicJoystick.Remove(joystick_id);
								}
								this.dicJoystick.Add(joystick_id, joystick_guid);
							}
						}

					}

					conn.Close();
				}
            }
			catch (System.Data.SQLite.SQLiteException e)
			{
				Trace.TraceError(e.Message);
			}

			//Key assignment
			this.tAssignKeysFromDBValues(dbFilePath);
		}

		private void tAssignKeysFromDBValues(string dbFilePath)
        {
			try
            {
				string connectionString = @"Data Source=" + dbFilePath + @";Version=3; FailIfMissing=True; Foreign Keys=True;";
				using (SQLiteConnection conn = new SQLiteConnection(connectionString))
				{
					conn.Open();

					using (SQLiteCommand readDBCommand = new SQLiteCommand(conn))
					{
						readDBCommand.CommandText = @"SELECT * FROM [dtx_key_assignment] where profile_id = @profileid";
						readDBCommand.Parameters.AddWithValue("@profileid", 1);//Always use system profileid of 1
						using (SQLiteDataReader rdr = readDBCommand.ExecuteReader())
						{
							if (rdr.Read())
							{
								this.tキーの読み出しと設定(rdr.GetString(2), this.KeyAssign.Drums.LC);
								this.tキーの読み出しと設定(rdr.GetString(3), this.KeyAssign.Drums.HH);
								this.tキーの読み出しと設定(rdr.GetString(4), this.KeyAssign.Drums.HHO);
								this.tキーの読み出しと設定(rdr.GetString(5), this.KeyAssign.Drums.SD);
								this.tキーの読み出しと設定(rdr.GetString(6), this.KeyAssign.Drums.BD);
								this.tキーの読み出しと設定(rdr.GetString(7), this.KeyAssign.Drums.HT);
								this.tキーの読み出しと設定(rdr.GetString(8), this.KeyAssign.Drums.LT);
								this.tキーの読み出しと設定(rdr.GetString(9), this.KeyAssign.Drums.FT);
								this.tキーの読み出しと設定(rdr.GetString(10), this.KeyAssign.Drums.CY);
								this.tキーの読み出しと設定(rdr.GetString(11), this.KeyAssign.Drums.RD);
								this.tキーの読み出しと設定(rdr.GetString(12), this.KeyAssign.Drums.LP);
								this.tキーの読み出しと設定(rdr.GetString(13), this.KeyAssign.Drums.LBD);

								this.tキーの読み出しと設定(rdr.GetString(14), this.KeyAssign.Guitar.R);
								this.tキーの読み出しと設定(rdr.GetString(15), this.KeyAssign.Guitar.G);
								this.tキーの読み出しと設定(rdr.GetString(16), this.KeyAssign.Guitar.B);
								this.tキーの読み出しと設定(rdr.GetString(17), this.KeyAssign.Guitar.Y);
								this.tキーの読み出しと設定(rdr.GetString(18), this.KeyAssign.Guitar.P);
								this.tキーの読み出しと設定(rdr.GetString(19), this.KeyAssign.Guitar.Pick);
								this.tキーの読み出しと設定(rdr.GetString(20), this.KeyAssign.Guitar.Wail);
								this.tキーの読み出しと設定(rdr.GetString(21), this.KeyAssign.Guitar.Decide);

								this.tキーの読み出しと設定(rdr.GetString(22), this.KeyAssign.Bass.R);
								this.tキーの読み出しと設定(rdr.GetString(23), this.KeyAssign.Bass.G);
								this.tキーの読み出しと設定(rdr.GetString(24), this.KeyAssign.Bass.B);
								this.tキーの読み出しと設定(rdr.GetString(25), this.KeyAssign.Bass.Y);
								this.tキーの読み出しと設定(rdr.GetString(26), this.KeyAssign.Bass.P);
								this.tキーの読み出しと設定(rdr.GetString(27), this.KeyAssign.Bass.Pick);
								this.tキーの読み出しと設定(rdr.GetString(28), this.KeyAssign.Bass.Wail);
								this.tキーの読み出しと設定(rdr.GetString(29), this.KeyAssign.Bass.Decide);

								this.tキーの読み出しと設定(rdr.GetString(30), this.KeyAssign.System.Capture);
								this.tキーの読み出しと設定(rdr.GetString(31), this.KeyAssign.Guitar.Help);
								this.tキーの読み出しと設定(rdr.GetString(32), this.KeyAssign.Bass.Help);


							}

						}
					}

					conn.Close();
				}
			}
			catch(System.Data.SQLite.SQLiteException e)
            {
				Trace.TraceError(e.Message);
			}
        }

		private void tSaveIntoDB(string dbFilePath)
        {
			//Update into sqlite db
			//Assumes current selected profileid already has data inserted
			try
			{
				string connectionString = @"Data Source=" + dbFilePath + @";Version=3; FailIfMissing=True; Foreign Keys=True;";
				using (SQLiteConnection conn = new SQLiteConnection(connectionString))
				{
					conn.Open();

					using (SQLiteTransaction updateDBTransaction = conn.BeginTransaction())
					{
						//Update selected profile. This is to support profile settings later
						using (SQLiteCommand updateDBCommand = new SQLiteCommand(conn))
						{
							updateDBCommand.CommandText = @"UPDATE [dtx_system_settings] SET dtx_path = @dtxpathvalue, curr_selected_profile_id = @selectedprofileid WHERE item_id=1";
							updateDBCommand.Parameters.AddWithValue("@dtxpathvalue", this.str曲データ検索パス);
							updateDBCommand.Parameters.AddWithValue("@selectedprofileid", this.nSelectedProfileID);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();
						}

						//Update profile name title colors
						using (SQLiteCommand updateDBCommand = new SQLiteCommand(conn))
						{
							updateDBCommand.CommandText = @"UPDATE [dtx_profile] SET drums_player_name = @dm_name, 
drums_title = @dm_title, drums_color = @dm_color,
guitar_player_name = @gt_name, guitar_title = @gt_title, guitar_color = @gt_color,
bass_player_name = @bs_name, bass_title = @bs_title, bass_color = @bs_color WHERE profile_id=@selectedprofileid";
							updateDBCommand.Parameters.AddWithValue("@selectedprofileid", this.nSelectedProfileID);
							updateDBCommand.Parameters.AddWithValue("@dm_name", this.strCardName[0]);
							updateDBCommand.Parameters.AddWithValue("@gt_name", this.strCardName[1]);
							updateDBCommand.Parameters.AddWithValue("@bs_name", this.strCardName[2]);
							updateDBCommand.Parameters.AddWithValue("@dm_title", this.strGroupName[0]);
							updateDBCommand.Parameters.AddWithValue("@gt_title", this.strGroupName[1]);
							updateDBCommand.Parameters.AddWithValue("@bs_title", this.strGroupName[2]);
							updateDBCommand.Parameters.AddWithValue("@dm_color", this.nNameColor[0]);
							updateDBCommand.Parameters.AddWithValue("@gt_color", this.nNameColor[1]);
							updateDBCommand.Parameters.AddWithValue("@bs_color", this.nNameColor[2]);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();
						}

						//Update profile system settings
						using (SQLiteCommand updateDBCommand = new SQLiteCommand(conn))
						{
							updateDBCommand.CommandText = @"UPDATE [dtx_profile_system_settings] SET field_value = @field_value WHERE field_name = @fieldname AND profile_id = @selectedprofileid";
							updateDBCommand.Parameters.AddWithValue("@selectedprofileid", this.nSelectedProfileID);

							updateDBCommand.Parameters.AddWithValue("@fieldname", "MovieMode");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nMovieMode);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "MovieAlpha");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nMovieAlpha);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "InfoType");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nInfoType);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "SkinPath");
							#region [ Skinパスの絶対パス→相対パス変換 ]
							Uri uriRoot = new Uri(System.IO.Path.Combine(CDTXMania.strEXEのあるフォルダ, "System" + System.IO.Path.DirectorySeparatorChar));
							Uri uriPath = new Uri(System.IO.Path.Combine(this.strSystemSkinSubfolderFullName, "." + System.IO.Path.DirectorySeparatorChar));
							string relPath = uriRoot.MakeRelativeUri(uriPath).ToString();               // 相対パスを取得
							relPath = System.Web.HttpUtility.UrlDecode(relPath);                        // デコードする
							relPath = relPath.Replace('/', System.IO.Path.DirectorySeparatorChar);  // 区切り文字が\ではなく/なので置換する
							#endregion
							updateDBCommand.Parameters.AddWithValue("@field_value", relPath);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "SkinChangeByBoxDef");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bUseBoxDefSkin ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "FullScreen");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.b全画面モード ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "WindowWidth");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nウインドウwidth);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "WindowHeight");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nウインドウheight);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "WindowX");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.n初期ウィンドウ開始位置X);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "WindowY");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.n初期ウィンドウ開始位置Y);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DoubleClickFullScreen");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bIsAllowedDoubleClickFullscreen ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "EnableSystemMenu");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bIsEnabledSystemMenu ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BackSleep");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.n非フォーカス時スリープms);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "VSyncWait");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.b垂直帰線待ちを行う ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "SleepTimePerFrame");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nフレーム毎スリープms);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "SoundDeviceType");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nSoundDeviceType);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "WASAPIBufferSizeMs");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nWASAPIBufferSizeMs);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "ASIODevice");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nASIODevice);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "Guitar");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bGuitar有効 ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "Drums");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bDrums有効 ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DirectShowMode");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bDirectShowMode ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BGAlpha");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nBGAlpha);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DamageLevel");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eダメージレベル);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "StageFailed");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bSTAGEFAILED有効 ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "HHGroup");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eHHGroup);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "FTGroup");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eFTGroup);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "CYGroup");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eCYGroup);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BDGroup");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eBDGroup);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "HitSoundPriorityHH");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eHitSoundPriorityHH);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "HitSoundPriorityFT");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eHitSoundPriorityFT);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "HitSoundPriorityCY");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eHitSoundPriorityCY);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "HitSoundPriorityLP");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eHitSoundPriorityLP);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "CymbalFree");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bシンバルフリー ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "AVI");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bAVI有効 ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BGA");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bBGA有効 ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "FillInEffect");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bフィルイン有効 ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "AudienceSound");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.b歓声を発声する ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "PreviewSoundWait");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.n曲が選択されてからプレビュー音が鳴るまでのウェイトms);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "PreviewImageWait");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "AdjustWaves");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bWave再生位置自動調整機能有効);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BGMSound");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bBGM音を発声する ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "HitSound");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bドラム打音を発声する ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "SaveScoreIni");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bScoreIniを出力する ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "RandomFromSubBox");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bランダムセレクトで子BOXを検索対象とする ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "SoundMonitorDrums");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.b演奏音を強調する.Drums ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "SoundMonitorGuitar");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.b演奏音を強調する.Guitar ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "SoundMonitorBass");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.b演奏音を強調する.Bass ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "MinComboDrums");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.n表示可能な最小コンボ数.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "MinComboGuitar");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.n表示可能な最小コンボ数.Guitar);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "MinComboBass");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.n表示可能な最小コンボ数.Bass);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "MusicNameDispDef");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.b曲名表示をdefのものにする ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "ShowDebugStatus");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.b演奏情報を表示する ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "Difficulty");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.b難易度表示をXG表示にする ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "ShowScore");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bShowScore ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "ShowMusicInfo");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bShowMusicInfo ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DisplayFontName");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.str曲名表示フォント);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "SelectListFontName");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.str選曲リストフォント);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "SelectListFontSize");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.n選曲リストフォントのサイズdot);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "SelectListFontItalic");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.b選曲リストフォントを斜体にする ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "SelectListFontBold");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.b選曲リストフォントを太字にする ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "ChipVolume");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.n手動再生音量);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "AutoChipVolume");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.n自動再生音量);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "StoicMode");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bストイックモード ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BufferedInput");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bバッファ入力を行う ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "HHOGraphics");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eHHOGraphics.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "LBDGraphics");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eLBDGraphics.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "RDPosition");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eRDPosition);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "PolyphonicSounds");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nPoliphonicSounds);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "ShowLagTime");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nShowLagType);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "ShowLagTimeColor");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nShowLagTypeColor);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "AutoResultCapture");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bIsAutoResultCapture ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "TimeStretch");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bTimeStretch ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "InputAdjustTimeDrums");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nInputAdjustTimeMs.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "InputAdjustTimeGuitar");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nInputAdjustTimeMs.Guitar);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "InputAdjustTimeBass");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nInputAdjustTimeMs.Bass);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BGMAdjustTime");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nCommonBGMAdjustMs);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "JudgeLinePosOffsetDrums");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nJudgeLinePosOffset.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "JudgeLinePosOffsetGuitar");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nJudgeLinePosOffset.Guitar);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "JudgeLinePosOffsetBass");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nJudgeLinePosOffset.Bass);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "LCVelocityMin");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.LC);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "HHVelocityMin");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.HH);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "SDVelocityMin");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.SD);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BDVelocityMin");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.BD);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "HTVelocityMin");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.HT);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "LTVelocityMin");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.LT);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "FTVelocityMin");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.FT);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "CYVelocityMin");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.CY);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "RDVelocityMin");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.RD);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "LPVelocityMin");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.LP);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "LBDVelocityMin");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nVelocityMin.LBD);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "AutoAddGage");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bAutoAddGage ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "OutputLog");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bログ出力 ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "TraceSongSearch");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bLog曲検索ログ出力 ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "TraceCreatedDisposed");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bLog作成解放ログ出力 ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "TraceDTXDetails");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bLogDTX詳細ログ出力 ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();



						}


						//Update profile play options
						using (SQLiteCommand updateDBCommand = new SQLiteCommand(conn)) 
						{
							updateDBCommand.CommandText = @"UPDATE [dtx_playoption] SET field_value = @field_value WHERE field_name = @fieldname AND profile_id = @selectedprofileid";
							updateDBCommand.Parameters.AddWithValue("@selectedprofileid", this.nSelectedProfileID);

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumsReverse");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bReverse.Drums ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "GuitarReverse");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bReverse.Guitar ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BassReverse");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bReverse.Bass ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "GuitarRandom");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eRandom.Guitar);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BassRandom");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eRandom.Bass);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "GuitarLight");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bLight.Guitar ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BassLight");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bLight.Bass ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "GuitarLeft");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bLeft.Guitar ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BassLeft");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bLeft.Bass ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "Risky");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nRisky);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "HAZARD");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bHAZARD ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumsTight");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bTight ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumsHiddenSudden");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.nHidSud.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "GuitarHiddenSudden");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.nHidSud.Guitar);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BassHiddenSudden");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.nHidSud.Bass);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumsPosition");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.判定文字表示位置.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "GuitarPosition");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.判定文字表示位置.Guitar);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BassPosition");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.判定文字表示位置.Bass);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumsScrollSpeed");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.n譜面スクロール速度.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "GuitarScrollSpeed");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.n譜面スクロール速度.Guitar);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BassScrollSpeed");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.n譜面スクロール速度.Bass);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "PlaySpeed");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.n演奏速度);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumGraph");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bGraph有効.Drums ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "GuitarGraph");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bGraph有効.Guitar ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BassGraph");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bGraph有効.Bass ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumComboDisp");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bドラムコンボ文字の表示 ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumAutoGhost");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)eAutoGhost.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "GuitarAutoGhost");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)eAutoGhost.Guitar);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BassAutoGhost");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)eAutoGhost.Bass);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumTargetGhost");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)eTargetGhost.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "GuitarTargetGhost");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)eTargetGhost.Guitar);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BassTargetGhost");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)eTargetGhost.Bass);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "NumOfLanes");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eNumOfLanes.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DkdkType");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eDkdkType.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "AssignToLBD");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bAssignToLBD.Drums ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumsRandomPad");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eRandom.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumsRandomPedal");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eRandomPedal.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "MutingLP");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bMutingLP ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumsJudgeLine");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.nJudgeLine.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "GuitarJudgeLine");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.nJudgeLine.Guitar);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BassJudgeLine");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.nJudgeLine.Bass);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumsShutterIn");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.nShutterInSide.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "GuitarShutterIn");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.nShutterInSide.Guitar);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BassShutterIn");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.nShutterInSide.Bass);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumsShutterOut");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.nShutterOutSide.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "GuitarShutterOut");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.nShutterOutSide.Guitar);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BassShutterOut");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.nShutterOutSide.Bass);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumsStageEffect");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.ボーナス演出を表示する ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumsLaneType");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eLaneType.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "CLASSIC");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bCLASSIC譜面判別を有効にする ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "SkillMode");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.nSkillMode);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "SwitchSkillMode");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bSkillModeを自動切換えする ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumsAttackEffect");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eAttackEffect.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "GuitarAttackEffect");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eAttackEffect.Guitar);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BassAttackEffect");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.eAttackEffect.Bass);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumsLaneDisp");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.nLaneDisp.Drums);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "GuitarLaneDisp");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.nLaneDisp.Guitar);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BassLaneDisp");
							updateDBCommand.Parameters.AddWithValue("@field_value", (int)this.nLaneDisp.Bass);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumsJudgeLineDisp");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bJudgeLineDisp.Drums ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "GuitarJudgeLineDisp");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bJudgeLineDisp.Guitar ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BassJudgeLineDisp");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bJudgeLineDisp.Bass ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "DrumsLaneFlush");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bLaneFlush.Drums ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "GuitarLaneFlush");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bLaneFlush.Guitar ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "BassLaneFlush");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.bLaneFlush.Bass ? 1 : 0);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "PedalLagTime");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nPedalLagTime);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "JudgeAnimeType");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nJudgeAnimeType);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "JudgeFrames");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nJudgeFrames);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "JudgeInterval");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nJudgeInterval);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "JudgeWidth");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nJudgeWidgh);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "JudgeHeight");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nJudgeHeight);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "ExplosionFrames");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nExplosionFrames);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "ExplosionInterval");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nExplosionInterval);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "ExplosionWidth");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nExplosionWidgh);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "ExplosionHeight");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nExplosionHeight);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "WailingFireFrames");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nWailingFireFrames);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "WailingFireInterval");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nWailingFireInterval);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "WailingFireWidth");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nWailingFireWidgh);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "WailingFireHeight");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nWailingFireHeight);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "WailingFirePosXGuitar");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nWailingFireX.Guitar);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "WailingFirePosXBass");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nWailingFireX.Bass);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

							updateDBCommand.Parameters.AddWithValue("@fieldname", "WailingFirePosY");
							updateDBCommand.Parameters.AddWithValue("@field_value", this.nWailingFireY);
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();

						}

						//Update Auto Key settings
						using (SQLiteCommand updateDBCommand = new SQLiteCommand(conn))
                        {
							updateDBCommand.CommandText = @"UPDATE [dtx_autoplay] SET LC = @value_LC, 
HH = @value_HH, SD = @value_SD, BD = @value_BD, HT = @value_HT, LT = @value_LT, FT = @value_FT,
CY = @value_CY, RD = @value_RD, LP = @value_LP, LBD = @value_LBD,
GuitarR = @value_GuitarR, GuitarG = @value_GuitarG, GuitarB = @value_GuitarB, GuitarY = @value_GuitarY, GuitarP = @value_GuitarP, GuitarPick = @value_GuitarPick, GuitarWailing = @value_GuitarWailing, 
BassR = @value_BassR, BassG = @value_BassG, BassB = @value_BassB, BassY = @value_BassY, BassP = @value_BassP, BassPick = @value_BassPick, BassWailing = @value_BassWailing WHERE profile_id = @selectedprofileid";
							updateDBCommand.Parameters.AddWithValue("@selectedprofileid", this.nSelectedProfileID);
							//Drum
							updateDBCommand.Parameters.AddWithValue("@value_LC", this.bAutoPlay.LC);
							updateDBCommand.Parameters.AddWithValue("@value_HH", this.bAutoPlay.HH);
							updateDBCommand.Parameters.AddWithValue("@value_SD", this.bAutoPlay.SD);
							updateDBCommand.Parameters.AddWithValue("@value_BD", this.bAutoPlay.BD);
							updateDBCommand.Parameters.AddWithValue("@value_HT", this.bAutoPlay.HT);
							updateDBCommand.Parameters.AddWithValue("@value_LT", this.bAutoPlay.LT);
							updateDBCommand.Parameters.AddWithValue("@value_FT", this.bAutoPlay.FT);
							updateDBCommand.Parameters.AddWithValue("@value_CY", this.bAutoPlay.CY);
							updateDBCommand.Parameters.AddWithValue("@value_RD", this.bAutoPlay.RD);
							updateDBCommand.Parameters.AddWithValue("@value_LP", this.bAutoPlay.LP);
							updateDBCommand.Parameters.AddWithValue("@value_LBD", this.bAutoPlay.LBD);

							//Guitar
							updateDBCommand.Parameters.AddWithValue("@value_GuitarR", this.bAutoPlay.GtR);
							updateDBCommand.Parameters.AddWithValue("@value_GuitarG", this.bAutoPlay.GtG);
							updateDBCommand.Parameters.AddWithValue("@value_GuitarB", this.bAutoPlay.GtB);
							updateDBCommand.Parameters.AddWithValue("@value_GuitarY", this.bAutoPlay.GtY);
							updateDBCommand.Parameters.AddWithValue("@value_GuitarP", this.bAutoPlay.GtP);
							updateDBCommand.Parameters.AddWithValue("@value_GuitarPick", this.bAutoPlay.GtPick);
							updateDBCommand.Parameters.AddWithValue("@value_GuitarWailing", this.bAutoPlay.GtW);

							//Bass
							updateDBCommand.Parameters.AddWithValue("@value_BassR", this.bAutoPlay.BsR);
							updateDBCommand.Parameters.AddWithValue("@value_BassG", this.bAutoPlay.BsG);
							updateDBCommand.Parameters.AddWithValue("@value_BassB", this.bAutoPlay.BsB);
							updateDBCommand.Parameters.AddWithValue("@value_BassY", this.bAutoPlay.BsY);
							updateDBCommand.Parameters.AddWithValue("@value_BassP", this.bAutoPlay.BsP);
							updateDBCommand.Parameters.AddWithValue("@value_BassPick", this.bAutoPlay.BsPick);
							updateDBCommand.Parameters.AddWithValue("@value_BassWailing", this.bAutoPlay.BsW);

							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();
						}

						//Update Key Assignments
						using (SQLiteCommand updateDBCommand = new SQLiteCommand(conn))
						{
							updateDBCommand.CommandText = @"UPDATE [dtx_key_assignment] SET LC = @value_LC, 
HH = @value_HH, HOpen = @value_HO, SD = @value_SD, BD = @value_BD, HT = @value_HT, LT = @value_LT, FT = @value_FT,
CY = @value_CY, RD = @value_RD, LP = @value_LP, LBD = @value_LBD,
GuitarR = @value_GuitarR, GuitarG = @value_GuitarG, GuitarB = @value_GuitarB, GuitarY = @value_GuitarY, GuitarP = @value_GuitarP, GuitarPick = @value_GuitarPick, GuitarWailing = @value_GuitarWailing, GuitarDecide = @value_GuitarDecide,
BassR = @value_BassR, BassG = @value_BassG, BassB = @value_BassB, BassY = @value_BassY, BassP = @value_BassP, BassPick = @value_BassPick, BassWailing = @value_BassWailing, BassDecide = @value_BassDecide,
SystemCapture = @value_SystemCapture, SystemHelp = @value_SystemHelp, SystemPause = @value_SystemPause WHERE profile_id = @selectedprofileid";
							updateDBCommand.Parameters.AddWithValue("@selectedprofileid", this.nSelectedProfileID);

							//Drum
							updateDBCommand.Parameters.AddWithValue("@value_LC", this.tConvertKeyAssignmentsToText(this.KeyAssign.Drums.LC));
							updateDBCommand.Parameters.AddWithValue("@value_HH", this.tConvertKeyAssignmentsToText(this.KeyAssign.Drums.HH));
							updateDBCommand.Parameters.AddWithValue("@value_HO", this.tConvertKeyAssignmentsToText(this.KeyAssign.Drums.HHO));
							updateDBCommand.Parameters.AddWithValue("@value_SD", this.tConvertKeyAssignmentsToText(this.KeyAssign.Drums.SD));
							updateDBCommand.Parameters.AddWithValue("@value_BD", this.tConvertKeyAssignmentsToText(this.KeyAssign.Drums.BD));
							updateDBCommand.Parameters.AddWithValue("@value_HT", this.tConvertKeyAssignmentsToText(this.KeyAssign.Drums.HT));
							updateDBCommand.Parameters.AddWithValue("@value_LT", this.tConvertKeyAssignmentsToText(this.KeyAssign.Drums.LT));
							updateDBCommand.Parameters.AddWithValue("@value_FT", this.tConvertKeyAssignmentsToText(this.KeyAssign.Drums.FT));
							updateDBCommand.Parameters.AddWithValue("@value_CY", this.tConvertKeyAssignmentsToText(this.KeyAssign.Drums.CY));
							updateDBCommand.Parameters.AddWithValue("@value_RD", this.tConvertKeyAssignmentsToText(this.KeyAssign.Drums.RD));
							updateDBCommand.Parameters.AddWithValue("@value_LP", this.tConvertKeyAssignmentsToText(this.KeyAssign.Drums.LP));
							updateDBCommand.Parameters.AddWithValue("@value_LBD", this.tConvertKeyAssignmentsToText(this.KeyAssign.Drums.LBD));

							//Guitar
							updateDBCommand.Parameters.AddWithValue("@value_GuitarR", this.tConvertKeyAssignmentsToText(this.KeyAssign.Guitar.R));
							updateDBCommand.Parameters.AddWithValue("@value_GuitarG", this.tConvertKeyAssignmentsToText(this.KeyAssign.Guitar.G));
							updateDBCommand.Parameters.AddWithValue("@value_GuitarB", this.tConvertKeyAssignmentsToText(this.KeyAssign.Guitar.B));
							updateDBCommand.Parameters.AddWithValue("@value_GuitarY", this.tConvertKeyAssignmentsToText(this.KeyAssign.Guitar.Y));
							updateDBCommand.Parameters.AddWithValue("@value_GuitarP", this.tConvertKeyAssignmentsToText(this.KeyAssign.Guitar.P));
							updateDBCommand.Parameters.AddWithValue("@value_GuitarPick", this.tConvertKeyAssignmentsToText(this.KeyAssign.Guitar.Pick));
							updateDBCommand.Parameters.AddWithValue("@value_GuitarWailing", this.tConvertKeyAssignmentsToText(this.KeyAssign.Guitar.Wail));
							updateDBCommand.Parameters.AddWithValue("@value_GuitarDecide", this.tConvertKeyAssignmentsToText(this.KeyAssign.Guitar.Decide));

							//Bass
							updateDBCommand.Parameters.AddWithValue("@value_BassR", this.tConvertKeyAssignmentsToText(this.KeyAssign.Bass.R));
							updateDBCommand.Parameters.AddWithValue("@value_BassG", this.tConvertKeyAssignmentsToText(this.KeyAssign.Bass.G));
							updateDBCommand.Parameters.AddWithValue("@value_BassB", this.tConvertKeyAssignmentsToText(this.KeyAssign.Bass.B));
							updateDBCommand.Parameters.AddWithValue("@value_BassY", this.tConvertKeyAssignmentsToText(this.KeyAssign.Bass.Y));
							updateDBCommand.Parameters.AddWithValue("@value_BassP", this.tConvertKeyAssignmentsToText(this.KeyAssign.Bass.P));
							updateDBCommand.Parameters.AddWithValue("@value_BassPick", this.tConvertKeyAssignmentsToText(this.KeyAssign.Bass.Pick));
							updateDBCommand.Parameters.AddWithValue("@value_BassWailing", this.tConvertKeyAssignmentsToText(this.KeyAssign.Bass.Wail));
							updateDBCommand.Parameters.AddWithValue("@value_BassDecide", this.tConvertKeyAssignmentsToText(this.KeyAssign.Bass.Decide));

							//System
							updateDBCommand.Parameters.AddWithValue("@value_SystemCapture", this.tConvertKeyAssignmentsToText(this.KeyAssign.System.Capture));
							updateDBCommand.Parameters.AddWithValue("@value_SystemHelp", this.tConvertKeyAssignmentsToText(this.KeyAssign.Guitar.Help));
							updateDBCommand.Parameters.AddWithValue("@value_SystemPause", this.tConvertKeyAssignmentsToText(this.KeyAssign.Bass.Help));

							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();


						}

						//Update Joystick GUID
						//First delete all entries from table
						using (SQLiteCommand updateDBCommand = new SQLiteCommand(conn))
                        {
							updateDBCommand.CommandText = @"DELETE FROM [dtx_joystick]";
							//updateDBCommand.Prepare();
							updateDBCommand.ExecuteNonQuery();
						}
						//Then re-fill using internal variable
						using (SQLiteCommand updateDBCommand = new SQLiteCommand(conn))
						{
							updateDBCommand.CommandText = @"INSERT INTO [dtx_joystick] (joystick_id, joystick_guid) VALUES(@id, @guid)";

							foreach (KeyValuePair<int, string> pair in this.dicJoystick)
							{	
								updateDBCommand.Parameters.AddWithValue("@id", pair.Key);
								updateDBCommand.Parameters.AddWithValue("@guid", pair.Value);
								//updateDBCommand.Prepare();
								updateDBCommand.ExecuteNonQuery();
							}
							
						}

						updateDBTransaction.Commit();
					}

					conn.Close();
				}

			}
            catch (System.Data.SQLite.SQLiteException e)
            {
				Trace.TraceError(e.Message);                
            }

        }

		//StringBuilder version of tキーの書き出し
		private string tConvertKeyAssignmentsToText(CKeyAssign.STKEYASSIGN[] assign)
        {
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
            for (int i = 0; i < 0x10; i++)
            {
				if (assign[i].入力デバイス == E入力デバイス.不明)
				{
					continue;
				}
				if (!flag)
				{
					stringBuilder.Append(',');
				}
				flag = false;
				switch (assign[i].入力デバイス)
				{
					case E入力デバイス.キーボード:
						stringBuilder.Append('K');
						break;

					case E入力デバイス.MIDI入力:
						stringBuilder.Append('M');
						break;

					case E入力デバイス.ジョイパッド:
						stringBuilder.Append('J');
						break;

					case E入力デバイス.マウス:
						stringBuilder.Append('N');
						break;
				}
				stringBuilder.AppendFormat("{0}{1}", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(assign[i].ID, 1), assign[i].コード);				
			}

			return stringBuilder.ToString();
        }

		/// <summary>
		/// ギターとベースのキーアサイン入れ替え
		/// </summary>
        /*
		public void SwapGuitarBassKeyAssign()		// #24063 2011.1.16 yyagi
		{
			for ( int j = 0; j <= (int)EKeyConfigPad.Capture; j++ )
			{
				CKeyAssign.STKEYASSIGN t; //= new CConfigDB.CKeyAssign.STKEYASSIGN();
				for ( int k = 0; k < 16; k++ )
				{
					t = this.KeyAssign[ (int)EKeyConfigPart.GUITAR ][ j ][ k ];
					this.KeyAssign[ (int)EKeyConfigPart.GUITAR ][ j ][ k ] = this.KeyAssign[ (int)EKeyConfigPart.BASS ][ j ][ k ];
					this.KeyAssign[ (int)EKeyConfigPart.BASS ][ j ][ k ] = t;
				}
			}
			this.bIsSwappedGuitarBass = !bIsSwappedGuitarBass;
		}
        */


		// その他

		#region [ private ]
		//-----------------
		private enum Eセクション種別
		{
			Unknown,
			System,
			Log,
			PlayOption,
			AutoPlay,
			HitRange,
			GUID,
			DrumsKeyAssign,
			GuitarKeyAssign,
			BassKeyAssign,
			SystemKeyAssign,
			Temp,
		}

		private bool _bDrums有効;
		private bool _bGuitar有効;
		private bool bConfigIniが存在している;
		private string ConfigIniファイル名;

		//private void tJoystickIDの取得( string strキー記述 )
		//{
		//	string[] strArray = strキー記述.Split( new char[] { ',' } );
		//	if( strArray.Length >= 2 )
		//	{
		//		int result = 0;
		//		if( ( int.TryParse( strArray[ 0 ], out result ) && ( result >= 0 ) ) && ( result <= 9 ) )
		//		{
		//			if( this.dicJoystick.ContainsKey( result ) )
		//			{
		//				this.dicJoystick.Remove( result );
		//			}
		//			this.dicJoystick.Add( result, strArray[ 1 ] );
		//		}
		//	}
		//}
		private void tキーアサインを全部クリアする()
		{
			this.KeyAssign = new CKeyAssign();
			for( int i = 0; i <= (int)EKeyConfigPart.SYSTEM; i++ )
			{
				for( int j = 0; j <= (int)EKeyConfigPad.Capture; j++ )
				{
					this.KeyAssign[ i ][ j ] = new CKeyAssign.STKEYASSIGN[ 16 ];
					for( int k = 0; k < 16; k++ )
					{
						this.KeyAssign[ i ][ j ][ k ] = new CKeyAssign.STKEYASSIGN( E入力デバイス.不明, 0, 0 );
					}
				}
			}
		}
		private void tキーの書き出し( StreamWriter sw, CKeyAssign.STKEYASSIGN[] assign )
		{
			bool flag = true;
			for( int i = 0; i < 0x10; i++ )
			{
				if( assign[ i ].入力デバイス == E入力デバイス.不明 )
				{
					continue;
				}
				if( !flag )
				{
					sw.Write( ',' );
				}
				flag = false;
				switch( assign[ i ].入力デバイス )
				{
					case E入力デバイス.キーボード:
						sw.Write( 'K' );
						break;

					case E入力デバイス.MIDI入力:
						sw.Write( 'M' );
						break;

					case E入力デバイス.ジョイパッド:
						sw.Write( 'J' );
						break;

					case E入力デバイス.マウス:
						sw.Write( 'N' );
						break;
				}
				sw.Write( "{0}{1}", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring( assign[ i ].ID, 1 ), assign[ i ].コード );	// #24166 2011.1.15 yyagi: to support ID > 10, change 2nd character from Decimal to 36-numeral system. (e.g. J1023 -> JA23)
			}
		}
        private void tキーの読み出しと設定(string strキー記述, CKeyAssign.STKEYASSIGN[] assign)
        {
            string[] strArray = strキー記述.Split(new char[] { ',' });
            for (int i = 0; (i < strArray.Length) && (i < 16); i++)
            {
                E入力デバイス e入力デバイス;
                int id;
                int code;
                string str = strArray[i].Trim().ToUpper();
                if (str.Length >= 3)
                {
                    e入力デバイス = E入力デバイス.不明;
                    switch (str[0])
                    {
                        case 'J':
                            e入力デバイス = E入力デバイス.ジョイパッド;
                            break;

                        case 'K':
                            e入力デバイス = E入力デバイス.キーボード;
                            break;

                        case 'L':
                            continue;

                        case 'M':
                            e入力デバイス = E入力デバイス.MIDI入力;
                            break;

                        case 'N':
                            e入力デバイス = E入力デバイス.マウス;
                            break;
                    }
                }
                else
                {
                    continue;
                }
                id = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(str[1]);	// #24166 2011.1.15 yyagi: to support ID > 10, change 2nd character from Decimal to 36-numeral system. (e.g. J1023 -> JA23)
                if (((id >= 0) && int.TryParse(str.Substring(2), out code)) && ((code >= 0) && (code <= 0xff)))
                {
                    this.t指定した入力が既にアサイン済みである場合はそれを全削除する(e入力デバイス, id, code);
                    assign[i].入力デバイス = e入力デバイス;
                    assign[i].ID = id;
                    assign[i].コード = code;
                }
            }
        }
		private void tデフォルトのキーアサインに設定する()
		{
			this.tキーアサインを全部クリアする();

			string strDefaultKeyAssign = @"
[DrumsKeyAssign]

HH=K033,M042,M093
SD=K012,K013,M025,M026,M027,M028,M029,M031,M032,M034,M037,M038,M040,M0113
BD=K0126,K048,M033,M035,M036,M0112
HT=K031,K015,M048,M050
LT=K011,K016,M047
FT=K023,K017,M041,M043,M045
CY=K022,K019,M049,M052,M055,M057,M091
HO=K028,M046,M092
RD=K047,K020,M051,M053,M059,M089
LC=K035,K010
LP=K087
LBD=K077

[GuitarKeyAssign]

R=K054
G=K055,J012
B=K056
Y=K057
P=K058
Pick=K0115,K046,J06
Wail=K0116
Decide=K060

[BassKeyAssign]

R=K090
G=K091,J013
B=K092
Y=K093
P=K094
Pick=K0103,K0100,J08
Wail=K089
Decide=K096

[SystemKeyAssign]
Capture=K065
Help=K064
Pause=K0110
";
			//t文字列から読み込み( strDefaultKeyAssign );
		}
		//-----------------
		#endregion
	}
}
