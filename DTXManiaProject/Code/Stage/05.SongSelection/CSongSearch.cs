using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTXMania
{
    internal class CSongSearch
    {
        public CSongSearch()
        {

        }

        public static List<CSongListNode> tSearchForSongs(in List<CSongListNode> listInputList, string strSearchInput)
        {
            //
            tParseArguments(strSearchInput);

            return tInnerSearchFunction(listInputList);
        }

        private static List<CSongListNode> tInnerSearchFunction(in List<CSongListNode> listInputList)
        {
            List<CSongListNode> searchOutputList = new List<CSongListNode>();
            string strSearchText = strCurrentSearchText;

            for (int i = 0; i < listInputList.Count; i++)
            {
                //Recursive search through all directories
                if (listInputList[i].eNodeType == CSongListNode.ENodeType.BOX)
                {
                    List<CSongListNode> tempSearchOutputList = tInnerSearchFunction(listInputList[i].list子リスト);
                    searchOutputList.AddRange(tempSearchOutputList);
                }
                //Match search string only if node is a score or score_midi type
                else if ((listInputList[i].eNodeType == CSongListNode.ENodeType.SCORE) || (listInputList[i].eNodeType == CSongListNode.ENodeType.SCORE_MIDI))
                {
                    bool bFound = false;
                    //First check if allEnabled is set
                    if(bAllEnabled)
                    {
                        //
                        bFound = true;
                    }
                    else
                    {
                        //Check title
                        if (bTitleSearchEnabled)
                        {
                            string l_strTitle = bCaseSensitiveEnabled ? listInputList[i].strタイトル : listInputList[i].strタイトル.ToLower();
                            if (l_strTitle.Contains(bCaseSensitiveEnabled ? strSearchText : strSearchText.ToLower()))
                            {
                                bFound = true;
                            }
                        }

                        if (bArtistSearchEnabled)
                        {
                            //Check artist (Artist info is found in CScore objects only)
                            for (int k = 0; k < listInputList[i].arScore.Length; k++)
                            {
                                if (listInputList[i].arScore[k] != null)
                                {
                                    string l_strArtist = bCaseSensitiveEnabled ? listInputList[i].arScore[k].SongInformation.ArtistName :
                                    listInputList[i].arScore[k].SongInformation.ArtistName.ToLower();
                                    if (l_strArtist.Contains(bCaseSensitiveEnabled ? strSearchText : strSearchText.ToLower()))
                                    {
                                        bFound = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (bCommentSearchEnabled)
                        {
                            //Check comment (Comment is found in CScore objects only)
                            for (int k = 0; k < listInputList[i].arScore.Length; k++)
                            {
                                if (listInputList[i].arScore[k] != null)
                                {
                                    string l_strComment = bCaseSensitiveEnabled ? listInputList[i].arScore[k].SongInformation.Comment :
                                    listInputList[i].arScore[k].SongInformation.Comment.ToLower();
                                    if (l_strComment.Contains(bCaseSensitiveEnabled ? strSearchText : strSearchText.ToLower()))
                                    {
                                        bFound = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (bFound)
                    {
                        CSongListNode foundNode = listInputList[i].ShallowCopyOfSelf();
                        //Remove any children and parent nodes from shallow copy. Search return list is flat.
                        foundNode.list子リスト = null;
                        foundNode.r親ノード = null;

                        searchOutputList.Add(foundNode);
                    }
                }
            }

            return searchOutputList;
        }

        private static void tParseArguments(string strInput)
        {
            resetParameters();
            string[] strArray = strInput.Split(' ');
            List<string> listSplitSearchText = new List<string>();

            /*
            Example input:
                BabyMetal
                /a BabyMetal
                /a /s BABYMETAL
                /t /a dragon
                /all to return all songs in a flat list
            */
            bool bSearchSwitchFound = false;
            for (int i = 0; i < strArray.Length; i++)
            {
                if(strArray[i] == CommentOnlySwitch)
                {
                    bSearchSwitchFound = true;
                    bCommentSearchEnabled = true;
                }
                else if (strArray[i] == TitleOnlySwitch)
                {
                    bSearchSwitchFound = true;
                    bTitleSearchEnabled = true;
                }
                else if (strArray[i] == ArtistOnlySwitch)
                {
                    bSearchSwitchFound = true;
                    bArtistSearchEnabled = true;
                }
                else if (strArray[i] == CaseSensitiveSwitch)
                {
                    bCaseSensitiveEnabled = true;
                }
                else if (strArray[i] == ExitSwitch)
                {
                    //Handled externally. Do nothing for now.
                }
                else if (strArray[i] == AllSwitch)
                {
                    bAllEnabled = true;
                    return;
                }
                else
                {
                    listSplitSearchText.Add(strArray[i]);
                }
            }

            strCurrentSearchText = String.Join(" ", listSplitSearchText.ToArray());

            //If no search switch is set, enable all by default
            if(!bSearchSwitchFound)
            {
                bCommentSearchEnabled = true;
                bTitleSearchEnabled = true;
                bArtistSearchEnabled = true;
            }
        }

        private static void resetParameters()
        {
            bCommentSearchEnabled = false;
            bTitleSearchEnabled = false;
            bArtistSearchEnabled = false;
            bCaseSensitiveEnabled = false;
            bAllEnabled = false;
            strCurrentSearchText = "";
        }

        #region Members
        public readonly static string CommentOnlySwitch = "/c";
        public readonly static string TitleOnlySwitch = "/t";
        public readonly static string ArtistOnlySwitch = "/a";
        public readonly static string CaseSensitiveSwitch = "/s";
        public readonly static string ExitSwitch = "/q";
        public readonly static string AllSwitch = "/all";

        private static bool bCommentSearchEnabled = false;
        private static bool bTitleSearchEnabled = false;
        private static bool bArtistSearchEnabled = false;
        private static bool bCaseSensitiveEnabled = false;
        private static bool bAllEnabled = false;
        private static string strCurrentSearchText = "";

        #endregion

    }
}
