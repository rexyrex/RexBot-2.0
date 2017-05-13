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
        [Summary("Kappa face")]
        public async Task kappaCmd()
        {
            Console.WriteLine("attempt");
            await Context.Channel.SendFileAsync(picPath + "Kappahd.png");
        }

        [Command("feelsgood")]
        [Summary("feels good man!")]
        public async Task fgmCmd()
        {
            await Context.Channel.SendFileAsync(picPath + "feelsgood.png");
        }

        [Command("feelsbad")]
        [Summary("feels bad man!")]
        public async Task fbmCmd()
        {
            await Context.Channel.SendFileAsync(picPath + "feelsbad.png");
        }

        [Command("choo")]
        [Alias("choochoo","conductor")]
        [Summary("feels bad man!")]
        public async Task chooCmd()
        {
            await Context.Channel.SendFileAsync(picPath + "choochooz.png");
        }

        [Command("doge")]
        [Summary("Random doge meme")]
        public async Task dogeCmd()
        {
            string topText = "wow! such " + UtilMaster.getWord(DataUtils.adjList);
            string botText = "much " + UtilMaster.getWord(DataUtils.nounList);
            topText = UtilMaster.processTextForMeme(topText);
            botText = UtilMaster.processTextForMeme(botText);
            await Context.Channel.SendFileAsync(picPath + "Kappahd.png");
        }

        [Command("sup")]
        [Summary("Get a pic with an appropriate description")]
        public async Task supCmd()
        {
            int r = DataUtils.rnd.Next(DataUtils.picNames.Count);
            string picToString = DataUtils.picNames[r];
            await Context.Channel.SendFileAsync("Data/friendpics/" + picToString);
            await Context.Channel.SendMessageAsync(UtilMaster.commentOnPic(),true);

        }


    }
}
