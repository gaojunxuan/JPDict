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
        [PrimaryKey, AutoIncrement]
        int ID { get; set; }
        string Kanji { get; set; }
        /// <summary>
        /// Hantai kanji (繁体字)
        /// </summary>
        string Hantaiji { get; set; }
        int Strokes { get; set; }
        string Grade { get; set; }
        /// <summary>
        /// Readings which contain On-reading(音読み) and Kun-reading(訓読み)
        /// </summary>
        string Reading { get; set; }
    }
}
