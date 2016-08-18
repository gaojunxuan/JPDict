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
using Windows.Phone.UI.Input;
using JapaneseDict.Models;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ResultPage : MVVMPage
    {
        string _keyword;
        public ResultPage()
            : this(null)
        {
            //if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            //{
            //    HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            //}
            //SetUpPageAnimation();
            //NavigationCacheMode = NavigationCacheMode.Disabled;
        }
        public ResultPage(ResultPage_Model model)
            : base(model)
        {
            this.InitializeComponent();

            /* because this page is cached, the constructor will not be triggered every time. */
            /* so the subscription of BackPressed should be in the handler of OnNavigatedTo event */
            /* because this event will be triggered every time when user navigate to this page */
            //if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            //{
            //    HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            //}
            //NavigationCacheMode = NavigationCacheMode.Disabled;
            this.RegisterPropertyChangedCallback(ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = this.ViewModel as ResultPage_Model;
            });
            StrongTypeViewModel = this.ViewModel as ResultPage_Model;
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
        public ResultPage_Model StrongTypeViewModel
        {
            get { return (ResultPage_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register("StrongTypeViewModel", typeof(ResultPage_Model), typeof(ResultPage), new PropertyMetadata(null));




        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
            if (e.Parameter!=null&!(e.Parameter is int))
            {
                _keyword = e.Parameter.ToString();
                if (_keyword != null)
                {
                    this.ViewModel = new ResultPage_Model(this._keyword);
                }
            }
            else
            {
                this.ViewModel = new ResultPage_Model(Convert.ToInt32(e.Parameter.ToString()));
                
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
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            //{
            //    HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

            //}
            //GC.Collect();
            base.OnNavigatedFrom(e);
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

            }
        }
        private void pageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            Observable.FromEventPattern<AutoSuggestBoxTextChangedEventArgs>(this.QueryBox, "TextChanged").Throttle(TimeSpan.FromMilliseconds(900)).Subscribe(async x =>
                            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                            {
                                this.QueryBox.ItemsSource = await QueryEngine.QueryEngine.MainDictQueryEngine.FuzzyQueryForUIAsync(Util.StringHelper.ResolveReplicator(QueryBox.Text));
                                //this.QueryBox.ItemsSource = await QueryEngine.QueryEngine.MainDictQueryEngine.FuzzyQueryForUIAsync(QueryBox.Text);
                                //await Task.Delay(500);
                            }));
        }

        private void AddToNote_Btn_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Visibility = Visibility.Collapsed;
            ((Button)((Button)sender).Tag).Visibility = Visibility.Visible;
        }

        private void RemoveFromNote_Btn_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Visibility = Visibility.Collapsed;
            ((Button)((Button)sender).Tag).Visibility = Visibility.Visible;
        }

        private void QueryBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            MainDict suggest = args.SelectedItem as MainDict;
            if (suggest == null)
                return;
            sender.Text = suggest.JpChar;
        }

        private void seeOnlineResult_Btn_Click(object sender, RoutedEventArgs e)
        {
            mainPivot.SelectedIndex = 1;
        }

        private void noteItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.GoBack();
            rootFrame.Navigate(typeof(ResultPage),((StackPanel)sender).Tag.ToString());
        }
    }
}
