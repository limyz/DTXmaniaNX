using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;

namespace DTXMania
{
	[Serializable]
	internal class CSongListNode
	{
		// プロパティ

		public ENodeType eNodeType = ENodeType.UNKNOWN;
		public enum ENodeType
		{
			SCORE,
			SCORE_MIDI,
			BOX,
			BACKBOX,
			RANDOM,
			UNKNOWN
		}
		public int nID { get; private set; }
		public CScore[] arScore = new CScore[ 5 ];
		public string[] arDifficultyLabel = new string[ 5 ];
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
		public List<CSongListNode> listランダム用ノードリスト;
		public List<CSongListNode> list子リスト;
		public CHitRanges DrumHitRanges = new CHitRanges(nDefaultSize: -1);
		public CHitRanges DrumPedalHitRanges = new CHitRanges(nDefaultSize: -1);
		public CHitRanges GuitarHitRanges = new CHitRanges(nDefaultSize: -1);
		public CHitRanges BassHitRanges = new CHitRanges(nDefaultSize: -1);
		public int nスコア数;
		public string pathSetDefの絶対パス = "";
		public CSongListNode r親ノード;
		public int SetDefのブロック番号;
		public Stack<int> stackRandomPerformanceNumber = new Stack<int>();
		public string strジャンル = "";
		public string strタイトル = "";
		public string strBreadcrumbs = "";		// #27060 2011.2.27 yyagi; MUSIC BOXのパンくずリスト (曲リスト構造内の絶対位置捕捉のために使う)
		public string strSkinPath = "";			// #28195 2012.5.4 yyagi; box.defでのスキン切り替え対応
        public string strバージョン = "";
		
		// コンストラクタ

		public CSongListNode()
		{
			this.nID = id++;
		}

		//
		public CSongListNode ShallowCopyOfSelf()
        {
			CSongListNode newNode = new CSongListNode();
			newNode.eNodeType = this.eNodeType;
			newNode.nID = this.nID;
			newNode.arDifficultyLabel = this.arDifficultyLabel;
			newNode.arScore = this.arScore;
			newNode.bDTXFilesで始まるフォルダ名のBOXである = this.bDTXFilesで始まるフォルダ名のBOXである;
			newNode.bBoxDefで作成されたBOXである = this.bBoxDefで作成されたBOXである;
			newNode.col文字色 = this.col文字色;
			newNode.listランダム用ノードリスト = this.listランダム用ノードリスト;
			newNode.list子リスト = this.list子リスト;
			newNode.DrumHitRanges.tCopyFrom(DrumHitRanges);
			newNode.DrumPedalHitRanges.tCopyFrom(DrumPedalHitRanges);
			newNode.GuitarHitRanges.tCopyFrom(GuitarHitRanges);
			newNode.BassHitRanges.tCopyFrom(BassHitRanges);
			newNode.nスコア数 = this.nスコア数;
			newNode.pathSetDefの絶対パス = this.pathSetDefの絶対パス;
			newNode.r親ノード = this.r親ノード;
			newNode.SetDefのブロック番号 = this.SetDefのブロック番号;
			newNode.stackRandomPerformanceNumber = this.stackRandomPerformanceNumber;
			newNode.strジャンル = this.strジャンル;
			newNode.strタイトル = this.strタイトル;
			newNode.strバージョン = this.strバージョン;
			newNode.strBreadcrumbs = this.strBreadcrumbs;
			newNode.strSkinPath = this.strSkinPath;

			return newNode;
		}

		// Other

		#region [ private ]
		//-----------------
		private static int id;
		//-----------------
		#endregion
	}
}
