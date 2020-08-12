using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SlimDX;
using FDK;

namespace DTXMania
{
    internal class CAct演奏Drumsフィルインエフェクト : CActivity
    {

        public CAct演奏Drumsフィルインエフェクト()
		{
			base.bNotActivated = true;
		}

        public override void OnManagedCreateResources()
        {
            if (!base.bNotActivated)
            {
            }
        }
        public override void OnManagedReleaseResources()
        {
            if (!base.bNotActivated)
            {
                base.OnManagedReleaseResources();
            }
        }
        public override void OnDeactivate()
        {
        }
        public override int On進行描画()
        {
            return 0;
        }


        // その他

        #region [ private ]
        //-----------------

        //-----------------
        #endregion
    }
}
