using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace JapaneseDict.Models
{

    public class EverydaySentence
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string JpText { get; set; }
        public string CnText { get; set; }
        public string Author { get; set; }
        public string NotesOnText { get; set; }
        public Uri AudioUri { get; set; }
        public string ContentToShare { get { return $"原文：{this.JpText}\n\n翻译：{this.CnText}"; } }
        public ImageSource BackgroundImage { get; set; }
    }
}
