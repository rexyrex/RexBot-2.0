using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RexBot2.Utils;
using Discord;

namespace RexBot2.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("off")]
        [Remarks("admin")]
        [Alias("exit","quit")]
        [Summary("Repeats what you said")]
        public async Task offCmd()
        {
            EmbedBuilder emb = new EmbedBuilder();
            emb.Color = new Color(196, 09, 155);
            //emb.Title = "`HELP!`";
            emb.Timestamp = new DateTimeOffset(DateTime.Now);            

            if (MasterUtils.ContainsAny(Context.User.ToString(), GlobalVars.ADMINS)){
                emb.Description = "**I am going down for maintenance! brb...**";
                await Context.Channel.SendMessageAsync("",false,emb);
                System.Threading.Thread.Sleep(1000);
                System.Environment.Exit(1);
            } else
            {
                await Context.Channel.SendMessageAsync("Nice try " + MasterUtils.stripName(Context.User.ToString()));
            }            
        }

        [Command("purge")]
        [Remarks("admin")]
        [Summary("Delete Messages")]
        public async Task purgeCmd(int msgToDel = 1)
        {
            try
            {
                if (MasterUtils.ContainsAny(Context.User.ToString(), GlobalVars.ADMINS))
                {                    
                    var messages = await Context.Channel.GetMessagesAsync(((int)msgToDel + 1)).Flatten();
                    await Context.Channel.DeleteMessagesAsync(messages);
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Not implemented");
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }

        [Command("purgeid")]
        [Remarks("admin")]
        [Summary("Delete Message with id")]
        public async Task purgeidCmd(ulong id)
        {
            try
            {
                string userName = Context.User.ToString();
                if (MasterUtils.ContainsAny(Context.User.ToString(), GlobalVars.ADMINS))
                {
                    IMessage messages = await Context.Channel.GetMessageAsync(id);
                    await messages.DeleteAsync();
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Not implemented");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        [Command("restrain")]
        [Alias("r")]
        [Remarks("admin")]
        [Summary("Restrain this bellend")]
        public async Task restrainCmd(string username, double timeInSeconds = 60)
        {
            username = AliasUtils.getNameFromAlias(username);
            if(username != "null")
            {                
                if (MasterUtils.ContainsAny(Context.User.ToString(), GlobalVars.ADMINS))
                {
                    AdminUtils.addRestriction(username, timeInSeconds);
                    await Context.Channel.SendMessageAsync(username + " is restrained for " + timeInSeconds + "s!");
                }
                else
                {
                    await Context.Channel.SendMessageAsync("\"Get Fukt Idiot\" - Nickalodeon 2017");
                }
            } else
            {
                await Context.Channel.SendMessageAsync("Unregistered Username");
            }            
        }

        [Command("removerestrain")]
        [Alias("rr")]
        [Remarks("admin")]
        [Summary("Restrain this bellend")]
        public async Task RemoveRestrainCmd(string username)
        {
            username = AliasUtils.getNameFromAlias(username);
            if (username != "null")
            {
                string userName = Context.User.ToString();
                if (MasterUtils.ContainsAny(Context.User.ToString(), GlobalVars.ADMINS))
                {
                    AdminUtils.RemoveRestrain(username);
                    await Context.Channel.SendMessageAsync(username + " is no longer restrained!");
                }
                else
                {
                    await Context.Channel.SendMessageAsync("\"Get Fukt Idiot\" - Nickalodeon 2017");
                }
            } else
            {
                await Context.Channel.SendMessageAsync("Unregistered Username");
            }
        }

        [Command("restrainlist")]
        [Alias("rl")]
        [Remarks("admin")]
        [Summary("Show who is restrained")]
        public async Task restrainListCmd()
        {
            string res = AdminUtils.GetRestrainedList();
            await Context.Channel.SendMessageAsync("```" + res + "```");
        }

        [Command("mode")]
        [Remarks("admin")]
        [Summary("Change mode")]
        public async Task modeCmd(string reqMode="invalid"  )
        {
            if (MasterUtils.ContainsAny(Context.User.ToString(), GlobalVars.MODE_ADMINS))
            {
                if (MasterUtils.isMode(reqMode))
                {
                    DataUtils.changeMode(reqMode);
                    await Context.Channel.SendMessageAsync("RexBot mode changed to " + reqMode);
                }
                else if (reqMode == "help" || reqMode == "")
                {
                    await Context.Channel.SendMessageAsync(MasterUtils.getAllModesInfo());
                }
                else
                {
                    await Context.Channel.SendMessageAsync("***You input an invalid mode!***\n\n__Available modes__:\n" + MasterUtils.getAllModesInfo());
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("I don't listen to scrubs like you (v2)");
            }
        }
    }
}
