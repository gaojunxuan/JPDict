using JapaneseDict.Models;
using JapaneseDict.GUI;
using Microsoft.ApplicationInsights;
using Microsoft.WindowsAzure.MobileServices;
using SQLite.Net;
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
using Microsoft.Services.Store.Engagement;
using Windows.Networking.PushNotifications;
using Microsoft.WindowsAzure.Messaging;
using System.Diagnostics;
using Windows.UI.Notifications;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=402347&clcid=0x409

namespace JapaneseDict
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            WindowsAppInitializer.InitializeAsync();
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            
        }
        private async void CopyMainDb()
        {
            Windows.Storage.StorageFolder storageFolder =Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///dict.db"));
            if (await storageFolder.TryGetItemAsync("dict.db") == null)
            {
                await file.CopyAsync(storageFolder, "dict.db");
            }
            if (await storageFolder.TryGetItemAsync("kanji.db") == null)
            {
                var kanjifile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///kanji.db"));
                await kanjifile.CopyAsync(storageFolder, "kanji.db", NameCollisionOption.ReplaceExisting);
            }
            if(await storageFolder.TryGetItemAsync("synclog.log") == null)
            {
                Windows.Storage.StorageFile logFile = await storageFolder.CreateFileAsync("synclog.log", Windows.Storage.CreationCollisionOption.OpenIfExists);
                Windows.Storage.StorageFile onlineFile = await storageFolder.CreateFileAsync("onlinelog.log", Windows.Storage.CreationCollisionOption.OpenIfExists);
            }
            var updatefile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///update.db"));
            await updatefile.CopyAsync(ApplicationData.Current.LocalFolder, "update.db", NameCollisionOption.ReplaceExisting);
        }

        private async void InitNotificationsAsync()
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
                var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                var hub = new NotificationHub("jpdictHub", "Endpoint=sb://jpdictnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=b7P3q6gSzqgLDiCIFOv8q62J7EUft7RQr3F6TEIfXMg=");
                var result = await hub.RegisterNativeAsync(channel.Uri);
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
        public static void InitNavigationConfigurationInThisAssembly()
		{
			MVVMSidekick.Startups.StartupFunctions.RunAllConfig();
		}
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        public static MobileServiceClient MobileService =new MobileServiceClient("https://skylarkjpdict.azurewebsites.net");

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            
            CopyMainDb();
            InitNotificationsAsync();
            //StoreServicesEngagementManager mgr=StoreServicesEngagementManager.GetDefault();
            //await mgr.RegisterNotificationChannelAsync();
            //Init MVVM-Sidekick Navigations:
            InitNavigationConfigurationInThisAssembly();

            Frame rootFrame = Window.Current.Content as Frame;
            

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
           
    }
        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            if (args is ToastNotificationActivatedEventArgs)
            {
                var toastActivationArgs = args as ToastNotificationActivatedEventArgs;
                Frame rootFrame = Window.Current.Content as Frame;

                CopyMainDb();
                InitNotificationsAsync();
                InitNavigationConfigurationInThisAssembly();
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
                    rootFrame.Navigate(typeof(MainPage),"update");
                }
                // Ensure the current window is active
                Window.Current.Activate();
                // Use the originalArgs variable to access the original arguments
                // that were passed to the app.
            }
        }
        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
