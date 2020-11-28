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
		/// The custom <see cref="CHitRanges"/> for all drum chips, except pedals, wthin this box.
		/// </summary>
		/// <remarks>
		/// As each range can be individually overridden, if an individual range is less than zero then it uses the global value.
		/// </remarks>
		public CHitRanges DrumHitRanges;

		/// <summary>
		/// The custom <see cref="CHitRanges"/> for drum pedal chips within this box.
		/// </summary>
		/// <remarks>
		/// As each range can be individually overridden, if an individual range is less than zero then it uses the global value.
		/// </remarks>
		public CHitRanges DrumPedalHitRanges;

		/// <summary>
		/// The custom <see cref="CHitRanges"/> for guitar chips within this box.
		/// </summary>
		/// <remarks>
		/// As each range can be individually overridden, if an individual range is less than zero then it uses the global value.
		/// </remarks>
		public CHitRanges GuitarHitRanges;

		/// <summary>
		/// The custom <see cref="CHitRanges"/> for bass guitar chips within this box.
		/// </summary>
		/// <remarks>
		/// As each range can be individually overridden, if an individual range is less than zero then it uses the global value.
		/// </remarks>
		public CHitRanges BassHitRanges;

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
			this.SkinPath = "";
            this.Difficlty = false;

			DrumHitRanges = new CHitRanges(@"DRUM")
			{
				Perfect = -1,
				Great = -1,
				Good = -1,
				Poor = -1,
			};

			DrumPedalHitRanges = new CHitRanges(@"DRUMPEDAL")
			{
				Perfect = -1,
				Great = -1,
				Good = -1,
				Poor = -1,
			};

			GuitarHitRanges = new CHitRanges(@"GUITAR")
			{
				Perfect = -1,
				Great = -1,
				Good = -1,
				Poor = -1,
			};

			BassHitRanges = new CHitRanges(@"BASS")
			{
				Perfect = -1,
				Great = -1,
				Good = -1,
				Poor = -1,
			};
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

									this.Difficlty = b;
								}
							}
							else
							{
								// hit ranges
								// map the legacy hit ranges to apply to each category
								// they should only appear when reading from a legacy box.def,
								// so simply copy all values over whenever there is a change
								CHitRanges legacyRanges = new CHitRanges(string.Empty);
								if (tTryReadHitRangesField(str, legacyRanges))
								{
									DrumHitRanges.CopyFrom(legacyRanges);
									DrumPedalHitRanges.CopyFrom(legacyRanges);
									GuitarHitRanges.CopyFrom(legacyRanges);
									BassHitRanges.CopyFrom(legacyRanges);
									continue;
								}

								if (tTryReadHitRangesField(str, DrumHitRanges))
									continue;

								if (tTryReadHitRangesField(str, DrumPedalHitRanges))
									continue;

								if (tTryReadHitRangesField(str, GuitarHitRanges))
									continue;

								if (tTryReadHitRangesField(str, BassHitRanges))
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
		/// Read the box.def <see cref="CHitRanges"/> field, if any, described by the given parameters into the given <see cref="CHitRanges"/>.
		/// </summary>
		/// <param name="strLine">The raw box.def line being read from.</param>
		/// <param name="ranges">The <see cref="CHitRanges"/> to read into.</param>
		/// <returns>Whether or not a field was read.</returns>
		private bool tTryReadHitRangesField(string strLine, CHitRanges ranges)
		{
			switch (strLine)
			{
				// perfect range size (±ms)
				case var l when tTryReadInt(l, $@"#{ranges.Name}PERFECTRANGE", out var r):
					ranges.Perfect = r;
					return true;

				// great range size (±ms)
				case var l when tTryReadInt(l, $@"#{ranges.Name}GREATRANGE", out var r):
					ranges.Great = r;
					return true;

				// good range size (±ms)
				case var l when tTryReadInt(l, $@"#{ranges.Name}GOODRANGE", out var r):
					ranges.Good = r;
					return true;

				// poor range size (±ms)
				case var l when tTryReadInt(l, $@"#{ranges.Name}POORRANGE", out var r):
					ranges.Poor = r;
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
		/// <param name="iValue">The <see cref="int"/> to read into.</param>
		/// <returns>Whether or not the field was read.</returns>
		private bool tTryReadInt(string strLine, string strFieldName, out int iValue)
		{
			// write a default value in case of a failure
			iValue = -1;

			// ensure the line is for the given field
			string strPrefix = $@"#{strFieldName}";
			if (!strLine.StartsWith(strPrefix, StringComparison.OrdinalIgnoreCase))
				return false;

			// read the value into the given int, stripping the field name and ignored characters
			char[] chIgnoredCharacters = new[] { ':', ' ', '\t' };
			string strValue = strLine.Substring(strPrefix.Length).Trim(chIgnoredCharacters);
			if (!int.TryParse(strValue, out iValue))
				return false;

			return true;
		}
	}
}
