using System.Reactive;
using System.Reactive.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using JapaneseDict.Models;
using JapaneseDict.GUI.Extensions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace JapaneseDict.GUI.ViewModels
{
    public class KanjiFlashcardViewModel : ViewModelBase
    {
        private ObservableCollection<Kanjidict> kanji;
        public ObservableCollection<Kanjidict> Kanji
        {
            get { return kanji; }
            set
            {
                kanji = value;
                RaisePropertyChanged();
            }
        }

        private RelayCommand _replayCommand;
        /// <summary>
        /// Gets the ReplayCommand.
        /// </summary>
        public RelayCommand ReplayCommand
        {
            get
            {
                return _replayCommand
                    ?? (_replayCommand = new RelayCommand(
                    () =>
                    {
                        if (this.Kanji.Count != 0)
                        {
                            var res = this.Kanji.ToList();
                            res.Shuffle();
                            this.Kanji = new ObservableCollection<Kanjidict>(res);
                        }
                    }));
            }
        }

        private RelayCommand _showReadingCommand;
        /// <summary>
        /// Gets the ShowReadingCommand.
        /// </summary>
        public RelayCommand ShowReadingCommand
        {
            get
            {
                return _showReadingCommand
                    ?? (_showReadingCommand = new RelayCommand(
                    () =>
                    {
                        var res = new ObservableCollection<Kanjidict>(this.Kanji);
                        foreach (var i in res)
                        {
                            i.ShowReading = Windows.UI.Xaml.Visibility.Visible;
                        }
                        this.Kanji = res;
                    }));
            }
        }

        private RelayCommand _hideReadingCommand;
        /// <summary>
        /// Gets the HideReadingCommand.
        /// </summary>
        public RelayCommand HideReadingCommand
        {
            get
            {
                return _hideReadingCommand
                    ?? (_hideReadingCommand = new RelayCommand(
                    () =>
                    {
                        var res = new ObservableCollection<Kanjidict>(this.Kanji);
                        foreach (var i in res)
                        {
                            i.ShowReading = Windows.UI.Xaml.Visibility.Collapsed;
                        }
                        this.Kanji = res;
                    }));
            }
        }                
    }
}

