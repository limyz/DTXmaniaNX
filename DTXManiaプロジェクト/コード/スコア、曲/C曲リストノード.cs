using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;

namespace DTXMania
{
	[Serializable]
	internal class C曲リストノード
	{
		// プロパティ

		public Eノード種別 eノード種別 = Eノード種別.UNKNOWN;
		public enum Eノード種別
		{
			SCORE,
			SCORE_MIDI,
			BOX,
			BACKBOX,
			RANDOM,
			UNKNOWN
		}
		public int nID { get; private set; }
		public Cスコア[] arスコア = new Cスコア[ 5 ];
		public string[] ar難易度ラベル = new string[ 5 ];
		public bool bDTXFilesで始まるフォルダ名のBOXである;
		public bool bBoxDefで作成されたBOXである
		{
			get
			{
				return !this.bDTXFilesで始まるフォルダ名のBOXである;
			}
			set
			{
				this.bDTXFilesで始まるフォルダ名のBOXである = !value;
			}
		}
		public Color col文字色 = Color.White;
		public List<C曲リストノード> listランダム用ノードリスト;
		public List<C曲リストノード> list子リスト;
		public int nGood範囲ms = -1;
		public int nGreat範囲ms = -1;
		public int nPerfect範囲ms = -1;
		public int nPoor範囲ms = -1;
		public int nスコア数;
		public string pathSetDefの絶対パス = "";
		public C曲リストノード r親ノード;
		public int SetDefのブロック番号;
		public Stack<int> stackランダム演奏番号 = new Stack<int>();
		public string strジャンル = "";
		public string strタイトル = "";
		public string strBreadcrumbs = "";		// #27060 2011.2.27 yyagi; MUSIC BOXのパンくずリスト (曲リスト構造内の絶対位置捕捉のために使う)
		public string strSkinPath = "";			// #28195 2012.5.4 yyagi; box.defでのスキン切り替え対応
        public string strバージョン = "";
		
		// コンストラクタ

		public C曲リストノード()
		{
			this.nID = id++;
		}


		// その他

		#region [ private ]
		//-----------------
		private static int id;
		//-----------------
		#endregion
	}
}
