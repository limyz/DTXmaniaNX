using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace FDK
{
	public class CActivity
	{
		// プロパティ

		public bool bActivated { get; private set; }
		public bool bNotActivated
		{
			get
			{
				return !this.bActivated;
			}
			set
			{
				this.bActivated = !value;
			}
		}
		public List<CActivity> listChildActivities;

		/// <summary>
		/// <para>初めて OnUpdateAndDraw() を呼び出す場合に true を示す。（OnActivate() 内で true にセットされる。）</para>
		/// <para>このフラグは、OnActivate() では行えないタイミングのシビアな初期化を OnUpdateAndDraw() で行うために準備されている。利用は必須ではない。</para>
		/// <para>OnUpdateAndDraw() 側では、必要な初期化を追えたら false をセットすること。</para>
		/// </summary>
		protected bool bJustStartedUpdate = true;

	
		// コンストラクタ

		public CActivity()
		{
			this.bNotActivated = true;
			this.listChildActivities = new List<CActivity>();
		}


		// ライフサイクルメソッド

		#region [ 子クラスで必要なもののみ override すること。]
		//-----------------

		public virtual void OnActivate()
		{
			// すでに活性化してるなら何もしない。
			if( this.bActivated )
				return;

			this.bActivated = true;		// このフラグは、以下の処理をする前にセットする。

			// 自身のリソースを作成する。
			this.OnManagedCreateResources();
			this.OnUnmanagedCreateResources();

			// すべての子 Activity を活性化する。
			foreach( CActivity activity in this.listChildActivities )
				activity.OnActivate();

			// その他の初期化
			this.bJustStartedUpdate = true;
		}
		public virtual void OnDeactivate()
		{
			// 活性化してないなら何もしない。
			if( this.bNotActivated )
				return;

			// 自身のリソースを解放する。
			this.OnUnmanagedReleaseResources();
			this.OnManagedReleaseResources();

			// すべての 子Activity を非活性化する。
			foreach( CActivity activity in this.listChildActivities )
				activity.OnDeactivate();

			this.bNotActivated = true;	// このフラグは、以上のメソッドを呼び出した後にセットする。
		}

		/// <summary>
		/// <para>Managed リソースの作成を行う。</para>
		/// <para>Direct3D デバイスが作成された直後に呼び出されるので、自分が活性化している時に限り、
		/// Managed リソースを作成（または再構築）すること。</para>
		/// <para>いつどのタイミングで呼び出されるか（いつDirect3Dが再作成されるか）分からないので、
		/// いつ何時呼び出されても問題無いようにコーディングしておくこと。</para>
		/// </summary>
		public virtual void OnManagedCreateResources()
		{
			// 活性化してないなら何もしない。
			if( this.bNotActivated )
				return;

			// すべての 子Activity の Managed リソースを作成する。
			foreach( CActivity activity in this.listChildActivities )
				activity.OnManagedCreateResources();
		}

		/// <summary>
		/// <para>Unmanaged リソースの作成を行う。</para>
		/// <para>Direct3D デバイスが作成またはリセットされた直後に呼び出されるので、自分が活性化している時に限り、
		/// Unmanaged リソースを作成（または再構築）すること。</para>
		/// <para>いつどのタイミングで呼び出されるか（いつDirect3Dが再作成またはリセットされるか）分からないので、
		/// いつ何時呼び出されても問題無いようにコーディングしておくこと。</para>
		/// </summary>
		public virtual void OnUnmanagedCreateResources()
		{
			// 活性化してないなら何もしない。
			if( this.bNotActivated )
				return;

			// すべての 子Activity の Unmanaged リソースを作成する。
			foreach( CActivity activity in this.listChildActivities )
				activity.OnUnmanagedCreateResources();
		}
		
		/// <summary>
		/// <para>Unmanaged リソースの解放を行う。</para>
		/// <para>Direct3D デバイスの解放直前またはリセット直前に呼び出される。</para>
		/// <para>いつどのタイミングで呼び出されるか（いつDirect3Dが解放またはリセットされるか）分からないので、
		/// いつ何時呼び出されても問題無いようにコーディングしておくこと。</para>
		/// </summary>
		public virtual void OnUnmanagedReleaseResources()
		{
			// 活性化してないなら何もしない。
			if( this.bNotActivated )
				return;

			// すべての 子Activity の Unmanaged リソースを解放する。
			foreach( CActivity activity in this.listChildActivities )
				activity.OnUnmanagedReleaseResources();
		}

		/// <summary>
		/// <para>Managed リソースの解放を行う。</para>
		/// <para>Direct3D デバイスの解放直前に呼び出される。
		/// （Unmanaged リソースとは異なり、Direct3D デバイスのリセット時には呼び出されない。）</para>
		/// <para>いつどのタイミングで呼び出されるか（いつDirect3Dが解放されるか）分からないので、
		/// いつ何時呼び出されても問題無いようにコーディングしておくこと。</para>
		/// </summary>
		public virtual void OnManagedReleaseResources()
		{
			// 活性化してないなら何もしない。
			if( this.bNotActivated )
				return;

			// すべての 子Activity の Managed リソースを解放する。
			foreach( CActivity activity in this.listChildActivities )
				activity.OnManagedReleaseResources();
		}

		/// <summary>
		/// <para>Update and draw. (These are not separated, only one method is implemented).</para>
		/// <para>This method is called after BeginScene(), so it doesn't matter which drawing method is used.</para>
		/// </summary>
		/// <returns>Any integer. Be consistent with the caller.</returns>
		public virtual int OnUpdateAndDraw()
		{
			// 活性化してないなら何もしない。
			if( this.bNotActivated )
				return 0;


			/* ここで進行と描画を行う。*/


			// 戻り値とその意味は子クラスで自由に決めていい。
			return 0;
		}
        /// <summary>
        /// <para>進行と描画を行う。（これらは分離されず、この１つのメソッドだけで実装する。）</para>
        /// <para>このメソッドは BeginScene() の後に呼び出されるので、メソッド内でいきなり描画を行ってかまわない。</para>
        /// <para>大体はSSTのOn進行とOn描画を合体させたようなものです。</para>
        /// </summary>
        /// <returns>任意の整数。呼び出し元との整合性を合わせておくこと。</returns>
        public virtual int OnUpdateAndDraw(SharpDX.Direct3D9.Device D3D9Device)
        {
            // 活性化してないなら何もしない。
            if (this.bNotActivated)
                return 0;


            /* ここで進行と描画を行う。*/


            // 戻り値とその意味は子クラスで自由に決めていい。
            return 0;
        }
        /// <summary>
        /// <para>この Activity を活性化（有効化）する。</para>
        /// <para>具体的には内部リソースの初期化などを行う。</para>
        /// </summary>
        public virtual void OnActivate(SharpDX.Direct3D9.Device D3D9Device)
        {
            if (this.bActivated)
                return;

            this.bActivated = true;		// このフラグは、以下の処理をする前にセットする。


            // 自身のリソースを作成する。

            this.OnManagedCreateResource(D3D9Device);
            this.OnUnmanagedCreateResources(D3D9Device);


            // すべての子Activityを活性化する。

            foreach (CActivity activity in this.listChildActivities)
                activity.OnActivate(D3D9Device);


            // その他

            this.bJustStartedUpdate = true;
        }

		/// <summary>
		/// <para>Managed リソースの作成を行う。</para>
		/// <para>Direct3D デバイスが作成された直後に呼び出されるので、自分が活性化している時に限り、Managed リソースを作成（または再構築）すること。</para>
		/// <para>いつどのタイミングで呼び出されるか（いつDirect3Dが再作成されるか）分からないので、いつ何時呼び出されても問題無いようにコーディングしておくこと。</para>
		/// </summary>
		public virtual void OnManagedCreateResource( SharpDX.Direct3D9.Device D3D9Device )
		{
			if( this.bNotActivated )
				return;


			// すべての 子Activity の Managed リソースを作成する。
			
			foreach( CActivity activity in this.listChildActivities )
				activity.OnManagedCreateResource( D3D9Device );
		}

		/// <summary>
		/// <para>Unmanaged リソースの作成を行う。</para>
		/// <para>Direct3D デバイスが作成またはリセットされた直後に呼び出されるので、自分が活性化している時に限り、Unmanaged リソースを作成（または再構築）すること。</para>
		/// <para>いつどのタイミングで呼び出されるか（いつDirect3Dが再作成またはリセットされるか）分からないので、いつ何時呼び出されても問題無いようにコーディングしておくこと。</para>
		/// </summary>
		public virtual void OnUnmanagedCreateResources( SharpDX.Direct3D9.Device D3D9Device )
		{
			if( this.bNotActivated )
				return;


			// すべての 子Activity の Unmanaged リソースを作成する。

			foreach( CActivity activity in this.listChildActivities )
				activity.OnUnmanagedCreateResources( D3D9Device );
		}

        /// <summary>
        /// <para>Update only. Do not draw.</para>
        /// <para>Direct3DDeviceを使ってはならない。</para>
        /// </summary>
        public virtual int OnUpdate()
        {
            if (this.bNotActivated)
                return 0;

            this.bJustStartedUpdate = true;


            /* ここで進行を行う。*/


            // 戻り値とその意味は子クラスで自由に決めていい。
            return 0;
        }

        /// <summary>
        /// <para>Only draw。</para>
        /// <para>This method is called between BeginScene() and EndScene(). メソッド内でいきなり描画を行ってかまわない。</para>
        /// <para>ただし、Direct3D デバイスの変更は行ってはならない。</para>
        /// </summary>
        public virtual void OnDraw(SharpDX.Direct3D9.Device D3D9Device)
        {
            if (this.bNotActivated)
                return;

            /* ここで描画を行う。*/
        }

		//-----------------
		#endregion
	}
}