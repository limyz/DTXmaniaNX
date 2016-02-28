using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DTXMania
{
	internal class CAct演奏GuitarRGB : CAct演奏RGB共通
	{
		// コンストラクタ

		public CAct演奏GuitarRGB()
		{
			base.b活性化してない = true;
		}


		// CActivity 実装（共通クラスからの差分のみ）

		public override int On進行描画()
		{
			if( !base.b活性化してない )
            {
                if (!CDTXMania.ConfigIni.bGuitar有効)
                {
                    return 0;
                }

                //CLASSICシャッター(レーンシャッター)は未実装。
                //if ((CDTXMania.ConfigIni.bCLASSIC譜面判別を有効にする == true ) && ((CDTXMania.DTX.bチップがある.LeftCymbal == false) && ( CDTXMania.DTX.bチップがある.FT == false ) && ( CDTXMania.DTX.bチップがある.Ride == false ) && ( CDTXMania.DTX.bチップがある.LP == false )))
                {
                    //if ( this.txLaneCover != null )
                    {
                        //旧画像
                        //this.txLaneCover.t2D描画(CDTXMania.app.Device, 295, 0);
                        //if (CDTXMania.DTX.bチップがある.LeftCymbal == false)
                        {
                            //this.txLaneCover.t2D描画(CDTXMania.app.Device, 295, 0, new Rectangle(0, 0, 70, 720));
                        }
                        //if ((CDTXMania.DTX.bチップがある.LP == false) && (CDTXMania.DTX.bチップがある.LBD == false))
                        {
                            //レーンタイプでの入れ替わりあり
                            //if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.A || CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.C)
                            {
                                //    this.txLaneCover.t2D描画(CDTXMania.app.Device, 416, 0, new Rectangle(124, 0, 54, 720));
                            }
                            //else if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.B)
                            {
                                //    this.txLaneCover.t2D描画(CDTXMania.app.Device, 470, 0, new Rectangle(124, 0, 54, 720));
                            }
                            //else if (CDTXMania.ConfigIni.eLaneType.Drums == Eタイプ.D)
                            {
                                //    this.txLaneCover.t2D描画(CDTXMania.app.Device, 522, 0, new Rectangle(124, 0, 54, 720));
                            }
                        }
                        //if (CDTXMania.DTX.bチップがある.FT == false)
                        {
                            //this.txLaneCover.t2D描画(CDTXMania.app.Device, 690, 0, new Rectangle(71, 0, 52, 720));
                        }
                        //if (CDTXMania.DTX.bチップがある.Ride == false)
                        {
                            //RDPositionで入れ替わり
                            //if (CDTXMania.ConfigIni.eRDPosition == ERDPosition.RCRD)
                            {
                                //    this.txLaneCover.t2D描画(CDTXMania.app.Device, 815, 0, new Rectangle(178, 0, 38, 720));
                            }
                            //else if (CDTXMania.ConfigIni.eRDPosition == ERDPosition.RDRC)
                            {
                                //    this.txLaneCover.t2D描画(CDTXMania.app.Device, 743, 0, new Rectangle(178, 0, 38, 720));
                            }
                        }
                    }
                }

                #region[ シャッター 変数]
                if (base.txシャッター != null)
                {
                    this.nシャッター上.Guitar = CDTXMania.ConfigIni.nShutterInSide.Guitar;
                    this.nシャッター下.Guitar = CDTXMania.ConfigIni.nShutterOutSide.Guitar;

                    if (CDTXMania.ConfigIni.bReverse.Guitar)
                    {
                        this.nシャッター上.Guitar = CDTXMania.ConfigIni.nShutterOutSide.Guitar;
                        this.nシャッター下.Guitar = CDTXMania.ConfigIni.nShutterInSide.Guitar;
                    }

                    this.dbシャッター上.Guitar = 108 - base.txシャッター.sz画像サイズ.Height + (this.nシャッター上.Guitar * this.db倍率);
                    this.dbシャッター下.Guitar = 720 - 50 - (this.nシャッター下.Guitar * this.db倍率);

                    this.nシャッター上.Bass = CDTXMania.ConfigIni.nShutterInSide.Bass;
                    this.nシャッター下.Bass = CDTXMania.ConfigIni.nShutterOutSide.Bass;

                    if (CDTXMania.ConfigIni.bReverse.Bass)
                    {
                        this.nシャッター上.Bass = CDTXMania.ConfigIni.nShutterOutSide.Bass;
                        this.nシャッター下.Bass = CDTXMania.ConfigIni.nShutterInSide.Bass;
                    }

                    this.dbシャッター上.Bass = 108 - base.txシャッター.sz画像サイズ.Height + (this.nシャッター上.Bass * this.db倍率);
                    this.dbシャッター下.Bass = 720 - 50 - (this.nシャッター下.Bass * this.db倍率);
                }
                #endregion

                #region [ ギター ]
                if (CDTXMania.DTX.bチップがある.Guitar)
                {
                    /*
					for( int j = 0; j < 5; j++ )
					{
						int index = CDTXMania.ConfigIni.bLeft.Guitar ? ( 2 - j ) : j;
						Rectangle rectangle = new Rectangle( index * 24, 0, 0x18, 0x20 );
						//if( base.b押下状態[ index ] )
						{
							rectangle.Y += 0x20;
						}
						if( base.txRGB != null )
						{
							//base.txRGB.t2D描画( CDTXMania.app.Device, 0x1f + ( j * 0x24 ), 3, rectangle );
						}
					}
                     */

                    if (base.txRGB != null)
                    {
                        if (this.nシャッター下.Guitar == 0)
                            base.txRGB.t2D描画(CDTXMania.app.Device, 67, 670, new Rectangle(0, 128, 277, 50));

                        if (this.nシャッター上.Guitar == 0)
                            base.txRGB.t2D描画(CDTXMania.app.Device, 67, 42, new Rectangle(0, (CDTXMania.ConfigIni.bLeft.Guitar ? 64 : 0), 277, 64));
                    }

                    if (base.txシャッター != null)
                    {
                        if (this.nシャッター下.Guitar != 0)
                        {
                            base.txシャッター.t2D描画(CDTXMania.app.Device, 80, (int)this.dbシャッター下.Guitar);

                            if (CDTXMania.ConfigIni.b演奏情報を表示する)
                                this.actLVFont.t文字列描画(195, (int)this.dbシャッター下.Guitar + 5, this.nシャッター下.Guitar.ToString());
                        }
                        if (this.nシャッター上.Guitar != 0)
                        {
                            base.txシャッター.t2D描画(CDTXMania.app.Device, 80, (int)this.dbシャッター上.Guitar);

                            if (CDTXMania.ConfigIni.b演奏情報を表示する)
                                this.actLVFont.t文字列描画(195, (int)this.dbシャッター上.Guitar - 25 + base.txシャッター.sz画像サイズ.Height, this.nシャッター上.Guitar.ToString());
                        }
                    }
                }
                #endregion
                #region [ ベース ]
                if (CDTXMania.DTX.bチップがある.Bass)
                {
                    /*
					for( int j = 0; j < 5; j++ )
					{
						int index = CDTXMania.ConfigIni.bLeft.Guitar ? ( 2 - j ) : j;
						Rectangle rectangle = new Rectangle( index * 24, 0, 0x18, 0x20 );
						//if( base.b押下状態[ index ] )
						{
							rectangle.Y += 0x20;
						}
						if( base.txRGB != null )
						{
							//base.txRGB.t2D描画( CDTXMania.app.Device, 0x1f + ( j * 0x24 ), 3, rectangle );
						}
					}
                     */

                    if (base.txRGB != null)
                    {
                        if (this.nシャッター下.Bass == 0)
                            base.txRGB.t2D描画(CDTXMania.app.Device, 937, 670, new Rectangle(0, 128, 277, 50));

                        if (this.nシャッター上.Bass == 0)
                            base.txRGB.t2D描画(CDTXMania.app.Device, 937, 42, new Rectangle(0, (CDTXMania.ConfigIni.bLeft.Bass ? 64 : 0), 277, 64));
                    }

                    if (base.txシャッター != null)
                    {
                        if (this.nシャッター下.Bass != 0)
                        {
                            base.txシャッター.t2D描画(CDTXMania.app.Device, 950, (int)this.dbシャッター下.Bass);

                            if (CDTXMania.ConfigIni.b演奏情報を表示する)
                                this.actLVFont.t文字列描画(1065, (int)this.dbシャッター下.Bass + 5, this.nシャッター下.Bass.ToString());
                        }
                        if (this.nシャッター上.Bass != 0)
                        {
                            base.txシャッター.t2D描画(CDTXMania.app.Device, 950, (int)this.dbシャッター上.Bass);

                            if (CDTXMania.ConfigIni.b演奏情報を表示する)
                                this.actLVFont.t文字列描画(1065, (int)this.dbシャッター上.Bass - 25 + base.txシャッター.sz画像サイズ.Height, this.nシャッター上.Bass.ToString());
                        }
                    }
                }
                #endregion
                for (int i = 0; i < 10; i++)
                {
                    base.b押下状態[i] = false;
                }
            }
			return 0;
		}
	}
}
