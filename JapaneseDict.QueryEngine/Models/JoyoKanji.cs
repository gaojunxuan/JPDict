using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Models
{
    public class JoyoKanji : IKanji
    {
        public string Grade
        {
            get;set;
        }

        public string Hantaiji
        {
            get; set;
        }

        public int ID
        {
            get; set;
        }

        public string Kanji
        {
            get; set;
        }

        public string Reading
        {
            get; set;
        }

        public int Strokes
        {
            get; set;
        }
    }
}
