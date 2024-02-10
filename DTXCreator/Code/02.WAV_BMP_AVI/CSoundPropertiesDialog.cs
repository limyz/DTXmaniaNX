using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using DTXCreator.汎用;
using DTXCreator.Properties;
using FDK;

namespace DTXCreator.WAV_BMP_AVI
{
	internal partial class CSoundPropertiesDialog : Form  // Cサウンドプロパティダイアログ
	{
		internal CWAV wav;

		public CSoundPropertiesDialog( string str相対パスの基点フォルダ, string str初期フォルダ, CWAVListManager.DGサウンドを再生する dgサウンドを再生する )
		{
			this.str相対パスの基点フォルダ = str相対パスの基点フォルダ;
			this.str初期フォルダ = str初期フォルダ;
			this.dgサウンドを再生する = dgサウンドを再生する;

			this.InitializeComponent();
		}
		public void t位置testBoxの値を範囲修正したのちtextBox位置とhScrollBar位置へ反映させる()
		{
			int num;
			if( this.textBox位置.Text.Length == 0 )
			{
				num = 0;
			}
			else if( !int.TryParse( this.textBox位置.Text, out num ) )
			{
				num = 0;
			}
			else
			{
				num = CConversion.nRoundToRange( num, -127, 127 );
			}
			this.textBox位置.Text = num.ToString();
			this.hScrollBar位置.Value = num + 127;
		}
		public void t音量textBoxの値を範囲修正したのちtextBox音量とhScrollBar音量へ反映させる()
		{
			int num;
			if( this.textBox音量.Text.Length == 0 )
			{
				num = 0;
			}
			else if( !int.TryParse( this.textBox音量.Text, out num ) )
			{
				num = 0;
			}
			else
			{
				num = CConversion.nRoundToRange( num, 0, 127 );
			}
			this.textBox音量.Text = num.ToString();
			this.hScrollBar音量.Value = num;
		}

		private CWAVListManager.DGサウンドを再生する dgサウンドを再生する;
		private string str初期フォルダ = "";
		private string str相対パスの基点フォルダ = "";
		private static int[] カスタムカラー;

		private void textBoxラベル_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Return )
			{
				this.buttonOK.PerformClick();
			}
			if( e.KeyCode == Keys.Escape )
			{
				this.buttonキャンセル.PerformClick();
			}
		}
		private void textBoxファイル_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Return )
			{
				this.buttonOK.PerformClick();
			}
			if( e.KeyCode == Keys.Escape )
			{
				this.buttonキャンセル.PerformClick();
			}
		}
		private void button参照_Click( object sender, EventArgs e )
		{
			var dialog = new OpenFileDialog() {
				Title = Resources.strサウンドファイル選択ダイアログのタイトル,
				Filter = Resources.strサウンドファイル選択ダイアログのフィルタ,
				FilterIndex = 1,
				InitialDirectory = this.str初期フォルダ,
			};
			if( dialog.ShowDialog() == DialogResult.OK )
			{
				string str = CFileSelector_PathConversion.str基点からの相対パスに変換して返す( dialog.FileName, this.str相対パスの基点フォルダ );
				str.Replace( '/', '\\' );
				this.textBoxファイル.Text = str;
			}
		}
		private void button参照_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Escape )
			{
				this.buttonキャンセル.PerformClick();
			}
		}
		private void hScrollBar音量_ValueChanged( object sender, EventArgs e )
		{
			this.textBox音量.Text = CConversion.nRoundToRange( this.hScrollBar音量.Value, 0, 127 ).ToString();
		}
		private void textBox音量_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Return )
			{
				this.t音量textBoxの値を範囲修正したのちtextBox音量とhScrollBar音量へ反映させる();
				this.buttonOK.PerformClick();
			}
			if( e.KeyCode == Keys.Escape )
			{
				this.buttonキャンセル.PerformClick();
			}
		}
		private void textBox音量_Leave( object sender, EventArgs e )
		{
			this.t音量textBoxの値を範囲修正したのちtextBox音量とhScrollBar音量へ反映させる();
		}
		private void textBox位置_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Return )
			{
				this.t位置testBoxの値を範囲修正したのちtextBox位置とhScrollBar位置へ反映させる();
				this.buttonOK.PerformClick();
			}
			if( e.KeyCode == Keys.Escape )
			{
				this.buttonキャンセル.PerformClick();
			}
		}
		private void textBox位置_Leave( object sender, EventArgs e )
		{
			this.t位置testBoxの値を範囲修正したのちtextBox位置とhScrollBar位置へ反映させる();
		}
		private void hScrollBar位置_ValueChanged( object sender, EventArgs e )
		{
			this.textBox位置.Text = ( CConversion.nRoundToRange( this.hScrollBar位置.Value, 0, 254 ) - 127).ToString();
		}
		private void button背景色_Click( object sender, EventArgs e )
		{
			var dialog = new ColorDialog() {
				AllowFullOpen = true,
				FullOpen = true,
				Color = this.textBoxWAV番号.BackColor,
				CustomColors = カスタムカラー,
			};
			if( dialog.ShowDialog() == DialogResult.OK )
			{
				this.textBoxWAV番号.BackColor = dialog.Color;
				カスタムカラー = dialog.CustomColors;
			}
		}
		private void button背景色_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Escape )
			{
				this.buttonキャンセル.PerformClick();
			}
		}
		private void button文字色_Click( object sender, EventArgs e )
		{
			var dialog = new ColorDialog() {
				AllowFullOpen = true,
				FullOpen = true,
				Color = this.textBoxWAV番号.ForeColor,
				CustomColors = カスタムカラー,
			};
			if( dialog.ShowDialog() == DialogResult.OK )
			{
				this.textBoxWAV番号.ForeColor = dialog.Color;
				カスタムカラー = dialog.CustomColors;
			}
		}
		private void button文字色_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Escape )
			{
				this.buttonキャンセル.PerformClick();
			}
		}
		private void button標準色に戻す_Click( object sender, EventArgs e )
		{
			this.textBoxWAV番号.ForeColor = SystemColors.WindowText;
			this.textBoxWAV番号.BackColor = SystemColors.Window;
		}
		private void button標準色に戻す_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Escape )
			{
				this.buttonキャンセル.PerformClick();
			}
		}
		private void button試聴_Click( object sender, EventArgs e )
		{
			int num = CConversion.nConvert2DigitBase62StringToNumber( this.textBoxWAV番号.Text );
			this.dgサウンドを再生する( num );
		}
		private void button試聴_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Escape )
			{
				this.buttonキャンセル.PerformClick();
			}
		}
	}
}
