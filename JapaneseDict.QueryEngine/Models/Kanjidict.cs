using JapaneseDict.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Models
{
    public class Kanjidict : IKanji
    {
        public string Grade
        {
            get;set;
        }

        public string Jlpt
        {
            get; set;
        }

        public string Kanji
        {
            get; set;
        }

        public string KunReading
        {
            get; set;
        }

        public string OnReading
        {
            get; set;
        }

        public string Strokes
        {
            get; set;
        }
    }
}
