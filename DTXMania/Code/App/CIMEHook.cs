using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace DTXMania
{
	public class CIMEHook : ScrollableControl
	{
		private readonly int hIMC;

		private bool bDisposed;

		private const int GCS_COMPSTR = 8;

		private const int GCS_RESULTSTR = 2048;

		public bool bAccessible
		{
			get;
		}

		public string str入力中文字列
		{
			get
			{
				if (!bAccessible)
				{
					return "";
				}
				int num = ImmGetCompositionString(hIMC, 8, null, 0);
				char[] array = new char[num / 2];
				ImmGetCompositionString(hIMC, 8, array, num);
				return new string(array, 0, num / 2);
			}
		}

		public string str確定文字列
		{
			get
			{
				if (!bAccessible)
				{
					return "";
				}
				int num = ImmGetCompositionString(hIMC, 2048, null, 0);
				char[] array = new char[num / 2];
				ImmGetCompositionString(hIMC, 2048, array, num);
				return new string(array, 0, num / 2);
			}
		}

		public CIMEHook()
		{
			base.TabStop = false;
			SetStyle(ControlStyles.Selectable, value: false);
			base.KeyPress += delegate(object sender, KeyPressEventArgs e)
			{
                if (CDTXMania.app.textboxテキスト入力中 != null)
                {
                    CDTXMania.app.textboxテキスト入力中.t1文字格納する(e.KeyChar);
                    e.Handled = true;
                }
            };
			hIMC = ImmGetContext(base.Handle);
			bAccessible = (hIMC != 0);
		}

		public new void Dispose()
		{
			if (!bDisposed)
			{
				if (bAccessible)
				{
					ImmReleaseContext(base.Handle, hIMC);
				}
				bDisposed = true;
			}
		}

		[DllImport("Imm32.dll")]
		[SuppressUnmanagedCodeSecurity]
		public static extern int ImmGetContext(IntPtr hWnd);

		[DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
		[SuppressUnmanagedCodeSecurity]
		private static extern int ImmGetCompositionString(int hIMC, int dwIndex, char[] lpBuf, int dwBufLen);

		[DllImport("Imm32.dll")]
		[SuppressUnmanagedCodeSecurity]
		private static extern bool ImmReleaseContext(IntPtr hWnd, int hIMC);

		[DllImport("imm32.dll", CharSet = CharSet.Unicode)]
		[SuppressUnmanagedCodeSecurity]
		private static extern int ImmGetCandidateList(IntPtr hIMC, int dwIndex, byte[] lpCandList, int dwBufLen);
	}
}
