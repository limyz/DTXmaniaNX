using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using System.Threading;
using SlimDX;
using SlimDX.Direct3D9;
using DirectShowLib;
using FDK;

namespace DTXMania
{
    /// <summary>
    /// 演奏画面の共通クラス (ドラム演奏画面, ギター演奏画面の継承元)
    /// </summary>
    internal abstract class CStagePerfCommonScreen : CStage
    {
        // プロパティ

        public CStagePerfCommonScreen()
        {
            base.listChildActivities.Add(this.actLVFont = new CActLVLNFont());
        }

        public bool bAUTOでないチップが１つでもバーを通過した
        {
            get;
            protected set;
        }

        // メソッド

        #region [ tStorePerfResults_Drums() ]
        public void tStorePerfResults_Drums(out CScoreIni.CPerformanceEntry Drums)
        {
            Drums = new CScoreIni.CPerformanceEntry();

            if (CDTXMania.DTX.bチップがある.Drums && !CDTXMania.ConfigIni.bGuitarRevolutionMode)
            {
                Drums.nスコア = (long)this.actScore.Get(EInstrumentPart.DRUMS);
                if (CDTXMania.ConfigIni.nSkillMode == 0)
                {
                    Drums.dbGameSkill = CScoreIni.tCalculateGameSkillOld(CDTXMania.DTX.LEVEL.Drums, CDTXMania.DTX.LEVELDEC.Drums, CDTXMania.DTX.nVisibleChipsCount.Drums, this.nHitCount_ExclAuto.Drums.Perfect, this.nHitCount_ExclAuto.Drums.Great, this.actCombo.nCurrentCombo.HighestValue.Drums, EInstrumentPart.DRUMS, bIsAutoPlay);
                    Drums.dbPerformanceSkill = CScoreIni.tCalculatePlayingSkillOld(CDTXMania.DTX.nVisibleChipsCount.Drums, this.nHitCount_ExclAuto.Drums.Perfect, this.nHitCount_ExclAuto.Drums.Great, this.nHitCount_ExclAuto.Drums.Good, this.nHitCount_ExclAuto.Drums.Poor, this.nHitCount_ExclAuto.Drums.Miss, this.actCombo.nCurrentCombo.HighestValue.Drums, EInstrumentPart.DRUMS, bIsAutoPlay);
                }
                else if (CDTXMania.ConfigIni.nSkillMode == 1)
                {
                    Drums.dbGameSkill = CScoreIni.tCalculateGameSkill(CDTXMania.DTX.LEVEL.Drums, CDTXMania.DTX.LEVELDEC.Drums, CDTXMania.DTX.nVisibleChipsCount.Drums, this.nHitCount_ExclAuto.Drums.Perfect, this.nHitCount_ExclAuto.Drums.Great, this.actCombo.nCurrentCombo.HighestValue.Drums, EInstrumentPart.DRUMS, bIsAutoPlay);
                    Drums.dbPerformanceSkill = CScoreIni.tCalculatePlayingSkill(CDTXMania.DTX.nVisibleChipsCount.Drums, this.nHitCount_ExclAuto.Drums.Perfect, this.nHitCount_ExclAuto.Drums.Great, this.nHitCount_ExclAuto.Drums.Good, this.nHitCount_ExclAuto.Drums.Poor, this.nHitCount_ExclAuto.Drums.Miss, this.actCombo.nCurrentCombo.HighestValue.Drums, EInstrumentPart.DRUMS, bIsAutoPlay);
                }
                Drums.nPerfectCount = CDTXMania.ConfigIni.bAllDrumsAreAutoPlay ? this.nHitCount_IncAuto.Drums.Perfect : this.nHitCount_ExclAuto.Drums.Perfect;
                Drums.nGreatCount = CDTXMania.ConfigIni.bAllDrumsAreAutoPlay ? this.nHitCount_IncAuto.Drums.Great : this.nHitCount_ExclAuto.Drums.Great;
                Drums.nGoodCount = CDTXMania.ConfigIni.bAllDrumsAreAutoPlay ? this.nHitCount_IncAuto.Drums.Good : this.nHitCount_ExclAuto.Drums.Good;
                Drums.nPoorCount = CDTXMania.ConfigIni.bAllDrumsAreAutoPlay ? this.nHitCount_IncAuto.Drums.Poor : this.nHitCount_ExclAuto.Drums.Poor;
                Drums.nMissCount = CDTXMania.ConfigIni.bAllDrumsAreAutoPlay ? this.nHitCount_IncAuto.Drums.Miss : this.nHitCount_ExclAuto.Drums.Miss;
                Drums.nPerfectCount_ExclAuto = this.nHitCount_ExclAuto.Drums.Perfect;
                Drums.nGreatCount_ExclAuto = this.nHitCount_ExclAuto.Drums.Great;
                Drums.nGoodCount_ExclAuto = this.nHitCount_ExclAuto.Drums.Good;
                Drums.nPoorCount_ExclAuto = this.nHitCount_ExclAuto.Drums.Poor;
                Drums.nMissCount_ExclAuto = this.nHitCount_ExclAuto.Drums.Miss;
                Drums.nMaxCombo = this.actCombo.nCurrentCombo.HighestValue.Drums;
                Drums.nTotalChipsCount = CDTXMania.DTX.nVisibleChipsCount.Drums;
                for (int i = 0; i < (int)ELane.MAX; i++)
                {
                    Drums.bAutoPlay[i] = bIsAutoPlay[i];
                }
                Drums.bTight = CDTXMania.ConfigIni.bTight;
                for (int i = 0; i < 3; i++)
                {
                    Drums.bSudden[i] = CDTXMania.ConfigIni.bSudden[i];
                    Drums.bHidden[i] = CDTXMania.ConfigIni.bHidden[i];
                    Drums.bReverse[i] = CDTXMania.ConfigIni.bReverse[i];
                    Drums.eRandom[i] = CDTXMania.ConfigIni.eRandom[i];
                    Drums.bLight[i] = CDTXMania.ConfigIni.bLight[i];
                    Drums.bLeft[i] = CDTXMania.ConfigIni.bLeft[i];
                    Drums.fScrollSpeed[i] = ((float)(CDTXMania.ConfigIni.nScrollSpeed[i] + 1)) * 0.5f;
                }
                Drums.eDark = CDTXMania.ConfigIni.eDark;
                Drums.nPlaySpeedNumerator = CDTXMania.ConfigIni.nPlaySpeed;
                Drums.nPlaySpeedDenominator = 20;
                Drums.eHHGroup = CDTXMania.ConfigIni.eHHGroup;
                Drums.eFTGroup = CDTXMania.ConfigIni.eFTGroup;
                Drums.eCYGroup = CDTXMania.ConfigIni.eCYGroup;
                Drums.eHitSoundPriorityHH = CDTXMania.ConfigIni.eHitSoundPriorityHH;
                Drums.eHitSoundPriorityFT = CDTXMania.ConfigIni.eHitSoundPriorityFT;
                Drums.eHitSoundPriorityCY = CDTXMania.ConfigIni.eHitSoundPriorityCY;
                Drums.bGuitarEnabled = CDTXMania.ConfigIni.bGuitarEnabled;
                Drums.bDrumsEnabled = CDTXMania.ConfigIni.bDrumsEnabled;
                Drums.bSTAGEFAILEDEnabled = CDTXMania.ConfigIni.bSTAGEFAILEDEnabled;
                Drums.eDamageLevel = CDTXMania.ConfigIni.eDamageLevel;
                Drums.bKeyboardUsed = this.bKeyboardUsed.Drums;
                Drums.bMIDIUsed = this.bMIDIUsed.Drums;
                Drums.bJoypadUsed = this.bJoypadUsed.Drums;
                Drums.bMouseUsed = this.bMouseUsed.Drums;
                Drums.nPerfectRangeMs = CDTXMania.nPerfectRangeMs;
                Drums.nGreatRangeMs = CDTXMania.nGreatRangeMs;
                Drums.nGoodRangeMs = CDTXMania.nGoodRangeMs;
                Drums.nPoorRangeMs = CDTXMania.nPoorRangeMs;
                Drums.strDTXManiaVersion = CDTXMania.VERSION;
                Drums.strDateTime = DateTime.Now.ToString();
                Drums.Hash = CScoreIni.tComputePerformanceSectionMD5(Drums);
            }
        }
        #endregion
        #region [ tStorePerfResults_Guitar() ]
        public void tStorePerfResults_Guitar(out CScoreIni.CPerformanceEntry Guitar)
        {
            Guitar = new CScoreIni.CPerformanceEntry();

            if (CDTXMania.DTX.bチップがある.Guitar)
            {
                Guitar.nスコア = (long)this.actScore.Get(EInstrumentPart.GUITAR);
                if (CDTXMania.ConfigIni.nSkillMode == 0)
                {
                    Guitar.dbGameSkill = CScoreIni.tCalculateGameSkillOld(CDTXMania.DTX.LEVEL.Guitar, CDTXMania.DTX.LEVELDEC.Guitar, CDTXMania.DTX.nVisibleChipsCount.Guitar, this.nHitCount_ExclAuto.Guitar.Perfect, this.nHitCount_ExclAuto.Guitar.Great, this.actCombo.nCurrentCombo.HighestValue.Guitar, EInstrumentPart.GUITAR, bIsAutoPlay);
                    Guitar.dbPerformanceSkill = CScoreIni.tCalculatePlayingSkillOld(CDTXMania.DTX.nVisibleChipsCount.Guitar, this.nHitCount_ExclAuto.Guitar.Perfect, this.nHitCount_ExclAuto.Guitar.Great, this.nHitCount_ExclAuto.Guitar.Good, this.nHitCount_ExclAuto.Guitar.Poor, this.nHitCount_ExclAuto.Guitar.Miss, this.actCombo.nCurrentCombo.HighestValue.Guitar, EInstrumentPart.GUITAR, bIsAutoPlay);
                }
                else if (CDTXMania.ConfigIni.nSkillMode == 1)
                {
                    Guitar.dbGameSkill = CScoreIni.tCalculateGameSkill(CDTXMania.DTX.LEVEL.Guitar, CDTXMania.DTX.LEVELDEC.Guitar, CDTXMania.DTX.nVisibleChipsCount.Guitar, this.nHitCount_ExclAuto.Guitar.Perfect, this.nHitCount_ExclAuto.Guitar.Great, this.actCombo.nCurrentCombo.HighestValue.Guitar, EInstrumentPart.GUITAR, bIsAutoPlay);
                    Guitar.dbPerformanceSkill = CScoreIni.tCalculatePlayingSkill(CDTXMania.DTX.nVisibleChipsCount.Guitar, this.nHitCount_ExclAuto.Guitar.Perfect, this.nHitCount_ExclAuto.Guitar.Great, this.nHitCount_ExclAuto.Guitar.Good, this.nHitCount_ExclAuto.Guitar.Poor, this.nHitCount_ExclAuto.Guitar.Miss, this.actCombo.nCurrentCombo.HighestValue.Guitar, EInstrumentPart.GUITAR, bIsAutoPlay);
                }
                Guitar.nPerfectCount = CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay ? this.nHitCount_IncAuto.Guitar.Perfect : this.nHitCount_ExclAuto.Guitar.Perfect;
                Guitar.nGreatCount = CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay ? this.nHitCount_IncAuto.Guitar.Great : this.nHitCount_ExclAuto.Guitar.Great;
                Guitar.nGoodCount = CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay ? this.nHitCount_IncAuto.Guitar.Good : this.nHitCount_ExclAuto.Guitar.Good;
                Guitar.nPoorCount = CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay ? this.nHitCount_IncAuto.Guitar.Poor : this.nHitCount_ExclAuto.Guitar.Poor;
                Guitar.nMissCount = CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay ? this.nHitCount_IncAuto.Guitar.Miss : this.nHitCount_ExclAuto.Guitar.Miss;
                Guitar.nPerfectCount_ExclAuto = this.nHitCount_ExclAuto.Guitar.Perfect;
                Guitar.nGreatCount_ExclAuto = this.nHitCount_ExclAuto.Guitar.Great;
                Guitar.nGoodCount_ExclAuto = this.nHitCount_ExclAuto.Guitar.Good;
                Guitar.nPoorCount_ExclAuto = this.nHitCount_ExclAuto.Guitar.Poor;
                Guitar.nMissCount_ExclAuto = this.nHitCount_ExclAuto.Guitar.Miss;
                Guitar.nMaxCombo = this.actCombo.nCurrentCombo.HighestValue.Guitar;
                Guitar.nTotalChipsCount = CDTXMania.DTX.nVisibleChipsCount.Guitar;
                for (int i = 0; i < (int)ELane.MAX; i++)
                {
                    Guitar.bAutoPlay[i] = bIsAutoPlay[i];
                }
                Guitar.bTight = CDTXMania.ConfigIni.bTight;
                for (int i = 0; i < 3; i++)
                {
                    Guitar.bSudden[i] = CDTXMania.ConfigIni.bSudden[i];
                    Guitar.bHidden[i] = CDTXMania.ConfigIni.bHidden[i];
                    Guitar.bReverse[i] = CDTXMania.ConfigIni.bReverse[i];
                    Guitar.eRandom[i] = CDTXMania.ConfigIni.eRandom[i];
                    Guitar.bLight[i] = CDTXMania.ConfigIni.bLight[i];
                    Guitar.bLeft[i] = CDTXMania.ConfigIni.bLeft[i];
                    Guitar.fScrollSpeed[i] = ((float)(CDTXMania.ConfigIni.nScrollSpeed[i] + 1)) * 0.5f;
                }
                Guitar.eDark = CDTXMania.ConfigIni.eDark;
                Guitar.nPlaySpeedNumerator = CDTXMania.ConfigIni.nPlaySpeed;
                Guitar.nPlaySpeedDenominator = 20;
                Guitar.eHHGroup = CDTXMania.ConfigIni.eHHGroup;
                Guitar.eFTGroup = CDTXMania.ConfigIni.eFTGroup;
                Guitar.eCYGroup = CDTXMania.ConfigIni.eCYGroup;
                Guitar.eHitSoundPriorityHH = CDTXMania.ConfigIni.eHitSoundPriorityHH;
                Guitar.eHitSoundPriorityFT = CDTXMania.ConfigIni.eHitSoundPriorityFT;
                Guitar.eHitSoundPriorityCY = CDTXMania.ConfigIni.eHitSoundPriorityCY;
                Guitar.bGuitarEnabled = CDTXMania.ConfigIni.bGuitarEnabled;
                Guitar.bDrumsEnabled = CDTXMania.ConfigIni.bDrumsEnabled;
                Guitar.bSTAGEFAILEDEnabled = CDTXMania.ConfigIni.bSTAGEFAILEDEnabled;
                Guitar.eDamageLevel = CDTXMania.ConfigIni.eDamageLevel;
                Guitar.bKeyboardUsed = this.bKeyboardUsed.Guitar;
                Guitar.bMIDIUsed = this.bMIDIUsed.Guitar;
                Guitar.bJoypadUsed = this.bJoypadUsed.Guitar;
                Guitar.bMouseUsed = this.bMouseUsed.Guitar;
                Guitar.nPerfectRangeMs = CDTXMania.nPerfectRangeMs;
                Guitar.nGreatRangeMs = CDTXMania.nGreatRangeMs;
                Guitar.nGoodRangeMs = CDTXMania.nGoodRangeMs;
                Guitar.nPoorRangeMs = CDTXMania.nPoorRangeMs;
                Guitar.strDTXManiaVersion = CDTXMania.VERSION;
                Guitar.strDateTime = DateTime.Now.ToString();
                Guitar.Hash = CScoreIni.tComputePerformanceSectionMD5(Guitar);
            }
        }
        #endregion
        #region [ tStorePerfResultsBass() ]
        public void tStorePerfResultsBass(out CScoreIni.CPerformanceEntry Bass)
        {
            Bass = new CScoreIni.CPerformanceEntry();

            if (CDTXMania.DTX.bチップがある.Bass)
            {
                Bass.nスコア = (long)this.actScore.Get(EInstrumentPart.BASS);
                if (CDTXMania.ConfigIni.nSkillMode == 0)
                {
                    Bass.dbGameSkill = CScoreIni.tCalculateGameSkillOld(CDTXMania.DTX.LEVEL.Bass, CDTXMania.DTX.LEVELDEC.Bass, CDTXMania.DTX.nVisibleChipsCount.Bass, this.nHitCount_ExclAuto.Bass.Perfect, this.nHitCount_ExclAuto.Bass.Great, this.actCombo.nCurrentCombo.HighestValue.Bass, EInstrumentPart.BASS, bIsAutoPlay);
                    Bass.dbPerformanceSkill = CScoreIni.tCalculatePlayingSkillOld(CDTXMania.DTX.nVisibleChipsCount.Bass, this.nHitCount_ExclAuto.Bass.Perfect, this.nHitCount_ExclAuto.Bass.Great, this.nHitCount_ExclAuto.Bass.Good, this.nHitCount_ExclAuto.Bass.Poor, this.nHitCount_ExclAuto.Bass.Miss, this.actCombo.nCurrentCombo.HighestValue.Bass, EInstrumentPart.BASS, bIsAutoPlay);
                }
                else if (CDTXMania.ConfigIni.nSkillMode == 1)
                {
                    Bass.dbGameSkill = CScoreIni.tCalculateGameSkill(CDTXMania.DTX.LEVEL.Bass, CDTXMania.DTX.LEVELDEC.Bass, CDTXMania.DTX.nVisibleChipsCount.Bass, this.nHitCount_ExclAuto.Bass.Perfect, this.nHitCount_ExclAuto.Bass.Great, this.actCombo.nCurrentCombo.HighestValue.Bass, EInstrumentPart.BASS, bIsAutoPlay);
                    Bass.dbPerformanceSkill = CScoreIni.tCalculatePlayingSkill(CDTXMania.DTX.nVisibleChipsCount.Bass, this.nHitCount_ExclAuto.Bass.Perfect, this.nHitCount_ExclAuto.Bass.Great, this.nHitCount_ExclAuto.Bass.Good, this.nHitCount_ExclAuto.Bass.Poor, this.nHitCount_ExclAuto.Bass.Miss, this.actCombo.nCurrentCombo.HighestValue.Bass, EInstrumentPart.BASS, bIsAutoPlay);
                }
                Bass.nPerfectCount = CDTXMania.ConfigIni.bAllBassAreAutoPlay ? this.nHitCount_IncAuto.Bass.Perfect : this.nHitCount_ExclAuto.Bass.Perfect;
                Bass.nGreatCount = CDTXMania.ConfigIni.bAllBassAreAutoPlay ? this.nHitCount_IncAuto.Bass.Great : this.nHitCount_ExclAuto.Bass.Great;
                Bass.nGoodCount = CDTXMania.ConfigIni.bAllBassAreAutoPlay ? this.nHitCount_IncAuto.Bass.Good : this.nHitCount_ExclAuto.Bass.Good;
                Bass.nPoorCount = CDTXMania.ConfigIni.bAllBassAreAutoPlay ? this.nHitCount_IncAuto.Bass.Poor : this.nHitCount_ExclAuto.Bass.Poor;
                Bass.nMissCount = CDTXMania.ConfigIni.bAllBassAreAutoPlay ? this.nHitCount_IncAuto.Bass.Miss : this.nHitCount_ExclAuto.Bass.Miss;
                Bass.nPerfectCount_ExclAuto = this.nHitCount_ExclAuto.Bass.Perfect;
                Bass.nGreatCount_ExclAuto = this.nHitCount_ExclAuto.Bass.Great;
                Bass.nGoodCount_ExclAuto = this.nHitCount_ExclAuto.Bass.Good;
                Bass.nPoorCount_ExclAuto = this.nHitCount_ExclAuto.Bass.Poor;
                Bass.nMissCount_ExclAuto = this.nHitCount_ExclAuto.Bass.Miss;
                Bass.nMaxCombo = this.actCombo.nCurrentCombo.HighestValue.Bass;
                Bass.nTotalChipsCount = CDTXMania.DTX.nVisibleChipsCount.Bass;
                for (int i = 0; i < (int)ELane.MAX; i++)
                {
                    Bass.bAutoPlay[i] = bIsAutoPlay[i];
                }
                Bass.bTight = CDTXMania.ConfigIni.bTight;
                for (int i = 0; i < 3; i++)
                {
                    Bass.bSudden[i] = CDTXMania.ConfigIni.bSudden[i];
                    Bass.bHidden[i] = CDTXMania.ConfigIni.bHidden[i];
                    Bass.bReverse[i] = CDTXMania.ConfigIni.bReverse[i];
                    Bass.eRandom[i] = CDTXMania.ConfigIni.eRandom[i];
                    Bass.bLight[i] = CDTXMania.ConfigIni.bLight[i];
                    Bass.bLeft[i] = CDTXMania.ConfigIni.bLeft[i];
                    Bass.fScrollSpeed[i] = ((float)(CDTXMania.ConfigIni.nScrollSpeed[i] + 1)) * 0.5f;
                }
                Bass.eDark = CDTXMania.ConfigIni.eDark;
                Bass.nPlaySpeedNumerator = CDTXMania.ConfigIni.nPlaySpeed;
                Bass.nPlaySpeedDenominator = 20;
                Bass.eHHGroup = CDTXMania.ConfigIni.eHHGroup;
                Bass.eFTGroup = CDTXMania.ConfigIni.eFTGroup;
                Bass.eCYGroup = CDTXMania.ConfigIni.eCYGroup;
                Bass.eHitSoundPriorityHH = CDTXMania.ConfigIni.eHitSoundPriorityHH;
                Bass.eHitSoundPriorityFT = CDTXMania.ConfigIni.eHitSoundPriorityFT;
                Bass.eHitSoundPriorityCY = CDTXMania.ConfigIni.eHitSoundPriorityCY;
                Bass.bGuitarEnabled = CDTXMania.ConfigIni.bGuitarEnabled;
                Bass.bDrumsEnabled = CDTXMania.ConfigIni.bDrumsEnabled;
                Bass.bSTAGEFAILEDEnabled = CDTXMania.ConfigIni.bSTAGEFAILEDEnabled;
                Bass.eDamageLevel = CDTXMania.ConfigIni.eDamageLevel;
                Bass.bKeyboardUsed = this.bKeyboardUsed.Bass;			// #24280 2011.1.29 yyagi
                Bass.bMIDIUsed = this.bMIDIUsed.Bass;				//
                Bass.bJoypadUsed = this.bJoypadUsed.Bass;		//
                Bass.bMouseUsed = this.bMouseUsed.Bass;					//
                Bass.nPerfectRangeMs = CDTXMania.nPerfectRangeMs;
                Bass.nGreatRangeMs = CDTXMania.nGreatRangeMs;
                Bass.nGoodRangeMs = CDTXMania.nGoodRangeMs;
                Bass.nPoorRangeMs = CDTXMania.nPoorRangeMs;
                Bass.strDTXManiaVersion = CDTXMania.VERSION;
                Bass.strDateTime = DateTime.Now.ToString();
                Bass.Hash = CScoreIni.tComputePerformanceSectionMD5(Bass);
            }
        }
        #endregion

        // CStage 実装

        public override void OnActivate()
        {
            listChip = CDTXMania.DTX.listChip;
            listWAV = CDTXMania.DTX.listWAV;

            this.eReturnValueAfterFadeOut = EPerfScreenReturnValue.Continue;
            this.nCurrentTopChip = (listChip.Count > 0) ? 0 : -1;
            this.LLastPlayedHHWAVNumber = new List<int>(16);
            this.nLastPlayedHHChannelNumber = 0;
            this.nLastPlayedWAVNumber.Guitar = -1;
            this.nLastPlayedWAVNumber.Bass = -1;
            for (int i = 0; i < 50; i++)
            {
                this.nLastPlayedBGMWAVNumber[i] = -1;
            }
            this.rNextGuitarChip = null;
            this.rNextBassChip = null;
            for (int j = 0; j < 12; j++)
            {
                this.r現在の空うちドラムChip[j] = null;
            }
            this.r現在の空うちギターChip = null;
            this.r現在の空うちベースChip = null;
            this.n最大コンボ数_TargetGhost = new STDGBVALUE<int>(); // #35411 2015.08.21 chnmr0 add
            for (int k = 0; k < 3; k++)
            {
                //for ( int n = 0; n < 5; n++ )
                {
                    this.nHitCount_ExclAuto[ k ] = new CHITCOUNTOFRANK();
                    this.nHitCount_IncAuto[ k ] = new CHITCOUNTOFRANK();
                    this.nヒット数_TargetGhost[ k ] = new CHITCOUNTOFRANK(); // #35411 2015.08.21 chnmr0 add
                }
                this.queWailing[k] = new Queue<CDTX.CChip>();
                this.r現在の歓声Chip[k] = null;
            }
            for (int i = 0; i < 3; i++)
            {
                this.bKeyboardUsed[i] = false;
                this.bJoypadUsed[i] = false;
                this.bMIDIUsed[i] = false;
                this.bMouseUsed[i] = false;
                this.ctTimer[i] = null;//new CCounter(0, 3000, 1, CDTXMania.Timer);
                this.ctTimer[i] = new CCounter(0, 3000, 1, CDTXMania.Timer);
            }
            this.bAUTOでないチップが１つでもバーを通過した = false;
            base.OnActivate();
            this.tステータスパネルの選択();
            //this.tパネル文字列の設定();
            this.nJudgeLinePosY.Drums = (CDTXMania.ConfigIni.bReverse.Drums ? 159 + CDTXMania.ConfigIni.nJudgeLine.Drums : 561 - CDTXMania.ConfigIni.nJudgeLine.Drums);
            this.nJudgeLinePosY.Guitar = (CDTXMania.ConfigIni.bReverse.Guitar ? 611 - CDTXMania.ConfigIni.nJudgeLine.Guitar : 154 + CDTXMania.ConfigIni.nJudgeLine.Guitar);
            this.nJudgeLinePosY.Bass = (CDTXMania.ConfigIni.bReverse.Bass ? 611 - CDTXMania.ConfigIni.nJudgeLine.Bass : 154 + CDTXMania.ConfigIni.nJudgeLine.Bass);

            this.nShutterInPosY.Drums = CDTXMania.ConfigIni.nShutterInSide.Drums;
            this.nShutterOutPosY.Drums = CDTXMania.ConfigIni.nShutterOutSide.Drums;
            this.nShutterInPosY.Guitar = CDTXMania.ConfigIni.nShutterInSide.Guitar;
            this.nShutterOutPosY.Guitar = CDTXMania.ConfigIni.nShutterOutSide.Guitar;
            this.nShutterInPosY.Bass = CDTXMania.ConfigIni.nShutterInSide.Bass;
            this.nShutterOutPosY.Bass = CDTXMania.ConfigIni.nShutterOutSide.Bass;

            this.actJudgeString.iP_A = CDTXMania.ConfigIni.bReverse.Drums ? 159 + 0xbd : 561 - 0xbd;
            this.actJudgeString.iP_B = CDTXMania.ConfigIni.bReverse.Drums ? 159 - 0x17 : 561 + 0x17;

            this.nInputAdjustTimeMs.Drums = CDTXMania.ConfigIni.nInputAdjustTimeMs.Drums;		// #23580 2011.1.3 yyagi
            this.nInputAdjustTimeMs.Guitar = CDTXMania.ConfigIni.nInputAdjustTimeMs.Guitar;		//        2011.1.7 ikanick 修正
            this.nInputAdjustTimeMs.Bass = CDTXMania.ConfigIni.nInputAdjustTimeMs.Bass;			//
            this.nJudgeLinePosY_delta.Drums = CDTXMania.ConfigIni.nJudgeLinePosOffset.Drums;    // #31602 2013.6.23 yyagi
            this.nJudgeLinePosY_delta.Guitar = CDTXMania.ConfigIni.nJudgeLinePosOffset.Guitar;  //
            this.nJudgeLinePosY_delta.Bass = CDTXMania.ConfigIni.nJudgeLinePosOffset.Bass;      //
            this.bIsAutoPlay = CDTXMania.ConfigIni.bAutoPlay;									// #24239 2011.1.23 yyagi
            //this.bIsAutoPlay.Guitar = CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay;
            //this.bIsAutoPlay.Bass = CDTXMania.ConfigIni.bAllBassAreAutoPlay;										// #23559 2011.7.28 yyagi
            actGauge.Init(CDTXMania.ConfigIni.nRisky);									// #23559 2011.7.28 yyagi

            this.nPolyphonicSounds = CDTXMania.ConfigIni.nPoliphonicSounds;

            CDTXMania.Skin.tRemoveMixerAll();	// 効果音のストリームをミキサーから解除しておく

            //lockmixer = new object();
            queueMixerSound = new Queue<stmixer>(64);
            bIsDirectSound = (CDTXMania.SoundManager.GetCurrentSoundDeviceType() == "DirectSound");
            dbPlaySpeed = ((double)CDTXMania.ConfigIni.nPlaySpeed) / 20.0;
            bValidScore = true;
            bDTXVmode = false; // とりあえずfalse固定

            #region [ Sounds that should be registered in the mixer before starting playing (chip sounds that will be played immediately after the start of the performance) ]
            foreach (CDTX.CChip pChip in listChip)
            {
                //				Debug.WriteLine( "CH=" + pChip.nChannelNumber.ToString( "x2" ) + ", 整数値=" + pChip.nIntegerValue +  ", time=" + pChip.nPlaybackTimeMs );
                if (pChip.nPlaybackTimeMs <= 0)
                {
                    if (pChip.nChannelNumber == 0xEA)
                    {
                        pChip.bHit = true;
                        //						Debug.WriteLine( "first [DA] BAR=" + pChip.nPlaybackPosition / 384 + " ch=" + pChip.nChannelNumber.ToString( "x2" ) + ", wav=" + pChip.nIntegerValue + ", time=" + pChip.nPlaybackTimeMs );
                        if (listWAV.ContainsKey(pChip.nIntegerValue_InternalNumber))
                        {
                            CDTX.CWAV wc = listWAV[pChip.nIntegerValue_InternalNumber];
                            for (int i = 0; i < nPolyphonicSounds; i++)
                            {
                                if (wc.rSound[i] != null)
                                {
                                    CDTXMania.SoundManager.AddMixer(wc.rSound[i], dbPlaySpeed, pChip.bChipKeepsPlayingAfterPerfEnds);
                                    //AddMixer( wc.rSound[ i ] );		// 最初はqueueを介さず直接ミキサー登録する
                                }
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            #endregion
            if (CDTXMania.ConfigIni.bIsSwappedGuitarBass)	// #24063 2011.1.24 yyagi Gt/Bsの譜面情報入れ替え
            {
                CDTXMania.DTX.SwapGuitarBassInfos();
            }
            this.bブーストボーナス = false;
            this.sw = new Stopwatch();
            this.sw2 = new Stopwatch();
            //			this.gclatencymode = GCSettings.LatencyMode;
            //          GCSettings.LatencyMode = GCLatencyMode.Batch; // 演奏画面中はGCを抑止する
        }
        public override void OnDeactivate()
        {
            this.LLastPlayedHHWAVNumber.Clear();	// #23921 2011.1.4 yyagi
            this.LLastPlayedHHWAVNumber = null;	//
            for (int i = 0; i < 3; i++)
            {
                this.queWailing[i].Clear();
                this.queWailing[i] = null;
            }
            this.ctWailingChipPatternAnimation = null;
            this.ctBPMBar = null;
            this.ctChipPatternAnimation.Drums = null;
            this.ctChipPatternAnimation.Guitar = null;
            this.ctChipPatternAnimation.Bass = null;
            //listWAV.Clear();
            listWAV = null;
            listChip = null;
            queueMixerSound.Clear();
            queueMixerSound = null;
            //          GCSettings.LatencyMode = this.gclatencymode;
            base.OnDeactivate();
        }
        public override void OnManagedCreateResources()
        {
            if (!base.bNotActivated)
            {
                this.tGenerateBackgroundTexture();

				this.txWailingFrame = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\ScreenPlay wailing cursor.png" ) );
                this.txBonusEffect = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\7_Fillin Effect.png" ) );
                if( CDTXMania.ConfigIni.nJudgeAnimeType == 1 )
                    this.tx判定画像anime = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\7_judge strings.png" ) );
                else if( CDTXMania.ConfigIni.nJudgeAnimeType == 2 )
                {
                    this.tx判定画像anime = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\7_JudgeStrings_XG.png" ) );
                    this.tx判定画像anime_2 = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\7_JudgeStrings_XG.png" ) );
                    this.tx判定画像anime_3 = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\7_JudgeStrings_XG.png" ) );
                }

                base.OnManagedCreateResources();
            }
        }
        public override void OnManagedReleaseResources()
        {
            if (!base.bNotActivated)
            {
                CDTXMania.tReleaseTexture(ref this.tx背景);

                CDTXMania.tReleaseTexture(ref this.txWailingFrame);
                CDTXMania.tReleaseTexture(ref this.tx判定画像anime);
                CDTXMania.tReleaseTexture(ref this.tx判定画像anime_2);
                CDTXMania.tReleaseTexture(ref this.tx判定画像anime_3);
                CDTXMania.tReleaseTexture(ref this.txBonusEffect);
                base.OnManagedReleaseResources();
            }
        }

        // Other

        #region [ protected ]
        //-----------------
        public class CHITCOUNTOFRANK
        {
            // Fields
            public int Good;
            public int Great;
            public int Miss;
            public int Perfect;
            public int Poor;

            // Properties
            public int this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return this.Perfect;

                        case 1:
                            return this.Great;

                        case 2:
                            return this.Good;

                        case 3:
                            return this.Poor;

                        case 4:
                            return this.Miss;
                    }
                    throw new IndexOutOfRangeException();
                }
                set
                {
                    switch (index)
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

                        case 4:
                            this.Miss = value;
                            return;
                    }
                    throw new IndexOutOfRangeException();
                }
            }
        }

        static CStagePerfCommonScreen()
        {
            nJudgeLineMinPosY = 461;//(CDTXMania.ConfigIni.bReverse.Drums ? 259 : 461);
            nJudgeLineMaxPosY = 561;//(CDTXMania.ConfigIni.bReverse.Drums ? 159 : 561);
            nShutterMinPosY = 0;
            nShutterMaxPosY = 100;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct STKARAUCHI
        {
            public CDTX.CChip HH;
            public CDTX.CChip SD;
            public CDTX.CChip BD;
            public CDTX.CChip HT;
            public CDTX.CChip LT;
            public CDTX.CChip FT;
            public CDTX.CChip CY;
            public CDTX.CChip HHO;
            public CDTX.CChip RD;
            public CDTX.CChip LC;
            public CDTX.CChip LP;
            public CDTX.CChip LBD;
            public CDTX.CChip this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return this.HH;

                        case 1:
                            return this.SD;

                        case 2:
                            return this.BD;

                        case 3:
                            return this.HT;

                        case 4:
                            return this.LT;

                        case 5:
                            return this.FT;

                        case 6:
                            return this.CY;

                        case 7:
                            return this.HHO;

                        case 8:
                            return this.RD;

                        case 9:
                            return this.LC;

                        case 10:
                            return this.LP;

                        case 11:
                            return this.LBD;

                    }
                    throw new IndexOutOfRangeException();
                }
                set
                {
                    switch (index)
                    {
                        case 0:
                            this.HH = value;
                            return;

                        case 1:
                            this.SD = value;
                            return;

                        case 2:
                            this.BD = value;
                            return;

                        case 3:
                            this.HT = value;
                            return;

                        case 4:
                            this.LT = value;
                            return;

                        case 5:
                            this.FT = value;
                            return;

                        case 6:
                            this.CY = value;
                            return;

                        case 7:
                            this.HHO = value;
                            return;

                        case 8:
                            this.RD = value;
                            return;

                        case 9:
                            this.LC = value;
                            return;

                        case 10:
                            this.LP = value;
                            return;

                        case 11:
                            this.LBD = value;
                            return;

                    }
                    throw new IndexOutOfRangeException();
                }
            }
        }
        protected struct stmixer
        {
            internal bool bIsAdd;
            internal CSound csound;
            internal bool b演奏終了後も再生が続くチップである;
        };

        public CActPerfAVI actAVI;
        public CActPerfBGA actBGA;

        protected CActLVLNFont actLVFont;
        protected CActPerfChipFireGB actChipFireGB;
        public CActPerfCommonCombo actCombo;
        protected CActPerfCommonDanger actDANGER;
        protected CActFIFOBlackStart actFI;
        protected CActFIFOBlack actFO;
        protected CActFIFOWhite actFOClear;
        public CActFIFOWhiteClear actFOStageClear;
        //protected CActPerfStageClear actStageClear;
        public CActPerfCommonGauge actGauge;
        public CActPerfDrumsFillingEffect actFillin;
        protected CActPerfCommonJudgementCharacterString actJudgeString;
        protected CActPerfDrumsLaneFlashD actLaneFlushD;
        protected CActPerfCommonLaneFlushGB actLaneFlushGB;
//      protected CActPerfPanelString actPanel;
        protected CActPerformanceInformation actPlayInfo;
        public CActPerfCommonRGB actRGB;
        public CActPerfCommonScore actScore;
        protected CActPerfStageFailure actStageFailed;
        public CActPerfCommonStatusPanel actStatusPanel;
        protected CActPerfCommonWailingBonus actWailingBonus;
        public CActPerfScrollSpeed actScrollSpeed;
        protected CActPerfSkillMeter actGraph;
        protected bool bPAUSE;
        protected STDGBVALUE<bool> bMIDIUsed;
        protected STDGBVALUE<bool> bKeyboardUsed;
        protected STDGBVALUE<bool> bJoypadUsed;
        protected STDGBVALUE<bool> bMouseUsed;
        protected CCounter ctWailingChipPatternAnimation;
        public CCounter ctBPMBar;
        public CCounter ct登場用;
        public CCounter ctComboTimer;

        protected STDGBVALUE<CCounter> ctChipPatternAnimation;
        protected abstract void tJudgeLineMovingUpandDown();
        protected EPerfScreenReturnValue eReturnValueAfterFadeOut;
        protected readonly int[,] nBGAスコープチャンネルマップ = new int[,] { { 0xc4, 0xc7, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xe0 }, { 4, 7, 0x55, 0x56, 0x57, 0x58, 0x59, 0x60 } };
        protected readonly int[] nチャンネル0Atoパッド08 = new int[] { 1, 2, 3, 4, 5, 7, 6, 1, 8, 0, 9, 9 };
        protected readonly int[] nチャンネル0Atoレーン07 = new int[] { 1, 2, 3, 4, 5, 7, 6, 1, 9, 0, 8, 8 };
        //                         RD LC  LP  RD
        protected readonly int[] nパッド0Atoチャンネル0A = new int[] { 0x11, 0x12, 0x13, 0x14, 0x15, 0x17, 0x16, 0x18, 0x19, 0x1a, 0x1b, 0x1c };
        protected readonly int[] nパッド0Atoパッド08 = new int[] { 1, 2, 3, 4, 5, 6, 7, 1, 8, 0, 9, 9 };// パッド画像のヒット処理用
        //   HH SD BD HT LT FT CY HHO RD LC LP LBD
        protected readonly int[] nパッド0Atoレーン07 = new int[] { 1, 2, 3, 4, 5, 6, 7, 1, 9, 0, 8, 8 };
        protected readonly float[,] fDamageGaugeDelta = new float[,] { { 0.004f, 0.006f, 0.006f }, { 0.002f, 0.003f, 0.003f }, { 0f, 0f, 0f }, { -0.02f, -0.03f, -0.03f }, { -0.05f, -0.05f, -0.05f } };
        protected readonly float[] fDamageLevelFactor = new float[] { 0.5f, 1f, 1.5f };

        public STDGBVALUE<int> nJudgeLinePosY = new STDGBVALUE<int>();//(CDTXMania.ConfigIni.bReverse.Drums ? 159 : 561);
        public STDGBVALUE<int> nShutterInPosY = new STDGBVALUE<int>();
        public STDGBVALUE<int> nShutterOutPosY = new STDGBVALUE<int>();
        public long n現在のスコア = 0;
        public STDGBVALUE<CHITCOUNTOFRANK> nHitCount_ExclAuto;
        public STDGBVALUE<CHITCOUNTOFRANK> nHitCount_IncAuto;
        protected int nCurrentTopChip = -1;
        protected int[] nLastPlayedBGMWAVNumber = new int[50];
        protected static int nJudgeLineMaxPosY;
        protected static int nJudgeLineMinPosY;
        protected static int nShutterMaxPosY;
        protected static int nShutterMinPosY;
        protected int nLastPlayedHHChannelNumber;
        protected List<int> LLastPlayedHHWAVNumber;		// #23921 2011.1.4 yyagi: change "int" to "List<int>", for recording multiple wav No.
        protected STLANEVALUE<int> nLastPlayedWAVNumber;	// #26388 2011.11.8 yyagi: change "nLastPlayedWAVNumber.GUITAR" and "nLastPlayedWAVNumber.BASS"
        //							into "nLastPlayedWAVNumber";
        //		protected int nLastPlayedWAVNumber.GUITAR;
        //		protected int nLastPlayedWAVNumber.BASS;

        protected volatile Queue<stmixer> queueMixerSound; // #24820 2013.1.21 yyagi まずは単純にAdd/Removeを1個のキューでまとめて管理するやり方で設計する
        protected DateTime dtLastQueueOperation; //
        protected bool bIsDirectSound; //
        protected double dbPlaySpeed;
        protected bool bValidScore;
        protected bool bDTXVmode;
        protected STDGBVALUE<int> nJudgeLinePosY_delta; // #31602 2013.6.23 yyagi 表示遅延対策として、判定ラインの表示位置をずらす機能を追加する

        private CCounter[] ctTimer = new CCounter[3];
        public bool bブーストボーナス = false;

        protected STDGBVALUE<Queue<CDTX.CChip>> queWailing;
        protected STDGBVALUE<CDTX.CChip> r現在の歓声Chip;
        protected CDTX.CChip r現在の空うちギターChip;
        protected STKARAUCHI r現在の空うちドラムChip;
        protected CDTX.CChip r現在の空うちベースChip;
        protected CDTX.CChip rNextGuitarChip;
        protected CDTX.CChip rNextBassChip;
        protected CTexture txWailingFrame;
        protected CTexture txチップ;
        protected CTexture txヒットバー;
        public CTexture tx判定画像anime;     //2013.8.2 kairera0467 アニメーションの場合はあらかじめこっちで読み込む。
        public CTexture tx判定画像anime_2;   //2014.3.16 kairera0467 棒とかで必要になる。
        public CTexture tx判定画像anime_3;
        public CTexture txBonusEffect;

        //fork
        protected STDGBVALUE<CHITCOUNTOFRANK> nヒット数_TargetGhost; // #35411 2015.08.21 chnmr0 add
        protected STDGBVALUE<int> nコンボ数_TargetGhost;
        public STDGBVALUE<int> n最大コンボ数_TargetGhost;

        protected CTexture tx背景;
        protected STDGBVALUE<int> nInputAdjustTimeMs;		// #23580 2011.1.3 yyagi
        public STAUTOPLAY bIsAutoPlay;		// #24239 2011.1.23 yyagi
        //		protected int nRisky_InitialVar, nRiskyTime;		// #23559 2011.7.28 yyagi → CAct演奏ゲージ共通クラスに隠蔽
        protected int nPolyphonicSounds;

        protected List<CDTX.CChip> listChip;
        protected Dictionary<int, CDTX.CWAV> listWAV;

        protected Stopwatch sw;		// 2011.6.13 最適化検討用のストップウォッチ
        protected Stopwatch sw2;
        //		protected GCLatencyMode gclatencymode;

        public void AddMixer(CSound cs, bool _b演奏終了後も再生が続くチップである)
        {
            stmixer stm = new stmixer()
            {
                bIsAdd = true,
                csound = cs,
                b演奏終了後も再生が続くチップである = _b演奏終了後も再生が続くチップである
            };
            queueMixerSound.Enqueue(stm);
            //Debug.WriteLine("★Queue: add " + Path.GetFileName(stm.csound.strFilename));
        }
        public void RemoveMixer(CSound cs)
        {
            stmixer stm = new stmixer()
            {
                bIsAdd = false,
                csound = cs,
                b演奏終了後も再生が続くチップである = false
            };
            queueMixerSound.Enqueue(stm);
            //Debug.WriteLine("★Queue: remove " + Path.GetFileName(stm.csound.strFilename));
        }
        public void ManageMixerQueue()
        {
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
                            CDTXMania.SoundManager.AddMixer(stm.csound, dbPlaySpeed, stm.b演奏終了後も再生が続くチップである);
                        }
                        else
                        {
                            CDTXMania.SoundManager.RemoveMixer(stm.csound);
                        }
                    }
                }
            }
        }

		protected EJudgement e指定時刻からChipのJUDGEを返す( long nTime, CDTX.CChip pChip, int nInputAdjustTime, bool saveLag = true )
        {
            if (pChip != null)
            {
                // #35411 2015.08.22 chnmr0 modified add check save lag flag for ghost
                int lag = (int)(nTime + nInputAdjustTime - pChip.nPlaybackTimeMs);
                if (saveLag)
                {
                    pChip.nLag = lag;       // #23580 2011.1.3 yyagi: add "nInputAdjustTime" to add input timing adjust feature
					if (pChip.eInstrumentPart != EInstrumentPart.UNKNOWN)
					{
						pChip.nCurrentComboForGhost = this.actCombo.nCurrentCombo[(int)pChip.eInstrumentPart];
					}
                }
                // #35411 modify end

                int nDeltaTime = Math.Abs( lag );
                if (nDeltaTime <= CDTXMania.nPerfectRangeMs)
                {
                    return EJudgement.Perfect;
                }
                if (nDeltaTime <= CDTXMania.nGreatRangeMs)
                {
                    return EJudgement.Great;
                }
                if (nDeltaTime <= CDTXMania.nGoodRangeMs)
                {
                    return EJudgement.Good;
                }
                if (nDeltaTime <= CDTXMania.nPoorRangeMs)
                {
                    return EJudgement.Poor;
                }
            }
            return EJudgement.Miss;
        }
        protected CDTX.CChip r空うちChip(EInstrumentPart part, EPad pad)
        {
            switch (part)
            {
                case EInstrumentPart.DRUMS:
                    switch (pad)
                    {
                        case EPad.HH:
                            if (this.r現在の空うちドラムChip.HH != null)
                            {
                                return this.r現在の空うちドラムChip.HH;
                            }
                            if (CDTXMania.ConfigIni.eHHGroup != EHHGroup.ハイハットのみ打ち分ける)
                            {
                                if (CDTXMania.ConfigIni.eHHGroup == EHHGroup.左シンバルのみ打ち分ける)
                                {
                                    return this.r現在の空うちドラムChip.HHO;
                                }
                                if (this.r現在の空うちドラムChip.HHO != null)
                                {
                                    return this.r現在の空うちドラムChip.HHO;
                                }
                            }
                            return this.r現在の空うちドラムChip.LC;

                        case EPad.SD:
                            return this.r現在の空うちドラムChip.SD;

                        case EPad.BD:
                            return this.r現在の空うちドラムChip.BD;

                        case EPad.HT:
                            return this.r現在の空うちドラムChip.HT;

                        case EPad.LT:
                            if (this.r現在の空うちドラムChip.LT != null)
                            {
                                return this.r現在の空うちドラムChip.LT;
                            }
                            if (CDTXMania.ConfigIni.eFTGroup == EFTGroup.共通)
                            {
                                return this.r現在の空うちドラムChip.FT;
                            }
                            return null;

                        case EPad.FT:
                            if (this.r現在の空うちドラムChip.FT != null)
                            {
                                return this.r現在の空うちドラムChip.FT;
                            }
                            if (CDTXMania.ConfigIni.eFTGroup == EFTGroup.共通)
                            {
                                return this.r現在の空うちドラムChip.LT;
                            }
                            return null;

                        case EPad.CY:
                            if (this.r現在の空うちドラムChip.CY != null)
                            {
                                return this.r現在の空うちドラムChip.CY;
                            }
                            if (CDTXMania.ConfigIni.eCYGroup == ECYGroup.共通)
                            {
                                return this.r現在の空うちドラムChip.RD;
                            }
                            return null;

                        case EPad.HHO:
                            if (this.r現在の空うちドラムChip.HHO != null)
                            {
                                return this.r現在の空うちドラムChip.HHO;
                            }
                            if (CDTXMania.ConfigIni.eHHGroup != EHHGroup.ハイハットのみ打ち分ける)
                            {
                                if (CDTXMania.ConfigIni.eHHGroup == EHHGroup.左シンバルのみ打ち分ける)
                                {
                                    return this.r現在の空うちドラムChip.HH;
                                }
                                if (this.r現在の空うちドラムChip.HH != null)
                                {
                                    return this.r現在の空うちドラムChip.HH;
                                }
                            }
                            return this.r現在の空うちドラムChip.LC;

                        case EPad.RD:
                            if (this.r現在の空うちドラムChip.RD != null)
                            {
                                return this.r現在の空うちドラムChip.RD;
                            }
                            if (CDTXMania.ConfigIni.eCYGroup == ECYGroup.共通)
                            {
                                return this.r現在の空うちドラムChip.CY;
                            }
                            return null;

                        case EPad.LC:
                            if (this.r現在の空うちドラムChip.LC != null)
                            {
                                return this.r現在の空うちドラムChip.LC;
                            }
                            if ((CDTXMania.ConfigIni.eHHGroup != EHHGroup.ハイハットのみ打ち分ける) && (CDTXMania.ConfigIni.eHHGroup != EHHGroup.全部共通))
                            {
                                return null;
                            }
                            if (this.r現在の空うちドラムChip.HH != null)
                            {
                                return this.r現在の空うちドラムChip.HH;
                            }
                            return this.r現在の空うちドラムChip.HHO;

                        case EPad.LP:
                            if (this.r現在の空うちドラムChip.LP != null)
                            {
                                return this.r現在の空うちドラムChip.LP;
                            }
                            if (CDTXMania.ConfigIni.eBDGroup != EBDGroup.左右ペダルのみ打ち分ける)
                            {
                                if (this.r現在の空うちドラムChip.LBD != null)
                                {
                                    return this.r現在の空うちドラムChip.LBD;
                                }
                            }
                            return this.r現在の空うちドラムChip.LP;

                        case EPad.LBD:
                            if (this.r現在の空うちドラムChip.LBD != null)
                            {
                                return this.r現在の空うちドラムChip.LBD;
                            }
                            if (CDTXMania.ConfigIni.eBDGroup != EBDGroup.左右ペダルのみ打ち分ける)
                            {
                                if (this.r現在の空うちドラムChip.LP != null)
                                {
                                    return this.r現在の空うちドラムChip.LP;
                                }
                            }
                            return this.r現在の空うちドラムChip.LBD;


                    }
                    break;

                case EInstrumentPart.GUITAR:
                    return this.r現在の空うちギターChip;

                case EInstrumentPart.BASS:
                    return this.r現在の空うちベースChip;
            }
            return null;
        }
        protected CDTX.CChip r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(long nTime, int nChannel, int nInputAdjustTime)
        {
            sw2.Start();
            nTime += nInputAdjustTime;						// #24239 2011.1.23 yyagi InputAdjust

            int nIndex_InitialPositionSearchingToPast;
            if (this.nCurrentTopChip == -1)				// 演奏データとして1個もチップがない場合は
            {
                sw2.Stop();
                return null;
            }
            int count = listChip.Count;
            int nIndex_NearestChip_Future = nIndex_InitialPositionSearchingToPast = this.nCurrentTopChip;
            if (this.nCurrentTopChip >= count)			// その時点で演奏すべきチップが既に全部無くなっていたら
            {
                nIndex_NearestChip_Future = nIndex_InitialPositionSearchingToPast = count - 1;
            }
            for (; nIndex_NearestChip_Future < count; nIndex_NearestChip_Future++)
            {
                CDTX.CChip chip = CDTXMania.DTX.listChip[nIndex_NearestChip_Future];
                if (((0x11 <= nChannel) && (nChannel <= 0x1c)))
                {
                    if ((chip.nChannelNumber == nChannel) || (chip.nChannelNumber == (nChannel + 0x20)))
                    {
                        if (chip.nPlaybackTimeMs > nTime)
                        {
                            break;
                        }
                        nIndex_InitialPositionSearchingToPast = nIndex_NearestChip_Future;
                    }
                    continue;	// ほんの僅かながら高速化
                }
                else if ((nChannel == 0x2F && chip.eInstrumentPart == EInstrumentPart.GUITAR) || (((0x20 <= nChannel && nChannel <= 0x28) || (0x93 <= nChannel && nChannel <= 0x9F) || (0xA9 <= nChannel && nChannel <= 0xAF) || (0xD0 <= nChannel && nChannel <= 0xD3)) && chip.nChannelNumber == nChannel))
                {
                    if (chip.nPlaybackTimeMs > nTime)
                    {
                        break;
                    }
                    nIndex_InitialPositionSearchingToPast = nIndex_NearestChip_Future;
                }
                else if ((nChannel == 0x4F) && (chip.eInstrumentPart == EInstrumentPart.BASS) || (((0xA0 <= nChannel && nChannel <= 0xA8) || (0xC5 <= nChannel && nChannel <= 0xC6) || (0xC8 <= nChannel && nChannel <= 0xCF) || (0xDA <= nChannel && nChannel <= 0xDF) || (0xE1 <= nChannel && nChannel <= 0xE8)) && chip.nChannelNumber == nChannel))
                {
                    if (chip.nPlaybackTimeMs > nTime)
                    {
                        break;
                    }
                    nIndex_InitialPositionSearchingToPast = nIndex_NearestChip_Future;
                }
                // nIndex_NearestChip_Future++;
            }
            int nIndex_NearestChip_Past = nIndex_InitialPositionSearchingToPast;
            //while ( nIndex_NearestChip_Past >= 0 )			// 過去方向への検索
            for (; nIndex_NearestChip_Past >= 0; nIndex_NearestChip_Past--)
            {
                CDTX.CChip chip = listChip[nIndex_NearestChip_Past];
                if ((0x11 <= nChannel) && (nChannel <= 0x1c))
                {
                    if ((chip.nChannelNumber == nChannel) || (chip.nChannelNumber == (nChannel + 0x20)))
                    {
                        break;
                    }
                }
                else if ((nChannel == 0x2F && chip.eInstrumentPart == EInstrumentPart.GUITAR) || (((0x20 <= nChannel && nChannel <= 0x28) || (0x93 <= nChannel && nChannel <= 0x9F) || (0xA9 <= nChannel && nChannel <= 0xAF) || (0xD0 <= nChannel && nChannel <= 0xD3)) && chip.nChannelNumber == nChannel))
                {
                    if ((0x20 <= chip.nChannelNumber && chip.nChannelNumber <= 0x28) || (((0x20 <= nChannel && nChannel <= 0x28) || (0x93 <= nChannel && nChannel <= 0x9F) || (0xA9 <= nChannel && nChannel <= 0xAF) || (0xD0 <= nChannel && nChannel <= 0xD3)) && chip.nChannelNumber == nChannel))
                    {
                        break;
                    }
                }
                else if (((nChannel == 0xAF && chip.eInstrumentPart == EInstrumentPart.BASS) || (((0xA0 <= nChannel && nChannel <= 0xA8) || (0xC5 <= nChannel && nChannel <= 0xC6) || (0xC8 <= nChannel && nChannel <= 0xCF) || (0xDA <= nChannel && nChannel <= 0xDF) || (0xE1 <= nChannel && nChannel <= 0xE8)) && chip.nChannelNumber == nChannel)))
                {
                    if ((0xA0 <= nChannel && nChannel <= 0xA8) || (((0xC5 <= nChannel && nChannel <= 0xC6) || (0xC8 <= nChannel && nChannel <= 0xCF) || (0xDA <= nChannel && nChannel <= 0xDF) || (0xE1 <= nChannel && nChannel <= 0xE8)) && chip.nChannelNumber == nChannel))
                    {
                        break;
                    }
                }
                // nIndex_NearestChip_Past--;
            }

            if (nIndex_NearestChip_Future >= count)
            {
                if (nIndex_NearestChip_Past < 0)	// 検索対象が過去未来どちらにも見つからなかった場合
                {
                    return null;
                }
                else 								// 検索対象が未来方向には見つからなかった(しかし過去方向には見つかった)場合
                {
                    sw2.Stop();
                    return listChip[nIndex_NearestChip_Past];
                }
            }
            else if (nIndex_NearestChip_Past < 0)	// 検索対象が過去方向には見つからなかった(しかし未来方向には見つかった)場合
            {
                sw2.Stop();
                return listChip[nIndex_NearestChip_Future];
            }
            // 検索対象が過去未来の双方に見つかったなら、より近い方を採用する
            CDTX.CChip nearestChip_Future = listChip[nIndex_NearestChip_Future];
            CDTX.CChip nearestChip_Past = listChip[nIndex_NearestChip_Past];
            int nDiffTime_Future = Math.Abs((int)(nTime - nearestChip_Future.nPlaybackTimeMs));
            int nDiffTime_Past = Math.Abs((int)(nTime - nearestChip_Past.nPlaybackTimeMs));
            if (nDiffTime_Future >= nDiffTime_Past)
            {
                sw2.Stop();
                return nearestChip_Past;
            }
            sw2.Stop();
            return nearestChip_Future;
        }
        protected void tPlaySound(CDTX.CChip rChip, long n再生開始システム時刻ms, EInstrumentPart part)
        {
            this.tPlaySound(rChip, n再生開始システム時刻ms, part, CDTXMania.ConfigIni.n手動再生音量, false, false);
        }
        protected void tPlaySound(CDTX.CChip rChip, long n再生開始システム時刻ms, EInstrumentPart part, int n音量)
        {
            this.tPlaySound(rChip, n再生開始システム時刻ms, part, n音量, false, false);
        }
        protected void tPlaySound(CDTX.CChip rChip, long n再生開始システム時刻ms, EInstrumentPart part, int n音量, bool bモニタ)
        {
            this.tPlaySound(rChip, n再生開始システム時刻ms, part, n音量, bモニタ, false);
        }
        protected void tPlaySound(CDTX.CChip pChip, long n再生開始システム時刻ms, EInstrumentPart part, int n音量, bool bモニタ, bool b音程をずらして再生)
        {
            if (pChip != null)
            {
                bool overwrite = false;
                switch (part)
                {
                    case EInstrumentPart.DRUMS:
                        #region [ DRUMS ]
                        {
                            int index = pChip.nChannelNumber;
                            if ((0x11 <= index) && (index <= 0x1c))
                            {
                                index -= 0x11;
                            }
                            else if ((0x31 <= index) && (index <= 0x3c))
                            {
                                index -= 0x31;
                            }
                            // mute sound (auto)
                            // 4A: 84: HH (HO/HC)
                            // 4B: 85: CY
                            // 4C: 86: RD
                            // 4D: 87: LC
                            // 2A: 88: Gt
                            // AA: 89: Bs
                            else if (0x84 == index)	// 仮に今だけ追加 HHは消音処理があるので overwriteフラグ系の処理は改めて不要
                            {
                                index = 0;
                            }
                            else if ((0x85 <= index) && (index <= 0x87))	// 仮に今だけ追加
                            {
                                //            CY    RD    LC
                                int[] ch = { 0x16, 0x19, 0x1A };
                                pChip.nChannelNumber = ch[pChip.nChannelNumber - 0x85];
                                index = pChip.nChannelNumber - 0x11;
                                overwrite = true;
                            }
                            else
                            {
                                return;
                            }

                            int nLane = this.nチャンネル0Atoレーン07[index];
                            if (((((nLane == 1) && (index == 0)) && ((this.nLastPlayedHHChannelNumber != 0x18) && (this.nLastPlayedHHChannelNumber != 0x38))) || ((((nLane == 8)) && ((index == 10) && (this.nLastPlayedHHChannelNumber != 0x18))) && (this.nLastPlayedHHChannelNumber != 0x38))) && CDTXMania.ConfigIni.bMutingLP)
                            {
                                for (int i = 0; i < this.LLastPlayedHHWAVNumber.Count; i++)
                                {
                                    CDTXMania.DTX.tStopPlayingWav(this.LLastPlayedHHWAVNumber[i]);
                                }
                                this.LLastPlayedHHWAVNumber.Clear();
                                this.nLastPlayedHHChannelNumber = pChip.nChannelNumber;
                            }
                            switch (index)
                            {
                                case 0:
                                case 7:
                                case 0x20:
                                case 0x27:
                                    if (this.LLastPlayedHHWAVNumber.Count >= 0x10)
                                    {
                                        this.LLastPlayedHHWAVNumber.RemoveAt(0);
                                    }
                                    if (!this.LLastPlayedHHWAVNumber.Contains(pChip.nIntegerValue_InternalNumber))
                                    {
                                        this.LLastPlayedHHWAVNumber.Add(pChip.nIntegerValue_InternalNumber);
                                    }
                                    break;
                            }

                            CDTXMania.DTX.tPlayChip(pChip, n再生開始システム時刻ms, nLane, n音量, bモニタ);
                            this.nLastPlayedWAVNumber[nLane] = pChip.nIntegerValue_InternalNumber;		// nLaneでなくindexにすると、LC(1A-11=09)とギター(enumで09)がかぶってLC音が消されるので注意
                            return;
                        }
                        #endregion
                    case EInstrumentPart.GUITAR:
                        #region [ GUITAR ]
                        CDTXMania.DTX.tStopPlayingWav(this.nLastPlayedWAVNumber.Guitar);
                        CDTXMania.DTX.tPlayChip(pChip, n再生開始システム時刻ms, (int)ELane.Guitar, n音量, bモニタ, b音程をずらして再生);
                        this.nLastPlayedWAVNumber.Guitar = pChip.nIntegerValue_InternalNumber;
                        return;
                        #endregion
                    case EInstrumentPart.BASS:
                        #region [ BASS ]
                        CDTXMania.DTX.tStopPlayingWav(this.nLastPlayedWAVNumber.Bass);
                        CDTXMania.DTX.tPlayChip(pChip, n再生開始システム時刻ms, (int)ELane.Bass, n音量, bモニタ, b音程をずらして再生);
                        this.nLastPlayedWAVNumber.Bass = pChip.nIntegerValue_InternalNumber;
                        return;
                        #endregion

                    default:
                        break;
                }
            }
        }
        protected void tステータスパネルの選択()
        {
            if( CDTXMania.bCompactMode )
            {
                this.actStatusPanel.tGetDifficultyLabelFromScript( CDTXMania.stageSongSelection.rConfirmedSong.arDifficultyLabel[ CDTXMania.stageSongSelection.nConfirmedSongDifficulty ] );
            }
            else if( CDTXMania.stageSongSelection.rConfirmedSong != null )
            {
                this.actStatusPanel.tGetDifficultyLabelFromScript( CDTXMania.stageSongSelection.rConfirmedSong.arDifficultyLabel[ CDTXMania.stageSongSelection.nConfirmedSongDifficulty ] );
            }
        }
        protected EJudgement tProcessChipHit(long nHitTime, CDTX.CChip pChip)
        {
            return tProcessChipHit(nHitTime, pChip, true);
        }
        protected abstract EJudgement tProcessChipHit(long nHitTime, CDTX.CChip pChip, bool bCorrectLane);
        protected EJudgement tProcessChipHit(long nHitTime, CDTX.CChip pChip, EInstrumentPart screenmode)		// EInstrumentPart screenmode
        {
            return tProcessChipHit(nHitTime, pChip, screenmode, true);
        }
        protected EJudgement tProcessChipHit(long nHitTime, CDTX.CChip pChip, EInstrumentPart screenmode, bool bCorrectLane)
        {
            pChip.bHit = true;
            if (pChip.eInstrumentPart == EInstrumentPart.UNKNOWN)
            {
                this.bAUTOでないチップが１つでもバーを通過した = true;
            }
            bool bPChipIsAutoPlay = bCheckAutoPlay(pChip);
            //bool bPChipIsAutoPlay = false; // Test code only Fisyher
            pChip.bIsAutoPlayed = bPChipIsAutoPlay;			// 2011.6.10 yyagi
            EJudgement eJudgeResult = EJudgement.Auto;
            switch (pChip.eInstrumentPart)
            {
                case EInstrumentPart.DRUMS:
                    {
                        int nInputAdjustTime = bPChipIsAutoPlay ? 0 : this.nInputAdjustTimeMs.Drums;
                        eJudgeResult = (bCorrectLane) ? this.e指定時刻からChipのJUDGEを返す(nHitTime, pChip, nInputAdjustTime) : EJudgement.Miss;
                        this.actJudgeString.Start(this.nチャンネル0Atoレーン07[pChip.nChannelNumber - 0x11], bPChipIsAutoPlay ? EJudgement.Auto : eJudgeResult, pChip.nLag);
                    }
                    break;

                case EInstrumentPart.GUITAR:
                    {
                        int nInputAdjustTime = bPChipIsAutoPlay ? 0 : this.nInputAdjustTimeMs.Guitar;
                        eJudgeResult = (bCorrectLane) ? this.e指定時刻からChipのJUDGEを返す(nHitTime, pChip, nInputAdjustTime) : EJudgement.Miss;
                        this.actJudgeString.Start(13, bPChipIsAutoPlay ? EJudgement.Auto : eJudgeResult, pChip.nLag);
                        break;
                    }

                case EInstrumentPart.BASS:
                    {
                        int nInputAdjustTime = bPChipIsAutoPlay ? 0 : this.nInputAdjustTimeMs.Bass;
                        eJudgeResult = (bCorrectLane) ? this.e指定時刻からChipのJUDGEを返す(nHitTime, pChip, nInputAdjustTime) : EJudgement.Miss;
                        this.actJudgeString.Start(14, bPChipIsAutoPlay ? EJudgement.Auto : eJudgeResult, pChip.nLag);
                    }
                    break;

                case EInstrumentPart.UNKNOWN:
                    {
                        if (pChip.nChannelNumber == 0x4F)
                        {
                            int nInputAdjustTime = bPChipIsAutoPlay ? 0 : this.nInputAdjustTimeMs.Drums;
                            eJudgeResult = (bCorrectLane) ? this.e指定時刻からChipのJUDGEを返す(nHitTime, pChip, nInputAdjustTime) : EJudgement.Miss;
                        }
                    }
                    break;
            }

            if (CDTXMania.ConfigIni.bAutoAddGage == false)
            {
                if (!bPChipIsAutoPlay && (pChip.eInstrumentPart != EInstrumentPart.UNKNOWN))
                {
                    //this.t判定にあわせてゲージを増減する( screenmode, pChip.eInstrumentPart, eJudgeResult );
                    actGauge.Damage(screenmode, pChip.eInstrumentPart, eJudgeResult);
                }
            }
            else if (CDTXMania.ConfigIni.bAutoAddGage == true)
            {
                if ((pChip.eInstrumentPart != EInstrumentPart.UNKNOWN))
                {
                    actGauge.Damage(screenmode, pChip.eInstrumentPart, eJudgeResult);
                }
            }

            switch (pChip.eInstrumentPart)
            {
                case EInstrumentPart.DRUMS:
                    switch (eJudgeResult)
                    {
                        #region[ ヒット数の加算 ]
                        case EJudgement.Miss:
                        case EJudgement.Bad:
                            this.actCombo.tコンボリセット処理();
                            this.nHitCount_IncAuto.Drums.Miss++;
                            if (!bPChipIsAutoPlay)
                            {
                                this.nHitCount_ExclAuto.Drums.Miss++;
                                this.actPlayInfo.nMISS数++;
                            }
                            break;
                        case EJudgement.Poor:
                            this.nHitCount_IncAuto.Drums.Poor++;
                            if (!bPChipIsAutoPlay)
                            {
                                this.nHitCount_ExclAuto.Drums.Poor++;
                                this.actPlayInfo.nPOOR数++;
                            }
                            break;
                        case EJudgement.Good:
                            this.nHitCount_IncAuto.Drums.Good++;
                            if (!bPChipIsAutoPlay)
                            {
                                this.nHitCount_ExclAuto.Drums.Good++;
                                this.actPlayInfo.nGOOD数++;
                            }
                            break;
                        case EJudgement.Great:
                            this.nHitCount_IncAuto.Drums.Great++;
                            if (!bPChipIsAutoPlay)
                            {
                                this.nHitCount_ExclAuto.Drums.Great++;
                                this.actPlayInfo.nGREAT数++;
                            }
                            break;
                        case EJudgement.Perfect:
                            this.nHitCount_IncAuto.Drums.Perfect++;
                            if (!bPChipIsAutoPlay)
                            {
                                this.nHitCount_ExclAuto.Drums.Perfect++;
                                this.actPlayInfo.nPERFECT数++;
                            }
                            break;
                        case EJudgement.Auto:
                            break;
                        #endregion
                    }

                    if (CDTXMania.ConfigIni.bAllDrumsAreAutoPlay || !bPChipIsAutoPlay)
                    {
                        switch (eJudgeResult)
                        {
                            case EJudgement.Perfect:
                            case EJudgement.Great:
                            case EJudgement.Good:
                                this.actCombo.nCurrentCombo.Drums++;
                                this.actCombo.tComboAnime(EInstrumentPart.DRUMS);
                                break;

                            default:
                                this.actCombo.nCurrentCombo.Drums = 0;
                                break;
                        }
                    }

                    if( eJudgeResult == EJudgement.Great || eJudgeResult == EJudgement.Perfect || eJudgeResult == EJudgement.Auto )
                        CDTXMania.stagePerfDrumsScreen.tボーナスチップのヒット処理( CDTXMania.ConfigIni, CDTXMania.DTX, pChip );
                    break;

                case EInstrumentPart.GUITAR:
                case EInstrumentPart.BASS:
                    int indexInst = (int)pChip.eInstrumentPart;
                    switch (eJudgeResult)
                    {
                        case EJudgement.Miss:
                        case EJudgement.Bad:
                            this.nHitCount_IncAuto[indexInst].Miss++;
                            if (!bPChipIsAutoPlay)
                            {
                                this.nHitCount_ExclAuto[indexInst].Miss++;
                            }
                            break;
                        default:	// #24068 2011.1.10 ikanick changed
                            // #24167 2011.1.16 yyagi changed
                            this.nHitCount_IncAuto[indexInst][(int)eJudgeResult]++;
                            if (!bPChipIsAutoPlay)
                            {
                                this.nHitCount_ExclAuto[indexInst][(int)eJudgeResult]++;
                            }
                            break;
                    }
                    switch (eJudgeResult)
                    {
                        case EJudgement.Perfect:
                        case EJudgement.Great:
                        case EJudgement.Good:
                            this.actCombo.nCurrentCombo[indexInst]++;
                            break;

                        default:
                            this.actCombo.nCurrentCombo[indexInst] = 0;
                            break;
                    }
                    break;

                case EInstrumentPart.UNKNOWN:
                    if (pChip.nChannelNumber == 0x4F)
                    {
                        switch (eJudgeResult)
                        {
                            case EJudgement.Perfect:
                            case EJudgement.Great:
                                {
                                }
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
            for (int i = 0; i < 3; i++)
            {
                if (this.ctTimer[i] != null)
                {
                    if (!this.ctTimer[i].b停止中)
                    {
                        if (this.ctTimer[i].bReachedEndValue)
                        {
                            this.bブーストボーナス = false;
                            this.ctTimer[i].tStop();

                        }
                    }
                }
            }
            #region[スコア]
            //!bPChipIsAutoPlayを入れるとオート時にスコアを加算しなくなる。
            if (CDTXMania.ConfigIni.nSkillMode == 1)
            {
                int nRate = this.bブーストボーナス == true ? 2 : 1;
                if (CDTXMania.ConfigIni.bAutoAddGage == true)
                {
                    if (((pChip.eInstrumentPart == EInstrumentPart.DRUMS)) && (eJudgeResult != EJudgement.Miss) && (eJudgeResult != EJudgement.Bad))
                    {
                        #region[ドラム]
                        int nCombos = this.actCombo.nCurrentCombo.Drums;
                        float nScoreDelta = 0;
                        float nComboMax;
                        nComboMax = CDTXMania.DTX.nVisibleChipsCount.Drums;
                        if (eJudgeResult == EJudgement.Perfect)//ここでパフェ基準を作成。
                        {
                            if (nCombos < nComboMax)
                            {
                                nScoreDelta = (1000000.0f - 500.0f * CDTXMania.DTX.nボーナスチップ数) / (1275.0f + 50.0f * (nComboMax - 50f));
                            }
                            //1000000÷50÷(その曲のMAXCOMBO-24.5)
                            else if (this.nHitCount_IncAuto.Drums.Perfect >= nComboMax)
                            {
                                nScoreDelta = 1000000.0f - (float)this.actScore.nCurrentTrueScore.Drums;
                                //nScoreDelta = (1000000.0f - 500.0f * CDTXMania.DTX.nボーナスチップ数 - (1000000.0f - 500.0f * CDTXMania.DTX.nボーナスチップ数) / (1275.0f + 50.0f * (nComboMax - 50f)) * ((1275.0f + 50.0f * (nComboMax - 49.0f))));
                            }
                            //1000000-PERFECT基準値×50×(その曲のMAXCOMBO-25.5)

                        }
                        else if (eJudgeResult == EJudgement.Great)
                        {
                            nScoreDelta = ((1000000.0f - 500.0f * CDTXMania.DTX.nボーナスチップ数) / (1275.0f + 50.0f * (nComboMax - 50f))) * 0.5f;
                        }
                        else if (eJudgeResult == EJudgement.Good)
                        {
                            nScoreDelta = ((1000000.0f - 500.0f * CDTXMania.DTX.nボーナスチップ数) / (1275.0f + 50.0f * (nComboMax - 50f))) * 0.2f;
                        }

                        if (nCombos < 50)
                        {
                            nScoreDelta = nScoreDelta * nCombos;
                        }
                        else if (nCombos == nComboMax || this.nHitCount_ExclAuto.Drums.Perfect == nComboMax)
                        {
                        }
                        else
                        {
                            nScoreDelta = nScoreDelta * 50;
                        }

                        this.actScore.Add(pChip.eInstrumentPart, bIsAutoPlay, (long)nScoreDelta);
                        this.actStatusPanel.nCurrentScore += (long)nScoreDelta;
                        #endregion
                    }
                    else if (((pChip.eInstrumentPart == EInstrumentPart.GUITAR) || pChip.eInstrumentPart == EInstrumentPart.BASS) && (eJudgeResult != EJudgement.Miss) && (eJudgeResult != EJudgement.Bad))
                    {
                        #region[ ギター&ベース ]
                        int nCombos = this.actCombo.nCurrentCombo[(int)pChip.eInstrumentPart];
                        float nScoreDelta = 0;
                        float nComboMax = (pChip.eInstrumentPart == EInstrumentPart.GUITAR ? CDTXMania.DTX.nVisibleChipsCount.Guitar : CDTXMania.DTX.nVisibleChipsCount.Bass);
                        if (eJudgeResult == EJudgement.Perfect || eJudgeResult == EJudgement.Auto)//ここでパフェ基準を作成。
                        {
                            if (nCombos < nComboMax)
                            {
                                nScoreDelta = 1000000.0f / (1275.0f + 50.0f * (nComboMax - 50.0f));
                            }
                            // 100万/{1275+50×(総ノーツ数-50)}
                            else if (this.nHitCount_IncAuto[(int)pChip.eInstrumentPart].Perfect >= nComboMax)
                            {
                                nScoreDelta = 1000000.0f - (float)this.actScore.nCurrentTrueScore[(int)pChip.eInstrumentPart];
                                //nScoreDelta = 1000000.0f - (1000000.0f / (1275.0f + 50.0f / (nComboMax - 50.0f))) * ((1275.0f + 50.0f * (nComboMax - 49.0f)));
                            }
                            //1000000-PERFECT基準値×50×(その曲のMAXCOMBO-25.5)

                        }
                        else if (eJudgeResult == EJudgement.Great)
                        {
                            nScoreDelta = 1000000.0f / (1275.0f + 50.0f * (nComboMax - 50.0f)) * 0.5f;
                        }
                        else if (eJudgeResult == EJudgement.Good)
                        {
                            nScoreDelta = 1000000.0f / (1275.0f + 50.0f * (nComboMax - 50.0f)) * 0.2f;
                        }


                        if (nCombos < 50)
                        {
                            nScoreDelta = nScoreDelta * nCombos;
                        }
                        else if (nCombos == nComboMax || this.nHitCount_ExclAuto[(int)pChip.eInstrumentPart].Perfect == nComboMax)
                        {
                        }
                        else
                        {
                            nScoreDelta = nScoreDelta * 50.0f;
                        }

                        this.actScore.Add(pChip.eInstrumentPart, bIsAutoPlay, (long)nScoreDelta);
                        //this.actStatusPanel.nCurrentScore += (long)nScoreDelta;
                        #endregion
                    }
                    else
                    {
                    }
                    //return eJudgeResult;
                }
                else
                {
                    if ((!bPChipIsAutoPlay && (pChip.eInstrumentPart != EInstrumentPart.UNKNOWN)) && (eJudgeResult != EJudgement.Miss) && (eJudgeResult != EJudgement.Bad))
                    {
                        #region[ドラム]
                        int nCombos = this.actCombo.nCurrentCombo.Drums;
                        float nScoreDelta = 0;
                        float nComboMax;
                        nComboMax = CDTXMania.DTX.nVisibleChipsCount.Drums;
                        if (eJudgeResult == EJudgement.Perfect)//ここでパフェ基準を作成。
                        {
                            if (nCombos < nComboMax)
                            {
                                nScoreDelta = (1000000.0f - 500.0f * CDTXMania.DTX.nボーナスチップ数) / (1275.0f + 50.0f * (nComboMax - 50f));
                            }
                            //1000000÷50÷(その曲のMAXCOMBO-24.5)
                            else if (this.nHitCount_IncAuto.Drums.Perfect >= nComboMax)
                            {
                                nScoreDelta = 1000000.0f - (float)this.actScore.nCurrentTrueScore.Drums;
                            }
                            //1000000-PERFECT基準値×50×(その曲のMAXCOMBO-25.5)

                        }
                        else if (eJudgeResult == EJudgement.Great)
                        {
                            nScoreDelta = ((1000000.0f - 500.0f * CDTXMania.DTX.nボーナスチップ数) / (1275.0f + 50.0f * (nComboMax - 50.0f))) * 0.5f;
                        }
                        else if (eJudgeResult == EJudgement.Good)
                        {
                            nScoreDelta = ((1000000.0f - 500.0f * CDTXMania.DTX.nボーナスチップ数) / (1275.0f + 50.0f * (nComboMax - 50.0f))) * 0.2f;
                        }

                        if (nCombos < 50)
                        {
                            nScoreDelta = nScoreDelta * nCombos;
                        }
                        else if (nCombos == nComboMax || this.nHitCount_ExclAuto.Drums.Perfect == nComboMax)
                        {
                        }
                        else
                        {
                            nScoreDelta = nScoreDelta * 50;
                        }

                        this.actScore.Add(pChip.eInstrumentPart, bIsAutoPlay, (long)nScoreDelta);
                        this.actStatusPanel.nCurrentScore += (long)nScoreDelta;
                        #endregion
                    }
                    else if (((pChip.eInstrumentPart == EInstrumentPart.GUITAR) || pChip.eInstrumentPart == EInstrumentPart.BASS) && (eJudgeResult != EJudgement.Miss) && (eJudgeResult != EJudgement.Bad))
                    {
                        #region[ ギター&ベース ]
                        int nCombos = this.actCombo.nCurrentCombo[(int)pChip.eInstrumentPart];
                        float nScoreDelta = 0;
                        float nComboMax = (pChip.eInstrumentPart == EInstrumentPart.GUITAR ? CDTXMania.DTX.nVisibleChipsCount.Guitar : CDTXMania.DTX.nVisibleChipsCount.Bass);
                        if (eJudgeResult == EJudgement.Perfect || eJudgeResult == EJudgement.Auto)//ここでパフェ基準を作成。
                        {
                            if (nCombos < nComboMax)
                            {
                                nScoreDelta = 1000000.0f / (1275.0f + 50.0f * (nComboMax - 50.0f));
                            }
                            // 100万/{1275+50×(総ノーツ数-50)}
                            else if (this.nHitCount_IncAuto[(int)pChip.eInstrumentPart].Perfect >= nComboMax)
                            {
                                nScoreDelta = 1000000.0f - (float)this.actScore.nCurrentTrueScore[(int)pChip.eInstrumentPart];
                                //nScoreDelta = 1000000.0f - (1000000.0f / (1275.0f + 50.0f / (nComboMax - 50.0f))) * ((1275.0f + 50.0f * (nComboMax - 49.0f)));
                            }
                            //1000000-PERFECT基準値×50×(その曲のMAXCOMBO-25.5)

                        }
                        else if (eJudgeResult == EJudgement.Great)
                        {
                            nScoreDelta = 1000000.0f / (1275.0f + 50.0f * (nComboMax - 50.0f)) * 0.5f;
                        }
                        else if (eJudgeResult == EJudgement.Good)
                        {
                            nScoreDelta = 1000000.0f / (1275.0f + 50.0f * (nComboMax - 50.0f)) * 0.2f;
                        }


                        if (nCombos < 50)
                        {
                            nScoreDelta = nScoreDelta * nCombos;
                        }
                        else if (nCombos == nComboMax || this.nHitCount_ExclAuto[(int)pChip.eInstrumentPart].Perfect == nComboMax)
                        {
                        }
                        else
                        {
                            nScoreDelta = nScoreDelta * 50.0f;
                        }

                        this.actScore.Add(pChip.eInstrumentPart, bIsAutoPlay, (long)nScoreDelta);
                        //this.actStatusPanel.nCurrentScore += (long)nScoreDelta;
                        #endregion
                    }
                    else
                    {
                    }
                }
            }
            else if (CDTXMania.ConfigIni.nSkillMode == 0)
            {
                if (CDTXMania.ConfigIni.bAutoAddGage)
                {
                    if (((pChip.eInstrumentPart != EInstrumentPart.UNKNOWN)) && (eJudgeResult != EJudgement.Miss) && (eJudgeResult != EJudgement.Bad))
                    {
                        int nCombos = this.actCombo.nCurrentCombo[(int)pChip.eInstrumentPart];
                        float nScoreDelta = 0;
                        long[] nComboScoreDelta = new long[] { 350L, 200L, 50L, 0L };
                        if ((nCombos <= 500) || (eJudgeResult == EJudgement.Good))
                        {
                            nScoreDelta = nComboScoreDelta[(int)eJudgeResult] * nCombos;
                        }
                        else if ((eJudgeResult == EJudgement.Perfect))
                        {
                            nScoreDelta = nComboScoreDelta[(int)eJudgeResult] * 500L;
                        }
                        this.actScore.Add(pChip.eInstrumentPart, bIsAutoPlay, (long)nScoreDelta);
                        this.actStatusPanel.nCurrentScore += (long)nScoreDelta;
                    }
                }
                else
                {
                    if ((!bPChipIsAutoPlay && (pChip.eInstrumentPart != EInstrumentPart.UNKNOWN)) && (eJudgeResult != EJudgement.Miss) && (eJudgeResult != EJudgement.Bad))
                    {
                        int nCombos = this.actCombo.nCurrentCombo[(int)pChip.eInstrumentPart];
                        float nScoreDelta = 0;
                        long[] nComboScoreDelta = new long[] { 350L, 200L, 50L, 0L };
                        if ((nCombos <= 500) || (eJudgeResult == EJudgement.Good))
                        {
                            nScoreDelta = nComboScoreDelta[(int)eJudgeResult] * nCombos;
                        }
                        else if ((eJudgeResult == EJudgement.Perfect))
                        {
                            nScoreDelta = nComboScoreDelta[(int)eJudgeResult] * 500L;
                        }
                        this.actScore.Add(pChip.eInstrumentPart, bIsAutoPlay, (long)nScoreDelta);
                        this.actStatusPanel.nCurrentScore += (long)nScoreDelta;
                    }
                }
            }
            #endregion

            return eJudgeResult;
        }
        protected abstract void tチップのヒット処理_BadならびにTight時のMiss(EInstrumentPart part);
        protected abstract void tチップのヒット処理_BadならびにTight時のMiss(EInstrumentPart part, int nLane);
        protected void tチップのヒット処理_BadならびにTight時のMiss(EInstrumentPart part, EInstrumentPart screenmode)
        {
            this.tチップのヒット処理_BadならびにTight時のMiss(part, 0, screenmode);
        }
        protected void tチップのヒット処理_BadならびにTight時のMiss(EInstrumentPart part, int nLane, EInstrumentPart screenmode)
        {
            this.bAUTOでないチップが１つでもバーを通過した = true;
            //this.t判定にあわせてゲージを増減する( screenmode, part, EJudgement.Miss );
            actGauge.Damage(screenmode, part, EJudgement.Miss);
            switch (part)
            {
                case EInstrumentPart.DRUMS:
                    if ((nLane >= 0) && (nLane <= 10))
                    {
                        this.actJudgeString.Start(nLane, bIsAutoPlay[nLane] ? EJudgement.Auto : EJudgement.Miss, 999);
                    }
                    this.actCombo.nCurrentCombo.Drums = 0;
                    return;

                case EInstrumentPart.GUITAR:
                    this.actJudgeString.Start(13, EJudgement.Bad, 999);
                    this.actCombo.nCurrentCombo.Guitar = 0;
                    return;

                case EInstrumentPart.BASS:
                    this.actJudgeString.Start(14, EJudgement.Bad, 999);
                    this.actCombo.nCurrentCombo.Bass = 0;
                    break;

                default:
                    return;
            }
        }

        protected CDTX.CChip r指定時刻に一番近い未ヒットChip(long nTime, int nChannelFlag, int nInputAdjustTime)
        {
            return this.r指定時刻に一番近い未ヒットChip(nTime, nChannelFlag, nInputAdjustTime, 0);
        }
        protected CDTX.CChip r指定時刻に一番近い未ヒットChip(long nTime, int nChannel, int nInputAdjustTime, int n検索範囲時間ms)
        {
            sw2.Start();
            //Trace.TraceInformation( "nTime={0}, nChannel={1:x2}, 現在のTop={2}", nTime, nChannel,CDTXMania.DTX.listChip[ this.nCurrentTopChip ].nPlaybackTimeMs );
            nTime += nInputAdjustTime;

            int nIndex_InitialPositionSearchingToPast;
            int nTimeDiff;
            if (this.nCurrentTopChip == -1)			// 演奏データとして1個もチップがない場合は
            {
                sw2.Stop();
                return null;
            }
            int count = listChip.Count;
            int nIndex_NearestChip_Future = nIndex_InitialPositionSearchingToPast = this.nCurrentTopChip;
            if (this.nCurrentTopChip >= count)		// その時点で演奏すべきチップが既に全部無くなっていたら
            {
                nIndex_NearestChip_Future = nIndex_InitialPositionSearchingToPast = count - 1;
            }
            // int nIndex_NearestChip_Future = nIndex_InitialPositionSearchingToFuture;
            //			while ( nIndex_NearestChip_Future < count )	// 未来方向への検索
            for (; nIndex_NearestChip_Future < count; nIndex_NearestChip_Future++)
            {
                CDTX.CChip chip = listChip[nIndex_NearestChip_Future];
                if (!chip.bHit)
                {
                    if ((0x11 <= nChannel) && (nChannel <= 0x1c))
                    {
                        if ((chip.nChannelNumber == nChannel) || (chip.nChannelNumber == (nChannel + 0x20)))
                        {
                            if (chip.nPlaybackTimeMs > nTime)
                            {
                                break;
                            }
                            nIndex_InitialPositionSearchingToPast = nIndex_NearestChip_Future;
                        }
                        continue;
                    }
                    else if ((nChannel == 0x2F && chip.eInstrumentPart == EInstrumentPart.GUITAR) || (((0x20 <= nChannel && nChannel <= 0x28) || (0x93 <= nChannel && nChannel <= 0x9F) || (0xA9 <= nChannel && nChannel <= 0xAF) || (0xD0 <= nChannel && nChannel <= 0xD3)) && chip.nChannelNumber == nChannel))
                    {
                        if (chip.nPlaybackTimeMs > nTime)
                        {
                            break;
                        }
                        nIndex_InitialPositionSearchingToPast = nIndex_NearestChip_Future;
                    }
                    else if ((nChannel == 0xAF && chip.eInstrumentPart == EInstrumentPart.BASS) || (((nChannel >= 0xA0 && nChannel <= 0xA8) || (0xC5 == nChannel) || (nChannel == 0xC6) || (nChannel >= 0xC8 && nChannel <= 0xCF) || (nChannel >= 0xDA && nChannel <= 0xDF) || (nChannel >= 0xE1 && nChannel <= 0xE8)) && chip.nChannelNumber == nChannel))
                    {
                        if (chip.nPlaybackTimeMs > nTime)
                        {
                            break;
                        }
                        nIndex_InitialPositionSearchingToPast = nIndex_NearestChip_Future;
                    }
                }
                //				nIndex_NearestChip_Future++;
            }
            int nIndex_NearestChip_Past = nIndex_InitialPositionSearchingToPast;
            //			while ( nIndex_NearestChip_Past >= 0 )		// 過去方向への検索
            for (; nIndex_NearestChip_Past >= 0; nIndex_NearestChip_Past--)
            {
                CDTX.CChip chip = listChip[nIndex_NearestChip_Past];
                if ((!chip.bHit) &&
                        (
                            ((nChannel >= 0x11) && (nChannel <= 0x1c) &&
                                ((chip.nChannelNumber == nChannel) || (chip.nChannelNumber == (nChannel + 0x20)))
                            )
                            ||
                            (
                                ((nChannel == 0x2f) && (chip.eInstrumentPart == EInstrumentPart.GUITAR)) ||
                                (((nChannel >= 0x20) && (nChannel <= 0x28)) ||
                                (0x93 <= nChannel && nChannel <= 0x9F) ||
                                (0xA9 <= nChannel && nChannel <= 0xAF) ||
                                (0xD0 <= nChannel && nChannel <= 0xD3)
                                && (chip.nChannelNumber == nChannel))
                            )
                            ||
                            (
                                ((nChannel == 0xaf) && (chip.eInstrumentPart == EInstrumentPart.BASS)) ||
                                (((nChannel >= 0xA0) && (nChannel <= 0xa8)) ||
                                (0xC5 <= nChannel && nChannel <= 0xC6) ||
                                (0xC8 <= nChannel && nChannel <= 0xCF) ||
                                (0xDA <= nChannel && nChannel <= 0xDF) ||
                                (0xE1 <= nChannel && nChannel <= 0xE8)
                                && (chip.nChannelNumber == nChannel))
                            )
                        )
                    )
                {
                    break;
                }
                //				nIndex_NearestChip_Past--;
            }
            if ((nIndex_NearestChip_Future >= count) && (nIndex_NearestChip_Past < 0))	// 検索対象が過去未来どちらにも見つからなかった場合
            {
                sw2.Stop();
                return null;
            }
            CDTX.CChip nearestChip;	// = null;	// 以下のifブロックのいずれかで必ずnearestChipには非nullが代入されるので、null初期化を削除
            if (nIndex_NearestChip_Future >= count)											// 検索対象が未来方向には見つからなかった(しかし過去方向には見つかった)場合
            {
                nearestChip = listChip[nIndex_NearestChip_Past];
                //				nTimeDiff = Math.Abs( (int) ( nTime - nearestChip.nPlaybackTimeMs ) );
            }
            else if (nIndex_NearestChip_Past < 0)												// 検索対象が過去方向には見つからなかった(しかし未来方向には見つかった)場合
            {
                nearestChip = listChip[nIndex_NearestChip_Future];
                //				nTimeDiff = Math.Abs( (int) ( nTime - nearestChip.nPlaybackTimeMs ) );
            }
            else
            {
                int nTimeDiff_Future = Math.Abs((int)(nTime - listChip[nIndex_NearestChip_Future].nPlaybackTimeMs));
                int nTimeDiff_Past = Math.Abs((int)(nTime - listChip[nIndex_NearestChip_Past].nPlaybackTimeMs));
                if (nTimeDiff_Future < nTimeDiff_Past)
                {
                    nearestChip = listChip[nIndex_NearestChip_Future];
                    //					nTimeDiff = Math.Abs( (int) ( nTime - nearestChip.nPlaybackTimeMs ) );
                }
                else
                {
                    nearestChip = listChip[nIndex_NearestChip_Past];
                    //					nTimeDiff = Math.Abs( (int) ( nTime - nearestChip.nPlaybackTimeMs ) );
                }
            }
            nTimeDiff = Math.Abs((int)(nTime - nearestChip.nPlaybackTimeMs));
            if ((n検索範囲時間ms > 0) && (nTimeDiff > n検索範囲時間ms))					// チップは見つかったが、検索範囲時間外だった場合
            {
                sw2.Stop();
                return null;
            }
            sw2.Stop();
            return nearestChip;
        }

        protected CDTX.CChip r次に来る指定楽器Chipを更新して返す(EInstrumentPart inst)
        {
            switch ((int)inst)
            {
                case (int)EInstrumentPart.GUITAR:
                    return r次にくるギターChipを更新して返す();
                case (int)EInstrumentPart.BASS:
                    return r次にくるベースChipを更新して返す();
                default:
                    return null;
            }
        }
        protected CDTX.CChip r次にくるギターChipを更新して返す()
        {
            int nInputAdjustTime = this.bIsAutoPlay.GtPick ? 0 : this.nInputAdjustTimeMs.Guitar;
            this.rNextGuitarChip = this.r指定時刻に一番近い未ヒットChip(CSoundManager.rcPerformanceTimer.nCurrentTime, 0x2f, nInputAdjustTime, 500);
            return this.rNextGuitarChip;
        }
        protected CDTX.CChip r次にくるベースChipを更新して返す()
        {
            int nInputAdjustTime = this.bIsAutoPlay.BsPick ? 0 : this.nInputAdjustTimeMs.Bass;
            this.rNextBassChip = this.r指定時刻に一番近い未ヒットChip(CSoundManager.rcPerformanceTimer.nCurrentTime, 0xaf, nInputAdjustTime, 500);
            return this.rNextBassChip;
        }

        protected void ChangeInputAdjustTimeInPlaying(IInputDevice keyboard, int plusminus)		// #23580 2011.1.16 yyagi UI for InputAdjustTime in playing screen.
        {
            int part, offset = plusminus;
            if (keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.LeftShift) || keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.RightShift))	// Guitar InputAdjustTime
            {
                part = (int)EInstrumentPart.GUITAR;
            }
            else if (keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.LeftAlt) || keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.RightAlt))	// Bass InputAdjustTime
            {
                part = (int)EInstrumentPart.BASS;
            }
            else	// Drums InputAdjustTime
            {
                part = (int)EInstrumentPart.DRUMS;
            }
            if (!keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.LeftControl) && !keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.RightControl))
            {
                offset *= 10;
            }

            this.nInputAdjustTimeMs[part] += offset;
            if (this.nInputAdjustTimeMs[part] > 99)
            {
                this.nInputAdjustTimeMs[part] = 99;
            }
            else if (this.nInputAdjustTimeMs[part] < -99)
            {
                this.nInputAdjustTimeMs[part] = -99;
            }
            CDTXMania.ConfigIni.nInputAdjustTimeMs[part] = this.nInputAdjustTimeMs[part];
        }

        protected abstract void tHandleInput_Drums();
        protected abstract void ScrollSpeedUp();
        protected abstract void ScrollSpeedDown();
        protected void tHandleKeyInput()
        {
            IInputDevice keyboard = CDTXMania.InputManager.Keyboard;
            if (CDTXMania.Pad.bPressed(EInstrumentPart.BASS, EPad.Help))
            {	// shift+f1 (pause)
                this.bPAUSE = !this.bPAUSE;
                if (this.bPAUSE)
                {
                    CSoundManager.rcPerformanceTimer.tPause();
                    CDTXMania.Timer.tPause();
                    CDTXMania.DTX.tPausePlaybackForAllChips();
                }
                else
                {
                    CSoundManager.rcPerformanceTimer.tResume();
                    CDTXMania.Timer.tResume();
                    CDTXMania.DTX.tResumePlaybackForAllChips();
                }
            }
            if (((base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED)) && (base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED_フェードアウト))
            {
                if (!this.bPAUSE)
                {
                    this.tHandleInput_Drums();
                    this.tHandleInput_GuitarBass(EInstrumentPart.GUITAR);
                    this.tHandleInput_GuitarBass(EInstrumentPart.BASS);
                }
                if (!this.bPAUSE && keyboard.bKeyPressed((int)SlimDX.DirectInput.Key.UpArrow) && (keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.RightShift) || keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.LeftShift)))
                {	// shift (+ctrl) + UpArrow (BGMAdjust)
                    CDTXMania.DTX.t各自動再生音チップの再生時刻を変更する((keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.LeftControl) || keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.RightControl)) ? 1 : 10);
                    CDTXMania.DTX.tAutoCorrectWavPlaybackPosition();
                }
                else if (!this.bPAUSE && keyboard.bKeyPressed((int)SlimDX.DirectInput.Key.DownArrow) && (keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.RightShift) || keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.LeftShift)))
                {	// shift + DownArrow (BGMAdjust)
                    CDTXMania.DTX.t各自動再生音チップの再生時刻を変更する((keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.LeftControl) || keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.RightControl)) ? -1 : -10);
                    CDTXMania.DTX.tAutoCorrectWavPlaybackPosition();
                }
                else if (keyboard.bKeyPressed((int)SlimDX.DirectInput.Key.UpArrow))
                {	// UpArrow(scrollspeed up)
                    ScrollSpeedUp();
                }
                else if (keyboard.bKeyPressed((int)SlimDX.DirectInput.Key.DownArrow))
                {	// DownArrow (scrollspeed down)
                    ScrollSpeedDown();
                }

                else if (CDTXMania.Pad.bPressed(EInstrumentPart.GUITAR, EPad.Help))
                {	// del (debug info)
                    CDTXMania.ConfigIni.b演奏情報を表示する = !CDTXMania.ConfigIni.b演奏情報を表示する;
                }
                else if (!this.bPAUSE && keyboard.bKeyPressed((int)SlimDX.DirectInput.Key.LeftArrow))		// #24243 2011.1.16 yyagi UI for InputAdjustTime in playing screen.
                {
                    ChangeInputAdjustTimeInPlaying(keyboard, -1);
                }
                else if (!this.bPAUSE && keyboard.bKeyPressed((int)SlimDX.DirectInput.Key.RightArrow))		// #24243 2011.1.16 yyagi UI for InputAdjustTime in playing screen.
                {
                    ChangeInputAdjustTimeInPlaying(keyboard, +1);
                }
                else if (!this.bPAUSE && (base.ePhaseID == CStage.EPhase.Common_DefaultState) && (keyboard.bKeyPressed((int)SlimDX.DirectInput.Key.Escape)))
                {	// escape (exit)
                    this.actFO.tStartFadeOut();
                    base.ePhaseID = CStage.EPhase.Common_FadeOut;
                    this.eReturnValueAfterFadeOut = EPerfScreenReturnValue.Interruption;
                }

                if (!CDTXMania.ConfigIni.bReverse.Drums && keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.PageUp))
                {
                    if (!this.sw.IsRunning)
                    {
                        this.sw2 = Stopwatch.StartNew();
                        this.sw = Stopwatch.StartNew();
                        if (this.nJudgeLinePosY.Drums > nJudgeLineMinPosY)
                        {
                            this.nJudgeLinePosY.Drums--;
                        }
                    }
                    else
                    {
                        if (this.sw.ElapsedMilliseconds > 10L)
                        {
                            if (this.sw2.IsRunning)
                            {
                                if (this.sw2.ElapsedMilliseconds > 100L)
                                {
                                    this.sw2.Reset();
                                }
                            }
                            else if (this.nJudgeLinePosY.Drums > nJudgeLineMinPosY)
                            {
                                this.nJudgeLinePosY.Drums--;
                            }
                            this.sw.Reset();
                            this.sw.Start();
                        }
                    }
                    CDTXMania.ConfigIni.nJudgeLine.Drums = nJudgeLineMaxPosY - this.nJudgeLinePosY.Drums;
                    CDTXMania.stagePerfDrumsScreen.tJudgeLineMovingUpandDown();
                }
                if (!CDTXMania.ConfigIni.bReverse.Drums && keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.PageDown))
                {
                    if (!this.sw.IsRunning)
                    {
                        this.sw2 = Stopwatch.StartNew();
                        this.sw = Stopwatch.StartNew();
                        if (this.nJudgeLinePosY.Drums < nJudgeLineMaxPosY)
                        {
                            this.nJudgeLinePosY.Drums++;
                        }
                    }
                    else if (this.sw.ElapsedMilliseconds > 10L)
                    {
                        if (this.sw2.IsRunning)
                        {
                            if (this.sw2.ElapsedMilliseconds > 100L)
                            {
                                this.sw2.Reset();
                            }
                        }
                        else if (this.nJudgeLinePosY.Drums < nJudgeLineMaxPosY)
                        {
                            this.nJudgeLinePosY.Drums++;
                        }
                        this.sw.Reset();
                        this.sw.Start();
                    }
                    CDTXMania.ConfigIni.nJudgeLine.Drums = nJudgeLineMaxPosY - this.nJudgeLinePosY.Drums;
                    CDTXMania.stagePerfDrumsScreen.tJudgeLineMovingUpandDown();
                }

                if (keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.NumberPad8))
                {
                    if (!this.sw.IsRunning)
                    {
                        this.sw2 = Stopwatch.StartNew();
                        this.sw = Stopwatch.StartNew();
                        if (this.nShutterInPosY.Drums > 0)
                        {
                            this.nShutterInPosY.Drums--;
                        }
                    }
                    else if (this.sw.ElapsedMilliseconds > 10L)
                    {
                        if (this.sw2.IsRunning)
                        {
                            if (this.sw2.ElapsedMilliseconds > 100L)
                            {
                                this.sw2.Reset();
                            }
                        }
                        else if (this.nShutterInPosY.Drums > 0)
                        {
                            this.nShutterInPosY.Drums--;
                        }
                        this.sw.Reset();
                        this.sw.Start();
                    }
                    CDTXMania.ConfigIni.nShutterInSide.Drums = this.nShutterInPosY.Drums;
                }
                if (keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.NumberPad2))
                {
                    if (!this.sw.IsRunning)
                    {
                        this.sw2 = Stopwatch.StartNew();
                        this.sw = Stopwatch.StartNew();
                        if (this.nShutterInPosY.Drums < 100)
                        {
                            if (this.nShutterInPosY.Drums + this.nShutterOutPosY.Drums <= 99)
                                this.nShutterInPosY.Drums++;
                        }
                    }
                    else if (this.sw.ElapsedMilliseconds > 10L)
                    {
                        if (this.sw2.IsRunning)
                        {
                            if (this.sw2.ElapsedMilliseconds > 100L)
                            {
                                this.sw2.Reset();
                            }
                        }
                        else if (this.nShutterInPosY.Drums < 100)
                        {
                            if (this.nShutterInPosY.Drums + this.nShutterOutPosY.Drums <= 99)
                                this.nShutterInPosY.Drums++;
                        }
                        this.sw.Reset();
                        this.sw.Start();
                    }
                    CDTXMania.ConfigIni.nShutterInSide = this.nShutterInPosY;
                }
                if (keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.NumberPad4))
                {
                    if (!this.sw.IsRunning)
                    {
                        this.sw2 = Stopwatch.StartNew();
                        this.sw = Stopwatch.StartNew();
                        if (this.nShutterOutPosY.Drums < 100)
                        {
                            if (this.nShutterInPosY.Drums + this.nShutterOutPosY.Drums <= 99)
                                this.nShutterOutPosY.Drums++;
                        }
                    }
                    else if (this.sw.ElapsedMilliseconds > 10L)
                    {
                        if (this.sw2.IsRunning)
                        {
                            if (this.sw2.ElapsedMilliseconds > 100L)
                            {
                                this.sw2.Reset();
                            }
                        }
                        else if (this.nShutterOutPosY.Drums < 100)
                        {
                            if (this.nShutterInPosY.Drums + this.nShutterOutPosY.Drums <= 99)
                                this.nShutterOutPosY.Drums++;
                        }
                        this.sw.Reset();
                        this.sw.Start();
                    }
                    CDTXMania.ConfigIni.nShutterOutSide.Drums = this.nShutterOutPosY.Drums;
                }
                if (keyboard.bKeyPressing((int)SlimDX.DirectInput.Key.NumberPad6))
                {
                    if (!this.sw.IsRunning)
                    {
                        this.sw2 = Stopwatch.StartNew();
                        this.sw = Stopwatch.StartNew();
                        if (this.nShutterOutPosY.Drums > 0)
                        {
                            this.nShutterOutPosY.Drums--;
                        }
                    }
                    else if (this.sw.ElapsedMilliseconds > 10L)
                    {
                        if (this.sw2.IsRunning)
                        {
                            if (this.sw2.ElapsedMilliseconds > 100L)
                            {
                                this.sw2.Reset();
                            }
                        }
                        else if (this.nShutterOutPosY.Drums > 0)
                        {
                            this.nShutterOutPosY.Drums--;
                        }
                        this.sw.Reset();
                        this.sw.Start();
                    }
                    CDTXMania.ConfigIni.nShutterOutSide.Drums = this.nShutterOutPosY.Drums;
                }

                if (keyboard.bKeyPressed(0x3a))
                {
                    CConfigIni configIni = CDTXMania.ConfigIni;
                    configIni.nMovieMode++;
                    if (CDTXMania.ConfigIni.nMovieMode >= 4)
                    {
                        CDTXMania.ConfigIni.nMovieMode = 0;
                    }
                    this.actAVI.MovieMode();
                }
                if (keyboard.bKeyPressed(0x3b))
                {
                    CConfigIni configIni = CDTXMania.ConfigIni;
                    configIni.nInfoType++;
                    if (CDTXMania.ConfigIni.nInfoType >= 2)
                    {
                        CDTXMania.ConfigIni.nInfoType = 0;
                    }
                }
                if ((keyboard.bKeyPressing(0x3c)))
                {
                    //F7
                    //CDTXMania.stagePerfDrumsScreen.actGauge.db現在のゲージ値.Drums = 1.0;
                    //CDTXMania.stagePerfDrumsScreen.actGraph.dbグラフ値現在_渡 = 100.0;
                    //CDTXMania.ConfigIni.nヒット範囲ms.Perfect = 1000;
                }
                if (keyboard.bKeyPressed(0x3d))
                {
                    //F8キー

                }
            }
        }

        protected void tSaveInputMethod(EInstrumentPart part)
        {
            if (CDTXMania.Pad.stDetectedDevice.Keyboard)
            {
                this.bKeyboardUsed[(int)part] = true;
            }
            if (CDTXMania.Pad.stDetectedDevice.Joypad)
            {
                this.bJoypadUsed[(int)part] = true;
            }
            if (CDTXMania.Pad.stDetectedDevice.MIDIIN)
            {
                this.bMIDIUsed[(int)part] = true;
            }
            if (CDTXMania.Pad.stDetectedDevice.Mouse)
            {
                this.bMouseUsed[(int)part] = true;
            }
        }


//      protected abstract void tUpdateAndDraw_AVI();
        protected void tUpdateAndDraw_AVI()
        {
            if (((base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED) && (base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED_フェードアウト)) && (!CDTXMania.ConfigIni.bストイックモード))
            {
                this.actAVI.tUpdateAndDraw(0, 0);
            }
        }
        /*
        protected abstract void t進行描画_BGA();
        protected void t進行描画_BGA(int x, int y)
        {
            if (((base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED) && (base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED_フェードアウト)) && (CDTXMania.ConfigIni.bBGAEnabled))
            {
                this.actBGA.tUpdateAndDraw(x, y);
            }
        }
         */
        protected abstract void tUpdateAndDraw_DANGER();
        protected void tUpdateAndDraw_MIDIBGM()
        {
            if (base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED)
            {
                CStage.EPhase ePhaseid1 = base.ePhaseID;
            }
        }
        protected void tUpdateAndDraw_RGBButton()
        {
            this.actRGB.OnUpdateAndDraw();
        }
        protected void tUpdateAndDraw_STAGEFAILED()
        {
            if (((base.ePhaseID == CStage.EPhase.演奏_STAGE_FAILED) || (base.ePhaseID == CStage.EPhase.演奏_STAGE_FAILED_フェードアウト)) && ((this.actStageFailed.OnUpdateAndDraw() != 0) && (base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED_フェードアウト)))
            {
                this.eReturnValueAfterFadeOut = EPerfScreenReturnValue.StageFailure;
                base.ePhaseID = CStage.EPhase.演奏_STAGE_FAILED_フェードアウト;
                CDTXMania.DTX.tStopPlayingAllChips();
                this.actFO.tStartFadeOut();
            }
        }
        protected void tUpdateAndDraw_WailingBonus()
        {
            if ((base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED) && (base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED_フェードアウト))
            {
                this.actWailingBonus.OnUpdateAndDraw();
            }
        }
        protected abstract void tUpdateAndDraw_WailingFrame();
        protected void tUpdateAndDraw_WailingFrame(int GtWailingFrameX, int BsWailingFrameX, int GtWailingFrameY, int BsWailingFrameY)
        {
            if (this.txWailingFrame != null && CDTXMania.ConfigIni.bGuitarEnabled)
            {
                if (CDTXMania.DTX.bチップがある.Guitar)
                {
                    this.txWailingFrame.tDraw2D(CDTXMania.app.Device, GtWailingFrameX, GtWailingFrameY);
                }
                if (CDTXMania.DTX.bチップがある.Bass)
                {
                    this.txWailingFrame.tDraw2D(CDTXMania.app.Device, BsWailingFrameX, BsWailingFrameY);
                }
            }
        }


        protected void tUpdateAndDraw_ChipFireGB()
        {
            this.actChipFireGB.OnUpdateAndDraw();
        }

        /*
        protected abstract void t進行描画_パネル文字列();
        protected void t進行描画_パネル文字列()
        {
            if ((base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED) && (base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED_フェードアウト))
            {
                this.actPanel.tUpdateAndDraw();
            }
        }
        protected void tパネル文字列の設定()
        {
            this.actPanel.SetPanelString(string.IsNullOrEmpty(CDTXMania.DTX.PANEL) ? CDTXMania.stageSongSelection.rConfirmedSong.strタイトル : CDTXMania.DTX.PANEL);
        }
         */

        protected void tUpdateAndDraw_Gauge()
        {
            this.actGauge.OnUpdateAndDraw();
        }
        protected void tUpdateAndDraw_Combo()
        {
            this.actCombo.OnUpdateAndDraw();
        }
        protected void tUpdateAndDraw_Score()
        {
            this.actScore.OnUpdateAndDraw();
        }
        protected void tUpdateAndDraw_StatusPanel()
        {
            this.actStatusPanel.OnUpdateAndDraw();
        }
        protected bool tUpdateAndDraw_Chip(EInstrumentPart ePlayMode)
        {
            if ((base.ePhaseID == CStage.EPhase.演奏_STAGE_FAILED) || (base.ePhaseID == CStage.EPhase.演奏_STAGE_FAILED_フェードアウト))
            {
                return true;
            }
            if ((this.nCurrentTopChip == -1) || (this.nCurrentTopChip >= listChip.Count))
            {
                return true;
            }
            if (this.nCurrentTopChip == -1)
            {
                return true;
            }

            const double speed = 286;	// BPM150の時の1小節の長さ[dot]
            //XGのHS4.5が1289。思えばBPMじゃなくて拍の長さが関係あるよね。

            double ScrollSpeedDrums = (this.actScrollSpeed.db現在の譜面スクロール速度.Drums + 1.0) * 0.5 * 37.5 * speed / 60000.0;
            double ScrollSpeedGuitar = (this.actScrollSpeed.db現在の譜面スクロール速度.Guitar + 1.0) * 0.5 * 0.5 * 37.5 * speed / 60000.0;
            double ScrollSpeedBass = (this.actScrollSpeed.db現在の譜面スクロール速度.Bass + 1.0) * 0.5 * 0.5 * 37.5 * speed / 60000.0;

            CDTX dTX = CDTXMania.DTX;
            CConfigIni configIni = CDTXMania.ConfigIni;
            for (int nCurrentTopChip = this.nCurrentTopChip; nCurrentTopChip < dTX.listChip.Count; nCurrentTopChip++)
            {
                CDTX.CChip pChip = dTX.listChip[nCurrentTopChip];
                //Debug.WriteLine( "nCurrentTopChip=" + nCurrentTopChip + ", ch=" + pChip.nChannelNumber.ToString("x2") + ", 発音位置=" + pChip.nPlaybackPosition + ", 発声時刻ms=" + pChip.nPlaybackTimeMs );
                pChip.nDistanceFromBar.Drums = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedDrums);
                pChip.nDistanceFromBar.Guitar = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedGuitar);
                pChip.nDistanceFromBar.Bass = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedBass);
                if (Math.Min(Math.Min(pChip.nDistanceFromBar.Drums, pChip.nDistanceFromBar.Guitar), pChip.nDistanceFromBar.Bass) > 600)
                {
                    break;
                }
                //				if ( ( ( nCurrentTopChip == this.nCurrentTopChip ) && ( pChip.nDistanceFromBar.Drums < -65 ) ) && pChip.bHit )
                // #28026 2012.4.5 yyagi; 信心ワールドエンドの曲終了後リザルトになかなか行かない問題の修正
                if ((dTX.listChip[this.nCurrentTopChip].nDistanceFromBar.Drums < -65) && dTX.listChip[this.nCurrentTopChip].bHit)
                {
                    //					nCurrentTopChip = ++this.nCurrentTopChip;
                    ++this.nCurrentTopChip;
                    continue;
                }

                bool bPChipIsAutoPlay = bCheckAutoPlay(pChip);

                int nInputAdjustTime = (bPChipIsAutoPlay || (pChip.eInstrumentPart == EInstrumentPart.UNKNOWN)) ? 0 : this.nInputAdjustTimeMs[(int)pChip.eInstrumentPart];

				int instIndex = (int) pChip.eInstrumentPart;
				if ( ( ( pChip.eInstrumentPart != EInstrumentPart.UNKNOWN ) && !pChip.bHit ) &&
				    ( ( pChip.nDistanceFromBar[ instIndex ] < 0 ) && ( this.e指定時刻からChipのJUDGEを返す( CSoundManager.rcPerformanceTimer.nCurrentTime, pChip, nInputAdjustTime ) == EJudgement.Miss ) ) )
				{
				    this.tProcessChipHit( CSoundManager.rcPerformanceTimer.nCurrentTime, pChip );
				}

                // #35411 chnmr0 add (ターゲットゴースト)
                if ( CDTXMania.ConfigIni.eTargetGhost[instIndex] != ETargetGhostData.NONE &&
                     CDTXMania.listTargetGhsotLag[instIndex] != null &&
                     pChip.eInstrumentPart != EInstrumentPart.UNKNOWN &&
                     pChip.nDistanceFromBar[instIndex] < 0 )
                {
                    if ( !pChip.bTargetGhost判定済み )
                    {
                        pChip.bTargetGhost判定済み = true;

						int ghostLag = 128;
						if( 0 <= pChip.n楽器パートでの出現順 && pChip.n楽器パートでの出現順 < CDTXMania.listTargetGhsotLag[instIndex].Count )
                        {
                            ghostLag = CDTXMania.listTargetGhsotLag[instIndex][pChip.n楽器パートでの出現順];
							// 上位８ビットが１ならコンボが途切れている（ギターBAD空打ちでコンボ数を再現するための措置）
							if( ghostLag > 255 )
							{
								this.nコンボ数_TargetGhost[instIndex] = 0;
							}
							ghostLag = (ghostLag & 255) - 128;
						}
                        else if( CDTXMania.ConfigIni.eTargetGhost[instIndex] == ETargetGhostData.PERFECT )
                        {
                            ghostLag = 0;
                        }
                        
                        if ( ghostLag <= 127 )
                        {
                            EJudgement eJudge = this.e指定時刻からChipのJUDGEを返す(pChip.nPlaybackTimeMs + ghostLag, pChip, 0, false);
                            this.nヒット数_TargetGhost[instIndex][(int)eJudge]++;
                            if (eJudge == EJudgement.Miss || eJudge == EJudgement.Poor)
                            {
                                this.n最大コンボ数_TargetGhost[instIndex] = Math.Max(this.n最大コンボ数_TargetGhost[instIndex], this.nコンボ数_TargetGhost[instIndex]);
                                this.nコンボ数_TargetGhost[instIndex] = 0;
                            }
                            else
                            {
                                this.n最大コンボ数_TargetGhost[instIndex] = Math.Max(this.n最大コンボ数_TargetGhost[instIndex], this.nコンボ数_TargetGhost[instIndex]);
                                this.nコンボ数_TargetGhost[instIndex]++;
                            }
                        }
                    }
                }

                switch (pChip.nChannelNumber)
                {
                    //描画順の都合上こちらから描画。

                    #region [ 11-1c: Drums ]
                    case 0x11:	// ドラム演奏
                    case 0x12:
                    case 0x13:
                    case 0x14:
                    case 0x15:
                    case 0x16:
                    case 0x17:
                    case 0x18:
                    case 0x19:
                    case 0x1a:
                    case 0x1b:
                    case 0x1c:
                        this.tUpdateAndDraw_Chip_Drums(configIni, ref dTX, ref pChip);
                        break;
                    #endregion

                    #region [ 01: BGM ]
                    case 0x01:	// BGM
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            if (configIni.bBGM音を発声する)
                            {
                                dTX.tPlayChip(pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs, (int)ELane.BGM, dTX.nモニタを考慮した音量(EInstrumentPart.UNKNOWN));
                            }
                        }
                        break;
                    #endregion
                    #region [ 03: BPM変更 ]
                    case 0x03:	// BPM変更
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            this.actPlayInfo.dbBPM = (pChip.nIntegerValue * (((double)configIni.nPlaySpeed) / 20.0)) + dTX.BASEBPM;

                            if (CDTXMania.ConfigIni.bDrumsEnabled)
                            {
                                CDTXMania.stagePerfDrumsScreen.UnitTime = ((60.0 / (CDTXMania.stagePerfDrumsScreen.actPlayInfo.dbBPM) / 14.0));
                                CDTXMania.stagePerfDrumsScreen.ctComboTimer = new CCounter(1.0, 16.0, ((60.0 / (CDTXMania.stagePerfDrumsScreen.actPlayInfo.dbBPM) / 16)), CSoundManager.rcPerformanceTimer);
                            }
                            else if (CDTXMania.ConfigIni.bGuitarEnabled)
                            {
                                CDTXMania.stagePerfGuitarScreen.UnitTime = ((60.0 / (CDTXMania.stagePerfGuitarScreen.actPlayInfo.dbBPM) / 14.0));
                                CDTXMania.stagePerfGuitarScreen.ctComboTimer = new CCounter(1.0, 16.0, ((60.0 / (CDTXMania.stagePerfGuitarScreen.actPlayInfo.dbBPM) / 16)), CSoundManager.rcPerformanceTimer);
                            }
                        }
                        break;
                    #endregion
                    #region [ 04, 07, 55, 56,57, 58, 59, 60:レイヤーBGA ]
                    case 0x04:	// レイヤーBGA
                    case 0x07:
                    case 0x55:
                    case 0x56:
                    case 0x57:
                    case 0x58:
                    case 0x59:
                    case 0x60:
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            if (configIni.bBGAEnabled)
                            {
                                switch (pChip.eBGA種別)
                                {
                                    case EBGAType.BMPTEX:
                                        if (pChip.rBMPTEX != null)
                                        {
                                            this.actBGA.Start(pChip.nChannelNumber, null, pChip.rBMPTEX, pChip.rBMPTEX.tx画像.szImageSize.Width, pChip.rBMPTEX.tx画像.szImageSize.Height, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                                        }
                                        break;

                                    case EBGAType.BGA:
                                        if ((pChip.rBGA != null) && ((pChip.rBMP != null) || (pChip.rBMPTEX != null)))
                                        {
                                            this.actBGA.Start(pChip.nChannelNumber, pChip.rBMP, pChip.rBMPTEX, pChip.rBGA.pt画像側右下座標.X - pChip.rBGA.pt画像側左上座標.X, pChip.rBGA.pt画像側右下座標.Y - pChip.rBGA.pt画像側左上座標.Y, 0, 0, pChip.rBGA.pt画像側左上座標.X, pChip.rBGA.pt画像側左上座標.Y, 0, 0, pChip.rBGA.pt表示座標.X, pChip.rBGA.pt表示座標.Y, 0, 0, 0);
                                        }
                                        break;

                                    case EBGAType.BGAPAN:
                                        if ((pChip.rBGAPan != null) && ((pChip.rBMP != null) || (pChip.rBMPTEX != null)))
                                        {
                                            this.actBGA.Start(pChip.nChannelNumber, pChip.rBMP, pChip.rBMPTEX, pChip.rBGAPan.sz開始サイズ.Width, pChip.rBGAPan.sz開始サイズ.Height, pChip.rBGAPan.sz終了サイズ.Width, pChip.rBGAPan.sz終了サイズ.Height, pChip.rBGAPan.pt画像側開始位置.X, pChip.rBGAPan.pt画像側開始位置.Y, pChip.rBGAPan.pt画像側終了位置.X, pChip.rBGAPan.pt画像側終了位置.Y, pChip.rBGAPan.pt表示側開始位置.X, pChip.rBGAPan.pt表示側開始位置.Y, pChip.rBGAPan.pt表示側終了位置.X, pChip.rBGAPan.pt表示側終了位置.Y, pChip.n総移動時間);
                                        }
                                        break;

                                    default:
                                        if (pChip.rBMP != null)
                                        {
                                            this.actBGA.Start(pChip.nChannelNumber, pChip.rBMP, null, pChip.rBMP.n幅, pChip.rBMP.n高さ, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    #endregion
                    #region [ 08: BPM変更(拡張) ]
                    case 0x08:	// BPM変更(拡張)
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            if (dTX.listBPM.ContainsKey(pChip.nIntegerValue_InternalNumber))
                            {
                                this.actPlayInfo.dbBPM = (dTX.listBPM[pChip.nIntegerValue_InternalNumber].dbBPM値 * (((double)configIni.nPlaySpeed) / 20.0)) + dTX.BASEBPM;

                                if (CDTXMania.ConfigIni.bDrumsEnabled)
                                {
                                    CDTXMania.stagePerfDrumsScreen.UnitTime = ((60.0 / (CDTXMania.stagePerfDrumsScreen.actPlayInfo.dbBPM) / 14.0));
                                    CDTXMania.stagePerfDrumsScreen.ctComboTimer = new CCounter(1.0, 16.0, ((60.0 / (CDTXMania.stagePerfDrumsScreen.actPlayInfo.dbBPM) / 16)), CSoundManager.rcPerformanceTimer);
                                }
                                else if (CDTXMania.ConfigIni.bGuitarEnabled)
                                {
                                    CDTXMania.stagePerfGuitarScreen.UnitTime = ((60.0 / (CDTXMania.stagePerfGuitarScreen.actPlayInfo.dbBPM) / 14.0));
                                    CDTXMania.stagePerfGuitarScreen.ctComboTimer = new CCounter(1.0, 16.0, ((60.0 / (CDTXMania.stagePerfGuitarScreen.actPlayInfo.dbBPM) / 16)), CSoundManager.rcPerformanceTimer);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region [ 1f: フィルインサウンド(ドラム) ]
                    case 0x1f:	// フィルインサウンド(ドラム)
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            this.r現在の歓声Chip.Drums = pChip;
                        }
                        break;
                    #endregion
                    #region [ 20-27: ギター演奏 ]
                    case 0x20:	// ギター演奏
                    case 0x21:
                    case 0x22:
                    case 0x23:
                    case 0x24:
                    case 0x25:
                    case 0x26:
                    case 0x27:


                    case 0x93:
                    case 0x94:
                    case 0x95:
                    case 0x96:
                    case 0x97:
                    case 0x98:
                    case 0x99:
                    case 0x9A:
                    case 0x9B:
                    case 0x9C:
                    case 0x9D:
                    case 0x9E:
                    case 0x9F:
                    case 0xA9:
                    case 0xAA:
                    case 0xAB:
                    case 0xAC:
                    case 0xAD:
                    case 0xAE:
                    case 0xAF:
                    case 0xD0:
                    case 0xD1:
                    case 0xD2:
                    case 0xD3:

                        this.t進行描画_チップ_ギターベース(configIni, ref dTX, ref pChip, EInstrumentPart.GUITAR);
                        break;
                    #endregion
                    #region [ 28: ウェイリング(ギター) ]
                    case 0x28:	// ウェイリング(ギター)
                        this.tUpdateAndDraw_Chip_Guitar_Wailing(configIni, ref dTX, ref pChip);
                        break;
                    #endregion
                    #region [ 2f: ウェイリングサウンド(ギター) ]
                    case 0x2f:	// ウェイリングサウンド(ギター)
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Guitar < 0))
                        {
                            pChip.bHit = true;
                            this.r現在の歓声Chip.Guitar = pChip;
                        }
                        break;
                    #endregion
                    #region [ 31-3a: 不可視チップ配置(ドラム) ]
                    case 0x31:	// 不可視チップ配置(ドラム)
                    case 0x32:
                    case 0x33:
                    case 0x34:
                    case 0x35:
                    case 0x36:
                    case 0x37:
                    case 0x38:
                    case 0x39:
                    case 0x3a:
                    case 0x3b:
                    case 0x3c:
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                        }
                        break;
                    #endregion
                    #region [ 4F、4E、4D、4C: ボーナス ]
                    case 0x4C:
                    case 0x4D:
                    case 0x4E:
                    case 0x4F:  //追加した順番の都合上、4F、4E____という順でBonus1、Bonus2___という割り当てになってます。
                        //this.t進行描画_チップ_ボーナス(configIni, ref dTX, ref pChip);
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                        }
                        break;
                    #endregion

                    #region [ 52: MIDIコーラス ]
                    case 0x52:	// MIDIコーラス
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                        }
                        break;
                    #endregion
                    #region [ 53: フィルイン ]
                    case 0x53:	// フィルイン
                        this.t進行描画_チップ_フィルイン(configIni, ref dTX, ref pChip);
                        break;
                    #endregion
                    #region [ 54, 5A: 動画再生 ]
                    case 0x54:	// 動画再生
                    case 0x5A:
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            if (configIni.bAVIEnabled)
                            {
                                switch (pChip.eAVI種別)
                                {
                                    case EAVIType.AVI:
                                        if (pChip.rAVI != null)
                                        {
                                            this.actAVI.Start(pChip.nChannelNumber, pChip.rAVI, pChip.rDShow, 278, 355, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, pChip.nPlaybackTimeMs);
                                        }
                                        break;

                                    case EAVIType.AVIPAN:
                                        if (pChip.rAVIPan != null)
                                        {
                                            this.actAVI.Start(pChip.nChannelNumber, pChip.rAVI, pChip.rDShow, pChip.rAVIPan.sz開始サイズ.Width, pChip.rAVIPan.sz開始サイズ.Height, pChip.rAVIPan.sz終了サイズ.Width, pChip.rAVIPan.sz終了サイズ.Height, pChip.rAVIPan.pt動画側開始位置.X, pChip.rAVIPan.pt動画側開始位置.Y, pChip.rAVIPan.pt動画側終了位置.X, pChip.rAVIPan.pt動画側終了位置.Y, pChip.rAVIPan.pt表示側開始位置.X, pChip.rAVIPan.pt表示側開始位置.Y, pChip.rAVIPan.pt表示側終了位置.X, pChip.rAVIPan.pt表示側終了位置.Y, pChip.n総移動時間, pChip.nPlaybackTimeMs);
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    #endregion
                    #region [ 61-92: 自動再生(BGM, SE) ]
                    case 0x61:
                    case 0x62:
                    case 0x63:
                    case 0x64:	// 自動再生(BGM, SE)
                    case 0x65:
                    case 0x66:
                    case 0x67:
                    case 0x68:
                    case 0x69:
                    case 0x70:
                    case 0x71:
                    case 0x72:
                    case 0x73:
                    case 0x74:
                    case 0x75:
                    case 0x76:
                    case 0x77:
                    case 0x78:
                    case 0x79:
                    case 0x80:
                    case 0x81:
                    case 0x82:
                    case 0x83:
                    case 0x90:
                    case 0x91:
                    case 0x92:
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            if (configIni.bBGM音を発声する)
                            {
                                dTX.tStopPlayingWav(this.nLastPlayedBGMWAVNumber[pChip.nChannelNumber - 0x61]);
                                dTX.tPlayChip(pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs, (int)ELane.BGM, dTX.nモニタを考慮した音量(EInstrumentPart.UNKNOWN));
                                this.nLastPlayedBGMWAVNumber[pChip.nChannelNumber - 0x61] = pChip.nIntegerValue_InternalNumber;
                            }
                        }
                        break;
                    #endregion

                    #region [ 84-89: 仮: override sound ]	// #26338 2011.11.8 yyagi
                    case 0x84:	// HH (HO/HC)
                    case 0x85:	// CY
                    case 0x86:	// RD
                    case 0x87:	// LC
                    case 0x88:	// Guitar
                    case 0x89:	// Bass
                        // mute sound (auto)
                        // 4A: 84: HH (HO/HC)
                        // 4B: 85: CY
                        // 4C: 86: RD
                        // 4D: 87: LC
                        // 2A: 88: Gt
                        // AA: 89: Bs

                        //	CDTXMania.DTX.tStopPlayingWav( this.nLastPlayedWAVNumber.Guitar );
                        //	CDTXMania.DTX.tPlayChip( pChip, n再生開始システム時刻ms, 8, nVolume, bモニタ, b音程をずらして再生 );
                        //	this.nLastPlayedWAVNumber.Guitar = pChip.nIntegerValue_InternalNumber;

                        //	protected void tPlaySound( CDTX.CChip pChip, long n再生開始システム時刻ms, EInstrumentPart part, int nVolume, bool bモニタ, bool b音程をずらして再生 )
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            EInstrumentPart[] p = { EInstrumentPart.DRUMS, EInstrumentPart.DRUMS, EInstrumentPart.DRUMS, EInstrumentPart.DRUMS, EInstrumentPart.GUITAR, EInstrumentPart.BASS };

                            EInstrumentPart pp = p[pChip.nChannelNumber - 0x84];

                            //							if ( pp == EInstrumentPart.DRUMS ) {			// pChip.nChannelNumber= ..... HHとか、ドラムの場合は変える。
                            //								//            HC    CY    RD    LC
                            //								int[] ch = { 0x11, 0x16, 0x19, 0x1A };
                            //								pChip.nChannelNumber = ch[ pChip.nChannelNumber - 0x84 ]; 
                            //							}
                            this.tPlaySound(pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs, pp, dTX.nモニタを考慮した音量(pp));
                        }
                        break;
                    #endregion

                    #region [ a0-a7: ベース演奏 ]
                    case 0xa0:	// ベース演奏
                    case 0xa1:
                    case 0xa2:
                    case 0xa3:
                    case 0xa4:
                    case 0xa5:
                    case 0xa6:
                    case 0xa7:

                    case 0xC5:
                    case 0xC6:
                    case 0xC8:
                    case 0xC9:
                    case 0xCA:
                    case 0xCB:
                    case 0xCC:
                    case 0xCD:
                    case 0xCE:
                    case 0xCF:
                    case 0xDA:
                    case 0xDB:
                    case 0xDC:
                    case 0xDD:
                    case 0xDE:
                    case 0xDF:
                    case 0xE1:
                    case 0xE2:
                    case 0xE3:
                    case 0xE4:
                    case 0xE5:
                    case 0xE6:
                    case 0xE7:
                    case 0xE8:
                        this.t進行描画_チップ_ギターベース(configIni, ref dTX, ref pChip, EInstrumentPart.BASS);
                        break;
                    #endregion
                    #region [ a8: ウェイリング(ベース) ]
                    case 0xa8:	// ウェイリング(ベース)
                        this.t進行描画_チップ_ベース_ウェイリング(configIni, ref dTX, ref pChip);
                        break;
                    #endregion
                    #region [ af: ウェイリングサウンド(ベース) ]
                    /*
					case 0xaf:	// ウェイリングサウンド(ベース)
						if ( !pChip.bHit && ( pChip.nDistanceFromBar.Bass < 0 ) )
						{
							pChip.bHit = true;
							this.r現在の歓声Chip.Bass = pChip;
						}
						break;
                        */
                    #endregion
                    #region [ b1-b9, bc: 空打ち音設定(ドラム) ]
                    case 0xb1:	// 空打ち音設定(ドラム)
                    case 0xb2:
                    case 0xb3:
                    case 0xb4:
                    case 0xb5:
                    case 0xb6:
                    case 0xb7:
                    case 0xb8:
                    case 0xb9:
                    case 0xbc:
                    case 0xbd:
                    case 0xbe:
                        this.t進行描画_チップ_空打ち音設定_ドラム(configIni, ref dTX, ref pChip);
                        break;
                    #endregion
                    #region [ ba: 空打ち音設定(ギター) ]
                    case 0xba:	// 空打ち音設定(ギター)
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Guitar < 0))
                        {
                            pChip.bHit = true;
                            this.r現在の空うちギターChip = pChip;
                            pChip.nChannelNumber = 0x20;
                        }
                        break;
                    #endregion
                    #region [ bb: 空打ち音設定(ベース) ]
                    case 0xbb:	// 空打ち音設定(ベース)
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Bass < 0))
                        {
                            pChip.bHit = true;
                            this.r現在の空うちベースChip = pChip;
                            pChip.nChannelNumber = 0xA0;
                        }
                        break;
                    #endregion
                    #region [ c4, c7, d5-d9, e0: BGA画像入れ替え ]
                    case 0xc4:
                    case 0xc7:
                    case 0xd5:
                    case 0xd6:	// BGA画像入れ替え
                    case 0xd7:
                    case 0xd8:
                    case 0xd9:
                    case 0xe0:
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                            if ((configIni.bBGAEnabled && (pChip.eBGA種別 == EBGAType.BMP)) || (pChip.eBGA種別 == EBGAType.BMPTEX))
                            {
                                for (int i = 0; i < 8; i++)
                                {
                                    if (this.nBGAスコープチャンネルマップ[0, i] == pChip.nChannelNumber)
                                    {
                                        this.actBGA.ChangeScope(this.nBGAスコープチャンネルマップ[1, i], pChip.rBMP, pChip.rBMPTEX);
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region [ ea: ミキサーへチップ音追加 ]
                    case 0xEA:
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            //Debug.WriteLine("[DA(AddMixer)] BAR=" + pChip.nPlaybackPosition / 384 + " ch=" + pChip.nChannelNumber.ToString("x2") + ", wav=" + pChip.nIntegerValue.ToString("x2") + ", time=" + pChip.nPlaybackTimeMs);
                            pChip.bHit = true;
                            if (listWAV.ContainsKey(pChip.nIntegerValue_InternalNumber)) // 参照が遠いので後日最適化する
                            {
                                CDTX.CWAV wc = listWAV[pChip.nIntegerValue_InternalNumber];
                                for (int i = 0; i < nPolyphonicSounds; i++)
                                {
                                    if (wc.rSound[i] != null)
                                    {
                                        //CDTXMania.SoundManager.AddMixer( wc.rSound[ i ] );
                                        AddMixer(wc.rSound[i], pChip.bChipKeepsPlayingAfterPerfEnds);
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region [ eb: ミキサーからチップ音削除 ]
                    case 0xEB:
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            //Debug.WriteLine("[DB(RemoveMixer)] BAR=" + pChip.nPlaybackPosition / 384 + " ch=" + pChip.nChannelNumber.ToString("x2") + ", wav=" + pChip.nIntegerValue.ToString("x2") + ", time=" + pChip.nPlaybackTimeMs);
                            pChip.bHit = true;
                            if (listWAV.ContainsKey(pChip.nIntegerValue_InternalNumber))	// 参照が遠いので後日最適化する
                            {
                                CDTX.CWAV wc = listWAV[pChip.nIntegerValue_InternalNumber];
                                for (int i = 0; i < nPolyphonicSounds; i++)
                                {
                                    if (wc.rSound[i] != null)
                                    {
                                        //CDTXMania.SoundManager.RemoveMixer( wc.rSound[ i ] );
                                        RemoveMixer(wc.rSound[i]);
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region [ その他(未定義) ]
                    default:
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                        }
                        break;
                    #endregion
                }
            }
            return false;
        }
        protected bool tUpdateAndDraw_BarLine(EInstrumentPart ePlayMode)
        {
            if ((base.ePhaseID == CStage.EPhase.演奏_STAGE_FAILED) || (base.ePhaseID == CStage.EPhase.演奏_STAGE_FAILED_フェードアウト))
            {
                return true;
            }
            if ((this.nCurrentTopChip == -1) || (this.nCurrentTopChip >= listChip.Count))
            {
                return true;
            }
            if (this.nCurrentTopChip == -1)
            {
                return true;
            }

            const double speed = 286;	// BPM150の時の1小節の長さ[dot]
            //XGのHS4.5が1289。思えばBPMじゃなくて拍の長さが関係あるよね。

            double ScrollSpeedDrums = (this.actScrollSpeed.db現在の譜面スクロール速度.Drums + 1.0) * 0.5 * 37.5 * speed / 60000.0;
            double ScrollSpeedGuitar = (this.actScrollSpeed.db現在の譜面スクロール速度.Guitar + 1.0) * 0.5 * 0.5 * 37.5 * speed / 60000.0;
            double ScrollSpeedBass = (this.actScrollSpeed.db現在の譜面スクロール速度.Bass + 1.0) * 0.5 * 0.5 * 37.5 * speed / 60000.0;

            CDTX dTX = CDTXMania.DTX;
            CConfigIni configIni = CDTXMania.ConfigIni;            
            for (int nCurrentTopChip = this.nCurrentTopChip; nCurrentTopChip < dTX.listChip.Count; nCurrentTopChip++)
            {
                CDTX.CChip pChip = dTX.listChip[nCurrentTopChip];
                //Debug.WriteLine( "nCurrentTopChip=" + nCurrentTopChip + ", ch=" + pChip.nChannelNumber.ToString("x2") + ", 発音位置=" + pChip.nPlaybackPosition + ", 発声時刻ms=" + pChip.nPlaybackTimeMs );
                pChip.nDistanceFromBar.Drums = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedDrums);
                pChip.nDistanceFromBar.Guitar = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedGuitar);
                pChip.nDistanceFromBar.Bass = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedBass);
                if (Math.Min(Math.Min(pChip.nDistanceFromBar.Drums, pChip.nDistanceFromBar.Guitar), pChip.nDistanceFromBar.Bass) > 600)
                {
                    break;
                }
                //				if ( ( ( nCurrentTopChip == this.nCurrentTopChip ) && ( pChip.nDistanceFromBar.Drums < -65 ) ) && pChip.bHit )
                // #28026 2012.4.5 yyagi; 信心ワールドエンドの曲終了後リザルトになかなか行かない問題の修正
                if ((dTX.listChip[this.nCurrentTopChip].nDistanceFromBar.Drums < -65) && dTX.listChip[this.nCurrentTopChip].bHit)
                {
                    //					nCurrentTopChip = ++this.nCurrentTopChip;
                    ++this.nCurrentTopChip;
                    continue;
                }

                bool bPChipIsAutoPlay = bCheckAutoPlay(pChip);

                int nInputAdjustTime = (bPChipIsAutoPlay || (pChip.eInstrumentPart == EInstrumentPart.UNKNOWN)) ? 0 : this.nInputAdjustTimeMs[(int)pChip.eInstrumentPart];

                switch (pChip.nChannelNumber)
                {
                    #region [ 50: 小節線 ]
                    case 0x50:	// 小節線
                        {
                            this.tUpdateAndDraw_Chip_BarLine(configIni, ref dTX, ref pChip);
                            break;
                        }
                    #endregion
                    #region [ 51: 拍線 ]
                    case 0x51:	// 拍線
                        if (!pChip.bHit && (pChip.nDistanceFromBar.Drums < 0))
                        {
                            pChip.bHit = true;
                        }
                        if ((ePlayMode == EInstrumentPart.DRUMS) && (configIni.nLaneDisp.Drums == 0 || configIni.nLaneDisp.Drums == 1) && pChip.bVisible && (this.txチップ != null))
                        {
                            int l_drumPanelWidth = 0x22f;
                            int l_xOffset = 0;
                            if(configIni.eNumOfLanes.Drums == EType.B)
                            {
                                l_drumPanelWidth = 0x207;
                            }
                            else if (CDTXMania.ConfigIni.eNumOfLanes.Drums == EType.C)
                            {
                                l_drumPanelWidth = 447;
                                l_xOffset = 72;
                            }
                            this.txチップ.tDraw2D(CDTXMania.app.Device, 0x127 + l_xOffset, configIni.bReverse.Drums ? ((this.nJudgeLinePosY.Drums + pChip.nDistanceFromBar.Drums) - 1) : ((this.nJudgeLinePosY.Drums - pChip.nDistanceFromBar.Drums) - 1), new Rectangle(0, 772, l_drumPanelWidth, 2));
                        }
                        break;
                    #endregion
                }
            }
            return false;
        }
        protected bool tUpdateAndDraw_Chip_PatternOnly(EInstrumentPart ePlayMode)
        {
            if ((base.ePhaseID == CStage.EPhase.演奏_STAGE_FAILED) || (base.ePhaseID == CStage.EPhase.演奏_STAGE_FAILED_フェードアウト))
            {
                return true;
            }
            if ((this.nCurrentTopChip == -1) || (this.nCurrentTopChip >= listChip.Count))
            {
                return true;
            }
            if (this.nCurrentTopChip == -1)
            {
                return true;
            }

            const double speed = 286;	// BPM150の時の1小節の長さ[dot]
            //XGのHS4.5が1289。思えばBPMじゃなくて拍の長さが関係あるよね。

            double ScrollSpeedDrums = (this.actScrollSpeed.db現在の譜面スクロール速度.Drums + 1.0) * 0.5 * 37.5 * speed / 60000.0;
            double ScrollSpeedGuitar = (this.actScrollSpeed.db現在の譜面スクロール速度.Guitar + 1.0) * 0.5 * 0.5 * 37.5 * speed / 60000.0;
            double ScrollSpeedBass = (this.actScrollSpeed.db現在の譜面スクロール速度.Bass + 1.0) * 0.5 * 0.5 * 37.5 * speed / 60000.0;

            CDTX dTX = CDTXMania.DTX;
            CConfigIni configIni = CDTXMania.ConfigIni;
            for (int nCurrentTopChip = this.nCurrentTopChip; nCurrentTopChip < dTX.listChip.Count; nCurrentTopChip++)
            {
                CDTX.CChip pChip = dTX.listChip[nCurrentTopChip];
                //Debug.WriteLine( "nCurrentTopChip=" + nCurrentTopChip + ", ch=" + pChip.nChannelNumber.ToString("x2") + ", 発音位置=" + pChip.nPlaybackPosition + ", 発声時刻ms=" + pChip.nPlaybackTimeMs );
                pChip.nDistanceFromBar.Drums = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedDrums);
                pChip.nDistanceFromBar.Guitar = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedGuitar);
                pChip.nDistanceFromBar.Bass = (int)((pChip.nPlaybackTimeMs - CSoundManager.rcPerformanceTimer.nCurrentTime) * ScrollSpeedBass);
                if (Math.Min(Math.Min(pChip.nDistanceFromBar.Drums, pChip.nDistanceFromBar.Guitar), pChip.nDistanceFromBar.Bass) > 600)
                {
                    break;
                }
                //				if ( ( ( nCurrentTopChip == this.nCurrentTopChip ) && ( pChip.nDistanceFromBar.Drums < -65 ) ) && pChip.bHit )
                // #28026 2012.4.5 yyagi; 信心ワールドエンドの曲終了後リザルトになかなか行かない問題の修正
                if ((dTX.listChip[this.nCurrentTopChip].nDistanceFromBar.Drums < -65) && dTX.listChip[this.nCurrentTopChip].bHit)
                {
                    //					nCurrentTopChip = ++this.nCurrentTopChip;
                    ++this.nCurrentTopChip;
                    continue;
                }

                bool bPChipIsAutoPlay = bCheckAutoPlay(pChip);

                int nInputAdjustTime = (bPChipIsAutoPlay || (pChip.eInstrumentPart == EInstrumentPart.UNKNOWN)) ? 0 : this.nInputAdjustTimeMs[(int)pChip.eInstrumentPart];

                switch (pChip.nChannelNumber)
                {
                    #region [ 11-1c: ドラム演奏 ]
                    case 0x11:	// ドラム演奏
                    case 0x12:
                    case 0x13:
                    case 0x14:
                    case 0x15:
                    case 0x16:
                    case 0x17:
                    case 0x18:
                    case 0x19:
                    case 0x1a:
                    case 0x1b:
                    case 0x1c:
                        this.t進行描画_チップ_模様のみ_ドラムス(configIni, ref dTX, ref pChip);
                        break;
                    #endregion
                    #region [ その他(未定義) ]
                    default:
                        //if ( !pChip.bHit && ( pChip.nDistanceFromBar.Drums < 0 ) )
                        {
                            //	pChip.bHit = true;
                        }
                        break;
                    #endregion
                }
            }
            return false;
        }

        public bool bCheckAutoPlay(CDTX.CChip pChip)
        {
            bool bPChipIsAutoPlay = false;
            bool bGtBsR = false;
            bool bGtBsG = false;
            bool bGtBsB = false;
            bool bGtBsY = false;
            bool bGtBsP = false;
            bool bGtBsW = false;
            bool bGtBsO = false;
            switch (pChip.nChannelNumber)
            {
                case 0x20:
                    bGtBsO = true;
                    break;
                case 0x21:
                    bGtBsB = true;
                    break;
                case 0x22:
                    bGtBsG = true;
                    break;
                case 0x23:
                    bGtBsG = true;
                    bGtBsB = true;
                    break;
                case 0x24:
                    bGtBsR = true;
                    break;
                case 0x25:
                    bGtBsR = true;
                    bGtBsB = true;
                    break;
                case 0x26:
                    bGtBsR = true;
                    bGtBsG = true;
                    break;
                case 0x27:
                    bGtBsR = true;
                    bGtBsG = true;
                    bGtBsB = true;
                    break;
                case 0x28:
                    bGtBsW = true;
                    break;
                default:
                    switch (pChip.nChannelNumber)
                    {
                        case 0x93:
                            bGtBsY = true;
                            break;
                        case 0x94:
                            bGtBsB = true;
                            bGtBsY = true;
                            break;
                        case 0x95:
                            bGtBsG = true;
                            bGtBsY = true;
                            break;
                        case 0x96:
                            bGtBsG = true;
                            bGtBsB = true;
                            bGtBsY = true;
                            break;
                        case 0x97:
                            bGtBsR = true;
                            bGtBsY = true;
                            break;
                        case 0x98:
                            bGtBsR = true;
                            bGtBsB = true;
                            bGtBsY = true;
                            break;
                        case 0x99:
                            bGtBsR = true;
                            bGtBsG = true;
                            bGtBsY = true;
                            break;
                        case 0x9A:
                            bGtBsR = true;
                            bGtBsG = true;
                            bGtBsB = true;
                            bGtBsY = true;
                            break;
                        case 0x9B:
                            bGtBsP = true;
                            break;
                        case 0x9C:
                            bGtBsB = true;
                            bGtBsP = true;
                            break;
                        case 0x9D:
                            bGtBsG = true;
                            bGtBsP = true;
                            break;
                        case 0x9E:
                            bGtBsG = true;
                            bGtBsB = true;
                            bGtBsP = true;
                            break;
                        case 0x9F:
                            bGtBsR = true;
                            bGtBsP = true;
                            break;

                        case 0xA0:
                            bGtBsO = true;
                            break;
                        case 0xA1:
                            bGtBsB = true;
                            break;
                        case 0xA2:
                            bGtBsG = true;
                            break;
                        case 0xA3:
                            bGtBsG = true;
                            bGtBsB = true;
                            break;
                        case 0xA4:
                            bGtBsR = true;
                            break;
                        case 0xA5:
                            bGtBsR = true;
                            bGtBsB = true;
                            break;
                        case 0xA6:
                            bGtBsR = true;
                            bGtBsG = true;
                            break;
                        case 0xA7:
                            bGtBsR = true;
                            bGtBsG = true;
                            bGtBsB = true;
                            break;


                        case 0xA8:
                            bGtBsW = true;
                            break;
                        case 0xA9:
                            bGtBsR = true;
                            bGtBsB = true;
                            bGtBsP = true;
                            break;
                        case 0xAA:
                            bGtBsR = true;
                            bGtBsG = true;
                            bGtBsP = true;
                            break;
                        case 0xAB:
                            bGtBsR = true;
                            bGtBsG = true;
                            bGtBsB = true;
                            bGtBsP = true;
                            break;
                        case 0xAC:
                            bGtBsY = true;
                            bGtBsP = true;
                            break;
                        case 0xAD:
                            bGtBsB = true;
                            bGtBsY = true;
                            bGtBsP = true;
                            break;
                        case 0xAE:
                            bGtBsG = true;
                            bGtBsY = true;
                            bGtBsP = true;
                            break;
                        case 0xAF:
                            bGtBsG = true;
                            bGtBsB = true;
                            bGtBsY = true;
                            bGtBsP = true;
                            break;

                        case 0xC5:
                            bGtBsY = true;
                            break;
                        case 0xC6:
                            bGtBsB = true;
                            bGtBsY = true;
                            break;

                        case 0xC8:
                            bGtBsG = true;
                            bGtBsY = true;
                            break;
                        case 0xC9:
                            bGtBsG = true;
                            bGtBsB = true;
                            bGtBsY = true;
                            break;
                        case 0xCA:
                            bGtBsR = true;
                            bGtBsY = true;
                            break;
                        case 0xCB:
                            bGtBsR = true;
                            bGtBsB = true;
                            bGtBsY = true;
                            break;
                        case 0xCC:
                            bGtBsR = true;
                            bGtBsG = true;
                            bGtBsY = true;
                            break;
                        case 0xCD:
                            bGtBsR = true;
                            bGtBsG = true;
                            bGtBsB = true;
                            bGtBsY = true;
                            break;
                        case 0xCE:
                            bGtBsP = true;
                            break;
                        case 0xCF:
                            bGtBsB = true;
                            bGtBsP = true;
                            break;
                        case 0xD0:
                            bGtBsR = true;
                            bGtBsY = true;
                            bGtBsP = true;
                            break;
                        case 0xD1:
                            bGtBsR = true;
                            bGtBsB = true;
                            bGtBsY = true;
                            bGtBsP = true;
                            break;
                        case 0xD2:
                            bGtBsR = true;
                            bGtBsG = true;
                            bGtBsY = true;
                            bGtBsP = true;
                            break;
                        case 0xD3:
                            bGtBsR = true;
                            bGtBsG = true;
                            bGtBsB = true;
                            bGtBsY = true;
                            bGtBsP = true;
                            break;


                        case 0xDA:
                            bGtBsG = true;
                            bGtBsP = true;
                            break;
                        case 0xDB:
                            bGtBsG = true;
                            bGtBsB = true;
                            bGtBsP = true;
                            break;
                        case 0xDC:
                            bGtBsR = true;
                            bGtBsP = true;
                            break;
                        case 0xDD:
                            bGtBsR = true;
                            bGtBsB = true;
                            bGtBsP = true;
                            break;
                        case 0xDE:
                            bGtBsR = true;
                            bGtBsG = true;
                            bGtBsP = true;
                            break;
                        case 0xDF:
                            bGtBsR = true;
                            bGtBsG = true;
                            bGtBsB = true;
                            bGtBsP = true;
                            break;
                        case 0xE1:
                            bGtBsY = true;
                            bGtBsP = true;
                            break;
                        case 0xE2:
                            bGtBsB = true;
                            bGtBsY = true;
                            bGtBsP = true;
                            break;
                        case 0xE3:
                            bGtBsG = true;
                            bGtBsY = true;
                            bGtBsP = true;
                            break;
                        case 0xE4:
                            bGtBsG = true;
                            bGtBsB = true;
                            bGtBsY = true;
                            bGtBsP = true;
                            break;
                        case 0xE5:
                            bGtBsR = true;
                            bGtBsY = true;
                            bGtBsP = true;
                            break;
                        case 0xE6:
                            bGtBsR = true;
                            bGtBsB = true;
                            bGtBsY = true;
                            bGtBsP = true;
                            break;
                        case 0xE7:
                            bGtBsR = true;
                            bGtBsG = true;
                            bGtBsY = true;
                            bGtBsP = true;
                            break;
                        case 0xE8:
                            bGtBsR = true;
                            bGtBsG = true;
                            bGtBsB = true;
                            bGtBsY = true;
                            bGtBsP = true;
                            break;
                    }
                    break;
            }
            if (pChip.eInstrumentPart == EInstrumentPart.DRUMS)
            {
                if (bIsAutoPlay[this.nチャンネル0Atoレーン07[pChip.nChannelNumber - 0x11]])
                {
                    bPChipIsAutoPlay = true;
                }
            }
            else if (pChip.eInstrumentPart == EInstrumentPart.GUITAR)
            {
                //Trace.TraceInformation( "chip:{0}{1}{2} ", bGtBsR, bGtBsG, bGtBsB );
                //Trace.TraceInformation( "auto:{0}{1}{2} ", bIsAutoPlay[ (int) ELane.GtR ], bIsAutoPlay[ (int) ELane.GtG ], bIsAutoPlay[ (int) ELane.GtB ]);
                bPChipIsAutoPlay = true;
                if (bIsAutoPlay[(int)ELane.GtPick] == false) bPChipIsAutoPlay = false;
                else
                {
                    if (bGtBsR == true && bIsAutoPlay[(int)ELane.GtR] == false) bPChipIsAutoPlay = false;
                    else if (bGtBsG == true && bIsAutoPlay[(int)ELane.GtG] == false) bPChipIsAutoPlay = false;
                    else if (bGtBsB == true && bIsAutoPlay[(int)ELane.GtB] == false) bPChipIsAutoPlay = false;
                    else if (bGtBsY == true && bIsAutoPlay[(int)ELane.GtY] == false) bPChipIsAutoPlay = false;
                    else if (bGtBsP == true && bIsAutoPlay[(int)ELane.GtP] == false) bPChipIsAutoPlay = false;
                    else if (bGtBsW == true && bIsAutoPlay[(int)ELane.GtW] == false) bPChipIsAutoPlay = false;
                    else if (bGtBsO == true &&
                        (bIsAutoPlay[(int)ELane.GtR] == false || bIsAutoPlay[(int)ELane.GtG] == false || bIsAutoPlay[(int)ELane.GtB] == false || bIsAutoPlay[(int)ELane.GtY] == false || bIsAutoPlay[(int)ELane.GtP] == false))
                        bPChipIsAutoPlay = false;
                }
                //Trace.TraceInformation( "{0:x2}: {1}", pChip.nChannelNumber, bPChipIsAutoPlay.ToString() );
            }
            else if (pChip.eInstrumentPart == EInstrumentPart.BASS)
            {
                bPChipIsAutoPlay = true;
                if (bIsAutoPlay[(int)ELane.BsPick] == false) bPChipIsAutoPlay = false;
                else
                {
                    if (bGtBsR == true && bIsAutoPlay[(int)ELane.BsR] == false) bPChipIsAutoPlay = false;
                    else if (bGtBsG == true && bIsAutoPlay[(int)ELane.BsG] == false) bPChipIsAutoPlay = false;
                    else if (bGtBsB == true && bIsAutoPlay[(int)ELane.BsB] == false) bPChipIsAutoPlay = false;
                    else if (bGtBsW == true && bIsAutoPlay[(int)ELane.BsW] == false) bPChipIsAutoPlay = false;
                    else if (bGtBsY == true && bIsAutoPlay[(int)ELane.BsY] == false) bPChipIsAutoPlay = false;
                    else if (bGtBsP == true && bIsAutoPlay[(int)ELane.BsP] == false) bPChipIsAutoPlay = false;
                    else if (bGtBsO == true &&
                        (bIsAutoPlay[(int)ELane.BsR] == false || bIsAutoPlay[(int)ELane.BsG] == false || bIsAutoPlay[(int)ELane.BsB] == false || bIsAutoPlay[(int)ELane.BsY] == false || bIsAutoPlay[(int)ELane.BsP] == false))
                        bPChipIsAutoPlay = false;
                }
            }
            return bPChipIsAutoPlay;
        }


        protected abstract void tUpdateAndDraw_Chip_Drums(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip);
        protected abstract void t進行描画_チップ_模様のみ_ドラムス(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip);
        //protected abstract void t進行描画_チップ_ギター( CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip );
        protected abstract void t進行描画_チップ_ギターベース(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip, EInstrumentPart inst);

        protected void t進行描画_チップ_ギターベース(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip, EInstrumentPart inst,
            int barYNormal, int barYReverse, int showRangeY0, int showRangeY1, int openXg, int openXb,
            int rectOpenOffsetX, int rectOpenOffsetY, int openChipWidth, int chipHeight,
            int chipWidth, int guitarNormalX, int guitarLeftyX, int bassNormalX, int bassLeftyX, int drawDeltaX, int chipTexDeltaX)
        {
            int instIndex = (int)inst;
            if (configIni.bGuitarEnabled)
            {
                #region [ Hidden/Sudden処理 ]
                #region [ Sudden処理 ]
                if ((CDTXMania.ConfigIni.nHidSud[instIndex] == 2) || (CDTXMania.ConfigIni.nHidSud[instIndex] == 3))
                {
                    if (pChip.nDistanceFromBar[instIndex] < 250)
                    {
                        pChip.bVisible = true;
                        pChip.nTransparency = 0xff;
                    }
                    else if (pChip.nDistanceFromBar[instIndex] < 300)
                    {
                        pChip.bVisible = true;
                        pChip.nTransparency = 0xff - ((int)((((double)(pChip.nDistanceFromBar[instIndex] - 250)) * 255.0) / 75.0));
                    }
                    else
                    {
                        pChip.bVisible = false;
                        pChip.nTransparency = 0;
                    }
                }
                #endregion
                #region [ Hidden処理 ]
                if ((CDTXMania.ConfigIni.nHidSud[instIndex] == 1) || (CDTXMania.ConfigIni.nHidSud[instIndex] == 3))
                {
                    if (pChip.nDistanceFromBar[instIndex] < 150)
                    {
                        pChip.bVisible = false;
                    }
                    else if (pChip.nDistanceFromBar[instIndex] < 200)
                    {
                        pChip.bVisible = true;
                        pChip.nTransparency = (int)((((double)(pChip.nDistanceFromBar[instIndex] - 150)) * 255.0) / 75.0);
                    }
                }
                #endregion
                #region [ ステルス処理 ]
                if (CDTXMania.ConfigIni.nHidSud[instIndex] == 4)
                {
                    pChip.bVisible = false;
                }
                if (this.txチップ != null)
                {
                    this.txチップ.nTransparency = pChip.nTransparency;
                }
                #endregion
                #endregion

                bool bChipHasR = false;
                bool bChipHasG = false;
                bool bChipHasB = false;
                bool bChipHasY = false;
                bool bChipHasP = false;
                bool bChipHasW = false;
                bool bChipIsO = false;
                int nチャンネル番号 = pChip.nChannelNumber;

                switch (nチャンネル番号)
                {
                    case 0x20:
                        bChipIsO = true;
                        break;
                    case 0x21:
                        bChipHasB = true;
                        break;
                    case 0x22:
                        bChipHasG = true;
                        break;
                    case 0x23:
                        bChipHasG = true;
                        bChipHasB = true;
                        break;
                    case 0x24:
                        bChipHasR = true;
                        break;
                    case 0x25:
                        bChipHasR = true;
                        bChipHasB = true;
                        break;
                    case 0x26:
                        bChipHasR = true;
                        bChipHasG = true;
                        break;
                    case 0x27:
                        bChipHasR = true;
                        bChipHasG = true;
                        bChipHasB = true;
                        break;
                    case 0x28:
                        bChipHasW = true;
                        break;
                    default:
                        switch (nチャンネル番号)
                        {
                            case 0x93:
                                bChipHasY = true;
                                break;
                            case 0x94:
                                bChipHasB = true;
                                bChipHasY = true;
                                break;
                            case 0x95:
                                bChipHasG = true;
                                bChipHasY = true;
                                break;
                            case 0x96:
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                break;
                            case 0x97:
                                bChipHasR = true;
                                bChipHasY = true;
                                break;
                            case 0x98:
                                bChipHasR = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                break;
                            case 0x99:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasY = true;
                                break;
                            case 0x9a:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                break;
                            case 0x9B:
                                bChipHasP = true;
                                break;
                            case 0x9C:
                                bChipHasB = true;
                                bChipHasP = true;
                                break;
                            case 0x9D:
                                bChipHasG = true;
                                bChipHasP = true;
                                break;
                            case 0x9E:
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasP = true;
                                break;
                            case 0x9F:
                                bChipHasR = true;
                                bChipHasP = true;
                                break;

                            case 0xA0:
                                bChipIsO = true;
                                break;
                            case 0xA1:
                                bChipHasB = true;
                                break;
                            case 0xA2:
                                bChipHasG = true;
                                break;
                            case 0xA3:
                                bChipHasG = true;
                                bChipHasB = true;
                                break;
                            case 0xA4:
                                bChipHasR = true;
                                break;
                            case 0xA5:
                                bChipHasR = true;
                                bChipHasB = true;
                                break;
                            case 0xA6:
                                bChipHasR = true;
                                bChipHasG = true;
                                break;
                            case 0xA7:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasB = true;
                                break;
                            case 0xA8:
                                bChipHasW = true;
                                break;

                            case 0xA9:
                                bChipHasR = true;
                                bChipHasB = true;
                                bChipHasP = true;
                                break;
                            case 0xAA:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasP = true;
                                break;
                            case 0xAB:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasP = true;
                                break;
                            case 0xAC:
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case 0xAD:
                                bChipHasB = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case 0xAE:
                                bChipHasG = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case 0xAF:
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case 0xC5:
                                bChipHasY = true;
                                break;
                            case 0xC6:
                                bChipHasB = true;
                                bChipHasY = true;
                                break;

                            case 0xC8:
                                bChipHasG = true;
                                bChipHasY = true;
                                break;
                            case 0xC9:
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                break;
                            case 0xCA:
                                bChipHasR = true;
                                bChipHasY = true;
                                break;
                            case 0xCB:
                                bChipHasR = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                break;
                            case 0xCC:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasY = true;
                                break;
                            case 0xCD:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                break;
                            case 0xCE:
                                bChipHasP = true;
                                break;
                            case 0xCF:
                                bChipHasB = true;
                                bChipHasP = true;
                                break;
                            case 0xD0:
                                bChipHasR = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case 0xD1:
                                bChipHasR = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case 0xD2:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case 0xD3:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;


                            case 0xDA:
                                bChipHasG = true;
                                bChipHasP = true;
                                break;
                            case 0xDB:
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasP = true;
                                break;

                            case 0xDC:
                                bChipHasR = true;
                                bChipHasP = true;
                                break;

                            case 0xDD:
                                bChipHasR = true;
                                bChipHasB = true;
                                bChipHasP = true;
                                break;
                            case 0xDE:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasP = true;
                                break;
                            case 0xDF:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasP = true;
                                break;
                            case 0xE1:
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case 0xE2:
                                bChipHasB = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case 0xE3:
                                bChipHasG = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case 0xE4:
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case 0xE5:
                                bChipHasR = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case 0xE6:
                                bChipHasR = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case 0xE7:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                            case 0xE8:
                                bChipHasR = true;
                                bChipHasG = true;
                                bChipHasB = true;
                                bChipHasY = true;
                                bChipHasP = true;
                                break;
                        }
                        break;
                }

                #region [ chip描画 ]
                int OPEN = (inst == EInstrumentPart.GUITAR) ? 10 : 10;
                if (!pChip.bHit && pChip.bVisible)
                {
                    int y = configIni.bReverse[instIndex] ? (barYReverse - pChip.nDistanceFromBar[instIndex]) : (barYNormal + pChip.nDistanceFromBar[instIndex]);
                    if ((showRangeY0 < y) && (y < showRangeY1))
                    {
                        if (this.txチップ != null)
                        {
                            int nアニメカウンタ現在の値 = this.ctChipPatternAnimation[instIndex].nCurrentValue;
                            if (bChipIsO)
                            {
                                int xo = (inst == EInstrumentPart.GUITAR) ? 88 : 959;
                                this.txチップ.tDraw2D(CDTXMania.app.Device, xo, y - 2, new Rectangle(0, 10, 196, 10));
                            }
                            Rectangle rc = new Rectangle(rectOpenOffsetX, chipHeight, chipWidth, 10);
                            int x;
                            if (inst == EInstrumentPart.GUITAR)
                            {
                                x = (configIni.bLeft.Guitar) ? guitarLeftyX : guitarNormalX;
                            }
                            else
                            {
                                x = (configIni.bLeft.Bass) ? bassLeftyX : bassNormalX;
                            }
                            int deltaX = (configIni.bLeft[instIndex]) ? -drawDeltaX : +drawDeltaX;

                            //Trace.TraceInformation( "chip={0:x2}, EInstrumentPart={1}, x={2}", pChip.nChannelNumber, inst, x );
                            if (bChipHasR)
                            {
                                if (inst == EInstrumentPart.GUITAR)
                                {
                                    this.txチップ.tDraw2D(CDTXMania.app.Device, (CDTXMania.ConfigIni.bLeft.Guitar ? 244 : 88), y - chipHeight / 2, new Rectangle(0, 0, 38, 10));
                                }
                                else if (inst == EInstrumentPart.BASS)
                                {
                                    this.txチップ.tDraw2D(CDTXMania.app.Device, (CDTXMania.ConfigIni.bLeft.Bass ? 1115 : 959), y - chipHeight / 2, new Rectangle(0, 0, 38, 10));
                                }
                            }
                            if (bChipHasG)
                            {
                                if (inst == EInstrumentPart.GUITAR)
                                {
                                    this.txチップ.tDraw2D(CDTXMania.app.Device, (CDTXMania.ConfigIni.bLeft.Guitar ? 205 : 127), y - chipHeight / 2, new Rectangle(38, 0, 38, 10));
                                }
                                else if (inst == EInstrumentPart.BASS)
                                {
                                    this.txチップ.tDraw2D(CDTXMania.app.Device, (CDTXMania.ConfigIni.bLeft.Bass ? 1076 : 998), y - chipHeight / 2, new Rectangle(38, 0, 38, 10));
                                }
                            }
                            if (bChipHasB)
                            {
                                if (inst == EInstrumentPart.GUITAR)
                                {
                                    this.txチップ.tDraw2D(CDTXMania.app.Device, 166, y - chipHeight / 2, new Rectangle(76, 0, 38, 10));
                                }
                                else if (inst == EInstrumentPart.BASS)
                                {
                                    this.txチップ.tDraw2D(CDTXMania.app.Device, 1036, y - chipHeight / 2, new Rectangle(76, 0, 38, 10));
                                }
                            }
                            if (bChipHasY)
                            {
                                if (inst == EInstrumentPart.GUITAR)
                                {
                                    this.txチップ.tDraw2D(CDTXMania.app.Device, (CDTXMania.ConfigIni.bLeft.Guitar ? 127 : 205), y - chipHeight / 2, new Rectangle(114, 0, 38, 10));
                                }
                                else if (inst == EInstrumentPart.BASS)
                                {
                                    this.txチップ.tDraw2D(CDTXMania.app.Device, (CDTXMania.ConfigIni.bLeft.Bass ? 998 : 1076), y - chipHeight / 2, new Rectangle(114, 0, 38, 10));
                                }
                            }
                            if (bChipHasP)
                            {
                                if (inst == EInstrumentPart.GUITAR)
                                {
                                    this.txチップ.tDraw2D(CDTXMania.app.Device, (CDTXMania.ConfigIni.bLeft.Guitar ? 88 : 244), y - chipHeight / 2, new Rectangle(152, 0, 38, 10));
                                }
                                else if (inst == EInstrumentPart.BASS)
                                {
                                    this.txチップ.tDraw2D(CDTXMania.app.Device, (CDTXMania.ConfigIni.bLeft.Bass ? 959 : 1115), y - chipHeight / 2, new Rectangle(152, 0, 38, 10));
                                }
                            }
                        }
                    }
                }
                #endregion
				//if ( ( configIni.bAutoPlay.Guitar && !pChip.bHit ) && ( pChip.nDistanceFromBar.Guitar < 0 ) )
				//if ( ( !pChip.bHit ) && ( pChip.nDistanceFromBar[ instIndex ] < 0 ) )
                
                // #35411 2015.08.20 chnmr0 modified
                // 従来のAUTO処理に加えてプレーヤーゴーストの再生機能を追加
                bool autoPlayCondition = (!pChip.bHit) && (pChip.nDistanceFromBar[instIndex] < 0);
				if ( autoPlayCondition )
                {
                    //cInvisibleChip.StartSemiInvisible( inst );
                }

                bool autoPick = ( inst == EInstrumentPart.GUITAR ) ? bIsAutoPlay.GtPick : bIsAutoPlay.BsPick;
                autoPlayCondition = !pChip.bHit && autoPick;
                long ghostLag = 0;
                bool bUsePerfectGhost = true;

                if ( (pChip.eInstrumentPart == EInstrumentPart.GUITAR || pChip.eInstrumentPart == EInstrumentPart.BASS ) &&
                    CDTXMania.ConfigIni.eAutoGhost[(int)(pChip.eInstrumentPart)] != EAutoGhostData.PERFECT &&
                    CDTXMania.listAutoGhostLag[(int)pChip.eInstrumentPart] != null &&
                    0 <= pChip.n楽器パートでの出現順 &&
                    pChip.n楽器パートでの出現順 < CDTXMania.listAutoGhostLag[(int)pChip.eInstrumentPart].Count)
                {
                	// #35411 (mod) Ghost data が有効なので 従来のAUTOではなくゴーストのラグを利用
                	// 発生時刻と現在時刻からこのタイミングで演奏するかどうかを決定
					ghostLag = CDTXMania.listAutoGhostLag[(int)pChip.eInstrumentPart][pChip.n楽器パートでの出現順];
					bool resetCombo = ghostLag > 255;
					ghostLag = (ghostLag & 255) - 128;
					ghostLag -= (pChip.eInstrumentPart == EInstrumentPart.GUITAR ? nInputAdjustTimeMs.Guitar : nInputAdjustTimeMs.Bass);
                    autoPlayCondition &= (pChip.nPlaybackTimeMs + ghostLag <= CSoundManager.rcPerformanceTimer.n現在時刻ms);
					if (resetCombo && autoPlayCondition )
					{
						this.actCombo.nCurrentCombo[(int)pChip.eInstrumentPart] = 0;
					}
					bUsePerfectGhost = false;
                }

                if( bUsePerfectGhost )
                {
                	// 従来のAUTOを使用する場合
                    autoPlayCondition &= ( pChip.nDistanceFromBar[ instIndex ] < 0 );
                }

                if ( autoPlayCondition )
                {
                    int lo = (inst == EInstrumentPart.GUITAR) ? 0 : 5;	// lane offset
                    bool autoR = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtR : bIsAutoPlay.BsR;
                    bool autoG = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtG : bIsAutoPlay.BsG;
                    bool autoB = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtB : bIsAutoPlay.BsB;
                    bool autoY = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtY : bIsAutoPlay.BsY;
                    bool autoP = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtP : bIsAutoPlay.BsP;
                    bool pushingR = CDTXMania.Pad.bPressing(inst, EPad.R);
                    bool pushingG = CDTXMania.Pad.bPressing(inst, EPad.G);
                    bool pushingB = CDTXMania.Pad.bPressing(inst, EPad.B);
                    bool pushingY = CDTXMania.Pad.bPressing(inst, EPad.Y);
                    bool pushingP = CDTXMania.Pad.bPressing(inst, EPad.P);

					#region [ Chip Fire effects (auto時用) ]
                    // autoPickでない時の処理は、 tHandleInput_GuitarBass(EInstrumentPart) で行う
					bool bSuccessOPEN = bChipIsO && ( autoR || !pushingR ) && ( autoG || !pushingG ) && ( autoB || !pushingB ) && ( autoY || !pushingY ) && ( autoP || !pushingP );
					if ( ( bChipHasR && ( autoR || pushingR ) && autoPick ) || bSuccessOPEN && autoPick )
					{
						this.actChipFireGB.Start( 0 + lo );
					}
					if ( ( bChipHasG && ( autoG || pushingG ) && autoPick ) || bSuccessOPEN && autoPick )
					{
						this.actChipFireGB.Start( 1 + lo );
					}
					if ( ( bChipHasB && ( autoB || pushingB ) && autoPick ) || bSuccessOPEN && autoPick )
					{
						this.actChipFireGB.Start( 2 + lo );
					}
                    if ( ( bChipHasY && ( autoY || pushingY ) && autoPick ) || bSuccessOPEN && autoPick )
					{
						this.actChipFireGB.Start( 3 + lo );
					}
                    if ( ( bChipHasP && ( autoP || pushingP ) && autoPick ) || bSuccessOPEN && autoPick )
					{
						this.actChipFireGB.Start( 4 + lo );
					}
					#endregion
					#region [ autopick ]
					if ( autoPick )
					{
						bool bMiss = true;
						if ( bChipHasR == autoR && bChipHasG == autoG && bChipHasB == autoB && bChipHasY == autoY && bChipHasP == autoP )		// autoレーンとチップレーン一致時はOK
						{																			// この条件を加えないと、同時に非autoレーンを押下している時にNGとなってしまう。
							bMiss = false;
						}
						else if ( ( autoR || ( bChipHasR == pushingR ) ) && ( autoG || ( bChipHasG == pushingG ) ) && ( autoB || ( bChipHasB == pushingB ) ) && ( autoY || ( bChipHasY == pushingY ) ) && ( autoP || bChipHasP == pushingP ) )
							// ( bChipHasR == ( pushingR | autoR ) ) && ( bChipHasG == ( pushingG | autoG ) ) && ( bChipHasB == ( pushingB | autoB ) ) )
						{
							bMiss = false;
						}
						else if ( ( ( bChipIsO == true ) && ( !pushingR | autoR ) && ( !pushingG | autoG ) && ( !pushingB | autoB ) && ( !pushingY | autoY) && ( !pushingP | autoP) ) )	// OPEN時
						{
							bMiss = false;
						}
						pChip.bHit = true;
						this.tPlaySound( pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs + ghostLag, inst, dTX.nモニタを考慮した音量( inst ), false, bMiss );
						this.rNextGuitarChip = null;
						if ( !bMiss )
						{
							this.tProcessChipHit( pChip.nPlaybackTimeMs + ghostLag, pChip );
						}
						else
						{
							pChip.nLag = 0;		// tProcessChipHit()の引数最後がfalseの時はpChip.nLagを計算しないため、ここでAutoPickかつMissのLag=0を代入
							this.tProcessChipHit( pChip.nPlaybackTimeMs + ghostLag, pChip, false );
						}
						int chWailingChip = ( inst == EInstrumentPart.GUITAR ) ? 0x28 : 0xA8;
						CDTX.CChip item = this.r指定時刻に一番近い未ヒットChip( pChip.nPlaybackTimeMs + ghostLag, chWailingChip, this.nInputAdjustTimeMs[ instIndex ], 140 );
						if ( item != null && !bMiss )
						{
							this.queWailing[ instIndex ].Enqueue( item );
						}
					}
					#endregion

					// #35411 modify end
				}

                if( pChip.eInstrumentPart == EInstrumentPart.GUITAR && CDTXMania.ConfigIni.bGraph有効.Guitar )
                {
                    #region[ ギターゴースト ]
                    if (CDTXMania.ConfigIni.eTargetGhost.Guitar != ETargetGhostData.NONE &&
                        CDTXMania.listTargetGhsotLag.Guitar != null)
                    {
                        double val = 0;
                        if (CDTXMania.ConfigIni.eTargetGhost.Guitar == ETargetGhostData.ONLINE)
                        {
                            if (CDTXMania.DTX.nVisibleChipsCount.Guitar > 0)
                            {
                            	// Online Stats の計算式
                                val = 100 *
                                    (this.nヒット数_TargetGhost.Guitar.Perfect * 17 +
                                     this.nヒット数_TargetGhost.Guitar.Great * 7 +
                                     this.n最大コンボ数_TargetGhost.Guitar * 3) / (20.0 * CDTXMania.DTX.nVisibleChipsCount.Guitar);
                            }
                        }
                        else
                        {
                            if( CDTXMania.ConfigIni.nSkillMode == 0 )
                            {
                                val = CScoreIni.tCalculatePlayingSkillOld(
                                    CDTXMania.DTX.nVisibleChipsCount.Guitar,
                                    this.nヒット数_TargetGhost.Guitar.Perfect,
                                    this.nヒット数_TargetGhost.Guitar.Great,
                                    this.nヒット数_TargetGhost.Guitar.Good,
                                    this.nヒット数_TargetGhost.Guitar.Poor,
                                    this.nヒット数_TargetGhost.Guitar.Miss,
                                    this.n最大コンボ数_TargetGhost.Guitar,
                                    EInstrumentPart.GUITAR, new STAUTOPLAY());
                            }
                            else
                            {
                                val = CScoreIni.tCalculatePlayingSkill(
                                    CDTXMania.DTX.nVisibleChipsCount.Guitar,
                                    this.nヒット数_TargetGhost.Guitar.Perfect,
                                    this.nヒット数_TargetGhost.Guitar.Great,
                                    this.nヒット数_TargetGhost.Guitar.Good,
                                    this.nヒット数_TargetGhost.Guitar.Poor,
                                    this.nヒット数_TargetGhost.Guitar.Miss,
                                    this.n最大コンボ数_TargetGhost.Guitar,
                                    EInstrumentPart.GUITAR, new STAUTOPLAY());
                            }

                        }
                        if (val < 0) val = 0;
                        if (val > 100) val = 100;
                        this.actGraph.dbGraphValue_Goal = val;
                    }
                    #endregion
                }
                else if( pChip.eInstrumentPart == EInstrumentPart.BASS && CDTXMania.ConfigIni.bGraph有効.Bass )
                {
                    #region[ ベースゴースト ]
                    if (CDTXMania.ConfigIni.eTargetGhost.Bass != ETargetGhostData.NONE &&
                        CDTXMania.listTargetGhsotLag.Bass != null)
                    {
                        double val = 0;
                        if (CDTXMania.ConfigIni.eTargetGhost.Bass == ETargetGhostData.ONLINE)
                        {
                            if (CDTXMania.DTX.nVisibleChipsCount.Bass > 0)
                            {
                            	// Online Stats の計算式
                                val = 100 *
                                    (this.nヒット数_TargetGhost.Bass.Perfect * 17 +
                                     this.nヒット数_TargetGhost.Bass.Great * 7 +
                                     this.n最大コンボ数_TargetGhost.Bass * 3) / (20.0 * CDTXMania.DTX.nVisibleChipsCount.Bass);
                            }
                        }
                        else
                        {
                            if( CDTXMania.ConfigIni.nSkillMode == 0 )
                            {
                                val = CScoreIni.tCalculatePlayingSkillOld(
                                    CDTXMania.DTX.nVisibleChipsCount.Bass,
                                    this.nヒット数_TargetGhost.Bass.Perfect,
                                    this.nヒット数_TargetGhost.Bass.Great,
                                    this.nヒット数_TargetGhost.Bass.Good,
                                    this.nヒット数_TargetGhost.Bass.Poor,
                                    this.nヒット数_TargetGhost.Bass.Miss,
                                    this.n最大コンボ数_TargetGhost.Bass,
                                    EInstrumentPart.BASS, new STAUTOPLAY());
                            }
                            else
                            {
                                val = CScoreIni.tCalculatePlayingSkill(
                                    CDTXMania.DTX.nVisibleChipsCount.Bass,
                                    this.nヒット数_TargetGhost.Bass.Perfect,
                                    this.nヒット数_TargetGhost.Bass.Great,
                                    this.nヒット数_TargetGhost.Bass.Good,
                                    this.nヒット数_TargetGhost.Bass.Poor,
                                    this.nヒット数_TargetGhost.Bass.Miss,
                                    this.n最大コンボ数_TargetGhost.Bass,
                                    EInstrumentPart.BASS, new STAUTOPLAY());
                            }

                        }
                        if (val < 0) val = 0;
                        if (val > 100) val = 100;
                        this.actGraph.dbGraphValue_Goal = val;
                    }
                    #endregion
                }

				return;
			}	// end of "if configIni.bGuitarEnabled"
			if ( !pChip.bHit && ( pChip.nDistanceFromBar[ instIndex ] < 0 ) )	// Guitar/Bass無効の場合は、自動演奏する
			{
				pChip.bHit = true;
				this.tPlaySound( pChip, CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻 + pChip.nPlaybackTimeMs, inst, dTX.nモニタを考慮した音量( inst ) );
			}
        }


        protected virtual void tUpdateAndDraw_Chip_GuitarBass_Wailing(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip, EInstrumentPart inst)
        {
            int indexInst = (int)inst;
            if (configIni.bGuitarEnabled)
            {
                #region [ Hidden/Sudden処理 ]
                #region [ Sudden処理 ]
                if ((CDTXMania.ConfigIni.nHidSud[indexInst] == 2) || (CDTXMania.ConfigIni.nHidSud[indexInst] == 3))
                {
                    if (pChip.nDistanceFromBar[indexInst] < 250)
                    {
                        pChip.bVisible = true;
                        pChip.nTransparency = 0xff;
                    }
                    else if (pChip.nDistanceFromBar[indexInst] < 300)
                    {
                        pChip.bVisible = true;
                        pChip.nTransparency = 0xff - ((int)((((double)(pChip.nDistanceFromBar[indexInst] - 250)) * 255.0) / 75.0));
                    }
                    else
                    {
                        pChip.bVisible = false;
                        pChip.nTransparency = 0;
                    }
                }
                #endregion
                #region [ Hidden処理 ]
                if ((CDTXMania.ConfigIni.nHidSud[indexInst] == 1) || (CDTXMania.ConfigIni.nHidSud[indexInst] == 3))
                {
                    if (pChip.nDistanceFromBar[indexInst] < 150)
                    {
                        pChip.bVisible = false;
                    }
                    else if (pChip.nDistanceFromBar[indexInst] < 200)
                    {
                        pChip.bVisible = true;
                        pChip.nTransparency = (int)((((double)(pChip.nDistanceFromBar[indexInst] - 150)) * 255.0) / 75.0);
                    }
                }
                #endregion
                #region [ ステルス処理 ]
                if (CDTXMania.ConfigIni.nHidSud[indexInst] == 4)
                {
                    pChip.bVisible = false;
                }
                if (this.txチップ != null)
                {
                    this.txチップ.nTransparency = pChip.nTransparency;
                }
                #endregion
                #endregion
                //
                // ここにチップ更新処理が入る(overrideで入れる)。といっても座標とチップサイズが違うだけで処理はまるまる同じ。
                //
                if (!pChip.bHit && (pChip.nDistanceFromBar[indexInst] < 0))
                {
                    if (pChip.nDistanceFromBar[indexInst] < -234)	// #25253 2011.5.29 yyagi: Don't set pChip.bHit=true for wailing at once. It need to 1sec-delay (234pix per 1sec). 
                    {
                        pChip.bHit = true;
                    }
                    bool autoW = (inst == EInstrumentPart.GUITAR) ? configIni.bAutoPlay.GtW : configIni.bAutoPlay.BsW;
                    //if ( configIni.bAutoPlay[ ((int) ELane.Guitar - 1) + indexInst ] )	// このような、バグの入りやすい書き方(GT/BSのindex値が他と異なる)はいずれ見直したい
                    if (autoW)
                    {
                        //    pChip.bHit = true;								// #25253 2011.5.29 yyagi: Set pChip.bHit=true if autoplay.
                        //    this.actWailingBonus.Start( inst, this.r現在の歓声Chip[indexInst] );
                        // #23886 2012.5.22 yyagi; To support auto Wailing; Don't do wailing for ALL wailing chips. Do wailing for queued wailing chip.
                        // wailing chips are queued when 1) manually wailing and not missed at that time 2) AutoWailing=ON and not missed at that time
                        long nTimeStamp_Wailed = pChip.nPlaybackTimeMs + CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻;
                        DoWailingFromQueue(inst, nTimeStamp_Wailed, autoW);
                    }
                }
                return;
            }
            pChip.bHit = true;
        }
        protected virtual void tUpdateAndDraw_Chip_Guitar_Wailing(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip)
        {
            tUpdateAndDraw_Chip_GuitarBass_Wailing(configIni, ref dTX, ref pChip, EInstrumentPart.GUITAR);
        }
        protected abstract void t進行描画_チップ_フィルイン(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip);
        protected abstract void t進行描画_チップ_ボーナス(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip);
        protected void t進行描画_フィルインエフェクト()
        {
            this.actFillin.OnUpdateAndDraw();
        }
        protected abstract void tUpdateAndDraw_Chip_BarLine(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip);
        //protected abstract void t進行描画_チップ_ベース( CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip );
        protected virtual void t進行描画_チップ_ベース_ウェイリング(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip)
        {
            tUpdateAndDraw_Chip_GuitarBass_Wailing(configIni, ref dTX, ref pChip, EInstrumentPart.BASS);
        }
        protected abstract void t進行描画_チップ_空打ち音設定_ドラム(CConfigIni configIni, ref CDTX dTX, ref CDTX.CChip pChip);
        protected void tUpdateAndDraw_ChipAnimation()
        {
            for (int i = 0; i < 3; i++)			// 0=drums, 1=guitar, 2=bass
            {
                if (this.ctChipPatternAnimation[i] != null)
                {
                    this.ctChipPatternAnimation[i].tUpdateLoop();
                }
            }
            if (this.ctWailingChipPatternAnimation != null)
            {
                this.ctWailingChipPatternAnimation.tUpdateLoop();
            }
            if (this.ctBPMBar != null)
            {
                this.ctBPMBar.tUpdateLoopDb();
                this.ctComboTimer.tUpdateLoopDb();
            }
            if (CDTXMania.ConfigIni.bDrumsEnabled)  //2013.05.16.kairera0467 ギター側のアニメーションは未実装なのでとりあえず。
                this.ct登場用.tUpdate();
        }

        protected bool tUpdateAndDraw_FadeIn_Out()
        {
            switch (base.ePhaseID)
            {
                case CStage.EPhase.Common_FadeIn:
                    if (this.actFI.OnUpdateAndDraw() != 0)
                    {
                        base.ePhaseID = CStage.EPhase.Common_DefaultState;
                    }
                    break;

                case CStage.EPhase.Common_FadeOut:
                case CStage.EPhase.演奏_STAGE_FAILED_フェードアウト:
                    if (this.actFO.OnUpdateAndDraw() != 0)
                    {
                        return true;
                    }
                    break;

                case CStage.EPhase.演奏_STAGE_CLEAR_フェードアウト:
                    if (this.actFOStageClear.OnUpdateAndDraw() == 0)
                    {
                        break;
                    }
                    return true;

            }
            return false;
        }
        protected void tUpdateAndDraw_LaneFlushD()
        {
            if ((base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED) && (base.ePhaseID != CStage.EPhase.演奏_STAGE_FAILED_フェードアウト))
            {
                this.actLaneFlushD.OnUpdateAndDraw();
            }
        }
        protected void tUpdateAndDraw_LaneFlushGB()
        {
            if (CDTXMania.ConfigIni.bGuitarEnabled)
            {
                this.actLaneFlushGB.OnUpdateAndDraw();
            }
        }
        protected abstract void tUpdateAndDraw_PerformanceInformation();
        protected void tUpdateAndDraw_PerformanceInformation(int x, int y)
        {
            if (!CDTXMania.ConfigIni.b演奏情報を表示しない)
            {
                this.actPlayInfo.t進行描画(x, y);
            }
        }
        protected void tUpdateAndDraw_Background()
        {

            if (this.tx背景 != null)
            {
                this.tx背景.tDraw2D(CDTXMania.app.Device, 0, 0);
            }
            //CDTXMania.app.Device.Clear( ClearFlags.ZBuffer | ClearFlags.Target, Color.Black, 0f, 0 );
        }

        protected void t進行描画_判定ライン()
        {
            if (CDTXMania.ConfigIni.bDrumsEnabled)
            {
                int y = CDTXMania.ConfigIni.bReverse.Drums ? nJudgeLinePosY.Drums - nJudgeLinePosY_delta.Drums : nJudgeLinePosY.Drums + nJudgeLinePosY_delta.Drums;
                if (CDTXMania.ConfigIni.bJudgeLineDisp.Drums)
                {
                    // #31602 2013.6.23 yyagi 描画遅延対策として、判定ラインの表示位置をオフセット調整できるようにする
                    // Check for numofLanes config to decide on the length to draw
                    int l_drumPanelWidth = 0x22f;
                    int l_xOffset = 0;
                    if (CDTXMania.ConfigIni.eNumOfLanes.Drums == EType.B)
                    {
                        l_drumPanelWidth = 0x207;
                    }
                    else if (CDTXMania.ConfigIni.eNumOfLanes.Drums == EType.C)
                    {
                        l_drumPanelWidth = 447;
                        l_xOffset = 72;
                    }
                    this.txヒットバー.tDraw2D(CDTXMania.app.Device, 295 + l_xOffset, y, new Rectangle(0, 0, l_drumPanelWidth, 6));
                }
                if (CDTXMania.ConfigIni.b演奏情報を表示する)
                    this.actLVFont.tDrawString(295, (CDTXMania.ConfigIni.bReverse.Drums ? y - 20 : y + 8), CDTXMania.ConfigIni.nJudgeLine.Drums.ToString());
            }
        }

        protected void t進行描画_判定文字列()
        {
            this.actJudgeString.OnUpdateAndDraw();
        }
        protected void t進行描画_判定文字列1_通常位置指定の場合()
        {
            if (((EType)CDTXMania.ConfigIni.判定文字表示位置.Drums) != EType.B)
            {
                this.actJudgeString.OnUpdateAndDraw();
            }
        }
        protected void t進行描画_判定文字列2_判定ライン上指定の場合()
        {
            if (((EType)CDTXMania.ConfigIni.判定文字表示位置.Drums) == EType.B)
            {
                this.actJudgeString.OnUpdateAndDraw();
            }
        }

        protected void tUpdateAndDraw_ScrollSpeed()
        {
            this.actScrollSpeed.OnUpdateAndDraw();
        }

        protected abstract void tGenerateBackgroundTexture();



        protected void tGenerateBackgroundTexture(string DefaultBgFilename, Rectangle bgrect, string bgfilename)
        {
            Bitmap image = null;
            bool flag = true;

            if (bgfilename != null && File.Exists(bgfilename))
            {
                try
                {
                    Bitmap bitmap2 = null;
                    bitmap2 = new Bitmap(bgfilename);
                    if ((bitmap2.Size.Width == 0) && (bitmap2.Size.Height == 0))
                    {
                        this.tx背景 = null;
                        return;
                    }
                    Bitmap bitmap3 = new Bitmap(SampleFramework.GameWindowSize.Width, SampleFramework.GameWindowSize.Height);
                    Graphics graphics = Graphics.FromImage(bitmap3);
                    for (int i = 0; i < SampleFramework.GameWindowSize.Height; i += bitmap2.Size.Height)
                    {
                        for (int j = 0; j < SampleFramework.GameWindowSize.Width; j += bitmap2.Size.Width)
                        {
                            graphics.DrawImage(bitmap2, j, i, bitmap2.Width, bitmap2.Height);
                        }
                    }
                    graphics.Dispose();
                    bitmap2.Dispose();
                    image = new Bitmap(CSkin.Path(DefaultBgFilename));
                    graphics = Graphics.FromImage(image);
                    ColorMatrix matrix2 = new ColorMatrix();
                    matrix2.Matrix00 = 1f;
                    matrix2.Matrix11 = 1f;
                    matrix2.Matrix22 = 1f;
                    matrix2.Matrix33 = ((float)CDTXMania.ConfigIni.n背景の透過度) / 255f;
                    matrix2.Matrix44 = 1f;
                    ColorMatrix newColorMatrix = matrix2;
                    ImageAttributes imageAttr = new ImageAttributes();
                    imageAttr.SetColorMatrix(newColorMatrix);
                    graphics.DrawImage(bitmap3, new Rectangle(0, 0, SampleFramework.GameWindowSize.Width, SampleFramework.GameWindowSize.Height), 0, 0, SampleFramework.GameWindowSize.Width, SampleFramework.GameWindowSize.Height, GraphicsUnit.Pixel, imageAttr);
                    imageAttr.Dispose();
                    graphics.DrawImage(bitmap3, bgrect, bgrect.X, bgrect.Y, bgrect.Width, bgrect.Height, GraphicsUnit.Pixel);
                    graphics.Dispose();
                    bitmap3.Dispose();
                    flag = false;
                }
                catch
                {
                    Trace.TraceError("背景画像の読み込みに失敗しました。({0})", new object[] { bgfilename });
                }
            }
            if (flag)
            {
                bgfilename = CSkin.Path(DefaultBgFilename);
                try
                {
                    image = new Bitmap(bgfilename);
                }
                catch
                {
                    Trace.TraceError("背景画像の読み込みに失敗しました。({0})", new object[] { bgfilename });
                    this.tx背景 = null;
                    return;
                }
            }
            if ((CDTXMania.DTX.listBMP.Count > 0) || (CDTXMania.DTX.listBMPTEX.Count > 0))
            {
                Graphics graphics2 = Graphics.FromImage(image);
                graphics2.FillRectangle(Brushes.Black, bgrect.X, bgrect.Y, bgrect.Width, bgrect.Height);
                graphics2.Dispose();
            }
            try
            {
                this.tx背景 = new CTexture(CDTXMania.app.Device, image, CDTXMania.TextureFormat);
            }
            catch (CTextureCreateFailedException)
            {
                Trace.TraceError("背景テクスチャの生成に失敗しました。");
                this.tx背景 = null;
            }
            image.Dispose();
        }

        protected virtual void tHandleInput_Guitar()
        {
            tHandleInput_GuitarBass(EInstrumentPart.GUITAR);
        }
        protected virtual void tHandleInput_Bass()
        {
            tHandleInput_GuitarBass(EInstrumentPart.BASS);
        }


        protected virtual void tHandleInput_GuitarBass(EInstrumentPart inst)
        {
            int indexInst = (int)inst;
            #region [ スクロール速度変更 ]
            if (CDTXMania.Pad.bPressing(inst, EPad.Decide) && CDTXMania.Pad.bPressed(inst, EPad.B))
            {
                CDTXMania.ConfigIni.nScrollSpeed[indexInst] = Math.Min(CDTXMania.ConfigIni.nScrollSpeed[indexInst] + 1, 0x7cf);
            }
            if (CDTXMania.Pad.bPressing(inst, EPad.Decide) && CDTXMania.Pad.bPressed(inst, EPad.R))
            {
                CDTXMania.ConfigIni.nScrollSpeed[indexInst] = Math.Max(CDTXMania.ConfigIni.nScrollSpeed[indexInst] - 1, 0);
            }
            #endregion

            if (!CDTXMania.ConfigIni.bGuitarEnabled || !CDTXMania.DTX.bチップがある[indexInst])
            {
                return;
            }

            int R = (inst == EInstrumentPart.GUITAR) ? 0 : 5;
            int G = R + 1;
            int B = R + 2;
            int Y = R + 3;
            int P = R + 4;
            bool autoW = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtW : bIsAutoPlay.BsW;
            bool autoR = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtR : bIsAutoPlay.BsR;
            bool autoG = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtG : bIsAutoPlay.BsG;
            bool autoB = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtB : bIsAutoPlay.BsB;
            bool autoY = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtY : bIsAutoPlay.BsY;
            bool autoP = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtP : bIsAutoPlay.BsP;
            bool autoPick = (inst == EInstrumentPart.GUITAR) ? bIsAutoPlay.GtPick : bIsAutoPlay.BsPick;
            int nAutoW = (autoW) ? 8 : 0;
            int nAutoR = (autoR) ? 4 : 0;
            int nAutoG = (autoG) ? 2 : 0;
            int nAutoB = (autoB) ? 1 : 0;
            int nAutoY = (autoY) ? 16 : 0;
            int nAutoP = (autoP) ? 32 : 0;
            int nAutoMask = nAutoW | nAutoR | nAutoG | nAutoB | nAutoY | nAutoP;

            //			if ( bIsAutoPlay[ (int) ELane.Guitar - 1 + indexInst ] )	// このような、バグの入りやすい書き方(GT/BSのindex値が他と異なる)はいずれ見直したい
            //			{
            CDTX.CChip chip = this.r次に来る指定楽器Chipを更新して返す(inst);
            if (chip != null)
            {
                bool bAutoGuitarR = false;
                bool bAutoGuitarG = false;
                bool bAutoGuitarB = false;
                bool bAutoGuitarY = false;
                bool bAutoGuitarP = false;
                bool bAutoBassR = false;
                bool bAutoBassG = false;
                bool bAutoBassB = false;
                bool bAutoBassY = false;
                bool bAutoBassP = false;

                switch (chip.nChannelNumber)
                {
                    case 0x20:
                        break;
                    case 0x21:
                        bAutoGuitarB = true;
                        break;
                    case 0x22:
                        bAutoGuitarG = true;
                        break;
                    case 0x23:
                        bAutoGuitarG = true;
                        bAutoGuitarB = true;
                        break;
                    case 0x24:
                        bAutoGuitarR = true;
                        break;
                    case 0x25:
                        bAutoGuitarR = true;
                        bAutoGuitarB = true;
                        break;
                    case 0x26:
                        bAutoGuitarR = true;
                        bAutoGuitarG = true;
                        break;
                    case 0x27:
                        bAutoGuitarR = true;
                        bAutoGuitarG = true;
                        bAutoGuitarB = true;
                        break;

                    case 0x93:
                        bAutoGuitarY = true;
                        break;
                    case 0x94:
                        bAutoGuitarB = true;
                        bAutoGuitarY = true;
                        break;
                    case 0x95:
                        bAutoGuitarG = true;
                        bAutoGuitarY = true;
                        break;
                    case 0x96:
                        bAutoGuitarG = true;
                        bAutoGuitarB = true;
                        bAutoGuitarY = true;
                        break;
                    case 0x97:
                        bAutoGuitarR = true;
                        bAutoGuitarY = true;
                        break;
                    case 0x98:
                        bAutoGuitarR = true;
                        bAutoGuitarB = true;
                        bAutoGuitarY = true;
                        break;
                    case 0x99:
                        bAutoGuitarR = true;
                        bAutoGuitarG = true;
                        bAutoGuitarY = true;
                        break;
                    case 0x9A:
                        bAutoGuitarR = true;
                        bAutoGuitarG = true;
                        bAutoGuitarB = true;
                        bAutoGuitarY = true;
                        break;
                    case 0x9B:
                        bAutoGuitarP = true;
                        break;
                    case 0x9C:
                        bAutoGuitarB = true;
                        bAutoGuitarP = true;
                        break;
                    case 0x9D:
                        bAutoGuitarG = true;
                        bAutoGuitarP = true;
                        break;
                    case 0x9E:
                        bAutoGuitarG = true;
                        bAutoGuitarB = true;
                        bAutoGuitarP = true;
                        break;
                    case 0x9F:
                        bAutoGuitarR = true;
                        bAutoGuitarP = true;
                        break;

                    //BASS
                    case 0xA1:
                        bAutoBassB = true;
                        break;

                    case 0xA2:
                        bAutoBassG = true;
                        break;

                    case 0xA3:
                        bAutoBassG = true;
                        bAutoBassB = true;
                        break;

                    case 0xA4:
                        bAutoBassR = true;
                        break;

                    case 0xA5:
                        bAutoBassR = true;
                        bAutoBassB = true;
                        break;

                    case 0xA6:
                        bAutoBassR = true;
                        bAutoBassG = true;
                        break;

                    case 0xA7:
                        bAutoBassR = true;
                        bAutoBassG = true;
                        bAutoBassB = true;
                        break;

                    //A8 WAILING(BASS)

                    case 0xA9:
                        bAutoGuitarR = true;
                        bAutoGuitarB = true;
                        bAutoGuitarP = true;
                        break;

                    case 0xAA:
                        bAutoGuitarR = true;
                        bAutoGuitarG = true;
                        bAutoGuitarP = true;
                        break;

                    case 0xAB:
                        bAutoGuitarR = true;
                        bAutoGuitarG = true;
                        bAutoGuitarB = true;
                        bAutoGuitarP = true;
                        break;

                    case 0xAC:
                        bAutoGuitarY = true;
                        bAutoGuitarP = true;
                        break;

                    case 0xAD:
                        bAutoGuitarB = true;
                        bAutoGuitarY = true;
                        bAutoGuitarP = true;
                        break;

                    case 0xAE:
                        bAutoGuitarG = true;
                        bAutoGuitarY = true;
                        bAutoGuitarP = true;
                        break;

                    case 0xAF:
                        bAutoGuitarG = true;
                        bAutoGuitarB = true;
                        bAutoGuitarY = true;
                        bAutoGuitarP = true;
                        break;

                    case 0xC5:
                        bAutoBassY = true;
                        break;

                    case 0xC6:
                        bAutoBassB = true;
                        bAutoBassY = true;
                        break;

                    case 0xC8:
                        bAutoBassG = true;
                        bAutoBassY = true;
                        break;

                    case 0xC9:
                        bAutoBassG = true;
                        bAutoBassB = true;
                        bAutoBassY = true;
                        break;

                    case 0xCA:
                        bAutoBassR = true;
                        bAutoBassY = true;
                        break;

                    case 0xCB:
                        bAutoBassR = true;
                        bAutoBassB = true;
                        bAutoBassY = true;
                        break;
                    case 0xCC:
                        bAutoBassR = true;
                        bAutoBassG = true;
                        bAutoBassY = true;
                        break;
                    case 0xCD:
                        bAutoBassR = true;
                        bAutoBassG = true;
                        bAutoBassB = true;
                        bAutoBassY = true;
                        break;
                    case 0xCE:
                        bAutoBassP = true;
                        break;
                    case 0xCF:
                        bAutoBassB = true;
                        bAutoBassP = true;
                        break;

                    case 0xD0:
                        bAutoGuitarR = true;
                        bAutoGuitarY = true;
                        bAutoGuitarP = true;
                        break;
                    case 0xD1:
                        bAutoGuitarR = true;
                        bAutoGuitarB = true;
                        bAutoGuitarY = true;
                        bAutoGuitarP = true;
                        break;
                    case 0xD2:
                        bAutoGuitarR = true;
                        bAutoGuitarG = true;
                        bAutoGuitarY = true;
                        bAutoGuitarP = true;
                        break;
                    case 0xD3:
                        bAutoGuitarR = true;
                        bAutoGuitarG = true;
                        bAutoGuitarB = true;
                        bAutoGuitarY = true;
                        bAutoGuitarP = true;
                        break;

                    case 0xDA:
                        bAutoBassG = true;
                        bAutoBassP = true;
                        break;
                    case 0xDB:
                        bAutoBassG = true;
                        bAutoBassB = true;
                        bAutoBassP = true;
                        break;
                    case 0xDC:
                        bAutoBassR = true;
                        bAutoBassP = true;
                        break;
                    case 0xDD:
                        bAutoBassR = true;
                        bAutoBassB = true;
                        bAutoBassP = true;
                        break;
                    case 0xDE:
                        bAutoBassR = true;
                        bAutoBassG = true;
                        bAutoBassP = true;
                        break;
                    case 0xDF:
                        bAutoBassR = true;
                        bAutoBassG = true;
                        bAutoBassB = true;
                        bAutoBassP = true;
                        break;
                    case 0xE1:
                        bAutoBassY = true;
                        bAutoBassP = true;
                        break;
                    case 0xE2:
                        bAutoBassB = true;
                        bAutoBassY = true;
                        bAutoBassP = true;
                        break;
                    case 0xE3:
                        bAutoBassG = true;
                        bAutoBassY = true;
                        bAutoBassP = true;
                        break;
                    case 0xE4:
                        bAutoBassG = true;
                        bAutoBassB = true;
                        bAutoBassY = true;
                        bAutoBassP = true;
                        break;
                    case 0xE5:
                        bAutoBassR = true;
                        bAutoBassY = true;
                        bAutoBassP = true;
                        break;
                    case 0xE6:
                        bAutoBassR = true;
                        bAutoBassB = true;
                        bAutoBassY = true;
                        bAutoBassP = true;
                        break;
                    case 0xE7:
                        bAutoBassR = true;
                        bAutoBassG = true;
                        bAutoBassY = true;
                        bAutoBassP = true;
                        break;
                    case 0xE8:
                        bAutoBassR = true;
                        bAutoBassG = true;
                        bAutoBassB = true;
                        bAutoBassY = true;
                        bAutoBassP = true;
                        break;
                }
                //オートさん専用
                if (bAutoGuitarR && bIsAutoPlay.GtR)
                {
                    this.actLaneFlushGB.Start(0);
                    this.actRGB.Push(R);
                }
                if (bAutoGuitarG && bIsAutoPlay.GtG)
                {
                    this.actLaneFlushGB.Start(1);
                    this.actRGB.Push(G);
                }
                if (bAutoGuitarB && bIsAutoPlay.GtB)
                {
                    this.actLaneFlushGB.Start(2);
                    this.actRGB.Push(B);
                }
                if (bAutoGuitarY && bIsAutoPlay.GtY)
                {
                    this.actLaneFlushGB.Start(3);
                    this.actRGB.Push(Y);
                }
                if (bAutoGuitarP && bIsAutoPlay.GtP)
                {
                    this.actLaneFlushGB.Start(4);
                    this.actRGB.Push(P);
                }

                //バグが起こる場所(推定)　こ↑こ↓から
                if (bAutoBassR && bIsAutoPlay.BsR)
                {
                    this.actLaneFlushGB.Start(5);
                    this.actRGB.Push(R);
                }
                if (bAutoBassG && bIsAutoPlay.BsG)
                {
                    this.actLaneFlushGB.Start(6);
                    this.actRGB.Push(G);
                }
                if (bAutoBassB && bIsAutoPlay.BsB)
                {
                    this.actLaneFlushGB.Start(7);
                    this.actRGB.Push(B);
                }
                if (bAutoBassY && bIsAutoPlay.BsY)
                {
                    this.actLaneFlushGB.Start(8);
                    this.actRGB.Push(Y);
                }
                if (bAutoBassP && bIsAutoPlay.BsP)
                {
                    this.actLaneFlushGB.Start(9);
                    this.actRGB.Push(P);
                }
                //バグが起こる場所(推定)　こ↑こ↓まで
            }
            //			else
            {
                //オートさん以外
                int pushingR = CDTXMania.Pad.bPressing(inst, EPad.R) ? 4 : 0;
                this.tSaveInputMethod(inst);
                int pushingG = CDTXMania.Pad.bPressing(inst, EPad.G) ? 2 : 0;
                this.tSaveInputMethod(inst);
                int pushingB = CDTXMania.Pad.bPressing(inst, EPad.B) ? 1 : 0;
                this.tSaveInputMethod(inst);
                int pushingY = CDTXMania.Pad.bPressing(inst, EPad.Y) ? 16 : 0;
                this.tSaveInputMethod(inst);
                int pushingP = CDTXMania.Pad.bPressing(inst, EPad.P) ? 32 : 0;
                this.tSaveInputMethod(inst);
                int flagRGB = pushingR | pushingG | pushingB | pushingY | pushingP;
                if (pushingR != 0)
                {
                    this.actLaneFlushGB.Start(R);
                    this.actRGB.Push(R);
                }
                if (pushingG != 0)
                {
                    this.actLaneFlushGB.Start(G);
                    this.actRGB.Push(G);
                }
                if (pushingB != 0)
                {
                    this.actLaneFlushGB.Start(B);
                    this.actRGB.Push(B);
                }
                if (pushingY != 0)
                {
                    this.actLaneFlushGB.Start(Y);
                    this.actRGB.Push(Y);
                }
                if (pushingP != 0)
                {
                    this.actLaneFlushGB.Start(P);
                    this.actRGB.Push(P);
                }
                // auto pickだとここから先に行かないので注意
                List<STInputEvent> events = CDTXMania.Pad.GetEvents(inst, EPad.Pick);
                if ((events != null) && (events.Count > 0))
                {
                    foreach (STInputEvent eventPick in events)
                    {
                        if (!eventPick.b押された)
                        {
                            continue;
                        }
                        this.tSaveInputMethod(inst);
                        long nTime = eventPick.nTimeStamp - CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻;
                        int chWailingSound = (inst == EInstrumentPart.GUITAR) ? 0x2F : 0xAF;
                        CDTX.CChip pChip = this.r指定時刻に一番近い未ヒットChip(nTime, chWailingSound, this.nInputAdjustTimeMs[indexInst]);	// EInstrumentPart.GUITARなチップ全てにヒットする
                        EJudgement e判定 = this.e指定時刻からChipのJUDGEを返す(nTime, pChip, this.nInputAdjustTimeMs[indexInst]);
                        //Trace.TraceInformation("ch={0:x2}, mask1={1:x1}, mask2={2:x2}", pChip.nChannelNumber,  ( pChip.nChannelNumber & ~nAutoMask ) & 0x0F, ( flagRGB & ~nAutoMask) & 0x0F );
                        if (pChip != null)
                        {
                            bool bChipHasR = false;
                            bool bChipHasG = false;
                            bool bChipHasB = false;
                            bool bChipHasY = false;
                            bool bChipHasP = false;
                            bool bChipHasW = ((pChip.nChannelNumber & 0x0F) == 0x08);
                            bool bChipIsO = false;
                            bool bSuccessOPEN = bChipIsO && (autoR || pushingR == 0) && (autoG || pushingG == 0) && (autoB || pushingB == 0) && (autoY || pushingY == 0) && (autoP || pushingP == 0);

                            switch (pChip.nChannelNumber)
                            {
                                case 0x20:
                                    bChipIsO = true;
                                    break;
                                case 0x21:
                                    bChipHasB = true;
                                    break;
                                case 0x22:
                                    bChipHasG = true;
                                    break;
                                case 0x23:
                                    bChipHasG = true;
                                    bChipHasB = true;
                                    break;
                                case 0x24:
                                    bChipHasR = true;
                                    break;
                                case 0x25:
                                    bChipHasR = true;
                                    bChipHasB = true;
                                    break;
                                case 0x26:
                                    bChipHasR = true;
                                    bChipHasG = true;
                                    break;
                                case 0x27:
                                    bChipHasR = true;
                                    bChipHasG = true;
                                    bChipHasB = true;
                                    break;
                                case 0x28:
                                    bChipHasW = true;
                                    break;
                                default:
                                    switch (pChip.nChannelNumber)
                                    {
                                        case 0x93:
                                            bChipHasY = true;
                                            break;
                                        case 0x94:
                                            bChipHasB = true;
                                            bChipHasY = true;
                                            break;
                                        case 0x95:
                                            bChipHasG = true;
                                            bChipHasY = true;
                                            break;
                                        case 0x96:
                                            bChipHasG = true;
                                            bChipHasB = true;
                                            bChipHasY = true;
                                            break;
                                        case 0x97:
                                            bChipHasR = true;
                                            bChipHasY = true;
                                            break;
                                        case 0x98:
                                            bChipHasR = true;
                                            bChipHasB = true;
                                            bChipHasY = true;
                                            break;
                                        case 0x99:
                                            bChipHasR = true;
                                            bChipHasG = true;
                                            bChipHasY = true;
                                            break;
                                        //OK

                                        case 0x9A:
                                            bChipHasR = true;
                                            bChipHasG = true;
                                            bChipHasB = true;
                                            bChipHasY = true;
                                            break;
                                        case 0x9B:
                                            bChipHasP = true;
                                            break;
                                        case 0x9C:
                                            bChipHasB = true;
                                            bChipHasP = true;
                                            break;
                                        case 0x9D:
                                            bChipHasG = true;
                                            bChipHasP = true;
                                            break;
                                        case 0x9E:
                                            bChipHasG = true;
                                            bChipHasB = true;
                                            bChipHasP = true;
                                            break;
                                        case 0x9F:
                                            bChipHasR = true;
                                            bChipHasP = true;
                                            break;
                                        //OK

                                        case 0xA1:
                                            bChipHasB = true;
                                            break;
                                        case 0xA2:
                                            bChipHasG = true;
                                            break;
                                        case 0xA3:
                                            bChipHasG = true;
                                            bChipHasB = true;
                                            break;
                                        case 0xA4:
                                            bChipHasR = true;
                                            break;
                                        case 0xA5:
                                            bChipHasR = true;
                                            bChipHasB = true;
                                            break;
                                        case 0xA6:
                                            bChipHasR = true;
                                            bChipHasG = true;
                                            break;
                                        case 0xA7:
                                            bChipHasR = true;
                                            bChipHasG = true;
                                            bChipHasB = true;
                                            break;
                                        //OK
                                        case 0xA8:
                                            bChipHasW = true;
                                            break;

                                        case 0xA9:
                                            bChipHasR = true;
                                            bChipHasB = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xAA:
                                            bChipHasR = true;
                                            bChipHasG = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xAB:
                                            bChipHasR = true;
                                            bChipHasG = true;
                                            bChipHasB = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xAC:
                                            bChipHasY = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xAD:
                                            bChipHasB = true;
                                            bChipHasY = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xAE:
                                            bChipHasG = true;
                                            bChipHasY = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xAF:
                                            bChipHasG = true;
                                            bChipHasB = true;
                                            bChipHasY = true;
                                            bChipHasP = true;
                                            break;
                                        //OK

                                        case 0xC5:
                                            bChipHasY = true;
                                            break;
                                        case 0xC6:
                                            bChipHasB = true;
                                            bChipHasY = true;
                                            break;
                                        case 0xC8:
                                            bChipHasG = true;
                                            bChipHasY = true;
                                            break;
                                        case 0xC9:
                                            bChipHasG = true;
                                            bChipHasB = true;
                                            bChipHasY = true;
                                            break;
                                        case 0xCA:
                                            bChipHasR = true;
                                            bChipHasY = true;
                                            break;
                                        case 0xCB:
                                            bChipHasR = true;
                                            bChipHasB = true;
                                            bChipHasY = true;
                                            break;
                                        case 0xCC:
                                            bChipHasR = true;
                                            bChipHasG = true;
                                            bChipHasY = true;
                                            break;
                                        case 0xCD:
                                            bChipHasR = true;
                                            bChipHasG = true;
                                            bChipHasB = true;
                                            bChipHasY = true;
                                            break;
                                        case 0xCE:
                                            bChipHasP = true;
                                            break;
                                        case 0xCF:
                                            bChipHasB = true;
                                            bChipHasP = true;
                                            break;
                                        //OK

                                        case 0xD0:
                                            bChipHasR = true;
                                            bChipHasY = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xD1:
                                            bChipHasR = true;
                                            bChipHasB = true;
                                            bChipHasY = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xD2:
                                            bChipHasR = true;
                                            bChipHasG = true;
                                            bChipHasY = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xD3:
                                            bChipHasR = true;
                                            bChipHasG = true;
                                            bChipHasB = true;
                                            bChipHasY = true;
                                            bChipHasP = true;
                                            break;
                                        //OK

                                        case 0xDA:
                                            bChipHasG = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xDB:
                                            bChipHasG = true;
                                            bChipHasB = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xDC:
                                            bChipHasR = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xDD:
                                            bChipHasR = true;
                                            bChipHasB = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xDE:
                                            bChipHasR = true;
                                            bChipHasG = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xDF:
                                            bChipHasR = true;
                                            bChipHasG = true;
                                            bChipHasB = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xE1:
                                            bChipHasY = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xE2:
                                            bChipHasB = true;
                                            bChipHasY = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xE3:
                                            bChipHasG = true;
                                            bChipHasY = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xE4:
                                            bChipHasG = true;
                                            bChipHasB = true;
                                            bChipHasY = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xE5:
                                            bChipHasR = true;
                                            bChipHasY = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xE6:
                                            bChipHasR = true;
                                            bChipHasB = true;
                                            bChipHasY = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xE7:
                                            bChipHasR = true;
                                            bChipHasG = true;
                                            bChipHasY = true;
                                            bChipHasP = true;
                                            break;
                                        case 0xE8:
                                            bChipHasR = true;
                                            bChipHasG = true;
                                            bChipHasB = true;
                                            bChipHasY = true;
                                            bChipHasP = true;
                                            break;
                                        //OK
                                    }
                                    break;
                            }

                            int num17 = (bChipHasR ? 4 : 0) | (bChipHasG ? 2 : 0) | (bChipHasB ? 1 : 0) | (bChipHasY ? 16 : 0) | (bChipHasP ? 32 : 0);
                            if (pChip != null && (num17 & ~nAutoMask & 0x3F) == (flagRGB & ~nAutoMask & 0x3F) && e判定 != EJudgement.Miss)
                            {

                                if ((bChipHasR && (autoR || pushingR != 0)) || bSuccessOPEN)
                                {
                                    this.actChipFireGB.Start(R);
                                }
                                if ((bChipHasG && (autoG || pushingG != 0)) || bSuccessOPEN)
                                {
                                    this.actChipFireGB.Start(G);
                                }
                                if ((bChipHasB && (autoB || pushingB != 0)) || bSuccessOPEN)
                                {
                                    this.actChipFireGB.Start(B);
                                }
                                if ((bChipHasY && (autoY || pushingY != 0)) || bSuccessOPEN)
                                {
                                    this.actChipFireGB.Start(Y);
                                }
                                if ((bChipHasP && (autoP || pushingP != 0)) || bSuccessOPEN)
                                {
                                    this.actChipFireGB.Start(P);
                                }
                                this.tProcessChipHit(nTime, pChip);
                                this.tPlaySound(pChip, CSoundManager.rcPerformanceTimer.nシステム時刻, inst, CDTXMania.ConfigIni.n手動再生音量, CDTXMania.ConfigIni.b演奏音を強調する[indexInst], e判定 == EJudgement.Poor);
                                int chWailingChip = (inst == EInstrumentPart.GUITAR) ? 0x28 : 0xA8;
                                CDTX.CChip item = this.r指定時刻に一番近い未ヒットChip(nTime, chWailingChip, this.nInputAdjustTimeMs[indexInst], 140);
                                if (item != null)
                                {
                                    this.queWailing[indexInst].Enqueue(item);
                                }
                                continue;
                            }
                        }

                        // 以下、間違いレーンでのピック時
                        CDTX.CChip NoChipPicked = (inst == EInstrumentPart.GUITAR) ? this.r現在の空うちギターChip : this.r現在の空うちベースChip;
                        if ((NoChipPicked != null) || ((NoChipPicked = this.r指定時刻に一番近いChip_ヒット未済問わず不可視考慮(nTime, chWailingSound, this.nInputAdjustTimeMs[indexInst])) != null))
                        {
                            this.tPlaySound(NoChipPicked, CSoundManager.rcPerformanceTimer.nシステム時刻, inst, CDTXMania.ConfigIni.n手動再生音量, CDTXMania.ConfigIni.b演奏音を強調する[indexInst], true);
                        }
                        if (!CDTXMania.ConfigIni.bLight[indexInst])
                        {
                            this.tチップのヒット処理_BadならびにTight時のMiss(inst);
                        }
                    }
                }
                List<STInputEvent> list = CDTXMania.Pad.GetEvents(inst, EPad.Wail);
                if ((list != null) && (list.Count > 0))
                {
                    foreach (STInputEvent eventWailed in list)
                    {
                        if (!eventWailed.b押された)
                        {
                            continue;
                        }
                        DoWailingFromQueue(inst, eventWailed.nTimeStamp, autoW);
                    }
                }
            }
        }

        private void DoWailingFromQueue(EInstrumentPart inst, long nTimeStamp_Wailed, bool autoW)
        {
            int indexInst = (int)inst;
            long nTimeWailed = nTimeStamp_Wailed - CSoundManager.rcPerformanceTimer.n前回リセットした時のシステム時刻;
            CDTX.CChip chipWailing;
            while ((this.queWailing[indexInst].Count > 0) && ((chipWailing = this.queWailing[indexInst].Dequeue()) != null))
            {
                if ((nTimeWailed - chipWailing.nPlaybackTimeMs) <= 1000)		// #24245 2011.1.26 yyagi: 800 -> 1000
                {
                    chipWailing.bHit = true;
                    this.actWailingBonus.Start(inst, this.r現在の歓声Chip[indexInst]);
                    //if ( !bIsAutoPlay[indexInst] )
                    if (!autoW)
                    {
                        if (CDTXMania.ConfigIni.nSkillMode == 0)
                        {
                            int nCombo = (this.actCombo.nCurrentCombo[indexInst] < 500) ? this.actCombo.nCurrentCombo[indexInst] : 500;
                            this.actScore.Add(inst, bIsAutoPlay, nCombo * 3000L);		// #24245 2011.1.26 yyagi changed DRUMS->BASS, add nCombo conditions
                        }
                        else
                        {
                            int nAddScore = this.actCombo.nCurrentCombo[indexInst] > 500 ? 50000 : this.actCombo.nCurrentCombo[indexInst] * 100;
                            this.actScore.Add(inst, bIsAutoPlay, nAddScore);		// #24245 2011.1.26 yyagi changed DRUMS->BASS, add nCombo conditions

                            this.tBoostBonus();
                        }
                    }
                }
            }
        }

        private void tBoostBonus()
        {

            for (int i = 0; i < 1; i++)
            {

                if (this.ctTimer[i].b進行中)
                {
                    this.ctTimer[i].tStop();
                }

            }

            for (int i = 0; i < 1; i++)
            {
                if (!this.ctTimer[i].b進行中)
                {
                    this.bブーストボーナス = true;
                    this.ctTimer[i].tUpdate();
                    if (this.ctTimer[i].bReachedEndValue)
                    {
                        this.ctTimer[i].tStop();
                        this.bブーストボーナス = false;
                    }
                }

            }
        }

        #endregion
    }
}
