using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;

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

    }
}
