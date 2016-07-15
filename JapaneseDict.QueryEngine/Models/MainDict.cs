using JapaneseDict.Models;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.Models
{
    public class MainDict:IDictItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string JpChar { get; set; }
        private string _Kana;
        public string Kana
        {
            get
            {
                
                if (string.IsNullOrEmpty(_Kana) && Explanation != "没有本地释义")
                    return JpChar.Replace("·","");
                else if (!string.IsNullOrEmpty(_Kana))
                    return _Kana.Replace("·", "");
                else
                    return null;
            }
            set
            {
                _Kana = value;
            }
        }
        public string Explanation { get; set; }
        public string Comment { get; set; }
        [Ignore]
        public string PreviewExplanation
        {
            get
            {
                return this.Explanation.Replace("\r", " ").Replace("\n"," ").Substring(0,((Explanation.Length>=31)?(30):(Explanation.Length)))+" ...";
            }
        }
        [Ignore]
        public bool IsControlBtnVisible
        {
            get
            {
                return !(string.IsNullOrEmpty(Kana));
            }
        }
       　public bool IsInUserDefDict
        {
            get; set;
        }
        
    }
}
