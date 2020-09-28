using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using SharpDX.Direct3D9;
using FDK;

namespace DTXMania
{
	internal class CPluginHost : IPluginHost
	{
		// コンストラクタ

		public CPluginHost()
		{
			this._DTXManiaVersion = new CDTXVersion( CDTXMania.VERSION );
		}


		// IPluginHost 実装

		public CDTXVersion DTXManiaVersion
		{
			get { return this._DTXManiaVersion; }
		}
		public Device D3D9Device
		{
			get { return (CDTXMania.app != null ) ? CDTXMania.app.Device : null; }
		}
		public Format TextureFormat
		{
			get { return CDTXMania.TextureFormat; }
		}
		public CTimer Timer
		{
			get { return CDTXMania.Timer; }
		}
		public CSoundManager Sound管理
		{
			get { return CDTXMania.SoundManager; }
		}
		public Size ClientSize
		{
			get { return CDTXMania.app.Window.ClientSize; }
		}
		public CStage.EStage e現在のステージ
		{
			get { return ( CDTXMania.rCurrentStage != null ) ? CDTXMania.rCurrentStage.eStageID : CStage.EStage.DoNothing; }
		}
		public CStage.EPhase e現在のフェーズ
		{
			get { return ( CDTXMania.rCurrentStage != null ) ? CDTXMania.rCurrentStage.ePhaseID : CStage.EPhase.Common_DefaultState; }
		}
		public bool t入力を占有する(IPluginActivity act)
		{
			if (CDTXMania.actPluginOccupyingInput != null)
				return false;

			CDTXMania.actPluginOccupyingInput = act;
			return true;
		}
		public bool t入力の占有を解除する(IPluginActivity act)
		{
			if (CDTXMania.actPluginOccupyingInput == null || CDTXMania.actPluginOccupyingInput != act)
				return false;

			CDTXMania.actPluginOccupyingInput = null;
			return true;
		}
		public void tシステムサウンドを再生する( ESystemSound sound )
		{
			if( CDTXMania.Skin != null )
				CDTXMania.Skin[ sound ].tPlay();
		}
		
		
		// Other

		#region [ private ]
		//-----------------
		private CDTXVersion _DTXManiaVersion;
		//-----------------
		#endregion
	}
}
