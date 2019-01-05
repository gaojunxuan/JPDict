using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibNMeCab.UWP;
using NMeCab;
using Windows.Storage;

namespace JapaneseDict.QueryEngine
{
    public class Lemmatizer
    {
        static int[] outputPos1 = { 10, 31, 38, 39, 36, 47, 26, 59, 60, 34, 35, 67, 66, 58, 37, 68 };
        static int[] outputPos2 = { 10, 31, 38, 39, 36, 47, 26, 59, 60, 34, 35, 67, 66, 61, 11, 12, 32, 33, 15, 25, 2, 58, 37, 68 };
        static string path = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "dic\\ipadic");
        static MeCabParam param = new MeCabParam() { DicDir = path };
        static MeCabTagger t = MeCabTagger.Create(param);
        public static List<(string,string,string)> GetLemmatized(string sentence)
        {
            int[] outputPos = null;
            if (StorageHelper.GetSetting<int>("LemmatizerMode") == 0)
            {
                outputPos = outputPos1;
            }
            else if (StorageHelper.GetSetting<int>("LemmatizerMode") == 1)
            {
                outputPos = outputPos2;
            }
            List <(string, string, string)> err = new List<(string, string, string)>();
            err.Add((sentence,"",""));
            try
            {
                if (!string.IsNullOrWhiteSpace(sentence))
                {
                    MeCabNode node = t.ParseToNode(sentence);
                    List<(string, string, string)> lemmatized = new List<(string, string, string)>();
                    while (node != null)
                    {
                        if (node.CharType > 0)
                        {
                            if (outputPos == null || (outputPos != null && outputPos.Contains(node.PosId)))
                            {
                                var features = node.Feature.Split(',');
                                if (node.Surface == "死ね")
                                    lemmatized.Add(("死ぬ", "しぬ", "動詞"));
                                else if (node.Surface == "しね")
                                    lemmatized.Add(("しぬ", "しぬ", "動詞"));
                                else
                                {
                                    string str = features[features.Count() - 3];
                                    if (str != "ない" && str != "する")
                                    {
                                        if (node.PosId == 33 && str == "いる")
                                            lemmatized.Add(("居る", features[features.Count() - 2], features[0]));
                                        else if (node.PosId == 37)
                                            lemmatized.Add((str + "ない", features[features.Count() - 2] + "ナイ", features[0]));
                                        else
                                            lemmatized.Add((str, features[features.Count() - 2], features[0]));                                       
                                    }
                                }
                            }
                        }
                        node = node.Next;
                    }
                    return lemmatized.Distinct().ToList();
                }
                return err;
            }
            catch
            {
                return err;
            }
        }
    }
}
