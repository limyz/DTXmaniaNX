using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using SlimDX;
using FDK;

namespace DTXMania
{
    internal class CActResultImage : CActivity
    {
        // コンストラクタ

        public CActResultImage()
        {
            base.b活性化してない = true;
        }


        // メソッド

        public void tアニメを完了させる()
        {
            this.ct登場用.n現在の値 = this.ct登場用.n終了値;
        }


        // CActivity 実装

        public override void On活性化()
        {
            this.n本体X = 0x1d5;
            this.n本体Y = 0x11b;

            this.ftSongNameFont = new System.Drawing.Font("Impact", 24f, FontStyle.Regular, GraphicsUnit.Pixel);
            this.ftSongDifficultyFont = new System.Drawing.Font("Impact", 15f, FontStyle.Regular);
            this.ftSongNameFont = new System.Drawing.Font("ＤＦＧ平成ゴシック体W7", 21f, FontStyle.Regular, GraphicsUnit.Pixel);
            this.iDrumSpeed = Image.FromFile(CSkin.Path(@"Graphics\7_panel_icons.jpg"));
            this.txジャケットパネル = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_JacketPanel.png"));
            base.On活性化();

        }
        public override void On非活性化()
        {
            if (this.ct登場用 != null)
            {
                this.ct登場用 = null;
            }
            CDTXMania.tテクスチャの解放(ref this.txジャケットパネル);
            base.On非活性化();
        }
        public override void OnManagedリソースの作成()
        {
            if (!base.b活性化してない)
            {
                this.txリザルト画像がないときの画像 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\5_preimage default.png"));
                if (CDTXMania.ConfigIni.bストイックモード)
                {
                    this.txリザルト画像 = this.txリザルト画像がないときの画像;
                }
                else if (((!this.tリザルト画像の指定があれば構築する()) && (!this.tプレビュー画像の指定があれば構築する())))
                {
                    this.txリザルト画像 = this.txリザルト画像がないときの画像;
                }

                #region[ 曲名、アーティスト名テクスチャの生成 ]
                if (string.IsNullOrEmpty(CDTXMania.DTX.TITLE) || (!CDTXMania.bコンパクトモード && CDTXMania.ConfigIni.b曲名表示をdefのものにする))
                    this.strSongName = CDTXMania.stage選曲.r現在選択中の曲.strタイトル;
                else
                    this.strSongName = CDTXMania.DTX.TITLE;

                this.pfタイトル = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.str選曲リストフォント), 20, FontStyle.Regular);
                Bitmap bmpSongName = new Bitmap(1, 1);
                bmpSongName = pfタイトル.DrawPrivateFont(this.strSongName, CPrivateFont.DrawMode.Edge, Color.Black, Color.Black, this.clGITADORAgradationTopColor, this.clGITADORAgradationBottomColor, true);
                this.txSongName = CDTXMania.tテクスチャの生成(bmpSongName, false);
                bmpSongName.Dispose();

                this.pfアーティスト = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.str選曲リストフォント), 15, FontStyle.Regular);
                Bitmap bmpArtistName = new Bitmap(1, 1);
                bmpArtistName = pfアーティスト.DrawPrivateFont(CDTXMania.DTX.ARTIST, CPrivateFont.DrawMode.Edge, Color.Black, Color.Black, this.clGITADORAgradationTopColor, this.clGITADORAgradationBottomColor, true);
                this.txArtistName = CDTXMania.tテクスチャの生成(bmpArtistName, false);
                bmpArtistName.Dispose();
                #endregion

                Bitmap bitmap2 = new Bitmap(0x3a, 0x12);
                Graphics graphics = Graphics.FromImage(bitmap2);

                graphics.Dispose();
                this.txSongDifficulty = new CTexture(CDTXMania.app.Device, bitmap2, CDTXMania.TextureFormat, false);
                bitmap2.Dispose();
                Bitmap bitmap3 = new Bitmap(100, 100);
                graphics = Graphics.FromImage(bitmap3);
                float num;

                if (CDTXMania.ConfigIni.bCLASSIC譜面判別を有効にする && (CDTXMania.DTX.bチップがある.LeftCymbal == false) && (CDTXMania.DTX.bチップがある.LP == false) && (CDTXMania.DTX.bチップがある.LBD == false) && (CDTXMania.DTX.bチップがある.FT == false) && (CDTXMania.DTX.bチップがある.Ride == false))
                {
                    num = ((float)CDTXMania.stage選曲.r確定されたスコア.譜面情報.レベル.Drums);
                }
                else
                {
                    if (CDTXMania.stage選曲.r確定されたスコア.譜面情報.レベル.Drums > 100)
                    {
                        num = ((float)CDTXMania.stage選曲.r確定されたスコア.譜面情報.レベル.Drums);
                    }
                    else
                    {
                        num = ((float)CDTXMania.stage選曲.r確定されたスコア.譜面情報.レベル.Drums) / 10f;
                    }
                }

                if (CDTXMania.ConfigIni.bCLASSIC譜面判別を有効にする && (CDTXMania.DTX.bチップがある.LeftCymbal == false) && (CDTXMania.DTX.bチップがある.LP == false) && (CDTXMania.DTX.bチップがある.LBD == false) && (CDTXMania.DTX.bチップがある.FT == false) && (CDTXMania.DTX.bチップがある.Ride == false))
                {
                    graphics.DrawString(string.Format("{0:00}", num), this.ftSongDifficultyFont, new SolidBrush(Color.FromArgb(0xba, 0xba, 0xba)), (float)0f, (float)-4f);
                }
                else
                {
                    graphics.DrawString(string.Format("{0:0.00}", num), this.ftSongDifficultyFont, new SolidBrush(Color.FromArgb(0xba, 0xba, 0xba)), (float)0f, (float)-4f);
                }
                this.txSongLevel = new CTexture(CDTXMania.app.Device, bitmap3, CDTXMania.TextureFormat, false);
                graphics.Dispose();
                bitmap3.Dispose();
                Bitmap bitmap4 = new Bitmap(0x2a, 0x30);
                graphics = Graphics.FromImage(bitmap4);
                graphics.DrawImage(this.iDrumSpeed, new Rectangle(0, 0, 0x2a, 0x30), new Rectangle(0, CDTXMania.ConfigIni.n譜面スクロール速度.Drums * 0x30, 0x2a, 0x30), GraphicsUnit.Pixel);
                this.txDrumSpeed = new CTexture(CDTXMania.app.Device, bitmap4, CDTXMania.TextureFormat, false);
                graphics.Dispose();
                //graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                bitmap4.Dispose();
                base.OnManagedリソースの作成();
            }
        }
        public override void OnManagedリソースの解放()
        {
            if (!base.b活性化してない)
            {
                CDTXMania.tテクスチャの解放(ref this.txリザルト画像);
                CDTXMania.tテクスチャの解放(ref this.txリザルト画像がないときの画像);
                CDTXMania.tテクスチャの解放(ref this.txSongName);
                CDTXMania.tテクスチャの解放(ref this.txArtistName);
                CDTXMania.tテクスチャの解放(ref this.r表示するリザルト画像);
                CDTXMania.tテクスチャの解放(ref this.txSongLevel);
                CDTXMania.tテクスチャの解放(ref this.txSongDifficulty);
                CDTXMania.tテクスチャの解放(ref this.txDrumSpeed);

                CDTXMania.t安全にDisposeする( ref this.pfタイトル );
                CDTXMania.t安全にDisposeする( ref this.pfアーティスト );
                base.OnManagedリソースの解放();
            }
        }
        public override unsafe int On進行描画()
        {
            if (base.b活性化してない)
            {
                return 0;
            }
            if (base.b初めての進行描画)
            {
                this.ct登場用 = new CCounter(0, 100, 5, CDTXMania.Timer);
                base.b初めての進行描画 = false;
            }
            this.ct登場用.t進行();
            int x = this.n本体X;
            int y = this.n本体Y;
            this.txジャケットパネル.t2D描画(CDTXMania.app.Device, 467, 287);
            if (this.txリザルト画像 != null)
            {
                Matrix mat = Matrix.Identity;
                mat *= Matrix.Scaling(245.0f / this.txリザルト画像.sz画像サイズ.Width, 245.0f / this.txリザルト画像.sz画像サイズ.Height, 1f);
                mat *= Matrix.Translation(-28f, -94.5f, 0f);
                mat *= Matrix.RotationZ(0.3f);

                this.txリザルト画像.t3D描画(CDTXMania.app.Device, mat);
            }

            if (this.txSongName.sz画像サイズ.Width > 320)
                this.txSongName.vc拡大縮小倍率.X = 320f / this.txSongName.sz画像サイズ.Width;

            if (this.txArtistName.sz画像サイズ.Width > 320)
                this.txArtistName.vc拡大縮小倍率.X = 320f / this.txArtistName.sz画像サイズ.Width;

            this.txSongName.t2D描画(CDTXMania.app.Device, 500, 630);
            this.txArtistName.t2D描画(CDTXMania.app.Device, 500, 665);

            if (!this.ct登場用.b終了値に達した)
            {
                return 0;
            }
            return 1;
        }


        // その他

        #region [ private ]
        //-----------------
        private CCounter ct登場用;
        private System.Drawing.Font ftSongDifficultyFont;
        private System.Drawing.Font ftSongNameFont;
        private Image iDrumSpeed;
        private int n本体X;
        private int n本体Y;
        private CTexture r表示するリザルト画像;
        private string strSongName;
        private CTexture txDrumSpeed;
        private CTexture txSongDifficulty;
        private CTexture txSongLevel;
        private CTexture txリザルト画像;
        private CTexture txリザルト画像がないときの画像;
        private CTexture txジャケットパネル;

        private CTexture txSongName;
        private CTexture txArtistName;

        private CPrivateFastFont pfタイトル;
        private CPrivateFastFont pfアーティスト;

        //2014.04.05.kairera0467 GITADORAグラデーションの色。
        //本当は共通のクラスに設置してそれを参照する形にしたかったが、なかなかいいメソッドが無いため、とりあえず個別に設置。
        private Color clGITADORAgradationTopColor = Color.FromArgb(0, 220, 200);
        private Color clGITADORAgradationBottomColor = Color.FromArgb(255, 250, 40);

        private bool tプレビュー画像の指定があれば構築する()
        {
            if (string.IsNullOrEmpty(CDTXMania.DTX.PREIMAGE))
            {
                return false;
            }
            CDTXMania.tテクスチャの解放(ref this.txリザルト画像);
            this.r表示するリザルト画像 = null;
            string path = CDTXMania.DTX.strフォルダ名 + CDTXMania.DTX.PREIMAGE;
            if (!File.Exists(path))
            {
                Trace.TraceWarning("ファイルが存在しません。({0})", new object[] { path });
                return false;
            }
            this.txリザルト画像 = CDTXMania.tテクスチャの生成(path);
            this.r表示するリザルト画像 = this.txリザルト画像;
            return (this.r表示するリザルト画像 != null);
        }
        private bool tリザルト画像の指定があれば構築する()
        {
            int rank = CScoreIni.t総合ランク値を計算して返す(CDTXMania.stage結果.st演奏記録.Drums, CDTXMania.stage結果.st演奏記録.Guitar, CDTXMania.stage結果.st演奏記録.Bass);
            if (rank == 99)	// #23534 2010.10.28 yyagi: 演奏チップが0個のときは、rankEと見なす
            {
                rank = 6;
            }
            if (string.IsNullOrEmpty(CDTXMania.DTX.RESULTIMAGE[rank]))
            {
                return false;
            }
            CDTXMania.tテクスチャの解放(ref this.txリザルト画像);
            this.r表示するリザルト画像 = null;
            string path = CDTXMania.DTX.strフォルダ名 + CDTXMania.DTX.RESULTIMAGE[rank];
            if (!File.Exists(path))
            {
                Trace.TraceWarning("ファイルが存在しません。({0})", new object[] { path });
                return false;
            }
            this.txリザルト画像 = CDTXMania.tテクスチャの生成(path);
            this.r表示するリザルト画像 = this.txリザルト画像;
            return (this.r表示するリザルト画像 != null);
        }
        //-----------------
        #endregion
    }
}
