using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Networking.PushNotifications;
using System.Diagnostics;
using Windows.UI.Notifications;
using Windows.UI.Core;
using Windows.Phone.UI.Input;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using JapaneseDict.GUI.Services;
using Microsoft.AppCenter.Push;
using Microsoft.AppCenter.Crashes;
using Microsoft.Services.Store.Engagement;
using JapaneseDict.GUI.Helpers;
using Windows.UI.Xaml.Media.Animation;
using Microsoft.QueryStringDotNET;
using Windows.UI.ViewManagement;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=402347&clcid=0x409
namespace JapaneseDict.GUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private Lazy<ActivationService> _activationService;
        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            _activationService = new Lazy<ActivationService>(CreateActivationService);
            Suspending += OnSuspending;
        }

        private async void CopyMainDb()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            if (await storageFolder.TryGetItemAsync("kanji.db") == null)
            {
                var kanjifile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///kanji.db"));
                await kanjifile.CopyAsync(storageFolder, "kanji.db", NameCollisionOption.ReplaceExisting);
            }
            if (await storageFolder.TryGetItemAsync("kanjirad.db") == null)
            {
                var kanjifile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///kanjirad.db"));
                await kanjifile.CopyAsync(storageFolder, "kanjirad.db", NameCollisionOption.ReplaceExisting);
            }
            Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
        }

        private async void InitOnlineServiceAsync()
        {
            try
            {
                var tileUpdater = Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication();
                IReadOnlyList<ScheduledTileNotification> plannedUpdated = tileUpdater.GetScheduledTileNotifications();
                tileUpdater.Clear();
                for (int i = 0, len = plannedUpdated.Count; i < len; i++)
                {
                    // The itemId value is the unique ScheduledTileNotification.Id assigned to the notification when it was created.
                    tileUpdater.RemoveFromSchedule(plannedUpdated[i]);
                }

                //var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                //var hub = new NotificationHub("jpdictHub", "Endpoint=sb://jpdictnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=b7P3q6gSzqgLDiCIFOv8q62J7EUft7RQr3F6TEIfXMg=");
                //var result = await hub.RegisterNativeAsync(channel.Uri);
                //AppCenter.Configure("de248288-41ba-4ca6-b857-4bfaa6758c63");
                StoreServicesEngagementManager engagementManager = StoreServicesEngagementManager.GetDefault();
                await engagementManager.RegisterNotificationChannelAsync();
                AppCenter.Start("de248288-41ba-4ca6-b857-4bfaa6758c63", typeof(Analytics), typeof(Crashes), typeof(Push));
            }
            catch
            {
                Debug.WriteLine("Registration failed");
            }
        //// Displays the registration ID so you know it was successful
        //if (result.RegistrationId != null)
        //{
        //    var dialog = new MessageDialog("Registration successful: " + result.RegistrationId);
        //    dialog.Commands.Add(new UICommand("OK"));
        //    await dialog.ShowAsync();
        //}
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name = "e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }

#endif
            CopyMainDb();
            InitOnlineServiceAsync();
            Helpers.ThemeHelper.SetThemeForJPDict();
            if (!e.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(e);
            }
        }

        private ActivationService CreateActivationService()
        {
            UpdatePromptHelper.LoadState();
            if (UpdatePromptHelper.Updated)
            {
                return new ActivationService(this, typeof(ViewModels.UpdateViewModel));
            }
            else
            {
                return new ActivationService(this, typeof(ViewModels.MainViewModel));
            }
        }
        protected override async void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
            await ActivationService.ActivateAsync(args);
            Helpers.ThemeHelper.SetThemeForJPDict();
            if (args is ToastNotificationActivatedEventArgs)
            {
                var toastActivationArgs = args as ToastNotificationActivatedEventArgs;
                QueryString parameters= QueryString.Parse(toastActivationArgs.Argument);
                Frame rootFrame = Window.Current.Content as Frame;
                if (parameters.Contains("action"))
                {
                    switch (parameters["action"])
                    {
                        case "detailResult":
                            string keyword = parameters["keyword"];
                            if (rootFrame == null)
                            {
                                rootFrame = new Frame();
                                rootFrame.NavigationFailed += OnNavigationFailed;
                                // Place the frame in the current Window
                                Window.Current.Content = rootFrame;
                            }
                            if (rootFrame.Content == null)
                            {
                                rootFrame.Navigate(typeof(MainPage));
                                rootFrame.Navigate(typeof(ResultPage), keyword);
                            }
                            else
                            {
                                if (rootFrame.CanGoBack)
                                    rootFrame.GoBack();
                                rootFrame.Navigate(typeof(ResultPage), keyword);
                                await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);
                            }
                            break;
                    }
                }

                CopyMainDb();
                InitOnlineServiceAsync();
                Helpers.ThemeHelper.SetThemeForJPDict();
                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (rootFrame == null)
                {
                    // Create a Frame to act as the navigation context and navigate to the first page
                    rootFrame = new Frame();
                    rootFrame.NavigationFailed += OnNavigationFailed;
                    // Place the frame in the current Window
                    Window.Current.Content = rootFrame;
                }

                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    UpdatePromptHelper.LoadState();
                    if(UpdatePromptHelper.Updated)
                    {
                        rootFrame.Navigate(typeof(UpdatePage));
                    }
                    else
                    {
                        rootFrame.Navigate(typeof(MainPage));
                    }
                }

                // Ensure the current window is active
                Window.Current.Activate();
                StoreServicesEngagementManager engagementManager = StoreServicesEngagementManager.GetDefault();
                string originalArgs = engagementManager.ParseArgumentsAndTrackAppLaunch(toastActivationArgs.Argument);

                // Use the originalArgs variable to access the original arguments
                // that were passed to the app.
            }

            if (args.Kind == ActivationKind.Protocol)
            {
                var uriArgs = args as ProtocolActivatedEventArgs;
                if (uriArgs != null)
                {
                    Frame rootFrame = Window.Current.Content as Frame;
                    CopyMainDb();
                    InitOnlineServiceAsync();
                    // Do not repeat app initialization when the Window already has content,
                    // just ensure that the window is active
                    if (rootFrame == null)
                    {
                        // Create a Frame to act as the navigation context and navigate to the first page
                        rootFrame = new Frame();
                        rootFrame.NavigationFailed += OnNavigationFailed;
                        // Place the frame in the current Window
                        Window.Current.Content = rootFrame;
                    }
                    if (rootFrame.Content == null)
                    {
                        if (uriArgs.Uri.Host == "result")
                        {
                            //rootFrame.BackStack.Insert(0,new PageStackEntry(typeof(MainPage), null, new EntranceNavigationTransitionInfo()));
                            rootFrame.Navigate(typeof(MainPage));
                            rootFrame.Navigate(typeof(ResultPage),Uri.UnescapeDataString(uriArgs.Uri.Query.Replace("?keyword=","")));
                        }
                    }

                    // Ensure the current window is active
                    Window.Current.Activate();
                    
                }
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name = "sender">The Frame which failed navigation</param>
        /// <param name = "e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name = "sender">The source of the suspend request.</param>
        /// <param name = "e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = ((Frame)sender).CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;
            //if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            //{
            //    HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            //}
        }

        #region Legacy Code for Handling Hardware Button on Windows Phone
        //private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        //{
        //    Frame rootFrame = Window.Current.Content as Frame;
        //    if (rootFrame.CanGoBack)
        //    {
        //        e.Handled = true;
        //        rootFrame.GoBack();
        //    }
        //    else
        //    {
        //        //e.Handled = false;
        //        if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
        //        {
        //            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        //        }
        //    }
        //}
        #endregion

        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;
            if (rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }
    }
}