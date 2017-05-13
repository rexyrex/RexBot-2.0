using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.Diagnostics;
using RexBot2.Data;

namespace RexBot2
{
    public class Program
    {

        static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;

        private CommandHandler _handler;

        public async Task StartAsync()
        {
            var sw = Stopwatch.StartNew();
            _client = new DiscordSocketClient();

            new CommandHandler();
            new DataMaster();

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