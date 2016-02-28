namespace DTXCreator.オプション関連
{
	partial class Cオプションダイアログ
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( Cオプションダイアログ ) );
			this.tabPage全般 = new System.Windows.Forms.TabPage();
			this.checkBoxPlaySoundOnChip = new System.Windows.Forms.CheckBox();
			this.checkBoxPreviewBGM = new System.Windows.Forms.CheckBox();
			this.checkBoxオートフォーカス = new System.Windows.Forms.CheckBox();
			this.label個まで表示する = new System.Windows.Forms.Label();
			this.checkBox最近使用したファイル = new System.Windows.Forms.CheckBox();
			this.numericUpDown最近使用したファイルの最大表示個数 = new System.Windows.Forms.NumericUpDown();
			this.tabControlオプション = new System.Windows.Forms.TabControl();
			this.tabPageLanes = new System.Windows.Forms.TabPage();
			this.labelSelectLanes = new System.Windows.Forms.Label();
			this.checkedListBoxLaneSelectList = new System.Windows.Forms.CheckedListBox();
			this.button1 = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.tabPage全般.SuspendLayout();
			( (System.ComponentModel.ISupportInitialize) ( this.numericUpDown最近使用したファイルの最大表示個数 ) ).BeginInit();
			this.tabControlオプション.SuspendLayout();
			this.tabPageLanes.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabPage全般
			// 
			this.tabPage全般.Controls.Add( this.checkBoxPlaySoundOnChip );
			this.tabPage全般.Controls.Add( this.checkBoxPreviewBGM );
			this.tabPage全般.Controls.Add( this.checkBoxオートフォーカス );
			this.tabPage全般.Controls.Add( this.label個まで表示する );
			this.tabPage全般.Controls.Add( this.checkBox最近使用したファイル );
			this.tabPage全般.Controls.Add( this.numericUpDown最近使用したファイルの最大表示個数 );
			resources.ApplyResources( this.tabPage全般, "tabPage全般" );
			this.tabPage全般.Name = "tabPage全般";
			this.tabPage全般.UseVisualStyleBackColor = true;
			// 
			// checkBoxPlaySoundOnChip
			// 
			resources.ApplyResources( this.checkBoxPlaySoundOnChip, "checkBoxPlaySoundOnChip" );
			this.checkBoxPlaySoundOnChip.Name = "checkBoxPlaySoundOnChip";
			this.checkBoxPlaySoundOnChip.UseVisualStyleBackColor = true;
			// 
			// checkBoxPreviewBGM
			// 
			resources.ApplyResources( this.checkBoxPreviewBGM, "checkBoxPreviewBGM" );
			this.checkBoxPreviewBGM.Name = "checkBoxPreviewBGM";
			this.checkBoxPreviewBGM.UseVisualStyleBackColor = true;
			// 
			// checkBoxオートフォーカス
			// 
			resources.ApplyResources( this.checkBoxオートフォーカス, "checkBoxオートフォーカス" );
			this.checkBoxオートフォーカス.Name = "checkBoxオートフォーカス";
			this.checkBoxオートフォーカス.UseVisualStyleBackColor = true;
			// 
			// label個まで表示する
			// 
			resources.ApplyResources( this.label個まで表示する, "label個まで表示する" );
			this.label個まで表示する.Name = "label個まで表示する";
			// 
			// checkBox最近使用したファイル
			// 
			resources.ApplyResources( this.checkBox最近使用したファイル, "checkBox最近使用したファイル" );
			this.checkBox最近使用したファイル.Name = "checkBox最近使用したファイル";
			this.checkBox最近使用したファイル.UseVisualStyleBackColor = true;
			// 
			// numericUpDown最近使用したファイルの最大表示個数
			// 
			resources.ApplyResources( this.numericUpDown最近使用したファイルの最大表示個数, "numericUpDown最近使用したファイルの最大表示個数" );
			this.numericUpDown最近使用したファイルの最大表示個数.Maximum = new decimal( new int[] {
            10,
            0,
            0,
            0} );
			this.numericUpDown最近使用したファイルの最大表示個数.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            0} );
			this.numericUpDown最近使用したファイルの最大表示個数.Name = "numericUpDown最近使用したファイルの最大表示個数";
			this.numericUpDown最近使用したファイルの最大表示個数.Value = new decimal( new int[] {
            1,
            0,
            0,
            0} );
			// 
			// tabControlオプション
			// 
			resources.ApplyResources( this.tabControlオプション, "tabControlオプション" );
			this.tabControlオプション.Controls.Add( this.tabPage全般 );
			this.tabControlオプション.Controls.Add( this.tabPageLanes );
			this.tabControlオプション.Name = "tabControlオプション";
			this.tabControlオプション.SelectedIndex = 0;
			this.tabControlオプション.KeyDown += new System.Windows.Forms.KeyEventHandler( this.tabControlオプション_KeyDown );
			// 
			// tabPageLanes
			// 
			this.tabPageLanes.Controls.Add( this.labelSelectLanes );
			this.tabPageLanes.Controls.Add( this.checkedListBoxLaneSelectList );
			resources.ApplyResources( this.tabPageLanes, "tabPageLanes" );
			this.tabPageLanes.Name = "tabPageLanes";
			this.tabPageLanes.UseVisualStyleBackColor = true;
			// 
			// labelSelectLanes
			// 
			resources.ApplyResources( this.labelSelectLanes, "labelSelectLanes" );
			this.labelSelectLanes.Name = "labelSelectLanes";
			// 
			// checkedListBoxLaneSelectList
			// 
			this.checkedListBoxLaneSelectList.CheckOnClick = true;
			this.checkedListBoxLaneSelectList.FormattingEnabled = true;
			resources.ApplyResources( this.checkedListBoxLaneSelectList, "checkedListBoxLaneSelectList" );
			this.checkedListBoxLaneSelectList.Name = "checkedListBoxLaneSelectList";
			// 
			// button1
			// 
			resources.ApplyResources( this.button1, "button1" );
			this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button1.Name = "button1";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// buttonOK
			// 
			resources.ApplyResources( this.buttonOK, "buttonOK" );
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			// 
			// Cオプションダイアログ
			// 
			resources.ApplyResources( this, "$this" );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ControlBox = false;
			this.Controls.Add( this.buttonOK );
			this.Controls.Add( this.button1 );
			this.Controls.Add( this.tabControlオプション );
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Cオプションダイアログ";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler( this.Cオプションダイアログ_KeyDown );
			this.tabPage全般.ResumeLayout( false );
			this.tabPage全般.PerformLayout();
			( (System.ComponentModel.ISupportInitialize) ( this.numericUpDown最近使用したファイルの最大表示個数 ) ).EndInit();
			this.tabControlオプション.ResumeLayout( false );
			this.tabPageLanes.ResumeLayout( false );
			this.tabPageLanes.PerformLayout();
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.TabPage tabPage全般;
		internal System.Windows.Forms.CheckBox checkBoxオートフォーカス;
		private System.Windows.Forms.Label label個まで表示する;
		internal System.Windows.Forms.CheckBox checkBox最近使用したファイル;
		internal System.Windows.Forms.NumericUpDown numericUpDown最近使用したファイルの最大表示個数;
		private System.Windows.Forms.TabControl tabControlオプション;
		internal System.Windows.Forms.CheckBox checkBoxPreviewBGM;
		internal System.Windows.Forms.CheckBox checkBoxPlaySoundOnChip;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.TabPage tabPageLanes;
		internal System.Windows.Forms.CheckedListBox checkedListBoxLaneSelectList;
		private System.Windows.Forms.Label labelSelectLanes;

	}
}