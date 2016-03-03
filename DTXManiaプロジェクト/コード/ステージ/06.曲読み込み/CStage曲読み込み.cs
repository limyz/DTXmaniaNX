using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using SlimDX;
using SlimDX.Direct3D9;
using System.Drawing.Text;
using FDK;

namespace DTXMania
{
    internal class CStage曲読み込み : CStage
    {
        // コンストラクタ

        public CStage曲読み込み()
        {
            base.eステージID = CStage.Eステージ.曲読み込み;
            base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
            base.b活性化してない = true;
            //			base.list子Activities.Add( this.actFI = new CActFIFOBlack() );	// #27787 2012.3.10 yyagi 曲読み込み画面のフェードインの省略
            base.list子Activities.Add(this.actFO = new CActFIFOBlackStart());

            #region[ 難易度数字 ]
            ST文字位置[] st文字位置Array2 = new ST文字位置[11];
            ST文字位置 st文字位置12 = new ST文字位置();
            st文字位置12.ch = '0';
            st文字位置12.pt = new Point(0, 0);
            st文字位置Array2[0] = st文字位置12;
            ST文字位置 st文字位置13 = new ST文字位置();
            st文字位置13.ch = '1';
            st文字位置13.pt = new Point(100, 0);
            st文字位置Array2[1] = st文字位置13;
            ST文字位置 st文字位置14 = new ST文字位置();
            st文字位置14.ch = '2';
            st文字位置14.pt = new Point(200, 0);
            st文字位置Array2[2] = st文字位置14;
            ST文字位置 st文字位置15 = new ST文字位置();
            st文字位置15.ch = '3';
            st文字位置15.pt = new Point(300, 0);
            st文字位置Array2[3] = st文字位置15;
            ST文字位置 st文字位置16 = new ST文字位置();
            st文字位置16.ch = '4';
            st文字位置16.pt = new Point(400, 0);
            st文字位置Array2[4] = st文字位置16;
            ST文字位置 st文字位置17 = new ST文字位置();
            st文字位置17.ch = '5';
            st文字位置17.pt = new Point(500, 0);
            st文字位置Array2[5] = st文字位置17;
            ST文字位置 st文字位置18 = new ST文字位置();
            st文字位置18.ch = '6';
            st文字位置18.pt = new Point(600, 0);
            st文字位置Array2[6] = st文字位置18;
            ST文字位置 st文字位置19 = new ST文字位置();
            st文字位置19.ch = '7';
            st文字位置19.pt = new Point(700, 0);
            st文字位置Array2[7] = st文字位置19;
            ST文字位置 st文字位置20 = new ST文字位置();
            st文字位置20.ch = '8';
            st文字位置20.pt = new Point(800, 0);
            st文字位置Array2[8] = st文字位置20;
            ST文字位置 st文字位置21 = new ST文字位置();
            st文字位置21.ch = '9';
            st文字位置21.pt = new Point(900, 0);
            st文字位置Array2[9] = st文字位置21;
            ST文字位置 st文字位置22 = new ST文字位置();
            st文字位置22.ch = '-';
            st文字位置22.pt = new Point(0, 40);
            st文字位置Array2[10] = st文字位置22;
            this.st小文字位置 = st文字位置Array2;

            //大文字
            ST文字位置[] st文字位置Array3 = new ST文字位置[12];
            ST文字位置 st文字位置23 = new ST文字位置();
            st文字位置23.ch = '.';
            st文字位置23.pt = new Point(1000, 0);
            st文字位置Array3[0] = st文字位置23;
            ST文字位置 st文字位置24 = new ST文字位置();
            st文字位置24.ch = '1';
            st文字位置24.pt = new Point(100, 0);
            st文字位置Array3[1] = st文字位置24;
            ST文字位置 st文字位置25 = new ST文字位置();
            st文字位置25.ch = '2';
            st文字位置25.pt = new Point(200, 0);
            st文字位置Array3[2] = st文字位置25;
            ST文字位置 st文字位置26 = new ST文字位置();
            st文字位置26.ch = '3';
            st文字位置26.pt = new Point(300, 0);
            st文字位置Array3[3] = st文字位置26;
            ST文字位置 st文字位置27 = new ST文字位置();
            st文字位置27.ch = '4';
            st文字位置27.pt = new Point(400, 0);
            st文字位置Array3[4] = st文字位置27;
            ST文字位置 st文字位置28 = new ST文字位置();
            st文字位置28.ch = '5';
            st文字位置28.pt = new Point(500, 0);
            st文字位置Array3[5] = st文字位置28;
            ST文字位置 st文字位置29 = new ST文字位置();
            st文字位置29.ch = '6';
            st文字位置29.pt = new Point(600, 0);
            st文字位置Array3[6] = st文字位置29;
            ST文字位置 st文字位置30 = new ST文字位置();
            st文字位置30.ch = '7';
            st文字位置30.pt = new Point(700, 0);
            st文字位置Array3[7] = st文字位置30;
            ST文字位置 st文字位置31 = new ST文字位置();
            st文字位置31.ch = '8';
            st文字位置31.pt = new Point(800, 0);
            st文字位置Array3[8] = st文字位置31;
            ST文字位置 st文字位置32 = new ST文字位置();
            st文字位置32.ch = '9';
            st文字位置32.pt = new Point(900, 0);
            st文字位置Array3[9] = st文字位置32;
            ST文字位置 st文字位置33 = new ST文字位置();
            st文字位置33.ch = '0';
            st文字位置33.pt = new Point(0, 0);
            st文字位置Array3[10] = st文字位置33;
            ST文字位置 st文字位置34 = new ST文字位置();
            st文字位置34.ch = '-';
            st文字位置34.pt = new Point(0, 0);
            st文字位置Array3[11] = st文字位置34;
            this.st大文字位置 = st文字位置Array3;
            #endregion

            this.stパネルマップ = null;
            this.stパネルマップ = new STATUSPANEL[12];		// yyagi: 以下、手抜きの初期化でスマン
            string[] labels = new string[12] {
            "DTXMANIA",     //0
            "DEBUT",        //1
            "NOVICE",       //2
            "REGULAR",      //3
            "EXPERT",       //4
            "MASTER",       //5
            "BASIC",        //6
            "ADVANCED",     //7
            "EXTREME",      //8
            "RAW",          //9
            "RWS",          //10
            "REAL"          //11
            };
            int[] status = new int[12] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

            for (int i = 0; i < 12; i++)
            {
                this.stパネルマップ[i] = default(STATUSPANEL);
                this.stパネルマップ[i].status = status[i];
                this.stパネルマップ[i].label = labels[i];
            }
        }

        // CStage 実装

        public void tラベル名からステータスパネルを決定する( string strラベル名 )
        {
            if( string.IsNullOrEmpty( strラベル名 ) )
            {
                this.nIndex = 0;
            }
            else
            {
                STATUSPANEL[] array = this.stパネルマップ;
                for( int i = 0; i < array.Length; i++ )
                {
                    STATUSPANEL sTATUSPANEL = array[ i ];
                    if( strラベル名.Equals( sTATUSPANEL.label, StringComparison.CurrentCultureIgnoreCase ) )
                    {
                        this.nIndex = sTATUSPANEL.status;
                        CDTXMania.nSongDifficulty = sTATUSPANEL.status;
                        return;
                    }
                    this.nIndex++;
                }
            }
        }

        public override void On活性化()
        {
            Trace.TraceInformation( "曲読み込みステージを活性化します。" );
            Trace.Indent();
            try
            {
                this.str曲タイトル = "";
                this.strアーティスト名 = "";
                this.strSTAGEFILE = "";

                this.nBGM再生開始時刻 = -1L;
                this.nBGMの総再生時間ms = 0;
                if( this.sd読み込み音 != null )
                {
                    CDTXMania.Sound管理.tサウンドを破棄する( this.sd読み込み音 );
                    this.sd読み込み音 = null;
                }

                string strDTXファイルパス = ( CDTXMania.bコンパクトモード ) ?
                    CDTXMania.strコンパクトモードファイル : CDTXMania.stage選曲.r確定されたスコア.ファイル情報.ファイルの絶対パス;

                CDTX cdtx = new CDTX( strDTXファイルパス, true );

                if( !CDTXMania.bコンパクトモード && CDTXMania.ConfigIni.b曲名表示をdefのものにする )
                    this.str曲タイトル = CDTXMania.stage選曲.r現在選択中の曲.strタイトル;
                else
                    this.str曲タイトル = cdtx.TITLE;

                this.strアーティスト名 = cdtx.ARTIST;
                if( ( ( cdtx.SOUND_NOWLOADING != null ) && ( cdtx.SOUND_NOWLOADING.Length > 0 ) ) && File.Exists( cdtx.strフォルダ名 + cdtx.SOUND_NOWLOADING ) )
                {
                    string strNowLoadingサウンドファイルパス = cdtx.strフォルダ名 + cdtx.SOUND_NOWLOADING;
                    try
                    {
                        this.sd読み込み音 = CDTXMania.Sound管理.tサウンドを生成する( strNowLoadingサウンドファイルパス );
                    }
                    catch
                    {
                        Trace.TraceError( "#SOUND_NOWLOADING に指定されたサウンドファイルの読み込みに失敗しました。({0})", strNowLoadingサウンドファイルパス );
                    }
                }
                int LEVEL = cdtx.LEVEL.Drums;
                cdtx.On非活性化();
                base.On活性化();
                if( !CDTXMania.bコンパクトモード )
                    this.tラベル名からステータスパネルを決定する( CDTXMania.stage選曲.r確定された曲.ar難易度ラベル[ CDTXMania.stage選曲.n確定された曲の難易度 ] );
            }
            finally
            {
                Trace.TraceInformation( "曲読み込みステージの活性化を完了しました。" );
                Trace.Unindent();
            }
        }
        public override void On非活性化()
        {
            Trace.TraceInformation("曲読み込みステージを非活性化します。");
            Trace.Indent();
            try
            {
                base.On非活性化();
            }
            finally
            {
                Trace.TraceInformation("曲読み込みステージの非活性化を完了しました。");
                Trace.Unindent();
            }
        }
        public override void OnManagedリソースの作成()
        {
            if (!base.b活性化してない)
            {
                this.tx背景 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\6_background.jpg" ) );
                this.txLevel = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\6_LevelNumber.png" ) );
                this.tx難易度パネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\6_Difficulty.png" ) );
                this.txパートパネル = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\6_Part.png"));

                #region[ 曲名、アーティスト名テクスチャの生成 ]
                try
                {
                    #region[ 曲名、アーティスト名テクスチャの生成 ]
                    if ((this.str曲タイトル != null) && (this.str曲タイトル.Length > 0))
                    {
                        pfタイトル = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.str選曲リストフォント), 40, FontStyle.Regular);
                        Bitmap bmpSongName = new Bitmap(1, 1);
                        bmpSongName = pfタイトル.DrawPrivateFont(this.str曲タイトル, CPrivateFont.DrawMode.Edge, Color.Black, Color.Black, this.clGITADORAgradationTopColor, this.clGITADORAgradationBottomColor, true);
                        this.txタイトル = CDTXMania.tテクスチャの生成(bmpSongName, false);
                        bmpSongName.Dispose();
                    }
                    else
                    {
                        this.txタイトル = null;
                    }

                    if ((this.strアーティスト名 != null) && (this.strアーティスト名.Length > 0))
                    {
                        pfアーティスト = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.str選曲リストフォント), 30, FontStyle.Regular);
                        Bitmap bmpArtistName = new Bitmap(1, 1);
                        bmpArtistName = pfアーティスト.DrawPrivateFont(this.strアーティスト名, CPrivateFont.DrawMode.Edge, Color.Black, Color.Black, this.clGITADORAgradationTopColor, this.clGITADORAgradationBottomColor, true);
                        this.txアーティスト = CDTXMania.tテクスチャの生成(bmpArtistName, false);
                        bmpArtistName.Dispose();
                    }
                    else
                    {
                        this.txアーティスト = null;
                    }
                    #endregion
                }
                catch( CTextureCreateFailedException )
                {
                    Trace.TraceError("テクスチャの生成に失敗しました。({0})", new object[] { this.strSTAGEFILE });
                    this.txタイトル = null;
                    this.tx背景 = null;
                }
                #endregion
                base.OnManagedリソースの作成();
            }
        }
        public override void OnManagedリソースの解放()
        {
            if( !base.b活性化してない )
            {
                //テクスチャ11枚
                CDTXMania.tテクスチャの解放( ref this.tx背景 );
                CDTXMania.tテクスチャの解放( ref this.txジャケット );
                CDTXMania.tテクスチャの解放( ref this.txタイトル );
                CDTXMania.tテクスチャの解放( ref this.txアーティスト );
                CDTXMania.tテクスチャの解放( ref this.tx難易度パネル );
                CDTXMania.tテクスチャの解放( ref this.txパートパネル );
                base.OnManagedリソースの解放();
            }
        }
        public override int On進行描画()
        {
            string str;

            if( base.b活性化してない )
                return 0;

            #region [ 初めての進行描画 ]
            //-----------------------------
            if (base.b初めての進行描画)
            {
                Cスコア cスコア1 = CDTXMania.stage選曲.r確定されたスコア;
                if (this.sd読み込み音 != null)
                {
                    if (CDTXMania.Skin.sound曲読込開始音.b排他 && (CSkin.Cシステムサウンド.r最後に再生した排他システムサウンド != null))
                    {
                        CSkin.Cシステムサウンド.r最後に再生した排他システムサウンド.t停止する();
                    }
                    this.sd読み込み音.t再生を開始する();
                    this.nBGM再生開始時刻 = CSound管理.rc演奏用タイマ.n現在時刻;
                    this.nBGMの総再生時間ms = this.sd読み込み音.n総演奏時間ms;
                }
                else
                {
                    CDTXMania.Skin.sound曲読込開始音.t再生する();
                    this.nBGM再生開始時刻 = CSound管理.rc演奏用タイマ.n現在時刻;
                    this.nBGMの総再生時間ms = CDTXMania.Skin.sound曲読込開始音.n長さ・現在のサウンド;
                }
                //				this.actFI.tフェードイン開始();							// #27787 2012.3.10 yyagi 曲読み込み画面のフェードインの省略
                base.eフェーズID = CStage.Eフェーズ.共通_フェードイン;
                base.b初めての進行描画 = false;
                this.tラベル名からステータスパネルを決定する( CDTXMania.stage選曲.r確定された曲.ar難易度ラベル[ CDTXMania.stage選曲.n確定された曲の難易度 ] );

                nWAVcount = 1;
            }
            //-----------------------------
            #endregion

            #region [ ESC押下時は選曲画面に戻る ]
            if (tキー入力())
            {
                if (this.sd読み込み音 != null)
                {
                    this.sd読み込み音.tサウンドを停止する();
                    this.sd読み込み音.t解放する();
                }
                return (int)E曲読込画面の戻り値.読込中止;
            }
            #endregion

            #region [ 背景、レベル、タイトル表示 ]
            //-----------------------------
            if( this.tx背景 != null )
                this.tx背景.t2D描画( CDTXMania.app.Device, 0, 0 );

            string strDTXファイルパス = (CDTXMania.bコンパクトモード) ?
            CDTXMania.strコンパクトモードファイル : CDTXMania.stage選曲.r確定されたスコア.ファイル情報.ファイルの絶対パス;
            CDTX cdtx = new CDTX(strDTXファイルパス, true);

            string path = cdtx.strフォルダ名 + cdtx.PREIMAGE;
            try
            {
                if( !File.Exists( path ) )
                {
                    this.txジャケット = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_preimage default.png" ) );
                }
                else
                {
                    this.txジャケット = CDTXMania.tテクスチャの生成( path );
                }
            }
            #pragma warning disable 0168 // variable declared but not used.
            catch (Exception ex)
            #pragma warning restore 0168 // variable declared but not used.
            {
            }


            int y = 184;

            if( this.txジャケット != null )
            {
                Matrix mat = Matrix.Identity;
                mat *= Matrix.Scaling(384.0f / this.txジャケット.sz画像サイズ.Width, 384.0f / this.txジャケット.sz画像サイズ.Height, 1f);
                mat *= Matrix.Translation(206f, 66f, 0f);
                mat *= Matrix.RotationZ(0.28f);

            this.txジャケット.t3D描画(CDTXMania.app.Device, mat);
            }

            if (this.txタイトル != null)
            {
                if (this.txタイトル.sz画像サイズ.Width > 625)
                    this.txタイトル.vc拡大縮小倍率.X = 625f / this.txタイトル.sz画像サイズ.Width;

                this.txタイトル.t2D描画(CDTXMania.app.Device, 190, 285);
            }

            if (this.txアーティスト != null)
            {
                if (this.txアーティスト.sz画像サイズ.Width > 625)
                    this.txアーティスト.vc拡大縮小倍率.X = 625f / this.txアーティスト.sz画像サイズ.Width;

                this.txアーティスト.t2D描画(CDTXMania.app.Device, 190, 360);
            }

            int[] iPart = { 0, CDTXMania.ConfigIni.bIsSwappedGuitarBass ? 2 : 1, CDTXMania.ConfigIni.bIsSwappedGuitarBass ? 1 : 2 };

            int j = 0;
            int k = 0;
            int DTXLevel = 0;
            double DTXLevelDeci = 0;

            for (int i = 0; i < 3; i++)
            {
                j = iPart[i];

                DTXLevel = cdtx.LEVEL[j];
                DTXLevelDeci = cdtx.LEVELDEC[j];

                if ((CDTXMania.ConfigIni.bDrums有効 && i == 0) || (CDTXMania.ConfigIni.bGuitar有効 && i != 0))
                {

                    if (DTXLevel != 0 || DTXLevelDeci != 0)
                    {
                        if (CDTXMania.stage選曲.r確定されたスコア.譜面情報.b完全にCLASSIC譜面である[j] && !cdtx.b強制的にXG譜面にする)
                        {
                            this.t大文字表示(187 + k, 152, string.Format("{0:00}", DTXLevel));
                        }
                        else
                        {
                            if (cdtx.LEVEL[j] > 99)
                            {
                                DTXLevel = cdtx.LEVEL[j] / 100;
                                DTXLevelDeci = cdtx.LEVEL[j] - (DTXLevel * 100);
                            }
                            else
                            {
                                DTXLevel = cdtx.LEVEL[j] / 10;
                                DTXLevelDeci = ((cdtx.LEVEL[j] - DTXLevel * 10) * 10) + cdtx.LEVELDEC[j];
                            }

                            this.txLevel.t2D描画(CDTXMania.app.Device, 307 + k, 243, new Rectangle(1000, 92, 30, 38));
                            this.t大文字表示(187 + k, 152, string.Format("{0:0}", DTXLevel));
                            this.t大文字表示(357 + k, 152, string.Format("{0:00}", DTXLevelDeci));

                        }

                        if (this.txパートパネル != null)
                            this.txパートパネル.t2D描画(CDTXMania.app.Device, 191 + k, 52, new Rectangle(0, j * 50, 262, 50));

                        //this.txジャケット.Dispose();
                        if (this.tx難易度パネル != null)
                            this.tx難易度パネル.t2D描画(CDTXMania.app.Device, 191 + k, 102, new Rectangle(0, this.nIndex * 50, 262, 50));

                        k = 700;
                    }
                }

                if (i == 2 && k == 0)
                {
                    if (this.txパートパネル != null && CDTXMania.ConfigIni.bDrums有効)
                        this.txパートパネル.t2D描画(CDTXMania.app.Device, 191, 52, new Rectangle(0, 0, 262, 50));

                    if (this.tx難易度パネル != null)
                        this.tx難易度パネル.t2D描画(CDTXMania.app.Device, 191, 102, new Rectangle(0, this.nIndex * 50, 262, 50));
                }
            }
            //-----------------------------
            #endregion

            switch (base.eフェーズID)
            {
                case CStage.Eフェーズ.共通_フェードイン:
                    //if( this.actFI.On進行描画() != 0 )					// #27787 2012.3.10 yyagi 曲読み込み画面のフェードインの省略
                    // 必ず一度「CStaeg.Eフェーズ.共通_フェードイン」フェーズを経由させること。
                    // さもないと、曲読み込みが完了するまで、曲読み込み画面が描画されない。
                    base.eフェーズID = CStage.Eフェーズ.NOWLOADING_DTXファイルを読み込む;
                    return (int)E曲読込画面の戻り値.継続;

                case CStage.Eフェーズ.NOWLOADING_DTXファイルを読み込む:
                    {
                        timeBeginLoad = DateTime.Now;
                        TimeSpan span;
                        str = null;
                        if (!CDTXMania.bコンパクトモード)
                            str = CDTXMania.stage選曲.r確定されたスコア.ファイル情報.ファイルの絶対パス;
                        else
                            str = CDTXMania.strコンパクトモードファイル;

                        CScoreIni ini = new CScoreIni(str + ".score.ini");
                        ini.t全演奏記録セクションの整合性をチェックし不整合があればリセットする();

                        if ((CDTXMania.DTX != null) && CDTXMania.DTX.b活性化してる)
                            CDTXMania.DTX.On非活性化();

                        CDTXMania.DTX = new CDTX(str, false, ((double)CDTXMania.ConfigIni.n演奏速度) / 20.0, ini.stファイル.BGMAdjust);
                        Trace.TraceInformation("----曲情報-----------------");
                        Trace.TraceInformation("TITLE: {0}", CDTXMania.DTX.TITLE);
                        Trace.TraceInformation("FILE: {0}", CDTXMania.DTX.strファイル名の絶対パス);
                        Trace.TraceInformation("---------------------------");

                        span = (TimeSpan)(DateTime.Now - timeBeginLoad);
                        Trace.TraceInformation("DTX読込所要時間:           {0}", span.ToString());

                        if (CDTXMania.bコンパクトモード)
                            CDTXMania.DTX.MIDIレベル = 1;
                        else
                            CDTXMania.DTX.MIDIレベル = (CDTXMania.stage選曲.r確定された曲.eノード種別 == C曲リストノード.Eノード種別.SCORE_MIDI) ? CDTXMania.stage選曲.n現在選択中の曲の難易度 : 0;

                        base.eフェーズID = CStage.Eフェーズ.NOWLOADING_WAVファイルを読み込む;
                        timeBeginLoadWAV = DateTime.Now;
                        return (int)E曲読込画面の戻り値.継続;
                    }

                case CStage.Eフェーズ.NOWLOADING_WAVファイルを読み込む:
                    {
                        if (nWAVcount == 1 && CDTXMania.DTX.listWAV.Count > 0)			// #28934 2012.7.7 yyagi (added checking Count)
                        {
                            //ShowProgressByFilename(CDTXMania.DTX.listWAV[nWAVcount].strファイル名);
                        }
                        int looptime = (CDTXMania.ConfigIni.b垂直帰線待ちを行う) ? 3 : 1;	// VSyncWait=ON時は1frame(1/60s)あたり3つ読むようにする
                        for (int i = 0; i < looptime && nWAVcount <= CDTXMania.DTX.listWAV.Count; i++)
                        {
                            if (CDTXMania.DTX.listWAV[nWAVcount].listこのWAVを使用するチャンネル番号の集合.Count > 0)	// #28674 2012.5.8 yyagi
                            {
                                CDTXMania.DTX.tWAVの読み込み(CDTXMania.DTX.listWAV[nWAVcount]);
                            }
                            nWAVcount++;
                        }
                        if (nWAVcount <= CDTXMania.DTX.listWAV.Count)
                        {
                            //ShowProgressByFilename(CDTXMania.DTX.listWAV[nWAVcount].strファイル名);
                        }
                        if (nWAVcount > CDTXMania.DTX.listWAV.Count)
                        {
                            TimeSpan span = (TimeSpan)(DateTime.Now - timeBeginLoadWAV);
                            Trace.TraceInformation("WAV読込所要時間({0,4}):     {1}", CDTXMania.DTX.listWAV.Count, span.ToString());
                            timeBeginLoadWAV = DateTime.Now;

                            if (CDTXMania.ConfigIni.bDynamicBassMixerManagement)
                            {
                                CDTXMania.DTX.PlanToAddMixerChannel();
                            }
                            CDTXMania.DTX.t譜面仕様変更(E楽器パート.DRUMS, CDTXMania.ConfigIni.eNumOfLanes.Drums);
                            CDTXMania.DTX.t旧仕様のドコドコチップを振り分ける(E楽器パート.DRUMS, CDTXMania.ConfigIni.bAssignToLBD.Drums);
                            CDTXMania.DTX.tドコドコ仕様変更(E楽器パート.DRUMS, CDTXMania.ConfigIni.eDkdkType.Drums);
                            CDTXMania.DTX.tドラムのランダム化(E楽器パート.DRUMS, CDTXMania.ConfigIni.eRandom.Drums);
                            CDTXMania.DTX.tドラムの足ランダム化(E楽器パート.DRUMS, CDTXMania.ConfigIni.eRandomPedal.Drums);
                            CDTXMania.DTX.tギターとベースのランダム化(E楽器パート.GUITAR, CDTXMania.ConfigIni.eRandom.Guitar);
                            CDTXMania.DTX.tギターとベースのランダム化(E楽器パート.BASS, CDTXMania.ConfigIni.eRandom.Bass);

                            if (CDTXMania.ConfigIni.bギタレボモード)
                                CDTXMania.stage演奏ギター画面.On活性化();
                            else
                                CDTXMania.stage演奏ドラム画面.On活性化();

                            span = (TimeSpan)(DateTime.Now - timeBeginLoadWAV);
                            Trace.TraceInformation("WAV/譜面後処理時間({0,4}):  {1}", (CDTXMania.DTX.listBMP.Count + CDTXMania.DTX.listBMPTEX.Count + CDTXMania.DTX.listAVI.Count), span.ToString());

                            base.eフェーズID = CStage.Eフェーズ.NOWLOADING_BMPファイルを読み込む;
                        }
                        return (int)E曲読込画面の戻り値.継続;
                    }

                case CStage.Eフェーズ.NOWLOADING_BMPファイルを読み込む:
                    {
                        TimeSpan span;
                        DateTime timeBeginLoadBMPAVI = DateTime.Now;
                        if (CDTXMania.ConfigIni.bBGA有効)
                            CDTXMania.DTX.tBMP_BMPTEXの読み込み();

                        if (CDTXMania.ConfigIni.bAVI有効)
                            CDTXMania.DTX.tAVIの読み込み();
                        span = (TimeSpan)(DateTime.Now - timeBeginLoadBMPAVI);
                        Trace.TraceInformation("BMP/AVI読込所要時間({0,4}): {1}", (CDTXMania.DTX.listBMP.Count + CDTXMania.DTX.listBMPTEX.Count + CDTXMania.DTX.listAVI.Count), span.ToString());

                        span = (TimeSpan)(DateTime.Now - timeBeginLoad);
                        Trace.TraceInformation("総読込時間:                {0}", span.ToString());
                        CDTXMania.Timer.t更新();
                        base.eフェーズID = CStage.Eフェーズ.NOWLOADING_システムサウンドBGMの完了を待つ;
                        return (int)E曲読込画面の戻り値.継続;
                    }

                case CStage.Eフェーズ.NOWLOADING_システムサウンドBGMの完了を待つ:
                    {
                        long nCurrentTime = CDTXMania.Timer.n現在時刻;
                        if (nCurrentTime < this.nBGM再生開始時刻)
                            this.nBGM再生開始時刻 = nCurrentTime;

                        //						if ( ( nCurrentTime - this.nBGM再生開始時刻 ) > ( this.nBGMの総再生時間ms - 1000 ) )
                        if ((nCurrentTime - this.nBGM再生開始時刻) > (this.nBGMの総再生時間ms))	// #27787 2012.3.10 yyagi 1000ms == フェードイン分の時間
                        {
                            this.actFO.tフェードアウト開始();
                            base.eフェーズID = CStage.Eフェーズ.共通_フェードアウト;
                        }
                        return (int)E曲読込画面の戻り値.継続;
                    }

                case CStage.Eフェーズ.共通_フェードアウト:
                    //if (this.actFO.On進行描画() == 0)
                    //return 0;
                    if (this.sd読み込み音 != null)
                    {
                        this.sd読み込み音.t解放する();
                    }
                    return (int)E曲読込画面の戻り値.読込完了;
            }
            return (int)E曲読込画面の戻り値.継続;
        }


        /// <summary>
        /// ESC押下時、trueを返す
        /// </summary>
        /// <returns></returns>
        protected bool tキー入力()
        {
            IInputDevice keyboard = CDTXMania.Input管理.Keyboard;
            if ( keyboard.bキーが押された( (int)SlimDX.DirectInput.Key.Escape ) )		// escape (exit)
            {
                if ( CDTXMania.ConfigIni.bギタレボモード )
                {
                    if (CDTXMania.stage演奏ギター画面.b活性化してる == true)
                        CDTXMania.stage演奏ギター画面.On非活性化();
                }
                else
                {
                    if (CDTXMania.stage演奏ドラム画面.b活性化してる == true)
                        CDTXMania.stage演奏ドラム画面.On非活性化();
                }

                return true;
            }
            return false;
        }

        // その他

        #region [ private ]
        //-----------------
        [StructLayout(LayoutKind.Sequential)]
        private struct ST文字位置
        {
            public char ch;
            public Point pt;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct ST泡
        {
            public bool b使用中;
            public CCounter ct進行;
            public int n前回のValue;
            public float fX;
            public float fY;
            public float f加速度X;
            public float f加速度Y;
            public float f加速度の加速度X;
            public float f加速度の加速度Y;
            public float f半径;
        }
        //		private CActFIFOBlack actFI;
        private CActFIFOBlackStart actFO;

        private readonly ST文字位置[] st小文字位置;
        private readonly ST文字位置[] st大文字位置;
        private long nBGMの総再生時間ms;
        private long nBGM再生開始時刻;
        private CSound sd読み込み音;
        private string strSTAGEFILE;
        private string str曲タイトル;
        private string strアーティスト名;
        private CTexture txタイトル;
        private CTexture txアーティスト;
        private CTexture txジャケット;
        private CTexture tx背景;
        private CTexture tx難易度パネル;
        private CTexture txパートパネル;

        private CPrivateFastFont pfタイトル;
        private CPrivateFastFont pfアーティスト;

        //2014.04.05.kairera0467 GITADORAグラデーションの色。
        //本当は共通のクラスに設置してそれを参照する形にしたかったが、なかなかいいメソッドが無いため、とりあえず個別に設置。
        private Color clGITADORAgradationTopColor = Color.FromArgb(0, 220, 200);
        private Color clGITADORAgradationBottomColor = Color.FromArgb(255, 250, 40);

        private DateTime timeBeginLoad;
        private DateTime timeBeginLoadWAV;
        private int nWAVcount;
        private CTexture txLevel;

        [StructLayout(LayoutKind.Sequential)]
        public struct STATUSPANEL
        {
            public string label;
            public int status;
        }
        public int nIndex;
        public STATUSPANEL[] stパネルマップ;
        //-----------------

        private void t小文字表示(int x, int y, string str)
        {
            this.t小文字表示(x, y, str, false);
        }
        private void t小文字表示(int x, int y, string str, bool b強調)
        {
            foreach (char ch in str)
            {
                for (int i = 0; i < this.st小文字位置.Length; i++)
                {
                    if (this.st小文字位置[i].ch == ch)
                    {
                        Rectangle rectangle = new Rectangle(this.st小文字位置[i].pt.X, this.st小文字位置[i].pt.Y, 13, 22);
                        if (this.txLevel != null)
                        {
                            this.txLevel.t2D描画(CDTXMania.app.Device, x, y, rectangle);
                        }
                        break;
                    }
                }
                x += 12;
            }
        }
        private void t大文字表示(int x, int y, string str)
        {
            this.t大文字表示(x, y, str, false);
        }
        private void t大文字表示(int x, int y, string str, bool bExtraLarge)
        {
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                for (int j = 0; j < this.st大文字位置.Length; j++)
                {
                    if (this.st大文字位置[j].ch == c)
                    {
                        int num;
                        int num2;
                        num = 0;
                        num2 = 0;
                        Rectangle rc画像内の描画領域 = new Rectangle(this.st大文字位置[j].pt.X, this.st大文字位置[j].pt.Y, 100, 130);
                        if (this.txLevel != null)
                        {
                            this.txLevel.t2D描画(CDTXMania.app.Device, x, y, rc画像内の描画領域);
                        }
                        break;
                    }
                }
                if (c == '.')
                {
                    x += 30;
                }
                else
                {
                    x += 90;
                }
            }
        }
        #endregion
    }
}
