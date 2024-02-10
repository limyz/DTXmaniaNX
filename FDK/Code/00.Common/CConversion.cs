using System;
using System.Collections.Generic;
using System.Text;

namespace FDK
{
	public class CConversion  // C変換
	{
		// プロパティ

		public static readonly string strBase16Characters = "0123456789ABCDEFabcdef";
		public static readonly string strBase62Characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
		

		// メソッド

		public static bool bONorOFF( char c )
		{
			return ( c != '0' );
		}

		public static double DegreeToRadian( double angle )
		{
			return ( ( Math.PI * angle ) / 180.0 );
		}
		public static double RadianToDegree( double angle )
		{
			return ( angle * 180.0 / Math.PI );
		}
		public static float DegreeToRadian( float angle )
		{
			return (float) DegreeToRadian( (double) angle );
		}
		public static float RadianToDegree( float angle )
		{
			return (float) RadianToDegree( (double) angle );
		}

		public static int nRoundToRange( int nValue, int nMin, int nMax)  // n値を範囲内に丸めて返す
		{
			if( nValue < nMin )
				return nMin;

			if( nValue > nMax )
				return nMax;

			return nValue;
		}
		public static int nGetNumberIfInRange( string strNumber, int nMin, int nMax, int nDefaultValueOnFailure)  // n値を文字列から取得して範囲内に丸めて返す
		{
			int num;
			if( ( int.TryParse( strNumber, out num ) && ( num >= nMin ) ) && ( num <= nMax ) )
				return num;

			return nDefaultValueOnFailure;
        }
        // #23568 2010.11.04 ikanick add
        public static int nRoundToRange(string str数値文字列, int n最小値, int n最大値, int n取得失敗時のデフォルト値)  // n値を文字列から取得して範囲内にちゃんと丸めて返す
		{
            // 1 と違って範囲外の場合ちゃんと丸めて返します。
            int num;
            if (int.TryParse(str数値文字列, out num)) {
                if ((num >= n最小値) && (num <= n最大値))
                    return num;
			    if ( num < n最小値 )
				    return n最小値;
			    if ( num > n最大値 )
				    return n最大値;
            }

            return n取得失敗時のデフォルト値;
        }
        // --------------------ここまで-------------------------/
		public static int nStringToInt( string str数値文字列, int n取得失敗時のデフォルト値 )
		{
			int num;
			if( !int.TryParse( str数値文字列, out num ) )
				num = n取得失敗時のデフォルト値;

			return num;
		}
		
		public static int nConvert2DigitHexadecimalStringToNumber( string strNum)  // n16進数2桁の文字列を数値に変換して返す
		{
			if( strNum.Length < 2 )
				return -1;

			int digit2 = strBase16Characters.IndexOf( strNum[ 0 ] );
			if( digit2 < 0 )
				return -1;

			if( digit2 >= 16 )
				digit2 -= (16 - 10);		// A,B,C... -> 1,2,3...

			int digit1 = strBase16Characters.IndexOf( strNum[ 1 ] );
			if( digit1 < 0 )
				return -1;

			if( digit1 >= 16 )
				digit1 -= (16 - 10);

			return digit2 * 16 + digit1;
		}
		public static int nConvert2DigitBase62StringToNumber( string strNum)  // n62進数2桁の文字列を数値に変換して返す
		{
			if( strNum.Length < 2 )
				return -1;

			int digit2 = strBase62Characters.IndexOf( strNum[ 0 ] );
			if( digit2 < 0 )
				return -1;

			if( digit2 >= 62 )
				digit2 -= (62 - 10);		// A,B,C... -> 1,2,3...

			int digit1 = strBase62Characters.IndexOf( strNum[ 1 ] );
			if( digit1 < 0 )
				return -1;

			if( digit1 >= 62 )
				digit1 -= (62 - 10);

			return digit2 * 62 + digit1;
		}
		public static int nConvert3DigitMeasureNumberToNumber( string strNum)  // n小節番号の文字列3桁を数値に変換して返す
		{
			if( strNum.Length >= 3 )
			{
				int digit3 = strBase62Characters.IndexOf( strNum[ 0 ] );
				if( digit3 < 0 )
					return -1;

				if( digit3 >= 62 )									// 3桁目は36進数
					digit3 -= (62 - 10);

				int digit2 = strBase16Characters.IndexOf( strNum[ 1 ] );	// 2桁目は10進数
				if( ( digit2 < 0 ) || ( digit2 > 9 ) )
					return -1;

				int digit1 = strBase16Characters.IndexOf( strNum[ 2 ] );	// 1桁目も10進数
				if( ( digit1 >= 0 ) && ( digit1 <= 9 ) )
					return digit3 * 100 + digit2 * 10 + digit1;
			}
			return -1;
		}
		
		public static string strConvertNumberTo3DigitMeasureNumber( int num)  // str小節番号を文字列3桁に変換して返す
		{
			if( ( num < 0 ) || ( num >= 3600 ) )	// 3600 == Z99 + 1
				return "000";

			int digit4 = num / 100;
			int digit2 = ( num % 100 ) / 10;
			int digit1 = ( num % 100 ) % 10;
			char ch3 = strBase62Characters[ digit4 ];
			char ch2 = strBase16Characters[ digit2 ];
			char ch1 = strBase16Characters[ digit1 ];
			return ( ch3.ToString() + ch2.ToString() + ch1.ToString() );
		}
		public static string strConvertNumberTo2DigitHexadecimalString( int num)  // str数値を16進数2桁に変換して返す
		{
			if( ( num < 0 ) || ( num >= 0x100 ) )
				return "00";

			char ch2 = strBase16Characters[ num / 0x10 ];
			char ch1 = strBase16Characters[ num % 0x10 ];
			return ( ch2.ToString() + ch1.ToString() );
		}
		public static string strConvertNumberTo2DigitBase62String( int num)  // str数値を62進数2桁に変換して返す
		{
			if( ( num < 0 ) || ( num >= 62 * 62 ) )
				return "00";

			char ch2 = strBase62Characters[ num / 62 ];
			char ch1 = strBase62Characters[ num % 62 ];
			return ( ch2.ToString() + ch1.ToString() );
		}

		#region [ private ]
		//-----------------

		// private コンストラクタでインスタンス生成を禁止する。
		private CConversion()
		{
		}
		//-----------------
		#endregion
	} 
}
