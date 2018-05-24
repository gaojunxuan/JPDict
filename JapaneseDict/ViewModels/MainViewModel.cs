using System.Reactive.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using JapaneseDict.Models;
using JapaneseDict.OnlineService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JapaneseDict.GUI;

namespace JapaneseDict.GUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
        public MainViewModel()
        {
            LoadData();
        }
        async void LoadData()
        {
            await GetEverydaySentence();
            await GetNHKNews();
            await GetListening();
        }
        async Task GetEverydaySentence()
        {
            this.DailySentence = new ObservableCollection<EverydaySentence>();
            for (int i = 0; i < 3; i++)
            {
                this.DailySentence.Add((await JapaneseDict.OnlineService.JsonHelper.GetEverydaySentence(i)));
                //await Task.Delay(100);
            }
        }
        async Task GetNHKNews()
        {
            var res = await JapaneseDict.OnlineService.JsonHelper.GetNHKNews();
            this.NHKNews = new ObservableCollection<NHKNews>(res.Take(4));
        }
        async Task GetListening()
        {
            this.NHKListeningSlow = new ObservableCollection<NHKRadios>();
            this.NHKListeningNormal = new ObservableCollection<NHKRadios>();
            this.NHKListeningFast = new ObservableCollection<NHKRadios>();
            for (int i = 0; i < await OnlineService.JsonHelper.GetNHKRadiosItemsCount()-1; i++)
            {
                this.NHKListeningSlow.Add(await JsonHelper.GetNHKRadios(i, "slow"));
                this.NHKListeningNormal.Add(await JsonHelper.GetNHKRadios(i, "normal"));
                this.NHKListeningFast.Add(await JsonHelper.GetNHKRadios(i, "fast"));
            }
        }
        const string CLIENT_ID = "skylark_jpdict";
        const string CLIENT_SECRET = "uzHa5qUm4+GehYnL2pMIw8XtNox8sbqGNq7S+UiM6bk=";
        SpeechSynthesizer transServ = new SpeechSynthesizer(CLIENT_ID, CLIENT_SECRET);

        private RelayCommand<string> _querywordsCommand;
        /// <summary>
        /// Gets the QueryWordCommand.
        /// </summary>
        public RelayCommand<string> QueryWordsCommand
        {
            get
            {
                return _querywordsCommand
                    ?? (_querywordsCommand = new RelayCommand<string>(
                    (x) =>
                    {
                        if (!string.IsNullOrWhiteSpace(x) && (x != "Windows.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs"))
                        {
                            Frame rootFrame = Window.Current.Content as Frame;
                            rootFrame.Navigate(typeof(ResultPage), Util.StringHelper.ResolveReplicator(x.ToString().Replace(" ", "").Replace(" ", "")));
                        }
                    }));
            }
        }

        private RelayCommand<string> _navToKanjiFlashcardPageCommand;
        /// <summary>
        /// Gets the NavToKanjiFlashcardPage.
        /// </summary>
        public RelayCommand<string> NavToKanjiFlashcardPageCommand
        {
            get
            {
                return _navToKanjiFlashcardPageCommand
                    ?? (_navToKanjiFlashcardPageCommand = new RelayCommand<string>(
                    (x) =>
                    {
                        bool result = Int32.TryParse(x, out int jlpt);
                        if (result)
                        {
                            (Window.Current.Content as Frame).Navigate(typeof(KanjiFlashcardPage), jlpt);
                        }
                    }));
            }
        }

        private RelayCommand<string> _navToKanaFlashcardPageCommand;
        /// <summary>
        /// Gets the NavToKanjiFlashcardPage.
        /// </summary>
        public RelayCommand<string> NavToKanaFlashcardPageCommand
        {
            get
            {
                return _navToKanaFlashcardPageCommand
                    ?? (_navToKanaFlashcardPageCommand = new RelayCommand<string>(
                    (x) =>
                    {
                        bool result = Int32.TryParse(x, out int index);
                        if (result)
                        {
                            (Window.Current.Content as Frame).Navigate(typeof(KanaFlashcardPage), index);
                        }
                    }));
            }
        }

        private ObservableCollection<EverydaySentence> dailySentence;
        public ObservableCollection<EverydaySentence> DailySentence
        {
            get { return dailySentence; }
            set
            {
                dailySentence = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<NHKNews> nhkNews;
        public ObservableCollection<NHKNews> NHKNews
        {
            get { return nhkNews; }
            set
            {
                nhkNews = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<NHKRadios> nhkListeningSlow;
        public ObservableCollection<NHKRadios> NHKListeningSlow
        {
            get { return nhkListeningSlow; }
            set
            {
                nhkListeningSlow = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<NHKRadios> nhkListeningNormal;
        public ObservableCollection<NHKRadios> NHKListeningNormal
        {
            get { return nhkListeningNormal; }
            set
            {
                nhkListeningNormal = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<NHKRadios> nhkListeningFast;
        public ObservableCollection<NHKRadios> NHKListeningFast
        {
            get { return nhkListeningFast; }
            set
            {
                nhkListeningFast = value;
                RaisePropertyChanged();
            }
        }      
    }
}

