using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using SlimDX;
using SlimDX.Direct3D9;
using FDK;

namespace DTXMania
{
    internal class CActPerfDrumsPad : CActivity
    {
        // コンストラクタ

        public CActPerfDrumsPad()
        {
            ST基本位置[] st基本位置Array = new ST基本位置[10];

            //LC
            ST基本位置 st基本位置 = new ST基本位置();
            st基本位置.x = 263;
            st基本位置.y = 10;
            st基本位置.rc = new Rectangle(0, 0, 0x60, 0x60);
            st基本位置Array[0] = st基本位置;

            //HH
            ST基本位置 st基本位置2 = new ST基本位置();
            st基本位置2.x = 336;
            st基本位置2.y = 10;
            st基本位置2.rc = new Rectangle(0x60, 0, 0x60, 0x60);
            st基本位置Array[1] = st基本位置2;

            //SD
            ST基本位置 st基本位置3 = new ST基本位置();
            st基本位置3.x = 446;
            st基本位置3.y = 10;
            st基本位置3.rc = new Rectangle(0, 0x60, 0x60, 0x60);
            st基本位置Array[2] = st基本位置3;

            //BD
            ST基本位置 st基本位置4 = new ST基本位置();
            st基本位置4.x = 565;
            st基本位置4.y = 10;
            st基本位置4.rc = new Rectangle(0, 0xc0, 0x60, 0x60);
            st基本位置Array[3] = st基本位置4;

            //HT
            ST基本位置 st基本位置5 = new ST基本位置();
            st基本位置5.x = 510;
            st基本位置5.y = 10;
            st基本位置5.rc = new Rectangle(0x60, 0x60, 0x60, 0x60);
            st基本位置Array[4] = st基本位置5;

            //LT
            ST基本位置 st基本位置6 = new ST基本位置();
            st基本位置6.x = 622;
            st基本位置6.y = 10;
            st基本位置6.rc = new Rectangle(0xc0, 0x60, 0x60, 0x60);
            st基本位置Array[5] = st基本位置6;

            //FT
            ST基本位置 st基本位置7 = new ST基本位置();
            st基本位置7.x = 672;
            st基本位置7.y = 10;
            st基本位置7.rc = new Rectangle(288, 0x60, 0x60, 0x60);
            st基本位置Array[6] = st基本位置7;

            //CY
            ST基本位置 st基本位置8 = new ST基本位置();
            st基本位置8.x = 0x2df;
            st基本位置8.y = 10;
            st基本位置8.rc = new Rectangle(0xc0, 0, 0x60, 0x60);
            st基本位置Array[7] = st基本位置8;

            //RD
            ST基本位置 st基本位置9 = new ST基本位置();
            st基本位置9.x = 0x317;
            st基本位置9.y = 10;
            st基本位置9.rc = new Rectangle(288, 0, 0x60, 0x60);
            st基本位置Array[8] = st基本位置9;

            //LP
            ST基本位置 st基本位置10 = new ST基本位置();
            st基本位置10.x = 0x18c;
            st基本位置10.y = 10;
            st基本位置10.rc = new Rectangle(0x60, 0xc0, 0x60, 0x60);
            st基本位置Array[9] = st基本位置10;

            this.st基本位置 = st基本位置Array;
            base.bNotActivated = true;
        }


        // メソッド

        public void Hit(int nLane)
        {
            this.stパッド状態[nLane].n明るさ = 6;
            this.stパッド状態[nLane].nY座標加速度dot = 2;
            this.stパッド状態[nLane].nY座標加速度dot2 = 2;
        }

        public override void OnDeactivate()
        {

            base.OnDeactivate();
        }
        // CActivity 実装

        public override void OnActivate()
        {
            this.nフラッシュ制御タイマ = -1L;
            this.nY座標制御タイマ = -1L;
            for (int i = 0; i < 10; i++)
            {
                STパッド状態 stパッド状態2 = new STパッド状態();
                STパッド状態 stパッド状態 = stパッド状態2;
                stパッド状態.nY座標オフセットdot = 0;
                stパッド状態.nY座標加速度dot = 0;
                stパッド状態.nY座標オフセットdot2 = 0;
                stパッド状態.nY座標加速度dot2 = 0;
                stパッド状態.n明るさ = 0;
                this.stパッド状態[i] = stパッド状態;
            }
            base.OnActivate();
        }
        public override void OnManagedCreateResources()
        {
            if (!base.bNotActivated)
            {
                this.txパッド = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_pads.png"));
                this.tx光るパッド = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\ScreenPlayDrums pads flush.png"));
                this.txボーナス文字 = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_Bonus.png"));
                base.OnManagedCreateResources();
            }
        }
        public override void OnManagedReleaseResources()
        {
            if (!base.bNotActivated)
            {
                CDTXMania.tReleaseTexture(ref this.txパッド);
                CDTXMania.tReleaseTexture(ref this.tx光るパッド);
                CDTXMania.tReleaseTexture(ref this.txボーナス文字);
                base.OnManagedReleaseResources();
            }
        }
        public override int OnUpdateAndDraw()
        {
            if (!base.bNotActivated)
            {
                int num1 = CDTXMania.stagePerfDrumsScreen.ctBPMBar.nCurrentValue;


                if (base.bJustStartedUpdate)
                {
                    this.nフラッシュ制御タイマ = CDTXMania.Timer.nCurrentTime;
                    this.nY座標制御タイマ = CDTXMania.Timer.nCurrentTime;
                    base.bJustStartedUpdate = false;
                }
                long num = CDTXMania.Timer.nCurrentTime;
                if (num < this.nフラッシュ制御タイマ)
                {
                    this.nフラッシュ制御タイマ = num;
                }
                while ((num - this.nフラッシュ制御タイマ) >= 18L)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (this.stパッド状態[j].n明るさ > 0)
                        {
                            this.stパッド状態[j].n明るさ--;
                        }
                    }
                    this.nフラッシュ制御タイマ += 18L;
                }
                long num3 = CDTXMania.Timer.nCurrentTime;
                if (num3 < this.nY座標制御タイマ)
                {
                    this.nY座標制御タイマ = num3;
                }
                if (num3 < this.nY座標制御タイマ2)
                {
                    this.nY座標制御タイマ2 = num3;
                }
                while ((num3 - this.nY座標制御タイマ) >= 5L)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        this.stパッド状態[k].nY座標オフセットdot += this.stパッド状態[k].nY座標加速度dot;
                        if (this.stパッド状態[k].nY座標オフセットdot > 15)
                        {
                            this.stパッド状態[k].nY座標オフセットdot = 15;
                            this.stパッド状態[k].nY座標加速度dot = -1;
                        }
                        else if (this.stパッド状態[k].nY座標オフセットdot < 0)
                        {
                            this.stパッド状態[k].nY座標オフセットdot = 0;
                            this.stパッド状態[k].nY座標加速度dot = 0;
                        }

                        this.stパッド状態[k].nX座標オフセットdot += this.stパッド状態[k].nY座標加速度dot;
                        if (this.stパッド状態[k].nX座標オフセットdot > 6)
                        {
                            this.stパッド状態[k].nX座標オフセットdot = 6;
                            this.stパッド状態[k].nX座標加速度dot = -2;
                        }
                        else if (this.stパッド状態[k].nX座標オフセットdot < 0)
                        {
                            this.stパッド状態[k].nX座標オフセットdot = 0;
                            this.stパッド状態[k].nX座標加速度dot = 0;
                        }

                        this.stパッド状態[k].nX座標オフセットdotFLIP += this.stパッド状態[k].nY座標加速度dot;
                        if (this.stパッド状態[k].nX座標オフセットdotFLIP > 6)
                        {
                            this.stパッド状態[k].nX座標オフセットdotFLIP = 6;
                            this.stパッド状態[k].nX座標加速度dot = -2;
                        }
                        else if (this.stパッド状態[k].nX座標オフセットdotFLIP < 0)
                        {
                            this.stパッド状態[k].nX座標オフセットdotFLIP = 0;
                            this.stパッド状態[k].nX座標加速度dot = 0;
                        }

                        this.stパッド状態[k].nX座標オフセットdot2 += this.stパッド状態[k].nY座標加速度dot;
                        if (this.stパッド状態[k].nX座標オフセットdot2 > 3)
                        {
                            this.stパッド状態[k].nX座標オフセットdot2 = 3;
                            this.stパッド状態[k].nX座標加速度dot = -1;
                        }
                        else if (this.stパッド状態[k].nX座標オフセットdot2 < 0)
                        {
                            this.stパッド状態[k].nX座標オフセットdot2 = 0;
                            this.stパッド状態[k].nX座標加速度dot = 0;
                        }

                        this.stパッド状態[k].nX座標オフセットdot2FLIP += this.stパッド状態[k].nY座標加速度dot;
                        if (this.stパッド状態[k].nX座標オフセットdot2FLIP > 3)
                        {
                            this.stパッド状態[k].nX座標オフセットdot2FLIP = 3;
                            this.stパッド状態[k].nX座標加速度dot = -1;
                        }
                        else if (this.stパッド状態[k].nX座標オフセットdot2FLIP < 0)
                        {
                            this.stパッド状態[k].nX座標オフセットdot2FLIP = 0;
                            this.stパッド状態[k].nX座標加速度dot = 0;
                        }
                    }
                    this.nY座標制御タイマ += 6L;
                }
                while ((num3 - this.nY座標制御タイマ2) >= 6L)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        this.stパッド状態[k].nY座標オフセットdot2 += this.stパッド状態[k].nY座標加速度dot2;
                        if (this.stパッド状態[k].nY座標オフセットdot2 > 11)
                        {
                            this.stパッド状態[k].nY座標オフセットdot2 = 11;
                            this.stパッド状態[k].nY座標加速度dot2 = -1;
                        }
                        else if (this.stパッド状態[k].nY座標オフセットdot2 < 0)
                        {
                            this.stパッド状態[k].nY座標オフセットdot2 = 0;
                            this.stパッド状態[k].nY座標加速度dot2 = 0;
                        }
                    }
                    this.nY座標制御タイマ2 += 8L;
                }
                for (int i = 0; i < 10; i++)
                {
                    int index = this.n描画順[i];
                    int numOfLanesflagIndex = (int)CDTXMania.ConfigIni.eNumOfLanes.Drums;
                    //26072020: Check DrawFlags array before even compute positions for drawing
                    if (this.nDrawFlags[numOfLanesflagIndex, index] == 1)
                    {
                        #region[レーン切り替え]                    
                        int x = this.st基本位置[index].x;
                        int x2 = (this.st基本位置[index].x + (CDTXMania.ConfigIni.bReverse.Drums ? -10 : 32)) + this.stパッド状態[index].nX座標オフセットdot;
                        int x3 = (this.st基本位置[index].x + (CDTXMania.ConfigIni.bReverse.Drums ? -10 : 79)) - this.stパッド状態[index].nX座標オフセットdotFLIP;
                        int y = (this.st基本位置[index].y + (CDTXMania.ConfigIni.bReverse.Drums ? 60 : 560)) + this.stパッド状態[index].nY座標オフセットdot2;
                        int y2 = (this.st基本位置[index].y + (CDTXMania.ConfigIni.bReverse.Drums ? 0 : 482)) + this.stパッド状態[index].nY座標オフセットdot;
                        int yh = (this.st基本位置[index].y + (CDTXMania.ConfigIni.bReverse.Drums ? 0 : 495)) + this.stパッド状態[index].nY座標オフセットdot;
                        int yb = (this.st基本位置[index].y + (CDTXMania.ConfigIni.bReverse.Drums ? 0 : 570)) - this.stパッド状態[index].nY座標オフセットdot;
                        int yl = (this.st基本位置[index].y + (CDTXMania.ConfigIni.bReverse.Drums ? 0 : 486)) + this.stパッド状態[index].nY座標オフセットdot;
                        int yf = (this.st基本位置[index].y + (CDTXMania.ConfigIni.bReverse.Drums ? 0 : 470)) + this.stパッド状態[index].nY座標オフセットdot;

                        if ((index == 2) && ((CDTXMania.ConfigIni.eLaneType.Drums == EType.B) || CDTXMania.ConfigIni.eLaneType.Drums == EType.D))
                        {
                            x = this.st基本位置[9].x - 4;
                        }
                        if (index == 3)
                        {
                            if ((CDTXMania.ConfigIni.eLaneType.Drums == EType.B) || (CDTXMania.ConfigIni.eLaneType.Drums == EType.C))
                            {
                                x = this.st基本位置[4].x + 7;
                            }
                        }
                        if (index == 4)
                        {
                            if ((CDTXMania.ConfigIni.eLaneType.Drums == EType.B) || (CDTXMania.ConfigIni.eLaneType.Drums == EType.C))
                            {
                                x = this.st基本位置[3].x + 15;
                            }
                            else if (CDTXMania.ConfigIni.eLaneType.Drums == EType.D)
                            {
                                x = this.st基本位置[3].x - 108;
                            }
                        }
                        if (index == 9)
                        {
                            if (CDTXMania.ConfigIni.eLaneType.Drums == EType.B)
                            {
                                x = this.st基本位置[2].x + 10;
                            }
                            else if (CDTXMania.ConfigIni.eLaneType.Drums == EType.D)
                            {
                                x = this.st基本位置[2].x + 50;
                            }
                        }
                        if ((index == 5) && (CDTXMania.ConfigIni.eLaneType.Drums == EType.B))
                        {
                            x = this.st基本位置[5].x + 2;
                        }
                        if ((index == 8) && (CDTXMania.ConfigIni.eRDPosition == ERDPosition.RDRC))
                        {
                            x = this.st基本位置[7].x - 15;
                        }
                        if ((index == 7) && (CDTXMania.ConfigIni.eRDPosition == ERDPosition.RDRC))
                        {
                            x = this.st基本位置[8].x - 15;
                        }
                        if (this.txパッド != null)
                        {
                            this.txパッド.tDraw2D(CDTXMania.app.Device, x, y, this.st基本位置[index].rc);

                            //if (CDTXMania.ConfigIni.eNumOfLanes.Drums == EType.B && index == 8)
                            //{
                            //    // Do not draw RD if numOfLanes is 9
                            //}
                            //else
                            //{

                            //}

                        }
                        if (this.tx光るパッド != null)
                        {
                            this.tx光るパッド.nTransparency = (this.stパッド状態[index].n明るさ * 50) + 0;
                            this.tx光るパッド.tDraw2D(CDTXMania.app.Device, x, y, this.st基本位置[index].rc);

                            //if (CDTXMania.ConfigIni.eNumOfLanes.Drums == EType.B && index == 8)
                            //{
                            //    // Do not draw RD if numOfLanes is 9
                            //}
                            //else
                            //{

                            //}

                        }

                        #endregion
                    }

                }
                #region[ ボーナス表示 ]
                for (int i = 0; i < 4; i++)
                {
                    //アニメーションは仮のもの。後から強化する予定。
                    if (this.stボーナス[i].b使用中)
                    {
                        int numf = this.stボーナス[i].ct進行.nCurrentValue;
                        this.stボーナス[i].ct進行.tUpdate();
                        if (this.stボーナス[i].ct進行.bReachedEndValue)
                        {
                            this.stボーナス[i].ct進行.tStop();
                            this.stボーナス[i].b使用中 = false;
                        }
                        if (this.txボーナス文字 != null && CDTXMania.ConfigIni.bShowScore)
                        {
                            this.txボーナス文字.tDraw2D(CDTXMania.app.Device, 160, 80);
                            //if (this.stボーナス[i].ctUpdate.nCurrentValue >= 765)
                            //{
                            //    int n = this.stボーナス[i].ctUpdate.nCurrentValue - 765;
                            //    this.txボーナス文字.nTransparency = 255 - n;
                            //}
                        }
                    }
                }
                #endregion
            }
            return 0;
        }

        public void Start(int nLane, bool bボーナス)
        {
            for (int j = 0; j < 3; j++)
            {
                if (this.stボーナス[j].b使用中)
                {
                    this.stボーナス[j].ct進行.tStop();
                    this.stボーナス[j].b使用中 = false;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!this.stボーナス[j].b使用中)
                    {
                        this.stボーナス[j].b使用中 = true;
                        this.stボーナス[j].ct進行 = new CCounter(0, 1020, 1, CDTXMania.Timer);
                        break;
                    }
                }
            }
        }

        public void Start(int nLane, bool bボーナス, int n代入番号)
        {
            for (int j = 0; j < 4; j++)
            {
                if (this.stボーナス[j].b使用中)
                {
                    this.stボーナス[j].ct進行.tStop();
                    this.stボーナス[j].b使用中 = false;
                }
            }
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (!this.stボーナス[j].b使用中)
                    {
                        this.stボーナス[j].b使用中 = true;
                        this.stボーナス[j].ct進行 = new CCounter(0, 1020, 1, CDTXMania.Timer);
                    }

                }
            }
        }


        // Other

        #region [ private ]
        //-----------------
        [StructLayout(LayoutKind.Sequential)]
        public struct STパッド状態
        {
            public int n明るさ;
            public int nY座標オフセットdot;
            public int nY座標オフセットdot2;
            public int nX座標オフセットdot;
            public int nX座標オフセットdotFLIP;
            public int nX座標オフセットdot2;
            public int nX座標オフセットdot2FLIP;
            public int nY座標加速度dot;
            public int nY座標加速度dot2;
            public int nX座標加速度dot;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct ST基本位置
        {
            public int x;
            public int y;
            public Rectangle rc;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct STボーナス
        {
            public bool b使用中;
            public CCounter ct進行;
            public int x;
        }
        private long nY座標制御タイマ;
        private long nY座標制御タイマ2;
        private long nフラッシュ制御タイマ;
        private readonly int[] n描画順 = new int[] { 9, 3, 2, 6, 5, 4, 8, 7, 1, 0 };
                                                 // LP BD SD FT HT LT RD CY HH LC
        //26072020: New array Fisyher
        private readonly int[,] nDrawFlags = new int[3, 10] { { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1, 1, 1, 1, 0, 1 }, { 0, 1, 1, 1, 1, 1, 0, 1, 0, 0 } };
        public STパッド状態[] stパッド状態 = new STパッド状態[ 0x13 ];
		public readonly ST基本位置[] st基本位置;
		private CTexture txパッド;
		private CTexture tx光るパッド;
        private CTexture txボーナス文字;
        public bool[] bボーナス文字 = new bool[10];
        public STボーナス[] stボーナス = new STボーナス[4];
		//-----------------
		#endregion
	}
}
