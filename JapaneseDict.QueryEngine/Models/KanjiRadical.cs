using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Models
{
    public class KanjiRadical
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Unicode { get; set; }
        public string Kanji { get; set; }
        public string SVGPath { get; set; }
        public int Order { get; set; }
    }
}
