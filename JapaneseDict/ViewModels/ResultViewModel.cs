using System.Reactive;
using System.Reactive.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using Windows.UI.Xaml;
using System.Threading;
using JapaneseDict.Models;
using JapaneseDict.OnlineService;
using Windows.UI.Popups;
using System.Net.Http;
using Windows.Storage.Streams;
using System.Text.RegularExpressions;
using JapaneseDict.GUI.Models;
using GalaSoft.MvvmLight.Ioc;
using JapaneseDict.GUI.Helpers;

namespace JapaneseDict.GUI.ViewModels
{
    public class ResultViewModel : ViewModelBase
    {
        private string keyword;

        public string Keyword
        {
            get { return keyword; }
            set
            {
                keyword = value;
                RaisePropertyChanged();
            }
        }


        public ResultViewModel(string keyword)
        {
            Keyword = keyword;
            QueryWord();
            if (IsInDesignMode)
            {
                Keyword = "あ";
            }
            
        }
        public ResultViewModel(int id)
        {
            QueryWord(id);
            if (IsInDesignMode)
            {
                Keyword = "あ";
            }

        }
        [PreferredConstructor]
        public ResultViewModel()
        {
            if (IsInDesignMode)
            {
                Keyword = "あ";
            }
        }
        
        private async void QueryWord()
        {
            IsLocalQueryBusy = true;
            var queryResult = await QueryEngine.QueryEngine.MainDictQueryEngine.QueryForUIAsync(Keyword);
            if (queryResult.Count != 0 && queryResult.FirstOrDefault().Definition == "没有本地释义")
            {
                
                var seealsoresults = await QueryEngine.QueryEngine.MainDictQueryEngine.QueryForUIAsync(await OnlineService.Helpers.JsonHelper.GetLemmatized(keyword));
                if (seealsoresults.Count != 0 && seealsoresults.FirstOrDefault().Definition != "没有本地释义")
                {
                    queryResult = seealsoresults;
                }
                else
                {
                    string word = StringHelper.PrepareVerbs(Keyword);
                    seealsoresults = await QueryEngine.QueryEngine.MainDictQueryEngine.QueryForUIAsync(word);
                    if (seealsoresults.Count != 0 && seealsoresults.FirstOrDefault().Definition != "没有本地释义")
                    {
                        queryResult = await QueryEngine.QueryEngine.MainDictQueryEngine.QueryForUIAsync(seealsoresults.First().Keyword);
                    }
                }
                var pv = Windows.ApplicationModel.Package.Current.Id.Version;
                Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Word not found", new Dictionary<string, string>
                {
                    { "Keyword", Keyword },
                    { "Version", $"{pv.Major}.{pv.Minor}.{pv.Build}.{pv.Revision}" }
                });
            }
            var grouped = queryResult.GroupBy(x => x.ItemId).Select(g=>new GroupedDictItem(g));
            Result = new ObservableCollection<GroupedDictItem>(grouped);
            IsLocalQueryBusy = false;
            QueryVerb();
            await QueryKanji();
            await QueryOnline();      
        }
        private async void QueryWord(int id)
        {
            IsLocalQueryBusy = true;
            var queryResult = await QueryEngine.QueryEngine.MainDictQueryEngine.QueryForUIAsync(id);
            if(queryResult != null && queryResult.Count!=0)
            {
                Keyword = queryResult.First().Keyword;
            }
            var grouped = queryResult.GroupBy(x => x.ItemId).Select(g => new GroupedDictItem(g));
            Result = new ObservableCollection<GroupedDictItem>(grouped);
            IsLocalQueryBusy = false;
            QueryVerb();
            await QueryKanji();
            await QueryOnline();
        }        
        private void QueryVerb()
        {
            var vbres = new ObservableCollection<Verb>();
            foreach (var i in Result)
            {
                foreach (var x in i)
                {
                    if (!string.IsNullOrEmpty(x.Pos))
                    {
                        if (x.Pos.Contains("五") | x.Pos.Contains("一") | x.Pos.Contains("サ") | x.Pos.Contains("カ") | x.Pos.Contains("動詞"))
                        {
                            if (vbres != null)
                            {
                                string keyword = x.Keyword;
                                if (x.Pos.Contains("サ") && !x.Keyword.EndsWith("する"))
                                {
                                    keyword += "する";
                                }
                                vbres.Add(new Verb()
                                {
                                    Causative = VerbConjugationHelper.GetCausative(keyword, x.Pos),
                                    EbaForm = VerbConjugationHelper.GetEbaForm(keyword, x.Pos),
                                    Imperative = VerbConjugationHelper.GetImperative(keyword, x.Pos),
                                    MasuForm = VerbConjugationHelper.GetMasuForm(keyword, x.Pos),
                                    MasuNegative = VerbConjugationHelper.GetMasuNegative(keyword, x.Pos),
                                    NegativeCausative = VerbConjugationHelper.GetNegativeCausative(keyword, x.Pos),
                                    NegativeForm = VerbConjugationHelper.GetNegative(keyword, x.Pos),
                                    NegativeImperative = VerbConjugationHelper.GetNegativeImperative(keyword),
                                    NegativePassive = VerbConjugationHelper.GetNegativePassive(keyword, x.Pos),
                                    NegativePotential = VerbConjugationHelper.GetNegativePotential(keyword, x.Pos),
                                    OriginalForm = keyword,
                                    Passive = VerbConjugationHelper.GetPassive(keyword, x.Pos),
                                    PastNegative = VerbConjugationHelper.GetPastNegative(keyword, x.Pos),
                                    Potential = VerbConjugationHelper.GetPotential(keyword, x.Pos),
                                    TaForm = VerbConjugationHelper.GetTaForm(keyword, x.Pos),
                                    TeForm = VerbConjugationHelper.GetTeForm(keyword, x.Pos),
                                    Volitional = VerbConjugationHelper.GetVolitional(keyword, x.Pos)
                                });
                            }
                        }
                    }
                }


            }
            VerbResult = vbres;
        }
        private async Task QueryOnline()
        {
            IsOnlineQueryBusy = true;
            OnlineResult = await OnlineQueryEngine.Query(Keyword);
            IsOnlineQueryBusy = false;
        }
        private async Task QueryKanji()
        {
            Regex reg = new Regex("[\u4e00-\u9fa5]+");
            Regex en_reg = new Regex("[A-z\\s]+");
            StringBuilder sbkey = new StringBuilder();
            foreach (var i in Result)
            {
                sbkey.Append(i.Kanji);
                sbkey.Append(i.Keyword);
            }
            var matches = reg.Matches(sbkey.ToString());
            KanjiResult = await QueryEngine.QueryEngine.KanjiDictQueryEngine.QueryForUIAsync(matches);
        }

        private ObservableCollection<GroupedDictItem> result;
        public ObservableCollection<GroupedDictItem> Result
        {
            get { return result; }
            set
            {
                result = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Kanjidict> kanjiResult;
        public ObservableCollection<Kanjidict> KanjiResult
        {
            get { return kanjiResult; }
            set
            {
                kanjiResult = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Verb> verbResult;
        public ObservableCollection<Verb> VerbResult
        {
            get { return verbResult; }
            set
            {
                verbResult = value;
                RaisePropertyChanged();
            }
        }

        private RelayCommand<object> _querywordsCommand;
        /// <summary>
        /// Gets the QueryWordsCommand.
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

                            if (rootFrame.CanGoBack)
                            {
                                rootFrame.GoBack();
                                rootFrame.Navigate(typeof(ResultPage), StringHelper.ResolveReplicator(x.ToString().Replace(" ", "").Replace(" ", "")));
                            }
                        }
                    }));
            }
        }
        
        private bool isOnlineQueryBusy;
        public bool IsOnlineQueryBusy
        {
            get { return isOnlineQueryBusy; }
            set
            {
                isOnlineQueryBusy = value;
                RaisePropertyChanged();
            }
        }

        private bool isLocalQueryBusy;
        public bool IsLocalQueryBusy
        {
            get { return isLocalQueryBusy; }
            set
            {
                isLocalQueryBusy = value;
                RaisePropertyChanged();
            }
        }

        private RelayCommand<string> _speakCommand;
        /// <summary>
        /// Gets the SpeakCommand.
        /// </summary>
        public RelayCommand<string> SpeakCommand
        {
            get
            {
                return _speakCommand
                    ?? (_speakCommand = new RelayCommand<string>(
                    async(x) =>
                    {
                        const string CLIENT_SECRET = "80d5b0ade63946efb57fd09d1d1db6fb";
                        try
                        {
                            SpeechSynthesizer speech = new SpeechSynthesizer(CLIENT_SECRET);
                            string text = x.ToString();
                            string language = "ja";
                            // Gets the audio stream.
                            var stream = await speech.GetSpeakStreamAsyncHelper(text, language);
                            MediaElement mediaEle = new MediaElement();
                            // Reproduces the audio stream using a MediaElement.
                            mediaEle.SetSource(stream, speech.MimeContentType);
                            mediaEle.Play();

                        }
                        catch (HttpRequestException)
                        {
                            await new MessageDialog("请检查您的网络连接", "出现错误").ShowAsync();
                        }
                    }));
            }
        }

        private RelayCommand<int> _addToNotebookCommand;
        /// <summary>
        /// Gets the AddToNotebookCommand.
        /// </summary>
        public RelayCommand<int> AddToNotebookCommand
        {
            get
            {
                return _addToNotebookCommand
                    ?? (_addToNotebookCommand = new RelayCommand<int>(
                    (x) =>
                    {
                        QueryEngine.QueryEngine.NotebookQueryEngine.Add(x);
                    }));
            }
        }

        private RelayCommand<int> _removeFromNotebookCommand;
        /// <summary>
        /// Gets the RemoveFromNotebookCommand.
        /// </summary>
        public RelayCommand<int> RemoveFromNotebookCommand
        {
            get
            {
                return _removeFromNotebookCommand
                    ?? (_removeFromNotebookCommand = new RelayCommand<int>(
                    (x) =>
                    {
                        QueryEngine.QueryEngine.NotebookQueryEngine.Remove(x);
                    }));
            }
        }

        private ObservableCollection<OnlineDict> onlineResult;
        public ObservableCollection<OnlineDict> OnlineResult
        {
            get { return onlineResult; }
            set
            {
                onlineResult = value;
                RaisePropertyChanged();
            }
        }        
    }

}

