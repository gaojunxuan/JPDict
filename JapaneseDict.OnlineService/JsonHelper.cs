using JapaneseDict.Models;
using JapaneseDict.Util;
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

namespace JapaneseDict.OnlineService
{
    public static class JsonHelper
    {
      
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
            catch
            {
                return "出现连接错误";
            }
        }
        //public static async Task<EverydaySentence> GetEverydaySentence(DateTime date,int index)
        //{
        //    try
        //    {
        //        string jsonStr = await GetJsonString("http://skylarkwsp-services.azurewebsites.net/api/EverydayJapanese?datestr=" + date.ToString("yyyyMMdd"));
        //        JsonObject resultobj = JsonObject.Parse(jsonStr);
        //        return new EverydaySentence() { JpText= resultobj["sentence"].GetString(), CnText= resultobj["trans"].GetString(), AudioUri=new Uri(resultobj["audio"].GetString()),NotesOnText= resultobj["sentencePoint"].GetString(), Author= resultobj["creator"].GetString(),BackgroundImage=new BitmapImage(new Uri($"ms-appx:///Assets/EverydaySentenceBackground/{index}.jpg",UriKind.RelativeOrAbsolute)) };
        //    }
        //    catch(Exception)
        //    {
        //        return new EverydaySentence() { JpText = "出现错误", CnText = "请确认您是否已连接到互联网", BackgroundImage = new BitmapImage(new Uri($"ms-appx:///Assets/EverydaySentenceBackground/{index}.jpg", UriKind.RelativeOrAbsolute)) };
        //    }
            
        //}
        public static async Task<EverydaySentence> GetEverydaySentence(int index)
        {
            try
            {
                //string jsonStr = await GetJsonString("http://skylarkwsp-services.azurewebsites.net/api/EverydayJapanese?index=" +index);
                string jsonStr = await GetJsonString("http://api.skylark-workshop.xyz/api/GetDailySentence?code=/BiWp6KIaa4DJ5fP5n4CfG6KxD9DRHqc2Wwosiw2tKAoPADvDtizEw==&index=" + index);
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
                string jsonStr = await GetJsonString("http://api.skylark-workshop.xyz/api/GetNHKNews?code=G6TCeDVc9HGW8TU7C6pGEv2Ivfuoxy/aY22TSaLNa/9LF/y9WfLJrQ==");
                JsonObject jsonobj = JsonObject.Parse(jsonStr);
                var resultarritem = jsonobj["data"].GetObject()["item"].GetArray()[index];
                NHKNews res = new NHKNews() {Title=resultarritem.GetObject()["title"].GetString(),Link= new Uri(resultarritem.GetObject()["link"].GetString()),IconPath=new Uri(resultarritem.GetObject()["iconPath"].GetString()),VideoPath=new Uri(resultarritem.GetObject()["videoPath"].GetString()) };
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
                string jsonStr = await GetJsonString("http://api.skylark-workshop.xyz/api/GetNHKNews?code=G6TCeDVc9HGW8TU7C6pGEv2Ivfuoxy/aY22TSaLNa/9LF/y9WfLJrQ==");
                JsonObject jsonobj = JsonObject.Parse(jsonStr);
                var resultarritem = jsonobj["data"].GetObject()["item"].GetArray();
                List<NHKNews> res = new List<NHKNews>();
                foreach(var i in resultarritem)
                {
                    res.Add(new NHKNews() { Title = i.GetObject()["title"].GetString(), Link = new Uri(i.GetObject()["link"].GetString()), IconPath = new Uri(i.GetObject()["iconPath"].GetString()), VideoPath = new Uri(i.GetObject()["videoPath"].GetString()) });
                }
                return res;
            }
            catch
            {
                var err = new List<NHKNews>();
                for(int i=0;i<10;i++)
                {
                    err.Add(new NHKNews() { Title = "出现错误", IconPath = new Uri("ms-appx:///Assets/connectionerr.png", UriKind.RelativeOrAbsolute) });
                }
                return err;
            }
        }
        public static async Task<NHKRadios> GetNHKRadios(int index, string speed)
        {
            try
            {
                string jsonStr = await GetJsonString($"http://api.skylark-workshop.xyz/api/GetNHKRadio?code=Lwgwi3BFqmOzq/C7SIAaN1kK/GpiDtppAfr0X3MXklpp057unrBmHQ==&speed={speed}&index={index}");
                JsonObject jsonobj = JsonObject.Parse(jsonStr);
                NHKRadios res = new NHKRadios() { Title = jsonobj["title"].GetString(), StartDate = jsonobj["startdate"].GetString(), EndDate = jsonobj["enddate"].GetString(), SoundUrl = new Uri(jsonobj["soundurl"].GetString()) };
                return res;
            }
            catch
            {
                return new NHKRadios() { Title = "出现连接错误", StartDate = "请确认您是否已连接到互联网" };
            }
        }
        public static async Task<int> GetNHKRadiosItemsCount()
        {
            try
            {
                return Int32.Parse(await GetJsonString("http://api.skylark-workshop.xyz/api/GetNHKRadio?code=Lwgwi3BFqmOzq/C7SIAaN1kK/GpiDtppAfr0X3MXklpp057unrBmHQ==&getItemsCount=true"));

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
