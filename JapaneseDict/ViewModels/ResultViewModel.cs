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
using System.Runtime.Serialization;
using Windows.Phone.UI.Input;
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
using JapaneseDict.Util;
using JapaneseDict.GUI;
using GalaSoft.MvvmLight.Ioc;

namespace JapaneseDict.GUI.ViewModels
{
    public class ResultViewModel : ViewModelBase
    {
        
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property。
        string _keyword;

        public ResultViewModel(string keyword)
        {
            
            _keyword = keyword;
            QueryWord();
            if(IsInDesignMode)
            {
                this._keyword = "あ";
            }
            
        }
        public ResultViewModel(int id)
        {
            
            QueryWord(id);
            if (IsInDesignMode)
            {
                this._keyword = "あ";
            }

        }
        [PreferredConstructor]
        public ResultViewModel()
        {
            if (IsInDesignMode)
            {
                this._keyword = "あ";
            }
        }
        
        private async void QueryWord()
        {
            this.result = await QueryEngine.QueryEngine.MainDictQueryEngine.QueryForUIAsync(_keyword);
            if (result.Count != 0 && result.FirstOrDefault().Explanation == "没有本地释义")
            {
                string word = StringHelper.PrepareVerbs(_keyword);
                var seealsoresults = await QueryEngine.QueryEngine.MainDictQueryEngine.FuzzyQueryForUIAsync(word);
                if (seealsoresults.Count != 0 && seealsoresults.FirstOrDefault().Explanation != "没有本地释义")
                {
                    this.result.First().SeeAlso = seealsoresults.First().JpChar;
                }
            }
            this.Result = this.result;
            foreach (var i in this.result)
            {
                var content = i.Explanation;
            }
        }
        private async void QueryWord(int id)
        {
            this.result = await QueryEngine.QueryEngine.MainDictQueryEngine.QueryForUIAsync(id);
            this.Result = this.result;
            if(this.result!=null&&this.result.Count!=0)
            {
                this._keyword = this.result.First().JpChar;
            }
        }

        private ObservableCollection<MainDict> result;
        public ObservableCollection<MainDict> Result
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

        private RelayCommand<string> _querywordsCommand;
        /// <summary>
        /// Gets the QueryWordsCommand.
        /// </summary>
        public RelayCommand<string> QueryWordsCommand
        {
            get
            {
                return _querywordsCommand
                    ?? (_querywordsCommand = new RelayCommand<string>(
                    (x) =>
                    {
                        if ((!string.IsNullOrWhiteSpace(x.ToString())) && (x.ToString() != "Windows.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs"))
                        {
                            Frame rootFrame = Window.Current.Content as Frame;

                            if (rootFrame.CanGoBack)
                            {
                                rootFrame.GoBack();
                                rootFrame.Navigate(typeof(ResultPage), Util.StringHelper.ResolveReplicator(x.ToString().Replace(" ", "").Replace(" ", "")));
                            }
                        }
                    }));
            }
        }

        private RelayCommand _queryOnlineCommand;
        /// <summary>
        /// Gets the QueryOnlineCommand.
        /// </summary>
        public RelayCommand QueryOnlineCommand
        {
            get
            {
                return _queryOnlineCommand
                    ?? (_queryOnlineCommand = new RelayCommand(
                    async() =>
                    {
                        this.IsOnlineQueryBusy = true;
                        this.OnlineResult = await OnlineQueryEngine.Query(this.result.First().JpChar);
                        this.IsOnlineQueryBusy = false;
                    }));
            }
        }

        private RelayCommand _queryVerbCommand;
        /// <summary>
        /// Gets the QueryVerbCommand.
        /// </summary>
        public RelayCommand QueryVerbCommand
        {
            get
            {
                return _queryVerbCommand
                    ?? (_queryVerbCommand = new RelayCommand(
                    () =>
                    {
                        var vbres = new ObservableCollection<Verb>();
                        foreach (var i in this.Result)
                        {

                            var content = i.Explanation;
                            if (content.Contains("五 ]") | content.Contains("一 ]") | content.Contains("サ ]") | content.Contains("カ ]") | content.Contains("動詞"))
                            {
                                Regex reg = new Regex("[\u4e00-\u9fa5]+");
                                string keyword = i.JpChar;
                                var matches = reg.Matches(keyword);
                                if (matches.Count == 0)
                                {
                                    string newkey;
                                    if (content.Contains("\r"))
                                        newkey = content.Split('\r')[1];
                                    else
                                        newkey = content.Split('\n')[1];
                                    if (!string.IsNullOrEmpty(newkey) & !newkey.Contains("["))
                                    {
                                        if (newkey.Contains("；"))
                                        {
                                            keyword = newkey.Split('；')[0].Replace(" ", "").Replace("　", "");
                                        }
                                        else
                                        {
                                            keyword = newkey.Replace(" ", "").Replace("　", "");
                                        }
                                    }
                                }
                                if (content.Contains("サ") && !i.JpChar.EndsWith("する"))
                                {
                                    keyword += "する";
                                }

                                //vm.IsVerbQueryEnabled = Visibility.Visible;
                                vbres.Add(new Verb()
                                {
                                    Causative = VerbConjugationHelper.GetCausative(keyword, content),
                                    EbaForm = VerbConjugationHelper.GetEbaForm(keyword, content),
                                    Imperative = VerbConjugationHelper.GetImperative(keyword, content),
                                    MasuForm = VerbConjugationHelper.GetMasuForm(keyword, content),
                                    MasuNegative = VerbConjugationHelper.GetMasuNegative(keyword, content),
                                    NegativeCausative = VerbConjugationHelper.GetNegativeCausative(keyword, content),
                                    NegativeForm = VerbConjugationHelper.GetNegative(keyword, content),
                                    NegativeImperative = VerbConjugationHelper.GetNegativeImperative(keyword),
                                    NegativePassive = VerbConjugationHelper.GetNegativePassive(keyword, content),
                                    NegativePotential = VerbConjugationHelper.GetNegativePotential(keyword, content),
                                    OriginalForm = keyword,
                                    Passive = VerbConjugationHelper.GetPassive(keyword, content),
                                    PastNegative = VerbConjugationHelper.GetPastNegative(keyword, content),
                                    Potential = VerbConjugationHelper.GetPotential(keyword, content),
                                    TaForm = VerbConjugationHelper.GetTaForm(keyword, content),
                                    TeForm = VerbConjugationHelper.GetTeForm(keyword, content),
                                    Volitional = VerbConjugationHelper.GetVolitional(keyword, content)
                                });
                            }
                            else
                            {
                                //vm.IsVerbQueryEnabled = Visibility.Collapsed;
                            }
                        }
                        this.VerbResult = vbres;
                    }));
            }
        }

        private RelayCommand _queryKanjiCommand;
        /// <summary>
        /// Gets the QueryKanjiCommand.
        /// </summary>
        public RelayCommand QueryKanjiCommand
        {
            get
            {
                return _queryKanjiCommand
                    ?? (_queryKanjiCommand = new RelayCommand(
                    async() =>
                    {
                        Regex reg = new Regex("[\u4e00-\u9fa5]+");
                        Regex en_reg = new Regex("[A-z\\s]+");
                        StringBuilder sbkey = new StringBuilder();
                        foreach (var i in this.Result)
                        {
                            sbkey.Append(i.JpChar);
                            if (reg.Matches(i.JpChar).Count == 0)
                            {
                                if (i.Explanation != "没有本地释义")
                                {
                                    string[] lines;
                                    if (i.Explanation.Contains("\r"))
                                    {
                                        lines = i.Explanation.Split('\r');
                                    }
                                    else
                                    {
                                        lines = i.Explanation.Split('\n');
                                    }
                                    if (string.IsNullOrWhiteSpace(lines[0]))
                                    {
                                        if (!lines[1].Contains("["))
                                        {
                                            if (!en_reg.IsMatch(lines[1]) && lines[1].Contains(")"))
                                                sbkey.Append(lines[1]);
                                        }
                                    }
                                    else
                                    {
                                        if (!lines[0].Contains("["))
                                        {
                                            if (!en_reg.IsMatch(lines[0]) && lines[0].Contains(")"))
                                                sbkey.Append(lines[0]);
                                        }
                                    }
                                }

                            }
                        }
                        var matches = reg.Matches(sbkey.ToString());
                        this.KanjiResult = await QueryEngine.QueryEngine.KanjiDictQueryEngine.QueryForUIAsync(matches);

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
                            var stream = await speech.GetSpeakStreamAsync(text, language);
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
                        QueryEngine.QueryEngine.UserDefDictQueryEngine.Add(x);
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
                        QueryEngine.QueryEngine.UserDefDictQueryEngine.Remove(x);
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

