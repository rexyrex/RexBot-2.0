using System;
using System.Collections.Generic;
using RexBot2.Objects;
using RexBot2.Timers;
using System.IO;
using System.Linq;
using Discord.WebSocket;
using Discord;
using Discord.Commands;
using System.Diagnostics;
using Newtonsoft.Json;

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

        public static List<string> superHateList;

        //Only stores commands suitable for !random command
        public static List<string> commands;

        public static string bingAuthStr = "";

        public static bool activation = false;

        public static string textPath = "Data/texts/";

        public static List<ShopItem> shop;

        public static Dictionary<string[], string> aliases;
        public static Dictionary<string, string[]> responses;
        public static Dictionary<string, RexMode> modes;
        public static Dictionary<string, double> wordScoresDict;

        public static int coinsOnGround;

        public static int companyIndex;
        public static List<Company> companies;
        public static List<Company> allcompanies;
        public static Dictionary<string, Dictionary<Company, DateTime>> investments;

        public static Dictionary<string, Dictionary<DateTime, Email>> emails;

        public static Dictionary<string, Dictionary<string, string>> rexDB;

        public static Dictionary<string, int> reports;
        public static Dictionary<string, int> coins;
        public static Dictionary<string, int> waddchances;
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

            superHateList = new List<string>();

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
            coins = new Dictionary<string, int>();
            waddchances = new Dictionary<string, int>();
            helpEmojiBinds = new Dictionary<string, string>();
            usernameDict = new Dictionary<string, string[]>();
            wordScoresDict = new Dictionary<string, double>();

            shop = new List<ShopItem>();

            companies = new List<Company>();
            allcompanies = new List<Company>();
            investments = new Dictionary<string, Dictionary<Company, DateTime>>();
            emails = new Dictionary<string, Dictionary<DateTime, Email>>();

            companyIndex = 0;
            coinsOnGround = 0;

            populateModes();

            populateShop();

            Logger.Log(LogSeverity.Info, "DataUtils", "Starting Data Population...");
            repopulate();

            repopulateCompanies();

            populateHelpEmojiBinds(); // no need to reload

            //loadRexDB();
            Logger.Log(LogSeverity.Info, "DataUtils", "Done Loading Data!");
            Logger.NewLine();
        }

        public static void sendEmail(string sentBy, string sentTo, string contents)
        {
            if (!emails.ContainsKey(sentTo))
            {
                emails[sentTo] = new Dictionary<DateTime, Email>();
            }

            emails[sentTo].Add(DateTime.Now, new Email(sentBy, contents));
        }

        public static int getEmailCount(string username)
        {
            int count = 0;
            foreach (KeyValuePair<DateTime, Email> kvp in emails[username])
            {
                count++;
            }

            return count;
        }

        public static string getLastEmail(string username)
        {
            Email email = emails[username].ElementAt(0).Value;
            DateTime sentTime = emails[username].ElementAt(0).Key;
            string author = email.sender;
            string content = email.contents;
            TimeSpan ts = sentTime - DateTime.Now;
            string timepassed = ts.ToString("h'h 'm'm 's's'");
            string finalRes = "```";
            finalRes += "Sent By:" + author + "\n";
            finalRes += "Sent At:" + DateTime.Now + " (" + timepassed + " ago)" + " \n";
            finalRes += "Content:" + content + "";
            finalRes += "```";
            emails[username].Remove(sentTime);

            return finalRes;
        }

        public static bool hasEmail(string username)
        {
            if (!emails.ContainsKey(username))
            {
                return false;
            }
            if (emails[username].Count == 0)
            {
                return false;
            }
            return true;
        }

        public static void repopulateCompanies()
        {

            companies.Clear();

            int compcount = rnd.Next(4, 7);

            for(int i=0; i<compcount; i++)
            {
                generateCompany();
            }
            
        }

        public static bool existsVisibleCompanyWIthID(int id)
        {
            foreach (Company comp in companies)
            {
                if (comp.Index == id)
                {
                    return true;
                }
            }
            return false;
        }

        public static Company getCompanyWithID(int id)
        {
            foreach(Company comp in allcompanies)
            {
                if(comp.Index == id)
                {
                    return comp;
                }
            }

            return null;
        }

        public static string getInvestmentSuccessString(int max, int actual)
        {
            double res = (double)actual / max;
            string resstring = "";
            if(res > 0.9)
            {
                resstring += "Extremely Successful";
            } else if(res > 0.8)
            {
                resstring += "Successful";
            } else if(res > 0.7)
            {
                resstring += "Quite Successful";
            }
            else if (res > 0.6)
            {
                resstring += "Not Bad";
            }
            else if (res > 0.5)
            {
                resstring += "OK";
            }
            else if (res > 0.4)
            {
                resstring += "Meh";
            }
            else if (res > 0.3)
            {
                resstring += "Pretty bad";
            }
            else if (res > 0.2)
            {
                resstring += "Aweful";
            }
            else if (res > 0.1)
            {
                resstring += "Disastrous";
            } else
            {
                resstring += "BASICALLY WORST CASE SCENARIO";
            }
            return resstring;
        }

        public static bool hasMadeAnInvestment(string username)
        {
            if (investments.ContainsKey(username))
            {
                return true;
            }
            return false;
        }

        public static bool alreadyInvestedInCompany(string username, Company comp)
        {
            if (investments.ContainsKey(username))
            {
                foreach (KeyValuePair<Company, DateTime> kvp in investments[username])
                {
                    if(kvp.Key.Index == comp.Index)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void returnInvestedCoins()
        {
            foreach(string username in investments.Keys)
            {
                foreach (KeyValuePair<Company, DateTime> kvp in investments[username])
                {
                    gainCoins(username, kvp.Key.AvailableShare);
                }
            }
            investments.Clear();
                
        }

        public static void makeInvestment(string username, Company comp)
        {
            if (!investments.ContainsKey(username))
            {
                investments[username] = new Dictionary<Company, DateTime>();
            }
            TimeSpan time = new TimeSpan(0, comp.Duration, 0);
            DateTime completionDate = DateTime.Now + time;
            investments[username].Add(comp, completionDate);
        }

        public static void generateCompany()
        {
            string[] nameends = { "Society", "Zoo", "Hospital", "University", "Coorporation",
                "Charity", "Inc", "Games", "Education", "Construction", "Photography", "Pornography" };
            int nameendindex = rnd.Next(0, nameends.Length);
            int risk = rnd.Next(0, 6);
            int ethicality = rnd.Next(0, 6);
            int maxinvestment = rnd.Next(20, 300);
            int maxreward = rnd.Next(105, 220);
            int duration =  rnd.Next(180, 600);

            companyIndex++;

            string companyName = MasterUtils.capitalizeFirstChar(MasterUtils.getWord(adjList)) + " " + MasterUtils.capitalizeFirstChar(MasterUtils.getWord(nounList)) + " " + nameends[nameendindex];
            

            Company comp = new Company(companyIndex, companyName, risk, ethicality, maxinvestment, maxreward, duration);
            companies.Add(comp);
            allcompanies.Add(comp);
            //Logger.NewLine("added " + comp.ToString());
        }

        public static void serializeAll()
        {
            serializeRexDB();
            serializeUsernameDict();
            serializeReports();
            serializeCoins();
            serializeWAddChances();
        }

        public static void loadAllSerializables()
        {
            serializeLoadUsernameDict();
            serializeLoadCoins();
            serializeLoadReports();
            serializeLoadWAddChances();
            serializeLoadRexDB();
        }

        public static void serializeWAddChances()
        {
            File.WriteAllText(textPath + "waddchances_serialized.json", Newtonsoft.Json.JsonConvert.SerializeObject(waddchances));
        }

        public static void serializeReports()
        {
            File.WriteAllText(textPath + "reports_serialized.json", Newtonsoft.Json.JsonConvert.SerializeObject(reports));
        }

        public static void serializeCoins()
        {
            
            File.WriteAllText(textPath + "coins_serialized.json", Newtonsoft.Json.JsonConvert.SerializeObject(coins));
        }

        public static void serializeRexDB()
        {
            File.WriteAllText(textPath + "rexdb_serialized.json", Newtonsoft.Json.JsonConvert.SerializeObject(rexDB));
        }

        public static void serializeUsernameDict()
        {
            File.WriteAllText(textPath + "usernamedict_serialized.json", Newtonsoft.Json.JsonConvert.SerializeObject(usernameDict));
        }

        public static void serializeLoadWAddChances()
        {
            waddchances = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(textPath + "waddchances_serialized.json"));
        }

        public static void serializeLoadReports()
        {
            reports = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(textPath + "reports_serialized.json"));
        }

        public static void serializeLoadCoins()
        {
            coins = JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(textPath + "coins_serialized.json"));
        }

        public static void serializeLoadUsernameDict()
        {
            usernameDict = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(File.ReadAllText(textPath + "usernamedict_serialized.json"));
        }

        public static void serializeLoadRexDB()
        {
            rexDB = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText(textPath + "rexdb_serialized.json"));
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
            //serializeLoadUsernameDict();
            loadAllSerializables();
            //populateUsernameDict("usernameDict.txt");
            AliasUtils.ParseAliases(); //cant reload due to array key design
        }

        private static void populateModes()
        {
            modes.Add("xander", new RexMode("xander", "Only few commands functional", new string[] { "functions", "xander" }));
            modes.Add("quiet", new RexMode("quiet", "No auto triggers. All functions online.", new string[] { "functions" }));
            modes.Add("moderate", new RexMode("moderate", "Few auto triggers.All functions online.", new string[] { "functions", "trigger 7", "tts", "auto restrain" }));
            modes.Add("active", new RexMode("active", "Occasional auto triggers.", new string[] { "functions", "trigger 30", "tts", "auto restrain" }));
            modes.Add("loud", new RexMode("loud", "Many auto triggers. Status changes.", new string[] { "functions", "trigger 60", "status", "tts", "auto restrain" }));
            modes.Add("tooloud", new RexMode("tooloud", "RexBot on Steroids", new string[] { "functions", "trigger 100", "status", "tts", "auto restrain" }));
            modes.Add("cat", new RexMode("cat", "Posts a cat photo for every message - Use with caution", new string[] { "functions", "trigger 100", "status", "cat", "tts", "auto restrain" }));
        }


        //call name used for calling cost
        private static void populateShop()
        {
            //cmdcallname, argcount, cost, syntax, description
            shop.Add(new ShopItem("report", 2, "Super Report",50,"buy report <username>", "50% chance to increase <username>'s reports by 77"));
            shop.Add(new ShopItem("forgive", 2, "Super Forgive", 50, "buy forgive <username>", "50% chance to decrease <username>'s reports by 77"));
            shop.Add(new ShopItem("w", 1, "Super W", 30, "buy w", "100% chance to proc !w"));
            shop.Add(new ShopItem("wchance", 1, "Increase W Chance", 1000, "buy incw", "Increase !w chance by 1%"));
            //shop.Add(new ShopItem("beg", 1, "Super Beg", 200, "buy beg", "50% chance to increase reports by 400"));
            shop.Add(new ShopItem("catmode", 1, "Temp Cat Mode", 150, "buy catmode", "Cat mode is on for 30 seconds"));
            shop.Add(new ShopItem("restrain", 2, "Super Restrain", 80, "buy restrain <username>", "Restrains <username> for 2-3mins"));
            shop.Add(new ShopItem("purge", 2, "Super Purge", 70, "buy purge <amount>", "Delete the last <amount> messages (0<=amount<30)"));
            shop.Add(new ShopItem("tts", 2, "Super TTS", 180, "buy tts <username>", "Rexbot starts blurting out random phrases every time someone says something (duration: 3 min)"));
            shop.Add(new ShopItem("annoy", 2, "Super Annoy", 220, "buy annoy <username>", "Whenever <username> types something, rexbot will use everything in its arsenal to annoy this person (duration: 5 min)"));
            shop.Add(new ShopItem("confuse", 2, "Super Confuse", 130, "buy confuse <username>", "Rexbot will scramble the words of <username>'s messages so that people have to guess what s/he is saying (duration: 5 mins)"));
            shop.Add(new ShopItem("bribe", 2, "Super Bribe", 100, "buy bribe <amount>", "Attempt to bribe rexbot to treat you better in the future. Amount must be greater than 100"));
        }

        public static int getReportCount(string username)
        {
            if (reports.ContainsKey(username))
            {
                return reports[username];
            }
            else
            {
                return 0;
            }
        }

        public static int getCoinCount(string username)
        {
            if (coins.ContainsKey(username))
            {
                return coins[username];
            }
            else
            {
                return 0;
            }
        }

        //Check if anyone is above report cap and make appropriate actions
        public static void cleanReports()
        {
            foreach(KeyValuePair<string, int> report in reports){
                if (report.Value > GlobalVars.MAX_REPORTS)
                {
                    gainCoins(report.Key, GlobalVars.MAX_REPORTS);
                    reports[report.Key] = 0;
                }
                if (report.Value <0)
                {
                    reports[report.Key] = 0;
                }
            }
            serializeReports();
            serializeCoins();
        }

        public static void gainReports(string username, int amount)
        {
            if (reports.ContainsKey(username))
            {
                reports[username] += amount;
            }
            else
            {
                reports[username] = amount;
            }
            cleanReports();
            serializeReports();
        }

        public static void incReports(string username)
        {
            gainReports(username, 1);
        }

        public static void setReports(string username, int amount)
        {
            reports[username] = amount;
            cleanReports();
            serializeReports();
        }


        public static void gainCoins(string username, int amount)
        {
            if (coins.ContainsKey(username))
            {
                coins[username] += amount;
            }
            else
            {
                coins[username] = amount;
            }
            serializeCoins();
        }

        public static void setCoins(string username, int amount)
        {
            coins[username] = amount;            
            serializeCoins();
        }

        public static void setWAddChance(string username, int amount)
        {
            waddchances[username] = amount;
            serializeWAddChances();
        }

        public static Boolean canBuy(string username, int amount)
        {
            if (coins.ContainsKey(username))
            {
                return coins[username] >= amount;
            } else
            {
                return false;
            }
        }

        //deduct reports by amount
        public static void spendCoins(string username, int amount)
        {
            if(canBuy(username, amount))
            {
                coins[username] -= amount;
            }
            serializeCoins();
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

        //private void populateUsernameDict()
        //{
        //    usernameDict.Add("Schafer#7273", new string[] { "Henry", "201292109884424193"});
        //    usernameDict.Add("Rexyrex#5838", new string[] { "Adrian", "308305348643782656" });
        //    usernameDict.Add("CPTOblivious#4652", new string[] { "Nick", "106222902575104000" });
        //    usernameDict.Add("Geffo#1689", new string[] { "Geoff", "200019349061369856" });
        //    usernameDict.Add("RayRay#4807", new string[] { "Ray", "310263594233364490" });
        //    usernameDict.Add("BonoboCop#0335", new string[] { "Xander", "237170530157854732" });
        //    usernameDict.Add("Wolfy#8611", new string[] { "Ryan", "244522891587223552" });
        //    usernameDict.Add("Ryanne#6203", new string[] { "Guki", "206378968935301120" });
        //    usernameDict.Add("Laura#7174", new string[] { "Laura", "277595678186930176" });
        //    usernameDict.Add("rooster212#7948", new string[] { "Jamie", "200016915538640896" });
        //    usernameDict.Add("Pash#8006", new string[] { "Pash", "231862514999099392" });
        //    usernameDict.Add("Andy", new string[] { "Andy", "0" });
        //    usernameDict.Add("Pink Socks#1146", new string[] { "Emily", "311668112007102464" });
        //    usernameDict.Add("RexBot#4568", new string[] { "RexBot", "309908194208251904" });
        //    usernameDict.Add("ratva#9894", new string[] { "ratva", "325759907099836427" });
        //    usernameDict.Add("RexBot 2.0#2358", new string[] { "RexBot2.0", "312739347361431562" });
        //}

        private static void populateUsernameDict(string filename)
        {
            usernameDict.Clear();
            string line;
            string inputFilePath = textPath + filename;
            try
            {
                FileStream fsr = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
                using (StreamReader sr = new StreamReader(fsr))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] res = line.Split('ㄱ');
                        usernameDict.Add(res[0], new string[] { res[1], res[2] });
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("populate error");
                Console.WriteLine(e.Message);
            }
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

        public static string getCoinTopList()
        {
            if (coins.Count == 0)
            {
                return "Noone has any money";
            }
            else
            {
                var top3 = coins.OrderByDescending(pair => pair.Value).Take(GlobalVars.STATS_SHOW);
                string res = string.Empty;
                foreach (KeyValuePair<string, int> kvp in top3)
                {
                    SocketUser su = _client.GetUser(ulong.Parse(DataUtils.usernameDict[kvp.Key][1]));
                    StatsUtils.updateMentionedUsers(su);
                    res += su.Mention + " : " + kvp.Value + "\n";
                }
                return res;
            }
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

        public static string getWaddChancesTopList()
        {
            if (waddchances.Count == 0)
            {
                return "Noone has a !w advantage";
            }
            else
            {
                var top3 = waddchances.OrderByDescending(pair => pair.Value).Take(GlobalVars.STATS_SHOW);
                string res = string.Empty;
                foreach (KeyValuePair<string, int> kvp in top3)
                {
                    SocketUser su = _client.GetUser(ulong.Parse(DataUtils.usernameDict[kvp.Key][1]));
                    StatsUtils.updateMentionedUsers(su);
                    res += su.Mention + " : " + kvp.Value + "\n";
                }
                return res;
            }
        }

        public static void incWAddChances(string name)
        {
            if (DataUtils.waddchances.ContainsKey(name))
            {
                DataUtils.waddchances[name]++;
            }
            else
            {
                DataUtils.waddchances[name] = 1;
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
                if (MasterUtils.ContainsAny(res, GlobalVars.REXDB_DISALLOWED_FUNCTIONS))
                {
                    res = "Guys I suck! hahaha... Just report me please haha!";
                }                
            }
            else
            {
                res = "You have nothing stored here";
            }
            return res;
        }

        public static void addUserToUserDict(string username, string actualname, string id)
        {



            using (StreamWriter sw = File.AppendText(textPath + "usernamedict.txt"))
            {
                sw.WriteLine(username + 'ㄱ' + actualname + 'ㄱ' + id);
            }
            repopulate();

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

            if (MasterUtils.ContainsAny(content, GlobalVars.REXDB_DISALLOWED_FUNCTIONS))
            {
                content = "Guys I suck! hahaha... Just report me please haha!";
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
