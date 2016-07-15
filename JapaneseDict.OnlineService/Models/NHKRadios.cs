using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Models
{
    public class NHKRadios
    {
        public string Title { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Uri SoundUrl { get; set; }
    }
}
