using System;
using System.Collections.Generic;
using System.Text;
using FDK;

namespace DTXMania
{
	internal class CAct演奏RGB共通 : CActivity
	{
        //こっちではほとんどやることなんてないんだけどね____
        //一応暫定対応として押している状態を取得&発信しているだけ。

		// プロパティ

		public bool[] b押下状態 = new bool[ 10 ];
        protected STDGBVALUE<int> nシャッター上;
        protected STDGBVALUE<int> nシャッター下;
        protected STDGBVALUE<double> dbシャッター上;
        protected STDGBVALUE<double> dbシャッター下;
        protected double db倍率 = 6.14;
        protected CTexture txRGB;
        protected CTexture txシャッター;
        protected CActLVLNFont actLVFont;

		// コンストラクタ

		public CAct演奏RGB共通()
		{
            base.listChildActivities.Add(this.actLVFont = new CActLVLNFont());
            base.bNotActivated = true;
		}
		
		
		// メソッド

		public void Push( int nLane )
		{
			this.b押下状態[ nLane ] = true;
		}


		// CActivity 実装

		public override void OnActivate()
		{
			for( int i = 0; i < 10; i++ )
			{
				this.b押下状態[ i ] = false;
			}
			base.OnActivate();
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
                this.txRGB = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_RGB buttons.png"));
                this.txシャッター = CDTXMania.tGenerateTexture(CSkin.Path(@"Graphics\7_shutter_GB.png"));
                base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
				CDTXMania.tReleaseTexture( ref this.txRGB );
                CDTXMania.tReleaseTexture(ref this.txシャッター);
                base.OnManagedReleaseResources();
			}
		}
	}
}
