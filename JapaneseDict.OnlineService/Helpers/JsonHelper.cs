using JapaneseDict.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Web.Http;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Security.Cryptography.Core;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http.Filters;
using JapaneseDict.OnlineService.Helpers;
using Windows.UI.Xaml.Media;
using Windows.Foundation.Metadata;
using SQLite;

namespace JapaneseDict.OnlineService.Helpers
{
    public static class JsonHelper
    {
        private static readonly string JPDICT_API_KEY = AppConfig.Configurations["jpdict_api_key"];
        private static readonly string BAIDU_APP_ID = AppConfig.Configurations["baidu_app_id"];
        private static readonly string BAIDU_APP_KEY = AppConfig.Configurations["baidu_app_key"];
        private static readonly string TEXTRA_KEY = AppConfig.Configurations["textra_key"];
        private static readonly string TEXTRA_SECRET = AppConfig.Configurations["textra_secret"];

        private static async Task<string> GetJsonString(string uri)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(new Uri(uri));
                response.EnsureSuccessStatusCode();
                var responseText = await response.Content.ReadAsStringAsync();
                return responseText; 
            }
            catch
            {
                return "";
            }
        }
        const string TRANS_URL_JPTOCN = "https://mt-auto-minhon-mlt.ucri.jgn-x.jp/api/mt/generalN_ja_zh-CN/";
        const string TRANS_URL_CNTOJP = "https://mt-auto-minhon-mlt.ucri.jgn-x.jp/api/mt/generalN_zh-CN_ja/";
        private static async Task<string> GetJsonStringForTranslate(string uri,string text)
        {
            try
            {
                OAuthBase oAuthBase = new OAuthBase();
                var timestamp = oAuthBase.GenerateTimeStamp();
                var nonce = oAuthBase.GenerateNonce();
                var httpClient = new HttpClient();
                var request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                Dictionary<string, string> pairs = new Dictionary<string, string>();
                pairs.Add("name", "kevingao");
                pairs.Add("key", TEXTRA_KEY);
                pairs.Add("text", text);
                pairs.Add("split", "1");
                pairs.Add("oauth_consumer_key", TEXTRA_KEY);
                pairs.Add("oauth_token", TEXTRA_SECRET);
                pairs.Add("oauth_timestamp", timestamp);
                pairs.Add("oauth_nonce", nonce);
                pairs.Add("oauth_version", "1.0");
                pairs.Add("oauth_signature", TEXTRA_SECRET+"&");
                pairs.Add("oauth_signature_method", "PLAINTEXT");

                var formContent = new HttpFormUrlEncodedContent(pairs);
                var response = await httpClient.PostAsync(new Uri(uri),formContent);
                response.EnsureSuccessStatusCode();
                var responseText = await response.Content.ReadAsStringAsync();
                return responseText;
            }
            catch
            {
                return "ERROR";
            }
        }
        
        public static async Task<String> GetTranslateResult(string input, string sourcelang, string targetlang)
        {
            try
            {
                var random = new Random(1000000000).Next();
                var sign = Md5Helper.Encode(BAIDU_APP_ID + input + random + BAIDU_APP_KEY);
                string jsonStr = await GetJsonString("https://fanyi-api.baidu.com/api/trans/vip/translate?q=" + $"{Uri.EscapeDataString(input)}&from={sourcelang}&to={targetlang}&appid={BAIDU_APP_ID}&salt={random}&sign={sign}");
                JsonObject jsonobj = JsonObject.Parse(jsonStr);
                return jsonobj["trans_result"].GetArray().GetObjectAt(0).GetObject()["dst"].GetString(); 
            }
            catch
            {
                return "出现错误";
            }
        }
        public static async Task<String> GetJpToCnTranslationResult(string input)
        {
            string jsonStr = await GetJsonStringForTranslate($"{TRANS_URL_JPTOCN}",input);
            JsonObject jsonobj = JsonObject.Parse(jsonStr);
            return jsonobj["resultset"].GetObject()["result"].GetObject()["text"].GetString();
        }
        public static async Task<String> GetCnToJpTranslationResult(string input)
        {
            string jsonStr = await GetJsonStringForTranslate($"{TRANS_URL_CNTOJP}", input);
            JsonObject jsonobj = JsonObject.Parse(jsonStr);
            return jsonobj["resultset"].GetObject()["result"].GetObject()["text"].GetString();
        }
        [Obsolete]
        public static async Task<EverydaySentence> GetEverydaySentence(int index)
        {
            try
            {
                JsonObject resultobj = JsonObject.Parse(await GetJsonString("http://api.skylark-workshop.xyz/api/GetDailySentence?code=fi6c4bz3w5LkUnl8hGT0V4n/PoKBq7KH3Ly8za8HC4b/r8QRfj/zzw==&index=" + index));
                return new EverydaySentence() { JpText = resultobj["sentence"].GetString(), CnText = resultobj["trans"].GetString(), AudioUri = new Uri(resultobj["audio"].GetString()), NotesOnText = resultobj["sentencePoint"].GetString(), Author = resultobj["creator"].GetString(), BackgroundImage = new BitmapImage(new Uri($"ms-appx:///Assets/EverydaySentenceBackground/{index}.jpg", UriKind.RelativeOrAbsolute)) };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return new EverydaySentence() { JpText = "出现错误", CnText = "请检查网络连接", BackgroundImage = new BitmapImage(new Uri($"ms-appx:///Assets/EverydaySentenceBackground/{index}.jpg", UriKind.RelativeOrAbsolute)) };
            }
        }
        public static async Task<List<EverydaySentence>> GetEverydaySentences()
        {
            try
            {
                JsonArray resultList = JsonArray.Parse(await GetJsonString($"https://jpdictapi.terra-incognita.dev/api/GetDailySentence?code={JPDICT_API_KEY}"));
                List<EverydaySentence> result = new List<EverydaySentence>();
                for (int i = 0; i < 3; i++)
                {
                    JsonObject resultObj = resultList[i].GetObject();
                    result.Add(new EverydaySentence() 
                    { 
                        JpText = resultObj["sentence"].GetString(), 
                        CnText = resultObj["trans"].GetString(), 
                        AudioUri = new Uri(resultObj["audio"].GetString()), 
                        NotesOnText = resultObj["sentencePoint"].GetString(), 
                        Author = resultObj["creator"].GetString(), 
                        BackgroundImage = new BitmapImage(new Uri($"ms-appx:///Assets/EverydaySentenceBackground/{i}.jpg", UriKind.RelativeOrAbsolute)) 
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                List<EverydaySentence> result = new List<EverydaySentence>();
                for (int i = 0; i < 3; i++)
                {
                    result.Add(new EverydaySentence() { JpText = "出现错误", CnText = "请检查网络连接", BackgroundImage = new BitmapImage(new Uri($"ms-appx:///Assets/EverydaySentenceBackground/{i}.jpg", UriKind.RelativeOrAbsolute)) });
                }
                return result;
            }
        }
        [Obsolete]
        public static async Task<NHKNews> GetNHKNews(int index)
        {
            try
            {
                string jsonStr = await GetJsonString("http://api.skylark-workshop.xyz/api/GetNHKNews?code=cElmudLe2wJ8tOXumYBog85EiqHN/76341GVoB5Ogtltdxrr/xlGmQ==");
                JsonObject jsonobj = JsonObject.Parse(jsonStr);
                var resultarritem = jsonobj["data"].GetObject()["item"].GetArray()[index];
                NHKNews res = new NHKNews() {Title=resultarritem.GetObject()["title"].GetString(),Link= new Uri(resultarritem.GetObject()["link"].GetString()), OriginalLink = new Uri(resultarritem.GetObject()["link"].GetString()), IconPath=new Uri(resultarritem.GetObject()["iconPath"].GetString()),VideoPath=new Uri(resultarritem.GetObject()["videoPath"].GetString()) };
                return res;
            }
            catch (Exception)
            {
                return new NHKNews() { Title = "出现连接错误",IconPath=new Uri("ms-appx:///Assets/connectionerr.png",UriKind.RelativeOrAbsolute)};
            }
        }
        public static async Task<List<NHKNews>> GetNHKNews()
        {
            try
            {
                string jsonStr = await GetJsonString($"https://jpdictapi.terra-incognita.dev/api/GetNHKNews?code={JPDICT_API_KEY}");
                JsonObject jsonobj = JsonObject.Parse(jsonStr);
                var resultarritem = jsonobj["data"].GetObject()["item"].GetArray();
                List<NHKNews> res = new List<NHKNews>();
                foreach(var i in resultarritem)
                {
                    res.Add(new NHKNews() { Title = i.GetObject()["title"].GetString(), Link = new Uri(i.GetObject()["link"].GetString()), OriginalLink = new Uri(i.GetObject()["link"].GetString()), IconPath = new Uri(i.GetObject()["iconPath"].GetString()), VideoPath = new Uri(i.GetObject()["videoPath"].GetString()) });
                }
                return res;
            }
            catch
            {
                var err = new List<NHKNews>();
                for(int i=0;i<10;i++)
                {
                    err.Add(new NHKNews() { Title = "エラー", IconPath = new Uri("ms-appx:///Assets/connectionerr.png", UriKind.RelativeOrAbsolute) });
                }
                return err;
            }
        }
        public static async Task<List<NHKNews>> GetNHKEasyNews()
        {
            try
            {
                string jsonStr = await GetJsonString($"https://jpdictapi.terra-incognita.dev/api/GetNHKEasyNews?code={JPDICT_API_KEY}");
                var jsonobj = JsonArray.Parse(jsonStr);
                List<NHKNews> res = new List<NHKNews>();
                foreach (var i in jsonobj)
                {
                    string id = i.GetObject()["newsId"].GetString();
                    string img = i.GetObject()["imageUri"].GetString();
                    res.Add(new NHKNews() { Title = i.GetObject()["title"].GetString(), Link= new Uri($"https://jpdictapi.terra-incognita.dev/api/GetFormattedEasyNews?code={JPDICT_API_KEY}&id={id}&img={img}"), OriginalLink= new Uri($"https://www3.nhk.or.jp/news/easy/{id}/{id}.html"), IconPath = new Uri(i.GetObject()["imageUri"].GetString()), IsEasy = true });
                }
                return res;
            }
            catch
            {
                var err = new List<NHKNews>();
                for (int i = 0; i < 10; i++)
                {
                    err.Add(new NHKNews() { Title = "エラー", IconPath = new Uri("ms-appx:///Assets/connectionerr.png", UriKind.RelativeOrAbsolute) });
                }
                return err;
            }
        }
        public static async Task<List<NHKRadio>> GetNHKRadios(string speed)
        {
            try
            {
                string jsonStr = await GetJsonString($"https://jpdictapi.terra-incognita.dev/api/GetNHKRadio?code={JPDICT_API_KEY}&speed={speed}");
                JsonArray jsonArr = JsonArray.Parse(jsonStr);
                List<NHKRadio> result = new List<NHKRadio>();
                foreach (var i in jsonArr)
                {
                    JsonObject jsonobj = i.GetObject();
                    NHKRadio res = new NHKRadio() { Title = jsonobj["title"].GetString(), StartDate = jsonobj["startdate"].GetString(), EndDate = jsonobj["enddate"].GetString(), SoundUrl = new Uri(jsonobj["soundurl"].GetString()) };
                    result.Add(res);
                }
                return result;
            }
            catch
            {
                List<NHKRadio> result = new List<NHKRadio>();
                result.Add(new NHKRadio() { Title = "出现连接错误", StartDate = "请检查网络连接" });
                return result;
            }
        }
        public static async Task<int> GetNHKRadiosItemsCount()
        {
            try
            {
                return Int32.Parse(await GetJsonString($"https://jpdictapi.terra-incognita.dev/api/GetNHKRadio?code={JPDICT_API_KEY}&getItemsCount=true"));

            }
            catch
            {
                return 1;
            }
        }
        public static async Task<FormattedNews> GetFormattedNews(string url)
        {
            string jsonStr = await GetJsonString($"https://jpdictapi.terra-incognita.dev/api/GetFormattedNews?code={JPDICT_API_KEY}&url={url}");
            JsonObject jsonobj = JsonObject.Parse(jsonStr);
            return new FormattedNews() { Title = jsonobj["title"].GetString(), Content = jsonobj["content"].GetString(), Image = new Uri(jsonobj["image"].GetString()) };
        }
        /// <summary>
        /// Get UTC+8
        /// </summary>
        /// <returns></returns>
        public static async Task<DateTime> GetDate()
        {
            try
            {
                string datejson = await GetJsonString("http://skylarkwsp-services.azurewebsites.net/api/DateTime");
                return (DateTime.Parse(JsonObject.Parse(datejson)["datetime"].GetString()));
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public static async Task<List<string>> GetLemmatized(string sentence)
        {
            try
            {
                string jsonStr = await GetJsonString($"https://jpdict-lemmatizer.azurewebsites.net/api/lemmatized?sentence={sentence}");
                JsonArray jsonArr = JsonArray.Parse(jsonStr);
                List<string> result = new List<string>();
                foreach(var i in jsonArr)
                {
                    result.Add(i.GetString());
                }
                return result;
            }
            catch
            {
                List<string> result = new List<string>();
                result.Add(sentence);
                return result;
            }
        }
    }
}
