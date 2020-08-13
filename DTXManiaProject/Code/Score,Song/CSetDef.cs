using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace DTXMania
{
	public class CSetDef
	{
		// プロパティ

		public List<CBlock> blocks;
		public class CBlock
		{
			/// <summary>
			/// このブロックが有効である（何かのプロパティがセットされた）場合、true を示す。
			/// </summary>
			public bool b使用中 { get; set; }

			/// <summary>
			/// スコアファイル名（#LxFILE）を保持する。配列は [0～4] で、存在しないレベルは null となる。
			/// </summary>
			public string[] File
			{
				get
				{
					return this._file;
				}
                set // ここには来ない( Label[xx] にsetすると、結局Labelのgetが呼ばれるだけで、Labelのsetは呼ばれない)
				{
					this._file = value;
					this.b使用中 = true;
				}
			}

			/// <summary>
			/// スコアのフォント色（#FONTCOLOR）を保持する。
			/// </summary>
			public Color FontColor
			{
				get
				{
					return this._fontcolor;
				}
				set
				{
					this._fontcolor = value;
					this.b使用中 = true;
				}
			}

			/// <summary>
			/// スコアのジャンル名を保持する。（現在は使われていない。）
			/// </summary>
			public string Genre
			{
				get
				{
					return this._genre;
				}
				set
				{
					this._genre = value;
					this.b使用中 = true;
				}
			}

			/// <summary>
			/// スコアのラベル（#LxLABEL）を保持する。配列は[0～4] で、存在しないレベルは null となる。
			/// </summary>
			public string[] Label
			{
				get
				{
					return this._label;
				}
                set // ここには来ない( Label[xx] にsetすると、結局Labelのgetが呼ばれるだけで、Labelのsetは呼ばれない)
				{
					this._label = value;
					this.b使用中 = true;
				}
			}

			/// <summary>
			/// スコアのタイトル（#TITLE）を保持する。
			/// </summary>
			public string Title
			{
				get
				{
					return this._title;
				}
				set
				{
					this._title = value;
					this.b使用中 = true;
				}
			}

			#region [ private ]
			//-----------------
			private string[] _file = new string[ 5 ];
			private Color _fontcolor = Color.White;
			private string _genre = "";
			private string[] _label = new string[ 5 ];
			private string _title = "";
			//-----------------
			#endregion
		}


		// コンストラクタ

		public CSetDef()
		{
			this.blocks = new List<CBlock>();
		}
		public CSetDef( string setdefファイル名 )
			: this()
		{
			this.t読み込み( setdefファイル名 );
		}


		// メソッド

		public void t読み込み( string setdefファイル名 )
		{
			var reader = new StreamReader( setdefファイル名, Encoding.GetEncoding( "shift-jis" ) );
			CBlock block = new CBlock();
			string str = null;
			while( ( str = reader.ReadLine() ) != null )
			{
				if( str.Length != 0 )
				{
					try
					{
						str = str.TrimStart( new char[] { ' ', '\t' } );
						if( ( str.Length > 0 ) && ( str[ 0 ] == '#' ) && ( str[ 0 ] != ';' ) )
						{
							if( str.IndexOf( ';' ) != -1 )
							{
								str = str.Substring( 0, str.IndexOf( ';' ) );
							}
							if( str.StartsWith( "#TITLE", StringComparison.OrdinalIgnoreCase ) )
							{
								if( block.b使用中 )
								{
									this.tFILEの指定があるのにLxLABELが省略されているときはデフォルトの名前をセットする( block );
									this.tLxLABELの指定があるのにFILEが省略されているときはなかったものとする( block );
									this.blocks.Add( block );
									block = new CBlock();
								}
								block.Title = str.Substring( 6 ).TrimStart( new char[] { ':', ' ', '\t' } );
							}
							else if( str.StartsWith( "#FONTCOLOR", StringComparison.OrdinalIgnoreCase ) )
							{
								block.FontColor = ColorTranslator.FromHtml( "#" + str.Substring( 10 ).Trim( new char[] { ':', '#', ' ', '\t' } ) );
							}
							else if( str.StartsWith( "#L1FILE", StringComparison.OrdinalIgnoreCase ) )
							{
								block.File[ 0 ] = str.Substring( 7 ).Trim( new char[] { ':', ' ', '\t' } );
								block.b使用中 = true;		// #28937 2012.7.7 yyagi; "get" accessor is called for T[] property. So b使用中 is not modified to set the property. I need to update it myself.
							}
							else if( str.StartsWith( "#L2FILE", StringComparison.OrdinalIgnoreCase ) )
							{
								block.File[ 1 ] = str.Substring( 7 ).Trim( new char[] { ':', ' ', '\t' } );
								block.b使用中 = true;		// #28937 2012.7.7 yyagi; "get" accessor is called for T[] property. So b使用中 is not modified to set the property. I need to update it myself.
							}
							else if( str.StartsWith( "#L3FILE", StringComparison.OrdinalIgnoreCase ) )
							{
								block.File[ 2 ] = str.Substring( 7 ).Trim( new char[] { ':', ' ', '\t' } );
								block.b使用中 = true;		// #28937 2012.7.7 yyagi; "get" accessor is called for T[] property. So b使用中 is not modified to set the property. I need to update it myself.
							}
							else if( str.StartsWith( "#L4FILE", StringComparison.OrdinalIgnoreCase ) )
							{
								block.File[ 3 ] = str.Substring( 7 ).Trim( new char[] { ':', ' ', '\t' } );
								block.b使用中 = true;		// #28937 2012.7.7 yyagi; "get" accessor is called for T[] property. So b使用中 is not modified to set the property. I need to update it myself.
							}
							else if( str.StartsWith( "#L5FILE", StringComparison.OrdinalIgnoreCase ) )
							{
								block.File[ 4 ] = str.Substring( 7 ).Trim( new char[] { ':', ' ', '\t' } );
								block.b使用中 = true;		// #28937 2012.7.7 yyagi; "get" accessor is called for T[] property. So b使用中 is not modified to set the property. I need to update it myself.
							}
							else if( str.StartsWith( "#L1LABEL", StringComparison.OrdinalIgnoreCase ) )
							{
								block.Label[ 0 ] = str.Substring( 8 ).Trim( new char[] { ':', ' ', '\t' } );
								block.b使用中 = true;		// #28937 2012.7.7 yyagi; "get" accessor is called for T[] property. So b使用中 is not modified to set the property. I need to update it myself.
							}
							else if( str.StartsWith( "#L2LABEL", StringComparison.OrdinalIgnoreCase ) )
							{
								block.Label[ 1 ] = str.Substring( 8 ).Trim( new char[] { ':', ' ', '\t' } );
								block.b使用中 = true;		// #28937 2012.7.7 yyagi; "get" accessor is called for T[] property. So b使用中 is not modified to set the property. I need to update it myself.
							}
							else if( str.StartsWith( "#L3LABEL", StringComparison.OrdinalIgnoreCase ) )
							{
								block.Label[ 2 ] = str.Substring( 8 ).Trim( new char[] { ':', ' ', '\t' } );
								block.b使用中 = true;		// #28937 2012.7.7 yyagi; "get" accessor is called for T[] property. So b使用中 is not modified to set the property. I need to update it myself.
							}
							else if( str.StartsWith( "#L4LABEL", StringComparison.OrdinalIgnoreCase ) )
							{
								block.Label[ 3 ] = str.Substring( 8 ).Trim( new char[] { ':', ' ', '\t' } );
								block.b使用中 = true;		// #28937 2012.7.7 yyagi; "get" accessor is called for T[] property. So b使用中 is not modified to set the property. I need to update it myself.
							}
							else if( str.StartsWith( "#L5LABEL", StringComparison.OrdinalIgnoreCase ) )
							{
								block.Label[ 4 ] = str.Substring( 8 ).Trim( new char[] { ':', ' ', '\t' } );
								block.b使用中 = true;		// #28937 2012.7.7 yyagi; "get" accessor is called for T[] property. So b使用中 is not modified to set the property. I need to update it myself.
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
			if( block.b使用中 )
			{
				this.tFILEの指定があるのにLxLABELが省略されているときはデフォルトの名前をセットする( block );
				this.tLxLABELの指定があるのにFILEが省略されているときはなかったものとする( block );
				this.blocks.Add( block );
			}
		}


		// その他

		#region [ private ]
		//-----------------
		private void tFILEの指定があるのにLxLABELが省略されているときはデフォルトの名前をセットする( CBlock block )
		{
			string[] strArray = new string[] { "NOVICE", "REGULAR", "EXPERT", "MASTER", "DTXMania" };
			for( int i = 0; i < 5; i++ )
			{
				if( ( ( block.File[ i ] != null ) && ( block.File[ i ].Length > 0 ) ) && string.IsNullOrEmpty( block.Label[ i ] ) )
				{
					block.Label[ i ] = strArray[ i ];
				}
			}
		}
		private void tLxLABELの指定があるのにFILEが省略されているときはなかったものとする( CBlock block )
		{
			for( int i = 0; i < 5; i++ )
			{
				if( ( ( block.Label[ i ] != null ) && ( block.Label[ i ].Length > 0 ) ) && string.IsNullOrEmpty( block.File[ i ] ) )
				{
					block.Label[ i ] = "";
				}
			}
		}
		//-----------------
		#endregion
	}
}
