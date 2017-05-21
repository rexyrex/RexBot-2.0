using Discord.Commands;
using Discord.WebSocket;
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
        public static Dictionary<SocketUser, int> messageUsageDict = new Dictionary<SocketUser, int>();
        public static Dictionary<SocketUser, int> mostMentionedUsersDict = new Dictionary<SocketUser, int>();
        public static Dictionary<SocketUser, double> userSentScoresDict = new Dictionary<SocketUser, double>();
        public static Dictionary<SocketUser, double> avgUserSentScoresDict = new Dictionary<SocketUser, double>();
        public static Dictionary<string, int> wordUsageDict = new Dictionary<string, int>();
        
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
        public static void incDictUserVal(Dictionary<SocketUser, int> dict, SocketUser key)
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

        public static void updateWordFreq(string sentence)
        {
            foreach(string part in sentence.Split())
            {
                if(part.Length > 2)
                    incDictVal(wordUsageDict, part.ToLower());
            }
        }

        public static void updateMentionedUsers(SocketUser user)
        {
            incDictUserVal(mostMentionedUsersDict, user);
        }

        public static void updateCommandUsage(string cmd)
        {
            incDictVal(commandUsageDict, cmd);
        }

        public static void updateMessageUsage(SocketUser user)
        {
            incDictUserVal(messageUsageDict, user);
        }

        public static void updateUserSentScore(SocketUser user, double score)
        {
            if (userSentScoresDict.ContainsKey(user))
            {
                userSentScoresDict[user] += score;
            } else
            {
                userSentScoresDict[user] = score;
            }
            
        }

        public static double getAverageUserSentScore(SocketUser user)
        {
            if (userSentScoresDict.ContainsKey(user))
            {
                return userSentScoresDict[user] / messageUsageDict[user];
            }
            else
            {
                return -100;
            }
        }

        public static string getTop3(Dictionary<string,int> dict)
        {
            if (dict.Count == 0)
            {
                return "None";
            }
            else
            {
                var top3 = dict.OrderByDescending(pair => pair.Value).Take(GlobalVars.STATS_SHOW);
                string res = string.Empty;
                foreach (KeyValuePair<string, int> kvp in top3)
                {
                    res += kvp.Key + " : " + kvp.Value + "\n";
                }
                return res;
            }
        }

        public static string getTop3Words()
        {
            if (wordUsageDict.Count == 0)
            {
                return "None";
            }
            else
            {
                var top3 = wordUsageDict.OrderByDescending(pair => pair.Value).Take(GlobalVars.STATS_SHOW);
                string res = string.Empty;
                foreach (KeyValuePair<string, int> kvp in top3)
                {
                    res += kvp.Key + " : " + kvp.Value + "\n";
                }
                return res;
            }
        }

        public static string getTop3SentScoreUser()
        {
            foreach(SocketUser su in userSentScoresDict.Keys)
            {
                avgUserSentScoresDict[su] = getAverageUserSentScore(su);
            }
            if (avgUserSentScoresDict.Count == 0)
            {
                return "None";
            }
            else
            {
                var top3 = avgUserSentScoresDict.OrderByDescending(pair => pair.Value).Take(GlobalVars.STATS_SHOW);
                string res = string.Empty;
                foreach (KeyValuePair<SocketUser, double> kvp in top3)
                {
                    res += kvp.Key.Mention + " : " + Math.Round(kvp.Value,2) + "\n";
                }
                return res;
            }
        }

        public static string getTop3User(Dictionary<SocketUser, int> dict)
        {
            if (dict.Count == 0)
            {
                return "None";
            }
            else
            {
                var top3 = dict.OrderByDescending(pair => pair.Value).Take(GlobalVars.STATS_SHOW);
                string res = string.Empty;
                foreach (KeyValuePair<SocketUser, int> kvp in top3)
                {
                    res += kvp.Key.Mention + " : " + kvp.Value + "\n";
                }
                return res;
            }
        }

        public static string getTop3MentionedUsers()
        {
            return getTop3User(mostMentionedUsersDict);
        }

        public static string getTop3Messagers()
        {
            return getTop3User(messageUsageDict);
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
