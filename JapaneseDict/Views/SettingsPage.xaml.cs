using JapaneseDict.GUI.ViewModels;
using System.Reactive;
using System.Reactive.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.System;
using Windows.System.Profile;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.ApplicationModel;
using JapaneseDict.OnlineService;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using JapaneseDict.GUI.Helpers;
using Windows.ApplicationModel.Core;
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.NavigationCacheMode = NavigationCacheMode.Disabled;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
        PackageVersion pv = Package.Current.Id.Version;
        private async void feedback_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported())
            {
                var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
                await launcher.LaunchAsync();
            }
            else
            {
                string sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
                ulong v = ulong.Parse(sv);
                ulong v1 = (v & 0xFFFF000000000000L) >> 48;
                ulong v2 = (v & 0x0000FFFF00000000L) >> 32;
                ulong v3 = (v & 0x00000000FFFF0000L) >> 16;
                ulong v4 = (v & 0x000000000000FFFFL);
                string version = $"{v1}.{v2}.{v3}.{v4}";
                EasClientDeviceInformation eas = new EasClientDeviceInformation();
                var mailto = new Uri($"mailto:{"skylarkworkshop@gmail.com"}?subject={"Skylark JPDict 反馈"}&body={$"设备信息：%0ADeviceManufacturer: {eas.SystemManufacturer}%0ADeviceModel: {eas.SystemProductName}%0ADevice family: {AnalyticsInfo.VersionInfo.DeviceFamily}%0ADevice family version: {v.ToString()}%0AOS version: {version}%0AOS architecture: {Package.Current.Id.Architecture.ToString()}%0AApp version:{$"v{pv.Major}.{pv.Minor}.{pv.Build}.{pv.Revision}"}%0A%0A反馈:"}");
                await Launcher.LaunchUriAsync(mailto);
            }
        }

        private async void oss_Btn_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri($"https://skylark-workshop.xyz/oss-license.htm");
            await Launcher.LaunchUriAsync(uri);
        }

        private async void privacypolicy_Btn_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri($"https://skylark-workshop.xyz/privacy-jpdict");
            await Launcher.LaunchUriAsync(uri);
        }

        private void Upload_Btn_Click(object sender, RoutedEventArgs e)
        {
            pageRoot.Focus(FocusState.Pointer);
        }

        private void Download_Btn_Click(object sender, RoutedEventArgs e)
        {
            pageRoot.Focus(FocusState.Pointer);
        }

        private void update_Btn_Click(object sender, RoutedEventArgs e)
        {
            pageRoot.Focus(FocusState.Pointer);
        }

        private void update_Btn_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            pageRoot.Focus(FocusState.Pointer);
        }

        private void Upload_Btn_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            pageRoot.Focus(FocusState.Pointer);
        }

        private void Download_Btn_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            pageRoot.Focus(FocusState.Pointer);
        }

        private async void nhkEasy_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (!ApplicationViewHelper.Contains("newsViewerWithRuby"))
            {
                int newViewId = await ApplicationViewHelper.CreateNewViewAsync(typeof(NewsReaderWithRubyPage));
                CoreApplicationView newView = ApplicationViewHelper.GetViewFromId(newViewId);
                bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId, ViewSizePreference.UseMore);
            }
            else
            {
                int newViewId = ApplicationViewHelper.GetId("newsViewerWithRuby");
                if (newViewId != -1)
                    await ApplicationViewSwitcher.SwitchAsync(newViewId);
            }
        }

        private async void survey_Btn_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://goo.gl/forms/8ee1bvMz83zrpMmF2"));
        }

        private void lemMode1_Checked(object sender, RoutedEventArgs e)
        {
            lemSample1.Foreground = new SolidColorBrush(Colors.Black);
            lemSample2.Foreground = new SolidColorBrush(Colors.Black);
            lemSample3.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void lemMode2_Checked(object sender, RoutedEventArgs e)
        {
            lemSample2.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 178, 148));
            lemSample1.Foreground = new SolidColorBrush(Colors.Black);
            lemSample3.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void lemMode3_Checked(object sender, RoutedEventArgs e)
        {
            lemSample1.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 178, 148));
            lemSample2.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 178, 148));
            lemSample3.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 178, 148));
        }
    }
}
