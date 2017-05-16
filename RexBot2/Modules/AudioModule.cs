using Discord;
using Discord.Audio;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace RexBot2.Modules
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        [Command("join2")]
        [Remarks("audio")]
        [Summary("Join your voice channel")]
        public async Task JoinChannelCmd(IVoiceChannel channel = null)
        {
            // Get the audio channel
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument."); return; }
            
            // For the next step with transmitting audio, you would want to pass this Audio Client in to a service.
            var audioClient = await channel.ConnectAsync();
        }

        [Command("music")]
        [Remarks("audio")]
        [Summary("Play sth")]
        public async Task PlayMusicCmd(IVoiceChannel channel = null)
        {

        }
            
        private async Task SendAsync(IAudioClient client, string path)
        {
            // Create FFmpeg using the previous example
            var ffmpeg = CreateStream(path);
            var output = ffmpeg.StandardOutput.BaseStream;
            var discord = client.CreatePCMStream(AudioApplication.Mixed, 1920);
            await output.CopyToAsync(discord);
            await discord.FlushAsync();
        }

        private Process CreateStream(string path)
        {
            var ffmpeg = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i {path} -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            return Process.Start(ffmpeg);
        }
    }
}
