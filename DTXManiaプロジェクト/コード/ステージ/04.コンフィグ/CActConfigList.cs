using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Threading;
using SlimDX;
using FDK;

namespace DTXMania
{
    internal class CActConfigList : CActivity
    {
        // プロパティ

        public bool bIsKeyAssignSelected		// #24525 2011.3.15 yyagi
        {
            get
            {
                Eメニュー種別 e = this.eメニュー種別;
                if (e == Eメニュー種別.KeyAssignBass || e == Eメニュー種別.KeyAssignDrums ||
                    e == Eメニュー種別.KeyAssignGuitar || e == Eメニュー種別.KeyAssignSystem)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool bIsFocusingParameter		// #32059 2013.9.17 yyagi
        {
            get
            {
                return b要素値にフォーカス中;
            }
        }
        public bool b現在選択されている項目はReturnToMenuである
        {
            get
            {
                CItemBase currentItem = this.list項目リスト[this.n現在の選択項目];
                if (currentItem == this.iSystemReturnToMenu || currentItem == this.iDrumsReturnToMenu ||
                    currentItem == this.iGuitarReturnToMenu || currentItem == this.iBassReturnToMenu)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public CItemBase ib現在の選択項目
        {
            get
            {
                return this.list項目リスト[this.n現在の選択項目];
            }
        }
        public int n現在の選択項目;


        // メソッド
        #region [ t項目リストの設定・System() ]
        public void t項目リストの設定・System()
        {
            this.tConfigIniへ記録する();
            this.list項目リスト.Clear();

            // #27029 2012.1.5 from: 説明文は最大9行→13行に変更。
            //横は13文字が目安。

            this.iSystemReturnToMenu = new CItemBase("<< ReturnTo Menu", CItemBase.Eパネル種別.その他,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            this.list項目リスト.Add(this.iSystemReturnToMenu);

            this.iSystemReloadDTX = new CItemBase("Reload Songs", CItemBase.Eパネル種別.通常,
                "曲データの一覧情報を\n"+
                "取得し直します。",
                "Reload song data.");
            this.list項目リスト.Add(this.iSystemReloadDTX);

            int nDGmode = (CDTXMania.ConfigIni.bGuitar有効 ? 1 : 1) + (CDTXMania.ConfigIni.bDrums有効 ? 0 : 1) - 1;
            this.iSystemGRmode = new CItemList("Drums & GR ", CItemBase.Eパネル種別.通常, nDGmode,
                "使用楽器の選択：\nDrOnly: ドラムのみ有効にします。\nGROnly: ギター/ベースのみの専用画面を\n用います。",
                "DrOnly: Only Drums is available.\nGROnly: Only Guitar/Bass are available.\n You can play them in GR screen.",
                new string[] { "DrOnly", "GROnly" });
            this.list項目リスト.Add(this.iSystemGRmode);

            this.iSystemRisky = new CItemInteger("Risky", 0, 10, CDTXMania.ConfigIni.nRisky,
                "設定した回数分\n" +
                "ミスをすると、強制的に\n"+
                "STAGE FAILEDになります。",
                "Risky mode:\nSet over 1, in case you'd like to specify\n the number of Poor/Miss times to be\n FAILED.\nSet 0 to disable Risky mode.");
            this.list項目リスト.Add(this.iSystemRisky);

            this.iSystemMovieMode = new CItemList("Movie Mode", CItemBase.Eパネル種別.通常, CDTXMania.ConfigIni.nMovieMode,
                "Movie Mode:\n0 = 非表示\n1 = 全画面\n2 = ウインドウモード\n3 = 全画面&ウインドウ\n演奏中にF5キーで切り替え。",
                new string[] { "Off", "Full Screen", "Window Mode", "Both" });
            this.list項目リスト.Add(this.iSystemMovieMode);

            this.iSystemMovieAlpha = new CItemList("LaneAlpha", CItemBase.Eパネル種別.通常, CDTXMania.ConfigIni.nMovieAlpha,
                "レーンの透明度を指定します。\n0% が完全不透明で、\n100% が完全透明となります。",
                "The degree for transparing playing\n Movie.\n\n0%=completely transparent,\n100%=no transparency",
                new string[] { "0%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%" });
            this.list項目リスト.Add(this.iSystemMovieAlpha);

            this.iCommonPlaySpeed = new CItemInteger("PlaySpeed", 5, 40, CDTXMania.ConfigIni.n演奏速度,
                "曲の演奏速度を、速くしたり\n"+
                "遅くしたりすることができます。\n"+
                "※一部のサウンドカードでは、\n"+
                "正しく再生できない可能性が\n"+
                "あります。）",
                "It changes the song speed.\nFor example, you can play in half\n speed by setting PlaySpeed = 0.500\n for your practice.\nNote: It also changes the songs' pitch.");
            this.list項目リスト.Add(this.iCommonPlaySpeed);

            this.iSystemTimeStretch = new CItemToggle("TimeStretch", CDTXMania.ConfigIni.bTimeStretch,
                "演奏速度の変更方式:\n" +
                "ONにすると、\n"+
                "演奏速度の変更を、\n" +
                "周波数変更ではなく\n" +
                "タイムストレッチで行います。",
                "How to change the playing speed:\n" +
                "Turn ON to use time stretch\n" +
                "to change the play speed.");
            this.list項目リスト.Add(this.iSystemTimeStretch);

            this.iSystemSkillMode = new CItemList("SkillMode", CItemBase.Eパネル種別.通常, CDTXMania.ConfigIni.nSkillMode,
                "達成率、スコアの計算方法を変更します。\n" +
                "CLASSIC:V6までのスコア計算とV8までの\n" +
                "ランク計算です。\n" +
                "XG:XGシリーズの達成率計算とV7以降の\n" +
                "スコア計算です。",
                "",
                new string[] { "CLASSIC", "XG" });
            this.list項目リスト.Add(this.iSystemSkillMode);

            this.iSystemClassicNotes = new CItemToggle("CLASSIC Notes", CDTXMania.ConfigIni.bCLASSIC譜面判別を有効にする,
                "CLASSIC譜面の判別の有無を設定します。\n",
                "Set the presence or absence of a determination of CLASSIC Score.");
            this.list項目リスト.Add(this.iSystemClassicNotes);

            this.iSystemFullscreen = new CItemToggle("Fullscreen", CDTXMania.ConfigIni.b全画面モード,
                "画面モード設定：\n ON で全画面モード、\n OFF でウィンドウモードになります。",
                "Fullscreen mode or window mode.");
            this.list項目リスト.Add(this.iSystemFullscreen);

            this.iSystemStageFailed = new CItemToggle("StageFailed", CDTXMania.ConfigIni.bSTAGEFAILED有効,
                "ONにするとゲージが\n" +
                "なくなった時にSTAGE FAILED" +
                "となり演奏が中断されます。",
                "Turn OFF if you don't want to encount\n GAME OVER.");
            this.list項目リスト.Add(this.iSystemStageFailed);

            this.iSystemRandomFromSubBox = new CItemToggle("RandSubBox", CDTXMania.ConfigIni.bランダムセレクトで子BOXを検索対象とする,
                "子BOXをRANDOMの対象とする：\nON にすると、RANDOM SELECT 時に、\n子BOXも選択対象とします。",
                "Turn ON to use child BOX (subfolders)\n at RANDOM SELECT.");
            this.list項目リスト.Add(this.iSystemRandomFromSubBox);

            this.iSystemAdjustWaves = new CItemToggle("AdjustWaves", CDTXMania.ConfigIni.bWave再生位置自動調整機能有効,
                "サウンド再生位置自動補正：\n" +
                "ハードウェアやOSに起因する\n" +
                "サウンドのずれを補正します。\n" +
                "再生時間の長い曲で\n"+
                "効果があります。\n" +
                "※DirectSound使用時のみ有効です。",
                "Automatic wave playing position\n" +
                " adjustment feature. If you turn it ON,\n" +
                " it decrease the lag which comes from\n" +
                " the difference of hardware/OS.\n" +
                "Usually, you should turn it ON." +
                "\n" +
                "Note: This setting is effetive\n" +
                " only when DirectSound is used.");
            this.list項目リスト.Add(this.iSystemAdjustWaves);

            this.iSystemVSyncWait = new CItemToggle("VSyncWait", CDTXMania.ConfigIni.b垂直帰線待ちを行う,
                "垂直帰線同期：\n" +
                "画面の描画をディスプレイの\n" +
                "垂直帰線中に行なう場合には\n" +
                "ONを指定します。\n" + 
                "ONにすると、ガタつきのない\n" +
                "滑らかな画面描画が実現されます。",
                "Turn ON to wait VSync (Vertical\n Synchronizing signal) at every\n drawings. (so FPS becomes 60)\nIf you have enough CPU/GPU power,\n the scroll would become smooth.");
            this.list項目リスト.Add(this.iSystemVSyncWait);

            this.iSystemAVI = new CItemToggle("AVI", CDTXMania.ConfigIni.bAVI有効,
                "AVIの使用：\n動画(AVI)を再生可能にする場合に\nON にします。AVI の再生には、それ\nなりのマシンパワーが必要とされます。",
                "To use AVI playback or not.");
            this.list項目リスト.Add(this.iSystemAVI);

            this.iSystemDirectShowMode = new CItemToggle("DirectShowMode", CDTXMania.ConfigIni.bDirectShowMode,
                "このオプションが有効の場合、ワイドクリップ\n" +
                "の動画をDirectShowを使って描画します。\n" +
                "旧サイズのクリップは従来の方式で描画します。",
                "If this option is enabled,\n" +
                "Draw using the DirectShow the Wideclip.");
            this.list項目リスト.Add(this.iSystemDirectShowMode);

            this.iSystemBGA = new CItemToggle("BGA", CDTXMania.ConfigIni.bBGA有効,
                "BGAの使用：\n画像(BGA)を表示可能にする場合に\nON にします。BGA の再生には、それ\nなりのマシンパワーが必要とされます。",
                "To draw BGA (back ground animations)\n or not.");
            this.list項目リスト.Add(this.iSystemBGA);

            this.iSystemPreviewSoundWait = new CItemInteger("PreSoundWait", 0, 0x2710, CDTXMania.ConfigIni.n曲が選択されてからプレビュー音が鳴るまでのウェイトms,
                "カーソルが合わされてから\n"+
                "プレビュー音が鳴り始める\n"+
                "までの時間を指定します。\n"+
                "0～10000[ms]が指定可能です。",
                "Delay time(ms) to start playing preview\n sound in SELECT MUSIC screen.\nYou can specify from 0ms to 10000ms.");
            this.list項目リスト.Add(this.iSystemPreviewSoundWait);

            this.iSystemPreviewImageWait = new CItemInteger("PreImageWait", 0, 0x2710, CDTXMania.ConfigIni.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms,
                "カーソルが合わされてから\n"+
                "プレビュー画像が表示\n"+
                "されるまでの時間を\n"+
                "指定します。\n"+
                "0～10000[ms]が指定可能です。",
                "Delay time(ms) to show preview image\n in SELECT MUSIC screen.\nYou can specify from 0ms to 10000ms.");
            this.list項目リスト.Add(this.iSystemPreviewImageWait);

            this.iSystemDebugInfo = new CItemToggle("Debug Info", CDTXMania.ConfigIni.b演奏情報を表示する,
                "演奏情報の表示：\n" +
                "演奏中、BGA領域の下部に\n" +
                "演奏情報を表示します。\n" +
                "また、小節線の横に\n"+
                "小節番号が表示されるように\n"+
                "なります。",
                "To show song informations on playing\n BGA area. (FPS, BPM, total time etc)\nYou can ON/OFF the indications\n by pushing [Del] while playing drums,\n guitar or bass.");
            this.list項目リスト.Add(this.iSystemDebugInfo);

            this.iSystemBGAlpha = new CItemInteger("BG Alpha", 0, 0xff, CDTXMania.ConfigIni.n背景の透過度,
                "背景画像をDTXManiaの\n"+
                "背景画像と合成する際の\n"+
                "背景画像の透明度を\n"+
                "指定します。\n"+
                "255に近いほど、不透明\n"+
                "になります。",
                "The degree for transparing playing\n screen and wallpaper.\n\n0=completely transparent,\n255=no transparency");
            this.list項目リスト.Add(this.iSystemBGAlpha);

            this.iSystemBGMSound = new CItemToggle("BGM Sound", CDTXMania.ConfigIni.bBGM音を発声する,
                "OFFにするとBGMを再生しません。",
                "Turn OFF if you don't want to play\n BGM.");
            this.list項目リスト.Add(this.iSystemBGMSound);

            this.iSystemAudienceSound = new CItemToggle("Audience", CDTXMania.ConfigIni.b歓声を発声する,
                "OFFにすると歓声を再生しません。",
                "Turn ON if you want to be cheered\n at the end of fill-in zone or not.");
            this.list項目リスト.Add(this.iSystemAudienceSound);

            this.iSystemDamageLevel = new CItemList("DamageLevel", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eダメージレベル,
                "Miss時のゲージの減少度合い\n"+
                "を指定します。\n"+
                "Risky時は無効となります",
                "Damage level at missing (and\n recovering level) at playing.\nThis setting is ignored when Risky >= 1.",
                new string[] { "Small", "Normal", "Large" });
            this.list項目リスト.Add(this.iSystemDamageLevel);

            this.iSystemSaveScore = new CItemToggle("SaveScore", CDTXMania.ConfigIni.bScoreIniを出力する,
                "演奏記録の保存：\n"+
                "ONで演奏記録を.score.iniに\n"+
                "保存します。\n",
                "To save high-scores/skills, turn it ON.\nTurn OFF in case your song data are\n in read-only media (CD-ROM etc).\nNote that the score files also contain\n 'BGM Adjust' parameter. So if you\n want to keep adjusting parameter,\n you need to set SaveScore=ON.");
            this.list項目リスト.Add(this.iSystemSaveScore);

            this.iSystemChipVolume = new CItemInteger("ChipVolume", 0, 100, CDTXMania.ConfigIni.n手動再生音量,
                "打音の音量：\n入力に反応して再生される\nチップの音量を指定します。\n0 ～ 100 % の値が指定可能\nです。\n",
                "The volumes for chips you hit.\nYou can specify from 0 to 100%.");
            this.list項目リスト.Add(this.iSystemChipVolume);

            this.iSystemAutoChipVolume = new CItemInteger("AutoVolume", 0, 100, CDTXMania.ConfigIni.n自動再生音量,
                "自動再生音の音量：\n自動的に再生される\nチップの音量を指定します。\n0 ～ 100 % の値が指定可能\nです。\n",
                "The volumes for AUTO chips.\nYou can specify from 0 to 100%.");
            this.list項目リスト.Add(this.iSystemAutoChipVolume);

            /*
            this.iSystemStoicMode = new CItemToggle("StoicMode", CDTXMania.ConfigIni.bストイックモード,
                "ストイック（禁欲）モード：\n" +
                "以下をまとめて表示ON/OFFします。\n" +
                "・プレビュー画像/動画\n" +
                "・リザルト画像/動画\n" +
                "・NowLoading画像\n" +
                "・演奏画面の背景画像\n" +
                "・BGA 画像 / AVI 動画\n" +
                "・グラフ画像\n",
                "Turn ON to disable drawing\n * preview image / movie\n * result image / movie\n * nowloading image\n * wallpaper (in playing screen)\n * BGA / AVI (in playing screen)");
            this.list項目リスト.Add(this.iSystemStoicMode);
            */

            this.iSystemStageEffect = new CItemToggle("StageEffect", CDTXMania.ConfigIni.ボーナス演出を表示する,
                "OFFにすると、\n" +
                "ゲーム中の背景演出が\n" +
                "非表示になります。",
                "");
            this.list項目リスト.Add(this.iSystemStageEffect);

            this.iSystemShowLag = new CItemList("ShowLagTime", CItemBase.Eパネル種別.通常, CDTXMania.ConfigIni.nShowLagType,
                "ズレ時間表示：\n"+
                "ジャストタイミングからの\n"+
                "ズレ時間(ms)を表示します。\n"+
                "OFF: 表示しません。\n"+
                "ON: ズレ時間を表示します。\n"+
                "GREAT-: PERFECT以外の時\n"+
                "のみ表示します。",
                "About displaying the lag from\n the \"just timing\".\n  OFF: Don't show it.\n  ON: Show it.\n  GREAT-: Show it except you've\n  gotten PERFECT.",
                new string[] { "OFF", "ON", "GREAT-" });
            this.list項目リスト.Add(this.iSystemShowLag);

            this.iSystemShowLagColor = new CItemList("ShowLagTimeColor", CItemBase.Eパネル種別.通常, CDTXMania.ConfigIni.nShowLagTypeColor,
				"ズレ時間表示の表示色変更：\n  TYPE-A: 早ズレを赤、遅ズレを青で表示します。\n  TYPE-B: 早ズレを青、遅ズレを赤で表示します。",
				"ズレ時間表示の表示色変更：\n  TYPE-A: 早ズレを赤、遅ズレを青で表示します。\n  TYPE-B: 早ズレを青、遅ズレを赤で表示します。",
				new string[] { "TYPE-A", "TYPE-B" } );
			this.list項目リスト.Add( this.iSystemShowLagColor );

            this.iSystemAutoResultCapture = new CItemToggle("AutoSaveResult", CDTXMania.ConfigIni.bIsAutoResultCapture,
                "ONにすると、NewRecord時に\n"+
                "自動でリザルト画像を\n"+
                "曲データと同じフォルダに\n"+
                "保存します。",
                "AutoSaveResult:\nTurn ON to save your result screen\n image automatically when you get\n hiscore/hiskill.");
            this.list項目リスト.Add(this.iSystemAutoResultCapture);

            this.iSystemMusicNameDispDef = new CItemToggle("MusicNameDispDEF", CDTXMania.ConfigIni.b曲名表示をdefのものにする,
                "表示される曲名をdefのものにします。\n" +
                "ただし選曲画面の表示は、\n" +
                "defファイルの曲名が\n"+
                "優先されます。",
                "");
            this.list項目リスト.Add(this.iSystemMusicNameDispDef);

            this.iAutoAddGage = new CItemToggle("AutoAddGage", CDTXMania.ConfigIni.bAutoAddGage,
                "ONの場合、AUTO判定も\n"+
                "ゲージに加算されます。\n",
                "If ON, will be added to the judgment also gauge AUTO.\n" +
                "");
            this.list項目リスト.Add(this.iAutoAddGage);

            this.iSystemBufferedInput = new CItemToggle("BufferedInput", CDTXMania.ConfigIni.bバッファ入力を行う,
                "バッファ入力モード：\nON にすると、FPS を超える入力解像\n度を実現します。\nOFF にすると、入力解像度は FPS に\n等しくなります。",
                "To select joystick input method.\n\nON to use buffer input. No lost/lags.\nOFF to use realtime input. It may\n causes lost/lags for input.\n Moreover, input frequency is\n synchronized with FPS.");
            this.list項目リスト.Add(this.iSystemBufferedInput);

            this.iLogOutputLog = new CItemToggle("TraceLog", CDTXMania.ConfigIni.bログ出力,
                "Traceログ出力：\nDTXManiaLog.txt にログを出力します。\n変更した場合は、DTXMania の再起動\n後に有効となります。",
                "Turn ON to put debug log to\n DTXManiaLog.txt\nTo take it effective, you need to\n re-open DTXMania.");
            this.list項目リスト.Add(this.iLogOutputLog);

            // #24820 2013.1.3 yyagi
            this.iSystemSoundType = new CItemList("SoundType", CItemList.Eパネル種別.通常, CDTXMania.ConfigIni.nSoundDeviceType,
                "サウンド出力方式を選択\n"+
                "します。\n" +
                "WASAPIはVista以降、\n"+
                "ASIOは対応機器でのみ\n"+
                "使用可能です。\n" +
                "WASAPIかASIOを使うと、\n"+
                "遅延を少なくできます。\n",
                "Sound output type:\n" +
                "You can choose WASAPI, ASIO or DShow\n" +
                "(DirectShow).\n" +
                "WASAPI can use only after Vista.\n" +
                "ASIO can use on the\n" +
                "\"ASIO-supported\" sound device.\n" +
                "You should use WASAPI or ASIO\n" +
                "to decrease the sound lag.\n" +
                "Note: Exit CONFIGURATION to make\n" +
                "     the setting take effect.",
                new string[] { "DSound", "ASIO", "WASAPI" });
            this.list項目リスト.Add(this.iSystemSoundType);

            // #24820 2013.1.15 yyagi
            this.iSystemWASAPIBufferSizeMs = new CItemInteger("WASAPIBufSize", 0, 99999, CDTXMania.ConfigIni.nWASAPIBufferSizeMs,
                "WASAPI時のバッファサイズ:\n" +
                "0～99999msを指定できます。\n" +
                "0を指定すると、OSがサイズを\n" +
                "自動設定します。\n" +
                "値を小さくするほどラグが減少\n" +
                "しますが、音割れや異常を\n" +
                "引き起こす場合があります。\n",
                "Sound buffer size for WASAPI:\n" +
                "You can set from 0 to 99999ms.\n" +
                "Set 0 to use a default sysytem\n" +
                "buffer size.\n" +
                "Smaller value makes smaller lag,\n" +
                "but it may cause sound troubles.\n" +
                "\n" +
                "Note: Exit CONFIGURATION to make\n" +
                "     the setting take effect.");
            this.list項目リスト.Add(this.iSystemWASAPIBufferSizeMs);

            // #24820 2013.1.17 yyagi
            string[] asiodevs = CEnumerateAllAsioDevices.GetAllASIODevices();
            this.iSystemASIODevice = new CItemList("ASIO device", CItemList.Eパネル種別.通常, CDTXMania.ConfigIni.nASIODevice,
                "ASIOデバイス:\n" +
                "ASIO使用時の\n" +
                "サウンドデバイスを選択\n"+
                "します。\n",
                "ASIO Sound Device:\n" +
                "Select the sound device to use\n" +
                "under ASIO mode.\n" +
                "\n" +
                "Note: Exit CONFIGURATION to make\n" +
                "     the setting take effect.",
                asiodevs);
            this.list項目リスト.Add(this.iSystemASIODevice);
            // #24820 2013.1.3 yyagi

            /*
            this.iSystemASIOBufferSizeMs = new CItemInteger("ASIOBuffSize", 0, 99999, CDTXMania.ConfigIni.nASIOBufferSizeMs,
                "ASIO使用時のバッファサイズ:\n" +
                "0～99999ms を指定可能です。\n" +
                "0を指定すると、サウンドデバイスに\n" +
                "指定されている設定値を使用します。\n" +
                "値を小さくするほど発音ラグが\n" +
                "減少しますが、音割れや異常動作を\n" +
                "引き起こす場合があります。\n"+
                "※ 設定はCONFIGURATION画面の\n" +
                "　終了時に有効になります。",
                "Sound buffer size for ASIO:\n" +
                "You can set from 0 to 99999ms.\n" +
                "Set 0 to use a default value already\n" +
                "specified to the sound device.\n" +
                "Smaller value makes smaller lag,\n" +
                "but it may cause sound troubles.\n" +
                "\n" +
                "Note: Exit CONFIGURATION to make\n" +
                " the setting take effect.");
            this.list項目リスト.Add(this.iSystemASIOBufferSizeMs);
            */

            #region [ GDオプション ]
            
            this.iSystemDifficlty = new CItemToggle("Difficlty", CDTXMania.ConfigIni.b難易度表示をXG表示にする,
                "選曲画面での難易度表示方法を変更します。\nON でXG風3ケタ、\nOFF で従来の2ケタ表示になります。",
                "");
            this.list項目リスト.Add(this.iSystemDifficlty);
            
            this.iSystemShowScore = new CItemToggle("ShowScore", CDTXMania.ConfigIni.bShowScore,
                    "演奏中のスコアの表示の有無を設定します。",
                    "");
            this.list項目リスト.Add(this.iSystemShowScore);

            this.iSystemShowMusicInfo = new CItemToggle("ShowMusicInfo", CDTXMania.ConfigIni.bShowMusicInfo,
                    "OFFにすると演奏中のジャケット、曲情報を\n表示しません。",
                    "");
            this.list項目リスト.Add(this.iSystemShowMusicInfo);
             
            #endregion

            this.iSystemSkinSubfolder = new CItemList("Skin (General)", CItemBase.Eパネル種別.通常, nSkinIndex,
                "スキン切替：スキンを切り替えます。\n" +
                "\n",
                "Skin:Change skin.\n" +
                "",
                skinNames);
            this.list項目リスト.Add(this.iSystemSkinSubfolder);

            this.iSystemUseBoxDefSkin = new CItemToggle("Skin (Box)", CDTXMania.ConfigIni.bUseBoxDefSkin,
                "Music boxスキンの利用：\n" +
                "特別なスキンが設定されたMusic box\n" +
                "に出入りしたときに、自動でスキンを\n" +
                "切り替えるかどうかを設定します。\n",
                "Box skin:\n" +
                "Automatically change skin\n" +
                "specified in box.def file.");
            this.list項目リスト.Add(this.iSystemUseBoxDefSkin);

            this.iInfoType = new CItemList("InfoType", CItemBase.Eパネル種別.通常, CDTXMania.ConfigIni.nInfoType,
                "Helpボタンを押した時に出る\n" +
                "情報表示を変更できます。\n" +
                "Type-A FPS、BGMアジャスト\n" +
                "などの情報が出ます。\n" +
                "Type-B 判定数などが出ます。\n",
                "The display position for Drums Combo.\n" +
                "Note that it doesn't take effect\n" +
                " at Autoplay ([Left] is forcely used).",
                new string[] { "Type-A", "Type-B" });
            this.list項目リスト.Add(this.iInfoType);

            // #36372 2016.06.19 kairera0467
			this.iSystemBGMAdjust = new CItemInteger( "BGMAdjust", -99, 99, CDTXMania.ConfigIni.nCommonBGMAdjustMs,
				"BGMの再生タイミングの微調整を行います。\n" +
				"-99 ～ 99ms まで指定可能です。\n" +
                "値を指定してください。\n",
				"To adjust the BGM play timing.\n" +
				"You can set from -99 to 0ms.\n" );
			this.list項目リスト.Add( this.iSystemBGMAdjust );

            this.iSystemGoToKeyAssign = new CItemBase("System Keys", CItemBase.Eパネル種別.通常,
                "システムのキー入力に関する項目を設定します。",
                "Settings for the system key/pad inputs.");
            this.list項目リスト.Add(this.iSystemGoToKeyAssign);

            OnListMenuの初期化();
            this.n現在の選択項目 = 0;
            this.eメニュー種別 = Eメニュー種別.System;
        }
        #endregion
        #region [ t項目リストの設定・Drums() ]
        public void t項目リストの設定・Drums()
        {
            this.tConfigIniへ記録する();
            this.list項目リスト.Clear();

            // #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

            this.iDrumsReturnToMenu = new CItemBase("<< Return To Menu", CItemBase.Eパネル種別.その他,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            this.list項目リスト.Add(this.iDrumsReturnToMenu);

            //----------AutoPlay----------
            this.iDrumsAutoPlayAll = new CItemThreeState("AutoPlay (All)", CItemThreeState.E状態.不定,
                "全パッドの自動演奏のON/OFFを\n" +
                "まとめて切り替えます。",
                "You can change whether Auto or not\n" +
                " for all drums lanes at once.");
            this.list項目リスト.Add(this.iDrumsAutoPlayAll);

            this.iDrumsLeftCymbal = new CItemToggle("    LeftCymbal", CDTXMania.ConfigIni.bAutoPlay.LC,
                "左シンバルを自動で演奏します。",
                "To play LeftCymbal automatically.");
            this.list項目リスト.Add(this.iDrumsLeftCymbal);

            this.iDrumsHiHat = new CItemToggle("    HiHat", CDTXMania.ConfigIni.bAutoPlay.HH,
                "ハイハットを自動で演奏します。\n" +
                "（クローズ、オープンとも）",
                "To play HiHat automatically.\n" +
                "(It effects to both HH-close and\n HH-open)");
            this.list項目リスト.Add(this.iDrumsHiHat);

            this.iDrumsLeftPedal = new CItemToggle("    LeftPedal", CDTXMania.ConfigIni.bAutoPlay.LP,
                "左ペダルを自動で演奏します。",
                "To play Left Pedal automatically.");
            this.list項目リスト.Add(this.iDrumsLeftPedal);

            this.iDrumsLeftBassDrum = new CItemToggle("    LBassDrum", CDTXMania.ConfigIni.bAutoPlay.LBD,
                "左バスドラムを自動で演奏します。",
                "To play Left Pedal automatically.");
            this.list項目リスト.Add(this.iDrumsLeftBassDrum);

            this.iDrumsSnare = new CItemToggle("    Snare", CDTXMania.ConfigIni.bAutoPlay.SD,
                "スネアを自動で演奏します。",
                "To play Snare automatically.");
            this.list項目リスト.Add(this.iDrumsSnare);

            this.iDrumsBass = new CItemToggle("    BassDrum", CDTXMania.ConfigIni.bAutoPlay.BD,
                "バスドラムを自動で演奏します。",
                "To play Bass Drum automatically.");
            this.list項目リスト.Add(this.iDrumsBass);

            this.iDrumsHighTom = new CItemToggle("    HighTom", CDTXMania.ConfigIni.bAutoPlay.HT,
                "ハイタムを自動で演奏します。",
                "To play High Tom automatically.");
            this.list項目リスト.Add(this.iDrumsHighTom);

            this.iDrumsLowTom = new CItemToggle("    LowTom", CDTXMania.ConfigIni.bAutoPlay.LT,
                "ロータムを自動で演奏します。",
                "To play Low Tom automatically.");
            this.list項目リスト.Add(this.iDrumsLowTom);

            this.iDrumsFloorTom = new CItemToggle("    FloorTom", CDTXMania.ConfigIni.bAutoPlay.FT,
                "フロアタムを自動で演奏します。",
                "To play Floor Tom automatically.");
            this.list項目リスト.Add(this.iDrumsFloorTom);

            this.iDrumsCymbal = new CItemToggle("    Cymbal", CDTXMania.ConfigIni.bAutoPlay.CY,
                "右シンバルを自動で演奏します。",
                "To play both right- and Ride-Cymbal\n" +
                " automatically.");
            this.list項目リスト.Add(this.iDrumsCymbal);

            this.iDrumsRide = new CItemToggle("    Ride", CDTXMania.ConfigIni.bAutoPlay.RD,
                "ライドシンバルを自動で演奏します。",
                "To play both right- and Ride-Cymbal\n" +
                " automatically.");
            this.list項目リスト.Add(this.iDrumsRide);

            //----------StandardOption----------

            this.iDrumsScrollSpeed = new CItemInteger("ScrollSpeed", 0, 0x7cf, CDTXMania.ConfigIni.n譜面スクロール速度.Drums,
                "ノーツの流れるスピードを\n"+
                "変更します。\n" +
                "数字が大きくなるほど\n" +
                "スピードが速くなり、\n"+
                "ノーツの間隔が広がります。",
                "To change the scroll speed for the\n" +
                "drums lanes.\n" +
                "You can set it from x0.5 to x1000.0.\n" +
                "(ScrollSpeed=x0.5 means half speed)");
            this.list項目リスト.Add(this.iDrumsScrollSpeed);

            this.iDrumsHIDSUD = new CItemList("HID-SUD", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.nHidSud.Drums,
                "HIDDEN:チップが途中から見えなくなります。\n" +
                "SUDDEN:チップが途中まで見えません。\n" +
                "HID-SUD:HIDDEN、SUDDEN\n" +
                "の両方が適用されます。\n" +
                "STEALTH:チップがずっと表示されません。",
                "The display position for Drums Combo.\n" +
                "Note that it doesn't take effect\n" +
                " at Autoplay ([Left] is forcely used).",
                new string[] { "OFF", "Hidden", "Sudden", "HidSud", "Stealth" });
            this.list項目リスト.Add(this.iDrumsHIDSUD);

            //----------DisplayOption----------

            this.iDrumsDark = new CItemList("       Dark", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eDark,
                "レーン表示のオプションを\n"+
                "まとめて切り替えます。\n" +
                "HALF:レーンが表示されなく\n"+
                "なります。\n" +
                "FULL:さらに小節線、拍線、\n"+
                "判定ラインも表示されなくなります。",
                "OFF: all display parts are shown.\nHALF: lanes and gauge are\n disappeared.\nFULL: additionaly to HALF, bar/beat\n lines, hit bar are disappeared.",
                new string[] { "OFF", "HALF", "FULL" });
            this.list項目リスト.Add(this.iDrumsDark);

            this.iDrumsLaneDisp = new CItemList("LaneDisp", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.nLaneDisp.Drums,
                "レーンの縦線と小節線の表示を切り替えます。\n" +
                "ALL  ON :全て表示します。\n" +
                "LANE OFF:レーン背景を表示しません。\n" +
                "LINE OFF:小節線を表示しません。\n" +
                "ALL  OFF:全て表示しません。",
                "",
                new string[] { "ALL ON", "LANE OFF", "LINE OFF", "ALL OFF" });
            this.list項目リスト.Add(this.iDrumsLaneDisp);

            this.iDrumsJudgeLineDisp = new CItemToggle("JudgeLineDisp", CDTXMania.ConfigIni.bJudgeLineDisp.Drums,
                "判定ラインの表示 / 非表示を切り替えます。",
                "Toggle JudgeLine");
            this.list項目リスト.Add(this.iDrumsJudgeLineDisp);

            this.iDrumsLaneFlush = new CItemToggle("LaneFlush", CDTXMania.ConfigIni.bLaneFlush.Drums,
                "レーンフラッシュの表示 / 非表示を\n" +
                 "切り替えます。",
                "Toggle LaneFlush");
            this.list項目リスト.Add(this.iDrumsLaneFlush);

            this.iDrumsAttackEffect = new CItemList("AttackEffect", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eAttackEffect.Drums,
                "アタックエフェクトの表示方法を設定します。\n" +
                "ALL ON: すべて表示\n" +
                "ChipOFF: チップエフェクトのみ消す\n" +
                "EffectOnly: エフェクト画像以外消す\n" +
                "ALL OFF: すべて消す",
                "\n" +
                "",
                new string[] { "ALL ON", "ChipOFF", "EffectOnly", "ALL OFF" });
            this.list項目リスト.Add(this.iDrumsAttackEffect);

            this.iDrumsReverse = new CItemToggle("Reverse", CDTXMania.ConfigIni.bReverse.Drums,
                "ONにすると\n"+
                "判定ラインが上になり、\n" +
                "ノーツが下から上へ\n"+
                "流れます。",
                "The scroll way is reversed. Drums chips\n"
                + "flow from the bottom to the top.");
            this.list項目リスト.Add(this.iDrumsReverse);

            this.iDrumsPosition = new CItemList("JudgePosition", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.判定文字表示位置.Drums,
                "ゲーム中に表示される\n"+
                "判定文字の位置を変更します。\n" +
                "  P-A: レーン上\n" +
                "  P-B: 判定ライン下\n" +
                "  OFF: 表示しない",
                "The position to show judgement mark.\n" +
                "(Perfect, Great, ...)\n" +
                "\n" +
                " P-A: on the lanes.\n" +
                " P-B: under the hit bar.\n" +
                " OFF: no judgement mark.",
                new string[] { "P-A", "P-B", "OFF" });
            this.list項目リスト.Add(this.iDrumsPosition);

            this.iDrumsComboDisp = new CItemToggle("Combo", CDTXMania.ConfigIni.bドラムコンボ文字の表示,
                "OFFにするとコンボが表示されなくなります。",
                "Turn ON the Drums Combo Display");
            this.list項目リスト.Add( this.iDrumsComboDisp );

            this.iDrumsLaneType = new CItemList("LaneType", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eLaneType.Drums,
                "ドラムのレーンの配置を変更します。\n" +
                "Type-A 通常の設定です。\n" +
                "Type-B 2ペダルとタムをそれぞれま\n" +
                "とめた表示です。\n" +
                "Type-C 3タムのみをまとめた表示です。\n" +
                "Type-D 左右完全対象の表示です。",
                "To change the displaying position of\n" +
                "Drum Lanes.\n" +
                "Type-A default\n" +
                "Type-B Summarized 2 pedals and Toms.\n" +
                "Type-C Summarized 3 Toms only.\n" +
                "Type-D Work In Progress....",
                new string[] { "Type-A", "Type-B", "Type-C", "Type-D" });
            this.list項目リスト.Add(this.iDrumsLaneType);

            this.iDrumsRDPosition = new CItemList("RDPosition", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eRDPosition,
                "ライドシンバルレーンの表示\n" +
                "位置を変更します。\n"+
                "RD RC:最右端はRCレーンになります\n"+
                "RC RD: 最右端はRDレーンになります",
                "To change the displaying position of\n" +
                "the ride cymbal.",
                new string[] { "RD RC", "RC RD" });
            this.list項目リスト.Add(this.iDrumsRDPosition);

            //----------SpecialOption----------

            this.iDrumsHAZARD = new CItemToggle("HAZARD", CDTXMania.ConfigIni.bHAZARD,
                "ドSハザードモード\n" +
                "GREAT以下の判定でも回数が減ります。",
                "SuperHazardMode\n" +
                "");
            this.list項目リスト.Add(this.iDrumsHAZARD);

            this.iDrumsTight = new CItemToggle("Tight", CDTXMania.ConfigIni.bTight,
                "ドラムチップのないところでパッドを\n" +
                "叩くとミスになります。",
                "It becomes MISS to hit pad without\n" +
                " chip.");
            this.list項目リスト.Add(this.iDrumsTight);

            this.iSystemHHGroup = new CItemList("HH Group", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eHHGroup,
                "ハイハットレーン打ち分け設定：\n" +
                "左シンバル、ハイハットオープン、ハ\n" +
                "イハットクローズの打ち分け方法を指\n" +
                "定します。\n" +
                "  HH-0 ... LC | HHC | HHO\n" +
                "  HH-1 ... LC & ( HHC | HHO )\n" +
                "  HH-2 ... LC | ( HHC & HHO )\n" +
                "  HH-3 ... LC & HHC & HHO\n" +
                "\n",
                "HH-0: LC|HC|HO; all are separated.\n" +
                "HH-1: LC&(HC|HO);\n" +
                " HC and HO are separted.\n" +
                " LC is grouped with HC and HHO.\n" +
                "HH-2: LC|(HC&HO);\n" +
                " LC and HHs are separated.\n" +
                " HC and HO are grouped.\n" +
                "HH-3: LC&HC&HO; all are grouped.\n" +
                "\n",
                new string[] { "HH-0", "HH-1", "HH-2", "HH-3" });
            this.list項目リスト.Add(this.iSystemHHGroup);

            this.iSystemFTGroup = new CItemList("FT Group", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eFTGroup,
                "フロアタム打ち分け設定：\n" +
                "ロータムとフロアタムの打ち分け方法\n" +
                "を指定します。\n" +
                "  FT-0 ... LT | FT\n" +
                "  FT-1 ... LT & FT\n",
                "FT-0: LT|FT\n" +
                " LT and FT are separated.\n" +
                "FT-1: LT&FT\n" +
                " LT and FT are grouped.",
                new string[] { "FT-0", "FT-1" });
            this.list項目リスト.Add(this.iSystemFTGroup);

            this.iSystemCYGroup = new CItemList("CY Group", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eCYGroup,
                "シンバルレーン打ち分け設定：\n" +
                "右シンバルとライドシンバルの打ち分\n" +
                "け方法を指定します。\n" +
                "  CY-0 ... CY | RD\n" +
                "  CY-1 ... CY & RD\n",
                "CY-0: CY|RD\n" +
                " CY and RD are separated.\n" +
                "CY-1: CY&RD\n" +
                " CY and RD are grouped.",
                new string[] { "CY-0", "CY-1" });
            this.list項目リスト.Add(this.iSystemCYGroup);

            this.iSystemBDGroup = new CItemList("BD Group", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eBDGroup,		// #27029 2012.1.4 from
                "フットペダル打ち分け設定：\n" +
                "左ペダル、左バスドラ、右バスドラの打ち分け\n" +
                "方法を指定します。\n" +
                "  BD-0 ... LP | LBD | BD\n" +
                "  BD-1 ... LP | LBD & BD\n" +
                "  BD-2 ... LP & LBD | BD\n" +
                "  BD-3 ... LP & LBD & BD\n",
                new string[] { "BD-0", "BD-1", "BD-2", "BD-3" });
            this.list項目リスト.Add(this.iSystemBDGroup);

            this.iSystemCymbalFree = new CItemToggle("CymbalFree", CDTXMania.ConfigIni.bシンバルフリー,
                "シンバルフリーモード：\n" +
                "左シンバル・右シンバルの区別をなく\n" +
                "します。ライドシンバルまで区別をな\n" +
                "くすか否かは、CYGroup に従います。\n",
                "Turn ON to group LC (left cymbal) and\n" +
                " CY (right cymbal).\n" +
                "Whether RD (ride cymbal) is also\n" +
                " grouped or not depends on the\n" +
                "'CY Group' setting.");
            this.list項目リスト.Add(this.iSystemCymbalFree);

            this.iSystemHitSoundPriorityHH = new CItemList("HH Priority", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eHitSoundPriorityHH,
                "発声音決定の優先順位：\n" +
                "ハイハットレーン打ち分け有効時に、\n" +
                "チップの発声音をどのように決定する\n" +
                "かを指定します。\n" +
                "  C > P ... チップの音が優先\n" +
                "  P > C ... 叩いたパッドの音が優先\n" +
                "\n",
                "To specify playing sound in case you're\n" +
                " using HH-0,1 and 2.\n" +
                "\n" +
                "C>P:\n" +
                " Chip sound is prior to the pad sound.\n" +
                "P>C:\n" +
                " Pad sound is prior to the chip sound.\n" +
                "\n" +
                "* This value cannot be changed while \n" +
                "  BD Group is set as BD-1.",
                new string[] { "C>P", "P>C" });
            this.list項目リスト.Add(this.iSystemHitSoundPriorityHH);

            this.iSystemHitSoundPriorityFT = new CItemList("FT Priority", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eHitSoundPriorityFT,
                "発声音決定の優先順位：\n" +
                "フロアタム打ち分け有効時に、チップ\n" +
                "の発声音をどのように決定するかを\n" +
                "指定します。\n" +
                "  C > P ... チップの音が優先\n" +
                "  P > C ... 叩いたパッドの音が優先",
                "To specify playing sound in case you're\n" +
                " using FT-0.\n" +
                "\n" +
                "C>P:\n" +
                " Chip sound is prior to the pad sound.\n" +
                "P>C:\n" +
                " Pad sound is prior to the chip sound.",
                new string[] { "C>P", "P>C" });
            this.list項目リスト.Add(this.iSystemHitSoundPriorityFT);

            this.iSystemHitSoundPriorityCY = new CItemList("CY Priority", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eHitSoundPriorityCY,
                "発声音決定の優先順位：\n" +
                "シンバルレーン打ち分け有効時に、\n" +
                "チップの発声音をどのように決定する\n" +
                "かを指定します。\n" +
                "  C > P ... チップの音が優先\n" +
                "  P > C ... 叩いたパッドの音が優先",
                "To specify playing sound in case you're\n" +
                " using CY-0.\n" +
                "\n" +
                "C>P:\n" +
                " Chip sound is prior to the pad sound.\n" +
                "P>C:\n" +
                " Pad sound is prior to the chip sound.",
                new string[] { "C>P", "P>C" });
            this.list項目リスト.Add(this.iSystemHitSoundPriorityCY);

            this.iSystemFillIn = new CItemToggle("FillIn", CDTXMania.ConfigIni.bフィルイン有効,
                "フィルインエフェクトの使用：\n" +
                "フィルイン区間の爆発パターンに特別" +
                "のエフェクトを使用します。\n" +
                "フィルインエフェクトの描画にはそれな" +
                "りのマシンパワーが必要とされます。",
                "To show bursting effects at the fill-in\n" +
                " zone or not.");
            this.list項目リスト.Add(this.iSystemFillIn);

            this.iSystemHitSound = new CItemToggle("HitSound", CDTXMania.ConfigIni.bドラム打音を発声する,
                "打撃音の再生：\n" +
                "これをOFFにすると、パッドを叩いた\n" +
                "ときの音を再生しなくなります（ドラム\n" +
                "のみ）。\n" +
                "DTX の音色で演奏したい場合などに\n" +
                "OFF にします。\n" +
                "\n",
                "Turn OFF if you don't want to play\n" +
                " hitting chip sound.\n" +
                "It is useful to play with real/electric\n" +
                " drums kit.\n");
            this.list項目リスト.Add(this.iSystemHitSound);

            this.iSystemSoundMonitorDrums = new CItemToggle("DrumsMonitor", CDTXMania.ConfigIni.b演奏音を強調する.Drums,
                "ドラム音モニタ：\n" +
                "ドラム音を他の音より大きめの音量で\n" +
                "発声します。\n" +
                "ただし、オートプレイの場合は通常音\n" +
                "量で発声されます。",
                "To enhance the drums chip sound\n" +
                "(except autoplay).");
            this.list項目リスト.Add(this.iSystemSoundMonitorDrums);

            this.iSystemMinComboDrums = new CItemInteger("D-MinCombo", 1, 0x1869f, CDTXMania.ConfigIni.n表示可能な最小コンボ数.Drums,
                "表示可能な最小コンボ数（ドラム）：\n" +
                "画面に表示されるコンボの最小の数\n" +
                "を指定します。\n" +
                "1 ～ 99999 の値が指定可能です。",
                "Initial number to show the combo\n" +
                " for the drums.\n" +
                "You can specify from 1 to 99999.");
            this.list項目リスト.Add(this.iSystemMinComboDrums);
            
            this.iDrumsHHOGraphics = new CItemList("HHOGraphics", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eHHOGraphics.Drums,
                "オープンハイハットの表示画像を変更します。\n" +
                "A: DTXMania元仕様\n" +
                "B: ○なし\n" +
                "C: クローズハットと同じ",
                "To change the graphics of open hihat.\n" +
                "A: default graphics of DTXMania\n" +
                "B: A without a circle\n" +
                "C: same as closed hihat",
                new string[] { "Type A", "Type B", "Type C" });
            this.list項目リスト.Add(this.iDrumsHHOGraphics);

            this.iDrumsLBDGraphics = new CItemList("LBDGraphics", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eLBDGraphics.Drums,
                "LBDチップの表示画像を変更します。\n" +
                "A: LPと同じ画像を使う\n" +
                "B: LBDとLPで色分けをする",
                "To change the graphics of left bass.\n" +
                "A: same as LP chips\n" +
                "B: In LP and LBD Color-coded.",
                new string[] { "Type A", "Type B" });
            this.list項目リスト.Add(this.iDrumsLBDGraphics);

            this.iDrumsJudgeLinePos = new CItemInteger("JudgeLinePos", 0, 100, CDTXMania.ConfigIni.nJudgeLine.Drums,
                "判定ラインの位置を\n" +
                "調整できます。\n" +
                "0～100の間で指定できます。",
                "To change the judgeLinePosition for the\n" +
                "You can set it from 0 to 100.");
            this.list項目リスト.Add(this.iDrumsJudgeLinePos);

            this.iDrumsShutterInPos = new CItemInteger("ShutterInPos", 0, 100, CDTXMania.ConfigIni.nShutterInSide.Drums,
                "ノーツ出現側の\n"+
                "シャッター位置を調整し\n" +
                "ノーツの見える位置を\n"+
                "制限します。",
                "\n" +
                "\n" +
                "");
            this.list項目リスト.Add(this.iDrumsShutterInPos);

            this.iDrumsShutterOutPos = new CItemInteger("ShutterOutPos", 0, 100, CDTXMania.ConfigIni.nShutterOutSide.Drums,
                "判定ライン側の\n" +
                "シャッター位置を調整し\n" +
                "ノーツの見える位置を\n" +
                "制限します。",
                "\n" +
                "\n" +
                "");
            this.list項目リスト.Add(this.iDrumsShutterOutPos);

            this.iMutingLP = new CItemToggle("Muting LP", CDTXMania.ConfigIni.bMutingLP,
                "LPの入力で発声中のHHを\n消音します。",
                "Turn ON to let HH chips be muted\n" +
                "by LP chips.");
            this.list項目リスト.Add(this.iMutingLP);

            this.iDrumsAssignToLBD = new CItemToggle("AssignToLBD", CDTXMania.ConfigIni.bAssignToLBD.Drums,
                "旧仕様のドコドコチップを\n" +
                "LBDレーンに振り分けます。\n" +
                "LP、LBDがある譜面では\n"+
                "無効になります。",
                "To move some of BassDrum chips to\n" +
                "LBD lane moderately.\n" +
                "(for old-style 2-bass DTX scores\n" +
                "without LP & LBD chips)");
            this.list項目リスト.Add(this.iDrumsAssignToLBD);

            this.iDrumsDkdkType = new CItemList("DkdkType", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eDkdkType.Drums,
                "ツーバス譜面の仕様を変更する。\n" +
                "L R: デフォルト\n" +
                "R L: 始動足変更\n" +
                "R Only: dkdk1レーン化",
                "To change the style of double-bass-\n" +
                "concerned chips.\n" +
                "L R: default\n" +
                "R L: changes the beginning foot\n" +
                "R Only: puts bass chips into single\n" +
                "lane",
                new string[] { "L R", "R L", "R Only" });
            this.list項目リスト.Add(this.iDrumsDkdkType);

            this.iDrumsNumOfLanes = new CItemList("NumOfLanes", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eNumOfLanes.Drums,
                "10レーン譜面の仕様を変更する。\n" +
                "A: デフォルト10レーン\n" +
                "B: XG仕様9レーン\n" +
                "C: CLASSIC仕様6レーン",

                "To change the number of lanes.\n" +
                "10: default 10 lanes\n" +
                "9: XG style 9 lanes\n" +
                "6: classic style 6 lanes",
                new string[] { "10", "9", "6" });
            this.list項目リスト.Add(this.iDrumsNumOfLanes);

            this.iDrumsRandomPad = new CItemList("RandomPad", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eRandom.Drums,
                "ドラムのパッドチップが\n" +
                "ランダムに降ってきます。\n" +
                "Mirror:ミラーをかけます\n" +
                "Part:レーン単位で交換\n" +
                "Super:小節単位で交換\n" +
                "Hyper:1拍ごとに交換\n" +
                "Master:死ぬがよい\n" +
                "Another:丁度よくバラける",
                "Drums chips (pads) come randomly.\n" +
                "Mirror: \n" +
                "Part: swapping lanes randomly\n" +
                "Super: swapping for each measure\n" +
                "Hyper: swapping for each 1/4 measure\n" +
                "Master: game over...\n" +
                "Another: moderately swapping each\n" +
                "chip randomly",
                new string[] { "OFF", "Mirror", "Part", "Super", "Hyper", "Master", "Another" });
            this.list項目リスト.Add(this.iDrumsRandomPad);

            this.iDrumsRandomPedal = new CItemList("RandomPedal", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eRandomPedal.Drums,
                "ドラムの足チップがランダムに\n降ってきます。\n" +
                "Mirror:ミラーをかけます\n" +
                "Part:レーン単位で交換\n" +
                "Super:小節単位で交換\n" +
                "Hyper:1拍ごとに交換\n" +
                "Master:死ぬがよい\n" +
                "Another:丁度よくバラける",
                "Drums chips (pedals) come randomly.\n" +
                "Part: swapping lanes randomly\n" +
                "Super: swapping for each measure\n" +
                "Hyper: swapping for each 1/4 measure\n" +
                "Master: game over...\n" +
                "Another: moderately swapping each\n" +
                "chip randomly",
                new string[] { "OFF", "Mirror", "Part", "Super", "Hyper", "Master", "Another" });
            this.list項目リスト.Add(this.iDrumsRandomPedal);

			this.iDrumsGraph = new CItemToggle( "Graph", CDTXMania.ConfigIni.bGraph有効.Drums,
				"最高スキルと比較できるグラフを表示します。\n" +
				"オートプレイだと表示されません。",
				"To draw Graph or not." );
			this.list項目リスト.Add( this.iDrumsGraph );

            // #23580 2011.1.3 yyagi
            this.iDrumsInputAdjustTimeMs = new CItemInteger("InputAdjust", -99, 99, CDTXMania.ConfigIni.nInputAdjustTimeMs.Drums,
                "ドラムの入力タイミングの微調整を行います。\n" +
                "-99 ～ 99ms まで指定可能です。\n" +
                "値を指定してください。\n",
                "To adjust the drums input timing.\n" +
                "You can set from -99 to 0ms.\n" +
                "To decrease input lag, set minus value.");
            this.list項目リスト.Add(this.iDrumsInputAdjustTimeMs);

            this.iDrumsGoToKeyAssign = new CItemBase("Drums Keys", CItemBase.Eパネル種別.通常,
                "ドラムのキー入力に関する項目を設定します。",
                "Settings for the drums key/pad inputs.");
            this.list項目リスト.Add(this.iDrumsGoToKeyAssign);

            OnListMenuの初期化();
            this.n現在の選択項目 = 0;
            this.eメニュー種別 = Eメニュー種別.Drums;
        }
        #endregion
        #region [ t項目リストの設定・Guitar() ]
        public void t項目リストの設定・Guitar()
        {
            this.tConfigIniへ記録する();
            this.list項目リスト.Clear();

            // #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

            this.iGuitarReturnToMenu = new CItemBase("<< Return To Menu", CItemBase.Eパネル種別.その他,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            this.list項目リスト.Add(this.iGuitarReturnToMenu);
            //this.iGuitarAutoPlay = new CItemToggle( "AutoPlay", CDTXMania.ConfigIni.bAutoPlay.Guitar,
            //    "ギターパートを自動で演奏します。",
            //    "To play the guitar part automatically." );
            //this.list項目リスト.Add( this.iGuitarAutoPlay );

            this.iGuitarAutoPlayAll = new CItemThreeState("AutoPlay (All)", CItemThreeState.E状態.不定,
                "全ネック/ピックの自動演奏の ON/OFF を\n" +
                "まとめて切り替えます。",
                "You can change whether Auto or not\n" +
                " for all guitar neck/pick at once.");
            this.list項目リスト.Add(this.iGuitarAutoPlayAll);

            this.iGuitarR = new CItemToggle("    R", CDTXMania.ConfigIni.bAutoPlay.GtR,
                "Rネックを自動で演奏します。",
                "To play R neck automatically.");
            this.list項目リスト.Add(this.iGuitarR);

            this.iGuitarG = new CItemToggle("    G", CDTXMania.ConfigIni.bAutoPlay.GtG,
                "Gネックを自動で演奏します。",
                "To play G neck automatically.");
            this.list項目リスト.Add(this.iGuitarG);

            this.iGuitarB = new CItemToggle("    B", CDTXMania.ConfigIni.bAutoPlay.GtB,
                "Bネックを自動で演奏します。",
                "To play B neck automatically.");
            this.list項目リスト.Add(this.iGuitarB);

            this.iGuitarY = new CItemToggle("    Y", CDTXMania.ConfigIni.bAutoPlay.GtY,
                "Yネックを自動で演奏します。",
                "To play Y neck automatically.");
            this.list項目リスト.Add(this.iGuitarY);

            this.iGuitarP = new CItemToggle("    P", CDTXMania.ConfigIni.bAutoPlay.GtP,
                "Pネックを自動で演奏します。",
                "To play P neck automatically.");
            this.list項目リスト.Add(this.iGuitarP);

            this.iGuitarPick = new CItemToggle("    Pick", CDTXMania.ConfigIni.bAutoPlay.GtPick,
                "ピックを自動で演奏します。",
                "To play Pick automatically.");
            this.list項目リスト.Add(this.iGuitarPick);

            this.iGuitarW = new CItemToggle("    Wailing", CDTXMania.ConfigIni.bAutoPlay.GtW,
                "ウェイリングを自動で演奏します。",
                "To play wailing automatically.");
            this.list項目リスト.Add(this.iGuitarW);

            this.iGuitarScrollSpeed = new CItemInteger("ScrollSpeed", 0, 0x7cf, CDTXMania.ConfigIni.n譜面スクロール速度.Guitar,
                "演奏時のギター譜面のスクロールの\n速度を指定します。\nx0.5 ～ x1000.0 までを指定可能です。",
                "To change the scroll speed for the\nguitar lanes.\nYou can set it from x0.5 to x1000.0.\n(ScrollSpeed=x0.5 means half speed)");
            this.list項目リスト.Add(this.iGuitarScrollSpeed);

            this.iGuitarHIDSUD = new CItemList("HID-SUD", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.nHidSud.Guitar,
                "HIDDEN:チップが途中から見えなくなります。\n" +
                "SUDDEN:チップが途中まで見えません。\n" +
                "HID-SUD:HIDDEN、SUDDENの両方が適用\n" +
                "されます。\n" +
                "STEALTH:チップがずっと表示されません。",
                "The display position for Drums Combo.\n" +
                "Note that it doesn't take effect\n" +
                " at Autoplay ([Left] is forcely used).",
                new string[] { "OFF", "Hidden", "Sudden", "HidSud", "Stealth" });
            this.list項目リスト.Add(this.iGuitarHIDSUD);

            //----------DisplayOption----------

            this.iGuitarDark = new CItemList("       Dark", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eDark,
                 "レーン表示のオプションをまとめて切り替えます。\n" +
                 "HALF: レーンが表示されなくなります。\n" +
                 "FULL: さらに小節線、拍線、判定ラインも\n" +
                 "表示されなくなります。",
                 "OFF: all display parts are shown.\nHALF: lanes and gauge are\n disappeared.\nFULL: additionaly to HALF, bar/beat\n lines, hit bar are disappeared.",
                 new string[] { "OFF", "HALF", "FULL" });
            this.list項目リスト.Add(this.iGuitarDark);

            this.iGuitarLaneDisp = new CItemList("LaneDisp", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.nLaneDisp.Guitar,
                "レーンの縦線と小節線の表示を切り替えます。\n" +
                "ALL  ON :レーン背景、小節線を表示します。\n" +
                "LANE OFF:レーン背景を表示しません。\n" +
                "LINE OFF:小節線を表示しません。\n" +
                "ALL  OFF:レーン背景、小節線を表示しません。",
                "",
                new string[] { "ALL ON", "LANE OFF", "LINE OFF", "ALL OFF" });
            this.list項目リスト.Add(this.iGuitarLaneDisp);

            this.iGuitarJudgeLineDisp = new CItemToggle("JudgeLineDisp", CDTXMania.ConfigIni.bJudgeLineDisp.Guitar,
                "判定ラインの表示 / 非表示を切り替えます。",
                "Toggle JudgeLine");
            this.list項目リスト.Add(this.iGuitarJudgeLineDisp);

            this.iGuitarLaneFlush = new CItemToggle("LaneFlush", CDTXMania.ConfigIni.bLaneFlush.Guitar,
                "レーンフラッシュの表示の表示 / 非表示を\n" +
                 "切り替えます。",
                "Toggle LaneFlush");
            this.list項目リスト.Add(this.iGuitarLaneFlush);

            this.iGuitarAttackEffect = new CItemList("AttackEffect", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eAttackEffect.Guitar,
                 "アタックエフェクトの表示 / 非表示を\n" +
                 "切り替えます。",
                 "",
                new string[] { "ON", "OFF" });
            this.list項目リスト.Add(this.iGuitarAttackEffect);

            this.iGuitarReverse = new CItemToggle("Reverse", CDTXMania.ConfigIni.bReverse.Guitar,
                "ギターチップが譜面の上から下に\n流れるようになります。",
                "The scroll way is reversed. Guitar chips\nflow from the top to the bottom.");
            this.list項目リスト.Add(this.iGuitarReverse);

            //コンボ表示

            //RISKY

            this.iGuitarPosition = new CItemList("Position", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.判定文字表示位置.Guitar,
                "ギターの判定文字の表示位置を指定\nします。\n  P-A: レーン上\n  P-B: COMBO の下\n  P-C: 判定ライン上\n  OFF: 表示しない",
                "The position to show judgement mark.\n(Perfect, Great, ...)\n\n P-A: on the lanes.\n P-B: under the COMBO indication.\n P-C: on the JudgeLine.\n OFF: no judgement mark.",
                new string[] { "P-A", "P-B", "P-C", "OFF" });
            this.list項目リスト.Add(this.iGuitarPosition);

            //実機ではここにオートオプションが入る。

            this.iGuitarLight = new CItemToggle("Light", CDTXMania.ConfigIni.bLight.Guitar,
                "ギターチップのないところでピッキングしても\n BAD になりません。",
                "Even if you pick without any chips,\nit doesn't become BAD.");
            this.list項目リスト.Add(this.iGuitarLight);

            this.iGuitarRandom = new CItemList("Random", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eRandom.Guitar,
                "ギターのチップがランダムに降ってきます。\n  Mirror: ミラーをかけます\n  Part: 小節・レーン単位で交換\n  Super: チップ単位で交換\n  Hyper: 全部完全に変更",
                "Guitar chips come randomly.\n Mirror: \n Part: swapping lanes randomly for each\n  measures.\n Super: swapping chip randomly\n Hyper: swapping randomly\n  (number of lanes also changes)",
                new string[] { "OFF", "Mirror", "Part", "Super", "Hyper" });
            this.list項目リスト.Add(this.iGuitarRandom);

            //NumOfLanes(ここではレーンオ－トに相当する。)
            //バイブオプション(実装不可)
            //StageEffect

            this.iGuitarLeft = new CItemToggle("Left", CDTXMania.ConfigIni.bLeft.Guitar,
                "ギターの RGBYP の並びが左右反転します。\n（左利きモード）",
                "Lane order 'R-G-B-Y-P' becomes 'P-Y-B-G-R'\nfor lefty.");
            this.list項目リスト.Add(this.iGuitarLeft);

            this.iGuitarJudgeLinePos = new CItemInteger("JudgeLinePos", 0, 100, CDTXMania.ConfigIni.nJudgeLine.Guitar,
                "演奏時の判定ラインの高さを変更します。\n" +
                "0～100の間で指定できます。",
                "To change the judgeLinePosition for the\n" +
                "You can set it from 0 to 100.");
            this.list項目リスト.Add(this.iGuitarJudgeLinePos);

            //比較対象(そもそも比較グラフさえ完成していない)
            //シャッタータイプ
            this.iGuitarShutterInPos = new CItemInteger("ShutterInPos", 0, 100, CDTXMania.ConfigIni.nShutterInSide.Guitar,
                "演奏時のノーツが現れる側のシャッターの\n" +
                "位置を変更します。",
                "\n" +
                "\n" +
                "");
            this.list項目リスト.Add(this.iGuitarShutterInPos);

            this.iGuitarShutterOutPos = new CItemInteger("ShutterOutPos", 0, 100, CDTXMania.ConfigIni.nShutterOutSide.Guitar,
                "演奏時のノーツが消える側のシャッターの\n" +
                "位置を変更します。",
                "\n" +
                "\n" +
                "");
            this.list項目リスト.Add(this.iGuitarShutterOutPos);

            this.iSystemSoundMonitorGuitar = new CItemToggle("GuitarMonitor", CDTXMania.ConfigIni.b演奏音を強調する.Guitar,
                "ギター音モニタ：\nギター音を他の音より大きめの音量で\n発声します。\nただし、オートプレイの場合は通常音量で\n発声されます。",
                "To enhance the guitar chip sound\n(except autoplay).");
            this.list項目リスト.Add(this.iSystemSoundMonitorGuitar);

            this.iSystemMinComboGuitar = new CItemInteger("G-MinCombo", 0, 0x1869f, CDTXMania.ConfigIni.n表示可能な最小コンボ数.Guitar,
                "表示可能な最小コンボ数（ギター）：\n画面に表示されるコンボの最小の数を\n指定します。\n1 ～ 99999 の値が指定可能です。\n0にするとコンボを表示しません。",
                "Initial number to show the combo\n for the guitar.\nYou can specify from 1 to 99999.");
            this.list項目リスト.Add(this.iSystemMinComboGuitar);

			this.iGuitarGraph = new CItemToggle( "Graph", CDTXMania.ConfigIni.bGraph有効.Guitar,
				"最高スキルと比較できるグラフを表示します。\n" +
				"オートプレイだと表示されません。\n" +
                "この項目を有効にすると、ベースパートのグラフは\n" +
                "無効になります。",
				"To draw Graph or not." );
			this.list項目リスト.Add( this.iGuitarGraph );

            // #23580 2011.1.3 yyagi
            this.iGuitarInputAdjustTimeMs = new CItemInteger("InputAdjust", -99, 99, CDTXMania.ConfigIni.nInputAdjustTimeMs.Guitar,
                "ギターの入力タイミングの微調整を行います。\n-99 ～ 99ms まで指定可能です。\n入力ラグを軽減するためには、\n負の値を指定してください。",
                "To adjust the guitar input timing.\nYou can set from -99 to 0ms.\nTo decrease input lag, set minus value.");
            this.list項目リスト.Add(this.iGuitarInputAdjustTimeMs);

            this.iGuitarGoToKeyAssign = new CItemBase("Guitar Keys", CItemBase.Eパネル種別.通常,
                "ギターのキー入力に関する項目を設定します。",
                "Settings for the guitar key/pad inputs.");
            this.list項目リスト.Add(this.iGuitarGoToKeyAssign);

            OnListMenuの初期化();
            this.n現在の選択項目 = 0;
            this.eメニュー種別 = Eメニュー種別.Guitar;
        }
        #endregion
        #region [ t項目リストの設定・Bass() ]
        public void t項目リストの設定・Bass()
        {
            this.tConfigIniへ記録する();
            this.list項目リスト.Clear();

            // #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

            this.iBassReturnToMenu = new CItemBase("<< Return To Menu", CItemBase.Eパネル種別.その他,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            this.list項目リスト.Add(this.iBassReturnToMenu);
            //this.iBassAutoPlay = new CItemToggle( "AutoPlay", CDTXMania.ConfigIni.bAutoPlay.Bass,
            //    "ベースパートを自動で演奏します。",
            //    "To play the bass part automatically." );
            //this.list項目リスト.Add( this.iBassAutoPlay );

            this.iBassAutoPlayAll = new CItemThreeState("AutoPlay (All)", CItemThreeState.E状態.不定,
                "全ネック/ピックの自動演奏の ON/OFF を\n" +
                "まとめて切り替えます。",
                "You can change whether Auto or not\n" +
                " for all bass neck/pick at once.");
            this.list項目リスト.Add(this.iBassAutoPlayAll);

            this.iBassR = new CItemToggle("    R", CDTXMania.ConfigIni.bAutoPlay.BsR,
                "Rネックを自動で演奏します。",
                "To play R neck automatically.");
            this.list項目リスト.Add(this.iBassR);

            this.iBassG = new CItemToggle("    G", CDTXMania.ConfigIni.bAutoPlay.BsG,
                "Gネックを自動で演奏します。",
                "To play G neck automatically.");
            this.list項目リスト.Add(this.iBassG);

            this.iBassB = new CItemToggle("    B", CDTXMania.ConfigIni.bAutoPlay.BsB,
                "Bネックを自動で演奏します。",
                "To play B neck automatically.");
            this.list項目リスト.Add(this.iBassB);

            this.iBassY = new CItemToggle("    Y", CDTXMania.ConfigIni.bAutoPlay.BsY,
                "Yネックを自動で演奏します。",
                "To play Y neck automatically.");
            this.list項目リスト.Add(this.iBassY);

            this.iBassP = new CItemToggle("    P", CDTXMania.ConfigIni.bAutoPlay.BsP,
                "Pネックを自動で演奏します。",
                "To play P neck automatically.");
            this.list項目リスト.Add(this.iBassP);

            this.iBassPick = new CItemToggle("    Pick", CDTXMania.ConfigIni.bAutoPlay.BsPick,
                "ピックを自動で演奏します。",
                "To play Pick automatically.");
            this.list項目リスト.Add(this.iBassPick);

            this.iBassW = new CItemToggle("    Wailing", CDTXMania.ConfigIni.bAutoPlay.BsW,
                "ウェイリングを自動で演奏します。",
                "To play wailing automatically.");
            this.list項目リスト.Add(this.iBassW);

            this.iBassScrollSpeed = new CItemInteger("ScrollSpeed", 0, 0x7cf, CDTXMania.ConfigIni.n譜面スクロール速度.Bass,
                "演奏時のベース譜面のスクロールの\n速度を指定します。\nx0.5 ～ x1000.0 までを指定可能です。",
                "To change the scroll speed for the\nbass lanes.\nYou can set it from x0.5 to x1000.0.\n(ScrollSpeed=x0.5 means half speed)");
            this.list項目リスト.Add(this.iBassScrollSpeed);

            this.iBassHIDSUD = new CItemList("HID-SUD", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.nHidSud.Bass,
                "HIDDEN:チップが途中から見えなくなります。\n" +
                "SUDDEN:チップが途中まで見えません。\n" +
                "HID-SUD:HIDDEN、SUDDENの両方が適用\n" +
                "されます。\n" +
                "STEALTH:チップがずっと表示されません。",
                "The display position for Drums Combo.\n" +
                "Note that it doesn't take effect\n" +
                " at Autoplay ([Left] is forcely used).",
                new string[] { "OFF", "Hidden", "Sudden", "HidSud", "Stealth" });
            this.list項目リスト.Add(this.iBassHIDSUD);

            //----------DisplayOption----------

            this.iBassDark = new CItemList("       Dark", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eDark,
                 "レーン表示のオプションをまとめて切り替えます。\n" +
                 "HALF: レーンが表示されなくなります。\n" +
                 "FULL: さらに小節線、拍線、判定ラインも\n" +
                 "表示されなくなります。",
                 "OFF: all display parts are shown.\nHALF: lanes and gauge are\n disappeared.\nFULL: additionaly to HALF, bar/beat\n lines, hit bar are disappeared.",
                 new string[] { "OFF", "HALF", "FULL" });
            this.list項目リスト.Add(this.iBassDark);

            this.iBassLaneDisp = new CItemList("LaneDisp", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.nLaneDisp.Bass,
                "レーンの縦線と小節線の表示を切り替えます。\n" +
                "ALL  ON :レーン背景、小節線を表示します。\n" +
                "LANE OFF:レーン背景を表示しません。\n" +
                "LINE OFF:小節線を表示しません。\n" +
                "ALL  OFF:レーン背景、小節線を表示しません。",
                "",
                new string[] { "ALL ON", "LANE OFF", "LINE OFF", "ALL OFF" });
            this.list項目リスト.Add(this.iBassLaneDisp);

            this.iBassJudgeLineDisp = new CItemToggle("JudgeLineDisp", CDTXMania.ConfigIni.bJudgeLineDisp.Bass,
                "判定ラインの表示 / 非表示を切り替えます。",
                "Toggle JudgeLine");
            this.list項目リスト.Add(this.iBassJudgeLineDisp);

            this.iBassLaneFlush = new CItemToggle("LaneFlush", CDTXMania.ConfigIni.bLaneFlush.Bass,
                "レーンフラッシュの表示 / 非表示を\n" +
                 "切り替えます。",
                "Toggle LaneFlush");
            this.list項目リスト.Add(this.iBassLaneFlush);

            this.iBassAttackEffect = new CItemList("AttackEffect", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eAttackEffect.Bass,
                 "アタックエフェクトの表示 / 非表示を\n" +
                 "切り替えます。",
                 "",
                 new string[] { "ON", "OFF" });
            this.list項目リスト.Add(this.iBassAttackEffect);


            this.iBassReverse = new CItemToggle("Reverse", CDTXMania.ConfigIni.bReverse.Bass,
                "ベースチップが譜面の上から下に\n流れるようになります。",
                "The scroll way is reversed. Bass chips\nflow from the top to the bottom.");
            this.list項目リスト.Add(this.iBassReverse);

            this.iBassPosition = new CItemList("Position", CItemBase.Eパネル種別.通常,
                (int)CDTXMania.ConfigIni.判定文字表示位置.Bass,
                "ベースの判定文字の表示位置を指定します。\n  P-A: レーン上\n  P-B: COMBO の下\n  P-C: 判定ライン上\n  OFF: 表示しない",
                "The position to show judgement mark.\n(Perfect, Great, ...)\n\n P-A: on the lanes.\n P-B: under the COMBO indication.\n P-C: on the JudgeLine.\n OFF: no judgement mark.",
                new string[] { "P-A", "P-B", "P-C", "OFF" });
            this.list項目リスト.Add(this.iBassPosition);

            this.iBassRandom = new CItemList("Random", CItemBase.Eパネル種別.通常,
                (int)CDTXMania.ConfigIni.eRandom.Bass,
                "ベースのチップがランダムに降ってきます。\n  Mirror: ミラーをかけます\n  Part: 小節・レーン単位で交換\n  Super: チップ単位で交換\n  Hyper: 全部完全に変更",
                "Bass chips come randomly.\n Mirror: \n Part: swapping lanes randomly for each\n  measures.\n Super: swapping chip randomly\n Hyper: swapping randomly\n  (number of lanes also changes)",
                new string[] { "OFF", "Mirror", "Part", "Super", "Hyper" });
            this.list項目リスト.Add(this.iBassRandom);

            this.iBassLight = new CItemToggle("Light", CDTXMania.ConfigIni.bLight.Bass,
                "ベースチップのないところでピッキングしても\n BAD になりません。",
                "Even if you pick without any chips,\nit doesn't become BAD.");
            this.list項目リスト.Add(this.iBassLight);

            this.iBassLeft = new CItemToggle("Left", CDTXMania.ConfigIni.bLeft.Bass,
                "ベースの RGBYP の並びが左右反転します。\n（左利きモード）",
                "Lane order 'R-G-B-Y-P' becomes 'P-Y-B-G-R'\nfor lefty.");
            this.list項目リスト.Add(this.iBassLeft);

            this.iSystemSoundMonitorBass = new CItemToggle("BassMonitor", CDTXMania.ConfigIni.b演奏音を強調する.Bass,
            "ベース音モニタ：\nベース音を他の音より大きめの音量で\n発声します。\nただし、オートプレイの場合は通常音量で\n発声されます。",
            "To enhance the bass chip sound\n(except autoplay).");
            this.list項目リスト.Add(this.iSystemSoundMonitorBass);

            this.iSystemMinComboBass = new CItemInteger("B-MinCombo", 0, 0x1869f, CDTXMania.ConfigIni.n表示可能な最小コンボ数.Bass,
                "表示可能な最小コンボ数（ベース）：\n画面に表示されるコンボの最小の数\nを指定します。\n1 ～ 99999 の値が指定可能です。\n0にするとコンボを表示しません。",
                "Initial number to show the combo\n for the bass.\nYou can specify from 1 to 99999.");
            this.list項目リスト.Add(this.iSystemMinComboBass);

            this.iBassJudgeLinePos = new CItemInteger("JudgeLinePos", 0, 100, CDTXMania.ConfigIni.nJudgeLine.Bass,
                "演奏時の判定ラインの高さを変更します。\n" +
                "0～100の間で指定できます。",
                "To change the judgeLinePosition for the\n" +
                "You can set it from 0 to 100.");
            this.list項目リスト.Add(this.iBassJudgeLinePos);

            this.iBassShutterInPos = new CItemInteger("ShutterInPos", 0, 100, CDTXMania.ConfigIni.nShutterInSide.Bass,
                "演奏時のノーツが現れる側のシャッターの\n" +
                "位置を変更します。",
                "\n" +
                "\n" +
                "");
            this.list項目リスト.Add(this.iBassShutterInPos);

            this.iBassShutterOutPos = new CItemInteger("ShutterOutPos", 0, 100, CDTXMania.ConfigIni.nShutterOutSide.Bass,
                "演奏時のノーツが消える側のシャッターの\n" +
                "位置を変更します。",
                "\n" +
                "\n" +
                "");
            this.list項目リスト.Add(this.iBassShutterOutPos);

			this.iBassGraph = new CItemToggle( "Graph", CDTXMania.ConfigIni.bGraph有効.Bass,
				"最高スキルと比較できるグラフを表示します。\n" +
				"オートプレイだと表示されません。\n" +
                "この項目を有効にすると、ギターパートのグラフは\n" +
                "無効になります。",
				"To draw Graph or not." );
			this.list項目リスト.Add( this.iBassGraph );

            // #23580 2011.1.3 yyagi
            this.iBassInputAdjustTimeMs = new CItemInteger("InputAdjust", -99, 99, CDTXMania.ConfigIni.nInputAdjustTimeMs.Bass,
                "ベースの入力タイミングの微調整を行います。\n-99 ～ 99ms まで指定可能です。\n入力ラグを軽減するためには、\n負の値を指定してください。",
                "To adjust the bass input timing.\nYou can set from -99 to 0ms.\nTo decrease input lag, set minus value.");
            this.list項目リスト.Add(this.iBassInputAdjustTimeMs);

            this.iBassGoToKeyAssign = new CItemBase("Bass Keys", CItemBase.Eパネル種別.通常,
                "ベースのキー入力に関する項目を設定します。",
                "Settings for the bass key/pad inputs.");
            this.list項目リスト.Add(this.iBassGoToKeyAssign);

            OnListMenuの初期化();
            this.n現在の選択項目 = 0;
            this.eメニュー種別 = Eメニュー種別.Bass;
        }
        #endregion

        /// <summary>
        /// ESC押下時の右メニュー描画
        /// </summary>
        public void tEsc押下()
        {
            if (this.eメニュー種別 == Eメニュー種別.KeyAssignSystem)
            {
                t項目リストの設定・System();
            }
            else if (this.eメニュー種別 == Eメニュー種別.KeyAssignDrums)
            {
                t項目リストの設定・Drums();
            }
            else if (this.eメニュー種別 == Eメニュー種別.KeyAssignGuitar)
            {
                t項目リストの設定・Guitar();
            }
            else if (this.eメニュー種別 == Eメニュー種別.KeyAssignBass)
            {
                t項目リストの設定・Bass();
            }
            // これ以外なら何もしない
        }
        public void tEnter押下()
        {
            CDTXMania.Skin.sound決定音.t再生する();
            if (this.b要素値にフォーカス中)
            {
                this.b要素値にフォーカス中 = false;
            }
            else if (this.list項目リスト[this.n現在の選択項目].e種別 == CItemBase.E種別.整数)
            {
                this.b要素値にフォーカス中 = true;
            }
            else if (this.b現在選択されている項目はReturnToMenuである)
            {
                //this.tConfigIniへ記録する();
                //CONFIG中にスキン変化が発生すると面倒なので、一旦マスクした。
            }
            #region [ 個々のキーアサイン ]
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignDrumsLC)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.DRUMS, EKeyConfigPad.LC);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignDrumsHHC)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.DRUMS, EKeyConfigPad.HH);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignDrumsHHO)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.DRUMS, EKeyConfigPad.HHO);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignDrumsSD)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.DRUMS, EKeyConfigPad.SD);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignDrumsBD)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.DRUMS, EKeyConfigPad.BD);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignDrumsHT)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.DRUMS, EKeyConfigPad.HT);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignDrumsLT)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.DRUMS, EKeyConfigPad.LT);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignDrumsFT)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.DRUMS, EKeyConfigPad.FT);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignDrumsCY)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.DRUMS, EKeyConfigPad.CY);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignDrumsRD)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.DRUMS, EKeyConfigPad.RD);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignDrumsLP)			// #27029 2012.1.4 from
            {																							//
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.DRUMS, EKeyConfigPad.LP);	//
            }																							//
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignDrumsLBD)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.DRUMS, EKeyConfigPad.LBD);
            }


            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignGuitarR)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.GUITAR, EKeyConfigPad.R);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignGuitarG)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.GUITAR, EKeyConfigPad.G);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignGuitarB)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.GUITAR, EKeyConfigPad.B);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignGuitarY)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.GUITAR, EKeyConfigPad.Y);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignGuitarP)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.GUITAR, EKeyConfigPad.P);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignGuitarPick)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.GUITAR, EKeyConfigPad.Pick);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignGuitarWail)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.GUITAR, EKeyConfigPad.Wail);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignGuitarDecide)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.GUITAR, EKeyConfigPad.Decide);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignGuitarHelp)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.GUITAR, EKeyConfigPad.Help);
            }


            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignBassR)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.BASS, EKeyConfigPad.R);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignBassG)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.BASS, EKeyConfigPad.G);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignBassB)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.BASS, EKeyConfigPad.B);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignBassY)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.BASS, EKeyConfigPad.Y);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignBassP)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.BASS, EKeyConfigPad.P);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignBassPick)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.BASS, EKeyConfigPad.Pick);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignBassWail)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.BASS, EKeyConfigPad.Wail);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignBassDecide)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.BASS, EKeyConfigPad.Decide);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignBassHelp)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.BASS, EKeyConfigPad.Help);
            }
            else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignSystemCapture)
            {
                CDTXMania.stageコンフィグ.tパッド選択通知(EKeyConfigPart.SYSTEM, EKeyConfigPad.Capture);
            }
            #endregion
            else
            {

                // Enter押下後の後処理
                this.list項目リスト[this.n現在の選択項目].tEnter押下();
                if (this.list項目リスト[this.n現在の選択項目] == this.iSystemFullscreen)
                {
                    CDTXMania.app.b次のタイミングで全画面・ウィンドウ切り替えを行う = true;
                }
                else if (this.list項目リスト[this.n現在の選択項目] == this.iSystemVSyncWait)
                {
                    CDTXMania.ConfigIni.b垂直帰線待ちを行う = this.iSystemVSyncWait.bON;
                    CDTXMania.app.b次のタイミングで垂直帰線同期切り替えを行う = true;
                }
                #region [ AutoPlay #23886 2012.5.8 yyagi ]
                else if (this.list項目リスト[this.n現在の選択項目] == this.iDrumsAutoPlayAll)
                {
                    this.t全部のドラムパッドのAutoを切り替える(this.iDrumsAutoPlayAll.e現在の状態 == CItemThreeState.E状態.ON);
                }
                else if (this.list項目リスト[this.n現在の選択項目] == this.iGuitarAutoPlayAll)
                {
                    this.t全部のギターパッドのAutoを切り替える(this.iGuitarAutoPlayAll.e現在の状態 == CItemThreeState.E状態.ON);
                }
                else if (this.list項目リスト[this.n現在の選択項目] == this.iBassAutoPlayAll)
                {
                    this.t全部のベースパッドのAutoを切り替える(this.iBassAutoPlayAll.e現在の状態 == CItemThreeState.E状態.ON);
                }
                #endregion
                #region [ キーアサインへの遷移と脱出 ]
                else if (this.list項目リスト[this.n現在の選択項目] == this.iSystemGoToKeyAssign)			// #24609 2011.4.12 yyagi
                {
                    t項目リストの設定・KeyAssignSystem();
                }
                else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignSystemReturnToMenu)	// #24609 2011.4.12 yyagi
                {
                    t項目リストの設定・System();
                }
                else if (this.list項目リスト[this.n現在の選択項目] == this.iDrumsGoToKeyAssign)				// #24525 2011.3.15 yyagi
                {
                    t項目リストの設定・KeyAssignDrums();
                }
                else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignDrumsReturnToMenu)		// #24525 2011.3.15 yyagi
                {
                    t項目リストの設定・Drums();
                }
                else if (this.list項目リスト[this.n現在の選択項目] == this.iGuitarGoToKeyAssign)			// #24525 2011.3.15 yyagi
                {
                    t項目リストの設定・KeyAssignGuitar();
                }
                else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignGuitarReturnToMenu)	// #24525 2011.3.15 yyagi
                {
                    t項目リストの設定・Guitar();
                }
                else if (this.list項目リスト[this.n現在の選択項目] == this.iBassGoToKeyAssign)				// #24525 2011.3.15 yyagi
                {
                    t項目リストの設定・KeyAssignBass();
                }
                else if (this.list項目リスト[this.n現在の選択項目] == this.iKeyAssignBassReturnToMenu)		// #24525 2011.3.15 yyagi
                {
                    t項目リストの設定・Bass();
                }
                #endregion
                #region [ ダーク ]
                else if (this.list項目リスト[this.n現在の選択項目] == this.iDrumsDark)					// #27029 2012.1.4 from
                {
                    if (this.iDrumsDark.n現在選択されている項目番号 == (int)Eダークモード.FULL)
                    {
                        this.iDrumsLaneDisp.n現在選択されている項目番号 = 3;
                        this.iDrumsJudgeLineDisp.bON = false;
                        this.iDrumsLaneFlush.bON = false;
                    }
                    else if (this.iDrumsDark.n現在選択されている項目番号 == (int)Eダークモード.HALF)
                    {
                        this.iDrumsLaneDisp.n現在選択されている項目番号 = 1;
                        this.iDrumsJudgeLineDisp.bON = true;
                        this.iDrumsLaneFlush.bON = true;
                    }
                    else
                    {
                        this.iDrumsLaneDisp.n現在選択されている項目番号 = 0;
                        this.iDrumsJudgeLineDisp.bON = true;
                        this.iDrumsLaneFlush.bON = true;
                    }
                }
                else if (this.list項目リスト[this.n現在の選択項目] == this.iGuitarDark)					// #27029 2012.1.4 from
                {
                    if (this.iGuitarDark.n現在選択されている項目番号 == (int)Eダークモード.FULL)
                    {
                        this.iGuitarLaneDisp.n現在選択されている項目番号 = 3;
                        this.iGuitarJudgeLineDisp.bON = false;
                        this.iGuitarLaneFlush.bON = false;
                    }
                    else if (this.iGuitarDark.n現在選択されている項目番号 == (int)Eダークモード.HALF)
                    {
                        this.iGuitarLaneDisp.n現在選択されている項目番号 = 1;
                        this.iGuitarJudgeLineDisp.bON = true;
                        this.iGuitarLaneFlush.bON = true;
                    }
                    else
                    {
                        this.iGuitarLaneDisp.n現在選択されている項目番号 = 0;
                        this.iGuitarJudgeLineDisp.bON = true;
                        this.iGuitarLaneFlush.bON = true;
                    }
                }
                else if (this.list項目リスト[this.n現在の選択項目] == this.iBassDark)					// #27029 2012.1.4 from
                {
                    if (this.iBassDark.n現在選択されている項目番号 == (int)Eダークモード.FULL)
                    {
                        this.iBassLaneDisp.n現在選択されている項目番号 = 3;
                        this.iBassJudgeLineDisp.bON = false;
                        this.iBassLaneFlush.bON = false;
                    }
                    else if (this.iBassDark.n現在選択されている項目番号 == (int)Eダークモード.HALF)
                    {
                        this.iBassLaneDisp.n現在選択されている項目番号 = 1;
                        this.iBassJudgeLineDisp.bON = true;
                        this.iBassLaneFlush.bON = true;
                    }
                    else
                    {
                        this.iBassLaneDisp.n現在選択されている項目番号 = 0;
                        this.iBassJudgeLineDisp.bON = true;
                        this.iBassLaneFlush.bON = true;
                    }
                }
                #endregion
                #region[ ギター・ベースグラフ ]
                else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iGuitarGraph )
                {
                    if( this.iGuitarGraph.bON == true )
                    {
                        CDTXMania.ConfigIni.bGraph有効.Bass = false;
                        this.iBassGraph.bON = false;
                    }
                }
                else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iBassGraph )
                {
                    if( this.iBassGraph.bON == true )
                    {
                        CDTXMania.ConfigIni.bGraph有効.Guitar = false;
                        this.iGuitarGraph.bON = false;
                    }
                }
                #endregion
                else if (this.list項目リスト[this.n現在の選択項目] == this.iSystemUseBoxDefSkin)			// #28195 2012.5.6 yyagi
                {
                    CSkin.bUseBoxDefSkin = this.iSystemUseBoxDefSkin.bON;
                }
                #region [ スキン項目でEnterを押下した場合に限り、スキンの縮小サンプルを生成する。]
                else if (this.list項目リスト[this.n現在の選択項目] == this.iSystemSkinSubfolder)			// #28195 2012.5.2 yyagi
                {
                    tGenerateSkinSample();
                }
                #endregion
                #region [ 曲データ一覧の再読み込み ]
                else if (this.list項目リスト[this.n現在の選択項目] == this.iSystemReloadDTX)				// #32081 2013.10.21 yyagi
                {
                    if (CDTXMania.EnumSongs.IsEnumerating)
                    {
                        // Debug.WriteLine( "バックグラウンドでEnumeratingSongs中だったので、一旦中断します。" );
                        CDTXMania.EnumSongs.Abort();
                        CDTXMania.actEnumSongs.On非活性化();
                    }

                    CDTXMania.EnumSongs.StartEnumFromDisk();
                    CDTXMania.EnumSongs.ChangeEnumeratePriority(ThreadPriority.Normal);
                    CDTXMania.actEnumSongs.bコマンドでの曲データ取得 = true;
                    CDTXMania.actEnumSongs.On活性化();
                }
                #endregion
            }
        }

        private void tGenerateSkinSample()
        {
            nSkinIndex = ((CItemList)this.list項目リスト[this.n現在の選択項目]).n現在選択されている項目番号;
            if (nSkinSampleIndex != nSkinIndex)
            {
                string path = skinSubFolders[nSkinIndex];
                path = System.IO.Path.Combine(path, @"Graphics\2_background.jpg");
                Bitmap bmSrc = new Bitmap(path);
                Bitmap bmDest = new Bitmap(1280, 720);
                Graphics g = Graphics.FromImage(bmDest);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmSrc, new Rectangle(60, 106, (int)(1280 * 0.1984), (int)(720 * 0.1984)),
                    0, 0, 1280, 720, GraphicsUnit.Pixel);
                if (txSkinSample1 != null)
                {
                    CDTXMania.t安全にDisposeする(ref txSkinSample1);
                }
                txSkinSample1 = CDTXMania.tテクスチャの生成(bmDest, false);
                g.Dispose();
                bmDest.Dispose();
                bmSrc.Dispose();
                nSkinSampleIndex = nSkinIndex;
            }
        }

        #region [ 項目リストの設定 ( Exit, KeyAssignSystem/Drums/Guitar/Bass) ]
        public void t項目リストの設定・Exit()
        {
            this.tConfigIniへ記録する();
            this.eメニュー種別 = Eメニュー種別.Unknown;
        }
        public void t項目リストの設定・KeyAssignSystem()
        {
            //this.tConfigIniへ記録する();
            this.list項目リスト.Clear();

            // #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

            this.iKeyAssignSystemReturnToMenu = new CItemBase("<< ReturnTo Menu", CItemBase.Eパネル種別.その他,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            this.list項目リスト.Add(this.iKeyAssignSystemReturnToMenu);

            this.iKeyAssignSystemCapture = new CItemBase("Capture",
                "キャプチャキー設定：\n画面キャプチャのキーの割り当てを設\n定します。",
                "Capture key assign:\nTo assign key for screen capture.\n (You can use keyboard only. You can't\nuse pads to capture screenshot.");
            this.list項目リスト.Add(this.iKeyAssignSystemCapture);

            this.iKeyAssignGuitarHelp = new CItemBase("Help",
                "ヘルプボタンのキー設定：\nヘルプボタンへのキーの割り当\nてを設定します。",
                "Help button key assign:\nTo assign key/pads for Help button.");
            this.list項目リスト.Add(this.iKeyAssignGuitarHelp);

            this.iKeyAssignBassHelp = new CItemBase("Pause",
                "一時停止キー設定：\n 一時停止キーの割り当てを設定します。",
                "Pause key assign:\n To assign key/pads for Pause button.");
            this.list項目リスト.Add(this.iKeyAssignBassHelp);

            OnListMenuの初期化();
            this.n現在の選択項目 = 0;
            this.eメニュー種別 = Eメニュー種別.KeyAssignSystem;
        }
        public void t項目リストの設定・KeyAssignDrums()
        {
            //			this.tConfigIniへ記録する();
            this.list項目リスト.Clear();

            // #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

            this.iKeyAssignDrumsReturnToMenu = new CItemBase("<< ReturnTo Menu", CItemBase.Eパネル種別.その他,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            this.list項目リスト.Add(this.iKeyAssignDrumsReturnToMenu);

            this.iKeyAssignDrumsLC = new CItemBase("LeftCymbal",
                "ドラムのキー設定：\n左シンバルへのキーの割り当てを設\n定します。",
                "Drums key assign:\nTo assign key/pads for LeftCymbal\n button.");
            this.list項目リスト.Add(this.iKeyAssignDrumsLC);

            this.iKeyAssignDrumsHHC = new CItemBase("HiHat(Close)",
                "ドラムのキー設定：\nハイハット（クローズ）へのキーの割り\n当てを設定します。",
                "Drums key assign:\nTo assign key/pads for HiHat(Close)\n button.");
            this.list項目リスト.Add(this.iKeyAssignDrumsHHC);

            this.iKeyAssignDrumsHHO = new CItemBase("HiHat(Open)",
                "ドラムのキー設定：\nハイハット（オープン）へのキーの割り\n当てを設定します。",
                "Drums key assign:\nTo assign key/pads for HiHat(Open)\n button.");
            this.list項目リスト.Add(this.iKeyAssignDrumsHHO);

            this.iKeyAssignDrumsSD = new CItemBase("Snare",
                "ドラムのキー設定：\nスネアへのキーの割り当てを設定し\nます。",
                "Drums key assign:\nTo assign key/pads for Snare button.");
            this.list項目リスト.Add(this.iKeyAssignDrumsSD);

            this.iKeyAssignDrumsBD = new CItemBase("Bass",
                "ドラムのキー設定：\nバスドラムへのキーの割り当てを設定\nします。",
                "Drums key assign:\nTo assign key/pads for Bass button.");
            this.list項目リスト.Add(this.iKeyAssignDrumsBD);

            this.iKeyAssignDrumsHT = new CItemBase("HighTom",
                "ドラムのキー設定：\nハイタムへのキーの割り当てを設定\nします。",
                "Drums key assign:\nTo assign key/pads for HighTom\n button.");
            this.list項目リスト.Add(this.iKeyAssignDrumsHT);

            this.iKeyAssignDrumsLT = new CItemBase("LowTom",
                "ドラムのキー設定：\nロータムへのキーの割り当てを設定\nします。",
                "Drums key assign:\nTo assign key/pads for LowTom button.");
            this.list項目リスト.Add(this.iKeyAssignDrumsLT);

            this.iKeyAssignDrumsFT = new CItemBase("FloorTom",
                "ドラムのキー設定：\nフロアタムへのキーの割り当てを設\n定します。",
                "Drums key assign:\nTo assign key/pads for FloorTom\n button.");
            this.list項目リスト.Add(this.iKeyAssignDrumsFT);

            this.iKeyAssignDrumsCY = new CItemBase("RightCymbal",
                "ドラムのキー設定：\n右シンバルへのキーの割り当てを設\n定します。",
                "Drums key assign:\nTo assign key/pads for RightCymbal\n button.");
            this.list項目リスト.Add(this.iKeyAssignDrumsCY);

            this.iKeyAssignDrumsRD = new CItemBase("RideCymbal",
                "ドラムのキー設定：\nライドシンバルへのキーの割り当て\nを設定します。",
                "Drums key assign:\nTo assign key/pads for RideCymbal\n button.");
            this.list項目リスト.Add(this.iKeyAssignDrumsRD);

            this.iKeyAssignDrumsLP = new CItemBase("LeftPedal",									// #27029 2012.1.4 from
                "ドラムのキー設定：\n左ペダルへのキーの\n割り当てを設定します。",
                "Drums key assign:\nTo assign key/pads for HiHatPedal\n button.");
            this.list項目リスト.Add(this.iKeyAssignDrumsLP);

            this.iKeyAssignDrumsLBD = new CItemBase("LeftBassDrum",
                "ドラムのキー設定：\n左バスドラムへのキーの割り当てを設\n定します。",
                "Drums key assign:\nTo assign key/pads for RightCymbal\n button.");
            this.list項目リスト.Add(this.iKeyAssignDrumsLBD);

            OnListMenuの初期化();
            this.n現在の選択項目 = 0;
            this.eメニュー種別 = Eメニュー種別.KeyAssignDrums;
        }
        public void t項目リストの設定・KeyAssignGuitar()
        {
            //			this.tConfigIniへ記録する();
            this.list項目リスト.Clear();

            // #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

            this.iKeyAssignGuitarReturnToMenu = new CItemBase("<< ReturnTo Menu", CItemBase.Eパネル種別.その他,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            this.list項目リスト.Add(this.iKeyAssignGuitarReturnToMenu);

            this.iKeyAssignGuitarR = new CItemBase("R",
                "ギターのキー設定：\nRボタンへのキーの割り当てを設定し\nます。",
                "Guitar key assign:\nTo assign key/pads for R button.");
            this.list項目リスト.Add(this.iKeyAssignGuitarR);

            this.iKeyAssignGuitarG = new CItemBase("G",
                "ギターのキー設定：\nGボタンへのキーの割り当てを設定し\nます。",
                "Guitar key assign:\nTo assign key/pads for G button.");
            this.list項目リスト.Add(this.iKeyAssignGuitarG);

            this.iKeyAssignGuitarB = new CItemBase("B",
                "ギターのキー設定：\nBボタンへのキーの割り当てを設定し\nます。",
                "Guitar key assign:\nTo assign key/pads for B button.");
            this.list項目リスト.Add(this.iKeyAssignGuitarB);

            this.iKeyAssignGuitarY = new CItemBase("Y",
                "ギターのキー設定：\nYボタンへのキーの割り当てを設定し\nます。",
                "Guitar key assign:\nTo assign key/pads for Y button.");
            this.list項目リスト.Add(this.iKeyAssignGuitarY);

            this.iKeyAssignGuitarP = new CItemBase("P",
                "ギターのキー設定：\nPボタンへのキーの割り当てを設定し\nます。",
                "Guitar key assign:\nTo assign key/pads for P button.");
            this.list項目リスト.Add(this.iKeyAssignGuitarP);

            this.iKeyAssignGuitarPick = new CItemBase("Pick",
                "ギターのキー設定：\nピックボタンへのキーの割り当てを設\n定します。",
                "Guitar key assign:\nTo assign key/pads for Pick button.");
            this.list項目リスト.Add(this.iKeyAssignGuitarPick);

            this.iKeyAssignGuitarWail = new CItemBase("Wailing",
                "ギターのキー設定：\nWailingボタンへのキーの割り当てを\n設定します。",
                "Guitar key assign:\nTo assign key/pads for Wailing button.");
            this.list項目リスト.Add(this.iKeyAssignGuitarWail);

            this.iKeyAssignGuitarDecide = new CItemBase("Decide",
                "ギターのキー設定：\n決定ボタンへのキーの割り当てを設\n定します。",
                "Guitar key assign:\nTo assign key/pads for Decide button.");
            this.list項目リスト.Add(this.iKeyAssignGuitarDecide);

            OnListMenuの初期化();
            this.n現在の選択項目 = 0;
            this.eメニュー種別 = Eメニュー種別.KeyAssignGuitar;
        }
        public void t項目リストの設定・KeyAssignBass()
        {
            //			this.tConfigIniへ記録する();
            this.list項目リスト.Clear();

            // #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

            this.iKeyAssignBassReturnToMenu = new CItemBase("<< ReturnTo Menu", CItemBase.Eパネル種別.その他,
                "左側のメニューに戻ります。",
                "Return to left menu.");
            this.list項目リスト.Add(this.iKeyAssignBassReturnToMenu);

            this.iKeyAssignBassR = new CItemBase("R",
                "ベースのキー設定：\nRボタンへのキーの割り当てを設定し\nます。",
                "Bass key assign:\nTo assign key/pads for R button.");
            this.list項目リスト.Add(this.iKeyAssignBassR);

            this.iKeyAssignBassG = new CItemBase("G",
                "ベースのキー設定：\nGボタンへのキーの割り当てを設定し\nます。",
                "Bass key assign:\nTo assign key/pads for G button.");
            this.list項目リスト.Add(this.iKeyAssignBassG);

            this.iKeyAssignBassB = new CItemBase("B",
                "ベースのキー設定：\nBボタンへのキーの割り当てを設定し\nます。",
                "Bass key assign:\nTo assign key/pads for B button.");
            this.list項目リスト.Add(this.iKeyAssignBassB);

            this.iKeyAssignBassY = new CItemBase("Y",
                "ベースのキー設定：\nYボタンへのキーの割り当てを設定し\nます。",
                "Bass key assign:\nTo assign key/pads for Y button.");
            this.list項目リスト.Add(this.iKeyAssignBassY);

            this.iKeyAssignBassP = new CItemBase("P",
                "ベースのキー設定：\nPボタンへのキーの割り当てを設定し\nます。",
                "Bass key assign:\nTo assign key/pads for P button.");
            this.list項目リスト.Add(this.iKeyAssignBassP);

            this.iKeyAssignBassPick = new CItemBase("Pick",
                "ベースのキー設定：\nピックボタンへのキーの割り当てを設\n定します。",
                "Bass key assign:\nTo assign key/pads for Pick button.");
            this.list項目リスト.Add(this.iKeyAssignBassPick);

            this.iKeyAssignBassWail = new CItemBase("Wailing",
                "ベースのキー設定：\nWailingボタンへのキーの割り当てを設\n定します。",
                "Bass key assign:\nTo assign key/pads for Wailing button.");
            this.list項目リスト.Add(this.iKeyAssignBassWail);

            this.iKeyAssignBassDecide = new CItemBase("Decide",
                "ベースのキー設定：\n決定ボタンへのキーの割り当てを設\n定します。",
                "Bass key assign:\nTo assign key/pads for Decide button.");
            this.list項目リスト.Add(this.iKeyAssignBassDecide);

            OnListMenuの初期化();
            this.n現在の選択項目 = 0;
            this.eメニュー種別 = Eメニュー種別.KeyAssignBass;
        }
        #endregion
        public void t次に移動()
        {
            CDTXMania.Skin.soundカーソル移動音.t再生する();
            if (this.b要素値にフォーカス中)
            {
                this.list項目リスト[this.n現在の選択項目].t項目値を前へ移動();
            }
            else
            {
                this.n目標のスクロールカウンタ += 100;
                CDTXMania.stageコンフィグ.ct表示待機.n現在の値 = 0;
            }
        }
        public void t前に移動()
        {
            CDTXMania.Skin.soundカーソル移動音.t再生する();
            if (this.b要素値にフォーカス中)
            {
                this.list項目リスト[this.n現在の選択項目].t項目値を次へ移動();
            }
            else
            {
                this.n目標のスクロールカウンタ -= 100;
                CDTXMania.stageコンフィグ.ct表示待機.n現在の値 = 0;
            }
        }


        // CActivity 実装

        public override void On活性化()
        {
            if (this.b活性化してる)
                return;

            this.list項目リスト = new List<CItemBase>();
            this.eメニュー種別 = Eメニュー種別.Unknown;

            #region [ スキン選択肢と、現在選択中のスキン(index)の準備 #28195 2012.5.2 yyagi ]
            int ns = (CDTXMania.Skin.strSystemSkinSubfolders == null) ? 0 : CDTXMania.Skin.strSystemSkinSubfolders.Length;
            int nb = (CDTXMania.Skin.strBoxDefSkinSubfolders == null) ? 0 : CDTXMania.Skin.strBoxDefSkinSubfolders.Length;
            skinSubFolders = new string[ns + nb];
            for (int i = 0; i < ns; i++)
            {
                skinSubFolders[i] = CDTXMania.Skin.strSystemSkinSubfolders[i];
            }
            for (int i = 0; i < nb; i++)
            {
                skinSubFolders[ns + i] = CDTXMania.Skin.strBoxDefSkinSubfolders[i];
            }
            skinSubFolder_org = CDTXMania.Skin.GetCurrentSkinSubfolderFullName(true);
            Array.Sort(skinSubFolders);
            skinNames = CSkin.GetSkinName(skinSubFolders);
            nSkinIndex = Array.BinarySearch(skinSubFolders, skinSubFolder_org);
            if (nSkinIndex < 0)	// 念のため
            {
                nSkinIndex = 0;
            }
            nSkinSampleIndex = -1;
            #endregion

            this.prvFont = new CPrivateFastFont( new FontFamily( CDTXMania.ConfigIni.str選曲リストフォント ), 15 );	// t項目リストの設定 の前に必要

            this.t項目リストの設定・Bass();		// #27795 2012.3.11 yyagi; System設定の中でDrumsの設定を参照しているため、
            this.t項目リストの設定・Guitar();	// 活性化の時点でDrumsの設定も入れ込んでおかないと、System設定中に例外発生することがある。
            this.t項目リストの設定・Drums();	// 
            this.t項目リストの設定・System();	// 順番として、最後にSystemを持ってくること。設定一覧の初期位置がSystemのため。
            this.b要素値にフォーカス中 = false;
            this.n目標のスクロールカウンタ = 0;
            this.n現在のスクロールカウンタ = 0;
            this.nスクロール用タイマ値 = -1;
            this.ct三角矢印アニメ = new CCounter();

            this.iSystemSoundType_initial = this.iSystemSoundType.n現在選択されている項目番号; // CONFIGに入ったときの値を保持しておく
            this.iSystemWASAPIBufferSizeMs_initial = this.iSystemWASAPIBufferSizeMs.n現在の値; // CONFIG脱出時にこの値から変更されているようなら
            //this.iSystemASIOBufferSizeMs_initial = this.iSystemASIOBufferSizeMs.n現在の値; // サウンドデバイスを再構築する
            this.iSystemASIODevice_initial = this.iSystemASIODevice.n現在選択されている項目番号; //
            base.On活性化();
        }
        public override void On非活性化()
        {
            if (this.b活性化してない)
                return;

            this.tConfigIniへ記録する();
            this.list項目リスト.Clear();
            this.ct三角矢印アニメ = null;

            OnListMenuの解放();
            prvFont.Dispose();

            base.On非活性化();

            #region [ Skin変更 ]
            if (CDTXMania.Skin.GetCurrentSkinSubfolderFullName(true) != this.skinSubFolder_org)
            {
                CDTXMania.stageChangeSkin.tChangeSkinMain();	// #28195 2012.6.11 yyagi CONFIG脱出時にSkin更新
            }
            #endregion

            // #24820 2013.1.22 yyagi CONFIGでWASAPI/ASIO/DirectSound関連の設定を変更した場合、サウンドデバイスを再構築する。
            #region [ サウンドデバイス変更 ]
            if (this.iSystemSoundType_initial != this.iSystemSoundType.n現在選択されている項目番号 ||
                this.iSystemWASAPIBufferSizeMs_initial != this.iSystemWASAPIBufferSizeMs.n現在の値 ||
                //this.iSystemASIOBufferSizeMs_initial != this.iSystemASIOBufferSizeMs.n現在の値 ||
                this.iSystemASIODevice_initial != this.iSystemASIODevice.n現在選択されている項目番号)
            {
                ESoundDeviceType soundDeviceType;
                switch (this.iSystemSoundType.n現在選択されている項目番号)
                {
                    case 0:
                        soundDeviceType = ESoundDeviceType.DirectSound;
                        break;
                    case 1:
                        soundDeviceType = ESoundDeviceType.ASIO;
                        break;
                    case 2:
                        soundDeviceType = ESoundDeviceType.ExclusiveWASAPI;
                        break;
                    default:
                        soundDeviceType = ESoundDeviceType.Unknown;
                        break;
                }

                FDK.CSound管理.t初期化(soundDeviceType,
                                        this.iSystemWASAPIBufferSizeMs.n現在の値,
                                        0,
                                        this.iSystemASIODevice.n現在選択されている項目番号);
                //CDTXMania.app.ShowWindowTitleWithSoundType();   //XGオプション
                CDTXMania.app.AddSoundTypeToWindowTitle();    //GDオプション
            }
            #endregion
            #region [ サウンドのタイムストレッチモード変更 ]
            FDK.CSound管理.bIsTimeStretch = this.iSystemTimeStretch.bON;
            #endregion
        }
        public override void OnManagedリソースの作成()
        {
            if (this.b活性化してない)
                return;

            this.tx通常項目行パネル = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\4_itembox.png"), false);
            this.txその他項目行パネル = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\4_itembox other.png"), false);
            this.tx三角矢印 = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\4_triangle arrow.png"), false);
            this.tx説明文パネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\4_Description Panel.png" ) );
            this.tx矢印 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\4_Arrow.png" ) );
            this.txカーソル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\4_itembox cursor.png" ) );
            this.tx判定ライン = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\ScreenPlayDrums hit-bar.png"));
            this.txレーン = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_Paret.png"));
            this.txシャッター = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_shutter.png"));
            this.txSkinSample1 = null;		// スキン選択時に動的に設定するため、ここでは初期化しない
            base.OnManagedリソースの作成();
        }
        public override void OnManagedリソースの解放()
        {
            if (this.b活性化してない)
                return;

            CDTXMania.tテクスチャの解放(ref this.txSkinSample1);
            CDTXMania.tテクスチャの解放(ref this.tx通常項目行パネル);
            CDTXMania.tテクスチャの解放(ref this.txその他項目行パネル);
            CDTXMania.tテクスチャの解放(ref this.tx三角矢印);
            CDTXMania.tテクスチャの解放( ref this.tx説明文パネル );
            CDTXMania.tテクスチャの解放( ref this.tx矢印 );
            CDTXMania.tテクスチャの解放( ref this.txカーソル );
            CDTXMania.tテクスチャの解放(ref this.txレーン);
            CDTXMania.tテクスチャの解放(ref this.tx判定ライン);
            CDTXMania.tテクスチャの解放(ref this.txシャッター);
            base.OnManagedリソースの解放();
        }

		private void OnListMenuの初期化()
		{
			OnListMenuの解放();
			this.listMenu = new stMenuItemRight[ this.list項目リスト.Count ];
		}

		/// <summary>
		/// 事前にレンダリングしておいたテクスチャを解放する。
		/// </summary>
		private void OnListMenuの解放()
		{
			if ( listMenu != null )
			{
				for ( int i = 0; i < listMenu.Length; i++ )
				{
					if ( listMenu[ i ].txParam != null )
					{
						listMenu[ i ].txParam.Dispose();
					}
					if ( listMenu[ i ].txMenuItemRight != null )
					{
						listMenu[ i ].txMenuItemRight.Dispose();
					}
				}
				this.listMenu = null;
			}
		}
        public override int On進行描画()
        {
            throw new InvalidOperationException("t進行描画(bool)のほうを使用してください。");
        }
        public int t進行描画(bool b項目リスト側にフォーカスがある)
        {
            if (this.b活性化してない)
                return 0;

            // 進行

            #region [ 初めての進行描画 ]
            //-----------------
            if (base.b初めての進行描画)
            {
                this.nスクロール用タイマ値 = CSound管理.rc演奏用タイマ.n現在時刻;
                this.ct三角矢印アニメ.t開始(0, 9, 50, CDTXMania.Timer);

                base.b初めての進行描画 = false;
            }
            //-----------------
            #endregion

            this.b項目リスト側にフォーカスがある = b項目リスト側にフォーカスがある;		// 記憶

            #region [ 項目スクロールの進行 ]
            //-----------------
            long n現在時刻 = CDTXMania.Timer.n現在時刻;
            if (n現在時刻 < this.nスクロール用タイマ値) this.nスクロール用タイマ値 = n現在時刻;

            const int INTERVAL = 2;	// [ms]
            while ((n現在時刻 - this.nスクロール用タイマ値) >= INTERVAL)
            {
                int n目標項目までのスクロール量 = Math.Abs((int)(this.n目標のスクロールカウンタ - this.n現在のスクロールカウンタ));
                int n加速度 = 0;

                #region [ n加速度の決定；目標まで遠いほど加速する。]
                //-----------------
                if (n目標項目までのスクロール量 <= 100)
                {
                    n加速度 = 2;
                }
                else if (n目標項目までのスクロール量 <= 300)
                {
                    n加速度 = 3;
                }
                else if (n目標項目までのスクロール量 <= 500)
                {
                    n加速度 = 4;
                }
                else
                {
                    n加速度 = 8;
                }
                //-----------------
                #endregion
                #region [ this.n現在のスクロールカウンタに n加速度 を加減算。]
                //-----------------
                if (this.n現在のスクロールカウンタ < this.n目標のスクロールカウンタ)
                {
                    this.n現在のスクロールカウンタ += n加速度;
                    if (this.n現在のスクロールカウンタ > this.n目標のスクロールカウンタ)
                    {
                        // 目標を超えたら目標値で停止。
                        this.n現在のスクロールカウンタ = this.n目標のスクロールカウンタ;
                    }
                }
                else if (this.n現在のスクロールカウンタ > this.n目標のスクロールカウンタ)
                {
                    this.n現在のスクロールカウンタ -= n加速度;
                    if (this.n現在のスクロールカウンタ < this.n目標のスクロールカウンタ)
                    {
                        // 目標を超えたら目標値で停止。
                        this.n現在のスクロールカウンタ = this.n目標のスクロールカウンタ;
                    }
                }
                //-----------------
                #endregion
                #region [ 行超え処理、ならびに目標位置に到達したらスクロールを停止して項目変更通知を発行。]
                //-----------------
                if (this.n現在のスクロールカウンタ >= 100)
                {
                    this.n現在の選択項目 = this.t次の項目(this.n現在の選択項目);
                    this.n現在のスクロールカウンタ -= 100;
                    this.n目標のスクロールカウンタ -= 100;
                    if (this.n目標のスクロールカウンタ == 0)
                    {
                        CDTXMania.stageコンフィグ.t項目変更通知();
                    }
                }
                else if (this.n現在のスクロールカウンタ <= -100)
                {
                    this.n現在の選択項目 = this.t前の項目(this.n現在の選択項目);
                    this.n現在のスクロールカウンタ += 100;
                    this.n目標のスクロールカウンタ += 100;
                    if (this.n目標のスクロールカウンタ == 0)
                    {
                        CDTXMania.stageコンフィグ.t項目変更通知();
                    }
                }
                //-----------------
                #endregion

                this.nスクロール用タイマ値 += INTERVAL;
            }
            //-----------------
            #endregion

            #region [ ▲印アニメの進行 ]
            //-----------------
            if (this.b項目リスト側にフォーカスがある && (this.n目標のスクロールカウンタ == 0))
                this.ct三角矢印アニメ.t進行Loop();
            //-----------------
            #endregion


            // 描画

            this.ptパネルの基本座標[4].X = this.b項目リスト側にフォーカスがある ? 0x228 : 0x25a;		// メニューにフォーカスがあるなら、項目リストの中央は頭を出さない。

            //2014.04.25 kairera0467 GITADORAでは項目パネルが11個だが、選択中のカーソルは中央に無いので両方を同じにすると7×2+1=15個パネルが必要になる。
            //　　　　　　　　　　　 さらに画面に映らないがアニメーション中に見える箇所を含めると17個は必要とされる。
            //　　　　　　　　　　　 ただ、画面に表示させる分には上のほうを考慮しなくてもよさそうなので、上4個は必要なさげ。
            #region [ 計11個の項目パネルを描画する。]
            //-----------------
            int nItem = this.n現在の選択項目;
            for (int i = 0; i < 4; i++)
                nItem = this.t前の項目(nItem);

            for (int n行番号 = -4; n行番号 < 10; n行番号++)		// n行番号 == 0 がフォーカスされている項目パネル。
            {
                #region [ 今まさに画面外に飛びだそうとしている項目パネルは描画しない。]
                //-----------------
                if (((n行番号 == -4) && (this.n現在のスクロールカウンタ > 0)) ||		// 上に飛び出そうとしている
                    ((n行番号 == +9) && (this.n現在のスクロールカウンタ < 0)))		// 下に飛び出そうとしている
                {
                    nItem = this.t次の項目(nItem);
                    continue;
                }
                //-----------------
                #endregion

                int n移動元の行の基本位置 = n行番号 + 4;
                int n移動先の行の基本位置 = (this.n現在のスクロールカウンタ <= 0) ? ((n移動元の行の基本位置 + 1) % 14) : (((n移動元の行の基本位置 - 1) + 14) % 14);
                int x = this.pt新パネルの基本座標[n移動元の行の基本位置].X + ((int)((this.pt新パネルの基本座標[n移動先の行の基本位置].X - this.pt新パネルの基本座標[n移動元の行の基本位置].X) * (((double)Math.Abs(this.n現在のスクロールカウンタ)) / 100.0)));
                int y = this.pt新パネルの基本座標[n移動元の行の基本位置].Y + ((int)((this.pt新パネルの基本座標[n移動先の行の基本位置].Y - this.pt新パネルの基本座標[n移動元の行の基本位置].Y) * (((double)Math.Abs(this.n現在のスクロールカウンタ)) / 100.0)));
                int n新項目パネルX = 420;

                #region [ 現在の行の項目パネル枠を描画。]
                //-----------------
                switch (this.list項目リスト[nItem].eパネル種別)
                {
                    case CItemBase.Eパネル種別.通常:
                        if (this.tx通常項目行パネル != null)
                            this.tx通常項目行パネル.t2D描画(CDTXMania.app.Device, n新項目パネルX, y);
                        break;

                    case CItemBase.Eパネル種別.その他:
                        if (this.txその他項目行パネル != null)
                            this.txその他項目行パネル.t2D描画(CDTXMania.app.Device, n新項目パネルX, y);
                        break;
                }
                //-----------------
                #endregion
                #region [ 現在の行の項目名を描画。]
                //-----------------
				if ( listMenu[ nItem ].txMenuItemRight != null )	// 自前のキャッシュに含まれているようなら、再レンダリングせずキャッシュを使用
				{
					listMenu[ nItem ].txMenuItemRight.t2D描画( CDTXMania.app.Device, ( n新項目パネルX + 20 ), ( y + 24 ) );
				}
				else
				{
					Bitmap bmpItem = prvFont.DrawPrivateFont( this.list項目リスト[ nItem ].str項目名, Color.White, Color.Transparent );
					listMenu[ nItem ].txMenuItemRight = CDTXMania.tテクスチャの生成( bmpItem );
//					ctItem.t2D描画( CDTXMania.app.Device, ( x + 0x12 ) * Scale.X, ( y + 12 ) * Scale.Y - 20 );
//					CDTXMania.tテクスチャの解放( ref ctItem );
					CDTXMania.t安全にDisposeする( ref bmpItem );
				}
				//CDTXMania.stageコンフィグ.actFont.t文字列描画( x + 0x12, y + 12, this.list項目リスト[ nItem ].str項目名 );
                //-----------------
                #endregion
                #region [ 現在の行の項目の要素を描画。]
				//-----------------
				string strParam = null;
				bool b強調 = false;
				switch( this.list項目リスト[ nItem ].e種別 )
				{
					case CItemBase.E種別.ONorOFFトグル:
						#region [ *** ]
						//-----------------
						//CDTXMania.stageコンフィグ.actFont.t文字列描画( x + 210, y + 12, ( (CItemToggle) this.list項目リスト[ nItem ] ).bON ? "ON" : "OFF" );
						strParam = ( (CItemToggle) this.list項目リスト[ nItem ] ).bON ? "ON" : "OFF";
						break;
						//-----------------
						#endregion

					case CItemBase.E種別.ONorOFFor不定スリーステート:
						#region [ *** ]
						//-----------------
						switch( ( (CItemThreeState) this.list項目リスト[ nItem ] ).e現在の状態 )
						{
							case CItemThreeState.E状態.ON:
								strParam = "ON";
								break;

							case CItemThreeState.E状態.不定:
								strParam = "- -";
								break;

							default:
								strParam = "OFF";
								break;
						}
						//CDTXMania.stageコンフィグ.actFont.t文字列描画( x + 210, y + 12, "ON" );
						break;
						//-----------------
						#endregion

					case CItemBase.E種別.整数:		// #24789 2011.4.8 yyagi: add PlaySpeed supports (copied them from OPTION)
						#region [ *** ]
						//-----------------
						if( this.list項目リスト[ nItem ] == this.iCommonPlaySpeed )
						{
							double d = ( (double) ( (CItemInteger) this.list項目リスト[ nItem ] ).n現在の値 ) / 20.0;
							//CDTXMania.stageコンフィグ.actFont.t文字列描画( x + 210, y + 12, d.ToString( "0.000" ), ( n行番号 == 0 ) && this.b要素値にフォーカス中 );
							strParam = d.ToString( "0.000" );
						}
						else if( this.list項目リスト[ nItem ] == this.iDrumsScrollSpeed || this.list項目リスト[ nItem ] == this.iGuitarScrollSpeed || this.list項目リスト[ nItem ] == this.iBassScrollSpeed )
						{
							float f = ( ( (CItemInteger) this.list項目リスト[ nItem ] ).n現在の値 + 1 ) * 0.5f;
							//CDTXMania.stageコンフィグ.actFont.t文字列描画( x + 210, y + 12, f.ToString( "x0.0" ), ( n行番号 == 0 ) && this.b要素値にフォーカス中 );
							strParam = f.ToString( "x0.0" );
						}
						else
						{
							//CDTXMania.stageコンフィグ.actFont.t文字列描画( x + 210, y + 12, ( (CItemInteger) this.list項目リスト[ nItem ] ).n現在の値.ToString(), ( n行番号 == 0 ) && this.b要素値にフォーカス中 );
							strParam = ( (CItemInteger) this.list項目リスト[ nItem ] ).n現在の値.ToString();
						}
						b強調 = ( n行番号 == 0 ) && this.b要素値にフォーカス中;
						break;
						//-----------------
						#endregion

					case CItemBase.E種別.リスト:	// #28195 2012.5.2 yyagi: add Skin supports
						#region [ *** ]
						//-----------------
						{
							CItemList list = (CItemList) this.list項目リスト[ nItem ];
							//CDTXMania.stageコンフィグ.actFont.t文字列描画( x + 210, y + 12, list.list項目値[ list.n現在選択されている項目番号 ] );
							strParam = list.list項目値[ list.n現在選択されている項目番号 ];

							#region [ 必要な場合に、Skinのサンプルを生成・描画する。#28195 2012.5.2 yyagi ]
							if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iSystemSkinSubfolder )
							{
								tGenerateSkinSample();		// 最初にSkinの選択肢にきたとき(Enterを押す前)に限り、サンプル生成が発生する。

							}
							#endregion
							break;
						}
						//-----------------
						#endregion
				}
				if ( b強調 )
				{
					Bitmap bmpStr = b強調 ?
						prvFont.DrawPrivateFont( strParam, Color.White, Color.Black, Color.Yellow, Color.OrangeRed ) :
						prvFont.DrawPrivateFont( strParam, Color.Black, Color.Transparent );
					CTexture txStr = CDTXMania.tテクスチャの生成( bmpStr, false );
					txStr.t2D描画( CDTXMania.app.Device, ( n新項目パネルX + 260 ) , ( y + 20 ) );
					CDTXMania.tテクスチャの解放( ref txStr );
					CDTXMania.t安全にDisposeする( ref bmpStr );
				}
				else
				{
					int nIndex = this.list項目リスト[ nItem ].GetIndex();
					if ( listMenu[ nItem ].nParam != nIndex || listMenu[ nItem ].txParam == null )
					{
						stMenuItemRight stm = listMenu[ nItem ];
						stm.nParam = nIndex;
						object o = this.list項目リスト[ nItem ].obj現在値();
						stm.strParam = ( o == null ) ? "" : o.ToString();

				        Bitmap bmpStr =
				            prvFont.DrawPrivateFont( strParam, Color.Black, Color.Transparent );
				        stm.txParam = CDTXMania.tテクスチャの生成( bmpStr, false );
				        CDTXMania.t安全にDisposeする( ref bmpStr );

				        listMenu[ nItem ] = stm;
				    }
				    listMenu[ nItem ].txParam.t2D描画( CDTXMania.app.Device, ( n新項目パネルX + 260 ) , ( y + 24 ) );
				}
				//-----------------
                #endregion

                nItem = this.t次の項目(nItem);
            }
            //-----------------
            #endregion

            #region[ カーソル ]
            if( this.b項目リスト側にフォーカスがある )
            {
                this.txカーソル.t2D描画( CDTXMania.app.Device, 413, 193 );
            }
            #endregion

            #region[ 説明文パネル ]
            if( this.b項目リスト側にフォーカスがある && this.n目標のスクロールカウンタ == 0 && CDTXMania.stageコンフィグ.ct表示待機.b終了値に達した )
            {
                this.tx説明文パネル.t2D描画( CDTXMania.app.Device, 601, 252 );
                if ( txSkinSample1 != null && this.n目標のスクロールカウンタ == 0 && this.list項目リスト[ this.n現在の選択項目 ] == this.iSystemSkinSubfolder )
				{
					txSkinSample1.t2D描画( CDTXMania.app.Device, 615 - 60, 442 - 106 );
				}
            }
            #endregion

            #region [ 項目リストにフォーカスがあって、かつスクロールが停止しているなら、パネルの上下に▲印を描画する。]
            //-----------------
            if( this.b項目リスト側にフォーカスがある )//&& (this.n目標のスクロールカウンタ == 0))
            {
                int x;
                int y_upper;
                int y_lower;

                int n新カーソルX = 394;
                int n新カーソル上Y = 174;
                int n新カーソル下Y = 240;

                // 位置決定。

                if (this.b要素値にフォーカス中)
                {
                    x = 552;	// 要素値の上下あたり。
                    y_upper = 0x117 - this.ct三角矢印アニメ.n現在の値;
                    y_lower = 0x17d + this.ct三角矢印アニメ.n現在の値;
                }
                else
                {
                    x = 552;	// 項目名の上下あたり。
                    y_upper = 0x129 - this.ct三角矢印アニメ.n現在の値;
                    y_lower = 0x16b + this.ct三角矢印アニメ.n現在の値;
                }

                //新矢印
                if( this.tx矢印 != null )
                {
                    this.tx矢印.t2D描画(CDTXMania.app.Device, n新カーソルX, n新カーソル上Y, new Rectangle(0, 0, 40, 40));
                    this.tx矢印.t2D描画(CDTXMania.app.Device, n新カーソルX, n新カーソル下Y, new Rectangle(0, 40, 40, 40));
                }
            }
            //-----------------
            #endregion
            return 0;
        }


        // その他

        #region [ private ]
        //-----------------
        private enum Eメニュー種別
        {
            System,
            Drums,
            Guitar,
            Bass,
            KeyAssignSystem,		// #24609 2011.4.12 yyagi: 画面キャプチャキーのアサイン
            KeyAssignDrums,
            KeyAssignGuitar,
            KeyAssignBass,
            Unknown
        }

        private bool b項目リスト側にフォーカスがある;
        private bool b要素値にフォーカス中;
        private CCounter ct三角矢印アニメ;
        private Eメニュー種別 eメニュー種別;
        private CItemBase iKeyAssignSystemCapture;			// #24609
        private CItemBase iKeyAssignSystemReturnToMenu;		// #24609

        private CItemBase iKeyAssignGuitarHelp;
        private CItemBase iKeyAssignBassHelp;

        private CItemBase iKeyAssignBassR;
        private CItemBase iKeyAssignBassG;
        private CItemBase iKeyAssignBassB;
        private CItemBase iKeyAssignBassY;
        private CItemBase iKeyAssignBassP;
        private CItemBase iKeyAssignBassPick;
        private CItemBase iKeyAssignBassWail;
        private CItemBase iKeyAssignBassDecide;
        private CItemBase iKeyAssignBassReturnToMenu;

        public CItemBase iKeyAssignDrumsBD;
        public CItemBase iKeyAssignDrumsCY;
        public CItemBase iKeyAssignDrumsFT;
        public CItemBase iKeyAssignDrumsHHC;
        public CItemBase iKeyAssignDrumsHHO;
        public CItemBase iKeyAssignDrumsHT;
        public CItemBase iKeyAssignDrumsLC;
        public CItemBase iKeyAssignDrumsLT;
        public CItemBase iKeyAssignDrumsRD;
        private CItemBase iKeyAssignDrumsReturnToMenu;
        public CItemBase iKeyAssignDrumsSD;
        public CItemBase iKeyAssignDrumsLP;	// #27029 2012.1.4 from
        public CItemBase iKeyAssignDrumsLBD;

        private CItemBase iKeyAssignGuitarR;
        private CItemBase iKeyAssignGuitarG;
        private CItemBase iKeyAssignGuitarB;
        private CItemBase iKeyAssignGuitarY;
        private CItemBase iKeyAssignGuitarP;
        private CItemBase iKeyAssignGuitarPick;
        private CItemBase iKeyAssignGuitarWail;
        private CItemBase iKeyAssignGuitarDecide;
        private CItemBase iKeyAssignGuitarReturnToMenu;

        private CItemToggle iLogOutputLog;
        private CItemToggle iSystemAdjustWaves;
        private CItemToggle iSystemAudienceSound;
        private CItemInteger iSystemAutoChipVolume;
        private CItemToggle iSystemAVI;
        private CItemToggle iSystemBGA;
        private CItemToggle iSystemDirectShowMode;
        private CItemInteger iSystemBGAlpha;
        private CItemToggle iSystemBGMSound;
        private CItemInteger iSystemChipVolume;
        private CItemList iSystemCYGroup;
        private CItemToggle iSystemCymbalFree;
        private CItemList iSystemDamageLevel;
        private CItemToggle iSystemDebugInfo;
        private CItemToggle iSystemFillIn;
        private CItemList iSystemFTGroup;
        private CItemToggle iSystemFullscreen;
        private CItemList iSystemHHGroup;
        private CItemList iSystemBDGroup;		// #27029 2012.1.4 from
        private CItemToggle iSystemHitSound;
        private CItemList iSystemHitSoundPriorityCY;
        private CItemList iSystemHitSoundPriorityFT;
        private CItemList iSystemHitSoundPriorityHH;
        private CItemInteger iSystemMinComboBass;
        private CItemInteger iSystemMinComboDrums;
        private CItemInteger iSystemMinComboGuitar;
        private CItemList iSystemMovieMode;
        private CItemList iSystemMovieAlpha;
        private CItemInteger iSystemPreviewImageWait;
        private CItemInteger iSystemPreviewSoundWait;
        private CItemToggle iSystemRandomFromSubBox;
        private CItemBase iSystemReturnToMenu;
        private CItemToggle iSystemSaveScore;
        private CItemToggle iSystemSoundMonitorBass;
        private CItemToggle iSystemSoundMonitorDrums;
        private CItemToggle iSystemSoundMonitorGuitar;
        private CItemToggle iSystemStageFailed;
        private CItemToggle iSystemStageEffect;
        private CItemToggle iSystemStoicMode;
        private CItemToggle iSystemVSyncWait;
        private CItemList iSystemShowLag;					// #25370 2011.6.3 yyagi
        private CItemList iSystemShowLagColor;
        private CItemToggle iSystemAutoResultCapture;		// #25399 2011.6.9 yyagi
        private CItemToggle iSystemBufferedInput;
        private CItemInteger iSystemRisky;					// #23559 2011.7.27 yyagi
        private CItemList iSystemSoundType;         // #24820 2013.1.3 yyagi
        private CItemInteger iSystemWASAPIBufferSizeMs;		// #24820 2013.1.15 yyagi
        //private CItemInteger iSystemASIOBufferSizeMs;		// #24820 2013.1.3 yyagi
        private CItemList iSystemASIODevice;				// #24820 2013.1.17 yyagi
        private CItemList iInfoType;
        private CItemToggle iAutoAddGage;
        private CItemList iSystemSkillMode;
        private CItemToggle iMutingLP;
        private CItemToggle iSystemClassicNotes;
        private CItemInteger iSystemBGMAdjust;              // #36372 2016.06.19 kairera0467

        #region [ GDオプション ]
        
        private CItemToggle iSystemDifficlty;
        private CItemToggle iSystemShowScore;
        private CItemToggle iSystemShowMusicInfo;
        
        #endregion

        private int iSystemSoundType_initial;
        private int iSystemWASAPIBufferSizeMs_initial;
        //private int iSystemASIOBufferSizeMs_initial;
        private int iSystemASIODevice_initial;

        private CItemToggle iSystemTimeStretch;             // #23664 2013.2.24 yyagi

        private List<CItemBase> list項目リスト;
        private long nスクロール用タイマ値;
        private int n現在のスクロールカウンタ;
        public int n目標のスクロールカウンタ;
        private Point[] ptパネルの基本座標 = new Point[] { new Point(0x25a, 4), new Point(0x25a, 0x4f), new Point(0x25a, 0x9a), new Point(0x25a, 0xe5), new Point(0x228, 0x130), new Point(0x25a, 0x17b), new Point(0x25a, 0x1c6), new Point(0x25a, 0x211), new Point(0x25a, 0x25c), new Point(0x25a, 0x2a7), new Point(0x25a, 0x2d0) };
        private Point[] pt新パネルの基本座標 = new Point[] { new Point(0x25a, -79), new Point(0x25a, -12), new Point(0x25a, 55), new Point(0x25a, 122), new Point(0x228, 189), new Point(0x25a, 256), new Point(0x25a, 323), new Point(0x25a, 390), new Point(0x25a, 457), new Point(0x25a, 524), new Point(0x25a, 591), new Point(0x25a, 658), new Point(0x25a, 725), new Point(0x25a, 792) };
        private CTexture txその他項目行パネル;
        private CTexture tx三角矢印;
        private CTexture tx矢印;
        private CTexture tx通常項目行パネル;
        private CTexture txカーソル;
        private CTexture tx説明文パネル;

        private CPrivateFastFont prvFont;
        //private List<string> list項目リスト_str最終描画名;
        private struct stMenuItemRight
        {
            //	public string strMenuItem;
            public CTexture txMenuItemRight;
            public int nParam;
            public string strParam;
            public CTexture txParam;
        }
        private stMenuItemRight[] listMenu;

        private CTexture txSkinSample1;				// #28195 2012.5.2 yyagi
        private string[] skinSubFolders;			//
        private string[] skinNames;					//
        private string skinSubFolder_org;			//
        private int nSkinSampleIndex;				//
        private int nSkinIndex;						//

        private CTexture txレーン;
        private CTexture tx判定ライン;
        private CTexture txシャッター;

        private CItemBase iDrumsGoToKeyAssign;
        private CItemBase iGuitarGoToKeyAssign;
        private CItemBase iBassGoToKeyAssign;
        private CItemBase iSystemGoToKeyAssign;		// #24609

        private CItemList iSystemGRmode;
        private CItemToggle iSystemMusicNameDispDef;

        //private CItemToggle iBassAutoPlay;
        private CItemThreeState iBassAutoPlayAll;			// #23886 2012.5.8 yyagi
        private CItemToggle iBassR;							//
        private CItemToggle iBassG;							//
        private CItemToggle iBassB;							//
        private CItemToggle iBassY;							//
        private CItemToggle iBassP;							//
        private CItemToggle iBassPick;						//
        private CItemToggle iBassW;							//

        private CItemList iBassHIDSUD;
        private CItemToggle iBassLeft;
        private CItemToggle iBassLight;
        private CItemList iBassPosition;
        private CItemList iBassRandom;
        private CItemBase iBassReturnToMenu;
        private CItemToggle iBassReverse;
        private CItemInteger iBassScrollSpeed;
        private CItemList iBassDark;
        private CItemList iBassLaneDisp;
        private CItemToggle iBassJudgeLineDisp;
        private CItemList iBassAttackEffect;
        private CItemInteger iBassJudgeLinePos;
        private CItemInteger iBassShutterInPos;
        private CItemInteger iBassShutterOutPos;
        private CItemToggle iBassLaneFlush;
        private CItemToggle iBassGraph;

        private CItemInteger iCommonPlaySpeed;
        //		private CItemBase iCommonReturnToMenu;

        private CItemThreeState iDrumsAutoPlayAll;
        private CItemToggle iDrumsBass;
        private CItemList iDrumsComboPosition;
        private CItemToggle iDrumsCymbal;
        private CItemToggle iDrumsRide;
        private CItemToggle iDrumsFloorTom;
        private CItemList iDrumsHHOGraphics;
        private CItemList iDrumsHIDSUD;
        private CItemToggle iDrumsHighTom;
        private CItemToggle iDrumsHiHat;
        private CItemList iDrumsLaneType;
        private CItemList iDrumsLBDGraphics;
        private CItemToggle iDrumsLeftCymbal;
        private CItemToggle iDrumsLowTom;
        private CItemToggle iDrumsLeftPedal;
        private CItemToggle iDrumsLeftBassDrum;
        private CItemList iDrumsRDPosition;
        private CItemList iDrumsPosition;
        private CItemBase iDrumsReturnToMenu;
        private CItemToggle iDrumsReverse;
        private CItemInteger iDrumsScrollSpeed;
        private CItemToggle iDrumsSnare;
        private CItemToggle iDrumsTight;
        private CItemList iDrumsRandomPad;
        private CItemList iDrumsRandomPedal;
        private CItemList iDrumsNumOfLanes;
        private CItemList iDrumsDkdkType;
        private CItemToggle iDrumsAssignToLBD;
        private CItemToggle iDrumsHAZARD;
        private CItemList iDrumsAttackEffect;
        private CItemList iDrumsDark;
        private CItemList iDrumsLaneDisp;
        private CItemToggle iDrumsJudgeLineDisp;
        private CItemToggle iDrumsLaneFlush;
        private CItemInteger iDrumsJudgeLinePos;
        private CItemInteger iDrumsShutterInPos;
        private CItemInteger iDrumsShutterOutPos;
        private CItemToggle iDrumsComboDisp;
        private CItemToggle iDrumsGraph;

        //private CItemToggle iGuitarAutoPlay;
        private CItemThreeState iGuitarAutoPlayAll;			// #23886 2012.5.8 yyagi
        private CItemToggle iGuitarR;						//
        private CItemToggle iGuitarG;						//
        private CItemToggle iGuitarB;						//
        private CItemToggle iGuitarY;						//
        private CItemToggle iGuitarP;						//
        private CItemToggle iGuitarPick;					//
        private CItemToggle iGuitarW;						//

        private CItemList iGuitarHIDSUD;
        private CItemToggle iGuitarLeft;
        private CItemToggle iGuitarLight;
        private CItemList iGuitarPosition;
        private CItemList iGuitarRandom;
        private CItemBase iGuitarReturnToMenu;
        private CItemToggle iGuitarReverse;
        private CItemInteger iGuitarScrollSpeed;
        private CItemList iGuitarDark;
        private CItemList iGuitarLaneDisp;
        private CItemToggle iGuitarJudgeLineDisp;
        private CItemList iGuitarAttackEffect;
        private CItemInteger iGuitarJudgeLinePos;
        private CItemInteger iGuitarShutterInPos;
        private CItemInteger iGuitarShutterOutPos;
        private CItemToggle iGuitarLaneFlush;
        private CItemToggle iGuitarGraph;

        private CItemInteger iDrumsInputAdjustTimeMs;		// #23580 2011.1.3 yyagi
        private CItemInteger iGuitarInputAdjustTimeMs;		//
        private CItemInteger iBassInputAdjustTimeMs;		//
        private CItemList iSystemSkinSubfolder;				// #28195 2012.5.2 yyagi
        private CItemToggle iSystemUseBoxDefSkin;			// #28195 2012.5.6 yyagi
        private CItemBase iSystemReloadDTX;					// #32081 2013.10.21 yyagi

        private int t前の項目(int nItem)
        {
            if (--nItem < 0)
            {
                nItem = this.list項目リスト.Count - 1;
            }
            return nItem;
        }
        private int t次の項目(int nItem)
        {
            if (++nItem >= this.list項目リスト.Count)
            {
                nItem = 0;
            }
            return nItem;
        }
        private void t全部のドラムパッドのAutoを切り替える(bool bAutoON)
        {
            this.iDrumsLeftCymbal.bON = this.iDrumsHiHat.bON = this.iDrumsSnare.bON = this.iDrumsBass.bON = this.iDrumsHighTom.bON = this.iDrumsLowTom.bON = this.iDrumsFloorTom.bON = this.iDrumsCymbal.bON = this.iDrumsRide.bON = this.iDrumsLeftPedal.bON = this.iDrumsLeftBassDrum.bON = bAutoON;
        }
        private void t全部のギターパッドのAutoを切り替える(bool bAutoON)
        {
            this.iGuitarR.bON = this.iGuitarG.bON = this.iGuitarB.bON = this.iGuitarY.bON = this.iGuitarP.bON = this.iGuitarPick.bON = this.iGuitarW.bON = bAutoON;
        }
        private void t全部のベースパッドのAutoを切り替える(bool bAutoON)
        {
            this.iBassR.bON = this.iBassG.bON = this.iBassB.bON = this.iBassY.bON = this.iBassP.bON = this.iBassPick.bON = this.iBassW.bON = bAutoON;
        }
        private void tConfigIniへ記録する()
        {
            switch (this.eメニュー種別)
            {
                case Eメニュー種別.System:
                    this.tConfigIniへ記録する・System();
                    this.tConfigIniへ記録する・KeyAssignSystem();
                    return;

                case Eメニュー種別.Drums:
                    this.tConfigIniへ記録する・Drums();
                    this.tConfigIniへ記録する・KeyAssignDrums();
                    return;

                case Eメニュー種別.Guitar:
                    this.tConfigIniへ記録する・Guitar();
                    this.tConfigIniへ記録する・KeyAssignGuitar();
                    return;

                case Eメニュー種別.Bass:
                    this.tConfigIniへ記録する・Bass();
                    this.tConfigIniへ記録する・KeyAssignBass();
                    return;
            }
        }
        private void tConfigIniへ記録する・KeyAssignBass()
        {
        }
        private void tConfigIniへ記録する・KeyAssignDrums()
        {
        }
        private void tConfigIniへ記録する・KeyAssignGuitar()
        {
        }
        private void tConfigIniへ記録する・KeyAssignSystem()
        {
        }
        private void tConfigIniへ記録する・System()
        {
            //CDTXMania.ConfigIni.eDark = (Eダークモード) this.iCommonDark.n現在選択されている項目番号;
            CDTXMania.ConfigIni.n演奏速度 = this.iCommonPlaySpeed.n現在の値;

            CDTXMania.ConfigIni.bGuitar有効 = (((this.iSystemGRmode.n現在選択されている項目番号 + 1) / 2) == 1);
            //this.iSystemGuitar.bON;
            CDTXMania.ConfigIni.bDrums有効 = (((this.iSystemGRmode.n現在選択されている項目番号 + 1) % 2) == 1);
            //this.iSystemDrums.bON;

            CDTXMania.ConfigIni.b全画面モード = this.iSystemFullscreen.bON;
            CDTXMania.ConfigIni.bSTAGEFAILED有効 = this.iSystemStageFailed.bON;
            CDTXMania.ConfigIni.bランダムセレクトで子BOXを検索対象とする = this.iSystemRandomFromSubBox.bON;

            CDTXMania.ConfigIni.bWave再生位置自動調整機能有効 = this.iSystemAdjustWaves.bON;
            CDTXMania.ConfigIni.b垂直帰線待ちを行う = this.iSystemVSyncWait.bON;
            CDTXMania.ConfigIni.bバッファ入力を行う = this.iSystemBufferedInput.bON;
            CDTXMania.ConfigIni.bAVI有効 = this.iSystemAVI.bON;
            CDTXMania.ConfigIni.bBGA有効 = this.iSystemBGA.bON;
            CDTXMania.ConfigIni.bDirectShowMode = this.iSystemDirectShowMode.bON;
            CDTXMania.ConfigIni.n曲が選択されてからプレビュー音が鳴るまでのウェイトms = this.iSystemPreviewSoundWait.n現在の値;
            CDTXMania.ConfigIni.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms = this.iSystemPreviewImageWait.n現在の値;
            CDTXMania.ConfigIni.b演奏情報を表示する = this.iSystemDebugInfo.bON;
            CDTXMania.ConfigIni.nMovieMode = this.iSystemMovieMode.n現在選択されている項目番号;
            CDTXMania.ConfigIni.nMovieAlpha = this.iSystemMovieAlpha.n現在選択されている項目番号;
            CDTXMania.ConfigIni.n背景の透過度 = this.iSystemBGAlpha.n現在の値;
            CDTXMania.ConfigIni.bBGM音を発声する = this.iSystemBGMSound.bON;
            CDTXMania.ConfigIni.b歓声を発声する = this.iSystemAudienceSound.bON;
            CDTXMania.ConfigIni.eダメージレベル = (Eダメージレベル)this.iSystemDamageLevel.n現在選択されている項目番号;
            CDTXMania.ConfigIni.bScoreIniを出力する = this.iSystemSaveScore.bON;

            CDTXMania.ConfigIni.bログ出力 = this.iLogOutputLog.bON;
            CDTXMania.ConfigIni.n手動再生音量 = this.iSystemChipVolume.n現在の値;
            CDTXMania.ConfigIni.n自動再生音量 = this.iSystemAutoChipVolume.n現在の値;
            //CDTXMania.ConfigIni.bストイックモード = this.iSystemStoicMode.bON;
            CDTXMania.ConfigIni.ボーナス演出を表示する = this.iSystemStageEffect.bON;

            CDTXMania.ConfigIni.nShowLagType = this.iSystemShowLag.n現在選択されている項目番号;				// #25370 2011.6.3 yyagi
            CDTXMania.ConfigIni.nShowLagTypeColor = this.iSystemShowLagColor.n現在選択されている項目番号;
            CDTXMania.ConfigIni.bIsAutoResultCapture = this.iSystemAutoResultCapture.bON;					// #25399 2011.6.9 yyagi
            CDTXMania.ConfigIni.bAutoAddGage = this.iAutoAddGage.bON;
            CDTXMania.ConfigIni.nInfoType = this.iInfoType.n現在選択されている項目番号;
            CDTXMania.ConfigIni.nRisky = this.iSystemRisky.n現在の値;										// #23559 2911.7.27 yyagi

            #region [ GDオプション ]
            
            CDTXMania.ConfigIni.b難易度表示をXG表示にする = this.iSystemDifficlty.bON;
            CDTXMania.ConfigIni.bShowScore = this.iSystemShowScore.bON;
            CDTXMania.ConfigIni.bShowMusicInfo = this.iSystemShowMusicInfo.bON;
            
            #endregion

            CDTXMania.ConfigIni.strSystemSkinSubfolderFullName = skinSubFolders[nSkinIndex];				// #28195 2012.5.2 yyagi
            CDTXMania.Skin.SetCurrentSkinSubfolderFullName(CDTXMania.ConfigIni.strSystemSkinSubfolderFullName, true);
            CDTXMania.ConfigIni.bUseBoxDefSkin = this.iSystemUseBoxDefSkin.bON;								// #28195 2012.5.6 yyagi
            CDTXMania.ConfigIni.nSkillMode = this.iSystemSkillMode.n現在選択されている項目番号;
            CDTXMania.ConfigIni.bCLASSIC譜面判別を有効にする = iSystemClassicNotes.bON;

            CDTXMania.ConfigIni.nSoundDeviceType = this.iSystemSoundType.n現在選択されている項目番号; // #24820 2013.1.3 yyagi
            CDTXMania.ConfigIni.nWASAPIBufferSizeMs = this.iSystemWASAPIBufferSizeMs.n現在の値;				// #24820 2013.1.15 yyagi
            //CDTXMania.ConfigIni.nASIOBufferSizeMs = this.iSystemASIOBufferSizeMs.n現在の値; // #24820 2013.1.3 yyagi
            CDTXMania.ConfigIni.nASIODevice = this.iSystemASIODevice.n現在選択されている項目番号;			// #24820 2013.1.17 yyagi
            CDTXMania.ConfigIni.bTimeStretch = this.iSystemTimeStretch.bON; // #23664 2013.2.24 yyagi
            CDTXMania.ConfigIni.b曲名表示をdefのものにする = this.iSystemMusicNameDispDef.bON;
            CDTXMania.ConfigIni.nCommonBGMAdjustMs = this.iSystemBGMAdjust.n現在の値;                       // #36372 2016.06.19 kairera0467

            //Trace.TraceInformation( "saved" );
            //Trace.TraceInformation( "Skin現在Current : " + CDTXMania.Skin.GetCurrentSkinSubfolderFullName(true) );
            //Trace.TraceInformation( "Skin現在System  : " + CSkin.strSystemSkinSubfolderFullName );
            //Trace.TraceInformation( "Skin現在BoxDef  : " + CSkin.strBoxDefSkinSubfolderFullName );

        }
        private void tConfigIniへ記録する・Bass()
        {
            //CDTXMania.ConfigIni.bAutoPlay.Bass = this.iBassAutoPlay.bON;
            CDTXMania.ConfigIni.bAutoPlay.BsR = this.iBassR.bON;
            CDTXMania.ConfigIni.bAutoPlay.BsG = this.iBassG.bON;
            CDTXMania.ConfigIni.bAutoPlay.BsB = this.iBassB.bON;
            CDTXMania.ConfigIni.bAutoPlay.BsY = this.iBassY.bON;
            CDTXMania.ConfigIni.bAutoPlay.BsP = this.iBassP.bON;
            CDTXMania.ConfigIni.bAutoPlay.BsPick = this.iBassPick.bON;
            CDTXMania.ConfigIni.bAutoPlay.BsW = this.iBassW.bON;
            CDTXMania.ConfigIni.n譜面スクロール速度.Bass = this.iBassScrollSpeed.n現在の値;
            CDTXMania.ConfigIni.nHidSud.Bass = this.iBassHIDSUD.n現在選択されている項目番号;
            CDTXMania.ConfigIni.bReverse.Bass = this.iBassReverse.bON;
            CDTXMania.ConfigIni.判定文字表示位置.Bass = (Eタイプ)this.iBassPosition.n現在選択されている項目番号;
            CDTXMania.ConfigIni.eRandom.Bass = (Eランダムモード)this.iBassRandom.n現在選択されている項目番号;
            CDTXMania.ConfigIni.bLight.Bass = this.iBassLight.bON;
            CDTXMania.ConfigIni.bLeft.Bass = this.iBassLeft.bON;
            CDTXMania.ConfigIni.nInputAdjustTimeMs.Bass = this.iBassInputAdjustTimeMs.n現在の値;		// #23580 2011.1.3 yyagi

            CDTXMania.ConfigIni.bLaneFlush.Bass = this.iBassLaneFlush.bON;
            CDTXMania.ConfigIni.nLaneDisp.Bass = this.iBassLaneDisp.n現在選択されている項目番号;
            CDTXMania.ConfigIni.bJudgeLineDisp.Bass = this.iBassJudgeLineDisp.bON;
            CDTXMania.ConfigIni.eAttackEffect.Bass = (Eタイプ)this.iBassAttackEffect.n現在選択されている項目番号;
            CDTXMania.ConfigIni.nJudgeLine.Bass = this.iBassJudgeLinePos.n現在の値;
            CDTXMania.ConfigIni.nShutterInSide.Bass = this.iBassShutterInPos.n現在の値;
            CDTXMania.ConfigIni.nShutterOutSide.Bass = this.iBassShutterOutPos.n現在の値;

            CDTXMania.ConfigIni.b演奏音を強調する.Bass = this.iSystemSoundMonitorBass.bON;
            CDTXMania.ConfigIni.n表示可能な最小コンボ数.Bass = this.iSystemMinComboBass.n現在の値;
            CDTXMania.ConfigIni.bGraph有効.Bass = this.iBassGraph.bON;
        }
        private void tConfigIniへ記録する・Drums()
        {
            CDTXMania.ConfigIni.bAutoPlay.LC = this.iDrumsLeftCymbal.bON;
            CDTXMania.ConfigIni.bAutoPlay.HH = this.iDrumsHiHat.bON;
            CDTXMania.ConfigIni.bAutoPlay.SD = this.iDrumsSnare.bON;
            CDTXMania.ConfigIni.bAutoPlay.BD = this.iDrumsBass.bON;
            CDTXMania.ConfigIni.bAutoPlay.HT = this.iDrumsHighTom.bON;
            CDTXMania.ConfigIni.bAutoPlay.LT = this.iDrumsLowTom.bON;
            CDTXMania.ConfigIni.bAutoPlay.FT = this.iDrumsFloorTom.bON;
            CDTXMania.ConfigIni.bAutoPlay.CY = this.iDrumsCymbal.bON;
            CDTXMania.ConfigIni.bAutoPlay.RD = this.iDrumsRide.bON;
            CDTXMania.ConfigIni.bAutoPlay.LP = this.iDrumsLeftPedal.bON;
            CDTXMania.ConfigIni.bAutoPlay.LBD = this.iDrumsLeftBassDrum.bON;
            CDTXMania.ConfigIni.n譜面スクロール速度.Drums = this.iDrumsScrollSpeed.n現在の値;
            CDTXMania.ConfigIni.bReverse.Drums = this.iDrumsReverse.bON;
            CDTXMania.ConfigIni.判定文字表示位置.Drums = (Eタイプ)this.iDrumsPosition.n現在選択されている項目番号;
            CDTXMania.ConfigIni.bTight = this.iDrumsTight.bON;
            CDTXMania.ConfigIni.nInputAdjustTimeMs.Drums = this.iDrumsInputAdjustTimeMs.n現在の値;		// #23580 2011.1.3 yyagi
            CDTXMania.ConfigIni.nHidSud.Drums = this.iDrumsHIDSUD.n現在選択されている項目番号;

            CDTXMania.ConfigIni.eHHGroup = (EHHGroup)this.iSystemHHGroup.n現在選択されている項目番号;
            CDTXMania.ConfigIni.eFTGroup = (EFTGroup)this.iSystemFTGroup.n現在選択されている項目番号;
            CDTXMania.ConfigIni.eCYGroup = (ECYGroup)this.iSystemCYGroup.n現在選択されている項目番号;
            CDTXMania.ConfigIni.eBDGroup = (EBDGroup)this.iSystemBDGroup.n現在選択されている項目番号;
            CDTXMania.ConfigIni.eHitSoundPriorityHH = (E打ち分け時の再生の優先順位)this.iSystemHitSoundPriorityHH.n現在選択されている項目番号;
            CDTXMania.ConfigIni.eHitSoundPriorityFT = (E打ち分け時の再生の優先順位)this.iSystemHitSoundPriorityFT.n現在選択されている項目番号;
            CDTXMania.ConfigIni.eHitSoundPriorityCY = (E打ち分け時の再生の優先順位)this.iSystemHitSoundPriorityCY.n現在選択されている項目番号;
            CDTXMania.ConfigIni.eHHOGraphics.Drums = (Eタイプ)this.iDrumsHHOGraphics.n現在選択されている項目番号;
            CDTXMania.ConfigIni.eLBDGraphics.Drums = (Eタイプ)this.iDrumsLBDGraphics.n現在選択されている項目番号;
            CDTXMania.ConfigIni.eRDPosition = (ERDPosition)this.iDrumsRDPosition.n現在選択されている項目番号;
            CDTXMania.ConfigIni.bフィルイン有効 = this.iSystemFillIn.bON;
            CDTXMania.ConfigIni.b演奏音を強調する.Drums = this.iSystemSoundMonitorDrums.bON;
            CDTXMania.ConfigIni.bドラム打音を発声する = this.iSystemHitSound.bON;
            CDTXMania.ConfigIni.n表示可能な最小コンボ数.Drums = this.iSystemMinComboDrums.n現在の値;
            CDTXMania.ConfigIni.bシンバルフリー = this.iSystemCymbalFree.bON;
            CDTXMania.ConfigIni.eLaneType.Drums = (Eタイプ)this.iDrumsLaneType.n現在選択されている項目番号;

            CDTXMania.ConfigIni.nLaneDisp.Drums = this.iDrumsLaneDisp.n現在選択されている項目番号;
            CDTXMania.ConfigIni.bJudgeLineDisp.Drums = this.iDrumsJudgeLineDisp.bON;
            CDTXMania.ConfigIni.bLaneFlush.Drums = this.iDrumsLaneFlush.bON;

            CDTXMania.ConfigIni.bMutingLP = this.iMutingLP.bON;
            CDTXMania.ConfigIni.bAssignToLBD.Drums = this.iDrumsAssignToLBD.bON;

            CDTXMania.ConfigIni.eRandom.Drums = (Eランダムモード)this.iDrumsRandomPad.n現在選択されている項目番号;
            CDTXMania.ConfigIni.eRandomPedal.Drums = (Eランダムモード)this.iDrumsRandomPedal.n現在選択されている項目番号;
            CDTXMania.ConfigIni.eNumOfLanes.Drums = (Eタイプ)this.iDrumsNumOfLanes.n現在選択されている項目番号;
            CDTXMania.ConfigIni.eDkdkType.Drums = (Eタイプ)this.iDrumsDkdkType.n現在選択されている項目番号;

            CDTXMania.ConfigIni.bHAZARD = this.iDrumsHAZARD.bON;
            CDTXMania.ConfigIni.eAttackEffect.Drums = (Eタイプ)this.iDrumsAttackEffect.n現在選択されている項目番号;
            CDTXMania.ConfigIni.nJudgeLine.Drums = this.iDrumsJudgeLinePos.n現在の値;
            CDTXMania.ConfigIni.nShutterInSide.Drums = this.iDrumsShutterInPos.n現在の値;
            CDTXMania.ConfigIni.nShutterOutSide.Drums = this.iDrumsShutterOutPos.n現在の値;
            CDTXMania.ConfigIni.bドラムコンボ文字の表示 = this.iDrumsComboDisp.bON;
            CDTXMania.ConfigIni.bGraph有効.Drums = this.iDrumsGraph.bON;

            //CDTXMania.ConfigIni.eDark = (Eダークモード) this.iCommonDark.n現在選択されている項目番号;     // ダークはプリセット切り替えとして使うため、保存はしない。
        }
        private void tConfigIniへ記録する・Guitar()
        {
            //CDTXMania.ConfigIni.bAutoPlay.Guitar = this.iGuitarAutoPlay.bON;
            CDTXMania.ConfigIni.bAutoPlay.GtR = this.iGuitarR.bON;
            CDTXMania.ConfigIni.bAutoPlay.GtG = this.iGuitarG.bON;
            CDTXMania.ConfigIni.bAutoPlay.GtB = this.iGuitarB.bON;
            CDTXMania.ConfigIni.bAutoPlay.GtY = this.iGuitarY.bON;
            CDTXMania.ConfigIni.bAutoPlay.GtP = this.iGuitarP.bON;
            CDTXMania.ConfigIni.bAutoPlay.GtPick = this.iGuitarPick.bON;
            CDTXMania.ConfigIni.bAutoPlay.GtW = this.iGuitarW.bON;
            CDTXMania.ConfigIni.n譜面スクロール速度.Guitar = this.iGuitarScrollSpeed.n現在の値;
            CDTXMania.ConfigIni.nHidSud.Guitar = this.iGuitarHIDSUD.n現在選択されている項目番号;
            CDTXMania.ConfigIni.bReverse.Guitar = this.iGuitarReverse.bON;
            CDTXMania.ConfigIni.判定文字表示位置.Guitar = (Eタイプ)this.iGuitarPosition.n現在選択されている項目番号;
            CDTXMania.ConfigIni.eRandom.Guitar = (Eランダムモード)this.iGuitarRandom.n現在選択されている項目番号;
            CDTXMania.ConfigIni.bLight.Guitar = this.iGuitarLight.bON;
            CDTXMania.ConfigIni.bLeft.Guitar = this.iGuitarLeft.bON;
            CDTXMania.ConfigIni.nInputAdjustTimeMs.Guitar = this.iGuitarInputAdjustTimeMs.n現在の値;	// #23580 2011.1.3 yyagi

            CDTXMania.ConfigIni.bLaneFlush.Guitar = this.iGuitarLaneFlush.bON;
            CDTXMania.ConfigIni.nLaneDisp.Guitar = this.iGuitarLaneDisp.n現在選択されている項目番号;
            CDTXMania.ConfigIni.bJudgeLineDisp.Guitar = this.iGuitarJudgeLineDisp.bON;
            CDTXMania.ConfigIni.eAttackEffect.Guitar = (Eタイプ)this.iGuitarAttackEffect.n現在選択されている項目番号;
            CDTXMania.ConfigIni.nJudgeLine.Guitar = this.iGuitarJudgeLinePos.n現在の値;
            CDTXMania.ConfigIni.nShutterInSide.Guitar = this.iGuitarShutterInPos.n現在の値;
            CDTXMania.ConfigIni.nShutterOutSide.Guitar = this.iGuitarShutterOutPos.n現在の値;

            CDTXMania.ConfigIni.n表示可能な最小コンボ数.Guitar = this.iSystemMinComboGuitar.n現在の値;
            CDTXMania.ConfigIni.b演奏音を強調する.Guitar = this.iSystemSoundMonitorGuitar.bON;
            CDTXMania.ConfigIni.bGraph有効.Guitar = this.iGuitarGraph.bON;
        }
        //-----------------
        #endregion
    }
}
