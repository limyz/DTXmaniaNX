using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Threading;

namespace DTXMania
{
	[Serializable]
	internal class CSongs管理
	{
		// プロパティ

		public int nSongsDBから取得できたスコア数
		{
			get; 
			set; 
		}
		public int nSongsDBへ出力できたスコア数
		{
			get;
			set;
		}
		public int nスコアキャッシュから反映できたスコア数 
		{
			get;
			set; 
		}
		public int nファイルから反映できたスコア数
		{
			get;
			set;
		}
		public int n検索されたスコア数 
		{ 
			get;
			set;
		}
		public int n検索された曲ノード数
		{
			get; 
			set;
		}
		[NonSerialized]
		public List<Cスコア> listSongsDB;					// songs.dbから構築されるlist
		public List<C曲リストノード> list曲ルート;			// 起動時にフォルダ検索して構築されるlist
		public bool bIsSuspending							// 外部スレッドから、内部スレッドのsuspendを指示する時にtrueにする
		{													// 再開時は、これをfalseにしてから、次のautoReset.Set()を実行する
			get;
			set;
		}
		public bool bIsSlowdown								// #PREMOVIE再生時に曲検索を遅くする
		{
			get;
			set;
		}
		[NonSerialized]
		private AutoResetEvent autoReset;
		public AutoResetEvent AutoReset
		{
			get
			{
				return autoReset;
			}
			private set
			{
				autoReset = value;
			}
		}

		private int searchCount;							// #PREMOVIE中は検索n回実行したら少しスリープする

		// コンストラクタ

		public CSongs管理()
		{
			this.listSongsDB = new List<Cスコア>();
			this.list曲ルート = new List<C曲リストノード>();
			this.n検索された曲ノード数 = 0;
			this.n検索されたスコア数 = 0;
			this.bIsSuspending = false;						// #27060
			this.autoReset = new AutoResetEvent( true );	// #27060
			this.searchCount = 0;
		}


		// メソッド

		#region [ SongsDB(songs.db) を読み込む ]
		//-----------------
		public void tSongsDBを読み込む( string SongsDBファイル名 )
		{
			this.nSongsDBから取得できたスコア数 = 0;
			if( File.Exists( SongsDBファイル名 ) )
			{
				BinaryReader br = null;
				try
				{
					br = new BinaryReader( File.OpenRead( SongsDBファイル名 ) );
					if ( !br.ReadString().Equals( SONGSDB_VERSION ) )
					{
						throw new InvalidDataException( "ヘッダが異なります。" );
					}
					this.listSongsDB = new List<Cスコア>();

					while( true )
					{
						try
						{
							Cスコア item = this.tSongsDBからスコアを１つ読み込む( br );
							this.listSongsDB.Add( item );
							this.nSongsDBから取得できたスコア数++;
						}
						catch( EndOfStreamException )
						{
							break;
						}
					}
				}
				finally
				{
					if( br != null )
						br.Close();
				}
			}
		}
		//-----------------
		#endregion

		#region [ 曲を検索してリストを作成する ]
		//-----------------
		public void t曲を検索してリストを作成する( string str基点フォルダ, bool b子BOXへ再帰する )
		{
			this.t曲を検索してリストを作成する( str基点フォルダ, b子BOXへ再帰する, this.list曲ルート, null );
		}
		private void t曲を検索してリストを作成する( string str基点フォルダ, bool b子BOXへ再帰する, List<C曲リストノード> listノードリスト, C曲リストノード node親 )
		{
			if( !str基点フォルダ.EndsWith( @"\" ) )
				str基点フォルダ = str基点フォルダ + @"\";

			DirectoryInfo info = new DirectoryInfo( str基点フォルダ );

			if( CDTXMania.ConfigIni.bLog曲検索ログ出力 )
				Trace.TraceInformation( "基点フォルダ: " + str基点フォルダ );

			#region [ a.フォルダ内に set.def が存在する場合 → set.def からノード作成]
			//-----------------------------
			string path = str基点フォルダ + "set.def";
			if( File.Exists( path ) )
			{
				CSetDef def = new CSetDef( path );
				new FileInfo( path );
				if( CDTXMania.ConfigIni.bLog曲検索ログ出力 )
				{
					Trace.TraceInformation( "set.def検出 : {0}", path );
					Trace.Indent();
				}
				try
				{
					SlowOrSuspendSearchTask();		// #27060 中断要求があったら、解除要求が来るまで待機, #PREMOVIE再生中は検索負荷を落とす
					for( int i = 0; i < def.blocks.Count; i++ )
					{
						CSetDef.CBlock block = def.blocks[ i ];
						C曲リストノード item = new C曲リストノード();
						item.eノード種別 = C曲リストノード.Eノード種別.SCORE;
						item.strタイトル = block.Title;
						item.strジャンル = block.Genre;
						item.nスコア数 = 0;
						item.col文字色 = block.FontColor;
						item.SetDefのブロック番号 = i;
						item.pathSetDefの絶対パス = path;
						item.r親ノード = node親;

						item.strBreadcrumbs = ( item.r親ノード == null ) ?
							path + i : item.r親ノード.strBreadcrumbs + " > " + path + i;

						for( int j = 0; j < 5; j++ )
						{
							if( !string.IsNullOrEmpty( block.File[ j ] ) )
							{
								string str2 = str基点フォルダ + block.File[ j ];
								if( File.Exists( str2 ) )
								{
									item.ar難易度ラベル[ j ] = block.Label[ j ];
									item.arスコア[ j ] = new Cスコア();
									item.arスコア[ j ].ファイル情報.ファイルの絶対パス = str2;
									item.arスコア[ j ].ファイル情報.フォルダの絶対パス = Path.GetFullPath( Path.GetDirectoryName( str2 ) ) + @"\";
									FileInfo info2 = new FileInfo( str2 );
									item.arスコア[ j ].ファイル情報.ファイルサイズ = info2.Length;
									item.arスコア[ j ].ファイル情報.最終更新日時 = info2.LastWriteTime;
									string str3 = str2 + ".score.ini";
									if( File.Exists( str3 ) )
									{
										FileInfo info3 = new FileInfo( str3 );
										item.arスコア[ j ].ScoreIni情報.ファイルサイズ = info3.Length;
										item.arスコア[ j ].ScoreIni情報.最終更新日時 = info3.LastWriteTime;
									}
									item.nスコア数++;
									this.n検索されたスコア数++;
								}
								else
								{
									item.arスコア[ j ] = null;
								}
							}
						}
						if( item.nスコア数 > 0 )
						{
							listノードリスト.Add( item );
							this.n検索された曲ノード数++;
							if( CDTXMania.ConfigIni.bLog曲検索ログ出力 )
							{
								StringBuilder builder = new StringBuilder( 0x200 );
								builder.Append( string.Format( "nID#{0:D3}", item.nID ) );
								if( item.r親ノード != null )
								{
									builder.Append( string.Format( "(in#{0:D3}):", item.r親ノード.nID ) );
								}
								else
								{
									builder.Append( "(onRoot):" );
								}
								if( ( item.strタイトル != null ) && ( item.strタイトル.Length > 0 ) )
								{
									builder.Append( " SONG, Title=" + item.strタイトル );
								}
								if( ( item.strジャンル != null ) && ( item.strジャンル.Length > 0 ) )
								{
									builder.Append( ", Genre=" + item.strジャンル );
								}
								if( item.col文字色 != Color.White )
								{
									builder.Append( ", FontColor=" + item.col文字色 );
								}
								Trace.TraceInformation( builder.ToString() );
								Trace.Indent();
								try
								{
									for( int k = 0; k < 5; k++ )
									{
										if( item.arスコア[ k ] != null )
										{
											Cスコア cスコア = item.arスコア[ k ];
											builder.Remove( 0, builder.Length );
											builder.Append( string.Format( "ブロック{0}-{1}:", item.SetDefのブロック番号 + 1, k + 1 ) );
											builder.Append( " Label=" + item.ar難易度ラベル[ k ] );
											builder.Append( ", File=" + cスコア.ファイル情報.ファイルの絶対パス );
											builder.Append( ", Size=" + cスコア.ファイル情報.ファイルサイズ );
											builder.Append( ", LastUpdate=" + cスコア.ファイル情報.最終更新日時 );
											Trace.TraceInformation( builder.ToString() );
										}
									}
								}
								finally
								{
									Trace.Unindent();
								}
							}
						}
					}
				}
				finally
				{
					if( CDTXMania.ConfigIni.bLog曲検索ログ出力 )
					{
						Trace.Unindent();
					}
				}
			}
			//-----------------------------
			#endregion

			#region [ b.フォルダ内に set.def が存在しない場合 → 個別ファイルからノード作成 ]
			//-----------------------------
			else
			{
				foreach( FileInfo fileinfo in info.GetFiles() )
				{
					SlowOrSuspendSearchTask();		// #27060 中断要求があったら、解除要求が来るまで待機, #PREMOVIE再生中は検索負荷を落とす
					string strExt = fileinfo.Extension.ToLower();
					if( ( strExt.Equals( ".dtx" ) || strExt.Equals( ".gda" ) ) || ( ( strExt.Equals( ".g2d" ) || strExt.Equals( ".bms" ) ) || strExt.Equals( ".bme" ) ) )
					{
						C曲リストノード c曲リストノード = new C曲リストノード();
						c曲リストノード.eノード種別 = C曲リストノード.Eノード種別.SCORE;
						c曲リストノード.nスコア数 = 1;
						c曲リストノード.r親ノード = node親;

						c曲リストノード.strBreadcrumbs = ( c曲リストノード.r親ノード == null ) ?
							str基点フォルダ + fileinfo.Name : c曲リストノード.r親ノード.strBreadcrumbs + " > " + str基点フォルダ + fileinfo.Name;

						c曲リストノード.arスコア[ 0 ] = new Cスコア();
						c曲リストノード.arスコア[ 0 ].ファイル情報.ファイルの絶対パス = str基点フォルダ + fileinfo.Name;
						c曲リストノード.arスコア[ 0 ].ファイル情報.フォルダの絶対パス = str基点フォルダ;
						c曲リストノード.arスコア[ 0 ].ファイル情報.ファイルサイズ = fileinfo.Length;
						c曲リストノード.arスコア[ 0 ].ファイル情報.最終更新日時 = fileinfo.LastWriteTime;
						string strFileNameScoreIni = c曲リストノード.arスコア[ 0 ].ファイル情報.ファイルの絶対パス + ".score.ini";
						if( File.Exists( strFileNameScoreIni ) )
						{
							FileInfo infoScoreIni = new FileInfo( strFileNameScoreIni );
							c曲リストノード.arスコア[ 0 ].ScoreIni情報.ファイルサイズ = infoScoreIni.Length;
							c曲リストノード.arスコア[ 0 ].ScoreIni情報.最終更新日時 = infoScoreIni.LastWriteTime;
						}
						this.n検索されたスコア数++;
						listノードリスト.Add( c曲リストノード );
						this.n検索された曲ノード数++;
						if( CDTXMania.ConfigIni.bLog曲検索ログ出力 )
						{
							Trace.Indent();
							try
							{
								StringBuilder sb = new StringBuilder( 0x100 );
								sb.Append( string.Format( "nID#{0:D3}", c曲リストノード.nID ) );
								if( c曲リストノード.r親ノード != null )
								{
									sb.Append( string.Format( "(in#{0:D3}):", c曲リストノード.r親ノード.nID ) );
								}
								else
								{
									sb.Append( "(onRoot):" );
								}
								sb.Append( " SONG, File=" + c曲リストノード.arスコア[ 0 ].ファイル情報.ファイルの絶対パス );
								sb.Append( ", Size=" + c曲リストノード.arスコア[ 0 ].ファイル情報.ファイルサイズ );
								sb.Append( ", LastUpdate=" + c曲リストノード.arスコア[ 0 ].ファイル情報.最終更新日時 );
								Trace.TraceInformation( sb.ToString() );
							}
							finally
							{
								Trace.Unindent();
							}
						}
					}
					else if( strExt.Equals( ".mid" ) || strExt.Equals( ".smf" ))
					{
						// 何もしない
					}
				}
			}
			//-----------------------------
			#endregion

			foreach( DirectoryInfo infoDir in info.GetDirectories() )
			{
				SlowOrSuspendSearchTask();		// #27060 中断要求があったら、解除要求が来るまで待機, #PREMOVIE再生中は検索負荷を落とす

				#region [ a. "dtxfiles." で始まるフォルダの場合 ]
				//-----------------------------
				if( infoDir.Name.ToLower().StartsWith( "dtxfiles." ) )
				{
					C曲リストノード c曲リストノード = new C曲リストノード();
					c曲リストノード.eノード種別 = C曲リストノード.Eノード種別.BOX;
					c曲リストノード.bDTXFilesで始まるフォルダ名のBOXである = true;
					c曲リストノード.strタイトル = infoDir.Name.Substring( 9 );
					c曲リストノード.nスコア数 = 1;
					c曲リストノード.r親ノード = node親;

					// 一旦、上位BOXのスキン情報をコピー (後でbox.defの記載にて上書きされる場合がある)
					c曲リストノード.strSkinPath = ( c曲リストノード.r親ノード == null ) ?
						"" : c曲リストノード.r親ノード.strSkinPath;

					c曲リストノード.strBreadcrumbs = ( c曲リストノード.r親ノード == null ) ?
						c曲リストノード.strタイトル : c曲リストノード.r親ノード.strBreadcrumbs + " > " + c曲リストノード.strタイトル;

		
					c曲リストノード.list子リスト = new List<C曲リストノード>();
					c曲リストノード.arスコア[ 0 ] = new Cスコア();
					c曲リストノード.arスコア[ 0 ].ファイル情報.フォルダの絶対パス = infoDir.FullName + @"\";
					c曲リストノード.arスコア[ 0 ].譜面情報.タイトル = c曲リストノード.strタイトル;
					c曲リストノード.arスコア[ 0 ].譜面情報.コメント =
						(CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ja") ?
						"BOX に移動します。" :
						"Enter into the BOX.";
					listノードリスト.Add(c曲リストノード);
					if( File.Exists( infoDir.FullName + @"\box.def" ) )
					{
						CBoxDef boxdef = new CBoxDef( infoDir.FullName + @"\box.def" );
						if( ( boxdef.Title != null ) && ( boxdef.Title.Length > 0 ) )
						{
							c曲リストノード.strタイトル = boxdef.Title;
						}
						if( ( boxdef.Genre != null ) && ( boxdef.Genre.Length > 0 ) )
						{
							c曲リストノード.strジャンル = boxdef.Genre;
						}
						if( boxdef.Color != Color.White )
						{
							c曲リストノード.col文字色 = boxdef.Color;
						}
						if( ( boxdef.Artist != null ) && ( boxdef.Artist.Length > 0 ) )
						{
							c曲リストノード.arスコア[ 0 ].譜面情報.アーティスト名 = boxdef.Artist;
						}
						if( ( boxdef.Comment != null ) && ( boxdef.Comment.Length > 0 ) )
						{
							c曲リストノード.arスコア[ 0 ].譜面情報.コメント = boxdef.Comment;
						}
						if( ( boxdef.Preimage != null ) && ( boxdef.Preimage.Length > 0 ) )
						{
							c曲リストノード.arスコア[ 0 ].譜面情報.Preimage = boxdef.Preimage;
						}
						if( ( boxdef.Premovie != null ) && ( boxdef.Premovie.Length > 0 ) )
						{
							c曲リストノード.arスコア[ 0 ].譜面情報.Premovie = boxdef.Premovie;
						}
						if( ( boxdef.Presound != null ) && ( boxdef.Presound.Length > 0 ) )
						{
							c曲リストノード.arスコア[ 0 ].譜面情報.Presound = boxdef.Presound;
						}
						if ( boxdef.SkinPath != null )
						{
							if ( boxdef.SkinPath == "" )
							{
								// box.defにスキン情報が記載されていないなら、上位BOXのスキン情報をコピー
								c曲リストノード.strSkinPath = ( c曲リストノード.r親ノード == null ) ?
									"" : c曲リストノード.r親ノード.strSkinPath;
							}
							else
							{
								// box.defに記載されているスキン情報をコピー。末尾に必ず\をつけておくこと。
								string s = System.IO.Path.Combine( infoDir.FullName, boxdef.SkinPath );
								if ( s[ s.Length - 1 ] != System.IO.Path.DirectorySeparatorChar )	// フォルダ名末尾に\を必ずつけて、CSkin側と表記を統一する
								{
									s += System.IO.Path.DirectorySeparatorChar;
								}
								if ( CDTXMania.Skin.bIsValid( s ) )
								{
									c曲リストノード.strSkinPath = s;
								}
								else
								{
									c曲リストノード.strSkinPath = ( c曲リストノード.r親ノード == null ) ?
										"" : c曲リストノード.r親ノード.strSkinPath;
								}
							}
						}
						if ( boxdef.PerfectRange >= 0 )
						{
							c曲リストノード.nPerfect範囲ms = boxdef.PerfectRange;
						}
						if( boxdef.GreatRange >= 0 )
						{
							c曲リストノード.nGreat範囲ms = boxdef.GreatRange;
						}
						if( boxdef.GoodRange >= 0 )
						{
							c曲リストノード.nGood範囲ms = boxdef.GoodRange;
						}
						if( boxdef.PoorRange >= 0 )
						{
							c曲リストノード.nPoor範囲ms = boxdef.PoorRange;
						}
					}
					if( CDTXMania.ConfigIni.bLog曲検索ログ出力 )
					{
						Trace.Indent();
						try
						{
							StringBuilder sb = new StringBuilder( 0x100 );
							sb.Append( string.Format( "nID#{0:D3}", c曲リストノード.nID ) );
							if( c曲リストノード.r親ノード != null )
							{
								sb.Append( string.Format( "(in#{0:D3}):", c曲リストノード.r親ノード.nID ) );
							}
							else
							{
								sb.Append( "(onRoot):" );
							}
							sb.Append( " BOX, Title=" + c曲リストノード.strタイトル );
							sb.Append( ", Folder=" + c曲リストノード.arスコア[ 0 ].ファイル情報.フォルダの絶対パス );
							sb.Append( ", Comment=" + c曲リストノード.arスコア[ 0 ].譜面情報.コメント );
							sb.Append( ", SkinPath=" + c曲リストノード.strSkinPath );
							Trace.TraceInformation( sb.ToString() );
						}
						finally
						{
							Trace.Unindent();
						}
					}
					if( b子BOXへ再帰する )
					{
						this.t曲を検索してリストを作成する( infoDir.FullName + @"\", b子BOXへ再帰する, c曲リストノード.list子リスト, c曲リストノード );
					}
				}
				//-----------------------------
				#endregion

				#region [ b.box.def を含むフォルダの場合  ]
				//-----------------------------
				else if( File.Exists( infoDir.FullName + @"\box.def" ) )
				{
					CBoxDef boxdef = new CBoxDef( infoDir.FullName + @"\box.def" );
					C曲リストノード c曲リストノード = new C曲リストノード();
					c曲リストノード.eノード種別 = C曲リストノード.Eノード種別.BOX;
					c曲リストノード.bDTXFilesで始まるフォルダ名のBOXである = false;
					c曲リストノード.strタイトル = boxdef.Title;
					c曲リストノード.strジャンル = boxdef.Genre;
					c曲リストノード.col文字色 = boxdef.Color;
					c曲リストノード.nスコア数 = 1;
					c曲リストノード.arスコア[ 0 ] = new Cスコア();
					c曲リストノード.arスコア[ 0 ].ファイル情報.フォルダの絶対パス = infoDir.FullName + @"\";
					c曲リストノード.arスコア[ 0 ].譜面情報.タイトル = boxdef.Title;
					c曲リストノード.arスコア[ 0 ].譜面情報.ジャンル = boxdef.Genre;
					c曲リストノード.arスコア[ 0 ].譜面情報.アーティスト名 = boxdef.Artist;
					c曲リストノード.arスコア[ 0 ].譜面情報.コメント = boxdef.Comment;
					c曲リストノード.arスコア[ 0 ].譜面情報.Preimage = boxdef.Preimage;
					c曲リストノード.arスコア[ 0 ].譜面情報.Premovie = boxdef.Premovie;
					c曲リストノード.arスコア[ 0 ].譜面情報.Presound = boxdef.Presound;
					c曲リストノード.r親ノード = node親;

					if ( boxdef.SkinPath == "" )
					{
						// box.defにスキン情報が記載されていないなら、上位BOXのスキン情報をコピー
						c曲リストノード.strSkinPath = ( c曲リストノード.r親ノード == null ) ?
							"" : c曲リストノード.r親ノード.strSkinPath;
					}
					else
					{
						// box.defに記載されているスキン情報をコピー。末尾に必ず\をつけておくこと。
						string s = System.IO.Path.Combine( infoDir.FullName, boxdef.SkinPath );
						if ( s[ s.Length - 1 ] != System.IO.Path.DirectorySeparatorChar )	// フォルダ名末尾に\を必ずつけて、CSkin側と表記を統一する
						{
							s += System.IO.Path.DirectorySeparatorChar;
						}
						if ( CDTXMania.Skin.bIsValid( s ) )
						{
							c曲リストノード.strSkinPath = s;
						}
						else
						{
							c曲リストノード.strSkinPath = ( c曲リストノード.r親ノード == null ) ?
								"" : c曲リストノード.r親ノード.strSkinPath;
						}
					}
					c曲リストノード.strBreadcrumbs = ( c曲リストノード.r親ノード == null ) ?
						c曲リストノード.strタイトル : c曲リストノード.r親ノード.strBreadcrumbs + " > " + c曲リストノード.strタイトル;
	
					
					c曲リストノード.list子リスト = new List<C曲リストノード>();
					c曲リストノード.nPerfect範囲ms = boxdef.PerfectRange;
					c曲リストノード.nGreat範囲ms = boxdef.GreatRange;
					c曲リストノード.nGood範囲ms = boxdef.GoodRange;
					c曲リストノード.nPoor範囲ms = boxdef.PoorRange;
					listノードリスト.Add( c曲リストノード );
					if( CDTXMania.ConfigIni.bLog曲検索ログ出力 )
					{
						Trace.TraceInformation( "box.def検出 : {0}", infoDir.FullName + @"\box.def" );
						Trace.Indent();
						try
						{
							StringBuilder sb = new StringBuilder( 0x400 );
							sb.Append( string.Format( "nID#{0:D3}", c曲リストノード.nID ) );
							if( c曲リストノード.r親ノード != null )
							{
								sb.Append( string.Format( "(in#{0:D3}):", c曲リストノード.r親ノード.nID ) );
							}
							else
							{
								sb.Append( "(onRoot):" );
							}
							sb.Append( "BOX, Title=" + c曲リストノード.strタイトル );
							if( ( c曲リストノード.strジャンル != null ) && ( c曲リストノード.strジャンル.Length > 0 ) )
							{
								sb.Append( ", Genre=" + c曲リストノード.strジャンル );
							}
							if( ( c曲リストノード.arスコア[ 0 ].譜面情報.アーティスト名 != null ) && ( c曲リストノード.arスコア[ 0 ].譜面情報.アーティスト名.Length > 0 ) )
							{
								sb.Append( ", Artist=" + c曲リストノード.arスコア[ 0 ].譜面情報.アーティスト名 );
							}
							if( ( c曲リストノード.arスコア[ 0 ].譜面情報.コメント != null ) && ( c曲リストノード.arスコア[ 0 ].譜面情報.コメント.Length > 0 ) )
							{
								sb.Append( ", Comment=" + c曲リストノード.arスコア[ 0 ].譜面情報.コメント );
							}
							if( ( c曲リストノード.arスコア[ 0 ].譜面情報.Preimage != null ) && ( c曲リストノード.arスコア[ 0 ].譜面情報.Preimage.Length > 0 ) )
							{
								sb.Append( ", Preimage=" + c曲リストノード.arスコア[ 0 ].譜面情報.Preimage );
							}
							if( ( c曲リストノード.arスコア[ 0 ].譜面情報.Premovie != null ) && ( c曲リストノード.arスコア[ 0 ].譜面情報.Premovie.Length > 0 ) )
							{
								sb.Append( ", Premovie=" + c曲リストノード.arスコア[ 0 ].譜面情報.Premovie );
							}
							if( ( c曲リストノード.arスコア[ 0 ].譜面情報.Presound != null ) && ( c曲リストノード.arスコア[ 0 ].譜面情報.Presound.Length > 0 ) )
							{
								sb.Append( ", Presound=" + c曲リストノード.arスコア[ 0 ].譜面情報.Presound );
							}
							if( c曲リストノード.col文字色 != ColorTranslator.FromHtml( "White" ) )
							{
								sb.Append( ", FontColor=" + c曲リストノード.col文字色 );
							}
							if( c曲リストノード.nPerfect範囲ms != -1 )
							{
								sb.Append( ", Perfect=" + c曲リストノード.nPerfect範囲ms + "ms" );
							}
							if( c曲リストノード.nGreat範囲ms != -1 )
							{
								sb.Append( ", Great=" + c曲リストノード.nGreat範囲ms + "ms" );
							}
							if( c曲リストノード.nGood範囲ms != -1 )
							{
								sb.Append( ", Good=" + c曲リストノード.nGood範囲ms + "ms" );
							}
							if( c曲リストノード.nPoor範囲ms != -1 )
							{
								sb.Append( ", Poor=" + c曲リストノード.nPoor範囲ms + "ms" );
							}
							if ( ( c曲リストノード.strSkinPath != null ) && ( c曲リストノード.strSkinPath.Length > 0 ) )
							{
								sb.Append( ", SkinPath=" + c曲リストノード.strSkinPath );
							}
							Trace.TraceInformation( sb.ToString() );
						}
						finally
						{
							Trace.Unindent();
						}
					}
					if( b子BOXへ再帰する )
					{
						this.t曲を検索してリストを作成する( infoDir.FullName + @"\", b子BOXへ再帰する, c曲リストノード.list子リスト, c曲リストノード );
					}
				}
				//-----------------------------
				#endregion

				#region [ c.通常フォルダの場合 ]
				//-----------------------------
				else
				{
					this.t曲を検索してリストを作成する( infoDir.FullName + @"\", b子BOXへ再帰する, listノードリスト, node親 );
				}
				//-----------------------------
				#endregion
			}
		}
		//-----------------
		#endregion
		#region [ スコアキャッシュを曲リストに反映する ]
		//-----------------
		public void tスコアキャッシュを曲リストに反映する()
		{
			this.nスコアキャッシュから反映できたスコア数 = 0;
			this.tスコアキャッシュを曲リストに反映する( this.list曲ルート );
		}
		private void tスコアキャッシュを曲リストに反映する( List<C曲リストノード> ノードリスト )
		{
			using( List<C曲リストノード>.Enumerator enumerator = ノードリスト.GetEnumerator() )
			{
				while( enumerator.MoveNext() )
				{
					SlowOrSuspendSearchTask();		// #27060 中断要求があったら、解除要求が来るまで待機, #PREMOVIE再生中は検索負荷を落とす

					C曲リストノード node = enumerator.Current;
					if( node.eノード種別 == C曲リストノード.Eノード種別.BOX )
					{
						this.tスコアキャッシュを曲リストに反映する( node.list子リスト );
					}
					else if( ( node.eノード種別 == C曲リストノード.Eノード種別.SCORE ) || ( node.eノード種別 == C曲リストノード.Eノード種別.SCORE_MIDI ) )
					{
						Predicate<Cスコア> match = null;
						for( int lv = 0; lv < 5; lv++ )
						{
							if( node.arスコア[ lv ] != null )
							{
								if( match == null )
								{
									match = delegate( Cスコア sc )
									{
										return
											(
											( sc.ファイル情報.ファイルの絶対パス.Equals( node.arスコア[ lv ].ファイル情報.ファイルの絶対パス )
											&& sc.ファイル情報.ファイルサイズ.Equals( node.arスコア[ lv ].ファイル情報.ファイルサイズ ) )
											&& ( sc.ファイル情報.最終更新日時.Equals( node.arスコア[ lv ].ファイル情報.最終更新日時 )
											&& sc.ScoreIni情報.ファイルサイズ.Equals( node.arスコア[ lv ].ScoreIni情報.ファイルサイズ ) ) )
											&& sc.ScoreIni情報.最終更新日時.Equals( node.arスコア[ lv ].ScoreIni情報.最終更新日時 );
									};
								}
								int nMatched = this.listSongsDB.FindIndex( match );
								if( nMatched == -1 )
								{
//Trace.TraceInformation( "songs.db に存在しません。({0})", node.arスコア[ lv ].ファイル情報.ファイルの絶対パス );
									if ( CDTXMania.ConfigIni.bLog曲検索ログ出力 )
									{
										Trace.TraceInformation( "songs.db に存在しません。({0})", node.arスコア[ lv ].ファイル情報.ファイルの絶対パス );
									}
								}
								else
								{
									node.arスコア[ lv ].譜面情報 = this.listSongsDB[ nMatched ].譜面情報;
									node.arスコア[ lv ].bSongDBにキャッシュがあった = true;
									if( CDTXMania.ConfigIni.bLog曲検索ログ出力 )
									{
										Trace.TraceInformation( "songs.db から転記しました。({0})", node.arスコア[ lv ].ファイル情報.ファイルの絶対パス );
									}
									this.nスコアキャッシュから反映できたスコア数++;
									if( node.arスコア[ lv ].ScoreIni情報.最終更新日時 != this.listSongsDB[ nMatched ].ScoreIni情報.最終更新日時 )
									{
										string strFileNameScoreIni = node.arスコア[ lv ].ファイル情報.ファイルの絶対パス + ".score.ini";
										try
										{
											CScoreIni scoreIni = new CScoreIni( strFileNameScoreIni );
											scoreIni.t全演奏記録セクションの整合性をチェックし不整合があればリセットする();
											for( int i = 0; i < 3; i++ )
											{
												int nSectionHiSkill = ( i * 2 ) + 1;
                                                int nSectionHiScore = i * 2;
												if(    scoreIni.stセクション[ nSectionHiSkill ].b演奏にMIDI入力を使用した
													|| scoreIni.stセクション[ nSectionHiSkill ].b演奏にキーボードを使用した
													|| scoreIni.stセクション[ nSectionHiSkill ].b演奏にジョイパッドを使用した
													|| scoreIni.stセクション[ nSectionHiSkill ].b演奏にマウスを使用した )
												{
                                                    if (CDTXMania.ConfigIni.nSkillMode == 0)
                                                    {
                                                        node.arスコア[lv].譜面情報.最大ランク[i] =
                                                            (scoreIni.stファイル.BestRank[i] != (int)CScoreIni.ERANK.UNKNOWN) ?
                                                            (int)scoreIni.stファイル.BestRank[i] : CScoreIni.t旧ランク値を計算して返す(scoreIni.stセクション[nSectionHiSkill]);
                                                    }
                                                    else
                                                    {
                                                        node.arスコア[lv].譜面情報.最大ランク[i] =
                                                            (scoreIni.stファイル.BestRank[i] != (int)CScoreIni.ERANK.UNKNOWN) ?
                                                            (int)scoreIni.stファイル.BestRank[i] : CScoreIni.tランク値を計算して返す(scoreIni.stセクション[nSectionHiSkill]);
                                                    }
												}
												else
												{
													node.arスコア[ lv ].譜面情報.最大ランク[ i ] = (int)CScoreIni.ERANK.UNKNOWN;
												}
												node.arスコア[ lv ].譜面情報.最大スキル[ i ] = scoreIni.stセクション[ nSectionHiSkill ].db演奏型スキル値;
												node.arスコア[ lv ].譜面情報.フルコンボ[ i ] = scoreIni.stセクション[ nSectionHiSkill ].bフルコンボである | scoreIni.stセクション[ nSectionHiScore ].bフルコンボである;
											}
											node.arスコア[ lv ].譜面情報.演奏回数.Drums = scoreIni.stファイル.PlayCountDrums;
											node.arスコア[ lv ].譜面情報.演奏回数.Guitar = scoreIni.stファイル.PlayCountGuitar;
											node.arスコア[ lv ].譜面情報.演奏回数.Bass = scoreIni.stファイル.PlayCountBass;
											for( int j = 0; j < 5; j++ )
											{
												node.arスコア[ lv ].譜面情報.演奏履歴[ j ] = scoreIni.stファイル.History[ j ];
											}
											if( CDTXMania.ConfigIni.bLog曲検索ログ出力 )
											{
												Trace.TraceInformation( "演奏記録ファイルから HiSkill 情報と演奏履歴を取得しました。({0})", strFileNameScoreIni );
											}
										}
										catch
										{
											Trace.TraceError( "演奏記録ファイルの読み込みに失敗しました。({0})", strFileNameScoreIni );
										}
									}
								}
							}
						}
					}
				}
			}
		}
		private Cスコア tSongsDBからスコアを１つ読み込む( BinaryReader br )
		{
			Cスコア cスコア = new Cスコア();
			cスコア.ファイル情報.ファイルの絶対パス = br.ReadString();
			cスコア.ファイル情報.フォルダの絶対パス = br.ReadString();
			cスコア.ファイル情報.最終更新日時 = new DateTime( br.ReadInt64() );
			cスコア.ファイル情報.ファイルサイズ = br.ReadInt64();
			cスコア.ScoreIni情報.最終更新日時 = new DateTime( br.ReadInt64() );
			cスコア.ScoreIni情報.ファイルサイズ = br.ReadInt64();
			cスコア.譜面情報.タイトル = br.ReadString();
			cスコア.譜面情報.アーティスト名 = br.ReadString();
			cスコア.譜面情報.コメント = br.ReadString();
			cスコア.譜面情報.ジャンル = br.ReadString();
			cスコア.譜面情報.Preimage = br.ReadString();
			cスコア.譜面情報.Premovie = br.ReadString();
			cスコア.譜面情報.Presound = br.ReadString();
			cスコア.譜面情報.Backgound = br.ReadString();
			cスコア.譜面情報.レベル.Drums = br.ReadInt32();
			cスコア.譜面情報.レベル.Guitar = br.ReadInt32();
			cスコア.譜面情報.レベル.Bass = br.ReadInt32();
            cスコア.譜面情報.レベルDec.Drums = br.ReadInt32();
            cスコア.譜面情報.レベルDec.Guitar = br.ReadInt32();
            cスコア.譜面情報.レベルDec.Bass = br.ReadInt32();
			cスコア.譜面情報.最大ランク.Drums = br.ReadInt32();
			cスコア.譜面情報.最大ランク.Guitar = br.ReadInt32();
			cスコア.譜面情報.最大ランク.Bass = br.ReadInt32();
			cスコア.譜面情報.最大スキル.Drums = br.ReadDouble();
			cスコア.譜面情報.最大スキル.Guitar = br.ReadDouble();
			cスコア.譜面情報.最大スキル.Bass = br.ReadDouble();
			cスコア.譜面情報.フルコンボ.Drums = br.ReadBoolean();
			cスコア.譜面情報.フルコンボ.Guitar = br.ReadBoolean();
			cスコア.譜面情報.フルコンボ.Bass = br.ReadBoolean();
			cスコア.譜面情報.演奏回数.Drums = br.ReadInt32();
			cスコア.譜面情報.演奏回数.Guitar = br.ReadInt32();
			cスコア.譜面情報.演奏回数.Bass = br.ReadInt32();
			cスコア.譜面情報.演奏履歴.行1 = br.ReadString();
			cスコア.譜面情報.演奏履歴.行2 = br.ReadString();
			cスコア.譜面情報.演奏履歴.行3 = br.ReadString();
			cスコア.譜面情報.演奏履歴.行4 = br.ReadString();
			cスコア.譜面情報.演奏履歴.行5 = br.ReadString();
			cスコア.譜面情報.レベルを非表示にする = br.ReadBoolean();
            cスコア.譜面情報.b完全にCLASSIC譜面である.Drums = br.ReadBoolean();
            cスコア.譜面情報.b完全にCLASSIC譜面である.Guitar = br.ReadBoolean();
            cスコア.譜面情報.b完全にCLASSIC譜面である.Bass = br.ReadBoolean();
            cスコア.譜面情報.b譜面がある.Drums = br.ReadBoolean();
            cスコア.譜面情報.b譜面がある.Guitar = br.ReadBoolean();
            cスコア.譜面情報.b譜面がある.Bass = br.ReadBoolean();
			cスコア.譜面情報.曲種別 = (CDTX.E種別) br.ReadInt32();
			cスコア.譜面情報.Bpm = br.ReadDouble();
			cスコア.譜面情報.Duration = br.ReadInt32();


//Debug.WriteLine( "songs.db: " + cスコア.ファイル情報.ファイルの絶対パス );
			return cスコア;
		}
		//-----------------
		#endregion
		#region [ SongsDBになかった曲をファイルから読み込んで反映する ]
		//-----------------
		public void tSongsDBになかった曲をファイルから読み込んで反映する()
		{
			this.nファイルから反映できたスコア数 = 0;
			this.tSongsDBになかった曲をファイルから読み込んで反映する( this.list曲ルート );
		}
		private void tSongsDBになかった曲をファイルから読み込んで反映する( List<C曲リストノード> ノードリスト )
		{
			foreach( C曲リストノード c曲リストノード in ノードリスト )
			{
				SlowOrSuspendSearchTask();		// #27060 中断要求があったら、解除要求が来るまで待機, #PREMOVIE再生中は検索負荷を落とす

				if( c曲リストノード.eノード種別 == C曲リストノード.Eノード種別.BOX )
				{
					this.tSongsDBになかった曲をファイルから読み込んで反映する( c曲リストノード.list子リスト );
				}
				else if( ( c曲リストノード.eノード種別 == C曲リストノード.Eノード種別.SCORE )
					  || ( c曲リストノード.eノード種別 == C曲リストノード.Eノード種別.SCORE_MIDI ) )
				{
					for( int i = 0; i < 5; i++ )
					{
						if( ( c曲リストノード.arスコア[ i ] != null ) && !c曲リストノード.arスコア[ i ].bSongDBにキャッシュがあった )
						{
							#region [ DTX ファイルのヘッダだけ読み込み、Cスコア.譜面情報 を設定する ]
							//-----------------
							string path = c曲リストノード.arスコア[ i ].ファイル情報.ファイルの絶対パス;
							if( File.Exists( path ) )
							{
								try
								{
									CDTX cdtx = new CDTX( c曲リストノード.arスコア[ i ].ファイル情報.ファイルの絶対パス, false );    //2013.06.04 kairera0467 ここの「ヘッダのみ読み込む」をfalseにすると、選曲画面のBPM表示が狂う場合があるので注意。
                                    //CDTX cdtx2 = new CDTX( c曲リストノード.arスコア[ i ].ファイル情報.ファイルの絶対パス, false );
									c曲リストノード.arスコア[ i ].譜面情報.タイトル = cdtx.TITLE;
									c曲リストノード.arスコア[ i ].譜面情報.アーティスト名 = cdtx.ARTIST;
									c曲リストノード.arスコア[ i ].譜面情報.コメント = cdtx.COMMENT;
									c曲リストノード.arスコア[ i ].譜面情報.ジャンル = cdtx.GENRE;
									c曲リストノード.arスコア[ i ].譜面情報.Preimage = cdtx.PREIMAGE;
									c曲リストノード.arスコア[ i ].譜面情報.Premovie = cdtx.PREMOVIE;
									c曲リストノード.arスコア[ i ].譜面情報.Presound = cdtx.PREVIEW;
									c曲リストノード.arスコア[ i ].譜面情報.Backgound = ( ( cdtx.BACKGROUND != null ) && ( cdtx.BACKGROUND.Length > 0 ) ) ? cdtx.BACKGROUND : cdtx.BACKGROUND_GR;
									c曲リストノード.arスコア[ i ].譜面情報.レベル.Drums = cdtx.LEVEL.Drums;
									c曲リストノード.arスコア[ i ].譜面情報.レベル.Guitar = cdtx.LEVEL.Guitar;
									c曲リストノード.arスコア[ i ].譜面情報.レベル.Bass = cdtx.LEVEL.Bass;
                                    c曲リストノード.arスコア[ i ].譜面情報.レベルDec.Drums = cdtx.LEVELDEC.Drums;
                                    c曲リストノード.arスコア[ i ].譜面情報.レベルDec.Guitar = cdtx.LEVELDEC.Guitar;
                                    c曲リストノード.arスコア[ i ].譜面情報.レベルDec.Bass = cdtx.LEVELDEC.Bass;
									c曲リストノード.arスコア[ i ].譜面情報.レベルを非表示にする = cdtx.HIDDENLEVEL;
                                    c曲リストノード.arスコア[ i ].譜面情報.b完全にCLASSIC譜面である.Drums = (cdtx.bチップがある.LeftCymbal == false && cdtx.bチップがある.LP == false && cdtx.bチップがある.LBD == false && cdtx.bチップがある.FT == false && cdtx.bチップがある.Ride == false) ? true : false;
                                    c曲リストノード.arスコア[ i ].譜面情報.b完全にCLASSIC譜面である.Guitar = !cdtx.bチップがある.YPGuitar ? true : false;
                                    c曲リストノード.arスコア[ i ].譜面情報.b完全にCLASSIC譜面である.Bass = !cdtx.bチップがある.YPBass ? true : false;
                                    c曲リストノード.arスコア[ i ].譜面情報.b譜面がある.Drums = cdtx.bチップがある.Drums;
                                    c曲リストノード.arスコア[ i ].譜面情報.b譜面がある.Guitar = cdtx.bチップがある.Guitar;
                                    c曲リストノード.arスコア[ i ].譜面情報.b譜面がある.Bass = cdtx.bチップがある.Bass;
									c曲リストノード.arスコア[ i ].譜面情報.曲種別 = cdtx.e種別;
									c曲リストノード.arスコア[ i ].譜面情報.Bpm = cdtx.BPM;
									c曲リストノード.arスコア[ i ].譜面情報.Duration = 0;	//  (cdtx.listChip == null)? 0 : cdtx.listChip[ cdtx.listChip.Count - 1 ].n発声時刻ms;
									this.nファイルから反映できたスコア数++;
									cdtx.On非活性化();
//Debug.WriteLine( "★" + this.nファイルから反映できたスコア数 + " " + c曲リストノード.arスコア[ i ].譜面情報.タイトル );
									#region [ 曲検索ログ出力 ]
									//-----------------
									if( CDTXMania.ConfigIni.bLog曲検索ログ出力 )
									{
										StringBuilder sb = new StringBuilder( 0x400 );
										sb.Append( string.Format( "曲データファイルから譜面情報を転記しました。({0})", path ) );
										sb.Append( "(title=" + c曲リストノード.arスコア[ i ].譜面情報.タイトル );
										sb.Append( ", artist=" + c曲リストノード.arスコア[ i ].譜面情報.アーティスト名 );
										sb.Append( ", comment=" + c曲リストノード.arスコア[ i ].譜面情報.コメント );
										sb.Append( ", genre=" + c曲リストノード.arスコア[ i ].譜面情報.ジャンル );
										sb.Append( ", preimage=" + c曲リストノード.arスコア[ i ].譜面情報.Preimage );
										sb.Append( ", premovie=" + c曲リストノード.arスコア[ i ].譜面情報.Premovie );
										sb.Append( ", presound=" + c曲リストノード.arスコア[ i ].譜面情報.Presound );
										sb.Append( ", background=" + c曲リストノード.arスコア[ i ].譜面情報.Backgound );
										sb.Append( ", lvDr=" + c曲リストノード.arスコア[ i ].譜面情報.レベル.Drums );
										sb.Append( ", lvGt=" + c曲リストノード.arスコア[ i ].譜面情報.レベル.Guitar );
										sb.Append( ", lvBs=" + c曲リストノード.arスコア[ i ].譜面情報.レベル.Bass );
										sb.Append( ", lvHide=" + c曲リストノード.arスコア[ i ].譜面情報.レベルを非表示にする );
                                        sb.Append( ", classic=" + c曲リストノード.arスコア[ i ].譜面情報.b完全にCLASSIC譜面である );
										sb.Append( ", type=" + c曲リストノード.arスコア[ i ].譜面情報.曲種別 );
										sb.Append( ", bpm=" + c曲リストノード.arスコア[ i ].譜面情報.Bpm );
									//	sb.Append( ", duration=" + c曲リストノード.arスコア[ i ].譜面情報.Duration );
										Trace.TraceInformation( sb.ToString() );
									}
									//-----------------
									#endregion
								}
								catch( Exception exception )
								{
									Trace.TraceError( exception.Message );
									c曲リストノード.arスコア[ i ] = null;
									c曲リストノード.nスコア数--;
									this.n検索されたスコア数--;
									Trace.TraceError( "曲データファイルの読み込みに失敗しました。({0})", path );
								}
							}
							//-----------------
							#endregion

							#region [ 対応する .score.ini が存在していれば読み込み、Cスコア.譜面情報 に追加設定する ]
							//-----------------
							this.tScoreIniを読み込んで譜面情報を設定する( c曲リストノード.arスコア[ i ].ファイル情報.ファイルの絶対パス + ".score.ini", ref c曲リストノード.arスコア[ i ] );
							//-----------------
							#endregion
						}
					}
				}
			}
		}
		//-----------------
		#endregion
		#region [ 曲リストへ後処理を適用する ]
		//-----------------
		public void t曲リストへ後処理を適用する()
		{
			listStrBoxDefSkinSubfolderFullName = new List<string>();
			if ( CDTXMania.Skin.strBoxDefSkinSubfolders != null )
			{
				foreach ( string b in CDTXMania.Skin.strBoxDefSkinSubfolders )
				{
					listStrBoxDefSkinSubfolderFullName.Add( b );
				}
			}

			this.t曲リストへ後処理を適用する( this.list曲ルート );

			#region [ skin名で比較して、systemスキンとboxdefスキンに重複があれば、boxdefスキン側を削除する ]
			string[] systemSkinNames = CSkin.GetSkinName( CDTXMania.Skin.strSystemSkinSubfolders );
			List<string> l = new List<string>( listStrBoxDefSkinSubfolderFullName );
			foreach ( string boxdefSkinSubfolderFullName in l )
			{
				if ( Array.BinarySearch( systemSkinNames,
					CSkin.GetSkinName( boxdefSkinSubfolderFullName ),
					StringComparer.InvariantCultureIgnoreCase ) >= 0 )
				{
					listStrBoxDefSkinSubfolderFullName.Remove( boxdefSkinSubfolderFullName );
				}
			}
			#endregion
			string[] ba = listStrBoxDefSkinSubfolderFullName.ToArray();
			Array.Sort( ba );
			CDTXMania.Skin.strBoxDefSkinSubfolders = ba;
		}
		private void t曲リストへ後処理を適用する( List<C曲リストノード> ノードリスト )
		{
			#region [ リストに１つ以上の曲があるなら RANDOM BOX を入れる ]
			//-----------------------------
			if( ノードリスト.Count > 0 )
			{
				C曲リストノード itemRandom = new C曲リストノード();
				itemRandom.eノード種別 = C曲リストノード.Eノード種別.RANDOM;
				itemRandom.strタイトル = "< RANDOM SELECT >";
				itemRandom.nスコア数 = 5;
				itemRandom.r親ノード = ノードリスト[ 0 ].r親ノード;

				itemRandom.strBreadcrumbs = ( itemRandom.r親ノード == null ) ?
					itemRandom.strタイトル :  itemRandom.r親ノード.strBreadcrumbs + " > " + itemRandom.strタイトル;

				for( int i = 0; i < 5; i++ )
				{
					itemRandom.arスコア[ i ] = new Cスコア();
					itemRandom.arスコア[ i ].譜面情報.タイトル = string.Format( "< RANDOM SELECT Lv.{0} >", i + 1 );
                    itemRandom.arスコア[ i ].譜面情報.Preimage = CSkin.Path(@"Graphics\5_preimage random.png");
                    itemRandom.arスコア[ i ].譜面情報.コメント =
						 (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ja") ?
						 string.Format("難易度レベル {0} 付近の曲をランダムに選択します。難易度レベルを持たない曲も選択候補となります。", i + 1) :
						 string.Format("Random select from the songs which has the level about L{0}. Non-leveled songs may also selected.", i + 1);
					itemRandom.ar難易度ラベル[ i ] = string.Format( "L{0}", i + 1 );
				}
				ノードリスト.Add( itemRandom );

				#region [ ログ出力 ]
				//-----------------------------
				if( CDTXMania.ConfigIni.bLog曲検索ログ出力 )
				{
					StringBuilder sb = new StringBuilder( 0x100 );
					sb.Append( string.Format( "nID#{0:D3}", itemRandom.nID ) );
					if( itemRandom.r親ノード != null )
					{
						sb.Append( string.Format( "(in#{0:D3}):", itemRandom.r親ノード.nID ) );
					}
					else
					{
						sb.Append( "(onRoot):" );
					}
					sb.Append( " RANDOM" );
					Trace.TraceInformation( sb.ToString() );
				}
				//-----------------------------
				#endregion
			}
			//-----------------------------
			#endregion

			// すべてのノードについて…
			foreach( C曲リストノード c曲リストノード in ノードリスト )
			{
				SlowOrSuspendSearchTask();		// #27060 中断要求があったら、解除要求が来るまで待機, #PREMOVIE再生中は検索負荷を落とす

				#region [ BOXノードなら子リストに <<BACK を入れ、子リストに後処理を適用する ]
				//-----------------------------
				if( c曲リストノード.eノード種別 == C曲リストノード.Eノード種別.BOX )
				{
					C曲リストノード itemBack = new C曲リストノード();
					itemBack.eノード種別 = C曲リストノード.Eノード種別.BACKBOX;
					itemBack.strタイトル = "<< BACK";
					itemBack.nスコア数 = 1;
					itemBack.r親ノード = c曲リストノード;

					itemBack.strSkinPath = ( c曲リストノード.r親ノード == null ) ?
						"" : c曲リストノード.r親ノード.strSkinPath;

					if ( itemBack.strSkinPath != "" && !listStrBoxDefSkinSubfolderFullName.Contains( itemBack.strSkinPath ) )
					{
						listStrBoxDefSkinSubfolderFullName.Add( itemBack.strSkinPath );
					}

					itemBack.strBreadcrumbs = ( itemBack.r親ノード == null ) ?
						itemBack.strタイトル : itemBack.r親ノード.strBreadcrumbs + " > " + itemBack.strタイトル;

					itemBack.arスコア[ 0 ] = new Cスコア();
					itemBack.arスコア[ 0 ].ファイル情報.フォルダの絶対パス = "";
					itemBack.arスコア[ 0 ].譜面情報.タイトル = itemBack.strタイトル;
                    itemBack.arスコア[ 0 ].譜面情報.Preimage = CSkin.Path(@"Graphics\5_preimage backbox.png");
					itemBack.arスコア[ 0 ].譜面情報.コメント =
						(CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ja") ?
						"BOX を出ます。" :
						"Exit from the BOX.";
					c曲リストノード.list子リスト.Insert( 0, itemBack );

					#region [ ログ出力 ]
					//-----------------------------
					if( CDTXMania.ConfigIni.bLog曲検索ログ出力 )
					{
						StringBuilder sb = new StringBuilder( 0x100 );
						sb.Append( string.Format( "nID#{0:D3}", itemBack.nID ) );
						if( itemBack.r親ノード != null )
						{
							sb.Append( string.Format( "(in#{0:D3}):", itemBack.r親ノード.nID ) );
						}
						else
						{
							sb.Append( "(onRoot):" );
						}
						sb.Append( " BACKBOX" );
						Trace.TraceInformation( sb.ToString() );
					}
					//-----------------------------
					#endregion

					this.t曲リストへ後処理を適用する( c曲リストノード.list子リスト );
					continue;
				}
				//-----------------------------
				#endregion

				#region [ ノードにタイトルがないなら、最初に見つけたスコアのタイトルを設定する ]
				//-----------------------------
				if( string.IsNullOrEmpty( c曲リストノード.strタイトル ) )
				{
					for( int j = 0; j < 5; j++ )
					{
						if( ( c曲リストノード.arスコア[ j ] != null ) && !string.IsNullOrEmpty( c曲リストノード.arスコア[ j ].譜面情報.タイトル ) )
						{
							c曲リストノード.strタイトル = c曲リストノード.arスコア[ j ].譜面情報.タイトル;

							if( CDTXMania.ConfigIni.bLog曲検索ログ出力 )
								Trace.TraceInformation( "タイトルを設定しました。(nID#{0:D3}, title={1})", c曲リストノード.nID, c曲リストノード.strタイトル );

							break;
						}
					}
				}
				//-----------------------------
				#endregion
			}

			#region [ ノードをソートする ]
			//-----------------------------
			this.t曲リストのソート1_絶対パス順( ノードリスト );
			//-----------------------------
			#endregion
		}
		//-----------------
		#endregion
		#region [ スコアキャッシュをSongsDBに出力する ]
		//-----------------
		public void tスコアキャッシュをSongsDBに出力する( string SongsDBファイル名 )
		{
			this.nSongsDBへ出力できたスコア数 = 0;
			try
			{
				BinaryWriter bw = new BinaryWriter( new FileStream( SongsDBファイル名, FileMode.Create, FileAccess.Write ) );
				bw.Write( SONGSDB_VERSION );
				this.tSongsDBにリストを１つ出力する( bw, this.list曲ルート );
				bw.Close();
			}
			catch
			{
				Trace.TraceError( "songs.dbの出力に失敗しました。" );
			}
		}
		private void tSongsDBにノードを１つ出力する( BinaryWriter bw, C曲リストノード node )
		{
			for( int i = 0; i < 5; i++ )
			{
				// ここではsuspendに応じないようにしておく(深い意味はない。ファイルの書き込みオープン状態を長時間維持したくないだけ)
				//if ( this.bIsSuspending )		// #27060 中断要求があったら、解除要求が来るまで待機
				//{
				//	autoReset.WaitOne();
				//}

				if( node.arスコア[ i ] != null )
				{
					bw.Write( node.arスコア[ i ].ファイル情報.ファイルの絶対パス );
					bw.Write( node.arスコア[ i ].ファイル情報.フォルダの絶対パス );
					bw.Write( node.arスコア[ i ].ファイル情報.最終更新日時.Ticks );
					bw.Write( node.arスコア[ i ].ファイル情報.ファイルサイズ );
					bw.Write( node.arスコア[ i ].ScoreIni情報.最終更新日時.Ticks );
					bw.Write( node.arスコア[ i ].ScoreIni情報.ファイルサイズ );
					bw.Write( node.arスコア[ i ].譜面情報.タイトル );
					bw.Write( node.arスコア[ i ].譜面情報.アーティスト名 );
					bw.Write( node.arスコア[ i ].譜面情報.コメント );
					bw.Write( node.arスコア[ i ].譜面情報.ジャンル );
					bw.Write( node.arスコア[ i ].譜面情報.Preimage );
					bw.Write( node.arスコア[ i ].譜面情報.Premovie );
					bw.Write( node.arスコア[ i ].譜面情報.Presound );
					bw.Write( node.arスコア[ i ].譜面情報.Backgound );
					bw.Write( node.arスコア[ i ].譜面情報.レベル.Drums );
					bw.Write( node.arスコア[ i ].譜面情報.レベル.Guitar );
					bw.Write( node.arスコア[ i ].譜面情報.レベル.Bass );
                    bw.Write( node.arスコア[ i ].譜面情報.レベルDec.Drums );
                    bw.Write( node.arスコア[ i ].譜面情報.レベルDec.Guitar );
                    bw.Write( node.arスコア[ i ].譜面情報.レベルDec.Bass );
					bw.Write( node.arスコア[ i ].譜面情報.最大ランク.Drums );
					bw.Write( node.arスコア[ i ].譜面情報.最大ランク.Guitar );
					bw.Write( node.arスコア[ i ].譜面情報.最大ランク.Bass );
					bw.Write( node.arスコア[ i ].譜面情報.最大スキル.Drums );
					bw.Write( node.arスコア[ i ].譜面情報.最大スキル.Guitar );
					bw.Write( node.arスコア[ i ].譜面情報.最大スキル.Bass );
					bw.Write( node.arスコア[ i ].譜面情報.フルコンボ.Drums );
					bw.Write( node.arスコア[ i ].譜面情報.フルコンボ.Guitar );
					bw.Write( node.arスコア[ i ].譜面情報.フルコンボ.Bass );
					bw.Write( node.arスコア[ i ].譜面情報.演奏回数.Drums );
					bw.Write( node.arスコア[ i ].譜面情報.演奏回数.Guitar );
					bw.Write( node.arスコア[ i ].譜面情報.演奏回数.Bass );
					bw.Write( node.arスコア[ i ].譜面情報.演奏履歴.行1 );
					bw.Write( node.arスコア[ i ].譜面情報.演奏履歴.行2 );
					bw.Write( node.arスコア[ i ].譜面情報.演奏履歴.行3 );
					bw.Write( node.arスコア[ i ].譜面情報.演奏履歴.行4 );
					bw.Write( node.arスコア[ i ].譜面情報.演奏履歴.行5 );
					bw.Write( node.arスコア[ i ].譜面情報.レベルを非表示にする );
                    bw.Write( node.arスコア[ i ].譜面情報.b完全にCLASSIC譜面である.Drums );
                    bw.Write( node.arスコア[ i ].譜面情報.b完全にCLASSIC譜面である.Guitar );
                    bw.Write( node.arスコア[ i ].譜面情報.b完全にCLASSIC譜面である.Bass );
                    bw.Write( node.arスコア[ i ].譜面情報.b譜面がある.Drums );
                    bw.Write( node.arスコア[ i ].譜面情報.b譜面がある.Guitar );
                    bw.Write( node.arスコア[ i ].譜面情報.b譜面がある.Bass );
					bw.Write( (int) node.arスコア[ i ].譜面情報.曲種別 );
					bw.Write( node.arスコア[ i ].譜面情報.Bpm );
					bw.Write( node.arスコア[ i ].譜面情報.Duration );
					this.nSongsDBへ出力できたスコア数++;
				}
			}
		}
		private void tSongsDBにリストを１つ出力する( BinaryWriter bw, List<C曲リストノード> list )
		{
			foreach( C曲リストノード c曲リストノード in list )
			{
				if(    ( c曲リストノード.eノード種別 == C曲リストノード.Eノード種別.SCORE )
					|| ( c曲リストノード.eノード種別 == C曲リストノード.Eノード種別.SCORE_MIDI ) )
				{
					this.tSongsDBにノードを１つ出力する( bw, c曲リストノード );
				}
				if( c曲リストノード.list子リスト != null )
				{
					this.tSongsDBにリストを１つ出力する( bw, c曲リストノード.list子リスト );
				}
			}
		}
		//-----------------
		#endregion
		
		#region [ 曲リストソート ]
		//-----------------
		public void t曲リストのソート1_絶対パス順( List<C曲リストノード> ノードリスト )
		{
			ノードリスト.Sort( delegate( C曲リストノード n1, C曲リストノード n2 )
			{
				#region [ 共通処理 ]
				if ( n1 == n2 )
				{
					return 0;
				}
				int num = this.t比較0_共通( n1, n2 );
				if( num != 0 )
				{
					return num;
				}
				if( ( n1.eノード種別 == C曲リストノード.Eノード種別.BOX ) && ( n2.eノード種別 == C曲リストノード.Eノード種別.BOX ) )
				{
					return n1.arスコア[ 0 ].ファイル情報.フォルダの絶対パス.CompareTo( n2.arスコア[ 0 ].ファイル情報.フォルダの絶対パス );
				}
				#endregion
				string str = "";
				if( string.IsNullOrEmpty( n1.pathSetDefの絶対パス ) )
				{
					for( int i = 0; i < 5; i++ )
					{
						if( n1.arスコア[ i ] != null )
						{
							str = n1.arスコア[ i ].ファイル情報.ファイルの絶対パス;
							if( str == null )
							{
								str = "";
							}
							break;
						}
					}
				}
				else
				{
					str = n1.pathSetDefの絶対パス + n1.SetDefのブロック番号.ToString( "00" );
				}
				string strB = "";
				if( string.IsNullOrEmpty( n2.pathSetDefの絶対パス ) )
				{
					for( int j = 0; j < 5; j++ )
					{
						if( n2.arスコア[ j ] != null )
						{
							strB = n2.arスコア[ j ].ファイル情報.ファイルの絶対パス;
							if( strB == null )
							{
								strB = "";
							}
							break;
						}
					}
				}
				else
				{
					strB = n2.pathSetDefの絶対パス + n2.SetDefのブロック番号.ToString( "00" );
				}
				return str.CompareTo( strB );
			} );
			foreach( C曲リストノード c曲リストノード in ノードリスト )
			{
				if( ( c曲リストノード.list子リスト != null ) && ( c曲リストノード.list子リスト.Count > 1 ) )
				{
					this.t曲リストのソート1_絶対パス順( c曲リストノード.list子リスト );
				}
			}
		}
		public void t曲リストのソート2_タイトル順( List<C曲リストノード> ノードリスト, E楽器パート part, int order, params object[] p )
		{
			ノードリスト.Sort( delegate( C曲リストノード n1, C曲リストノード n2 )
			{
				if( n1 == n2 )
				{
					return 0;
				}
				int num = this.t比較0_共通( n1, n2 );
				if( num != 0 )
				{
					return order * num;
				}
				return order * n1.strタイトル.CompareTo( n2.strタイトル );
			} );
//			foreach( C曲リストノード c曲リストノード in ノードリスト )
//			{
//				if( ( c曲リストノード.list子リスト != null ) && ( c曲リストノード.list子リスト.Count > 1 ) )
//				{
//					this.t曲リストのソート2_タイトル順( c曲リストノード.list子リスト, part, order );
//				}
//			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ノードリスト"></param>
		/// <param name="part"></param>
		/// <param name="order">1=Ascend -1=Descend</param>
		public void t曲リストのソート3_演奏回数の多い順( List<C曲リストノード> ノードリスト, E楽器パート part, int order, params object[] p )
		{
			order = -order;
			int nL12345 = (int) p[ 0 ];
			if ( part != E楽器パート.UNKNOWN )
			{
				ノードリスト.Sort( delegate( C曲リストノード n1, C曲リストノード n2 )
				{
					#region [ 共通処理 ]
					if ( n1 == n2 )
					{
						return 0;
					}
					int num = this.t比較0_共通( n1, n2 );
					if( num != 0 )
					{
						return order * num;
					}
					if( ( n1.eノード種別 == C曲リストノード.Eノード種別.BOX ) && ( n2.eノード種別 == C曲リストノード.Eノード種別.BOX ) )
					{
						return order * n1.arスコア[ 0 ].ファイル情報.フォルダの絶対パス.CompareTo( n2.arスコア[ 0 ].ファイル情報.フォルダの絶対パス );
					}
					#endregion
					int nSumPlayCountN1 = 0, nSumPlayCountN2 = 0;
//					for( int i = 0; i < 5; i++ )
//					{
						if( n1.arスコア[ nL12345 ] != null )
						{
							nSumPlayCountN1 += n1.arスコア[ nL12345 ].譜面情報.演奏回数[ (int) part ];
						}
						if( n2.arスコア[ nL12345 ] != null )
						{
							nSumPlayCountN2 += n2.arスコア[ nL12345 ].譜面情報.演奏回数[ (int) part ];
						}
//					}
					num = nSumPlayCountN2 - nSumPlayCountN1;
					if( num != 0 )
					{
						return order * num;
					}
					return order * n1.strタイトル.CompareTo( n2.strタイトル );
				} );
				foreach ( C曲リストノード c曲リストノード in ノードリスト )
				{
					int nSumPlayCountN1 = 0;
//					for ( int i = 0; i < 5; i++ )
//					{
						if ( c曲リストノード.arスコア[ nL12345 ] != null )
						{
							nSumPlayCountN1 += c曲リストノード.arスコア[ nL12345 ].譜面情報.演奏回数[ (int) part ];
						}
//					}
// Debug.WriteLine( nSumPlayCountN1 + ":" + c曲リストノード.strタイトル );
				}

//				foreach( C曲リストノード c曲リストノード in ノードリスト )
//				{
//					if( ( c曲リストノード.list子リスト != null ) && ( c曲リストノード.list子リスト.Count > 1 ) )
//					{
//						this.t曲リストのソート3_演奏回数の多い順( c曲リストノード.list子リスト, part );
//					}
//				}
			}
		}
		public void t曲リストのソート4_LEVEL順( List<C曲リストノード> ノードリスト, E楽器パート part, int order, params object[] p )
		{
			order = -order;
			int nL12345 = (int)p[ 0 ];
			if ( part != E楽器パート.UNKNOWN )
			{
                Trace.WriteLine( "----------ソート開始------------" );
				ノードリスト.Sort( delegate( C曲リストノード n1, C曲リストノード n2 ) //2016.03.12 kairera0467 少数第2位も考慮するようにするテスト。
				{
					#region [ 共通処理 ]
					if ( n1 == n2 )
					{
						return 0;
					}
					float num = this.t比較0_共通( n1, n2 ); //2016.06.17 kairera0467 ソートが正確に行われるよう修正。(int→float)
					if ( num != 0 )
					{
						return (int)(order * num);
					}
					if ( ( n1.eノード種別 == C曲リストノード.Eノード種別.BOX ) && ( n2.eノード種別 == C曲リストノード.Eノード種別.BOX ) )
					{
						return order * n1.arスコア[ 0 ].ファイル情報.フォルダの絶対パス.CompareTo( n2.arスコア[ 0 ].ファイル情報.フォルダの絶対パス );
					}
					#endregion
					float nSumPlayCountN1 = 0, nSumPlayCountN2 = 0;
					if ( n1.arスコア[ nL12345 ] != null )
					{
						nSumPlayCountN1 = ( n1.arスコア[ nL12345 ].譜面情報.レベル[ (int) part ] / 10.0f ) + ( n1.arスコア[ nL12345 ].譜面情報.レベルDec[ (int) part ] / 100.0f );
					}
					if ( n2.arスコア[ nL12345 ] != null )
					{
						nSumPlayCountN2 = ( n2.arスコア[ nL12345 ].譜面情報.レベル[ (int) part ] / 10.0f ) + ( n2.arスコア[ nL12345 ].譜面情報.レベルDec[ (int) part ] / 100.0f );
					}
					num = nSumPlayCountN2 - nSumPlayCountN1;
					if ( num != 0 )
					{
						return (int)( (order * num) * 100 );
					}
					return order * n1.strタイトル.CompareTo( n2.strタイトル );
				} );
				foreach ( C曲リストノード c曲リストノード in ノードリスト )
				{
					int nSumPlayCountN1 = 0;
					if ( c曲リストノード.arスコア[ nL12345 ] != null )
					{
						nSumPlayCountN1 = c曲リストノード.arスコア[ nL12345 ].譜面情報.レベル[ (int) part ] + c曲リストノード.arスコア[ nL12345 ].譜面情報.レベルDec[ (int) part ];
					}
// Debug.WriteLine( nSumPlayCountN1 + ":" + c曲リストノード.strタイトル );
				}
			}
		}
		public void t曲リストのソート5_BestRank順( List<C曲リストノード> ノードリスト, E楽器パート part, int order, params object[] p )
		{
			order = -order;
			int nL12345 = (int) p[ 0 ];
			if ( part != E楽器パート.UNKNOWN )
			{
				ノードリスト.Sort( delegate( C曲リストノード n1, C曲リストノード n2 )
				{
					#region [ 共通処理 ]
					if ( n1 == n2 )
					{
						return 0;
					}
					int num = this.t比較0_共通( n1, n2 );
					if ( num != 0 )
					{
						return order * num;
					}
					if ( ( n1.eノード種別 == C曲リストノード.Eノード種別.BOX ) && ( n2.eノード種別 == C曲リストノード.Eノード種別.BOX ) )
					{
						return order * n1.arスコア[ 0 ].ファイル情報.フォルダの絶対パス.CompareTo( n2.arスコア[ 0 ].ファイル情報.フォルダの絶対パス );
					}
					#endregion
					int nSumPlayCountN1 = 0, nSumPlayCountN2 = 0;
					bool isFullCombo1 = false, isFullCombo2 = false;
					if ( n1.arスコア[ nL12345 ] != null )
					{
						isFullCombo1 = n1.arスコア[ nL12345 ].譜面情報.フルコンボ[ (int) part ];
						nSumPlayCountN1 = n1.arスコア[ nL12345 ].譜面情報.最大ランク[ (int) part ];
					}
					if ( n2.arスコア[ nL12345 ] != null )
					{
						isFullCombo2 = n2.arスコア[ nL12345 ].譜面情報.フルコンボ[ (int) part ];
						nSumPlayCountN2 = n2.arスコア[ nL12345 ].譜面情報.最大ランク[ (int) part ];
					}
					if ( isFullCombo1 ^ isFullCombo2 )
					{
						if ( isFullCombo1 ) return order; else return -order;
					}
					num = nSumPlayCountN2 - nSumPlayCountN1;
					if ( num != 0 )
					{
						return order * num;
					}
					return order * n1.strタイトル.CompareTo( n2.strタイトル );
				} );
				foreach ( C曲リストノード c曲リストノード in ノードリスト )
				{
					int nSumPlayCountN1 = 0;
					if ( c曲リストノード.arスコア[ nL12345 ] != null )
					{
						nSumPlayCountN1 = c曲リストノード.arスコア[ nL12345 ].譜面情報.最大ランク[ (int) part ];
					}
// Debug.WriteLine( nSumPlayCountN1 + ":" + c曲リストノード.strタイトル );
				}
			}
		}
		public void t曲リストのソート6_SkillPoint順( List<C曲リストノード> ノードリスト, E楽器パート part, int order, params object[] p )
		{
			order = -order;
			int nL12345 = (int) p[ 0 ];
			if ( part != E楽器パート.UNKNOWN )
			{
				ノードリスト.Sort( delegate( C曲リストノード n1, C曲リストノード n2 )
				{
					#region [ 共通処理 ]
					if ( n1 == n2 )
					{
						return 0;
					}
					int num = this.t比較0_共通( n1, n2 );
					if ( num != 0 )
					{
						return order * num;
					}
					if ( ( n1.eノード種別 == C曲リストノード.Eノード種別.BOX ) && ( n2.eノード種別 == C曲リストノード.Eノード種別.BOX ) )
					{
						return order * n1.arスコア[ 0 ].ファイル情報.フォルダの絶対パス.CompareTo( n2.arスコア[ 0 ].ファイル情報.フォルダの絶対パス );
					}
					#endregion
					double nSumPlayCountN1 = 0, nSumPlayCountN2 = 0;
					if ( n1.arスコア[ nL12345 ] != null )
					{
						nSumPlayCountN1 = n1.arスコア[ nL12345 ].譜面情報.最大スキル[ (int) part ];
					}
					if ( n2.arスコア[ nL12345 ] != null )
					{
						nSumPlayCountN2 = n2.arスコア[ nL12345 ].譜面情報.最大スキル[ (int) part ];
					}
					double d = nSumPlayCountN2 - nSumPlayCountN1;
					if ( d != 0 )
					{
						return order * System.Math.Sign(d);
					}
					return order * n1.strタイトル.CompareTo( n2.strタイトル );
				} );
				foreach ( C曲リストノード c曲リストノード in ノードリスト )
				{
					double nSumPlayCountN1 = 0;
					if ( c曲リストノード.arスコア[ nL12345 ] != null )
					{
						nSumPlayCountN1 = c曲リストノード.arスコア[ nL12345 ].譜面情報.最大スキル[ (int) part ];
					}
// Debug.WriteLine( nSumPlayCountN1 + ":" + c曲リストノード.strタイトル );
				}
			}
		}
		public void t曲リストのソート7_更新日時順( List<C曲リストノード> ノードリスト, E楽器パート part, int order, params object[] p )
		{
			int nL12345 = (int) p[ 0 ];
			if ( part != E楽器パート.UNKNOWN )
			{
				ノードリスト.Sort( delegate( C曲リストノード n1, C曲リストノード n2 )
				{
					#region [ 共通処理 ]
					if ( n1 == n2 )
					{
						return 0;
					}
					int num = this.t比較0_共通( n1, n2 );
					if ( num != 0 )
					{
						return order * num;
					}
					if ( ( n1.eノード種別 == C曲リストノード.Eノード種別.BOX ) && ( n2.eノード種別 == C曲リストノード.Eノード種別.BOX ) )
					{
						return order * n1.arスコア[ 0 ].ファイル情報.フォルダの絶対パス.CompareTo( n2.arスコア[ 0 ].ファイル情報.フォルダの絶対パス );
					}
					#endregion
					DateTime nSumPlayCountN1 = DateTime.Parse("0001/01/01 12:00:01.000");
					DateTime nSumPlayCountN2 = DateTime.Parse("0001/01/01 12:00:01.000");
					if ( n1.arスコア[ nL12345 ] != null )
					{
						nSumPlayCountN1 = n1.arスコア[ nL12345 ].ファイル情報.最終更新日時;
					}
					if ( n2.arスコア[ nL12345 ] != null )
					{
						nSumPlayCountN2 = n2.arスコア[ nL12345 ].ファイル情報.最終更新日時;
					}
					int d = nSumPlayCountN1.CompareTo(nSumPlayCountN2);
					if ( d != 0 )
					{
						return order * System.Math.Sign( d );
					}
					return order * n1.strタイトル.CompareTo( n2.strタイトル );
				} );
				foreach ( C曲リストノード c曲リストノード in ノードリスト )
				{
					DateTime nSumPlayCountN1 = DateTime.Parse( "0001/01/01 12:00:01.000" );
					if ( c曲リストノード.arスコア[ nL12345 ] != null )
					{
						nSumPlayCountN1 = c曲リストノード.arスコア[ nL12345 ].ファイル情報.最終更新日時;
					}
// Debug.WriteLine( nSumPlayCountN1 + ":" + c曲リストノード.strタイトル );
				}
			}
		}
		public void t曲リストのソート8_アーティスト名順( List<C曲リストノード> ノードリスト, E楽器パート part, int order, params object[] p )
		{
			int nL12345 = (int) p[ 0 ]; 
			ノードリスト.Sort( delegate( C曲リストノード n1, C曲リストノード n2 )
			{
				if ( n1 == n2 )
				{
					return 0;
				}
				int num = this.t比較0_共通( n1, n2 );
				if ( num != 0 )
				{
					return order * System.Math.Sign( num );
				}
				string strAuthorN1 = "";
				string strAuthorN2 = "";
				if (n1.arスコア[ nL12345 ] != null )
                {
					strAuthorN1 = n1.arスコア[ nL12345 ].譜面情報.アーティスト名;
				}
				if ( n2.arスコア[ nL12345 ] != null )
				{
					strAuthorN2 = n2.arスコア[ nL12345 ].譜面情報.アーティスト名;
				}

				return order * strAuthorN1.CompareTo( strAuthorN2 );
			} );
			foreach ( C曲リストノード c曲リストノード in ノードリスト )
			{
				string s = "";
				if ( c曲リストノード.arスコア[ nL12345 ] != null )
				{
					s = c曲リストノード.arスコア[ nL12345 ].譜面情報.アーティスト名;
				}
Debug.WriteLine( s + ":" + c曲リストノード.strタイトル );
			}
		}
#if TEST_SORTBGM
		public void t曲リストのソート9_BPM順( List<C曲リストノード> ノードリスト, E楽器パート part, int order, params object[] p )
		{
			order = -order;
			int nL12345 = (int) p[ 0 ];
			if ( part != E楽器パート.UNKNOWN )
			{
				ノードリスト.Sort( delegate( C曲リストノード n1, C曲リストノード n2 )
				{
					#region [ 共通処理 ]
					if ( n1 == n2 )
					{
						return 0;
					}
					int num = this.t比較0_共通( n1, n2 );
					if ( num != 0 )
					{
						return order * num;
					}
					if ( ( n1.eノード種別 == C曲リストノード.Eノード種別.BOX ) && ( n2.eノード種別 == C曲リストノード.Eノード種別.BOX ) )
					{
						return order * n1.arスコア[ 0 ].ファイル情報.フォルダの絶対パス.CompareTo( n2.arスコア[ 0 ].ファイル情報.フォルダの絶対パス );
					}
					#endregion
					double dBPMn1 = 0.0, dBPMn2 = 0.0;
					if ( n1.arスコア[ nL12345 ] != null )
					{
						dBPMn1 = n1.arスコア[ nL12345 ].譜面情報.bpm;
					}
					if ( n2.arスコア[ nL12345 ] != null )
					{
						dBPMn2 = n2.arスコア[ nL12345 ].譜面情報.bpm;
					}
					double d = dBPMn1- dBPMn2;
					if ( d != 0 )
					{
						return order * System.Math.Sign( d );
					}
					return order * n1.strタイトル.CompareTo( n2.strタイトル );
				} );
				foreach ( C曲リストノード c曲リストノード in ノードリスト )
				{
					double dBPM = 0;
					if ( c曲リストノード.arスコア[ nL12345 ] != null )
					{
						dBPM = c曲リストノード.arスコア[ nL12345 ].譜面情報.bpm;
					}
Debug.WriteLine( dBPM + ":" + c曲リストノード.strタイトル );
				}
			}
		}
#endif
		//-----------------
		#endregion
		#region [ .score.ini を読み込んで Cスコア.譜面情報に設定する ]
		//-----------------
		public void tScoreIniを読み込んで譜面情報を設定する( string strScoreIniファイルパス, ref Cスコア score )
		{
			if( !File.Exists( strScoreIniファイルパス ) )
				return;

			try
			{
				var ini = new CScoreIni( strScoreIniファイルパス );
				ini.t全演奏記録セクションの整合性をチェックし不整合があればリセットする();

				for( int n楽器番号 = 0; n楽器番号 < 3; n楽器番号++ )
				{
					int n = ( n楽器番号 * 2 ) + 1;	// n = 0～5

					#region socre.譜面情報.最大ランク[ n楽器番号 ] = ... 
					//-----------------
					if( ini.stセクション[ n ].b演奏にMIDI入力を使用した ||
						ini.stセクション[ n ].b演奏にキーボードを使用した ||
						ini.stセクション[ n ].b演奏にジョイパッドを使用した ||
						ini.stセクション[ n ].b演奏にマウスを使用した )
                    {
                        // (A) 全オートじゃないようなので、演奏結果情報を有効としてランクを算出する。
                        if ( CDTXMania.ConfigIni.nSkillMode == 0 )
                        {
                            score.譜面情報.最大ランク[ n楽器番号 ] =
                            CScoreIni.t旧ランク値を計算して返す(
                                ini.stセクション[n].n全チップ数,
                                ini.stセクション[n].nPerfect数,
                                ini.stセクション[n].nGreat数,
                                ini.stセクション[n].nGood数,
                                ini.stセクション[n].nPoor数,
                                ini.stセクション[n].nMiss数
                                );
                        }
                        else if( CDTXMania.ConfigIni.nSkillMode == 1 )
                        {
                            score.譜面情報.最大ランク[ n楽器番号 ] =
                            CScoreIni.tランク値を計算して返す(
                                ini.stセクション[ n ].n全チップ数,
                                ini.stセクション[ n ].nPerfect数,
                                ini.stセクション[ n ].nGreat数,
                                ini.stセクション[ n ].nGood数,
                                ini.stセクション[ n ].nPoor数,
                                ini.stセクション[ n ].nMiss数,
                                ini.stセクション[ n ].n最大コンボ数
                                );
                        }
                    }
					else
					{
						// (B) 全オートらしいので、ランクは無効とする。
						score.譜面情報.最大ランク[ n楽器番号 ] = (int) CScoreIni.ERANK.UNKNOWN;
					}
					//-----------------
					#endregion
					score.譜面情報.最大スキル[ n楽器番号 ] = ini.stセクション[ n ].db演奏型スキル値;
                    score.譜面情報.最大曲別スキル[ n楽器番号 ] = ini.stセクション[ n ].dbゲーム型スキル値;
					score.譜面情報.フルコンボ[ n楽器番号 ] = ini.stセクション[ n ].bフルコンボである | ini.stセクション[ n楽器番号 * 2 ].bフルコンボである;
				}
				score.譜面情報.演奏回数.Drums = ini.stファイル.PlayCountDrums;
				score.譜面情報.演奏回数.Guitar = ini.stファイル.PlayCountGuitar;
				score.譜面情報.演奏回数.Bass = ini.stファイル.PlayCountBass;
				for( int i = 0; i < 5; i++ )
					score.譜面情報.演奏履歴[ i ] = ini.stファイル.History[ i ];
			}
			catch
			{
				Trace.TraceError( "演奏記録ファイルの読み込みに失敗しました。[{0}]", strScoreIniファイルパス );
			}
		}
		//-----------------
		#endregion


		// その他

		#region [ private ]
		//-----------------
		private const string SONGSDB_VERSION = "SongsDB3(ver.K)rev2";
		private List<string> listStrBoxDefSkinSubfolderFullName;

		private int t比較0_共通( C曲リストノード n1, C曲リストノード n2 )
		{
			if( n1.eノード種別 == C曲リストノード.Eノード種別.BACKBOX )
			{
				return -1;
			}
			if( n2.eノード種別 == C曲リストノード.Eノード種別.BACKBOX )
			{
				return 1;
			}
			if( n1.eノード種別 == C曲リストノード.Eノード種別.RANDOM )
			{
				return 1;
			}
			if( n2.eノード種別 == C曲リストノード.Eノード種別.RANDOM )
			{
				return -1;
			}
			if( ( n1.eノード種別 == C曲リストノード.Eノード種別.BOX ) && ( n2.eノード種別 != C曲リストノード.Eノード種別.BOX ) )
			{
				return -1;
			}
			if( ( n1.eノード種別 != C曲リストノード.Eノード種別.BOX ) && ( n2.eノード種別 == C曲リストノード.Eノード種別.BOX ) )
			{
				return 1;
			}
			return 0;
		}

		/// <summary>
		/// 検索を中断・スローダウンする
		/// </summary>
		private void SlowOrSuspendSearchTask()
		{
			if ( this.bIsSuspending )		// #27060 中断要求があったら、解除要求が来るまで待機
			{
				autoReset.WaitOne();
			}
			if ( this.bIsSlowdown && ++this.searchCount > 10 )			// #27060 #PREMOVIE再生中は検索負荷を下げる
			{
				Thread.Sleep( 100 );
				this.searchCount = 0;
			}
		}

		//-----------------
		#endregion
	}
}
