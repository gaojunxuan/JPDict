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
using System.Diagnostics;
using Windows.UI.Popups;
using JapaneseDict.GUI.Helpers;
using JapaneseDict.OnlineService.Helpers;

namespace JapaneseDict.GUI.ViewModels
{
    public class TranslationViewModel : ViewModelBase
    {
        public TranslationViewModel()
        {
            SourceLang = "jp";
        }

        private string translationResult;

        public string TranslationResult
        {
            get { return translationResult; }
            set
            {
                translationResult = value;
                RaisePropertyChanged();
            }
        }
        private RelayCommand<string> _translateCommand;

        /// <summary>
        /// Gets the TranslateCommand.
        /// </summary>
        public RelayCommand<string> TranslateCommand
        {
            get
            {
                return _translateCommand
                    ?? (_translateCommand = new RelayCommand<string>(
                    async(x) =>
                    {
                        string sourcelang = SourceLang;
                        if (x != null)
                        {
                            try
                            {
                                bool useTexTra = StorageHelper.GetSetting<bool>("UseTexTra");
                                if (x.ToString().Count() > 2000)
                                {
                                    await new MessageDialog("最多只能翻译 2000 字符的内容，超出部分将被自动去除。", "提示").ShowAsync();
                                    if (sourcelang == "jp")
                                    {
                                        if (useTexTra)
                                        {
                                            TranslationResult = await JsonHelper.GetJpToCnTranslationResult(x.ToString().Substring(0,2000));
                                        }
                                        else
                                        {
                                            TranslationResult = await JsonHelper.GetTranslateResult(x.ToString().Substring(0,2000), "jp", "zh");
                                        }
                                    }
                                    else
                                    {
                                        if (useTexTra)
                                        {
                                            TranslationResult = await JsonHelper.GetCnToJpTranslationResult(x.ToString().Substring(0, 2000));
                                        }
                                        else
                                        {
                                            TranslationResult = await JsonHelper.GetTranslateResult(x.ToString().Substring(0, 2000), "zh", "jp");
                                        }
                                    }
                                }
                                else
                                {
                                    if (sourcelang == "jp")
                                    {
                                        if (useTexTra)
                                        {
                                            TranslationResult = await JsonHelper.GetJpToCnTranslationResult(x.ToString());
                                        }
                                        else
                                        {
                                            TranslationResult = await JsonHelper.GetTranslateResult(x.ToString(), "jp", "zh");
                                        }
                                    }
                                    else
                                    {
                                        if (useTexTra)
                                        {
                                            TranslationResult = await JsonHelper.GetCnToJpTranslationResult(x.ToString());
                                        }
                                        else
                                        {
                                            TranslationResult = await JsonHelper.GetTranslateResult(x.ToString(), "zh", "jp");
                                        }
                                    }
                                }

                            }
                            catch
                            {
                                Debug.WriteLine("error");
                            }
                        }
                        
                    }));
            }
        }
        private RelayCommand _clearResultCommand;

        /// <summary>
        /// Gets the ClearResultCommand.
        /// </summary>
        public RelayCommand ClearResultCommand
        {
            get
            {
                return _clearResultCommand
                    ?? (_clearResultCommand = new RelayCommand(
                    () =>
                    {
                        TranslationResult = "";
                    }));
            }
        }
        private string sourceLang = "jp";
        public string SourceLang
        {
            get
            {
                return sourceLang ?? "jp";
            }
            set
            {
                sourceLang = value;
                RaisePropertyChanged();
            }
        }
              
    }

}

