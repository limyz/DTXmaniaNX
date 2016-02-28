using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Security;
using System.Security.AccessControl;

namespace DTXMania
{
	class Folder : IDisposable
	{
		// プロパティ

		/// <summary>
		/// <para>システムフォルダ。StrokeStyleT.exe が格納されているフォルダを意味する。読み込み専用。</para>
		/// <para>（例："c:\Program Files\StrokeStyleT" ）</para>
		/// </summary>
		public string strシステムフォルダ
		{
			get;
			protected set;
		}

		/// <summary>
		/// <para>全ユーザで共通のデータを格納するフォルダ。読み書き両用。</para>
		/// <para>（例： 2000,XP … "C:\Documents and Settings\All Users\Application Data\StrokeStyleT"</para>
		/// <para>　　　 Vista,7 … "C:\PrograData\StrokeStyleT"）</para>
		/// </summary>
		public string strユーザ共通フォルダ
		{
			get
			{
				var folder = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.CommonApplicationData ), @"StrokeStyleT" );

				// なければ作成する。

				if( !Directory.Exists( folder ) )
					Directory.CreateDirectory( folder );	// 例外が起きたらそのまま発出。

				return folder;
			}
		}

		/// <summary>
		/// <para>ユーザフォルダ。読み書き両用。</para>
		/// <para>ユーザ共通フォルダ ＋ SSTユーザ名。</para>
		/// <para>（例："C:\PrograData\StrokeStyleT\"SSTFViewer" ）</para>
		/// </summary>
		public string strユーザ個別フォルダ
		{
			get;
			protected set;
		}


		// ユーザ共通
#if 封印
		public string strUsersXMLの絶対パス
		{
			get { return Path.Combine( this.strユーザ共通フォルダ, Properties.Resources.XMLNAME_USERS ); }
		}
		public string strEnvironmentPropertiesXMLの絶対パス
		{
			get { return Path.Combine( this.strユーザ共通フォルダ, Properties.Resources.XMLNAME_ENVIRONMENT_PROPERTIES ); }
		}
		public string strKeyAssignXMLの絶対パス
		{
			get { return Path.Combine( this.strユーザ共通フォルダ, Properties.Resources.XMLNAME_KEY_ASSIGN ); }
		}
		public string strMidiNoteNameMappingsXMLの絶対パス
		{
			get { return Path.Combine( this.strユーザ共通フォルダ, Properties.Resources.XMLNAME_MIDINOTE_NAME_MAPPINGS ); }
		}

		// ユーザ依存
		public string strConfigXMLの絶対パス
		{
			get { return Path.Combine( this.strユーザ個別フォルダ, Properties.Resources.XMLNAME_CONFIG ); }
		}
#endif


		// メソッド

		public Folder()
		{
			this.strユーザ個別フォルダ = null;

#if DEBUG
			this.strシステムフォルダ =  Environment.CurrentDirectory;							// DEBUG 時に限り、システムフォルダはカレントフォルダ（VC# で設定した作業フォルダ）になる。
#else
			this.strシステムフォルダ =  Path.GetDirectoryName( Application.ExecutablePath );	// DEBUG 以外は、exe の存在するフォルダ。
#endif
		}
		public Folder( string strユーザ名 )
			: this()
		{
			this.tユーザ個別フォルダを変更する( strユーザ名 );
		}

		public void tユーザ個別フォルダを変更する( string str新しいユーザ名 )
		{
			#region [ 新しいユーザ名が null なら初期化して終了。]
			//-----------------
			if( string.IsNullOrEmpty( str新しいユーザ名 ) )
			{
				this.strユーザ個別フォルダ = null;
				return;
			}
			//-----------------
			#endregion

			this.strユーザ個別フォルダ = Path.Combine( this.strユーザ共通フォルダ, str新しいユーザ名 );


			// フォルダがなければ作成する。

			if( !Directory.Exists( this.strユーザ個別フォルダ ) )
				Directory.CreateDirectory( this.strユーザ個別フォルダ );	// 作成に失敗したら例外発出。
		}

		/// <summary>
		/// <para>指定されたパスを絶対パスに変換して返す。</para>
		/// <para>・path が相対パス指定であれば、str相対パス時のルートからの相対パスと見なす。</para>
		/// <para>・path が空文字列 or null であれば空文字列を返す。</para>
		/// <para>・path に問題があれば例外を発出する。</para>
		/// </summary>
		public string tパスを絶対パスに変換しパスとしての正当性を確認する( string path, string str相対パス時のルート )
		{
			if( string.IsNullOrEmpty( path ) )		// 空文字列 or null は空文字列にして返す。
				return "";

			#region [ path が相対パス指定であれば、引数 str相対パス時のルートからの相対パスと見なす。]
			//-----------------
			try
			{
				if( !Path.IsPathRooted( path ) )
					path = Path.Combine( str相対パス時のルート, path );
			}
			catch( ArgumentException e )
			{
				throw new ArgumentException( string.Format( "パス文字列 '{0}' に、無効な文字が含まれています。", CDTXMania.Folder.tファイルパスをマクロ付きパスに逆展開する( path ) ), e );
			}
			//-----------------
			#endregion
			#region [ path 文字列がパスとして正しいかを個別確認する。]
			//-----------------
			try
			{
				path = Path.GetFullPath( path );	// path は既に絶対パスになっているが、Path.GetFullPath() を利用して、パス文字列の正当性確認を行う。
			}
			catch( ArgumentException e )
			{
				throw new ArgumentException( string.Format( "パス文字列 '{0}' に、無効な文字が含まれています。", path ), e );
			}
			catch( SecurityException e )
			{
				throw new SecurityException( string.Format( "パス '{0}' には、アクセス権限がありません。", path ), e );
			}
			catch( NotSupportedException e )
			{
				throw new NotSupportedException( string.Format( "パス文字列 '{0}' に、サポートされていない文字が含まれています。", path ), e );
			}
			catch( PathTooLongException e )
			{
				throw new PathTooLongException( string.Format( "パス文字列 '{0}' が長すぎます。", path ), e );
			}
			//-----------------
			#endregion

			return path;
		}
		public string tパスを絶対パスに変換しパスとしての正当性を確認する( string path )
		{
			return this.tパスを絶対パスに変換しパスとしての正当性を確認する( path, @"\" );
		}
		public void tパスを絶対パスに変換しパスとしての正当性を確認する( ref string path, string str相対パス時のルート )
		{
			path = this.tパスを絶対パスに変換しパスとしての正当性を確認する( path, str相対パス時のルート );
		}
		public void tパスを絶対パスに変換しパスとしての正当性を確認する( ref string path )
		{
			path = this.tパスを絶対パスに変換しパスとしての正当性を確認する( path, @"\" );
		}

		public string tファイルパスのマクロを展開する( string path )
		{
			this.tファイルパスのマクロを展開する( ref path );
			return path;
		}
		public void tファイルパスのマクロを展開する( ref string path )
		{
			path = path.Replace( @"$(SystemFolder)", this.strシステムフォルダ );
			path = path.Replace( @"$(CommonUserFolder)", this.strユーザ共通フォルダ );

			if( !string.IsNullOrEmpty( this.strユーザ個別フォルダ ) )
				path = path.Replace( @"$(UserFolder)", this.strユーザ個別フォルダ );
		}
		public string tファイルパスをマクロ付きパスに逆展開する( string path )
		{
			this.tファイルパスをマクロ付きパスに逆展開する( ref path );
			return path;
		}
		public void tファイルパスをマクロ付きパスに逆展開する( ref string path )
		{
			path = path.Replace( this.strシステムフォルダ, @"$(SystemFolder)" );
			path = path.Replace( this.strユーザ共通フォルダ, @"$(CommonUserFolder)" );
	
			if( !string.IsNullOrEmpty( this.strユーザ個別フォルダ ) )
				path = path.Replace( this.strユーザ個別フォルダ, @"$(UserFolder)" );
		}

		#region [ IDisposable 実装 ]
		//-----------------
		public void Dispose()
		{
			// 特になし
		}
		//-----------------
		#endregion
	}
}
