using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Models
{
    interface IKanji
    {
        string Kanji { get; set; }
        string Strokes { get; set; }
        string Grade { get; set; }
        string Jlpt { get; set; }
        string OnReading { get; set; }
        string KunReading { get; set; }
    }
}
