using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Util
{
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
        public static void ResetHiragana()
        {
            pickedhira.Clear();
        }
    }
}
