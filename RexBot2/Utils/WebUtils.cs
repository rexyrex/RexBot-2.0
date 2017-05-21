using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi;
using System.Linq;
using Google.Apis.Translate.v2;
using TranslationsResource = Google.Apis.Translate.v2.Data.TranslationsResource;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using RexBot2.Timers;

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
                //Console.WriteLine(item.ToString());
                if (!(item.ToString().Contains("@")))
                {
                    finalTweet = item.ToString();
                    //Search.SearchRepliesTo(item, false);
                }
            }
            return finalTweet;
        }

        private static string GenerateTranslateOptionsRequestBody(string category, string contentType, string reservedFlags, string state, string uri, string user)
        {
            string body =
                "<TranslateOptions xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\">" +
                "  <Category>{0}</Category>" +
                "  <ContentType>{1}</ContentType>" +
                "  <ReservedFlags>{2}</ReservedFlags>" +
                "  <State>{3}</State>" +
                "  <Uri>{4}</Uri>" +
                "  <User>{5}</User>" +
                "</TranslateOptions>";
            return string.Format(body, category, contentType, reservedFlags, state, uri, user);
        }
        //client id : rexbot2
        //secret: xRi60OBVa+tfqDmqwDpVDmHoXwO+yci5WhbR8MSIZBg=
        //scope: http://api.microsofttranslator.com
        // grant_type = client_credentials
        public static async Task<string> TranslateText(string input, string inlang, string outlang)
        {
            //First check if auth token is valid
            //if first time calling this function or 8minutes passed since auth token update
            if(DataUtils.bingAuthStr == string.Empty || RexTimers.bingAuthClock.Elapsed.TotalMinutes > 8)
            {
                await updateBingAuthToken();
                Console.WriteLine("Auth token refreshed on translate call!");
            }


            string processed = input;//uriEncode(input);
            string fromlanguage = inlang;//from language you can change this as your requirement
            string translatedText = "";//collect result here
            string texttotranslate = processed;//what to be translated?
            string tolanguage = outlang;//in which language?
                                                                   //preparing url with all four parameter
            string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?" + "text=" + texttotranslate + "&from=" + fromlanguage + "&to=" + tolanguage;
            //making web request to url
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            string authToken = DataUtils.bingAuthStr;
            request.Headers["Authorization"] = "Bearer " + authToken;
            try
            {
                //getting response from api
                WebResponse response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null));
                //WebResponse response = await request.GetResponseAsync();
                System.IO.Stream strm = response.GetResponseStream();
                StreamReader reader = new System.IO.StreamReader(strm);
                //reading result 
                translatedText = reader.ReadToEnd();
                //Console.Write("Converted Texts Are: " + translatedText);
                return processBingOutput(translatedText);
            } catch (Exception e)
            {
                Console.WriteLine("Translate error : " + e.ToString());
            }
            return "OAuth Token Error! Should work the next time you invoke this command... (If not, notify Adrian plz)";
            
        }

        public static string processBingOutput(string input)
        {
            string res = string.Empty;
            bool write = false;
            for(int i=0; i<input.Length; i++)
            {
                if (write)
                {
                    res += input[i];
                }
                if (input[i] == '>')
                {
                    write = true;
                }
                if (input[i] == '<')
                {
                    write = false;
                }
            }
            res = res.Trim('<');
            return res;
        }

        public static async Task updateBingAuthToken()
        {
            DataUtils.bingAuthStr = await getAuthToken();
        }

        //key: 00bd3d2150614e8594f7d5657e913189
        //Use httpclient instead of legacy httpwebrequest
        //we need to generate a new auth token every 10 mins (cuz microsoft)
        public static async Task<string> getAuthToken()
        {
            string data = "";
            HttpClient client = new HttpClient();

            var requestContent = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("", ""),
                });
            HttpResponseMessage response = await client.PostAsync(
    "https://api.cognitive.microsoft.com/sts/v1.0/issueToken?Subscription-Key=" + GlobalVars.MICROSOFT_SUBSCRIPTION_KEY,
    requestContent);

            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                // Write the output.
                //Console.WriteLine(await reader.ReadToEndAsync());
                string res = await reader.ReadToEndAsync();
                return res;
            }


            //var response = client.PostAsync("https://api.cognitive.microsoft.com/sts/v1.0/issueToken?Subscription-Key=00bd3d2150614e8594f7d5657e913189", data);
            //UTF8Encoding enc = new UTF8Encoding();
            //var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.cognitive.microsoft.com/sts/v1.0/issueToken?Subscription-Key=00bd3d2150614e8594f7d5657e913189");
            ////httpWebRequest.Headers["Ocp-Apim-Subscription-Key"] = "00bd3d2150614e8594f7d5657e913189";
            //Console.WriteLine("here!1");
            
            //try
            //{
            //    System.IO.Stream wr = await httpWebRequest.GetRequestStreamAsync();
            //    wr.Write(enc.GetBytes(data), 0, data.Length);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("error : " + e.ToString());
            //}
            
            //try
            //{
            //    WebResponse httpResponse = (await Task<WebResponse>.Factory.FromAsync(httpWebRequest.BeginGetResponse, httpWebRequest.EndGetResponse, null));
            //    var responseStream = httpResponse.GetResponseStream();

            //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //    {
            //        string answer = JsonConvert.DeserializeObject<string>(streamReader.ReadToEnd());
            //        return answer;
            //    }
            //} catch (Exception e)
            //{
            //    Console.WriteLine("error : " + e.ToString());
            //}
            
            //Console.WriteLine("here!2");
            //return "fail";

        }


       

        public static async void RexTranslate2()
        {
            string authToken = "05fadf6df12e4d958d45ce5140db56fc";
            string text = "una importante contribución a la rentabilidad de la empresa";
            string uri = "https://api.microsofttranslator.com/v2/Http.svc/GetTranslations?text=" + text + "&from=" + "es" + "&to=" + "en" + "&maxTranslations=5";
            string requestBody = GenerateTranslateOptionsRequestBody("general", "text/plain", "", "", "", "TestUserId");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers["Authorization"] = authToken;
            request.ContentType = "text/xml";
            request.Method = "POST";
            using (var stream = await Task.Factory.FromAsync<System.IO.Stream>(request.BeginGetRequestStream, request.EndGetRequestStream, null))
            {
                byte[] arrBytes = Encoding.ASCII.GetBytes(requestBody);
                stream.Write(arrBytes, 0, arrBytes.Length);
            }
            using (WebResponse response = await request.GetResponseAsync())
            using (System.IO.Stream respStream = response.GetResponseStream())
            {
                StreamReader rdr = new StreamReader(respStream, System.Text.Encoding.ASCII);
                string strResponse = rdr.ReadToEnd();

                Console.WriteLine("Available translations for source text '{0}' are", text);
                XDocument doc = XDocument.Parse(@strResponse);
                XNamespace ns = "http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2";
                int i = 1;
                foreach (XElement xe in doc.Descendants(ns + "TranslationMatch"))
                {
                    Console.WriteLine("{0}Result {1}", Environment.NewLine, i++);
                    foreach (var node in xe.Elements())
                    {
                        Console.WriteLine("{0} = {1}", node.Name.LocalName, node.Value);
                    }
                }
            }
        }

            //Discontinued... THIS IS PAYED SERVICE WITH NO FREE QUOTA WTF (smh google)
            public static async Task<string> RexTranslate(string originalStr)
        {
            var service = new TranslateService(new BaseClientService.Initializer()
            {
                ApiKey = GlobalVars.GOOGLE_TRANSLATE_KEY,
                ApplicationName = "Translate API Sample"
            });
            string[] toTranslate = new string[1];
            toTranslate[0] = originalStr;
            var response = await service.Translations.List(toTranslate, "ko").ExecuteAsync();
            var translations = new List<string>();

            foreach (TranslationsResource translation in response.Translations)
            {
                translations.Add(translation.TranslatedText);
                Console.WriteLine("translation :" + translation.TranslatedText);
            }

            // Translate the text (back) to English.
            //Console.WriteLine("Translating to English ...");
            //response = service.Translations.List(translations, "en").Execute();
            //foreach (TranslationsResource translation in response.Translations)
            //{
            //    Console.WriteLine("translation :" + translation.TranslatedText);
            //}

            return translations.ElementAt<string>(0);
        }

        public static async Task<string> YoutubeTest(string term)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = GlobalVars.YOUTUBE_API_KEY,
                //ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = term;
            searchListRequest.MaxResults = 1;

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

        public static async Task<string> getOneLiner()
        {
            string jsonStr = await httpRequest("http://api.yomomma.info/"); 
            dynamic dynObj = JsonConvert.DeserializeObject(jsonStr);
            string joke = dynObj.joke;
            return joke;
        }

        public static async Task<string> yodaOutput(string input)
        {
            Uri uri = new Uri("https://yoda.p.mashape.com/yoda?sentence=" + processForUrl(input));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            string received;
            request.Headers["X-Mashape-Key"] = GlobalVars.XMASHAPE_KEY;
            request.Headers["Accept"] = "text/plain";

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

        public static string processForUrl(string input)
        {
            string res = string.Empty;
            res = input.Replace(" ", "+");
            res = res.Replace("?", "%3F");
            res = res.Replace("^", "%5E");
            res = res.Replace("#", "%23");
            //res = res.Replace("#", "~h");
            //res = res.Replace("/", "~s");
            return res;
        }

        public static async Task<string> getImgurUrl(string searchTerm)
        {
            List<string> urls = new List<string>();

            string t = await WebUtils.httpRequest("https://api.imgur.com/3/gallery/search/?q=" + searchTerm, true);

            dynamic dynObj = JsonConvert.DeserializeObject(t);

            foreach (var data in dynObj.data)
            {
                string dt = data.link;
                urls.Add(dt);
            }

            return MasterUtils.getWord(urls);
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
            request.Headers["Authorization"] = "Client-ID " + GlobalVars.IMGUR_CLIENT_ID;

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
