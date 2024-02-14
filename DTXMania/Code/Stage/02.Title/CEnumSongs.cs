using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using SharpDX;
using SharpDX.Direct3D9;
using FDK;
using SampleFramework;
using System.Threading.Tasks;

namespace DTXMania
{
    internal class CEnumSongs							// #27060 2011.2.7 yyagi 曲リストを取得するクラス
    {													// ファイルキャッシュ(songslist.db)からの取得と、ディスクからの取得を、この一つのクラスに集約。

        public CSongManager Songs管理						// 曲の探索結果はこのSongs管理に読み込まれる
        {
            get;
            private set;
        }

        public bool IsSongListEnumCompletelyDone		// 曲リスト探索と、実際の曲リストへの反映が完了した？
        {
            get
            {
                return (this.state == DTXEnumState.CompletelyDone);
            }
        }
        public bool IsEnumerating
        {
            get
            {
                if (thDTXFileEnumerate == null)
                {
                    return false;
                }
                return thDTXFileEnumerate.IsAlive;
            }
        }
        public bool IsSongListEnumerated				// 曲リスト探索が完了したが、実際の曲リストへの反映はまだ？
        {
            get
            {
                return (this.state == DTXEnumState.Enumeratad);
            }
        }
        public bool IsSongListEnumStarted				// 曲リスト探索開始後？(探索完了も含む)
        {
            get
            {
                return (this.state != DTXEnumState.None);
            }
        }
        public void SongListEnumCompletelyDone()
        {
            this.state = DTXEnumState.CompletelyDone;
            this.Songs管理 = null;						// GCはOSに任せる
        }
        public bool IsSlowdown							// #PREMOVIE再生中は検索負荷を落とす
        {
            get
            {
                return this.Songs管理.bIsSlowdown;
            }
            set
            {
                this.Songs管理.bIsSlowdown = value;
            }
        }

        public void ChangeEnumeratePriority(ThreadPriority tp)
        {
            if (this.thDTXFileEnumerate != null && this.thDTXFileEnumerate.IsAlive == true)
            {
                this.thDTXFileEnumerate.Priority = tp;
            }
        }
        private readonly string strPathSongsDB = CDTXMania.strEXEのあるフォルダ + "songs.db";
        private readonly string strPathSongList = CDTXMania.strEXEのあるフォルダ + "songlist.db";

        public Thread thDTXFileEnumerate
        {
            get;
            private set;
        }
        private enum DTXEnumState
        {
            None,
            Ongoing,
            Suspended,
            Enumeratad,				// 探索完了、現在の曲リストに未反映
            CompletelyDone			// 探索完了、現在の曲リストに反映完了
        }
        private DTXEnumState state = DTXEnumState.None;


        /// <summary>
        /// Constractor
        /// </summary>
        public CEnumSongs()
        {
            this.Songs管理 = new CSongManager();
        }

        public void Init(List<CScore> ls, int n)
        {
            if (state == DTXEnumState.None)
            {
                this.Songs管理.listSongsDB = ls;
                this.Songs管理.nNbScoresFromSongsDB = n;
            }
        }

        /// <summary>
        /// 曲リストのキャッシュ(songlist.db)取得スレッドの開始
        /// </summary>
        public void StartEnumFromCache()
        {
            this.thDTXFileEnumerate = new Thread(new ThreadStart(this.t曲リストの構築1));
            this.thDTXFileEnumerate.Name = "曲リストの構築";
            this.thDTXFileEnumerate.IsBackground = true;
            this.thDTXFileEnumerate.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public delegate void AsyncDelegate();

        /// <summary>
        /// 曲検索スレッドの開始
        /// </summary>
        public void StartEnumFromDisk(bool readCache)
        {
            if (state == DTXEnumState.None || state == DTXEnumState.CompletelyDone)
            {
                Trace.TraceInformation("★曲データ検索スレッドを起動しました。");
                lock (this)
                {
                    state = DTXEnumState.Ongoing;
                }
                // this.autoReset = new AutoResetEvent( true );

                if (this.Songs管理 == null)		// Enumerating Songs完了後、CONFIG画面から再スキャンしたときにこうなる
                {
                    this.Songs管理 = new CSongManager();
                }
                this.thDTXFileEnumerate = new Thread(new ParameterizedThreadStart(this.t曲リストの構築2));
                this.thDTXFileEnumerate.Name = "曲リストの構築";
                this.thDTXFileEnumerate.IsBackground = true;
                //Is this really necessary?
                //this.thDTXFileEnumerate.Priority = System.Threading.ThreadPriority.Lowest;
                this.thDTXFileEnumerate.Start(readCache);
            }
        }


        /// <summary>
        /// 曲探索スレッドのサスペンド
        /// </summary>
        public void Suspend()
        {
            if (this.state != DTXEnumState.CompletelyDone &&
                ((thDTXFileEnumerate.ThreadState & (System.Threading.ThreadState.Background)) != 0))
            {
                // this.thDTXFileEnumerate.Suspend();		// obsoleteにつき使用中止
                this.Songs管理.bIsSuspending = true;
                this.state = DTXEnumState.Suspended;
                Trace.TraceInformation("★曲データ検索スレッドを中断しました。");
            }
        }

        /// <summary>
        /// 曲探索スレッドのレジューム
        /// </summary>
        public void Resume()
        {
            if (this.state == DTXEnumState.Suspended)
            {
                if ((this.thDTXFileEnumerate.ThreadState & (System.Threading.ThreadState.WaitSleepJoin | System.Threading.ThreadState.StopRequested)) != 0)	//
                {
                    // this.thDTXFileEnumerate.Resume();	// obsoleteにつき使用中止
                    this.Songs管理.bIsSuspending = false;
                    this.Songs管理.AutoReset.Set();
                    this.state = DTXEnumState.Ongoing;
                    Trace.TraceInformation("★曲データ検索スレッドを再開しました。");
                }
            }
        }

        /// <summary>
        /// 曲探索スレッドにサスペンド指示を出してから、本当にサスペンド状態に遷移するまでの間、ブロックする
        /// 500ms * 10回＝5秒でタイムアウトし、サスペンド完了して無くてもブロック解除する
        /// </summary>
        public void WaitUntilSuspended()
        {
            // 曲検索が一時中断されるまで待機
            for (int i = 0; i < 10; i++)
            {
                if (this.state == DTXEnumState.CompletelyDone ||
                    (thDTXFileEnumerate.ThreadState & (System.Threading.ThreadState.WaitSleepJoin | System.Threading.ThreadState.Background | System.Threading.ThreadState.Stopped)) != 0)
                {
                    break;
                }
                Trace.TraceInformation("★曲データ検索スレッドの中断待ちです: {0}", this.thDTXFileEnumerate.ThreadState.ToString());
                Thread.Sleep(500);
            }

        }

        /// <summary>
        /// 曲探索スレッドを強制終了する
        /// </summary>
        public void Abort()
        {
            if (thDTXFileEnumerate != null)
            {
                thDTXFileEnumerate.Abort();
                thDTXFileEnumerate = null;
                this.state = DTXEnumState.None;

                this.Songs管理 = null;					// Songs管理を再初期化する (途中まで作った曲リストの最後に、一から重複して追記することにならないようにする。)
                this.Songs管理 = new CSongManager();
            }
        }



        /// <summary>
        /// songlist.dbからの曲リスト構築
        /// </summary>
        public async void t曲リストの構築1()
        {
            // ！注意！
            // 本メソッドは別スレッドで動作するが、プラグイン側でカレントディレクトリを変更しても大丈夫なように、
            // すべてのファイルアクセスは「絶対パス」で行うこと。(2010.9.16)
            // 構築が完了したら、DTXEnumerateState state を DTXEnumerateState.Done にすること。(2012.2.9)
            DateTime now = DateTime.Now;

            try
            {
                #region [ 0) システムサウンドの構築  ]
                //-----------------------------
                CDTXMania.stageStartup.ePhaseID = CStage.EPhase.起動0_システムサウンドを構築;

                Trace.TraceInformation("0) システムサウンドを構築します。");
                Trace.Indent();

                try
                {
                    CDTXMania.Skin.bgm起動画面.tPlay();
                    for (int i = 0; i < CDTXMania.Skin.nシステムサウンド数; i++)
                    {
                        if (!CDTXMania.Skin[i].bExclusive)	// BGM系以外のみ読み込む。(BGM系は必要になったときに読み込む)
                        {
                            CSkin.CSystemSound cシステムサウンド = CDTXMania.Skin[i];
                            if (!CDTXMania.bCompactMode || cシステムサウンド.bCompact対象)
                            {
                                try
                                {
                                    cシステムサウンド.tRead();
                                    Trace.TraceInformation("システムサウンドを読み込みました。({0})", cシステムサウンド.strFilename);
                                    //if ( ( cシステムサウンド == CDTXMania.Skin.bgm起動画面 ) && cシステムサウンド.b読み込み成功 )
                                    //{
                                    //	cシステムサウンド.tPlay();
                                    //}
                                }
                                catch (FileNotFoundException)
                                {
                                    Trace.TraceWarning("システムサウンドが存在しません。({0})", cシステムサウンド.strFilename);
                                }
                                catch (Exception e)
                                {
                                    Trace.TraceError(e.Message);
                                    Trace.TraceWarning("システムサウンドの読み込みに失敗しました。({0})", cシステムサウンド.strFilename);
                                }
                            }
                        }
                    }
                    lock (CDTXMania.stageStartup.list進行文字列)
                    {
                        CDTXMania.stageStartup.list進行文字列.Add("Loading system sounds ... OK ");
                    }
                }
                finally
                {
                    Trace.Unindent();
                }
                //-----------------------------
                #endregion

                if (CDTXMania.bCompactMode)
                {
                    Trace.TraceInformation("コンパクトモードなので残りの起動処理は省略します。");
                    return;
                }

                #region [ 00) songlist.dbの読み込みによる曲リストの構築  ]
                //-----------------------------
                CDTXMania.stageStartup.ePhaseID = CStage.EPhase.起動00_songlistから曲リストを作成する;
                DateTime start1 = DateTime.Now;
                Trace.TraceInformation("1) Loading songlist.db ...");
                Trace.Indent();

                try
                {
                    if (!CDTXMania.ConfigIni.bConfigIniがないかDTXManiaのバージョンが異なる)
                    {
                        CSongManager s = new CSongManager();
                        s = await Deserialize(strPathSongList);		// 直接this.Songs管理にdeserialize()結果を代入するのは避ける。nullにされてしまうことがあるため。
                        if (s != null)
                        {
                            this.Songs管理 = s;
                        }

                        int scores = this.Songs管理.nNbScoresFound;
                        Trace.TraceInformation("Loading songlist.db complete. [{0} scores]", scores);
                        lock (CDTXMania.stageStartup.list進行文字列)
                        {
                            CDTXMania.stageStartup.list進行文字列.Add("Loading songlist.db ... OK");
                        }
                    }
                    else
                    {
                        Trace.TraceInformation("初回の起動であるかまたはDTXManiaのバージョンが上がったため、songlist.db の読み込みをスキップします。");
                        lock (CDTXMania.stageStartup.list進行文字列)
                        {
                            CDTXMania.stageStartup.list進行文字列.Add("Loading songlist.db ... Skip");
                        }
                    }
                }
                finally
                {
                    Trace.Unindent();
                    TimeSpan currSpan = (TimeSpan)(DateTime.Now - start1);
                    Trace.TraceInformation("Duration of Loading songlist.db: {0}", currSpan.ToString());
                }

                #endregion

                #region [ 1) songs.db の読み込み ]
                //-----------------------------
                CDTXMania.stageStartup.ePhaseID = CStage.EPhase.起動1_SongsDBからスコアキャッシュを構築;
                start1 = DateTime.Now;
                Trace.TraceInformation("2) Loading songs.db ...");
                Trace.Indent();

                try
                {
                    if (!CDTXMania.ConfigIni.bConfigIniがないかDTXManiaのバージョンが異なる)
                    {
                        try
                        {
                            this.Songs管理.tReadSongsDB(strPathSongsDB);
                        }
                        catch
                        {
                            Trace.TraceError("Fail to load songs.db");
                        }

                        int scores = (this.Songs管理 == null) ? 0 : this.Songs管理.nNbScoresFromSongsDB;	// 読み込み途中でアプリ終了した場合など、CDTXMania.SongManager がnullの場合があるので注意
                        Trace.TraceInformation("Loading songs.db complete. [{0} scores]", scores);
                        lock (CDTXMania.stageStartup.list進行文字列)
                        {
                            CDTXMania.stageStartup.list進行文字列.Add("Loading songs.db ... OK");
                        }
                    }
                    else
                    {
                        Trace.TraceInformation("初回の起動であるかまたはDTXManiaのバージョンが上がったため、songs.db の読み込みをスキップします。");
                        lock (CDTXMania.stageStartup.list進行文字列)
                        {
                            CDTXMania.stageStartup.list進行文字列.Add("Loading songs.db ... Skip");
                        }
                    }
                }
                finally
                {
                    Trace.Unindent();
                    TimeSpan currSpan = (TimeSpan)(DateTime.Now - start1);
                    Trace.TraceInformation("Duration of Loading songs.db: {0}", currSpan.ToString());
                }
                //-----------------------------
                #endregion

            }
            finally
            {
                CDTXMania.stageStartup.ePhaseID = CStage.EPhase.起動7_完了;
                TimeSpan span = (TimeSpan)(DateTime.Now - now);
                Trace.TraceInformation("Initialization Time: {0}", span.ToString());
                lock (this)							// #28700 2012.6.12 yyagi; state change must be in finally{} for exiting as of compact mode.
                {
                    state = DTXEnumState.CompletelyDone;
                }
            }
        }


        /// <summary>
        /// 起動してタイトル画面に遷移した後にバックグラウンドで発生させる曲検索
        /// #27060 2012.2.6 yyagi
        /// </summary>
        private async void t曲リストの構築2(object argObject)
        {
            // ！注意！
            // 本メソッドは別スレッドで動作するが、プラグイン側でカレントディレクトリを変更しても大丈夫なように、
            // すべてのファイルアクセスは「絶対パス」で行うこと。(2010.9.16)
            // 構築が完了したら、DTXEnumerateState state を DTXEnumerateState.Done にすること。(2012.2.9)

            DateTime now = DateTime.Now;
            bool bIsAvailableSongList = false;
            bool bIsAvailableSongsDB = false;
            bool bSucceededFastBoot = false;
            bool bReadCache = (bool)argObject;

            try
            {
                Trace.TraceInformation("Read cache option: " + bReadCache);
                if (bReadCache)
                {
                    try
                    {
                        #region [ 00) songlist.dbの読み込みによる曲リストの構築  ]
                        //-----------------------------
                        //CDTXMania.stageStartup.ePhaseID = CStage.EPhase.起動00_songlistから曲リストを作成する;
                        DateTime start1 = DateTime.Now;
                        Trace.TraceInformation("1) Loading songlist.db ...");
                        Trace.Indent();

                        try
                        {
                            if (!CDTXMania.ConfigIni.bConfigIniがないかDTXManiaのバージョンが異なる)
                            {
                                CSongManager s = new CSongManager();
                                s = await Deserialize(strPathSongList);       // 直接this.Songs管理にdeserialize()結果を代入するのは避ける。nullにされてしまうことがあるため。
                                if (s != null)
                                {
                                    this.Songs管理 = s;
                                }

                                int scores = this.Songs管理.nNbScoresFound;
                                Trace.TraceInformation("Loading songlist.db complete. [{0} scores]", scores);

                            }
                            else
                            {
                                Trace.TraceInformation("初回の起動であるかまたはDTXManiaのバージョンが上がったため、songlist.db の読み込みをスキップします。");

                            }
                        }
                        finally
                        {
                            Trace.Unindent();
                            TimeSpan currSpan = (TimeSpan)(DateTime.Now - start1);
                            Trace.TraceInformation("Duration of Loading songlist.db: {0}", currSpan.ToString());
                        }

                        #endregion

                        #region [ 1) songs.db の読み込み ]
                        //-----------------------------
                        //CDTXMania.stageStartup.ePhaseID = CStage.EPhase.起動1_SongsDBからスコアキャッシュを構築;
                        start1 = DateTime.Now;
                        Trace.TraceInformation("2) Loading songs.db ...");
                        Trace.Indent();

                        try
                        {
                            if (!CDTXMania.ConfigIni.bConfigIniがないかDTXManiaのバージョンが異なる)
                            {
                                try
                                {
                                    this.Songs管理.tReadSongsDB(strPathSongsDB);
                                }
                                catch
                                {
                                    Trace.TraceError("Fail to load songs.db");
                                }

                                int scores = (this.Songs管理 == null) ? 0 : this.Songs管理.nNbScoresFromSongsDB;    // 読み込み途中でアプリ終了した場合など、CDTXMania.SongManager がnullの場合があるので注意
                                Trace.TraceInformation("Loading songs.db complete. [{0} scores]", scores);

                            }
                            else
                            {
                                Trace.TraceInformation("初回の起動であるかまたはDTXManiaのバージョンが上がったため、songs.db の読み込みをスキップします。");

                            }
                        }
                        finally
                        {
                            Trace.Unindent();
                            TimeSpan currSpan = (TimeSpan)(DateTime.Now - start1);
                            Trace.TraceInformation("Duration of Loading songs.db: {0}", currSpan.ToString());
                        }
                        //-----------------------------
                        #endregion
                    }
                    finally
                    {
                        CDTXMania.SongManager = this.Songs管理;
                        //Reset temporary SongManager
                        this.Songs管理 = new CSongManager();
                        //Init temporary SongManager with SongListDB and Count
                        this.Songs管理.listSongsDB = CDTXMania.SongManager.listSongsDB;
                        this.Songs管理.nNbScoresFromSongsDB = CDTXMania.SongManager.nNbScoresForSongsDB;

                    }                    
                }

                #region [ 2) 曲データの検索 ]
                //-----------------------------
                //	base.ePhaseID = CStage.EPhase.起動2_曲を検索してリストを作成する;
                DateTime start = DateTime.Now;
                Trace.TraceInformation("enum2) Searching Song Data ...");                
                Trace.Indent();

                try
                {
                    if (!string.IsNullOrEmpty(CDTXMania.ConfigIni.str曲データ検索パス))
                    {
                        string[] strArray = CDTXMania.ConfigIni.str曲データ検索パス.Split(new char[] { ';' });
                        if (strArray.Length > 0)
                        {
                            // 全パスについて…
                            foreach (string str in strArray)
                            {
                                string path = str;
                                if (!Path.IsPathRooted(path))
                                {
                                    path = CDTXMania.strEXEのあるフォルダ + str;	// 相対パスの場合、絶対パスに直す(2010.9.16)
                                }

                                if (!string.IsNullOrEmpty(path))
                                {
                                    Trace.TraceInformation("Search Path: " + path);
                                    Trace.Indent();

                                    try
                                    {
                                        this.Songs管理.tSearchSongsAndCreateList(path, true);
                                    }
                                    catch (Exception e)
                                    {
                                        Trace.TraceError(e.Message);
                                        Trace.TraceError(e.StackTrace);
                                        Trace.TraceError("An exception occurred but processing will continue.");
                                    }
                                    finally
                                    {
                                        Trace.Unindent();                                        
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Trace.TraceWarning("No song data search path (DTXPath) specified in config.ini file. ");
                    }
                }
                finally
                {
                    Trace.TraceInformation("Song Data search complete. [{0} songs {1} scores]", this.Songs管理.nNbSongNodesFound, this.Songs管理.nNbScoresFound);
                    Trace.Unindent();
                    TimeSpan currSpan = (TimeSpan)(DateTime.Now - start);
                    Trace.TraceInformation("Duration of enum2) Searching Song Data : {0}", currSpan.ToString());
                }
                //	lock ( this.list進行文字列 )
                //	{
                //		this.list進行文字列.Add( string.Format( "{0} ... {1} scores ({2} songs)", "Enumerating songs", this..Songs管理_裏読.nNbScoresFound, this.Songs管理_裏読.nNbSongNodesFound ) );
                //	}
                //-----------------------------
                #endregion
                #region [ 3) songs.db 情報の曲リストへの反映 ]
                //-----------------------------
                //					base.ePhaseID = CStage.EPhase.起動3_スコアキャッシュをリストに反映する;
                start = DateTime.Now;
                Trace.TraceInformation("enum3) Loading score cache into songs.db. ");
                Trace.Indent();

                try
                {
                    if (this.Songs管理.listSongsDB != null)
                    {
                        this.Songs管理.tReflectScoreCacheInSongList();
                    }
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.Message);
                    Trace.TraceError(e.StackTrace);
                    Trace.TraceError("An exception has occurred, but processing will continue.");
                }
                finally
                {
                    Trace.TraceInformation("Score cache loading complete. [{0}/{1}スコア]", this.Songs管理.nNbScoresFromScoreCache, this.Songs管理.nNbScoresFound);
                    Trace.Unindent();
                    TimeSpan currSpan = (TimeSpan)(DateTime.Now - start);
                    Trace.TraceInformation("Duration of enum3) Loading score cache into songs.db. : {0}", currSpan.ToString());
                }
                //	lock ( this.list進行文字列 )
                //	{
                //		this.list進行文字列.Add( string.Format( "{0} ... {1}/{2}", "Loading score properties from songs.db", CDTXMania.Songs管理_裏読.nNbScoresFromScoreCache, cs.nNbScoresFound ) );
                //	}
                //-----------------------------
                #endregion
                #region [ 4) songs.db になかった曲データをファイルから読み込んで反映 ]
                //-----------------------------
                //					base.ePhaseID = CStage.EPhase.起動4_スコアキャッシュになかった曲をファイルから読み込んで反映する;

                int num2 = this.Songs管理.nNbScoresFound - this.Songs管理.nNbScoresFromScoreCache;

                Trace.TraceInformation("{0}, {1}", this.Songs管理.nNbScoresFound, this.Songs管理.nNbScoresFromScoreCache);
                start = DateTime.Now;
                Trace.TraceInformation("enum4) Reads and copy song data information from files [{0} scores] that were not in songs.db. ", num2);
                Trace.Indent();

                try
                {
                    //NOTE: This is the most time consuming step in Song Loading. To be optimized
                    this.Songs管理.tSongsDBになかった曲をファイルから読み込んで反映する();
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.Message);
                    Trace.TraceError(e.StackTrace);
                    Trace.TraceError("An exception has occurred, but processing will continue.");
                }
                finally
                {
                    Trace.TraceInformation("Copying into song data complete. [{0}/{1} scores]", this.Songs管理.nNbScoresFromFile, num2);
                    Trace.Unindent();
                    TimeSpan currSpan = (TimeSpan)(DateTime.Now - start);
                    Trace.TraceInformation("Duration of enum4) Reads and copy song data: {0}", currSpan.ToString());
                }
                //					lock ( this.list進行文字列 )
                //					{
                //						this.list進行文字列.Add( string.Format( "{0} ... {1}/{2}", "Loading score properties from files", CDTXMania.Songs管理_裏読.nNbScoresFromFile, CDTXMania.Songs管理_裏読.nNbScoresFound - cs.nNbScoresFromScoreCache ) );
                //					}
                //-----------------------------
                #endregion
                #region [ 5) 曲リストへの後処理の適用 ]
                //-----------------------------
                //					base.ePhaseID = CStage.EPhase.起動5_曲リストへ後処理を適用する;
                start = DateTime.Now;
                Trace.TraceInformation("enum5) Apply post-processing to song list.");
                Trace.Indent();

                try
                {
                    this.Songs管理.t曲リストへ後処理を適用する();
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.Message);
                    Trace.TraceError(e.StackTrace);
                    Trace.TraceError("An exception has occurred, but processing will continue.");
                }
                finally
                {
                    Trace.TraceInformation("Post-processing to song list completed.");
                    Trace.Unindent();
                    TimeSpan currSpan = (TimeSpan)(DateTime.Now - start);
                    Trace.TraceInformation("Duration of enum5) Apply post-processing to song list: {0}", currSpan.ToString());
                }
                //					lock ( this.list進行文字列 )
                //					{
                //						this.list進行文字列.Add( string.Format( "{0} ... OK", "Building songlists" ) );
                //					}
                //-----------------------------
                #endregion
                #region [ 6) songs.db への保存 ]
                //-----------------------------
                //					base.ePhaseID = CStage.EPhase.起動6_スコアキャッシュをSongsDBに出力する;
                start = DateTime.Now;
                Trace.TraceInformation("enum6) Saving song metadata into songs.db.");
                Trace.Indent();

                try
                {
                    this.Songs管理.tスコアキャッシュをSongsDBに出力する(strPathSongsDB);
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.Message);
                    Trace.TraceError(e.StackTrace);
                    Trace.TraceError("An exception has occurred, but processing will continue.");
                }
                finally
                {
                    Trace.TraceInformation("Saving into songs.db complete.[{0} scores]", this.Songs管理.nNbScoresForSongsDB);
                    Trace.Unindent();
                    TimeSpan currSpan = (TimeSpan)(DateTime.Now - start);
                    Trace.TraceInformation("Duration of enum6) Saving song metadata into songs.db: {0}", currSpan.ToString());
                }
                //					lock ( this.list進行文字列 )
                //					{
                //						this.list進行文字列.Add( string.Format( "{0} ... OK", "Saving songs.db" ) );
                //					}
                #endregion

                //				if ( !bSucceededFastBoot )	// songs2.db読み込みに成功したなら、songs2.dbを新たに作らない
                #region [ 7) songs2.db への保存 ]		// #27060 2012.1.26 yyagi
                start = DateTime.Now;
                Trace.TraceInformation("enum7) Saving additional song data into songlist.db.");
                Trace.Indent();

                SerializeSongList(this.Songs管理, strPathSongList);
                Trace.TraceInformation("Saving into songlist.db complete. [{0} scores]", this.Songs管理.nNbScoresForSongsDB);
                Trace.Unindent();
                TimeSpan span = (TimeSpan)(DateTime.Now - start);
                Trace.TraceInformation("Duration of enum7) Saving additional song data into songlist.db: {0}", span.ToString());
                //-----------------------------
                #endregion
                //				}

            }
            finally
            {
                //				base.ePhaseID = CStage.EPhase.起動7_完了;
                TimeSpan span = (TimeSpan)(DateTime.Now - now);
                Trace.TraceInformation("Duration of full Song Enumerating: {0}", span.ToString());
            }
            lock (this)
            {
                // state = DTXEnumState.Done;		// DoneにするのはCDTXMania.cs側にて。
                state = DTXEnumState.Enumeratad;
            }
        }



        /// <summary>
        /// 曲リストのserialize
        /// </summary>
        private static void SerializeSongList(CSongManager cs, string strPathSongList)
        {
            bool bSucceededSerialize = true;
            Stream output = null;
            try
            {
                output = File.Create(strPathSongList);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(output, cs);
            }
            catch (Exception e)
            {
                bSucceededSerialize = false;
                Trace.TraceError(e.Message);
                Trace.TraceError(e.StackTrace);
                Trace.TraceError("例外が発生しましたが処理を継続します。");
            }
            finally
            {
                output.Close();
                if (!bSucceededSerialize)
                {
                    try
                    {
                        File.Delete(strPathSongList);	// serializeに失敗したら、songs2.dbファイルを消しておく
                    }
                    catch (Exception)
                    {
                        // 特に何もしない
                    }
                }
            }
        }

        /// <summary>
        /// 曲リストのdeserialize
        /// </summary>
        /// <param name="songs管理"></param>
        /// <param name="strPathSongList"></param>
        private async Task<CSongManager> Deserialize(string strPathSongList)
        {
            CSongManager songs管理 = null;
            if (File.Exists(strPathSongList))
            {
                await Task.Run(delegate 
                {
                    try
                    {
                        #region [ SongListDB(songlist.db)を読み込む ]
                        //	byte[] buf = File.ReadAllBytes( SongListDBファイル名 );			// 一旦メモリにまとめ読みしてからdeserializeした方が高速かと思ったら全く変わらなかったので削除
                        //	using ( MemoryStream input = new MemoryStream(buf, false) )
                        using (Stream input = File.OpenRead(strPathSongList))
                        {
                            try
                            {
                                BinaryFormatter formatter = new BinaryFormatter();
                                songs管理 = (CSongManager)formatter.Deserialize(input);
                            }
                            catch (Exception)
                            {
                                // songs管理 = null;
                            }
                        }
                        #endregion
                    }
                    catch
                    {
                        Trace.TraceError("Fail to load songlist.db");
                    }
                });
            }
            else 
            {
                Trace.TraceInformation("songlist.db does not exist");
            }

            return songs管理;
        }
    }
}
