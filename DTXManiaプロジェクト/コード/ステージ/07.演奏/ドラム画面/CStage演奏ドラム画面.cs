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
	internal class CStage演奏ドラム画面 : CStage演奏画面共通
	{
		// コンストラクタ

		public CStage演奏ドラム画面()
		{
			base.eステージID = CStage.Eステージ.演奏;
			base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
			base.b活性化してない = true;
			base.list子Activities.Add( this.actPad = new CAct演奏Drumsパッド() );
			base.list子Activities.Add( this.actCombo = new CAct演奏DrumsコンボDGB() );
			base.list子Activities.Add( this.actDANGER = new CAct演奏DrumsDanger() );
			base.list子Activities.Add( this.actChipFireD = new CAct演奏DrumsチップファイアD() );
            base.list子Activities.Add( this.actGauge = new CAct演奏Drumsゲージ() );
            base.list子Activities.Add( this.actGraph = new CAct演奏スキルメーター() ); // #24074 2011.01.23 add ikanick
			base.list子Activities.Add( this.actJudgeString = new CAct演奏Drums判定文字列() );
			base.list子Activities.Add( this.actLaneFlushD = new CAct演奏DrumsレーンフラッシュD() );
			base.list子Activities.Add( this.actScore = new CAct演奏Drumsスコア() );
			base.list子Activities.Add( this.actStatusPanels = new CAct演奏Drumsステータスパネル() );
			base.list子Activities.Add( this.act譜面スクロール速度 = new CAct演奏スクロール速度() );
			base.list子Activities.Add( this.actAVI = new CAct演奏AVI() );
			base.list子Activities.Add( this.actBGA = new CAct演奏BGA() );
//			base.list子Activities.Add( this.actPanel = new CAct演奏パネル文字列() );
			base.list子Activities.Add( this.actStageFailed = new CAct演奏ステージ失敗() );
			base.list子Activities.Add( this.actPlayInfo = new CAct演奏演奏情報() );
			base.list子Activities.Add( this.actFI = new CActFIFOBlackStart() );
			base.list子Activities.Add( this.actFO = new CActFIFOBlack() );
			base.list子Activities.Add( this.actFOClear = new CActFIFOWhite() );
            base.list子Activities.Add( this.actFOStageClear = new CActFIFOWhiteClear());
            base.list子Activities.Add( this.actFillin = new CAct演奏Drumsフィルインエフェクト() );
            base.list子Activities.Add( this.actLVFont = new CActLVLNFont() );
//          base.list子Activities.Add( this.actChipFireGB = new CAct演奏DrumsチップファイアGB());
//			base.list子Activities.Add( this.actLaneFlushGB = new CAct演奏DrumsレーンフラッシュGB() );
//			base.list子Activities.Add( this.actRGB = new CAct演奏DrumsRGB() );
//			base.list子Activities.Add( this.actWailingBonus = new CAct演奏DrumsWailingBonus() );
//          base.list子Activities.Add( this.actStageCleared = new CAct演奏ステージクリア());
		}


		// メソッド

		public void t演奏結果を格納する( out CScoreIni.C演奏記録 Drums, out CScoreIni.C演奏記録 Guitar, out CScoreIni.C演奏記録 Bass, out CDTX.CChip[] r空打ちドラムチップ )
		{
			base.t演奏結果を格納する・ドラム( out Drums );
			base.t演奏結果を格納する・ギター( out Guitar );
			base.t演奏結果を格納する・ベース( out Bass );

			r空打ちドラムチップ = new CDTX.CChip[ 12 ];
			for ( int i = 0; i < 12; i++ )
			{
				r空打ちドラムチップ[ i ] = this.r空うちChip( E楽器パート.DRUMS, (Eパッド) i );
				if( r空打ちドラムチップ[ i ] == null )
				{
					r空打ちドラムチップ[ i ] = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮( CDTXMania.Timer.n現在時刻, this.nパッド0Atoチャンネル0A[ i ], this.nInputAdjustTimeMs.Drums );
				}
			}
		}


		// CStage 実装

		public override void On活性化()
		{
			this.bフィルイン中 = false;
			base.On活性化();
            Cスコア cスコア = CDTXMania.stage選曲.r確定されたスコア;
            this.ct登場用 = new CCounter(0, 12, 16, CDTXMania.Timer);

            this.actChipFireD.iPosY = (CDTXMania.ConfigIni.bReverse.Drums ? base.nJudgeLinePosY.Drums - 183 : base.nJudgeLinePosY.Drums - 186);
            base.actPlayInfo.jl = (CDTXMania.ConfigIni.bReverse.Drums ? base.nJudgeLinePosY.Drums - 159 : CStage演奏画面共通.nJudgeLineMaxPosY - base.nJudgeLinePosY.Drums);

			if( CDTXMania.bコンパクトモード )
			{
				var score = new Cスコア();
				CDTXMania.Songs管理.tScoreIniを読み込んで譜面情報を設定する( CDTXMania.strコンパクトモードファイル + ".score.ini", ref score );
				this.actGraph.dbグラフ値目標_渡 = score.譜面情報.最大スキル[ 0 ];
			}
			else
			{
				this.actGraph.dbグラフ値目標_渡 = CDTXMania.stage選曲.r確定されたスコア.譜面情報.最大スキル[ 0 ];	// #24074 2011.01.23 add ikanick
                this.actGraph.dbグラフ値自己ベスト = CDTXMania.stage選曲.r確定されたスコア.譜面情報.最大スキル[ 0 ];

                // #35411 2015.08.21 chnmr0 add
                // ゴースト利用可のなとき、0で初期化
                if (CDTXMania.ConfigIni.eTargetGhost.Drums != ETargetGhostData.NONE)
                {
                    if (CDTXMania.listTargetGhsotLag[(int)E楽器パート.DRUMS] != null)
                    {
                        this.actGraph.dbグラフ値目標_渡 = 0;
                    }
                }
            }
            dtLastQueueOperation = DateTime.MinValue;
		}
		public override void On非活性化()
		{
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
                this.bサビ区間 = false;
                this.bボーナス = false;
                this.txチップ = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_chips_drums.png"));
				this.txヒットバー = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlayDrums hit-bar.png" ) );
                this.txシャッター = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_shutter.png" ) );
                this.txLaneCover = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_lanes_Cover_cls.png"));

                /*
				this.txヒットバーGB = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlayDrums hit-bar guitar.png" ) );
				this.txレーンフレームGB = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenPlayDrums lane parts guitar.png" ) );
                if( this.txレーンフレームGB != null )
				{
					this.txレーンフレームGB.n透明度 = 0xff - CDTXMania.ConfigIni.n背景の透過度;
				}
                 */

				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				CDTXMania.tテクスチャの解放( ref this.txヒットバー );
				CDTXMania.tテクスチャの解放( ref this.txチップ );
                CDTXMania.tテクスチャの解放( ref this.txLaneCover );
                CDTXMania.tテクスチャの解放( ref this.txシャッター );
//				CDTXMania.tテクスチャの解放( ref this.txヒットバーGB );
//				CDTXMania.tテクスチャの解放( ref this.txレーンフレームGB );
                
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			base.sw.Start();
			if( !base.b活性化してない )
            {
                this.bIsFinishedPlaying = false;
                this.bIsFinishedFadeout = false;
                this.bエクセ = false;
                this.bフルコン = false;
                if (base.b初めての進行描画)
                {
                    CSound管理.rc演奏用タイマ.tリセット();
                    CDTXMania.Timer.tリセット();
                    this.actChipFireD.Start(Eレーン.HH, false, false, false, 0, false); // #31554 2013.6.12 yyagi

                    this.ctチップ模様アニメ.Drums = new CCounter(0, 7, 70, CDTXMania.Timer);
                    double UnitTime;
                    UnitTime = ((60.0 / (CDTXMania.stage演奏ドラム画面.actPlayInfo.dbBPM) / 14.0));
                    this.ctBPMバー = new CCounter(1.0, 14.0, UnitTime, CSound管理.rc演奏用タイマ);

                    this.ctコンボ動作タイマ = new CCounter( 1.0, 16.0, ((60.0 / (CDTXMania.stage演奏ドラム画面.actPlayInfo.dbBPM) / 16)), CSound管理.rc演奏用タイマ);

                    this.ctチップ模様アニメ.Guitar = new CCounter(0, 0x17, 20, CDTXMania.Timer);
                    this.ctチップ模様アニメ.Bass = new CCounter(0, 0x17, 20, CDTXMania.Timer);
                    this.ctWailingチップ模様アニメ = new CCounter(0, 4, 50, CDTXMania.Timer);
                    base.eフェーズID = CStage.Eフェーズ.共通_フェードイン;
                    
                    if( this.tx判定画像anime != null && this.txボーナスエフェクト != null )
                    {
                        this.tx判定画像anime.t2D描画( CDTXMania.app.Device, 1280, 720 );
                        this.txボーナスエフェクト.t2D描画( CDTXMania.app.Device, 1280, 720 );
                    }
                    this.actFI.tフェードイン開始();
                    this.ct登場用.t進行();
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

                        #region [ BGMの途中再生開始 (CDTXのt入力・行解析・チップ配置()で小節番号が+1されているのを削っておくこと) ]
						foreach ( CDTX.CChip pChip in this.listChip )
						{
							if ( pChip.nチャンネル番号 == 0x01 )
							{
								CDTX.CWAV wc = CDTXMania.DTX.listWAV[ pChip.n整数値・内部番号 ];
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

                    base.b初めての進行描画 = false;
                }

                if ((CDTXMania.ConfigIni.bSTAGEFAILED有効 && this.actGauge.IsFailed(E楽器パート.DRUMS)) && (base.eフェーズID == CStage.Eフェーズ.共通_通常状態))
                {
                    this.actStageFailed.Start();
                    CDTXMania.DTX.t全チップの再生停止();
                    base.eフェーズID = CStage.Eフェーズ.演奏_STAGE_FAILED;
                }
                this.t進行描画・背景();
                this.t進行描画・MIDIBGM();
                this.t進行描画・AVI();
                this.t進行描画・レーンフラッシュD();
                this.t進行描画・譜面スクロール速度();
                this.t進行描画・チップアニメ();
                this.t進行描画・小節線( E楽器パート.DRUMS );
                this.t進行描画・チップ・模様のみ( E楽器パート.DRUMS );
                bIsFinishedPlaying = this.t進行描画・チップ( E楽器パート.DRUMS );
                #region[ シャッター ]
                //シャッターを使うのはLC、LP、FT、RDレーンのみ。その他のレーンでは一切使用しない。
                if ((CDTXMania.ConfigIni.bCLASSIC譜面判別を有効にする == true ) && ((CDTXMania.DTX.bチップがある.LeftCymbal == false) && ( CDTXMania.DTX.bチップがある.FT == false ) && ( CDTXMania.DTX.bチップがある.Ride == false ) && ( CDTXMania.DTX.bチップがある.LP == false ) && ( CDTXMania.DTX.b強制的にXG譜面にする == false)))
                {
                    if ( this.txLaneCover != null )
                    {
                        //旧画像
                        //this.txLaneCover.t2D描画(CDTXMania.app.Device, 295, 0);
                        //if (CDTXMania.DTX.bチップがある.LeftCymbal == false)
                        {
                            this.txLaneCover.t2D描画(CDTXMania.app.Device, 295, 0, new Rectangle(0, 0, 70, 720));
                        }
                        //if ((CDTXMania.DTX.bチップがある.LP == false) && (CDTXMania.DTX.bチップがある.LBD == false))
                        {
                            //レーンタイプでの入れ替わりあり
                            if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.A || CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.C)
                            {
                                this.txLaneCover.t2D描画(CDTXMania.app.Device, 416, 0, new Rectangle(124, 0, 54, 720));
                            }
                            else if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.B)
                            {
                                this.txLaneCover.t2D描画(CDTXMania.app.Device, 470, 0, new Rectangle(124, 0, 54, 720));
                            }
                            else if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.D)
                            {
                                this.txLaneCover.t2D描画(CDTXMania.app.Device, 522, 0, new Rectangle(124, 0, 54, 720));
                            }
                        }
                        //if (CDTXMania.DTX.bチップがある.FT == false)
                        {
                            this.txLaneCover.t2D描画(CDTXMania.app.Device, 690, 0, new Rectangle(71, 0, 52, 720));
                        }
                        //if (CDTXMania.DTX.bチップがある.Ride == false)
                        {
                            //RDPositionで入れ替わり
                            if (CDTXMania.ConfigIni.eRDPosition == ERDPosition.RCRD)
                            {
                                this.txLaneCover.t2D描画(CDTXMania.app.Device, 815, 0, new Rectangle(178, 0, 38, 720));
                            }
                            else if (CDTXMania.ConfigIni.eRDPosition == ERDPosition.RDRC)
                            {
                                this.txLaneCover.t2D描画(CDTXMania.app.Device, 743, 0, new Rectangle(178, 0, 38, 720));
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
                    this.txシャッター.t2D描画(CDTXMania.app.Device, 295, (int)(-720 + dbシャッターIN));

                    if (CDTXMania.ConfigIni.b演奏情報を表示する)
                        this.actLVFont.t文字列描画(564, (int)dbシャッターIN - 20, CDTXMania.ConfigIni.nShutterOutSide.Drums.ToString());

                    dbシャッターOUT = 720 - (base.nShutterInPosY.Drums * db倍率);
                    this.txシャッター.t2D描画(CDTXMania.app.Device, 295, (int)dbシャッターOUT);

                    if (CDTXMania.ConfigIni.b演奏情報を表示する)
                        this.actLVFont.t文字列描画(564, (int)dbシャッターOUT + 2, CDTXMania.ConfigIni.nShutterInSide.Drums.ToString());
                }
                else
                {
                    this.txシャッター.t2D描画(CDTXMania.app.Device, 295, (int)(-720 + dbシャッターIN));

                    if (CDTXMania.ConfigIni.b演奏情報を表示する)
                        this.actLVFont.t文字列描画(564, (int)dbシャッターIN - 20, CDTXMania.ConfigIni.nShutterInSide.Drums.ToString());

                    this.txシャッター.t2D描画(CDTXMania.app.Device, 295, (int)dbシャッターOUT);

                    if (CDTXMania.ConfigIni.b演奏情報を表示する)
                        this.actLVFont.t文字列描画(564, (int)dbシャッターOUT + 2, CDTXMania.ConfigIni.nShutterOutSide.Drums.ToString());
                }

                #endregion
                this.t進行描画・判定ライン();
                this.t進行描画・ドラムパッド();
                bIsFinishedFadeout = this.t進行描画・フェードイン・アウト();
                if (bIsFinishedPlaying && (base.eフェーズID == CStage.Eフェーズ.共通_通常状態) )
                {
                    if ((this.actGauge.IsFailed(E楽器パート.DRUMS)) && (base.eフェーズID == CStage.Eフェーズ.共通_通常状態))
                    {
                        this.actStageFailed.Start();
                        CDTXMania.DTX.t全チップの再生停止();
                        base.eフェーズID = CStage.Eフェーズ.演奏_STAGE_FAILED;
                    }
                    else
                    {
                        this.eフェードアウト完了時の戻り値 = E演奏画面の戻り値.ステージクリア;
                        base.eフェーズID = CStage.Eフェーズ.演奏_STAGE_CLEAR_フェードアウト;
                        if (base.nヒット数・Auto含まない.Drums.Miss + base.nヒット数・Auto含まない.Drums.Poor == 0)
                        {
                            this.nパフェ数 = CDTXMania.ConfigIni.bドラムが全部オートプレイである ? this.nパフェ数 = base.nヒット数・Auto含む.Drums.Perfect : base.nヒット数・Auto含まない.Drums.Perfect;
                            if (nパフェ数 == CDTXMania.DTX.n可視チップ数.Drums)
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
                        this.actFOStageClear.tフェードアウト開始();
                    }
                }
                if( CDTXMania.ConfigIni.bShowScore )
                    this.t進行描画・スコア();
//              if( CDTXMania.ConfigIni.bShowMusicInfo )
//                  this.t進行描画・パネル文字列();
                if (CDTXMania.ConfigIni.nInfoType == 1)
                    this.t進行描画・ステータスパネル();
                this.t進行描画・ゲージ();
                this.t進行描画・コンボ();
                this.t進行描画・グラフ();
                this.t進行描画・演奏情報();
                this.t進行描画・判定文字列1・通常位置指定の場合();
                this.t進行描画・判定文字列2・判定ライン上指定の場合();
                this.t進行描画・チップファイアD();
                this.t進行描画・STAGEFAILED();
                bすべてのチップが判定された = true;
                if (bIsFinishedFadeout)
                {
                    if (!CDTXMania.Skin.soundステージクリア音.b再生中 && !CDTXMania.Skin.soundSTAGEFAILED音.b再生中)
                    {
                        Debug.WriteLine("Total On進行描画=" + sw.ElapsedMilliseconds + "ms");
                        this.nミス数 = base.nヒット数・Auto含まない.Drums.Miss + base.nヒット数・Auto含まない.Drums.Poor;
                        switch (nミス数)
                        {
                            case 0:
                                {
                                    this.nパフェ数 = base.nヒット数・Auto含まない.Drums.Perfect;
                                    if (CDTXMania.ConfigIni.bドラムが全部オートプレイである)
                                    {
                                        this.nパフェ数 = base.nヒット数・Auto含む.Drums.Perfect;
                                    }
                                    if (nパフェ数 == CDTXMania.DTX.n可視チップ数.Drums)
                                    #region[ エクセ ]
                                    {
                                        this.bエクセ = true;
                                        if (CDTXMania.ConfigIni.nSkillMode == 1)
                                            this.actScore.n現在の本当のスコア.Drums += 30000;
                                        break;
                                    }
                                    #endregion
                                    else
                                    #region[ フルコン ]
                                    {
                                        this.bフルコン = true;
                                        if (CDTXMania.ConfigIni.nSkillMode == 1)
                                            this.actScore.n現在の本当のスコア.Drums += 15000;
                                        break;
                                    }
                                    #endregion
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        return (int)this.eフェードアウト完了時の戻り値;
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
                                CDTXMania.Sound管理.AddMixer(stm.csound);
                            }
                            else
                            {
                                CDTXMania.Sound管理.RemoveMixer(stm.csound);
                            }
                        }
                    }
                }
                // キー入力

                if (CDTXMania.act現在入力を占有中のプラグイン == null)
                    this.tキー入力();
            }
			base.sw.Stop();
			return 0;
		}




		// その他

		#region [ private ]
		//-----------------
        public bool bIsFinishedFadeout;
        public bool bIsFinishedPlaying;
        public bool bエクセ;
        public bool bフルコン;
        public bool bすべてのチップが判定された;
        public int nミス数;
        public int nパフェ数;
		private CAct演奏DrumsチップファイアD actChipFireD;
		public CAct演奏Drumsパッド actPad;
		public bool bフィルイン中;
        public bool bフィルイン終了;
        public bool bサビ区間;
        public bool bボーナス;
		private readonly Eパッド[] eチャンネルtoパッド = new Eパッド[12]
		{
			Eパッド.HH, Eパッド.SD, Eパッド.BD, Eパッド.HT,
			Eパッド.LT, Eパッド.CY, Eパッド.FT, Eパッド.HHO,
			Eパッド.RD, Eパッド.UNKNOWN, Eパッド.UNKNOWN, Eパッド.LC
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

        private void tフェードアウト()
        {
            this.eフェードアウト完了時の戻り値 = E演奏画面の戻り値.ステージクリア;
            base.eフェーズID = CStage.Eフェーズ.演奏_STAGE_CLEAR_フェードアウト;

            this.actFOStageClear.tフェードアウト開始();
        }

		private bool bフィルイン区間の最後のChipである( CDTX.CChip pChip )
		{
			if( pChip == null )
			{
				return false;
			}
			int num = pChip.n発声位置;
            for (int i = listChip.IndexOf(pChip) + 1; i < listChip.Count; i++)
			{
                pChip = listChip[i];
				if( ( pChip.nチャンネル番号 == 0x53 ) && ( pChip.n整数値 == 2 ) )
				{
					return true;
				}
				if( ( ( pChip.nチャンネル番号 >= 0x11 ) && ( pChip.nチャンネル番号 <= 0x1c ) ) && ( ( pChip.n発声位置 - num ) > 0x18 ) )
				{
					return false;
				}
			}
			return true;
		}

		protected override E判定 tチップのヒット処理( long nHitTime, CDTX.CChip pChip, bool bCorrectLane )
		{
			E判定 eJudgeResult = tチップのヒット処理( nHitTime, pChip, E楽器パート.DRUMS, bCorrectLane );
			// #24074 2011.01.23 add ikanick
            if (CDTXMania.ConfigIni.nSkillMode == 0)
            {
                this.actGraph.dbグラフ値現在_渡 = CScoreIni.t旧演奏型スキルを計算して返す(CDTXMania.DTX.n可視チップ数.Drums, this.nヒット数・Auto含まない.Drums.Perfect, this.nヒット数・Auto含まない.Drums.Great, this.nヒット数・Auto含まない.Drums.Good, this.nヒット数・Auto含まない.Drums.Poor, this.nヒット数・Auto含まない.Drums.Miss, E楽器パート.DRUMS, bIsAutoPlay);
            }
            else if (CDTXMania.ConfigIni.nSkillMode == 1)
            {
                this.actGraph.dbグラフ値現在_渡 = CScoreIni.t演奏型スキルを計算して返す(CDTXMania.DTX.n可視チップ数.Drums, this.nヒット数・Auto含まない.Drums.Perfect, this.nヒット数・Auto含まない.Drums.Great, this.nヒット数・Auto含まない.Drums.Good, this.nヒット数・Auto含まない.Drums.Poor, this.nヒット数・Auto含まない.Drums.Miss, this.actCombo.n現在のコンボ数.最高値.Drums, E楽器パート.DRUMS, bIsAutoPlay);
            }
			// #35411 2015.09.07 add chnmr0
			if( CDTXMania.listTargetGhsotLag.Drums != null &&
                CDTXMania.ConfigIni.eTargetGhost.Drums == ETargetGhostData.ONLINE &&
				CDTXMania.DTX.n可視チップ数.Drums > 0 )
			{
				// Online Stats の計算式
				this.actGraph.dbグラフ値現在_渡 = 100 *
								( this.nヒット数・Auto含まない.Drums.Perfect * 17 +
								 this.nヒット数・Auto含まない.Drums.Great * 7 +
								 this.actCombo.n現在のコンボ数.最高値.Drums * 3 ) / ( 20.0 * CDTXMania.DTX.n可視チップ数.Drums );
			}

            this.actStatusPanels.db現在の達成率.Drums = this.actGraph.dbグラフ値現在_渡;
			return eJudgeResult;
		}

		protected override void tチップのヒット処理・BadならびにTight時のMiss( E楽器パート part )
		{
			this.tチップのヒット処理・BadならびにTight時のMiss( part, 0, E楽器パート.DRUMS );
		}
		protected override void tチップのヒット処理・BadならびにTight時のMiss( E楽器パート part, int nLane )
		{
			this.tチップのヒット処理・BadならびにTight時のMiss( part, nLane, E楽器パート.DRUMS );
		}

        protected override void tJudgeLineMovingUpandDown()
        {
            this.actJudgeString.iP_A = (CDTXMania.ConfigIni.bReverse.Drums ? base.nJudgeLinePosY.Drums : base.nJudgeLinePosY.Drums - 189);
            this.actJudgeString.iP_B = (CDTXMania.ConfigIni.bReverse.Drums ? base.nJudgeLinePosY.Drums : base.nJudgeLinePosY.Drums + 23);
            this.actChipFireD.iPosY = (CDTXMania.ConfigIni.bReverse.Drums ? base.nJudgeLinePosY.Drums - 183 : base.nJudgeLinePosY.Drums - 186);
            CDTXMania.stage演奏ドラム画面.actPlayInfo.jl = (CDTXMania.ConfigIni.bReverse.Drums ? 0 : CStage演奏画面共通.nJudgeLineMaxPosY - base.nJudgeLinePosY.Drums);
        }

		private bool tドラムヒット処理( long nHitTime, Eパッド type, CDTX.CChip pChip, int n強弱度合い0to127 )
		{
			if( pChip == null )
			{
				return false;
			}
			int index = pChip.nチャンネル番号;
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
			E判定 e判定 = this.e指定時刻からChipのJUDGEを返す( nHitTime, pChip, nInputAdjustTime );
			if( e判定 == E判定.Miss )
			{
				return false;
			}
			this.tチップのヒット処理( nHitTime, pChip );
			this.actLaneFlushD.Start( (Eレーン) nLane, ( (float) n強弱度合い0to127 ) / 127f );
			this.actPad.Hit( nPad );
			if( ( e判定 != E判定.Poor ) && ( e判定 != E判定.Miss ) )
			{
				bool flag = this.bフィルイン中;
				bool flag2 = this.bフィルイン中 && this.bフィルイン区間の最後のChipである( pChip );
                this.actChipFireD.Start( (Eレーン)nLane, flag, flag2, flag2, nJudgeLinePosY_delta.Drums );
                // #31602 2013.6.24 yyagi 判定ラインの表示位置をずらしたら、チップのヒットエフェクトの表示もずらすために、nJudgeLine..を追加
			}
			if( CDTXMania.ConfigIni.bドラム打音を発声する )
			{
				CDTX.CChip rChip = null;
				bool bIsChipsoundPriorToPad = true;
				if( ( ( type == Eパッド.HH ) || ( type == Eパッド.HHO ) ) || ( type == Eパッド.LC ) )
				{
					bIsChipsoundPriorToPad = CDTXMania.ConfigIni.eHitSoundPriorityHH == E打ち分け時の再生の優先順位.ChipがPadより優先;
				}
				else if( ( type == Eパッド.LT ) || ( type == Eパッド.FT ) )
				{
					bIsChipsoundPriorToPad = CDTXMania.ConfigIni.eHitSoundPriorityFT == E打ち分け時の再生の優先順位.ChipがPadより優先;
				}
				else if( ( type == Eパッド.CY ) || ( type == Eパッド.RD ) )
				{
					bIsChipsoundPriorToPad = CDTXMania.ConfigIni.eHitSoundPriorityCY == E打ち分け時の再生の優先順位.ChipがPadより優先;
				}
                else if (((type == Eパッド.LP) || (type == Eパッド.LBD)) || (type == Eパッド.BD))
                {
                    bIsChipsoundPriorToPad = CDTXMania.ConfigIni.eHitSoundPriorityLP == E打ち分け時の再生の優先順位.ChipがPadより優先;
                }

				if( bIsChipsoundPriorToPad )
				{
					rChip = pChip;
				}
				else
				{
					Eパッド hH = type;
					if( !CDTXMania.DTX.bチップがある.HHOpen && ( type == Eパッド.HHO ) )
					{
						hH = Eパッド.HH;
					}
					if( !CDTXMania.DTX.bチップがある.Ride && ( type == Eパッド.RD ) )
					{
						hH = Eパッド.CY;
					}
					if( !CDTXMania.DTX.bチップがある.LeftCymbal && ( type == Eパッド.LC ) )
					{
						hH = Eパッド.HH;
					}
					rChip = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮( nHitTime, this.nパッド0Atoチャンネル0A[ (int) hH ], nInputAdjustTime );
					if( rChip == null )
					{
						rChip = pChip;
					}
				}
				this.tサウンド再生( rChip, CSound管理.rc演奏用タイマ.nシステム時刻, E楽器パート.DRUMS, CDTXMania.ConfigIni.n手動再生音量, CDTXMania.ConfigIni.b演奏音を強調する.Drums );
			}
			return true;
		}

		protected override void ドラムスクロール速度アップ()
		{
			CDTXMania.ConfigIni.n譜面スクロール速度.Drums = Math.Min( CDTXMania.ConfigIni.n譜面スクロール速度.Drums + 1, 1999 );
		}
		protected override void ドラムスクロール速度ダウン()
		{
			CDTXMania.ConfigIni.n譜面スクロール速度.Drums = Math.Max( CDTXMania.ConfigIni.n譜面スクロール速度.Drums - 1, 0 );
		}
	
        /*
		protected override void t進行描画・AVI()
		{
			base.t進行描画・AVI( 0, 0 );
		}
		protected override void t進行描画・BGA()
		{
			base.t進行描画・BGA( 990, 0 );
		}
         */
		protected override void t進行描画・DANGER()
		{
			this.actDANGER.t進行描画( this.actGauge.IsDanger(E楽器パート.DRUMS), false, false );
		}

		protected override void t進行描画・Wailing枠()
		{
			base.t進行描画・Wailing枠( 587, 478,
				CDTXMania.ConfigIni.bReverse.Guitar ? ( 400 - this.txWailing枠.sz画像サイズ.Height ) : 69,
				CDTXMania.ConfigIni.bReverse.Bass ? ( 400 - this.txWailing枠.sz画像サイズ.Height ) : 69
			);
		}

        /*
		private void t進行描画・ギターベースフレーム()
		{
			if( ( ( CDTXMania.ConfigIni.eDark != Eダークモード.HALF ) && ( CDTXMania.ConfigIni.eDark != Eダークモード.FULL ) ) && CDTXMania.ConfigIni.bGuitar有効 )
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
							this.txレーンフレームGB.t2D描画( CDTXMania.app.Device, 0x1fb, 0x39 + i, rectangle );
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
							this.txレーンフレームGB.t2D描画( CDTXMania.app.Device, 0x18e, 0x39 + j, rectangle2 );
						}
					}
				}
			}
		}
		private void t進行描画・ギターベース判定ライン()		// yyagi: ギタレボモードとは座標が違うだけですが、まとめづらかったのでそのまま放置してます。
		{
			if ( ( CDTXMania.ConfigIni.eDark != Eダークモード.FULL ) && CDTXMania.ConfigIni.bGuitar有効 )
			{
				if ( CDTXMania.DTX.bチップがある.Guitar )
				{
                    int y = ( CDTXMania.ConfigIni.bReverse.Guitar ? 374 + nJudgeLinePosY_delta.Guitar : 95 - nJudgeLinePosY_delta.Guitar ) - 3;
                    	// #31602 2013.6.23 yyagi 描画遅延対策として、判定ラインの表示位置をオフセット調整できるようにする
                    if ( this.txヒットバーGB != null )
					{
    					for ( int i = 0; i < 3; i++ )						
						{
							this.txヒットバーGB.t2D描画( CDTXMania.app.Device, 509 + ( 26 * i ), y );
							this.txヒットバーGB.t2D描画( CDTXMania.app.Device, ( 509 + ( 26 * i ) ) + 16, y, new Rectangle( 0, 0, 10, 16 ) );
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
							this.txヒットバーGB.t2D描画( CDTXMania.app.Device, 400 + ( 26 * j ), y );
							this.txヒットバーGB.t2D描画( CDTXMania.app.Device, ( 400 + ( 26 * j ) ) + 16, y, new Rectangle( 0, 0, 10, 16 ) );
						}
					}
				}
			}
		}
         */

        private void t進行描画・グラフ()
        {
            if( CDTXMania.ConfigIni.bGraph有効.Drums )
            {
                this.actGraph.On進行描画();
            }
        }

		private void t進行描画・チップファイアD()
        {
			this.actChipFireD.On進行描画();
        }
        private void t進行描画・ドラムパッド()
        {
            this.actPad.On進行描画();
        }

        /*
        protected override void t進行描画・パネル文字列()
        {
            base.t進行描画・パネル文字列(912, 640);
        }
         */

		protected override void t進行描画・演奏情報()
		{
			base.t進行描画・演奏情報( 1000, 257 );
		}

		protected override void t入力処理・ドラム()
        {

            for (int nPad = 0; nPad < (int)Eパッド.MAX; nPad++)
            {
                List<STInputEvent> listInputEvent = CDTXMania.Pad.GetEvents(E楽器パート.DRUMS, (Eパッド)nPad);

                if ((listInputEvent == null) || (listInputEvent.Count == 0))
                    continue;

                this.t入力メソッド記憶(E楽器パート.DRUMS);

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

                    long nTime = inputEvent.nTimeStamp - CSound管理.rc演奏用タイマ.n前回リセットした時のシステム時刻;
                    int nInputAdjustTime = this.bIsAutoPlay[base.nチャンネル0Atoレーン07[nPad]] ? 0 : this.nInputAdjustTimeMs.Drums;
                    int nPedalLagTime = CDTXMania.ConfigIni.nPedalLagTime;

                    bool bHitted = false;

                    #region [ (A) ヒットしていればヒット処理して次の inputEvent へ ]
                    //-----------------------------
                    switch (((Eパッド)nPad))
                    {
                        case Eパッド.HH:
                            #region [ HHとLC(groupingしている場合) のヒット処理 ]
                            //-----------------------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.HH)
                                    continue;	// 電子ドラムによる意図的なクロストークを無効にする

                                CDTX.CChip chipHC = this.r指定時刻に一番近い未ヒットChip(nTime, 0x11, nInputAdjustTime);	// HiHat Close
                                CDTX.CChip chipHO = this.r指定時刻に一番近い未ヒットChip(nTime, 0x18, nInputAdjustTime);	// HiHat Open
                                CDTX.CChip chipLC = this.r指定時刻に一番近い未ヒットChip(nTime, 0x1a, nInputAdjustTime);	// LC
                                E判定 e判定HC = (chipHC != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipHC, nInputAdjustTime) : E判定.Miss;
                                E判定 e判定HO = (chipHO != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipHO, nInputAdjustTime) : E判定.Miss;
                                E判定 e判定LC = (chipLC != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLC, nInputAdjustTime) : E判定.Miss;
                                switch (eHHGroup)
                                {
                                    case EHHGroup.ハイハットのみ打ち分ける:
                                        #region [ HCとLCのヒット処理 ]
                                        //-----------------------------
                                        if ((e判定HC != E判定.Miss) && (e判定LC != E判定.Miss))
                                        {
                                            if (chipHC.n発声位置 < chipLC.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipHC, inputEvent.nVelocity);
                                            }
                                            else if (chipHC.n発声位置 > chipLC.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipLC, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipHC, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipLC, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定HC != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.HH, chipHC, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定LC != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.HH, chipLC, inputEvent.nVelocity);
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
                                        if ((e判定HC != E判定.Miss) && (e判定HO != E判定.Miss))
                                        {
                                            if (chipHC.n発声位置 < chipHO.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipHC, inputEvent.nVelocity);
                                            }
                                            else if (chipHC.n発声位置 > chipHO.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipHO, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipHC, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipHO, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定HC != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.HH, chipHC, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定HO != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.HH, chipHO, inputEvent.nVelocity);
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
                                        if (((e判定HC != E判定.Miss) && (e判定HO != E判定.Miss)) && (e判定LC != E判定.Miss))
                                        {
                                            CDTX.CChip chip;
                                            CDTX.CChip[] chipArray = new CDTX.CChip[] { chipHC, chipHO, chipLC };
                                            // ここから、chipArrayをn発生位置の小さい順に並び替える
                                            if (chipArray[1].n発声位置 > chipArray[2].n発声位置)
                                            {
                                                chip = chipArray[1];
                                                chipArray[1] = chipArray[2];
                                                chipArray[2] = chip;
                                            }
                                            if (chipArray[0].n発声位置 > chipArray[1].n発声位置)
                                            {
                                                chip = chipArray[0];
                                                chipArray[0] = chipArray[1];
                                                chipArray[1] = chip;
                                            }
                                            if (chipArray[1].n発声位置 > chipArray[2].n発声位置)
                                            {
                                                chip = chipArray[1];
                                                chipArray[1] = chipArray[2];
                                                chipArray[2] = chip;
                                            }
                                            this.tドラムヒット処理(nTime, Eパッド.HH, chipArray[0], inputEvent.nVelocity);
                                            if (chipArray[0].n発声位置 == chipArray[1].n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipArray[1], inputEvent.nVelocity);
                                            }
                                            if (chipArray[0].n発声位置 == chipArray[2].n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipArray[2], inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定HC != E判定.Miss) && (e判定HO != E判定.Miss))
                                        {
                                            if (chipHC.n発声位置 < chipHO.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipHC, inputEvent.nVelocity);
                                            }
                                            else if (chipHC.n発声位置 > chipHO.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipHO, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipHC, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipHO, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定HC != E判定.Miss) && (e判定LC != E判定.Miss))
                                        {
                                            if (chipHC.n発声位置 < chipLC.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipHC, inputEvent.nVelocity);
                                            }
                                            else if (chipHC.n発声位置 > chipLC.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipLC, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipHC, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipLC, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定HO != E判定.Miss) && (e判定LC != E判定.Miss))
                                        {
                                            if (chipHO.n発声位置 < chipLC.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipHO, inputEvent.nVelocity);
                                            }
                                            else if (chipHO.n発声位置 > chipLC.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipLC, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipHO, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.HH, chipLC, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定HC != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.HH, chipHC, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定HO != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.HH, chipHO, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定LC != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.HH, chipLC, inputEvent.nVelocity);
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
                                        if (e判定HC != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.HH, chipHC, inputEvent.nVelocity);
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

                        case Eパッド.SD:
                            #region [ SDのヒット処理 ]
                            //-----------------------------
                            if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.SD)	// #23857 2010.12.12 yyagi: to support VelocityMin
                                continue;	// 電子ドラムによる意図的なクロストークを無効にする
                            if (!this.tドラムヒット処理(nTime, Eパッド.SD, this.r指定時刻に一番近い未ヒットChip(nTime, 0x12, nInputAdjustTime), inputEvent.nVelocity))
                                break;
                            continue;
                        //-----------------------------
                            #endregion

                        case Eパッド.BD:
                            #region [ BDとLPとLBD(ペアリングしている場合)のヒット処理 ]
                            //-----------------------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.BD)	// #23857 2010.12.12 yyagi: to support VelocityMin
                                    continue;	// 電子ドラムによる意図的なクロストークを無効にする

                                CDTX.CChip chipBD  = this.r指定時刻に一番近い未ヒットChip(nTime, 0x13, nInputAdjustTime + nPedalLagTime);	// BD
                                CDTX.CChip chipLP  = this.r指定時刻に一番近い未ヒットChip(nTime, 0x1b, nInputAdjustTime + nPedalLagTime);	// LP
                                CDTX.CChip chipLBD = this.r指定時刻に一番近い未ヒットChip(nTime, 0x1c, nInputAdjustTime + nPedalLagTime);	// LBD
                                E判定 e判定BD  = (chipBD  != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipBD, nInputAdjustTime + nPedalLagTime) : E判定.Miss;
                                E判定 e判定LP  = (chipLP  != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLP, nInputAdjustTime + nPedalLagTime) : E判定.Miss;
                                E判定 e判定LBD = (chipLBD != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLBD, nInputAdjustTime + nPedalLagTime) : E判定.Miss;
                                switch (eBDGroup)
                                {
                                    case EBDGroup.BDとLPで打ち分ける:
                                        #region[ BD & LBD | LP ]
                                        if( e判定BD != E判定.Miss && e判定LBD != E判定.Miss )
                                        {
                                            if( chipBD.n発声位置 < chipLBD.n発声位置 )
                                            {
                                                this.tドラムヒット処理( nTime, Eパッド.BD, chipBD, inputEvent.nVelocity );
                                            }
                                            else if( chipBD.n発声位置 > chipLBD.n発声位置 )
                                            {
                                                this.tドラムヒット処理( nTime, Eパッド.BD, chipLBD, inputEvent.nVelocity );
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理( nTime, Eパッド.BD, chipBD, inputEvent.nVelocity);
                                                this.tドラムヒット処理( nTime, Eパッド.BD, chipLBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if( e判定BD != E判定.Miss)
                                        {
                                            this.tドラムヒット処理( nTime, Eパッド.BD, chipBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if( e判定LBD != E判定.Miss)
                                        {
                                            this.tドラムヒット処理( nTime, Eパッド.BD, chipLBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if( bHitted )
                                            continue;
                                        else
                                            break;
                                        #endregion

                                    case EBDGroup.左右ペダルのみ打ち分ける:
                                        #region[ BDのヒット処理]
                                        if (e判定BD != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.BD, chipBD, inputEvent.nVelocity);
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
                                        if (((e判定LP != E判定.Miss) && (e判定LBD != E判定.Miss)) && (e判定BD != E判定.Miss))
                                        {
                                            CDTX.CChip chip8;
                                            CDTX.CChip[] chipArray2 = new CDTX.CChip[] { chipLP, chipLBD, chipBD };
                                            if (chipArray2[1].n発声位置 > chipArray2[2].n発声位置)
                                            {
                                                chip8 = chipArray2[1];
                                                chipArray2[1] = chipArray2[2];
                                                chipArray2[2] = chip8;
                                            }
                                            if (chipArray2[0].n発声位置 > chipArray2[1].n発声位置)
                                            {
                                                chip8 = chipArray2[0];
                                                chipArray2[0] = chipArray2[1];
                                                chipArray2[1] = chip8;
                                            }
                                            if (chipArray2[1].n発声位置 > chipArray2[2].n発声位置)
                                            {
                                                chip8 = chipArray2[1];
                                                chipArray2[1] = chipArray2[2];
                                                chipArray2[2] = chip8;
                                            }
                                            this.tドラムヒット処理(nTime, Eパッド.BD, chipArray2[0], inputEvent.nVelocity);
                                            if (chipArray2[0].n発声位置 == chipArray2[1].n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.BD, chipArray2[1], inputEvent.nVelocity);
                                            }
                                            if (chipArray2[0].n発声位置 == chipArray2[2].n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.BD, chipArray2[2], inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        //chip7 BD  chip6LBD  chip5LP
                                        //判定6 BD  判定5　　 判定4
                                        else if ((e判定LP != E判定.Miss) && (e判定LBD != E判定.Miss))
                                        {
                                            if (chipLP.n発声位置 < chipLBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.BD, chipLP, inputEvent.nVelocity);
                                            }
                                            else if (chipLP.n発声位置 > chipLBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.BD, chipLBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.BD, chipLP, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.BD, chipLBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定LP != E判定.Miss) && (e判定BD != E判定.Miss))
                                        {
                                            if (chipLP.n発声位置 < chipBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.BD, chipLP, inputEvent.nVelocity);
                                            }
                                            else if (chipLP.n発声位置 > chipBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.BD, chipBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.BD, chipLP, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.BD, chipBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        //chip7 BD  chip6LBD  chip5LP
                                        //判定6 BD  判定5　　 判定4
                                        else if ((e判定LBD != E判定.Miss) && (e判定BD != E判定.Miss))
                                        {
                                            if (chipLBD.n発声位置 < chipBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.BD, chipLBD, inputEvent.nVelocity);
                                            }
                                            else if (chipLBD.n発声位置 > chipBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.BD, chipBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.BD, chipLBD, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.BD, chipBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定LP != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.BD, chipLP, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定LBD != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.BD, chipLBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定BD != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.BD, chipBD, inputEvent.nVelocity);
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
                                        if (e判定BD != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.BD, chipBD, inputEvent.nVelocity);
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

                        case Eパッド.HT:
                            #region [ HTのヒット処理 ]
                            //-----------------------------
                            if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.HT)	// #23857 2010.12.12 yyagi: to support VelocityMin
                                continue;	// 電子ドラムによる意図的なクロストークを無効にする
                            if (this.tドラムヒット処理(nTime, Eパッド.HT, this.r指定時刻に一番近い未ヒットChip(nTime, 20, nInputAdjustTime), inputEvent.nVelocity))
                                continue;
                            break;
                        //-----------------------------
                            #endregion

                        case Eパッド.LT:
                            #region [ LTとFT(groupingしている場合)のヒット処理 ]
                            //-----------------------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.LT)	// #23857 2010.12.12 yyagi: to support VelocityMin
                                    continue;	// 電子ドラムによる意図的なクロストークを無効にする
                                CDTX.CChip chipLT = this.r指定時刻に一番近い未ヒットChip(nTime, 0x15, nInputAdjustTime);	// LT
                                CDTX.CChip chipFT = this.r指定時刻に一番近い未ヒットChip(nTime, 0x17, nInputAdjustTime);	// FT
                                E判定 e判定LT = (chipLT != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLT, nInputAdjustTime) : E判定.Miss;
                                E判定 e判定FT = (chipFT != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipFT, nInputAdjustTime) : E判定.Miss;
                                switch (eFTGroup)
                                {
                                    case EFTGroup.打ち分ける:
                                        #region [ LTのヒット処理 ]
                                        //-----------------------------
                                        if (e判定LT != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.LT, chipLT, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        break;
                                    //-----------------------------
                                        #endregion

                                    case EFTGroup.共通:
                                        #region [ LTとFTのヒット処理 ]
                                        //-----------------------------
                                        if ((e判定LT != E判定.Miss) && (e判定FT != E判定.Miss))
                                        {
                                            if (chipLT.n発声位置 < chipFT.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LT, chipLT, inputEvent.nVelocity);
                                            }
                                            else if (chipLT.n発声位置 > chipFT.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LT, chipFT, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LT, chipLT, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.LT, chipFT, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定LT != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.LT, chipLT, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定FT != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.LT, chipFT, inputEvent.nVelocity);
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

                        case Eパッド.FT:
                            #region [ FTとLT(groupingしている場合)のヒット処理 ]
                            //-----------------------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.FT)	// #23857 2010.12.12 yyagi: to support VelocityMin
                                    continue;	// 電子ドラムによる意図的なクロストークを無効にする
                                CDTX.CChip chipLT = this.r指定時刻に一番近い未ヒットChip(nTime, 0x15, nInputAdjustTime);	// LT
                                CDTX.CChip chipFT = this.r指定時刻に一番近い未ヒットChip(nTime, 0x17, nInputAdjustTime);	// FT
                                E判定 e判定LT = (chipLT != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLT, nInputAdjustTime) : E判定.Miss;
                                E判定 e判定FT = (chipFT != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipFT, nInputAdjustTime) : E判定.Miss;
                                switch (eFTGroup)
                                {
                                    case EFTGroup.打ち分ける:
                                        #region [ FTのヒット処理 ]
                                        //-----------------------------
                                        if (e判定FT != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.FT, chipFT, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        //-----------------------------
                                        #endregion
                                        break;

                                    case EFTGroup.共通:
                                        #region [ FTとLTのヒット処理 ]
                                        //-----------------------------
                                        if ((e判定LT != E判定.Miss) && (e判定FT != E判定.Miss))
                                        {
                                            if (chipLT.n発声位置 < chipFT.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.FT, chipLT, inputEvent.nVelocity);
                                            }
                                            else if (chipLT.n発声位置 > chipFT.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.FT, chipFT, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.FT, chipLT, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.FT, chipFT, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定LT != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.FT, chipLT, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定FT != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.FT, chipFT, inputEvent.nVelocity);
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

                        case Eパッド.CY:
                            #region [ CY(とLCとRD:groupingしている場合)のヒット処理 ]
                            //-----------------------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.CY)	// #23857 2010.12.12 yyagi: to support VelocityMin
                                    continue;	// 電子ドラムによる意図的なクロストークを無効にする
                                CDTX.CChip chipCY = this.r指定時刻に一番近い未ヒットChip(nTime, 0x16, nInputAdjustTime);	// CY
                                CDTX.CChip chipRD = this.r指定時刻に一番近い未ヒットChip(nTime, 0x19, nInputAdjustTime);	// RD
                                CDTX.CChip chipLC = CDTXMania.ConfigIni.bシンバルフリー ? this.r指定時刻に一番近い未ヒットChip(nTime, 0x1a, nInputAdjustTime) : null;
                                E判定 e判定CY = (chipCY != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipCY, nInputAdjustTime) : E判定.Miss;
                                E判定 e判定RD = (chipRD != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipRD, nInputAdjustTime) : E判定.Miss;
                                E判定 e判定LC = (chipLC != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLC, nInputAdjustTime) : E判定.Miss;
                                CDTX.CChip[] chipArray = new CDTX.CChip[] { chipCY, chipRD, chipLC };
                                E判定[] e判定Array = new E判定[] { e判定CY, e判定RD, e判定LC };
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
                                //        if ( ( chipArray[ j - 1 ] == null ) || ( ( chipArray[ j ] != null ) && ( chipArray[ j - 1 ].n発声位置 > chipArray[ j ].n発声位置 ) ) )
                                //        {
                                //            // swap
                                //            CDTX.CChip chipTemp = chipArray[ j - 1 ];
                                //            chipArray[ j - 1 ] = chipArray[ j ];
                                //            chipArray[ j ] = chipTemp;
                                //            E判定 e判定Temp = e判定Array[ j - 1 ];
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
                                            
                                            if (e判定CY != E判定.Miss)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.CY, chipCY, inputEvent.nVelocity);
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
                                            if ((e判定Array[i] != E判定.Miss) && ((chipArray[i] == chipCY) || (chipArray[i] == chipLC)))
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.CY, chipArray[i], inputEvent.nVelocity);
                                                bHitted = true;
                                                break;
                                            }
                                            //num10++;
                                        }
                                        if (e判定CY != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.CY, chipCY, inputEvent.nVelocity);
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
                                                if ((e判定Array[i] != E判定.Miss) && ((chipArray[i] == chipCY) || (chipArray[i] == chipRD)))
                                                {
                                                    this.tドラムヒット処理(nTime, Eパッド.CY, chipArray[i], inputEvent.nVelocity);
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
                                            if (e判定Array[i] != E判定.Miss)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.CY, chipArray[i], inputEvent.nVelocity);
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

                        case Eパッド.HHO:
                            #region [ HO(とHCとLC:groupingしている場合)のヒット処理 ]
                            //-----------------------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.HH)
                                    continue;	// 電子ドラムによる意図的なクロストークを無効にする

                                CDTX.CChip chipHC = this.r指定時刻に一番近い未ヒットChip(nTime, 0x11, nInputAdjustTime);	// HC
                                CDTX.CChip chipHO = this.r指定時刻に一番近い未ヒットChip(nTime, 0x18, nInputAdjustTime);	// HO
                                CDTX.CChip chipLC = this.r指定時刻に一番近い未ヒットChip(nTime, 0x1a, nInputAdjustTime);	// LC
                                E判定 e判定HC = (chipHC != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipHC, nInputAdjustTime) : E判定.Miss;
                                E判定 e判定HO = (chipHO != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipHO, nInputAdjustTime) : E判定.Miss;
                                E判定 e判定LC = (chipLC != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLC, nInputAdjustTime) : E判定.Miss;
                                switch (eHHGroup)
                                {
                                    case EHHGroup.全部打ち分ける:
                                        if (e判定HO != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.HHO, chipHO, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;

                                    case EHHGroup.ハイハットのみ打ち分ける:
                                        if ((e判定HO != E判定.Miss) && (e判定LC != E判定.Miss))
                                        {
                                            if (chipHO.n発声位置 < chipLC.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipHO, inputEvent.nVelocity);
                                            }
                                            else if (chipHO.n発声位置 > chipLC.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipLC, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipHO, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipLC, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定HO != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.HHO, chipHO, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定LC != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.HHO, chipLC, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;

                                    case EHHGroup.左シンバルのみ打ち分ける:
                                        if ((e判定HC != E判定.Miss) && (e判定HO != E判定.Miss))
                                        {
                                            if (chipHC.n発声位置 < chipHO.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipHC, inputEvent.nVelocity);
                                            }
                                            else if (chipHC.n発声位置 > chipHO.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipHO, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipHC, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipHO, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定HC != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.HHO, chipHC, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定HO != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.HHO, chipHO, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if (!bHitted)
                                            break;
                                        continue;

                                    case EHHGroup.全部共通:
                                        if (((e判定HC != E判定.Miss) && (e判定HO != E判定.Miss)) && (e判定LC != E判定.Miss))
                                        {
                                            CDTX.CChip chip;
                                            CDTX.CChip[] chipArray = new CDTX.CChip[] { chipHC, chipHO, chipLC };
                                            // ここから、chipArrayをn発生位置の小さい順に並び替える
                                            if (chipArray[1].n発声位置 > chipArray[2].n発声位置)
                                            {
                                                chip = chipArray[1];
                                                chipArray[1] = chipArray[2];
                                                chipArray[2] = chip;
                                            }
                                            if (chipArray[0].n発声位置 > chipArray[1].n発声位置)
                                            {
                                                chip = chipArray[0];
                                                chipArray[0] = chipArray[1];
                                                chipArray[1] = chip;
                                            }
                                            if (chipArray[1].n発声位置 > chipArray[2].n発声位置)
                                            {
                                                chip = chipArray[1];
                                                chipArray[1] = chipArray[2];
                                                chipArray[2] = chip;
                                            }
                                            this.tドラムヒット処理(nTime, Eパッド.HHO, chipArray[0], inputEvent.nVelocity);
                                            if (chipArray[0].n発声位置 == chipArray[1].n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipArray[1], inputEvent.nVelocity);
                                            }
                                            if (chipArray[0].n発声位置 == chipArray[2].n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipArray[2], inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定HC != E判定.Miss) && (e判定HO != E判定.Miss))
                                        {
                                            if (chipHC.n発声位置 < chipHO.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipHC, inputEvent.nVelocity);
                                            }
                                            else if (chipHC.n発声位置 > chipHO.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipHO, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipHC, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipHO, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定HC != E判定.Miss) && (e判定LC != E判定.Miss))
                                        {
                                            if (chipHC.n発声位置 < chipLC.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipHC, inputEvent.nVelocity);
                                            }
                                            else if (chipHC.n発声位置 > chipLC.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipLC, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipHC, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipLC, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定HO != E判定.Miss) && (e判定LC != E判定.Miss))
                                        {
                                            if (chipHO.n発声位置 < chipLC.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipHO, inputEvent.nVelocity);
                                            }
                                            else if (chipHO.n発声位置 > chipLC.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipLC, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipHO, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.HHO, chipLC, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定HC != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.HHO, chipHC, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定HO != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.HHO, chipHO, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定LC != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.HHO, chipLC, inputEvent.nVelocity);
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

                        case Eパッド.RD:
                            #region [ RD(とCYとLC:groupingしている場合)のヒット処理 ]
                            //-----------------------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.RD)	// #23857 2010.12.12 yyagi: to support VelocityMin
                                    continue;	// 電子ドラムによる意図的なクロストークを無効にする
                                CDTX.CChip chipCY = this.r指定時刻に一番近い未ヒットChip(nTime, 0x16, nInputAdjustTime);	// CY
                                CDTX.CChip chipRD = this.r指定時刻に一番近い未ヒットChip(nTime, 0x19, nInputAdjustTime);	// RD
                                CDTX.CChip chipLC = CDTXMania.ConfigIni.bシンバルフリー ? this.r指定時刻に一番近い未ヒットChip(nTime, 0x1a, nInputAdjustTime) : null;
                                E判定 e判定CY = (chipCY != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipCY, nInputAdjustTime) : E判定.Miss;
                                E判定 e判定RD = (chipRD != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipRD, nInputAdjustTime) : E判定.Miss;
                                E判定 e判定LC = (chipLC != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLC, nInputAdjustTime) : E判定.Miss;
                                CDTX.CChip[] chipArray = new CDTX.CChip[] { chipCY, chipRD, chipLC };
                                E判定[] e判定Array = new E判定[] { e判定CY, e判定RD, e判定LC };
                                const int NumOfChips = 3;	// chipArray.GetLength(0)
                                SortChipsByNTime(chipArray, e判定Array, NumOfChips);
                                switch (eCYGroup)
                                {
                                    case ECYGroup.打ち分ける:
                                        if (e判定RD != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.RD, chipRD, inputEvent.nVelocity);
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
                                                if ((e判定Array[i] != E判定.Miss) && ((chipArray[i] == chipCY) || (chipArray[i] == chipRD)))
                                                {
                                                    this.tドラムヒット処理(nTime, Eパッド.CY, chipArray[i], inputEvent.nVelocity);
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
                                            if (e判定Array[i] != E判定.Miss)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.CY, chipArray[i], inputEvent.nVelocity);
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

                        case Eパッド.LC:
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
                                E判定 e判定HC = (chipHC != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipHC, nInputAdjustTime) : E判定.Miss;
                                E判定 e判定HO = (chipHO != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipHO, nInputAdjustTime) : E判定.Miss;
                                E判定 e判定LC = (chipLC != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLC, nInputAdjustTime) : E判定.Miss;
                                E判定 e判定CY = (chipCY != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipCY, nInputAdjustTime) : E判定.Miss;
                                E判定 e判定RD = (chipRD != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipRD, nInputAdjustTime) : E判定.Miss;
                                CDTX.CChip[] chipArray = new CDTX.CChip[] { chipHC, chipHO, chipLC, chipCY, chipRD };
                                E判定[] e判定Array = new E判定[] { e判定HC, e判定HO, e判定LC, e判定CY, e判定RD };
                                const int NumOfChips = 5;	// chipArray.GetLength(0)
                                SortChipsByNTime(chipArray, e判定Array, NumOfChips);

                                switch (eHHGroup)
                                {
                                    case EHHGroup.全部打ち分ける:
                                    case EHHGroup.左シンバルのみ打ち分ける:
                                        #region[左シンバルのみ打ち分ける]
                                        if (!CDTXMania.ConfigIni.bシンバルフリー)
                                        {
                                            if (e判定LC != E判定.Miss)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LC, chipLC, inputEvent.nVelocity);
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
                                            if ((e判定Array[i] != E判定.Miss) && (((chipArray[i] == chipLC) || (chipArray[i] == chipCY)) || ((chipArray[i] == chipRD) && (CDTXMania.ConfigIni.eCYGroup == ECYGroup.共通))))
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LC, chipArray[i], inputEvent.nVelocity);
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
                                                if ((e判定Array[i] != E判定.Miss) && (((chipArray[i] == chipLC) || (chipArray[i] == chipHC)) || (chipArray[i] == chipHO)))
                                                {
                                                    this.tドラムヒット処理(nTime, Eパッド.LC, chipArray[i], inputEvent.nVelocity);
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
                                            if ((e判定Array[i] != E判定.Miss) && ((chipArray[i] != chipRD) || (CDTXMania.ConfigIni.eCYGroup == ECYGroup.共通)))
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LC, chipArray[i], inputEvent.nVelocity);
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
                        case Eパッド.LP:
                            #region [ LPのヒット処理 ]
                            //-----------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.LP)
                                    continue;
                                CDTX.CChip chipBD  = this.r指定時刻に一番近い未ヒットChip(nTime, 0x13, nInputAdjustTime + nPedalLagTime);	// BD
                                CDTX.CChip chipLP  = this.r指定時刻に一番近い未ヒットChip(nTime, 0x1b, nInputAdjustTime + nPedalLagTime);	// LP
                                CDTX.CChip chipLBD = this.r指定時刻に一番近い未ヒットChip(nTime, 0x1c, nInputAdjustTime + nPedalLagTime);	// LBD
                                E判定 e判定BD  = (chipBD  != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipBD,  nInputAdjustTime + nPedalLagTime) : E判定.Miss;
                                E判定 e判定LP  = (chipLP  != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLP,  nInputAdjustTime + nPedalLagTime) : E判定.Miss;
                                E判定 e判定LBD = (chipLBD != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLBD, nInputAdjustTime + nPedalLagTime) : E判定.Miss;
                                switch (eBDGroup)
                                {
                                    case EBDGroup.左右ペダルのみ打ち分ける:
                                        #region[ LPのヒット処理]
                                        if (e判定LP != E判定.Miss && e判定LBD != E判定.Miss)
                                        {
                                            if (chipLP.n発声位置 < chipLBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LP, chipLP, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                if (chipLP.n発声位置 > chipLBD.n発声位置)
                                                {
                                                    this.tドラムヒット処理(nTime, Eパッド.LP, chipLBD, inputEvent.nVelocity);
                                                }
                                                else
                                                {
                                                    this.tドラムヒット処理(nTime, Eパッド.LP, chipLP, inputEvent.nVelocity);
                                                    this.tドラムヒット処理(nTime, Eパッド.LP, chipLBD, inputEvent.nVelocity);
                                                }
                                            }
                                            bHitted = true;
                                        }
                                        else
                                        {
                                            if (e判定LP != E判定.Miss)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LP, chipLP, inputEvent.nVelocity);
                                                bHitted = true;
                                            }
                                            else
                                            {
                                                if (e判定LBD != E判定.Miss)
                                                {
                                                    this.tドラムヒット処理(nTime, Eパッド.LP, chipLBD, inputEvent.nVelocity);
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
                                        if (((e判定LP != E判定.Miss) && (e判定LBD != E判定.Miss)) && (e判定BD != E判定.Miss))
                                        {
                                            CDTX.CChip chip;
                                            CDTX.CChip[] chipArray = new CDTX.CChip[] { chipLP, chipLBD, chipBD };
                                            if (chipArray[1].n発声位置 > chipArray[2].n発声位置)
                                            {
                                                chip = chipArray[1];
                                                chipArray[1] = chipArray[2];
                                                chipArray[2] = chip;
                                            }
                                            if (chipArray[0].n発声位置 > chipArray[1].n発声位置)
                                            {
                                                chip = chipArray[0];
                                                chipArray[0] = chipArray[1];
                                                chipArray[1] = chip;
                                            }
                                            if (chipArray[1].n発声位置 > chipArray[2].n発声位置)
                                            {
                                                chip = chipArray[1];
                                                chipArray[1] = chipArray[2];
                                                chipArray[2] = chip;
                                            }
                                            this.tドラムヒット処理(nTime, Eパッド.LP, chipArray[0], inputEvent.nVelocity);
                                            if (chipArray[0].n発声位置 == chipArray[1].n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LP, chipArray[1], inputEvent.nVelocity);
                                            }
                                            if (chipArray[0].n発声位置 == chipArray[2].n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LP, chipArray[2], inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定LP != E判定.Miss) && (e判定LBD != E判定.Miss))
                                        {
                                            if (chipLP.n発声位置 < chipLBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LP, chipLP, inputEvent.nVelocity);
                                            }
                                            else if (chipLP.n発声位置 > chipLBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LP, chipLBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LP, chipLP, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.LP, chipLBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定LP != E判定.Miss) && (e判定BD != E判定.Miss))
                                        {
                                            if (chipLP.n発声位置 < chipBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LP, chipLP, inputEvent.nVelocity);
                                            }
                                            else if (chipLP.n発声位置 > chipBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LP, chipBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LP, chipLP, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.LP, chipBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定LBD != E判定.Miss) && (e判定BD != E判定.Miss))
                                        {
                                            if (chipLBD.n発声位置 < chipBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LP, chipLBD, inputEvent.nVelocity);
                                            }
                                            else if (chipLBD.n発声位置 > chipBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LP, chipBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LP, chipLBD, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.LP, chipBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定LP != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.LP, chipLP, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定LBD != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.LP, chipLBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定BD != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.LP, chipBD, inputEvent.nVelocity);
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
                                        if (e判定LP != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.LP, chipLP, inputEvent.nVelocity);
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

                        case Eパッド.LBD:
                            #region [ LBDのヒット処理 ]
                            //-----------------
                            {
                                if (inputEvent.nVelocity <= CDTXMania.ConfigIni.nVelocityMin.LBD)
                                    continue;
                                CDTX.CChip chipBD  = this.r指定時刻に一番近い未ヒットChip(nTime, 0x13, nInputAdjustTime + nPedalLagTime);	// BD
                                CDTX.CChip chipLP  = this.r指定時刻に一番近い未ヒットChip(nTime, 0x1b, nInputAdjustTime + nPedalLagTime);	// LP
                                CDTX.CChip chipLBD = this.r指定時刻に一番近い未ヒットChip(nTime, 0x1c, nInputAdjustTime + nPedalLagTime);	// LBD
                                E判定 e判定BD  = (chipBD  != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipBD,  nInputAdjustTime + nPedalLagTime) : E判定.Miss;
                                E判定 e判定LP  = (chipLP  != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLP,  nInputAdjustTime + nPedalLagTime) : E判定.Miss;
                                E判定 e判定LBD = (chipLBD != null) ? this.e指定時刻からChipのJUDGEを返す(nTime, chipLBD, nInputAdjustTime + nPedalLagTime) : E判定.Miss;
                                switch (eBDGroup)
                                {
                                    case EBDGroup.BDとLPで打ち分ける:
                                        #region[ BD & LBD | LP ]
                                        if( e判定BD != E判定.Miss && e判定LBD != E判定.Miss )
                                        {
                                            if( chipBD.n発声位置 < chipLBD.n発声位置 )
                                            {
                                                this.tドラムヒット処理( nTime, Eパッド.LBD, chipBD, inputEvent.nVelocity );
                                            }
                                            else if( chipBD.n発声位置 > chipLBD.n発声位置 )
                                            {
                                                this.tドラムヒット処理( nTime, Eパッド.LBD, chipLBD, inputEvent.nVelocity );
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理( nTime, Eパッド.LBD, chipBD, inputEvent.nVelocity );
                                                this.tドラムヒット処理( nTime, Eパッド.LBD, chipLBD, inputEvent.nVelocity );
                                            }
                                            bHitted = true;
                                        }
                                        else if( e判定BD != E判定.Miss )
                                        {
                                            this.tドラムヒット処理( nTime, Eパッド.LBD, chipBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if( e判定LBD != E判定.Miss )
                                        {
                                            this.tドラムヒット処理( nTime, Eパッド.LBD, chipLBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        if( bHitted )
                                            continue;
                                        else
                                            break;
                                        #endregion

                                    case EBDGroup.左右ペダルのみ打ち分ける:
                                        #region[ LPのヒット処理]
                                        if (e判定LP != E判定.Miss && e判定LBD != E判定.Miss)
                                        {
                                            if (chipLP.n発声位置 < chipLBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LBD, chipLP, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                if (chipLP.n発声位置 > chipLBD.n発声位置)
                                                {
                                                    this.tドラムヒット処理(nTime, Eパッド.LBD, chipLBD, inputEvent.nVelocity);
                                                }
                                                else
                                                {
                                                    this.tドラムヒット処理(nTime, Eパッド.LBD, chipLP, inputEvent.nVelocity);
                                                    this.tドラムヒット処理(nTime, Eパッド.LBD, chipLBD, inputEvent.nVelocity);
                                                }
                                            }
                                            bHitted = true;
                                        }
                                        else
                                        {
                                            if (e判定LP != E判定.Miss)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LBD, chipLP, inputEvent.nVelocity);
                                                bHitted = true;
                                            }
                                            else
                                            {
                                                if (e判定LBD != E判定.Miss)
                                                {
                                                    this.tドラムヒット処理(nTime, Eパッド.LBD, chipLBD, inputEvent.nVelocity);
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
                                        if (((e判定LP != E判定.Miss) && (e判定LBD != E判定.Miss)) && (e判定BD != E判定.Miss))
                                        {
                                            CDTX.CChip chip;
                                            CDTX.CChip[] chipArray = new CDTX.CChip[] { chipLP, chipLBD, chipBD };
                                            if (chipArray[1].n発声位置 > chipArray[2].n発声位置)
                                            {
                                                chip = chipArray[1];
                                                chipArray[1] = chipArray[2];
                                                chipArray[2] = chip;
                                            }
                                            if (chipArray[0].n発声位置 > chipArray[1].n発声位置)
                                            {
                                                chip = chipArray[0];
                                                chipArray[0] = chipArray[1];
                                                chipArray[1] = chip;
                                            }
                                            if (chipArray[1].n発声位置 > chipArray[2].n発声位置)
                                            {
                                                chip = chipArray[1];
                                                chipArray[1] = chipArray[2];
                                                chipArray[2] = chip;
                                            }
                                            this.tドラムヒット処理(nTime, Eパッド.LBD, chipArray[0], inputEvent.nVelocity);
                                            if (chipArray[0].n発声位置 == chipArray[1].n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LBD, chipArray[1], inputEvent.nVelocity);
                                            }
                                            if (chipArray[0].n発声位置 == chipArray[2].n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LBD, chipArray[2], inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定LP != E判定.Miss) && (e判定LBD != E判定.Miss))
                                        {
                                            if (chipLP.n発声位置 < chipLBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LBD, chipLP, inputEvent.nVelocity);
                                            }
                                            else if (chipLP.n発声位置 > chipLBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LBD, chipLBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LBD, chipLP, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.LBD, chipLBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定LP != E判定.Miss) && (e判定BD != E判定.Miss))
                                        {
                                            if (chipLP.n発声位置 < chipBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LBD, chipLP, inputEvent.nVelocity);
                                            }
                                            else if (chipLP.n発声位置 > chipBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LBD, chipBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LBD, chipLP, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.LBD, chipBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if ((e判定LBD != E判定.Miss) && (e判定BD != E判定.Miss))
                                        {
                                            if (chipLBD.n発声位置 < chipBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LBD, chipLBD, inputEvent.nVelocity);
                                            }
                                            else if (chipLBD.n発声位置 > chipBD.n発声位置)
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LBD, chipBD, inputEvent.nVelocity);
                                            }
                                            else
                                            {
                                                this.tドラムヒット処理(nTime, Eパッド.LBD, chipLBD, inputEvent.nVelocity);
                                                this.tドラムヒット処理(nTime, Eパッド.LBD, chipBD, inputEvent.nVelocity);
                                            }
                                            bHitted = true;
                                        }
                                        else if (e判定LP != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.LBD, chipLP, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定LBD != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.LBD, chipLBD, inputEvent.nVelocity);
                                            bHitted = true;
                                        }
                                        else if (e判定BD != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.LBD, chipBD, inputEvent.nVelocity);
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
                                        if (e判定LBD != E判定.Miss)
                                        {
                                            this.tドラムヒット処理(nTime, Eパッド.LBD, chipLBD, inputEvent.nVelocity);
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
                    this.actLaneFlushD.Start((Eレーン)this.nパッド0Atoレーン07[nPad], ((float)inputEvent.nVelocity) / 127f);
                    this.actPad.Hit(this.nパッド0Atoパッド08[nPad]);

                    if (CDTXMania.ConfigIni.bドラム打音を発声する)
                    {
                        CDTX.CChip rChip = this.r空うちChip(E楽器パート.DRUMS, (Eパッド)nPad);
                        if (rChip != null)
                        {
                            #region [ (B1) 空打ち音が譜面で指定されているのでそれを再生する。]
                            //-----------------
                            this.tサウンド再生(rChip, CSound管理.rc演奏用タイマ.nシステム時刻, E楽器パート.DRUMS, CDTXMania.ConfigIni.n手動再生音量, CDTXMania.ConfigIni.b演奏音を強調する.Drums);
                            //-----------------
                            #endregion
                        }
                        else
                        {
                            #region [ (B2) 空打ち音が指定されていないので一番近いチップを探して再生する。]
                            //-----------------
                            switch (((Eパッド)nPad))
                            {
                                case Eパッド.HH:
                                    #region [ *** ]
                                    //-----------------------------
                                    {
                                        CDTX.CChip chipHC = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[0], nInputAdjustTime);
                                        CDTX.CChip chipHO = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[7], nInputAdjustTime);
                                        CDTX.CChip chipLC = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[9], nInputAdjustTime);
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
                                                else if (chipHO.n発声位置 < chipLC.n発声位置)
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

                                case Eパッド.LT:
                                    #region [ *** ]
                                    //-----------------------------
                                    {
                                        CDTX.CChip chipLT = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[4], nInputAdjustTime);
                                        CDTX.CChip chipFT = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[5], nInputAdjustTime);
                                        if (CDTXMania.ConfigIni.eFTGroup != EFTGroup.打ち分ける)
                                            rChip = (chipLT != null) ? chipLT : chipFT;
                                        else
                                            rChip = chipLT;
                                    }
                                    //-----------------------------
                                    #endregion
                                    break;

                                case Eパッド.FT:
                                    #region [ *** ]
                                    //-----------------------------
                                    {
                                        CDTX.CChip chipLT = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[4], nInputAdjustTime);
                                        CDTX.CChip chipFT = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[5], nInputAdjustTime);
                                        if (CDTXMania.ConfigIni.eFTGroup != EFTGroup.打ち分ける)
                                            rChip = (chipFT != null) ? chipFT : chipLT;
                                        else
                                            rChip = chipFT;
                                    }
                                    //-----------------------------
                                    #endregion
                                    break;

                                case Eパッド.CY:
                                    #region [ *** ]
                                    //-----------------------------
                                    {

                                        CDTX.CChip chipCY = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[6], nInputAdjustTime);
                                        CDTX.CChip chipRD = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[8], nInputAdjustTime);
                                        if (CDTXMania.ConfigIni.eCYGroup != ECYGroup.打ち分ける)
                                            rChip = (chipCY != null) ? chipCY : chipRD;
                                        else
                                            rChip = chipCY;
                                    }
                                    //-----------------------------
                                    #endregion
                                    break;

                                case Eパッド.HHO:
                                    #region [ *** ]
                                    //-----------------------------
                                    {
                                        CDTX.CChip chipHC = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[0], nInputAdjustTime);
                                        CDTX.CChip chipHO = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[7], nInputAdjustTime);
                                        CDTX.CChip chipLC = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[9], nInputAdjustTime);
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
                                                else if (chipHC.n発声位置 < chipLC.n発声位置)
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

                                case Eパッド.RD:
                                    #region [ *** ]
                                    //-----------------------------
                                    {
                                        CDTX.CChip chipCY = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[6], nInputAdjustTime);
                                        CDTX.CChip chipRD = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[8], nInputAdjustTime);
                                        if (CDTXMania.ConfigIni.eCYGroup != ECYGroup.打ち分ける)
                                            rChip = (chipRD != null) ? chipRD : chipCY;
                                        else
                                            rChip = chipRD;
                                    }
                                    //-----------------------------
                                    #endregion
                                    break;

                                case Eパッド.LC:
                                    #region [ *** ]
                                    //-----------------------------
                                    {
                                        CDTX.CChip chipHC = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[0], nInputAdjustTime);
                                        CDTX.CChip chipHO = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[7], nInputAdjustTime);
                                        CDTX.CChip chipLC = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[9], nInputAdjustTime);
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
                                                else if (chipHC.n発声位置 < chipHO.n発声位置)
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

                                case Eパッド.BD:
                                    #region [ *** ]
                                    //-----------------------------
                                    {
                                        CDTX.CChip chipBD = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[2], nInputAdjustTime + nPedalLagTime);
                                        CDTX.CChip chipLP = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[10], nInputAdjustTime + nPedalLagTime);
                                        CDTX.CChip chipLBD = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[11], nInputAdjustTime + nPedalLagTime);
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
                                                    if( chipBD.n発声時刻ms >= chipLBD.n発声時刻ms )
                                                        rChip = chipBD;
                                                    else if( chipBD.n発声時刻ms < chipLBD.n発声時刻ms )
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
                                                else if (chipLP.n発声位置 < chipLBD.n発声位置)
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
                                    
                                case Eパッド.LP:
                                    #region [ *** ]
                                    //-----------------------------
                                    {
                                        CDTX.CChip chipBD = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[2], nInputAdjustTime + nPedalLagTime );
                                        CDTX.CChip chipLP = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[10], nInputAdjustTime + nPedalLagTime );
                                        CDTX.CChip chipLBD = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[11], nInputAdjustTime + nPedalLagTime );
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
                                                else if (chipLBD.n発声位置 < chipBD.n発声位置)
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

                                case Eパッド.LBD:
                                    #region [ *** ]
                                    {
                                        CDTX.CChip chipBD = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[2], nInputAdjustTime + nPedalLagTime);
                                        CDTX.CChip chipLP = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[10], nInputAdjustTime + nPedalLagTime);
                                        CDTX.CChip chipLBD = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[11], nInputAdjustTime + nPedalLagTime);
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
                                                    if( chipBD.n発声時刻ms <= chipLBD.n発声時刻ms )
                                                        rChip = chipLBD;
                                                    else if( chipBD.n発声時刻ms > chipLBD.n発声時刻ms )
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
                                                else if (chipLP.n発声位置 < chipBD.n発声位置)
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
                                    rChip = this.r指定時刻に一番近いChip・ヒット未済問わず不可視考慮(nTime, this.nパッド0Atoチャンネル0A[nPad], nInputAdjustTime);
                                    //-----------------------------
                                    #endregion
                                    break;
                            }
                            if (rChip != null)
                            {
                                // 空打ち音が見つかったので再生する。
                                this.tサウンド再生(rChip, CSound管理.rc演奏用タイマ.nシステム時刻, E楽器パート.DRUMS, CDTXMania.ConfigIni.n手動再生音量, CDTXMania.ConfigIni.b演奏音を強調する.Drums);
                            }
                            //-----------------
                            #endregion
                        }
                    }

                    // BAD or TIGHT 時の処理。
                    if (CDTXMania.ConfigIni.bTight)
                        this.tチップのヒット処理・BadならびにTight時のMiss(E楽器パート.DRUMS, this.nパッド0Atoレーン07[nPad]);
                    //-----------------------------
                    #endregion
                }
            }
        }

		// t入力処理・ドラム()からメソッドを抽出したもの。
		/// <summary>
		/// chipArrayの中を, n発生位置の小さい順に並べる + nullを大きい方に退かす。セットでe判定Arrayも並べ直す。
		/// </summary>
		/// <param name="chipArray">ソート対象chip群</param>
		/// <param name="e判定Array">ソート対象e判定群</param>
		/// <param name="NumOfChips">チップ数</param>
		private static void SortChipsByNTime( CDTX.CChip[] chipArray, E判定[] e判定Array, int NumOfChips )
		{
			for ( int i = 0; i < NumOfChips - 1; i++ )
			{
				//num9 = 2;
				//while( num9 > num8 )
				for ( int j = NumOfChips - 1; j > i; j-- )
				{
					if ( ( chipArray[ j - 1 ] == null ) || ( ( chipArray[ j ] != null ) && ( chipArray[ j - 1 ].n発声位置 > chipArray[ j ].n発声位置 ) ) )
					{
						// swap
						CDTX.CChip chipTemp = chipArray[ j - 1 ];
						chipArray[ j - 1 ] = chipArray[ j ];
						chipArray[ j ] = chipTemp;
						E判定 e判定Temp = e判定Array[ j - 1 ];
						e判定Array[ j - 1 ] = e判定Array[ j ];
						e判定Array[ j ] = e判定Temp;
					}
					//num9--;
				}
				//num8++;
			}
		}

		protected override void t背景テクスチャの生成()
		{
            Rectangle bgrect = new Rectangle(980, 0, 0, 0);
            if (CDTXMania.ConfigIni.bBGA有効)
            {
                bgrect = new Rectangle(980, 0, 278, 355);
            }
			string DefaultBgFilename = @"Graphics\7_background.jpg";
			string BgFilename = "";
			if ( ( ( CDTXMania.DTX.BACKGROUND != null ) && ( CDTXMania.DTX.BACKGROUND.Length > 0 ) ) && !CDTXMania.ConfigIni.bストイックモード )
			{
				BgFilename = CDTXMania.DTX.strフォルダ名 + CDTXMania.DTX.BACKGROUND;
			}
			base.t背景テクスチャの生成( DefaultBgFilename, bgrect, BgFilename );
		}

        protected override void t進行描画・チップ・模様のみ・ドラムス(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip)
        {
            if (configIni.bDrums有効)
            {
                #region [ Sudden処理 ]
                if ((CDTXMania.ConfigIni.nHidSud.Drums == 2) || (CDTXMania.ConfigIni.nHidSud.Drums == 3))
                {
                    if (pChip.nバーからの距離dot.Drums < 200)
                    {
                        pChip.b可視 = true;
                        pChip.n透明度 = 0xff;
                    }
                    else if (pChip.nバーからの距離dot.Drums < 250)
                    {
                        pChip.b可視 = true;
                        pChip.n透明度 = 0xff - ((int)((((double)(pChip.nバーからの距離dot.Drums - 200)) * 255.0) / 50.0));
                    }
                    else
                    {
                        pChip.b可視 = false;
                        pChip.n透明度 = 0;
                    }
                }
                #endregion
                #region [ Hidden処理 ]
                if ((CDTXMania.ConfigIni.nHidSud.Drums == 1) || (CDTXMania.ConfigIni.nHidSud.Drums == 3))
                {
                    if (pChip.nバーからの距離dot.Drums < 100)
                    {
                        pChip.b可視 = false;
                    }
                    else if (pChip.nバーからの距離dot.Drums < 150)
                    {
                        pChip.b可視 = true;
                        pChip.n透明度 = (int)((((double)(pChip.nバーからの距離dot.Drums - 100)) * 255.0) / 50.0);
                    }
                }
                #endregion
                #region [ ステルス処理 ]
                if (CDTXMania.ConfigIni.nHidSud.Drums == 4)
                {
                    pChip.b可視 = false;
                }
                #endregion
                if (!pChip.bHit && pChip.b可視)
                {
                    if (this.txチップ != null)
                    {
                        this.txチップ.n透明度 = pChip.n透明度;
                    }
                    int x = this.nチャンネルtoX座標[pChip.nチャンネル番号 - 0x11];

                    if (configIni.eLaneType.Drums == Eタイプ.A)
                    {
                        if (configIni.eRDPosition == ERDPosition.RCRD)
                        {
                            x = this.nチャンネルtoX座標[pChip.nチャンネル番号 - 0x11];
                        }
                        else if (configIni.eRDPosition == ERDPosition.RDRC)
                        {
                            x = this.nチャンネルtoX座標改[pChip.nチャンネル番号 - 0x11];
                        }
                    }
                    else if (configIni.eLaneType.Drums == Eタイプ.B)
                    {
                        if (configIni.eRDPosition == ERDPosition.RCRD)
                        {
                            x = this.nチャンネルtoX座標B[pChip.nチャンネル番号 - 0x11];
                        }
                        else if (configIni.eRDPosition == ERDPosition.RDRC)
                        {
                            x = this.nチャンネルtoX座標B改[pChip.nチャンネル番号 - 0x11];
                        }
                    }
                    else if (configIni.eLaneType.Drums == Eタイプ.C)
                    {
                        if (configIni.eRDPosition == ERDPosition.RCRD)
                        {
                            x = this.nチャンネルtoX座標C[pChip.nチャンネル番号 - 0x11];
                        }
                        else if (configIni.eRDPosition == ERDPosition.RDRC)
                        {
                            x = this.nチャンネルtoX座標C改[pChip.nチャンネル番号 - 0x11];
                        }
                    }
                    else if (configIni.eLaneType.Drums == Eタイプ.D)
                    {
                        if (configIni.eRDPosition == ERDPosition.RCRD)
                        {
                            x = this.nチャンネルtoX座標D[pChip.nチャンネル番号 - 0x11];
                        }
                        else if (configIni.eRDPosition == ERDPosition.RDRC)
                        {
                            x = this.nチャンネルtoX座標D改[pChip.nチャンネル番号 - 0x11];
                        }
                    }

                    if (configIni.eRDPosition == ERDPosition.RDRC)
                    {
                        if (configIni.eLaneType.Drums == Eタイプ.A)
                        {
                            x = this.nチャンネルtoX座標改[pChip.nチャンネル番号 - 0x11];
                        }
                        else if (configIni.eLaneType.Drums == Eタイプ.B)
                        {
                            x = this.nチャンネルtoX座標B改[pChip.nチャンネル番号 - 0x11];
                        }
                    }

                    int y = configIni.bReverse.Drums ? (base.nJudgeLinePosY.Drums + pChip.nバーからの距離dot.Drums) : (base.nJudgeLinePosY.Drums - pChip.nバーからの距離dot.Drums);
                    if (base.txチップ != null)
                    {
                        base.txチップ.vc拡大縮小倍率 = new Vector3((float)pChip.dbチップサイズ倍率, (float)pChip.dbチップサイズ倍率, 1f);
                    }
                    int num9 = this.ctチップ模様アニメ.Drums.n現在の値;

                    switch (pChip.nチャンネル番号)
                    {
                        case 0x11:
                            x = (x + 0x10) - ((int)((32.0 * pChip.dbチップサイズ倍率) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.t2D描画(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(60 + 10, 128 + (num9 * 64), 0x2e + 10, 64));
                            }
                            break;

                        case 0x12:
                            x = (x + 0x10) - ((int)((32.0 * pChip.dbチップサイズ倍率) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.t2D描画(CDTXMania.app.Device, x - 5, y - 32, new Rectangle(0x6a + 20, 128 + (num9 * 64), 0x36 + 10, 64));
                            }
                            break;

                        case 0x13:
                            x = (x + 0x16) - ((int)((44.0 * pChip.dbチップサイズ倍率) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.t2D描画(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(0, 128 + (num9 * 0x40), 60 + 10, 0x40));
                            }
                            break;

                        case 0x14:
                            x = (x + 0x10) - ((int)((32.0 * pChip.dbチップサイズ倍率) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.t2D描画(CDTXMania.app.Device, x - 5, y - 32, new Rectangle(160 + 30, 128 + (num9 * 0x40), 0x2e + 10, 64));
                            }
                            break;

                        case 0x15:
                            x = (x + 0x10) - ((int)((32.0 * pChip.dbチップサイズ倍率) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.t2D描画(CDTXMania.app.Device, x - 5, y - 32, new Rectangle(0xce + 40, 128 + (num9 * 0x40), 0x2e + 10, 64));
                            }
                            break;

                        case 0x16:
                            x = (x + 19) - ((int)((38.0 * pChip.dbチップサイズ倍率) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.t2D描画(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(298 + 60, 128 + (num9 * 64), 64 + 10, 64));
                            }
                            break;

                        case 0x17:
                            x = (x + 0x10) - ((int)((32.0 * pChip.dbチップサイズ倍率) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.t2D描画(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(0xfc + 50, 128 + (num9 * 64), 0x2e + 10, 0x40));
                            }
                            break;

                        case 0x18:
                            x = (x + 13) - ((int)((26.0 * pChip.dbチップサイズ倍率) / 2.0));
                            if (this.txチップ != null)
                            {
                                switch (configIni.eHHOGraphics.Drums)
                                {
                                    case Eタイプ.A:
                                        x = (x + 14) - ((int)((26.0 * pChip.dbチップサイズ倍率) / 2.0));
                                        this.txチップ.t2D描画(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(0x200 + 100, 128 + (num9 * 64), 0x26 + 10, 64));
                                        break;

                                    /*
                                case Eタイプ.B:
                                    x = (x + 14) - ((int)((26.0 * pChip.dbチップサイズ倍率) / 2.0));
                                    this.txチップ.t2D描画(CDTXMania.app.Device, x, y - 32, new Rectangle(0x200, 128 + (num9 * 64), 0x26, 64));
                                    break;
                                     */

                                    case Eタイプ.C:
                                        x = (x + 13) - ((int)((32.0 * pChip.dbチップサイズ倍率) / 2.0));
                                        this.txチップ.t2D描画(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(60 + 10, 128 + (num9 * 64), 0x2e + 10, 64));
                                        break;
                                }
                            }
                            break;

                        case 0x19:
                            x = (x + 13) - ((int)((26.0 * pChip.dbチップサイズ倍率) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.t2D描画(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(0x16a + 70, 128 + (num9 * 64), 0x26 + 10, 0x40));
                            }
                            break;

                        case 0x1a:
                            x = (x + 0x13) - ((int)((38.0 * pChip.dbチップサイズ倍率) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.t2D描画(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(448 + 90, 128 + (num9 * 64), 64 + 10, 64));
                            }
                            break;

                        case 0x1b:
                            x = (x + 0x13) - ((int)((38.0 * pChip.dbチップサイズ倍率) / 2.0));
                            if (this.txチップ != null)
                            {
                                this.txチップ.t2D描画(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(550 + 110, 128 + (num9 * 64), 0x30 + 10, 64));
                            }
                            break;

                        case 0x1c:
                            x = (x + 0x13) - ((int)((38.0 * pChip.dbチップサイズ倍率) / 2.0));
                            if (this.txチップ != null)
                            {

                                if (configIni.eLBDGraphics.Drums == Eタイプ.A)
                                {
                                    this.txチップ.t2D描画(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(550 + 110, 128 + (num9 * 64), 0x30, 0x40));
                                }
                                else if (configIni.eLBDGraphics.Drums == Eタイプ.B)
                                {
                                    this.txチップ.t2D描画(CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle(400 + 80, 128 + (num9 * 64), 0x30 + 10, 0x40));
                                }
                            }
                            break;
                    }
                    if (this.txチップ != null)
                    {
                        this.txチップ.vc拡大縮小倍率 = new Vector3(1f, 1f, 1f);
                        this.txチップ.n透明度 = 0xff;
                    }
                }

                /*
				int indexSevenLanes = this.nチャンネル0Atoレーン07[ pChip.nチャンネル番号 - 0x11 ];
				if ( ( configIni.bAutoPlay[ indexSevenLanes ] && !pChip.bHit ) && ( pChip.nバーからの距離dot.Drums < 0 ) )
				{
					pChip.bHit = true;
					this.actLaneFlushD.Start( (Eレーン) indexSevenLanes, ( (float) CInput管理.n通常音量 ) / 127f );
					bool flag = this.bフィルイン中;
					bool flag2 = this.bフィルイン中 && this.bフィルイン区間の最後のChipである( pChip );
					//bool flag3 = flag2;
                    // #31602 2013.6.24 yyagi 判定ラインの表示位置をずらしたら、チップのヒットエフェクトの表示もずらすために、nJudgeLine..を追加
                    this.actChipFireD.Start( (Eレーン)indexSevenLanes, flag, flag2, flag2, nJudgeLinePosY_delta.Drums );
					this.actPad.Hit( this.nチャンネル0Atoパッド08[ pChip.nチャンネル番号 - 0x11 ] );
					this.tサウンド再生( pChip, CSound管理.rc演奏用タイマ.n前回リセットした時のシステム時刻 + pChip.n発声時刻ms, E楽器パート.DRUMS, dTX.nモニタを考慮した音量( E楽器パート.DRUMS ) );
					this.tチップのヒット処理( pChip.n発声時刻ms, pChip );
				}
                */
                return;
            }	// end of "if configIni.bDrums有効"
            if (!pChip.bHit && (pChip.nバーからの距離dot.Drums < 0))
            {
                //this.tサウンド再生(pChip, CSound管理.rc演奏用タイマ.n前回リセットした時のシステム時刻 + pChip.n発声時刻ms, E楽器パート.DRUMS, dTX.nモニタを考慮した音量(E楽器パート.DRUMS));
                pChip.bHit = true;
            }
        }
		protected override void t進行描画・チップ・ドラムス( CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip )
		{
			if( configIni.bDrums有効 )
			{
				#region [ Sudden処理 ]
                if( ( CDTXMania.ConfigIni.nHidSud.Drums == 2 ) || ( CDTXMania.ConfigIni.nHidSud.Drums == 3 ) )
				{
					if( pChip.nバーからの距離dot.Drums < 200 )
					{
						pChip.b可視 = true;
						pChip.n透明度 = 0xff;
					}
					else if( pChip.nバーからの距離dot.Drums < 250 )
					{
						pChip.b可視 = true;
						pChip.n透明度 = 0xff - ( (int) ( ( ( (double) ( pChip.nバーからの距離dot.Drums - 200 ) ) * 255.0 ) / 50.0 ) );
					}
					else
					{
						pChip.b可視 = false;
						pChip.n透明度 = 0;
					}
				}
				#endregion
				#region [ Hidden処理 ]
                if( ( CDTXMania.ConfigIni.nHidSud.Drums == 1 ) || ( CDTXMania.ConfigIni.nHidSud.Drums == 3 ) )
				{
					if( pChip.nバーからの距離dot.Drums < 100 )
					{
						pChip.b可視 = false;
					}
					else if( pChip.nバーからの距離dot.Drums < 150 )
					{
						pChip.b可視 = true;
						pChip.n透明度 = (int) ( ( ( (double) ( pChip.nバーからの距離dot.Drums - 100 ) ) * 255.0 ) / 50.0 );
					}
				}
				#endregion
                #region [ ステルス処理 ]
                if( CDTXMania.ConfigIni.nHidSud.Drums == 4 )
                {
                        pChip.b可視 = false;
                }
                #endregion
				if( !pChip.bHit && pChip.b可視 )
                {
                    if( this.txチップ != null )
                    {
                        this.txチップ.n透明度 = pChip.n透明度;
                    }
                    int x = this.nチャンネルtoX座標[ pChip.nチャンネル番号 - 0x11 ];

                    if( configIni.eLaneType.Drums == Eタイプ.A )
                    {
                        if (configIni.eRDPosition == ERDPosition.RCRD)
                        {
                            x = this.nチャンネルtoX座標[ pChip.nチャンネル番号 - 0x11 ];
                        }
                        else if( configIni.eRDPosition == ERDPosition.RDRC )
                        {
                            x = this.nチャンネルtoX座標改[ pChip.nチャンネル番号 - 0x11 ];
                        }
                    }
                    else if( configIni.eLaneType.Drums == Eタイプ.B )
                    {
                        if( configIni.eRDPosition == ERDPosition.RCRD )
                        {
                            x = this.nチャンネルtoX座標B[ pChip.nチャンネル番号 - 0x11 ];
                        }
                        else if( configIni.eRDPosition == ERDPosition.RDRC )
                        {
                            x = this.nチャンネルtoX座標B改[ pChip.nチャンネル番号 - 0x11 ];
                        }
                    }
                    else if( configIni.eLaneType.Drums == Eタイプ.C )
                    {
                        if( configIni.eRDPosition == ERDPosition.RCRD )
                        {
                            x = this.nチャンネルtoX座標C[ pChip.nチャンネル番号 - 0x11 ];
                        }
                        else if( configIni.eRDPosition == ERDPosition.RDRC )
                        {
                            x = this.nチャンネルtoX座標C改[ pChip.nチャンネル番号 - 0x11 ];
                        }
                    }
                    else if( configIni.eLaneType.Drums == Eタイプ.D )
                    {
                        if( configIni.eRDPosition == ERDPosition.RCRD )
                        {
                            x = this.nチャンネルtoX座標D[ pChip.nチャンネル番号 - 0x11 ];
                        }
                        else if( configIni.eRDPosition == ERDPosition.RDRC )
                        {
                            x = this.nチャンネルtoX座標D改[ pChip.nチャンネル番号 - 0x11 ];
                        }
                    }

                    if( configIni.eRDPosition == ERDPosition.RDRC )
                    {
                        if( configIni.eLaneType.Drums == Eタイプ.A )
                        {
                            x = this.nチャンネルtoX座標改[ pChip.nチャンネル番号 - 0x11 ];
                        }
                        else if( configIni.eLaneType.Drums == Eタイプ.B )
                        {
                            x = this.nチャンネルtoX座標B改[ pChip.nチャンネル番号 - 0x11 ];
                        }
                    }

                    int y = configIni.bReverse.Drums ? ( base.nJudgeLinePosY.Drums + pChip.nバーからの距離dot.Drums ) : ( base.nJudgeLinePosY.Drums - pChip.nバーからの距離dot.Drums );
                    if( base.txチップ != null )
                    {
                        base.txチップ.vc拡大縮小倍率 = new Vector3( ( float )pChip.dbチップサイズ倍率, ( float )pChip.dbチップサイズ倍率, 1f );
                    }
                    int num9 = this.ctチップ模様アニメ.Drums.n現在の値;

                    switch( pChip.nチャンネル番号 )
                    {
                        case 0x11:
                            x = ( x + 0x10 ) - ( ( int )( ( 32.0 * pChip.dbチップサイズ倍率 ) / 2.0 ) );
                            if( this.txチップ != null )
                            {
                                this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 60 + 10, 0, 0x2e + 10, 64 ) );

                                if( pChip.bボーナスチップ )
                                    this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 60 + 10, 64, 0x2e + 10, 64 ) );
                            }
                            break;

                        case 0x12:
                            x = ( x + 0x10 ) - ( ( int )( ( 32.0 * pChip.dbチップサイズ倍率 ) / 2.0 ) );
                            if (this.txチップ != null)
                            {
                                this.txチップ.t2D描画(CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0x6a + 20, 0, 0x36 +10, 64 ) );

                                if( pChip.bボーナスチップ )
                                    this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0x6a + 20, 64, 0x36 + 10, 64 ) );
                            }
                            break;

                        case 0x13:
                            x = ( x + 0x16 ) - ( ( int )( ( 44.0 * pChip.dbチップサイズ倍率 ) / 2.0 ) );
                            if( this.txチップ != null )
                            {
                                this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0, 0, 60 + 10, 64 ) );

                                if( pChip.bボーナスチップ )
                                    this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0, 64, 60 + 10, 64 ) );
                            }
                            break;

                        case 0x14:
                            x = ( x + 0x10 ) - ( ( int )( ( 32.0 * pChip.dbチップサイズ倍率 ) / 2.0 ) );
                            if( this.txチップ != null )
                            {
                                this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 160 + 30, 0, 0x2e + 10, 64 ) );

                                if( pChip.bボーナスチップ )
                                    this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 160 + 30, 64, 0x2e + 10, 64 ) );
                            }
                            break;

                        case 0x15:
                            x = ( x + 0x10 ) - ( ( int )( ( 32.0 * pChip.dbチップサイズ倍率 ) / 2.0 ) );
                            if (this.txチップ != null)
                            {
                                this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0xce + 40, 0, 0x2e + 10, 64 ) );

                                if( pChip.bボーナスチップ )
                                    this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0xce + 40, 64, 0x2e + 10, 64 ) );
                            }
                            break;

                        case 0x16:
                            x = ( x + 19 ) - ( ( int )( ( 38.0 * pChip.dbチップサイズ倍率 ) / 2.0 ) );
                            if( this.txチップ != null )
                            {
                                this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 298 + 60, 0, 0x40 + 10, 64 ) );

                                if( pChip.bボーナスチップ )
                                    this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 298 + 60, 64, 0x40 + 10, 64 ) );
                            }
                            break;

                        case 0x17:
                            x = ( x + 0x10 ) - ( ( int )( ( 32.0 * pChip.dbチップサイズ倍率 ) / 2.0 ) );
                            if( this.txチップ != null )
                            {
                                this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0xfc + 50, 0, 0x2e + 10, 64 ) );

                                if( pChip.bボーナスチップ )
                                    this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0xfc + 50, 64, 0x2e + 10, 64 ) );
                            }
                            break;

                        case 0x18:
                            x = ( x + 13 ) - ( ( int )( ( 26.0 * pChip.dbチップサイズ倍率 ) / 2.0 ) );
                            if( this.txチップ != null )
                            {
                                switch( configIni.eHHOGraphics.Drums )
                                {
                                    case Eタイプ.A:
                                        x = ( x + 14 ) - ( ( int )( ( 26.0 * pChip.dbチップサイズ倍率 ) / 2.0 ) );
                                        this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0x200 + 100, 0, 0x26 + 10, 64 ) );
                                        if( pChip.bボーナスチップ )
                                            this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0x200 + 100, 64, 0x26 + 10, 64 ) );
                                        break;

                                    case Eタイプ.B:
                                        x = ( x + 14 ) - ( ( int )( ( 26.0 * pChip.dbチップサイズ倍率 ) / 2.0 ) );
                                        this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0x200 + 100, 0, 0x26 + 10, 64 ) );
                                        if( pChip.bボーナスチップ )
                                            this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0x200 + 100, 64, 0x26 + 10, 64 ) );
                                        break;

                                    case Eタイプ.C:
                                        x = ( x + 13 ) - ( ( int )( ( 32.0 * pChip.dbチップサイズ倍率 ) / 2.0 ) );
                                        this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 60 + 10, 0, 0x2e + 10, 64 ) );
                                        if( pChip.bボーナスチップ )
                                            this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 60 + 100, 64, 0x2e + 10, 64 ) );
                                        break;
                                }
                            }
                                break;

                        case 0x19:
                            x = ( x + 13 ) - ( ( int )( ( 26.0 * pChip.dbチップサイズ倍率 ) / 2.0 ) );
                            if( this.txチップ != null )
                            {
                                this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 0x16a + 70, 0, 0x26 + 10, 64 ) );

                                if( pChip.bボーナスチップ )
                                    this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle( 0x16a + 70, 64, 0x26 + 10, 0x40 ) );
                            }
                            break;

                        case 0x1a:
                            x = ( x + 0x13 ) - ( ( int )( ( 38.0 * pChip.dbチップサイズ倍率 ) / 2.0 ) );
                            if (this.txチップ != null)
                            {
                                this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 448 + 90, 0, 64 + 10, 64 ) );

                                if( pChip.bボーナスチップ )
                                    this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 0x20, new Rectangle( 448 + 90, 64, 64 + 10, 64 ) );
                            }
                            break;

                        case 0x1b:
                            x = ( x + 0x13 ) - ( ( int )( ( 38.0 * pChip.dbチップサイズ倍率 ) / 2.0 ) );
                            if (this.txチップ != null)
                            {
                                this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 550 + 110, 0, 0x30 + 10, 64 ) );
                                
                                if( pChip.bボーナスチップ )
                                    this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 550 + 110, 64, 0x30 + 10, 64 ) );
                            }
                            break;

                        case 0x1c:
                            x = ( x + 0x13 ) - ( ( int )( ( 38.0 * pChip.dbチップサイズ倍率 ) / 2.0 ) );
                            if( this.txチップ != null )
                            {
                                if( configIni.eLBDGraphics.Drums == Eタイプ.A )
                                {
                                    this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 550 + 110, 0, 0x30 + 10, 64 ) );
                                    if( pChip.bボーナスチップ )
                                        this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 550 + 110, 64, 0x30 + 10, 64 ) );
                                }
                                else if( configIni.eLBDGraphics.Drums == Eタイプ.B )
                                {
                                    this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 400 + 80, 0, 0x30 + 10, 64 ) );
                                    if( pChip.bボーナスチップ )
                                        this.txチップ.t2D描画( CDTXMania.app.Device, x - 5, y - 32, new Rectangle( 400 + 80, 64, 0x30 + 10, 64 ) );
                                }
                            }
                            break;
                    }
                    if( this.txチップ != null )
                    {
                        this.txチップ.vc拡大縮小倍率 = new Vector3( 1f, 1f, 1f );
                        this.txチップ.n透明度 = 0xff;
                    }
                }

				int indexSevenLanes = this.nチャンネル0Atoレーン07[ pChip.nチャンネル番号 - 0x11 ];
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
					autoPlayCondition &= !pChip.bHit && (ghostLag + pChip.n発声時刻ms <= CSound管理.rc演奏用タイマ.n現在時刻ms);
                    UsePerfectGhost = false;
                }
                if( UsePerfectGhost )
                {
                    // 従来の AUTO : バー下で判定
                    autoPlayCondition &= ( pChip.nバーからの距離dot.Drums < 0 );
                }

				if ( autoPlayCondition )
				{
					pChip.bHit = true;
					this.actLaneFlushD.Start( (Eレーン) indexSevenLanes, ( (float) CInput管理.n通常音量 ) / 127f );
					bool flag = this.bフィルイン中;
					bool flag2 = this.bフィルイン中 && this.bフィルイン区間の最後のChipである( pChip );
					//bool flag3 = flag2;
					// #31602 2013.6.24 yyagi 判定ラインの表示位置をずらしたら、チップのヒットエフェクトの表示もずらすために、nJudgeLine..を追加
					this.actChipFireD.Start( (Eレーン)indexSevenLanes, flag, flag2, flag2, nJudgeLinePosY_delta.Drums );
					this.actPad.Hit( this.nチャンネル0Atoパッド08[ pChip.nチャンネル番号 - 0x11 ] );
					this.tサウンド再生( pChip, CSound管理.rc演奏用タイマ.n前回リセットした時のシステム時刻 + pChip.n発声時刻ms + ghostLag, E楽器パート.DRUMS, dTX.nモニタを考慮した音量( E楽器パート.DRUMS ) );
					this.tチップのヒット処理(pChip.n発声時刻ms + ghostLag, pChip);
					//cInvisibleChip.StartSemiInvisible( E楽器パート.DRUMS );
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
                        if (CDTXMania.DTX.n可視チップ数.Drums > 0)
                        {
                        	// Online Stats の計算式
                            val = 100 *
                                (this.nヒット数_TargetGhost.Drums.Perfect * 17 +
                                 this.nヒット数_TargetGhost.Drums.Great * 7 +
                                 this.n最大コンボ数_TargetGhost.Drums * 3) / (20.0 * CDTXMania.DTX.n可視チップ数.Drums);
                        }
                    }
                    else
                    {
                        if( CDTXMania.ConfigIni.nSkillMode == 0 )
                        {
                            val = CScoreIni.t旧演奏型スキルを計算して返す(
                                CDTXMania.DTX.n可視チップ数.Drums,
                                this.nヒット数_TargetGhost.Drums.Perfect,
                                this.nヒット数_TargetGhost.Drums.Great,
                                this.nヒット数_TargetGhost.Drums.Good,
                                this.nヒット数_TargetGhost.Drums.Poor,
                                this.nヒット数_TargetGhost.Drums.Miss,
                                E楽器パート.DRUMS, new STAUTOPLAY());
                        }
                        else
                        {
                            val = CScoreIni.t演奏型スキルを計算して返す(
                                CDTXMania.DTX.n可視チップ数.Drums,
                                this.nヒット数_TargetGhost.Drums.Perfect,
                                this.nヒット数_TargetGhost.Drums.Great,
                                this.nヒット数_TargetGhost.Drums.Good,
                                this.nヒット数_TargetGhost.Drums.Poor,
                                this.nヒット数_TargetGhost.Drums.Miss,
                                this.n最大コンボ数_TargetGhost.Drums,
                                E楽器パート.DRUMS, new STAUTOPLAY());
                        }

                    }
                    if (val < 0) val = 0;
                    if (val > 100) val = 100;
                    this.actGraph.dbグラフ値目標_渡 = val;
                }
				return;
			}	// end of "if configIni.bDrums有効"
			if( !pChip.bHit && ( pChip.nバーからの距離dot.Drums < 0 ) )
			{
                this.tサウンド再生( pChip, CSound管理.rc演奏用タイマ.n前回リセットした時のシステム時刻 + pChip.n発声時刻ms, E楽器パート.DRUMS, dTX.nモニタを考慮した音量( E楽器パート.DRUMS ) );
				pChip.bHit = true;
			}
		}
        protected override void t進行描画・チップ・ギターベース(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip, E楽器パート inst)
		{
			base.t進行描画・チップ・ギターベース( configIni, ref dTX, ref pChip, inst,
				95, 374, 57, 412, 509, 400,
				268, 144, 76, 6,
				24, 509, 561, 400, 452, 26, 24 );
		}

        /*
		protected override void t進行描画・チップ・ギター・ウェイリング( CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip )
		{
			if ( configIni.bGuitar有効 )
			{
				//if ( configIni.bSudden.Guitar )
				//{
				//    pChip.b可視 = pChip.nバーからの距離dot.Guitar < 200;
				//}
				//if ( configIni.bHidden.Guitar && ( pChip.nバーからの距離dot.Guitar < 100 ) )
				//{
				//    pChip.b可視 = false;
				//}

				// 後日、以下の部分を何とかCStage演奏画面共通.csに移したい。
				if ( !pChip.bHit && pChip.b可視 )
				{
					int[] y_base = { 0x5f, 0x176 };		// 判定バーのY座標: ドラム画面かギター画面かで変わる値
					int offset = 0x39;					// ドラム画面かギター画面かで変わる値

					const int WailingWidth = 20;		// ウェイリングチップ画像の幅: 4種全て同じ値
					const int WailingHeight = 50;		// ウェイリングチップ画像の高さ: 4種全て同じ値
					const int baseTextureOffsetX = 268;	// テクスチャ画像中のウェイリングチップ画像の位置X: ドラム画面かギター画面かで変わる値
					const int baseTextureOffsetY = 174;	// テクスチャ画像中のウェイリングチップ画像の位置Y: ドラム画面かギター画面かで変わる値
					const int drawX = 588;				// ウェイリングチップ描画位置X座標: 4種全て異なる値

					const int numA = 25;				// 4種全て同じ値
					int y = configIni.bReverse.Guitar ? ( y_base[1] - pChip.nバーからの距離dot.Guitar ) : ( y_base[0] + pChip.nバーからの距離dot.Guitar );
					int numB = y - offset;				// 4種全て同じ定義
					int numC = 0;						// 4種全て同じ初期値
					const int numD = 355;				// ドラム画面かギター画面かで変わる値
					if ( ( numB < ( numD + numA ) ) && ( numB > -numA ) )	// 以下のロジックは4種全て同じ
					{
						int c = this.ctWailingチップ模様アニメ.n現在の値;
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
							this.txチップ.t2D描画( CDTXMania.app.Device, drawX, ( y - numA ) + numC, rect );
						}
					}
				}
				//    if ( !pChip.bHit && ( pChip.nバーからの距離dot.Guitar < 0 ) )
				//    {
				//        if ( pChip.nバーからの距離dot.Guitar < -234 )	// #25253 2011.5.29 yyagi: Don't set pChip.bHit=true for wailing at once. It need to 1sec-delay (234pix per 1sec). 
				//        {
				//            pChip.bHit = true;
				//        }
				//        if ( configIni.bAutoPlay.Guitar )
				//        {
				//            pChip.bHit = true;						// #25253 2011.5.29 yyagi: Set pChip.bHit=true if autoplay.
				//            this.actWailingBonus.Start( E楽器パート.GUITAR, this.r現在の歓声Chip.Guitar );
				//        }
				//    }
				//    return;
				//}
				//pChip.bHit = true;
			}
			base.t進行描画・チップ・ギター・ウェイリング( configIni, ref dTX, ref pChip );
		}
         */
		protected override void t進行描画・チップ・フィルイン( CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip )
		{
			if ( !pChip.bHit && ( pChip.nバーからの距離dot.Drums < 0 ) )
			{
				pChip.bHit = true;
				switch ( pChip.n整数値 )
				{
					case 0x01:	// フィルイン開始
                        this.bフィルイン終了 = true;
						if ( configIni.bフィルイン有効 )
						{
							this.bフィルイン中 = true;
						}
						break;

					case 0x02:	// フィルイン終了
                        this.bフィルイン終了 = true;
						if ( configIni.bフィルイン有効 )
						{
							this.bフィルイン中 = false;
						}
                        if (((this.actCombo.n現在のコンボ数.Drums > 0) || configIni.bドラムが全部オートプレイである) && configIni.b歓声を発声する)
                        {
                            this.actAVI.Start(bフィルイン中);
                            if (this.r現在の歓声Chip.Drums != null)
                            {
                                dTX.tチップの再生(this.r現在の歓声Chip.Drums, CSound管理.rc演奏用タイマ.nシステム時刻, (int)Eレーン.BGM, dTX.nモニタを考慮した音量(E楽器パート.UNKNOWN));
                            }
                            else
                            {
                                CDTXMania.Skin.sound歓声音.t再生する();
                                CDTXMania.Skin.sound歓声音.n位置・次に鳴るサウンド = 0;
                            }
                            //if (CDTXMania.ConfigIni.nSkillMode == 1)
                            //    this.actScore.n現在の本当のスコア.Drums += 500;
                        }
						break;
                    case 0x03:
                        this.bサビ区間 = true;
                        break;
                    case 0x04:
                        this.bサビ区間 = false;
                        break;
                    case 0x05:
                        if (configIni.bフィルイン有効)
                        {
                            this.bサビ区間 = true;
                        }
                        if (((this.actCombo.n現在のコンボ数.Drums > 0) || configIni.bドラムが全部オートプレイである) && configIni.b歓声を発声する && configIni.ボーナス演出を表示する)
                        {
                            this.actAVI.Start(true);
                            if (this.r現在の歓声Chip.Drums != null)
                            {
                                dTX.tチップの再生(this.r現在の歓声Chip.Drums, CSound管理.rc演奏用タイマ.nシステム時刻, (int)Eレーン.BGM, dTX.nモニタを考慮した音量(E楽器パート.UNKNOWN));
                            }
                            else
                            {
                                CDTXMania.Skin.sound歓声音.t再生する();
                                CDTXMania.Skin.sound歓声音.n位置・次に鳴るサウンド = 0;
                            }
                        }
                        break;
                    case 0x06:
                        if (configIni.bフィルイン有効)
                        {
                            this.bサビ区間 = false;
                        }
                        if (((this.actCombo.n現在のコンボ数.Drums > 0) || configIni.bドラムが全部オートプレイである) && configIni.b歓声を発声する && configIni.ボーナス演出を表示する)
                        {
                            this.actAVI.Start(true);
                            if (this.r現在の歓声Chip.Drums != null)
                            {
                                dTX.tチップの再生(this.r現在の歓声Chip.Drums, CSound管理.rc演奏用タイマ.nシステム時刻, (int)Eレーン.BGM, dTX.nモニタを考慮した音量(E楽器パート.UNKNOWN));
                            }
                            else
                            {
                                CDTXMania.Skin.sound歓声音.t再生する();
                                CDTXMania.Skin.sound歓声音.n位置・次に鳴るサウンド = 0;
                            }
                        }
                        break;
				}
			}
		}

        
        protected override void t進行描画・チップ・ボーナス(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip)
        {

        }
        
        public void tボーナスチップのヒット処理( CConfigIni configIni, CDTX dTX, CDTX.CChip pChip )
        {
            pChip.bHit = true;

            //if ((this.actCombo.n現在のコンボ数.Drums > 0) && configIni.b歓声を発声する )
            if( pChip.bボーナスチップ )
            {
                bボーナス = true;
                switch( pChip.nチャンネル番号 )
                {
                    //case 0x01: //LC
                    //    this.actPad.Start(0, true, pChip.nチャンネル番号);
                    //    break;

                    //case 0x02: //HH
                    //    this.actPad.Start(1, true, pChip.nチャンネル番号);
                    //    break;

                    //case 0x03: //LP
                    //    this.actPad.Start(2, true, pChip.nチャンネル番号);
                    //    break;

                    //case 0x04: //SD
                    //    this.actPad.Start(3, true, pChip.nチャンネル番号);
                    //    break;

                    //case 0x05: //HT
                    //    this.actPad.Start(4, true, pChip.nチャンネル番号);
                    //    break;

                    //case 0x06: //BD
                    //    this.actPad.Start(5, true, pChip.nチャンネル番号);
                    //    break;

                    //case 0x07: //LT
                    //    this.actPad.Start(6, true, pChip.nチャンネル番号);
                    //    break;

                    //case 0x08: //FT
                    //    this.actPad.Start(7, true, pChip.nチャンネル番号);
                    //    break;

                    //case 0x09: //CY
                    //    this.actPad.Start(8, true, pChip.nチャンネル番号);
                    //    break;

                    //case 0x0A: //RD
                    //    this.actPad.Start(9, true, pChip.nチャンネル番号);
                    //    break;

                    case 0x1A: //LC
                        this.actPad.Start( 0, true, pChip.nチャンネル番号 );
                        break;

                    case 0x11: //HH
                    case 0x18:
                        this.actPad.Start( 1, true, pChip.nチャンネル番号 );
                        break;

                    case 0x1B: //LP
                    case 0x1C:
                        this.actPad.Start( 2, true, pChip.nチャンネル番号 );
                        break;

                    case 0x12: //SD
                        this.actPad.Start( 3, true, pChip.nチャンネル番号 );
                        break;

                    case 0x14: //HT
                        this.actPad.Start( 4, true, pChip.nチャンネル番号 );
                        break;

                    case 0x13: //BD
                        this.actPad.Start( 5, true, pChip.nチャンネル番号 );
                        break;

                    case 0x15: //LT
                        this.actPad.Start( 6, true, pChip.nチャンネル番号 );
                        break;

                    case 0x17: //FT
                        this.actPad.Start( 7, true, pChip.nチャンネル番号 );
                        break;

                    case 0x16: //CY
                        this.actPad.Start( 8, true, pChip.nチャンネル番号 );
                        break;

                    case 0x19: //RD
                        this.actPad.Start( 9, true, pChip.nチャンネル番号 );
                        break;
                    default:
                        break;
                }
                if( configIni.ボーナス演出を表示する )
                {
                    this.actAVI.Start( true );
                    CDTXMania.Skin.sound歓声音.t再生する();
                    CDTXMania.Skin.sound歓声音.n位置・次に鳴るサウンド = 0;
                }
                if( CDTXMania.ConfigIni.nSkillMode == 1 && ( !CDTXMania.ConfigIni.bドラムが全部オートプレイである || CDTXMania.ConfigIni.bAutoAddGage ) )
                    this.actScore.Add( E楽器パート.DRUMS, bIsAutoPlay, 500L );
            }


        }

        /*
		protected override void t進行描画・チップ・ベース・ウェイリング( CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip )
		{
			if ( configIni.bGuitar有効 )
			{
				//if ( configIni.bSudden.Bass )
				//{
				//    pChip.b可視 = pChip.nバーからの距離dot.Bass < 200;
				//}
				//if ( configIni.bHidden.Bass && ( pChip.nバーからの距離dot.Bass < 100 ) )
				//{
				//    pChip.b可視 = false;
				//}

				//
				// 後日、以下の部分を何とかCStage演奏画面共通.csに移したい。
				//
				if ( !pChip.bHit && pChip.b可視 )
				{
					int[] y_base = { 0x5f, 0x176 };		// 判定バーのY座標: ドラム画面かギター画面かで変わる値
					int offset = 0x39;					// ドラム画面かギター画面かで変わる値

					const int WailingWidth = 20;		// ウェイリングチップ画像の幅: 4種全て同じ値
					const int WailingHeight = 50;		// ウェイリングチップ画像の高さ: 4種全て同じ値
					const int baseTextureOffsetX = 268;	// テクスチャ画像中のウェイリングチップ画像の位置X: ドラム画面かギター画面かで変わる値
					const int baseTextureOffsetY = 174;	// テクスチャ画像中のウェイリングチップ画像の位置Y: ドラム画面かギター画面かで変わる値
					const int drawX = 479;				// ウェイリングチップ描画位置X座標: 4種全て異なる値

					const int numA = 25;				// 4種全て同じ値
					int y = configIni.bReverse.Bass ? ( y_base[ 1 ] - pChip.nバーからの距離dot.Bass ) : ( y_base[ 0 ] + pChip.nバーからの距離dot.Bass );
					int numB = y - offset;				// 4種全て同じ定義
					int numC = 0;						// 4種全て同じ初期値
					const int numD = 355;				// ドラム画面かギター画面かで変わる値
					if ( ( numB < ( numD + numA ) ) && ( numB > -numA ) )	// 以下のロジックは4種全て同じ
					{
						int c = this.ctWailingチップ模様アニメ.n現在の値;
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
							this.txチップ.t2D描画( CDTXMania.app.Device, drawX, ( y - numA ) + numC, rect );
						}
					}
				}
				//    if ( !pChip.bHit && ( pChip.nバーからの距離dot.Bass < 0 ) )
				//    {
				//        if ( pChip.nバーからの距離dot.Bass < -234 )	// #25253 2011.5.29 yyagi: Don't set pChip.bHit=true for wailing at once. It need to 1sec-delay (234pix per 1sec).
				//        {
				//            pChip.bHit = true;
				//        }
				//        if ( configIni.bAutoPlay.Bass )
				//        {
				//            this.actWailingBonus.Start( E楽器パート.BASS, this.r現在の歓声Chip.Bass );
				//            pChip.bHit = true;						// #25253 2011.5.29 yyagi: Set pChip.bHit=true if autoplay.
				//        }
				//    }
				//    return;
				//}
				//pChip.bHit = true;
			}
				base.t進行描画・チップ・ベース・ウェイリング( configIni, ref dTX, ref pChip);
		}
         */

        protected override void t進行描画・チップ・空打ち音設定・ドラム(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip)
        {
            if (!pChip.bHit && (pChip.nバーからの距離dot.Drums < 0))
            {
                try
                {
                    pChip.bHit = true;
                    this.r現在の空うちドラムChip[(int)this.eチャンネルtoパッド[pChip.nチャンネル番号 - 0xb1]] = pChip;
                    pChip.nチャンネル番号 = ((pChip.nチャンネル番号 < 0xbc) || (pChip.nチャンネル番号 > 190)) ? ((pChip.nチャンネル番号 - 0xb1) + 0x11) : ((pChip.nチャンネル番号 - 0xb3) + 0x11);
                }
                catch
                {
                    return;
                }
            }

        }
		protected override void t進行描画・チップ・小節線( CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip )
		{
			int n小節番号plus1 = pChip.n発声位置 / 384;
			if ( !pChip.bHit && ( pChip.nバーからの距離dot.Drums < 0 ) )
			{
				pChip.bHit = true;
				this.actPlayInfo.n小節番号 = n小節番号plus1 - 1;
                if ( configIni.bWave再生位置自動調整機能有効 && bIsDirectSound )
				{
					dTX.tWave再生位置自動補正();
				}
			}
				if ( configIni.b演奏情報を表示する && ( configIni.nLaneDisp.Drums == 0 || configIni.nLaneDisp.Drums == 1 ) )
                {
                        int n小節番号 = n小節番号plus1 - 1;
                        CDTXMania.act文字コンソール.tPrint(858, configIni.bReverse.Drums ? ((base.nJudgeLinePosY.Drums + pChip.nバーからの距離dot.Drums) - 0x11) : ((base.nJudgeLinePosY.Drums - pChip.nバーからの距離dot.Drums) - 0x11), C文字コンソール.Eフォント種別.白, n小節番号.ToString());
				}
                if (((configIni.nLaneDisp.Drums == 0 || configIni.nLaneDisp.Drums == 1) && pChip.b可視) && (this.txチップ != null))
				{
                    this.txチップ.t2D描画(CDTXMania.app.Device, 295, configIni.bReverse.Drums ? ((base.nJudgeLinePosY.Drums + pChip.nバーからの距離dot.Drums) - 1) : ((base.nJudgeLinePosY.Drums - pChip.nバーからの距離dot.Drums) - 1), new Rectangle(0, 769, 0x22f, 2));
				}
              
            /*
			if ( ( pChip.b可視 && configIni.bGuitar有効 ) && ( configIni.eDark != Eダークモード.FULL ) )
			{
				int y = configIni.bReverse.Guitar ? ( ( 0x176 - pChip.nバーからの距離dot.Guitar ) - 1 ) : ( ( 0x5f + pChip.nバーからの距離dot.Guitar ) - 1 );
				if ( ( dTX.bチップがある.Guitar && ( y > 0x39 ) ) && ( ( y < 0x19c ) && ( this.txチップ != null ) ) )
				{
					this.txチップ.t2D描画( CDTXMania.app.Device, 374, y, new Rectangle( 0, 450, 0x4e, 1 ) );
				}
				y = configIni.bReverse.Bass ? ( ( 0x176 - pChip.nバーからの距離dot.Bass ) - 1 ) : ( ( 0x5f + pChip.nバーからの距離dot.Bass ) - 1 );
				if ( ( dTX.bチップがある.Bass && ( y > 0x39 ) ) && ( ( y < 0x19c ) && ( this.txチップ != null ) ) )
				{
					this.txチップ.t2D描画( CDTXMania.app.Device, 398, y, new Rectangle( 0, 450, 0x4e, 1 ) );
				}
			}
             */
		}              //移植完了。
    #endregion
	}

}
