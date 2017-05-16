using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using RexBot2.Utils;
using Discord;

namespace RexBot2.Modules
{
    

    public class AudioModule2 : ModuleBase<ICommandContext>
    {
        // Scroll down further for the AudioService.
        // Like, way down.
        // Hit 'End' on your keyboard if you still can't find it.
        private readonly AudioService _service;

        public AudioModule2()
        {
            _service = DataUtils.rexAS;
        }

        // You *MUST* mark these commands with 'RunMode.Async'
        // otherwise the bot will not respond until the Task times out.
        [Command("join", RunMode = RunMode.Async)]
        [Remarks("audio")]
        [Summary("Join your voice channel")]
        public async Task JoinCmd()
        {
            Console.WriteLine("joining!");
            await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
        }

        // Remember to add preconditions to your commands,
        // this is merely the minimal amount necessary.
        // Adding more commands of your own is also encouraged.
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
