using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RexBot2.Utils
{
    public static class Stats
    {
        public static int MessagesRecieved { get; set; } = 0;

        public static int CommandsRun { get; set; } = 0;

        public static Dictionary<string, int> commandUsage = new Dictionary<string, int>();
        public static Dictionary<string, int> messageUsage = new Dictionary<string, int>();



        public static void updateCommandUsage(string cmd)
        {
            if (commandUsage.ContainsKey(cmd))
            {
                commandUsage[cmd]++;
            }
            else
            {
                commandUsage.Add(cmd, 1);
            }
        }

        public static void updateMessageUsage(string user)
        {
            if (messageUsage.ContainsKey(user))
            {
                messageUsage[user]++;
            }
            else
            {
                messageUsage.Add(user, 1);
            }
        }

        public static string getTop3Messagers()
        {
            if (messageUsage.Count == 0)
            {   
                return "No reports";
            }
            else
            {
                var top3 = messageUsage.OrderByDescending(pair => pair.Value).Take(3);
                string res = string.Empty;
                foreach (KeyValuePair<string, int> kvp in top3)
                {
                    res += kvp.Key + " - " + kvp.Value + "\n";
                }
                return res;
            }
        }

        public static string getTop3Commands()
        {
            if (commandUsage.Count == 0)
            {
                return "No reports";
            }
            else
            {
                var top3 = commandUsage.OrderByDescending(pair => pair.Value).Take(3);
                string res = string.Empty;
                foreach (KeyValuePair<string, int> kvp in top3)
                {
                    res += kvp.Key + " - " + kvp.Value + "\n";
                }
                return res;
            }
        }
    }
}
