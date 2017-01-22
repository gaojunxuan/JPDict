using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Util
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
            if (word == "する" || word.Substring(word.Length - 2) == "する")
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
        static bool IsIchidan(string word)
        {
            word = word.Replace(" ", "").Replace("　", "");
            var okurigana = word.Substring(word.Length - 2);
            if (okurigana == "べる" || okurigana == "める" || okurigana == "える" || okurigana == "へる" || okurigana == "ける" || okurigana == "ぺる" || okurigana == "げる" || okurigana == "せる" || okurigana == "れる")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region phonetic changes
        static string PrepGodanVerbPhoneticChange(string word)
        {
            string newWord = word.Substring(0, word.Length - 1);
            if (word[word.Length - 1] == 'う' || word[word.Length - 1] == 'る' || word[word.Length - 1] == 'つ')
            {
                //nasal sound change
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
        static string PrepNegative(string word)
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
            else if (IsSuruVerb(word))
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
            else if (IsIchidan(word))
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
        static string PrepPotential(string word)
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
            else if (IsSuruVerb(word))
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
            else if (IsIchidan(word))
            {
                return word.Substring(0, word.Length - 1) + "られ";
            }
            else
            {
                return MoveToELine(word);
            }
        }
        static string PrepPassive(string word)
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
            else if (IsSuruVerb(word))
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
            else if (IsIchidan(word))
            {
                return word.Substring(0, word.Length - 1) + "られ";
            }
            else
            {
                return MoveToALine(word) + "れ";
            }
        }
        static string PrepImperative(string word)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (IsKuruVerb(word))
            {
                return "こい";
            }
            else if (IsSuruVerb(word))
            {
                return word.Substring(0,word.Length-2)+"しろ(せよ)";
            }
            else if (IsIchidan(word))
            {
                return word.Substring(0, word.Length - 1) + "ろ";
            }
            else
            {
                return MoveToELine(word);
            }
        }
        static string PrepCausative(string word)
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
            else if (IsSuruVerb(word))
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
            else if (IsIchidan(word))
            {
                return word.Substring(0, word.Length - 1) + "させ";
            }
            else
            {
                return MoveToALine(word) + "せ";
            }
        }
        static string PrepMasuForm(string word)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (IsIchidan(word))
            {
                return word.Substring(0, word.Length - 1) + "ま";
            }
            else if(IsKuruVerb(word))
            {
                if(word=="来る")
                {
                    return "きま";
                }
                return "きま";
            }
            else if(IsSuruVerb(word))
            {
                if(word== "為る")
                {
                    return "しま";
                }
                return word.Substring(0, word.Length - 2) + "しま";
            }
            else
            {
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
        #endregion
        #region get
        public static string GetTeForm(string word)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (IsIchidan(word))
            {
                return word.Substring(0, word.Length - 1) + "て";
            }
            else if (IsKuruVerb(word))
            {
                return "きて";
            }
            else if (IsSuruVerb(word))
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
                if (word[word.Length-1] == 'ぐ' || word[word.Length - 1] == 'ぬ' || word[word.Length - 1] == 'む' || word[word.Length - 1] == 'ぶ')
                {
                    return newword + "で";
                }
                else
                {
                    return newword + "て";
                }
            }
        }
        public static string GetTaForm(string word)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (IsIchidan(word))
            {
                return word.Substring(0, word.Length - 1) + "た";
            }
            else if (IsKuruVerb(word))
            {
                return "きた";
            }
            else if (IsSuruVerb(word))
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
                if (word[word.Length-1] == 'ぐ' || word[word.Length - 1] == 'ぬ' || word[word.Length - 1] == 'む' || word[word.Length - 1] == 'ぶ')
                {
                    return newword + "だ";
                }
                else
                {
                    return newword + "た";
                }
            }
        }
        public static string GetNegative(string word)
        {
            return PrepNegative(word) + "い";
        }
        public static string GetPastNegative(string word)
        {
            return PrepNegative(word) + "かった";
        }
        public static string GetEbaForm(string word)
        {
            if (IsIchidan(word))
            {
                return word.Substring(0, word.Length - 1) + "れば";
            }
            else if (IsKuruVerb(word))
            {
                return "くれば";
            }
            else if (IsSuruVerb(word))
            {
                return word.Substring(0, word.Length - 1) + "れば";
            }
            else
            {
                return MoveToELine(word) + "ば";
            }
        }
        public static string GetPotential(string word)
        {
            return PrepPotential(word) + "る";
        }
        public static string GetNegativePotential(string word)
        {
            return PrepPotential(word) + "ない";
        }
        public static string GetPassive(string word)
        {
            return PrepPassive(word) + "る";
        }
        public static string GetNegativePassive(string word)
        {
            return PrepPassive(word) + "ない";
        }
        public static string GetCausative(string word)
        {
            return PrepCausative(word) + "る";
        }
        public static string GetNegativeCausative(string word)
        {
            return PrepCausative(word) + "ない";
        }
        public static string GetImperative(string word)
        {
            return PrepImperative(word);
        }
        public static string GetNegativeImperative(string word)
        {
            return word + "な";
        }
        public static string GetVolitional(string word)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (IsIchidan(word))
            {
                return word.Substring(0, word.Length - 1) + "よう";
            }
            else if (IsKuruVerb(word))
            {
                return "こよう";
            }
            else if(IsSuruVerb(word))
            {
                if (word == "為る"||word=="する")
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
        public static string GetMasuForm(string word)
        {
            return PrepMasuForm(word) + "す";
        }
        public static string GetMasuNegative(string word)
        {
            return PrepMasuForm(word) + "せん";
        }

        #endregion
    }
}
