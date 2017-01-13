using JapaneseDict.Util;
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

        static string[] _allhira = new string[] { "あ", "い", "う", "え", "お", "か", "き", "く", "け", "こ", "さ", "し", "す", "せ", "そ", "た", "ち", "つ", "て", "と", "な", "に", "ぬ", "ね", "の", "は", "ひ", "ふ", "へ", "ほ", "ま", "み", "む", "め", "も", "や", "", "ゆ", "", "よ", "ら", "り", "る", "れ", "ろ", "わ", "", "", "", "を", "ん", "", "", "", "", "が", "ぎ", "ぐ", "げ", "ご", "ざ", "じ", "ず", "ぜ", "ぞ", "だ", "ぢ", "づ", "で", "ど", "ば", "び", "ぶ", "べ", "ぼ", "ぱ", "ぴ", "ぷ", "ぺ", "ぽ" };
        static string[] _allkata = new string[] { "ア", "イ", "ウ", "エ", "オ", "カ", "キ", "ク", "ケ", "コ", "サ", "シ", "ス", "セ", "ソ", "タ", "チ", "ツ", "テ", "ト", "ナ", "ニ", "ヌ", "ネ", "ノ", "ハ", "ヒ", "フ", "ヘ", "ホ", "マ", "ミ", "ム", "メ", "モ", "ヤ", "", "ユ", "", "ヨ", "ラ", "リ", "ル", "レ", "ロ", "ワ", "", "", "", "ヲ", "ン", "", "", "", "", "ガ", "ギ", "グ", "ゲ", "ゴ", "ザ", "ジ", "ズ", "ゼ", "ゾ", "ダ", "ヂ", "ヅ", "デ", "ド", "バ", "ビ", "ブ", "ベ", "ボ", "パ", "ピ", "プ", "ペ", "ポ" };

        //static string[,] _hiraganaHistory = new string[11, 5] { { "あ", "い", "う", "え", "お" }, { "か", "き", "く", "け", "こ" }, { "さ", "し", "す", "せ", "そ" }, { "た", "ち", "つ", "て", "と" }, { "な", "に", "ぬ", "ね", "の" }, { "は", "ひ", "ふ", "へ", "ほ" }, { "ま", "み", "む", "め", "も" }, { "や", "", "ゆ", "", "よ" }, { "ら", "り", "る", "れ", "ろ" }, { "わ", "ゐ", "", "ゑ", "を" }, { "ん", "", "", "", "" } };
        //static string[] _hiraganaHistoryPlain = new string[] { "あ", "い", "う", "え", "お", "か", "き", "く", "け", "こ", "さ", "し", "す", "せ", "そ", "た", "ち", "つ", "て", "と", "な", "に", "ぬ", "ね", "の", "は", "ひ", "ふ", "へ", "ほ", "ま", "み", "む", "め", "も", "や", "", "ゆ", "", "よ", "ら", "り", "る", "れ", "ろ", "わ", "ゐ", "", "ゑ", "を", "ん", "", "", "", "" };

        static string[,] _hiraganaSonant = new string[4, 5] {  { "が", "ぎ", "ぐ", "げ", "ご" }, { "ざ", "じ", "ず", "ぜ", "ぞ" }, { "だ", "ぢ", "づ", "で", "ど" },  { "ば", "び", "ぶ", "べ", "ぼ" } };
        static string[] _hiraganaSonantPlain = new string[] {  "が", "ぎ", "ぐ", "げ", "ご" ,  "ざ", "じ", "ず", "ぜ", "ぞ" ,  "だ", "ぢ", "づ", "で", "ど" ,  "ば", "び", "ぶ", "べ", "ぼ"  };

        static string[,] _hiraganaSemiSonant = new string[1, 5] { { "ぱ", "ぴ", "ぷ", "ぺ", "ぽ" }};
        static string[] _hiraganaSemiSonantPlain = new string[] { "ぱ", "ぴ", "ぷ", "ぺ", "ぽ" };

        static string[,] _katakana = new string[11, 5] { { "ア", "イ", "ウ", "エ", "オ" }, { "カ", "キ", "ク", "ケ", "コ" }, { "サ", "シ", "ス", "セ", "ソ" }, { "タ", "チ", "ツ", "テ", "ト" }, { "ナ", "ニ", "ヌ", "ネ", "ノ" }, { "ハ", "ヒ", "フ", "ヘ", "ホ" }, { "マ", "ミ", "ム", "メ", "モ" }, { "ヤ", "", "ユ", "", "ヨ" }, { "ラ", "リ", "ル", "レ", "ロ" }, { "ワ", "", "", "", "ヲ" }, { "ン", "", "", "", "" } };
        static string[] _katakanaPlain = new string[] { "ア", "イ", "ウ", "エ", "オ", "カ", "キ", "ク", "ケ", "コ", "サ", "シ", "ス", "セ", "ソ", "タ", "チ", "ツ", "テ", "ト", "ナ", "ニ", "ヌ", "ネ", "ノ", "ハ", "ヒ", "フ", "ヘ", "ホ", "マ", "ミ", "ム", "メ", "モ", "ヤ", "", "ユ", "", "ヨ", "ラ", "リ", "ル", "レ", "ロ", "ワ", "", "", "", "ヲ", "ン", "", "", "", "" };

        static string[,] _katakanaHistory = new string[11, 5] { { "ア", "イ", "ウ", "エ", "オ" }, { "カ", "キ", "ク", "ケ", "コ" }, { "サ", "シ", "ス", "セ", "ソ" }, { "タ", "チ", "ツ", "テ", "ト" }, { "ナ", "ニ", "ヌ", "ネ", "ノ" }, { "ハ", "ヒ", "フ", "ヘ", "ホ" }, { "マ", "ミ", "ム", "メ", "モ" }, { "ヤ", "", "ユ", "", "ヨ" }, { "ラ", "リ", "ル", "レ", "ロ" }, { "ワ", "ヰ", "", "ヱ", "ヲ" }, { "ン", "", "", "", "" } };
        static string[] _katakanaHistoryPlain = new string[] { "ア", "イ", "ウ", "エ", "オ", "カ", "キ", "ク", "ケ", "コ", "サ", "シ", "ス", "セ", "ソ", "タ", "チ", "ツ", "テ", "ト", "ナ", "ニ", "ヌ", "ネ", "ノ", "ハ", "ヒ", "フ", "ヘ", "ホ", "マ", "ミ", "ム", "メ", "モ", "ヤ", "", "ユ", "", "ヨ", "ラ", "リ", "ル", "レ", "ロ", "ワ", "ヰ", "", "ヱ", "ヲ", "ン", "", "", "", "" };

        static string[,] _katakanaSonant = new string[4, 5] { { "ガ", "ギ", "グ", "ゲ", "ゴ" }, { "ザ", "ジ", "ズ", "ゼ", "ゾ" }, { "ダ", "ヂ", "ヅ", "デ", "ド" }, { "バ", "ビ", "ブ", "ベ", "ボ" } };
        static string[] _katakanaSonantPlain = new string[] {  "ガ", "ギ", "グ", "ゲ", "ゴ" , "ザ", "ジ", "ズ", "ゼ", "ゾ" , "ダ", "ヂ", "ヅ", "デ", "ド" ,  "バ", "ビ", "ブ", "ベ", "ボ" };

        static string[,] _katakanaSemiSonant = new string[1, 5] { { "パ", "ピ", "プ", "ペ", "ポ" } };
        static string[] _katakanaSemiSonantPlain = new string[] { "パ", "ピ", "プ", "ペ", "ポ" };

        static string[,] _romaji = new string[11, 5] { { "a", "i", "u", "e", "o" }, { "ka", "ki", "ku", "ke", "ko" }, { "sa", "shi", "su", "se", "so" }, { "ta", "chi", "tsu", "te", "to" }, { "na", "ni", "nu", "ne", "no" }, { "ha", "hi", "fu", "he", "ho" }, { "ma", "mi", "mu", "me", "mo" }, { "ya", "", "yu", "", "yo" }, { "ra", "ri", "ru", "re", "ro" }, { "wa", "", "", "", "o" }, { "n", "", "", "", "" } };
        static string[] _romajiPlain = new string[] { "a", "i", "u", "e", "o", "ka", "ki", "ku", "ke", "ko", "sa", "shi", "su", "se", "so", "ta", "chi", "tsu", "te", "to", "na", "ni", "nu", "ne", "no", "ha", "hi", "fu", "he", "ho", "ma", "mi", "mu", "me", "mo", "ya", "", "yu", "", "yo", "ra", "ri", "ru", "re", "ro", "wa", "", "", "", "o", "n", "", "", "", "" };

        static string[,] _sonantRomaji = new string[4, 5] { { "ga", "gi", "gu", "ge", "go" }, { "za", "ji", "zu", "ze", "zo" }, { "da", "ji", "zu", "de", "do" }, { "ba", "bi", "bu", "be", "bo" } };
        static string[] _sonantRomajiPlain = new string[] { "ga", "gi", "gu", "ge", "go" , "za", "ji", "zu", "ze", "zo" , "da", "ji", "zu", "de", "do" , "ba", "bi", "bu", "be", "bo"  };

        static string[,] _semiSonantRomaji = new string[1, 5] { { "pa", "pi", "pu", "pe", "po" } };
        static string[] _semiSonantRomajiPlain = new string[] { "pa", "pi", "pu", "pe", "po" };

        static char[] _kanaconsonant = new char[] { 'a', 'k', 's', 't', 'n', 'h', 'm', 'y', 'r', 'w' };
        static char[] _kanasonantConsonant = new char[] { 'g', 'z', 'd', 'b' };
        public class Kana
        {
            public string Content { get; set; }
            public string Romaji { get; set; }
            public bool IsHistory { get; set; }
        }
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
        public static KeyValuePair<string, string> GetRandomHiraganaWithNoSonant()
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
                    return GetRandomHiraganaWithNoSonant();
                }

            }
            else
            {
                pickedhira.Clear();
                return GetRandomHiraganaWithNoSonant();
            }
        }
        static List<int> pickedkata = new List<int>(51);
        public static KeyValuePair<string, string> GetRandomKatakanaWithNoSonant()
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
                    return GetRandomKatakanaWithNoSonant();
                }

            }
            else
            {
                pickedkata.Clear();
                return GetRandomKatakanaWithNoSonant();
            }
        }
        static List<int> pickedallhira = new List<int>(101);
        public static KeyValuePair<string, string> GetRandomHiragana()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            if (pickedallhira.Count < 101)
            {
                int r = rand.Next(0, 100);
                while (pickedallhira.Contains(r))
                {
                    r = rand.Next(0, 100);
                }
                pickedallhira.Add(r);
                var res = _allhira[r];
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
                return GetRandomHiraganaWithNoSonant();
            }
        }
        static List<int> pickedallkata = new List<int>(101);
        public static KeyValuePair<string, string> GetRandomKatakana()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            if (pickedallkata.Count < 101)
            {
                int r = rand.Next(0, 100);
                while (pickedallkata.Contains(r))
                {
                    r = rand.Next(0, 100);
                }
                pickedallkata.Add(r);

                var res = _allkata[r];
                if (res != "")
                {
                    return new KeyValuePair<string, string>(res, _romajiPlain[r]);
                }
                else
                {
                    return GetRandomKatakana();
                }

            }
            else
            {
                pickedallkata.Clear();
                return GetRandomKatakana();
            }
        }
        public static void ResetHiragana()
        {
            pickedhira.Clear();
        }
        public static void ResetKatakana()
        {
            pickedkata.Clear();
        }
        public static void ResetAllHira()
        {
            pickedallhira.Clear();
        }
        public static void ResetAllKata()
        {
            pickedallkata.Clear();
        }
        
    }
}
