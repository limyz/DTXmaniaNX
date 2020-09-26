using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using SlimDX;
using FDK;

using Rectangle = System.Drawing.Rectangle;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace DTXMania
{
	/// <summary>
	/// プライベートフォントでの描画を扱うクラス。
	/// </summary>
	/// <exception cref="FileNotFoundException">フォントファイルが見つからず、更にMS PGothicのフォント指定にも失敗したときに例外発生</exception>
	/// <remarks>
	/// 簡単な使い方
	/// CPrivateFont prvFont = new CPrivateFont( CSkin.Path( @"Graphics\fonts\mplus-1p-bold.ttf" ), 36 );	// プライベートフォント
	/// とか
	/// CPrivateFont prvFont = new CPrivateFont( "MS UI Gothic", 36, FontStyle.Bold );						// システムフォント
	/// CPrivateFont prvFont = new CPrivateFont( CSkin.Path( @"Graphics\fonts\Arial" ), 36 );				// システムフォント
	/// (拡張子の有無でシステムフォントかプライベートフォントかを判断。拡張子なしの場合はパス名指定部分は無視される)
	/// とか
	/// CPrivateFont prvFont = new CPrivateFont( new FontFamily("MS UI Gothic"), 36, FontStyle.Bold );		// システムフォント指定方法その2
	/// とかした上で、
	/// Bitmap bmp = prvFont.DrawPrivateFont( "ABCDE", Color.White, Color.Black );							// フォント色＝白、縁の色＝黒の例。縁の色は省略可能
	/// とか
	/// Bitmap bmp = prvFont.DrawPrivateFont( "ABCDE", Color.White, Color.Black, Color.Yellow, Color.OrangeRed ); // 上下グラデーション(Yellow→OrangeRed)
	/// とか
	/// prvFont.font.DrawString(...)
	/// とかして、
	/// CTexture ctBmp = TextureFactory.tテクスチャの生成( bmp, false );
	/// ctBMP.t2D描画( ～～～ );
	/// で表示してください。
	/// 
	/// フォントファイルが見つからない時には、代わりにMS PGothicを使用しようと試みます。(MS PGothicも使えなかったときは、FileNotFoundException発生)
	/// スタイル指定不正時には、それなりのスタイルを設定します。
	/// 
	/// 注意点
	/// 任意のフォントでのレンダリングは結構負荷が大きいので、なるべくなら描画フレーム毎にフォントを再レンダリングするようなことはせず、
	/// 一旦レンダリングしたものを描画に使い回すようにしてください。
	/// また、長い文字列を与えると、返されるBitmapも横長になります。この横長画像をそのままテクスチャとして使うと、
	/// 古いPCで問題を発生させやすいです。これを回避するには、一旦Bitmapとして取得したのち、256pixや512pixで分割して
	/// テクスチャに定義するようにしてください。(CTextureAf()は、そのあたりを自動処理してくれます)
	/// </remarks>
	public class CPrivateFont : IDisposable
	{
		/// <summary>
		/// プライベートフォントのFontクラス。CPrivateFont()の初期化後に使用可能となる。
		/// プライベートフォントでDrawString()したい場合にご利用ください。
		/// </summary>
		public Font font
		{
			get => _font;
		}

		/// <summary>
		/// フォント登録失敗時に代替使用するフォント名。システムフォントのみ設定可能。
		/// 後日外部指定できるようにします。(＝コンストラクタで指定できるようにします)
		/// </summary>
		private string strAlternativeFont = "MS PGothic";

		#region [ コンストラクタ ]
		public CPrivateFont(FontFamily fontfamily, float pt, FontStyle style)
		{
			Initialize(null, null, fontfamily, pt, style);
		}
		public CPrivateFont(FontFamily fontfamily, float pt)
		{
			Initialize(null, null, fontfamily, pt, FontStyle.Regular);
		}
		public CPrivateFont(string fontpath, FontFamily fontfamily, float pt, FontStyle style)
		{
			Initialize(fontpath, null, fontfamily, pt, style);
		}
		public CPrivateFont(string fontpath, float pt, FontStyle style)
		{
			Initialize(fontpath, null, null, pt, style);
		}
		public CPrivateFont(string fontpath, float pt)
		{
			Initialize(fontpath, null, null, pt, FontStyle.Regular);
		}
		public CPrivateFont()
		{
			//throw new ArgumentException("CPrivateFont: 引数があるコンストラクタを使用してください。");
		}
		#endregion

		protected void Initialize(string fontpath, string baseFontPath, FontFamily fontfamily, float pt, FontStyle style)
		{
			this._pfc = null;
			this._fontfamily = null;
			this._font = null;
			this._pt = pt;
			this._rectStrings = new Rectangle(0, 0, 0, 0);
			this._ptOrigin = new Point(0, 0);
			this.bDispose完了済み = false;
			this._baseFontname = baseFontPath;
			this.bIsSystemFont = false;

			float emSize = 0f;
			using (Bitmap b = new Bitmap(1, 1))
			{
				using (Graphics g = Graphics.FromImage(b))
				{
					emSize = pt * 96.0f / 72.0f * g.DpiX / 96.0f;   // DPIを考慮したpxサイズ。GraphicsUnit.Pixelと併用のこと
				}
			}

			if (fontfamily != null)
			{
				this._fontfamily = fontfamily;
			}
			else
			{
				try
				{
					//拡張子あり == 通常のPrivateFontパス指定
					if (Path.GetExtension(fontpath) != string.Empty)
					{
						this._pfc = new System.Drawing.Text.PrivateFontCollection();    //PrivateFontCollectionオブジェクトを作成する
						this._pfc.AddFontFile(fontpath);                                //PrivateFontCollectionにフォントを追加する
						_fontfamily = _pfc.Families[0];
						bIsSystemFont = false;
					}
					//拡張子なし == Arial, MS Gothicなど、システムフォントの指定
					else
					{
						this._font = PublicFont(Path.GetFileName(fontpath), emSize, style, GraphicsUnit.Pixel);
						bIsSystemFont = true;
					}
				}
				catch (Exception e) when (e is System.IO.FileNotFoundException || e is System.Runtime.InteropServices.ExternalException)
				{
					Trace.TraceWarning(e.Message);
					Trace.TraceWarning("プライベートフォントの追加に失敗しました({0})。代わりに{1}の使用を試みます。", fontpath, strAlternativeFont);
					//throw new FileNotFoundException( "プライベートフォントの追加に失敗しました。({0})", Path.GetFileName( fontpath ) );
					//return;

					_fontfamily = null;
				}

				//foreach ( FontFamily ff in _pfc.Families )
				//{
				//	Debug.WriteLine( "fontname=" + ff.Name );
				//	if ( ff.Name == Path.GetFileNameWithoutExtension( fontpath ) )
				//	{
				//		_fontfamily = ff;
				//		break;
				//	}
				//}
				//if ( _fontfamily == null )
				//{
				//	Trace.TraceError( "プライベートフォントの追加後、検索に失敗しました。({0})", fontpath );
				//	return;
				//}
			}

			// システムフォントの登録に成功した場合
			if (bIsSystemFont && _font != null)
			{
				// 追加処理なし。何もしない
			}
			// PrivateFontの登録に成功した場合は、指定されたフォントスタイルをできるだけ適用する
			else if (_fontfamily != null)
			{
				if (!_fontfamily.IsStyleAvailable(style))
				{
					FontStyle[] FS = { FontStyle.Regular, FontStyle.Bold, FontStyle.Italic, FontStyle.Underline, FontStyle.Strikeout };
					style = FontStyle.Regular | FontStyle.Bold | FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout;  // null非許容型なので、代わりに全盛をNGワードに設定
					foreach (FontStyle ff in FS)
					{
						if (this._fontfamily.IsStyleAvailable(ff))
						{
							style = ff;
							Trace.TraceWarning("フォント{0}へのスタイル指定を、{1}に変更しました。", Path.GetFileName(fontpath), style.ToString());
							break;
						}
					}
					if (style == (FontStyle.Regular | FontStyle.Bold | FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout))
					{
						Trace.TraceWarning("フォント{0}は適切なスタイル{1}を選択できませんでした。", Path.GetFileName(fontpath), style.ToString());
					}
				}
				this._font = new Font(this._fontfamily, emSize, style, GraphicsUnit.Pixel); //PrivateFontCollectionの先頭のフォントのFontオブジェクトを作成する
			}
			// PrivateFontと通常フォント、どちらの登録もできていない場合は、MS PGothic改め代替フォントを代わりに設定しようと試みる
			else
			{
				this._font = PublicFont(strAlternativeFont, emSize, style, GraphicsUnit.Pixel);
				if (this._font != null)
				{
					Trace.TraceInformation("{0}の代わりに{1}を指定しました。", Path.GetFileName(fontpath), strAlternativeFont);
					bIsSystemFont = true;
					return;
				}
				throw new FileNotFoundException(string.Format("プライベートフォントの追加に失敗し、{1}での代替処理にも失敗しました。({0})", Path.GetFileName(fontpath), strAlternativeFont));
			}
		}


		/// <summary>
		/// プライベートフォントではない、システムフォントの設定
		/// </summary>
		/// <param name="fontname">フォント名</param>
		/// <param name="emSize">フォントサイズ</param>
		/// <param name="style">フォントスタイル</param>
		/// <param name="unit">GraphicsUnit</param>
		/// <returns></returns>
		private Font PublicFont(string fontname, float emSize, FontStyle style, GraphicsUnit unit)
		{
			Font f = new Font(fontname, emSize, style, unit);
			FontFamily[] ffs = new System.Drawing.Text.InstalledFontCollection().Families;
			int lcid = System.Globalization.CultureInfo.GetCultureInfo("en-us").LCID;
			foreach (FontFamily ff in ffs)
			{
				// Trace.WriteLine( lcid ) );
				if (ff.GetName(lcid) == fontname)
				{
					this._fontfamily = ff;
					return f;
				}
			}
			return null;
		}

		[Flags]
		public enum DrawMode
		{
			Normal,
			Edge,
			Gradation,
			EdgeGradation
		}

		#region [ DrawPrivateFontのオーバーロード群 ]
		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <returns>描画済テクスチャ</returns>
		public Bitmap DrawPrivateFont(string drawstr, Color fontColor)
		{
			return DrawPrivateFont(drawstr, DrawMode.Normal, fontColor, Color.White, Color.White, Color.White);
		}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <returns>描画済テクスチャ</returns>
		public Bitmap DrawPrivateFont(string drawstr, Color fontColor, Color edgeColor)
		{
			return DrawPrivateFont(drawstr, DrawMode.Edge, fontColor, edgeColor, Color.White, Color.White);
		}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		//public Bitmap DrawPrivateFont( string drawstr, Color fontColor, Color gradationTopColor, Color gradataionBottomColor )
		//{
		//    return DrawPrivateFont( drawstr, DrawMode.Gradation, fontColor, Color.White, gradationTopColor, gradataionBottomColor );
		//}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		public Bitmap DrawPrivateFont(string drawstr, Color fontColor, Color edgeColor, Color gradationTopColor, Color gradataionBottomColor)
		{
			return DrawPrivateFont(drawstr, DrawMode.Edge | DrawMode.Gradation, fontColor, edgeColor, gradationTopColor, gradataionBottomColor);
		}

#if こちらは使わない // (Bitmapではなく、CTextureを返す版)
		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <returns>描画済テクスチャ</returns>
		public CTexture DrawPrivateFont( string drawstr, Color fontColor )
		{
			Bitmap bmp = DrawPrivateFont( drawstr, DrawMode.Normal, fontColor, Color.White, Color.White, Color.White );
			return TextureFactory.tテクスチャの生成( bmp, false );
		}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <returns>描画済テクスチャ</returns>
		public CTexture DrawPrivateFont( string drawstr, Color fontColor, Color edgeColor )
		{
			Bitmap bmp = DrawPrivateFont( drawstr, DrawMode.Edge, fontColor, edgeColor, Color.White, Color.White );
			return TextureFactory.tテクスチャの生成( bmp, false );
		}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		//public CTexture DrawPrivateFont( string drawstr, Color fontColor, Color gradationTopColor, Color gradataionBottomColor )
		//{
		//    Bitmap bmp = DrawPrivateFont( drawstr, DrawMode.Gradation, fontColor, Color.White, gradationTopColor, gradataionBottomColor );
		//	  return TextureFactory.tテクスチャの生成( bmp, false );
		//}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		public CTexture DrawPrivateFont( string drawstr, Color fontColor, Color edgeColor,  Color gradationTopColor, Color gradataionBottomColor )
		{
			Bitmap bmp = DrawPrivateFont( drawstr, DrawMode.Edge | DrawMode.Gradation, fontColor, edgeColor, gradationTopColor, gradataionBottomColor );
			return TextureFactory.tテクスチャの生成( bmp, false );
		}
#endif
		#endregion

		/// <summary>
		/// 文字列を描画したbitmapを返す(メイン処理)
		/// </summary>
		/// <param name="rectDrawn">描画された領域</param>
		/// <param name="ptOrigin">描画文字列</param>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="drawmode">描画モード</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済Bitmap</returns>
		protected Bitmap DrawPrivateFont(string drawstr, DrawMode drawmode, Color fontColor, Color edgeColor, Color gradationTopColor, Color gradationBottomColor)
		{
			if (this._fontfamily == null || drawstr == null || drawstr == "")
			{
				// nullを返すと、その後bmp→texture処理や、textureのサイズを見て・・の処理で全部例外が発生することになる。
				// それは非常に面倒なので、最小限のbitmapを返してしまう。
				// まずはこの仕様で進めますが、問題有れば(上位側からエラー検出が必要であれば)例外を出したりエラー状態であるプロパティを定義するなり検討します。
				if (drawstr != "")
				{
					Trace.TraceWarning("DrawPrivateFont()の入力不正。最小値のbitmapを返します。");
				}
				_rectStrings = new Rectangle(0, 0, 0, 0);
				_ptOrigin = new Point(0, 0);
				return new Bitmap(1, 1);
			}
			bool bEdge = ((drawmode == DrawMode.Edge || drawmode == DrawMode.EdgeGradation));
			bool bGradation = (drawmode == DrawMode.Gradation || drawmode == DrawMode.EdgeGradation);

			// 縁取りの縁のサイズは、とりあえずフォントの大きさの1/4とする
			float nEdgePt = (bEdge) ? _pt / 4 : 0;

			// 描画サイズを測定する
			Size stringSize = System.Windows.Forms.TextRenderer.MeasureText(drawstr, this._font, new Size(int.MaxValue, int.MaxValue),
				System.Windows.Forms.TextFormatFlags.NoPrefix |
				System.Windows.Forms.TextFormatFlags.NoPadding
			);

			//取得した描画サイズを基に、描画先のbitmapを作成する
			Bitmap bmp = new Bitmap((int)(stringSize.Width + nEdgePt * 2), (int)(stringSize.Height + nEdgePt * 2));
			bmp.MakeTransparent();

			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

				using (StringFormat sf = new StringFormat())
				{
					// 画面下部（垂直方向位置）
					sf.LineAlignment = StringAlignment.Far;
					// 画面中央（水平方向位置）
					sf.Alignment = StringAlignment.Center;
					sf.FormatFlags = StringFormatFlags.NoWrap;

					// レイアウト枠
					Rectangle r = new Rectangle(0, 0, (int)(stringSize.Width + nEdgePt * 2), (int)(stringSize.Height + nEdgePt * 2));

					// 縁取り有りの描画
					if (bEdge)
					{
						// DrawPathで、ポイントサイズを使って描画するために、DPIを使って単位変換する
						// (これをしないと、単位が違うために、小さめに描画されてしまう)
						float sizeInPixels = _font.SizeInPoints * g.DpiY / 72;  // 1 inch = 72 points

						using (System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath())
						{
							gp.AddString(drawstr, this._fontfamily, (int)this._font.Style, sizeInPixels, r, sf);

							// 縁取りを描画する
							using (Pen p = new Pen(edgeColor, nEdgePt))
							{
								p.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
								g.DrawPath(p, gp);

								// 塗りつぶす
								using (Brush br = bGradation ?
									new LinearGradientBrush(r, gradationTopColor, gradationBottomColor, LinearGradientMode.Vertical) as Brush :
									new SolidBrush(fontColor) as Brush)
								{
									g.FillPath(br, gp);
								}
							}
						}
					}
					else
					{
						// 縁取りなしの描画
						using (Brush br = new SolidBrush(fontColor))
						{
							g.DrawString(drawstr, _font, br, 0f, 0f);
						}
						// System.Windows.Forms.TextRenderer.DrawText(g, drawstr, _font, new Point(0, 0), fontColor);
					}
#if debug表示
			g.DrawRectangle( new Pen( Color.White, 1 ), new Rectangle( 1, 1, stringSize.Width-1, stringSize.Height-1 ) );
			g.DrawRectangle( new Pen( Color.Green, 1 ), new Rectangle( 0, 0, bmp.Width - 1, bmp.Height - 1 ) );
#endif
					_rectStrings = new Rectangle(0, 0, stringSize.Width, stringSize.Height);
					_ptOrigin = new Point((int)(nEdgePt * 2), (int)(nEdgePt * 2));
				}
			}

			return bmp;
		}

		/// <summary>
		/// 最後にDrawPrivateFont()した文字列の描画領域を取得します。
		/// </summary>
		public Rectangle RectStrings
		{
			get
			{
				return _rectStrings;
			}
			protected set
			{
				_rectStrings = value;
			}
		}
		public Point PtOrigin
		{
			get
			{
				return _ptOrigin;
			}
			protected set
			{
				_ptOrigin = value;
			}
		}

		#region [ IDisposable 実装 ]
		//-----------------
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected void Dispose(bool disposeManagedObjects)
		{
			if (this.bDispose完了済み)
				return;

			if (disposeManagedObjects)
			{
				// (A) Managed リソースの解放
				if (this._font != null)
				{
					this._font.Dispose();
					this._font = null;
				}
				if (this._pfc != null)
				{
					this._pfc.Dispose();
					this._pfc = null;
				}
				if (this._fontfamily != null)
				{
					this._fontfamily.Dispose();
					this._fontfamily = null;
				}
			}

			// (B) Unamanaged リソースの解放

			this.bDispose完了済み = true;
		}
		//-----------------
		~CPrivateFont()
		{
			this.Dispose(false);
		}
		//-----------------
		#endregion

		#region [ private ]
		//-----------------
		protected bool bDispose完了済み;
		protected Font _font;

		private System.Drawing.Text.PrivateFontCollection _pfc;
		private FontFamily _fontfamily;
		private float _pt;
		private Rectangle _rectStrings;
		private Point _ptOrigin;
		private string _baseFontname = null;
		private bool bIsSystemFont;
		//-----------------
		#endregion
	}
}
