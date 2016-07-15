using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Models
{
    public class OnlineDict : IDictItem
    {
        public string Comment
        {
            get; set;
        }

        public string Explanation
        {
            get; set;
        }

        public string Kana
        {
            get; set;
        }

        public int ID
        {
            get; set;
        }

        public string JpChar
        {
            get; set;
        }
    }
}
