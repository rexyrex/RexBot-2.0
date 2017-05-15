using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RexBot2.Utils;
using Newtonsoft.Json;
using Discord;
using Google.Apis.Translate;

namespace RexBot2.Modules
{
    public class WebModule : ModuleBase<SocketCommandContext>
    {
        [Command("dog")]
        [Remarks("web")]
        [Summary("Get a random dog pic")]
        public async Task dogCmd()
        {
            string jsonStr = await WebUtils.httpRequest("https://random.dog/woof.json");
            dynamic dynObj = JsonConvert.DeserializeObject(jsonStr);
            string urlStr = dynObj.url;
            await Context.Channel.SendMessageAsync(urlStr);
        }

        [Command("cat")]
        [Remarks("web")]
        [Summary("Get a random cat pic")]
        public async Task catCmd()
        {
            string jsonStr = await WebUtils.httpRequest("http://random.cat/meow");
            dynamic dynObj = JsonConvert.DeserializeObject(jsonStr);
            string urlStr = dynObj.file;
            await Context.Channel.SendMessageAsync(urlStr);
        }

        [Command("rather")]
        [Remarks("web")]
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

            EmbedBuilder emb = new EmbedBuilder();
            emb.Color = new Color(196, 09, 155);
            emb.Description = question + "\nA. " + choiceA + "\nB. " + choiceB + "\n\n" + votes + " people voted on this question\n\n" + "Link : " + urlStr;

            await Context.Channel.SendMessageAsync("",false,emb);
        }

        [Command("twitter")]
        [Remarks("web")]
        [Summary("Search for term on twitter")]
        public async Task twitterCmd([Remainder] string term = "dota2")
        {
            await Context.Channel.SendMessageAsync(WebUtils.getTweet(term));
        }

        [Command("urban")]
        [Remarks("web")]
        [Summary("Definition from urban dictionary")]
        public async Task urbanCmd([Remainder] string term = "dota2")
        {
            string jsonStr = await WebUtils.httpRequest("http://api.urbandictionary.com/v0/define?term=" + term);
            dynamic dynObj = JsonConvert.DeserializeObject(jsonStr);
            List<string> urls = new List<string>();
            foreach (var data in dynObj.list)
            {
                string urlStr = data.definition;
                urls.Add(urlStr);
            }
            await Context.Channel.SendMessageAsync(MasterUtils.getWord(urls));
        }

        [Command("gif")]
        [Remarks("web")]
        [Summary("Search for a gif (default:dota2)")]
        public async Task gifCmd([Remainder] string term = "dota2")
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

            await Context.Channel.SendMessageAsync(MasterUtils.getWord(urls));
        }

        [Command("img")]
        [Remarks("web")]
        [Summary("Search for image on imgur")]
        public async Task imgCmd([Remainder] string query = "dota2")
        {
            List<string> urls = new List<string>();


            string t = await WebUtils.httpRequest("https://api.imgur.com/3/gallery/search/?q=" + query,true);


            dynamic dynObj = JsonConvert.DeserializeObject(t);

            foreach (var data in dynObj.data)
            {
                string dt = data.link;
                urls.Add(dt);
            }

            await Context.Channel.SendMessageAsync(MasterUtils.getWord(urls));
        }

        [Command("youtube")]
        [Remarks("web")]
        [Summary("Search for video on youtube")]
        public async Task youtubeCmd([Remainder] string query = "dota2")
        {
            string res = await WebUtils.YoutubeTest(query);

            Console.WriteLine(res);
            await Context.Channel.SendMessageAsync(res);
        }

        [Command("today")]
        [Remarks("web")]
        [Summary("What happened today in the past?")]
        public async Task todayCmd()
        {
            string jsonStr = await WebUtils.httpRequest("http://history.muffinlabs.com/date");
            dynamic dynObj = JsonConvert.DeserializeObject(jsonStr);
            string date = dynObj.date;
            List<string> results = new List<string>();
            foreach (var data in dynObj.data.Events)
            {
                string year = data.year;
                string text = data.text;
                results.Add("Year " + year + ", " + date + " : " + text);
            }

            await Context.Channel.SendMessageAsync(MasterUtils.getWord(results));
        }

        [Command("translate")]
        [Alias("t")]
        [Remarks("web")]
        [Summary("Translate given text from English to Korean")]
        public async Task translateCmd([Remainder] string input)
        {
            //Console.WriteLine("translate call");
            string res = await WebUtils.TranslateText(input);
            //Console.WriteLine("done translate");
            await Context.Channel.SendMessageAsync(res);
        }

        [Command("auth")]
        [Remarks("web")]
        [Summary("Generate auth token for bing translate")]
        public async Task authCmd()
        {
            Console.WriteLine("auth call");
            string res = await WebUtils.getAuthToken();
            Console.WriteLine("auth done");
            await Context.Channel.SendMessageAsync("Success! Token:" + res);
        }

    }
}
