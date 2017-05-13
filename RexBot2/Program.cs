using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
    

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
            _client = new DiscordSocketClient();

            new CommandHandler();

            await _client.LoginAsync(TokenType.Bot, "MzEyNzM5MzQ3MzYxNDMxNTYy.C_fcug.4MF5L2T8z0yz5Y5SH8KqNWV5uNs");

            await _client.StartAsync();

            _handler = new CommandHandler();

            await _handler.InitializeAsync(_client);

            await Task.Delay(-1);
        }
    }
}