using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RexBot2.Utils;

namespace RexBot2.Modules
{
    public class TrollModule : ModuleBase<SocketCommandContext>
    {
        private string picPath = "Data/pics/";

        [Command("eminem")]
        [Summary("Rap God")]
        public async Task eminemCmd()
        {
            await Context.Channel.SendFileAsync(picPath + "eminem.jpg");
            await Context.Channel.SendMessageAsync("PALMS SPAGHETTI KNEAS WEAK ARM SPAGHETTI THERES SPAGHETTI ON HIS SPAGHETTI ALREADY, MOMS SPAGHETTI", true);
        }

        [Command("w")]
        [Summary("A chance to be annoying")]
        public async Task wCmd()
        {
            int randInt = DataUtils.rnd.Next(1, 11);
            int randInt2 = DataUtils.rnd.Next(1, 11);
            string res = "you rolled " + randInt + " when you should have rolled " + randInt2;
            res += "\nNo W's for you today " + UtilMaster.stripName(Context.User.ToString()) + "!";
            if(randInt != randInt2)
            {
                await Context.Channel.SendMessageAsync(res);
            } else
            {
                await Context.Channel.SendMessageAsync("W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W W, W", true);
            }
            
        }
    }
}
