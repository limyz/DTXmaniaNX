using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DTXCreator.Options  // オプション関連
{
	internal class COptionsManager  // Cオプション管理
	{
		public COptionsManager(CMainForm pメインフォーム)
		{
			this.formメインフォーム = pメインフォーム;
		}
		public void tオプションダイアログを開いて編集し結果をアプリ設定に格納する()
		{
			COptionsDialog cオプションダイアログ = new COptionsDialog();
			#region [ Generalタブ ]
			cオプションダイアログ.checkBoxオートフォーカス.CheckState = this.formメインフォーム.appアプリ設定.AutoFocus ? CheckState.Checked : CheckState.Unchecked;
			cオプションダイアログ.checkBox最近使用したファイル.CheckState = this.formメインフォーム.appアプリ設定.ShowRecentFiles ? CheckState.Checked : CheckState.Unchecked;
			cオプションダイアログ.numericUpDown最近使用したファイルの最大表示個数.Value = this.formメインフォーム.appアプリ設定.RecentFilesNum;
			cオプションダイアログ.checkBoxPreviewBGM.CheckState = this.formメインフォーム.appアプリ設定.NoPreviewBGM ? CheckState.Checked : CheckState.Unchecked;
			cオプションダイアログ.checkBoxPlaySoundOnChip.CheckState = this.formメインフォーム.appアプリ設定.PlaySoundOnWAVChipAllocated ? CheckState.Checked : CheckState.Unchecked;
			cオプションダイアログ.radioButton_SelectMode.Checked = this.formメインフォーム.appアプリ設定.InitialOperationMode;
			cオプションダイアログ.radioButton_EditMode.Checked = !this.formメインフォーム.appアプリ設定.InitialOperationMode;
			#endregion
			#region [ Laneタブ ]
			if (!cオプションダイアログ.bレーンリストの内訳が生成済みである)
			{
				cオプションダイアログ.tレーンリストの内訳を生成する(this.formメインフォーム.mgr譜面管理者.listレーン);
			}
			#endregion
			#region [ Viewerタブ ]
			cオプションダイアログ.radioButton_UseDTXViewer.Checked = this.formメインフォーム.appアプリ設定.ViewerInfo.bViewerIsDTXV;
			cオプションダイアログ.radioButton_UseDTXManiaGR.Checked = !this.formメインフォーム.appアプリ設定.ViewerInfo.bViewerIsDTXV;
			cオプションダイアログ.textBox_DTXViewerPath.Text = this.formメインフォーム.appアプリ設定.ViewerInfo.PathDTXV;

			cオプションダイアログ.groupBox_SoundDeviceSettings.Enabled = !this.formメインフォーム.appアプリ設定.ViewerInfo.bViewerIsDTXV;
			cオプションダイアログ.radioButton_DirectSound.Checked = this.formメインフォーム.appアプリ設定.ViewerInfo.SoundType == FDK.ESoundDeviceType.DirectSound;
			cオプションダイアログ.radioButton_WASAPI_Exclusive.Checked = this.formメインフォーム.appアプリ設定.ViewerInfo.SoundType == FDK.ESoundDeviceType.ExclusiveWASAPI;
			cオプションダイアログ.radioButton_WASAPI_Shared.Checked = this.formメインフォーム.appアプリ設定.ViewerInfo.SoundType == FDK.ESoundDeviceType.SharedWASAPI;
			cオプションダイアログ.radioButton_ASIO.Checked = this.formメインフォーム.appアプリ設定.ViewerInfo.SoundType == FDK.ESoundDeviceType.ASIO;

			int[] supportedWindowHeightSize = { 360, 540, 720, 1080 };
			cオプションダイアログ.groupBox_WindowsSizeSettings.Enabled = !this.formメインフォーム.appアプリ設定.ViewerInfo.bViewerIsDTXV;
			cオプションダイアログ.radioButton_WinSize360.Checked = this.formメインフォーム.appアプリ設定.ViewerInfo.ViewerHeightResolution == supportedWindowHeightSize[0];
            cオプションダイアログ.radioButton_WinSize540.Checked = this.formメインフォーム.appアプリ設定.ViewerInfo.ViewerHeightResolution == supportedWindowHeightSize[1];
            cオプションダイアログ.radioButton_WinSize720.Checked = this.formメインフォーム.appアプリ設定.ViewerInfo.ViewerHeightResolution == supportedWindowHeightSize[2];
            cオプションダイアログ.radioButton_WinSize1080.Checked = this.formメインフォーム.appアプリ設定.ViewerInfo.ViewerHeightResolution == supportedWindowHeightSize[3];

            int nASIOdevs = cオプションダイアログ.tASIOデバイスリストの内訳を生成する();
			if (nASIOdevs <= this.formメインフォーム.appアプリ設定.ViewerInfo.ASIODeviceNo) // ASIOの構成が変わった(機器が減った)場合は、ASIOを使わない
			{
				this.formメインフォーム.appアプリ設定.ViewerInfo.ASIODeviceNo = 0;
				cオプションダイアログ.radioButton_ASIO.Checked = false;
				cオプションダイアログ.radioButton_DirectSound.Checked = true;
			}
			cオプションダイアログ.comboBox_ASIOdevices.SelectedIndex = this.formメインフォーム.appアプリ設定.ViewerInfo.ASIODeviceNo;
			if (nASIOdevs == 1 && cオプションダイアログ.comboBox_ASIOdevices.Items[0].ToString() == "None")
			{
				cオプションダイアログ.radioButton_ASIO.Enabled = false;
			}

			cオプションダイアログ.checkBox_GRmode.Checked = this.formメインフォーム.appアプリ設定.ViewerInfo.GRmode;
			cオプションダイアログ.checkBox_TimeStretch.Checked = this.formメインフォーム.appアプリ設定.ViewerInfo.TimeStretch;
			cオプションダイアログ.checkBox_VSyncWait.Checked = this.formメインフォーム.appアプリ設定.ViewerInfo.VSyncWait;
			#endregion



			if (cオプションダイアログ.ShowDialog() == DialogResult.OK)
			{
				this.formメインフォーム.appアプリ設定.AutoFocus = cオプションダイアログ.checkBoxオートフォーカス.Checked;
				this.formメインフォーム.appアプリ設定.ShowRecentFiles = cオプションダイアログ.checkBox最近使用したファイル.Checked;
				this.formメインフォーム.appアプリ設定.RecentFilesNum = (int)cオプションダイアログ.numericUpDown最近使用したファイルの最大表示個数.Value;
				this.formメインフォーム.appアプリ設定.NoPreviewBGM = cオプションダイアログ.checkBoxPreviewBGM.Checked;
				this.formメインフォーム.appアプリ設定.PlaySoundOnWAVChipAllocated = cオプションダイアログ.checkBoxPlaySoundOnChip.Checked;

				this.formメインフォーム.appアプリ設定.InitialOperationMode = cオプションダイアログ.radioButton_SelectMode.Checked;

				for (int i = 0; i < this.formメインフォーム.mgr譜面管理者.listレーン.Count; i++)
				{
					DTXCreator.Score.CLane.ELaneType e = this.formメインフォーム.mgr譜面管理者.listレーン[i].eLaneType;
					int index = cオプションダイアログ.checkedListBoxLaneSelectList.FindStringExact(e.ToString());
					bool ch = cオプションダイアログ.checkedListBoxLaneSelectList.GetItemChecked(index);
					this.formメインフォーム.mgr譜面管理者.listレーン[i].bIsVisible = ch;
				}

				#region [ Viewer設定 ]
				this.formメインフォーム.appアプリ設定.ViewerInfo.bViewerIsDTXV = cオプションダイアログ.radioButton_UseDTXViewer.Checked;
				this.formメインフォーム.appアプリ設定.ViewerInfo.PathDTXV = cオプションダイアログ.textBox_DTXViewerPath.Text;

				//AppSetting.ViewerSoundType vst = ( FDK.COS.bIsVistaOrLater ) ? AppSetting.ViewerSoundType.WASAPI : AppSetting.ViewerSoundType.DirectSound;
				FDK.ESoundDeviceType vst = (FDK.COS.bIsVistaOrLater()) ? FDK.ESoundDeviceType.ExclusiveWASAPI : FDK.ESoundDeviceType.DirectSound;
				if (cオプションダイアログ.radioButton_DirectSound.Checked)
				{
					//vst = AppSetting.ViewerSoundType.DirectSound;
					vst = FDK.ESoundDeviceType.DirectSound;
				}
				else if (cオプションダイアログ.radioButton_WASAPI_Exclusive.Checked)
				{
					//vst = AppSetting.ViewerSoundType.WASAPI;
					vst = FDK.ESoundDeviceType.ExclusiveWASAPI;
				}
				else if (cオプションダイアログ.radioButton_WASAPI_Shared.Checked)
				{
					//vst = AppSetting.ViewerSoundType.WASAPI;
					vst = FDK.ESoundDeviceType.SharedWASAPI;
				}
				else if (cオプションダイアログ.radioButton_ASIO.Checked)
				{
					//vst = AppSetting.ViewerSoundType.ASIO;
					vst = FDK.ESoundDeviceType.ASIO;
				}
				this.formメインフォーム.appアプリ設定.ViewerInfo.SoundType = vst;

				this.formメインフォーム.appアプリ設定.ViewerInfo.ASIODeviceNo = cオプションダイアログ.comboBox_ASIOdevices.SelectedIndex;

				this.formメインフォーム.appアプリ設定.ViewerInfo.GRmode = cオプションダイアログ.checkBox_GRmode.Checked;
				this.formメインフォーム.appアプリ設定.ViewerInfo.TimeStretch = cオプションダイアログ.checkBox_TimeStretch.Checked;
				this.formメインフォーム.appアプリ設定.ViewerInfo.VSyncWait = cオプションダイアログ.checkBox_VSyncWait.Checked;

				//Update Viewer Height Resolution from checkbox options
				if (cオプションダイアログ.radioButton_WinSize360.Checked)
				{
					this.formメインフォーム.appアプリ設定.ViewerInfo.ViewerHeightResolution = supportedWindowHeightSize[0];
				}
				else if (cオプションダイアログ.radioButton_WinSize540.Checked) 
				{
					this.formメインフォーム.appアプリ設定.ViewerInfo.ViewerHeightResolution = supportedWindowHeightSize[1];
				}
                else if (cオプションダイアログ.radioButton_WinSize720.Checked)
                {
                    this.formメインフォーム.appアプリ設定.ViewerInfo.ViewerHeightResolution = supportedWindowHeightSize[2];
                }
                else if (cオプションダイアログ.radioButton_WinSize1080.Checked)
                {
                    this.formメインフォーム.appアプリ設定.ViewerInfo.ViewerHeightResolution = supportedWindowHeightSize[3];
                }
                

                this.formメインフォーム.tDTXV演奏関連のボタンとメニューのEnabledの設定();

				#endregion

				this.formメインフォーム.t最近使ったファイルをFileメニューへ追加する();
			}
		}

		#region [ private ]
		//-----------------
		private CMainForm formメインフォーム;
		//-----------------
		#endregion
	}
}