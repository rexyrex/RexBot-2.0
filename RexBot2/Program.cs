using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.Diagnostics;
using RexBot2.Utils;
using RexBot2.Timers;

namespace RexBot2
{

    public class Program
    {
        //https://discordapp.com/api/oauth2/authorize?client_id=312739347361431562&scope=bot&permissions=0

        static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;

        private CommandHandler _handler;

        public async Task StartAsync()
        {
            var sw = Stopwatch.StartNew();
            _client = new DiscordSocketClient();

            new CommandHandler();
            new DataUtils();
            new RexTimers();

            await _client.LoginAsync(TokenType.Bot, "MzEyNzM5MzQ3MzYxNDMxNTYy.C_fcug.4MF5L2T8z0yz5Y5SH8KqNWV5uNs");

            await _client.StartAsync();

            _handler = new CommandHandler();
            

            await _handler.InitializeAsync(_client);
            sw.Stop();
            Console.WriteLine("Connected in " + sw.Elapsed.TotalSeconds.ToString("F2") + " seconds");
            await Task.Delay(-1);
            
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}