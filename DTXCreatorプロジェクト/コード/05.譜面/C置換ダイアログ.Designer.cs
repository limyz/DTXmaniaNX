namespace DTXCreator.譜面
{
	partial class C置換ダイアログ
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( C置換ダイアログ ) );
			this.buttonキャンセル = new System.Windows.Forms.Button();
			this.button置換 = new System.Windows.Forms.Button();
			this.label説明 = new System.Windows.Forms.Label();
			this.radioButton表裏反転 = new System.Windows.Forms.RadioButton();
			this.radioButton単純置換 = new System.Windows.Forms.RadioButton();
			this.textBox元番号 = new System.Windows.Forms.TextBox();
			this.textBox先番号 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonキャンセル
			// 
			resources.ApplyResources( this.buttonキャンセル, "buttonキャンセル" );
			this.buttonキャンセル.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonキャンセル.Name = "buttonキャンセル";
			this.buttonキャンセル.UseVisualStyleBackColor = true;
			// 
			// button置換
			// 
			resources.ApplyResources( this.button置換, "button置換" );
			this.button置換.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button置換.Name = "button置換";
			this.button置換.UseVisualStyleBackColor = true;
			// 
			// label説明
			// 
			resources.ApplyResources( this.label説明, "label説明" );
			this.label説明.Name = "label説明";
			// 
			// radioButton表裏反転
			// 
			resources.ApplyResources( this.radioButton表裏反転, "radioButton表裏反転" );
			this.radioButton表裏反転.Name = "radioButton表裏反転";
			this.radioButton表裏反転.TabStop = true;
			this.radioButton表裏反転.UseVisualStyleBackColor = true;
			this.radioButton表裏反転.KeyDown += new System.Windows.Forms.KeyEventHandler( this.radioButton表裏反転_KeyDown );
			// 
			// radioButton単純置換
			// 
			resources.ApplyResources( this.radioButton単純置換, "radioButton単純置換" );
			this.radioButton単純置換.Name = "radioButton単純置換";
			this.radioButton単純置換.TabStop = true;
			this.radioButton単純置換.UseVisualStyleBackColor = true;
			this.radioButton単純置換.KeyDown += new System.Windows.Forms.KeyEventHandler( this.radioButton単純置換_KeyDown );
			// 
			// textBox元番号
			// 
			resources.ApplyResources( this.textBox元番号, "textBox元番号" );
			this.textBox元番号.Name = "textBox元番号";
			this.textBox元番号.KeyDown += new System.Windows.Forms.KeyEventHandler( this.textBox元番号_KeyDown );
			// 
			// textBox先番号
			// 
			resources.ApplyResources( this.textBox先番号, "textBox先番号" );
			this.textBox先番号.Name = "textBox先番号";
			this.textBox先番号.KeyDown += new System.Windows.Forms.KeyEventHandler( this.textBox先番号_KeyDown );
			// 
			// label1
			// 
			resources.ApplyResources( this.label1, "label1" );
			this.label1.Name = "label1";
			// 
			// label2
			// 
			resources.ApplyResources( this.label2, "label2" );
			this.label2.Name = "label2";
			// 
			// C置換ダイアログ
			// 
			resources.ApplyResources( this, "$this" );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ControlBox = false;
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.textBox先番号 );
			this.Controls.Add( this.textBox元番号 );
			this.Controls.Add( this.radioButton単純置換 );
			this.Controls.Add( this.radioButton表裏反転 );
			this.Controls.Add( this.label説明 );
			this.Controls.Add( this.button置換 );
			this.Controls.Add( this.buttonキャンセル );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "C置換ダイアログ";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.C置換ダイアログ_FormClosing );
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonキャンセル;
		private System.Windows.Forms.Button button置換;
		private System.Windows.Forms.Label label説明;
		private System.Windows.Forms.RadioButton radioButton表裏反転;
		private System.Windows.Forms.RadioButton radioButton単純置換;
		private System.Windows.Forms.TextBox textBox元番号;
		private System.Windows.Forms.TextBox textBox先番号;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}