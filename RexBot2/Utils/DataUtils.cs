using System;
using System.Collections.Generic;
using System.Text;
using RexBot2.Objects;
using System.IO;
using Tweetinvi;
using System.Linq;
using RexBot2.Utils;

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

        public static List<string> trainPhrases1;
        public static List<string> trainPhrases2;
        public static List<string> trainPhrases3;

        public static string bingAuthStr = "";

        public static string textPath = "Data/texts/";

        public static Dictionary<string[], string> aliases;
        public static Dictionary<string, string[]> responses;
        public static Dictionary<string, RexMode> modes;



        public static Dictionary<string, Dictionary<string, string>> rexDB;

        public static Dictionary<string, int> reports;
        public static string[] games = { "Dota 2 : Cyka Expansion","Starcraft", "GTA W","InterStellaris", "The W Spam Game", "Maplestory","The Binding of John: Rebirth",
        "WODOTA","League of Losers","Feed Train Simulator"};

        //public static VideoSearch youtubeSearcher;

        public static string mode = "quiet";

        public static AudioService rexAS;

        public static Random rnd;

        public static Dictionary<string, string> helpEmojiBinds;

        public DataUtils()
        {
            InitVars();
        }

        private void InitVars()
        {
            Tweetinvi.Auth.SetUserCredentials("5JFjR7DXgDb4CQE0K1UwRO3Vt", "3i9ynzdJUhpxWkFoCXmuMjHIP19oxMbLlpcbdUUU6HhFMLI3f4", "1561997844-pZyWmSSAewcCVAV8u9IFK3j7iCPKZ7TQwXlfblO", "9faKrZDGS0FwEkJGKT3Xd90uqzVvSIattAuI5r7uVRdqI");
            var zuser = Tweetinvi.User.GetAuthenticatedUser();
            Console.WriteLine("Twitter initializing: " + zuser);

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

            trainPhrases1 = new List<string>();
            trainPhrases2 = new List<string>();
            trainPhrases3 = new List<string>();

            aliases = new Dictionary<string[], string>();
            responses = new Dictionary<string, string[]>();
            modes = new Dictionary<string, RexMode>();
            rexDB = new Dictionary<string, Dictionary<string, string>>();
            reports = new Dictionary<string, int>();
            helpEmojiBinds = new Dictionary<string, string>();
            

            modes.Add("quiet", new RexMode("quiet", "No auto triggers. No status updates. All functions online.", new string[] { "functions" }));
            modes.Add("active", new RexMode("active", "Occasional auto triggers.", new string[] { "functions", "trigger 30" }));
            modes.Add("loud", new RexMode("loud", "Many auto triggers. Status changes.", new string[] { "functions", "trigger 60", "status" }));
            modes.Add("tooloud", new RexMode("tooloud", "RexBot on Steroids", new string[] { "functions", "trigger 100", "status" }));
            modes.Add("cat", new RexMode("cat", "Posts a cat photo for every message, as well as a snarky comment", new string[] { "functions", "trigger 100", "status", "cat" }));

            Console.WriteLine("Starting population...");
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
            populateResponses();
            populatePicFileNames();
            populateHelpEmojiBinds();
            AliasUtils.ParseAliases();
            loadRexDB();
            Console.WriteLine("Done Loading!");
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
                var top3 = reports.OrderByDescending(pair => pair.Value).Take(3);
                string res = string.Empty;
                foreach(KeyValuePair<string,int> kvp in top3)
                {
                    res += kvp.Key + " - " + kvp.Value + "\n";
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

        private void populateResponses()
        {
            string line;
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

        private void populate(List<string> l, string filename)
        {
            string line;
            string inputFilePath = textPath + filename;
            try
            {
                FileStream fsr = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
                using (StreamReader sr = new StreamReader(fsr))
                {
                    while ((line = sr.ReadLine()) != null)
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
        public void populatePicFileNames()
        {
            DirectoryInfo d = new DirectoryInfo("Data/friendpics/");
            FileInfo[] Files = d.GetFiles("*.*");
            foreach (FileInfo file in Files)
            {
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
