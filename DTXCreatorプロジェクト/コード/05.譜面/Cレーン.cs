using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DTXCreator.譜面
{
	public class Cレーン
	{
		public const int LANEWIDTH = 30;

		public enum E種別
		{
            BPM,
            WAV,
            BMP,
            AVI,
            FI,
            GtV,
            GtR,
            GtG,
            GtB,
            GtY,
            GtP,
            GtW,
            BsV,
            BsR,
            BsG,
            BsB,
            BsY,
            BsP,
            BsW
        }
		public enum ELaneType
		{
			BPM,
			Drums,
			BGM,
			SE1_5,
			SE6_32,
			Guitar,
			Bass,
			AVI1_2,
			BGA1_5,
			BGA6_8,
			END			// 何か非値を設定したくなったときのための値(nullの代わり)
		}
		
		public bool b左側の線が太線;
		public Color col背景色 = Color.Black;
		public E種別 eレーン種別 = E種別.WAV;
		public int nチャンネル番号・表00toFF;
		public int nチャンネル番号・裏00toFF;
		public int nレーン割付チップ・表0or1to1295;
		public int nレーン割付チップ・裏0or1to1295;
		public int n位置Xdot;
		public int n幅dot = 30;
		public string strレーン名 = "";
		public ELaneType eLaneType { get; set; }
		public bool bIsVisible		// 
		{
			get
			{
				return ( n幅dot > 0 );
			}
			set
			{
				n幅dot = ( value == true ) ? LANEWIDTH : 0;
			}
		}


		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Cレーン()
		{
			this.eレーン種別 = E種別.WAV;
			this.strレーン名 = "";
			this.nチャンネル番号・表00toFF = 0;
			this.nチャンネル番号・裏00toFF = 0;
			this.b左側の線が太線 = false;
			this.col背景色 = Color.FromArgb(0, 0, 0, 0);
			this.n位置Xdot = 0;
			this.n幅dot = 30;
			this.eLaneType = ELaneType.SE1_5;
			this.bIsVisible = true;
		}

		/// <summary>
		/// コンストラクタ(初期化用)
		/// </summary>
		/// <param name="eレーン種別"></param>
		/// <param name="strレーン名"></param>
		/// <param name="nチャンネル番号・表00toFF"></param>
		/// <param name="nチャンネル番号・裏00toFF"></param>
		/// <param name="b左側の線が太線"></param>
		/// <param name="col背景色"></param>
		/// <param name="n位置Xdot"></param>
		/// <param name="n幅dot"></param>
		/// <param name="eLaneType"></param>
		/// <param name="bIsVisible"></param>
		public Cレーン(
			E種別 eレーン種別_, string strレーン名_,
			int nチャンネル番号・表00toFF_, int nチャンネル番号・裏00toFF_,
			bool b左側の線が太線_,
			Color col背景色_,
			int n位置Xdot_, int n幅dot_,
			ELaneType eLaneType_,
			bool bIsVisible_ )
		{
			this.eレーン種別 = eレーン種別_;
			this.strレーン名 = strレーン名_;
			this.nチャンネル番号・表00toFF = nチャンネル番号・表00toFF_;
			this.nチャンネル番号・裏00toFF = nチャンネル番号・裏00toFF_;
			this.b左側の線が太線 = b左側の線が太線_;
			this.col背景色 = col背景色_;
			this.n位置Xdot = n位置Xdot_;
			this.n幅dot = n幅dot_;
			this.eLaneType = eLaneType_;
			this.bIsVisible = bIsVisible_;
		}

		public bool bパターンレーンである()
		{
            return this.eレーン種別 == Cレーン.E種別.GtR || this.eレーン種別 == Cレーン.E種別.GtG || this.eレーン種別 == Cレーン.E種別.GtB || this.eレーン種別 == Cレーン.E種別.GtY || this.eレーン種別 == Cレーン.E種別.GtP || this.eレーン種別 == Cレーン.E種別.BsR || this.eレーン種別 == Cレーン.E種別.BsG || this.eレーン種別 == Cレーン.E種別.BsB || this.eレーン種別 == Cレーン.E種別.BsY || this.eレーン種別 == Cレーン.E種別.BsP;
        }
	}
}
