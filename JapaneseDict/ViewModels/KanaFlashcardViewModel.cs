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
using static JapaneseDict.Util.KanaFlashcardHelper;

namespace JapaneseDict.GUI.ViewModels
{
    public class KanaFlashcardViewModel : ViewModelBase
    {
        private ObservableCollection<Kana> hiragana;
        public ObservableCollection<Kana> Hiragana
        {
            get { return hiragana; }
            set
            {
                hiragana = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Kana> katakana;
        public ObservableCollection<Kana> Katakana
        {
            get { return katakana; }
            set
            {
                katakana = value;
                RaisePropertyChanged();
            }
        }

        private RelayCommand _showVoicedHiraganaCommand;

        /// <summary>
        /// Gets the ShowVoicedHiraganaCommand.
        /// </summary>
        public RelayCommand ShowVoicedHiraganaCommand
        {
            get
            {
                return _showVoicedHiraganaCommand
                    ?? (_showVoicedHiraganaCommand = new RelayCommand(
                    () =>
                    {
                        List<Kana> res = GetRandomHiraganaWithVoicedConsonants().ToList();

                        if (this.Hiragana.Where(k => k.IsHistory == true).Count() == 0)
                        {
                            res.Remove(res.Where(k => k.Content == "ゐ").First());
                            res.Remove(res.Where(k => k.Content == "ゑ").First());
                        }

                        if (this.Hiragana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                            }
                        }
                        this.Hiragana = new ObservableCollection<Kana>(res);
                    }));
            }
        }

        private RelayCommand _hideVoicedHiraganaCommand;

        /// <summary>
        /// Gets the HideVoicedHiraganaCommand.
        /// </summary>
        public RelayCommand HideVoicedHiraganaCommand
        {
            get
            {
                return _hideVoicedHiraganaCommand
                    ?? (_hideVoicedHiraganaCommand = new RelayCommand(
                    () =>
                    {
                        List<Kana> res = GetRandomHiragana().ToList();

                        if (this.Hiragana.Where(k => k.IsHistory == true).Count() == 0)
                        {
                            res.Remove(res.Where(k => k.Content == "ゐ").First());
                            res.Remove(res.Where(k => k.Content == "ゑ").First());
                        }
                        if (this.Hiragana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                            }
                        }
                        this.Hiragana = new ObservableCollection<Kana>(res);
                    }));
            }
        }

        private RelayCommand _showHistoricalHiraganaCommand;

        /// <summary>
        /// Gets the ShowHistoricalHiraganaCommand.
        /// </summary>
        public RelayCommand ShowHistoricalHiraganaCommand
        {
            get
            {
                return _showHistoricalHiraganaCommand
                    ?? (_showHistoricalHiraganaCommand = new RelayCommand(
                    () =>
                    {
                        Random rand = new Random(DateTime.Now.Millisecond);
                        var wyi = new Kana() { Content = "ゐ", Romaji = "wyi", IsHistory = true };
                        var wye = new Kana() { Content = "ゑ", Romaji = "wye", IsHistory = true };
                        if (this.Hiragana.Where(k => k.IsHistory == true).Count() == 0)
                        {
                            this.Hiragana.Insert(rand.Next(0, this.Hiragana.Count - 1), wyi);
                            this.Hiragana.Insert(rand.Next(0, this.Hiragana.Count - 1), wye);
                        }
                        var res = new ObservableCollection<Kana>(this.Hiragana);
                        if (this.Hiragana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                            }
                        }
                        this.Hiragana = res;
                    }));
            }
        }

        private RelayCommand _hideHistoricalHiraganaCommand;

        /// <summary>
        /// Gets the HideHistoricalHiraganaCommand.
        /// </summary>
        public RelayCommand HideHistoricalHiraganaCommand
        {
            get
            {
                return _hideHistoricalHiraganaCommand
                    ?? (_hideHistoricalHiraganaCommand = new RelayCommand(
                    () =>
                    {
                        if (this.Hiragana.Where(k => k.IsHistory == true).Count() != 0)
                        {
                            this.Hiragana.Remove(this.Hiragana.Where(k => k.Content == "ゐ").First());
                            this.Hiragana.Remove(this.Hiragana.Where(k => k.Content == "ゑ").First());
                        }
                    }));
            }
        }

        private RelayCommand _replayHiraganaCommand;

        /// <summary>
        /// Gets the ReplayHiraganaCommand.
        /// </summary>
        public RelayCommand ReplayHiraganaCommand
        {
            get
            {
                return _replayHiraganaCommand
                    ?? (_replayHiraganaCommand = new RelayCommand(
                    () =>
                    {
                        ObservableCollection<Kana> res;
                        if (this.Hiragana.Where(k => k.Content == "ば").Count() != 0)
                        {
                            res = new ObservableCollection<Kana>(GetRandomHiraganaWithVoicedConsonants());
                        }
                        else
                        {
                            res = new ObservableCollection<Kana>(GetRandomHiragana());
                        }

                        if (this.Hiragana.Where(k => k.IsHistory == true).Count() == 0)
                        {
                            res.Remove(res.Where(k => k.Content == "ゐ").First());
                            res.Remove(res.Where(k => k.Content == "ゑ").First());
                        }
                        if (this.Hiragana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                            }
                        }
                        this.Hiragana = res;
                    }));
            }
        }

        private RelayCommand _hideHiraganaRomajiCommand;

        /// <summary>
        /// Gets the HideHiraganaRomajiCommand.
        /// </summary>
        public RelayCommand HideHiraganaRomajiCommand
        {
            get
            {
                return _hideHiraganaRomajiCommand
                    ?? (_hideHiraganaRomajiCommand = new RelayCommand(
                    () =>
                    {
                        ObservableCollection<Kana> res = new ObservableCollection<Kana>(this.Hiragana);
                        foreach (var i in res)
                        {
                            i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                        }
                        this.Hiragana = res;
                    }));
            }
        }

        private RelayCommand _showHiraganaRomajiCommand;

        /// <summary>
        /// Gets the ShowHiraganaRomajiCommand.
        /// </summary>
        public RelayCommand ShowHiraganaRomajiCommand
        {
            get
            {
                return _showHiraganaRomajiCommand
                    ?? (_showHiraganaRomajiCommand = new RelayCommand(
                    () =>
                    {
                        ObservableCollection<Kana> res = new ObservableCollection<Kana>(this.Hiragana);
                        foreach (var i in res)
                        {
                            i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                        }
                        this.Hiragana = res;
                    }));
            }
        }

        private RelayCommand _getOrderedHiraganaCommand;

        /// <summary>
        /// Gets the GetOrderedHiraganaCommand.
        /// </summary>
        public RelayCommand GetOrderedHiraganaCommand
        {
            get
            {
                return _getOrderedHiraganaCommand
                    ?? (_getOrderedHiraganaCommand = new RelayCommand(
                    () =>
                    {
                        ObservableCollection<Kana> res;
                        res = new ObservableCollection<Kana>(GetOrderHiraganaWithVoicedConsonants());
                        res.Remove(res.Where(k => k.Content == "ゐ").First());
                        res.Remove(res.Where(k => k.Content == "ゑ").First());

                        if (this.Hiragana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                            }
                        }
                        this.Hiragana = res;
                    }));
            }
        }

        private RelayCommand _getDisorderedHiraganaCommand;

        /// <summary>
        /// Gets the GetDisorderedHiraganaCommand.
        /// </summary>
        public RelayCommand GetDisorderedHiraganaCommand
        {
            get
            {
                return _getDisorderedHiraganaCommand
                    ?? (_getDisorderedHiraganaCommand = new RelayCommand(
                    () =>
                    {
                        var res = new ObservableCollection<Kana>(GetRandomHiragana());
                        if (this.Hiragana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                            }
                        }
                        this.Hiragana = res;
                    }));
            }
        }

        private RelayCommand _showVoicedKatakanaCommand;

        /// <summary>
        /// Gets the ShowVoicedKatakanaCommand.
        /// </summary>
        public RelayCommand ShowVoicedKatakanaCommand
        {
            get
            {
                return _showVoicedKatakanaCommand
                    ?? (_showVoicedKatakanaCommand = new RelayCommand(
                    () =>
                    {
                        List<Kana> res = GetRandomKatakanaWithVoicedConsonants().ToList();

                        if (this.Katakana.Where(k => k.IsHistory == true).Count() == 0)
                        {
                            res.Remove(res.Where(k => k.Content == "ゐ").First());
                            res.Remove(res.Where(k => k.Content == "ゑ").First());
                        }

                        if (this.Katakana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                            }
                        }
                        this.Katakana = new ObservableCollection<Kana>(res);
                    }));
            }
        }

        private RelayCommand _hideVoicedKatakanaCommand;

        /// <summary>
        /// Gets the HideVoicedKatakanaCommand.
        /// </summary>
        public RelayCommand HideVoicedKatakanaCommand
        {
            get
            {
                return _hideVoicedKatakanaCommand
                    ?? (_hideVoicedKatakanaCommand = new RelayCommand(
                    () =>
                    {
                        List<Kana> res = GetRandomKatakana().ToList();

                        if (this.Katakana.Where(k => k.IsHistory == true).Count() == 0)
                        {
                            res.Remove(res.Where(k => k.Content == "ゐ").First());
                            res.Remove(res.Where(k => k.Content == "ゑ").First());
                        }
                        if (this.Katakana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                            }
                        }
                        this.Katakana = new ObservableCollection<Kana>(res);
                    }));
            }
        }

        private RelayCommand _showHistoricalKatakanaCommand;

        /// <summary>
        /// Gets the ShowHistoricalKatakanaCommand.
        /// </summary>
        public RelayCommand ShowHistoricalKatakanaCommand
        {
            get
            {
                return _showHistoricalKatakanaCommand
                    ?? (_showHistoricalKatakanaCommand = new RelayCommand(
                    () =>
                    {
                        Random rand = new Random(DateTime.Now.Millisecond);
                        var wyi = new Kana() { Content = "ゐ", Romaji = "wyi", IsHistory = true };
                        var wye = new Kana() { Content = "ゑ", Romaji = "wye", IsHistory = true };
                        if (this.Katakana.Where(k => k.IsHistory == true).Count() == 0)
                        {
                            this.Katakana.Insert(rand.Next(0, this.Katakana.Count - 1), wyi);
                            this.Katakana.Insert(rand.Next(0, this.Katakana.Count - 1), wye);
                        }
                        var res = new ObservableCollection<Kana>(this.Katakana);
                        if (this.Katakana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                            }
                        }
                        this.Katakana = res;
                    }));
            }
        }

        private RelayCommand _hideHistoricalKatakanaCommand;

        /// <summary>
        /// Gets the HideHistoricalKatakanaCommand.
        /// </summary>
        public RelayCommand HideHistoricalKatakanaCommand
        {
            get
            {
                return _hideHistoricalKatakanaCommand
                    ?? (_hideHistoricalKatakanaCommand = new RelayCommand(
                    () =>
                    {
                        if (this.Katakana.Where(k => k.IsHistory == true).Count() != 0)
                        {
                            this.Katakana.Remove(this.Katakana.Where(k => k.Content == "ゐ").First());
                            this.Katakana.Remove(this.Katakana.Where(k => k.Content == "ゑ").First());
                        }
                    }));
            }
        }

        private RelayCommand _replayKatakanaCommand;

        /// <summary>
        /// Gets the ReplayKatakanaCommand.
        /// </summary>
        public RelayCommand ReplayKatakanaCommand
        {
            get
            {
                return _replayKatakanaCommand
                    ?? (_replayKatakanaCommand = new RelayCommand(
                    () =>
                    {
                        ObservableCollection<Kana> res;
                        if (this.Katakana.Where(k => k.Content == "ば").Count() != 0)
                        {
                            res = new ObservableCollection<Kana>(GetRandomKatakanaWithVoicedConsonants());
                        }
                        else
                        {
                            res = new ObservableCollection<Kana>(GetRandomKatakana());
                        }

                        if (this.Katakana.Where(k => k.IsHistory == true).Count() == 0)
                        {
                            res.Remove(res.Where(k => k.Content == "ゐ").First());
                            res.Remove(res.Where(k => k.Content == "ゑ").First());
                        }
                        if (this.Katakana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                            }
                        }
                        this.Katakana = res;
                    }));
            }
        }

        private RelayCommand _hideKatakanaRomajiCommand;

        /// <summary>
        /// Gets the HideKatakanaRomajiCommand.
        /// </summary>
        public RelayCommand HideKatakanaRomajiCommand
        {
            get
            {
                return _hideKatakanaRomajiCommand
                    ?? (_hideKatakanaRomajiCommand = new RelayCommand(
                    () =>
                    {
                        ObservableCollection<Kana> res = new ObservableCollection<Kana>(this.Katakana);
                        foreach (var i in res)
                        {
                            i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                        }
                        this.Katakana = res;
                    }));
            }
        }

        private RelayCommand _showKatakanaRomajiCommand;

        /// <summary>
        /// Gets the ShowKatakanaRomajiCommand.
        /// </summary>
        public RelayCommand ShowKatakanaRomajiCommand
        {
            get
            {
                return _showKatakanaRomajiCommand
                    ?? (_showKatakanaRomajiCommand = new RelayCommand(
                    () =>
                    {
                        ObservableCollection<Kana> res = new ObservableCollection<Kana>(this.Katakana);
                        foreach (var i in res)
                        {
                            i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                        }
                        this.Katakana = res;
                    }));
            }
        }

        private RelayCommand _getOrderedKatakanaCommand;

        /// <summary>
        /// Gets the GetOrderedKatakanaCommand.
        /// </summary>
        public RelayCommand GetOrderedKatakanaCommand
        {
            get
            {
                return _getOrderedKatakanaCommand
                    ?? (_getOrderedKatakanaCommand = new RelayCommand(
                    () =>
                    {
                        ObservableCollection<Kana> res;
                        res = new ObservableCollection<Kana>(GetOrderKatakanaWithVoicedConsonants());
                        res.Remove(res.Where(k => k.Content == "ゐ").First());
                        res.Remove(res.Where(k => k.Content == "ゑ").First());

                        if (this.Katakana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                            }
                        }
                        this.Katakana = res;
                    }));
            }
        }

        private RelayCommand _getDisorderedKatakanaCommand;

        /// <summary>
        /// Gets the GetDisorderedKatakanaCommand.
        /// </summary>
        public RelayCommand GetDisorderedKatakanaCommand
        {
            get
            {
                return _getDisorderedKatakanaCommand
                    ?? (_getDisorderedKatakanaCommand = new RelayCommand(
                    () =>
                    {
                        var res = new ObservableCollection<Kana>(GetRandomKatakana());
                        if (this.Katakana.Where(k => k.ShowRomaji == Windows.UI.Xaml.Visibility.Collapsed).Count() != 0)
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            foreach (var i in res)
                            {
                                i.ShowRomaji = Windows.UI.Xaml.Visibility.Visible;
                            }
                        }
                        this.Katakana = res;
                    }));
            }
        }

    }

}

