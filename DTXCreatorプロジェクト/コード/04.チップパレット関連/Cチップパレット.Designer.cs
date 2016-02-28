namespace DTXCreator.チップパレット関連
{
	partial class Cチップパレット
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( Cチップパレット ) );
			this.toolStripツールバー = new System.Windows.Forms.ToolStrip();
			this.toolStripSplitButton表示形式 = new System.Windows.Forms.ToolStripSplitButton();
			this.toolStripMenuItem大きなアイコン = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem小さなアイコン = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem一覧 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem詳細 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton上移動 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton下移動 = new System.Windows.Forms.ToolStripButton();
			this.listViewチップリスト = new System.Windows.Forms.ListView();
			this.columnHeaderラベル = new System.Windows.Forms.ColumnHeader();
			this.columnHeader番号 = new System.Windows.Forms.ColumnHeader();
			this.columnHeaderファイル = new System.Windows.Forms.ColumnHeader();
			this.imageList大きなアイコン = new System.Windows.Forms.ImageList( this.components );
			this.imageList小さなアイコン = new System.Windows.Forms.ImageList( this.components );
			this.contextMenuStripリスト用 = new System.Windows.Forms.ContextMenuStrip( this.components );
			this.toolStripMenuItemパレットから削除する = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripツールバー.SuspendLayout();
			this.contextMenuStripリスト用.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripツールバー
			// 
			this.toolStripツールバー.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripツールバー.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton表示形式,
            this.toolStripSeparator1,
            this.toolStripButton上移動,
            this.toolStripButton下移動} );
			resources.ApplyResources( this.toolStripツールバー, "toolStripツールバー" );
			this.toolStripツールバー.Name = "toolStripツールバー";
			// 
			// toolStripSplitButton表示形式
			// 
			this.toolStripSplitButton表示形式.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem大きなアイコン,
            this.toolStripMenuItem小さなアイコン,
            this.toolStripMenuItem一覧,
            this.toolStripMenuItem詳細} );
			this.toolStripSplitButton表示形式.Image = global::DTXCreator.Properties.Resources.表示形式選択;
			resources.ApplyResources( this.toolStripSplitButton表示形式, "toolStripSplitButton表示形式" );
			this.toolStripSplitButton表示形式.Name = "toolStripSplitButton表示形式";
			this.toolStripSplitButton表示形式.ButtonClick += new System.EventHandler( this.toolStripSplitButton表示形式_ButtonClick );
			// 
			// toolStripMenuItem大きなアイコン
			// 
			this.toolStripMenuItem大きなアイコン.Name = "toolStripMenuItem大きなアイコン";
			resources.ApplyResources( this.toolStripMenuItem大きなアイコン, "toolStripMenuItem大きなアイコン" );
			this.toolStripMenuItem大きなアイコン.Click += new System.EventHandler( this.toolStripMenuItem大きなアイコン_Click );
			// 
			// toolStripMenuItem小さなアイコン
			// 
			this.toolStripMenuItem小さなアイコン.Name = "toolStripMenuItem小さなアイコン";
			resources.ApplyResources( this.toolStripMenuItem小さなアイコン, "toolStripMenuItem小さなアイコン" );
			this.toolStripMenuItem小さなアイコン.Click += new System.EventHandler( this.toolStripMenuItem小さなアイコン_Click );
			// 
			// toolStripMenuItem一覧
			// 
			this.toolStripMenuItem一覧.Name = "toolStripMenuItem一覧";
			resources.ApplyResources( this.toolStripMenuItem一覧, "toolStripMenuItem一覧" );
			this.toolStripMenuItem一覧.Click += new System.EventHandler( this.toolStripMenuItem一覧_Click );
			// 
			// toolStripMenuItem詳細
			// 
			this.toolStripMenuItem詳細.Name = "toolStripMenuItem詳細";
			resources.ApplyResources( this.toolStripMenuItem詳細, "toolStripMenuItem詳細" );
			this.toolStripMenuItem詳細.Click += new System.EventHandler( this.toolStripMenuItem詳細_Click );
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources( this.toolStripSeparator1, "toolStripSeparator1" );
			// 
			// toolStripButton上移動
			// 
			this.toolStripButton上移動.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton上移動.Image = global::DTXCreator.Properties.Resources.上移動;
			resources.ApplyResources( this.toolStripButton上移動, "toolStripButton上移動" );
			this.toolStripButton上移動.Name = "toolStripButton上移動";
			this.toolStripButton上移動.Click += new System.EventHandler( this.toolStripButton上移動_Click );
			// 
			// toolStripButton下移動
			// 
			this.toolStripButton下移動.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton下移動.Image = global::DTXCreator.Properties.Resources.下移動;
			resources.ApplyResources( this.toolStripButton下移動, "toolStripButton下移動" );
			this.toolStripButton下移動.Name = "toolStripButton下移動";
			this.toolStripButton下移動.Click += new System.EventHandler( this.toolStripButton下移動_Click );
			// 
			// listViewチップリスト
			// 
			this.listViewチップリスト.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderラベル,
            this.columnHeader番号,
            this.columnHeaderファイル} );
			this.listViewチップリスト.ContextMenuStrip = this.contextMenuStripリスト用;
			resources.ApplyResources( this.listViewチップリスト, "listViewチップリスト" );
			this.listViewチップリスト.FullRowSelect = true;
			this.listViewチップリスト.GridLines = true;
			this.listViewチップリスト.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listViewチップリスト.HideSelection = false;
			this.listViewチップリスト.LargeImageList = this.imageList大きなアイコン;
			this.listViewチップリスト.MultiSelect = false;
			this.listViewチップリスト.Name = "listViewチップリスト";
			this.listViewチップリスト.SmallImageList = this.imageList小さなアイコン;
			this.listViewチップリスト.UseCompatibleStateImageBehavior = false;
			this.listViewチップリスト.View = System.Windows.Forms.View.Details;
			this.listViewチップリスト.SelectedIndexChanged += new System.EventHandler( this.listViewチップリスト_SelectedIndexChanged );
			// 
			// columnHeaderラベル
			// 
			resources.ApplyResources( this.columnHeaderラベル, "columnHeaderラベル" );
			// 
			// columnHeader番号
			// 
			resources.ApplyResources( this.columnHeader番号, "columnHeader番号" );
			// 
			// columnHeaderファイル
			// 
			resources.ApplyResources( this.columnHeaderファイル, "columnHeaderファイル" );
			// 
			// imageList大きなアイコン
			// 
			this.imageList大きなアイコン.ImageStream = ( (System.Windows.Forms.ImageListStreamer) ( resources.GetObject( "imageList大きなアイコン.ImageStream" ) ) );
			this.imageList大きなアイコン.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList大きなアイコン.Images.SetKeyName( 0, "" );
			this.imageList大きなアイコン.Images.SetKeyName( 1, "" );
			this.imageList大きなアイコン.Images.SetKeyName( 2, "" );
			// 
			// imageList小さなアイコン
			// 
			this.imageList小さなアイコン.ImageStream = ( (System.Windows.Forms.ImageListStreamer) ( resources.GetObject( "imageList小さなアイコン.ImageStream" ) ) );
			this.imageList小さなアイコン.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList小さなアイコン.Images.SetKeyName( 0, "MusicDoc.PNG" );
			this.imageList小さなアイコン.Images.SetKeyName( 1, "PicDoc.PNG" );
			this.imageList小さなアイコン.Images.SetKeyName( 2, "VideoDoc.PNG" );
			// 
			// contextMenuStripリスト用
			// 
			this.contextMenuStripリスト用.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemパレットから削除する} );
			this.contextMenuStripリスト用.Name = "contextMenuStripリスト用";
			resources.ApplyResources( this.contextMenuStripリスト用, "contextMenuStripリスト用" );
			// 
			// toolStripMenuItemパレットから削除する
			// 
			this.toolStripMenuItemパレットから削除する.Name = "toolStripMenuItemパレットから削除する";
			resources.ApplyResources( this.toolStripMenuItemパレットから削除する, "toolStripMenuItemパレットから削除する" );
			this.toolStripMenuItemパレットから削除する.Click += new System.EventHandler( this.toolStripMenuItemパレットから削除する_Click );
			// 
			// Cチップパレット
			// 
			this.AllowDrop = true;
			resources.ApplyResources( this, "$this" );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.listViewチップリスト );
			this.Controls.Add( this.toolStripツールバー );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "Cチップパレット";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.TopMost = true;
			this.DragDrop += new System.Windows.Forms.DragEventHandler( this.Cチップパレット_DragDrop );
			this.DragEnter += new System.Windows.Forms.DragEventHandler( this.Cチップパレット_DragEnter );
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.Cチップパレット_FormClosing );
			this.toolStripツールバー.ResumeLayout( false );
			this.toolStripツールバー.PerformLayout();
			this.contextMenuStripリスト用.ResumeLayout( false );
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStripツールバー;
		internal System.Windows.Forms.ListView listViewチップリスト;
		private System.Windows.Forms.ImageList imageList小さなアイコン;
		private System.Windows.Forms.ImageList imageList大きなアイコン;
		private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton表示形式;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem大きなアイコン;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem小さなアイコン;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem一覧;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem詳細;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolStripButton上移動;
		private System.Windows.Forms.ToolStripButton toolStripButton下移動;
		private System.Windows.Forms.ColumnHeader columnHeaderラベル;
		private System.Windows.Forms.ColumnHeader columnHeader番号;
		private System.Windows.Forms.ColumnHeader columnHeaderファイル;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripリスト用;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemパレットから削除する;
	}
}