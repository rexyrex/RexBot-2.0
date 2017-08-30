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

        private MasterHandler _handler;

        public async Task StartAsync()
        {
            var sw = Stopwatch.StartNew();
            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                MessageCacheSize = 12,
                AlwaysDownloadUsers = true,
                LogLevel = LogSeverity.Info
            });

            Logger.Log(LogSeverity.Info, "StartAsync()", "Connected in " + sw.Elapsed.TotalSeconds.ToString("F2") + " seconds");
            Logger.NewLine();
            
            await _client.LoginAsync(TokenType.Bot, GlobalVars.BOT_TOKEN);

            await _client.StartAsync();
            
            _handler = new MasterHandler();

            await _handler.InitializeAsync(_client);
            sw.Stop();
            
            await Task.Delay(-1);
            
        }
    }
}