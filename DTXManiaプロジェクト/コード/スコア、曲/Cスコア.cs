using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace DTXMania
{
    [Serializable]
    internal class CScore
    {
        // プロパティ

        public STScoreIniInformation ScoreIniInformation;
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct STScoreIniInformation
        {
            public DateTime LastModified;
            public long ファイルサイズ;

            public STScoreIniInformation(DateTime 最終更新日時, long ファイルサイズ)
            {
                this.LastModified = 最終更新日時;
                this.ファイルサイズ = ファイルサイズ;
            }
        }

        public STFileInformation FileInformation;
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct STFileInformation
        {
            public string AbsoluteFilePath;
            public string AbsoluteFolderPath;
            public DateTime LastModified;
            public long FileSize;

            public STFileInformation(string AbsoluteFilePath, string AbsoluteFolderPath, DateTime LastModified, long FileSize)
            {
                this.AbsoluteFilePath = AbsoluteFilePath;
                this.AbsoluteFolderPath = AbsoluteFolderPath;
                this.LastModified = LastModified;
                this.FileSize = FileSize;
            }
        }

        public STMusicInformation SongInformation;
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct STMusicInformation
        {
            public string Title;
            public string ArtistName;
            public string Comment;
            public string Genre;
            public string Preimage;
            public string Premovie;
            public string Presound;
            public string Backgound;
            public STDGBVALUE<int> Level;
            public STDGBVALUE<int> LevelDec;
            public STRANK BestRank;
            public STSKILL HighSkill;
            public STSKILL HighSongSkill;
            public STDGBVALUE<bool> FullCombo;
            public STDGBVALUE<int> NbPerformances;
            public STHISTORY PerformanceHistory;
            public bool bHiddenLevel;
            public CDTX.EType SongType;
            public double Bpm;
            public int Duration;
            public STDGBVALUE<bool> b完全にCLASSIC譜面である;
            public STDGBVALUE<bool> bScoreExists;

            [Serializable]
            [StructLayout(LayoutKind.Sequential)]
            public struct STHISTORY
            {
                public string 行1;
                public string 行2;
                public string 行3;
                public string 行4;
                public string 行5;
                public string this[int index]
                {
                    get
                    {
                        switch (index)
                        {
                            case 0:
                                return this.行1;

                            case 1:
                                return this.行2;

                            case 2:
                                return this.行3;

                            case 3:
                                return this.行4;

                            case 4:
                                return this.行5;
                        }
                        throw new IndexOutOfRangeException();
                    }
                    set
                    {
                        switch (index)
                        {
                            case 0:
                                this.行1 = value;
                                return;

                            case 1:
                                this.行2 = value;
                                return;

                            case 2:
                                this.行3 = value;
                                return;

                            case 3:
                                this.行4 = value;
                                return;

                            case 4:
                                this.行5 = value;
                                return;
                        }
                        throw new IndexOutOfRangeException();
                    }
                }
            }

            [Serializable]
            [StructLayout(LayoutKind.Sequential)]
            public struct STRANK
            {
                public int Drums;
                public int Guitar;
                public int Bass;
                public int this[int index]
                {
                    get
                    {
                        switch (index)
                        {
                            case 0:
                                return this.Drums;

                            case 1:
                                return this.Guitar;

                            case 2:
                                return this.Bass;
                        }
                        throw new IndexOutOfRangeException();
                    }
                    set
                    {
                        if ((value < (int)CScoreIni.ERANK.SS) || ((value != (int)CScoreIni.ERANK.UNKNOWN) && (value > (int)CScoreIni.ERANK.E)))
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                        switch (index)
                        {
                            case 0:
                                this.Drums = value;
                                return;

                            case 1:
                                this.Guitar = value;
                                return;

                            case 2:
                                this.Bass = value;
                                return;
                        }
                        throw new IndexOutOfRangeException();
                    }
                }
            }

            [Serializable]
            [StructLayout(LayoutKind.Sequential)]
            public struct STSKILL
            {
                public double Drums;
                public double Guitar;
                public double Bass;
                public double this[int index]
                {
                    get
                    {
                        switch (index)
                        {
                            case 0:
                                return this.Drums;

                            case 1:
                                return this.Guitar;

                            case 2:
                                return this.Bass;
                        }
                        throw new IndexOutOfRangeException();
                    }
                    set
                    {
                        if ((value < 0.0) || (value > 200.0))
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                        switch (index)
                        {
                            case 0:
                                this.Drums = value;
                                return;

                            case 1:
                                this.Guitar = value;
                                return;

                            case 2:
                                this.Bass = value;
                                return;
                        }
                        throw new IndexOutOfRangeException();
                    }
                }
            }
        }

        public bool bHadACacheInSongDB;
        public bool bスコアが有効である
        {
            get
            {
                return (((this.SongInformation.Level[0] + this.SongInformation.Level[1]) + this.SongInformation.Level[2]) != 0);
            }
        }


        // Constructor

        public CScore()
        {
            this.ScoreIniInformation = new STScoreIniInformation(DateTime.MinValue, 0L);
            this.bHadACacheInSongDB = false;
            this.FileInformation = new STFileInformation("", "", DateTime.MinValue, 0L);
            this.SongInformation = new STMusicInformation();
            this.SongInformation.Title = "";
            this.SongInformation.ArtistName = "";
            this.SongInformation.Comment = "";
            this.SongInformation.Genre = "";
            this.SongInformation.Preimage = "";
            this.SongInformation.Premovie = "";
            this.SongInformation.Presound = "";
            this.SongInformation.Backgound = "";
            this.SongInformation.Level = new STDGBVALUE<int>();
            this.SongInformation.LevelDec = new STDGBVALUE<int>();
            this.SongInformation.BestRank = new STMusicInformation.STRANK();
            this.SongInformation.BestRank.Drums = (int)CScoreIni.ERANK.UNKNOWN;
            this.SongInformation.BestRank.Guitar = (int)CScoreIni.ERANK.UNKNOWN;
            this.SongInformation.BestRank.Bass = (int)CScoreIni.ERANK.UNKNOWN;
            this.SongInformation.FullCombo = new STDGBVALUE<bool>();
            this.SongInformation.NbPerformances = new STDGBVALUE<int>();
            this.SongInformation.PerformanceHistory = new STMusicInformation.STHISTORY();
            this.SongInformation.PerformanceHistory.行1 = "";
            this.SongInformation.PerformanceHistory.行2 = "";
            this.SongInformation.PerformanceHistory.行3 = "";
            this.SongInformation.PerformanceHistory.行4 = "";
            this.SongInformation.PerformanceHistory.行5 = "";
            this.SongInformation.bHiddenLevel = false;
            this.SongInformation.HighSkill = new STMusicInformation.STSKILL();
            this.SongInformation.HighSongSkill = new STMusicInformation.STSKILL();
            this.SongInformation.SongType = CDTX.EType.DTX;
            this.SongInformation.Bpm = 120.0;
            this.SongInformation.Duration = 0;
            this.SongInformation.b完全にCLASSIC譜面である.Drums = false;
            this.SongInformation.b完全にCLASSIC譜面である.Guitar = false;
            this.SongInformation.b完全にCLASSIC譜面である.Bass = false;
            this.SongInformation.bScoreExists.Drums = false;
            this.SongInformation.bScoreExists.Guitar = false;
            this.SongInformation.bScoreExists.Bass = false;
        }
    }
}
