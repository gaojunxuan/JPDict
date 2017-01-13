using JapaneseDict.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.OnlineService
{
    public static class OnlineQueryEngine
    {
        /// <summary>
        /// Query online
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static async Task<ObservableCollection<OnlineDict>> Query(string keyword)
        {
            var res = new ObservableCollection<OnlineDict>();
            res.Add(new OnlineDict() { JpChar = keyword, Explanation = await OnlineService.JsonHelper.GetTranslateResult(keyword, "jp", "zh") });
            return res;
        }
        public static async Task<ObservableCollection<OnlineDict>> Query(string keyword, string originLang, string targetLang)
        {
            var res = new ObservableCollection<OnlineDict>();
            res.Add(new OnlineDict() { JpChar = keyword, Explanation = await OnlineService.JsonHelper.GetTranslateResult(keyword, originLang, targetLang) });
            return res;
        }
    }
}
