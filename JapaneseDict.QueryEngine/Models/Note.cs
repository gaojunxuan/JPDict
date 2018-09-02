using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Models
{
    public class Note : IDictItem
    {
        public Note()
        {

        }
        public Note(Dict source)
        {
            ItemId = source.ItemId;
            Pos = source.Pos;
            Definition = source.Definition;
            Keyword = source.Keyword;
            Kanji = source.Kanji;
            Reading = source.Reading;
            LoanWord = source.LoanWord;
            SeeAlso = source.SeeAlso;
        }
        public Note(Note data)
        {
            ItemId = data.ItemId;
            Pos = data.Pos;
            Definition = data.Definition;
            Keyword = data.Keyword;
            Kanji = data.Kanji;
            Reading = data.Reading;
            LoanWord = data.LoanWord;
            SeeAlso = data.SeeAlso;
            Comment = data.Comment;
        }
        [PrimaryKey]
        [AutoIncrement]
        public int Id
        {
            get; set;
        }
        public string Comment
        {
            get; set;
        }
        public int ItemId { get; set; }
        public string Definition { get; set; }
        public string Pos { get; set; }
        public string Keyword { get; set; }
        public string Reading { get; set; }
        public string Kanji { get; set; }
        public string LoanWord { get; set; }
        public string SeeAlso { get; set; }

        [Ignore]
        public string GroupingKey
        {
            get { return Reading[0].ToString(); }
        }
        [Ignore]
        public string PreviewExplanation
        {
            get
            {
                return Pos + (string.IsNullOrEmpty(Pos)?"":" ") + Definition.Replace("\n", " ").Replace("\r", " ").Substring(0, ((Definition.Length >= 31) ? (30) : (Definition.Length))).Trim() + " ...";
            }
        }
    }
}
