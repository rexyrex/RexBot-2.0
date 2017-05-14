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
        public async Task swCmd()
        {

            string elapsedTime = RexTimers.getTime(RexTimers.systemRunClock);
            Console.WriteLine(elapsedTime);
            await Context.Channel.SendMessageAsync(elapsedTime);
        }



    }
}
