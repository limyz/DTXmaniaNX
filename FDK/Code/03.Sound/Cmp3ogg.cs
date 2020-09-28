using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Un4seen.Bass;
using Un4seen.Bass.Misc;


namespace FDK
{
	public unsafe class Cmp3ogg : SoundDecoder
	{
		private int stream_in = -1;


		public override int Open( string filename )
		{
			bool r = Bass.BASS_Init(0, 48000, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);

			stream_in = Bass.BASS_StreamCreateFile(filename, 0, 0, BASSFlag.BASS_DEFAULT | BASSFlag.BASS_STREAM_DECODE);
			// BASS_DEFAULT: output 32bit (16bit stereo)
			if (stream_in == 0)
			{
				BASSError be = Bass.BASS_ErrorGetCode();
				Trace.TraceInformation("Cmp3ogg: StreamCreateFile error: " + be.ToString());
			}
			nTotalPCMSize = Bass.BASS_ChannelGetLength(stream_in);

			#region [ Getting WAVEFORMEX info ]
			var chinfo = Bass.BASS_ChannelGetInfo(stream_in);
			wfx = new CWin32.WAVEFORMATEX(
				(ushort)1,								// wFormatTag
				(ushort)chinfo.chans,					// nChannels
				(uint)chinfo.freq,						// nSamplesPerSec
				(uint)(chinfo.freq * 2 * chinfo.chans),	// nAvgBytesPerSec
				(ushort)(2 * chinfo.chans),				// nBlockAlign
				16,										// wBitsPerSample
				0										// cbSize				
			);
			#endregion

			//string fn = Path.GetFileName(filename); 
			//Trace.TraceInformation("filename=" + fn + ", size=(decode): " + wavdata.Length + ", channelgetlength=" + _TotalPCMSize2 + ", " + _TotalPCMSize) ;

			return 0;
		}

		public override int Decode( ref byte[] Dest, long offset )
		{
			#region [ decode ]
			int LEN = 65536;
			byte[] data = new byte[LEN]; // 2 x 16-bit and length in is bytes
			int len = 0;
			long p = 0;
			do
			{
				len = Bass.BASS_ChannelGetData(stream_in, data, LEN);
				if (len < 0)
				{
					BASSError be = Bass.BASS_ErrorGetCode();
					Trace.TraceInformation("Cmp3: BASS_ChannelGetData Error: " + be.ToString());
				}
				Array.Copy(data, 0, Dest, p, len);
				p += len;
			} while (p < nTotalPCMSize);
			#endregion

//SaveWav(filename, Dest);

			data = null;
			return 0;
		}

		public override void Close()
		{
			Bass.BASS_StreamFree(stream_in);
			Bass.BASS_Free();
		}

		/// <summary>
		/// save wav file (for debugging)
		/// </summary>
		/// <param name="filename">input mp3/xa filename</param>
		private void SaveWav(string filename, byte[] Dest)
		{
			string outfile = Path.GetFileName(filename);
			var fs = new FileStream(outfile + ".wav", FileMode.Create);
			var st = new BinaryWriter(fs);

			st.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });      // 'RIFF'
			st.Write((int)(nTotalPCMSize + 44 - 8));      // filesize - 8 [byte]；今は不明なので後で上書きする。
			st.Write(new byte[] { 0x57, 0x41, 0x56, 0x45 });      // 'WAVE'
			st.Write(new byte[] { 0x66, 0x6D, 0x74, 0x20 });      // 'fmt '
			st.Write(new byte[] { 0x10, 0x00, 0x00, 0x00 });      // chunk size 16bytes
			st.Write(new byte[] { 0x01, 0x00 }, 0, 2);                  // formatTag 0001 PCM
			st.Write((short)wfx.nChannels);                              // channels
			st.Write((int)wfx.nSamplesPerSec);                             // samples per sec
			st.Write((int)wfx.nAvgBytesPerSec);          // avg bytesper sec
			st.Write((short)wfx.nBlockAlign);                        // blockalign = 16bit * mono/stereo
			st.Write((short)wfx.wBitsPerSample);                  // bitspersample = 16bits

			st.Write(new byte[] { 0x64, 0x61, 0x74, 0x61 });      // 'data'
			st.Write((int) nTotalPCMSize);      // datasize 
			
			st.Write(Dest);
Trace.TraceInformation($"wrote ({outfile}.wav) fsLength=" + fs.Length + ", TotalPCMSize=" + nTotalPCMSize + ", diff=" + (fs.Length - nTotalPCMSize));
			st.Dispose();
			fs.Dispose();
		}
	}
}
