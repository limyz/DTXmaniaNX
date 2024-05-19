using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTXMania
{
	public class CChip : IComparable<CChip>, ICloneable
	{
		public bool bHit;
		public bool bVisible = true;    // b可視
		public double dbChipSizeRatio = 1.0;
		public double db実数値;
		internal EAVIType eAVI種別;
		internal EBGAType eBGA種別;
		public EInstrumentPart eInstrumentPart = EInstrumentPart.UNKNOWN;
		public EChannel nChannelNumber;
		public STDGBVALUE<int> nDistanceFromBar;
		public int nIntegerValue;       // n整数値
		public int nIntegerValue_InternalNumber; // n整数値_内部番号
		public int n総移動時間;
		public int nTransparency = 0xff;
		public int nPlaybackPosition;   // n発声位置
		public int nPlaybackTimeMs;     // n発声時刻ms
		public bool bBonusChip;
		public int nLag;                // 2011.2.1 yyagi
		public int nCurrentComboForGhost; // 2015.9.29 chnmr0 fork
		internal CDTX.CAVI rAVI;
		//internal CDTX.CDirectShow rDShow;
		internal CDTX.CAVIPAN rAVIPan;
		internal CDTX.CBGA rBGA;
		internal CDTX.CBGAPAN rBGAPan;
		internal CDTX.CBMP rBMP;
		internal CDTX.CBMPTEX rBMPTEX;

        public bool bBPMチップである
		{
			get
			{
				if (this.nChannelNumber == EChannel.BPM || this.nChannelNumber == EChannel.BPMEx)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		public bool bChipIsOpenNote
        {
            get
            {
				switch (this.nChannelNumber)
				{
					case EChannel.Guitar_Open:
					case EChannel.Bass_Open:
						return true;
				}
				return false;
			}
        }

		public bool bChipIsWailingNote {
            get
            {
                switch (this.nChannelNumber)
                {
                    case EChannel.Guitar_Wailing:
                    case EChannel.Bass_Wailing:
                        return true;
                }
                return false;
            }
        }

		public bool bChannelWithVisibleChip
        {
            get
            {
				switch (this.nChannelNumber)
				{					
					case EChannel.HiHatClose:
					case EChannel.Snare:
					case EChannel.BassDrum:
					case EChannel.HighTom:
					case EChannel.LowTom:
					case EChannel.Cymbal:
					case EChannel.FloorTom:
					case EChannel.HiHatOpen:
					case EChannel.RideCymbal:
					case EChannel.LeftCymbal:
					case EChannel.LeftPedal:
					case EChannel.LeftBassDrum:
					case EChannel.Guitar_Open:
					case EChannel.Guitar_xxBxx:
					case EChannel.Guitar_xGxxx:
					case EChannel.Guitar_xGBxx:
					case EChannel.Guitar_Rxxxx:
					case EChannel.Guitar_RxBxx:
					case EChannel.Guitar_RGxxx:
					case EChannel.Guitar_RGBxx:
					case EChannel.Guitar_xxxYx:
					case EChannel.Guitar_xxBYx:
					case EChannel.Guitar_xGxYx:
					case EChannel.Guitar_xGBYx:
					case EChannel.Guitar_RxxYx:
					case EChannel.Guitar_RxBYx:
					case EChannel.Guitar_RGxYx:
					case EChannel.Guitar_RGBYx:
					case EChannel.Guitar_xxxxP:
					case EChannel.Guitar_xxBxP:
					case EChannel.Guitar_xGxxP:
					case EChannel.Guitar_xGBxP:
					case EChannel.Guitar_RxxxP:
					case EChannel.Bass_Open:
					case EChannel.Bass_xxBxx:
					case EChannel.Bass_xGxxx:
					case EChannel.Bass_xGBxx:
					case EChannel.Bass_Rxxxx:
					case EChannel.Bass_RxBxx:
					case EChannel.Bass_RGxxx:
					case EChannel.Bass_RGBxx:
					case EChannel.Guitar_RxBxP:
					case EChannel.Guitar_RGxxP:
					case EChannel.Guitar_RGBxP:
					case EChannel.Guitar_xxxYP:
					case EChannel.Guitar_xxBYP:
					case EChannel.Guitar_xGxYP:
					case EChannel.Guitar_xGBYP:
					case EChannel.Bass_xxxYx:
					case EChannel.Bass_xxBYx:
					case EChannel.Bass_xGxYx:
					case EChannel.Bass_xGBYx:
					case EChannel.Bass_RxxYx:
					case EChannel.Bass_RxBYx:
					case EChannel.Bass_RGxYx:
					case EChannel.Bass_RGBYx:
					case EChannel.Bass_xxxxP:
					case EChannel.Bass_xxBxP:
					case EChannel.Guitar_RxxYP:
					case EChannel.Guitar_RxBYP:
					case EChannel.Guitar_RGxYP:
					case EChannel.Guitar_RGBYP:
					case EChannel.Bass_xGxxP:
					case EChannel.Bass_xGBxP:
					case EChannel.Bass_RxxxP:
					case EChannel.Bass_RxBxP:
					case EChannel.Bass_RGxxP:
					case EChannel.Bass_RGBxP:
					case EChannel.Bass_xxxYP:
					case EChannel.Bass_xxBYP:
					case EChannel.Bass_xGxYP:
					case EChannel.Bass_xGBYP:
					case EChannel.Bass_RxxYP:
					case EChannel.Bass_RxBYP:
					case EChannel.Bass_RGxYP:
					case EChannel.Bass_RGBYP:
						return true;
				}
				return false;
			}
        }

        public bool bDrums可視チップ
        {
            get
            {
                if (EChannel.HiHatClose <= nChannelNumber)
                {
                    return nChannelNumber <= EChannel.LeftBassDrum;
                }
                return false;
            }
        }

        public bool bGuitar可視チップ
        {
            get
            {
                if ((EChannel.Guitar_Open > nChannelNumber || nChannelNumber > EChannel.Guitar_RGBxx) && (EChannel.Guitar_xxxYx > nChannelNumber || nChannelNumber > EChannel.Guitar_RxxxP) && (EChannel.Guitar_RxBxP > nChannelNumber || nChannelNumber > EChannel.Guitar_xGBYP))
                {
                    if (EChannel.Guitar_RxxYP <= nChannelNumber)
                    {
                        return nChannelNumber <= EChannel.Guitar_RGBYP;
                    }
                    return false;
                }
                return true;
            }
        }

        public bool bGuitar可視チップ_Wailing含む
        {
            get
            {
                if ((EChannel.Guitar_Open > nChannelNumber || nChannelNumber > EChannel.Guitar_Wailing) && (EChannel.Guitar_xxxYx > nChannelNumber || nChannelNumber > EChannel.Guitar_RxxxP) && (EChannel.Guitar_RxBxP > nChannelNumber || nChannelNumber > EChannel.Guitar_xGBYP))
                {
                    if (EChannel.Guitar_RxxYP <= nChannelNumber)
                    {
                        return nChannelNumber <= EChannel.Guitar_RGBYP;
                    }
                    return false;
                }
                return true;
            }
        }

        public bool bBass可視チップ
        {
            get
            {
                if ((EChannel.Bass_Open > nChannelNumber || nChannelNumber > EChannel.Bass_RGBxx) && (EChannel.Bass_xxxYx > nChannelNumber || nChannelNumber > EChannel.Bass_xxBYx) && (EChannel.Bass_xGxYx > nChannelNumber || nChannelNumber > EChannel.Bass_xxBxP))
                {
                    if (EChannel.Bass_xGxxP <= nChannelNumber)
                    {
                        return nChannelNumber <= EChannel.Bass_RGBYP;
                    }
                    return false;
                }
                return true;
            }
        }

        public bool bBass可視チップ_Wailing含む
        {
            get
            {
                if ((EChannel.Bass_Open > nChannelNumber || nChannelNumber > EChannel.Bass_Wailing) && (EChannel.Bass_xxxYx > nChannelNumber || nChannelNumber > EChannel.Bass_xxBYx) && (EChannel.Bass_xGxYx > nChannelNumber || nChannelNumber > EChannel.Bass_xxBxP))
                {
                    if (EChannel.Bass_xGxxP <= nChannelNumber)
                    {
                        return nChannelNumber <= EChannel.Bass_RGBYP;
                    }
                    return false;
                }
                return true;
            }
        }

        public bool bWAVを使うチャンネルである
		{
			get
			{
				switch (this.nChannelNumber)
				{
					case EChannel.BGM:
					case EChannel.HiHatClose:
					case EChannel.Snare:
					case EChannel.BassDrum:
					case EChannel.HighTom:
					case EChannel.LowTom:
					case EChannel.Cymbal:
					case EChannel.FloorTom:
					case EChannel.HiHatOpen:
					case EChannel.RideCymbal:
					case EChannel.LeftCymbal:
					case EChannel.LeftPedal:
					case EChannel.LeftBassDrum:
					case EChannel.DrumsFillin:
					case EChannel.Guitar_Open:
					case EChannel.Guitar_xxBxx:
					case EChannel.Guitar_xGxxx:
					case EChannel.Guitar_xGBxx:
					case EChannel.Guitar_Rxxxx:
					case EChannel.Guitar_RxBxx:
					case EChannel.Guitar_RGxxx:
					case EChannel.Guitar_RGBxx:
					case EChannel.Guitar_WailingSound:
					case EChannel.HiHatClose_Hidden:
					case EChannel.Snare_Hidden:
					case EChannel.BassDrum_Hidden:
					case EChannel.HighTom_Hidden:
					case EChannel.LowTom_Hidden:
					case EChannel.Cymbal_Hidden:
					case EChannel.FloorTom_Hidden:
					case EChannel.HiHatOpen_Hidden:
					case EChannel.RideCymbal_Hidden:
					case EChannel.LeftCymbal_Hidden:
					case EChannel.SE01:
					case EChannel.SE02:
					case EChannel.SE03:
					case EChannel.SE04:
					case EChannel.SE05:
					case EChannel.SE06:
					case EChannel.SE07:
					case EChannel.SE08:
					case EChannel.SE09:
					case EChannel.SE10:
					case EChannel.SE11:
					case EChannel.SE12:
					case EChannel.SE13:
					case EChannel.SE14:
					case EChannel.SE15:
					case EChannel.SE16:
					case EChannel.SE17:
					case EChannel.SE18:
					case EChannel.SE19:
					case EChannel.SE20:
					case EChannel.SE21:
					case EChannel.SE22:
					case EChannel.SE23:
					case EChannel.SE24:
					case EChannel.SE25:
					case EChannel.SE26:
					case EChannel.SE27:
					case EChannel.SE28:
					case EChannel.SE29:
					case EChannel.SE30:
					case EChannel.SE31:
					case EChannel.SE32:

					case EChannel.Guitar_xxxYx:
					case EChannel.Guitar_xxBYx:
					case EChannel.Guitar_xGxYx:
					case EChannel.Guitar_xGBYx:
					case EChannel.Guitar_RxxYx:
					case EChannel.Guitar_RxBYx:
					case EChannel.Guitar_RGxYx:
					case EChannel.Guitar_RGBYx:
					case EChannel.Guitar_xxxxP:
					case EChannel.Guitar_xxBxP:
					case EChannel.Guitar_xGxxP:
					case EChannel.Guitar_xGBxP:
					case EChannel.Guitar_RxxxP:

					case EChannel.Bass_Open:
					case EChannel.Bass_xxBxx:
					case EChannel.Bass_xGxxx:
					case EChannel.Bass_xGBxx:
					case EChannel.Bass_Rxxxx:
					case EChannel.Bass_RxBxx:
					case EChannel.Bass_RGxxx:
					case EChannel.Bass_RGBxx:

					case EChannel.Guitar_RxBxP:
					case EChannel.Guitar_RGxxP:
					case EChannel.Guitar_RGBxP:
					case EChannel.Guitar_xxxYP:
					case EChannel.Guitar_xxBYP:
					case EChannel.Guitar_xGxYP:

					case EChannel.Guitar_xGBYP:
					case EChannel.HiHatClose_NoChip:
					case EChannel.Snare_NoChip:
					case EChannel.BassDrum_NoChip:
					case EChannel.HighTom_NoChip:
					case EChannel.LowTom_NoChip:
					case EChannel.Cymbal_NoChip:
					case EChannel.FloorTom_NoChip:
					case EChannel.HiHatOpen_NoChip:
					case EChannel.RideCymbal_NoChip:
					case EChannel.Guitar_NoChip:
					case EChannel.Bass_NoChip:
					case EChannel.LeftCymbal_NoChip:

					case EChannel.LeftPedal_NoChip:
					case EChannel.LeftBassDrum_NoChip:

					case EChannel.Bass_xxxYx:
					case EChannel.Bass_xxBYx:
					case EChannel.Bass_xGxYx:
					case EChannel.Bass_xGBYx:
					case EChannel.Bass_RxxYx:
					case EChannel.Bass_RxBYx:
					case EChannel.Bass_RGxYx:
					case EChannel.Bass_RGBYx:
					case EChannel.Bass_xxxxP:
					case EChannel.Bass_xxBxP:
					case EChannel.Guitar_RxxYP:
					case EChannel.Guitar_RxBYP:
					case EChannel.Guitar_RGxYP:
					case EChannel.Guitar_RGBYP:
					case EChannel.Bass_xGxxP:
					case EChannel.Bass_xGBxP:
					case EChannel.Bass_RxxxP:
					case EChannel.Bass_RxBxP:
					case EChannel.Bass_RGxxP:
					case EChannel.Bass_RGBxP:
					case EChannel.Bass_xxxYP:
					case EChannel.Bass_xxBYP:
					case EChannel.Bass_xGxYP:
					case EChannel.Bass_xGBYP:
					case EChannel.Bass_RxxYP:
					case EChannel.Bass_RxBYP:
					case EChannel.Bass_RGxYP:
					case EChannel.Bass_RGBYP:
						return true;
				}
				return false;
			}
		}
		public bool b自動再生音チャンネルである
		{
			get
			{
				EChannel num = this.nChannelNumber;
				if (num == EChannel.BGM ||
					(EChannel.SE01 <= num && num <= EChannel.SE09) ||
					(EChannel.SE10 <= num && num <= EChannel.SE19) ||
					(EChannel.SE20 <= num && num <= EChannel.SE29) ||
					(EChannel.SE30 <= num && num <= EChannel.SE32)
					)
				{
					return true;
				}

				return false;
			}
		}
		public bool bIsAutoPlayed;                      // 2011.6.10 yyagi
		public bool bChipKeepsPlayingAfterPerfEnds; // #32248 2013.10.14 yyagi

		//fork
		public int n楽器パートでの出現順;                // #35411 2015.08.20 chnmr0
		public bool bTargetGhost判定済み;               // #35411 2015.08.22 chnmr0

		//New property for no chip
        //public bool b空打ちチップである { get; private set; }

        //Long Notes Data members
        public CChip chipロングノート終端 { get; set; }
		public bool bロングノートである => chipロングノート終端 != null;
		public bool bロングノートHit中 { get; set; }

		//New property for empty chip
        public bool bIsEmptyChip
        {
            get
            {
                bool retResult = false;
                if (EChannel.HiHatClose_NoChip <= nChannelNumber && nChannelNumber <= EChannel.RideCymbal_NoChip)
                {
                    retResult = true;
                }
                else if (nChannelNumber == EChannel.Guitar_NoChip || nChannelNumber == EChannel.Bass_NoChip)
                {
                    retResult = true;
                }
                else if (nChannelNumber == EChannel.LeftCymbal_NoChip)
                {
                    retResult = true;
                }
                else if (nChannelNumber == EChannel.LeftPedal_NoChip)
                {
                    retResult = true;
                }
                else if (nChannelNumber == EChannel.LeftBassDrum_NoChip)
                {
                    retResult = true;
                }
                return retResult;
            }
        }

        public CChip()
		{
			this.nDistanceFromBar = new STDGBVALUE<int>()
			{
				Drums = 0,
				Guitar = 0,
				Bass = 0,
			};
			chipロングノート終端 = null;
		}
		public void t初期化()
		{
			this.nChannelNumber = 0;
			this.nIntegerValue = 0;
			this.nIntegerValue_InternalNumber = 0;
			this.db実数値 = 0.0;
			this.nPlaybackPosition = 0;
			this.nPlaybackTimeMs = 0;
			this.bBonusChip = false;
			this.nLag = -999;
			this.n楽器パートでの出現順 = -1;
			this.bTargetGhost判定済み = false;
			this.bIsAutoPlayed = false;
			this.bChipKeepsPlayingAfterPerfEnds = false;
			this.dbChipSizeRatio = 1.0;
			this.bHit = false;
			this.bVisible = true;
			this.eInstrumentPart = EInstrumentPart.UNKNOWN;
			this.nTransparency = 0xff;
			this.nDistanceFromBar.Drums = 0;
			this.nDistanceFromBar.Guitar = 0;
			this.nDistanceFromBar.Bass = 0;
			this.n総移動時間 = 0;
		}
		public override string ToString()
		{
			string[] chToStr =
			{
					"??", "バックコーラス", "小節長変更", "BPM変更", "BMPレイヤ1", "??", "??", "BMPレイヤ2",
					"BPM変更(拡張)", "??", "??", "??", "??", "??", "??", "??",
					"??", "HHClose", "Snare", "Kick", "HiTom", "LowTom", "Cymbal", "FloorTom",
					"HHOpen", "RideCymbal", "LeftCymbal", "LeftPedal", "LeftBassDrum", "", "", "ドラム歓声切替",
					"ギターOPEN", "ギター - - B", "ギター - G -", "ギター - G B", "ギター R - -", "ギター R - B", "ギター R G -", "ギター R G B",
					"ギターWailing", "??", "??", "??", "??", "??", "??", "ギターWailing音切替",
					"??", "HHClose(不可視)", "Snare(不可視)", "Kick(不可視)", "HiTom(不可視)", "LowTom(不可視)", "Cymbal(不可視)", "FloorTom(不可視)",
					"HHOpen(不可視)", "RideCymbal(不可視)", "LeftCymbal(不可視)", "LeftPedal(不可視)", "LeftBassDrum(不可視)", "??", "??", "??",
					"??", "??", "??", "??", "??", "??", "??", "??",
					"??", "??", "??", "??", "??", "??", "??", "??",
					"小節線", "拍線", "MIDIコーラス", "フィルイン", "AVI", "??", "BMPレイヤ4", "BMPレイヤ5",
					"BMPレイヤ6", "BMPレイヤ7", "AVIWIDE", "??", "??", "??", "??", "??",
					"BMPレイヤ8", "SE01", "SE02", "SE03", "SE04", "SE05", "SE06", "SE07",
					"SE08", "SE09", "??", "??", "??", "??", "??", "??",
					"SE10", "SE11", "SE12", "SE13", "SE14", "SE15", "SE16", "SE17",
					"SE18", "SE19", "??", "??", "??", "??", "??", "??",
					"SE20", "SE21", "SE22", "SE23", "SE24", "SE25", "SE26", "SE27",
					"SE28", "SE29", "??", "??", "??", "??", "??", "??",
					"SE30", "SE31", "SE32", "---Y-", "--BY-", "-G-Y-", "-GBY-", "R--Y-",
					"R-BY-", "RG-Y", "RGBY-", "----P", "--B-P", "-G--P", "-GB-P", "R---P",
					"ベースOPEN", "ベース - - B", "ベース - G -", "ベース - G B", "ベース R - -", "ベース R - B", "ベース R G -", "ベース R G B",
					"ベースWailing", "??", "??", "??", "??", "??", "??", "ベースWailing音切替",
					"??", "HHClose(空うち)", "Snare(空うち)", "Kick(空うち)", "HiTom(空うち)", "LowTom(空うち)", "Cymbal(空うち)", "FloorTom(空うち)",
					"HHOpen(空うち)", "RideCymbal(空うち)", "ギター(空打ち)", "ベース(空打ち)", "LeftCymbal(空うち)", "LeftPedal(空うち)", "LeftBassDrum(空うち)", "??",
					"??", "??", "??", "??", "BGAスコープ画像切替1", "??", "??", "BGAスコープ画像切替2",
					"??", "??", "??", "??", "??", "??", "??", "??",
					"??", "??", "??", "??", "??", "BGAスコープ画像切替3", "BGAスコープ画像切替4", "BGAスコープ画像切替5",
					"BGAスコープ画像切替6", "BGAスコープ画像切替7", "??", "??", "??", "??", "??", "??",
					"BGAスコープ画像切替8"
				};
			return string.Format("CChip: 位置:{0:D4}.{1:D3}, 時刻{2:D6}, Ch:{3:X2}({4}), Pn:{5}({11})(内部{6}), Pd:{7}, Sz:{8}, UseWav:{9}, Auto:{10}",
				this.nPlaybackPosition / 384, this.nPlaybackPosition % 384,
				this.nPlaybackTimeMs,
				this.nChannelNumber, chToStr[(int)this.nChannelNumber],
				this.nIntegerValue, this.nIntegerValue_InternalNumber,
				this.db実数値,
				this.dbChipSizeRatio,
				this.bWAVを使うチャンネルである,
				this.b自動再生音チャンネルである,
				CDTX.tZZ(this.nIntegerValue));
		}
		/// <summary>
		/// チップの再生長を取得する。現状、WAVチップとBGAチップでのみ使用可能。
		/// </summary>
		/// <returns>再生長(ms)</returns>
		public int GetDuration()
		{
			int nDuration = 0;

			if (this.bWAVを使うチャンネルである)       // WAV
			{
				CDTX.CWAV wc;
				CDTXMania.DTX.listWAV.TryGetValue(this.nIntegerValue_InternalNumber, out wc);
				if (wc == null)
				{
					nDuration = 0;
				}
				else
				{
					nDuration = (wc.rSound[0] == null) ? 0 : wc.rSound[0].nTotalPlayTimeMs;
				}
			}
			else if (this.nChannelNumber == EChannel.Movie || this.nChannelNumber == EChannel.MovieFull)    // AVI
			{
				if (this.rAVI != null && this.rAVI.avi != null)
				{
					nDuration = this.rAVI.avi.GetDuration();
					//int dwRate = (int)this.rAVI.avi.dwレート;
					//int dwScale = (int)this.rAVI.avi.dwスケール;
					//nDuration = (int)(1000.0f * dwScale / dwRate * this.rAVI.avi.GetMaxFrameCount());
				}
			}

			double _db再生速度 = /*( CDTXMania.DTXVmode.Enabled ) ? CDTXMania.DTX.dbDTXVPlaySpeed :*/ CDTXMania.DTX.db再生速度;
			return (int)(nDuration / _db再生速度);
		}

		

        public void ComputeDistanceFromBar(long nCurrentTime, STDGBVALUE<double> dbPerformanceScrollSpeed) 
		{
			const double speed = 286;   // BPM150の時の1小節の長さ[dot]
										//XGのHS4.5が1289。思えばBPMじゃなくて拍の長さが関係あるよね。
			double ScrollSpeedDrums = (dbPerformanceScrollSpeed.Drums + 1.0) * 0.5 * 37.5 * speed / 60000.0;
			double ScrollSpeedGuitar = (dbPerformanceScrollSpeed.Guitar + 1.0) * 0.5 * 0.5 * 37.5 * speed / 60000.0;
			double ScrollSpeedBass = (dbPerformanceScrollSpeed.Bass + 1.0) * 0.5 * 0.5 * 37.5 * speed / 60000.0;

			this.nDistanceFromBar.Drums = (int)((this.nPlaybackTimeMs - nCurrentTime) * ScrollSpeedDrums);
			this.nDistanceFromBar.Guitar = (int)((this.nPlaybackTimeMs - nCurrentTime) * ScrollSpeedGuitar);
			this.nDistanceFromBar.Bass = (int)((this.nPlaybackTimeMs - nCurrentTime) * ScrollSpeedBass);

			//New: Compute Distance for End of Long Note chip
			if(this.bロングノートである)
            {
				this.chipロングノート終端.ComputeDistanceFromBar(nCurrentTime, dbPerformanceScrollSpeed);
            }
		}

		#region [ IComparable 実装 ]
		//-----------------
		public int CompareTo(CChip other)
		{
			//チップの重なり順を決める。16進数で16個ずつ並んでいます。
			byte[] n優先度 = new byte[] {
					5, 5, 3, 3, 5, 5, 5, 5, 3, 5, 5, 5, 5, 5, 5, 5, //0x00 ～ 0x0F
					5, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 5, 5, 5, //0x10 ～ 0x1F　ドラム演奏
					7, 7, 7, 7, 7, 7, 7, 7, 5, 5, 5, 5, 5, 5, 5, 5, //0x20 ～ 0x2F　ギター演奏
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0x30 ～ 0x3F　ドラム不可視
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 8, //0x40 ～ 0x4F　未使用(0x4Fはボーナスチップ)
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0x50 ～ 0x5F　小節線、拍線、フィル、AVIなど
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0x60 ～ 0x6F　BGA、SE
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0x70 ～ 0x7F　SE
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0x80 ～ 0x8F　SE
					5, 5, 5, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, //0x90 ～ 0x9F　SE、Guitar5レーン
					7, 7, 7, 7, 7, 7, 7, 7, 5, 7, 7, 7, 7, 7, 7, 7, //0xA0 ～ 0xAF　
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0xB0 ～ 0xBF　
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0xC0 ～ 0xCF　
					7, 7, 7, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0xD0 ～ 0xDF　
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0xE0 ～ 0xEF　
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0xF0 ～ 0xFF　
				};


			// まずは位置で比較。

			if (this.nPlaybackPosition < other.nPlaybackPosition)
				return -1;

			if (this.nPlaybackPosition > other.nPlaybackPosition)
				return 1;


			// 位置が同じなら優先度で比較。

			if (n優先度[(int)this.nChannelNumber] < n優先度[(int)other.nChannelNumber])
				return -1;

			if (n優先度[(int)this.nChannelNumber] > n優先度[(int)other.nChannelNumber])
				return 1;


			// 位置も優先度も同じなら同じと返す。

			return 0;
		}
		//-----------------
		#endregion
		/// <summary>
		/// shallow copyです。
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			return MemberwiseClone();
		}

    }
}
