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
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Windows.Foundation.Metadata;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TranslationPage : Page
    {
        public TranslationPage()
        {
            InitializeComponent();
        }

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
            resultTbx.Text = "";
        }

        private void CopyTransResult_Btn_Click(object sender, RoutedEventArgs e)
        {
            var datapack = new DataPackage();
            datapack.SetText(resultTbx.Text);
            Clipboard.SetContent(datapack);
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

                var sizeAnimation = compositor.CreateVector2KeyFrameAnimation();
                sizeAnimation.Target = nameof(Visual.Size);
                sizeAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue");
                sizeAnimation.Duration = TimeSpan.FromMilliseconds(400);

                var animationGroup = compositor.CreateAnimationGroup();
                animationGroup.Add(offsetAnimation);
                animationGroup.Add(sizeAnimation);

                _implicitAnimations = compositor.CreateImplicitAnimationCollection();
                _implicitAnimations[nameof(Visual.Offset)] = animationGroup;
            }
        }
        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            if (ApiInformation.IsTypePresent(
            typeof(ImplicitAnimationCollection).FullName))
            {
                var resultVisual = ElementCompositionPreview.GetElementVisual(resultTbx);
                var originVisual = ElementCompositionPreview.GetElementVisual(originTbx);
                EnsureImplicitAnimations();
                resultVisual.ImplicitAnimations = _implicitAnimations;
                originVisual.ImplicitAnimations = _implicitAnimations;
            }
        }

        private void Jp2CnTransRdBtn_Checked(object sender, RoutedEventArgs e)
        {
            originTbx.FontFamily = new FontFamily("Yu Gothic UI");
            resultTbx.FontFamily = new FontFamily("Microsoft YaHei UI");
        }

        private void Cn2JpTransRdBtn_Checked(object sender, RoutedEventArgs e)
        {
            originTbx.FontFamily = new FontFamily("Microsoft YaHei UI");
            resultTbx.FontFamily = new FontFamily("Yu Gothic UI");
        }
    }
}
