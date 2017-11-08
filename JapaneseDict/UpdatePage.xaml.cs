
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
using JapaneseDict.OnlineService;
using SQLite.Net;
using Windows.Storage;
using System.Threading.Tasks;
using JapaneseDict.GUI.Helpers;
using Windows.UI.Core;
using JapaneseDict.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UpdatePage : MVVMPage
    {



        public UpdatePage()
        {

            this.InitializeComponent();
            this.RegisterPropertyChangedCallback(ViewModelProperty, (_, __) =>
            {
                StrongTypeViewModel = this.ViewModel as UpdatePage_Model;
            });
            StrongTypeViewModel = this.ViewModel as UpdatePage_Model;
        }


        public UpdatePage_Model StrongTypeViewModel
        {
            get { return (UpdatePage_Model)GetValue(StrongTypeViewModelProperty); }
            set { SetValue(StrongTypeViewModelProperty, value); }
        }

        public static readonly DependencyProperty StrongTypeViewModelProperty =
                    DependencyProperty.Register("StrongTypeViewModel", typeof(UpdatePage_Model), typeof(UpdatePage), new PropertyMetadata(null));



        //private static SQLiteConnection _conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "dict.db"));

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationCacheMode = NavigationCacheMode.Disabled;
            var updatefile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///update.db"));
            await updatefile.CopyAsync(ApplicationData.Current.LocalFolder, "update.db", NameCollisionOption.ReplaceExisting);
            await Task.Run(async() =>
            {
                string updatepath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "update.db");
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => this.Update_ProgressBar.Value = 25);
                QueryEngine.QueryEngine.MainDictQueryEngine._conn.Execute($"ATTACH '{updatepath}' AS `tomerge` KEY ''");
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => this.Update_ProgressBar.Value = 35);
                QueryEngine.QueryEngine.MainDictQueryEngine._conn.Execute("INSERT INTO Maindict(Id,JpChar,Kana,Explanation) SELECT AutoId,JpChar,Reading,Defination FROM tomerge.UpdateDict WHERE AutoId NOT IN (SELECT Id from MainDict);");
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => this.Update_ProgressBar.Value = 65);
                QueryEngine.QueryEngine.MainDictQueryEngine._conn.Commit();

            }).ContinueWith(async t=> 
            {
                //_conn.Close();
                UpdatePromptHelper.StoreState();
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => 
                {
                    (Window.Current.Content as Frame).Navigate(typeof(MainPage),"");
                    (Window.Current.Content as Frame).BackStack.Clear();
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                });
            });
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

    }
}
