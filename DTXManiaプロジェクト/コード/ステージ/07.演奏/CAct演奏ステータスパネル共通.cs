using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using FDK;

namespace DTXMania
{
    internal class CAct演奏ステータスパネル共通 : CActivity
    {
        // コンストラクタ
        public CAct演奏ステータスパネル共通()
        {
            this.stパネルマップ = null;
            this.stパネルマップ = new STATUSPANEL[12];		// yyagi: 以下、手抜きの初期化でスマン
            // { "DTXMANIA", 0 }, { "EXTREME", 1 }, ... みたいに書きたいが・・・

            //2013.09.07.kairera0467 画像の順番もこの並びになるので、難易度ラベルを追加する時は12以降に追加した方が画像編集でも助かります。
            string[] labels = new string[12] {
            "DTXMANIA",     //0
            "DEBUT",        //1
            "NOVICE",        //2
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
                this.stパネルマップ[i] = default(CAct演奏ステータスパネル共通.STATUSPANEL);
                //this.stパネルマップ[i] = new STATUSPANEL();
                this.stパネルマップ[i].status = status[i];
                this.stパネルマップ[i].label = labels[i];
            }
            base.b活性化してない = true;
        }


        // メソッド
        public void tラベル名からステータスパネルを決定する(string strラベル名)
        {
            if (string.IsNullOrEmpty(strラベル名))
            {
                this.nStatus = 0;
                this.nIndex = 0;
            }
            else
            {
                this.nIndex = 0;
                CAct演奏ステータスパネル共通.STATUSPANEL[] array = this.stパネルマップ;
                for (int i = 0; i < array.Length; i++)
                {
                    CAct演奏ステータスパネル共通.STATUSPANEL sTATUSPANEL = array[i];
                    if (strラベル名.Equals(sTATUSPANEL.label, StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.nStatus = sTATUSPANEL.status;
                        CDTXMania.nSongDifficulty = sTATUSPANEL.status;
                        CDTXMania.strSongDifficulyName = sTATUSPANEL.label;
                        return;
                    }
                    this.nIndex++;
                }
                this.nStatus = 0;
            }
        }

        // CActivity 実装

        public override void On活性化()
        {
            this.nCurrentScore = 0L;
            this.n現在のスコアGuitar = 0L;
            this.n現在のスコアBass = 0L;
            this.nStatus = 0;
            this.nIndex = 0;
            base.On活性化();
        }

        #region [ protected ]
        //-----------------
        [StructLayout(LayoutKind.Sequential)]
        public struct STATUSPANEL
        {
            public string label;
            public int status;
        }
        public long nCurrentScore;
        public long n現在のスコアGuitar;
        public long n現在のスコアBass;
        public int nIndex;
        public int nStatus;
        public STATUSPANEL[] stパネルマップ;
        //-----------------
        #endregion
    }
}
