using System;
using System.Collections.Generic;
using System.Text;

using WindowsKey = System.Windows.Forms.Keys;
using SlimDXKey = SlimDX.DirectInput.Key;
using SharpDXKey = SharpDX.DirectInput.Key;

namespace FDK
{
	public class DeviceConstantConverter
	{
		/// <returns>
		///		対応する値がなければ SlimDX.DirectInput.Unknown を返す。
		/// </returns>
		public static SlimDXKey DIKtoKey(SharpDXKey key)
		{
			if (_DIKtoKey.ContainsKey(key))
			{
				return _DIKtoKey[key];
			}
			else
			{
				return SlimDXKey.Unknown;
			}
		}

		/// <returns>
		///		対応する値がなければ System.Windows.Forms.Keys.None を返す。
		/// </returns>
		public static WindowsKey KeyToKeys(SlimDXKey key)
		{
			if (_KeyToKeys.ContainsKey(key))
			{
				return _KeyToKeys[key];
			}
			else
			{
				return WindowsKey.None;
			}
		}


		/// <summary>
		///		DIK (SharpDX.DirectInput.Key) から SlimDX.DirectInput.Key への変換表。
		/// </summary>
		private static readonly Dictionary<SharpDXKey, SlimDXKey> _DIKtoKey = new Dictionary<SharpDXKey, SlimDXKey>() {
			#region [ *** ]
			{ SharpDXKey.Unknown, SlimDXKey.Unknown },
			{ SharpDXKey.Escape, SlimDXKey.Escape },
			{ SharpDXKey.D1, SlimDXKey.D1 },
			{ SharpDXKey.D2, SlimDXKey.D2 },
			{ SharpDXKey.D3, SlimDXKey.D3 },
			{ SharpDXKey.D4, SlimDXKey.D4 },
			{ SharpDXKey.D5, SlimDXKey.D5 },
			{ SharpDXKey.D6, SlimDXKey.D6 },
			{ SharpDXKey.D7, SlimDXKey.D7 },
			{ SharpDXKey.D8, SlimDXKey.D8 },
			{ SharpDXKey.D9, SlimDXKey.D9 },
			{ SharpDXKey.D0, SlimDXKey.D0 },
			{ SharpDXKey.Minus, SlimDXKey.Minus },
			{ SharpDXKey.Equals, SlimDXKey.Equals },
			{ SharpDXKey.Back, SlimDXKey.Backspace },
			{ SharpDXKey.Tab, SlimDXKey.Tab },
			{ SharpDXKey.Q, SlimDXKey.Q },
			{ SharpDXKey.W, SlimDXKey.W },
			{ SharpDXKey.E, SlimDXKey.E },
			{ SharpDXKey.R, SlimDXKey.R },
			{ SharpDXKey.T, SlimDXKey.T },
			{ SharpDXKey.Y, SlimDXKey.Y },
			{ SharpDXKey.U, SlimDXKey.U },
			{ SharpDXKey.I, SlimDXKey.I },
			{ SharpDXKey.O, SlimDXKey.O },
			{ SharpDXKey.P, SlimDXKey.P },
			{ SharpDXKey.LeftBracket, SlimDXKey.LeftBracket },
			{ SharpDXKey.RightBracket, SlimDXKey.RightBracket },
			{ SharpDXKey.Return, SlimDXKey.Return },
			{ SharpDXKey.LeftControl, SlimDXKey.LeftControl },
			{ SharpDXKey.A, SlimDXKey.A },
			{ SharpDXKey.S, SlimDXKey.S },
			{ SharpDXKey.D, SlimDXKey.D },
			{ SharpDXKey.F, SlimDXKey.F },
			{ SharpDXKey.G, SlimDXKey.G },
			{ SharpDXKey.H, SlimDXKey.H },
			{ SharpDXKey.J, SlimDXKey.J },
			{ SharpDXKey.K, SlimDXKey.K },
			{ SharpDXKey.L, SlimDXKey.L },
			{ SharpDXKey.Semicolon, SlimDXKey.Semicolon },
			{ SharpDXKey.Apostrophe, SlimDXKey.Apostrophe },
			{ SharpDXKey.Grave, SlimDXKey.Grave },
			{ SharpDXKey.LeftShift, SlimDXKey.LeftShift },
			{ SharpDXKey.Backslash, SlimDXKey.Backslash },
			{ SharpDXKey.Z, SlimDXKey.Z },
			{ SharpDXKey.X, SlimDXKey.X },
			{ SharpDXKey.C, SlimDXKey.C },
			{ SharpDXKey.V, SlimDXKey.V },
			{ SharpDXKey.B, SlimDXKey.B },
			{ SharpDXKey.N, SlimDXKey.N },
			{ SharpDXKey.M, SlimDXKey.M },
			{ SharpDXKey.Comma, SlimDXKey.Comma },
			{ SharpDXKey.Period, SlimDXKey.Period },
			{ SharpDXKey.Slash, SlimDXKey.Slash },
			{ SharpDXKey.RightShift, SlimDXKey.RightShift },
			{ SharpDXKey.Multiply, SlimDXKey.NumberPadStar },
			{ SharpDXKey.LeftAlt, SlimDXKey.LeftAlt },
			{ SharpDXKey.Space, SlimDXKey.Space },
			{ SharpDXKey.Capital, SlimDXKey.CapsLock },
			{ SharpDXKey.F1, SlimDXKey.F1 },
			{ SharpDXKey.F2, SlimDXKey.F2 },
			{ SharpDXKey.F3, SlimDXKey.F3 },
			{ SharpDXKey.F4, SlimDXKey.F4 },
			{ SharpDXKey.F5, SlimDXKey.F5 },
			{ SharpDXKey.F6, SlimDXKey.F6 },
			{ SharpDXKey.F7, SlimDXKey.F7 },
			{ SharpDXKey.F8, SlimDXKey.F8 },
			{ SharpDXKey.F9, SlimDXKey.F9 },
			{ SharpDXKey.F10, SlimDXKey.F10 },
			{ SharpDXKey.NumberLock, SlimDXKey.NumberLock },
			{ SharpDXKey.ScrollLock, SlimDXKey.ScrollLock },
			{ SharpDXKey.NumberPad7, SlimDXKey.NumberPad7 },
			{ SharpDXKey.NumberPad8, SlimDXKey.NumberPad8 },
			{ SharpDXKey.NumberPad9, SlimDXKey.NumberPad9 },
			{ SharpDXKey.Subtract, SlimDXKey.NumberPadMinus },
			{ SharpDXKey.NumberPad4, SlimDXKey.NumberPad4 },
			{ SharpDXKey.NumberPad5, SlimDXKey.NumberPad5 },
			{ SharpDXKey.NumberPad6, SlimDXKey.NumberPad6 },
			{ SharpDXKey.Add, SlimDXKey.NumberPadPlus },
			{ SharpDXKey.NumberPad1, SlimDXKey.NumberPad1 },
			{ SharpDXKey.NumberPad2, SlimDXKey.NumberPad2 },
			{ SharpDXKey.NumberPad3, SlimDXKey.NumberPad3 },
			{ SharpDXKey.NumberPad0, SlimDXKey.NumberPad0 },
			{ SharpDXKey.Decimal, SlimDXKey.NumberPadPeriod },
			{ SharpDXKey.Oem102, SlimDXKey.Oem102 },
			{ SharpDXKey.F11, SlimDXKey.F11 },
			{ SharpDXKey.F12, SlimDXKey.F12 },
			{ SharpDXKey.F13, SlimDXKey.F13 },
			{ SharpDXKey.F14, SlimDXKey.F14 },
			{ SharpDXKey.F15, SlimDXKey.F15 },
			{ SharpDXKey.Kana, SlimDXKey.Kana },
			{ SharpDXKey.AbntC1, SlimDXKey.AbntC1 },
			{ SharpDXKey.Convert, SlimDXKey.Convert },
			{ SharpDXKey.NoConvert, SlimDXKey.NoConvert },
			{ SharpDXKey.Yen, SlimDXKey.Yen },
			{ SharpDXKey.AbntC2, SlimDXKey.AbntC2 },
			{ SharpDXKey.NumberPadEquals, SlimDXKey.NumberPadEquals },
			{ SharpDXKey.PreviousTrack, SlimDXKey.PreviousTrack },
			{ SharpDXKey.AT, SlimDXKey.AT },
			{ SharpDXKey.Colon, SlimDXKey.Colon },
			{ SharpDXKey.Underline, SlimDXKey.Underline },
			{ SharpDXKey.Kanji, SlimDXKey.Kanji },
			{ SharpDXKey.Stop, SlimDXKey.Stop },
			{ SharpDXKey.AX, SlimDXKey.AX },
			{ SharpDXKey.Unlabeled, SlimDXKey.Unlabeled },
			{ SharpDXKey.NextTrack, SlimDXKey.NextTrack },
			{ SharpDXKey.NumberPadEnter, SlimDXKey.NumberPadEnter },
			{ SharpDXKey.RightControl, SlimDXKey.RightControl },
			{ SharpDXKey.Mute, SlimDXKey.Mute },
			{ SharpDXKey.Calculator, SlimDXKey.Calculator },
			{ SharpDXKey.PlayPause, SlimDXKey.PlayPause },
			{ SharpDXKey.MediaStop, SlimDXKey.MediaStop },
			{ SharpDXKey.VolumeDown, SlimDXKey.VolumeDown },
			{ SharpDXKey.VolumeUp, SlimDXKey.VolumeUp },
			{ SharpDXKey.WebHome, SlimDXKey.WebHome },
			{ SharpDXKey.PrintScreen, SlimDXKey.PrintScreen },
			{ SharpDXKey.RightAlt, SlimDXKey.RightAlt },
			{ SharpDXKey.Pause, SlimDXKey.Pause },
			{ SharpDXKey.Home, SlimDXKey.Home },
			{ SharpDXKey.Up, SlimDXKey.UpArrow },
			{ SharpDXKey.PageUp, SlimDXKey.PageUp },
			{ SharpDXKey.Left, SlimDXKey.LeftArrow },
			{ SharpDXKey.Right, SlimDXKey.RightArrow },
			{ SharpDXKey.End, SlimDXKey.End },
			{ SharpDXKey.Down, SlimDXKey.DownArrow },
			{ SharpDXKey.PageDown, SlimDXKey.PageDown },
			{ SharpDXKey.Insert, SlimDXKey.Insert },
			{ SharpDXKey.Delete, SlimDXKey.Delete },
			{ SharpDXKey.LeftWindowsKey, SlimDXKey.LeftWindowsKey },
			{ SharpDXKey.RightWindowsKey, SlimDXKey.RightWindowsKey },
			{ SharpDXKey.Applications, SlimDXKey.Applications },
			{ SharpDXKey.Power, SlimDXKey.Power },
			{ SharpDXKey.Sleep, SlimDXKey.Sleep },
			{ SharpDXKey.Wake, SlimDXKey.Wake },
			{ SharpDXKey.WebSearch, SlimDXKey.WebSearch },
			{ SharpDXKey.WebFavorites, SlimDXKey.WebFavorites },
			{ SharpDXKey.WebRefresh, SlimDXKey.WebRefresh },
			{ SharpDXKey.WebStop, SlimDXKey.WebStop },
			{ SharpDXKey.WebForward, SlimDXKey.WebForward },
			{ SharpDXKey.WebBack, SlimDXKey.WebBack },
			{ SharpDXKey.MyComputer, SlimDXKey.MyComputer },
			{ SharpDXKey.Mail, SlimDXKey.Mail },
			{ SharpDXKey.MediaSelect, SlimDXKey.MediaSelect },
			#endregion
		};

		/// <summary>
		///		SlimDX.DirectInput.Key から System.Windows.Form.Keys への変換表。
		/// </summary>
		private static readonly Dictionary<SlimDXKey, WindowsKey> _KeyToKeys = new Dictionary<SlimDXKey, WindowsKey>() {
			#region [ *** ]
			{ SlimDXKey.D0, WindowsKey.D0 },
			{ SlimDXKey.D1, WindowsKey.D1 },
			{ SlimDXKey.D2, WindowsKey.D2 },
			{ SlimDXKey.D3, WindowsKey.D3 },
			{ SlimDXKey.D4, WindowsKey.D4 },
			{ SlimDXKey.D5, WindowsKey.D5 },
			{ SlimDXKey.D6, WindowsKey.D6 },
			{ SlimDXKey.D7, WindowsKey.D7 },
			{ SlimDXKey.D8, WindowsKey.D8 },
			{ SlimDXKey.D9, WindowsKey.D9 },
			{ SlimDXKey.A, WindowsKey.A },
			{ SlimDXKey.B, WindowsKey.B },
			{ SlimDXKey.C, WindowsKey.C },
			{ SlimDXKey.D, WindowsKey.D },
			{ SlimDXKey.E, WindowsKey.E },
			{ SlimDXKey.F, WindowsKey.F },
			{ SlimDXKey.G, WindowsKey.G },
			{ SlimDXKey.H, WindowsKey.H },
			{ SlimDXKey.I, WindowsKey.I },
			{ SlimDXKey.J, WindowsKey.J },
			{ SlimDXKey.K, WindowsKey.K },
			{ SlimDXKey.L, WindowsKey.L },
			{ SlimDXKey.M, WindowsKey.M },
			{ SlimDXKey.N, WindowsKey.N },
			{ SlimDXKey.O, WindowsKey.O },
			{ SlimDXKey.P, WindowsKey.P },
			{ SlimDXKey.Q, WindowsKey.Q },
			{ SlimDXKey.R, WindowsKey.R },
			{ SlimDXKey.S, WindowsKey.S },
			{ SlimDXKey.T, WindowsKey.T },
			{ SlimDXKey.U, WindowsKey.U },
			{ SlimDXKey.V, WindowsKey.V },
			{ SlimDXKey.W, WindowsKey.W },
			{ SlimDXKey.X, WindowsKey.X },
			{ SlimDXKey.Y, WindowsKey.Y },
			{ SlimDXKey.Z, WindowsKey.Z },
			//{ SlimDXKey.AbntC1, WindowsKey.A },
			//{ SlimDXKey.AbntC2, WindowsKey.A },
			{ SlimDXKey.Apostrophe, WindowsKey.OemQuotes },
			{ SlimDXKey.Applications, WindowsKey.Apps },
			{ SlimDXKey.AT, WindowsKey.Oem3 },	// OemTilde と同値
			//{ SlimDXKey.AX, WindowsKey.A },	// OemAX (225) は未定義
			{ SlimDXKey.Backspace, WindowsKey.Back },
			{ SlimDXKey.Backslash, WindowsKey.OemBackslash },
			//{ SlimDXKey.Calculator, WindowsKey.A },
			{ SlimDXKey.CapsLock, WindowsKey.CapsLock },
			{ SlimDXKey.Colon, WindowsKey.Oem1 },
			{ SlimDXKey.Comma, WindowsKey.Oemcomma },
			{ SlimDXKey.Convert, WindowsKey.IMEConvert },
			{ SlimDXKey.Delete, WindowsKey.Delete },
			{ SlimDXKey.DownArrow, WindowsKey.Down },
			{ SlimDXKey.End, WindowsKey.End },
			{ SlimDXKey.Equals, WindowsKey.A },		// ?
			{ SlimDXKey.Escape, WindowsKey.Escape },
			{ SlimDXKey.F1, WindowsKey.F1 },
			{ SlimDXKey.F2, WindowsKey.F2 },
			{ SlimDXKey.F3, WindowsKey.F3 },
			{ SlimDXKey.F4, WindowsKey.F4 },
			{ SlimDXKey.F5, WindowsKey.F5 },
			{ SlimDXKey.F6, WindowsKey.F6 },
			{ SlimDXKey.F7, WindowsKey.F7 },
			{ SlimDXKey.F8, WindowsKey.F8 },
			{ SlimDXKey.F9, WindowsKey.F9 },
			{ SlimDXKey.F10, WindowsKey.F10 },
			{ SlimDXKey.F11, WindowsKey.F11 },
			{ SlimDXKey.F12, WindowsKey.F12 },
			{ SlimDXKey.F13, WindowsKey.F13 },
			{ SlimDXKey.F14, WindowsKey.F14 },
			{ SlimDXKey.F15, WindowsKey.F15 },
			{ SlimDXKey.Grave, WindowsKey.A },		// ?
			{ SlimDXKey.Home, WindowsKey.Home },
			{ SlimDXKey.Insert, WindowsKey.Insert },
			{ SlimDXKey.Kana, WindowsKey.KanaMode },
			{ SlimDXKey.Kanji, WindowsKey.KanjiMode },
			{ SlimDXKey.LeftBracket, WindowsKey.Oem4 },
			{ SlimDXKey.LeftControl, WindowsKey.LControlKey },
			{ SlimDXKey.LeftArrow, WindowsKey.Left },
			{ SlimDXKey.LeftAlt, WindowsKey.LMenu },
			{ SlimDXKey.LeftShift, WindowsKey.LShiftKey },
			{ SlimDXKey.LeftWindowsKey, WindowsKey.LWin },
			{ SlimDXKey.Mail, WindowsKey.LaunchMail },
			{ SlimDXKey.MediaSelect, WindowsKey.SelectMedia },
			{ SlimDXKey.MediaStop, WindowsKey.MediaStop },
			{ SlimDXKey.Minus, WindowsKey.OemMinus },
			{ SlimDXKey.Mute, WindowsKey.VolumeMute },
			{ SlimDXKey.MyComputer, WindowsKey.A },		// ?
			{ SlimDXKey.NextTrack, WindowsKey.MediaNextTrack },
			{ SlimDXKey.NoConvert, WindowsKey.IMENonconvert },
			{ SlimDXKey.NumberLock, WindowsKey.NumLock },
			{ SlimDXKey.NumberPad0, WindowsKey.NumPad0 },
			{ SlimDXKey.NumberPad1, WindowsKey.NumPad1 },
			{ SlimDXKey.NumberPad2, WindowsKey.NumPad2 },
			{ SlimDXKey.NumberPad3, WindowsKey.NumPad3 },
			{ SlimDXKey.NumberPad4, WindowsKey.NumPad4 },
			{ SlimDXKey.NumberPad5, WindowsKey.NumPad5 },
			{ SlimDXKey.NumberPad6, WindowsKey.NumPad6 },
			{ SlimDXKey.NumberPad7, WindowsKey.NumPad7 },
			{ SlimDXKey.NumberPad8, WindowsKey.NumPad8 },
			{ SlimDXKey.NumberPad9, WindowsKey.NumPad9 },
			{ SlimDXKey.NumberPadComma, WindowsKey.Separator },
			{ SlimDXKey.NumberPadEnter, WindowsKey.A },		// ?
			{ SlimDXKey.NumberPadEquals, WindowsKey.A },		// ?
			{ SlimDXKey.NumberPadMinus, WindowsKey.Subtract },
			{ SlimDXKey.NumberPadPeriod, WindowsKey.Decimal },
			{ SlimDXKey.NumberPadPlus, WindowsKey.Add },
			{ SlimDXKey.NumberPadSlash, WindowsKey.Divide },
			{ SlimDXKey.NumberPadStar, WindowsKey.Multiply },
			{ SlimDXKey.Oem102, WindowsKey.Oem102 },
			{ SlimDXKey.PageDown, WindowsKey.PageDown },
			{ SlimDXKey.PageUp, WindowsKey.PageUp },
			{ SlimDXKey.Pause, WindowsKey.Pause },
			{ SlimDXKey.Period, WindowsKey.OemPeriod },
			{ SlimDXKey.PlayPause, WindowsKey.MediaPlayPause },
			{ SlimDXKey.Power, WindowsKey.A },		// ?
			{ SlimDXKey.PreviousTrack, WindowsKey.MediaPreviousTrack },
			{ SlimDXKey.RightBracket, WindowsKey.Oem6 },
			{ SlimDXKey.RightControl, WindowsKey.RControlKey },
			{ SlimDXKey.Return, WindowsKey.Return },
			{ SlimDXKey.RightArrow, WindowsKey.Right },
			{ SlimDXKey.RightAlt, WindowsKey.RMenu },
			{ SlimDXKey.RightShift, WindowsKey.A },		// ?
			{ SlimDXKey.RightWindowsKey, WindowsKey.RWin },
			{ SlimDXKey.ScrollLock, WindowsKey.Scroll },
			{ SlimDXKey.Semicolon, WindowsKey.Oemplus    },	// OemSemicolon じゃなくて？
			{ SlimDXKey.Slash, WindowsKey.Oem2 },
			{ SlimDXKey.Sleep, WindowsKey.Sleep },
			{ SlimDXKey.Space, WindowsKey.Space },
			{ SlimDXKey.Stop, WindowsKey.MediaStop },
			{ SlimDXKey.PrintScreen, WindowsKey.PrintScreen },
			{ SlimDXKey.Tab, WindowsKey.Tab },
			{ SlimDXKey.Underline, WindowsKey.Oem102 },
			//{ SlimDXKey.Unlabeled, WindowsKey.A },		// ?
			{ SlimDXKey.UpArrow, WindowsKey.Up },
			{ SlimDXKey.VolumeDown, WindowsKey.VolumeDown },
			{ SlimDXKey.VolumeUp, WindowsKey.VolumeUp },
			{ SlimDXKey.Wake, WindowsKey.A },		// ?
			{ SlimDXKey.WebBack, WindowsKey.BrowserBack },
			{ SlimDXKey.WebFavorites, WindowsKey.BrowserFavorites },
			{ SlimDXKey.WebForward, WindowsKey.BrowserForward },
			{ SlimDXKey.WebHome, WindowsKey.BrowserHome },
			{ SlimDXKey.WebRefresh, WindowsKey.BrowserRefresh },
			{ SlimDXKey.WebSearch, WindowsKey.BrowserSearch },
			{ SlimDXKey.WebStop, WindowsKey.BrowserStop },
			{ SlimDXKey.Yen, WindowsKey.OemBackslash },
			#endregion
		};
	}
}
