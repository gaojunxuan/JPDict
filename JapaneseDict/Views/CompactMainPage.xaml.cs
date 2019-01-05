using JapaneseDict.GUI.Helpers;
using JapaneseDict.GUI.Models;
using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CompactMainPage : Page
    {
        public CompactMainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ExtendAcrylicIntoTitleBar();
            base.OnNavigatedTo(e);
        }

        private void ExtendAcrylicIntoTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

        private async void Grid_Drop(object sender, DragEventArgs e)
        {
            try
            {
                var content = e.DataView;
                if (content != null)
                {
                    Regex regex = new Regex(@"[\u3040-\u30ff\u3400-\u4dbf\u4e00-\u9fff\uf900-\ufaff\uff66-\uff9f]");
                    if (content.Contains(StandardDataFormats.Text))
                    {
                        string keyword = await content.GetTextAsync();
                        if (regex.IsMatch(keyword))
                        {
                            await PushToast(keyword);
                        }
                    }
                }
            }
            catch
            {
                await Task.Delay(500);
            }
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
        }

        private async void exitCompactBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
            await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay);
        }

        private async void QueryTbx_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                string keyword = (sender as TextBox).Text;
                await PushToast(keyword);
                
            }
        }
        async Task PushToast(string keyword)
        {
            var result = (await QueryEngine.QueryEngine.MainDictQueryEngine.QueryForUIAsync(StringHelper.ResolveReplicator(keyword))).GroupBy(x => x.ItemId).Select(g => new GroupedDictItem(g)).First();
            string parameters = new QueryString()
                {
                    {"action","detailResult" },
                    {"keyword",keyword }
                }.ToString();
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                    }
                },
                Actions = new ToastActionsCustom()
                {
                    Buttons =
                        {
                            new ToastButton("详细", parameters)
                            {
                                ActivationType = ToastActivationType.Foreground
                            }
                        }
                },
                Launch = parameters
            };
            string header = "";
            if (!string.IsNullOrEmpty(result.Kanji))
            {
                header = $"{result.Kanji}（{result.Reading}）";
            }
            else
            {
                header = result.Keyword;

            }
            toastContent.Visual.BindingGeneric.Children.Add(new AdaptiveText() { Text = header });
            foreach (var r in result)
            {
                toastContent.Visual.BindingGeneric.Children.Add(new AdaptiveText() { Text = r.Pos + " " + r.Definition });
            }
            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }
    }
}
