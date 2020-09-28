/*-
 * Copyright (C) 2018-2019  Dridi Boukelmoune
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Alternatively, you can also redistribute this library and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of either License along with this
 * program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using System.Reflection;

using static System.Diagnostics.Debug;

[assembly: AssemblyVersionAttribute("0.0")]

namespace bjxa
{

	internal static class LittleEndian
	{
		private static uint Read(byte[] buf, int off, int len)
		{
			Assert(buf != null);
			Assert(off >= 0);
			Assert(len > 0);
			Assert(off + len / 8 <= buf.Length);

			uint res = 0;
			int shl = 0;
			while (len > 0)
			{
				res += (uint)buf[off] << shl;
				shl += 8;
				len -= 8;
				off++;
			}

			return (res);
		}

		internal static short ReadShort(byte[] buf, int off)
		{
			return ((short)ReadUShort(buf, off));
		}

		internal static ushort ReadUShort(byte[] buf, int off)
		{
			return ((ushort)Read(buf, off, 16));
		}

		internal static uint ReadUInt(byte[] buf, int off)
		{
			return (Read(buf, off, 32));
		}

		internal static void Write(byte[] buf, int off, long val,
			int len)
		{
			Write(buf, off, (ulong)val, len);
		}

		internal static void Write(byte[] buf, int off, ulong val,
			int len)
		{
			Assert(buf != null);
			Assert(off >= 0);
			Assert(len > 0);
			Assert(off + len / 8 <= buf.Length);

			while (len > 0)
			{
				buf[off] = (byte)val;
				val >>= 8;
				len -= 8;
				off++;
			}
		}
	}

	public class Format
	{
		public const int HEADER_SIZE_XA = 32;
		public const int HEADER_SIZE_RIFF = 44;

		internal const uint HEADER_MAGIC = 0x3144574b;
		internal const uint BLOCK_SAMPLES = 32;
		internal const uint WAVE_HEADER_LEN = 16;
		internal const ushort WAVE_FORMAT_PCM = 1;

		public long DataLengthPcm;
		public long Blocks;
		public uint BlockSizePcm;
		public uint BlockSizeXa;
		public ushort SamplesRate;
		public uint SampleBits;
		public uint Channels;

		public uint BlockSamples
		{
			get { return (BLOCK_SAMPLES * Channels); }
		}

		public long RiffHeaderLength
		{
			get { return (HEADER_SIZE_RIFF - 8 + DataLengthPcm); }
		}

		public uint WaveHeaderLength
		{
			get { return (WAVE_HEADER_LEN); }
		}

		public uint WaveFormatPcm
		{
			get { return (WAVE_FORMAT_PCM); }
		}

		public uint WaveByteRate
		{
			get
			{
				return (SamplesRate * BlockSizePcm /
					BLOCK_SAMPLES);
			}
		}

		public uint WaveBlockAlign
		{
			get { return (Channels * SampleBits / 8); }
		}

		internal Format() { }

		public byte[] WritePcm(short[] pcm, long len)
		{
			if (pcm == null)
				throw new ArgumentNullException(
					"bjxa.Format.WritePcm: pcm");
			if (len % sizeof(short) != 0)
				throw new ArgumentException(
					"Invalid length: {len}");
			int pcmLen = (int)len / sizeof(short);
			if (pcmLen < 0 || pcmLen > pcm.Length)
				throw new ArgumentException(
					"Invalid length: {len}");

			byte[] dst = new byte[pcmLen * sizeof(short)];

			int dstOff = 0, pcmOff = 0;
			while (pcmOff < pcmLen)
			{
				ushort sample = (ushort)pcm[pcmOff];
				pcmOff++;

				dst[dstOff] = (byte)(sample & 0xff);
				dstOff++;
				dst[dstOff] = (byte)(sample >> 8);
				dstOff++;
			}

			return (dst);
		}

		public int WritePcm(Stream wav, short[] pcm, long len)
		{
			if (wav == null)
				throw new ArgumentNullException(
					"bjxa.Format.WritePcm: wav");
			if (pcm == null)
				throw new ArgumentNullException(
					"bjxa.Format.WritePcm: pcm");

			byte[] buf = WritePcm(pcm, len);
			wav.Write(buf, 0, buf.Length);
			return (buf.Length);
		}
	}

	public sealed class FormatError : Exception
	{
		internal FormatError(String message) : base(message) { }
	}

	internal delegate byte Inflate(DecoderState dec, short[] dst, int off,
		byte[] src);

	internal class ChannelState
	{
		internal short Prev0;
		internal short Prev1;
	}

	internal class DecoderState
	{
		internal uint DataLength;
		internal uint Samples;
		internal ushort SamplesRate;
		internal uint BlockSize;
		internal uint Channels;
		internal ChannelState[] LR;
		internal Inflate InflateFunc;

		internal DecoderState()
		{
			LR = new ChannelState[2] {
				new ChannelState(),
				new ChannelState(),
			};
		}

		internal bool IsValid()
		{
			long blocks, maxSamples;

			if (DataLength == 0 || Samples == 0 ||
				SamplesRate == 0 || BlockSize == 0)
				return (false);

			if (Channels != 1 && Channels != 2)
				return (false);

			blocks = DataLength / BlockSize;
			maxSamples = (Format.BLOCK_SAMPLES * DataLength) /
				(BlockSize * Channels);

			if (blocks * BlockSize != DataLength)
				return (false);

			if (Samples > maxSamples)
				return (false);

			if (maxSamples - Samples >= Format.BLOCK_SAMPLES)
				return (false);

			if (InflateFunc == null)
				return (false);

			return (true);
		}

		internal Format ToFormat()
		{
			Assert(IsValid());

			Format fmt = new Format();
			fmt.DataLengthPcm = Samples * Channels *
				sizeof(short);
			fmt.SamplesRate = SamplesRate;
			fmt.SampleBits = 16;
			fmt.Channels = Channels;
			fmt.BlockSizeXa = BlockSize * Channels;
			fmt.BlockSizePcm = Format.BLOCK_SAMPLES * Channels *
				sizeof(short);
			fmt.Blocks = DataLength / fmt.BlockSizeXa;

			Assert(fmt.Blocks * fmt.BlockSizeXa == DataLength);

			return (fmt);
		}

		internal byte Inflate(short[] dst, int off, byte[] src)
		{
			return (InflateFunc(this, dst, off, src));
		}
	}

	public class Decoder
	{
		DecoderState state;
		Format fmt;

		static Inflate BlockInflater(uint bits)
		{
			if (bits == 4)
				return ((dec, dst, off, src) => {
					Assert(off == 0 || off == 1);
					Assert(Format.BLOCK_SAMPLES *
						dec.Channels == dst.Length);

					byte profile = src[0];
					int srcOff = 1;

					for (uint n = Format.BLOCK_SAMPLES;
						n > 0; n -= 2)
					{
						ushort s = src[srcOff];
						srcOff++;

						dst[off] =
							(short)((s & 0xf0) << 8);
						off += (int)dec.Channels;
						dst[off] =
							(short)((s & 0x0f) << 12);
						off += (int)dec.Channels;
					}

					return (profile);
				});

			if (bits == 6)
				return ((dec, dst, off, src) => {
					Assert(off == 0 || off == 1);
					Assert(Format.BLOCK_SAMPLES *
						dec.Channels == dst.Length);

					byte profile = src[0];
					int srcOff = 1;

					for (uint n = Format.BLOCK_SAMPLES;
						n > 0; n -= 4)
					{
						int s = (src[srcOff] << 16) |
							(src[srcOff + 1] << 8) |
							src[srcOff + 2];
						srcOff += 3;

						dst[off] = (short)
							((s & 0x00fc0000) >> 8);
						off += (int)dec.Channels;
						dst[off] = (short)
							((s & 0x0003f000) >> 2);
						off += (int)dec.Channels;
						dst[off] = (short)
							((s & 0x00000fc0) << 4);
						off += (int)dec.Channels;
						dst[off] = (short)
							((s & 0x0000003f) << 10);
						off += (int)dec.Channels;
					}

					return (profile);
				});

			if (bits == 8)
				return ((dec, dst, off, src) => {
					Assert(off == 0 || off == 1);
					Assert(Format.BLOCK_SAMPLES *
						dec.Channels == dst.Length);

					byte profile = src[0];
					int srcOff = 1;

					for (uint n = Format.BLOCK_SAMPLES;
						n > 0; n--)
					{
						dst[off] =
							(short)(src[srcOff] << 8);
						off += (int)dec.Channels;
						srcOff++;
					}

					return (profile);
				});

			return (null);
		}

		public Format ReadHeader(byte[] xa)
		{
			if (xa == null)
				throw new ArgumentNullException(
					"bjxa.Decoder.ReadHeader: xa");
			if (xa.Length < Format.HEADER_SIZE_XA)
				throw new ArgumentException(
					"buffer too small");

			DecoderState tmp = new DecoderState();
			uint magic = LittleEndian.ReadUInt(xa, 0);
			tmp.DataLength = LittleEndian.ReadUInt(xa, 4);
			tmp.Samples = LittleEndian.ReadUInt(xa, 8);
			tmp.SamplesRate = LittleEndian.ReadUShort(xa, 12);
			uint bits = xa[14];
			tmp.Channels = xa[15];
			/* XXX: skipping loop ptr field for now */
			tmp.LR[0].Prev0 = LittleEndian.ReadShort(xa, 20);
			tmp.LR[0].Prev1 = LittleEndian.ReadShort(xa, 22);
			tmp.LR[1].Prev0 = LittleEndian.ReadShort(xa, 24);
			tmp.LR[1].Prev1 = LittleEndian.ReadShort(xa, 26);
			/* XXX: ignoring padding for now */

			tmp.InflateFunc = BlockInflater(bits);
			tmp.BlockSize = bits * 4 + 1;

			if (magic != Format.HEADER_MAGIC || !tmp.IsValid())
				throw new FormatError("Invalid XA header");

			state = tmp;
			fmt = state.ToFormat();

			return (state.ToFormat());
		}

		public Format ReadHeader(Stream xa)
		{
			if (xa == null)
				throw new ArgumentNullException(
					"bjxa.Decoder.ReadHeader: xa");

			byte[] hdr = new byte[Format.HEADER_SIZE_XA];
			if (xa.Read(hdr, 0, hdr.Length) != hdr.Length)
				throw new EndOfStreamException("XA header");

			return ReadHeader(hdr);
		}

		public byte[] WriteRiffHeader()
		{
			if (state == null)
				throw new InvalidOperationException(
					"Decoder not ready");
			Format fmt = state.ToFormat();
			byte[] hdr = new byte[Format.HEADER_SIZE_RIFF];
			LittleEndian.Write(hdr, 0, 0x46464952, 32); /* RIFF */
			LittleEndian.Write(hdr, 4, fmt.RiffHeaderLength, 32);
			LittleEndian.Write(hdr, 8, 0x45564157, 32); /* WAVE */
			LittleEndian.Write(hdr, 12, 0x20746d66, 32); /* fmt  */
			LittleEndian.Write(hdr, 16, fmt.WaveHeaderLength, 32);
			LittleEndian.Write(hdr, 20, fmt.WaveFormatPcm, 16);
			LittleEndian.Write(hdr, 22, fmt.Channels, 16);
			LittleEndian.Write(hdr, 24, fmt.SamplesRate, 32);
			LittleEndian.Write(hdr, 28, fmt.WaveByteRate, 32);
			LittleEndian.Write(hdr, 32, fmt.WaveBlockAlign, 16);
			LittleEndian.Write(hdr, 34, fmt.SampleBits, 16);
			LittleEndian.Write(hdr, 36, 0x61746164, 32); /* data */
			LittleEndian.Write(hdr, 40, fmt.DataLengthPcm, 32);
			return (hdr);
		}

		public int WriteRiffHeader(Stream wav)
		{
			if (state == null)
				throw new InvalidOperationException(
					"Decoder not ready");
			if (wav == null)
				throw new ArgumentNullException(
					"bjxa.Decoder.WriteRiffHeader: wav");

			byte[] hdr = WriteRiffHeader();
			wav.Write(hdr, 0, hdr.Length);
			return (hdr.Length);
		}

		private readonly short[,] GainFactor = {
			{  0,    0},
			{240,    0},
			{460, -208},
			{392, -220},
			{488, -240},
		};

		private void DecodeInflated(short[] pcm, int off, byte prof)
		{
			Assert(off == 0 || off == 1);
			Assert(Format.BLOCK_SAMPLES * state.Channels ==
				pcm.Length);

			int factor = prof >> 4;
			int range = prof & 0x0f;

			if (factor >= GainFactor.Length)
				throw new FormatError(
					$"Invalid factor: {factor}");

			ChannelState chan = state.LR[off];
			short k0 = GainFactor[factor, 0];
			short k1 = GainFactor[factor, 1];

			for (uint i = Format.BLOCK_SAMPLES; i > 0; i--)
			{
				int ranged = pcm[off] >> range;
				int gain = (chan.Prev0 * k0) +
					(chan.Prev1 * k1);
				int sample = ranged + gain / 256;

				sample = Math.Max(sample, Int16.MinValue);
				sample = Math.Min(sample, Int16.MaxValue);

				pcm[off] = (short)sample;
				chan.Prev1 = chan.Prev0;
				chan.Prev0 = (short)sample;

				off += (int)state.Channels;
			}
		}

		public int Decode(byte[] xa, short[] pcm, out long pcm_data)
		{
			if (state == null)
				throw new InvalidOperationException(
					"Decoder not ready");
			if (xa == null)
				throw new ArgumentNullException(
					"bjxa.Decoder.Decode: xa");
			if (pcm == null)
				throw new ArgumentNullException(
					"bjxa.Decoder.Decode: pcm");
			Assert(fmt != null);
			if (fmt.Blocks == 0)
				throw new InvalidOperationException(
					"XA stream fully decoded");

			int xaLen = xa.Length * sizeof(byte);
			int xaOff = 0;
			int pcmLen = pcm.Length * sizeof(short);
			int pcmOff = 0;
			byte[] xaBuf = new byte[state.BlockSize];
			short[] pcmBuf = new short[fmt.BlockSamples];
			int blocks = 0;
			long pcm_block = Math.Min(fmt.BlockSizePcm,
				fmt.DataLengthPcm);
			pcm_data = 0;

			while (fmt.Blocks > 0 && xaLen >= fmt.BlockSizeXa &&
				pcmLen >= pcm_block)
			{

				Array.Copy(xa, xaOff, xaBuf, 0, xaBuf.Length);
				byte prof = state.Inflate(pcmBuf, 0, xaBuf);
				DecodeInflated(pcmBuf, 0, prof);

				xaOff += xaBuf.Length;
				xaLen -= xaBuf.Length;

				if (state.Channels == 2)
				{
					Array.Copy(xa, xaOff, xaBuf, 0,
						xaBuf.Length);
					prof = state.Inflate(pcmBuf, 1, xaBuf);
					DecodeInflated(pcmBuf, 1, prof);

					xaOff += xaBuf.Length;
					xaLen -= xaBuf.Length;
				}

				Array.Copy(pcmBuf, 0, pcm, pcmOff,
					pcm_block / sizeof(short));
				pcmOff += pcmBuf.Length;
				pcmLen -= (int)pcm_block;
				pcm_data += pcm_block;
				fmt.DataLengthPcm -= pcm_block;

				pcm_block = Math.Min(fmt.BlockSizePcm,
					fmt.DataLengthPcm);

				blocks++;
				fmt.Blocks--;
			}

			return (blocks);
		}
	}
}