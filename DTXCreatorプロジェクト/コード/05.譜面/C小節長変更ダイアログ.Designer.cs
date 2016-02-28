namespace DTXCreator.譜面
{
	partial class C小節長変更ダイアログ
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( C小節長変更ダイアログ ) );
			this.numericUpDown小節長の倍率 = new System.Windows.Forms.NumericUpDown();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonキャンセル = new System.Windows.Forms.Button();
			this.textBox小節番号 = new System.Windows.Forms.TextBox();
			this.checkBox後続設定 = new System.Windows.Forms.CheckBox();
			this.label小節長倍率 = new System.Windows.Forms.Label();
			this.label小節番号 = new System.Windows.Forms.Label();
			( (System.ComponentModel.ISupportInitialize) ( this.numericUpDown小節長の倍率 ) ).BeginInit();
			this.SuspendLayout();
			// 
			// numericUpDown小節長の倍率
			// 
			this.numericUpDown小節長の倍率.DecimalPlaces = 3;
			resources.ApplyResources( this.numericUpDown小節長の倍率, "numericUpDown小節長の倍率" );
			this.numericUpDown小節長の倍率.Increment = new decimal( new int[] {
            5,
            0,
            0,
            131072} );
			this.numericUpDown小節長の倍率.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            196608} );
			this.numericUpDown小節長の倍率.Name = "numericUpDown小節長の倍率";
			this.numericUpDown小節長の倍率.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
			this.numericUpDown小節長の倍率.KeyDown += new System.Windows.Forms.KeyEventHandler( this.numericUpDown小節長の倍率_KeyDown );
			// 
			// buttonOK
			// 
			resources.ApplyResources( this.buttonOK, "buttonOK" );
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			// 
			// buttonキャンセル
			// 
			resources.ApplyResources( this.buttonキャンセル, "buttonキャンセル" );
			this.buttonキャンセル.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonキャンセル.Name = "buttonキャンセル";
			this.buttonキャンセル.UseVisualStyleBackColor = true;
			// 
			// textBox小節番号
			// 
			this.textBox小節番号.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources( this.textBox小節番号, "textBox小節番号" );
			this.textBox小節番号.Name = "textBox小節番号";
			this.textBox小節番号.ReadOnly = true;
			// 
			// checkBox後続設定
			// 
			resources.ApplyResources( this.checkBox後続設定, "checkBox後続設定" );
			this.checkBox後続設定.Name = "checkBox後続設定";
			this.checkBox後続設定.UseVisualStyleBackColor = true;
			this.checkBox後続設定.KeyDown += new System.Windows.Forms.KeyEventHandler( this.checkBox後続設定_KeyDown );
			// 
			// label小節長倍率
			// 
			resources.ApplyResources( this.label小節長倍率, "label小節長倍率" );
			this.label小節長倍率.Name = "label小節長倍率";
			// 
			// label小節番号
			// 
			resources.ApplyResources( this.label小節番号, "label小節番号" );
			this.label小節番号.Name = "label小節番号";
			// 
			// C小節長変更ダイアログ
			// 
			resources.ApplyResources( this, "$this" );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ControlBox = false;
			this.Controls.Add( this.label小節長倍率 );
			this.Controls.Add( this.numericUpDown小節長の倍率 );
			this.Controls.Add( this.label小節番号 );
			this.Controls.Add( this.checkBox後続設定 );
			this.Controls.Add( this.textBox小節番号 );
			this.Controls.Add( this.buttonキャンセル );
			this.Controls.Add( this.buttonOK );
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "C小節長変更ダイアログ";
			( (System.ComponentModel.ISupportInitialize) ( this.numericUpDown小節長の倍率 ) ).EndInit();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NumericUpDown numericUpDown小節長の倍率;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonキャンセル;
		private System.Windows.Forms.TextBox textBox小節番号;
		private System.Windows.Forms.CheckBox checkBox後続設定;
		private System.Windows.Forms.Label label小節長倍率;
		private System.Windows.Forms.Label label小節番号;
	}
}