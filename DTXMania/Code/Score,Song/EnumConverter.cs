using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTXMania
{
    public static class EnumConverter
    {
        private static Dictionary<(bool, bool, bool, bool, bool), (EChannel Guitar, EChannel Bass)> dicBoolTupleAsEChannels = new Dictionary<(bool, bool, bool, bool, bool), (EChannel, EChannel)>
        {
            {
                (false, false, false, false, false),
                (EChannel.Guitar_Open, EChannel.Bass_Open)
            },
            {
                (true, false, false, false, false),
                (EChannel.Guitar_Rxxxx, EChannel.Bass_Rxxxx)
            },
            {
                (false, true, false, false, false),
                (EChannel.Guitar_xGxxx, EChannel.Bass_xGxxx)
            },
            {
                (true, true, false, false, false),
                (EChannel.Guitar_RGxxx, EChannel.Bass_RGxxx)
            },
            {
                (false, false, true, false, false),
                (EChannel.Guitar_xxBxx, EChannel.Bass_xxBxx)
            },
            {
                (true, false, true, false, false),
                (EChannel.Guitar_RxBxx, EChannel.Bass_RxBxx)
            },
            {
                (false, true, true, false, false),
                (EChannel.Guitar_xGBxx, EChannel.Bass_xGBxx)
            },
            {
                (true, true, true, false, false),
                (EChannel.Guitar_RGBxx, EChannel.Bass_RGBxx)
            },
            {
                (false, false, false, true, false),
                (EChannel.Guitar_xxxYx, EChannel.Bass_xxxYx)
            },
            {
                (true, false, false, true, false),
                (EChannel.Guitar_RxxYx, EChannel.Bass_RxxYx)
            },
            {
                (false, true, false, true, false),
                (EChannel.Guitar_xGxYx, EChannel.Bass_xGxYx)
            },
            {
                (true, true, false, true, false),
                (EChannel.Guitar_RGxYx, EChannel.Bass_RGxYx)
            },
            {
                (false, false, true, true, false),
                (EChannel.Guitar_xxBYx, EChannel.Bass_xxBYx)
            },
            {
                (true, false, true, true, false),
                (EChannel.Guitar_RxBYx, EChannel.Bass_RxBYx)
            },
            {
                (false, true, true, true, false),
                (EChannel.Guitar_xGBYx, EChannel.Bass_xGBYx)
            },
            {
                (true, true, true, true, false),
                (EChannel.Guitar_RGBYx, EChannel.Bass_RGBYx)
            },
            {
                (false, false, false, false, true),
                (EChannel.Guitar_xxxxP, EChannel.Bass_xxxxP)
            },
            {
                (true, false, false, false, true),
                (EChannel.Guitar_RxxxP, EChannel.Bass_RxxxP)
            },
            {
                (false, true, false, false, true),
                (EChannel.Guitar_xGxxP, EChannel.Bass_xGxxP)
            },
            {
                (true, true, false, false, true),
                (EChannel.Guitar_RGxxP, EChannel.Bass_RGxxP)
            },
            {
                (false, false, true, false, true),
                (EChannel.Guitar_xxBxP, EChannel.Bass_xxBxP)
            },
            {
                (true, false, true, false, true),
                (EChannel.Guitar_RxBxP, EChannel.Bass_RxBxP)
            },
            {
                (false, true, true, false, true),
                (EChannel.Guitar_xGBxP, EChannel.Bass_xGBxP)
            },
            {
                (true, true, true, false, true),
                (EChannel.Guitar_RGBxP, EChannel.Bass_RGBxP)
            },
            {
                (false, false, false, true, true),
                (EChannel.Guitar_xxxYP, EChannel.Bass_xxxYP)
            },
            {
                (true, false, false, true, true),
                (EChannel.Guitar_RxxYP, EChannel.Bass_RxxYP)
            },
            {
                (false, true, false, true, true),
                (EChannel.Guitar_xGxYP, EChannel.Bass_xGxYP)
            },
            {
                (true, true, false, true, true),
                (EChannel.Guitar_RGxYP, EChannel.Bass_RGxYP)
            },
            {
                (false, false, true, true, true),
                (EChannel.Guitar_xxBYP, EChannel.Bass_xxBYP)
            },
            {
                (true, false, true, true, true),
                (EChannel.Guitar_RxBYP, EChannel.Bass_RxBYP)
            },
            {
                (false, true, true, true, true),
                (EChannel.Guitar_xGBYP, EChannel.Bass_xGBYP)
            },
            {
                (true, true, true, true, true),
                (EChannel.Guitar_RGBYP, EChannel.Bass_RGBYP)
            }
        };

        public static EChannel GetEChannelFromInstAndArrayBool(EInstrumentPart inst, bool[] b)
        {
            if (inst != EInstrumentPart.GUITAR && inst != EInstrumentPart.BASS)
            {
                throw new NotImplementedException();
            }
            if (b.Length != 5)
            {
                throw new NotImplementedException();
            }
            return GetEChannelFromInstAndTupleBool(inst, GetTupleBoolRGBYPFromArrayBool(b));
        }

        public static EChannel GetEChannelFromInstAndTupleBool(EInstrumentPart inst, (bool, bool, bool, bool, bool) btuple)
        {
            if (inst != EInstrumentPart.GUITAR && inst != EInstrumentPart.BASS)
            {
                throw new NotImplementedException();
            }
            if (dicBoolTupleAsEChannels.TryGetValue(btuple, out var value))
            {
                if (inst != EInstrumentPart.GUITAR)
                {
                    return value.Item2;
                }
                return value.Item1;
            }
            throw new NotImplementedException();
        }

        public static (bool R, bool G, bool B, bool Y, bool P) GetTupleBoolRGBYPFromArrayBool(bool[] b)
        {
            if (b.Length != 5)
            {
                throw new NotImplementedException();
            }
            return (b[0], b[1], b[2], b[3], b[4]);
        }

        public static bool[] GetArrayBoolRGBYPFromTupleBool((bool R, bool G, bool B, bool Y, bool P) tupleb)
        {
            return new bool[5] { tupleb.R, tupleb.G, tupleb.B, tupleb.Y, tupleb.P };
        }

        public static bool[] GetArrayBoolFromEChannel(EChannel ch)
        {
            return GetArrayBoolRGBYPFromTupleBool(GetTupleBoolFromEChannel(ch));
        }

        public static (bool R, bool G, bool B, bool Y, bool P) GetTupleBoolFromEChannel(EChannel ch)
        {
            (bool, bool, bool, bool, bool) result = (false, false, false, false, false);
            bool flag = false;
            foreach (KeyValuePair<(bool, bool, bool, bool, bool), (EChannel, EChannel)> dicBoolTupleAsEChannel in dicBoolTupleAsEChannels)
            {
                var (eChannel, eChannel2) = dicBoolTupleAsEChannel.Value;
                if (eChannel == ch || eChannel2 == ch)
                {
                    result = dicBoolTupleAsEChannel.Key;
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                return result;
            }
            throw new NotImplementedException();
        }
    }
}
