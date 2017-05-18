using Discord.Commands;
using RexBot2.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace RexBot2.Modules
{
    public class TextModule : ModuleBase<SocketCommandContext>
    {
        [Command("play")]
        [Remarks("text")]
        [Summary("(Dota2) Tells you what hero and role you should play")]
        public async Task playCmd([Remainder] string player = "You")
        {
            if(player == "You")
            {
                player = MasterUtils.stripName(Context.User.ToString());
            }
            await Context.Channel.SendMessageAsync($"{player} should play {MasterUtils.getWord(DataUtils.positionList)} {MasterUtils.getWord(DataUtils.heroList)}");
        }

        [Command("hero")]
        [Remarks("text")]
        [Summary("(Dota2) Display a random hero")]
        public async Task heroCmd()
        {
            await Context.Channel.SendMessageAsync(MasterUtils.getWord(DataUtils.heroList));
        }

        [Command("roll")]
        [Remarks("text")]
        [Summary("roll <min> <max> (default 0-100)")]
        public async Task rollCmd(int n1=0, int n2=100)
        {
            string result = string.Empty;
            result += MasterUtils.stripName(Context.User.ToString()) + " rolled ";
            int num;
            if(n1!=0 && n2 == 101)
            {//1arg input
                num = DataUtils.rnd.Next(0, n1+1);
            } else
            {
                num = DataUtils.rnd.Next(n1, n2+1);
            }
            await Context.Channel.SendMessageAsync(result + num.ToString());
        }

        [Command("flip")]
        [Remarks("text")]
        [Summary("flip a dino coin")]
        public async Task flipCmd()
        {
            int rand = DataUtils.rnd.Next(0, 2);
            string userName = MasterUtils.stripName(Context.User.ToString());
            if (rand == 1)
            {
                await Context.Channel.SendMessageAsync(userName + " flipped heads!");
            }
            else
            {
                await Context.Channel.SendMessageAsync(userName + " flipped tails!");
            }
        }

        [Command("calc")]
        [Remarks("text")]
        [Summary("Calculate the given math equation")]
        public async Task calcCmd(string eq)
        {
            //Expression e;
            await Context.Channel.SendMessageAsync(DotNetStandardCalculator.StandardCalculator.CalculateFromString(eq).ToString());            
        }        
    }
}
