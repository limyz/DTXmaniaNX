using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using DTXCreator.Properties;
using FDK;

namespace DTXCreator.チップパレット関連
{
	public partial class Cチップパレット : Form
	{
		public bool b表示ON
		{
			get
			{
				return this._b表示ON;
			}
		}

		public Cチップパレット( Cメインフォーム formメインフォーム )
		{
			this.InitializeComponent();
			this.formメインフォーム = formメインフォーム;
			this.toolStripMenuItem詳細.CheckState = CheckState.Checked;
		}
		public void tパレットセルの番号を置換する( int nImageIndex, int n置換番号1, int n置換番号2 )
		{
			for( int i = 0; i < this.listViewチップリスト.Items.Count; i++ )
			{
				ListViewItem item = this.listViewチップリスト.Items[ i ];
				if( item.ImageIndex == nImageIndex )
				{
					int num2 = C変換.n36進数2桁の文字列を数値に変換して返す( item.SubItems[ 1 ].Text );
					if( num2 == n置換番号1 )
					{
						item.SubItems[ 1 ].Text = C変換.str数値を36進数2桁に変換して返す( n置換番号2 );
					}
					else if( num2 == n置換番号2 )
					{
						item.SubItems[ 1 ].Text = C変換.str数値を36進数2桁に変換して返す( n置換番号1 );
					}
				}
			}
		}
		public void t一時的な隠蔽を解除する()
		{
			if( this._b表示ON )
			{
				base.Show();
			}
		}
		public void t一時的に隠蔽する()
		{
			if( this._b表示ON )
			{
				base.Hide();
			}
		}
		public void t隠す()
		{
			this._b表示ON = false;
			base.Hide();
			this.formメインフォーム.toolStripButtonチップパレット.CheckState = CheckState.Unchecked;
			this.formメインフォーム.toolStripMenuItemチップパレット.CheckState = CheckState.Unchecked;
		}
		public void t表示する()
		{
			this._b表示ON = true;
			base.Show();
			this.formメインフォーム.toolStripButtonチップパレット.CheckState = CheckState.Checked;
			this.formメインフォーム.toolStripMenuItemチップパレット.CheckState = CheckState.Checked;
		}

		private bool _b表示ON;
		private Cメインフォーム formメインフォーム;

		private void tチップの行交換( int n移動元Index0to1294, int n移動先Index0to1294 )
		{
			ListViewItem item = this.listViewチップリスト.Items[ n移動元Index0to1294 ];
			ListViewItem item2 = this.listViewチップリスト.Items[ n移動先Index0to1294 ];
			for( int i = 0; i < 3; i++ )
			{
				string text = item.SubItems[ i ].Text;
				item.SubItems[ i ].Text = item2.SubItems[ i ].Text;
				item2.SubItems[ i ].Text = text;
			}
			Color backColor = item.BackColor;
			item.BackColor = item2.BackColor;
			item2.BackColor = backColor;
			backColor = item.ForeColor;
			item.ForeColor = item2.ForeColor;
			item2.ForeColor = backColor;
			int imageIndex = item.ImageIndex;
			item.ImageIndex = item2.ImageIndex;
			item2.ImageIndex = imageIndex;
			item2.Selected = true;
			item2.Focused = true;
			this.formメインフォーム.b未保存 = true;
		}
		private void t表示形式メニューのアイコンのチェック( bool b大きなアイコン, bool b小さなアイコン, bool b一覧, bool b詳細 )
		{
			this.toolStripMenuItem大きなアイコン.CheckState = b大きなアイコン ? CheckState.Checked : CheckState.Unchecked;
			this.toolStripMenuItem小さなアイコン.CheckState = b小さなアイコン ? CheckState.Checked : CheckState.Unchecked;
			this.toolStripMenuItem一覧.CheckState = b一覧 ? CheckState.Checked : CheckState.Unchecked;
			this.toolStripMenuItem詳細.CheckState = b詳細 ? CheckState.Checked : CheckState.Unchecked;
		}
		
		private void Cチップパレット_DragDrop( object sender, DragEventArgs e )
		{
			if( e.Data.GetDataPresent( typeof( Cチップパレット向けDragDropデータ ) ) )
			{
				Cチップパレット向けDragDropデータ data = (Cチップパレット向けDragDropデータ) e.Data.GetData( typeof( Cチップパレット向けDragDropデータ ) );
				ListViewItem item = new ListViewItem( new string[] { data.strラベル名, C変換.str数値を36進数2桁に変換して返す( data.n番号1to1295 ), data.strファイル名 } );
				item.ImageIndex = data.n種類;
				item.ForeColor = data.col文字色;
				item.BackColor = data.col背景色;
				this.listViewチップリスト.Items.Add( item );
			}
		}
		private void Cチップパレット_FormClosing( object sender, FormClosingEventArgs e )
		{
			if( e.CloseReason == CloseReason.UserClosing )
			{
				this.t隠す();
				e.Cancel = true;
			}
		}
		private void Cチップパレット_DragEnter( object sender, DragEventArgs e )
		{
			if( e.Data.GetDataPresent( typeof( Cチップパレット向けDragDropデータ ) ) )
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}
		private void toolStripSplitButton表示形式_ButtonClick( object sender, EventArgs e )
		{
			this.toolStripSplitButton表示形式.ShowDropDown();
		}
		private void toolStripMenuItem大きなアイコン_Click( object sender, EventArgs e )
		{
			this.listViewチップリスト.View = View.LargeIcon;
			this.t表示形式メニューのアイコンのチェック( true, false, false, false );
		}
		private void toolStripMenuItem小さなアイコン_Click( object sender, EventArgs e )
		{
			this.listViewチップリスト.View = View.SmallIcon;
			this.t表示形式メニューのアイコンのチェック( false, true, false, false );
		}
		private void toolStripMenuItem一覧_Click( object sender, EventArgs e )
		{
			this.listViewチップリスト.View = View.List;
			this.t表示形式メニューのアイコンのチェック( false, false, true, false );
		}
		private void toolStripMenuItem詳細_Click( object sender, EventArgs e )
		{
			this.listViewチップリスト.View = View.Details;
			this.t表示形式メニューのアイコンのチェック( false, false, false, true );
		}
		private void toolStripButton上移動_Click( object sender, EventArgs e )
		{
			if( this.listViewチップリスト.SelectedIndices.Count > 0 )
			{
				int num = this.listViewチップリスト.SelectedIndices[ 0 ];
				if( num != 0 )
				{
					this.tチップの行交換( num, num - 1 );
				}
			}
		}
		private void toolStripButton下移動_Click( object sender, EventArgs e )
		{
			if( this.listViewチップリスト.SelectedIndices.Count > 0 )
			{
				int num = this.listViewチップリスト.SelectedIndices[ 0 ];
				if( num < ( this.listViewチップリスト.Items.Count - 1 ) )
				{
					this.tチップの行交換( num, num + 1 );
				}
			}
		}
		private void listViewチップリスト_SelectedIndexChanged( object sender, EventArgs e )
		{
			if( this.listViewチップリスト.SelectedIndices.Count != 0 )
			{
				ListViewItem item = this.listViewチップリスト.Items[ this.listViewチップリスト.SelectedIndices[ 0 ] ];
				int num = C変換.n36進数2桁の文字列を数値に変換して返す( item.SubItems[ 1 ].Text );
				this.formメインフォーム.tWAV・BMP・AVIリストのカーソルを全部同じ行に合わせる( num - 1 );
				this.formメインフォーム.tタブを選択する( (Cメインフォーム.Eタブ種別) ( item.ImageIndex + 1 ) );
			}
		}
		private void toolStripMenuItemパレットから削除する_Click( object sender, EventArgs e )
		{
			if( this.listViewチップリスト.SelectedIndices.Count != 0 )
			{
				int index = this.listViewチップリスト.SelectedIndices[ 0 ];
				this.listViewチップリスト.Items.RemoveAt( index );
			}
		}
	}
}
