using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using DTXCreator.Properties;
using FDK;

namespace DTXCreator.譜面
{
	public partial class C検索ダイアログ : Form
	{
		public static bool b前回値_チップ範囲指定 = true;
		public static bool[] b前回値_レーンリストチェック = null;
		public static bool b前回値_レーン指定 = false;
		public static bool b前回値_小節範囲指定 = false;
		public static bool b前回値_表チップ = true;
		public static bool b前回値_裏チップ = true;
		public static string[] str前回値_レーンリスト = null;

		public bool bチップ範囲指定CheckBoxがチェックされている
		{
			get
			{
				return this.checkBoxチップ範囲指定.Checked;
			}
		}
		public bool bレーンリストの内訳が生成済みである
		{
			get
			{
				if( str前回値_レーンリスト == null )
				{
					return false;
				}
				return true;
			}
		}
		public bool bレーン指定CheckBoxがチェックされている
		{
			get
			{
				return this.checkBoxレーン指定.Checked;
			}
		}
		public bool b小節範囲指定CheckBoxがチェックされている
		{
			get
			{
				return this.checkBox小節範囲指定.Checked;
			}
		}
		public bool b表チップCheckBoxがチェックされている
		{
			get
			{
				return this.checkBox表チップ.Checked;
			}
		}
		public bool b裏チップCheckBoxがチェックされている
		{
			get
			{
				return this.checkBox裏チップ.Checked;
			}
		}
		public int nチップ範囲開始番号
		{
			get
			{
				if( this.textBoxチップ範囲開始.Text.Length > 0 )
				{
					string num = this.textBoxチップ範囲開始.Text;
					if( num.Length == 1 )
					{
						num = "0" + num;
					}
					return C変換.n36進数2桁の文字列を数値に変換して返す( num );
				}
				if( this.textBoxチップ範囲終了.Text.Length <= 0 )
				{
					return -1;
				}
				string text = this.textBoxチップ範囲終了.Text;
				if( text.Length == 1 )
				{
					text = "0" + text;
				}
				return C変換.n36進数2桁の文字列を数値に変換して返す( text );
			}
		}
		public int nチップ範囲終了番号
		{
			get
			{
				if( this.textBoxチップ範囲終了.Text.Length > 0 )
				{
					string num = this.textBoxチップ範囲終了.Text;
					if( num.Length == 1 )
					{
						num = "0" + num;
					}
					return C変換.n36進数2桁の文字列を数値に変換して返す( num );
				}
				if( this.textBoxチップ範囲開始.Text.Length <= 0 )
				{
					return -1;
				}
				string text = this.textBoxチップ範囲開始.Text;
				if( text.Length == 1 )
				{
					text = "0" + text;
				}
				return C変換.n36進数2桁の文字列を数値に変換して返す( text );
			}
		}
		public int n小節範囲開始番号
		{
			get
			{
				int num2;
				if( this.textBox小節範囲開始.Text.Length > 0 )
				{
					int num;
					if( !int.TryParse( this.textBox小節範囲開始.Text, out num ) )
					{
						num = -1;
					}
					return num;
				}
				if( this.textBox小節範囲終了.Text.Length <= 0 )
				{
					return -1;
				}
				if( !int.TryParse( this.textBox小節範囲終了.Text, out num2 ) )
				{
					num2 = -1;
				}
				return num2;
			}
		}
		public int n小節範囲終了番号
		{
			get
			{
				int num2;
				if( this.textBox小節範囲終了.Text.Length > 0 )
				{
					int num;
					if( !int.TryParse( this.textBox小節範囲終了.Text, out num ) )
					{
						num = -1;
					}
					return num;
				}
				if( this.textBox小節範囲開始.Text.Length <= 0 )
				{
					return -1;
				}
				if( !int.TryParse( this.textBox小節範囲開始.Text, out num2 ) )
				{
					num2 = -1;
				}
				return num2;
			}
		}

		public C検索ダイアログ()
		{
			this.InitializeComponent();
			this.checkBoxレーン指定.CheckState = b前回値_レーン指定 ? CheckState.Checked : CheckState.Unchecked;
			this.checkBoxチップ範囲指定.CheckState = b前回値_チップ範囲指定 ? CheckState.Checked : CheckState.Unchecked;
			this.checkBox小節範囲指定.CheckState = b前回値_小節範囲指定 ? CheckState.Checked : CheckState.Unchecked;
			this.checkBox表チップ.CheckState = b前回値_表チップ ? CheckState.Checked : CheckState.Unchecked;
			this.checkBox裏チップ.CheckState = b前回値_裏チップ ? CheckState.Checked : CheckState.Unchecked;
			if( ( str前回値_レーンリスト != null ) && ( str前回値_レーンリスト.Length > 0 ) )
			{
				for( int i = 0; i < str前回値_レーンリスト.Length; i++ )
				{
					this.checkedListBoxレーン選択リスト.Items.Add( str前回値_レーンリスト[ i ], b前回値_レーンリストチェック[ i ] );
				}
			}
			this.tチェックに連動して有効・無効が決まるパーツについてEnabledを設定する();
		}
		public bool bレーンが検索対象である( int nレーン番号 )
		{
			if( ( nレーン番号 < 0 ) || ( nレーン番号 >= this.checkedListBoxレーン選択リスト.Items.Count ) )
			{
				return false;
			}
			if( this.checkedListBoxレーン選択リスト.GetItemCheckState( nレーン番号 ) != CheckState.Checked )
			{
				return false;
			}
			return true;
		}
		public void tレーンリストの内訳を生成する( string[] strリスト要素 )
		{
			b前回値_レーンリストチェック = new bool[ strリスト要素.Length ];
			for( int i = 0; i < strリスト要素.Length; i++ )
			{
				this.checkedListBoxレーン選択リスト.Items.Add( strリスト要素[ i ] );
				b前回値_レーンリストチェック[ i ] = false;
			}
			str前回値_レーンリスト = strリスト要素;
		}

		private void tチェックに連動して有効・無効が決まるパーツについてEnabledを設定する()
		{
			bool flag = this.checkBoxレーン指定.Checked;
			this.buttonALL.Enabled = flag;
			this.buttonNONE.Enabled = flag;
			this.checkedListBoxレーン選択リスト.Enabled = flag;
			flag = this.checkBoxチップ範囲指定.Checked;
			this.textBoxチップ範囲開始.Enabled = flag;
			this.textBoxチップ範囲終了.Enabled = flag;
			flag = this.checkBox小節範囲指定.Checked;
			this.textBox小節範囲開始.Enabled = flag;
			this.textBox小節範囲終了.Enabled = flag;
		}

		private void C検索ダイアログ_FormClosing( object sender, FormClosingEventArgs e )
		{
			if( base.DialogResult == DialogResult.OK )
			{
				string text = this.textBoxチップ範囲開始.Text;
				if( text.Length == 1 )
				{
					text = "0" + text;
				}
				if( ( text.Length > 0 ) && ( ( text.Length > 2 ) || ( C変換.n36進数2桁の文字列を数値に変換して返す( text ) == -1 ) ) )
				{
					MessageBox.Show( Resources.strチップ番号に誤りがありますMSG + Environment.NewLine + "'" + text + "'", Resources.strエラーダイアログのタイトル, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1 );
					this.textBoxチップ範囲開始.Focus();
					this.textBoxチップ範囲開始.SelectAll();
					e.Cancel = true;
				}
				else
				{
					text = this.textBoxチップ範囲終了.Text;
					if( text.Length == 1 )
					{
						text = "0" + text;
					}
					if( ( text.Length > 0 ) && ( ( text.Length > 2 ) || ( C変換.n36進数2桁の文字列を数値に変換して返す( text ) == -1 ) ) )
					{
						MessageBox.Show( Resources.strチップ番号に誤りがありますMSG + Environment.NewLine + "'" + text + "'", Resources.strエラーダイアログのタイトル, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1 );
						this.textBoxチップ範囲終了.Focus();
						this.textBoxチップ範囲終了.SelectAll();
						e.Cancel = true;
					}
					else
					{
						int num;
						text = this.textBox小節範囲開始.Text;
						if( ( text.Length > 0 ) && ( !int.TryParse( text, out num ) || ( num < 0 ) ) )
						{
							MessageBox.Show( Resources.str小節番号に誤りがありますMSG + Environment.NewLine + "'" + text + "'", Resources.strエラーダイアログのタイトル, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1 );
							this.textBox小節範囲開始.Focus();
							this.textBox小節範囲開始.SelectAll();
							e.Cancel = true;
						}
						else
						{
							text = this.textBox小節範囲終了.Text;
							if( ( text.Length > 0 ) && ( !int.TryParse( text, out num ) || ( num < 0 ) ) )
							{
								MessageBox.Show( Resources.str小節番号に誤りがありますMSG + Environment.NewLine + "'" + text + "'", Resources.strエラーダイアログのタイトル, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1 );
								this.textBox小節範囲終了.Focus();
								this.textBox小節範囲終了.SelectAll();
								e.Cancel = true;
							}
							else
							{
								b前回値_レーン指定 = this.checkBoxレーン指定.Checked;
								b前回値_チップ範囲指定 = this.checkBoxチップ範囲指定.Checked;
								b前回値_小節範囲指定 = this.checkBox小節範囲指定.Checked;
								b前回値_表チップ = this.checkBox表チップ.Checked;
								b前回値_裏チップ = this.checkBox裏チップ.Checked;
								for( int i = 0; i < this.checkedListBoxレーン選択リスト.Items.Count; i++ )
								{
									b前回値_レーンリストチェック[ i ] = this.checkedListBoxレーン選択リスト.GetItemCheckState( i ) == CheckState.Checked;
								}
							}
						}
					}
				}
			}
		}
		private void C検索ダイアログ_KeyDown( object sender, KeyEventArgs e )
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
		private void textBoxチップ範囲開始_KeyDown( object sender, KeyEventArgs e )
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
		private void textBoxチップ範囲終了_KeyDown( object sender, KeyEventArgs e )
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
		private void textBox小節範囲開始_KeyDown( object sender, KeyEventArgs e )
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
		private void textBox小節範囲終了_KeyDown( object sender, KeyEventArgs e )
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
		private void checkBox小節範囲指定_CheckStateChanged( object sender, EventArgs e )
		{
			this.tチェックに連動して有効・無効が決まるパーツについてEnabledを設定する();
		}
		private void checkBox小節範囲指定_KeyDown( object sender, KeyEventArgs e )
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
		private void checkBoxチップ範囲指定_CheckStateChanged( object sender, EventArgs e )
		{
			this.tチェックに連動して有効・無効が決まるパーツについてEnabledを設定する();
		}
		private void checkBoxチップ範囲指定_KeyDown( object sender, KeyEventArgs e )
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
		private void checkBoxレーン指定_CheckStateChanged( object sender, EventArgs e )
		{
			this.tチェックに連動して有効・無効が決まるパーツについてEnabledを設定する();
		}
		private void checkBoxレーン指定_KeyDown( object sender, KeyEventArgs e )
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
		private void checkedListBoxレーン選択リスト_KeyDown( object sender, KeyEventArgs e )
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
		private void checkBox表チップ_KeyDown( object sender, KeyEventArgs e )
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
		private void checkBox裏チップ_KeyDown( object sender, KeyEventArgs e )
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
		private void buttonNONE_Click( object sender, EventArgs e )
		{
			for( int i = 0; i < this.checkedListBoxレーン選択リスト.Items.Count; i++ )
			{
				this.checkedListBoxレーン選択リスト.SetItemCheckState( i, CheckState.Unchecked );
			}
		}
		private void buttonNONE_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Escape )
			{
				this.buttonキャンセル.PerformClick();
			}
		}
		private void buttonALL_Click( object sender, EventArgs e )
		{
			for( int i = 0; i < this.checkedListBoxレーン選択リスト.Items.Count; i++ )
			{
				this.checkedListBoxレーン選択リスト.SetItemChecked( i, true );
			}
		}
		private void buttonALL_KeyDown( object sender, KeyEventArgs e )
		{
			if( e.KeyCode == Keys.Escape )
			{
				this.buttonキャンセル.PerformClick();
			}
		}
	}
}
