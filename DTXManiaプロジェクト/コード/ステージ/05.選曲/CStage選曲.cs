using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using FDK;

namespace DTXMania
{
	internal class CStage選曲 : CStage
	{
		// プロパティ
		public int nスクロールバー相対y座標
		{
			get
			{
				if ( act曲リスト != null )
				{
					return act曲リスト.nスクロールバー相対y座標;
				}
				else
				{
					return 0;
				}
			}
		}
		public bool bIsEnumeratingSongs
		{
			get
			{
				return act曲リスト.bIsEnumeratingSongs;
			}
			set
			{
				act曲リスト.bIsEnumeratingSongs = value;
			}
		}
		public bool bIsPlayingPremovie
		{
			get
			{
				return this.actPreimageパネル.bIsPlayingPremovie;
			}
		}
		public bool bスクロール中
		{
			get
			{
				return this.act曲リスト.bスクロール中;
			}
		}
		public int n確定された曲の難易度
		{
			get;
			private set;
		}
		public Cスコア r確定されたスコア
		{
			get;
			private set;
		}
		public C曲リストノード r確定された曲 
		{
			get;
			private set;
		}
        /// <summary>
        /// <para>現在演奏中の曲のスコアに対応する背景動画。</para>
        /// <para>r現在演奏中の曲のスコア の読み込み時に、自動検索・抽出・生成される。</para>
        /// </summary>
        public CDirectShow r現在演奏中のスコアの背景動画 = null;
		public int n現在選択中の曲の難易度
		{
			get
			{
				return this.act曲リスト.n現在選択中の曲の現在の難易度レベル;
			}
		}
		public Cスコア r現在選択中のスコア
		{
			get
			{
				return this.act曲リスト.r現在選択中のスコア;
			}
		}
		public C曲リストノード r現在選択中の曲
		{
			get
			{
				return this.act曲リスト.r現在選択中の曲;
			}
		}

		// コンストラクタ
		public CStage選曲()
		{
			base.eステージID = CStage.Eステージ.選曲;
			base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
			base.b活性化してない = true;
//			base.list子Activities.Add( this.actオプションパネル = new CActオプションパネル() );
			base.list子Activities.Add( this.actFIFO = new CActFIFOBlack() );
			base.list子Activities.Add( this.actFIfrom結果画面 = new CActFIFOBlack() );
//			base.list子Activities.Add( this.actFOtoNowLoading = new CActFIFOBlack() );	// #27787 2012.3.10 yyagi 曲決定時の画面フェードアウトの省略
			base.list子Activities.Add( this.act曲リスト = new CActSelect曲リスト() );
			base.list子Activities.Add( this.actステータスパネル = new CActSelectステータスパネル() );
			base.list子Activities.Add( this.act演奏履歴パネル = new CActSelect演奏履歴パネル() );
			base.list子Activities.Add( this.actPreimageパネル = new CActSelectPreimageパネル() );
			base.list子Activities.Add( this.actPresound = new CActSelectPresound() );
			base.list子Activities.Add( this.actArtistComment = new CActSelectArtistComment() );
			base.list子Activities.Add( this.actInformation = new CActSelectInformation() );
			base.list子Activities.Add( this.actSortSongs = new CActSortSongs() );
			base.list子Activities.Add( this.actShowCurrentPosition = new CActSelectShowCurrentPosition() );
			base.list子Activities.Add( this.actQuickConfig = new CActSelectQuickConfig() );

			this.CommandHistory = new CCommandHistory();		// #24063 2011.1.16 yyagi
		}
		
		
		// メソッド

		public void t選択曲変更通知()
		{
			this.actPreimageパネル.t選択曲が変更された();
			this.actPresound.t選択曲が変更された();
			this.act演奏履歴パネル.t選択曲が変更された();
			this.actステータスパネル.t選択曲が変更された();
			this.actArtistComment.t選択曲が変更された();

			#region [ プラグインにも通知する（BOX, RANDOM, BACK なら通知しない）]
			//---------------------
			if( CDTXMania.app != null )
			{
				var c曲リストノード = CDTXMania.stage選曲.r現在選択中の曲;
				var cスコア = CDTXMania.stage選曲.r現在選択中のスコア;

				if( c曲リストノード != null && cスコア != null && c曲リストノード.eノード種別 == C曲リストノード.Eノード種別.SCORE )
				{
					string str選択曲ファイル名 = cスコア.ファイル情報.ファイルの絶対パス;
					CSetDef setDef = null;
					int nブロック番号inSetDef = -1;
					int n曲番号inブロック = -1;

					if( !string.IsNullOrEmpty( c曲リストノード.pathSetDefの絶対パス ) && File.Exists( c曲リストノード.pathSetDefの絶対パス ) )
					{
						setDef = new CSetDef( c曲リストノード.pathSetDefの絶対パス );
						nブロック番号inSetDef = c曲リストノード.SetDefのブロック番号;
						n曲番号inブロック = CDTXMania.stage選曲.act曲リスト.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( c曲リストノード );
					}

					foreach( CDTXMania.STPlugin stPlugin in CDTXMania.app.listプラグイン )
					{
						Directory.SetCurrentDirectory( stPlugin.strプラグインフォルダ );
						stPlugin.plugin.On選択曲変更( str選択曲ファイル名, setDef, nブロック番号inSetDef, n曲番号inブロック );
						Directory.SetCurrentDirectory( CDTXMania.strEXEのあるフォルダ );
					}
				}
			}
			//---------------------
			#endregion
		}

		// CStage 実装

		/// <summary>
		/// 曲リストをリセットする
		/// </summary>
		/// <param name="cs"></param>
		public void Refresh( CSongs管理 cs, bool bRemakeSongTitleBar)
		{
			this.act曲リスト.Refresh( cs, bRemakeSongTitleBar );
		}

		public override void On活性化()
		{
			Trace.TraceInformation( "選曲ステージを活性化します。" );
			Trace.Indent();
			try
			{
				this.eフェードアウト完了時の戻り値 = E戻り値.継続;
				this.bBGM再生済み = false;
				this.ftフォント = new Font( "MS PGothic", 26f, GraphicsUnit.Pixel );
				for( int i = 0; i < 4; i++ )
					this.ctキー反復用[ i ] = new CCounter( 0, 0, 0, CDTXMania.Timer );

				base.On活性化();

				this.actステータスパネル.t選択曲が変更された();	// 最大ランクを更新
			}
			finally
			{
				Trace.TraceInformation( "選曲ステージの活性化を完了しました。" );
				Trace.Unindent();
			}
		}
		public override void On非活性化()
		{
			Trace.TraceInformation( "選曲ステージを非活性化します。" );
			Trace.Indent();
			try
			{
				if( this.ftフォント != null )
				{
					this.ftフォント.Dispose();
					this.ftフォント = null;
				}
				for( int i = 0; i < 4; i++ )
				{
					this.ctキー反復用[ i ] = null;
				}
				base.On非活性化();
			}
			finally
			{
				Trace.TraceInformation( "選曲ステージの非活性化を完了しました。" );
				Trace.Unindent();
			}
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				this.tx背景 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_background.jpg" ), false );
				this.tx上部パネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_header panel.png" ), false );
				this.tx下部パネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_footer panel.png" ), false );
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
                CDTXMania.t安全にDisposeする( ref this.r現在演奏中のスコアの背景動画 );
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
				#region [ 初めての進行描画 ]
				//---------------------
				if( base.b初めての進行描画 )
				{
					this.ct登場時アニメ用共通 = new CCounter( 0, 100, 3, CDTXMania.Timer );
					if( CDTXMania.r直前のステージ == CDTXMania.stage結果 )
					{
						this.actFIfrom結果画面.tフェードイン開始();
						base.eフェーズID = CStage.Eフェーズ.選曲_結果画面からのフェードイン;
					}
					else
					{
						this.actFIFO.tフェードイン開始();
						base.eフェーズID = CStage.Eフェーズ.共通_フェードイン;
					}
					this.t選択曲変更通知();
					base.b初めての進行描画 = false;
				}
				//---------------------
				#endregion

				this.ct登場時アニメ用共通.t進行();

				if( this.tx背景 != null )
					this.tx背景.t2D描画( CDTXMania.app.Device, 0, 0 );

				this.actPreimageパネル.On進行描画();
			//	this.bIsEnumeratingSongs = !this.actPreimageパネル.bIsPlayingPremovie;				// #27060 2011.3.2 yyagi: #PREMOVIE再生中は曲検索を中断する

				this.actステータスパネル.On進行描画();
				this.actArtistComment.On進行描画();
				this.act曲リスト.On進行描画();
				this.act演奏履歴パネル.On進行描画();
				int y = 0;
				if( this.ct登場時アニメ用共通.b進行中 )
				{
					double db登場割合 = ( (double) this.ct登場時アニメ用共通.n現在の値 ) / 100.0;	// 100が最終値
					double dbY表示割合 = Math.Sin( Math.PI / 2 * db登場割合 );
					y = ( (int) ( this.tx上部パネル.sz画像サイズ.Height * dbY表示割合 ) ) - this.tx上部パネル.sz画像サイズ.Height;
				}
				if( this.tx上部パネル != null )
						this.tx上部パネル.t2D描画( CDTXMania.app.Device, 0, y );

				this.actInformation.On進行描画();
				if( this.tx下部パネル != null )
					this.tx下部パネル.t2D描画( CDTXMania.app.Device, 0, 720 - this.tx下部パネル.sz画像サイズ.Height );

				this.actPresound.On進行描画();
//				this.actオプションパネル.On進行描画();
				this.actShowCurrentPosition.On進行描画();								// #27648 2011.3.28 yyagi

				switch ( base.eフェーズID )
				{
					case CStage.Eフェーズ.共通_フェードイン:
						if( this.actFIFO.On進行描画() != 0 )
						{
							base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
						}
						break;

					case CStage.Eフェーズ.共通_フェードアウト:
						if( this.actFIFO.On進行描画() == 0 )
						{
							break;
						}
						return (int) this.eフェードアウト完了時の戻り値;

					case CStage.Eフェーズ.選曲_結果画面からのフェードイン:
						if( this.actFIfrom結果画面.On進行描画() != 0 )
						{
							base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
						}
						break;

					case CStage.Eフェーズ.選曲_NowLoading画面へのフェードアウト:
//						if( this.actFOtoNowLoading.On進行描画() == 0 )
//						{
//							break;
//						}
						return (int) this.eフェードアウト完了時の戻り値;
				}
				if( !this.bBGM再生済み && ( base.eフェーズID == CStage.Eフェーズ.共通_通常状態 ) )
				{
					CDTXMania.Skin.bgm選曲画面.n音量・次に鳴るサウンド = 100;
					CDTXMania.Skin.bgm選曲画面.t再生する();
					this.bBGM再生済み = true;
				}


//Debug.WriteLine( "パンくず=" + this.r現在選択中の曲.strBreadcrumbs );


				// キー入力
				if( base.eフェーズID == CStage.Eフェーズ.共通_通常状態 
					&& CDTXMania.act現在入力を占有中のプラグイン == null )
				{
					#region [ 簡易CONFIGでMore、またはShift+F1: 詳細CONFIG呼び出し ]
					if (  actQuickConfig.bGotoDetailConfig )
					{	// 詳細CONFIG呼び出し
						actQuickConfig.tDeativatePopupMenu();
						this.actPresound.tサウンド停止();
						this.eフェードアウト完了時の戻り値 = E戻り値.コンフィグ呼び出し;	// #24525 2011.3.16 yyagi: [SHIFT]-[F1]でCONFIG呼び出し
						this.actFIFO.tフェードアウト開始();
						base.eフェーズID = CStage.Eフェーズ.共通_フェードアウト;
						CDTXMania.Skin.sound取消音.t再生する();
						return 0;
					}
					#endregion
					if ( !this.actSortSongs.bIsActivePopupMenu && !this.actQuickConfig.bIsActivePopupMenu )
					{
                        #region [ ESC ]
                        if (CDTXMania.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.Escape) || ((CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.LC) || CDTXMania.Pad.b押されたGB(Eパッド.Pick)) && ((this.act曲リスト.r現在選択中の曲 != null) && (this.act曲リスト.r現在選択中の曲.r親ノード == null))))
                        {	// [ESC]
                            CDTXMania.Skin.sound取消音.t再生する();
                            this.eフェードアウト完了時の戻り値 = E戻り値.タイトルに戻る;
                            this.actFIFO.tフェードアウト開始();
                            base.eフェーズID = CStage.Eフェーズ.共通_フェードアウト;
                            return 0;
                        }
                        #endregion
                        #region [ CONFIG画面 ]
                        if (CDTXMania.Pad.b押された(E楽器パート.GUITAR, Eパッド.Help))
                        {	// [SHIFT] + [F1] CONFIG
                            this.actPresound.tサウンド停止();
                            this.eフェードアウト完了時の戻り値 = E戻り値.コンフィグ呼び出し;	// #24525 2011.3.16 yyagi: [SHIFT]-[F1]でCONFIG呼び出し
                            this.actFIFO.tフェードアウト開始();
                            base.eフェーズID = CStage.Eフェーズ.共通_フェードアウト;
                            CDTXMania.Skin.sound取消音.t再生する();
                            return 0;
                        }
                        #endregion
                        #region [ Shift-F2: 未使用 ]
                        // #24525 2011.3.16 yyagi: [SHIFT]+[F2]は廃止(将来発生するかもしれない別用途のためにキープ)
                        /*
                        if ((CDTXMania.Input管理.Keyboard.bキーが押されている((int)SlimDX.DirectInput.Key.RightShift) || CDTXMania.Input管理.Keyboard.bキーが押されている((int)SlimDX.DirectInput.Key.LeftShift)) &&
                            CDTXMania.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.F2))
                        {	// [SHIFT] + [F2] CONFIGURATION
                            this.actPresound.tサウンド停止();
                            this.eフェードアウト完了時の戻り値 = E戻り値.オプション呼び出し;
                            this.actFIFO.tフェードアウト開始();
                            base.eフェーズID = CStage.Eフェーズ.共通_フェードアウト;
                            CDTXMania.Skin.sound取消音.t再生する();
                            return 0;
                        }
						*/
                        #endregion
                        if (this.act曲リスト.r現在選択中の曲 != null)
                        {
                            #region [ Decide ]
                            if ((CDTXMania.Pad.b押されたDGB(Eパッド.Decide) || CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.CY) || CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.RD)) ||
                                (CDTXMania.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && CDTXMania.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.Return)))
                            {
                                if (this.act曲リスト.r現在選択中の曲 != null)
                                {
                                    switch (this.act曲リスト.r現在選択中の曲.eノード種別)
                                    {
                                        case C曲リストノード.Eノード種別.SCORE:
                                            CDTXMania.Skin.sound決定音.t再生する();
                                            this.t曲を選択する();
                                            break;

                                        case C曲リストノード.Eノード種別.SCORE_MIDI:
                                            CDTXMania.Skin.sound決定音.t再生する();
                                            this.t曲を選択する();
                                            break;

                                        case C曲リストノード.Eノード種別.BOX:
                                            {
                                                CDTXMania.Skin.sound決定音.t再生する();
                                                bool bNeedChangeSkin = this.act曲リスト.tBOXに入る();
                                                if (bNeedChangeSkin)
                                                {
                                                    this.eフェードアウト完了時の戻り値 = E戻り値.スキン変更;
                                                    base.eフェーズID = Eフェーズ.選曲_NowLoading画面へのフェードアウト;
                                                }
                                            }
                                            break;

                                        case C曲リストノード.Eノード種別.BACKBOX:
                                            {
                                                CDTXMania.Skin.sound取消音.t再生する();
                                                bool bNeedChangeSkin = this.act曲リスト.tBOXを出る();
                                                if (bNeedChangeSkin)
                                                {
                                                    this.eフェードアウト完了時の戻り値 = E戻り値.スキン変更;
                                                    base.eフェーズID = Eフェーズ.選曲_NowLoading画面へのフェードアウト;
                                                }
                                            }
                                            break;

                                        case C曲リストノード.Eノード種別.RANDOM:
                                            CDTXMania.Skin.sound決定音.t再生する();
                                            this.t曲をランダム選択する();
                                            break;
                                    }
                                }
                            }
                            #endregion
                            #region [ Up ]
                            this.ctキー反復用.Up.tキー反復(CDTXMania.Input管理.Keyboard.bキーが押されている((int)SlimDX.DirectInput.Key.UpArrow), new CCounter.DGキー処理(this.tカーソルを上へ移動する));
                            this.ctキー反復用.R.tキー反復(CDTXMania.Pad.b押されているGB(Eパッド.R), new CCounter.DGキー処理(this.tカーソルを上へ移動する));
                            if (CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.SD))
                            {
                                this.tカーソルを上へ移動する();
                            }
                            #endregion
                            #region [ Down ]
                            this.ctキー反復用.Down.tキー反復(CDTXMania.Input管理.Keyboard.bキーが押されている((int)SlimDX.DirectInput.Key.DownArrow), new CCounter.DGキー処理(this.tカーソルを下へ移動する));
                            this.ctキー反復用.B.tキー反復(CDTXMania.Pad.b押されているGB(Eパッド.G), new CCounter.DGキー処理(this.tカーソルを下へ移動する));
                            if (CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.FT))
                            {
                                this.tカーソルを下へ移動する();
                            }
                            #endregion
                            #region [ Upstairs ]
                            if (((this.act曲リスト.r現在選択中の曲 != null) && (this.act曲リスト.r現在選択中の曲.r親ノード != null)) && (CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.LC) || CDTXMania.Pad.b押されたGB(Eパッド.Pick)))
                            {
                                this.actPresound.tサウンド停止();
                                CDTXMania.Skin.sound取消音.t再生する();
                                this.act曲リスト.tBOXを出る();
                                this.t選択曲変更通知();
                            }
                            #endregion
                            #region [ BDx2: 簡易CONFIG ]
                            if (CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.BD))
                            {	// [BD]x2 スクロール速度変更
                                CommandHistory.Add(E楽器パート.DRUMS, EパッドFlag.BD);
                                EパッドFlag[] comChangeScrollSpeed = new EパッドFlag[] { EパッドFlag.BD, EパッドFlag.BD };
                                if (CommandHistory.CheckCommand(comChangeScrollSpeed, E楽器パート.DRUMS))
                                {
                                    // Debug.WriteLine( "ドラムススクロール速度変更" );
                                    // CDTXMania.ConfigIni.n譜面スクロール速度.Drums = ( CDTXMania.ConfigIni.n譜面スクロール速度.Drums + 1 ) % 0x10;
                                    CDTXMania.Skin.sound変更音.t再生する();
                                    this.actQuickConfig.tActivatePopupMenu(E楽器パート.DRUMS);
                                }
                            }
                            #endregion
                            #region [ HHx2: 難易度変更 ]
                            if (CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.HH) || CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.HHO))
                            {	// [HH]x2 難易度変更
                                CommandHistory.Add(E楽器パート.DRUMS, EパッドFlag.HH);
                                EパッドFlag[] comChangeDifficulty = new EパッドFlag[] { EパッドFlag.HH, EパッドFlag.HH };
                                if (CommandHistory.CheckCommand(comChangeDifficulty, E楽器パート.DRUMS))
                                {
                                    Debug.WriteLine("ドラムス難易度変更");
                                    this.act曲リスト.t難易度レベルをひとつ進める();
                                    //CDTXMania.Skin.sound変更音.t再生する();
                                }
                            }
                            #endregion
                            #region [ Bx2 Guitar: 難易度変更 ]
                            if (CDTXMania.Pad.b押された(E楽器パート.GUITAR, Eパッド.B))	// #24177 2011.1.17 yyagi || -> &&
                            {	// [B]x2 ギター難易度変更
                                CommandHistory.Add(E楽器パート.GUITAR, EパッドFlag.B);
                                EパッドFlag[] comChangeDifficultyG = new EパッドFlag[] { EパッドFlag.B, EパッドFlag.B };
                                if (CommandHistory.CheckCommand(comChangeDifficultyG, E楽器パート.GUITAR))
                                {
                                    Debug.WriteLine("ギター難易度変更");
                                    this.act曲リスト.t難易度レベルをひとつ進める();
                                    //CDTXMania.Skin.sound変更音.t再生する();
                                }
                            }
                            #endregion
                            #region [ Bx2 Bass: 難易度変更 ]
                            if (CDTXMania.Pad.b押された(E楽器パート.BASS, Eパッド.B))		// #24177 2011.1.17 yyagi || -> &&
                            {	// [B]x2 ベース難易度変更
                                CommandHistory.Add(E楽器パート.BASS, EパッドFlag.B);
                                EパッドFlag[] comChangeDifficultyB = new EパッドFlag[] { EパッドFlag.B, EパッドFlag.B };
                                if (CommandHistory.CheckCommand(comChangeDifficultyB, E楽器パート.BASS))
                                {
                                    Debug.WriteLine("ベース難易度変更");
                                    this.act曲リスト.t難易度レベルをひとつ進める();
                                    //CDTXMania.Skin.sound変更音.t再生する();
                                }
                            }
                            #endregion
                            #region [ Yx2 Guitar: ギターとベースを入れ替え ]
                            if (CDTXMania.Pad.b押された(E楽器パート.GUITAR, Eパッド.Y))
                            {	// Pick, Y, Y, Pick で、ギターとベースを入れ替え
                                CommandHistory.Add(E楽器パート.GUITAR, EパッドFlag.Y);
                                EパッドFlag[] comSwapGtBs1 = new EパッドFlag[] { EパッドFlag.Y, EパッドFlag.Y };
                                if (CommandHistory.CheckCommand(comSwapGtBs1, E楽器パート.GUITAR))
                                {
                                    Debug.WriteLine("ギターとベースの入れ替え1");
                                    CDTXMania.Skin.sound変更音.t再生する();
                                    // ギターとベースのキーを入れ替え
                                    //CDTXMania.ConfigIni.SwapGuitarBassKeyAssign();
                                    CDTXMania.ConfigIni.bIsSwappedGuitarBass = !CDTXMania.ConfigIni.bIsSwappedGuitarBass;
                                }
                            }
                            #endregion
                            #region [ Yx2 Bass: ギターとベースを入れ替え ]
                            if (CDTXMania.Pad.b押された(E楽器パート.BASS, Eパッド.Y))
                            {	// ベース[Pick]: コマンドとしてEnqueue
                                CommandHistory.Add(E楽器パート.BASS, EパッドFlag.Y);
                                // Pick, Y, Y, Pick で、ギターとベースを入れ替え
                                EパッドFlag[] comSwapGtBs1 = new EパッドFlag[] { EパッドFlag.Y, EパッドFlag.Y };
                                if (CommandHistory.CheckCommand(comSwapGtBs1, E楽器パート.BASS))
                                {
                                    Debug.WriteLine("ギターとベースの入れ替え2");
                                    CDTXMania.Skin.sound変更音.t再生する();
                                    // ギターとベースのキーを入れ替え
                                    //CDTXMania.ConfigIni.SwapGuitarBassKeyAssign();
                                    CDTXMania.ConfigIni.bIsSwappedGuitarBass = !CDTXMania.ConfigIni.bIsSwappedGuitarBass;
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
                                    // Debug.WriteLine( "ドラムススクロール速度変更" );
                                    // CDTXMania.ConfigIni.n譜面スクロール速度.Drums = ( CDTXMania.ConfigIni.n譜面スクロール速度.Drums + 1 ) % 0x10;
                                    CDTXMania.Skin.sound変更音.t再生する();
                                    this.actQuickConfig.tActivatePopupMenu(E楽器パート.GUITAR);
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
                                    // Debug.WriteLine( "ドラムススクロール速度変更" );
                                    // CDTXMania.ConfigIni.n譜面スクロール速度.Drums = ( CDTXMania.ConfigIni.n譜面スクロール速度.Drums + 1 ) % 0x10;
                                    CDTXMania.Skin.sound変更音.t再生する();
                                    this.actQuickConfig.tActivatePopupMenu(E楽器パート.BASS);
                                }
                            }
                            #endregion
                            #region [ Y P Guitar: ソート画面 ]
                            if (CDTXMania.Pad.b押されている(E楽器パート.GUITAR, Eパッド.Y) && CDTXMania.Pad.b押された(E楽器パート.GUITAR, Eパッド.P))
                            {	// ギター[Pick]: コマンドとしてEnqueue
                                CDTXMania.Skin.sound変更音.t再生する();
                                this.actSortSongs.tActivatePopupMenu(E楽器パート.GUITAR, ref this.act曲リスト);
                            }
                            #endregion
                            #region [ Y P Bass: ソート画面 ]
                            if (CDTXMania.Pad.b押されている(E楽器パート.BASS, Eパッド.Y) && CDTXMania.Pad.b押された(E楽器パート.BASS, Eパッド.P))
                            {	// ベース[Pick]: コマンドとしてEnqueue
                                CDTXMania.Skin.sound変更音.t再生する();
                                this.actSortSongs.tActivatePopupMenu(E楽器パート.BASS, ref this.act曲リスト);
                            }
                            #endregion
                            #region [ HTx2 Drums: ソート画面 ]
                            if (CDTXMania.Pad.b押された(E楽器パート.DRUMS, Eパッド.HT))
                            {	// [HT]x2 ソート画面        2013.12.31.kairera0467
                                //
                                CommandHistory.Add(E楽器パート.DRUMS, EパッドFlag.HT);
                                EパッドFlag[] comSort = new EパッドFlag[] { EパッドFlag.HT, EパッドFlag.HT };
                                if (CommandHistory.CheckCommand(comSort, E楽器パート.DRUMS))
                                {
                                    CDTXMania.Skin.sound変更音.t再生する();
                                    this.actSortSongs.tActivatePopupMenu(E楽器パート.DRUMS, ref this.act曲リスト);
                                }
                            }
                            #endregion
                        }
                        //if( CDTXMania.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.F6) )
                        //{
                        //    if (CDTXMania.EnumSongs.IsEnumerating)
                        //    {
                        //        // Debug.WriteLine( "バックグラウンドでEnumeratingSongs中だったので、一旦中断します。" );
                        //        CDTXMania.EnumSongs.Abort();
                        //        CDTXMania.actEnumSongs.On非活性化();
                        //    }

                        //    CDTXMania.EnumSongs.StartEnumFromDisk();
                        //    //CDTXMania.EnumSongs.ChangeEnumeratePriority(ThreadPriority.Normal);
                        //    CDTXMania.actEnumSongs.bコマンドでの曲データ取得 = true;
                        //    CDTXMania.actEnumSongs.On活性化();
                        //}
					}
					this.actSortSongs.t進行描画();
					this.actQuickConfig.t進行描画();
				}
			}
			return 0;
		}
		public enum E戻り値 : int
		{
			継続,
			タイトルに戻る,
			選曲した,
			オプション呼び出し,
			コンフィグ呼び出し,
			スキン変更
		}
		

		// その他

		#region [ private ]
		//-----------------
		[StructLayout( LayoutKind.Sequential )]
		private struct STキー反復用カウンタ
		{
			public CCounter Up;
			public CCounter Down;
			public CCounter R;
			public CCounter B;
			public CCounter this[ int index ]
			{
				get
				{
					switch( index )
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
					switch( index )
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
		private CActSelectArtistComment actArtistComment;
		private CActFIFOBlack actFIFO;
		private CActFIFOBlack actFIfrom結果画面;
//		private CActFIFOBlack actFOtoNowLoading;	// #27787 2012.3.10 yyagi 曲決定時の画面フェードアウトの省略
		private CActSelectInformation actInformation;
		private CActSelectPreimageパネル actPreimageパネル;
		private CActSelectPresound actPresound;
//		private CActオプションパネル actオプションパネル;
		public CActSelectステータスパネル actステータスパネル;
		private CActSelect演奏履歴パネル act演奏履歴パネル;
		private CActSelect曲リスト act曲リスト;
		private CActSelectShowCurrentPosition actShowCurrentPosition;

		private CActSortSongs actSortSongs;
		private CActSelectQuickConfig actQuickConfig;

		private bool bBGM再生済み;
		private STキー反復用カウンタ ctキー反復用;
		public CCounter ct登場時アニメ用共通;
		private E戻り値 eフェードアウト完了時の戻り値;
		private Font ftフォント;
		private CTexture tx下部パネル;
		private CTexture tx上部パネル;
		private CTexture tx背景;

		private struct STCommandTime		// #24063 2011.1.16 yyagi コマンド入力時刻の記録用
		{
			public E楽器パート eInst;		// 使用楽器
			public EパッドFlag ePad;		// 押されたコマンド(同時押しはOR演算で列挙する)
			public long time;				// コマンド入力時刻
		}
		public class CCommandHistory		// #24063 2011.1.16 yyagi コマンド入力履歴を保持・確認するクラス
		{
			readonly int buffersize = 16;
			private List<STCommandTime> stct;

			public CCommandHistory()		// コンストラクタ
			{
				stct = new List<STCommandTime>( buffersize );
			}

			/// <summary>
			/// コマンド入力履歴へのコマンド追加
			/// </summary>
			/// <param name="_eInst">楽器の種類</param>
			/// <param name="_ePad">入力コマンド(同時押しはOR演算で列挙すること)</param>
			public void Add( E楽器パート _eInst, EパッドFlag _ePad )
			{
				STCommandTime _stct = new STCommandTime {
					eInst = _eInst,
					ePad = _ePad,
					time = CDTXMania.Timer.n現在時刻
				};

				if ( stct.Count >= buffersize )
				{
					stct.RemoveAt( 0 );
				}
				stct.Add(_stct);
//Debug.WriteLine( "CMDHIS: 楽器=" + _stct.eInst + ", CMD=" + _stct.ePad + ", time=" + _stct.time );
			}
			public void RemoveAt( int index )
			{
				stct.RemoveAt( index );
			}

			/// <summary>
			/// コマンド入力に成功しているか調べる
			/// </summary>
			/// <param name="_ePad">入力が成功したか調べたいコマンド</param>
			/// <param name="_eInst">対象楽器</param>
			/// <returns>コマンド入力成功時true</returns>
			public bool CheckCommand( EパッドFlag[] _ePad, E楽器パート _eInst)
			{
				int targetCount = _ePad.Length;
				int stciCount = stct.Count;
				if ( stciCount < targetCount )
				{
//Debug.WriteLine("NOT start checking...stciCount=" + stciCount + ", targetCount=" + targetCount);
					return false;
				}

				long curTime = CDTXMania.Timer.n現在時刻;
//Debug.WriteLine("Start checking...targetCount=" + targetCount);
				for ( int i = targetCount - 1, j = stciCount - 1; i >= 0; i--, j-- )
				{
					if ( _ePad[ i ] != stct[ j ].ePad )
					{
//Debug.WriteLine( "CMD解析: false targetCount=" + targetCount + ", i=" + i + ", j=" + j + ": ePad[]=" + _ePad[i] + ", stci[j] = " + stct[j].ePad );
						return false;
					}
					if ( stct[ j ].eInst != _eInst )
					{
//Debug.WriteLine( "CMD解析: false " + i );
						return false;
					}
					if ( curTime - stct[ j ].time > 500 )
					{
//Debug.WriteLine( "CMD解析: false " + i + "; over 500ms" );
						return false;
					}
					curTime = stct[ j ].time;
				}

//Debug.Write( "CMD解析: 成功!(" + _ePad.Length + ") " );
//for ( int i = 0; i < _ePad.Length; i++ ) Debug.Write( _ePad[ i ] + ", " );
//Debug.WriteLine( "" );
				//stct.RemoveRange( 0, targetCount );			// #24396 2011.2.13 yyagi 
				stct.Clear();									// #24396 2011.2.13 yyagi Clear all command input history in case you succeeded inputting some command

				return true;
			}
		}
		public CCommandHistory CommandHistory;

		private void tカーソルを下へ移動する()
		{
			CDTXMania.Skin.soundカーソル移動音.t再生する();
			this.act曲リスト.t次に移動();
		}
		private void tカーソルを上へ移動する()
		{
			CDTXMania.Skin.soundカーソル移動音.t再生する();
			this.act曲リスト.t前に移動();
		}
		private void t曲をランダム選択する()
		{
			C曲リストノード song = this.act曲リスト.r現在選択中の曲;
			if( ( song.stackランダム演奏番号.Count == 0 ) || ( song.listランダム用ノードリスト == null ) )
			{
				if( song.listランダム用ノードリスト == null )
				{
					song.listランダム用ノードリスト = this.t指定された曲が存在する場所の曲を列挙する・子リスト含む( song );
				}
				int count = song.listランダム用ノードリスト.Count;
				if( count == 0 )
				{
					return;
				}
				int[] numArray = new int[ count ];
				for( int i = 0; i < count; i++ )
				{
					numArray[ i ] = i;
				}
				for( int j = 0; j < ( count * 1.5 ); j++ )
				{
					int index = CDTXMania.Random.Next( count );
					int num5 = CDTXMania.Random.Next( count );
					int num6 = numArray[ num5 ];
					numArray[ num5 ] = numArray[ index ];
					numArray[ index ] = num6;
				}
				for( int k = 0; k < count; k++ )
				{
					song.stackランダム演奏番号.Push( numArray[ k ] );
				}
				if( CDTXMania.ConfigIni.bLogDTX詳細ログ出力 )
				{
					StringBuilder builder = new StringBuilder( 0x400 );
					builder.Append( string.Format( "ランダムインデックスリストを作成しました: {0}曲: ", song.stackランダム演奏番号.Count ) );
					for( int m = 0; m < count; m++ )
					{
						builder.Append( string.Format( "{0} ", numArray[ m ] ) );
					}
					Trace.TraceInformation( builder.ToString() );
				}
			}
			this.r確定された曲 = song.listランダム用ノードリスト[ song.stackランダム演奏番号.Pop() ];
			this.n確定された曲の難易度 = this.act曲リスト.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( this.r確定された曲 );
			this.r確定されたスコア = this.r確定された曲.arスコア[ this.n確定された曲の難易度 ];
			this.eフェードアウト完了時の戻り値 = E戻り値.選曲した;
		//	this.actFOtoNowLoading.tフェードアウト開始();					// #27787 2012.3.10 yyagi 曲決定時の画面フェードアウトの省略
			base.eフェーズID = CStage.Eフェーズ.選曲_NowLoading画面へのフェードアウト;
			if( CDTXMania.ConfigIni.bLogDTX詳細ログ出力 )
			{
				int[] numArray2 = song.stackランダム演奏番号.ToArray();
				StringBuilder builder2 = new StringBuilder( 0x400 );
				builder2.Append( "ランダムインデックスリスト残り: " );
				if( numArray2.Length > 0 )
				{
					for( int n = 0; n < numArray2.Length; n++ )
					{
						builder2.Append( string.Format( "{0} ", numArray2[ n ] ) );
					}
				}
				else
				{
					builder2.Append( "(なし)" );
				}
				Trace.TraceInformation( builder2.ToString() );
			}
			CDTXMania.Skin.bgm選曲画面.t停止する();
		}
		private void t曲を選択する()
		{
			this.r確定された曲 = this.act曲リスト.r現在選択中の曲;
			this.r確定されたスコア = this.act曲リスト.r現在選択中のスコア;
			this.n確定された曲の難易度 = this.act曲リスト.n現在選択中の曲の現在の難易度レベル;
			if( ( this.r確定された曲 != null ) && ( this.r確定されたスコア != null ) )
			{
				this.eフェードアウト完了時の戻り値 = E戻り値.選曲した;
			//	this.actFOtoNowLoading.tフェードアウト開始();				// #27787 2012.3.10 yyagi 曲決定時の画面フェードアウトの省略
				base.eフェーズID = CStage.Eフェーズ.選曲_NowLoading画面へのフェードアウト;
			}
			CDTXMania.Skin.bgm選曲画面.t停止する();
		}
		private List<C曲リストノード> t指定された曲が存在する場所の曲を列挙する・子リスト含む( C曲リストノード song )
		{
			List<C曲リストノード> list = new List<C曲リストノード>();
			song = song.r親ノード;
			if( ( song == null ) && ( CDTXMania.Songs管理.list曲ルート.Count > 0 ) )
			{
				foreach( C曲リストノード c曲リストノード in CDTXMania.Songs管理.list曲ルート )
				{
					if( ( c曲リストノード.eノード種別 == C曲リストノード.Eノード種別.SCORE ) || ( c曲リストノード.eノード種別 == C曲リストノード.Eノード種別.SCORE_MIDI ) )
					{
						list.Add( c曲リストノード );
					}
					if( ( c曲リストノード.list子リスト != null ) && CDTXMania.ConfigIni.bランダムセレクトで子BOXを検索対象とする )
					{
						this.t指定された曲の子リストの曲を列挙する・孫リスト含む( c曲リストノード, ref list );
					}
				}
				return list;
			}
			this.t指定された曲の子リストの曲を列挙する・孫リスト含む( song, ref list );
			return list;
		}
		private void t指定された曲の子リストの曲を列挙する・孫リスト含む( C曲リストノード r親, ref List<C曲リストノード> list )
		{
			if( ( r親 != null ) && ( r親.list子リスト != null ) )
			{
				foreach( C曲リストノード c曲リストノード in r親.list子リスト )
				{
					if( ( c曲リストノード.eノード種別 == C曲リストノード.Eノード種別.SCORE ) || ( c曲リストノード.eノード種別 == C曲リストノード.Eノード種別.SCORE_MIDI ) )
					{
						list.Add( c曲リストノード );
					}
					if( ( c曲リストノード.list子リスト != null ) && CDTXMania.ConfigIni.bランダムセレクトで子BOXを検索対象とする )
					{
						this.t指定された曲の子リストの曲を列挙する・孫リスト含む( c曲リストノード, ref list );
					}
				}
			}
		}
		//-----------------
		#endregion
	}
}
