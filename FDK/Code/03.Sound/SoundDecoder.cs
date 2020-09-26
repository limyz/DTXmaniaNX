using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace FDK
{
	/// <summary>
	/// xa,oggデコード用の基底クラス
	/// </summary>
	public abstract class SoundDecoder //: IDisposable
	{
		public long nTotalPCMSize { get; protected set; }
		public CWin32.WAVEFORMATEX wfx { get; protected set; }

		public abstract int Open( string filename );
		public abstract int Decode(ref byte[] Dest, long offset);
		public abstract void Close();
	}
}
