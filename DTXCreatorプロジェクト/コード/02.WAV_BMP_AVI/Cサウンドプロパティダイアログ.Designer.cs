namespace DTXCreator.WAV_BMP_AVI
{
	partial class Cサウンドプロパティダイアログ
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( Cサウンドプロパティダイアログ ) );
			this.toolTip1 = new System.Windows.Forms.ToolTip( this.components );
			this.textBoxラベル = new System.Windows.Forms.TextBox();
			this.labelラベル = new System.Windows.Forms.Label();
			this.textBoxファイル = new System.Windows.Forms.TextBox();
			this.labelファイル = new System.Windows.Forms.Label();
			this.label音量 = new System.Windows.Forms.Label();
			this.label位置 = new System.Windows.Forms.Label();
			this.button参照 = new System.Windows.Forms.Button();
			this.hScrollBar音量 = new System.Windows.Forms.HScrollBar();
			this.textBox音量 = new System.Windows.Forms.TextBox();
			this.textBox位置 = new System.Windows.Forms.TextBox();
			this.hScrollBar位置 = new System.Windows.Forms.HScrollBar();
			this.button背景色 = new System.Windows.Forms.Button();
			this.button文字色 = new System.Windows.Forms.Button();
			this.button標準色に戻す = new System.Windows.Forms.Button();
			this.button試聴 = new System.Windows.Forms.Button();
			this.checkBoxBGM = new System.Windows.Forms.CheckBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonキャンセル = new System.Windows.Forms.Button();
			this.textBoxWAV番号 = new System.Windows.Forms.TextBox();
			this.labelWAV番号 = new System.Windows.Forms.Label();
			this.label音量無音 = new System.Windows.Forms.Label();
			this.label位置左 = new System.Windows.Forms.Label();
			this.labe音量原音 = new System.Windows.Forms.Label();
			this.label位置右 = new System.Windows.Forms.Label();
			this.label位置中央 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// textBoxラベル
			// 
			resources.ApplyResources( this.textBoxラベル, "textBoxラベル" );
			this.textBoxラベル.Name = "textBoxラベル";
			this.toolTip1.SetToolTip( this.textBoxラベル, resources.GetString( "textBoxラベル.ToolTip" ) );
			this.textBoxラベル.KeyDown += new System.Windows.Forms.KeyEventHandler( this.textBoxラベル_KeyDown );
			// 
			// labelラベル
			// 
			resources.ApplyResources( this.labelラベル, "labelラベル" );
			this.labelラベル.Name = "labelラベル";
			this.toolTip1.SetToolTip( this.labelラベル, resources.GetString( "labelラベル.ToolTip" ) );
			// 
			// textBoxファイル
			// 
			resources.ApplyResources( this.textBoxファイル, "textBoxファイル" );
			this.textBoxファイル.Name = "textBoxファイル";
			this.toolTip1.SetToolTip( this.textBoxファイル, resources.GetString( "textBoxファイル.ToolTip" ) );
			this.textBoxファイル.KeyDown += new System.Windows.Forms.KeyEventHandler( this.textBoxファイル_KeyDown );
			// 
			// labelファイル
			// 
			resources.ApplyResources( this.labelファイル, "labelファイル" );
			this.labelファイル.Name = "labelファイル";
			this.toolTip1.SetToolTip( this.labelファイル, resources.GetString( "labelファイル.ToolTip" ) );
			// 
			// label音量
			// 
			resources.ApplyResources( this.label音量, "label音量" );
			this.label音量.Name = "label音量";
			this.toolTip1.SetToolTip( this.label音量, resources.GetString( "label音量.ToolTip" ) );
			// 
			// label位置
			// 
			resources.ApplyResources( this.label位置, "label位置" );
			this.label位置.Name = "label位置";
			this.toolTip1.SetToolTip( this.label位置, resources.GetString( "label位置.ToolTip" ) );
			// 
			// button参照
			// 
			resources.ApplyResources( this.button参照, "button参照" );
			this.button参照.Name = "button参照";
			this.toolTip1.SetToolTip( this.button参照, resources.GetString( "button参照.ToolTip" ) );
			this.button参照.UseVisualStyleBackColor = true;
			this.button参照.Click += new System.EventHandler( this.button参照_Click );
			this.button参照.KeyDown += new System.Windows.Forms.KeyEventHandler( this.button参照_KeyDown );
			// 
			// hScrollBar音量
			// 
			resources.ApplyResources( this.hScrollBar音量, "hScrollBar音量" );
			this.hScrollBar音量.Maximum = 109;
			this.hScrollBar音量.Name = "hScrollBar音量";
			this.toolTip1.SetToolTip( this.hScrollBar音量, resources.GetString( "hScrollBar音量.ToolTip" ) );
			this.hScrollBar音量.Value = 100;
			this.hScrollBar音量.ValueChanged += new System.EventHandler( this.hScrollBar音量_ValueChanged );
			// 
			// textBox音量
			// 
			resources.ApplyResources( this.textBox音量, "textBox音量" );
			this.textBox音量.Name = "textBox音量";
			this.toolTip1.SetToolTip( this.textBox音量, resources.GetString( "textBox音量.ToolTip" ) );
			this.textBox音量.KeyDown += new System.Windows.Forms.KeyEventHandler( this.textBox音量_KeyDown );
			this.textBox音量.Leave += new System.EventHandler( this.textBox音量_Leave );
			// 
			// textBox位置
			// 
			resources.ApplyResources( this.textBox位置, "textBox位置" );
			this.textBox位置.Name = "textBox位置";
			this.toolTip1.SetToolTip( this.textBox位置, resources.GetString( "textBox位置.ToolTip" ) );
			this.textBox位置.KeyDown += new System.Windows.Forms.KeyEventHandler( this.textBox位置_KeyDown );
			this.textBox位置.Leave += new System.EventHandler( this.textBox位置_Leave );
			// 
			// hScrollBar位置
			// 
			resources.ApplyResources( this.hScrollBar位置, "hScrollBar位置" );
			this.hScrollBar位置.Maximum = 209;
			this.hScrollBar位置.Name = "hScrollBar位置";
			this.toolTip1.SetToolTip( this.hScrollBar位置, resources.GetString( "hScrollBar位置.ToolTip" ) );
			this.hScrollBar位置.Value = 100;
			this.hScrollBar位置.ValueChanged += new System.EventHandler( this.hScrollBar位置_ValueChanged );
			// 
			// button背景色
			// 
			resources.ApplyResources( this.button背景色, "button背景色" );
			this.button背景色.Name = "button背景色";
			this.toolTip1.SetToolTip( this.button背景色, resources.GetString( "button背景色.ToolTip" ) );
			this.button背景色.UseVisualStyleBackColor = true;
			this.button背景色.Click += new System.EventHandler( this.button背景色_Click );
			this.button背景色.KeyDown += new System.Windows.Forms.KeyEventHandler( this.button背景色_KeyDown );
			// 
			// button文字色
			// 
			resources.ApplyResources( this.button文字色, "button文字色" );
			this.button文字色.Name = "button文字色";
			this.toolTip1.SetToolTip( this.button文字色, resources.GetString( "button文字色.ToolTip" ) );
			this.button文字色.UseVisualStyleBackColor = true;
			this.button文字色.Click += new System.EventHandler( this.button文字色_Click );
			this.button文字色.KeyDown += new System.Windows.Forms.KeyEventHandler( this.button文字色_KeyDown );
			// 
			// button標準色に戻す
			// 
			resources.ApplyResources( this.button標準色に戻す, "button標準色に戻す" );
			this.button標準色に戻す.Name = "button標準色に戻す";
			this.toolTip1.SetToolTip( this.button標準色に戻す, resources.GetString( "button標準色に戻す.ToolTip" ) );
			this.button標準色に戻す.UseVisualStyleBackColor = true;
			this.button標準色に戻す.Click += new System.EventHandler( this.button標準色に戻す_Click );
			this.button標準色に戻す.KeyDown += new System.Windows.Forms.KeyEventHandler( this.button標準色に戻す_KeyDown );
			// 
			// button試聴
			// 
			resources.ApplyResources( this.button試聴, "button試聴" );
			this.button試聴.Name = "button試聴";
			this.toolTip1.SetToolTip( this.button試聴, resources.GetString( "button試聴.ToolTip" ) );
			this.button試聴.UseVisualStyleBackColor = true;
			this.button試聴.Click += new System.EventHandler( this.button試聴_Click );
			this.button試聴.KeyDown += new System.Windows.Forms.KeyEventHandler( this.button試聴_KeyDown );
			// 
			// checkBoxBGM
			// 
			resources.ApplyResources( this.checkBoxBGM, "checkBoxBGM" );
			this.checkBoxBGM.Name = "checkBoxBGM";
			this.toolTip1.SetToolTip( this.checkBoxBGM, resources.GetString( "checkBoxBGM.ToolTip" ) );
			this.checkBoxBGM.UseVisualStyleBackColor = true;
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources( this.buttonOK, "buttonOK" );
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			// 
			// buttonキャンセル
			// 
			this.buttonキャンセル.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources( this.buttonキャンセル, "buttonキャンセル" );
			this.buttonキャンセル.Name = "buttonキャンセル";
			this.buttonキャンセル.UseVisualStyleBackColor = true;
			// 
			// textBoxWAV番号
			// 
			resources.ApplyResources( this.textBoxWAV番号, "textBoxWAV番号" );
			this.textBoxWAV番号.Name = "textBoxWAV番号";
			this.textBoxWAV番号.ReadOnly = true;
			this.textBoxWAV番号.TabStop = false;
			// 
			// labelWAV番号
			// 
			resources.ApplyResources( this.labelWAV番号, "labelWAV番号" );
			this.labelWAV番号.Name = "labelWAV番号";
			// 
			// label音量無音
			// 
			resources.ApplyResources( this.label音量無音, "label音量無音" );
			this.label音量無音.Name = "label音量無音";
			// 
			// label位置左
			// 
			resources.ApplyResources( this.label位置左, "label位置左" );
			this.label位置左.Name = "label位置左";
			// 
			// labe音量原音
			// 
			resources.ApplyResources( this.labe音量原音, "labe音量原音" );
			this.labe音量原音.Name = "labe音量原音";
			// 
			// label位置右
			// 
			resources.ApplyResources( this.label位置右, "label位置右" );
			this.label位置右.Name = "label位置右";
			// 
			// label位置中央
			// 
			resources.ApplyResources( this.label位置中央, "label位置中央" );
			this.label位置中央.Name = "label位置中央";
			// 
			// Cサウンドプロパティダイアログ
			// 
			resources.ApplyResources( this, "$this" );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ControlBox = false;
			this.Controls.Add( this.checkBoxBGM );
			this.Controls.Add( this.button試聴 );
			this.Controls.Add( this.button標準色に戻す );
			this.Controls.Add( this.button文字色 );
			this.Controls.Add( this.button背景色 );
			this.Controls.Add( this.label位置中央 );
			this.Controls.Add( this.label位置右 );
			this.Controls.Add( this.labe音量原音 );
			this.Controls.Add( this.hScrollBar位置 );
			this.Controls.Add( this.textBox位置 );
			this.Controls.Add( this.textBox音量 );
			this.Controls.Add( this.hScrollBar音量 );
			this.Controls.Add( this.label位置左 );
			this.Controls.Add( this.label音量無音 );
			this.Controls.Add( this.button参照 );
			this.Controls.Add( this.labelWAV番号 );
			this.Controls.Add( this.textBoxWAV番号 );
			this.Controls.Add( this.label位置 );
			this.Controls.Add( this.label音量 );
			this.Controls.Add( this.labelファイル );
			this.Controls.Add( this.textBoxファイル );
			this.Controls.Add( this.labelラベル );
			this.Controls.Add( this.textBoxラベル );
			this.Controls.Add( this.buttonキャンセル );
			this.Controls.Add( this.buttonOK );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Cサウンドプロパティダイアログ";
			this.TopMost = true;
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonキャンセル;
		internal System.Windows.Forms.TextBox textBoxラベル;
		private System.Windows.Forms.Label labelラベル;
		internal System.Windows.Forms.TextBox textBoxファイル;
		private System.Windows.Forms.Label labelファイル;
		private System.Windows.Forms.Label label音量;
		private System.Windows.Forms.Label label位置;
		public System.Windows.Forms.TextBox textBoxWAV番号;
		private System.Windows.Forms.Label labelWAV番号;
		private System.Windows.Forms.Button button参照;
		private System.Windows.Forms.Label label音量無音;
		private System.Windows.Forms.Label label位置左;
		public System.Windows.Forms.HScrollBar hScrollBar音量;
		internal System.Windows.Forms.TextBox textBox音量;
		internal System.Windows.Forms.TextBox textBox位置;
		public System.Windows.Forms.HScrollBar hScrollBar位置;
		private System.Windows.Forms.Label labe音量原音;
		private System.Windows.Forms.Label label位置右;
		private System.Windows.Forms.Label label位置中央;
		private System.Windows.Forms.Button button背景色;
		private System.Windows.Forms.Button button文字色;
		private System.Windows.Forms.Button button標準色に戻す;
		private System.Windows.Forms.Button button試聴;
		internal System.Windows.Forms.CheckBox checkBoxBGM;
	}
}