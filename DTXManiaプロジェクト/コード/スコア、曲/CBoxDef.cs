using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace DTXMania
{
	internal class CBoxDef
	{
		// プロパティ

		public string Artist;
		public Color Color;
		public string Comment;
		public string Genre;
		public int GoodRange;
		public int GreatRange;
		public int PerfectRange;
		public int PoorRange;
		public string Preimage;
		public string Premovie;
		public string Presound;
		public string Title;
		public string SkinPath;		// ""ならユーザー指定スキン、さもなくばbox.def指定スキン。
        public bool Difficlty;

		// コンストラクタ

		public CBoxDef()
		{
			this.Title = "";
			this.Artist = "";
			this.Comment = "BOX に移動します。";
			this.Genre = "";
			this.Preimage = "";
			this.Premovie = "";
			this.Presound = "";
			this.Color = ColorTranslator.FromHtml( "White" );
			this.PerfectRange = -1;
			this.GreatRange = -1;
			this.GoodRange = -1;
			this.PoorRange = -1;
			this.SkinPath = "";
            this.Difficlty = false;
		}
		public CBoxDef( string boxdefファイル名 )
			: this()
		{
			this.t読み込み( boxdefファイル名 );
		}


		// メソッド

		public void t読み込み( string boxdefファイル名 )
		{
			StreamReader reader = new StreamReader( boxdefファイル名, Encoding.GetEncoding( "shift-jis" ) );
			string str = null;
			while( ( str = reader.ReadLine() ) != null )
			{
				if( str.Length != 0 )
				{
					try
					{
						char[] ignoreCharsWoColon = new char[] { ' ', '\t' };

						str = str.TrimStart( ignoreCharsWoColon );
						if( ( str[ 0 ] == '#' ) && ( str[ 0 ] != ';' ) )
						{
							if( str.IndexOf( ';' ) != -1 )
							{
								str = str.Substring( 0, str.IndexOf( ';' ) );
							}

							char[] ignoreChars = new char[] { ':', ' ', '\t' };
		
							if ( str.StartsWith( "#TITLE", StringComparison.OrdinalIgnoreCase ) )
							{
								this.Title = str.Substring( 6 ).Trim( ignoreChars );
							}
							else if( str.StartsWith( "#ARTIST", StringComparison.OrdinalIgnoreCase ) )
							{
								this.Artist = str.Substring( 7 ).Trim( ignoreChars );
							}
							else if( str.StartsWith( "#COMMENT", StringComparison.OrdinalIgnoreCase ) )
							{
								this.Comment = str.Substring( 8 ).Trim( ignoreChars );
							}
							else if( str.StartsWith( "#GENRE", StringComparison.OrdinalIgnoreCase ) )
							{
								this.Genre = str.Substring( 6 ).Trim( ignoreChars );
							}
							else if( str.StartsWith( "#PREVIEW", StringComparison.OrdinalIgnoreCase ) )
							{
								this.Presound = str.Substring( 8 ).Trim( ignoreChars );
							}
							else if( str.StartsWith( "#PREIMAGE", StringComparison.OrdinalIgnoreCase ) )
							{
								this.Preimage = str.Substring( 9 ).Trim( ignoreChars );
							}
							else if( str.StartsWith( "#PREMOVIE", StringComparison.OrdinalIgnoreCase ) )
							{
								this.Premovie = str.Substring( 9 ).Trim( ignoreChars );
							}
							else if ( str.StartsWith( "#SKINPATH", StringComparison.OrdinalIgnoreCase ) )
							{
								this.SkinPath = str.Substring( 9 ).Trim( ignoreChars );
							}
							else if ( str.StartsWith( "#FONTCOLOR", StringComparison.OrdinalIgnoreCase ) )
							{
								this.Color = ColorTranslator.FromHtml( str.Substring( 10 ).Trim( ignoreChars ) );
							}
							else if( str.StartsWith( "#PERFECTRANGE", StringComparison.OrdinalIgnoreCase ) )
							{
								int range = 0;
								if ( int.TryParse( str.Substring( 13 ).Trim( ignoreChars ), out range ) && ( range >= 0 ) )
								{
									this.PerfectRange = range;
								}
							}
							else if( str.StartsWith( "#GREATRANGE", StringComparison.OrdinalIgnoreCase ) )
							{
								int range = 0;
								if ( int.TryParse( str.Substring( 11 ).Trim( ignoreChars ), out range ) && ( range >= 0 ) )
								{
									this.GreatRange = range;
								}
							}
							else if( str.StartsWith( "#GOODRANGE", StringComparison.OrdinalIgnoreCase ) )
							{
								int range = 0;
								if ( int.TryParse( str.Substring( 10 ).Trim( ignoreChars ), out range ) && ( range >= 0 ) )
								{
									this.GoodRange = range;
								}
							}
							else if( str.StartsWith( "#POORRANGE", StringComparison.OrdinalIgnoreCase ) )
							{
								int range = 0;
								if ( int.TryParse( str.Substring( 10 ).Trim( ignoreChars ), out range ) && ( range >= 0 ) )
								{
									this.PoorRange = range;
								}
							}
                            else if ( str.StartsWith( "#DIFFICULTY", StringComparison.OrdinalIgnoreCase ) )
							{
								int range = 0;
                                bool b = false;
								if ( int.TryParse( str.Substring( 11 ).Trim( ignoreChars ), out range ) && ( range >= 0 ) )
								{
                                    if ( range == 0 )
                                    {
                                        b = false;
                                    }
                                    else
                                    {
                                        b = true;
                                    }

									this.Difficlty = b;
								}
							}
						}
						continue;
					}
					catch
					{
						continue;
					}
				}
			}
			reader.Close();
		}
	}
}
