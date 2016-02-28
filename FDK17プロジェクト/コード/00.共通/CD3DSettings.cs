using System;
using System.Collections.Generic;
using System.Text;
using SlimDX.Direct3D9;

namespace FDK
{
	public class CD3DSettings : ICloneable
	{
		// プロパティ

		public int nAdaptor
		{
			get;
			set;
		}
		public DeviceType DeviceType
		{
			get;
			set;
		}
		public CreateFlags CreateFlags
		{
			get;
			set;
		}
		public PresentParameters PresentParameters
		{
			get;
			set;
		}
		public bool VSyncWait
		{
			get
			{
				return ( this.PresentParameters.PresentationInterval == PresentInterval.Default );
			}
			set
			{
				this.PresentParameters.PresentationInterval = ( value ) ? PresentInterval.Default : PresentInterval.Immediate;
			}
		}


		// コンストラクタ

		public CD3DSettings()
		{
			this.nAdaptor = 0;
			this.DeviceType = DeviceType.Hardware;
			this.CreateFlags = CreateFlags.HardwareVertexProcessing;
			this.PresentParameters = new PresentParameters() {
				BackBufferCount = 1,
				BackBufferFormat = Format.X8R8G8B8,
				BackBufferWidth = 1280,
				BackBufferHeight = 720,
				Multisample = MultisampleType.None,
				SwapEffect = SwapEffect.Discard,
				EnableAutoDepthStencil = true,
				AutoDepthStencilFormat = Format.D16,
				PresentFlags = PresentFlags.DiscardDepthStencil,
				PresentationInterval = PresentInterval.Default,
				Windowed = true,
				DeviceWindowHandle = IntPtr.Zero,
			};
		}
		
		
		// メソッド

		public bool bデバイスの再生成が不要でリセットだけで済む( CD3DSettings newSettings )
		{
			return
				this.nAdaptor == newSettings.nAdaptor &&		// アダプタ番号が同じ　かつ
				this.DeviceType == newSettings.DeviceType &&	// デバイスタイプが同じ　かつ
				this.CreateFlags == newSettings.CreateFlags;	// 作成フラグが同じ　である場合、リセットだけで済む。
		}

		#region [ ICloneable 実装 ]
		//-----------------
		public CD3DSettings Clone()
		{
			var clone = new CD3DSettings() {
				nAdaptor = this.nAdaptor,
				DeviceType = this.DeviceType, 
				CreateFlags = this.CreateFlags,
			};
			
			if( this.PresentParameters != null )
				clone.PresentParameters = this.PresentParameters.Clone();

			return clone;
		}
		object ICloneable.Clone()
		{
			return (CD3DSettings) ( this.Clone() );
		}
		//-----------------
		#endregion
	}
}
