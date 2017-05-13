﻿using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RexBot2.Utils;

namespace RexBot2.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("off")]
        [Alias("exit","quit")]
        [Summary("Repeats what you said")]
        public async Task offCmd()
        {
            if(UtilMaster.ContainsAny(Context.User.ToString(), new string[] { "Rexyrex#5838" })){
                await Context.Channel.SendMessageAsync("I am going down for maintenance! brb...");
                System.Threading.Thread.Sleep(1000);
                System.Environment.Exit(1);
            } else
            {
                await Context.Channel.SendMessageAsync("Nice try " + UtilMaster.stripName(Context.User.ToString()));
            }            
        }

        [Command("purge")]
        [Summary("Delete Messages")]
        public async Task purgeCmd(int msgToDel = 2)
        {
            if (UtilMaster.ContainsAny(Context.User.ToString(), new string[] { "Rexyrex#5838" }))
            {
                await Context.Channel.SendMessageAsync("I am going down for maintenance! brb...");
                System.Threading.Thread.Sleep(1000);
                System.Environment.Exit(1);
            }
            else
            {
                await Context.Channel.SendMessageAsync("Nice try " + UtilMaster.stripName(Context.User.ToString()));
            }
        }

        [Command("mode")]
        [Summary("Change mode")]
        public async Task modeCmd(string reqMode="invalid"  )
        {
            if (UtilMaster.ContainsAny(Context.User.ToString(), new string[] { "Rexyrex#5838" }))
            {
                if (UtilMaster.isMode(reqMode))
                {
                    DataUtils.mode = reqMode;
                    await Context.Channel.SendMessageAsync("RexBot mode changed to " + reqMode);
                }
                else if (reqMode == "help" || reqMode == "")
                {
                    await Context.Channel.SendMessageAsync(UtilMaster.getAllModesInfo());
                }
                else
                {
                    await Context.Channel.SendMessageAsync("***You input an invalid mode!***\n\n*Available modes*:\n" + UtilMaster.getAllModesInfo());
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("I don't listen to scrubs like you (v2)");
            }
        }
    }
}
