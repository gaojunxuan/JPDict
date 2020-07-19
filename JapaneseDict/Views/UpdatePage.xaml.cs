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
using JapaneseDict.OnlineService;
using Windows.Storage;
using System.Threading.Tasks;
using JapaneseDict.GUI.Helpers;
using Windows.UI.Core;
using JapaneseDict.Models;
using JapaneseDict.GUI;
using SQLite;
using Windows.ApplicationModel.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace JapaneseDict.GUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UpdatePage : Page
    {
        public UpdatePage()
        {
            InitializeComponent();
            ExtendAcrylicIntoTitleBar();
        }

        //private static SQLiteConnection _conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "dict.db"));
        private static SQLiteConnection _noteConn = new SQLiteConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "note.db")); 
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationCacheMode = NavigationCacheMode.Disabled;
            if (await ApplicationData.Current.LocalFolder.TryGetItemAsync("dict.db") != null)
            {
                var dict = await ApplicationData.Current.LocalFolder.GetFileAsync("dict.db");
                await dict.DeleteAsync();
            }
            var updateFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///update.db"));
            var mainDictFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///dict.db"));
            var kanjiDictFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///kanji.db"));
            await updateFile.CopyAsync(ApplicationData.Current.LocalFolder, "update.db", NameCollisionOption.ReplaceExisting);
            await mainDictFile.CopyAsync(ApplicationData.Current.LocalFolder, "dict.db", NameCollisionOption.ReplaceExisting);
            await kanjiDictFile.CopyAsync(ApplicationData.Current.LocalFolder, "kanji.db", NameCollisionOption.ReplaceExisting);
            await Task.Run(async() =>
            {
#pragma warning disable CS0612 // Type or member is obsolete
                 ///Merge Notebook
                _noteConn.CreateTable<UserDefDict>();
#pragma warning restore CS0612 // Type or member is obsolete
                _noteConn.CreateTable<Note>();
                _noteConn.Execute("INSERT INTO Note(ItemId,Keyword,Reading,Definition) SELECT OriginID,JpChar,Kana,Explanation FROM UserDefDict WHERE OriginId NOT IN (SELECT ItemId FROM Note);");
                await SetProgess(25);
                string notePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "note.db");
                QueryEngine.QueryEngine.MainDictQueryEngine._conn.Execute($"ATTACH '{notePath}' AS `note` KEY ''");
                QueryEngine.QueryEngine.MainDictQueryEngine._conn.Execute($"UPDATE Dict SET IsInNotebook = 1 WHERE ItemId IN (SELECT OriginID FROM note.UserDefDict)");
                await SetProgess(35);
                QueryEngine.QueryEngine.MainDictQueryEngine._conn.Execute($"DETACH DATABASE 'note'");
                _noteConn.Execute($"DROP TABLE IF EXISTS UserDefDict");
                _noteConn.Commit();


                ///Merge MainDict

                string updatepath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "update.db");
                QueryEngine.QueryEngine.MainDictQueryEngine._conn.Execute($"ATTACH '{updatepath}' AS `merge` KEY ''");
                await SetProgess(45);
                QueryEngine.QueryEngine.MainDictQueryEngine._conn.Execute("INSERT INTO Dict(ItemId,Keyword,Kanji,Reading,Pos,LoanWord,Definition,SeeAlso) SELECT AutoId,Keyword,Kanji,Reading,Pos,LoanWord,Definition,SeeAlso FROM merge.UpdateDict WHERE AutoId NOT IN (SELECT ItemId FROM Dict);");
                await SetProgess(85);
                QueryEngine.QueryEngine.MainDictQueryEngine._conn.Execute($"DETACH DATABASE 'merge'");

                //QueryEngine.QueryEngine.MainDictQueryEngine._conn.Commit();

            }).ContinueWith(async t=> 
            {
                //_conn.Close();
                UpdatePromptHelper.StoreState();
                _noteConn.Close();
                _noteConn.Dispose();
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => 
                {
                    (Window.Current.Content as Frame).Navigate(typeof(MainPage),"");
                    (Window.Current.Content as Frame).BackStack.Clear();
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                });
            });
        }
        private async Task SetProgess(double value)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => Update_ProgressBar.Value = value);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
        private void ExtendAcrylicIntoTitleBar()
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(titleBarCtl);
        }
    }
}
