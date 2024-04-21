using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace DTXCreator.Options  // オプション関連
{
	public partial class COptionsDialog : Form  // Cオプションダイアログ
	{
		public bool bレーンリストの内訳が生成済みである
		{
			get; private set;
		}

		public COptionsDialog()
		{
			bレーンリストの内訳が生成済みである = false;
			InitializeComponent();
		}

		public void tレーンリストの内訳を生成する(List<DTXCreator.Score.CLane> listCLane)
		{
			DTXCreator.Score.CLane.ELaneType eLastLaneType = DTXCreator.Score.CLane.ELaneType.END;

			this.checkedListBoxLaneSelectList.BeginUpdate();
			foreach (DTXCreator.Score.CLane c in listCLane)
			{
				if (eLastLaneType != c.eLaneType && !this.checkedListBoxLaneSelectList.Items.Contains(c.eLaneType.ToString()))  // #36899 2017.4.27 yyagi 項目ダブりも抑制
				{
					eLastLaneType = c.eLaneType;
					this.checkedListBoxLaneSelectList.Items.Add(eLastLaneType.ToString(), c.bIsVisible);
				}
			}
			this.checkedListBoxLaneSelectList.EndUpdate();
			bレーンリストの内訳が生成済みである = true;
		}

		public int tASIOデバイスリストの内訳を生成する()
		{
			this.comboBox_ASIOdevices.Items.Clear();
			string[] asiodevs = FDK.CEnumerateAllAsioDevices.GetAllASIODevices();
			this.comboBox_ASIOdevices.Items.AddRange(asiodevs);

			return asiodevs.Length;
		}

		private void Cオプションダイアログ_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				this.buttonOK.PerformClick();
			}
			else if (e.KeyCode == Keys.Escape)
			{
				this.button1.PerformClick();
			}
		}

		private void tabControlオプション_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.button1.PerformClick();
			}
		}

		private void radioButton_UseDTXViewer_CheckedChanged(object sender, EventArgs e)
		{
			this.radioButton_DirectSound.Enabled = false;
			this.radioButton_WASAPI_Exclusive.Enabled = false;
			this.radioButton_WASAPI_Shared.Enabled = false;
			this.radioButton_ASIO.Enabled = false;
			this.comboBox_ASIOdevices.Enabled = false;
			this.groupBox_SoundDeviceSettings.Enabled = false;
            this.groupBox_WindowsSizeSettings.Enabled = false;
            this.textBox_DTXViewerPath.Enabled = true;
			this.button_DTXViewerPath.Enabled = true;
		}

		private void radioButton_UseDTXManiaGR_CheckedChanged(object sender, EventArgs e)
		{
			this.radioButton_DirectSound.Enabled = true;
			this.radioButton_WASAPI_Exclusive.Enabled = true;
			this.radioButton_WASAPI_Shared.Enabled = true;
			this.radioButton_ASIO.Enabled = true;
			this.comboBox_ASIOdevices.Enabled = true;
			this.groupBox_SoundDeviceSettings.Enabled = true;
            this.groupBox_WindowsSizeSettings.Enabled = true;
            this.textBox_DTXViewerPath.Enabled = false;
			this.button_DTXViewerPath.Enabled = false;
		}

		private void radioButton_DirectSound_CheckedChanged(object sender, EventArgs e)
		{
			this.comboBox_ASIOdevices.Enabled = false;
		}

		private void radioButton_WASAPI_Exclusive_CheckedChanged(object sender, EventArgs e)
		{
			this.comboBox_ASIOdevices.Enabled = false;
		}

		private void radioButton_WASAPI_Shared_CheckedChanged(object sender, EventArgs e)
		{
			this.comboBox_ASIOdevices.Enabled = false;
		}

		private void radioButton_ASIO_CheckedChanged(object sender, EventArgs e)
		{
			this.comboBox_ASIOdevices.Enabled = true;
		}

		private void radioButtonSelectMode_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void radioButtonEditMove_CheckedChanged(object sender, EventArgs e)
		{

		}

        private void button_DTXViewerPath_Click(object sender, EventArgs e)
        {
			if (openFileDialog_DTXViewerPath.ShowDialog() == DialogResult.OK)
			{
				textBox_DTXViewerPath.Text = openFileDialog_DTXViewerPath.FileName;
			}
        }

	}
}
