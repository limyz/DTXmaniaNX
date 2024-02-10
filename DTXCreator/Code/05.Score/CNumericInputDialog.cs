using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace DTXCreator.Score
{
	public partial class CNumericInputDialog : Form
	{
		public decimal dc数値
		{
			get
			{
				return this.numericUpDown数値.Value;
			}
		}

		public CNumericInputDialog()
		{
			this.InitializeComponent();
		}
		public CNumericInputDialog( decimal dc開始値, decimal dc最小値, decimal dc最大値, string strメッセージ )
		{
			this.InitializeComponent();
			this.labelメッセージ.Text = strメッセージ;
			this.numericUpDown数値.Value = dc開始値;
			this.numericUpDown数値.Minimum = dc最小値;
			this.numericUpDown数値.Maximum = dc最大値;


			#region [ 数値入力欄を全選択状態にする ]
			int lenIntpart = this.numericUpDown数値.Value.ToString().Length;
			string m = this.numericUpDown数値.Minimum.ToString();
			int lenDecpart = m.Length;
			int len = lenIntpart + lenDecpart;
 
            if (m.Contains(".") || m.Contains(","))
            {
                len -= 1;		// -2: "0.001"のピリオドのところ分の長さを削除
            }
            if (len< 2) len = 1;

			this.numericUpDown数値.Select(0, len);
			#endregion
		}

	private void numericUpDown数値_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Return )
			{
				this.buttonOK.PerformClick();
			}
			else if( e.KeyCode == Keys.Escape )
			{
				this.buttonキャンセル.PerformClick();
			}
		}
	}
}
