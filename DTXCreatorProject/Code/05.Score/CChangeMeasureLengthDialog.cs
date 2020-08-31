using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using FDK;

namespace DTXCreator.Score
{
	public partial class CChangeMeasureLengthDialog : Form  // C小節長変更ダイアログ
	{
		public bool b後続変更
		{
			get
			{
				return this.checkBox後続設定.Checked;
			}
			set
			{
				this.checkBox後続設定.CheckState = value ? CheckState.Checked : CheckState.Unchecked;
			}
		}
		public float f倍率
		{
			get
			{
				return (float) this.numericUpDown小節長の倍率.Value;
			}
			set
			{
				this.numericUpDown小節長の倍率.Value = (decimal) value;
			}
		}

		public CChangeMeasureLengthDialog( int n小節番号 )
		{
			this.InitializeComponent();
			this.textBox小節番号.Text = CConversion.strConvertNumberTo3DigitMeasureNumber( n小節番号 );
		}

		private void numericUpDown小節長の倍率_KeyDown( object sender, KeyEventArgs e )
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
		private void checkBox後続設定_KeyDown( object sender, KeyEventArgs e )
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
