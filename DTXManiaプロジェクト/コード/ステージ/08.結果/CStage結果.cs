using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using FDK;
using SlimDX.Direct3D9;
using DirectShowLib;

namespace DTXMania
{
	internal class CStage結果 : CStage
	{
		// プロパティ

		public STDGBVALUE<bool> b新記録スキル;
		public STDGBVALUE<bool> b新記録スコア;
        public STDGBVALUE<bool> b新記録ランク;
		public STDGBVALUE<float> fPerfect率;
		public STDGBVALUE<float> fGreat率;
		public STDGBVALUE<float> fGood率;
		public STDGBVALUE<float> fPoor率;
        public STDGBVALUE<float> fMiss率;
        public STDGBVALUE<bool> bオート;        // #23596 10.11.16 add ikanick
                                                //        10.11.17 change (int to bool) ikanick
		public STDGBVALUE<int> nランク値;
		public STDGBVALUE<int> n演奏回数;
		public int n総合ランク値;
		public CDTX.CChip[] r空うちドラムチップ;
		public STDGBVALUE<CScoreIni.C演奏記録> st演奏記録;


		// コンストラクタ

		public CStage結果()
		{
			this.st演奏記録.Drums = new CScoreIni.C演奏記録();
			this.st演奏記録.Guitar = new CScoreIni.C演奏記録();
			this.st演奏記録.Bass = new CScoreIni.C演奏記録();
			this.r空うちドラムチップ = new CDTX.CChip[ 10 ];
			this.n総合ランク値 = -1;
            this.nチャンネル0Atoレーン07 = new int[] { 1, 2, 3, 4, 5, 7, 6, 1, 8, 0, 9 };
			base.eステージID = CStage.Eステージ.結果;
			base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
			base.b活性化してない = true;
			base.list子Activities.Add( this.actResultImage = new CActResultImage() );
			base.list子Activities.Add( this.actParameterPanel = new CActResultParameterPanel() );
			base.list子Activities.Add( this.actRank = new CActResultRank() );
			base.list子Activities.Add( this.actSongBar = new CActResultSongBar() );
			base.list子Activities.Add( this.actFI = new CActFIFOWhite() );
			base.list子Activities.Add( this.actFO = new CActFIFOBlack() );
		}

		
		// CStage 実装

		public override void On活性化()
		{
			Trace.TraceInformation( "結果ステージを活性化します。" );
			Trace.Indent();
			try
			{
				#region [ 初期化 ]
				//---------------------
				this.eフェードアウト完了時の戻り値 = E戻り値.継続;
				this.bアニメが完了 = false;
				this.bIsCheckedWhetherResultScreenShouldSaveOrNot = false;				// #24609 2011.3.14 yyagi
				this.n最後に再生したHHのWAV番号 = -1;
				this.n最後に再生したHHのチャンネル番号 = 0;
				for( int i = 0; i < 3; i++ )
				{
					this.b新記録スキル[ i ] = false;
                    this.b新記録スコア[ i ] = false;
                    this.b新記録ランク[ i ] = false;
				}
				//---------------------
				#endregion

                #region [ 結果の計算 ]
                //---------------------
                for (int i = 0; i < 3; i++)
                {
                    this.nランク値[i] = -1;
                    this.fPerfect率[i] = this.fGreat率[i] = this.fGood率[i] = this.fPoor率[i] = this.fMiss率[i] = 0.0f;	// #28500 2011.5.24 yyagi
                    if ((((i != 0) || (CDTXMania.DTX.bチップがある.Drums && !CDTXMania.ConfigIni.bギタレボモード)) &&
                        ((i != 1) || CDTXMania.DTX.bチップがある.Guitar)) &&
                        ((i != 2) || CDTXMania.DTX.bチップがある.Bass))
                    {
                        CScoreIni.C演奏記録 part = this.st演奏記録[i];
                        bool bIsAutoPlay = true;
                        switch (i)
                        {
                            case 0:
                                bIsAutoPlay = CDTXMania.ConfigIni.bドラムが全部オートプレイである;
                                break;

                            case 1:
                                bIsAutoPlay = CDTXMania.ConfigIni.bギターが全部オートプレイである;
                                break;

                            case 2:
                                bIsAutoPlay = CDTXMania.ConfigIni.bベースが全部オートプレイである;
                                break;
                        }
                        this.fPerfect率[i] = bIsAutoPlay ? 0f : ((100f * part.nPerfect数) / ((float)part.n全チップ数));
                        this.fGreat率[i] = bIsAutoPlay ? 0f : ((100f * part.nGreat数) / ((float)part.n全チップ数));
                        this.fGood率[i] = bIsAutoPlay ? 0f : ((100f * part.nGood数) / ((float)part.n全チップ数));
                        this.fPoor率[i] = bIsAutoPlay ? 0f : ((100f * part.nPoor数) / ((float)part.n全チップ数));
                        this.fMiss率[i] = bIsAutoPlay ? 0f : ((100f * part.nMiss数) / ((float)part.n全チップ数));
                        this.bオート[i] = bIsAutoPlay;	// #23596 10.11.16 add ikanick そのパートがオートなら1
                        //        10.11.17 change (int to bool) ikanick
                        //04032016: Change first condition check to 1, XG mode is 1, not 0. KSM
                        if (CDTXMania.ConfigIni.nSkillMode == 1)//was 0, which is incorrect
                        {
                            this.nランク値[i] = CScoreIni.tランク値を計算して返す(part);
                        }
                        else if (CDTXMania.ConfigIni.nSkillMode == 0)
                        {
                            this.nランク値[i] = CScoreIni.t旧ランク値を計算して返す(part);
                        }
                    }
                }
                this.n総合ランク値 = CScoreIni.t総合ランク値を計算して返す(this.st演奏記録.Drums, this.st演奏記録.Guitar, this.st演奏記録.Bass);
				//---------------------
				#endregion

				#region [ .score.ini の作成と出力 ]
				//---------------------
				string str = CDTXMania.DTX.strファイル名の絶対パス + ".score.ini";
				CScoreIni ini = new CScoreIni( str );

				bool[] b今までにフルコンボしたことがある = new bool[] { false, false, false };
                if (!CDTXMania.ConfigIni.bドラムが全部オートプレイである || !CDTXMania.ConfigIni.bギターが全部オートプレイである || !CDTXMania.ConfigIni.bベースが全部オートプレイである)
                {
                    for (int i = 0; i < 3; i++)
                    {

                        // フルコンボチェックならびに新記録ランクチェックは、ini.Record[] が、スコアチェックや演奏型スキルチェックの IF 内で書き直されてしまうよりも前に行う。(2010.9.10)

                        b今までにフルコンボしたことがある[i] = ini.stセクション[i * 2].bフルコンボである | ini.stセクション[i * 2 + 1].bフルコンボである;

                        #region [deleted by #24459]
                        //		if( this.nランク値[ i ] <= CScoreIni.tランク値を計算して返す( ini.stセクション[ ( i * 2 ) + 1 ] ) )
                        //		{
                        //			this.b新記録ランク[ i ] = true;
                        //		}
                        #endregion
                        // #24459 上記の条件だと[HiSkill.***]でのランクしかチェックしていないので、BestRankと比較するよう変更。
                        if (this.nランク値[i] >= 0 && ini.stファイル.BestRank[i] > this.nランク値[i])		// #24459 2011.3.1 yyagi update BestRank
                        {
                            this.b新記録ランク[i] = true;
                            ini.stファイル.BestRank[i] = this.nランク値[i];
                        }

			    		// 新記録スコアチェック
				    	if( this.st演奏記録[ i ].nスコア > ini.stセクション[ i * 2 ].nスコア )
					    {
					        this.b新記録スコア[ i ] = true;
					        ini.stセクション[ i * 2 ] = this.st演奏記録[ i ];
                            this.SaveGhost( i * 2 ); // #35411 chnmr0 add
					    }

                        // 新記録スキルチェック
                        if ( this.st演奏記録[ i ].db演奏型スキル値 > ini.stセクション[ ( i * 2 ) + 1 ].db演奏型スキル値 )
                        {
                            this.b新記録スキル[ i ] = true;
                            ini.stセクション[ ( i * 2 ) + 1 ] = this.st演奏記録[ i ];
                            this.SaveGhost( ( i * 2 ) + 1 ); // #35411 chnmr0 add
                        }

			    		// ラストプレイ #23595 2011.1.9 ikanick
                        // オートじゃなければプレイ結果を書き込む
                        if( this.bオート[ i ] == false ) {
                            ini.stセクション[ i + 6 ] = this.st演奏記録[ i ];
                            this.SaveGhost(i + 6); // #35411 chnmr0 add
                        }

                        // #23596 10.11.16 add ikanick オートじゃないならクリア回数を1増やす
                        //        11.02.05 bオート to t更新条件を取得する use      ikanick
                        bool[] b更新が必要か否か = new bool[3];
                        CScoreIni.t更新条件を取得する(out b更新が必要か否か[0], out b更新が必要か否か[1], out b更新が必要か否か[2]);

                        if (b更新が必要か否か[i])
                        {
                            switch (i)
                            {
                                case 0:
                                    ini.stファイル.ClearCountDrums++;
                                    break;
                                case 1:
                                    ini.stファイル.ClearCountGuitar++;
                                    break;
                                case 2:
                                    ini.stファイル.ClearCountBass++;
                                    break;
                                default:
                                    throw new Exception("クリア回数増加のk(0-2)が範囲外です。");
                            }
                        }

                        //---------------------------------------------------------------------/
                    }
                    if( CDTXMania.ConfigIni.bScoreIniを出力する )
                    {
                        ini.t書き出し(str);
                    }
                }
				//---------------------
				#endregion

				#region [ リザルト画面への演奏回数の更新 #24281 2011.1.30 yyagi]
				this.n演奏回数.Drums = ini.stファイル.PlayCountDrums;
				this.n演奏回数.Guitar = ini.stファイル.PlayCountGuitar;
				this.n演奏回数.Bass = ini.stファイル.PlayCountBass;
				#endregion
				#region [ 選曲画面の譜面情報の更新 ]
				//---------------------
				if( !CDTXMania.bコンパクトモード )
				{
					Cスコア cスコア = CDTXMania.stage選曲.r確定されたスコア;
					bool[] b更新が必要か否か = new bool[ 3 ];
					CScoreIni.t更新条件を取得する( out b更新が必要か否か[ 0 ], out b更新が必要か否か[ 1 ], out b更新が必要か否か[ 2 ] );
					for( int m = 0; m < 3; m++ )
					{
						if( b更新が必要か否か[ m ] )
						{
							// FullCombo した記録を FullCombo なしで超えた場合、FullCombo マークが消えてしまう。
							// → FullCombo は、最新記録と関係なく、一度達成したらずっとつくようにする。(2010.9.11)
							cスコア.譜面情報.フルコンボ[ m ] = this.st演奏記録[ m ].bフルコンボである | b今までにフルコンボしたことがある[ m ];

							if( this.b新記録スキル[ m ] )
							{
								cスコア.譜面情報.最大スキル[ m ] = this.st演奏記録[ m ].db演奏型スキル値;
                            }

                            if (this.b新記録ランク[ m ])
                            {
                                cスコア.譜面情報.最大ランク[ m ] = this.nランク値[ m ];
                            }
						}
					}
				}
				//---------------------
				#endregion

				base.On活性化();
			}
			finally
			{
				Trace.TraceInformation( "結果ステージの活性化を完了しました。" );
				Trace.Unindent();
			}
		}
        //fork
        // #35411 chnmr0 add
        private void SaveGhost(int sectionIndex)
        {
            //return; //2015.12.31 kairera0467 以下封印

            STDGBVALUE<bool> saveCond = new STDGBVALUE<bool>();
            saveCond.Drums = true;
            saveCond.Guitar = true;
            saveCond.Bass = true;
            
            foreach( CDTX.CChip chip in CDTXMania.DTX.listChip )
            {
                if( chip.bIsAutoPlayed )
                {
					if (chip.nチャンネル番号 != 0x28 && chip.nチャンネル番号 != 0xA8) // Guitar/Bass Wailing は OK
					{
						saveCond[(int)(chip.e楽器パート)] = false;
					}
                }
            }
            for(int instIndex = 0; instIndex < 3; ++instIndex)
            {
                saveCond[instIndex] &= CDTXMania.listAutoGhostLag.Drums == null;
            }

            string directory = CDTXMania.DTX.strフォルダ名;
            string filename = CDTXMania.DTX.strファイル名 + ".";
            E楽器パート inst = E楽器パート.UNKNOWN;

            if ( sectionIndex == 0 )
            {
                filename += "hiscore.dr.ghost";
                inst = E楽器パート.DRUMS;
            }
            else if (sectionIndex == 1 )
            {
                filename += "hiskill.dr.ghost";
                inst = E楽器パート.DRUMS;
            }
            if (sectionIndex == 2 )
            {
                filename += "hiscore.gt.ghost";
                inst = E楽器パート.GUITAR;
            }
            else if (sectionIndex == 3 )
            {
                filename += "hiskill.gt.ghost";
                inst = E楽器パート.GUITAR;
            }
            if (sectionIndex == 4 )
            {
                filename += "hiscore.bs.ghost";
                inst = E楽器パート.BASS;
            }
            else if (sectionIndex == 5)
            {
                filename += "hiskill.bs.ghost";
                inst = E楽器パート.BASS;
            }
            else if (sectionIndex == 6)
            {
                filename += "lastplay.dr.ghost";
                inst = E楽器パート.DRUMS;
            }
            else if (sectionIndex == 7 )
            {
                filename += "lastplay.gt.ghost";
                inst = E楽器パート.GUITAR;
            }
            else if (sectionIndex == 8)
            {
                filename += "lastplay.bs.ghost";
                inst = E楽器パート.BASS;
            }

            if (inst == E楽器パート.UNKNOWN)
            {
                return;
            }

            int cnt = 0;
            foreach (DTXMania.CDTX.CChip chip in CDTXMania.DTX.listChip)
            {
                if (chip.e楽器パート == inst)
                {
                    ++cnt;
                }
            }

            if( saveCond[(int)inst] )
            //if(false)
            {
                using (FileStream fs = new FileStream(directory + "\\" + filename, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write((Int32)cnt);
                        foreach (DTXMania.CDTX.CChip chip in CDTXMania.DTX.listChip)
                        {
                            if (chip.e楽器パート == inst)
                            {
                            	// -128 ms から 127 ms までのラグしか保存しない
                            	// その範囲を超えているラグはクランプ
								// ラグデータの 上位８ビットでそのチップの前でギター空打ちBADがあったことを示す
								int lag = chip.nLag;
								if (lag < -128)
								{
									lag = -128;
								}
								if (lag > 127)
								{
									lag = 127;
								}
								byte lower = (byte)(lag + 128);
								int upper = chip.nCurrentComboForGhost == 0 ? 1 : 0;
								bw.Write((short)( (upper<<8) | lower));
                            }
                        }
                    }
                }

                //Ver.K追加 演奏結果の記録
                CScoreIni.C演奏記録 cScoreData;
                cScoreData = this.st演奏記録[ (int)inst ];
                using (FileStream fs = new FileStream(directory + "\\" + filename + ".score", FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine( "Score=" + cScoreData.nスコア );
                        sw.WriteLine( "PlaySkill=" + cScoreData.db演奏型スキル値 );
                        sw.WriteLine( "Skill=" + cScoreData.dbゲーム型スキル値 );
                        sw.WriteLine( "Perfect=" + cScoreData.nPerfect数・Auto含まない );
                        sw.WriteLine( "Great=" + cScoreData.nGreat数・Auto含まない );
                        sw.WriteLine( "Good=" + cScoreData.nGood数・Auto含まない );
                        sw.WriteLine( "Poor=" + cScoreData.nPoor数・Auto含まない );
                        sw.WriteLine( "Miss=" + cScoreData.nMiss数・Auto含まない );
                        sw.WriteLine( "MaxCombo=" + cScoreData.n最大コンボ数 );
                    }
                }
            }
        }
		public override void On非活性化()
		{
			if( this.rResultSound != null )
			{
				CDTXMania.Sound管理.tサウンドを破棄する( this.rResultSound );
				this.rResultSound = null;
			}
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
                this.ds背景動画 = CDTXMania.t失敗してもスキップ可能なDirectShowを生成する(CSkin.Path(@"Graphics\8_background.mp4"), CDTXMania.app.WindowHandle, true);
				this.tx背景 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\8_background.jpg" ) );
                switch (CDTXMania.stage結果.n総合ランク値)
                {
                    case 0:
                        if (File.Exists(CSkin.Path(@"Graphics\8_background rankSS.png")))
                        {
                            this.tx背景 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\8_background rankSS.png"));
                        }
                        break;
                    case 1:
                        if (File.Exists(CSkin.Path(@"Graphics\8_background rankS.png")))
                        {
                            this.tx背景 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\8_background rankS.png"));
                        }
                        break;
                    case 2:
                        if (File.Exists(CSkin.Path(@"Graphics\8_background rankA.png")))
                        {
                            this.tx背景 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\8_background rankA.png"));
                        }
                        break;
                    case 3:
                        if (File.Exists(CSkin.Path(@"Graphics\8_background rankB.png")))
                        {
                            this.tx背景 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\8_background rankB.png"));
                        }
                        break;
                    case 4:
                        if (File.Exists(CSkin.Path(@"Graphics\8_background rankC.png")))
                        {
                            this.tx背景 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\8_background rankC.png"));
                        }
                        break;
                    case 5:
                        if (File.Exists(CSkin.Path(@"Graphics\8_background rankD.png")))
                        {
                            this.tx背景 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\8_background rankD.png"));
                        }
                        break;
                    case 6:
                    case 99:
                        if (File.Exists(CSkin.Path(@"Graphics\8_background rankE.png")))
                        {
                            this.tx背景 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\8_background rankE.png"));
                        }
                        break;
                }
				this.tx上部パネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\8_header panel.png" ), true );
				this.tx下部パネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\8_footer panel.png" ), true );
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				if( this.ct登場用 != null )
				{
					this.ct登場用 = null;
				}
                CDTXMania.t安全にDisposeする( ref this.ds背景動画 );
				CDTXMania.tテクスチャの解放( ref this.tx背景 );
				CDTXMania.tテクスチャの解放( ref this.tx上部パネル );
				CDTXMania.tテクスチャの解放( ref this.tx下部パネル );
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
				int num;
				if( base.b初めての進行描画 )
				{
					this.ct登場用 = new CCounter( 0, 100, 5, CDTXMania.Timer );

                    //KSM 21052016: Added a check on game mode selected and play the sound clip for the correct game mode accordingly
                    //May still not work correctly with Guitar/Bass mode...
                    for (int i = 0; i < 3;++i )
                    {
                        if ((((i != 0) || (CDTXMania.DTX.bチップがある.Drums && !CDTXMania.ConfigIni.bギタレボモード)) &&
                        ((i != 1) || CDTXMania.DTX.bチップがある.Guitar)) &&
                        ((i != 2) || CDTXMania.DTX.bチップがある.Bass))
                        {
                            // Added conditions for excellent/fullcombo audio playback - limyz / 210516
                            if (fPerfect率[i] == 100.0 && bオート[i].Equals(false)) CDTXMania.Skin.soundExcellent.t再生する();
                            else if (fPoor率[i] == 0.0 && fMiss率[i] == 0.0 && bオート[i].Equals(false)) CDTXMania.Skin.soundフルコンボ音.t再生する();
                            else CDTXMania.Skin.soundステージクリア音.t再生する();
                        }
                    }

                    this.actFI.tフェードイン開始(false);
					base.eフェーズID = CStage.Eフェーズ.共通_フェードイン;
					base.b初めての進行描画 = false;
				}
                if( this.ds背景動画 != null )
                {
                    this.ds背景動画.t再生開始();
                    this.ds背景動画.MediaSeeking.GetPositions(out this.lDshowPosition, out this.lStopPosition);
                    this.ds背景動画.bループ再生 = true;
                    
                    if (this.lDshowPosition == this.lStopPosition)
                    {
                        this.ds背景動画.MediaSeeking.SetPositions(
                        DsLong.FromInt64((long)(0)),
                        AMSeekingSeekingFlags.AbsolutePositioning,
                        0,
                        AMSeekingSeekingFlags.NoPositioning);
                    }
                    
                    this.ds背景動画.t現時点における最新のスナップイメージをTextureに転写する( this.tx背景 );
                }
				this.bアニメが完了 = true;
				if( this.ct登場用.b進行中 )
				{
					this.ct登場用.t進行();
					if( this.ct登場用.b終了値に達した )
					{
						this.ct登場用.t停止();
					}
					else
					{
						this.bアニメが完了 = false;
					}
				}

				// 描画

				if( this.tx背景 != null )
				{
                    if( this.ds背景動画 != null && this.ds背景動画.b上下反転 )
                    {
                        this.tx背景.t2D上下反転描画( CDTXMania.app.Device, 0, 0 );
                    }
                    else
                    {
					    this.tx背景.t2D描画( CDTXMania.app.Device, 0, 0 );
                    }
				}
				if( this.ct登場用.b進行中 && ( this.tx上部パネル != null ) )
				{
					double num2 = ( (double) this.ct登場用.n現在の値 ) / 100.0;
					double num3 = Math.Sin( Math.PI / 2 * num2 );
					num = ( (int) ( this.tx上部パネル.sz画像サイズ.Height * num3 ) ) - this.tx上部パネル.sz画像サイズ.Height;
				}
				else
				{
					num = 0;
				}
				if( this.tx上部パネル != null )
				{
					this.tx上部パネル.t2D描画( CDTXMania.app.Device, 0, num );
				}
				if( this.tx下部パネル != null )
				{
					this.tx下部パネル.t2D描画( CDTXMania.app.Device, 0, 720 - this.tx下部パネル.sz画像サイズ.Height );
				}
                if( this.actResultImage.On進行描画() == 0 )
				{
					this.bアニメが完了 = false;
				}
				if ( this.actParameterPanel.On進行描画() == 0 )
				{
					this.bアニメが完了 = false;
				}
                if (this.actRank.On進行描画() == 0)
                {
                    this.bアニメが完了 = false;
                }
				if( base.eフェーズID == CStage.Eフェーズ.共通_フェードイン )
				{
					if( this.actFI.On進行描画() != 0 )
					{
						base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
					}
				}
				else if( ( base.eフェーズID == CStage.Eフェーズ.共通_フェードアウト ) )			//&& ( this.actFO.On進行描画() != 0 ) )
				{
					return (int) this.eフェードアウト完了時の戻り値;
				}
				#region [ #24609 2011.3.14 yyagi ランク更新or演奏型スキル更新時、リザルト画像をpngで保存する ]
				if ( this.bアニメが完了 == true && this.bIsCheckedWhetherResultScreenShouldSaveOrNot == false	// #24609 2011.3.14 yyagi; to save result screen in case BestRank or HiSkill.
					&& CDTXMania.ConfigIni.bScoreIniを出力する
					&& CDTXMania.ConfigIni.bIsAutoResultCapture)												// #25399 2011.6.9 yyagi
				{
					CheckAndSaveResultScreen(true);
					this.bIsCheckedWhetherResultScreenShouldSaveOrNot = true;
				}
				#endregion

				// キー入力

				if( CDTXMania.act現在入力を占有中のプラグイン == null )
				{
					if( CDTXMania.ConfigIni.bドラム打音を発声する && CDTXMania.ConfigIni.bDrums有効 )
					{
						for( int i = 0; i < 11; i++ )
						{
							List<STInputEvent> events = CDTXMania.Pad.GetEvents( E楽器パート.DRUMS, (Eパッド) i );
							if( ( events != null ) && ( events.Count > 0 ) )
							{
								foreach( STInputEvent event2 in events )
								{
									if( !event2.b押された )
									{
										continue;
									}
									CDTX.CChip rChip = this.r空うちドラムチップ[ i ];
									if( rChip == null )
									{
										switch( ( (Eパッド) i ) )
										{
											case Eパッド.HH:
												rChip = this.r空うちドラムチップ[ 7 ];
												if( rChip == null )
												{
													rChip = this.r空うちドラムチップ[ 9 ];
												}
												break;

											case Eパッド.FT:
												rChip = this.r空うちドラムチップ[ 4 ];
												break;

											case Eパッド.CY:
												rChip = this.r空うちドラムチップ[ 8 ];
												break;

											case Eパッド.HHO:
												rChip = this.r空うちドラムチップ[ 0 ];
												if( rChip == null )
												{
													rChip = this.r空うちドラムチップ[ 9 ];
												}
												break;

											case Eパッド.RD:
												rChip = this.r空うちドラムチップ[ 6 ];
												break;

											case Eパッド.LC:
												rChip = this.r空うちドラムチップ[ 0 ];
												if( rChip == null )
												{
													rChip = this.r空うちドラムチップ[ 7 ];
												}
												break;
										}
									}
									if( ( ( rChip != null ) && ( rChip.nチャンネル番号 >= 0x11 ) ) && ( rChip.nチャンネル番号 <= 0x1b ) )
									{
										int nLane = this.nチャンネル0Atoレーン07[ rChip.nチャンネル番号 - 0x11 ];
										if( ( nLane == 1 ) && ( ( rChip.nチャンネル番号 == 0x11 ) || ( ( rChip.nチャンネル番号 == 0x18 ) && ( this.n最後に再生したHHのチャンネル番号 != 0x18 ) ) ) )
										{
											CDTXMania.DTX.tWavの再生停止( this.n最後に再生したHHのWAV番号 );
											this.n最後に再生したHHのWAV番号 = rChip.n整数値・内部番号;
											this.n最後に再生したHHのチャンネル番号 = rChip.nチャンネル番号;
										}
										CDTXMania.DTX.tチップの再生( rChip, CDTXMania.Timer.nシステム時刻, nLane, CDTXMania.ConfigIni.n手動再生音量, CDTXMania.ConfigIni.b演奏音を強調する.Drums );
									}
								}
							}
						}
					}
					if( ( ( CDTXMania.Pad.b押されたDGB( Eパッド.CY ) || CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.RD ) ) || ( CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.LC ) || CDTXMania.Input管理.Keyboard.bキーが押された( (int)SlimDX.DirectInput.Key.Return ) ) ) && !this.bアニメが完了 )
					{
						this.actFI.tフェードイン完了();					// #25406 2011.6.9 yyagi
						this.actResultImage.tアニメを完了させる();
						this.actParameterPanel.tアニメを完了させる();
						this.actRank.tアニメを完了させる();
						this.ct登場用.t停止();
					}
					#region [ #24609 2011.4.7 yyagi リザルト画面で[F12]を押下すると、リザルト画像をpngで保存する機能は、CDTXManiaに移管。 ]
//					if ( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.F12 ) &&
//						CDTXMania.ConfigIni.bScoreIniを出力する )
//					{
//						CheckAndSaveResultScreen(false);
//						this.bIsCheckedWhetherResultScreenShouldSaveOrNot = true;
//					}
					#endregion
					if ( base.eフェーズID == CStage.Eフェーズ.共通_通常状態 )
					{
						if ( CDTXMania.Input管理.Keyboard.bキーが押された( (int)SlimDX.DirectInput.Key.Escape ) )
						{
							CDTXMania.Skin.sound取消音.t再生する();
							this.actFO.tフェードアウト開始();
							base.eフェーズID = CStage.Eフェーズ.共通_フェードアウト;
							this.eフェードアウト完了時の戻り値 = E戻り値.完了;
						}
						if ( ( ( CDTXMania.Pad.b押されたDGB( Eパッド.CY ) || CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.RD ) ) || ( CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.LC ) || CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.Return ) ) ) && this.bアニメが完了 )
						{
							CDTXMania.Skin.sound取消音.t再生する();
							base.eフェーズID = CStage.Eフェーズ.共通_フェードアウト;
							this.eフェードアウト完了時の戻り値 = E戻り値.完了;
						}
					}
				}
			}
			return 0;
		}

		public enum E戻り値 : int
		{
			継続,
			完了
		}


		// その他

		#region [ private ]
		//-----------------
		private CCounter ct登場用;
		private E戻り値 eフェードアウト完了時の戻り値;
		private CActFIFOWhite actFI;
		private CActFIFOBlack actFO;
		private CActResultParameterPanel actParameterPanel;
		private CActResultRank actRank;
		private CActResultImage actResultImage;
		private CActResultSongBar actSongBar;
		private bool bアニメが完了;
		private bool bIsCheckedWhetherResultScreenShouldSaveOrNot;				// #24509 2011.3.14 yyagi
		private readonly int[] nチャンネル0Atoレーン07;
		private int n最後に再生したHHのWAV番号;
		private int n最後に再生したHHのチャンネル番号;
		private CSound rResultSound;
		private CTexture tx下部パネル;
		private CTexture tx上部パネル;
		private CTexture tx背景;

        private CDirectShow ds背景動画;
        private long lDshowPosition;
        private long lStopPosition;

		#region [ #24609 リザルト画像をpngで保存する ]		// #24609 2011.3.14 yyagi; to save result screen in case BestRank or HiSkill.
		/// <summary>
		/// リザルト画像のキャプチャと保存。
		/// 自動保存モード時は、ランク更新or演奏型スキル更新時に自動保存。
		/// 手動保存モード時は、ランクに依らず保存。
		/// </summary>
		/// <param name="bIsAutoSave">true=自動保存モード, false=手動保存モード</param>
		private void CheckAndSaveResultScreen(bool bIsAutoSave)
		{
			string path = Path.GetDirectoryName( CDTXMania.DTX.strファイル名の絶対パス );
			string datetime = DateTime.Now.ToString( "yyyyMMddHHmmss" );
			if ( bIsAutoSave )
			{
				// リザルト画像を自動保存するときは、dtxファイル名.yyMMddHHmmss_DRUMS_SS.png という形式で保存。
				for ( int i = 0; i < 3; i++ )
				{
					if ( this.b新記録ランク[ i ] == true || this.b新記録スキル[ i ] == true )
					{
						string strPart = ( (E楽器パート) ( i ) ).ToString();
						string strRank = ( (CScoreIni.ERANK) ( this.nランク値[ i ] ) ).ToString();
						string strFullPath = CDTXMania.DTX.strファイル名の絶対パス + "." + datetime + "_" + strPart + "_" + strRank + ".png";
						//Surface.ToFile( pSurface, strFullPath, ImageFileFormat.Png );
						CDTXMania.app.SaveResultScreen( strFullPath );
					}
				}
			}
			#region [ #24609 2011.4.11 yyagi; リザルトの手動保存ロジックは、CDTXManiaに移管した。]
//			else
//			{
//				// リザルト画像を手動保存するときは、dtxファイル名.yyMMddHHmmss_SS.png という形式で保存。(楽器名無し)
//				string strRank = ( (CScoreIni.ERANK) ( CDTXMania.stage結果.n総合ランク値 ) ).ToString();
//				string strSavePath = CDTXMania.strEXEのあるフォルダ + "\\" + "Capture_img";
//				if ( !Directory.Exists( strSavePath ) )
//				{
//					try
//					{
//						Directory.CreateDirectory( strSavePath );
//					}
//					catch
//					{
//					}
//				}
//				string strSetDefDifficulty = CDTXMania.stage選曲.r確定された曲.ar難易度ラベル[ CDTXMania.stage選曲.n確定された曲の難易度 ];
//				if ( strSetDefDifficulty != null )
//				{
//					strSetDefDifficulty = "(" + strSetDefDifficulty + ")";
//				}
//				string strFullPath = strSavePath + "\\" + CDTXMania.DTX.TITLE + strSetDefDifficulty +
//					"." + datetime + "_" + strRank + ".png";
//				// Surface.ToFile( pSurface, strFullPath, ImageFileFormat.Png );
//				CDTXMania.app.SaveResultScreen( strFullPath );
//			}
			#endregion
		}
		#endregion
		//-----------------
		#endregion
	}
}
