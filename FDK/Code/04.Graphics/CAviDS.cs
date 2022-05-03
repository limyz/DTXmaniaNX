using System;
using System.Runtime.InteropServices;
using DirectShowLib;
using SharpDX;
using SharpDX.Direct3D9;

namespace FDK
{
	public class CAviDS : IDisposable
	{
		private const int timeOutMs = 1000;

		private int nWidth;

		private int nHeight;

		private long nMediaLength;

		private bool bPlaying;

		private bool bPause;

		private IGraphBuilder builder;

		private VideoInfoHeader videoInfo;

		private ISampleGrabber grabber;

		private IMediaControl control;

		private IMediaSeeking seeker;

		private IMediaFilter filter;

		private FilterState state;

		private AMMediaType mediaType;

		private IntPtr samplePtr = IntPtr.Zero;

		private bool bDisposed;

		public uint nフレーム高さ => (uint)nHeight;

		public uint nフレーム幅 => (uint)nWidth;

		public bool b再生中 => bPlaying;

		public bool b一時停止中 => bPause;

		public int GetDuration()
		{
			return (int)(nMediaLength / 10000);
		}

		public CAviDS(string filename, double playSpeed)
		{
			builder = new FilterGraph() as IGraphBuilder;
			grabber = new SampleGrabber() as ISampleGrabber;
			mediaType = new AMMediaType();
			mediaType.majorType = MediaType.Video;
			mediaType.subType = MediaSubType.RGB32;
			mediaType.formatType = FormatType.VideoInfo;
			DsError.ThrowExceptionForHR(grabber.SetMediaType(mediaType));
			DsError.ThrowExceptionForHR(builder.AddFilter(grabber as IBaseFilter, "Sample Grabber(DTXMania)"));
			DsError.ThrowExceptionForHR(builder.RenderFile(filename, null));
			CDirectShow.ConnectNullRendererFromSampleGrabber(builder, grabber as IBaseFilter);
			if (builder is IVideoWindow videoWindow)
			{
				videoWindow.put_AutoShow(OABool.False);
			}
			DsError.ThrowExceptionForHR(grabber.GetConnectedMediaType(mediaType));
			videoInfo = (VideoInfoHeader)Marshal.PtrToStructure(mediaType.formatPtr, typeof(VideoInfoHeader));
			nWidth = videoInfo.BmiHeader.Width;
			nHeight = videoInfo.BmiHeader.Height;
			seeker = builder as IMediaSeeking;
			DsError.ThrowExceptionForHR(seeker.GetDuration(out nMediaLength));
			DsError.ThrowExceptionForHR(seeker.SetRate(playSpeed / 20.0));
			control = builder as IMediaControl;
			filter = builder as IMediaFilter;
			grabber.SetBufferSamples(BufferThem: true);
			Run();
			Pause();
			bPlaying = false;
			bPause = false;
		}

		public void Seek(int timeInMs)
		{
			DsError.ThrowExceptionForHR(seeker.SetPositions(new DsLong(timeInMs * 10000), AMSeekingSeekingFlags.AbsolutePositioning, null, AMSeekingSeekingFlags.NoPositioning));
			DsError.ThrowExceptionForHR(control.GetState(1000, out state));
		}

		public void Run()
		{
			DsError.ThrowExceptionForHR(control.Run());
			DsError.ThrowExceptionForHR(control.GetState(1000, out state));
			bPlaying = true;
			bPause = false;
		}

		public void Stop()
		{
			DsError.ThrowExceptionForHR(control.Stop());
			DsError.ThrowExceptionForHR(control.GetState(1000, out state));
			bPlaying = false;
			bPause = false;
		}

		public void Pause()
		{
			DsError.ThrowExceptionForHR(control.Pause());
			DsError.ThrowExceptionForHR(control.GetState(1000, out state));
			bPause = true;
		}

		public void ToggleRun()
		{
			DsError.ThrowExceptionForHR(control.GetState(1000, out state));
			if (state == FilterState.Paused)
			{
				Run();
			}
			else if (state == FilterState.Running)
			{
				Pause();
			}
		}

		public void tGetBitmap(Device device, CTexture ctex, int timeMs, bool bCopyToCTex = true)
		{
			int pBufferSize = 0;
			DsError.ThrowExceptionForHR(grabber.GetCurrentBuffer(ref pBufferSize, IntPtr.Zero));
			if (samplePtr == IntPtr.Zero)
			{
				samplePtr = Marshal.AllocHGlobal(pBufferSize);
			}
			if (bCopyToCTex && ctex != null)
			{
				DataRectangle dataRectangle = ctex.texture.LockRectangle(0, LockFlags.None);
				DsError.ThrowExceptionForHR(grabber.GetCurrentBuffer(ref pBufferSize, dataRectangle.DataPointer));
				ctex.texture.UnlockRectangle(0);
			}
		}

		public void Dispose()
		{
			if (bDisposed)
			{
				return;
			}
			try
			{
				if (builder != null)
				{
					Marshal.ReleaseComObject(builder);
					builder = null;
				}
			}
			catch
			{
			}
			try
			{
				if (grabber != null)
				{
					Marshal.ReleaseComObject(grabber);
					grabber = null;
				}
			}
			catch
			{
			}
			try
			{
				if (mediaType != null)
				{
					DsUtils.FreeAMMediaType(mediaType);
					mediaType = null;
				}
			}
			catch
			{
			}
			try
			{
				if (samplePtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(samplePtr);
				}
			}
			catch
			{
			}
			GC.SuppressFinalize(this);
			bDisposed = true;
		}

		~CAviDS()
		{
			Dispose();
		}
	}

}

