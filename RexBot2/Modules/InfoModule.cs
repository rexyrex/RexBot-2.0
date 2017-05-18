using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RexBot2.Utils;
using Discord;
using RexBot2.Timers;
using System.Linq;

namespace RexBot2.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        private CommandService _commandService;
        private Dictionary<string, List<string>> cmdDict;
        public InfoModule(CommandService commandService)
        {
            _commandService = commandService;
            cmdDict = new Dictionary<string, List<string>>();
        }

        [Command("help")]
        [Remarks("info")]
        [Summary("Display info about command(s)")]
        public async Task helpCmd(string cmdName = "null")
        {
            EmbedBuilder emb = new EmbedBuilder();
            emb.Color = new Color(196, 09, 155);
            EmbedFooterBuilder efb = new EmbedFooterBuilder();
            
            string res = string.Empty;

            if (cmdName == "null")
            {
                foreach (CommandInfo c in _commandService.Commands)
                {
                    if (cmdDict.ContainsKey(c.Remarks))
                    {
                        cmdDict[c.Remarks].Add(c.Name);
                    }
                    else
                    {
                        List<string> tmpList = new List<string>();
                        tmpList.Add(c.Name);
                        cmdDict[c.Remarks] = tmpList;
                    }
                    //res += $"***{c.Name}*** : {c.Summary}\n";
                }
                foreach (string s in cmdDict.Keys)
                {
                    res += "\n**" + DataUtils.getHelpEmojiBind(s) + "  **";
                    //res += "***----------***\n";
                    foreach (string stmp in cmdDict[s])
                    {
                        res += "`!" + stmp + "`, ";
                    }
                    res += "\n";
                }
                emb.Description = res;
                efb.Text = "Type \"!help <command>\" for more info";
                efb.IconUrl = "http://clipartall.com/subimg/get-help-clipart-help-clip-art-300_300.png";
                emb.Footer = efb;
                //emb.ImageUrl = "https://blogs-images.forbes.com/markhughes/files/2016/01/Terminator-2-1200x873.jpg?width=960";
                //emb.ThumbnailUrl = "http://clipartall.com/subimg/get-help-clipart-help-clip-art-300_300.png";


            }
            else if (cmdName.Trim('!') == "meme")
            {
                res = MasterUtils.getMemeHelp();
            } else // SIngle command info
            {
                
                foreach (CommandInfo c in _commandService.Commands)
                {
                    string[] ali = new string[c.Aliases.Count];
                    int count = 0;
                    foreach(string alia in c.Aliases)
                    {
                        ali[count] = alia;
                        count++;
                    }

                    if (cmdName.Trim('!') == c.Name || (c.Aliases.Count>0 && Array.IndexOf(ali,cmdName.Trim('!')) > -1))
                    {
                        string aliasesStr = string.Empty;
                        string parametersStr = string.Empty;
                        foreach (string s in c.Aliases)
                        {
                            aliasesStr += s + ", ";
                        }
                        foreach (ParameterInfo x in c.Parameters)
                        {
                            parametersStr += "(" + x.Name + ", " + x.Type + ")";
                        }
                        res = "**Command :** " + c.Name;
                        res += "\n**Aliases:** " + aliasesStr;
                        res += "\n**Category:** " + c.Remarks;
                        res += "\n**Description:** " + c.Summary;
                        res += "\n**Parameters:** " + parametersStr;
                        //string imgurl = await WebUtils.getImgurUrl(c.Remarks);
                        //emb.ImageUrl = imgurl;
                    }                    
                }
            }
            emb.Description = res;
            await Context.Channel.SendMessageAsync("", false, emb);
        }

        [Command("stats")]
        [Alias("stat")]
        [Remarks("info")]
        [Summary("Display bot stats")]
        public async Task statsCmd()
        {
            DateTime dateTime = new DateTime(2017, 5, 13);
            EmbedBuilder emb = new EmbedBuilder();
            emb.Color = new Color(0, 255, 0);
            //emb.ThumbnailUrl = "http://pngimages.net/sites/default/files/bar-chart-png-image-892.png";            

            emb.Timestamp = new DateTimeOffset(DateTime.Now);
            emb.Title = "**📈 Statistics 📉**\n";
            //emb.Url = "https://www.youtube.com/watch?v=4YpTLy6dn5c";
            emb.Description = "[Displaying the stats of this session!](https://en.wikipedia.org/wiki/Statistics \"I hate ryan\")\n\n"+
                "**UpTime** : "+ RexTimers.getTime(RexTimers.systemRunClock) + "\n\n" +
                "**Commands Run** : " + Stats.CommandsRun + "\n" +
                "**Commands Per Minute** : " + Math.Round((double)(Stats.CommandsRun/RexTimers.systemRunClock.Elapsed.TotalMinutes),2) + "\n\n" +
                "**Reactions** : " + Stats.ReactionCount + "\n" +
                "**Reactions Per Minute** : " + Math.Round((double)(Stats.ReactionCount / RexTimers.systemRunClock.Elapsed.TotalMinutes), 2) + "\n\n" +
                "**Messages Received** : " + Stats.MessagesRecieved+ "\n" +
                "**Messages Edited** : " + Stats.MsgEditCount + "\n" +
                "**Messages Deleted** : " + Stats.MsgDeleteCount + "\n\n" +
                "__🥇Leaderboards🥇__";

            EmbedFieldBuilder topReportsField = new EmbedFieldBuilder();
            topReportsField.Name = "Reported";
            topReportsField.Value = DataUtils.getReportTopList();
            topReportsField.IsInline = true;

            EmbedFieldBuilder commandsRunField = new EmbedFieldBuilder();
            commandsRunField.Name = "Commands Run";
            commandsRunField.Value = Stats.CommandsRun;
            commandsRunField.IsInline = true;
            EmbedFieldBuilder mostUsedCommandsField = new EmbedFieldBuilder();
            mostUsedCommandsField.Name = "Commands";
            mostUsedCommandsField.Value = Stats.getTop3Commands();
            mostUsedCommandsField.IsInline = true;

            EmbedFieldBuilder mostMsgUserField = new EmbedFieldBuilder();
            mostMsgUserField.Name = "Messages";
            mostMsgUserField.Value = Stats.getTop3Messagers();
            mostMsgUserField.IsInline = true;
            EmbedFieldBuilder messagesSinceLogin = new EmbedFieldBuilder();
            messagesSinceLogin.Name = "Messages Received";
            messagesSinceLogin.Value = Stats.MessagesRecieved;
            messagesSinceLogin.IsInline = true;
            EmbedFieldBuilder upTimeField = new EmbedFieldBuilder();
            upTimeField.Name = "UpTime";
            upTimeField.Value = RexTimers.getTime(RexTimers.systemRunClock);
            upTimeField.IsInline = false;

            EmbedFieldBuilder newLineField = new EmbedFieldBuilder();
            newLineField.Name = " ";
            newLineField.Value = " ";
            upTimeField.IsInline = false;

            //emb.AddField(upTimeField);

            emb.AddField(topReportsField);

            //emb.AddField(commandsRunField);
           
            //emb.AddField(newLineField);
            //emb.AddField(messagesSinceLogin);            
            emb.AddField(mostMsgUserField);
            emb.AddField(mostUsedCommandsField);


            await Context.Channel.SendMessageAsync("", false, emb);
        }

        [Command("status",RunMode = RunMode.Async)]
        [Alias("info")]
        [Remarks("info")]
        [Summary("Display bot status")]
        public async Task statusCmd()
        {
            DateTime dateTime = new DateTime(2017, 5, 13);
            EmbedBuilder emb = new EmbedBuilder();
            emb.Color = new Color(196, 09, 155);
            emb.ThumbnailUrl = "http://silhouettesfree.com/machines/robots/robot-silhouette-image.png";

            emb.Title = "**🤜 RexBot 2.0 by Rexyrex 🤛**\n";

            try
            {
                string joke = await WebUtils.getOneLiner();
                emb.Description = "\"" + joke + "\"";
            } catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            emb.Description = "**➺Github** : [RexBot 2.0](https://github.com/rexyrex/RexBot-2.0 \"ALL\")\n"
                + "**➺Geoff DB** : [Google Docs](https://docs.google.com/spreadsheets/d/1EeJpyo7Rvh-WxcYoKB2qHzBR3L_0xvR9fW1COqGhljI/edit#gid=2091390326 \"ABOARD\")\n"
                + "**➺Support Rexyrex** : [Newgrounds](http://rexyrex.newgrounds.com/audio/ \"THE FEED TRAIN\")"
                + ", [Youtube](https://www.youtube.com/channel/UCq3yY-SCoglG8xm6Z1_udaw \"CHOO CHOO\")";
            EmbedFieldBuilder modeField = new EmbedFieldBuilder();
            modeField.Name = "Mode";
            modeField.Value = DataUtils.mode;
            modeField.IsInline = true;
            EmbedFieldBuilder ageField = new EmbedFieldBuilder();
            ageField.Name = "Age";
            ageField.Value = Math.Round((DateTime.Now - dateTime).TotalDays, 2) + " days";
            ageField.IsInline = true;
            EmbedFieldBuilder upTimeField = new EmbedFieldBuilder();
            upTimeField.Name = "UpTime";
            upTimeField.Value = RexTimers.getTime(RexTimers.systemRunClock);
            upTimeField.IsInline = true;
            string cmdCountStr = Stats.getCommandCount(_commandService).ToString();
            EmbedFieldBuilder cmdCountField = new EmbedFieldBuilder();
            cmdCountField.Name = "Command Count";
            cmdCountField.Value = cmdCountStr;
            cmdCountField.IsInline = true;
            //EmbedFieldBuilder cmdRunField = new EmbedFieldBuilder();
            //cmdRunField.Name = "Commands Run";
            //cmdRunField.Value = Stats.CommandsRun;
            //cmdRunField.IsInline = true;

            EmbedFieldBuilder efb6 = new EmbedFieldBuilder();
            efb6.Name = "❤️ Special Thanks To ❤️";
            efb6.Value = "Geoff - DB\nEm - Utils\nNick - W Testing";
            efb6.IsInline = false;

            emb.AddField(modeField);
            emb.AddField(cmdCountField);
            emb.AddField(ageField);
            emb.AddField(upTimeField);
            
            emb.AddField(efb6);
            await Context.Channel.SendMessageAsync("",false,emb);
        }

        [Command("aka")]
        [Alias("alias", "aliases")]
        [Remarks("info")]
        [Summary("Get alias(es)")]
        public async Task akaCmd([Remainder] string name = "empty")
        {
            if (name == "empty")
            {
                await Context.Channel.SendMessageAsync(AliasUtils.getAliases());
            } else
            {
                EmbedBuilder emb = new EmbedBuilder();
                emb.Color = new Color(196, 09, 155);
                //emb.Title = "`HELP!`";
                //emb.Timestamp = new DateTimeOffset(DateTime.Now);
                emb.Description = "**Aliases for " + name + "**\n" + AliasUtils.getAlias(name);

                EmbedFooterBuilder efb = new EmbedFooterBuilder();
                efb.Text = "Powered by GeoffDB";
                emb.Footer = efb;
                await Context.Channel.SendMessageAsync("", false, emb);
            }            
        }

        [Command("patchnotes")]
        [Alias("pn", "patch", "patchhistaory","version")]
        [Remarks("info")]
        [Summary("Show the most recent updates to Rexbot 2.0")]
        public async Task patchNotesCmd()
        {
            await Context.Channel.SendMessageAsync("```" + DataUtils.getRawStringFromFile("Data/texts/patchnotes.txt")+ "```");
        }

        [Command("cooldowns")]
        [Alias("cds","cd")]
        [Remarks("info")]
        [Summary("Show Cooldowns")]
        public async Task cdCmd()
        {
            await Context.Channel.SendMessageAsync(RexTimers.getCds(Context.User.ToString()));
        }

        [Command("meme")]
        [Remarks("meme builder")]
        [Summary("!meme (<type>) (<toptext>) (<bottetxt>), type \"!meme help\" to get list of meme types")]
        public async Task memeCmd([Remainder] string stz)
        {
            Console.WriteLine("in here");
            string res = string.Empty;
            if (MasterUtils.ContainsAny(stz, new string[] {"help","list","show" }))
            {
                res = MasterUtils.getMemeHelp();
                await Context.Channel.SendMessageAsync(res);
            } else if(stz.Count(x => x == '(') == 3)
            {
                Console.WriteLine("in the elseif");
                int bracketCount = 0;
                string type = string.Empty;
                string topText = string.Empty;
                string botText = string.Empty;

                for (int i = 0; i < stz.Length; i++)
                {
                    if (stz[i] == '(' || stz[i] == ')')
                    {
                        bracketCount++;
                    }
                    if (bracketCount == 3)
                    {
                        if (stz[i] != '(')
                            topText += stz[i];
                    }
                    if (bracketCount == 5)
                    {
                        if (stz[i] != '(')
                            botText += stz[i];
                    }
                    if (bracketCount == 1)
                    {
                        if (stz[i] != ')' && stz[i] != '(')
                            type += stz[i];
                    }
                }
                topText = MasterUtils.processTextForMeme(topText);
                botText = MasterUtils.processTextForMeme(botText);
                Console.WriteLine("printing");
                await Context.Channel.SendMessageAsync("https://memegen.link/" + type + "/" + topText + "/" + botText + ".jpg");
            } else
            {
                await Context.Channel.SendMessageAsync("Invalid argument structure. Type \"!meme help\" for more info");
            }
        }
    }
}
