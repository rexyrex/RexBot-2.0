﻿using Discord.WebSocket;
using System;
using System.Collections.Generic;
using Discord.Commands;
using System.Reflection;
using System.Threading.Tasks;
using RexBot2.Utils;
using System.Linq;
using Newtonsoft.Json;
using Discord;
using RexBot2.Timers;
using Discord.Rest;
using System.Text;
using System.Diagnostics;

namespace RexBot2
{
    public class MasterHandler
    {
        private DiscordSocketClient _client;

        private CommandService _service;
        private IReadOnlyCollection<SocketGuildUser> userCollection;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            
            _service = new CommandService(new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Error,
                DefaultRunMode = RunMode.Async,
            });
            await _service.AddModulesAsync(Assembly.GetEntryAssembly());

            

            _client.MessageReceived += HandleCommandAsync;
            //_client.UserUpdated += _client_UserUpdated; -> use guild member updated instead
            _client.MessageDeleted += HandleMsgDel;
            _client.MessageUpdated += MessageUpdated;
            _client.ReactionAdded += ReactionUpdated;
            _client.Disconnected += _client_Disconnected;
            _client.Connected += _client_Connected;
            _client.Ready += _client_Ready;
            //_client.ChannelUpdated += _client_ChannelUpdated;
            _client.Log += _client_Log;
            //_client.UserVoiceStateUpdated += _client_UserVoiceStateUpdated;
            //_client.CurrentUserUpdated += _client_CurrentUserUpdated;
            _client.GuildMemberUpdated += _client_GuildMemberUpdated;
            //Console.WriteLine("latency:" + _client.Latency);
        }

        private async Task _client_GuildMemberUpdated(SocketGuildUser beforeUser, SocketGuildUser afterUser)
        {
            if(beforeUser.Status == UserStatus.Offline 
                && afterUser.Status != UserStatus.Offline)
            {
                if((afterUser.Id == 237170530157854732||afterUser.Id == 310828842145284096))
                {
                    DataUtils.changeMode("xander");
                    ISocketMessageChannel sc = _client.GetGuild(GlobalVars.GUILD_ID).GetChannel(GlobalVars.CHANNEL_ID) as ISocketMessageChannel;
                    await sc.SendMessageAsync("Welcome back Xander! Mode set to Xander Mode!\nNote : You can always change mode with the **!mode** command :)");
                }

                if ((afterUser.Id == 308305348643782656))
                {
                    ISocketMessageChannel sc = _client.GetGuild(GlobalVars.PRIVATE_GUILD_ID).GetChannel(GlobalVars.PRIVATE_CHANNEL_ID) as ISocketMessageChannel;
                    await sc.SendMessageAsync("WB Adrian");
                }
            }
            if (afterUser.Status == UserStatus.Invisible)
            {
                ISocketMessageChannel sc = _client.GetGuild(GlobalVars.PRIVATE_GUILD_ID).GetChannel(GlobalVars.PRIVATE_CHANNEL_ID) as ISocketMessageChannel;
                await sc.SendMessageAsync("You can't hide from me!");
            }
            if (afterUser.Status == UserStatus.Offline)
            {
                ISocketMessageChannel sc = _client.GetGuild(GlobalVars.PRIVATE_GUILD_ID).GetChannel(GlobalVars.PRIVATE_CHANNEL_ID) as ISocketMessageChannel;
                await sc.SendMessageAsync("You went offline!");
            }

        }

        private async Task _client_CurrentUserUpdated(SocketSelfUser arg1, SocketSelfUser arg2)
        {
            Console.WriteLine("who is currnet user");
        }

        private async Task _client_ChannelUpdated(SocketChannel arg1, SocketChannel arg2)
        {
            Console.WriteLine("Channel Update call");
        }

        private async Task _client_UserUpdated(SocketUser arg1, SocketUser arg2)
        {
            Console.WriteLine("user update doesnt work... if ur seeing this... it means they fixed it");
        }

        private async Task _client_UserVoiceStateUpdated(SocketUser arg1, SocketVoiceState arg2, SocketVoiceState arg3)
        {
            //Console.WriteLine("voice state updated");
        }

        private async Task _client_Ready()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Logger.Log(LogSeverity.Info, "Rex CMD Handler", "Client Ready");
            Logger.Log(LogSeverity.Info, "Rex CMD Handler", "Initializing DataUtils...");
            new DataUtils(_client,_service);
            new EmojiUtils();
            await WebUtils.updateBingAuthToken();

            DataUtils.changeMode(GlobalVars.DEFAULT_MODE);
            try
            {
                //populate users from given guild ID
                userCollection = _client.GetGuild(GlobalVars.GUILD_ID).Users;
                new RexTimers(_client, userCollection);
                new AdminUtils();
            } catch (Exception e)
            {
                Console.WriteLine("YOU NEED TO JOIN FEED TRAIN GUILD FIRST!!! OR CHANGE GUILD ID IN GLOBAL VARS... Exception : " + e.ToString());
            }
            sw.Stop();
            Logger.Log(LogSeverity.Info, "Rex CMD Handler", "Time to get Ready : " + sw.Elapsed.TotalSeconds.ToString("F2") + " seconds");
        }

        private async Task _client_Log(LogMessage arg)
        {
            Logger.Log(arg.Severity,arg.Source,arg.Message);
        }

        private async Task _client_Connected()
        {
            Logger.Log(LogSeverity.Info, "Rex CMD Handler", "Can Confirm connected! YAY!");
        }

        private async Task _client_Disconnected(Exception arg)
        {
            DataUtils.turnOffStatus();
            await ErrorHandler.HandleLog(new LogMessage(LogSeverity.Error, "Disconnect Detected", "NOOOO", arg));
        }

        private async Task ReactionUpdated(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            StatsUtils.ReactionCount++;
            //IUserMessage msg = await cache.GetOrDownloadAsync();
            //await msg.ModifyAsync(x =>
            //{
            //    x.Content = "herro";
            //}                
            //);
            //await msg.AddReactionAsync(reaction.Emote);
            //await channel.SendMessageAsync("REACTED");
            //await channel.SendMessageAsync("I also want to react");
        }

        private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            StatsUtils.MsgEditCount++;
            //var message = await before.GetOrDownloadAsync();
            //string author = message.Author.ToString();
            //if(!message.Author.IsBot)// && message.Embeds == null)
            //    await channel.SendMessageAsync($"{author} changed his/her message from ```{message}``` to ```{after}```");
        }

        private async Task HandleMsgDel(Cacheable <Discord.IMessage, ulong> c, ISocketMessageChannel smc)
        {
            StatsUtils.MsgDeleteCount++;
            //IMessage msg = await c.GetOrDownloadAsync();
            //IUser user = msg.Author;
            //string time = msg.CreatedAt.ToString();
            //string content = msg.Content.ToString();
            //string res = string.Empty;
            //res += "**Someone deleted a message!**\n";
            //res += "```\n";
            //res += "Message Author: " + user.ToString() +"\n";
            //res += "Message Timestamp: " + time + "\n";
            //res += "Message Content: " + content + "\n```";
            //await smc.SendMessageAsync(res);
        }
        private async Task HandleUserUpdate(SocketUser userbefore,SocketUser userafter)
        {
            Console.WriteLine("user update");
            //string name = userafter.Username;
            //Console.WriteLine(name);
            //var user = userafter as SocketUser;
            //Discord.IDMChannel ch =  await user.CreateDMChannelAsync();
            //await ch.SendMessageAsync("hello");
            //Console.WriteLine(user.Username);            
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) return;
            
            var context = new SocketCommandContext(_client, msg);

            //private msgs sent to private channel
            if (context.IsPrivate && !msg.Author.IsBot)
            {
                ISocketMessageChannel sc = _client.GetGuild(GlobalVars.PRIVATE_GUILD_ID).GetChannel(GlobalVars.PRIVATE_CHANNEL_ID) as ISocketMessageChannel;
                await sc.SendMessageAsync("(PRIVATE)" + msg.Author + " : " + msg.Content);
                try
                {
                    //await msg.DeleteAsync(); //no permission cuz personal channel does not have roles
                } catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                
                return;
            }

            //del msg if restrained with %
            if (AdminUtils.isRestrained(msg.Author.ToString()) && MasterUtils.roll(GlobalVars.MESSAGE_DELETE_RESTRAINED_CHANCE))
            {
                await msg.DeleteAsync();
                return;
            }

            //random react
            if (MasterUtils.roll(GlobalVars.ADD_REACTION_CHANCE) && !msg.Author.IsBot)
            {
                var tk = Task.Run(async () =>
                {
                    await msg.AddReactionAsync(EmojiUtils.getEmoji());
                });
            }

            //change bot game
            if (RexTimers.gameChangeClock.Elapsed.TotalSeconds > 600 )
            {
                DataUtils.changeMode(DataUtils.mode);
                RexTimers.gameChangeClock.Restart();              
            }



            int argPos = 0;
            //https://discordapp.com/api/webhooks/314670507578490880/yzQttIUi-yE9ZKMTZyPGENlZS3c3sjuxCpTw-LLhow24T6rSHYk9n5aDnmR9sKoBbIOz
            //{"channel_id": "200017396281507840", "guild_id": "200017396281507840", "헬퍼id": "314670507578490880"}
            if (msg.HasStringPrefix("체널", ref argPos))
            {
                //메인체널메세지전달
                var msc = _client.GetChannel(GlobalVars.CHANNEL_ID) as ISocketMessageChannel;
                await msc.SendMessageAsync(msg.Content.Replace("체널", ""));
                return;
            }

            if (msg.HasStringPrefix("유저", ref argPos))
            {
                //메인체널메세지전달
                string rez = msg.Content.Replace("유저","");
                string un = rez.Split()[0];
                string pmmsg = string.Empty;
                for(int i=0; i<rez.Split().Length-1; i++)
                {
                    pmmsg += rez.Split()[i + 1] + " ";
                }

                SocketUser msc = _client.GetUser(DataUtils.getUserIDFromUsername(un));
                RestDMChannel rdc = await msc.CreateDMChannelAsync(); 
                await rdc.SendMessageAsync(pmmsg);
                return;
            }
            
            if (msg.HasStringPrefix("시크릿", ref argPos))
            {
                string rez = msg.Content.Replace("시크릿", "");
                string cmdstr = rez.Split()[0];
                string argstr = string.Empty;
                string valstr = string.Empty;
                if (rez.Split().Length == 2)
                {
                    valstr = rez.Split()[1];
                } else if(rez.Split().Length == 3) {
                    argstr = rez.Split()[1];
                    valstr = rez.Split()[2];
                } else
                {
                    for(int z = 1; z<rez.Split().Length; z++)
                    {
                        valstr += rez.Split()[z];
                        if (z < rez.Split().Length - 1) valstr += " ";
                    }
                }
                
                string res = string.Empty;
                switch (cmdstr)
                {
                    case "guildcount": res=_client.Guilds.Count.ToString(); break;
                    case "post": await context.Channel.SendFileAsync("Objects/GlobalVars.cs"); break;
                    case "change":
                        switch (argstr)
                        {
                            case "channel": GlobalVars.CHANNEL_ID = ulong.Parse(valstr); break;
                            case "guild": GlobalVars.GUILD_ID = ulong.Parse(valstr); break;
                            case "restrain%": GlobalVars.MESSAGE_DELETE_RESTRAINED_CHANCE = int.Parse(valstr); break;
                            case "addreaction%": GlobalVars.ADD_REACTION_CHANCE = int.Parse(valstr); break;
                            case "autorestrain%": GlobalVars.AUTO_RESTRAIN_CHANCE = int.Parse(valstr); break;
                            case "statsshow": GlobalVars.STATS_SHOW = int.Parse(valstr); break;
                            case "cmdprefix": GlobalVars.COMMAND_PREFIX = valstr[0]; break;
                               
                            default: res = "arg error"; break;
                        }
                        break;
                    case "repeat": res = argstr + valstr; break;
                    case "admins": res = MasterUtils.printStringList(GlobalVars.ADMINS); break;
                    default: res = "not a valid command"; break;
                }
                if (res == string.Empty) res = "done!";
                await context.Channel.SendMessageAsync(res);
                return;
            }

                //restrain
                if (DataUtils.reports.ContainsKey(msg.Author.ToString()) 
                && !AdminUtils.isRestrained(msg.Author.ToString()) 
                && MasterUtils.roll(GlobalVars.AUTO_RESTRAIN_CHANCE)
                && DataUtils.modes[DataUtils.mode].getPermissions().Contains("auto restrain"))
            {
                if(DataUtils.reports[msg.Author.ToString()] > 1)
                {
                    double duration = DataUtils.rnd.Next(20, 40);
                    AdminUtils.addRestriction(msg.Author.ToString(), duration);
                    await context.Channel.SendMessageAsync("I feel like restraining " + msg.Author.Mention + " for " + Math.Round(duration, 0).ToString() + "s");
                    return;
                }
            }

            //msg stat update
            if (!msg.Author.IsBot)
            {
                //remove punc and save to sss (unused)
                string sss = msg.Content;
                var sb = new StringBuilder();

                foreach (char c in sss)
                {
                    if (!char.IsPunctuation(c))
                        sb.Append(c);
                }
                sss = sb.ToString();

                StatsUtils.MessagesRecieved++;
                StatsUtils.updateMessageUsage(msg.Author);
                StatsUtils.updateWordFreq(sss);
                double score = DataUtils.scoreSentence(msg.Content);
                StatsUtils.updateUserSentScore(msg.Author, score);
                //await context.Channel.SendMessageAsync("your avg: " + StatsUtils.getAverageUserSentScore(msg.Author));
                //StatsUtils.updateMessageUsage(msg.Author.Username + "#" + msg.Author.Discriminator);
            }

            //Command handle
            if (msg.HasCharPrefix(GlobalVars.COMMAND_PREFIX, ref argPos) && ((double)msg.Content.Count(x => x == '!')/msg.Content.Length) <0.51 
                )
            {
                if (!AdminUtils.isRestrained(msg.Author.ToString()))
                {
                    string trimmedcmd = msg.Content.ToString().ToLower().Trim('!');
                    if (DataUtils.modes[DataUtils.mode].getPermissions().Contains("xander") 
                        && MasterUtils.isAny(GlobalVars.XANDER_DISALLOWED_FUNCTIONS,new string[] { trimmedcmd.Split()[0] }) )
                        //&& !trimmedcmd.Split().Contains("help"))
                    {
                        //await msg.DeleteAsync();
                        RestDMChannel rdc = await msg.Author.CreateDMChannelAsync();
                        await rdc.SendMessageAsync($"```The command you requested (\"{msg.Content.ToString()}\") may contain material that may annoy people and is disabled in Xander Mode. ```\n" +
                            "Sorry for the inconvenience... I wish it didn't have to be this way.\nHave a great day friend.");
                        return;
                    }

                    var result = await _service.ExecuteAsync(context, argPos);

                    if (result.IsSuccess)
                    {
                        StatsUtils.CommandsRun++;
                        StatsUtils.updateCommandUsage(msg.Content.Split()[0]);
                    }
                    else
                    {
                        string errorStr = string.Empty;
                        switch (result.Error)
                        {
                            case CommandError.UnknownCommand: errorStr = "unknown command"; break;
                            case CommandError.BadArgCount: errorStr = "check your arguments"; break;
                            case CommandError.MultipleMatches: errorStr = "Multiple Matches for given cmd"; break;
                            case CommandError.ParseFailed: errorStr = "Parse failed"; break;
                            case CommandError.ObjectNotFound: errorStr = "Object Not Found"; break;
                            case CommandError.UnmetPrecondition: errorStr = "You don't have permission to use this command"; break;
                            case CommandError.Exception: errorStr = "Unknown exception occured (plz notify Rexyrex)"; break;
                            default: errorStr = "Critical Error!!! Notify Rexyrex ASAP"; break;
                        }
                        await context.Channel.SendMessageAsync("```\nCommand Error : " + errorStr + "```");
                        await context.Channel.SendMessageAsync("!help " + msg.Content.Split()[0]);
                    }
                } else
                {//restrained
                    await context.Channel.SendMessageAsync(msg.Author.Mention + " " + TrollUtils.getSnarkyComment());
                    RestDMChannel rdc = await msg.Author.CreateDMChannelAsync();
                    await rdc.SendMessageAsync("You are still restrained... " + AdminUtils.GetRestrainTimeRemaining(msg.Author.ToString()) + "s remaining");                    
                    //"\n\n You are currently restrained " + msg.Author.Mention + " (" + AdminUtils.GetRestrainTimeRemaining(msg.Author.ToString()) + "s remaining)");
                }
                
            } else
            {
                //Not a command
                //report tts
                if (!msg.Author.IsBot && msg.IsTTS && DataUtils.modes[DataUtils.mode].getPermissions().Contains("tts"))
                {
                    await context.Channel.SendMessageAsync("!report " + msg.Author);
                    return;
                }

                //Check mention users
                foreach (KeyValuePair<string[], string> entry in DataUtils.aliases)
                {
                    foreach (string entS in entry.Key)
                    {
                        string[] splitStr = msg.Content.ToLower().Split();
                        foreach (string ss in splitStr)
                        {
                            //is in aliasDict && is not mentioned by bot && mentioned person is in usernameDict
                            if (ss == entS.ToLower() && !msg.Author.IsBot && DataUtils.usernameDict.ContainsKey(DataUtils.aliases[entry.Key]))
                            {
                                SocketUser su = _client.GetUser(DataUtils.getUserIDFromUsername(DataUtils.aliases[entry.Key]));
                                StatsUtils.updateMentionedUsers(su);
                            }
                        }
                    }
                }

                if (!AdminUtils.isRestrained(msg.Author.ToString()) && MasterUtils.roll(DataUtils.modes[DataUtils.mode].getTriggerChance()))
                {
                    if (!msg.Author.IsBot)
                    {
                        string stz = msg.Content;
                        //MasterUtils.ContainsAny(stz, Const.CAT_TRIGGERS) || 
                        if (DataUtils.modes[DataUtils.mode].hasPermission("cat")) // only trigger in cat mode
                        {
                            string jsonStr = await WebUtils.httpRequest("http://random.cat/meow");
                            dynamic dynObj = JsonConvert.DeserializeObject(jsonStr);
                            string urlStr = dynObj.file;
                            await context.Channel.SendMessageAsync("DID I HEAR CAT???" + urlStr);
                            return;
                        }

                        if (MasterUtils.ContainsAny(stz, GlobalVars.EMINEM_TRIGGERS))
                        {
                            await context.Channel.SendFileAsync("pics/" + "eminem.jpg");
                            await context.Channel.SendMessageAsync("PALMS SPAGHETTI KNEAS WEAK ARM SPAGHETTI THERES SPAGHETTI ON HIS SPAGHETTI ALREADY, MOMS SPAGHETTI", true);
                        }
                        //response triggers
                        foreach (KeyValuePair<string, string[]> trigger in DataUtils.responses)
                        {
                            if (stz.Contains(trigger.Key))
                            {
                                int rz = DataUtils.rnd.Next(0, DataUtils.responses[trigger.Key].Length);
                                await context.Channel.SendMessageAsync(DataUtils.responses[trigger.Key][rz]);
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}