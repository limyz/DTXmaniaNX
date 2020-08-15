using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using FDK;

namespace DTXMania
{
	internal class CStageEnd : CStage
	{
		// Constructor

		public CStageEnd()
		{
			base.eStageID = CStage.EStage.End;
			base.ePhaseID = CStage.EPhase.Common_DefaultState;
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
				this.txBackground = CDTXMania.tGenerateTexture( CSkin.Path( @"Graphics\9_background.jpg" ), false );
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
				CDTXMania.tReleaseTexture( ref this.txBackground );
				base.OnManagedReleaseResources();
			}
		}
		public override int OnUpdateAndDraw()
		{
			if( !base.bNotActivated )
			{
				if( base.bJustStartedUpdate )
				{
					CDTXMania.Skin.soundGameEnd.tPlay();
					this.ct時間稼ぎ.tStart( 0, 1, 0x3e8, CDTXMania.Timer );
                    base.bJustStartedUpdate = false;
				}
				this.ct時間稼ぎ.tUpdate();
				if( this.ct時間稼ぎ.bReachedEndValue && !CDTXMania.Skin.soundGameEnd.b再生中 )
				{
					return 1;
				}
				if( this.txBackground != null )
				{
					this.txBackground.tDraw2D( CDTXMania.app.Device, 0, 0 );
				}
			}
			return 0;
		}


		// Other

		#region [ private ]
		//-----------------
		private CCounter ct時間稼ぎ;
		private CTexture txBackground;
		//-----------------
		#endregion
	}
}
