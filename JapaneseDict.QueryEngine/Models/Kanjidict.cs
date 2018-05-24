using JapaneseDict.Models;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

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
        [Ignore]
        public Visibility ShowReading { get; set; } = Visibility.Visible;
    }
}
