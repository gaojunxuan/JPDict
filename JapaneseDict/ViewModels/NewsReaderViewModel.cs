using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using JapaneseDict.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDict.GUI.ViewModels
{
    public class NewsReaderViewModel : ViewModelBase
    {
        public NewsReaderViewModel(FormattedNews n)
        {
            News = n;
        }
        [PreferredConstructor]
        public NewsReaderViewModel()
        {

        }
        private FormattedNews news;

        public FormattedNews News
        {
            get { return news; }
            set
            {
                news = value;
                RaisePropertyChanged();
            }
        }
        public string Title => News?.Title;
        public string Content => News?.Content;
        public Uri Image => News?.Image;
    }
}
