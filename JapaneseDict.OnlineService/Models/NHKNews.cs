using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Models
{
    public class NHKNews
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Uri Link { get; set; }
        public Uri ImgPath { get; set; }
        public Uri IconPath { get; set; }
        public Uri VideoPath { get; set; }
        public DateTime PubDate { get; set; }
    }
}
