using JapaneseDict.GUI.ViewModels;
using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
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
using JapaneseDict.Util;
using Windows.System;
using Windows.System.Profile;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.ApplicationModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : MVVMPage
    {



        public SettingsPage()
        {

            this.InitializeComponent();
            this.RegisterPropertyChangedCallback(ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = this.ViewModel as SettingsPage_Model;
            });
            StrongTypeViewModel = this.ViewModel as SettingsPage_Model;
        }


        public SettingsPage_Model StrongTypeViewModel
        {
            get { return (SettingsPage_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register("StrongTypeViewModel", typeof(SettingsPage_Model), typeof(SettingsPage), new PropertyMetadata(null));




        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
        PackageVersion pv = Package.Current.Id.Version;
        private async void feedback_Btn_Click(object sender, RoutedEventArgs e)
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

        private async void oss_Btn_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri($"https://skylark-workshop.xyz/oss-license/");
            await Launcher.LaunchUriAsync(uri);
        }

        private async void privacypolicy_Btn_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri($"https://skylark-workshop.xyz/privacy-jpdict/");
            await Launcher.LaunchUriAsync(uri);
        }
    }
}
