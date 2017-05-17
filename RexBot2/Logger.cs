using Discord;
using System;

namespace RexBot2
{
    public static class Logger
    {
        /// <summary>
        /// Log information to the console.
        /// </summary>
        /// <param name="severity">The severity level of the log.</param>
        /// <param name="source">The source of the information.</param>
        /// <param name="message">The information in question.</param>
        public static void Log(LogSeverity severity, string source, string message)
        {
            NewLine($"{DateTime.Now.ToString("hh:mm:ss")} ", ConsoleColor.DarkGray);
            Append($"[{severity}] ", ConsoleColor.Red);
            Append($"{source}: ", ConsoleColor.DarkGreen);
            Append(message, ConsoleColor.White);
        }

        /// <summary> Write a string to the console on an new line. </summary>
        /// <param name="text">String written to the console.</param>
        /// <param name="foreground">The text color in the console.</param>
        /// <param name="background">The background color in the console.</param>
        public static void NewLine(string text = "", ConsoleColor? foreground = null, ConsoleColor? background = null)
        {
            if (foreground == null)
            {
                foreground = ConsoleColor.White;
            }

            if (background == null)
            {
                background = ConsoleColor.Black;
            }

            Console.ForegroundColor = (ConsoleColor)foreground;
            Console.BackgroundColor = (ConsoleColor)background;
            Console.Write(Environment.NewLine + text);
        }

        /// <summary> Write a string to the console on an existing line. </summary>
        /// <param name="text">String written to the console.</param>
        /// <param name="foreground">The text color in the console.</param>
        /// <param name="background">The background color in the console.</param>
        public static void Append(string text, ConsoleColor? foreground = null, ConsoleColor? background = null)
        {
            if (foreground == null)
            {
                foreground = ConsoleColor.White;
            }

            if (background == null)
            {
                background = ConsoleColor.Black;
            }

            Console.ForegroundColor = (ConsoleColor)foreground;
            Console.BackgroundColor = (ConsoleColor)background;
            Console.Write(text);
        }
    }
}
