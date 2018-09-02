using JapaneseDict.GUI.Helpers;
using JapaneseDict.GUI.ViewModels;
using JapaneseDict.Models;
using JapaneseDict.OnlineService.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewsReaderPage : Page
    {
        public NewsReaderPage()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            SetTitleBar();
            if(string.IsNullOrEmpty(e.Parameter.ToString()))
            {
                this.DataContext = new NewsReaderViewModel(new FormattedNews() { Title = "エラーが発生しました" });
            }
            else
            {
                await GetNews(e.Parameter.ToString());
            }
            base.OnNavigatedTo(e);
        }
        FormattedNews item = new FormattedNews();
        async Task GetNews(string url)
        {
            if(!string.IsNullOrWhiteSpace(url))
            {
                try
                {
                    item = await JsonHelper.GetFormattedNews(url);
                    this.DataContext = new NewsReaderViewModel(item);
                }
                catch
                {
                    this.DataContext = new NewsReaderViewModel(new FormattedNews() { Title = "エラーが発生しました" });
                }
            }
        }
        void SetTitleBar()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = Colors.Black;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
        }
    }
}
