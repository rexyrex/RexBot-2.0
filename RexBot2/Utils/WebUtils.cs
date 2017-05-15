using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.Translate;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Tweetinvi;
using System.Linq;

namespace RexBot2.Utils
{
    public class WebUtils
    {
        public static string getTweet(string search)
        {
            string finalTweet = "No Tweet Found (Rex Tweet error plz msg Adrian)";
            var tweetTest = Search.SearchTweets(search);

            foreach (var item in tweetTest)
            {
                if (!(item.ToString().Contains("@")))
                    finalTweet = item.ToString();
            }
            return finalTweet;
        }

        public static async Task<string> TranslateToEng(string originalStr)
        {

            return originalStr;
        }

        public static async Task<string> YoutubeTest(string term)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyD1zmi4hoHLA3KqH7E07Bn27GElo9Gne7g",
                //ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = term;
            searchListRequest.MaxResults = 5;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            List<string> videos = new List<string>();
            List<string> channels = new List<string>();
            List<string> playlists = new List<string>();

            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                        break;

                    //case "youtube#channel":
                    //    channels.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.ChannelId));
                    //    break;

                    //case "youtube#playlist":
                    //    playlists.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
                    //    break;
                }
            }

            //Console.WriteLine(String.Format("Videos:\n{0}\n", string.Join("\n", videos)));
            //Console.WriteLine(String.Format("Channels:\n{0}\n", string.Join("\n", channels)));
            //Console.WriteLine(String.Format("Playlists:\n{0}\n", string.Join("\n", playlists)));
            string vidurl = MasterUtils.getWord(videos);
            string vidID = MasterUtils.reverse(extractVideoID(MasterUtils.reverse(vidurl)));
            //Console.WriteLine(vids);
            //Console.WriteLine(vidID);
            return $"https://www.youtube.com/watch?v={vidID}";
        }

        public static string extractVideoID(string title)
        {
            if(title.Count(x => x == '(') == 1 && title.Count(x => x == ')') == 1)
            {
                string res = string.Empty;
                bool isID = true;
                //bracket detection swapped for reverse string
                for(int i=0; i<title.Length; i++)
                {
                    if (isID && title[i]!='(' && title[i] != ')')
                    {
                        res += title[i];
                    }
                    if(title[i] == '(')
                    {
                            isID = false;
                    }
                }
                return res;
            }
            else
            {
                return "Not a valid video";
            }
        }

        public static async Task<string> httpRequest(string url)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            string received;

            using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)))
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(responseStream))
                    {
                        received = await sr.ReadToEndAsync();
                    }
                }
            }

            return received;
        }

        public static async Task<string> httpRequest(string url, bool forImgur)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            string received;
            request.Headers["Authorization"] = "Client-ID 1a8e14c14351c3b";

            using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)))
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(responseStream))
                    {

                        received = await sr.ReadToEndAsync();
                    }
                }
            }

            return received;
        }


    }
}
