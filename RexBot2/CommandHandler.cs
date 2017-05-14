using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using System.Reflection;
using System.Threading.Tasks;
using RexBot2.Utils;
using System.Linq;

namespace RexBot2
{
    public class CommandHandler
    {
        private DiscordSocketClient _client;

        private CommandService _service;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;

            _service = new CommandService();

            await _service.AddModulesAsync(Assembly.GetEntryAssembly());

            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) return;

            var context = new SocketCommandContext(_client, msg);

            int argPos = 0;
            if (msg.HasCharPrefix('!', ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {

                }
            } else
            {               
                
            }
            if (!msg.Author.IsBot && DataUtils.modes[DataUtils.mode].hasPermission("functions"))
            {
                string stz = msg.Content;
                Console.WriteLine("in1");
                if (UtilMaster.ContainsAny(stz, new string[] { "!meme" }) && stz.Count(x => x == '(') == 3)
                {
                    Console.WriteLine("in2");
                    int bracketCount = 0;
                    string type = string.Empty;
                    string topText = string.Empty;
                    string botText = string.Empty;

                    for (int i = 0; i < stz.Length; i++)
                    {
                        if (stz[i] == '(' || stz[i] == ')')
                        {
                            bracketCount++;
                        }
                        if (bracketCount == 3)
                        {
                            if (stz[i] != '(')
                                topText += stz[i];
                        }
                        if (bracketCount == 5)
                        {
                            if (stz[i] != '(')
                                botText += stz[i];
                        }
                        if (bracketCount == 1)
                        {
                            if (stz[i] != ')' && stz[i] != '(')
                                type += stz[i];
                        }
                    }
                    topText = UtilMaster.processTextForMeme(topText);
                    botText = UtilMaster.processTextForMeme(botText);
                    await context.Channel.SendMessageAsync("https://memegen.link/" + type + "/" + topText + "/" + botText + ".jpg");
                }
            }
        }
    }
}