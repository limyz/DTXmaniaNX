namespace DTXCreator.Options  // オプション関連
{
	partial class COptionsDialog // Cオプションダイアログ
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(COptionsDialog));
            this.tabPage全般 = new System.Windows.Forms.TabPage();
            this.groupBoxDefaultOperationMode = new System.Windows.Forms.GroupBox();
            this.pictureBox_EditMode = new System.Windows.Forms.PictureBox();
            this.pictureBox_SelectMode = new System.Windows.Forms.PictureBox();
            this.radioButton_EditMode = new System.Windows.Forms.RadioButton();
            this.radioButton_SelectMode = new System.Windows.Forms.RadioButton();
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
            this.tabPageViewer = new System.Windows.Forms.TabPage();
            this.groupBox_SelectViewer = new System.Windows.Forms.GroupBox();
            this.groupBox_WindowsSizeSettings = new System.Windows.Forms.GroupBox();
            this.radioButton_WinSize360 = new System.Windows.Forms.RadioButton();
            this.radioButton_WinSize540 = new System.Windows.Forms.RadioButton();
            this.radioButton_WinSize720 = new System.Windows.Forms.RadioButton();
            this.radioButton_WinSize1080 = new System.Windows.Forms.RadioButton();
            this.button_DTXViewerPath = new System.Windows.Forms.Button();
            this.textBox_DTXViewerPath = new System.Windows.Forms.TextBox();
            this.groupBox_DTXManiaSettings = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_TimeStretch = new System.Windows.Forms.CheckBox();
            this.checkBox_VSyncWait = new System.Windows.Forms.CheckBox();
            this.checkBox_GRmode = new System.Windows.Forms.CheckBox();
            this.label_Notice = new System.Windows.Forms.Label();
            this.groupBox_SoundDeviceSettings = new System.Windows.Forms.GroupBox();
            this.radioButton_WASAPI_Shared = new System.Windows.Forms.RadioButton();
            this.radioButton_DirectSound = new System.Windows.Forms.RadioButton();
            this.radioButton_WASAPI_Exclusive = new System.Windows.Forms.RadioButton();
            this.comboBox_ASIOdevices = new System.Windows.Forms.ComboBox();
            this.radioButton_ASIO = new System.Windows.Forms.RadioButton();
            this.radioButton_UseDTXViewer = new System.Windows.Forms.RadioButton();
            this.radioButton_UseDTXManiaGR = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.openFileDialog_DTXViewerPath = new System.Windows.Forms.OpenFileDialog();
            this.tabPage全般.SuspendLayout();
            this.groupBoxDefaultOperationMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_EditMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_SelectMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown最近使用したファイルの最大表示個数)).BeginInit();
            this.tabControlオプション.SuspendLayout();
            this.tabPageLanes.SuspendLayout();
            this.tabPageViewer.SuspendLayout();
            this.groupBox_SelectViewer.SuspendLayout();
            this.groupBox_WindowsSizeSettings.SuspendLayout();
            this.groupBox_DTXManiaSettings.SuspendLayout();
            this.groupBox_SoundDeviceSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage全般
            // 
            this.tabPage全般.Controls.Add(this.groupBoxDefaultOperationMode);
            this.tabPage全般.Controls.Add(this.checkBoxPlaySoundOnChip);
            this.tabPage全般.Controls.Add(this.checkBoxPreviewBGM);
            this.tabPage全般.Controls.Add(this.checkBoxオートフォーカス);
            this.tabPage全般.Controls.Add(this.label個まで表示する);
            this.tabPage全般.Controls.Add(this.checkBox最近使用したファイル);
            this.tabPage全般.Controls.Add(this.numericUpDown最近使用したファイルの最大表示個数);
            resources.ApplyResources(this.tabPage全般, "tabPage全般");
            this.tabPage全般.Name = "tabPage全般";
            this.tabPage全般.UseVisualStyleBackColor = true;
            // 
            // groupBoxDefaultOperationMode
            // 
            this.groupBoxDefaultOperationMode.Controls.Add(this.pictureBox_EditMode);
            this.groupBoxDefaultOperationMode.Controls.Add(this.pictureBox_SelectMode);
            this.groupBoxDefaultOperationMode.Controls.Add(this.radioButton_EditMode);
            this.groupBoxDefaultOperationMode.Controls.Add(this.radioButton_SelectMode);
            resources.ApplyResources(this.groupBoxDefaultOperationMode, "groupBoxDefaultOperationMode");
            this.groupBoxDefaultOperationMode.Name = "groupBoxDefaultOperationMode";
            this.groupBoxDefaultOperationMode.TabStop = false;
            // 
            // pictureBox_EditMode
            // 
            this.pictureBox_EditMode.Image = global::DTXCreator.Properties.Resources.鉛筆;
            resources.ApplyResources(this.pictureBox_EditMode, "pictureBox_EditMode");
            this.pictureBox_EditMode.Name = "pictureBox_EditMode";
            this.pictureBox_EditMode.TabStop = false;
            // 
            // pictureBox_SelectMode
            // 
            this.pictureBox_SelectMode.Image = global::DTXCreator.Properties.Resources.矢印ポインタ;
            resources.ApplyResources(this.pictureBox_SelectMode, "pictureBox_SelectMode");
            this.pictureBox_SelectMode.Name = "pictureBox_SelectMode";
            this.pictureBox_SelectMode.TabStop = false;
            // 
            // radioButton_EditMode
            // 
            resources.ApplyResources(this.radioButton_EditMode, "radioButton_EditMode");
            this.radioButton_EditMode.Name = "radioButton_EditMode";
            this.radioButton_EditMode.TabStop = true;
            this.radioButton_EditMode.UseVisualStyleBackColor = true;
            this.radioButton_EditMode.CheckedChanged += new System.EventHandler(this.radioButtonEditMove_CheckedChanged);
            // 
            // radioButton_SelectMode
            // 
            resources.ApplyResources(this.radioButton_SelectMode, "radioButton_SelectMode");
            this.radioButton_SelectMode.Name = "radioButton_SelectMode";
            this.radioButton_SelectMode.TabStop = true;
            this.radioButton_SelectMode.UseVisualStyleBackColor = true;
            this.radioButton_SelectMode.CheckedChanged += new System.EventHandler(this.radioButtonSelectMode_CheckedChanged);
            // 
            // checkBoxPlaySoundOnChip
            // 
            resources.ApplyResources(this.checkBoxPlaySoundOnChip, "checkBoxPlaySoundOnChip");
            this.checkBoxPlaySoundOnChip.Name = "checkBoxPlaySoundOnChip";
            this.checkBoxPlaySoundOnChip.UseVisualStyleBackColor = true;
            // 
            // checkBoxPreviewBGM
            // 
            resources.ApplyResources(this.checkBoxPreviewBGM, "checkBoxPreviewBGM");
            this.checkBoxPreviewBGM.Name = "checkBoxPreviewBGM";
            this.checkBoxPreviewBGM.UseVisualStyleBackColor = true;
            // 
            // checkBoxオートフォーカス
            // 
            resources.ApplyResources(this.checkBoxオートフォーカス, "checkBoxオートフォーカス");
            this.checkBoxオートフォーカス.Name = "checkBoxオートフォーカス";
            this.checkBoxオートフォーカス.UseVisualStyleBackColor = true;
            // 
            // label個まで表示する
            // 
            resources.ApplyResources(this.label個まで表示する, "label個まで表示する");
            this.label個まで表示する.Name = "label個まで表示する";
            // 
            // checkBox最近使用したファイル
            // 
            resources.ApplyResources(this.checkBox最近使用したファイル, "checkBox最近使用したファイル");
            this.checkBox最近使用したファイル.Name = "checkBox最近使用したファイル";
            this.checkBox最近使用したファイル.UseVisualStyleBackColor = true;
            // 
            // numericUpDown最近使用したファイルの最大表示個数
            // 
            resources.ApplyResources(this.numericUpDown最近使用したファイルの最大表示個数, "numericUpDown最近使用したファイルの最大表示個数");
            this.numericUpDown最近使用したファイルの最大表示個数.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown最近使用したファイルの最大表示個数.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown最近使用したファイルの最大表示個数.Name = "numericUpDown最近使用したファイルの最大表示個数";
            this.numericUpDown最近使用したファイルの最大表示個数.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tabControlオプション
            // 
            resources.ApplyResources(this.tabControlオプション, "tabControlオプション");
            this.tabControlオプション.Controls.Add(this.tabPage全般);
            this.tabControlオプション.Controls.Add(this.tabPageLanes);
            this.tabControlオプション.Controls.Add(this.tabPageViewer);
            this.tabControlオプション.Name = "tabControlオプション";
            this.tabControlオプション.SelectedIndex = 0;
            this.tabControlオプション.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tabControlオプション_KeyDown);
            // 
            // tabPageLanes
            // 
            this.tabPageLanes.Controls.Add(this.labelSelectLanes);
            this.tabPageLanes.Controls.Add(this.checkedListBoxLaneSelectList);
            resources.ApplyResources(this.tabPageLanes, "tabPageLanes");
            this.tabPageLanes.Name = "tabPageLanes";
            this.tabPageLanes.UseVisualStyleBackColor = true;
            // 
            // labelSelectLanes
            // 
            resources.ApplyResources(this.labelSelectLanes, "labelSelectLanes");
            this.labelSelectLanes.Name = "labelSelectLanes";
            // 
            // checkedListBoxLaneSelectList
            // 
            this.checkedListBoxLaneSelectList.CheckOnClick = true;
            this.checkedListBoxLaneSelectList.FormattingEnabled = true;
            resources.ApplyResources(this.checkedListBoxLaneSelectList, "checkedListBoxLaneSelectList");
            this.checkedListBoxLaneSelectList.Name = "checkedListBoxLaneSelectList";
            // 
            // tabPageViewer
            // 
            this.tabPageViewer.Controls.Add(this.groupBox_SelectViewer);
            resources.ApplyResources(this.tabPageViewer, "tabPageViewer");
            this.tabPageViewer.Name = "tabPageViewer";
            this.tabPageViewer.UseVisualStyleBackColor = true;
            // 
            // groupBox_SelectViewer
            // 
            this.groupBox_SelectViewer.Controls.Add(this.groupBox_WindowsSizeSettings);
            this.groupBox_SelectViewer.Controls.Add(this.button_DTXViewerPath);
            this.groupBox_SelectViewer.Controls.Add(this.textBox_DTXViewerPath);
            this.groupBox_SelectViewer.Controls.Add(this.groupBox_DTXManiaSettings);
            this.groupBox_SelectViewer.Controls.Add(this.label_Notice);
            this.groupBox_SelectViewer.Controls.Add(this.groupBox_SoundDeviceSettings);
            this.groupBox_SelectViewer.Controls.Add(this.radioButton_UseDTXViewer);
            this.groupBox_SelectViewer.Controls.Add(this.radioButton_UseDTXManiaGR);
            resources.ApplyResources(this.groupBox_SelectViewer, "groupBox_SelectViewer");
            this.groupBox_SelectViewer.Name = "groupBox_SelectViewer";
            this.groupBox_SelectViewer.TabStop = false;
            // 
            // groupBox_WindowsSizeSettings
            // 
            this.groupBox_WindowsSizeSettings.Controls.Add(this.radioButton_WinSize360);
            this.groupBox_WindowsSizeSettings.Controls.Add(this.radioButton_WinSize540);
            this.groupBox_WindowsSizeSettings.Controls.Add(this.radioButton_WinSize720);
            this.groupBox_WindowsSizeSettings.Controls.Add(this.radioButton_WinSize1080);
            resources.ApplyResources(this.groupBox_WindowsSizeSettings, "groupBox_WindowsSizeSettings");
            this.groupBox_WindowsSizeSettings.Name = "groupBox_WindowsSizeSettings";
            this.groupBox_WindowsSizeSettings.TabStop = false;
            // 
            // radioButton_WinSize360
            // 
            resources.ApplyResources(this.radioButton_WinSize360, "radioButton_WinSize360");
            this.radioButton_WinSize360.Name = "radioButton_WinSize360";
            this.radioButton_WinSize360.TabStop = true;
            this.radioButton_WinSize360.UseVisualStyleBackColor = true;
            // 
            // radioButton_WinSize540
            // 
            resources.ApplyResources(this.radioButton_WinSize540, "radioButton_WinSize540");
            this.radioButton_WinSize540.Name = "radioButton_WinSize540";
            this.radioButton_WinSize540.TabStop = true;
            this.radioButton_WinSize540.UseVisualStyleBackColor = true;
            // 
            // radioButton_WinSize720
            // 
            resources.ApplyResources(this.radioButton_WinSize720, "radioButton_WinSize720");
            this.radioButton_WinSize720.Name = "radioButton_WinSize720";
            this.radioButton_WinSize720.TabStop = true;
            this.radioButton_WinSize720.UseVisualStyleBackColor = true;
            // 
            // radioButton_WinSize1080
            // 
            resources.ApplyResources(this.radioButton_WinSize1080, "radioButton_WinSize1080");
            this.radioButton_WinSize1080.Name = "radioButton_WinSize1080";
            this.radioButton_WinSize1080.TabStop = true;
            this.radioButton_WinSize1080.UseVisualStyleBackColor = true;
            // 
            // button_DTXViewerPath
            // 
            resources.ApplyResources(this.button_DTXViewerPath, "button_DTXViewerPath");
            this.button_DTXViewerPath.Name = "button_DTXViewerPath";
            this.button_DTXViewerPath.UseVisualStyleBackColor = true;
            this.button_DTXViewerPath.Click += new System.EventHandler(this.button_DTXViewerPath_Click);
            // 
            // textBox_DTXViewerPath
            // 
            resources.ApplyResources(this.textBox_DTXViewerPath, "textBox_DTXViewerPath");
            this.textBox_DTXViewerPath.Name = "textBox_DTXViewerPath";
            // 
            // groupBox_DTXManiaSettings
            // 
            this.groupBox_DTXManiaSettings.Controls.Add(this.label1);
            this.groupBox_DTXManiaSettings.Controls.Add(this.checkBox_TimeStretch);
            this.groupBox_DTXManiaSettings.Controls.Add(this.checkBox_VSyncWait);
            this.groupBox_DTXManiaSettings.Controls.Add(this.checkBox_GRmode);
            resources.ApplyResources(this.groupBox_DTXManiaSettings, "groupBox_DTXManiaSettings");
            this.groupBox_DTXManiaSettings.Name = "groupBox_DTXManiaSettings";
            this.groupBox_DTXManiaSettings.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // checkBox_TimeStretch
            // 
            resources.ApplyResources(this.checkBox_TimeStretch, "checkBox_TimeStretch");
            this.checkBox_TimeStretch.Name = "checkBox_TimeStretch";
            this.checkBox_TimeStretch.UseVisualStyleBackColor = true;
            // 
            // checkBox_VSyncWait
            // 
            resources.ApplyResources(this.checkBox_VSyncWait, "checkBox_VSyncWait");
            this.checkBox_VSyncWait.Name = "checkBox_VSyncWait";
            this.checkBox_VSyncWait.UseVisualStyleBackColor = true;
            // 
            // checkBox_GRmode
            // 
            resources.ApplyResources(this.checkBox_GRmode, "checkBox_GRmode");
            this.checkBox_GRmode.Name = "checkBox_GRmode";
            this.checkBox_GRmode.UseVisualStyleBackColor = true;
            // 
            // label_Notice
            // 
            resources.ApplyResources(this.label_Notice, "label_Notice");
            this.label_Notice.AutoEllipsis = true;
            this.label_Notice.Name = "label_Notice";
            // 
            // groupBox_SoundDeviceSettings
            // 
            this.groupBox_SoundDeviceSettings.Controls.Add(this.radioButton_WASAPI_Shared);
            this.groupBox_SoundDeviceSettings.Controls.Add(this.radioButton_DirectSound);
            this.groupBox_SoundDeviceSettings.Controls.Add(this.radioButton_WASAPI_Exclusive);
            this.groupBox_SoundDeviceSettings.Controls.Add(this.comboBox_ASIOdevices);
            this.groupBox_SoundDeviceSettings.Controls.Add(this.radioButton_ASIO);
            resources.ApplyResources(this.groupBox_SoundDeviceSettings, "groupBox_SoundDeviceSettings");
            this.groupBox_SoundDeviceSettings.Name = "groupBox_SoundDeviceSettings";
            this.groupBox_SoundDeviceSettings.TabStop = false;
            // 
            // radioButton_WASAPI_Shared
            // 
            resources.ApplyResources(this.radioButton_WASAPI_Shared, "radioButton_WASAPI_Shared");
            this.radioButton_WASAPI_Shared.Name = "radioButton_WASAPI_Shared";
            this.radioButton_WASAPI_Shared.TabStop = true;
            this.radioButton_WASAPI_Shared.UseVisualStyleBackColor = true;
            this.radioButton_WASAPI_Shared.CheckedChanged += new System.EventHandler(this.radioButton_WASAPI_Shared_CheckedChanged);
            // 
            // radioButton_DirectSound
            // 
            resources.ApplyResources(this.radioButton_DirectSound, "radioButton_DirectSound");
            this.radioButton_DirectSound.Name = "radioButton_DirectSound";
            this.radioButton_DirectSound.TabStop = true;
            this.radioButton_DirectSound.UseVisualStyleBackColor = true;
            this.radioButton_DirectSound.CheckedChanged += new System.EventHandler(this.radioButton_DirectSound_CheckedChanged);
            // 
            // radioButton_WASAPI_Exclusive
            // 
            resources.ApplyResources(this.radioButton_WASAPI_Exclusive, "radioButton_WASAPI_Exclusive");
            this.radioButton_WASAPI_Exclusive.Name = "radioButton_WASAPI_Exclusive";
            this.radioButton_WASAPI_Exclusive.TabStop = true;
            this.radioButton_WASAPI_Exclusive.UseVisualStyleBackColor = true;
            this.radioButton_WASAPI_Exclusive.CheckedChanged += new System.EventHandler(this.radioButton_WASAPI_Exclusive_CheckedChanged);
            // 
            // comboBox_ASIOdevices
            // 
            this.comboBox_ASIOdevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ASIOdevices.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox_ASIOdevices, "comboBox_ASIOdevices");
            this.comboBox_ASIOdevices.Name = "comboBox_ASIOdevices";
            // 
            // radioButton_ASIO
            // 
            resources.ApplyResources(this.radioButton_ASIO, "radioButton_ASIO");
            this.radioButton_ASIO.Name = "radioButton_ASIO";
            this.radioButton_ASIO.TabStop = true;
            this.radioButton_ASIO.UseVisualStyleBackColor = true;
            this.radioButton_ASIO.CheckedChanged += new System.EventHandler(this.radioButton_ASIO_CheckedChanged);
            // 
            // radioButton_UseDTXViewer
            // 
            resources.ApplyResources(this.radioButton_UseDTXViewer, "radioButton_UseDTXViewer");
            this.radioButton_UseDTXViewer.Name = "radioButton_UseDTXViewer";
            this.radioButton_UseDTXViewer.TabStop = true;
            this.radioButton_UseDTXViewer.UseVisualStyleBackColor = true;
            this.radioButton_UseDTXViewer.CheckedChanged += new System.EventHandler(this.radioButton_UseDTXViewer_CheckedChanged);
            // 
            // radioButton_UseDTXManiaGR
            // 
            resources.ApplyResources(this.radioButton_UseDTXManiaGR, "radioButton_UseDTXManiaGR");
            this.radioButton_UseDTXManiaGR.Name = "radioButton_UseDTXManiaGR";
            this.radioButton_UseDTXManiaGR.TabStop = true;
            this.radioButton_UseDTXManiaGR.UseVisualStyleBackColor = true;
            this.radioButton_UseDTXManiaGR.CheckedChanged += new System.EventHandler(this.radioButton_UseDTXManiaGR_CheckedChanged);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // openFileDialog_DTXViewerPath
            // 
            resources.ApplyResources(this.openFileDialog_DTXViewerPath, "openFileDialog_DTXViewerPath");
            this.openFileDialog_DTXViewerPath.SupportMultiDottedExtensions = true;
            // 
            // COptionsDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControlオプション);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "COptionsDialog";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Cオプションダイアログ_KeyDown);
            this.tabPage全般.ResumeLayout(false);
            this.tabPage全般.PerformLayout();
            this.groupBoxDefaultOperationMode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_EditMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_SelectMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown最近使用したファイルの最大表示個数)).EndInit();
            this.tabControlオプション.ResumeLayout(false);
            this.tabPageLanes.ResumeLayout(false);
            this.tabPageLanes.PerformLayout();
            this.tabPageViewer.ResumeLayout(false);
            this.groupBox_SelectViewer.ResumeLayout(false);
            this.groupBox_SelectViewer.PerformLayout();
            this.groupBox_WindowsSizeSettings.ResumeLayout(false);
            this.groupBox_WindowsSizeSettings.PerformLayout();
            this.groupBox_DTXManiaSettings.ResumeLayout(false);
            this.groupBox_DTXManiaSettings.PerformLayout();
            this.groupBox_SoundDeviceSettings.ResumeLayout(false);
            this.groupBox_SoundDeviceSettings.PerformLayout();
            this.ResumeLayout(false);

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
		private System.Windows.Forms.TabPage tabPageViewer;
		internal System.Windows.Forms.RadioButton radioButton_ASIO;
		internal System.Windows.Forms.RadioButton radioButton_WASAPI_Exclusive;
		internal System.Windows.Forms.RadioButton radioButton_DirectSound;
		internal System.Windows.Forms.RadioButton radioButton_UseDTXManiaGR;
		internal System.Windows.Forms.RadioButton radioButton_UseDTXViewer;
		private System.Windows.Forms.GroupBox groupBox_SelectViewer;
		internal System.Windows.Forms.ComboBox comboBox_ASIOdevices;
		private System.Windows.Forms.Label label_Notice;
		internal System.Windows.Forms.GroupBox groupBox_SoundDeviceSettings;
		private System.Windows.Forms.GroupBox groupBox_DTXManiaSettings;
		public System.Windows.Forms.CheckBox checkBox_GRmode;
		public System.Windows.Forms.CheckBox checkBox_TimeStretch;
		public System.Windows.Forms.CheckBox checkBox_VSyncWait;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBoxDefaultOperationMode;
		private System.Windows.Forms.PictureBox pictureBox_SelectMode;
		private System.Windows.Forms.PictureBox pictureBox_EditMode;
		internal System.Windows.Forms.RadioButton radioButton_EditMode;
		internal System.Windows.Forms.RadioButton radioButton_SelectMode;
		internal System.Windows.Forms.RadioButton radioButton_WASAPI_Shared;
        internal System.Windows.Forms.TextBox textBox_DTXViewerPath;
        internal System.Windows.Forms.OpenFileDialog openFileDialog_DTXViewerPath;
        private System.Windows.Forms.Button button_DTXViewerPath;
        internal System.Windows.Forms.RadioButton radioButton_WinSize720;
        internal System.Windows.Forms.RadioButton radioButton_WinSize1080;
        internal System.Windows.Forms.RadioButton radioButton_WinSize540;
        internal System.Windows.Forms.RadioButton radioButton_WinSize360;
        internal System.Windows.Forms.GroupBox groupBox_WindowsSizeSettings;
    }
}