using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using DTXCreator.Properties;
using DTXCreator.Score;
using DTXCreator.UndoRedo;
using DTXCreator.WAV_BMP_AVI;
using FDK;
using System.Diagnostics;

namespace DTXCreator
{
	internal class CDTXInputOutput  // CDTX入出力
	{
		internal CDTXInputOutput( CMainForm mf )
		{
			this._Form = mf;
		}
		public void tDTX出力( StreamWriter sw )
		{
			this.tDTX出力( sw, false );
		}
		public void tDTX出力( StreamWriter sw, bool bBGMのみ出力 )
		{
			sw.WriteLine( "; Created by DTXCreator " + Resources.DTXC_VERSION );
			this.tDTX出力_タイトルと製作者とコメントその他( sw );
			this.tDTX出力_自由入力欄( sw );
			this.tDTX出力_WAVリスト( sw, bBGMのみ出力 );
			this.tDTX出力_BMPリスト( sw );
			this.tDTX出力_AVIリスト( sw );
			this.tDTX出力_小節長倍率( sw );
			this.tDTX出力_BPxリスト( sw );
			this.tDTX出力_全チップ( sw );
			sw.WriteLine();
			this.tDTX出力_レーン割付チップ( sw );
			this.tDTX出力_WAVリスト色設定( sw );
			this.tDTX出力_BMPリスト色設定( sw );
			this.tDTX出力_AVIリスト色設定( sw );
			this.tDTX出力_チップパレット( sw );
		}
		public void tDTX入力( E種別 e種別, ref string str全入力文字列 )
		{
			this._Form.hScrollBarDLEVEL.Value = 0;
			this._Form.textBoxDLEVEL.Text = "0";

			this._Form.hScrollBarGLEVEL.Value = 0;
			this._Form.textBoxGLEVEL.Text = "0";

			this._Form.hScrollBarBLEVEL.Value = 0;
			this._Form.textBoxBLEVEL.Text = "0";

			this.e種別 = e種別;
			if( str全入力文字列.Length != 0 )
			{
				this.dic小節長倍率 = new Dictionary<int, float>();
				this.listチップパレット = new List<int>();
				this.listBGMWAV番号 = new List<int>();											// #26775 2011.11.21 yyagi
				this.nLastBarConverted = -1;
				this.eDTXbgmChs = DTXbgmChs.GetEnumerator();
				this._Form.listViewWAVリスト.BeginUpdate();
				this._Form.listViewBMPリスト.BeginUpdate();
				this._Form.listViewAVIリスト.BeginUpdate();
				str全入力文字列 = str全入力文字列.Replace( Environment.NewLine, "\n" );
				str全入力文字列 = str全入力文字列.Replace( '\t', ' ' );
				StringBuilder builder = new StringBuilder();
				CharEnumerator ce = str全入力文字列.GetEnumerator();
				if( ce.MoveNext() )
				{
					do
					{
						if( !this.tDTX入力_空白と改行をスキップする( ref ce ) )
						{
							break;
						}
						if( ce.Current == '#' )
						{
							if( ce.MoveNext() )
							{
								StringBuilder builder2 = new StringBuilder( 0x20 );
								if( this.tDTX入力_コマンド文字列を抜き出す( ref ce, ref builder2 ) )
								{
									StringBuilder builder3 = new StringBuilder( 0x400 );
									if( this.tDTX入力_パラメータ文字列を抜き出す( ref ce, ref builder3 ) )
									{
										StringBuilder builder4 = new StringBuilder( 0x400 );
										if( this.tDTX入力_コメント文字列を抜き出す( ref ce, ref builder4 ) )
										{
											if( !this.tDTX入力_行解析( ref builder2, ref builder3, ref builder4 ) )
											{
												builder.Append( string.Concat( new object[] { "#", builder2, ": ", builder3 } ) );
												if( builder4.Length > 0 )
												{
													builder.Append( "\t;" + builder4 );
												}
												builder.Append( Environment.NewLine );
											}
											continue;
										}
									}
								}
							}
							break;
						}
					}
					while( this.tDTX入力_コメントをスキップする( ref ce ) );
					CUndoRedoManager.bUndoRedoした直後 = true;
					this._Form.textBox自由入力欄.Text = this._Form.textBox自由入力欄.Text + builder.ToString();
					this.tDTX入力_小節内のチップリストを発声位置でソートする();
					this.tDTX入力_小節長倍率配列を昇順ソート済みの小節リストに適用する();
					this.tDTX入力_BPMチップにBPx数値をバインドする();
					this.tDTX入力_キャッシュからListViewを一括構築する();
					this.tDTX入力_チップパレットのListViewを一括構築する();
					if( this.listBGMWAV番号.Count > 0 )							// #26775 2011.11.21 yyagi
					{
						foreach ( int nBGMWAV番号 in listBGMWAV番号 )			// #26775 2011.11.21 yyagi
						{
							this._Form.mgrWAVリスト管理者.tWAVをキャッシュから検索して返す_なければ新規生成する( nBGMWAV番号 ).bBGMとして使用 = true;
						}
					}
					this._Form.listViewWAVリスト.EndUpdate();
					this._Form.listViewBMPリスト.EndUpdate();
					this._Form.listViewAVIリスト.EndUpdate();
				}
			}
		}

		public enum E種別
		{
			DTX,
			GDA,
			G2D,
			BMS,
			BME
		}

		#region [ private ]
		//-----------------
		private CMainForm _Form;
		private int[] arr素数リスト = new int[] {
			2, 3, 5, 7, 11, 13, 0x11, 0x13, 0x17, 0x1d, 0x1f, 0x25, 0x29, 0x2b, 0x2f, 0x35, 
			0x3b, 0x3d, 0x43, 0x47, 0x49, 0x4f, 0x53, 0x59, 0x61, 0x65, 0x67, 0x6b, 0x6d, 0x71, 0x7f, 0x83, 
			0x89, 0x8b, 0x95, 0x97, 0x9d, 0xa3, 0xa7, 0xad, 0xb3, 0xb5, 0xbf, 0xc1, 0xc5, 0xc7, 0xd3, 0xdf, 
			0xe3, 0xe5, 0xe9, 0xef, 0xf1, 0xfb, 0x101, 0x107, 0x10d, 0x10f, 0x115, 0x119, 0x11b, 0x125, 0x133, 0x137, 
			0x139, 0x13d, 0x14b, 0x151, 0x15b, 0x15d, 0x161, 0x167, 0x16f, 0x175, 0x17b, 0x17f
		};
#region [ #25990; for BMS/BME to DTX conversion ]
		// #25990 2011.8.12 yyagi: DTXのBGM用ch群(正確には効果音用ch群)
		private readonly int[] DTXbgmChs = new int[] {
			      0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69,
			0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79,
			0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89,
			0x90, 0x91, 0x92
		};
		// #25990 2011.8.12 yyagi: BMS/BME→DTX チャネル変換テーブル(BGMとキーを除いて、右から左にそのまま変換)
		private readonly int[] BMSgeneralChToDTXgeneralCh = {
			0x00, -1,   0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,
			0x10, -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,   -1,
			0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F,
			0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
			0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F,
			0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F,
			0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F,
			0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F,
			0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F,
			0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9C, 0x9D, 0x9E, 0x9F,
			0xA0, 0xA1, 0xA2, 0xA3, 0xA4, 0xA5, 0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xAB, 0xAC, 0xAD, 0xAE, 0xAF,
			0xB0, 0xB1, 0xB2, 0xB3, 0xB4, 0xB5, 0xB6, 0xB7, 0xB8, 0xB9, 0xBA, 0xBB, 0xBC, 0xBD, 0xBE, 0xBF,
			0xC0, 0xC1, 0xC2, 0xC3, 0xC4, 0xC5, 0xC6, 0xC7, 0xC8, 0xC9, 0xCA, 0xCB, 0xCC, 0xCD, 0xCE, 0xCF,
			0xD0, 0xD1, 0xD2, 0xD3, 0xD4, 0xD5, 0xD6, 0xD7, 0xD8, 0xD9, 0xDA, 0xDB, 0xDC, 0xDD, 0xDE, 0xDF,
			0xE0, 0xE1, 0xE2, 0xE3, 0xE4, 0xE5, 0xE6, 0xE7, 0xE8, 0xE9, 0xEA, 0xEB, 0xEC, 0xED, 0xEE, 0xEF,
			0xF0, 0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF
		};
		// #25990 2011.8.12 yyagi: BMS→DTXの、キー関係の変換表 (5鍵ならHH～LTとCYを使用)
		private readonly int[] BMSkeyChToDTXdrumsCh = {
		//	1key	2key,	3key,	4key,	5key,	scr,	free
			0x11,	0x12,	0x13,	0x14,	0x15,	0x16,	0x53
		//	HC,		SD,		BD,		HT,		LT,		CY,		FI
		};
		// #25990 2011.8.12 yyagi: BME→DTXの、キー関係の変換表 (7鍵ならLC～FTとCYを使用)
		private readonly int[] BMEkeyChToDTXdrumsCh = {
		//	1key	2key,	3key,	4key,	5key,	scr,	free,	6key,	7key
			0x1A,	0x11,	0x12,	0x13,	0x14,	0x16,	0x53,	0x15,	0x17
		//	LC,		HC,		SD,		BD,		HT,		CY,		FI,		LT,		FT
		};
		private int nLastBarConverted = -1;	// #25990 2011.8.12 yyagi BMS/BME→DTX変換用
		private IEnumerator eDTXbgmChs;			// #25990 2011.8.12 yyagi BMS/BME→DTX変換用
#endregion
		private Dictionary<int, float> dic小節長倍率;
		private E種別 e種別;
		private List<int> listチップパレット;
		private List<int> listBGMWAV番号 = null;			// #26775 2011.11.21 yyagi
		
		private void tDTX入力_BPMチップにBPx数値をバインドする()
		{
			foreach( KeyValuePair<int, CMeasure> pair in this._Form.mgr譜面管理者.dic小節 )
			{
				CMeasure c小節 = pair.Value;
				for( int i = 0; i < c小節.listチップ.Count; i++ )
				{
					CChip cチップ = c小節.listチップ[ i ];
					float num2 = 0f;
					if( ( cチップ.nチャンネル番号00toFF == 8 ) && this._Form.mgr譜面管理者.dicBPx.TryGetValue( cチップ.n値_整数1to1295, out num2 ) )
					{
						cチップ.f値_浮動小数 = num2;
					}
					if( cチップ.nチャンネル番号00toFF == 3 )
					{
						cチップ.nチャンネル番号00toFF = 8;
						cチップ.f値_浮動小数 = cチップ.n値_整数1to1295;
						cチップ.b裏 = false;
						for( int j = 1; j <= 36 * 36 - 1; j++ )
						{
							if( !this._Form.mgr譜面管理者.dicBPx.ContainsKey( j ) )
							{
								this._Form.mgr譜面管理者.dicBPx.Add( j, cチップ.f値_浮動小数 );
								cチップ.n値_整数1to1295 = j;
								break;
							}
						}
					}
				}
			}
		}
		private void tDTX入力_キャッシュからListViewを一括構築する()
		{
			for( int i = 1; i <= 36 * 36 - 1; i++ )
			{
				CWAV cwav = this._Form.mgrWAVリスト管理者.tWAVをキャッシュから検索して返す( i );
				if( cwav != null )
				{
					cwav.tコピーto( this._Form.listViewWAVリスト.Items[ i - 1 ] );
				}
				CBMP cbmp = this._Form.mgrBMPリスト管理者.tBMPをキャッシュから検索して返す( i );
				if( cbmp != null )
				{
					cbmp.tコピーto( this._Form.listViewBMPリスト.Items[ i - 1 ] );
				}
				CAVI cavi = this._Form.mgrAVIリスト管理者.tAVIをキャッシュから検索して返す( i );
				if( cavi != null )
				{
					cavi.tコピーto( this._Form.listViewAVIリスト.Items[ i - 1 ] );
				}
			}
		}
		private bool tDTX入力_コマンド文字列を抜き出す( ref CharEnumerator ce, ref StringBuilder sb文字列 )
		{
			if( this.tDTX入力_空白をスキップする( ref ce ) )
			{
				while( ( ( ce.Current != ':' ) && ( ce.Current != ' ' ) ) && ( ( ce.Current != ';' ) && ( ce.Current != '\n' ) ) )
				{
					sb文字列.Append( ce.Current );
					if( !ce.MoveNext() )
					{
						return false;
					}
				}
				if( ce.Current == ':' )
				{
					if( !ce.MoveNext() )
					{
						return false;
					}
					if( !this.tDTX入力_空白をスキップする( ref ce ) )
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
		private bool tDTX入力_コメントをスキップする( ref CharEnumerator ce )
		{
			while( ce.Current != '\n' )
			{
				if( !ce.MoveNext() )
				{
					return false;
				}
			}
			return ce.MoveNext();
		}
		private bool tDTX入力_コメント文字列を抜き出す( ref CharEnumerator ce, ref StringBuilder sb文字列 )
		{
			if( ce.Current != ';' )
			{
				return true;
			}
			if( ce.MoveNext() )
			{
				while( ce.Current != '\n' )
				{
					sb文字列.Append( ce.Current );
					if( !ce.MoveNext() )
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
		private void tDTX入力_チップパレットのListViewを一括構築する()
		{
			for( int i = 0; i < ( this.listチップパレット.Count / 2 ); i += 2 )
			{
				int num2 = this.listチップパレット[ i * 2 ];
				int num3 = this.listチップパレット[ ( i * 2 ) + 1 ];
				string[] items = new string[ 3 ];
				switch( num2 )
				{
					case 0:
						{
							CWAV cwav = this._Form.mgrWAVリスト管理者.tWAVをキャッシュから検索して返す( num3 );
							if( cwav != null )
							{
								items[ 0 ] = cwav.strラベル名;
								items[ 1 ] = CConversion.strConvertNumberTo2DigitBase36String( num3 );
								items[ 2 ] = cwav.strファイル名;
								ListViewItem item = new ListViewItem( items );
								item.ImageIndex = num2;
								item.ForeColor = cwav.col文字色;
								item.BackColor = cwav.col背景色;
								this._Form.dlgチップパレット.listViewチップリスト.Items.Add( item );
							}
							break;
						}
					case 1:
						{
							CBMP cbmp = this._Form.mgrBMPリスト管理者.tBMPをキャッシュから検索して返す( num3 );
							if( cbmp != null )
							{
								items[ 0 ] = cbmp.strラベル名;
								items[ 1 ] = CConversion.strConvertNumberTo2DigitBase36String( num3 );
								items[ 2 ] = cbmp.strファイル名;
								ListViewItem item2 = new ListViewItem( items );
								item2.ImageIndex = num2;
								item2.ForeColor = cbmp.col文字色;
								item2.BackColor = cbmp.col背景色;
								this._Form.dlgチップパレット.listViewチップリスト.Items.Add( item2 );
							}
							break;
						}
					case 2:
						{
							CAVI cavi = this._Form.mgrAVIリスト管理者.tAVIをキャッシュから検索して返す( num3 );
							if( cavi != null )
							{
								items[ 0 ] = cavi.strラベル名;
								items[ 1 ] = CConversion.strConvertNumberTo2DigitBase36String( num3 );
								items[ 2 ] = cavi.strファイル名;
								ListViewItem item3 = new ListViewItem( items );
								item3.ImageIndex = num2;
								item3.ForeColor = cavi.col文字色;
								item3.BackColor = cavi.col背景色;
								this._Form.dlgチップパレット.listViewチップリスト.Items.Add( item3 );
							}
							break;
						}
				}
			}
		}
		private bool tDTX入力_パラメータ文字列を抜き出す( ref CharEnumerator ce, ref StringBuilder sb文字列 )
		{
			if( this.tDTX入力_空白をスキップする( ref ce ) )
			{
				while( ( ce.Current != '\n' ) && ( ce.Current != ';' ) )
				{
					sb文字列.Append( ce.Current );
					if( !ce.MoveNext() )
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
		private bool tDTX入力_空白と改行をスキップする( ref CharEnumerator ce )
		{
			while( ( ce.Current == ' ' ) || ( ce.Current == '\n' ) )
			{
				if( !ce.MoveNext() )
				{
					return false;
				}
			}
			return true;
		}
		private bool tDTX入力_空白をスキップする( ref CharEnumerator ce )
		{
			while( ce.Current == ' ' )
			{
				if( !ce.MoveNext() )
				{
					return false;
				}
			}
			return true;
		}
		private bool tDTX入力_行解析( ref StringBuilder sbコマンド, ref StringBuilder sbパラメータ, ref StringBuilder sbコメント )
		{
			string str = sbコマンド.ToString();
			string str2 = sbパラメータ.ToString().Trim();
			string str3 = sbコメント.ToString();
			return ( this.tDTX入力_行解析_TITLE_ARTIST_COMMENT_その他( str, str2, str3 ) || ( this.tDTX入力_行解析_WAVVOL_VOLUME( str, str2, str3 ) || ( this.tDTX入力_行解析_WAVPAN_PAN( str, str2, str3 ) || ( this.tDTX入力_行解析_WAV( str, str2, str3 ) || ( this.tDTX入力_行解析_BGMWAV( str, str2, str3 ) || ( this.tDTX入力_行解析_BMPTEX( str, str2, str3 ) || ( this.tDTX入力_行解析_BMP( str, str2, str3 ) || ( this.tDTX入力_行解析_AVI_AVIPAN( str, str2, str3 ) || ( this.tDTX入力_行解析_BPx( str, str2, str3 ) || ( this.tDTX入力_行解析_DTXC_LANEBINDEDCHIP( str, str2, str3 ) || ( this.tDTX入力_行解析_DTXC_WAVFORECOLOR( str, str2, str3 ) || ( this.tDTX入力_行解析_DTXC_WAVBACKCOLOR( str, str2, str3 ) || ( this.tDTX入力_行解析_DTXC_BMPFORECOLOR( str, str2, str3 ) || ( this.tDTX入力_行解析_DTXC_BMPBACKCOLOR( str, str2, str3 ) || ( this.tDTX入力_行解析_DTXC_AVIFORECOLOR( str, str2, str3 ) || ( this.tDTX入力_行解析_DTXC_AVIBACKCOLOR( str, str2, str3 ) || ( this.tDTX入力_行解析_DTXC_CHIPPALETTE( str, str2, str3 ) || this.tDTX入力_行解析_チャンネル( str, str2, str3 ) ) ) ) ) ) ) ) ) ) ) ) ) ) ) ) ) );
		}
		private bool tDTX入力_行解析_AVI_AVIPAN( string strコマンド, string strパラメータ, string strコメント )
		{
			if( !strコマンド.StartsWith( "AVIPAN", StringComparison.OrdinalIgnoreCase ) && strコマンド.StartsWith( "AVI", StringComparison.OrdinalIgnoreCase ) )
			{
				strコマンド = strコマンド.Substring( 3 );
			}
			else
			{
				return false;
			}
			int num = CConversion.nConvert2DigitBase36StringToNumber( strコマンド.Substring( 0, 2 ) );
			if( ( num < 1 ) || ( num > 36 * 36 - 1 ) )
			{
				return false;
			}
			CAVI cavi = this._Form.mgrAVIリスト管理者.tAVIをキャッシュから検索して返す_なければ新規生成する( num );
			cavi.strラベル名 = strコメント;
			cavi.strファイル名 = strパラメータ;
			return true;
		}
		private bool tDTX入力_行解析_BGMWAV( string strコマンド, string strパラメータ, string strコメント )
		{
			if( strコマンド.StartsWith( "bgmwav", StringComparison.OrdinalIgnoreCase ) )
			{
				strコマンド = strコマンド.Substring( 6 );
			}
			else
			{
				return false;
			}
			int num = CConversion.nConvert2DigitBase36StringToNumber( strパラメータ.Substring( 0, 2 ) );
			if( ( num < 1 ) || ( num > 36 * 36 - 1 ) )
			{
				return false;
			}
			this.listBGMWAV番号.Add( num );
			return true;
		}
		private bool tDTX入力_行解析_BMP( string strコマンド, string strパラメータ, string strコメント )
		{
			if( ( strコマンド.Length > 3 ) && strコマンド.StartsWith( "BMP", StringComparison.OrdinalIgnoreCase ) )
			{
				strコマンド = strコマンド.Substring( 3 );
			}
			else
			{
				return false;
			}
			int num = CConversion.nConvert2DigitBase36StringToNumber( strコマンド.Substring( 0, 2 ) );
			if( ( num < 1 ) || ( num > 36 * 36 - 1 ) )
			{
				return false;
			}
			CBMP cbmp = this._Form.mgrBMPリスト管理者.tBMPをキャッシュから検索して返す_なければ新規生成する( num );
			cbmp.strラベル名 = strコメント;
			cbmp.strファイル名 = strパラメータ;
			cbmp.bテクスチャ = false;
			return true;
		}
		private bool tDTX入力_行解析_BMPTEX( string strコマンド, string strパラメータ, string strコメント )
		{
			if( strコマンド.StartsWith( "BMPTEX", StringComparison.OrdinalIgnoreCase ) )
			{
				strコマンド = strコマンド.Substring( 6 );
			}
			else
			{
				return false;
			}
			int num = CConversion.nConvert2DigitBase36StringToNumber( strコマンド.Substring( 0, 2 ) );
			if( ( num < 1 ) || ( num > 36 * 36 - 1 ) )
			{
				return false;
			}
			CBMP cbmp = this._Form.mgrBMPリスト管理者.tBMPをキャッシュから検索して返す_なければ新規生成する( num );
			cbmp.strラベル名 = strコメント;
			cbmp.strファイル名 = strパラメータ;
			cbmp.bテクスチャ = true;
			return true;
		}
		private bool tDTX入力_行解析_BPx( string strコマンド, string strパラメータ, string strコメント )
		{
			if( strコマンド.StartsWith( "BPM", StringComparison.OrdinalIgnoreCase ) )
			{
				strコマンド = strコマンド.Substring( 3 );
			}
			else
			{
				return false;
			}
			int key = CConversion.nConvert2DigitBase36StringToNumber( strコマンド.Substring( 0, 2 ) );
			if( ( key < 1 ) || ( key > 36 * 36 - 1 ) )
			{
				return false;
			}
			decimal result = 0;
			if( ( !this.TryParse( strパラメータ, out result ) || ( result < 0 ) ) || ( result > 1000 ) )		// #23880 2011.1.6 yyagi
			{
				return false;
			}
			this._Form.mgr譜面管理者.dicBPx.Add(key, (float)result);
			return true;
		}
		private bool tDTX入力_行解析_DTXC_AVIBACKCOLOR( string strコマンド, string strパラメータ, string strコメント )
		{
			int nChipNo;
			if( !strコマンド.Equals( "DTXC_AVIBACKCOLOR", StringComparison.OrdinalIgnoreCase ) )
			{
				return false;
			}
			string[] strArray = strパラメータ.Split( new char[] { ' ', '\t' } );
			if( strArray.Length < 2 )
			{
				return false;
			}
			if( !int.TryParse( strArray[ 0 ], out nChipNo ) )
			{
				return false;
			}
			if( ( nChipNo < 0 ) || ( nChipNo > 36 * 36 - 2 ) )
			{
				return false;
			}
			Color color = ColorTranslator.FromHtml( strArray[ 1 ] );
			if( ( nChipNo >= 0 ) && ( nChipNo <= 36 * 36 - 2 ) )
			{
				this._Form.mgrAVIリスト管理者.tAVIをキャッシュから検索して返す_なければ新規生成する( nChipNo + 1 ).col背景色 = color;
			}
			return true;
		}
		private bool tDTX入力_行解析_DTXC_AVIFORECOLOR( string strコマンド, string strパラメータ, string strコメント )
		{
			int nChipNo;
			if( !strコマンド.Equals( "DTXC_AVIFORECOLOR", StringComparison.OrdinalIgnoreCase ) )
			{
				return false;
			}
			string[] strArray = strパラメータ.Split( new char[] { ' ', '\t' } );
			if( strArray.Length < 2 )
			{
				return false;
			}
			if( !int.TryParse( strArray[ 0 ], out nChipNo ) )
			{
				return false;
			}
			if( ( nChipNo < 0 ) || ( nChipNo > 36 * 36 - 2 ) )
			{
				return false;
			}
			Color color = ColorTranslator.FromHtml( strArray[ 1 ] );
			if( ( nChipNo >= 0 ) && ( nChipNo <= 36 * 36 - 2 ) )
			{
				this._Form.mgrAVIリスト管理者.tAVIをキャッシュから検索して返す_なければ新規生成する( nChipNo + 1 ).col文字色 = color;
			}
			return true;
		}
		private bool tDTX入力_行解析_DTXC_BMPBACKCOLOR( string strコマンド, string strパラメータ, string strコメント )
		{
			int nChipNo;
			if( !strコマンド.Equals( "DTXC_BMPBACKCOLOR", StringComparison.OrdinalIgnoreCase ) )
			{
				return false;
			}
			string[] strArray = strパラメータ.Split( new char[] { ' ', '\t' } );
			if( strArray.Length < 2 )
			{
				return false;
			}
			if( !int.TryParse( strArray[ 0 ], out nChipNo ) )
			{
				return false;
			}
			if( ( nChipNo < 0 ) || ( nChipNo > 36 * 36 - 2 ) )
			{
				return false;
			}
			Color color = ColorTranslator.FromHtml( strArray[ 1 ] );
			if( ( nChipNo >= 0 ) && ( nChipNo <= 36 * 36 - 2 ) )
			{
				this._Form.mgrBMPリスト管理者.tBMPをキャッシュから検索して返す_なければ新規生成する( nChipNo + 1 ).col背景色 = color;
			}
			return true;
		}
		private bool tDTX入力_行解析_DTXC_BMPFORECOLOR( string strコマンド, string strパラメータ, string strコメント )
		{
			int nChipNo;
			if( !strコマンド.Equals( "DTXC_BMPFORECOLOR", StringComparison.OrdinalIgnoreCase ) )
			{
				return false;
			}
			string[] strArray = strパラメータ.Split( new char[] { ' ', '\t' } );
			if( strArray.Length < 2 )
			{
				return false;
			}
			if( !int.TryParse( strArray[ 0 ], out nChipNo ) )
			{
				return false;
			}
			if( ( nChipNo < 0 ) || ( nChipNo > 36 * 36 - 2 ) )
			{
				return false;
			}
			Color color = ColorTranslator.FromHtml( strArray[ 1 ] );
			if( ( nChipNo >= 0 ) && ( nChipNo <= 36 * 36 - 2 ) )
			{
				this._Form.mgrBMPリスト管理者.tBMPをキャッシュから検索して返す_なければ新規生成する( nChipNo + 1 ).col文字色 = color;
			}
			return true;
		}
		private bool tDTX入力_行解析_DTXC_CHIPPALETTE( string strコマンド, string strパラメータ, string strコメント )
		{
			if( !strコマンド.Equals( "DTXC_CHIPPALETTE", StringComparison.OrdinalIgnoreCase ) )
			{
				return false;
			}
			foreach( string str in strパラメータ.Split( new char[] { ' ' } ) )
			{
				int num;
				string[] strArray2 = str.Split( new char[] { ',' } );
				if( ( ( strArray2.Length == 2 ) && int.TryParse( strArray2[ 0 ], out num ) ) && ( ( num >= 0 ) && ( num <= 2 ) ) )
				{
					int item = CConversion.nConvert2DigitBase36StringToNumber( strArray2[ 1 ] );
					if( ( item >= 1 ) && ( item <= 36 * 36 - 1 ) )
					{
						this.listチップパレット.Add( num );
						this.listチップパレット.Add( item );
					}
				}
			}
			return true;
		}
		private bool tDTX入力_行解析_DTXC_LANEBINDEDCHIP( string strコマンド, string strパラメータ, string strコメント )
		{
			if( strコマンド.Equals( "DTXC_LANEBINDEDCHIP", StringComparison.OrdinalIgnoreCase ) && ( strパラメータ.Length == 8 ) )
			{
				int nLaneNo;
				if( !int.TryParse( strパラメータ.Substring( 0, 2 ), out nLaneNo ) )
				{
					return false;
				}
				int nChipNoFore = CConversion.nConvert2DigitBase36StringToNumber( strパラメータ.Substring( 3, 2 ) );
				if( ( nChipNoFore < 0 ) || ( nChipNoFore > 36 * 36 - 1 ) )
				{
					return false;
				}
				int nChipNoBack = CConversion.nConvert2DigitBase36StringToNumber( strパラメータ.Substring( 6, 2 ) );
				if( ( nChipNoBack < 0 ) || ( nChipNoBack > 36 * 36 - 1 ) )
				{
					return false;
				}
				if( ( nLaneNo >= 0 ) && ( nLaneNo < this._Form.mgr譜面管理者.listレーン.Count ) )
				{
					if( nChipNoFore != 0 )
					{
						this._Form.mgr譜面管理者.listレーン[ nLaneNo ].nレーン割付チップ_表0or1to1295 = nChipNoFore;
					}
					if( nChipNoBack != 0 )
					{
						this._Form.mgr譜面管理者.listレーン[ nLaneNo ].nレーン割付チップ_裏0or1to1295 = nChipNoBack;
					}
					return true;
				}
			}
			return false;
		}
		private bool tDTX入力_行解析_DTXC_WAVBACKCOLOR( string strコマンド, string strパラメータ, string strコメント )
		{
			int nChipNo;
			if( !strコマンド.Equals( "DTXC_WAVBACKCOLOR", StringComparison.OrdinalIgnoreCase ) )
			{
				return false;
			}
			string[] strArray = strパラメータ.Split( new char[] { ' ', '\t' } );
			if( strArray.Length < 2 )
			{
				return false;
			}
			if( !int.TryParse( strArray[ 0 ], out nChipNo ) )
			{
				return false;
			}
			if( ( nChipNo < 0 ) || ( nChipNo > 36 * 36 - 2 ) )
			{
				return false;
			}
			Color color = ColorTranslator.FromHtml( strArray[ 1 ] );
			if( ( nChipNo >= 0 ) && ( nChipNo <= 36 * 36 - 2 ) )
			{
				this._Form.mgrWAVリスト管理者.tWAVをキャッシュから検索して返す_なければ新規生成する( nChipNo + 1 ).col背景色 = color;
			}
			return true;
		}
		private bool tDTX入力_行解析_DTXC_WAVFORECOLOR( string strコマンド, string strパラメータ, string strコメント )
		{
			int nChipNo;
			if( !strコマンド.Equals( "DTXC_WAVFORECOLOR", StringComparison.OrdinalIgnoreCase ) )
			{
				return false;
			}
			string[] strArray = strパラメータ.Split( new char[] { ' ', '\t' } );
			if( strArray.Length < 2 )
			{
				return false;
			}
			if( !int.TryParse( strArray[ 0 ], out nChipNo ) )
			{
				return false;
			}
			if( ( nChipNo < 0 ) || ( nChipNo > 36 * 36 - 2 ) )
			{
				return false;
			}
			Color color = ColorTranslator.FromHtml( strArray[ 1 ] );
			if( ( nChipNo >= 0 ) && ( nChipNo <= 36 * 36 - 2 ) )
			{
				this._Form.mgrWAVリスト管理者.tWAVをキャッシュから検索して返す_なければ新規生成する( nChipNo + 1 ).col文字色 = color;
			}
			return true;
		}
		private bool tDTX入力_行解析_TITLE_ARTIST_COMMENT_その他( string strコマンド, string strパラメータ, string strコメント )
		{
			if( strコマンド.Equals( "TITLE", StringComparison.OrdinalIgnoreCase ) )
			{
				CUndoRedoManager.bUndoRedoした直後 = true;
				this._Form.textBox曲名.Text = strパラメータ.Trim();
				return true;
			}
			if( strコマンド.Equals( "ARTIST", StringComparison.OrdinalIgnoreCase ) )
			{
				CUndoRedoManager.bUndoRedoした直後 = true;
				this._Form.textBox製作者.Text = strパラメータ.Trim();
				return true;
			}
			if( strコマンド.Equals( "COMMENT", StringComparison.OrdinalIgnoreCase ) )
			{
				CUndoRedoManager.bUndoRedoした直後 = true;
				this._Form.textBoxコメント.Text = strパラメータ.Trim();
				return true;
			}
			if( strコマンド.Equals( "PANEL", StringComparison.OrdinalIgnoreCase ) )
			{
				int dummyResult;								// #23885, #26007 2011.8.13 yyagi: not to confuse "#PANEL strings (panel)" and "#PANEL int (panpot of EL)"
				if ( !int.TryParse( strパラメータ, out dummyResult ) )	// 数値じゃないならPANELとみなす
				{
					CUndoRedoManager.bUndoRedoした直後 = true;
					this._Form.textBoxパネル.Text = strパラメータ.Trim();
					return true;
				}												// 数値なら、ここでは何もせず、後で#PANに拾ってもらう (PAN ELとみなす)
			}
			if( strコマンド.Equals( "PREVIEW", StringComparison.OrdinalIgnoreCase ) )
			{
				CUndoRedoManager.bUndoRedoした直後 = true;
				this._Form.textBoxPREVIEW.Text = strパラメータ.Trim();
				return true;
			}
			if( strコマンド.Equals( "PREIMAGE", StringComparison.OrdinalIgnoreCase ) )
			{
				CUndoRedoManager.bUndoRedoした直後 = true;
				this._Form.textBoxPREIMAGE.Text = strパラメータ.Trim();
				return true;
			}
			if( strコマンド.Equals( "STAGEFILE", StringComparison.OrdinalIgnoreCase ) )
			{
				CUndoRedoManager.bUndoRedoした直後 = true;
				this._Form.textBoxSTAGEFILE.Text = strパラメータ.Trim();
				return true;
			}
			if( strコマンド.Equals( "BACKGROUND", StringComparison.OrdinalIgnoreCase ) || strコマンド.Equals( "WALL", StringComparison.OrdinalIgnoreCase ) )
			{
				CUndoRedoManager.bUndoRedoした直後 = true;
				this._Form.textBoxBACKGROUND.Text = strパラメータ.Trim();
				return true;
			}
			if( strコマンド.Equals( "RESULTIMAGE", StringComparison.OrdinalIgnoreCase ) )
			{
				CUndoRedoManager.bUndoRedoした直後 = true;
				this._Form.textBoxRESULTIMAGE.Text = strパラメータ.Trim();
				return true;
			}
			if( strコマンド.Equals( "BPM", StringComparison.OrdinalIgnoreCase ) )
			{
				decimal dBpm;
				if( !this.TryParse( strパラメータ, out dBpm ) )		// #23880 2011.1.6 yyagi
				{
					dBpm = 120.0M;
				}
				CUndoRedoManager.bUndoRedoした直後 = true;
				this._Form.numericUpDownBPM.Value = dBpm;
				return true;
			}
			if( strコマンド.Equals( "DLEVEL", StringComparison.OrdinalIgnoreCase ) )
			{
				int nLevel;
				if( !int.TryParse( strパラメータ, out nLevel ) )
				{
					nLevel = 0;
				}
				else if( nLevel < 0 )
				{
					nLevel = 0;
				}
				else if( nLevel > 999 )
				{
					nLevel = 999;
				}
				CUndoRedoManager.bUndoRedoした直後 = true;
				this._Form.hScrollBarDLEVEL.Value = nLevel;
				CUndoRedoManager.bUndoRedoした直後 = true;
				this._Form.textBoxDLEVEL.Text = nLevel.ToString();
				return true;
			}
			if( strコマンド.Equals( "GLEVEL", StringComparison.OrdinalIgnoreCase ) )
			{
				int nLevel;
				if( !int.TryParse( strパラメータ, out nLevel ) )
				{
					nLevel = 0;
				}
				else if( nLevel < 0 )
				{
					nLevel = 0;
				}
				else if( nLevel > 999 )
				{
					nLevel = 999;
				}
				CUndoRedoManager.bUndoRedoした直後 = true;
				this._Form.hScrollBarGLEVEL.Value = nLevel;
				CUndoRedoManager.bUndoRedoした直後 = true;
				this._Form.textBoxGLEVEL.Text = nLevel.ToString();
				return true;
			}
			if( strコマンド.Equals( "BLEVEL", StringComparison.OrdinalIgnoreCase ) )
			{
				int nLevel;
				if( !int.TryParse( strパラメータ, out nLevel ) )
				{
					nLevel = 0;
				}
				else if( nLevel < 0 )
				{
					nLevel = 0;
				}
				else if( nLevel > 999 )
				{
					nLevel = 999;
				}
				CUndoRedoManager.bUndoRedoした直後 = true;
				this._Form.hScrollBarBLEVEL.Value = nLevel;
				CUndoRedoManager.bUndoRedoした直後 = true;
				this._Form.textBoxBLEVEL.Text = nLevel.ToString();
				return true;
			}
			if( strコマンド.Equals( "DTXVPLAYSPEED", StringComparison.OrdinalIgnoreCase ) )
			{
				decimal dPlaySpeed;
//				if ( !this.TryParse( strパラメータ, out num5 ) )		// #24790 2011.4.8 yyagi
				if ( !decimal.TryParse( strパラメータ, out dPlaySpeed ) )		// #24790 2011.4.8 yyagi
				{
					dPlaySpeed = 0M;
				}
				else if( dPlaySpeed < 0.5M )
				{
					dPlaySpeed = 0.5M;
				}
				else if( dPlaySpeed > 1.5M )
				{
					dPlaySpeed = 1.5M;
				}
				decimal dIndex = ( 1.5M - dPlaySpeed ) * 10M;
				int nIndex = (int) dIndex;
				if( nIndex < 0 )
				{
					nIndex = 0;
				}
				else if( nIndex > 10 )
				{
					nIndex = 10;
				}
				CUndoRedoManager.bUndoRedoした直後 = true;
				this._Form.toolStripComboBox演奏速度.SelectedIndex = nIndex;
				return true;
			} else {
				return false;
			}
		}
		private bool tDTX入力_行解析_WAV( string strコマンド, string strパラメータ, string strコメント )
		{
			if( strコマンド.StartsWith( "wav", StringComparison.OrdinalIgnoreCase ) )
			{
				strコマンド = strコマンド.Substring( 3 );
			}
			else
			{
				return false;
			}
			int nChipNo = CConversion.nConvert2DigitBase36StringToNumber( strコマンド.Substring( 0, 2 ) );
			if( ( nChipNo < 1 ) || ( nChipNo > 36 * 36 - 1 ) )
			{
				return false;
			}
			CWAV cwav = this._Form.mgrWAVリスト管理者.tWAVをキャッシュから検索して返す_なければ新規生成する( nChipNo );
			cwav.strラベル名 = strコメント;
			cwav.strファイル名 = strパラメータ;
			return true;
		}
		private bool tDTX入力_行解析_WAVPAN_PAN( string strコマンド, string strパラメータ, string strコメント )
		{
			int nPan;
			if( strコマンド.StartsWith( "WAVPAN", StringComparison.OrdinalIgnoreCase ) )
			{
				strコマンド = strコマンド.Substring( 6 );
			}
			else if( strコマンド.StartsWith( "PAN", StringComparison.OrdinalIgnoreCase ) )
			{
				strコマンド = strコマンド.Substring( 3 );
			}
			else
			{
				return false;
			}
			if( strコマンド.Length < 2 )
			{
				return false;
			}
			int nChipNo = CConversion.nConvert2DigitBase36StringToNumber( strコマンド.Substring( 0, 2 ) );
			if( int.TryParse( strパラメータ, out nPan ) )
			{
				if( nPan < -100 )
				{
					nPan = -100;
				}
				else if( nPan >= 100 )
				{
					nPan = 100;
				}
				this._Form.mgrWAVリスト管理者.tWAVをキャッシュから検索して返す_なければ新規生成する( nChipNo ).n位置_100to100 = nPan;
			}
			return true;
		}
		private bool tDTX入力_行解析_WAVVOL_VOLUME( string strコマンド, string strパラメータ, string strコメント )
		{
			int nVol;
			if( strコマンド.StartsWith( "WAVVOL", StringComparison.OrdinalIgnoreCase ) )
			{
				strコマンド = strコマンド.Substring( 6 );
			}
			else if( strコマンド.StartsWith( "VOLUME", StringComparison.OrdinalIgnoreCase ) )
			{
				strコマンド = strコマンド.Substring( 6 );
			}
			else
			{
				return false;
			}
			if( strコマンド.Length < 2 )
			{
				return false;
			}
			int nChipNo = CConversion.nConvert2DigitBase36StringToNumber( strコマンド.Substring( 0, 2 ) );
			if( int.TryParse( strパラメータ, out nVol ) )
			{
				if( nVol < 0 )
				{
					nVol = 0;
				}
				else if( nVol >= 100 )
				{
					nVol = 100;
				}
				this._Form.mgrWAVリスト管理者.tWAVをキャッシュから検索して返す_なければ新規生成する( nChipNo ).n音量0to100 = nVol;
			}
			return true;
		}
		private bool tDTX入力_行解析_チャンネル( string strコマンド, string strパラメータ, string strコメント )
		{
			int nBar, nCh;
			if( !this.tDTX入力_行解析_チャンネル_コマンドから小節番号とチャンネル番号を抜き出す( strコマンド, out nBar, out nCh ) )
			{
				return false;
			}
			if( nCh == 2 )
			{
				decimal dBarLength;
				if( !this.TryParse( strパラメータ, out dBarLength ) )	// #23880 2011.1.6 yyagi
				{
					dBarLength = 1m;
				}
				this.dic小節長倍率.Add( nBar, (float)dBarLength );
				return true;
			}
            if (( nCh >= 32 && nCh <= 39) || ( nCh >= 147 && nCh <= 159) || ( nCh >= 169 && nCh <= 175) || ( nCh >= 208 && nCh <= 211))
			{
				CMeasure c小節 = this.tDTX入力_行解析_チャンネル_小節番号に対応する小節を探すか新規に作って返す( nBar );
				int startIndex = 0;
				while( ( startIndex = strパラメータ.IndexOf( '_' ) ) != -1 )
				{
					strパラメータ = strパラメータ.Remove( startIndex, 1 );
				}
				int nChips = strパラメータ.Length / 2;
				for( int i = 0; i < nChips; i++ )
				{
					int nChipNo = CConversion.nConvert2DigitBase36StringToNumber( strパラメータ.Substring( i * 2, 2 ) );
                    if (nChipNo != 0)
                    {
                        int nLaneGtV = this._Form.mgr譜面管理者.nレーン名に対応するレーン番号を返す("GtV");
                        int nLaneGtR = this._Form.mgr譜面管理者.nレーン名に対応するレーン番号を返す("GtR");
                        int nLaneGtG = this._Form.mgr譜面管理者.nレーン名に対応するレーン番号を返す("GtG");
                        int nLaneGtB = this._Form.mgr譜面管理者.nレーン名に対応するレーン番号を返す("GtB");
                        int nLaneGtY = this._Form.mgr譜面管理者.nレーン名に対応するレーン番号を返す("GtY");
                        int nLaneGtP = this._Form.mgr譜面管理者.nレーン名に対応するレーン番号を返す("GtP");
                        CChip item = new CChip();
                        item.nレーン番号0to = nLaneGtV;
                        item.n位置grid = i;
                        item.n値_整数1to1295 = nChipNo;
                        item.n読み込み時の解像度 = nChips;
                        c小節.listチップ.Add(item);
                        switch (nCh)
                        {
                            case 0x20:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtR;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 2;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;

                            case 0x21:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtB;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;

                            case 0x22:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtG;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;

                            case 0x23:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtG;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtB;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;

                            case 0x24:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtR;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;

                            case 0x25:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtR;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtB;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;

                            case 0x26:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtR;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtG;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;

                            case 0x27:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtR;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtG;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtB;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                        }
                        switch ( nCh )
                        {
                            case 147:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtY;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 148:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtB;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtY;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 149:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtG;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtY;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 150:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtG;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtB;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtY;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 151:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtR;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtY;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 152:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtR;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtB;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtY;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 153:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtR;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtG;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtY;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 154:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtR;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtG;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtB;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtY;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 155:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtP;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 156:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtB;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtP;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 157:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtG;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtP;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 158:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtG;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtB;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtP;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 159:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtR;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtP;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 160:
                            case 161:
                            case 162:
                            case 163:
                            case 164:
                            case 165:
                            case 166:
                            case 167:
                            case 168:
                                break;
                            case 169:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtR;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtB;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtP;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 170:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtR;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtG;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtP;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 171:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtR;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtG;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtB;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtP;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 172:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtY;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtP;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 173:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtB;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtY;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtP;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 174:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtG;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtY;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtP;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                            case 175:
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtG;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtB;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtY;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                item = new CChip();
                                item.nレーン番号0to = nLaneGtP;
                                item.n位置grid = i;
                                item.n値_整数1to1295 = 1;
                                item.n読み込み時の解像度 = nChips;
                                c小節.listチップ.Add(item);
                                break;
                             case 208:
                                        item = new CChip();
                                        item.nレーン番号0to = nLaneGtR;
                                        item.n位置grid = i;
                                        item.n値_整数1to1295 = 1;
                                        item.n読み込み時の解像度 = nChips;
                                        c小節.listチップ.Add(item);
                                        item = new CChip();
                                        item.nレーン番号0to = nLaneGtY;
                                        item.n位置grid = i;
                                        item.n値_整数1to1295 = 1;
                                        item.n読み込み時の解像度 = nChips;
                                        c小節.listチップ.Add(item);
                                        item = new CChip();
                                        item.nレーン番号0to = nLaneGtP;
                                        item.n位置grid = i;
                                        item.n値_整数1to1295 = 1;
                                        item.n読み込み時の解像度 = nChips;
                                        c小節.listチップ.Add(item);
                                        break;
                                    case 209:
                                        item = new CChip();
                                        item.nレーン番号0to = nLaneGtR;
                                        item.n位置grid = i;
                                        item.n値_整数1to1295 = 1;
                                        item.n読み込み時の解像度 = nChips;
                                        c小節.listチップ.Add(item);
                                        item = new CChip();
                                        item.nレーン番号0to = nLaneGtB;
                                        item.n位置grid = i;
                                        item.n値_整数1to1295 = 1;
                                        item.n読み込み時の解像度 = nChips;
                                        c小節.listチップ.Add(item);
                                        item = new CChip();
                                        item.nレーン番号0to = nLaneGtY;
                                        item.n位置grid = i;
                                        item.n値_整数1to1295 = 1;
                                        item.n読み込み時の解像度 = nChips;
                                        c小節.listチップ.Add(item);
                                        item = new CChip();
                                        item.nレーン番号0to = nLaneGtP;
                                        item.n位置grid = i;
                                        item.n値_整数1to1295 = 1;
                                        item.n読み込み時の解像度 = nChips;
                                        c小節.listチップ.Add(item);
                                        break;
                                    case 210:
                                        item = new CChip();
                                        item.nレーン番号0to = nLaneGtR;
                                        item.n位置grid = i;
                                        item.n値_整数1to1295 = 1;
                                        item.n読み込み時の解像度 = nChips;
                                        c小節.listチップ.Add(item);
                                        item = new CChip();
                                        item.nレーン番号0to = nLaneGtG;
                                        item.n位置grid = i;
                                        item.n値_整数1to1295 = 1;
                                        item.n読み込み時の解像度 = nChips;
                                        c小節.listチップ.Add(item);
                                        item = new CChip();
                                        item.nレーン番号0to = nLaneGtY;
                                        item.n位置grid = i;
                                        item.n値_整数1to1295 = 1;
                                        item.n読み込み時の解像度 = nChips;
                                        c小節.listチップ.Add(item);
                                        item = new CChip();
                                        item.nレーン番号0to = nLaneGtP;
                                        item.n位置grid = i;
                                        item.n値_整数1to1295 = 1;
                                        item.n読み込み時の解像度 = nChips;
                                        c小節.listチップ.Add(item);
                                        break;
                                    case 211:
                                        item = new CChip();
                                        item.nレーン番号0to = nLaneGtR;
                                        item.n位置grid = i;
                                        item.n値_整数1to1295 = 1;
                                        item.n読み込み時の解像度 = nChips;
                                        c小節.listチップ.Add(item);
                                        item = new CChip();
                                        item.nレーン番号0to = nLaneGtG;
                                        item.n位置grid = i;
                                        item.n値_整数1to1295 = 1;
                                        item.n読み込み時の解像度 = nChips;
                                        c小節.listチップ.Add(item);
                                        item = new CChip();
                                        item.nレーン番号0to = nLaneGtB;
                                        item.n位置grid = i;
                                        item.n値_整数1to1295 = 1;
                                        item.n読み込み時の解像度 = nChips;
                                        c小節.listチップ.Add(item);
                                        item = new CChip();
                                        item.nレーン番号0to = nLaneGtY;
                                        item.n位置grid = i;
                                        item.n値_整数1to1295 = 1;
                                        item.n読み込み時の解像度 = nChips;
                                        c小節.listチップ.Add(item);
                                        item = new CChip();
                                        item.nレーン番号0to = nLaneGtP;
                                        item.n位置grid = i;
                                        item.n値_整数1to1295 = 1;
                                        item.n読み込み時の解像度 = nChips;
                                        c小節.listチップ.Add(item);
                                break;
                        }
                    }
				}
				return true;
			}
            if ((nCh >= 160 && nCh <= 167) || (nCh >= 197 && nCh <= 198) || (nCh >= 200 && nCh <= 207) || (nCh >= 218 && nCh <= 223) || (nCh >= 225 && nCh <= 232))
            {
				CMeasure c小節2 = this.tDTX入力_行解析_チャンネル_小節番号に対応する小節を探すか新規に作って返す( nBar );
				int num12 = 0;
				while( ( num12 = strパラメータ.IndexOf( '_' ) ) != -1 )
				{
					strパラメータ = strパラメータ.Remove( num12, 1 );
				}
				int nChips = strパラメータ.Length / 2;
				for( int j = 0; j < nChips; j++ )
				{
					int nChipNo = CConversion.nConvert2DigitBase36StringToNumber( strパラメータ.Substring( j * 2, 2 ) );
					if( nChipNo != 0 )
					{
						int nLaneBsV = this._Form.mgr譜面管理者.nレーン名に対応するレーン番号を返す( "BsV" );
						int nLaneBsR = this._Form.mgr譜面管理者.nレーン名に対応するレーン番号を返す( "BsR" );
						int nLaneBsG = this._Form.mgr譜面管理者.nレーン名に対応するレーン番号を返す( "BsG" );
						int nLaneBsB = this._Form.mgr譜面管理者.nレーン名に対応するレーン番号を返す( "BsB" );
                        int nLaneBsY = this._Form.mgr譜面管理者.nレーン名に対応するレーン番号を返す( "BsY" );
                        int nLaneBsP = this._Form.mgr譜面管理者.nレーン名に対応するレーン番号を返す( "BsP" );
                        CChip cチップ2 = new CChip();
						cチップ2.nレーン番号0to = nLaneBsV;
						cチップ2.n位置grid = j;
						cチップ2.n値_整数1to1295 = nChipNo;
						cチップ2.n読み込み時の解像度 = nChips;
						c小節2.listチップ.Add( cチップ2 );
                        switch (nCh)
                        {
                            case 160:
                                cチップ2 = new CChip();
                                cチップ2.nレーン番号0to = nLaneBsR;
                                cチップ2.n位置grid = j;
                                cチップ2.n値_整数1to1295 = 2;
                                cチップ2.n読み込み時の解像度 = nChips;
                                c小節2.listチップ.Add(cチップ2);
                                break;
                            case 161:
                                cチップ2 = new CChip();
                                cチップ2.nレーン番号0to = nLaneBsB;
                                cチップ2.n位置grid = j;
                                cチップ2.n値_整数1to1295 = 1;
                                cチップ2.n読み込み時の解像度 = nChips;
                                c小節2.listチップ.Add(cチップ2);
                                break;
                            case 162:
                                cチップ2 = new CChip();
                                cチップ2.nレーン番号0to = nLaneBsG;
                                cチップ2.n位置grid = j;
                                cチップ2.n値_整数1to1295 = 1;
                                cチップ2.n読み込み時の解像度 = nChips;
                                c小節2.listチップ.Add(cチップ2);
                                break;
                            case 163:
                                cチップ2 = new CChip();
                                cチップ2.nレーン番号0to = nLaneBsG;
                                cチップ2.n位置grid = j;
                                cチップ2.n値_整数1to1295 = 1;
                                cチップ2.n読み込み時の解像度 = nChips;
                                c小節2.listチップ.Add(cチップ2);
                                cチップ2 = new CChip();
                                cチップ2.nレーン番号0to = nLaneBsB;
                                cチップ2.n位置grid = j;
                                cチップ2.n値_整数1to1295 = 1;
                                cチップ2.n読み込み時の解像度 = nChips;
                                c小節2.listチップ.Add(cチップ2);
                                break;
                            case 164:
                                cチップ2 = new CChip();
                                cチップ2.nレーン番号0to = nLaneBsR;
                                cチップ2.n位置grid = j;
                                cチップ2.n値_整数1to1295 = 1;
                                cチップ2.n読み込み時の解像度 = nChips;
                                c小節2.listチップ.Add(cチップ2);
                                break;
                            case 165:
                                cチップ2 = new CChip();
                                cチップ2.nレーン番号0to = nLaneBsR;
                                cチップ2.n位置grid = j;
                                cチップ2.n値_整数1to1295 = 1;
                                cチップ2.n読み込み時の解像度 = nChips;
                                c小節2.listチップ.Add(cチップ2);
                                cチップ2 = new CChip();
                                cチップ2.nレーン番号0to = nLaneBsB;
                                cチップ2.n位置grid = j;
                                cチップ2.n値_整数1to1295 = 1;
                                cチップ2.n読み込み時の解像度 = nChips;
                                c小節2.listチップ.Add(cチップ2);
                                break;
                            case 166:
                                cチップ2 = new CChip();
                                cチップ2.nレーン番号0to = nLaneBsR;
                                cチップ2.n位置grid = j;
                                cチップ2.n値_整数1to1295 = 1;
                                cチップ2.n読み込み時の解像度 = nChips;
                                c小節2.listチップ.Add(cチップ2);
                                cチップ2 = new CChip();
                                cチップ2.nレーン番号0to = nLaneBsG;
                                cチップ2.n位置grid = j;
                                cチップ2.n値_整数1to1295 = 1;
                                cチップ2.n読み込み時の解像度 = nChips;
                                c小節2.listチップ.Add(cチップ2);
                                break;
                            case 167:
                                cチップ2 = new CChip();
                                cチップ2.nレーン番号0to = nLaneBsR;
                                cチップ2.n位置grid = j;
                                cチップ2.n値_整数1to1295 = 1;
                                cチップ2.n読み込み時の解像度 = nChips;
                                c小節2.listチップ.Add(cチップ2);
                                cチップ2 = new CChip();
                                cチップ2.nレーン番号0to = nLaneBsG;
                                cチップ2.n位置grid = j;
                                cチップ2.n値_整数1to1295 = 1;
                                cチップ2.n読み込み時の解像度 = nChips;
                                c小節2.listチップ.Add(cチップ2);
                                cチップ2 = new CChip();
                                cチップ2.nレーン番号0to = nLaneBsB;
                                cチップ2.n位置grid = j;
                                cチップ2.n値_整数1to1295 = 1;
                                cチップ2.n読み込み時の解像度 = nChips;
                                c小節2.listチップ.Add(cチップ2);
                                break;
                            default:
                                switch (nCh)
                                {
                                    case 197:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsY;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 198:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsB;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsY;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 200:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsG;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsY;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 201:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsG;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsB;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsY;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 202:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsR;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsY;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 203:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsR;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsB;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsY;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 204:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsR;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsG;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsY;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 205:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsR;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsG;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsB;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsY;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 206:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsP;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 207:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsB;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsP;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 218:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsG;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsP;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 219:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsG;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsB;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsP;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 220:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsR;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsP;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 221:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsR;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsB;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsP;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 222:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsR;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsG;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsP;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 223:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsR;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsG;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsB;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsP;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 225:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsP;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsY;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 226:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsB;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsP;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsY;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 227:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsG;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsP;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsY;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 228:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsG;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsB;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsP;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsY;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 229:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsR;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsP;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsY;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 230:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsR;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsB;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsP;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsY;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 231:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsR;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsG;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsP;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsY;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                    case 232:
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsR;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsG;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsB;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsY;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        cチップ2 = new CChip();
                                        cチップ2.nレーン番号0to = nLaneBsP;
                                        cチップ2.n位置grid = j;
                                        cチップ2.n値_整数1to1295 = 1;
                                        cチップ2.n読み込み時の解像度 = nChips;
                                        c小節2.listチップ.Add(cチップ2);
                                        break;
                                }
                                break;
                        }
                    }
                }
                return true;
            }
			int num20 = -1;
			bool flag = false;
			if( this.tDTX入力_行解析_チャンネル_チャンネルに該当するレーン番号を返す( nCh, out num20, out flag ) )
			{
				CMeasure c小節3 = this.tDTX入力_行解析_チャンネル_小節番号に対応する小節を探すか新規に作って返す( nBar );
				int nPosOf_ = 0;
				while( ( nPosOf_ = strパラメータ.IndexOf( '_' ) ) != -1 )
				{
					strパラメータ = strパラメータ.Remove( nPosOf_, 1 );
				}
				int nChips = strパラメータ.Length / 2;
				for( int i = 0; i < nChips; i++ )
				{
					int nChipNo = ( nCh == 3 ) ? CConversion.nConvert2DigitHexadecimalStringToNumber( strパラメータ.Substring( i * 2, 2 ) ) : CConversion.nConvert2DigitBase36StringToNumber( strパラメータ.Substring( i * 2, 2 ) );
					if( nChipNo > 0 )
					{
						CChip cチップ3 = new CChip();
						cチップ3.nチャンネル番号00toFF = nCh;
						cチップ3.nレーン番号0to = num20;
						cチップ3.n位置grid = i;
						cチップ3.n読み込み時の解像度 = nChips;
						cチップ3.n値_整数1to1295 = nChipNo;
						cチップ3.b裏 = flag;
						c小節3.listチップ.Add( cチップ3 );
					}
				}
				return true;
			}
			StringBuilder builder = new StringBuilder( 0x400 );
			builder.Append( "#" + CConversion.strConvertNumberTo3DigitMeasureNumber( nBar ) + CConversion.strConvertNumberTo2DigitHexadecimalString( nCh ) + ": " + strパラメータ );
			if( strコメント.Length > 0 )
			{
				builder.Append( " ;" + strコメント );
			}
			builder.Append( Environment.NewLine );
			CUndoRedoManager.bUndoRedoした直後 = true;
			this._Form.textBox自由入力欄.AppendText( builder.ToString() );
			return true;
		}
		private int tDTX入力_行解析_チャンネル_GDAチャンネル文字列２桁をチャンネル番号にして返す( string strチャンネル文字列２桁 )
		{
			if( strチャンネル文字列２桁.Length == 2 )
			{
				switch( strチャンネル文字列２桁.ToUpper() )
				{
					case "TC":
						return 3;

					case "BL":
						return 2;

					case "GS":
						return 0x29;

					case "DS":
						return 0x30;

					case "FI":
						return 0x53;

					case "HH":
						return 0x11;

					case "SD":
						return 0x12;

					case "BD":
						return 0x13;

					case "HT":
						return 0x14;

					case "LT":
						return 0x15;

					case "CY":
						return 0x16;

					case "G1":
						return 0x21;

					case "G2":
						return 0x22;

					case "G3":
						return 0x23;

					case "G4":
						return 0x24;

					case "G5":
						return 0x25;

					case "G6":
						return 0x26;

					case "G7":
						return 0x27;

					case "GW":
						return 0x28;

					case "01":
						return 0x61;

					case "02":
						return 0x62;

					case "03":
						return 0x63;

					case "04":
						return 0x64;

					case "05":
						return 0x65;

					case "06":
						return 0x66;

					case "07":
						return 0x67;

					case "08":
						return 0x68;

					case "09":
						return 0x69;

					case "0A":
						return 0x70;

					case "0B":
						return 0x71;

					case "0C":
						return 0x72;

					case "0D":
						return 0x73;

					case "0E":
						return 0x74;

					case "0F":
						return 0x75;

					case "10":
						return 0x76;

					case "11":
						return 0x77;

					case "12":
						return 0x78;

					case "13":
						return 0x79;

					case "14":
						return 0x80;

					case "15":
						return 0x81;

					case "16":
						return 0x82;

					case "17":
						return 0x83;

					case "18":
						return 0x84;

					case "19":
						return 0x85;

					case "1A":
						return 0x86;

					case "1B":
						return 0x87;

					case "1C":
						return 0x88;

					case "1D":
						return 0x89;

					case "1E":
						return 0x90;

					case "1F":
						return 0x91;

					case "20":
						return 0x92;

					case "B1":
						return 0xa1;

					case "B2":
						return 0xa2;

					case "B3":
						return 0xa3;

					case "B4":
						return 0xa4;

					case "B5":
						return 0xa5;

					case "B6":
						return 0xa6;

					case "B7":
						return 0xa7;

					case "BW":
						return 0xa8;

					case "G0":
						return 0x20;

					case "B0":
						return 0xa0;
				}
			}
			return -1;
		}

		// #25990 2011.8.12 yyagi BMS/BME→DTX変換メイン
		private int tDTX入力_行解析_チャンネル_BMSチャンネル文字列２桁をチャンネル番号にして返す( string strチャンネル文字列２桁, int bar, E種別 eType )
		{
			if ( bar >= 0 &&  strチャンネル文字列２桁.Length == 2 )
			{
				if ( nLastBarConverted != bar )			// 小節が変わったら、BGM用に使うch群
				{
					nLastBarConverted = bar;
					eDTXbgmChs.Reset();
				}

				int bmsCh = Convert.ToInt32( strチャンネル文字列２桁, 16 );
				if ( bmsCh == 0x01 )						// BGMなら
				{
					if ( !eDTXbgmChs.MoveNext() )			// BGM用に使うSEチャネルの空きがもう無い
					{
						return -1;
					}
					return (int) eDTXbgmChs.Current;
				}
				else if ( 0x11 <= bmsCh && bmsCh <= 0x19 )	// 鍵盤なら
				{
					return ( eType == E種別.BMS ) ? BMSkeyChToDTXdrumsCh[ bmsCh - 0x11 ] : BMEkeyChToDTXdrumsCh[ bmsCh - 0x11 ];
				}
				else										// それ以外なら
				{
					return BMSgeneralChToDTXgeneralCh[ bmsCh ];
				}
			}
			return -1;
		}
		private bool tDTX入力_行解析_チャンネル_コマンドから小節番号とチャンネル番号を抜き出す( string strコマンド, out int n小節番号, out int nチャンネル番号 )
		{
			if( strコマンド.Length >= 5 )
			{
				n小節番号 = CConversion.nConvert3DigitMeasureNumberToNumber( strコマンド.Substring( 0, 3 ) );
				if( ( this.e種別 == E種別.GDA ) || ( this.e種別 == E種別.G2D ) )
				{
					nチャンネル番号 = this.tDTX入力_行解析_チャンネル_GDAチャンネル文字列２桁をチャンネル番号にして返す( strコマンド.Substring( 3, 2 ) );
				}
				else if( ( this.e種別 == E種別.BMS ) || ( this.e種別 == E種別.BME ) )	// #25990 2011.8.12 yyagi
				{
					nチャンネル番号 = this.tDTX入力_行解析_チャンネル_BMSチャンネル文字列２桁をチャンネル番号にして返す( strコマンド.Substring( 3, 2 ), n小節番号, this.e種別 );
				}
				else
				{
					nチャンネル番号 = CConversion.nConvert2DigitHexadecimalStringToNumber( strコマンド.Substring( 3, 2 ) );
				}
				return ( ( n小節番号 >= 0 ) && ( nチャンネル番号 > 0 ) );
			}
			n小節番号 = -1;
			nチャンネル番号 = -1;
			return false;
		}
		private bool tDTX入力_行解析_チャンネル_チャンネルに該当するレーン番号を返す( int nチャンネル番号, out int nレーン番号, out bool b裏 )
		{
			nレーン番号 = -1;
			b裏 = false;
			for( int i = 0; i < this._Form.mgr譜面管理者.listレーン.Count; i++ )
			{
				CLane cレーン = this._Form.mgr譜面管理者.listレーン[ i ];
				if( cレーン.nチャンネル番号_表00toFF == nチャンネル番号 )
				{
					nレーン番号 = i;
					b裏 = false;
					return true;
				}
				if( cレーン.nチャンネル番号_裏00toFF == nチャンネル番号 )
				{
					nレーン番号 = i;
					b裏 = true;
					return true;
				}
			}
			return false;
		}
		private CMeasure tDTX入力_行解析_チャンネル_小節番号に対応する小節を探すか新規に作って返す( int n小節番号 )
		{
			CMeasure c小節 = this._Form.mgr譜面管理者.p小節を返す( n小節番号 );
			if( c小節 == null )
			{
				if( n小節番号 > this._Form.mgr譜面管理者.n現在の最大の小節番号を返す() )
				{
					for( int i = this._Form.mgr譜面管理者.n現在の最大の小節番号を返す() + 1; i <= n小節番号; i++ )
					{
						c小節 = new CMeasure( i );
						this._Form.mgr譜面管理者.dic小節.Add( i, c小節 );
					}
					return c小節;
				}
				c小節 = new CMeasure( n小節番号 );
				this._Form.mgr譜面管理者.dic小節.Add( n小節番号, c小節 );
			}
			return c小節;
		}
		private void tDTX入力_小節長倍率配列を昇順ソート済みの小節リストに適用する()
		{
			float num = 1f;
			for( int i = 0; i < this._Form.mgr譜面管理者.dic小節.Count; i++ )
			{
				CMeasure c小節 = this._Form.mgr譜面管理者.dic小節[ i ];
				foreach( KeyValuePair<int, float> pair in this.dic小節長倍率 )
				{
					if( c小節.n小節番号0to3599 == pair.Key )
					{
						num = pair.Value;
					}
				}
				c小節.f小節長倍率 = num;
				for( int j = 0; j < c小節.listチップ.Count; j++ )
				{
					c小節.listチップ[ j ].n位置grid = ( c小節.listチップ[ j ].n位置grid * c小節.n小節長倍率を考慮した現在の小節の高さgrid ) / c小節.listチップ[ j ].n読み込み時の解像度;
				}
			}
		}
		private void tDTX入力_小節内のチップリストを発声位置でソートする()
		{
			foreach( KeyValuePair<int, CMeasure> pair in this._Form.mgr譜面管理者.dic小節 )
			{
				pair.Value.listチップ.Sort();
			}
		}

		private void tDTX出力_AVIリスト( StreamWriter sw )
		{
			sw.WriteLine();
			for( int i = 1; i <= 36 * 36 - 1; i++ )
			{
				CAVI cavi = this._Form.mgrAVIリスト管理者.tAVIをキャッシュから検索して返す( i );
				if( ( cavi != null ) && ( cavi.strファイル名.Length > 0 ) )
				{
					string str = CConversion.strConvertNumberTo2DigitBase36String( cavi.nAVI番号1to1295 );
					sw.Write( "#AVI{0}: {1}", str, cavi.strファイル名 );
					if( cavi.strラベル名.Length > 0 )
					{
						sw.Write( "\t;{0}", cavi.strラベル名 );
					}
					sw.WriteLine();
				}
			}
		}
		private void tDTX出力_AVIリスト色設定( StreamWriter sw )
		{
			Color color = ColorTranslator.FromHtml( "window" );
			Color color2 = ColorTranslator.FromHtml( "windowtext" );
			for( int i = 1; i <= 36 * 36 - 1; i++ )
			{
				CAVI cavi = this._Form.mgrAVIリスト管理者.tAVIをキャッシュから検索して返す( i );
				if( cavi != null )
				{
					if( cavi.col文字色 != color2 )
					{
						sw.WriteLine( "#DTXC_AVIFORECOLOR: {0} {1}", i, ColorTranslator.ToHtml( cavi.col文字色 ) );
					}
					if( cavi.col背景色 != color )
					{
						sw.WriteLine( "#DTXC_AVIBACKCOLOR: {0} {1}", i, ColorTranslator.ToHtml( cavi.col背景色 ) );
					}
				}
			}
		}
		private void tDTX出力_BMPリスト( StreamWriter sw )
		{
			sw.WriteLine();
			for( int i = 1; i <= 36 * 36 - 1; i++ )
			{
				CBMP cbmp = this._Form.mgrBMPリスト管理者.tBMPをキャッシュから検索して返す( i );
				if( ( cbmp != null ) && ( cbmp.strファイル名.Length > 0 ) )
				{
					string str = CConversion.strConvertNumberTo2DigitBase36String( cbmp.nBMP番号1to1295 );
					if( !cbmp.bテクスチャ )
					{
						sw.Write( "#BMP{0}: {1}", str, cbmp.strファイル名 );
						if( cbmp.strラベル名.Length > 0 )
						{
							sw.Write( "\t;{0}", cbmp.strラベル名 );
						}
						sw.WriteLine();
					}
					else
					{
						sw.Write( "#BMPTEX{0}: {1}", str, cbmp.strファイル名 );
						if( cbmp.strラベル名.Length > 0 )
						{
							sw.Write( "\t;{0}", cbmp.strラベル名 );
						}
						sw.WriteLine();
					}
				}
			}
		}
		private void tDTX出力_BMPリスト色設定( StreamWriter sw )
		{
			Color color = ColorTranslator.FromHtml( "window" );
			Color color2 = ColorTranslator.FromHtml( "windowtext" );
			for( int i = 1; i <= 36 * 36 - 1; i++ )
			{
				CBMP cbmp = this._Form.mgrBMPリスト管理者.tBMPをキャッシュから検索して返す( i );
				if( cbmp != null )
				{
					if( cbmp.col文字色 != color2 )
					{
						sw.WriteLine( "#DTXC_BMPFORECOLOR: {0} {1}", i, ColorTranslator.ToHtml( cbmp.col文字色 ) );
					}
					if( cbmp.col背景色 != color )
					{
						sw.WriteLine( "#DTXC_BMPBACKCOLOR: {0} {1}", i, ColorTranslator.ToHtml( cbmp.col背景色 ) );
					}
				}
			}
		}
		private void tDTX出力_BPxリスト( StreamWriter sw )
		{
			sw.WriteLine();
			foreach( KeyValuePair<int, float> pair in this._Form.mgr譜面管理者.dicBPx )
			{
				sw.WriteLine( "#BPM{0}: {1}", CConversion.strConvertNumberTo2DigitBase36String( pair.Key ), pair.Value );
			}
		}
		private void tDTX出力_WAVリスト( StreamWriter sw, bool bBGMのみ出力 )
		{
			sw.WriteLine();
			for( int i = 1; i <= 36 * 36 - 1; i++ )
			{
				CWAV cwav = this._Form.mgrWAVリスト管理者.tWAVをキャッシュから検索して返す( i );
				if( ( ( cwav != null ) && ( cwav.strファイル名.Length > 0 ) ) && ( !bBGMのみ出力 || cwav.bBGMとして使用 ) )
				{
					string str = CConversion.strConvertNumberTo2DigitBase36String( cwav.nWAV番号1to1295 );
					sw.Write( "#WAV{0}: {1}", str, cwav.strファイル名 );
					if( cwav.strラベル名.Length > 0 )
					{
						sw.Write( "\t;{0}", cwav.strラベル名 );
					}
					sw.WriteLine();
					if( cwav.n音量0to100 != 100 )
					{
						sw.WriteLine( "#VOLUME{0}: {1}", str, cwav.n音量0to100.ToString() );
					}
					if( cwav.n位置_100to100 != 0 )
					{
						sw.WriteLine( "#PAN{0}: {1}", str, cwav.n位置_100to100.ToString() );
					}
					if( cwav.bBGMとして使用 )
					{
						sw.WriteLine( "#BGMWAV: {0}", str );
					}
				}
			}
		}
		private void tDTX出力_WAVリスト色設定( StreamWriter sw )
		{
			Color color = ColorTranslator.FromHtml( "window" );
			Color color2 = ColorTranslator.FromHtml( "windowtext" );
			for( int i = 1; i <= 36 * 36 - 1; i++ )
			{
				CWAV cwav = this._Form.mgrWAVリスト管理者.tWAVをキャッシュから検索して返す( i );
				if( cwav != null )
				{
					if( cwav.col文字色 != color2 )
					{
						sw.WriteLine( "#DTXC_WAVFORECOLOR: {0} {1}", i, ColorTranslator.ToHtml( cwav.col文字色 ) );
					}
					if( cwav.col背景色 != color )
					{
						sw.WriteLine( "#DTXC_WAVBACKCOLOR: {0} {1}", i, ColorTranslator.ToHtml( cwav.col背景色 ) );
					}
				}
			}
		}
		private void tDTX出力_タイトルと製作者とコメントその他( StreamWriter sw )
		{
			sw.WriteLine();
			if( this._Form.textBox曲名.Text.Length == 0 )
			{
				sw.WriteLine( "#TITLE: (no title)" );
			}
			else
			{
				sw.WriteLine( "#TITLE: " + this._Form.textBox曲名.Text );
			}
			if( this._Form.textBox製作者.Text.Length > 0 )
			{
				sw.WriteLine( "#ARTIST: " + this._Form.textBox製作者.Text );
			}
			if( this._Form.textBoxコメント.Text.Length > 0 )
			{
				sw.WriteLine( "#COMMENT: " + this._Form.textBoxコメント.Text );
			}
			if( this._Form.textBoxパネル.Text.Length > 0 )
			{
				sw.WriteLine( "#PANEL: " + this._Form.textBoxパネル.Text );
			}
			if( this._Form.textBoxPREVIEW.Text.Length > 0 )
			{
				sw.WriteLine( "#PREVIEW: " + this._Form.textBoxPREVIEW.Text );
			}
			if( this._Form.textBoxPREIMAGE.Text.Length > 0 )
			{
				sw.WriteLine( "#PREIMAGE: " + this._Form.textBoxPREIMAGE.Text );
			}
			if( this._Form.textBoxSTAGEFILE.Text.Length > 0 )
			{
				sw.WriteLine( "#STAGEFILE: " + this._Form.textBoxSTAGEFILE.Text );
			}
			if( this._Form.textBoxBACKGROUND.Text.Length > 0 )
			{
				sw.WriteLine( "#BACKGROUND: " + this._Form.textBoxBACKGROUND.Text );
			}
			if( this._Form.textBoxRESULTIMAGE.Text.Length > 0 )
			{
				sw.WriteLine( "#RESULTIMAGE: " + this._Form.textBoxRESULTIMAGE.Text );
			}
			if( this._Form.numericUpDownBPM.Value != 0M )
			{
				sw.WriteLine( "#BPM: " + this._Form.numericUpDownBPM.Value );
			}
			if( this._Form.hScrollBarDLEVEL.Value != 0 )
			{
				sw.WriteLine( "#DLEVEL: " + this._Form.hScrollBarDLEVEL.Value );
			}
			if( this._Form.hScrollBarGLEVEL.Value != 0 )
			{
				sw.WriteLine( "#GLEVEL: " + this._Form.hScrollBarGLEVEL.Value );
			}
			if( this._Form.hScrollBarBLEVEL.Value != 0 )
			{
				sw.WriteLine( "#BLEVEL: " + this._Form.hScrollBarBLEVEL.Value );
			}
			if( this._Form.mgr譜面管理者.strPATH_WAV.Length != 0 )
			{
				sw.WriteLine( "#PATH_WAV: " + this._Form.mgr譜面管理者.strPATH_WAV );
			}
			if( this._Form.toolStripComboBox演奏速度.SelectedIndex != 5 )
			{
				sw.WriteLine( "#DTXVPLAYSPEED: " + ( 1.5f - ( this._Form.toolStripComboBox演奏速度.SelectedIndex * 0.1f ) ) );
			}
		}
		private void tDTX出力_チップパレット( StreamWriter sw )
		{
			sw.Write( "#DTXC_CHIPPALETTE: " );
			foreach( ListViewItem item in this._Form.dlgチップパレット.listViewチップリスト.Items )
			{
				sw.Write( " {0},{1}", item.ImageIndex, item.SubItems[ 1 ].Text );
			}
			sw.WriteLine();
		}
		private void tDTX出力_レーン割付チップ( StreamWriter sw )
		{
			sw.WriteLine();
			for( int i = 0; i < this._Form.mgr譜面管理者.listレーン.Count; i++ )
			{
				CLane cレーン = this._Form.mgr譜面管理者.listレーン[ i ];
				if( ( cレーン.nレーン割付チップ_表0or1to1295 > 0 ) || ( cレーン.nレーン割付チップ_裏0or1to1295 > 0 ) )
				{
					sw.WriteLine( "#DTXC_LANEBINDEDCHIP: {0} {1} {2}", i.ToString( "00" ), CConversion.strConvertNumberTo2DigitBase36String( cレーン.nレーン割付チップ_表0or1to1295 ), CConversion.strConvertNumberTo2DigitBase36String( cレーン.nレーン割付チップ_裏0or1to1295 ) );
				}
			}
		}
		private void tDTX出力_自由入力欄( StreamWriter sw )
		{
			sw.WriteLine();
			if( this._Form.textBox自由入力欄.Text.Length > 0 )
			{
				sw.WriteLine();
				sw.Write( this._Form.textBox自由入力欄.Text );
				sw.WriteLine();
			}
		}
		private void tDTX出力_小節長倍率( StreamWriter sw )
		{
			sw.WriteLine();
			float num = 1f;
			for( int i = 0; i < this._Form.mgr譜面管理者.dic小節.Count; i++ )
			{
				CMeasure c小節 = this._Form.mgr譜面管理者.dic小節[ i ];
				if( c小節.f小節長倍率 != num )
				{
					num = c小節.f小節長倍率;
					sw.WriteLine( "#{0}02: {1}", CConversion.strConvertNumberTo3DigitMeasureNumber( c小節.n小節番号0to3599 ), num );
				}
			}
		}
		private void tDTX出力_全チップ( StreamWriter sw )
		{
			sw.WriteLine();
			foreach( KeyValuePair<int, CMeasure> pair in this._Form.mgr譜面管理者.dic小節 )
			{
				CMeasure c小節 = pair.Value;
				List<int> list = new List<int>();
				foreach( CChip cチップ in c小節.listチップ )
				{
					if( list.IndexOf( cチップ.nチャンネル番号00toFF ) < 0 )
					{
						list.Add( cチップ.nチャンネル番号00toFF );
					}
				}
				int[,] numArray = new int[ c小節.n小節長倍率を考慮した現在の小節の高さgrid, 2 ];
				foreach( int num in list )
				{
					if( num != 0 )
					{
						for( int num2 = 0; num2 < c小節.n小節長倍率を考慮した現在の小節の高さgrid; num2++ )
						{
							numArray[ num2, 0 ] = numArray[ num2, 1 ] = 0;
						}
						foreach( CChip cチップ2 in c小節.listチップ )
						{
							if( cチップ2.nチャンネル番号00toFF == num )
							{
								numArray[ cチップ2.n位置grid, 0 ] = cチップ2.n値_整数1to1295;
							}
						}
						int num3 = 0;
						for( int num4 = 0; num4 < c小節.n小節長倍率を考慮した現在の小節の高さgrid; num4++ )
						{
							num3 += numArray[ num4, 0 ];
						}
						if( num3 != 0 )
						{
							int num5 = c小節.n小節長倍率を考慮した現在の小節の高さgrid;
							foreach( int num6 in this.arr素数リスト )
							{
								while( this.tDTX出力_全チップ_解像度をＮ分の１にできる( num6, ref numArray, num5 ) )
								{
									num5 /= num6;
									for( int num7 = 0; num7 < num5; num7++ )
									{
										numArray[ num7, 0 ] = numArray[ num7 * num6, 0 ];
									}
								}
							}
							StringBuilder builder = new StringBuilder();
							for( int num8 = 0; num8 < num5; num8++ )
							{
								if( num == 3 )
								{
									builder.Append( CConversion.strConvertNumberTo2DigitHexadecimalString( numArray[ num8, 0 ] ) );
								}
								else
								{
									builder.Append( CConversion.strConvertNumberTo2DigitBase36String( numArray[ num8, 0 ] ) );
								}
							}
							sw.WriteLine( "#{0}{1}: {2}", CConversion.strConvertNumberTo3DigitMeasureNumber( c小節.n小節番号0to3599 ), CConversion.strConvertNumberTo2DigitHexadecimalString( num ), builder.ToString() );
						}
					}
				}
				for( int i = 0; i < c小節.n小節長倍率を考慮した現在の小節の高さgrid; i++ )
				{
					numArray[ i, 0 ] = numArray[ i, 1 ] = 0;
				}
				foreach( CChip cチップ3 in c小節.listチップ )
				{
					CLane cレーン = this._Form.mgr譜面管理者.listレーン[ cチップ3.nレーン番号0to ];
					switch( cレーン.eレーン種別 )
					{
						case CLane.E種別.GtV:
							{
								numArray[ cチップ3.n位置grid, 0 ] = cチップ3.n値_整数1to1295;
								continue;
							}
						case CLane.E種別.GtR:
							{
								numArray[ cチップ3.n位置grid, 1 ] |= ( cチップ3.n値_整数1to1295 == 1 ) ? 0x04 : 0xFF; // OPEN = 0xFF
								continue;
							}
						case CLane.E種別.GtG:
							{
								numArray[ cチップ3.n位置grid, 1 ] |= 0x02;
								continue;
							}
						case CLane.E種別.GtB:
							{
								numArray[ cチップ3.n位置grid, 1 ] |= 0x01;
								continue;
							}
                        case CLane.E種別.GtY:
                            {
                                numArray[ cチップ3.n位置grid, 1 ] |= 16;
                                continue;
                            }
                        case CLane.E種別.GtP:
                            {
                                numArray[ cチップ3.n位置grid, 1 ] |= 32;
                                continue;
                            }
					}
				}
				for( int j = 0; j < c小節.n小節長倍率を考慮した現在の小節の高さgrid; j++ )
				{
					if( ( numArray[ j, 0 ] == 0 ) || ( numArray[ j, 1 ] == 0 ) )
					{
						numArray[ j, 0 ] = 0;
						numArray[ j, 1 ] = 0;
					}
				}
				int num11 = c小節.n小節長倍率を考慮した現在の小節の高さgrid;
				foreach( int num12 in this.arr素数リスト )
				{
					while( this.tDTX出力_全チップ_解像度をＮ分の１にできる( num12, ref numArray, num11 ) )
					{
						num11 /= num12;
						for( int num13 = 0; num13 < num11; num13++ )
						{
							numArray[ num13, 0 ] = numArray[ num13 * num12, 0 ];
							numArray[ num13, 1 ] = numArray[ num13 * num12, 1 ];
						}
					}
				}
				bool[] flagArray = new bool[ 255 ];
				for( int k = 0; k < 255; k++ )
				{
					flagArray[ k ] = false;
				}
				for( int m = 0; m < num11; m++ )
				{
					if( numArray[ m, 1 ] == 0xff )
					{
						flagArray[ 0 ] = true;
					}
					else if( numArray[ m, 1 ] != 0 )
					{
						flagArray[ numArray[ m, 1 ] ] = true;
					}
				}
				StringBuilder[] builderArray = new StringBuilder[ 255 ];
				for( int n = 0; n < 255; n++ )
				{
					builderArray[ n ] = new StringBuilder();
				}
				for( int num17 = 0; num17 < num11; num17++ )
				{
					int num18 = ( numArray[ num17, 1 ] == 0xff ) ? 0x20 : ( numArray[ num17, 1 ] + 0x20 );
					for( int num19 = 0; num19 < 255; num19++ )
					{
						if( flagArray[ num19 ] )
						{
							if( num19 == ( num18 - 0x20 ) )
							{
								builderArray[ num19 ].Append( CConversion.strConvertNumberTo2DigitBase36String( numArray[ num17, 0 ] ) );
							}
							else
							{
								builderArray[ num19 ].Append( "00" );
							}
						}
					}
				}
				for( int num20 = 0; num20 < 255; num20++ )
				{
					if( builderArray[ num20 ].Length != 0 )
					{
                        int num16 = 0;
                        if (num20 < 16)
                        {
                            num16 = num20 + 32;
                        }
                        if (num20 == 16)
                        {
                            num16 = 147;
                        }
                        if (num20 == 17)
                        {
                            num16 = 148;
                        }
                        if (num20 == 18)
                        {
                            num16 = 149;
                        }
                        if (num20 == 19)
                        {
                            num16 = 150;
                        }
                        if (num20 == 20)
                        {
                            num16 = 151;
                        }
                        if (num20 == 21)
                        {
                            num16 = 152;
                        }
                        if (num20 == 22)
                        {
                            num16 = 153;
                        }
                        if (num20 == 23)
                        {
                            num16 = 154;
                        }
                        if (num20 == 32)
                        {
                            num16 = 155;
                        }
                        if (num20 == 33)
                        {
                            num16 = 156;
                        }
                        if (num20 == 34)
                        {
                            num16 = 157;
                        }
                        if (num20 == 35)
                        {
                            num16 = 158;
                        }
                        if (num20 == 36)
                        {
                            num16 = 159;
                        }
                        if (num20 == 37)
                        {
                            num16 = 169;
                        }
                        if (num20 == 38)
                        {
                            num16 = 170;
                        }
                        if (num20 == 39)
                        {
                            num16 = 171;
                        }
                        if (num20 == 48)
                        {
                            num16 = 172;
                        }
                        if (num20 == 49)
                        {
                            num16 = 173;
                        }
                        if (num20 == 50)
                        {
                            num16 = 174;
                        }
                        if (num20 == 51)
                        {
                            num16 = 175;
                        }
                        if (num20 == 52)
                        {
                            num16 = 208;
                        }
                        if (num20 == 53)
                        {
                            num16 = 209;
                        }
                        if (num20 == 54)
                        {
                            num16 = 210;
                        }
                        if (num20 == 55)
                        {
                            num16 = 211;
                        }

						sw.WriteLine( "#{0}{1}: {2}", CConversion.strConvertNumberTo3DigitMeasureNumber( c小節.n小節番号0to3599 ), CConversion.strConvertNumberTo2DigitHexadecimalString( num16 ), builderArray[ num20 ].ToString() );
					}
				}
				for( int num21 = 0; num21 < c小節.n小節長倍率を考慮した現在の小節の高さgrid; num21++ )
				{
					numArray[ num21, 0 ] = numArray[ num21, 1 ] = 0;
				}
				foreach( CChip cチップ4 in c小節.listチップ )
				{
					CLane cレーン2 = this._Form.mgr譜面管理者.listレーン[ cチップ4.nレーン番号0to ];
					switch( cレーン2.eレーン種別 )
					{
						case CLane.E種別.BsV:
							{
								numArray[ cチップ4.n位置grid, 0 ] = cチップ4.n値_整数1to1295;
								continue;
							}
						case CLane.E種別.BsR:
							{
								numArray[ cチップ4.n位置grid, 1 ] |= ( cチップ4.n値_整数1to1295 == 1 ) ? 4 : 0xff;	// OPEN = 0xFF
								continue;
							}
						case CLane.E種別.BsG:
							{
								numArray[ cチップ4.n位置grid, 1 ] |= 0x02;
								continue;
							}
						case CLane.E種別.BsB:
							{
								numArray[ cチップ4.n位置grid, 1 ] |= 0x01;
								continue;
							}
                        case CLane.E種別.BsY:
                            {
                                numArray[cチップ4.n位置grid, 1] |= 16;
                                continue;
                            }
                        case CLane.E種別.BsP:
                            {
                                numArray[cチップ4.n位置grid, 1] |= 32;
                                continue;
                            }

					}
				}
				for( int num22 = 0; num22 < c小節.n小節長倍率を考慮した現在の小節の高さgrid; num22++ )
				{
					if( ( numArray[ num22, 0 ] == 0 ) || ( numArray[ num22, 1 ] == 0 ) )
					{
						numArray[ num22, 0 ] = 0;
						numArray[ num22, 1 ] = 0;
					}
				}
				int num23 = c小節.n小節長倍率を考慮した現在の小節の高さgrid;
				foreach( int num24 in this.arr素数リスト )
				{
					while( this.tDTX出力_全チップ_解像度をＮ分の１にできる( num24, ref numArray, num23 ) )
					{
						num23 /= num24;
						for( int num25 = 0; num25 < num23; num25++ )
						{
							numArray[ num25, 0 ] = numArray[ num25 * num24, 0 ];
							numArray[ num25, 1 ] = numArray[ num25 * num24, 1 ];
						}
					}
				}
				bool[] flagArray2 = new bool[ 255 ];
				for( int num26 = 0; num26 < 255; num26++ )
				{
					flagArray2[ num26 ] = false;
				}
				for( int num27 = 0; num27 < num23; num27++ )
				{
					if( numArray[ num27, 1 ] == 0xff )
					{
						flagArray2[ 0 ] = true;
					}
					else if( numArray[ num27, 1 ] != 0 )
					{
						flagArray2[ numArray[ num27, 1 ] ] = true;
					}
				}
				StringBuilder[] builderArray2 = new StringBuilder[ 255 ];
				for( int num28 = 0; num28 < 255; num28++ )
				{
					builderArray2[ num28 ] = new StringBuilder();
				}
				for( int num29 = 0; num29 < num23; num29++ )
				{
					int num30 = ( numArray[ num29, 1 ] == 0xff ) ? 160 : ( numArray[ num29, 1 ] + 160 );
					for( int num31 = 0; num31 < 255; num31++ )
					{
						if( flagArray2[ num31 ] )
						{
							if( num31 == ( num30 - 160 ) )
							{
								builderArray2[ num31 ].Append( CConversion.strConvertNumberTo2DigitBase36String( numArray[ num29, 0 ] ) );
							}
							else
							{
								builderArray2[ num31 ].Append( "00" );
							}
						}
					}
				}
                for (int num32 = 0; num32 < 255; num32++)
                {
                    if (builderArray2[num32].Length != 0)
                    {
                        int num33 = 0;
                        if (num32 < 16)
                        {
                            num33 = num32 + 160;
                        }
                        if (num32 == 16)
                        {
                            num33 = 197;
                        }
                        if (num32 == 17)
                        {
                            num33 = 198;
                        }
                        if (num32 == 18)
                        {
                            num33 = 200;
                        }
                        if (num32 == 19)
                        {
                            num33 = 201;
                        }
                        if (num32 == 20)
                        {
                            num33 = 202;
                        }
                        if (num32 == 21)
                        {
                            num33 = 203;
                        }
                        if (num32 == 22)
                        {
                            num33 = 204;
                        }
                        if (num32 == 23)
                        {
                            num33 = 205;
                        }
                        if (num32 == 32)
                        {
                            num33 = 206;
                        }
                        if (num32 == 33)
                        {
                            num33 = 207;
                        }
                        if (num32 == 34)
                        {
                            num33 = 218;
                        }
                        if (num32 == 35)
                        {
                            num33 = 219;
                        }
                        if (num32 == 36)
                        {
                            num33 = 220;
                        }
                        if (num32 == 37)
                        {
                            num33 = 221;
                        }
                        if (num32 == 38)
                        {
                            num33 = 222;
                        }
                        if (num32 == 39)
                        {
                            num33 = 223;
                        }
                        if (num32 == 48)
                        {
                            num33 = 225;
                        }
                        if (num32 == 49)
                        {
                            num33 = 226;
                        }
                        if (num32 == 50)
                        {
                            num33 = 227;
                        }
                        if (num32 == 51)
                        {
                            num33 = 228;
                        }
                        if (num32 == 52)
                        {
                            num33 = 229;
                        }
                        if (num32 == 53)
                        {
                            num33 = 230;
                        }
                        if (num32 == 54)
                        {
                            num33 = 231;
                        }
                        if (num32 == 55)
                        {
                            num33 = 232;
                        }
                        sw.WriteLine("#{0}{1}: {2}", CConversion.strConvertNumberTo3DigitMeasureNumber(c小節.n小節番号0to3599), CConversion.strConvertNumberTo2DigitHexadecimalString(num33), builderArray2[num32].ToString());
                    }
                }
            }
		}
		private bool tDTX出力_全チップ_解像度をＮ分の１にできる( int N, ref int[ , ] arrチップ配列, int n現在の解像度 )
		{
			if( ( n現在の解像度 % N ) != 0 )
			{
				return false;
			}
			for( int i = 0; i < ( n現在の解像度 / N ); i++ )
			{
				for( int j = 1; j < N; j++ )
				{
					if( arrチップ配列[ ( i * N ) + j, 0 ] != 0 )
					{
						return false;
					}
				}
			}
			return true;
		}

		#region [#23880 2010.12.30 yyagi: コンマとスペースの両方を小数点として扱うTryParse]
		/// <summary>
		/// 小数点としてコンマとピリオドの両方を受け付けるTryParse()
		/// </summary>
		/// <param name="s">strings convert to double</param>
		/// <param name="result">parsed double value</param>
		/// <returns>s が正常に変換された場合は true。それ以外の場合は false。</returns>
		/// <exception cref="ArgumentException">style が NumberStyles 値でないか、style に NumberStyles.AllowHexSpecifier 値が含まれている</exception>
		private bool TryParse(string s, out decimal result)
		{	// #23880 2010.12.30 yyagi: alternative TryParse to permit both '.' and ',' for decimal point
			// EU諸国での #BPM 123,45 のような記述に対応するため、
			// 小数点の最終位置を検出して、それをlocaleにあった
			// 文字に置き換えてからTryParse()する
			// 桁区切りの文字はスキップする

			const string DecimalSeparators = ".,";				// 小数点文字
			const string GroupSeparators = ".,' ";				// 桁区切り文字
			const string NumberSymbols = "0123456789";			// 数値文字

			int len = s.Length;									// 文字列長
			int decimalPosition = len;							// 真の小数点の位置 最初は文字列終端位置に仮置きする

			for (int i = 0; i < len; i++)
			{							// まず、真の小数点(一番最後に現れる小数点)の位置を求める
				char c = s[i];
				if (NumberSymbols.IndexOf(c) >= 0)
				{				// 数値だったらスキップ
					continue;
				}
				else if (DecimalSeparators.IndexOf(c) >= 0)
				{		// 小数点文字だったら、その都度位置を上書き記憶
					decimalPosition = i;
				}
				else if (GroupSeparators.IndexOf(c) >= 0)
				{		// 桁区切り文字の場合もスキップ
					continue;
				}
				else
				{											// 数値_小数点_区切り文字以外がきたらループ終了
					break;
				}
			}

			StringBuilder decimalStr = new StringBuilder(16);
			for (int i = 0; i < len; i++)
			{							// 次に、localeにあった数値文字列を生成する
				char c = s[i];
				if (NumberSymbols.IndexOf(c) >= 0)
				{				// 数値だったら
					decimalStr.Append(c);							// そのままコピー
				}
				else if (DecimalSeparators.IndexOf(c) >= 0)
				{		// 小数点文字だったら
					if (i == decimalPosition)
					{						// 最後に出現した小数点文字なら、localeに合った小数点を出力する
						decimalStr.Append(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
					}
				}
				else if (GroupSeparators.IndexOf(c) >= 0)
				{		// 桁区切り文字だったら
					continue;										// 何もしない(スキップ)
				}
				else
				{
					break;
				}
			}
			return decimal.TryParse(decimalStr.ToString(), out result);	// 最後に、自分のlocale向けの文字列に対してTryParse実行
		}
		#endregion
		//-----------------
		#endregion
	}
}
