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

        [Command("activate")]
        [Remarks("admin")]
        [Summary("Activate rexbot so people can use its functions")]
        public async Task activateCmd()
        {
            EmbedBuilder emb = new EmbedBuilder();
            emb.Color = new Color(1, 255, 1);
            emb.Timestamp = new DateTimeOffset(DateTime.Now);

            if (MasterUtils.ContainsAny(Context.User.ToString(), GlobalVars.ADMINS))
            {
                MasterUtils.toggleActivation();
                emb.Description = "**Rexbot activation = "+ DataUtils.activation +"**";

                await Context.Channel.SendMessageAsync("", false, emb);
            }
            else
            {
                await Context.Channel.SendMessageAsync("Nice try " + MasterUtils.stripName(Context.User.ToString()));
            }
        }

        [Command("data")]
        [Remarks("admin")]
        [Alias("thed")]
        [Summary("Post usernamedict, rexdb, and responses as text files")]
        public async Task datapostCmd()
        {
            if (MasterUtils.ContainsAny(Context.User.ToString(), GlobalVars.ADMINS))
            {
                await Context.Channel.SendFileAsync("Data/texts/usernamedict.txt");
                await Context.Channel.SendFileAsync("Data/texts/rexdb.txt");
                await Context.Channel.SendFileAsync("Data/texts/responses.txt");
                await Context.Channel.SendFileAsync("Data/texts/alias2.txt");
            }
            else
            {
                await Context.Channel.SendMessageAsync("Nice try " + MasterUtils.stripName(Context.User.ToString()));
            }
        }

        [Command("adduser")]
        [Remarks("admin")]
        [Alias("addu")]
        [Summary("Adds user to usernamedict")]
        public async Task adduserCmd(string username, string actualname, string userid)
        {
            EmbedBuilder emb = new EmbedBuilder();
            emb.Color = new Color(196, 09, 155);
            //emb.Title = "`HELP!`";
            emb.Timestamp = new DateTimeOffset(DateTime.Now);

            if (MasterUtils.ContainsAny(Context.User.ToString(), GlobalVars.ADMINS))
            {
                emb.Description = "**User added!**";
                DataUtils.addUserToUserDict(username, actualname, userid);
                await Context.Channel.SendMessageAsync("", false, emb);
            }
            else
            {
                await Context.Channel.SendMessageAsync("Nice try " + MasterUtils.stripName(Context.User.ToString()));
            }
        }

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
                DataUtils.turnOffStatus();
                DataUtils.returnInvestedCoins();
                emb.Description = "**I am going down for maintenance! brb...**\n\nAll Invested Coins have been returned!";
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

        [Command("reload")]
        [Alias("reloaddata")]
        [Remarks("admin")]
        [Summary("Reload data after txt update")]
        public async Task reloadDataCmd()
        {
            if (MasterUtils.ContainsAny(Context.User.ToString(), GlobalVars.MODE_ADMINS))
            {
                DataUtils.repopulate();
                await Context.Channel.SendMessageAsync("Repopulation complete!");
            } else
            {
                await Context.Channel.SendMessageAsync("You must be a mode mod to use this command");
            }
            
        }

        [Command("mode")]
        [Remarks("admin")]
        [Summary("Change mode")]
        public async Task modeCmd(string reqMode="invalid")
        {
            if (MasterUtils.ContainsAny(Context.User.ToString(), GlobalVars.MODE_ADMINS))
            {
                if (MasterUtils.isMode(reqMode) && reqMode!="cat")
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
                    await Context.Channel.SendMessageAsync($"***You input an invalid mode!***\n\n```Current Mode is : {DataUtils.mode}```\n\n__Available modes__:\n\n" + MasterUtils.getAllModesInfo());
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("I don't listen to scrubs like you (v2)");
            }
        }
    }
}
