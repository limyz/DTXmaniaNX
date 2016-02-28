using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DTXCreator.オプション関連
{
	internal class Cオプション管理
	{
		public Cオプション管理( Cメインフォーム pメインフォーム )
		{
			this.formメインフォーム = pメインフォーム;
		}
		public void tオプションダイアログを開いて編集し結果をアプリ設定に格納する()
		{
			Cオプションダイアログ cオプションダイアログ = new Cオプションダイアログ();
			cオプションダイアログ.checkBoxオートフォーカス.CheckState = this.formメインフォーム.appアプリ設定.AutoFocus ? CheckState.Checked : CheckState.Unchecked;
			cオプションダイアログ.checkBox最近使用したファイル.CheckState = this.formメインフォーム.appアプリ設定.ShowRecentFiles ? CheckState.Checked : CheckState.Unchecked;
			cオプションダイアログ.numericUpDown最近使用したファイルの最大表示個数.Value = this.formメインフォーム.appアプリ設定.RecentFilesNum;
			cオプションダイアログ.checkBoxPreviewBGM.CheckState = this.formメインフォーム.appアプリ設定.NoPreviewBGM ? CheckState.Checked : CheckState.Unchecked;
			cオプションダイアログ.checkBoxPlaySoundOnChip.CheckState = this.formメインフォーム.appアプリ設定.PlaySoundOnWAVChipAllocated ? CheckState.Checked : CheckState.Unchecked;

			if ( !cオプションダイアログ.bレーンリストの内訳が生成済みである )
			{
				cオプションダイアログ.tレーンリストの内訳を生成する( this.formメインフォーム.mgr譜面管理者.listレーン );
			}
			
			if( cオプションダイアログ.ShowDialog() == DialogResult.OK )
			{
				this.formメインフォーム.appアプリ設定.AutoFocus = cオプションダイアログ.checkBoxオートフォーカス.Checked;
				this.formメインフォーム.appアプリ設定.ShowRecentFiles = cオプションダイアログ.checkBox最近使用したファイル.Checked;
				this.formメインフォーム.appアプリ設定.RecentFilesNum = (int) cオプションダイアログ.numericUpDown最近使用したファイルの最大表示個数.Value;
				this.formメインフォーム.appアプリ設定.NoPreviewBGM = cオプションダイアログ.checkBoxPreviewBGM.Checked;
				this.formメインフォーム.appアプリ設定.PlaySoundOnWAVChipAllocated = cオプションダイアログ.checkBoxPlaySoundOnChip.Checked;

				for ( int i = 0; i < this.formメインフォーム.mgr譜面管理者.listレーン.Count; i++ )
				{
					DTXCreator.譜面.Cレーン.ELaneType e = this.formメインフォーム.mgr譜面管理者.listレーン[ i ].eLaneType;
					int index = cオプションダイアログ.checkedListBoxLaneSelectList.FindStringExact( e.ToString() );
					bool ch = cオプションダイアログ.checkedListBoxLaneSelectList.GetItemChecked( index );
					this.formメインフォーム.mgr譜面管理者.listレーン[ i ].bIsVisible = ch;
				}

				this.formメインフォーム.t最近使ったファイルをFileメニューへ追加する();
			}
		}

		#region [ private ]
		//-----------------
		private Cメインフォーム formメインフォーム;
		//-----------------
		#endregion
	}
}
