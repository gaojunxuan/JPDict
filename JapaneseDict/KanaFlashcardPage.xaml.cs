
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
using System.Collections.ObjectModel;
using static JapaneseDict.Util.KanaFlashcardHelper;
using Windows.Phone.UI.Input;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class KanaFlashcardPage : MVVMPage
    {



        public KanaFlashcardPage()
        {

            this.InitializeComponent();
            this.RegisterPropertyChangedCallback(ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = this.ViewModel as KanaFlashcardPage_Model;
            });
            StrongTypeViewModel = this.ViewModel as KanaFlashcardPage_Model;
        }


        public KanaFlashcardPage_Model StrongTypeViewModel
        {
            get { return (KanaFlashcardPage_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register("StrongTypeViewModel", typeof(KanaFlashcardPage_Model), typeof(KanaFlashcardPage), new PropertyMetadata(null));




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
                int index = 0;
                bool result = Int32.TryParse(e.Parameter.ToString(), out index);
                if (result)
                {
                    this.mainPivot.SelectedIndex = index;
                }
            }
            KanaFlashcardPage_Model vm = new KanaFlashcardPage_Model();
            var hirares = KanaFlashcardHelper.GetRandomHiragana();
            foreach (var i in hirares)
            {
                i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
            }
            vm.Hiragana = new ObservableCollection<Kana>(hirares);
            vm.Hiragana.Remove(vm.Hiragana.Where(k=>k.Content=="ゐ").First());
            vm.Hiragana.Remove(vm.Hiragana.Where(k => k.Content == "ゑ").First());

            var katares = KanaFlashcardHelper.GetRandomKatakana();
            foreach (var i in katares)
            {
                i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
            }
            vm.Katakana = new ObservableCollection<Kana>(katares);
            vm.Katakana.Remove(vm.Katakana.Where(k => k.Content == "ヰ").First());
            vm.Katakana.Remove(vm.Katakana.Where(k => k.Content == "ヱ").First());
            this.ViewModel = vm;
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

        private void showHiraSonant_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            hideHiraSonant_item.Visibility = Visibility.Visible;
            orderHira_item.Visibility = Visibility.Visible;
            disorderHira_item.Visibility = Visibility.Collapsed;
        }

        private void hideHiraSonant_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            showHiraSonant_item.Visibility = Visibility.Visible;
            orderHira_item.Visibility = Visibility.Visible;
            disorderHira_item.Visibility = Visibility.Collapsed;
        }

        private void showhistoryhira_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            hidehistoryhira_item.Visibility = Visibility.Visible;
            orderHira_item.Visibility = Visibility.Visible;
            disorderHira_item.Visibility = Visibility.Collapsed;
        }

        private void hidehistoryhira_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            showhistoryhira_item.Visibility = Visibility.Visible;
            orderHira_item.Visibility = Visibility.Visible;
            disorderHira_item.Visibility = Visibility.Collapsed;
        }

        private void showHiraRomaji_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            hideHiraRomaji_item.Visibility = Visibility.Visible;
        }

        private void hideHiraRomaji_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            showHiraRomaji_item.Visibility = Visibility.Visible;
        }

        private void orderHira_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            disorderHira_item.Visibility = Visibility.Visible;
            showHiraSonant_item.Visibility = Visibility.Collapsed;
            hideHiraSonant_item.Visibility = Visibility.Collapsed;
            showhistoryhira_item.Visibility = Visibility.Collapsed;
            hidehistoryhira_item.Visibility = Visibility.Collapsed;
            replayHira_item.Visibility = Visibility.Collapsed;
        }

        private void disorderHira_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            orderHira_item.Visibility = Visibility.Visible;
            showHiraSonant_item.Visibility = Visibility.Visible;
            hideHiraSonant_item.Visibility = Visibility.Collapsed;
            showhistoryhira_item.Visibility = Visibility.Visible;
            hidehistoryhira_item.Visibility = Visibility.Collapsed;
            replayHira_item.Visibility = Visibility.Visible;
        }

        private void replayHira_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            orderHira_item.Visibility = Visibility.Visible;
            disorderHira_item.Visibility = Visibility.Collapsed;
        }

        private void showKataSonant_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            hideKataSonant_item.Visibility = Visibility.Visible;
            orderKata_item.Visibility = Visibility.Visible;
            disorderKata_item.Visibility = Visibility.Collapsed;
        }

        private void hideKataSonant_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            showKataSonant_item.Visibility = Visibility.Visible;
            orderKata_item.Visibility = Visibility.Visible;
            disorderKata_item.Visibility = Visibility.Collapsed;
        }

        private void showhistoryKata_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            hidehistoryKata_item.Visibility = Visibility.Visible;
            orderKata_item.Visibility = Visibility.Visible;
            disorderKata_item.Visibility = Visibility.Collapsed;
        }

        private void hidehistoryKata_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            showhistoryKata_item.Visibility = Visibility.Visible;
            orderKata_item.Visibility = Visibility.Visible;
            disorderKata_item.Visibility = Visibility.Collapsed;
        }

        private void showKataRomaji_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            hideKataRomaji_item.Visibility = Visibility.Visible;
        }

        private void hideKataRomaji_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            showKataRomaji_item.Visibility = Visibility.Visible;
        }

        private void replayKata_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            orderKata_item.Visibility = Visibility.Visible;
            disorderKata_item.Visibility = Visibility.Collapsed;
        }

        private void orderKata_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            disorderKata_item.Visibility = Visibility.Visible;
            showKataSonant_item.Visibility = Visibility.Collapsed;
            hideKataSonant_item.Visibility = Visibility.Collapsed;
            showhistoryKata_item.Visibility = Visibility.Collapsed;
            hidehistoryKata_item.Visibility = Visibility.Collapsed;
            replayKata_item.Visibility = Visibility.Collapsed;
        }

        private void disorderKata_item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            (sender as UIElement).Visibility = Visibility.Collapsed;
            orderKata_item.Visibility = Visibility.Visible;
            showKataSonant_item.Visibility = Visibility.Visible;
            hideKataSonant_item.Visibility = Visibility.Collapsed;
            showhistoryKata_item.Visibility = Visibility.Visible;
            hidehistoryKata_item.Visibility = Visibility.Collapsed;
            replayKata_item.Visibility = Visibility.Visible;
        }
    }
}
