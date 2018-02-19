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
        [Alias("s")]
        [Remarks("rexdb")]
        [Summary("save <id> <string> - Save line in rex db")]
        public async Task saveCmd(string id, [Remainder] string content)
        {
            DataUtils.writeToRexDB(Context.User.ToString(), id, content);
            DataUtils.serializeRexDB();
            await Context.Channel.SendMessageAsync("Save successful");
        }

        [Command("load")]
        [Alias("l")]
        [Remarks("rexdb")]
        [Summary("load <id> - Loads your string saved in <id>")]
        public async Task loadCmd(string id)
        {
            await Context.Channel.SendMessageAsync(DataUtils.getFromRexDB(Context.User.ToString(), id));
        }

        [Command("delete")]
        [Alias("del")]
        [Remarks("rexdb")]
        [Summary("del <id> - Deletes your string saved in <id>")]
        public async Task delCmd(string id)
        {
            string username = Context.User.ToString();

            if (!DataUtils.rexDB.ContainsKey(username))
            {
                await Context.Channel.SendMessageAsync("You have nothing saved in rexdb!");
                return;
            }

            if (!DataUtils.rexDB[username].ContainsKey(id))
            {
                await Context.Channel.SendMessageAsync("You have nothing saved under the id of " + id);
                return;
            }

            if (DataUtils.rexDB[username].ContainsKey(id))
            {
                DataUtils.rexDB[username].Remove(id);
                DataUtils.serializeRexDB();
                await Context.Channel.SendMessageAsync("You successfully deleted all contents saved under id: " + id);
                
                return;
            }

            await Context.Channel.SendMessageAsync("ERROR");
        }

        [Command("list")]
        [Remarks("rexdb")]
        [Summary("List the strings you saved")]
        public async Task listCmd()
        {
            string user = Context.User.ToString();
            await Context.Channel.SendMessageAsync("**Key Value Pairs for " + MasterUtils.stripName(user) + "...**\n" + DataUtils.listRexDB(user));
        }


    }
}
