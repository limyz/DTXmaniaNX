using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

namespace DTXMania
{
    [Serializable]
    internal class CProgress
    {
        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct STProgressInfo
        {
            public STProgressInfo(int nPartDuration, short nMistakes, short nNoteCount)
            {
                this.nPartDuration = nPartDuration;
                this.nMistakes = nMistakes;
                this.nNoteCount = nNoteCount;
            }

            public int nPartDuration;
            public short nMistakes;
            public short nNoteCount;
        }

        public int nNumberOfParts;
        public List<STProgressInfo> sTProgressInfos;

        public static void Serialize(string filePath, CProgress cProgressObject)
        {            
            try
            {
                using (Stream stream = File.Open(filePath, FileMode.Create))
                {
                    BinaryWriter bw = new BinaryWriter(stream);
                    bw.Write(cProgressObject.sTProgressInfos.Count);
                    for (int i = 0; i < cProgressObject.sTProgressInfos.Count; i++)
                    {
                        bw.Write(cProgressObject.sTProgressInfos[i].nPartDuration);
                        bw.Write(cProgressObject.sTProgressInfos[i].nMistakes);
                        bw.Write(cProgressObject.sTProgressInfos[i].nNoteCount);
                    }
                    bw.Close();
                }                    
            }
            catch (Exception e)
            {
                Trace.TraceError("Exception Serializing CProgress ...");
                Trace.TraceError(e.Message);
                Trace.TraceError(e.StackTrace);                   
            }            

        }

        public static CProgress Deserialize(string filePath)
        {
            CProgress ret = null;            
            try
            {
                using (Stream stream = File.Open(filePath, FileMode.Open))
                {
                    BinaryReader br = new BinaryReader(stream);
                    int nParts = br.ReadInt32();
                    ret = new CProgress();
                    ret.nNumberOfParts = nParts;
                    ret.sTProgressInfos = new List<STProgressInfo>();

                    //
                    for (int i = 0; i < nParts; i++)
                    {
                        STProgressInfo sTProgressInfo = new STProgressInfo();
                        sTProgressInfo.nPartDuration = br.ReadInt32();
                        sTProgressInfo.nMistakes = br.ReadInt16();
                        sTProgressInfo.nNoteCount = br.ReadInt16();
                        ret.sTProgressInfos.Add(sTProgressInfo);
                    }
                    br.Close();
                }
            }
            catch (Exception e)
            {
                Trace.TraceError("Exception Deserializing CProgress ...");
                Trace.TraceError(e.Message);
                Trace.TraceError(e.StackTrace);
            }            

            return ret;
        }
    }
}
