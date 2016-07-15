
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
using Windows.ApplicationModel.DataTransfer;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TranslationPage : MVVMPage
    {



        public TranslationPage()
        {
            this.InitializeComponent();
            this.RegisterPropertyChangedCallback(ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = this.ViewModel as TranslationPage_Model;
            });
            StrongTypeViewModel = this.ViewModel as TranslationPage_Model;
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }


        public TranslationPage_Model StrongTypeViewModel
        {
            get { return (TranslationPage_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register("StrongTypeViewModel", typeof(TranslationPage_Model), typeof(TranslationPage), new PropertyMetadata(null));




        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
        private void ClearTrans_Btn_Click(object sender, RoutedEventArgs e)
        {
            originTbx.Text = "";
        }

        private void CopyTransResult_Btn_Click(object sender, RoutedEventArgs e)
        {
            var datapack = new DataPackage();
            datapack.SetText(resultTbx.Text);
            Clipboard.SetContent(datapack);
        }
    }
}
