using Discord.Commands;
using System.Threading.Tasks;
using RexBot2.Utils;
using Discord.WebSocket;
using System;
using Discord;

namespace RexBot2.Modules
{
    public class Test : ModuleBase<SocketCommandContext>
    {
        private CommandService _commandService;
        //private DiscordSocketClient adsc;
        public Test(CommandService commandService)//, DiscordSocketClient dsc)
        {
            _commandService = commandService;

            //adsc = dsc;

        }

        [Command("repeat")]
        [Remarks("test")]
        [Summary("Repeats what you said")]
        public async Task rpCmd(string input)
        {
            await Context.Message.AddReactionAsync(new Emoji("👎"));
            //await Context.Channel.SendMessageAsync(input);
        }

        [Command("sw")]
        [Remarks("test")]
        [Summary("sw")]
        public async Task swCmd([Remainder] string input)
        {
            string res = await WebUtils.yodaOutput(input);
            await Context.Channel.SendMessageAsync(res);
        }

        [Command("test")]
        [Remarks("test")]
        [Summary("Temporary function which should not be invoked by anyone else than Rexyrex")]
        public async Task dvtstCmd()
        {
            await Context.Channel.SendMessageAsync("NOT IMPLEMENTED MADAFAKA");
        }

        [Command("random")]
        [Alias("rand")]
        [Remarks("test")]
        [Summary("Invoke a completely random function")]
        public async Task randCmd()
        {
            int rand = DataUtils.rnd.Next(0, 3);
            string randcmd = "!";

            switch (rand)
            {
                case 0: randcmd += DataUtils.getPossibleCommand(1, "longsearch");  break;
                case 1: randcmd += DataUtils.getPossibleCommand(1, "shortsearch"); break;
                case 2: randcmd += DataUtils.getPossibleCommand(0); break;
                default: randcmd += "restrain " + Context.User.ToString();  break;
            }
            await Context.Channel.SendMessageAsync(randcmd);
        }
    }
}
