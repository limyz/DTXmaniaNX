using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using System.Threading;
using SlimDX;
using SlimDX.Direct3D9;
using FDK;

namespace DTXMania
{
	internal class CStagePerfDrumsScreen : CStagePerfCommonScreen
	{
		// Constructor

		public CStagePerfDrumsScreen()
		{
			base.eStageID = CStage.EStage.Playing;
			base.ePhaseID = CStage.EPhase.Common_DefaultState;
			base.bNotActivated = true;
			base.listChildActivities.Add( this.actPad = new CActPerfDrumsPad() );
			base.listChildActivities.Add( this.actCombo = new CActPerfDrumsComboDGB() );
			base.listChildActivities.Add( this.actDANGER = new CActPerfDrumsDanger() );
			base.listChildActivities.Add( this.actChipFireD = new CActPerfDrumsChipFireD() );
            base.listChildActivities.Add( this.actGauge = new CActPerfDrumsGauge() );
            base.listChildActivities.Add( this.actGraph = new CActPerfSkillMeter() ); // #24074 2011.01.23 add ikanick
			base.listChildActivities.Add( this.actJudgeString = new CActPerfDrumsJudgementCharacterString() );
			base.listChildActivities.Add( this.actLaneFlushD = new CActPerfDrumsLaneFlashD() );
			base.listChildActivities.Add( this.actScore = new CActPerfDrumsScore() );
			base.listChildActivities.Add( this.actStatusPanel = new CActPerfDrumsStatusPanel() );
			base.listChildActivities.Add( this.actScrollSpeed = new CActPerfScrollSpeed() );
			base.listChildActivities.Add( this.actAVI = new CActPerfAVI() );
			base.listChildActivities.Add( this.actBGA = new CActPerfBGA() );
//			base.listChildActivities.Add( this.actPanel = new CActPerfPanelString() );
			base.listChildActivities.Add( this.actStageFailed = new CActPerfStageFailure() );
			base.listChildActivities.Add( this.actPlayInfo = new CActPerformanceInformation() );
			base.listChildActivities.Add( this.actFI = new CActFIFOBlackStart() );
			base.listChildActivities.Add( this.actFO = new CActFIFOBlack() );
			base.listChildActivities.Add( this.actFOClear = new CActFIFOWhite() );
            base.listChildActivities.Add( this.actFOStageClear = new CActFIFOWhiteClear());
            base.listChildActivities.Add( this.actFillin = new CActPerfDrumsFillingEffect() );
            base.listChildActivities.Add( this.actLVFont = new CActLVLNFont() );
//          base.listChildActivities.Add( this.actChipFireGB = new CActPerfDrumsChipFireGB());
//			base.listChildActivities.Add( this.actLaneFlushGB = new CActPerfDrumsLaneFlashGB() );
//			base.listChildActivities.Add( this.actRGB = new CActPerfDrumsRGB() );
//			base.listChildActivities.Add( this.actWailingBonus = new CActPerfDrumsWailingBonus() );
//          base.listChildActivities.Add( this.actStageCleared = new CAct演奏ステージクリア());
		}


		// Methods

		public void tStorePerfResults( out CScoreIni.CPerformanceEntry Drums, out CScoreIni.CPerformanceEntry Guitar, out CScoreIni.CPerformanceEntry Bass, out CDTX.CChip[] r空打ちドラムチップ )
		{
			base.tStorePerfResults_Drums( out Drums );
			base.tStorePerfResults_Guitar( out Guitar );
			base.tStorePerfResultsBass( out Bass );

			r空打ちドラムチップ = new CDTX.CChip[ 12 ];
			for ( int i = 0; i < 12; i++ )
			{
				r空打ちドラムチップ[ i ] = this.r空うちChip( EInstrumentPart.DRUMS, (EPad) i );
				if( r空打ちドラムチップ[ i ] == null )
				{
					r空打ちドラムチップ[ i ] = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮( CDTXMania.Timer.nCurrentTime, this.nパッド0Atoチャンネル0A[ i ], this.nInputAdjustTimeMs.Drums );
				}
			}
		}


		// CStage 実装

		public override void OnActivate()
		{
			this.bInFillIn = false;
			base.OnActivate();
            CScore cScore = CDTXMania.stageSongSelection.rChosenScore;
            this.ct登場用 = new CCounter(0, 12, 16, CDTXMania.Timer);

            this.actChipFireD.iPosY = (CDTXMania.ConfigIni.bReverse.Drums ? base.nJudgeLinePosY.Drums - 183 : base.nJudgeLinePosY.Drums - 186);
            base.actPlayInfo.jl = (CDTXMania.ConfigIni.bReverse.Drums ? base.nJudgeLinePosY.Drums - 159 : CStagePerfCommonScreen.nJudgeLineMaxPosY - base.nJudgeLinePosY.Drums);

			if( CDTXMania.bCompactMode )
			{
				var score = new CScore();
				CDTXMania.SongManager.tReadScoreIniAndSetScoreInformation( CDTXMania.strCompactModeFile + ".score.ini", ref score );
				this.actGraph.dbGraphValue_Goal = score.SongInformation.HighSkill[ 0 ];
			}
			else
			{
				this.actGraph.dbGraphValue_Goal = CDTXMania.stageSongSelection.rChosenScore.SongInformation.HighSkill[ 0 ];	// #24074 2011.01.23 add ikanick
                this.actGraph.dbGraphValue_PersonalBest = CDTXMania.stageSongSelection.rChosenScore.SongInformation.HighSkill[ 0 ];

                // #35411 2015.08.21 chnmr0 add
                // ゴースト利用可のなとき、0で初期化
                if (CDTXMania.ConfigIni.eTargetGhost.Drums != ETargetGhostData.NONE)
                {
                    if (CDTXMania.listTargetGhsotLag[(int)EInstrumentPart.DRUMS] != null)
                    {
                        this.actGraph.dbGraphValue_Goal = 0;
                    }
                }
            }
            dtLastQueueOperation = DateTime.MinValue;
		}
		public override void OnDeactivate()
		{
			base.OnDeactivate();
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
                this.bChorusSection = false;
                this.bBonus = false;
                this.txチップ = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_chips_drums.png"));
				this.txヒットバー = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlayDrums hit-bar.png" ) );
                this.txシャッター = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\7_shutter.png" ) );
                this.txLaneCover = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_lanes_Cover_cls.png"));

                /*
				this.txヒットバーGB = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlayDrums hit-bar guitar.png" ) );
				this.txレーンフレームGB = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlayDrums lane parts guitar.png" ) );
                if( this.txレーンフレームGB != null )
				{
					this.txレーンフレームGB.nTransparency = 0xff - CDTXMania.ConfigIni.n背景の透過度;
				}
                 */

				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
				CDTXMania.tReleaseTexture( ref this.txヒットバー );
				CDTXMania.tReleaseTexture( ref this.txチップ );
                CDTXMania.tReleaseTexture( ref this.txLaneCover );
                CDTXMania.tReleaseTexture( ref this.txシャッター );
//				CDTXMania.tReleaseTexture( ref this.txヒットバーGB );
//				CDTXMania.tReleaseTexture( ref this.txレーンフレームGB );
                
				base.OnManagedReleaseResources();
			}
		}
		public override int OnUpdateAndDraw()
		{
			base.sw.Start();
			if( !base.bNotActivated )
            {
                this.bIsFinishedPlaying = false;
                this.bIsFinishedFadeout = false;
                this.bExc = false;
                this.bFullCom = false;
                if (base.bJustStartedUpdate)
                {
                    CSoundManager.rcPerformanceTimer.tReset();
                    CDTXMania.Timer.tReset();
                    this.actChipFireD.Start(ELane.HH, false, false, false, 0, false); // #31554 2013.6.12 yyagi

                    this.ctChipPatternAnimation.Drums = new CCounter(0, 7, 70, CDTXMania.Timer);
                    double UnitTime;
                    UnitTime = ((60.0 / (CDTXMania.stagePerfDrumsScreen.actPlayInfo.dbBPM) / 14.0));
                    this.ctBPMBar = new CCounter(1.0, 14.0, UnitTime, CSoundManager.rcPerformanceTimer);

                    this.ctComboTimer = new CCounter( 1.0, 16.0, ((60.0 / (CDTXMania.stagePerfDrumsScreen.actPlayInfo.dbBPM) / 16)), CSoundManager.rcPerformanceTimer);

                    this.ctChipPatternAnimation.Guitar = new CCounter(0, 0x17, 20, CDTXMania.Timer);
                    this.ctChipPatternAnimation.Bass = new CCounter(0, 0x17, 20, CDTXMania.Timer);
                    this.ctWailingChipPatternAnimation = new CCounter(0, 4, 50, CDTXMania.Timer);
                    base.ePhaseID = CStage.EPhase.Common_FadeIn;
                    
                    if( this.tx判定画像anime != null && this.txBonusEffect != null )
                    {
                        this.tx判定画像anime.tDraw2D( CDTXMania.app.Device, 1280, 720 );
                        this.txBonusEffect.tDraw2D( CDTXMania.app.Device, 1280, 720 );
                    }
                    this.actFI.tStartFadeIn();
                    this.ct登場用.tUpdate();
//					if ( this.bDTXVmode )
                    {
                        #region [テストコード: 再生開始小節の変更]
#if false
						int nStartBar = 30;
                        #region [ 処理を開始するチップの特定 ]
						bool bSuccessSeek = false;
						for ( int i = this.n現在のトップChip; i < CDTXMania.DTX.listChip.Count; i++ )
						{
							CDTX.CChip pChip = CDTXMania.DTX.listChip[ i ];
							if ( pChip.n発声位置 < 384 * nStartBar )
							{
								continue;
							}
							else
							{
								bSuccessSeek = true;
								this.n現在のトップChip = i;
								break;
							}
						}
						if ( !bSuccessSeek )
						{
							this.n現在のトップChip = CDTXMania.DTX.listChip.Count - 1;
						}
                        #endregion
                        #region [ 演奏開始の発声時刻msを取得し、タイマに設定 ]
						int nStartTime = CDTXMania.DTX.listChip[ this.n現在のトップChip ].n発声時刻ms;
						CSound管理.rc演奏用タイマ.n現在時刻 = nStartTime;
						CSound管理.rc演奏用タイマ.t一時停止();
                        #endregion

						List<CSound> pausedCSound = new List<CSound>();

                        #region [ BGMの途中再生開始 (CDTXのt入力_行解析_チップ配置()で小節番号が+1されているのを削っておくこと) ]
						foreach ( CDTX.CChip pChip in this.listChip )
						{
							if ( pChip.nチャンネル番号 == 0x01 )
							{
								CDTX.CWAV wc = CDTXMania.DTX.listWAV[ pChip.n整数値_内部番号 ];
								int nDuration = ( wc.rSound[ 0 ] == null ) ? 0 : (int) ( wc.rSound[ 0 ].n総演奏時間ms / CDTXMania.DTX.db再生速度 );
//								if (wc.bIsBGMSound || wc.bIsGuitarSound || wc.bIsBassSound || wc.bIsBGMSound || wc.bIsSESound )
								{
									if ( ( pChip.n発声時刻ms + nDuration > 0 ) && ( pChip.n発声時刻ms <= nStartTime ) && ( nStartTime <= pChip.n発声時刻ms + nDuration ) )
									{
										if ( CDTXMania.ConfigIni.bBGM音を発声する )
										{
											CDTXMania.DTX.tチップの再生( pChip, CSound管理.rc演奏用タイマ.n前回リセットした時のシステム時刻 + pChip.n発声時刻ms, (int) Eレーン.BGM, CDTXMania.DTX.nモニタを考慮した音量( E楽器パート.UNKNOWN ) );
											//CDTXMania.DTX.tチップの再生( pChip, CSound管理.rc演奏用タイマ.n現在時刻ms + pChip.n発声時刻ms, (int) Eレーン.BGM, CDTXMania.DTX.nモニタを考慮した音量( E楽器パート.UNKNOWN ) );
											for ( int i = 0; i < wc.rSound.Length; i++ )
											{
												if ( wc.rSound[ i ] != null )
												{
													wc.rSound[ i ].t再生を一時停止する();
													wc.rSound[ i ].t再生位置を変更する( nStartTime - pChip.n発声時刻ms );
													pausedCSound.Add( wc.rSound[ i ] );
												}
											}
										}
										break;
									}
								}
							}
						}
                        #endregion
// 以下未実装 ここから
                        #region [ 演奏開始時点で既に演奏中になっているチップの再生とシーク (一つ手前のBGM処理のところに混ぜてもいいかも  ]
                        #endregion
                        #region [ 演奏開始時点で既に表示されているBGAの再生とシーク (BGAの動きの途中状況を反映すること) ]
                        #endregion
                        #region [ 演奏開始時点で既に表示されているAVIの再生とシーク (AVIの動きの途中状況を反映すること) ]
                        #endregion

// 未実装 ここまで
                        #region [ PAUSEしていたサウンドを一斉に再生再開する ]
						foreach ( CSound cs in pausedCSound )
						{
							cs.tサウンドを再生する();
						}
						pausedCSound.Clear();
						pausedCSound = null;
                        #endregion
						CSound管理.rc演奏用タイマ.n現在時刻 = nStartTime;
						CSound管理.rc演奏用タイマ.t再開();
#endif
                        #endregion
                    }

                    base.bJustStartedUpdate = false;
                }

                if ((CDTXMania.ConfigIni.bSTAGEFAILEDEnabled && this.actGauge.IsFailed(EInstrumentPart.DRUMS)) && (base.ePhaseID == CStage.EPhase.Common_DefaultState))
                {
                    this.actStageFailed.Start();
                    CDTXMania.DTX.tStopPlayingAllChips();
                    base.ePhaseID = CStage.EPhase.演奏_STAGE_FAILED;
                }
                this.tUpdateAndDraw_Background();
                this.tUpdateAndDraw_MIDIBGM();
                this.tUpdateAndDraw_AVI();
                this.tUpdateAndDraw_LaneFlushD();
                this.tUpdateAndDraw_ScrollSpeed();
                this.tUpdateAndDraw_ChipAnimation();
                this.tUpdateAndDraw_BarLine( EInstrumentPart.DRUMS );
                this.tUpdateAndDraw_Chip_PatternOnly( EInstrumentPart.DRUMS );
                bIsFinishedPlaying = this.tUpdateAndDraw_Chip( EInstrumentPart.DRUMS );
                #region[ シャッター ]
                //シャッターを使うのはLC、LP、FT、RDレーンのみ。その他のレーンでは一切使用しない。
                //If Skill Mode is CLASSIC, always display lvl as Classic Style
                if (CDTXMania.ConfigIni.nSkillMode == 0 || ((CDTXMania.ConfigIni.bCLASSIC譜面判別を有効にする == true ) && 
                    ((CDTXMania.DTX.bチップがある.LeftCymbal == false) && 
                    ( CDTXMania.DTX.bチップがある.FT == false ) && 
                    ( CDTXMania.DTX.bチップがある.Ride == false ) && 
                    ( CDTXMania.DTX.bチップがある.LP == false ) &&
                    ( CDTXMania.DTX.bチップがある.LBD == false) &&
                    ( CDTXMania.DTX.b強制的にXG譜面にする == false))) )
                {
                    if ( this.txLaneCover != null )
                    {
                        //旧画像
                        //this.txLaneCover.tDraw2D(CDTXMania.app.Device, 295, 0);
                        //if (CDTXMania.DTX.bチップがある.LeftCymbal == false)
                        {
                            this.txLaneCover.tDraw2D(CDTXMania.app.Device, 295, 0, new Rectangle(0, 0, 70, 720));
                        }
                        //if ((CDTXMania.DTX.bチップがある.LP == false) && (CDTXMania.DTX.bチップがある.LBD == false))
                        {
                            //レーンタイプでの入れ替わりあり
                            if (CDTXMania.ConfigIni.eLaneType.Drums == EType.A || CDTXMania.ConfigIni.eLaneType.Drums == EType.C)
                            {
                                this.txLaneCover.tDraw2D(CDTXMania.app.Device, 416, 0, new Rectangle(124, 0, 54, 720));
                            }
                            else if (CDTXMania.ConfigIni.eLaneType.Drums == EType.B)
                            {
                                this.txLaneCover.tDraw2D(CDTXMania.app.Device, 470, 0, new Rectangle(124, 0, 54, 720));
                            }
                            else if (CDTXMania.ConfigIni.eLaneType.Drums == EType.D)
                            {
                                this.txLaneCover.tDraw2D(CDTXMania.app.Device, 522, 0, new Rectangle(124, 0, 54, 720));
                            }
                        }
                        //if (CDTXMania.DTX.bチップがある.FT == false)
                        {
                            this.txLaneCover.tDraw2D(CDTXMania.app.Device, 690, 0, new Rectangle(71, 0, 52, 720));
                        }
                        //if (CDTXMania.DTX.bチップがある.Ride == false)
                        {
                            //RDPositionで入れ替わり
                            if (CDTXMania.ConfigIni.eRDPosition == ERDPosition.RCRD)
                            {
                                this.txLaneCover.tDraw2D(CDTXMania.app.Device, 815, 0, new Rectangle(178, 0, 38, 720));
                            }
                            else if (CDTXMania.ConfigIni.eRDPosition == ERDPosition.RDRC)
                            {
                                this.txLaneCover.tDraw2D(CDTXMania.app.Device, 743, 0, new Rectangle(178, 0, 38, 720));
                            }
                        }
                    }
                }

                double db倍率 = 7.2;
                double dbシャッターIN = (base.nShutterInPosY.Drums * db倍率);
                double dbシャッターOUT = 720 - (base.nShutterOutPosY.Drums * db倍率);

                if ( CDTXMania.ConfigIni.bReverse.Drums )
                {
                    dbシャッターIN = (base.nShutterOutPosY.Drums * db倍率);
                    this.txシャッター.tDraw2D(CDTXMania.app.Device, 295, (int)(-720 + dbシャッターIN));

                    if (CDTXMania.ConfigIni.b演奏情報を表示する)
                        this.actLVFont.tDrawString(564, (int)dbシャッターIN - 20, CDTXMania.ConfigIni.nShutterOutSide.Drums.ToString());

                    dbシャッターOUT = 720 - (base.nShutterInPosY.Drums * db倍率);
                    this.txシャッター.tDraw2D(CDTXMania.app.Device, 295, (int)dbシャッターOUT);

                    if (CDTXMania.ConfigIni.b演奏情報を表示する)
                        this.actLVFont.tDrawString(564, (int)dbシャッターOUT + 2, CDTXMania.ConfigIni.nShutterInSide.Drums.ToString());
                }
                else
                {
                    this.txシャッター.tDraw2D(CDTXMania.app.Device, 295, (int)(-720 + dbシャッターIN));

                    if (CDTXMania.ConfigIni.b演奏情報を表示する)
                        this.actLVFont.tDrawString(564, (int)dbシャッターIN - 20, CDTXMania.ConfigIni.nShutterInSide.Drums.ToString());

                    this.txシャッター.tDraw2D(CDTXMania.app.Device, 295, (int)dbシャッターOUT);

                    if (CDTXMania.ConfigIni.b演奏情報を表示する)
                        this.actLVFont.tDrawString(564, (int)dbシャッターOUT + 2, CDTXMania.ConfigIni.nShutterOutSide.Drums.ToString());
                }

                #endregion
                this.t進行描画_判定ライン();
                this.t進行描画_ドラムパッド();
                bIsFinishedFadeout = this.tUpdateAndDraw_FadeIn_Out();
                if (bIsFinishedPlaying && (base.ePhaseID == CStage.EPhase.Common_DefaultState) )
                {
                    if ((this.actGauge.IsFailed(EInstrumentPart.DRUMS)) && (base.ePhaseID == CStage.EPhase.Common_DefaultState))
                    {
                        this.actStageFailed.Start();
                        CDTXMania.DTX.tStopPlayingAllChips();
                        base.ePhaseID = CStage.EPhase.演奏_STAGE_FAILED;
                    }
                    else
                    {
                        this.eReturnValueAfterFadeOut = EPerfScreenReturnValue.StageClear;
                        base.ePhaseID = CStage.EPhase.演奏_STAGE_CLEAR_フェードアウト;
                        if (base.nHitCount_ExclAuto.Drums.Miss + base.nHitCount_ExclAuto.Drums.Poor == 0)
                        {
                            this.nNumberPerfects = CDTXMania.ConfigIni.bAllDrumsAreAutoPlay ? this.nNumberPerfects = base.nHitCount_IncAuto.Drums.Perfect : base.nHitCount_ExclAuto.Drums.Perfect;
                            if (nNumberPerfects == CDTXMania.DTX.nVisibleChipsCount.Drums)
                            #region[ エクセ ]
                            {
                            }
                            #endregion
                            else
                            #region[ フルコン ]
                            {
                            }
                            #endregion
                        }
                        else
                        {

                        }
                        this.actFOStageClear.tStartFadeOut();
                    }
                }
                if( CDTXMania.ConfigIni.bShowScore )
                    this.tUpdateAndDraw_Score();
//              if( CDTXMania.ConfigIni.bShowMusicInfo )
//                  this.t進行描画_パネル文字列();
                if (CDTXMania.ConfigIni.nInfoType == 1)
                    this.tUpdateAndDraw_StatusPanel();
                this.tUpdateAndDraw_Gauge();
                this.tUpdateAndDraw_Combo();
                this.t進行描画_グラフ();
                this.tUpdateAndDraw_PerformanceInformation();
                this.t進行描画_判定文字列1_通常位置指定の場合();
                this.t進行描画_判定文字列2_判定ライン上指定の場合();
                this.t進行描画_チップファイアD();
                this.tUpdateAndDraw_STAGEFAILED();
                bすべてのチップが判定された = true;
                if (bIsFinishedFadeout)
                {
                    if (!CDTXMania.Skin.soundStageClear.b再生中 && !CDTXMania.Skin.soundSTAGEFAILED音.b再生中)
                    {
                        Debug.WriteLine("Total OnUpdateAndDraw=" + sw.ElapsedMilliseconds + "ms");
                        this.nNumberOfMistakes = base.nHitCount_ExclAuto.Drums.Miss + base.nHitCount_ExclAuto.Drums.Poor;
                        switch (nNumberOfMistakes)
                        {
                            case 0:
                                {
                                    this.nNumberPerfects = base.nHitCount_ExclAuto.Drums.Perfect;
                                    if (CDTXMania.ConfigIni.bAllDrumsAreAutoPlay)
                                    {
                                        this.nNumberPerfects = base.nHitCount_IncAuto.Drums.Perfect;
                                    }
                                    if (nNumberPerfects == CDTXMania.DTX.nVisibleChipsCount.Drums)
                                    #region[ エクセ ]
                                    {
                                        this.bExc = true;
                                        if (CDTXMania.ConfigIni.nSkillMode == 1)
                                            this.actScore.nCurrentTrueScore.Drums += 30000;
                                        break;
                                    }
                                    #endregion
                                    else
                                    #region[ フルコン ]
                                    {
                                        this.bFullCom = true;
                                        if (CDTXMania.ConfigIni.nSkillMode == 1)
                                            this.actScore.nCurrentTrueScore.Drums += 15000;
                                        break;
                                    }
                                    #endregion
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        return (int)this.eReturnValueAfterFadeOut;
                    }
                }

                // もしサウンドの登録/削除が必要なら、実行する
                if (queueMixerSound.Count > 0)
                {
                    //Debug.WriteLine( "☆queueLength=" + queueMixerSound.Count );
                    DateTime dtnow = DateTime.Now;
                    TimeSpan ts = dtnow - dtLastQueueOperation;
                    if (ts.Milliseconds > 7)
                    {
                        for (int i = 0; i < 2 && queueMixerSound.Count > 0; i++)
                        {
                            dtLastQueueOperation = dtnow;
                            stmixer stm = queueMixerSound.Dequeue();
                            if (stm.bIsAdd)
                            {
                                CDTXMania.SoundManager.AddMixer(stm.csound);
                            }
                            else
                            {
                                CDTXMania.SoundManager.RemoveMixer(stm.csound);
                            }
                        }
                    }
                }
                // キー入力

                if (CDTXMania.act現在入力を占有中のプラグイン == null)
                    this.tHandleKeyInput();
            }
			base.sw.Stop();
			return 0;
		}




		// Other

		#region [ private ]
		//-----------------
        public bool bIsFinishedFadeout;
        public bool bIsFinishedPlaying;
        public bool bExc;
        public bool bFullCom;
        public bool bすべてのチップが判定された;
        public int nNumberOfMistakes;
        public int nNumberPerfects;
		private CActPerfDrumsChipFireD actChipFireD;
		public CActPerfDrumsPad actPad;
		public bool bInFillIn;
        public bool bEndFillIn;
        public bool bChorusSection;
        public bool bBonus;
		private readonly EPad[] eChannelToPad = new EPad[12]
		{
			EPad.HH, EPad.SD, EPad.BD, EPad.HT,
			EPad.LT, EPad.CY, EPad.FT, EPad.HHO,
			EPad.RD, EPad.UNKNOWN, EPad.UNKNOWN, EPad.LC
		};
        private int[] nチャンネルtoX座標 = new int[] { 370, 470, 582, 527, 645, 748, 694, 373, 815, 298, 419, 419 };
        private int[] nチャンネルtoX座標B = new int[] { 370, 419, 533, 596, 645, 748, 694, 373, 815, 298, 476, 476 };
        private int[] nチャンネルtoX座標C = new int[] { 370, 470, 533, 596, 645, 748, 694, 373, 815, 298, 419, 419 };
        private int[] nチャンネルtoX座標D = new int[] { 370, 419, 582, 476, 645, 748, 694, 373, 815, 298, 525, 525 };
        private int[] nチャンネルtoX座標改 = new int[] { 370, 470, 582, 527, 645, 786, 694, 373, 746, 298, 419, 419 };
        private int[] nチャンネルtoX座標B改 = new int[] { 370, 419, 533, 596, 645, 786, 694, 373, 746, 298, 476, 476 };
        private int[] nチャンネルtoX座標C改 = new int[] { 370, 470, 533, 596, 644, 786, 694, 373, 746, 298, 419, 419 };
        private int[] nチャンネルtoX座標D改 = new int[] { 370, 419, 582, 476, 645, 786, 694, 373, 746, 298, 527, 527 };

        private int[] nボーナスチャンネルtoX座標 = new int[] { 0, 298, 370, 419, 470, 527, 582, 645, 694, 748, 815, 0 };
        private int[] nボーナスチャンネルtoX座標B = new int[] { 0, 298, 370, 476, 419, 596, 533, 645, 694, 748, 815, 476 };
        private int[] nボーナスチャンネルtoX座標C = new int[] { 0, 298, 370, 419, 470, 596, 533, 645, 694, 748, 815, 419 };
        private int[] nボーナスチャンネルtoX座標D = new int[] { 0, 298, 370, 527, 420, 477, 582, 645, 694, 748, 815, 527 };
        private int[] nボーナスチャンネルtoX座標改 = new int[] { 0, 298, 370, 419, 470, 527, 582, 645, 694, 786, 748, 419 };
        private int[] nボーナスチャンネルtoX座標B改 = new int[] { 0, 298, 370, 476, 419, 596, 533, 645, 694, 786, 748, 476 };
        private int[] nボーナスチャンネルtoX座標C改 = new int[] { 0, 298, 370, 419, 470, 596, 533, 645, 694, 786, 748, 419 };
        private int[] nボーナスチャンネルtoX座標D改 = new int[] { 0, 298, 370, 527, 420, 477, 582, 645, 694, 786, 748, 527 };
        //HH SD BD HT LT CY FT HHO RD LC LP LBD
        //レーンタイプB
        //LC 298  HH 371 HHO 374  SD 420  LP 477  BD 534  HT 597 LT 646  FT 695  CY 749  RD 815
        //レーンタイプC

        public double UnitTime;
//		private CTexture txヒットバーGB;
//		private CTexture txレーンフレームGB;
        public CTexture txシャッター;
        private CTexture txLaneCover;
		//-----------------

        private void tFadeOut()
        {
            this.eReturnValueAfterFadeOut = EPerfScreenReturnValue.StageClear;
            base.ePhaseID = CStage.EPhase.演奏_STAGE_CLEAR_フェードアウト;

            this.actFOStageClear.tStartFadeOut();
        }

		private bool bフィルイン区間の最後のChipである( CDTX.CChip pChip )
		{
			if( pChip == null )
			{
				return false;
			}
			int num = pChip.nPlaybackPosition;
            for (int i = listChip.IndexOf(pChip) + 1; i < listChip.Count; i++)
			{
                pChip = listChip[i];
				if( ( pChip.nChannelNumber == 0x53 ) && ( pChip.nIntegerValue == 2 ) )
				{
					return true;
				}
				if( ( ( pChip.nChannelNumber >= 0x11 ) && ( pChip.nChannelNumber <= 0x1c ) ) && ( ( pChip.nPlaybackPosition - num ) > 0x18 ) )
				{
					return false;
				}
			}
			return true;
		}

		protected override EJudgement tProcessChipHit( long nHitTime, CDTX.CChip pChip, bool bCorrectLane )
		{
			EJudgement eJudgeResult = tProcessChipHit( nHitTime, pChip, EInstrumentPart.DRUMS, bCorrectLane );
			// #24074 2011.01.23 add ikanick
            if (CDTXMania.ConfigIni.nSkillMode == 0)
            {
                this.actGraph.dbグラフ値現在_渡 = CScoreIni.tCalculatePlayingSkillOld(CDTXMania.DTX.nVisibleChipsCount.Drums, this.nHitCount_ExclAuto.Drums.Perfect, this.nHitCount_ExclAuto.Drums.Great, this.nHitCount_ExclAuto.Drums.Good, this.nHitCount_ExclAuto.Drums.Poor, this.nHitCount_ExclAuto.Drums.Miss, this.actCombo.nCurrentCombo.HighestValue.Drums, EInstrumentPart.DRUMS, bIsAutoPlay);
            }
            else if (CDTXMania.ConfigIni.nSkillMode == 1)
            {
                this.actGraph.dbグラフ値現在_渡 = CScoreIni.tCalculatePlayingSkill(CDTXMania.DTX.nVisibleChipsCount.Drums, this.nHitCount_ExclAuto.Drums.Perfect, this.nHitCount_ExclAuto.Drums.Great, this.nHitCount_ExclAuto.Drums.Good, this.nHitCount_ExclAuto.Drums.Poor, this.nHitCount_ExclAuto.Drums.Miss, this.actCombo.nCurrentCombo.HighestValue.Drums, EInstrumentPart.DRUMS, bIsAutoPlay);
            }
			// #35411 2015.09.07 add chnmr0
			if( CDTXMania.listTargetGhsotLag.Drums != null &&
                CDTXMania.ConfigIni.eTargetGhost.Drums == ETargetGhostData.ONLINE &&
				CDTXMania.DTX.nVisibleChipsCount.Drums > 0 )
			{
				// Online Stats の計算式
				this.actGraph.dbグラフ値現在_渡 = 100 *
								( this.nHitCount_ExclAuto.Drums.Perfect * 17 +
								 this.nHitCount_ExclAuto.Drums.Great * 7 +
								 this.actCombo.nCurrentCombo.HighestValue.Drums * 3 ) / ( 20.0 * CDTXMania.DTX.nVisibleChipsCount.Drums );
			}

            this.actStatusPanel.db現在の達成率.Drums = this.actGraph.dbグラフ値現在_渡;
			return eJudgeResult;
		}

		protected override void tチップのヒット処理_BadならびにTight時のMiss( EInstrumentPart part )
		{
			this.tチップのヒット処理_BadならびにTight時のMiss( part, 0, EInstrumentPart.DRUMS );
		}
		protected override void tチップのヒット処理_BadならびにTight時のMiss( EInstrumentPart part, int nLane )
		{
			this.tチップのヒット処理_BadならびにTight時のMiss( part, nLane, EInstrumentPart.DRUMS );
		}

        protected override void tJudgeLineMovingUpandDown()
        {
            this.actJudgeString.iP_A = (CDTXMania.ConfigIni.bReverse.Drums ? base.nJudgeLinePosY.Drums : base.nJudgeLinePosY.Drums - 189);
            this.actJudgeString.iP_B = (CDTXMania.ConfigIni.bReverse.Drums ? base.nJudgeLinePosY.Drums : base.nJudgeLinePosY.Drums + 23);
            this.actChipFireD.iPosY = (CDTXMania.ConfigIni.bReverse.Drums ? base.nJudgeLinePosY.Drums - 183 : base.nJudgeLinePosY.Drums - 186);
            CDTXMania.stagePerfDrumsScreen.actPlayInfo.jl = (CDTXMania.ConfigIni.bReverse.Drums ? 0 : CStagePerfCommonScreen.nJudgeLineMaxPosY - base.nJudgeLinePosY.Drums);
        }

		private bool tドラムヒット処理( long nHitTime, EPad type, CDTX.CChip pChip, int n強弱度合い0to127 )
		{
			if( pChip == null )
			{
				return false;
			}
			int index = pChip.nChannelNumber;
			if ( ( index >= 0x11 ) && ( index <= 0x1c ) )
			{
				index -= 0x11;
			}
			else if ( ( index >= 0x31 ) && ( index <= 60 ) )
			{
				index -= 0x31;
			}
			int nLane = this.nチャンネル0Atoレーン07[ index ];
			int nPad = this.nチャンネル0Atoパッド08[ index ];
			bool bPChipIsAutoPlay = bIsAutoPlay[ nLane ];
			int nInputAdjustTime = bPChipIsAutoPlay ? 0 : this.nInputAdjustTimeMs.Drums;
			EJudgement e判定 = this.e指定時刻からChipのJUDGEを返す( nHitTime, pChip, nInputAdjustTime );
			if( e判定 == EJudgement.Miss )
			{
				return false;
			}
			this.tProcessChipHit( nHitTime, pChip );
			this.actLaneFlushD.Start( (ELane) nLane, ( (float) n強弱度合い0to127 ) / 127f );
			this.actPad.Hit( nPad );
			if( ( e判定 != EJudgement.Poor ) && ( e判定 != EJudgement.Miss ) )
			{
				bool flag = this.bInFillIn;
				bool flag2 = this.bInFillIn && this.bフィルイン区間の最後のChipである( pChip );
                this.actChipFireD.Start( (ELane)nLane, flag, flag2, flag2, nJudgeLinePosY_delta.Drums );
                // #31602 2013.6.24 yyagi 判定ラインの表示位置をずらしたら、チップのヒットエフェクトの表示もずらすために、nJudgeLine..を追加
			}
			if( CDTXMania.ConfigIni.bドラム打音を発声する )
			{
				CDTX.CChip rChip = null;
				bool bIsChipsoundPriorToPad = true;
				if( ( ( type == EPad.HH ) || ( type == EPad.HHO ) ) || ( type == EPad.LC ) )
				{
					bIsChipsoundPriorToPad = CDTXMania.ConfigIni.eHitSoundPriorityHH == EPlaybackPriority.ChipOverPadPriority;
				}
				else if( ( type == EPad.LT ) || ( type == EPad.FT ) )
				{
					bIsChipsoundPriorToPad = CDTXMania.ConfigIni.eHitSoundPriorityFT == EPlaybackPriority.ChipOverPadPriority;
				}
				else if( ( type == EPad.CY ) || ( type == EPad.RD ) )
				{
					bIsChipsoundPriorToPad = CDTXMania.ConfigIni.eHitSoundPriorityCY == EPlaybackPriority.ChipOverPadPriority;
				}
                else if (((type == EPad.LP) || (type == EPad.LBD)) || (type == EPad.BD))
                {
                    bIsChipsoundPriorToPad = CDTXMania.ConfigIni.eHitSoundPriorityLP == EPlaybackPriority.ChipOverPadPriority;
                }

				if( bIsChipsoundPriorToPad )
				{
					rChip = pChip;
				}
				else
				{
					EPad hH = type;
					if( !CDTXMania.DTX.bチップがある.HHOpen && ( type == EPad.HHO ) )
					{
						hH = EPad.HH;
					}
					if( !CDTXMania.DTX.bチップがある.Ride && ( type == EPad.RD ) )
					{
						hH = EPad.CY;
					}
					if( !CDTXMania.DTX.bチップがある.LeftCymbal && ( type == EPad.LC ) )
					{
						hH = EPad.HH;
					}
					rChip = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮( nHitTime, this.nパッド0Atoチャンネル0A[ (int) hH ], nInputAdjustTime );
					if( rChip == null )
					{
						rChip = pChip;
					}
				}
				this.tPlaySound( rChip, CSoundManager.rcPerformanceTimer.nシステム時刻, EInstrumentPart.DRUMS, CDTXMania.ConfigIni.n手動再生音量, CDTXMania.ConfigIni.b演奏音を強調する.Drums );
			}
			return true;
		}

		protected override void ScrollSpeedUp()
		{
			CDTXMania.ConfigIni.nScrollSpeed.Drums = Math.Min( CDTXMania.ConfigIni.nScrollSpeed.Drums + 1, 1999 );
		}
		protected override void ScrollSpeedDown()
		{
			CDTXMania.ConfigIni.nScrollSpeed.Drums = Math.Max( CDTXMania.ConfigIni.nScrollSpeed.Drums - 1, 0 );
		}
	
        /*
		protected override void tUpdateAndDraw_AVI()
		{
			base.tUpdateAndDraw_AVI( 0, 0 );
		}
		protected override void t進行描画_BGA()
		{
			base.t進行描画_BGA( 990, 0 );
		}
         */
		protected override void tUpdateAndDraw_DANGER()
		{
			this.actDANGER.t進行描画( this.actGauge.IsDanger(EInstrumentPart.DRUMS), false, false );
		}

		protected override void tUpdateAndDraw_WailingFrame()
		{
			base.tUpdateAndDraw_WailingFrame( 587, 478,
				CDTXMania.ConfigIni.bReverse.Guitar ? ( 400 - this.txWailingFrame.szImageSize.Height ) : 69,
				CDTXMania.ConfigIni.bReverse.Bass ? ( 400 - this.txWailingFrame.szImageSize.Height ) : 69
			);
		}

        /*
		private void t進行描画_ギターベースフレーム()
		{
			if( ( ( CDTXMania.ConfigIni.eDark != EDarkMode.HALF ) && ( CDTXMania.ConfigIni.eDark != EDarkMode.FULL ) ) && CDTXMania.ConfigIni.bGuitarEnabled )
			{
				if( CDTXMania.DTX.bチップがある.Guitar )
				{
					for( int i = 0; i < 355; i += 0x80 )
					{
						Rectangle rectangle = new Rectangle( 0, 0, 0x6d, 0x80 );
						if( ( i + 0x80 ) > 355 )
						{
							rectangle.Height -= ( i + 0x80 ) - 355;
						}
						if( this.txレーンフレームGB != null )
						{
							this.txレーンフレームGB.tDraw2D( CDTXMania.app.Device, 0x1fb, 0x39 + i, rectangle );
						}
					}
				}
				if( CDTXMania.DTX.bチップがある.Bass )
				{
					for( int j = 0; j < 355; j += 0x80 )
					{
						Rectangle rectangle2 = new Rectangle( 0, 0, 0x6d, 0x80 );
						if( ( j + 0x80 ) > 355 )
						{
							rectangle2.Height -= ( j + 0x80 ) - 355;
						}
						if( this.txレーンフレームGB != null )
						{
							this.txレーンフレームGB.tDraw2D( CDTXMania.app.Device, 0x18e, 0x39 + j, rectangle2 );
						}
					}
				}
			}
		}
		private void t進行描画_ギターベース判定ライン()		// yyagi: ギタレボモードとは座標が違うだけですが、まとめづらかったのでそのまま放置してます。
		{
			if ( ( CDTXMania.ConfigIni.eDark != EDarkMode.FULL ) && CDTXMania.ConfigIni.bGuitarEnabled )
			{
				if ( CDTXMania.DTX.bチップがある.Guitar )
				{
                    int y = ( CDTXMania.ConfigIni.bReverse.Guitar ? 374 + nJudgeLinePosY_delta.Guitar : 95 - nJudgeLinePosY_delta.Guitar ) - 3;
                    	// #31602 2013.6.23 yyagi 描画遅延対策として、判定ラインの表示位置をオフセット調整できるようにする
                    if ( this.txヒットバーGB != null )
					{
    					for ( int i = 0; i < 3; i++ )						
						{
							this.txヒットバーGB.tDraw2D( CDTXMania.app.Device, 509 + ( 26 * i ), y );
							this.txヒットバーGB.tDraw2D( CDTXMania.app.Device, ( 509 + ( 26 * i ) ) + 16, y, new Rectangle( 0, 0, 10, 16 ) );
						}
					}
				}
				if ( CDTXMania.DTX.bチップがある.Bass )
				{
					int y = ( CDTXMania.ConfigIni.bReverse.Bass ? 374 + nJudgeLinePosY_delta.Bass : 95 - nJudgeLinePosY_delta.Bass ) - 3;
                    // #31602 2013.6.23 yyagi 描画遅延対策として、判定ラインの表示位置をオフセット調整できるようにする
                    if ( this.txヒットバーGB != null )
					{
					    for ( int j = 0; j < 3; j++ )
						{
							this.txヒットバーGB.tDraw2D( CDTXMania.app.Device, 400 + ( 26 * j ), y );
							this.txヒットバーGB.tDraw2D( CDTXMania.app.Device, ( 400 + ( 26 * j ) ) + 16, y, new Rectangle( 0, 0, 10, 16 ) );
						}
					}
				}
			}
		}
         */

        private void t進行描画_グラフ()
        {
            if( CDTXMania.ConfigIni.bGraph有効.Drums )
            {
                this.actGraph.OnUpdateAndDraw();
            }
        }

		private void t進行描画_チップファイアD()
        {
			this.actChipFireD.OnUpdateAndDraw();
        }
        private void t進行描画_ドラムパッド()
        {
            this.actPad.OnUpdateAndDraw();
        }

        /*
        protected override void t進行描画_パネル文字列()
        {
            base.t進行描画_パネル文字列(912, 640);
        }
         */

		protected override void tUpdateAndDraw_PerformanceInformation()
		{
			base.tUpdateAndDraw_PerformanceInformation( 1000, 257 );
		}

		protected override void tHandleInput_Drums()
        {

            for (int nPad = 0; nPad < (int)EPad.MAX; nPad++)
            {
                List<STInputEvent> listInputEvent = CDTXMania.Pad.GetEvents(EInstrumentPart.DRUMS, (EPad)nPad);

                if ((listInputEvent == null) || (listInputEvent.Count == 0))
                    continue;

                this.tSaveInputMethod(EInstrumentPart.DRUMS);

                #region [ 打ち分けグループ調整 ]
                //-----------------------------
                EHHGroup eHHGroup = CDTXMania.ConfigIni.eHHGroup;
                EFTGroup eFTGroup = CDTXMania.ConfigIni.eFTGroup;
                ECYGroup eCYGroup = CDTXMania.ConfigIni.eCYGroup;
                EBDGroup eBDGroup = CDTXMania.ConfigIni.eBDGroup;

                if (!CDTXMania.DTX.bチップがある.Ride && (eCYGroup == ECYGroup.打ち分ける))
                {
                    eCYGroup = ECYGroup.共通;
                }
                if (!CDTXMania.DTX.bチップがある.HHOpen && (eHHGroup == EHHGroup.全部打ち分ける))
                {
                    eHHGroup = EHHGroup.左シンバルのみ打ち分ける;
                }
                if (!CDTXMania.DTX.bチップがある.HHOpen && (eHHGroup == EHHGroup.ハイハットのみ打ち分ける))
                {
                    eHHGroup = EHHGroup.全部共通;
                }
                if (!CDTXMania.DTX.bチップがある.LeftCymbal && (eHHGroup == EHHGroup.全部打ち分ける))
                {
                    eHHGroup = EHHGroup.ハイハットのみ打ち分ける;
                }
                if (!CDTXMania.DTX.bチップがある.LeftCymbal && (eHHGroup == EHHGroup.左シンバルのみ打ち分ける))
                {
                    eHHGroup = EHHGroup.全部共通;
                }
                //-----------------------------
                #endregion

                foreach (STInputEvent inputEvent in listInputEvent)
                {

                    if (!inputEvent.b押された)
                        continue;

                    long nTime = inputEvent.nTimeStamp - CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻;
                    int nInputAdjustTime = this.bIsAutoPlay[base.nチャンネル0Atoレーン07[nPad]] ? 0 : this.nInputAdjustTimeMs.Drums;
                    int nPedalLagTime = CDTXMania.ConfigIni.nPedalLagTime;

                    bool bHitted = false;

                    #region [ (A) ヒットしていればヒット処理して次の inputEvent へ ]
                    //-----------------------------
                    switch (((EPad)nPad))
                    {
                        case EPad.HH:
                            #region [ HHとLC(groupingしている場合) のヒット処理 ]
                            //-----------------------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.HH)
                                    continue;	// 電子ドラムによる意図的なクロストークを無効にする

                                CDTX.CChip chipHC = this.r指定時刻に一番近い未ヒットChip(nTime, 0x11, nInputAdjustTime);	// HiHat Close
                                CDTX.CChip chipHO = this.r指定時刻に一番近い未ヒットChip(nTime, 0x18, nInputAdjustTime);	// HiHat Open
                                CDTX.CChip chipLC = this.r指定時刻に一番近い未ヒットChip(nTime, 0x1a, nInputAdjustTime);	// LC
                                EJudgement e判定HC = (chipHC != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipHC, nInputAdjustTime) : EJudgement.Miss;
                                EJudgement e判定HO = (chipHO != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipHO, nInputAdjustTime) : EJudgement.Miss;
                                EJudgement e判定LC = (chipLC != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLC, nInputAdjustTime) : EJudgement.Miss;
                                switch (eHHGroup)
                                {
                                    case EHHGroup.ハイハットのみ打ち分ける:
                                        #region [ HCとLCのヒット処理 ]
                                        //-----------------------------
                                        if ((e判定HC != EJudgement.Miss) && (e判定LC != EJudgement.Miss))
                                        {
                                            if (chipHC.nPlaybackPosition < chipLC.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HH, chipHC, inputEvent.nVelocity);
                                            }
                                            else if (chipHC.nPlaybackPosition > chipLC.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HH, chipLC, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HH, chipHC, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.HH, chipLC, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定HC != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.HH, chipHC, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定LC != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.HH, chipLC, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;
                                    //-----------------------------
                                        #endregion

                                    case EHHGroup.左シンバルのみ打ち分ける:
                                        #region [ HCとHOのヒット処理 ]
                                        //-----------------------------
                                        if ((e判定HC != EJudgement.Miss) && (e判定HO != EJudgement.Miss))
                                        {
                                            if (chipHC.nPlaybackPosition < chipHO.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HH, chipHC, inputEvent.nVelocity);
                                            }
                                            else if (chipHC.nPlaybackPosition > chipHO.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HH, chipHO, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HH, chipHC, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.HH, chipHO, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定HC != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.HH, chipHC, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定HO != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.HH, chipHO, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;
                                    //-----------------------------
                                        #endregion

                                    case EHHGroup.全部共通:
                                        #region [ HC,HO,LCのヒット処理 ]
                                        //-----------------------------
                                        if (((e判定HC != EJudgement.Miss) && (e判定HO != EJudgement.Miss)) && (e判定LC != EJudgement.Miss))
                                        {
                                            CDTX.CChip chip;
                                            CDTX.CChip[] chipArray = new CDTX.CChip[] { chipHC, chipHO, chipLC };
                                            // ここから、chipArrayをn発生位置の小さい順に並び替える
                                            if (chipArray[1].nPlaybackPosition > chipArray[2].nPlaybackPosition)
                                            {
                                                chip = chipArray[1];
                                                chipArray[1] = chipArray[2];
                                                chipArray[2] = chip;
                                            }
                                            if (chipArray[0].nPlaybackPosition > chipArray[1].nPlaybackPosition)
                                            {
                                                chip = chipArray[0];
                                                chipArray[0] = chipArray[1];
                                                chipArray[1] = chip;
                                            }
                                            if (chipArray[1].nPlaybackPosition > chipArray[2].nPlaybackPosition)
                                            {
                                                chip = chipArray[1];
                                                chipArray[1] = chipArray[2];
                                                chipArray[2] = chip;
                                            }
                                            this.tドラムヒット処理(nTime, EPad.HH, chipArray[0], inputEvent.nVelocity);
                                            if (chipArray[0].nPlaybackPosition == chipArray[1].nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HH, chipArray[1], inputEvent.nVelocity);
                                            }
                                            if (chipArray[0].nPlaybackPosition == chipArray[2].nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HH, chipArray[2], inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定HC != EJudgement.Miss) && (e判定HO != EJudgement.Miss))
                                        {
                                            if (chipHC.nPlaybackPosition < chipHO.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HH, chipHC, inputEvent.nVelocity);
                                            }
                                            else if (chipHC.nPlaybackPosition > chipHO.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HH, chipHO, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HH, chipHC, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.HH, chipHO, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定HC != EJudgement.Miss) && (e判定LC != EJudgement.Miss))
                                        {
                                            if (chipHC.nPlaybackPosition < chipLC.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HH, chipHC, inputEvent.nVelocity);
                                            }
                                            else if (chipHC.nPlaybackPosition > chipLC.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HH, chipLC, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HH, chipHC, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.HH, chipLC, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定HO != EJudgement.Miss) && (e判定LC != EJudgement.Miss))
                                        {
                                            if (chipHO.nPlaybackPosition < chipLC.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HH, chipHO, inputEvent.nVelocity);
                                            }
                                            else if (chipHO.nPlaybackPosition > chipLC.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HH, chipLC, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HH, chipHO, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.HH, chipLC, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定HC != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.HH, chipHC, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定HO != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.HH, chipHO, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定LC != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.HH, chipLC, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;
                                    //-----------------------------
                                        #endregion

                                    default:
                                        #region [ 全部打ち分け時のヒット処理 ]
                                        //-----------------------------
                                        if (e判定HC != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.HH, chipHC, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;
                                    //-----------------------------
                                        #endregion
                                }
                                if (!bHitted)
                                    break;
                                continue;
                            }
                        //-----------------------------
                            #endregion

                        case EPad.SD:
                            #region [ SDのヒット処理 ]
                            //-----------------------------
                            if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.SD)	// #23857 2010.12.12 yyagi: to support VelocityMin
                                continue;	// 電子ドラムによる意図的なクロストークを無効にする
                            if (!this.tドラムヒット処理(nTime, EPad.SD, this.r指定時刻に一番近い未ヒットChip(nTime, 0x12, nInputAdjustTime), inputEvent.nVelocity))
                                break;
                            continue;
                        //-----------------------------
                            #endregion

                        case EPad.BD:
                            #region [ BDとLPとLBD(ペアリングしている場合)のヒット処理 ]
                            //-----------------------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.BD)	// #23857 2010.12.12 yyagi: to support VelocityMin
                                    continue;	// 電子ドラムによる意図的なクロストークを無効にする

                                CDTX.CChip chipBD  = this.r指定時刻に一番近い未ヒットChip(nTime, 0x13, nInputAdjustTime + nPedalLagTime);	// BD
                                CDTX.CChip chipLP  = this.r指定時刻に一番近い未ヒットChip(nTime, 0x1b, nInputAdjustTime + nPedalLagTime);	// LP
                                CDTX.CChip chipLBD = this.r指定時刻に一番近い未ヒットChip(nTime, 0x1c, nInputAdjustTime + nPedalLagTime);	// LBD
                                EJudgement e判定BD  = (chipBD  != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipBD, nInputAdjustTime + nPedalLagTime) : EJudgement.Miss;
                                EJudgement e判定LP  = (chipLP  != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLP, nInputAdjustTime + nPedalLagTime) : EJudgement.Miss;
                                EJudgement e判定LBD = (chipLBD != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLBD, nInputAdjustTime + nPedalLagTime) : EJudgement.Miss;
                                switch (eBDGroup)
                                {
                                    case EBDGroup.BDとLPで打ち分ける:
                                        #region[ BD & LBD | LP ]
                                        if( e判定BD != EJudgement.Miss && e判定LBD != EJudgement.Miss )
                                        {
                                            if( chipBD.nPlaybackPosition < chipLBD.nPlaybackPosition )
                                            {
                                                this.tドラムヒット処理( nTime, EPad.BD, chipBD, inputEvent.nVelocity );
                                            }
                                            else if( chipBD.nPlaybackPosition > chipLBD.nPlaybackPosition )
                                            {
                                                this.tドラムヒット処理( nTime, EPad.BD, chipLBD, inputEvent.nVelocity );
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理( nTime, EPad.BD, chipBD, inputEvent.nVelocity);
                                                this.tドラムヒット処理( nTime, EPad.BD, chipLBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if( e判定BD != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理( nTime, EPad.BD, chipBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if( e判定LBD != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理( nTime, EPad.BD, chipLBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if( bHitted )
                                            continue;
                                        else
                                            break;
                                        #endregion

                                    case EBDGroup.左右ペダルのみ打ち分ける:
                                        #region[ BDのヒット処理]
                                        if (e判定BD != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.BD, chipBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (bHitted)
                                        {
                                            continue;
                                        }
                                        break;
                                        #endregion

                                    case EBDGroup.どっちもBD:
                                        #region[ LP&LBD&BD ]
                                        if (((e判定LP != EJudgement.Miss) && (e判定LBD != EJudgement.Miss)) && (e判定BD != EJudgement.Miss))
                                        {
                                            CDTX.CChip chip8;
                                            CDTX.CChip[] chipArray2 = new CDTX.CChip[] { chipLP, chipLBD, chipBD };
                                            if (chipArray2[1].nPlaybackPosition > chipArray2[2].nPlaybackPosition)
                                            {
                                                chip8 = chipArray2[1];
                                                chipArray2[1] = chipArray2[2];
                                                chipArray2[2] = chip8;
                                            }
                                            if (chipArray2[0].nPlaybackPosition > chipArray2[1].nPlaybackPosition)
                                            {
                                                chip8 = chipArray2[0];
                                                chipArray2[0] = chipArray2[1];
                                                chipArray2[1] = chip8;
                                            }
                                            if (chipArray2[1].nPlaybackPosition > chipArray2[2].nPlaybackPosition)
                                            {
                                                chip8 = chipArray2[1];
                                                chipArray2[1] = chipArray2[2];
                                                chipArray2[2] = chip8;
                                            }
                                            this.tドラムヒット処理(nTime, EPad.BD, chipArray2[0], inputEvent.nVelocity);
                                            if (chipArray2[0].nPlaybackPosition == chipArray2[1].nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.BD, chipArray2[1], inputEvent.nVelocity);
                                            }
                                            if (chipArray2[0].nPlaybackPosition == chipArray2[2].nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.BD, chipArray2[2], inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        //chip7 BD  chip6LBD  chip5LP
                                        //判定6 BD  判定5　　 判定4
                                        else if ((e判定LP != EJudgement.Miss) && (e判定LBD != EJudgement.Miss))
                                        {
                                            if (chipLP.nPlaybackPosition < chipLBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.BD, chipLP, inputEvent.nVelocity);
                                            }
                                            else if (chipLP.nPlaybackPosition > chipLBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.BD, chipLBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.BD, chipLP, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.BD, chipLBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定LP != EJudgement.Miss) && (e判定BD != EJudgement.Miss))
                                        {
                                            if (chipLP.nPlaybackPosition < chipBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.BD, chipLP, inputEvent.nVelocity);
                                            }
                                            else if (chipLP.nPlaybackPosition > chipBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.BD, chipBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.BD, chipLP, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.BD, chipBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        //chip7 BD  chip6LBD  chip5LP
                                        //判定6 BD  判定5　　 判定4
                                        else if ((e判定LBD != EJudgement.Miss) && (e判定BD != EJudgement.Miss))
                                        {
                                            if (chipLBD.nPlaybackPosition < chipBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.BD, chipLBD, inputEvent.nVelocity);
                                            }
                                            else if (chipLBD.nPlaybackPosition > chipBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.BD, chipBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.BD, chipLBD, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.BD, chipBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定LP != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.BD, chipLP, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定LBD != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.BD, chipLBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定BD != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.BD, chipBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (bHitted)
                                        {
                                            continue;
                                        }
                                        break;
                                        #endregion

                                    default:
                                        #region [ 全部打ち分け時のヒット処理 ]
                                        //-----------------------------
                                        if (e判定BD != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.BD, chipBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;
                                    //-----------------------------
                                        #endregion
                                }
                                if (!bHitted)
                                    break;
                                continue;
                            }
                        //-----------------------------
                            #endregion

                        case EPad.HT:
                            #region [ HTのヒット処理 ]
                            //-----------------------------
                            if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.HT)	// #23857 2010.12.12 yyagi: to support VelocityMin
                                continue;	// 電子ドラムによる意図的なクロストークを無効にする
                            if (this.tドラムヒット処理(nTime, EPad.HT, this.r指定時刻に一番近い未ヒットChip(nTime, 20, nInputAdjustTime), inputEvent.nVelocity))
                                continue;
                            break;
                        //-----------------------------
                            #endregion

                        case EPad.LT:
                            #region [ LTとFT(groupingしている場合)のヒット処理 ]
                            //-----------------------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.LT)	// #23857 2010.12.12 yyagi: to support VelocityMin
                                    continue;	// 電子ドラムによる意図的なクロストークを無効にする
                                CDTX.CChip chipLT = this.r指定時刻に一番近い未ヒットChip(nTime, 0x15, nInputAdjustTime);	// LT
                                CDTX.CChip chipFT = this.r指定時刻に一番近い未ヒットChip(nTime, 0x17, nInputAdjustTime);	// FT
                                EJudgement e判定LT = (chipLT != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLT, nInputAdjustTime) : EJudgement.Miss;
                                EJudgement e判定FT = (chipFT != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipFT, nInputAdjustTime) : EJudgement.Miss;
                                switch (eFTGroup)
                                {
                                    case EFTGroup.打ち分ける:
                                        #region [ LTのヒット処理 ]
                                        //-----------------------------
                                        if (e判定LT != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.LT, chipLT, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        break;
                                    //-----------------------------
                                        #endregion

                                    case EFTGroup.共通:
                                        #region [ LTとFTのヒット処理 ]
                                        //-----------------------------
                                        if ((e判定LT != EJudgement.Miss) && (e判定FT != EJudgement.Miss))
                                        {
                                            if (chipLT.nPlaybackPosition < chipFT.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LT, chipLT, inputEvent.nVelocity);
                                            }
                                            else if (chipLT.nPlaybackPosition > chipFT.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LT, chipFT, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LT, chipLT, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.LT, chipFT, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定LT != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.LT, chipLT, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定FT != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.LT, chipFT, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        break;
                                    //-----------------------------
                                        #endregion
                                }
                                if (!bHitted)
                                    break;
                                continue;
                            }
                        //-----------------------------
                            #endregion

                        case EPad.FT:
                            #region [ FTとLT(groupingしている場合)のヒット処理 ]
                            //-----------------------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.FT)	// #23857 2010.12.12 yyagi: to support VelocityMin
                                    continue;	// 電子ドラムによる意図的なクロストークを無効にする
                                CDTX.CChip chipLT = this.r指定時刻に一番近い未ヒットChip(nTime, 0x15, nInputAdjustTime);	// LT
                                CDTX.CChip chipFT = this.r指定時刻に一番近い未ヒットChip(nTime, 0x17, nInputAdjustTime);	// FT
                                EJudgement e判定LT = (chipLT != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLT, nInputAdjustTime) : EJudgement.Miss;
                                EJudgement e判定FT = (chipFT != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipFT, nInputAdjustTime) : EJudgement.Miss;
                                switch (eFTGroup)
                                {
                                    case EFTGroup.打ち分ける:
                                        #region [ FTのヒット処理 ]
                                        //-----------------------------
                                        if (e判定FT != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.FT, chipFT, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        //-----------------------------
                                        #endregion
                                        break;

                                    case EFTGroup.共通:
                                        #region [ FTとLTのヒット処理 ]
                                        //-----------------------------
                                        if ((e判定LT != EJudgement.Miss) && (e判定FT != EJudgement.Miss))
                                        {
                                            if (chipLT.nPlaybackPosition < chipFT.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.FT, chipLT, inputEvent.nVelocity);
                                            }
                                            else if (chipLT.nPlaybackPosition > chipFT.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.FT, chipFT, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.FT, chipLT, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.FT, chipFT, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定LT != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.FT, chipLT, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定FT != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.FT, chipFT, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        //-----------------------------
                                        #endregion
                                        break;
                                }
                                if (!bHitted)
                                    break;
                                continue;
                            }
                        //-----------------------------
                            #endregion

                        case EPad.CY:
                            #region [ CY(とLCとRD:groupingしている場合)のヒット処理 ]
                            //-----------------------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.CY)	// #23857 2010.12.12 yyagi: to support VelocityMin
                                    continue;	// 電子ドラムによる意図的なクロストークを無効にする
                                CDTX.CChip chipCY = this.r指定時刻に一番近い未ヒットChip(nTime, 0x16, nInputAdjustTime);	// CY
                                CDTX.CChip chipRD = this.r指定時刻に一番近い未ヒットChip(nTime, 0x19, nInputAdjustTime);	// RD
                                CDTX.CChip chipLC = CDTXMania.ConfigIni.bシンバルフリー ? this.r指定時刻に一番近い未ヒットChip(nTime, 0x1a, nInputAdjustTime) : null;
                                EJudgement e判定CY = (chipCY != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipCY, nInputAdjustTime) : EJudgement.Miss;
                                EJudgement e判定RD = (chipRD != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipRD, nInputAdjustTime) : EJudgement.Miss;
                                EJudgement e判定LC = (chipLC != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLC, nInputAdjustTime) : EJudgement.Miss;
                                CDTX.CChip[] chipArray = new CDTX.CChip[] { chipCY, chipRD, chipLC };
                                EJudgement[] e判定Array = new EJudgement[] { e判定CY, e判定RD, e判定LC };
                                const int NumOfChips = 3;	// chipArray.GetLength(0)

                                //num8 = 0;
                                //while( num8 < 2 )

                                // CY/RD/LC群を, n発生位置の小さい順に並べる + nullを大きい方に退かす
                                SortChipsByNTime(chipArray, e判定Array, NumOfChips);
                                //for ( int i = 0; i < NumOfChips - 1; i++ )
                                //{
                                //    //num9 = 2;
                                //    //while( num9 > num8 )
                                //    for ( int j = NumOfChips - 1; j > i; j-- )
                                //    {
                                //        if ( ( chipArray[ j - 1 ] == null ) || ( ( chipArray[ j ] != null ) && ( chipArray[ j - 1 ].nPlaybackPosition > chipArray[ j ].nPlaybackPosition ) ) )
                                //        {
                                //            // swap
                                //            CDTX.CChip chipTemp = chipArray[ j - 1 ];
                                //            chipArray[ j - 1 ] = chipArray[ j ];
                                //            chipArray[ j ] = chipTemp;
                                //            EJudgement e判定Temp = e判定Array[ j - 1 ];
                                //            e判定Array[ j - 1 ] = e判定Array[ j ];
                                //            e判定Array[ j ] = e判定Temp;
                                //        }
                                //        //num9--;
                                //    }
                                //    //num8++;
                                //}
                                switch (eCYGroup)
                                {
                                    case ECYGroup.打ち分ける:
                                        #region [打ち分ける]
                                        if (!CDTXMania.ConfigIni.bシンバルフリー)
                                        {
                                            
                                            if (e判定CY != EJudgement.Miss)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.CY, chipCY, inputEvent.nVelocity);
                                                bHitted = true;
                                            }
                                            if (!bHitted)
                                                break;
                                            continue;
                                        }
                                        //num10 = 0;
                                        //while ( num10 < NumOfChips )
                                        for (int i = 0; i < NumOfChips; i++)
                                        {
                                            if ((e判定Array[i] != EJudgement.Miss) && ((chipArray[i] == chipCY) || (chipArray[i] == chipLC)))
                                            {
                                                this.tドラムヒット処理(nTime, EPad.CY, chipArray[i], inputEvent.nVelocity);
                                                bHitted = true;
                                                break;
                                            }
                                            //num10++;
                                        }
                                        if (e判定CY != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.CY, chipCY, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;
                                        #endregion

                                    case ECYGroup.共通:
                                        
                                        if (!CDTXMania.ConfigIni.bシンバルフリー)
                                        {
                                            //num12 = 0;
                                            //while ( num12 < NumOfChips )
                                            for (int i = 0; i < NumOfChips; i++)
                                            {
                                                if ((e判定Array[i] != EJudgement.Miss) && ((chipArray[i] == chipCY) || (chipArray[i] == chipRD)))
                                                {
                                                    this.tドラムヒット処理(nTime, EPad.CY, chipArray[i], inputEvent.nVelocity);
                                                    bHitted = true;
                                                    break;
                                                }
                                                //num12++;
                                            }
                                            if (!bHitted)
                                                break;
                                            continue;
                                        }
                                        //num11 = 0;
                                        //while ( num11 < NumOfChips )
                                        for (int i = 0; i < NumOfChips; i++)
                                        {
                                            if (e判定Array[i] != EJudgement.Miss)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.CY, chipArray[i], inputEvent.nVelocity);
                                                bHitted = true;
                                                break;
                                            }
                                            //num11++;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;
                                }
                                if (!bHitted)
                                    break;
                                continue;
                            }
                        //-----------------------------
                            #endregion

                        case EPad.HHO:
                            #region [ HO(とHCとLC:groupingしている場合)のヒット処理 ]
                            //-----------------------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.HH)
                                    continue;	// 電子ドラムによる意図的なクロストークを無効にする

                                CDTX.CChip chipHC = this.r指定時刻に一番近い未ヒットChip(nTime, 0x11, nInputAdjustTime);	// HC
                                CDTX.CChip chipHO = this.r指定時刻に一番近い未ヒットChip(nTime, 0x18, nInputAdjustTime);	// HO
                                CDTX.CChip chipLC = this.r指定時刻に一番近い未ヒットChip(nTime, 0x1a, nInputAdjustTime);	// LC
                                EJudgement e判定HC = (chipHC != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipHC, nInputAdjustTime) : EJudgement.Miss;
                                EJudgement e判定HO = (chipHO != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipHO, nInputAdjustTime) : EJudgement.Miss;
                                EJudgement e判定LC = (chipLC != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLC, nInputAdjustTime) : EJudgement.Miss;
                                switch (eHHGroup)
                                {
                                    case EHHGroup.全部打ち分ける:
                                        if (e判定HO != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.HHO, chipHO, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;

                                    case EHHGroup.ハイハットのみ打ち分ける:
                                        if ((e判定HO != EJudgement.Miss) && (e判定LC != EJudgement.Miss))
                                        {
                                            if (chipHO.nPlaybackPosition < chipLC.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipHO, inputEvent.nVelocity);
                                            }
                                            else if (chipHO.nPlaybackPosition > chipLC.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipLC, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipHO, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipLC, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定HO != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.HHO, chipHO, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定LC != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.HHO, chipLC, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;

                                    case EHHGroup.左シンバルのみ打ち分ける:
                                        if ((e判定HC != EJudgement.Miss) && (e判定HO != EJudgement.Miss))
                                        {
                                            if (chipHC.nPlaybackPosition < chipHO.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipHC, inputEvent.nVelocity);
                                            }
                                            else if (chipHC.nPlaybackPosition > chipHO.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipHO, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipHC, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipHO, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定HC != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.HHO, chipHC, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定HO != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.HHO, chipHO, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;

                                    case EHHGroup.全部共通:
                                        if (((e判定HC != EJudgement.Miss) && (e判定HO != EJudgement.Miss)) && (e判定LC != EJudgement.Miss))
                                        {
                                            CDTX.CChip chip;
                                            CDTX.CChip[] chipArray = new CDTX.CChip[] { chipHC, chipHO, chipLC };
                                            // ここから、chipArrayをn発生位置の小さい順に並び替える
                                            if (chipArray[1].nPlaybackPosition > chipArray[2].nPlaybackPosition)
                                            {
                                                chip = chipArray[1];
                                                chipArray[1] = chipArray[2];
                                                chipArray[2] = chip;
                                            }
                                            if (chipArray[0].nPlaybackPosition > chipArray[1].nPlaybackPosition)
                                            {
                                                chip = chipArray[0];
                                                chipArray[0] = chipArray[1];
                                                chipArray[1] = chip;
                                            }
                                            if (chipArray[1].nPlaybackPosition > chipArray[2].nPlaybackPosition)
                                            {
                                                chip = chipArray[1];
                                                chipArray[1] = chipArray[2];
                                                chipArray[2] = chip;
                                            }
                                            this.tドラムヒット処理(nTime, EPad.HHO, chipArray[0], inputEvent.nVelocity);
                                            if (chipArray[0].nPlaybackPosition == chipArray[1].nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipArray[1], inputEvent.nVelocity);
                                            }
                                            if (chipArray[0].nPlaybackPosition == chipArray[2].nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipArray[2], inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定HC != EJudgement.Miss) && (e判定HO != EJudgement.Miss))
                                        {
                                            if (chipHC.nPlaybackPosition < chipHO.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipHC, inputEvent.nVelocity);
                                            }
                                            else if (chipHC.nPlaybackPosition > chipHO.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipHO, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipHC, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipHO, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定HC != EJudgement.Miss) && (e判定LC != EJudgement.Miss))
                                        {
                                            if (chipHC.nPlaybackPosition < chipLC.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipHC, inputEvent.nVelocity);
                                            }
                                            else if (chipHC.nPlaybackPosition > chipLC.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipLC, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipHC, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipLC, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定HO != EJudgement.Miss) && (e判定LC != EJudgement.Miss))
                                        {
                                            if (chipHO.nPlaybackPosition < chipLC.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipHO, inputEvent.nVelocity);
                                            }
                                            else if (chipHO.nPlaybackPosition > chipLC.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipLC, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipHO, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.HHO, chipLC, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定HC != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.HHO, chipHC, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定HO != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.HHO, chipHO, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定LC != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.HHO, chipLC, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;
                                }
                                if (!bHitted)
                                    break;
                                continue;
                            }
                        //-----------------------------
                            #endregion

                        case EPad.RD:
                            #region [ RD(とCYとLC:groupingしている場合)のヒット処理 ]
                            //-----------------------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.RD)	// #23857 2010.12.12 yyagi: to support VelocityMin
                                    continue;	// 電子ドラムによる意図的なクロストークを無効にする
                                CDTX.CChip chipCY = this.r指定時刻に一番近い未ヒットChip(nTime, 0x16, nInputAdjustTime);	// CY
                                CDTX.CChip chipRD = this.r指定時刻に一番近い未ヒットChip(nTime, 0x19, nInputAdjustTime);	// RD
                                CDTX.CChip chipLC = CDTXMania.ConfigIni.bシンバルフリー ? this.r指定時刻に一番近い未ヒットChip(nTime, 0x1a, nInputAdjustTime) : null;
                                EJudgement e判定CY = (chipCY != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipCY, nInputAdjustTime) : EJudgement.Miss;
                                EJudgement e判定RD = (chipRD != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipRD, nInputAdjustTime) : EJudgement.Miss;
                                EJudgement e判定LC = (chipLC != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLC, nInputAdjustTime) : EJudgement.Miss;
                                CDTX.CChip[] chipArray = new CDTX.CChip[] { chipCY, chipRD, chipLC };
                                EJudgement[] e判定Array = new EJudgement[] { e判定CY, e判定RD, e判定LC };
                                const int NumOfChips = 3;	// chipArray.GetLength(0)
                                SortChipsByNTime(chipArray, e判定Array, NumOfChips);
                                switch (eCYGroup)
                                {
                                    case ECYGroup.打ち分ける:
                                        if (e判定RD != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.RD, chipRD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        break;

                                    case ECYGroup.共通:
                                        if (!CDTXMania.ConfigIni.bシンバルフリー)
                                        {
                                            //num16 = 0;
                                            //while( num16 < 3 )
                                            for (int i = 0; i < NumOfChips; i++)
                                            {
                                                if ((e判定Array[i] != EJudgement.Miss) && ((chipArray[i] == chipCY) || (chipArray[i] == chipRD)))
                                                {
                                                    this.tドラムヒット処理(nTime, EPad.CY, chipArray[i], inputEvent.nVelocity);
                                                    bHitted = true;
                                                    break;
                                                }
                                                //num16++;
                                            }
                                            break;
                                        }
                                        //num15 = 0;
                                        //while( num15 < 3 )
                                        for (int i = 0; i < NumOfChips; i++)
                                        {
                                            if (e判定Array[i] != EJudgement.Miss)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.CY, chipArray[i], inputEvent.nVelocity);
                                                bHitted = true;
                                                break;
                                            }
                                            //num15++;
                                        }
                                        break;
                                }
                                if (bHitted)
                                {
                                    continue;
                                }
                                break;
                            }
                        //-----------------------------
                            #endregion

                        case EPad.LC:
                            #region [ LC(とHC/HOとCYと:groupingしている場合)のヒット処理 ]
                            //-----------------------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.LC)	// #23857 2010.12.12 yyagi: to support VelocityMin
                                    continue;	// 電子ドラムによる意図的なクロストークを無効にする
                                CDTX.CChip chipHC = this.r指定時刻に一番近い未ヒットChip(nTime, 0x11, nInputAdjustTime);	// HC
                                CDTX.CChip chipHO = this.r指定時刻に一番近い未ヒットChip(nTime, 0x18, nInputAdjustTime);	// HO
                                CDTX.CChip chipLC = this.r指定時刻に一番近い未ヒットChip(nTime, 0x1a, nInputAdjustTime);	// LC
                                CDTX.CChip chipCY = CDTXMania.ConfigIni.bシンバルフリー ? this.r指定時刻に一番近い未ヒットChip(nTime, 0x16, nInputAdjustTime) : null;
                                CDTX.CChip chipRD = CDTXMania.ConfigIni.bシンバルフリー ? this.r指定時刻に一番近い未ヒットChip(nTime, 0x19, nInputAdjustTime) : null;
                                EJudgement e判定HC = (chipHC != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipHC, nInputAdjustTime) : EJudgement.Miss;
                                EJudgement e判定HO = (chipHO != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipHO, nInputAdjustTime) : EJudgement.Miss;
                                EJudgement e判定LC = (chipLC != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLC, nInputAdjustTime) : EJudgement.Miss;
                                EJudgement e判定CY = (chipCY != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipCY, nInputAdjustTime) : EJudgement.Miss;
                                EJudgement e判定RD = (chipRD != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipRD, nInputAdjustTime) : EJudgement.Miss;
                                CDTX.CChip[] chipArray = new CDTX.CChip[] { chipHC, chipHO, chipLC, chipCY, chipRD };
                                EJudgement[] e判定Array = new EJudgement[] { e判定HC, e判定HO, e判定LC, e判定CY, e判定RD };
                                const int NumOfChips = 5;	// chipArray.GetLength(0)
                                SortChipsByNTime(chipArray, e判定Array, NumOfChips);

                                switch (eHHGroup)
                                {
                                    case EHHGroup.全部打ち分ける:
                                    case EHHGroup.左シンバルのみ打ち分ける:
                                        #region[左シンバルのみ打ち分ける]
                                        if (!CDTXMania.ConfigIni.bシンバルフリー)
                                        {
                                            if (e判定LC != EJudgement.Miss)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LC, chipLC, inputEvent.nVelocity);
                                                bHitted = true;
                                            }
                                            if (!bHitted)
                                                break;
                                            continue;
                                        }
                                        //num5 = 0;
                                        //while( num5 < 5 )
                                        for (int i = 0; i < NumOfChips; i++)
                                        {
                                            if ((e判定Array[i] != EJudgement.Miss) && (((chipArray[i] == chipLC) || (chipArray[i] == chipCY)) || ((chipArray[i] == chipRD) && (CDTXMania.ConfigIni.eCYGroup == ECYGroup.共通))))
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LC, chipArray[i], inputEvent.nVelocity);
                                                bHitted = true;
                                                break;
                                            }
                                            //num5++;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;
                                        #endregion
                                    case EHHGroup.ハイハットのみ打ち分ける:
                                    case EHHGroup.全部共通:
                                        if (!CDTXMania.ConfigIni.bシンバルフリー)
                                        #region[全部共通]
                                        {
                                            //num7 = 0;
                                            //while( num7 < 5 )
                                            for (int i = 0; i < NumOfChips; i++)
                                            {
                                                if ((e判定Array[i] != EJudgement.Miss) && (((chipArray[i] == chipLC) || (chipArray[i] == chipHC)) || (chipArray[i] == chipHO)))
                                                {
                                                    this.tドラムヒット処理(nTime, EPad.LC, chipArray[i], inputEvent.nVelocity);
                                                    bHitted = true;
                                                    break;
                                                }
                                                //num7++;
                                            }
                                            if (!bHitted)
                                                break;
                                            continue;
                                        }
                                        //num6 = 0;
                                        //while( num6 < 5 )
                                        for (int i = 0; i < NumOfChips; i++)
                                        {
                                            if ((e判定Array[i] != EJudgement.Miss) && ((chipArray[i] != chipRD) || (CDTXMania.ConfigIni.eCYGroup == ECYGroup.共通)))
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LC, chipArray[i], inputEvent.nVelocity);
                                                bHitted = true;
                                                break;
                                            }
                                            //num6++;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;
                                        #endregion
                                }
                                if (!bHitted)
                                    break;

                                break;
                            }
                        //-----------------------------
                            #endregion

                        #region [rev030追加処理]
                        case EPad.LP:
                            #region [ LPのヒット処理 ]
                            //-----------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.LP)
                                    continue;
                                CDTX.CChip chipBD  = this.r指定時刻に一番近い未ヒットChip(nTime, 0x13, nInputAdjustTime + nPedalLagTime);	// BD
                                CDTX.CChip chipLP  = this.r指定時刻に一番近い未ヒットChip(nTime, 0x1b, nInputAdjustTime + nPedalLagTime);	// LP
                                CDTX.CChip chipLBD = this.r指定時刻に一番近い未ヒットChip(nTime, 0x1c, nInputAdjustTime + nPedalLagTime);	// LBD
                                EJudgement e判定BD  = (chipBD  != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipBD,  nInputAdjustTime + nPedalLagTime) : EJudgement.Miss;
                                EJudgement e判定LP  = (chipLP  != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLP,  nInputAdjustTime + nPedalLagTime) : EJudgement.Miss;
                                EJudgement e判定LBD = (chipLBD != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLBD, nInputAdjustTime + nPedalLagTime) : EJudgement.Miss;
                                switch (eBDGroup)
                                {
                                    case EBDGroup.左右ペダルのみ打ち分ける:
                                        #region[ LPのヒット処理]
                                        if (e判定LP != EJudgement.Miss && e判定LBD != EJudgement.Miss)
                                        {
                                            if (chipLP.nPlaybackPosition < chipLBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LP, chipLP, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                if (chipLP.nPlaybackPosition > chipLBD.nPlaybackPosition)
                                                {
                                                    this.tドラムヒット処理(nTime, EPad.LP, chipLBD, inputEvent.nVelocity);
                                                }
                                                else
                                                {
                                                    this.tドラムヒット処理(nTime, EPad.LP, chipLP, inputEvent.nVelocity);
                                                    this.tドラムヒット処理(nTime, EPad.LP, chipLBD, inputEvent.nVelocity);
                                                }
                                            }
                                            bHitted = true;
                                        }
                                        else
                                        {
                                            if (e判定LP != EJudgement.Miss)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LP, chipLP, inputEvent.nVelocity);
                                                bHitted = true;
                                            }
                                            else
                                            {
                                                if (e判定LBD != EJudgement.Miss)
                                                {
                                                    this.tドラムヒット処理(nTime, EPad.LP, chipLBD, inputEvent.nVelocity);
                                                    bHitted = true;
                                                }
                                            }
                                        }
                                        if (bHitted)
                                        {
                                            continue;
                                        }
                                        break;
                                        #endregion

                                    case EBDGroup.どっちもBD:
                                        #region[ LP&LBD&BD ]
                                        if (((e判定LP != EJudgement.Miss) && (e判定LBD != EJudgement.Miss)) && (e判定BD != EJudgement.Miss))
                                        {
                                            CDTX.CChip chip;
                                            CDTX.CChip[] chipArray = new CDTX.CChip[] { chipLP, chipLBD, chipBD };
                                            if (chipArray[1].nPlaybackPosition > chipArray[2].nPlaybackPosition)
                                            {
                                                chip = chipArray[1];
                                                chipArray[1] = chipArray[2];
                                                chipArray[2] = chip;
                                            }
                                            if (chipArray[0].nPlaybackPosition > chipArray[1].nPlaybackPosition)
                                            {
                                                chip = chipArray[0];
                                                chipArray[0] = chipArray[1];
                                                chipArray[1] = chip;
                                            }
                                            if (chipArray[1].nPlaybackPosition > chipArray[2].nPlaybackPosition)
                                            {
                                                chip = chipArray[1];
                                                chipArray[1] = chipArray[2];
                                                chipArray[2] = chip;
                                            }
                                            this.tドラムヒット処理(nTime, EPad.LP, chipArray[0], inputEvent.nVelocity);
                                            if (chipArray[0].nPlaybackPosition == chipArray[1].nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LP, chipArray[1], inputEvent.nVelocity);
                                            }
                                            if (chipArray[0].nPlaybackPosition == chipArray[2].nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LP, chipArray[2], inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定LP != EJudgement.Miss) && (e判定LBD != EJudgement.Miss))
                                        {
                                            if (chipLP.nPlaybackPosition < chipLBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LP, chipLP, inputEvent.nVelocity);
                                            }
                                            else if (chipLP.nPlaybackPosition > chipLBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LP, chipLBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LP, chipLP, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.LP, chipLBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定LP != EJudgement.Miss) && (e判定BD != EJudgement.Miss))
                                        {
                                            if (chipLP.nPlaybackPosition < chipBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LP, chipLP, inputEvent.nVelocity);
                                            }
                                            else if (chipLP.nPlaybackPosition > chipBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LP, chipBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LP, chipLP, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.LP, chipBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定LBD != EJudgement.Miss) && (e判定BD != EJudgement.Miss))
                                        {
                                            if (chipLBD.nPlaybackPosition < chipBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LP, chipLBD, inputEvent.nVelocity);
                                            }
                                            else if (chipLBD.nPlaybackPosition > chipBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LP, chipBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LP, chipLBD, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.LP, chipBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定LP != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.LP, chipLP, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定LBD != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.LP, chipLBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定BD != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.LP, chipBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (bHitted)
                                        {
                                            continue;
                                        }
                                        #endregion
                                        break;

                                    case EBDGroup.BDとLPで打ち分ける:
                                    default:
                                        #region [ 全部打ち分け時のヒット処理 ]
                                        //-----------------------------
                                        if (e判定LP != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.LP, chipLP, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;
                                    //-----------------------------
                                        #endregion
                                }
                                if (!bHitted)
                                    break;
                                continue;
                            }
                        //-----------------
                            #endregion

                        case EPad.LBD:
                            #region [ LBDのヒット処理 ]
                            //-----------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.LBD)
                                    continue;
                                CDTX.CChip chipBD  = this.r指定時刻に一番近い未ヒットChip(nTime, 0x13, nInputAdjustTime + nPedalLagTime);	// BD
                                CDTX.CChip chipLP  = this.r指定時刻に一番近い未ヒットChip(nTime, 0x1b, nInputAdjustTime + nPedalLagTime);	// LP
                                CDTX.CChip chipLBD = this.r指定時刻に一番近い未ヒットChip(nTime, 0x1c, nInputAdjustTime + nPedalLagTime);	// LBD
                                EJudgement e判定BD  = (chipBD  != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipBD,  nInputAdjustTime + nPedalLagTime) : EJudgement.Miss;
                                EJudgement e判定LP  = (chipLP  != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLP,  nInputAdjustTime + nPedalLagTime) : EJudgement.Miss;
                                EJudgement e判定LBD = (chipLBD != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLBD, nInputAdjustTime + nPedalLagTime) : EJudgement.Miss;
                                switch (eBDGroup)
                                {
                                    case EBDGroup.BDとLPで打ち分ける:
                                        #region[ BD & LBD | LP ]
                                        if( e判定BD != EJudgement.Miss && e判定LBD != EJudgement.Miss )
                                        {
                                            if( chipBD.nPlaybackPosition < chipLBD.nPlaybackPosition )
                                            {
                                                this.tドラムヒット処理( nTime, EPad.LBD, chipBD, inputEvent.nVelocity );
                                            }
                                            else if( chipBD.nPlaybackPosition > chipLBD.nPlaybackPosition )
                                            {
                                                this.tドラムヒット処理( nTime, EPad.LBD, chipLBD, inputEvent.nVelocity );
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理( nTime, EPad.LBD, chipBD, inputEvent.nVelocity );
                                                this.tドラムヒット処理( nTime, EPad.LBD, chipLBD, inputEvent.nVelocity );
                                            }
                                            bHitted = true;
                                        }
                                        else if( e判定BD != EJudgement.Miss )
                                        {
                                            this.tドラムヒット処理( nTime, EPad.LBD, chipBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if( e判定LBD != EJudgement.Miss )
                                        {
                                            this.tドラムヒット処理( nTime, EPad.LBD, chipLBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if( bHitted )
                                            continue;
                                        else
                                            break;
                                        #endregion

                                    case EBDGroup.左右ペダルのみ打ち分ける:
                                        #region[ LPのヒット処理]
                                        if (e判定LP != EJudgement.Miss && e判定LBD != EJudgement.Miss)
                                        {
                                            if (chipLP.nPlaybackPosition < chipLBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LBD, chipLP, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                if (chipLP.nPlaybackPosition > chipLBD.nPlaybackPosition)
                                                {
                                                    this.tドラムヒット処理(nTime, EPad.LBD, chipLBD, inputEvent.nVelocity);
                                                }
                                                else
                                                {
                                                    this.tドラムヒット処理(nTime, EPad.LBD, chipLP, inputEvent.nVelocity);
                                                    this.tドラムヒット処理(nTime, EPad.LBD, chipLBD, inputEvent.nVelocity);
                                                }
                                            }
                                            bHitted = true;
                                        }
                                        else
                                        {
                                            if (e判定LP != EJudgement.Miss)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LBD, chipLP, inputEvent.nVelocity);
                                                bHitted = true;
                                            }
                                            else
                                            {
                                                if (e判定LBD != EJudgement.Miss)
                                                {
                                                    this.tドラムヒット処理(nTime, EPad.LBD, chipLBD, inputEvent.nVelocity);
                                                    bHitted = true;
                                                }
                                            }
                                        }
                                        if (bHitted)
                                        {
                                            continue;
                                        }
                                        break;
                                        #endregion

                                    case EBDGroup.どっちもBD:
                                        #region[ LP&LBD&BD ]
                                        if (((e判定LP != EJudgement.Miss) && (e判定LBD != EJudgement.Miss)) && (e判定BD != EJudgement.Miss))
                                        {
                                            CDTX.CChip chip;
                                            CDTX.CChip[] chipArray = new CDTX.CChip[] { chipLP, chipLBD, chipBD };
                                            if (chipArray[1].nPlaybackPosition > chipArray[2].nPlaybackPosition)
                                            {
                                                chip = chipArray[1];
                                                chipArray[1] = chipArray[2];
                                                chipArray[2] = chip;
                                            }
                                            if (chipArray[0].nPlaybackPosition > chipArray[1].nPlaybackPosition)
                                            {
                                                chip = chipArray[0];
                                                chipArray[0] = chipArray[1];
                                                chipArray[1] = chip;
                                            }
                                            if (chipArray[1].nPlaybackPosition > chipArray[2].nPlaybackPosition)
                                            {
                                                chip = chipArray[1];
                                                chipArray[1] = chipArray[2];
                                                chipArray[2] = chip;
                                            }
                                            this.tドラムヒット処理(nTime, EPad.LBD, chipArray[0], inputEvent.nVelocity);
                                            if (chipArray[0].nPlaybackPosition == chipArray[1].nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LBD, chipArray[1], inputEvent.nVelocity);
                                            }
                                            if (chipArray[0].nPlaybackPosition == chipArray[2].nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LBD, chipArray[2], inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定LP != EJudgement.Miss) && (e判定LBD != EJudgement.Miss))
                                        {
                                            if (chipLP.nPlaybackPosition < chipLBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LBD, chipLP, inputEvent.nVelocity);
                                            }
                                            else if (chipLP.nPlaybackPosition > chipLBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LBD, chipLBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LBD, chipLP, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.LBD, chipLBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定LP != EJudgement.Miss) && (e判定BD != EJudgement.Miss))
                                        {
                                            if (chipLP.nPlaybackPosition < chipBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LBD, chipLP, inputEvent.nVelocity);
                                            }
                                            else if (chipLP.nPlaybackPosition > chipBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LBD, chipBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LBD, chipLP, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.LBD, chipBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定LBD != EJudgement.Miss) && (e判定BD != EJudgement.Miss))
                                        {
                                            if (chipLBD.nPlaybackPosition < chipBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LBD, chipLBD, inputEvent.nVelocity);
                                            }
                                            else if (chipLBD.nPlaybackPosition > chipBD.nPlaybackPosition)
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LBD, chipBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, EPad.LBD, chipLBD, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, EPad.LBD, chipBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定LP != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.LBD, chipLP, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定LBD != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.LBD, chipLBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定BD != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.LBD, chipBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (bHitted)
                                        {
                                            continue;
                                        }
                                        #endregion
                                        break;

                                    default:
                                        #region [ 全部打ち分け時のヒット処理 ]
                                        //-----------------------------
                                        if (e判定LBD != EJudgement.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, EPad.LBD, chipLBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;
                                    //-----------------------------
                                        #endregion
                                }
                                if (!bHitted)
                                    break;
                                continue;
                            }
                        //-----------------
                            #endregion
                        #endregion

                    }
                    //-----------------------------
                    #endregion
                    #region [ (B) ヒットしてなかった場合は、レーンフラッシュ、パッドアニメ、空打ち音再生を実行 ]
                    //-----------------------------
                    this.actLaneFlushD.Start((ELane)this.nパッド0Atoレーン07[nPad], ((float)inputEvent.nVelocity) / 127f);
                    this.actPad.Hit(this.nパッド0Atoパッド08[nPad]);

                    if (CDTXMania.ConfigIni.bドラム打音を発声する)
                    {
                        CDTX.CChip rChip = this.r空うちChip(EInstrumentPart.DRUMS, (EPad)nPad);
                        if (rChip != null)
                        {
                            #region [ (B1) 空打ち音が譜面で指定されているのでそれを再生する。]
                            //-----------------
                            this.tPlaySound(rChip, CSoundManager.rcPerformanceTimer.nシステム時刻, EInstrumentPart.DRUMS, CDTXMania.ConfigIni.n手動再生音量, CDTXMania.ConfigIni.b演奏音を強調する.Drums);
                            //-----------------
                            #endregion
                        }
                        else
                        {
                            #region [ (B2) 空打ち音が指定されていないので一番近いチップを探して再生する。]
                            //-----------------
                            switch (((EPad)nPad))
                            {
                                case EPad.HH:
                                    #region [ *** ]
                                    //-----------------------------
                                    {
                                        CDTX.CChip chipHC = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[0], nInputAdjustTime);
                                        CDTX.CChip chipHO = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[7], nInputAdjustTime);
                                        CDTX.CChip chipLC = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[9], nInputAdjustTime);
                                        switch (CDTXMania.ConfigIni.eHHGroup)
                                        {
                                            case EHHGroup.ハイハットのみ打ち分ける:
                                                rChip = (chipHC != null) ? chipHC : chipLC;
                                                break;

                                            case EHHGroup.左シンバルのみ打ち分ける:
                                                rChip = (chipHC != null) ? chipHC : chipHO;
                                                break;

                                            case EHHGroup.全部共通:
                                                if (chipHC != null)
                                                {
                                                    rChip = chipHC;
                                                }
                                                else if (chipHO == null)
                                                {
                                                    rChip = chipLC;
                                                }
                                                else if (chipLC == null)
                                                {
                                                    rChip = chipHO;
                                                }
                                                else if (chipHO.nPlaybackPosition < chipLC.nPlaybackPosition)
                                                {
                                                    rChip = chipHO;
                                                }
                                                else
                                                {
                                                    rChip = chipLC;
                                                }
                                                break;

                                            default:
                                                rChip = chipHC;
                                                break;
                                        }
                                    }
                                    //-----------------------------
                                    #endregion
                                    break;

                                case EPad.LT:
                                    #region [ *** ]
                                    //-----------------------------
                                    {
                                        CDTX.CChip chipLT = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[4], nInputAdjustTime);
                                        CDTX.CChip chipFT = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[5], nInputAdjustTime);
                                        if (CDTXMania.ConfigIni.eFTGroup != EFTGroup.打ち分ける)
                                            rChip = (chipLT != null) ? chipLT : chipFT;
                                        else
                                            rChip = chipLT;
                                    }
                                    //-----------------------------
                                    #endregion
                                    break;

                                case EPad.FT:
                                    #region [ *** ]
                                    //-----------------------------
                                    {
                                        CDTX.CChip chipLT = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[4], nInputAdjustTime);
                                        CDTX.CChip chipFT = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[5], nInputAdjustTime);
                                        if (CDTXMania.ConfigIni.eFTGroup != EFTGroup.打ち分ける)
                                            rChip = (chipFT != null) ? chipFT : chipLT;
                                        else
                                            rChip = chipFT;
                                    }
                                    //-----------------------------
                                    #endregion
                                    break;

                                case EPad.CY:
                                    #region [ *** ]
                                    //-----------------------------
                                    {

                                        CDTX.CChip chipCY = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[6], nInputAdjustTime);
                                        CDTX.CChip chipRD = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[8], nInputAdjustTime);
                                        if (CDTXMania.ConfigIni.eCYGroup != ECYGroup.打ち分ける)
                                            rChip = (chipCY != null) ? chipCY : chipRD;
                                        else
                                            rChip = chipCY;
                                    }
                                    //-----------------------------
                                    #endregion
                                    break;

                                case EPad.HHO:
                                    #region [ *** ]
                                    //-----------------------------
                                    {
                                        CDTX.CChip chipHC = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[0], nInputAdjustTime);
                                        CDTX.CChip chipHO = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[7], nInputAdjustTime);
                                        CDTX.CChip chipLC = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[9], nInputAdjustTime);
                                        switch (CDTXMania.ConfigIni.eHHGroup)
                                        {
                                            case EHHGroup.全部打ち分ける:
                                                rChip = chipHO;
                                                break;

                                            case EHHGroup.ハイハットのみ打ち分ける:
                                                rChip = (chipHO != null) ? chipHO : chipLC;
                                                break;

                                            case EHHGroup.左シンバルのみ打ち分ける:
                                                rChip = (chipHO != null) ? chipHO : chipHC;
                                                break;

                                            case EHHGroup.全部共通:
                                                if (chipHO != null)
                                                {
                                                    rChip = chipHO;
                                                }
                                                else if (chipHC == null)
                                                {
                                                    rChip = chipLC;
                                                }
                                                else if (chipLC == null)
                                                {
                                                    rChip = chipHC;
                                                }
                                                else if (chipHC.nPlaybackPosition < chipLC.nPlaybackPosition)
                                                {
                                                    rChip = chipHC;
                                                }
                                                else
                                                {
                                                    rChip = chipLC;
                                                }
                                                break;
                                        }
                                    }
                                    //-----------------------------
                                    #endregion
                                    break;

                                case EPad.RD:
                                    #region [ *** ]
                                    //-----------------------------
                                    {
                                        CDTX.CChip chipCY = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[6], nInputAdjustTime);
                                        CDTX.CChip chipRD = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[8], nInputAdjustTime);
                                        if (CDTXMania.ConfigIni.eCYGroup != ECYGroup.打ち分ける)
                                            rChip = (chipRD != null) ? chipRD : chipCY;
                                        else
                                            rChip = chipRD;
                                    }
                                    //-----------------------------
                                    #endregion
                                    break;

                                case EPad.LC:
                                    #region [ *** ]
                                    //-----------------------------
                                    {
                                        CDTX.CChip chipHC = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[0], nInputAdjustTime);
                                        CDTX.CChip chipHO = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[7], nInputAdjustTime);
                                        CDTX.CChip chipLC = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[9], nInputAdjustTime);
                                        switch (CDTXMania.ConfigIni.eHHGroup)
                                        {
                                            case EHHGroup.全部打ち分ける:
                                            case EHHGroup.左シンバルのみ打ち分ける:
                                                rChip = chipLC;
                                                break;

                                            case EHHGroup.ハイハットのみ打ち分ける:
                                            case EHHGroup.全部共通:
                                                if (chipLC != null)
                                                {
                                                    rChip = chipLC;
                                                }
                                                else if (chipHC == null)
                                                {
                                                    rChip = chipHO;
                                                }
                                                else if (chipHO == null)
                                                {
                                                    rChip = chipHC;
                                                }
                                                else if (chipHC.nPlaybackPosition < chipHO.nPlaybackPosition)
                                                {
                                                    rChip = chipHC;
                                                }
                                                else
                                                {
                                                    rChip = chipHO;
                                                }
                                                break;
                                        }
                                    }
                                    //-----------------------------
                                    #endregion
                                    break;

                                case EPad.BD:
                                    #region [ *** ]
                                    //-----------------------------
                                    {
                                        CDTX.CChip chipBD = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[2], nInputAdjustTime + nPedalLagTime);
                                        CDTX.CChip chipLP = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[10], nInputAdjustTime + nPedalLagTime);
                                        CDTX.CChip chipLBD = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[11], nInputAdjustTime + nPedalLagTime);
                                        switch (CDTXMania.ConfigIni.eBDGroup)
                                        {
                                            case EBDGroup.打ち分ける:
                                                rChip = chipBD;
                                                break;

                                            case EBDGroup.左右ペダルのみ打ち分ける:
                                                rChip = (chipBD != null) ? chipBD : chipLP;
                                                break;

                                            case EBDGroup.BDとLPで打ち分ける:
                                                if( chipBD != null && chipLBD != null )
                                                {
                                                    if( chipBD.nPlaybackTimeMs >= chipLBD.nPlaybackTimeMs )
                                                        rChip = chipBD;
                                                    else if( chipBD.nPlaybackTimeMs < chipLBD.nPlaybackTimeMs )
                                                        rChip = chipLBD;
                                                }
                                                else if( chipLBD != null )
                                                {
                                                    rChip = chipLBD;
                                                }
                                                else
                                                {
                                                    rChip = chipBD;
                                                }
                                                break;

                                            case EBDGroup.どっちもBD:
                                                #region [ *** ]
                                                if (chipBD != null)
                                                {
                                                    rChip = chipBD;
                                                }
                                                else if (chipLP == null)
                                                {
                                                    rChip = chipLBD;
                                                }
                                                else if (chipLBD == null)
                                                {
                                                    rChip = chipLP;
                                                }
                                                else if (chipLP.nPlaybackPosition < chipLBD.nPlaybackPosition)
                                                {
                                                    rChip = chipLP;
                                                }
                                                else
                                                {
                                                    rChip = chipLBD;
                                                }
                                                #endregion
                                                break;
                                        }
                                    }
                                    //-----------------------------
                                    #endregion
                                    break;
                                    
                                case EPad.LP:
                                    #region [ *** ]
                                    //-----------------------------
                                    {
                                        CDTX.CChip chipBD = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[2], nInputAdjustTime + nPedalLagTime );
                                        CDTX.CChip chipLP = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[10], nInputAdjustTime + nPedalLagTime );
                                        CDTX.CChip chipLBD = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[11], nInputAdjustTime + nPedalLagTime );
                                        switch (CDTXMania.ConfigIni.eBDGroup)
                                        {
                                            case EBDGroup.打ち分ける:
                                                rChip = chipLP;
                                                break;
                                            case EBDGroup.左右ペダルのみ打ち分ける:
                                                #region[左右ペダル]
                                                rChip = (chipLP != null) ? chipLP : chipLBD;
                                                #endregion
                                                break;
                                            case EBDGroup.BDとLPで打ち分ける:
                                                #region[ BDとLP ]
                                                if( chipLP != null ){ rChip = chipLP; }
                                                #endregion
                                                break;
                                            case EBDGroup.どっちもBD:
                                                #region[共通]
                                                if (chipLP != null)
                                                {
                                                    rChip = chipLP;
                                                }
                                                else if (chipLBD == null)
                                                {
                                                    rChip = chipBD;
                                                }
                                                else if (chipBD == null)
                                                {
                                                    rChip = chipLBD;
                                                }
                                                else if (chipLBD.nPlaybackPosition < chipBD.nPlaybackPosition)
                                                {
                                                    rChip = chipLBD;
                                                }
                                                else
                                                {
                                                    rChip = chipBD;
                                                }
                                                #endregion
                                                break;

                                        }
                                    }
                                    //-----------------------------
                                    #endregion
                                    break;

                                case EPad.LBD:
                                    #region [ *** ]
                                    {
                                        CDTX.CChip chipBD = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[2], nInputAdjustTime + nPedalLagTime);
                                        CDTX.CChip chipLP = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[10], nInputAdjustTime + nPedalLagTime);
                                        CDTX.CChip chipLBD = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[11], nInputAdjustTime + nPedalLagTime);
                                        switch (CDTXMania.ConfigIni.eBDGroup)
                                        {
                                            case EBDGroup.打ち分ける:
                                                rChip = chipLBD;
                                                break;
                                            case EBDGroup.左右ペダルのみ打ち分ける:
                                                #region [ *** ]
                                                rChip = (chipLBD != null) ? chipLBD : chipBD;
                                                #endregion
                                                break;
                                            case EBDGroup.BDとLPで打ち分ける:
                                                #region[ BDとLBD ]
                                                if( chipBD != null && chipLBD != null )
                                                {
                                                    if( chipBD.nPlaybackTimeMs <= chipLBD.nPlaybackTimeMs )
                                                        rChip = chipLBD;
                                                    else if( chipBD.nPlaybackTimeMs > chipLBD.nPlaybackTimeMs )
                                                        rChip = chipBD;
                                                }
                                                else if( chipLBD != null )
                                                {
                                                    rChip = chipLBD;
                                                }
                                                else
                                                {
                                                    rChip = chipBD;
                                                }
                                                #endregion
                                                break;
                                            case EBDGroup.どっちもBD:
                                                #region[ *** ]
                                                if (chipLBD != null)
                                                {
                                                    rChip = chipLBD;
                                                }
                                                else if (chipLP == null)
                                                {
                                                    rChip = chipBD;
                                                }
                                                else if (chipBD == null)
                                                {
                                                    rChip = chipLP;
                                                }
                                                else if (chipLP.nPlaybackPosition < chipBD.nPlaybackPosition)
                                                {
                                                    rChip = chipLP;
                                                }
                                                else
                                                {
                                                    rChip = chipBD;
                                                }
                                                #endregion
                                                break;
                                        }
                                    }
                                    #endregion
                                    break;



                                default:
                                    #region [ *** ]
                                    //-----------------------------
                                    rChip = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[nPad], nInputAdjustTime);
                                    //-----------------------------
                                    #endregion
                                    break;
                            }
                            if (rChip != null)
                            {
                                // 空打ち音が見つかったので再生する。
                                this.tPlaySound(rChip, CSoundManager.rcPerformanceTimer.nシステム時刻, EInstrumentPart.DRUMS, CDTXMania.ConfigIni.n手動再生音量, CDTXMania.ConfigIni.b演奏音を強調する.Drums);
                            }
                            //-----------------
                            #endregion
                        }
                    }

                    // BAD or TIGHT 時の処理。
                    if (CDTXMania.ConfigIni.bTight)
                        this.tチップのヒット処理_BadならびにTight時のMiss(EInstrumentPart.DRUMS, this.nパッド0Atoレーン07[nPad]);
                    //-----------------------------
                    #endregion
                }
            }
        }

		// tHandleInput_Drums()からメソッドを抽出したもの。
		/// <summary>
		/// chipArrayの中を, n発生位置の小さい順に並べる + nullを大きい方に退かす。セットでe判定Arrayも並べ直す。
		/// </summary>
		/// <param name="chipArray">ソート対象chip群</param>
		/// <param name="e判定Array">ソート対象e判定群</param>
		/// <param name="NumOfChips">チップ数</param>
		private static void SortChipsByNTime( CDTX.CChip[] chipArray, EJudgement[] e判定Array, int NumOfChips )
		{
			for ( int i = 0; i < NumOfChips - 1; i++ )
			{
				//num9 = 2;
				//while( num9 > num8 )
				for ( int j = NumOfChips - 1; j > i; j-- )
				{
					if ( ( chipArray[ j - 1 ] == null ) || ( ( chipArray[ j ] != null ) && ( chipArray[ j - 1 ].nPlaybackPosition > chipArray[ j ].nPlaybackPosition ) ) )
					{
						// swap
						CDTX.CChip chipTemp = chipArray[ j - 1 ];
						chipArray[ j - 1 ] = chipArray[ j ];
						chipArray[ j ] = chipTemp;
						EJudgement e判定Temp = e判定Array[ j - 1 ];
						e判定Array[ j - 1 ] = e判定Array[ j ];
						e判定Array[ j ] = e判定Temp;
					}
					//num9--;
				}
				//num8++;
			}
		}

		protected override void tGenerateBackgroundTexture()
		{
            Rectangle bgrect = new Rectangle(980, 0, 0, 0);
            if (CDTXMania.ConfigIni.bBGAEnabled)
            {
                bgrect = new Rectangle(980, 0, 278, 355);
            }
			string DefaultBgFilename = @"Graphics\7_background.jpg";
			string BgFilename = "";
			if ( ( ( CDTXMania.DTX.BACKGROUND != null ) && ( CDTXMania.DTX.BACKGROUND.Length > 0 ) ) && !CDTXMania.ConfigIni.bストイックモード )
			{
				BgFilename = CDTXMania.DTX.strFolderName + CDTXMania.DTX.BACKGROUND;
			}
			base.tGenerateBackgroundTexture( DefaultBgFilename, bgrect, BgFilename );
		}

        protected override void t進行描画_チップ_模様のみ_ドラムス(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip)
        {
            if (configIni.bDrumsEnabled)
            {
                #region [ Sudden処理 ]
                if ((CDTXMania.ConfigIni.nHidSud.Drums == 2) || (CDTXMania.ConfigIni.nHidSud.Drums == 3))
                {
                    if (pChip.nDistanceFromBar.Drums < 200)
                    {
                        pChip.bVisible = true;
                        pChip.nTransparency = 0xff;
                    }
                    else if (pChip.nDistanceFromBar.Drums < 250)
                    {
                        pChip.bVisible = true;
                        pChip.nTransparency = 0xff - ((int)((((double)(pChip.nDistanceFromBar.Drums - 200)) * 255.0) / 50.0));
                    }
                    else
                    {
                        pChip.bVisible = false;
                        pChip.nTransparency = 0;
                    }
                }
                #endregion
                #region [ Hidden処理 ]
                if ((CDTXMania.ConfigIni.nHidSud.Drums == 1) || (CDTXMania.ConfigIni.nHidSud.Drums == 3))
                {
                    if (pChip.nDistanceFromBar.Drums < 100)
                    {
                        pChip.bVisible = false;
                    }
                    else if (pChip.nDistanceFromBar.Drums < 150)
                    {
                        pChip.bVisible = true;
                        pChip.nTransparency = (int)((((double)(pChip.nDistanceFromBar.Drums - 100)) * 255.0) / 50.0);
                    }
                }
                #endregion
                #region [ ステルス処理 ]
                if (CDTXMania.ConfigIni.nHidSud.Drums == 4)
                {
                    pChip.bVisible = false;
                }
                #endregion
                if (!pChip.bHit && pChip.bVisible)
                {
                    if (this.txチップ != null)
                    {
                        this.txチップ.nTransparency = pChip.nTransparency;
                    }
                    int x = this.nチャンネルtoX座標[pChip.nChannelNumber - 0x11];

                    if (configIni.eLaneType.Drums == EType.A)
                    {
                        if (configIni.eRDPosition == ERDPosition.RCRD)
                        {
                            x = this.nチャンネルtoX座標[pChip.nChannelNumber - 0x11];
                        }
                        else if (configIni.eRDPosition == ERDPosition.RDRC)
                        {
                            x = this.nチャンネルtoX座標改[pChip.nChannelNumber - 0x11];
                        }
                    }
                    else if (configIni.eLaneType.Drums == EType.B)
                    {
                        if (configIni.eRDPosition == ERDPosition.RCRD)
                        {
                            x = this.nチャンネルtoX座標B[pChip.nChannelNumber - 0x11];
                        }
                        else if (configIni.eRDPosition == ERDPosition.RDRC)
                        {
                            x = this.nチャンネルtoX座標B改[pChip.nChannelNumber - 0x11];
                        }
                    }
                    else if (configIni.eLaneType.Drums == EType.C)
                    {
                        if (configIni.eRDPosition == ERDPosition.RCRD)
                        {
                            x = this.nチャンネルtoX座標C[pChip.nChannelNumber - 0x11];
                        }
                        else if (configIni.eRDPosition == ERDPosition.RDRC)
                        {
                            x = this.nチャンネルtoX座標C改[pChip.nChannelNumber - 0x11];
                        }
                    }
                    else if (configIni.eLaneType.Drums == EType.D)
                    {
                        if (configIni.eRDPosition == ERDPosition.RCRD)
                        {
                            x = this.nチャンネルtoX座標D[pChip.nChannelNumber - 0x11];
                        }
                        else if (configIni.eRDPosition == ERDPosition.RDRC)
                        {
                            x = this.nチャンネルtoX座標D改[pChip.nChannelNumber - 0x11];
                        }
                    }

                    if (configIni.eRDPosition == ERDPosition.RDRC)
                    {
                        if (configIni.eLaneType.Drums == EType.A)
                        {
                            x = this.nチャンネルtoX座標改[pChip.nChannelNumber - 0x11];
                        }
                        else if (configIni.eLaneType.Drums == EType.B)
                        {
                            x = this.nチャンネルtoX座標B改[pChip.nChannelNumber - 0x11];
                        }
                    }

                    int y = configIni.bReverse.Drums ? (base.nJudgeLinePosY.Drums + pChip.nDistanceFromBar.Drums) : (base.nJudgeLinePosY.Drums - pChip.nDistanceFromBar.Drums);
                    if (base.txチップ != null)
                    {
                        base.txチップ.vcScaleRatio = new Vector3((float)pChip.dbChipSizeRatio, (float)pChip.dbChipSizeRatio, 1f);
                    }
                    int num9 = this.ctChipPatternAnimation.Drums.nCurrentValue;

                    switch (pChip.nChannelNumber)
                    {
                        case 0x11:
                            x = (x + 0x10) - ((int)((32.0 * pChip.dbChipSizeRatio) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.tDraw2D(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(60 + 10, 128 + (num9 * 64), 0x2e + 10, 64));
                            }
                            break;

                        case 0x12:
                            x = (x + 0x10) - ((int)((32.0 * pChip.dbChipSizeRatio) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.tDraw2D(CDTXMania.app.Device, x - 5, y - 32, new Rectangle(0x6a + 20, 128 + (num9 * 64), 0x36 + 10, 64));
                            }
                            break;

                        case 0x13:
                            x = (x + 0x16) - ((int)((44.0 * pChip.dbChipSizeRatio) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.tDraw2D(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(0, 128 + (num9 * 0x40), 60 + 10, 0x40));
                            }
                            break;

                        case 0x14:
                            x = (x + 0x10) - ((int)((32.0 * pChip.dbChipSizeRatio) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.tDraw2D(CDTXMania.app.Device, x - 5, y - 32, new Rectangle(160 + 30, 128 + (num9 * 0x40), 0x2e + 10, 64));
                            }
                            break;

                        case 0x15:
                            x = (x + 0x10) - ((int)((32.0 * pChip.dbChipSizeRatio) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.tDraw2D(CDTXMania.app.Device, x - 5, y - 32, new Rectangle(0xce + 40, 128 + (num9 * 0x40), 0x2e + 10, 64));
                            }
                            break;

                        case 0x16:
                            x = (x + 19) - ((int)((38.0 * pChip.dbChipSizeRatio) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.tDraw2D(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(298 + 60, 128 + (num9 * 64), 64 + 10, 64));
                            }
                            break;

                        case 0x17:
                            x = (x + 0x10) - ((int)((32.0 * pChip.dbChipSizeRatio) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.tDraw2D(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(0xfc + 50, 128 + (num9 * 64), 0x2e + 10, 0x40));
                            }
                            break;

                        case 0x18:
                            x = (x + 13) - ((int)((26.0 * pChip.dbChipSizeRatio) / 2.0));
                            if (this.txチップ != null)
                            {
                                switch (configIni.eHHOGraphics.Drums)
                                {
                                    case EType.A:
                                        x = (x + 14) - ((int)((26.0 * pChip.dbChipSizeRatio) / 2.0));
                                        this.txチップ.tDraw2D(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(0x200 + 100, 128 + (num9 * 64), 0x26 + 10, 64));
                                        break;

                                    /*
                                case EType.B:
                                    x = (x + 14) - ((int)((26.0 * pChip.dbChipSizeRatio) / 2.0));
                                    this.txチップ.tDraw2D(CDTXMania.app.Device, x, y - 32, new Rectangle(0x200, 128 + (num9 * 64), 0x26, 64));
                                    break;
                                     */

                                    case EType.C:
                                        x = (x + 13) - ((int)((32.0 * pChip.dbChipSizeRatio) / 2.0));
                                        this.txチップ.tDraw2D(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(60 + 10, 128 + (num9 * 64), 0x2e + 10, 64));
                                        break;
                                }
                            }
                            break;

                        case 0x19:
                            x = (x + 13) - ((int)((26.0 * pChip.dbChipSizeRatio) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.tDraw2D(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(0x16a + 70, 128 + (num9 * 64), 0x26 + 10, 0x40));
                            }
                            break;

                        case 0x1a:
                            x = (x + 0x13) - ((int)((38.0 * pChip.dbChipSizeRatio) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.tDraw2D(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(448 + 90, 128 + (num9 * 64), 64 + 10, 64));
                            }
                            break;

                        case 0x1b:
                            x = (x + 0x13) - ((int)((38.0 * pChip.dbChipSizeRatio) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.tDraw2D(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(550 + 110, 128 + (num9 * 64), 0x30 + 10, 64));
                            }
                            break;

                        case 0x1c:
                            x = (x + 0x13) - ((int)((38.0 * pChip.dbChipSizeRatio) / 2.0));
                            if (this.txチップ != null)
                            {

                                if (configIni.eLBDGraphics.Drums == EType.A)
                                {
                                    this.txチップ.tDraw2D(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(550 + 110, 128 + (num9 * 64), 0x30, 0x40));
                                }
                                else if (configIni.eLBDGraphics.Drums == EType.B)
                                {
                                    this.txチップ.tDraw2D(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(400 + 80, 128 + (num9 * 64), 0x30 + 10, 0x40));
                                }
                            }
                            break;
                    }
                    if (this.txチップ != null)
                    {
                        this.txチップ.vcScaleRatio = new Vector3(1f, 1f, 1f);
                        this.txチップ.nTransparency = 0xff;
                    }
                }

                /*
				int indexSevenLanes = this.nチャンネル0Atoレーン07[ pChip.nChannelNumber - 0x11 ];
				if ( ( configIni.bAutoPlay[ indexSevenLanes ] && !pChip.bHit ) && ( pChip.nDistanceFromBar.Drums < 0 ) )
				{
					pChip.bHit = true;
					this.actLaneFlushD.Start( (ELane) indexSevenLanes, ( (float) CInputManager.n通常音量 ) / 127f );
					bool flag = this.bInFillIn;
					bool flag2 = this.bInFillIn && this.bフィルイン区間の最後のChipである( pChip );
					//bool flag3 = flag2;
                    // #31602 2013.6.24 yyagi 判定ラインの表示位置をずらしたら、チップのヒットエフェクトの表示もずらすために、nJudgeLine..を追加
                    this.actChipFireD.Start( (ELane)indexSevenLanes, flag, flag2, flag2, nJudgeLinePosY_delta.Drums );
					this.actPad.Hit( this.nチャンネル0Atoパッド08[ pChip.nChannelNumber - 0x11 ] );
					this.tPlaySound( pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs, EInstrumentPart.DRUMS, dTX.nモニタを考慮した音量( EInstrumentPart.DRUMS ) );
					this.tProcessChipHit( pChip.nPlaybackTimeMs, pChip );
				}
                */
                return;
            }	// end of "if configIni.bDrumsEnabled"
            if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
            {
                //this.tPlaySound(pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs, EInstrumentPart.DRUMS, dTX.nモニタを考慮した音量(EInstrumentPart.DRUMS));
                pChip.bHit = true;
            }
        }
		protected override void tUpdateAndDraw_Chip_Drums( CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip )
		{
			if( configIni.bDrumsEnabled )
			{
				#region [ Sudden処理 ]
                if( ( CDTXMania.ConfigIni.nHidSud.Drums == 2 ) || ( CDTXMania.ConfigIni.nHidSud.Drums == 3 ) )
				{
					if( pChip.nDistanceFromBar.Drums < 200 )
					{
						pChip.bVisible = true;
						pChip.nTransparency = 0xff;
					}
					else if( pChip.nDistanceFromBar.Drums < 250 )
					{
						pChip.bVisible = true;
						pChip.nTransparency = 0xff - ( (int) ( ( ( (double) ( pChip.nDistanceFromBar.Drums - 200 ) ) * 255.0 ) / 50.0 ) );
					}
					else
					{
						pChip.bVisible = false;
						pChip.nTransparency = 0;
					}
				}
				#endregion
				#region [ Hidden処理 ]
                if( ( CDTXMania.ConfigIni.nHidSud.Drums == 1 ) || ( CDTXMania.ConfigIni.nHidSud.Drums == 3 ) )
				{
					if( pChip.nDistanceFromBar.Drums < 100 )
					{
						pChip.bVisible = false;
					}
					else if( pChip.nDistanceFromBar.Drums < 150 )
					{
						pChip.bVisible = true;
						pChip.nTransparency = (int) ( ( ( (double) ( pChip.nDistanceFromBar.Drums - 100 ) ) * 255.0 ) / 50.0 );
					}
				}
				#endregion
                #region [ ステルス処理 ]
                if( CDTXMania.ConfigIni.nHidSud.Drums == 4 )
                {
                        pChip.bVisible = false;
                }
                #endregion
				if( !pChip.bHit && pChip.bVisible )
                {
                    if( this.txチップ != null )
                    {
                        this.txチップ.nTransparency = pChip.nTransparency;
                    }
                    int x = this.nチャンネルtoX座標[ pChip.nChannelNumber - 0x11 ];

                    if( configIni.eLaneType.Drums == EType.A )
                    {
                        if (configIni.eRDPosition == ERDPosition.RCRD)
                        {
                            x = this.nチャンネルtoX座標[ pChip.nChannelNumber - 0x11 ];
                        }
                        else if( configIni.eRDPosition == ERDPosition.RDRC )
                        {
                            x = this.nチャンネルtoX座標改[ pChip.nChannelNumber - 0x11 ];
                        }
                    }
                    else if( configIni.eLaneType.Drums == EType.B )
                    {
                        if( configIni.eRDPosition == ERDPosition.RCRD )
                        {
                            x = this.nチャンネルtoX座標B[ pChip.nChannelNumber - 0x11 ];
                        }
                        else if( configIni.eRDPosition == ERDPosition.RDRC )
                        {
                            x = this.nチャンネルtoX座標B改[ pChip.nChannelNumber - 0x11 ];
                        }
                    }
                    else if( configIni.eLaneType.Drums == EType.C )
                    {
                        if( configIni.eRDPosition == ERDPosition.RCRD )
                        {
                            x = this.nチャンネルtoX座標C[ pChip.nChannelNumber - 0x11 ];
                        }
                        else if( configIni.eRDPosition == ERDPosition.RDRC )
                        {
                            x = this.nチャンネルtoX座標C改[ pChip.nChannelNumber - 0x11 ];
                        }
                    }
                    else if( configIni.eLaneType.Drums == EType.D )
                    {
                        if( configIni.eRDPosition == ERDPosition.RCRD )
                        {
                            x = this.nチャンネルtoX座標D[ pChip.nChannelNumber - 0x11 ];
                        }
                        else if( configIni.eRDPosition == ERDPosition.RDRC )
                        {
                            x = this.nチャンネルtoX座標D改[ pChip.nChannelNumber - 0x11 ];
                        }
                    }

                    if( configIni.eRDPosition == ERDPosition.RDRC )
                    {
                        if( configIni.eLaneType.Drums == EType.A )
                        {
                            x = this.nチャンネルtoX座標改[ pChip.nChannelNumber - 0x11 ];
                        }
                        else if( configIni.eLaneType.Drums == EType.B )
                        {
                            x = this.nチャンネルtoX座標B改[ pChip.nChannelNumber - 0x11 ];
                        }
                    }

                    int y = configIni.bReverse.Drums ? ( base.nJudgeLinePosY.Drums + pChip.nDistanceFromBar.Drums ) : ( base.nJudgeLinePosY.Drums - pChip.nDistanceFromBar.Drums );
                    if( base.txチップ != null )
                    {
                        base.txチップ.vcScaleRatio = new Vector3( ( float )pChip.dbChipSizeRatio, ( float )pChip.dbChipSizeRatio, 1f );
                    }
                    int num9 = this.ctChipPatternAnimation.Drums.nCurrentValue;

                    switch( pChip.nChannelNumber )
                    {
                        case 0x11:
                            x = ( x + 0x10 ) - ( ( int )( ( 32.0 * pChip.dbChipSizeRatio ) / 2.0 ) );
                            if( this.txチップ != null )
                            {
                                this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 60 + 10, 0, 0x2e + 10, 64 ) );

                                if( pChip.bBonusChip )
                                    this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 60 + 10, 64, 0x2e + 10, 64 ) );
                            }
                            break;

                        case 0x12:
                            x = ( x + 0x10 ) - ( ( int )( ( 32.0 * pChip.dbChipSizeRatio ) / 2.0 ) );
                            if (this.txチップ != null)
                            {
                                this.txチップ.tDraw2D(CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0x6a + 20, 0, 0x36 +10, 64 ) );

                                if( pChip.bBonusChip )
                                    this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0x6a + 20, 64, 0x36 + 10, 64 ) );
                            }
                            break;

                        case 0x13:
                            x = ( x + 0x16 ) - ( ( int )( ( 44.0 * pChip.dbChipSizeRatio ) / 2.0 ) );
                            if( this.txチップ != null )
                            {
                                this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0, 0, 60 + 10, 64 ) );

                                if( pChip.bBonusChip )
                                    this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0, 64, 60 + 10, 64 ) );
                            }
                            break;

                        case 0x14:
                            x = ( x + 0x10 ) - ( ( int )( ( 32.0 * pChip.dbChipSizeRatio ) / 2.0 ) );
                            if( this.txチップ != null )
                            {
                                this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 160 + 30, 0, 0x2e + 10, 64 ) );

                                if( pChip.bBonusChip )
                                    this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 160 + 30, 64, 0x2e + 10, 64 ) );
                            }
                            break;

                        case 0x15:
                            x = ( x + 0x10 ) - ( ( int )( ( 32.0 * pChip.dbChipSizeRatio ) / 2.0 ) );
                            if (this.txチップ != null)
                            {
                                this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0xce + 40, 0, 0x2e + 10, 64 ) );

                                if( pChip.bBonusChip )
                                    this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0xce + 40, 64, 0x2e + 10, 64 ) );
                            }
                            break;

                        case 0x16:
                            x = ( x + 19 ) - ( ( int )( ( 38.0 * pChip.dbChipSizeRatio ) / 2.0 ) );
                            if( this.txチップ != null )
                            {
                                this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 298 + 60, 0, 0x40 + 10, 64 ) );

                                if( pChip.bBonusChip )
                                    this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 298 + 60, 64, 0x40 + 10, 64 ) );
                            }
                            break;

                        case 0x17:
                            x = ( x + 0x10 ) - ( ( int )( ( 32.0 * pChip.dbChipSizeRatio ) / 2.0 ) );
                            if( this.txチップ != null )
                            {
                                this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0xfc + 50, 0, 0x2e + 10, 64 ) );

                                if( pChip.bBonusChip )
                                    this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0xfc + 50, 64, 0x2e + 10, 64 ) );
                            }
                            break;

                        case 0x18:
                            x = ( x + 13 ) - ( ( int )( ( 26.0 * pChip.dbChipSizeRatio ) / 2.0 ) );
                            if( this.txチップ != null )
                            {
                                switch( configIni.eHHOGraphics.Drums )
                                {
                                    case EType.A:
                                        x = ( x + 14 ) - ( ( int )( ( 26.0 * pChip.dbChipSizeRatio ) / 2.0 ) );
                                        this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0x200 + 100, 0, 0x26 + 10, 64 ) );
                                        if( pChip.bBonusChip )
                                            this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0x200 + 100, 64, 0x26 + 10, 64 ) );
                                        break;

                                    case EType.B:
                                        x = ( x + 14 ) - ( ( int )( ( 26.0 * pChip.dbChipSizeRatio ) / 2.0 ) );
                                        this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0x200 + 100, 0, 0x26 + 10, 64 ) );
                                        if( pChip.bBonusChip )
                                            this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0x200 + 100, 64, 0x26 + 10, 64 ) );
                                        break;

                                    case EType.C:
                                        x = ( x + 13 ) - ( ( int )( ( 32.0 * pChip.dbChipSizeRatio ) / 2.0 ) );
                                        this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 60 + 10, 0, 0x2e + 10, 64 ) );
                                        if( pChip.bBonusChip )
                                            this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 60 + 100, 64, 0x2e + 10, 64 ) );
                                        break;
                                }
                            }
                                break;

                        case 0x19:
                            x = ( x + 13 ) - ( ( int )( ( 26.0 * pChip.dbChipSizeRatio ) / 2.0 ) );
                            if( this.txチップ != null )
                            {
                                this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0x16a + 70, 0, 0x26 + 10, 64 ) );

                                if( pChip.bBonusChip )
                                    this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle( 0x16a + 70, 64, 0x26 + 10, 0x40 ) );
                            }
                            break;

                        case 0x1a:
                            x = ( x + 0x13 ) - ( ( int )( ( 38.0 * pChip.dbChipSizeRatio ) / 2.0 ) );
                            if (this.txチップ != null)
                            {
                                this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 448 + 90, 0, 64 + 10, 64 ) );

                                if( pChip.bBonusChip )
                                    this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle( 448 + 90, 64, 64 + 10, 64 ) );
                            }
                            break;

                        case 0x1b:
                            x = ( x + 0x13 ) - ( ( int )( ( 38.0 * pChip.dbChipSizeRatio ) / 2.0 ) );
                            if (this.txチップ != null)
                            {
                                this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 550 + 110, 0, 0x30 + 10, 64 ) );
                                
                                if( pChip.bBonusChip )
                                    this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 550 + 110, 64, 0x30 + 10, 64 ) );
                            }
                            break;

                        case 0x1c:
                            x = ( x + 0x13 ) - ( ( int )( ( 38.0 * pChip.dbChipSizeRatio ) / 2.0 ) );
                            if( this.txチップ != null )
                            {
                                if( configIni.eLBDGraphics.Drums == EType.A )
                                {
                                    this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 550 + 110, 0, 0x30 + 10, 64 ) );
                                    if( pChip.bBonusChip )
                                        this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 550 + 110, 64, 0x30 + 10, 64 ) );
                                }
                                else if( configIni.eLBDGraphics.Drums == EType.B )
                                {
                                    this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 400 + 80, 0, 0x30 + 10, 64 ) );
                                    if( pChip.bBonusChip )
                                        this.txチップ.tDraw2D( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 400 + 80, 64, 0x30 + 10, 64 ) );
                                }
                            }
                            break;
                    }
                    if( this.txチップ != null )
                    {
                        this.txチップ.vcScaleRatio = new Vector3( 1f, 1f, 1f );
                        this.txチップ.nTransparency = 0xff;
                    }
                }

				int indexSevenLanes = this.nチャンネル0Atoレーン07[ pChip.nChannelNumber - 0x11 ];
				// #35411 chnmr0 modified
				bool autoPlayCondition = ( configIni.bAutoPlay[ indexSevenLanes ] && !pChip.bHit );
                bool UsePerfectGhost = true;
                long ghostLag = 0;

                if( CDTXMania.ConfigIni.eAutoGhost.Drums != EAutoGhostData.PERFECT &&
                    CDTXMania.listAutoGhostLag.Drums != null &&
                    0 <= pChip.n楽器パートでの出現順 && pChip.n楽器パートでの出現順 < CDTXMania.listAutoGhostLag.Drums.Count)

                {
                    // ゴーストデータが有効 : ラグに合わせて判定
					ghostLag = CDTXMania.listAutoGhostLag.Drums[pChip.n楽器パートでの出現順];
					ghostLag = (ghostLag & 255) - 128;
					ghostLag -= this.nInputAdjustTimeMs.Drums;
					autoPlayCondition &= !pChip.bHit && (ghostLag + pChip.nPlaybackTimeMs <= CSoundManager.rcPerformanceTimer.n現在時刻ms);
                    UsePerfectGhost = false;
                }
                if( UsePerfectGhost )
                {
                    // 従来の AUTO : バー下で判定
                    autoPlayCondition &= ( pChip.nDistanceFromBar.Drums < 0 );
                }

				if ( autoPlayCondition )
				{
					pChip.bHit = true;
					this.actLaneFlushD.Start( (ELane) indexSevenLanes, ( (float) CInputManager.n通常音量 ) / 127f );
					bool flag = this.bInFillIn;
					bool flag2 = this.bInFillIn && this.bフィルイン区間の最後のChipである( pChip );
					//bool flag3 = flag2;
					// #31602 2013.6.24 yyagi 判定ラインの表示位置をずらしたら、チップのヒットエフェクトの表示もずらすために、nJudgeLine..を追加
					this.actChipFireD.Start( (ELane)indexSevenLanes, flag, flag2, flag2, nJudgeLinePosY_delta.Drums );
					this.actPad.Hit( this.nチャンネル0Atoパッド08[ pChip.nChannelNumber - 0x11 ] );
					this.tPlaySound( pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs + ghostLag, EInstrumentPart.DRUMS, dTX.nモニタを考慮した音量( EInstrumentPart.DRUMS ) );
					this.tProcessChipHit(pChip.nPlaybackTimeMs + ghostLag, pChip);
					//cInvisibleChip.StartSemiInvisible( EInstrumentPart.DRUMS );
				}
                // #35411 modify end
                
                // #35411 2015.08.21 chnmr0 add
                // 目標値グラフにゴーストの達成率を渡す
                if (CDTXMania.ConfigIni.eTargetGhost.Drums != ETargetGhostData.NONE &&
                    CDTXMania.listTargetGhsotLag.Drums != null)
                {
                    double val = 0;
                    if (CDTXMania.ConfigIni.eTargetGhost.Drums == ETargetGhostData.ONLINE)
                    {
                        if (CDTXMania.DTX.nVisibleChipsCount.Drums > 0)
                        {
                        	// Online Stats の計算式
                            val = 100 *
                                (this.nヒット数_TargetGhost.Drums.Perfect * 17 +
                                 this.nヒット数_TargetGhost.Drums.Great * 7 +
                                 this.n最大コンボ数_TargetGhost.Drums * 3) / (20.0 * CDTXMania.DTX.nVisibleChipsCount.Drums);
                        }
                    }
                    else
                    {
                        if( CDTXMania.ConfigIni.nSkillMode == 0 )
                        {
                            val = CScoreIni.tCalculatePlayingSkillOld(
                                CDTXMania.DTX.nVisibleChipsCount.Drums,
                                this.nヒット数_TargetGhost.Drums.Perfect,
                                this.nヒット数_TargetGhost.Drums.Great,
                                this.nヒット数_TargetGhost.Drums.Good,
                                this.nヒット数_TargetGhost.Drums.Poor,
                                this.nヒット数_TargetGhost.Drums.Miss,
                                this.n最大コンボ数_TargetGhost.Drums,
                                EInstrumentPart.DRUMS, new STAUTOPLAY());
                        }
                        else
                        {
                            val = CScoreIni.tCalculatePlayingSkill(
                                CDTXMania.DTX.nVisibleChipsCount.Drums,
                                this.nヒット数_TargetGhost.Drums.Perfect,
                                this.nヒット数_TargetGhost.Drums.Great,
                                this.nヒット数_TargetGhost.Drums.Good,
                                this.nヒット数_TargetGhost.Drums.Poor,
                                this.nヒット数_TargetGhost.Drums.Miss,
                                this.n最大コンボ数_TargetGhost.Drums,
                                EInstrumentPart.DRUMS, new STAUTOPLAY());
                        }

                    }
                    if (val < 0) val = 0;
                    if (val > 100) val = 100;
                    this.actGraph.dbGraphValue_Goal = val;
                }
				return;
			}	// end of "if configIni.bDrumsEnabled"
			if( !pChip.bHit && ( pChip.nDistanceFromBar.Drums < 0 ) )
			{
                this.tPlaySound( pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs, EInstrumentPart.DRUMS, dTX.nモニタを考慮した音量( EInstrumentPart.DRUMS ) );
				pChip.bHit = true;
			}
		}
        protected override void t進行描画_チップ_ギターベース(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip, EInstrumentPart inst)
		{
			base.t進行描画_チップ_ギターベース( configIni, ref dTX, ref pChip, inst,
				95, 374, 57, 412, 509, 400,
				268, 144, 76, 6,
				24, 509, 561, 400, 452, 26, 24 );
		}

        /*
		protected override void tUpdateAndDraw_Chip_Guitar_Wailing( CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip )
		{
			if ( configIni.bGuitarEnabled )
			{
				//if ( configIni.bSudden.Guitar )
				//{
				//    pChip.bVisible = pChip.nDistanceFromBar.Guitar < 200;
				//}
				//if ( configIni.bHidden.Guitar && ( pChip.nDistanceFromBar.Guitar < 100 ) )
				//{
				//    pChip.bVisible = false;
				//}

				// 後日、以下の部分を何とかCStage演奏画面共通.csに移したい。
				if ( !pChip.bHit && pChip.bVisible )
				{
					int[] y_base = { 0x5f, 0x176 };		// 判定バーのY座標: ドラム画面かギター画面かで変わる値
					int offset = 0x39;					// ドラム画面かギター画面かで変わる値

					const int WailingWidth = 20;		// ウェイリングチップ画像の幅: 4種全て同じ値
					const int WailingHeight = 50;		// ウェイリングチップ画像の高さ: 4種全て同じ値
					const int baseTextureOffsetX = 268;	// テクスチャ画像中のウェイリングチップ画像の位置X: ドラム画面かギター画面かで変わる値
					const int baseTextureOffsetY = 174;	// テクスチャ画像中のウェイリングチップ画像の位置Y: ドラム画面かギター画面かで変わる値
					const int drawX = 588;				// ウェイリングチップ描画位置X座標: 4種全て異なる値

					const int numA = 25;				// 4種全て同じ値
					int y = configIni.bReverse.Guitar ? ( y_base[1] - pChip.nDistanceFromBar.Guitar ) : ( y_base[0] + pChip.nDistanceFromBar.Guitar );
					int numB = y - offset;				// 4種全て同じ定義
					int numC = 0;						// 4種全て同じ初期値
					const int numD = 355;				// ドラム画面かギター画面かで変わる値
					if ( ( numB < ( numD + numA ) ) && ( numB > -numA ) )	// 以下のロジックは4種全て同じ
					{
						int c = this.ctWailingChipPatternAnimation.nCurrentValue;
						Rectangle rect = new Rectangle( baseTextureOffsetX + ( c * WailingWidth ), baseTextureOffsetY, WailingWidth, WailingHeight);
						if ( numB < numA )
						{
							rect.Y += numA - numB;
							rect.Height -= numA - numB;
							numC = numA - numB;
						}
						if ( numB > ( numD - numA ) )
						{
							rect.Height -= numB - ( numD - numA );
						}
						if ( ( rect.Bottom > rect.Top ) && ( this.txチップ != null ) )
						{
							this.txチップ.tDraw2D( CDTXMania.app.Device, drawX, ( y - numA ) + numC, rect );
						}
					}
				}
				//    if ( !pChip.bHit && ( pChip.nDistanceFromBar.Guitar < 0 ) )
				//    {
				//        if ( pChip.nDistanceFromBar.Guitar < -234 )	// #25253 2011.5.29 yyagi: Don't set pChip.bHit=true for wailing at once. It need to 1sec-delay (234pix per 1sec). 
				//        {
				//            pChip.bHit = true;
				//        }
				//        if ( configIni.bAutoPlay.Guitar )
				//        {
				//            pChip.bHit = true;						// #25253 2011.5.29 yyagi: Set pChip.bHit=true if autoplay.
				//            this.actWailingBonus.Start( EInstrumentPart.GUITAR, this.r現在の歓声Chip.Guitar );
				//        }
				//    }
				//    return;
				//}
				//pChip.bHit = true;
			}
			base.tUpdateAndDraw_Chip_Guitar_Wailing( configIni, ref dTX, ref pChip );
		}
         */
		protected override void t進行描画_チップ_フィルイン( CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip )
		{
			if ( !pChip.bHit && ( pChip.nDistanceFromBar.Drums < 0 ) )
			{
				pChip.bHit = true;
				switch ( pChip.nIntegerValue )
				{
					case 0x01:	// フィルイン開始
                        this.bEndFillIn = true;
						if ( configIni.bFillInEnabled )
						{
							this.bInFillIn = true;
						}
						break;

					case 0x02:	// フィルイン終了
                        this.bEndFillIn = true;
						if ( configIni.bFillInEnabled )
						{
							this.bInFillIn = false;
						}
                        if (((this.actCombo.nCurrentCombo.Drums > 0) || configIni.bAllDrumsAreAutoPlay) && configIni.b歓声を発声する)
                        {
                            this.actAVI.Start(bInFillIn);
                            if (this.r現在の歓声Chip.Drums != null)
                            {
                                dTX.tPlayChip(this.r現在の歓声Chip.Drums, CSoundManager.rcPerformanceTimer.nシステム時刻, (int)ELane.BGM, dTX.nモニタを考慮した音量(EInstrumentPart.UNKNOWN));
                            }
                            else
                            {
                                CDTXMania.Skin.sound歓声音.tPlay();
                                CDTXMania.Skin.sound歓声音.n位置_次に鳴るサウンド = 0;
                            }
                            //if (CDTXMania.ConfigIni.nSkillMode == 1)
                            //    this.actScore.nCurrentTrueScore.Drums += 500;
                        }
						break;
                    case 0x03:
                        this.bChorusSection = true;
                        break;
                    case 0x04:
                        this.bChorusSection = false;
                        break;
                    case 0x05:
                        if (configIni.bFillInEnabled)
                        {
                            this.bChorusSection = true;
                        }
                        if (((this.actCombo.nCurrentCombo.Drums > 0) || configIni.bAllDrumsAreAutoPlay) && configIni.b歓声を発声する && configIni.DisplayBonusEffects)
                        {
                            this.actAVI.Start(true);
                            if (this.r現在の歓声Chip.Drums != null)
                            {
                                dTX.tPlayChip(this.r現在の歓声Chip.Drums, CSoundManager.rcPerformanceTimer.nシステム時刻, (int)ELane.BGM, dTX.nモニタを考慮した音量(EInstrumentPart.UNKNOWN));
                            }
                            else
                            {
                                CDTXMania.Skin.sound歓声音.tPlay();
                                CDTXMania.Skin.sound歓声音.n位置_次に鳴るサウンド = 0;
                            }
                        }
                        break;
                    case 0x06:
                        if (configIni.bFillInEnabled)
                        {
                            this.bChorusSection = false;
                        }
                        if (((this.actCombo.nCurrentCombo.Drums > 0) || configIni.bAllDrumsAreAutoPlay) && configIni.b歓声を発声する && configIni.DisplayBonusEffects)
                        {
                            this.actAVI.Start(true);
                            if (this.r現在の歓声Chip.Drums != null)
                            {
                                dTX.tPlayChip(this.r現在の歓声Chip.Drums, CSoundManager.rcPerformanceTimer.nシステム時刻, (int)ELane.BGM, dTX.nモニタを考慮した音量(EInstrumentPart.UNKNOWN));
                            }
                            else
                            {
                                CDTXMania.Skin.sound歓声音.tPlay();
                                CDTXMania.Skin.sound歓声音.n位置_次に鳴るサウンド = 0;
                            }
                        }
                        break;
				}
			}
		}

        
        protected override void t進行描画_チップ_ボーナス(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip)
        {

        }
        
        public void tボーナスチップのヒット処理( CConfigIni configIni, CDTX dTX, CDTX.CChip pChip )
        {
            pChip.bHit = true;

            //if ((this.actCombo.nCurrentCombo.Drums > 0) && configIni.b歓声を発声する )
            if( pChip.bBonusChip )
            {
                bBonus = true;
                switch( pChip.nChannelNumber )
                {
                    //case 0x01: //LC
                    //    this.actPad.Start(0, true, pChip.nChannelNumber);
                    //    break;

                    //case 0x02: //HH
                    //    this.actPad.Start(1, true, pChip.nChannelNumber);
                    //    break;

                    //case 0x03: //LP
                    //    this.actPad.Start(2, true, pChip.nChannelNumber);
                    //    break;

                    //case 0x04: //SD
                    //    this.actPad.Start(3, true, pChip.nChannelNumber);
                    //    break;

                    //case 0x05: //HT
                    //    this.actPad.Start(4, true, pChip.nChannelNumber);
                    //    break;

                    //case 0x06: //BD
                    //    this.actPad.Start(5, true, pChip.nChannelNumber);
                    //    break;

                    //case 0x07: //LT
                    //    this.actPad.Start(6, true, pChip.nChannelNumber);
                    //    break;

                    //case 0x08: //FT
                    //    this.actPad.Start(7, true, pChip.nChannelNumber);
                    //    break;

                    //case 0x09: //CY
                    //    this.actPad.Start(8, true, pChip.nChannelNumber);
                    //    break;

                    //case 0x0A: //RD
                    //    this.actPad.Start(9, true, pChip.nChannelNumber);
                    //    break;

                    case 0x1A: //LC
                        this.actPad.Start( 0, true, pChip.nChannelNumber );
                        break;

                    case 0x11: //HH
                    case 0x18:
                        this.actPad.Start( 1, true, pChip.nChannelNumber );
                        break;

                    case 0x1B: //LP
                    case 0x1C:
                        this.actPad.Start( 2, true, pChip.nChannelNumber );
                        break;

                    case 0x12: //SD
                        this.actPad.Start( 3, true, pChip.nChannelNumber );
                        break;

                    case 0x14: //HT
                        this.actPad.Start( 4, true, pChip.nChannelNumber );
                        break;

                    case 0x13: //BD
                        this.actPad.Start( 5, true, pChip.nChannelNumber );
                        break;

                    case 0x15: //LT
                        this.actPad.Start( 6, true, pChip.nChannelNumber );
                        break;

                    case 0x17: //FT
                        this.actPad.Start( 7, true, pChip.nChannelNumber );
                        break;

                    case 0x16: //CY
                        this.actPad.Start( 8, true, pChip.nChannelNumber );
                        break;

                    case 0x19: //RD
                        this.actPad.Start(9, true, pChip.nChannelNumber);
                        break;
                    default:
                        break;
                }
                if( configIni.DisplayBonusEffects )
                {
                    this.actAVI.Start( true );
                    CDTXMania.Skin.sound歓声音.tPlay();
                    CDTXMania.Skin.sound歓声音.n位置_次に鳴るサウンド = 0;
                }
                if( CDTXMania.ConfigIni.nSkillMode == 1 && ( !CDTXMania.ConfigIni.bAllDrumsAreAutoPlay || CDTXMania.ConfigIni.bAutoAddGage ) )
                    this.actScore.Add( EInstrumentPart.DRUMS, bIsAutoPlay, 500L );
            }


        }

        /*
		protected override void t進行描画_チップ_ベース_ウェイリング( CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip )
		{
			if ( configIni.bGuitarEnabled )
			{
				//if ( configIni.bSudden.Bass )
				//{
				//    pChip.bVisible = pChip.nDistanceFromBar.Bass < 200;
				//}
				//if ( configIni.bHidden.Bass && ( pChip.nDistanceFromBar.Bass < 100 ) )
				//{
				//    pChip.bVisible = false;
				//}

				//
				// 後日、以下の部分を何とかCStage演奏画面共通.csに移したい。
				//
				if ( !pChip.bHit && pChip.bVisible )
				{
					int[] y_base = { 0x5f, 0x176 };		// 判定バーのY座標: ドラム画面かギター画面かで変わる値
					int offset = 0x39;					// ドラム画面かギター画面かで変わる値

					const int WailingWidth = 20;		// ウェイリングチップ画像の幅: 4種全て同じ値
					const int WailingHeight = 50;		// ウェイリングチップ画像の高さ: 4種全て同じ値
					const int baseTextureOffsetX = 268;	// テクスチャ画像中のウェイリングチップ画像の位置X: ドラム画面かギター画面かで変わる値
					const int baseTextureOffsetY = 174;	// テクスチャ画像中のウェイリングチップ画像の位置Y: ドラム画面かギター画面かで変わる値
					const int drawX = 479;				// ウェイリングチップ描画位置X座標: 4種全て異なる値

					const int numA = 25;				// 4種全て同じ値
					int y = configIni.bReverse.Bass ? ( y_base[ 1 ] - pChip.nDistanceFromBar.Bass ) : ( y_base[ 0 ] + pChip.nDistanceFromBar.Bass );
					int numB = y - offset;				// 4種全て同じ定義
					int numC = 0;						// 4種全て同じ初期値
					const int numD = 355;				// ドラム画面かギター画面かで変わる値
					if ( ( numB < ( numD + numA ) ) && ( numB > -numA ) )	// 以下のロジックは4種全て同じ
					{
						int c = this.ctWailingChipPatternAnimation.nCurrentValue;
						Rectangle rect = new Rectangle( baseTextureOffsetX + ( c * WailingWidth ), baseTextureOffsetY, WailingWidth, WailingHeight );
						if ( numB < numA )
						{
							rect.Y += numA - numB;
							rect.Height -= numA - numB;
							numC = numA - numB;
						}
						if ( numB > ( numD - numA ) )
						{
							rect.Height -= numB - ( numD - numA );
						}
						if ( ( rect.Bottom > rect.Top ) && ( this.txチップ != null ) )
						{
							this.txチップ.tDraw2D( CDTXMania.app.Device, drawX, ( y - numA ) + numC, rect );
						}
					}
				}
				//    if ( !pChip.bHit && ( pChip.nDistanceFromBar.Bass < 0 ) )
				//    {
				//        if ( pChip.nDistanceFromBar.Bass < -234 )	// #25253 2011.5.29 yyagi: Don't set pChip.bHit=true for wailing at once. It need to 1sec-delay (234pix per 1sec).
				//        {
				//            pChip.bHit = true;
				//        }
				//        if ( configIni.bAutoPlay.Bass )
				//        {
				//            this.actWailingBonus.Start( EInstrumentPart.BASS, this.r現在の歓声Chip.Bass );
				//            pChip.bHit = true;						// #25253 2011.5.29 yyagi: Set pChip.bHit=true if autoplay.
				//        }
				//    }
				//    return;
				//}
				//pChip.bHit = true;
			}
				base.t進行描画_チップ_ベース_ウェイリング( configIni, ref dTX, ref pChip);
		}
         */

        protected override void t進行描画_チップ_空打ち音設定_ドラム(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip)
        {
            if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
            {
                try
                {
                    pChip.bHit = true;
                    this.r現在の空うちドラムChip[(int)this.eChannelToPad[pChip.nChannelNumber - 0xb1]] = pChip;
                    pChip.nChannelNumber = ((pChip.nChannelNumber < 0xbc) || (pChip.nChannelNumber > 190)) ? ((pChip.nChannelNumber - 0xb1) + 0x11) : ((pChip.nChannelNumber - 0xb3) + 0x11);
                }
                catch
                {
                    return;
                }
            }

        }
		protected override void tUpdateAndDraw_Chip_BarLine( CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip )
		{
			int n小節番号plus1 = pChip.nPlaybackPosition / 384;
			if ( !pChip.bHit && ( pChip.nDistanceFromBar.Drums < 0 ) )
			{
				pChip.bHit = true;
				this.actPlayInfo.n小節番号 = n小節番号plus1 - 1;
                if ( configIni.bWave再生位置自動調整機能有効 && bIsDirectSound )
				{
					dTX.tAutoCorrectWavPlaybackPosition();
				}
			}
				if ( configIni.b演奏情報を表示する && ( configIni.nLaneDisp.Drums == 0 || configIni.nLaneDisp.Drums == 1 ) )
                {
                        int n小節番号 = n小節番号plus1 - 1;
                        CDTXMania.actDisplayString.tPrint(858, configIni.bReverse.Drums ? ((base.nJudgeLinePosY.Drums + pChip.nDistanceFromBar.Drums) - 0x11) : ((base.nJudgeLinePosY.Drums - pChip.nDistanceFromBar.Drums) - 0x11), CCharacterConsole.EFontType.White, n小節番号.ToString());
				}
                if (((configIni.nLaneDisp.Drums == 0 || configIni.nLaneDisp.Drums == 1) && pChip.bVisible) && (this.txチップ != null))
				{
                    int l_drumPanelWidth = 0x22f;
                    int l_xOffset = 0;
                    if (configIni.eNumOfLanes.Drums == EType.B)
                    {
                        l_drumPanelWidth = 0x207;
                    }
                    else if (CDTXMania.ConfigIni.eNumOfLanes.Drums == EType.C)
                    {
                        l_drumPanelWidth = 447;
                        l_xOffset = 72;
                    }

                this.txチップ.tDraw2D(CDTXMania.app.Device, 295 + l_xOffset, configIni.bReverse.Drums ? ((base.nJudgeLinePosY.Drums + pChip.nDistanceFromBar.Drums) - 1) : ((base.nJudgeLinePosY.Drums - pChip.nDistanceFromBar.Drums) - 1), new Rectangle(0, 769, l_drumPanelWidth, 2));
				}
              
            /*
			if ( ( pChip.bVisible && configIni.bGuitarEnabled ) && ( configIni.eDark != EDarkMode.FULL ) )
			{
				int y = configIni.bReverse.Guitar ? ( ( 0x176 - pChip.nDistanceFromBar.Guitar ) - 1 ) : ( ( 0x5f + pChip.nDistanceFromBar.Guitar ) - 1 );
				if ( ( dTX.bチップがある.Guitar && ( y > 0x39 ) ) && ( ( y < 0x19c ) && ( this.txチップ != null ) ) )
				{
					this.txチップ.tDraw2D( CDTXMania.app.Device, 374, y, new Rectangle( 0, 450, 0x4e, 1 ) );
				}
				y = configIni.bReverse.Bass ? ( ( 0x176 - pChip.nDistanceFromBar.Bass ) - 1 ) : ( ( 0x5f + pChip.nDistanceFromBar.Bass ) - 1 );
				if ( ( dTX.bチップがある.Bass && ( y > 0x39 ) ) && ( ( y < 0x19c ) && ( this.txチップ != null ) ) )
				{
					this.txチップ.tDraw2D( CDTXMania.app.Device, 398, y, new Rectangle( 0, 450, 0x4e, 1 ) );
				}
			}
             */
		}              //移植完了。
    #endregion
	}

}
