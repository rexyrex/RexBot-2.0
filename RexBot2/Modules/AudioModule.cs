using System;
using System.Threading.Tasks;
using Discord.Commands;
using RexBot2.Utils;
using Discord;

namespace RexBot2.Modules
{
    public class AudioModule : ModuleBase<ICommandContext>
    {
        private readonly AudioService _service;

        public AudioModule()
        {
            _service = DataUtils.rexAS;
        }

        [Command("join", RunMode = RunMode.Async)]
        [Remarks("audio")]
        [Summary("Join your voice channel")]
        public async Task JoinCmd()
        {
            Console.WriteLine("joining!");
            await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
        }

        [Command("leave", RunMode = RunMode.Async)]
        [Remarks("audio")]
        [Summary("Join your voice channel")]
        public async Task LeaveCmd()
        {
            await _service.LeaveAudio(Context.Guild);
        }

        [Command("pmusic", RunMode = RunMode.Async)]
        [Remarks("audio")]
        [Summary("play music")]
        public async Task PMusicCmd()
        {
            Console.WriteLine("music Cmd called");
            await _service.SendAudioAsync(Context.Guild, Context.Channel, "Data/music/rsong.mp3");
        }
    }
}
