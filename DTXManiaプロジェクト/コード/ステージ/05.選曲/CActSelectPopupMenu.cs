using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using FDK;


namespace DTXMania
{
    internal class CActSelectPopupMenu : CActivity
    {

        // プロパティ


        public int GetIndex(int pos)
        {
            return lciMenuItems[pos].GetIndex();
        }
        public object GetObj現在値(int pos)
        {
            return lciMenuItems[pos].obj現在値();
        }
        public bool bGotoDetailConfig
        {
            get;
            internal set;
        }

        /// <summary>
        /// ソートメニュー機能を使用中かどうか。外部からこれをtrueにすると、ソートメニューが出現する。falseにすると消える。
        /// </summary>
        public bool bIsActivePopupMenu
        {
            get;
            private set;
        }
        public virtual void tActivatePopupMenu(E楽器パート einst)
        {
            nItemSelecting = -1;		// #24757 2011.4.1 yyagi: Clear sorting status in each stating menu.
            this.eInst = einst;
            this.bIsActivePopupMenu = true;
            this.bIsSelectingIntItem = false;
            this.bGotoDetailConfig = false;
        }
        public virtual void tDeativatePopupMenu()
        {
            this.bIsActivePopupMenu = false;
        }


        public void Initialize(List<CItemBase> menulist, bool showAllItems, string title)
        {
            Initialize(menulist, showAllItems, title, 0);
        }

        public void Initialize(List<CItemBase> menulist, bool showAllItems, string title, int defaultPos)
        {
            strMenuTitle = title;
            lciMenuItems = menulist;
            bShowAllItems = showAllItems;
            n現在の選択行 = defaultPos;
        }


        public void tEnter押下()
        {
            if (this.bキー入力待ち)
            {
                CDTXMania.Skin.sound決定音.t再生する();

                if (this.n現在の選択行 != lciMenuItems.Count - 1)
                {
                    if (lciMenuItems[n現在の選択行].e種別 == CItemBase.E種別.リスト ||
                         lciMenuItems[n現在の選択行].e種別 == CItemBase.E種別.ONorOFFトグル ||
                         lciMenuItems[n現在の選択行].e種別 == CItemBase.E種別.ONorOFFor不定スリーステート)
                    {
                        lciMenuItems[n現在の選択行].t項目値を次へ移動();
                    }
                    else if (lciMenuItems[n現在の選択行].e種別 == CItemBase.E種別.整数)
                    {
                        bIsSelectingIntItem = !bIsSelectingIntItem;		// 選択状態/選択解除状態を反転する
                    }
                    else if (lciMenuItems[n現在の選択行].e種別 == CItemBase.E種別.切替リスト)
                    {
                        // 特に何もしない
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                    nItemSelecting = n現在の選択行;
                }
                tEnter押下Main((int)lciMenuItems[n現在の選択行].GetIndex());

                this.bキー入力待ち = true;
            }
        }

        /// <summary>
        /// Decide押下時の処理を、継承先で記述する。
        /// </summary>
        /// <param name="val">CItemBaseの現在の設定値のindex</param>
        public virtual void tEnter押下Main(int val)
        {
        }
        /// <summary>
        /// Cancel押下時の追加処理があれば、継承先で記述する。
        /// </summary>
        public virtual void tCancel()
        {
        }
        /// <summary>
        /// BD二回入力時の追加処理があれば、継承先で記述する。
        /// </summary>
        public virtual void tBDContinuity()
        {
        }
        /// <summary>
        /// 追加の描画処理。必要に応じて、継承先で記述する。
        /// </summary>
        public virtual void t進行描画sub()
        {
        }


        public void t次に移動()
        {
            if (this.bキー入力待ち)
            {
                CDTXMania.Skin.soundカーソル移動音.t再生する();
                if (bIsSelectingIntItem)
                {
                    lciMenuItems[n現在の選択行].t項目値を前へ移動();		// 項目移動と数値上下は方向が逆になるので注意
                }
                else
                {
                    if (++this.n現在の選択行 >= this.lciMenuItems.Count)
                    {
                        this.n現在の選択行 = 0;
                    }
                }
            }
        }
        public void t前に移動()
        {
            if (this.bキー入力待ち)
            {
                CDTXMania.Skin.soundカーソル移動音.t再生する();
                if (bIsSelectingIntItem)
                {
                    lciMenuItems[n現在の選択行].t項目値を次へ移動();		// 項目移動と数値上下は方向が逆になるので注意
                }
                else
                {
                    if (--this.n現在の選択行 < 0)
                    {
                        this.n現在の選択行 = this.lciMenuItems.Count - 1;
                    }
                }
            }
        }

        // CActivity 実装

        public override void On活性化()
        {
            //		this.n現在の選択行 = 0;
            this.bキー入力待ち = true;
            for (int i = 0; i < 4; i++)
            {
                this.ctキー反復用[i] = new CCounter(0, 0, 0, CDTXMania.Timer);
            }
            base.b活性化してない = true;

            this.bIsActivePopupMenu = false;
            this.font = new CActDFPFont();
            base.list子Activities.Add(this.font);
            nItemSelecting = -1;

            this.CommandHistory = new DTXMania.CStage選曲.CCommandHistory();
            base.On活性化();
        }
        public override void On非活性化()
        {
            if (!base.b活性化してない)
            {
                base.list子Activities.Remove(this.font);
                this.font.On非活性化();
                this.font = null;

                CDTXMania.tテクスチャの解放(ref this.txCursor);
                CDTXMania.tテクスチャの解放(ref this.txPopupMenuBackground);
                for (int i = 0; i < 4; i++)
                {
                    this.ctキー反復用[i] = null;
                }
                base.On非活性化();
            }
        }
        public override void OnManagedリソースの作成()
        {
            if (!base.b活性化してない)
            {
                string pathCursor = CSkin.Path(@"Graphics\ScreenConfig menu cursor.png"); ;
                string pathPopupMenuBackground = CSkin.Path(@"Graphics\ScreenSelect sort menu background.png");
                if (File.Exists(pathCursor))
                {
                    this.txCursor = CDTXMania.tテクスチャの生成(pathCursor, false);
                }
                if (File.Exists(pathPopupMenuBackground))
                {
                    this.txPopupMenuBackground = CDTXMania.tテクスチャの生成(pathPopupMenuBackground, false);
                }
                base.OnManagedリソースの作成();
            }
        }
        public override void OnManagedリソースの解放()
        {
            if (!base.b活性化してない)
            {
                CDTXMania.tテクスチャの解放(ref this.txPopupMenuBackground);
                CDTXMania.tテクスチャの解放(ref this.txCursor);
            }
            base.OnManagedリソースの解放();
        }

        public override int On進行描画()
        {
            throw new InvalidOperationException("t進行描画(bool)のほうを使用してください。");
        }

        public int t進行描画()
        {
            if (!base.b活性化してない && this.bIsActivePopupMenu)
            {
                n本体X = 460; //XG選曲画面の中心点はX=646 Y=358
                n本体Y = 150;


                if (this.bキー入力待ち)
                {
                    #region [ CONFIG画面 ]
                    if (CDTXMania.Pad.b押された(E楽器パート.GUITAR, Eパッド.Help))
                    {	// [SHIFT] + [F1] CONFIG
                        CDTXMania.Skin.sound取消音.t再生する();
                        tCancel();
                        this.bGotoDetailConfig = true;
                    }
                    #endregion
                    #region [ キー入力: キャンセル ]
                    else if (CDTXMania.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.Escape)
                        || CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.LC)
                        || CDTXMania.Pad.b押されたGB(Eパッド.Pick))
                    {	// キャンセル
                        CDTXMania.Skin.sound取消音.t再生する();
                        tCancel();
                        this.bIsActivePopupMenu = false;
                    }
                    #endregion
                    #region [ BD二回: キャンセル ]
                    else if (CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.BD))
                    {	// キャンセル
                        this.CommandHistory.Add(E楽器パート.DRUMS, EパッドFlag.BD);
                        EパッドFlag[] comChangeScrollSpeed = new EパッドFlag[] { EパッドFlag.BD, EパッドFlag.BD };
                        if (this.CommandHistory.CheckCommand(comChangeScrollSpeed, E楽器パート.DRUMS))
                        {
                            CDTXMania.Skin.sound変更音.t再生する();
                            tBDContinuity();
                            this.bIsActivePopupMenu = false;
                        }
                    }
                    #endregion
                    #region [ Px2 Guitar: 簡易CONFIG ]
                    if (CDTXMania.Pad.b押された(E楽器パート.GUITAR, Eパッド.P))
                    {	// [BD]x2 スクロール速度変更
                        CommandHistory.Add(E楽器パート.GUITAR, EパッドFlag.P);
                        EパッドFlag[] comChangeScrollSpeed = new EパッドFlag[] { EパッドFlag.P, EパッドFlag.P };
                        if (CommandHistory.CheckCommand(comChangeScrollSpeed, E楽器パート.GUITAR))
                        {
                            CDTXMania.Skin.sound変更音.t再生する();
                            tBDContinuity();
                            this.bIsActivePopupMenu = false;
                        }
                    }
                    #endregion
                    #region [ Px2 Bass: 簡易CONFIG ]
                    if (CDTXMania.Pad.b押された(E楽器パート.BASS, Eパッド.P))
                    {	// [BD]x2 スクロール速度変更
                        CommandHistory.Add(E楽器パート.BASS, EパッドFlag.P);
                        EパッドFlag[] comChangeScrollSpeed = new EパッドFlag[] { EパッドFlag.P, EパッドFlag.P };
                        if (CommandHistory.CheckCommand(comChangeScrollSpeed, E楽器パート.BASS))
                        {
                            CDTXMania.Skin.sound変更音.t再生する();
                            tBDContinuity();
                            this.bIsActivePopupMenu = false;
                        }
                    }
                    #endregion

                    #region [ キー入力: 決定 ]
                    // E楽器パート eInst = E楽器パート.UNKNOWN;
                    ESortAction eAction = ESortAction.END;
                    if (CDTXMania.Pad.b押された(E楽器パート.GUITAR, Eパッド.Decide))
                    {
                        eInst = E楽器パート.GUITAR;
                        eAction = ESortAction.Decide;
                    }
                    else if (CDTXMania.Pad.b押された(E楽器パート.BASS, Eパッド.Decide))
                    {
                        eInst = E楽器パート.BASS;
                        eAction = ESortAction.Decide;
                    }
                    else if (
                        CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.Decide)	// #24756 2011.4.1 yyagi: Add condition "Drum-Decide" to enable CY in Sort Menu.
                        || CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.RD)
                        || (CDTXMania.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && CDTXMania.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.Return)))
                    {
                        eInst = E楽器パート.DRUMS;
                        eAction = ESortAction.Decide;
                    }
                    if (eAction == ESortAction.Decide)	// 決定
                    {
                        this.tEnter押下();
                    }
                    #endregion
                    #region [ キー入力: 前に移動 ]
                    this.ctキー反復用.Up.tキー反復(CDTXMania.Input管理.Keyboard.bキーが押されている((int)SlimDX.DirectInput.Key.UpArrow), new CCounter.DGキー処理(this.t前に移動));
                    this.ctキー反復用.R.tキー反復(CDTXMania.Pad.b押されているGB(Eパッド.R), new CCounter.DGキー処理(this.t前に移動));
                    if (CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.SD))
                    {
                        this.t前に移動();
                    }
                    #endregion
                    #region [ キー入力: 次に移動 ]
                    this.ctキー反復用.Down.tキー反復(CDTXMania.Input管理.Keyboard.bキーが押されている((int)SlimDX.DirectInput.Key.DownArrow), new CCounter.DGキー処理(this.t次に移動));
                    this.ctキー反復用.B.tキー反復(CDTXMania.Pad.b押されているGB(Eパッド.G), new CCounter.DGキー処理(this.t次に移動));
                    if (CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.FT))
                    {
                        this.t次に移動();
                    }
                    #endregion
                }
                #region [ ポップアップメニュー 背景描画 ]
                if (this.txPopupMenuBackground != null)
                {
                    this.txPopupMenuBackground.t2D描画(CDTXMania.app.Device, n本体X, n本体Y);
                }
                #endregion
                #region [ ソートメニュータイトル描画 ]
                int x = n本体X + 96, y = n本体Y + 4;
                font.t文字列描画(x, y, strMenuTitle, false, 1.0f);
                #endregion
                #region [ カーソル描画 ]
				if ( this.txCursor != null )
				{
					int height = 32;
                    int curX = n本体X + 12;
                    int curY = n本体Y + 6 + (height * (this.n現在の選択行 + 1));
					this.txCursor.t2D描画( CDTXMania.app.Device, curX, curY, new Rectangle( 0, 0, 16, 32 ) );
					curX += 0x10;
					Rectangle rectangle = new Rectangle( 8, 0, 0x10, 0x20 );
					for ( int j = 0; j < 19; j++ )
					{
						this.txCursor.t2D描画( CDTXMania.app.Device, curX, curY, rectangle );
						curX += 16;
					}
					this.txCursor.t2D描画( CDTXMania.app.Device, curX, curY, new Rectangle( 0x10, 0, 16, 32 ) );
				}
                #endregion
                #region [ ソート候補文字列描画 ]
                for (int i = 0; i < lciMenuItems.Count; i++)
                {
                    bool bItemBold = (i == nItemSelecting && !bShowAllItems) ? true : false;
                    font.t文字列描画(n本体X + 18, n本体Y + 40 + i * 32, lciMenuItems[i].str項目名, bItemBold, 1.0f);

                    bool bValueBold = (bItemBold || (i == nItemSelecting && bIsSelectingIntItem)) ? true : false;
                    if (bItemBold || bShowAllItems)
                    {
                        string s;
                        switch (lciMenuItems[i].str項目名)
                        {
                            case "PlaySpeed":
                                {
                                    double d = (double)((int)lciMenuItems[i].obj現在値() / 20.0);
                                    s = "x" + d.ToString("0.000");
                                }
                                break;
                            case "ScrollSpeed":
                                {
                                    double d = (double)((((int)lciMenuItems[i].obj現在値()) + 1) / 2.0);
                                    s = "x" + d.ToString("0.0");
                                }
                                break;

                            default:
                                s = lciMenuItems[i].obj現在値().ToString();
                                break;
                        }
                        font.t文字列描画(n本体X + 200, n本体Y + 40 + i * 32, s, bValueBold, 1.0f);
                    }
                }
                #endregion
                t進行描画sub();
            }
            return 0;
        }


        // その他

        #region [ private ]
        //-----------------

        private bool bキー入力待ち;

        internal int n現在の選択行;
        internal E楽器パート eInst = E楽器パート.UNKNOWN;

        private CTexture txPopupMenuBackground;
        private CTexture txCursor;
        private CActDFPFont font;

        private int n本体X;
        private int n本体Y;

        private string strMenuTitle;
        private List<CItemBase> lciMenuItems;
        private bool bShowAllItems;
        private bool bIsSelectingIntItem;
        public DTXMania.CStage選曲.CCommandHistory CommandHistory;

        [StructLayout(LayoutKind.Sequential)]
        private struct STキー反復用カウンタ
        {
            public CCounter Up;
            public CCounter Down;
            public CCounter R;
            public CCounter B;
            public CCounter this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return this.Up;

                        case 1:
                            return this.Down;

                        case 2:
                            return this.R;

                        case 3:
                            return this.B;
                    }
                    throw new IndexOutOfRangeException();
                }
                set
                {
                    switch (index)
                    {
                        case 0:
                            this.Up = value;
                            return;

                        case 1:
                            this.Down = value;
                            return;

                        case 2:
                            this.R = value;
                            return;

                        case 3:
                            this.B = value;
                            return;
                    }
                    throw new IndexOutOfRangeException();
                }
            }
        }
        private STキー反復用カウンタ ctキー反復用;

        private enum ESortAction : int
        {
            Cancel, Decide, Previous, Next, END
        }
        private int nItemSelecting;		// 「n現在の選択行」とは別に設ける。sortでメニュー表示直後にアイテムの中身を表示しないようにするため
        //-----------------
        #endregion
    }
}
