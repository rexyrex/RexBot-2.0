using System;
using System.Collections.Generic;
using RexBot2.Objects;


namespace RexBot2.Utils
{
    public class MasterUtils
    {
        public static string printStringList(string[] input)
        {
            string res = string.Empty;
            for(int i=0; i<input.Length; i++)
            {
                res += input[i] + ", " ;
            }
            return res;
        }
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

            info += "**" + rm.getName() + "** - `" + rm.getDescription() + " { ";
            foreach (string perm in rm.getPermissions())
            {
                info += perm + " ";
            }
            info += "}`";

            return info;
        }

        public static bool isAny(string[] solid, string[] vari)
        {
            foreach(string s in solid)
            {
                foreach(string s1 in vari)
                {
                    if (s1 == s) return true;
                }
            }
            return false;
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
            return l[r];
        }

        public static string getWord(string[] l)
        {
            int r = DataUtils.rnd.Next(l.Length);
            return l[r];
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

        public static string getAnnoyingTTSString()
        {
            string res = string.Empty;
            string consonants = "bcdfghjklmnpqrstvwxyz";
            string vowels = "aeiou";
            int index = DataUtils.rnd.Next(1, consonants.Length);
            int index2 = DataUtils.rnd.Next(1, vowels.Length);
            int index3 = DataUtils.rnd.Next(1, consonants.Length);
            string repeat = consonants[index].ToString() + vowels[index2].ToString();
            for (int i=0; i<92; i++)
            {
                
                res += repeat;
            }

            string[] words = GlobalVars.TONGUE_TWISTERS;
            string word = words[DataUtils.rnd.Next(0, words.Length)];
            string wres = string.Empty;
            int limit = 184 - (int)(184 / word.Length);
            for (int i=0; i<limit; i++)
            {
                wres += word[i % word.Length];
                if(i%word.Length == word.Length-1)
                {
                    wres += " ";
                }
            }
            switch(DataUtils.rnd.Next(1, 6))
            {
                case 4: return wres; 
                case 1: return "MACHINE GUN MOTHER FUCKERS @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@";
                case 2: return "IM A RAP GOD nenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenenen";
                case 3: return "CONGRATZ DOUCHE BAG HERE'S YOUR PRESENT w w w w w w w w w w w w w w w w w w w w w w w w w w w w w w w w w w, w";
                default: return res;
            }
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
            if (needles == null)
            {
                return false;
            }
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

        public static string getMemeHelp()
        {
            string res = string.Empty;
            res += "** - Meme Creation - **\n";
            res += "```MarkDown\n" + "!meme (type) (Top Line) (Bottom Line)\n```";
            res += "\n** - Meme Types - **\n" + "```";
            foreach (string memeType in DataUtils.memeTypesList)
            {
                res += memeType + ", ";
            }
            res += "```";
            return res;
        }

    }
}
