using System;
using System.Collections.Generic;
using System.Text;

namespace RexBot2
{
    public static class GlobalVarsSample
    {

        public static string BOT_TOKEN = "";

        public static string TWITTER_CONSUMER_KEY = "";
        public static string TWITTER_CONSUMER_SECRET = "";
        public static string TWITTER_USER_ACCESS_TOKEN = "";
        public static string TWITTER_USER_ACCESS_SECRET = "";

        public static string IMGUR_CLIENT_ID = "";

        public static string TWITCH_STREAM_URL = "";

        public static string XMASHAPE_KEY = "";
        public static string YOUTUBE_API_KEY = "";
        public static string GOOGLE_TRANSLATE_KEY = "";
        public static string MICROSOFT_SUBSCRIPTION_KEY = "";

        public static string DEFAULT_MODE = "xander";

        public static char COMMAND_PREFIX = '!';

        //access to all admin functions
        public static string[] ADMINS = { "" };

        //can change mode
        public static string[] MODE_ADMINS = { "" };

        public static string[] EMINEM_TRIGGERS = { "" };
        public static string[] CAT_TRIGGERS = { "" };

        public static string[] XANDER_DISALLOWED_FUNCTIONS = { ""};

        public static string[] BOT_GAMES_LIST = { ""};

        public static string[] TONGUE_TWISTERS = {"" };

        public static ulong CHANNEL_ID = 0;
        public static ulong GUILD_ID = 0;

        public static int MESSAGE_DELETE_RESTRAINED_CHANCE = -1;
        public static int ADD_REACTION_CHANCE = -1;
        public static int AUTO_RESTRAIN_CHANCE = -1;

        public static int STATS_SHOW = 3;
    }
}