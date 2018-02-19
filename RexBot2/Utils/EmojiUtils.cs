using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace RexBot2.Utils
{

    public class EmojiUtils
    {
        public static string tmpEmojis;
        public static string[] tmpEmojisList;
        public EmojiUtils()
        {
            //populate emojilist
            tmpEmojis = "😀 😃 😄 😁 😆 😅 😂 🤣 ☺️ 😊 😇 🙂 🙃 😉 😌 😍 😘 😗 😙 😚 😋 😜 😝 😛 🤑 🤗 🤓 😎 🤡 🤠 😏 😒 😞 😔 😟 😕 🙁 ☹️ 😣 😖 😫 😩 😤 😠 😡 😶 😐 😑 😯 😦 😧 😮 😲 😵 😳 😱 😨 😰 😢 😥 🤤 😭 😓 😪 😴 🙄 🤔 🤥 😬 🤐 🤢 🤧 😷 🤒 🤕 😈 👿 👹 👺 💩 👻 💀 ☠️ 👽 👾 🤖 🎃 😺 😸 😹 😻 😼 😽 🙀 😿 😾 👐 🙌 👏 🙏 🤝 👍 👎 👊 ✊ 🤛 🤜 🤞 ✌️ 🤘 👌 👈 👉 👆 👇 ☝️ ✋ 🤚 🖐 🖖 👋 🤙 💪 🖕 ✍️ 🤳 💅 🖖 💄 💋 👄 👅 👂 👃 👣 👁 👀 🗣 👤 👥 👶 👦 👧 👨 👩 👱‍♀️ 👱 👴 👵 👲 👳‍♀️ 👳 👮‍♀️ 👮";
            populate();
        }

        public static void populate()
        {
            tmpEmojisList = tmpEmojis.Split();
        }

        public static string rand()
        {
            Random r = new Random();
            int i = r.Next(0, tmpEmojisList.Length);
            return tmpEmojisList[i];
        }

        public static Emoji getRandEmoji()
        {
            Emoji ej = new Emoji(rand());
            return ej;
        }

        public static Emoji getEmoji(string c)
        {
            Emoji ej = new Emoji(c);
            return ej;
        }
    }
}
