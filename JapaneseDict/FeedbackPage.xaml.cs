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
using Windows.UI.Popups;
using Windows.UI.Core;
using Windows.Phone.UI.Input;
using JapaneseDict.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FeedbackPage : MVVMPage
    {



        public FeedbackPage()
        {

            this.InitializeComponent();
            this.RegisterPropertyChangedCallback(ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = this.ViewModel as FeedbackPage_Model;
            });
            StrongTypeViewModel = this.ViewModel as FeedbackPage_Model;
        }


        public FeedbackPage_Model StrongTypeViewModel
        {
            get { return (FeedbackPage_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register("StrongTypeViewModel", typeof(FeedbackPage_Model), typeof(FeedbackPage), new PropertyMetadata(null));




        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            EnableBackButtonOnTitleBar((sender, args) =>
            {
                Frame rootFrame = Window.Current.Content as Frame;
                if (rootFrame.CanGoBack)
                {
                    rootFrame.GoBack();

                }
                DisableBackButtonOnTitleBar();
            });
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
            if (e.Parameter != null)
            {
                this.jpchar_Tbx.Text = e.Parameter.ToString();
            }
        }
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }
        private void EnableBackButtonOnTitleBar(EventHandler<BackRequestedEventArgs> onBackRequested)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            currentView.BackRequested += onBackRequested;
        }
        private void DisableBackButtonOnTitleBar()
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

            }
        }

        private async void sendFeedback_Btn_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(jpchar_Tbx.Text)&& !string.IsNullOrWhiteSpace(kana_Tbx.Text)&& !string.IsNullOrWhiteSpace(email_Tbx.Text))
            {
                JPDictFeedbackItem item = new JPDictFeedbackItem
                {
                    SuggestJpChar=jpchar_Tbx.Text,SuggestKana=kana_Tbx.Text,SuggestExplanation=explanation_Tbx.Text,Comment=comments_Tbx.Text,Email=email_Tbx.Text
                };
                try
                {
                    await App.MobileService.GetTable<JPDictFeedbackItem>().InsertAsync(item);
                    await (new MessageDialog("发送成功，我们会认真审阅您的反馈并完善我们的词库", "成功")).ShowAsync();
                }
                catch
                {
                    await (new MessageDialog("发送失败，请检查您的网络连接", "出现错误")).ShowAsync();

                }

            }
            else
            {
                await (new MessageDialog("请正确填写所有必填信息", "出现错误")).ShowAsync();

            }
        }
    }
}
