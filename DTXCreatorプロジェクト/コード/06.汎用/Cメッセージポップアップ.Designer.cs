namespace DTXCreator.汎用
{
	partial class Cメッセージポップアップ
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( Cメッセージポップアップ ) );
			this.panelメッセージ = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			( (System.ComponentModel.ISupportInitialize) ( this.pictureBox1 ) ).BeginInit();
			this.SuspendLayout();
			// 
			// panelメッセージ
			// 
			this.panelメッセージ.AccessibleDescription = null;
			this.panelメッセージ.AccessibleName = null;
			resources.ApplyResources( this.panelメッセージ, "panelメッセージ" );
			this.panelメッセージ.BackgroundImage = null;
			this.panelメッセージ.Font = null;
			this.panelメッセージ.Name = "panelメッセージ";
			this.panelメッセージ.Paint += new System.Windows.Forms.PaintEventHandler( this.panelメッセージ_Paint );
			// 
			// pictureBox1
			// 
			this.pictureBox1.AccessibleDescription = null;
			this.pictureBox1.AccessibleName = null;
			resources.ApplyResources( this.pictureBox1, "pictureBox1" );
			this.pictureBox1.BackgroundImage = null;
			this.pictureBox1.Font = null;
			this.pictureBox1.Image = global::DTXCreator.Properties.Resources.りらちょー;
			this.pictureBox1.ImageLocation = null;
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			// 
			// Cメッセージポップアップ
			// 
			this.AccessibleDescription = null;
			this.AccessibleName = null;
			resources.ApplyResources( this, "$this" );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.BackgroundImage = null;
			this.ControlBox = false;
			this.Controls.Add( this.pictureBox1 );
			this.Controls.Add( this.panelメッセージ );
			this.Font = null;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = null;
			this.Name = "Cメッセージポップアップ";
			this.Load += new System.EventHandler( this.Cメッセージポップアップ_Load );
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.Cメッセージポップアップ_FormClosing );
			( (System.ComponentModel.ISupportInitialize) ( this.pictureBox1 ) ).EndInit();
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.Panel panelメッセージ;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}