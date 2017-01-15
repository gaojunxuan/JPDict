using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace JapaneseDict.Util
{
    public class KanaFlashcardHelper
    {
        public class Kana
        {
            public string Content { get; set; }
            public string Romaji { get; set; }
            public bool IsHistory { get; set; }
            public Visibility ShowRomaji { get; set; } = Visibility.Visible;
        }
        public static List<Kana> hiragana = new List<Kana>()
        {
            new Kana() {Content="あ",Romaji="a" },
            new Kana() {Content="い",Romaji="i" },
            new Kana() {Content="う",Romaji="u" },
            new Kana() {Content="え",Romaji="e" },
            new Kana() {Content="お",Romaji="o" },
            new Kana() {Content="か",Romaji="ka" },
            new Kana() {Content="き",Romaji="ki" },
            new Kana() {Content="く",Romaji="ku" },
            new Kana() {Content="け",Romaji="ke" },
            new Kana() {Content="こ",Romaji="ko" },
            new Kana() {Content="さ",Romaji="sa" },
            new Kana() {Content="し",Romaji="shi" },
            new Kana() {Content="す",Romaji="su" },
            new Kana() {Content="せ",Romaji="se" },
            new Kana() {Content="そ",Romaji="so" },
            new Kana() {Content="た",Romaji="ta" },
            new Kana() {Content="ち",Romaji="chi" },
            new Kana() {Content="つ",Romaji="tsu" },
            new Kana() {Content="て",Romaji="te" },
            new Kana() {Content="と",Romaji="to" },
            new Kana() {Content="な",Romaji="na" },
            new Kana() {Content="に",Romaji="ni" },
            new Kana() {Content="ぬ",Romaji="nu" },
            new Kana() {Content="ね",Romaji="ne" },
            new Kana() {Content="の",Romaji="no" },
            new Kana() {Content="は",Romaji="ha" },
            new Kana() {Content="ひ",Romaji="hi" },
            new Kana() {Content="ふ",Romaji="fu" },
            new Kana() {Content="へ",Romaji="he" },
            new Kana() {Content="ほ",Romaji="ho" },
            new Kana() {Content="ま",Romaji="ma" },
            new Kana() {Content="み",Romaji="mi" },
            new Kana() {Content="む",Romaji="mu" },
            new Kana() {Content="め",Romaji="me" },
            new Kana() {Content="も",Romaji="mo" },
            new Kana() {Content="や",Romaji="ya" },
            new Kana() {Content="ゆ",Romaji="yu" },
            new Kana() {Content="よ",Romaji="yo" },
            new Kana() {Content="ら",Romaji="ra" },
            new Kana() {Content="り",Romaji="ri" },
            new Kana() {Content="る",Romaji="ru" },
            new Kana() {Content="れ",Romaji="re" },
            new Kana() {Content="ろ",Romaji="ro" },
            new Kana() {Content="わ",Romaji="wa" },
            new Kana() {Content="ゐ",Romaji="wyi",IsHistory=true },
            new Kana() {Content="ゑ",Romaji="wye",IsHistory=true },
            new Kana() {Content="を",Romaji="o" },
            new Kana() {Content="ん",Romaji="n" }
        };
        public static List<Kana> katakana = new List<Kana>()
        {
            new Kana() {Content="ア",Romaji="a" },
            new Kana() {Content="イ",Romaji="i" },
            new Kana() {Content="ウ",Romaji="u" },
            new Kana() {Content="エ",Romaji="e" },
            new Kana() {Content="オ",Romaji="o" },
            new Kana() {Content="カ",Romaji="ka" },
            new Kana() {Content="キ",Romaji="ki" },
            new Kana() {Content="ク",Romaji="ku" },
            new Kana() {Content="ケ",Romaji="ke" },
            new Kana() {Content="コ",Romaji="ko" },
            new Kana() {Content="サ",Romaji="sa" },
            new Kana() {Content="シ",Romaji="shi" },
            new Kana() {Content="ス",Romaji="su" },
            new Kana() {Content="セ",Romaji="se" },
            new Kana() {Content="ソ",Romaji="so" },
            new Kana() {Content="タ",Romaji="ta" },
            new Kana() {Content="チ",Romaji="chi" },
            new Kana() {Content="ツ",Romaji="tsu" },
            new Kana() {Content="テ",Romaji="te" },
            new Kana() {Content="ト",Romaji="to" },
            new Kana() {Content="ナ",Romaji="na" },
            new Kana() {Content="ニ",Romaji="ni" },
            new Kana() {Content="ヌ",Romaji="nu" },
            new Kana() {Content="ネ",Romaji="ne" },
            new Kana() {Content="ノ",Romaji="no" },
            new Kana() {Content="ハ",Romaji="ha" },
            new Kana() {Content="ヒ",Romaji="hi" },
            new Kana() {Content="フ",Romaji="fu" },
            new Kana() {Content="ヘ",Romaji="he" },
            new Kana() {Content="ホ",Romaji="ho" },
            new Kana() {Content="マ",Romaji="ma" },
            new Kana() {Content="ミ",Romaji="mi" },
            new Kana() {Content="ム",Romaji="mu" },
            new Kana() {Content="メ",Romaji="me" },
            new Kana() {Content="モ",Romaji="mo" },
            new Kana() {Content="ヤ",Romaji="ya" },
            new Kana() {Content="ユ",Romaji="yu" },
            new Kana() {Content="ヨ",Romaji="yo" },
            new Kana() {Content="ラ",Romaji="ra" },
            new Kana() {Content="リ",Romaji="ri" },
            new Kana() {Content="ル",Romaji="ru" },
            new Kana() {Content="レ",Romaji="re" },
            new Kana() {Content="ロ",Romaji="ro" },
            new Kana() {Content="ワ",Romaji="wa" },
            new Kana() {Content="ヰ",Romaji="wyi",IsHistory=true },
            new Kana() {Content="ヱ",Romaji="wye",IsHistory=true },
            new Kana() {Content="ヲ",Romaji="o" },
            new Kana() {Content="ン",Romaji="n" }
        };
        public static List<Kana> hiraganaVoiced = new List<Kana>()
        {
            new Kana() {Content="が",Romaji="ga" },
            new Kana() {Content="ぎ",Romaji="gi" },
            new Kana() {Content="ぐ",Romaji="gu" },
            new Kana() {Content="げ",Romaji="ge" },
            new Kana() {Content="ご",Romaji="go" },
            new Kana() {Content="ざ",Romaji="za" },
            new Kana() {Content="じ",Romaji="ji" },
            new Kana() {Content="ず",Romaji="zu" },
            new Kana() {Content="ぜ",Romaji="ze" },
            new Kana() {Content="ぞ",Romaji="zo" },
            new Kana() {Content="だ",Romaji="da" },
            new Kana() {Content="ぢ",Romaji="ji" },
            new Kana() {Content="づ",Romaji="zu" },
            new Kana() {Content="で",Romaji="de" },
            new Kana() {Content="ど",Romaji="do" },
            new Kana() {Content="ば",Romaji="ba" },
            new Kana() {Content="び",Romaji="bi" },
            new Kana() {Content="ぶ",Romaji="bu" },
            new Kana() {Content="べ",Romaji="be" },
            new Kana() {Content="ぼ",Romaji="bo" },
            new Kana() {Content="ぱ",Romaji="pa" },
            new Kana() {Content="ぴ",Romaji="pi" },
            new Kana() {Content="ぷ",Romaji="pu" },
            new Kana() {Content="ぺ",Romaji="pe" },
            new Kana() {Content="ぽ",Romaji="po" },
        };
        public static List<Kana> katakanaVoiced = new List<Kana>()
        {
            new Kana() {Content="ガ",Romaji="ga" },
            new Kana() {Content="ギ",Romaji="gi" },
            new Kana() {Content="グ",Romaji="gu" },
            new Kana() {Content="ゲ",Romaji="ge" },
            new Kana() {Content="ゴ",Romaji="go" },
            new Kana() {Content="ザ",Romaji="za" },
            new Kana() {Content="ジ",Romaji="ji" },
            new Kana() {Content="ズ",Romaji="zu" },
            new Kana() {Content="ゼ",Romaji="ze" },
            new Kana() {Content="ゾ",Romaji="zo" },
            new Kana() {Content="ダ",Romaji="da" },
            new Kana() {Content="ヂ",Romaji="ji" },
            new Kana() {Content="ヅ",Romaji="zu" },
            new Kana() {Content="デ",Romaji="de" },
            new Kana() {Content="ド",Romaji="do" },
            new Kana() {Content="バ",Romaji="ba" },
            new Kana() {Content="ビ",Romaji="bi" },
            new Kana() {Content="ブ",Romaji="bu" },
            new Kana() {Content="ベ",Romaji="be" },
            new Kana() {Content="ボ",Romaji="bo" },
            new Kana() {Content="パ",Romaji="pa" },
            new Kana() {Content="ピ",Romaji="pi" },
            new Kana() {Content="プ",Romaji="pu" },
            new Kana() {Content="ペ",Romaji="pe" },
            new Kana() {Content="ポ",Romaji="po" },
        };
        public static IEnumerable<Kana> GetRandomHiragana()
        {
            Random ran = new Random();
            List<Kana> res = new List<Kana>(hiragana);
            int index = 0;
            Kana temp = null;
            for (int i = 0; i < res.Count; i++)
            {

                index = ran.Next(0, res.Count - 1);
                if (index != i)
                {
                    temp = res[i];
                    res[i] = res[index];
                    res[index] = temp;
                }
            }
            return res;
        }
        public static IEnumerable<Kana> GetOrderHiragana()
        {
            return new List<Kana>(hiragana);
        }
        public static IEnumerable<Kana> GetOrderHiraganaWithVoicedConsonants()
        {
            var res = new List<Kana>(hiragana);
            res.Concat(hiraganaVoiced);
            return res;
        }
        public static IEnumerable<Kana> GetRandomHiraganaWithVoicedConsonants()
        {
            Random ran = new Random();
            List<Kana> res = new List<Kana>(hiragana.Concat(hiraganaVoiced));
            int index = 0;
            Kana temp = null;
            for (int i = 0; i < res.Count; i++)
            {

                index = ran.Next(0, res.Count - 1);
                if (index != i)
                {
                    temp = res[i];
                    res[i] = res[index];
                    res[index] = temp;
                }
            }
            return res;
        }
        public static IEnumerable<Kana> GetRandomKatakana()
        {
            Random ran = new Random();
            List<Kana> res = new List<Kana>(katakana);
            int index = 0;
            Kana temp = null;
            for (int i = 0; i < res.Count; i++)
            {

                index = ran.Next(0, res.Count - 1);
                if (index != i)
                {
                    temp = res[i];
                    res[i] = res[index];
                    res[index] = temp;
                }
            }
            return res;
        }
        public static IEnumerable<Kana> GetRandomKatakanaWithVoicedConsonants()
        {
            Random ran = new Random();
            List<Kana> res = new List<Kana>(katakana.Concat(hiraganaVoiced));
            int index = 0;
            Kana temp = null;
            for (int i = 0; i < res.Count; i++)
            {

                index = ran.Next(0, res.Count - 1);
                if (index != i)
                {
                    temp = res[i];
                    res[i] = res[index];
                    res[index] = temp;
                }
            }
            return res;
        }
        public static IEnumerable<Kana> GetOrderKatakana()
        {
            return new List<Kana>(katakana);
        }
        public static IEnumerable<Kana> GetOrderKatakanaWithVoicedConsonants()
        {
            var res = new List<Kana>(katakana);
            res.Concat(katakanaVoiced);
            return res;
        }
    }
}
