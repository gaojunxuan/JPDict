using JapaneseDict.GUI.ViewModels;
using System.Reactive;
using System.Reactive.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Windows.Foundation.Metadata;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using JapaneseDict.GUI.Helpers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class KanjiFlashcardPage : Page
    {
        public KanjiFlashcardPage()
        {
            InitializeComponent();
            ExtendAcrylicIntoTitleBar();
        }
        private void ExtendAcrylicIntoTitleBar()
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(titleBarCtl);
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter!=null)
            {
                bool result = Int32.TryParse(e.Parameter.ToString(), out int jlpt);
                if (result)
                {
                    var kanjires = await QueryEngine.QueryEngine.KanjiDictQueryEngine.QueryAsync(jlpt);
                    kanjires.Shuffle();
                    DataContext = new KanjiFlashcardViewModel() { Kanji = new ObservableCollection<Kanjidict>(kanjires) };
                }
            }
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
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

        ImplicitAnimationCollection _implicitAnimations;
        private void EnsureImplicitAnimations()
        {
            if (_implicitAnimations == null)
            {
                var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;

                var offsetAnimation = compositor.CreateVector3KeyFrameAnimation();
                offsetAnimation.Target = nameof(Visual.Offset);
                offsetAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue");
                offsetAnimation.Duration = TimeSpan.FromMilliseconds(400);

                var animationGroup = compositor.CreateAnimationGroup();
                animationGroup.Add(offsetAnimation);

                _implicitAnimations = compositor.CreateImplicitAnimationCollection();
                _implicitAnimations[nameof(Visual.Offset)] = animationGroup;
            }
        }
        private void MenuGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (ApiInformation.IsTypePresent(
            typeof(ImplicitAnimationCollection).FullName))
            {
                var elementVisual = ElementCompositionPreview.GetElementVisual(args.ItemContainer);
                if (args.InRecycleQueue)
                {
                    elementVisual.ImplicitAnimations = null;
                }
                else
                {
                    EnsureImplicitAnimations();
                    elementVisual.ImplicitAnimations = _implicitAnimations;
                }
            }
        }
        private async void overlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationView.GetForCurrentView().IsViewModeSupported(ApplicationViewMode.CompactOverlay))
            {
                Frame.Navigate(typeof(CompactMainPage));
                await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay);
            }
        }
    }
}
