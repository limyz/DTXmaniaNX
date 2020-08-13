using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using FDK;
using System.Runtime.Serialization.Formatters.Binary;


namespace DTXMania
{
	/// <summary>
	/// box.defによるスキン変更時に一時的に遷移する、スキン画像の一切無いステージ。
	/// </summary>
	internal class CStageChangeSkin : CStage
	{
		// コンストラクタ

		public CStageChangeSkin()
		{
			base.eステージID = CStage.EStage.ChangeSkin;
			base.bNotActivated = true;
		}


		// CStage 実装

		public override void OnActivate()
		{
			Trace.TraceInformation( "スキン変更ステージを活性化します。" );
			Trace.Indent();
			try
			{
				base.OnActivate();
				Trace.TraceInformation( "スキン変更ステージの活性化を完了しました。" );
			}
			finally
			{
				Trace.Unindent();
			}
		}
		public override void OnDeactivate()
		{
			Trace.TraceInformation( "スキン変更ステージを非活性化します。" );
			Trace.Indent();
			try
			{
				base.OnDeactivate();
				Trace.TraceInformation( "スキン変更ステージの非活性化を完了しました。" );
			}
			finally
			{
				Trace.Unindent();
			}
		}
		public override void OnManagedCreateResources()
		{
			if( !base.bNotActivated )
			{
				base.OnManagedCreateResources();
			}
		}
		public override void OnManagedReleaseResources()
		{
			if( !base.bNotActivated )
			{
				base.OnManagedReleaseResources();
			}
		}
		public override int On進行描画()
		{
			if( !base.bNotActivated )
			{
				if ( base.b初めての進行描画 )
				{
					base.b初めての進行描画 = false;
					return 0;
				}

				//スキン変更処理
				tChangeSkinMain();
				return 1;
			}
			return 0;
		}
		public void tChangeSkinMain()
		{
			Trace.TraceInformation( "スキン変更:" + CDTXMania.Skin.GetCurrentSkinSubfolderFullName( false ) );

			CDTXMania.act文字コンソール.OnDeactivate();

			CDTXMania.Skin.PrepareReloadSkin();
			CDTXMania.Skin.ReloadSkin();

			CDTXMania.act文字コンソール.OnActivate();
		}
	}
}
