using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using FDK;

namespace DTXMania
{
	internal class CStage終了 : CStage
	{
		// コンストラクタ

		public CStage終了()
		{
			base.eステージID = CStage.Eステージ.終了;
			base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
			base.bNotActivated = true;
		}


		// CStage 実装

		public override void OnActivate()
		{
			Trace.TraceInformation( "終了ステージを活性化します。" );
			Trace.Indent();
			try
			{
				this.ct時間稼ぎ = new CCounter();
				base.OnActivate();
			}
			finally
			{
				Trace.TraceInformation( "終了ステージの活性化を完了しました。" );
				Trace.Unindent();
			}
		}
		public override void OnDeactivate()
		{
			Trace.TraceInformation( "終了ステージを非活性化します。" );
			Trace.Indent();
			try
			{
				base.OnDeactivate();
			}
			finally
			{
				Trace.TraceInformation( "終了ステージの非活性化を完了しました。" );
				Trace.Unindent();
			}
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
				this.tx背景 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\9_background.jpg" ), false );
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
				CDTXMania.tテクスチャの解放( ref this.tx背景 );
				base.OnManagedReleaseResources();
			}
		}
		public override int On進行描画()
		{
			if( !base.bNotActivated )
			{
				if( base.b初めての進行描画 )
				{
					CDTXMania.Skin.soundゲーム終了音.t再生する();
					this.ct時間稼ぎ.t開始( 0, 1, 0x3e8, CDTXMania.Timer );
                    base.b初めての進行描画 = false;
				}
				this.ct時間稼ぎ.t進行();
				if( this.ct時間稼ぎ.b終了値に達した && !CDTXMania.Skin.soundゲーム終了音.b再生中 )
				{
					return 1;
				}
				if( this.tx背景 != null )
				{
					this.tx背景.tDraw2D( CDTXMania.app.Device, 0, 0 );
				}
			}
			return 0;
		}


		// その他

		#region [ private ]
		//-----------------
		private CCounter ct時間稼ぎ;
		private CTexture tx背景;
		//-----------------
		#endregion
	}
}
