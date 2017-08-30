using System;
using System.Collections.Generic;
using RexBot2.Objects;
using System.IO;
using System.Linq;
using Discord.WebSocket;
using Discord;
using Discord.Commands;
using System.Diagnostics;

namespace RexBot2.Utils
{


    public class DataUtils
    {
        public static List<string> picNames;
        public static List<string> adjList;
        public static List<string> nounList;
        public static List<string> verbList;
        public static List<string> expList;
        public static List<string> introList;
        public static List<string> laughList;
        public static List<string> heroList;
        public static List<string> positionList;
        public static List<string> memeTypesList;
        public static List<string> stopWordsList;

        public static Dictionary<string, string[]> usernameDict;

        public static List<string> trainPhrases1;
        public static List<string> trainPhrases2;
        public static List<string> trainPhrases3;

        //Only stores commands suitable for !random command
        public static List<string> commands;

        public static string bingAuthStr = "";

        public static string textPath = "Data/texts/";

        public static Dictionary<string[], string> aliases;
        public static Dictionary<string, string[]> responses;
        public static Dictionary<string, RexMode> modes;
        public static Dictionary<string, double> wordScoresDict;


        public static Dictionary<string, Dictionary<string, string>> rexDB;

        public static Dictionary<string, int> reports;
        public static string[] games = GlobalVars.BOT_GAMES_LIST;

        //public static VideoSearch youtubeSearcher;

        public static string mode = GlobalVars.DEFAULT_MODE;

        public static AudioService rexAS;

        public static Random rnd;

        public static Dictionary<string, string> helpEmojiBinds;
        public static DiscordSocketClient _client;
        private static CommandService _cs;
        public DataUtils(DiscordSocketClient client, CommandService cs)
        {
            _client = client;
            _cs = cs;
            InitTwitter();
            InitVars();
        }
        private void InitTwitter()
        {
            Tweetinvi.Auth.SetUserCredentials(GlobalVars.TWITTER_CONSUMER_KEY, GlobalVars.TWITTER_CONSUMER_SECRET, GlobalVars.TWITTER_USER_ACCESS_TOKEN, GlobalVars.TWITTER_USER_ACCESS_SECRET);
            var zuser = Tweetinvi.User.GetAuthenticatedUser();
            Logger.NewLine();
            Logger.Log(LogSeverity.Info, "DataUtils", "Twitter initializing: " + zuser);
        }

        private void InitVars()
        {            
            rexAS = new AudioService();
            rnd = new Random();
            picNames = new List<string>();
            adjList = new List<string>();
            nounList = new List<string>();
            verbList = new List<string>();
            expList = new List<string>();
            introList = new List<string>();
            laughList = new List<string>();
            heroList = new List<string>();
            positionList = new List<string>();
            memeTypesList = new List<string>();

            stopWordsList = new List<string>();

            trainPhrases1 = new List<string>();
            trainPhrases2 = new List<string>();
            trainPhrases3 = new List<string>();

            commands = new List<string>();

            aliases = new Dictionary<string[], string>();
            responses = new Dictionary<string, string[]>();
            modes = new Dictionary<string, RexMode>();
            rexDB = new Dictionary<string, Dictionary<string, string>>();
            reports = new Dictionary<string, int>();
            helpEmojiBinds = new Dictionary<string, string>();
            usernameDict = new Dictionary<string, string[]>();
            wordScoresDict = new Dictionary<string, double>();

            modes.Add("xander", new RexMode("xander", "Only few commands functional", new string[] { "functions","xander" }));
            modes.Add("quiet", new RexMode("quiet", "No auto triggers. All functions online.", new string[] { "functions" }));
            modes.Add("moderate", new RexMode("moderate", "Few auto triggers.All functions online.", new string[] { "functions", "trigger 7","tts","auto restrain"}));
            modes.Add("active", new RexMode("active", "Occasional auto triggers.", new string[] { "functions", "trigger 30", "tts", "auto restrain" }));
            modes.Add("loud", new RexMode("loud", "Many auto triggers. Status changes.", new string[] { "functions", "trigger 60", "status", "tts", "auto restrain" }));
            modes.Add("tooloud", new RexMode("tooloud", "RexBot on Steroids", new string[] { "functions", "trigger 100", "status", "tts", "auto restrain" }));
            modes.Add("cat", new RexMode("cat", "Posts a cat photo for every message - Use with caution", new string[] { "functions", "trigger 100", "status", "cat", "tts", "auto restrain" }));

            Logger.Log(LogSeverity.Info, "DataUtils", "Starting Data Population...");
            repopulate();
            
            
            populateHelpEmojiBinds(); // no need to reload
            
            
            loadRexDB(); // no need to reload
            populateUsernameDict(); // can't cuz internal load
            Logger.Log(LogSeverity.Info, "DataUtils", "Done Loading Data!");
            Logger.NewLine();
        }

        public static void repopulate()
        {
            populate(adjList, "adjective.txt");
            populate(nounList, "noun.txt");
            populate(verbList, "verb.txt");
            populate(expList, "statement.txt");
            populate(introList, "intro.txt");
            populate(laughList, "laugh.txt");
            populate(heroList, "heros.txt");
            populate(positionList, "position.txt");
            populate(memeTypesList, "memeType.txt");
            populate(trainPhrases1, "trainphrases1.txt");
            populate(trainPhrases2, "trainphrases2.txt");
            populate(trainPhrases3, "trainphrases3.txt");
            populate(stopWordsList, "stopwords.txt");
            populateResponses();
            populateCommands();
            populatePicFileNames();
            populateWordScoreDict();
            AliasUtils.ParseAliases(); //cant reload due to array key design
        }

        private static void populateCommands()
        {
            foreach(CommandInfo c in _cs.Commands)
            {
                if(!commands.Contains(c.Name))
                    commands.Add(c.Name);
            }

            //remove commands not suitable for random
            //foreach(string dis in GlobalVars.DISABLED_FROM_RANDOM)
            //{
            //    commands.Remove(dis);
            //}
        }

        public static string getPossibleCommand(int argCount, string argType = "username")
        {
            if (argCount == 0)
            {
                return MasterUtils.getWord(GlobalVars.POSSIBLE_TO_RANDOM_0ARG);
            } else if(argCount ==1 && argType == "username")
            {
                return MasterUtils.getWord(GlobalVars.POSSIBLE_TO_RANDOM_1ARG_USERNAME) + " " + getRandomName();
            } else if(argCount == 1 && argType == "shortsearch")
            {
                return MasterUtils.getWord(GlobalVars.POSSIBLE_TO_RANDOM_1ARG_SHORTSEARCH) + " " + MasterUtils.getWord(GlobalVars.RANDOM_SHORTSEARCH_TERMS);
            } else if (argCount == 1 && argType == "longsearch")
            {
                return MasterUtils.getWord(GlobalVars.POSSIBLE_TO_RANDOM_1ARG_LONGSEARCH) + " " + MasterUtils.getWord(GlobalVars.RANDOM_LONGSEARCH_TERMS) + " " + MasterUtils.sillyName();
            }
                return "error";
        }

        public static string getRandomName()
        {
            return usernameDict.ElementAt(rnd.Next(0, usernameDict.Count)).Value[0];
        }

        private static void populateWordScoreDict()
        {
            wordScoresDict.Clear();
            string line;
            try
            {
                FileStream fsr = new FileStream(textPath + "wordscores.txt", FileMode.Open, FileAccess.Read);
                using (StreamReader sr = new StreamReader(fsr))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] res = line.Split('\t');
                        wordScoresDict[res[0].ToLower()] = double.Parse(res[1]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("populate wordscores error");
                Console.WriteLine(e.Message);
            }
        }

        public static double scoreSentence(string sentence)
        {
            string[] divided = sentence.ToLower().Split();
            double score = 0;
            for (int i=0; i<divided.Length; i++)
            {
                if (wordScoresDict.ContainsKey(divided[i]))
                {
                    score += wordScoresDict[divided[i]];
                }
            }
            score = score / divided.Length;
            return score;
        }

        public static async void changeMode(string newmode)
        {
            mode = newmode;
            if(mode == "xander")
            {
                await _client.SetStatusAsync(UserStatus.DoNotDisturb);
                await _client.SetGameAsync("Xander Mode");
            } else
            {
                await _client.SetStatusAsync(UserStatus.Online);
                await _client.SetGameAsync(newmode + " mode");
            }
        }

        public static async void turnOffStatus()
        {
            await _client.SetStatusAsync(UserStatus.DoNotDisturb);
            await _client.SetGameAsync("Shutting down...");
        }

        private void populateUsernameDict()
        {
            usernameDict.Add("Schafer#7273", new string[] { "Henry", "201292109884424193"});
            usernameDict.Add("Rexyrex#5838", new string[] { "Adrian", "308305348643782656" });
            usernameDict.Add("CPTOblivious#4652", new string[] { "Nick", "106222902575104000" });
            usernameDict.Add("Geffo#1689", new string[] { "Geoff", "200019349061369856" });
            usernameDict.Add("RayRay#4807", new string[] { "Ray", "310263594233364490" });
            usernameDict.Add("BonoboCop#0335", new string[] { "Xander", "237170530157854732" });
            usernameDict.Add("Wolfy#8611", new string[] { "Ryan", "244522891587223552" });
            usernameDict.Add("Ryanne#6203", new string[] { "Guki", "206378968935301120" });
            usernameDict.Add("Laura#7174", new string[] { "Laura", "277595678186930176" });
            usernameDict.Add("rooster212#7948", new string[] { "Jamie", "200016915538640896" });
            usernameDict.Add("Pash#8006", new string[] { "Pash", "231862514999099392" });
            usernameDict.Add("Andy", new string[] { "Andy", "0" });
            usernameDict.Add("Pink Socks#1146", new string[] { "Emily", "311668112007102464" });
            usernameDict.Add("RexBot#4568", new string[] { "RexBot", "309908194208251904" });
        }

        public static ulong getUserIDFromUsername(string username)
        {
            if (usernameDict.ContainsKey(username))
            {         
                return ulong.Parse(usernameDict[username][1]);
            } else
            {
                return 0;
            }
        }

        public static string getNameFromUsername(string username)
        {
            if (usernameDict.ContainsKey(username))
            {
                return usernameDict[username][0];
            }
            else
            {
                return username;
            }
        }

        public static string getRawStringFromFile(string path)
        {
            string res = string.Empty;
            string line = string.Empty;
            try
            {
                FileStream fsr = new FileStream(path, FileMode.Open, FileAccess.Read);
                using (StreamReader sr = new StreamReader(fsr))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        res += line + "\n";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Raw string read error");
                Console.WriteLine(e.Message);
            }
            return res;
        }

        public static string getReportTopList()
        {
            if (reports.Count == 0)
            {
                return "No reports";
            } else
            {
                var top3 = reports.OrderByDescending(pair => pair.Value).Take(GlobalVars.STATS_SHOW);
                string res = string.Empty;
                foreach(KeyValuePair<string,int> kvp in top3)
                {
                    SocketUser su = _client.GetUser(ulong.Parse(DataUtils.usernameDict[kvp.Key][1]));
                    StatsUtils.updateMentionedUsers(su);
                    res += su.Mention + " : " + kvp.Value + "\n";
                }
                return res;
            }
        }

        private void populateHelpEmojiBinds()
        {
            helpEmojiBinds.Add("admin", "🤖");
            helpEmojiBinds.Add("music", "🎵");
            helpEmojiBinds.Add("audio", "🎵");
            helpEmojiBinds.Add("info", "📊");
            helpEmojiBinds.Add("meme builder", "🐱");
            helpEmojiBinds.Add("pic", "📸");
            helpEmojiBinds.Add("rexdb", "💾");
            helpEmojiBinds.Add("test", "🛠");
            helpEmojiBinds.Add("text", "📋");
            helpEmojiBinds.Add("troll", "⁉️");
            helpEmojiBinds.Add("web", "🌐");
        }

        public static string getHelpEmojiBind(string category)
        {
            if (helpEmojiBinds.ContainsKey(category))
            {
                return helpEmojiBinds[category];
            } else
            {
                return "🤜";
            }
        }

        private static void populateResponses()
        {
            string line;
            responses.Clear();
            try
            {
                FileStream fsr = new FileStream(textPath + "responses.txt", FileMode.Open, FileAccess.Read);
                using (StreamReader sr = new StreamReader(fsr))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        int count = line.Count(x => x == '|');
                        count++;
                        string[] res = line.Split('ㄱ');
                        string[] resSplit = res[1].Split('|');
                        string[] responseStrings = new string[count];


                        for (int i = 0; i < count; i++)
                        {
                            responseStrings[i] = resSplit[i];
                        }

                        responses[res[0]] = responseStrings;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("populate responses error");
                Console.WriteLine(e.Message);
            }
        }

        private static void populate(List<string> l, string filename)
        {
            string line;
            string inputFilePath = textPath + filename;
            try
            {
                FileStream fsr = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
                using (StreamReader sr = new StreamReader(fsr))
                {
                    while ((line = sr.ReadLine()) != null && !l.Contains(line))
                    {
                        l.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("populate error");
                Console.WriteLine(e.Message);
            }
        }
        public static void populatePicFileNames()
        {
            DirectoryInfo d = new DirectoryInfo("Data/friendpics/");
            FileInfo[] Files = d.GetFiles("*.*");
            foreach (FileInfo file in Files)
            {
                if(!picNames.Contains(file.Name))
                    picNames.Add(file.Name);
            }
        }


        public static string listRexDB(string username)
        {
            string res = string.Empty;
            int count = 0;
            res += "```Markdown\n";
            foreach (string s in rexDB[username].Keys)
            {
                count++;
                res += "id: <" + s + "> contents: <" + rexDB[username][s] + ">\n";
            }
            res += "```";
            if (count == 0)
            {
                res = "You have nothing stored under your username!";
            }
            return res;
        }

        public static void loadRexDB()
        {
            //Usernameㄱidㄱcontent
            string line;

            try
            {
                FileStream fsr = new FileStream(textPath + "rexdb.txt", FileMode.Open, FileAccess.Read);
                using (StreamReader sr = new StreamReader(fsr))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] afterSplit = line.Split('ㄱ');
                        string userName = afterSplit[0];
                        string id = afterSplit[1];
                        string content = afterSplit[2];
                        if (!rexDB.ContainsKey(userName))
                        {
                            rexDB[userName] = new Dictionary<string, string>();
                        }
                        if (rexDB[userName].ContainsKey(id))
                        {
                            rexDB[userName][id] = content;
                        }
                        else
                        {
                            rexDB[userName].Add(id, content);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("rexDB load error");
                Console.WriteLine(e.Message);
            }
        }

        //return content from dictionary
        public static string getFromRexDB(string username, string id)
        {
            string res = string.Empty;
            if (rexDB.ContainsKey(username) && rexDB[username].ContainsKey(id))
            {
                res = rexDB[username][id];
            }
            else
            {
                res = "You have nothing stored here";
            }
            return res;
        }

        //Add line to file + update current dictionary
        //File wont delete existing id's
        //Load process will simply overwrite so most recent pair survives
        public static void writeToRexDB(string userName, string id, string content)
        {
            if (MasterUtils.ContainsAny(content, new string[] { "!meme" }) && content.Count(x => x == '(') == 3)
            {

                int bracketCount = 0;
                string type = string.Empty;
                string topText = string.Empty;
                string botText = string.Empty;

                for (int i = 0; i < content.Length; i++)
                {

                    if (content[i] == '(' || content[i] == ')')
                    {
                        bracketCount++;
                    }
                    if (bracketCount == 3)
                    {
                        if (content[i] != '(')
                            topText += content[i];
                    }
                    if (bracketCount == 5)
                    {
                        if (content[i] != '(')
                            botText += content[i];
                    }
                    if (bracketCount == 1)
                    {
                        if (content[i] != ')' && content[i] != '(')
                            type += content[i];
                    }
                }
                topText = MasterUtils.processTextForMeme(topText);
                botText = MasterUtils.processTextForMeme(botText);
                string final = "https://memegen.link/" + type + "/" + topText + "/" + botText + ".jpg";
                content = final;
            }

            if (!rexDB.ContainsKey(userName))
            {
                rexDB[userName] = new Dictionary<string, string>();
            }

            if (rexDB[userName].ContainsKey(id))
            {
                rexDB[userName][id] = content;
            }
            else
            {
                rexDB[userName].Add(id, content);
            }

            using (StreamWriter sw = File.AppendText(textPath + "rexdb.txt"))
            {
                sw.WriteLine(userName + 'ㄱ' + id + 'ㄱ' + content);
            }
        }
    }
}
