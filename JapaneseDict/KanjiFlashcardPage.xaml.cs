
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
using System.Collections.ObjectModel;
using JapaneseDict.Models;
using Windows.Phone.UI.Input;
using Windows.UI.Core;
using JapaneseDict.GUI.Extensions;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class KanjiFlashcardPage : MVVMPage
    {



        public KanjiFlashcardPage()
        {

            this.InitializeComponent();
            this.RegisterPropertyChangedCallback(ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = this.ViewModel as KanjiFlashcardPage_Model;
            });
            StrongTypeViewModel = this.ViewModel as KanjiFlashcardPage_Model;
        }


        public KanjiFlashcardPage_Model StrongTypeViewModel
        {
            get { return (KanjiFlashcardPage_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register("StrongTypeViewModel", typeof(KanjiFlashcardPage_Model), typeof(KanjiFlashcardPage), new PropertyMetadata(null));




        protected override async void OnNavigatedTo(NavigationEventArgs e)
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
            if (e.Parameter!=null)
            {
                int jlpt = 0;
                bool result = Int32.TryParse(e.Parameter.ToString(), out jlpt);
                if(result)
                {
                    var kanjires = await QueryEngine.QueryEngine.KanjiDictQueryEngine.QueryAsync(jlpt);
                    kanjires.Shuffle();
                    this.ViewModel = new KanjiFlashcardPage_Model() { Kanji = new ObservableCollection<Kanjidict>(kanjires) };
                }
            }
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
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

        private void showReading_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            hideReading_item.Visibility = Visibility.Visible;
        }

        private void hideReading_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            showReading_item.Visibility = Visibility.Visible;
        }
    }
}
