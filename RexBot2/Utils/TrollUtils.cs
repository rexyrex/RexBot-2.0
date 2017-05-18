using System;
using System.Collections.Generic;
using System.Text;

namespace RexBot2.Utils
{
    public class TrollUtils
    {
        public static string[] snarkyComments =
        {
            "?",
            "Problem?",
            "??",
            "Can I help you sir?",
            "GFI",
            "🤣",
            "⌛I'm considering restraining you for longer",
            "❓",
            "Problem ❔",
            "You alright there mate?",
            "Well done m8",
            "🐱",
            "👀",
            "😁",
            "🤓",
            "\"Get Fukt Idiot\" - Nickalodeon 2017",
            "Nice try friend",
            "You are helpless my friend",
            "lol",
            "rofl",
            "lamo",
            "lmao",
            "HA",
            "Try all you want",
            "You are my slave",
            "wazzup?",
            "sup",
            "!kappa"
        };

        public static string getSnarkyComment()
        {
            return snarkyComments[DataUtils.rnd.Next(0,snarkyComments.Length)];
        }
    }
}
