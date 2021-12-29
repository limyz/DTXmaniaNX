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

		/// <summary>
		/// The custom <see cref="STHitRanges"/> for all drum chips, except pedals, wthin this box.
		/// </summary>
		/// <remarks>
		/// Note that individual values of this set can be set while others are not, as it is intended to be used with <see cref="STHitRanges.tCompose(STHitRanges, STHitRanges)"/>.
		/// </remarks>
		public STHitRanges stDrumHitRanges;

		/// <summary>
		/// The custom <see cref="STHitRanges"/> for drum pedal chips within this box.
		/// </summary>
		/// <remarks>
		/// Note that individual values of this set can be set while others are not, as it is intended to be used with <see cref="STHitRanges.tCompose(STHitRanges, STHitRanges)"/>.
		/// </remarks>
		public STHitRanges stDrumPedalHitRanges;

		/// <summary>
		/// The custom <see cref="STHitRanges"/> for guitar chips within this box.
		/// </summary>
		/// <remarks>
		/// Note that individual values of this set can be set while others are not, as it is intended to be used with <see cref="STHitRanges.tCompose(STHitRanges, STHitRanges)"/>.
		/// </remarks>
		public STHitRanges stGuitarHitRanges;

		/// <summary>
		/// The custom <see cref="STHitRanges"/> for bass guitar chips within this box.
		/// </summary>
		/// <remarks>
		/// Note that individual values of this set can be set while others are not, as it is intended to be used with <see cref="STHitRanges.tCompose(STHitRanges, STHitRanges)"/>.
		/// </remarks>
		public STHitRanges stBassHitRanges;

		public string Preimage;
		public string Premovie;
		public string Presound;
		public string Title;
		public string SkinPath;		// ""ならユーザー指定スキン、さもなくばbox.def指定スキン。
        public bool Difficulty;

		// コンストラクタ

		public CBoxDef()
		{
			this.Title = "";
			this.Artist = "";
			this.Comment = "BOX に移動します。";
			this.Genre = "";
			stDrumHitRanges = new STHitRanges(nDefaultSizeMs: -1);
			stDrumPedalHitRanges = new STHitRanges(nDefaultSizeMs: -1);
			stGuitarHitRanges = new STHitRanges(nDefaultSizeMs: -1);
			stBassHitRanges = new STHitRanges(nDefaultSizeMs: -1);
			this.Preimage = "";
			this.Premovie = "";
			this.Presound = "";
			this.Color = ColorTranslator.FromHtml( "White" );
			this.SkinPath = "";
            this.Difficulty = false;
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

									this.Difficulty = b;
								}
							}
							else
							{
								// hit ranges
								// map the legacy hit ranges to apply to each category
								// they should only appear when reading from a legacy box.def,
								// so simply copy values over whenever there is a change
								STHitRanges stLegacyHitRanges = new STHitRanges(nDefaultSizeMs: -1);
								if (tTryReadHitRangesField(str, string.Empty, ref stLegacyHitRanges))
								{
									stDrumHitRanges = STHitRanges.tCompose(stLegacyHitRanges, stDrumHitRanges);
									stDrumPedalHitRanges = STHitRanges.tCompose(stLegacyHitRanges, stDrumPedalHitRanges);
									stGuitarHitRanges = STHitRanges.tCompose(stLegacyHitRanges, stGuitarHitRanges);
									stBassHitRanges = STHitRanges.tCompose(stLegacyHitRanges, stBassHitRanges);
									continue;
								}

								if (tTryReadHitRangesField(str, @"DRUM", ref stDrumHitRanges))
									continue;

								if (tTryReadHitRangesField(str, @"DRUMPEDAL", ref stDrumPedalHitRanges))
									continue;

								if (tTryReadHitRangesField(str, @"GUITAR", ref stGuitarHitRanges))
									continue;

								if (tTryReadHitRangesField(str, @"BASS", ref stBassHitRanges))
									continue;
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

		/// <summary>
		/// Read the box.def <see cref="STHitRanges"/> field, if any, described by the given parameters into the given <see cref="STHitRanges"/>.
		/// </summary>
		/// <param name="strLine">The raw box.def line being read from.</param>
		/// <param name="strName">The unique identifier of <paramref name="stHitRanges"/>.</param>
		/// <param name="stHitRanges">The <see cref="STHitRanges"/> to read into.</param>
		/// <returns>Whether or not a field was read.</returns>
		private bool tTryReadHitRangesField(string strLine, string strName, ref STHitRanges stHitRanges)
		{
			switch (strLine)
			{
				// perfect range size (±ms)
				case var l when tTryReadInt(l, $@"{strName}PERFECTRANGE", out var r):
					stHitRanges.nPerfectSizeMs = r;
					return true;

				// great range size (±ms)
				case var l when tTryReadInt(l, $@"{strName}GREATRANGE", out var r):
					stHitRanges.nGreatSizeMs = r;
					return true;

				// good range size (±ms)
				case var l when tTryReadInt(l, $@"{strName}GOODRANGE", out var r):
					stHitRanges.nGoodSizeMs = r;
					return true;

				// poor range size (±ms)
				case var l when tTryReadInt(l, $@"{strName}POORRANGE", out var r):
					stHitRanges.nPoorSizeMs = r;
					return true;

				// unknown field
				default:
					return false;
			}
		}

		/// <summary>
		/// Read the box.def <see cref="int"/> field, if any, described by the given parameters into the given <see cref="int"/>.
		/// </summary>
		/// <param name="strLine">The raw box.def line being read from.</param>
		/// <param name="strFieldName">The name of the field to try and read.</param>
		/// <param name="nValue">The <see cref="int"/> to read into.</param>
		/// <returns>Whether or not the field was read.</returns>
		private bool tTryReadInt(string strLine, string strFieldName, out int nValue)
		{
			// write a default value in case of a failure
			nValue = 0;

			// ensure the line is for the given field
			string strPrefix = $@"#{strFieldName}";
			if (!strLine.StartsWith(strPrefix, StringComparison.OrdinalIgnoreCase))
				return false;

			// read the value into the given int, stripping the field name and ignored characters
			char[] chIgnoredCharacters = new[] { ':', ' ', '\t' };
			string strValue = strLine.Substring(strPrefix.Length).Trim(chIgnoredCharacters);
			if (!int.TryParse(strValue, out nValue))
				return false;

			return true;
		}
	}
}
