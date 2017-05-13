using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RexBot2.Utils;

namespace RexBot2.Modules
{
    public class Test : ModuleBase<SocketCommandContext>
    {
        private CommandService _commandService;
        public Test(CommandService commandService)
        {
            _commandService = commandService;
        }

        [Command("test")]
        public async Task testCmd(string s = "zzz")
        {
            
            await Context.Channel.SendMessageAsync(AliasUtils.getAlias(s));
        }

        [Command("repeat")]
        [Summary("Repeats what you said")]
        public async Task rpCmd(string input)
        {
            await Context.Channel.SendMessageAsync(input);
        }



    }
}
