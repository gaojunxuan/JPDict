using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace JapaneseDict.GUI.Helpers
{
    public static class ThemeHelper
    {
        public static void SetThemeForJPDict()
        {
            SetTitleBar(Color.FromArgb(100, 0, 178, 148), Colors.White, Color.FromArgb(100, 0, 200, 166), Color.FromArgb(100, 0, 219, 182));
            SetStatusBar(Color.FromArgb(100, 0, 178, 148), Colors.White);
        }
        private static void SetTitleBar(Color bc, Color fc, Color btnHoverColor, Color btnPressedColor)
        {
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = bc;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = fc;
            titleBar.InactiveBackgroundColor = bc;
            titleBar.InactiveForegroundColor = fc;
            titleBar.ButtonInactiveBackgroundColor = bc;
            titleBar.ButtonInactiveForegroundColor = fc;
            titleBar.ButtonHoverBackgroundColor = btnHoverColor;
            titleBar.ButtonPressedBackgroundColor = btnPressedColor;
            titleBar.ForegroundColor = fc;
        }
        private static async void SetStatusBar(Color bc, Color fc)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.BackgroundColor = bc;
                statusBar.ForegroundColor = fc;
                statusBar.BackgroundOpacity = 1;
                await statusBar.ShowAsync();
            }
        }
    }
}
