using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.GUI.Helpers
{
    public class VerbConjugationHelper
    {
        static List<string> hira = new List<string>() { "わ", "い", "う", "え", "お", "か", "き", "く", "け", "こ", "さ", "し", "す", "せ", "そ", "た", "ち", "つ", "て", "と", "な", "に", "ぬ", "ね", "の", "は", "ひ", "ふ", "へ", "ほ", "ま", "み", "む", "め", "も", "や", "い", "ゆ", "え", "よ", "ら", "り", "る", "れ", "ろ", "わ", "い", "う", "え", "を", "ん", "が", "ぎ", "ぐ", "げ", "ご", "ざ", "じ", "ず", "ぜ", "ぞ", "だ", "ぢ", "づ", "で", "ど", "ば", "び", "ぶ", "べ", "ぼ", "ぱ", "ぴ", "ぷ", "ぺ", "ぽ" };
        #region check
        static bool IsKuruVerb(string word)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (word == "くる" || word == "来る")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static bool IsSuruVerb(string word)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (word.Length >= 2 && (word == "する" || word.Substring(word.Length - 2) == "する"))
            {
                return true;
            }
            else if (word == "為る")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static bool IsIchidan(string word,string tag)
        {
            //word = word.Replace(" ", "").Replace("　", "");
            ////var okurigana = word.Substring(word.Length - 2);
            if(tag.Contains("一 ]") || tag.Contains("[ 一段動詞 ]"))
            {
                return true;
            }
            //if (okurigana == "べる" || okurigana == "める" || okurigana == "える" || okurigana == "へる" || okurigana == "ける" || okurigana == "ぺる" || okurigana == "げる" || okurigana == "せる" || okurigana == "れる")
            //{
            //    if(tag.Contains("一 ]")||tag.Contains("[ 一段動詞 ]"))
            //    {
            //        return true;
            //    }
            //    return false;
            //}
            return false;
        }
        //いらっしゃる、ござる、くださる、なさる、おっしゃる
        static bool IsSpecialKeigo(string word)
        {
            if (word == "いらっしゃる" | word == "ござる" | word == "御座る" | word == "なさる" | word == "為さる" | word == "くださる" | word == "下さる" | word == "おっしゃる" | word == "仰しゃる")
            {
                return true;
            }
            return false;
        }
        //問う、訪う、請う、乞う
        static bool IsUOnbin(string word)
        {
            if(word== "問う"|word== "訪う"|word== "請う" | word== "乞う" | word=="とう"|word=="こう")
            {
                return true;
            }
            return false;
        }
        public static bool IsNegative(string word)
        {
            return word.Length > 2 && word.EndsWith("ない");
        }
        public static bool IsCausative(string word)
        {
            return word.Length > 2 && word.EndsWith("せる");
        }
        public static bool IsPassive(string word)
        {
            return word.Length > 2 && word.EndsWith("れる");
        }
        public static bool IsMasu(string word)
        {
            return word.Length > 2 && word.EndsWith("ます");
        }
        static char[] edan = { 'え','け','せ','て','ね','へ','め','え','れ','え' };
        public static bool IsPotential(string word)
        {
            return word.Length > 2 && (word.EndsWith("られる") || (word.EndsWith("る") && edan.Contains(word[word.Length - 2])));
        }
        #endregion
        #region phonetic changes
        static string PrepGodanVerbPhoneticChange(string word)
        {
            string newWord = word.Substring(0, word.Length - 1);
            if (word[word.Length - 1] == 'う' || word[word.Length - 1] == 'る' || word[word.Length - 1] == 'つ')
            {
                if (IsUOnbin(word))
                    newWord = newWord + "う";
                else   
                    newWord = newWord + "っ";
            }
            else if (word[word.Length - 1] == 'ぬ' || word[word.Length - 1] == 'む' || word[word.Length - 1] == 'ぶ')
            {
                //nasal sound change
                newWord = newWord + "ん";
            }
            else if (word[word.Length - 1] == 'く')
            {
                if (word[word.Length - 2] == 'い' || word[word.Length - 2] == 'ゆ' || word[word.Length - 2] == '行' || word[word.Length - 2] == '逝')
                {
                    newWord = newWord + "っ";
                }
                else
                {
                    newWord = newWord + "い";
                }
            }
            else if (word[word.Length - 1] == 'ぐ')
            {
                newWord = newWord + "い";
            }
            else
            {
                newWord = newWord + "し";
            }
            return newWord;
        }
        #endregion
        #region prep
        static string PrepNegative(string word,string tag)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (IsKuruVerb(word))
            {
                if (word == "来る")
                {
                    return "こな";
                }
                else
                {
                    return "こな";
                }
            }
            else if (IsSuruVerb(word) && tag.Contains("サ"))
            {
                if (word == "為る")
                {
                    return "しな";
                }
                else
                {
                    return word.Substring(0, word.Length - 2) + "しな";
                }
            }
            else if (IsIchidan(word,tag))
            {
                return word.Substring(0, word.Length - 1) + "な";
            }
            else
            {
                return MoveToALine(word) + "な";
            }
        }
        static string PrepTeTa(string word)
        {
            return PrepGodanVerbPhoneticChange(word);
        }
        static string PrepPotential(string word,string tag)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (IsKuruVerb(word))
            {
                if (word == "来る")
                {
                    return "こられ";
                }
                else
                {
                    return "こられ";
                }
            }
            else if (IsSuruVerb(word) && tag.Contains("サ"))
            {
                if (word == "為る")
                {
                    return "でき";
                }
                else
                {
                    return word.Substring(0, word.Length - 2) + "でき";
                }
            }
            else if (IsIchidan(word,tag))
            {
                return word.Substring(0, word.Length - 1) + "られ";
            }
            else
            {
                return MoveToELine(word);
            }
        }
        static string PrepPassive(string word,string tag)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (IsKuruVerb(word))
            {
                if (word == "来る")
                {
                    return "こられ";
                }
                else
                {
                    return "こられ";
                }
            }
            else if (IsSuruVerb(word) && tag.Contains("サ"))
            {
                if (word == "為る")
                {
                    return "され";
                }
                else
                {
                    return word.Substring(0, word.Length - 2) + "され";
                }
            }
            else if (IsIchidan(word,tag))
            {
                return word.Substring(0, word.Length - 1) + "られ";
            }
            else
            {
                return MoveToALine(word) + "れ";
            }
        }
        static string PrepImperative(string word,string tag)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (IsKuruVerb(word))
            {
                return "こい";
            }
            else if (IsSuruVerb(word) && tag.Contains("サ"))
            {
                return word.Substring(0, word.Length - 2) + "しろ(せよ)";
            }
            else if (IsIchidan(word,tag))
            {
                return word.Substring(0, word.Length - 1) + "ろ";
            }
            else
            {
                if (IsSpecialKeigo(word))
                {
                    return word.Substring(0, word.Length - 1) + "い";
                }
                return MoveToELine(word);
            }
        }
        static string PrepCausative(string word,string tag)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (IsKuruVerb(word))
            {
                if (word == "来る")
                {
                    return "こられ";
                }
                else
                {
                    return "こられ";
                }
            }
            else if (IsSuruVerb(word) && tag.Contains("サ"))
            {
                if (word == "為る")
                {
                    return "させ";
                }
                else
                {
                    return word.Substring(0, word.Length - 2) + "させ";
                }
            }
            else if (IsIchidan(word,tag))
            {
                return word.Substring(0, word.Length - 1) + "させ";
            }
            else
            {
                return MoveToALine(word) + "せ";
            }
        }
        static string PrepMasuForm(string word,string tag)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (IsIchidan(word,tag))
            {
                return word.Substring(0, word.Length - 1) + "ま";
            }
            else if (IsKuruVerb(word))
            {
                if (word == "来る")
                {
                    return "きま";
                }
                return "きま";
            }
            else if (IsSuruVerb(word) && tag.Contains("サ"))
            {
                if (word == "為る")
                {
                    return "しま";
                }
                return word.Substring(0, word.Length - 2) + "しま";
            }
            else
            {
                if (IsSpecialKeigo(word))
                {
                    return word.Substring(0, word.Length - 1) + "いま";
                }
                return MoveToILine(word) + "ま";
            }
        }
        static string MoveToALine(string word)
        {
            string okuri = word.Substring(word.Length - 1);
            okuri = hira[hira.IndexOf(okuri) - 2];
            return word.Substring(0, word.Length - 1) + okuri;
        }
        static string MoveToILine(string word)
        {
            string okuri = word.Substring(word.Length - 1);
            okuri = hira[hira.IndexOf(okuri) - 1];
            return word.Substring(0, word.Length - 1) + okuri;
        }
        static string MoveToELine(string word)
        {
            string okuri = word.Substring(word.Length - 1);
            okuri = hira[hira.IndexOf(okuri) + 1];
            return word.Substring(0, word.Length - 1) + okuri;
        }
        static string MoveToOLine(string word)
        {
            string okuri = word.Substring(word.Length - 1);
            okuri = hira[hira.IndexOf(okuri) + 2];
            return word.Substring(0, word.Length - 1) + okuri;
        }
        static string MoveFromAToU(string word)
        {
            string okuri = word.Substring(word.Length - 1);
            okuri = hira[hira.IndexOf(okuri) + 2];
            return word.Substring(0, word.Length - 1) + okuri;
        }
        static string MoveFromIToU(string word)
        {
            string okuri = word.Substring(word.Length - 1);
            okuri = hira[hira.IndexOf(okuri) + 1];
            return word.Substring(0, word.Length - 1) + okuri;
        }
        static string MoveFromEToU(string word)
        {
            string okuri = word.Substring(word.Length - 1);
            okuri = hira[hira.IndexOf(okuri) - 1];
            return word.Substring(0, word.Length - 1) + okuri;
        }
        #endregion
        #region get
        public static string GetTeForm(string word,string tag)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (IsIchidan(word,tag))
            {
                return word.Substring(0, word.Length - 1) + "て";
            }
            else if (IsKuruVerb(word))
            {
                return "きて";
            }
            else if (IsSuruVerb(word) && tag.Contains("サ"))
            {
                if (word == "為る")
                {
                    return "して";
                }
                return word.Substring(0, word.Length - 2) + "して";
            }
            else
            {
                var newword = PrepGodanVerbPhoneticChange(word);
                if (word[word.Length - 1] == 'ぐ' || word[word.Length - 1] == 'ぬ' || word[word.Length - 1] == 'む' || word[word.Length - 1] == 'ぶ')
                {
                    return newword + "で";
                }
                else
                {
                    return newword + "て";
                }
            }
        }
        public static string GetTaForm(string word,string tag)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (IsIchidan(word,tag))
            {
                return word.Substring(0, word.Length - 1) + "た";
            }
            else if (IsKuruVerb(word))
            {
                return "きた";
            }
            else if (IsSuruVerb(word) && tag.Contains("サ"))
            {
                if (word == "為る")
                {
                    return "した";
                }
                return word.Substring(0, word.Length - 2) + "した";
            }
            else
            {
                var newword = PrepGodanVerbPhoneticChange(word);
                if (word[word.Length - 1] == 'ぐ' || word[word.Length - 1] == 'ぬ' || word[word.Length - 1] == 'む' || word[word.Length - 1] == 'ぶ')
                {
                    return newword + "だ";
                }
                else
                {
                    return newword + "た";
                }
            }
        }
        public static string GetNegative(string word,string tag)
        {
            if(word=="ある"||word=="有る")
            {
                return "ない";
            }
            else
                return PrepNegative(word, tag) + "い";
        }
        public static string GetPastNegative(string word, string tag)
        {
            return PrepNegative(word,tag) + "かった";
        }
        public static string GetEbaForm(string word, string tag)
        {
            if (IsIchidan(word, tag))
            {
                return word.Substring(0, word.Length - 1) + "れば";
            }
            else if (IsKuruVerb(word))
            {
                return "くれば";
            }
            else if (IsSuruVerb(word) && tag.Contains("サ"))
            {
                return word.Substring(0, word.Length - 1) + "れば";
            }
            else
            {
                return MoveToELine(word) + "ば";
            }
        }
        public static string GetPotential(string word, string tag)
        {
            if (word == "ある" || word == "有る")
            {
                return "あり得る";
            }
            return PrepPotential(word, tag) + "る";
        }
        public static string GetNegativePotential(string word, string tag)
        {
            return PrepPotential(word, tag) + "ない";
        }
        public static string GetPassive(string word, string tag)
        {
            return PrepPassive(word, tag) + "る";
        }
        public static string GetNegativePassive(string word, string tag)
        {
            if (word == "ある" || word == "有る")
            {
                return "あり得ない";
            }
            return PrepPassive(word, tag) + "ない";
        }
        public static string GetCausative(string word, string tag)
        {
            return PrepCausative(word, tag) + "る";
        }
        public static string GetNegativeCausative(string word, string tag)
        {
            return PrepCausative(word, tag) + "ない";
        }
        public static string GetImperative(string word, string tag)
        {
            return PrepImperative(word, tag);
        }
        public static string GetNegativeImperative(string word)
        {
            return word + "な";
        }
        public static string GetVolitional(string word, string tag)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (IsIchidan(word, tag))
            {
                return word.Substring(0, word.Length - 1) + "よう";
            }
            else if (IsKuruVerb(word))
            {
                return "こよう";
            }
            else if (IsSuruVerb(word) && tag.Contains("サ"))
            {
                if (word == "為る" || word == "する")
                {
                    return "しよう(そう)";
                }
                else
                {
                    return word.Substring(0, word.Length - 2) + "しよう(そう)";
                }
            }
            else
            {
                return MoveToOLine(word) + "う";
            }
        }
        public static string GetMasuForm(string word,string tag)
        {
            return PrepMasuForm(word, tag) + "す";
        }
        public static string GetMasuNegative(string word,string tag)
        {
            return PrepMasuForm(word, tag) + "せん";
        }

        #endregion
        #region convert
        public static string FromNegativeToOriginal(string word)
        {
            string wordroot = word.Substring(0, word.Length - 2);
            string okurigana = wordroot.Substring(wordroot.Length - 1);
            if (okurigana == "べ" || okurigana == "め" || okurigana == "え" || okurigana == "へ" || okurigana == "け" || okurigana == "ぺ" || okurigana == "げ" || okurigana == "せ" || okurigana == "れ")
                return word.Substring(0, word.Length - 2) + "る";
            else if(word=="こない"||word=="来ない")
            {
                return "くる";
            }
            else if(word.EndsWith("しない"))
            {
                return word.Substring(0, word.Length - 3);
            }
            else
            {
                return MoveFromAToU(wordroot);
            }
        }
        public static string FromCausativeToOriginal(string word)
        {
            string wordroot = word.Substring(0, word.Length - 2);
            string okurigana = wordroot.Substring(wordroot.Length - 2);
            if (okurigana == "べさ" || okurigana == "めさ" || okurigana == "えさ" || okurigana == "へさ" || okurigana == "けさ" || okurigana == "ぺさ" || okurigana == "げさ" || okurigana == "せさ" || okurigana == "れさ")
                return word.Substring(0, word.Length - 3) + "る";
            else if (word == "こさせる" || word == "来させる")
            {
                return "くる";
            }
            else if (word.EndsWith("させる"))
            {
                return word.Substring(0, word.Length - 3);
            }
            else
            {
                return MoveFromAToU(wordroot);
            }
        }
        public static string FromPassiveToOriginal(string word)
        {
            string wordroot = word.Substring(0, word.Length - 2);
            string okurigana = wordroot.Substring(wordroot.Length - 2);
            if (okurigana == "べら" || okurigana == "めら" || okurigana == "えら" || okurigana == "へら" || okurigana == "けら" || okurigana == "ぺら" || okurigana == "げら" || okurigana == "せら" || okurigana == "れら")
                return word.Substring(0, word.Length - 3) + "る";
            else if (word == "こられる" || word == "来られる")
            {
                return "くる";
            }
            else
            {
                return MoveFromAToU(wordroot);
            }
        }
        public static string FromMasuToOriginal(string word)
        {
            string wordroot = word.Substring(0, word.Length - 2);
            string okurigana = wordroot.Substring(wordroot.Length - 1);
            if (okurigana == "べ" || okurigana == "め" || okurigana == "え" || okurigana == "へ" || okurigana == "け" || okurigana == "ぺら" || okurigana == "げ" || okurigana == "せ" || okurigana == "れ")
                return word.Substring(0, word.Length - 2) + "る";
            else if (word == "きます" || word == "来ます")
            {
                return "くる";
            }
            else if(word.EndsWith("します"))
            {
                if (word == "します")
                    return "する";
                else
                    return word.Substring(0, word.Length - 4);
            }
            else
            {
                return MoveFromIToU(wordroot);
            }
        }
        public static string FromPotentialToOrigianl(string word)
        {
            if (word.EndsWith("られる") && word.Length > 3)
                return word.TrimEnd("られる".ToCharArray()) + "る";
            else
            {
                string stem = word.TrimEnd('る');
                return MoveFromEToU(stem);
            }
        }
        #endregion
    }
}
