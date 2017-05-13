using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RexBot2.Utils;
using Newtonsoft.Json;


namespace RexBot2.Modules
{
    public class WebModule : ModuleBase<SocketCommandContext>
    {
        [Command("dog")]
        [Summary("Get a random dog pic")]
        public async Task dogCmd()
        {
            string jsonStr = await WebUtils.httpRequest("https://random.dog/woof.json");
            dynamic dynObj = JsonConvert.DeserializeObject(jsonStr);
            string urlStr = dynObj.url;
            await Context.Channel.SendMessageAsync(urlStr);
        }

        [Command("cat")]
        [Summary("Get a random cat pic")]
        public async Task catCmd()
        {
            string jsonStr = await WebUtils.httpRequest("http://random.cat/meow");
            dynamic dynObj = JsonConvert.DeserializeObject(jsonStr);
            string urlStr = dynObj.file;
            await Context.Channel.SendMessageAsync(urlStr);
        }

        [Command("rather")]
        [Summary("What would you rather do?")]
        public async Task ratherCmd()
        {
            string jsonStr = await WebUtils.httpRequest("http://www.rrrather.com/botapi");
            dynamic dynObj = JsonConvert.DeserializeObject(jsonStr);
            string urlStr = dynObj.link;
            string question = dynObj.title;
            string choiceA = dynObj.choicea;
            string choiceB = dynObj.choiceb;
            string votes = dynObj.votes;
            await Context.Channel.SendMessageAsync("```" + question + "\nA. " + choiceA + "\nB. " + choiceB + "\n\n" + votes + " people voted on this question\n\n```" + "Click here to find out what dino option they chose : " + urlStr);
        }

        [Command("twitter")]
        [Summary("Search for term on twitter")]
        public async Task twitterCmd(string term = "dota2")
        {
            await Context.Channel.SendMessageAsync(WebUtils.getTweet(term));
        }

        [Command("urban")]
        [Summary("Definition from urban dictionary")]
        public async Task urbanCmd(string term = "dota2")
        {
            string users = Context.User.ToString();

            string jsonStr = await WebUtils.httpRequest("http://api.urbandictionary.com/v0/define?term=" + users);
            dynamic dynObj = JsonConvert.DeserializeObject(jsonStr);
            List<string> urls = new List<string>();
            foreach (var data in dynObj.list)
            {
                string urlStr = data.definition;
                urls.Add(urlStr);
            }
            await Context.Channel.SendMessageAsync(UtilMaster.getWord(urls));
        }

        [Command("gif")]
        [Summary("Search for a gif (default:dota2)")]
        public async Task gifCmd(string term = "dota2")
        {
            string query = term;
            string jsonStr = string.Empty;
            List<string> urls = new List<string>();
            dynamic dynObj;
            if (term == "random")
            {
                jsonStr = await WebUtils.httpRequest("http://api.giphy.com/v1/gifs/random?api_key=dc6zaTOxFJmzC");
                dynObj = JsonConvert.DeserializeObject(jsonStr);

                string urlStr = dynObj.data.url;
                urls.Add(urlStr);
            } else
            {
                jsonStr = await WebUtils.httpRequest("http://api.giphy.com/v1/gifs/search?q=" + query + "&api_key=dc6zaTOxFJmzC");
                dynObj = JsonConvert.DeserializeObject(jsonStr);

                foreach (var d in dynObj.data)
                {
                    string ul = d.url;
                    urls.Add(ul);
                }
            }

            await Context.Channel.SendMessageAsync(UtilMaster.getWord(urls));
        }

        [Command("img")]
        [Summary("Search for image on imgur")]
        public async Task imgCmd(string query = "dota2")
        {
            List<string> urls = new List<string>();


            string t = await WebUtils.httpRequest("https://api.imgur.com/3/gallery/search/?q=" + query,true);


            dynamic dynObj = JsonConvert.DeserializeObject(t);

            foreach (var data in dynObj.data)
            {
                string dt = data.link;
                urls.Add(dt);
            }

            await Context.Channel.SendMessageAsync(UtilMaster.getWord(urls));
        }


    }
}
