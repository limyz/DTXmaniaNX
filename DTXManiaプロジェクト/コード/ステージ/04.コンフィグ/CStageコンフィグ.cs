using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using FDK;

namespace DTXMania
{
    internal class CStageコンフィグ : CStage
    {
        // プロパティ

        public CActDFPFont actFont { get; private set; }


        // コンストラクタ

        public CStageコンフィグ()
        {
            CActDFPFont font;
            base.eステージID = CStage.Eステージ.コンフィグ;
            base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
            this.actFont = font = new CActDFPFont();
            base.list子Activities.Add(font);
            base.list子Activities.Add(this.actFIFO = new CActFIFOWhite());
            base.list子Activities.Add(this.actList = new CActConfigList());
            base.list子Activities.Add(this.actKeyAssign = new CActConfigKeyAssign());
            //base.list子Activities.Add(this.actオプションパネル = new CActオプションパネル());
            base.b活性化してない = true;
        }


        // メソッド

        public void tアサイン完了通知()															// CONFIGにのみ存在
        {																						//
            this.eItemPanelモード = EItemPanelモード.パッド一覧;								//
        }																						//
        public void tパッド選択通知(EKeyConfigPart part, EKeyConfigPad pad)							//
        {																						//
            this.actKeyAssign.t開始(part, pad, this.actList.ib現在の選択項目.str項目名);		//
            this.eItemPanelモード = EItemPanelモード.キーコード一覧;							//
        }																						//
        public void t項目変更通知()																// OPTIONと共通
        {																						//
            this.t説明文パネルに現在選択されている項目の説明を描画する();						//
        }																						//


        // CStage 実装

        public override void On活性化()
        {
            Trace.TraceInformation("コンフィグステージを活性化します。");
            Trace.Indent();
            try
            {
                this.n現在のメニュー番号 = 0;													//
                this.ftフォント = new Font("MS PGothic", 17f, FontStyle.Regular, GraphicsUnit.Pixel);			//
                for (int i = 0; i < 4; i++)													//
                {																				//
                    this.ctキー反復用[i] = new CCounter(0, 0, 0, CDTXMania.Timer);			//
                }																				//
                this.bメニューにフォーカス中 = true;											// ここまでOPTIONと共通
                this.eItemPanelモード = EItemPanelモード.パッド一覧;
                this.ct表示待機 = new CCounter( 0, 350, 1, CDTXMania.Timer );
            }
            finally
            {
                Trace.TraceInformation("コンフィグステージの活性化を完了しました。");
                Trace.Unindent();
            }
            base.On活性化();		// 2011.3.14 yyagi: On活性化()をtryの中から外に移動
        }
        public override void On非活性化()
        {
            Trace.TraceInformation("コンフィグステージを非活性化します。");
            Trace.Indent();
            try
            {
                CDTXMania.ConfigIni.t書き出し(CDTXMania.strEXEのあるフォルダ + "Config.ini");	// CONFIGだけ
                if (this.ftフォント != null)													// 以下OPTIONと共通
                {
                    this.ftフォント.Dispose();
                    this.ftフォント = null;
                }
                for (int i = 0; i < 4; i++)
                {
                    this.ctキー反復用[i] = null;
                }
                this.ct表示待機 = null;
                base.On非活性化();
            }
            catch (UnauthorizedAccessException e)
            {
                Trace.TraceError(e.Message + "ファイルが読み取り専用になっていないか、管理者権限がないと書き込めなくなっていないか等を確認して下さい");
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
            }
            finally
            {
                Trace.TraceInformation("コンフィグステージの非活性化を完了しました。");
                Trace.Unindent();
            }
        }
        public override void OnManagedリソースの作成()											// OPTIONと画像以外共通
        {
            if (!base.b活性化してない)
            {
                this.tx背景 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\4_background.png" ) );
                this.tx上部パネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\4_header panel.png" ) );
                this.tx下部パネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\4_footer panel.png" ) );
                this.txMenuカーソル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\4_menu cursor.png" ) );
                this.txMenuパネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\4_menu panel.png" ) );
                this.txItemBar = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\4_item bar.png" ) );

				prvFont = new CPrivateFastFont( new FontFamily( CDTXMania.ConfigIni.str選曲リストフォント ), 18 );
				string[] strMenuItem = { "System", "Drums", "Guitar", "Bass", "Exit" };
				txMenuItemLeft = new CTexture[ strMenuItem.Length, 2 ];
				for ( int i = 0; i < strMenuItem.Length; i++ )
				{
					Bitmap bmpStr;
					bmpStr = prvFont.DrawPrivateFont( strMenuItem[ i ], Color.White, Color.Black );
					txMenuItemLeft[ i, 0 ] = CDTXMania.tテクスチャの生成( bmpStr, false );
					bmpStr.Dispose();
					bmpStr = prvFont.DrawPrivateFont( strMenuItem[ i ], Color.White, Color.Black, Color.Yellow, Color.OrangeRed );
					txMenuItemLeft[ i, 1 ] = CDTXMania.tテクスチャの生成( bmpStr, false );
					bmpStr.Dispose();
				}

                if (this.bメニューにフォーカス中)
                {
                    this.t説明文パネルに現在選択されているメニューの説明を描画する();
                }
                else
                {
                    this.t説明文パネルに現在選択されている項目の説明を描画する();
                }
                base.OnManagedリソースの作成();
            }
        }
        public override void OnManagedリソースの解放()											// OPTIONと同じ(COnfig.iniの書き出しタイミングのみ異なるが、無視して良い)
        {
            if (!base.b活性化してない)
            {
                CDTXMania.tテクスチャの解放(ref this.tx背景);
                CDTXMania.tテクスチャの解放(ref this.tx上部パネル);
                CDTXMania.tテクスチャの解放(ref this.tx下部パネル);
                CDTXMania.tテクスチャの解放(ref this.txMenuカーソル);
                CDTXMania.tテクスチャの解放( ref this.txMenuパネル );
                CDTXMania.tテクスチャの解放( ref this.txItemBar );
                CDTXMania.tテクスチャの解放(ref this.tx説明文パネル);
				prvFont.Dispose();
				for ( int i = 0; i < txMenuItemLeft.GetLength(0); i++ )
				{
					txMenuItemLeft[ i, 0 ].Dispose();
					txMenuItemLeft[ i, 0 ] = null;
					txMenuItemLeft[ i, 1 ].Dispose();
					txMenuItemLeft[ i, 1 ] = null;
				}
				txMenuItemLeft = null;
                base.OnManagedリソースの解放();
            }
        }
        public override int On進行描画()
        {
            if (base.b活性化してない)
                return 0;

            if (base.b初めての進行描画)
            {
                base.eフェーズID = CStage.Eフェーズ.共通_フェードイン;
                this.actFIFO.tフェードイン開始();
                base.b初めての進行描画 = false;
            }
            this.ct表示待機.t進行();

            // 描画

            #region [ 背景 ]
            //---------------------
            if (this.tx背景 != null)
                this.tx背景.t2D描画(CDTXMania.app.Device, 0, 0);
            if( this.txItemBar != null )
                this.txItemBar.t2D描画( CDTXMania.app.Device, 400, 0 );
            //---------------------
            #endregion
            #region [ メニューカーソル ]
            //---------------------
            if( this.txMenuパネル != null )
            {
                this.txMenuパネル.t2D描画( CDTXMania.app.Device, 245, 140 );
            }

            if (this.txMenuカーソル != null)
            {
				Rectangle rectangle;
				this.txMenuカーソル.n透明度 = this.bメニューにフォーカス中 ? 0xff : 0x80;
				int x = 250;
				int y = (int)((146 + ( this.n現在のメニュー番号 * 32 )));
				int num3 = (int)(170);
				this.txMenuカーソル.t2D描画( CDTXMania.app.Device, x, y, new Rectangle( 0, 0, (int)0x10, (int)(0x20) ) );
				this.txMenuカーソル.t2D描画( CDTXMania.app.Device, ( x + num3 ) - 0x10, y, new Rectangle( (int)(0x10), 0, (int)(0x10), (int)(0x20) ) );
				x += (int)(0x10);
				for( num3 -= (int)(0x20); num3 > 0; num3 -= rectangle.Width )
				{
					rectangle = new Rectangle( (int)(8), 0, (int)(0x10), (int)(0x20) );
					if( num3 < (int)(0x10) )
					{
						rectangle.Width -= (int)(0x10) - num3;
					}
					this.txMenuカーソル.t2D描画( CDTXMania.app.Device, x, y, rectangle );
					x += rectangle.Width;
				}
            }
            //---------------------
            #endregion
			#region [ メニュー ]
			//---------------------
			int menuY = 144;
			int stepY = 32;
			for ( int i = 0; i < txMenuItemLeft.GetLength(0); i++ )
			{
				//Bitmap bmpStr = (this.n現在のメニュー番号 == i) ?
				//      prvFont.DrawPrivateFont( strMenuItem[ i ], Color.White, Color.Black, Color.Yellow, Color.OrangeRed ) :
				//      prvFont.DrawPrivateFont( strMenuItem[ i ], Color.White, Color.Black );
				//txMenuItemLeft = CDTXMania.tテクスチャの生成( bmpStr, false );
				int flag = ( this.n現在のメニュー番号 == i ) ? 1 : 0;
				int num4 = txMenuItemLeft[ i , flag ].sz画像サイズ.Width;
				txMenuItemLeft[ i, flag ].t2D描画( CDTXMania.app.Device, 340 - (num4 / 2), menuY ); //55
				//txMenuItem.Dispose();
				menuY += stepY;
			}
			//---------------------
			#endregion

            #region [ アイテム ]
            //---------------------
            switch (this.eItemPanelモード)
            {
                case EItemPanelモード.パッド一覧:
                    this.actList.t進行描画(!this.bメニューにフォーカス中);
                    break;

                case EItemPanelモード.キーコード一覧:
                    this.actKeyAssign.On進行描画();
                    break;
            }
            //---------------------
            #endregion
            #region [ 説明文パネル ]
            //---------------------
            if( this.tx説明文パネル != null && !this.bメニューにフォーカス中 && this.actList.n目標のスクロールカウンタ == 0 && this.ct表示待機.b終了値に達した )
                this.tx説明文パネル.t2D描画(CDTXMania.app.Device, 620, 270);
            //---------------------
            #endregion
            #region [ 上部パネル ]
            //---------------------
            if (this.tx上部パネル != null)
                this.tx上部パネル.t2D描画(CDTXMania.app.Device, 0, 0);
            //---------------------
            #endregion
            #region [ 下部パネル ]
            //---------------------
            if (this.tx下部パネル != null)
                this.tx下部パネル.t2D描画(CDTXMania.app.Device, 0, 720 - this.tx下部パネル.szテクスチャサイズ.Height);
            //---------------------
            #endregion
            #region [ オプションパネル ]
            //---------------------
            //this.actオプションパネル.On進行描画();
            //---------------------
            #endregion
            #region [ フェードイン・アウト ]
            //---------------------
            switch (base.eフェーズID)
            {
                case CStage.Eフェーズ.共通_フェードイン:
                    if (this.actFIFO.On進行描画() != 0)
                    {
                        CDTXMania.Skin.bgmコンフィグ画面.t再生する();
                        base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
                    }
                    break;

                case CStage.Eフェーズ.共通_フェードアウト:
                    if (this.actFIFO.On進行描画() == 0)
                    {
                        break;
                    }
                    return 1;
            }
            //---------------------
            #endregion

            #region [ Enumerating Songs ]
            // CActEnumSongs側で表示する
            #endregion
            // キー入力

            if ((base.eフェーズID != CStage.Eフェーズ.共通_通常状態)
                || this.actKeyAssign.bキー入力待ちの最中である
                || CDTXMania.act現在入力を占有中のプラグイン != null)
                return 0;

            // 曲データの一覧取得中は、キー入力を無効化する
            if (!CDTXMania.EnumSongs.IsEnumerating || CDTXMania.actEnumSongs.bコマンドでの曲データ取得 != true)
            {
                if ((CDTXMania.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.Escape) || CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.LC)) || CDTXMania.Pad.b押されたGB(Eパッド.Pick))
                {
                    CDTXMania.Skin.sound取消音.t再生する();
                    if (!this.bメニューにフォーカス中)
                    {
                        if (this.eItemPanelモード == EItemPanelモード.キーコード一覧)
                        {
                            CDTXMania.stageコンフィグ.tアサイン完了通知();
                            return 0;
                        }
                        if (!this.actList.bIsKeyAssignSelected && !this.actList.bIsFocusingParameter)	// #24525 2011.3.15 yyagi, #32059 2013.9.17 yyagi
                        {
                            this.bメニューにフォーカス中 = true;
                        }
                        this.t説明文パネルに現在選択されているメニューの説明を描画する();
                        this.actList.tEsc押下();								// #24525 2011.3.15 yyagi ESC押下時の右メニュー描画用
                    }
                    else
                    {
                        this.actFIFO.tフェードアウト開始();
                        base.eフェーズID = CStage.Eフェーズ.共通_フェードアウト;
                    }
                }
                else if ((CDTXMania.Pad.b押されたDGB(Eパッド.CY) || CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.RD) || (CDTXMania.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && CDTXMania.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.Return))))
                {
                    if (this.n現在のメニュー番号 == 4)
                    {
                        CDTXMania.Skin.sound決定音.t再生する();
                        this.actFIFO.tフェードアウト開始();
                        base.eフェーズID = CStage.Eフェーズ.共通_フェードアウト;
                    }
                    else if (this.bメニューにフォーカス中)
                    {
                        CDTXMania.Skin.sound決定音.t再生する();
                        this.bメニューにフォーカス中 = false;
                        this.t説明文パネルに現在選択されている項目の説明を描画する();
                    }
                    else
                    {
                        switch (this.eItemPanelモード)
                        {
                            case EItemPanelモード.パッド一覧:
                                bool bIsKeyAssignSelectedBeforeHitEnter = this.actList.bIsKeyAssignSelected;	// #24525 2011.3.15 yyagi
                                this.actList.tEnter押下();
                                if (this.actList.b現在選択されている項目はReturnToMenuである)
                                {
                                    this.t説明文パネルに現在選択されているメニューの説明を描画する();
                                    if (bIsKeyAssignSelectedBeforeHitEnter == false)							// #24525 2011.3.15 yyagi
                                    {
                                        this.bメニューにフォーカス中 = true;
                                    }
                                }
                                break;

                            case EItemPanelモード.キーコード一覧:
                                this.actKeyAssign.tEnter押下();
                                break;
                        }
                    }
                }
                this.ctキー反復用.Up.tキー反復(CDTXMania.Input管理.Keyboard.bキーが押されている((int)SlimDX.DirectInput.Key.UpArrow), new CCounter.DGキー処理(this.tカーソルを上へ移動する));
                this.ctキー反復用.R.tキー反復(CDTXMania.Pad.b押されているGB(Eパッド.HH), new CCounter.DGキー処理(this.tカーソルを上へ移動する));
                if (CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.SD))
                {
                    this.tカーソルを上へ移動する();
                }
                this.ctキー反復用.Down.tキー反復(CDTXMania.Input管理.Keyboard.bキーが押されている((int)SlimDX.DirectInput.Key.DownArrow), new CCounter.DGキー処理(this.tカーソルを下へ移動する));
                this.ctキー反復用.B.tキー反復(CDTXMania.Pad.b押されているGB(Eパッド.SD), new CCounter.DGキー処理(this.tカーソルを下へ移動する));
                if (CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.FT))
                {
                    this.tカーソルを下へ移動する();
                }
            }
            return 0;
        }


        // その他

        #region [ private ]
        //-----------------
        private enum EItemPanelモード
        {
            パッド一覧,
            キーコード一覧
        }

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

        private CActFIFOWhite actFIFO;
        private CActConfigKeyAssign actKeyAssign;
        private CActConfigList actList;
        //private CActオプションパネル actオプションパネル;
        private bool bメニューにフォーカス中;
        private STキー反復用カウンタ ctキー反復用;
        private const int DESC_H = 0x80;
        private const int DESC_W = 220;
        private EItemPanelモード eItemPanelモード;
        private Font ftフォント;
        private int n現在のメニュー番号;
        private CTexture txMenuカーソル;
        private CTexture tx下部パネル;
        private CTexture tx上部パネル;
        private CTexture tx説明文パネル;
        private CTexture tx背景;
        private CTexture txMenuパネル;
        private CTexture txItemBar;
        private CPrivateFastFont prvFont;
        private CTexture[,] txMenuItemLeft;
        public CCounter ct表示待機;

        private void tカーソルを下へ移動する()
        {
            if (!this.bメニューにフォーカス中)
            {
                switch (this.eItemPanelモード)
                {
                    case EItemPanelモード.パッド一覧:
                        this.actList.t次に移動();
                        return;

                    case EItemPanelモード.キーコード一覧:
                        this.actKeyAssign.t次に移動();
                        return;
                }
            }
            else
            {
                CDTXMania.Skin.soundカーソル移動音.t再生する();
                this.ct表示待機.n現在の値 = 0;
                this.n現在のメニュー番号 = (this.n現在のメニュー番号 + 1) % 5;
                switch (this.n現在のメニュー番号)
                {
                    case 0:
                        this.actList.t項目リストの設定・System();
                        break;

                    //case 1:
                    //    this.actList.t項目リストの設定・KeyAssignDrums();
                    //    break;

                    //case 2:
                    //    this.actList.t項目リストの設定・KeyAssignGuitar();
                    //    break;

                    //case 3:
                    //    this.actList.t項目リストの設定・KeyAssignBass();
                    //    break;

                    case 1:
                        this.actList.t項目リストの設定・Drums();
                        break;

                    case 2:
                        this.actList.t項目リストの設定・Guitar();
                        break;

                    case 3:
                        this.actList.t項目リストの設定・Bass();
                        break;

                    case 4:
                        this.actList.t項目リストの設定・Exit();
                        break;
                }
                this.t説明文パネルに現在選択されているメニューの説明を描画する();
            }
        }
        private void tカーソルを上へ移動する()
        {
            if (!this.bメニューにフォーカス中)
            {
                switch (this.eItemPanelモード)
                {
                    case EItemPanelモード.パッド一覧:
                        this.actList.t前に移動();
                        return;

                    case EItemPanelモード.キーコード一覧:
                        this.actKeyAssign.t前に移動();
                        return;
                }
            }
            else
            {
                CDTXMania.Skin.soundカーソル移動音.t再生する();
                this.ct表示待機.n現在の値 = 0;
                this.n現在のメニュー番号 = ((this.n現在のメニュー番号 - 1) + 5) % 5;
                switch (this.n現在のメニュー番号)
                {
                    case 0:
                        this.actList.t項目リストの設定・System();
                        break;

                    //case 1:
                    //    this.actList.t項目リストの設定・KeyAssignDrums();
                    //    break;

                    //case 2:
                    //    this.actList.t項目リストの設定・KeyAssignGuitar();
                    //    break;

                    //case 3:
                    //    this.actList.t項目リストの設定・KeyAssignBass();
                    //    break;
                    case 1:
                        this.actList.t項目リストの設定・Drums();
                        break;

                    case 2:
                        this.actList.t項目リストの設定・Guitar();
                        break;

                    case 3:
                        this.actList.t項目リストの設定・Bass();
                        break;

                    case 4:
                        this.actList.t項目リストの設定・Exit();
                        break;
                }
                this.t説明文パネルに現在選択されているメニューの説明を描画する();
            }
        }
		private void t説明文パネルに現在選択されているメニューの説明を描画する()
		{
			try
			{
				var image = new Bitmap( (int)(220 * 2 ), (int)(192 * 2 ) );		// 説明文領域サイズの縦横 2 倍。（描画時に 0.5 倍で表示する。）
				var graphics = Graphics.FromImage( image );
				graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				
				string[,] str = new string[ 2, 2 ];
				switch( this.n現在のメニュー番号 )
				{
					case 0:
						str[ 0, 0 ] = "システムに関係する項目を設定します。";
						str[ 0, 1 ] = "";
						str[ 1, 0 ] = "Settings for an overall systems.";
						break;

					//case 1:
					//    str[0, 0] = "ドラムのキー入力に関する項目を設";
					//    str[0, 1] = "定します。";
					//    str[1, 0] = "Settings for the drums key/pad inputs.";
					//    str[1, 1] = "";
					//    break;

					//case 2:
					//    str[0, 0] = "ギターのキー入力に関する項目を設";
					//    str[0, 1] = "定します。";
					//    str[1, 0] = "Settings for the guitar key/pad inputs.";
					//    str[1, 1] = "";
					//    break;

					//case 3:
					//    str[0, 0] = "ベースのキー入力に関する項目を設";
					//    str[0, 1] = "定します。";
					//    str[1, 0] = "Settings for the bass key/pad inputs.";
					//    str[1, 1] = "";
					//    break;
					case 1:
						str[ 0, 0 ] = "ドラムの演奏に関する項目を設定します。";
						str[ 0, 1 ] = "";
						str[ 1, 0 ] = "Settings to play the drums.";
						str[ 1, 1 ] = "";
						break;

					case 2:
						str[ 0, 0 ] = "ギターの演奏に関する項目を設定します。";
						str[ 0, 1 ] = "";
						str[ 1, 0 ] = "Settings to play the guitar.";
						str[ 1, 1 ] = "";
						break;

					case 3:
						str[ 0, 0 ] = "ベースの演奏に関する項目を設定します。";
						str[ 0, 1 ] = "";
						str[ 1, 0 ] = "Settings to play the bass.";
						str[ 1, 1 ] = "";
						break;

					case 4:
						str[ 0, 0 ] = "設定を保存し、コンフィグ画面を終了します。";
						str[ 0, 1 ] = "";
						str[ 1, 0 ] = "Save the settings and exit from\nCONFIGURATION menu.";
						str[ 1, 1 ] = "";
						break;
				}
				
				int c = (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ja") ? 0 : 1;
				for (int i = 0; i < 2; i++)
				{
					graphics.DrawString( str[ c, i ], this.ftフォント, Brushes.Black, new PointF( 4f , ( i * 30 ) ) );
				}
				graphics.Dispose();
				if( this.tx説明文パネル != null )
				{
					this.tx説明文パネル.Dispose();
				}
				//this.tx説明文パネル = new CTexture( CDTXMania.app.Device, image, CDTXMania.TextureFormat );
				// this.tx説明文パネル.vc拡大縮小倍率.X = 0.5f;
				// this.tx説明文パネル.vc拡大縮小倍率.Y = 0.5f;
				image.Dispose();
			}
			catch( CTextureCreateFailedException )
			{
				Trace.TraceError( "説明文テクスチャの作成に失敗しました。" );
				this.tx説明文パネル = null;
			}
		}
		private void t説明文パネルに現在選択されている項目の説明を描画する()
		{
			try
			{
				var image = new Bitmap( (int)(400), (int)(192) );		// 説明文領域サイズの縦横 2 倍。（描画時に 0.5 倍で表示する・・・のは中止。処理速度向上のため。）
				var graphics = Graphics.FromImage( image );
				graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

				CItemBase item = this.actList.ib現在の選択項目;
				if( ( item.str説明文 != null ) && ( item.str説明文.Length > 0 ) )
				{
					//int num = 0;
					//foreach( string str in item.str説明文.Split( new char[] { '\n' } ) )
					//{
					//    graphics.DrawString( str, this.ftフォント, Brushes.White, new PointF( 4f * Scale.X, (float) num * Scale.Y ) );
					//    num += 30;
					//}
					graphics.DrawString( item.str説明文, this.ftフォント, Brushes.Black, new RectangleF( 4f, (float) 0, 230, 430 ) );
				}
				graphics.Dispose();
				if( this.tx説明文パネル != null )
				{
					this.tx説明文パネル.Dispose();
				}
				this.tx説明文パネル = new CTexture( CDTXMania.app.Device, image, CDTXMania.TextureFormat, false );
				//this.tx説明文パネル.vc拡大縮小倍率.X = 0.58f;
				//this.tx説明文パネル.vc拡大縮小倍率.Y = 0.58f;
				image.Dispose();
			}
			catch( CTextureCreateFailedException )
			{
				Trace.TraceError( "説明文パネルテクスチャの作成に失敗しました。" );
				this.tx説明文パネル = null;
			}
		}
        //-----------------
        #endregion
    }
}
