using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DTXMania
{
	public enum ECYGroup
	{
		打ち分ける,
		共通
	}
	public enum EFTGroup
	{
		打ち分ける,
		共通
	}
	public enum EHHGroup
	{
		全部打ち分ける,
		ハイハットのみ打ち分ける,
		左シンバルのみ打ち分ける,
		全部共通
	}
	public enum EBDGroup		// #27029 2012.1.4 from add
	{
		打ち分ける,
        BDとLPで打ち分ける,
		左右ペダルのみ打ち分ける,
        どっちもBD
	}
    public enum Eタイプ
    {
        A,
        B,
        C,
        D,
        E
    }
    public enum ERDPosition
    {
        RDRC,
        RCRD
    }
	public enum Eダークモード
	{
		OFF,
		HALF,
		FULL
	}
	public enum Eダメージレベル
	{
		少ない	= 0,
		普通	= 1,
		大きい	= 2
	}
	public enum Eパッド			// 演奏用のenum。ここを修正するときは、次に出てくる EKeyConfigPad と EパッドFlag もセットで修正すること。
	{
		HH		= 0,
		R		= 0,
		SD		= 1,
		G		= 1,
		BD		= 2,
		B		= 2,
		HT		= 3,
		Pick	= 3,
		LT		= 4,
		Wail	= 4,
		FT		= 5,
		Help	= 5,
		CY		= 6,
		Decide	= 6,
		HHO		= 7,
        Y       = 7,
		RD		= 8,
		LC		= 9,
        P       = 9,
		//HP		= 10,	// #27029 2012.1.4 from
		LP      = 10,
        LBD     = 11,
        MAX,			// 門番用として定義
		UNKNOWN = 99
	}
	public enum EKeyConfigPad		// #24609 キーコンフィグで使うenum。capture要素あり。
	{
		HH		= Eパッド.HH,
		R		= Eパッド.R,
		SD		= Eパッド.SD,
		G		= Eパッド.G,
		BD		= Eパッド.BD,
		B		= Eパッド.B,
		HT		= Eパッド.HT,
		Pick	= Eパッド.Pick,
		LT		= Eパッド.LT,
		Wail	= Eパッド.Wail,
		FT		= Eパッド.FT,
		Help	= Eパッド.Help,
		CY		= Eパッド.CY,
		Decide	= Eパッド.Decide,
		HHO		= Eパッド.HHO,
        Y       = Eパッド.Y,
		RD		= Eパッド.RD,
        P       = Eパッド.P,
		LC		= Eパッド.LC,
		//HP		= Eパッド.HP,		// #27029 2012.1.4 from
        LP      = Eパッド.LP,
        LBD     = Eパッド.LBD,
		Capture,
		UNKNOWN = Eパッド.UNKNOWN
	}
	[Flags]
	public enum EパッドFlag		// #24063 2011.1.16 yyagi コマンド入力用 パッド入力のフラグ化
	{
		None	= 0,
		HH		= 1,
		R		= 1,
		SD		= 2,
		G		= 2,
		B		= 4,
		BD		= 4,
		HT		= 8,
		Pick	= 8,
		LT		= 16,
		Wail	= 16,
		FT		= 32,
		Help	= 32,
		CY		= 64,
		Decide	= 128,
		HHO		= 128,
		RD		= 256,
        Y       = 256,
		LC		= 512,
        P       = 512,
        LP      = 1024,
        LBD     = 2048,
		UNKNOWN = 4096
	}
	public enum Eランダムモード
	{
		OFF,
        MIRROR,
		RANDOM,
		SUPERRANDOM,
		HYPERRANDOM,
        MASTERRANDOM,
        ANOTHERRANDOM
	}
	public enum E楽器パート		// ここを修正するときは、セットで次の EKeyConfigPart も修正すること。
	{
		DRUMS	= 0,
		GUITAR	= 1,
		BASS	= 2,
		UNKNOWN	= 99
	}
	public enum EKeyConfigPart	// : E楽器パート
	{
		DRUMS	= E楽器パート.DRUMS,
		GUITAR	= E楽器パート.GUITAR,
		BASS	= E楽器パート.BASS,
		SYSTEM,
		UNKNOWN	= E楽器パート.UNKNOWN
	}

	public enum E打ち分け時の再生の優先順位
	{
		ChipがPadより優先,
		PadがChipより優先
	}
	internal enum E入力デバイス
	{
		キーボード		= 0,
		MIDI入力		= 1,
		ジョイパッド	= 2,
		マウス			= 3,
		不明			= -1
	}
	public enum E判定
	{
		Perfect	= 0,
		Great	= 1,
		Good	= 2,
		Poor	= 3,
		Miss	= 4,
		Bad		= 5,
		Auto
	}



	internal enum E判定文字表示位置
	{
		レーン上,
		判定ライン上または横,
		表示OFF
	}
	internal enum EAVI種別
	{
		Unknown,
		AVI,
		AVIPAN
	}
	internal enum EBGA種別
	{
		Unknown,
		BMP,
		BMPTEX,
		BGA,
		BGAPAN
	}
	internal enum EFIFOモード
	{
		フェードイン,
		フェードアウト
	}
	internal enum Eドラムコンボ文字の表示位置
	{
		LEFT,
		CENTER,
		RIGHT,
		OFF
	}
	internal enum Eレーン
	{
		LC = 0,
		HH,
		SD,
		BD,
		HT,
		LT,
		FT,
		CY,
        LP,
		RD,		// 将来の独立レーン化/独立AUTO設定を見越して追加
        LBD,
		Guitar,	// AUTOレーン判定を容易にするため、便宜上定義しておく(未使用)
		Bass,	// (未使用)
		GtR,
		GtG,
		GtB,
        GtY,
        GtP,
		GtPick,
		GtW,
		BsR,
		BsG,
		BsB,
        BsY,
        BsP,
		BsPick,
		BsW,
		MAX,	// 要素数取得のための定義 ("BGM"は使わない前提で)
		BGM
	}
	internal enum Eログ出力
	{
		OFF,
		ON通常,
		ON詳細あり
	}
	internal enum E演奏画面の戻り値
	{
		継続,
		演奏中断,
		ステージ失敗,
		ステージクリア
	}
    internal enum E曲読込画面の戻り値
    {
        継続 = 0,
        読込完了,
        読込中止
    }
	/// <summary>
	/// 入力ラグ表示タイプ
	/// </summary>
	internal enum EShowLagType
	{
		OFF,			// 全く表示しない
		ON,				// 判定に依らず全て表示する
		GREAT_POOR		// GREAT-MISSの時のみ表示する(PERFECT時は表示しない)
	}
    /// <summary>
    /// 使用するAUTOゴーストデータの種類 (#35411 chnmr0)
    /// </summary>
    public enum EAutoGhostData
    {
        PERFECT = 0, // 従来のAUTO
        LAST_PLAY = 1, // (.score.ini) の LastPlay ゴースト
        HI_SKILL = 2, // (.score.ini) の HiSkill ゴースト
        HI_SCORE = 3, // (.score.ini) の HiScore ゴースト
        ONLINE = 4 // オンラインゴースト (DTXMOS からプラグインが取得、本体のみでは指定しても無効)
    }

    /// <summary>
    /// 使用するターゲットゴーストデータの種類 (#35411 chnmr0)
    /// ここでNONE以外を指定してかつ、ゴーストが利用可能な場合グラフの目標値に描画される
    /// NONE の場合従来の動作
    /// </summary>
    public enum ETargetGhostData
    {
        NONE = 0,
        PERFECT = 1,
        LAST_PLAY = 2, // (.score.ini) の LastPlay ゴースト
        HI_SKILL = 3, // (.score.ini) の HiSkill ゴースト
        HI_SCORE = 4, // (.score.ini) の HiScore ゴースト
        ONLINE = 5 // オンラインゴースト (DTXMOS からプラグインが取得、本体のみでは指定しても無効)
    }
	/// <summary>
	/// Drum/Guitar/Bass の値を扱う汎用の構造体。
	/// </summary>
	/// <typeparam name="T">値の型。</typeparam>
	[Serializable]
	[StructLayout( LayoutKind.Sequential )]
	public struct STDGBVALUE<T>			// indexはE楽器パートと一致させること
	{
		public T Drums;
		public T Guitar;
		public T Bass;
		public T Unknown;
		public T this[ int index ]
		{
			get
			{
				switch( index )
				{
					case (int) E楽器パート.DRUMS:
						return this.Drums;

					case (int) E楽器パート.GUITAR:
						return this.Guitar;

					case (int) E楽器パート.BASS:
						return this.Bass;

					case (int) E楽器パート.UNKNOWN:
						return this.Unknown;
				}
				throw new IndexOutOfRangeException();
			}
			set
			{
				switch( index )
				{
					case (int) E楽器パート.DRUMS:
						this.Drums = value;
						return;

					case (int) E楽器パート.GUITAR:
						this.Guitar = value;
						return;

					case (int) E楽器パート.BASS:
						this.Bass = value;
						return;

					case (int) E楽器パート.UNKNOWN:
						this.Unknown = value;
						return;
				}
				throw new IndexOutOfRangeException();
			}
		}
	}

	/// <summary>
	/// レーンの値を扱う汎用の構造体。列挙型"Eドラムレーン"に準拠。
	/// </summary>
	/// <typeparam name="T">値の型。</typeparam>
	[StructLayout( LayoutKind.Sequential )]
	public struct STLANEVALUE<T>
	{
		public T LC;
		public T HH;
		public T SD;
		public T BD;
		public T HT;
		public T LT;
		public T FT;
		public T CY;
        public T LP;
		public T RD;
        public T LBD;
		public T Guitar;
		public T Bass;
		public T GtR;
		public T GtG;
		public T GtB;
        public T GtY;
        public T GtP;
		public T GtPick;
		public T GtW;
		public T BsR;
		public T BsG;
		public T BsB;
        public T BsY;
        public T BsP;
		public T BsPick;
		public T BsW;
		public T BGM;

		public T this[ int index ]
		{
			get
			{
				switch ( index )
				{
					case (int) Eレーン.LC:
						return this.LC;
					case (int) Eレーン.HH:
						return this.HH;
					case (int) Eレーン.SD:
						return this.SD;
					case (int) Eレーン.BD:
						return this.BD;
					case (int) Eレーン.HT:
						return this.HT;
					case (int) Eレーン.LT:
						return this.LT;
					case (int) Eレーン.FT:
						return this.FT;
					case (int) Eレーン.CY:
						return this.CY;
                    case (int) Eレーン.LP:
                        return this.LP;
					case (int) Eレーン.RD:
						return this.RD;
                    case (int) Eレーン.LBD:
                        return this.LBD;
					case (int) Eレーン.Guitar:
						return this.Guitar;
					case (int) Eレーン.Bass:
						return this.Bass;
					case (int) Eレーン.GtR:
						return this.GtR;
					case (int) Eレーン.GtG:
						return this.GtG;
					case (int) Eレーン.GtB:
						return this.GtB;
                    case (int) Eレーン.GtY:
                        return this.GtY;
                    case (int) Eレーン.GtP:
                        return this.GtP;
					case (int) Eレーン.GtPick:
						return this.GtPick;
					case (int) Eレーン.GtW:
						return this.GtW;
					case (int) Eレーン.BsR:
						return this.BsR;
					case (int) Eレーン.BsG:
						return this.BsG;
					case (int) Eレーン.BsB:
						return this.BsB;
                    case (int) Eレーン.BsY:
                        return this.BsY;
                    case (int) Eレーン.BsP:
                        return this.BsP;
					case (int) Eレーン.BsPick:
						return this.BsPick;
					case (int) Eレーン.BsW:
						return this.BsW;
				}
				throw new IndexOutOfRangeException();
			}
			set
			{
				switch ( index )
				{
					case (int) Eレーン.LC:
						this.LC = value;
						return;
					case (int) Eレーン.HH:
						this.HH = value;
						return;
					case (int) Eレーン.SD:
						this.SD = value;
						return;
					case (int) Eレーン.BD:
						this.BD = value;
						return;
					case (int) Eレーン.HT:
						this.HT = value;
						return;
					case (int) Eレーン.LT:
						this.LT = value;
						return;
					case (int) Eレーン.FT:
						this.FT = value;
						return;
					case (int) Eレーン.CY:
						this.CY = value;
						return;
                    case (int) Eレーン.LP:
                        this.LP = value;
                        return;
					case (int) Eレーン.RD:
						this.RD = value;
						return;
                    case (int) Eレーン.LBD:
                        this.LBD = value;
                        return;
					case (int) Eレーン.Guitar:
						this.Guitar = value;
						return;
					case (int) Eレーン.Bass:
						this.Bass = value;
						return;
					case (int) Eレーン.GtR:
						this.GtR = value;
						return;
					case (int) Eレーン.GtG:
						this.GtG = value;
						return;
					case (int) Eレーン.GtB:
						this.GtB = value;
						return;
                    case (int) Eレーン.GtY:
                        this.GtY = value;
                        return;
                    case (int) Eレーン.GtP:
                        this.GtP = value;
                        return;
					case (int) Eレーン.GtPick:
						this.GtPick = value;
						return;
					case (int) Eレーン.GtW:
						this.GtW = value;
						return;
					case (int) Eレーン.BsR:
						this.BsR = value;
						return;
					case (int) Eレーン.BsG:
						this.BsG = value;
						return;
					case (int) Eレーン.BsB:
						this.BsB = value;
						return;
                    case (int) Eレーン.BsY:
                        this.BsY = value;
                        return;
                    case (int) Eレーン.BsP:
                        this.BsP = value;
                        return;
					case (int) Eレーン.BsPick:
						this.BsPick = value;
						return;
					case (int) Eレーン.BsW:
						this.BsW = value;
						return;
				}
				throw new IndexOutOfRangeException();
			}
		}
	}


	[StructLayout( LayoutKind.Sequential )]
	public struct STAUTOPLAY								// Eレーンとindexを一致させること
	{
		public bool LC;			// 0
		public bool HH;			// 1
		public bool SD;			// 2
		public bool BD;			// 3
		public bool HT;			// 4
		public bool LT;			// 5
		public bool FT;			// 6
		public bool CY;			// 7
        public bool LP;
		public bool RD;			// 8
        public bool LBD;
		public bool Guitar;		// 9	(not used)
		public bool Bass;		// 10	(not used)
		public bool GtR;		// 11
		public bool GtG;		// 12
		public bool GtB;		// 13
        public bool GtY;
        public bool GtP;
		public bool GtPick;		// 14
		public bool GtW;		// 15
		public bool BsR;		// 16
		public bool BsG;		// 17
		public bool BsB;		// 18
        public bool BsY;
        public bool BsP;
		public bool BsPick;		// 19
		public bool BsW;		// 20
		public bool this[ int index ]
		{
			get
			{
				switch ( index )
				{
					case (int) Eレーン.LC:
						return this.LC;
					case (int) Eレーン.HH:
						return this.HH;
					case (int) Eレーン.SD:
						return this.SD;
					case (int) Eレーン.BD:
						return this.BD;
					case (int) Eレーン.HT:
						return this.HT;
					case (int) Eレーン.LT:
						return this.LT;
					case (int) Eレーン.FT:
						return this.FT;
					case (int) Eレーン.CY:
						return this.CY;
                    case (int) Eレーン.LP:
                        return this.LP;
					case (int) Eレーン.RD:
						return this.RD;
                    case (int) Eレーン.LBD:
                        return this.LBD;
                    case (int)Eレーン.Guitar:
                        if (!this.GtR) return false;
                        if (!this.GtG) return false;
                        if (!this.GtB) return false;
                        if (!this.GtY) return false;
                        if (!this.GtP) return false;
                        if (!this.GtPick) return false;
                        if (!this.GtW) return false;
                        return true;
                    case (int)Eレーン.Bass:
                        if (!this.BsR) return false;
                        if (!this.BsG) return false;
                        if (!this.BsB) return false;
                        if (!this.BsY) return false;
                        if (!this.BsP) return false;
                        if (!this.BsPick) return false;
                        if (!this.BsW) return false;
                        return true;
					case (int) Eレーン.GtR:
						return this.GtR;
					case (int) Eレーン.GtG:
						return this.GtG;
					case (int) Eレーン.GtB:
						return this.GtB;
                    case (int) Eレーン.GtY:
                        return this.GtY;
                    case (int) Eレーン.GtP:
                        return this.GtP;
					case (int) Eレーン.GtPick:
						return this.GtPick;
					case (int) Eレーン.GtW:
						return this.GtW;
					case (int) Eレーン.BsR:
						return this.BsR;
					case (int) Eレーン.BsG:
						return this.BsG;
					case (int) Eレーン.BsB:
						return this.BsB;
                    case (int) Eレーン.BsY:
                        return this.BsY;
                    case (int) Eレーン.BsP:
                        return this.BsP;
					case (int) Eレーン.BsPick:
						return this.BsPick;
					case (int) Eレーン.BsW:
						return this.BsW;
				}
				throw new IndexOutOfRangeException();
			}
			set
			{
				switch ( index )
				{
					case (int) Eレーン.LC:
						this.LC = value;
						return;
					case (int) Eレーン.HH:
						this.HH = value;
						return;
					case (int) Eレーン.SD:
						this.SD = value;
						return;
					case (int) Eレーン.BD:
						this.BD = value;
						return;
					case (int) Eレーン.HT:
						this.HT = value;
						return;
					case (int) Eレーン.LT:
						this.LT = value;
						return;
					case (int) Eレーン.FT:
						this.FT = value;
						return;
					case (int) Eレーン.CY:
						this.CY = value;
						return;
                    case (int) Eレーン.LP:
                        this.LP = value;
                        return;
					case (int) Eレーン.RD:
						this.RD = value;
						return;
                    case (int) Eレーン.LBD:
                        this.LBD = value;
                        return;
                    case (int)Eレーン.Guitar:
                        this.GtR = this.GtG = this.GtB = this.GtY = this.GtP = this.GtPick = this.GtW = value;
                        //this.GtR = this.GtG = this.GtB = this.GtPick = this.GtW = value;
                        return;
                    case (int)Eレーン.Bass:
                        this.BsR = this.BsG = this.BsB = this.BsY = this.BsP = this.BsPick = this.BsW = value;
                        //this.BsR = this.BsG = this.BsB = this.BsPick = this.BsW = value;
                        return;
					case (int) Eレーン.GtR:
						this.GtR = value;
						return;
					case (int) Eレーン.GtG:
						this.GtG = value;
						return;
					case (int) Eレーン.GtB:
						this.GtB = value;
						return;
                    case (int) Eレーン.GtY:
                        this.GtY = value;
                        return;
                    case (int) Eレーン.GtP:
                        this.GtP = value;
                        return;
					case (int) Eレーン.GtPick:
						this.GtPick = value;
						return;
					case (int) Eレーン.GtW:
						this.GtW = value;
						return;
					case (int) Eレーン.BsR:
						this.BsR = value;
						return;
					case (int) Eレーン.BsG:
						this.BsG = value;
						return;
					case (int) Eレーン.BsB:
						this.BsB = value;
						return;
                    case (int) Eレーン.BsY:
                        this.BsY = value;
                        return;
                    case (int) Eレーン.BsP:
                        this.BsP = value;
                        return;
					case (int) Eレーン.BsPick:
						this.BsPick = value;
						return;
					case (int) Eレーン.BsW:
						this.BsW = value;
						return;
				}
				throw new IndexOutOfRangeException();
			}
		}
	}


	internal class C定数
	{
		public const int BGA_H = 1280;
		public const int BGA_W = 720;
		public const int HIDDEN_POS = 100;
		public const int MAX_AVI_LAYER = 1;
		public const int MAX_WAILING = 4;
		public const int PANEL_H = 0x1a;
		public const int PANEL_W = 0x116;
		public const int PREVIEW_H = 0x10d;
		public const int PREVIEW_W = 0xcc;
		public const int SCORE_H = 0x18;
		public const int SCORE_W = 12;
		public const int SUDDEN_POS = 200;

		public class Drums
		{
            public const int BAR_Y = 0x1a6;
            public const int BAR_Y_REV = 0x38;
            public const int BASS_BAR_Y = 0x5f;
            public const int BASS_BAR_Y_REV = 0x176;
            public const int BASS_H = 0x163;
            public const int BASS_W = 0x6d;
            public const int BASS_X = 0x18e;
            public const int BASS_Y = 0x39;
            public const int BGA_X = 1280;
            public const int BGA_Y = 720;
            public const int GAUGE_H = 0x160;
            public const int GAUGE_W = 0x10;
            public const int GAUGE_X = 6;
            public const int GAUGE_Y = 0x35;
            public const int GUITAR_BAR_Y = 0x5f;
            public const int GUITAR_BAR_Y_REV = 0x176;
            public const int GUITAR_H = 0x163;
            public const int GUITAR_W = 0x6d;
            public const int GUITAR_X = 0x1fb;
            public const int GUITAR_Y = 0x39;
            public const int PANEL_X = 0x150;
            public const int PANEL_Y = 0x1ab;
            public const int SCORE_X = 0x164;
            public const int SCORE_Y = 14;
		}
		public class Guitar
		{
			public const int BAR_Y = 40;
			public const int BAR_Y_REV = 0x171;
			public const int BASS_H = 0x199;
			public const int BASS_W = 140;
			public const int BASS_X = 480;
			public const int BASS_Y = 0;
			public const int BGA_X = 0xb5;
			public const int BGA_Y = 50;
			public const int GAUGE_H = 0x10;
			public const int GAUGE_W = 0x80;
			public const int GAUGE_X_BASS = 0x14f;
			public const int GAUGE_X_GUITAR = 0xb2;
			public const int GAUGE_Y_BASS = 8;
			public const int GAUGE_Y_GUITAR = 8;
			public const int GUITAR_H = 0x199;
			public const int GUITAR_W = 140;
			public const int GUITAR_X = 0x1a;
			public const int GUITAR_Y = 0;
			public const int PANEL_X = 0xb5;
			public const int PANEL_Y = 430;
		}
	}
}
