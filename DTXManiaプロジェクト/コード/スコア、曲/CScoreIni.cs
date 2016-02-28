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
		public STファイル stファイル;
		[StructLayout( LayoutKind.Sequential )]
		public struct STファイル
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
		public STセクション stセクション;
		[StructLayout( LayoutKind.Sequential )]
		public struct STセクション
		{
            public CScoreIni.C演奏記録 HiScoreDrums;
            public CScoreIni.C演奏記録 HiSkillDrums;
			public CScoreIni.C演奏記録 HiScoreGuitar;
            public CScoreIni.C演奏記録 HiSkillGuitar;
			public CScoreIni.C演奏記録 HiScoreBass;
            public CScoreIni.C演奏記録 HiSkillBass;
            public CScoreIni.C演奏記録 LastPlayDrums;   // #23595 2011.1.9 ikanick
            public CScoreIni.C演奏記録 LastPlayGuitar;  //
            public CScoreIni.C演奏記録 LastPlayBass;    //
			public CScoreIni.C演奏記録 this[ int index ]
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
		public enum Eセクション種別 : int
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
		public class C演奏記録
		{
			public STAUTOPLAY bAutoPlay;
			public bool bDrums有効;
			public bool bGuitar有効;
			public STDGBVALUE<bool> bHidden;
			public STDGBVALUE<bool> bLeft;
			public STDGBVALUE<bool> bLight;
			public STDGBVALUE<bool> bReverse;
			public bool bSTAGEFAILED有効;
			public STDGBVALUE<bool> bSudden;
			public bool bTight;
			public bool b演奏にMIDI入力を使用した;
			public bool b演奏にキーボードを使用した;
			public bool b演奏にジョイパッドを使用した;
			public bool b演奏にマウスを使用した;
			public double dbゲーム型スキル値;
			public double db演奏型スキル値;
			public ECYGroup eCYGroup;
			public Eダークモード eDark;
			public EFTGroup eFTGroup;
			public EHHGroup eHHGroup;
            public EBDGroup eBDGroup;
			public E打ち分け時の再生の優先順位 eHitSoundPriorityCY;
			public E打ち分け時の再生の優先順位 eHitSoundPriorityFT;
			public E打ち分け時の再生の優先順位 eHitSoundPriorityHH;
			public STDGBVALUE<Eランダムモード> eRandom;
			public Eダメージレベル eダメージレベル;
			public STDGBVALUE<float> f譜面スクロール速度;
			public string Hash;
			public int nGoodになる範囲ms;
			public int nGood数;
			public int nGreatになる範囲ms;
			public int nGreat数;
			public int nMiss数;
			public int nPerfectになる範囲ms;
			public int nPerfect数;
			public int nPoorになる範囲ms;
			public int nPoor数;
			public int nPerfect数・Auto含まない;
			public int nGreat数・Auto含まない;
			public int nGood数・Auto含まない;
			public int nPoor数・Auto含まない;
			public int nMiss数・Auto含まない;
			public long nスコア;
			public int n演奏速度分子;
			public int n演奏速度分母;
			public int n最大コンボ数;
			public int n全チップ数;
			public string strDTXManiaのバージョン;
			public bool レーン9モード;
			public int nRisky;		// #23559 2011.6.20 yyagi 0=OFF, 1-10=Risky
			public string 最終更新日時;

			public C演奏記録()
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
				this.eRandom = new STDGBVALUE<Eランダムモード>();
				this.eRandom.Drums = Eランダムモード.OFF;
				this.eRandom.Guitar = Eランダムモード.OFF;
				this.eRandom.Bass = Eランダムモード.OFF;
				this.bLight = new STDGBVALUE<bool>();
				this.bLight.Drums = false;
				this.bLight.Guitar = false;
				this.bLight.Bass = false;
				this.bLeft = new STDGBVALUE<bool>();
				this.bLeft.Drums = false;
				this.bLeft.Guitar = false;
				this.bLeft.Bass = false;
				this.f譜面スクロール速度 = new STDGBVALUE<float>();
				this.f譜面スクロール速度.Drums = 1f;
				this.f譜面スクロール速度.Guitar = 1f;
				this.f譜面スクロール速度.Bass = 1f;
				this.n演奏速度分子 = 20;
				this.n演奏速度分母 = 20;
				this.bGuitar有効 = true;
				this.bDrums有効 = true;
				this.bSTAGEFAILED有効 = true;
				this.eダメージレベル = Eダメージレベル.普通;
				this.nPerfectになる範囲ms = 34;
				this.nGreatになる範囲ms = 67;
				this.nGoodになる範囲ms = 84;
				this.nPoorになる範囲ms = 117;
				this.strDTXManiaのバージョン = "Unknown";
				this.最終更新日時 = "";
				this.Hash = "00000000000000000000000000000000";
				this.レーン9モード = true;
				this.nRisky = 0;									// #23559 2011.6.20 yyagi
			}

			public bool bフルコンボじゃない
			{
				get
				{
					return !this.bフルコンボである;
				}
			}
			public bool bフルコンボである
			{
				get
				{
					return ( ( this.n最大コンボ数 > 0 ) && ( this.n最大コンボ数 == ( this.nPerfect数 + this.nGreat数 + this.nGood数 + this.nPoor数 + this.nMiss数 ) ) );
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
					return (this.n全チップ数 - this.nPerfect数・Auto含まない - this.nGreat数・Auto含まない - this.nGood数・Auto含まない - this.nPoor数・Auto含まない - this.nMiss数・Auto含まない) == this.n全チップ数;
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
		public string iniファイルのあるフォルダ名
		{
			get;
			private set;
		}

		/// <summary>
		/// <para>.score.ini のファイル名（絶対パス）。</para>
		/// <para>未保存などでファイル名がない場合は null。</para>
		/// </summary>
		public string iniファイル名
		{
			get; 
			private set;
		}


		// コンストラクタ

		public CScoreIni()
		{
			this.iniファイルのあるフォルダ名 = null;
			this.iniファイル名 = null;
			this.stファイル = new STファイル();
			stファイル.Title = "";
			stファイル.Name = "";
			stファイル.Hash = "";
			stファイル.History = new string[] { "", "", "", "", "" };
			stファイル.BestRank.Drums =  (int)ERANK.UNKNOWN;		// #24459 2011.2.24 yyagi
			stファイル.BestRank.Guitar = (int)ERANK.UNKNOWN;		//
			stファイル.BestRank.Bass =   (int)ERANK.UNKNOWN;		//
	
			this.stセクション = new STセクション();
			stセクション.HiScoreDrums = new C演奏記録();
			stセクション.HiSkillDrums = new C演奏記録();
			stセクション.HiScoreGuitar = new C演奏記録();
            stセクション.HiSkillGuitar = new C演奏記録();
            stセクション.HiScoreBass = new C演奏記録();
            stセクション.HiSkillBass = new C演奏記録();
            stセクション.LastPlayDrums = new C演奏記録();
            stセクション.LastPlayGuitar = new C演奏記録();
            stセクション.LastPlayBass = new C演奏記録();
		}

		/// <summary>
		/// <para>初期化後にiniファイルを読み込むコンストラクタ。</para>
		/// <para>読み込んだiniに不正値があれば、それが含まれるセクションをリセットする。</para>
		/// </summary>
		public CScoreIni( string str読み込むiniファイル )
			: this()
		{
			this.t読み込み( str読み込むiniファイル );
			this.t全演奏記録セクションの整合性をチェックし不整合があればリセットする();
		}


		// メソッド

		/// <summary>
		/// <para>現在の this.Record[] オブジェクトの、指定されたセクションの情報が正当であるか否かを判定する。
		/// 真偽どちらでも、その内容は書き換えない。</para>
		/// </summary>
		/// <param name="eセクション">判定するセクション。</param>
		/// <returns>正当である（整合性がある）場合は true。</returns>
		public bool b整合性がある( Eセクション種別 eセクション )
		{
			return true;	// オープンソース化に伴い、整合性チェックを無効化。（2010.10.21）
		}
		
		/// <summary>
		/// 指定されたファイルの内容から MD5 値を求め、それを16進数に変換した文字列を返す。
		/// </summary>
		/// <param name="ファイル名">MD5 を求めるファイル名。</param>
		/// <returns>算出結果の MD5 を16進数で並べた文字列。</returns>
		public static string tファイルのMD5を求めて返す( string ファイル名 )
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
		public void t読み込み( string iniファイル名 )
		{
			this.iniファイルのあるフォルダ名 = Path.GetDirectoryName( iniファイル名 );
			this.iniファイル名 = Path.GetFileName( iniファイル名 );

			Eセクション種別 section = Eセクション種別.Unknown;
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
							C演奏記録 c演奏記録;
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
									section = Eセクション種別.File;
								}
								else if( str2.Equals( "HiScore.Drums" ) )
								{
									section = Eセクション種別.HiScoreDrums;
								}
								else if( str2.Equals( "HiSkill.Drums" ) )
								{
									section = Eセクション種別.HiSkillDrums;
								}
								else if( str2.Equals( "HiScore.Guitar" ) )
								{
									section = Eセクション種別.HiScoreGuitar;
								}
								else if( str2.Equals( "HiSkill.Guitar" ) )
								{
									section = Eセクション種別.HiSkillGuitar;
								}
								else if( str2.Equals( "HiScore.Bass" ) )
								{
									section = Eセクション種別.HiScoreBass;
                                }
                                else if (str2.Equals("HiSkill.Bass"))
                                {
                                    section = Eセクション種別.HiSkillBass;
                                }
                                // #23595 2011.1.9 ikanick
                                else if (str2.Equals("LastPlay.Drums"))
                                {
                                    section = Eセクション種別.LastPlayDrums;
                                }
                                else if (str2.Equals("LastPlay.Guitar"))
                                {
                                    section = Eセクション種別.LastPlayGuitar;
                                }
                                else if (str2.Equals("LastPlay.Bass"))
                                {
                                    section = Eセクション種別.LastPlayBass;
                                }
                                //----------------------------------------------------
								else
								{
									section = Eセクション種別.Unknown;
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
										case Eセクション種別.File:
											{
												if( !item.Equals( "Title" ) )
												{
													goto Label_01C7;
												}
												this.stファイル.Title = para;
												continue;
											}
										case Eセクション種別.HiScoreDrums:
										case Eセクション種別.HiSkillDrums:
										case Eセクション種別.HiScoreGuitar:
										case Eセクション種別.HiSkillGuitar:
										case Eセクション種別.HiScoreBass:
                                        case Eセクション種別.HiSkillBass:
                                        case Eセクション種別.LastPlayDrums:// #23595 2011.1.9 ikanick
                                        case Eセクション種別.LastPlayGuitar:
                                        case Eセクション種別.LastPlayBass:
											{
												c演奏記録 = this.stセクション[ (int) section ];
												if( !item.Equals( "Score" ) )
												{
													goto Label_03B9;
												}
												c演奏記録.nスコア = long.Parse( para );
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
								this.stファイル.Name = para;
							}
							else if( item.Equals( "Hash" ) )
							{
								this.stファイル.Hash = para;
							}
							else if( item.Equals( "PlayCountDrums" ) )
							{
								this.stファイル.PlayCountDrums = C変換.n値を文字列から取得して範囲内に丸めて返す( para, 0, 99999999, 0 );
							}
							else if( item.Equals( "PlayCountGuitars" ) )// #23596 11.2.5 changed ikanick
							{
								this.stファイル.PlayCountGuitar = C変換.n値を文字列から取得して範囲内に丸めて返す( para, 0, 99999999, 0 );
							}
							else if( item.Equals( "PlayCountBass" ) )
							{
								this.stファイル.PlayCountBass = C変換.n値を文字列から取得して範囲内に丸めて返す( para, 0, 99999999, 0 );
                            }
                            // #23596 10.11.16 add ikanick------------------------------------/
                            else if (item.Equals("ClearCountDrums"))
                            {
                                this.stファイル.ClearCountDrums = C変換.n値を文字列から取得して範囲内に丸めて返す(para, 0, 99999999, 0);
                            }
                            else if (item.Equals("ClearCountGuitars"))// #23596 11.2.5 changed ikanick
                            {
                                this.stファイル.ClearCountGuitar = C変換.n値を文字列から取得して範囲内に丸めて返す(para, 0, 99999999, 0);
                            }
                            else if (item.Equals("ClearCountBass"))
                            {
                                this.stファイル.ClearCountBass = C変換.n値を文字列から取得して範囲内に丸めて返す(para, 0, 99999999, 0);
                            }
                            // #24459 2011.2.24 yyagi-----------------------------------------/
							else if ( item.Equals( "BestRankDrums" ) )
							{
								this.stファイル.BestRank.Drums = C変換.n値を文字列から取得して範囲内に丸めて返す( para, (int) ERANK.SS, (int) ERANK.E, (int) ERANK.UNKNOWN );
							}
							else if ( item.Equals( "BestRankGuitar" ) )
							{
								this.stファイル.BestRank.Guitar = C変換.n値を文字列から取得して範囲内に丸めて返す( para, (int) ERANK.SS, (int) ERANK.E, (int) ERANK.UNKNOWN );
							}
							else if ( item.Equals( "BestRankBass" ) )
							{
								this.stファイル.BestRank.Bass = C変換.n値を文字列から取得して範囲内に丸めて返す( para, (int) ERANK.SS, (int) ERANK.E, (int) ERANK.UNKNOWN );
							}
							//----------------------------------------------------------------/
							else if ( item.Equals( "History0" ) )
							{
								this.stファイル.History[ 0 ] = para;
							}
							else if( item.Equals( "History1" ) )
							{
								this.stファイル.History[ 1 ] = para;
							}
							else if( item.Equals( "History2" ) )
							{
								this.stファイル.History[ 2 ] = para;
							}
							else if( item.Equals( "History3" ) )
							{
								this.stファイル.History[ 3 ] = para;
							}
							else if( item.Equals( "History4" ) )
							{
								this.stファイル.History[ 4 ] = para;
							}
							else if( item.Equals( "HistoryCount" ) )
							{
								this.stファイル.HistoryCount = C変換.n値を文字列から取得して範囲内に丸めて返す( para, 0, 99999999, 0 );
							}
							else if( item.Equals( "BGMAdjust" ) )
							{
								this.stファイル.BGMAdjust = C変換.n値を文字列から取得して返す( para, 0 );
							}
							continue;
							#endregion
							#region [ Score section ]
						Label_03B9:
							if( item.Equals( "PlaySkill" ) )
							{
								c演奏記録.db演奏型スキル値 = (double) decimal.Parse( para );
							}
							else if( item.Equals( "Skill" ) )
							{
								c演奏記録.dbゲーム型スキル値 = (double) decimal.Parse( para );
							}
							else if( item.Equals( "Perfect" ) )
							{
								c演奏記録.nPerfect数 = int.Parse( para );
							}
							else if( item.Equals( "Great" ) )
							{
								c演奏記録.nGreat数 = int.Parse( para );
							}
							else if( item.Equals( "Good" ) )
							{
								c演奏記録.nGood数 = int.Parse( para );
							}
							else if( item.Equals( "Poor" ) )
							{
								c演奏記録.nPoor数 = int.Parse( para );
							}
							else if( item.Equals( "Miss" ) )
							{
								c演奏記録.nMiss数 = int.Parse( para );
							}
							else if( item.Equals( "MaxCombo" ) )
							{
								c演奏記録.n最大コンボ数 = int.Parse( para );
							}
							else if( item.Equals( "TotalChips" ) )
							{
								c演奏記録.n全チップ数 = int.Parse( para );
							}
							else if( item.Equals( "AutoPlay" ) )
							{
								// LCなし               LCあり               CYとRDが別           Gt/Bs autolane/pick
								if( para.Length == 9 || para.Length == 10 || para.Length == 11 || para.Length == 21 )
								{
									for( int i = 0; i < para.Length; i++ )
									{
										c演奏記録.bAutoPlay[ i ] = this.ONorOFF( para[ i ] );
									}
								}
							}
							else if ( item.Equals( "Risky" ) )
							{
								c演奏記録.nRisky = int.Parse( para );
							}
							else if ( item.Equals( "TightDrums" ) )
							{
								c演奏記録.bTight = C変換.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "SuddenDrums" ) )
							{
								c演奏記録.bSudden.Drums = C変換.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "SuddenGuitar" ) )
							{
								c演奏記録.bSudden.Guitar = C変換.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "SuddenBass" ) )
							{
								c演奏記録.bSudden.Bass = C変換.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "HiddenDrums" ) )
							{
								c演奏記録.bHidden.Drums = C変換.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "HiddenGuitar" ) )
							{
								c演奏記録.bHidden.Guitar = C変換.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "HiddenBass" ) )
							{
								c演奏記録.bHidden.Bass = C変換.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "ReverseDrums" ) )
							{
								c演奏記録.bReverse.Drums = C変換.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "ReverseGuitar" ) )
							{
								c演奏記録.bReverse.Guitar = C変換.bONorOFF( para[ 0 ] );
							}
							else if ( item.Equals( "ReverseBass" ) )
							{
								c演奏記録.bReverse.Bass = C変換.bONorOFF( para[ 0 ] );
							}
							#endregion
							else
							{
								#region [ RandomGuitar ]
								if ( item.Equals( "RandomGuitar" ) )
								{
									switch ( int.Parse( para ) )
									{
										case (int) Eランダムモード.OFF:
											{
												c演奏記録.eRandom.Guitar = Eランダムモード.OFF;
												continue;
											}
										case (int) Eランダムモード.RANDOM:
											{
												c演奏記録.eRandom.Guitar = Eランダムモード.RANDOM;
												continue;
											}
										case (int) Eランダムモード.SUPERRANDOM:
											{
												c演奏記録.eRandom.Guitar = Eランダムモード.SUPERRANDOM;
												continue;
											}
										case (int) Eランダムモード.HYPERRANDOM:		// #25452 2011.6.20 yyagi
											{
												c演奏記録.eRandom.Guitar = Eランダムモード.SUPERRANDOM;
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
										case (int) Eランダムモード.OFF:
											{
												c演奏記録.eRandom.Bass = Eランダムモード.OFF;
												continue;
											}
										case (int) Eランダムモード.RANDOM:
											{
												c演奏記録.eRandom.Bass = Eランダムモード.RANDOM;
												continue;
											}
										case (int) Eランダムモード.SUPERRANDOM:
											{
												c演奏記録.eRandom.Bass = Eランダムモード.SUPERRANDOM;
												continue;
											}
										case (int) Eランダムモード.HYPERRANDOM:		// #25452 2011.6.20 yyagi
											{
												c演奏記録.eRandom.Bass = Eランダムモード.SUPERRANDOM;
												continue;
											}
									}
									throw new Exception( "RandomBass の値が無効です。" );
								}
								#endregion
								#region [ LightGuitar ]
								if ( item.Equals( "LightGuitar" ) )
								{
									c演奏記録.bLight.Guitar = C変換.bONorOFF( para[ 0 ] );
								}
								#endregion
								#region [ LightBass ]
								else if ( item.Equals( "LightBass" ) )
								{
									c演奏記録.bLight.Bass = C変換.bONorOFF( para[ 0 ] );
								}
								#endregion
								#region [ LeftGuitar ]
								else if ( item.Equals( "LeftGuitar" ) )
								{
									c演奏記録.bLeft.Guitar = C変換.bONorOFF( para[ 0 ] );
								}
								#endregion
								#region [ LeftBass ]
								else if ( item.Equals( "LeftBass" ) )
								{
									c演奏記録.bLeft.Bass = C変換.bONorOFF( para[ 0 ] );
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
													c演奏記録.eDark = Eダークモード.OFF;
													continue;
												}
											case 1:
												{
													c演奏記録.eDark = Eダークモード.HALF;
													continue;
												}
											case 2:
												{
													c演奏記録.eDark = Eダークモード.FULL;
													continue;
												}
										}
										throw new Exception( "Dark の値が無効です。" );
									}
									#endregion
									#region [ ScrollSpeedDrums ]
									if ( item.Equals( "ScrollSpeedDrums" ) )
									{
										c演奏記録.f譜面スクロール速度.Drums = (float) decimal.Parse( para );
									}
									#endregion
									#region [ ScrollSpeedGuitar ]
									else if ( item.Equals( "ScrollSpeedGuitar" ) )
									{
										c演奏記録.f譜面スクロール速度.Guitar = (float) decimal.Parse( para );
									}
									#endregion
									#region [ ScrollSpeedBass ]
									else if ( item.Equals( "ScrollSpeedBass" ) )
									{
										c演奏記録.f譜面スクロール速度.Bass = (float) decimal.Parse( para );
									}
									#endregion
									#region [ PlaySpeed ]
									else if ( item.Equals( "PlaySpeed" ) )
									{
										string[] strArray2 = para.Split( new char[] { '/' } );
										if ( strArray2.Length == 2 )
										{
											c演奏記録.n演奏速度分子 = int.Parse( strArray2[ 0 ] );
											c演奏記録.n演奏速度分母 = int.Parse( strArray2[ 1 ] );
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
														c演奏記録.eHHGroup = EHHGroup.全部打ち分ける;
														continue;
													}
												case 1:
													{
														c演奏記録.eHHGroup = EHHGroup.ハイハットのみ打ち分ける;
														continue;
													}
												case 2:
													{
														c演奏記録.eHHGroup = EHHGroup.左シンバルのみ打ち分ける;
														continue;
													}
												case 3:
													{
														c演奏記録.eHHGroup = EHHGroup.全部共通;
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
														c演奏記録.eFTGroup = EFTGroup.打ち分ける;
														continue;
													}
												case 1:
													{
														c演奏記録.eFTGroup = EFTGroup.共通;
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
														c演奏記録.eCYGroup = ECYGroup.打ち分ける;
														continue;
													}
												case 1:
													{
														c演奏記録.eCYGroup = ECYGroup.共通;
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
                                                        c演奏記録.eBDGroup = EBDGroup.打ち分ける;
                                                        continue;
                                                    }
                                                case 1:
                                                    {
                                                        c演奏記録.eBDGroup = EBDGroup.左右ペダルのみ打ち分ける;
                                                        continue;
                                                    }
                                                case 2:
                                                    {
                                                        c演奏記録.eBDGroup = EBDGroup.どっちもBD;
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
														c演奏記録.eHitSoundPriorityHH = E打ち分け時の再生の優先順位.ChipがPadより優先;
														continue;
													}
												case 1:
													{
														c演奏記録.eHitSoundPriorityHH = E打ち分け時の再生の優先順位.PadがChipより優先;
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
														c演奏記録.eHitSoundPriorityFT = E打ち分け時の再生の優先順位.ChipがPadより優先;
														continue;
													}
												case 1:
													{
														c演奏記録.eHitSoundPriorityFT = E打ち分け時の再生の優先順位.PadがChipより優先;
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
														c演奏記録.eHitSoundPriorityCY = E打ち分け時の再生の優先順位.ChipがPadより優先;
														continue;
													}
												case 1:
													{
														c演奏記録.eHitSoundPriorityCY = E打ち分け時の再生の優先順位.PadがChipより優先;
														continue;
													}
											}
											throw new Exception( "HitSoundPriorityCY の値が無効です。" );
										}
										#endregion
										#region [ Guitar ]
										if ( item.Equals( "Guitar" ) )
										{
											c演奏記録.bGuitar有効 = C変換.bONorOFF( para[ 0 ] );
										}
										#endregion
										#region [ Drums ]
										else if ( item.Equals( "Drums" ) )
										{
											c演奏記録.bDrums有効 = C変換.bONorOFF( para[ 0 ] );
										}
										#endregion
										#region [ StageFailed ]
										else if ( item.Equals( "StageFailed" ) )
										{
											c演奏記録.bSTAGEFAILED有効 = C変換.bONorOFF( para[ 0 ] );
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
															c演奏記録.eダメージレベル = Eダメージレベル.少ない;
															continue;
														}
													case 1:
														{
															c演奏記録.eダメージレベル = Eダメージレベル.普通;
															continue;
														}
													case 2:
														{
															c演奏記録.eダメージレベル = Eダメージレベル.大きい;
															continue;
														}
												}
												throw new Exception( "DamageLevel の値が無効です。" );
											}
											#endregion
											if ( item.Equals( "UseKeyboard" ) )
											{
												c演奏記録.b演奏にキーボードを使用した = C変換.bONorOFF( para[ 0 ] );
											}
											else if ( item.Equals( "UseMIDIIN" ) )
											{
												c演奏記録.b演奏にMIDI入力を使用した = C変換.bONorOFF( para[ 0 ] );
											}
											else if ( item.Equals( "UseJoypad" ) )
											{
												c演奏記録.b演奏にジョイパッドを使用した = C変換.bONorOFF( para[ 0 ] );
											}
											else if ( item.Equals( "UseMouse" ) )
											{
												c演奏記録.b演奏にマウスを使用した = C変換.bONorOFF( para[ 0 ] );
											}
											else if ( item.Equals( "PerfectRange" ) )
											{
												c演奏記録.nPerfectになる範囲ms = int.Parse( para );
											}
											else if ( item.Equals( "GreatRange" ) )
											{
												c演奏記録.nGreatになる範囲ms = int.Parse( para );
											}
											else if ( item.Equals( "GoodRange" ) )
											{
												c演奏記録.nGoodになる範囲ms = int.Parse( para );
											}
											else if ( item.Equals( "PoorRange" ) )
											{
												c演奏記録.nPoorになる範囲ms = int.Parse( para );
											}
											else if ( item.Equals( "DTXManiaVersion" ) )
											{
												c演奏記録.strDTXManiaのバージョン = para;
											}
											else if ( item.Equals( "DateTime" ) )
											{
												c演奏記録.最終更新日時 = para;
											}
											else if ( item.Equals( "Hash" ) )
											{
												c演奏記録.Hash = para;
											}
											else if ( item.Equals( "9LaneMode" ) )
											{
												c演奏記録.レーン9モード = C変換.bONorOFF( para[ 0 ] );
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

		internal void tヒストリを追加する( string str追加文字列 )
		{
			this.stファイル.HistoryCount++;
			for( int i = 3; i >= 0; i-- )
				this.stファイル.History[ i + 1 ] = this.stファイル.History[ i ];
			DateTime now = DateTime.Now;
			this.stファイル.History[ 0 ] = string.Format( "{0:0}.{1:D2}/{2}/{3} {4}", this.stファイル.HistoryCount, now.Year % 100, now.Month, now.Day, str追加文字列 );
		}
		internal void t書き出し( string iniファイル名 )
		{
			this.iniファイルのあるフォルダ名 = Path.GetDirectoryName( iniファイル名 );
			this.iniファイル名 = Path.GetFileName( iniファイル名 );

			StreamWriter writer = new StreamWriter( iniファイル名, false, Encoding.GetEncoding( "shift-jis" ) );
			writer.WriteLine( "[File]" );
			writer.WriteLine( "Title={0}", this.stファイル.Title );
			writer.WriteLine( "Name={0}", this.stファイル.Name );
			writer.WriteLine( "Hash={0}", this.stファイル.Hash );
			writer.WriteLine( "PlayCountDrums={0}", this.stファイル.PlayCountDrums );
			writer.WriteLine( "PlayCountGuitars={0}", this.stファイル.PlayCountGuitar );
            writer.WriteLine( "PlayCountBass={0}", this.stファイル.PlayCountBass );
            writer.WriteLine( "ClearCountDrums={0}", this.stファイル.ClearCountDrums );       // #23596 10.11.16 add ikanick
            writer.WriteLine( "ClearCountGuitars={0}", this.stファイル.ClearCountGuitar );    //
            writer.WriteLine( "ClearCountBass={0}", this.stファイル.ClearCountBass );         //
			writer.WriteLine( "BestRankDrums={0}", this.stファイル.BestRank.Drums );		// #24459 2011.2.24 yyagi
			writer.WriteLine( "BestRankGuitar={0}", this.stファイル.BestRank.Guitar );		//
			writer.WriteLine( "BestRankBass={0}", this.stファイル.BestRank.Bass );			//
			writer.WriteLine( "HistoryCount={0}", this.stファイル.HistoryCount );
			writer.WriteLine( "History0={0}", this.stファイル.History[ 0 ] );
			writer.WriteLine( "History1={0}", this.stファイル.History[ 1 ] );
			writer.WriteLine( "History2={0}", this.stファイル.History[ 2 ] );
			writer.WriteLine( "History3={0}", this.stファイル.History[ 3 ] );
			writer.WriteLine( "History4={0}", this.stファイル.History[ 4 ] );
			writer.WriteLine( "BGMAdjust={0}", this.stファイル.BGMAdjust );
			writer.WriteLine();
			for( int i = 0; i < 9; i++ )
			{
                string[] strArray = { "HiScore.Drums", "HiSkill.Drums", "HiScore.Guitar", "HiSkill.Guitar", "HiScore.Bass", "HiSkill.Bass", "LastPlay.Drums", "LastPlay.Guitar", "LastPlay.Bass" };
				writer.WriteLine( "[{0}]", strArray[ i ] );
				writer.WriteLine( "Score={0}", this.stセクション[ i ].nスコア );
				writer.WriteLine( "PlaySkill={0}", this.stセクション[ i ].db演奏型スキル値 );
				writer.WriteLine( "Skill={0}", this.stセクション[ i ].dbゲーム型スキル値 );
				writer.WriteLine( "Perfect={0}", this.stセクション[ i ].nPerfect数 );
				writer.WriteLine( "Great={0}", this.stセクション[ i ].nGreat数 );
				writer.WriteLine( "Good={0}", this.stセクション[ i ].nGood数 );
				writer.WriteLine( "Poor={0}", this.stセクション[ i ].nPoor数 );
				writer.WriteLine( "Miss={0}", this.stセクション[ i ].nMiss数 );
				writer.WriteLine( "MaxCombo={0}", this.stセクション[ i ].n最大コンボ数 );
				writer.WriteLine( "TotalChips={0}", this.stセクション[ i ].n全チップ数 );
				writer.Write( "AutoPlay=" );
				for ( int j = 0; j < (int) Eレーン.MAX; j++ )
				{
					writer.Write( this.stセクション[ i ].bAutoPlay[ j ] ? 1 : 0 );
				}
				writer.WriteLine();
				writer.WriteLine( "Risky={0}", this.stセクション[ i ].nRisky );
				writer.WriteLine( "SuddenDrums={0}", this.stセクション[ i ].bSudden.Drums ? 1 : 0 );
				writer.WriteLine( "SuddenGuitar={0}", this.stセクション[ i ].bSudden.Guitar ? 1 : 0 );
				writer.WriteLine( "SuddenBass={0}", this.stセクション[ i ].bSudden.Bass ? 1 : 0 );
				writer.WriteLine( "HiddenDrums={0}", this.stセクション[ i ].bHidden.Drums ? 1 : 0 );
				writer.WriteLine( "HiddenGuitar={0}", this.stセクション[ i ].bHidden.Guitar ? 1 : 0 );
				writer.WriteLine( "HiddenBass={0}", this.stセクション[ i ].bHidden.Bass ? 1 : 0 );
				writer.WriteLine( "ReverseDrums={0}", this.stセクション[ i ].bReverse.Drums ? 1 : 0 );
				writer.WriteLine( "ReverseGuitar={0}", this.stセクション[ i ].bReverse.Guitar ? 1 : 0 );
				writer.WriteLine( "ReverseBass={0}", this.stセクション[ i ].bReverse.Bass ? 1 : 0 );
				writer.WriteLine( "TightDrums={0}", this.stセクション[ i ].bTight ? 1 : 0 );
				writer.WriteLine( "RandomGuitar={0}", (int) this.stセクション[ i ].eRandom.Guitar );
				writer.WriteLine( "RandomBass={0}", (int) this.stセクション[ i ].eRandom.Bass );
				writer.WriteLine( "LightGuitar={0}", this.stセクション[ i ].bLight.Guitar ? 1 : 0 );
				writer.WriteLine( "LightBass={0}", this.stセクション[ i ].bLight.Bass ? 1 : 0 );
				writer.WriteLine( "LeftGuitar={0}", this.stセクション[ i ].bLeft.Guitar ? 1 : 0 );
				writer.WriteLine( "LeftBass={0}", this.stセクション[ i ].bLeft.Bass ? 1 : 0 );
				writer.WriteLine( "Dark={0}", (int) this.stセクション[ i ].eDark );
				writer.WriteLine( "ScrollSpeedDrums={0}", this.stセクション[ i ].f譜面スクロール速度.Drums );
				writer.WriteLine( "ScrollSpeedGuitar={0}", this.stセクション[ i ].f譜面スクロール速度.Guitar );
				writer.WriteLine( "ScrollSpeedBass={0}", this.stセクション[ i ].f譜面スクロール速度.Bass );
				writer.WriteLine( "PlaySpeed={0}/{1}", this.stセクション[ i ].n演奏速度分子, this.stセクション[ i ].n演奏速度分母 );
				writer.WriteLine( "HHGroup={0}", (int) this.stセクション[ i ].eHHGroup );
				writer.WriteLine( "FTGroup={0}", (int) this.stセクション[ i ].eFTGroup );
				writer.WriteLine( "CYGroup={0}", (int) this.stセクション[ i ].eCYGroup );
                writer.WriteLine( "BDGroup={0}", (int) this.stセクション[ i ].eBDGroup);
				writer.WriteLine( "HitSoundPriorityHH={0}", (int) this.stセクション[ i ].eHitSoundPriorityHH );
				writer.WriteLine( "HitSoundPriorityFT={0}", (int) this.stセクション[ i ].eHitSoundPriorityFT );
				writer.WriteLine( "HitSoundPriorityCY={0}", (int) this.stセクション[ i ].eHitSoundPriorityCY );
				writer.WriteLine( "Guitar={0}", this.stセクション[ i ].bGuitar有効 ? 1 : 0 );
				writer.WriteLine( "Drums={0}", this.stセクション[ i ].bDrums有効 ? 1 : 0 );
				writer.WriteLine( "StageFailed={0}", this.stセクション[ i ].bSTAGEFAILED有効 ? 1 : 0 );
				writer.WriteLine( "DamageLevel={0}", (int) this.stセクション[ i ].eダメージレベル );
				writer.WriteLine( "UseKeyboard={0}", this.stセクション[ i ].b演奏にキーボードを使用した ? 1 : 0 );
				writer.WriteLine( "UseMIDIIN={0}", this.stセクション[ i ].b演奏にMIDI入力を使用した ? 1 : 0 );
				writer.WriteLine( "UseJoypad={0}", this.stセクション[ i ].b演奏にジョイパッドを使用した ? 1 : 0 );
				writer.WriteLine( "UseMouse={0}", this.stセクション[ i ].b演奏にマウスを使用した ? 1 : 0 );
				writer.WriteLine( "PerfectRange={0}", this.stセクション[ i ].nPerfectになる範囲ms );
				writer.WriteLine( "GreatRange={0}", this.stセクション[ i ].nGreatになる範囲ms );
				writer.WriteLine( "GoodRange={0}", this.stセクション[ i ].nGoodになる範囲ms );
				writer.WriteLine( "PoorRange={0}", this.stセクション[ i ].nPoorになる範囲ms );
				writer.WriteLine( "DTXManiaVersion={0}", this.stセクション[ i ].strDTXManiaのバージョン );
				writer.WriteLine( "DateTime={0}", this.stセクション[ i ].最終更新日時 );
				writer.WriteLine( "Hash={0}", this.stセクション[ i ].Hash );
			}
			writer.Close();
		}
		internal void t全演奏記録セクションの整合性をチェックし不整合があればリセットする()
		{
			for( int i = 0; i < 9; i++ )
			{
				if( !this.b整合性がある( (Eセクション種別) i ) )
					this.stセクション[ i ] = new C演奏記録();
			}
        }
        internal static int tランク値を計算して返す(C演奏記録 part)
        {
            if (part.b演奏にMIDI入力を使用した || part.b演奏にキーボードを使用した || part.b演奏にジョイパッドを使用した || part.b演奏にマウスを使用した)	// 2010.9.11
            {
                int nTotal = part.nPerfect数 + part.nGreat数 + part.nGood数 + part.nPoor数 + part.nMiss数;
                return tランク値を計算して返す(nTotal, part.nPerfect数, part.nGreat数, part.nGood数, part.nPoor数, part.nMiss数, part.n最大コンボ数);
            }
            return (int)ERANK.UNKNOWN;
        }
        
        /// <summary>
        /// nDummy 適当な数値を入れてください。特に使いません。
        /// dRate 達成率を入れます。
        /// </summary>
        internal static int tランク値を計算して返す( int nDummy, double dRate )
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
        internal static int tランク値を計算して返す(int nTotal, int nPerfect, int nGreat, int nGood, int nPoor, int nMiss, int nCombo)
        {
            if (nTotal <= 0)
                return (int)ERANK.UNKNOWN;

            //int nRank = (int)ERANK.E;
            int nAuto = nTotal - (nPerfect + nGreat + nGood + nPoor + nMiss);
            if (nTotal <= nAuto)
            {
                return (int)ERANK.SS;
            }
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
        internal static double tゲーム型スキルを計算して返す(double dbLevel, int nLevelDec, int nTotal, int nPerfect, int nGreat, int nCombo, E楽器パート inst, STAUTOPLAY bAutoPlay)
        {
            //こちらはプレイヤースキル・全曲スキルに加算される得点。いわゆる曲別スキル。
            double dbPERFECT率 = (100.0 * nPerfect / nTotal);
            double dbGREAT率 = (100.0 * nGreat / nTotal);
            double dbCOMBO率 = (100.0 * nCombo / nTotal);

            double ret;
            double dbRate = (dbPERFECT率 * 0.85 + dbGREAT率 * 0.35 + dbCOMBO率 * 0.15);
            if ((nTotal == 0) || ((nPerfect == 0) && (nCombo == 0)))
                ret = 0.0;

            if (dbLevel >= 100)
            {
                dbLevel = dbLevel / 100.0;
            }
            else if (dbLevel < 100)
            {
                dbLevel = dbLevel / 10.0 + ( nLevelDec != 0 ? 0 : nLevelDec / 100.0 );
            }

            ret = ((dbRate * dbLevel * 0.2));
            ret *= dbCalcReviseValForDrGtBsAutoLanes(inst, bAutoPlay);
            if (CDTXMania.ConfigIni.bドラムが全部オートプレイである)
            {
                return 0;
            }
            return ret;
        }
        internal static double t演奏型スキルを計算して返す(int nTotal, int nPerfect, int nGreat, int nGood, int nPoor, int nMiss, int nCombo, E楽器パート inst, STAUTOPLAY bAutoPlay)
        {
            if (nTotal == 0)
                return 0.0;

            int nAuto = nTotal - (nPerfect + nGreat + nGood + nPoor + nMiss);
            double dbPERFECT率 = (100.0 * nPerfect / nTotal);
            double dbGREAT率 = (100.0 * nGreat / nTotal);
            double dbCOMBO率 = (100.0 * nCombo / (nTotal));

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
        internal static double tゴーストスキルを計算して返す(int nTotal, int nPerfect, int nCombo, E楽器パート inst)
        {
            if (nTotal == 0)
                return 0.0;

            double dbPERFECT率 = (100.0 * nPerfect / nTotal);
            double dbGREAT率 = (100.0 * nPerfect / nTotal);
            double dbCOMBO率 = (100.0 * nCombo / (nTotal));

            double ret = dbPERFECT率 * 0.85 + dbGREAT率 * 0.35 + dbCOMBO率 * 0.15;

            return ret;
        }
        internal static int t旧ランク値を計算して返す(C演奏記録 part)
        {
            if (part.b演奏にMIDI入力を使用した || part.b演奏にキーボードを使用した || part.b演奏にジョイパッドを使用した || part.b演奏にマウスを使用した)	// 2010.9.11
            {
                int nTotal = part.nPerfect数 + part.nGreat数 + part.nGood数 + part.nPoor数 + part.nMiss数;
                return t旧ランク値を計算して返す(nTotal, part.nPerfect数, part.nGreat数, part.nGood数, part.nPoor数, part.nMiss数);
            }
            return (int)ERANK.UNKNOWN;
        }
        internal static int t旧ランク値を計算して返す(int nTotal, int nPerfect, int nGreat, int nGood, int nPoor, int nMiss)
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
        internal static double t旧ゲーム型スキルを計算して返す( double dbLevel, int nLevelDec, int nTotal, int nPerfect, int nCombo, E楽器パート inst, STAUTOPLAY bAutoPlay )
        {
            double ret;
            if ( ( nTotal == 0 ) || ( ( nPerfect == 0 ) && ( nCombo == 0 ) ) )
                ret = 0.0;

            ret = ( ( dbLevel * ( ( nPerfect * 0.8 + nCombo * 0.2 ) / ( ( double )nTotal ) ) ) / 2.0 );
            ret *= dbCalcReviseValForDrGtBsAutoLanes( inst, bAutoPlay );
            if ( CDTXMania.ConfigIni.bドラムが全部オートプレイである )
            {
                return 0;
            }

            return ret;
        }
        internal static double t旧演奏型スキルを計算して返す(int nTotal, int nPerfect, int nGreat, int nGood, int nPoor, int nMiss, E楽器パート inst, STAUTOPLAY bAutoPlay)
        {
            if (nTotal == 0)
                return 0.0;

            int nAuto = nTotal - (nPerfect + nGreat + nGood + nPoor + nMiss);
            double y = ((nPerfect * 1.0 + nGreat * 0.8 + nGood * 0.5 + nPoor * 0.2 + nMiss * 0.0 + nAuto * 0.0) * 100.0) / ((double)nTotal);
            double ret = (100.0 * ((Math.Pow(1.03, y) - 1.0) / (Math.Pow(1.03, 100.0) - 1.0)));

            ret *= dbCalcReviseValForDrGtBsAutoLanes(inst, bAutoPlay);
            return ret;
        }
        internal static double t旧ゴーストスキルを計算して返す(int nTotal, int nPerfect, int nGreat, int nGood, int nPoor, int nMiss, E楽器パート inst)
        {
            if (nTotal == 0)
                return 0.0;
            int nAuto = nTotal - (nPerfect + nGreat + nGood + nPoor + nMiss);
            double y = ((nPerfect * 1.0 + nGreat * 0.8 + nGood * 0.5 + nPoor * 0.2 + nMiss * 0.0 + nAuto * 0.0) * 100.0) / ((double)nTotal);
            double ret = (100.0 * ((Math.Pow(1.03, y) - 1.0) / (Math.Pow(1.03, 100.0) - 1.0)));

            return ret;
        }
        internal static double dbCalcReviseValForDrGtBsAutoLanes(E楽器パート inst, STAUTOPLAY bAutoPlay)	// #28607 2012.6.7 yyagi
        {
            double ret = 1.0;

            switch (inst)
            {
                #region [ Unknown ]
                case E楽器パート.UNKNOWN:
                    throw new ArgumentException();
                #endregion
                #region [ Drums ]
                case E楽器パート.DRUMS:
                    if (!CDTXMania.ConfigIni.bドラムが全部オートプレイである)
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
                case E楽器パート.GUITAR:
                    if (!CDTXMania.ConfigIni.bギターが全部オートプレイである)
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
                case E楽器パート.BASS:
                    if (!CDTXMania.ConfigIni.bベースが全部オートプレイである)
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
		internal static string t演奏セクションのMD5を求めて返す( C演奏記録 cc )
		{
			StringBuilder builder = new StringBuilder();
			builder.Append( cc.nスコア.ToString() );
			builder.Append( cc.dbゲーム型スキル値.ToString( ".000000" ) );
			builder.Append( cc.db演奏型スキル値.ToString( ".000000" ) );
			builder.Append( cc.nPerfect数 );
			builder.Append( cc.nGreat数 );
			builder.Append( cc.nGood数 );
			builder.Append( cc.nPoor数 );
			builder.Append( cc.nMiss数 );
			builder.Append( cc.n最大コンボ数 );
			builder.Append( cc.n全チップ数 );
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
			builder.Append( cc.f譜面スクロール速度.Drums.ToString( ".000000" ) );
			builder.Append( cc.f譜面スクロール速度.Guitar.ToString( ".000000" ) );
			builder.Append( cc.f譜面スクロール速度.Bass.ToString( ".000000" ) );
			builder.Append( cc.n演奏速度分子 );
			builder.Append( cc.n演奏速度分母 );
			builder.Append( (int) cc.eHHGroup );
			builder.Append( (int) cc.eFTGroup );
			builder.Append( (int) cc.eCYGroup );
			builder.Append( (int) cc.eHitSoundPriorityHH );
			builder.Append( (int) cc.eHitSoundPriorityFT );
			builder.Append( (int) cc.eHitSoundPriorityCY );
			builder.Append( boolToChar( cc.bGuitar有効 ) );
			builder.Append( boolToChar( cc.bDrums有効 ) );
			builder.Append( boolToChar( cc.bSTAGEFAILED有効 ) );
			builder.Append( (int) cc.eダメージレベル );
			builder.Append( boolToChar( cc.b演奏にキーボードを使用した ) );
			builder.Append( boolToChar( cc.b演奏にMIDI入力を使用した ) );
			builder.Append( boolToChar( cc.b演奏にジョイパッドを使用した ) );
			builder.Append( boolToChar( cc.b演奏にマウスを使用した ) );
			builder.Append( cc.nPerfectになる範囲ms );
			builder.Append( cc.nGreatになる範囲ms );
			builder.Append( cc.nGoodになる範囲ms );
			builder.Append( cc.nPoorになる範囲ms );
			builder.Append( cc.strDTXManiaのバージョン );
			builder.Append( cc.最終更新日時 );

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
		internal static void t更新条件を取得する( out bool bDrumsを更新する, out bool bGuitarを更新する, out bool bBassを更新する )
		{
			bDrumsを更新する =  CDTXMania.ConfigIni.bDrums有効  && CDTXMania.DTX.bチップがある.Drums  && !CDTXMania.ConfigIni.bドラムが全部オートプレイである;
			bGuitarを更新する = CDTXMania.ConfigIni.bGuitar有効 && CDTXMania.DTX.bチップがある.Guitar && !CDTXMania.ConfigIni.bギターが全部オートプレイである;
			bBassを更新する =   CDTXMania.ConfigIni.bGuitar有効 && CDTXMania.DTX.bチップがある.Bass   && !CDTXMania.ConfigIni.bベースが全部オートプレイである;
		}
        internal static int t総合ランク値を計算して返す(C演奏記録 Drums, C演奏記録 Guitar, C演奏記録 Bass)
        {
            int nTotal = Drums.n全チップ数 + Guitar.n全チップ数 + Bass.n全チップ数;
            int nPerfect = Drums.nPerfect数・Auto含まない + Guitar.nPerfect数・Auto含まない + Bass.nPerfect数・Auto含まない;	// #24569 2011.3.1 yyagi: to calculate result rank without AUTO chips
            int nGreat = Drums.nGreat数・Auto含まない + Guitar.nGreat数・Auto含まない + Bass.nGreat数・Auto含まない;		//
            int nGood = Drums.nGood数・Auto含まない + Guitar.nGood数・Auto含まない + Bass.nGood数・Auto含まない;		//
            int nPoor = Drums.nPoor数・Auto含まない + Guitar.nPoor数・Auto含まない + Bass.nPoor数・Auto含まない;		//
            int nMiss = Drums.nMiss数・Auto含まない + Guitar.nMiss数・Auto含まない + Bass.nMiss数・Auto含まない;		//
            int nCombo = Drums.n最大コンボ数 + Guitar.n最大コンボ数 + Bass.n最大コンボ数;		//
            if (CDTXMania.ConfigIni.nSkillMode == 0)
            {
                return t旧ランク値を計算して返す(nTotal, nPerfect, nGreat, nGood, nPoor, nMiss);
            }
            return tランク値を計算して返す(nTotal, nPerfect, nGreat, nGood, nPoor, nMiss, nCombo);
        }

		// その他

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
