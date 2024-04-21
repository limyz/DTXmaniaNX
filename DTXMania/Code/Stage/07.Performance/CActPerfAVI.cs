using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using SharpDX;
using SharpDX.Direct3D9;
using DirectShowLib;
using FDK;

using Rectangle = System.Drawing.Rectangle;
using Point = System.Drawing.Point;

namespace DTXMania
{
    internal class CActPerfAVI : CActivity
    {
        // コンストラクタ

        public CActPerfAVI(bool bIsDuringPerformance = true)
        {
            this.bIsDuringPerformance = bIsDuringPerformance;
            if (this.bIsDuringPerformance)
            {
                //base.listChildActivities.Add(this.actFill = new CActPerfDrumsFillingEffect());
                base.listChildActivities.Add(this.actPanel = new CActPerfPanelString());
            }
            
            base.bNotActivated = true;
        }


        // メソッド
        
        public void Start(EChannel nチャンネル番号, CDTX.CAVI rAVI, int n開始サイズW, int n開始サイズH, int n終了サイズW, int n終了サイズH, int n画像側開始位置X, int n画像側開始位置Y, int n画像側終了位置X, int n画像側終了位置Y, int n表示側開始位置X, int n表示側開始位置Y, int n表示側終了位置X, int n表示側終了位置Y, int n総移動時間ms, int n移動開始時刻ms, bool bPlayFromBeginning = false)
        {
            //2016.01.21 kairera0467 VfW時代のコードを除去+大改造
            Trace.TraceInformation("CActPerfAVI: Start(): " + rAVI.strファイル名);

            this.rAVI = rAVI;
            
            #region[ アスペクト比からどっちを使うか判別 ]
            // 旧DShowモードを使っていて、旧規格クリップだったら新DShowモードを使う。
            //if( CDTXMania.ConfigIni.bDirectShowMode == false )
            {
                this.fClipアスペクト比 = ( (float)rAVI.avi.nフレーム幅 / (float)rAVI.avi.nフレーム高さ );
                //this.bUseMRenderer = false;
            }
            
            #endregion

            if( nチャンネル番号 == EChannel.Movie || nチャンネル番号 == EChannel.MovieFull)
            {
                if( this.bUseCAviDS )
                {
                    //CAviDS
                    this.rAVI = rAVI;
                    this.n開始サイズW = n開始サイズW;
                    this.n開始サイズH = n開始サイズH;
                    this.n終了サイズW = n終了サイズW;
                    this.n終了サイズH = n終了サイズH;
                    this.n画像側開始位置X = n画像側開始位置X;
                    this.n画像側開始位置Y = n画像側開始位置Y;
                    this.n画像側終了位置X = n画像側終了位置X;
                    this.n画像側終了位置Y = n画像側終了位置Y;
                    this.n表示側開始位置X = n表示側開始位置X;
                    this.n表示側開始位置Y = n表示側開始位置Y;
                    this.n表示側終了位置X = n表示側終了位置X;
                    this.n表示側終了位置Y = n表示側終了位置Y;
                    this.n総移動時間ms = n総移動時間ms;
                    this.n移動開始時刻ms = ( n移動開始時刻ms != -1 ) ? n移動開始時刻ms : CSoundManager.rcPerformanceTimer.nCurrentTime;

                    if( ( this.rAVI != null ) && ( this.rAVI.avi != null ) )
                    {
                        float f拡大率x;
                        float f拡大率y;
                        this.framewidth = (uint)this.rAVI.avi.nフレーム幅;
                        this.frameheight = (uint)this.rAVI.avi.nフレーム高さ;
                        if( this.tx描画用 == null )
                        {
                            this.tx描画用 = new CTexture( CDTXMania.app.Device, (int)this.framewidth, (int)this.frameheight, CDTXMania.app.GraphicsDeviceManager.CurrentSettings.BackBufferFormat, Pool.Managed );
                        }

                        if( fClipアスペクト比 < 1.77f )
                        {
                            //旧規格クリップだった場合
                            this.ratio1 = 720f / ( (float)this.frameheight );
                            this.position = (int)( ( 1280f - ( this.framewidth * this.ratio1 ) ) / 2f );
                            int num = (int)( this.framewidth * this.ratio1 );
                            if( num <= 565 )
                            {
                                this.position = 295 + ( (int)( ( 565f - ( this.framewidth * this.ratio1 ) ) / 2f ) );
                                this.i1 = 0;
                                this.i2 = (int)this.framewidth;
                                this.rec = new Rectangle( 0, 0, 0, 0 );
                                this.rec3 = new Rectangle( 0, 0, 0, 0 );
                                this.rec2 = new Rectangle( 0, 0, (int)this.framewidth, (int)this.frameheight );
                            }
                            else
                            {
                                this.position = 295 - ( (int)( ( ( this.framewidth * this.ratio1 ) - 565f ) / 2f ) );
                                this.i1 = (int)( ( (float)( 295 - this.position ) ) / this.ratio1 );
                                this.i2 = (int)( ( 565f / ( (float)num ) ) * this.framewidth );
                                this.rec = new Rectangle( 0, 0, this.i1, (int)this.frameheight );
                                this.rec3 = new Rectangle( this.i1 + this.i2, 0, ( ( (int)this.framewidth ) - this.i1 ) - this.i2, (int)this.frameheight );
                                this.rec2 = new Rectangle( this.i1, 0, this.i2, (int)this.frameheight );
                            }
                            this.tx描画用.vcScaleRatio.X = this.ratio1;
                            this.tx描画用.vcScaleRatio.Y = this.ratio1;
                        }
                        else
                        {
                            //ワイドクリップの処理
                            this.ratio1 = 1280f / ( (float)this.framewidth );
                            this.position = (int)( ( 720f - ( this.frameheight * this.ratio1 ) ) / 2f );
                            this.i1 = (int)( this.framewidth * 0.23046875 );
                            this.i2 = (int)( this.framewidth * 0.44140625 );
                            this.rec = new Rectangle( 0, 0, this.i1, (int)this.frameheight );
                            this.rec2 = new Rectangle( this.i1, 0, this.i2, (int)this.frameheight );
                            this.rec3 = new Rectangle( this.i1 + this.i2, 0, ( ( (int)this.framewidth ) - this.i1 ) - this.i2, (int)this.frameheight );
                            this.tx描画用.vcScaleRatio.X = this.ratio1;
                            this.tx描画用.vcScaleRatio.Y = this.ratio1;
                        }


                        if( this.framewidth > 420 )
                        {
                            f拡大率x = 420f / ( (float)this.framewidth );
                        }
                        else
                        {
                            f拡大率x = 1f;
                        }
                        if( this.frameheight > 580 )
                        {
                            f拡大率y = 580f / ( (float)this.frameheight );
                        }
                        else
                        {
                            f拡大率y = 1f;
                        }
                        if( f拡大率x > f拡大率y )
                        {
                            f拡大率x = f拡大率y;
                        }
                        else
                        {
                            f拡大率y= f拡大率x;
                        }

                        this.smallvc = new Vector3( f拡大率x, f拡大率y, 1f );
                        this.vclip = new Vector3( 1.42f, 1.42f, 1f );
                        this.rAVI.avi.Run();
                    }
                }
                else
                {
                    this.rAVI = rAVI;
                    this.n開始サイズW = n開始サイズW;
                    this.n開始サイズH = n開始サイズH;
                    this.n終了サイズW = n終了サイズW;
                    this.n終了サイズH = n終了サイズH;
                    this.n画像側開始位置X = n画像側開始位置X;
                    this.n画像側開始位置Y = n画像側開始位置Y;
                    this.n画像側終了位置X = n画像側終了位置X;
                    this.n画像側終了位置Y = n画像側終了位置Y;
                    this.n表示側開始位置X = n表示側開始位置X;
                    this.n表示側開始位置Y = n表示側開始位置Y;
                    this.n表示側終了位置X = n表示側終了位置X;
                    this.n表示側終了位置Y = n表示側終了位置Y;
                    this.n総移動時間ms = n総移動時間ms;
                    this.n移動開始時刻ms = ( n移動開始時刻ms != -1 ) ? n移動開始時刻ms : CSoundManager.rcPerformanceTimer.nCurrentTime;
                    this.n前回表示したフレーム番号 = -1;
                    if( ( this.rAVI != null ) && ( this.rAVI.avi != null ) )
                    {
                        float f拡大率x;
                        float f拡大率y;
                        this.framewidth = this.rAVI.avi.nフレーム幅;
                        this.frameheight = this.rAVI.avi.nフレーム高さ;
                        if( this.tx描画用 == null )
                        {
                            this.tx描画用 = new CTexture( CDTXMania.app.Device, (int)this.framewidth, (int)this.frameheight, CDTXMania.app.GraphicsDeviceManager.CurrentSettings.BackBufferFormat, Pool.Managed );
                        }
                        if( fClipアスペクト比 < 1.77f )
                        {
                            //旧規格クリップだった場合
                            this.ratio1 = 720.0f / this.frameheight;
                            this.position = (int)( ( 1280.0f - ( this.framewidth * this.ratio1 ) ) / 2.0f );
                            int num = (int)( this.framewidth * this.ratio1 );
                            if( num <= 565 )
                            {
                                this.position = 295 + ( (int)( ( 565f - ( this.framewidth * this.ratio1 ) ) / 2f ) );
                                this.i1 = 0;
                                this.i2 = (int)this.framewidth;
                                this.rec = new Rectangle(0, 0, 0, 0);
                                this.rec3 = new Rectangle(0, 0, 0, 0);
                                this.rec2 = new Rectangle(0, 0, (int)this.framewidth, (int)this.frameheight);
                            }
                            else
                            {
                                this.position = 295 - ((int)(((this.framewidth * this.ratio1) - 565f) / 2f));
                                this.i1 = (int)(((float)(295 - this.position)) / this.ratio1);
                                this.i2 = (int)((565f / ((float)num)) * this.framewidth);
                                this.rec = new Rectangle(0, 0, this.i1, (int)this.frameheight);
                                this.rec3 = new Rectangle(this.i1 + this.i2, 0, (((int)this.framewidth) - this.i1) - this.i2, (int)this.frameheight);
                                this.rec2 = new Rectangle(this.i1, 0, this.i2, (int)this.frameheight);
                            }
                            this.tx描画用.vcScaleRatio.X = this.ratio1;
                            this.tx描画用.vcScaleRatio.Y = this.ratio1;
                        }
                        else
                        {
                            //ワイドクリップの処理
                            this.ratio1 = 1280f / ((float)this.framewidth);
                            this.position = (int)((720f - (this.frameheight * this.ratio1)) / 2f);
                            this.i1 = (int)(this.framewidth * 0.23046875);
                            this.i2 = (int)(this.framewidth * 0.44140625);
                            this.rec = new Rectangle(0, 0, this.i1, (int)this.frameheight);
                            this.rec2 = new Rectangle(this.i1, 0, this.i2, (int)this.frameheight);
                            this.rec3 = new Rectangle(this.i1 + this.i2, 0, (((int)this.framewidth) - this.i1) - this.i2, (int)this.frameheight);
                            this.tx描画用.vcScaleRatio.X = this.ratio1;
                            this.tx描画用.vcScaleRatio.Y = this.ratio1;
                        }


                        if (this.framewidth > 420)
                        {
                            f拡大率x = 420f / ((float)this.framewidth);
                        }
                        else
                        {
                            f拡大率x = 1f;
                        }
                        if (this.frameheight > 580)
                        {
                            f拡大率y = 580f / ((float)this.frameheight);
                        }
                        else
                        {
                            f拡大率y = 1f;
                        }
                        if (f拡大率x > f拡大率y)
                        {
                            f拡大率x = f拡大率y;
                        }
                        else
                        {
                            f拡大率y = f拡大率x;
                        }

                        this.smallvc = new Vector3(f拡大率x, f拡大率y, 1f);
                        this.vclip = new Vector3(1.42f, 1.42f, 1f);
                    }
                }
            }

        }
        public void SkipStart(int n移動開始時刻ms)
        {
            if (CDTXMania.DTX != null) {
                foreach (CChip chip in CDTXMania.DTX.listChip)
                {
                    if (chip.nPlaybackTimeMs > n移動開始時刻ms)
                    {
                        break;
                    }
                    switch (chip.eAVI種別)
                    {
                        case EAVIType.AVI:
                            {
                                if (chip.rAVI != null)
                                {
                                    if (chip.rAVI.avi != null) {
                                        chip.rAVI.avi.Seek(n移動開始時刻ms - chip.nPlaybackTimeMs);
                                    }
                                    this.Start(chip.nChannelNumber, chip.rAVI, 1280, 720, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, chip.nPlaybackTimeMs);
                                }
                                continue;
                            }
                        case EAVIType.AVIPAN:
                            {
                                if (chip.rAVIPan != null)
                                {
                                    if (chip.rAVI != null && chip.rAVI.avi != null)
                                    {
                                        chip.rAVI.avi.Seek(n移動開始時刻ms - chip.nPlaybackTimeMs);
                                    }
                                    this.Start(chip.nChannelNumber, chip.rAVI, chip.rAVIPan.sz開始サイズ.Width, chip.rAVIPan.sz開始サイズ.Height, chip.rAVIPan.sz終了サイズ.Width, chip.rAVIPan.sz終了サイズ.Height, chip.rAVIPan.pt動画側開始位置.X, chip.rAVIPan.pt動画側開始位置.Y, chip.rAVIPan.pt動画側終了位置.X, chip.rAVIPan.pt動画側終了位置.Y, chip.rAVIPan.pt表示側開始位置.X, chip.rAVIPan.pt表示側開始位置.Y, chip.rAVIPan.pt表示側終了位置.X, chip.rAVIPan.pt表示側終了位置.Y, chip.n総移動時間, chip.nPlaybackTimeMs);
                                }
                                continue;
                            }
                    }
                }
            }
            
        }
        public void Stop()
        {
            Trace.TraceInformation("CActPerfAVI: Stop()");
            if ((this.rAVI != null) && (this.rAVI.avi != null))
            {  
                this.n移動開始時刻ms = -1;
                this.rAVI.avi.Stop();
                this.rAVI.avi.Seek(0);
            }
            //if (this.dsBGV != null && CDTXMania.ConfigIni.bDirectShowMode == true)
            //{
            //    this.dsBGV.dshow.MediaCtrl.Stop();
            //    this.bDShowクリップを再生している = false;
            //}
        }
        public void MovieMode()
        {
            this.nCurrentMovieMode = CDTXMania.ConfigIni.nMovieMode;
            if ((this.nCurrentMovieMode == 1) || (this.nCurrentMovieMode == 3))
            {
                this.bFullScreen = true;
            }
            else
            {
                this.bFullScreen = false;
            }
            if ((this.nCurrentMovieMode == 2) || (this.nCurrentMovieMode == 3))
            {
                this.bWindowMode = true;
            }
            else
            {
                this.bWindowMode = false;
            }
        }

        public void Cont(int n再開時刻ms)
        {
            if ((this.rAVI != null) && (this.rAVI.avi != null))
            {
                this.n移動開始時刻ms = n再開時刻ms;
            }
        }


        // CActivity 実装
        public override void OnActivate()
        {
            //this.rAVI = null;
            //this.dsBGV = null;
            this.n移動開始時刻ms = -1;
            this.n前回表示したフレーム番号 = -1;
            this.bフレームを作成した = false;
            this.b再生トグル = false;
            this.bDShowクリップを再生している = false;
            this.pBmp = IntPtr.Zero;
            this.MovieMode();
            this.nAlpha = 255 - ((int)(((float)(CDTXMania.ConfigIni.nMovieAlpha * 255)) / 10f));
            
            base.OnActivate();
        }
        public override void OnDeactivate()
        {            
            base.OnDeactivate();
        }
        public override void OnManagedCreateResources()
        {
            if (!base.bNotActivated)
            {
                //this.txドラム = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Drums.png"));
                if (CDTXMania.ConfigIni.bGuitarEnabled)
                {
                    this.txクリップパネル = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_ClipPanelC.png"));
                }
                else if (CDTXMania.ConfigIni.bGraph有効.Drums && CDTXMania.ConfigIni.bDrumsEnabled)
                {
                    this.txクリップパネル = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_ClipPanelB.png"));
                }
                else
                {
                    this.txクリップパネル = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_ClipPanel.png"));
                }
                this.txDShow汎用 = new CTexture(CDTXMania.app.Device, 1280, 720, CDTXMania.app.GraphicsDeviceManager.CurrentSettings.BackBufferFormat, Pool.Managed);

                for (int i = 0; i < 1; i++)
                {
                    this.stFillIn[i] = new STフィルイン();
                    this.stFillIn[i].ctUpdate = new CCounter(0, 30, 30, CDTXMania.Timer);
                    this.stFillIn[i].bInUse = false;
                }
                this.txフィルインエフェクト = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\7_Fillin Effect.png" ) );

                //this.txフィルインエフェクト = new CTexture[ 31 ];
                //for( int fill = 0; fill < 31; fill++ )
                //{
                //    this.txフィルインエフェクト[ fill ] = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\StageEffect\7_StageEffect_" + fill.ToString() + ".png" ) );
                //    if( this.txフィルインエフェクト[ fill ] == null )
                //        continue;

                //    this.txフィルインエフェクト[ fill ].bAdditiveBlending = true;
                //    this.txフィルインエフェクト[ fill ].vcScaleRatio = new Vector3( 2.0f, 2.0f, 1.0f );
                //}

                base.OnManagedCreateResources();
            }
        }
        public override void OnManagedReleaseResources()
        {
            if (!base.bNotActivated)
            {                
                //特殊テクスチャ 3枚
                if (this.tx描画用 != null)
                {
                    this.tx描画用.Dispose();
                    this.tx描画用 = null;
                }
                if (this.tx描画用2 != null)
                {
                    this.tx描画用2.Dispose();
                    this.tx描画用2 = null;
                }
                if (this.txDShow汎用 != null)
                {
                    this.txDShow汎用.Dispose();
                    this.txDShow汎用 = null;
                }
                if (this.txlanes != null)
                {
                    this.txlanes.Dispose();
                    this.txlanes = null;
                }
                //CDTXMania.t安全にDisposeする(ref this.ds汎用);
                //テクスチャ 17枚
                //CDTXMania.tReleaseTexture(ref this.txドラム);
                CDTXMania.tReleaseTexture(ref this.txクリップパネル);
                CDTXMania.tReleaseTexture( ref this.txフィルインエフェクト );
                //for( int ar = 0; ar < 31; ar++ )
                //{
                //    CDTXMania.tReleaseTexture( ref this.txフィルインエフェクト[ ar ] );
                //}
                base.OnManagedReleaseResources();
            }
        }
        public unsafe int tUpdateAndDraw(int x, int y)
        {   
            if ((!base.bNotActivated))
            {
                #region[ムービーのフレーム作成処理]
                if ( ( ( this.tx描画用 != null )) && ( this.rAVI != null ) ) //クリップ無し曲での進入防止。
                {
                    Rectangle rectangle;
                    Rectangle rectangle2;

                    #region[ frameNoFromTime ]
                    int time = (int)( ( CSoundManager.rcPerformanceTimer.nCurrentTime - this.n移動開始時刻ms ) * ( ( (double)CDTXMania.ConfigIni.nPlaySpeed ) / 20.0 ) );
                    int frameNoFromTime = 0;
                    if( this.bUseCAviDS )
                        frameNoFromTime = time;
                    //else
                    //    frameNoFromTime = this.rAVI.avi.GetFrameNoFromTime( time );
                    #endregion

                    if( ( this.n総移動時間ms != 0 ) && ( this.n総移動時間ms < time ) )
                    {
                        this.n総移動時間ms = 0;
                        this.n移動開始時刻ms = -1L;
                    }

                    //Loop
                    if (n総移動時間ms == 0 && time >= rAVI.avi.GetDuration())
                    {
                        if (!bIsPreviewMovie && !bLoop)
                        {
                            n移動開始時刻ms = -1L;
                            //return 0;
                        }
                        else 
                        {
                            n移動開始時刻ms = CSoundManager.rcPerformanceTimer.nCurrentTime;
                            time = (int)((CSoundManager.rcPerformanceTimer.nCurrentTime - this.n移動開始時刻ms) * (((double)CDTXMania.ConfigIni.nPlaySpeed) / 20.0));
                            rAVI.avi.Seek(0);
                        }
                        
                    }

                    if ((((this.n前回表示したフレーム番号 != frameNoFromTime) || !this.bフレームを作成した)) && ( fClipアスペクト比 < 1.77f ))
                    {
                        this.n前回表示したフレーム番号 = frameNoFromTime;
                        this.bフレームを作成した = true;
                    }
                    
                    Size size = new Size( (int)this.framewidth, (int)this.frameheight );
                    Size sz720pサイズ = new Size( 1280, 720);
                    Size sz開始サイズ = new Size( this.n開始サイズW, this.n開始サイズH );
                    Size sz終了サイズ = new Size( this.n終了サイズW, this.n終了サイズH );
                    Point location = new Point( this.n画像側開始位置X, this.n画像側終了位置Y );
                    Point point2 = new Point( this.n画像側終了位置X, this.n画像側終了位置Y );
                    Point point3 = new Point( this.n表示側開始位置X, this.n表示側開始位置Y );
                    Point point4 = new Point( this.n表示側終了位置X, this.n表示側終了位置Y );
                    if( CSoundManager.rcPerformanceTimer.nCurrentTime < this.n移動開始時刻ms )
                    {
                        this.n移動開始時刻ms = CSoundManager.rcPerformanceTimer.nCurrentTime;
                    }
                    if( this.n総移動時間ms == 0 )
                    {
                        rectangle = new Rectangle( location, sz開始サイズ );
                        rectangle2 = new Rectangle( point3, sz開始サイズ );
                    }
                    else
                    {
                        double db経過時間倍率 = ( (double)time ) / ( (double)this.n総移動時間ms );
                        Size size5 = new Size( sz開始サイズ.Width + ( (int)( ( sz終了サイズ.Width - sz開始サイズ.Width ) * db経過時間倍率 ) ), sz開始サイズ.Height + ( (int)( ( sz終了サイズ.Height - sz開始サイズ.Height ) * db経過時間倍率 ) ) );
                        rectangle = new Rectangle( (int)( (point2.X - location.X ) * db経過時間倍率 ), (int)( ( point2.Y - location.Y ) * db経過時間倍率 ), ( (int)( ( point2.X - location.X ) * db経過時間倍率 ) ) + size5.Width, ( (int)((point2.Y - location.Y) * db経過時間倍率 ) ) + size5.Height );
                        rectangle2 = new Rectangle( (int)( (point4.X - point3.X ) * db経過時間倍率 ), (int)( ( point4.Y - point3.Y ) * db経過時間倍率 ), ( (int)( (point4.X - point3.X ) * db経過時間倍率 ) ) + size5.Width, ( (int)( ( point4.Y - point3.Y ) * db経過時間倍率 ) ) + size5.Height );
                        if( rectangle.X < 0 )
                        {
                            int num6 = -rectangle.X;
                            rectangle2.X += num6;
                            rectangle2.Width -= num6;
                            rectangle.X = 0;
                            rectangle.Width -= num6;
                        }
                        if( rectangle.Y < 0 )
                        {
                            int num7 = -rectangle.Y;
                            rectangle2.Y += num7;
                            rectangle2.Height -= num7;
                            rectangle.Y = 0;
                            rectangle.Height -= num7;
                        }
                        if( rectangle.Right > size.Width )
                        {
                            int num8 = rectangle.Right - size.Width;
                            rectangle2.Width -= num8;
                            rectangle.Width -= num8;
                        }
                        if( rectangle.Bottom > size.Height )
                        {
                            int num9 = rectangle.Bottom - size.Height;
                            rectangle2.Height -= num9;
                            rectangle.Height -= num9;
                        }
                        if( rectangle2.X < 0 )
                        {
                            int num10 = -rectangle2.X;
                            rectangle.X += num10;
                            rectangle.Width -= num10;
                            rectangle2.X = 0;
                            rectangle2.Width -= num10;
                        }
                        if( rectangle2.Y < 0 )
                        {
                            int num11 = -rectangle2.Y;
                            rectangle.Y += num11;
                            rectangle.Height -= num11;
                            rectangle2.Y = 0;
                            rectangle2.Height -= num11;
                        }
                        if( rectangle2.Right > sz720pサイズ.Width )
                        {
                            int num12 = rectangle2.Right - sz720pサイズ.Width;
                            rectangle.Width -= num12;
                            rectangle2.Width -= num12;
                        }
                        if( rectangle2.Bottom > sz720pサイズ.Height )
                        {
                            int num13 = rectangle2.Bottom - sz720pサイズ.Height;
                            rectangle.Height -= num13;
                            rectangle2.Height -= num13;
                        }
                    }
                                        
                    if( this.bUseCAviDS  )
                    {
                        if( ( this.tx描画用 != null ) && ( this.n総移動時間ms != -1 ) )
                        {
                            #region[ フレームの生成 ]
                            this.rAVI.avi.tGetBitmap( CDTXMania.app.Device, this.tx描画用, time );
                            #endregion

                            if( this.bFullScreen )
                            {
                                #region[ 動画の描画 ]
                                if( fClipアスペクト比 > 1.77f )
                                {
                                    this.tx描画用.tDraw2DUpsideDown( CDTXMania.app.Device, this.position, 0 );
                                    //this.tx描画用.tDraw2D(CDTXMania.app.Device, this.position, 0);
                                }
                                else
                                {
                                    if( CDTXMania.ConfigIni.bDrumsEnabled )
                                    {
                                        this.tx描画用.vcScaleRatio = this.vclip;
                                        //this.tx描画用.tDraw2D( CDTXMania.app.Device, 882, 0 );
                                        this.tx描画用.tDraw2DUpsideDown( CDTXMania.app.Device, 882, 0 );
                                    }
                                    else if( CDTXMania.ConfigIni.bGuitarEnabled )
                                    {
                                        this.tx描画用.vcScaleRatio = new Vector3( 1f, 1f, 1f );
                                        this.PositionG = (int)( ( 1280f - (float)( this.framewidth ) ) / 2f);
                                        //this.tx描画用.tDraw2D( CDTXMania.app.Device, this.PositionG, 0 );
                                        this.tx描画用.tDraw2DUpsideDown( CDTXMania.app.Device, this.PositionG, 0);
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    
                }


                #endregion

                if (this.bIsDuringPerformance) 
                {
                    if (CDTXMania.DTX != null && CDTXMania.DTX.listBMP.Count >= 1 && CDTXMania.ConfigIni.bBGAEnabled == true)
                    {
                        if (CDTXMania.ConfigIni.bDrumsEnabled)
                            CDTXMania.stagePerfDrumsScreen.actBGA.tUpdateAndDraw(980, 0);
                        else
                            CDTXMania.stagePerfGuitarScreen.actBGA.tUpdateAndDraw(501, 0);
                    }
                }

                if( CDTXMania.ConfigIni.DisplayBonusEffects == true )
                {
                    for( int i = 0; i < 1; i++ )
                    {
                        if (this.stFillIn[ i ].bInUse)
                        {
                            int numf = this.stFillIn[ i ].ctUpdate.nCurrentValue;
                            this.stFillIn[ i ].ctUpdate.tUpdate();
                            if( this.stFillIn[ i ].ctUpdate.bReachedEndValue )
                            {
                                this.stFillIn[ i ].ctUpdate.tStop();
                                this.stFillIn[ i ].bInUse = false;
                            }
                            //if ( this.txフィルインエフェクト != null )
                            CStagePerfDrumsScreen stageDrum = CDTXMania.stagePerfDrumsScreen;
                            //CStagePerfGuitarScreen stageGuitar = CDTXMania.stagePerfGuitarScreen;

                            //if( ( CDTXMania.ConfigIni.bDrumsEnabled ? stageDrum.txBonusEffect : stageGuitar.txBonusEffect ) != null )
                            {
                                //this.txフィルインエフェクト.vcScaleRatio.X = 2.0f;
                                //this.txフィルインエフェクト.vcScaleRatio.Y = 2.0f;
                                //this.txフィルインエフェクト.bAdditiveBlending = true;
                                //this.txフィルインエフェクト.tDraw2D(CDTXMania.app.Device, 0, -2, new Rectangle(0, 0 + (360 * numf), 640, 360));
                                if( CDTXMania.ConfigIni.bDrumsEnabled && stageDrum.txBonusEffect != null)
                                {
                                    stageDrum.txBonusEffect.vcScaleRatio = new Vector3( 2.0f, 2.0f, 1.0f );
                                    stageDrum.txBonusEffect.bAdditiveBlending = true;
                                    stageDrum.txBonusEffect.tDraw2D( CDTXMania.app.Device, 0, -2, new Rectangle(0, 0 + ( 360 * numf ), 640, 360 )) ;
                                    try
                                    {
                                    //if( this.txフィルインエフェクト[ this.stFillIn[ i ].ctUpdate.nCurrentValue ] != null )
                                    //    this.txフィルインエフェクト[ this.stFillIn[ i ].ctUpdate.nCurrentValue ].tDraw2D( CDTXMania.app.Device, 0, 0 );
                                    }
                                    catch( Exception ex )
                                    {
                                    }
                                }
                            }

                        }
                    }
                }

                if (CDTXMania.ConfigIni.bShowMusicInfo && this.bIsDuringPerformance)
                    this.actPanel.tUpdateAndDraw();

                if( ( ( this.bWindowMode ) && this.tx描画用 != null && ( CDTXMania.ConfigIni.bAVIEnabled ) ) )
                {
                    this.vector = this.tx描画用.vcScaleRatio;
                    this.tx描画用.vcScaleRatio = this.smallvc;
                    this.tx描画用.nTransparency = 0xff;

                    if( CDTXMania.ConfigIni.bDrumsEnabled )
                    {
                        if( CDTXMania.ConfigIni.bGraph有効.Drums )
                        {
                            #region[ スキルメーター有効 ]
                            this.n本体X = 2;
                            this.n本体Y = 402;

                            if( this.fClipアスペクト比 > 0.96f )
                            {
                                this.ratio2 = 260f / ( (float)this.framewidth );
                                this.position2 = 20 + this.n本体Y + (int)( (270f - ( this.frameheight * this.ratio2 ) ) / 2f );
                            }
                            else
                            {
                                this.ratio2 = 270f / ( (float)this.frameheight );
                                this.position2 = 5 + this.n本体X + (int)( ( 260 - ( this.framewidth * this.ratio2 ) ) / 2f );
                            }
                            if( this.txクリップパネル != null )
                                this.txクリップパネル.tDraw2D( CDTXMania.app.Device, this.n本体X, this.n本体Y );

                            this.smallvc = new Vector3( this.ratio2, this.ratio2, 1f );
                                                        
                            {
                                if( this.n総移動時間ms != -1 && this.rAVI != null )
                                {
                                    if( this.fClipアスペクト比 < 0.96f )
                                        this.tx描画用.tDraw2DUpsideDown( CDTXMania.app.Device, this.position2, 20 + this.n本体Y );
                                    else
                                        this.tx描画用.tDraw2DUpsideDown( CDTXMania.app.Device, 5 + this.n本体X, this.position2 );
                                }
                            }
                        }
                        #endregion
                        else
                        {
                            #region[ スキルメーター無効 ]
                            this.n本体X = 854;
                            this.n本体Y = 142;

                            if( this.fClipアスペクト比 > 1.77f )
                            {
                                this.ratio2 = 416f / ((float)this.framewidth);
                                this.position2 = 30 + this.n本体Y + (int)((234f - (this.frameheight * this.ratio2)) / 2f);
                            }
                            else
                            {
                                this.ratio2 = 234f / ((float)this.frameheight);
                                this.position2 = 5 + this.n本体X + (int)((416f - (this.framewidth * this.ratio2)) / 2f);
                            }
                            if( this.txクリップパネル != null )
                                this.txクリップパネル.tDraw2D( CDTXMania.app.Device, this.n本体X, this.n本体Y ); 
                            this.smallvc = new Vector3( this.ratio2, this.ratio2, 1f );
                            this.tx描画用.vcScaleRatio = this.smallvc;
                            {
                                if( this.n総移動時間ms != -1 && this.rAVI != null )
                                {
                                    if( this.fClipアスペクト比 < 1.77f )
                                        this.tx描画用.tDraw2DUpsideDown( CDTXMania.app.Device, this.position2, 30 + this.n本体Y );
                                    else
                                        this.tx描画用.tDraw2DUpsideDown( CDTXMania.app.Device, 5 + this.n本体X, this.position2 );
                                }
                            }
                            #endregion
                        }
                    }
                    else if( CDTXMania.ConfigIni.bGuitarEnabled )
                    {
                        #region[ ギター時 ]
                        #region[ 本体位置 ]
                        this.n本体X = 380;
                        this.n本体Y = 50;
                        int nグラフX = 267;

                        if( CDTXMania.ConfigIni.bGraph有効.Bass && CDTXMania.DTX != null && !CDTXMania.DTX.bチップがある.Bass )
                            this.n本体X = this.n本体X + nグラフX;
                        if( CDTXMania.ConfigIni.bGraph有効.Guitar && CDTXMania.DTX != null && !CDTXMania.DTX.bチップがある.Guitar )
                            this.n本体X = this.n本体X - nグラフX;
                        #endregion

                        if( this.fClipアスペクト比 > 1.77f )
                        {
                            this.ratio2 = 460f / ( (float)this.framewidth );
                            this.position2 = 5 + this.n本体Y + (int)( ( 258f - ( this.frameheight * this.ratio2 ) ) / 2f );
                        }
                        else
                        {
                            this.ratio2 = 258f / ( (float)this.frameheight );
                            this.position2 = 30 + this.n本体X + (int)( ( 460f - ( this.framewidth * this.ratio2 ) ) / 2f );
                        }
                        if( this.txクリップパネル != null )
                            this.txクリップパネル.tDraw2D( CDTXMania.app.Device, this.n本体X, this.n本体Y );
                        this.smallvc = new Vector3( this.ratio2, this.ratio2, 1f );
                        this.tx描画用.vcScaleRatio = this.smallvc;
                        
                        {
                            if( this.rAVI != null )
                            {
                                if( this.fClipアスペクト比 < 1.77f )
                                    this.tx描画用.tDraw2D( CDTXMania.app.Device, this.position2, 5 + this.n本体Y );
                                else
                                    this.tx描画用.tDraw2D( CDTXMania.app.Device, 30 + this.n本体X, this.position2 );
                            }
                        }
                        #endregion
                    }
                    this.tx描画用.vcScaleRatio = this.vector;
                }
                IInputDevice keyboard = CDTXMania.InputManager.Keyboard;
                if( CDTXMania.Pad.bPressed( EInstrumentPart.BASS, EPad.Help ) )
                {
                    if( this.b再生トグル == false )
                    {                        
                        if( this.bUseCAviDS )
                        {
                            if(this.rAVI != null && this.rAVI.avi != null )
                            {
                                this.rAVI.avi.Pause();
                            }
                        }
                        this.b再生トグル = true;
                    }
                    else if( this.b再生トグル == true )
                    {
                        if( this.bUseCAviDS )
                        {
                            if(this.rAVI != null && this.rAVI.avi != null )
                            {
                                this.rAVI.avi.Run();
                            }
                        }
                        this.b再生トグル = false;
                    }
                }
            }

            return 0;
        }
        public void Start(bool bフィルイン)
        {
            for (int j = 0; j < 1; j++)
            {
                if (this.stFillIn[j].bInUse)
                {
                    this.stFillIn[j].ctUpdate.tStop();
                    this.stFillIn[j].bInUse = false;
                }
            }
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    if (!this.stFillIn[j].bInUse)
                    {
                        this.stFillIn[j].bInUse = true;
                        this.stFillIn[j].ctUpdate = new CCounter(0, 30, 30, CDTXMania.Timer);
                        break;
                    }
                }
            }
        }
        public override int OnUpdateAndDraw()
        {
            throw new InvalidOperationException("tUpdateAndDraw(int,int)のほうを使用してください。");
        }


        // Other

        #region [ private ]
        //-----------------
//      public CActPerfBGA actBGA;
        //public CActPerfDrumsFillingEffect actFill;
        public CActPerfPanelString actPanel;
        public bool bIsDuringPerformance = true;

        private bool bFullScreen;
//      private Bitmap blanes;
        public bool bWindowMode;
        private bool bフレームを作成した;
        private bool b再生トグル;
        private bool bDShowクリップを再生している;
        //private bool bUseMRenderer = false;
        private bool bUseCAviDS = true;//
        public float fClipアスペクト比;
        private uint frameheight;
        private uint framewidth;
        private int i1;
        private int i2;
//      private Image ilanes;
        private int nAlpha;
        private int nCurrentMovieMode;
        private long n移動開始時刻ms;
        private int n画像側開始位置X;
        private int n画像側開始位置Y;
        private int n画像側終了位置X;
        private int n画像側終了位置Y;
        private int n開始サイズH;
        private int n開始サイズW;
        private int n終了サイズH;
        private int n終了サイズW;
        private int n前回表示したフレーム番号;
        private int n総移動時間ms;
        private int n表示側開始位置X;
        private int n表示側開始位置Y;
        private int n表示側終了位置X;
        private int n表示側終了位置Y;
        private int n本体X;
        private int n本体Y;
        private int PositionG;
        private long lDshowPosition;
        private long lStopPosition;
        public IntPtr pBmp;
        private int position;
        private int position2;
        //NOTE: This is a soft reference to externally initialized object
        //Do not call Dispose() for rAVI
        private CDTX.CAVI rAVI;
        public bool bIsPreviewMovie { get; set; }
        public bool bHasBGA { get; set; }
        public bool bFullScreenMovie { get; set; }
        public bool bLoop { get; set; }
        //private CDirectShow ds汎用;

        //public CDTX.CDirectShow dsBGV;

        private CTexture txlanes;
        private CTexture txクリップパネル;
        //private CTexture txドラム;
        //private CTexture[] txフィルインエフェクト;
        private CTexture txフィルインエフェクト;
        private CTexture tx描画用;
        private CTexture tx描画用2;
        private CTexture txDShow汎用;

        private float ratio1;
        private float ratio2;
        private Rectangle rec;
        private Rectangle rec2;
        private Rectangle rec3;
        public Vector3 smallvc;
        private Vector3 vclip;
        public Vector3 vector;

        [StructLayout(LayoutKind.Sequential)]
        private struct STパッド状態
        {
            public int nY座標オフセットdot;
            public int nY座標加速度dot;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct STフィルイン
        {
            public bool bInUse;
            public CCounter ctUpdate;
        }
        private STパッド状態[] stパッド状態 = new STパッド状態[19];
        public STフィルイン[] stFillIn = new STフィルイン[2];
        //-----------------
        #endregion
    }
}
