using JapaneseDict.GUI.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewsReaderWithRubyPage : Page
    {
        public NewsReaderWithRubyPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetTitleBar();
            if(e.Parameter!=null)
            {
                if (!string.IsNullOrWhiteSpace(e.Parameter.ToString()))
                {
                    ReaderWebView.Navigate(new Uri(e.Parameter.ToString()));
                }
            }
            howTo_Notification.Show();
            base.OnNavigatedTo(e);
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

        private void ReaderWebView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            ErrorGrid.Visibility = Visibility.Visible;
            ReaderWebView.Visibility = Visibility.Collapsed;
        }

        private void retryBtn_Click(object sender, RoutedEventArgs e)
        {
            ReaderWebView.Navigate(ReaderWebView.Source);
            ReaderWebView.Visibility = Visibility.Visible;
            ErrorGrid.Visibility = Visibility.Collapsed;
        }
    }
}
