using System;
using System.Collections.Generic;
using System.Text;
using FDK;

namespace DTXMania
{
	internal class CActPerfCommonRGB : CActivity
	{
        //こっちではほとんどやることなんてないんだけどね____
        //一応暫定対応として押している状態を取得&発信しているだけ。

		// プロパティ

		public bool[] bPressedState = new bool[ 10 ];
        protected STDGBVALUE<int> nシャッター上;
        protected STDGBVALUE<int> nシャッター下;
        protected STDGBVALUE<double> dbAboveShutter;
        protected STDGBVALUE<double> dbUnderShutter;
        protected double db倍率 = 6.14;
        protected CTexture txRGB;
        protected CTexture txShutter;
        protected CActLVLNFont actLVFont;

		// コンストラクタ

		public CActPerfCommonRGB()
		{
            base.listChildActivities.Add(this.actLVFont = new CActLVLNFont());
            base.bNotActivated = true;
		}
		
		
		// メソッド

		public void Push( int nLane )
		{
			this.bPressedState[ nLane ] = true;
		}


		// CActivity 実装

		public override void OnActivate()
		{
			for( int i = 0; i < 10; i++ )
			{
				this.bPressedState[ i ] = false;
			}
			base.OnActivate();
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
                this.txRGB = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_RGB buttons.png"));
                this.txShutter = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_shutter_GB.png"));
                base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
				CDTXMania.tReleaseTexture( ref this.txRGB );
                CDTXMania.tReleaseTexture(ref this.txShutter);
                base.OnManagedReleaseResources();
			}
		}
	}
}
