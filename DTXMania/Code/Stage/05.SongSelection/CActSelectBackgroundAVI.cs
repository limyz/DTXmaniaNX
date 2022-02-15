using System;
using System.Diagnostics;
using SharpDX.Direct3D9;
using FDK;

namespace DTXMania
{
    internal class CActSelectBackgroundAVI : CActivity
    {
        public CActSelectBackgroundAVI()
        {
            this.bLoop = true;
            this.position = 0;

            base.bNotActivated = true;
        }

        public void Start(EChannel nチャンネル番号, CDTX.CAVI rAVI, int n総移動時間ms, int n移動開始時刻ms)
        {
            //2016.01.21 kairera0467 VfW時代のコードを除去+大改造
            Trace.TraceInformation("CActPerfAVI: Start(): " + rAVI.strファイル名);

            this.rAVI = rAVI;
            #region[ アスペクト比からどっちを使うか判別 ]
            // 旧DShowモードを使っていて、旧規格クリップだったら新DShowモードを使う。
            //if( CDTXMania.ConfigIni.bDirectShowMode == false )
            {
                this.fClipアスペクト比 = ((float)rAVI.avi.nフレーム幅 / (float)rAVI.avi.nフレーム高さ);
                
            }
            //else
            //{
            //    this.fClipアスペクト比 = ( (float)dsBGV.dshow.n幅px / (float)dsBGV.dshow.n高さpx );
            //    if( this.fClipアスペクト比 < 1.77f )
            //        this.bUseMRenderer = false;
            //}
            #endregion

            if (nチャンネル番号 == EChannel.Movie || nチャンネル番号 == EChannel.MovieFull)
            {                
                
                //CAviDS
                this.rAVI = rAVI;
                
                this.n総移動時間ms = n総移動時間ms;
                this.n移動開始時刻ms = (n移動開始時刻ms != -1) ? n移動開始時刻ms : CSoundManager.rcPerformanceTimer.nCurrentTime;

                if ((this.rAVI != null) && (this.rAVI.avi != null))
                {
                    //float f拡大率x;
                    //float f拡大率y;
                    this.framewidth = (uint)this.rAVI.avi.nフレーム幅;
                    this.frameheight = (uint)this.rAVI.avi.nフレーム高さ;
                    if (this.tx描画用 == null)
                    {
                        this.tx描画用 = new CTexture(CDTXMania.app.Device, (int)this.framewidth, (int)this.frameheight, CDTXMania.app.GraphicsDeviceManager.CurrentSettings.BackBufferFormat, Pool.Managed);
                    }

                    if (fClipアスペクト比 < 1.77f)
                    {
                        //旧規格クリップだった場合
                        float ratio1 = this.fullScreenWidthPx / ((float)this.frameheight);
                        this.position = (int)((this.fullScreenHeightPx - (this.framewidth * ratio1)) / 2f);
                        int num = (int)(this.framewidth * ratio1);
                        if (num <= 565)
                        {
                            this.position = 295 + ((int)((565f - (this.framewidth * ratio1)) / 2f));
                            
                        }
                        else
                        {
                            this.position = 295 - ((int)(((this.framewidth * ratio1) - 565f) / 2f));
                        }
                        this.tx描画用.vcScaleRatio.X = ratio1;
                        this.tx描画用.vcScaleRatio.Y = ratio1;
                    }
                    else
                    {
                        //ワイドクリップの処理
                        float ratio1 = this.fullScreenHeightPx / ((float)this.framewidth);
                        this.position = (int)((this.fullScreenWidthPx - (this.frameheight * ratio1)) / 2f);
                        this.tx描画用.vcScaleRatio.X = ratio1;
                        this.tx描画用.vcScaleRatio.Y = ratio1;
                    }                    
                    this.rAVI.avi.Run();
                }
            }
        }


        public void Stop()
        {
            Trace.TraceInformation("CActSelectBackgroundAVI: Stop()");
            if ((this.rAVI != null) && (this.rAVI.avi != null))
            {
                this.n移動開始時刻ms = -1;
            }
        }

        public void Cont(int n再開時刻ms)
        {
            if ((this.rAVI != null) && (this.rAVI.avi != null))
            {
                this.n移動開始時刻ms = n再開時刻ms;
            }
        }

        public int tUpdateAndDraw() 
        {
            if ((!base.bNotActivated))
            {
                #region[ムービーのフレーム作成処理]
                if (((this.tx描画用 != null)) && (this.rAVI != null)) //クリップ無し曲での進入防止。
                {
                    //Rectangle rectangle;
                    //Rectangle rectangle2;

                    #region[ frameNoFromTime ]
                    int time = (int)((CSoundManager.rcPerformanceTimer.nCurrentTime - this.n移動開始時刻ms) * (((double)CDTXMania.ConfigIni.nPlaySpeed) / 20.0));
                    int frameNoFromTime = time;
                    #endregion

                    if ((this.n総移動時間ms != 0) && (this.n総移動時間ms < time))
                    {
                        this.n総移動時間ms = 0;
                        this.n移動開始時刻ms = -1L;
                        //return 0;
                    }

                    //Loop
                    if (n総移動時間ms == 0 && time >= rAVI.avi.GetDuration())
                    {
                        if (!bLoop)
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

                    if ((this.tx描画用 != null) && (this.n総移動時間ms != -1))
                    {
                        #region[ フレームの生成 ]
                        this.rAVI.avi.tGetBitmap(CDTXMania.app.Device, this.tx描画用, time);
                        #endregion

                        #region[ 動画の描画 ]
                        //Background video is always full screen (1280 x 720)
                        this.tx描画用.tDraw2DUpsideDown(CDTXMania.app.Device, this.position, 0);                        
                        #endregion
                        
                    }
                                        
                    //if( !this.bUseMRenderer && !this.bUseCAviDS )
                    //{
                    //    this.pBmp = this.rAVI.avi.GetFramePtr( frameNoFromTime );
                    //    this.n前回表示したフレーム番号 = frameNoFromTime;
                    //    this.bフレームを作成した = true;
                    //}
                    
                    if (CSoundManager.rcPerformanceTimer.nCurrentTime < this.n移動開始時刻ms)
                    {
                        this.n移動開始時刻ms = CSoundManager.rcPerformanceTimer.nCurrentTime;
                    }
                }
                #endregion
            }


            return 0;
        }

        // CActivity 実装
        #region[CActivity 実装]
        public override void OnActivate()
        {
            this.n移動開始時刻ms = -1;
            base.OnActivate();
        }

        public override void OnDeactivate()
        {
            //if (this.dsBGV != null)
            //    this.dsBGV.Dispose();
            base.OnDeactivate();
        }

        public override void OnManagedCreateResources()
        {
            if (!base.bNotActivated)
            {
                base.OnManagedCreateResources();
            }
        }

        public override void OnManagedReleaseResources()
        {
            if (!base.bNotActivated)
            {
                if (this.tx描画用 != null)
                {
                    this.tx描画用.Dispose();
                    this.tx描画用 = null;
                }
                base.OnManagedReleaseResources();
            }
        }

        public override int OnUpdateAndDraw()
        {
            throw new InvalidOperationException("tUpdateAndDraw(int,int)のほうを使用してください。");
        }
        #endregion

        #region[Properties]
        //public bool bIsPreviewMovie { get; set; }
        public bool bLoop { get; set; }
        #endregion

        #region[Private]

        private long n移動開始時刻ms;
        private int n総移動時間ms;

        private CTexture tx描画用;
        private float fClipアスペクト比;
        private uint frameheight;
        private uint framewidth;
        private int position;
        //private int position2;
        //DTXNX is 1280 by 720
        private readonly float fullScreenHeightPx = 1280f;
        private readonly float fullScreenWidthPx = 720f;
        private CDTX.CAVI rAVI;
        
        #endregion

    }
}
