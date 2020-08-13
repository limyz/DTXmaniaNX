using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using FDK;

namespace DTXMania
{
    public class CPad
	{
		// プロパティ

		internal STHIT st検知したデバイス;
		[StructLayout( LayoutKind.Sequential )]
		internal struct STHIT
		{
			public bool Keyboard;
			public bool MIDIIN;
			public bool Joypad;
			public bool Mouse;
			public void Clear()
			{
				this.Keyboard = false;
				this.MIDIIN = false;
				this.Joypad = false;
				this.Mouse = false;
			}
		}


		// コンストラクタ

		internal CPad( CConfigIni configIni, CInputManager mgrInput )
		{
			this.rConfigIni = configIni;
			this.rInput管理 = mgrInput;
			this.st検知したデバイス.Clear();
		}


		// メソッド

		public List<STInputEvent> GetEvents( EInstrumentPart part, Eパッド pad )
		{
			CConfigIni.CKeyAssign.STKEYASSIGN[] stkeyassignArray = this.rConfigIni.KeyAssign[ (int) part ][ (int) pad ];
			List<STInputEvent> list = new List<STInputEvent>();

			// すべての入力デバイスについて…
			foreach( IInputDevice device in this.rInput管理.listInputDevices )
			{
				if( ( device.list入力イベント != null ) && ( device.list入力イベント.Count != 0 ) )
				{
					foreach( STInputEvent event2 in device.list入力イベント )
					{
						for( int i = 0; i < stkeyassignArray.Length; i++ )
						{
							switch( stkeyassignArray[ i ].入力デバイス )
							{
								case E入力デバイス.キーボード:
									if( ( device.eInputDeviceType == EInputDeviceType.Keyboard ) && ( event2.nKey == stkeyassignArray[ i ].コード ) )
									{
										list.Add( event2 );
										this.st検知したデバイス.Keyboard = true;
									}
									break;

								case E入力デバイス.MIDI入力:
									if( ( ( device.eInputDeviceType == EInputDeviceType.MidiIn ) && ( device.ID == stkeyassignArray[ i ].ID ) ) && ( event2.nKey == stkeyassignArray[ i ].コード ) )
									{
										list.Add( event2 );
										this.st検知したデバイス.MIDIIN = true;
									}
									break;

								case E入力デバイス.ジョイパッド:
									if( ( ( device.eInputDeviceType == EInputDeviceType.Joystick ) && ( device.ID == stkeyassignArray[ i ].ID ) ) && ( event2.nKey == stkeyassignArray[ i ].コード ) )
									{
										list.Add( event2 );
										this.st検知したデバイス.Joypad = true;
									}
									break;

								case E入力デバイス.マウス:
									if( ( device.eInputDeviceType == EInputDeviceType.Mouse ) && ( event2.nKey == stkeyassignArray[ i ].コード ) )
									{
										list.Add( event2 );
										this.st検知したデバイス.Mouse = true;
									}
									break;
							}
						}
					}
					continue;
				}
			}
			return list;
		}
		public bool b押された( EInstrumentPart part, Eパッド pad )
		{
			if( part != EInstrumentPart.UNKNOWN )
			{
				
				CConfigIni.CKeyAssign.STKEYASSIGN[] stkeyassignArray = this.rConfigIni.KeyAssign[ (int) part ][ (int) pad ];
				for( int i = 0; i < stkeyassignArray.Length; i++ )
				{
					switch( stkeyassignArray[ i ].入力デバイス )
					{
						case E入力デバイス.キーボード:
							if( !this.rInput管理.Keyboard.bキーが押された( stkeyassignArray[ i ].コード ) )
								break;

							this.st検知したデバイス.Keyboard = true;
							return true;

						case E入力デバイス.MIDI入力:
							{
								IInputDevice device2 = this.rInput管理.MidiIn( stkeyassignArray[ i ].ID );
								if( ( device2 == null ) || !device2.bキーが押された( stkeyassignArray[ i ].コード ) )
									break;

								this.st検知したデバイス.MIDIIN = true;
								return true;
							}
						case E入力デバイス.ジョイパッド:
							{
								if( !this.rConfigIni.dicJoystick.ContainsKey( stkeyassignArray[ i ].ID ) )
									break;

								IInputDevice device = this.rInput管理.Joystick( stkeyassignArray[ i ].ID );
								if( ( device == null ) || !device.bキーが押された( stkeyassignArray[ i ].コード ) )
									break;

								this.st検知したデバイス.Joypad = true;
								return true;
							}
						case E入力デバイス.マウス:
							if( !this.rInput管理.Mouse.bキーが押された( stkeyassignArray[ i ].コード ) )
								break;

							this.st検知したデバイス.Mouse = true;
							return true;
					}
				}
			}
			return false;
		}
		public bool b押されたDGB( Eパッド pad )
		{
			if( !this.b押された( EInstrumentPart.DRUMS, pad ) && !this.b押された( EInstrumentPart.GUITAR, pad ) )
			{
				return this.b押された( EInstrumentPart.BASS, pad );
			}
			return true;
		}
		public bool b押されたGB( Eパッド pad )
		{
			if( !this.b押された( EInstrumentPart.GUITAR, pad ) )
			{
				return this.b押された( EInstrumentPart.BASS, pad );
			}
			return true;
		}
		public bool b押されている( EInstrumentPart part, Eパッド pad )
		{
			if( part != EInstrumentPart.UNKNOWN )
			{
				CConfigIni.CKeyAssign.STKEYASSIGN[] stkeyassignArray = this.rConfigIni.KeyAssign[ (int) part ][ (int) pad ];
				for( int i = 0; i < stkeyassignArray.Length; i++ )
				{
					switch( stkeyassignArray[ i ].入力デバイス )
					{
						case E入力デバイス.キーボード:
							if( !this.rInput管理.Keyboard.bキーが押されている( stkeyassignArray[ i ].コード ) )
							{
								break;
							}
							this.st検知したデバイス.Keyboard = true;
							return true;

						case E入力デバイス.ジョイパッド:
							{
								if( !this.rConfigIni.dicJoystick.ContainsKey( stkeyassignArray[ i ].ID ) )
								{
									break;
								}
								IInputDevice device = this.rInput管理.Joystick( stkeyassignArray[ i ].ID );
								if( ( device == null ) || !device.bキーが押されている( stkeyassignArray[ i ].コード ) )
								{
									break;
								}
								this.st検知したデバイス.Joypad = true;
								return true;
							}
						case E入力デバイス.マウス:
							if( !this.rInput管理.Mouse.bキーが押されている( stkeyassignArray[ i ].コード ) )
							{
								break;
							}
							this.st検知したデバイス.Mouse = true;
							return true;
					}
				}
			}
			return false;
		}
		public bool b押されているGB( Eパッド pad )
		{
			if( !this.b押されている( EInstrumentPart.GUITAR, pad ) )
			{
				return this.b押されている( EInstrumentPart.BASS, pad );
			}
			return true;
		}


		// その他

		#region [ private ]
		//-----------------
		private CConfigIni rConfigIni;
		private CInputManager rInput管理;
		//-----------------
		#endregion
	}
}
