using System;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace DTXCreator
{
	/// <summary>
	/// DTXV呼び出し時にDTXファイルを%TEMP%フォルダに作成する
	/// その際、直前に作ったDTXファイルを消去する
	/// dispose時も消去する
	/// </summary>
	internal class MakeTempDTX : IDisposable					// #24746 2011.4.1 yyagi add; a countermeasure for temp-flooding
	{
		/// <summary>
		/// 直前に作成したtemp dtxファイルのフルパス
		/// </summary>
		string lastTempFullPath;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		internal void makeTempDTX()
		{
			this.lastTempFullPath = null;
		}

		/// <summary>
		/// temp dtxファイル名を作成して返す
		/// その際、直前に作成したtemp dtxファイルを削除する
		/// </summary>
		/// <returns>temp dtxファイル名のフルパス</returns>
		internal string GetTempFileName()
		{
			string strTempFileName = Path.GetTempFileName();

			this.DeleteLastTempFile();
			this.lastTempFullPath = strTempFileName;
			return strTempFileName;
		}

		/// <summary>
		/// 直前のtemp dtxファイルを削除する
		/// 削除しようとしたtemp dtxファイルが見つからなかった場合も成功を返す
		/// </summary>
		/// <returns>true=削除成功, false=削除失敗</returns>
		private bool DeleteLastTempFile()
		{
			bool result = true;
			if ( lastTempFullPath != null )
			{
				try
				{
					File.Delete( lastTempFullPath );
				}
				catch 
				{
					result = false;
				}
			}
			return result;
		}
		
		/// <summary>
		/// Dispose処理。直前のtemp dtxファイルを削除するだけ。
		/// </summary>
		public virtual void Dispose()
		{
			DeleteLastTempFile();
			lastTempFullPath = "";
		}
	}
}
