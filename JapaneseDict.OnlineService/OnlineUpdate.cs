using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JapaneseDict.QueryEngine;
using static JapaneseDict.QueryEngine.QueryEngine;

namespace JapaneseDict.OnlineService
{
    public class OnlineUpdate
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
            catch (HttpRequestException)
            {
                return "";
            }
            catch
            {
                return "";
            }
        }
        public static async Task<IEnumerable<UpdateDict>> GetAllUpdates()
        {
            string jsonStr = await GetJsonString($"http://jpdictbackend.azurewebsites.net/getupdates");
            var deserialized = JsonConvert.DeserializeObject<List<UpdateDict>>(jsonStr);
            return deserialized;
        }
        public static async Task ApplyUpdate(IEnumerable<UpdateDict> source)
        {
            await Task.Run(() => 
            {
                foreach (var i in source)
                {
                    MainDictQueryEngine.Add(i.JpChar, i.Defination, i.Reading, i.AutoId);
                }
            });
        }
    }
    public class UpdateDict
    {
        public int ID { get; set; }
        public int AutoId { get; set; }
        public string JpChar { get; set; }
        public string Reading { get; set; }
        public string Defination { get; set; }
        public string Comment { get; set; }
        public string Category { get; set; }
    }
}
