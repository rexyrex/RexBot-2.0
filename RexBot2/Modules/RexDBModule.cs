using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RexBot2.Utils;

namespace RexBot2.Modules
{
    public class RexDBModule : ModuleBase<SocketCommandContext>
    {
        [Command("save")]
        [Summary("save <id> <string> - Save line in rex db")]
        public async Task saveCmd(string id, string content)
        {
            DataUtils.writeToRexDB(Context.User.ToString(), id, content);
            await Context.Channel.SendMessageAsync("Save successful");
        }

        [Command("load")]
        [Summary("load <id> - Loads your string saved in <id>")]
        public async Task loadCmd(string id)
        {
            await Context.Channel.SendMessageAsync(DataUtils.getFromRexDB(Context.User.ToString(), id));
        }

        [Command("list")]
        [Summary("List the strings you saved")]
        public async Task listCmd()
        {
            string user = Context.User.ToString();
            await Context.Channel.SendMessageAsync("**Key Value Pairs for " + UtilMaster.stripName(user) + "...**\n" + DataUtils.listRexDB(user));
        }


    }
}
