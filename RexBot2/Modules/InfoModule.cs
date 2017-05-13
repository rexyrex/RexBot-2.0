using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RexBot2.Utils;

namespace RexBot2.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        private CommandService _commandService;
        public InfoModule(CommandService commandService)
        {
            _commandService = commandService;
        }

        [Command("help")]
        [Summary("Display info about command(s)")]
        public async Task helpCmd()
        {
            string res = string.Empty;
            foreach (CommandInfo c in _commandService.Commands)
            {
                res += $"{c.Name} : {c.Summary}\n";
            }
            await Context.Channel.SendMessageAsync(res);
        }

        [Command("status")]
        [Summary("Display bot status")]
        public async Task statusCmd()
        {

            await Context.Channel.SendMessageAsync("coming soon");
        }

        [Command("aka")]
        [Summary("Get alias(es)")]
        public async Task akaCmd(string name = "empty")
        {
            if (name == "empty")
            {
                await Context.Channel.SendMessageAsync(AliasUtils.getAliases());
            } else
            {
                await Context.Channel.SendMessageAsync(AliasUtils.getAlias(name));
            }
            
        }
    }
}
