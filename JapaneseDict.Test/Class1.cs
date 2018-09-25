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
        //public static void Do()
        //{
        //    var connection = new SQLiteConnection(Console.ReadLine());
        //    var result = connection.Query<MainDict>("select * from MainDict where JpChar like '%·%'");
        //    foreach(var i in result)
        //    {
        //        i.Kana = i.Kana.Replace("·", "");
        //        connection.Update(i);
        //        connection.Insert(new MainDict() {JpChar=i.JpChar.Replace("·",""),Kana= i.Kana.Replace("·", ""),Explanation=i.Explanation });
        //        Console.WriteLine(i.JpChar);
        //    }
        //}
    }
    public class Dict
    {
        [PrimaryKey, AutoIncrement]
        public int DefinitionId { get; set; }
        public int ItemId { get; set; }
        public string Definition { get; set; }
        public string Pos { get; set; }
        public string Keyword { get; set; }
        public string Reading { get; set; }
        public string Kanji { get; set; }
        public string LoanWord { get; set; }
        public string SeeAlso { get; set; }
        [Ignore]
        public string PreviewExplanation
        {
            get
            {
                return Pos + " " + Definition.Replace("\n", " ").Substring(0, ((Definition.Length >= 31) ? (30) : (Definition.Length))).Trim() + " ...";
            }
        }
        [Ignore]
        public bool IsControlBtnVisible
        {
            get
            {
                return !string.IsNullOrEmpty(Reading);
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
        public string Suggestion
        {
            get;
            set;
        }
        public bool IsInNotebook
        {
            get; set;
        }

    }
}
