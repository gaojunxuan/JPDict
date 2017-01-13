using MVVMSidekick.Views;
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



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : MVVMPage
    {

        public MainPage()
        {

            this.InitializeComponent();
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            }
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.RegisterPropertyChangedCallback(ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = this.ViewModel as MainPage_Model;
            });
            StrongTypeViewModel = this.ViewModel as MainPage_Model;
            //SetUpPageAnimation();
        }
        //private void SetUpPageAnimation()
        //{
        //    TransitionCollection collection = new TransitionCollection();
        //    NavigationThemeTransition theme = new NavigationThemeTransition();

        //    var info = new DrillInNavigationTransitionInfo();

        //    theme.DefaultNavigationTransitionInfo = info;
        //    collection.Add(theme);
        //    this.Transitions = collection;
        //}
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = false;
        }

        public MainPage_Model StrongTypeViewModel
        {
            get { return (MainPage_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register("StrongTypeViewModel", typeof(MainPage_Model), typeof(MainPage), new PropertyMetadata(null));

        private void shareEverydaySentence_Btn_Click(object sender, RoutedEventArgs e)
        {
            JapaneseDict.Util.SharingHelper.ShowShareUI("每日一句分享", ((Button)sender).Tag.ToString());
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
                StopNHKRadiosPlay_Btn.Visibility = Visibility.Collapsed;
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
                await new MessageDialog("请检查您的网络连接", "播放失败").ShowAsync();
            }


        }
        MainPage_Model vm = new MainPage_Model();
        private void MVVMPage_Loaded(object sender, RoutedEventArgs e)
        {
            Observable.FromEventPattern<AutoSuggestBoxTextChangedEventArgs>(this.QueryBox, "TextChanged").Throttle(TimeSpan.FromMilliseconds(900)).Subscribe(async x =>
                            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                            {
                                this.QueryBox.ItemsSource = await QueryEngine.QueryEngine.MainDictQueryEngine.FuzzyQueryForUIAsync(Util.StringHelper.ResolveReplicator(QueryBox.Text));
                                //await Task.Delay(500);
                            }));

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
            this.mediaEle.Source = null;
            this.mediaEle.Stop();
            StopNHKRadiosPlay_Btn.Visibility = Visibility.Collapsed;
            this.listeningPosition_Slider.Visibility = Visibility.Collapsed;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            base.OnNavigatedTo(e);
            this.mediaEle.Stop();
        }
        private async void playNHKRadio_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                await new MessageDialog("详细信息:\n\n" + ex.ToString() + "\n\n您可以在联系作者时提供以上的错误信息", "出现错误").ShowAsync();
            }

        }
        private void translate_frame_Loaded(object sender, RoutedEventArgs e)
        {
            translate_frame.Navigate(typeof(TranslationPage));
        }

        private void ShowFastListeningList_Btn_Click(object sender, RoutedEventArgs e)
        {
            ((HyperlinkButton)sender).SetValue(HyperlinkButton.FontWeightProperty, FontWeights.ExtraBold);
            ShowSlowListeningList_Btn.FontWeight = FontWeights.Light;
            ShowNormalListeningList_Btn.FontWeight = FontWeights.Light;
            listeningFast_List.Visibility = Visibility.Visible;
            listeningNormal_List.Visibility = Visibility.Collapsed;
            listeningSlow_List.Visibility = Visibility.Collapsed;
        }

        private void ShowNormalListeningList_Btn_Click(object sender, RoutedEventArgs e)
        {
            ((HyperlinkButton)sender).SetValue(HyperlinkButton.FontWeightProperty, FontWeights.ExtraBold);
            ShowSlowListeningList_Btn.FontWeight = FontWeights.Light;
            ShowFastListeningList_Btn.FontWeight = FontWeights.Light;
            listeningFast_List.Visibility = Visibility.Collapsed;
            listeningNormal_List.Visibility = Visibility.Visible;
            listeningSlow_List.Visibility = Visibility.Collapsed;
        }

        private void ShowSlowListeningList_Btn_Click(object sender, RoutedEventArgs e)
        {
            ((HyperlinkButton)sender).SetValue(HyperlinkButton.FontWeightProperty, FontWeights.ExtraBold);
            ShowNormalListeningList_Btn.FontWeight = FontWeights.Light;
            ShowFastListeningList_Btn.FontWeight = FontWeights.Light;
            listeningFast_List.Visibility = Visibility.Collapsed;
            listeningNormal_List.Visibility = Visibility.Visible;
            listeningSlow_List.Visibility = Visibility.Collapsed;
        }

        private void StopNHKRadiosPlay_Btn_Click(object sender, RoutedEventArgs e)
        {
            mediaEle.Pause();
            StopNHKRadiosPlay_Btn.Visibility = Visibility.Collapsed;
            ResumeNHKRadiosPlay_Btn.Visibility = Visibility.Visible;
        }

        private void QueryBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            MainDict suggest = args.SelectedItem as MainDict;
            if (suggest == null)
                return;
            sender.Text = suggest.JpChar;
        }

        private void adControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile"))
            {
                ad.Margin = new Thickness(24, 12, 0, 24);
            }
        }

        private void mediaEle_MediaEnded(object sender, RoutedEventArgs e)
        {
            StopNHKRadiosPlay_Btn.Visibility = Visibility.Collapsed;
            listeningPosition_Slider.Visibility = Visibility.Collapsed;
        }
        private void ResumeNHKRadiosPlay_Btn_Click(object sender, RoutedEventArgs e)
        {
            mediaEle.Play();
            (sender as HyperlinkButton).Visibility = Visibility.Collapsed;
            StopNHKRadiosPlay_Btn.Visibility = Visibility.Visible;
        }
    }
}
