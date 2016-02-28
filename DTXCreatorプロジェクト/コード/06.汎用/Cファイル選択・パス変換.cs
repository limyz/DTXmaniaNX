using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DTXCreator.汎用
{
	public class Cファイル選択・パス変換
	{
		public static bool b絶対パスである( string strパス )
		{
			try
			{
				new Uri( strパス );
			}
			catch
			{
				return false;
			}
			return true;
		}
		public static string str基点からの相対パスに変換して返す( string str変換対象の絶対パス, string str基点となる絶対パス )
		{
			if( str基点となる絶対パス == "" )
			{
				return str変換対象の絶対パス;
			}
			if( str変換対象の絶対パス == "" )
			{
				return "";
			}
			Uri uri = new Uri( str基点となる絶対パス );
			return Uri.UnescapeDataString( uri.MakeRelativeUri( new Uri( str変換対象の絶対パス ) ).ToString() ).Replace( '/', '\\' );
		}
		public static string str指定されたファイルの存在するフォルダを絶対パスで返す( string strファイルのパス, string strパスが相対の場合の基点絶対パス )
		{
			if( !b絶対パスである( strファイルのパス ) )
			{
				strファイルのパス = strパスが相対の場合の基点絶対パス + strファイルのパス;
			}
			string directoryName = Path.GetDirectoryName( strファイルのパス );
			if( directoryName == null )
			{
				return @"\";
			}
			if( !directoryName.EndsWith( @"\" ) )
			{
				directoryName = directoryName + @"\";
			}
			return directoryName;
		}
	}
}
