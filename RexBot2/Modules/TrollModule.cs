using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RexBot2.Utils;
using System.Linq;
using RexBot2.Timers;
using System.Diagnostics;

namespace RexBot2.Modules
{
    public class TrollModule : ModuleBase<SocketCommandContext>
    {
        private string picPath = "Data/pics/";

        [Command("eminem")]
        [Remarks("troll")]
        [Summary("Rap God")]
        public async Task eminemCmd()
        {
            await Context.Channel.SendFileAsync(picPath + "eminem.jpg");
            await Context.Channel.SendMessageAsync("PALMS SPAGHETTI KNEAS WEAK ARM SPAGHETTI THERES SPAGHETTI ON HIS SPAGHETTI ALREADY, MOMS SPAGHETTI", true);
        }

        [Command("w")]
        [Remarks("troll")]
        [Summary("A chance to be annoying")]
        public async Task wCmd()
        {
            string username = Context.User.ToString();
            if (RexTimers.canRunCmd(username, "w"))
            {
                int randInt = DataUtils.rnd.Next(1, 11);
                int randInt2 = DataUtils.rnd.Next(1, 11);
                string res = MasterUtils.stripName(username) + " rolled " + randInt + " when s/he should have rolled " + randInt2;
                res += "\nNo W's for you today " + MasterUtils.stripName(Context.User.ToString()) + "!";
                if (randInt != randInt2)
                {
                    await Context.Channel.SendMessageAsync(res);
                }
                else
                {                    
                    await Context.Channel.SendMessageAsync("W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W, W", true);
                }
                RexTimers.resetTimer(username, "w");
            } else
            {
                await Context.Channel.SendMessageAsync(RexTimers.getWaitMsg(username, "w"));
            }
        }

        [Command("report")]
        [Remarks("troll")]
        [Summary("report a fool")]
        public async Task reportCmd([Remainder] string name)
        {
            string username = Context.User.ToString();
            if (RexTimers.canRunCmd(username, "report"))
            {
                if (AliasUtils.getAliasKey(name).Contains("None"))
                {
                    await Context.Channel.SendMessageAsync("You're trying to report an unregistered user!");
                }
                else
                {
                    name = DataUtils.aliases[AliasUtils.getAliasKey(name)];
                    if (DataUtils.reports.ContainsKey(name))
                    {
                        DataUtils.reports[name]++;
                    }
                    else
                    {
                        DataUtils.reports[name] = 1;
                    }
                    await Context.Channel.SendMessageAsync("Report successful");
                    RexTimers.resetTimer(username, "report");
                }
            } else
            {
                await Context.Channel.SendMessageAsync(RexTimers.getWaitMsg(username,"report"));
            }
        }

        [Command("reports")]
        [Remarks("troll")]
        [Summary("show all reports")]
        public async Task reportsCmd()
        {
            string res = string.Empty;
            foreach (KeyValuePair<string, int> kv in DataUtils.reports)
            {
                res += "User " + kv.Key + ", reported " + kv.Value + " times!\n";
            }
            if (res == string.Empty)
            {
                res += "Nobody has been reported! YET...";
            }

            await Context.Channel.SendMessageAsync(res);
        }

    }
}
