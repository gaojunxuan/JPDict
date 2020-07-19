using JapaneseDict.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Models
{
    public class Dict : IDictItem
    {
        public Dict()
        {

        }
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
                return string.IsNullOrEmpty(Definition) ? "..." :  Pos + " " + Definition.Replace("\n", " ").Substring(0, ((Definition.Length >= 31) ? (30) : (Definition.Length))).Trim() + " ...";
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
