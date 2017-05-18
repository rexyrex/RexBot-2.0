using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RexBot2.Utils
{
    public class AdminUtils
    {
        public static Dictionary<string, KeyValuePair<double, Stopwatch>> functionRestrictDict;
        //id -> bool ( is sw > double? )
        public AdminUtils()
        {
            functionRestrictDict = new Dictionary<string, KeyValuePair<double, Stopwatch>>();
        }

        //watch out for memory leak
        public static void addRestriction(string username, double timeInSeconds)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            KeyValuePair<double, Stopwatch> kv = new KeyValuePair<double, Stopwatch>(timeInSeconds, sw);
            functionRestrictDict[username] = kv;           
        }

        public static bool isRestrained(string username)
        {
            if(GetRestrainTimeRemaining(username)>0)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public static void RemoveRestrain(string username)
        {
            addRestriction(username, -1);
        }

        public static double GetRestrainTimeRemaining(string username)
        {
            if (functionRestrictDict.ContainsKey(username))
            {
                return Math.Round(functionRestrictDict[username].Key - functionRestrictDict[username].Value.Elapsed.TotalSeconds,2);
            } else
            {
                return -1;
            }            
        }

        public static string GetRestrainedList()
        {
            string res = string.Empty;

            foreach(KeyValuePair<string,KeyValuePair<double,Stopwatch>> kv in functionRestrictDict)
            {
                if (isRestrained(kv.Key))
                {
                    res += kv.Key + " is restrained for " + Math.Round(GetRestrainTimeRemaining(kv.Key),2) + "s\n";
                }
            }
            if (res == string.Empty) res = "Noone is restrained!";
            return res;
        }
    }
}
