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
            this.tスクリプトから難易度ラベルを取得する( strラベル名 );

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

        public void tスクリプトから難易度ラベルを取得する( string strラベル名 )
        {
            string strRawScriptFile;

            //ファイルの存在チェック
            if( File.Exists( CSkin.Path( @"Script\difficult.dtxs" ) ) )
            {
                //スクリプトを開く
                StreamReader reader = new StreamReader( CSkin.Path( @"Script\difficult.dtxs" ), Encoding.GetEncoding( "Shift_JIS" ) );
                strRawScriptFile = reader.ReadToEnd();

                strRawScriptFile = strRawScriptFile.Replace( Environment.NewLine, "\n" );
                string[] delimiter = { "\n" };
                string[] strSingleLine = strRawScriptFile.Split( delimiter, StringSplitOptions.RemoveEmptyEntries );

                for( int i = 0; i < strSingleLine.Length; i++ )
                {
                    if( strSingleLine[ i ].StartsWith( "//" ) )
                        continue; //コメント行の場合は無視

                    //まずSplit
                    string[] arScriptLine = strSingleLine[ i ].Split( ',' );

                    if( ( arScriptLine.Length >= 4 && arScriptLine.Length <= 5 ) == false )
                        continue; //引数が4つか5つじゃなければ無視。

                    if( arScriptLine[ 0 ] != "7" )
                        continue; //使用するシーンが違うなら無視。

                    if( arScriptLine.Length == 4 )
                    {
                        if( String.Compare( arScriptLine[ 1 ], strラベル名, true ) != 0 )
                            continue; //ラベル名が違うなら無視。大文字小文字区別しない
                    }
                    else if( arScriptLine.Length == 5 )
                    {
                        if( arScriptLine[ 4 ] == "1" )
                        {
                            if( arScriptLine[ 1 ] != strラベル名 )
                                continue; //ラベル名が違うなら無視。
                        }
                        else
                        {
                            if( String.Compare( arScriptLine[ 1 ], strラベル名, true ) != 0 )
                                continue; //ラベル名が違うなら無視。大文字小文字区別しない
                        }
                    }
                    this.rectDiffPanelPoint.X = Convert.ToInt32( arScriptLine[ 2 ] );
                    this.rectDiffPanelPoint.Y = Convert.ToInt32( arScriptLine[ 3 ] );

                    reader.Close();
                    break;
                }
            }
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
        public STDGBVALUE<double> db現在の達成率;
        public int nIndex;
        public int nStatus;
        protected Rectangle rectDiffPanelPoint;
        public STATUSPANEL[] stパネルマップ;
        //-----------------
        #endregion
    }
}
