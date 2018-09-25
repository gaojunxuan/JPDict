using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JapaneseDict.GUI.ViewModels;
using Windows.Phone.UI.Input;
using JapaneseDict.Models;
using Windows.UI.Popups;
using System.Reactive.Linq;
using Windows.UI.Text;
using Windows.System.Profile;
using Windows.UI.Core;
using Windows.Media;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Windows.Foundation.Metadata;
using Microsoft.Toolkit.Uwp.UI.Animations;
using JapaneseDict.GUI.Extensions;
using Microsoft.Toolkit.Uwp.UI.Controls;
using JapaneseDict.GUI.Helpers;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Media.Imaging;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ExtendAcrylicIntoTitleBar();
            systemControls = SystemMediaTransportControls.GetForCurrentView();
            systemControls.ButtonPressed += SystemControls_ButtonPressed;
        }

        private async void SystemControls_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 switch (args.Button)
                 {
                     case SystemMediaTransportControlsButton.Play:
                         if (mediaEle.Source != null)
                         {
                             mediaEle.Play();
                             StopNHKRadiosPlay_Btn.Visibility = Visibility.Visible;
                             mainPivot.Focus(FocusState.Pointer);
                             ResumeNHKRadiosPlay_Btn.Visibility = Visibility.Collapsed;
                         }
                         break;
                     case SystemMediaTransportControlsButton.Pause:
                         if (mediaEle.Source != null)
                         {
                             mediaEle.Pause();
                             mainPivot.Focus(FocusState.Pointer);
                             StopNHKRadiosPlay_Btn.Visibility = Visibility.Collapsed;
                             ResumeNHKRadiosPlay_Btn.Visibility = Visibility.Visible;
                         }
                         break;
                     default:
                         break;
                 }
             });  
        }

        SystemMediaTransportControls systemControls;

        private void shareEverydaySentence_Btn_Click(object sender, RoutedEventArgs e)
        {
            SharingHelper.ShowShareUI("每日一句分享", ((Button)sender).Tag.ToString());
        }

        private async void showNotesEverydaySentence_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag != null)
            {
                await new MessageDialog(((Button)sender).Tag.ToString().Replace("<br />", "\n"), "注释").ShowAsync();
            }

        }

        private async void playEverydaySentence_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mainPivot.Focus(FocusState.Pointer);
                StopNHKRadiosPlay_Btn.Visibility = Visibility.Collapsed;
                listeningPosition_Slider.Visibility = Visibility.Collapsed;
                ((Button)sender).IsEnabled = false;
                mediaEle.Stop();
                mediaEle.Position = TimeSpan.FromMilliseconds(0);
                mediaEle.Source = new Uri(((Button)sender).Tag.ToString(), UriKind.Absolute);
                mediaEle.Play();
                ((Button)sender).IsEnabled = true;

            }
            catch
            {
                ((Button)sender).IsEnabled = true;
                await new MessageDialog("请检查网络连接", "播放失败").ShowAsync();
            }


        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Observable.FromEventPattern<AutoSuggestBoxTextChangedEventArgs>(QueryBox, "TextChanged").Throttle(TimeSpan.FromMilliseconds(900)).Subscribe(async x =>
                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                            {
                                QueryBox.ItemsSource = await QueryEngine.QueryEngine.MainDictQueryEngine.FuzzyQueryForUIAsync(StringHelper.ResolveReplicator(QueryBox.Text));
                                //await Task.Delay(500);
                            }));
            pageRoot.Focus(FocusState.Pointer);
        }

        private void noteBook_frame_Loaded(object sender, RoutedEventArgs e)
        {
            noteBook_frame.Navigate(typeof(NotebookPage));
        }


        private void settings_frame_Loaded(object sender, RoutedEventArgs e)
        {
            settings_frame.Navigate(typeof(SettingsPage));
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            mediaEle.Source = null;
            mediaEle.Stop();
            mainPivot.Focus(FocusState.Pointer);
            StopNHKRadiosPlay_Btn.Visibility = Visibility.Collapsed;
            ResumeNHKRadiosPlay_Btn.Visibility = Visibility.Collapsed;
            listeningPosition_Slider.Visibility = Visibility.Collapsed;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            mediaEle.Stop();
            systemControls.IsEnabled = true;
            systemControls.IsPlayEnabled = true;
            systemControls.IsPauseEnabled = true;
            systemControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
            if (e.Parameter!=null)
            {
                if(e.Parameter.ToString() == "update")
                    mainPivot.SelectedIndex = 4;
            }
        }

        private async void MediaEle_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => 
            {
                if (systemControls != null)
                {
                    switch (mediaEle.CurrentState)
                    {
                        case MediaElementState.Playing:
                            systemControls.PlaybackStatus = MediaPlaybackStatus.Playing;
                            break;
                        case MediaElementState.Paused:
                            systemControls.PlaybackStatus = MediaPlaybackStatus.Paused;
                            break;
                        case MediaElementState.Stopped:
                            systemControls.PlaybackStatus = MediaPlaybackStatus.Stopped;
                            break;
                        case MediaElementState.Closed:
                            systemControls.PlaybackStatus = MediaPlaybackStatus.Closed;
                            break;
                        default:
                            break;
                    }
                }
            });                
        }

        private async void playNHKRadio_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mainPivot.Focus(FocusState.Pointer);
                ResumeNHKRadiosPlay_Btn.Visibility = Visibility.Collapsed;
                mediaEle.Stop();
                var currentBtn = ((Button)sender);

                var tag = currentBtn.Tag.ToString();
                if (!string.IsNullOrEmpty(tag))
                {
                    mediaEle.Source = new Uri(tag, UriKind.Absolute);
                    mediaEle.Position = TimeSpan.FromMilliseconds(0);
                    mediaEle.Play();
                    StopNHKRadiosPlay_Btn.Visibility = Visibility.Visible;
                    listeningPosition_Slider.Visibility = Visibility.Visible;

                }
            }
            catch (Exception ex)
            {
                await new MessageDialog("详细信息:\n\n" + ex.ToString() + "\n\n你可以在联系作者时提供以上的错误信息", "出现错误").ShowAsync();
            }

        }
        private void translate_frame_Loaded(object sender, RoutedEventArgs e)
        {
            translate_frame.Navigate(typeof(TranslationPage));
        }

        private void StopNHKRadiosPlay_Btn_Click(object sender, RoutedEventArgs e)
        {
            mediaEle.Pause();
            mainPivot.Focus(FocusState.Pointer);
            StopNHKRadiosPlay_Btn.Visibility = Visibility.Collapsed;
            ResumeNHKRadiosPlay_Btn.Visibility = Visibility.Visible;
        }

        private void QueryBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Dict suggest = args.SelectedItem as Dict;
            if (suggest == null)
                return;
            sender.Text = suggest.Keyword;
        }

        private void mediaEle_MediaEnded(object sender, RoutedEventArgs e)
        {
            mainPivot.Focus(FocusState.Pointer);
            StopNHKRadiosPlay_Btn.Visibility = Visibility.Collapsed;
            listeningPosition_Slider.Visibility = Visibility.Collapsed;
        }
        private void ResumeNHKRadiosPlay_Btn_Click(object sender, RoutedEventArgs e)
        {
            mediaEle.Play();
            mainPivot.Focus(FocusState.Pointer);
            (sender as HyperlinkButton).Visibility = Visibility.Collapsed;
            StopNHKRadiosPlay_Btn.Visibility = Visibility.Visible;
        }
        //refresh the notebook list after recovering from backup
        private void mainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //switch(mainPivot.SelectedIndex)
            //{
            //    case 1:
            //        noteBook_frame.Navigate(typeof(NotebookPage));
            //        break;
            //    case 2:
            //        translate_frame.Navigate(typeof(TranslationPage));
            //        break;
            //    case 4:
            //        settings_frame.Navigate(typeof(SettingsPage));
            //        break;
            //}
            if ((sender as Pivot).SelectedIndex == 1)
            {
                if (NotebookPage.NeedRefresh)
                {
                    if (noteBook_frame.Content != null)
                        ((noteBook_frame.Content as NotebookPage).DataContext as NotebookViewModel).LoadData();
                    NotebookPage.NeedRefresh = false;
                }
            }
            if ((sender as Pivot).SelectedIndex == 0)
            {
                MainViewModel mainViewModel = this.DataContext as MainViewModel;
                if(mainViewModel!=null)
                {
                    mainViewModel.RaisePropertyChanged("UseNHKEasyNews");
                }
            }
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
        private void FlashcardGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
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

        private void FlashcardItem_Grid_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ScaleAnimation animation = new ScaleAnimation() { To = "1.005", Duration = TimeSpan.FromMilliseconds(300) };
            animation.StartAnimation(sender as UIElement);
            var shadowPanel = (sender as UIElement).GetFirstDescendantOfType<DropShadowPanel>();
            OpacityAnimation opacityAnimation = new OpacityAnimation() { To = 1, Duration = TimeSpan.FromMilliseconds(300) };
            opacityAnimation.StartAnimation(shadowPanel);
        }

        private void FlashcardItem_Grid_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ScaleAnimation animation = new ScaleAnimation() { To = "1", Duration = TimeSpan.FromMilliseconds(600) };
            animation.StartAnimation(sender as UIElement);
            var shadowPanel = (sender as UIElement).GetFirstDescendantOfType<DropShadowPanel>();
            OpacityAnimation opacityAnimation = new OpacityAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(600) };
            opacityAnimation.StartAnimation(shadowPanel);
        }

        private async void ReadNews_Btn_Click(object sender, RoutedEventArgs e)
        {
            string x = ((HyperlinkButton)sender).Tag?.ToString();
            if (!string.IsNullOrWhiteSpace(x))
            {
                if(!ApplicationViewHelper.Contains(x))
                {
                    int newViewId = await ApplicationViewHelper.CreateNewViewAsync(x, typeof(NewsReaderPage), navParameter: x);
                    CoreApplicationView newView = ApplicationViewHelper.GetViewFromId(newViewId);
                    bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId, ViewSizePreference.UseMore);
                }
                else
                {
                    int newViewId = ApplicationViewHelper.GetId(x);
                    if (newViewId != -1)
                        await ApplicationViewSwitcher.SwitchAsync(newViewId);
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

        private void ExtendAcrylicIntoTitleBar()
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(titleBarCtl);
            titleBarCtl.SetPaddingForMainPage();
        }
        private void QueryBox_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
        }

        private async void QueryBox_Drop(object sender, DragEventArgs e)
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
                        if (Frame.CanGoBack)
                            Frame.GoBack();
                        Frame.Navigate(typeof(ResultPage), keyword);
                    }
                }
            }
        }

        private async void ReadEasyNews_Btn_Click(object sender, RoutedEventArgs e)
        {
            string x = ((HyperlinkButton)sender).Tag?.ToString();
            if (!string.IsNullOrWhiteSpace(x))
            {
                if (!ApplicationViewHelper.Contains(x))
                {
                    int newViewId = await ApplicationViewHelper.CreateNewViewAsync(x, typeof(NewsReaderWithRubyPage), navParameter: x);
                    CoreApplicationView newView = ApplicationViewHelper.GetViewFromId(newViewId);
                    bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId, ViewSizePreference.UseMore);
                }
                else
                {
                    int newViewId = ApplicationViewHelper.GetId(x);
                    if (newViewId != -1)
                        await ApplicationViewSwitcher.SwitchAsync(newViewId);
                }
            }
        }

        private void easyNews_img_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var img = (sender as Image);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri("ms-appx:///Assets/imgnotfound.png");
            img.Source = bitmapImage;
        }
    }
}

