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
    public sealed partial class KanaFlashcardPage : Page
    {
        public KanaFlashcardPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                bool result = Int32.TryParse(e.Parameter.ToString(), out int index);
                if (result)
                {
                    this.mainPivot.SelectedIndex = index;
                }
            }
            KanaFlashcardViewModel vm = new KanaFlashcardViewModel();
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
            this.DataContext = vm;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
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
