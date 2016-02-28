using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using FDK;

namespace DTXMania
{
	internal class CActSelectPresound : CActivity
	{
		// メソッド

		public CActSelectPresound()
		{
			base.b活性化してない = true;
		}
		public void tサウンド停止()
		{
			if( this.sound != null )
			{
				this.sound.t再生を停止する();
				CDTXMania.Sound管理.tサウンドを破棄する( this.sound );
				this.sound = null;
			}
		}
		public void t選択曲が変更された()
		{
			Cスコア cスコア = CDTXMania.stage選曲.r現在選択中のスコア;
			if( ( cスコア != null ) && ( ( !( cスコア.ファイル情報.フォルダの絶対パス + cスコア.譜面情報.Presound ).Equals( this.str現在のファイル名 ) || ( this.sound == null ) ) || !this.sound.b再生中 ) )
			{
				this.tサウンド停止();
				this.tBGMフェードイン開始();
				if( ( cスコア.譜面情報.Presound != null ) && ( cスコア.譜面情報.Presound.Length > 0 ) )
				{
					this.ct再生待ちウェイト = new CCounter( 0, CDTXMania.ConfigIni.n曲が選択されてからプレビュー音が鳴るまでのウェイトms, 1, CDTXMania.Timer );
				}
			}
		}


		// CActivity 実装

		public override void On活性化()
		{
			this.sound = null;
			this.str現在のファイル名 = "";
			this.ct再生待ちウェイト = null;
			this.ctBGMフェードアウト用 = null;
			this.ctBGMフェードイン用 = null;
			base.On活性化();
		}
		public override void On非活性化()
		{
			this.tサウンド停止();
			this.ct再生待ちウェイト = null;
			this.ctBGMフェードイン用 = null;
			this.ctBGMフェードアウト用 = null;
			base.On非活性化();
		}
		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
				if( ( this.ctBGMフェードイン用 != null ) && this.ctBGMフェードイン用.b進行中 )
				{
					this.ctBGMフェードイン用.t進行();
					CDTXMania.Skin.bgm選曲画面.n音量・現在のサウンド = this.ctBGMフェードイン用.n現在の値;
					if( this.ctBGMフェードイン用.b終了値に達した )
					{
						this.ctBGMフェードイン用.t停止();
					}
				}
				if( ( this.ctBGMフェードアウト用 != null ) && this.ctBGMフェードアウト用.b進行中 )
				{
					this.ctBGMフェードアウト用.t進行();
					CDTXMania.Skin.bgm選曲画面.n音量・現在のサウンド = 100 - this.ctBGMフェードアウト用.n現在の値;
					if( this.ctBGMフェードアウト用.b終了値に達した )
					{
						this.ctBGMフェードアウト用.t停止();
					}
				}
				this.t進行処理・プレビューサウンド();
			}
			return 0;
		}


		// その他

		#region [ private ]
		//-----------------
		private CCounter ctBGMフェードアウト用;
		private CCounter ctBGMフェードイン用;
		private CCounter ct再生待ちウェイト;
		private CSound sound;
		private string str現在のファイル名;
		
		private void tBGMフェードアウト開始()
		{
			if( this.ctBGMフェードイン用 != null )
			{
				this.ctBGMフェードイン用.t停止();
			}
			this.ctBGMフェードアウト用 = new CCounter( 0, 100, 10, CDTXMania.Timer );
			this.ctBGMフェードアウト用.n現在の値 = 100 - CDTXMania.Skin.bgm選曲画面.n音量・現在のサウンド;
		}
		private void tBGMフェードイン開始()
		{
			if( this.ctBGMフェードアウト用 != null )
			{
				this.ctBGMフェードアウト用.t停止();
			}
			this.ctBGMフェードイン用 = new CCounter( 0, 100, 20, CDTXMania.Timer );
			this.ctBGMフェードイン用.n現在の値 = CDTXMania.Skin.bgm選曲画面.n音量・現在のサウンド;
		}
		private void tプレビューサウンドの作成()
		{
			Cスコア cスコア = CDTXMania.stage選曲.r現在選択中のスコア;
			if( ( cスコア != null ) && !string.IsNullOrEmpty( cスコア.譜面情報.Presound ) )
			{
				string strPreviewFilename = cスコア.ファイル情報.フォルダの絶対パス + cスコア.譜面情報.Presound;
				try
				{
					this.sound = CDTXMania.Sound管理.tサウンドを生成する( strPreviewFilename );
					this.sound.n音量 = 80;	// CDTXMania.ConfigIni.n自動再生音量;			// #25217 changed preview volume from AutoVolume
					this.sound.t再生を開始する( true );
					this.str現在のファイル名 = strPreviewFilename;
					this.tBGMフェードアウト開始();
					Trace.TraceInformation( "プレビューサウンドを生成しました。({0})", strPreviewFilename );
				}
				catch
				{
					Trace.TraceError( "プレビューサウンドの生成に失敗しました。({0})", strPreviewFilename );
					if( this.sound != null )
					{
						this.sound.Dispose();
					}
					this.sound = null;
				}
			}
		}
		private void t進行処理・プレビューサウンド()
		{
			if( ( this.ct再生待ちウェイト != null ) && !this.ct再生待ちウェイト.b停止中 )
			{
				this.ct再生待ちウェイト.t進行();
				if( !this.ct再生待ちウェイト.b終了値に達してない )
				{
					this.ct再生待ちウェイト.t停止();
					if( !CDTXMania.stage選曲.bスクロール中 )
					{
						this.tプレビューサウンドの作成();
					}
				}
			}
		}
		//-----------------
		#endregion
	}
}
