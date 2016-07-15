using JapaneseDict.Models;
using JapaneseDict.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Security.Cryptography.Core;
using Windows.UI.Xaml.Media.Imaging;

namespace JapaneseDict.OnlineService
{
    public static class JsonHelper
    {
      
        private static async Task<string> GetJsonString(string uri)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var responseText = await response.Content.ReadAsStringAsync();
                return responseText; 
            }
            catch(HttpRequestException)
            {
                return "";
            }
            catch
            {
                return "";
            }
        }
        const string appid = "20160211000011632";
        const string key = "NvduVsfjpNEclI03Sbei";

        public static async Task<String> GetTranslateResult(string input, string sourcelang, string targetlang)
        {
            try
            {
                var random = new Random(1000000000).Next();
                var sign = Md5Helper.Encode(appid + input + random + key);
                string jsonStr = await GetJsonString("http://api.fanyi.baidu.com/api/trans/vip/translate?q=" + $"{Uri.EscapeDataString(input)}&from={sourcelang}&to={targetlang}&appid={appid}&salt={random}&sign={sign}");
                JsonObject jsonobj = JsonObject.Parse(jsonStr);
                return jsonobj["trans_result"].GetArray().GetObjectAt(0).GetObject()["dst"].GetString(); 
            }
            catch(HttpRequestException)
            {
                return "出现连接错误";
            }
            catch(Exception)
            {
                return "出现连接错误";
            }
        }
        public static async Task<EverydaySentence> GetEverydaySentence(DateTime date,int index)
        {
            try
            {
                string jsonStr = await GetJsonString("http://skylarkwsp-services.azurewebsites.net/api/EverydayJapanese?datestr=" + date.ToString("yyyyMMdd"));
                JsonObject resultobj = JsonObject.Parse(jsonStr);
                return new EverydaySentence() { JpText= resultobj["sentence"].GetString(), CnText= resultobj["trans"].GetString(), AudioUri=new Uri(resultobj["audio"].GetString()),NotesOnText= resultobj["sentencePoint"].GetString(), Author= resultobj["creator"].GetString(),BackgroundImage=new BitmapImage(new Uri($"ms-appx:///Assets/EverydaySentenceBackground/{index}.jpg",UriKind.RelativeOrAbsolute)) };
            }
            catch(Exception)
            {
                return new EverydaySentence() { JpText = "出现错误", CnText = "请确认您是否已连接到互联网", BackgroundImage = new BitmapImage(new Uri($"ms-appx:///Assets/EverydaySentenceBackground/{index}.jpg", UriKind.RelativeOrAbsolute)) };
            }
            
        }
        public static async Task<EverydaySentence> GetEverydaySentence(int index)
        {
            try
            {
                string jsonStr = await GetJsonString("http://skylarkwsp-services.azurewebsites.net/api/EverydayJapanese?index=" +index);
                JsonObject resultobj = JsonObject.Parse(jsonStr);
                return new EverydaySentence() { JpText = resultobj["sentence"].GetString(), CnText = resultobj["trans"].GetString(), AudioUri = new Uri(resultobj["audio"].GetString()), NotesOnText = resultobj["sentencePoint"].GetString(), Author = resultobj["creator"].GetString(), BackgroundImage = new BitmapImage(new Uri($"ms-appx:///Assets/EverydaySentenceBackground/{index}.jpg", UriKind.RelativeOrAbsolute)) };
            }
            catch (Exception)
            {
                return new EverydaySentence() { JpText = "出现错误", CnText = "请确认您是否已连接到互联网", BackgroundImage = new BitmapImage(new Uri($"ms-appx:///Assets/EverydaySentenceBackground/{index}.jpg", UriKind.RelativeOrAbsolute)) };
            }
        }
        public static async Task<NHKNews> GetNHKNews(int index)
        {
            try
            {
                string jsonStr = await GetJsonString("http://skylarkwsp-services.azurewebsites.net/api/NHKNews");
                JsonObject jsonobj = JsonObject.Parse(jsonStr);
                var resultarritem = jsonobj["data"].GetObject()["item"].GetArray()[index];
                NHKNews res = new NHKNews() {Title=resultarritem.GetObject()["title"].GetString(),Link= new Uri(resultarritem.GetObject()["link"].GetString()),IconPath=new Uri(resultarritem.GetObject()["iconPath"].GetString()),VideoPath=new Uri(resultarritem.GetObject()["videoPath"].GetString()) };
                return res;
            }
            catch (HttpRequestException)
            {
                return new NHKNews() { Title = "出现连接错误",IconPath=new Uri("ms-appx:///Assets/connectionerr.png",UriKind.RelativeOrAbsolute)};
            }
            catch (Exception)
            {
                return new NHKNews() { Title = "出现连接错误",IconPath=new Uri("ms-appx:///Assets/connectionerr.png", UriKind.RelativeOrAbsolute) };
            }
        }
        public static async Task<NHKRadios> GetNHKRadios(int index, string speed)
        {
            try
            {
                string jsonStr = await GetJsonString($"http://skylarkwsp-services.azurewebsites.net/api/NHKListening?speed={speed}&index={index}");
                JsonObject jsonobj = JsonObject.Parse(jsonStr);
                NHKRadios res = new NHKRadios() { Title = jsonobj["title"].GetString(), StartDate = jsonobj["startdate"].GetString(), EndDate = jsonobj["enddate"].GetString(), SoundUrl = new Uri(jsonobj["soundurl"].GetString()) };
                return res;
            }
            catch (HttpRequestException)
            {
                return new NHKRadios() { Title = "出现连接错误", StartDate = "请确认您是否已连接到互联网" };
            }
            catch (Exception)
            {
                return new NHKRadios() { Title = "出现连接错误", StartDate = "请确认您是否已连接到互联网" };
            }
        }
        public static async Task<int> GetNHKRadiosItemsCount()
        {
            try
            {
                return Int32.Parse(await GetJsonString("http://skylarkwsp-services.azurewebsites.net/api/NHKListening?getItemsCount=true"));

            }
            catch
            {
                return 1;
            }
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
    }
}
