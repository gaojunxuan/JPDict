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
using Windows.UI.Xaml.Media.Animation;
using JapaneseDict.GUI.Models;
using JapaneseDict.OnlineService.Helpers;
using JapaneseDict.GUI.Helpers;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;

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
            DailySentence = new ObservableCollection<EverydaySentence>();
            for (int i = 0; i < 3; i++)
            {
                DailySentence.Add((await JsonHelper.GetEverydaySentence(i)));
                //await Task.Delay(100);
            }
        }
        async Task GetNHKNews()
        {
            var res = await JsonHelper.GetNHKNews();
            NHKNews = new ObservableCollection<NHKNews>(res.Take(4));
            var easyres = await JsonHelper.GetNHKEasyNews();
            NHKEasyNews = new ObservableCollection<NHKNews>(easyres.Take(4));
        }
        async Task GetListening()
        {
            NHKListeningSlow = new ObservableCollection<NHKRadios>();
            NHKListeningNormal = new ObservableCollection<NHKRadios>();
            NHKListeningFast = new ObservableCollection<NHKRadios>();
            for (int i = 0; i < await JsonHelper.GetNHKRadiosItemsCount() - 1; i++)
            {
                NHKListeningSlow.Add(await JsonHelper.GetNHKRadios(i, "slow"));
                NHKListeningNormal.Add(await JsonHelper.GetNHKRadios(i, "normal"));
                NHKListeningFast.Add(await JsonHelper.GetNHKRadios(i, "fast"));
            }
        }
        const string CLIENT_ID = "skylark_jpdict";
        const string CLIENT_SECRET = "uzHa5qUm4+GehYnL2pMIw8XtNox8sbqGNq7S+UiM6bk=";
        SpeechSynthesizer transServ = new SpeechSynthesizer(CLIENT_ID, CLIENT_SECRET);

        private RelayCommand<object> _querywordsCommand;
        /// <summary>
        /// Gets the QueryWordCommand.
        /// </summary>
        public RelayCommand<object> QueryWordsCommand
        {
            get
            {
                return _querywordsCommand
                    ?? (_querywordsCommand = new RelayCommand<object>(
                    (x) =>
                    {
                        if (x is string && !string.IsNullOrWhiteSpace(x.ToString()))
                        {
                            Frame rootFrame = Window.Current.Content as Frame;
                            rootFrame.Navigate(typeof(ResultPage), StringHelper.ResolveReplicator(x.ToString().Replace(" ", "").Replace(" ", "")));
                        }
                    }));
            }
        }

        public ObservableCollection<PageInfo> FlashcardPageInfo
        {
            get
            {
                return new ObservableCollection<PageInfo>()
                {
                    new PageInfo(){ Name="平假名", Description="ひらがな", Icon="あ", TargetPageType=typeof(KanaFlashcardPage), Parameter="0" },
                    new PageInfo(){ Name="片假名", Description="カタカナ", Icon="カ", TargetPageType=typeof(KanaFlashcardPage), Parameter="1" },
                    new PageInfo(){ Name="N1 汉字", Description="JLPT 1", Icon="磯", TargetPageType=typeof(KanjiFlashcardPage), Parameter="1" },
                    new PageInfo(){ Name="N2 汉字", Description="JLPT 2", Icon="囲", TargetPageType=typeof(KanjiFlashcardPage), Parameter="2" },
                    new PageInfo(){ Name="N3 汉字", Description="JLPT 3", Icon="夏", TargetPageType=typeof(KanjiFlashcardPage), Parameter="3" },
                    new PageInfo(){ Name="N4 汉字", Description="JLPT 4", Icon="円", TargetPageType=typeof(KanjiFlashcardPage), Parameter="4" }
                };
            }
        }

        private RelayCommand<(Type, object)> _navToFlashcardPageCommand;
        /// <summary>
        /// Gets the NavToKanjiFlashcardPage.
        /// </summary>
        public RelayCommand<(Type, object)> NavToFlashcardPageCommand
        {
            get
            {
                return _navToFlashcardPageCommand
                    ?? (_navToFlashcardPageCommand = new RelayCommand<(Type, object)>(
                    (x) =>
                    {
                        if (x.Item1 == typeof(KanjiFlashcardPage))
                        {
                            (Window.Current.Content as Frame).Navigate(typeof(KanjiFlashcardPage), x.Item2);
                        }
                        else if (x.Item1 == typeof(KanaFlashcardPage))
                        {
                            (Window.Current.Content as Frame).Navigate(typeof(KanaFlashcardPage), x.Item2);
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

        private ObservableCollection<NHKNews> nHKEasyNews;

        public ObservableCollection<NHKNews> NHKEasyNews
        {
            get { return nHKEasyNews; }
            set
            {
                nHKEasyNews = value;
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
        public bool UseNHKEasyNews
        {
            get { return StorageHelper.GetSetting<bool>("UseNHKEasyNews"); }
            set
            {
                StorageHelper.StoreSetting("UseNHKEasyNews", value, true);
                StorageHelper.FlushToStorage();
                RaisePropertyChanged();
            }
        }
    }
}

