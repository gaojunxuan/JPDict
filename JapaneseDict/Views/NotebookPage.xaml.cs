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
using System.Diagnostics;
using JapaneseDict.Models;
using System.Threading.Tasks;
using JapaneseDict.GUI.Extensions;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Composition;
using Windows.Foundation.Metadata;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using JapaneseDict.GUI.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotebookPage : Page
    {
        public static bool NeedRefresh { get; set; }
        public NotebookPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.NavigationCacheMode = NavigationCacheMode.Disabled;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void SemanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            if (e.IsSourceZoomedInView == false)
            {
                e.DestinationItem.Item = e.SourceItem.Item;
            }
        }

        private void noteItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(ResultPage), Convert.ToInt32(((StackPanel)sender).Tag));
            //GC.Collect();
        }

        private void noteItem_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void noteItem_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void pageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            (pageRoot.DataContext as NotebookViewModel).LoadData();
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

        private void NotebookGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
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

        private void ZoomedOutGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            pageRoot.Focus(FocusState.Pointer);
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

        private void NoteItem_Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ScaleAnimation animation = new ScaleAnimation() { To = "1.005", Duration = TimeSpan.FromMilliseconds(300) };
            animation.StartAnimation(sender as UIElement);
            var shadowPanel = (sender as UIElement).GetFirstDescendantOfType<DropShadowPanel>();
            var delBtn = (sender as UIElement).GetFirstDescendantOfType<Button>();
            OpacityAnimation opacityAnimation = new OpacityAnimation() { To = 1, Duration = TimeSpan.FromMilliseconds(300) };
            OpacityAnimation buttonOpacityAnimation = new OpacityAnimation() { To = 1, Duration = TimeSpan.FromMilliseconds(100) };
            opacityAnimation.StartAnimation(shadowPanel);
            opacityAnimation.StartAnimation(delBtn);
        }

        private void NoteItem_Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ScaleAnimation animation = new ScaleAnimation() { To = "1", Duration = TimeSpan.FromMilliseconds(600) };
            animation.StartAnimation(sender as UIElement);
            DropShadowPanel shadowPanel = (sender as UIElement).GetFirstDescendantOfType<DropShadowPanel>();
            Button delBtn = (sender as UIElement).GetFirstDescendantOfType<Button>();
            OpacityAnimation opacityAnimation = new OpacityAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(600) };
            OpacityAnimation buttonOpacityAnimation = new OpacityAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(200) };
            opacityAnimation.StartAnimation(shadowPanel);
            buttonOpacityAnimation.StartAnimation(delBtn);
        }

        private void DeleteBtn_Loaded(object sender, RoutedEventArgs e)
        {
            ElementCompositionPreview.GetElementVisual(sender as UIElement).Opacity = 0;
        }
        private void NotebookGridView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ProgressRing_Loaded(object sender, RoutedEventArgs e)
        {
            this.pageRoot.Focus(FocusState.Pointer);
        }
    }
}
