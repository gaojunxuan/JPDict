using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace JapaneseDict.Models
{
    public class Kana:ViewModelBase
    {
        public string Content { get; set; }
        public string Romaji { get; set; }
        public bool IsHistory { get; set; }
        Visibility _showRomaji = Visibility.Visible;
        public Visibility ShowRomaji
        {
            get
            {
                return _showRomaji;
            }
            set
            {
                _showRomaji = value;
                RaisePropertyChanged();
            }
        } 
    }
}
