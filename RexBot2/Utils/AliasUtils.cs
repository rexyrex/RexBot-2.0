using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace RexBot2.Utils
{
    public class AliasUtils
    {
        public static string getAliases()
        {
            string res = string.Empty;
            res += "Aliases...\n";
            foreach (KeyValuePair<string[], string> entry in DataUtils.aliases)
            {
                res += entry.Value.ToString() + " : ";
                for (int i = 0; i < entry.Key.Length; i++)
                {
                    res += '`' + entry.Key[i].ToString() + '`';
                    if (i < entry.Key.Length - 1)
                    {
                        res += ", ";
                    }
                }
                res += "\n";
            }
            res += "";
            if (res.Length > 2000)
            {
                res = "Too many aliases to display at once";
            }
            return res;
        }

        public static string getAlias(string username)
        {
            string res = string.Empty;

            if (getAliasKey(username).Contains("None"))
            {
                return "NOT A USER!!";
            }
            else
            {
                res += DataUtils.aliases[getAliasKey(username)] + " : ";
                foreach (string als in getAliasKey(username))
                {
                    res += als + "\n";
                }
            }

            res += "";
            if (res.Length > 2000)
            {
                res = "Too many aliases to display at once";
            }
            return res;
        }

        public static string[] getAliasKey(string username)
        {
            foreach (KeyValuePair<string[], string> entry in DataUtils.aliases)
            {
                if (entry.Value.ToString().ToLower() == username.ToLower())
                {
                    return entry.Key;
                }
                foreach (string ent in entry.Key)
                {
                    if (ent.ToLower() == username.ToLower())
                    {
                        return entry.Key;
                    }
                }
            }
            return new string[] { "None" };
        }

        public static string getNameFromAlias(string alias)
        {
            if (isUserInDB(alias))
            {
                return DataUtils.aliases[getAliasKey(alias)];
            } else
            {
                return "null";
            }
        }

        public static bool isUserInDB(string username)
        {
            if (getAliasKey(username).Contains("None"))
            {
                return false;
            }
            return true;
        }

        public static void ParseAliases()
        {
            string line;

            string res = string.Empty;
            try
            {
                FileStream fsr = new FileStream(DataUtils.textPath + "alias2.txt", FileMode.Open, FileAccess.Read);
                using (StreamReader sr = new StreamReader(fsr))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] initSplit = line.Split('\t');
                        string name = initSplit[0];
                        string[] aliasesString = new string[initSplit.Length - 1];
                        for (int i = 0; i < initSplit.Length - 1; i++)
                        {
                            aliasesString[i] = initSplit[i + 1];
                        }
                        DataUtils.aliases.Add(aliasesString, name);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("alias parse error");
                Console.WriteLine(e.Message);
            }
        }
    }
}
