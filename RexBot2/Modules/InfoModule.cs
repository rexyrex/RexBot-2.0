using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RexBot2.Utils;
using Discord;
using RexBot2.Timers;

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
            emb.Timestamp = new DateTimeOffset(DateTime.Now);
            EmbedFooterBuilder efb = new EmbedFooterBuilder();
            efb.Text = "RexBot 2.0";
            emb.Footer = efb;
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
                    res += "\n***-- " + s + " --***\n";
                    //res += "***----------***\n";
                    foreach (string stmp in cmdDict[s])
                    {
                        res += "!" + stmp + ", ";
                    }
                    res += "\n";
                }
                emb.Description = res;

                //emb.ImageUrl = "Data/pics/Kappahd.png";
                //EmbedAuthorBuilder embAuth = new EmbedAuthorBuilder();
                //embAuth.Name = "Rexyrex";
                //embAuth.Url = "www.google.com";
                //emb.Author = embAuth;
                //EmbedFieldBuilder efb1 = new EmbedFieldBuilder();
                //efb1.Name = "field1";
                //efb1.Value = DataUtils.adjList;
                //efb1.IsInline = true;
                //EmbedFieldBuilder efb2 = new EmbedFieldBuilder();
                //efb2.Name = "field2";
                //efb2.Value = "value 2";
                //efb2.IsInline = true;
                //EmbedFieldBuilder efb3 = new EmbedFieldBuilder();
                //efb3.Name = "field2";
                //efb3.Value = "value 2";
                //efb3.IsInline = true;
                //EmbedFieldBuilder efb4 = new EmbedFieldBuilder();
                //efb4.Name = "field2";
                //efb4.Value = "value 2";
                //efb4.IsInline = true;

                //emb.AddField(efb1);
                //emb.AddField(efb2);
                //emb.AddField(efb3);
                //emb.AddField(efb4);

            }
            else
            {
                
                foreach (CommandInfo c in _commandService.Commands)
                {
                    if (cmdName == c.Name)
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
                    }                    
                }
            }
            emb.Description = res;
            await Context.Channel.SendMessageAsync("", false, emb);

        }

        [Command("status")]
        [Remarks("info")]
        [Summary("Display bot status")]
        public async Task statusCmd()
        {
            DateTime dateTime = new DateTime(2017, 5, 13);
            EmbedBuilder emb = new EmbedBuilder();
            emb.Color = new Color(196, 09, 155);
            //emb.Title = "`HELP!`";
            emb.Timestamp = new DateTimeOffset(DateTime.Now);
            emb.Title = "**-- Status --**";

            EmbedFooterBuilder efb = new EmbedFooterBuilder();
            efb.Text = "RexBot 2.0";
            emb.Footer = efb;

            EmbedFieldBuilder efb1 = new EmbedFieldBuilder();
            efb1.Name = "Mode";
            efb1.Value = DataUtils.mode;
            efb1.IsInline = true;
            EmbedFieldBuilder efb2 = new EmbedFieldBuilder();
            efb2.Name = "Age";
            efb2.Value = Math.Round((DateTime.Now - dateTime).TotalDays, 2) + " days" + "\n";
            efb2.IsInline = true;
            EmbedFieldBuilder efb4 = new EmbedFieldBuilder();
            efb4.Name = "Channel";
            efb4.Value = Context.Channel.Name;
            efb4.IsInline = true;
            EmbedFieldBuilder efb5 = new EmbedFieldBuilder();
            efb5.Name = "UpTime";
            efb5.Value = RexTimers.getTime(RexTimers.systemRunClock);
            efb5.IsInline = true;
            EmbedFieldBuilder efb3 = new EmbedFieldBuilder();
            efb3.Name = "GitHub";
            efb3.Value = "https://github.com/rexyrex/RexBot-2.0";
            efb3.IsInline = false;

            emb.AddField(efb1);
            emb.AddField(efb2);
            emb.AddField(efb4);
            emb.AddField(efb5);
            emb.AddField(efb3);
            
            await Context.Channel.SendMessageAsync("",false,emb);
        }

        [Command("aka")]
        [Remarks("info")]
        [Summary("Get alias(es)")]
        public async Task akaCmd(string name = "empty")
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

        [Command("cooldowns")]
        [Alias("cds","cd")]
        [Remarks("info")]
        [Summary("Show Cooldowns")]
        public async Task cdCmd()
        {
            await Context.Channel.SendMessageAsync(RexTimers.getCds(Context.User.ToString()));
        }
    }
}
