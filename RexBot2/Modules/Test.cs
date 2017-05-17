using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RexBot2.Utils;
using RexBot2.Timers;

namespace RexBot2.Modules
{
    public class Test : ModuleBase<SocketCommandContext>
    {
        private CommandService _commandService;
        public Test(CommandService commandService)
        {
            _commandService = commandService;
        }

        [Command("repeat")]
        [Remarks("test")]
        [Summary("Repeats what you said")]
        public async Task rpCmd(string input)
        {
            await Context.Channel.SendMessageAsync(input);
        }

        [Command("sw")]
        [Remarks("test")]
        [Summary("sw")]
        public async Task swCmd([Remainder] string input)
        {

            string res = await WebUtils.yodaOutput(input);

            await Context.Channel.SendMessageAsync(res);
        }

        [Command("yoda")]
        [Remarks("test")]
        [Summary("speak like yoda")]
        public async Task yodaCmd([Remainder] string input)
        {

            string res = await WebUtils.yodaOutput(input);

            await Context.Channel.SendMessageAsync(res);
        }

    }
}
