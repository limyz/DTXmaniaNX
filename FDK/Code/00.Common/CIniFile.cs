using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace FDK
{
	/// <summary>
	/// 汎用的な .iniファイルを扱う。
	/// </summary>
	public class CIniFile
	{
		// プロパティ

		public string strFilename 
		{
			get;
			private set;
		}
		public List<CSection> Sections
		{
			get;
			set; 
		}
		public class CSection
		{
			public string strセクション名 = "";
			public List<KeyValuePair<string,string>> listパラメータリスト = new List<KeyValuePair<string, string>>();
		}


		// コンストラクタ

		public CIniFile()
		{
			this.strFilename = "";
			this.Sections = new List<CSection>();
		}
		public CIniFile( string strファイル名 )
			:this()
		{
			this.tRead( strファイル名 );
		}


		// メソッド

		public void tRead( string strファイル名 )
		{
			this.strFilename = strファイル名;

			StreamReader sr = null;
			CSection section = null;
			try
			{
				sr = new StreamReader( this.strFilename, Encoding.GetEncoding( "Shift_JIS" ) );	// ファイルが存在しない場合は例外発生。

				string line;
				while( ( line = sr.ReadLine() ) != null )
				{
					line = line.Replace( '\t', ' ' ).TrimStart( new char[] { '\t', ' ' } );
					if( string.IsNullOrEmpty( line ) || line[ 0 ] == ';' )	// ';'以降はコメントとして無視
						continue;

					if( line[ 0 ] == '[' )
					{
						#region [ セクションの変更 ]
						//-----------------------------
						var builder = new StringBuilder( 32 );
						int num = 1;
						while( ( num < line.Length ) && ( line[ num ] != ']' ) )
							builder.Append( line[ num++ ] );

						// 変数 section が使用中の場合は、List<CSection> に追加して新しい section を作成する。
						if( section != null )
							this.Sections.Add( section );

						section = new CSection();
						section.strセクション名 = builder.ToString();
						//-----------------------------
						#endregion

						continue;
					}

					string[] strArray = line.Split( new char[] { '=' } );
					if( strArray.Length != 2 )
						continue;

					string key = strArray[ 0 ].Trim();
					string value = strArray[ 1 ].Trim();

					if( section != null && !string.IsNullOrEmpty( key ) && !string.IsNullOrEmpty( value ) )
						section.listパラメータリスト.Add( new KeyValuePair<string, string>( key, value ) );
				}

				if( section != null )
					this.Sections.Add( section );
			}
			finally
			{
				if( sr != null )
					sr.Close();
			}
		}
		public void tWrite( string strファイル名 )
		{
			this.strFilename = strファイル名;
			this.tWrite();
		}
		public void tWrite()
		{
			StreamWriter sw = null;
			try
			{
				sw = new StreamWriter( this.strFilename, false, Encoding.GetEncoding( "Shift_JIS" ) );	// オープン失敗の場合は例外発生。

				foreach( CSection section in this.Sections )
				{
					sw.WriteLine( "[{0}]", section.strセクション名 );

					foreach( KeyValuePair<string,string> kvp in section.listパラメータリスト )
						sw.WriteLine( "{0}={1}", kvp.Key, kvp.Value );
				}
			}
			finally
			{
				if( sw != null )
					sw.Close();
			}
		}
	}
}
