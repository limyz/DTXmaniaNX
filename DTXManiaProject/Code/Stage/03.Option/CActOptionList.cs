using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using SlimDX;
using FDK;

namespace DTXMania
{
	internal class CActOptionList : CActivity
	{
		// プロパティ
		public bool b現在選択されている項目はReturnToMenuである
		{
			get
			{
				CItemBase currentItem = this.list項目リスト[ this.n現在の選択項目 ];
				if (currentItem == this.iCommonReturnToMenu || currentItem == this.iDrumsReturnToMenu ||
					currentItem == this.iGuitarReturnToMenu || currentItem == this.iBassReturnToMenu)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		public CItemBase ib現在の選択項目
		{
			get
			{
				return this.list項目リスト[ this.n現在の選択項目 ];
			}
		}
        public int n現在の選択項目;

		// メソッド

		public void t項目リストの設定・Common()
		{
			this.tConfigIniへ記録する();
			this.list項目リスト.Clear();

            this.iCommonDark = new CItemList("Dark", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eDark,
            "HALF: レーンが表示されな\nくなります。\nFULL: さらに小節線、拍線、判定ラ\nインも表示されなくなります。",
            "OFF: all display parts are shown.\nHALF: lanes and gauge are\n disappeared.\nFULL: additionaly to HALF, bar/beat\n lines, hit bar are disappeared.",
            new string[] { "OFF", "HALF", "FULL" });
            this.list項目リスト.Add(this.iCommonDark);

			this.n現在の選択項目 = 0;
			this.eメニュー種別 = Eメニュー種別.Common;
		}
		public void t項目リストの設定・Drums()
		{
			this.tConfigIniへ記録する();
			this.list項目リスト.Clear();
			this.iDrumsReturnToMenu = new CItemBase( "<< Return To Menu", CItemBase.Eパネル種別.その他,
				"左側のメニューに戻ります。",
				"Return to left menu.");
			this.list項目リスト.Add(this.iDrumsReturnToMenu);
			this.iDrumsAutoPlayAll = new CItemThreeState( "AutoPlay (All)", CItemThreeState.E状態.不定,
				"全パッドの自動演奏の ON/OFF を\nまとめて切り替えます。",
				"You can change whether Auto or not\n for all drums lanes at once.");
			this.list項目リスト.Add( this.iDrumsAutoPlayAll );
			this.iDrumsLeftCymbal = new CItemToggle( "    LeftCymbal", CDTXMania.ConfigIni.bAutoPlay.LC,
				"左シンバルを自動で演奏します。",
				"To play LeftCymbal automatically.");
			this.list項目リスト.Add( this.iDrumsLeftCymbal );
			this.iDrumsHiHat = new CItemToggle( "    HiHat", CDTXMania.ConfigIni.bAutoPlay.HH,
				"ハイハットを自動で演奏します。\n（クローズ、オープンとも）",
				"To play HiHat automatically.\n(It effects to both HH-close and\n HH-open)");
			this.list項目リスト.Add( this.iDrumsHiHat );
			this.iDrumsSnare = new CItemToggle( "    Snare", CDTXMania.ConfigIni.bAutoPlay.SD,
				"スネアを自動で演奏します。",
				"To play Snare automatically.");
			this.list項目リスト.Add( this.iDrumsSnare );
			this.iDrumsBass = new CItemToggle( "    BassDrum", CDTXMania.ConfigIni.bAutoPlay.BD,
				"バスドラムを自動で演奏します。",
				"To play Bass Drum automatically.");
			this.list項目リスト.Add( this.iDrumsBass );
			this.iDrumsHighTom = new CItemToggle( "    HighTom", CDTXMania.ConfigIni.bAutoPlay.HT,
				"ハイタムを自動で演奏します。",
				"To play High Tom automatically.");
			this.list項目リスト.Add( this.iDrumsHighTom );
			this.iDrumsLowTom = new CItemToggle( "    LowTom", CDTXMania.ConfigIni.bAutoPlay.LT,
				"ロータムを自動で演奏します。",
				"To play Low Tom automatically.");
			this.list項目リスト.Add( this.iDrumsLowTom );
			this.iDrumsFloorTom = new CItemToggle( "    FloorTom", CDTXMania.ConfigIni.bAutoPlay.FT,
				"フロアタムを自動で演奏します。",
				"To play Floor Tom automatically.");
			this.list項目リスト.Add( this.iDrumsFloorTom );
			this.iDrumsCymbalRide = new CItemToggle( "    Cym/Ride", CDTXMania.ConfigIni.bAutoPlay.CY,
				"右シンバルとライドシンバルを自動で\n演奏します。",
				"To play both right- and Ride-Cymbal\n automatically.");
			this.list項目リスト.Add( this.iDrumsCymbalRide );
			this.iDrumsScrollSpeed = new CItemInteger( "ScrollSpeed", 0, 0x7cf, CDTXMania.ConfigIni.n譜面スクロール速度.Drums,
				"演奏時のドラム譜面のスクロールの\n速度を指定します。\nx0.5 ～ x1000.0 を指定可能です。",
				"To change the scroll speed for the\ndrums lanes.\nYou can set it from x0.5 to x1000.0.\n(ScrollSpeed=x0.5 means half speed)");
			this.list項目リスト.Add( this.iDrumsScrollSpeed );
			this.iDrumsComboPosition = new CItemList( "ComboPosition", CItemBase.Eパネル種別.通常, (int) CDTXMania.ConfigIni.ドラムコンボ文字の表示位置,
				"演奏時のドラムコンボ文字列の位置\nを指定します。",
				"The display position for Drums Combo.\nNote that it doesn't take effect\n at Autoplay ([Left] is forcely used).",
				new string[] { "Left", "Center", "Right", "OFF" } );
			this.list項目リスト.Add( this.iDrumsComboPosition );
			this.iDrumsSudden = new CItemToggle("Sudden", CDTXMania.ConfigIni.bSudden.Drums,
				"ドラムチップが譜面の下の方から表\n示されるようになります。",
				"Drums chips are disappered until they\ncome near the hit bar, and suddenly\nappears.");
			this.list項目リスト.Add( this.iDrumsSudden );
			this.iDrumsHidden = new CItemToggle( "Hidden", CDTXMania.ConfigIni.bHidden.Drums,
				"ドラムチップが譜面の下の方で表示\nされなくなります。",
				"Drums chips are hidden by approaching\nto the hit bar. ");
			this.list項目リスト.Add( this.iDrumsHidden );
			this.iDrumsReverse = new CItemToggle( "Reverse", CDTXMania.ConfigIni.bReverse.Drums,
				"ドラムチップが譜面の下から上に流\nれるようになります。",
				"The scroll way is reversed. Drums chips\nflow from the bottom to the top.");
			this.list項目リスト.Add( this.iDrumsReverse );
			this.iDrumsPosition = new CItemList( "Position", CItemBase.Eパネル種別.通常, (int) CDTXMania.ConfigIni.判定文字表示位置.Drums,
				"ドラムの判定文字の表示位置を指定\nします。\n  P-A: レーン上\n  P-B: 判定ライン下\n  OFF: 表示しない",
				"The position to show judgement mark.\n(Perfect, Great, ...)\n\n P-A: on the lanes.\n P-B: under the hit bar.\n OFF: no judgement mark.",
				new string[] { "P-A", "P-B", "OFF" } );
			this.list項目リスト.Add( this.iDrumsPosition );
			this.iDrumsTight = new CItemToggle( "Tight", CDTXMania.ConfigIni.bTight,
				"ドラムチップのないところでパッドを\n叩くとミスになります。",
				"It becomes MISS to hit pad without\n chip.");
			this.list項目リスト.Add( this.iDrumsTight );
																					// #23580 2011.1.3 yyagi
			this.iDrumsInputAdjustTimeMs = new CItemInteger("InputAdjust", -99, 0, CDTXMania.ConfigIni.nInputAdjustTimeMs.Drums,
				"ドラムの入力タイミングの微調整を\n行います。\n-99 ～ 0ms まで指定可能です。\n入力ラグを軽減するためには、負の\n値を指定してください。",
				"To adjust the drums input timing.\nYou can set from -99 to 0ms.\nTo decrease input lag, set minus value.");
			this.list項目リスト.Add(this.iDrumsInputAdjustTimeMs);
            // #24074 2011.01.23 add ikanick
			this.iDrumsGraph = new CItemToggle( "Graph", CDTXMania.ConfigIni.bGraph.Drums,
				"最高スキルと比較できるグラフを\n表示します。\nオートプレイだと表示されません。",
				"To draw Graph \n or not.");
			this.list項目リスト.Add( this.iDrumsGraph );

			this.n現在の選択項目 = 0;
			this.eメニュー種別 = Eメニュー種別.Drums;
		}
		public void t項目リストの設定・Exit()
		{
			this.tConfigIniへ記録する();
			this.eメニュー種別 = Eメニュー種別.Unknown;
		}
		public void t項目リストの設定・Guitar()
		{
			this.tConfigIniへ記録する();
			this.list項目リスト.Clear();
			this.iGuitarReturnToMenu = new CItemBase( "<< Return To Menu", CItemBase.Eパネル種別.その他,
				"左側のメニューに戻ります。",
				"Return to left menu.");
			this.list項目リスト.Add(this.iGuitarReturnToMenu);
			this.iGuitarAutoPlay = new CItemToggle( "AutoPlay", CDTXMania.ConfigIni.bAutoPlay.Guitar,
				"ギターパートを自動で演奏します。",
				"To play the guitar part automatically.");
			this.list項目リスト.Add( this.iGuitarAutoPlay );
			this.iGuitarScrollSpeed = new CItemInteger( "ScrollSpeed", 0, 0x7cf, CDTXMania.ConfigIni.n譜面スクロール速度.Guitar,
				"演奏時のギター譜面のスクロールの\n速度を指定します。\nx0.5 ～ x1000.0 までを指定可能です。",
				"To change the scroll speed for the\nguitar lanes.\nYou can set it from x0.5 to x1000.0.\n(ScrollSpeed=x0.5 means half speed)");
			this.list項目リスト.Add(this.iGuitarScrollSpeed);
			this.iGuitarSudden = new CItemToggle( "Sudden", CDTXMania.ConfigIni.bSudden.Guitar,
				"ギターチップがヒットバー付近にくる\nまで表示されなくなります。",
				"Guitar chips are disappered until they\ncome near the hit bar, and suddenly\nappears.");
			this.list項目リスト.Add( this.iGuitarSudden );
			this.iGuitarHidden = new CItemToggle( "Hidden", CDTXMania.ConfigIni.bHidden.Guitar,
				"ギターチップがヒットバー付近で表示\nされなくなります。",
				"Guitar chips are hidden by approaching\nto the hit bar. ");
			this.list項目リスト.Add( this.iGuitarHidden );
			this.iGuitarReverse = new CItemToggle( "Reverse", CDTXMania.ConfigIni.bReverse.Guitar,
				"ギターチップが譜面の上から下に流\nれるようになります。",
				"The scroll way is reversed. Guitar chips\nflow from the top to the bottom.");
			this.list項目リスト.Add( this.iGuitarReverse );
			this.iGuitarPosition = new CItemList( "Position", CItemBase.Eパネル種別.通常, (int) CDTXMania.ConfigIni.判定文字表示位置.Guitar,
				"ギターの判定文字の表示位置を指定\nします。\n  P-A: レーン上\n  P-B: COMBO の下\n  OFF: 表示しない",
				"The position to show judgement mark.\n(Perfect, Great, ...)\n\n P-A: on the lanes.\n P-B: under the COMBO indication.\n OFF: no judgement mark.",
				new string[] { "P-A", "P-B", "OFF" } );
			this.list項目リスト.Add( this.iGuitarPosition );
			this.iGuitarRandom = new CItemList( "Random", CItemBase.Eパネル種別.通常, (int) CDTXMania.ConfigIni.eRandom.Guitar,
				"ギターのチップがランダムに降ってき\nます。\n  Part: 小節・レーン単位で交換\n  Super: チップ単位で交換\n  Hyper: 全部完全に変更",
				"Guitar chips come randomly.\n\n Part: swapping lanes randomly for each\n  measures.\n Super: swapping chip randomly\n Hyper: swapping randomly\n  (number of lanes also changes)",
				new string[] { "OFF", "Part", "Super", "Hyper" } );
			this.list項目リスト.Add( this.iGuitarRandom );
			this.iGuitarLight = new CItemToggle( "Light", CDTXMania.ConfigIni.bLight.Guitar,
				"ギターチップのないところでピッキン\nグしても BAD になりません。",
				"Even if you pick without any chips,\nit doesn't become BAD.");
			this.list項目リスト.Add( this.iGuitarLight );
			this.iGuitarLeft = new CItemToggle( "Left", CDTXMania.ConfigIni.bLeft.Guitar,
				"ギターの RGB の並びが左右反転し\nます。（左利きモード）",
				"Lane order 'R-G-B' becomes 'B-G-R'\nfor lefty.");
			this.list項目リスト.Add( this.iGuitarLeft );
																					// #23580 2011.1.3 yyagi
			this.iGuitarInputAdjustTimeMs= new CItemInteger("InputAdjust", -99, 0, CDTXMania.ConfigIni.nInputAdjustTimeMs.Guitar,
				"ギターの入力タイミングの微調整を\n行います。\n-99 ～ 0ms まで指定可能です。\n入力ラグを軽減するためには、負の\n値を指定してください。",
				"To adjust the guitar input timing.\nYou can set from -99 to 0ms.\nTo decrease input lag, set minus value." );
			this.list項目リスト.Add( this.iGuitarInputAdjustTimeMs );

			this.n現在の選択項目 = 0;
			this.eメニュー種別 = Eメニュー種別.Guitar;
		}
		public void t項目リストの設定・Bass()
		{
			this.tConfigIniへ記録する();
			this.list項目リスト.Clear();

			this.iBassReturnToMenu = new CItemBase( "<< Return To Menu", CItemBase.Eパネル種別.その他,
				"左側のメニューに戻ります。",
				"Return to left menu." );
			this.list項目リスト.Add( this.iBassReturnToMenu );
			this.iBassAutoPlay = new CItemToggle( "AutoPlay", CDTXMania.ConfigIni.bAutoPlay.Bass,
				"ベースパートを自動で演奏します。",
				"To play the bass part automatically." );
			this.list項目リスト.Add( this.iBassAutoPlay );
			this.iBassScrollSpeed = new CItemInteger( "ScrollSpeed", 0, 0x7cf, CDTXMania.ConfigIni.n譜面スクロール速度.Bass,
				"演奏時のベース譜面のスクロールの\n速度を指定します。\nx0.5 ～ x1000.0 までを指定可能です。",
				"To change the scroll speed for the\nbass lanes.\nYou can set it from x0.5 to x1000.0.\n(ScrollSpeed=x0.5 means half speed)" );
			this.list項目リスト.Add( this.iBassScrollSpeed );
			this.iBassSudden = new CItemToggle( "Sudden", CDTXMania.ConfigIni.bSudden.Bass,
				"ベースチップがヒットバー付近にくる\nまで表示されなくなります。",
				"Bass chips are disappered until they\ncome near the hit bar, and suddenly\nappears." );
			this.list項目リスト.Add( this.iBassSudden );
			this.iBassHidden = new CItemToggle( "Hidden", CDTXMania.ConfigIni.bHidden.Bass,
				"ベースチップがヒットバー付近で表示\nされなくなります。",
				"Bass chips are hidden by approaching\nto the hit bar." );
			this.list項目リスト.Add( this.iBassHidden );
			this.iBassReverse = new CItemToggle( "Reverse", CDTXMania.ConfigIni.bReverse.Bass,
				"ベースチップが譜面の上から下に流\nれるようになります。",
				"The scroll way is reversed. Bass chips\nflow from the top to the bottom." );
			this.list項目リスト.Add( this.iBassReverse );
			this.iBassPosition = new CItemList( "Position", CItemBase.Eパネル種別.通常,
				(int) CDTXMania.ConfigIni.判定文字表示位置.Bass,
				"ベースの判定文字の表示位置を指定\nします。\n  P-A: レーン上\n  P-B: COMBO の下\n  OFF: 表示しない",
				"The position to show judgement mark.\n(Perfect, Great, ...)\n\n P-A: on the lanes.\n P-B: under the COMBO indication.\n OFF: no judgement mark.",
				new string[] { "P-A", "P-B", "OFF" } );
			this.list項目リスト.Add( this.iBassPosition );
			this.iBassRandom = new CItemList( "Random", CItemBase.Eパネル種別.通常,
				(int) CDTXMania.ConfigIni.eRandom.Bass,
				"ベースのチップがランダムに降ってき\nます。\n  Part: 小節・レーン単位で交換\n  Super: チップ単位で交換\n  Hyper: 全部完全に変更",
				"Bass chips come randomly.\n\n Part: swapping lanes randomly for each\n  measures.\n Super: swapping chip randomly\n Hyper: swapping randomly\n  (number of lanes also changes)",
				new string[] { "OFF", "Part", "Super", "Hyper" } );
			this.list項目リスト.Add( this.iBassRandom );
			this.iBassLight = new CItemToggle( "Light", CDTXMania.ConfigIni.bLight.Bass,
				"ベースチップのないところでピッキン\nグしても BAD になりません。",
				"Even if you pick without any chips,\nit doesn't become BAD." );
			this.list項目リスト.Add( this.iBassLight );
			this.iBassLeft = new CItemToggle( "Left", CDTXMania.ConfigIni.bLeft.Bass,
				"ベースの RGB の並びが左右反転し\nます。（左利きモード）",
				"Lane order 'R-G-B' becomes 'B-G-R'\nfor lefty." );
			this.list項目リスト.Add( this.iBassLeft );
			// #23580 2011.1.3 yyagi
			this.iBassInputAdjustTimeMs = new CItemInteger( "InputAdjust", -99, 0, CDTXMania.ConfigIni.nInputAdjustTimeMs.Bass,
				"ベースの入力タイミングの微調整を\n行います。\n-99 ～ 0ms まで指定可能です。入力ラグを軽減するためには、負の\n値を指定してください。",
				"To adjust the bass input timing.\nYou can set from -99 to 0ms.\nTo decrease input lag, set minus value." );
			this.list項目リスト.Add( this.iBassInputAdjustTimeMs );

			this.n現在の選択項目 = 0;
			this.eメニュー種別 = Eメニュー種別.Bass;
		}

        public void tEnter押下()
        {
            CDTXMania.Skin.sound決定音.t再生する();
            if (this.b要素値にフォーカス中)
            {
                this.b要素値にフォーカス中 = false;
            }
            else if (this.list項目リスト[this.n現在の選択項目].e種別 == CItemBase.E種別.整数)
            {
                this.b要素値にフォーカス中 = true;
            }
            else if (this.b現在選択されている項目はReturnToMenuである)
            {
                //this.tConfigIniへ記録する();
                //CONFIG中にスキン変化が発生すると面倒なので、一旦マスクした。
            }
        }
        public void t次に移動()
        {
            CDTXMania.Skin.soundカーソル移動音.t再生する();
            if (this.b要素値にフォーカス中)
            {
                this.list項目リスト[this.n現在の選択項目].t項目値を前へ移動();
            }
            else
            {
                this.n目標のスクロールカウンタ += 100;
            }
        }
        public void t前に移動()
        {
            CDTXMania.Skin.soundカーソル移動音.t再生する();
            if (this.b要素値にフォーカス中)
            {
                this.list項目リスト[this.n現在の選択項目].t項目値を次へ移動();
            }
            else
            {
                this.n目標のスクロールカウンタ -= 100;
            }
        }

		
		// CActivity 実装

		public override void On活性化()
		{
			this.list項目リスト = new List<CItemBase>();
			this.eメニュー種別 = Eメニュー種別.Unknown;
			this.t項目リストの設定・Common();
			this.b要素値にフォーカス中 = false;
			this.n目標のスクロールカウンタ = 0;
			this.n現在のスクロールカウンタ = 0;
			this.nスクロール用タイマ値 = -1;
			this.ct三角矢印アニメ = new CCounter();
			base.On活性化();
		}
		public override void On非活性化()
		{
			this.tConfigIniへ記録する();
			this.list項目リスト.Clear();
			this.ct三角矢印アニメ = null;
			base.On非活性化();
		}
        public override void OnManagedリソースの作成()
        {
            if (this.b活性化してない)
                return;

            this.tx通常項目行パネル = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\4_itembox.png"), false);
            this.txその他項目行パネル = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\4_itembox other.png"), false);
            this.tx三角矢印 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\4_triangle arrow.png"), false);
            base.OnManagedリソースの作成();
        }
        public override void OnManagedリソースの解放()
        {
            if (this.b活性化してない)
                return;

            CDTXMania.tテクスチャの解放(ref this.tx通常項目行パネル);
            CDTXMania.tテクスチャの解放(ref this.txその他項目行パネル);
            CDTXMania.tテクスチャの解放(ref this.tx三角矢印);

            base.OnManagedリソースの解放();
        }
		public override int On進行描画()
		{
			throw new InvalidOperationException( "t進行描画(bool)のほうを使用してください。" );
		}
        public int t進行描画(bool b項目リスト側にフォーカスがある)
        {
            if (this.b活性化してない)
                return 0;

            // 進行

            #region [ 初めての進行描画 ]
            //-----------------
            if (base.b初めての進行描画)
            {
                this.nスクロール用タイマ値 = CSound管理.rc演奏用タイマ.n現在時刻;
                this.ct三角矢印アニメ.t開始(0, 9, 50, CDTXMania.Timer);

                base.b初めての進行描画 = false;
            }
            //-----------------
            #endregion

            this.b項目リスト側にフォーカスがある = b項目リスト側にフォーカスがある;		// 記憶

            #region [ 項目スクロールの進行 ]
            //-----------------
            long n現在時刻 = CDTXMania.Timer.n現在時刻;
            if (n現在時刻 < this.nスクロール用タイマ値) this.nスクロール用タイマ値 = n現在時刻;

            const int INTERVAL = 2;	// [ms]
            while ((n現在時刻 - this.nスクロール用タイマ値) >= INTERVAL)
            {
                int n目標項目までのスクロール量 = Math.Abs((int)(this.n目標のスクロールカウンタ - this.n現在のスクロールカウンタ));
                int n加速度 = 0;

                #region [ n加速度の決定；目標まで遠いほど加速する。]
                //-----------------
                if (n目標項目までのスクロール量 <= 100)
                {
                    n加速度 = 2;
                }
                else if (n目標項目までのスクロール量 <= 300)
                {
                    n加速度 = 3;
                }
                else if (n目標項目までのスクロール量 <= 500)
                {
                    n加速度 = 4;
                }
                else
                {
                    n加速度 = 8;
                }
                //-----------------
                #endregion
                #region [ this.n現在のスクロールカウンタに n加速度 を加減算。]
                //-----------------
                if (this.n現在のスクロールカウンタ < this.n目標のスクロールカウンタ)
                {
                    this.n現在のスクロールカウンタ += n加速度;
                    if (this.n現在のスクロールカウンタ > this.n目標のスクロールカウンタ)
                    {
                        // 目標を超えたら目標値で停止。
                        this.n現在のスクロールカウンタ = this.n目標のスクロールカウンタ;
                    }
                }
                else if (this.n現在のスクロールカウンタ > this.n目標のスクロールカウンタ)
                {
                    this.n現在のスクロールカウンタ -= n加速度;
                    if (this.n現在のスクロールカウンタ < this.n目標のスクロールカウンタ)
                    {
                        // 目標を超えたら目標値で停止。
                        this.n現在のスクロールカウンタ = this.n目標のスクロールカウンタ;
                    }
                }
                //-----------------
                #endregion
                #region [ 行超え処理、ならびに目標位置に到達したらスクロールを停止して項目変更通知を発行。]
                //-----------------
                if (this.n現在のスクロールカウンタ >= 100)
                {
                    this.n現在の選択項目 = this.t次の項目(this.n現在の選択項目);
                    this.n現在のスクロールカウンタ -= 100;
                    this.n目標のスクロールカウンタ -= 100;
                    if (this.n目標のスクロールカウンタ == 0)
                    {
                        CDTXMania.stageコンフィグ.t項目変更通知();
                    }
                }
                else if (this.n現在のスクロールカウンタ <= -100)
                {
                    this.n現在の選択項目 = this.t前の項目(this.n現在の選択項目);
                    this.n現在のスクロールカウンタ += 100;
                    this.n目標のスクロールカウンタ += 100;
                    if (this.n目標のスクロールカウンタ == 0)
                    {
                        CDTXMania.stageコンフィグ.t項目変更通知();
                    }
                }
                //-----------------
                #endregion

                this.nスクロール用タイマ値 += INTERVAL;
            }
            //-----------------
            #endregion

            #region [ ▲印アニメの進行 ]
            //-----------------
            if (this.b項目リスト側にフォーカスがある && (this.n目標のスクロールカウンタ == 0))
                this.ct三角矢印アニメ.t進行Loop();
            //-----------------
            #endregion


            // 描画

            this.ptパネルの基本座標[4].X = this.b項目リスト側にフォーカスがある ? 0x228 : 0x25a;		// メニューにフォーカスがあるなら、項目リストの中央は頭を出さない。

            #region [ 計11個の項目パネルを描画する。]
            //-----------------
            int nItem = this.n現在の選択項目;
            for (int i = 0; i < 4; i++)
                nItem = this.t前の項目(nItem);

            for (int n行番号 = -4; n行番号 < 6; n行番号++)		// n行番号 == 0 がフォーカスされている項目パネル。
            {
                #region [ 今まさに画面外に飛びだそうとしている項目パネルは描画しない。]
                //-----------------
                if (((n行番号 == -4) && (this.n現在のスクロールカウンタ > 0)) ||		// 上に飛び出そうとしている
                    ((n行番号 == +5) && (this.n現在のスクロールカウンタ < 0)))		// 下に飛び出そうとしている
                {
                    nItem = this.t次の項目(nItem);
                    continue;
                }
                //-----------------
                #endregion

                int n移動元の行の基本位置 = n行番号 + 4;
                int n移動先の行の基本位置 = (this.n現在のスクロールカウンタ <= 0) ? ((n移動元の行の基本位置 + 1) % 10) : (((n移動元の行の基本位置 - 1) + 10) % 10);
                int x = this.ptパネルの基本座標[n移動元の行の基本位置].X + ((int)((this.ptパネルの基本座標[n移動先の行の基本位置].X - this.ptパネルの基本座標[n移動元の行の基本位置].X) * (((double)Math.Abs(this.n現在のスクロールカウンタ)) / 100.0)));
                int y = this.ptパネルの基本座標[n移動元の行の基本位置].Y + ((int)((this.ptパネルの基本座標[n移動先の行の基本位置].Y - this.ptパネルの基本座標[n移動元の行の基本位置].Y) * (((double)Math.Abs(this.n現在のスクロールカウンタ)) / 100.0)));

                #region [ 現在の行の項目パネル枠を描画。]
                //-----------------
                switch (this.list項目リスト[nItem].eパネル種別)
                {
                    case CItemBase.Eパネル種別.通常:
                        if (this.tx通常項目行パネル != null)
                            this.tx通常項目行パネル.t2D描画(CDTXMania.app.Device, x, y);
                        break;

                    case CItemBase.Eパネル種別.その他:
                        if (this.txその他項目行パネル != null)
                            this.txその他項目行パネル.t2D描画(CDTXMania.app.Device, x, y);
                        break;
                }
                //-----------------
                #endregion
                #region [ 現在の行の項目名を描画。]
                //-----------------
                CDTXMania.stageオプション.actFont.t文字列描画(x + 0x38, y + 19, this.list項目リスト[nItem].str項目名);
                //-----------------
                #endregion
                #region [ 現在の行の項目の要素を描画。]
                //-----------------
                switch (this.list項目リスト[nItem].e種別)
                {
                    case CItemBase.E種別.ONorOFFトグル:
                        #region [ *** ]
                        //-----------------
                        CDTXMania.stageオプション.actFont.t文字列描画(x + 420, y + 19, ((CItemToggle)this.list項目リスト[nItem]).bON ? "ON" : "OFF");
                        break;
                    //-----------------
                        #endregion

                    case CItemBase.E種別.ONorOFFor不定スリーステート:
                        #region [ *** ]
                        //-----------------
                        switch (((CItemThreeState)this.list項目リスト[nItem]).e現在の状態)
                        {
                            case CItemThreeState.E状態.ON:
                                CDTXMania.stageオプション.actFont.t文字列描画(x + 420, y + 19, "ON");
                                break;

                            case CItemThreeState.E状態.不定:
                                CDTXMania.stageオプション.actFont.t文字列描画(x + 420, y + 19, "- -");
                                break;

                            default:
                                CDTXMania.stageオプション.actFont.t文字列描画(x + 420, y + 19, "OFF");
                                break;
                        }
                        break;
                    //-----------------
                        #endregion

                    case CItemBase.E種別.整数:		// #24789 2011.4.8 yyagi: add PlaySpeed supports (copied them from OPTION)
                        #region [ *** ]
                        //-----------------
                        if (this.list項目リスト[nItem] == this.iCommonPlaySpeed)
                        {
                            double d = ((double)((CItemInteger)this.list項目リスト[nItem]).n現在の値) / 20.0;
                            CDTXMania.stageオプション.actFont.t文字列描画(x + 420, y + 19, d.ToString("0.000"), (n行番号 == 0) && this.b要素値にフォーカス中);
                        }
                        else if (this.list項目リスト[nItem] == this.iDrumsScrollSpeed || this.list項目リスト[nItem] == this.iGuitarScrollSpeed || this.list項目リスト[nItem] == this.iBassScrollSpeed)
                        {
                            float f = (((CItemInteger)this.list項目リスト[nItem]).n現在の値 + 1) * 0.5f;
                            CDTXMania.stageオプション.actFont.t文字列描画(x + 420, y + 19, f.ToString("x0.0"), (n行番号 == 0) && this.b要素値にフォーカス中);
                        }
                        else
                        {
                            CDTXMania.stageオプション.actFont.t文字列描画(x + 420, y + 19, ((CItemInteger)this.list項目リスト[nItem]).n現在の値.ToString(), (n行番号 == 0) && this.b要素値にフォーカス中);
                        }
                        break;
                    //-----------------
                        #endregion

                    case CItemBase.E種別.リスト:	// #28195 2012.5.2 yyagi: add Skin supports
                        #region [ *** ]
                        //-----------------
                        {
                            CItemList list = (CItemList)this.list項目リスト[nItem];
                            CDTXMania.stageオプション.actFont.t文字列描画(x + 420, y + 19, list.list項目値[list.n現在選択されている項目番号]);
                            break;
                        }
                    //-----------------
                        #endregion
                }
                //-----------------
                #endregion

                nItem = this.t次の項目(nItem);
            }
            //-----------------
            #endregion

            #region [ 項目リストにフォーカスがあって、かつスクロールが停止しているなら、パネルの上下に▲印を描画する。]
            //-----------------
            if (this.b項目リスト側にフォーカスがある && (this.n目標のスクロールカウンタ == 0))
            {
                int x;
                int y_upper;
                int y_lower;

                // 位置決定。

                if (this.b要素値にフォーカス中)
                {
                    x = 552;	// 要素値の上下あたり。
                    y_upper = 0x117 - this.ct三角矢印アニメ.n現在の値;
                    y_lower = 0x17d + this.ct三角矢印アニメ.n現在の値;
                }
                else
                {
                    x = 552;	// 項目名の上下あたり。
                    y_upper = 0x129 - this.ct三角矢印アニメ.n現在の値;
                    y_lower = 0x16b + this.ct三角矢印アニメ.n現在の値;
                }

                // 描画。

                if (this.tx三角矢印 != null)
                {
                    this.tx三角矢印.t2D描画(CDTXMania.app.Device, x, y_upper, new Rectangle(0, 0, 0x40, 0x18));
                    this.tx三角矢印.t2D描画(CDTXMania.app.Device, x, y_lower, new Rectangle(0, 0x18, 0x40, 0x18));
                }
            }
            //-----------------
            #endregion
            return 0;
        }


		// その他

		#region [ private ]
		//-----------------
		private enum Eメニュー種別
		{
			Common=0,
			System=0,
			Drums=1,
			Guitar=2,
			Bass=3,
			Unknown=4
		}

		private bool b項目リスト側にフォーカスがある;
		private bool b要素値にフォーカス中;
		private CCounter ct三角矢印アニメ;
		private Eメニュー種別 eメニュー種別;
		private CItemToggle iBassAutoPlay;
		private CItemToggle iBassHidden;
		private CItemToggle iBassLeft;
		private CItemToggle iBassLight;
		private CItemList iBassPosition;
		private CItemList iBassRandom;
		private CItemBase iBassReturnToMenu;
		private CItemToggle iBassReverse;
		private CItemInteger iBassScrollSpeed;
		private CItemToggle iBassSudden;
		private CItemList iCommonDark;
		private CItemInteger iCommonPlaySpeed;
		private CItemBase iCommonReturnToMenu;
		private CItemThreeState iDrumsAutoPlayAll;
		private CItemToggle iDrumsBass;
		private CItemList iDrumsComboPosition;
		private CItemToggle iDrumsCymbalRide;
		private CItemToggle iDrumsFloorTom;
		private CItemToggle iDrumsHidden;
		private CItemToggle iDrumsHighTom;
		private CItemToggle iDrumsHiHat;
		private CItemToggle iDrumsLeftCymbal;
		private CItemToggle iDrumsLowTom;
		private CItemList iDrumsPosition;
		private CItemBase iDrumsReturnToMenu;
		private CItemToggle iDrumsReverse;
		private CItemInteger iDrumsScrollSpeed;
		private CItemToggle iDrumsSnare;
		private CItemToggle iDrumsSudden;
		private CItemToggle iDrumsTight;
		private CItemToggle iDrumsGraph;        // #24074 2011.01.23 add ikanick
		private CItemToggle iGuitarAutoPlay;
		private CItemToggle iGuitarHidden;
		private CItemToggle iGuitarLeft;
		private CItemToggle iGuitarLight;
		private CItemList iGuitarPosition;
		private CItemList iGuitarRandom;
		private CItemBase iGuitarReturnToMenu;
		private CItemToggle iGuitarReverse;
		private CItemInteger iGuitarScrollSpeed;
		private CItemToggle iGuitarSudden;
		private CItemInteger iDrumsInputAdjustTimeMs;		// #23580 2011.1.3 yyagi
		private CItemInteger iGuitarInputAdjustTimeMs;		//
		private CItemInteger iBassInputAdjustTimeMs;		//


        private List<CItemBase> list項目リスト;
        private long nスクロール用タイマ値;
        private int n現在のスクロールカウンタ;
        private int n目標のスクロールカウンタ;
        private Point[] ptパネルの基本座標 = new Point[] { new Point(0x25a, 4), new Point(0x25a, 0x4f), new Point(0x25a, 0x9a), new Point(0x25a, 0xe5), new Point(0x228, 0x130), new Point(0x25a, 0x17b), new Point(0x25a, 0x1c6), new Point(0x25a, 0x211), new Point(0x25a, 0x25c), new Point(0x25a, 0x2a7) };
        private CTexture txその他項目行パネル;
        private CTexture tx三角矢印;
        private CTexture tx通常項目行パネル;

		private int t前の項目( int nItem )
		{
			if( --nItem < 0 )
			{
				nItem = this.list項目リスト.Count - 1;
			}
			return nItem;
		}
		private int t次の項目( int nItem )
		{
			if( ++nItem >= this.list項目リスト.Count )
			{
				nItem = 0;
			}
			return nItem;
		}
		private void t全部のドラムパッドのAutoを切り替える( bool bAutoON )
		{
			this.iDrumsLeftCymbal.bON = this.iDrumsHiHat.bON = this.iDrumsSnare.bON = this.iDrumsBass.bON = this.iDrumsHighTom.bON = this.iDrumsLowTom.bON = this.iDrumsFloorTom.bON = this.iDrumsCymbalRide.bON = bAutoON;
		}
		private void tConfigIniへ記録する()
		{
			switch( this.eメニュー種別 )
			{
				case Eメニュー種別.Common:
					this.tConfigIniへ記録する・Common();
					return;

				case Eメニュー種別.Drums:
					this.tConfigIniへ記録する・Drums();
					return;

				case Eメニュー種別.Guitar:
					this.tConfigIniへ記録する・Guitar();
					return;

				case Eメニュー種別.Bass:
					this.tConfigIniへ記録する・Bass();
					return;
			}
		}
		private void tConfigIniへ記録する・Bass()
		{
			CDTXMania.ConfigIni.bAutoPlay.Bass = this.iBassAutoPlay.bON;
			CDTXMania.ConfigIni.n譜面スクロール速度.Bass = this.iBassScrollSpeed.n現在の値;
			CDTXMania.ConfigIni.bSudden.Bass = this.iBassSudden.bON;
			CDTXMania.ConfigIni.bHidden.Bass = this.iBassHidden.bON;
			CDTXMania.ConfigIni.bReverse.Bass = this.iBassReverse.bON;
			CDTXMania.ConfigIni.判定文字表示位置.Bass = (E判定文字表示位置) this.iBassPosition.n現在選択されている項目番号;
			CDTXMania.ConfigIni.eRandom.Bass = (Eランダムモード) this.iBassRandom.n現在選択されている項目番号;
			CDTXMania.ConfigIni.bLight.Bass = this.iBassLight.bON;
			CDTXMania.ConfigIni.bLeft.Bass = this.iBassLeft.bON;
			CDTXMania.ConfigIni.nInputAdjustTimeMs.Bass = this.iBassInputAdjustTimeMs.n現在の値;		// #23580 2011.1.3 yyagi
		}
		private void tConfigIniへ記録する・Common()
		{
			CDTXMania.ConfigIni.eDark = (Eダークモード) this.iCommonDark.n現在選択されている項目番号;
			CDTXMania.ConfigIni.n演奏速度 = this.iCommonPlaySpeed.n現在の値;
		}
		private void tConfigIniへ記録する・Drums()
		{
			CDTXMania.ConfigIni.bAutoPlay.LC = this.iDrumsLeftCymbal.bON;
			CDTXMania.ConfigIni.bAutoPlay.HH = this.iDrumsHiHat.bON;
			CDTXMania.ConfigIni.bAutoPlay.SD = this.iDrumsSnare.bON;
			CDTXMania.ConfigIni.bAutoPlay.BD = this.iDrumsBass.bON;
			CDTXMania.ConfigIni.bAutoPlay.HT = this.iDrumsHighTom.bON;
			CDTXMania.ConfigIni.bAutoPlay.LT = this.iDrumsLowTom.bON;
			CDTXMania.ConfigIni.bAutoPlay.FT = this.iDrumsFloorTom.bON;
			CDTXMania.ConfigIni.bAutoPlay.CY = this.iDrumsCymbalRide.bON;
			CDTXMania.ConfigIni.n譜面スクロール速度.Drums = this.iDrumsScrollSpeed.n現在の値;
			CDTXMania.ConfigIni.ドラムコンボ文字の表示位置 = (Eドラムコンボ文字の表示位置) this.iDrumsComboPosition.n現在選択されている項目番号;
			CDTXMania.ConfigIni.bSudden.Drums = this.iDrumsSudden.bON;
			CDTXMania.ConfigIni.bHidden.Drums = this.iDrumsHidden.bON;
			CDTXMania.ConfigIni.bReverse.Drums = this.iDrumsReverse.bON;
			CDTXMania.ConfigIni.判定文字表示位置.Drums = (E判定文字表示位置) this.iDrumsPosition.n現在選択されている項目番号;
			CDTXMania.ConfigIni.bTight = this.iDrumsTight.bON;
			CDTXMania.ConfigIni.nInputAdjustTimeMs.Drums = this.iDrumsInputAdjustTimeMs.n現在の値;		// #23580 2011.1.3 yyagi
            CDTXMania.ConfigIni.bGraph.Drums = this.iDrumsGraph.bON;// #24074 2011.01.23 add ikanick
		}
		private void tConfigIniへ記録する・Guitar()
		{
			CDTXMania.ConfigIni.bAutoPlay.Guitar = this.iGuitarAutoPlay.bON;
			CDTXMania.ConfigIni.n譜面スクロール速度.Guitar = this.iGuitarScrollSpeed.n現在の値;
			CDTXMania.ConfigIni.bSudden.Guitar = this.iGuitarSudden.bON;
			CDTXMania.ConfigIni.bHidden.Guitar = this.iGuitarHidden.bON;
			CDTXMania.ConfigIni.bReverse.Guitar = this.iGuitarReverse.bON;
			CDTXMania.ConfigIni.判定文字表示位置.Guitar = (E判定文字表示位置) this.iGuitarPosition.n現在選択されている項目番号;
			CDTXMania.ConfigIni.eRandom.Guitar = (Eランダムモード) this.iGuitarRandom.n現在選択されている項目番号;
			CDTXMania.ConfigIni.bLight.Guitar = this.iGuitarLight.bON;
			CDTXMania.ConfigIni.bLeft.Guitar = this.iGuitarLeft.bON;
			CDTXMania.ConfigIni.nInputAdjustTimeMs.Guitar = this.iGuitarInputAdjustTimeMs.n現在の値;	// #23580 2011.1.3 yyagi
		}
		//-----------------
		#endregion
	}
}
