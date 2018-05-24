using JapaneseDict.Models;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JapaneseDict.Models
{
    public class UserDefDict : IDictItem
    {
        public UserDefDict()
        {

        }
        public UserDefDict(MainDict source)
        {
            this.Comment = source.Comment;
            this.Explanation = source.Explanation;
            this.Kana = source.Kana;
            this.JpChar = source.JpChar;
            
        }
        public UserDefDict(UserDefDict data)
        {
            this.Comment = data.Comment;
            this.Explanation = data.Explanation;
            this.JpChar = data.JpChar;
            
        }
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
        [PrimaryKey]
        [AutoIncrement]
        public int ID
        {
            get; set;
        }
        public int OriginID { get; set; }
        public string JpChar
        {
            get; set;
        }
        [Ignore]
        public string GroupingKey
        {
            get { return Kana[0].ToString(); }
        }
        [Ignore]
        public string PreviewExplanation
        {
            get
            {
                return this.Explanation.Replace("\r", " ").Substring(0, ((Explanation.Length >= 31) ? (30) : (Explanation.Length))) + " ...";
            }
        }
    }
}
