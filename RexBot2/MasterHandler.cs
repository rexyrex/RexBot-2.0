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
            _client.UserUpdated += HandleUserUpdate;
            //_client.MessageDeleted += HandleMsgDel;
            _client.MessageUpdated += MessageUpdated;
            _client.ReactionAdded += ReactionUpdated;
            _client.Disconnected += _client_Disconnected;
            _client.Connected += _client_Connected;
            _client.Ready += _client_Ready;
            _client.Log += _client_Log;
            

            //Console.WriteLine("latency:" + _client.Latency);
        }

        private async Task _client_Ready()
        {
            Logger.Log(LogSeverity.Info, "Rex CMD Handler", "Client Ready");
            await _client.SetStatusAsync(UserStatus.DoNotDisturb);

            //Initialize stopwatches
            userCollection = _client.GetGuild(200017396281507840).Users;
            new RexTimers(_client,userCollection);
            Console.WriteLine("Timer Initialization Complete!");
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
            await ErrorHandler.HandleLog(new LogMessage(LogSeverity.Error, "Disconnect Detected", "NOOOO", arg));
        }

        private async Task ReactionUpdated(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            Stats.ReactionCount++;
            //IUserMessage msg = await cache.GetOrDownloadAsync();
            //await msg.ModifyAsync(x =>
            //{
            //    x.Content = "herro";
            //}                
            //);
            //Console.WriteLine("Triggered!");
            //await msg.AddReactionAsync(reaction.Emote);
            //await channel.SendMessageAsync("REACTED");
            //await channel.SendMessageAsync("I also want to react");
        }

        private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            Stats.MsgEditCount++;
            //var message = await before.GetOrDownloadAsync();
            //string author = message.Author.ToString();
            //if(!message.Author.IsBot)// && message.Embeds == null)
            //    await channel.SendMessageAsync($"{author} changed his/her message from ```{message}``` to ```{after}```");
        }

        private async Task HandleMsgDel(Cacheable <Discord.IMessage, ulong> c, ISocketMessageChannel smc)
        {
            Stats.MsgDeleteCount++;
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

            if (!msg.Author.IsBot)
            {
                Stats.MessagesRecieved++;
                Stats.updateMessageUsage(msg.Author.Username);
            }

            if (MasterUtils.roll(-1) && !msg.Author.IsBot)
            {
                var tk = Task.Run(async () =>
                {
                    await msg.AddReactionAsync(EmojiUtils.getEmoji());
                });                    
            }

            if (MasterUtils.roll(27) && RexTimers.gameChangeClock.Elapsed.TotalSeconds > 127)
            {
                RexTimers.gameChangeClock.Restart();
                await _client.SetGameAsync(MasterUtils.getWord(DataUtils.games), "https://www.twitch.tv/ALL_ABOARD_THE_FEED_TRAIN", StreamType.Twitch);
            }

            int argPos = 0;
            if (msg.HasCharPrefix('!', ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    
                } else if (result.IsSuccess)
                {
                    Stats.CommandsRun++;
                    Stats.updateCommandUsage(msg.Content);
                }
            } else
            {
                
            }

            if (MasterUtils.roll(DataUtils.modes[DataUtils.mode].getTriggerChance()))
            {
                if (!msg.Author.IsBot)
                {
                    string stz = msg.Content;
                    int wcount = stz.Count(x => x == 'w');
                    int acount = stz.Count(x => x == '@');

                    if (msg.IsTTS && ((wcount > 0.4 * stz.Length && !stz.Contains('a')) || acount > 0.4 * stz.Length || stz.Contains("tata")) && stz.Length >= 3)
                    {
                        await context.Channel.SendMessageAsync("TTS ABUSER DETECTED");
                    }

                    foreach (KeyValuePair<string[], string> entry in DataUtils.aliases)
                    {
                        foreach (string entS in entry.Key)
                        {
                            if (stz.Contains(entS))
                            {
                                //await e.Channel.SendMessage(user + " mentioned something about " + aliases[entry.Key] + "...");
                                //await e.Channel.SendTTSMessage("Here is a recent tweet related to " + aliases[entry.Key] + " : " + getTweet(aliases[entry.Key]));
                            }
                        }
                    }

                    if (MasterUtils.ContainsAny(stz, new string[] { "cat", "kitty", "kitten", "caat", "caaat", "caaaat", "caaaaat", "meow" }) || DataUtils.modes[DataUtils.mode].hasPermission("cat"))
                    {
                        string jsonStr = await WebUtils.httpRequest("http://random.cat/meow");
                        dynamic dynObj = JsonConvert.DeserializeObject(jsonStr);
                        string urlStr = dynObj.file;
                        await context.Channel.SendMessageAsync("DID I HEAR CAT???");
                        await context.Channel.SendMessageAsync(urlStr);
                    }

                    if (MasterUtils.ContainsAny(stz, new string[] { "eminem", "one shot", "spaghetti" }))
                    {
                        await context.Channel.SendFileAsync("pics/" + "eminem.jpg");
                        await context.Channel.SendMessageAsync("PALMS SPAGHETTI KNEAS WEAK ARM SPAGHETTI THERES SPAGHETTI ON HIS SPAGHETTI ALREADY, MOMS SPAGHETTI",true);
                    }

                    foreach (KeyValuePair<string, string[]> trigger in DataUtils.responses)
                    {
                        if (stz.Contains(trigger.Key))
                        {
                            int rz = DataUtils.rnd.Next(0, DataUtils.responses[trigger.Key].Length);
                            await context.Channel.SendMessageAsync(DataUtils.responses[trigger.Key][rz]);
                        }
                    }
                }
            }
        }
    }
}