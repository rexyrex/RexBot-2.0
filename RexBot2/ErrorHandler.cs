using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RexBot2
{
    class ErrorHandler
    {
        private readonly CommandService _commandService;

        public ErrorHandler(CommandService commandService)
        {
            _commandService = commandService;
            _commandService.Log += HandleLog;
        }

        public static Task HandleLog(LogMessage logMessage)
        {
            return Task.Run(async () =>
            {
                if (logMessage.Exception is CommandException cmdEx)
                {
                    if (cmdEx.InnerException is RateLimitedException)
                    {
                        return;
                    } else if (cmdEx.InnerException is HttpException httpEx)
                    {
                        var message = string.Empty;
                        switch (httpEx.DiscordCode)
                        {
                            case null:
                                switch (httpEx.HttpCode)
                                {
                                    case HttpStatusCode.BadRequest:
                                        message = "There seems to have been a bad request.";
                                        break;
                                    case HttpStatusCode.BadGateway:
                                        message = "Something went wrong with the gateway connection. Try again in a bit.";
                                        break;
                                    case HttpStatusCode.Forbidden:
                                        message = "Not enough permission to do that.";
                                        break;
                                    case HttpStatusCode.InternalServerError:
                                        message = "Looks like Discord fucked up. An interal server error has occured on Discord's part which is " +
                                        "entirely unrelated with RexBot. Sorry, nothing I can do.";
                                        break;
                                    default:
                                        message = "Something went wrong. Please try again later.";
                                        break;
                                }
                                break;
                            case 50013:
                                message = "No permission to do that.";
                                break;
                            case 50007:
                                message = "No permission to send messages to this user.";
                                break;
                            default:
                                message = httpEx.Message.Remove(0, 39) + ".";
                                break;
                        }
                        //await cmdEx.Context.Channel.ReplyAsync(cmdEx.Context.User, message, null, Config.ERROR_COLOR);
                        Console.WriteLine(message);
                    }
                    else
                    {
                        var message = cmdEx.InnerException.Message;
                        if (cmdEx.InnerException.InnerException != null)
                        {
                            message += $"\n**Inner Exception:** {cmdEx.InnerException.InnerException.Message}";
                        }

                        //await cmdEx.Context.Channel.ReplyAsync(cmdEx.Context.User, message, null, Config.ERROR_COLOR);
                        Console.WriteLine(message);
                        if ((await cmdEx.Context.Guild.GetCurrentUserAsync() as IGuildUser).GetPermissions(cmdEx.Context.Channel as SocketTextChannel).AttachFiles)
                        {
                            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(cmdEx.ToString() ?? string.Empty)))
                            {
                                await cmdEx.Context.Channel.SendFileAsync(ms, "Stack_Trace.txt");
                            }
                        }
                    }
                }
                else if (logMessage.Exception != null)
                {
                    Logger.Log(LogSeverity.Error, logMessage.Exception.Source, logMessage.Exception.StackTrace);
                }
            });
        }
    }
}
