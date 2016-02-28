namespace DTXCreator.WAV_BMP_AVI
{
	partial class C動画プロパティダイアログ
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( C動画プロパティダイアログ ) );
			this.textBoxAVI番号 = new System.Windows.Forms.TextBox();
			this.labelAVI番号 = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonキャンセル = new System.Windows.Forms.Button();
			this.textBoxラベル = new System.Windows.Forms.TextBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip( this.components );
			this.labelラベル = new System.Windows.Forms.Label();
			this.textBoxファイル = new System.Windows.Forms.TextBox();
			this.labelファイル = new System.Windows.Forms.Label();
			this.button参照 = new System.Windows.Forms.Button();
			this.button背景色 = new System.Windows.Forms.Button();
			this.button文字色 = new System.Windows.Forms.Button();
			this.button標準色に戻す = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textBoxAVI番号
			// 
			resources.ApplyResources( this.textBoxAVI番号, "textBoxAVI番号" );
			this.textBoxAVI番号.Name = "textBoxAVI番号";
			this.textBoxAVI番号.ReadOnly = true;
			this.textBoxAVI番号.TabStop = false;
			// 
			// labelAVI番号
			// 
			resources.ApplyResources( this.labelAVI番号, "labelAVI番号" );
			this.labelAVI番号.Name = "labelAVI番号";
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
			// button参照
			// 
			resources.ApplyResources( this.button参照, "button参照" );
			this.button参照.Name = "button参照";
			this.toolTip1.SetToolTip( this.button参照, resources.GetString( "button参照.ToolTip" ) );
			this.button参照.UseVisualStyleBackColor = true;
			this.button参照.Click += new System.EventHandler( this.button参照_Click );
			this.button参照.KeyDown += new System.Windows.Forms.KeyEventHandler( this.button参照_KeyDown );
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
			// C動画プロパティダイアログ
			// 
			resources.ApplyResources( this, "$this" );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ControlBox = false;
			this.Controls.Add( this.button標準色に戻す );
			this.Controls.Add( this.button文字色 );
			this.Controls.Add( this.button背景色 );
			this.Controls.Add( this.button参照 );
			this.Controls.Add( this.labelファイル );
			this.Controls.Add( this.textBoxファイル );
			this.Controls.Add( this.labelラベル );
			this.Controls.Add( this.textBoxラベル );
			this.Controls.Add( this.buttonキャンセル );
			this.Controls.Add( this.buttonOK );
			this.Controls.Add( this.labelAVI番号 );
			this.Controls.Add( this.textBoxAVI番号 );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "C動画プロパティダイアログ";
			this.TopMost = true;
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TextBox textBoxAVI番号;
		private System.Windows.Forms.Label labelAVI番号;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonキャンセル;
		public System.Windows.Forms.TextBox textBoxラベル;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Label labelラベル;
		public System.Windows.Forms.TextBox textBoxファイル;
		private System.Windows.Forms.Label labelファイル;
		private System.Windows.Forms.Button button参照;
		private System.Windows.Forms.Button button背景色;
		private System.Windows.Forms.Button button文字色;
		private System.Windows.Forms.Button button標準色に戻す;
	}
}