using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using DTXCreator.Properties;
using FDK;

namespace DTXCreator.譜面
{
	public partial class C置換ダイアログ : Form
	{
		public bool b単純置換RadioButtonがチェックされている
		{
			get
			{
				return this.radioButton単純置換.Checked;
			}
		}
		public bool b表裏反転RadioButtonがチェックされている
		{
			get
			{
				return this.radioButton表裏反転.Checked;
			}
		}
		public int n元番号
		{
			get
			{
				if( ( this.textBox元番号.Text.Length != 1 ) && ( this.textBox元番号.Text.Length != 2 ) )
				{
					return -1;
				}
				string text = this.textBox元番号.Text;
				if( text.Length == 1 )
				{
					text = "0" + text;
				}
				return C変換.n36進数2桁の文字列を数値に変換して返す( text );
			}
			set
			{
				this.textBox元番号.Text = C変換.str数値を36進数2桁に変換して返す( value );
			}
		}
		public int n先番号
		{
			get
			{
				if( ( this.textBox先番号.Text.Length != 1 ) && ( this.textBox先番号.Text.Length != 2 ) )
				{
					return -1;
				}
				string text = this.textBox先番号.Text;
				if( text.Length == 1 )
				{
					text = "0" + text;
				}
				return C変換.n36進数2桁の文字列を数値に変換して返す( text );
			}
		}

		public C置換ダイアログ()
		{
			this.InitializeComponent();
			this.radioButton単純置換.Select();
			this.textBox元番号.Focus();
		}

		private void C置換ダイアログ_FormClosing( object sender, FormClosingEventArgs e )
		{
			if( ( base.DialogResult == DialogResult.OK ) && this.b単純置換RadioButtonがチェックされている )
			{
				if( this.n元番号 < 0 )
				{
					MessageBox.Show( Resources.strチップ番号に誤りがありますMSG + Environment.NewLine + "'" + this.textBox元番号.Text + "'", Resources.strエラーダイアログのタイトル, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1 );
					this.textBox元番号.Focus();
					this.textBox元番号.SelectAll();
					e.Cancel = true;
				}
				else if( this.n先番号 < 0 )
				{
					MessageBox.Show( Resources.strチップ番号に誤りがありますMSG + Environment.NewLine + "'" + this.textBox先番号.Text + "'", Resources.strエラーダイアログのタイトル, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1 );
					this.textBox先番号.Focus();
					this.textBox先番号.SelectAll();
					e.Cancel = true;
				}
			}
		}
		private void radioButton単純置換_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Return )
			{
				this.button置換.PerformClick();
			}
			else if( e.KeyCode == Keys.Escape )
			{
				this.buttonキャンセル.PerformClick();
			}
		}
		private void radioButton表裏反転_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Return )
			{
				this.button置換.PerformClick();
			}
			else if( e.KeyCode == Keys.Escape )
			{
				this.buttonキャンセル.PerformClick();
			}
		}
		private void textBox元番号_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Return )
			{
				this.button置換.PerformClick();
			}
			else if( e.KeyCode == Keys.Escape )
			{
				this.buttonキャンセル.PerformClick();
			}
		}
		private void textBox先番号_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Return )
			{
				this.button置換.PerformClick();
			}
			else if( e.KeyCode == Keys.Escape )
			{
				this.buttonキャンセル.PerformClick();
			}
		}
	}
}
