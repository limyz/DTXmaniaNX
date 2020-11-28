using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using FDK;

namespace DTXMania
{
	public class CScoreIni
	{
		// プロパティ

		// [File] セクション
		public STFile stFile;
		[StructLayout( LayoutKind.Sequential )]
		public struct STFile
		{
			public string Title;
			public string Name;
			public string Hash;
			public int PlayCountDrums;
			public int PlayCountGuitar;
            public int PlayCountBass;
            // #23596 10.11.16 add ikanick-----/
            public int ClearCountDrums;
            public int ClearCountGuitar;
            public int ClearCountBass;
            // #24459 2011.2.24 yyagi----------/
			public STDGBVALUE<int> BestRank;
			// --------------------------------/
			public int HistoryCount;
			public string[] History;
			public int BGMAdjust;
		}

		// 演奏記録セクション（9種類）
		public STSection stSection;
		[StructLayout( LayoutKind.Sequential )]
		public struct STSection
		{
            public CScoreIni.CPerformanceEntry HiScoreDrums;
            public CScoreIni.CPerformanceEntry HiSkillDrums;
			public CScoreIni.CPerformanceEntry HiScoreGuitar;
            public CScoreIni.CPerformanceEntry HiSkillGuitar;
			public CScoreIni.CPerformanceEntry HiScoreBass;
            public CScoreIni.CPerformanceEntry HiSkillBass;
            public CScoreIni.CPerformanceEntry LastPlayDrums;   // #23595 2011.1.9 ikanick
            public CScoreIni.CPerformanceEntry LastPlayGuitar;  //
            public CScoreIni.CPerformanceEntry LastPlayBass;    //
			public CScoreIni.CPerformanceEntry this[ int index ]
			{
				get
				{
					switch( index )
					{
						case 0:
							return this.HiScoreDrums;

						case 1:
							return this.HiSkillDrums;

						case 2:
							return this.HiScoreGuitar;

						case 3:
							return this.HiSkillGuitar;

						case 4:
							return this.HiScoreBass;

                        case 5:
                            return this.HiSkillBass;

                        // #23595 2011.1.9 ikanick
                        case 6:
                            return this.LastPlayDrums;

                        case 7:
                            return this.LastPlayGuitar;

                        case 8:
                            return this.LastPlayBass;
                        //------------
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case 0:
							this.HiScoreDrums = value;
							return;

						case 1:
							this.HiSkillDrums = value;
							return;

						case 2:
							this.HiScoreGuitar = value;
							return;

						case 3:
							this.HiSkillGuitar = value;
							return;

						case 4:
							this.HiScoreBass = value;
                            return;

                        case 5:
                            this.HiSkillBass = value;
                            return;
                        // #23595 2011.1.9 ikanick
                        case 6:
                            this.LastPlayDrums = value;
                            return;

                        case 7:
                            this.LastPlayGuitar = value;
                            return;

                        case 8:
                            this.LastPlayBass = value;
                            return;
                        //------------------
					}
					throw new IndexOutOfRangeException();
				}
			}
		}
		public enum ESectionType : int
		{
			Unknown = -2,
			File = -1,
			HiScoreDrums = 0,
			HiSkillDrums = 1,
			HiScoreGuitar = 2,
			HiSkillGuitar = 3,
			HiScoreBass = 4,
			HiSkillBass = 5,
			LastPlayDrums = 6,  // #23595 2011.1.9 ikanick
			LastPlayGuitar = 7, //
			LastPlayBass = 8,   //
		}
		public enum ERANK : int		// #24459 yyagi
		{
			SS = 0,
			S = 1,
			A = 2,
			B = 3,
			C = 4,
			D = 5,
			E = 6,
			UNKNOWN = 99
		}
		public class CPerformanceEntry
		{
			public STAUTOPLAY bAutoPlay;
			public bool bDrumsEnabled;
			public bool bGuitarEnabled;
			public STDGBVALUE<bool> bHidden;
			public STDGBVALUE<bool> bLeft;
			public STDGBVALUE<bool> bLight;
			public STDGBVALUE<bool> bReverse;
			public bool bSTAGEFAILEDEnabled;
			public STDGBVALUE<bool> bSudden;
			public bool bTight;
			public bool bMIDIUsed;
			public bool bKeyboardUsed;
			public bool bJoypadUsed;
			public bool bMouseUsed;
			public double dbGameSkill;
			public double dbPerformanceSkill;
			public ECYGroup eCYGroup;
			public EDarkMode eDark;
			public EFTGroup eFTGroup;
			public EHHGroup eHHGroup;
            public EBDGroup eBDGroup;
			public EPlaybackPriority eHitSoundPriorityCY;
			public EPlaybackPriority eHitSoundPriorityFT;
			public EPlaybackPriority eHitSoundPriorityHH;
			public STDGBVALUE<ERandomMode> eRandom;
			public EDamageLevel eDamageLevel;
			public STDGBVALUE<float> fScrollSpeed;
			public string Hash;

			/// <summary>
			/// The primary <see cref="CHitRanges"/> used to achieve the score.
			/// </summary>
			/// <remarks>
			/// For drums, "primary" refers to all non-pedal chips. <br/>
			/// For guitar and bass guitar, this refers to all chips.
			/// </remarks>
			public CHitRanges PrimaryHitRanges;

			/// <summary>
			/// The secondary <see cref="CHitRanges"/> used to achieve the score.
			/// </summary>
			/// <remarks>
			/// For drums, "secondary" refers to all pedal chips. <br/>
			/// For guitar and bass guitar, this is unused.
			/// </remarks>
			public CHitRanges SecondaryHitRanges;

			public int nGoodCount;
			public int nGreatCount;
			public int nMissCount;
			public int nPerfectCount;
			public int nPoorCount;
			public int nPerfectCount_ExclAuto;
			public int nGreatCount_ExclAuto;
			public int nGoodCount_ExclAuto;
			public int nPoorCount_ExclAuto;
			public int nMissCount_ExclAuto;
			public long nスコア;
			public int nPlaySpeedNumerator;
			public int nPlaySpeedDenominator;
			public int nMaxCombo;
			public int nTotalChipsCount;
			public string strDTXManiaVersion;
			public bool レーン9モード;
			public int nRisky;		// #23559 2011.6.20 yyagi 0=OFF, 1-10=Risky
			public string strDateTime;

			public CPerformanceEntry()
			{
				this.bAutoPlay = new STAUTOPLAY();
				this.bAutoPlay.LC = false;
				this.bAutoPlay.HH = false;
				this.bAutoPlay.SD = false;
				this.bAutoPlay.BD = false;
				this.bAutoPlay.HT = false;
				this.bAutoPlay.LT = false;
				this.bAutoPlay.FT = false;
				this.bAutoPlay.CY = false;
                this.bAutoPlay.LP = false;
                this.bAutoPlay.LBD = false;
				this.bAutoPlay.Guitar = false;
				this.bAutoPlay.Bass = false;
				this.bAutoPlay.GtR = false;
				this.bAutoPlay.GtG = false;
				this.bAutoPlay.GtB = false;
                this.bAutoPlay.GtY = false;
                this.bAutoPlay.GtP = false;
				this.bAutoPlay.GtPick = false;
				this.bAutoPlay.GtW = false;
				this.bAutoPlay.BsR = false;
				this.bAutoPlay.BsG = false;
				this.bAutoPlay.BsB = false;
                this.bAutoPlay.BsY = false;
                this.bAutoPlay.BsP = false;
				this.bAutoPlay.BsPick = false;
				this.bAutoPlay.BsW = false;

				this.bSudden = new STDGBVALUE<bool>();
				this.bSudden.Drums = false;
				this.bSudden.Guitar = false;
				this.bSudden.Bass = false;
				this.bHidden = new STDGBVALUE<bool>();
				this.bHidden.Drums = false;
				this.bHidden.Guitar = false;
				this.bHidden.Bass = false;
				this.bReverse = new STDGBVALUE<bool>();
				this.bReverse.Drums = false;
				this.bReverse.Guitar = false;
				this.bReverse.Bass = false;
				this.eRandom = new STDGBVALUE<ERandomMode>();
				this.eRandom.Drums = ERandomMode.OFF;
				this.eRandom.Guitar = ERandomMode.OFF;
				this.eRandom.Bass = ERandomMode.OFF;
				this.bLight = new STDGBVALUE<bool>();
				this.bLight.Drums = false;
				this.bLight.Guitar = false;
				this.bLight.Bass = false;
				this.bLeft = new STDGBVALUE<bool>();
				this.bLeft.Drums = false;
				this.bLeft.Guitar = false;
				this.bLeft.Bass = false;
				this.fScrollSpeed = new STDGBVALUE<float>();
				this.fScrollSpeed.Drums = 1f;
				this.fScrollSpeed.Guitar = 1f;
				this.fScrollSpeed.Bass = 1f;
				this.nPlaySpeedNumerator = 20;
				this.nPlaySpeedDenominator = 20;
				this.bGuitarEnabled = true;
				this.bDrumsEnabled = true;
				this.bSTAGEFAILEDEnabled = true;
				this.eDamageLevel = EDamageLevel.Normal;
				PrimaryHitRanges = CHitRanges.tCreateDTXHitRanges();
				SecondaryHitRanges = CHitRanges.tCreateDTXHitRanges();
				this.strDTXManiaVersion = "Unknown";
				this.strDateTime = "";
				this.Hash = "00000000000000000000000000000000";
				this.レーン9モード = true;
				this.nRisky = 0;									// #23559 2011.6.20 yyagi
			}

			public bool bフルコンボじゃない
			{
				get
				{
					return !this.bIsFullCombo;
				}
			}
			public bool bIsFullCombo
			{
				get
				{
					return ( ( this.nMaxCombo > 0 ) && ( this.nMaxCombo == ( this.nPerfectCount + this.nGreatCount + this.nGoodCount + this.nPoorCount + this.nMissCount ) ) );
				}
			}

			public bool b全AUTOじゃない
			{
				get
				{
					return !b全AUTOである;
				}
			}
			public bool b全AUTOである
			{
				get
				{
					return (this.nTotalChipsCount - this.nPerfectCount_ExclAuto - this.nGreatCount_ExclAuto - this.nGoodCount_ExclAuto - this.nPoorCount_ExclAuto - this.nMissCount_ExclAuto) == this.nTotalChipsCount;
				}
			}
#if false
			[StructLayout( LayoutKind.Sequential )]
			public struct STAUTOPLAY
			{
				public bool LC;
				public bool HH;
				public bool SD;
				public bool BD;
				public bool HT;
				public bool LT;
				public bool FT;
				public bool CY;
				public bool RD;
				public bool Guitar;
				public bool Bass;
				public bool GtR;
				public bool GtG;
				public bool GtB;
				public bool GtPick;
				public bool GtW;
				public bool BsR;
				public bool BsG;
				public bool BsB;
				public bool BsPick;
				public bool BsW;
				public bool this[ int index ]
				{
					get
					{
						switch ( index )
						{
							case (int) Eレーン.LC:
								return this.LC;
							case (int) Eレーン.HH:
								return this.HH;
							case (int) Eレーン.SD:
								return this.SD;
							case (int) Eレーン.BD:
								return this.BD;
							case (int) Eレーン.HT:
								return this.HT;
							case (int) Eレーン.LT:
								return this.LT;
							case (int) Eレーン.FT:
								return this.FT;
							case (int) Eレーン.CY:
								return this.CY;
							case (int) Eレーン.RD:
								return this.RD;
							case (int) Eレーン.Guitar:
								return this.Guitar;
							case (int) Eレーン.Bass:
								return this.Bass;
							case (int) Eレーン.GtR:
								return this.GtR;
							case (int) Eレーン.GtG:
								return this.GtG;
							case (int) Eレーン.GtB:
								return this.GtB;
							case (int) Eレーン.GtPick:
								return this.GtPick;
							case (int) Eレーン.GtW:
								return this.GtW;
							case (int) Eレーン.BsR:
								return this.BsR;
							case (int) Eレーン.BsG:
								return this.BsG;
							case (int) Eレーン.BsB:
								return this.BsB;
							case (int) Eレーン.BsPick:
								return this.BsPick;
							case (int) Eレーン.BsW:
								return this.BsW;
						}
						throw new IndexOutOfRangeException();
					}
					set
					{
						switch ( index )
						{
							case (int) Eレーン.LC:
								this.LC = value;
								return;
							case (int) Eレーン.HH:
								this.HH = value;
								return;
							case (int) Eレーン.SD:
								this.SD = value;
								return;
							case (int) Eレーン.BD:
								this.BD = value;
								return;
							case (int) Eレーン.HT:
								this.HT = value;
								return;
							case (int) Eレーン.LT:
								this.LT = value;
								return;
							case (int) Eレーン.FT:
								this.FT = value;
								return;
							case (int) Eレーン.CY:
								this.CY = value;
								return;
							case (int) Eレーン.RD:
								this.RD = value;
								return;
							case (int) Eレーン.Guitar:
								this.Guitar = value;
								return;
							case (int) Eレーン.Bass:
								this.Bass = value;
								return;
							case (int) Eレーン.GtR:
								this.GtR = value;
								return;
							case (int) Eレーン.GtG:
								this.GtG = value;
								return;
							case (int) Eレーン.GtB:
								this.GtB = value;
								return;
							case (int) Eレーン.GtPick:
								this.GtPick = value;
								return;
							case (int) Eレーン.GtW:
								this.GtW = value;
								return;
							case (int) Eレーン.BsR:
								this.BsR = value;
								return;
							case (int) Eレーン.BsG:
								this.BsG = value;
								return;
							case (int) Eレーン.BsB:
								this.BsB = value;
								return;
							case (int) Eレーン.BsPick:
								this.BsPick = value;
								return;
							case (int) Eレーン.BsW:
								this.BsW = value;
								return;
						}
						throw new IndexOutOfRangeException();
					}
				}
			}
#endif
		}

		/// <summary>
		/// <para>.score.ini の存在するフォルダ（絶対パス；末尾に '\' はついていない）。</para>
		/// <para>未保存などでファイル名がない場合は null。</para>
		/// </summary>
		public string iniFileDirectoryName
		{
			get;
			private set;
		}

		/// <summary>
		/// <para>.score.ini のファイル名（絶対パス）。</para>
		/// <para>未保存などでファイル名がない場合は null。</para>
		/// </summary>
		public string iniFilename
		{
			get; 
			private set;
		}


		// コンストラクタ

		public CScoreIni()
		{
			this.iniFileDirectoryName = null;
			this.iniFilename = null;
			this.stFile = new STFile();
			stFile.Title = "";
			stFile.Name = "";
			stFile.Hash = "";
			stFile.History = new string[] { "", "", "", "", "" };
			stFile.BestRank.Drums =  (int)ERANK.UNKNOWN;		// #24459 2011.2.24 yyagi
			stFile.BestRank.Guitar = (int)ERANK.UNKNOWN;		//
			stFile.BestRank.Bass =   (int)ERANK.UNKNOWN;		//
	
			this.stSection = new STSection();
			stSection.HiScoreDrums = new CPerformanceEntry();
			stSection.HiSkillDrums = new CPerformanceEntry();
			stSection.HiScoreGuitar = new CPerformanceEntry();
            stSection.HiSkillGuitar = new CPerformanceEntry();
            stSection.HiScoreBass = new CPerformanceEntry();
            stSection.HiSkillBass = new CPerformanceEntry();
            stSection.LastPlayDrums = new CPerformanceEntry();
            stSection.LastPlayGuitar = new CPerformanceEntry();
            stSection.LastPlayBass = new CPerformanceEntry();
		}

		/// <summary>
		/// <para>初期化後にiniファイルを読み込むコンストラクタ。</para>
		/// <para>読み込んだiniに不正値があれば、それが含まれるセクションをリセットする。</para>
		/// </summary>
		public CScoreIni( string str読み込むiniファイル )
			: this()
		{
			this.tRead( str読み込むiniファイル );
			this.tCheckIntegrity();
		}


		// メソッド

		/// <summary>
		/// <para>現在の this.Record[] オブジェクトの、指定されたセクションの情報が正当であるか否かを判定する。
		/// 真偽どちらでも、その内容は書き換えない。</para>
		/// </summary>
		/// <param name="eセクション">判定するセクション。</param>
		/// <returns>正当である（整合性がある）場合は true。</returns>
		public bool bCheckConsistency( ESectionType eセクション )
		{
			return true;	// オープンソース化に伴い、整合性チェックを無効化。（2010.10.21）
		}
		
		/// <summary>
		/// 指定されたファイルの内容から MD5 値を求め、それを16進数に変換した文字列を返す。
		/// </summary>
		/// <param name="ファイル名">MD5 を求めるファイル名。</param>
		/// <returns>算出結果の MD5 を16進数で並べた文字列。</returns>
		public static string tComputeFileMD5( string ファイル名 )
		{
			byte[] buffer = null;
			FileStream stream = new FileStream( ファイル名, FileMode.Open, FileAccess.Read );
			buffer = new byte[ stream.Length ];
			stream.Read( buffer, 0, (int) stream.Length );
			stream.Close();
			StringBuilder builder = new StringBuilder(0x21);
			{
				MD5CryptoServiceProvider m = new MD5CryptoServiceProvider();
				byte[] buffer2 = m.ComputeHash(buffer);
				foreach (byte num in buffer2)
					builder.Append(num.ToString("x2"));
			}
			return builder.ToString();
		}
		
		/// <summary>
		/// 指定された .score.ini を読み込む。内容の真偽は判定しない。
		/// </summary>
		/// <param name="iniファイル名">読み込む .score.ini ファイルを指定します（絶対パスが安全）。</param>
		public void tRead( string iniファイル名 )
		{
			this.iniFileDirectoryName = Path.GetDirectoryName( iniファイル名 );
			this.iniFilename = Path.GetFileName( iniファイル名 );

			ESectionType section = ESectionType.Unknown;
			if( File.Exists( iniファイル名 ) )
			{
				string str;
				StreamReader reader = new StreamReader( iniファイル名, Encoding.GetEncoding( "shift-jis" ) );
				while( ( str = reader.ReadLine() ) != null )
				{
					str = str.Replace( '\t', ' ' ).TrimStart( new char[] { '\t', ' ' } );
					if( ( str.Length != 0 ) && ( str[ 0 ] != ';' ) )
					{
						try
						{
							string item;
							string para;
							CPerformanceEntry cPerformanceEntry;
							#region [ section ]
							if ( str[ 0 ] == '[' )
							{
								StringBuilder builder = new StringBuilder( 0x20 );
								int num = 1;
								while( ( num < str.Length ) && ( str[ num ] != ']' ) )
								{
									builder.Append( str[ num++ ] );
								}
								string str2 = builder.ToString();
								if( str2.Equals( "File" ) )
								{
									section = ESectionType.File;
								}
								else if( str2.Equals( "HiScore.Drums" ) )
								{
									section = ESectionType.HiScoreDrums;
								}
								else if( str2.Equals( "HiSkill.Drums" ) )
								{
									section = ESectionType.HiSkillDrums;
								}
								else if( str2.Equals( "HiScore.Guitar" ) )
								{
									section = ESectionType.HiScoreGuitar;
								}
								else if( str2.Equals( "HiSkill.Guitar" ) )
								{
									section = ESectionType.HiSkillGuitar;
								}
								else if( str2.Equals( "HiScore.Bass" ) )
								{
									section = ESectionType.HiScoreBass;
                                }
                                else if (str2.Equals("HiSkill.Bass"))
                                {
                                    section = ESectionType.HiSkillBass;
                                }
                                // #23595 2011.1.9 ikanick
                                else if (str2.Equals("LastPlay.Drums"))
                                {
                                    section = ESectionType.LastPlayDrums;
                                }
                                else if (str2.Equals("LastPlay.Guitar"))
                                {
                                    section = ESectionType.LastPlayGuitar;
                                }
                                else if (str2.Equals("LastPlay.Bass"))
                                {
                                    section = ESectionType.LastPlayBass;
                                }
                                //----------------------------------------------------
								else
								{
									section = ESectionType.Unknown;
								}
							}
							#endregion
							else
							{
								string[] strArray = str.Split( new char[] { '=' } );
								if( strArray.Length == 2 )
								{
									item = strArray[ 0 ].Trim();
									para = strArray[ 1 ].Trim();
									switch( section )
									{
										case ESectionType.File:
											{
												if( !item.Equals( "Title" ) )
												{
													goto Label_01C7;
												}
												this.stFile.Title = para;
												continue;
											}
										case ESectionType.HiScoreDrums:
										case ESectionType.HiSkillDrums:
										case ESectionType.HiScoreGuitar:
										case ESectionType.HiSkillGuitar:
										case ESectionType.HiScoreBass:
                                        case ESectionType.HiSkillBass:
                                        case ESectionType.LastPlayDrums:// #23595 2011.1.9 ikanick
                                        case ESectionType.LastPlayGuitar:
                                        case ESectionType.LastPlayBass:
											{
												cPerformanceEntry = this.stSection[ (int) section ];
												if( !item.Equals( "Score" ) )
												{
													goto Label_03B9;
												}
												cPerformanceEntry.nスコア = long.Parse( para );
												continue;
											}
									}
								}
							}
							continue;
							#region [ File section ]
						Label_01C7:
							if( item.Equals( "Name" ) )
							{
								this.stFile.Name = para;
							}
							else if( item.Equals( "Hash" ) )
							{
								this.stFile.Hash = para;
							}
							else if( item.Equals( "PlayCountDrums" ) )
							{
								this.stFile.PlayCountDrums = CConversion.nGetNumberIfInRange( para, 0, 99999999, 0 );
							}
							else if( item.Equals( "PlayCountGuitars" ) )// #23596 11.2.5 changed ikanick
							{
								this.stFile.PlayCountGuitar = CConversion.nGetNumberIfInRange( para, 0, 99999999, 0 );
							}
							else if( item.Equals( "PlayCountBass" ) )
							{
								this.stFile.PlayCountBass = CConversion.nGetNumberIfInRange( para, 0, 99999999, 0 );
                            }
                            // #23596 10.11.16 add ikanick------------------------------------/
                            else if (item.Equals("ClearCountDrums"))
                            {
                                this.stFile.ClearCountDrums = CConversion.nGetNumberIfInRange(para, 0, 99999999, 0);
                            }
                            else if (item.Equals("ClearCountGuitars"))// #23596 11.2.5 changed ikanick
                            {
                                this.stFile.ClearCountGuitar = CConversion.nGetNumberIfInRange(para, 0, 99999999, 0);
                            }
                            else if (item.Equals("ClearCountBass"))
                            {
                                this.stFile.ClearCountBass = CConversion.nGetNumberIfInRange(para, 0, 99999999, 0);
                            }
                            // #24459 2011.2.24 yyagi-----------------------------------------/
							else if ( item.Equals( "BestRankDrums" ) )
							{
								this.stFile.BestRank.Drums = CConversion.nGetNumberIfInRange( para, (int) ERANK.SS, (int) ERANK.E, (int) ERANK.UNKNOWN );
							}
							else if ( item.Equals( "BestRankGuitar" ) )
							{
								this.stFile.BestRank.Guitar = CConversion.nGetNumberIfInRange( para, (int) ERANK.SS, (int) ERANK.E, (int) ERANK.UNKNOWN );
							}
							else if ( item.Equals( "BestRankBass" ) )
							{
								this.stFile.BestRank.Bass = CConversion.nGetNumberIfInRange( para, (int) ERANK.SS, (int) ERANK.E, (int) ERANK.UNKNOWN );
							}
							//----------------------------------------------------------------/
							else if ( item.Equals( "History0" ) )
							{
								this.stFile.History[ 0 ] = para;
							}
							else if( item.Equals( "History1" ) )
							{
								this.stFile.History[ 1 ] = para;
							}
							else if( item.Equals( "History2" ) )
							{
								this.stFile.History[ 2 ] = para;
							}
							else if( item.Equals( "History3" ) )
							{
								this.stFile.History[ 3 ] = para;
							}
							else if( item.Equals( "History4" ) )
							{
								this.stFile.History[ 4 ] = para;
							}
							else if( item.Equals( "HistoryCount" ) )
							{
								this.stFile.HistoryCount = CConversion.nGetNumberIfInRange( para, 0, 99999999, 0 );
							}
							else if( item.Equals( "BGMAdjust" ) )
							{
								this.stFile.BGMAdjust = CConversion.nStringToInt( para, 0 );
							}
							continue;
							#endregion
							#region [ Score section ]
						Label_03B9:
							if( item.Equals( "PlaySkill" ) )
							{
								cPerformanceEntry.dbPerformanceSkill = (double) decimal.Parse( para );
							}
							else if( item.Equals( "Skill" ) )
							{
								cPerformanceEntry.dbGameSkill = (double) decimal.Parse( para );
							}
							else if( item.Equals( "Perfect" ) )
							{
								cPerformanceEntry.nPerfectCount = int.Parse( para );
							}
							else if( item.Equals( "Great" ) )
							{
								cPerformanceEntry.nGreatCount = int.Parse( para );
							}
							else if( item.Equals( "Good" ) )
							{
								cPerformanceEntry.nGoodCount = int.Parse( para );
							}
							else if( item.Equals( "Poor" ) )
							{
								cPerformanceEntry.nPoorCount = int.Parse( para );
							}
							else if( item.Equals( "Miss" ) )
							{
								cPerformanceEntry.nMissCount = int.Parse( para );
							}
							else if( item.Equals( "MaxCombo" ) )
							{
								cPerformanceEntry.nMaxCombo = int.Parse( para );
							}
							else if( item.Equals( "TotalChips" ) )
							{
								cPerformanceEntry.nTotalChipsCount = int.Parse( para );
							}
							else if( item.Equals( "AutoPlay" ) )
							{
								// LCなし               LCあり               CYとRDが別           Gt/Bs autolane/pick
								if( para.Length == 9 || para.Length == 10 || para.Length == 11 || para.Length == 21 )
								{
									for( int i = 0; i < para.Length; i++ )
									{
										cPerformanceEntry.bAutoPlay[ i ] = this.ONorOFF( para[ i ] );
									}
								}
							}
							else if ( item.Equals( "Risky" ) )
							{
								cPerformanceEntry.nRisky = int.Parse( para );
							}
							else if ( item.Equals( "TightDrums" ) )
							{
								cPerformanceEntry.bTight = CConversion.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "SuddenDrums" ) )
							{
								cPerformanceEntry.bSudden.Drums = CConversion.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "SuddenGuitar" ) )
							{
								cPerformanceEntry.bSudden.Guitar = CConversion.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "SuddenBass" ) )
							{
								cPerformanceEntry.bSudden.Bass = CConversion.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "HiddenDrums" ) )
							{
								cPerformanceEntry.bHidden.Drums = CConversion.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "HiddenGuitar" ) )
							{
								cPerformanceEntry.bHidden.Guitar = CConversion.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "HiddenBass" ) )
							{
								cPerformanceEntry.bHidden.Bass = CConversion.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "ReverseDrums" ) )
							{
								cPerformanceEntry.bReverse.Drums = CConversion.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "ReverseGuitar" ) )
							{
								cPerformanceEntry.bReverse.Guitar = CConversion.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "ReverseBass" ) )
							{
								cPerformanceEntry.bReverse.Bass = CConversion.bONorOFF( para[ 0 ] );
							}
							#endregion
							else
							{
								#region [ RandomGuitar ]
								if ( item.Equals( "RandomGuitar" ) )
								{
									switch ( int.Parse( para ) )
									{
										case (int) ERandomMode.OFF:
											{
												cPerformanceEntry.eRandom.Guitar = ERandomMode.OFF;
												continue;
											}
										case (int) ERandomMode.RANDOM:
											{
												cPerformanceEntry.eRandom.Guitar = ERandomMode.RANDOM;
												continue;
											}
										case (int) ERandomMode.SUPERRANDOM:
											{
												cPerformanceEntry.eRandom.Guitar = ERandomMode.SUPERRANDOM;
												continue;
											}
										case (int) ERandomMode.HYPERRANDOM:		// #25452 2011.6.20 yyagi
											{
												cPerformanceEntry.eRandom.Guitar = ERandomMode.SUPERRANDOM;
												continue;
											}
									}
									throw new Exception( "RandomGuitar の値が無効です。" );
								}
								#endregion
								#region [ RandomBass ]
								if ( item.Equals( "RandomBass" ) )
								{
									switch ( int.Parse( para ) )
									{
										case (int) ERandomMode.OFF:
											{
												cPerformanceEntry.eRandom.Bass = ERandomMode.OFF;
												continue;
											}
										case (int) ERandomMode.RANDOM:
											{
												cPerformanceEntry.eRandom.Bass = ERandomMode.RANDOM;
												continue;
											}
										case (int) ERandomMode.SUPERRANDOM:
											{
												cPerformanceEntry.eRandom.Bass = ERandomMode.SUPERRANDOM;
												continue;
											}
										case (int) ERandomMode.HYPERRANDOM:		// #25452 2011.6.20 yyagi
											{
												cPerformanceEntry.eRandom.Bass = ERandomMode.SUPERRANDOM;
												continue;
											}
									}
									throw new Exception( "RandomBass の値が無効です。" );
								}
								#endregion
								#region [ LightGuitar ]
								if ( item.Equals( "LightGuitar" ) )
								{
									cPerformanceEntry.bLight.Guitar = CConversion.bONorOFF( para[ 0 ] );
								}
								#endregion
								#region [ LightBass ]
								else if ( item.Equals( "LightBass" ) )
								{
									cPerformanceEntry.bLight.Bass = CConversion.bONorOFF( para[ 0 ] );
								}
								#endregion
								#region [ LeftGuitar ]
								else if ( item.Equals( "LeftGuitar" ) )
								{
									cPerformanceEntry.bLeft.Guitar = CConversion.bONorOFF( para[ 0 ] );
								}
								#endregion
								#region [ LeftBass ]
								else if ( item.Equals( "LeftBass" ) )
								{
									cPerformanceEntry.bLeft.Bass = CConversion.bONorOFF( para[ 0 ] );
								}
								#endregion
								else
								{
									#region [ Dark ]
									if ( item.Equals( "Dark" ) )
									{
										switch ( int.Parse( para ) )
										{
											case 0:
												{
													cPerformanceEntry.eDark = EDarkMode.OFF;
													continue;
												}
											case 1:
												{
													cPerformanceEntry.eDark = EDarkMode.HALF;
													continue;
												}
											case 2:
												{
													cPerformanceEntry.eDark = EDarkMode.FULL;
													continue;
												}
										}
										throw new Exception( "Dark の値が無効です。" );
									}
									#endregion
									#region [ ScrollSpeedDrums ]
									if ( item.Equals( "ScrollSpeedDrums" ) )
									{
										cPerformanceEntry.fScrollSpeed.Drums = (float) decimal.Parse( para );
									}
									#endregion
									#region [ ScrollSpeedGuitar ]
									else if ( item.Equals( "ScrollSpeedGuitar" ) )
									{
										cPerformanceEntry.fScrollSpeed.Guitar = (float) decimal.Parse( para );
									}
									#endregion
									#region [ ScrollSpeedBass ]
									else if ( item.Equals( "ScrollSpeedBass" ) )
									{
										cPerformanceEntry.fScrollSpeed.Bass = (float) decimal.Parse( para );
									}
									#endregion
									#region [ PlaySpeed ]
									else if ( item.Equals( "PlaySpeed" ) )
									{
										string[] strArray2 = para.Split( new char[] { '/' } );
										if ( strArray2.Length == 2 )
										{
											cPerformanceEntry.nPlaySpeedNumerator = int.Parse( strArray2[ 0 ] );
											cPerformanceEntry.nPlaySpeedDenominator = int.Parse( strArray2[ 1 ] );
										}
									}
									#endregion
									else
									{
										#region [ HHGroup ]
										if ( item.Equals( "HHGroup" ) )
										{
											switch ( int.Parse( para ) )
											{
												case 0:
													{
														cPerformanceEntry.eHHGroup = EHHGroup.全部打ち分ける;
														continue;
													}
												case 1:
													{
														cPerformanceEntry.eHHGroup = EHHGroup.ハイハットのみ打ち分ける;
														continue;
													}
												case 2:
													{
														cPerformanceEntry.eHHGroup = EHHGroup.左シンバルのみ打ち分ける;
														continue;
													}
												case 3:
													{
														cPerformanceEntry.eHHGroup = EHHGroup.全部共通;
														continue;
													}
											}
											throw new Exception( "HHGroup の値が無効です。" );
										}
										#endregion
										#region [ FTGroup ]
										if ( item.Equals( "FTGroup" ) )
										{
											switch ( int.Parse( para ) )
											{
												case 0:
													{
														cPerformanceEntry.eFTGroup = EFTGroup.打ち分ける;
														continue;
													}
												case 1:
													{
														cPerformanceEntry.eFTGroup = EFTGroup.共通;
														continue;
													}
											}
											throw new Exception( "FTGroup の値が無効です。" );
										}
										#endregion
										#region [ CYGroup ]
										if ( item.Equals( "CYGroup" ) )
										{
											switch ( int.Parse( para ) )
											{
												case 0:
													{
														cPerformanceEntry.eCYGroup = ECYGroup.打ち分ける;
														continue;
													}
												case 1:
													{
														cPerformanceEntry.eCYGroup = ECYGroup.共通;
														continue;
													}
											}
											throw new Exception( "CYGroup の値が無効です。" );
										}
										#endregion
                                        #region [ BDGroup ]
                                        if (item.Equals("BDGroup"))
                                        {
                                            switch (int.Parse(para))
                                            {
                                                case 0:
                                                    {
                                                        cPerformanceEntry.eBDGroup = EBDGroup.打ち分ける;
                                                        continue;
                                                    }
                                                case 1:
                                                    {
                                                        cPerformanceEntry.eBDGroup = EBDGroup.左右ペダルのみ打ち分ける;
                                                        continue;
                                                    }
                                                case 2:
                                                    {
                                                        cPerformanceEntry.eBDGroup = EBDGroup.どっちもBD;
                                                        continue;
                                                    }
                                            }
                                            throw new Exception("HHGroup の値が無効です。");
                                        }
                                        #endregion
										#region [ HitSoundPriorityHH ]
										if ( item.Equals( "HitSoundPriorityHH" ) )
										{
											switch ( int.Parse( para ) )
											{
												case 0:
													{
														cPerformanceEntry.eHitSoundPriorityHH = EPlaybackPriority.ChipOverPadPriority;
														continue;
													}
												case 1:
													{
														cPerformanceEntry.eHitSoundPriorityHH = EPlaybackPriority.PadOverChipPriority;
														continue;
													}
											}
											throw new Exception( "HitSoundPriorityHH の値が無効です。" );
										}
										#endregion
										#region [ HitSoundPriorityFT ]
										if ( item.Equals( "HitSoundPriorityFT" ) )
										{
											switch ( int.Parse( para ) )
											{
												case 0:
													{
														cPerformanceEntry.eHitSoundPriorityFT = EPlaybackPriority.ChipOverPadPriority;
														continue;
													}
												case 1:
													{
														cPerformanceEntry.eHitSoundPriorityFT = EPlaybackPriority.PadOverChipPriority;
														continue;
													}
											}
											throw new Exception( "HitSoundPriorityFT の値が無効です。" );
										}
										#endregion
										#region [ HitSoundPriorityCY ]
										if ( item.Equals( "HitSoundPriorityCY" ) )
										{
											switch ( int.Parse( para ) )
											{
												case 0:
													{
														cPerformanceEntry.eHitSoundPriorityCY = EPlaybackPriority.ChipOverPadPriority;
														continue;
													}
												case 1:
													{
														cPerformanceEntry.eHitSoundPriorityCY = EPlaybackPriority.PadOverChipPriority;
														continue;
													}
											}
											throw new Exception( "HitSoundPriorityCY の値が無効です。" );
										}
										#endregion
										#region [ Guitar ]
										if ( item.Equals( "Guitar" ) )
										{
											cPerformanceEntry.bGuitarEnabled = CConversion.bONorOFF( para[ 0 ] );
										}
										#endregion
										#region [ Drums ]
										else if ( item.Equals( "Drums" ) )
										{
											cPerformanceEntry.bDrumsEnabled = CConversion.bONorOFF( para[ 0 ] );
										}
										#endregion
										#region [ StageFailed ]
										else if ( item.Equals( "StageFailed" ) )
										{
											cPerformanceEntry.bSTAGEFAILEDEnabled = CConversion.bONorOFF( para[ 0 ] );
										}
										#endregion
										else
										{
											#region [ DamageLevel ]
											if ( item.Equals( "DamageLevel" ) )
											{
												switch ( int.Parse( para ) )
												{
													case 0:
														{
															cPerformanceEntry.eDamageLevel = EDamageLevel.Small;
															continue;
														}
													case 1:
														{
															cPerformanceEntry.eDamageLevel = EDamageLevel.Normal;
															continue;
														}
													case 2:
														{
															cPerformanceEntry.eDamageLevel = EDamageLevel.High;
															continue;
														}
												}
												throw new Exception( "DamageLevel の値が無効です。" );
											}
											#endregion
											if ( item.Equals( "UseKeyboard" ) )
											{
												cPerformanceEntry.bKeyboardUsed = CConversion.bONorOFF( para[ 0 ] );
											}
											else if ( item.Equals( "UseMIDIIN" ) )
											{
												cPerformanceEntry.bMIDIUsed = CConversion.bONorOFF( para[ 0 ] );
											}
											else if ( item.Equals( "UseJoypad" ) )
											{
												cPerformanceEntry.bJoypadUsed = CConversion.bONorOFF( para[ 0 ] );
											}
											else if ( item.Equals( "UseMouse" ) )
											{
												cPerformanceEntry.bMouseUsed = CConversion.bONorOFF( para[ 0 ] );
											}
											else if ( item.Equals( "DTXManiaVersion" ) )
											{
												cPerformanceEntry.strDTXManiaVersion = para;
											}
											else if ( item.Equals( "DateTime" ) )
											{
												cPerformanceEntry.strDateTime = para;
											}
											else if ( item.Equals( "Hash" ) )
											{
												cPerformanceEntry.Hash = para;
											}
											else if ( item.Equals( "9LaneMode" ) )
											{
												cPerformanceEntry.レーン9モード = CConversion.bONorOFF( para[ 0 ] );
											}
											else
											{
												int.TryParse(para, out var iValue);
												switch (item)
												{
													// legacy perfect range size (±ms)
													case @"PerfectRange":
														cPerformanceEntry.PrimaryHitRanges.nPerfectSizeMs = iValue;
														cPerformanceEntry.SecondaryHitRanges.nPerfectSizeMs = iValue;
														break;

													// legacy great range size (±ms)
													case @"GreatRange":
														cPerformanceEntry.PrimaryHitRanges.nGreatSizeMs = iValue;
														cPerformanceEntry.SecondaryHitRanges.nGreatSizeMs = iValue;
														break;

													// legacy good range size (±ms)
													case @"GoodRange":
														cPerformanceEntry.PrimaryHitRanges.nGoodSizeMs = iValue;
														cPerformanceEntry.SecondaryHitRanges.nGoodSizeMs = iValue;
														break;

													// legacy poor range size (±ms)
													case @"PoorRange":
														cPerformanceEntry.PrimaryHitRanges.nPoorSizeMs = iValue;
														cPerformanceEntry.SecondaryHitRanges.nPoorSizeMs = iValue;
														break;

													// primary perfect range size (±ms)
													case @"PrimaryPerfectRange":
														cPerformanceEntry.PrimaryHitRanges.nPerfectSizeMs = iValue;
														break;

													// primary great range size (±ms)
													case @"PrimaryGreatRange":
														cPerformanceEntry.PrimaryHitRanges.nGreatSizeMs = iValue;
														break;

													// primary good range size (±ms)
													case @"PrimaryGoodRange":
														cPerformanceEntry.PrimaryHitRanges.nGoodSizeMs = iValue;
														break;

													// primary poor range size (±ms)
													case @"PrimaryPoorRange":
														cPerformanceEntry.PrimaryHitRanges.nPoorSizeMs = iValue;
														break;

													// secondary perfect range size (±ms)
													case @"SecondaryPerfectRange":
														cPerformanceEntry.SecondaryHitRanges.nPerfectSizeMs = iValue;
														break;

													// secondary great range size (±ms)
													case @"SecondaryGreatRange":
														cPerformanceEntry.SecondaryHitRanges.nGreatSizeMs = iValue;
														break;

													// secondary good range size (±ms)
													case @"SecondaryGoodRange":
														cPerformanceEntry.SecondaryHitRanges.nGoodSizeMs = iValue;
														break;

													// secondary poor range size (±ms)
													case @"SecondaryPoorRange":
														cPerformanceEntry.SecondaryHitRanges.nPoorSizeMs = iValue;
														break;
												}
											}
										}
									}
								}
							}
							continue;
						}
						catch( Exception exception )
						{
							Trace.TraceError( "{0}読み込みを中断します。({1})",  exception.Message, iniファイル名 );
							break;
						}
					}
				}
				reader.Close();
			}
		}

		internal void tAddHistory( string str追加文字列 )
		{
			this.stFile.HistoryCount++;
			for( int i = 3; i >= 0; i-- )
				this.stFile.History[ i + 1 ] = this.stFile.History[ i ];
			DateTime now = DateTime.Now;
			this.stFile.History[ 0 ] = string.Format( "{0:0}.{1:D2}/{2}/{3} {4}", this.stFile.HistoryCount, now.Year % 100, now.Month, now.Day, str追加文字列 );
		}
		internal void tExport( string iniファイル名 )
		{
			this.iniFileDirectoryName = Path.GetDirectoryName( iniファイル名 );
			this.iniFilename = Path.GetFileName( iniファイル名 );

			StreamWriter writer = new StreamWriter( iniファイル名, false, Encoding.GetEncoding( "shift-jis" ) );
			writer.WriteLine( "[File]" );
			writer.WriteLine( "Title={0}", this.stFile.Title );
			writer.WriteLine( "Name={0}", this.stFile.Name );
			writer.WriteLine( "Hash={0}", this.stFile.Hash );
			writer.WriteLine( "PlayCountDrums={0}", this.stFile.PlayCountDrums );
			writer.WriteLine( "PlayCountGuitars={0}", this.stFile.PlayCountGuitar );
            writer.WriteLine( "PlayCountBass={0}", this.stFile.PlayCountBass );
            writer.WriteLine( "ClearCountDrums={0}", this.stFile.ClearCountDrums );       // #23596 10.11.16 add ikanick
            writer.WriteLine( "ClearCountGuitars={0}", this.stFile.ClearCountGuitar );    //
            writer.WriteLine( "ClearCountBass={0}", this.stFile.ClearCountBass );         //
			writer.WriteLine( "BestRankDrums={0}", this.stFile.BestRank.Drums );		// #24459 2011.2.24 yyagi
			writer.WriteLine( "BestRankGuitar={0}", this.stFile.BestRank.Guitar );		//
			writer.WriteLine( "BestRankBass={0}", this.stFile.BestRank.Bass );			//
			writer.WriteLine( "HistoryCount={0}", this.stFile.HistoryCount );
			writer.WriteLine( "History0={0}", this.stFile.History[ 0 ] );
			writer.WriteLine( "History1={0}", this.stFile.History[ 1 ] );
			writer.WriteLine( "History2={0}", this.stFile.History[ 2 ] );
			writer.WriteLine( "History3={0}", this.stFile.History[ 3 ] );
			writer.WriteLine( "History4={0}", this.stFile.History[ 4 ] );
			writer.WriteLine( "BGMAdjust={0}", this.stFile.BGMAdjust );
			writer.WriteLine();
			for( int i = 0; i < 9; i++ )
			{
                string[] strArray = { "HiScore.Drums", "HiSkill.Drums", "HiScore.Guitar", "HiSkill.Guitar", "HiScore.Bass", "HiSkill.Bass", "LastPlay.Drums", "LastPlay.Guitar", "LastPlay.Bass" };
				writer.WriteLine( "[{0}]", strArray[ i ] );
				writer.WriteLine( "Score={0}", this.stSection[ i ].nスコア );
				writer.WriteLine( "PlaySkill={0}", this.stSection[ i ].dbPerformanceSkill );
				writer.WriteLine( "Skill={0}", this.stSection[ i ].dbGameSkill );
				writer.WriteLine( "Perfect={0}", this.stSection[ i ].nPerfectCount );
				writer.WriteLine( "Great={0}", this.stSection[ i ].nGreatCount );
				writer.WriteLine( "Good={0}", this.stSection[ i ].nGoodCount );
				writer.WriteLine( "Poor={0}", this.stSection[ i ].nPoorCount );
				writer.WriteLine( "Miss={0}", this.stSection[ i ].nMissCount );
				writer.WriteLine( "MaxCombo={0}", this.stSection[ i ].nMaxCombo );
				writer.WriteLine( "TotalChips={0}", this.stSection[ i ].nTotalChipsCount );
				writer.Write( "AutoPlay=" );
				for ( int j = 0; j < (int) ELane.MAX; j++ )
				{
					writer.Write( this.stSection[ i ].bAutoPlay[ j ] ? 1 : 0 );
				}
				writer.WriteLine();
				writer.WriteLine( "Risky={0}", this.stSection[ i ].nRisky );
				writer.WriteLine( "SuddenDrums={0}", this.stSection[ i ].bSudden.Drums ? 1 : 0 );
				writer.WriteLine( "SuddenGuitar={0}", this.stSection[ i ].bSudden.Guitar ? 1 : 0 );
				writer.WriteLine( "SuddenBass={0}", this.stSection[ i ].bSudden.Bass ? 1 : 0 );
				writer.WriteLine( "HiddenDrums={0}", this.stSection[ i ].bHidden.Drums ? 1 : 0 );
				writer.WriteLine( "HiddenGuitar={0}", this.stSection[ i ].bHidden.Guitar ? 1 : 0 );
				writer.WriteLine( "HiddenBass={0}", this.stSection[ i ].bHidden.Bass ? 1 : 0 );
				writer.WriteLine( "ReverseDrums={0}", this.stSection[ i ].bReverse.Drums ? 1 : 0 );
				writer.WriteLine( "ReverseGuitar={0}", this.stSection[ i ].bReverse.Guitar ? 1 : 0 );
				writer.WriteLine( "ReverseBass={0}", this.stSection[ i ].bReverse.Bass ? 1 : 0 );
				writer.WriteLine( "TightDrums={0}", this.stSection[ i ].bTight ? 1 : 0 );
				writer.WriteLine( "RandomGuitar={0}", (int) this.stSection[ i ].eRandom.Guitar );
				writer.WriteLine( "RandomBass={0}", (int) this.stSection[ i ].eRandom.Bass );
				writer.WriteLine( "LightGuitar={0}", this.stSection[ i ].bLight.Guitar ? 1 : 0 );
				writer.WriteLine( "LightBass={0}", this.stSection[ i ].bLight.Bass ? 1 : 0 );
				writer.WriteLine( "LeftGuitar={0}", this.stSection[ i ].bLeft.Guitar ? 1 : 0 );
				writer.WriteLine( "LeftBass={0}", this.stSection[ i ].bLeft.Bass ? 1 : 0 );
				writer.WriteLine( "Dark={0}", (int) this.stSection[ i ].eDark );
				writer.WriteLine( "ScrollSpeedDrums={0}", this.stSection[ i ].fScrollSpeed.Drums );
				writer.WriteLine( "ScrollSpeedGuitar={0}", this.stSection[ i ].fScrollSpeed.Guitar );
				writer.WriteLine( "ScrollSpeedBass={0}", this.stSection[ i ].fScrollSpeed.Bass );
				writer.WriteLine( "PlaySpeed={0}/{1}", this.stSection[ i ].nPlaySpeedNumerator, this.stSection[ i ].nPlaySpeedDenominator );
				writer.WriteLine( "HHGroup={0}", (int) this.stSection[ i ].eHHGroup );
				writer.WriteLine( "FTGroup={0}", (int) this.stSection[ i ].eFTGroup );
				writer.WriteLine( "CYGroup={0}", (int) this.stSection[ i ].eCYGroup );
                writer.WriteLine( "BDGroup={0}", (int) this.stSection[ i ].eBDGroup);
				writer.WriteLine( "HitSoundPriorityHH={0}", (int) this.stSection[ i ].eHitSoundPriorityHH );
				writer.WriteLine( "HitSoundPriorityFT={0}", (int) this.stSection[ i ].eHitSoundPriorityFT );
				writer.WriteLine( "HitSoundPriorityCY={0}", (int) this.stSection[ i ].eHitSoundPriorityCY );
				writer.WriteLine( "Guitar={0}", this.stSection[ i ].bGuitarEnabled ? 1 : 0 );
				writer.WriteLine( "Drums={0}", this.stSection[ i ].bDrumsEnabled ? 1 : 0 );
				writer.WriteLine( "StageFailed={0}", this.stSection[ i ].bSTAGEFAILEDEnabled ? 1 : 0 );
				writer.WriteLine( "DamageLevel={0}", (int) this.stSection[ i ].eDamageLevel );
				writer.WriteLine( "UseKeyboard={0}", this.stSection[ i ].bKeyboardUsed ? 1 : 0 );
				writer.WriteLine( "UseMIDIIN={0}", this.stSection[ i ].bMIDIUsed ? 1 : 0 );
				writer.WriteLine( "UseJoypad={0}", this.stSection[ i ].bJoypadUsed ? 1 : 0 );
				writer.WriteLine( "UseMouse={0}", this.stSection[ i ].bMouseUsed ? 1 : 0 );
				writer.WriteLine($@"PrimaryPerfectRange={stSection[i].PrimaryHitRanges.nPerfectSizeMs}");
				writer.WriteLine($@"PrimaryGreatRange={stSection[i].PrimaryHitRanges.nGreatSizeMs}");
				writer.WriteLine($@"PrimaryGoodRange={stSection[i].PrimaryHitRanges.nGoodSizeMs}");
				writer.WriteLine($@"PrimaryPoorRange={stSection[i].PrimaryHitRanges.nPoorSizeMs}");
				writer.WriteLine($@"SecondaryPerfectRange={stSection[i].SecondaryHitRanges.nPerfectSizeMs}");
				writer.WriteLine($@"SecondaryGreatRange={stSection[i].SecondaryHitRanges.nGreatSizeMs}");
				writer.WriteLine($@"SecondaryGoodRange={stSection[i].SecondaryHitRanges.nGoodSizeMs}");
				writer.WriteLine($@"SecondaryPoorRange={stSection[i].SecondaryHitRanges.nPoorSizeMs}");
				writer.WriteLine( "DTXManiaVersion={0}", this.stSection[ i ].strDTXManiaVersion );
				writer.WriteLine( "DateTime={0}", this.stSection[ i ].strDateTime );
				writer.WriteLine( "Hash={0}", this.stSection[ i ].Hash );
			}
			writer.Close();
		}
		internal void tCheckIntegrity()
		{
			for( int i = 0; i < 9; i++ )
			{
				if( !this.bCheckConsistency( (ESectionType) i ) )
					this.stSection[ i ] = new CPerformanceEntry();
			}
        }
        internal static int tCalculateRank(CPerformanceEntry part)
        {
            if (part.bMIDIUsed || part.bKeyboardUsed || part.bJoypadUsed || part.bMouseUsed)	// 2010.9.11
            {
                int nTotal = part.nPerfectCount + part.nGreatCount + part.nGoodCount + part.nPoorCount + part.nMissCount;
                return tCalculateRank(nTotal, part.nPerfectCount, part.nGreatCount, part.nGoodCount, part.nPoorCount, part.nMissCount, part.nMaxCombo);
            }
            return (int)ERANK.UNKNOWN;
        }
        
        /// <summary>
        /// nDummy 適当な数値を入れてください。特に使いません。
        /// dRate 達成率を入れます。
        /// </summary>
        internal static int tCalculateRank( int nDummy, double dRate )
        {
            if ( dRate == 0 )
                return (int)ERANK.UNKNOWN;

            if ( dRate >= 95 )
            {
                return (int)ERANK.SS;
            }
            if ( dRate >= 80 )
            {
                return (int)ERANK.S;
            }
            if ( dRate >= 73 )
            {
                return (int)ERANK.A;
            }
            if ( dRate >= 63 )
            {
                return (int)ERANK.B;
            }
            if ( dRate >= 53 )
            {
                return (int)ERANK.C;
            }
            if ( dRate >= 45 )
            {
                return (int)ERANK.D;
            }
            return (int)ERANK.E;
        }
        internal static int tCalculateRank(int nTotal, int nPerfect, int nGreat, int nGood, int nPoor, int nMiss, int nCombo)
        {
            if (nTotal <= 0)
                return (int)ERANK.UNKNOWN;

            //int nRank = (int)ERANK.E;
            int nAuto = nTotal - (nPerfect + nGreat + nGood + nPoor + nMiss);
            if (nTotal <= nAuto)
            {
                return (int)ERANK.SS;
            }

			// Remark: this rate uses the percentage of perfect, great and combo compared to the number of non-auto chips only
			// while the official rate from tCalculatePlayingSkill uses the percentage compared to the full total number of chips
			// So this is probably wrong, but I'm not touching it for now.
            double dRate = ((((100.0 * nPerfect / (nTotal - nAuto))) * 0.85) + (((100.0 * nGreat / (nTotal - nAuto))) * 0.35) + ((100.0 * nCombo / (nTotal - nAuto))) * 0.15);

            //System.IO.StreamWriter sw = new System.IO.StreamWriter(@"debug.txt", true, System.Text.Encoding.GetEncoding("shift_jis"));
            //sw.WriteLine("-------------------------------");
            //sw.WriteLine("dRateの値は{0}です。", dRate);
            //sw.WriteLine("nTotalは{0}で、nAutoは{1}です。", nTotal, nAuto);
            //sw.Close();
            if (dRate >= 95)
            {
                return (int)ERANK.SS;
            }
            if (dRate >= 80)
            {
                return (int)ERANK.S;
            }
            if (dRate >= 73)
            {
                return (int)ERANK.A;
            }
            if (dRate >= 63)
            {
                return (int)ERANK.B;
            }
            if (dRate >= 53)
            {
                return (int)ERANK.C;
            }
            if (dRate >= 45)
            {
                return (int)ERANK.D;
            }
            return (int)ERANK.E;
        }
        internal static double tCalculateGameSkill(double dbLevel, int nLevelDec, int nTotal, int nPerfect, int nGreat, int nGood, int nPoor, int nMiss, int nCombo, EInstrumentPart inst, STAUTOPLAY bAutoPlay)
        {
            //こちらはプレイヤースキル_全曲スキルに加算される得点。いわゆる曲別スキル。

			double dbRate = tCalculatePlayingSkill(nTotal, nPerfect, nGreat, nCombo, nPoor, nMiss, nCombo, inst, bAutoPlay);

			double ret = tCalculateGameSkillFromPlayingSkill(dbLevel, nLevelDec, dbRate);

            return ret;
        }
		internal static double tCalculateGameSkillFromPlayingSkill(double dbLevel, int nLevelDec, double dbPlayingSkill)
		{
			if (dbLevel >= 100)
			{
				dbLevel = dbLevel / 100.0;
			}
			else if (dbLevel < 100)
			{
				dbLevel = dbLevel / 10.0 + nLevelDec / 100.0;
			}

			if (CDTXMania.ConfigIni.bDrumsEnabled && CDTXMania.ConfigIni.bAllDrumsAreAutoPlay)
			{
				return 0;
			}
			return dbPlayingSkill * dbLevel * 0.2;
		}
		internal static double tCalculatePlayingSkill(int nTotal, int nPerfect, int nGreat, int nGood, int nPoor, int nMiss, int nCombo, EInstrumentPart inst, STAUTOPLAY bAutoPlay)
        {
            if (nTotal == 0)
                return 0.0;

            int nAuto = nTotal - (nPerfect + nGreat + nGood + nPoor + nMiss);
            double dbPERFECT率 = (100.0 * nPerfect / nTotal);
            double dbGREAT率 = (100.0 * nGreat / nTotal);
            double dbCOMBO率 = (100.0 * nCombo / nTotal);

            if (nTotal == nAuto)
            {
                dbCOMBO率 = 0.0;
            }

            double ret = dbPERFECT率 * 0.85 + dbGREAT率 * 0.35 + dbCOMBO率 * 0.15;

            //System.IO.StreamWriter sw = new System.IO.StreamWriter(@"debug.txt", true, System.Text.Encoding.GetEncoding("shift_jis"));
            //sw.WriteLine("retの値は{0}です。", ret);
            //sw.WriteLine("nTotalは{0}で、dbPERFECT率は{1}、dbGREAT率は{2}です。", nTotal, dbPERFECT率, dbGREAT率);
            //sw.Close();

            ret *= dbCalcReviseValForDrGtBsAutoLanes(inst, bAutoPlay);
            return ret;
        }
        internal static double tCalculateGhostSkill(int nTotal, int nPerfect, int nCombo, EInstrumentPart inst)
        {
            if (nTotal == 0)
                return 0.0;

            double dbPERFECT率 = (100.0 * nPerfect / nTotal);
            double dbGREAT率 = (100.0 * nPerfect / nTotal);
            double dbCOMBO率 = (100.0 * nCombo / (nTotal));

            double ret = dbPERFECT率 * 0.85 + dbGREAT率 * 0.35 + dbCOMBO率 * 0.15;

            return ret;
        }
        internal static int tCalculateRankOld(CPerformanceEntry part)
        {
            if (part.bMIDIUsed || part.bKeyboardUsed || part.bJoypadUsed || part.bMouseUsed)	// 2010.9.11
            {
                int nTotal = part.nPerfectCount + part.nGreatCount + part.nGoodCount + part.nPoorCount + part.nMissCount;
                return tCalculateRankOld(nTotal, part.nPerfectCount, part.nGreatCount, part.nGoodCount, part.nPoorCount, part.nMissCount);
            }
            return (int)ERANK.UNKNOWN;
        }
        internal static int tCalculateRankOld(int nTotal, int nPerfect, int nGreat, int nGood, int nPoor, int nMiss)
        {
            if (nTotal <= 0)
                return (int)ERANK.UNKNOWN;

            //int nRank = (int)ERANK.E;
            int nAuto = nTotal - (nPerfect + nGreat + nGood + nPoor + nMiss);
            if (nTotal == nAuto)
            {
                return (int)ERANK.SS;
            }
            double dRate = ((double)(nPerfect + nGreat)) / ((double)(nTotal - nAuto));
            if (dRate == 1.0)
            {
                return (int)ERANK.SS;
            }
            if (dRate >= 0.95)
            {
                return (int)ERANK.S;
            }
            if (dRate >= 0.9)
            {
                return (int)ERANK.A;
            }
            if (dRate >= 0.85)
            {
                return (int)ERANK.B;
            }
            if (dRate >= 0.8)
            {
                return (int)ERANK.C;
            }
            if (dRate >= 0.7)
            {
                return (int)ERANK.D;
            }
            return (int)ERANK.E;
        }
        internal static double tCalculateGameSkillOld( double dbLevel, int nLevelDec, int nTotal, int nPerfect, int nGreat, int nCombo, EInstrumentPart inst, STAUTOPLAY bAutoPlay )
        {
            double ret;
			double rate = 0.0;
            if ( ( nTotal == 0 ) || ( ( nPerfect == 0 ) && ( nCombo == 0 ) && (nGreat == 0) ) )
                ret = 0.0;

			//Drums: Perfect% x 0.80 + Great% x 0.30 + Combo% + 0.20 (percents as decimals)
			//Guitar: Perfect% x 0.80 + Great% x 0.20 + Combo% + 0.20 (percents as decimals)
			switch (inst)
			{
				#region [ Unknown ]
				case EInstrumentPart.UNKNOWN:
					throw new ArgumentException();
				#endregion
				#region [ Drums ]
				case EInstrumentPart.DRUMS:
					rate = ((nPerfect * 0.8 + nGreat * 0.3 + nCombo * 0.2) / ((double)nTotal));
					break;
				#endregion
				#region [ Bass and Guitar ]
				case EInstrumentPart.BASS:
				case EInstrumentPart.GUITAR:
					rate = ((nPerfect * 0.8 + nGreat * 0.2 + nCombo * 0.2) / ((double)nTotal));
					break;
                #endregion
            }

			//Skill Ratio x Song Level x 0.33 x (0.5 if using Auto-anything, 1 otherwise)
			ret = dbLevel * rate * 0.33;
            ret *= dbCalcReviseValForDrGtBsAutoLanes( inst, bAutoPlay );
            if ( CDTXMania.ConfigIni.bAllDrumsAreAutoPlay )
            {
                return 0;
            }

            return ret;
        }
        internal static double tCalculatePlayingSkillOld(int nTotal, int nPerfect, int nGreat, int nGood, int nPoor, int nMiss, int nCombo, EInstrumentPart inst, STAUTOPLAY bAutoPlay)
        {
            if (nTotal == 0)
                return 0.0;

            //int nAuto = nTotal - (nPerfect + nGreat + nGood + nPoor + nMiss);
			//double y = ((nPerfect * 1.0 + nGreat * 0.8 + nGood * 0.5 + nPoor * 0.2 + nMiss * 0.0 + nAuto * 0.0) * 100.0) / ((double)nTotal);
			//double ret = (100.0 * ((Math.Pow(1.03, y) - 1.0) / (Math.Pow(1.03, 100.0) - 1.0)));
			double ret = 0.0;
			//Drums: Perfect% x 0.80 + Great% x 0.30 + Combo% + 0.20 (percents as decimals)
			//Guitar: Perfect% x 0.80 + Great% x 0.20 + Combo% + 0.20 (percents as decimals)
			switch (inst)
			{
				#region [ Unknown ]
				case EInstrumentPart.UNKNOWN:
					throw new ArgumentException();
				#endregion
				#region [ Drums ]
				case EInstrumentPart.DRUMS:
					ret = ((nPerfect * 0.8 + nGreat * 0.3 + nCombo * 0.2) / ((double)nTotal)) * 100.0;
					break;
				#endregion
				#region [ Bass and Guitar ]
				case EInstrumentPart.BASS:
				case EInstrumentPart.GUITAR:
					ret = ((nPerfect * 0.8 + nGreat * 0.2 + nCombo * 0.2) / ((double)nTotal)) * 100.0;
					break;
					#endregion
			}

			ret *= dbCalcReviseValForDrGtBsAutoLanes(inst, bAutoPlay);
            return ret;
        }
        internal static double tCalculateGhostSkillOld(int nTotal, int nPerfect, int nGreat, int nGood, int nPoor, int nMiss, int nCombo, EInstrumentPart inst)
        {
            if (nTotal == 0)
                return 0.0;
			//int nAuto = nTotal - (nPerfect + nGreat + nGood + nPoor + nMiss);
			//double y = ((nPerfect * 1.0 + nGreat * 0.8 + nGood * 0.5 + nPoor * 0.2 + nMiss * 0.0 + nAuto * 0.0) * 100.0) / ((double)nTotal);
			//double ret = (100.0 * ((Math.Pow(1.03, y) - 1.0) / (Math.Pow(1.03, 100.0) - 1.0)));
			double ret = 0.0;
			switch (inst)
			{
				#region [ Unknown ]
				case EInstrumentPart.UNKNOWN:
					throw new ArgumentException();
				#endregion
				#region [ Drums ]
				case EInstrumentPart.DRUMS:
					ret = ((nPerfect * 0.8 + nGreat * 0.3 + nCombo * 0.2) / ((double)nTotal)) * 100.0;
					break;
				#endregion
				#region [ Bass and Guitar ]
				case EInstrumentPart.BASS:
				case EInstrumentPart.GUITAR:
					ret = ((nPerfect * 0.8 + nGreat * 0.2 + nCombo * 0.2) / ((double)nTotal)) * 100.0;
					break;
					#endregion
			}

			return ret;
        }
        internal static double dbCalcReviseValForDrGtBsAutoLanes(EInstrumentPart inst, STAUTOPLAY bAutoPlay)	// #28607 2012.6.7 yyagi
        {
            double ret = 1.0;

            switch (inst)
            {
                #region [ Unknown ]
                case EInstrumentPart.UNKNOWN:
                    throw new ArgumentException();
                #endregion
                #region [ Drums ]
                case EInstrumentPart.DRUMS:
                    if (!CDTXMania.ConfigIni.bAllDrumsAreAutoPlay)
                    {
                        #region [ Auto BD ]
                        if (bAutoPlay.BD && bAutoPlay.LP == false && bAutoPlay.LBD == false)
                        {
                            ret /= 2;
                        }
                        #endregion

                        #region [ Auto LP ]
                        else if (bAutoPlay.BD == false && bAutoPlay.LP || bAutoPlay.LBD)
                        {
                            ret /= 2;
                        }
                        #endregion

                        #region [ 2Pedal Auto ]
                        else if (bAutoPlay.BD && bAutoPlay.LP && bAutoPlay.LBD)
                        {
                            ret *= 0.25;
                        }
                        #endregion
                    }
                    break;
                #endregion
                #region [ Guitar ]
                case EInstrumentPart.GUITAR:
                    if (!CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay)
                    {
                        #region [ Auto Pick ]
                        if (bAutoPlay.GtPick)
                        {
                            ret /= 2;			 // AutoPick時、達成率を1/2にする
                        }
                        #endregion
                        #region [ Auto Neck ]
                        int nAutoLanes = 0;
                        if (bAutoPlay.GtR)
                        {
                            nAutoLanes++;
                        }
                        if (bAutoPlay.GtG)
                        {
                            nAutoLanes++;
                        }
                        if (bAutoPlay.GtB)
                        {
                            nAutoLanes++;
                        }
                        ret /= Math.Sqrt(nAutoLanes + 1);
                        #endregion
                    }
                    break;
                #endregion
                #region [ Bass ]
                case EInstrumentPart.BASS:
                    if (!CDTXMania.ConfigIni.bAllBassAreAutoPlay)
                    {
                        #region [ Auto Pick ]
                        if (bAutoPlay.BsPick)
                        {
                            ret /= 2;			 // AutoPick時、達成率を1/2にする
                        }
                        #endregion
                        #region [ Auto lanes ]
                        int nAutoLanes = 0;
                        if (bAutoPlay.BsR)
                        {
                            nAutoLanes++;
                        }
                        if (bAutoPlay.BsG)
                        {
                            nAutoLanes++;
                        }
                        if (bAutoPlay.BsB)
                        {
                            nAutoLanes++;
                        }
                        ret /= Math.Sqrt(nAutoLanes + 1);
                        #endregion
                    }
                    break;
                #endregion
            }
            return ret;
        }
		internal static string tComputePerformanceSectionMD5( CPerformanceEntry cc )
		{
			StringBuilder builder = new StringBuilder();
			builder.Append( cc.nスコア.ToString() );
			builder.Append( cc.dbGameSkill.ToString( ".000000" ) );
			builder.Append( cc.dbPerformanceSkill.ToString( ".000000" ) );
			builder.Append( cc.nPerfectCount );
			builder.Append( cc.nGreatCount );
			builder.Append( cc.nGoodCount );
			builder.Append( cc.nPoorCount );
			builder.Append( cc.nMissCount );
			builder.Append( cc.nMaxCombo );
			builder.Append( cc.nTotalChipsCount );
			for( int i = 0; i < 10; i++ )
				builder.Append( boolToChar( cc.bAutoPlay[ i ] ) );
			builder.Append( boolToChar( cc.bTight ) );
			builder.Append( boolToChar( cc.bSudden.Drums ) );
			builder.Append( boolToChar( cc.bSudden.Guitar ) );
			builder.Append( boolToChar( cc.bSudden.Bass ) );
			builder.Append( boolToChar( cc.bHidden.Drums ) );
			builder.Append( boolToChar( cc.bHidden.Guitar ) );
			builder.Append( boolToChar( cc.bHidden.Bass ) );
			builder.Append( boolToChar( cc.bReverse.Drums ) );
			builder.Append( boolToChar( cc.bReverse.Guitar ) );
			builder.Append( boolToChar( cc.bReverse.Bass ) );
			builder.Append( (int) cc.eRandom.Guitar );
			builder.Append( (int) cc.eRandom.Bass );
			builder.Append( boolToChar( cc.bLight.Guitar ) );
			builder.Append( boolToChar( cc.bLight.Bass ) );
			builder.Append( boolToChar( cc.bLeft.Guitar ) );
			builder.Append( boolToChar( cc.bLeft.Bass ) );
			builder.Append( (int) cc.eDark );
			builder.Append( cc.fScrollSpeed.Drums.ToString( ".000000" ) );
			builder.Append( cc.fScrollSpeed.Guitar.ToString( ".000000" ) );
			builder.Append( cc.fScrollSpeed.Bass.ToString( ".000000" ) );
			builder.Append( cc.nPlaySpeedNumerator );
			builder.Append( cc.nPlaySpeedDenominator );
			builder.Append( (int) cc.eHHGroup );
			builder.Append( (int) cc.eFTGroup );
			builder.Append( (int) cc.eCYGroup );
			builder.Append( (int) cc.eHitSoundPriorityHH );
			builder.Append( (int) cc.eHitSoundPriorityFT );
			builder.Append( (int) cc.eHitSoundPriorityCY );
			builder.Append( boolToChar( cc.bGuitarEnabled ) );
			builder.Append( boolToChar( cc.bDrumsEnabled ) );
			builder.Append( boolToChar( cc.bSTAGEFAILEDEnabled ) );
			builder.Append( (int) cc.eDamageLevel );
			builder.Append( boolToChar( cc.bKeyboardUsed ) );
			builder.Append( boolToChar( cc.bMIDIUsed ) );
			builder.Append( boolToChar( cc.bJoypadUsed ) );
			builder.Append( boolToChar( cc.bMouseUsed ) );
			builder.Append(cc.PrimaryHitRanges.nPerfectSizeMs);
			builder.Append(cc.PrimaryHitRanges.nGreatSizeMs);
			builder.Append(cc.PrimaryHitRanges.nGoodSizeMs);
			builder.Append(cc.PrimaryHitRanges.nPoorSizeMs);
			builder.Append(cc.SecondaryHitRanges.nPerfectSizeMs);
			builder.Append(cc.SecondaryHitRanges.nGreatSizeMs);
			builder.Append(cc.SecondaryHitRanges.nGoodSizeMs);
			builder.Append(cc.SecondaryHitRanges.nPoorSizeMs);
			builder.Append( cc.strDTXManiaVersion );
			builder.Append( cc.strDateTime );

			byte[] bytes = Encoding.GetEncoding( "shift-jis" ).GetBytes( builder.ToString() );
			StringBuilder builder2 = new StringBuilder(0x21);
			{
				MD5CryptoServiceProvider m = new MD5CryptoServiceProvider();
				byte[] buffer2 = m.ComputeHash(bytes);
				foreach (byte num2 in buffer2)
					builder2.Append(num2.ToString("x2"));
			}
			return builder2.ToString();
		}
		internal static void tGetIsUpdateNeeded( out bool bDrumsを更新する, out bool bGuitarを更新する, out bool bBassを更新する )
		{
			bDrumsを更新する =  CDTXMania.ConfigIni.bDrumsEnabled  && CDTXMania.DTX.bチップがある.Drums  && !CDTXMania.ConfigIni.bAllDrumsAreAutoPlay;
			bGuitarを更新する = CDTXMania.ConfigIni.bGuitarEnabled && CDTXMania.DTX.bチップがある.Guitar && !CDTXMania.ConfigIni.bAllGuitarsAreAutoPlay;
			bBassを更新する =   CDTXMania.ConfigIni.bGuitarEnabled && CDTXMania.DTX.bチップがある.Bass   && !CDTXMania.ConfigIni.bAllBassAreAutoPlay;
		}
        internal static int tCalculateOverallRankValue(CPerformanceEntry Drums, CPerformanceEntry Guitar, CPerformanceEntry Bass)
        {
            int nTotal = Drums.nTotalChipsCount + Guitar.nTotalChipsCount + Bass.nTotalChipsCount;
            int nPerfect = Drums.nPerfectCount_ExclAuto + Guitar.nPerfectCount_ExclAuto + Bass.nPerfectCount_ExclAuto;	// #24569 2011.3.1 yyagi: to calculate result rank without AUTO chips
            int nGreat = Drums.nGreatCount_ExclAuto + Guitar.nGreatCount_ExclAuto + Bass.nGreatCount_ExclAuto;		//
            int nGood = Drums.nGoodCount_ExclAuto + Guitar.nGoodCount_ExclAuto + Bass.nGoodCount_ExclAuto;		//
            int nPoor = Drums.nPoorCount_ExclAuto + Guitar.nPoorCount_ExclAuto + Bass.nPoorCount_ExclAuto;		//
            int nMiss = Drums.nMissCount_ExclAuto + Guitar.nMissCount_ExclAuto + Bass.nMissCount_ExclAuto;		//
            int nCombo = Drums.nMaxCombo + Guitar.nMaxCombo + Bass.nMaxCombo;		//
            if (CDTXMania.ConfigIni.nSkillMode == 0)
            {
                return tCalculateRankOld(nTotal, nPerfect, nGreat, nGood, nPoor, nMiss);
            }
            return tCalculateRank(nTotal, nPerfect, nGreat, nGood, nPoor, nMiss, nCombo);
        }

		// Other

		#region [ private ]
		//-----------------
		private bool ONorOFF( char c )
		{
			return ( c != '0' );
		}
		private static char boolToChar( bool b )
		{
			if( !b )
			{
				return '0';
			}
			return '1';
		}
		//-----------------
		#endregion
	}
}
