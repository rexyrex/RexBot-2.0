using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RexBot2.Modules
{
    public class Test : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        public async Task testCmd()
        {
            await Context.Channel.SendMessageAsync("success!");
        }
    }
}
