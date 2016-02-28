using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SlimDX.Direct3D9;
using FDK;

namespace DTXMania
{
    internal class CActオプションパネル : CActivity
    {
        // CActivity 実装

        public override void On非活性化()
        {
            if (!base.b活性化してない)
            {
                CDTXMania.tテクスチャの解放(ref this.txオプションパネル);
                base.On非活性化();
            }
        }
        public override void OnManagedリソースの作成()
        {
            if (!base.b活性化してない)
            {
                this.txオプションパネル = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Screen option panels.png"), false);
                base.OnManagedリソースの作成();
            }
        }
        public override int On進行描画()
        {
            if (!base.b活性化してない)
            {
                Device device = CDTXMania.app.Device;
                CConfigIni configIni = CDTXMania.ConfigIni;
                if (this.txオプションパネル != null)
                {
                    int drums = configIni.n譜面スクロール速度.Drums;
                    if (drums > 15)
                    {
                        drums = 15;
                    }
                    this.txオプションパネル.t2D描画(device, 0x2e2, 14, this.rc譜面スピード[drums]);
                    drums = configIni.n譜面スクロール速度.Guitar;
                    if (drums > 15)
                    {
                        drums = 15;
                    }
                    this.txオプションパネル.t2D描画(device, 0x2e2, 0x20, this.rc譜面スピード[drums]);
                    drums = configIni.n譜面スクロール速度.Bass;
                    if (drums > 15)
                    {
                        drums = 15;
                    }
                    this.txオプションパネル.t2D描画(device, 0x2e2, 50, this.rc譜面スピード[drums]);
                    this.txオプションパネル.t2D描画(device, 0x312, 14, this.rcHS[(configIni.bHidden.Drums ? 1 : 0) + (configIni.bSudden.Drums ? 2 : 0)]);
                    this.txオプションパネル.t2D描画(device, 0x312, 0x20, this.rcHS[(configIni.bHidden.Guitar ? 1 : 0) + (configIni.bSudden.Guitar ? 2 : 0)]);
                    this.txオプションパネル.t2D描画(device, 0x312, 50, this.rcHS[(configIni.bHidden.Bass ? 1 : 0) + (configIni.bSudden.Bass ? 2 : 0)]);
                    this.txオプションパネル.t2D描画(device, 0x342, 14, this.rcDark[(int)configIni.eDark]);
                    this.txオプションパネル.t2D描画(device, 0x342, 0x20, this.rcDark[(int)configIni.eDark]);
                    this.txオプションパネル.t2D描画(device, 0x342, 50, this.rcDark[(int)configIni.eDark]);
                    this.txオプションパネル.t2D描画(device, 0x372, 14, this.rcReverse[configIni.bReverse.Drums ? 1 : 0]);
                    this.txオプションパネル.t2D描画(device, 0x372, 0x20, this.rcReverse[configIni.bReverse.Guitar ? 1 : 0]);
                    this.txオプションパネル.t2D描画(device, 0x372, 50, this.rcReverse[configIni.bReverse.Bass ? 1 : 0]);
                    this.txオプションパネル.t2D描画(device, 930, 14, this.rcPosition[(int)configIni.判定文字表示位置.Drums]);
                    this.txオプションパネル.t2D描画(device, 930, 0x20, this.rcPosition[(int)configIni.判定文字表示位置.Guitar]);
                    this.txオプションパネル.t2D描画(device, 930, 50, this.rcPosition[(int)configIni.判定文字表示位置.Bass]);
                    this.txオプションパネル.t2D描画(device, 0x3d2, 14, this.rcTight[configIni.bTight ? 1 : 0]);
                    this.txオプションパネル.t2D描画(device, 0x3d2, 0x20, this.rcRandom[(int)configIni.eRandom.Guitar]);
                    this.txオプションパネル.t2D描画(device, 0x3d2, 50, this.rcRandom[(int)configIni.eRandom.Bass]);
                    this.txオプションパネル.t2D描画(device, 0x402, 14, this.rcComboPos[(int)configIni.ドラムコンボ文字の表示位置]);
                    this.txオプションパネル.t2D描画(device, 0x402, 0x20, this.rcLight[configIni.bLight.Guitar ? 1 : 0]);
                    this.txオプションパネル.t2D描画(device, 0x402, 50, this.rcLight[configIni.bLight.Bass ? 1 : 0]);
                    this.txオプションパネル.t2D描画(device, 0x432, 0x20, this.rcLeft[configIni.bLeft.Guitar ? 1 : 0]);
                    this.txオプションパネル.t2D描画(device, 0x432, 50, this.rcLeft[configIni.bLeft.Bass ? 1 : 0]);
                }
            }
            return 0;
        }


        // その他

        #region [ private ]
        //-----------------
        private readonly Rectangle[] rcComboPos = new Rectangle[] { new Rectangle(0x60, 0x6c, 0x30, 0x12), new Rectangle(0x60, 90, 0x30, 0x12), new Rectangle(0x60, 0x48, 0x30, 0x12), new Rectangle(0x30, 0x6c, 0x30, 0x12) };
        private readonly Rectangle[] rcDark = new Rectangle[] { new Rectangle(0x30, 0, 0x30, 0x12), new Rectangle(0x30, 0x12, 0x30, 0x12), new Rectangle(0x30, 0x54, 0x30, 0x12) };
        private readonly Rectangle[] rcHS = new Rectangle[] { new Rectangle(0, 0, 0x30, 0x12), new Rectangle(0, 0x12, 0x30, 0x12), new Rectangle(0, 0x24, 0x30, 0x12), new Rectangle(0, 0x36, 0x30, 0x12) };
        private readonly Rectangle[] rcLeft = new Rectangle[] { new Rectangle(0xc0, 0x6c, 0x30, 0x12), new Rectangle(240, 0x6c, 0x30, 0x12) };
        private readonly Rectangle[] rcLight = new Rectangle[] { new Rectangle(240, 0x48, 0x30, 0x12), new Rectangle(240, 90, 0x30, 0x12) };
        private readonly Rectangle[] rcPosition = new Rectangle[] { new Rectangle(0, 0x48, 0x30, 0x12), new Rectangle(0, 90, 0x30, 0x12), new Rectangle(0, 0x6c, 0x30, 0x12) };
        private readonly Rectangle[] rcRandom = new Rectangle[] { new Rectangle(0x90, 0x48, 0x30, 0x12), new Rectangle(0x90, 90, 0x30, 0x12), new Rectangle(0x90, 0x6c, 0x30, 0x12), new Rectangle(0x90, 0x7e, 0x30, 0xb6) };
        private readonly Rectangle[] rcReverse = new Rectangle[] { new Rectangle(0x30, 0x24, 0x30, 0x12), new Rectangle(0x30, 0x36, 0x30, 0x12) };
        private readonly Rectangle[] rcTight = new Rectangle[] { new Rectangle(0xc0, 0x48, 0x30, 0x12), new Rectangle(0xc0, 90, 0x30, 0x12) };
        private readonly Rectangle[] rc譜面スピード = new Rectangle[] { new Rectangle(0x60, 0, 0x30, 0x12), new Rectangle(0x60, 0x12, 0x30, 0x12), new Rectangle(0x60, 0x24, 0x30, 0x12), new Rectangle(0x60, 0x36, 0x30, 0x12), new Rectangle(0x90, 0, 0x30, 0x12), new Rectangle(0x90, 0x12, 0x30, 0x12), new Rectangle(0x90, 0x24, 0x30, 0x12), new Rectangle(0x90, 0x36, 0x30, 0x12), new Rectangle(0xc0, 0, 0x30, 0x12), new Rectangle(0xc0, 0x12, 0x30, 0x12), new Rectangle(0xc0, 0x24, 0x30, 0x12), new Rectangle(0xc0, 0x36, 0x30, 0x12), new Rectangle(240, 0, 0x30, 0x12), new Rectangle(240, 0x12, 0x30, 0x12), new Rectangle(240, 0x24, 0x30, 0x12), new Rectangle(240, 0x36, 0x30, 0x12) };
        private CTexture txオプションパネル;
        //-----------------
        #endregion
    }
}
