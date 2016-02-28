using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace DTXCreator.オプション関連
{
	public partial class Cオプションダイアログ : Form
	{
		public bool bレーンリストの内訳が生成済みである
		{
			get; private set;
		}

		public Cオプションダイアログ()
		{
			bレーンリストの内訳が生成済みである = false;
			InitializeComponent();
		}

		public void tレーンリストの内訳を生成する( List<DTXCreator.譜面.Cレーン> listCLane )
		{
			DTXCreator.譜面.Cレーン.ELaneType eLastLaneType = DTXCreator.譜面.Cレーン.ELaneType.END;

			this.checkedListBoxLaneSelectList.BeginUpdate();
			foreach ( DTXCreator.譜面.Cレーン c in listCLane)
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
