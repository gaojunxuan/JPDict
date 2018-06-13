using JapaneseDict.GUI.ViewModels;
using System.Reactive;
using System.Reactive.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
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
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Windows.Foundation.Metadata;
using JapaneseDict.GUI.Extensions;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Shapes;
using JapaneseDict.GUI.Helpers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ResultPage : Page
    {
        string _keyword;
        public ResultPage()
        {
            InitializeComponent();
            //if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            //{
            //    HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            //}
            //SetUpPageAnimation();
            //NavigationCacheMode = NavigationCacheMode.Disabled;
        }
        //public ResultPage(ResultViewModel model)
        //{
        //    this.InitializeComponent();

        //    /* because this page is cached, the constructor will not be triggered every time. */
        //    /* so the subscription of BackPressed should be in the handler of OnNavigatedTo event */
        //    /* because this event will be triggered every time when user navigate to this page */
        //}

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter!=null&!(e.Parameter is int))
            {
                _keyword = e.Parameter.ToString();
                if (_keyword != null)
                {
                    DataContext = new ResultViewModel(_keyword);
                }
            }
            else
            {
                DataContext = new ResultViewModel(Convert.ToInt32(e.Parameter.ToString()));
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
            base.OnNavigatedFrom(e);
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

            }
        }
        private void pageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            Observable.FromEventPattern<AutoSuggestBoxTextChangedEventArgs>(QueryBox, "TextChanged").Throttle(TimeSpan.FromMilliseconds(900)).Subscribe(async x =>
                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                            {
                                QueryBox.ItemsSource = await QueryEngine.QueryEngine.MainDictQueryEngine.FuzzyQueryForUIAsync(StringHelper.ResolveReplicator(QueryBox.Text));
                            }));
        }

        private void AddToNote_Btn_Click(object sender, RoutedEventArgs e)
        {
            mainPivot.Focus(FocusState.Programmatic);
            ((Button)sender).Visibility = Visibility.Collapsed;
            ((Button)((Button)sender).Tag).Visibility = Visibility.Visible;
        }

        private void RemoveFromNote_Btn_Click(object sender, RoutedEventArgs e)
        {
            mainPivot.Focus(FocusState.Programmatic);
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

        private void kanjiItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }
        string _content = "";
        private void explanationBox_Loaded(object sender, RoutedEventArgs e)
        {
            var content = (sender as TextBlock).Text;
            _content += content;
            if(_content!="")
            {
                if (!(_content.Contains("五 ]") | _content.Contains("一 ]") | _content.Contains("サ ]") | _content.Contains("カ ]") | content.Contains("動詞")))
                {
                    mainPivot.Items.Remove(mainPivot.Items[3]);
                }
            }
        }
        private void kanjiItem_Loaded(object sender, RoutedEventArgs e)
        {
            kanjinores_Tbx.Visibility = Visibility.Collapsed;
        }

        private void SeeAlso_Btn_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
                rootFrame.Navigate(typeof(ResultPage), StringHelper.ResolveReplicator((sender as Button).Content.ToString().Replace(" ", "").Replace(" ", "")));
            }
        }

        private void Kanji_GridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
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
        
        private void KanjiItem_Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ScaleAnimation animation = new ScaleAnimation() { To = "1.005", Duration = TimeSpan.FromMilliseconds(300) };
            animation.StartAnimation(sender as UIElement);
            var shadowPanel = (sender as UIElement).GetFirstDescendantOfType<DropShadowPanel>();
            OpacityAnimation opacityAnimation = new OpacityAnimation() { To = 1, Duration = TimeSpan.FromMilliseconds(300) };
            opacityAnimation.StartAnimation(shadowPanel);
        }

        private void KanjiItem_Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ScaleAnimation animation = new ScaleAnimation() { To = "1", Duration = TimeSpan.FromMilliseconds(600) };
            animation.StartAnimation(sender as UIElement);
            var shadowPanel = (sender as UIElement).GetFirstDescendantOfType<DropShadowPanel>();
            OpacityAnimation opacityAnimation = new OpacityAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(600) };
            opacityAnimation.StartAnimation(shadowPanel);
        }

        private void Path_Loaded(object sender, RoutedEventArgs e)
        {
            var geometry = (Geometry)XamlReader.Load("<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>"+ (sender as Path).Tag.ToString() + "</Geometry>");
            var transform = new ScaleTransform();
            transform.ScaleX = 0.4;
            transform.ScaleY = 0.4;
            geometry.Transform = transform;
            (sender as Path).Data = geometry;
        }
    }
}
