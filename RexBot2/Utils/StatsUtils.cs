using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RexBot2.Utils
{
    public static class StatsUtils
    {
        public static int MessagesRecieved { get; set; } = 0;

        public static int CommandsRun { get; set; } = 0;

        public static int ReactionCount { get; set; } = 0;
        public static int MsgEditCount { get; set; } = 0;
        public static int MsgDeleteCount { get; set; } = 0;
        public static int UserCount { get; set; } = 0;


        public static Dictionary<string, int> commandUsageDict = new Dictionary<string, int>();
        public static Dictionary<string, int> messageUsageDict = new Dictionary<string, int>();
        public static Dictionary<string, int> mostMentionedUsersDict = new Dictionary<string, int>();


        public static void incDictVal(Dictionary<string,int> dict, string key)
        {
            if (dict.ContainsKey(key))
            {
                dict[key]++;
            }
            else
            {
                dict.Add(key, 1);
            }
        }

        public static void updateMentionedUsers(string user)
        {
            incDictVal(mostMentionedUsersDict, user);
        }

        public static void updateCommandUsage(string cmd)
        {
            incDictVal(commandUsageDict, cmd);
        }

        public static void updateMessageUsage(string user)
        {
            incDictVal(messageUsageDict, user);
        }

        public static string getTop3(Dictionary<string,int> dict)
        {
            if (dict.Count == 0)
            {
                return "None";
            }
            else
            {
                var top3 = dict.OrderByDescending(pair => pair.Value).Take(3);
                string res = string.Empty;
                foreach (KeyValuePair<string, int> kvp in top3)
                {
                    res += kvp.Key + " - " + kvp.Value + "\n";
                }
                return res;
            }
        }

        public static string getTop3MentionedUsers()
        {
            return getTop3(mostMentionedUsersDict);
        }

        public static string getTop3Messagers()
        {
            return getTop3(messageUsageDict);
        }

        public static string getTop3Commands()
        {
            return getTop3(commandUsageDict);
        }

        public static int getCommandCount(CommandService cs)
        {
            int cmdCount = 0;
            foreach (CommandInfo c in cs.Commands)
            {
                cmdCount++;
            }
            return cmdCount;
        }
    }
}
