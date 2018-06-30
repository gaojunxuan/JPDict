using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Test
{
    public static class Class1
    {
        public static void Do()
        {
            var connection = new SQLiteConnection(Console.ReadLine());
            var result = connection.Query<MainDict>("select * from MainDict where JpChar like '%·%'");
            foreach(var i in result)
            {
                i.Kana = i.Kana.Replace("·", "");
                connection.Update(i);
                connection.Insert(new MainDict() {JpChar=i.JpChar.Replace("·",""),Kana= i.Kana.Replace("·", ""),Explanation=i.Explanation });
                Console.WriteLine(i.JpChar);
            }
        }
    }
    public class MainDict
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string JpChar { get; set; }
        private string _Kana;
        public string Kana
        {
            get
            {

                if (string.IsNullOrEmpty(_Kana) && Explanation != "没有本地释义")
                    return JpChar.Replace("·", "");
                else if (!string.IsNullOrEmpty(_Kana))
                    return _Kana.Replace("·", "");
                else
                    return null;
            }
            set
            {
                _Kana = value;
            }
        }
        public string Explanation { get; set; }
        public string Comment { get; set; }
        [Ignore]
        public string PreviewExplanation
        {
            get
            {
                return Explanation.Replace("\n", " ").Substring(0, ((Explanation.Length >= 31) ? (30) : (Explanation.Length))).Trim() + " ...";
            }
        }
        [Ignore]
        public bool IsControlBtnVisible
        {
            get
            {
                return !string.IsNullOrEmpty(Kana);
            }
        }
        [Ignore]
        public bool IsSeeAlsoBtnVisible
        {
            get
            {
                return !string.IsNullOrWhiteSpace(SeeAlso);
            }
        }
        [Ignore]
        public string SeeAlso
        {
            get;
            set;
        }
        public bool IsInUserDefDict
        {
            get; set;
        }

    }
}
