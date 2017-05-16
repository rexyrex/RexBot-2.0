using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RexBot2.Utils;


namespace RexBot2.Modules
{
    public class PicModule : ModuleBase<SocketCommandContext>
    {
        private string picPath = "Data/pics/";

        [Command("kappa")]
        [Remarks("pic")]
        [Summary("Kappa face")]
        public async Task kappaCmd()
        {
            Console.WriteLine("attempt");
            await Context.Channel.SendFileAsync(picPath + "Kappahd.png");
        }

        [Command("feelsgood")]
        [Remarks("pic")]
        [Summary("feels good man!")]
        public async Task fgmCmd()
        {
            await Context.Channel.SendFileAsync(picPath + "feelsgood.png");
        }

        [Command("feelsbad")]
        [Remarks("pic")]
        [Summary("feels bad man!")]
        public async Task fbmCmd()
        {
            await Context.Channel.SendFileAsync(picPath + "feelsbad.png");
        }

        [Command("choo")]
        [Remarks("pic")]
        [Alias("choochoo","conductor","train")]
        [Summary("feels bad man!")]
        public async Task chooCmd()
        {            
            string phrase = MasterUtils.getWord(DataUtils.trainPhrases1) + " " + MasterUtils.getWord(DataUtils.trainPhrases2) + " " + MasterUtils.getWord(DataUtils.trainPhrases3);
            await Context.Channel.SendFileAsync(picPath + "choochooz.png");
            await Context.Channel.SendMessageAsync(phrase);
        }

        [Command("doge")]
        [Remarks("pic")]
        [Summary("Random doge meme")]
        public async Task dogeCmd()
        {
            string topText = "wow! such " + MasterUtils.getWord(DataUtils.adjList);
            string botText = "much " + MasterUtils.getWord(DataUtils.nounList);
            topText = MasterUtils.processTextForMeme(topText);
            botText = MasterUtils.processTextForMeme(botText);
            await Context.Channel.SendMessageAsync("https://memegen.link/" + "doge" + "/" + topText + "/" + botText + ".jpg");
        }

        [Command("sup")]
        [Remarks("pic")]
        [Alias("pic")]
        [Summary("Get a pic with an appropriate description")]
        public async Task supCmd()
        {
            int r = DataUtils.rnd.Next(DataUtils.picNames.Count);
            string picToString = DataUtils.picNames[r];
            await Context.Channel.SendFileAsync("Data/friendpics/" + picToString);
            await Context.Channel.SendMessageAsync(MasterUtils.commentOnPic(),true);

        }


    }
}
