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
        [PrimaryKey, AutoIncrement]
        int ID { get; set; }
        string JpChar { get; set; }
        string Kana { get; set; }
        string Explanation { get; set; }
        string Comment { get; set; }
    }
}
