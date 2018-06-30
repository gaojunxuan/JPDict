using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Models
{
    interface IDictItem
    {
        int ItemId { get; set; }
        string Definition { get; set; }
        string Pos { get; set; }
        string Keyword { get; set; }
        string Reading { get; set; }
        string Kanji { get; set; }
        string LoanWord { get; set; }
        string SeeAlso { get; set; }
    }
}
