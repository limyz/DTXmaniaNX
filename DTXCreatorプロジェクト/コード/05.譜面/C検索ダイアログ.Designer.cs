namespace DTXCreator.譜面
{
	partial class C検索ダイアログ
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( C検索ダイアログ ) );
			this.toolTip1 = new System.Windows.Forms.ToolTip( this.components );
			this.checkBox小節範囲指定 = new System.Windows.Forms.CheckBox();
			this.checkBoxチップ範囲指定 = new System.Windows.Forms.CheckBox();
			this.checkBoxレーン指定 = new System.Windows.Forms.CheckBox();
			this.checkedListBoxレーン選択リスト = new System.Windows.Forms.CheckedListBox();
			this.checkBox表チップ = new System.Windows.Forms.CheckBox();
			this.checkBox裏チップ = new System.Windows.Forms.CheckBox();
			this.buttonNONE = new System.Windows.Forms.Button();
			this.buttonALL = new System.Windows.Forms.Button();
			this.buttonキャンセル = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxチップ範囲開始 = new System.Windows.Forms.TextBox();
			this.textBoxチップ範囲終了 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox小節範囲開始 = new System.Windows.Forms.TextBox();
			this.textBox小節範囲終了 = new System.Windows.Forms.TextBox();
			this.label説明 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// checkBox小節範囲指定
			// 
			resources.ApplyResources( this.checkBox小節範囲指定, "checkBox小節範囲指定" );
			this.checkBox小節範囲指定.Name = "checkBox小節範囲指定";
			this.toolTip1.SetToolTip( this.checkBox小節範囲指定, resources.GetString( "checkBox小節範囲指定.ToolTip" ) );
			this.checkBox小節範囲指定.UseVisualStyleBackColor = true;
			this.checkBox小節範囲指定.CheckStateChanged += new System.EventHandler( this.checkBox小節範囲指定_CheckStateChanged );
			this.checkBox小節範囲指定.KeyDown += new System.Windows.Forms.KeyEventHandler( this.checkBox小節範囲指定_KeyDown );
			// 
			// checkBoxチップ範囲指定
			// 
			resources.ApplyResources( this.checkBoxチップ範囲指定, "checkBoxチップ範囲指定" );
			this.checkBoxチップ範囲指定.Name = "checkBoxチップ範囲指定";
			this.toolTip1.SetToolTip( this.checkBoxチップ範囲指定, resources.GetString( "checkBoxチップ範囲指定.ToolTip" ) );
			this.checkBoxチップ範囲指定.UseVisualStyleBackColor = true;
			this.checkBoxチップ範囲指定.CheckStateChanged += new System.EventHandler( this.checkBoxチップ範囲指定_CheckStateChanged );
			this.checkBoxチップ範囲指定.KeyDown += new System.Windows.Forms.KeyEventHandler( this.checkBoxチップ範囲指定_KeyDown );
			// 
			// checkBoxレーン指定
			// 
			resources.ApplyResources( this.checkBoxレーン指定, "checkBoxレーン指定" );
			this.checkBoxレーン指定.Name = "checkBoxレーン指定";
			this.toolTip1.SetToolTip( this.checkBoxレーン指定, resources.GetString( "checkBoxレーン指定.ToolTip" ) );
			this.checkBoxレーン指定.UseVisualStyleBackColor = true;
			this.checkBoxレーン指定.CheckStateChanged += new System.EventHandler( this.checkBoxレーン指定_CheckStateChanged );
			this.checkBoxレーン指定.KeyDown += new System.Windows.Forms.KeyEventHandler( this.checkBoxレーン指定_KeyDown );
			// 
			// checkedListBoxレーン選択リスト
			// 
			resources.ApplyResources( this.checkedListBoxレーン選択リスト, "checkedListBoxレーン選択リスト" );
			this.checkedListBoxレーン選択リスト.CheckOnClick = true;
			this.checkedListBoxレーン選択リスト.FormattingEnabled = true;
			this.checkedListBoxレーン選択リスト.Name = "checkedListBoxレーン選択リスト";
			this.toolTip1.SetToolTip( this.checkedListBoxレーン選択リスト, resources.GetString( "checkedListBoxレーン選択リスト.ToolTip" ) );
			this.checkedListBoxレーン選択リスト.KeyDown += new System.Windows.Forms.KeyEventHandler( this.checkedListBoxレーン選択リスト_KeyDown );
			// 
			// checkBox表チップ
			// 
			resources.ApplyResources( this.checkBox表チップ, "checkBox表チップ" );
			this.checkBox表チップ.Name = "checkBox表チップ";
			this.toolTip1.SetToolTip( this.checkBox表チップ, resources.GetString( "checkBox表チップ.ToolTip" ) );
			this.checkBox表チップ.UseVisualStyleBackColor = true;
			this.checkBox表チップ.KeyDown += new System.Windows.Forms.KeyEventHandler( this.checkBox表チップ_KeyDown );
			// 
			// checkBox裏チップ
			// 
			resources.ApplyResources( this.checkBox裏チップ, "checkBox裏チップ" );
			this.checkBox裏チップ.Name = "checkBox裏チップ";
			this.toolTip1.SetToolTip( this.checkBox裏チップ, resources.GetString( "checkBox裏チップ.ToolTip" ) );
			this.checkBox裏チップ.UseVisualStyleBackColor = true;
			this.checkBox裏チップ.KeyDown += new System.Windows.Forms.KeyEventHandler( this.checkBox裏チップ_KeyDown );
			// 
			// buttonNONE
			// 
			resources.ApplyResources( this.buttonNONE, "buttonNONE" );
			this.buttonNONE.Name = "buttonNONE";
			this.toolTip1.SetToolTip( this.buttonNONE, resources.GetString( "buttonNONE.ToolTip" ) );
			this.buttonNONE.UseVisualStyleBackColor = true;
			this.buttonNONE.Click += new System.EventHandler( this.buttonNONE_Click );
			this.buttonNONE.KeyDown += new System.Windows.Forms.KeyEventHandler( this.buttonNONE_KeyDown );
			// 
			// buttonALL
			// 
			resources.ApplyResources( this.buttonALL, "buttonALL" );
			this.buttonALL.Name = "buttonALL";
			this.toolTip1.SetToolTip( this.buttonALL, resources.GetString( "buttonALL.ToolTip" ) );
			this.buttonALL.UseVisualStyleBackColor = true;
			this.buttonALL.Click += new System.EventHandler( this.buttonALL_Click );
			this.buttonALL.KeyDown += new System.Windows.Forms.KeyEventHandler( this.buttonALL_KeyDown );
			// 
			// buttonキャンセル
			// 
			this.buttonキャンセル.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources( this.buttonキャンセル, "buttonキャンセル" );
			this.buttonキャンセル.Name = "buttonキャンセル";
			this.buttonキャンセル.UseVisualStyleBackColor = true;
			// 
			// buttonOK
			// 
			resources.ApplyResources( this.buttonOK, "buttonOK" );
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			resources.ApplyResources( this.label1, "label1" );
			this.label1.Name = "label1";
			// 
			// textBoxチップ範囲開始
			// 
			resources.ApplyResources( this.textBoxチップ範囲開始, "textBoxチップ範囲開始" );
			this.textBoxチップ範囲開始.Name = "textBoxチップ範囲開始";
			this.textBoxチップ範囲開始.KeyDown += new System.Windows.Forms.KeyEventHandler( this.textBoxチップ範囲開始_KeyDown );
			// 
			// textBoxチップ範囲終了
			// 
			resources.ApplyResources( this.textBoxチップ範囲終了, "textBoxチップ範囲終了" );
			this.textBoxチップ範囲終了.Name = "textBoxチップ範囲終了";
			this.textBoxチップ範囲終了.KeyDown += new System.Windows.Forms.KeyEventHandler( this.textBoxチップ範囲終了_KeyDown );
			// 
			// label2
			// 
			resources.ApplyResources( this.label2, "label2" );
			this.label2.Name = "label2";
			// 
			// textBox小節範囲開始
			// 
			resources.ApplyResources( this.textBox小節範囲開始, "textBox小節範囲開始" );
			this.textBox小節範囲開始.Name = "textBox小節範囲開始";
			this.textBox小節範囲開始.KeyDown += new System.Windows.Forms.KeyEventHandler( this.textBox小節範囲開始_KeyDown );
			// 
			// textBox小節範囲終了
			// 
			resources.ApplyResources( this.textBox小節範囲終了, "textBox小節範囲終了" );
			this.textBox小節範囲終了.Name = "textBox小節範囲終了";
			this.textBox小節範囲終了.KeyDown += new System.Windows.Forms.KeyEventHandler( this.textBox小節範囲終了_KeyDown );
			// 
			// label説明
			// 
			resources.ApplyResources( this.label説明, "label説明" );
			this.label説明.Name = "label説明";
			// 
			// C検索ダイアログ
			// 
			resources.ApplyResources( this, "$this" );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ControlBox = false;
			this.Controls.Add( this.buttonALL );
			this.Controls.Add( this.buttonNONE );
			this.Controls.Add( this.checkBox裏チップ );
			this.Controls.Add( this.checkBox表チップ );
			this.Controls.Add( this.checkedListBoxレーン選択リスト );
			this.Controls.Add( this.checkBoxレーン指定 );
			this.Controls.Add( this.checkBoxチップ範囲指定 );
			this.Controls.Add( this.checkBox小節範囲指定 );
			this.Controls.Add( this.label説明 );
			this.Controls.Add( this.textBox小節範囲終了 );
			this.Controls.Add( this.textBox小節範囲開始 );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.textBoxチップ範囲終了 );
			this.Controls.Add( this.textBoxチップ範囲開始 );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.buttonOK );
			this.Controls.Add( this.buttonキャンセル );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "C検索ダイアログ";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.C検索ダイアログ_FormClosing );
			this.KeyDown += new System.Windows.Forms.KeyEventHandler( this.C検索ダイアログ_KeyDown );
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button buttonキャンセル;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxチップ範囲開始;
		private System.Windows.Forms.TextBox textBoxチップ範囲終了;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBox小節範囲開始;
		private System.Windows.Forms.TextBox textBox小節範囲終了;
		private System.Windows.Forms.Label label説明;
		private System.Windows.Forms.CheckBox checkBox小節範囲指定;
		private System.Windows.Forms.CheckBox checkBoxチップ範囲指定;
		private System.Windows.Forms.CheckBox checkBoxレーン指定;
		private System.Windows.Forms.CheckedListBox checkedListBoxレーン選択リスト;
		private System.Windows.Forms.CheckBox checkBox表チップ;
		private System.Windows.Forms.CheckBox checkBox裏チップ;
		private System.Windows.Forms.Button buttonNONE;
		private System.Windows.Forms.Button buttonALL;
	}
}