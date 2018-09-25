using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Debug.WriteLine(VerbConjugationHelper.PrepNegative("くる"));
            //Debug.WriteLine(VerbConjugationHelper.PrepNegative("たべる"));
            //Debug.WriteLine(VerbConjugationHelper.PrepNegative("会う"));
            //Class2.ReplaceAll();
            Class2.Do2();
            Console.ReadLine();
        }
    }
    public class KanaReciteHelper
    {
        static string[,] _hiragana = new string[11, 5] { { "あ", "い", "う", "え", "お" }, { "か", "き", "く", "け", "こ" }, { "さ", "し", "す", "せ", "そ" }, { "た", "ち", "つ", "て", "と" }, { "な", "に", "ぬ", "ね", "の" }, { "は", "ひ", "ふ", "へ", "ほ" }, { "ま", "み", "む", "め", "も" }, { "や", "", "ゆ", "", "よ" }, { "ら", "り", "る", "れ", "ろ" }, { "わ", "", "", "", "を" }, { "ん", "", "", "", "" } };
        static string[] _hiraganaPlain = new string[] { "あ", "い", "う", "え", "お", "か", "き", "く", "け", "こ", "さ", "し", "す", "せ", "そ", "た", "ち", "つ", "て", "と", "な", "に", "ぬ", "ね", "の", "は", "ひ", "ふ", "へ", "ほ", "ま", "み", "む", "め", "も", "や", "", "ゆ", "", "よ", "ら", "り", "る", "れ", "ろ", "わ", "", "", "", "を", "ん", "", "", "", "" };

        static string[,] _katakana = new string[11, 5] { { "ア", "イ", "ウ", "エ", "オ" }, { "カ", "キ", "ク", "ケ", "コ" }, { "サ", "シ", "ス", "セ", "ソ" }, { "タ", "チ", "ツ", "テ", "ト" }, { "ナ", "ニ", "ヌ", "ネ", "ノ" }, { "ハ", "ヒ", "フ", "ヘ", "ホ" }, { "マ", "ミ", "ム", "メ", "モ" }, { "ヤ", "", "ユ", "", "ヨ" }, { "ラ", "リ", "ル", "レ", "ロ" }, { "ワ", "", "", "", "ヲ" }, { "ン", "", "", "", "" } };
        static string[] _katakanaPlain = new string[] { "ア", "イ", "ウ", "エ", "オ", "カ", "キ", "ク", "ケ", "コ", "サ", "シ", "ス", "セ", "ソ", "タ", "チ", "ツ", "テ", "ト", "ナ", "ニ", "ヌ", "ネ", "ノ", "ハ", "ヒ", "フ", "ヘ", "ホ", "マ", "ミ", "ム", "メ", "モ", "ヤ", "", "ユ", "", "ヨ", "ラ", "リ", "ル", "レ", "ロ", "ワ", "", "", "", "ヲ", "ン", "", "", "", "" };

        static string[,] _romaji = new string[11, 5] { { "a", "i", "u", "e", "o" }, { "ka", "ki", "ku", "ke", "ko" }, { "sa", "shi", "su", "se", "so" }, { "ta", "chi", "tsu", "te", "to" }, { "na", "ni", "nu", "ne", "no" }, { "ha", "hi", "fu", "he", "ho" }, { "ma", "mi", "mu", "me", "mo" }, { "ya", "", "yu", "", "yo" }, { "ra", "ri", "ru", "re", "ro" }, { "wa", "", "", "", "o" }, { "n", "", "", "", "" } };
        static string[] _romajiPlain = new string[] { "a", "i", "u", "e", "o", "ka", "ki", "ku", "ke", "ko", "sa", "shi", "su", "se", "so", "ta", "chi", "tsu", "te", "to", "na", "ni", "nu", "ne", "no", "ha", "hi", "fu", "he", "ho", "ma", "mi", "mu", "me", "mo", "ya", "", "yu", "", "yo", "ra", "ri", "ru", "re", "ro", "wa", "", "", "", "o", "n", "", "", "", "" };

        static char[] _kanaconsonant = new char[] { 'a', 'k', 's', 't', 'n', 'h', 'm', 'y', 'r', 'w' };
        public static KeyValuePair<string, string>[] GetLineHiragana(char consonant)
        {
            int index = Array.IndexOf(_kanaconsonant, consonant);
            KeyValuePair<string, string>[] res = new KeyValuePair<string, string>[5];
            for (int i = 0; i < 5; i++)
            {
                res[i] = new KeyValuePair<string, string>(_hiragana[index, i], _romaji[index, i]);
            }
            return res;
        }
        public static KeyValuePair<string, string>[] GetLineKatakana(char consonant)
        {
            int index = Array.IndexOf(_kanaconsonant, consonant);
            KeyValuePair<string, string>[] res = new KeyValuePair<string, string>[5];
            for (int i = 0; i < 5; i++)
            {
                res[i] = new KeyValuePair<string, string>(_katakana[index, i], _romaji[index, i]);
            }
            return res;
        }
        static List<int> pickedhira = new List<int>(51);
        public static KeyValuePair<string, string> GetRandomHiragana()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            if (pickedhira.Count < 51)
            {
                int r = rand.Next(0, 50);
                while (pickedhira.Contains(r))
                {
                    r = rand.Next(0, 50);
                }
                pickedhira.Add(r);

                var res = _hiraganaPlain[r];
                if (res != "")
                {
                    return new KeyValuePair<string, string>(res, _romajiPlain[r]);
                }
                else
                {
                    return GetRandomHiragana();
                }

            }
            else
            {
                pickedhira.Clear();
                return GetRandomHiragana();
            }
        }
        static List<int> pickedkata = new List<int>(51);
        public static KeyValuePair<string, string> GetRandomKatakana()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            if (pickedkata.Count < 51)
            {
                int r = rand.Next(0, 50);
                while (pickedkata.Contains(r))
                {
                    r = rand.Next(0, 50);
                }
                pickedkata.Add(r);

                var res = _katakanaPlain[r];
                if (res != "")
                {
                    return new KeyValuePair<string, string>(res, _romajiPlain[r]);
                }
                else
                {
                    return GetRandomHiragana();
                }

            }
            else
            {
                pickedkata.Clear();
                return GetRandomKatakana();
            }
        }
    }
    public class VerbConjugationHelper
    {
        static List<string> hira = new List<string>() { "わ", "い", "う", "え", "お", "か", "き", "く", "け", "こ", "さ", "し", "す", "せ", "そ", "た", "ち", "つ", "て", "と", "な", "に", "ぬ", "ね", "の", "は", "ひ", "ふ", "へ", "ほ", "ま", "み", "む", "め", "も", "や", "い", "ゆ", "え", "よ", "ら", "り", "る", "れ", "ろ", "わ", "い", "う", "え", "を", "ん" };
        #region check
        public static bool IsKuruVerb(string word)
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
        public static bool IsSuruVerb(string word)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (word == "する" || word.Substring(word.Length - 2) == "する")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsIchidan(string word)
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
        public static string PrepGodanVerbPhoneticChange(string word)
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
                newWord = newWord + "い";
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
        public static string PrepNegative(string word)
        {
            word = word.Replace(" ", "").Replace("　", "");
            if (IsKuruVerb(word))
            {
                return "こな";
            }
            else if (IsSuruVerb(word))
            {
                return word.Substring(0, word.Length - 2) + "しな";
            }
            else if (IsIchidan(word))
            {
                return word.Substring(0, word.Length - 1) + "な";
            }
            else
            {
                string okuri = word.Substring(word.Length - 1);
                okuri = hira[hira.IndexOf(okuri) - 2];
                return word.Substring(0, word.Length - 1) + okuri;
            }
        }
        public string PrepTeTa(string word)
        {
            return PrepGodanVerbPhoneticChange(word);
        }
        #endregion
    }
}
