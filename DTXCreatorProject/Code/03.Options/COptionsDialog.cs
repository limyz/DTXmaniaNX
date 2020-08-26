using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace DTXCreator.Options
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

		public void tレーンリストの内訳を生成する( List<DTXCreator.Score.CLane> listCLane )
		{
			DTXCreator.Score.CLane.ELaneType eLastLaneType = DTXCreator.Score.CLane.ELaneType.END;

			this.checkedListBoxLaneSelectList.BeginUpdate();
			foreach ( DTXCreator.Score.CLane c in listCLane)
			{
				if ( eLastLaneType != c.eLaneType )
				{
					eLastLaneType = c.eLaneType;
					this.checkedListBoxLaneSelectList.Items.Add( eLastLaneType.ToString(), c.bIsVisible );
				}
			}
			this.checkedListBoxLaneSelectList.EndUpdate();
			bレーンリストの内訳が生成済みである = true;
		}

		private void Cオプションダイアログ_KeyDown( object sender, KeyEventArgs e )
		{
			if ( e.KeyCode == Keys.Escape )
			{
				this.button1.PerformClick();
			}
		}

		private void tabControlオプション_KeyDown( object sender, KeyEventArgs e )
		{
			if ( e.KeyCode == Keys.Escape )
			{
				this.button1.PerformClick();
			}
		}
	}
}
