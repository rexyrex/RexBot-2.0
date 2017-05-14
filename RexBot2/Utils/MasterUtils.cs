using System;
using System.Collections.Generic;
using System.Text;
using RexBot2.Objects;

namespace RexBot2.Utils
{
    public class MasterUtils
    {
        public static string getAllModesInfo()
        {
            string res = string.Empty;

            foreach (KeyValuePair<string, RexMode> kv in DataUtils.modes)
            {
                res += getModeInfo(kv.Value) + "\n";
            }

            return res;
        }

        public static string getModeInfo(RexMode rm)
        {
            string info = string.Empty;

            info += "**" + rm.getName() + "** - " + rm.getDescription() + " { ";
            foreach (string perm in rm.getPermissions())
            {
                info += perm + " ";
            }
            info += "}";

            return info;
        }

        public static string processTextForMeme(string input)
        {
            string res = string.Empty;
            res = input.Replace("-", "--");
            res = res.Replace(' ', '-');
            res = res.Replace("_", "__");
            res = res.Replace("?", "~q");
            res = res.Replace("%", "~p");
            res = res.Replace("#", "~h");
            res = res.Replace("/", "~s");
            return res;
        }

        public static string getWord(List<string> l)
        {
            int r = DataUtils.rnd.Next(l.Count);
            return (string)l[r];
        }

        public static string commentOnPic()
        {
            string res = getWord(DataUtils.laughList) + " " + getWord(DataUtils.introList) + " ";

            res += sillyName();
            if (roll(50)) { res += "! " + getWord(DataUtils.expList); }
            return res;
        }

        public static string sillyName()
        {
            Random rnzd = new Random();
            int rz = rnzd.Next(4);
            string res = string.Empty;
            for (int i = 0; i < rz; i++)
            {
                res += getWord(DataUtils.adjList) + " ";
            }

            res += getWord(DataUtils.nounList);
            return res;
        }

        public static Boolean roll(int chance)
        {
            int r = DataUtils.rnd.Next(1, 100);
            if (r <= chance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string stripName(string name)
        {
            return name.Split('#')[0];
        }

        public static bool ContainsAny(string haystack, params string[] needles)
        {
            foreach (string needle in needles)
            {
                if (haystack.Contains(needle))
                    return true;
            }
            return false;
        }

        public static bool isMode(string reqMode)
        {
            string check = reqMode.ToLower();
            foreach (KeyValuePair<string, RexMode> kv in DataUtils.modes)
            {
                if (reqMode == kv.Key)
                {
                    return true;
                }
            }
            return false;
        }

        public static string reverse(string str)
        {
            string res = string.Empty;
            for(int i=0; i<str.Length; i++)
            {
                res += str[str.Length - i - 1];
            }
            return res;

        }

    }
}
