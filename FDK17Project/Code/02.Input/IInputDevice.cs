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


		// Method interfaces

		void tPolling( bool bWindowがアクティブ中, bool bバッファ入力を使用する );
		bool bKeyPressed( int nKey );
		bool bKeyPressing( int nKey );
		bool bKeyReleased( int nKey );
		bool bKeyReleasing( int nKey );
	}
}
