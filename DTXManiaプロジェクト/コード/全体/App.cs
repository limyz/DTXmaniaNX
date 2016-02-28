using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Security;
using System.Threading;
using SlimDX;
using SlimDX.Direct3D9;
using SlimDX.Windows;
using SlimDX.DirectInput;
using FDK;
using DirectShowLib;

namespace DTXMania
{
    class CApp : CApplicationForm
    {
        /// <summary>
        /// <para>CDTXMania.App.Window.Handle　を進行スレッドにて使用すると例外が出る。（コントロール作成スレッドではないため。）</para>
        /// <para>そのため、ウィンドウハンドルをここに控えておく。</para>
        /// </summary>
        public IntPtr hWnd
        {
            get;
            protected set;
        }

        // 以下の２つはユーザ別設定ウィンドウにて立てられるフラグ。
        public volatile bool bD3Dデバイスを変更する = false;
        public volatile bool bサウンドデバイスを変更する = false;


        // オーバーライドメソッド 

        /// <summary>
        /// <para>メインウィンドウの生成と各種初期化を行う。</para>
        /// <para>Direct3D の生成の後に呼び出される。</para>
        /// <para>エラー等でアプリを終了したい場合は例外を発生させ、正常に（無言で）終了したい場合は this.Window を null にして return すること。</para>
        /// </summary>
        protected override void On初期化()
        {
            #region [ プライマリアダプタのHALとフォーマットのチェックを行う。]
            //-----------------
            if (!this.Direct3D.CheckDeviceType(0, SlimDX.Direct3D9.DeviceType.Hardware, Format.X8R8G8B8, Format.X8R8G8B8, true) ||	// ウィンドウモード
                !this.Direct3D.CheckDeviceType(0, SlimDX.Direct3D9.DeviceType.Hardware, Format.X8R8G8B8, Format.X8R8G8B8, false))	// 全画面モード
            {
                string msg = "プライマリディスプレイアダプタが、本ソフトの動作に必要な機能を満たしていません。";
                MessageBox.Show(msg, "StrokeStyle<T> エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception(msg);
            }
            //-----------------
            #endregion
            #region [ Global.* を初期化する。--> コンストラクタへ移動 ]
            //-----------------
            //Global.t初期化( this );	
            //-----------------
            #endregion

            #region [ ウィンドウを生成する。]
            //-----------------
            this.hWnd = this.Window.Handle;
            //-----------------
            #endregion
        }

        /// <summary>
        /// <para>Direct3Dデバイス（this.Device）に対するデフォルト設定を行う。</para>
        /// <para>Direct3Dデバイスのリセット・変更・再作成時に呼び出される。</para>
        /// </summary>
        protected override void OnD3Dデバイスステータスの初期化()
        {

            float f視野角 = 45.0f;		// [度]																				// z（遠）

            D3D9Device.SetRenderState(RenderState.Lighting, false);
            D3D9Device.SetRenderState(RenderState.ZEnable, false);
            D3D9Device.SetRenderState(RenderState.AntialiasedLineEnable, false);
            D3D9Device.SetRenderState(RenderState.AlphaTestEnable, true);
            D3D9Device.SetRenderState(RenderState.AlphaRef, 10);
            D3D9Device.SetRenderState(RenderState.MultisampleAntialias, false);
            D3D9Device.SetRenderState<Compare>(RenderState.AlphaFunc, Compare.Greater);
            D3D9Device.SetRenderState(RenderState.AlphaBlendEnable, true);
            D3D9Device.SetRenderState<Blend>(RenderState.SourceBlend, Blend.SourceAlpha);
            D3D9Device.SetRenderState<Blend>(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
            D3D9Device.SetTextureStageState(0, TextureStage.AlphaOperation, TextureOperation.Modulate);
            D3D9Device.SetTextureStageState(0, TextureStage.AlphaArg1, 2);
            D3D9Device.SetTextureStageState(0, TextureStage.AlphaArg2, 1);
        }

        //------------

        /*
        /// <summary>
        /// <para>進行処理を行う。</para>
        /// <para>ロックを得た進行スレッドにより実行される。</para>
        /// </summary>
        protected override void On進行()
        {
            switch ( CDTXMania.r現在のステージ.eステージID )
            {

                case CStage.Eステージ.演奏:
                    {
                        break;
                    }
                #region [ default ]
                //-----------------
                default:
                    break;
                //-----------------
                #endregion
            }
        }
        */
        public Size LogicalDisplaySize = new Size( 1280, 720 );
        /// <summary>
        /// <para>描画処理を行う。</para>
        /// <para>ロックを得た描画スレッドにより実行される。</para>
        /// <para>BeginScene() と EndScene() の間に呼び出される。</para>
        /// <para>そのため、Direct3Dデバイスの変更を伴うような操作は行わないこと。</para>
        /// </summary>
        protected override void On描画()
        {
            switch (this.e現在の状態[THREAD_描画])
            {
                #region [ D3Dデバイスの変更 ]
                //-----------------
                case Eアプリ状態.D3Dデバイスの変更:
                    {
                        if (this.bD3Dデバイスを変更する)
                        {
                            var newSettings = this.currentD3DSettings.Clone();

                            this.tDirect3Dデバイスを生成・変更・リセットする(		// 例外はキャッチしない。準正常じゃなくて異常なので。
                                newSettings,
                                this.LogicalDisplaySize,
                                CApplicationForm.wsウィンドウスタイル,
                                CApplicationForm.ws全画面スタイル,
                                true);

                            this.bD3Dデバイスを変更する = false;
                        }
                    }
                    this.bPresent停止 = true;
                    this.t完了(THREAD_描画);
                    break;
                //-----------------
                #endregion
                #region [ default ]
                //-----------------
                default:
                    this.bPresent停止 = true;
                    this.t完了(THREAD_描画);
                    break;
                //-----------------
                #endregion
            }
        }

        /// <summary>
        /// <para>アプリケーション全体のフローについて、現在の状態を管理し、状態に応じて各スレッドに指示を出す。</para>
        /// </summary>
        protected override void Onフロー制御()
        {
            // アプリの状態遷移図にそってフローが流れるようプログラミングする。
            // lock してないことに注意。


        }


        #region [| フロー制御用 |]
        //-----------------
        public enum Eアプリ状態
        {
            待機,
            アプリ起動,
            ログイン名取得,
            ログイン,
            ステージ以外のトップレベルActivityの活性化,
            ログアウト,
            トップレベルActivityの非活性化,
            アプリ終了,

            タイトルステージ活性化,
            タイトルステージ進行描画,
            タイトルステージ非活性化,

            選曲ステージ活性化,
            選曲ステージ進行描画,
            選曲ステージ非活性化,
            ユーザ別設定,
            D3Dデバイスの変更,
            サウンドデバイスの変更,
            ユーザ別曲管理,
            入力割り当て,
            曲読み込み,

            演奏ステージ活性化,
            演奏ステージ進行描画,
            演奏ステージ非活性化,
            曲の停止と解放,

            クリアステージ活性化,
            クリアステージ進行描画,
            クリアステージ非活性化,

            結果ステージ活性化,
            結果ステージ進行描画,
            結果ステージ非活性化,

            プレイヤーモード時の演奏ステージの活性化,
            ウィンドウを前面へ,
        }

        /// <summary>
        /// 無理やり適合させるためNG、OKがSSTと逆になっています。
        /// 2013.02.13.kairera0467
        /// </summary>
        public enum E状態処理結果 : int
        {
            NG,
            OK,
            キャンセル,
            ステージ継続,
            ユーザ別設定開始,
            ユーザ別曲管理開始,
            入力割り当て開始,
            ログインユーザ変更,
            曲決定,
            終了,
            CLEAR,
            //STAGEFAILED,
            GUI割込・演奏停止,
            GUI割込・演奏開始,
        }
        public volatile bool bWM_CLOSEを受け取った = false;

        private const int THREAD_進行 = 0;
        private const int THREAD_描画 = 1;
        private const int THREAD_フロー制御 = 2;
        /// <summary>
        /// <para>現在のスレッドの状態。処理完了時に「待機」に設定される。</para>
        /// </summary>
        private volatile Eアプリ状態[] e現在の状態 = new Eアプリ状態[3] { Eアプリ状態.待機, Eアプリ状態.待機, Eアプリ状態.待機 };

        /// <summary>
        /// <para>進行・描画スレッドの処理が完了したら、このイベントを Set() する。</para>
        /// </summary>
        private ManualResetEvent[] ev状態処理完了通知 = new ManualResetEvent[2] { 
			new ManualResetEvent( false ),	// [THREAD_進行]
			new ManualResetEvent( false ),	// [THREAD_描画]
		};
        private class FlowThreadAbortException : Exception { };

        private void t遷移(Eアプリ状態 e次の状態)
        {
            lock (this.obj排他用)
            {
                if (this.bアプリケーションを終了する || this.bWindowClose済み)
                    throw new FlowThreadAbortException();	// スレッド終了。

                Trace.TraceInformation("フロー遷移：" + e次の状態);

                for (int i = 0; i < this.e現在の状態.Length; i++)
                    this.e現在の状態[i] = e次の状態;		// 状態遷移。

                for (int i = 0; i < this.ev状態処理完了通知.Length; i++)
                    this.ev状態処理完了通知[i].Reset();	// 完了フラグリセット。
            }

            // 各スレッドの状態処理が全部終わるまでロック。

            Debug.WriteLine(e次の状態 + ": 待ち開始。");
            ManualResetEvent.WaitAll(this.ev状態処理完了通知);
            Debug.WriteLine(e次の状態 + ": 待ち終了。");
        }
        private void t完了(int threadID)
        {
            this.e現在の状態[threadID] = Eアプリ状態.待機;
            this.ev状態処理完了通知[threadID].Set();
        }
        private void t全完了()
        {
            this.t完了(THREAD_進行);
            this.t完了(THREAD_描画);
        }
        //public int n進行描画の戻り値;
        //private void t現在のステージの進行()
        //{
        //
        //}
        #endregion
    }
}
