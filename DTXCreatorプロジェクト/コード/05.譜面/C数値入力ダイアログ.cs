using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace DTXCreator.譜面
{
	public partial class C数値入力ダイアログ : Form
	{
		public decimal dc数値
		{
			get
			{
				return this.numericUpDown数値.Value;
			}
		}

		public C数値入力ダイアログ()
		{
			this.InitializeComponent();
		}
		public C数値入力ダイアログ( decimal dc開始値, decimal dc最小値, decimal dc最大値, string strメッセージ )
		{
			this.InitializeComponent();
			this.labelメッセージ.Text = strメッセージ;
			this.numericUpDown数値.Value = dc開始値;
			this.numericUpDown数値.Minimum = dc最小値;
			this.numericUpDown数値.Maximum = dc最大値;
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
