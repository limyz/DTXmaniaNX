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
			base.b活性化してない = true;
		}

        public override void OnManagedリソースの作成()
        {
            if (!base.b活性化してない)
            {
            }
        }
        public override void OnManagedリソースの解放()
        {
            if (!base.b活性化してない)
            {
                base.OnManagedリソースの解放();
            }
        }
        public override void On非活性化()
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
