using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace FDK
{
	public class CInputMIDI : IInputDevice, IDisposable
	{
		// プロパティ

		public IntPtr hMidiIn;
		public List<STInputEvent> listEventBuffer;


		// コンストラクタ

		public CInputMIDI( uint nID )
		{
			this.hMidiIn = IntPtr.Zero;
			this.listEventBuffer = new List<STInputEvent>( 32 );
			this.listInputEvent = new List<STInputEvent>( 32 );
			this.eInputDeviceType = EInputDeviceType.MidiIn;
			this.GUID = "";
			this.ID = (int) nID;
			this.strDeviceName = "";    // CInput管理で初期化する
		}


		// メソッド

		public void tメッセージからMIDI信号のみ受信( uint wMsg, int dwInstance, int dwParam1, int dwParam2, long n受信システム時刻 )
		{
			if( wMsg == CWin32.MIM_DATA )
			{
				int nMIDIevent = dwParam1 & 0xF0;
				int nPara1 = ( dwParam1 >> 8 ) & 0xFF;
				int nPara2 = ( dwParam1 >> 16 ) & 0xFF;

// Trace.TraceInformation( "MIDIevent={0:X2} para1={1:X2} para2={2:X2}", nMIDIevent, nPara1, nPara2 );
			
				if( ( nMIDIevent == 0x90 ) && ( nPara2 != 0 ) )
				{
					STInputEvent item = new STInputEvent();
					item.nKey = nPara1;
					item.b押された = true;
					item.nTimeStamp = n受信システム時刻;
					item.nVelocity = nPara2;
					this.listEventBuffer.Add( item );
				}
			}
		}

		#region [ IInputDevice 実装 ]
		//-----------------
		public EInputDeviceType eInputDeviceType { get; private set; }
		public string GUID { get; private set; }
		public int ID { get; private set; }
		public List<STInputEvent> listInputEvent { get; private set; }
		public string strDeviceName { get; set; }

		public void tPolling( bool bWindowがアクティブ中, bool bバッファ入力を使用する )
		{
			// this.listInputEvent = new List<STInputEvent>( 32 );
			this.listInputEvent.Clear();								// #xxxxx 2012.6.11 yyagi; To optimize, I removed new();

			for( int i = 0; i < this.listEventBuffer.Count; i++ )
				this.listInputEvent.Add( this.listEventBuffer[ i ] );

			this.listEventBuffer.Clear();
		}
		public bool bKeyPressed( int nKey )
		{
			foreach( STInputEvent event2 in this.listInputEvent )
			{
				if( ( event2.nKey == nKey ) && event2.b押された )
				{
					return true;
				}
			}
			return false;
		}
		public bool bKeyPressing( int nKey )
		{
			return false;
		}
		public bool bKeyReleased( int nKey )
		{
			return false;
		}
		public bool bKeyReleasing( int nKey )
		{
			return false;
		}
		//-----------------
		#endregion

		#region [ IDisposable 実装 ]
		//-----------------
		public void Dispose()
		{
			if ( this.listEventBuffer != null )
			{
				this.listEventBuffer = null;
			}
			if ( this.listInputEvent != null )
			{
				this.listInputEvent = null;
			}
		}
		//-----------------
		#endregion
	}
}
