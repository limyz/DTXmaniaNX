using System;
using System.Collections.Generic;
using System.Text;

namespace FDK
{
	public interface IInputDevice : IDisposable
	{
		// プロパティ

		EInputDeviceType eInputDeviceType
		{
			get;
		}
		string GUID 
		{
			get; 
		}
		int ID 
		{
			get;
		}
		List<STInputEvent> listInputEvent
		{
			get;
		}
		string strDeviceName
		{
			get;
		}


		// Method interfaces

		void tPolling( bool bWindowがアクティブ中, bool bバッファ入力を使用する );  // tポーリング
		bool bKeyPressed( int nKey );  // bキーが押された
		bool bKeyPressing( int nKey );  // bキーが押されている
		bool bKeyReleased( int nKey );  // bキーが離された
		bool bKeyReleasing( int nKey );  // bキーが離されている
	}
}
